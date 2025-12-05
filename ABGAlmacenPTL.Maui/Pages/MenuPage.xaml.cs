// ***********************************************************************
// Nombre: MenuPage.xaml.cs
// Menú Principal - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     frmMenu.frm de VB6
// ***********************************************************************

namespace ABGAlmacenPTL.Maui.Pages
{
    public partial class MenuPage : ContentPage
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navega a Consultas PTL
        /// Equivalente a mostrar frmConsultaPTL en VB6
        /// </summary>
        private async void OnConsultasPTLClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ConsultaPTLPage));
        }

        /// <summary>
        /// Navega a Extraer BAC
        /// Equivalente a mostrar frmExtraerBAC en VB6
        /// </summary>
        private async void OnExtraerBACClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ExtraerBACPage));
        }

        /// <summary>
        /// Navega a Ubicar BAC
        /// Equivalente a mostrar frmUbicarBAC en VB6
        /// </summary>
        private async void OnUbicarBACClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(UbicarBACPage));
        }

        /// <summary>
        /// Navega a Repartir Artículo
        /// Equivalente a mostrar frmRepartirArticulo en VB6
        /// </summary>
        private async void OnRepartirArticuloClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RepartirArticuloPage));
        }

        /// <summary>
        /// Navega a Empaquetar BAC
        /// Equivalente a mostrar frmEmpaquetarBAC en VB6
        /// </summary>
        private async void OnEmpaquetarBACClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(EmpaquetarBACPage));
        }

        /// <summary>
        /// Sale de la aplicación
        /// Equivalente a Unload Me en VB6
        /// </summary>
        private async void OnSalirClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(
                "Confirmar", 
                "¿Desea salir de la aplicación?", 
                "Sí", 
                "No");

            if (answer)
            {
                Application.Current?.Quit();
            }
        }
    }
}
