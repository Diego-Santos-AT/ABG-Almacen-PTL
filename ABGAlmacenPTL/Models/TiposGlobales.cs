namespace ABGAlmacenPTL.Models
{
    /// <summary>
    /// Tipo de datos para Opción
    /// Migrado desde VB6 Type TipoOpcion
    /// </summary>
    public class TipoOpcion
    {
        public string Menu { get; set; } = string.Empty;
        public string Formulario { get; set; } = string.Empty;
    }

    /// <summary>
    /// Tipo de datos para menu
    /// Migrado desde VB6 Type TipoMenu
    /// </summary>
    public class TipoMenu
    {
        // Nombre de menu
        public string Nombre { get; set; } = string.Empty;
        
        // Lista de menu Opciones
        public List<TipoOpcion> Opcion { get; set; } = new List<TipoOpcion>();
        
        // Lista de menu Listados
        public List<TipoOpcion> Listado { get; set; } = new List<TipoOpcion>();
        
        // Lista de menu Especiales
        public List<TipoOpcion> Especial { get; set; } = new List<TipoOpcion>();
    }

    /// <summary>
    /// Tipo de Datos para Usuario
    /// Migrado desde VB6 Type TipoUsuario
    /// </summary>
    public class TipoUsuario
    {
        // Identificador de usuario
        public int Id { get; set; }
        
        // Nombre Usuario
        public string Nombre { get; set; } = string.Empty;
        
        // Instancias
        public int Instancias { get; set; }
        
        // Nombre del PC que puede arrancar(Null = todos)
        public object? NombrePC { get; set; }
    }

    /// <summary>
    /// Tipo de Datos para Puesto de trabajo
    /// Migrado desde VB6 Type PuestoTrabajo
    /// </summary>
    public class PuestoTrabajo
    {
        // Identificador de puesto de trabajo
        public int Id { get; set; }
        
        // Nombre del puesto de trabajo
        public string Nombre { get; set; } = string.Empty;
        
        // Nombre corto del puesto de trabajo
        public string NombreCorto { get; set; } = string.Empty;
        
        // Identificador de la impresora asignada
        public int Impresora { get; set; }
        
        // Nombre de la impresora
        public string NombreImpresora { get; set; } = string.Empty;
        
        // Tipo de Lenguaje de la impresora: TEC, ZEBRA, OTRO
        public string TipoImpresora { get; set; } = string.Empty;
    }
}
