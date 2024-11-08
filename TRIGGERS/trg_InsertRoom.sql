USE BuenaCineGt
GO
CREATE OR ALTER TRIGGER trg_InsertRoom
ON Room
AFTER INSERT
AS
BEGIN
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (SYSTEM_USER, GETDATE(), 'Se insertó una nueva sala en la tabla Room', 'INSERT');
END;
