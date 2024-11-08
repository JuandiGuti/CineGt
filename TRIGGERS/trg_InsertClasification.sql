USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_InsertClasification
ON Clasification
AFTER INSERT
AS
BEGIN
    DECLARE @ClasificationName VARCHAR(20);
    
    -- Obtener el nombre de la Clasification del registro insertado
    SELECT @ClasificationName = Clasification FROM inserted;
    
    -- Insertar el log con el nombre de la clasificación en la descripción
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (SYSTEM_USER, GETDATE(), 'Se insertó una nueva clasificación: ' + @ClasificationName, 'INSERT');
END;
