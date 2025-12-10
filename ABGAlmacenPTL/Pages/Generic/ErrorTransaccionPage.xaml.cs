using System.Timers;

namespace ABGAlmacenPTL.Pages.Generic
{
    /// <summary>
    /// Formulario para mostrar errores de transacción con temporizador
    /// Migrado desde VB6 frmErrorTransaccion.frm
    /// </summary>
    public partial class ErrorTransaccionPage : ContentPage
    {
        private bool _resultado = false;
        private TaskCompletionSource<bool>? _tcs;
        private System.Timers.Timer? _timer;
        private int _tiempoSegundos;
        private int _progressValue;
        private int _progressMax;

        public bool Resultado => _resultado;

        public ErrorTransaccionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muestra el formulario de error con temporizador
        /// </summary>
        /// <param name="mensaje">Mensaje de error a mostrar</param>
        /// <param name="titulo">Título de la ventana</param>
        /// <param name="tiempoSegundos">Tiempo de espera en segundos antes de cerrar automáticamente</param>
        /// <returns>True si el usuario aceptó, False si canceló o expiró el tiempo</returns>
        public static async Task<bool> ShowAsync(string mensaje, string titulo = "Error de Transacción", int tiempoSegundos = 10)
        {
            var page = new ErrorTransaccionPage();
            page.Title = titulo;
            page.lblMensaje.Text = mensaje;
            page._tiempoSegundos = tiempoSegundos;
            page._tcs = new TaskCompletionSource<bool>();
            
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            
            return await page._tcs.Task;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Configurar progreso
            _progressMax = _tiempoSegundos * 5; // 5 ticks por segundo (cada 200ms)
            _progressValue = 0;
            progressBar.Progress = 0;

            // Iniciar timer
            _timer = new System.Timers.Timer(200); // 200ms = 0.2 segundos
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Detener y limpiar timer
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= OnTimerElapsed;
                _timer.Dispose();
                _timer = null;
            }
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            _progressValue++;

            if (_progressValue >= _progressMax)
            {
                // Tiempo expirado - cerrar automáticamente
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    _resultado = false;
                    _timer?.Stop();
                    _tcs?.SetResult(_resultado);
                    await Navigation.PopModalAsync();
                });
            }
            else
            {
                // Actualizar barra de progreso
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    progressBar.Progress = (double)_progressValue / _progressMax;
                });
            }
        }

        private async void OnAceptarClicked(object sender, EventArgs e)
        {
            _resultado = true;
            _timer?.Stop();
            _tcs?.SetResult(_resultado);
            await Navigation.PopModalAsync();
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            _resultado = false;
            _timer?.Stop();
            _tcs?.SetResult(_resultado);
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            // Manejar botón de retroceso como cancelar
            _resultado = false;
            _timer?.Stop();
            _tcs?.SetResult(_resultado);
            Navigation.PopModalAsync();
            return true;
        }
    }
}
