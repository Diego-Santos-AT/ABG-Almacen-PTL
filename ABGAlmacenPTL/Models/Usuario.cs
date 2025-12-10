using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABGAlmacenPTL.Models;

/// <summary>
/// Modelo de datos para Usuarios del sistema
/// Migrado desde VB6 - representa usuarios con acceso al sistema PTL
/// </summary>
[Table("Usuarios")]
public class Usuario
{
    [Key]
    public int UsuarioId { get; set; }

    [Required]
    [MaxLength(50)]
    public string NombreUsuario { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string NombreCompleto { get; set; } = string.Empty;

    [MaxLength(200)]
    [EmailAddress]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Empresa { get; set; }

    [MaxLength(50)]
    public string? Rol { get; set; }

    public bool Activo { get; set; } = true;

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    public DateTime? UltimoAcceso { get; set; }
}
