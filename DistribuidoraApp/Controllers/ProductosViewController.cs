using DistribuidoraApp.Models;
using DistribuidoraApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribuidoraApp.Controllers
{
    public class ProductosViewController : Controller
    {
        private readonly ProductoService _productoService;

        public ProductosViewController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            // Este método ahora solo renderizará la vista, sin cargar productos
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> Buscar(string clave, int? tipoProductoId)
        {
            var productos = await _productoService.GetProductosAsync(clave, tipoProductoId);
            var resultados = productos.Select(p => new {
                p.Id,
                p.Clave,
                p.Nombre,
                p.TipoProductoId,
                p.EsActivo,
                p.Precio
            }).ToList();

            return Json(resultados);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _productoService.GetProductoByIdAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            return View(producto);
        }
    }
}
