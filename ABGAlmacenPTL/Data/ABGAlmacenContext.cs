using Microsoft.EntityFrameworkCore;

namespace ABGAlmacenPTL.Data;

/// <summary>
/// Contexto de Entity Framework Core para ABG Almacén PTL
/// Migrado desde VB6 ADO/Recordset a EF Core
/// Gestiona acceso a base de datos SQL Server
/// </summary>
public class ABGAlmacenContext : DbContext
{
    public ABGAlmacenContext(DbContextOptions<ABGAlmacenContext> options)
        : base(options)
    {
    }

    // DbSets - Tablas principales
    public DbSet<Models.Articulo> Articulos { get; set; }
    public DbSet<Models.BAC> BACs { get; set; }
    public DbSet<Models.Ubicacion> Ubicaciones { get; set; }
    public DbSet<Models.Caja> Cajas { get; set; }
    public DbSet<Models.TipoCaja> TiposCaja { get; set; }
    public DbSet<Models.Puesto> Puestos { get; set; }
    public DbSet<Models.Usuario> Usuarios { get; set; }

    // Tablas de unión (muchos-a-muchos)
    public DbSet<Models.BACArticulo> BACArticulos { get; set; }
    public DbSet<Models.CajaArticulo> CajaArticulos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Articulo
        modelBuilder.Entity<Models.Articulo>()
            .HasIndex(a => a.EAN13);

        // Configuración de BAC
        modelBuilder.Entity<Models.BAC>()
            .HasOne(b => b.Ubicacion)
            .WithMany(u => u.BACs)
            .HasForeignKey(b => b.CodigoUbicacion)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Models.BAC>()
            .HasIndex(b => b.CodigoUbicacion);

        modelBuilder.Entity<Models.BAC>()
            .HasIndex(b => b.Estado);

        // Configuración de Ubicacion
        modelBuilder.Entity<Models.Ubicacion>()
            .HasIndex(u => new { u.Almacen, u.Bloque, u.Fila, u.Altura })
            .IsUnique();

        // Configuración de Caja
        modelBuilder.Entity<Models.Caja>()
            .HasOne(c => c.TipoCaja)
            .WithMany(t => t.Cajas)
            .HasForeignKey(c => c.TipoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Models.Caja>()
            .HasIndex(c => c.Estado);

        modelBuilder.Entity<Models.Caja>()
            .HasIndex(c => c.FechaCreacion);

        // Configuración de BACArticulo (tabla de unión)
        modelBuilder.Entity<Models.BACArticulo>()
            .HasOne(ba => ba.BAC)
            .WithMany(b => b.BACArticulos)
            .HasForeignKey(ba => ba.CodigoBAC)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Models.BACArticulo>()
            .HasOne(ba => ba.Articulo)
            .WithMany(a => a.BACArticulos)
            .HasForeignKey(ba => ba.CodigoArticulo)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Models.BACArticulo>()
            .HasIndex(ba => new { ba.CodigoBAC, ba.CodigoArticulo })
            .IsUnique();

        // Configuración de CajaArticulo (tabla de unión)
        modelBuilder.Entity<Models.CajaArticulo>()
            .HasOne(ca => ca.Caja)
            .WithMany(c => c.CajaArticulos)
            .HasForeignKey(ca => ca.SSCC)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Models.CajaArticulo>()
            .HasOne(ca => ca.Articulo)
            .WithMany(a => a.CajaArticulos)
            .HasForeignKey(ca => ca.CodigoArticulo)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Models.CajaArticulo>()
            .HasIndex(ca => new { ca.SSCC, ca.CodigoArticulo })
            .IsUnique();

        // Configuración de Puesto
        modelBuilder.Entity<Models.Puesto>()
            .HasIndex(p => p.Numero)
            .IsUnique();

        // Configuración de Usuario
        modelBuilder.Entity<Models.Usuario>()
            .HasIndex(u => u.NombreUsuario)
            .IsUnique();

        modelBuilder.Entity<Models.Usuario>()
            .HasIndex(u => u.Email);

        // Datos de prueba (seed data) - opcional
        SeedData(modelBuilder);
    }

    /// <summary>
    /// Datos iniciales para desarrollo/pruebas
    /// </summary>
    private void SeedData(ModelBuilder modelBuilder)
    {
        // TiposCaja iniciales
        modelBuilder.Entity<Models.TipoCaja>().HasData(
            new Models.TipoCaja
            {
                TipoId = 1,
                Nombre = "Caja Pequeña",
                Descripcion = "Caja estándar pequeña",
                PesoMaximo = 10.0m,
                VolumenMaximo = 0.1m,
                Activo = true
            },
            new Models.TipoCaja
            {
                TipoId = 2,
                Nombre = "Caja Mediana",
                Descripcion = "Caja estándar mediana",
                PesoMaximo = 25.0m,
                VolumenMaximo = 0.25m,
                Activo = true
            },
            new Models.TipoCaja
            {
                TipoId = 3,
                Nombre = "Caja Grande",
                Descripcion = "Caja estándar grande",
                PesoMaximo = 50.0m,
                VolumenMaximo = 0.5m,
                Activo = true
            }
        );

        // Puestos de trabajo PTL con colores VB6
        modelBuilder.Entity<Models.Puesto>().HasData(
            new Models.Puesto
            {
                PuestoId = 1,
                Numero = 1,
                Nombre = "Puesto 1",
                Color = Models.ColorPuesto.Rojo,
                Activo = true
            },
            new Models.Puesto
            {
                PuestoId = 2,
                Numero = 2,
                Nombre = "Puesto 2",
                Color = Models.ColorPuesto.Verde,
                Activo = true
            },
            new Models.Puesto
            {
                PuestoId = 3,
                Numero = 3,
                Nombre = "Puesto 3",
                Color = Models.ColorPuesto.Azul,
                Activo = true
            },
            new Models.Puesto
            {
                PuestoId = 4,
                Numero = 4,
                Nombre = "Puesto 4",
                Color = Models.ColorPuesto.Amarillo,
                Activo = true
            },
            new Models.Puesto
            {
                PuestoId = 5,
                Numero = 5,
                Nombre = "Puesto 5",
                Color = Models.ColorPuesto.Magenta,
                Activo = true
            }
        );
    }
}
