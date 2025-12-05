// ***********************************************************************
// Nombre: RepartirArticuloPage.xaml.cs
// Formulario para el reparto de Artículos en BACs casilleros de PTL
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     frmRepartirArticulo.frm de VB6 - línea por línea
//
// Realización:   A.Esteban (VB6) - 02/09/20
// ***********************************************************************

using ABGAlmacenPTL.Maui.Models;
using ABGAlmacenPTL.Maui.Services;

namespace ABGAlmacenPTL.Maui.Pages
{
    public partial class RepartirArticuloPage : ContentPage
    {
        // ----- Constantes de Módulo -------------
        private const string MOD_Nombre = "Repartir Articulo";
        private const int CML_Salir = 990;

        // ----- Variables generales -------------
        private DataEnvironment ed = new DataEnvironment();
        private int tUsuario = 0;
        private bool bInicio = true;

        // Lista de puestos de trabajo para el Picker
        private List<PuestoTrabajoPTL> puestos = new List<PuestoTrabajoPTL>();

        public RepartirArticuloPage()
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
            bInicio = true;

            // VB6: MousePointer = ccHourglass
            try
            {
                // VB6: Me.Caption = MOD_Nombre
                Title = MOD_Nombre;

                // VB6: If ed.GestionAlmacen.State <> adStateClosed Then ed.GestionAlmacen.Close
                // VB6: ed.GestionAlmacen.Open ConexionGestionAlmacen
                await ed.OpenAsync();

                // VB6: CargarPistolas
                await CargarPistolas();
                
                tUsuario = 0;
            }
            catch (Exception ex)
            {
                await MessageService.Instance.MsgBoxError($"Error al inicializar: {ex.Message}");
            }

            bInicio = false;
            
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
        /// Form_KeyDown - Manejo de teclas especiales
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            // VB6: Case vbKeyEscape: Unload Me
            return base.OnBackButtonPressed();
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
        /// CargarPistolas - Carga el combo de puestos de trabajo
        /// Equivalente a CargarPistolas de VB6
        /// </summary>
        private async Task CargarPistolas()
        {
            // VB6: 'Combo de puestos de trabajo
            // VB6: Combo1.Clear

            Combo1.Items.Clear();
            puestos.Clear();

            // VB6: If ed.rsDamePuestosTrabajoPTL.State <> adStateClosed Then ed.rsDamePuestosTrabajoPTL.Close
            // VB6: ed.DamePuestosTrabajoPTL
            var puestosDB = await ed.DamePuestosTrabajoPTL();

            // VB6: Combo1.AddItem "(0) Sin puesto"
            // VB6: Combo1.ItemData(Combo1.NewIndex) = 0
            Combo1.Items.Add("(0) Sin puesto");
            puestos.Add(new PuestoTrabajoPTL { Puecod = 0, Puedes = "Sin puesto", Puecol = 0 });

            // VB6: With ed.rsDamePuestosTrabajoPTL
            // VB6:     If .RecordCount > 0 Then
            foreach (var puesto in puestosDB)
            {
                // VB6: Combo1.AddItem "(" & CStr(!puecod) & ") " & !puedes
                // VB6: Combo1.ItemData(Combo1.NewIndex) = !puecol
                Combo1.Items.Add($"({puesto.Puecod}) {puesto.Puedes}");
                puestos.Add(puesto);
            }

            // VB6: Combo1.Text = Combo1.List(0)
            Combo1.SelectedIndex = 0;
        }

        /// <summary>
        /// Combo1_Click - Selección de puesto de trabajo
        /// Equivalente a Combo1_Click de VB6
        /// </summary>
        private void Combo1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Combo1.SelectedIndex < 0 || Combo1.SelectedIndex >= puestos.Count)
                return;

            var puestoSeleccionado = puestos[Combo1.SelectedIndex];
            int colorCode = puestoSeleccionado.Puecol;

            // VB6: Select Case Combo1.ItemData(Combo1.ListIndex)
            // VB6:     Case 1: 'Blanco
            // VB6:         pColor.BackColor = vbWhite
            // VB6:         tUsuario = CInt(Mid(Combo1.Text, 2, 3))
            switch (colorCode)
            {
                case 1: // Blanco
                    pColor.Color = Colors.White;
                    tUsuario = puestoSeleccionado.Puecod;
                    break;
                case 2: // Amarillo
                    pColor.Color = Colors.Yellow;
                    tUsuario = puestoSeleccionado.Puecod;
                    break;
                case 3: // Magenta
                    pColor.Color = Colors.Magenta;
                    tUsuario = puestoSeleccionado.Puecod;
                    break;
                case 4: // Cian
                    pColor.Color = Colors.Cyan;
                    tUsuario = puestoSeleccionado.Puecod;
                    break;
                case 5: // Azul
                    pColor.Color = Colors.Blue;
                    tUsuario = puestoSeleccionado.Puecod;
                    break;
                case 6: // Verde
                    pColor.Color = Colors.Green;
                    tUsuario = puestoSeleccionado.Puecod;
                    break;
                case 7: // Rojo
                    pColor.Color = Colors.Red;
                    tUsuario = puestoSeleccionado.Puecod;
                    break;
                default: // Sin Color
                    pColor.Color = Color.FromRgb(128, 128, 128);
                    tUsuario = 0;
                    break;
            }

            // VB6: If Not bInicio Then txtLecturaCodigo.SetFocus
            if (!bInicio)
            {
                txtLecturaCodigo.Focus();
            }
        }

        /// <summary>
        /// txtLecturaCodigo_KeyDown - Procesamiento de lectura
        /// Equivalente a txtLecturaCodigo_KeyDown de VB6
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
                case 13:
                    // VB6: Case 13: ' EAN13 --------------------------
                    // VB6: 'Comprobar si la lectura es un EAN13
                    // VB6: If fValidarEAN13(txtLecturaCodigo.Text, True) = False Then
                    if (await fValidarEAN13(codigo, true) == false)
                    {
                        // VB6: 'No existe el Artículo
                        // VB6: Call wsMensaje(" No se ha encontrado el Artículo", vbCritical)
                        await MessageService.Instance.WsMensaje("No se ha encontrado el Artículo", 16);
                    }
                    break;

                case 4:
                case 5:
                    // VB6: Case 4, 5:  'Código de artículo
                    // VB6: 'Comprobar si la lectura es un Artículo
                    // VB6: If fValidarArticulo(txtLecturaCodigo.Text, True) = False Then
                    if (await fValidarArticulo(codigo, true) == false)
                    {
                        // VB6: 'No existe el Artículo
                        // VB6: Call wsMensaje(" No se ha encontrado el Artículo", vbCritical)
                        await MessageService.Instance.WsMensaje("No se ha encontrado el Artículo", 16);
                    }
                    break;

                default:
                    await MessageService.Instance.WsMensaje("Longitud de código no válida", 16);
                    break;
            }

            // VB6: txtLecturaCodigo.Text = ""
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        /// <summary>
        /// fValidarArticulo - Valida un artículo por código
        /// Equivalente a fValidarArticulo de VB6
        /// </summary>
        private async Task<bool> fValidarArticulo(string stArticulo, bool blMensaje = true)
        {
            // VB6: nArticulo = CLng(stArticulo)
            if (!int.TryParse(stArticulo, out int nArticulo))
                return false;

            // VB6: fValidarArticulo = False
            // VB6: If ed.rsDameArticuloConsulta.State <> adStateClosed Then ed.rsDameArticuloConsulta.Close
            // VB6: ed.DameArticuloConsulta stArticulo
            var articulo = await ed.DameArticuloConsulta(nArticulo);

            // VB6: If .RecordCount > 0 Then
            if (articulo != null)
            {
                // VB6: fValidarArticulo = True
                // VB6: 'Se muestran los datos
                // VB6: RefrescarDatos False, !artcod, !artnom, !artean, !artcj3, !artpea, !artcua
                RefrescarDatos(false, articulo.Artcod, articulo.Artnom ?? "", 
                    articulo.Artean ?? "", articulo.Artcj3, articulo.Artpea, articulo.Artcua);

                // VB6: 'Se procede a repartir el artículo
                // VB6: If RepartirArticulo(!artcod) Then
                if (await RepartirArticulo(articulo.Artcod))
                {
                    // VB6: Call wsMensaje(" Se ha reservado el BAC para el Artículo: " & !artcod, MENSAJE_Exclamacion)
                    await MessageService.Instance.WsMensaje(
                        $"Se ha reservado el BAC para el Artículo: {articulo.Artcod}", 
                        Constants.MENSAJE_Exclamacion);
                }

                return true;
            }
            else
            {
                // VB6: 'No se ha encontrado el Artículo
                // VB6: If blMensaje Then Call wsMensaje(" No existe el Artículo ", vbCritical)
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe el Artículo", 16);
                }
            }

            return false;
        }

        /// <summary>
        /// fValidarEAN13 - Valida un artículo por EAN13
        /// Equivalente a fValidarEAN13 de VB6
        /// </summary>
        private async Task<bool> fValidarEAN13(string stEAN13, bool blMensaje = true)
        {
            // VB6: fValidarEAN13 = False
            // VB6: tEAN13 = Mid(stEAN13, 1, 13)
            string tEAN13 = stEAN13.Length > 13 ? stEAN13[..13] : stEAN13;

            // VB6: If ed.rsDameArticuloEAN13.State <> adStateClosed Then ed.rsDameArticuloEAN13.Close
            // VB6: ed.DameArticuloEAN13 stEAN13
            var articulo = await ed.DameArticuloEAN13(tEAN13);

            // VB6: If .RecordCount > 0 Then
            if (articulo != null)
            {
                // VB6: fValidarEAN13 = True
                // VB6: 'Se muestran los datos
                // VB6: RefrescarDatos False, !artcod, !artnom, !artean, !artcj3, !artpea, !artcua
                RefrescarDatos(false, articulo.Artcod, articulo.Artnom ?? "", 
                    articulo.Artean ?? "", articulo.Artcj3, articulo.Artpea, articulo.Artcua);

                // VB6: 'Se procede a repartir el artículo
                // VB6: If RepartirArticulo(!artcod) Then
                if (await RepartirArticulo(articulo.Artcod))
                {
                    // VB6: Call wsMensaje(" Se ha reservado el BAC para el Artículo: " & !artcod, MENSAJE_Exclamacion)
                    await MessageService.Instance.WsMensaje(
                        $"Se ha reservado el BAC para el Artículo: {articulo.Artcod}", 
                        Constants.MENSAJE_Exclamacion);
                }

                return true;
            }
            else
            {
                // VB6: 'No se ha encontrado el Artículo
                // VB6: If blMensaje Then Call wsMensaje(" No existe el Artículo ", vbCritical)
                if (blMensaje)
                {
                    await MessageService.Instance.WsMensaje("No existe el Artículo", 16);
                }
            }

            return false;
        }

        /// <summary>
        /// RefrescarDatos - Actualiza la visualización de datos
        /// Equivalente a RefrescarDatos de VB6
        /// </summary>
        private void RefrescarDatos(bool sEnBlanco, int sArticulo = 0, string sNombre = "",
            string sEAN13 = "", int sSTD = 0, double sPeso = 0, double sVolumen = 0)
        {
            if (sEnBlanco)
            {
                // VB6: lbArticulo = ""
                lbArticulo.Text = "";
                lbNombreArticulo.Text = "";
                lbEAN13.Text = "";
                lbSTD.Text = "";
                lbPeso.Text = "";
                lbVolumen.Text = "";
            }
            else
            {
                // VB6: lbArticulo = CStr(sArticulo)
                lbArticulo.Text = sArticulo.ToString();
                lbNombreArticulo.Text = sNombre;
                lbEAN13.Text = sEAN13;
                lbSTD.Text = sSTD.ToString();
                // VB6: lbPeso = CStr(Format(sPeso, "#0.0000"))
                lbPeso.Text = sPeso.ToString("#0.0000");
                // VB6: lbVolumen = CStr(Format(sVolumen, "#0.0000"))
                lbVolumen.Text = sVolumen.ToString("#0.0000");
            }
        }

        /// <summary>
        /// RepartirArticulo - Reparte el artículo
        /// Equivalente a RepartirArticulo de VB6
        /// </summary>
        private async Task<bool> RepartirArticulo(int tArticulo)
        {
            // VB6: 'Reparto del Artículo
            // VB6: RepartirArticulo = False
            // VB6: ed.ReservaBACdePTL tArticulo, tUsuario, Retorno, msgSalida
            var resultado = await ed.ReservaBACdePTL(tArticulo, tUsuario);

            // VB6: If Retorno = 0 Then
            if (resultado.Exitoso)
            {
                return true;
            }
            else
            {
                // VB6: Call wsMensaje(" No se ha podido repartir el Artículo. " & msgSalida, vbCritical)
                await MessageService.Instance.WsMensaje(
                    $"No se ha podido repartir el Artículo. {resultado.MsgSalida}", 16);
            }

            return false;
        }
    }
}
