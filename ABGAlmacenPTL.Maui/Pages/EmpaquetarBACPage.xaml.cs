// ***********************************************************************
// Nombre: EmpaquetarBACPage.xaml.cs
// Formulario para el empaquetado de BACs en CAJAs de PTL
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     frmEmpaquetarBAC.frm de VB6 - línea por línea
//
// Realización:   A.Esteban (VB6) - 02/09/20
// ***********************************************************************

using ABGAlmacenPTL.Maui.Models;
using ABGAlmacenPTL.Maui.Services;
using System.Collections.ObjectModel;

namespace ABGAlmacenPTL.Maui.Pages
{
    public partial class EmpaquetarBACPage : ContentPage
    {
        // ----- Constantes de Módulo -------------
        private const string MOD_Nombre = "Empaquetar BAC";

        // ----- Variables generales -------------
        private DataEnvironment ed = new DataEnvironment();
        
        // Variables de estado (equivalentes a VB6)
        private string stBAC = string.Empty;
        private int tGrupo = 0;
        private int tTablilla = 0;
        private string stSSCC = string.Empty;
        private int tEstadoBAC = 0;
        private int tUbicacionBAC = 0;  // VB6: tUbicacionBAC

        // Lista de tipos de caja
        private List<TipoCaja> tiposCaja = new List<TipoCaja>();

        // Lista de contenido
        private ObservableCollection<ContenidoItem> contenidoItems = new ObservableCollection<ContenidoItem>();

        public EmpaquetarBACPage()
        {
            InitializeComponent();
            MessageService.Instance.SetCurrentPage(this);
            lstContenido.ItemsSource = contenidoItems;
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
                
                // Cargar tipos de caja
                await CargarTiposCaja();
                
                // Inicializar opciones desde configuración
                chkCerrarBAC.IsChecked = AppSettings.Instance.OpcionCerrarBAC;
                chkExtraerBAC.IsChecked = AppSettings.Instance.OpcionExtraerBAC;
                chkCrearCAJA.IsChecked = AppSettings.Instance.OpcionCrearCAJA;
                chkImprimirCAJA.IsChecked = AppSettings.Instance.OpcionImprimirCAJA;
                chkRelContenido.IsChecked = AppSettings.Instance.OpcionRelContenido;
                chkEmpaquetado.IsChecked = AppSettings.Instance.OpcionEmpaquetado;
                
                // Inicializar visualización
                InicializarVisualizacion();
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
            
            // Guardar opciones
            AppSettings.Instance.OpcionCerrarBAC = chkCerrarBAC.IsChecked;
            AppSettings.Instance.OpcionExtraerBAC = chkExtraerBAC.IsChecked;
            AppSettings.Instance.OpcionCrearCAJA = chkCrearCAJA.IsChecked;
            AppSettings.Instance.OpcionImprimirCAJA = chkImprimirCAJA.IsChecked;
            AppSettings.Instance.OpcionRelContenido = chkRelContenido.IsChecked;
            AppSettings.Instance.OpcionEmpaquetado = chkEmpaquetado.IsChecked;
            AppSettings.Instance.GuardarOpcionesEmpaquetado();
            
            ed.Close();
            ed.Dispose();
        }

        /// <summary>
        /// Inicializar visualización
        /// </summary>
        private void InicializarVisualizacion()
        {
            RefrescarDatosBAC(true);
            RefrescarDatosCAJA(true);
            fraOpciones.IsVisible = true;
            fraAcciones.IsVisible = false;
            fraCAJA.IsVisible = false;
            fraLista.IsVisible = false;
            fraTiposCaja.IsVisible = false;
            lbTexto.Text = "EMPAQUETAR BAC";
            Label8.Text = "Leer BAC de PTL para Empaquetar";
        }

        /// <summary>
        /// Cargar tipos de caja
        /// </summary>
        private async Task CargarTiposCaja()
        {
            cmbTiposCaja.Items.Clear();
            tiposCaja.Clear();

            var tipos = await ed.DameTiposCajasActivas();
            
            foreach (var tipo in tipos)
            {
                cmbTiposCaja.Items.Add(tipo.Tipdes ?? $"Tipo {tipo.Tipcod}");
                tiposCaja.Add(tipo);
            }

            if (cmbTiposCaja.Items.Count > 0)
            {
                cmbTiposCaja.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// cmdAccion_Click(990) - SALIR
        /// </summary>
        private async void CmdSalir_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        /// <summary>
        /// cmdVolver - Volver a opciones
        /// </summary>
        private void CmdVolver_Clicked(object sender, EventArgs e)
        {
            InicializarVisualizacion();
            stBAC = string.Empty;
            stSSCC = string.Empty;
            tGrupo = 0;
            tTablilla = 0;
            txtLecturaCodigo.Text = string.Empty;
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
        /// txtLecturaCodigo_KeyDown - Procesamiento de lectura
        /// </summary>
        private async void TxtLecturaCodigo_Completed(object sender, EventArgs e)
        {
            string codigo = txtLecturaCodigo.Text?.Trim() ?? string.Empty;

            switch (codigo.Length)
            {
                case 12:
                    // BAC
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
        /// Equivalente a fValidarBAC de VB6 - frmEmpaquetarBAC.frm líneas 1778-1858
        /// </summary>
        private async Task<bool> fValidarBAC(string stBACInput, bool blMensaje = true)
        {
            // VB6: fValidarBAC = False
            bool bCalculoPeso = false;
            bool bCalculoVolumen = false;

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
                tEstadoBAC = datos.Uniest;
                tUbicacionBAC = datos.Uninum;

                // VB6: 'Se muestran los datos
                if (datos.Ubicod == null || datos.Ubicod == 0)
                {
                    // VB6: RefrescarDatos False, 0, 0, 0, 0, 0, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, !uninca, bCalculoPeso, bCalculoVolumen
                    RefrescarDatosBAC(false, datos.Unicod!, datos.Uniest, datos.Unigru, datos.Unitab,
                        datos.Unipes, datos.Univol, datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);
                    lbUbicacion.Text = "SIN UBICACION";
                }
                else
                {
                    // VB6: RefrescarDatos False, !ubicod, !ubialm, !ubiblo, !ubifil, !ubialt, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, !uninca, bCalculoPeso, bCalculoVolumen
                    RefrescarDatosBAC(false, datos.Unicod!, datos.Uniest, datos.Unigru, datos.Unitab,
                        datos.Unipes, datos.Univol, datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);
                    lbUbicacion.Text = GeneralFunctions.FormatearUbicacion(datos.Ubicod.Value, datos.Ubialm, datos.Ubiblo, datos.Ubifil, datos.Ubialt);
                }

                stBAC = stBACInput;
                tGrupo = datos.Unigru;
                tTablilla = datos.Unitab;
                lbNumCaja.Text = datos.Uninca ?? "";

                // VB6: 'Lista de artículos contenidos en el BAC
                // VB6: Call sRefrescarArticulosBAC(!unigru, !unicod)
                await sRefrescarArticulosBAC(datos.Unigru, datos.Unicod!);

                // VB6: 'Comprueba el estado del BAC
                // VB6: If !uniest = 0 Then
                if (datos.Uniest == 0)
                {
                    // VB6: If Check1(OPC_CerrarBAC).Value = 1 Then
                    if (chkCerrarBAC.IsChecked)
                    {
                        // VB6: CerrarBAC
                        await CerrarBAC();
                    }
                    else
                    {
                        // VB6: If blMensaje Then Call wsMensaje(" El BAC está abierto!!", vbCritical)
                        if (blMensaje)
                        {
                            await MessageService.Instance.WsMensaje(" El BAC está abierto!!", 16);
                        }
                    }
                }

                // VB6: 'Comprueba si está ubicado el BAC
                // VB6: If !uninum > 0 Then
                if (datos.Uninum > 0)
                {
                    // VB6: If Check1(OPC_ExtraerBAC).Value = 1 Then
                    if (chkExtraerBAC.IsChecked)
                    {
                        // VB6: ExtraerBAC
                        await ExtraerBAC();
                    }
                    else
                    {
                        // VB6: If blMensaje Then Call wsMensaje(" El BAC está ubicado!!", vbCritical)
                        if (blMensaje)
                        {
                            await MessageService.Instance.WsMensaje(" El BAC está ubicado!!", 16);
                        }
                    }
                }

                // VB6: 'Acciones de la lectura
                // VB6: cmdAccion(CML_Acciones).Caption = ACC_Empaquetar
                lbTexto.Text = Constants.ACC_Empaquetar;

                // VB6: 'Obtener el SSCC de la caja si existe
                if (!string.IsNullOrEmpty(datos.Uninca) && int.TryParse(datos.Uninca, out int numCaja) && numCaja > 0)
                {
                    // VB6: ed.DameCajaGrupoTablillaPTL sGrupo, sTablilla, CStr(sNumCaja)
                    var datosCaja = await ed.DameCajaGrupoTablillaPTL(datos.Unigru, datos.Unitab, datos.Uninca);
                    if (datosCaja != null)
                    {
                        lbSSCC.Text = datosCaja.Ltcssc ?? "";
                        stSSCC = datosCaja.Ltcssc ?? "";
                    }
                    else
                    {
                        lbSSCC.Text = "ERROR EN LA CAJA";
                    }
                }
                else
                {
                    lbSSCC.Text = "";
                    stSSCC = "";
                }

                // Mostrar acciones
                MostrarAcciones();

                return true;
            }
            else
            {
                // VB6: 'Cuando no existe el bac se busca la última caja a la que se ha traspasado desde ese BAC
                // VB6: If ed.rsDameUltimaCajaDeBAC.State <> adStateClosed Then ed.rsDameUltimaCajaDeBAC.Close
                // VB6: ed.DameUltimaCajaDeBAC stBAC
                var ultimaCaja = await ed.DameUltimaCajaDeBAC(stBACInput);

                // VB6: With ed.rsDameUltimaCajaDeBAC
                // VB6:     If .RecordCount = 0 Then
                if (string.IsNullOrEmpty(ultimaCaja))
                {
                    // VB6: If blMensaje Then Call wsMensaje(" No existe el BAC ", vbCritical)
                    if (blMensaje)
                    {
                        await MessageService.Instance.WsMensaje(" No existe el BAC ", 16);
                    }
                }
                else
                {
                    // VB6: If MsgBox("¿Recuperar última caja de este BAC?", vbInformation + vbYesNo, "BAC vacío!!") = vbYes Then
                    bool respuesta = await DisplayAlert("BAC vacío!!", "¿Recuperar última caja de este BAC?", "Sí", "No");
                    if (respuesta)
                    {
                        // VB6: Label3.Caption = "CAJA"
                        // VB6: fValidarCaja !ltcssc, True
                        Label3.Text = "CAJA";
                        await fValidarCaja(ultimaCaja, true);
                        return true;
                    }
                }

                // VB6: 'Acciones de la lectura
                // VB6: cmdAccion(CML_Acciones).Caption = ACC_General
                lbTexto.Text = Constants.ACC_General;
            }

            return false;
        }

        /// <summary>
        /// Procesar empaquetado automático
        /// </summary>
        private async Task ProcesarEmpaquetado()
        {
            bool exito = true;
            string mensaje = "";

            // 1. Cerrar BAC si está seleccionado
            if (chkCerrarBAC.IsChecked && tEstadoBAC == 0)
            {
                var resultado = await ed.CambiaEstadoBACdePTL(stBAC, 1, AppSettings.Instance.Usuario.Id);
                if (!resultado.Exitoso)
                {
                    exito = false;
                    mensaje = $"Error al cerrar BAC: {resultado.MsgSalida}";
                }
            }

            // 2. Crear CAJA si está seleccionado
            if (exito && chkCrearCAJA.IsChecked)
            {
                var (resultado, sscc) = await ed.TraspasaBACaCAJAdePTL(stBAC, AppSettings.Instance.Usuario.Id, "");
                if (resultado.Exitoso)
                {
                    stSSCC = sscc;
                    RefrescarDatosCAJA(false, sscc, 0, 0, "");
                    fraCAJA.IsVisible = true;
                }
                else
                {
                    exito = false;
                    mensaje = $"Error al crear CAJA: {resultado.MsgSalida}";
                }
            }

            // 3. Extraer BAC si está seleccionado
            if (exito && chkExtraerBAC.IsChecked)
            {
                var resultado = await ed.RetirarBACdePTL(stBAC, AppSettings.Instance.Usuario.Id);
                if (!resultado.Exitoso)
                {
                    exito = false;
                    mensaje = $"Error al extraer BAC: {resultado.MsgSalida}";
                }
            }

            if (exito)
            {
                await MessageService.Instance.WsMensaje(
                    $"Empaquetado completado. SSCC: {stSSCC}", Constants.MENSAJE_Exclamacion);
                
                // Reiniciar para nuevo BAC
                InicializarVisualizacion();
                stBAC = string.Empty;
                stSSCC = string.Empty;
            }
            else
            {
                await MessageService.Instance.WsMensaje(mensaje, 16);
            }
        }

        /// <summary>
        /// Mostrar panel de acciones
        /// </summary>
        private void MostrarAcciones()
        {
            fraOpciones.IsVisible = false;
            fraAcciones.IsVisible = true;
            lbTexto.Text = "ACCIONES";
        }

        /// <summary>
        /// CerrarBAC - Cierra el BAC
        /// Equivalente a CerrarBAC de VB6 - frmEmpaquetarBAC.frm líneas 2146-2161
        /// </summary>
        private async Task CerrarBAC()
        {
            // VB6: If tEstadoBAC = 1 Then 'El BAC ya está cerrado - Exit Sub
            if (tEstadoBAC == 1)
            {
                return;
            }

            // VB6: 'Cambiar estado de BAC de 0 a 1
            // VB6: If CambiarEstadoBAC(lbBAC.Caption, 1) Then
            var resultado = await ed.CambiaEstadoBACdePTL(stBAC, 1, AppSettings.Instance.Usuario.Id);
            if (resultado.Exitoso)
            {
                tEstadoBAC = 1;
                // VB6: lbBAC.BackColor = IIf(tEstadoBAC = 0, vbWhite, ColorVerde)
                lbEstadoBAC.Text = "CERRADO";
                lbEstadoBAC.BackgroundColor = Constants.ColorVerde;
            }
        }

        /// <summary>
        /// ExtraerBAC - Extrae el BAC de la ubicación
        /// Equivalente a ExtraerBAC de VB6 - frmEmpaquetarBAC.frm líneas 2181-2220
        /// </summary>
        private async Task ExtraerBAC()
        {
            // VB6: If tUbicacionBAC = 0 Then 'El BAC ya está extraido - Exit Sub
            if (tUbicacionBAC == 0)
            {
                return;
            }

            // VB6: If RetirarBAC(lbBAC.Caption, tEstadoBAC, True) Then
            var resultado = await ed.RetirarBACdePTL(stBAC, AppSettings.Instance.Usuario.Id);
            if (resultado.Exitoso)
            {
                tUbicacionBAC = 0;
                lbUbicacion.Text = "-------------";

                // VB6: 'Cambiar estado de BAC si es necesario
                if (tEstadoBAC == 0)
                {
                    // Cambiar a cerrado
                    var resultadoEstado = await ed.CambiaEstadoBACdePTL(stBAC, 1, AppSettings.Instance.Usuario.Id);
                    if (resultadoEstado.Exitoso)
                    {
                        tEstadoBAC = 1;
                        lbEstadoBAC.Text = "CERRADO";
                        lbEstadoBAC.BackgroundColor = Constants.ColorVerde;
                    }
                }
            }
        }

        /// <summary>
        /// sRefrescarArticulosBAC - Refresca la lista de artículos del BAC
        /// Equivalente a sRefrescarArticulosBAC de VB6 - frmEmpaquetarBAC.frm líneas 1995-2029
        /// </summary>
        private async Task sRefrescarArticulosBAC(int sGrupo, string sBAC)
        {
            contenidoItems.Clear();
            int iUds = 0;

            // VB6: If ed.rsDameContenidoBacGrupo.State <> adStateClosed Then ed.rsDameContenidoBacGrupo.Close
            // VB6: ed.DameContenidoBacGrupo sGrupo, sBAC
            var contenido = await ed.DameContenidoBacGrupo(sGrupo, sBAC);

            // VB6: With ed.rsDameContenidoBacGrupo
            // VB6:     If .RecordCount > 0 Then
            foreach (var item in contenido)
            {
                contenidoItems.Add(new ContenidoItem
                {
                    Articulo = item.Uniart.ToString(),
                    Nombre = item.Artnom ?? "",
                    Cantidad = item.Unican.ToString()
                });
                iUds += item.Unican;
            }

            // VB6: lbUds = iUds
            // VB6: lbArts = r_Art.RecordCount
            lbUds.Text = iUds.ToString();
            lbArts.Text = contenidoItems.Count.ToString();
        }

        /// <summary>
        /// sRefrescarArticulosCAJA - Refresca la lista de artículos de la CAJA
        /// Equivalente a sRefrescarArticulosCAJA de VB6 - frmEmpaquetarBAC.frm líneas 2031-2065
        /// </summary>
        private async Task sRefrescarArticulosCAJA(int sGrupo, int sTablilla, string sCaja)
        {
            contenidoItems.Clear();
            int iUds = 0;

            // VB6: If ed.rsDameContenidoCajaGrupo.State <> adStateClosed Then ed.rsDameContenidoCajaGrupo.Close
            // VB6: ed.DameContenidoCajaGrupo sGrupo, sTablilla, sCaja
            var contenido = await ed.DameContenidoCajaGrupo(sGrupo, sTablilla, sCaja);

            // VB6: With ed.rsDameContenidoCajaGrupo
            // VB6:     If .RecordCount > 0 Then
            foreach (var item in contenido)
            {
                contenidoItems.Add(new ContenidoItem
                {
                    Articulo = item.Ltcart.ToString(),
                    Nombre = item.Artnom ?? "",
                    Cantidad = item.Ltccan.ToString()
                });
                iUds += Convert.ToInt32(Math.Round(item.Ltccan));
            }

            // VB6: lbUds = iUds
            // VB6: lbArts = r_ArtC.RecordCount
            lbUds.Text = iUds.ToString();
            lbArts.Text = contenidoItems.Count.ToString();
        }

        /// <summary>
        /// fValidarCaja - Valida y carga datos de una CAJA
        /// Equivalente a fValidarCaja de VB6 - frmEmpaquetarBAC.frm líneas 1861-1900
        /// </summary>
        private async Task<bool> fValidarCaja(string stSSCCInput, bool blMensaje = true)
        {
            // VB6: fValidarCaja = False
            bool bCalculoPeso = false;
            bool bCalculoVolumen = false;
            int bEstado = 0;

            // VB6: If ed.rsDameDatosCAJAdePTL.State <> adStateClosed Then ed.rsDameDatosCAJAdePTL.Close
            // VB6: ed.DameDatosCAJAdePTL stSSCC
            var datos = await ed.DameDatosCAJAdePTL(stSSCCInput);

            // VB6: With ed.rsDameDatosCAJAdePTL
            // VB6:     'Existencia del registro
            // VB6:     If .RecordCount > 0 Then
            if (datos != null)
            {
                // VB6: fValidarCaja = True
                // VB6: bEstado = IIf(!ltcvol > 0, 1, 0)
                bEstado = datos.Ltcvol > 0 ? 1 : 0;

                tGrupo = datos.Ltcgru;
                tTablilla = datos.Ltctab;
                stSSCC = stSSCCInput;

                // VB6: 'Se muestran los datos
                // VB6: RefrescarDatos False, 0, 0, 0, 0, 0, !ltcssc, bEstado, !ltcgru, !ltctab, !ltcpes, !ltcvol, !ltctip, !tipdes, !ltccaj, bCalculoPeso, bCalculoVolumen
                RefrescarDatosBAC(false, stSSCCInput, bEstado, datos.Ltcgru, datos.Ltctab,
                    datos.Ltcpes, datos.Ltcvol, datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);

                lbSSCC.Text = stSSCCInput;
                lbNumCaja.Text = datos.Ltccaj ?? "";

                // VB6: 'Lista de artículos contenidos en la CAJA
                // VB6: Call sRefrescarArticulosCAJA(!ltcgru, !ltctab, !ltccaj)
                await sRefrescarArticulosCAJA(datos.Ltcgru, datos.Ltctab, datos.Ltccaj ?? "");

                // VB6: 'Acciones de la lectura
                // VB6: cmdAccion(CML_Acciones).Caption = ACC_Etiquetas
                lbTexto.Text = Constants.ACC_Etiquetas;

                MostrarAcciones();

                return true;
            }
            else
            {
                // VB6: If blMensaje Then Call wsMensaje(" No existe la CAJA ", vbCritical)
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje(" No existe la CAJA ", 16);
                }

                // VB6: 'Acciones de la lectura
                // VB6: cmdAccion(CML_Acciones).Caption = ACC_General
                lbTexto.Text = Constants.ACC_General;
            }

            return false;
        }

        // ==================== Botones de acción ====================

        private async void CmdCerrarBAC_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stBAC))
            {
                await MessageService.Instance.WsMensaje("Primero debe leer un BAC", 16);
                return;
            }

            var resultado = await ed.CambiaEstadoBACdePTL(stBAC, 1, AppSettings.Instance.Usuario.Id);
            if (resultado.Exitoso)
            {
                await MessageService.Instance.WsMensaje("BAC cerrado correctamente", Constants.MENSAJE_Exclamacion);
                tEstadoBAC = 1;
                lbEstadoBAC.Text = "CERRADO";
                lbEstadoBAC.BackgroundColor = Constants.ColorVerde;
            }
            else
            {
                await MessageService.Instance.WsMensaje($"Error: {resultado.MsgSalida}", 16);
            }
        }

        private async void CmdExtraerBAC_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stBAC))
            {
                await MessageService.Instance.WsMensaje("Primero debe leer un BAC", 16);
                return;
            }

            var resultado = await ed.RetirarBACdePTL(stBAC, AppSettings.Instance.Usuario.Id);
            if (resultado.Exitoso)
            {
                await MessageService.Instance.WsMensaje("BAC extraído correctamente", Constants.MENSAJE_Exclamacion);
            }
            else
            {
                await MessageService.Instance.WsMensaje($"Error: {resultado.MsgSalida}", 16);
            }
        }

        private async void CmdCrearCAJA_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stBAC))
            {
                await MessageService.Instance.WsMensaje("Primero debe leer un BAC", 16);
                return;
            }

            var (resultado, sscc) = await ed.TraspasaBACaCAJAdePTL(stBAC, AppSettings.Instance.Usuario.Id, "");
            if (resultado.Exitoso)
            {
                stSSCC = sscc;
                await MessageService.Instance.WsMensaje($"CAJA creada. SSCC: {sscc}", Constants.MENSAJE_Exclamacion);
                RefrescarDatosCAJA(false, sscc, 0, 0, "");
                fraCAJA.IsVisible = true;
            }
            else
            {
                await MessageService.Instance.WsMensaje($"Error: {resultado.MsgSalida}", 16);
            }
        }

        private async void CmdImprimirCAJA_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stSSCC))
            {
                await MessageService.Instance.WsMensaje("No hay CAJA para imprimir", 16);
                return;
            }

            // Aquí iría la lógica de impresión
            await MessageService.Instance.WsMensaje($"Imprimiendo CAJA: {stSSCC}", Constants.MENSAJE_Exclamacion);
        }

        private async void CmdRelContenido_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(stBAC))
            {
                await MessageService.Instance.WsMensaje("Primero debe leer un BAC", 16);
                return;
            }

            // Cargar contenido del BAC
            var contenido = await ed.DameContenidoBacGrupo(tGrupo, stBAC);
            contenidoItems.Clear();
            
            foreach (var item in contenido)
            {
                contenidoItems.Add(new ContenidoItem
                {
                    Articulo = item.Uniart.ToString(),
                    Nombre = item.Artnom ?? "",
                    Cantidad = item.Unican.ToString()
                });
            }

            fraLista.IsVisible = true;
            lbTituloLista.Text = "CONTENIDO BAC";
        }

        private async void CmdEmpaquetado_Clicked(object sender, EventArgs e)
        {
            await ProcesarEmpaquetado();
        }

        private void CmdCambiarCAJA_Clicked(object sender, EventArgs e)
        {
            fraTiposCaja.IsVisible = true;
        }

        private async void CmdCambiarUDS_Clicked(object sender, EventArgs e)
        {
            await MessageService.Instance.WsMensaje("Función no implementada", 16);
        }

        private async void CmdCombinarCAJAS_Clicked(object sender, EventArgs e)
        {
            await MessageService.Instance.WsMensaje("Función no implementada", 16);
        }

        private async void CmdAplicarTipoCaja_Clicked(object sender, EventArgs e)
        {
            if (cmbTiposCaja.SelectedIndex < 0 || cmbTiposCaja.SelectedIndex >= tiposCaja.Count)
            {
                await MessageService.Instance.WsMensaje("Seleccione un tipo de caja", 16);
                return;
            }

            if (string.IsNullOrEmpty(stSSCC))
            {
                await MessageService.Instance.WsMensaje("No hay CAJA para cambiar tipo", 16);
                return;
            }

            var tipoSeleccionado = tiposCaja[cmbTiposCaja.SelectedIndex];
            
            await ed.CambiaTipoCajaPTL(tipoSeleccionado.Tipcod, stBAC, stSSCC, AppSettings.Instance.Usuario.Id);
            
            await MessageService.Instance.WsMensaje("Tipo de caja cambiado", Constants.MENSAJE_Exclamacion);
            fraTiposCaja.IsVisible = false;
        }

        private void CmbTiposCaja_SelectedIndexChanged(object sender, EventArgs e)
        {
            // No se requiere acción adicional
        }

        private void LstContenido_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Manejo de selección de contenido
        }

        /// <summary>
        /// RefrescarDatosBAC
        /// </summary>
        private void RefrescarDatosBAC(bool sEnBlanco, string sBAC = "", int sEstadoBAC = 0,
            int sGrupo = 0, int sTablilla = 0, double sPeso = 0, double sVolumen = 0,
            string sTipoCaja = "", bool bPeso = false, bool bVolumen = false)
        {
            if (sEnBlanco)
            {
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
            }
            else
            {
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
            }
        }

        /// <summary>
        /// RefrescarDatosCAJA
        /// </summary>
        private void RefrescarDatosCAJA(bool sEnBlanco, string sSSCC = "",
            double sPeso = 0, double sVolumen = 0, string sTipo = "")
        {
            if (sEnBlanco)
            {
                lbSSCC.Text = "";
                lbPesoCAJA.Text = "";
                lbVolumenCAJA.Text = "";
                lbTipoCAJA.Text = "";
            }
            else
            {
                lbSSCC.Text = sSSCC;
                lbPesoCAJA.Text = sPeso.ToString("#0.000");
                lbVolumenCAJA.Text = sVolumen.ToString("#0.000");
                lbTipoCAJA.Text = sTipo;
            }
        }
    }

    /// <summary>
    /// Clase para items de contenido en la lista
    /// </summary>
    public class ContenidoItem
    {
        public string Articulo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Cantidad { get; set; } = string.Empty;
    }
}
