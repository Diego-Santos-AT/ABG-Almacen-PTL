// ***********************************************************************
// Platforms/MacCatalyst/AppDelegate.cs
// Punto de entrada para macOS Catalyst
// ***********************************************************************

using Foundation;

namespace ABGAlmacenPTL.Maui
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
