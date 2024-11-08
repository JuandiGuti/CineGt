USE BuenaCineGt;
GO
CREATE OR ALTER PROCEDURE ByClientSearchSeats(@EMAIL VARCHAR(50))
AS 
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY

        DECLARE @VERIFICATION INT;

        -- Verificar si el cliente existe
        SELECT @VERIFICATION = COUNT(*) 
        FROM Client
        WHERE Email = @EMAIL;

        IF @VERIFICATION = 0 
        BEGIN
            RAISERROR('The client does not exist.', 16, 1);
            RETURN;
        END

        SET @VERIFICATION = 0;

        -- Verificar si el cliente ha comprado asientos
        SELECT @VERIFICATION = COUNT(*) 
        FROM TicketsTransaction TT
        JOIN Client CL ON TT.Client = CL.ID
        JOIN SeatByMovieSession SMS ON TT.ID = SMS.TicketsTransaction
        JOIN MovieSession MS ON SMS.MovieSession = MS.ID
        JOIN Movie M ON MS.Movie = M.ID
        WHERE TT.TransactionStatus = 1
          AND CL.Email = @EMAIL
          AND MS.BeginningDate > GETDATE();

        IF @VERIFICATION = 0 
        BEGIN
            RAISERROR('The client has not bought any seats.', 16, 1);
            RETURN;
        END

        -- Seleccionar los detalles de la transacción, incluyendo el nombre del asiento
        SELECT 
            TT.ID AS TransactionId, 
            MS.ID AS MovieSessionId,
            TT.CreateDate, 
            TT.ModificationDate, 
            S.seat AS Seat,  -- Obtener el nombre del asiento desde la tabla Seat
            M.MovieName,
			MS.Room
        FROM TicketsTransaction TT
        JOIN Client CL ON TT.Client = CL.ID
        JOIN SeatByMovieSession SMS ON TT.ID = SMS.TicketsTransaction
        JOIN MovieSession MS ON SMS.MovieSession = MS.ID
        JOIN Movie M ON MS.Movie = M.ID
        JOIN Seat S ON SMS.Seat = S.ID  -- Unir con la tabla Seat para obtener el nombre
        WHERE TT.TransactionStatus = 1
          AND CL.Email = @EMAIL
          AND MS.BeginningDate > GETDATE()
        ORDER BY M.MovieName ASC;

        COMMIT;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 
        BEGIN
            ROLLBACK TRANSACTION; -- TERMINAR LA TRANSACCIÓN
        END
        DECLARE @MENSAJE NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @SEVERIDAD INT = ERROR_SEVERITY();
        DECLARE @ESTADO INT = ERROR_STATE();
        RAISERROR(@MENSAJE, @SEVERIDAD, @ESTADO);
    END CATCH
END;

EXEC ByClientSearchSeats 'juan@gmail.com';