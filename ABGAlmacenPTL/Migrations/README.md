# Entity Framework Core Migrations

## How to Use Migrations

### 1. Create Initial Migration (First Time Only)
```bash
cd ABGAlmacenPTL
dotnet ef migrations add InitialCreate
```

### 2. Apply Migration to Database
```bash
dotnet ef database update
```

### 3. Add Seed Data
In your `Program.cs` or `MauiProgram.cs`, add:
```csharp
using (var context = new ABGAlmacenContext())
{
    SeedData.Initialize(context);
}
```

## Connection String Configuration

### Development (Local)
Edit `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "ABGAlmacenDB": "Server=(localdb)\\mssqllocaldb;Database=ABGAlmacenPTL;Trusted_Connection=true;"
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
8. **BACArticulos** - BAC ↔ Article junction
9. **CajaArticulos** - Caja ↔ Article junction

### Seed Data Included:
- 10 Sample articles
- 15 Locations (3 warehouses)
- 10 BACs with contents
- 5 Boxes with SSCC
- 5 Workstations (VB6 colors)
- 3 Box types
- 2 Test users

## Notes
- Migration files will be auto-generated when you run `dotnet ef migrations add`
- The actual migration code is created by EF Core based on your `ABGAlmacenContext` model
- Seed data is defined in `Data/SeedData.cs`
