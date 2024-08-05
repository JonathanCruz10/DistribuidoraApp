
using DistribuidoraApp.Models;

using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DistribuidoraApp.Services
{
    
    public interface ITipoProductoService
    {
        Task<IEnumerable<TipoProducto>> GetAllTipoProductosAsync();
    }

 
}
