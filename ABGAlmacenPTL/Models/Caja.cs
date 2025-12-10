using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABGAlmacenPTL.Models;

/// <summary>
/// Estado de la Caja - migrado desde VB6
/// Abierta = en proceso, Cerrada = finalizada
/// </summary>
public enum EstadoCaja
{
    Abierta = 0,
    Cerrada = 1
}

/// <summary>
/// Modelo de datos para Cajas (contenedores de empaquetado)
/// Migrado desde VB6 - representa cajas con código SSCC
/// SSCC: 18 dígitos con dígito de control (GS1 estándar)
/// </summary>
[Table("Cajas")]
public class Caja
{
    [Key]
    [MaxLength(18)]
    public string SSCC { get; set; } = string.Empty;

    [Required]
    public int TipoId { get; set; }

    [Required]
    public EstadoCaja Estado { get; set; } = EstadoCaja.Abierta;

    public int Unidades { get; set; } = 0;

    [Column(TypeName = "decimal(10,3)")]
    public decimal? Peso { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal? Volumen { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? FechaCierre { get; set; }

    [MaxLength(200)]
    public string? Observaciones { get; set; }

    // Navigation properties
    [ForeignKey(nameof(TipoId))]
    public TipoCaja TipoCaja { get; set; } = null!;

    public ICollection<CajaArticulo> CajaArticulos { get; set; } = new List<CajaArticulo>();
}

/// <summary>
/// Tabla de unión para relación muchos-a-muchos Caja-Artículo
/// </summary>
[Table("Caja_Articulos")]
public class CajaArticulo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(18)]
    public string SSCC { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string CodigoArticulo { get; set; } = string.Empty;

    public int Cantidad { get; set; } = 1;

    // Navigation properties
    [ForeignKey(nameof(SSCC))]
    public Caja Caja { get; set; } = null!;

    [ForeignKey(nameof(CodigoArticulo))]
    public Articulo Articulo { get; set; } = null!;
}
