// ***********************************************************************
// Nombre: BACModels.cs
// Modelos para BAC, Ubicación, Caja y Artículo - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     EntornoDeDatos.Dsr recordsets
// ***********************************************************************

namespace ABGAlmacenPTL.Maui.Models
{
    /// <summary>
    /// Datos de BAC de PTL - Equivalente a rsDameDatosBACdePTL
    /// </summary>
    public class DatosBACPTL
    {
        public string? Unicod { get; set; }     // Código BAC
        public int Uniest { get; set; }         // Estado BAC (0=Abierto, 1=Cerrado)
        public int Unigru { get; set; }         // Grupo
        public int Unitab { get; set; }         // Tablilla
        public double Unipes { get; set; }      // Peso
        public double Unipma { get; set; }      // Peso máximo
        public double Univol { get; set; }      // Volumen
        public double Univma { get; set; }      // Volumen máximo
        public string? Unicaj { get; set; }     // Tipo caja
        public string? Tipdes { get; set; }     // Descripción tipo caja
        public string? Uninca { get; set; }     // Número caja
        public int? Ubicod { get; set; }        // Código ubicación
        public int Ubialm { get; set; }         // Almacén
        public int Ubiblo { get; set; }         // Bloque
        public int Ubifil { get; set; }         // Fila
        public int Ubialt { get; set; }         // Altura
        public int Uninum { get; set; }         // Número ubicación (0 = sin ubicar)
    }

    /// <summary>
    /// Datos de Ubicación de PTL - Equivalente a rsDameDatosUbicacionPTL
    /// </summary>
    public class DatosUbicacionPTL
    {
        public int Ubicod { get; set; }         // Código ubicación
        public int Ubialm { get; set; }         // Almacén
        public int Ubiblo { get; set; }         // Bloque
        public int Ubifil { get; set; }         // Fila
        public int Ubialt { get; set; }         // Altura
        public string? Unicod { get; set; }     // Código BAC asociado
    }

    /// <summary>
    /// Datos de CAJA de PTL - Equivalente a rsDameDatosCAJAdePTL
    /// </summary>
    public class DatosCajaPTL
    {
        public int Ltcgru { get; set; }         // Grupo
        public int Ltctab { get; set; }         // Tablilla
        public string? Ltccaj { get; set; }     // Número caja
        public int Ltctip { get; set; }         // Tipo caja
        public long Ltcide { get; set; }        // Identificador
        public double Ltcpes { get; set; }      // Peso
        public string? Ltcssc { get; set; }     // SSCC
        public double Ltcvol { get; set; }      // Volumen
        public string? Tipdes { get; set; }     // Descripción tipo
    }

    /// <summary>
    /// Contenido de BAC - Equivalente a rsDameContenidoBacGrupo
    /// </summary>
    public class ContenidoBACGrupo
    {
        public string? Unicod { get; set; }
        public int Unigru { get; set; }
        public int Unitab { get; set; }
        public int Uniart { get; set; }
        public int Unican { get; set; }
        public double Univol { get; set; }
        public int Uniusu { get; set; }
        public DateTime? Unifmd { get; set; }
        public int Unires { get; set; }
        public string? Artnom { get; set; }
        public int Ctapue { get; set; }
    }

    /// <summary>
    /// Contenido de CAJA - Equivalente a rsDameContenidoCajaGrupo
    /// </summary>
    public class ContenidoCajaGrupo
    {
        public int Ltagru { get; set; }
        public int Ltatab { get; set; }
        public int Ltaart { get; set; }
        public string? Ltacaj { get; set; }
        public double Ltacan { get; set; }
        public string? Ltafin { get; set; }
        public int Ltcusu { get; set; }
        public DateTime? Ltcfal { get; set; }
        public DateTime? Ltcfmd { get; set; }
        public double Ltapes { get; set; }
        public double Ltavol { get; set; }
        public long Ltcide { get; set; }
        public string? Artnom { get; set; }
    }

    /// <summary>
    /// Tipo de Caja - Equivalente a rsDameTiposCajasActivas
    /// </summary>
    public class TipoCaja
    {
        public int Tipcod { get; set; }
        public string? Tipdes { get; set; }
    }

    /// <summary>
    /// Artículo - Equivalente a rsDameArticuloConsulta
    /// </summary>
    public class Articulo
    {
        public int Artcod { get; set; }
        public string? Artnom { get; set; }
        public string? Artref { get; set; }
        public string? Artean { get; set; }
        public int Artcj1 { get; set; }
        public int Artcj2 { get; set; }
        public int Artcj3 { get; set; }
        public double Artcub { get; set; }
        public double Artcur { get; set; }
        public double Artpes { get; set; }
        public double Artalt { get; set; }
        public double Artanc { get; set; }
        public double Artlar { get; set; }
        public double Artpea { get; set; }
        public double Artcua { get; set; }
        public int Arttes { get; set; }
        public int Artcla { get; set; }
    }

    /// <summary>
    /// Puesto de trabajo PTL - Equivalente a rsDamePuestosTrabajoPTL
    /// </summary>
    public class PuestoTrabajoPTL
    {
        public int Puecod { get; set; }
        public string? Puedes { get; set; }
        public string? Puecor { get; set; }
        public int Puetip { get; set; }
        public int Pueusu { get; set; }
        public DateTime? Puefal { get; set; }
        public DateTime? Puefmd { get; set; }
        public int Pueimp { get; set; }
        public int Puecol { get; set; }
        public int Puegru { get; set; }
        public string? Puemac { get; set; }
    }

    /// <summary>
    /// Caja de grupo/tablilla PTL - Equivalente a rsDameCajasGrupoTablillaPTL
    /// </summary>
    public class CajaGrupoTablillaPTL
    {
        public int Ltcgru { get; set; }
        public int Ltctab { get; set; }
        public string? Ltccaj { get; set; }
        public int Ltctip { get; set; }
        public long Ltcide { get; set; }
        public double Ltcpes { get; set; }
        public string? Ltcssc { get; set; }
        public double Ltcvol { get; set; }
        public string? Tipdes { get; set; }
    }

    /// <summary>
    /// Resultado de operación con BAC
    /// </summary>
    public class ResultadoOperacion
    {
        public int Retorno { get; set; }
        public string MsgSalida { get; set; } = string.Empty;
        public bool Exitoso => Retorno == 0;
    }
}
