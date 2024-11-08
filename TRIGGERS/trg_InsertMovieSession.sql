USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_InsertMovieSession
ON MovieSession
AFTER INSERT
AS
BEGIN
    DECLARE @MovieID INT;
    DECLARE @RoomID INT;
    DECLARE @AppUser VARCHAR(100);
    DECLARE @BeginningDate DATETIME;
    DECLARE @EndingDate DATETIME;
    
    -- Obtener los valores del registro insertado
    SELECT 
        @MovieID = Movie,
        @RoomID = Room,
        @AppUser = AppUser,
        @BeginningDate = BeginningDate,
        @EndingDate = EndingDate
    FROM inserted;
    
    -- Insertar el log con los detalles de la nueva sesión
    INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
    VALUES (
        SYSTEM_USER, 
        GETDATE(), 
        'Se insertó una nueva sesión de película para la MovieID: ' + CAST(@MovieID AS VARCHAR(10)) +
        ', en la sala RoomID: ' + CAST(@RoomID AS VARCHAR(10)) + 
        ', creada por el usuario: ' + @AppUser + 
        ', que inicia en: ' + CAST(@BeginningDate AS VARCHAR) + 
        ' y termina en: ' + CAST(@EndingDate AS VARCHAR), 
        'INSERT'
    );
END;
