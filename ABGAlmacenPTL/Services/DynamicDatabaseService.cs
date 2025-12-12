using Microsoft.Data.SqlClient;
using System.Data;

namespace ABGAlmacenPTL.Services
{
    /// <summary>
    /// Implementación del servicio de base de datos dinámica.
    /// Permite ejecutar stored procedures y consultas SQL de forma dinámica,
    /// como lo hacía el VB6 original.
    /// </summary>
    public class DynamicDatabaseService : IDynamicDatabaseService
    {
        private readonly ABGConfigService _configService;
        private readonly AuthService _authService;

        public DynamicDatabaseService(ABGConfigService configService, AuthService authService)
        {
            _configService = configService;
            _authService = authService;
        }

        /// <summary>
        /// Obtiene la connection string según la base de datos especificada
        /// </summary>
        private string GetConnectionString(string database)
        {
            return database.ToUpper() switch
            {
                "CONFIG" => _configService.GetConfigConnectionString(),
                "GESTION" => _authService.ObtenerConnectionStringGestion() ?? throw new InvalidOperationException("No hay empresa seleccionada"),
                "GESTIONALMACEN" => _authService.ObtenerConnectionStringGestionAlmacen() ?? throw new InvalidOperationException("No hay empresa seleccionada"),
                _ => _authService.ObtenerConnectionStringGestionAlmacen() ?? throw new InvalidOperationException("No hay empresa seleccionada") // Default
            };
        }

        /// <summary>
        /// Ejecuta un stored procedure y retorna un DataTable
        /// </summary>
        public async Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen")
        {
            var dataTable = new DataTable();
            var connectionString = GetConnectionString(database);

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 30; // Timeout de 30 segundos

                    // Agregar parámetros si existen
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                        }
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Ejecuta un stored procedure sin retornar resultados
        /// </summary>
        public async Task<int> ExecuteNonQueryAsync(string procedureName, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen")
        {
            var connectionString = GetConnectionString(database);
            int rowsAffected = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 30;

                    // Agregar parámetros si existen
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                        }
                    }

                    rowsAffected = await command.ExecuteNonQueryAsync();
                }
            }

            return rowsAffected;
        }

        /// <summary>
        /// Ejecuta un stored procedure y retorna un valor escalar
        /// </summary>
        public async Task<object?> ExecuteScalarAsync(string procedureName, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen")
        {
            var connectionString = GetConnectionString(database);
            object? result = null;

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandTimeout = 30;

                    // Agregar parámetros si existen
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                        }
                    }

                    result = await command.ExecuteScalarAsync();
                }
            }

            return result == DBNull.Value ? null : result;
        }

        /// <summary>
        /// Ejecuta una consulta SQL dinámica
        /// </summary>
        public async Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen")
        {
            var dataTable = new DataTable();
            var connectionString = GetConnectionString(database);

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 30;

                    // Agregar parámetros si existen
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                        }
                    }

                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        /// <summary>
        /// Ejecuta un stored procedure y retorna una lista de diccionarios (objetos dinámicos)
        /// </summary>
        public async Task<List<Dictionary<string, object>>> ExecuteStoredProcedureDynamicAsync(string procedureName, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen")
        {
            var results = new List<Dictionary<string, object>>();
            var dataTable = await ExecuteStoredProcedureAsync(procedureName, parameters, database);

            foreach (DataRow row in dataTable.Rows)
            {
                var dict = new Dictionary<string, object>();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dict[column.ColumnName] = row[column] == DBNull.Value ? null! : row[column];
                }
                results.Add(dict);
            }

            return results;
        }

        /// <summary>
        /// Verifica si existe un stored procedure
        /// </summary>
        public async Task<bool> StoredProcedureExistsAsync(string procedureName, string database = "GestionAlmacen")
        {
            var query = @"
                SELECT COUNT(*)
                FROM sys.procedures
                WHERE name = @ProcedureName";

            var parameters = new Dictionary<string, object>
            {
                { "ProcedureName", procedureName }
            };

            try
            {
                var result = await ExecuteQueryAsync(query, parameters, database);
                if (result.Rows.Count > 0)
                {
                    return Convert.ToInt32(result.Rows[0][0]) > 0;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
