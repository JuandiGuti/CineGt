USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_UpdateTicketsTransaction
ON TicketsTransaction
AFTER UPDATE
AS
BEGIN
    DECLARE @ID INT;
    DECLARE @TransactionStatus INT;
    DECLARE @ModificationDate DATETIME;
    DECLARE @AppUser VARCHAR(100);
    DECLARE @Payment DECIMAL(10, 2);
    
    -- Obtener los valores del registro actualizado
    SELECT 
        @ID = ID,
        @TransactionStatus = TransactionStatus,
        @ModificationDate = ModificationDate,
        @AppUser = AppUser,
        @Payment = Payment
    FROM inserted;
    
	IF (@ModificationDate IS NULL) BEGIN
		SET @ModificationDate = '9999-12-31 23:59:59.997';
	END

    -- Insertar el log con los detalles de la transacción actualizada
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (
        SYSTEM_USER, 
        GETDATE(), 
        'Se actualizó la transacción de boletos con ID: ' + CAST(@ID AS VARCHAR(10)) +
        ', nuevo estado de transacción: ' + CAST(@TransactionStatus AS VARCHAR(10)) +
        ', modificada por el usuario: ' + @AppUser + 
        ', con pago de: $' + CAST(@Payment AS VARCHAR(10)) + 
        ', en la fecha de modificación: ' + CAST(@ModificationDate AS VARCHAR), 
        'UPDATE'
    );
END;
