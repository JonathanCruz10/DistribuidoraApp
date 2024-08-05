using DistribuidoraApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace DistribuidoraApp.Services
{
    public interface IProveedorService
    {
        Task<IEnumerable<Proveedor>> GetAllProveedoresAsync();
       
    }
}
