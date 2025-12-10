using ABGAlmacenPTL.Modules;

namespace ABGAlmacenPTL.Pages
{
    /// <summary>
    /// Menú Principal de la aplicación PTL
    /// Migrado desde VB6: frmMenu.frm
    /// </summary>
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Mostrar información del usuario y empresa
            Title = $"Menú Principal - {Gestion.Empresa}";
        }

        /// <summary>
        /// Navegar a Consultas PTL
        /// Migrado desde VB6: cmdAccionMenu_Click(0)
        /// </summary>
        private async void OnConsultasPTLClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ConsultaPTLPage");
        }

        /// <summary>
        /// Navegar a Ubicar BAC
        /// Migrado desde VB6: cmdAccionMenu_Click(1)
        /// </summary>
        private async void OnUbicarBACClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("UbicarBACPage");
        }

        /// <summary>
        /// Navegar a Extraer BAC
        /// Migrado desde VB6: cmdAccionMenu_Click(2)
        /// </summary>
        private async void OnExtraerBACClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ExtraerBACPage");
        }

        /// <summary>
        /// Navegar a Reparto de Artículos
        /// Migrado desde VB6: cmdAccionMenu_Click(3)
        /// </summary>
        private async void OnRepartoClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("RepartirArticuloPage");
        }

        /// <summary>
        /// Navegar a Empaquetado BAC
        /// Migrado desde VB6: cmdAccionMenu_Click(4)
        /// </summary>
        private async void OnEmpaquetadoClicked(object sender, EventArgs e)
        {
            // TODO: Implementar navegación cuando la página esté creada
            // await Shell.Current.GoToAsync("//EmpaquetarBACPage");
            await DisplayAlert("Información", "Empaquetado - En desarrollo", "OK");
        }

        /// <summary>
        /// Salir de la aplicación
        /// Migrado desde VB6: cmdAccionMenu_Click(5)
        /// </summary>
        private async void OnSalirClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert(
                "Salir", 
                "¿Desea salir de la aplicación?", 
                "Sí", 
                "No");

            if (confirm)
            {
                // Cerrar la aplicación
                Application.Current?.Quit();
            }
        }

        /// <summary>
        /// Manejo de teclas (KeyPreview en VB6)
        /// </summary>
        protected override bool OnBackButtonPressed()
        {
            // Prevenir que el botón de retroceso cierre la aplicación
            // Mostrar confirmación de salida
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await OnSalirClicked(this, EventArgs.Empty);
            });
            
            return true; // Indica que manejamos el evento
        }
    }
}
