USE BuenaCineGt;
GO

CREATE OR ALTER TRIGGER trg_UpdateMovieSession
ON MovieSession
AFTER UPDATE
AS
BEGIN
    DECLARE @ID INT;
    DECLARE @SessionState INT;
    DECLARE @CompromisedSeats INT;
    DECLARE @AppUser VARCHAR(100);
    DECLARE @BeginningDate DATETIME;
    DECLARE @EndingDate DATETIME;

    -- Verificar si realmente hubo cambios
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN deleted d ON i.ID = d.ID
        WHERE i.SessionState <> d.SessionState
           OR i.CompromisedSeats <> d.CompromisedSeats
           OR i.AppUser <> d.AppUser
           OR i.BeginningDate <> d.BeginningDate
           OR i.EndingDate <> d.EndingDate
    )
    BEGIN
        -- Obtener los valores del registro actualizado
        SELECT 
            @ID = ID,
            @SessionState = SessionState,
            @CompromisedSeats = CompromisedSeats,
            @AppUser = AppUser,
            @BeginningDate = BeginningDate,
            @EndingDate = EndingDate
        FROM inserted;

        -- Insertar el log con los detalles de la sesión actualizada
        INSERT INTO TransactionLog (SqlUser, DateCreated, DescriptionLog, LogType)
        VALUES (
            SYSTEM_USER, 
            GETDATE(), 
            'Se actualizó la sesión de película con ID: ' + CAST(@ID AS VARCHAR(10)) +
            ', nuevo estado de sesión: ' + CAST(@SessionState AS VARCHAR(10)) +
            ', asientos comprometidos: ' + CAST(@CompromisedSeats AS VARCHAR(10)) +
            ', modificado por el usuario: ' + @AppUser + 
            ', con fecha de inicio: ' + CAST(@BeginningDate AS VARCHAR) + 
            ' y fecha de finalización: ' + CAST(@EndingDate AS VARCHAR), 
            'UPDATE'
        );
    END
END;
