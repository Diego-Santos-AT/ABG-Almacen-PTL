Attribute VB_Name = "GDConstantes"
'*************************************************************************************
'GDConstantes
'               Modulo para la definición de constantes generales de la aplicación
'*************************************************************************************

Option Explicit

'-------------------------------------------------------------------------------------
'Constantes para los modos del formulario
Public Const MOD_Seleccion = 0
Public Const MOD_Edicion = 1
Public Const MOD_Todo = 2
Public Const MOD_Nada = 3

'Constantes para los botones de acción
'según el ToolBar del formulario Principal frmMain
Public Const CMD_Salir = 1
'Separador
Public Const CMD_Primero = 3
Public Const CMD_Anterior = 4
Public Const CMD_Siguiente = 5
Public Const CMD_Ultimo = 6
'Separador
Public Const CMD_Nuevo = 8
Public Const CMD_Eliminar = 9
Public Const CMD_Deshacer = 10
Public Const CMD_Grabar = 11
'Separador
Public Const CMD_Pantalla = 13
Public Const CMD_Imprimir = 14
'Separador
Public Const CMD_Filtrar = 16
Public Const CMD_Buscar = 17
Public Const CMD_Divisa = 18
'Separador
Public Const CMD_Ayuda = 20
'Separador
Public Const CMD_Menu = 22

Public Const MAX_Botones = CMD_Menu

' --- Constantes de Impresion -------------------
Public Const CTE_ImpresionPantalla = 0
Public Const CTE_ImpresionImpresora = 1
Public Const CTE_CancelarImpresion = 2
Public compro As Boolean
Public driv As String
Public nom As String
Public port As String
Public cop As Integer

'-------------------------------------------------------------------------------------
'Constantes de mensajes
Public Const MSG_001 = " Grabar los cambios? "
Public Const MSG_002 = " Se regularizarán a 0 las existencias, Continuar? "
Public Const MSG_003 = " Abandonar los cambios? "
Public Const MSG_004 = " Imprimir el Formulario? "
Public Const MSG_005 = " Mensaje Nº 5 "
Public Const MSG_006 = " No existe: "
Public Const MSG_007 = " Se Eliminiaran los Datos Permanentemente. Continuar?"

Public Const MSG_050 = " No se ha podido actualizar el artículo! "
Public Const MSG_051 = " No se ha encontrado el artículo! "
Public Const MSG_052 = " Grabación Realizada! "

'Errores
Public Const MSG_100 = " Error al grabar los datos! "
Public Const MSG_101 = " Error al borrar los datos! "
Public Const MSG_102 = " Error, el dato está fuera de rango. "
Public Const MSG_103 = " Error en el campo: "
Public Const MSG_104 = " Debe introducir un valor en: "
Public Const MSG_105 = " se desharán los cambios. "

'-------------------------------------------------------------------------------------
'Constantes de divisas
Public Const DIV_Peseta = 0     'Divisa Peseta
Public Const DIV_Euro = 1       'Divisa Euro
Public Const DEC_Peseta = 0  'Decimales de trabajo en Pesetas
Public Const DEC_Euro = 3    'Decimales de trabajo en Euros

'*************************************************************************************

'Constantes para la linea de estado
Public Const EST_Mensaje = 1
Public Const EST_Empresa = 2
Public Const EST_Divisa = 3
Public Const EST_Usuario = 4

Public Const CTE_TiempoEsperaEntornoDatos = 200
Public Const CTE_TiempoEsperaTransaccion = 10

Public Const MetrosCubicos = 0.028317 ' Factor de conversión a m3 de 1 pie3


'Constantes de Tamaño de Etiquetas para Impresion de Bultos (SSCC,Ubicaciones,..)
Public Const ETI_14Con8 = 1
Public Const ETI_12Con9 = 2

' --- Estados de los Grupos ------------------------------
Public Const EstadoGrupo_Creado = "010"
Public Const EstadoGrupo_Asignado = "020"
Public Const EstadoGrupo_Iniciado = "030"
Public Const EstadoGrupo_Pausado = "040"
Public Const EstadoGrupo_Finalizado = "080"
Public Const EstadoGrupo_Completo = "085"
Public Const EstadoGrupo_Exportado = "090"

' -- Constantes SSCC
Public Const IncrementoSerieSSCC_Hipodromo = 30

