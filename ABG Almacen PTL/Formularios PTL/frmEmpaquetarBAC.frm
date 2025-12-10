VERSION 5.00
Object = "{8CE292C1-A705-11D1-9EE6-000000000000}#92.7#0"; "GesData.ocx"
Object = "{8294475C-ED2D-4E90-9A02-9CA1A17809E4}#1.0#0"; "SSUltraGridTrial.ocx"
Object = "{00025600-0000-0000-C000-000000000046}#5.2#0"; "Crystl32.OCX"
Begin VB.Form frmEmpaquetarBAC 
   Appearance      =   0  'Flat
   BackColor       =   &H00B06000&
   BorderStyle     =   0  'None
   Caption         =   "Form1"
   ClientHeight    =   4515
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   21105
   KeyPreview      =   -1  'True
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   4515
   ScaleWidth      =   21105
   ShowInTaskbar   =   0   'False
   Begin VB.PictureBox FrameCombinar 
      Appearance      =   0  'Flat
      BackColor       =   &H00808080&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   4510
      Left            =   16000
      ScaleHeight     =   4515
      ScaleWidth      =   3810
      TabIndex        =   67
      Top             =   0
      Width           =   3805
      Begin GesData.Texto txtLecturaCaja2 
         Height          =   390
         Left            =   75
         TabIndex        =   100
         Top             =   1950
         Width           =   3645
         _ExtentX        =   6429
         _ExtentY        =   688
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "Arial"
            Size            =   14.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Alignment       =   2
         Appearance      =   0
         FocoBColor      =   12648384
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Combinar Cajas"
         Height          =   375
         Index           =   180
         Left            =   75
         TabIndex        =   95
         TabStop         =   0   'False
         ToolTipText     =   "Aplicar cambios"
         Top             =   3600
         Width           =   3665
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "VOLVER"
         Height          =   375
         Index           =   140
         Left            =   75
         TabIndex        =   68
         TabStop         =   0   'False
         ToolTipText     =   "Volver a pantalla anterior"
         Top             =   4100
         Width           =   3665
      End
      Begin VB.Label lbVolumenTot 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   2775
         TabIndex        =   99
         Top             =   3150
         Width           =   930
      End
      Begin VB.Label lbPesoTot 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   98
         Top             =   3150
         Width           =   930
      End
      Begin VB.Label Label42 
         Caption         =   "Vol. Total"
         Height          =   315
         Left            =   1875
         TabIndex        =   97
         Top             =   3150
         Width           =   840
      End
      Begin VB.Label Label35 
         Caption         =   "Peso Total"
         Height          =   315
         Left            =   75
         TabIndex        =   96
         Top             =   3150
         Width           =   765
      End
      Begin VB.Label lbVolumen4 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   2775
         TabIndex        =   94
         Top             =   2775
         Width           =   930
      End
      Begin VB.Label lbPeso4 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   93
         Top             =   2775
         Width           =   930
      End
      Begin VB.Label Label32 
         Caption         =   "Volumen"
         Height          =   315
         Left            =   1875
         TabIndex        =   92
         Top             =   2775
         Width           =   840
      End
      Begin VB.Label Label31 
         Caption         =   "Peso"
         Height          =   315
         Left            =   75
         TabIndex        =   91
         Top             =   2775
         Width           =   765
      End
      Begin VB.Label Label41 
         Caption         =   "Art."
         Height          =   315
         Left            =   2700
         TabIndex        =   90
         Top             =   2400
         Width           =   465
      End
      Begin VB.Label lbArts4 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   3225
         TabIndex        =   89
         Top             =   2400
         Width           =   480
      End
      Begin VB.Label Label39 
         Caption         =   "Uds"
         Height          =   315
         Left            =   1575
         TabIndex        =   88
         Top             =   2400
         Width           =   465
      End
      Begin VB.Label lbUds4 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   2100
         TabIndex        =   87
         Top             =   2400
         Width           =   480
      End
      Begin VB.Label Label37 
         Caption         =   "Nº Caja"
         Height          =   315
         Left            =   75
         TabIndex        =   86
         Top             =   2400
         Width           =   765
      End
      Begin VB.Label lbNumCaja4 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   85
         Top             =   2400
         Width           =   555
      End
      Begin VB.Label Label30 
         Alignment       =   2  'Center
         Caption         =   "Leer caja a combinar (última caja!!)"
         Height          =   240
         Left            =   75
         TabIndex        =   84
         Top             =   1650
         Width           =   3645
      End
      Begin VB.Label Label29 
         Caption         =   "Art."
         Height          =   315
         Left            =   2700
         TabIndex        =   83
         Top             =   150
         Width           =   465
      End
      Begin VB.Label lbArts3 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   3225
         TabIndex        =   82
         Top             =   150
         Width           =   480
      End
      Begin VB.Label Label27 
         Caption         =   "Uds"
         Height          =   315
         Left            =   1575
         TabIndex        =   81
         Top             =   150
         Width           =   390
      End
      Begin VB.Label lbUds3 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   2025
         TabIndex        =   80
         Top             =   150
         Width           =   480
      End
      Begin VB.Label lbVolumen3 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   2850
         TabIndex        =   79
         Top             =   1275
         Width           =   855
      End
      Begin VB.Label lbPeso3 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   78
         Top             =   1275
         Width           =   855
      End
      Begin VB.Label Label23 
         Caption         =   "Volumen"
         Height          =   315
         Left            =   1950
         TabIndex        =   77
         Top             =   1275
         Width           =   840
      End
      Begin VB.Label Label22 
         Caption         =   "Peso"
         Height          =   315
         Left            =   75
         TabIndex        =   76
         Top             =   1275
         Width           =   765
      End
      Begin VB.Label Label21 
         Caption         =   "Nº Caja"
         Height          =   315
         Left            =   75
         TabIndex        =   75
         Top             =   150
         Width           =   765
      End
      Begin VB.Label lbNumCaja3 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   74
         Top             =   150
         Width           =   555
      End
      Begin VB.Label lbSSCC3 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   73
         Top             =   525
         Width           =   2805
      End
      Begin VB.Label Label18 
         Caption         =   "SSCC"
         Height          =   315
         Left            =   75
         TabIndex        =   72
         Top             =   525
         Width           =   765
      End
      Begin VB.Label Label17 
         Caption         =   "Tipo Caja"
         Height          =   315
         Left            =   75
         TabIndex        =   71
         Top             =   900
         Width           =   765
      End
      Begin VB.Label lbTipoCaja3 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   70
         Top             =   900
         Width           =   555
      End
      Begin VB.Label lbNombreCaja3 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   1500
         TabIndex        =   69
         Top             =   900
         Width           =   2205
      End
   End
   Begin VB.PictureBox FrameCantidad 
      Appearance      =   0  'Flat
      BackColor       =   &H00808080&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   4510
      Left            =   8000
      ScaleHeight     =   4515
      ScaleWidth      =   3810
      TabIndex        =   53
      Top             =   0
      Width           =   3805
      Begin VB.CommandButton cmdAccion 
         Caption         =   "   "
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   24
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   -1  'True
         EndProperty
         Height          =   855
         Index           =   90
         Left            =   75
         TabIndex        =   63
         ToolTipText     =   "Restar"
         Top             =   2250
         Width           =   930
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "+"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   24
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   855
         Index           =   95
         Left            =   2835
         TabIndex        =   62
         ToolTipText     =   "Sumar"
         Top             =   2250
         Width           =   915
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "VOLVER"
         Height          =   375
         Index           =   130
         Left            =   75
         TabIndex        =   60
         TabStop         =   0   'False
         ToolTipText     =   "Volver a pantalla anterior"
         Top             =   4100
         Width           =   3665
      End
      Begin VB.CommandButton cmdAccion 
         Height          =   645
         Index           =   110
         Left            =   75
         Picture         =   "frmEmpaquetarBAC.frx":0000
         Style           =   1  'Graphical
         TabIndex        =   54
         ToolTipText     =   "Aplicar cambios"
         Top             =   3300
         Width           =   3665
      End
      Begin GesData.Numero nCantidad 
         Height          =   855
         Left            =   1200
         TabIndex        =   55
         TabStop         =   0   'False
         Top             =   2250
         Width           =   1440
         _ExtentX        =   2540
         _ExtentY        =   1508
         BackColor       =   16777215
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   24
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         BeginProperty FuenteEtiqueta {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   18
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         FondoEtiqueta   =   12640511
         MaxLength       =   14
         Alignment       =   2
         Appearance      =   0
         TeclaEnter      =   -1  'True
         LinkItem        =   "Text1"
         ValorDefecto    =   "0"
         FocoBColor      =   12648447
      End
      Begin GesData.Numero nArticulo 
         Height          =   285
         Left            =   900
         TabIndex        =   56
         TabStop         =   0   'False
         Top             =   600
         Width           =   2850
         _ExtentX        =   5027
         _ExtentY        =   503
         BackColor       =   16777215
         BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         BeginProperty FuenteEtiqueta {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         FondoEtiqueta   =   14737632
         Locked          =   -1  'True
         Alignment       =   0
         Appearance      =   0
         LinkItem        =   "Text1"
         FocoBColor      =   12648447
      End
      Begin VB.Label Label10 
         Alignment       =   2  'Center
         BackColor       =   &H00E0E0E0&
         Caption         =   "CAMBIO DE UNIDADES"
         BeginProperty Font 
            Name            =   "Arial"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   375
         Left            =   75
         TabIndex        =   61
         Top             =   75
         Width           =   3675
      End
      Begin VB.Label lblCantidad 
         Alignment       =   2  'Center
         BackColor       =   &H00E0E0E0&
         Caption         =   "CONFIRME LA CANTIDAD  DEL ARTÍCULO"
         BeginProperty Font 
            Name            =   "Arial"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   675
         Left            =   75
         TabIndex        =   59
         Top             =   1425
         Width           =   3675
      End
      Begin VB.Label Label8 
         BackColor       =   &H00C0C0C0&
         Caption         =   "Artículo"
         Height          =   285
         Left            =   75
         TabIndex        =   58
         Top             =   600
         Width           =   765
      End
      Begin VB.Label lbNombreArticulo 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   285
         Left            =   75
         TabIndex        =   57
         Top             =   975
         Width           =   3675
      End
   End
   Begin VB.PictureBox FrameCajas 
      Appearance      =   0  'Flat
      BackColor       =   &H00808080&
      BorderStyle     =   0  'None
      ForeColor       =   &H80000008&
      Height          =   4510
      Left            =   12000
      ScaleHeight     =   4515
      ScaleWidth      =   3810
      TabIndex        =   41
      Top             =   0
      Width           =   3805
      Begin VB.CommandButton cmdAccion 
         Caption         =   "VOLVER"
         Height          =   375
         Index           =   120
         Left            =   75
         TabIndex        =   51
         TabStop         =   0   'False
         ToolTipText     =   "Volver a pantalla anterior"
         Top             =   4100
         Width           =   3665
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Aplicar cambio de tipo de caja"
         Height          =   375
         Index           =   170
         Left            =   75
         TabIndex        =   50
         TabStop         =   0   'False
         ToolTipText     =   "Aplicar cambios"
         Top             =   3600
         Width           =   3665
      End
      Begin UltraGrid.SSUltraGrid ugCajas 
         Height          =   2130
         Left            =   75
         TabIndex        =   49
         ToolTipText     =   "Artículos contenidos"
         Top             =   1275
         Width           =   3675
         _ExtentX        =   6482
         _ExtentY        =   3757
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
      Begin VB.Label lbNombreCaja2 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   1500
         TabIndex        =   48
         Top             =   900
         Width           =   2205
      End
      Begin VB.Label lbTipoCaja2 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   47
         Top             =   900
         Width           =   555
      End
      Begin VB.Label Label15 
         Caption         =   "Tipo Caja"
         Height          =   315
         Left            =   75
         TabIndex        =   46
         Top             =   900
         Width           =   765
      End
      Begin VB.Label Label14 
         Caption         =   "SSCC"
         Height          =   315
         Left            =   75
         TabIndex        =   45
         Top             =   525
         Width           =   765
      End
      Begin VB.Label lbSSCC 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   44
         Top             =   525
         Width           =   2805
      End
      Begin VB.Label lbNumCaja2 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   315
         Left            =   900
         TabIndex        =   43
         Top             =   150
         Width           =   555
      End
      Begin VB.Label Label11 
         Caption         =   "Nº Caja"
         Height          =   315
         Left            =   75
         TabIndex        =   42
         Top             =   150
         Width           =   765
      End
   End
   Begin VB.Frame FrameOpciones 
      Appearance      =   0  'Flat
      BackColor       =   &H00808080&
      BorderStyle     =   0  'None
      Caption         =   "Frame1"
      ForeColor       =   &H80000008&
      Height          =   4510
      Left            =   4000
      TabIndex        =   21
      Top             =   0
      Width           =   3805
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Combinar cajas"
         Height          =   375
         Index           =   85
         Left            =   75
         TabIndex        =   66
         TabStop         =   0   'False
         ToolTipText     =   "Cambiar unidades"
         Top             =   3660
         Width           =   3665
      End
      Begin VB.CheckBox Check1 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Empaquetado automático"
         ForeColor       =   &H80000008&
         Height          =   315
         Index           =   6
         Left            =   75
         TabIndex        =   40
         Top             =   2400
         Width           =   2190
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Empaquetado"
         Height          =   315
         Index           =   60
         Left            =   2375
         TabIndex        =   39
         TabStop         =   0   'False
         Top             =   2400
         Width           =   1365
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Cambiar Unidades Articulo"
         Height          =   375
         Index           =   80
         Left            =   75
         TabIndex        =   38
         TabStop         =   0   'False
         ToolTipText     =   "Cambiar unidades"
         Top             =   3225
         Width           =   3665
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "VOLVER"
         Height          =   375
         Index           =   100
         Left            =   75
         TabIndex        =   37
         TabStop         =   0   'False
         ToolTipText     =   "Volver a pantalla anterior"
         Top             =   4100
         Width           =   3665
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Cambiar tipo de caja"
         Height          =   375
         Index           =   70
         Left            =   75
         TabIndex        =   36
         TabStop         =   0   'False
         ToolTipText     =   "Cambiar Caja"
         Top             =   2775
         Width           =   3665
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "R. Contenido"
         Height          =   315
         Index           =   50
         Left            =   2375
         TabIndex        =   35
         TabStop         =   0   'False
         ToolTipText     =   "[F10]"
         Top             =   2025
         Width           =   1365
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Imprimir CAJA"
         Height          =   315
         Index           =   40
         Left            =   2375
         TabIndex        =   34
         TabStop         =   0   'False
         ToolTipText     =   "[F4]"
         Top             =   1650
         Width           =   1365
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Crear CAJA"
         Height          =   315
         Index           =   30
         Left            =   2375
         TabIndex        =   33
         TabStop         =   0   'False
         ToolTipText     =   "[F3]"
         Top             =   1275
         Width           =   1365
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Extraer BAC"
         Height          =   315
         Index           =   20
         Left            =   2375
         TabIndex        =   32
         TabStop         =   0   'False
         Top             =   900
         Width           =   1365
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   "Cerrar BAC"
         Height          =   315
         Index           =   10
         Left            =   2375
         TabIndex        =   31
         TabStop         =   0   'False
         ToolTipText     =   "[F6]"
         Top             =   525
         Width           =   1365
      End
      Begin VB.CheckBox Check1 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Imprimir rel. contenido"
         ForeColor       =   &H80000008&
         Height          =   315
         Index           =   4
         Left            =   75
         TabIndex        =   30
         Top             =   2025
         Width           =   2190
      End
      Begin VB.CheckBox Check1 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Imprimir etiqueta de caja"
         ForeColor       =   &H80000008&
         Height          =   315
         Index           =   3
         Left            =   75
         TabIndex        =   29
         Top             =   1650
         Width           =   2190
      End
      Begin VB.CheckBox Check1 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Crear caja según BAC"
         ForeColor       =   &H80000008&
         Height          =   315
         Index           =   2
         Left            =   75
         TabIndex        =   28
         Top             =   1275
         Width           =   2190
      End
      Begin VB.CheckBox Check1 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Extraer BAC ubicados"
         ForeColor       =   &H80000008&
         Height          =   315
         Index           =   1
         Left            =   75
         TabIndex        =   27
         Top             =   900
         Width           =   2190
      End
      Begin VB.CheckBox Check1 
         Appearance      =   0  'Flat
         BackColor       =   &H80000005&
         Caption         =   "Cerrar BAC abiertos"
         ForeColor       =   &H80000008&
         Height          =   315
         Index           =   0
         Left            =   75
         TabIndex        =   26
         Top             =   525
         Width           =   2190
      End
      Begin VB.Label Label2 
         Caption         =   "Ubicación"
         Height          =   285
         Left            =   75
         TabIndex        =   23
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
         TabIndex        =   22
         Top             =   75
         Width           =   2805
      End
   End
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
      Begin VB.CommandButton cmdAccion 
         Caption         =   "ACCIONES"
         Height          =   375
         Index           =   5
         Left            =   600
         TabIndex        =   25
         TabStop         =   0   'False
         ToolTipText     =   "Acciones inmediatas"
         Top             =   4100
         Width           =   2415
      End
      Begin VB.CommandButton cmdAccion 
         Caption         =   ">>"
         Height          =   375
         Index           =   0
         Left            =   3135
         TabIndex        =   24
         TabStop         =   0   'False
         ToolTipText     =   "Ver opciones"
         Top             =   4100
         Width           =   615
      End
      Begin VB.Frame fraArticulo 
         Appearance      =   0  'Flat
         BackColor       =   &H00C0C0C0&
         BorderStyle     =   0  'None
         ForeColor       =   &H80000008&
         Height          =   1575
         Left            =   75
         TabIndex        =   5
         Top             =   600
         Width           =   3675
         Begin VB.Label Label12 
            Caption         =   "Caja"
            Height          =   285
            Left            =   2625
            TabIndex        =   64
            Top             =   450
            Width           =   465
         End
         Begin VB.Label lbNumCaja 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   3150
            TabIndex        =   52
            Top             =   450
            Width           =   405
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
            TabIndex        =   20
            Top             =   75
            Width           =   2655
         End
         Begin VB.Label Label3 
            Caption         =   "BAC"
            Height          =   285
            Left            =   75
            TabIndex        =   19
            Top             =   75
            Width           =   765
         End
         Begin VB.Label Label1 
            Caption         =   "Grupo"
            Height          =   285
            Left            =   75
            TabIndex        =   18
            Top             =   450
            Width           =   765
         End
         Begin VB.Label Label4 
            Caption         =   "Tablilla"
            Height          =   285
            Left            =   1500
            TabIndex        =   17
            Top             =   450
            Width           =   615
         End
         Begin VB.Label Label5 
            Caption         =   "Peso"
            Height          =   285
            Left            =   75
            TabIndex        =   16
            Top             =   825
            Width           =   765
         End
         Begin VB.Label Label6 
            Caption         =   "Volumen"
            Height          =   285
            Left            =   1725
            TabIndex        =   15
            Top             =   825
            Width           =   840
         End
         Begin VB.Label Label7 
            Caption         =   "Tipo Caja"
            Height          =   285
            Left            =   75
            TabIndex        =   14
            Top             =   1200
            Width           =   765
         End
         Begin VB.Label lbGrupo 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   13
            Top             =   450
            Width           =   555
         End
         Begin VB.Label lbTablilla 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   2175
            TabIndex        =   12
            Top             =   450
            Width           =   405
         End
         Begin VB.Label lbPeso 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   11
            Top             =   825
            Width           =   780
         End
         Begin VB.Label lbVolumen 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   2625
            TabIndex        =   10
            Top             =   825
            Width           =   930
         End
         Begin VB.Label lbTipoCaja 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   900
            TabIndex        =   9
            Top             =   1200
            Width           =   405
         End
         Begin VB.Label lbNombreCaja 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   1350
            TabIndex        =   8
            Top             =   1200
            Width           =   1215
         End
         Begin VB.Label lbUds 
            Alignment       =   2  'Center
            BackColor       =   &H00FFFFFF&
            Height          =   285
            Left            =   3150
            TabIndex        =   7
            Top             =   1200
            Width           =   405
         End
         Begin VB.Label Label9 
            Caption         =   "Uds"
            Height          =   285
            Left            =   2625
            TabIndex        =   6
            Top             =   1200
            Width           =   465
         End
      End
      Begin VB.Frame fraArticulos 
         Appearance      =   0  'Flat
         BackColor       =   &H00E0E0E0&
         BorderStyle     =   0  'None
         Caption         =   "Datos de Tablillas de Articulo"
         ForeColor       =   &H80000008&
         Height          =   1770
         Left            =   75
         TabIndex        =   3
         Top             =   2250
         Width           =   3675
         Begin UltraGrid.SSUltraGrid ugArticulos 
            Height          =   1635
            Left            =   75
            TabIndex        =   4
            ToolTipText     =   "Lista de Artículos"
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
      Begin VB.CommandButton cmdAccion 
         Caption         =   "SALIR"
         Height          =   450
         Index           =   990
         Left            =   2850
         TabIndex        =   1
         TabStop         =   0   'False
         ToolTipText     =   "Salir de Empaquetar"
         Top             =   75
         Width           =   915
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
         Top             =   75
         Width           =   2700
      End
      Begin VB.Label lbArts 
         Alignment       =   2  'Center
         BackColor       =   &H00FFFFFF&
         Height          =   375
         Left            =   75
         TabIndex        =   65
         Top             =   4100
         Width           =   390
      End
   End
   Begin Crystal.CrystalReport cReport 
      Left            =   20325
      Top             =   0
      _ExtentX        =   741
      _ExtentY        =   741
      _Version        =   348160
      PrintFileLinesPerPage=   60
   End
End
Attribute VB_Name = "frmEmpaquetarBAC"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'***********************************************************************
'Nombre: frmEmpaquetarBAC
' Formulario de empaquetado rápido de BAC del sistema de PTL
'
'Creación:      05/06/20
'
'Realización:   A.Esteban
'
'***********************************************************************
Option Explicit
Option Compare Text

' ----- Constantes de Módulo -------------
Private Const MOD_Nombre = "Empaquetar BAC"

Private Const CML_Salir = 990

Private Const CML_Opciones = 0
Private Const CML_Acciones = 5

Private Const CML_CerrarBAC = 10
Private Const CML_ExtraerBAC = 20
Private Const CML_CrearCAJA = 30
Private Const CML_ImprimirCAJA = 40
Private Const CML_RelContenido = 50
Private Const CML_Empaquetado = 60
Private Const CML_CambiarCAJA = 70
Private Const CML_CambiarUDS = 80
Private Const CML_CombinarCAJAS = 85

Private Const CML_RestarUDS = 90
Private Const CML_SumarUDS = 95
Private Const CML_AplicarUDS = 110

Private Const CML_Volver = 100
Private Const CML_CambiaCAJA = 170
Private Const CML_AplicaCombinar = 180

Private Const CML_VolverCajas = 120
Private Const CML_VolverCantidad = 130
Private Const CML_VolverCombinar = 140


Private Const OPC_CerrarBAC = 0
Private Const OPC_ExtraerBAC = 1
Private Const OPC_CrearCAJA = 2
Private Const OPC_ImprimirCAJA = 3
Private Const OPC_RelContenido = 4
Private Const OPC_CerrarCAJA = 5
Private Const OPC_Empaquetado = 6

Private Const ACC_General = "ACCIONES"
Private Const ACC_Empaquetar = "EMPAQUETAR"
Private Const ACC_Etiquetas = "IMPRIMIR ETIQUETAS"


Private Const LIS_ContenidoBAC = 1
Private Const LIS_ContenidoCAJA = 2
Private Const LIS_TipoCajas = 3

Private Const ColorRojo = &H8080FF
Private Const ColorVerde = &H80FF80

' ----- Variables generales -------------
Private ed As New EntornoDeDatos
Private r_Art As ADODB.Recordset
Private r_ArtC As ADODB.Recordset
Private r_Caj As ADODB.Recordset


Private tEstadoBAC As Integer   'Estado del BAC
Private tUbicacionBAC As Integer  'Ubicacion del BAC

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
    
    'Leer opciones del Ini
    Check1(OPC_CerrarBAC).Value = LeerIni(ficINI, "Opciones", "CerrarBAC", 0)
    Check1(OPC_ExtraerBAC).Value = LeerIni(ficINI, "Opciones", "ExtraerBAC", 0)
    Check1(OPC_CrearCAJA).Value = LeerIni(ficINI, "Opciones", "CrearCAJA", 0)
    Check1(OPC_ImprimirCAJA).Value = LeerIni(ficINI, "Opciones", "ImprimirCAJA", 0)
    Check1(OPC_RelContenido).Value = LeerIni(ficINI, "Opciones", "RelContenido", 0)
    Check1(OPC_Empaquetado).Value = LeerIni(ficINI, "Opciones", "Empaquetado", 0)

    cmdAccion(CML_CerrarBAC).Enabled = False
    cmdAccion(CML_ExtraerBAC).Enabled = False
    cmdAccion(CML_CrearCAJA).Enabled = False
    cmdAccion(CML_ImprimirCAJA).Enabled = False
    cmdAccion(CML_RelContenido).Enabled = False
    cmdAccion(CML_Empaquetado).Enabled = False
    cmdAccion(CML_CambiarCAJA).Enabled = False
    cmdAccion(CML_CambiarUDS).Enabled = False
    cmdAccion(CML_CombinarCAJAS).Enabled = False

    'Acciones de la lectura
    cmdAccion(CML_Acciones).Caption = ACC_General

    MousePointer = ccDefault

End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
    Select Case KeyCode

        Case vbKeyEscape:   Unload Me

        Case vbKeyF3:   Call cmdAccion_Click(CML_CrearCAJA)
            
        Case vbKeyF4:   Call cmdAccion_Click(CML_ImprimirCAJA)
        
        Case vbKeyF6:   Call cmdAccion_Click(CML_CerrarBAC)
        
        Case vbKeyF10:  Call cmdAccion_Click(CML_RelContenido)

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

Private Sub Check1_Click(index As Integer)
    Select Case index
        Case OPC_CerrarBAC
            GuardarIni ficINI, "Opciones", "CerrarBAC", Check1(index).Value
            If Check1(index).Value = 0 Then Check1(OPC_ExtraerBAC).Value = 0
        
        Case OPC_ExtraerBAC
            GuardarIni ficINI, "Opciones", "ExtraerBAC", Check1(index).Value
            If Check1(index).Value = 1 Then Check1(OPC_CerrarBAC).Value = 1
        
        Case OPC_CrearCAJA
            GuardarIni ficINI, "Opciones", "CrearCAJA", Check1(index).Value
        
        Case OPC_ImprimirCAJA
            GuardarIni ficINI, "Opciones", "ImprimirCAJA", Check1(index).Value
        
        Case OPC_RelContenido
            GuardarIni ficINI, "Opciones", "RelContenido", Check1(index).Value
        
        Case OPC_Empaquetado
            GuardarIni ficINI, "Opciones", "Empaquetado", Check1(index).Value
    
    End Select

End Sub

Private Sub IniciaLista(index As Integer)
    Select Case index
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
        
        Case LIS_TipoCajas
            'Lista de tipos de CAJA
            Set r_Caj = New ADODB.Recordset
        
            With r_Caj
                .Fields.Append "tipcod", adBigInt, , adFldUpdatable + adFldMayBeNull
                .Fields.Append "tipdes", adVarChar, 50, adFldUpdatable + adFldMayBeNull
                
                .CursorType = adOpenKeyset
                .LockType = adLockOptimistic
                .Open
            End With
        
    End Select

End Sub

Private Sub cmdAccion_Click(index As Integer)

    If cmdAccion(index).Enabled = True Then
    
        Select Case index
            Case CML_Opciones
                PantallaOpciones
            
            Case CML_Acciones
                AccionesAuto
            
            Case CML_Volver
                PantallaPrincipal
        
            'Acciones principales
            Case CML_CerrarBAC
                CerrarBAC
                PantallaPrincipal
                
            Case CML_ExtraerBAC
                ExtraerBAC
                PantallaPrincipal
            
            Case CML_CrearCAJA
                CrearCAJA
                PantallaPrincipal
            
            Case CML_ImprimirCAJA
                ImprimirETIQUETA
                PantallaPrincipal
                
            Case CML_RelContenido
                ImprimirRELACION
                PantallaPrincipal
            
            Case CML_Empaquetado
                EmpaquetarBACaCAJA
                PantallaPrincipal
            
            'Acciones adicionales
            Case CML_CambiarCAJA
                PantallaCambioCaja
            
            Case CML_CambiarUDS
                PantallaCambioUnidades
                
            Case CML_SumarUDS
                ModificaUnidades (1)
                
            Case CML_RestarUDS
                ModificaUnidades (-1)
            
            Case CML_AplicarUDS
                CambiarUnidades
            
            Case CML_VolverCantidad
                'PantallaOpciones
                PantallaPrincipal
                
            'Cambio de caja
            Case CML_CambiaCAJA
                CambiaTipoCaja
            
            Case CML_VolverCajas
                'PantallaOpciones
                PantallaPrincipal
            
            'Combinar cajas
            Case CML_CombinarCAJAS
                PantallaCombinarCajas
                
            Case CML_VolverCombinar
                PantallaOpciones
                
            Case CML_AplicaCombinar
                CombinarCajas lbSSCC3, txtLecturaCaja2
            
            'Salir
            Case CML_Salir
                Salir
        
        End Select
        
    End If
    
    If index <> CML_Salir And index <> CML_CombinarCAJAS Then txtLecturaCodigo.SetFocus

End Sub

Private Sub AccionesAuto()
    Select Case cmdAccion(CML_Acciones).Caption
        Case ACC_General
            PantallaOpciones
            
        Case ACC_Empaquetar
            EmpaquetarBACaCAJA
            
        Case ACC_Etiquetas
            ImprimirETIQUETA
            ImprimirRELACION
            
    End Select
    
End Sub

Private Sub lbNombreCaja_Click()
    PantallaCambioCaja
End Sub

Private Sub PantallaPrincipal()
    FrameLectura.Left = 0
    FrameOpciones.Left = 4000
    FrameCantidad.Left = 8000
    FrameCajas.Left = 12000
    FrameCombinar.Left = 16000
    txtLecturaCodigo.SetFocus
End Sub

Private Sub PantallaOpciones()

    'Se deshabilitan todas las opciones
    cmdAccion(CML_CerrarBAC).Enabled = False
    cmdAccion(CML_ExtraerBAC).Enabled = False
    cmdAccion(CML_CrearCAJA).Enabled = False
    cmdAccion(CML_ImprimirCAJA).Enabled = False
    cmdAccion(CML_RelContenido).Enabled = False
    cmdAccion(CML_Empaquetado).Enabled = False
    cmdAccion(CML_CambiarCAJA).Enabled = False
    cmdAccion(CML_CambiarUDS).Enabled = False
    cmdAccion(CML_CombinarCAJAS).Enabled = False

    'Configura opciones de BAC
    If Label3.Caption = "BAC" Then
        If lbBAC <> "" And tEstadoBAC = 0 Then cmdAccion(CML_CerrarBAC).Enabled = True
        If tUbicacionBAC > 0 Then cmdAccion(CML_ExtraerBAC).Enabled = True
        If Val(lbNumCaja) = 0 Then cmdAccion(CML_CrearCAJA).Enabled = True
        If Val(lbNumCaja) > 0 Then cmdAccion(CML_ImprimirCAJA).Enabled = True
        If Val(lbNumCaja) > 0 Or tUbicacionBAC = 0 Then cmdAccion(CML_CambiarCAJA).Enabled = True
            
        cmdAccion(CML_Empaquetado).Enabled = True

    End If
    
    'Configura opciones de CAJA
    If Label3.Caption = "CAJA" Then
        cmdAccion(CML_ImprimirCAJA).Enabled = True
        cmdAccion(CML_RelContenido).Enabled = True
        cmdAccion(CML_CambiarCAJA).Enabled = True
        cmdAccion(CML_CambiarUDS).Enabled = True
        cmdAccion(CML_CombinarCAJAS).Enabled = True
    End If
    
    'Visualiza en el formulario
    FrameLectura.Left = 4000
    FrameCantidad.Left = 8000
    FrameCajas.Left = 12000
    FrameCombinar.Left = 16000
    FrameOpciones.Left = 0
End Sub

Private Sub PantallaCambioCaja()
    'Cambio de tipo de Caja
    FrameOpciones.Left = 4000
    FrameCantidad.Left = 8000
    FrameLectura.Left = 12000
    FrameCombinar.Left = 16000
    FrameCajas.Left = 0
    
    Set ugCajas.DataSource = Nothing
    
    IniciaLista LIS_TipoCajas
    
    'Carga la lista de tipos de caja
    If ed.rsDameTiposCajasActivas.State <> adStateClosed Then ed.rsDameTiposCajasActivas.Close
    ed.DameTiposCajasActivas
    
    With ed.rsDameTiposCajasActivas
        If .RecordCount > 0 Then
            .MoveFirst
            Do Until .EOF
                r_Caj.AddNew
                r_Caj!tipcod = !tipcod
                r_Caj!tipdes = !tipdes
                r_Caj.Update
                .MoveNext
            Loop
            
            'Se posiciona en la caja actual
            r_Caj.MoveFirst
            Do Until r_Caj.EOF
                If r_Caj!tipcod = Val(lbTipoCaja2.Caption) Then Exit Do
                r_Caj.MoveNext
            Loop
            If r_Caj.EOF = True Then r_Caj.MoveFirst
        
            Set ugCajas.DataSource = r_Caj
        Else
            Call wsMensaje("No Existen Cajas de Empaquetar Definidas.", vbInformation)
        End If
        
        .Close
    End With
    
    ugCajas.SetFocus
    
End Sub

Private Sub PantallaCambioUnidades()
    'Cambio de unidades de artículo. Sólo permitido en CAJAS
    FrameLectura.Left = 8000
    FrameOpciones.Left = 4000
    FrameCajas.Left = 12000
    FrameCombinar.Left = 16000
    FrameCantidad.Left = 0

    'Posiciona el artículo actual
    If r_ArtC.RecordCount > 0 Then
        nArticulo = r_ArtC!ltaart
        nCantidad = r_ArtC!ltacan
        lbNombreArticulo = r_ArtC!artnom
    Else
        nArticulo = 0
        nCantidad = 0
        lbNombreArticulo = ""
    End If

    nArticulo.SetFocus
    
End Sub

Private Sub PantallaCombinarCajas()
    'Cambio de tipo de Caja
    FrameLectura.Left = 16000
    FrameOpciones.Left = 4000
    FrameCantidad.Left = 8000
    FrameCajas.Left = 12000
    FrameCombinar.Left = 0
    
    'Datos Caja 1
    lbNumCaja3 = lbNumCaja
    lbUds3 = lbUds
    lbArts3 = lbArts
    
    lbSSCC3 = lbSSCC
    lbTipoCaja3 = lbTipoCaja
    lbNombreCaja3 = lbNombreCaja
    lbPeso3 = lbPeso
    lbVolumen3 = lbVolumen
    
    'Datos Caja 2
    txtLecturaCaja2 = ""
    lbNumCaja4 = ""
    lbUds4 = ""
    lbArts4 = ""
    lbPeso4 = ""
    lbVolumen4 = ""
    
    'Datos totales
    lbPesoTot = ""
    lbVolumenTot = ""
    
    cmdAccion(CML_AplicaCombinar).Enabled = False
    txtLecturaCaja2.Enabled = True
    
    txtLecturaCaja2.SetFocus
    
End Sub

Private Sub txtLecturaCaja2_KeyDown(KeyCode As Integer, Shift As Integer)
    If KeyCode = vbKeyReturn Then
        
        Select Case Len(txtLecturaCaja2)

            Case 18, 20: 'SSCC de Caja

                If Len(txtLecturaCaja2) = 20 Then
                    txtLecturaCaja2 = Mid(txtLecturaCaja2, 3, 18)
                End If
                
                
                'Comprobación de datos: Caja repetida
                If CStr(txtLecturaCaja2) = lbBAC Then
                    wsMensaje " La caja está repetida! ", vbCritical
                    cmdAccion(CML_AplicaCombinar).Enabled = False
                    txtLecturaCaja2 = ""
                    txtLecturaCaja2.SetFocus
                    Exit Sub
                End If
                
                
                If ed.rsDameDatosCAJAdePTL.State <> adStateClosed Then ed.rsDameDatosCAJAdePTL.Close
                ed.DameDatosCAJAdePTL CStr(txtLecturaCaja2)
                
                With ed.rsDameDatosCAJAdePTL
                    'Existencia del registro
                    If .RecordCount > 0 Then
                    
                        'Comprobación de datos: Grupo y Tablilla
                        If lbGrupo <> !ltcgru Or lbTablilla <> !ltctab Then
                            wsMensaje " La caja no pertenece al Grupo / Tablilla ", vbCritical
                            cmdAccion(CML_AplicaCombinar).Enabled = False
                            txtLecturaCaja2 = ""
                            txtLecturaCaja2.SetFocus
                            .Close
                            Exit Sub
                        End If
                        
                        'Se muestran los datos
                        '!ltcssc, bEstado, !ltcgru, !ltctab, !ltcpes, !ltcvol, !ltctip, !tipdes, !ltccaj, bCalculoPeso, bCalculoVolumen
                        
                        'Datos Caja 2
                        lbNumCaja4 = !ltccaj
                        lbUds4 = ""
                        lbArts4 = ""
                        lbPeso4 = 0
                        lbVolumen4 = 0
                        
                        'Datos totales
                        lbPesoTot = Format((CDbl(lbPeso3) + CDbl(lbPeso4)), "#0.000")
                        lbVolumenTot = Format((CDbl(lbVolumen3) + CDbl(lbVolumen4)), "#0.000")
                        
                        txtLecturaCaja2.Enabled = False
                        cmdAccion(CML_AplicaCombinar).Enabled = True
                        cmdAccion(CML_AplicaCombinar).SetFocus
                        
                    Else
                        wsMensaje " No existe la CAJA ", vbCritical
                        cmdAccion(CML_AplicaCombinar).Enabled = False
                        txtLecturaCaja2 = ""
                        txtLecturaCaja2.SetFocus
                        
                    End If
                
                    .Close
                End With
                
            Case Else
                wsMensaje " No existe la CAJA ", vbCritical

        End Select
        
    End If

End Sub

Private Sub txtLecturaCodigo_GotFocus()
    txtLecturaCodigo.BackColor = &HC0FFC0
End Sub

Private Sub txtLecturaCodigo_LostFocus()
    txtLecturaCodigo.BackColor = &H80000005
End Sub

Private Sub txtLecturaCodigo_KeyDown(KeyCode As Integer, Shift As Integer)

    If KeyCode = vbKeyReturn Then
        
        'Inicializa la visualización
        RefrescarDatos True

        Select Case Len(txtLecturaCodigo.Text)

            Case 12: ' Unidad de transporte --------------------------
                'Comprobar si la lectura es un BAC
                Label3.Caption = "BAC"
                fValidarBAC txtLecturaCodigo.Text, True
            
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
            tEstadoBAC = !uniest
            tUbicacionBAC = !uninum
            
            'Se muestran los datos
            If IsNull(!ubicod) Then
                RefrescarDatos False, 0, 0, 0, 0, 0, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, !uninca, bCalculoPeso, bCalculoVolumen
            Else
                RefrescarDatos False, !ubicod, !ubialm, !ubiblo, !ubifil, !ubialt, !unicod, !uniest, !unigru, !unitab, !unipes, !univol, !unicaj, !tipdes, !uninca, bCalculoPeso, bCalculoVolumen
            End If
            
            'Lista de artículos contenidos en el BAC
            Call sRefrescarArticulosBAC(!unigru, !unicod)
            
            'Comprueba el estado del BAC
            If !uniest = 0 Then
                If Check1(OPC_CerrarBAC).Value = 1 Then
                    CerrarBAC
                Else
                    If blMensaje Then Call wsMensaje(" El BAC está abierto!!", vbCritical)
                    .Close
                    Exit Function
                End If
            End If
            
            'Comprueba si está ubicado el BAC
            If !uninum > 0 Then
                If Check1(OPC_ExtraerBAC).Value = 1 Then
                    ExtraerBAC
                Else
                    If blMensaje Then Call wsMensaje(" El BAC está ubicado!!", vbCritical)
                    .Close
                    Exit Function
                End If
            End If
            
            'Acciones de la lectura
            cmdAccion(CML_Acciones).Caption = ACC_Empaquetar
        
        Else
            'Cuando no existe el bac se busca la última caja a la que se ha traspasado desde ese BAC preguntando al usuario
            If ed.rsDameUltimaCajaDeBAC.State <> adStateClosed Then ed.rsDameUltimaCajaDeBAC.Close
            ed.DameUltimaCajaDeBAC stBAC
            
            With ed.rsDameUltimaCajaDeBAC
                If .RecordCount = 0 Then
                    If blMensaje Then Call wsMensaje(" No existe el BAC ", vbCritical)
                Else
                    If MsgBox("¿Recuperar última caja de este BAC?", vbInformation + vbYesNo, "BAC vacío!!") = vbYes Then
                        Label3.Caption = "CAJA"
                        fValidarCaja !ltcssc, True
                        .Close
                        Exit Function
                    End If
                End If
                .Close
            End With
            'Acciones de la lectura
            cmdAccion(CML_Acciones).Caption = ACC_General
        
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
            
            'Acciones de la lectura
            cmdAccion(CML_Acciones).Caption = ACC_Etiquetas
            
        Else
            If blMensaje Then Call wsMensaje(" No existe la CAJA ", vbCritical)
            
            'Acciones de la lectura
            cmdAccion(CML_Acciones).Caption = ACC_General
            
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
        lbArts = ""
        
        lbPeso = ""
        lbPeso.BackColor = vbWhite
        
        lbVolumen = ""
        lbVolumen.BackColor = vbWhite
        
        lbTipoCaja = ""
        lbNombreCaja = ""
        
        tEstadoBAC = 0
        tUbicacionBAC = 0
        
        lbNumCaja = ""
        lbNumCaja2 = ""
        lbSSCC = ""
        lbTipoCaja2 = ""
        lbNombreCaja2 = ""
        
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
        lbArts = 0
        
        lbPeso = Format(sPeso, "#0.000")
        If bPeso Then lbPeso.BackColor = ColorRojo
        
        lbVolumen = Format(sVolumen, "#0.000")
        If bVolumen Then lbVolumen.BackColor = ColorRojo
        
        lbTipoCaja = sTipoCaja
        lbNombreCaja = sNombreCaja
        
        lbNumCaja = sNumCaja
        lbNumCaja2 = sNumCaja
        
        'Datos relacionados
        If Val(sNumCaja) > 0 Then
            If ed.rsDameCajaGrupoTablillaPTL.State <> adStateClosed Then ed.rsDameCajaGrupoTablillaPTL.Close
            ed.DameCajaGrupoTablillaPTL sGrupo, sTablilla, CStr(sNumCaja)
            If ed.rsDameCajaGrupoTablillaPTL.RecordCount > 0 Then
                lbSSCC = ed.rsDameCajaGrupoTablillaPTL!ltcssc
            Else
                lbSSCC = "ERROR EN LA CAJA"
            End If
            ed.rsDameCajaGrupoTablillaPTL.Close
        Else
            lbSSCC = ""
        End If
        
        lbTipoCaja2 = sTipoCaja
        lbNombreCaja2 = sNombreCaja

        
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
    lbArts = r_Art.RecordCount

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
    lbArts = r_ArtC.RecordCount

End Sub


Private Sub ugArticulos_DblClick()
    'Call cmdAccion_Click(CML_AmpliarFoto)
    PantallaCambioUnidades
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

Private Sub ugCajas_InitializeLayout(ByVal Context As UltraGrid.Constants_Context, ByVal Layout As UltraGrid.SSLayout)
    Dim i As Integer
    
    Set ugArticulos.DataFilter = CustomDataFilter

    With ugCajas.Override
        .AllowColMoving = ssAllowColMovingNotAllowed    ' No permite mover columnas
        .FetchRows = ssFetchRowsPreloadWithParent
        .AllowUpdate = ssAllowUpdateNo
        .AllowAddNew = ssAllowAddNewNo
        .AllowDelete = ssAllowDeleteNo
    End With
    ugCajas.Appearance.BackColor = &HFFDCCE

    Layout.Override.CellClickAction = ssClickActionRowSelect            ' Al clik seleccionar toda la fila
    Layout.Override.HeaderClickAction = ssHeaderClickActionSortMulti    ' Columnas para ordenar
    
    With ugCajas.Bands(0)
        For i = 0 To .Columns.Count - 1
            .Columns(i).Hidden = True
        Next i

        i = 0
        FormatoColumnaUGrid ugCajas, 0, "tipcod", False, "Codigo", 600, i, "", "#,##0", &H80000005, False
        FormatoColumnaUGrid ugCajas, 0, "tipdes", False, "Descripcion", 2100, i, "", "", &H80000005, False
    End With

End Sub


'---------------------------------------------------------------------------------------------------------------
'Acciones
'---------------------------------------------------------------------------------------------------------------

'--- CERRAR BAC

Private Sub CerrarBAC()
    
    If tEstadoBAC = 1 Then
        'El BAC ya está cerrado
        Exit Sub
    End If
    
    'Cambiar estado de BAC de 0 a 1
    If CambiarEstadoBAC(lbBAC.Caption, 1) Then
        tEstadoBAC = 1
        'lbEstadoBAC = IIf(tEstadoBAC = 0, "ABIERTO", "CERRADO")
        'If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde Else lbEstadoBAC.BackColor = vbWhite
        lbBAC.BackColor = IIf(tEstadoBAC = 0, vbWhite, ColorVerde)
    End If

End Sub

Private Function CambiarEstadoBAC(tBac As String, tEstado As Integer) As Boolean
    'Cambio de estado de BAC
    Dim Retorno As Integer
    Dim msgSalida As String
    
    CambiarEstadoBAC = False
    
    ed.CambiaEstadoBACdePTL tBac, tEstado, Usuario.Id, Retorno, msgSalida
    
    If Retorno = 0 Then
        CambiarEstadoBAC = True
    End If
    
End Function


'--- EXTRAER BAC

Private Sub ExtraerBAC()
    If tUbicacionBAC = 0 Then
        'El BAC ya está extraido
        Exit Sub
    End If
    
    If RetirarBAC(lbBAC.Caption, tEstadoBAC, True) Then
        tUbicacionBAC = 0
        lbUbicacion = "-------------"
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
                'lbEstadoBAC = IIf(tEstado = 0, "ABIERTO", "CERRADO")
                'If lbEstadoBAC = "CERRADO" Then lbEstadoBAC.BackColor = ColorVerde Else lbEstadoBAC.BackColor = vbWhite
                lbBAC.BackColor = IIf(tEstadoBAC = 0, vbWhite, ColorVerde)
            End If
        End If
    Else
        'Call wsMensaje(" No se ha podido retirar el BAC de la estanteria de PTL. " & msgSalida, vbCritical)
    End If
    
    
End Function


'--- CREAR CAJA

Private Sub CrearCAJA()
    Dim nCaja As Long
    Dim sSSCC As String
    
    If tUbicacionBAC <> 0 And tEstadoBAC = 0 Then
        'No se puede crear caja cuando el BAC está ubicado y está abierto
        Exit Sub
    End If
    
    'Comprueba si el BAC ya tiene caja asignada
    If Val(lbNumCaja.Caption) > 0 Then
        'La caja ya ha sido creada previamente
        Exit Sub
    End If
    
    
    'Busca el siguiente número de caja
    If ed.rsDameCajasGrupoTablillaPTL.State <> adStateClosed Then ed.rsDameCajasGrupoTablillaPTL.Close
    ed.DameCajasGrupoTablillaPTL lbGrupo, lbTablilla
    
    If ed.rsDameCajasGrupoTablillaPTL.RecordCount > 0 Then
        ed.rsDameCajasGrupoTablillaPTL.MoveLast
        nCaja = Val(ed.rsDameCajasGrupoTablillaPTL!ltccaj) + 1
    Else
        'No hay ninguna caja
        nCaja = 1
    End If
    ed.rsDameCajasGrupoTablillaPTL.Close
    
    
    'Se crea la caja
    If CrearCajaNueva(CInt(lbGrupo.Caption), CInt(lbTablilla.Caption), nCaja, CInt(lbTipoCaja.Caption), sSSCC, lbBAC) Then
        'Se ha creado la caja
        lbSSCC.Caption = sSSCC
        lbNumCaja = nCaja
        lbNumCaja2 = nCaja
    Else
        Call wsMensaje(" No se ha podido crear la CAJA. ", MENSAJE_Grave)
    End If
    
End Sub

Private Function CrearCajaNueva(ByVal ilGrupo As Long, ByVal ilTablilla As Long, ByRef ilCaja As Long, ByRef ilTipoCaja As Long, ByRef slSSCC As String, slBAC As String) As Boolean
    Dim msg As String
        
    CrearCajaNueva = False
    
    ' ------- Obtencion del SSCC que le va a corresponder a la nueva Caja creada para Hipdromo -------
    slSSCC = ""
    slSSCC = ObtenerSSCC_Heterogeneo
    
    If slSSCC = "" Then
        Exit Function
    End If
        
    On Error GoTo ErrGrabar
        
Grabar:
    ' Inicio la Transacción
    ed.GestionAlmacen.BeginTrans
        
        'Inserta la caja
        ed.CrearCajaGrupoTablillaPTL ilGrupo, ilTablilla, CStr(ilCaja), ilTipoCaja, slSSCC, slBAC
         
        'Actualiza el BAC
        ed.ActualizaCajaBACPTL lbBAC, CStr(ilCaja)
        
        ' --- Inserta el LOG --------------------
        ed.InsertaLogEmpaquetado ilGrupo, ilTablilla, 2, "000", 0, 0, 0, "", 1, slSSCC, "Creación de Caja", wPuestoTrabajo.Id, Usuario.Id
        ' ---------------------------------------
    
        CrearCajaNueva = True

    ' --- Cierre de Transaccion ---
    ed.GestionAlmacen.CommitTrans
    
    
    On Error GoTo 0


    Exit Function
    
ErrGrabar:
    
    'Control de Errores
    Debug.Print "Error:"; Err, Error$
    Select Case Err
        Case Else
            Dim f1 As New frmErrorTransaccion
            msg = "No se ha podido crear la caja!. ¿Intentar de Nuevo o Cancelar?" & Chr(13) & "Error: " & Err & "/" & Error$
            f1.Cabecera = "Error de creación de caja"
            f1.Mensaje = msg
            f1.Tiempo = CTE_TiempoEsperaTransaccion  ' Segundos aproximados
            f1.Show vbModal
            
    End Select
    
    Select Case f1.bResultado
        Case True
            Set f1 = Nothing
            ed.GestionAlmacen.RollbackTrans
            Resume Grabar
            
        Case False
            Set f1 = Nothing
            ed.GestionAlmacen.RollbackTrans
            MsgBox "Operacion Cancelada", , "MENSAJE"
            Exit Function
    End Select
    
    
End Function


'--- IMPRIMIR ETIQUETA DE LA CAJA

Private Sub ImprimirETIQUETA()
    Dim vtDatosEtiquetasCajas() As Variant
    
    ReDim Preserve vtDatosEtiquetasCajas(6, 0)

    vtDatosEtiquetasCajas(0, 0) = lbGrupo
    vtDatosEtiquetasCajas(1, 0) = lbTablilla
    vtDatosEtiquetasCajas(2, 0) = lbNumCaja
    vtDatosEtiquetasCajas(3, 0) = Usuario.Id
    vtDatosEtiquetasCajas(4, 0) = Usuario.Nombre
    vtDatosEtiquetasCajas(5, 0) = lbNombreCaja
    vtDatosEtiquetasCajas(6, 0) = lbSSCC
    
    
    If ComprobarImpresora_wImpresora = False Then
        MsgBox "No Existe Impresora de Etiquetas. No se puede imprimir", vbExclamation, "Impresión de etiqueta"
        Exit Sub
    End If
  
    Call wsImprimirEtiquetasCajas(vtDatosEtiquetasCajas, False, cReport, MOD_Nombre)
    
    
End Sub


'--- IMPRIMIR RELACIÓN DE CONTENIDO DE LA CAJA

Private Sub ImprimirRELACION()
    
    If tUbicacionBAC <> 0 And tEstadoBAC = 0 Then
        'No se puede imprimir la caja cuando el BAC está ubicado y está abierto
        Exit Sub
    End If
    
    If Val(lbNumCaja) = 0 Then
        'No se puede imprimir la relación porque no hay caja creada
        Exit Sub
    End If
    
    wsImprimirContenidoCaja lbGrupo, lbTablilla, lbNumCaja, cReport
    
End Sub


'--- CAMBIAR EL TIPO DE CAJA

Private Sub CambiaTipoCaja()
    Dim stBAC As String
    Dim stSSCC As String
    
    If r_Caj!tipcod = Val(lbTipoCaja2.Caption) Then Exit Sub
    
    stBAC = ""
    stSSCC = ""
    
    If Label3.Caption = "BAC" Then
        stBAC = lbBAC
    Else
        stSSCC = lbBAC
    End If
    
    ed.CambiaTipoCajaPTL r_Caj!tipcod, stBAC, stSSCC, Usuario.Id
    
    'refresca los datos
    lbTipoCaja.Caption = r_Caj!tipcod
    lbTipoCaja2.Caption = r_Caj!tipcod
    
    lbNombreCaja.Caption = r_Caj!tipdes
    lbNombreCaja2.Caption = r_Caj!tipdes
    
    'Acciones adicionales
    If Check1(OPC_ImprimirCAJA).Value = 1 And Val(lbNumCaja) > 0 Then
        'Imprimir caja
        ImprimirETIQUETA
    End If
    
    wsMensaje "Se ha realizado el cambio de tipo de caja.", vbInformation

End Sub

'--- COMBINAR CAJAS

Private Sub CombinarCajas(SSCC1 As String, SSCC2 As String)
    'Solo se puede combinar con otra caja la última caja de la tablilla.
    Dim Retorno As Integer
    Dim msgSalida As String

    'Combinar CAJAS por SQL
    ed.CombinarCajasPTL SSCC1, SSCC2, Usuario.Id, Retorno, msgSalida
    
    If Retorno = 0 Then

        'Imprimir nueva relación de contenido
        ImprimirRELACION

        wsMensaje "Se han combinado las cajas.", vbInformation
        
        cmdAccion(CML_AplicaCombinar).Enabled = False
        'PantallaOpciones
    Else
        wsMensaje " Error al combinar Cajas" + vbNewLine + msgSalida, vbCritical
    End If

End Sub

'--- EMPAQUETAR BAC

Private Sub EmpaquetarBACaCAJA()
    Dim Retorno As Integer
    Dim msgSalida As String
    Dim tSSCC As String
    
    'Comprobaciones previas
    If r_Art.RecordCount = 0 Then Exit Sub
    'If Val(lbNumCaja) = 0 Then Exit Sub
    
    'Base del SSCC para pasar al procedimiento. No es el SSCC final, sólo sirve para obtener los primeros 10 dígitos
    tSSCC = Dame_SSCC(EmpresaTrabajo.Codigo, EmpresaTrabajo.Ean, 5000005, IncrementoSerieSSCC_Hipodromo)
    
    'Empaquetado de BAC a CAJA por SQL
    ed.TraspasaBACaCAJAdePTL lbBAC, Usuario.Id, tSSCC, Retorno, msgSalida
    
    If Retorno = 0 Then
        wsMensaje msgSalida, MENSAJE_Exclamacion
        'Refresco de los datos de la pantalla con el SSCC de destino
        Label3.Caption = "CAJA"
        fValidarCaja tSSCC, True
        
        'Acciones adicionales
        If Check1(OPC_ImprimirCAJA).Value = 1 Then
            'Imprimir caja
            ImprimirETIQUETA
        End If
        
        If Check1(OPC_RelContenido).Value = 1 Then
            'Imprimir caja
            ImprimirRELACION
        End If
        
    Else
        wsMensaje " Error al Empaquetar el BAC" + vbNewLine + msgSalida, vbCritical
    End If
    
End Sub


'--- CAMBIO DE UNIDADES

Private Function ModificaUnidades(tCantidad As Integer)
    nCantidad = nCantidad + tCantidad
    If nCantidad < 0 Then nCantidad = 0
End Function

Private Sub CambiarUnidades()
    'Solo se pueden cambiar uniades en la caja
    Dim Retorno As Integer
    Dim msgSalida As String

    'Cambio de unidades
    ed.CambiaUnidadesArtCajaPTL lbBAC, nArticulo, nCantidad, Usuario.Id, Retorno, msgSalida
    
    If Retorno = 0 Then
        wsMensaje msgSalida, MENSAJE_Exclamacion
        'Refresco de los datos de la pantalla con el SSCC de destino
        Label3.Caption = "CAJA"
        fValidarCaja lbBAC, True
        
        'Acciones adicionales
        If Check1(OPC_RelContenido).Value = 1 Then
            'Imprimir caja
            ImprimirRELACION
        End If
        
    Else
        wsMensaje " Error al cambiar las unidades del Artículo" + vbNewLine + msgSalida, vbCritical
    End If


End Sub


'------------------------------------------------------------------------------------------------------------------
'FUNCIONES

' -- Funcion para Obtener el SSCC Heterogeneo que Corresponda -----
Private Function ObtenerSSCC_Heterogeneo() As String

    Dim j As Long
    Dim NumeradorSSCC
    Dim Reintentos_Duplicidad_SSCC As Long
    Dim sSSCC As String
    Dim msg As String
    Dim sSql As String


    Screen.MousePointer = vbHourglass
    
    ObtenerSSCC_Heterogeneo = ""
    Reintentos_Duplicidad_SSCC = 0
    
Obtener_SSCC_Heterogenea:

    ' - COMIENZA LA TRANSACCION ----
    On Error GoTo ErrorObtenerSSCC_Heterogeneo
    ed.GestionAlmacen.BeginTrans
    ' ------------------------------
    
    ' --- Obtencion del Numerador SSCC Que Le Corresponda que obtener un nuevo SSCC -----
    NumeradorSSCC = Dame_Siguiente_Numerador_SSCC_Heterogeneo
    
    If NumeradorSSCC = -1 Then
        Screen.MousePointer = vbDefault
        ed.GestionAlmacen.RollbackTrans
        MsgBox "No Existe Informacion para el SSCC.", vbInformation, "SSCC"
        Exit Function
    End If
    
    If NumeradorSSCC = -2 Then
        Screen.MousePointer = vbDefault
        ed.GestionAlmacen.RollbackTrans
        MsgBox "Se ha excedido el Rango de SSCC permitido.", vbInformation, "SSCC"
        Exit Function
    End If
    ' ---------------------------------------
    
    ' ---- Actualizacion del SSCC -----------
    ed.ActualizaNumeradorSSCCHipodromo NumeradorSSCC
    ' ---------------------------------------

    '--- Obtencion del SSCC e Insercion del Historico de SSCC (LA INSERCION EN EL HISTORICO GARANTIZA SSCC UNICOS) -------------------------
    sSSCC = Dame_SSCC(EmpresaTrabajo.Codigo, EmpresaTrabajo.Ean, NumeradorSSCC, IncrementoSerieSSCC_Hipodromo)
    
    On Error GoTo ErrorDuplicidadHistorico
    ed.InsertaHistoricoSSCCHipodromo 0, sSSCC, "HETEROGENEA", 0, 0
    On Error GoTo ErrorObtenerSSCC_Heterogeneo
    ' ---------------------------------------------------------------------------------------------------------------------------------------
    
    ' --- TERMINA LA TRANSACCION -----
    ed.GestionAlmacen.CommitTrans
    On Error GoTo 0

    Screen.MousePointer = vbDefault
    
    ' --- Resultado --------
    ObtenerSSCC_Heterogeneo = sSSCC
    ' ----------------------
    
    Exit Function

' ----------- Control De Error para la insercion doble del mismo SSCC ---------
ErrorDuplicidadHistorico:

    'Control de Errores
    Screen.MousePointer = vbDefault
    Debug.Print "Error:"; Err, Error$
    
    Reintentos_Duplicidad_SSCC = Reintentos_Duplicidad_SSCC + 1
    
    If Reintentos_Duplicidad_SSCC < 2000 Then
        ed.GestionAlmacen.RollbackTrans
        Resume Obtener_SSCC_Heterogenea
    Else
        ed.GestionAlmacen.RollbackTrans
        MsgBox "Imposibilidad de Impresion de Etiquetas." & _
            Chr(10) & Chr(13) & _
            "Compruebe las etiquetas ya impresas, e intentelo mas tarde con las que faltan por imprimir" & _
        Chr(10) & Chr(13), vbInformation
        On Error GoTo 0
        Exit Function
    End If
' ------------------------------------------------------------------------------

ErrorObtenerSSCC_Heterogeneo:
    Screen.MousePointer = vbDefault
    'Control de Errores
    Debug.Print "Error:"; Err, Error$
    
    Select Case Err
        Case Else
            Dim f As New frmErrorTransaccion
            msg = "Impresion de Etiqueta Erronea - No Realizada. ¿Intentar de Nuevo O Cancelar?" & _
                    Chr(13) & Chr(13) & "Error: " & Err & "/ " & Error$
            f.Cabecera = "Error de Impresion"
            f.Mensaje = msg
            f.Tiempo = CTE_TiempoEsperaTransaccion  ' Segundos aproximados
            f.Show vbModal
    End Select
    
    Select Case f.bResultado
        Case True
            Set f = Nothing
            ed.GestionAlmacen.RollbackTrans
            Resume Obtener_SSCC_Heterogenea
            
        Case False
            Set f = Nothing
            ed.GestionAlmacen.RollbackTrans
            MsgBox "Operacion Cancelada", , "MENSAJE"
            Exit Function
    End Select

End Function

'-------------------------------------------------------------------
' -- Funcion para obtener el Siguiente numerador unico de bultos ---
' -- Valores de Retorno:
' --     n --> Numerador SSCC siguiente
' --    -1 --> No Existe Registro de control del numerador SSCC
' --    -2 --> Se ha alcanzado el final del Rango Permitido
' -------------------------------------------------------------------
Private Function Dame_Siguiente_Numerador_SSCC_Heterogeneo() As Long
    Dim Numerador_Actual

    If ed.rsDameNumeradorSSCCHipodromo.State <> adStateClosed Then ed.rsDameNumeradorSSCCHipodromo.Close
    
    ed.DameNumeradorSSCCHipodromo
    
    With ed.rsDameNumeradorSSCCHipodromo
        If .RecordCount > 0 Then
            Numerador_Actual = !numnum
            
            If Numerador_Actual = 0 Then
                Dame_Siguiente_Numerador_SSCC_Heterogeneo = !numdes
            Else
                If Numerador_Actual = !numhas Then
                    ' --- Se ha alcanzado el final del Rango Permitido ---
                    Dame_Siguiente_Numerador_SSCC_Heterogeneo = -2
                Else
                    Dame_Siguiente_Numerador_SSCC_Heterogeneo = Numerador_Actual + 1
                End If
            End If
        Else
            ' --- No Existe Registro y por lo tanto hay un error
            Dame_Siguiente_Numerador_SSCC_Heterogeneo = -1
        End If
        ed.rsDameNumeradorSSCCHipodromo.Close
    End With
    
End Function

' --- Funcion de impresion de Etiqueta de bulto interno de PTL para pruebas
' ---
' --- Para utilizar hacer una llamada desde cualquier punto del programa.
' ---
' --- Ejemplo: ImprimirETIQUETA_Pruebas 26181, 4, 1, 1001, "moreno", "CAJA C", "384222593106995260"
' ---
' ---
Private Sub ImprimirETIQUETA_Forzar_Pruebas(Grupo As Variant, Tablilla As Variant, Caja As Variant, _
Usuario As Variant, NombreUsuario As Variant, NombreCaja As Variant, SSCC As Variant)
    Dim vtDatosEtiquetasCajas() As Variant
    
    ReDim Preserve vtDatosEtiquetasCajas(6, 0)

    vtDatosEtiquetasCajas(0, 0) = Grupo
    vtDatosEtiquetasCajas(1, 0) = Tablilla
    vtDatosEtiquetasCajas(2, 0) = Caja
    vtDatosEtiquetasCajas(3, 0) = Usuario
    vtDatosEtiquetasCajas(4, 0) = NombreUsuario
    vtDatosEtiquetasCajas(5, 0) = NombreCaja
    vtDatosEtiquetasCajas(6, 0) = SSCC
    
    
    If ComprobarImpresora_wImpresora = False Then
        MsgBox "No Existe Impresora de Etiquetas. No se puede imprimir", vbExclamation, "Impresión de etiqueta"
        Exit Sub
    End If
  
    Call wsImprimirEtiquetasCajas(vtDatosEtiquetasCajas, False, cReport, MOD_Nombre)
    
    
End Sub


