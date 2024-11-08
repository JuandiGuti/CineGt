USE BuenaCineGt;
GO

CREATE OR ALTER PROCEDURE changeSeat
(
    @TICKETSTRANSACTIONID INT,
    @MOVIESSESIONID INT,
    @ACTUALSEAT VARCHAR(3),
    @NEWSEAT VARCHAR(3),
    @ROOM INT
)
AS 
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DECLARE @NEWSEATAVAILABLE INT;
        DECLARE @ACTUALSEATID INT;
        DECLARE @NEWSEATID INT;

        -- Obtener el ID del asiento actual basado en su nombre y sala
        SELECT @ACTUALSEATID = S.ID
        FROM Seat S
        WHERE S.seat = @ACTUALSEAT 
          AND S.Room = @ROOM;

        -- Obtener el ID del nuevo asiento basado en su nombre y sala
        SELECT @NEWSEATID = S.ID
        FROM Seat S
        WHERE S.seat = @NEWSEAT 
          AND S.Room = @ROOM;

        -- Verificar si el nuevo asiento está disponible en la sesión de película
        SELECT @NEWSEATAVAILABLE = MAX(Available) 
        FROM SeatByMovieSession
        WHERE MovieSession = @MOVIESSESIONID
        AND Seat = @NEWSEATID;

        IF @NEWSEATAVAILABLE = 0 
        BEGIN
            RAISERROR('The new seat is not available.', 16, 1);
            RETURN;
        END

        -- Liberar el asiento actual
        UPDATE SeatByMovieSession
        SET Available = 1, TicketsTransaction = NULL
        WHERE MovieSession = @MOVIESSESIONID
        AND Seat = @ACTUALSEATID
        AND TicketsTransaction = @TICKETSTRANSACTIONID;

        -- Asignar el nuevo asiento a la transacción de tickets
        UPDATE SeatByMovieSession
        SET Available = 0, TicketsTransaction = @TICKETSTRANSACTIONID
        WHERE MovieSession = @MOVIESSESIONID
        AND Seat = @NEWSEATID;

        -- Actualizar la fecha de modificación en TicketsTransaction
        UPDATE TicketsTransaction
        SET ModificationDate = GETDATE()
        WHERE ID = @TICKETSTRANSACTIONID;

        COMMIT;
    END TRY
    BEGIN CATCH
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
