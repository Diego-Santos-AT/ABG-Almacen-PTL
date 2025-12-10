# Instrucciones para abrir el proyecto en Visual Studio

## Requisitos previos

1. **Visual Studio 2022** (versión 17.0 o superior) o **Visual Studio 2025**
2. **.NET 10 SDK** instalado
3. **Carga de trabajo de .NET MAUI** instalada

## Instalación de requisitos

### 1. Instalar .NET 10 SDK

Si aún no tienes .NET 10 SDK instalado:

1. Descarga desde: https://dotnet.microsoft.com/download/dotnet/10.0
2. Ejecuta el instalador
3. Verifica la instalación abriendo una terminal y ejecutando:
   ```bash
   dotnet --version
   ```
   Debe mostrar versión 10.0.x

### 2. Instalar la carga de trabajo de .NET MAUI

**Opción A: Usando Visual Studio Installer**

1. Abre **Visual Studio Installer**
2. Haz clic en **Modificar** en tu instalación de Visual Studio
3. En la pestaña **Cargas de trabajo**, marca:
   - **.NET Multi-platform App UI development** (.NET MAUI)
4. Haz clic en **Modificar** para instalar

**Opción B: Usando línea de comandos**

Abre una terminal como administrador y ejecuta:

```bash
dotnet workload install maui
```

## Abrir el proyecto

1. **Doble clic** en el archivo `ABGAlmacenPTL.sln` en la raíz del repositorio
   
   O desde Visual Studio:
   - Archivo → Abrir → Proyecto/Solución
   - Navega a la carpeta del repositorio
   - Selecciona `ABGAlmacenPTL.sln`

2. Visual Studio abrirá la solución y restaurará automáticamente los paquetes NuGet

3. La primera vez que abras el proyecto, Visual Studio puede solicitar instalar componentes adicionales. Acepta las instalaciones recomendadas.

## Configuración del proyecto

### Target Frameworks

El proyecto está configurado para:
- **Android**: `net10.0-android`
- **Windows**: `net10.0-windows10.0.19041.0` (solo en Windows)

### Plataforma de compilación

Para desarrollar:

- **En Windows**: Puedes compilar y ejecutar tanto para Android como para Windows
- **En macOS/Linux**: Solo puedes compilar y ejecutar para Android

Para seleccionar la plataforma de destino:
1. En Visual Studio, busca el selector de plataforma en la barra de herramientas
2. Selecciona:
   - **Android** → para compilar para Android
   - **Windows Machine** → para compilar para Windows (solo disponible en Windows)

## Solución de problemas

### Error: "To build this project, the following workloads must be installed: maui-android"

**Solución**: Instala la carga de trabajo de MAUI:
```bash
dotnet workload install maui
```

### Error: "project.assets.json no tiene un destino para net10.0-windows"

**Solución**: 
1. Asegúrate de tener .NET 10 SDK instalado
2. Restaura los paquetes NuGet:
   - Clic derecho en la solución → Restaurar paquetes NuGet
   - O desde terminal: `dotnet restore`

### Error al compilar para Windows en macOS/Linux

Esto es normal. La compilación para Windows solo está disponible en sistemas Windows. En sistemas no-Windows:
1. Edita el archivo `ABGAlmacenPTL.csproj`
2. Comenta la línea que agrega el target de Windows:
   ```xml
   <!-- <TargetFrameworks Condition="$([MSBuild]::IsOSPlatform('windows'))">$(TargetFrameworks);net10.0-windows10.0.19041.0</TargetFrameworks> -->
   ```

## Compilar y ejecutar

### Compilar
- **Menú**: Compilación → Compilar solución
- **Atajo**: `Ctrl+Shift+B` (Windows) o `Cmd+Shift+B` (Mac)
- **Terminal**: `dotnet build`

### Ejecutar
1. Selecciona el dispositivo de destino (emulador Android o máquina Windows)
2. Presiona `F5` o haz clic en el botón **Iniciar**

## Estructura del proyecto

```
ABG-Almacen-PTL/
├── ABGAlmacenPTL.sln           ← Archivo de solución (abrir este)
├── .gitignore                   ← Archivos ignorados por Git
├── ABGAlmacenPTL/
│   ├── ABGAlmacenPTL.csproj    ← Archivo del proyecto
│   ├── MauiProgram.cs          ← Punto de entrada de la aplicación
│   ├── App.xaml                ← Aplicación MAUI
│   ├── AppShell.xaml           ← Shell de navegación
│   ├── Models/                 ← Modelos de datos (Entity Framework)
│   ├── Services/               ← Servicios de negocio y acceso a datos
│   ├── Views/                  ← Páginas/Vistas XAML
│   └── Resources/              ← Recursos (imágenes, fuentes, etc.)
```

## Más información

- [Documentación de .NET MAUI](https://docs.microsoft.com/dotnet/maui/)
- [Guía de inicio de Visual Studio](https://docs.microsoft.com/visualstudio/get-started/)
- Para problemas específicos del proyecto, consulta los archivos de documentación en la raíz del repositorio
