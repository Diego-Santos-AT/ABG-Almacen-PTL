// ***********************************************************************
// Nombre: UbicarBACPage.xaml.cs
// Formulario para la ubicación de un BAC en una ubicación de PTL
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     frmUbicarBAC.frm de VB6 - línea por línea
//
// Realización:   A.Esteban (VB6) - 05/06/20
// ***********************************************************************

using ABGAlmacenPTL.Maui.Models;
using ABGAlmacenPTL.Maui.Services;

namespace ABGAlmacenPTL.Maui.Pages
{
    public partial class UbicarBACPage : ContentPage
    {
        // ----- Constantes de Módulo -------------
        private const string MOD_Nombre = "Ubicar BAC";
        private const int CML_Salir = 990;
        private const int CML_Cancelar = 0;

        // ----- Variables generales -------------
        private DataEnvironment ed = new DataEnvironment();

        // Variables de estado (equivalentes a VB6)
        private string stBAC = string.Empty;
        private int tUbicacion = 0;

        public UbicarBACPage()
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
            
            // VB6: MousePointer = ccHourglass
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
            
            // VB6: If ed.GestionAlmacen.State <> adStateClosed Then ed.GestionAlmacen.Close
            // VB6: Set ed = Nothing
            ed.Close();
            ed.Dispose();
        }

        /// <summary>
        /// cmdAccion_Click(990) - SALIR
        /// </summary>
        private async void CmdSalir_Clicked(object sender, EventArgs e)
        {
            // VB6: Unload Me
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// cmdAccion_Click(0) - CANCELAR
        /// </summary>
        private void CmdCancelar_Clicked(object sender, EventArgs e)
        {
            // VB6: RefrescarDatos True
            RefrescarDatos(true);
            stBAC = string.Empty;
            tUbicacion = 0;
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// oEstado_Click - Cambio de opción de estado
        /// </summary>
        private void OEstado_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // VB6: txtLecturaCodigo.SetFocus
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// txtLecturaCodigo_GotFocus
        /// </summary>
        private void TxtLecturaCodigo_Focused(object sender, FocusEventArgs e)
        {
            // VB6: txtLecturaCodigo.BackColor = &HC0FFC0
            txtLecturaCodigo.BackgroundColor = Color.FromArgb("#C0FFC0");
        }

        /// <summary>
        /// txtLecturaCodigo_LostFocus
        /// </summary>
        private void TxtLecturaCodigo_Unfocused(object sender, FocusEventArgs e)
        {
            // VB6: txtLecturaCodigo.BackColor = &H80000005
            txtLecturaCodigo.BackgroundColor = Colors.White;
        }

        /// <summary>
        /// txtLecturaCodigo_KeyDown - Procesamiento de lectura de código
        /// Equivalente a txtLecturaCodigo_KeyDown de VB6
        /// </summary>
        private async void TxtLecturaCodigo_Completed(object sender, EventArgs e)
        {
            // VB6: If KeyCode = vbKeyReturn Then
            string codigo = txtLecturaCodigo.Text?.Trim() ?? string.Empty;

            // VB6: Select Case Len(txtLecturaCodigo.Text)
            switch (codigo.Length)
            {
                case 12:
                    // VB6: Case 12: 'Unidad de transporte o Ubicación --------------------------
                    if (string.IsNullOrEmpty(stBAC))
                    {
                        // VB6: 'Comprobar si la lectura es un BAC
                        // VB6: If fValidarBAC(txtLecturaCodigo.Text, True) = False Then
                        if (await fValidarBAC(codigo, true) == false)
                        {
                            // VB6: 'Comprobar si la lectura es una UBICACION de PTL
                            await fValidarUbicacion(codigo, true);
                        }
                    }
                    else
                    {
                        // VB6: 'Comprobar si la lectura es una UBICACION de PTL
                        if (await fValidarUbicacion(codigo, true) == false)
                        {
                            // VB6: 'Comprobar si la lectura es otro BAC
                            if (await fValidarBAC(codigo, true) == false)
                            {
                                // VB6: 'No existe la Ubicación ni otro BAC
                                await MessageService.Instance.WsMensaje("No existe la Ubicación ni otro BAC", 16);
                            }
                        }
                    }
                    break;

                default:
                    // VB6: Case Else
                    await MessageService.Instance.WsMensaje("Lectura no válida. Introduzca BAC o Ubicación", 16);
                    break;
            }

            // VB6: txtLecturaCodigo.Text = ""
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// fValidarBAC - Valida y carga datos del BAC
        /// Equivalente a fValidarBAC de VB6
        /// </summary>
        private async Task<bool> fValidarBAC(string stBACInput, bool blMensaje = true)
        {
            // VB6: fValidarBAC = False
            // VB6: If ed.rsDameDatosBACdePTL.State <> adStateClosed Then ed.rsDameDatosBACdePTL.Close
            // VB6: ed.DameDatosBACdePTL stBAC

            var datos = await ed.DameDatosBACdePTL(stBACInput);

            // VB6: With ed.rsDameDatosBACdePTL
            // VB6:     'Existencia del registro
            // VB6:     If .RecordCount > 0 Then
            if (datos != null)
            {
                // VB6: fValidarBAC = True
                // VB6: bCalculoPeso = !unipes > !unipma
                // VB6: bCalculoVolumen = !univol > !univma
                bool bCalculoPeso = datos.Unipes > datos.Unipma;
                bool bCalculoVolumen = datos.Univol > datos.Univma;

                // VB6: 'Se muestran los datos
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

                // VB6: stBAC = stBAC
                stBAC = stBACInput;

                return true;
            }
            else
            {
                // VB6: 'No se ha encontrado el BAC
                // VB6: If blMensaje Then Call wsMensaje(" No existe el BAC ", vbCritical)
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe el BAC", 16);
                }
            }

            return false;
        }

        /// <summary>
        /// fValidarUbicacion - Valida y procesa ubicación
        /// Equivalente a fValidarUbicacion de VB6
        /// </summary>
        private async Task<bool> fValidarUbicacion(string stUbicacion, bool blMensaje = true)
        {
            // VB6: fValidarUbicacion = False
            // VB6: tALF = CInt(Mid(stUbicacion, 1, 3))
            // VB6: tALM = CInt(Mid(stUbicacion, 4, 3))
            // VB6: tBLO = CInt(Mid(stUbicacion, 7, 3))
            // VB6: tFIL = CInt(Mid(stUbicacion, 10, 3))
            // VB6: tALT = CInt(Mid(stUbicacion, 13, 3))

            var (alm, blo, fil, alt) = GeneralFunctions.ParsearUbicacion(stUbicacion);
            int alf = alm; // En VB6 parece que ALF y ALM son lo mismo

            // VB6: If ed.rsDameDatosUbicacionPTL.State <> adStateClosed Then ed.rsDameDatosUbicacionPTL.Close
            // VB6: ed.DameDatosUbicacionPTL tALF, tALM, tBLO, tFIL, tALT

            var datos = await ed.DameDatosUbicacionPTL(alf, alm, blo, fil, alt);

            // VB6: If .RecordCount > 0 Then
            if (datos != null)
            {
                // VB6: fValidarUbicacion = True
                tUbicacion = datos.Ubicod;

                // VB6: 'La ubicación existe, procede a ubicar el BAC
                // VB6: 'Primero comprueba que la ubicación no esté ocupada
                // VB6: If IsNull(!unicod) = False Then
                if (!string.IsNullOrEmpty(datos.Unicod))
                {
                    // VB6: If !unicod <> stBAC Then
                    if (datos.Unicod != stBAC)
                    {
                        // VB6: 'La ubicación está ocupada por otro BAC
                        // VB6: Call wsMensaje(" La ubicación está ocupada por otro BAC: " & !unicod, vbCritical)
                        await MessageService.Instance.WsMensaje(
                            $"La ubicación está ocupada por otro BAC: {datos.Unicod}", 16);
                        return true;
                    }
                }

                // VB6: 'Procede a ubicar el BAC
                // VB6: If UbicarBAC(!ubicod, stBAC) Then
                if (await UbicarBAC(tUbicacion, stBAC))
                {
                    // VB6: Call wsMensaje(" Se ha ubicado el BAC en: " & !ubicod, MENSAJE_Exclamacion)
                    await MessageService.Instance.WsMensaje(
                        $"Se ha ubicado el BAC en: {tUbicacion}", Constants.MENSAJE_Exclamacion);

                    // VB6: RefrescarDatos True
                    RefrescarDatos(true);
                    stBAC = string.Empty;
                }

                return true;
            }
            else
            {
                // VB6: 'No se ha encontrado la Ubicación
                // VB6: If blMensaje Then Call wsMensaje(" No existe la Ubicación ", vbCritical)
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe la Ubicación", 16);
                }
            }

            return false;
        }

        /// <summary>
        /// UbicarBAC - Ubica un BAC en una ubicación
        /// Equivalente a UbicarBAC de VB6
        /// </summary>
        private async Task<bool> UbicarBAC(int ubicacion, string bac)
        {
            // VB6: UbicarBAC = False
            // VB6: 'Si el estado del BAC es cerrar se pasa a cerrado
            // VB6: If oEstado(0).Value = True Then tEstado = 1 Else tEstado = 0
            int tEstado = oEstadoCerrar.IsChecked ? 1 : 0;

            // VB6: ed.UbicarBACenPTL bac, ubicacion, tEstado, Usuario.Id, Retorno, msgSalida
            var resultado = await ed.UbicarBACenPTL(bac, ubicacion, AppSettings.Instance.Usuario.Id);

            // VB6: If Retorno = 0 Then
            if (resultado.Exitoso)
            {
                return true;
            }
            else
            {
                // VB6: Call wsMensaje(" No se ha podido ubicar el BAC. " & msgSalida, vbCritical)
                await MessageService.Instance.WsMensaje(
                    $"No se ha podido ubicar el BAC. {resultado.MsgSalida}", 16);
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
                // VB6: 'Inicia la visualización
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
                // VB6: lbUbicacion = "(" & sCodUbicacion & ") " & ...
                if (sCodUbicacion == 0)
                {
                    lbUbicacion.Text = "SIN UBICACION";
                }
                else
                {
                    lbUbicacion.Text = GeneralFunctions.FormatearUbicacion(sCodUbicacion, sALM, sBLO, sFIL, sALT);
                }

                lbBAC.Text = sBAC;
                
                // VB6: lbEstadoBAC = IIf(sEstadoBAC = 0, "ABIERTO", "CERRADO")
                lbEstadoBAC.Text = sEstadoBAC == 0 ? "ABIERTO" : "CERRADO";
                // VB6: If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde
                lbEstadoBAC.BackgroundColor = sEstadoBAC == 0 ? Colors.White : Constants.ColorVerde;

                lbGrupo.Text = sGrupo.ToString();
                lbTablilla.Text = sTablilla.ToString();
                lbUds.Text = "0"; // Se actualiza después

                // VB6: lbPeso = Format(sPeso, "#0.000")
                lbPeso.Text = sPeso.ToString("#0.000");
                // VB6: If bPeso Then lbPeso.BackColor = ColorRojo
                lbPeso.BackgroundColor = bPeso ? Constants.ColorRojo : Colors.White;

                // VB6: lbVolumen = Format(sVolumen, "#0.000")
                lbVolumen.Text = sVolumen.ToString("#0.000");
                // VB6: If bVolumen Then lbVolumen.BackColor = ColorRojo
                lbVolumen.BackgroundColor = bVolumen ? Constants.ColorRojo : Colors.White;

                lbTipoCaja.Text = sTipoCaja;
                lbNombreCaja.Text = sNombreCaja;
            }
        }
    }
}
