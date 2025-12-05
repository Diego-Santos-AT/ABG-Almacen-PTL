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
        /// Equivalente a txtLecturaCodigo_KeyDown de VB6 - frmExtraerBAC.frm líneas 413-438
        /// </summary>
        private async void TxtLecturaCodigo_Completed(object sender, EventArgs e)
        {
            // VB6: If KeyCode = vbKeyReturn Then
            string codigo = txtLecturaCodigo.Text?.Trim() ?? string.Empty;

            // VB6: 'Inicializa la visualización
            // VB6: RefrescarDatos True
            RefrescarDatos(true);

            // VB6: Select Case Len(txtLecturaCodigo.Text)
            switch (codigo.Length)
            {
                case 12:
                    // VB6: Case 12: 'Unidad de transporte / Ubicación --------------------------
                    // VB6: 'Comprobar si la lectura es un BAC
                    // VB6: If fValidarBAC(txtLecturaCodigo.Text, True) = False Then
                    if (await fValidarBAC(codigo, true) == false)
                    {
                        // VB6: 'Comprobar si la lectura es una ubicación
                        // VB6: If fValidarUbicacion(txtLecturaCodigo.Text, True) = False Then
                        if (await fValidarUbicacion(codigo, true) == false)
                        {
                            // VB6: 'No existe la ubicación / BAC
                            // VB6: Call wsMensaje(" No se ha encontrado Ubicación o BAC", vbCritical)
                            await MessageService.Instance.WsMensaje("No se ha encontrado Ubicación o BAC", 16);
                        }
                    }
                    break;

                default:
                    await MessageService.Instance.WsMensaje("Lectura no válida. Longitud debe ser 12", 16);
                    break;
            }

            // VB6: txtLecturaCodigo.Text = ""
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// fValidarUbicacion - Valida y procesa ubicación
        /// Equivalente a fValidarUbicacion de VB6 - frmExtraerBAC.frm líneas 497-538
        /// </summary>
        private async Task<bool> fValidarUbicacion(string stUbicacion, bool blMensaje = true)
        {
            // Validación de longitud mínima para evitar excepciones
            if (string.IsNullOrEmpty(stUbicacion) || stUbicacion.Length < 12)
            {
                return false;
            }

            // VB6: iALF = 2
            // VB6: iALM = Val(Mid(stUbicacion, 1, 3))
            // VB6: iBLO = Val(Mid(stUbicacion, 4, 3))
            // VB6: iFIL = Val(Mid(stUbicacion, 7, 3))
            // VB6: iALT = Val(Mid(stUbicacion, 10, 3))
            int iALF = 2;
            int iALM = int.TryParse(stUbicacion.Substring(0, 3), out int alm) ? alm : 0;
            int iBLO = int.TryParse(stUbicacion.Substring(3, 3), out int blo) ? blo : 0;
            int iFIL = int.TryParse(stUbicacion.Substring(6, 3), out int fil) ? fil : 0;
            int iALT = int.TryParse(stUbicacion.Substring(9, 3), out int alt) ? alt : 0;

            // VB6: ed.DameDatosUbicacionPTL iALF, iALM, iBLO, iFIL, iALT
            var datos = await ed.DameDatosUbicacionPTL(iALF, iALM, iBLO, iFIL, iALT);

            // VB6: If .RecordCount > 0 Then
            if (datos != null)
            {
                // VB6: fValidarUbicacion = True
                // VB6: 'Si existe que tenga BAC asociado
                // VB6: If IsNull(!unicod) Then
                if (string.IsNullOrEmpty(datos.Unicod))
                {
                    // VB6: If blMensaje Then Call wsMensaje(" La Ubicación no tiene asociada un BAC ", vbCritical)
                    if (blMensaje)
                    {
                        await MessageService.Instance.WsMensaje("La Ubicación no tiene asociada un BAC", 16);
                    }
                    // VB6: lbUbicacion = "(" & !ubicod & ") " & Format(iALM, "000") & "." & ...
                    lbUbicacion.Text = $"({datos.Ubicod}) {iALM:D3}.{iBLO:D3}.{iFIL:D3}.{iALT:D3}";
                }
                else
                {
                    // VB6: If fValidarBAC(!unicod, False) = False Then
                    if (await fValidarBAC(datos.Unicod, false) == false)
                    {
                        // VB6: If blMensaje Then Call wsMensaje(" La Ubicación no tiene asociada un BAC válido ", vbCritical)
                        if (blMensaje)
                        {
                            await MessageService.Instance.WsMensaje("La Ubicación no tiene asociada un BAC válido", 16);
                        }
                    }
                }
                return true;
            }
            else
            {
                // VB6: If blMensaje Then Call wsMensaje(" No existe la Unidad de Transporte ", vbCritical)
                // VB6: lbUbicacion = ""
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe la Unidad de Transporte", 16);
                }
                lbUbicacion.Text = "";
                return false;
            }
        }

        /// <summary>
        /// fValidarBAC - Valida y carga datos del BAC
        /// Equivalente a fValidarBAC de VB6 - frmExtraerBAC.frm líneas 440-495
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
                // VB6: If IsNull(!ubicod) Then
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

                // VB6: 'Extracción del BAC
                // VB6: If !uninum > 0 Then
                if (datos.Uninum > 0)
                {
                    // VB6: If RetirarBAC(!unicod, !uniest, oEstado(0).Value) Then
                    //      Call wsMensaje(" Se ha extraído el BAC: " & !unicod & " de su ubicación de PTL ", MENSAJE_Exclamacion)
                    if (await RetirarBAC(datos.Unicod!, datos.Uniest, oEstadoCerrar.IsChecked))
                    {
                        await MessageService.Instance.WsMensaje(
                            $"Se ha extraído el BAC: {datos.Unicod} de su ubicación de PTL", 
                            Constants.MENSAJE_Exclamacion);
                        RefrescarDatos(true);
                        stBAC = string.Empty;
                    }
                }
                else
                {
                    // VB6: If (!uniest = 0) = oEstado(0).Value Then
                    //      'Cambiar estado de BAC
                    if ((datos.Uniest == 0) == oEstadoCerrar.IsChecked)
                    {
                        // VB6: If oEstado(0).Value Then tEstado = 1 Else tEstado = 0
                        int tEstado = oEstadoCerrar.IsChecked ? 1 : 0;
                        
                        // VB6: If CambiarEstadoBAC(!unicod, tEstado) Then
                        if (await CambiarEstadoBAC(datos.Unicod!, tEstado))
                        {
                            // VB6: lbEstadoBAC = IIf(tEstado = 0, "ABIERTO", "CERRADO")
                            lbEstadoBAC.Text = tEstado == 0 ? "ABIERTO" : "CERRADO";
                            // VB6: If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde
                            lbEstadoBAC.BackgroundColor = tEstado == 1 ? Constants.ColorVerde : Colors.White;
                        }
                    }
                    else
                    {
                        // VB6: Call wsMensaje(" El BAC ya está fuera de las ubicaciones de PTL ", vbCritical)
                        await MessageService.Instance.WsMensaje(
                            "El BAC ya está fuera de las ubicaciones de PTL", 16);
                    }
                }

                return true;
            }
            else
            {
                // VB6: 'No se ha encontrado el BAC
                // VB6: Exit Function (no muestra mensaje aquí, lo hace el caller)
                return false;
            }
        }

        /// <summary>
        /// RetirarBAC - Extrae un BAC de su ubicación
        /// Equivalente a RetirarBAC de VB6 - frmExtraerBAC.frm líneas 592-617
        /// </summary>
        private async Task<bool> RetirarBAC(string tBac, int tEstado, bool tEstadoFinal)
        {
            // VB6: RetirarBAC = False
            // VB6: ed.RetirarBACdePTL tBac, Usuario.Id, Retorno, msgSalida
            var resultado = await ed.RetirarBACdePTL(tBac, AppSettings.Instance.Usuario.Id);

            // VB6: If Retorno = 0 Then
            if (resultado.Exitoso)
            {
                // VB6: RetirarBAC = True
                // VB6: If (tEstado = 0) = tEstadoFinal Then
                if ((tEstado == 0) == tEstadoFinal)
                {
                    // VB6: If tEstadoFinal Then nEstado = 1 Else nEstado = 0
                    int nEstado = tEstadoFinal ? 1 : 0;
                    
                    // VB6: 'Cambiar estado de BAC
                    // VB6: If CambiarEstadoBAC(tBac, nEstado) Then
                    if (await CambiarEstadoBAC(tBac, nEstado))
                    {
                        // VB6: lbEstadoBAC = IIf(tEstado = 0, "ABIERTO", "CERRADO")
                        lbEstadoBAC.Text = nEstado == 0 ? "ABIERTO" : "CERRADO";
                        // VB6: If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde
                        lbEstadoBAC.BackgroundColor = nEstado == 1 ? Constants.ColorVerde : Colors.White;
                    }
                }
                return true;
            }
            else
            {
                // VB6: Call wsMensaje(" No se ha podido retirar el BAC de la estanteria de PTL. " & msgSalida, vbCritical)
                await MessageService.Instance.WsMensaje(
                    $"No se ha podido retirar el BAC de la estantería de PTL. {resultado.MsgSalida}", 16);
            }

            return false;
        }

        /// <summary>
        /// CambiarEstadoBAC - Cambia el estado de un BAC
        /// Equivalente a CambiarEstadoBAC de VB6 - frmExtraerBAC.frm líneas 619-634
        /// </summary>
        private async Task<bool> CambiarEstadoBAC(string tBac, int tEstado)
        {
            // VB6: CambiarEstadoBAC = False
            // VB6: ed.CambiaEstadoBACdePTL tBac, tEstado, Usuario.Id, Retorno, msgSalida
            var resultado = await ed.CambiaEstadoBACdePTL(tBac, tEstado, AppSettings.Instance.Usuario.Id);

            // VB6: If Retorno = 0 Then
            if (resultado.Exitoso)
            {
                return true;
            }
            else
            {
                // VB6: Call wsMensaje(" No se ha podido cambiar el estado al BAC " & msgSalida, vbCritical)
                await MessageService.Instance.WsMensaje(
                    $"No se ha podido cambiar el estado al BAC {resultado.MsgSalida}", 16);
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
