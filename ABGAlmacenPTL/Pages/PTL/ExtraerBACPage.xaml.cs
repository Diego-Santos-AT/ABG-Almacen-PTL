using ABGAlmacenPTL.Pages.Generic;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario para extraer BAC del sistema PTL
    /// Migrado desde VB6 frmExtraerBAC.frm
    /// </summary>
    public partial class ExtraerBACPage : ContentPage
    {
        // TESTING MODE: Set to true to test UI without DAL
        private const bool TESTING_MODE = true;
        
        private int _ubicacionId = 0;
        private string _bacCodigo = string.Empty;

        public ExtraerBACPage()
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

            // Validar ubicación y extraer BAC
            await ValidarUbicacion(codigo);

            // Limpiar entrada para siguiente escaneo
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        private async Task ValidarUbicacion(string ubicacionCodigo)
        {
            try
            {
                // TODO: Implementar validación real cuando tengamos DAL
                
                // Parsear código de ubicación (12 dígitos)
                if (ubicacionCodigo.Length != 12 || !long.TryParse(ubicacionCodigo, out _))
                {
                    await DisplayAlert("Error", "Código de ubicación inválido", "OK");
                    return;
                }

                int alm = int.Parse(ubicacionCodigo.Substring(0, 3));
                int blo = int.Parse(ubicacionCodigo.Substring(3, 3));
                int fil = int.Parse(ubicacionCodigo.Substring(6, 3));
                int alt = int.Parse(ubicacionCodigo.Substring(9, 3));

                // TODO: Consultar BD para obtener BAC de la ubicación
                bool ubicacionExiste = TESTING_MODE; // Change to DB query when DAL available
                bool ubicacionTieneBAC = TESTING_MODE; // Verificar si tiene BAC

                if (ubicacionExiste)
                {
                    if (ubicacionTieneBAC)
                    {
                        // Mostrar datos del BAC en la ubicación
                        _ubicacionId = 12345; // TODO: ID real de BD
                        _bacCodigo = "BAC12345"; // TODO: Código real de BD

                        lblUbicacion.Text = $"({_ubicacionId}) {alm:000}.{blo:000}.{fil:000}.{alt:000}";
                        
                        RefrescarDatosBAC(
                            bac: _bacCodigo,
                            estadoBAC: "ABIERTO", // TODO: Estado real
                            grupo: 1,
                            tablilla: 1,
                            uds: 100,
                            peso: "25.5",
                            volumen: "1.250",
                            tipoCaja: "STD",
                            nombreCaja: "CAJA ESTANDAR");

                        // Confirmar extracción
                        bool confirmar = await DisplayAlert(
                            "Confirmar Extracción",
                            $"¿Extraer BAC {_bacCodigo} de la ubicación?",
                            "Sí",
                            "No");

                        if (confirmar)
                        {
                            bool extraido = await ExtraerBAC(_bacCodigo, _ubicacionId);
                            if (extraido)
                            {
                                await MensajePage.ShowAsync(
                                    $"Se ha extraído el BAC: {_bacCodigo} de la ubicación PTL {_ubicacionId}",
                                    "Éxito");
                                
                                LimpiarDatos();
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "La Ubicación no tiene ningún BAC asociado", "OK");
                        _ubicacionId = 0;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No existe la Ubicación", "OK");
                    _ubicacionId = 0;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al validar ubicación: {ex.Message}", "OK");
            }
        }

        private async Task<bool> ExtraerBAC(string bacCodigo, int ubicacionId)
        {
            // TODO: Implementar lógica de extracción cuando tengamos DAL
            // Esta función debe:
            // 1. Verificar que el BAC existe en la ubicación
            // 2. Eliminar asociación BAC-Ubicación
            // 3. Actualizar estado del BAC según opción seleccionada
            // 4. Apagar luz del puesto PTL
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
            lblVolumen.Text = "-";
            
            lblTipoCaja.Text = "-";
            lblNombreCaja.Text = "-";
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
