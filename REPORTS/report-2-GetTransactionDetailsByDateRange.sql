CREATE OR ALTER PROCEDURE listTicketTransactionByDate
    @StartDate DATETIME,
    @EndDate DATETIME
AS
BEGIN
    SELECT 
        tt.ID AS TransactionID,
        tt.TransactionStatus,
        tt.CreateDate AS PurchaseDate,
        tt.ModificationDate,
        tt.Payment,
        c.ClientName,
        c.Email,
        c.Phone,
        tt.NumberSeats AS TotalSeats,
        ms.ID AS MovieSessionID,
        ms.SessionState,
        ms.BeginningDate,
        ms.EndingDate,
        m.MovieName,
        r.ID AS RoomID
    FROM 
        TicketsTransaction tt
    JOIN 
        SeatByMovieSession sbms ON tt.ID = sbms.TicketsTransaction
    JOIN 
        MovieSession ms ON sbms.MovieSession = ms.ID
    JOIN 
        Movie m ON ms.Movie = m.ID
    JOIN 
        Room r ON ms.Room = r.ID
    JOIN 
        Client c ON tt.Client = c.ID
    WHERE 
        tt.CreateDate BETWEEN @StartDate AND @EndDate
		AND tt.TransactionStatus = 1
    GROUP BY 
        tt.ID, tt.TransactionStatus, tt.CreateDate, tt.ModificationDate, tt.Payment, c.ClientName, c.Email, 
        c.Phone, tt.NumberSeats, ms.ID, ms.SessionState, ms.BeginningDate, ms.EndingDate, m.MovieName, r.ID
    ORDER BY 
        tt.CreateDate;
END;

EXEC listTicketTransactionByDate '2022-11-02 00:00:00', '2025-11-02 23:59:59';