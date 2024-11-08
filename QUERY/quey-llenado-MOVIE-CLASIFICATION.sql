USE CineGt;
GO
INSERT INTO Clasification(Clasification) VALUES
('G'),
('PG'),
('PG-13'),
('R'),
('NC-17');
GO
INSERT INTO Movie(MovieName, MovieDuration, MovieDescription, Clasification) VALUES
('Saussage Party', '01:29:00', 'A parody of Disney and Pixar films, the film follows an anthropomorphic sausage who lives in a supermarket and goes on a journey with his friends to escape their fate as groceries while also facing a malicious douche out for revenge on him.', 'R'), -- Clasificación: R
('Ted', '01:46:00', 'When John Bennett was a little boy, he made a wish that Ted, his beloved teddy bear, would come to life. Thirty years later, Ted continues to be Johns partner, much to the displeasure of Lori, Johns girlfriend. Although Loris displeasure is compounded by the couples constant consumption of beer and marijuana, she is not the one who is most disappointed in John; since he may need the intervention of Johns toy to make him mature.', 'R'), -- Clasificación: R
('A Haunted House 2', '01:27:00', 'After having exorcised his exs demons, Malcolm begins a new life with his girlfriend and her two children. However, after moving into the house of his dreams, he once again suffers strange paranormal events.', 'R'), -- Clasificación: R
('Avengers Infinity War', '02:29:00', 'The powerful Thanos, determined to get hold of the Infinity Stones to control the universe, attacks the ship in which the survivors of Asgard are fleeing. His intention is to get hold of the second of the gems, now in the hands of Loki.', 'PG-13'), -- Clasificación: PG-13
('John Wick', '01:41:00', 'New York City is filled with bullets when John Wick, a former hitman, returns from retirement to confront Russian gangsters, led by Viggo Tarasov, who destroyed everything he loved and put a price on his head.', 'R'); -- Clasificación: R
