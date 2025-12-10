using ABGAlmacenPTL.Modules;
using ABGAlmacenPTL.Configuration;

namespace ABGAlmacenPTL.Pages
{
    /// <summary>
    /// Form de pantalla de Inicio de la aplicación
    /// Muestra el nombre de programa, versión, etc
    /// Control de la conexión con BD
    /// Validación de Usuario
    /// Migrado desde VB6 frmInicio.frm
    /// Creado: 04/04/01
    /// </summary>
    public partial class InicioPage : ContentPage
    {
        private int reintentos = 0;
        private const int CMD_Aceptar = 0;
        private const int CMD_Cancelar = 1;

        public InicioPage()
        {
            InitializeComponent();
            
            // Configurar versión y comentarios
            lblVersion.Text = "Versión 23.4.2";
            lblComentarios.Text = "Fecha Versión: 27/04/2023";
            
            // Inicializar
            reintentos = 0;
            Gestion.LoginSucceeded = false;
            
            // Cargar datos iniciales
            CargarDatosIniciales();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            lblEstado.Text = $"Conectando con el Servidor {Gestion.BDDServLocal}...";
            
            // Cargar usuario por defecto si existe
            if (!string.IsNullOrEmpty(Gestion.UsrDefault))
            {
                txtUsuario.Text = Gestion.UsrDefault;
            }
            
            lblEstado.Text = "Iniciando...";
        }

        private void CargarDatosIniciales()
        {
            // Cargar lista de empresas
            // TODO: Implementar carga desde base de datos cuando esté el Data Access Layer
            var empresas = new List<string>
            {
                "ATOSA",
                "BOYSTOYS",
                "GIEPOOL",
                "DISTRIELITE"
            };
            
            foreach (var empresa in empresas)
            {
                pickerEmpresa.Items.Add(empresa);
            }
            
            // Cargar lista de puestos
            // TODO: Implementar carga desde base de datos
            var puestos = new List<string>
            {
                "Puesto 1",
                "Puesto 2",
                "Puesto 3",
                "Puesto 4"
            };
            
            foreach (var puesto in puestos)
            {
                pickerPuesto.Items.Add(puesto);
            }
            
            // Seleccionar valores por defecto
            if (!string.IsNullOrEmpty(Gestion.CodEmpresa?.ToString()))
            {
                // Seleccionar empresa por defecto
                pickerEmpresa.SelectedIndex = 0;
            }
            
            if (Gestion.wPuestoTrabajo.Id > 0)
            {
                // Seleccionar puesto por defecto
                pickerPuesto.SelectedIndex = Gestion.wPuestoTrabajo.Id - 1;
            }
        }

        private async void OnAceptarClicked(object sender, EventArgs e)
        {
            await Accion(CMD_Aceptar);
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Accion(CMD_Cancelar);
        }

        private async Task Accion(int index)
        {
            switch (index)
            {
                case CMD_Aceptar:
                    await ValidarYConectar();
                    break;
                    
                case CMD_Cancelar:
                    // Salir de la aplicación
                    Gestion.LoginSucceeded = false;
                    Application.Current?.Quit();
                    break;
            }
        }

        private async Task ValidarYConectar()
        {
            // Validar campos
            if (string.IsNullOrWhiteSpace(txtUsuario.Text))
            {
                await DisplayAlertAsync("Error", "Debe ingresar un usuario", "OK");
                txtUsuario.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                await DisplayAlertAsync("Error", "Debe ingresar una contraseña", "OK");
                txtPassword.Focus();
                return;
            }

            if (pickerEmpresa.SelectedIndex < 0)
            {
                await DisplayAlertAsync("Error", "Debe seleccionar una empresa", "OK");
                return;
            }

            if (pickerPuesto.SelectedIndex < 0)
            {
                await DisplayAlertAsync("Error", "Debe seleccionar un puesto", "OK");
                return;
            }

            lblEstado.Text = "Validando usuario...";
            
            try
            {
                // TODO: Implementar validación real contra base de datos
                // Por ahora, validación básica de ejemplo
                bool usuarioValido = await ValidarUsuario(
                    txtUsuario.Text, 
                    txtPassword.Text);

                if (usuarioValido)
                {
                    // Usuario válido
                    Gestion.LoginSucceeded = true;
                    Gestion.Usuario.Nombre = txtUsuario.Text;
                    
                    // Guardar valores en INI
                    GuardarPreferencias();
                    
                    lblEstado.Text = "Conexión exitosa";
                    
                    // Navegar al menú principal
                    await Shell.Current.GoToAsync("//MenuPage");
                }
                else
                {
                    reintentos++;
                    
                    if (reintentos >= 3)
                    {
                        await DisplayAlertAsync("Error", 
                            "Ha excedido el número de intentos permitidos. La aplicación se cerrará.", 
                            "OK");
                        Application.Current?.Quit();
                    }
                    else
                    {
                        await DisplayAlertAsync("Error", 
                            $"Usuario o contraseña incorrectos. Intentos restantes: {3 - reintentos}", 
                            "OK");
                        txtPassword.Text = string.Empty;
                        txtPassword.Focus();
                    }
                    
                    lblEstado.Text = "Error de autenticación";
                }
            }
            catch (Exception ex)
            {
                lblEstado.Text = "Error de conexión";
                await DisplayAlertAsync("Error", 
                    $"Error al conectar con el servidor: {ex.Message}", 
                    "OK");
            }
        }

        private async Task<bool> ValidarUsuario(string usuario, string password)
        {
            // TODO: Implementar validación real contra base de datos
            // Por ahora, simulamos un delay y aceptamos cualquier usuario
            await Task.Delay(500);
            
            // Validación temporal - aceptar cualquier usuario para desarrollo
            return !string.IsNullOrEmpty(usuario) && !string.IsNullOrEmpty(password);
        }

        private void GuardarPreferencias()
        {
            // Guardar usuario y empresa por defecto en INI
            if (!string.IsNullOrEmpty(Gestion.ficINI))
            {
                ProfileManager.GuardarIni(Gestion.ficINI, "Varios", "UsrDefault", txtUsuario.Text);
                
                if (pickerEmpresa.SelectedIndex >= 0)
                {
                    // Guardar código de empresa
                    ProfileManager.GuardarIni(Gestion.ficINI, "Varios", "EmpDefault", 
                        (pickerEmpresa.SelectedIndex + 1).ToString());
                }
                
                if (pickerPuesto.SelectedIndex >= 0)
                {
                    // Guardar puesto
                    ProfileManager.GuardarIni(Gestion.ficINI, "Varios", "PueDefault", 
                        (pickerPuesto.SelectedIndex + 1).ToString());
                }
            }
        }

        private void RegistrarVersion()
        {
            // Registrar versión en archivo INI local
            #if WINDOWS
            string ficheroIniLocal = "C:\\Archivos de programa\\ABG\\abg.ini";
            if (System.IO.Directory.Exists("C:\\Archivos de programa\\ABG\\"))
            {
                ProfileManager.GuardarIni(ficheroIniLocal, "Versiones", "ABG", "23.4.2");
            }
            #endif
        }
    }
}
