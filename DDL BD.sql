GO
CREATE DATABASE BuenaCineGt;
GO
USE BuenaCineGt;
GO
CREATE TABLE LogType (
	LogType VARCHAR(100) PRIMARY KEY
);
GO
CREATE TABLE TransactionLog (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	SqlUser VARCHAR(100) NOT NULL,
	DateCreated DATETIME NOT NULL,
	DescriptionLog VARCHAR(500) NOT NULL,
	--fk
	LogType VARCHAR(100) NOT NULL,
	CONSTRAINT FK_TransactionLog_LogType FOREIGN KEY(LogType) REFERENCES LogType(LogType)
);
GO
CREATE TABLE Room (
	ID INT PRIMARY KEY
);
GO
CREATE TABLE Seat (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	seat VARCHAR(5) NOT NULL,
	--fk
	Room INT NOT NULL,
	CONSTRAINT FK_Seat_Room FOREIGN KEY(Room) REFERENCES Room(ID)
);
GO
CREATE TABLE Clasification ( 
	Clasification VARCHAR(20) PRIMARY KEY
);
GO
CREATE TABLE Movie (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	MovieName VARCHAR(100) NOT NULL,
	MovieDuration TIME(7) NOT NULL, 
	MovieDescription VARCHAR(500) NOT NULL,
	--fk
	Clasification VARCHAR(20) NOT NULL,
	CONSTRAINT FK_Movie_Clasification FOREIGN KEY(Clasification) REFERENCES Clasification(Clasification)
);
GO
CREATE TABLE AppUser (
	Username VARCHAR(100) PRIMARY KEY,
	Pass VARCHAR (300) NOT NULL,
	UserRole INT NOT NULL
);
GO
CREATE TABLE Client(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	ClientName VARCHAR(50) NOT NULL,
	Email VARCHAR(50) NOT NULL, 
	Phone VARCHAR(12) NOT NULL,
	Age INT NOT NULL
);
GO
CREATE TABLE MovieSession (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	SessionState INT NOT NULL,
	BeginningDate DATETIME NOT NULL,
	EndingDate DATETIME NOT NULL,
	CompromisedSeats INT NOT NULL, -- # seats in sell process ej. 100 seats 52 Compromised Only can sell 48.
	--fk
	Movie INT NOT NULL,
	Room INT NOT NULL,
	AppUser VARCHAR(100) NOT NULL,
	CONSTRAINT FK_MovieSession_Movie FOREIGN KEY(Movie) REFERENCES Movie(ID),
	CONSTRAINT FK_MovieSession_Room FOREIGN KEY(Room) REFERENCES Room(ID),
	CONSTRAINT FK_MovieSession_AppUser FOREIGN KEY(AppUser) REFERENCES AppUser(Username)
);
GO
CREATE TABLE BalanceLog (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Balance DECIMAL(10,2) NOT NULL,
	ModificationDatetime DATETIME NOT NULL,
	BalanceType INT NOT NULL, --0 add , 1 sub
	TicketsTransaction INT NOT NULL --ID of TicketTransaction
);
GO
CREATE TABLE TicketsTransaction (
	ID INT IDENTITY(1,1) PRIMARY KEY,
	TransactionStatus INT NOT NULL, -- 1 = ACTIVE, 0 = CANCEL
	CreateDate DATETIME NOT NULL,
	ModificationDate DATETIME NULL,
	Payment DECIMAL(10,2) NOT NULL,
	NumberSeats INT NOT NULL,
	--fk
	Client INT NOT NULL,
	AppUser VARCHAR(100) NOT NULL,
	CONSTRAINT FK_TicketsTransaction_Client FOREIGN KEY(Client) REFERENCES Client(ID),
	CONSTRAINT FK_TicketsTransaction_AppUser FOREIGN KEY(AppUser) REFERENCES AppUser(Username)
);
GO
CREATE TABLE SeatByMovieSession(
	ID INT IDENTITY(1,1) PRIMARY KEY,
	Available INT NOT NULL, -- 1  = VACIO , 0 = OCUPADO
	--fk
	MovieSession INT NOT NULL,
	Seat INT NOT NULL,
	TicketsTransaction INT NULL,
	CONSTRAINT FK_SeatByMovieSession_MovieSession FOREIGN KEY(MovieSession) REFERENCES MovieSession(ID),
	CONSTRAINT FK_SeatByMovieSession_Seat FOREIGN KEY(Seat) REFERENCES Seat(ID),
	CONSTRAINT FK_SeatByTransaction_TicketsTransaction FOREIGN KEY(TicketsTransaction) REFERENCES TicketsTransaction(ID)
);