// ***********************************************************************
// Platforms/iOS/AppDelegate.cs
// Punto de entrada para iOS
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
