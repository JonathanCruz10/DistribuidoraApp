using DistribuidoraApp.Models;
using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DistribuidoraApp.Services
{
    public class TipoProductoService : ITipoProductoService
    {
        private readonly Database _database;

        public TipoProductoService(Database database)
        {
            _database = database;
        }

        public async Task<IEnumerable<TipoProducto>> GetAllTipoProductosAsync()
        {
            var parameters = new MySqlParameter[0];
            var dataTable = await _database.ExecuteReaderAsync("sp_get_all_tipo_productos", parameters);

            return dataTable.AsEnumerable().Select(row => new TipoProducto
            {
                Id = row.Field<int>("Id"),
                Nombre = row.Field<string>("Nombre")
            }).ToList();
        }
    }
}

