# ABG Almacén PTL - .NET 10 MAUI

Sistema de gestión de almacén PTL (Pick To Light) para Android y Windows.

## Descripción

Este proyecto es una migración moderna del sistema VB6 original a .NET 10 MAUI, manteniendo la lógica de negocio original línea por línea.

## Plataformas Soportadas

- ✅ **Android** 5.0+ (API 21+)
- ✅ **Windows** 10 1903+ (Build 10.0.19041)

## Optimización

- **Pantallas objetivo**: Dispositivos de 4 pulgadas
- **Diseño responsive** adaptado para terminales de mano

## Tecnologías

- .NET 10
- MAUI (Multi-platform App UI)
- C# 12
- XAML

## Estructura del Proyecto

```
ABGAlmacenPTL/
├── Models/              # Modelos de datos (tipos, entidades)
├── Modules/             # Módulos de lógica (estáticos, globales)
├── Classes/             # Clases de negocio
├── Pages/               # Páginas MAUI (XAML + code-behind)
│   ├── Generic/         # Páginas genéricas (mensajes, errores)
│   └── PTL/             # Páginas específicas PTL
├── Data/                # Acceso a datos (DbContext, repositorios)
├── Configuration/       # Configuración de la aplicación
├── Services/            # Servicios de negocio
├── Platforms/           # Código específico de plataforma
│   ├── Android/
│   └── Windows/
└── Resources/           # Recursos (imágenes, fuentes, estilos)
```

## Requisitos de Desarrollo

- .NET 10 SDK
- Workloads MAUI instalados:
  - `maui-android`
  - `maui-windows`

## Compilación

```bash
# Restaurar dependencias
dotnet restore

# Compilar para Android
dotnet build -f net10.0-android

# Compilar para Windows
dotnet build -f net10.0-windows10.0.19041.0
```

## Estado de Migración

Ver [MIGRATION_STATUS.md](../MIGRATION_STATUS.md) para el estado detallado de la migración desde VB6.

## Información Original

- **Empresa**: ATOSA - Kiokids
- **Versión VB6 Original**: 23.4.2 (27/04/2023)
- **Descripción**: Gestión de Almacén PTL en terminales de mano

## Funcionalidades Principales

1. **Ubicar BAC** - Ubicación de contenedores
2. **Extraer BAC** - Extracción de contenedores
3. **Reparto** - Distribución de artículos
4. **Empaquetado** - Empaquetado de productos
5. **Consultas PTL** - Consultas del sistema

## Base de Datos

- SQL Server
- Conexión via Microsoft.Data.SqlClient
- Soporte para múltiples empresas y almacenes

## Licencia

Copyright © Dpto. Informática ATOSA
