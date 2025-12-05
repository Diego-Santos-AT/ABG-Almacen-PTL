// ***********************************************************************
// Nombre: GeneralFunctions.cs
// Funciones Generales - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     wsFuncionesGenerales.bas
// ***********************************************************************

namespace ABGAlmacenPTL.Maui.Services
{
    /// <summary>
    /// Funciones Generales - Equivalente a wsFuncionesGenerales.bas de VB6
    /// </summary>
    public static class GeneralFunctions
    {
        /// <summary>
        /// Calcula el dígito de control EAN
        /// Equivalente a Dame_Digito_Control_EAN de VB6
        /// </summary>
        public static int DameDigitoControlEAN(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
                return 0;

            int suma = 0;
            bool esImpar = true;

            for (int i = codigo.Length - 1; i >= 0; i--)
            {
                if (int.TryParse(codigo[i].ToString(), out int digito))
                {
                    suma += esImpar ? digito * 3 : digito;
                    esImpar = !esImpar;
                }
            }

            int resto = suma % 10;
            return resto == 0 ? 0 : 10 - resto;
        }

        /// <summary>
        /// Genera un SSCC
        /// Equivalente a Dame_SSCC de VB6
        /// </summary>
        public static string DameSSCC(int codigoEmpresa, string eanEmpresa, long numerador, int incrementoSerie)
        {
            // Formato SSCC: 0 + EAN empresa (7 dígitos) + Serie (9 dígitos) + Dígito control
            // VB6: stSSCC = "0" & Mid(EAN, 1, 7) & CStr(Format(Numerador, "000000000"))
            string ssccSinDigito = $"0{eanEmpresa[..Math.Min(7, eanEmpresa.Length)]}{numerador + incrementoSerie:D9}";
            int digitoControl = DameDigitoControlEAN(ssccSinDigito);
            return $"{ssccSinDigito}{digitoControl}";
        }

        /// <summary>
        /// Formatea una ubicación para mostrar
        /// Equivalente a la construcción de lbUbicacion en VB6
        /// </summary>
        public static string FormatearUbicacion(int codigo, int alm, int blo, int fil, int alt)
        {
            if (codigo == 0)
                return "SIN UBICACION";
            
            return $"({codigo}) {alm:D3}.{blo:D3}.{fil:D3}.{alt:D3}";
        }

        /// <summary>
        /// Parsea una ubicación desde código de barras
        /// Equivalente al parsing en txtLecturaCodigo_KeyDown de frmUbicarBAC
        /// </summary>
        public static (int alm, int blo, int fil, int alt) ParsearUbicacion(string codigo)
        {
            if (string.IsNullOrEmpty(codigo) || codigo.Length != 12)
                return (0, 0, 0, 0);

            // Formato: AAABBBFFFAAA (3 dígitos cada uno)
            int alm = int.TryParse(codigo[..3], out int almVal) ? almVal : 0;
            int blo = int.TryParse(codigo.Substring(3, 3), out int bloVal) ? bloVal : 0;
            int fil = int.TryParse(codigo.Substring(6, 3), out int filVal) ? filVal : 0;
            int alt = int.TryParse(codigo.Substring(9, 3), out int altVal) ? altVal : 0;

            return (alm, blo, fil, alt);
        }

        /// <summary>
        /// Valida un código EAN13
        /// </summary>
        public static bool ValidarEAN13(string ean)
        {
            if (string.IsNullOrEmpty(ean) || ean.Length != 13)
                return false;

            if (!long.TryParse(ean, out _))
                return false;

            // Verificar dígito de control
            string sinDigito = ean[..12];
            int digitoCalculado = DameDigitoControlEAN(sinDigito);
            int digitoReal = int.Parse(ean[12].ToString());

            return digitoCalculado == digitoReal;
        }

        /// <summary>
        /// Convierte un color BGR (VB6) a Color (MAUI)
        /// VB6 usa formato BGR: &H00BBGGRR
        /// </summary>
        public static Color ColorFromBGR(long bgrColor)
        {
            int b = (int)((bgrColor >> 16) & 0xFF);
            int g = (int)((bgrColor >> 8) & 0xFF);
            int r = (int)(bgrColor & 0xFF);
            return Color.FromRgb(r, g, b);
        }

        /// <summary>
        /// Obtiene el color según el código de puesto
        /// Equivalente a Combo1_Click en frmRepartirArticulo
        /// </summary>
        public static Color GetColorPuesto(int colorCode)
        {
            return colorCode switch
            {
                1 => Colors.White,      // vbWhite
                2 => Colors.Yellow,     // vbYellow
                3 => Colors.Magenta,    // vbMagenta
                4 => Colors.Cyan,       // vbCyan
                5 => Colors.Blue,       // vbBlue
                6 => Colors.Green,      // vbGreen
                7 => Colors.Red,        // vbRed
                _ => Color.FromRgb(128, 128, 128)  // Gris por defecto
            };
        }

        /// <summary>
        /// Extrae el código de puesto del texto del combo
        /// Equivalente a CInt(Mid(Combo1.Text, 2, 3)) de VB6
        /// </summary>
        public static int ExtraerCodigoPuesto(string textoCombo)
        {
            if (string.IsNullOrEmpty(textoCombo) || textoCombo.Length < 4)
                return 0;

            // Formato esperado: "(XXX) Descripción"
            if (textoCombo.StartsWith("(") && textoCombo.Contains(")"))
            {
                int cierre = textoCombo.IndexOf(')');
                string numero = textoCombo.Substring(1, cierre - 1);
                return int.TryParse(numero, out int codigo) ? codigo : 0;
            }

            return 0;
        }

        /// <summary>
        /// Limpia el SSCC si viene con prefijo
        /// Equivalente a Mid(txtLecturaCodigo.Text, 3) cuando Length = 20
        /// </summary>
        public static string LimpiarSSCC(string sscc)
        {
            if (string.IsNullOrEmpty(sscc))
                return string.Empty;

            // Si viene con prefijo de 2 caracteres, quitarlo
            if (sscc.Length == 20)
                return sscc[2..];

            return sscc;
        }

        /// <summary>
        /// Determina el tipo de lectura según la longitud
        /// Equivalente a Select Case Len(txtLecturaCodigo.Text) de VB6
        /// </summary>
        public static TipoLectura DeterminarTipoLectura(string codigo)
        {
            if (string.IsNullOrEmpty(codigo))
                return TipoLectura.Desconocido;

            return codigo.Length switch
            {
                4 or 5 => TipoLectura.CodigoArticulo,
                12 => TipoLectura.BACOUbicacion,
                13 => TipoLectura.EAN13,
                18 => TipoLectura.SSCC,
                20 => TipoLectura.SSCCConPrefijo,
                _ => TipoLectura.Desconocido
            };
        }
    }

    /// <summary>
    /// Tipos de lectura de código de barras
    /// </summary>
    public enum TipoLectura
    {
        Desconocido,
        CodigoArticulo,
        BACOUbicacion,
        EAN13,
        SSCC,
        SSCCConPrefijo
    }
}
