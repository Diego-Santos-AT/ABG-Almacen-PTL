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
            // Puestos de trabajo (como VB6)
            pickerPuesto.Items.Clear();
            pickerPuesto.Items.Add("Puesto 1");
            pickerPuesto.Items.Add("Puesto 2");
            pickerPuesto.Items.Add("Puesto 3");
            pickerPuesto.Items.Add("Puesto 4");
            pickerPuesto.Items.Add("Puesto 5");
            
            // Seleccionar puesto por defecto desde abg.ini
            if (_abgConfig.PueDefault > 0 && _abgConfig.PueDefault <= pickerPuesto.Items.Count)
            {
                pickerPuesto.SelectedIndex = _abgConfig.PueDefault - 1;
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
                
                // Mostrar empresas en el picker
                pickerEmpresa.Items.Clear();
                foreach (var empresa in _empresasDisponibles)
                {
                    pickerEmpresa.Items.Add($"{empresa.CodigoEmpresa} - {empresa.NombreEmpresa}");
                }
                
                // Seleccionar empresa por defecto si existe en abg.ini
                if (!string.IsNullOrEmpty(_abgConfig.CodEmpresa))
                {
                    var empDefault = _empresasDisponibles.FindIndex(e => e.CodigoEmpresa.ToString() == _abgConfig.CodEmpresa);
                    if (empDefault >= 0)
                    {
                        pickerEmpresa.SelectedIndex = empDefault;
                    }
                }
                
                // Verificar si tiene contraseña (VB6: HayPassword)
                if (string.IsNullOrEmpty(usuario.Contraseña))
                {
                    // Sin contraseña - login directo
                    txtPassword.IsVisible = false;
                    var (exito, mensaje) = await _authService.ValidarCredencialesAsync(txtUsuario.Text, null);
                    if (exito)
                    {
                        // Continuar sin pedir contraseña
                        lblEstado.Text = "Usuario validado";
                    }
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
                
                // Guardar puesto de trabajo
                Gestion.wPuestoTrabajo.Id = pickerPuesto.SelectedIndex + 1;
                
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
