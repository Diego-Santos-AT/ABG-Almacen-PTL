using ABGAlmacenPTL.Pages.Generic;
using ABGAlmacenPTL.Services;
using ABGAlmacenPTL.Models;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario para ubicar BAC en el sistema PTL
    /// Migrado desde VB6 frmUbicarBAC.frm
    /// </summary>
    public partial class UbicarBACPage : ContentPage
    {
        private readonly PTLService _ptlService;
        
        private string _ubicacionCodigo = string.Empty;
        private string _bacCodigo = string.Empty;
        private EstadoBAC _estadoBAC = EstadoBAC.Abierto;

        public UbicarBACPage(PTLService ptlService)
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

                // Buscar ubicación en BD
                var ubicacion = await _ptlService.GetUbicacionByCodigoAsync(ubicacionCodigo);

                if (ubicacion != null)
                {
                    // Verificar si ya tiene BAC asignado
                    var bacEnUbicacion = await _ptlService.GetBACEnUbicacionAsync(ubicacionCodigo);
                    
                    if (bacEnUbicacion == null)
                    {
                        // Ubicación válida y libre
                        _ubicacionCodigo = ubicacionCodigo;
                        lblUbicacion.Text = $"{alm:000}.{blo:000}.{fil:000}.{alt:000}";

                        // Si ya tenemos un BAC escaneado, proceder a ubicar
                        if (!string.IsNullOrEmpty(_bacCodigo))
                        {
                            bool ubicado = await UbicarBAC(_bacCodigo, ubicacionCodigo);
                            if (ubicado)
                            {
                                await MensajePage.ShowAsync(
                                    $"Se ha ubicado el BAC: {_bacCodigo} en la ubicación PTL {ubicacionCodigo}",
                                    "Éxito");
                                
                                LimpiarDatos();
                            }
                        }
                    }
                    else
                    {
                        await DisplayAlert("Error", "La Ubicación ya tiene asociado un BAC", "OK");
                        _ubicacionCodigo = string.Empty;
                        lblUbicacion.Text = "-";
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No existe la Ubicación", "OK");
                    _ubicacionCodigo = string.Empty;
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
                // Buscar BAC en BD
                var bac = await _ptlService.GetBACByCodigoAsync(bacCodigo);

                if (bac != null)
                {
                    // Mostrar datos del BAC
                    _bacCodigo = bacCodigo;
                    _estadoBAC = bac.Estado;

                    var articulos = await _ptlService.GetArticulosEnBACAsync(bacCodigo);
                    int totalUds = articulos.Sum(a => 1); // TODO: Sum actual quantities from junction table

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

                    // Si ya tenemos una ubicación, proceder a ubicar
                    if (!string.IsNullOrEmpty(_ubicacionCodigo))
                    {
                        bool ubicado = await UbicarBAC(_bacCodigo, _ubicacionCodigo);
                        if (ubicado)
                        {
                            await MensajePage.ShowAsync(
                                $"Se ha ubicado el BAC: {_bacCodigo} en la ubicación PTL {_ubicacionCodigo}",
                                "Éxito");
                            
                            LimpiarDatos();
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No existe el BAC", "OK");
                    _bacCodigo = string.Empty;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al validar BAC: {ex.Message}", "OK");
            }
        }

        private async Task<bool> UbicarBAC(string bacCodigo, string ubicacionCodigo)
        {
            try
            {
                // Obtener estado del BAC según selección
                bool abrirBAC = rbAbrir.IsChecked;
                EstadoBAC nuevoEstado = abrirBAC ? EstadoBAC.Abierto : EstadoBAC.Cerrado;

                // Asignar BAC a ubicación
                bool resultado = await _ptlService.AsignarBACaUbicacionAsync(
                    bacCodigo, 
                    ubicacionCodigo, 
                    nuevoEstado);

                return resultado;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al ubicar BAC: {ex.Message}", "OK");
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
