Attribute VB_Name = "GDFunc02"
'*****************************************************************************************
'Programa de Gestión Integral de la Empresa
'
'GDFunc02: Módulo de funciones de relación de datos
'
'*****************************************************************************************
Option Explicit
Private edGdfunc02 As New EntornoDeDatos

'**************************************************************************************
'Función:   wfBuscarEnArray
'Creación:  16/10/03
'Objetivo:  Buscar en un array de dos dimensiones por la primera para devolver el valor de la segunda
'           Se usa desde wfImprimir_Informe para dar el valor de la propiedad de Crystal
'Entrada:   vArray =        Array en el que buscar
'           vValorBuscar =  Valor a buscar en la primera dimensión
'Salida:    Variant =       Valor encontrado inicializado con ""
'**************************************************************************************

Public Function wfBuscarEnArray(vArray, vValorBuscar) As Variant

    Dim lA As Long
    
    'Si no encuentra nada devuelve ""
    wfBuscarEnArray = ""
    
    For lA = LBound(vArray) To UBound(vArray)
    
        If vArray(lA)(0) = vValorBuscar Then
            wfBuscarEnArray = vArray(lA)(1)
            Exit For
        End If
        
    Next lA
    
End Function

'**************************************************************************************
'Función:   wfImprimir_Informe
'Creación:  16/10/03
'Objetivo:  Imprimir un Informe con Crystal Report centralizando la incialización
'           de todas las propiedades comunes a todos los listados, y cambiando
'           aquellas que se mandan en vArrayOpciones.
'           Se apoya en la función wfBuscarEnArray para buscar en el array
'
'           Esta función parte de la información de uno de los report ya existentes, por lo
'           que deberá ir cambiando para incorporar la inicialización de aquellas propiedades
'           de los informes que lo vaya necesitando.
'
'           Ejemplo:
'
'           Impresión del informe "RelacionContenidoTablilla.rpt" con destino a pantalla,
'           mandando el Título, la Selección del Filtro de Datos y el Identificador del informe,
'           así como los tres parámetros que espera el procedimiento almacenado del que depende
'           y con el que está creado el report.
'
'           Call wfImprimir_Informe(Array( _
'                                       Array("ReportFileName", "RelacionContenidoTablilla.rpt"), _
'                                       Array("Destination", 0), _
'                                       Array("ParameterFields(1)", MOD_Nombre), _
'                                       Array("ParameterFields(2)", sSeleccion), _
'                                       Array("ParameterFields(5)", 95), _
'                                       Array("StoredProcParam(0)", nDatos(DAT_Grupo)), _
'                                       Array("StoredProcParam(1)", nDatos(DAT_DesdeTablilla)), _
'                                       Array("StoredProcParam(2)", nDatos(DAT_HastaTablilla))), _
'                                   cReport)
'
'Entrada:   vArrayOpciones  =   Array con las opciones de impresión
'           cReport =   Objeto CrystalReport con el que imprimir
'Salida:    Boolean =   Tipo de Terminación (True = Impresión Correcta, False = Error en Impresión)
'**************************************************************************************
Public Function wfImprimir_Informe(vArrayOpciones, cReport As CrystalReport) As Boolean
    Dim fFecha As Date
    Dim sAuxDLL, sAuxDSN As String
    Dim iIdconexion As Integer
    Dim crInforme As CrystalReport
    Dim vDivisa As Variant
    Dim sSQLQuery As String
    Dim sSelectionFormula As String
    Dim vStoredProcParam As Variant
    Dim lStoredProcParam As Long
    Dim lFormulas As Long
    Dim vFormulas As Variant
    Dim oErrorImpr
    Dim vCopias As Variant
    Dim sTitulo As String
        
    wfImprimir_Informe = False
    
    'Definición de objeto Crystal Report en función del que se manda
    'Inicialmente se pretendía tomar frmMain.cReportMain, pero no refrescaba bien
    Set crInforme = cReport
    On Error Resume Next
    crInforme.Reset
    On Error GoTo 0

    
    '---- Fecha del sistema -------------------------------------
    fFecha = Dame_FechaHora_Sistema

    '---- Conexion ----------------------------------------------
    sAuxDLL = FicheroDLL
    
    sAuxDSN = Dame_DSN
    
    iIdconexion = crInforme.LogOnServer(FicheroDLL, FicheroDSN, "SQL", UsrBDD, UsrKey)
    
    
    '---- Nombre del Informe ------------------------------------
    crInforme.ReportFileName = wRInformes & wfBuscarEnArray(vArrayOpciones, "ReportFileName")
    
    '---- Título de la venta ------------------------------------
    sTitulo = UCase(wfBuscarEnArray(vArrayOpciones, "ParameterFields(1)"))
    crInforme.WindowTitle = sTitulo

    '---- Configuracion de la Vista Preliminar ------------
    crInforme.WindowShowCancelBtn = True
    crInforme.WindowShowCloseBtn = True
    crInforme.WindowShowExportBtn = True
    crInforme.WindowShowNavigationCtls = True
    crInforme.WindowShowPrintBtn = True
    crInforme.WindowShowPrintSetupBtn = True
    crInforme.WindowShowProgressCtls = True
    crInforme.WindowShowRefreshBtn = True
    crInforme.WindowShowSearchBtn = True
    crInforme.WindowShowZoomCtl = True

    '
    '---- Configuracion de los Parametros Fijos Para Todos los Informes -----------
    '
    
    '---- Empresa de Trabajo -----
    crInforme.ParameterFields(0) = "Empresa" & ";" & Empresa & ";true"
    
    '---- Titulo del Informe -----
    crInforme.ParameterFields(1) = "Titulo" & ";" & crInforme.WindowTitle & ";true"
    
    '---- Informacion de Seleccion de Registros del Informe -----
    crInforme.ParameterFields(2) = "Seleccion" & ";" & wfBuscarEnArray(vArrayOpciones, "ParameterFields(2)") & ";true"
    
    '---- Fecha de Impresion -----
    crInforme.ParameterFields(3) = "FechaImpresion" & ";" & fFecha & ";true"
    
    '---- Identificacion de Usuario -----
    crInforme.ParameterFields(4) = "Usuario" & ";" & Usuario.Id & ";true"
    
    '---- Identificacion del Informe -----
    crInforme.ParameterFields(5) = "IdInforme" & ";" & wfBuscarEnArray(vArrayOpciones, "ParameterFields(5)") & ";true"
    
    '---- Divisa -----
    vDivisa = wfBuscarEnArray(vArrayOpciones, "ParameterFields(6)")
    If vDivisa <> "" Then
        crInforme.ParameterFields(6) = "Divisa;" & vDivisa & ";true"
        '---- Fecha -----
        crInforme.ParameterFields(7) = "Fecha;" & crInforme.ParameterFields(3) & ";true"
    End If
    
    '---- Seleccion de la Salida (Impresora o Pantalla) ---------
    Select Case wfBuscarEnArray(vArrayOpciones, "Destination")
        Case 1 'CMD_Impresora
            crInforme.PrinterDriver = Printer.DriverName
            crInforme.PrinterName = Printer.DeviceName
            crInforme.PrinterPort = Printer.port
            vCopias = wfBuscarEnArray(vArrayOpciones, "PrinterCopies")
            vCopias = IIf(vCopias = "", 1, vCopias)
            crInforme.PrinterCopies = vCopias
            crInforme.Destination = crptToPrinter

        Case 0 'CMD_Pantalla
            crInforme.WindowState = crptMaximized
            crInforme.Destination = crptToWindow
    End Select
    
    '---- SQLQuery ------------------------------------
    sSQLQuery = wfBuscarEnArray(vArrayOpciones, "SQLQuery")
    If sSQLQuery <> "" Then
        crInforme.SQLQuery = sSQLQuery
    End If
    
    '---- SelectionFormula------------------------------------
     sSelectionFormula = wfBuscarEnArray(vArrayOpciones, "SelectionFormula")
    If sSelectionFormula <> "" Then
        crInforme.SelectionFormula = sSelectionFormula
    End If
    
    '---- StoredProcParam ------------------------------------
    For lStoredProcParam = 0 To UBound(vArrayOpciones)
        vStoredProcParam = wfBuscarEnArray(vArrayOpciones, "StoredProcParam(" & lStoredProcParam & ")")
        If vStoredProcParam <> "" Then
            crInforme.StoredProcParam(lStoredProcParam) = vStoredProcParam
        End If
    Next lStoredProcParam
    
    '---- Fórmulas ------------------------------------
    For lFormulas = 0 To UBound(vArrayOpciones)
        vFormulas = wfBuscarEnArray(vArrayOpciones, "Formulas(" & lFormulas & ")")
        If vFormulas <> "" Then
            crInforme.Formulas(lFormulas) = vFormulas
        End If
    Next lFormulas
        
    '---- Impresión del Informe ------------------------------------
    oErrorImpr = crInforme.PrintReport
        
    
    '---- Control de errores ------------------------------------
    If oErrorImpr > 0 Then
        Call MsgBox("Error: " & oErrorImpr & ". No se ha podido imprimir: " & crInforme.LastErrorString, vbExclamation, sTitulo)
        wfImprimir_Informe = False
    Else
        wfImprimir_Informe = True
    End If
    
    '--- Limpieza -------------
    On Error Resume Next
    crInforme.Reset
    On Error GoTo 0
    Set crInforme = Nothing
   
End Function

'**************************************************************************************
'Función: VerFoto
'
'Función para mostrar la foto ampliada de un artículo
'
'Llama al formulario frmVerFoto
'
'Creado: 08/02/00
'**************************************************************************************
'Function verFoto(Codigo As String, Nombre As String, Ruta As String)
'    Dim f As frmVerFoto
'    If Codigo <> "" Then
'        If Dir(Ruta & Trim(Codigo) & ".jpg") <> "" Then
'            Set f = New frmVerFoto
'            f.Codigo = Codigo
'            f.Nombre = Nombre
'            f.Ruta = Ruta
'            f.Show vbModal
'            Set f = Nothing
'        Else
'            MsgBox ("No se dispone de Foto")
'        End If
'    End If
'End Function
'**************************************************************************************

Public Sub wsImprimirEtiquetasCajas(ByVal vtDatosEtiquetasCajas As Variant, ByVal blPantalla As Boolean, ByVal cReport As CrystalReport, ByVal MOD_Nombre As String)
    Dim stNombreImpresora As String
    
    stNombreImpresora = Printer.DeviceName
    
    If InStr(1, stNombreImpresora, wImpresora) >= 1 Then
        Select Case wPuestoTrabajo.TipoImpresora
            Case "TEC"
                Call wsImprimirEtiquetasCajasImpresoraTec(vtDatosEtiquetasCajas, blPantalla, cReport, MOD_Nombre)
            
            Case "ZEBRA"
                Call wsImprimirEtiquetasCajasImpresoraZebra(vtDatosEtiquetasCajas, blPantalla, cReport, MOD_Nombre)
                
            Case Else
                Call wsImprimirEtiquetasCajasImpresoraNormal(vtDatosEtiquetasCajas, blPantalla, cReport, MOD_Nombre, 0, "GA_EtiquetasGrupoTablillaCaja.rpt")
        End Select
        
    End If
End Sub

Public Sub wsImprimirPlantillaTablillas(ByVal vtDatosEtiquetasCajas As Variant, ByVal blPantalla As Boolean, ByVal cReport As CrystalReport, ByVal MOD_Nombre As String, ByVal Nombre_Report As Variant)
    Dim stNombreImpresora As String
    
    stNombreImpresora = Printer.DeviceName
    
    If InStr(1, stNombreImpresora, wImpresora) >= 1 Then
        If wPuestoTrabajo.TipoImpresora = "TEC" Then
            MsgBox "Imposible Imprimir este Informe por la Impresora TEC", vbInformation, "Impresion de Informe)"
        Else
            Call wsImprimirEtiquetasCajasImpresoraNormal(vtDatosEtiquetasCajas, blPantalla, cReport, MOD_Nombre, 1, Nombre_Report)
        End If
    End If
End Sub

Public Sub wsImprimirContenidoCaja(nGrupo As String, nTablilla As String, nCaja As String, cReport As Object)
    Dim stNombreImpresora As String
    
    stNombreImpresora = Printer.DeviceName
    
    If InStr(1, stNombreImpresora, wImpresora) >= 1 Then
        Select Case wPuestoTrabajo.TipoImpresora
            Case "TEC"
                Call wsImprimirContenidoCajaImpresoraTec(nGrupo, nTablilla, nCaja)
            Case "ZEBRA"
                Call wsImprimirContenidoCajaImpresoraZebra(nGrupo, nTablilla, nCaja)
            Case Else
                Call wsImprimirContenidoCajaImpresoraNormal(nGrupo, nTablilla, nCaja, cReport)
        End Select
    End If

End Sub

Public Sub wsImprimirEtiquetasCajasImpresoraNormal(ByVal vtDatosEtiquetasCajas As Variant, ByVal blPantalla As Boolean, ByVal cReport As CrystalReport, ByVal MOD_Nombre As String, ByVal Tipo_Informe As Long, ByVal Nombre_Report As Variant)

    Dim stSql As String
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim ilCaja As Long
    Dim rsTemp As Recordset
    
    '--- Nueva conexión para el listado
    On Error Resume Next
    Set cnn = Nothing
    On Error GoTo 0
    Set cnn = New ADODB.Connection
    cnn.Open ConexionGestion
    '----------------------------------
    
    '--- Crear Tabla ---
    stSql = " CREATE TABLE ##tmpEtiquetasGrupoTablillaCaja (" & _
            "[GRUPO] [int] NULL ," & _
            "[TABLILLA] [int] NULL ," & _
            "[CAJA] [int] NULL ," & _
            "[CODIGO_USUARIO] [smallint] ," & _
            "[NOMBRE_USUARIO] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL , " & _
            "[DESCRIPCION_TIPO_CAJA] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL)"


    Set cmd = Nothing
    Set cmd = New ADODB.Command
    cmd.ActiveConnection = cnn
    cmd.CommandText = stSql
    On Error GoTo Err_Creacion
    cmd.Execute , , adCmdText
    On Error GoTo 0
    ' ------------------
    
    ' ---- Rellenarla ---
    Screen.MousePointer = vbHourglass
    
    For ilCaja = 0 To UBound(vtDatosEtiquetasCajas, 2)
        ' ---- Insercion -------
        Set cmd = Nothing
        Set cmd = New ADODB.Command
        stSql = "insert into ##tmpEtiquetasGrupoTablillaCaja values(" & _
                vtDatosEtiquetasCajas(0, ilCaja) & ", " & _
                vtDatosEtiquetasCajas(1, ilCaja) & ", " & _
                vtDatosEtiquetasCajas(2, ilCaja) & ", " & _
                vtDatosEtiquetasCajas(3, ilCaja) & ", " & _
                wfPonerComillas(vtDatosEtiquetasCajas(4, ilCaja)) & ", " & _
                wfPonerComillas(vtDatosEtiquetasCajas(5, ilCaja)) & ")"
    
        cmd.CommandText = stSql
        cmd.CommandType = adCmdText
        cmd.CommandTimeout = 1000
        cmd.Name = "insercion"
        Set cmd.ActiveConnection = cnn
        
        On Error GoTo Err_Insercion
        cmd.Execute
        On Error GoTo 0
    Next

    Screen.MousePointer = vbDefault
    ' ------------------
    
    'Sentencia SQL Indicar el Alias sin ## para que funcione la consulta en el report
    stSql = "select * from ##tmpEtiquetasGrupoTablillaCaja tmpEtiquetasGrupoTablillaCaja where CODIGO_USUARIO = " & Usuario.Id & " order by GRUPO, TABLILLA, CAJA"
    
    
    'Se inicializan temas generales. El código del listado es la secuencia del código del formulario seguido de 1
    
    If Tipo_Informe = 0 Then
        ' --- Listado de Tablillas --
        Call wfImprimir_Informe(Array( _
            Array("ReportFileName", Nombre_Report), _
            Array("Destination", IIf(blPantalla, 0, 1)), _
            Array("ParameterFields(1)", " CAJA PIEZAS SUELTAS "), _
            Array("ParameterFields(2)", " "), _
            Array("ParameterFields(5)", 111), _
            Array("SQLQuery", stSql)), cReport)
    End If
    
    If Tipo_Informe = 1 Then
        ' --- Plantilla Tablilla ----
        Call wfImprimir_Informe(Array( _
            Array("ReportFileName", Nombre_Report), _
            Array("Destination", IIf(blPantalla, 0, 1)), _
            Array("SQLQuery", stSql)), cReport)
    End If
    ' ------------------------------
    
    '--- Eliminacion de la tabla -------
    stSql = "DROP TABLE ##tmpEtiquetasGrupoTablillaCaja"

    Set cmd = Nothing
    Set cmd = New ADODB.Command
    cmd.ActiveConnection = cnn
    cmd.CommandText = stSql
    On Error Resume Next
    cmd.Execute , , adCmdText
    On Error GoTo 0
    '----------------------------------
    
    On Error Resume Next
    rsTemp.Close
    cnn.Close
    Set rsTemp = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    On Error GoTo 0
    Exit Sub

Err_Execute:
    Screen.MousePointer = vbDefault
    ' Notifica al usuario cualquier error resultante tras ejecutar la consulta.
    Call MsgBox("Error número: " & Err.Number & " : " & Err.Description, vbCritical, MOD_Nombre)

   Resume Next

Err_Creacion:
    Screen.MousePointer = vbDefault
    ' Si la tabla existe ya la ha creado otro usuario. Esperar a que la suelte.
    
    ' Leer el usuario
    ' Leer Tabla Tmp
    Set cmd = New ADODB.Command
    stSql = "select CODIGO_USUARIO, NOMBRE_USUARIO top 1 from ##tmpEtiquetasGrupoTablillaCaja"
    cmd.CommandText = stSql
    cmd.CommandType = adCmdText
    cmd.CommandTimeout = 1000
    cmd.Name = "seleccion"
    Set cmd.ActiveConnection = cnn
    Set rsTemp = New ADODB.Recordset
    
    rsTemp.CursorType = adOpenStatic
    On Error GoTo Err_Execute
    cnn.Seleccion rsTemp
    On Error GoTo 0
    
    If rsTemp.RecordCount = 0 Then
        Call MsgBox("Error de creación de tabla.Imposible imprimir.", vbCritical, MOD_Nombre)
        Exit Sub
    End If
    rsTemp.MoveFirst
    
    Call MsgBox("Error de creación de tabla. El informe lo está utilizando el usuario " & _
            rsTemp!CODIGO_USUARIO & " - " & rsTemp!NOMBRE_USUARIO, vbCritical, MOD_Nombre)
            
    On Error Resume Next
    rsTemp.Close
    cnn.Close
    Set rsTemp = Nothing
    Set cmd = Nothing
    Set cnn = Nothing
    On Error GoTo 0
            
    Exit Sub
    
Err_Insercion:
    Screen.MousePointer = vbDefault
    On Error Resume Next
    cnn.Close
    Set cmd = Nothing
    Set cnn = Nothing
    On Error GoTo 0
    
    Call MsgBox("Error de Inserción en la tabla temporal de Informe", vbCritical, MOD_Nombre)
    
End Sub

Public Sub wsImprimirEtiquetasCajasImpresoraTec(ByVal vtDatosEtiquetasCajas As Variant, ByVal blPantalla As Boolean, ByVal cReport As CrystalReport, ByVal MOD_Nombre As String)
    Dim ilCaja As Long

    ' ---- Rellenarla ---
    Screen.MousePointer = vbHourglass
    
    For ilCaja = 0 To UBound(vtDatosEtiquetasCajas, 2)
    
        ' --- Impresion de Etiqueta Una a Una -------------
        Call wsImprimirEtiquetaCajaImpresoraTec(vtDatosEtiquetasCajas(0, ilCaja), _
            vtDatosEtiquetasCajas(1, ilCaja), _
            vtDatosEtiquetasCajas(2, ilCaja), _
            vtDatosEtiquetasCajas(5, ilCaja), _
            vtDatosEtiquetasCajas(6, ilCaja))
        ' -------------------------------------------------
    Next

    Screen.MousePointer = vbDefault
    
    ' -- Impresion de Documento ---
    Printer.EndDoc
    ' ------------------
    
    
End Sub

Public Sub wsImprimirEtiquetaCajaImpresoraTec(ByVal grupo As Variant, ByVal tablilla As Variant, Num_Caja As Variant, Descripcion_Caja As Variant, SSCC As Variant)
    
    Dim stBarras As String
    Dim num_copias As Long

    ' --- Numero de Copias ----
    num_copias = 1
    ' -------------------------
    ' -- ALM 007 CAJA PIEZAS SUELTAS - XXXXX GRUPO - XX TABILLA  - XX CAJA  -----
    'stBarras = "007" & Format(CLng(grupo), "00000") & Format(CLng(tablilla), "00") & Format(CLng(Num_Caja), "00")

    ' ---- SSCC
    stBarras = "00" & Strings.Left(SSCC, 17)
    
    ' Dimensiones de la etiqueta
    Printer.Print "{D1330,1000,1220|}"
    
    ' Ajuste fino de la posicion de impresion
    Printer.Print "{AX;+000,+000,+00|}"
    
    ' Ajuste fino de la densidad de impresion
    ' (Corresponde a la temperatura de la impresora en el cabezal)
    '  Valores:     +00  Menos Calor
    '               +05
    '               +10  Mayor Calor
    Printer.Print "{AY;+00,0|}"
    
    ' Borrado de memoria
    Printer.Print "{C|}"


    'Rectangulos
    Printer.Print "{LC;0050,0000,0940,0130,1,3|}"
    Printer.Print "{LC;0050,0130,0500,0830,1,3|}"
    Printer.Print "{LC;0500,0130,0940,0450,1,3|}"
    Printer.Print "{LC;0500,0450,0940,0830,1,3|}"
    
    
    ' Formatos
    
        '----- VECTORIALES (INDEPENDIZA EL TIPO DE IMPRESORA ----------
    Printer.Print "{PV00;0205,0060,0035,0035,B,00,B|}"   'Etiqueta Cajas Piezas Sueltas: Empresa
    Printer.Print "{PV01;0340,0120,0052,0052,B,00,B|}"   'Empresa
    
    Printer.Print "{PV02;0100,0250,0052,0052,B,00,B|}"   'Etiqueta Tablilla
    Printer.Print "{PV03;0100,0680,0305,0305,B,00,B|}"   'Tablilla
    
    Printer.Print "{PV04;0510,0250,0052,0052,B,00,B|}"   'Etiqueta Grupo
    Printer.Print "{PV05;0500,0375,0125,0125,B,00,B|}"   'Grupo

    
    Printer.Print "{PV06;0550,0550,0052,0052,B,00,B|}"   'Etiqueta Nº Caja
    Printer.Print "{PV07;0550,0675,0125,0125,B,00,B|}"   'Caja
    Printer.Print "{PV08;0510,0725,0022,0022,A,00,B|}"   'Descripcion de La Caja

    'Printer.Print "{PV09;0200,1200,0052,0052,A,00,B|}"   'CODIGO DE BARRAS
    Printer.Print "{PV09;0080,1200,0040,0040,A,00,B|}"   'CODIGO DE BARRAS
    
    'Valores

    '----- VECTORIALES (INDEPENDIZA EL TIPO DE IMPRESORA ----------
    Printer.Print "{RV00;CAJA PIEZAS SUELTAS |}"
    Printer.Print "{RV01;" & EmpresaTrabajo.Nombre & "|}"    'Valor de Empresa
    Printer.Print "{RV02;TABLILLA: |}"
    Printer.Print "{RV03;" & CLng(tablilla) & "|}"     'Valor de Tablilla
    Printer.Print "{RV04;GRUPO: |}"
    Printer.Print "{RV05;" & CLng(grupo) & "|}"     'Valor de Grupo
    Printer.Print "{RV06;NO. CAJA: |}"
    Printer.Print "{RV07;" & CLng(Num_Caja) & "|}"     'Valor de Nº Caja
    Printer.Print "{RV08;" & Descripcion_Caja & "|}"     'Descripcion de la Caja
    
    ' ----- NUMERO DEL CB o NUMERO SSCC ----------------
    'Printer.Print "{RV09;" & stBarras & "|}"            'NUMERO DEL CB
    Printer.Print "{RV09;(00)" & SSCC & "|}"          'NUMERO SSCC
    ' --------------------------------------------------

    ' --- Codigo de Barras SSCC -----
    Printer.Print "{XB00;0110,0850,N,1,04,0,0280,+0000000000,020,0,00|}"
    Printer.Print "{RB00;" & stBarras & "|}"
    ' -----------------------------
        
    ' ----------------------------------------------------------

    ' Ajuste fino de la tension de los motores de ribbon
    Printer.Print "{RM;-00-00|}"
                    
    ' Impresion
    ' (En este parametro se marca la velocidad de impresion,
    ' por ejemplo, "0012C6101" El valor 6 es la velocidad en
    ' pulgadas por segundo.
    ' Posibles valores: 1 a 10
    Printer.Print "{XS;I," & Format(num_copias, "0000") & ",0012C3101|}"
    
End Sub

Public Sub wsImprimirEtiquetasCajasImpresoraZebra(ByVal vtDatosEtiquetasCajas As Variant, ByVal blPantalla As Boolean, ByVal cReport As CrystalReport, ByVal MOD_Nombre As String)
    Dim ilCaja As Long

    ' ---- Rellenarla ---
    Screen.MousePointer = vbHourglass
    
    For ilCaja = 0 To UBound(vtDatosEtiquetasCajas, 2)
    
        ' --- Impresion de Etiqueta Una a Una -------------
        Call wsImprimirEtiquetaCajaImpresoraZebra(vtDatosEtiquetasCajas(0, ilCaja), _
            vtDatosEtiquetasCajas(1, ilCaja), _
            vtDatosEtiquetasCajas(2, ilCaja), _
            vtDatosEtiquetasCajas(5, ilCaja), _
            vtDatosEtiquetasCajas(6, ilCaja))
        ' -------------------------------------------------
    Next

    Screen.MousePointer = vbDefault
    
    ' -- Impresion de Documento ---
    Printer.EndDoc
    ' ------------------
    
    
End Sub


Public Sub wsImprimirEtiquetaCajaImpresoraZebra(ByVal grupo As Variant, ByVal tablilla As Variant, Num_Caja As Variant, Descripcion_Caja As Variant, SSCC As Variant)
    
    Dim stBarras As String

    ' -------------------------
    ' -- ALM 007 CAJA PIEZAS SUELTAS - XXXXX GRUPO - XX TABILLA  - XX CAJA  -----
    'stBarras = "007" & Format(CLng(grupo), "00000") & Format(CLng(tablilla), "00") & Format(CLng(Num_Caja), "00")

    ' ---- SSCC
    stBarras = "00" & Strings.Left(SSCC, 18)
    
    ' Cabecera de la etiqueta ---------------------
    Printer.Print "^XA"
    Printer.Print "^MMT"
    Printer.Print "^PW1205"
    Printer.Print "^LL1512"
    Printer.Print "^LS0"
    
    'Lineas ---------------------------------------
    Printer.Print "^FO65,45^GB1077,147,5^FS"
    Printer.Print "^FO65,213^GB1078,821,5^FS"
    Printer.Print "^FO560,213^GB0,821,5^FS"
    Printer.Print "^FO565,597^GB577,0,5^FS"
    
    'Textos Fijos ---------------------------------
    Printer.Print "^FT0,301^A0N,67,63^FB598,1,17,C^FH\^CI28^FDTABLILLA:^FS^CI27"    'TABLILLA
    Printer.Print "^FT606,301^A0N,67,63^FH\^CI28^FDGRUPO:^FS^CI27"                  'GRUPO
    Printer.Print "^FT606,688^A0N,67,63^FH\^CI28^FDNUM. CAJA:^FS^CI27"              'NUM. CAJA
    
    
    'Valores --------------------------------------
    Printer.Print "^FT0,147^A0N,92,96^FB1205,1,24,C^FH\^CI28^FD" & EmpresaTrabajo.Nombre & "^FS^CI27"   'EMPRESA
    Printer.Print "^FT605,549^A0N,200,200^FH\^CI28^FD" & CLng(grupo) & "^FS^CI27"                       'GRUPO
    Printer.Print "^FT0,855^A0N,405,428^FB698,1,104,C^FH\^CI28^FD" & CLng(tablilla) & "^FS^CI27"        'TABLILLA
    Printer.Print "^FT740,933^A0N,200,200^FH\^CI28^FD" & CLng(Num_Caja) & "^FS^CI27"                    'NUMERO DE CAJA
    Printer.Print "^FT604,1011^A0N,33,33^FH\^CI28^FD" & Descripcion_Caja & "^FS^CI27"                   'TIPO DE CAJA
    
    'Texto de SSCC --------------------------------
    Printer.Print "^FT338,1469^A0N,42,46^FH\^CI28^FD" & "(00)" & SSCC & "^FS^CI27"       'TEXTO DE SSCC
    
    'Código de barras -----------------------------
    'Printer.Print "^BY4,3,316^FT106,1405^BCN,,N,N,N"                'Configuraión del código de barras
    Printer.Print "^BY5,3,316^FT106,1405^BRN,12,5,1,60,22"            'Configuraión del código de barras GS1/EAN-128
    Printer.Print "^FH\^FD" & stBarras & "^FS"                      'Código de barras
    
    Printer.Print "^PQ1,0,1,Y"                                      'Número de etiquetas
    
    'Fin de la impresión
    Printer.Print "^XZ"

End Sub

Public Sub wsImprimirContenidoCajaImpresoraTec(grupo As String, tablilla As String, caja As String)
    
    Dim ed As New EntornoDeDatos
    Dim stSql As String
 
    Dim stLinea As String
    Dim nContador As Integer    ' Contador de impresiones por linea (cada 2 incrementos es 1 linea)
    Dim rs As Recordset

    
    ed.GestionAlmacen.Open
    stSql = "select * from galtcaja join gaarticu on (ltcart=artcod) " & _
    " Where ltcgru = " & grupo & " and ltctab = " & tablilla & " and ltccaj = " & caja & " and ltccan > 0"
    Set rs = ed.GestionAlmacen.Execute(stSql)
    
    If rs.RecordCount = 0 Then Exit Sub
    
        ' ---- Rellenarla ---
    Screen.MousePointer = vbHourglass
    
    Call ImprimirCabeceraContenidoTEC(grupo, tablilla, caja)
        
    ' Bucle de impresion de lineas
    rs.MoveFirst
    nContador = 1
    While Not rs.EOF
        ' Linea de código y descripcion
        'stlinea = Format(Int(rs!artcod), "0,000") & "   " & Left(wfCambiarCadena(rs!artnom, "/", "-"), 35)
        stLinea = Format(Int(rs!artcod), "0,000") & "  " & Left(wfCambiarCadena(rs!artnom, "/", "-"), 20)
            '----- MATRICIALES ---------------------------------------------
        'stlinea = "{PC" & Format(nContador + 1, "000") & ";0055," & Format(145 + (25 * nContador), "0000") & ",1,1,R,00,B=" & stlinea & "|}"
            '----- VECTORIALES (INDEPENDIZA EL TIPO DE IMPRESORA) ----------
        stLinea = "{PV" & Format(nContador + 1, "00") & ";0055," & Format(145 + (25 * nContador), "0000") & ",0035,0035,B,00,B=" & stLinea & "|}"
        
        Printer.Print stLinea 'Etiqueta Cajas Piezas Sueltas: Empresa
        nContador = nContador + 1
        
        ' Linea con la cantidad
        stLinea = rs!ltccan
            '----- MATRICIALES ---------------------------------------------
        'stlinea = "{PC" & Format(nContador + 1, "000") & ";0835," & Format(145 + (25 * (nContador - 1)), "0000") & ",1,1,R,00,B=" & stlinea & "|}"
            '----- VECTORIALES (INDEPENDIZA EL TIPO DE IMPRESORA) ----------
        stLinea = "{PV" & Format(nContador + 1, "00") & ";0835," & Format(145 + (25 * (nContador - 1)), "0000") & ",0035,0035,B,00,B=" & stLinea & "|}"
        
        Printer.Print stLinea 'Etiqueta Cajas Piezas Sueltas: Empresa
        nContador = nContador + 1
        rs.MoveNext
        ' Fin de la etiqueta y se comienza en una nueva
        If nContador = 39 Then
            Call ImprimirFinalContenidoTEC(nContador)
            Call ImprimirCabeceraContenidoTEC(grupo, tablilla, caja)
            nContador = 1
        End If
    Wend
    
'    ' -- Imprimios el peso total de la caja - Se quita la impresión del peso: 29/03/2023
'    stSql = "select ltcpes from galtcati where ltcgru = " & grupo & " and ltctab = " & tablilla & " and ltccaj = " & caja
'    Set rs = ed.GestionAlmacen.Execute(stSql)
'    If rs.RecordCount > 0 Then
'
'        stLinea = "PESO APROX. CAJA: " & Format(rs!ltcpes, "0.00") & " kg"
'            '----- VECTORIALES (INDEPENDIZA EL TIPO DE IMPRESORA) ----------
'        stLinea = "{PV" & Format(nContador + 1, "00") & ";0055," & Format(145 + (25 * nContador), "0000") & ",0035,0035,B,00,B=" & stLinea & "|}"
'
'        Printer.Print stLinea 'Etiqueta Cajas Piezas Sueltas: Empresa
'        nContador = nContador + 1
'
'    End If
    
    rs.Close
    ed.GestionAlmacen.Close
    
    ImprimirFinalContenidoTEC (nContador)
    
    Screen.MousePointer = vbDefault
End Sub

Private Sub ImprimirCabeceraContenidoTEC(grupo As String, tablilla As String, caja As String)
   Dim stCabecera As String
       ' Dimensiones de la etiqueta
    Printer.Print "{D1330,1000,1220|}"
    ' Ajuste fino de la posicion de impresion
    Printer.Print "{AX;+000,+000,+00|}"
    ' Ajuste fino de la densidad de impresion
    ' (Corresponde a la temperatura de la impresora en el cabezal)
    '  Valores:     +00  Menos Calor
    '               +05
    '               +10  Mayor Calor
    Printer.Print "{AY;+00,0|}"

    ' Borrado de memoria
    Printer.Print "{C|}"
    
        ' Formatos y texto de cabecera
    stCabecera = "RELACION CONTENIDO " & EmpresaTrabajo.Nombre
        '----- MATRICIALES ---------------------------------------------
    'Printer.Print "{PC000;0060,0050,1,1,R,00,B=" & stCabecera & "|}"   'Etiqueta Cajas Piezas Sueltas: Empresa
        '----- VECTORIALES (INDEPENDIZA EL TIPO DE IMPRESORA) ----------
    Printer.Print "{PV00;0060,0050,0035,0035,B,00,B=" & stCabecera & "|}"   'Etiqueta Cajas Piezas Sueltas: Empresa
    
    stCabecera = "GRUPO-TAB.-CAJA: " & grupo & " - " & tablilla & " - " & caja
    
        '----- MATRICIALES ---------------------------------------------
    'Printer.Print "{PC001;0065,0100,1,1,R,00,B=" & stCabecera & "|}"   'Etiqueta Cajas Piezas Sueltas: Empresa
            '----- VECTORIALES (INDEPENDIZA EL TIPO DE IMPRESORA) ----------
    Printer.Print "{PV01;0065,0100,0035,0035,B,00,B=" & stCabecera & "|}"   'Etiqueta Cajas Piezas Sueltas: Empresa
        
    'Rectangulos
        Printer.Print "{LC;0050,0000,0940,0130,1,3|}"
End Sub

Private Sub ImprimirFinalContenidoTEC(lineas_largo As Integer)
    Dim num_copias As Long
    num_copias = 1
    
    ' Rectangulo exterior
    Printer.Print "{LC;0050,0132,0940," & Format(125 + (25 * lineas_largo), "0000") & ",1,3|}"
    
    ' Ajuste fino de la tension de los motores de ribbon
    Printer.Print "{RM;-00-00|}"
    ' Impresion
    ' (En este parametro se marca la velocidad de impresion,
    ' por ejemplo, "0012C6101" El valor 6 es la velocidad en
    ' pulgadas por segundo.
    ' Posibles valores: 1 a 10
    Printer.Print "{XS;I," & Format(num_copias, "0000") & ",0012C3101|}"
        
        ' -- Impresion de Documento ---
    Printer.EndDoc
End Sub

Public Sub wsImprimirContenidoCajaImpresoraZebra(grupo As String, tablilla As String, caja As String)
    Dim ed As New EntornoDeDatos
    Dim stSql As String
    
    Dim stLinea As String       ' Texto de la línea
    Dim nContador As Integer    ' Contador de lineas
    Dim yLinea As Integer       ' Posición de escritura de la línea (incremento por línea eje y)
    
    Dim rs As Recordset

    
    ed.GestionAlmacen.Open
    stSql = "select * from galtcaja join gaarticu on (ltcart=artcod) " & _
    " Where ltcgru = " & grupo & " and ltctab = " & tablilla & " and ltccaj = " & caja & " and ltccan > 0"
    Set rs = ed.GestionAlmacen.Execute(stSql)
    
    If rs.RecordCount = 0 Then Exit Sub
    
        ' ---- Rellenarla ---
    Screen.MousePointer = vbHourglass
    
    Call ImprimirCabeceraContenidoZEBRA(grupo, tablilla, caja)
        
    ' Bucle de impresion de lineas
    rs.MoveFirst
    nContador = 1
    
    While Not rs.EOF
        'Calculo de la línea
        yLinea = 212 + (nContador * 58)
    
        ' Linea de código y descripcion
        stLinea = Format(Int(rs!artcod), "0,000")
        Printer.Print "^FT67," & yLinea & "^A0N,46,43^FH\^CI28^FD" & stLinea & "^FS^CI27"       'Codigo
        
        stLinea = Left(wfCambiarCadena(rs!artnom, "/", "-"), 20)
        Printer.Print "^FT264," & yLinea & "^A0N,46,43^FH\^CI28^FD" & stLinea & "^FS^CI27"     'Nombre
        
        stLinea = rs!ltccan
        Printer.Print "^FT1028," & yLinea & "^A0N,46,43^FH\^CI28^FD" & stLinea & "^FS^CI27"    'Cantidad
            
        nContador = nContador + 1
        rs.MoveNext
        
        ' Fin de la etiqueta y se comienza en una nueva
        If nContador = 20 Then
            Call ImprimirFinalContenidoZEBRA(nContador)
            Call ImprimirCabeceraContenidoZEBRA(grupo, tablilla, caja)
            nContador = 1
        End If
    Wend
    
    rs.Close
    ed.GestionAlmacen.Close
    
    ImprimirFinalContenidoZEBRA (nContador)
    
    Screen.MousePointer = vbDefault
End Sub

Private Sub ImprimirCabeceraContenidoZEBRA(grupo As String, tablilla As String, caja As String)
   Dim stCabecera As String
       
    ' Cabecera de la etiqueta ---------------------
    Printer.Print "^XA"
    Printer.Print "^MMT"
    Printer.Print "^PW1205"
    Printer.Print "^LL1512"
    Printer.Print "^LS0"
       
    ' Lineas --------------------------------------
    Printer.Print "^FO67,197^GB1078,0,5^FS"
    Printer.Print "^FO39,40^GB1132,1441,5^FS"
        
    'Textos fijos ---------------------------------
    Printer.Print "^FT67,110^A0N,58,56^FH\^CI28^FDRELACION DE CONTENIDO ^FS^CI27"
    Printer.Print "^FT67,175^A0N,50,48^FH\^CI28^FDGRU/TAB/CAJ:^FS^CI27"
    
    'Textos variables -----------------------------
    Printer.Print "^FT759,110^A0N,58,56^FH\^CI28^FD" & EmpresaTrabajo.Nombre & "^FS^CI27"   'Nombre empresa
    stCabecera = grupo & " - " & tablilla & " - " & caja
    Printer.Print "^FT407,175^A0N,50,48^FH\^CI28^FD" & stCabecera & "^FS^CI27"              'Grupo - Tablilla - Caja
    
End Sub

Private Sub ImprimirFinalContenidoZEBRA(lineas_largo As Integer)
    
    'Fin de la etiqueta
    Printer.Print "^PQ1,0,1,Y"
    Printer.Print "^XZ"
        
    ' Impresion de Documento ---
    Printer.EndDoc
End Sub

'**************************************************************************************
'Función:   wfValidarEstadoGrupo
'Creación:  23/11/04
'Objetivo:  Validar el Estado de un Grupo
'Entrada:   stDonde =               Donde se tiene que validar el Estado del Grupo
'           ilGrupo =               Grupo a validar
'           stCodigoEstado =        Estado actual del grupo a contrastar con "donde" esta
'           stDescripcionEstado =   Descripción del Estado del Grupo
'           stFinEstadoGrupo =      Si es Estado Final de Grupo
'           blMensaje =             Si se quiere mensaje informativo o no
'Salida:    Validado o no
'**************************************************************************************

Public Function wfValidarEstadoGrupo(ByVal ed As EntornoDeDatos, ByVal stDonde As String, Optional ilGrupo As Long = 0, Optional ByVal stCodigoEstado As String = "", Optional ByVal stDescripcionEstado As String = "", Optional ByVal stFinEstadoGrupo As String = "-1", Optional ByVal blMensaje As Boolean = True) As Boolean
Dim sSql As String
Dim rsTemp As Recordset
Dim blSeguir As Boolean

wfValidarEstadoGrupo = False
blSeguir = True

Select Case stDonde

    ' El Estado debe ser correcto, es decir, no ser Final y permitir Asignación
    Case "ASIGNAR_ARTICULO_TABLILLA", "ASOCIAR_UNIDADES_TRANSPORTE"
        'Si no se manda el estado hay que averiguarlo del Grupo
        If stCodigoEstado = "" Then
        
            blSeguir = False
        
            sSql = "select * from gacgrupo with ( index = gacgrupo_cgrcod, nolock) left join gaestgru with ( index = gaestgru_estcod, nolock) on ( cgrest = estcod) where cgrcod = " & ilGrupo
            
            On Error Resume Next
            Set rsTemp = ed.GestionAlmacen.Execute(sSql)
            On Error GoTo 0
        
            'Existencia del registro
            If rsTemp.RecordCount > 0 Then
            
                stCodigoEstado = rsTemp!estcod
                stDescripcionEstado = rsTemp!estdes
                stFinEstadoGrupo = rsTemp!estfin
                
                blSeguir = True
            Else
                If blMensaje Then Call wsMensaje(" No existe el Grupo " & ilGrupo, vbCritical)
            End If
        End If
        
        If blSeguir Then
            If stFinEstadoGrupo = "0" Then
                'Sólo lo valida si está Iniciado
                If stCodigoEstado = EstadoGrupo_Iniciado Then
                    wfValidarEstadoGrupo = True
                Else
                    If blMensaje Then Call wsMensaje(" El Grupo está en Estado " & stDescripcionEstado & ", que no permite su modificación ", vbCritical)
                End If
            Else
                If blMensaje Then Call wsMensaje(" El Grupo " & ilGrupo & ", está en estado " & stDescripcionEstado & ", que no permite su modificación ", vbCritical)
            End If
        End If
    
End Select
End Function

'**************************************************************************************
'Función:   wfCrearAsignacion
'Creación:  26/11/04
'Objetivo:  Crear la asociación del Bac a la Tablilla
'Entrada:   ed =            Entorno de datos
'           ilGrupo =       Grupo
'           ilTablilla =    Tablilla
'           stBac =        Bac a asociar
'Salida:    Asociado o no
'**************************************************************************************
Public Function wfCrearAsignacion(ed As EntornoDeDatos, ByVal ilGrupo As Long, ByVal ilTablilla As Long, ByVal stBac As String) As Boolean
    On Error GoTo ControlError
    Dim stSql As String
    Dim rsTemp As Recordset
    
    wfCrearAsignacion = False
    
    ' Comprobación de la existencia de la tablilla
    stSql = "SELECT * FROM GACTABLI WHERE CTAGRU=" & ilGrupo & " AND CTATAB=" & ilTablilla
    Set rsTemp = ed.GestionAlmacen.Execute(stSql)
    If rsTemp.RecordCount = 0 Then GoTo ErrorDatos
    
    ' Asignación del BAC a la tablilla
    ed.AsignacionBacATablilla stBac, ilGrupo, ilTablilla, Usuario.Id

    Call wsMensaje(" Asociada la tablilla " & ilTablilla & " del grupo " & ilGrupo & " al BAC " & stBac, vbInformation)
    wfCrearAsignacion = True
    Exit Function
    
' Error general del sistema
ControlError:
    Call wsMensaje(" Error de asignacion del bac: " & stBac, vbCritical)
    Exit Function
    
' Error informativo por falta de la tablilla
ErrorDatos:
    Call wsMensaje(" La tablilla " & ilTablilla & " del grupo " & ilGrupo & " no existe. No se puede crear la asignación", vbCritical)
End Function

'**************************************************************************************
'Función:   wfCrearAsignacionArticuloCantidad
'Creación:  26/11/04
'Objetivo:  Crear la asociación del Bac a la Tablilla e inserta el detalle del BAC
'Entrada:   ed =            Entorno de datos
'           ilGrupo =       Grupo
'           ilTablilla =    Tablilla
'           iArticulo = Articulo
'           iCantidad = Cantidad asociacada
'           stBac =        Bac a asociar
'Salida:    Asociado o no
'**************************************************************************************
Public Function wfCrearAsignacionArticuloCantidad(ed As EntornoDeDatos, ByVal ilGrupo As Long, ByVal ilTablilla As Long, ByVal iArticulo As Long, ByVal iCantidad As Integer, ByVal stBac As String) As Boolean
    On Error GoTo ControlError
    Dim stSql As String
    Dim rsTemp As Recordset
    Dim sRespuesta As String
    
    wfCrearAsignacionArticuloCantidad = False
    
    ' Comprobación de la existencia de la tablilla
    stSql = "SELECT * FROM GACTABLI WHERE CTAGRU=" & ilGrupo & " AND CTATAB=" & ilTablilla
    Set rsTemp = ed.GestionAlmacen.Execute(stSql)
    If rsTemp.RecordCount = 0 Then GoTo ErrorDatos
    
    ' Asignación del BAC a la tablilla
    ed.AsignacionBacATablilla stBac, ilGrupo, ilTablilla, Usuario.Id
    
    ' Inserción del detalle del BAC
    ed.InsertaDetalleBac stBac, ilGrupo, ilTablilla, iArticulo, iCantidad, Usuario.Id

    ' --- Inserta el LOG --------------------
    ed.InsertaLogEmpaquetado ilGrupo, ilTablilla, 1, "000", iArticulo, iCantidad, 2, stBac, 0, "", "Asignación de Articulo a BAC", wPuestoTrabajo.Id, Usuario.Id

    Call wsMensaje(" Asociada la tablilla " & ilTablilla & " del grupo " & ilGrupo & " al BAC " & stBac, vbInformation)
    wfCrearAsignacionArticuloCantidad = True
    Exit Function
    
' Error general del sistema
ControlError:
    Call wsMensaje(" Error de asignacion del bac: " & stBac, vbCritical)
    Exit Function
    
' Error informativo por falta de la tablilla
ErrorDatos:
    Call wsMensaje(" La tablilla " & ilTablilla & " del grupo " & ilGrupo & " no existe. No se puede crear la asignación", vbCritical)
End Function

'**************************************************************************************
'Función:   wfValidarBAC
'Creación:  29/05/2012
'Objetivo:  Funcion para Validar la Existencia de un BAC en el maestro de BAC
'Entrada:   ed =            Entorno de datos
'           vtBAC =         Codigo de BAC
'           blMensaje =     Booleano para mostrar o no mensajes de error.
'
'Salida:    Valido o NO
'**************************************************************************************
Public Function wfValidarBAC(ed As EntornoDeDatos, vtBAC As Variant, Optional ByVal blMensaje As Boolean = True) As Boolean

Dim sSql As String
Dim rsTemp As Recordset

    wfValidarBAC = False

    sSql = "SELECT * FROM GAUBIBAC WHERE UBIBAC = '" & vtBAC & "'"
    
    On Error Resume Next
    Set rsTemp = ed.GestionAlmacen.Execute(sSql)
    On Error GoTo 0

    'Existencia del registro
    If Not rsTemp Is Nothing Then
        If rsTemp.RecordCount > 0 Then
            wfValidarBAC = True
        Else
            wfValidarBAC = False
        End If
    Else
        wfValidarBAC = False
    End If
    If blMensaje Then Call wsMensaje(" No existe el BAC " & vtBAC, vbCritical)
    '--------------------------------------------------------------------------------------------

    Set rsTemp = Nothing

End Function
'**************************************************************************************
'Función:   wfValidarEAN13
'Creación:  09/02/2015
'Objetivo:  Funcion para Validar la Existencia de un EAN13 o UPC12 en el maestro de Artículos
'Entrada:   ed =            Entorno de datos
'           vtEAN13 =         Codigo de BAC
'           blMensaje =     Booleano para mostrar o no mensajes de error.
'
'Salida:    Valido o NO
'**************************************************************************************
Public Function wfValidarEAN13(ed As EntornoDeDatos, vtEAN13 As Variant, Optional ByVal blMensaje As Boolean = True) As Boolean

Dim sSql As String
Dim rsTemp As Recordset

    wfValidarEAN13 = False

    sSql = "select * from gaarticu  with ( index = gaarticu_artean, nolock)  where artean = '" & vtEAN13 & "'"
    
    On Error Resume Next
    Set rsTemp = ed.GestionAlmacen.Execute(sSql)
    On Error GoTo 0

    'Existencia del registro
    If Not rsTemp Is Nothing Then
        If rsTemp.RecordCount > 0 Then
            wfValidarEAN13 = True
        Else
            wfValidarEAN13 = False
        End If
    Else
        wfValidarEAN13 = False
    End If
    If blMensaje Then Call wsMensaje("Error Ean13: " & " " & vtEAN13, vbCritical)
    '--------------------------------------------------------------------------------------------

    Set rsTemp = Nothing

End Function


'**************************************************************************************
'Función:   ComprobarImpresora_wImpresora
'Creación:  27/12/2019
'Objetivo:  Función para Comprobar si existe la impresora wImpresora.
'           Si existe la pone como predeterminada.
'
'Salida:    Si o No
'**************************************************************************************
Public Function ComprobarImpresora_wImpresora() As Boolean
    Dim tPrinter As Printer
    
    ComprobarImpresora_wImpresora = False
    
    For Each tPrinter In Printers
        If InStr(1, UCase(tPrinter.DeviceName), wImpresora) > 0 Then
            ComprobarImpresora_wImpresora = True
            Set Printer = tPrinter
            Exit For
        End If
    Next

End Function

