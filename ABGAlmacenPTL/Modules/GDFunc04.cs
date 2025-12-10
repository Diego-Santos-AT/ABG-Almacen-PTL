namespace ABGAlmacenPTL.Modules
{
    /// <summary>
    /// Módulo de funciones de utilidad para navegación y selección
    /// Migrado desde VB6 GDFunc04.bas
    /// </summary>
    public static class GDFunc04
    {
        /// <summary>
        /// Tipo de Datos para Registro Seleccionado
        /// Migrado desde VB6 Type Registro_Seleccionado
        /// </summary>
        public class RegistroSeleccionado
        {
            // --- CODIGO
            public long iCodigo { get; set; }
            
            // --- DESCRIPCION
            public string sDescripcion { get; set; } = string.Empty;
        }

        /// <summary>
        /// Tipo de Datos para contener las ubicaciones por defecto de empresa desglosada
        /// Migrado desde VB6 Type Desglose_Ubicacion
        /// </summary>
        public class DesgloseUbicacion
        {
            public int Almacen_Fisico { get; set; }
            public int Almacen_Logico { get; set; }
            public int Bloque { get; set; }
            public int Fila { get; set; }
            public int Altura { get; set; }
        }

        /// <summary>
        /// Función para simular la navegación sobre un recordset con los Movimientos
        /// tradicionales de Primero, Anterior, Siguiente y Ultimo
        /// Migrado desde VB6: DesplazaRegistro2
        /// Creado: 10/05/02
        /// </summary>
        /// <param name="tabla">Nombre de la tabla</param>
        /// <param name="campo">Campo por el que navegar</param>
        /// <param name="codigoActual">Código actual</param>
        /// <param name="tipoDesplazamiento">Tipo: P=Primero, A=Anterior, S=Siguiente, U=Ultimo</param>
        /// <param name="condicion">Condición WHERE opcional</param>
        /// <returns>Nuevo código después del desplazamiento</returns>
        public static string DesplazaRegistro2(
            string tabla, 
            string campo, 
            string codigoActual, 
            string tipoDesplazamiento, 
            string? condicion = null)
        {
            string cond1;
            string cond2;
            
            if (string.IsNullOrEmpty(condicion))
            {
                cond1 = "";
                cond2 = "";
            }
            else
            {
                cond1 = $" WHERE {condicion}";
                cond2 = $" AND {condicion}";
            }
            
            string sql = tipoDesplazamiento.ToUpper() switch
            {
                "P" => // Primero
                    $"SELECT MIN({campo}) as Registro FROM {tabla}{cond1}",
                
                "A" => // Anterior
                    string.IsNullOrEmpty(cond1)
                        ? $"SELECT MAX({campo}) as Registro FROM {tabla} WHERE {campo} < {codigoActual}"
                        : $"SELECT MAX({campo}) as Registro FROM {tabla}{cond1} AND {campo} < {codigoActual}",
                
                "S" => // Siguiente
                    string.IsNullOrEmpty(cond1)
                        ? $"SELECT MIN({campo}) as Registro FROM {tabla} WHERE {campo} > {codigoActual}"
                        : $"SELECT MIN({campo}) as Registro FROM {tabla}{cond1} AND {campo} > {codigoActual}",
                
                "U" => // Ultimo
                    $"SELECT MAX({campo}) as Registro FROM {tabla}{cond1}",
                
                _ => throw new ArgumentException($"Tipo de desplazamiento no válido: {tipoDesplazamiento}")
            };
            
            // En la implementación real, esto ejecutaría la consulta contra la base de datos
            // Por ahora, devolvemos el código actual como placeholder
            // TODO: Implementar ejecución de consulta cuando se tenga el Data Access Layer
            return codigoActual;
        }

        /// <summary>
        /// Ejecuta una consulta SQL y devuelve el resultado
        /// Migrado desde VB6: EjecutaConsulta
        /// </summary>
        /// <param name="sql">Consulta SQL a ejecutar</param>
        /// <param name="valorDefecto">Valor por defecto si no hay resultado</param>
        /// <returns>Resultado de la consulta o valor por defecto</returns>
        private static string EjecutaConsulta(string sql, string valorDefecto)
        {
            // TODO: Implementar ejecución de consulta cuando se tenga el Data Access Layer
            // Por ahora, devolver el valor por defecto
            return valorDefecto;
        }

        /// <summary>
        /// Desglosar un código de ubicación completo en sus componentes
        /// </summary>
        /// <param name="ubicacionCompleta">Código de ubicación completo</param>
        /// <returns>Objeto con los componentes desglosados</returns>
        public static DesgloseUbicacion? DesglosaUbicacion(string ubicacionCompleta)
        {
            if (string.IsNullOrEmpty(ubicacionCompleta))
                return null;
                
            // Implementación basada en el formato de ubicación del sistema VB6
            // TODO: Ajustar según el formato real de ubicaciones
            var desglose = new DesgloseUbicacion();
            
            // Placeholder - ajustar según formato real
            return desglose;
        }
    }
}
