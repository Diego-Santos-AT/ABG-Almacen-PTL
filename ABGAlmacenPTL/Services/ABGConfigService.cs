using ABGAlmacenPTL.Configuration;

namespace ABGAlmacenPTL.Services;

/// <summary>
/// Servicio para gestionar la configuración desde abg.ini
/// Migrado fielmente desde VB6 Gestion.bas
/// </summary>
public class ABGConfigService
{
    private readonly string _iniFilePath;
    
    // Propiedades leídas desde [Conexion] en abg.ini
    public string BDDServ { get; private set; } = string.Empty;
    public string BDDServLocal { get; private set; } = string.Empty;
    public int BDDTime { get; private set; } = 30;
    public string BDDConfig { get; private set; } = "Config";
    
    // Usuarios y contraseñas fijos (desde VB6)
    public const string UsrBDDConfig = "ABG";
    public const string UsrKeyConfig = "A_34ggyx4";
    
    // Propiedades leídas desde [Varios] en abg.ini  
    public string UsrDefault { get; private set; } = string.Empty;
    public string CodEmpresa { get; private set; } = string.Empty;
    public int PueDefault { get; private set; } = 1;
    public string wDirExport { get; private set; } = string.Empty;
    
    public ABGConfigService(string? iniFilePath = null)
    {
        _iniFilePath = iniFilePath ?? "abg.ini";
        LoadConfiguration();
    }
    
    /// <summary>
    /// Lee los parámetros desde abg.ini
    /// Migrado desde VB6: LeerParamentrosIni
    /// </summary>
    private void LoadConfiguration()
    {
        if (!File.Exists(_iniFilePath))
        {
            throw new FileNotFoundException($"No se encontró el archivo de configuración: {_iniFilePath}");
        }
        
        // Leer sección [Conexion]
        BDDServ = ProfileManager.LeerIni(_iniFilePath, "Conexion", "BDDServ", "SELENE");
        BDDServLocal = ProfileManager.LeerIni(_iniFilePath, "Conexion", "BDDServLocal", "GROOT");
        
        var bddTimeStr = ProfileManager.LeerIni(_iniFilePath, "Conexion", "BDDTime", "30");
        BDDTime = int.TryParse(bddTimeStr, out var time) ? time : 30;
        
        BDDConfig = ProfileManager.LeerIni(_iniFilePath, "Conexion", "BDDConfig", "Config");
        
        // Migración de servidores antiguos (desde VB6)
        if (BDDServ == "RODABALLO")
        {
            BDDServ = "GROOT";
            ProfileManager.GuardarIni(_iniFilePath, "Conexion", "BDDServ", BDDServ);
        }
        if (BDDServ == "ARENQUE")
        {
            BDDServ = "SELENE";
            ProfileManager.GuardarIni(_iniFilePath, "Conexion", "BDDServ", BDDServ);
        }
        
        if (BDDServLocal == "RODABALLO")
        {
            BDDServLocal = "GROOT";
            ProfileManager.GuardarIni(_iniFilePath, "Conexion", "BDDServLocal", BDDServLocal);
        }
        if (BDDServLocal == "ARENQUE")
        {
            BDDServLocal = "SELENE";
            ProfileManager.GuardarIni(_iniFilePath, "Conexion", "BDDServLocal", BDDServLocal);
        }
        
        // Leer sección [Varios]
        wDirExport = ProfileManager.LeerIni(_iniFilePath, "Varios", "wDirExport", "");
        UsrDefault = ProfileManager.LeerIni(_iniFilePath, "Varios", "UsrDefault", "");
        CodEmpresa = ProfileManager.LeerIni(_iniFilePath, "Varios", "EmpDefault", "");
        
        var pueStr = ProfileManager.LeerIni(_iniFilePath, "Varios", "PueDefault", "1");
        PueDefault = int.TryParse(pueStr, out var pue) ? pue : 1;
    }
    
    /// <summary>
    /// Construye la cadena de conexión para Config
    /// Migrado desde VB6: ConexionConfig
    /// </summary>
    public string GetConfigConnectionString()
    {
        return $"Server={BDDServLocal};Database={BDDConfig};User ID={UsrBDDConfig};Password={UsrKeyConfig};TrustServerCertificate=True;MultipleActiveResultSets=true;Connect Timeout={BDDTime}";
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
