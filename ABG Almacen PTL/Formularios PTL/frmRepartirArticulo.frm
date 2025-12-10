VERSION 5.00
Begin VB.Form frmRepartirArticulo 
   Appearance      =   0  'Flat
   BackColor       =   &H00B06000&
   BorderStyle     =   0  'None
   Caption         =   "Form1"
   ClientHeight    =   4515
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   4095
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   4515
   ScaleWidth      =   4095
   ShowInTaskbar   =   0   'False
   Begin VB.PictureBox FrameLectura 
      Appearance      =   0  'Flat
      BackColor       =   &H00808080&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   4510
      Left            =   0
      ScaleHeight     =   4515
      ScaleWidth      =   3810
      TabIndex        =   2
      Top             =   0
      Width           =   3805
      Begin VB.ComboBox Combo1 
         Height          =   315
         Left            =   975
         Style           =   2  'Dropdown List
         TabIndex        =   18
         Top             =   1200
         Width           =   2115
      End
      Begin VB.PictureBox pColor 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         BorderStyle     =   0  'None
         ForeColor       =   &H80000008&
         Height          =   315
         Left            =   3150
         ScaleHeight     =   315
         ScaleWidth      =   540
         TabIndex        =   17
         Top             =   1200
         Width           =   540
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "SALIR"
         Height          =   525
         Index           =   990
         Left            =   75
         TabIndex        =   1
         TabStop         =   0   'False
         ToolTipText     =   "[Esc]"
         Top             =   3900
         Width           =   3660
      End
      Begin VB.TextBox txtLecturaCodigo 
         Appearance      =   0  'Flat
         BackColor       =   &H00FFFFFF&
         BeginProperty Font 
            Name            =   "Arial"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   450
         Left            =   150
         MaxLength       =   36
         TabIndex        =   0
         Top             =   450
         Width           =   3540
      End
      Begin VB.Label Label4 
         Caption         =   "Puesto"
         Height          =   315
         Left            =   150
         TabIndex        =   16
         Top             =   1200
         Width           =   765
      End
      Begin VB.Label lbVolumen 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   2700
         TabIndex        =   15
         Top             =   3450
         Width           =   705
      End
      Begin VB.Label lbPeso 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   945
         TabIndex        =   14
         Top             =   3450
         Width           =   705
      End
      Begin VB.Label Label3 
         Caption         =   "Volumen"
         Height          =   315
         Left            =   1875
         TabIndex        =   13
         Top             =   3450
         Width           =   765
      End
      Begin VB.Label Label1 
         Caption         =   "Peso"
         Height          =   315
         Left            =   150
         TabIndex        =   12
         Top             =   3450
         Width           =   765
      End
      Begin VB.Label Label2 
         Caption         =   "Código"
         Height          =   315
         Left            =   150
         TabIndex        =   11
         Top             =   1575
         Width           =   765
      End
      Begin VB.Label lbArticulo 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   945
         TabIndex        =   10
         Top             =   1575
         Width           =   2745
      End
      Begin VB.Label lbEAN13 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   945
         TabIndex        =   9
         Top             =   2700
         Width           =   2745
      End
      Begin VB.Label Label14 
         Caption         =   "EAN13"
         Height          =   315
         Left            =   150
         TabIndex        =   8
         Top             =   2700
         Width           =   765
      End
      Begin VB.Label Label15 
         Caption         =   "STD"
         Height          =   315
         Left            =   150
         TabIndex        =   7
         Top             =   3075
         Width           =   765
      End
      Begin VB.Label lbSTD 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   945
         TabIndex        =   6
         Top             =   3075
         Width           =   705
      End
      Begin VB.Label lbNombreArticulo 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   705
         Left            =   150
         TabIndex        =   5
         Top             =   1950
         Width           =   3540
      End
      Begin VB.Label lbTexto 
         Alignment       =   2  'Center
         Appearance      =   0  'Flat
         BackColor       =   &H00808080&
         Caption         =   "REPARTIR ARTICULO"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Left            =   75
         TabIndex        =   4
         Top             =   75
         Width           =   3675
      End
      Begin VB.Label Label8 
         Alignment       =   2  'Center
         Appearance      =   0  'Flat
         BackColor       =   &H00808080&
         Caption         =   "Leer o teclear Artículo"
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Left            =   150
         TabIndex        =   3
         Top             =   900
         Width           =   3540
      End
   End
End
Attribute VB_Name = "frmRepartirArticulo"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'***********************************************************************
'Nombre: frmRepartirArticulo
' Formulario para el reparto de Artículos en BACs casilleros de PTL
'
'Creación:      02/09/20
'
'Realización:   A.Esteban
'
'***********************************************************************
Option Explicit
Option Compare Text

' ----- Constantes de Módulo -------------
Private Const MOD_Nombre = "Repartir Articulo"

Private Const CML_Salir = 990

Private Const LIS_ContenidoBAC = 1
Private Const ColorRojo = &H8080FF
Private Const ColorVerde = &H80FF80

' ----- Variables generales -------------
Private ed As New EntornoDeDatos

Private tUsuario As Integer
Private bInicio As Boolean

Private CustomDataFilter As clsDataFilter
'*******************************************************************************

Private Sub Form_Load()
    bInicio = True

    MousePointer = ccHourglass
    
    Me.Top = 0
    Me.Left = 0
    Me.Width = 3805
    Me.Height = 4525
    
    Me.Caption = MOD_Nombre

    'Inicia el entorno de datos
    If ed.GestionAlmacen.State <> adStateClosed Then ed.GestionAlmacen.Close
    ed.GestionAlmacen.Open ConexionGestionAlmacen

    CargarPistolas
    tUsuario = 0

    FrameLectura.Left = 0
    
    MousePointer = ccDefault
    
    bInicio = False

End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
    Select Case KeyCode

        Case vbKeyEscape:   Unload Me

    End Select
End Sub

Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
    If ed.GestionAlmacen.State <> adStateClosed Then ed.GestionAlmacen.Close
    Set ed = Nothing
End Sub
'----------------------------------------------------------------------------------<

Private Sub Salir()
    Unload Me
End Sub

Private Sub cmdAccion_Click(index As Integer)

    Select Case index
    
        Case CML_Salir
            Salir
    
    End Select
    
    If index <> CML_Salir Then txtLecturaCodigo.SetFocus

End Sub

Private Sub oEstado_Click(index As Integer)
    txtLecturaCodigo.SetFocus
End Sub

Private Sub txtLecturaCodigo_GotFocus()
    txtLecturaCodigo.BackColor = &HC0FFC0
End Sub

Private Sub txtLecturaCodigo_LostFocus()
    txtLecturaCodigo.BackColor = &H80000005
End Sub

Private Sub CargarPistolas()
    'Combo de puestos de trabajo
    Combo1.Clear

    If ed.rsDamePuestosTrabajoPTL.State <> adStateClosed Then ed.rsDamePuestosTrabajoPTL.Close
    ed.DamePuestosTrabajoPTL
    
    Combo1.Clear
    Combo1.AddItem "(0) Sin puesto"
    Combo1.ItemData(Combo1.NewIndex) = 0
    
    With ed.rsDamePuestosTrabajoPTL
        If .RecordCount > 0 Then
            .MoveFirst
            Do Until .EOF
                Combo1.AddItem "(" & CStr(!puecod) & ") " & !puedes
                Combo1.ItemData(Combo1.NewIndex) = !puecol
                .MoveNext
            Loop
        End If
        .Close
    End With
    
    Combo1.Text = Combo1.List(0)

End Sub

Private Sub Combo1_Click()
    'Aplica el color y el ususario
    
    Select Case Combo1.ItemData(Combo1.ListIndex)
        Case 1
            'Blanco
            pColor.BackColor = vbWhite
            tUsuario = CInt(Mid(Combo1.Text, 2, 3))
            
        Case 2
            'Amarillo
            pColor.BackColor = vbYellow
            tUsuario = CInt(Mid(Combo1.Text, 2, 3))
        
        Case 3
            'Magenta
            pColor.BackColor = vbMagenta
            tUsuario = CInt(Mid(Combo1.Text, 2, 3))
        Case 4
            'Cian
            pColor.BackColor = vbCyan
            tUsuario = CInt(Mid(Combo1.Text, 2, 3))
        Case 5
            'Azul
            pColor.BackColor = vbBlue
            tUsuario = CInt(Mid(Combo1.Text, 2, 3))
        Case 6
            'Verde
            pColor.BackColor = vbGreen
            tUsuario = CInt(Mid(Combo1.Text, 2, 3))
        Case 7
            'Rojo
            pColor.BackColor = vbRed
            tUsuario = CInt(Mid(Combo1.Text, 2, 3))
        Case Else
            'Sin Color
            pColor.BackColor = &H808080
            tUsuario = 0
            
    End Select

    If Not bInicio Then txtLecturaCodigo.SetFocus
    
End Sub

Private Sub txtLecturaCodigo_KeyDown(KeyCode As Integer, Shift As Integer)
    Dim bSeguir As Boolean

    If KeyCode = vbKeyReturn Then
        
        'Inicializa la visualización
        RefrescarDatos True

        Select Case Len(txtLecturaCodigo.Text)

            Case 13: ' EAN13 --------------------------
                    'Comprobar si la lectura es un EAN13
                    If fValidarEAN13(txtLecturaCodigo.Text, True) = False Then
                        'No existe el Artículo
                        Call wsMensaje(" No se ha encontrado el Artículo", vbCritical)
                    End If
                    
            Case 4, 5:  'Código de artículo
                    'Comprobar si la lectura es un Artículo
                    If fValidarArticulo(txtLecturaCodigo.Text, True) = False Then
                        'No existe el Artículo
                        Call wsMensaje(" No se ha encontrado el Artículo", vbCritical)
                    End If

        End Select

        txtLecturaCodigo.Text = ""
        
    End If
End Sub

Private Function fValidarArticulo(ByVal stArticulo As String, Optional ByVal blMensaje As Boolean = True) As Boolean
    Dim nArticulo As Long
    
    nArticulo = CLng(stArticulo)
    
    fValidarArticulo = False

    If ed.rsDameArticuloConsulta.State <> adStateClosed Then ed.rsDameArticuloConsulta.Close
    ed.DameArticuloConsulta stArticulo
    
    With ed.rsDameArticuloConsulta
        'Existencia del registro
        If .RecordCount > 0 Then
            
            fValidarArticulo = True
            
            'Se muestran los datos
            RefrescarDatos False, !artcod, !artnom, !artean, !artcj3, !artpea, !artcua
            
            'Se procede a repartir el artículo
            If RepartirArticulo(!artcod) Then
                Call wsMensaje(" Se ha reservado el BAC para el Artículo: " & !artcod, MENSAJE_Exclamacion)
                'RefrescarDatos True
            End If
        Else
            'No se ha encontrado el Artículo
            If blMensaje Then Call wsMensaje(" No existe el Artículo ", vbCritical)
        End If
        
        .Close
    End With

End Function

Private Function fValidarEAN13(ByVal stEAN13 As String, Optional ByVal blMensaje As Boolean = True) As Boolean
    Dim tEAN13 As String
    
    fValidarEAN13 = False

    tEAN13 = Mid(stEAN13, 1, 13)

    If ed.rsDameArticuloEAN13.State <> adStateClosed Then ed.rsDameArticuloEAN13.Close
    ed.DameArticuloEAN13 stEAN13
    
    
    With ed.rsDameArticuloEAN13
        'Existencia del registro
        If .RecordCount > 0 Then
            
            fValidarEAN13 = True
            
            'Se muestran los datos
            RefrescarDatos False, !artcod, !artnom, !artean, !artcj3, !artpea, !artcua
            
            'Se procede a repartir el artículo
            If RepartirArticulo(!artcod) Then
                Call wsMensaje(" Se ha reservado el BAC para el Artículo: " & !artcod, MENSAJE_Exclamacion)
                'RefrescarDatos True
            End If
        Else
            'No se ha encontrado el Artículo
            If blMensaje Then Call wsMensaje(" No existe el Artículo ", vbCritical)
        End If
        
        .Close
    End With

End Function

Private Sub RefrescarDatos(sEnBlanco As Boolean, Optional sArticulo As Long, Optional sNombre As String, Optional sEAN13 As String, Optional sSTD As Integer, _
                            Optional sPeso As Double, Optional sVolumen As Double)

    If sEnBlanco = True Then
        lbArticulo = ""
        lbNombreArticulo = ""
        lbEAN13 = ""
        lbSTD = ""
        lbPeso = ""
        lbVolumen = ""
        
    Else
        lbArticulo = CStr(sArticulo)
        lbNombreArticulo = sNombre
        lbEAN13 = sEAN13
        lbSTD = CStr(sSTD)
        lbPeso = CStr(Format(sPeso, "#0.0000"))
        lbVolumen = CStr(Format(sVolumen, "#0.0000"))
    End If

End Sub

Private Function RepartirArticulo(tArticulo As Long) As Boolean
    'Reparto del Artículo
    Dim Retorno As Integer
    Dim msgSalida As String
    
    RepartirArticulo = False
    
    ed.ReservaBACdePTL tArticulo, tUsuario, Retorno, msgSalida
    
    If Retorno = 0 Then
        RepartirArticulo = True
    Else
        Call wsMensaje(" No se ha podido repartir el Artículo. " & msgSalida, vbCritical)
    End If
    
End Function



