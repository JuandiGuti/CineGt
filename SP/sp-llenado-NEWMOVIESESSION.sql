USE BuenaCineGt;
GO
--SP Crear nueva movie session
CREATE OR ALTER PROCEDURE addSession (@BEGININGDATE DATETIME, @MOVIENAME VARCHAR(100), @ROOM# INT, @APPUSER VARCHAR(100))
AS BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		--verification
		IF @BEGININGDATE <= GETDATE() BEGIN
			RAISERROR('Can not create a movie session in the past.', 16, 1);
			RETURN;
		END;

		DECLARE @VERIFICATION INT = 0;

		SELECT @VERIFICATION = COUNT(*) FROM Movie WHERE MovieName = @MOVIENAME

		IF @VERIFICATION = 0 BEGIN
			RAISERROR('The movie does not exist.', 16, 1);
			RETURN;
		END;

		SET @VERIFICATION = 0;

		SELECT @VERIFICATION = COUNT(*) FROM Room WHERE ID = @ROOM#

		IF @VERIFICATION = 0 BEGIN
			RAISERROR('The room does not exist.', 16, 1);
			RETURN;
		END;

		SET @VERIFICATION = 0;

		SELECT @VERIFICATION = COUNT(*) FROM AppUser WHERE Username = @APPUSER

		IF @VERIFICATION = 0 BEGIN
			RAISERROR('The room does not exist.', 16, 1);
			RETURN;
		END;

		DECLARE @MOVIEID INT, @MOVIEDURATION TIME(7), @ENDINGDATE DATETIME, @MINSUM INT;

		SELECT @MOVIEID = ID, @MOVIEDURATION = MovieDuration FROM Movie WHERE MovieName = @MOVIENAME

		SET @MINSUM = DATEDIFF(MINUTE, 0, @MOVIEDURATION) + DATEDIFF(MINUTE, 0, '00:15:00');

		SET @ENDINGDATE = DATEADD(MINUTE, @MINSUM, @BEGININGDATE);

		SET @VERIFICATION = 0;

		SELECT @VERIFICATION = COUNT(*) FROM (
		SELECT * FROM MovieSession 
		WHERE	(Room = @ROOM#)
				AND
				(BeginningDate <= @BEGININGDATE AND @BEGININGDATE <= EndingDate)
				OR 
				(BeginningDate <= @ENDINGDATE AND @ENDINGDATE <= EndingDate)) AS A
		WHERE A.SessionState = 0 OR A.SessionState = 1;

		IF @VERIFICATION > 0 BEGIN
			RAISERROR('Can not create the movie session at this date in the room: %d', 16, 1, @ROOM#);
			RETURN;
		END

		INSERT INTO MovieSession (SessionState, BeginningDate, EndingDate, CompromisedSeats, Movie,	Room, AppUser)
		VALUES (0, @BEGININGDATE, @ENDINGDATE, 0, @MOVIEID, @ROOM#, @APPUSER);

		DECLARE @IDMOVIESESSION INT;
		SELECT @IDMOVIESESSION = SCOPE_IDENTITY() FROM MovieSession

		DECLARE CURSOR_Seat CURSOR FOR
		SELECT ID
		FROM Seat
		WHERE Room = @ROOM#;

		OPEN CURSOR_Seat;

		DECLARE @SEATID VARCHAR(25);
		FETCH NEXT FROM CURSOR_Seat INTO @SEATID

		WHILE @@FETCH_STATUS = 0 BEGIN
			INSERT INTO SeatByMovieSession (Available, MovieSession, Seat)
			VALUES(1, @IDMOVIESESSION, @SEATID);

			FETCH NEXT FROM CURSOR_Seat INTO @SEATID
		END; 

		CLOSE CURSOR_Seat;
		DEALLOCATE CURSOR_Seat;

		COMMIT;
	END TRY
	BEGIN CATCH
		IF CURSOR_STATUS('global', 'CURSOR_Seat') >= 0 BEGIN
			CLOSE CURSOR_Seat;
			DEALLOCATE CURSOR_Seat;
		END
		IF @@TRANCOUNT > 0 BEGIN
            ROLLBACK TRANSACTION; --TERMINAR LA TRANSACCION
        END
		DECLARE @MENSAJE NVARCHAR(4000)= ERROR_MESSAGE();
		DECLARE @SEVERIDAD INT = ERROR_SEVERITY();
        DECLARE @ESTADO INT = ERROR_STATE();
		RAISERROR(@MENSAJE, @SEVERIDAD, @ESTADO);
	END CATCH
END;

EXEC addSession '11/29/2024 21:16:00', 'Saussage Party', 1, 'Admin'
EXEC addSession '11/30/2024 00:45:00', 'Saussage Party', 1, 'Admin'
EXEC addSession '11/29/2024 23:01:00', 'Saussage Party', 1, 'Admin'

/*
SELECT * FROM MovieSession 
		WHERE	Room = 1
		ORDER BY BeginningDate ASC

Declare @BD1 datetime= '2024-10-29 21:16:00.000'
Declare @END1 datetime= '2024-10-29 23:00:00.000'

Declare @BDINGRESAR datetime= '2024-10-29 21:00:00.000'
Declare @ENDINGRESAR datetime= '2024-10-29 22:44:00.000'

Declare @BD2 datetime= '2024-10-30 00:44:00.000'
Declare @END2 datetime= '2024-10-30 02:28:00.000'

SELECT CASE 
    WHEN (@BD1 < @BDINGRESAR AND @BDINGRESAR < @END1)
		 OR
		 (@BD1 < @ENDINGRESAR AND @ENDINGRESAR < @END1)
    THEN 'True' 
    ELSE 'False' 
END;


Declare @BDINGRESAR datetime= '2024-10-29 21:00:00.000'
Declare @ENDINGRESAR datetime= '2024-10-29 22:44:00.000'

DECLARE @VERIFICATION INT = 0;

		SELECT * FROM MovieSession 
		WHERE
				(BeginningDate < @BDINGRESAR AND @BDINGRESAR < EndingDate)
				OR 
				(BeginningDate < @ENDINGRESAR AND @ENDINGRESAR < EndingDate)

select @VERIFICATION
*/
Declare @BD1 datetime= '2024-11-08 23:20:00.000'
Declare @END1 datetime= '2024-11-09 01:21:00.000'
DECLARE @VERIFICATION INT = 0;

		SELECT * FROM MovieSession 
		WHERE	(Room = 1)
				AND
				(SessionState = 0)
				AND
				(BeginningDate <= @BD1 AND @BD1 <= EndingDate)
				AND
				(BeginningDate <= @END1 AND @END1 <= EndingDate)

		IF @VERIFICATION > 0 BEGIN
			RAISERROR('Can not create the movie session at this date in the room', 16, 1);
			RETURN;
		END
