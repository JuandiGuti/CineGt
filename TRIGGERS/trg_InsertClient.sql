USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_InsertClient
ON Client
AFTER INSERT
AS
BEGIN
    DECLARE @ClientName VARCHAR(100);
    DECLARE @Email VARCHAR(100);
    
    -- Obtener el nombre y el correo del cliente insertado
    SELECT @ClientName = ClientName, @Email = Email FROM inserted;
    
    -- Insertar un registro en TransactionLog con los detalles del cliente
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (
        SYSTEM_USER, 
        GETDATE(), 
        'Se insertó un nuevo cliente: ' + @ClientName + ' con email: ' + @Email, 
        'INSERT'
    );
END;
