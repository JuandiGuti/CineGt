USE BuenaCineGt;
GO
--SP Crear nuevo cliente
CREATE OR ALTER PROCEDURE addClient (@NAME VARCHAR(50), @EMAIL VARCHAR(50), @PHONE VARCHAR(12), @AGE INT)
AS BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		--VERIFIACIONES
		DECLARE @VERIFICATION INT = 0;

		SELECT @VERIFICATION = COUNT(*) FROM Client 
		WHERE Email = @EMAIL

		IF @VERIFICATION > 0 BEGIN
			RAISERROR('Can not create a client with the same email: %s already exist.', 16, 1, @EMAIL);
			RETURN;
		END

		SET @VERIFICATION =0;

		SELECT @VERIFICATION = COUNT(*) FROM Client 
		WHERE Phone = @PHONE

		IF @VERIFICATION > 0 BEGIN
			RAISERROR('Can not create a client with the same phone: %s already exist.', 16, 1, @PHONE);
			RETURN;
		END

		INSERT INTO Client(ClientName, Email, Phone, Age) 
		VALUES (@NAME, @EMAIL, @PHONE, @AGE);

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

EXEC addClient 'Juan', 'juan@gmail.com', '+50235170077', 18