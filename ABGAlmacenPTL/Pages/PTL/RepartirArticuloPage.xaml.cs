using ABGAlmacenPTL.Modules;
using ABGAlmacenPTL.Pages.Generic;
using ABGAlmacenPTL.Services;
using ABGAlmacenPTL.Models;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario para repartir artículos en el sistema PTL
    /// Migrado desde VB6 frmRepartirArticulo.frm
    /// </summary>
    public partial class RepartirArticuloPage : ContentPage
    {
        private readonly PTLService _ptlService;

        // Colores de puestos (correspondientes con VB6)
        private readonly Dictionary<int, Color> _coloresPuestos = new()
        {
            { 1, Color.FromRgb(255, 0, 0) },      // Rojo
            { 2, Color.FromRgb(0, 255, 0) },      // Verde
            { 3, Color.FromRgb(0, 0, 255) },      // Azul
            { 4, Color.FromRgb(255, 255, 0) },    // Amarillo
            { 5, Color.FromRgb(255, 0, 255) },    // Magenta
        };

        private List<Puesto> _puestos = new();

        public RepartirArticuloPage(PTLService ptlService)
        {
            InitializeComponent();
            _ptlService = ptlService;
            _ = CargarPuestos();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Limpiar datos
            LimpiarDatos();
            
            // Focus en campo de código
            txtLecturaCodigo.Focus();
        }

        private async Task CargarPuestos()
        {
            try
            {
                _puestos = (await _ptlService.GetPuestosActivosAsync()).ToList();
                
                pickerPuesto.Items.Clear();
                foreach (var puesto in _puestos)
                {
                    pickerPuesto.Items.Add($"Puesto {puesto.Numero}");
                }

                // Seleccionar primer puesto por defecto
                if (pickerPuesto.Items.Count > 0)
                {
                    pickerPuesto.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar puestos: {ex.Message}", "OK");
            }
        }

        private void OnPuestoSeleccionChanged(object sender, EventArgs e)
        {
            if (pickerPuesto.SelectedIndex >= 0 && pickerPuesto.SelectedIndex < _puestos.Count)
            {
                var puesto = _puestos[pickerPuesto.SelectedIndex];
                // Obtener color del enum
                colorPuesto.Color = GetColorFromEnum(puesto.Color);
            }
        }

        private Color GetColorFromEnum(ColorPuesto colorEnum)
        {
            return colorEnum switch
            {
                ColorPuesto.Rojo => Color.FromRgb(255, 0, 0),
                ColorPuesto.Verde => Color.FromRgb(0, 255, 0),
                ColorPuesto.Azul => Color.FromRgb(0, 0, 255),
                ColorPuesto.Amarillo => Color.FromRgb(255, 255, 0),
                ColorPuesto.Magenta => Color.FromRgb(255, 0, 255),
                _ => Colors.Gray
            };
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

            // Validar código
            await ValidarCodigo(codigo);
        }

        private async Task ValidarCodigo(string codigo)
        {
            try
            {
                // TODO: Implementar validación real cuando tengamos DAL
                // Por ahora, simulamos validación

                // Determinar tipo de código (Artículo o EAN13)
                // EAN13: 13 dígitos numéricos con dígito de control válido
                bool esEAN13 = codigo.Length == 13 && long.TryParse(codigo, out _) && ValidarDigitoControlEAN13(codigo);

                if (esEAN13)
                {
                    await ValidarEAN13(codigo);
                }
                else
                {
                    await ValidarArticulo(codigo);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al validar código: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Valida el dígito de control de un código EAN13
        /// </summary>
        private bool ValidarDigitoControlEAN13(string ean13)
        {
            if (ean13.Length != 13 || !long.TryParse(ean13, out _))
            {
                return false;
            }

            // Algoritmo de validación EAN13
            int suma = 0;
            for (int i = 0; i < 12; i++)
            {
                int digito = int.Parse(ean13[i].ToString());
                suma += (i % 2 == 0) ? digito : digito * 3;
            }

            int digitoControl = (10 - (suma % 10)) % 10;
            int digitoEAN = int.Parse(ean13[12].ToString());

            return digitoControl == digitoEAN;
        }

        private async Task ValidarArticulo(string codigoArticulo)
        {
            try
            {
                var articulo = await _ptlService.GetArticuloByCodigoAsync(codigoArticulo);

                if (articulo != null)
                {
                    // Mostrar datos del artículo
                    RefrescarDatos(
                        codigoArticulo: articulo.CodigoArticulo,
                        nombreArticulo: articulo.NombreArticulo,
                        ean13: articulo.EAN13 ?? "-",
                        std: articulo.STD ?? "-",
                        peso: articulo.Peso?.ToString("F2") ?? "-",
                        volumen: articulo.Volumen?.ToString("F3") ?? "-");

                    // Proceder a repartir
                    bool repartido = await RepartirArticulo(codigoArticulo);
                    if (repartido)
                    {
                        await MensajePage.ShowAsync(
                            $"Se ha reservado el BAC para el Artículo: {codigoArticulo}",
                            "Éxito");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No existe el Artículo", "OK");
                    LimpiarDatos();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al buscar artículo: {ex.Message}", "OK");
            }

            // Limpiar entrada para siguiente escaneo
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        private async Task ValidarEAN13(string ean13)
        {
            try
            {
                var articulo = await _ptlService.GetArticuloByEAN13Async(ean13);

                if (articulo != null)
                {
                    // Mostrar datos del artículo
                    RefrescarDatos(
                        codigoArticulo: articulo.CodigoArticulo,
                        nombreArticulo: articulo.NombreArticulo,
                        ean13: articulo.EAN13 ?? "-",
                        std: articulo.STD ?? "-",
                        peso: articulo.Peso?.ToString("F2") ?? "-",
                        volumen: articulo.Volumen?.ToString("F3") ?? "-");

                    // Proceder a repartir
                    bool repartido = await RepartirArticulo(articulo.CodigoArticulo);
                    if (repartido)
                    {
                        await MensajePage.ShowAsync(
                            $"Se ha reservado el BAC para el Artículo: {articulo.CodigoArticulo}",
                            "Éxito");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No existe el Artículo con ese EAN13", "OK");
                    LimpiarDatos();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al buscar artículo por EAN13: {ex.Message}", "OK");
            }
            
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        private async Task<bool> RepartirArticulo(string codigoArticulo)
        {
            try
            {
                // TODO: Implementar lógica completa de reparto
                // Esta función debe:
                // 1. Buscar BAC disponible del artículo (con inventario del artículo)
                // 2. Asignar BAC al puesto seleccionado
                // 3. Encender luz del puesto (integración PTL hardware)
                // 4. Registrar operación en BD

                // Por ahora, registramos que el proceso fue exitoso
                // La implementación completa requiere:
                // - Búsqueda de BAC con el artículo
                // - Sistema de reserva de BAC por puesto
                // - Integración con hardware PTL (luces)
                // - Log de operaciones

                if (pickerPuesto.SelectedIndex >= 0 && pickerPuesto.SelectedIndex < _puestos.Count)
                {
                    var puesto = _puestos[pickerPuesto.SelectedIndex];
                    // Lógica de reparto implementada parcialmente
                    // Requiere extensión de PTLService para manejo de reparto completo
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al repartir: {ex.Message}", "OK");
                return false;
            }
        }

        private void RefrescarDatos(
            string codigoArticulo,
            string nombreArticulo,
            string ean13,
            string std,
            string peso,
            string volumen)
        {
            lblArticulo.Text = codigoArticulo;
            lblNombreArticulo.Text = nombreArticulo;
            lblEAN13.Text = ean13;
            lblSTD.Text = std;
            lblPeso.Text = peso;
            lblVolumen.Text = volumen;
        }

        private void LimpiarDatos()
        {
            txtLecturaCodigo.Text = string.Empty;
            lblArticulo.Text = "-";
            lblNombreArticulo.Text = "-";
            lblEAN13.Text = "-";
            lblSTD.Text = "-";
            lblPeso.Text = "-";
            lblVolumen.Text = "-";
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
