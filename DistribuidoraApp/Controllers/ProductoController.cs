using DistribuidoraApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DistribuidoraApp.Services;
using static DistribuidoraApp.Services.IProductoService;

namespace DistribuidoraApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductosController(ProductoService productoService)
        {
            _productoService = productoService;
        }
       

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Producto>>> GetProductos([FromQuery] string clave, [FromQuery] int? tipoProductoId)
        {
            var productos = await _productoService.GetProductosAsync(clave, tipoProductoId);
            return Ok(productos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> GetProducto(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
                return NotFound();
            return producto;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProducto([FromBody] Producto producto)
        {
            if (producto == null)
            {
                return BadRequest(new { mensaje = "El producto es requerido." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // El campo Id no debe ser enviado en la creación
                producto.Id = 0;  // Forzar a cero para asegurar que no se use
                var resultado = await _productoService.CreateProductoAsync(producto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al crear el producto", detalle = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProducto([FromBody] Producto producto)
        {
            if (producto == null || producto.Id <= 0)
            {
                return BadRequest(new { mensaje = "El producto y el Id son requeridos." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var resultado = await _productoService.UpdateProductoAsync(producto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al actualizar el producto", detalle = ex.Message });
            }
        }
    

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var existingProducto = await _productoService.GetProductoByIdAsync(id);
            if (existingProducto == null)
            {
                return NotFound();
            }

            await _productoService.DeleteProductoAsync(id);
            return NoContent(); // Status code 204
        }
}
}