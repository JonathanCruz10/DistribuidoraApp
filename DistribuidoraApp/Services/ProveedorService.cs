using DistribuidoraApp.Models;
using DistribuidoraApp.Services;
using MySqlConnector;
using System.Data;

public class ProveedorService : IProveedorService
{
    private readonly Database _database;

    public ProveedorService(Database database)
    {
        _database = database;
    }

    public async Task<IEnumerable<Proveedor>> GetAllProveedoresAsync()
    {
        var parameters = new MySqlParameter[0];
        var dataTable = await _database.ExecuteReaderAsync("sp_get_all_proveedores", null); // Quita las comillas dobles

        return dataTable.AsEnumerable().Select(row => new Proveedor
        {
            Id = row.Field<int>("Id"),
            Nombre = row.Field<string>("Nombre"),
        }).ToList();
    }

}