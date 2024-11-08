USE [BuenaCineGt]
GO
/****** Object:  StoredProcedure [dbo].[GetTop5Movies]    Script Date: 11/3/2024 8:39:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE [dbo].[top5Movies]
AS
BEGIN
DECLARE @ThreeMonthsAgo DATE = DATEADD(MONTH, -3, GETDATE());

SELECT TOP 5 m.MovieName,
       SUM(SoldSeats.SeatsSold) * 1.0 / COUNT(ms.ID) AS AvgSeatsSoldPerSession
FROM MovieSession ms
JOIN Movie m ON ms.Movie = m.ID
LEFT JOIN (
    SELECT MovieSession, COUNT(*) AS SeatsSold
    FROM SeatByMovieSession
    WHERE Available = 0
    GROUP BY MovieSession
) AS SoldSeats ON ms.ID = SoldSeats.MovieSession
WHERE ms.BeginningDate >= @ThreeMonthsAgo
  AND ms.SessionState = 0  -- Solo considerar sesiones activas o finalizadas
GROUP BY m.MovieName
ORDER BY AvgSeatsSoldPerSession DESC;
END;

EXEC top5Movies