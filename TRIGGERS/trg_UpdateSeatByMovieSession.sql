USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_UpdateSeatByMovieSession
ON SeatByMovieSession
AFTER UPDATE
AS
BEGIN
    DECLARE @ID INT;
    DECLARE @Available INT;
    DECLARE @MovieSession INT;
    DECLARE @Seat INT;
    DECLARE @TicketsTransaction INT;

    -- Obtener los valores del registro actualizado
    SELECT 
        @ID = ID,
        @Available = Available,
        @MovieSession = MovieSession,
        @Seat = Seat,
        @TicketsTransaction = TicketsTransaction
    FROM inserted;
    
    -- Insertar el log con los detalles del asiento actualizado en la sesión de película
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (
        SYSTEM_USER, 
        GETDATE(), 
        'Se actualizó el asiento en la sesión de película: ' + COALESCE(CAST(@MovieSession AS VARCHAR(10)), 'N/A') +
        ', asiento ID: ' + COALESCE(CAST(@Seat AS VARCHAR(10)), 'N/A') +
        ', nuevo estado de disponibilidad: ' + COALESCE(CAST(@Available AS VARCHAR(10)), 'N/A') +
        ', asignado a la transacción de tickets: ' + COALESCE(CAST(@TicketsTransaction AS VARCHAR(10)), 'N/A'), 
        'UPDATE'
    );
END;
