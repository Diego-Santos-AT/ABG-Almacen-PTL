using System.Data;

namespace ABGAlmacenPTL.Services
{
    /// <summary>
    /// Servicio para ejecutar operaciones dinámicas en la base de datos,
    /// incluyendo stored procedures y consultas SQL dinámicas.
    /// Similar al VB6 que ejecutaba procedimientos almacenados directamente.
    /// </summary>
    public interface IDynamicDatabaseService
    {
        /// <summary>
        /// Ejecuta un stored procedure y retorna un DataTable con los resultados
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento (ej: "DameArticulo", "DameGrupos")</param>
        /// <param name="parameters">Diccionario de parámetros nombre-valor</param>
        /// <param name="database">Base de datos: "Config", "Gestion", "GestionAlmacen"</param>
        /// <returns>DataTable con los resultados</returns>
        Task<DataTable> ExecuteStoredProcedureAsync(string procedureName, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen");

        /// <summary>
        /// Ejecuta un stored procedure sin retornar resultados (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento</param>
        /// <param name="parameters">Diccionario de parámetros nombre-valor</param>
        /// <param name="database">Base de datos objetivo</param>
        /// <returns>Número de filas afectadas</returns>
        Task<int> ExecuteNonQueryAsync(string procedureName, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen");

        /// <summary>
        /// Ejecuta un stored procedure y retorna un valor escalar
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento</param>
        /// <param name="parameters">Diccionario de parámetros nombre-valor</param>
        /// <param name="database">Base de datos objetivo</param>
        /// <returns>Valor escalar</returns>
        Task<object?> ExecuteScalarAsync(string procedureName, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen");

        /// <summary>
        /// Ejecuta una consulta SQL dinámica y retorna un DataTable
        /// </summary>
        /// <param name="query">Consulta SQL</param>
        /// <param name="parameters">Diccionario de parámetros nombre-valor</param>
        /// <param name="database">Base de datos objetivo</param>
        /// <returns>DataTable con los resultados</returns>
        Task<DataTable> ExecuteQueryAsync(string query, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen");

        /// <summary>
        /// Ejecuta un stored procedure y retorna una lista de objetos dinámicos
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento</param>
        /// <param name="parameters">Diccionario de parámetros nombre-valor</param>
        /// <param name="database">Base de datos objetivo</param>
        /// <returns>Lista de objetos dinámicos</returns>
        Task<List<Dictionary<string, object>>> ExecuteStoredProcedureDynamicAsync(string procedureName, Dictionary<string, object>? parameters = null, string database = "GestionAlmacen");

        /// <summary>
        /// Verifica si existe un stored procedure en la base de datos
        /// </summary>
        /// <param name="procedureName">Nombre del procedimiento</param>
        /// <param name="database">Base de datos objetivo</param>
        /// <returns>True si existe, False si no</returns>
        Task<bool> StoredProcedureExistsAsync(string procedureName, string database = "GestionAlmacen");
    }
}
