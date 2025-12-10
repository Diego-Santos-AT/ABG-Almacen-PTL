using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABGAlmacenPTL.Models;

/// <summary>
/// Modelo de datos para Tipos de Caja
/// Migrado desde VB6 - representa tipos/categorías de contenedores
/// </summary>
[Table("TiposCaja")]
public class TipoCaja
{
    [Key]
    public int TipoId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descripcion { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal? PesoMaximo { get; set; }

    [Column(TypeName = "decimal(10,3)")]
    public decimal? VolumenMaximo { get; set; }

    public bool Activo { get; set; } = true;

    // Navigation properties
    public ICollection<Caja> Cajas { get; set; } = new List<Caja>();
}
