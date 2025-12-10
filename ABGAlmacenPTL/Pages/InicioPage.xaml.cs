using ABGAlmacenPTL.Modules;
using ABGAlmacenPTL.Configuration;
using ABGAlmacenPTL.Services;
using ABGAlmacenPTL.Models.Config;

namespace ABGAlmacenPTL.Pages
{
    /// <summary>
    /// Form de pantalla de Inicio de la aplicación
    /// Validación de Usuario contra Config DB
    /// Selección de Empresa
    /// Migrado fielmente desde VB6 frmInicio.frm
    /// </summary>
    public partial class InicioPage : ContentPage
    {
        private readonly AuthService _authService;
        private readonly ABGConfigService _abgConfig;
        private int reintentos = 0;
        private List<Empresa> _empresasDisponibles = new List<Empresa>();
        private List<Models.Config.PuestoTrabajo> _puestosDisponibles = new List<Models.Config.PuestoTrabajo>();

        public InicioPage(AuthService authService, ABGConfigService abgConfig)
        {
            InitializeComponent();
            
            _authService = authService;
            _abgConfig = abgConfig;
            
            // Configurar versión y comentarios
            lblVersion.Text = "Versión 23.4.2";
            lblComentarios.Text = "Fecha Versión: 27/04/2023";
            
            // Inicializar
            reintentos = 0;
            Gestion.LoginSucceeded = false;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            
            lblEstado.Text = $"Conectando con el Servidor {_abgConfig.BDDServLocal}...";
            
            // Cargar usuario por defecto desde abg.ini
            txtUsuario.Text = _abgConfig.UsrDefault;
            
            // Cargar puestos de trabajo (desde tabla o hardcoded como VB6)
            await CargarPuestosAsync();
            
            lblEstado.Text = "Listo para iniciar sesión";
        }

        private async Task CargarPuestosAsync()
        {
            try
            {
                // Cargar puestos desde la base de datos (como VB6: edC.DamePuestos)
                _puestosDisponibles = await _authService.ObtenerPuestosAsync();
                
                pickerPuesto.Items.Clear();
                
                foreach (var puesto in _puestosDisponibles)
                {
                    pickerPuesto.Items.Add(puesto.DescripcionCorta ?? "");
                }
                
                // Seleccionar puesto por defecto desde abg.ini (como VB6: CargaPuestos)
                int puestoIndex = -1;
                if (_abgConfig.PueDefault > 0)
                {
                    puestoIndex = _puestosDisponibles.FindIndex(p => p.CodigoPuesto == _abgConfig.PueDefault);
                }
                
                if (puestoIndex >= 0)
                {
                    pickerPuesto.SelectedIndex = puestoIndex;
                }
                else if (_puestosDisponibles.Count > 0)
                {
                    pickerPuesto.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                // Si falla la carga desde BD, usar valores por defecto
                System.Diagnostics.Debug.WriteLine($"Error cargando puestos: {ex.Message}");
                pickerPuesto.Items.Clear();
                pickerPuesto.Items.Add("Puesto 1");
                pickerPuesto.SelectedIndex = 0;
            }
        }

        private async Task ValidarUsuarioAsync()
        {
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un usuario", "OK");
                txtUsuario.Focus();
                return;
            }
            
            lblEstado.Text = "Validando usuario...";
            
            try
            {
                // Buscar usuario en Config DB (VB6: edC.BuscaUsuario)
                var usuario = await _authService.BuscarUsuarioAsync(txtUsuario.Text);
                
                if (usuario == null)
                {
                    await DisplayAlert("Error", "Usuario no encontrado", "OK");
                    txtUsuario.Text = string.Empty;
                    txtUsuario.Focus();
                    lblEstado.Text = "Usuario inválido";
                    return;
                }
                
                // Cargar empresas del usuario (VB6: edC.DameEmpresasAccesoUsuario)
                _empresasDisponibles = await _authService.ObtenerEmpresasUsuarioAsync(usuario.UsuarioId);
                
                if (_empresasDisponibles.Count == 0)
                {
                    await DisplayAlert("Error", 
                        "No tiene asignada empresa actualmente.\nConsulte con el dpto. de informática.", 
                        "OK");
                    lblEstado.Text = "Sin empresas asignadas";
                    return;
                }
                
                // Mostrar empresas en el picker (VB6: CargaEmpresas)
                pickerEmpresa.Items.Clear();
                foreach (var empresa in _empresasDisponibles)
                {
                    // Solo mostrar el nombre, como en VB6: ComboEmpresa.AddItem edC.rsDameParametrosEmpresa!empnom
                    pickerEmpresa.Items.Add(empresa.NombreEmpresa);
                }
                
                // Seleccionar empresa por defecto si existe en abg.ini
                int empresaIndex = -1;
                if (!string.IsNullOrEmpty(_abgConfig.CodEmpresa))
                {
                    empresaIndex = _empresasDisponibles.FindIndex(e => e.CodigoEmpresa.ToString() == _abgConfig.CodEmpresa);
                }
                
                if (empresaIndex >= 0)
                {
                    pickerEmpresa.SelectedIndex = empresaIndex;
                }
                else if (_empresasDisponibles.Count > 0)
                {
                    pickerEmpresa.SelectedIndex = 0;
                }
                
                // Verificar si tiene contraseña (VB6: HayPassword)
                if (string.IsNullOrEmpty(usuario.Contraseña))
                {
                    // Sin contraseña - ocultar campo de contraseña pero no validar todavía
                    txtPassword.IsVisible = false;
                    lblEstado.Text = "Usuario validado (sin contraseña)";
                }
                else
                {
                    // Con contraseña - mostrar campo
                    txtPassword.IsVisible = true;
                    txtPassword.Focus();
                    lblEstado.Text = "Ingrese contraseña";
                }
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error de conexión";
                await DisplayAlert("Error", 
                    $"Error al conectar con el servidor Config: {ex.Message}", 
                    "OK");
            }
        }

        private async void OnAceptarClicked(object sender, EventArgs e)
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                await DisplayAlert("Error", "Debe ingresar un usuario", "OK");
                txtUsuario.Focus();
                return;
            }

            if (pickerEmpresa.SelectedIndex < 0)
            {
                // Si no hay empresas cargadas, validar usuario primero
                await ValidarUsuarioAsync();
                return;
            }

            if (pickerPuesto.SelectedIndex < 0)
            {
                await DisplayAlert("Error", "Debe seleccionar un puesto", "OK");
                return;
            }

            lblEstado.Text = "Iniciando sesión...";
            
            try
            {
                // Validar credenciales (VB6: ValidaContraseña)
                var (exito, mensaje) = await _authService.ValidarCredencialesAsync(
                    txtUsuario.Text, 
                    txtPassword.IsVisible ? txtPassword.Text : null);

                if (!exito)
                {
                    reintentos++;
                    
                    if (reintentos >= 3)
                    {
                        await DisplayAlert("Error", 
                            "Ha excedido el número de intentos permitidos. La aplicación se cerrará.", 
                            "OK");
                        Application.Current?.Quit();
                    }
                    else
                    {
                        await DisplayAlert("Error", 
                            $"{mensaje}\nIntentos restantes: {3 - reintentos}", 
                            "OK");
                        txtPassword.Text = string.Empty;
                        txtPassword.Focus();
                    }
                    
                    lblEstado.Text = "Error de autenticación";
                    return;
                }
                
                // Seleccionar empresa (VB6: ConfiguracionEmpresa)
                var empresaSeleccionada = _empresasDisponibles[pickerEmpresa.SelectedIndex];
                _authService.SeleccionarEmpresa(empresaSeleccionada);
                
                // Guardar puesto de trabajo (VB6: ComboPuesto.Text y DameCodigoPuesto)
                if (_puestosDisponibles.Count > 0 && pickerPuesto.SelectedIndex >= 0)
                {
                    var puestoSeleccionado = _puestosDisponibles[pickerPuesto.SelectedIndex];
                    
                    // Cargar datos del puesto (VB6: DamePuestoTrabajo)
                    Gestion.wPuestoTrabajo.Id = puestoSeleccionado.CodigoPuesto;
                    Gestion.wPuestoTrabajo.Nombre = puestoSeleccionado.Descripcion;
                    Gestion.wPuestoTrabajo.NombreCorto = puestoSeleccionado.DescripcionCorta ?? "";
                    Gestion.wPuestoTrabajo.Impresora = puestoSeleccionado.CodigoImpresora ?? 0;
                    
                    if (puestoSeleccionado.Impresora != null)
                    {
                        Gestion.wPuestoTrabajo.NombreImpresora = puestoSeleccionado.Impresora.Nombre;
                        Gestion.wPuestoTrabajo.TipoImpresora = puestoSeleccionado.Impresora.Lenguaje ?? "";
                    }
                    
                    // Guardar en abg.ini (VB6: GuardarIni ficINI, "Varios", "PueDefault")
                    ProfileManager.GuardarIni(
                        Path.Combine(FileSystem.AppDataDirectory, "abg.ini"),
                        "Varios",
                        "PueDefault",
                        puestoSeleccionado.CodigoPuesto.ToString());
                }
                else
                {
                    // Valores por defecto si no hay puesto seleccionado
                    Gestion.wPuestoTrabajo.Id = 1;
                    Gestion.wPuestoTrabajo.Nombre = "";
                    Gestion.wPuestoTrabajo.NombreCorto = "";
                    Gestion.wPuestoTrabajo.Impresora = 0;
                    Gestion.wPuestoTrabajo.NombreImpresora = "";
                    Gestion.wPuestoTrabajo.TipoImpresora = "";
                }
                
                // Impresora asociada al puesto de trabajo (VB6: wImpresora = wPuestoTrabajo.NombreImpresora)
                Gestion.wImpresora = Gestion.wPuestoTrabajo.NombreImpresora;
                
                lblEstado.Text = "Verificando conexión a base de datos...";
                
                // Verificar conexión a GestionAlmacen DB (VB6: ConexionGestionAlmacen)
                try
                {
                    var dbManager = Handler?.MauiContext?.Services.GetService<DatabaseConnectionManager>();
                    if (dbManager != null)
                    {
                        bool conexionOk = await dbManager.VerificarConexionGestionAlmacenAsync();
                        if (!conexionOk)
                        {
                            await DisplayAlert("Advertencia", 
                                $"No se pudo conectar a la base de datos GestionAlmacen.\n" +
                                $"Empresa: {empresaSeleccionada.NombreEmpresa}\n" +
                                $"Servidor: {empresaSeleccionada.ServidorGA}\n" +
                                $"BD: {empresaSeleccionada.BaseDatosGA}", 
                                "OK");
                        }
                    }
                }
                catch (Exception exDB)
                {
                    await DisplayAlert("Advertencia", 
                        $"Error al verificar conexión a base de datos: {exDB.Message}", 
                        "OK");
                }
                
                // Login exitoso
                Gestion.LoginSucceeded = true;
                Gestion.Usuario.Id = _authService.UsuarioActual!.UsuarioId;
                Gestion.Usuario.Nombre = _authService.UsuarioActual.NombreUsuario;
                Gestion.CodEmpresa = empresaSeleccionada.CodigoEmpresa;
                Gestion.Empresa = empresaSeleccionada.NombreEmpresa;
                
                lblEstado.Text = $"Conexión exitosa - Empresa: {empresaSeleccionada.NombreEmpresa}";
                
                // Navegar al menú principal
                await Task.Delay(500); // Breve delay para mostrar mensaje
                await Shell.Current.GoToAsync("//MenuPage");
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error de conexión";
                await DisplayAlert("Error", 
                    $"Error al iniciar sesión: {ex.Message}", 
                    "OK");
            }
        }

        private void OnCancelarClicked(object sender, EventArgs e)
        {
            // Salir de la aplicación
            Gestion.LoginSucceeded = false;
            Application.Current?.Quit();
        }

        /// <summary>
        /// Cuando el usuario termina de escribir su nombre, validar contra Config DB
        /// Migrado desde VB6: txtUsuarios_LostFocus → ValidaUsuario
        /// </summary>
        private async void txtUsuario_Completed(object sender, EventArgs e)
        {
            await ValidarUsuarioAsync();
        }
    }
}
