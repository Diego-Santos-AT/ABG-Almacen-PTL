using ABGAlmacenPTL.Configuration;
using ABGAlmacenPTL.Models;
using ABGAlmacenPTL.Pages;

namespace ABGAlmacenPTL.Modules
{
    /// <summary>
    /// Módulo principal de gestión - Métodos
    /// Migrado desde VB6 Gestion.bas
    /// </summary>
    public static partial class Gestion
    {
        /// <summary>
        /// Punto de entrada principal de la aplicación
        /// Migrado desde VB6: Sub Main()
        /// </summary>
        public static async Task Main()
        {
            string sMsg = string.Empty;
            string config;

            try
            {
                // **** 1er Paso del Arranque: Comprobar que existe el fichero ABG.INI
                ficINI = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "abg.ini");

                // Si no existe se crea
                if (!File.Exists(ficINI))
                {
                    CrearAGBIni(ficINI);
                }

                // Lee los primeros parametros
                LeerParamentrosIni(ficINI);

                // **** Inicializar otras variables generales
                InicializarVariablesGenerales();

                // **** 3º Comprobación de la conexión con el servidor y acceso del usuario
                // En MAUI, la navegación se maneja diferente, esto debería llamarse desde App.xaml.cs
                // await Shell.Current.GoToAsync("//InicioPage");

                // El flujo continúa en InicioPage.xaml.cs después del login exitoso
                // Si LoginSucceeded = true entonces:
                // 1. InstanciasPrograma()
                // 2. ConfiguracionEmpresa(CodEmpresa)
                // 3. LeerDSN()
                // 4. Navegar a MenuPage o MainPage
            }
            catch (Exception ex)
            {
                // Control de errores
                await Application.Current.MainPage.DisplayAlert(
                    "Error de Inicio",
                    $"Error: {ex.Message}",
                    "OK");
            }
        }

        /// <summary>
        /// Crea el fichero de inicio ABG.INI
        /// Migrado desde VB6: Private Sub CrearAGBIni(Fichero As String)
        /// </summary>
        private static void CrearAGBIni(string fichero)
        {
            // --- Configuración de la pantalla
            ProfileManager.GuardarIni(fichero, "Pantalla", "MainLeft", "-60");
            ProfileManager.GuardarIni(fichero, "Pantalla", "MainTop", "-60");
            ProfileManager.GuardarIni(fichero, "Pantalla", "MainWidth", "15480");
            ProfileManager.GuardarIni(fichero, "Pantalla", "MainHeight", "11220");

            // --- Conexión
            ProfileManager.GuardarIni(fichero, "Conexion", "BDDTime", "30");
            ProfileManager.GuardarIni(fichero, "Conexion", "BDDConfig", "Config");

            // --- Servidores por defecto si no existe el ABG.INI. Deberán cambiarse por los correspondientes
            ProfileManager.GuardarIni(fichero, "Conexion", "BDDServ", "ARES");
            ProfileManager.GuardarIni(fichero, "Conexion", "BDDServLocal", "ARES");

            // --- Varios
            ProfileManager.GuardarIni(fichero, "Varios", "wDirExport", "");
            ProfileManager.GuardarIni(fichero, "Varios", "UsrDefault", "");
            ProfileManager.GuardarIni(fichero, "Varios", "EmpDefault", "");
            ProfileManager.GuardarIni(fichero, "Varios", "PueDefault", "");
        }

        /// <summary>
        /// Lee los parámetros del fichero INI
        /// Migrado desde VB6: Private Sub LeerParamentrosIni(Fichero As String)
        /// </summary>
        private static void LeerParamentrosIni(string fichero)
        {
            BDDServ = ProfileManager.LeerIni(ficINI, "Conexion", "BDDServ");

            // Leemos el servidor local
            BDDServLocal = ProfileManager.LeerIni(ficINI, "Conexion", "BDDServLocal", "");
            BDDTime = int.Parse(ProfileManager.LeerIni(ficINI, "Conexion", "BDDTime", "30"));
            BDDConfig = ProfileManager.LeerIni(ficINI, "Conexion", "BDDConfig", "Config");
            UsrBDDConfig = "ABG";  // El usuario es fijo
            UsrKeyConfig = "A_34ggyx4";    // Su contraseña tb

            // Leemos varios
            wDirExport = ProfileManager.LeerIni(ficINI, "Varios", "wDirExport", AppDomain.CurrentDomain.BaseDirectory);
            UsrDefault = ProfileManager.LeerIni(ficINI, "Varios", "UsrDefault", "");
            CodEmpresa = ProfileManager.LeerIni(ficINI, "Varios", "EmpDefault", "");
            wPuestoTrabajo.Id = int.Parse(ProfileManager.LeerIni(ficINI, "Varios", "PueDefault", "1"));  // -- Puesto 1 por defecto

            // -- MIGRACION A SQL 2016 (03 ABRIL 2020)
            if (BDDServ == "RODABALLO")
            {
                BDDServ = "GROOT";
            }
            if (BDDServ == "ARENQUE")
            {
                BDDServ = "SELENE";
            }
            ProfileManager.GuardarIni(fichero, "Conexion", "BDDServ", BDDServ);

            if (BDDServLocal == "RODABALLO")
            {
                BDDServLocal = "GROOT";
            }
            if (BDDServLocal == "ARENQUE")
            {
                BDDServLocal = "SELENE";
            }
            ProfileManager.GuardarIni(fichero, "Conexion", "BDDServLocal", BDDServLocal);

            // Creamos la cadena del conexión al Config siempre al servidor local
            ConexionConfig = $"Provider=SQLOLEDB.1;Persist Security Info=False;User ID={UsrBDDConfig};Password={UsrKeyConfig};Initial Catalog={BDDConfig};Data Source={BDDServLocal};Connect Timeout={BDDTime}";
        }

        /// <summary>
        /// Inicializa las variables generales de la aplicación
        /// Migrado desde VB6: Private Sub InicializarVariablesGenerales()
        /// </summary>
        private static void InicializarVariablesGenerales()
        {
            // Inicializar arrays y variables globales
            ClaveMaestra = "GARIBALDI";

            // Nota: vEuro, vPeseta, vPesetaE ya están definidos como constantes
            // No necesitan re-asignación

            // Nota: CMD_* ya están definidos como constantes
            // No necesitan re-asignación

            // Nota: RecordsetEOF y RecordsetBOF no son necesarios en .NET
            // EF Core maneja el estado del recordset automáticamente

            // Inicializar puesto de trabajo por defecto
            wPuestoTrabajo = new PuestoTrabajo
            {
                Id = 1,
                Nombre = "Por Defecto",
                NombreCorto = "DEF",
                Impresora = 0,
                NombreImpresora = "",
                TipoImpresora = ""
            };
        }

        /// <summary>
        /// Configura la empresa de trabajo
        /// Migrado desde VB6: Private Sub ConfiguracionEmpresa(codemp)
        /// </summary>
        public static async Task ConfiguracionEmpresa(object codemp)
        {
            // TODO: Implementar acceso a datos cuando tengamos el DAL
            // Por ahora, esto es un placeholder

            try
            {
                // Abrir conexión con la configuración
                // edC.Config.Open(ConexionConfig)

                // Obtener empresas de acceso del usuario
                // edC.DameEmpresasAccesoUsuario(Usuario.Id)

                // Verificar que el usuario tiene empresas asignadas
                // if (empresasCount == 0) { mostrar error y salir }

                // Cargar parámetros de empresa
                await CargarParametrosEmpresa();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error de Configuración",
                    $"Error al configurar empresa: {ex.Message}",
                    "OK");
            }
        }

        /// <summary>
        /// Carga los parámetros de la empresa activa
        /// Migrado desde VB6: Public Sub CargarParametrosEmpresa()
        /// </summary>
        public static async Task CargarParametrosEmpresa()
        {
            // TODO: Implementar cuando tengamos el Data Access Layer
            // Esta función carga todos los parámetros de EmpresaTrabajo desde la BD
            // Por ahora es un placeholder

            try
            {
                // Consultar BD Config para obtener parámetros de empresa
                // Llenar EmpresaTrabajo con los datos

                // Configurar cadenas de conexión
                ConexionGestion = $"Provider=SQLOLEDB.1;Persist Security Info=False;User ID={EmpresaTrabajo.Usuario};Password={EmpresaTrabajo.Clave};Initial Catalog={EmpresaTrabajo.Base};Data Source={EmpresaTrabajo.Servidor};Connect Timeout={BDDTime}";

                if (!string.IsNullOrEmpty(EmpresaTrabajo.Servidor_RadioFrecuencia))
                {
                    ConexionRadioFrecuencia = $"Provider=SQLOLEDB.1;Persist Security Info=False;User ID={EmpresaTrabajo.Usuario_RadioFrecuencia};Password={EmpresaTrabajo.Clave_RadioFrecuencia};Initial Catalog={EmpresaTrabajo.Base_RadioFrecuencia};Data Source={EmpresaTrabajo.Servidor_RadioFrecuencia};Connect Timeout={BDDTime}";
                }

                if (!string.IsNullOrEmpty(EmpresaTrabajo.Servidor_GestionAlmacen))
                {
                    ConexionGestionAlmacen = $"Provider=SQLOLEDB.1;Persist Security Info=False;User ID={EmpresaTrabajo.Usuario_GestionAlmacen};Password={EmpresaTrabajo.Clave_GestionAlmacen};Initial Catalog={EmpresaTrabajo.Base_GestionAlmacen};Data Source={EmpresaTrabajo.Servidor_GestionAlmacen};Connect Timeout={BDDTime}";
                }

                Empresa = EmpresaTrabajo.Nombre;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Error al cargar parámetros de empresa: {ex.Message}",
                    "OK");
            }
        }

        /// <summary>
        /// Lee y verifica el archivo DSN
        /// Migrado desde VB6: Public Sub LeerDSN()
        /// </summary>
        public static void LeerDSN()
        {
            // En .NET MAUI no usamos archivos DSN, usamos cadenas de conexión directas
            // Este método se mantiene por compatibilidad pero no hace nada
            // Las cadenas de conexión se configuran en CargarParametrosEmpresa()
        }

        /// <summary>
        /// Obtiene el nombre del PC
        /// Migrado desde VB6: Public Function nombrePC() As String
        /// </summary>
        public static string NombrePC()
        {
#if WINDOWS
            var buffer = new System.Text.StringBuilder(256);
            int size = buffer.Capacity;
            if (GetComputerName(buffer, ref size))
            {
                return buffer.ToString();
            }
            return Environment.MachineName;
#else
            // En Android y otras plataformas
            return Environment.MachineName;
#endif
        }

        /// <summary>
        /// Verifica si hay múltiples instancias del programa
        /// Migrado desde VB6: Private Function InstanciasPrograma() As Boolean
        /// </summary>
        private static bool InstanciasPrograma()
        {
            // TODO: Implementar control de instancias cuando tengamos DAL
            // Por ahora permitimos todas las instancias
            return true;

            // La lógica original verificaba:
            // - Si Usuario.instancias = 0: permitir todas las instancias
            // - Si Usuario.nombrePC es Null: permitir en cualquier PC
            // - Si Usuario.nombrePC coincide con el PC actual: permitir
            // - Contar instancias activas en BD y comparar con Usuario.instancias
        }

        /// <summary>
        /// Obtiene el grupo de empresa del usuario
        /// Migrado desde VB6: Private Function DameGrupoEmpresaUsuario(usr As Integer) As Integer
        /// </summary>
        private static int DameGrupoEmpresaUsuario(int usr)
        {
            // TODO: Implementar cuando tengamos DAL
            // Por ahora retornamos -1 (sin grupo)
            return -1;
        }
    }
}
