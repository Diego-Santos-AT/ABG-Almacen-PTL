Attribute VB_Name = "GDFunc01"
'*****************************************************************************************
'Programa de Gestión Integral de la Empresa
'
'GDFunc01: Módulo de funciones generales.
'
'*****************************************************************************************
' FUNCIONES:
' CargaMenu     Función para cargar los menús de la aplicación según la opción elegida
' MenuActivo    Función para activar las opciones correspondientes al menú seleccionado
' CambiaModo    Cambia el modo de la barra de botones del modulo principal frmMain
'=========================================================================================
Option Explicit
Public r_menu As New clGenericaRecordset ' -- Recordset Genérico para
                                        '---- almacenar los menus y permisos
                                        '---- a los que tiene acceso el
                                        '---- usuario.
Enum TipoMensaje
    MENSAJE_Informativo = vbInformation '64
    MENSAJE_Grave = vbCritical '16
    MENSAJE_Exclamacion = vbExclamation '48
End Enum

'*****************************************************************************************
' CargaMenu: Función para cargar los menús de la aplicación según la opción elegida
'*****************************************************************************************
Function CargaMenu(ByVal Index As Integer)
    Menu(Index).Nombre = "ABG Almacén RE" 'frmMenu.cmdMenu.TabIndex
    With Menu(Index)
        Select Case Index
            Case CMD_Aduana
                ReDim .Opcion(2)
                .Opcion(0).Formulario = ""
                .Opcion(1).Formulario = ""
            Case CMD_Almacen
                ReDim .Opcion(2)
                .Opcion(0).Formulario = "&Reparto Automático"
                .Opcion(1).Formulario = "&Empaquetado"
        End Select
    End With
End Function
'=========================================================================================

'*****************************************************************************************
' MenuActivo: Función para activar las opciones correspondientes al menú seleccionado
'*****************************************************************************************
'Function MenuActivo(ByVal Index As Integer)
'    Dim i As Integer
'    Dim n As Integer
'
'    With frmMenu
'        .lblMenuActivo(0).Caption = Menu(Index).Nombre
'        'Bucle para todas las posibles opciones del menú: Opciones
'        n = UBound(Menu(Index).Opcion, 1)
'        For i = 0 To 1
'            If i <= n Then
'                .Opcion(i).Caption = Menu(Index).Opcion(i).Formulario
'            Else
'                .Opcion(i).Caption = ""
'            End If
'        Next i
'
'    End With
'End Function
'=========================================================================================

'*****************************************************************************************
' Función CambiaModo:
'               Función para activar/desactivar los botones de la barra de herramientas
'               según el modo de trabajo.
' Parámetros:
'               Modo : Modo de trabajo: MOD_Edición | MOD_Seleccion
' Utilización:
'               Se llama desde los formularios cliente.
'*****************************************************************************************
Function CambiaModo(Modo As Integer)
    'Cambia el Modo del toolbar del Formulario Principal MDI: frmMain
    Dim i As Integer
    With frmMain.tbToolbar.Buttons
        Select Case Modo
            Case MOD_Seleccion
                'Entra en el modo de selección de registros
                .Item(CMD_Primero).Enabled = True
                .Item(CMD_Anterior).Enabled = True
                .Item(CMD_Siguiente).Enabled = True
                .Item(CMD_Ultimo).Enabled = True
                .Item(CMD_Nuevo).Enabled = True
                .Item(CMD_Eliminar).Enabled = True
                .Item(CMD_Deshacer).Enabled = False
                .Item(CMD_Grabar).Enabled = False
                .Item(CMD_Salir).Enabled = True
                .Item(CMD_Pantalla).Enabled = True
                .Item(CMD_Imprimir).Enabled = True
                .Item(CMD_Filtrar).Enabled = True
                .Item(CMD_Buscar).Enabled = True
                
            Case MOD_Edicion
                'Entra en el modo de edición de registros
                .Item(CMD_Primero).Enabled = False
                .Item(CMD_Anterior).Enabled = False
                .Item(CMD_Siguiente).Enabled = False
                .Item(CMD_Ultimo).Enabled = False
                .Item(CMD_Nuevo).Enabled = False
                .Item(CMD_Eliminar).Enabled = False
                .Item(CMD_Deshacer).Enabled = True
                .Item(CMD_Grabar).Enabled = True
                .Item(CMD_Salir).Enabled = False
                .Item(CMD_Pantalla).Enabled = False
                .Item(CMD_Imprimir).Enabled = False
                .Item(CMD_Filtrar).Enabled = False
                .Item(CMD_Buscar).Enabled = False
                
            Case MOD_Todo
                'Activa todos los botones
                For i = 1 To MAX_Botones
                    .Item(i).Enabled = True
                Next i
                
            Case MOD_Nada
                'Descativa todos los botones
                For i = 1 To MAX_Botones
                    .Item(i).Enabled = False
                Next i
                .Item(1).Enabled = True
                .Item(MAX_Botones).Enabled = True
        End Select
    End With
End Function
'=========================================================================================

' --- Funcion para Obtener los Menus a los que tiene acceso el Usuario ----
Function Obtener_Menus()

Dim edMenu As New edConfig    'Nuevo entorno de datos
Dim Codigo, Opcion, posicion As Integer 'Variables para guardar la codificación del menu(111,231,...)
Dim Permiso As Integer
Dim i, j As Integer               'Variables para los FOR
Dim bexiste As Boolean

    Set r_menu = Nothing
    r_menu.Configura_Clase adOpenDynamic, adLockOptimistic, _
                        "GRUPO", adInteger, 5, _
                        "CODIGO_MENU", adInteger, 5, _
                        "OPCION_MENU", adInteger, 5, _
                        "POSICION_MENU", adInteger, 5, _
                        "PERMISO", adInteger, 5, _
                        "MENU", adVarChar, 50, _
                        "FORMULARIO", adVarChar, 50
                        
                        
    ' --- Conexion
    'Conexion = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" & UsrBDD & ";Initial Catalog=" & BDDConfig & ";Data Source= " & BDDServ & ";Connect Timeout=10"
    'edMenu.Config.ConnectionString = Conexion
    'frmMain.sbStatusBar.Panels(1).Text = ConexionConfig
    edMenu.Config.Open ConexionConfig
    'frmMain.sbStatusBar.Panels(1).Text = ""
    ' ---- Usuario Nos da los Menus del usuario entrante --------------------
    edMenu.DameMenusUsuario Usuario.Nombre, CodEmpresa
    ' -----------------------------------------------------------------------
    
    If edMenu.rsDameMenusUsuario.RecordCount > 0 Then
        edMenu.rsDameMenusUsuario.MoveFirst
        
        For i = 0 To edMenu.rsDameMenusUsuario.RecordCount - 1
            bexiste = False
            Codigo = CInt(edMenu.rsDameMenusUsuario!CODIGO_MENU)
            Opcion = CInt(edMenu.rsDameMenusUsuario!OPCION_MENU)
            posicion = CInt(edMenu.rsDameMenusUsuario!POSICION_MENU)
            Permiso = CInt(edMenu.rsDameMenusUsuario!Permiso)
            
            ' -- Comprobacion de si Existe Ya el Menu con Permiso Inferior -
            ' -- en tal caso se elimina y se inserta el nuevo menu ---------
            If r_menu.RecordCount > 0 Then
                r_menu.MoveFirst
                For j = 0 To r_menu.RecordCount - 1
                    If r_menu.Campo(1) = Codigo And _
                       r_menu.Campo(2) = Opcion And _
                       r_menu.Campo(3) = posicion Then
                       
                        If r_menu.Campo(4) < Permiso Then
                            r_menu.Delete
                        Else
                            bexiste = True
                        End If
                        r_menu.MoveLast
                        Exit For
                    End If
                    r_menu.MoveNext
                Next j
            End If
            ' ---------------------------------------------------
            
            If bexiste = False Then
                r_menu.Add edMenu.rsDameMenusUsuario!grupo, _
                        Codigo, _
                        Opcion, _
                        posicion, _
                        Permiso, _
                        edMenu.rsDameMenusUsuario!Menu, _
                        edMenu.rsDameMenusUsuario!Formulario
            End If
            
            edMenu.rsDameMenusUsuario.MoveNext
        Next i
    End If
    ' -----------------------------------------------------------------------
    edMenu.rsDameMenusUsuario.Close
    edMenu.Config.Close
    Set edMenu = Nothing
End Function

'**************************************************************************************
' Prueba si hay conexión con el Servidor que se le pasa por parámetro
'**************************************************************************************
Public Function ProbarConexion(serv As String) As Boolean

    Dim edC As New edConfig
    Dim conexion As String
    
    conexion = "Provider=SQLOLEDB.1;Persist Security Info=False;" & _
               "User ID=" & UsrBDDConfig & ";Password=" & UsrKeyConfig & _
               ";Initial Catalog=" & BDDConfig & _
               ";Data Source= " & serv & _
               ";Connect Timeout=" & BDDTime
    On Error Resume Next
        edC.Config.Close
    On Error GoTo 0
        
    Screen.MousePointer = vbHourglass
    On Error GoTo ErrInicio
        edC.Config.Open conexion
    ProbarConexion = True
    edC.Config.Close
    Set edC = Nothing
    Screen.MousePointer = vbDefault
    Exit Function
ErrInicio:
    ProbarConexion = False
    edC.Config.Close
    Set edC = Nothing
    Screen.MousePointer = vbDefault
End Function

'**************************************************************************************
'Función:   wfCambiarCadena
'Creación:  20/04/04
'Objetivo:  Cambiar en la Cadena que se manda, la Cadena Buscada por la Cadena a Reemplazar
'
'           Ejemplo:    Sustituir un .es por un .com
'
'           Call wfCambiarCadena("jmlopez@abgpool.es", ".es", ".com") -> Salida : "jmlopez@abgpool.com"
'
'Entrada:   vCadenaInicial  =   Cadena en la que sustituir
'           vCadenaBuscada  =   Cadena a buscar para ser sustituida
'           vCadenaReemplazar=  Cadena que se va a Reemplazar por la Buscada
'Salida:    Cadena Inicial con la Cadena Buscada sustituida
'**************************************************************************************
Public Function wfCambiarCadena(vCadenaInicial As Variant, vCadenaBuscada As Variant, vCadenaReemplazar As Variant) As Variant

    Dim lPos As Long
    Dim vCadenaTotal As Variant
    
    vCadenaTotal = vCadenaInicial

    lPos = 1
    While lPos <= Len(vCadenaTotal) And lPos <> 0
        lPos = InStr(lPos, vCadenaTotal, vCadenaBuscada)
        If lPos <= Len(vCadenaTotal) And lPos <> 0 Then
            vCadenaTotal = Mid(vCadenaTotal, 1, lPos - 1) & vCadenaReemplazar & Mid(vCadenaTotal, lPos + Len(vCadenaBuscada))
            lPos = lPos + Len(vCadenaReemplazar)
        End If
    Wend
    wfCambiarCadena = vCadenaTotal
    
End Function

'**************************************************************************************
'Función:   wfComaXPunto
'Creación:  20/04/04
'Objetivo:  Cambiar en la Cadena la Coma por un Punto. Función mas directa y abreviada apoyada en wfCambiarCadena
'
'           Ejemplo:    Sustituir una coma por un punto cuando se realiza un Insert directo
'
'           Call wfComaXPunto("98,234", ",", ".") -> Salida : "98.234"
'
'Entrada:   vCadenaInicial  =   Cadena en la que sustituir la coma por el punto
'Salida:    Cadena Inicial Cambiada com la coma por el punto
'**************************************************************************************
Public Function wfComaXPunto(vCadenaInicial As Variant) As Variant
    
    wfComaXPunto = wfCambiarCadena(vCadenaInicial, ",", ".")

End Function


'**************************************************************************************
'Función:   wfQuitarCorchetes
'Creación:  07/02/06
'Objetivo:  Elimina los corchetes abierto y cerrado, [ ] , de una cadena. Usada para eliminar los corchetes de los nombres
'                   de instancias de los servidores
'
'           Call wfQuitarCorchetes("[HOLA]", ",", ".") -> Salida : "HOLA"
'
'Entrada:   vCadenaInicial  =   Cadena en la que eliminar los cochetes
'Salida:    Cadena Inicial pero sin los cochetes
'**************************************************************************************
Public Function wfQuitarCorchetes(vCadenaInicial As Variant) As Variant
    
    wfQuitarCorchetes = wfCambiarCadena(wfCambiarCadena(vCadenaInicial, "[", ""), "]", "")

End Function


'**************************************************************************************
'Función:   wfPonerComillas
'Creación:  20/04/04
'Objetivo:  Añadir a la Cadena enviada, comillas simples al principio y final de ésta.
'           Si la cadena enviada ya las contiene la devuelve sin modificaciones.
'           Si la cadena enviada contiene comillas simples las sustituye por acentos
'
'           Ejemplo:    Añadir a un texto las comillas para la sintaxis de un insert
'
'           Call wfPonerComillas("Grupo ABG") -> Salida : "'Grupo ABG"
'
'Entrada:   vCadenaInicial  =   Cadena en la que sustituir la coma por el punto
'Salida:    Cadena Inicial Cambiada con Comillas simples al inicio y al final
'**************************************************************************************
Public Function wfPonerComillas(vCadenaInicial As Variant) As Variant
    Dim vCadenaTotal As Variant
    
    vCadenaTotal = Trim(vCadenaInicial)
    
    If vCadenaTotal = "" Then
        vCadenaTotal = "''"
    Else
        'Si el primer y último carácter es ya comillas nos se sigue
        If Len(vCadenaTotal) = 1 Then
            'Primero se sustituyen las posibles comillas simples internas por acentos para evitar error de sintaxis
            vCadenaTotal = wfCambiarCadena(vCadenaTotal, "'", "´")
                
            'Luego se añaden las comillas simples
            vCadenaTotal = "'" & Trim(vCadenaTotal) & "'"
        Else
            If Mid(vCadenaTotal, 1, 1) <> "'" And Mid(vCadenaTotal, Len(vCadenaTotal) - 1) <> "'" Then
            
                'Primero se sustituyen las posibles comillas simples internas por acentos para evitar error de sintaxis
                vCadenaTotal = wfCambiarCadena(vCadenaTotal, "'", "´")
                
                'Luego se añaden las comillas simples
                vCadenaTotal = "'" & Trim(vCadenaTotal) & "'"
            End If
        End If
    End If
    
    wfPonerComillas = vCadenaTotal

End Function

'**************************************************************************************
'Función:   wsMensaje
'Creación:  26/07/04
'Objetivo:  Presentar un Mensaje en Formulario
'
'           Ejemplo:    Mostrar un mensaje de error por que un dato es incorrecto
'
'           Call wsMensaje(" La Tablilla no existe", vbCritical )
'
'Entrada:   stMensaje   =   Cadena con el mensaje
'           vtTipo      = Tipo de presentación del mensaje. Igual que en MsgBox
'Salida:    -
'**************************************************************************************
Public Sub wsMensaje(ByVal stMensaje As String, Optional ByVal vtTipo As TipoMensaje = vbCritical)
        
    frmMensaje.Tipo = vtTipo
    frmMensaje.Texto = stMensaje
    frmMensaje.Show vbModal

End Sub


'**************************************************************************************
'Función:   wsImprimirContenidoCaja
'Creación:  27/07/04
'Objetivo:  Imprimir una etiqueta con el contenido de una caja
'
'
'Entrada:   Grupo =   Cadena con el codigo de grupo
'           Tablilla= Cadena con el codigo de tablilla
'           Caja= Cadena con el codigo de caja
'Salida:    Una etiqueta impresa con la relacion de contenido de la caja
'**************************************************************************************

Public Sub wsImprimirContenidoCajaImpresoraNormal(grupo As String, tablilla As String, caja As String, cReport As CrystalReport)
    Dim stSql
    Dim titulo1 As String
    Dim titulo2 As String
    
    titulo1 = "CONTENIDO CAJA: "
    titulo2 = " RELACION CONTENIDO : " & grupo & " - " & tablilla & " - " & caja
    
    stSql = "select * from galtcaja join gaarticu on (ltcart=artcod) " & _
    " Where ltcgru = " & grupo & " and ltctab = " & tablilla & " and ltccaj = " & caja & " and ltccan > 0"
    
       
    Call wfImprimir_Informe(Array( _
        Array("ReportFileName", "GA_EtiquetasContenidoCaja.rpt"), _
        Array("Destination", 0), _
        Array("ParameterFields(0)", titulo1), _
        Array("ParameterFields(1)", titulo2), _
        Array("ParameterFields(5)", 111), _
        Array("SQLQuery", stSql)), cReport)
End Sub

'**************************************************************************************
'Función:   wfColorTerminacion
'Creación:  27/07/04
'Objetivo:  Obtener los colores para mostrar el grado de terminación de las cantidades
'
'
'Entrada:   vtValor = Porcentaje de Terminación
'Salida:    Color que corresponde
'**************************************************************************************

Public Function wfColorTerminacion(ByVal vtValor As Variant) As Variant
   
    Select Case vtValor
    
        Case 0
            wfColorTerminacion = &HFF&      ' Rojo oscuro
        
        Case Is >= 100
            wfColorTerminacion = &HC000&    ' Verde fuerte
        
        Case Else
            wfColorTerminacion = &H80FF&    ' Ambar
            
    End Select

End Function



'**************************************************************************************
'Función:   ControlEjecucion
'Creación:  26/04/05
'Objetivo:  Controla una marca de ejecución correcta del programa en el archivo ABG.INI junto con el nombre del
'                       programa que se ejecuta. Este procedimiento será válido para todos los programas ABG que hacen uso
'                       del sistema de actualización por CargadorABG, para indicar a este que programa es y que está correcto
'
'Salida:  La marca sera siempre un 1 en el valor EjecucucionCorrecta y el programa será el nombre del ejecutable en el valor
'                       Programa dentro de la sección Versiones para ambos casos

'**************************************************************************************
Public Sub ControlEjecucion()
    Dim nombreEXE As String
    Dim ficINI As String
    On Error Resume Next
    nombreEXE = App.EXEName
    ficINI = App.Path & "\ABG.INI"
    GuardarIni ficINI, "Versiones", "Programa", nombreEXE
    GuardarIni ficINI, "Versiones", "EjecucionCorrecta", "1"
End Sub

'**************************************************************************************
'Función:   ActualizaCargador
'Creación:  26/04/05
'Objetivo:  Actualiza el CargadorABG para mantener una actualización conjunta de ambos programas
'
'Salida:  Actualiza el programa CargadorABG

'**************************************************************************************
Public Sub ActualizaCargador()
    Dim ficINIlocal As String
    Dim ficINIserv As String
    Dim Ruta As String
    Dim serv As String
    Dim version As String
    Dim version_serv As String
    'On Error Resume Next
    
    ficINIlocal = App.Path & "\ABG.INI"
    
    ' Lectura de la ruta de donde están los programas para actualizar el cargador
    serv = LeerIni(ficINIlocal, "Versiones", "APPServ", "")
    Ruta = LeerIni(ficINIlocal, "Versiones", "RutaProgramas", "")
    If serv = "" Then Exit Sub
    If Ruta = "" Then
        Ruta = "\Programas\ABG\Ejecutable\"
        GuardarIni ficINIlocal, "Versiones", "RutaProgramas", Ruta
    End If
        
    ficINIserv = serv & Ruta & "Version.INI"
    
    ' Se leen las versiones del cargador locales y del servidor
    version_serv = LeerIni(ficINIserv, "CargadorABG", "Version", "")
    version = LeerIni(ficINIserv, "Versiones", "Cargador", "")
    
    ' Si las versiones son diferentes hay que actualizar
    If version <> version_serv Then
        FileCopy serv & Ruta & "CargadorABG.EXE", App.Path & "\CargadorABG.EXE"
        GuardarIni ficINI, "Versiones", "Cargador", version_serv
    End If
End Sub



'-----------------------------------------------------------------------------------
'------ Funcion para formatear las columnas de un ultragrid
'-----------------------------------------------------------------------------------
Public Function FormatoColumnaUGrid(ByRef UGrid As SSUltraGrid, iBanda As Integer, sCampo As String, bOculto As Boolean, sNombre As String, iAnchura As Integer, _
                                    iPosicion As Integer, sGrupo As String, sFormato As String, sColor, Editable As Boolean)
    Dim CDF As clsDataFilter
    Set CDF = UGrid.DataFilter
    
    UGrid.Bands(iBanda).Columns(sCampo).Hidden = bOculto
    If sNombre <> "" Then UGrid.Bands(iBanda).Columns(sCampo).Header.Caption = sNombre
    UGrid.Bands(iBanda).Columns(sCampo).Width = iAnchura
    UGrid.Bands(iBanda).Columns(sCampo).Header.VisiblePosition = iPosicion
    If sGrupo <> "" Then
        UGrid.Bands(iBanda).Columns(sCampo).Group = sGrupo
    End If
    'CDF.ColumnFormat(UGrid.Bands(iBanda).Columns(sCampo)) = sFormato
    If sColor <> "" Then UGrid.Bands(iBanda).Columns(sCampo).CellAppearance.BackColor = sColor
    If Editable Then
        UGrid.Bands(iBanda).Columns(sCampo).Activation = ssActivationAllowEdit
    Else
        UGrid.Bands(iBanda).Columns(sCampo).Activation = ssActivationActivateOnly
    End If
    iPosicion = iPosicion + 1   'Incrementa la posiciçon para el siguiente objeto a configurar
    
End Function


