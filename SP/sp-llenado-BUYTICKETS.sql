USE BuenaCineGt;
GO

CREATE OR ALTER PROCEDURE confirmTicket(@json NVARCHAR(MAX))
AS 
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @TICKETSTRANSACTION INT;
        DECLARE @MOVIESESSION INT;
        DECLARE @SEATNAME VARCHAR(3); -- El nombre del asiento como 'A1'
        DECLARE @ROOM INT;            -- El ID de la sala
        DECLARE @SEATID INT;          -- El ID del asiento correspondiente
		DECLARE @PAYMENT DECIMAL(10,2);
		DECLARE @BALANCE DECIMAL(10,2);

        -- Creamos el cursor
        DECLARE json_cursor CURSOR FOR
        SELECT TicketsTransaction, MovieSession, Seat, Room
        FROM OPENJSON(@json)
        WITH (TicketsTransaction INT, MovieSession INT, Seat VARCHAR(3), Room INT);

        -- Abrimos el cursor
        OPEN json_cursor;

        -- Recuperamos la primera fila
        FETCH NEXT FROM json_cursor INTO @TICKETSTRANSACTION, @MOVIESESSION, @SEATNAME, @ROOM;

        -- Iteramos a través de todas las filas del cursor
        WHILE @@FETCH_STATUS = 0
        BEGIN
            DECLARE @SeatByMovieSessionID INT, @VERIFICATION INT;

            -- Obtener el ID del asiento desde la tabla Seat basado en el nombre del asiento y la sala
            SELECT @SEATID = ID 
            FROM Seat 
            WHERE seat = @SEATNAME AND Room = @ROOM;

            -- Verificar si el asiento ya está tomado en la sesión de película
            SELECT @VERIFICATION = COUNT(*) 
            FROM SeatByMovieSession
            WHERE MovieSession = @MOVIESESSION
            AND Seat = @SEATID
            AND Available = 0;

            IF @VERIFICATION > 0 
            BEGIN
                RAISERROR('The seat: %s in room: %d is already taken in the movie session: %d.', 16, 1, @SEATNAME, @ROOM, @MOVIESESSION);
                RETURN;
            END

            -- Obtener el ID de SeatByMovieSession para el asiento y sesión
            SELECT @SeatByMovieSessionID = MAX(ID) 
            FROM SeatByMovieSession
            WHERE MovieSession = @MOVIESESSION
            AND Seat = @SEATID
            AND Available = 1;

            -- Actualizar el estado de disponibilidad y asignar la transacción de tickets
            UPDATE SeatByMovieSession
            SET Available = 0, TicketsTransaction = @TICKETSTRANSACTION
            WHERE MovieSession = @MOVIESESSION
            AND Seat = @SEATID;

            -- Recuperamos la siguiente fila
            FETCH NEXT FROM json_cursor INTO @TICKETSTRANSACTION, @MOVIESESSION, @SEATNAME, @ROOM;
        END;

        -- Cerramos y liberamos el cursor
        CLOSE json_cursor;
        DEALLOCATE json_cursor;

		DECLARE @IDBALANCE INT;

		SELECT @IDBALANCE = MAX(ID) FROM BalanceLog

		SELECT @BALANCE = Balance FROM BalanceLog WHERE ID = @IDBALANCE

		SELECT @PAYMENT = Payment FROM TicketsTransaction WHERE ID = @TICKETSTRANSACTION;

		SET @BALANCE = @BALANCE + @PAYMENT;

		SET @IDBALANCE = @IDBALANCE + 1

		INSERT INTO BalanceLog (Balance, ModificationDatetime, BalanceType, TicketsTransaction)
		VALUES (@BALANCE, GETDATE(), 1, @TICKETSTRANSACTION);
        COMMIT;
    END TRY
    BEGIN CATCH
        CLOSE json_cursor;
        DEALLOCATE json_cursor;
        IF @@TRANCOUNT > 0 
        BEGIN
            ROLLBACK TRANSACTION; -- TERMINAR LA TRANSACCIÓN
        END
        DECLARE @MENSAJE NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @SEVERIDAD INT = ERROR_SEVERITY();
        DECLARE @ESTADO INT = ERROR_STATE();
        RAISERROR(@MENSAJE, @SEVERIDAD, @ESTADO);
    END CATCH
END;

EXEC confirmTicket '[{"TicketsTransaction":11,"MovieSession":6,"Seat":"C2","Room":3},{"TicketsTransaction":11,"MovieSession":6,"Seat":"C3","Room":3},{"TicketsTransaction":11,"MovieSession":6,"Seat":"C4","Room":3}]'