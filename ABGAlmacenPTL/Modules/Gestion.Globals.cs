using System.Runtime.InteropServices;
using ABGAlmacenPTL.Models;

namespace ABGAlmacenPTL.Modules
{
    /// <summary>
    /// Módulo principal de gestión - Variables y constantes globales
    /// Migrado desde VB6 Gestion.bas
    /// </summary>
    public static partial class Gestion
    {
        // Importación de funciones de Win32 (solo para Windows)
        #if WINDOWS
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetComputerName(System.Text.StringBuilder lpBuffer, ref int nSize);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Beep(uint dwFreq, uint dwDuration);
        #endif

        // VARIABLES GLOBALES
        
        // --- Empresa activa de Trabajo
        public static TipoEmpresa EmpresaTrabajo { get; set; } = new TipoEmpresa();
        
        // Menu general de la aplicación (8 opciones principales)
        public static TipoMenu[] Menu { get; set; } = new TipoMenu[2];
        
        public static TipoUsuario Usuario { get; set; } = new TipoUsuario();
        
        // Codigo de empresa activa
        public static object? CodEmpresa { get; set; }
        
        // Nombre del la Empresa Activa
        public static string Empresa { get; set; } = string.Empty;
        
        // Cadena de conexión con el servidor SQL correspondiente para la BD Gestion
        public static string ConexionGestion { get; set; } = string.Empty;
        
        // Cadena de conexión con el servidor SQL correspondiente para la BD Config
        public static string ConexionConfig { get; set; } = string.Empty;
        
        // Cadena de conexión con el servidor SQL de RadioFrecuencia para la BD
        public static string ConexionRadioFrecuencia { get; set; } = string.Empty;
        
        // Cadena de conexión con el servidor de Gestion de Almacen
        public static string ConexionGestionAlmacen { get; set; } = string.Empty;
        
        // Servidor de Base de datos para la conexión
        public static string BDDServ { get; set; } = string.Empty;
        
        // Servidor local
        public static string BDDServLocal { get; set; } = string.Empty;
        
        // Base de datos para la conexión
        public static string BDDGestion { get; set; } = string.Empty;
        
        // Base de datos de configuración para la conexión
        public static string BDDConfig { get; set; } = string.Empty;
        
        // Tiempo de espera del servidor
        public static int BDDTime { get; set; }
        
        // Nombre de Usuario de acceso a la Base de Datos
        public static string UsrBDD { get; set; } = string.Empty;
        
        // Clave de acceso del Usuario de la Base de Datos
        public static string UsrKey { get; set; } = string.Empty;
        
        // Nombre de Usuario de acceso a la Base de Datos Config
        public static string UsrBDDConfig { get; set; } = string.Empty;
        
        // Clave de acceso del Usuario de la Base de Datos Config
        public static string UsrKeyConfig { get; set; } = string.Empty;
        
        // Fichero DLL de acceso para informes
        public static string FicheroDLL { get; set; } = string.Empty;
        
        // Fichero DSN de conexión para los informes
        public static string FicheroDSN { get; set; } = string.Empty;
        
        // Ruta del Fichero DSN
        public static string RutaDSN { get; set; } = string.Empty;
        
        // Fichero jpg de logotipo de la empresa
        public static string LogoEmpresa { get; set; } = string.Empty;
        
        // --- VARIABLES GLOBALES DE LA OTRA EMPRESA COINCIDENTE ----------------
        public static object? Otro_CodEmpresa { get; set; }
        public static string Otro_Empresa { get; set; } = string.Empty;
        public static string Otro_ConexionGestion { get; set; } = string.Empty;
        public static string Otro_BDDServ { get; set; } = string.Empty;
        public static string Otro_BDDGestion { get; set; } = string.Empty;
        public static int Otro_BDDTime { get; set; }
        public static string Otro_UsrBDD { get; set; } = string.Empty;
        public static string Otro_UsrKey { get; set; } = string.Empty;
        
        // --- VARIABLES GLOBALES DE LA EMPRESA CONTABILIDAD --------------------
        public static object? Contable_CodEmpresa { get; set; }
        public static string Contable_Empresa_Oficial { get; set; } = string.Empty;
        public static string Contable_Empresa_TT { get; set; } = string.Empty;
        public static string Contable_Conexion { get; set; } = string.Empty;
        public static string Contable_BDDServ { get; set; } = string.Empty;
        public static string Contable_BDDGestion { get; set; } = string.Empty;
        public static int Contable_BDDTime { get; set; }
        public static string Contable_UsrBDD { get; set; } = string.Empty;
        public static string Contable_UsrKey { get; set; } = string.Empty;
        
        // Clave maestra de acceso a zonas restringidas
        public static string ClaveMaestra { get; set; } = string.Empty;
        
        // Variable booleana que contiene el resultado de un intento de acceso a zonas restringidas
        public static bool LoginSucceeded { get; set; }
        
        // Ruta de fotos para consulta
        public static string wRCFotos { get; set; } = string.Empty;
        
        // Ruta de fotos para impresión
        public static string wRCFotosImp { get; set; } = string.Empty;
        
        // Ruta de los informes
        public static string wRInformes { get; set; } = string.Empty;
        
        // Ruta de Origen para exportación de datos
        public static string wDirOrigen { get; set; } = string.Empty;
        
        // Ruta de exportación por defecto
        public static string wDirExport { get; set; } = string.Empty;
        
        // Usuario de entrada por defecto
        public static string UsrDefault { get; set; } = string.Empty;
        
        // Fichero ini
        public static string ficINI { get; set; } = string.Empty;
        
        // Codigo de divisa de la Base de Datos
        public static int wDivisa { get; set; }
        
        // Codigo de divisa de trabajo
        public static int wTDivisa { get; set; }
        
        // Numero de decimales de trabajo
        public static int wDecimales { get; set; }
        
        // Tasa de conversión entre divisa de trabajo
        public static double wTasaCambio { get; set; }
        
        // Bloqueo de cambio de divisa cuando hay un formulario en modo edición
        public static int wBloqueoDivisa { get; set; }
        
        // Puesto de trabajo
        public static PuestoTrabajo wPuestoTrabajo { get; set; } = new PuestoTrabajo();
        
        // Impresora de etiquetas relacionada con el puesto de trabajo
        public static string wImpresora { get; set; } = string.Empty;
        
        // DECLARACIÓN DE CONSTANTES PUBLICAS
        // Constantes para los botones de Menu
        public const int CMD_Almacen = 0;
        public const int CMD_Compras = 1;
        public const int CMD_Ventas = 2;
        public const int CMD_Terceros = 3;
        public const int CMD_Ficheros = 4;
        public const int CMD_Contabilidad = 5;
        public const int CMD_Empresa = 6;
        public const int CMD_Aduana = 7;
        
        // Constantes generales
        public const double vEuro = 166.386;    // Valor del cambio Pesetas por Euro
        public const double vPeseta = 1;        // Valor del cambio Pesetas por Pesetas
        public const double vPesetaE = 0.006010121;  // Valor del cambio Euros por Peseta
        
        // NOMBRE DE LA EMPRESA DE TRABAJO
        public static string wNEmpresa { get; set; } = string.Empty;
        
        // Variables generales
        // Sistema de Ayuda
        public static string cHelpFile { get; set; } = string.Empty;
        public static string sHelpFile { get; set; } = string.Empty;
        public static bool Impresion { get; set; }
        
        // Variable para que no de error frmCátalogosConsulta
        public static int wIdioma { get; set; }

        // Método auxiliar para obtener el nombre del ordenador
        public static string GetComputerNameHelper()
        {
            #if WINDOWS
            var buffer = new System.Text.StringBuilder(256);
            int size = buffer.Capacity;
            if (GetComputerName(buffer, ref size))
            {
                return buffer.ToString();
            }
            #endif
            return Environment.MachineName;
        }

        // Método auxiliar para hacer beep (solo Windows)
        public static void BeepHelper(uint frequency, uint duration)
        {
            #if WINDOWS
            Beep(frequency, duration);
            #else
            // En Android, usar diferentes APIs o simplemente ignorar
            #endif
        }
    }
}
