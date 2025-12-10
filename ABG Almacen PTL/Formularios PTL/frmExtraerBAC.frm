VERSION 5.00
Begin VB.Form frmExtraerBAC 
   Appearance      =   0  'Flat
   BackColor       =   &H00B06000&
   BorderStyle     =   0  'None
   Caption         =   "Extraer BAC"
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
      Begin VB.PictureBox Picture1 
         Appearance      =   0  'Flat
         BackColor       =   &H00C0C0C0&
         BorderStyle     =   0  'None
         ForeColor       =   &H80000008&
         Height          =   915
         Left            =   75
         ScaleHeight     =   915
         ScaleWidth      =   3675
         TabIndex        =   24
         Top             =   1425
         Width           =   3675
         Begin VB.OptionButton oEstado 
            Appearance      =   0  'Flat
            BackColor       =   &H00C0C0C0&
            Caption         =   "Abrir BAC"
            ForeColor       =   &H80000008&
            Height          =   315
            Index           =   1
            Left            =   150
            TabIndex        =   26
            Top             =   450
            Width           =   1815
         End
         Begin VB.OptionButton oEstado 
            Appearance      =   0  'Flat
            BackColor       =   &H00C0C0C0&
            Caption         =   "Cerrar BAC"
            ForeColor       =   &H80000008&
            Height          =   315
            Index           =   0
            Left            =   150
            TabIndex        =   25
            Top             =   75
            Value           =   -1  'True
            Width           =   1815
         End
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
         Left            =   75
         MaxLength       =   36
         TabIndex        =   0
         Top             =   450
         Width           =   2700
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "SALIR"
         Height          =   450
         Index           =   990
         Left            =   2850
         TabIndex        =   1
         TabStop         =   0   'False
         ToolTipText     =   "[Esc]"
         Top             =   450
         Width           =   915
      End
      Begin VB.Frame fraArticulo 
         Appearance      =   0  'Flat
         BackColor       =   &H00C0C0C0&
         BorderStyle     =   0  'None
         ForeColor       =   &H80000008&
         Height          =   1950
         Left            =   75
         TabIndex        =   3
         Top             =   2400
         Width           =   3675
         Begin VB.Label Label9 
            Caption         =   "Uds"
            Height          =   285
            Left            =   2625
            TabIndex        =   21
            Top             =   825
            Width           =   465
         End
         Begin VB.Label lbUds 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   3150
            TabIndex        =   20
            Top             =   825
            Width           =   405
         End
         Begin VB.Label lbEstadoBAC 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   2640
            TabIndex        =   19
            Top             =   450
            Width           =   915
         End
         Begin VB.Label lbNombreCaja 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   1515
            TabIndex        =   18
            Top             =   1575
            Width           =   2040
         End
         Begin VB.Label lbTipoCaja 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   17
            Top             =   1575
            Width           =   555
         End
         Begin VB.Label lbVolumen 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   2625
            TabIndex        =   16
            Top             =   1200
            Width           =   930
         End
         Begin VB.Label lbPeso 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   15
            Top             =   1200
            Width           =   780
         End
         Begin VB.Label lbTablilla 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   2175
            TabIndex        =   14
            Top             =   825
            Width           =   405
         End
         Begin VB.Label lbGrupo 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   13
            Top             =   825
            Width           =   555
         End
         Begin VB.Label Label7 
            Caption         =   "Tipo Caja"
            Height          =   285
            Left            =   75
            TabIndex        =   12
            Top             =   1575
            Width           =   765
         End
         Begin VB.Label Label6 
            Caption         =   "Volumen"
            Height          =   285
            Left            =   1725
            TabIndex        =   11
            Top             =   1200
            Width           =   840
         End
         Begin VB.Label Label5 
            Caption         =   "Peso"
            Height          =   285
            Left            =   75
            TabIndex        =   10
            Top             =   1200
            Width           =   765
         End
         Begin VB.Label Label4 
            Caption         =   "Tablilla"
            Height          =   285
            Left            =   1500
            TabIndex        =   9
            Top             =   825
            Width           =   615
         End
         Begin VB.Label Label1 
            Caption         =   "Grupo"
            Height          =   285
            Left            =   75
            TabIndex        =   8
            Top             =   825
            Width           =   765
         End
         Begin VB.Label Label3 
            Caption         =   "BAC"
            Height          =   285
            Left            =   75
            TabIndex        =   7
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
            Width           =   1680
         End
      End
      Begin VB.Label Label8 
         Alignment       =   2  'Center
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Leer BAC  o Ubicación de PTL"
         ForeColor       =   &H80000008&
         Height          =   315
         Left            =   75
         TabIndex        =   23
         Top             =   1050
         Width           =   3675
      End
      Begin VB.Label lbTexto 
         Alignment       =   2  'Center
         Appearance      =   0  'Flat
         BackColor       =   &H00808080&
         Caption         =   "EXTRAER BAC"
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
         TabIndex        =   22
         Top             =   75
         Width           =   3675
      End
   End
End
Attribute VB_Name = "frmExtraerBAC"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'***********************************************************************
'Nombre: frmExtraerBAC
' Formulario para la extracción de un BAC de una ubicación de PTL
'
'Creación:      04/06/20
'
'Realización:   A.Esteban
'
'***********************************************************************
Option Explicit
Option Compare Text

' ----- Constantes de Módulo -------------
Private Const MOD_Nombre = "Extraer BAC"

Private Const CML_Salir = 990

Private Const LIS_ContenidoBAC = 1
Private Const ColorRojo = &H8080FF
Private Const ColorVerde = &H80FF80

' ----- Variables generales -------------
Private ed As New EntornoDeDatos

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

Private Sub txtLecturaCodigo_KeyDown(KeyCode As Integer, Shift As Integer)
    Dim bSeguir As Boolean

    If KeyCode = vbKeyReturn Then
        
        'Inicializa la visualización
        RefrescarDatos True

        Select Case Len(txtLecturaCodigo.Text)

            Case 12: ' Unidad de transporte / Ubicación --------------------------
                    'Comprobar si la lectura es un BAC
                    If fValidarBAC(txtLecturaCodigo.Text, True) = False Then
                        'Comprobar si la lectura es una ubicación
                        If fValidarUbicacion(txtLecturaCodigo.Text, True) = False Then
                            'No existe la ubicación / BAC
                            Call wsMensaje(" No se ha encontrado Ubicación o BAC", vbCritical)
                        End If
                    End If

        End Select

        txtLecturaCodigo.Text = ""
        
    End If
End Sub

Private Function fValidarBAC(ByVal stBAC As String, Optional ByVal blMensaje As Boolean = True) As Boolean
    Dim bCalculoPeso As Boolean
    Dim bCalculoVolumen As Boolean
    Dim tEstado As Integer
    
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
                RefrescarDatos False, 0, 0, 0, 0, 0, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, bCalculoPeso, bCalculoVolumen
            Else
                RefrescarDatos False, !ubicod, !ubialm, !ubiblo, !ubifil, !ubialt, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, bCalculoPeso, bCalculoVolumen
            End If
            
        Else
            'If blMensaje Then Call wsMensaje(" No existe el BAC ", vbCritical)
            Exit Function
        End If
        
        'Extracción del BAC
        If !uninum > 0 Then
            If RetirarBAC(!unicod, !uniest, oEstado(0).Value) Then
                Call wsMensaje(" Se ha extraído el BAC: " & !unicod & " de su ubicación de PTL ", MENSAJE_Exclamacion)
            End If
            
        Else
            If (!uniest = 0) = oEstado(0).Value Then
                If oEstado(0).Value Then tEstado = 1 Else tEstado = 0
                'Cambiar estado de BAC
                If CambiarEstadoBAC(!unicod, tEstado) Then
                    lbEstadoBAC = IIf(tEstado = 0, "ABIERTO", "CERRADO")
                    If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde Else lbEstadoBAC.BackColor = vbWhite
                End If
            
            Else
                Call wsMensaje(" El BAC ya está fuera de las ubicaciones de PTL ", vbCritical)
            End If
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
                    If blMensaje Then Call wsMensaje(" La Ubicación no tiene asociada un BAC válido ", vbCritical)
                End If
            End If
        Else
            If blMensaje Then Call wsMensaje(" No existe la Unidad de Transporte ", vbCritical)
            lbUbicacion = ""
        End If
        
        .Close
    End With

End Function

Private Sub RefrescarDatos(sEnBlanco As Boolean, Optional sCodUbicacion As Integer, Optional sALM As Integer, Optional sBLO As Integer, Optional sFIL As Integer, _
                            Optional sALT As Integer, Optional sBAC As String, Optional sEstadoBAC As Integer, Optional sGrupo As Integer, Optional sTablilla As Integer, _
                            Optional sPeso As Double, Optional sVolumen As Double, Optional sTipoCaja As String, Optional sNombreCaja As String, _
                            Optional bPeso As Boolean, Optional bVolumen As Boolean)

    If sEnBlanco = True Then
        'Inicia la visualización
        lbUbicacion = ""
        lbBAC = ""
        lbEstadoBAC = ""
        lbEstadoBAC.BackColor = vbWhite
        
        lbGrupo = ""
        lbTablilla = ""
        lbUds = ""
        
        lbPeso = ""
        lbPeso.BackColor = vbWhite
        
        lbVolumen = ""
        lbVolumen.BackColor = vbWhite
        
        lbTipoCaja = ""
        lbNombreCaja = ""
        
    Else
        If sCodUbicacion = 0 Then
            lbUbicacion = "SIN UBICACION"
            lbUbicacion = "-------------"
        Else
            lbUbicacion = "(" & sCodUbicacion & ") " & CStr(Format(sALM, "000")) & "." & CStr(Format(sBLO, "000")) & "." & CStr(Format(sFIL, "000")) & "." & CStr(Format(sALT, "000"))
        End If
        lbBAC = sBAC
        lbEstadoBAC = IIf(sEstadoBAC = 0, "ABIERTO", "CERRADO")
        If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde
        
        lbGrupo = sGrupo
        lbTablilla = sTablilla
        lbUds = 0
        
        lbPeso = Format(sPeso, "#0.000")
        If bPeso Then lbPeso.BackColor = ColorRojo
        
        lbVolumen = Format(sVolumen, "#0.000")
        If bVolumen Then lbVolumen.BackColor = ColorRojo
        
        lbTipoCaja = sTipoCaja
        lbNombreCaja = sNombreCaja
    End If

End Sub

Private Function RetirarBAC(tBac As String, tEstado As Integer, tEstadoFinal As Boolean) As Boolean
    'Extracción del BAC
    Dim Retorno As Integer
    Dim msgSalida As String
    Dim nEstado As Integer
    
    RetirarBAC = False
    
    ed.RetirarBACdePTL tBac, Usuario.Id, Retorno, msgSalida
    
    If Retorno = 0 Then
        RetirarBAC = True
        If (tEstado = 0) = tEstadoFinal Then
            If tEstadoFinal Then nEstado = 1 Else nEstado = 0
            'Cambiar estado de BAC
            If CambiarEstadoBAC(tBac, nEstado) Then
                lbEstadoBAC = IIf(tEstado = 0, "ABIERTO", "CERRADO")
                If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde Else lbEstadoBAC.BackColor = vbWhite
            End If
        End If
    Else
        Call wsMensaje(" No se ha podido retirar el BAC de la estanteria de PTL. " & msgSalida, vbCritical)
    End If
    
    
End Function

Private Function CambiarEstadoBAC(tBac As String, tEstado As Integer) As Boolean
    'Cambio de estado de BAC
    Dim Retorno As Integer
    Dim msgSalida As String
    
    CambiarEstadoBAC = False
    
    ed.CambiaEstadoBACdePTL tBac, tEstado, Usuario.Id, Retorno, msgSalida
    
    If Retorno = 0 Then
        CambiarEstadoBAC = True
    Else
        Call wsMensaje(" No se ha podido cambiar el estado al BAC " & msgSalida, vbCritical)
    End If
    
End Function
