using System.Collections.ObjectModel;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario de consultas PTL
    /// Migrado desde VB6 frmConsultaPTL.frm
    /// </summary>
    public partial class ConsultaPTLPage : ContentPage
    {
        // TESTING_MODE: Set to true to test UI without DAL
        private const bool TESTING_MODE = true;

        private ObservableCollection<ArticuloItem> _articulos = new();

        public ConsultaPTLPage()
        {
            InitializeComponent();
            collectionViewArticulos.ItemsSource = _articulos;
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

            // Determinar tipo de consulta
            await ProcesarConsulta(codigo);

            // Limpiar entrada para siguiente consulta
            txtLecturaCodigo.Text = string.Empty;
            txtLecturaCodigo.Focus();
        }

        private async Task ProcesarConsulta(string codigo)
        {
            try
            {
                // Detectar tipo de código
                if (EsUbicacion(codigo))
                {
                    await ConsultarUbicacion(codigo);
                }
                else if (EsCaja(codigo))
                {
                    await ConsultarCaja(codigo);
                }
                else
                {
                    await ConsultarBAC(codigo);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al procesar consulta: {ex.Message}", "OK");
            }
        }

        private bool EsUbicacion(string codigo)
        {
            // Ubicación: 12 dígitos
            return codigo.Length == 12 && long.TryParse(codigo, out _);
        }

        private bool EsCaja(string codigo)
        {
            // TODO: Reemplazar con lógica real de detección de cajas cuando esté definido el formato
            // Por ahora, asumimos que empieza con "C" o patrón específico
            // Esta lógica debe actualizarse según el formato real de códigos de caja en producción
            return codigo.StartsWith("C", StringComparison.OrdinalIgnoreCase);
        }

        private async Task ConsultarUbicacion(string ubicacionCodigo)
        {
            // TODO: Implementar consulta real cuando tengamos DAL
            
            // Validar longitud antes de parsear
            if (ubicacionCodigo.Length != 12)
            {
                await DisplayAlert("Error", "Código de ubicación debe ser de 12 dígitos", "OK");
                return;
            }

            // Parsear código
            int alm = int.Parse(ubicacionCodigo.Substring(0, 3));
            int blo = int.Parse(ubicacionCodigo.Substring(3, 3));
            int fil = int.Parse(ubicacionCodigo.Substring(6, 3));
            int alt = int.Parse(ubicacionCodigo.Substring(9, 3));

            bool ubicacionExiste = TESTING_MODE;

            if (ubicacionExiste)
            {
                // Mostrar datos de la ubicación
                RefrescarDatos(
                    ubicacionId: 12345,
                    alm: alm, blo: blo, fil: fil, alt: alt,
                    bac: "BAC12345",
                    estadoBAC: 0,
                    grupo: 1,
                    tablilla: 1,
                    numCaja: "C001",
                    peso: 25.5,
                    volumen: 1.250,
                    tipoCaja: "STD",
                    nombreCaja: "CAJA ESTANDAR");

                // Cargar artículos del BAC
                await CargarArticulosBAC(1, "BAC12345");
            }
            else
            {
                await DisplayAlert("Error", "No existe la Ubicación", "OK");
                LimpiarDatos();
            }
        }

        private async Task ConsultarBAC(string bacCodigo)
        {
            // TODO: Implementar consulta real cuando tengamos DAL
            
            bool bacExiste = TESTING_MODE;

            if (bacExiste)
            {
                // Mostrar datos del BAC
                RefrescarDatos(
                    ubicacionId: 12345,
                    alm: 1, blo: 2, fil: 3, alt: 4,
                    bac: bacCodigo,
                    estadoBAC: 0,
                    grupo: 1,
                    tablilla: 1,
                    numCaja: "C001",
                    peso: 25.5,
                    volumen: 1.250,
                    tipoCaja: "STD",
                    nombreCaja: "CAJA ESTANDAR");

                // Cargar artículos del BAC
                await CargarArticulosBAC(1, bacCodigo);
            }
            else
            {
                await DisplayAlert("Error", "No existe el BAC", "OK");
                LimpiarDatos();
            }
        }

        private async Task ConsultarCaja(string cajaCodigo)
        {
            // TODO: Implementar consulta real cuando tengamos DAL
            
            bool cajaExiste = TESTING_MODE;

            if (cajaExiste)
            {
                // Mostrar datos de la caja
                RefrescarDatos(
                    ubicacionId: 0, // Sin ubicación específica
                    alm: 0, blo: 0, fil: 0, alt: 0,
                    bac: "N/A",
                    estadoBAC: 1,
                    grupo: 1,
                    tablilla: 1,
                    numCaja: cajaCodigo,
                    peso: 10.5,
                    volumen: 0.500,
                    tipoCaja: "STD",
                    nombreCaja: "CAJA ESTANDAR");

                // Cargar artículos de la caja
                await CargarArticulosCaja(1, 1, cajaCodigo);
            }
            else
            {
                await DisplayAlert("Error", "No existe la CAJA", "OK");
                LimpiarDatos();
            }
        }

        private async Task CargarArticulosBAC(int grupo, string bac)
        {
            // TODO: Consultar BD real cuando tengamos DAL
            
            _articulos.Clear();

            if (TESTING_MODE)
            {
                // Datos de prueba
                _articulos.Add(new ArticuloItem { Codigo = "ART001", Nombre = "ARTÍCULO DE PRUEBA 1", Cantidad = 10 });
                _articulos.Add(new ArticuloItem { Codigo = "ART002", Nombre = "ARTÍCULO DE PRUEBA 2", Cantidad = 25 });
                _articulos.Add(new ArticuloItem { Codigo = "ART003", Nombre = "ARTÍCULO DE PRUEBA 3", Cantidad = 15 });
            }

            // Actualizar contador de unidades
            int totalUds = _articulos.Sum(a => a.Cantidad);
            lblUds.Text = totalUds.ToString();

            await Task.CompletedTask;
        }

        private async Task CargarArticulosCaja(int grupo, int tablilla, string caja)
        {
            // TODO: Consultar BD real cuando tengamos DAL
            
            _articulos.Clear();

            if (TESTING_MODE)
            {
                // Datos de prueba
                _articulos.Add(new ArticuloItem { Codigo = "ART010", Nombre = "ARTÍCULO CAJA 1", Cantidad = 5 });
                _articulos.Add(new ArticuloItem { Codigo = "ART011", Nombre = "ARTÍCULO CAJA 2", Cantidad = 8 });
            }

            // Actualizar contador de unidades
            int totalUds = _articulos.Sum(a => a.Cantidad);
            lblUds.Text = totalUds.ToString();

            await Task.CompletedTask;
        }

        private void RefrescarDatos(
            int ubicacionId,
            int alm, int blo, int fil, int alt,
            string bac,
            int estadoBAC,
            int grupo,
            int tablilla,
            string numCaja,
            double peso,
            double volumen,
            string tipoCaja,
            string nombreCaja)
        {
            if (ubicacionId == 0)
            {
                lblUbicacion.Text = "SIN UBICACION";
            }
            else
            {
                lblUbicacion.Text = $"({ubicacionId}) {alm:000}.{blo:000}.{fil:000}.{alt:000}";
            }

            lblBAC.Text = bac;
            lblBAC.BackgroundColor = estadoBAC == 0 ? Colors.White : Colors.LightGreen;

            lblGrupo.Text = grupo.ToString();
            lblTablilla.Text = tablilla.ToString();
            lblNumCaja.Text = numCaja;
            lblUds.Text = "0"; // Se actualizará al cargar artículos

            lblPeso.Text = $"{peso:F3}";
            lblVolumen.Text = $"{volumen:F3}";

            lblTipoCaja.Text = tipoCaja;
            lblNombreCaja.Text = nombreCaja;
        }

        private void LimpiarDatos()
        {
            txtLecturaCodigo.Text = string.Empty;
            
            lblUbicacion.Text = "-";
            lblBAC.Text = "-";
            lblBAC.BackgroundColor = Colors.White;
            
            lblGrupo.Text = "-";
            lblTablilla.Text = "-";
            lblNumCaja.Text = "-";
            lblUds.Text = "-";
            
            lblPeso.Text = "-";
            lblPeso.BackgroundColor = Colors.White;
            
            lblVolumen.Text = "-";
            lblVolumen.BackgroundColor = Colors.White;
            
            lblTipoCaja.Text = "-";
            lblNombreCaja.Text = "-";

            _articulos.Clear();
        }

        private async void OnSalirClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        protected override bool OnBackButtonPressed()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.GoToAsync("..");
            });
            return true;
        }
    }

    /// <summary>
    /// Item de artículo para el CollectionView
    /// TODO: Mover a Models/ cuando se organice el proyecto
    /// </summary>
    public class ArticuloItem
    {
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}
