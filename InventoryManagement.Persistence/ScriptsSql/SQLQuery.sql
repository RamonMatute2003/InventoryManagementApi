
create table Departamentos (
id int primary key,
Nombre varchar(100) not null
)

create table Empleados (
Id int primary key,
Nombre varchar(100) not null,
DepartamentoId int foreign key references Departamentos(Id),
Salario decimal(10,2),
FechaIngreso date)


insert into Departamentos (id, nombre)
values
(1,'IT'),
(2,'Contabilidad'),
(3,'RRHH'),
(4,'Mercadeo'),
(5,'Ventas')


Insert into Empleados (Id, Nombre, DepartamentoId, Salario, FechaIngreso)
values
(1, 'Mario Castro', 1, 25000, getdate()-730),
(2, 'Luisa Cerrato', 1, 15000, getdate()-120),
(3, 'Rodolfo Villas', 1, 30000, getdate()-180),
(4, 'Julian Rodas', 1, 12000, getdate()-1000),

(5, 'Ana Leiva', 2, 18000, getdate()-400),
(6, 'Julian Rodas', 2, 30000, getdate()-180),

(7, 'Marta Elvir', 3, 15000, getdate()-180),
(8, 'Carlos Rivas', 3, 18000, getdate()-180),
(9, 'Dina Clark', 3, 20000, getdate()-180),

(10, 'Antonio Garcia', 4, 25000, getdate()-900),
(11, 'Luisa Cerrato', 4, 25000, getdate()-120),
(12, 'Rodolfo Villas', 4, 32000, getdate()-730),
(13, 'Julian Rodas', 4, 12000, getdate()-180),

(14, 'Pedro Franco', 5, 20000, getdate()-200),
(15, 'Daniela Romero', 5, 22000, getdate()-180)

SELECT * FROM Departamentos;
SELECT * FROM Empleados;

/*2.1*/
SELECT 
	d.Nombre AS Departamento, 
	MIN(e.Salario) AS Minimo, 
	MAX(e.Salario) AS Maximo,
	AVG(e.Salario) AS Promedio
FROM 
	Departamentos d
INNER JOIN 
	Empleados e 
ON 
	e.DepartamentoId = d.id
GROUP BY 
	d.Nombre;


/*2.2*/
SELECT 
	d.Nombre AS Departamento, 
	COUNT(e.Id) AS NumeroEmpleados
FROM 
	Departamentos d 
INNER JOIN 
	Empleados e 
ON 
	e.DepartamentoId = d.id 
GROUP BY
	d.Nombre
HAVING 
	COUNT(e.Id) >= 3;



/*2.3*/
SELECT
	Nombre AS Colaborador,
	DATEDIFF(MONTH, FechaIngreso, GETDATE()) AS Meses
FROM 
	Empleados
WHERE 
	DATEDIFF(MONTH, FechaIngreso, GETDATE()) >= 12
ORDER BY 
    Meses ASC;



/*2.4*/
SELECT 
    e.Nombre AS Colaborador,
    d.Nombre AS Departamento,
    CASE 
        WHEN d.Nombre = 'Mercadeo' THEN 1
        ELSE 2
    END AS Orden
FROM 
    Empleados e
INNER JOIN 
    Departamentos d
ON 
    e.DepartamentoId = d.Id
ORDER BY 
    Orden,
    d.Nombre,
    e.Nombre;



/*2.5*/
SELECT 
    e1.Nombre AS Empleado,
    e1.Salario,
    d.Nombre AS Departamento
FROM 
    Empleados e1
INNER JOIN 
    Departamentos d
ON 
    e1.DepartamentoId = d.Id
LEFT JOIN 
    Empleados e2
ON 
    e1.DepartamentoId = e2.DepartamentoId 
    AND e1.Salario < e2.Salario
GROUP BY 
    e1.Nombre, e1.Salario, d.Nombre
HAVING 
    COUNT(e2.Id) < 2
ORDER BY 
    d.Nombre, e1.Salario DESC;


/*2.6*/
SELECT 
    ROW_NUMBER() OVER (PARTITION BY d.Nombre ORDER BY e.Nombre) AS Numero,
    e.Nombre AS Empleado,
    d.Nombre AS Departamento
FROM 
    Empleados e
INNER JOIN 
    Departamentos d
ON 
    e.DepartamentoId = d.Id
ORDER BY 
    d.Nombre, Numero;
