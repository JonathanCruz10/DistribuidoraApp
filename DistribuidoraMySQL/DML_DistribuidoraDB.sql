-- Usar la base de datos DistribuidoraDB
USE DistribuidoraDB;

-- Insertar registros en la tabla TipoProducto
INSERT INTO TipoProducto (Nombre) VALUES 
('Bebida'),
('Alimento'),
('Ropa');

-- Insertar registros en la tabla Producto
INSERT INTO Producto (Clave, Nombre, TipoProductoId, EsActivo, Precio) VALUES 
('P001', 'Coca Cola', 1, 1, 10.50),
('P002', 'Pepsi', 1, 1, 9.75),
('P003', 'Pan Bimbo', 2, 1, 20.00),
('P004', 'Camiseta', 3, 1, 150.00);

-- Insertar registros en la tabla Proveedor
INSERT INTO Proveedor (Nombre) VALUES 
('Proveedor A'),
('Proveedor B'),
('Proveedor C');

-- Insertar registros en la tabla ProductoProveedor
INSERT INTO ProductoProveedor (ProductoId, ProveedorId) VALUES 
(1, 1),
(2, 1),
(3, 2),
(4, 3);



