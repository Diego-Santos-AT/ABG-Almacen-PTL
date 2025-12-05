Attribute VB_Name = "GDFunc04"
Option Explicit
Private edSeleccion As New EntornoDeDatos
Private r_Reg As clGenericaRecordset
Private r_Reg2 As New ADODB.Recordset
Private crInforme As CrystalReport  ' ---- Objeto Crystal Report de Impresion
Private i As Integer
Private bInicial As Boolean
Private PosicionInicial



Public Type Registro_Seleccionado
    iCodigo As Long      ' --- CODIGO
    sDescripcion As String  ' --- DESCRIPCION
End Type

' -- Tipo de Datos para contener las ubicaciones por defecto de empresa desglosada
Public Type Desglose_Ubicacion
    Almacen_Fisico As Integer
    Almacen_Logico As Integer
    Bloque As Integer
    Fila As Integer
    Altura As Integer
End Type
'**************************************************************************************
'**************************************************************************************
'Función: DesplazaRegistro2
'
'Función para simular la navegación sobre un recorset con los Movimientos
' tradicionales de Primero, Anterior, Siguiente y Ultimo
'
'Creado: 10/05/02
'**************************************************************************************
Function DesplazaRegistro2(tabla As String, Campo As String, CodigoActual As String, TipoDesplazamiento As String, Optional condicion As String) As String
    Dim cnn As ADODB.Connection
    Dim cmd As ADODB.Command
    Dim rs As ADODB.Recordset
    Dim sql As String
    Dim Cond1 As String
    Dim Cond2 As String
    
    Dim Codigo As Long
    Dim Primero As Long
    Dim Ultimo As Long
    Dim Cuantos As Long
    
    If condicion = Null Or condicion = "" Then
        Cond1 = ""
        Cond2 = ""
    Else
        Cond1 = " where " & condicion
        Cond2 = " and " & condicion
    End If
    
    
    'Inicia la conexion con el origen de datos
    Set cnn = New ADODB.Connection
    cnn.Open ConexionGestion
    Set cmd = New ADODB.Command
    cmd.ActiveConnection = cnn

    Select Case TipoDesplazamiento
        Case "P"    'Primero
            sql = "SELECT min(" & Campo & ") as Registro  FROM " & tabla & Cond1
            DesplazaRegistro2 = EjecutaConsulta(sql, CodigoActual)
        
        Case "A"    'Anterior
            If Cond1 = "" Then
                Cond1 = " where " & Campo & " < " & CodigoActual
            Else
                Cond1 = Cond1 & " and " & Campo & " < " & CodigoActual
            End If
            sql = "SELECT max(" & Campo & ") as Registro FROM " & tabla & Cond1
            DesplazaRegistro2 = EjecutaConsulta(sql, CodigoActual)
            
        Case "S"    'Siguiente
            If Cond1 = "" Then
                Cond1 = " where " & Campo & " > " & CodigoActual
            Else
                Cond1 = Cond1 & " and " & Campo & " > " & CodigoActual
            End If
            sql = "SELECT min(" & Campo & ") as Registro FROM " & tabla & Cond1
            DesplazaRegistro2 = EjecutaConsulta(sql, CodigoActual)
        
        Case "U"    'Ultimo
            sql = "SELECT max(" & Campo & ") as Registro FROM " & tabla & Cond1
            DesplazaRegistro2 = EjecutaConsulta(sql, CodigoActual)
            
    End Select
    
    cnn.Close
    Exit Function

Err_Execute:

    ' Notifica al usuario cualquier error resultante tras
    ' ejecutar la consulta.
    MsgBox "Error número: " & Err.Number & " : " & Err.Description

   Resume Next

End Function
Private Function EjecutaConsulta(sql As String, CodAct As String) As String
    Dim cnn As New ADODB.Connection
    Dim cmd As New ADODB.Command
    Dim rs As New ADODB.Recordset
    
    cnn.Open ConexionGestion
    cmd.ActiveConnection = cnn
    Screen.MousePointer = vbHourglass
    cmd.CommandText = sql
    On Error GoTo Err_Execute
    Set rs = cmd.Execute
    On Error GoTo 0
    Screen.MousePointer = vbDefault
    If IsNull(rs!Registro) Then
        EjecutaConsulta = CodAct
    Else
        EjecutaConsulta = rs!Registro
    End If
    Exit Function
    cnn.Close
    
Err_Execute:

    ' Notifica al usuario cualquier error resultante tras
    ' ejecutar la consulta.
    MsgBox "Error número: " & Err.Number & " : " & Err.Description

   Resume Next
End Function

'- Funcion para truncar un numero decimal con el numero de decimales que se indique
' --- Numero_decimales >= 0
Public Function Truncar(Numero As Double, Numero_Decimales As Long) As Double
    Dim i As Long
    Dim posicion As Long
    Dim Cadena As String
    
    Numero = Round(Numero, 5)
    Cadena = CStr(Numero)
    posicion = InStr(1, Cadena, ",", vbTextCompare)
    
    If posicion = 0 Then
        Truncar = CDbl(Cadena)
    Else
        Truncar = CDbl(Mid(Cadena, 1, posicion + Numero_Decimales))
    End If
End Function

Public Function CambiaComaPorPunto(Precio) As Variant
    Dim Longitud As Long
    Dim posicion As Long
    Dim Cadena
    
    Cadena = Precio
    Longitud = Len(Cadena)
    
    posicion = InStr(1, Precio, ",", vbTextCompare)
    
    If posicion = 0 Then
        CambiaComaPorPunto = Precio
    Else
        CambiaComaPorPunto = Mid(Cadena, 1, posicion - 1) & "." & Mid(Cadena, posicion + 1, Longitud)
    End If

End Function

'**************************************************************************************
'Función: Exportar
'
'Función para exportar un ADODB.recorset a un fichero seleccionable
'
'
'
'Creado: 25/07/01
'**************************************************************************************
Public Sub Exportar(rs As ADODB.Recordset, Fichero As String, Optional Separador As String = "|")
    'Dim Contador As Integer
    Dim Cadena As String
    Dim i As Integer
    Dim msg As String
    
    'Proceso de grabación del archivo a disco
    If Fichero <> "" Then
        'Contador = 1
        Open Fichero For Output As #1
        rs.MoveFirst
        On Error Resume Next
        
        For i = 0 To rs.Fields.Count
            Cadena = Cadena & rs.Fields(i).Name & Separador
        Next i
        Print #1, Cadena
        
        Do Until rs.EOF
            Cadena = ""
            For i = 0 To rs.Fields.Count
                Cadena = Cadena & rs(i) & Separador
            Next i
            Print #1, Cadena
            rs.MoveNext
            'Contador = Contador + 1
        Loop
        Close #1
    End If
End Sub

' -- Procedimiento para Borrar los ficheros de un directorio segun un patron --
Public Sub Limpiar_Temporales(Ruta As String, Patron As String)
    Dim Cadena As String
    
    Cadena = Ruta & Patron
    ' ---- Eliminado de los ficheros .txt Descomprimidos ---------
    If Len(Dir(Ruta & Patron)) > 0 Then
        Kill Cadena
    End If
    ' ------------------------------------------------------------

End Sub


' -- Funcion que crear el Lote aplicado a una etiqueta ----
Public Function Dame_Lote(Fecha_Hoy) As String

    Dame_Lote = Dame_Fecha_Juliana(Fecha_Hoy)
    ' --- Le damos formato al lote con 8 Digitos de Longitud ---------
    Dame_Lote = Format(Dame_Lote, "00000000")
    ' ----------------------------------------------------------------
    
End Function

'---- Procedimiento que calcula la Fecha Juliana a partir de una fecha determinada --
Public Function Dame_Fecha_Juliana(Fecha) As String
Dim dia As Integer
Dim mes As Integer
Dim año As Integer
Dim Z As Double
Dim B As Integer
    
    dia = Day(Fecha)
    mes = Month(Fecha)
    año = Year(Fecha)
        
    'Calculamos el número de días transcurridos segun el período juliano hasta que comienza el Año
        
    Z = (4712 + año) * 365.25
    If Z = Int(Z) Then Z = Z - 1 Else Z = Int(Z)
    If (año <= 1583 Or (año = 1582 And (mes > 10 Or (mes = 10 And dia >= 15)))) And año <= 1700 Then Z = Z - 10
    If 1701 <= año And año <= 1800 Then Z = Z - 11
    If 1801 <= año And año <= 1900 Then Z = Z - 12
    If 1901 <= año And año <= 2100 Then Z = Z - 13
    If 2101 <= año And año <= 2200 Then Z = Z - 14
        
    'Calculamos el número de días de los meses anteriores a Mes
        
    B = CDate(Fecha) - dia - CDate("31/12/" & año - 1)
        
    'Calculamos el día juliano
        
    Z = Z + B + dia
    
    Dame_Fecha_Juliana = Z

End Function
' ---- Funcion que Devuelve la Fecha y Hora del Sistema ---------------------
Public Function Dame_FechaHora_Sistema() As Variant

Dim edLocal As New EntornoDeDatos

    ' ---------- Abro el Entorno de Datos y Los Registros -------------------
    On Error Resume Next
    edLocal.GestionAlmacen.Close
    On Error GoTo 0
    edLocal.GestionAlmacen.Open ConexionGestion
    
    On Error Resume Next
    edLocal.rsDameFechaHoraHoy.Close
    On Error GoTo 0
    
    edLocal.DameFechaHoraHoy
    
    Dame_FechaHora_Sistema = edLocal.rsDameFechaHoraHoy!Hoy
    
    edLocal.rsDameFechaHoraHoy.Close
    edLocal.GestionAlmacen.Close
    Set edLocal = Nothing

End Function

' --- Funcion para obtener la ruta completa de ubicacion del fichero ABG.dsn --------
Public Function Dame_DSN() As String

Dim sAuxDSN As String

    If Not LeerClave(HKEY_LOCAL_MACHINE, DSNDir, ClaveRegistroDSN, sAuxDSN) Then
        ' No se encontro el directorio de programas de Windows
        sAuxDSN = "C:\Archivos de programa\Archivos comunes\ODBC\Data Sources"
    End If

    RutaDSN = sAuxDSN & "\"
    
    Dame_DSN = RutaDSN & FicheroDSN

End Function

' ---- Funcion para Crear el SSCC unico para Una etiqueta
' ----
' ----     Parametros:
' ----          IncrementoSerie: Utilizado por Si en la creacion del SSCC se quiere incrementar los 2 digitos del CODIGO DE SERIE
' ----                  (Se utilizará para SSCC Heterogeneos
' ----
Public Function Dame_SSCC(Empresa_Fabricante, Ean_Empresa_Fabricante, Numerador, Optional IncrementoSerie As Integer = 0) As String

Dim Digito_Extension As String
Dim Ean_Empresa As String
Dim Codigo_Serie As String
Dim Numerador_Unico_Bultos As String
Dim Digito_Control As String
Dim SSCC As String
Dim SSCC_Sin_DigitoControl As String
Dim Suma_Parcial As Integer
Dim Multiplo_Diez As Long
Dim i As Long

    ' -- Digito de Extension (Fijo a 3) --------------
    Digito_Extension = "3"
    
    ' -- Codigo Ean de la Empresa --------------------
    Ean_Empresa = Ean_Empresa_Fabricante
    
    ' -- Codigo de la serie (puede ser el codigo ---------
    ' -- de empresa o el codigo de Fabricante asignado) --
    Codigo_Serie = Format(Empresa_Fabricante + IncrementoSerie, "00")
    
    
    ' -- Si la empresa tiene mas de dos dígitos toma los dos últimos, como es Informática
    If Len(Codigo_Serie) > 2 Then
        Codigo_Serie = Mid(Codigo_Serie, Len(Codigo_Serie) - 1, 2)
    End If
    
    ' --- Numerador único de bultos ----------------------
    Numerador_Unico_Bultos = Format(Numerador, "0000000")
    
    ' --- Digito de Control (calculado) ------------------
    SSCC_Sin_DigitoControl = Digito_Extension & Ean_Empresa & Codigo_Serie & Numerador_Unico_Bultos
    
    Suma_Parcial = (CInt(Mid(SSCC_Sin_DigitoControl, 1, 1)) * 3) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 2, 1)) * 1) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 3, 1)) * 3) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 4, 1)) * 1) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 5, 1)) * 3) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 6, 1)) * 1) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 7, 1)) * 3) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 8, 1)) * 1) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 9, 1)) * 3) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 10, 1)) * 1) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 11, 1)) * 3) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 12, 1)) * 1) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 13, 1)) * 3) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 14, 1)) * 1) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 15, 1)) * 3) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 16, 1)) * 1) + _
        (CInt(Mid(SSCC_Sin_DigitoControl, 17, 1)) * 3)
        
    'Multiplo_Diez = Suma_Parcial
    
    For i = 1 To Suma_Parcial
        If (i * 10) >= Suma_Parcial Then
            Multiplo_Diez = i * 10
            Exit For
        End If
    Next i
    
    Digito_Control = Multiplo_Diez - Suma_Parcial
    
    
    ' ----- CODIGO SSCC ----------------------------------
    SSCC = Digito_Extension & Ean_Empresa & Codigo_Serie & Numerador_Unico_Bultos & Digito_Control
    
    
    Dame_SSCC = SSCC
End Function


