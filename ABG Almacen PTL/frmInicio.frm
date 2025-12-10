VERSION 5.00
Object = "{8CE292C1-A705-11D1-9EE6-000000000000}#92.7#0"; "GesData.ocx"
Begin VB.Form frmInicio 
   Appearance      =   0  'Flat
   AutoRedraw      =   -1  'True
   BackColor       =   &H00B06000&
   BorderStyle     =   0  'None
   ClientHeight    =   4770
   ClientLeft      =   0
   ClientTop       =   -375
   ClientWidth     =   3810
   ClipControls    =   0   'False
   ControlBox      =   0   'False
   Icon            =   "frmInicio.frx":0000
   LinkTopic       =   "Form2"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   4770
   ScaleMode       =   0  'User
   ScaleWidth      =   3810
   ShowInTaskbar   =   0   'False
   StartUpPosition =   2  'CenterScreen
   Begin VB.ComboBox ComboPuesto 
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   360
      Left            =   1560
      TabIndex        =   12
      Top             =   3075
      Width           =   1935
   End
   Begin VB.CommandButton cmdAccion 
      Height          =   645
      Index           =   0
      Left            =   120
      Picture         =   "frmInicio.frx":000C
      Style           =   1  'Graphical
      TabIndex        =   8
      Top             =   3615
      Width           =   1635
   End
   Begin VB.CommandButton cmdAccion 
      Cancel          =   -1  'True
      Height          =   645
      Index           =   1
      Left            =   1950
      Picture         =   "frmInicio.frx":044E
      Style           =   1  'Graphical
      TabIndex        =   7
      Top             =   3615
      Width           =   1650
   End
   Begin VB.Timer Timer1 
      Left            =   3840
      Top             =   120
   End
   Begin VB.ComboBox ComboEmpresa 
      Appearance      =   0  'Flat
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   360
      Left            =   1560
      TabIndex        =   2
      Top             =   2625
      Width           =   1935
   End
   Begin GesData.Texto txtPassword 
      Height          =   330
      Left            =   1560
      TabIndex        =   1
      Top             =   2025
      Width           =   1935
      _ExtentX        =   3413
      _ExtentY        =   582
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Arial"
         Size            =   18
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      BeginProperty FuenteEtiqueta {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      PasswordChar    =   "*"
      BorrarConFoco   =   -1  'True
      Sonido          =   0   'False
      Appearance      =   0
   End
   Begin GesData.Texto txtUsuarios 
      Height          =   330
      Left            =   1560
      TabIndex        =   0
      Top             =   1665
      Width           =   1935
      _ExtentX        =   3413
      _ExtentY        =   582
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "Arial"
         Size            =   11.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      BeginProperty FuenteEtiqueta {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      PermitirNulo    =   0   'False
      Sonido          =   0   'False
      Appearance      =   0
   End
   Begin VB.Label lblLabels 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00B06000&
      Caption         =   "Puesto:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00E0E0E0&
      Height          =   270
      Index           =   3
      Left            =   150
      TabIndex        =   13
      Top             =   3075
      Width           =   1350
   End
   Begin VB.Label lblProductName 
      Alignment       =   2  'Center
      AutoSize        =   -1  'True
      BackColor       =   &H00B06000&
      Caption         =   "Producto"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   18
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FFFFFF&
      Height          =   495
      Left            =   60
      TabIndex        =   11
      Top             =   300
      Width           =   3645
   End
   Begin VB.Label lblVersion 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      BackColor       =   &H00B06000&
      Caption         =   "Versión"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00E0E0E0&
      Height          =   225
      Left            =   300
      TabIndex        =   10
      Top             =   975
      Width           =   3195
   End
   Begin VB.Label lblComentarios 
      Alignment       =   1  'Right Justify
      AutoSize        =   -1  'True
      BackColor       =   &H00B06000&
      Caption         =   "Comentarios (Fecha Versión, ...)"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00E0E0E0&
      Height          =   225
      Left            =   300
      TabIndex        =   9
      Top             =   1275
      Width           =   3195
   End
   Begin VB.Label lblLabels 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00B06000&
      Caption         =   "&Empresa:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00E0E0E0&
      Height          =   270
      Index           =   2
      Left            =   150
      TabIndex        =   6
      Top             =   2610
      Width           =   1350
   End
   Begin VB.Label lblEstado 
      Alignment       =   2  'Center
      BackColor       =   &H00B06000&
      Caption         =   "Iniciando"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   9
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C0C0C0&
      Height          =   255
      Left            =   75
      TabIndex        =   5
      Top             =   4350
      Width           =   3585
   End
   Begin VB.Label lblLabels 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00B06000&
      Caption         =   "&Contraseña:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00E0E0E0&
      Height          =   270
      Index           =   1
      Left            =   150
      TabIndex        =   4
      Top             =   2100
      Width           =   1350
   End
   Begin VB.Label lblLabels 
      Alignment       =   1  'Right Justify
      BackColor       =   &H00B06000&
      Caption         =   "Usuario:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00E0E0E0&
      Height          =   270
      Index           =   0
      Left            =   150
      TabIndex        =   3
      Top             =   1725
      Width           =   1350
   End
End
Attribute VB_Name = "frmInicio"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'******************************************************************************
' frmInicio
'
'   Form de pantalla de Inicio de la aplicación
'   Muestra el nombre de programa, versión, etc
'   Control de la conexión con BD
'   Validación de Usuario
'
'
'
' Creado   : 04/04/01
' Ult. Mod.: 04/04/01
'******************************************************************************
Option Explicit
Private Reintentos As Integer
Private Password As Variant
Private HayPassword As Boolean
Private Const CMD_Aceptar = 0
Private Const CMD_Cancelar = 1
Private edC As New edConfig
Private msg As String
Private rutaLogo As String
Private bEjecutado As Boolean
'******************************************************************************

Private Sub Form_Load()
    lblVersion.Caption = "Versión " & App.Major & "." & App.Minor & "." & App.Revision
    lblComentarios.Caption = App.Comments
    lblProductName.Caption = App.Title
    
    RegistrarVersion
    Reintentos = 0
    LoginSucceeded = False
    
    'edC.Config.Open ConexionConfig
    lblEstado.Caption = "Conectando con el Servidor " & BDDServLocal & " ..."
    
    ' -- Aplicar un timer
    Timer1.Interval = 500
    Timer1.Enabled = True
    bEjecutado = False
    
    
    txtUsuarios.Visible = True
    
    lblEstado.Caption = "Iniciando..."
    
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
    
    If edC.Config.State <> adStateClosed Then edC.Config.Close
    Set edC = Nothing

End Sub
'========================================================================================================================

Private Sub cmdAccion_Click(Index As Integer)
    Accion (Index)
End Sub

Private Sub RegistrarVersion()
    Dim FicheroIniLocal As String
    If Dir("C:\Archivos de programa\ABG\") <> "" Then
        FicheroIniLocal = "C:\Archivos de programa\ABG\abg.ini"
        GuardarIni FicheroIniLocal, "Versiones", "ABG", App.Major & "." & App.Minor & "." & App.Revision
    End If
End Sub

Private Sub ComboEmpresa_Click()
     ' -- Validar nombre
    On Error Resume Next
        edC.rsDameCodigoEmpresa.Close
    On Error GoTo 0
    edC.DameCodigoEmpresa ComboEmpresa.Text
    
    If edC.rsDameCodigoEmpresa.RecordCount = 0 Then
        MsgBox "Empresa desconocida. Elija una de la lista...", vbExclamation, "Conexión"
        ComboEmpresa.SetFocus
        Exit Sub
    End If
    
    If edC.rsDameParametrosEmpresa.State <> adStateClosed Then edC.rsDameParametrosEmpresa.Close
    edC.DameParametrosEmpresa edC.rsDameCodigoEmpresa!empcod
    
    rutaLogo = edC.rsDameParametrosEmpresa!emplog
    edC.rsDameParametrosEmpresa.Close
    edC.rsDameCodigoEmpresa.Close
    
    ' Cargamos foto
    On Error Resume Next
    
'    If Dir(rutaLogo) <> "" Then
'
'        Logo.Picture = LoadPicture(rutaLogo)
'    Else
'        Logo.Picture = LoadPicture
'    End If
'    On Error GoTo 0

End Sub

Private Sub ComboEmpresa_KeyPress(KeyAscii As Integer)
    If KeyAscii = vbKeyReturn Then
        cmdAccion(CMD_Aceptar).SetFocus
    End If
End Sub

Private Sub Accion(Index As Integer)
    Select Case Index
        Case CMD_Aceptar
            Reintentos = Reintentos + 1
            ValidaContraseña
        Case CMD_Cancelar
            'establecer la variable global a false
            'para indicar un inicio de sesión fallido
            LoginSucceeded = False
            Unload Me
    End Select
End Sub

Private Sub Timer1_Timer()
   Timer1.Enabled = True
    'Lanza el proceso de probar conexión solo una vez
    If bEjecutado = False Then
        bEjecutado = True
        Timer1.Enabled = False
        ' -- Probar la conexión
        If ProbarConexion(BDDServLocal) = False Then End
    End If
End Sub

Private Sub txtPassword_KeyPress(KeyAscii As Integer)
    If KeyAscii = vbKeyReturn Then
        cmdAccion(CMD_Aceptar).SetFocus
    End If
End Sub

Private Sub txtUsuarios_GotFocus()

    txtUsuarios = UsrDefault ' Usuario por defecto
    
End Sub

Private Sub txtUsuarios_KeyPress(KeyAscii As Integer)
    If KeyAscii = vbKeyReturn Then
        ValidaUsuario
    '    SendKeys "{Tab}"
        If txtPassword.Visible = True Then
            txtPassword.SetFocus
        Else
            cmdAccion(CMD_Aceptar).SetFocus
        End If
    End If
End Sub

Private Sub txtUsuarios_LostFocus()
    ValidaUsuario
End Sub

Private Sub ValidaUsuario()
    Dim emp
    'Comprueba si el usuario es correcto
    edC.BuscaUsuario txtUsuarios.Text
    If edC.rsBuscaUsuario.RecordCount > 0 Then
        Password = edC.rsBuscaUsuario!usucon
        Usuario.Id = edC.rsBuscaUsuario!usuide
        Usuario.Nombre = txtUsuarios
        Usuario.instancias = edC.rsBuscaUsuario!usuins
        Usuario.nombrePC = edC.rsBuscaUsuario!Usunpc
        
        emp = LeerIni(ficINI, "Varios", "EmpDefault", "")
        If emp = "" Then
            CargaEmpresas 0
        Else
            CargaEmpresas CInt(emp)
        End If
        
        ' --- Cargamos Puestos ---
        CargaPuestos wPuestoTrabajo.Id
                
        ' --- Validar el nombre del PC
        If IsNull(Usuario.nombrePC) Or Usuario.nombrePC = "" Then
            ' -- Puede acceder a todos
        Else
            If Usuario.nombrePC <> nombrePC Then
                MsgBox "No puede ejecutar el Programa desde este PC...", vbExclamation, "Conexión"
                End
            End If
        End If
        '----------------------------
        If Password = "" Or IsNull(Password) Then
            HayPassword = False
            'lblLabels(1).Visible = False
            'txtPassword.Visible = False
            Me.Refresh
        Else
            HayPassword = True
            'lblLabels(1).Visible = True
            'txtPassword.Visible = True
            Me.Refresh
        End If
    Else
        'No se encuentra el usuario
        Password = ""
        Usuario.Nombre = ""
        Usuario.Id = 0
        Usuario.instancias = 0
        Usuario.nombrePC = ""
        HayPassword = False
        'lblLabels(1).Visible = False
        'txtPassword.Visible = False
        Me.Refresh
    End If
    
    edC.rsBuscaUsuario.Close
End Sub

Private Sub ValidaContraseña()
    'comprobar si la contraseña es correcta
    If (HayPassword And txtPassword = Password) Or (Not HayPassword And Usuario.Nombre <> "") Then
        LoginSucceeded = True
        
        lblEstado.Caption = "Inicio de Sesión... "
        
        ' -- Guarda el usuario por defecto
        GuardarIni ficINI, "Varios", "UsrDefault", txtUsuarios
        
        '--- Carga paramentros de empresa
        If edC.rsDameCodigoEmpresa.State <> adStateClosed Then edC.rsDameCodigoEmpresa.Close
        edC.DameCodigoEmpresa ComboEmpresa.Text
    
        If edC.rsDameCodigoEmpresa.RecordCount = 0 Then
            MsgBox "Empresa desconocida. Elija una de la lista...", vbExclamation, "Conexión"
            ComboEmpresa.SetFocus
            Exit Sub
        End If
        
        If edC.rsDameParametrosEmpresa.State <> adStateClosed Then edC.rsDameParametrosEmpresa.Close
        edC.DameParametrosEmpresa edC.rsDameCodigoEmpresa!empcod
        
        CodEmpresa = edC.rsDameParametrosEmpresa!empcod
        Empresa = edC.rsDameParametrosEmpresa!empnom
        
        edC.rsDameParametrosEmpresa.Close
        edC.rsDameCodigoEmpresa.Close
        
        ' --- Puesto de trabajo ----
        If edC.rsDameCodigoPuesto.State <> adStateClosed Then edC.rsDameCodigoPuesto.Close
        edC.DameCodigoPuesto ComboPuesto.Text
        If edC.rsDameCodigoPuesto.RecordCount > 0 Then
            wPuestoTrabajo.Id = edC.rsDameCodigoPuesto!puecod
        Else
            wPuestoTrabajo.Id = 1
        End If
        edC.rsDameCodigoPuesto.Close
        
        If edC.rsDamePuestoTrabajo.State <> adStateClosed Then edC.rsDamePuestoTrabajo.Close 'combobox
        edC.DamePuestoTrabajo wPuestoTrabajo.Id
        If edC.rsDamePuestoTrabajo.RecordCount <> 0 Then
            wPuestoTrabajo.Nombre = edC.rsDamePuestoTrabajo!puedes
            wPuestoTrabajo.NombreCorto = edC.rsDamePuestoTrabajo!puecor
            wPuestoTrabajo.Impresora = edC.rsDamePuestoTrabajo!impcod
            wPuestoTrabajo.NombreImpresora = edC.rsDamePuestoTrabajo!impnom
            wPuestoTrabajo.TipoImpresora = edC.rsDamePuestoTrabajo!implen
        Else
            wPuestoTrabajo.Nombre = ""
            wPuestoTrabajo.NombreCorto = ""
            wPuestoTrabajo.Impresora = 1
            wPuestoTrabajo.NombreImpresora = ""
            wPuestoTrabajo.TipoImpresora = ""
        End If
        
        If edC.rsDamePuestoTrabajo.State <> adStateClosed Then edC.rsDamePuestoTrabajo.Close
        GuardarIni ficINI, "Varios", "PueDefault", CStr(wPuestoTrabajo.Id)
        ' -----------------
        
        ' -- Impresora asociada al puesto de trabajo
        wImpresora = wPuestoTrabajo.NombreImpresora
        
        ' -- Guarda la empresa por defecto
        GuardarIni ficINI, "Varios", "EmpDefault", CStr(CodEmpresa)
        lblEstado.Caption = "Conectando con el servidor ..."
        lblEstado.Refresh
        
        
        Unload Me
    Else
        If Reintentos < 3 Then
            If HayPassword Then
                msg = "La contraseña no es válida. Vuelva a intentarlo"
                txtPassword.SetFocus
            Else
                msg = "El nombre de usuario no es válido. Vuelva a intentarlo"
                txtUsuarios.SetFocus
            End If
            MsgBox msg, vbExclamation, "Inicio de sesión"
            'SendKeys "{Home}+{End}"
        Else
            If HayPassword Then
                msg = "La contraseña no es válida. Ha agotado los intentos de acceso"
            Else
                msg = "El nombre de usuario no es válido. Ha agotado los intentos de acceso"
            End If
            MsgBox msg, vbCritical, "Inicio de sesión"
            Unload Me
        End If
    End If
End Sub

Private Sub CargaEmpresas(empdef As Integer)
    Dim i As Integer
' Empresas de acceso del usuario
    If edC.rsDameEmpresasAccesoUsuario.State <> adStateClosed Then edC.rsDameEmpresasAccesoUsuario.Close
    edC.DameEmpresasAccesoUsuario Usuario.Id
    
    ' Cargamos el combo
    ComboEmpresa.Visible = True
    lblLabels(2).Visible = True
    
    ComboEmpresa.Clear
    
    empdef = -1
    edC.rsDameEmpresasAccesoUsuario.MoveFirst
    
    ' ---------- Logo
    If edC.rsDameParametrosEmpresa.State <> adStateClosed Then edC.rsDameParametrosEmpresa.Close
    edC.DameParametrosEmpresa edC.rsDameEmpresasAccesoUsuario!useemp
    
    rutaLogo = edC.rsDameParametrosEmpresa!emplog
    edC.rsDameParametrosEmpresa.Close
    
    For i = 0 To edC.rsDameEmpresasAccesoUsuario.RecordCount - 1
        ' Recupera el nombre
        If edC.rsDameParametrosEmpresa.State <> adStateClosed Then edC.rsDameParametrosEmpresa.Close
        edC.DameParametrosEmpresa edC.rsDameEmpresasAccesoUsuario!useemp
        
        ' Vemos si es la empresa por defecto
        If CStr(edC.rsDameEmpresasAccesoUsuario!useemp) = CStr(CodEmpresa) Then
            empdef = i
            ' ---------- Logo
            rutaLogo = edC.rsDameParametrosEmpresa!emplog
        End If
        
        ComboEmpresa.AddItem edC.rsDameParametrosEmpresa!empnom
        edC.rsDameParametrosEmpresa.Close
        edC.rsDameEmpresasAccesoUsuario.MoveNext
        
    Next i
    ' Empresa por defecto
    If empdef = -1 Then
        ComboEmpresa.Text = ComboEmpresa.List(0)
    Else
        ComboEmpresa.Text = ComboEmpresa.List(empdef)
    End If
  ' Cargamos foto
    On Error Resume Next
    
'    If Dir(rutaLogo) <> "" Then
'
'       Logo.Picture = LoadPicture(rutaLogo)
'    Else
'        Logo.Picture = LoadPicture
'    End If
    On Error GoTo 0
End Sub

' ------- Procedimiento de Carga de Puestos de trabajo----
Private Sub CargaPuestos(idPuesto As Integer)
    Dim i As Integer
    Dim iPuesto As Integer

    If edC.rsDamePuestos.State <> adStateClosed Then edC.rsDamePuestos.Close
    edC.DamePuestos
   
    ' Cargamos el combo
    ComboPuesto.Visible = True
    lblLabels(3).Visible = True
    
    ComboPuesto.Clear
    
    iPuesto = -1
    edC.rsDamePuestos.MoveFirst
    
    For i = 0 To edC.rsDamePuestos.RecordCount - 1
        ' Vemos si es la empresa por defecto
        If edC.rsDamePuestos!puecod = idPuesto Then
            iPuesto = i
        End If
        ComboPuesto.AddItem edC.rsDamePuestos!puecor
        edC.rsDamePuestos.MoveNext
    Next i
    
    If edC.rsDamePuestos.State <> adStateClosed Then edC.rsDamePuestos.Close
    
    ' Puesto por defecto
    If iPuesto = -1 Then
        ComboPuesto.Text = ComboPuesto.List(0)
    Else
        ComboPuesto.Text = ComboPuesto.List(iPuesto)
    End If
    
End Sub
