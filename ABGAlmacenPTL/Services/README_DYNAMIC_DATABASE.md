# Dynamic Database Services - VB6-Faithful Implementation

## Overview

This document describes the dynamic database services that allow the .NET MAUI application to work with the existing SELENE database structure, just like the VB6 version did.

## Architecture

The dynamic database services enable the application to:

1. **Execute stored procedures dynamically** - Call any stored procedure from the SELENE databases without needing pre-defined models
2. **Query tables dynamically** - Execute SQL queries flexibly
3. **Work with multiple databases** - Support for Config, Gestion, and GestionAlmacen databases
4. **Return dynamic results** - Results as DataTables or Dictionary lists for maximum flexibility

## Services

### IDynamicDatabaseService

Interface that defines the contract for dynamic database operations:

- `ExecuteStoredProcedureAsync` - Executes a stored procedure and returns a DataTable
- `ExecuteNonQueryAsync` - Executes a stored procedure without returning results (INSERT, UPDATE, DELETE)
- `ExecuteScalarAsync` - Executes a stored procedure and returns a scalar value
- `ExecuteQueryAsync` - Executes a dynamic SQL query
- `ExecuteStoredProcedureDynamicAsync` - Executes a stored procedure and returns a list of dictionaries
- `StoredProcedureExistsAsync` - Checks if a stored procedure exists

### DynamicDatabaseService

Implementation of `IDynamicDatabaseService` that:

- Uses `SqlConnection` and `SqlCommand` for direct database access
- Supports parameterized queries and stored procedures
- Automatically gets connection strings from `AuthService` and `ABGConfigService`
- Handles different databases (Config, Gestion, GestionAlmacen)

### PTLStoredProcedureService

High-level service that wraps common PTL stored procedures found in SELENE.txt:

**BAC Operations:**
- `UbicarBACenPTLAsync` - Places a BAC in a PTL location
- `ExtraerBACdePTLAsync` - Extracts a BAC from PTL
- `RetirarBACdePTLAsync` - Removes a BAC from PTL
- `VaciarBACdePTLAsync` - Empties a BAC from PTL
- `ConsultaBACdePTLAsync` - Queries a BAC in PTL
- `DameDatosBACdePTLAsync` - Gets BAC data
- `DameBACUbicacionPTLAsync` - Gets BAC location
- `DameBACsUbicadosPTLAsync` - Gets all placed BACs
- `DameBACsGrupoPTLAsync` - Gets BACs for a group
- `CambiaEstadoBACdePTLAsync` - Changes BAC state
- `ActualizaEstadoBACPTLAsync` - Updates BAC state

**Box Operations:**
- `TraspasaBACaCAJAdePTLAsync` - Transfers BAC to box
- `DameDatosCAJAdePTLAsync` - Gets box data
- `ActualizaCajaBACPTLAsync` - Updates box
- `CambiaTipoCajaPTLAsync` - Changes box type
- `CrearCajaGrupoTablillaPTLAsync` - Creates box for group/board
- `DameCajasGrupoTablillaPTLAsync` - Gets boxes for group/board
- `DameCajaGrupoTablillaPTLAsync` - Gets specific box

**Group and Workstation Operations:**
- `DameGruposAsync` - Gets groups
- `DameGruposFiltroPTLAsync` - Gets filtered PTL groups
- `InicializaGrupoPTLAsync` - Initializes PTL group
- `ActualizaGrupoPuestosPTLAsync` - Updates group workstations
- `ActualizaGrupoPuestoPTLAsync` - Updates group workstation
- `DamePuestosTrabajoPTLAsync` - Gets PTL workstations
- `DamePuestosTrabajoGrupoPTLAsync` - Gets workstations for group
- `DamePuestoTrabajoCodigoAsync` - Gets workstation by code

**Article Operations:**
- `DameArticuloEAN13Async` - Gets article by EAN13
- `CambiaUnidadesArtCajaPTLAsync` - Changes article units in box

**Location Operations:**
- `DameDatosUbicacionPTLAsync` - Gets location data

**Packing Operations:**
- `CombinarCajasPTLAsync` - Combines boxes
- `InsertaLogEmpaquetadoAsync` - Inserts packing log
- `DameLogEmpaquetadoPTLAsync` - Gets packing log

**Distribution Operations:**
- `DameLogRepartoPTLAsync` - Gets distribution log

**Statistics Operations:**
- `DameEstadisticaPTLAsync` - Gets PTL statistics
- `DameEstadisticaUsuarioRepPTLAsync` - Gets user distribution statistics
- `DameEstadisticaUsuarioEmpPTLAsync` - Gets user packing statistics

**Liberation Operations:**
- `LiberarBacsErroneosAsync` - Frees erroneous BACs
- `LiberarBacsGrupoAsync` - Frees BACs for a group

## Database Structure from SELENE.txt

The SELENE.txt file contains 4110 lines documenting all database objects across multiple databases:

- **[Config]** - 173 objects (Configuration database)
- **[GAKIOKIDS]** - 347 objects (Kiokids warehouse)
- **[GAATFRA]** - 196 objects (ATOSA warehouse)
- **[GAWERNER]** - 260 objects (Werner warehouse)
- **[LOATOSA]** - 223 objects (ATOSA logistics)
- **[GATOTAL]** - 189 objects (Total warehouse)
- **[GAKIOKIDSFRA]** - 186 objects (Kiokids France)
- **[SGA007]**, **[SGA009]** - 1000+ objects each (WMS systems)

## Usage Examples

### Example 1: Execute a stored procedure with parameters

```csharp
// Inject the service
private readonly IDynamicDatabaseService _dbService;

// Execute stored procedure
var parameters = new Dictionary<string, object>
{
    { "CodigoBAC", "BAC001" },
    { "CodigoUbicacion", "001010100001" },
    { "IdPuesto", 1 }
};

var rowsAffected = await _dbService.ExecuteNonQueryAsync(
    "UbicarBACenPTL", 
    parameters, 
    "GestionAlmacen"
);
```

### Example 2: Query data dynamically

```csharp
// Get BAC data
var result = await _dbService.ExecuteStoredProcedureAsync(
    "DameDatosBACdePTL",
    new Dictionary<string, object> { { "CodigoBAC", "BAC001" } },
    "GestionAlmacen"
);

// Access the DataTable
foreach (DataRow row in result.Rows)
{
    var codigoBAC = row["CodigoBAC"].ToString();
    var estado = row["Estado"].ToString();
    // ...
}
```

### Example 3: Use PTLStoredProcedureService wrapper

```csharp
// Inject the high-level service
private readonly PTLStoredProcedureService _ptlService;

// Place a BAC
await _ptlService.UbicarBACenPTLAsync("BAC001", "001010100001", 1);

// Get BAC data
var bacData = await _ptlService.DameDatosBACdePTLAsync("BAC001");

// Get all groups
var grupos = await _ptlService.DameGruposAsync();
```

### Example 4: Get dynamic results as dictionaries

```csharp
// Execute and get results as list of dictionaries
var results = await _dbService.ExecuteStoredProcedureDynamicAsync(
    "DameGrupos",
    null,
    "GestionAlmacen"
);

// Access results
foreach (var row in results)
{
    var idGrupo = row["IdGrupo"];
    var nombreGrupo = row["NombreGrupo"];
    // ...
}
```

## Integration with Existing Code

The dynamic database services work alongside Entity Framework Core:

1. **EF Core** - Used for strongly-typed CRUD operations with defined models
2. **Dynamic Services** - Used for:
   - Calling stored procedures that already exist in SELENE
   - Flexible queries without pre-defined models
   - Working with multiple databases dynamically
   - VB6-style database operations

## Configuration

The services are registered in `MauiProgram.cs`:

```csharp
// Register dynamic database services (VB6-faithful - stored procedures)
builder.Services.AddScoped<IDynamicDatabaseService, DynamicDatabaseService>();
builder.Services.AddScoped<PTLStoredProcedureService>();
```

Connection strings are obtained automatically from:
- `ABGConfigService` - For Config database
- `AuthService` - For Gestion and GestionAlmacen databases (company-specific)

## Benefits

1. **VB6 Compatibility** - Works exactly like the VB6 version
2. **Flexibility** - No need to create models for every table
3. **Stored Procedures** - Direct access to existing database logic
4. **Multiple Databases** - Seamless multi-database support
5. **Dynamic Results** - Results can be processed flexibly
6. **Type Safety** - PTLStoredProcedureService provides type-safe wrappers

## Future Enhancements

Potential improvements:

- Add transaction support for multi-step operations
- Add result caching for frequently accessed data
- Add logging and monitoring
- Add retry logic for transient failures
- Add more stored procedure wrappers as needed
- Generate wrappers automatically from SELENE.txt

## Testing

To test the dynamic database services:

1. Ensure connection to SELENE server
2. Select a company with GestionAlmacen database
3. Call stored procedures from PTLStoredProcedureService
4. Verify results against VB6 application

Example test:

```csharp
// Test BAC query
var bacData = await _ptlService.DameDatosBACdePTLAsync("BAC001");
Assert.NotNull(bacData);
Assert.True(bacData.Rows.Count > 0);
```

## Related Files

- `Services/IDynamicDatabaseService.cs` - Interface definition
- `Services/DynamicDatabaseService.cs` - Implementation
- `Services/PTLStoredProcedureService.cs` - PTL stored procedure wrappers
- `Services/ABGConfigService.cs` - Configuration service
- `Services/AuthService.cs` - Authentication and company selection
- `SELENE.txt` - Database object catalog from SELENE server

## See Also

- [Database Architecture](../Migrations/DATABASE_ARCHITECTURE.md)
- [VB6 Migration Guide](../README.md)
- [EF Core Models](../Models/)
