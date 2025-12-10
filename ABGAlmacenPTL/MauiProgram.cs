using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using ABGAlmacenPTL.Data;
using ABGAlmacenPTL.Data.Repositories;
using ABGAlmacenPTL.Services;
using ABGAlmacenPTL.Pages;
using ABGAlmacenPTL.Pages.Generic;
using ABGAlmacenPTL.Pages.PTL;

namespace ABGAlmacenPTL;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Load configuration from appsettings.json
		var assembly = Assembly.GetExecutingAssembly();
		using var stream = assembly.GetManifestResourceStream("ABGAlmacenPTL.appsettings.json");
		
		var config = new ConfigurationBuilder()
			.AddJsonStream(stream!)
			.Build();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// ========================================================================
		// CONFIGURACIÓN FIEL AL VB6 ORIGINAL
		// Lee abg.ini y configura conexiones a múltiples bases de datos
		// ========================================================================
		
		// Copiar abg.ini desde recursos a carpeta de datos de la aplicación
		var iniPath = Path.Combine(FileSystem.AppDataDirectory, "abg.ini");
		if (!File.Exists(iniPath))
		{
			// Copiar desde recursos en primera ejecución
			using var iniStream = assembly.GetManifestResourceStream("ABGAlmacenPTL.abg.ini");
			if (iniStream != null)
			{
				using var fileStream = File.Create(iniPath);
				iniStream.CopyTo(fileStream);
			}
		}
		
		// Crear servicio de configuración que lee abg.ini (como VB6)
		var abgConfig = new ABGConfigService(iniPath);
		builder.Services.AddSingleton(abgConfig);
		
		// Obtener connection string para Config DB (servidor local GROOT)
		var configConnectionString = abgConfig.GetConfigConnectionString();
		
		// NOTA: En VB6, después de login se obtiene la empresa del usuario desde Config DB
		// y luego se construyen las conexiones a Gestion y GestionAlmacen dinámicamente.
		// Por ahora usamos Config DB que es el que tiene usuarios y configuración.
		builder.Services.AddDbContext<ABGAlmacenContext>(options =>
			options.UseSqlServer(configConnectionString));

		// Register repositories
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
		
		// Register services
		builder.Services.AddScoped<PTLService>();

		// Register pages
		builder.Services.AddTransient<InicioPage>();
		builder.Services.AddTransient<MenuPage>();
		builder.Services.AddTransient<ConsultaPTLPage>();
		builder.Services.AddTransient<RepartirArticuloPage>();
		builder.Services.AddTransient<UbicarBACPage>();
		builder.Services.AddTransient<ExtraerBACPage>();
		builder.Services.AddTransient<EmpaquetarBACPage>();
		
		// Register generic pages
		builder.Services.AddTransient<MsgBoxPage>();
		builder.Services.AddTransient<MensajePage>();
		builder.Services.AddTransient<ErrorTransaccionPage>();
		builder.Services.AddTransient<SeleccionTabla2Page>();
		builder.Services.AddTransient<VerFotoPage>();

		return builder.Build();
	}
}
