using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DistribuidoraApp.Services
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public async Task<DataTable> ExecuteReaderAsync(string commandText, MySqlParameter[] parameters = null)
        {
            if (string.IsNullOrEmpty(commandText))
            {
                throw new ArgumentException("El texto del comando no puede ser null o vacío.", nameof(commandText));
            }

            try
            {
                using (var connection = new MySqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(commandText, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            command.Parameters.AddRange(parameters);
                        }
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            var dataTable = new DataTable();
                            dataTable.Load(reader);
                            return dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al ejecutar el comando.", ex);
            }
        }

        public async Task<object> ExecuteScalarAsync(string storedProcedure, MySqlParameter[] parameters, MySqlConnection connection, MySqlTransaction transaction)
        {
            using (var command = new MySqlCommand(storedProcedure, connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);
                return await command.ExecuteScalarAsync();
            }
        }

        public async Task<int> ExecuteNonQueryAsync(string storedProcedure, MySqlParameter[] parameters, MySqlConnection connection, MySqlTransaction transaction)
        {
            using (var command = new MySqlCommand(storedProcedure, connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);
                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}