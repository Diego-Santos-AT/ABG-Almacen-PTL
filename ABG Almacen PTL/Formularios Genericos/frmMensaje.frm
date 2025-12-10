VERSION 5.00
Begin VB.Form frmMensaje 
   Appearance      =   0  'Flat
   BackColor       =   &H00B06000&
   BorderStyle     =   0  'None
   Caption         =   "Mensaje General de Aplicación"
   ClientHeight    =   4800
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   3810
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4800
   ScaleWidth      =   3810
   ShowInTaskbar   =   0   'False
   Begin VB.Timer tmEspera 
      Interval        =   100
      Left            =   1500
      Top             =   2925
   End
   Begin VB.PictureBox Picture1 
      Appearance      =   0  'Flat
      BackColor       =   &H00E0E0E0&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   4215
      Left            =   218
      ScaleHeight     =   4215
      ScaleWidth      =   3315
      TabIndex        =   1
      Top             =   293
      Width           =   3315
      Begin VB.TextBox txtMensaje 
         Alignment       =   2  'Center
         BackColor       =   &H00E0E0E0&
         BorderStyle     =   0  'None
         BeginProperty Font 
            Name            =   "Arial"
            Size            =   21.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H000000FF&
         Height          =   3810
         Left            =   75
         Locked          =   -1  'True
         MultiLine       =   -1  'True
         TabIndex        =   2
         Text            =   "frmMensaje.frx":0000
         Top             =   225
         Width           =   3180
      End
   End
   Begin VB.CommandButton botonOcultoGanaFoco 
      Appearance      =   0  'Flat
      Caption         =   "Command1"
      Height          =   435
      Left            =   1275
      TabIndex        =   0
      Top             =   5000
      Width           =   1035
   End
End
Attribute VB_Name = "frmMensaje"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'***********************************************************************
'Nombre: frmMensaje
' Formulario para mostrar mensajes temporizados al usuario
'
'Creación:      05/06/20
'
'Realización:   A.Esteban
'
'***********************************************************************
Option Explicit

Public Tipo As TipoMensaje
Public Texto As String
Private Intervalo As Integer
Private Espera As Integer
'-----------------------------------------------------------------------

Private Sub Form_Load()
    Select Case Tipo
        Case MENSAJE_Informativo
            txtMensaje.ForeColor = &HC00000
            Espera = 20
        Case MENSAJE_Grave
            txtMensaje.ForeColor = &HFF&
            Espera = 40
        Case MENSAJE_Exclamacion
            txtMensaje.ForeColor = &HC000&
            Espera = 20
    End Select
    Me.BackColor = txtMensaje.ForeColor
    txtMensaje.Text = Texto
    Intervalo = 0
    
End Sub

Private Sub tmEspera_Timer()
    If Intervalo = Espera Then
        tmEspera.Enabled = False
        
        ReproduceSonido
        
        Unload Me
    Else
        Intervalo = Intervalo + 1
'        Select Case Tipo
'            Case MENSAJE_Informativo
'                Beep 700, 200
'                Beep 1500, 400
'            Case MENSAJE_Grave
'                Beep 1500, 200
'                Beep 700, 400
'            Case MENSAJE_Exclamacion
'                Beep 700, 200
'                Beep 1500, 400
'        End Select
        
    End If
    
    
End Sub

Private Sub ReproduceSonido()
    Dim i As Integer
    Dim iFrecuencia As Integer
    Dim iDuracion As Integer
    Dim iRepeticiones As Integer
    
    Select Case Tipo
        Case MENSAJE_Informativo, MENSAJE_Exclamacion
            iFrecuencia = 3000      'Frecuencia
            iDuracion = 100         'Duración
            iRepeticiones = 3       'Repeticiones
        Case MENSAJE_Grave
            iFrecuencia = 600       'Frecuencia
            iDuracion = 600         'Duración
            iRepeticiones = 1       'Repeticiones
    End Select
    
    For i = 0 To iRepeticiones + 1
    
        Beep iFrecuencia, iDuracion
    
        i = i + 1
    Next i

    'Unload Me
End Sub
