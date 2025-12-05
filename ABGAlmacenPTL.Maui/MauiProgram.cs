// ***********************************************************************
// Nombre: MauiProgram.cs
// Punto de entrada de la aplicación .NET MAUI
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     Sub Main de VB6
// ***********************************************************************

using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace ABGAlmacenPTL.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
