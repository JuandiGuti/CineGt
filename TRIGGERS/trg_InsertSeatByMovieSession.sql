USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_InsertSeatByMovieSession
ON SeatByMovieSession
AFTER INSERT
AS
BEGIN
    DECLARE @ID INT;
    DECLARE @Available INT;
    DECLARE @MovieSession INT;
    DECLARE @Seat INT;
    DECLARE @TicketsTransaction INT;

    -- Obtener los valores del registro insertado
    SELECT 
        @ID = ID,
        @Available = Available,
        @MovieSession = MovieSession,
        @Seat = Seat,
        @TicketsTransaction = TicketsTransaction
    FROM inserted;
    
    -- Insertar el log con los detalles del nuevo asiento en la sesión de película
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (
        SYSTEM_USER, 
        GETDATE(), 
        'Se insertó un nuevo asiento en la sesión de película: ' + CAST(@MovieSession AS VARCHAR(10)) +
        ', asiento ID: ' + CAST(@Seat AS VARCHAR(10)) +
        ', estado de disponibilidad: ' + CAST(@Available AS VARCHAR(10)) +
        ', asignado a la transacción de tickets: ' + COALESCE(CAST(@TicketsTransaction AS VARCHAR(10)), 'N/A'), 
        'INSERT'
    );
END;
