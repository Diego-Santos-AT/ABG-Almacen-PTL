namespace ABGAlmacenPTL.Pages.Generic
{
    /// <summary>
    /// Formulario genérico para mostrar mensajes personalizados
    /// Migrado desde VB6 frmMsgBox.frm
    /// </summary>
    public partial class MsgBoxPage : ContentPage
    {
        // Valores de retorno (compatibles con VB6)
        public enum MsgBoxResult
        {
            OK = 1,
            Cancel = 2,
            Abort = 3,
            Retry = 4,
            Ignore = 5,
            Yes = 6,
            No = 7
        }

        // Tipos de botones (compatibles con VB6)
        public enum MsgBoxButtons
        {
            OKOnly = 0,
            OKCancel = 1,
            AbortRetryIgnore = 2,
            YesNoCancel = 3,
            YesNo = 4,
            RetryCancel = 5
        }

        // Tipos de iconos (compatibles con VB6)
        public enum MsgBoxIcon
        {
            Critical = 16,
            Question = 32,
            Exclamation = 48,
            Information = 64
        }

        private MsgBoxResult _resultado;
        private TaskCompletionSource<MsgBoxResult>? _tcs;

        public MsgBoxResult Resultado => _resultado;

        private string _cabecera = string.Empty;
        private string _mensaje = string.Empty;
        private MsgBoxButtons _botones = MsgBoxButtons.OKOnly;
        private MsgBoxIcon _icono = MsgBoxIcon.Information;

        public MsgBoxPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Configura y muestra el cuadro de mensaje
        /// </summary>
        public static async Task<MsgBoxResult> ShowAsync(
            string mensaje, 
            string cabecera = "Mensaje",
            MsgBoxButtons botones = MsgBoxButtons.OKOnly,
            MsgBoxIcon icono = MsgBoxIcon.Information)
        {
            var page = new MsgBoxPage
            {
                _cabecera = cabecera,
                _mensaje = mensaje,
                _botones = botones,
                _icono = icono
            };

            page._tcs = new TaskCompletionSource<MsgBoxResult>();
            
            await Application.Current!.Windows[0].Page!.Navigation.PushModalAsync(page);
            
            return await page._tcs.Task;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Configurar título y mensaje
            Title = _cabecera;
            lblMensaje.Text = _mensaje;

            // Configurar icono
            ConfigurarIcono();

            // Configurar botones
            ConfigurarBotones();
        }

        private void ConfigurarIcono()
        {
            // Establecer icono según tipo
            switch (_icono)
            {
                case MsgBoxIcon.Critical:
                    lblIcon.Text = "❌"; // Error
                    lblMensaje.TextColor = Colors.Red;
                    break;
                case MsgBoxIcon.Question:
                    lblIcon.Text = "❓"; // Pregunta
                    lblMensaje.TextColor = Colors.Blue;
                    break;
                case MsgBoxIcon.Exclamation:
                    lblIcon.Text = "⚠️"; // Advertencia
                    lblMensaje.TextColor = Color.FromRgb(255, 140, 0); // Orange
                    break;
                case MsgBoxIcon.Information:
                default:
                    lblIcon.Text = "ℹ️"; // Información
                    lblMensaje.TextColor = Colors.Blue;
                    break;
            }
        }

        private void ConfigurarBotones()
        {
            // Ocultar todos los botones primero
            btnSi.IsVisible = false;
            btnNo.IsVisible = false;
            btnCancelar.IsVisible = false;

            // Mostrar botones según configuración
            switch (_botones)
            {
                case MsgBoxButtons.OKOnly:
                    btnSi.Text = "ACEPTAR";
                    btnSi.IsVisible = true;
                    Grid.SetColumn(btnSi, 1); // Centrar
                    Grid.SetColumnSpan(btnSi, 1);
                    break;

                case MsgBoxButtons.OKCancel:
                    btnSi.Text = "ACEPTAR";
                    btnSi.IsVisible = true;
                    btnCancelar.IsVisible = true;
                    Grid.SetColumn(btnSi, 0);
                    Grid.SetColumn(btnCancelar, 2);
                    break;

                case MsgBoxButtons.YesNo:
                    btnSi.Text = "SÍ";
                    btnSi.IsVisible = true;
                    btnNo.IsVisible = true;
                    Grid.SetColumn(btnSi, 0);
                    Grid.SetColumn(btnNo, 2);
                    break;

                case MsgBoxButtons.YesNoCancel:
                    btnSi.Text = "SÍ";
                    btnSi.IsVisible = true;
                    btnNo.IsVisible = true;
                    btnCancelar.IsVisible = true;
                    Grid.SetColumn(btnSi, 0);
                    Grid.SetColumn(btnNo, 1);
                    Grid.SetColumn(btnCancelar, 2);
                    break;

                case MsgBoxButtons.RetryCancel:
                    btnSi.Text = "REINTENTAR";
                    btnSi.IsVisible = true;
                    btnCancelar.IsVisible = true;
                    Grid.SetColumn(btnSi, 0);
                    Grid.SetColumn(btnCancelar, 2);
                    break;

                case MsgBoxButtons.AbortRetryIgnore:
                    btnSi.Text = "ANULAR";
                    btnNo.Text = "REINTENTAR";
                    btnCancelar.Text = "IGNORAR";
                    btnSi.IsVisible = true;
                    btnNo.IsVisible = true;
                    btnCancelar.IsVisible = true;
                    Grid.SetColumn(btnSi, 0);
                    Grid.SetColumn(btnNo, 1);
                    Grid.SetColumn(btnCancelar, 2);
                    break;
            }
        }

        private async void OnSiClicked(object sender, EventArgs e)
        {
            // Determinar resultado según tipo de botones
            switch (_botones)
            {
                case MsgBoxButtons.YesNo:
                case MsgBoxButtons.YesNoCancel:
                    _resultado = MsgBoxResult.Yes;
                    break;
                case MsgBoxButtons.RetryCancel:
                    _resultado = MsgBoxResult.Retry;
                    break;
                case MsgBoxButtons.AbortRetryIgnore:
                    _resultado = MsgBoxResult.Abort;
                    break;
                default:
                    _resultado = MsgBoxResult.OK;
                    break;
            }

            _tcs?.SetResult(_resultado);
            await Navigation.PopModalAsync();
        }

        private async void OnNoClicked(object sender, EventArgs e)
        {
            // Determinar resultado según tipo de botones
            switch (_botones)
            {
                case MsgBoxButtons.YesNo:
                case MsgBoxButtons.YesNoCancel:
                    _resultado = MsgBoxResult.No;
                    break;
                case MsgBoxButtons.AbortRetryIgnore:
                    _resultado = MsgBoxResult.Retry;
                    break;
                default:
                    _resultado = MsgBoxResult.No;
                    break;
            }

            _tcs?.SetResult(_resultado);
            await Navigation.PopModalAsync();
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            // Determinar resultado según tipo de botones
            switch (_botones)
            {
                case MsgBoxButtons.AbortRetryIgnore:
                    _resultado = MsgBoxResult.Ignore;
                    break;
                default:
                    _resultado = MsgBoxResult.Cancel;
                    break;
            }

            _tcs?.SetResult(_resultado);
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            // Manejar botón de retroceso como cancelar
            _resultado = MsgBoxResult.Cancel;
            _tcs?.SetResult(_resultado);
            Navigation.PopModalAsync();
            return true;
        }
    }
}
