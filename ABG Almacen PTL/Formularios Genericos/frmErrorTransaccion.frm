VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "Mscomctl.ocx"
Begin VB.Form frmErrorTransaccion 
   AutoRedraw      =   -1  'True
   ClientHeight    =   3015
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5940
   Icon            =   "frmErrorTransaccion.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3015
   ScaleWidth      =   5940
   StartUpPosition =   1  'CenterOwner
   Begin VB.Timer Timer1 
      Left            =   2760
      Top             =   2040
   End
   Begin MSComctlLib.ProgressBar ProgressBar1 
      Align           =   2  'Align Bottom
      Height          =   375
      Left            =   0
      TabIndex        =   3
      Top             =   2640
      Width           =   5940
      _ExtentX        =   10478
      _ExtentY        =   661
      _Version        =   393216
      Appearance      =   1
      Scrolling       =   1
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Cancelar"
      Height          =   375
      Index           =   1
      Left            =   3240
      TabIndex        =   1
      Top             =   2040
      Width           =   1935
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Aceptar"
      Height          =   375
      Index           =   0
      Left            =   720
      TabIndex        =   0
      Top             =   2040
      Width           =   1935
   End
   Begin VB.Label LbMensaje 
      ForeColor       =   &H000000FF&
      Height          =   1515
      Left            =   480
      TabIndex        =   2
      Top             =   240
      Width           =   4965
   End
End
Attribute VB_Name = "frmErrorTransaccion"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
' Definición de propiedades
Dim tCabecera As String '--- Mensaje de Cabecera del Formulario
Dim tMensaje As String  '--- Mensaje para el Formulario
Dim tTiempo As Long     '--- Tiempo de Espera
Dim tResultado As Boolean '--- Valor de Retorno: False ---> Cancelar; True -->Aceptar
Property Let Cabecera(ByVal sCadena As String)
    tCabecera = sCadena
End Property
Property Let Mensaje(ByVal sTexto As String)
    tMensaje = sTexto
End Property
Property Let Tiempo(ByVal sTiempo As Long)
    tTiempo = sTiempo
End Property
Property Get bResultado() As Boolean
    bResultado = tResultado
End Property

Private Sub Form_Load()
    Me.Caption = tCabecera
    Me.LbMensaje = tMensaje
    tResultado = False
    
    ProgressBar1.Visible = True
    ProgressBar1.Min = 0
    ProgressBar1.Max = tTiempo * 5
    ProgressBar1.Value = ProgressBar1.Min
    
    Timer1.Enabled = True
    Timer1.Interval = 200

    
End Sub

Private Sub Command1_Click(Index As Integer)
    If Index = 0 Then
        tResultado = True
    Else
        tResultado = False
    End If
    Unload Me
End Sub

Private Sub Timer1_Timer()
    On Error GoTo error_timer
    ProgressBar1.Value = ProgressBar1.Value + 1
    On Error GoTo 0
    Exit Sub
    
error_timer:
    Unload Me
End Sub
