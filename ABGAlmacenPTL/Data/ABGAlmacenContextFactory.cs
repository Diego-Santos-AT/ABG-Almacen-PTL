using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ABGAlmacenPTL.Data;

/// <summary>
/// Factory para crear ABGAlmacenContext en tiempo de diseño
/// Necesario para que EF Core tools (dotnet ef migrations) funcione correctamente
/// </summary>
public class ABGAlmacenContextFactory : IDesignTimeDbContextFactory<ABGAlmacenContext>
{
    public ABGAlmacenContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ABGAlmacenContext>();
        
        // Usar connection string de LocalDB para migraciones
        // En producción, esto se obtiene de appsettings.json o User Secrets
        optionsBuilder.UseSqlServer(
            "Server=(localdb)\\mssqllocaldb;Database=ABGAlmacenPTL;Trusted_Connection=true;MultipleActiveResultSets=true"
        );

        return new ABGAlmacenContext(optionsBuilder.Options);
    }
}
