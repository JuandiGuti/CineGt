USE [BuenaCineGt]
GO
/****** Object:  StoredProcedure [dbo].[GetLowOccupancySessions]    Script Date: 11/3/2024 8:45:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[lowOccupancySessions]
    @PercentageThreshold DECIMAL(5, 2)
AS
BEGIN
    DECLARE @ThreeMonthsAgo DATE = DATEADD(MONTH, -3, GETDATE());

    SELECT ms.ID AS SessionID,
           ms.BeginningDate,
           ms.EndingDate,
           ms.Movie,
           ms.Room,
           ISNULL(SoldSeats.SeatsSold, 0) AS SeatsSold,
           (ISNULL(SoldSeats.SeatsSold, 0) * 1.0 / 100) AS OccupancyRate
    FROM MovieSession ms
    LEFT JOIN (
        SELECT MovieSession, COUNT(*) AS SeatsSold
        FROM SeatByMovieSession
        WHERE Available = 0
        GROUP BY MovieSession
    ) AS SoldSeats ON ms.ID = SoldSeats.MovieSession
    WHERE ms.BeginningDate >= @ThreeMonthsAgo
      AND ms.SessionState = 0  -- Solo considerar sesiones activas o finalizadas
      AND (ISNULL(SoldSeats.SeatsSold, 0) * 1.0 / 100) < @PercentageThreshold
    ORDER BY OccupancyRate ASC;
END;

EXEC lowOccupancySessions 0.05;