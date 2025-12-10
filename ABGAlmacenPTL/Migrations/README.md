# Entity Framework Core Migrations

## ⚠️ Important: MAUI Multi-Targeting Limitation

EF Core command-line tools have a known issue with multi-targeted MAUI projects. The migrations must be created using one of these workarounds:

### ✅ Option 1: Manual SQL Script (Current Approach)

A SQL migration script has been created manually at `InitialCreate.sql`. This script:
- Creates all tables with proper schema
- Adds indexes and foreign keys
- Includes seed data for TiposCaja and Puestos

**To apply the migration:**
```bash
# Using sqlcmd (Windows/Linux)
sqlcmd -S (localdb)\mssqllocaldb -d ABGAlmacenPTL -i Migrations/InitialCreate.sql

# Or using SQL Server Management Studio (SSMS)
# Open InitialCreate.sql and execute against your database
```

**To load additional seed data:**
- Use the `Data/SeedData.cs` class in your application startup
- Or run the INSERT statements from SeedData.cs manually

### Option 2: Temporary Single-Target (For EF Core CLI)

1. **Temporarily edit ABGAlmacenPTL.csproj** to single-target:
```xml
<TargetFrameworks>net10.0-android</TargetFrameworks>
<!-- Comment out: <TargetFrameworks Condition="...">$(TargetFrameworks);net10.0-windows...</TargetFrameworks> -->
```

2. **Create migration**:
```bash
cd ABGAlmacenPTL
dotnet ef migrations add InitialCreate
```

3. **Restore multi-targeting** in .csproj after migration is created

### Option 3: Use Package Manager Console in Visual Studio

If using Visual Studio, you can use Package Manager Console which handles multi-targeting better:
```powershell
Add-Migration InitialCreate
Update-Database
```

### Option 4: SQL Script Generation (Production)

For production deployments, generate SQL scripts:
```bash
# After migration is created via Option 2 or 3
dotnet ef migrations script --output migration.sql
```

## How to Apply Migrations

### 1. Create Initial Migration (Use Option 1 or 2 above)
```bash
cd ABGAlmacenPTL
dotnet ef migrations add InitialCreate
```

### 2. Apply Migration to Database
```bash
dotnet ef database update
```

### 3. Add Seed Data
In your `App.xaml.cs` or startup code:
```csharp
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ABGAlmacenContext>();
    SeedData.Initialize(context);
}
```

## Connection String Configuration

### Development (Local)
Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "ABGAlmacenDB": "Server=(localdb)\\mssqllocaldb;Database=ABGAlmacenPTL;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Production (Secure)
Use User Secrets or Azure Key Vault:
```bash
dotnet user-secrets set "ConnectionStrings:ABGAlmacenDB" "your-production-connection-string"
```

## Database Schema

### Tables Created:
1. **Articulos** - Articles/Products
2. **BACs** - Storage containers
3. **Ubicaciones** - Locations (12-digit codes)
4. **Cajas** - Boxes (SSCC 18-digit)
5. **TiposCaja** - Box types
6. **Puestos** - Workstations
7. **Usuarios** - Users
8. **BAC_Articulos** - BAC ↔ Article junction
9. **Caja_Articulos** - Caja ↔ Article junction

### Seed Data Included:
- 10 Sample articles
- 15 Locations (3 warehouses)
- 10 BACs with contents
- 5 Boxes with SSCC
- 5 Workstations (VB6 colors)
- 3 Box types
- 2 Test users

## Troubleshooting

### "Target ResolvePackageAssets does not exist"
This is the multi-targeting issue. Use Option 1 or 2 above.

### "Unable to create an object of type 'ABGAlmacenContext'"
Make sure ABGAlmacenContextFactory.cs exists and has the correct connection string.

### LocalDB not available
On Linux/Mac or if LocalDB isn't installed, use SQL Server Express or Docker:
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

Then update connection string:
```
Server=localhost,1433;Database=ABGAlmacenPTL;User Id=sa;Password=YourPassword123!;TrustServerCertificate=True
```

## Notes
- Migration files will be auto-generated in `Migrations/` folder
- The actual migration code is created by EF Core based on your `ABGAlmacenContext` model
- Seed data is defined in `Data/SeedData.cs`
- ABGAlmacenContextFactory.cs provides design-time support for EF Core tools

