// ***********************************************************************
// Platforms/Android/MainApplication.cs
// Punto de entrada para Android
// ***********************************************************************

using Android.App;
using Android.Runtime;

namespace ABGAlmacenPTL.Maui
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
