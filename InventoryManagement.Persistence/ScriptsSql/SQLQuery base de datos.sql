USE InventoryManagement;

DROP TABLE IF EXISTS InventoryOutDetails;
DROP TABLE IF EXISTS InventoryOutHeaders;
DROP TABLE IF EXISTS InventoryOutStatus;
DROP TABLE IF EXISTS InventoryLots;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Branches;
DROP TABLE IF EXISTS Users;
DROP TABLE IF EXISTS Roles;

CREATE TABLE Roles (
    IdRole INT PRIMARY KEY IDENTITY(1,1),
    RoleName VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE Users (
    IdUser INT PRIMARY KEY IDENTITY(1,1),
    UserName VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    IdRole INT NOT NULL, 

	CONSTRAINT FK_Users_Roles FOREIGN KEY (IdRole) REFERENCES Roles(IdRole) ON DELETE CASCADE
);

CREATE TABLE Branches(
	IdBranch INT PRIMARY KEY IDENTITY(1,1), 
	BranchName VARCHAR(255) NOT NULL,
	BranchLocation VARCHAR(255)
);

CREATE TABLE Products(
	IdProduct INT PRIMARY KEY IDENTITY(1,1), 
	ProductCode VARCHAR(50) NOT NULL UNIQUE,
	Name VARCHAR(255) NOT NULL,
	Cost DECIMAL(18, 2) NOT NULL,
	IdBranch INT,

	CONSTRAINT FK_Products_Branches FOREIGN KEY (IdBranch) REFERENCES Branches(IdBranch) ON DELETE SET NULL
);

CREATE TABLE InventoryLots(
	IdBatch INT PRIMARY KEY IDENTITY(1,1),
    BatchQuantity INT NOT NULL,
    ExpirationDate DATE NOT NULL,
    Cost DECIMAL(18, 2) NOT NULL,
    IdProduct INT NOT NULL,

	CONSTRAINT FK_InventoryLots_Products FOREIGN KEY (IdProduct) REFERENCES Products(IdProduct) ON DELETE CASCADE
);

CREATE TABLE InventoryOutStatus (
    IdStatus INT PRIMARY KEY IDENTITY(1,1),
    StatusName VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE InventoryOutHeaders (
    IdOutHeader INT PRIMARY KEY IDENTITY(1,1),
    OutDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalCost DECIMAL(18, 2) NOT NULL,
    IdUser INT NOT NULL,
    IdBranch INT NOT NULL,
    IdStatus INT NOT NULL DEFAULT 1,
    ReceivedBy INT NULL,
    ReceivedDate DATETIME NULL,

    CONSTRAINT FK_InventoryOutHeaders_Users FOREIGN KEY (IdUser) REFERENCES Users(IdUser) ON DELETE NO ACTION,
    CONSTRAINT FK_InventoryOutHeaders_Branches FOREIGN KEY (IdBranch) REFERENCES Branches(IdBranch) ON DELETE CASCADE,
    CONSTRAINT FK_InventoryOutHeaders_InventoryOutStatus FOREIGN KEY (IdStatus) REFERENCES InventoryOutStatus(IdStatus) ON DELETE CASCADE,
    CONSTRAINT FK_InventoryOutHeaders_ReceivedBy FOREIGN KEY (ReceivedBy) REFERENCES Users(IdUser) ON DELETE SET NULL
);

CREATE TABLE InventoryOutDetails(
	IdOutDetail INT PRIMARY KEY IDENTITY(1,1),
    Quantity INT NOT NULL,
    Cost DECIMAL(18, 2) NOT NULL,
	IdOutHeader INT NOT NULL,
    IdProduct INT NOT NULL,
    IdBatch INT NOT NULL,

    CONSTRAINT FK_InventoryOutDetails_InventoryOutHeaders FOREIGN KEY (IdOutHeader) REFERENCES InventoryOutHeaders(IdOutHeader) ON DELETE CASCADE,
    CONSTRAINT FK_InventoryOutDetails_Products FOREIGN KEY (IdProduct) REFERENCES Products(IdProduct) ON DELETE CASCADE,
    CONSTRAINT FK_InventoryOutDetails_InventoryLots FOREIGN KEY (IdBatch) REFERENCES InventoryLots(IdBatch) ON DELETE NO ACTION
);

INSERT INTO Roles (RoleName) VALUES ('Jefe de Bodega'), ('Administrador'), ('Empleado');
INSERT INTO Branches (BranchName, BranchLocation) VALUES 
('Sucursal Centro', 'Avenida Principal #123'),
('Sucursal Norte', 'Calle 45 #678');
INSERT INTO Products (ProductCode, Name, Cost, IdBranch) VALUES 
('P001', 'Laptop Dell', 15000.00, 1),
('P002', 'Mouse Logitech', 500.00, 1),
('P003', 'Teclado Microsoft', 800.00, 2);
INSERT INTO InventoryOutStatus (StatusName) VALUES ('Enviada a Sucursal'), ('Recibida en Sucursal');

SELECT * FROM Users; 
SELECT * FROM Branches; 
SELECT * FROM Products; 
SELECT * FROM InventoryOutStatus;
