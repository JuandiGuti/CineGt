USE BuenaCineGt;
GO

CREATE OR ALTER PROCEDURE ProcessSessionsFromCSV
    @CSVPath NVARCHAR(255),
    @CommitOnErrors INT -- 0: Rollback on errors; 1: Commit partial inserts
AS
BEGIN
    -- Crear la tabla temporal
    CREATE TABLE #TempSessions (
        BEGININGDATE DATETIME,
        MOVIENAME VARCHAR(100),
        ROOM# INT,
        APPUSER VARCHAR(100)
    );

    -- Comando BULK INSERT usando SQL dinámico
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = N'BULK INSERT #TempSessions FROM ''' + @CSVPath + ''' WITH (
                    FIELDTERMINATOR = '','',  -- Delimitador de campo en el CSV
                    ROWTERMINATOR = ''\n'',   -- Terminador de fila en el CSV
                    FIRSTROW = 2              -- Saltar encabezado si existe
                );';

    BEGIN TRY
        -- Ejecutar BULK INSERT usando SQL dinámico
        EXEC sp_executesql @sql;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR('Error al cargar el archivo CSV: %s', 16, 1, @ErrorMessage);
        RETURN;
    END CATCH;

    DECLARE @ErrorCount INT = 0; -- Contador de errores

    -- Iniciar la transacción
    BEGIN TRANSACTION;

    DECLARE SessionCursor CURSOR FOR
        SELECT BEGININGDATE, MOVIENAME, ROOM#, APPUSER FROM #TempSessions;

    OPEN SessionCursor;

    DECLARE @BEGININGDATE DATETIME, @MOVIENAME VARCHAR(100), @ROOM# INT, @APPUSER VARCHAR(100);
    FETCH NEXT FROM SessionCursor INTO @BEGININGDATE, @MOVIENAME, @ROOM#, @APPUSER;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        BEGIN TRY
            -- Llamada al procedimiento para insertar sesión
            EXEC addSession_NoTransaction @BEGININGDATE, @MOVIENAME, @ROOM#, @APPUSER;

        END TRY
        BEGIN CATCH
            -- Incrementar el contador de errores si ocurre uno
            SET @ErrorCount = @ErrorCount + 1;
        END CATCH;

        FETCH NEXT FROM SessionCursor INTO @BEGININGDATE, @MOVIENAME, @ROOM#, @APPUSER;
    END;

    CLOSE SessionCursor;
    DEALLOCATE SessionCursor;

    -- Verificar errores y decidir si hacer COMMIT o ROLLBACK
    IF @ErrorCount > 0
    BEGIN
        IF @CommitOnErrors = 1
        BEGIN
            IF @@TRANCOUNT > 0
                COMMIT TRANSACTION;
        END
        ELSE
        BEGIN
            IF @@TRANCOUNT > 0
                ROLLBACK TRANSACTION;
        END
    END
    ELSE
    BEGIN
        IF @@TRANCOUNT > 0
            COMMIT TRANSACTION;
    END;

    -- Borrar la tabla temporal
    DROP TABLE #TempSessions;
END;
GO
