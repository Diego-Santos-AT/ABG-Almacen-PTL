using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace ABGAlmacenPTL.Pages.PTL
{
    public partial class EmpaquetarBACPage : ContentPage
    {
        // TESTING_MODE: Set to true to test UI without DAL
        private const bool TESTING_MODE = true;

        // Estado actual
        private string? _currentBAC;
        private string? _currentCaja;
        private long _currentGrupo;
        private long _currentTablilla;
        private int _currentEstadoBAC; // 0=ABIERTO, 1=CERRADO
        
        // Colecciones
        private ObservableCollection<ArticuloItem> _articulos = new();

        public EmpaquetarBACPage()
        {
            InitializeComponent();
            cvArticulos.ItemsSource = _articulos;
            InicializarPantalla();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            txtLecturaCodigo.Focus();
        }

        private void InicializarPantalla()
        {
            // Ocultar todos los frames inicialmente
            frameBAC.IsVisible = false;
            frameCaja.IsVisible = false;
            frameArticulos.IsVisible = false;
            frameAcciones.IsVisible = false;
            
            // Limpiar datos
            LimpiarDatos();
            
            // Focus en entrada
            txtLecturaCodigo.Text = string.Empty;
        }

        private void LimpiarDatos()
        {
            _currentBAC = null;
            _currentCaja = null;
            _currentGrupo = 0;
            _currentTablilla = 0;
            _currentEstadoBAC = 0;
            
            _articulos.Clear();
            
            // Limpiar labels
            lblUbicacion.Text = string.Empty;
            lblBAC.Text = string.Empty;
            lblEstadoBAC.Text = string.Empty;
            lblEstadoBAC.BackgroundColor = Colors.White;
            lblGrupo.Text = string.Empty;
            lblTablilla.Text = string.Empty;
            lblUnidades.Text = string.Empty;
            
            lblSSCC.Text = string.Empty;
            lblTipoCaja.Text = string.Empty;
            lblNombreCaja.Text = string.Empty;
            lblUnidadesCaja.Text = string.Empty;
            lblPesoCaja.Text = string.Empty;
            lblVolumenCaja.Text = string.Empty;
            lblEstadoCaja.Text = string.Empty;
            lblEstadoCaja.BackgroundColor = Colors.White;
        }

        private void OnCodigoTextChanged(object? sender, TextChangedEventArgs e)
        {
            // Convertir a mayúsculas automáticamente (comportamiento VB6)
            if (sender is Entry entry && !string.IsNullOrEmpty(entry.Text))
            {
                var upperText = entry.Text.ToUpper();
                if (entry.Text != upperText)
                {
                    entry.Text = upperText;
                }
            }
        }

        private async void OnCodigoCompleted(object? sender, EventArgs e)
        {
            if (sender is Entry entry)
            {
                var codigo = entry.Text?.Trim().ToUpper();
                if (string.IsNullOrEmpty(codigo))
                    return;

                await ProcesarCodigo(codigo);
                entry.Text = string.Empty;
            }
        }

        private async Task ProcesarCodigo(string codigo)
        {
            // Detectar tipo de código
            if (EsCodigoSSCC(codigo))
            {
                await CargarCaja(codigo);
            }
            else
            {
                await CargarBAC(codigo);
            }
        }

        private bool EsCodigoSSCC(string codigo)
        {
            // SSCC normalmente tiene 18 dígitos y empieza con número específico
            // TODO: Ajustar según formato real del sistema
            return codigo.Length >= 14 && codigo.All(char.IsDigit);
        }

        private async Task CargarBAC(string bacCodigo)
        {
            // TODO: Implementar consulta real cuando tengamos DAL
            if (TESTING_MODE)
            {
                // Simulación de carga de BAC
                _currentBAC = bacCodigo;
                _currentGrupo = 12345;
                _currentTablilla = 1;
                _currentEstadoBAC = 0; // ABIERTO

                lblUbicacion.Text = "001.002.003.004";
                lblBAC.Text = bacCodigo;
                lblEstadoBAC.Text = "ABIERTO";
                lblEstadoBAC.BackgroundColor = Colors.LightGreen;
                lblGrupo.Text = _currentGrupo.ToString();
                lblTablilla.Text = _currentTablilla.ToString();
                lblUnidades.Text = "24";

                // Cargar artículos de ejemplo
                _articulos.Clear();
                _articulos.Add(new ArticuloItem { Codigo = "ART001", Nombre = "Artículo de prueba 1", Cantidad = 12 });
                _articulos.Add(new ArticuloItem { Codigo = "ART002", Nombre = "Artículo de prueba 2", Cantidad = 12 });

                frameBAC.IsVisible = true;
                frameArticulos.IsVisible = true;
                frameAcciones.IsVisible = true;
                frameCaja.IsVisible = false;

                await DisplayAlert("BAC Cargado", $"BAC: {bacCodigo} cargado correctamente", "OK");
            }
            else
            {
                // TODO: Implementar lógica real de base de datos
                await DisplayAlert("Error", "Data Access Layer no implementado aún", "OK");
            }
        }

        private async Task CargarCaja(string sscc)
        {
            // TODO: Implementar consulta real cuando tengamos DAL
            if (TESTING_MODE)
            {
                // Simulación de carga de CAJA
                _currentCaja = sscc;
                _currentGrupo = 12345;
                _currentTablilla = 1;

                lblSSCC.Text = sscc;
                lblTipoCaja.Text = "CAJA01";
                lblNombreCaja.Text = "Caja Estándar 60x40";
                lblUnidadesCaja.Text = "48";
                lblPesoCaja.Text = "15.5 kg";
                lblVolumenCaja.Text = "0.25 m³";
                lblEstadoCaja.Text = "ABIERTA";
                lblEstadoCaja.BackgroundColor = Colors.LightGreen;

                // Cargar artículos de la caja
                _articulos.Clear();
                _articulos.Add(new ArticuloItem { Codigo = "ART001", Nombre = "Artículo empaquetado 1", Cantidad = 24 });
                _articulos.Add(new ArticuloItem { Codigo = "ART002", Nombre = "Artículo empaquetado 2", Cantidad = 24 });

                frameBAC.IsVisible = false;
                frameCaja.IsVisible = true;
                frameArticulos.IsVisible = true;
                frameAcciones.IsVisible = true;

                await DisplayAlert("Caja Cargada", $"SSCC: {sscc} cargada correctamente", "OK");
            }
            else
            {
                // TODO: Implementar lógica real de base de datos
                await DisplayAlert("Error", "Data Access Layer no implementado aún", "OK");
            }
        }

        private async void OnCrearCajaClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentBAC))
            {
                await DisplayAlert("Error", "Debe escanear un BAC primero", "OK");
                return;
            }

            // TODO: Implementar creación de caja real
            if (TESTING_MODE)
            {
                var nuevaSSCC = GenerarSSCC();
                
                await DisplayAlert("Caja Creada", 
                    $"Nueva caja creada:\nSSCC: {nuevaSSCC}\nGrupo: {_currentGrupo}\nTablilla: {_currentTablilla}", 
                    "OK");
                
                // Cargar la nueva caja
                await CargarCaja(nuevaSSCC);
            }
            else
            {
                await DisplayAlert("Error", "Data Access Layer no implementado aún", "OK");
            }
        }

        private async void OnEmpaquetarClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentBAC) || string.IsNullOrEmpty(_currentCaja))
            {
                await DisplayAlert("Error", "Debe escanear un BAC y una CAJA", "OK");
                return;
            }

            var confirmar = await DisplayAlert("Confirmar", 
                $"¿Empaquetar BAC {_currentBAC} en CAJA {_currentCaja}?", 
                "Sí", "No");
            
            if (!confirmar)
                return;

            // TODO: Implementar empaquetado real
            if (TESTING_MODE)
            {
                await DisplayAlert("Empaquetado", 
                    $"BAC {_currentBAC} empaquetado en CAJA {_currentCaja} correctamente", 
                    "OK");
                
                // Limpiar y volver a pantalla inicial
                InicializarPantalla();
            }
            else
            {
                await DisplayAlert("Error", "Data Access Layer no implementado aún", "OK");
            }
        }

        private async void OnCerrarBACClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentBAC))
            {
                await DisplayAlert("Error", "Debe escanear un BAC primero", "OK");
                return;
            }

            if (_currentEstadoBAC == 1)
            {
                await DisplayAlert("Aviso", "El BAC ya está cerrado", "OK");
                return;
            }

            var confirmar = await DisplayAlert("Confirmar", 
                $"¿Cerrar BAC {_currentBAC}?", 
                "Sí", "No");
            
            if (!confirmar)
                return;

            // TODO: Implementar cierre real
            if (TESTING_MODE)
            {
                _currentEstadoBAC = 1;
                lblEstadoBAC.Text = "CERRADO";
                lblEstadoBAC.BackgroundColor = Colors.Coral;
                
                await DisplayAlert("BAC Cerrado", 
                    $"BAC {_currentBAC} cerrado correctamente", 
                    "OK");
            }
            else
            {
                await DisplayAlert("Error", "Data Access Layer no implementado aún", "OK");
            }
        }

        private async void OnExtraerBACClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentBAC))
            {
                await DisplayAlert("Error", "Debe escanear un BAC primero", "OK");
                return;
            }

            var confirmar = await DisplayAlert("Confirmar", 
                $"¿Extraer BAC {_currentBAC} de su ubicación?", 
                "Sí", "No");
            
            if (!confirmar)
                return;

            // TODO: Implementar extracción real
            if (TESTING_MODE)
            {
                await DisplayAlert("BAC Extraído", 
                    $"BAC {_currentBAC} extraído correctamente", 
                    "OK");
                
                // Limpiar y volver
                InicializarPantalla();
            }
            else
            {
                await DisplayAlert("Error", "Data Access Layer no implementado aún", "OK");
            }
        }

        private async void OnImprimirEtiquetaClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCaja))
            {
                await DisplayAlert("Error", "Debe tener una CAJA seleccionada", "OK");
                return;
            }

            // TODO: Implementar impresión real con TEC/ZEBRA
            if (TESTING_MODE)
            {
                await DisplayAlert("Etiqueta", 
                    $"Enviando etiqueta a impresora...\nSSCC: {_currentCaja}\n(Simulación - Requiere impresora TEC/ZEBRA)", 
                    "OK");
            }
            else
            {
                await DisplayAlert("Error", "Impresora no configurada", "OK");
            }
        }

        private async void OnCambiarTipoCajaClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCaja))
            {
                await DisplayAlert("Error", "Debe tener una CAJA seleccionada", "OK");
                return;
            }

            // TODO: Implementar selector de tipos de caja
            var action = await DisplayActionSheet(
                "Seleccionar Tipo de Caja", 
                "Cancelar", 
                null, 
                "CAJA01 - Estándar 60x40", 
                "CAJA02 - Grande 80x60", 
                "CAJA03 - Pequeña 40x30");

            if (action != null && action != "Cancelar")
            {
                if (TESTING_MODE)
                {
                    var tipoCaja = action.Split('-')[0].Trim();
                    lblTipoCaja.Text = tipoCaja;
                    lblNombreCaja.Text = action.Split('-')[1].Trim();
                    
                    await DisplayAlert("Tipo Cambiado", 
                        $"Tipo de caja cambiado a: {tipoCaja}", 
                        "OK");
                }
            }
        }

        private async void OnCombinarCajasClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCaja))
            {
                await DisplayAlert("Error", "Debe tener una CAJA seleccionada primero", "OK");
                return;
            }

            var caja1 = _currentCaja;
            
            // Pedir segunda caja
            var caja2 = await DisplayPromptAsync("Combinar Cajas", 
                $"Primera caja: {caja1}\nIntroduzca el código de la segunda caja:", 
                "OK", "Cancelar", 
                placeholder: "SSCC segunda caja");

            if (string.IsNullOrEmpty(caja2))
                return;

            // TODO: Implementar combinación real
            if (TESTING_MODE)
            {
                await DisplayAlert("Cajas Combinadas", 
                    $"Cajas combinadas:\n{caja1} + {caja2}\n= Nueva caja unificada", 
                    "OK");
                
                // Recargar caja actualizada
                await CargarCaja(caja1);
            }
            else
            {
                await DisplayAlert("Error", "Data Access Layer no implementado aún", "OK");
            }
        }

        private async void OnSalirClicked(object? sender, EventArgs e)
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

        // Utilidades
        private string GenerarSSCC()
        {
            // Generar SSCC de prueba (18 dígitos)
            // Formato: Extension digit (1) + GS1 Company Prefix (7-10) + Serial Reference (5-8) + Check digit (1)
            var random = new Random();
            var prefix = "384"; // Código de España
            var company = "12345";
            var serial = random.Next(100000000, 999999999).ToString();
            var sscc = prefix + company + serial;
            
            // Calcular dígito de control (simplificado)
            var checkDigit = CalcularDigitoControl(sscc);
            return sscc + checkDigit;
        }

        private int CalcularDigitoControl(string codigo)
        {
            // Algoritmo módulo 10 para SSCC
            var sum = 0;
            var multiplier = 3;
            
            for (int i = codigo.Length - 1; i >= 0; i--)
            {
                sum += int.Parse(codigo[i].ToString()) * multiplier;
                multiplier = multiplier == 3 ? 1 : 3;
            }
            
            var remainder = sum % 10;
            return remainder == 0 ? 0 : 10 - remainder;
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
