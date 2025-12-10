using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABGAlmacenPTL.Models.Config;

/// <summary>
/// Modelo para tabla gdeusr (Usuarios del sistema Config)
/// Migrado desde VB6 - tabla en base de datos Config
/// </summary>
[Table("gdeusr")]
public class Usuario
{
    [Key]
    [Column("usuide")]
    public int UsuarioId { get; set; }
    
    [Required]
    [Column("usunom")]
    [MaxLength(50)]
    public string NombreUsuario { get; set; } = string.Empty;
    
    [Column("usucon")]
    [MaxLength(255)]
    public string? Contraseña { get; set; }
    
    [Column("usuins")]
    public int? Instancias { get; set; }
    
    [Column("usunpc")]
    [MaxLength(100)]
    public string? NombrePC { get; set; }
}

/// <summary>
/// Modelo para tabla gdeemp (Empresas)
/// Migrado desde VB6 - tabla en base de datos Config
/// </summary>
[Table("gdeemp")]
public class Empresa
{
    [Key]
    [Column("empcod")]
    public int CodigoEmpresa { get; set; }
    
    [Required]
    [Column("empnom")]
    [MaxLength(200)]
    public string NombreEmpresa { get; set; } = string.Empty;
    
    // Conexión a Gestion DB
    [Column("empser")]
    [MaxLength(100)]
    public string? Servidor { get; set; }
    
    [Column("empbdd")]
    [MaxLength(100)]
    public string? BaseDatos { get; set; }
    
    [Column("empusr")]
    [MaxLength(50)]
    public string? Usuario { get; set; }
    
    [Column("empkey")]
    [MaxLength(255)]
    public string? Clave { get; set; }
    
    // Conexión a GestionAlmacen DB (PTL)
    [Column("empservidorga")]
    [MaxLength(100)]
    public string? ServidorGA { get; set; }
    
    [Column("empbga")]
    [MaxLength(100)]
    public string? BaseDatosGA { get; set; }
    
    [Column("empuga")]
    [MaxLength(50)]
    public string? UsuarioGA { get; set; }
    
    [Column("empkga")]
    [MaxLength(255)]
    public string? ClaveGA { get; set; }
    
    [Column("empcif")]
    [MaxLength(20)]
    public string? CIF { get; set; }
    
    [Column("emplog")]
    [MaxLength(255)]
    public string? Logo { get; set; }
    
    [Column("empact")]
    public bool? Activa { get; set; }
}

/// <summary>
/// Modelo para tabla gdusremp (Relación Usuario-Empresa)
/// Migrado desde VB6 - tabla en base de datos Config
/// </summary>
[Table("gdusremp")]
public class UsuarioEmpresa
{
    [Key]
    [Column("useide")]
    public int Id { get; set; }
    
    [Column("useusr")]
    public int UsuarioId { get; set; }
    
    [Column("useemp")]
    public int EmpresaId { get; set; }
    
    // Navigation properties
    [ForeignKey(nameof(UsuarioId))]
    public Usuario? Usuario { get; set; }
    
    [ForeignKey(nameof(EmpresaId))]
    public Empresa? Empresa { get; set; }
}

/// <summary>
/// Modelo para tabla gdepue (Puestos de Trabajo)
/// Migrado desde VB6 - tabla en base de datos Config
/// </summary>
[Table("gdepue")]
public class PuestoTrabajo
{
    [Key]
    [Column("puecod")]
    public int CodigoPuesto { get; set; }
    
    [Required]
    [Column("puedes")]
    [MaxLength(30)]
    public string Descripcion { get; set; } = string.Empty;
    
    [Column("puecor")]
    [MaxLength(10)]
    public string? DescripcionCorta { get; set; }
    
    [Column("impcod")]
    public int? CodigoImpresora { get; set; }
    
    // Navigation property
    [ForeignKey(nameof(CodigoImpresora))]
    public Impresora? Impresora { get; set; }
}

/// <summary>
/// Modelo para tabla gdeimp (Impresoras)
/// Migrado desde VB6 - tabla en base de datos Config
/// </summary>
[Table("gdeimp")]
public class Impresora
{
    [Key]
    [Column("impcod")]
    public int CodigoImpresora { get; set; }
    
    [Required]
    [Column("impnom")]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;
    
    [Column("implen")]
    [MaxLength(10)]
    public string? Lenguaje { get; set; }
}
