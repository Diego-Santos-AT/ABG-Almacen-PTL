# ABG Almacén PTL - .NET MAUI

Aplicación de Gestión de Almacén PTL (Pick To Light) - Conversión fiel de VB6 a .NET MAUI.

## Descripción

Esta aplicación es una conversión línea por línea del proyecto original VB6 "ABG Almacén PTL" a .NET MAUI, manteniendo la misma funcionalidad y estructura del código original.

## Origen del Proyecto

El proyecto original fue desarrollado en Visual Basic 6.0 con las siguientes características:

- **Creación original**: 30/01/2001
- **Última modificación VB6**: 23/09/2020
- **Autor original**: A. Esteban / A. Moreno Marquéz

## Funcionalidades

### Módulos Principales

1. **Consultas PTL** (`ConsultaPTLPage`)
   - Consulta de BACs, Cajas (SSCC) y Artículos
   - Lectura por código de barras EAN13, SSCC o código interno

2. **Extraer BAC** (`ExtraerBACPage`)
   - Extracción de BACs de ubicaciones PTL
   - Cambio de estado (Abierto/Cerrado)

3. **Ubicar BAC** (`UbicarBACPage`)
   - Ubicación de BACs en ubicaciones PTL
   - Validación de ubicaciones libres/ocupadas

4. **Repartir Artículo** (`RepartirArticuloPage`)
   - Reparto de artículos en BACs casilleros
   - Selección de puesto de trabajo con colores

5. **Empaquetar BAC** (`EmpaquetarBACPage`)
   - Empaquetado de BACs en CAJAs
   - Opciones configurables: Cerrar BAC, Extraer BAC, Crear CAJA, Imprimir
   - Cambio de tipo de caja
   - Combinación de cajas

## Estructura del Proyecto

```
ABGAlmacenPTL.Maui/
├── App.xaml                     # Recursos de la aplicación
├── AppShell.xaml                # Shell de navegación (MDIForm)
├── MauiProgram.cs               # Punto de entrada
├── Constants.cs                 # Constantes globales (wsConstantes.bas)
├── Models/
│   ├── Usuario.cs               # Modelos de usuario/empresa
│   └── BACModels.cs             # Modelos de BAC, CAJA, Artículo
├── Services/
│   ├── AppSettings.cs           # Configuración (wsConfiguracion.bas)
│   ├── DataEnvironment.cs       # Acceso a datos (EntornoDeDatos.Dsr)
│   ├── GeneralFunctions.cs      # Funciones generales (wsFuncionesGenerales.bas)
│   └── MessageService.cs        # Mensajes (wsMensaje)
├── Pages/
│   ├── MenuPage.xaml            # Menú principal (frmMenu.frm)
│   ├── ConsultaPTLPage.xaml     # Consultas PTL (frmConsultaPTL.frm)
│   ├── ExtraerBACPage.xaml      # Extraer BAC (frmExtraerBAC.frm)
│   ├── UbicarBACPage.xaml       # Ubicar BAC (frmUbicarBAC.frm)
│   ├── RepartirArticuloPage.xaml # Repartir Artículo (frmRepartirArticulo.frm)
│   └── EmpaquetarBACPage.xaml   # Empaquetar BAC (frmEmpaquetarBAC.frm)
├── Resources/
│   ├── Styles/
│   │   ├── Colors.xaml          # Colores (convertidos de BGR a RGB)
│   │   └── Styles.xaml          # Estilos MAUI
│   ├── appicon.svg
│   ├── appiconfg.svg
│   └── splash.svg
└── Platforms/
    ├── Android/
    ├── iOS/
    ├── MacCatalyst/
    └── Windows/
```

## Requisitos

- .NET 8.0 SDK
- Visual Studio 2022 con carga de trabajo .NET MAUI
- Para Windows: Windows 10 versión 1809 o superior
- Para Android: Android API 21+
- Para iOS: iOS 11+
- Para macOS: macOS 10.15+

## Base de Datos

La aplicación se conecta a una base de datos SQL Server con los siguientes procedimientos almacenados:

- `DameDatosBACdePTL` - Obtener datos de BAC
- `DameDatosUbicacionPTL` - Obtener datos de ubicación
- `DameDatosCAJAdePTL` - Obtener datos de caja
- `DameContenidoBacGrupo` - Obtener contenido del BAC
- `DameTiposCajasActivas` - Tipos de cajas activas
- `CambiaEstadoBACdePTL` - Cambiar estado del BAC
- `RetirarBACdePTL` - Extraer BAC
- `UbicarBACenPTL` - Ubicar BAC
- `TraspasaBACaCAJAdePTL` - Crear caja desde BAC
- `DameArticuloConsulta` - Obtener artículo por código
- `DameArticuloEAN13` - Obtener artículo por EAN13
- `DamePuestosTrabajoPTL` - Puestos de trabajo
- `ReservaBACdePTL` - Reservar BAC para artículo
- `CombinarCajasPTL` - Combinar cajas
- `CambiaTipoCajaPTL` - Cambiar tipo de caja

## Configuración

La cadena de conexión se configura a través de `AppSettings.Instance.ConexionGestionAlmacen`.

Por defecto:
```
Server=localhost;Database=GAGESTION;User Id=sa;Password=;TrustServerCertificate=True;
```

## Compilación

```bash
# Restaurar paquetes
dotnet restore

# Compilar para Windows
dotnet build -f net8.0-windows10.0.19041.0

# Compilar para Android
dotnet build -f net8.0-android

# Compilar para iOS
dotnet build -f net8.0-ios
```

## Mapeo VB6 → .NET MAUI

| VB6                        | .NET MAUI                  |
|---------------------------|----------------------------|
| MDIForm                   | Shell                      |
| Form                      | ContentPage                |
| CommandButton             | Button                     |
| TextBox                   | Entry                      |
| Label                     | Label                      |
| Frame                     | Frame                      |
| ComboBox                  | Picker                     |
| CheckBox                  | CheckBox                   |
| OptionButton              | RadioButton                |
| UltraGrid                 | CollectionView             |
| ADODB.Recordset           | SqlDataReader              |
| ADODB.Connection          | SqlConnection              |
| Module                    | static class               |
| Class                     | class                      |
| Collection                | List<T>                    |
| MsgBox                    | DisplayAlert               |

## Notas de Conversión

1. **Colores**: VB6 usa formato BGR (&H00BBGGRR), convertido a formato RGB (#RRGGBB)
2. **Eventos KeyDown**: Convertidos a evento `Completed` de Entry
3. **SetFocus**: Convertido a método `Focus()`
4. **Screen.MousePointer**: Manejado internamente por MAUI
5. **Recordset**: Convertido a métodos async con SqlDataReader
6. **Parámetros Output**: Manejados con SqlParameter Direction.Output

## Licencia

Proyecto propietario - ABG Soluciones
