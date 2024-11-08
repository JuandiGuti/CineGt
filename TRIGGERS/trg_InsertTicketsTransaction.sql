USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_InsertTicketsTransaction
ON TicketsTransaction
AFTER INSERT
AS
BEGIN
    DECLARE @NumberSeats INT;
    DECLARE @Payment DECIMAL(10, 2);
    DECLARE @AppUser VARCHAR(100);
    DECLARE @CreateDate DATETIME;
    
    -- Obtener los valores del registro insertado
    SELECT 
        @NumberSeats = NumberSeats,
        @Payment = Payment,
        @AppUser = AppUser,
        @CreateDate = CreateDate
    FROM inserted;
    
    -- Insertar el log con los detalles de la nueva transacción
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (
        SYSTEM_USER, 
        GETDATE(), 
        'Se insertó una nueva transacción de boletos: ' + CAST(@NumberSeats AS VARCHAR(10)) + 
        ' asientos, con un pago de: $' + CAST(@Payment AS VARCHAR(10)) + 
        ', creada por el usuario: ' + @AppUser + 
        ', en la fecha: ' + CAST(@CreateDate AS VARCHAR), 
        'INSERT'
    );
END;
