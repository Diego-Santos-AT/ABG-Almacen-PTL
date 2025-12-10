using ABGAlmacenPTL.Modules;
using ABGAlmacenPTL.Pages.Generic;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario para repartir artículos en el sistema PTL
    /// Migrado desde VB6 frmRepartirArticulo.frm
    /// </summary>
    public partial class RepartirArticuloPage : ContentPage
    {
        // Colores de puestos (correspondientes con VB6)
        private readonly Dictionary<string, Color> _coloresPuestos = new()
        {
            { "Puesto 1", Color.FromRgb(255, 0, 0) },      // Rojo
            { "Puesto 2", Color.FromRgb(0, 255, 0) },      // Verde
            { "Puesto 3", Color.FromRgb(0, 0, 255) },      // Azul
            { "Puesto 4", Color.FromRgb(255, 255, 0) },    // Amarillo
            { "Puesto 5", Color.FromRgb(255, 0, 255) },    // Magenta
        };

        public RepartirArticuloPage()
        {
            InitializeComponent();
            CargarPuestos();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Limpiar datos
            LimpiarDatos();
            
            // Focus en campo de código
            txtLecturaCodigo.Focus();
        }

        private void CargarPuestos()
        {
            // TODO: Cargar puestos desde base de datos cuando esté implementado el DAL
            // Por ahora, usar puestos de ejemplo
            pickerPuesto.Items.Clear();
            foreach (var puesto in _coloresPuestos.Keys)
            {
                pickerPuesto.Items.Add(puesto);
            }

            // Seleccionar primer puesto por defecto
            if (pickerPuesto.Items.Count > 0)
            {
                pickerPuesto.SelectedIndex = 0;
            }
        }

        private void OnPuestoSeleccionChanged(object sender, EventArgs e)
        {
            if (pickerPuesto.SelectedItem is string puesto && _coloresPuestos.ContainsKey(puesto))
            {
                colorPuesto.Color = _coloresPuestos[puesto];
            }
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
            // TODO: Consultar base de datos
            // Por ahora, mostrar datos de ejemplo
            
            // Simulación de datos
            bool encontrado = false; // Cambiar a true cuando tengamos DAL

            if (encontrado)
            {
                // Mostrar datos del artículo
                RefrescarDatos(
                    codigoArticulo: codigoArticulo,
                    nombreArticulo: "ARTÍCULO DE EJEMPLO",
                    ean13: "1234567890123",
                    std: "STD001",
                    peso: "1.5",
                    volumen: "0.025");

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

            // Limpiar entrada para siguiente escaneo
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        private async Task ValidarEAN13(string ean13)
        {
            // TODO: Consultar base de datos por EAN13
            // Por ahora, similar a ValidarArticulo
            
            await DisplayAlert("Info", "Validación EAN13 - Pendiente implementación DAL", "OK");
            
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        private async Task<bool> RepartirArticulo(string codigoArticulo)
        {
            // TODO: Implementar lógica de reparto cuando tengamos DAL
            // Esta función debe:
            // 1. Buscar BAC disponible del artículo
            // 2. Asignar BAC al puesto seleccionado
            // 3. Encender luz del puesto
            // 4. Registrar operación en BD

            await Task.Delay(100); // Simulación
            return false; // Cambiar cuando esté implementado
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
