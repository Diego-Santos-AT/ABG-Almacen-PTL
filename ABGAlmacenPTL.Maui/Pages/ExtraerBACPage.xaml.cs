// ***********************************************************************
// Nombre: ExtraerBACPage.xaml.cs
// Formulario para la extracción de un BAC de una ubicación de PTL
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     frmExtraerBAC.frm de VB6 - línea por línea
//
// Realización:   A.Esteban (VB6) - 05/06/20
// ***********************************************************************

using ABGAlmacenPTL.Maui.Models;
using ABGAlmacenPTL.Maui.Services;

namespace ABGAlmacenPTL.Maui.Pages
{
    public partial class ExtraerBACPage : ContentPage
    {
        // ----- Constantes de Módulo -------------
        private const string MOD_Nombre = "Extraer BAC";
        private const int CML_Salir = 990;
        private const int CML_Cancelar = 0;

        // ----- Variables generales -------------
        private DataEnvironment ed = new DataEnvironment();

        // Variables de estado (equivalentes a VB6)
        private string stBAC = string.Empty;

        public ExtraerBACPage()
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
                // VB6: Me.Caption = MOD_Nombre
                Title = MOD_Nombre;

                // VB6: If ed.GestionAlmacen.State <> adStateClosed Then ed.GestionAlmacen.Close
                // VB6: ed.GestionAlmacen.Open ConexionGestionAlmacen
                await ed.OpenAsync();

                // Inicializar opciones
                oEstadoAbrir.IsChecked = true;
            }
            catch (Exception ex)
            {
                await MessageService.Instance.MsgBoxError($"Error al inicializar: {ex.Message}");
            }
            
            // Dar foco al campo de lectura
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
        /// cmdAccion_Click(0) - CANCELAR
        /// </summary>
        private void CmdCancelar_Clicked(object sender, EventArgs e)
        {
            RefrescarDatos(true);
            stBAC = string.Empty;
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// oEstado_Click - Cambio de opción de estado
        /// </summary>
        private void OEstado_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            txtLecturaCodigo.Focus();
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
        /// txtLecturaCodigo_KeyDown - Procesamiento de lectura de código
        /// Equivalente a txtLecturaCodigo_KeyDown de VB6
        /// </summary>
        private async void TxtLecturaCodigo_Completed(object sender, EventArgs e)
        {
            string codigo = txtLecturaCodigo.Text?.Trim() ?? string.Empty;

            // VB6: Select Case Len(txtLecturaCodigo.Text)
            switch (codigo.Length)
            {
                case 12:
                    // VB6: Case 12: 'BAC --------------------------
                    // VB6: 'Comprobar si la lectura es un BAC
                    if (await fValidarBAC(codigo, true) == false)
                    {
                        await MessageService.Instance.WsMensaje("No existe el BAC", 16);
                    }
                    break;

                default:
                    await MessageService.Instance.WsMensaje("Lectura no válida. Introduzca BAC", 16);
                    break;
            }

            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// fValidarBAC - Valida y carga datos del BAC
        /// Equivalente a fValidarBAC de VB6
        /// </summary>
        private async Task<bool> fValidarBAC(string stBACInput, bool blMensaje = true)
        {
            var datos = await ed.DameDatosBACdePTL(stBACInput);

            if (datos != null)
            {
                bool bCalculoPeso = datos.Unipes > datos.Unipma;
                bool bCalculoVolumen = datos.Univol > datos.Univma;

                // VB6: 'Comprueba que el BAC esté ubicado
                // VB6: If IsNull(!ubicod) = True Then
                if (datos.Uninum == 0)
                {
                    // VB6: 'El BAC no está ubicado
                    // VB6: Call wsMensaje(" El BAC no está ubicado ", vbCritical)
                    await MessageService.Instance.WsMensaje("El BAC no está ubicado", 16);
                    return true;
                }

                // Se muestran los datos
                if (datos.Ubicod == null || datos.Ubicod == 0)
                {
                    RefrescarDatos(false, 0, 0, 0, 0, 0, datos.Unicod!, datos.Uniest, 
                        datos.Unigru, datos.Unitab, datos.Unipes, datos.Univol, 
                        datos.Unicaj ?? "", datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);
                }
                else
                {
                    RefrescarDatos(false, datos.Ubicod.Value, datos.Ubialm, datos.Ubiblo, 
                        datos.Ubifil, datos.Ubialt, datos.Unicod!, datos.Uniest, 
                        datos.Unigru, datos.Unitab, datos.Unipes, datos.Univol, 
                        datos.Unicaj ?? "", datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);
                }

                stBAC = stBACInput;

                // VB6: 'Procede a extraer el BAC
                // VB6: If ExtraerBAC(stBAC) Then
                if (await ExtraerBAC(stBAC))
                {
                    await MessageService.Instance.WsMensaje(
                        $"Se ha extraído el BAC: {stBAC}", Constants.MENSAJE_Exclamacion);
                    RefrescarDatos(true);
                    stBAC = string.Empty;
                }

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
        /// ExtraerBAC - Extrae un BAC de su ubicación
        /// Equivalente a ExtraerBAC de VB6
        /// </summary>
        private async Task<bool> ExtraerBAC(string bac)
        {
            // VB6: ExtraerBAC = False
            // VB6: 'Si el estado del BAC es cerrar se pasa a cerrado
            // VB6: If oEstado(0).Value = True Then tEstado = 1 Else tEstado = 0
            int tEstado = oEstadoCerrar.IsChecked ? 1 : 0;

            // VB6: ed.RetirarBACdePTL bac, Usuario.Id, tEstado, Retorno, msgSalida
            var resultado = await ed.RetirarBACdePTL(bac, AppSettings.Instance.Usuario.Id);

            if (resultado.Exitoso)
            {
                return true;
            }
            else
            {
                await MessageService.Instance.WsMensaje(
                    $"No se ha podido extraer el BAC. {resultado.MsgSalida}", 16);
            }

            return false;
        }

        /// <summary>
        /// RefrescarDatos - Refresca los datos mostrados en pantalla
        /// Equivalente a RefrescarDatos de VB6
        /// </summary>
        private void RefrescarDatos(bool sEnBlanco, int sCodUbicacion = 0, int sALM = 0, 
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
                lbUds.Text = "";
                lbPeso.Text = "";
                lbPeso.BackgroundColor = Colors.White;
                lbVolumen.Text = "";
                lbVolumen.BackgroundColor = Colors.White;
                lbTipoCaja.Text = "";
                lbNombreCaja.Text = "";
            }
            else
            {
                if (sCodUbicacion == 0)
                {
                    lbUbicacion.Text = "SIN UBICACION";
                }
                else
                {
                    lbUbicacion.Text = GeneralFunctions.FormatearUbicacion(sCodUbicacion, sALM, sBLO, sFIL, sALT);
                }

                lbBAC.Text = sBAC;
                lbEstadoBAC.Text = sEstadoBAC == 0 ? "ABIERTO" : "CERRADO";
                lbEstadoBAC.BackgroundColor = sEstadoBAC == 0 ? Colors.White : Constants.ColorVerde;

                lbGrupo.Text = sGrupo.ToString();
                lbTablilla.Text = sTablilla.ToString();
                lbUds.Text = "0";

                lbPeso.Text = sPeso.ToString("#0.000");
                lbPeso.BackgroundColor = bPeso ? Constants.ColorRojo : Colors.White;

                lbVolumen.Text = sVolumen.ToString("#0.000");
                lbVolumen.BackgroundColor = bVolumen ? Constants.ColorRojo : Colors.White;

                lbTipoCaja.Text = sTipoCaja;
                lbNombreCaja.Text = sNombreCaja;
            }
        }
    }
}
