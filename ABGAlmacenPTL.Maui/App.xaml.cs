// ***********************************************************************
// Nombre: App.xaml.cs
// Aplicación principal - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     frmMain (MDIForm) de VB6
// ***********************************************************************

namespace ABGAlmacenPTL.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell())
            {
                Title = $"PTL ALM ({Services.AppSettings.Instance.Empresa})"
            };
        }
    }
}
