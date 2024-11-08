USE BuenaCineGt;
GO
CREATE OR ALTER PROCEDURE cancelSession
(@MOVIESESSIONID INT)
AS BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		UPDATE MovieSession
		SET SessionState = 2, CompromisedSeats = -1
		WHERE ID = @MOVIESESSIONID

		UPDATE SeatByMovieSession
		SET Available = -1
		WHERE MovieSession = @MOVIESESSIONID

		DECLARE @TICKETSTRANSACTION INT;

		DECLARE transactionCancelByMovieSessionCancel CURSOR FOR
		SELECT TicketsTransaction FROM SeatByMovieSession
		WHERE MovieSession = @MOVIESESSIONID
		AND TicketsTransaction IS NOT NULL
		GROUP BY TicketsTransaction

		-- Abrimos el cursor
		OPEN transactionCancelByMovieSessionCancel;

		-- Recuperamos la primera fila
		FETCH NEXT FROM transactionCancelByMovieSessionCancel INTO @TICKETSTRANSACTION;
		WHILE @@FETCH_STATUS = 0
		BEGIN

			UPDATE TicketsTransaction
			SET TransactionStatus = 0
			WHERE ID = @TICKETSTRANSACTION

			DECLARE @PAYMENT DECIMAL(10,2);

			SELECT @PAYMENT = Payment FROM TicketsTransaction WHERE ID = @TICKETSTRANSACTION

			DECLARE @IDBALANCE INT, @BALANCE DECIMAL(10,2);

			SELECT @IDBALANCE = MAX(ID) FROM BalanceLog

			SELECT @BALANCE = Balance FROM BalanceLog WHERE ID = @IDBALANCE

			SET @BALANCE = @BALANCE - @PAYMENT

			INSERT INTO BalanceLog (Balance, ModificationDatetime, BalanceType, TicketsTransaction)
			VALUES (@BALANCE, GETDATE(), 0, @TICKETSTRANSACTION);

			UPDATE SeatByMovieSession
			SET TicketsTransaction = NULL
			WHERE MovieSession = @MOVIESESSIONID
			AND TicketsTransaction = @TICKETSTRANSACTION

			FETCH NEXT FROM transactionCancelByMovieSessionCancel INTO @TICKETSTRANSACTION;
		END
		CLOSE transactionCancelByMovieSessionCancel;
		DEALLOCATE transactionCancelByMovieSessionCancel;
		COMMIT;
	END TRY
	BEGIN CATCH
		CLOSE transactionCancelByMovieSessionCancel;
		DEALLOCATE transactionCancelByMovieSessionCancel;
		IF @@TRANCOUNT > 0 BEGIN
            ROLLBACK TRANSACTION; --TERMINAR LA TRANSACCION
        END
		DECLARE @MENSAJE NVARCHAR(4000)= ERROR_MESSAGE();
		DECLARE @SEVERIDAD INT = ERROR_SEVERITY();
        DECLARE @ESTADO INT = ERROR_STATE();
		RAISERROR(@MENSAJE, @SEVERIDAD, @ESTADO);
	END CATCH
END;

EXEC cancelSession 1;


select * from MovieSession
select * from SeatByMovieSession
where MovieSession = 5
select * from TicketsTransaction


EXEC BuyTickets '[{"TicketsTransaction":9, "MovieSession":"4", "Seat":"B10"}]'