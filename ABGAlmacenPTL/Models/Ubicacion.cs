using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABGAlmacenPTL.Models;

/// <summary>
/// Modelo de datos para Ubicaciones PTL
/// Migrado desde VB6 - representa posiciones físicas en el almacén
/// Formato: 12 dígitos AAABBBCCCDD (Almacén.Bloque.Fila.Altura)
/// </summary>
[Table("Ubicaciones")]
public class Ubicacion
{
    [Key]
    [MaxLength(12)]
    public string CodigoUbicacion { get; set; } = string.Empty;

    [Required]
    [Range(0, 999)]
    public int Almacen { get; set; }

    [Required]
    [Range(0, 999)]
    public int Bloque { get; set; }

    [Required]
    [Range(0, 999)]
    public int Fila { get; set; }

    [Required]
    [Range(0, 99)]
    public int Altura { get; set; }

    [MaxLength(200)]
    public string? Descripcion { get; set; }

    public bool Activa { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    // Propiedad calculada - formato VB6: 000.000.000.00
    [NotMapped]
    public string CodigoFormateado => $"{Almacen:D3}.{Bloque:D3}.{Fila:D3}.{Altura:D2}";

    // Navigation properties
    public ICollection<BAC> BACs { get; set; } = new List<BAC>();
}
