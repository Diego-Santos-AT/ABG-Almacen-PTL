// ***********************************************************************
// Platforms/Windows/App.xaml.cs
// Punto de entrada para Windows
// ***********************************************************************

namespace ABGAlmacenPTL.Maui.WinUI
{
    public partial class App : MauiWinUIApplication
    {
        public App()
        {
            this.InitializeComponent();
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
