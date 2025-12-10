using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABGAlmacenPTL.Models;

/// <summary>
/// Colores de puestos de trabajo PTL - migrado desde VB6
/// Matching exacto con colores VB6
/// </summary>
public enum ColorPuesto
{
    Rojo = 0,      // Red - &HFF&
    Verde = 1,     // Green - &HFF00&
    Azul = 2,      // Blue - &HFF0000
    Amarillo = 3,  // Yellow - &HFFFF&
    Magenta = 4    // Magenta - &HFF00FF
}

/// <summary>
/// Modelo de datos para Puestos de Trabajo PTL
/// Migrado desde VB6 - representa estaciones de trabajo con luces de colores
/// </summary>
[Table("Puestos")]
public class Puesto
{
    [Key]
    public int PuestoId { get; set; }

    [Required]
    public int Numero { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public ColorPuesto Color { get; set; }

    public bool Activo { get; set; } = true;

    [MaxLength(200)]
    public string? Descripcion { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;
}
