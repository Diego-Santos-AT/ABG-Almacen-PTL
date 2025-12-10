VERSION 5.00
Object = "{8294475C-ED2D-4E90-9A02-9CA1A17809E4}#1.0#0"; "SSUltraGridTrial.ocx"
Begin VB.Form frmSeleccionTabla2 
   ClientHeight    =   3375
   ClientLeft      =   1215
   ClientTop       =   2850
   ClientWidth     =   9375
   Icon            =   "frmSeleccionTabla2.frx":0000
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   3375
   ScaleWidth      =   9375
   Begin UltraGrid.SSUltraGrid UGDatos 
      Height          =   3375
      Left            =   0
      TabIndex        =   0
      Top             =   0
      Width           =   9375
      _ExtentX        =   16536
      _ExtentY        =   5953
      _Version        =   65536
      GridFlags       =   1024
   End
End
Attribute VB_Name = "frmSeleccionTabla2"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'######################################################################################
'   frmSeleccionTabla:    Formulario de seleccion de datos
'
'
'
'
'   Creado:
'   Ult. Mod.: 23/04/02
'**************************************************************************************
Option Explicit
Private tCabecera As String         ' Titulo del Formulario
Private r_Tmp As ADODB.Recordset    ' Lista de Datos de Entrada del Formulario
Private tCampos As String           'Datos de los campos a mostrar
Private tResultado As Boolean       ' True --> Ha seleccionado un codigo
                                    ' False --> No ha seleccionado ningun codigo
Private bInicio As Boolean          'Control de la primera ejecución

'Propiedades
Property Let Cabecera(ByVal newCabecera As String)
    tCabecera = newCabecera
End Property

Property Let RegistrosEntrada(ByRef newR_Tmp As ADODB.Recordset)
    Set r_Tmp = newR_Tmp
End Property

Property Let Campos(ByRef newCampos As String)
    tCampos = newCampos
End Property

Property Get Resultado() As Boolean
    Resultado = tResultado
End Property
'**************************************************************************************

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
    If KeyCode = vbKeyEscape Then
        Unload Me
    End If
End Sub

Private Sub Form_Load()
    Me.Caption = tCabecera
    tResultado = False
    bInicio = False
    Set UGDatos.DataSource = r_Tmp
End Sub

Private Sub Form_Resize()
    If bInicio Then
        UGDatos.Width = Me.Width - 125
        UGDatos.Height = Me.Height - 400
    End If
End Sub

Private Sub UGDatos_DblClick()
     tResultado = True
     Unload Me
End Sub

Private Sub UGDatos_InitializeLayout(ByVal Context As UltraGrid.Constants_Context, ByVal Layout As UltraGrid.SSLayout)
    Dim Campo As String
    Dim Nombre As String
    Dim Ancho As Long
    Dim pos As Integer
    Dim Ant As Integer
    Dim i As Long
    Dim AnchoTotal As Long
    
    ' Al clik seleccionar toda la fila
    Layout.Override.CellClickAction = ssClickActionRowSelect
    
    'Columnas para ordenar
    Layout.Override.HeaderClickAction = ssHeaderClickActionSortMulti
    
    ' Alternar filas
    'UGDatos.Override.RowAlternateAppearance.BackColor = &H80000018  ' amarillito
    
    'Configuración de las columnas
    AnchoTotal = 0
    With UGDatos.Bands(0)
        'Las oculta todas
        For i = 0 To .Columns.Count - 1
            .Columns(i).Hidden = True
        Next i
        
        pos = 0
        Ant = 1
        Do Until pos >= Len(tCampos)
            'Formato: Campo|Nombre Columna|Ancho Columna
            pos = InStr(pos + 1, tCampos, "|")
            Campo = Mid(tCampos, Ant, pos - Ant)
            Ant = pos + 1
            
            pos = InStr(pos + 1, tCampos, "|")
            Nombre = Mid(tCampos, Ant, pos - Ant)
            Ant = pos + 1
            
            pos = InStr(pos + 1, tCampos, "|")
            Ancho = Mid(tCampos, Ant, pos - Ant)
            Ant = pos + 1
            
            .Columns(Campo).Hidden = False
            .Columns(Campo).Header.Caption = Nombre
            .Columns(Campo).Width = Ancho
            AnchoTotal = AnchoTotal + Ancho
        Loop
    End With
    'Apariencia del formulario
    UGDatos.Width = AnchoTotal + 550
    If r_Tmp.RecordCount > 25 Then
        UGDatos.Height = 5175
    Else
        UGDatos.Height = (r_Tmp.RecordCount * 175) + 800
    End If
    
    Me.Width = UGDatos.Width + 125
    Me.Height = UGDatos.Height + 400
    bInicio = True
End Sub

Private Sub UGDatos_KeyPress(KeyAscii As UltraGrid.SSReturnShort)
    If KeyAscii = vbKeyReturn Then
        tResultado = True
        Unload Me
    End If
End Sub
