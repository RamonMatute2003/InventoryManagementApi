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
('Sucursal Norte', 'Calle 45 #678'),
('Sucursal Sur', 'Calle 60 #78');

INSERT INTO Products (ProductCode, Name, Cost, IdBranch) VALUES 
('P004', 'Monitor Samsung', 2500.00, 1),
('P005', 'Disco Duro SSD 1TB', 1800.00, 2),
('P006', 'Memoria RAM 16GB', 700.00, 1),
('P007', 'Silla Ergonómica', 3200.00, 1),
('P008', 'Mesa de Oficina', 5000.00, 2),
('P009', 'Impresora HP', 3800.00, 1),
('P010', 'Cable HDMI', 150.00, 2);

INSERT INTO InventoryOutStatus (StatusName) VALUES ('Enviada a Sucursal'), ('Recibida en Sucursal');

INSERT INTO InventoryLots (BatchQuantity, ExpirationDate, Cost, IdProduct) VALUES
(5, '2024-06-30', 1500.00, 1),
(10, '2024-12-15', 1500.00, 1),
(20, '2025-06-30', 500.00, 2),
(50, '2025-10-31', 450.00, 2),
(5, '2024-05-20', 2500.00, 4),
(15, '2024-08-15', 2500.00, 4),
(8, '2024-07-10', 1800.00, 5),
(12, '2025-01-01', 1800.00, 5),
(6, '2024-09-20', 700.00, 6),
(20, '2025-03-10', 700.00, 6),
(5, '2024-06-10', 3200.00, 7),
(7, '2025-02-20', 3200.00, 7),
(3, '2024-04-01', 5000.00, 5),
(10, '2025-06-30', 5000.00, 4),
(6, '2024-11-11', 3800.00, 1),
(9, '2025-07-01', 3800.00, 6),
(15, '2025-12-01', 150.00, 7),
(25, '2026-03-15', 150.00, 5);

SELECT * FROM Users; 
SELECT * FROM Branches; 
SELECT * FROM Products; 
SELECT * FROM InventoryOutStatus;
SELECT * FROM InventoryOutHeaders;
SELECT * FROM InventoryLots;
SELECT * FROM InventoryOutDetails;


SELECT 
    il.IdProduct,
    p.ProductCode,
    p.Name,
    SUM(il.BatchQuantity) AS AvailableStock
FROM InventoryLots il
JOIN Products p ON il.IdProduct = p.IdProduct
GROUP BY il.IdProduct, p.ProductCode, p.Name
ORDER BY il.IdProduct;