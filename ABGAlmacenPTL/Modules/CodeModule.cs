using System.IO.Compression;

namespace ABGAlmacenPTL.Modules
{
    /// <summary>
    /// Módulo genérico de funciones auxiliares
    /// Migrado desde VB6 CodeModule.bas
    /// Contiene funciones para ZIP/UNZIP y otras utilidades
    /// </summary>
    public static class CodeModule
    {
        /// <summary>
        /// Comprime archivos en formato ZIP
        /// Migrado desde VB6: VBZip
        /// </summary>
        /// <param name="sourceDirectory">Directorio de origen</param>
        /// <param name="zipFileName">Nombre del archivo ZIP de destino</param>
        /// <returns>True si se completó exitosamente</returns>
        public static bool VBZip(string sourceDirectory, string zipFileName)
        {
            try
            {
                // En .NET moderno usamos System.IO.Compression
                if (File.Exists(zipFileName))
                {
                    File.Delete(zipFileName);
                }
                
                ZipFile.CreateFromDirectory(sourceDirectory, zipFileName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Descomprime archivos ZIP
        /// Migrado desde VB6: VBUnzip
        /// </summary>
        /// <param name="zipFileName">Nombre del archivo ZIP</param>
        /// <param name="extractDirectory">Directorio de extracción</param>
        /// <returns>True si se completó exitosamente</returns>
        public static bool VBUnzip(string zipFileName, string extractDirectory)
        {
            try
            {
                // En .NET moderno usamos System.IO.Compression
                if (!Directory.Exists(extractDirectory))
                {
                    Directory.CreateDirectory(extractDirectory);
                }
                
                ZipFile.ExtractToDirectory(zipFileName, extractDirectory, overwriteFiles: true);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Recorta espacios en blanco de una cadena
        /// Migrado desde VB6: szTrim
        /// </summary>
        /// <param name="szString">Cadena a recortar</param>
        /// <returns>Cadena recortada</returns>
        public static string SzTrim(string szString)
        {
            if (string.IsNullOrEmpty(szString))
            {
                return string.Empty;
            }
            
            // Eliminar espacios en blanco y caracteres nulos
            return szString.Trim().TrimEnd('\0');
        }
    }
}
