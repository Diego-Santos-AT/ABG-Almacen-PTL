VERSION 5.00
Object = "{8294475C-ED2D-4E90-9A02-9CA1A17809E4}#1.0#0"; "SSUltraGridTrial.ocx"
Begin VB.Form frmConsultaPTL 
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
         Left            =   75
         MaxLength       =   36
         TabIndex        =   0
         Top             =   75
         Width           =   2700
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "SALIR"
         Height          =   450
         Index           =   990
         Left            =   2850
         TabIndex        =   1
         TabStop         =   0   'False
         ToolTipText     =   "Salir de Consultas"
         Top             =   75
         Width           =   915
      End
      Begin VB.Frame fraArticulos 
         Appearance      =   0  'Flat
         BackColor       =   &H00E0E0E0&
         BorderStyle     =   0  'None
         Caption         =   "Datos de Tablillas de Articulo"
         ForeColor       =   &H80000008&
         Height          =   1770
         Left            =   75
         TabIndex        =   7
         Top             =   2625
         Width           =   3675
         Begin UltraGrid.SSUltraGrid ugArticulos 
            Height          =   1635
            Left            =   75
            TabIndex        =   8
            ToolTipText     =   "Contenido de Artículos"
            Top             =   75
            Width           =   3510
            _ExtentX        =   6191
            _ExtentY        =   2884
            _Version        =   65536
            GridFlags       =   263168
            LayoutFlags     =   20
            MaxRowScrollRegions=   10000
            BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
               Name            =   "Arial"
               Size            =   8.25
               Charset         =   0
               Weight          =   400
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
         End
      End
      Begin VB.Frame fraArticulo 
         Appearance      =   0  'Flat
         BackColor       =   &H00C0C0C0&
         BorderStyle     =   0  'None
         ForeColor       =   &H80000008&
         Height          =   1950
         Left            =   75
         TabIndex        =   3
         Top             =   600
         Width           =   3675
         Begin VB.Label Label8 
            Caption         =   "Caja"
            Height          =   285
            Left            =   2625
            TabIndex        =   24
            Top             =   825
            Width           =   465
         End
         Begin VB.Label lbNumCaja 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   3150
            TabIndex        =   23
            Top             =   825
            Width           =   405
         End
         Begin VB.Label Label9 
            Caption         =   "Uds"
            Height          =   285
            Left            =   2625
            TabIndex        =   22
            Top             =   1575
            Width           =   465
         End
         Begin VB.Label lbUds 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   3150
            TabIndex        =   21
            Top             =   1575
            Width           =   405
         End
         Begin VB.Label lbNombreCaja 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   1350
            TabIndex        =   20
            Top             =   1575
            Width           =   1215
         End
         Begin VB.Label lbTipoCaja 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   19
            Top             =   1575
            Width           =   405
         End
         Begin VB.Label lbVolumen 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   2625
            TabIndex        =   18
            Top             =   1200
            Width           =   930
         End
         Begin VB.Label lbPeso 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   17
            Top             =   1200
            Width           =   780
         End
         Begin VB.Label lbTablilla 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   2175
            TabIndex        =   16
            Top             =   825
            Width           =   405
         End
         Begin VB.Label lbGrupo 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   15
            Top             =   825
            Width           =   555
         End
         Begin VB.Label Label7 
            Caption         =   "Tipo Caja"
            Height          =   285
            Left            =   75
            TabIndex        =   14
            Top             =   1575
            Width           =   765
         End
         Begin VB.Label Label6 
            Caption         =   "Volumen"
            Height          =   285
            Left            =   1725
            TabIndex        =   13
            Top             =   1200
            Width           =   840
         End
         Begin VB.Label Label5 
            Caption         =   "Peso"
            Height          =   285
            Left            =   75
            TabIndex        =   12
            Top             =   1200
            Width           =   765
         End
         Begin VB.Label Label4 
            Caption         =   "Tablilla"
            Height          =   285
            Left            =   1500
            TabIndex        =   11
            Top             =   825
            Width           =   615
         End
         Begin VB.Label Label1 
            Caption         =   "Grupo"
            Height          =   285
            Left            =   75
            TabIndex        =   10
            Top             =   825
            Width           =   765
         End
         Begin VB.Label Label3 
            Caption         =   "BAC"
            Height          =   285
            Left            =   75
            TabIndex        =   9
            Top             =   450
            Width           =   765
         End
         Begin VB.Label Label2 
            Caption         =   "Ubicación"
            Height          =   285
            Left            =   75
            TabIndex        =   6
            Top             =   75
            Width           =   765
         End
         Begin VB.Label lbUbicacion 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   285
            Left            =   900
            TabIndex        =   5
            Top             =   75
            Width           =   2655
         End
         Begin VB.Label lbBAC 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   285
            Left            =   900
            TabIndex        =   4
            Top             =   450
            Width           =   2655
         End
      End
   End
End
Attribute VB_Name = "frmConsultaPTL"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'***********************************************************************
'Nombre: frmConsultaPTL
' Formulario de consulta de Ubicaciones de PTL y BAC
'
'Creación:      02/06/20
'
'Realización:   A.Esteban
'
'***********************************************************************
Option Explicit
Option Compare Text

' ----- Constantes de Módulo -------------
Private Const MOD_Nombre = "Consultas PTL"

Private Const CML_Salir = 990

Private Const LIS_ContenidoBAC = 1
Private Const LIS_ContenidoCAJA = 2

Private Const ColorRojo = &H8080FF
Private Const ColorVerde = &H80FF80

' ----- Variables generales -------------
Private ed As New EntornoDeDatos
Private r_Art As ADODB.Recordset
Private r_ArtC As ADODB.Recordset

Private CustomDataFilter As clsDataFilter
'*******************************************************************************

Private Sub Form_Load()

    MousePointer = ccHourglass
    
    Me.Top = 0
    Me.Left = 0
    Me.Width = 3805
    Me.Height = 4525
    
    Me.Caption = MOD_Nombre

    'Inicia el entorno de datos
    If ed.GestionAlmacen.State <> adStateClosed Then ed.GestionAlmacen.Close
    ed.GestionAlmacen.Open ConexionGestionAlmacen

    FrameLectura.Left = 0
    
    MousePointer = ccDefault

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

Private Sub IniciaLista(Index As Integer)
    Select Case Index
        Case LIS_ContenidoBAC
            'Lista contenido de BAC
            Set r_Art = New ADODB.Recordset
        
            With r_Art
                .Fields.Append "unicod", adVarChar, 255, adFldUpdatable + adFldMayBeNull
                .Fields.Append "unigru", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "unitab", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "uniart", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "unican", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "univol", adDouble, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "uniusu", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "unifmd", adDate, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "unires", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "artnom", adVarChar, 100, adFldUpdatable + adFldMayBeNull
                .Fields.Append "ctapue", adBigInt, , adFldUpdatable + adFldMayBeNull
                
                .CursorType = adOpenKeyset
                .LockType = adLockOptimistic
                .Open
            End With
        
        Case LIS_ContenidoCAJA
            'Lista contenido de CAJA
            Set r_ArtC = New ADODB.Recordset
        
            With r_ArtC
                .Fields.Append "ltagru", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltatab", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltaart", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltacaj", adVarChar, 255, adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltacan", adDouble, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltafin", adVarChar, 1, adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltcusu", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltcfal", adDate, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltcfmd", adDate, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltapes", adDouble, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltavol", adDouble, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "ltcide", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "artnom", adVarChar, 100, adFldUpdatable + adFldMayBeNull
                
                .CursorType = adOpenKeyset
                .LockType = adLockOptimistic
                .Open
            End With
        
        
    End Select

End Sub

Private Sub cmdAccion_Click(Index As Integer)

    Select Case Index
    
        Case CML_Salir
            Salir
    
    End Select
    
    If Index <> CML_Salir Then txtLecturaCodigo.SetFocus

End Sub

Private Sub txtLecturaCodigo_GotFocus()
    txtLecturaCodigo.BackColor = &HC0FFC0
End Sub

Private Sub txtLecturaCodigo_LostFocus()
    txtLecturaCodigo.BackColor = &H80000005
End Sub

Private Sub txtLecturaCodigo_KeyDown(KeyCode As Integer, Shift As Integer)
    Dim bSeguir As Boolean

    If KeyCode = vbKeyReturn Then
        
        'Inicializa la visualización
        RefrescarDatos True

        Select Case Len(txtLecturaCodigo.Text)

            Case 12: ' Unidad de transporte / Ubicación --------------------------
                'Comprobar si la lectura es un BAC
                Label3.Caption = "BAC"
                If fValidarBAC(txtLecturaCodigo.Text, False) = False Then
                    'Comprobar si la lectura es una ubicación
                    If fValidarUbicacion(txtLecturaCodigo.Text, False) = False Then
                        'No existe la ubicación / BAC
                        Call wsMensaje(" No se ha encontrado Ubicación o BAC", vbCritical)
                    End If
                End If
                    
            Case 18: 'SSCC de Caja
                Label3.Caption = "CAJA"
                fValidarCaja txtLecturaCodigo.Text, True

            Case 20: 'SSCC de Caja
                Label3.Caption = "CAJA"
                fValidarCaja Mid(txtLecturaCodigo.Text, 3), True

        End Select

        txtLecturaCodigo.Text = ""
        
    End If
End Sub

Private Function fValidarBAC(ByVal stBAC As String, Optional ByVal blMensaje As Boolean = True) As Boolean
    Dim bCalculoPeso As Boolean
    Dim bCalculoVolumen As Boolean
    
    fValidarBAC = False

    If ed.rsDameDatosBACdePTL.State <> adStateClosed Then ed.rsDameDatosBACdePTL.Close
    ed.DameDatosBACdePTL stBAC
    
    With ed.rsDameDatosBACdePTL
        'Existencia del registro
        If .RecordCount > 0 Then
            
            fValidarBAC = True
            
            bCalculoPeso = !unipes > !unipma
            bCalculoVolumen = !univol > !univma
            
            'Se muestran los datos
            If IsNull(!ubicod) Then
                RefrescarDatos False, 0, 0, 0, 0, 0, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, !uninca, bCalculoPeso, bCalculoVolumen
            Else
                RefrescarDatos False, !ubicod, !ubialm, !ubiblo, !ubifil, !ubialt, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, !uninca, bCalculoPeso, bCalculoVolumen
            End If
            
            'Lista de artículos contenidos en el BAC
            Call sRefrescarArticulosBAC(!unigru, !unicod)
        
        Else
            If blMensaje Then Call wsMensaje(" No existe el BAC ", vbCritical)
        End If
    
        .Close
    End With

End Function

Private Function fValidarUbicacion(ByVal stUbicacion As String, Optional ByVal blMensaje As Boolean = True) As Boolean
    Dim iALF As Integer
    Dim iALM As Integer
    Dim iBLO As Integer
    Dim iFIL As Integer
    Dim iALT As Integer

    fValidarUbicacion = False

    iALF = 2
    iALM = Val(Mid(stUbicacion, 1, 3))
    iBLO = Val(Mid(stUbicacion, 4, 3))
    iFIL = Val(Mid(stUbicacion, 7, 3))
    iALT = Val(Mid(stUbicacion, 10, 3))

    If ed.rsDameDatosUbicacionPTL.State <> adStateClosed Then ed.rsDameDatosUbicacionPTL.Close
    ed.DameDatosUbicacionPTL iALF, iALM, iBLO, iFIL, iALT

    With ed.rsDameDatosUbicacionPTL
        'Existencia del registro
        If .RecordCount > 0 Then
            
            fValidarUbicacion = True
            
            'Si existe que tenga BAC asociado
            If IsNull(!unicod) Then
                If blMensaje Then Call wsMensaje(" La Ubicación no tiene asociada un BAC ", vbCritical)
                lbUbicacion = "(" & !ubicod & ") " & CStr(Format(iALM, "000")) & "." & CStr(Format(iBLO, "000")) & "." & CStr(Format(iFIL, "000")) & "." & CStr(Format(iALT, "000"))
            Else
                If fValidarBAC(!unicod, False) = False Then
                    If blMensaje Then Call wsMensaje(" La Ubicación no tiene asociada un BAC ", vbCritical)
                End If
            End If
        Else
            If blMensaje Then Call wsMensaje(" No existe la Unidad de Transporte ", vbCritical)
            lbUbicacion = ""
        End If
        
        .Close
    End With

End Function

Private Function fValidarCaja(ByVal stSSCC As String, Optional ByVal blMensaje As Boolean = True) As Boolean
    Dim bCalculoPeso As Boolean
    Dim bCalculoVolumen As Boolean
    Dim bEstado As Integer
    
    
    fValidarCaja = False

    If ed.rsDameDatosCAJAdePTL.State <> adStateClosed Then ed.rsDameDatosCAJAdePTL.Close
    ed.DameDatosCAJAdePTL stSSCC
    
    With ed.rsDameDatosCAJAdePTL
        'Existencia del registro
        If .RecordCount > 0 Then
            
            fValidarCaja = True
            
            bEstado = IIf(!ltcvol > 0, 1, 0)
            
            'Se muestran los datos
            RefrescarDatos False, 0, 0, 0, 0, 0, !ltcssc, bEstado, !ltcgru, !ltctab, !ltcpes, !ltcvol, !ltctip, !tipdes, !ltccaj, bCalculoPeso, bCalculoVolumen
            
            'Lista de artículos contenidos en la CAJA
            Call sRefrescarArticulosCAJA(!ltcgru, !ltctab, !ltccaj)
            
        Else
            If blMensaje Then Call wsMensaje(" No existe la CAJA ", vbCritical)
            
        End If
    
        .Close
    End With

End Function

Private Sub RefrescarDatos(sEnBlanco As Boolean, Optional sCodUbicacion As Integer, Optional sALM As Integer, Optional sBLO As Integer, Optional sFIL As Integer, _
                            Optional sALT As Integer, Optional sBAC As String, Optional sEstadoBAC As Integer, Optional sGrupo As Integer, Optional sTablilla As Integer, _
                            Optional sPeso As Double, Optional sVolumen As Double, Optional sTipoCaja As String, Optional sNombreCaja As String, Optional sNumCaja As String, _
                            Optional bPeso As Boolean, Optional bVolumen As Boolean)

    If sEnBlanco = True Then
        'Inicia la visualización
        lbUbicacion = ""
        lbBAC = ""
        'lbEstadoBAC = ""
        'lbEstadoBAC.BackColor = vbWhite
        lbBAC.BackColor = vbWhite
        
        lbGrupo = ""
        lbTablilla = ""
        lbUds = ""
        
        lbPeso = ""
        lbPeso.BackColor = vbWhite
        
        lbVolumen = ""
        lbVolumen.BackColor = vbWhite
        
        lbTipoCaja = ""
        lbNombreCaja = ""
        
        lbNumCaja = ""
        
        
    Else
        If sCodUbicacion = 0 Then
            lbUbicacion = "SIN UBICACION"
            lbUbicacion = "-------------"
        Else
            lbUbicacion = "(" & sCodUbicacion & ") " & CStr(Format(sALM, "000")) & "." & CStr(Format(sBLO, "000")) & "." & CStr(Format(sFIL, "000")) & "." & CStr(Format(sALT, "000"))
        End If
        lbBAC = sBAC
        
        'lbEstadoBAC = IIf(sEstadoBAC = 0, "ABIERTO", "CERRADO")
        'If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde
        lbBAC.BackColor = IIf(sEstadoBAC = 0, vbWhite, ColorVerde)
        
        lbGrupo = sGrupo
        lbTablilla = sTablilla
        lbUds = 0
        
        lbPeso = Format(sPeso, "#0.000")
        If bPeso Then lbPeso.BackColor = ColorRojo
        
        lbVolumen = Format(sVolumen, "#0.000")
        If bVolumen Then lbVolumen.BackColor = ColorRojo
        
        lbTipoCaja = sTipoCaja
        lbNombreCaja = sNombreCaja
        
        lbNumCaja = sNumCaja
        
        
    End If

    Set ugArticulos.DataSource = Nothing
    'IniciaLista LIS_ContenidoBAC
    'Set ugArticulos.DataSource = r_Art


End Sub

Private Sub sRefrescarArticulosBAC(ByVal sGrupo As Long, sBAC As String)
    Dim i As Integer
    Dim iUds As Integer
    
    Set ugArticulos.DataSource = Nothing
    
    IniciaLista LIS_ContenidoBAC
    iUds = 0
    
    'Consulta de datos
    If ed.rsDameContenidoBacGrupo.State <> adStateClosed Then ed.rsDameContenidoBacGrupo.Close
    ed.DameContenidoBacGrupo sGrupo, sBAC

    With ed.rsDameContenidoBacGrupo
        If .RecordCount > 0 Then
            .MoveFirst
            Do Until .EOF
                r_Art.AddNew
                For i = 0 To .Fields.Count - 1
                    r_Art.Fields(i) = .Fields(i)
                Next i
                iUds = iUds + !unican
                r_Art.Update
                .MoveNext
            Loop
            r_Art.MoveFirst
        End If
        .Close
    End With

    Set ugArticulos.DataSource = r_Art
    lbUds = iUds

End Sub

Private Sub sRefrescarArticulosCAJA(ByVal sGrupo As Long, sTablilla As Long, sCaja As String)
    Dim i As Integer
    Dim iUds As Integer
    
    Set ugArticulos.DataSource = Nothing
    
    IniciaLista LIS_ContenidoCAJA
    iUds = 0
    
    'Consulta de datos
    If ed.rsDameContenidoCajaGrupo.State <> adStateClosed Then ed.rsDameContenidoCajaGrupo.Close
    ed.DameContenidoCajaGrupo sGrupo, sTablilla, sCaja

    With ed.rsDameContenidoCajaGrupo
        If .RecordCount > 0 Then
            .MoveFirst
            Do Until .EOF
                r_ArtC.AddNew
                For i = 0 To .Fields.Count - 1
                    r_ArtC.Fields(i) = .Fields(i)
                Next i
                iUds = iUds + !ltccan
                r_ArtC.Update
                .MoveNext
            Loop
            r_ArtC.MoveFirst
        End If
        .Close
    End With

    Set ugArticulos.DataSource = r_ArtC
    lbUds = iUds

End Sub


Private Sub ugArticulos_DblClick()
    'Call cmdAccion_Click(CML_AmpliarFoto)
End Sub

Private Sub ugArticulos_InitializeLayout(ByVal Context As UltraGrid.Constants_Context, ByVal Layout As UltraGrid.SSLayout)
    Dim i As Integer
    
    Set ugArticulos.DataFilter = CustomDataFilter

    With ugArticulos.Override
        .AllowColMoving = ssAllowColMovingNotAllowed    ' No permite mover columnas
        .FetchRows = ssFetchRowsPreloadWithParent
        .AllowUpdate = ssAllowUpdateNo
        .AllowAddNew = ssAllowAddNewNo
        .AllowDelete = ssAllowDeleteNo
    End With
    ugArticulos.Appearance.BackColor = &HFFDCCE

    Layout.Override.CellClickAction = ssClickActionRowSelect            ' Al clik seleccionar toda la fila
    Layout.Override.HeaderClickAction = ssHeaderClickActionSortMulti    ' Columnas para ordenar
    
    With ugArticulos.Bands(0)
        For i = 0 To .Columns.Count - 1
            .Columns(i).Hidden = True
        Next i

        i = 0
        Select Case Label3.Caption
            Case "BAC"
                FormatoColumnaUGrid ugArticulos, 0, "uniart", False, "Codigo", 600, i, "", "#,##0", &H80000005, False
                FormatoColumnaUGrid ugArticulos, 0, "artnom", False, "Articulo", 1600, i, "", "", &H80000005, False
                FormatoColumnaUGrid ugArticulos, 0, "unican", False, "Cant", 500, i, "", "#,##0", &H80000005, False
            Case "CAJA"
                FormatoColumnaUGrid ugArticulos, 0, "ltaart", False, "Codigo", 600, i, "", "#,##0", &H80000005, False
                FormatoColumnaUGrid ugArticulos, 0, "artnom", False, "Articulo", 1600, i, "", "", &H80000005, False
                FormatoColumnaUGrid ugArticulos, 0, "ltacan", False, "Cant", 500, i, "", "#,##0", &H80000005, False
        End Select
    End With

End Sub

