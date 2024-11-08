USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_InsertAppUser
ON AppUser
AFTER INSERT
AS
BEGIN
    DECLARE @UsernameInserted VARCHAR(100);
    
    -- Obtener el Username del registro insertado
    SELECT @UsernameInserted = Username FROM inserted;
    
    -- Insertar el log con el nombre de usuario incluido en la descripción
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (SYSTEM_USER, GETDATE(), 'Se insertó un nuevo usuario: ' + @UsernameInserted, 'INSERT');
END;
