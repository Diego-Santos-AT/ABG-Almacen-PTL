VERSION 5.00
Begin VB.Form frmMenu 
   BackColor       =   &H00B06000&
   BorderStyle     =   0  'None
   Caption         =   "Menú Principal"
   ClientHeight    =   4530
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   3810
   Icon            =   "frmMenu.frx":0000
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MDIChild        =   -1  'True
   Moveable        =   0   'False
   ScaleHeight     =   3792.558
   ScaleMode       =   0  'User
   ScaleWidth      =   3810
   ShowInTaskbar   =   0   'False
   Begin VB.CommandButton cmdAccionMenu 
      BackColor       =   &H00FFFF00&
      Caption         =   "REPARTO"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   15.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   660
      Index           =   3
      Left            =   113
      Style           =   1  'Graphical
      TabIndex        =   3
      Top             =   2325
      Width           =   3600
   End
   Begin VB.CommandButton cmdAccionMenu 
      BackColor       =   &H0080FF80&
      Caption         =   "EMPAQUETADO"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   15.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   660
      Index           =   4
      Left            =   113
      Style           =   1  'Graphical
      TabIndex        =   4
      Top             =   3075
      Width           =   3600
   End
   Begin VB.CommandButton cmdAccionMenu 
      BackColor       =   &H0080C0FF&
      Caption         =   "UBICAR BAC"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   15.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   660
      Index           =   1
      Left            =   113
      MaskColor       =   &H8000000F&
      Style           =   1  'Graphical
      TabIndex        =   1
      Top             =   825
      Width           =   3600
   End
   Begin VB.CommandButton cmdAccionMenu 
      Caption         =   "SALIR"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   15.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   630
      Index           =   5
      Left            =   113
      Style           =   1  'Graphical
      TabIndex        =   5
      Top             =   3825
      Width           =   3600
   End
   Begin VB.CommandButton cmdAccionMenu 
      BackColor       =   &H0080FFFF&
      Caption         =   "EXTRAER BAC"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   15.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   660
      Index           =   2
      Left            =   113
      Style           =   1  'Graphical
      TabIndex        =   2
      Top             =   1575
      Width           =   3600
   End
   Begin VB.CommandButton cmdAccionMenu 
      BackColor       =   &H008080FF&
      Caption         =   "CONSULTAS PTL"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   15.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   660
      Index           =   0
      Left            =   113
      MaskColor       =   &H8000000F&
      Style           =   1  'Graphical
      TabIndex        =   0
      Top             =   75
      Width           =   3600
   End
End
Attribute VB_Name = "frmMenu"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'******************************************************************************
' frmmenu
'
' Form principal de la aplicación de Gestion
'
'
' Conexiones:
'
' Creado: 8/03/00
'******************************************************************************
Option Explicit
Private ed As New EntornoDeDatos     'Entorno de Datos de trabajo
Private edC As New edConfig         ' Entorno de configuración
Dim i As Integer
'------------------------------------------------------------------------------

Private Sub botonSalir_Click()
    Dim msg As Integer
    msg = MsgBox("¿Desea salir de la aplicación?", vbQuestion + vbYesNo, "Almacen")
    If msg = vbYes Then
        Unload frmMain
    End If
End Sub

Private Sub cmdAccionMenu_Click(index As Integer)
    Select Case index
        Case 0
            frmConsultaPTL.Show
            frmConsultaPTL.SetFocus
        Case 1
            frmUbicarBAC.Show
            frmUbicarBAC.SetFocus
        Case 2
            frmExtraerBAC.Show
            frmExtraerBAC.SetFocus
        Case 3
            frmRepartirArticulo.Show
            frmRepartirArticulo.SetFocus
        Case 4
            frmEmpaquetarBAC.Show
            frmEmpaquetarBAC.SetFocus
        Case 5
            botonSalir_Click
    End Select
End Sub

Private Sub Form_Activate()
    'Refresca la barra de menu del formulario principal al tomar el foco
    CambiaModo (MOD_Todo)
End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
    Select Case KeyCode

        Case vbKey1:        cmdAccionMenu_Click 0
        Case vbKey2:        cmdAccionMenu_Click 1
        Case vbKey3:        cmdAccionMenu_Click 2
        Case vbKey4:        cmdAccionMenu_Click 3
        Case vbKey5:        cmdAccionMenu_Click 4
        Case vbKey6:        cmdAccionMenu_Click 5
        Case vbKeyEscape:   cmdAccionMenu_Click 5

    End Select

End Sub

Private Sub Form_Load()

    Dim rutaLogo As String
    'Activa el primer menu
    Me.Top = 0
    Me.Left = 0
    CargaMenu (0)
    ' ---------- Logo
    On Error Resume Next
        edC.Config.Close
    On Error GoTo 0
    edC.Config.Open ConexionConfig
        
    ' En este punto damos por buena la ejecución del programa y le damos al CargadorABG esa notificación
    GDFunc01.ControlEjecucion
    ' Actualización del CargadorABG
    GDFunc01.ActualizaCargador
End Sub

Private Sub mnuArchivoSalir_Click()
    Salir
End Sub

Private Sub Accion_Menu(ByVal Menu As Integer)
    Select Case Menu
        Case CMD_Salir
            Salir
    End Select
End Sub

Private Sub Salir()
    Unload Me
End Sub

Private Sub Toolbar1_ButtonClick(ByVal Button As MSComctlLib.Button)
   'Boton pulsado de la barra de menu
    Accion Button.index
End Sub

Public Sub Accion(index As Integer)
    'Acciones de la barra de menu segun los botones pulsados
    Select Case index
        Case CMD_Salir
              Unload Me
    End Select
End Sub

