using DistribuidoraApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribuidoraApp.Services
{
  
        public interface IProductoService
        {
            
        public interface IProductoService
        {
            Task<IEnumerable<Producto>> GetAllProductosAsync();
            /// <summary>
            /// Obtiene una lista de productos basada en criterios de búsqueda opcionales.
            /// </summary>
            /// <param name="clave">Clave del producto (opcional)</param>
            /// <param name="tipoProductoId">ID del tipo de producto (opcional)</param>
            /// <returns>Una colección de productos que coinciden con los criterios de búsqueda</returns>
            Task<IEnumerable<Producto>> GetProductosAsync(string clave = null, int? tipoProductoId = null);

            /// <summary>
            /// Obtiene un producto específico por su ID.
            /// </summary>
            /// <param name="id">ID del producto</param>
            /// <returns>El producto si se encuentra, null en caso contrario</returns>
            Task<Producto> GetProductoByIdAsync(int id);

            /// <summary>
            /// Crea un nuevo producto.
            /// </summary>
            /// <param name="producto">El producto a crear</param>
            /// <returns>El producto creado con su ID asignado</returns>
            Task<Producto> CreateProductoAsync(Producto producto);

            /// <summary>
            /// Actualiza un producto existente.
            /// </summary>
            /// <param name="producto">El producto con los datos actualizados</param>
            /// <returns>True si la actualización fue exitosa, False en caso contrario</returns>
            Task<bool> UpdateProductoAsync(Producto producto);

            /// <summary>
            /// Elimina un producto por su ID.
            /// </summary>
            /// <param name="id">ID del producto a eliminar</param>
            /// <returns>True si la eliminación fue exitosa, False en caso contrario</returns>
            Task<bool> DeleteProductoAsync(int id);

            /// <summary>
            /// Obtiene los proveedores asociados a un producto específico.
            /// </summary>
            /// <param name="productoId">ID del producto</param>
            /// <returns>Una colección de ProductoProveedor asociados al producto</returns>
            Task<IEnumerable<ProductoProveedor>> GetProveedoresPorProductoAsync(int productoId);

            /// <summary>
            /// Agrega un nuevo proveedor a un producto.
            /// </summary>
            /// <param name="productoProveedor">La relación ProductoProveedor a agregar</param>
            /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
            Task<bool> AgregarProveedorAProductoAsync(ProductoProveedor productoProveedor);

            /// <summary>
            /// Elimina un proveedor de un producto.
            /// </summary>
            /// <param name="productoId">ID del producto</param>
            /// <param name="proveedorId">ID del proveedor</param>
            /// <returns>True si la operación fue exitosa, False en caso contrario</returns>
            Task<bool> EliminarProveedorDeProductoAsync(int productoId, int proveedorId);
        }
    }
}
    

