USE BuenaCineGt;
GO
CREATE OR ALTER PROCEDURE ClientByTransaction(@EMAIL VARCHAR(50))
AS BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY

		DECLARE @VERIFICATION INT;
		SELECT @VERIFICATION = COUNT(*) FROM Client
		WHERE Email = @EMAIL

		IF @VERIFICATION = 0 BEGIN
			RAISERROR('The client does not exist.', 16, 1);
			RETURN;
		END

		SET @VERIFICATION =0;

		SELECT @VERIFICATION = COUNT(*)
		FROM TicketsTransaction TT, SeatByMovieSession SMS, MovieSession MS, Movie M, Client CL
		WHERE TT.ID = SMS.TicketsTransaction
		AND SMS.MovieSession = MS.ID
		AND MS.BeginningDate > GETDATE()
		AND MS.Movie = M.ID
		AND TT.Client = CL.ID
		AND CL.Email = @EMAIL
		AND TT.TransactionStatus =1;

		
		IF @VERIFICATION = 0 BEGIN
			RAISERROR('The client have not transactions active.', 16, 1);
			RETURN;
		END


		SELECT TT.ID AS TicketTransactionId, TT.CreateDate, TT.ModificationDate, TT.Payment, TT.NumberSeats, MS.BeginningDate AS BeginningDateSession, M.MovieName, MS.ID AS MovieSession
		FROM TicketsTransaction TT, SeatByMovieSession SMS, MovieSession MS, Movie M, Client CL
		WHERE TT.ID = SMS.TicketsTransaction
		AND SMS.MovieSession = MS.ID
		AND MS.BeginningDate > GETDATE()
		AND MS.Movie = M.ID
		AND TT.Client = CL.ID
		AND CL.Email = @EMAIL
		AND TT.TransactionStatus =1
		GROUP BY TT.ID, TT.CreateDate, TT.ModificationDate, TT.Payment, TT.NumberSeats, MS.BeginningDate, M.MovieName, MS.ID;

		COMMIT;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0 BEGIN
            ROLLBACK TRANSACTION; --TERMINAR LA TRANSACCION
        END
		DECLARE @MENSAJE NVARCHAR(4000)= ERROR_MESSAGE();
		DECLARE @SEVERIDAD INT = ERROR_SEVERITY();
        DECLARE @ESTADO INT = ERROR_STATE();
		RAISERROR(@MENSAJE, @SEVERIDAD, @ESTADO);
	END CATCH
END;

exec ClientByTransaction 'juan@gmail.com'