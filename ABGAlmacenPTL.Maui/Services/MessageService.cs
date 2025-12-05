// ***********************************************************************
// Nombre: MessageService.cs
// Servicio de mensajes - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     wsMensaje de VB6
// ***********************************************************************

namespace ABGAlmacenPTL.Maui.Services
{
    /// <summary>
    /// Servicio de mensajes - Equivalente a wsMensaje de VB6
    /// </summary>
    public class MessageService
    {
        private static MessageService? _instance;
        public static MessageService Instance => _instance ??= new MessageService();

        private Page? _currentPage;

        private MessageService() { }

        /// <summary>
        /// Establece la página actual para mostrar mensajes
        /// </summary>
        public void SetCurrentPage(Page page)
        {
            _currentPage = page;
        }

        /// <summary>
        /// Muestra un mensaje al usuario
        /// Equivalente a wsMensaje de VB6
        /// </summary>
        /// <param name="mensaje">Mensaje a mostrar</param>
        /// <param name="tipo">Tipo de mensaje (vbCritical, vbExclamation, etc.)</param>
        public async Task WsMensaje(string mensaje, int tipo)
        {
            if (_currentPage == null)
            {
                // Fallback si no hay página configurada
                await Application.Current!.MainPage!.DisplayAlert(
                    GetTitulo(tipo), 
                    mensaje.Trim(), 
                    "Aceptar");
                return;
            }

            await _currentPage.DisplayAlert(
                GetTitulo(tipo), 
                mensaje.Trim(), 
                "Aceptar");
        }

        /// <summary>
        /// Muestra un mensaje con opciones Sí/No
        /// Equivalente a MsgBox con vbYesNo de VB6
        /// </summary>
        public async Task<bool> MsgBoxYesNo(string mensaje, string titulo = "Confirmar")
        {
            if (_currentPage == null)
            {
                return await Application.Current!.MainPage!.DisplayAlert(
                    titulo, 
                    mensaje, 
                    "Sí", 
                    "No");
            }

            return await _currentPage.DisplayAlert(
                titulo, 
                mensaje, 
                "Sí", 
                "No");
        }

        /// <summary>
        /// Muestra un mensaje de información
        /// Equivalente a MsgBox con vbInformation
        /// </summary>
        public async Task MsgBoxInfo(string mensaje, string titulo = "Información")
        {
            await WsMensaje(mensaje, 64); // vbInformation = 64
        }

        /// <summary>
        /// Muestra un mensaje de error
        /// Equivalente a MsgBox con vbCritical
        /// </summary>
        public async Task MsgBoxError(string mensaje, string titulo = "Error")
        {
            await WsMensaje(mensaje, 16); // vbCritical = 16
        }

        /// <summary>
        /// Muestra un mensaje de exclamación
        /// Equivalente a MsgBox con vbExclamation
        /// </summary>
        public async Task MsgBoxExclamacion(string mensaje, string titulo = "Atención")
        {
            await WsMensaje(mensaje, Constants.MENSAJE_Exclamacion);
        }

        /// <summary>
        /// Obtiene el título según el tipo de mensaje
        /// </summary>
        private string GetTitulo(int tipo)
        {
            return tipo switch
            {
                16 => "Error",                          // vbCritical
                32 => "Pregunta",                       // vbQuestion
                48 => "Advertencia",                    // vbExclamation
                64 => "Información",                    // vbInformation
                Constants.MENSAJE_Exclamacion => "Atención",
                Constants.MENSAJE_Grave => "Error Grave",
                _ => "Mensaje"
            };
        }

        /// <summary>
        /// Constantes de VB6 para referencia:
        /// vbOKOnly = 0
        /// vbOKCancel = 1
        /// vbAbortRetryIgnore = 2
        /// vbYesNoCancel = 3
        /// vbYesNo = 4
        /// vbRetryCancel = 5
        /// vbCritical = 16
        /// vbQuestion = 32
        /// vbExclamation = 48
        /// vbInformation = 64
        /// </summary>
    }
}
