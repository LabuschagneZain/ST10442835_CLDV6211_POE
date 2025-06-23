DROP TABLE IF EXISTS Booking;
DROP TABLE IF EXISTS Event;
DROP TABLE IF EXISTS Venue;
DROP TABLE IF EXISTS EventType;

-- 1. Create EventType Table (renamed TypeName to EventType)
CREATE TABLE EventType (
    EventTypeId INT IDENTITY(1,1) PRIMARY KEY,
    EventType NVARCHAR(100) NOT NULL
);

-- 2. Updated Venue Table (added Availability)
CREATE TABLE Venue (
    VenueId INT IDENTITY(1,1) PRIMARY KEY,
    VenueName NVARCHAR(100) NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    Capacity INT NOT NULL,
    Availability BIT NOT NULL DEFAULT 1, -- 1 = available, 0 = unavailable
    ImageUrl NVARCHAR(255) NULL
);

-- 3. Updated Event Table (linked to EventType)
CREATE TABLE Event (
    EventId INT IDENTITY(1,1) PRIMARY KEY,
    EventName NVARCHAR(100) NOT NULL,
    EventDate DATETIME NOT NULL,
    Description NVARCHAR(500) NULL,
    VenueId INT NOT NULL,
    EventTypeId INT NOT NULL,
    CONSTRAINT FK_Event_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId) ON DELETE CASCADE,
    CONSTRAINT FK_Event_EventType FOREIGN KEY (EventTypeId) REFERENCES EventType(EventTypeId)
);

-- 4. Booking Table
CREATE TABLE Booking (
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    EventId INT NOT NULL,
    VenueId INT NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_Booking_Event FOREIGN KEY (EventId) REFERENCES Event(EventId) ON DELETE CASCADE,
    CONSTRAINT FK_Booking_Venue FOREIGN KEY (VenueId) REFERENCES Venue(VenueId)
);

-- Insert Event Types (updated to match new column name)
INSERT INTO EventType (EventType) VALUES 
('Conference'),
('Expo'),
('Seminar'),
('Workshop'),
('Concert');

-- Insert Venues (with availability)
INSERT INTO Venue (VenueName, Location, Capacity, Availability, ImageUrl)
VALUES 
('Skyline Ballroom', '10 Empire Street, Johannesburg', 800, 1, 'https://example.com/skyline.jpg'),
('Garden Pavilion', '22 Rose Avenue, Bloemfontein', 350, 1, 'https://example.com/gardenpavilion.jpg'),
('Lakeside Convention Centre', '88 Waterfront Blvd, Cape Town', 600, 1, 'https://example.com/lakeside.jpg');

-- Insert Events (with event type)
INSERT INTO Event (EventName, EventDate, Description, VenueId, EventTypeId)
VALUES 
('AI & Robotics Summit', '2025-07-22 08:30:00', 'Exploring advancements in AI and robotics.', 1, 1),
('Culinary Arts Fair', '2025-08-12 10:00:00', 'A food lover’s paradise with live cooking demos.', 2, 2),
('Green Energy Expo', '2025-09-25 09:00:00', 'A gathering of innovators in sustainable energy.', 3, 2);

-- Insert Bookings
INSERT INTO Booking (EventId, VenueId, BookingDate)
VALUES 
(1, 1, GETDATE()),
(2, 2, GETDATE()),
(3, 3, GETDATE());
