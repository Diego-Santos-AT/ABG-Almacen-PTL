VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "clsRowLoop"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
Option Explicit

Public Event RowFound(ByVal Row As UltraGrid.SSRow, ByRef EventData As Variant, ByRef Cancel As Boolean)
Public Event EndOfRows(ByVal StartingRow As UltraGrid.SSRow, ByRef EventData As Variant)

'***********************
'* Public Functions
'***********************
'Procedure: LoopThroughRows
'Parameters:
'   Grid - UltraGrid control whose rows will be iterated.
'   StartingRow - Optional SSRow object. If nothing, then
'       search will begin with the first row in the first band
'       of the Grid passed in. If a row is passed, then the
'       search will begin that row.
'   IncludeChildren - Boolean indicating whether all children,
'       grandchildren, etc. will be searched for. Since child
'       rows can be on multiple child bands, the SpanBands
'       parameter is not used and all sibling bands of the child
'       rows will be searched.
'   SpanBands - Boolean indicating whether any siblings bands
'       of the starting row will be included in the search. Note:
'       this parameter only affects whether siblings bands of the
'       band containing the StartingRow are searched.
'   IgnoreHidden - Boolean indicating whether any hidden rows will
'       be searched. Note, parent, grandparent, etc. of the
'       StartingRow are not checked to see if they are hidden.
'   IgnoreChildrenIfCollapsed -  Boolean indicating whether the
'       routine should iterate through the children of a row if the
'       row is not expanded.
'   EventData - Variant. The variable is not used or manipulated in
'       any way. EventData will be passed to the events raised by
'       the class.
Public Sub LoopThroughRows(ByVal Grid As UltraGrid.SSUltraGrid, _
    Optional ByVal StartingRow As UltraGrid.SSRow, _
    Optional ByVal IncludeChildren As Boolean = True, _
    Optional ByVal SpanBands As Boolean = True, _
    Optional ByVal IgnoreHidden As Boolean = False, _
    Optional ByVal IgnoreChildrenIfCollapsed As Boolean = False, _
    Optional ByVal EventData As Variant)
    
    On Error GoTo ErrHandler:
    
    Dim tmpRow As UltraGrid.SSRow
    
    If Grid Is Nothing Then Exit Sub
    
    'If no starting row was passed in, then start with the first row
    ' in the grid.
    If StartingRow Is Nothing Then
        Set tmpRow = Grid.GetRow(ssChildRowFirst)
    Else
        Set tmpRow = StartingRow
    End If

    err.Clear
    
    If Not IterateRows(tmpRow, IgnoreHidden, IncludeChildren, SpanBands, IgnoreChildrenIfCollapsed, EventData) Then
        'If the Iteration is not cancelled, then raise an event to notify
        ' the user that we ran out of rows.
        RaiseEvent EndOfRows(tmpRow, EventData)
    End If
    
    Exit Sub
    
ErrHandler:
    err.Clear
End Sub

'***********************
'* Private Functions
'***********************
Private Function IterateRows(ByVal Row As UltraGrid.SSRow, _
    IgnoreHidden As Boolean, GetChildren As Boolean, _
    SpanBands As Boolean, IgnoreChildrenIfCollapsed As Boolean, _
    EventData As Variant) As Boolean
    
    Dim bCancel As Boolean
    Dim tmpRow As UltraGrid.SSRow
    
    On Error Resume Next
    Set tmpRow = Row
    
    Do
        If Not (IgnoreHidden And tmpRow.Hidden) Then
            RaiseEvent RowFound(tmpRow, EventData, bCancel)
            
            If bCancel Then
                IterateRows = bCancel
                Exit Do
            End If
            
            If tmpRow.HasChild And GetChildren Then
                If IgnoreChildrenIfCollapsed And Not tmpRow.Expanded Then
                    'If we should ignore child rows when the row is collapsed
                    ' and this row is collapsed then do nothing.
                Else
                    'Note: SpanBands only applies to siblings of the row passed in so
                    ' True is passed instead of the SpanBands parameter passed to this
                    ' function externally. If the user wants to get child rows, then all
                    ' child rows will be returned including all siblings of the first
                    ' child band.
                    IterateRows = IterateRows(tmpRow.GetChild(ssChildRowFirst), IgnoreHidden, GetChildren, True, IgnoreChildrenIfCollapsed, EventData)
                    If IterateRows Then Exit Do
                End If
            End If
        End If
        
        If Not tmpRow.HasNextSibling(SpanBands) Then Exit Do
        
        Set tmpRow = tmpRow.GetSibling(ssSiblingRowNext, SpanBands)
        
        If err.Number <> 0 Then
            err.Clear
            Exit Do
        End If
    Loop
End Function





