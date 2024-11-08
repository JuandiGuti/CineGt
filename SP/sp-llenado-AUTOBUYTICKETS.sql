--SP
CREATE OR ALTER PROCEDURE confirmAutoTickets(@NUMSEATS INT, @TRANSACTIONID INT, @SESSIONID INT)
AS 
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY

DECLARE @ROOM INT;

SELECT @ROOM = MAX(Room) FROM MovieSession WHERE ID = @SESSIONID

DECLARE @SEATID INT;

DECLARE json_cursor CURSOR FOR
SELECT TOP (@NUMSEATS) S.ID
FROM SeatByMovieSession SBMS, Seat S
WHERE (Available != -1)
  AND (Available = 1)
  AND (SBMS.Seat = S.ID)
  AND (Room = @ROOM)
  AND (SBMS.MovieSession = @SESSIONID)
ORDER BY LEFT(S.Seat, 1), CAST(SUBSTRING(S.Seat, 2, LEN(S.Seat) - 1) AS INT);

-- Abrimos el cursor
OPEN json_cursor;

-- Recuperamos la primera fila
FETCH NEXT FROM json_cursor INTO @SEATID;

WHILE @@FETCH_STATUS = 0 BEGIN
	UPDATE SeatByMovieSession
	SET Available = 0, TicketsTransaction = @TRANSACTIONID
	WHERE Seat = @SEATID

	FETCH NEXT FROM json_cursor INTO @SEATID;
END

CLOSE json_cursor;
DEALLOCATE json_cursor;

        COMMIT;
    END TRY
    BEGIN CATCH
        CLOSE json_cursor;
        DEALLOCATE json_cursor;
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