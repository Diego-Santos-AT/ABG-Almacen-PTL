using Microsoft.EntityFrameworkCore;
using ABGAlmacenPTL.Models.Config;

namespace ABGAlmacenPTL.Data;

/// <summary>
/// Contexto de Entity Framework Core para Config DB
/// Migrado desde VB6 - base de datos de configuración (GROOT/BDDServLocal)
/// Tablas: gdeusr, gdeemp, gdusremp
/// </summary>
public class ConfigContext : DbContext
{
    public ConfigContext(DbContextOptions<ConfigContext> options)
        : base(options)
    {
    }

    // DbSets - Tablas de Config
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Empresa> Empresas { get; set; }
    public DbSet<UsuarioEmpresa> UsuariosEmpresas { get; set; }
    public DbSet<PuestoTrabajo> PuestosTrabajo { get; set; }
    public DbSet<Impresora> Impresoras { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Usuario
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("gdeusr");
            entity.HasKey(e => e.UsuarioId);
        });

        // Configuración de Empresa
        modelBuilder.Entity<Empresa>(entity =>
        {
            entity.ToTable("gdeemp");
            entity.HasKey(e => e.CodigoEmpresa);
        });

        // Configuración de UsuarioEmpresa
        modelBuilder.Entity<UsuarioEmpresa>(entity =>
        {
            entity.ToTable("gdusremp");
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Empresa)
                .WithMany()
                .HasForeignKey(e => e.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de PuestoTrabajo
        modelBuilder.Entity<PuestoTrabajo>(entity =>
        {
            entity.ToTable("gdepue");
            entity.HasKey(e => e.CodigoPuesto);
            
            entity.HasOne(e => e.Impresora)
                .WithMany()
                .HasForeignKey(e => e.CodigoImpresora)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de Impresora
        modelBuilder.Entity<Impresora>(entity =>
        {
            entity.ToTable("gdeimp");
            entity.HasKey(e => e.CodigoImpresora);
        });
    }
}
