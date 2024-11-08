USE BuenaCineGt
GO
CREATE OR ALTER TRIGGER trg_InsertSeat
ON Seat
AFTER INSERT
AS
BEGIN
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (SYSTEM_USER, GETDATE(), 'Se insert� un nuevos asientos en la tabla Seat: 10', 'INSERT');
END;
