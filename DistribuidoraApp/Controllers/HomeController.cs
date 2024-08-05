using Microsoft.AspNetCore.Mvc;
using DistribuidoraApp.Services;

namespace DistribuidoraApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductoService _productoService;

        public HomeController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _productoService.GetAllProductosAsync();
            return View(productos);
        }
    }

}

