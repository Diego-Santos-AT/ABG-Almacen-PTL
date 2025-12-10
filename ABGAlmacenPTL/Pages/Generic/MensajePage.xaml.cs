namespace ABGAlmacenPTL.Pages.Generic
{
    /// <summary>
    /// Formulario genérico para mostrar mensajes simples
    /// Migrado desde VB6 frmMensaje.frm
    /// </summary>
    public partial class MensajePage : ContentPage
    {
        private TaskCompletionSource<bool>? _tcs;

        public MensajePage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muestra un mensaje simple con botón Aceptar
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="titulo">Título de la ventana</param>
        public static async Task ShowAsync(string mensaje, string titulo = "Mensaje")
        {
            var page = new MensajePage();
            page.Title = titulo;
            page.lblMensaje.Text = mensaje;
            page._tcs = new TaskCompletionSource<bool>();
            
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            await page._tcs.Task;
        }

        private async void OnAceptarClicked(object sender, EventArgs e)
        {
            _tcs?.SetResult(true);
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            // Manejar botón de retroceso
            _tcs?.SetResult(true);
            Navigation.PopModalAsync();
            return true;
        }
    }
}
