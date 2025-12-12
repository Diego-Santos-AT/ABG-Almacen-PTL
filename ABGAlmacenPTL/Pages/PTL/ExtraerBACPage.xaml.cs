using ABGAlmacenPTL.Pages.Generic;
using ABGAlmacenPTL.Services;
using ABGAlmacenPTL.Models;
using ABGAlmacenPTL.Modules;
using System.Data;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario para extraer BAC del sistema PTL
    /// Migrado desde VB6 frmExtraerBAC.frm
    /// Ahora usa stored procedures de SELENE para fidelidad 100% con VB6
    /// </summary>
    public partial class ExtraerBACPage : ContentPage
    {
        private readonly PTLServiceEnhanced _ptlService;
        
        private string _ubicacionCodigo = string.Empty;
        private string _bacCodigo = string.Empty;

        public ExtraerBACPage(PTLServiceEnhanced ptlService)
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
                    await DisplayAlertAsync("Error", "Código de ubicación inválido", "OK");
                    return;
                }

                int alm = int.Parse(ubicacionCodigo.Substring(0, 3));
                int blo = int.Parse(ubicacionCodigo.Substring(3, 3));
                int fil = int.Parse(ubicacionCodigo.Substring(6, 3));
                int alt = int.Parse(ubicacionCodigo.Substring(9, 3));

                // Usar stored procedure DameDatosUbicacionPTL (VB6-faithful)
                var ubicacionData = await _ptlService.GetUbicacionDataAsync(ubicacionCodigo);

                if (ubicacionData != null && ubicacionData.Rows.Count > 0)
                {
                    var row = ubicacionData.Rows[0];
                    
                    // Verificar si tiene BAC asignado
                    string? bacCodigo = row.Table.Columns.Contains("CodigoBAC") ? 
                                      row["CodigoBAC"]?.ToString() : null;
                    
                    if (!string.IsNullOrEmpty(bacCodigo))
                    {
                        // Mostrar datos del BAC en la ubicación
                        _ubicacionCodigo = ubicacionCodigo;
                        _bacCodigo = bacCodigo;

                        lblUbicacion.Text = $"{alm:000}.{blo:000}.{fil:000}.{alt:000}";
                        
                        // Obtener datos completos del BAC usando stored procedure
                        var bacData = await _ptlService.GetBACDataAsync(bacCodigo);
                        if (bacData != null && bacData.Rows.Count > 0)
                        {
                            var bacRow = bacData.Rows[0];
                            
                            RefrescarDatosBAC(
                                bac: bacCodigo,
                                estadoBAC: bacRow["Estado"]?.ToString() ?? "ABIERTO",
                                grupo: bacRow["Grupo"]?.ToString(),
                                tablilla: bacRow["Tablilla"]?.ToString(),
                                uds: bacRow.Table.Columns.Contains("TotalUds") ? Convert.ToInt32(bacRow["TotalUds"] ?? 0) : 0,
                                peso: bacRow.Table.Columns.Contains("Peso") ? (Convert.ToDecimal(bacRow["Peso"] ?? 0)).ToString("F2") : "0",
                                volumen: bacRow.Table.Columns.Contains("Volumen") ? (Convert.ToDecimal(bacRow["Volumen"] ?? 0)).ToString("F3") : "0",
                                tipoCaja: bacRow.Table.Columns.Contains("TipoCaja") ? bacRow["TipoCaja"]?.ToString() ?? "STD" : "STD",
                                nombreCaja: bacRow.Table.Columns.Contains("NombreCaja") ? bacRow["NombreCaja"]?.ToString() ?? "CAJA" : "CAJA");
                        }

                        // Confirmar extracción
                        bool confirmar = await DisplayAlertAsync(
                            "Confirmar Extracción",
                            $"¿Extraer BAC {_bacCodigo} de la ubicación?",
                            "Sí",
                            "No");

                        if (confirmar)
                        {
                            bool extraido = await ExtraerBAC(_bacCodigo);
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
                        await DisplayAlertAsync("Error", "La Ubicación no tiene ningún BAC asociado", "OK");
                        _ubicacionCodigo = string.Empty;
                    }
                }
                else
                {
                    await DisplayAlertAsync("Error", "No existe la Ubicación", "OK");
                    _ubicacionCodigo = string.Empty;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Error al validar ubicación: {ex.Message}", "OK");
            }
        }

        private async Task<bool> ExtraerBAC(string bacCodigo)
        {
            try
            {
                // Usar stored procedure ExtraerBACdePTL (VB6-faithful)
                // El stored procedure maneja toda la lógica de negocio
                bool resultado = await _ptlService.ExtraerBACAsync(
                    bacCodigo,
                    Gestion.wPuestoTrabajo.Id); // Usar puesto de trabajo de la sesión

                return resultado;
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Error al extraer BAC: {ex.Message}", "OK");
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
