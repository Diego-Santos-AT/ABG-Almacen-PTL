# Arquitectura de Bases de Datos - Fiel al VB6

## ⚠️ IMPORTANTE: Multi-Database Architecture

El sistema ABG Almacén PTL utiliza **3 bases de datos diferentes** tal como está configurado en el VB6 original a través del archivo `abg.ini`.

### Bases de Datos del Sistema

#### 1. **Config** (Base de Configuración)
- **Servidor**: BDDServLocal (GROOT desde abg.ini)
- **Nombre BD**: Config
- **Usuario**: ABG
- **Password**: A_34ggyx4
- **Propósito**: 
  - Configuración general del sistema
  - Gestión de usuarios (tabla gdeusr)
  - Gestión de empresas (tabla gdeemp)
  - Permisos y accesos

**Tablas principales**:
- `gdeusr` - Usuarios del sistema
- `gdeemp` - Empresas
- `gdusremp` - Relación usuario-empresa
- `gdemen` - Menús
- Otras tablas de configuración global

#### 2. **Gestion** (Base de Gestión)
- **Servidor**: BDDServ (SELENE desde abg.ini)
- **Nombre BD**: Variable según empresa (campo `empbdd` de tabla gdeemp)
- **Usuario**: Variable según empresa (campo `empusr`)
- **Password**: Variable según empresa (campo `empkey`)
- **Propósito**:
  - Datos maestros de la aplicación
  - Artículos, clientes, proveedores
  - Transacciones de negocio

#### 3. **GestionAlmacen** (Base PTL Específica)
- **Servidor**: Variable según empresa (campo `empservidorga` de gdeemp)
- **Nombre BD**: Variable según empresa (campo `empbga`)
- **Usuario**: Variable según empresa (campo `empuga`)
- **Password**: Variable según empresa (campo `empkga`)
- **Propósito**:
  - Datos específicos del sistema PTL
  - BACs, Ubicaciones, Cajas
  - Puestos de trabajo PTL
  - Movimientos de almacén

**Tablas principales** (estas son las que migramos):
- `Articulos` - Productos/artículos
- `Ubicaciones` - Ubicaciones de almacén (12 dígitos)
- `BACs` - Contenedores de almacenamiento
- `Cajas` - Cajas con código SSCC (18 dígitos)
- `TiposCaja` - Tipos de cajas
- `Puestos` - Puestos de trabajo PTL
- `BAC_Articulos` - Relación BAC ↔ Artículo
- `Caja_Articulos` - Relación Caja ↔ Artículo

## Flujo de Conexión (VB6)

1. **Aplicación inicia** → Lee `abg.ini`
2. **Conecta a Config DB** (servidor local GROOT)
3. **Usuario hace login** → Valida en tabla `gdeusr` de Config
4. **Obtiene empresas del usuario** → Consulta `gdusremp` en Config
5. **Usuario selecciona empresa** → Lee datos de `gdeemp` en Config
6. **Conecta a Gestion DB** → Usa servidor SELENE y BD de empresa
7. **Conecta a GestionAlmacen DB** → Usa servidor y BD específicos de PTL
8. **Aplicación operativa** → Trabaja con las 3 BDs simultáneamente

## Implementación en .NET MAUI

### Archivo `abg.ini`
```ini
[Conexion]
BDDServ=SELENE              # Servidor principal (Gestion)
BDDServLocal=GROOT          # Servidor local (Config)
BDDTime=30                  # Timeout conexión
BDDConfig=Config            # Nombre BD configuración

[Varios]
UsrDefault=diego.santos     # Usuario por defecto
EmpDefault=1                # Empresa por defecto
PueDefault=4                # Puesto de trabajo por defecto
```

### Configuración MAUI

**ABGConfigService** lee `abg.ini` y construye las 3 connection strings:

```csharp
// 1. Config DB - siempre en servidor local
var configConn = abgConfig.GetConfigConnectionString();
// → Server=GROOT;Database=Config;User ID=ABG;Password=A_34ggyx4;...

// 2. Gestion DB - servidor principal, BD variable por empresa
var gestionConn = abgConfig.GetGestionConnectionString(bdName, usuario, password);
// → Server=SELENE;Database={empbdd};User ID={empusr};Password={empkey};...

// 3. GestionAlmacen DB - servidor y BD variables por empresa
var gaConn = abgConfig.GetGestionAlmacenConnectionString(servidor, bdName, usuario, password);
// → Server={empservidorga};Database={empbga};User ID={empuga};Password={empkga};...
```

## Script SQL de Migración

El script `InitialCreate.sql` crea las tablas en la **Base de Datos GestionAlmacen** (la específica de PTL).

Las tablas de Config y Gestion ya existen en el sistema VB6 y no necesitan migrarse.

### Aplicación del Script

```bash
# Conectar a la base de datos GestionAlmacen específica de tu empresa
# Ejemplo para empresa 1:
sqlcmd -S {SERVIDOR_GA} -d {BASE_GA} -U {USUARIO_GA} -P {PASSWORD_GA} -i Migrations/InitialCreate.sql
```

**NOTA**: Los valores entre {} se obtienen de la tabla `gdeemp` en Config DB para tu empresa.

## Próximos Pasos

1. ✅ ABGConfigService creado - lee abg.ini fielmente
2. ✅ MauiProgram configurado - usa Config DB por defecto
3. ⏳ Implementar login - conectar a Config y validar usuario
4. ⏳ Selector de empresa - leer gdeemp y gdusremp de Config
5. ⏳ Conexión dinámica - crear DbContexts para Gestion y GestionAlmacen según empresa seleccionada
6. ⏳ Aplicar InitialCreate.sql a la BD GestionAlmacen correspondiente

## Referencias

- VB6 Original: `Gestion.bas` líneas 348-400 (LeerParamentrosIni)
- VB6 Original: `Gestion.bas` líneas 787-800 (ConexionGestion, ConexionGestionAlmacen)
- .NET MAUI: `Services/ABGConfigService.cs`
- .NET MAUI: `MauiProgram.cs` líneas 27-50
