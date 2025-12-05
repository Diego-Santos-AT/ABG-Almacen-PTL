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
        /// </summary>
        private async Task<bool> fValidarBAC(string stBACInput, bool blMensaje = true)
        {
            var datos = await ed.DameDatosBACdePTL(stBACInput);

            if (datos != null)
            {
                bool bCalculoPeso = datos.Unipes > datos.Unipma;
                bool bCalculoVolumen = datos.Univol > datos.Univma;

                RefrescarDatosBAC(false, stBACInput, datos.Uniest, datos.Unigru, datos.Unitab, 
                    datos.Unipes, datos.Univol, datos.Tipdes ?? "", bCalculoPeso, bCalculoVolumen);

                stBAC = stBACInput;
                tGrupo = datos.Unigru;
                tTablilla = datos.Unitab;
                tEstadoBAC = datos.Uniest;

                // Procesar según opciones
                if (chkEmpaquetado.IsChecked)
                {
                    await ProcesarEmpaquetado();
                }
                else
                {
                    // Mostrar acciones
                    MostrarAcciones();
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
