VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsDataFilter"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Implements UltraGrid.ISSUGDataFilter

Private Type ssCustomDataFormats
    Band As Integer
    Column As String
    Format As String
End Type

Public Enum ssFormatTypes
    ssFormatTypeColumn
    ssFormatTypeDataType
    ssFormatTypeBoth
End Enum

Private bInitializedColArray As Boolean
Private bInitializedFormatArray As Boolean
Private arrFormats() As String
Private arrColumnFormats() As ssCustomDataFormats
Private bApplyFormat As Boolean

'**************************
'* Public Functions
'**************************
Public Sub Reset(Optional FormatType As ssFormatTypes = ssFormatTypeBoth)
    'clear any formats already set based on the enum passed in
    If FormatType = ssFormatTypeColumn Or FormatType = ssFormatTypeBoth Then
        bInitializedColArray = False
        ReDim arrColumnFormats(0)
        arrColumnFormats(0).Format = ""
    End If
    
    If FormatType = ssFormatTypeDataType Or FormatType = ssFormatTypeBoth Then
        bInitializedFormatArray = False
        ReDim arrFormats(0)
        arrFormats(0) = ""
    End If
End Sub

'**************************
'* Private Functions
'**************************
Private Sub Class_Initialize()
    Reset
    bApplyFormat = True
End Sub

Private Function GetFormatForColumn(Column As UltraGrid.SSColumn) As String
    Dim iItem As Integer
    
    'cannot proceed unless there is a column to work with
    If Column Is Nothing Then Exit Function
    
    If bInitializedColArray Then
        'first check for column specific format
        For iItem = 0 To UBound(arrColumnFormats)
            If arrColumnFormats(iItem).Band = Column.Band.Index Then
                If arrColumnFormats(iItem).Column = Column.Key Then
                    'not we are not using a with block on the array since
                    ' this could lead to an error later when redimensioning
                    ' the array - see article #Q187553 on
                    ' support.microsoft.com for more information
                    GetFormatForColumn = arrColumnFormats(iItem).Format
                    Exit Function
                End If
            End If
        Next iItem
    End If
    
    If bInitializedFormatArray Then
        'if there is none then check for datatype specific format
        GetFormatForColumn = DataTypeFormat(Column.DataType)
    End If
End Function

'*****************************
'* ISSUGDataFilter Functions
'*****************************
Private Sub ISSUGDataFilter_AfterGetValue(ByVal Context As UltraGrid.Constants_GetValueContext, ByVal Cell As UltraGrid.SSCell, Value As Variant)
    Dim Grid As UltraGrid.SSUltraGrid
    Dim i As Integer
    Dim strFormat As String
    
    On Error GoTo ErrHandler
    
    'if our flag to prevent filter is set, exit
    If Not ApplyFormat Then Exit Sub
    
    'if the value is empty and therefore could not be formatted then exit
    If IsEmpty(Value) Then Exit Sub
    
    'otherwise get a reference to the grid this class is attached to
    Set Grid = Cell.Column.Band.Layout.Grid
    
    If Not Grid Is Nothing Then
        'if there is a grid, get the activecell
        If Not Grid.ActiveCell Is Nothing Then
            'if the cell passed to this function is the activecell and
            ' we are in edit mode, then exit so that the raw data value is
            ' used.
            If Grid.ActiveCell.IsSameAs(Cell) And Grid.IsInEditMode Then Exit Sub
        End If
    End If
    
    'get the format to use for this cell
    strFormat = GetFormatForColumn(Cell.Column)
    
    If strFormat <> "" Then
        'return the formatted data
        Value = Format(Value, strFormat)
    End If

ErrHandler:
    '
End Sub

Private Sub ISSUGDataFilter_BeforeSetValue(ByVal Cell As UltraGrid.SSCell, Value As Variant)
    ' this method needs to be implemented but nothing needs to be done
End Sub

'**************************
'* Public Properties
'**************************
Public Property Get ColumnFormat(ByVal Column As UltraGrid.SSColumn) As String
    If Not bInitializedColArray Then Exit Property

    Dim iItem As Integer
    
    For iItem = 0 To UBound(arrColumnFormats)
        If arrColumnFormats(iItem).Band = Column.Band.Index Then
            If arrColumnFormats(iItem).Column = Column.Key Then
                'not we are not using a with block on the array since
                ' this could lead to an error later when redimensioning
                ' the array - see article #Q187553 on
                ' support.microsoft.com for more information
                ColumnFormat = arrColumnFormats(iItem).Format
                Exit Function
            End If
        End If
    Next iItem
End Property

Public Property Let ColumnFormat(ByVal Column As UltraGrid.SSColumn, ByVal NewValue As String)
    Dim iItem As Integer
    Dim bFound As Boolean
    
    'A column format is being assigned. loop through the
    ' existing column formats and update it if it is already
    ' stored.
    For iItem = 0 To UBound(arrColumnFormats)
        If arrColumnFormats(iItem).Band = Column.Band.Index Then
            If arrColumnFormats(iItem).Column = Column.Key Then
                'not we are not using a with block on the array since
                ' this could lead to an error later when redimensioning
                ' the array - see article #Q187553 on
                ' support.microsoft.com for more information
                arrColumnFormats(iItem).Format = NewValue
                bFound = True
                Exit For
            End If
        End If
    Next iItem
    
    'If it was not found then increase the array and store the data
    If Not bFound Then
        iItem = UBound(arrColumnFormats) + 1
        ReDim Preserve arrColumnFormats(0 To iItem)
        
        'store the format as well as band's inded and column's key so
        ' that we can locate it later.
        arrColumnFormats(iItem).Format = NewValue
        arrColumnFormats(iItem).Band = Column.Band.Index
        arrColumnFormats(iItem).Column = Column.Key
    End If
    
    bInitializedColArray = True
End Property

Public Property Get DataTypeFormat(ByVal DataType As UltraGrid.Constants_DataType) As String
    If Not bInitializedFormatArray Then Exit Property
    If UBound(arrFormats) < DataType Or LBound(arrFormats) > DataType Then Exit Property
    
    DataTypeFormat = arrFormats(DataType)
End Property

Public Property Let DataTypeFormat(ByVal DataType As UltraGrid.Constants_DataType, ByVal NewValue As String)
    On Error GoTo ErrHandler
    
    If LBound(arrFormats) > DataType Then
        Exit Property
    ElseIf UBound(arrFormats) < DataType Then
        ReDim Preserve arrFormats(0 To DataType)
    End If
    
    arrFormats(DataType) = NewValue
    bInitializedFormatArray = True

ErrHandler:
    '
End Property

Public Property Get ApplyFormat() As Boolean
    ApplyFormat = bApplyFormat
End Property

Public Property Let ApplyFormat(NewValue As Boolean)
    bApplyFormat = NewValue
End Property


