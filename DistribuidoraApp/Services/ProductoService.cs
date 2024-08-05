using DistribuidoraApp.Models;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DistribuidoraApp.Services
{
    public class ProductoService : IProductoService
    {
        private readonly Database _database;

        public ProductoService(Database database)
        {
            _database = database;
        }

        public async Task<IEnumerable<Producto>> GetAllProductosAsync()
        {
            var dataTable = await _database.ExecuteReaderAsync("sp_get_all_productos", null);

            return dataTable.AsEnumerable().Select(row => new Producto
            {
                Id = row.Field<int>("Id"),
                Clave = row.Field<string>("Clave"),
                Nombre = row.Field<string>("Nombre"),
                TipoProductoId = row.Field<int>("TipoProductoId"),
                EsActivo = row.Field<object>("EsActivo") is long longValue ? longValue == 1 : false, // Conversión segura
                Precio = row.Field<decimal?>("Precio")
            }).ToList();
        }



        public async Task<IEnumerable<Producto>> GetProductosAsync(string clave, int? tipoProductoId)
        {
            var parameters = new[]
            {
        new MySqlParameter("@p_clave", clave ?? (object)DBNull.Value),
        new MySqlParameter("@p_tipo_producto_id", tipoProductoId ?? (object)DBNull.Value)
    };

            var dataTable = await _database.ExecuteReaderAsync("sp_get_productos", parameters);

            return dataTable.AsEnumerable().Select(row => new Producto
            {
                Id = row.Field<int>("Id"),
                Clave = row.Field<string>("Clave"),
                Nombre = row.Field<string>("Nombre"),
                TipoProductoId = row.Field<int>("TipoProductoId"),
                EsActivo = Convert.ToBoolean(row.Field<object>("EsActivo")), // Conversión segura
                Precio = row.Field<decimal?>("Precio")
            }).ToList();
        }


        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            var parameters = new[] { new MySqlParameter("@p_id", id) };
            var dataTable = await _database.ExecuteReaderAsync("sp_get_producto_by_id", parameters);

            return dataTable.AsEnumerable().Select(row => new Producto
            {
                Id = row.Field<int>("Id"),
                Clave = row.Field<string>("Clave"),
                Nombre = row.Field<string>("Nombre"),
                TipoProductoId = row.Field<int>("TipoProductoId"),
                EsActivo = Convert.ToBoolean(row.Field<object>("EsActivo")), // Conversión segura
                Precio = row.Field<decimal?>("Precio")
            }).FirstOrDefault();
        }



        public async Task<Producto> CreateProductoAsync(Producto producto)
        {
            if (producto == null)
            {
                throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(producto.Clave))
            {
                throw new ArgumentException("La clave del producto es requerida.", nameof(producto));
            }

            if (string.IsNullOrWhiteSpace(producto.Nombre))
            {
                throw new ArgumentException("El nombre del producto es requerido.", nameof(producto));
            }

            if (producto.TipoProductoId <= 0)
            {
                throw new ArgumentException("El ID del tipo de producto debe ser un número positivo.", nameof(producto));
            }

            using (var connection = _database.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // Crear producto
                        var parameters = new[]
                        {
                    new MySqlParameter("@p_clave", producto.Clave),
                    new MySqlParameter("@p_nombre", producto.Nombre),
                    new MySqlParameter("@p_tipo_producto_id", producto.TipoProductoId),
                    new MySqlParameter("@p_es_activo", producto.EsActivo),
                    new MySqlParameter("@p_precio", producto.Precio ?? (object)DBNull.Value)
                };
                        var newId = await _database.ExecuteScalarAsync("sp_create_producto", parameters, connection, transaction);

                        if (newId == null || !int.TryParse(newId.ToString(), out int parsedId))
                        {
                            throw new InvalidOperationException("No se pudo obtener el ID del nuevo producto.");
                        }

                        producto.Id = parsedId;

                        // Asociar proveedores
                        if (producto.Proveedores != null)
                        {
                            foreach (var proveedor in producto.Proveedores)
                            {
                                if (proveedor.ProveedorId <= 0)
                                {
                                    throw new ArgumentException($"ID de proveedor inválido: {proveedor.ProveedorId}", nameof(producto));
                                }

                                var proveedorParams = new[]
                                {
                            new MySqlParameter("@p_producto_id", producto.Id),
                            new MySqlParameter("@p_proveedor_id", proveedor.ProveedorId),
                            new MySqlParameter("@p_clave_producto", proveedor.ClaveProducto ?? (object)DBNull.Value),
                            new MySqlParameter("@p_costo", proveedor.Costo)
                        };
                                await _database.ExecuteNonQueryAsync("sp_create_producto_proveedor", proveedorParams, connection, transaction);
                            }
                        }

                        await transaction.CommitAsync();
                        return producto;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        // Log de la excepción para más detalles
                        Console.Error.WriteLine($"Error al crear el producto: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public async Task<bool> UpdateProductoAsync(Producto producto)
        {
            if (producto == null)
            {
                throw new ArgumentNullException(nameof(producto), "El producto no puede ser nulo.");
            }

            if (producto.Id <= 0)
            {
                throw new ArgumentException("El ID del producto es requerido y debe ser un número positivo.", nameof(producto));
            }

            using (var connection = _database.CreateConnection())
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // Actualizar producto
                        var parameters = new[]
                        {
                    new MySqlParameter("@p_id", producto.Id),
                    new MySqlParameter("@p_clave", producto.Clave),
                    new MySqlParameter("@p_nombre", producto.Nombre),
                    new MySqlParameter("@p_tipo_producto_id", producto.TipoProductoId),
                    new MySqlParameter("@p_es_activo", producto.EsActivo),
                    new MySqlParameter("@p_precio", producto.Precio ?? (object)DBNull.Value) // Manejo de decimal nullable
                };

                        await _database.ExecuteNonQueryAsync("sp_update_producto", parameters, connection, transaction);

                        // Eliminar proveedores existentes
                        await _database.ExecuteNonQueryAsync("sp_delete_producto_proveedores", new[] { new MySqlParameter("@p_producto_id", producto.Id) }, connection, transaction);

                        // Insertar nuevos proveedores
                        if (producto.Proveedores != null)
                        {
                            foreach (var proveedor in producto.Proveedores)
                            {
                                var proveedorParams = new[]
                                {
                            new MySqlParameter("@p_producto_id", producto.Id),
                            new MySqlParameter("@p_proveedor_id", proveedor.ProveedorId),
                            new MySqlParameter("@p_clave_producto", proveedor.ClaveProducto ?? (object)DBNull.Value),
                            new MySqlParameter("@p_costo", proveedor.Costo)
                        };

                                await _database.ExecuteNonQueryAsync("sp_create_producto_proveedor", proveedorParams, connection, transaction);
                            }
                        }

                        await transaction.CommitAsync();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        Console.Error.WriteLine($"Error al actualizar producto: {ex.Message}");
                        throw;
                    }
                }
            }
        }
            public async Task<bool> DeleteProductoAsync(int id)
            {
                if (id <= 0)
                {
                    throw new ArgumentException("El ID del producto es requerido y debe ser un número positivo.", nameof(id));
                }

                using (var connection = _database.CreateConnection())
                {
                    await connection.OpenAsync();
                    using (var transaction = await connection.BeginTransactionAsync())
                    {
                        try
                        {
                            // Eliminar proveedores asociados al producto
                            await _database.ExecuteNonQueryAsync("sp_delete_producto_proveedores", new[] { new MySqlParameter("@p_producto_id", id) }, connection, transaction);

                            // Eliminar producto
                            var parameters = new[] { new MySqlParameter("@p_id", id) };
                            var rowsAffected = await _database.ExecuteNonQueryAsync("sp_delete_producto", parameters, connection, transaction);

                            await transaction.CommitAsync();
                            return rowsAffected > 0;
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            Console.Error.WriteLine($"Error al eliminar producto: {ex.Message}");
                            throw;
                        }
                    }
                }
            }

        }





    }




