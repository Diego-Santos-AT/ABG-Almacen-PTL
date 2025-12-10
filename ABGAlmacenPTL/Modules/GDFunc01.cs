namespace ABGAlmacenPTL.Modules
{
    /// <summary>
    /// Módulo de funciones generales
    /// Migrado desde VB6 GDFunc01.bas
    /// </summary>
    public static class GDFunc01
    {
        /// <summary>
        /// Tipos de mensajes
        /// </summary>
        public enum TipoMensaje
        {
            MENSAJE_Informativo = 64,  // vbInformation
            MENSAJE_Grave = 16,         // vbCritical
            MENSAJE_Exclamacion = 48    // vbExclamation
        }

        /// <summary>
        /// Función para cargar los menús de la aplicación según la opción elegida
        /// Migrado desde VB6: CargaMenu
        /// </summary>
        /// <param name="index">Índice del menú</param>
        public static void CargaMenu(int index)
        {
            Gestion.Menu[index].Nombre = "ABG Almacén RE";
            
            switch (index)
            {
                case Gestion.CMD_Aduana:
                    Gestion.Menu[index].Opcion = new List<Models.TipoOpcion>(2);
                    Gestion.Menu[index].Opcion.Add(new Models.TipoOpcion { Formulario = "" });
                    Gestion.Menu[index].Opcion.Add(new Models.TipoOpcion { Formulario = "" });
                    break;
                    
                case Gestion.CMD_Almacen:
                    Gestion.Menu[index].Opcion = new List<Models.TipoOpcion>(2);
                    Gestion.Menu[index].Opcion.Add(new Models.TipoOpcion { Formulario = "&Reparto Automático" });
                    Gestion.Menu[index].Opcion.Add(new Models.TipoOpcion { Formulario = "&Empaquetado" });
                    break;
            }
        }

        /// <summary>
        /// Función para activar/desactivar los botones de la barra de herramientas según el modo de trabajo
        /// Migrado desde VB6: CambiaModo
        /// </summary>
        /// <param name="modo">Modo de trabajo: MOD_Edicion | MOD_Seleccion</param>
        /// <remarks>
        /// En MAUI, esto se manejará a través del ViewModel y binding de comandos
        /// Esta función está preservada para referencia de la lógica original
        /// </remarks>
        public static void CambiaModo(int modo)
        {
            // En MAUI, el manejo de estados de botones se hará mediante:
            // - ViewModels con propiedades ICommand
            // - Binding a propiedades IsEnabled
            // - Gestión de estados a través de INotifyPropertyChanged
            
            // La lógica original VB6 se preserva aquí como referencia:
            switch (modo)
            {
                case GDConstantes.MOD_Seleccion:
                    // Entra en el modo de selección de registros
                    // Habilitar: Primero, Anterior, Siguiente, Ultimo, Nuevo, Eliminar, Salir, Pantalla, Imprimir, Filtrar, Buscar
                    // Deshabilitar: Deshacer, Grabar
                    break;
                    
                case GDConstantes.MOD_Edicion:
                    // Entra en el modo de edición de registros
                    // Habilitar: Deshacer, Grabar
                    // Deshabilitar: Primero, Anterior, Siguiente, Ultimo, Nuevo, Eliminar, Salir, Pantalla, Imprimir, Filtrar, Buscar
                    break;
                    
                case GDConstantes.MOD_Todo:
                    // Activa todos los botones
                    break;
                    
                case GDConstantes.MOD_Nada:
                    // Desactiva todos los botones excepto Salir y Menu
                    break;
            }
        }
    }
}
