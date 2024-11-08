CREATE OR ALTER PROCEDURE rangeDateSeatsBySession
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SELECT ms.*,
           COUNT(sbms.Seat) AS OccupiedSeats
    FROM MovieSession ms
    LEFT JOIN SeatByMovieSession sbms ON ms.ID = sbms.MovieSession AND sbms.Available = 0
    WHERE ms.BeginningDate BETWEEN @StartDate AND @EndDate AND (ms.SessionState = 0 OR ms.SessionState = 1)
    GROUP BY ms.ID, ms.SessionState, ms.BeginningDate, ms.EndingDate, ms.CompromisedSeats, ms.Movie, ms.Room, ms.AppUser
    ORDER BY ms.BeginningDate;
END;

EXEC rangeDateSeatsBySession @StartDate = '2022-01-01', @EndDate = '2025-01-01';
