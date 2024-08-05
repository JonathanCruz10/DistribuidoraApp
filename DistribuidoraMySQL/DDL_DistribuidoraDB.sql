-- **********************************************
--  Artifact:   BD_codeVisionary_DDL.sql
--  Version:    1.0
--  Date:       2024-07-15 00:00:00
--  Email:       
--  Author:     Jonathan Falcon Cruz
--
-- **********************************************

CREATE DATABASE IF NOT EXISTS DistribuidoraDB;

USE DistribuidoraDB;

CREATE TABLE IF NOT EXISTS TipoProducto (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL
);   

CREATE TABLE IF NOT EXISTS Producto (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Clave VARCHAR(255) NOT NULL,
    Nombre VARCHAR(255) NOT NULL,
    TipoProductoId INT NOT NULL,
    EsActivo BIT NOT NULL,
    Precio DECIMAL(18, 2) NULL,
    FOREIGN KEY (TipoProductoId) REFERENCES TipoProducto(Id)
);

CREATE TABLE IF NOT EXISTS Proveedor (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(255) NOT NULL
);

CREATE TABLE IF NOT EXISTS ProductoProveedor (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    ProductoId INT NOT NULL,
    ProveedorId INT NOT NULL,
    ClaveProducto VARCHAR(255),
    Costo DECIMAL(10, 2),
    FOREIGN KEY (ProductoId) REFERENCES Producto(Id),
    FOREIGN KEY (ProveedorId) REFERENCES Proveedor(Id)
);


