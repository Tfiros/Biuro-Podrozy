-- tables
-- Table: Clients
CREATE TABLE Clients (
    User_ID int  NOT NULL,
    FirstName nvarchar(50)  NOT NULL,
    LastName nvarchar(80)  NOT NULL,
    Address nvarchar(100)  NOT NULL,
    Email nvarchar(50)  NOT NULL,
    CONSTRAINT Clients_pk PRIMARY KEY  (User_ID)
);

-- Table: Orders
CREATE TABLE Orders (
    Order_ID int  NOT NULL IDENTITY,
    User_ID int  NOT NULL,
    Scheduled_Trip_ID int  NOT NULL,
    Issue_Date datetime  NOT NULL,
    End_Date datetime  NOT NULL,
    Total_Price decimal(10,2)  NOT NULL,
    Number_of_participants int  NOT NULL,
    CONSTRAINT Orders_pk PRIMARY KEY  (Order_ID)
);

-- Table: Permissions
CREATE TABLE Permissions (
    Permission_ID int  NOT NULL IDENTITY,
    Permission_Name nvarchar(50)  NOT NULL,
    CONSTRAINT Permissions_pk PRIMARY KEY  (Permission_ID)
);

-- Table: RolePermissions
CREATE TABLE RolePermissions (
    Role_ID int  NOT NULL,
    Permission_ID int  NOT NULL,
    CONSTRAINT RolePermissions_pk PRIMARY KEY  (Role_ID,Permission_ID)
);

-- Table: TripCategories
CREATE TABLE TripCategories (
    Category_ID int  NOT NULL IDENTITY,
    Category_Name nvarchar(20)  NOT NULL,
    CONSTRAINT TripCategories_pk PRIMARY KEY  (Category_ID)
);

-- Table: Trip_Schedule
CREATE TABLE Trip_Schedule (
    Schedule_ID int  NOT NULL IDENTITY,
    Trip_ID int  NOT NULL,
    Start_date datetime  NOT NULL,
    End_date datetime  NOT NULL,
    Max_participants int  NOT NULL,
    CONSTRAINT Trip_Schedule_pk PRIMARY KEY  (Schedule_ID)
);

-- Table: Trips
CREATE TABLE Trips (
    Trip_ID int  NOT NULL IDENTITY,
    Name nvarchar(30)  NOT NULL,
    Category_ID int  NOT NULL,
    Country nvarchar(20)  NOT NULL,
    Description nvarchar(max)  NOT NULL,
    Image_link nvarchar(max)  NOT NULL,
    Price decimal(10,2)  NOT NULL,
    CONSTRAINT Trips_pk PRIMARY KEY  (Trip_ID)
);

-- Table: UserRoles
CREATE TABLE UserRoles (
    Role_ID int  NOT NULL IDENTITY,
    Role_Name nvarchar(50)  NOT NULL,
    CONSTRAINT UserRoles_pk PRIMARY KEY  (Role_ID)
);

-- Table: Users
CREATE TABLE Users (
    User_ID int  NOT NULL IDENTITY,
    Role_ID int  NOT NULL,
    Login nvarchar(50)  NOT NULL,
    Password nvarchar(50)  NOT NULL,
    Refresh_token nvarchar(max)  NOT NULL,
    Expiration_date datetime  NOT NULL,
    Salt nvarchar(max)  NOT NULL,
    CONSTRAINT Users_pk PRIMARY KEY  (User_ID)
);

-- foreign keys
-- Reference: FK_0 (table: Users)
ALTER TABLE Users ADD CONSTRAINT FK_0
    FOREIGN KEY (Role_ID)
    REFERENCES UserRoles (Role_ID);

-- Reference: FK_1 (table: Clients)
ALTER TABLE Clients ADD CONSTRAINT FK_1
    FOREIGN KEY (User_ID)
    REFERENCES Users (User_ID);

-- Reference: FK_2 (table: Trips)
ALTER TABLE Trips ADD CONSTRAINT FK_2
    FOREIGN KEY (Category_ID)
    REFERENCES TripCategories (Category_ID);

-- Reference: FK_7 (table: RolePermissions)
ALTER TABLE RolePermissions ADD CONSTRAINT FK_7
    FOREIGN KEY (Role_ID)
    REFERENCES UserRoles (Role_ID);

-- Reference: FK_8 (table: RolePermissions)
ALTER TABLE RolePermissions ADD CONSTRAINT FK_8
    FOREIGN KEY (Permission_ID)
    REFERENCES Permissions (Permission_ID);

-- Reference: Orders_Clients (table: Orders)
ALTER TABLE Orders ADD CONSTRAINT Orders_Clients
    FOREIGN KEY (User_ID)
    REFERENCES Clients (User_ID);

-- Reference: Orders_Trip_Schedule (table: Orders)
ALTER TABLE Orders ADD CONSTRAINT Orders_Trip_Schedule
    FOREIGN KEY (Scheduled_Trip_ID)
    REFERENCES Trip_Schedule (Schedule_ID);

-- Reference: Trip_Schedule_Trips (table: Trip_Schedule)
ALTER TABLE Trip_Schedule ADD CONSTRAINT Trip_Schedule_Trips
    FOREIGN KEY (Trip_ID)
    REFERENCES Trips (Trip_ID);

-- Example inserts

INSERT INTO UserRoles (Role_Name) 
VALUES ('Admin'), ('Normal_user');

INSERT INTO Permissions (Permission_Name) 
VALUES ('View Trips'), ('Manage Trips');

INSERT INTO TripCategories (Category_Name) 
VALUES ('Adventure'), ('Relaxation');

INSERT INTO Trips (Name, Category_ID, Country, Description, Image_link, Price) 
VALUES 
    ('Mountain Expedition', 1, 'Poland', 'A thrilling mountain adventure.', 'image1.jpg', 1000.00),
    ('Beach Getaway', 2, 'Spain', 'Relax and unwind on the beach.', 'image2.jpg', 500.00);

INSERT INTO Trip_Schedule (Trip_ID, Start_date, End_date, Max_participants) 
VALUES 
    (1, '2025-07-01', '2025-07-10', 15),
    (2, '2025-08-01', '2025-08-10', 20);

INSERT INTO Users (Role_ID, Login, Password, Refresh_token, Expiration_date, Salt) 
VALUES 
    (1, 'admin_user', 'admin_pass', 'refresh_token1', '2025-01-01', 'salt1'),
    (2, 'normal_user', 'user_pass', 'refresh_token2', '2025-01-01', 'salt2');

INSERT INTO Clients (User_ID, FirstName, LastName, Address, Email) 
VALUES 
    (1, 'Admin', 'User', 'Admin Address', 'admin@example.com'),
    (2, 'John', 'Doe', '123 Main St', 'john.doe@example.com');

INSERT INTO Orders (User_ID, Scheduled_Trip_ID, Issue_Date, End_Date, Total_Price, Number_of_participants) 
VALUES 
    (1, 1, '2025-05-01', '2025-05-10', 1500.00, 2),
    (2, 2, '2025-06-01', '2025-06-10', 1000.00, 1);

INSERT INTO RolePermissions (Role_ID, Permission_ID) 
VALUES 
    (1, 1), -- Admin can View Trips
    (2, 2); -- Normal user can Manage Trips

INSERT INTO Trip_Schedule (Trip_ID, Start_date, End_date, Max_participants) 
VALUES 
    (1, '2025-07-01', '2025-07-10', 15),
    (2, '2025-08-01', '2025-08-10', 20);

INSERT INTO Trips (Name, Category_ID, Country, Description, Image_link, Price) 
VALUES 
    ('Mountain Adventure', 1, 'Poland', 'Exciting trekking through the Polish mountains.', 'mountain.jpg', 1200.00),
    ('Coastal Escape', 2, 'Greece', 'A relaxing retreat by the sea.', 'beach.jpg', 700.00);


