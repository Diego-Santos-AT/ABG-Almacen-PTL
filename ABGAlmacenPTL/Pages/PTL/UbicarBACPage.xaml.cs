using ABGAlmacenPTL.Pages.Generic;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario para ubicar BAC en el sistema PTL
    /// Migrado desde VB6 frmUbicarBAC.frm
    /// </summary>
    public partial class UbicarBACPage : ContentPage
    {
        // TESTING MODE: Set to true to test UI without DAL
        private const bool TESTING_MODE = true;
        
        private int _ubicacionId = 0;
        private string _bacCodigo = string.Empty;
        private int _estadoBAC = 0; // 0 = ABIERTO, 1 = CERRADO

        public UbicarBACPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Limpiar datos
            LimpiarDatos();
            
            // Focus en campo de código
            txtLecturaCodigo.Focus();
        }

        private void OnCodigoTextChanged(object sender, TextChangedEventArgs e)
        {
            // Convertir a mayúsculas (comportamiento VB6)
            if (sender is Entry entry && !string.IsNullOrEmpty(e.NewTextValue))
            {
                var upper = e.NewTextValue.ToUpper();
                if (upper != e.NewTextValue)
                {
                    entry.Text = upper;
                }
            }
        }

        private async void OnCodigoCompleted(object sender, EventArgs e)
        {
            var codigo = txtLecturaCodigo.Text?.Trim();
            
            if (string.IsNullOrEmpty(codigo))
            {
                return;
            }

            // Determinar si es ubicación o BAC
            if (EsUbicacion(codigo))
            {
                await ValidarUbicacion(codigo);
            }
            else
            {
                await ValidarBAC(codigo);
            }

            // Limpiar entrada para siguiente escaneo
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        private bool EsUbicacion(string codigo)
        {
            // Ubicación PTL: 12 dígitos (formato: AAABBBCCCDD)
            // AAA = Almacén, BBB = Bloque, CCC = Fila, DDD = Altura
            return codigo.Length == 12 && long.TryParse(codigo, out _);
        }

        private async Task ValidarUbicacion(string ubicacionCodigo)
        {
            try
            {
                // TODO: Implementar validación real cuando tengamos DAL
                
                // Parsear código de ubicación
                if (ubicacionCodigo.Length != 12)
                {
                    await DisplayAlert("Error", "Código de ubicación inválido", "OK");
                    return;
                }

                int alm = int.Parse(ubicacionCodigo.Substring(0, 3));
                int blo = int.Parse(ubicacionCodigo.Substring(3, 3));
                int fil = int.Parse(ubicacionCodigo.Substring(6, 3));
                int alt = int.Parse(ubicacionCodigo.Substring(9, 3));

                // TODO: Consultar BD para validar ubicación
                bool ubicacionExiste = TESTING_MODE; // Change to DB query when DAL available
                bool ubicacionOcupada = false; // Verificar si ya tiene BAC

                if (ubicacionExiste)
                {
                    if (!ubicacionOcupada)
                    {
                        // Ubicación válida y libre
                        _ubicacionId = 12345; // TODO: Obtener ID real de BD
                        lblUbicacion.Text = $"({_ubicacionId}) {alm:000}.{blo:000}.{fil:000}.{alt:000}";

                        // Si ya tenemos un BAC escaneado, proceder a ubicar
                        if (!string.IsNullOrEmpty(_bacCodigo))
                        {
                            bool ubicado = await UbicarBAC(_bacCodigo, _ubicacionId);
                            if (ubicado)
                            {
                                await MensajePage.ShowAsync(
                                    $"Se ha ubicado el BAC: {_bacCodigo} en la ubicación PTL {_ubicacionId}",
                                    "Éxito");
                                
                                LimpiarDatos();
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "La Ubicación ya tiene asociado un BAC", "OK");
                        _ubicacionId = 0;
                        lblUbicacion.Text = "-";
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No existe la Unidad de Transporte", "OK");
                    _ubicacionId = 0;
                    lblUbicacion.Text = "-";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al validar ubicación: {ex.Message}", "OK");
            }
        }

        private async Task ValidarBAC(string bacCodigo)
        {
            try
            {
                // TODO: Implementar validación real cuando tengamos DAL
                
                // Simulación de datos
                bool bacExiste = TESTING_MODE; // Change to DB query when DAL available

                if (bacExiste)
                {
                    // Mostrar datos del BAC
                    _bacCodigo = bacCodigo;
                    _estadoBAC = 0; // TODO: Obtener estado real de BD

                    RefrescarDatosBAC(
                        bac: bacCodigo,
                        estadoBAC: _estadoBAC == 0 ? "ABIERTO" : "CERRADO",
                        grupo: 1,
                        tablilla: 1,
                        uds: 100,
                        peso: "25.5",
                        volumen: "1.250",
                        tipoCaja: "STD",
                        nombreCaja: "CAJA ESTANDAR");

                    // Si ya tenemos una ubicación, proceder a ubicar
                    if (_ubicacionId > 0)
                    {
                        bool ubicado = await UbicarBAC(_bacCodigo, _ubicacionId);
                        if (ubicado)
                        {
                            await MensajePage.ShowAsync(
                                $"Se ha ubicado el BAC: {_bacCodigo} en la ubicación PTL {_ubicacionId}",
                                "Éxito");
                            
                            LimpiarDatos();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No existe la Unidad de Transporte (BAC)", "OK");
                    _bacCodigo = string.Empty;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al validar BAC: {ex.Message}", "OK");
            }
        }

        private async Task<bool> UbicarBAC(string bacCodigo, int ubicacionId)
        {
            // TODO: Implementar lógica de ubicación cuando tengamos DAL
            // Esta función debe:
            // 1. Verificar que el BAC existe
            // 2. Verificar que la ubicación existe y está libre
            // 3. Asignar el BAC a la ubicación
            // 4. Actualizar estado del BAC según opción seleccionada
            // 5. Registrar operación en BD

            bool cerrarBAC = rbCerrar.IsChecked;
            
            // Simulación
            await Task.Delay(100);
            
            // TODO: Replace with real DAL implementation
            return TESTING_MODE; // Returns success in testing mode
        }

        private void RefrescarDatosBAC(
            string bac,
            string estadoBAC,
            int grupo,
            int tablilla,
            int uds,
            string peso,
            string volumen,
            string tipoCaja,
            string nombreCaja)
        {
            lblBAC.Text = bac;
            lblEstadoBAC.Text = estadoBAC;
            lblEstadoBAC.BackgroundColor = estadoBAC == "ABIERTO" ? Colors.LightGreen : Colors.LightCoral;
            
            lblGrupo.Text = grupo.ToString();
            lblTablilla.Text = tablilla.ToString();
            lblUds.Text = uds.ToString();
            
            lblPeso.Text = peso;
            lblVolumen.Text = volumen;
            
            lblTipoCaja.Text = tipoCaja;
            lblNombreCaja.Text = nombreCaja;
        }

        private void LimpiarDatos()
        {
            txtLecturaCodigo.Text = string.Empty;
            
            _ubicacionId = 0;
            _bacCodigo = string.Empty;
            
            lblUbicacion.Text = "-";
            lblBAC.Text = "-";
            lblEstadoBAC.Text = "-";
            lblEstadoBAC.BackgroundColor = Colors.White;
            
            lblGrupo.Text = "-";
            lblTablilla.Text = "-";
            lblUds.Text = "-";
            
            lblPeso.Text = "-";
            lblPeso.BackgroundColor = Colors.White;
            
            lblVolumen.Text = "-";
            lblVolumen.BackgroundColor = Colors.White;
            
            lblTipoCaja.Text = "-";
            lblNombreCaja.Text = "-";
        }

        private void OnCancelarClicked(object sender, EventArgs e)
        {
            // Cancelar operación actual
            LimpiarDatos();
            txtLecturaCodigo.Focus();
        }

        private async void OnSalirClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        protected override bool OnBackButtonPressed()
        {
            // Permitir navegación hacia atrás
            Shell.Current.GoToAsync("..");
            return true;
        }
    }
}
