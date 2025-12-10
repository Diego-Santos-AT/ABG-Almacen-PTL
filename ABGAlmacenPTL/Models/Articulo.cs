using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABGAlmacenPTL.Models;

/// <summary>
/// Modelo de datos para Artículos/Productos
/// Migrado desde VB6 - representa productos en el sistema PTL
/// </summary>
[Table("Articulos")]
public class Articulo
{
    [Key]
    [MaxLength(50)]
    public string CodigoArticulo { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(13)]
    public string? EAN13 { get; set; }

    [MaxLength(50)]
    public string? CodigoSTD { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal? Peso { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal? Volumen { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? FechaModificacion { get; set; }

    // Navigation properties
    public ICollection<BACArticulo> BACArticulos { get; set; } = new List<BACArticulo>();
    public ICollection<CajaArticulo> CajaArticulos { get; set; } = new List<CajaArticulo>();
}
