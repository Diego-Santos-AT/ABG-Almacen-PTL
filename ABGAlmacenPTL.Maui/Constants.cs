// ***********************************************************************
// Nombre: Constants.cs
// Constantes globales de la aplicación - Conversión fiel de VB6
//
// Creación:      Conversión de VB6 a .NET MAUI
// Basado en:     wsConstantes.bas, wsConfiguracion.bas
// ***********************************************************************

namespace ABGAlmacenPTL.Maui
{
    /// <summary>
    /// Constantes globales de la aplicación PTL Almacén
    /// Equivalente a wsConstantes.bas en VB6
    /// </summary>
    public static class Constants
    {
        // ----- Constantes de Módulo PTL -------------
        public const string MOD_Nombre_ConsultaPTL = "Consultas PTL";
        public const string MOD_Nombre_ExtraerBAC = "Extraer BAC";
        public const string MOD_Nombre_UbicarBAC = "Ubicar BAC";
        public const string MOD_Nombre_RepartirArticulo = "Repartir Articulo";
        public const string MOD_Nombre_EmpaquetarBAC = "Empaquetar BAC";

        // ----- Constantes de Comandos -------------
        public const int CML_Salir = 990;
        public const int CML_Cancelar = 0;
        
        // Constantes de EmpaquetarBAC
        public const int CML_Opciones = 0;
        public const int CML_Acciones = 5;
        public const int CML_CerrarBAC = 10;
        public const int CML_ExtraerBAC = 20;
        public const int CML_CrearCAJA = 30;
        public const int CML_ImprimirCAJA = 40;
        public const int CML_RelContenido = 50;
        public const int CML_Empaquetado = 60;
        public const int CML_CambiarCAJA = 70;
        public const int CML_CambiarUDS = 80;
        public const int CML_CombinarCAJAS = 85;
        public const int CML_RestarUDS = 90;
        public const int CML_SumarUDS = 95;
        public const int CML_AplicarUDS = 110;
        public const int CML_Volver = 100;
        public const int CML_CambiaCAJA = 170;
        public const int CML_AplicaCombinar = 180;
        public const int CML_VolverCajas = 120;
        public const int CML_VolverCantidad = 130;
        public const int CML_VolverCombinar = 140;

        // ----- Opciones de CheckBox -------------
        public const int OPC_CerrarBAC = 0;
        public const int OPC_ExtraerBAC = 1;
        public const int OPC_CrearCAJA = 2;
        public const int OPC_ImprimirCAJA = 3;
        public const int OPC_RelContenido = 4;
        public const int OPC_CerrarCAJA = 5;
        public const int OPC_Empaquetado = 6;

        // ----- Acciones de Caption -------------
        public const string ACC_General = "ACCIONES";
        public const string ACC_Empaquetar = "EMPAQUETAR";
        public const string ACC_Etiquetas = "IMPRIMIR ETIQUETAS";

        // ----- Constantes de Listas -------------
        public const int LIS_ContenidoBAC = 1;
        public const int LIS_ContenidoCAJA = 2;
        public const int LIS_TipoCajas = 3;

        // ----- Colores (Convertidos de VB6 Long a Color) -------------
        // VB6: ColorRojo = &H8080FF (BGR format) = RGB(255, 128, 128)
        // VB6: ColorVerde = &H80FF80 (BGR format) = RGB(128, 255, 128)
        public static readonly Color ColorRojo = Color.FromRgb(255, 128, 128);
        public static readonly Color ColorVerde = Color.FromRgb(128, 255, 128);
        public static readonly Color ColorAzulFondo = Color.FromRgb(0, 96, 176); // &H00B06000

        // ----- Constantes de Mensajes -------------
        public const int MENSAJE_Exclamacion = 1;
        public const int MENSAJE_Grave = 2;

        // ----- Constantes de Estado de Barra -------------
        public const int EST_Empresa = 1;
        public const int EST_Usuario = 2;

        // ----- Constantes de Comandos de Toolbar -------------
        public const int CMD_Salir = 1;
        public const int CMD_Menu = 22;

        // ----- Constantes de Configuración -------------
        public const int CTE_TiempoEsperaTransaccion = 30; // Segundos
        public const int CTE_TiempoEsperaEntornoDatos = 60; // Segundos

        // ----- Incremento Serie SSCC Hipódromo -------------
        public const int IncrementoSerieSSCC_Hipodromo = 0;

        // ----- Constantes ADO State -------------
        public const int adStateClosed = 0;
        public const int adStateOpen = 1;
    }
}
