using Microsoft.AspNetCore.Mvc;
using DistribuidoraApp.Services;
using DistribuidoraApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class ProveedorController : ControllerBase
{
    private readonly IProveedorService _proveedorService;

    public ProveedorController(IProveedorService proveedorService)
    {
        _proveedorService = proveedorService;
    }

    [HttpGet("getAll")]
    public async Task<IActionResult> GetAllProveedores()
    {
        var proveedores = await _proveedorService.GetAllProveedoresAsync();
        return Ok(proveedores);
    }
}




