# Migración ABG Almacén PTL - VB6 a .NET 10 MAUI

## Estado del Proyecto

Este documento describe el estado de la migración del sistema de gestión de almacén PTL (Pick To Light) desde VB6 a .NET 10 MAUI para Android y Windows.

## Análisis del Proyecto Original (VB6)

### Resumen
- **Líneas de código totales**: ~12,232 líneas
- **Archivos de código**: 24 archivos
- **Tecnología origen**: Visual Basic 6.0
- **Base de datos**: SQL Server (ADO/OLEDB)
- **Interfaz**: Windows Forms con controles ActiveX

### Estructura del Código VB6

#### Clases (4 archivos - 702 líneas)
1. `cMemory.cls` (130 líneas) - Gestión de memoria
2. `clGenericaRecordset.cls` (214 líneas) - Recordset genérico en memoria
3. `clsDataFilter.cls` (222 líneas) - Filtrado de datos
4. `clsRowLoop.cls` (136 líneas) - Iteración de filas

#### Módulos (7 archivos - 4,006 líneas)
1. `Gestion.bas` (997 líneas) - **Módulo principal** con tipos, variables globales y lógica de arranque
2. `Profile.bas` (278 líneas) - Lectura/escritura de archivos INI
3. `CodeModule.bas` (616 líneas) - Funciones auxiliares de código
4. `GDConstantes.bas` (111 líneas) - Constantes globales
5. `GDFunc01.bas` (533 líneas) - Funciones generales 1
6. `GDFunc02.bas` (1,150 líneas) - Funciones generales 2 (más grande)
7. `GDFunc04.bas` (386 líneas) - Funciones generales 4

#### Formularios (13 archivos - 7,524 líneas)
**Formularios Genéricos (5):**
1. `frmMensaje.frm` (159 líneas) - Mensajes
2. `frmMsgBox.frm` (257 líneas) - Cuadro de mensajes
3. `frmErrorTransaccion.frm` (117 líneas) - Errores de transacción
4. `frmSeleccionTabla2.frm` (158 líneas) - Selección de tablas
5. `frmVerFoto.frm` (86 líneas) - Visor de fotos

**Formularios Principales (3):**
6. `frmInicio.frm` (717 líneas) - **Formulario de login**
7. `frmMain.frm` (374 líneas) - **Formulario MDI principal**
8. `frmMenu.frm` (259 líneas) - **Menú principal**

**Formularios PTL (5):**
9. `frmConsultaPTL.frm` (768 líneas) - Consultas PTL
10. `frmExtraerBAC.frm` (634 líneas) - Extracción de BAC
11. `frmUbicarBAC.frm` (681 líneas) - Ubicación de BAC
12. `frmRepartirArticulo.frm` (536 líneas) - Reparto de artículos
13. `frmEmpaquetarBAC.frm` (2,713 líneas) - **Empaquetado de BAC (el más grande)**

#### Data Environment (2 archivos)
1. `EntornoDeDatos.Dsr` - Entorno de datos principal
2. `edConfig.Dsr` - Entorno de datos de configuración

#### Configuración
1. `abg.ini` - Archivo de configuración

## Proyecto .NET 10 MAUI Creado

### Estructura del Proyecto

```
ABGAlmacenPTL/
├── ABGAlmacenPTL.csproj          # Proyecto .NET 10 MAUI
├── App.xaml / App.xaml.cs        # Aplicación MAUI
├── AppShell.xaml / AppShell.xaml.cs
├── MainPage.xaml / MainPage.xaml.cs
├── MauiProgram.cs                 # Configuración de la app
├── Models/                        # ✅ COMPLETADO
│   ├── TipoEmpresa.cs            # Modelo de empresa (130 líneas)
│   └── TiposGlobales.cs          # Tipos globales (80 líneas)
├── Modules/                       # ⚠️ PARCIALMENTE COMPLETADO
│   └── Gestion.Globals.cs        # Variables globales (210 líneas)
├── Classes/                       # ⏳ PENDIENTE
├── Pages/                         # ⏳ PENDIENTE
│   ├── Generic/                  # Para formularios genéricos
│   └── PTL/                      # Para formularios PTL
├── Data/                          # ⏳ PENDIENTE
├── Configuration/                 # ⏳ PENDIENTE
├── Services/                      # ⏳ PENDIENTE
├── Platforms/
│   ├── Android/                   # Configuración Android
│   └── Windows/                   # Configuración Windows
└── Resources/
    ├── Images/
    ├── Fonts/
    └── Styles/
```

### Configuración del Proyecto

**Frameworks objetivo:**
- ✅ `net10.0-android` (Android 5.0+)
- ✅ `net10.0-windows10.0.19041.0` (Windows 10 1903+)

**Información de la aplicación:**
- Título: "ABG Almacén PTL"
- ID: com.atosa.abgalmacenptl
- Versión: 23.4.2 (matching VB6 version)

## Estado de la Migración

### ✅ Completado

1. **Instalación de workloads MAUI** para Android y Windows
2. **Creación del proyecto base** .NET 10 MAUI
3. **Configuración del proyecto** para Android y Windows únicamente
4. **Migración de modelos de datos:**
   - `TipoEmpresa` (completo con todas las propiedades)
   - `TipoOpcion`, `TipoMenu`, `TipoUsuario`, `PuestoTrabajo`
5. **Variables globales** del módulo Gestion.bas migradas a clase estática

### ⚠️ En Progreso

1. **Módulo Gestion.bas** - Variables globales completadas, falta lógica de métodos

### ⏳ Pendiente (Trabajo Restante Significativo)

#### Módulos (6 archivos pendientes)
- [ ] Profile.bas → Configuration/ProfileManager.cs
- [ ] CodeModule.bas → Modules/CodeModule.cs
- [ ] GDConstantes.bas → Modules/GDConstantes.cs
- [ ] GDFunc01.bas → Modules/GDFunc01.cs
- [ ] GDFunc02.bas → Modules/GDFunc02.cs (1,150 líneas!)
- [ ] GDFunc04.bas → Modules/GDFunc04.cs

#### Clases (4 archivos pendientes)
- [ ] cMemory.cls → Classes/Memory.cs
- [ ] clGenericaRecordset.cls → Classes/GenericRecordset.cs
- [ ] clsDataFilter.cls → Classes/DataFilter.cs
- [ ] clsRowLoop.cls → Classes/RowLoop.cs

#### Páginas MAUI (13 formularios pendientes)
**Cada formulario requiere:**
- Archivo XAML para la interfaz (optimizado para 4")
- Archivo .cs code-behind con lógica
- Migración de eventos y controles

**Formularios Genéricos:**
- [ ] frmMensaje → Pages/Generic/MensajePage.xaml
- [ ] frmMsgBox → Pages/Generic/MsgBoxPage.xaml
- [ ] frmErrorTransaccion → Pages/Generic/ErrorTransaccionPage.xaml
- [ ] frmSeleccionTabla2 → Pages/Generic/SeleccionTabla2Page.xaml
- [ ] frmVerFoto → Pages/Generic/VerFotoPage.xaml

**Formularios Principales:**
- [ ] frmInicio → Pages/InicioPage.xaml (Login - 717 líneas)
- [ ] frmMain → AppShell.xaml (MDI → Shell navigation)
- [ ] frmMenu → Pages/MenuPage.xaml

**Formularios PTL:**
- [ ] frmConsultaPTL → Pages/PTL/ConsultaPTLPage.xaml (768 líneas)
- [ ] frmExtraerBAC → Pages/PTL/ExtraerBACPage.xaml (634 líneas)
- [ ] frmUbicarBAC → Pages/PTL/UbicarBACPage.xaml (681 líneas)
- [ ] frmRepartirArticulo → Pages/PTL/RepartirArticuloPage.xaml (536 líneas)
- [ ] frmEmpaquetarBAC → Pages/PTL/EmpaquetarBACPage.xaml (2,713 líneas!)

#### Infraestructura (todo pendiente)
- [ ] **Data Access Layer** - Migrar ADO a modern data access
  - Reemplazar ADODB.Recordset con Entity Framework Core o Dapper
  - Crear DbContext y modelos de entidades
  - Migrar consultas SQL
- [ ] **Configuration System**
  - Migrar abg.ini a sistema de configuración moderno
  - Implementar IConfiguration para .NET
  - Secure storage para credenciales
- [ ] **Dependency Injection Setup**
  - Registrar servicios en MauiProgram.cs
  - Crear interfaces para servicios
- [ ] **Navigation Service**
  - Sistema de navegación para MAUI
- [ ] **Print Service**
  - Integración con impresoras (TEC, ZEBRA)
  - Diferentes implementaciones Android/Windows

## Desafíos Técnicos Principales

### 1. Acceso a Datos
- **VB6**: ADO con ADODB.Recordset
- **MAUI**: Requiere Entity Framework Core, Dapper, o similar
- **Complejidad**: Alta - todas las consultas deben migrarse

### 2. Interfaz de Usuario
- **VB6**: Windows Forms con controles ActiveX
- **MAUI**: XAML con controles nativos
- **Complejidad**: Muy Alta - rediseño completo
- **4" screens**: Diseño responsive necesario

### 3. Lógica de Negocio
- **VB6**: Procedural con módulos y funciones globales
- **C#**: OOP con servicios y dependency injection
- **Complejidad**: Media-Alta - refactorización estructural

### 4. Configuración
- **VB6**: Archivos INI con APIs Windows
- **MAUI**: Sistema de configuración moderno + Secure Storage
- **Complejidad**: Media

### 5. Base de Datos
- **VB6**: SQL Server con cadenas OLE DB
- **MAUI**: SQL Server con Microsoft.Data.SqlClient
- **Complejidad**: Media - cambio de provider

### 6. Platform-Specific Code
- **Windows APIs**: Algunos reemplazan con APIs MAUI
- **Android equivalents**: Implementar alternativas
- **Complejidad**: Media-Alta

## Estimación de Tiempo

Basado en el análisis del código:

| Componente | Líneas VB6 | Esfuerzo Estimado | Estado |
|------------|------------|-------------------|--------|
| Modelos de datos | ~200 | 2 horas | ✅ Completo |
| Módulos (lógica) | ~4,000 | 40-60 horas | 5% |
| Clases | ~700 | 10-15 horas | 0% |
| Formularios (XAML + code-behind) | ~7,500 | 80-120 horas | 0% |
| Data Access Layer | N/A | 20-30 horas | 0% |
| Configuración/Servicios | N/A | 10-15 horas | 0% |
| Testing | N/A | 20-30 horas | 0% |
| **TOTAL** | **~12,400** | **182-270 horas** | **~2%** |

**Esto equivale a 4.5 - 6.75 semanas de trabajo a tiempo completo.**

## Próximos Pasos Recomendados

### Fase 1: Infraestructura Core (1-2 semanas)
1. Completar módulo Gestion.bas con métodos Main(), ConfiguracionEmpresa(), etc.
2. Implementar ProfileManager para leer/escribir configuración
3. Crear Data Access Layer (DbContext, repositorios)
4. Implementar sistema de navegación MAUI

### Fase 2: Formularios Principales (1-2 semanas)
5. Migrar frmInicio (Login)
6. Configurar AppShell/Navigation
7. Migrar frmMenu (Menú principal)

### Fase 3: Formularios PTL (2-3 semanas)
8. Migrar formularios PTL por orden de complejidad:
   - frmConsultaPTL
   - frmRepartirArticulo
   - frmExtraerBAC
   - frmUbicarBAC
   - frmEmpaquetarBAC (el más complejo)

### Fase 4: Formularios Genéricos y Testing (1 semana)
9. Migrar formularios genéricos (mensajes, errores, etc.)
10. Testing integral en Android y Windows
11. Optimización para pantallas 4"

## Notas Importantes

1. **Fidelidad al VB6**: Se está manteniendo la estructura y lógica original línea por línea según requisitos
2. **Pantallas 4"**: El diseño XAML debe optimizarse específicamente para displays pequeños
3. **Multiplataforma**: Android y Windows tienen diferentes capacidades (impresoras, APIs, etc.)
4. **SQL Server**: La conectividad a SQL Server funciona diferente en Android vs Windows

## Próximo Paso Inmediato

Continuar con la migración del módulo `Gestion.bas`, específicamente:
1. Método `Main()` - punto de entrada
2. Método `CrearAGBIni()` - crear configuración
3. Método `LeerParamentrosIni()` - leer configuración
4. Método `ConfiguracionEmpresa()` - configurar empresa

Luego proceder con Profile.bas para la gestión de archivos INI.
