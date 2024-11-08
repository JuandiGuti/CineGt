CREATE OR ALTER PROCEDURE roomOccupancyBy3Month
    @RoomID INT
AS
BEGIN
    DECLARE @ThreeMonthsAgo DATE = DATEADD(MONTH, -3, GETDATE());

    SELECT YEAR(ms.BeginningDate) AS Year,
           MONTH(ms.BeginningDate) AS Month,
           SUM(ISNULL(SoldSeats.SeatsSold, 0)) * 1.0 / COUNT(ms.ID) AS AvgSeatsOccupied,
           COUNT(ms.ID) AS SessionCount
    FROM MovieSession ms
    LEFT JOIN (
        SELECT MovieSession, COUNT(*) AS SeatsSold
        FROM SeatByMovieSession
        WHERE Available = 0
        GROUP BY MovieSession
    ) AS SoldSeats ON ms.ID = SoldSeats.MovieSession
    WHERE ms.BeginningDate >= @ThreeMonthsAgo
      AND ms.Room = @RoomID
      AND ms.SessionState = 0  -- Solo considerar sesiones activas o finalizadas
    GROUP BY YEAR(ms.BeginningDate), MONTH(ms.BeginningDate)
    ORDER BY Year DESC, Month DESC;
END;

EXEC roomOccupancyBy3Month @RoomID = 1;