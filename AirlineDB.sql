CREATE TABLE Customers (
    CustomerId int NOT NULL AUTO_INCREMENT,
    LastName varchar(255) NOT NULL,
    FirstName varchar(255),
    PRIMARY KEY (CustomerId)
);

INSERT INTO Customers (LastName, FirstName)
VALUES ('Leos', 'Adam'),
		('Leos', 'Mariia'),
        ('Liashenko', 'Nataliia'),
        ('Liashenko', 'Anatolii'),
        ('Furgalo', 'Oleh'),
        ('Furgalo', 'Galina');

CREATE TABLE Airlines (
	AirlineId int NOT NULL AUTO_INCREMENT,
    Name varchar(255) NOT NULL,
    PRIMARY KEY (AirlineId)
);

INSERT INTO Airlines (Name)
VALUES ('Ryanair'),
('Wizz Air'),
('SkyUp Airlines');

CREATE TABLE Planes (
	PlaneId int NOT NULL AUTO_INCREMENT,
    Name varchar(100) NOT NULL,
    AirlineId int,
	PRIMARY KEY (PlaneId),
    FOREIGN KEY (AirlineId) REFERENCES Airlines(AirlineId)
);

INSERT INTO Planes(AirlineId, Name)
VALUES (1, 'Plane1'),
		(1, 'Plane2'),
        (2, 'Plane3'),
        (2, 'Plane4'),
        (3, 'Plane5');

CREATE TABLE Airports (
	AirportId int NOT NULL AUTO_INCREMENT,
    Name varchar(255) NOT NULL,
    City varchar(255) NOT NULL,
    PRIMARY KEY (AirportId)
);

INSERT INTO Airports (Name, City)
VALUES ('Warsaw Chopin', 'Warsaw'),
		('Warsaw Modlin', 'Warsaw'),
        ('Paris Orly', 'Paris'),
        ('Naples International', 'Naples'),
        ('New York LaGuardia', 'New York');

CREATE TABLE Flights (
	FlightId int NOT NULL AUTO_INCREMENT,
    PlaneId int,
    Departure datetime,
    Arrival datetime,
    DepartureAirportId int NOT NULL,
    ArrivalAirportId int NOT NULL,
    PRIMARY KEY (FlightId),
    FOREIGN KEY (PlaneId) REFERENCES Planes(PlaneId),
    FOREIGN KEY (DepartureAirportId) REFERENCES Airports(AirportId),
    FOREIGN KEY (ArrivalAirportId) REFERENCES Airports(AirportId)
);

INSERT INTO Flights (PlaneId, Departure, Arrival, DepartureAirportId, ArrivalAirportId)
VALUES (1, ('2023-12-31 01:15:00'), ('2023-12-31 03:15:00'), 1, 3),
	   (2, ('2023-06-07 13:15:00'), ('2023-06-07 18:15:00'), 1, 3),
       (3, ('2023-01-30 11:15:00'), ('2023-01-30 16:20:00'), 5, 4),
       (2, ('2023-04-30 14:15:00'), ('2023-04-31 09:15:00'), 1, 5),
       (3, ('2023-03-30 11:15:00'), ('2023-03-30 16:20:00'), 4, 5),
	   (1, ('2023-12-29 01:15:00'), ('2023-12-29 03:15:00'), 1, 3),
	   (2, ('2023-06-08 13:15:00'), ('2023-06-08 18:15:00'), 1, 3),
       (3, ('2023-03-30 11:15:00'), ('2023-03-30 16:20:00'), 5, 4),
       (2, ('2023-05-30 14:15:00'), ('2023-05-31 09:15:00'), 1, 5),
       (3, ('2023-04-30 11:15:00'), ('2023-04-30 16:20:00'), 4, 5);

CREATE TABLE Tickets (
	TicketId int NOT NULL AUTO_INCREMENT,
    FlightId int,
    SeatNumber varchar (10) NOT NULL,
    Price int NOT NULL,
    PRIMARY KEY (TicketId),
    FOREIGN KEY (FlightId) REFERENCES Flights(FlightId)
);

INSERT INTO Tickets (FlightId, SeatNumber, Price)
VALUES (21, '30d', 700),
	   (21, '30f', 700),
	   (21, '30a', 700),
	   (22, '30f', 500),
	   (22, '30a', 500),
	   (23, '30f', 700),
	   (24, '30a', 700),
	   (25, '30f', 500),
	   (26, '30a', 500),
	   (26, '28a', 500),
	   (26, '29f', 1400),
	   (27, '30a', 1400),
	   (25, '30f', 1200),
	   (26, '30k', 1200);

CREATE TABLE BoughtTickets (
	TicketId int,
    CustomerId int,
    TimeOfBuying datetime,
    FOREIGN KEY (TicketId) REFERENCES Tickets(TicketId),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

INSERT INTO BoughtTickets (TicketId, CustomerId, TimeOfBuying)
VALUES (30,1, ('2023-12-31 00:15:00')),
	   (31,1, ('2023-12-31 00:20:00')),
	   (32,2, ('2023-06-07 13:15:00')),
	   (33,4, ('2023-04-30 14:00:00')),
	   (34,5, ('2023-03-30 11:00:00')),
	   (35,3, ('2023-01-30 10:00:00')),
	   (36,2, ('2023-06-07 12:00:00')),
	   (37,1, ('2023-12-31 00:30:00')),
	   (38,2, ('2023-06-07 13:00:00')),
	   (39,1, ('2023-12-31 00:40:00')),
	   (40,3, ('2023-01-30 11:00:00')),
	   (41,2, ('2023-06-07 13:10:00')),
	   (42,1, ('2023-12-31 00:45:00')),
	   (43,3, ('2023-01-30 11:10:00'));
       
-- Список квитків з даними клієнта.
SELECT t.FlightId, t.SeatNumber, t.Price,
c.FirstName, c.LastName
FROM BoughtTickets
INNER JOIN Customers c
ON BoughtTickets.CustomerId = c.CustomerId
INNER JOIN Tickets t
ON BoughtTickets.TicketId = t.TicketId;

-- Останні 5 проданих квитків

SELECT t.TicketId, t.Price, t.SeatNumber, bt.TimeOfBuying FROM BoughtTickets bt
INNER JOIN Tickets t
ON bt.TicketId = t.TicketId
ORDER BY bt.TimeOfBuying DESC
LIMIT 5;

-- Топ 3 клієнтів за частотою польотів
SELECT Count(BoughtTickets.CustomerId) AS ticketsCount, c.LastName, c.FirstName FROM BoughtTickets
INNER JOIN Customers c
ON BoughtTickets.CustomerId = c.CustomerId
GROUP BY BoughtTickets.CustomerId
ORDER BY ticketsCount DESC
LIMIT 3;
        

        
