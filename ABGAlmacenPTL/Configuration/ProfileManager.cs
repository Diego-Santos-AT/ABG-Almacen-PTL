using System.Runtime.InteropServices;

namespace ABGAlmacenPTL.Configuration
{
    /// <summary>
    /// Módulo genérico para las llamadas al API usando xxxProfileString
    /// Migrado desde VB6 Profile.bas
    /// Fecha inicio: 03/10/2001
    /// </summary>
    public static class ProfileManager
    {
        // Constantes de rutas y claves de registro
        public const string ProgramasDir = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion";
        public const string ClaveRegistroProgramas = "ProgramFilesDir";
        public const string DSNDir = "SOFTWARE\\ODBC\\ODBC.INI\\ODBC File DSN";
        public const string ClaveRegistroDSN = "DefaultDSNDir";

        #if WINDOWS
        // Declaraciones para 32/64 bits Windows
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetPrivateProfileString(
            string lpApplicationName,
            string? lpKeyName,
            string lpDefault,
            System.Text.StringBuilder lpReturnedString,
            int nSize,
            string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool WritePrivateProfileString(
            string lpApplicationName,
            string? lpKeyName,
            string? lpString,
            string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetPrivateProfileSection(
            string lpAppName,
            System.Text.StringBuilder lpReturnedString,
            int nSize,
            string lpFileName);
        #endif

        /// <summary>
        /// Lee un valor del fichero INI
        /// Migrado desde VB6: LeerIni
        /// </summary>
        /// <param name="lpFileName">La Aplicación (fichero INI)</param>
        /// <param name="lpAppName">La sección que suele estar entre corchetes</param>
        /// <param name="lpKeyName">Clave</param>
        /// <param name="vDefault">Valor opcional que devolverá si no se encuentra la clave</param>
        /// <returns>Valor leído o valor por defecto</returns>
        public static string LeerIni(string lpFileName, string lpAppName, string lpKeyName, string vDefault = "")
        {
            #if WINDOWS
            var sRetVal = new System.Text.StringBuilder(255);
            int result = GetPrivateProfileString(lpAppName, lpKeyName, vDefault, sRetVal, 255, lpFileName);
            
            if (result == 0)
            {
                return vDefault;
            }
            else
            {
                return sRetVal.ToString();
            }
            #else
            // En Android, usar Preferences o un sistema alternativo
            // Por ahora, devolver el valor por defecto
            return vDefault;
            #endif
        }

        /// <summary>
        /// Guarda los datos de configuración en el fichero INI
        /// Migrado desde VB6: GuardarIni
        /// </summary>
        /// <param name="lpFileName">La Aplicación (fichero INI)</param>
        /// <param name="lpAppName">La sección que suele estar entre corchetes</param>
        /// <param name="lpKeyName">Clave</param>
        /// <param name="lpString">Valor a guardar</param>
        public static void GuardarIni(string lpFileName, string lpAppName, string lpKeyName, string lpString)
        {
            #if WINDOWS
            WritePrivateProfileString(lpAppName, lpKeyName, lpString, lpFileName);
            #else
            // En Android, usar Preferences o un sistema alternativo
            // Por ahora, no hacer nada
            #endif
        }

        /// <summary>
        /// Lee una sección entera de un fichero INI
        /// Migrado desde VB6: LeerSeccionINI
        /// </summary>
        /// <param name="lpFileName">Nombre del fichero INI</param>
        /// <param name="lpAppName">Nombre de la sección a leer</param>
        /// <returns>Diccionario con las claves y valores de la sección</returns>
        public static Dictionary<string, string> LeerSeccionINI(string lpFileName, string lpAppName)
        {
            var resultado = new Dictionary<string, string>();

            #if WINDOWS
            var buffer = new System.Text.StringBuilder(32767);
            int result = GetPrivateProfileSection(lpAppName, buffer, 32767, lpFileName);
            
            if (result > 0)
            {
                string section = buffer.ToString();
                var lines = section.Split('\0', StringSplitOptions.RemoveEmptyEntries);
                
                foreach (var line in lines)
                {
                    int equalPos = line.IndexOf('=');
                    if (equalPos > 0)
                    {
                        string key = line.Substring(0, equalPos).Trim();
                        string value = line.Substring(equalPos + 1).Trim();
                        resultado[key] = value;
                    }
                }
            }
            #endif

            return resultado;
        }

        #region Registry Functions (Windows only)
        
        #if WINDOWS
        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RegOpenKeyEx(
            IntPtr hKey,
            string lpSubKey,
            int ulOptions,
            int samDesired,
            out IntPtr phkResult);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto)]
        private static extern int RegQueryValueEx(
            IntPtr hKey,
            string lpValueName,
            int lpReserved,
            out int lpType,
            System.Text.StringBuilder lpData,
            ref int lpcbData);

        [DllImport("advapi32.dll")]
        private static extern int RegCloseKey(IntPtr hKey);

        private static readonly IntPtr HKEY_CURRENT_USER = new IntPtr(unchecked((int)0x80000001));
        private static readonly IntPtr HKEY_LOCAL_MACHINE = new IntPtr(unchecked((int)0x80000002));
        private static readonly IntPtr HKEY_USERS = new IntPtr(unchecked((int)0x80000003));

        private const int KEY_QUERY_VALUE = 0x1;
        private const int KEY_SET_VALUE = 0x2;
        private const int KEY_CREATE_SUB_KEY = 0x4;
        private const int KEY_ENUMERATE_SUB_KEYS = 0x8;
        private const int KEY_NOTIFY = 0x10;
        private const int KEY_CREATE_LINK = 0x20;
        private const int READ_CONTROL = 0x20000;
        
        private const int KEY_ALL_ACCESS = KEY_QUERY_VALUE | KEY_SET_VALUE |
                                           KEY_CREATE_SUB_KEY | KEY_ENUMERATE_SUB_KEYS |
                                           KEY_NOTIFY | KEY_CREATE_LINK | READ_CONTROL;

        private const int ERROR_SUCCESS = 0;
        private const int REG_SZ = 1;
        private const int REG_DWORD = 4;

        /// <summary>
        /// Lee un valor del registro de Windows
        /// </summary>
        public static string? LeerRegistro(IntPtr hKey, string subKey, string valueName)
        {
            IntPtr keyHandle = IntPtr.Zero;
            try
            {
                if (RegOpenKeyEx(hKey, subKey, 0, KEY_QUERY_VALUE, out keyHandle) == ERROR_SUCCESS)
                {
                    int type;
                    int size = 1024;
                    var data = new System.Text.StringBuilder(size);
                    
                    if (RegQueryValueEx(keyHandle, valueName, 0, out type, data, ref size) == ERROR_SUCCESS)
                    {
                        return data.ToString();
                    }
                }
            }
            finally
            {
                if (keyHandle != IntPtr.Zero)
                {
                    RegCloseKey(keyHandle);
                }
            }
            return null;
        }
        #endif

        #endregion
    }
}
