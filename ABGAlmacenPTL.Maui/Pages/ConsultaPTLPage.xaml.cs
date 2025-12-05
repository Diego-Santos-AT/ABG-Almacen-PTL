// ***********************************************************************
// Nombre: ConsultaPTLPage.xaml.cs
// Formulario para consulta de BACs, Cajas y Artículos de PTL
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     frmConsultaPTL.frm de VB6 - línea por línea
//
// Realización:   A.Esteban (VB6) - 02/09/20
// ***********************************************************************

using ABGAlmacenPTL.Maui.Models;
using ABGAlmacenPTL.Maui.Services;

namespace ABGAlmacenPTL.Maui.Pages
{
    public partial class ConsultaPTLPage : ContentPage
    {
        // ----- Constantes de Módulo -------------
        private const string MOD_Nombre = "Consultas PTL";
        private const int CML_Salir = 990;

        // ----- Variables generales -------------
        private DataEnvironment ed = new DataEnvironment();

        public ConsultaPTLPage()
        {
            InitializeComponent();
            MessageService.Instance.SetCurrentPage(this);
        }

        /// <summary>
        /// Form_Load - Inicialización del formulario
        /// </summary>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            try
            {
                Title = MOD_Nombre;
                await ed.OpenAsync();
                
                // Inicializar visualización
                RefrescarDatosBAC(true);
                RefrescarDatosCAJA(true);
                RefrescarDatosArticulo(true);
                
                fraBAC.IsVisible = false;
                fraCAJA.IsVisible = false;
                fraArticulo.IsVisible = false;
            }
            catch (Exception ex)
            {
                await MessageService.Instance.MsgBoxError($"Error al inicializar: {ex.Message}");
            }
            
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// Form_QueryUnload - Cierre del formulario
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ed.Close();
            ed.Dispose();
        }

        /// <summary>
        /// cmdAccion_Click(990) - SALIR
        /// </summary>
        private async void CmdSalir_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// txtLecturaCodigo_GotFocus
        /// </summary>
        private void TxtLecturaCodigo_Focused(object sender, FocusEventArgs e)
        {
            txtLecturaCodigo.BackgroundColor = Color.FromArgb("#C0FFC0");
        }

        /// <summary>
        /// txtLecturaCodigo_LostFocus
        /// </summary>
        private void TxtLecturaCodigo_Unfocused(object sender, FocusEventArgs e)
        {
            txtLecturaCodigo.BackgroundColor = Colors.White;
        }

        /// <summary>
        /// txtLecturaCodigo_KeyDown - Procesamiento de lectura
        /// Equivalente a txtLecturaCodigo_KeyDown de VB6
        /// </summary>
        private async void TxtLecturaCodigo_Completed(object sender, EventArgs e)
        {
            string codigo = txtLecturaCodigo.Text?.Trim() ?? string.Empty;

            // Ocultar todos los frames primero
            fraBAC.IsVisible = false;
            fraCAJA.IsVisible = false;
            fraArticulo.IsVisible = false;

            // VB6: Select Case Len(txtLecturaCodigo.Text)
            switch (codigo.Length)
            {
                case 12:
                    // VB6: Case 12: 'BAC --------------------------
                    if (await fValidarBAC(codigo, true) == false)
                    {
                        await MessageService.Instance.WsMensaje("No existe el BAC", 16);
                    }
                    break;

                case 18:
                    // VB6: Case 18: 'SSCC CAJA --------------------------
                    if (await fValidarSSCC(codigo, true) == false)
                    {
                        await MessageService.Instance.WsMensaje("No existe la CAJA", 16);
                    }
                    break;

                case 20:
                    // VB6: Case 20: 'SSCC CAJA con prefijo --------------------------
                    // VB6: fValidarSSCC Mid(txtLecturaCodigo.Text, 3)
                    if (await fValidarSSCC(codigo[2..], true) == false)
                    {
                        await MessageService.Instance.WsMensaje("No existe la CAJA", 16);
                    }
                    break;

                case 13:
                    // VB6: Case 13: 'EAN13 --------------------------
                    if (await fValidarEAN13(codigo, true) == false)
                    {
                        await MessageService.Instance.WsMensaje("No existe el Artículo", 16);
                    }
                    break;

                case 4:
                case 5:
                    // VB6: Case 4, 5: 'Código Artículo
                    if (await fValidarArticulo(codigo, true) == false)
                    {
                        await MessageService.Instance.WsMensaje("No existe el Artículo", 16);
                    }
                    break;

                default:
                    await MessageService.Instance.WsMensaje("Lectura no válida", 16);
                    break;
            }

            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// fValidarBAC - Valida y muestra datos del BAC
        /// </summary>
        private async Task<bool> fValidarBAC(string stBAC, bool blMensaje = true)
        {
            var datos = await ed.DameDatosBACdePTL(stBAC);

            if (datos != null)
            {
                bool bCalculoPeso = datos.Unipes > datos.Unipma;
                bool bCalculoVolumen = datos.Univol > datos.Univma;

                if (datos.Ubicod == null || datos.Ubicod == 0)
                {
                    RefrescarDatosBAC(false, 0, 0, 0, 0, 0, datos.Unicod!, datos.Uniest, 
                        datos.Unigru, datos.Unitab, datos.Unipes, datos.Univol, 
                        datos.Unicaj ?? "", datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);
                }
                else
                {
                    RefrescarDatosBAC(false, datos.Ubicod.Value, datos.Ubialm, datos.Ubiblo, 
                        datos.Ubifil, datos.Ubialt, datos.Unicod!, datos.Uniest, 
                        datos.Unigru, datos.Unitab, datos.Unipes, datos.Univol, 
                        datos.Unicaj ?? "", datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);
                }

                fraBAC.IsVisible = true;
                return true;
            }
            else
            {
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe el BAC", 16);
                }
            }

            return false;
        }

        /// <summary>
        /// fValidarSSCC - Valida y muestra datos de la CAJA
        /// </summary>
        private async Task<bool> fValidarSSCC(string stSSCC, bool blMensaje = true)
        {
            var datos = await ed.DameDatosCAJAdePTL(stSSCC);

            if (datos != null)
            {
                RefrescarDatosCAJA(false, datos.Ltcssc ?? "", datos.Ltcgru, datos.Ltctab, 
                    datos.Ltcpes, datos.Ltcvol, datos.Tipdes ?? "");
                
                fraCAJA.IsVisible = true;
                return true;
            }
            else
            {
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe la CAJA", 16);
                }
            }

            return false;
        }

        /// <summary>
        /// fValidarArticulo - Valida y muestra datos del artículo
        /// </summary>
        private async Task<bool> fValidarArticulo(string stArticulo, bool blMensaje = true)
        {
            if (!int.TryParse(stArticulo, out int nArticulo))
                return false;

            var articulo = await ed.DameArticuloConsulta(nArticulo);

            if (articulo != null)
            {
                RefrescarDatosArticulo(false, articulo.Artcod, articulo.Artnom ?? "", 
                    articulo.Artean ?? "", articulo.Artcj3, articulo.Artpea, articulo.Artcua);
                
                fraArticulo.IsVisible = true;
                return true;
            }
            else
            {
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe el Artículo", 16);
                }
            }

            return false;
        }

        /// <summary>
        /// fValidarEAN13 - Valida y muestra datos del artículo por EAN13
        /// </summary>
        private async Task<bool> fValidarEAN13(string stEAN13, bool blMensaje = true)
        {
            string tEAN13 = stEAN13.Length > 13 ? stEAN13[..13] : stEAN13;

            var articulo = await ed.DameArticuloEAN13(tEAN13);

            if (articulo != null)
            {
                RefrescarDatosArticulo(false, articulo.Artcod, articulo.Artnom ?? "", 
                    articulo.Artean ?? "", articulo.Artcj3, articulo.Artpea, articulo.Artcua);
                
                fraArticulo.IsVisible = true;
                return true;
            }
            else
            {
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe el Artículo", 16);
                }
            }

            return false;
        }

        /// <summary>
        /// RefrescarDatosBAC - Actualiza la visualización de datos del BAC
        /// </summary>
        private void RefrescarDatosBAC(bool sEnBlanco, int sCodUbicacion = 0, int sALM = 0, 
            int sBLO = 0, int sFIL = 0, int sALT = 0, string sBAC = "", int sEstadoBAC = 0,
            int sGrupo = 0, int sTablilla = 0, double sPeso = 0, double sVolumen = 0,
            string sTipoCaja = "", string sNombreCaja = "", 
            bool bPeso = false, bool bVolumen = false)
        {
            if (sEnBlanco)
            {
                lbUbicacion.Text = "";
                lbBAC.Text = "";
                lbEstadoBAC.Text = "";
                lbEstadoBAC.BackgroundColor = Colors.White;
                lbGrupo.Text = "";
                lbTablilla.Text = "";
                lbPeso.Text = "";
                lbPeso.BackgroundColor = Colors.White;
                lbVolumen.Text = "";
                lbVolumen.BackgroundColor = Colors.White;
                lbTipoCaja.Text = "";
                lbNombreCaja.Text = "";
            }
            else
            {
                lbUbicacion.Text = sCodUbicacion == 0 ? "SIN UBICACION" : 
                    GeneralFunctions.FormatearUbicacion(sCodUbicacion, sALM, sBLO, sFIL, sALT);
                lbBAC.Text = sBAC;
                lbEstadoBAC.Text = sEstadoBAC == 0 ? "ABIERTO" : "CERRADO";
                lbEstadoBAC.BackgroundColor = sEstadoBAC == 0 ? Colors.White : Constants.ColorVerde;
                lbGrupo.Text = sGrupo.ToString();
                lbTablilla.Text = sTablilla.ToString();
                lbPeso.Text = sPeso.ToString("#0.000");
                lbPeso.BackgroundColor = bPeso ? Constants.ColorRojo : Colors.White;
                lbVolumen.Text = sVolumen.ToString("#0.000");
                lbVolumen.BackgroundColor = bVolumen ? Constants.ColorRojo : Colors.White;
                lbTipoCaja.Text = sTipoCaja;
                lbNombreCaja.Text = sNombreCaja;
            }
        }

        /// <summary>
        /// RefrescarDatosCAJA - Actualiza la visualización de datos de la CAJA
        /// </summary>
        private void RefrescarDatosCAJA(bool sEnBlanco, string sSSCC = "", 
            int sGrupo = 0, int sTablilla = 0, double sPeso = 0, double sVolumen = 0,
            string sTipo = "")
        {
            if (sEnBlanco)
            {
                lbSSCC.Text = "";
                lbGrupoCaja.Text = "";
                lbTablillaCaja.Text = "";
                lbPesoCaja.Text = "";
                lbVolumenCaja.Text = "";
                lbTipoCajaCaja.Text = "";
            }
            else
            {
                lbSSCC.Text = sSSCC;
                lbGrupoCaja.Text = sGrupo.ToString();
                lbTablillaCaja.Text = sTablilla.ToString();
                lbPesoCaja.Text = sPeso.ToString("#0.000");
                lbVolumenCaja.Text = sVolumen.ToString("#0.000");
                lbTipoCajaCaja.Text = sTipo;
            }
        }

        /// <summary>
        /// RefrescarDatosArticulo - Actualiza la visualización de datos del artículo
        /// </summary>
        private void RefrescarDatosArticulo(bool sEnBlanco, int sArticulo = 0, string sNombre = "",
            string sEAN13 = "", int sSTD = 0, double sPeso = 0, double sVolumen = 0)
        {
            if (sEnBlanco)
            {
                lbArticulo.Text = "";
                lbNombreArticulo.Text = "";
                lbEAN13.Text = "";
                lbSTD.Text = "";
                lbPesoArt.Text = "";
                lbVolumenArt.Text = "";
            }
            else
            {
                lbArticulo.Text = sArticulo.ToString();
                lbNombreArticulo.Text = sNombre;
                lbEAN13.Text = sEAN13;
                lbSTD.Text = sSTD.ToString();
                lbPesoArt.Text = sPeso.ToString("#0.0000");
                lbVolumenArt.Text = sVolumen.ToString("#0.0000");
            }
        }
    }
}
