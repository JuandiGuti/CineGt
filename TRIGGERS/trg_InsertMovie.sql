USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_InsertMovie
ON Movie
AFTER INSERT
AS
BEGIN
    DECLARE @MovieName VARCHAR(100);
    DECLARE @Clasification VARCHAR(20);
    
    -- Obtener el MovieName y Clasification del registro insertado
    SELECT @MovieName = MovieName, @Clasification = Clasification FROM inserted;
    
    -- Insertar el log con el nombre de la película y su clasificación en la descripción
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (SYSTEM_USER, GETDATE(), 'Se insertó una nueva película: ' + @MovieName + ' con clasificación: ' + @Clasification, 'INSERT');
END;
