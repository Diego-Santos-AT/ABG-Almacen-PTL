using ABGAlmacenPTL.Models;
using System;
using System.Linq;

namespace ABGAlmacenPTL.Data
{
    /// <summary>
    /// Clase para inicializar datos de prueba en la base de datos
    /// </summary>
    public static class SeedData
    {
        public static void Initialize(ABGAlmacenContext context)
        {
            // Asegurar que la base de datos existe
            context.Database.EnsureCreated();

            // Si ya hay datos, no hacer nada
            if (context.Articulos.Any())
            {
                return; // DB ya tiene datos
            }

            // Seed Tipos de Caja (ya están en OnModelCreating, pero por si acaso)
            if (!context.TiposCaja.Any())
            {
                context.TiposCaja.AddRange(
                    new TipoCaja { TipoId = 1, Nombre = "Pequeña", Descripcion = "Caja pequeña", PesoMaximo = 5.0m, VolumenMaximo = 0.05m, Activo = true },
                    new TipoCaja { TipoId = 2, Nombre = "Mediana", Descripcion = "Caja mediana", PesoMaximo = 15.0m, VolumenMaximo = 0.15m, Activo = true },
                    new TipoCaja { TipoId = 3, Nombre = "Grande", Descripcion = "Caja grande", PesoMaximo = 30.0m, VolumenMaximo = 0.30m, Activo = true }
                );
            }

            // Seed Puestos (ya están en OnModelCreating, pero por si acaso)
            if (!context.Puestos.Any())
            {
                context.Puestos.AddRange(
                    new Puesto { PuestoId = 1, Numero = 1, Nombre = "Puesto 1", Color = ColorPuesto.Rojo, Activo = true, Descripcion = "Puesto de trabajo 1" },
                    new Puesto { PuestoId = 2, Numero = 2, Nombre = "Puesto 2", Color = ColorPuesto.Verde, Activo = true, Descripcion = "Puesto de trabajo 2" },
                    new Puesto { PuestoId = 3, Numero = 3, Nombre = "Puesto 3", Color = ColorPuesto.Azul, Activo = true, Descripcion = "Puesto de trabajo 3" },
                    new Puesto { PuestoId = 4, Numero = 4, Nombre = "Puesto 4", Color = ColorPuesto.Amarillo, Activo = true, Descripcion = "Puesto de trabajo 4" },
                    new Puesto { PuestoId = 5, Numero = 5, Nombre = "Puesto 5", Color = ColorPuesto.Magenta, Activo = true, Descripcion = "Puesto de trabajo 5" }
                );
            }

            // Seed Artículos
            var articulos = new[]
            {
                new Articulo { CodigoArticulo = "ART001", NombreArticulo = "Tornillo M6x20", EAN13 = "8412345678906", CodigoSTD = "STD001", Peso = 0.010m, Volumen = 0.001m, Activo = true },
                new Articulo { CodigoArticulo = "ART002", NombreArticulo = "Tuerca M6", EAN13 = "8412345678913", CodigoSTD = "STD002", Peso = 0.005m, Volumen = 0.0005m, Activo = true },
                new Articulo { CodigoArticulo = "ART003", NombreArticulo = "Arandela plana 6mm", EAN13 = "8412345678920", CodigoSTD = "STD003", Peso = 0.003m, Volumen = 0.0003m, Activo = true },
                new Articulo { CodigoArticulo = "ART004", NombreArticulo = "Cable eléctrico 2.5mm", EAN13 = "8412345678937", CodigoSTD = "STD004", Peso = 0.150m, Volumen = 0.010m, Activo = true },
                new Articulo { CodigoArticulo = "ART005", NombreArticulo = "Interruptor simple", EAN13 = "8412345678944", CodigoSTD = "STD005", Peso = 0.080m, Volumen = 0.005m, Activo = true },
                new Articulo { CodigoArticulo = "ART006", NombreArticulo = "Enchufe schuko", EAN13 = "8412345678951", CodigoSTD = "STD006", Peso = 0.120m, Volumen = 0.008m, Activo = true },
                new Articulo { CodigoArticulo = "ART007", NombreArticulo = "Bombilla LED 10W", EAN13 = "8412345678968", CodigoSTD = "STD007", Peso = 0.050m, Volumen = 0.002m, Activo = true },
                new Articulo { CodigoArticulo = "ART008", NombreArticulo = "Cinta aislante negra", EAN13 = "8412345678975", CodigoSTD = "STD008", Peso = 0.100m, Volumen = 0.004m, Activo = true },
                new Articulo { CodigoArticulo = "ART009", NombreArticulo = "Caja derivación IP65", EAN13 = "8412345678982", CodigoSTD = "STD009", Peso = 0.200m, Volumen = 0.015m, Activo = true },
                new Articulo { CodigoArticulo = "ART010", NombreArticulo = "Tubo corrugado 20mm", EAN13 = "8412345678999", CodigoSTD = "STD010", Peso = 0.250m, Volumen = 0.020m, Activo = true }
            };
            context.Articulos.AddRange(articulos);

            // Seed Ubicaciones (3 almacenes, 5 bloques, 3 filas, 2 alturas cada uno)
            var ubicaciones = new[]
            {
                // Almacén 1
                new Ubicacion { CodigoUbicacion = "001001001001", Almacen = 1, Bloque = 1, Fila = 1, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "001001001002", Almacen = 1, Bloque = 1, Fila = 1, Altura = 2 },
                new Ubicacion { CodigoUbicacion = "001001002001", Almacen = 1, Bloque = 1, Fila = 2, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "001002001001", Almacen = 1, Bloque = 2, Fila = 1, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "001003001001", Almacen = 1, Bloque = 3, Fila = 1, Altura = 1 },
                // Almacén 2
                new Ubicacion { CodigoUbicacion = "002001001001", Almacen = 2, Bloque = 1, Fila = 1, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "002001001002", Almacen = 2, Bloque = 1, Fila = 1, Altura = 2 },
                new Ubicacion { CodigoUbicacion = "002001002001", Almacen = 2, Bloque = 1, Fila = 2, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "002002001001", Almacen = 2, Bloque = 2, Fila = 1, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "002003001001", Almacen = 2, Bloque = 3, Fila = 1, Altura = 1 },
                // Almacén 3
                new Ubicacion { CodigoUbicacion = "003001001001", Almacen = 3, Bloque = 1, Fila = 1, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "003001001002", Almacen = 3, Bloque = 1, Fila = 1, Altura = 2 },
                new Ubicacion { CodigoUbicacion = "003001002001", Almacen = 3, Bloque = 1, Fila = 2, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "003002001001", Almacen = 3, Bloque = 2, Fila = 1, Altura = 1 },
                new Ubicacion { CodigoUbicacion = "003003001001", Almacen = 3, Bloque = 3, Fila = 1, Altura = 1 }
            };
            context.Ubicaciones.AddRange(ubicaciones);
            context.SaveChanges(); // Guardar para tener las ubicaciones disponibles

            // Seed BACs con ubicaciones
            var bacs = new[]
            {
                new BAC { CodigoBAC = "BAC001", Estado = EstadoBAC.Abierto, CodigoUbicacion = "001001001001", Grupo = "G1", Tablilla = "T1", Unidades = 100, Peso = 5.5m, Volumen = 0.15m },
                new BAC { CodigoBAC = "BAC002", Estado = EstadoBAC.Abierto, CodigoUbicacion = "001001001002", Grupo = "G1", Tablilla = "T2", Unidades = 150, Peso = 7.2m, Volumen = 0.20m },
                new BAC { CodigoBAC = "BAC003", Estado = EstadoBAC.Cerrado, CodigoUbicacion = "001001002001", Grupo = "G2", Tablilla = "T1", Unidades = 80, Peso = 4.1m, Volumen = 0.12m },
                new BAC { CodigoBAC = "BAC004", Estado = EstadoBAC.Abierto, CodigoUbicacion = "002001001001", Grupo = "G1", Tablilla = "T3", Unidades = 120, Peso = 6.3m, Volumen = 0.18m },
                new BAC { CodigoBAC = "BAC005", Estado = EstadoBAC.Abierto, CodigoUbicacion = "002001001002", Grupo = "G2", Tablilla = "T1", Unidades = 90, Peso = 5.0m, Volumen = 0.14m },
                new BAC { CodigoBAC = "BAC006", Estado = EstadoBAC.Cerrado, CodigoUbicacion = "002001002001", Grupo = "G3", Tablilla = "T2", Unidades = 110, Peso = 6.8m, Volumen = 0.19m },
                new BAC { CodigoBAC = "BAC007", Estado = EstadoBAC.Abierto, CodigoUbicacion = "003001001001", Grupo = "G1", Tablilla = "T1", Unidades = 140, Peso = 7.5m, Volumen = 0.21m },
                new BAC { CodigoBAC = "BAC008", Estado = EstadoBAC.Abierto, CodigoUbicacion = "003001001002", Grupo = "G2", Tablilla = "T3", Unidades = 95, Peso = 5.3m, Volumen = 0.15m },
                new BAC { CodigoBAC = "BAC009", Estado = EstadoBAC.Abierto, CodigoUbicacion = null, Grupo = "G3", Tablilla = "T1", Unidades = 0, Peso = 0m, Volumen = 0m }, // BAC sin ubicar
                new BAC { CodigoBAC = "BAC010", Estado = EstadoBAC.Abierto, CodigoUbicacion = null, Grupo = "G1", Tablilla = "T2", Unidades = 0, Peso = 0m, Volumen = 0m }  // BAC sin ubicar
            };
            context.BACs.AddRange(bacs);
            context.SaveChanges(); // Guardar BACs para tener IDs

            // Seed Cajas con SSCC
            var cajas = new[]
            {
                new Caja { SSCC = "384123450000000011", TipoCajaId = 2, Estado = EstadoCaja.Abierta, Unidades = 50, Peso = 3.5m, Volumen = 0.08m, FechaCreacion = DateTime.Now.AddDays(-5) },
                new Caja { SSCC = "384123450000000028", TipoCajaId = 2, Estado = EstadoCaja.Cerrada, Unidades = 75, Peso = 5.2m, Volumen = 0.12m, FechaCreacion = DateTime.Now.AddDays(-4), FechaCierre = DateTime.Now.AddDays(-3) },
                new Caja { SSCC = "384123450000000035", TipoCajaId = 3, Estado = EstadoCaja.Abierta, Unidades = 100, Peso = 8.1m, Volumen = 0.18m, FechaCreacion = DateTime.Now.AddDays(-3) },
                new Caja { SSCC = "384123450000000042", TipoCajaId = 1, Estado = EstadoCaja.Cerrada, Unidades = 30, Peso = 2.3m, Volumen = 0.04m, FechaCreacion = DateTime.Now.AddDays(-2), FechaCierre = DateTime.Now.AddDays(-1) },
                new Caja { SSCC = "384123450000000059", TipoCajaId = 2, Estado = EstadoCaja.Abierta, Unidades = 60, Peso = 4.0m, Volumen = 0.10m, FechaCreacion = DateTime.Now.AddDays(-1) }
            };
            context.Cajas.AddRange(cajas);
            context.SaveChanges(); // Guardar cajas para tener SSCCs

            // Seed BACArticulos (artículos en BACs)
            var bacArticulos = new[]
            {
                new BACArticulo { CodigoBAC = "BAC001", CodigoArticulo = "ART001", Cantidad = 50 },
                new BACArticulo { CodigoBAC = "BAC001", CodigoArticulo = "ART002", Cantidad = 50 },
                new BACArticulo { CodigoBAC = "BAC002", CodigoArticulo = "ART003", Cantidad = 75 },
                new BACArticulo { CodigoBAC = "BAC002", CodigoArticulo = "ART004", Cantidad = 75 },
                new BACArticulo { CodigoBAC = "BAC003", CodigoArticulo = "ART005", Cantidad = 80 },
                new BACArticulo { CodigoBAC = "BAC004", CodigoArticulo = "ART006", Cantidad = 60 },
                new BACArticulo { CodigoBAC = "BAC004", CodigoArticulo = "ART007", Cantidad = 60 },
                new BACArticulo { CodigoBAC = "BAC005", CodigoArticulo = "ART008", Cantidad = 90 },
                new BACArticulo { CodigoBAC = "BAC006", CodigoArticulo = "ART009", Cantidad = 55 },
                new BACArticulo { CodigoBAC = "BAC006", CodigoArticulo = "ART010", Cantidad = 55 },
                new BACArticulo { CodigoBAC = "BAC007", CodigoArticulo = "ART001", Cantidad = 70 },
                new BACArticulo { CodigoBAC = "BAC007", CodigoArticulo = "ART002", Cantidad = 70 },
                new BACArticulo { CodigoBAC = "BAC008", CodigoArticulo = "ART003", Cantidad = 95 }
            };
            context.BACArticulos.AddRange(bacArticulos);

            // Seed CajaArticulos (artículos en cajas)
            var cajaArticulos = new[]
            {
                new CajaArticulo { SSCC = "384123450000000011", CodigoArticulo = "ART001", Cantidad = 25 },
                new CajaArticulo { SSCC = "384123450000000011", CodigoArticulo = "ART002", Cantidad = 25 },
                new CajaArticulo { SSCC = "384123450000000028", CodigoArticulo = "ART003", Cantidad = 40 },
                new CajaArticulo { SSCC = "384123450000000028", CodigoArticulo = "ART004", Cantidad = 35 },
                new CajaArticulo { SSCC = "384123450000000035", CodigoArticulo = "ART005", Cantidad = 100 },
                new CajaArticulo { SSCC = "384123450000000042", CodigoArticulo = "ART006", Cantidad = 30 },
                new CajaArticulo { SSCC = "384123450000000059", CodigoArticulo = "ART007", Cantidad = 30 },
                new CajaArticulo { SSCC = "384123450000000059", CodigoArticulo = "ART008", Cantidad = 30 }
            };
            context.CajaArticulos.AddRange(cajaArticulos);

            // Seed Usuarios (contraseñas deberían estar hasheadas en producción)
            var usuarios = new[]
            {
                new Usuario 
                { 
                    NombreUsuario = "admin", 
                    PasswordHash = "admin123", // TODO: Hashear con BCrypt en producción
                    NombreCompleto = "Administrador Sistema",
                    Email = "admin@abgalmacen.com",
                    Empresa = "ABG Almacén",
                    Rol = "Administrador",
                    Activo = true
                },
                new Usuario 
                { 
                    NombreUsuario = "operador1", 
                    PasswordHash = "oper123", // TODO: Hashear con BCrypt en producción
                    NombreCompleto = "Juan Pérez",
                    Email = "jperez@abgalmacen.com",
                    Empresa = "ABG Almacén",
                    Rol = "Operador",
                    Activo = true
                }
            };
            context.Usuarios.AddRange(usuarios);

            // Guardar todos los cambios
            context.SaveChanges();

            Console.WriteLine("Seed data agregado exitosamente.");
        }
    }
}
