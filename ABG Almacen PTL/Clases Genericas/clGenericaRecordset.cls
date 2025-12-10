VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 1  'vbDataSource
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clGenericaRecordset"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
' ---------------- CLASE GENERICA DE RECORDSET DE MEMORIA -----------------------
' FECHA CREACION: 07/12/2000
' AUTOR: A. Moreno Marquéz
' -------------------------------------------------------------------------------
Option Explicit
Private rs As ADODB.Recordset
Private mvarMaxParam As Integer ' Numero máximo de parámetros de entrada a la clase
Public Property Get DataMembers() As DataMembers
End Property
Private Sub Class_GetDataMember(DataMember As String, Data As Object)
    Set Data = rs
End Sub
Public Sub Configura_Clase(TipoCursor As Variant, TipoBloqueo As Variant, _
        ParamArray paramConf())
        
'Parámetros de entrada:
'
'   TipoCursor:  Tipo de cursor que se usa en un objeto Recordset
'       Valores:
'       adOpenForwardOnly:  Cursor de tipo Forward-only. Predeterminado.Idéntico a un cursor estático, excepto sólo permite desplazarse hacia delante en los registros. Esto mejora el rendimiento en situaciones en las que sólo se quiere pasar una vez por cada registro.
'       adOpenKeyset:       Cursor de conjunto de claves. Igual que un cursor dinámico, excepto que no se pueden ver los registros que agregan otros usuarios, aunque los registros que otros usuarios eliminan son inaccesibles desde su conjunto de registros. Los cambios que otros usuarios hacen en los datos permanecen visibles.
'       adOpenDynamic:      Cursor dinámico. Las incorporaciones, cambios y eliminaciones que hacen otros usuarios permanecen visibles, y se admiten todo tipo de movimientos entre registros, a excepción de los marcadores si el proveedor no los admite.
'       adOpenStatic:       Cursor estático. Una copia estática de un conjunto de registros que se puede usar para buscar datos o generar informes. Las incorporaciones, cambios o eliminaciones que hacen otros usuarios no son visibles.
'
'   TipoBloqueo: Tipo de bloqueo que se pone en los registros durante el proceso de edición.
'       Valores:
'       adLockReadOnly:     Predeterminado. Sólo lectura—no puede modificar los datos.
'       adLockPessimistic:  Bloqueo pesimista, registro a registro: el proveedor hace lo necesario para asegurar la modificación correcta de los registros, generalmente bloqueando registros en el origen de datos durante el proceso de modificación.
'       adLockOptimistic:   Bloqueo optimista, registro a registro: el proveedor usa bloqueo optimista, bloqueando registros sólo cuando llama al método Update.
'       adLockBatchOptimistic: Actualizaciones optimistas por lotes: requerido para el modo de actualización por lotes como contraposición al modo de actualización inmediata.
'
'   paramConf: Array variable para crear la estructura del Recordset.
'       Formato del array: Descripción del campo, tipo de dato del campo
'
'       Valores del tipo de dato del campo:

'       adArray:    Se une en una instrucción OR lógica con otro tipo para indicar que los datos son una matriz segura de ese tipo (DBTYPE_ARRAY).
'       adBigInt:   Un entero con signo de 8 bytes (DBTYPE_I8).
'       adBinary:   Un valor binario (DBTYPE_BYTES).
'       adBoolean:  Un valor Boolean (DBTYPE_BOOL).
'       adByRef:    Se une en una instrucción OR lógica con otro tipo para indicar que los datos son un puntero a los datos del otro tipo (DBTYPE_BYREF).
'       adBSTR:     Una cadena de caracteres terminada en nulo (Unicode) (DBTYPE_BSTR).
'       adChar:     Un valor de tipo String (DBTYPE_STR).
'       adCurrency: Un valor de tipo Currency (DBTYPE_CY). Un valor Currency es un número de coma fija con cuatro dígitos a la derecha del signo decimal. Se almacena en un entero con signo de 8 bytes en escala de 10.000.
'       adDate:     Un valor de tipo Date (DBTYPE_DATE). Un valor Date se almacena como un valor de tipo Double; la parte entera es el número de días transcurridos desde el 30 de diciembre de 1899 y la parte fraccionaria es la fracción de un día.
'       adDBDate:   Un valor de fecha (aaaammdd) (DBTYPE_DBDATE).
'       adDBTime:   Un valor de hora (hhmmss) (DBTYPE_DBTIME).
'       adDBTimeStamp: Una marca de fecha y hora (aaaammddhhmmss más una fracción de miles de millones) (DBTYPE_DBTIMESTAMP).
'       adDecimal:  Un valor numérico exacto con una precisión y una escala fijas (DBTYPE_DECIMAL).
'       adDouble:   Un valor de coma flotante de doble precisión (DBTYPE_R8).
'       adEmpty:    No se ha especificado ningún valor (DBTYPE_EMPTY).
'       adError:    Un código de error de 32 bits (DBTYPE_ERROR).
'       adGUID:     Un identificador único global (GUID) (DBTYPE_GUID).
'       adIDispatch:Un puntero a una interfaz Idispatch de un objeto OLE (DBTYPE_IDISPATCH).
'       adInteger:  Un entero firmado de 4 bytes (DBTYPE_I4).
'       adIUnknown: Un puntero a una interfaz Iunknown de un objeto OLE (DBTYPE_IUNKNOWN).
'       adLongVarBinary: Un valor binario largo (sólo para el objeto Parameter).
'       adLongVarChar: Un valor largo de tipo String (sólo para el objeto Parameter).
'       adLongVarWChar: Un valor largo de tipo String terminado en nulo (sólo para el objeto Parameter).
'       adNumeric:  Un valor numérico exacto con una precisión y una escala exactas (DBTYPE_NUMERIC).
'       adSingle:   Un valor de coma flotante de simple precisión (DBTYPE_R4).
'       adSmallInt: Un entero con signo de 2 bytes (DBTYPE_I2).
'       adTinyInt:  Un entero con signo de 1 byte (DBTYPE_I1).
'       adUnsignedBigInt: Un entero sin signo de 8 bytes (DBTYPE_UI8).
'       adUnsignedInt: Un entero sin signo de 4 bytes (DBTYPE_UI4).
'       adUnsignedSmallInt: Un entero sin signo de 2 bytes (DBTYPE_UI2).
'       adUnsignedTinyInt:  Un entero sin signo de 1 byte (DBTYPE_UI1).
'       adUserDefined: Una variable definida por el usuario (DBTYPE_UDT).
'       adVarBinary: Un valor binario (sólo para el objeto Parameter).
'       adVarChar:  Un valor de tipo String (sólo para el objeto Parameter).
'       adVariant:  Un tipo Variant de automatización (DBTYPE_VARIANT).
'       adVector:   Se une en una instrucción OR lógica con otro tipo para indicar que los datos son una estructura DBVECTOR, tal como está definida por OLE DB, que contiene un contador de elementos y un puntero a los datos del otro tipo (DBTYPE_VECTOR).
'       adVarWChar: Una cadena de caracteres Unicode terminada en nulo (sólo para el objeto Parameter).
'       adWChar:    Una cadena de caracteres Unicode terminada en nulo (DBTYPE_WSTR).

Dim i As Integer

    If UBound(paramConf) + 1 > mvarMaxParam Then
        mvarMaxParam = UBound(paramConf) + 1
    End If
    
    ' Creación de la Instancia del Recordset
    Set rs = New ADODB.Recordset
    
    ' Asignación del Tipo de Cursor
    rs.CursorType = TipoCursor
    
    ' Asignación del Tipo de Bloqueo
    rs.LockType = TipoBloqueo

    ' Creación de la Estructura del Recordset
    With rs
        For i = LBound(paramConf) To UBound(paramConf) Step 3
            .Fields.Append paramConf(i), paramConf(i + 1), paramConf(i + 2)
    Next i
    End With

    ' Apertura del Recodset
    rs.Open

    ' Desplazamiento al primer Registro del Recordset
    If rs.RecordCount <> 0 Then
        rs.MoveFirst
    End If
    
End Sub
' -------- Método Para Añadir Elementos al Recordset -------------------------
Public Sub Add(ParamArray Valores() As Variant)
Static i As Integer
    With rs
        .AddNew
        For i = 0 To UBound(Valores)
            .Fields(i) = Valores(i)
        Next i
        .Update
    End With
End Sub
'----------- Método para Borrar Elementos al Recordset --------------------------
Public Sub Delete()
    If rs.RecordCount <> 0 Then
        rs.Delete
    End If
End Sub
'----------- Método para Modificar Elementos al Recordset ----------------------
Public Sub Update()
    rs.Update
End Sub
'----------- Método para Moverse al Primero al Recordset ----------------------
Public Sub MoveFirst()
    rs.MoveFirst
End Sub
'----------- Método para Moverse al Siguiente al Recordset ----------------------
Public Sub MoveNext()
    rs.MoveNext
End Sub
'----------- Método para Moverse al Anterior al Recordset ----------------------
Public Sub MovePrevius()
    rs.MovePrevious
End Sub
'----------- Método para Moverse al Ultimo al Recordset ----------------------
Public Sub MoveLast()
    
    rs.MoveLast
End Sub
'----------- Método para Ordenar el Recordset por un campo --------------------
Public Sub Sort(Cadena As String)
    rs.Sort = Cadena
End Sub

'----------- Funcion para comprobar si es el final del Recordset -------------
Public Function EOF() As Boolean
    If rs.EOF = True Then
        EOF = True
    Else
        EOF = False
    End If
End Function
'----------- Funcion para comprobar si es el anterior del primero del Recordset --
Public Function BOF() As Boolean
    If rs.BOF = True Then
        BOF = True
    Else
        BOF = False
    End If
End Function
'----------- Funcion que me devuelve el valor de un campo del recordset ---------
Public Function Campo(Index As Integer) As Variant
    Campo = rs(Index)
End Function
'----------- Funcion que me devuelve el Tipo de Datos de un campo del recordset ---------
Public Function Tipo(Index As Integer) As Variant
    Tipo = rs(Index).Type
End Function

'----------- Propiedad Contar Elementos del Recordset ----------------------

Public Property Get RecordCount() As Integer
    RecordCount = rs.RecordCount
End Property
'----------- Longiutd Definida de un Campo del Recordset --------------------
Public Property Get LongitudDefinida(ByVal Index As Integer) As Long
    LongitudDefinida = rs.Fields(Index).DefinedSize
End Property

' ----------- POSICION DEL ACTUAL RECORDSET -------------------------
'NO FUNCIONA .------------
Public Function PosicionActual() As Variant
    PosicionActual = rs.Bookmark
End Function
' ------------- posicionarse en el recordset-----------------------------
Public Sub posicion(pos)
   
    rs.AbsolutePosition = pos
End Sub
' ---------- Funcion que Devuelve el Recordset -----------------------------
Public Function Recordset() As Variant
    Set Recordset = rs
End Function



