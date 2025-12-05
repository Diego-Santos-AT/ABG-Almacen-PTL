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
        /// Equivalente a fValidarBAC de VB6 - frmUbicarBAC.frm líneas 454-524
        /// </summary>
        private async Task<bool> fValidarBAC(string stBACInput, bool blMensaje = true)
        {
            // VB6: fValidarBAC = False
            bool bCalculoPeso = false;
            bool bCalculoVolumen = false;
            int tEstado = 0;

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
                bCalculoPeso = datos.Unipes > datos.Unipma;
                bCalculoVolumen = datos.Univol > datos.Univma;

                // VB6: 'Se muestran los datos
                if (datos.Ubicod == null || datos.Ubicod == 0)
                {
                    // VB6: RefrescarDatos False, 0, 0, 0, 0, 0, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, bCalculoPeso, bCalculoVolumen
                    RefrescarDatos(false, 0, 0, 0, 0, 0, datos.Unicod!, datos.Uniest, 
                        datos.Unigru, datos.Unitab, datos.Unipes, datos.Univol, 
                        datos.Unicaj ?? "", datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);
                }
                else
                {
                    // VB6: RefrescarDatos False, !ubicod, !ubialm, !ubiblo, !ubifil, !ubialt, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, bCalculoPeso, bCalculoVolumen
                    RefrescarDatos(false, datos.Ubicod.Value, datos.Ubialm, datos.Ubiblo, 
                        datos.Ubifil, datos.Ubialt, datos.Unicod!, datos.Uniest, 
                        datos.Unigru, datos.Unitab, datos.Unipes, datos.Univol, 
                        datos.Unicaj ?? "", datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);
                }

                // VB6: If !uninum > 0 Then
                if (datos.Uninum > 0)
                {
                    // VB6: wsMensaje " El BAC ya se encuentra ubicado ", vbCritical
                    await MessageService.Instance.WsMensaje(" El BAC ya se encuentra ubicado ", 16);
                    RefrescarDatos(true);
                    tUbicacion = 0;
                }
                else
                {
                    // VB6: If iUbicacion > 0 Then
                    if (tUbicacion > 0)
                    {
                        // VB6: 'Se ha leido la ubicación anteriormente. Se procede a ubicar el BAC
                        // VB6: If UbicarBAC(!unicod, iUbicacion, !uniest, oEstado(0).Value) Then
                        if (await UbicarBAC(tUbicacion, datos.Unicod!, datos.Uniest))
                        {
                            // VB6: Call wsMensaje(" Se ha ubicado el BAC: " & !unicod & " en la ubicación de PTL " & iUbicacion, MENSAJE_Exclamacion)
                            await MessageService.Instance.WsMensaje(
                                $" Se ha ubicado el BAC: {datos.Unicod} en la ubicación de PTL {tUbicacion}", Constants.MENSAJE_Exclamacion);
                            tUbicacion = 0;  // Reiniciamos la ubicación
                            RefrescarDatos(true);
                        }
                    }
                    else
                    {
                        // VB6: 'Se queda pendiente de la lectura de la ubicación o de otro BAC
                        stBAC = stBACInput;
                    }
                }

                return true;
            }
            else
            {
                // VB6: 'No se ha encontrado el BAC. Se comprueba si existe la definición en GAUBIBAC
                // VB6: If ed.rsConsultaBACdePTL.State <> adStateClosed Then ed.rsConsultaBACdePTL.Close
                // VB6: ed.ConsultaBACdePTL stBAC
                var consultaBAC = await ed.ConsultaBACdePTL(stBACInput);

                // VB6: If ed.rsConsultaBACdePTL.RecordCount > 0 Then
                if (consultaBAC != null)
                {
                    // VB6: 'Se ha encontrado el BAC
                    // VB6: fValidarBAC = True
                    // VB6: RefrescarDatos False, 0, 0, 0, 0, 0, ed.rsConsultaBACdePTL!ubibac, 0, 0, 0, 0, 0, 0, 0, False, False
                    RefrescarDatos(false, 0, 0, 0, 0, 0, consultaBAC.Ubibac ?? "", 0, 
                        0, 0, 0, 0, "", "", false, false);

                    // VB6: If iUbicacion > 0 Then
                    if (tUbicacion > 0)
                    {
                        // VB6: 'Se ha leido la ubicación anteriormente. Se procede a ubicar el BAC
                        // VB6: If UbicarBAC(ed.rsConsultaBACdePTL!ubibac, iUbicacion, 0, oEstado(0).Value) Then
                        if (await UbicarBAC(tUbicacion, consultaBAC.Ubibac ?? "", 0))
                        {
                            // VB6: Call wsMensaje(" Se ha ubicado el BAC: " & !unicod & " en la ubicación de PTL " & iUbicacion, MENSAJE_Exclamacion)
                            await MessageService.Instance.WsMensaje(
                                $" Se ha ubicado el BAC: {consultaBAC.Ubibac} en la ubicación de PTL {tUbicacion}", Constants.MENSAJE_Exclamacion);
                            tUbicacion = 0;  // Reiniciamos la ubicación
                            RefrescarDatos(true);
                        }
                    }
                    else
                    {
                        // VB6: 'Se queda pendiente de la lectura de la ubicación o de otro BAC
                        stBAC = consultaBAC.Ubibac ?? stBACInput;
                    }

                    return true;
                }
                else
                {
                    // VB6: If blMensaje Then Call wsMensaje(" No existe el BAC ", vbCritical)
                    if (blMensaje)
                    {
                        await MessageService.Instance.WsMensaje(" No existe el BAC ", 16);
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// fValidarUbicacion - Valida y procesa ubicación
        /// Equivalente a fValidarUbicacion de VB6 - frmUbicarBAC.frm líneas 526-577
        /// </summary>
        private async Task<bool> fValidarUbicacion(string stUbicacionInput, bool blMensaje = true)
        {
            // VB6: fValidarUbicacion = False
            // VB6: iALF = 2
            // VB6: iALM = Val(Mid(stUbicacion, 1, 3))
            // VB6: iBLO = Val(Mid(stUbicacion, 4, 3))
            // VB6: iFIL = Val(Mid(stUbicacion, 7, 3))
            // VB6: iALT = Val(Mid(stUbicacion, 10, 3))
            int iALF = 2;  // VB6 hardcoded value
            int iALM = 0, iBLO = 0, iFIL = 0, iALT = 0;

            if (stUbicacionInput.Length >= 12)
            {
                int.TryParse(stUbicacionInput.Substring(0, 3), out iALM);
                int.TryParse(stUbicacionInput.Substring(3, 3), out iBLO);
                int.TryParse(stUbicacionInput.Substring(6, 3), out iFIL);
                int.TryParse(stUbicacionInput.Substring(9, 3), out iALT);
            }

            // VB6: If ed.rsDameDatosUbicacionPTL.State <> adStateClosed Then ed.rsDameDatosUbicacionPTL.Close
            // VB6: ed.DameDatosUbicacionPTL iALF, iALM, iBLO, iFIL, iALT
            var datos = await ed.DameDatosUbicacionPTL(iALF, iALM, iBLO, iFIL, iALT);

            // VB6: With ed.rsDameDatosUbicacionPTL
            // VB6:     'Existencia del registro
            // VB6:     If .RecordCount > 0 Then
            if (datos != null)
            {
                // VB6: fValidarUbicacion = True
                // VB6: iUbicacion = !ubicod
                tUbicacion = datos.Ubicod;

                // VB6: 'Si existe comprueba si tiene un BAC asociado
                // VB6: If IsNull(!unicod) Then
                if (string.IsNullOrEmpty(datos.Unicod))
                {
                    // VB6: lbUbicacion = "(" & !ubicod & ") " & CStr(Format(iALM, "000")) & "." & CStr(Format(iBLO, "000")) & "." & CStr(Format(iFIL, "000")) & "." & CStr(Format(iALT, "000"))
                    lbUbicacion.Text = $"({datos.Ubicod}) {iALM:D3}.{iBLO:D3}.{iFIL:D3}.{iALT:D3}";

                    // VB6: 'Si se ha leido el BAC anteriormente. Se procede a ubicar el BAC
                    // VB6: If lbBAC.Caption <> "" Then
                    if (!string.IsNullOrEmpty(lbBAC.Text))
                    {
                        // VB6: If UbicarBAC(lbBAC, iUbicacion, IIf(lbEstadoBAC = "ABIERTO", 0, 1), oEstado(0).Value) Then
                        int estadoBAC = lbEstadoBAC.Text == "ABIERTO" ? 0 : 1;
                        if (await UbicarBAC(tUbicacion, lbBAC.Text, estadoBAC))
                        {
                            // VB6: Call wsMensaje(" Se ha ubicado el BAC: " & !unicod & " en la ubicación de PTL " & iUbicacion, MENSAJE_Exclamacion)
                            await MessageService.Instance.WsMensaje(
                                $" Se ha ubicado el BAC: {lbBAC.Text} en la ubicación de PTL {tUbicacion}", Constants.MENSAJE_Exclamacion);
                            tUbicacion = 0;  // Reiniciamos la ubicación
                            RefrescarDatos(true);
                        }
                    }
                    else
                    {
                        // VB6: 'Se queda pendiente de la lectura del BAC o de otra ubicación
                    }
                }
                else
                {
                    // VB6: wsMensaje " La Ubicación ya tiene asociado un BAC ", vbCritical
                    await MessageService.Instance.WsMensaje(" La Ubicación ya tiene asociado un BAC ", 16);
                    tUbicacion = 0;
                }

                return true;
            }
            else
            {
                // VB6: If blMensaje Then Call wsMensaje(" No existe la Unidad de Transporte ", vbCritical)
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje(" No existe la Unidad de Transporte ", 16);
                }
                lbUbicacion.Text = "";
                tUbicacion = 0;
            }

            return false;
        }

        /// <summary>
        /// UbicarBAC - Ubica un BAC en una ubicación
        /// Equivalente a UbicarBAC de VB6 - frmUbicarBAC.frm líneas 579-625
        /// </summary>
        private async Task<bool> UbicarBAC(int ubicacion, string bac, int estadoBACActual = 0)
        {
            // VB6: UbicarBAC = False
            // VB6: 'Si el estado del BAC es cerrar se pasa a cerrado
            // VB6: If oEstado(0).Value = True Then tEstado = 1 Else tEstado = 0
            int tEstado = oEstadoCerrar.IsChecked ? 1 : 0;

            // VB6: 'Primero cambia el estado del BAC si es necesario
            // VB6: If tEstado <> iEstado Then
            if (tEstado != estadoBACActual)
            {
                // VB6: ed.CambiaEstadoBACdePTL bac, tEstado, Usuario.Id, Retorno, msgSalida
                var resultadoEstado = await ed.CambiaEstadoBACdePTL(bac, tEstado, AppSettings.Instance.Usuario.Id);
                // VB6: If Retorno <> 0 Then
                if (!resultadoEstado.Exitoso)
                {
                    // VB6: Call wsMensaje(" No se ha podido cambiar el estado del BAC. " & msgSalida, vbCritical)
                    await MessageService.Instance.WsMensaje(
                        $" No se ha podido cambiar el estado del BAC. {resultadoEstado.MsgSalida}", 16);
                    return false;
                }
            }

            // VB6: ed.UbicarBACenPTL bac, ubicacion, Usuario.Id, Retorno, msgSalida
            var resultado = await ed.UbicarBACenPTL(bac, ubicacion, AppSettings.Instance.Usuario.Id);

            // VB6: If Retorno = 0 Then
            if (resultado.Exitoso)
            {
                // VB6: UbicarBAC = True
                return true;
            }
            else
            {
                // VB6: Call wsMensaje(" No se ha podido ubicar el BAC. " & msgSalida, vbCritical)
                await MessageService.Instance.WsMensaje(
                    $" No se ha podido ubicar el BAC. {resultado.MsgSalida}", 16);
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
