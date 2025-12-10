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

		// Database configuration from appsettings.json
		var connectionString = config.GetConnectionString("ABGAlmacenDB") 
			?? "Server=(localdb)\\mssqllocaldb;Database=ABGAlmacenPTL;Trusted_Connection=true;MultipleActiveResultSets=true";
		
		builder.Services.AddDbContext<ABGAlmacenContext>(options =>
			options.UseSqlServer(connectionString));

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
