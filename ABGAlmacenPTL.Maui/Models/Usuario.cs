// ***********************************************************************
// Nombre: Usuario.cs
// Modelo de Usuario - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     clCliente.cls y variables globales
// ***********************************************************************

namespace ABGAlmacenPTL.Maui.Models
{
    /// <summary>
    /// Clase Usuario - Equivalente a la clase/tipo Usuario de VB6
    /// </summary>
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        public int Nivel { get; set; }
        public bool Activo { get; set; }
    }

    /// <summary>
    /// Clase Empresa - Equivalente al tipo Empresa de VB6
    /// </summary>
    public class Empresa
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Ean { get; set; } = string.Empty;
    }

    /// <summary>
    /// Clase PuestoTrabajo - Equivalente a wPuestoTrabajo de VB6
    /// </summary>
    public class PuestoTrabajo
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string Corto { get; set; } = string.Empty;
        public int Tipo { get; set; }
        public int Usuario { get; set; }
        public int Impresora { get; set; }
        public int Color { get; set; }
        public int Grupo { get; set; }
        public string Mac { get; set; } = string.Empty;
    }

    /// <summary>
    /// Clase Impresora - Equivalente a wImpresora de VB6
    /// </summary>
    public class Impresora
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Puerto { get; set; } = string.Empty;
        public bool Activa { get; set; }
    }
}
