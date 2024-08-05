using Microsoft.AspNetCore.Mvc;
using DistribuidoraApp.Services;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class TipoProductoController : ControllerBase
{
    private readonly ITipoProductoService _tipoProductoService;

    public TipoProductoController(ITipoProductoService tipoProductoService)
    {
        _tipoProductoService = tipoProductoService;
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllTipoProductos()
    {
        var tipoProductos = await _tipoProductoService.GetAllTipoProductosAsync();
        return Ok(tipoProductos); // Devuelve los datos en formato JSON
    }
}

