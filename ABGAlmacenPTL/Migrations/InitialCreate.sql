-- ABG Almacén PTL - Initial Database Schema
-- Generated from EF Core models
-- Date: 2025-12-10
-- IMPORTANTE: Este script fue creado manualmente debido a limitaciones de EF Core tools con MAUI
-- En Visual Studio, usar: Add-Migration InitialCreate

-- ============================================================================
-- TABLAS PRINCIPALES
-- ============================================================================

-- Tabla: Articulos
CREATE TABLE [Articulos] (
    [CodigoArticulo] NVARCHAR(50) NOT NULL,
    [Nombre] NVARCHAR(200) NOT NULL,
    [EAN13] NVARCHAR(13) NULL,
    [CodigoSTD] NVARCHAR(50) NULL,
    [Peso] DECIMAL(10,3) NULL,
    [Volumen] DECIMAL(10,3) NULL,
    [Activo] BIT NOT NULL DEFAULT 1,
    [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [FechaModificacion] DATETIME2 NULL,
    CONSTRAINT [PK_Articulos] PRIMARY KEY ([CodigoArticulo])
);

CREATE INDEX [IX_Articulos_EAN13] ON [Articulos] ([EAN13]);

-- Tabla: Ubicaciones
CREATE TABLE [Ubicaciones] (
    [CodigoUbicacion] NVARCHAR(12) NOT NULL,
    [Almacen] INT NOT NULL,
    [Bloque] INT NOT NULL,
    [Fila] INT NOT NULL,
    [Altura] INT NOT NULL,
    [Descripcion] NVARCHAR(200) NULL,
    [Ocupada] BIT NOT NULL DEFAULT 0,
    [Activa] BIT NOT NULL DEFAULT 1,
    [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [PK_Ubicaciones] PRIMARY KEY ([CodigoUbicacion])
);

CREATE UNIQUE INDEX [IX_Ubicaciones_Almacen_Bloque_Fila_Altura] 
    ON [Ubicaciones] ([Almacen], [Bloque], [Fila], [Altura]);

-- Tabla: BACs (Contenedores de almacenamiento)
CREATE TABLE [BACs] (
    [CodigoBAC] NVARCHAR(50) NOT NULL,
    [Estado] INT NOT NULL DEFAULT 0, -- 0=Abierto, 1=Cerrado
    [CodigoUbicacion] NVARCHAR(12) NULL,
    [Grupo] NVARCHAR(50) NULL,
    [Tablilla] NVARCHAR(50) NULL,
    [Unidades] INT NOT NULL DEFAULT 0,
    [Peso] DECIMAL(10,3) NULL,
    [Volumen] DECIMAL(10,3) NULL,
    [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [FechaModificacion] DATETIME2 NULL,
    CONSTRAINT [PK_BACs] PRIMARY KEY ([CodigoBAC]),
    CONSTRAINT [FK_BACs_Ubicaciones] 
        FOREIGN KEY ([CodigoUbicacion]) 
        REFERENCES [Ubicaciones]([CodigoUbicacion]) 
        ON DELETE SET NULL
);

CREATE INDEX [IX_BACs_CodigoUbicacion] ON [BACs] ([CodigoUbicacion]);
CREATE INDEX [IX_BACs_Estado] ON [BACs] ([Estado]);

-- Tabla: TiposCaja
CREATE TABLE [TiposCaja] (
    [TipoId] INT NOT NULL IDENTITY(1,1),
    [Nombre] NVARCHAR(100) NOT NULL,
    [Descripcion] NVARCHAR(500) NULL,
    [PesoMaximo] DECIMAL(10,3) NOT NULL,
    [VolumenMaximo] DECIMAL(10,3) NOT NULL,
    [Activo] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_TiposCaja] PRIMARY KEY ([TipoId])
);

-- Tabla: Cajas (con SSCC)
CREATE TABLE [Cajas] (
    [SSCC] NVARCHAR(18) NOT NULL,
    [TipoId] INT NOT NULL,
    [Estado] INT NOT NULL DEFAULT 0, -- 0=Abierta, 1=Cerrada
    [Unidades] INT NOT NULL DEFAULT 0,
    [Peso] DECIMAL(10,3) NULL,
    [Volumen] DECIMAL(10,3) NULL,
    [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [FechaCierre] DATETIME2 NULL,
    [Observaciones] NVARCHAR(200) NULL,
    CONSTRAINT [PK_Cajas] PRIMARY KEY ([SSCC]),
    CONSTRAINT [FK_Cajas_TiposCaja] 
        FOREIGN KEY ([TipoId]) 
        REFERENCES [TiposCaja]([TipoId]) 
        ON DELETE RESTRICT
);

CREATE INDEX [IX_Cajas_Estado] ON [Cajas] ([Estado]);
CREATE INDEX [IX_Cajas_FechaCreacion] ON [Cajas] ([FechaCreacion]);

-- Tabla: Puestos (Estaciones de trabajo PTL)
CREATE TABLE [Puestos] (
    [PuestoId] INT NOT NULL IDENTITY(1,1),
    [Numero] INT NOT NULL,
    [Nombre] NVARCHAR(100) NOT NULL,
    [Color] INT NOT NULL, -- Enum: 0=Rojo, 1=Verde, 2=Azul, 3=Amarillo, 4=Magenta
    [Descripcion] NVARCHAR(500) NULL,
    [Activo] BIT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Puestos] PRIMARY KEY ([PuestoId])
);

CREATE UNIQUE INDEX [IX_Puestos_Numero] ON [Puestos] ([Numero]);

-- Tabla: Usuarios
CREATE TABLE [Usuarios] (
    [UsuarioId] INT NOT NULL IDENTITY(1,1),
    [NombreUsuario] NVARCHAR(50) NOT NULL,
    [Contraseña] NVARCHAR(255) NOT NULL, -- NOTA: Debe hashearse en producción
    [NombreCompleto] NVARCHAR(200) NULL,
    [Email] NVARCHAR(255) NULL,
    [Rol] NVARCHAR(50) NULL,
    [Activo] BIT NOT NULL DEFAULT 1,
    [FechaCreacion] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [UltimoAcceso] DATETIME2 NULL,
    CONSTRAINT [PK_Usuarios] PRIMARY KEY ([UsuarioId])
);

CREATE UNIQUE INDEX [IX_Usuarios_NombreUsuario] ON [Usuarios] ([NombreUsuario]);
CREATE INDEX [IX_Usuarios_Email] ON [Usuarios] ([Email]);

-- ============================================================================
-- TABLAS DE UNIÓN (Many-to-Many)
-- ============================================================================

-- Tabla: BAC_Articulos (Relación BAC ↔ Artículo)
CREATE TABLE [BAC_Articulos] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [CodigoBAC] NVARCHAR(50) NOT NULL,
    [CodigoArticulo] NVARCHAR(50) NOT NULL,
    [Cantidad] INT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_BAC_Articulos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BAC_Articulos_BACs] 
        FOREIGN KEY ([CodigoBAC]) 
        REFERENCES [BACs]([CodigoBAC]) 
        ON DELETE CASCADE,
    CONSTRAINT [FK_BAC_Articulos_Articulos] 
        FOREIGN KEY ([CodigoArticulo]) 
        REFERENCES [Articulos]([CodigoArticulo]) 
        ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_BAC_Articulos_CodigoBAC_CodigoArticulo] 
    ON [BAC_Articulos] ([CodigoBAC], [CodigoArticulo]);

-- Tabla: Caja_Articulos (Relación Caja ↔ Artículo)
CREATE TABLE [Caja_Articulos] (
    [Id] INT NOT NULL IDENTITY(1,1),
    [SSCC] NVARCHAR(18) NOT NULL,
    [CodigoArticulo] NVARCHAR(50) NOT NULL,
    [Cantidad] INT NOT NULL DEFAULT 1,
    CONSTRAINT [PK_Caja_Articulos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Caja_Articulos_Cajas] 
        FOREIGN KEY ([SSCC]) 
        REFERENCES [Cajas]([SSCC]) 
        ON DELETE CASCADE,
    CONSTRAINT [FK_Caja_Articulos_Articulos] 
        FOREIGN KEY ([CodigoArticulo]) 
        REFERENCES [Articulos]([CodigoArticulo]) 
        ON DELETE CASCADE
);

CREATE UNIQUE INDEX [IX_Caja_Articulos_SSCC_CodigoArticulo] 
    ON [Caja_Articulos] ([SSCC], [CodigoArticulo]);

-- ============================================================================
-- DATOS INICIALES (SEED DATA)
-- ============================================================================

-- TiposCaja
SET IDENTITY_INSERT [TiposCaja] ON;
INSERT INTO [TiposCaja] ([TipoId], [Nombre], [Descripcion], [PesoMaximo], [VolumenMaximo], [Activo])
VALUES 
    (1, 'Caja Pequeña', 'Caja estándar pequeña', 10.0, 0.1, 1),
    (2, 'Caja Mediana', 'Caja estándar mediana', 25.0, 0.25, 1),
    (3, 'Caja Grande', 'Caja estándar grande', 50.0, 0.5, 1);
SET IDENTITY_INSERT [TiposCaja] OFF;

-- Puestos de trabajo PTL con colores VB6
SET IDENTITY_INSERT [Puestos] ON;
INSERT INTO [Puestos] ([PuestoId], [Numero], [Nombre], [Color], [Activo])
VALUES 
    (1, 1, 'Puesto 1', 0, 1), -- Rojo
    (2, 2, 'Puesto 2', 1, 1), -- Verde
    (3, 3, 'Puesto 3', 2, 1), -- Azul
    (4, 4, 'Puesto 4', 3, 1), -- Amarillo
    (5, 5, 'Puesto 5', 4, 1); -- Magenta
SET IDENTITY_INSERT [Puestos] OFF;

GO

-- ============================================================================
-- VERIFICACIÓN
-- ============================================================================

PRINT '✓ Schema created successfully';
PRINT '✓ Seed data inserted';
PRINT '';
PRINT 'Tablas creadas:';
PRINT '  - Articulos';
PRINT '  - Ubicaciones';
PRINT '  - BACs';
PRINT '  - TiposCaja';
PRINT '  - Cajas';
PRINT '  - Puestos';
PRINT '  - Usuarios';
PRINT '  - BAC_Articulos';
PRINT '  - Caja_Articulos';
PRINT '';
PRINT 'Para cargar más datos de prueba, ejecutar Data/SeedData.cs';
