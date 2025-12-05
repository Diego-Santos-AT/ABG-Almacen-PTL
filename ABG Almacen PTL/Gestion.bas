Attribute VB_Name = "Gestion"
'#########################################################################################
'Programa para la Gestión Integral de las empresa del grupo Angel Tomás
'
'Gestion: Módulo principal de la aplicación
'
'#########################################################################################
Option Explicit
' Función para hallar el nombre del PC en el que se ejecuta el programa
Declare Function GetComputerName Lib "kernel32" Alias "GetComputerNameA" (ByVal lpBuffer As String, nSize As Long) As Long
Declare Function Beep Lib "kernel32" (ByVal dwFreq As Long, ByVal dwDuration As Long) As Long
'*****************************************************************************************
'TIPOS DE DATOS PERSONALIZADOS

Public Type TipoEmpresa
    Codigo As Integer       ' --- Codigo de Empresa
    Nombre As String        ' --- Nombre de Empresa
    Servidor As String      ' --- Servidor de Empresa
    base As String          ' --- Base de Datos de Empresa
    Usuario As String       ' --- Usuario de Acceso
    Clave As String         ' --- Clave de Usuario
    Dsn As String           ' --- DSN
    Dll As String           ' --- DLL
    RutaFotosImpresion As String    ' --- Ruta de fotos para Imprimir
    RutaFotosConsulta As String     ' --- Ruta de fotos para consulta
    RutaInformes As String          ' --- Ruta de informes Crystal Report
    Logo As String                  ' --- Logotipo de la empresa (jpg)
    ContraEmpresa As String         ' --- Codigo de ContraEmpresa por defecto
    MarcaDesarrollo As Boolean      ' --- Marca de empresa de desarrollo o gestión
    MarcaActiva As Boolean          ' --- Marca de empresa activa o inactiva
    MarcaCompradora As Boolean      ' --- Marca de empresa si es Compradora de otras empresas o NO
    MarcaImportadora As Boolean     ' --- Marca de empresa si es Importadora de otras empresas o NO
    Contabilidad_BT As String       ' --- Empresa de Contabilidad BT para DIMONI
    Contabilidad_TT As String       ' --- Empresa de Contabilidad TT para DIMONI
    Ean As String                   ' --- Codigo EAN de empresa
    CIF As String                   ' --- CIF de empresa
    poblacion As String             ' --- Población
    CodigoPostal As String          ' --- Codigo Postal
    Direccion As String             ' --- Dirección
    EMail As String                 ' --- e-mail
    web As String                   ' --- Dirección web
    Servidor_RadioFrecuencia As String      ' --- Servidor de Empresa para RadioFrecuencia
    Base_RadioFrecuencia As String          ' --- Base de Datos de Empresa para RadioFrecuencia
    Usuario_RadioFrecuencia As String       ' --- Usuario de Acceso para RadioFrecuencia
    Clave_RadioFrecuencia As String         ' --- Clave de Usuario para RadioFrecuencia
    Almacen_Fisico As Integer               ' --- Codigo de Almacen Fisico de la Empresa
    Suelo_Aduana As String          ' -- Ubicacion Suelo Aduana por Defecto
    Suelo_Almacen As String         ' -- Ubicacion Suelo Almacen por Defecto
    Suelo_Devolucion As String      ' -- Ubicacion Suelo Devoluciones por Defecto
    Suelo_Compras As String         ' -- Ubicacion Suelo Compras entre empresas por Defecto
    
    Servidor_GestionAlmacen As String   ' -- Servidor de empresa para Gestion de Almacen
    Base_GestionAlmacen As String       ' -- Base de Datos de empresa para Gestion de Almacen
    Usuario_GestionAlmacen As String    ' -- usuario de acceso para Gestion de Almacen
    Clave_GestionAlmacen As String      ' -- Clave de usuario para Gestion de Almacen
    
End Type

Public Type TipoOpcion
    Menu As String
    Formulario As String
End Type
'
Public Type TipoMenu        ' Tipo de datos para menu
    Nombre As String        ' Nombre de menu
    Opcion() As TipoOpcion     ' Lista de menu Opciones
    Listado() As TipoOpcion     ' Lista de menu Listados
    Especial() As TipoOpcion    ' Lista de menu Especiales
End Type

Public Type TipoUsuario         ' Tipo de Datos para Usuario
    Id As Integer               '  Identificador de usuario
    Nombre As String            '  Nombre Usuario
    instancias As Integer       ' Instancias
    nombrePC As Variant          ' Nombre del PC que puede arrancar(Null = todos)
End Type

Public Type PuestoTrabajo       ' Tipo de Datos para Puesto de trabajo
    Id As Integer               ' Identificador de puesto de trabajo
    Nombre As String            ' Nombre del puesto de trabajo
    NombreCorto As String       ' Nombre corto del puesto de trabajo
    Impresora As Integer        ' Identificador de la impresora asignada
    NombreImpresora As String   ' Nombre de la impresora
    TipoImpresora  As String    ' Tipo de Lenguaje de la impresora: TEC, ZEBRA, OTRO
End Type

'VARIABLES GLOBALES
Public EmpresaTrabajo As TipoEmpresa    ' --- Empresa activa de Trabajo

Public Menu(1) As TipoMenu          'Menu general de la aplicación (8 opciones principales)
'Public iUsuario As Integer          'Código de Usuario de entrada a la aplicación
'Public Usuario As String            'Nombre de Usuario de la aplicación

Public Usuario As TipoUsuario

Public CodEmpresa                   ' Codigo de empresa activa
Public Empresa As String            'Nombre del la Empresa Activa
'Public Conexion As String           'Cadena de conexión con el servidor SQL correspondiente'
Public ConexionGestion As String    'Cadena de conexión con el servidor SQL correspondiente para la BD Gestion
Public ConexionConfig As String    'Cadena de conexión con el servidor SQL correspondiente para la BD Config
Public ConexionRadioFrecuencia As String    ' Cadena de conexión con el servidor SQL de RadioFrecuencia para la BD
Public ConexionGestionAlmacen As String     ' Cadena de conexión con el servidor de Gestion de Almacen

'Public ConexionInformes As String   'Cadena de conexión para los informes.

Public BDDServ As String            'Servidor de Base de datos para la conexión
Public BDDServLocal As String       ' Servidor local
Public BDDGestion As String         'Base de datos para la conexión
Public BDDConfig As String          'Base de datos de configuración para la conexión
Public BDDTime As Integer           'Tiempo de espera del servidor
Public UsrBDD As String             'Nombre de Usuario de acceso a la Base de Datos
Public UsrKey As String             'Clave de acceso del Usuario de la Base de Datos
Public UsrBDDConfig As String      'Nombre de Usuario de acceso a la Base de Datos Config
Public UsrKeyConfig As String      'Clave de acceso del Usuario de la Base de Datos Config
Public FicheroDLL As String        ' Fichero DLL de acceso para informes
Public FicheroDSN As String        ' Fichero DSN de conexión para los informes
Public RutaDSN As String            ' Ruta del Fichero DSN
Public LogoEmpresa As String        ' Fichero jpg de logotipo de la empresa

' --- VARIABLES GLOBALES DE LA OTRA EMPRESA COINCIDENTE ----------------
Public Otro_CodEmpresa                   ' Codigo de empresa activa
Public Otro_Empresa As String            'Nombre del la Empresa Activa
Public Otro_ConexionGestion As String    'Cadena de conexión con el servidor SQL correspondiente para la BD Gestion

Public Otro_BDDServ As String            'Servidor de Base de datos para la conexión
Public Otro_BDDGestion As String         'Base de datos para la conexión
Public Otro_BDDTime As Integer           'Tiempo de espera del servidor
Public Otro_UsrBDD As String             'Nombre de Usuario de acceso a la Base de Datos
Public Otro_UsrKey As String             'Clave de acceso del Usuario de la Base de Datos
' ----------------------------------------------------------------------

' --- VARIABLES GLOBALES DE LA EMPRESA CONTABILIDAD --------------------
Public Contable_CodEmpresa                   ' Codigo de empresa activa
Public Contable_Empresa_Oficial As String    'Nombre de la Empresa Activa Contabilidad Oficial
Public Contable_Empresa_TT As String         'Nombre de la Empresa Activa Contabilidad TT
Public Contable_Conexion As String          'Cadena de conexión con el servidor SQL correspondiente para la BD Gestion

Public Contable_BDDServ As String            'Servidor de Base de datos para la conexión
Public Contable_BDDGestion As String         'Base de datos para la conexión
Public Contable_BDDTime As Integer           'Tiempo de espera del servidor
Public Contable_UsrBDD As String             'Nombre de Usuario de acceso a la Base de Datos
Public Contable_UsrKey As String             'Clave de acceso del Usuario de la Base de Datos
' ----------------------------------------------------------------------

Public ClaveMaestra As String       'Clave maestra de acceso a zonas restringidas
Public LoginSucceeded As Boolean    'Variable booleana que contiene el resultado de
                                    'un intento de acceso a zonas restringidas
Public wRCFotos As String           'Ruta de fotos para consulta
Public wRCFotosImp As String        'Ruta de fotos para impresión
Public wRInformes As String         'Ruta de los informes

Public wDirOrigen As String         'Ruta de Origen para exportación de datos
Public wDirExport As String         'Ruta de exportación por defecto
Public UsrDefault As String         'Usuario de entrada por defecto
Public ficINI As String             ' Fichero ini

Public wDivisa As Integer           'Codigo de divisa de la Base de Datos.
Public wTDivisa As Integer          'Codigo de divisa de trabajo.
Public wDecimales As Integer        'Numero de decimales de trabajo
Public wTasaCambio As Double        'Tasa de conversión entre divisa de trabajo
Public wBloqueoDivisa As Integer    'Bloqueo de cambio de divisa cuando hay un formulario
                                    'en modo edición.

Public wPuestoTrabajo As PuestoTrabajo  ' Puesto de trabajo
Public wImpresora As String             ' Impresora de etiquetas relacionada con el puesto de trabajo

'DECLARACIÓN DE CONSTANTES PUBLICAS
'Constantes para los botones de Menu
Public Const CMD_Almacen = 0
Public Const CMD_Compras = 1
Public Const CMD_Ventas = 2
Public Const CMD_Terceros = 3
Public Const CMD_Ficheros = 4
Public Const CMD_Contabilidad = 5
Public Const CMD_Empresa = 6
Public Const CMD_Aduana = 7
'Public Const CMD_Salir = 7

'Constantes generales
Public Const vEuro = 166.386    'Valor del cambio Pesetas por Euro
Public Const vPeseta = 1        'Valor del cambio Pesetas por Pesetas
'Public Const vEuro = 1              'Valor del cambio Euros por Euro
Public Const vPesetaE = 0.006010121  'Valor del cambio Euros por Peseta
Public wNEmpresa As String      ' NOMBRE DE LA EMPRESA DE TRABAJO

' Variables generales
'Sistema de Ayuda
Public cHelpFile As String      'Ruta completa del fichero de ayuda.
Public sHelpFile As String      'Cadena del fichero de ayuda.
Public Impresion As Boolean
Dim edC As New edConfig
Public wIdioma As Integer   ' Variable para que no de error frmCalálogosConsulta

'*****************************************************************************************
Sub Main()
'    Dim wBase As String         ' NOMBRE DE LA BASE DE DATOS
'    Dim wEmpresa As Boolean     ' EMPRESA DE TRABAJO
'    Dim wLogo As String         ' LOGOTIPO DE LA EMPRESA
'    Dim wRFotos As String       ' RUTA DE LAS FOTOS DE IMPRESION
'    Dim wRCFotos As String      ' RUTA DE LAS FOTOS PARA CONSULTAS
    Dim sMsg As String          ' MENSAJES EN PANTALLA
'    Dim BaseTrabajo As String   ' BASE DE TRABAJO
    
        
    Dim sql As String           ' Sentencias sql
    Dim i                       ' Variable de bucle
    Dim Config As String        ' Ruta del fichero de configuración
    
    'MousePointer = ccHourglass
    
    'Lectura de la Configuración del Programa.
    
Inicio:
    On Error GoTo ErrInicio
    Config = Dir(App.Path & "\config.mdb")
    
    '**** 1er Paso del Arranque: Comprobar que existe el fichero ABG.INI
    'ficIni = "C:\Archivos de Programa\ABG\abg.ini"     ' Windows98
    ficINI = App.Path & "\abg.ini"     ' Windows98
'    If dir(f) <> "" Then
'
''    Else
''        ficIni = "C:\WINNT\system\abg.ini"    ' Windows2000
'    End If
        ' Si no existe se crea
    If Dir(ficINI) = "" Then
        CrearAGBIni (ficINI)
    End If
    
    ' Lee los primeros parametros
    LeerParamentrosIni (ficINI)
 
    '**** 2do Prueba de Conexión.
   ' ProbarConexion BDDServLocal
    
        
Datos:

    
    On Error GoTo 0
    
    '**** Inicializar otras variables generales
    InicializarVariablesGenerales
           
    '---------------------------------------------
    '**** 3º Comprobación de la conexión con el servidor y acceso del usuario
    frmInicio.Show vbModal
    
    
    If LoginSucceeded Then ' Usuario Correcto
        '***** 4º Comprueba las instancias
        If InstanciasPrograma = False Then
            MsgBox "No puede ejecutar más instancias del programa ABG...", vbExclamation, "ABG"
            End
        End If
    
    
    
        ' ***** 5º Cargar la empresa y su configuración
        
        ConfiguracionEmpresa (CodEmpresa)
        
        
        ' ***** 6º Leer el archivo DSN
        ' Comprueba que existe el DSN de la empresa y si no la tiene la crea
        LeerDSN
        
        
        
        
        ' ***** 7º Configuracion de los Menus Segun El Usuario ----
        'Llamada al modulo principal
        'Obtener_Menus
        
        ' ------------------------------------------------------
        frmMain.Show
    Else
        'No se puede acceder
        End
    End If
    
    Exit Sub
  
'Control de errores
ErrInicio:
'    If bErrorConexion Then
'        MsgBox "Error de conexión con el servidor " & BDDServ & "...", vbExclamation, "Conexión"
'        Screen.MousePointer = vbDefault
'        Exit Sub
'    End If

    Debug.Print "Error:"; Err, Error$
    sMsg = "0"
    Select Case Err
'        Case 3024
'            sMsg = MsgBox("No se puede abrir el fichero de Configuración", vbRetryCancel, " Error de Inicio ")
'        Case 1
'            sMsg = MsgBox("No se encuentra la Base de Datos: " & wBase, vbRetryCancel, " Error de Inicio ")
        Case Else
            MsgBox "Error: " & Err & "/  " & Error$
    End Select
    
    If sMsg = 4 Then
        Select Case Err
            Case 3024
                Resume Inicio
            Case 1
                Resume Datos
        End Select
    End If
End Sub
'*****************************************************************************************

' Crea el fichero de inicio ABG.INI
Private Sub CrearAGBIni(Fichero As String)
    '--- Configuración de la pantalla
    GuardarIni Fichero, "Pantalla", "MainLeft", "-60"
    GuardarIni Fichero, "Pantalla", "MainTop", "-60"
    GuardarIni Fichero, "Pantalla", "MainWidth", "15480"
    GuardarIni Fichero, "Pantalla", "MainHeight", "11220"
    ' --- Conexión
'    While (BDDServLocal = "") Or (BDDServLocal <> "ZEUS" And BDDServLocal <> "THOR")
'        'BDDServLocal = UCase(InputBox("¿Servidor local?.", "Conexión ABG"))
'        'frmServidorLocal.Show vbModal
'       ' BDDServLocal = frmServidorLocal.Servidor
'    Wend
    'If BDDServLocal <> "" And (BDDServLocal = "ZEUS" Or BDDServLocal = "THOR") Then
'        GuardarIni Fichero, "Conexion", "BDDServ", "THOR"       ' Servidor BDD
'        GuardarIni Fichero, "Conexion", "BDDServLocal", "ODIN"       ' guarda Servidor local
    'End If
    GuardarIni Fichero, "Conexion", "BDDTime", "30"         ' Tiempo de espera?
    GuardarIni Fichero, "Conexion", "BDDConfig", "Config"  ' Base Datos Config
    
        ' --- Servidores por defecto si no existe el ABG.INI. Deberán cambiarse por los correspondientes
    GuardarIni Fichero, "Conexion", "BDDServ", "ARES"
    GuardarIni Fichero, "Conexion", "BDDServLocal", "ARES"
    
    ' --- Varios
    GuardarIni Fichero, "Varios", "wDirExport", ""          ' Directorio de exp.
    GuardarIni Fichero, "Varios", "UsrDefault", ""          ' Usuario por defecto
    GuardarIni Fichero, "Varios", "EmpDefault", ""          ' Empresa por defecto
    GuardarIni Fichero, "Varios", "PueDefault", ""          ' Puesto de trabajo por defecto
    
End Sub

Private Sub LeerParamentrosIni(Fichero As String)

    BDDServ = LeerIni(ficINI, "Conexion", "BDDServ")
    
    ' Leemos el servidor local
    BDDServLocal = LeerIni(ficINI, "Conexion", "BDDServLocal", "")
    BDDTime = LeerIni(ficINI, "Conexion", "BDDTime", "30")
    BDDConfig = LeerIni(ficINI, "Conexion", "BDDConfig", "Config")
    UsrBDDConfig = "ABG"  ' El usuario es fijo
    UsrKeyConfig = "A_34ggyx4"    ' Su contraseña tb
    
    ' Leemos varios
    wDirExport = LeerIni(ficINI, "Varios", "wDirExport", App.Path)
    UsrDefault = LeerIni(ficINI, "Varios", "UsrDefault", "")
    CodEmpresa = LeerIni(ficINI, "Varios", "EmpDefault", "")
    wPuestoTrabajo.Id = LeerIni(ficINI, "Varios", "PueDefault", "1")  ' -- Puesto 1 por defecto
        
'    ' -- CODIGO MIGRACION A NUEVO SISTEMA VMWARE (28 Septiembre 2010) (DEBE ELIMINARSE TRAS MIGRACION) ------
'    If BDDServ = "APOLO\APOLO" Then
'        BDDServ = "RODABALLO"
'    End If
'    If BDDServ = "MARTE\MARTE" Then
'        BDDServ = "ARENQUE"
'    End If
'    GuardarIni Fichero, "Conexion", "BDDServ", BDDServ
'
'    If BDDServLocal = "APOLO\APOLO" Then
'        BDDServLocal = "RODABALLO"
'    End If
'    If BDDServLocal = "MARTE\MARTE" Then
'        BDDServLocal = "ARENQUE"
'    End If
'    GuardarIni Fichero, "Conexion", "BDDServLocal", BDDServLocal
'    ' --------------------------------------------------------------------------------------------------------
    
    ' -- MIGRACION A SQL 2016 (03 ABRIL 2020) (DEBE ELIMINARSE TRAS MIGRACION) ------
    If BDDServ = "RODABALLO" Then
        BDDServ = "GROOT"
    End If
    If BDDServ = "ARENQUE" Then
        BDDServ = "SELENE"
    End If
    GuardarIni Fichero, "Conexion", "BDDServ", BDDServ
    
    If BDDServLocal = "RODABALLO" Then
        BDDServLocal = "GROOT"
    End If
    If BDDServLocal = "ARENQUE" Then
        BDDServLocal = "SELENE"
    End If
    GuardarIni Fichero, "Conexion", "BDDServLocal", BDDServLocal
    ' --------------------------------------------------------------------------------------------------------
    
    ' Creamos la cadena del conexión al Config siempre al servidor local
    ConexionConfig = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & UsrBDDConfig & ";Password=" & UsrKeyConfig & ";Initial Catalog=" & BDDConfig & ";Data Source= " & BDDServLocal & ";Connect Timeout=" & BDDTime
    
End Sub

Private Sub InicializarVariablesGenerales()
    'Sistema de Ayuda----------------------------
    'Asigna el nombre del fichero de ayuda
    cHelpFile = App.Path & "\Gestion.chm"
    
    sHelpFile = cHelpFile
    App.HelpFile = sHelpFile
    
    'Ayuda de los mensajes de error
    'Err.HelpFile = App.Path & "\GestionErr.chm"
    
    '---------------------------------------------
    ClaveMaestra = "atoy"
    LoginSucceeded = False
    
    
    'Establecimiento de la divisa de la base de datos:
    'Opciones: Peseta = 0; Euro = 1
    wDivisa = DIV_Peseta
    'wDivisa = DIV_Euro
    
    'Establecimiento de la divisa de inicio de trabajo:
    wTDivisa = DIV_Euro
    'wTDivisa = DIV_Peseta
    
    'Control de Divisa
    If wTDivisa = DIV_Peseta Then
        'Decimales para pesetas
        wDecimales = DEC_Peseta
    Else
        'Decimales para euros
        wDecimales = DEC_Euro
    End If
    
    'Establecimiento de la tasa de cambio
    If wDivisa = wTDivisa Then
        wTasaCambio = 1
    Else
        If wTDivisa = DIV_Euro Then
            wTasaCambio = 1 / vEuro
        Else
            wTasaCambio = vEuro
        End If
    End If
    wBloqueoDivisa = 0
 
    '<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

End Sub

'--- Carga la configuración de la empresa.
' El usuario puede tener varias empresas para acceder.
' Se mira la empresa por defecto del usuario. Si tiene 1 carga su conf. directamente,
' si tiene más de una sacamos una lista para elegir. Por defecto siempre la última que
' entró, que se guarda en el fichero abg.ini
' El usuario pertenece puede acceder a las empresas en la Tabla gdusremp.

Private Sub ConfiguracionEmpresa(codemp)

    Dim gruemp As Integer
        
    ' Agrimos conexión con la configuración
    edC.Config.Open ConexionConfig
    ' Grupo de acceso a empresas del usuario
'   gruemp = DameGrupoEmpresaUsuario(Usuario.id)

    ' Empresas de acceso del usuario
    On Error Resume Next
        edC.rsDameEmpresasAccesoUsuario.Close
    On Error GoTo 0
    edC.DameEmpresasAccesoUsuario Usuario.Id
       
    'If gruemp = -1 Then ' Error no hay asignada empresa al usuario
    If edC.rsDameEmpresasAccesoUsuario.RecordCount = 0 Then
        MsgBox "No tiene asignada empresa actualmente." & Chr(13) & _
                "Consulte con el dpto. de informática.", vbExclamation, "Conexión"
        edC.rsDameEmpresasAccesoUsuario.Close
        End
    End If
'    ' Empresa/s por grupo
'    On Error Resume Next
'        edC.rsDameEmpresasPorGrupo.Close
'    On Error GoTo 0
'    edC.DameEmpresasPorGrupo gruemp
'
'    ' Sólo una
'    If edC.rsDameEmpresasPorGrupo.RecordCount = 1 Then
'        CodEmpresa = edC.rsDameEmpresasPorGrupo!greemp
'        edC.rsDameEmpresasPorGrupo.Close
'    Else 'Mas de una
'        edC.rsDameEmpresasPorGrupo.Close
'        frmSelectorEmpresa.GrupoEmpresa = gruemp
'        frmSelectorEmpresa.Show vbModal
'    End If
    ' Empresas del usuario
'    If edC.rsDameEmpresasAccesoUsuario.RecordCount = 1 Then ' Sólo una
'        CodEmpresa = edC.rsDameEmpresasAccesoUsuario!useemp
'        edC.rsDameEmpresasAccesoUsuario.Close
'    Else
'        frmSelectorEmpresa.GrupoEmpresa = edC.rsDameEmpresasAccesoUsuario!useemp
'        frmSelectorEmpresa.Show vbModal
'        If frmSelectorEmpresa.Cancelado = True Then End ' Si ha cancelado salimos
'    End If
    
    CargarParametrosEmpresa ' Carga los parametros de la empresa
End Sub

Public Sub CargarParametrosEmpresa()

    Dim bMostrarConexion As Boolean ' True muestra de nuevo el formulario de conexión
    bMostrarConexion = False
    
    ' --- Cargar parametros
    On Error Resume Next
        edC.rsDameParametrosEmpresa.Close
    On Error GoTo 0
    edC.DameParametrosEmpresa CInt(CodEmpresa)
        
    ' ----- Configuracion de la Empresa de Trabajo ---------
    With edC.rsDameParametrosEmpresa
        EmpresaTrabajo.Codigo = CInt(CodEmpresa)
        If IsNull(!empcif) = True Then
            EmpresaTrabajo.CIF = ""
        Else
            EmpresaTrabajo.CIF = !empcif
        End If
        If IsNull(!empkey) = True Then
            EmpresaTrabajo.Clave = ""
        Else
            EmpresaTrabajo.Clave = !empkey
        End If
        If IsNull(!emppos) = True Then
            EmpresaTrabajo.CodigoPostal = ""
        Else
            EmpresaTrabajo.CodigoPostal = !emppos
        End If
        If IsNull(!empcbt) = True Then
            EmpresaTrabajo.Contabilidad_BT = ""
        Else
            EmpresaTrabajo.Contabilidad_BT = !empcbt
        End If
        If IsNull(!empctt) = True Then
            EmpresaTrabajo.Contabilidad_TT = ""
        Else
            EmpresaTrabajo.Contabilidad_TT = !empctt
        End If
        EmpresaTrabajo.ContraEmpresa = !empotr
        If IsNull(!empdir) = True Then
            EmpresaTrabajo.Direccion = ""
        Else
            EmpresaTrabajo.Direccion = !empdir
        End If
        If IsNull(!empdll) = True Then
            EmpresaTrabajo.Dll = ""
        Else
            EmpresaTrabajo.Dll = !empdll
        End If
        If IsNull(!empdsn) = True Then
            EmpresaTrabajo.Dsn = ""
        Else
            EmpresaTrabajo.Dsn = !empdsn
        End If
        If IsNull(!empean) = True Then
            EmpresaTrabajo.Ean = ""
        Else
            EmpresaTrabajo.Ean = !empean
        End If
        If IsNull(!empema) = True Then
            EmpresaTrabajo.EMail = ""
        Else
            EmpresaTrabajo.EMail = !empema
        End If
        If IsNull(!emplog) = True Then
            EmpresaTrabajo.Logo = ""
        Else
            EmpresaTrabajo.Logo = !emplog
        End If
        If IsNull(!empact) = True Then
            EmpresaTrabajo.MarcaActiva = False
        Else
        If !empact = 0 Then
                EmpresaTrabajo.MarcaActiva = False
            Else
                EmpresaTrabajo.MarcaActiva = True
            End If
        End If
        If IsNull(!empcom) = True Then
            EmpresaTrabajo.MarcaCompradora = False
        Else
        If !empcom = 0 Then
                EmpresaTrabajo.MarcaCompradora = False
            Else
                EmpresaTrabajo.MarcaCompradora = True
            End If
        End If
        If IsNull(!empimp) = True Then
            EmpresaTrabajo.MarcaImportadora = False
        Else
        If !empimp = 0 Then
                EmpresaTrabajo.MarcaImportadora = False
            Else
                EmpresaTrabajo.MarcaImportadora = True
            End If
        End If
        
        If IsNull(!empdes) = True Then
            EmpresaTrabajo.MarcaDesarrollo = False
        Else
            If !empdes = 0 Then
                EmpresaTrabajo.MarcaDesarrollo = False
            Else
                EmpresaTrabajo.MarcaDesarrollo = True
            End If
        End If
        If IsNull(!empnom) = True Then
            EmpresaTrabajo.Nombre = ""
        Else
            EmpresaTrabajo.Nombre = !empnom
        End If
        If IsNull(!emppob) = True Then
            EmpresaTrabajo.poblacion = ""
        Else
            EmpresaTrabajo.poblacion = !emppob
        End If
        If IsNull(!emprfc) = True Then
            EmpresaTrabajo.RutaFotosConsulta = ""
        Else
            EmpresaTrabajo.RutaFotosConsulta = !emprfc
        End If
        If IsNull(!emprfi) = True Then
            EmpresaTrabajo.RutaFotosImpresion = ""
        Else
            EmpresaTrabajo.RutaFotosImpresion = !emprfi
        End If
        If IsNull(!emprin) = True Then
                EmpresaTrabajo.RutaInformes = ""
        Else
                EmpresaTrabajo.RutaInformes = !emprin
        End If
        If IsNull(!empser) = True Then
            EmpresaTrabajo.Servidor = ""
        Else
            EmpresaTrabajo.Servidor = wfQuitarCorchetes(!empser)
        End If
        If IsNull(!empusr) = True Then
            EmpresaTrabajo.Usuario = ""
        Else
            EmpresaTrabajo.Usuario = !empusr
        End If
        If IsNull(!empweb) = True Then
            EmpresaTrabajo.web = ""
        Else
            EmpresaTrabajo.web = !empweb
        End If
        
        ' ---- Valores de Radio Frecuencia --------------
        If IsNull(!empsra) = True Then
            EmpresaTrabajo.Servidor_RadioFrecuencia = ""
        Else
            EmpresaTrabajo.Servidor_RadioFrecuencia = wfQuitarCorchetes(!empsra)
        End If
        If IsNull(!empbra) = True Then
            EmpresaTrabajo.Base_RadioFrecuencia = ""
        Else
            EmpresaTrabajo.Base_RadioFrecuencia = !empbra
        End If
        If IsNull(!empura) = True Then
            EmpresaTrabajo.Usuario_RadioFrecuencia = ""
        Else
            EmpresaTrabajo.Usuario_RadioFrecuencia = !empura
        End If
        If IsNull(!empkra) = True Then
            EmpresaTrabajo.Clave_RadioFrecuencia = ""
        Else
            EmpresaTrabajo.Clave_RadioFrecuencia = !empkra
        End If
        If IsNull(!empalf) = True Then
            EmpresaTrabajo.Almacen_Fisico = 0
        Else
            EmpresaTrabajo.Almacen_Fisico = !empalf
        End If
        
        If IsNull(!empsad) = True Then
            EmpresaTrabajo.Suelo_Aduana = ""
        Else
            EmpresaTrabajo.Suelo_Aduana = !empsad
        End If
        
        If IsNull(!empsal) = True Then
            EmpresaTrabajo.Suelo_Almacen = ""
        Else
            EmpresaTrabajo.Suelo_Almacen = !empsal
        End If
        
        If IsNull(!empsde) = True Then
            EmpresaTrabajo.Suelo_Devolucion = ""
        Else
            EmpresaTrabajo.Suelo_Devolucion = !empsde
        End If
        
        If IsNull(!empsco) = True Then
            EmpresaTrabajo.Suelo_Compras = ""
        Else
            EmpresaTrabajo.Suelo_Compras = !empsco
        End If
        ' -----------------------------------------------
        
        
        ' ---- Valores de Gestion de Almacen --------------
        If IsNull(!empsga) = True Then
            EmpresaTrabajo.Servidor_GestionAlmacen = ""
        Else
            EmpresaTrabajo.Servidor_GestionAlmacen = wfQuitarCorchetes(!empsga)
        End If
        If IsNull(!empbga) = True Then
            EmpresaTrabajo.Base_GestionAlmacen = ""
        Else
            EmpresaTrabajo.Base_GestionAlmacen = !empbga
        End If
        If IsNull(!empuga) = True Then
            EmpresaTrabajo.Usuario_GestionAlmacen = ""
        Else
            EmpresaTrabajo.Usuario_GestionAlmacen = !empuga
        End If
        If IsNull(!empkga) = True Then
            EmpresaTrabajo.Clave_GestionAlmacen = ""
        Else
            EmpresaTrabajo.Clave_GestionAlmacen = !empkga
        End If
        ' --------------------------------------------------
        
    End With
    ' ------------------------------------------------------
    
    Empresa = edC.rsDameParametrosEmpresa!empnom        ' Nombre de la empresa
    
    BDDServ = wfQuitarCorchetes(edC.rsDameParametrosEmpresa!empser)        ' Servidor de la empresa
    ' Si el servidor elegido es distinto al local probamos de nuevo la conexión
    If BDDServ <> BDDServLocal Then
        'If ProbarConexion(BDDServ) = False Then End
        bMostrarConexion = True
    End If
    BDDGestion = edC.rsDameParametrosEmpresa!empbdd     ' Nombre de la BD
    UsrBDD = edC.rsDameParametrosEmpresa!empusr         ' Usuario de acceso a la BD
    If IsNull(edC.rsDameParametrosEmpresa!empkey) Then
        UsrKey = ""
    Else                                                ' Clave acceso a la BD
        UsrKey = edC.rsDameParametrosEmpresa!empkey
    End If
    FicheroDSN = "GA_" & edC.rsDameParametrosEmpresa!empdsn     ' Fichero DSN de impresión
    'FicheroDSN = "abg.dsn"
    FicheroDLL = edC.rsDameParametrosEmpresa!empdll     ' Fichero DLL de impresión
    If IsNull(edC.rsDameParametrosEmpresa!emprfc) Then
        wRCFotos = ""
    Else
        wRCFotos = edC.rsDameParametrosEmpresa!emprfc       ' Ruta de fotos para consulta
    End If
    If IsNull(edC.rsDameParametrosEmpresa!emprfi) Then
        wRCFotosImp = ""
    Else
        wRCFotosImp = edC.rsDameParametrosEmpresa!emprfi    ' Ruta de fotos para impresión
    End If
    If IsNull(edC.rsDameParametrosEmpresa!emprin) Then
        wRInformes = ""
    Else
        wRInformes = edC.rsDameParametrosEmpresa!emprin     ' Ruta de los informes
    End If
    If IsNull(edC.rsDameParametrosEmpresa!emplog) Then
        LogoEmpresa = ""
    Else
        LogoEmpresa = edC.rsDameParametrosEmpresa!emplog    ' Fichero jpg de logotipo de la empresa
    End If
    
    ' Guardar el servidor en ini
    'GuardarIni ficIni, "Conexion", "BDDServ", CStr(BDDServ)
    GuardarIni ficINI, "Conexion", "BDDServ", wfQuitarCorchetes(CStr(EmpresaTrabajo.Servidor_RadioFrecuencia))
    
        ' Configuración del servidor de contabilidad
    Contable_BDDServ = wfQuitarCorchetes(edC.rsDameParametrosEmpresa!empcon)        ' Servidor de la empresa
    Contable_BDDGestion = edC.rsDameParametrosEmpresa!empbco     ' Nombre de la BD
    Contable_BDDTime = 30
    
    ' Cadenas de conexion a las BD del servidor
    ConexionGestion = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & UsrBDD & ";Password=" & UsrKey & ";Initial Catalog=" & BDDGestion & ";Data Source= " & BDDServ & ";Connect Timeout=" & BDDTime
    ConexionConfig = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & UsrBDDConfig & ";Password=" & UsrKeyConfig & ";Initial Catalog=" & BDDConfig & ";Data Source= " & BDDServLocal & ";Connect Timeout=" & BDDTime
    
    ConexionRadioFrecuencia = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & _
            EmpresaTrabajo.Usuario_RadioFrecuencia & _
            ";Password=" & EmpresaTrabajo.Clave_RadioFrecuencia & _
            ";Initial Catalog=" & EmpresaTrabajo.Base_RadioFrecuencia & _
            ";Data Source= " & EmpresaTrabajo.Servidor_RadioFrecuencia & _
            ";Connect Timeout=" & BDDTime
    
    ConexionGestionAlmacen = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & _
            EmpresaTrabajo.Usuario_GestionAlmacen & _
            ";Password=" & EmpresaTrabajo.Clave_GestionAlmacen & _
            ";Initial Catalog=" & EmpresaTrabajo.Base_GestionAlmacen & _
            ";Data Source= " & EmpresaTrabajo.Servidor_GestionAlmacen & _
            ";Connect Timeout=" & BDDTime

    ' --
    'ConexionConfig = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & UsrBDDConfig & ";Password=" & UsrKeyConfig & ";Initial Catalog=" & BDDConfig & ";Data Source= " & "ATOSA2000" & ";Connect Timeout=" & BDDTime
    ' --
    ' -- Si hemos cambiado de servidor probamos de nuevo la conexión
    If bMostrarConexion Then
        ' Con el servidor nuevo
        ConexionConfig = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & UsrBDDConfig & ";Password=" & UsrKeyConfig & ";Initial Catalog=" & BDDConfig & ";Data Source= " & BDDServ & ";Connect Timeout=" & BDDTime
        'Restauramos la conexión al servidor local
        ConexionConfig = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & UsrBDDConfig & ";Password=" & UsrKeyConfig & ";Initial Catalog=" & BDDConfig & ";Data Source= " & BDDServLocal & ";Connect Timeout=" & BDDTime
    End If
    
    Otro_CodEmpresa = edC.rsDameParametrosEmpresa!empotr ' -- OTRA EMPRESA
    
    edC.rsDameParametrosEmpresa.Close
    
    ' --- Cargar parametros de la Otra Empresa Coincidente
    On Error Resume Next
        edC.rsDameParametrosEmpresa.Close
    On Error GoTo 0
    edC.DameParametrosEmpresa CInt(Otro_CodEmpresa)
    With edC.rsDameParametrosEmpresa
        If .RecordCount > 0 Then
            Otro_Empresa = !empnom       ' Nombre de la empresa
            Otro_BDDServ = wfQuitarCorchetes(!empser)       ' Servidor de la empresa
            Otro_BDDGestion = !empbdd    ' Nombre de la BD
            Otro_UsrBDD = !empusr        ' Usuario de acceso a la BD
            If IsNull(!empkey) Then
                Otro_UsrKey = ""
            Else                         ' Clave acceso a la BD
                Otro_UsrKey = !empkey
            End If
            Otro_BDDTime = BDDTime
            Otro_ConexionGestion = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & _
                Otro_UsrBDD & ";Password=" & Otro_UsrKey & _
                ";Initial Catalog=" & Otro_BDDGestion & _
                ";Data Source= " & Otro_BDDServ & _
                ";Connect Timeout=" & Otro_BDDTime
        Else
            Otro_Empresa = ""       ' Nombre de la empresa
            Otro_BDDServ = ""       ' Servidor de la empresa
            Otro_BDDGestion = ""    ' Nombre de la BD
            Otro_UsrBDD = ""        ' Usuario de acceso a la BD
            Otro_UsrKey = ""        ' Clave acceso a la BD
            Otro_ConexionGestion = "" ' ---- Conexion
        End If
    End With
    ' ----------------------------------------------------------------
    edC.rsDameParametrosEmpresa.Close
    
    
    
    
    ' --- Cargar parametros de la Contabilidad -------
    Select Case Empresa
        Case "ATOSA"
            Contable_Empresa_Oficial = "AT01"
            Contable_Empresa_TT = "AT02"
        Case "BOYSTOYS"
            Contable_Empresa_Oficial = "BT01"
            Contable_Empresa_TT = "BT02"
        Case "GIEPOOL"
            Contable_Empresa_Oficial = "GI01"
            Contable_Empresa_TT = "GI02"
        Case "DISTRIELITE"
            Contable_Empresa_Oficial = "DI01"
            Contable_Empresa_TT = "DI02"
        Case Else
            Contable_Empresa_Oficial = "099"
            Contable_Empresa_TT = "099"
    End Select
       

    Contable_Conexion = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa" & _
                ";Initial Catalog=" & Contable_BDDGestion & _
                ";Data Source= " & Contable_BDDServ & _
                ";Connect Timeout=" & Contable_BDDTime

    ' ----------------------------------------------------------------
    
    ' Cerrar conexiones
    edC.Config.Close
    Set edC = Nothing
End Sub
   


' Devuelve el grupo de acceso a empresas
' Si tiene varios grupos de acceso elige el primero
' El grupo de Informática tiene acceso a todas las empresas
Private Function DameGrupoEmpresaUsuario(usr As Integer) As Integer

'    On Error Resume Next
'        edC.rsDameGruposPorUsuario.Close
'    On Error GoTo 0
'    edC.DameGruposPorUsuario usr
'    DameGrupoEmpresaUsuario = -1
'
'   If edC.rsDameGruposPorUsuario!usugru1 = 1 Or edC.rsDameGruposPorUsuario!usugru2 = 1 Or _
'       edC.rsDameGruposPorUsuario!usugru3 = 1 Or edC.rsDameGruposPorUsuario!usugru4 = 1 Or _
'       edC.rsDameGruposPorUsuario!usugru5 = 1 Then
'            DameGrupoEmpresaUsuario = 1
'            edC.rsDameGruposPorUsuario.Close
'            Exit Function
'    End If
    
    'Nuevo acceso-----
    With edC.rsDameGruposPorUsuario
        edC.DameGrupoAccesoEmpresas !usugru1, !usugru2, !usugru3, !usugru4, !usugru5
    End With
    If edC.rsDameGrupoAccesoEmpresas.RecordCount > 0 Then
        edC.rsDameGrupoAccesoEmpresas.MoveFirst
        DameGrupoEmpresaUsuario = edC.rsDameGrupoAccesoEmpresas!gregrp
    End If
    edC.rsDameGrupoAccesoEmpresas.Close
    Exit Function
    
End Function

Public Sub LeerDSN()

    Dim ficDsn As String
        
    RutaDSN = "C:\Archivos de programa\Archivos comunes\ODBC\Data Sources\"
    
    ' Busca si existe el fichero
    ficDsn = Dir(RutaDSN & FicheroDSN)
    If ficDsn = "" Then ' No existe lo creamos
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "DRIVER", "SQL Server"
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "UID", CStr(EmpresaTrabajo.Usuario_GestionAlmacen)
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "DATABASE", CStr(EmpresaTrabajo.Base_GestionAlmacen)
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "WSID", CStr(Usuario.Nombre)
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "APP", "Microsoft Open Database Connectivity"
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "SERVER", CStr(EmpresaTrabajo.Servidor_GestionAlmacen)
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "Description", "Conexión ODBC " & App.Title
    Else
        '-------------- Guarda el servidor de la empresa
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "UID", CStr(UsrBDD)
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "DATABASE", CStr(EmpresaTrabajo.Base_GestionAlmacen)
        GuardarIni RutaDSN & FicheroDSN, "ODBC", "SERVER", CStr(EmpresaTrabajo.Servidor_GestionAlmacen)
        '-------------
    End If
    
    
End Sub

' Obtiene el nombre del PC en el que se ejecuta el programa
Public Function nombrePC() As String

    Dim buffer As String
    Dim aux As String
    Dim estado As Long
    
    buffer = String$(255, " ")
    estado = GetComputerName(buffer, 255)
    If estado <> 0 Then
        aux = Trim(Left(buffer, 255))
        nombrePC = Mid(aux, 1, Len(aux) - 1) ' Elimina el último caracter
    End If

End Function

'-- Comprueba en el ficharo INI si puede ejecutar más instancias del programa.
' Pasos:
' 1.- Si es la primera vez
'
Private Function InstanciasPrograma() As Boolean
    Dim instancias As String
    
    InstanciasPrograma = True
    
    ' Comprueba si el programa se encuentra ya en ejecución
    If App.PrevInstance = False Then
        GuardarIni ficINI, "Varios", "Instancias", "0"
    End If

    ' -- Leemos las instancias
    instancias = LeerIni(ficINI, "Varios", "Instancias", "0")
    ' Si no existe las instancias las crea en el INI
    If instancias = "" Then
        GuardarIni ficINI, "Varios", "Instancias", "0"
        instancias = "0"
    End If
    
    ' -- Si se ha superado el limite de instancias cortamos.
    If Usuario.instancias < Int(instancias) + 1 Then
        InstanciasPrograma = False
        Exit Function
    End If
    
    '-- Guardamos una nueva instancia
    GuardarIni ficINI, "Varios", "Instancias", CStr(CInt(instancias) + 1)
    
End Function

