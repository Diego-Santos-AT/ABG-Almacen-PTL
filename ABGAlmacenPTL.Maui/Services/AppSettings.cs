// ***********************************************************************
// Nombre: AppSettings.cs
// Configuración de la aplicación - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     wsConfiguracion.bas, LeerIni, GuardarIni
// ***********************************************************************

using ABGAlmacenPTL.Maui.Models;

namespace ABGAlmacenPTL.Maui.Services
{
    /// <summary>
    /// Servicio de configuración de la aplicación
    /// Equivalente a wsConfiguracion.bas y funciones LeerIni/GuardarIni de VB6
    /// </summary>
    public class AppSettings
    {
        // ----- Variables globales (equivalentes a VB6) -------------
        private static AppSettings? _instance;
        public static AppSettings Instance => _instance ??= new AppSettings();

        // Equivalentes a variables globales de VB6
        public string FicINI { get; private set; } = string.Empty;
        public string ConexionGestionAlmacen { get; private set; } = string.Empty;
        public string Empresa { get; private set; } = string.Empty;
        public Usuario Usuario { get; set; } = new Usuario();
        public Empresa EmpresaTrabajo { get; set; } = new Empresa();
        public PuestoTrabajo WPuestoTrabajo { get; set; } = new PuestoTrabajo();
        public Impresora WImpresora { get; set; } = new Impresora();

        // Opciones de empaquetado (de frmEmpaquetarBAC)
        public bool OpcionCerrarBAC { get; set; }
        public bool OpcionExtraerBAC { get; set; }
        public bool OpcionCrearCAJA { get; set; }
        public bool OpcionImprimirCAJA { get; set; }
        public bool OpcionRelContenido { get; set; }
        public bool OpcionEmpaquetado { get; set; }

        // Configuración de pantalla principal
        public double MainLeft { get; set; } = 1000;
        public double MainTop { get; set; } = 1000;
        public double MainWidth { get; set; } = 3735;
        public double MainHeight { get; set; } = 4860;

        private AppSettings()
        {
            InitializeSettings();
        }

        /// <summary>
        /// Inicializa la configuración
        /// Equivalente a Form_Load en frmMain y frmInicio
        /// </summary>
        private void InitializeSettings()
        {
            // En VB6: ficINI = App.Path & "\" & App.EXEName & ".ini"
            FicINI = Path.Combine(FileSystem.AppDataDirectory, "ABGAlmacenPTL.ini");
            
            // Cargar opciones desde preferencias
            LoadPreferences();
        }

        /// <summary>
        /// Carga las preferencias guardadas
        /// Equivalente a LeerIni de VB6
        /// </summary>
        private void LoadPreferences()
        {
            // Cargar opciones de empaquetado
            OpcionCerrarBAC = Preferences.Get("CerrarBAC", false);
            OpcionExtraerBAC = Preferences.Get("ExtraerBAC", false);
            OpcionCrearCAJA = Preferences.Get("CrearCAJA", false);
            OpcionImprimirCAJA = Preferences.Get("ImprimirCAJA", false);
            OpcionRelContenido = Preferences.Get("RelContenido", false);
            OpcionEmpaquetado = Preferences.Get("Empaquetado", false);

            // Cargar configuración de pantalla
            MainLeft = Preferences.Get("MainLeft", 1000.0);
            MainTop = Preferences.Get("MainTop", 1000.0);
            MainWidth = Preferences.Get("MainWidth", 3735.0);
            MainHeight = Preferences.Get("MainHeight", 4860.0);

            // Cargar cadena de conexión
            ConexionGestionAlmacen = Preferences.Get("ConexionGestionAlmacen", 
                "Server=localhost;Database=GAGESTION;User Id=sa;Password=;TrustServerCertificate=True;");

            Empresa = Preferences.Get("Empresa", "PTL");
        }

        /// <summary>
        /// Guarda una preferencia
        /// Equivalente a GuardarIni de VB6
        /// </summary>
        public void GuardarPreferencia(string key, object value)
        {
            if (value is bool boolValue)
                Preferences.Set(key, boolValue);
            else if (value is int intValue)
                Preferences.Set(key, intValue);
            else if (value is double doubleValue)
                Preferences.Set(key, doubleValue);
            else
                Preferences.Set(key, value?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Lee una preferencia
        /// Equivalente a LeerIni de VB6
        /// </summary>
        public T LeerPreferencia<T>(string key, T defaultValue)
        {
            if (typeof(T) == typeof(bool))
                return (T)(object)Preferences.Get(key, (bool)(object)defaultValue!);
            if (typeof(T) == typeof(int))
                return (T)(object)Preferences.Get(key, (int)(object)defaultValue!);
            if (typeof(T) == typeof(double))
                return (T)(object)Preferences.Get(key, (double)(object)defaultValue!);
            return (T)(object)Preferences.Get(key, defaultValue?.ToString() ?? string.Empty);
        }

        /// <summary>
        /// Guarda la configuración de pantalla
        /// Equivalente a MDIForm_Unload guardando settings
        /// </summary>
        public void GuardarConfiguracionPantalla()
        {
            Preferences.Set("MainLeft", MainLeft);
            Preferences.Set("MainTop", MainTop);
            Preferences.Set("MainWidth", MainWidth);
            Preferences.Set("MainHeight", MainHeight);
        }

        /// <summary>
        /// Guarda las opciones de empaquetado
        /// </summary>
        public void GuardarOpcionesEmpaquetado()
        {
            Preferences.Set("CerrarBAC", OpcionCerrarBAC);
            Preferences.Set("ExtraerBAC", OpcionExtraerBAC);
            Preferences.Set("CrearCAJA", OpcionCrearCAJA);
            Preferences.Set("ImprimirCAJA", OpcionImprimirCAJA);
            Preferences.Set("RelContenido", OpcionRelContenido);
            Preferences.Set("Empaquetado", OpcionEmpaquetado);
        }

        /// <summary>
        /// Configura la conexión a la base de datos
        /// Equivalente a inicialización del EntornoDeDatos
        /// </summary>
        public void SetConnectionString(string connectionString)
        {
            ConexionGestionAlmacen = connectionString;
            Preferences.Set("ConexionGestionAlmacen", connectionString);
        }
    }
}
