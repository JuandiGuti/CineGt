USE BuenaCineGt;
GO
--SP ingresar un nuevo APPUSER
CREATE OR ALTER PROCEDURE newAppUser (@USERNAME VARCHAR(100), @PASS VARCHAR(300))
AS BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		--VERIFIACIONES
		DECLARE @VERIFICATION INT = 0;

		SELECT @VERIFICATION = COUNT(*) FROM AppUser 
		WHERE Username = @USERNAME

		IF @VERIFICATION > 0 BEGIN
			RAISERROR('The username: %s already exist.', 16, 1, @USERNAME);
			RETURN;
		END

		DECLARE @ROLE INT;
		SET @VERIFICATION = 0;

		SELECT @VERIFICATION = COUNT(*) FROM AppUser

		--role 1 admin role 0 employee 
		IF @VERIFICATION = 0 BEGIN
			SET @ROLE = 1;
		END
		ELSE BEGIN
			SET @ROLE = 0;
		END

		INSERT INTO AppUser (Username, Pass, UserRole) VALUES 
		(@USERNAME, @PASS, @ROLE);

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

EXEC newAppUser 'Admin', '123'

