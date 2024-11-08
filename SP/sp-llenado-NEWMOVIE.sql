USE BuenaCineGt;
--SP INGRESO DE PELICULA
GO
CREATE OR ALTER PROCEDURE addMovie 
(@NAME VARCHAR(100), @DURATION TIME(7), @DESCRIPTION VARCHAR(500), @CLASIFICATION VARCHAR(20))
AS BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		--VERIFICACIONES
		DECLARE @VERIFICATION INT = 0;

		SELECT @VERIFICATION = COUNT(*) FROM Clasification 
		WHERE Clasification = @CLASIFICATION

		IF @VERIFICATION = 0 BEGIN
			RAISERROR('The clasification: %s does not exists.', 16, 1, @CLASIFICATION);
			RETURN;
		END

		SET @VERIFICATION = 0;

		SELECT @VERIFICATION = COUNT(*) FROM Movie 
		WHERE MovieName = @NAME

		IF @VERIFICATION > 0 BEGIN
			RAISERROR('A movie named %s already exists.', 16, 1, @NAME);
			RETURN;
		END

		--INSERCION
		INSERT INTO Movie (MovieName, MovieDuration, MovieDescription, Clasification) VALUES
		(@NAME, @DURATION, @DESCRIPTION, @CLASIFICATION);

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

INSERT INTO Clasification(Clasification) VALUES
('G'),
('PG'),
('PG-13'),
('R'),
('NC-17');

EXEC addMovie 'Saussage Party', '01:29:00', 'A parody of Disney and Pixar films, the film follows an anthropomorphic sausage who lives in a supermarket and goes on a journey with his friends to escape their fate as groceries while also facing a malicious douche out for revenge on him.', 'R'
EXEC addMovie 'Ted', '01:46:00', 'When John Bennett was a little boy, he made a wish that Ted, his beloved teddy bear, would come to life. Thirty years later, Ted continues to be Johns partner, much to the displeasure of Lori, Johns girlfriend. Although Loris displeasure is compounded by the couples constant consumption of beer and marijuana, she is not the one who is most disappointed in John; since he may need the intervention of Johns toy to make him mature.', 'R'
EXEC addMovie 'A Haunted House 2', '01:27:00', 'After having exorcised his exs demons, Malcolm begins a new life with his girlfriend and her two children. However, after moving into the house of his dreams, he once again suffers strange paranormal events.', 'R'
EXEC addMovie 'Avengers Infinity War', '02:29:00', 'The powerful Thanos, determined to get hold of the Infinity Stones to control the universe, attacks the ship in which the survivors of Asgard are fleeing. His intention is to get hold of the second of the gems, now in the hands of Loki.', 'PG-13'
EXEC addMovie 'John Wick', '01:41:00', 'New York City is filled with bullets when John Wick, a former hitman, returns from retirement to confront Russian gangsters, led by Viggo Tarasov, who destroyed everything he loved and put a price on his head.', 'R'
