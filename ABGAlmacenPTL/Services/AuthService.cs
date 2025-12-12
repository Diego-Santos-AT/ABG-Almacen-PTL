using Microsoft.EntityFrameworkCore;
using ABGAlmacenPTL.Data;
using ABGAlmacenPTL.Models.Config;
using System.Data;

namespace ABGAlmacenPTL.Services;

/// <summary>
/// Servicio de autenticación
/// Migrado desde VB6: frmInicio.frm - ValidaUsuario/ValidaContraseña
/// Ahora usa stored procedures dinámicos de Config DB para fidelidad 100% con VB6
/// </summary>
public class AuthService
{
    private readonly ConfigContext _configContext;
    private readonly ABGConfigService _abgConfig;
    private readonly IDynamicDatabaseService _dbService;
    
    // Usuario actual de la sesión
    public Usuario? UsuarioActual { get; private set; }
    public Empresa? EmpresaActual { get; private set; }
    
    public AuthService(ConfigContext configContext, ABGConfigService abgConfig, IDynamicDatabaseService dbService)
    {
        _configContext = configContext;
        _abgConfig = abgConfig;
        _dbService = dbService;
    }
    
    /// <summary>
    /// Prueba la conexión con el servidor Config
    /// Migrado desde VB6: ProbarConexion(serv As String)
    /// </summary>
    public async Task<bool> ProbarConexionAsync()
    {
        try
        {
            return await _configContext.Database.CanConnectAsync();
        }
        catch
        {
            return false;
        }
    }
    
    /// <summary>
    /// Busca y valida un usuario en la base de datos Config
    /// Migrado desde VB6: edC.BuscaUsuario txtUsuarios.Text
    /// Ahora usa stored procedure BuscaUsuario de Config DB (VB6-faithful)
    /// </summary>
    public async Task<Usuario?> BuscarUsuarioAsync(string nombreUsuario)
    {
        try
        {
            // Usar stored procedure BuscaUsuario de Config DB (como VB6)
            var parameters = new Dictionary<string, object>
            {
                { "NombreUsuario", nombreUsuario }
            };
            
            var result = await _dbService.ExecuteStoredProcedureAsync("BuscaUsuario", parameters, "Config");
            
            if (result.Rows.Count == 0)
                return null;
            
            var row = result.Rows[0];
            
            // Mapear resultado a modelo Usuario
            return new Usuario
            {
                UsuarioId = Convert.ToInt32(row["UsuarioId"] ?? row["usuide"] ?? 0),
                NombreUsuario = row["NombreUsuario"]?.ToString() ?? row["usunom"]?.ToString() ?? "",
                Contraseña = row["Contraseña"]?.ToString() ?? row["usucon"]?.ToString(),
                NombrePC = row.Table.Columns.Contains("NombrePC") ? row["NombrePC"]?.ToString() : 
                          row.Table.Columns.Contains("usunpc") ? row["usunpc"]?.ToString() : null,
                Instancias = row.Table.Columns.Contains("Instancias") ? Convert.ToInt32(row["Instancias"] ?? 0) :
                            row.Table.Columns.Contains("usuins") ? Convert.ToInt32(row["usuins"] ?? 0) : null
            };
        }
        catch (Exception ex)
        {
            // Fallback a EF Core si el stored procedure no existe o falla
            System.Diagnostics.Debug.WriteLine($"Error usando stored procedure BuscaUsuario, fallback a EF Core: {ex.Message}");
            return await _configContext.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario);
        }
    }
    
    /// <summary>
    /// Valida las credenciales del usuario
    /// Migrado desde VB6: ValidaContraseña
    /// </summary>
    public async Task<(bool exito, string mensaje)> ValidarCredencialesAsync(string nombreUsuario, string? contraseña)
    {
        var usuario = await BuscarUsuarioAsync(nombreUsuario);
        
        if (usuario == null)
        {
            return (false, "Usuario no encontrado");
        }
        
        // Verificar contraseña (si tiene)
        bool hayPassword = !string.IsNullOrEmpty(usuario.Contraseña);
        
        if (hayPassword && usuario.Contraseña != contraseña)
        {
            return (false, "Contraseña incorrecta");
        }
        
        // Validar nombre de PC si está configurado (VB6 original)
        if (!string.IsNullOrEmpty(usuario.NombrePC))
        {
            var nombrePCActual = Environment.MachineName;
            if (usuario.NombrePC != nombrePCActual)
            {
                return (false, $"No puede ejecutar el Programa desde este PC... (PC actual: {nombrePCActual}, requerido: {usuario.NombrePC})");
            }
        }
        
        // Login exitoso
        UsuarioActual = usuario;
        
        // Guardar usuario por defecto en abg.ini (como VB6)
        ABGAlmacenPTL.Configuration.ProfileManager.GuardarIni(
            _abgConfig.GetType().GetField("_iniFilePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_abgConfig) as string ?? "abg.ini",
            "Varios",
            "UsrDefault",
            nombreUsuario);
        
        return (true, "Login exitoso");
    }
    
    /// <summary>
    /// Obtiene las empresas a las que el usuario tiene acceso
    /// Migrado desde VB6: edC.DameEmpresasAccesoUsuario
    /// Ahora usa stored procedure DameEmpresasAccesoUsuario de Config DB (VB6-faithful)
    /// </summary>
    public async Task<List<Empresa>> ObtenerEmpresasUsuarioAsync(int usuarioId)
    {
        try
        {
            // Usar stored procedure DameEmpresasAccesoUsuario de Config DB (como VB6)
            var parameters = new Dictionary<string, object>
            {
                { "UsuarioId", usuarioId }
            };
            
            var result = await _dbService.ExecuteStoredProcedureAsync("DameEmpresasAccesoUsuario", parameters, "Config");
            
            var empresas = new List<Empresa>();
            foreach (DataRow row in result.Rows)
            {
                empresas.Add(new Empresa
                {
                    CodigoEmpresa = Convert.ToInt32(row["CodigoEmpresa"] ?? 0),
                    NombreEmpresa = row["NombreEmpresa"]?.ToString() ?? "",
                    BaseDatos = row.Table.Columns.Contains("BaseDatos") ? row["BaseDatos"]?.ToString() : null,
                    Usuario = row.Table.Columns.Contains("Usuario") ? row["Usuario"]?.ToString() : null,
                    Clave = row.Table.Columns.Contains("Clave") ? row["Clave"]?.ToString() : null,
                    ServidorGA = row.Table.Columns.Contains("ServidorGA") ? row["ServidorGA"]?.ToString() : null,
                    BaseDatosGA = row.Table.Columns.Contains("BaseDatosGA") ? row["BaseDatosGA"]?.ToString() : null,
                    UsuarioGA = row.Table.Columns.Contains("UsuarioGA") ? row["UsuarioGA"]?.ToString() : null,
                    ClaveGA = row.Table.Columns.Contains("ClaveGA") ? row["ClaveGA"]?.ToString() : null,
                    Activa = !row.Table.Columns.Contains("Activa") || Convert.ToBoolean(row["Activa"] ?? true)
                });
            }
            
            return empresas.OrderBy(e => e.NombreEmpresa).ToList();
        }
        catch (Exception ex)
        {
            // Fallback a EF Core si el stored procedure no existe o falla
            System.Diagnostics.Debug.WriteLine($"Error usando stored procedure DameEmpresasAccesoUsuario, fallback a EF Core: {ex.Message}");
            var empresasIds = await _configContext.UsuariosEmpresas
                .Where(ue => ue.UsuarioId == usuarioId)
                .Select(ue => ue.EmpresaId)
                .ToListAsync();
            
            return await _configContext.Empresas
                .Where(e => empresasIds.Contains(e.CodigoEmpresa) && e.Activa == true)
                .OrderBy(e => e.NombreEmpresa)
                .ToListAsync();
        }
    }
    
    /// <summary>
    /// Obtiene todos los puestos de trabajo
    /// Migrado desde VB6: edC.DamePuestos
    /// </summary>
    public async Task<List<Models.Config.PuestoTrabajo>> ObtenerPuestosAsync()
    {
        return await _configContext.PuestosTrabajo
            .Include(p => p.Impresora)
            .OrderBy(p => p.CodigoPuesto)
            .ToListAsync();
    }
    
    /// <summary>
    /// Obtiene un puesto de trabajo por su descripción corta
    /// Migrado desde VB6: edC.DameCodigoPuesto
    /// </summary>
    public async Task<Models.Config.PuestoTrabajo?> ObtenerPuestoPorNombreAsync(string nombreCorto)
    {
        return await _configContext.PuestosTrabajo
            .Include(p => p.Impresora)
            .FirstOrDefaultAsync(p => p.DescripcionCorta == nombreCorto);
    }
    
    /// <summary>
    /// Obtiene un puesto de trabajo por su código
    /// Migrado desde VB6: edC.DamePuestoTrabajo
    /// </summary>
    public async Task<Models.Config.PuestoTrabajo?> ObtenerPuestoPorCodigoAsync(int codigoPuesto)
    {
        return await _configContext.PuestosTrabajo
            .Include(p => p.Impresora)
            .FirstOrDefaultAsync(p => p.CodigoPuesto == codigoPuesto);
    }
    
    /// <summary>
    /// Obtiene una empresa por su nombre
    /// Migrado desde VB6: edC.DameCodigoEmpresa
    /// </summary>
    public async Task<Empresa?> ObtenerEmpresaPorNombreAsync(string nombreEmpresa)
    {
        return await _configContext.Empresas
            .FirstOrDefaultAsync(e => e.NombreEmpresa == nombreEmpresa);
    }
    
    /// <summary>
    /// Selecciona una empresa para trabajar
    /// Migrado desde VB6: ConfiguracionEmpresa
    /// </summary>
    public void SeleccionarEmpresa(Empresa empresa)
    {
        EmpresaActual = empresa;
        
        // Guardar empresa por defecto en abg.ini
        var iniPath = _abgConfig.GetType().GetField("_iniFilePath", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(_abgConfig) as string ?? "abg.ini";
        ABGAlmacenPTL.Configuration.ProfileManager.GuardarIni(iniPath, "Varios", "EmpDefault", empresa.CodigoEmpresa.ToString());
    }
    
    /// <summary>
    /// Obtiene la connection string para Gestion DB de la empresa actual
    /// </summary>
    public string? ObtenerConnectionStringGestion()
    {
        if (EmpresaActual == null) return null;
        
        return _abgConfig.GetGestionConnectionString(
            EmpresaActual.BaseDatos ?? "",
            EmpresaActual.Usuario ?? "",
            EmpresaActual.Clave ?? "");
    }
    
    /// <summary>
    /// Obtiene la connection string para GestionAlmacen DB de la empresa actual
    /// </summary>
    public string? ObtenerConnectionStringGestionAlmacen()
    {
        if (EmpresaActual == null) return null;
        
        return _abgConfig.GetGestionAlmacenConnectionString(
            EmpresaActual.ServidorGA ?? _abgConfig.BDDServ,
            EmpresaActual.BaseDatosGA ?? "",
            EmpresaActual.UsuarioGA ?? "",
            EmpresaActual.ClaveGA ?? "");
    }
    
    /// <summary>
    /// Cierra la sesión actual
    /// </summary>
    public void CerrarSesion()
    {
        UsuarioActual = null;
        EmpresaActual = null;
    }
}
