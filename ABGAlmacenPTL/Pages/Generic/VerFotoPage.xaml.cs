namespace ABGAlmacenPTL.Pages.Generic
{
    /// <summary>
    /// Formulario para mostrar la foto de un artículo
    /// Migrado desde VB6 frmVerFoto.frm
    /// Creado: 7/02/00
    /// </summary>
    public partial class VerFotoPage : ContentPage
    {
        private long _codigo;
        private string _nombre = string.Empty;
        private string _ruta = string.Empty;

        public VerFotoPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Muestra la foto de un artículo
        /// </summary>
        /// <param name="codigo">Código del artículo</param>
        /// <param name="nombre">Nombre del artículo</param>
        /// <param name="ruta">Ruta donde se encuentran las fotos</param>
        public static async Task ShowAsync(long codigo, string nombre, string ruta)
        {
            var page = new VerFotoPage
            {
                _codigo = codigo,
                _nombre = nombre,
                _ruta = ruta
            };
            
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Actualizar título
            Title = $"Código: {_codigo} - {_nombre}";

            // Cargar imagen
            CargarImagen();
        }

        private void CargarImagen()
        {
            try
            {
                // Construir ruta completa de la imagen
                string nombreArchivo = Path.Combine(_ruta, $"{_codigo}.jpg");

                if (File.Exists(nombreArchivo))
                {
                    // Cargar imagen desde archivo
                    imgFoto.Source = ImageSource.FromFile(nombreArchivo);
                    imgFoto.IsVisible = true;
                    lblNoFoto.IsVisible = false;
                }
                else
                {
                    // Intentar con otras extensiones comunes
                    string[] extensiones = { ".png", ".jpeg", ".bmp", ".gif" };
                    bool encontrado = false;

                    foreach (var ext in extensiones)
                    {
                        nombreArchivo = Path.Combine(_ruta, $"{_codigo}{ext}");
                        if (File.Exists(nombreArchivo))
                        {
                            imgFoto.Source = ImageSource.FromFile(nombreArchivo);
                            imgFoto.IsVisible = true;
                            lblNoFoto.IsVisible = false;
                            encontrado = true;
                            break;
                        }
                    }

                    if (!encontrado)
                    {
                        // No se encontró la imagen
                        imgFoto.IsVisible = false;
                        lblNoFoto.IsVisible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // Error al cargar imagen
                imgFoto.IsVisible = false;
                lblNoFoto.Text = $"Error al cargar imagen: {ex.Message}";
                lblNoFoto.IsVisible = true;
            }
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            // Cerrar al hacer clic en la imagen (como VB6)
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            // Permitir cerrar con botón de retroceso
            Navigation.PopModalAsync();
            return true;
        }
    }
}
