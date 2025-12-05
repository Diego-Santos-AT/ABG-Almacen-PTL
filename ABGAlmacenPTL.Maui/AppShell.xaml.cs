// ***********************************************************************
// Nombre: AppShell.xaml.cs
// Shell de la aplicación - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     frmMain (MDIForm) de VB6
// ***********************************************************************

using ABGAlmacenPTL.Maui.Pages;

namespace ABGAlmacenPTL.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Registrar rutas para navegación
            Routing.RegisterRoute(nameof(MenuPage), typeof(MenuPage));
            Routing.RegisterRoute(nameof(ConsultaPTLPage), typeof(ConsultaPTLPage));
            Routing.RegisterRoute(nameof(ExtraerBACPage), typeof(ExtraerBACPage));
            Routing.RegisterRoute(nameof(UbicarBACPage), typeof(UbicarBACPage));
            Routing.RegisterRoute(nameof(RepartirArticuloPage), typeof(RepartirArticuloPage));
            Routing.RegisterRoute(nameof(EmpaquetarBACPage), typeof(EmpaquetarBACPage));
        }
    }
}
