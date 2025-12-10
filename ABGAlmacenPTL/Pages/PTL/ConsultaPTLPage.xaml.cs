using System.Collections.ObjectModel;
using ABGAlmacenPTL.Services;
using ABGAlmacenPTL.Models;
using System.Linq;

namespace ABGAlmacenPTL.Pages.PTL
{
    /// <summary>
    /// Formulario de consultas PTL
    /// Migrado desde VB6 frmConsultaPTL.frm
    /// </summary>
    public partial class ConsultaPTLPage : ContentPage
    {
        private readonly PTLService _ptlService;

        private ObservableCollection<ArticuloItem> _articulos = new();

        public ConsultaPTLPage(PTLService ptlService)
        {
            InitializeComponent();
            _ptlService = ptlService;
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
            try
            {
                // Validar longitud antes de parsear
                if (ubicacionCodigo.Length != 12)
                {
                    await DisplayAlert("Error", "Código de ubicación debe ser de 12 dígitos", "OK");
                    return;
                }

                // Consultar ubicación en BD
                var ubicacion = await _ptlService.GetUbicacionByCodigoAsync(ubicacionCodigo);
                
                if (ubicacion == null)
                {
                    await DisplayAlert("Error", "No existe la Ubicación", "OK");
                    LimpiarDatos();
                    return;
                }

                // Buscar BAC en la ubicación
                var bac = await _ptlService.GetBACEnUbicacionAsync(ubicacionCodigo);
                
                if (bac != null)
                {
                    // Mostrar datos de la ubicación y BAC
                    RefrescarDatos(
                        ubicacionId: int.Parse(ubicacion.CodigoUbicacion),
                        alm: ubicacion.Almacen, 
                        blo: ubicacion.Bloque, 
                        fil: ubicacion.Fila, 
                        alt: ubicacion.Altura,
                        bac: bac.CodigoBAC,
                        estadoBAC: (int)bac.Estado,
                        grupo: bac.Grupo,
                        tablilla: bac.Tablilla,
                        numCaja: "N/A", // TODO: Relación BAC->Caja cuando esté disponible
                        peso: bac.Peso,
                        volumen: bac.Volumen,
                        tipoCaja: "-",
                        nombreCaja: "-");

                    // Cargar artículos del BAC
                    await CargarArticulosBAC(bac.CodigoBAC);
                }
                else
                {
                    // Ubicación existe pero sin BAC
                    RefrescarDatos(
                        ubicacionId: int.Parse(ubicacion.CodigoUbicacion),
                        alm: ubicacion.Almacen,
                        blo: ubicacion.Bloque,
                        fil: ubicacion.Fila,
                        alt: ubicacion.Altura,
                        bac: "SIN BAC",
                        estadoBAC: 1,
                        grupo: 0,
                        tablilla: 0,
                        numCaja: "-",
                        peso: 0,
                        volumen: 0,
                        tipoCaja: "-",
                        nombreCaja: "-");
                    
                    _articulos.Clear();
                    lblUds.Text = "0";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al consultar ubicación: {ex.Message}", "OK");
                LimpiarDatos();
            }
        }

        private async Task ConsultarBAC(string bacCodigo)
        {
            try
            {
                // Consultar BAC en BD
                var bac = await _ptlService.GetBACByCodigoAsync(bacCodigo);
                
                if (bac == null)
                {
                    await DisplayAlert("Error", "No existe el BAC", "OK");
                    LimpiarDatos();
                    return;
                }

                // Si el BAC tiene ubicación asignada, mostrarla
                int ubicacionId = 0;
                int alm = 0, blo = 0, fil = 0, alt = 0;
                
                if (!string.IsNullOrEmpty(bac.CodigoUbicacion))
                {
                    var ubicacion = await _ptlService.GetUbicacionByCodigoAsync(bac.CodigoUbicacion);
                    if (ubicacion != null)
                    {
                        ubicacionId = int.Parse(ubicacion.CodigoUbicacion);
                        alm = ubicacion.Almacen;
                        blo = ubicacion.Bloque;
                        fil = ubicacion.Fila;
                        alt = ubicacion.Altura;
                    }
                }

                // Mostrar datos del BAC
                RefrescarDatos(
                    ubicacionId: ubicacionId,
                    alm: alm, blo: blo, fil: fil, alt: alt,
                    bac: bac.CodigoBAC,
                    estadoBAC: (int)bac.Estado,
                    grupo: bac.Grupo,
                    tablilla: bac.Tablilla,
                    numCaja: "N/A", // TODO: Relación BAC->Caja cuando esté disponible
                    peso: bac.Peso,
                    volumen: bac.Volumen,
                    tipoCaja: "-",
                    nombreCaja: "-");

                // Cargar artículos del BAC
                await CargarArticulosBAC(bac.CodigoBAC);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al consultar BAC: {ex.Message}", "OK");
                LimpiarDatos();
            }
        }

        private async Task ConsultarCaja(string cajaCodigo)
        {
            try
            {
                // Consultar Caja en BD (por SSCC)
                var caja = await _ptlService.GetCajaBySSCCAsync(cajaCodigo);
                
                if (caja == null)
                {
                    await DisplayAlert("Error", "No existe la CAJA", "OK");
                    LimpiarDatos();
                    return;
                }

                // Mostrar datos de la caja
                RefrescarDatos(
                    ubicacionId: 0, // Sin ubicación específica
                    alm: 0, blo: 0, fil: 0, alt: 0,
                    bac: "N/A",
                    estadoBAC: 1,
                    grupo: 0,
                    tablilla: 0,
                    numCaja: caja.SSCC,
                    peso: caja.Peso,
                    volumen: caja.Volumen,
                    tipoCaja: caja.TipoId.ToString(), // TODO: Cargar nombre del tipo de caja
                    nombreCaja: "-"); // TODO: Cargar nombre del tipo de caja desde relación

                // Cargar artículos de la caja
                await CargarArticulosCaja(caja.SSCC);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al consultar caja: {ex.Message}", "OK");
                LimpiarDatos();
            }
        }

        private async Task CargarArticulosBAC(string bacCodigo)
        {
            try
            {
                _articulos.Clear();

                // Consultar artículos del BAC desde BD
                var articulos = await _ptlService.GetArticulosEnBACAsync(bacCodigo);
                
                // TODO: Obtener cantidades desde tabla de unión BACArticulo
                foreach (var articulo in articulos)
                {
                    _articulos.Add(new ArticuloItem 
                    { 
                        Codigo = articulo.CodigoArticulo, 
                        Nombre = articulo.NombreArticulo, 
                        Cantidad = 1 // TODO: Obtener cantidad real desde BACArticulo
                    });
                }

                // Actualizar contador de unidades
                int totalUds = _articulos.Sum(a => a.Cantidad);
                lblUds.Text = totalUds.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar artículos del BAC: {ex.Message}", "OK");
            }
        }

        private async Task CargarArticulosCaja(string sscc)
        {
            try
            {
                _articulos.Clear();

                // Consultar artículos de la caja desde BD
                var articulos = await _ptlService.GetArticulosEnCajaAsync(sscc);
                
                // TODO: Obtener cantidades desde tabla de unión CajaArticulo
                foreach (var articulo in articulos)
                {
                    _articulos.Add(new ArticuloItem 
                    { 
                        Codigo = articulo.CodigoArticulo, 
                        Nombre = articulo.NombreArticulo, 
                        Cantidad = 1 // TODO: Obtener cantidad real desde CajaArticulo
                    });
                }

                // Actualizar contador de unidades
                int totalUds = _articulos.Sum(a => a.Cantidad);
                lblUds.Text = totalUds.ToString();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar artículos de la caja: {ex.Message}", "OK");
            }
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
