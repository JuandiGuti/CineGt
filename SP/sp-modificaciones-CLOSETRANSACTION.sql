USE BuenaCineGt;
GO
CREATE OR ALTER PROCEDURE closeTicket
(@TICKETSTRANSACTIONID INT, @MOVIESESSIONID INT)
AS BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @#SEATS INT;

		SELECT @#SEATS = MAX(NumberSeats) FROM TicketsTransaction
		WHERE ID = @TICKETSTRANSACTIONID

		UPDATE MovieSession
		SET CompromisedSeats = CompromisedSeats - @#SEATS
		WHERE ID = @MOVIESESSIONID

		UPDATE TicketsTransaction
		SET TransactionStatus = 0, ModificationDate = GETDATE()
		WHERE ID = @TICKETSTRANSACTIONID

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

EXEC closeTicket 6;

