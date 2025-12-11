using ABGAlmacenPTL.Configuration;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace ABGAlmacenPTL.Services;

/// <summary>
/// Servicio para gestionar la configuración desde abg.ini
/// Migrado fielmente desde VB6 Gestion.bas
/// </summary>
public class ABGConfigService
{
    private readonly string _configFilePath;
    private const string ServidorPorDefecto = "SELENE";
    
    // Propiedades leídas desde [Conexion] en abg.ini
    public string BDDServ { get; private set; } = string.Empty;
    public string BDDServLocal { get; private set; } = string.Empty;
    public int BDDTime { get; private set; } = 30;
    public string BDDConfig { get; private set; } = "Config";
    
    /// <summary>
    /// Resuelve el servidor de Config priorizando el principal, luego el alternativo
    /// y por último SELENE si ninguno está definido.
    /// </summary>
    private string ResolverServidorConfig(string? servidorPrincipal, string? servidorAlternativo = null)
    {
        if (!string.IsNullOrWhiteSpace(servidorPrincipal))
        {
            return servidorPrincipal;
        }
        
        if (!string.IsNullOrWhiteSpace(servidorAlternativo))
        {
            return servidorAlternativo;
        }
        
        return ServidorPorDefecto;
    }
    
    private string ServidorConfig => ResolverServidorConfig(BDDServ, BDDServLocal);
    
    public string ServidorConfigActual => ServidorConfig;
    
    // Usuarios y contraseñas fijos (desde VB6)
    public const string UsrBDDConfig = "ABG";
    public const string UsrKeyConfig = "A_34ggyx4";
    
    // Propiedades leídas desde [Varios] en abg.ini  
    public string UsrDefault { get; private set; } = string.Empty;
    public string CodEmpresa { get; private set; } = string.Empty;
    public int PueDefault { get; private set; } = 1;
    public string wDirExport { get; private set; } = string.Empty;
    
    public ABGConfigService()
    {
        // Obtener la ruta correcta para el archivo abg.ini según la plataforma
        _configFilePath = GetConfigFilePath();
        LoadConfiguration();
    }
    
    private string GetConfigFilePath()
    {
#if ANDROID
        // En Android, los MauiAssets se copian al directorio de archivos de la app
        var filesDir = Android.App.Application.Context.FilesDir?.AbsolutePath;
        return Path.Combine(filesDir ?? "", "abg.ini");
#else
        // Para otras plataformas
        return Path.Combine(FileSystem.AppDataDirectory, "abg.ini");
#endif
    }

    /// <summary>
    /// Lee los parámetros desde abg.ini
    /// Migrado desde VB6: LeerParamentrosIni
    /// </summary>
    private void LoadConfiguration()
    {
        try
        {
            // Verificar si el archivo existe en el sistema de archivos
            if (!File.Exists(_configFilePath))
            {
                // Intentar copiar desde los recursos embebidos
                CopyConfigFromResources();
            }

            if (File.Exists(_configFilePath))
            {
                // Cargar el archivo de configuración
                // Tu código de carga aquí
                // Leer sección [Conexion]
                BDDServ = ProfileManager.LeerIni(_configFilePath, "Conexion", "BDDServ", ServidorPorDefecto);
                BDDServLocal = ProfileManager.LeerIni(_configFilePath, "Conexion", "BDDServLocal", ServidorPorDefecto);
                
                var bddTimeStr = ProfileManager.LeerIni(_configFilePath, "Conexion", "BDDTime", "30");
                BDDTime = int.TryParse(bddTimeStr, out var time) ? time : 30;
                
                BDDConfig = ProfileManager.LeerIni(_configFilePath, "Conexion", "BDDConfig", "Config");
                
                // Migración de servidores antiguos (desde VB6)
                if (BDDServ == "RODABALLO")
                {
                    BDDServ = ServidorPorDefecto;
                    ProfileManager.GuardarIni(_configFilePath, "Conexion", "BDDServ", BDDServ);
                }
                if (BDDServ == "ARENQUE")
                {
                    BDDServ = "SELENE";
                    ProfileManager.GuardarIni(_configFilePath, "Conexion", "BDDServ", BDDServ);
                }
                
                if (BDDServLocal == "RODABALLO")
                {
                    BDDServLocal = ServidorPorDefecto;
                    ProfileManager.GuardarIni(_configFilePath, "Conexion", "BDDServLocal", BDDServLocal);
                }
                if (BDDServLocal == "ARENQUE")
                {
                    BDDServLocal = "SELENE";
                    ProfileManager.GuardarIni(_configFilePath, "Conexion", "BDDServLocal", BDDServLocal);
                }
                
                // Leer sección [Varios]
                wDirExport = ProfileManager.LeerIni(_configFilePath, "Varios", "wDirExport", "");
                UsrDefault = ProfileManager.LeerIni(_configFilePath, "Varios", "UsrDefault", "");
                CodEmpresa = ProfileManager.LeerIni(_configFilePath, "Varios", "EmpDefault", "");
                
                var pueStr = ProfileManager.LeerIni(_configFilePath, "Varios", "PueDefault", "1");
                PueDefault = int.TryParse(pueStr, out var pue) ? pue : 1;
            }
            else
            {
                // Usar valores por defecto
                LoadDefaultConfiguration();
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al cargar configuración: {ex.Message}");
            LoadDefaultConfiguration();
        }
    }
    
    private void CopyConfigFromResources()
    {
        try
        {
#if ANDROID
            // En Android, los MauiAssets están en el directorio assets
            var assetManager = Android.App.Application.Context.Assets;
            using var stream = assetManager?.Open("abg.ini");
            if (stream != null)
            {
                using var fileStream = File.Create(_configFilePath);
                stream.CopyTo(fileStream);
            }
#else
            // Para otras plataformas, usar FileSystem
            using var stream = FileSystem.OpenAppPackageFileAsync("abg.ini").Result;
            using var fileStream = File.Create(_configFilePath);
            stream.CopyTo(fileStream);
#endif
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"No se pudo copiar abg.ini desde recursos: {ex.Message}");
        }
    }
    
    private void LoadDefaultConfiguration()
    {
        // Cargar valores de configuración por defecto
        System.Diagnostics.Debug.WriteLine("Usando configuración por defecto");
        BDDServ = ServidorPorDefecto;
        BDDServLocal = ServidorPorDefecto;
        BDDTime = 30;
        BDDConfig = "Config";
        wDirExport = string.Empty;
        UsrDefault = string.Empty;
        CodEmpresa = string.Empty;
        PueDefault = 1;
    }
    
    /// <summary>
    /// Construye la cadena de conexión para Config
    /// Migrado desde VB6: ConexionConfig
    /// </summary>
    public string GetConfigConnectionString()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource = ServidorConfig,
            InitialCatalog = BDDConfig,
            UserID = UsrBDDConfig,
            Password = UsrKeyConfig,
            TrustServerCertificate = true,
            MultipleActiveResultSets = true,
            ConnectTimeout = BDDTime
        };
        return builder.ConnectionString;
    }
    
    /// <summary>
    /// Construye la cadena de conexión para Gestion
    /// Migrado desde VB6: ConexionGestion
    /// Requiere parámetros de empresa (desde tabla gdeemp en Config)
    /// </summary>
    public string GetGestionConnectionString(string bdName, string usuario, string password)
    {
        return $"Server={BDDServ};Database={bdName};User ID={usuario};Password={password};TrustServerCertificate=True;MultipleActiveResultSets=true;Connect Timeout={BDDTime}";
    }
    
    /// <summary>
    /// Construye la cadena de conexión para GestionAlmacen
    /// Migrado desde VB6: ConexionGestionAlmacen (desde empbga, empuga, empkga)
    /// </summary>
    public string GetGestionAlmacenConnectionString(string servidor, string bdName, string usuario, string password)
    {
        return $"Server={servidor};Database={bdName};User ID={usuario};Password={password};TrustServerCertificate=True;MultipleActiveResultSets=true;Connect Timeout={BDDTime}";
    }
    
    /// <summary>
    /// Refresca la configuración desde el archivo .ini
    /// </summary>
    public void Reload()
    {
        LoadConfiguration();
    }
}
