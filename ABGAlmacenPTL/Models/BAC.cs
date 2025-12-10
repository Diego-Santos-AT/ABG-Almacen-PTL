using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABGAlmacenPTL.Models;

/// <summary>
/// Estado del BAC - migrado desde VB6
/// ABIERTO = 0, CERRADO = 1
/// </summary>
public enum EstadoBAC
{
    Abierto = 0,
    Cerrado = 1
}

/// <summary>
/// Modelo de datos para BAC (contenedor de almacenamiento PTL)
/// Migrado desde VB6 - representa contenedores en el sistema PTL
/// </summary>
[Table("BACs")]
public class BAC
{
    [Key]
    [MaxLength(50)]
    public string CodigoBAC { get; set; } = string.Empty;

    [Required]
    public EstadoBAC Estado { get; set; } = EstadoBAC.Abierto;

    [MaxLength(12)]
    public string? CodigoUbicacion { get; set; }

    [MaxLength(50)]
    public string? Grupo { get; set; }

    [MaxLength(50)]
    public string? Tablilla { get; set; }

    public int Unidades { get; set; } = 0;

    [Column(TypeName = "decimal(10,3)")]
    public decimal? Peso { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal? Volumen { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? FechaModificacion { get; set; }

    // Navigation properties
    [ForeignKey(nameof(CodigoUbicacion))]
    public Ubicacion? Ubicacion { get; set; }

    public ICollection<BACArticulo> BACArticulos { get; set; } = new List<BACArticulo>();
}

/// <summary>
/// Tabla de unión para relación muchos-a-muchos BAC-Artículo
/// </summary>
[Table("BAC_Articulos")]
public class BACArticulo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string CodigoBAC { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CodigoArticulo { get; set; } = string.Empty;

    public int Cantidad { get; set; } = 1;

    // Navigation properties
    [ForeignKey(nameof(CodigoBAC))]
    public BAC BAC { get; set; } = null!;

    [ForeignKey(nameof(CodigoArticulo))]
    public Articulo Articulo { get; set; } = null!;
}
