using ABGAlmacenPTL.Pages.Generic;
using ABGAlmacenPTL.Services;
using ABGAlmacenPTL.Models;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario para extraer BAC del sistema PTL
    /// Migrado desde VB6 frmExtraerBAC.frm
    /// </summary>
    public partial class ExtraerBACPage : ContentPage
    {
        private readonly PTLService _ptlService;
        
        private string _ubicacionCodigo = string.Empty;
        private string _bacCodigo = string.Empty;

        public ExtraerBACPage(PTLService ptlService)
        {
            InitializeComponent();
            _ptlService = ptlService;
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

                // Buscar ubicación en BD
                var ubicacion = await _ptlService.GetUbicacionByCodigoAsync(ubicacionCodigo);

                if (ubicacion != null)
                {
                    // Buscar BAC en la ubicación
                    var bac = await _ptlService.GetBACEnUbicacionAsync(ubicacionCodigo);
                    
                    if (bac != null)
                    {
                        // Mostrar datos del BAC en la ubicación
                        _ubicacionCodigo = ubicacionCodigo;
                        _bacCodigo = bac.CodigoBAC;

                        lblUbicacion.Text = $"{alm:000}.{blo:000}.{fil:000}.{alt:000}";
                        
                        var articulos = await _ptlService.GetArticulosEnBACAsync(bac.CodigoBAC);
                        int totalUds = articulos.Count(); // TODO: Sum actual quantities from junction table

                        RefrescarDatosBAC(
                            bac: bac.CodigoBAC,
                            estadoBAC: bac.Estado == EstadoBAC.Abierto ? "ABIERTO" : "CERRADO",
                            grupo: bac.Grupo,
                            tablilla: bac.Tablilla,
                            uds: totalUds,
                            peso: bac.Peso?.ToString("F2") ?? "0",
                            volumen: bac.Volumen?.ToString("F3") ?? "0",
                            tipoCaja: "STD", // TODO: Get from BAC type
                            nombreCaja: "CAJA");

                        // Confirmar extracción
                        bool confirmar = await DisplayAlert(
                            "Confirmar Extracción",
                            $"¿Extraer BAC {_bacCodigo} de la ubicación?",
                            "Sí",
                            "No");

                        if (confirmar)
                        {
                            bool extraido = await ExtraerBAC(_bacCodigo, _ubicacionCodigo);
                            if (extraido)
                            {
                                await MensajePage.ShowAsync(
                                    $"Se ha extraído el BAC: {_bacCodigo} de la ubicación PTL {_ubicacionCodigo}",
                                    "Éxito");
                                
                                LimpiarDatos();
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "La Ubicación no tiene ningún BAC asociado", "OK");
                        _ubicacionCodigo = string.Empty;
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No existe la Ubicación", "OK");
                    _ubicacionCodigo = string.Empty;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al validar ubicación: {ex.Message}", "OK");
            }
        }

        private async Task<bool> ExtraerBAC(string bacCodigo, string ubicacionCodigo)
        {
            try
            {
                // Obtener estado del BAC según selección
                bool cerrarBAC = rbCerrar.IsChecked;
                EstadoBAC nuevoEstado = cerrarBAC ? EstadoBAC.Cerrado : EstadoBAC.Abierto;

                // Extraer BAC de ubicación
                bool resultado = await _ptlService.ExtraerBACDeUbicacionAsync(
                    bacCodigo,
                    nuevoEstado);

                return resultado;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al extraer BAC: {ex.Message}", "OK");
                return false;
            }
        }

        private void RefrescarDatosBAC(
            string bac,
            string estadoBAC,
            string? grupo,
            string? tablilla,
            int uds,
            string peso,
            string volumen,
            string tipoCaja,
            string nombreCaja)
        {
            lblBAC.Text = bac;
            lblEstadoBAC.Text = estadoBAC;
            lblEstadoBAC.BackgroundColor = estadoBAC == "ABIERTO" ? Colors.LightGreen : Colors.LightCoral;
            
            lblGrupo.Text = grupo ?? "-";
            lblTablilla.Text = tablilla ?? "-";
            lblUds.Text = uds.ToString();
            
            lblPeso.Text = peso;
            lblVolumen.Text = volumen;
            
            lblTipoCaja.Text = tipoCaja;
            lblNombreCaja.Text = nombreCaja;
        }

        private void LimpiarDatos()
        {
            txtLecturaCodigo.Text = string.Empty;
            
            _ubicacionCodigo = string.Empty;
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
