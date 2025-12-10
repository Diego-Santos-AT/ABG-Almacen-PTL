using ABGAlmacenPTL.Pages;
using ABGAlmacenPTL.Pages.PTL;

namespace ABGAlmacenPTL;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Registrar rutas para navegación
		Routing.RegisterRoute("MenuPage", typeof(MenuPage));
		Routing.RegisterRoute("InicioPage", typeof(InicioPage));
		
		// Registrar rutas PTL
		Routing.RegisterRoute("ConsultaPTLPage", typeof(ConsultaPTLPage));
		Routing.RegisterRoute("UbicarBACPage", typeof(UbicarBACPage));
		Routing.RegisterRoute("ExtraerBACPage", typeof(ExtraerBACPage));
		Routing.RegisterRoute("RepartirArticuloPage", typeof(RepartirArticuloPage));
		Routing.RegisterRoute("EmpaquetarBACPage", typeof(EmpaquetarBACPage));
	}
}
