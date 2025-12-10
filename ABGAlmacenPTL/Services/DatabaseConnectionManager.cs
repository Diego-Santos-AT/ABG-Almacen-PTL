using Microsoft.EntityFrameworkCore;
using ABGAlmacenPTL.Data;

namespace ABGAlmacenPTL.Services;

/// <summary>
/// Servicio para gestionar conexiones dinámicas a bases de datos
/// Migrado desde VB6 - reconexión dinámica según empresa seleccionada
/// </summary>
public class DatabaseConnectionManager
{
    private readonly AuthService _authService;
    private readonly IServiceProvider _serviceProvider;
    
    public DatabaseConnectionManager(AuthService authService, IServiceProvider serviceProvider)
    {
        _authService = authService;
        _serviceProvider = serviceProvider;
    }
    
    /// <summary>
    /// Reconfigura ABGAlmacenContext para usar la base de datos GestionAlmacen
    /// de la empresa seleccionada
    /// Migrado desde VB6: Después de ConfiguracionEmpresa
    /// </summary>
    public void ConfigurarConexionGestionAlmacen()
    {
        if (_authService.EmpresaActual == null)
        {
            throw new InvalidOperationException("No hay empresa seleccionada");
        }
        
        var connectionString = _authService.ObtenerConnectionStringGestionAlmacen();
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException(
                $"No se pudo obtener connection string para GestionAlmacen de empresa {_authService.EmpresaActual.NombreEmpresa}");
        }
        
        // La conexión se reconfigurará en el próximo uso del DbContext
        // mediante el patrón de factory que se implementará
    }
    
    /// <summary>
    /// Crea una nueva instancia de ABGAlmacenContext con la conexión correcta
    /// </summary>
    public ABGAlmacenContext CrearContextoGestionAlmacen()
    {
        var connectionString = _authService.ObtenerConnectionStringGestionAlmacen();
        
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("No hay connection string configurado para GestionAlmacen");
        }
        
        var optionsBuilder = new DbContextOptionsBuilder<ABGAlmacenContext>();
        optionsBuilder.UseSqlServer(connectionString);
        
        return new ABGAlmacenContext(optionsBuilder.Options);
    }
    
    /// <summary>
    /// Verifica si la conexión a GestionAlmacen está configurada y disponible
    /// </summary>
    public async Task<bool> VerificarConexionGestionAlmacenAsync()
    {
        try
        {
            using var context = CrearContextoGestionAlmacen();
            return await context.Database.CanConnectAsync();
        }
        catch
        {
            return false;
        }
    }
}
