USE DistribuidoraDB;

USE DistribuidoraDB;
DELIMITER //

CREATE PROCEDURE sp_get_all_productos()
BEGIN
    SELECT *
    FROM Producto;
END //

DELIMITER ;


DELIMITER //

CREATE PROCEDURE sp_get_productos(
    IN p_clave VARCHAR(255),
    IN p_tipo_producto_id INT
)
BEGIN
    SELECT *
    FROM Producto
    WHERE (Clave = p_clave OR p_clave IS NULL)
      AND (TipoProductoId = p_tipo_producto_id OR p_tipo_producto_id IS NULL);
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE sp_get_producto_by_id(
    IN p_id INT
)
BEGIN
    SELECT *
    FROM Producto
    WHERE Id = p_id;
END //

DELIMITER ;


DELIMITER //

CREATE PROCEDURE sp_create_producto(
    IN p_clave VARCHAR(255),
    IN p_nombre VARCHAR(255),
    IN p_tipo_producto_id INT,
    IN p_es_activo BIT,
    IN p_precio DECIMAL(18, 2)
)
BEGIN
    INSERT INTO Producto (Clave, Nombre, TipoProductoId, EsActivo, Precio)
    VALUES (p_clave, p_nombre, p_tipo_producto_id, p_es_activo, p_precio);

    SELECT LAST_INSERT_ID() AS Id;
END //

DELIMITER ;
USE DISTRIBUIDORADB;


DELIMITER //

CREATE PROCEDURE sp_update_producto(
    IN p_id INT,
    IN p_clave VARCHAR(255),
    IN p_nombre VARCHAR(255),
    IN p_tipo_producto_id INT,
    IN p_es_activo BIT,
    IN p_precio DECIMAL(18, 2)
)
BEGIN
    UPDATE Producto
    SET Clave = p_clave,
        Nombre = p_nombre,
        TipoProductoId = p_tipo_producto_id,
        EsActivo = p_es_activo,
        Precio = p_precio
    WHERE Id = p_id;
END //

DELIMITER ;


DELIMITER //

CREATE PROCEDURE sp_get_all_tipo_productos()
BEGIN
    SELECT Id, Nombre
    FROM TipoProducto;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE sp_get_proveedores()
BEGIN
    SELECT * FROM Proveedor;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE sp_get_proveedor_by_id(IN p_id INT)
BEGIN
    SELECT * FROM Proveedor WHERE Id = p_id;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE sp_create_proveedor(IN p_nombre VARCHAR(255))
BEGIN
    INSERT INTO Proveedor (Nombre) VALUES (p_nombre);
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE sp_update_proveedor(IN p_id INT, IN p_nombre VARCHAR(255))
BEGIN
    UPDATE Proveedor SET Nombre = p_nombre WHERE Id = p_id;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE sp_delete_proveedor(IN p_id INT)
BEGIN
    DELETE FROM Proveedor WHERE Id = p_id;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE sp_get_productos_by_proveedor(IN p_proveedor_id INT)
BEGIN
    SELECT p.*
    FROM Producto p
    JOIN ProductoProveedor pp ON p.Id = pp.ProductoId
    WHERE pp.ProveedorId = p_proveedor_id;
END //

DELIMITER ;

DELIMITER //

CREATE PROCEDURE sp_add_producto_proveedor(IN p_producto_id INT, IN p_proveedor_id INT)
BEGIN
    INSERT INTO ProductoProveedor (ProductoId, ProveedorId) VALUES (p_producto_id, p_proveedor_id);
END //

DELIMITER ;

DELIMITER $$

CREATE PROCEDURE sp_delete_producto_proveedores(
    IN p_producto_id INT
)
BEGIN
    DELETE FROM ProductoProveedor
    WHERE ProductoId = p_producto_id;
END$$

DELIMITER ;


DELIMITER //

CREATE PROCEDURE sp_create_producto_proveedor(
    IN p_producto_id INT,
    IN p_proveedor_id INT,
    IN p_clave_producto VARCHAR(255),
    IN p_costo DECIMAL(18, 2)
)
BEGIN
    INSERT INTO ProductoProveedor (ProductoId, ProveedorId, ClaveProducto, Costo)
    VALUES (p_producto_id, p_proveedor_id, p_clave_producto, p_costo);
END //

DELIMITER ;
DELIMITER $$

CREATE PROCEDURE sp_get_all_proveedores()
BEGIN
    SELECT Id, Nombre FROM Proveedor;
END $$

DELIMITER ;

SHOW PROCEDURE STATUS WHERE Db = 'DistribuidoraDB';
