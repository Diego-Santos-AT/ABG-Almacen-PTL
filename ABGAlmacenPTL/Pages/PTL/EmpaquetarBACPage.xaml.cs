using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;
using ABGAlmacenPTL.Services;
using ABGAlmacenPTL.Models;

namespace ABGAlmacenPTL.Pages.PTL
{
    public partial class EmpaquetarBACPage : ContentPage
    {
        private readonly PTLService _ptlService;

        // Constantes para generación de SSCC
        private const string SPAIN_PREFIX = "384"; // Código GS1 para España
        private const string COMPANY_CODE = "12345"; // Código de empresa (ejemplo)
        private const int SERIAL_MIN = 100000000;
        private const int SERIAL_MAX = 999999999;
        
        // Modo de prueba para simulación de impresoras
        private const bool TESTING_MODE = true; // TODO: Cambiar a false cuando se integren impresoras reales

        // Estado actual
        private string? _currentBAC;
        private string? _currentCaja;
        private string? _currentGrupo;
        private string? _currentTablilla;
        private EstadoBAC _currentEstadoBAC = EstadoBAC.Abierto;
        
        // Colecciones
        private ObservableCollection<ArticuloItem> _articulos = new();

        public EmpaquetarBACPage(PTLService ptlService)
        {
            InitializeComponent();
            _ptlService = ptlService;
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
            _currentGrupo = null;
            _currentTablilla = null;
            _currentEstadoBAC = EstadoBAC.Abierto;
            
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
            try
            {
                // Consultar BAC desde BD
                var bac = await _ptlService.GetBACByCodigoAsync(bacCodigo);
                
                if (bac == null)
                {
                    await DisplayAlertAsync("Error", "BAC no encontrado", "OK");
                    return;
                }

                _currentBAC = bacCodigo;
                _currentGrupo = bac.Grupo;
                _currentTablilla = bac.Tablilla;
                _currentEstadoBAC = bac.Estado;

                // Mostrar ubicación si tiene
                if (!string.IsNullOrEmpty(bac.CodigoUbicacion))
                {
                    var ubicacion = await _ptlService.GetUbicacionByCodigoAsync(bac.CodigoUbicacion);
                    if (ubicacion != null)
                    {
                        lblUbicacion.Text = $"{ubicacion.Almacen:000}.{ubicacion.Bloque:000}.{ubicacion.Fila:000}.{ubicacion.Altura:00}";
                    }
                    else
                    {
                        lblUbicacion.Text = "SIN UBICACIÓN";
                    }
                }
                else
                {
                    lblUbicacion.Text = "SIN UBICACIÓN";
                }

                lblBAC.Text = bacCodigo;
                lblEstadoBAC.Text = bac.Estado == EstadoBAC.Abierto ? "ABIERTO" : "CERRADO";
                lblEstadoBAC.BackgroundColor = bac.Estado == EstadoBAC.Abierto ? Colors.LightGreen : Colors.Coral;
                lblGrupo.Text = _currentGrupo ?? string.Empty;
                lblTablilla.Text = _currentTablilla ?? string.Empty;
                lblUnidades.Text = bac.Unidades.ToString();

                // Cargar artículos del BAC
                _articulos.Clear();
                var articulos = await _ptlService.GetArticulosEnBACAsync(bacCodigo);
                foreach (var articulo in articulos)
                {
                    _articulos.Add(new ArticuloItem 
                    { 
                        Codigo = articulo.CodigoArticulo, 
                        Nombre = articulo.Nombre, 
                        Cantidad = 1 // TODO: Obtener cantidad real desde BACArticulo
                    });
                }

                frameBAC.IsVisible = true;
                frameArticulos.IsVisible = true;
                frameAcciones.IsVisible = true;
                frameCaja.IsVisible = false;

                await DisplayAlertAsync("BAC Cargado", $"BAC: {bacCodigo} cargado correctamente", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Error al cargar BAC: {ex.Message}", "OK");
            }
        }

        private async Task CargarCaja(string sscc)
        {
            try
            {
                // Consultar Caja desde BD
                var caja = await _ptlService.GetCajaBySSCCAsync(sscc);
                
                if (caja == null)
                {
                    await DisplayAlertAsync("Error", "Caja no encontrada", "OK");
                    return;
                }

                _currentCaja = sscc;
                _currentGrupo = null; // TODO: Obtener desde relación si existe
                _currentTablilla = null;

                lblSSCC.Text = sscc;
                lblTipoCaja.Text = caja.TipoId.ToString(); // TODO: Cargar nombre del tipo
                lblNombreCaja.Text = "-"; // TODO: Cargar desde TipoCaja
                lblUnidadesCaja.Text = caja.Unidades.ToString();
                lblPesoCaja.Text = $"{caja.Peso:F2} kg";
                lblVolumenCaja.Text = $"{caja.Volumen:F2} m³";
                lblEstadoCaja.Text = caja.Estado == EstadoCaja.Abierta ? "ABIERTA" : "CERRADA";
                lblEstadoCaja.BackgroundColor = caja.Estado == EstadoCaja.Abierta ? Colors.LightGreen : Colors.Coral;

                // Cargar artículos de la caja
                _articulos.Clear();
                var articulos = await _ptlService.GetArticulosEnCajaAsync(sscc);
                foreach (var articulo in articulos)
                {
                    _articulos.Add(new ArticuloItem 
                    { 
                        Codigo = articulo.CodigoArticulo, 
                        Nombre = articulo.Nombre, 
                        Cantidad = 1 // TODO: Obtener cantidad real desde CajaArticulo
                    });
                }

                frameBAC.IsVisible = false;
                frameCaja.IsVisible = true;
                frameArticulos.IsVisible = true;
                frameAcciones.IsVisible = true;

                await DisplayAlertAsync("Caja Cargada", $"SSCC: {sscc} cargada correctamente", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Error al cargar caja: {ex.Message}", "OK");
            }
        }

        private async void OnCrearCajaClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentBAC))
            {
                await DisplayAlertAsync("Error", "Debe escanear un BAC primero", "OK");
                return;
            }

            try
            {
                // Crear nueva caja en la base de datos
                // TODO: Permitir seleccionar tipo de caja
                int tipoId = 1; // Por defecto, tipo 1
                
                var nuevaSSCC = await _ptlService.CrearNuevaCajaAsync(tipoId);
                
                await DisplayAlertAsync("Caja Creada", 
                    $"Nueva caja creada:\nSSCC: {nuevaSSCC}\nGrupo: {_currentGrupo}\nTablilla: {_currentTablilla}", 
                    "OK");
                
                // Cargar la nueva caja
                await CargarCaja(nuevaSSCC);
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Error al crear caja: {ex.Message}", "OK");
            }
        }

        private async void OnEmpaquetarClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentBAC) || string.IsNullOrEmpty(_currentCaja))
            {
                await DisplayAlertAsync("Error", "Debe escanear un BAC y una CAJA", "OK");
                return;
            }

            var confirmar = await DisplayAlertAsync("Confirmar", 
                $"¿Empaquetar BAC {_currentBAC} en CAJA {_currentCaja}?", 
                "Sí", "No");
            
            if (!confirmar)
                return;

            try
            {
                // Empaquetar BAC en Caja (transacción completa)
                bool success = await _ptlService.EmpaquetarBACEnCajaAsync(_currentBAC, _currentCaja);
                
                if (success)
                {
                    await DisplayAlertAsync("Empaquetado", 
                        $"BAC {_currentBAC} empaquetado en CAJA {_currentCaja} correctamente", 
                        "OK");
                    
                    // Limpiar y volver a pantalla inicial
                    InicializarPantalla();
                }
                else
                {
                    await DisplayAlertAsync("Error", "No se pudo empaquetar el BAC", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Error al empaquetar: {ex.Message}", "OK");
            }
        }

        private async void OnCerrarBACClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentBAC))
            {
                await DisplayAlertAsync("Error", "Debe escanear un BAC primero", "OK");
                return;
            }

            if (_currentEstadoBAC == EstadoBAC.Cerrado)
            {
                await DisplayAlertAsync("Aviso", "El BAC ya está cerrado", "OK");
                return;
            }

            var confirmar = await DisplayAlertAsync("Confirmar", 
                $"¿Cerrar BAC {_currentBAC}?", 
                "Sí", "No");
            
            if (!confirmar)
                return;

            try
            {
                // Cerrar BAC (actualizar estado en BD)
                var bac = await _ptlService.GetBACByCodigoAsync(_currentBAC);
                if (bac != null)
                {
                    bac.Estado = EstadoBAC.Cerrado;
                    bac.FechaModificacion = DateTime.Now;
                    // TODO: Actualizar en BD via Unit of Work
                    
                    _currentEstadoBAC = EstadoBAC.Cerrado;
                    lblEstadoBAC.Text = "CERRADO";
                    lblEstadoBAC.BackgroundColor = Colors.Coral;
                    
                    await DisplayAlertAsync("BAC Cerrado", 
                        $"BAC {_currentBAC} cerrado correctamente", 
                        "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Error al cerrar BAC: {ex.Message}", "OK");
            }
        }

        private async void OnExtraerBACClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentBAC))
            {
                await DisplayAlertAsync("Error", "Debe escanear un BAC primero", "OK");
                return;
            }

            var confirmar = await DisplayAlertAsync("Confirmar", 
                $"¿Extraer BAC {_currentBAC} de su ubicación?", 
                "Sí", "No");
            
            if (!confirmar)
                return;

            try
            {
                // Obtener BAC para saber su ubicación
                var bac = await _ptlService.GetBACByCodigoAsync(_currentBAC);
                
                if (bac != null && !string.IsNullOrEmpty(bac.CodigoUbicacion))
                {
                    // Extraer BAC (quitar de ubicación)
                    bool success = await _ptlService.ExtraerBACDeUbicacionAsync(bac.CodigoUbicacion, EstadoBAC.Cerrado);
                    
                    if (success)
                    {
                        await DisplayAlertAsync("BAC Extraído", 
                            $"BAC {_currentBAC} extraído correctamente", 
                            "OK");
                        
                        // Limpiar y volver
                        InicializarPantalla();
                    }
                    else
                    {
                        await DisplayAlertAsync("Error", "No se pudo extraer el BAC", "OK");
                    }
                }
                else
                {
                    await DisplayAlertAsync("Aviso", "El BAC no tiene ubicación asignada", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", $"Error al extraer BAC: {ex.Message}", "OK");
            }
        }

        private async void OnImprimirEtiquetaClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCaja))
            {
                await DisplayAlertAsync("Error", "Debe tener una CAJA seleccionada", "OK");
                return;
            }

            // TODO: Implementar impresión real con TEC/ZEBRA
            if (TESTING_MODE)
            {
                await DisplayAlertAsync("Etiqueta", 
                    $"Enviando etiqueta a impresora...\nSSCC: {_currentCaja}\n(Simulación - Requiere impresora TEC/ZEBRA)", 
                    "OK");
            }
            // else
            // {
            //     await DisplayAlertAsync("Error", "Impresora no configurada", "OK");
            // }
        }

        private async void OnCambiarTipoCajaClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCaja))
            {
                await DisplayAlertAsync("Error", "Debe tener una CAJA seleccionada", "OK");
                return;
            }

            // TODO: Implementar selector de tipos de caja
            var action = await DisplayActionSheetAsync(
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
                    
                    await DisplayAlertAsync("Tipo Cambiado", 
                        $"Tipo de caja cambiado a: {tipoCaja}", 
                        "OK");
                }
            }
        }

        private async void OnCombinarCajasClicked(object? sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentCaja))
            {
                await DisplayAlertAsync("Error", "Debe tener una CAJA seleccionada primero", "OK");
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
                await DisplayAlertAsync("Cajas Combinadas", 
                    $"Cajas combinadas:\n{caja1} + {caja2}\n= Nueva caja unificada", 
                    "OK");
                
                // Recargar caja actualizada
                await CargarCaja(caja1);
            }
            // else
            // {
            //     await DisplayAlertAsync("Error", "Data Access Layer no implementado aún", "OK");
            // }
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
            var serial = random.Next(SERIAL_MIN, SERIAL_MAX).ToString();
            var sscc = SPAIN_PREFIX + COMPANY_CODE + serial;
            
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
}
