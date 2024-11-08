USE BuenaCineGt;
GO

-- SP Crear nueva movie session sin transacciones
CREATE OR ALTER PROCEDURE addSession_NoTransaction 
    (@BEGININGDATE DATETIME, @MOVIENAME VARCHAR(100), @ROOM# INT, @APPUSER VARCHAR(100))
AS
BEGIN
    BEGIN TRY
        -- Verificaci�n de fecha
        IF @BEGININGDATE <= GETDATE() 
        BEGIN
            RAISERROR('Cannot create a movie session in the past.', 16, 1);
            RETURN;
        END;

        DECLARE @VERIFICATION INT = 0;

        -- Verificaci�n de que la pel�cula existe
        SELECT @VERIFICATION = COUNT(*) FROM Movie WHERE MovieName = @MOVIENAME;
        IF @VERIFICATION = 0 
        BEGIN
            RAISERROR('The movie does not exist.', 16, 1);
            RETURN;
        END;

        -- Verificaci�n de que la sala existe
        SET @VERIFICATION = 0;
        SELECT @VERIFICATION = COUNT(*) FROM Room WHERE ID = @ROOM#;
        IF @VERIFICATION = 0 
        BEGIN
            RAISERROR('The room does not exist.', 16, 1);
            RETURN;
        END;

        -- Verificaci�n de que el usuario existe
        SET @VERIFICATION = 0;
        SELECT @VERIFICATION = COUNT(*) FROM AppUser WHERE Username = @APPUSER;
        IF @VERIFICATION = 0 
        BEGIN
            RAISERROR('The user does not exist.', 16, 1);
            RETURN;
        END;

        DECLARE @MOVIEID INT, @MOVIEDURATION TIME(7), @ENDINGDATE DATETIME, @MINSUM INT;

        -- Obtener el ID y la duraci�n de la pel�cula
        SELECT @MOVIEID = ID, @MOVIEDURATION = MovieDuration FROM Movie WHERE MovieName = @MOVIENAME;

        -- Calcular el tiempo de finalizaci�n de la sesi�n
        SET @MINSUM = DATEDIFF(MINUTE, 0, @MOVIEDURATION) + DATEDIFF(MINUTE, 0, '00:15:00');
        SET @ENDINGDATE = DATEADD(MINUTE, @MINSUM, @BEGININGDATE);

        -- Verificaci�n de solapamiento de sesiones en la misma sala
        SET @VERIFICATION = 0;
        SELECT @VERIFICATION = COUNT(*) FROM (
            SELECT * FROM MovieSession 
            WHERE Room = @ROOM# 
                AND ((BeginningDate <= @BEGININGDATE AND @BEGININGDATE <= EndingDate)
                OR (BeginningDate <= @ENDINGDATE AND @ENDINGDATE <= EndingDate))
        ) AS A
        WHERE A.SessionState = 0 OR A.SessionState = 1;

        IF @VERIFICATION > 0 
        BEGIN
            RAISERROR('Cannot create the movie session at this date in the room: %d', 16, 1, @ROOM#);
            RETURN;
        END;

        -- Inserci�n de la nueva sesi�n en la tabla MovieSession
        INSERT INTO MovieSession (SessionState, BeginningDate, EndingDate, CompromisedSeats, Movie, Room, AppUser)
        VALUES (0, @BEGININGDATE, @ENDINGDATE, 0, @MOVIEID, @ROOM#, @APPUSER);

        -- Obtener el ID de la sesi�n reci�n creada
        DECLARE @IDMOVIESESSION INT;
        SET @IDMOVIESESSION = SCOPE_IDENTITY();

        -- Crear asientos para la sesi�n
        DECLARE CURSOR_Seat CURSOR FOR
        SELECT ID FROM Seat WHERE Room = @ROOM#;

        OPEN CURSOR_Seat;

        DECLARE @SEATID VARCHAR(25);
        FETCH NEXT FROM CURSOR_Seat INTO @SEATID;

        WHILE @@FETCH_STATUS = 0 
        BEGIN
            INSERT INTO SeatByMovieSession (Available, MovieSession, Seat)
            VALUES (1, @IDMOVIESESSION, @SEATID);

            FETCH NEXT FROM CURSOR_Seat INTO @SEATID;
        END;

        CLOSE CURSOR_Seat;
        DEALLOCATE CURSOR_Seat;
    END TRY
    BEGIN CATCH
        -- Manejo de errores sin ROLLBACK ni COMMIT
        IF CURSOR_STATUS('global', 'CURSOR_Seat') >= 0 
        BEGIN
            CLOSE CURSOR_Seat;
            DEALLOCATE CURSOR_Seat;
        END;

        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;
GO
