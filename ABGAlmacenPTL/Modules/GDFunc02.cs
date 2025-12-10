namespace ABGAlmacenPTL.Modules
{
    /// <summary>
    /// Módulo de funciones de relación de datos
    /// Migrado desde VB6 GDFunc02.bas
    /// </summary>
    public static partial class GDFunc02
    {
        /// <summary>
        /// Buscar en un array de dos dimensiones por la primera para devolver el valor de la segunda
        /// Migrado desde VB6: wfBuscarEnArray
        /// Creación: 16/10/03
        /// Se usa desde wfImprimir_Informe para dar el valor de la propiedad de Crystal
        /// </summary>
        /// <param name="vArray">Array en el que buscar</param>
        /// <param name="vValorBuscar">Valor a buscar en la primera dimensión</param>
        /// <returns>Valor encontrado o string vacío</returns>
        public static object wfBuscarEnArray(object[][] vArray, object vValorBuscar)
        {
            // Si no encuentra nada devuelve ""
            object resultado = "";
            
            foreach (var item in vArray)
            {
                if (item.Length >= 2 && Equals(item[0], vValorBuscar))
                {
                    resultado = item[1];
                    break;
                }
            }
            
            return resultado;
        }

        /// <summary>
        /// Obtiene la fecha y hora del sistema
        /// Migrado desde VB6: Dame_FechaHora_Sistema
        /// </summary>
        /// <returns>Fecha y hora actual</returns>
        public static DateTime Dame_FechaHora_Sistema()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Quita los corchetes de una cadena (usado para nombres de servidor SQL)
        /// Migrado desde VB6: wfQuitarCorchetes
        /// </summary>
        /// <param name="texto">Texto con posibles corchetes</param>
        /// <returns>Texto sin corchetes</returns>
        public static string wfQuitarCorchetes(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return texto;
                
            return texto.Replace("[", "").Replace("]", "");
        }

        /// <summary>
        /// Convierte un valor a cadena manejando nulos
        /// </summary>
        /// <param name="valor">Valor a convertir</param>
        /// <returns>Cadena o vacío si es nulo</returns>
        public static string ConvertirACadena(object? valor)
        {
            if (valor == null || valor == DBNull.Value)
                return string.Empty;
                
            return valor.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Convierte un valor a entero manejando nulos
        /// </summary>
        /// <param name="valor">Valor a convertir</param>
        /// <param name="valorDefecto">Valor por defecto si es nulo</param>
        /// <returns>Entero o valor por defecto</returns>
        public static int ConvertirAEntero(object? valor, int valorDefecto = 0)
        {
            if (valor == null || valor == DBNull.Value)
                return valorDefecto;
                
            if (valor is int i)
                return i;
                
            if (int.TryParse(valor.ToString(), out int resultado))
                return resultado;
                
            return valorDefecto;
        }

        /// <summary>
        /// Convierte un valor a booleano manejando nulos y valores numéricos VB6
        /// En VB6: 0 = False, cualquier otro número = True
        /// </summary>
        /// <param name="valor">Valor a convertir</param>
        /// <returns>Booleano</returns>
        public static bool ConvertirABooleano(object? valor)
        {
            if (valor == null || valor == DBNull.Value)
                return false;
                
            if (valor is bool b)
                return b;
                
            if (valor is int i)
                return i != 0;
                
            if (bool.TryParse(valor.ToString(), out bool resultado))
                return resultado;
                
            // Intentar como número
            if (int.TryParse(valor.ToString(), out int num))
                return num != 0;
                
            return false;
        }
    }
}
