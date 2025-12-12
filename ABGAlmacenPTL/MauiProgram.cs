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
		var abgConfig = new ABGConfigService();
		builder.Services.AddSingleton(abgConfig);
		
		// ========================================================================
		// CONFIGURACIÓN DE BASES DE DATOS MÚLTIPLES (FIEL AL VB6)
		// ========================================================================
		
		// 1. Config DB (GROOT) - Usuarios, empresas, configuración
		var configConnectionString = abgConfig.GetConfigConnectionString();
		builder.Services.AddDbContext<ConfigContext>(options =>
			options.UseSqlServer(configConnectionString));
		
		// 2. GestionAlmacen DB (PTL) - Se reconfigura dinámicamente después del login
		// Factory pattern para permitir reconexión según empresa seleccionada
		builder.Services.AddScoped<ABGAlmacenContext>(serviceProvider =>
		{
			var authService = serviceProvider.GetRequiredService<AuthService>();
			var optionsBuilder = new DbContextOptionsBuilder<ABGAlmacenContext>();
			
			// Si hay empresa seleccionada, usar su GestionAlmacen DB
			// Si no, usar Config DB temporalmente
			var connectionString = authService.EmpresaActual != null
				? authService.ObtenerConnectionStringGestionAlmacen()
				: configConnectionString;
			
			optionsBuilder.UseSqlServer(connectionString ?? configConnectionString);
			return new ABGAlmacenContext(optionsBuilder.Options);
		});

		// Register authentication service (VB6-faithful)
		builder.Services.AddScoped<AuthService>();
		
		// Register database connection manager (VB6-faithful)
		builder.Services.AddScoped<DatabaseConnectionManager>();
		
		// Register dynamic database services (VB6-faithful - stored procedures)
		builder.Services.AddScoped<IDynamicDatabaseService, DynamicDatabaseService>();
		builder.Services.AddScoped<PTLStoredProcedureService>();
		
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
