USE BuenaCineGt;
GO
CREATE OR ALTER PROCEDURE startTicket
(@CLIENTEMAIL VARCHAR(50), @APPUSER VARCHAR(100), @PAYMENT DECIMAL(10,2), @#SEATS INT, @MOVIESESSIONID INT, @IDTRANSACTION INT OUTPUT)
AS BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @CLIENTID INT, @VERIFICATION INT;

		SELECT @VERIFICATION = COUNT(*) FROM Client
		WHERE Email = @CLIENTEMAIL

		IF @VERIFICATION = 0 BEGIN
			RAISERROR('Do not exists a client associated to this email: %s ', 16, 1, @CLIENTEMAIL);
			RETURN;
		END

		SELECT @CLIENTID = MAX(ID) FROM Client
		WHERE Email = @CLIENTEMAIL
		
		SET @VERIFICATION = 0;

		SELECT @VERIFICATION = MAX(CompromisedSeats) FROM MovieSession
		WHERE ID = @MOVIESESSIONID

		SET @VERIFICATION = 100 - @VERIFICATION;

		IF @#SEATS > @VERIFICATION BEGIN
			IF @#SEATS = 1 BEGIN
				RAISERROR('The selected session does not have disponibility for: %d seat', 16, 1, @#SEATS);
			END
			ELSE BEGIN
				RAISERROR('The selected session does not have disponibility for: %d seats', 16, 1, @#SEATS);
			END
			RETURN;
		END

		INSERT INTO TicketsTransaction
		(TransactionStatus, CreateDate, Payment, Client, AppUser, NumberSeats)
		VALUES	(1, GETDATE(), @PAYMENT, @CLIENTID, @APPUSER, @#SEATS);

		SELECT @IDTRANSACTION = SCOPE_IDENTITY() FROM TicketsTransaction

		UPDATE MovieSession
		SET CompromisedSeats = CompromisedSeats + @#SEATS
		WHERE ID = @MOVIESESSIONID

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

EXEC startTicket 'juan@gmail.com' , 'Admin', 120.50, 1, 4
