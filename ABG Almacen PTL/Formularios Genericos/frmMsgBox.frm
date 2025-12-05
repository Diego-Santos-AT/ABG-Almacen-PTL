VERSION 5.00
Begin VB.Form frmMsgBox 
   AutoRedraw      =   -1  'True
   ClientHeight    =   3900
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   3345
   BeginProperty Font 
      Name            =   "Arial"
      Size            =   14.25
      Charset         =   0
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   3900
   ScaleWidth      =   3345
   StartUpPosition =   2  'CenterScreen
   Begin VB.CommandButton Command1 
      Caption         =   "Cancelar"
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   11.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1095
      Index           =   2
      Left            =   2280
      Picture         =   "frmMsgBox.frx":0000
      Style           =   1  'Graphical
      TabIndex        =   3
      Top             =   2520
      Width           =   975
   End
   Begin VB.CommandButton Command1 
      Caption         =   "No"
      Height          =   1095
      Index           =   1
      Left            =   1200
      Picture         =   "frmMsgBox.frx":08CA
      Style           =   1  'Graphical
      TabIndex        =   1
      Top             =   2520
      Width           =   975
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Sí"
      Height          =   1095
      Index           =   0
      Left            =   120
      Picture         =   "frmMsgBox.frx":0D0C
      Style           =   1  'Graphical
      TabIndex        =   0
      Top             =   2520
      Width           =   975
   End
   Begin VB.Label LbMensaje 
      BeginProperty Font 
         Name            =   "Arial"
         Size            =   11.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H000000FF&
      Height          =   1995
      Left            =   120
      TabIndex        =   2
      Top             =   240
      Width           =   3075
   End
End
Attribute VB_Name = "frmMsgBox"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'FORMULARIO GENERICO PARA MOSTRAR UN FORMULARIO COMO UNA CAJA DE MENSAJE

'vbOKOnly 0 Sólo el botón Aceptar (predeterminado)
'vbOKCancel 1 Los botones Aceptar y Cancelar
'vbAbortRetryIgnore 2 Los botones Anular, Reintentar e Ignorar
'vbYesNoCancel 3 Los botones Sí, No y Cancelar.
'VbYesNo 4 Los botones Sí y No
'vbRetryCancel 5 Los botones Reintentar y Cancelar
'vbCritical 16 Mensaje crítico
'vbQuestion 32 Consulta de advertencia
'vbExclamation 48 Mensaje de advertencia
'vbInformation 64 Mensaje de información
'vbDefaultButton1 0 El primer botón es el predeterminado (predeterminado)
'vbDefaultButton2 256 El segundo botón es el predeterminado
'vbDefaultButton3 512 El tercer botón es el predeterminado
'vbDefaultButton4 768 El cuarto botón es el predeterminado
'vbApplicationModal 0 Cuadro de mensajes de aplicación modal (valor predeterminado)
'vbSystemModal 4096 Cuadro de mensajes modal del sistema
'vbMsgBoxHelpButton 16384 Agrega el botón Ayuda al cuadro de mensaje
'VbMsgBoxSetForeground 65536 Especifica la ventana del cuadro de mensaje como la ventana de primer plano
'vbMsgBoxRight 524288 El texto se alinea a la derecha
'vbMsgBoxRtlReading


Option Explicit
' Definición de propiedades
Dim tCabecera As String '--- Mensaje de Cabecera del Formulario
Dim tMensaje As String  '--- Mensaje para el Formulario
Dim lResultado As Long '--- Valor de Retorno: False ---> Cancelar; True -->Aceptar
Dim lBotones As Long    ' --- Valor que contiene la informacion de los botones a mostrar
Dim TipoSiAceptar

Private Const CML_Si = 0
Private Const CML_No = 1
Private Const CML_Cancel = 2

Property Let Cabecera(ByVal sCadena As String)
    tCabecera = sCadena
End Property

Property Let Mensaje(ByVal sTexto As String)
    tMensaje = sTexto
End Property
Property Let Botones(ByVal vBotones As Variant)
    lBotones = vBotones
End Property

Property Get Resultado() As Long
    Resultado = lResultado
End Property

Private Sub Form_Load()

    Me.Caption = tCabecera
    Me.LbMensaje = tMensaje
    
    Configurar_Botones
End Sub

Private Sub Command1_Click(Index As Integer)

    Select Case Index
    
        Case 0  ' -- Boton SI / Aceptar
        
            If TipoSiAceptar = 0 Then
                lResultado = vbYes  ' --- Boton YES
            Else
                lResultado = vbOK   ' --- Boton ACEPTAR
            End If
            
        Case 1  ' -- Boton NO
        
            lResultado = vbNo
            
        Case 2  ' -- Boton Cancelar
        
            lResultado = vbCancel
            
    End Select
    
    Unload Me
    
End Sub

' -- Funcion para Configurar la pantalla según los botones
Private Sub Configurar_Botones()
Dim msg

    Select Case lBotones
    
        Case vbOKOnly
            Mostrar_Botones 1
            
        Case vbYesNoCancel
            Mostrar_Botones 0
            
        Case vbYesNo
            Mostrar_Botones 2
            
        Case vbCritical
            Mostrar_Botones 1
            
        Case vbQuestion
            Mostrar_Botones 1
            
        Case vbExclamation
            Mostrar_Botones 1
        
        Case vbInformation
            Mostrar_Botones 1
        
        Case (vbInformation + vbOKOnly)
            Mostrar_Botones 1
            
        Case (vbYesNoCancel + vbQuestion)
            Mostrar_Botones 0
            
        Case (vbYesNo + vbQuestion)
            Mostrar_Botones 2
            
        Case Else
                
            msg = MsgBox(tMensaje, lBotones, tCabecera)
            lResultado = msg
            Unload Me
        
    End Select
End Sub

Private Sub Mostrar_Botones(Tipo As Long)

    Select Case Tipo
    
        Case 0  ' -- Todos los botones
        
            TipoSiAceptar = 0 ' -- El Boton Si /Aceptar  = SI
            Command1(CML_Si).Visible = True
            Command1(CML_No).Visible = True
            Command1(CML_Cancel).Visible = True
            
            lResultado = vbCancel   ' -- Valor Por Defecto de Salida
            
        Case 1  ' -- Solo boton Aceptar
        
            TipoSiAceptar = 1 ' -- El Boton Si /Aceptar  = ACEPTAR
            
            Command1(CML_Si).Visible = True
            Command1(CML_Si).Caption = "Aceptar"
            Command1(CML_Si).Left = 1200
            Command1(CML_No).Visible = False
            Command1(CML_Cancel).Visible = False
            
            lResultado = vbOK
            
        Case 2  ' -- Solo boton Si o no
            
            TipoSiAceptar = 0 ' -- El Boton Si /Aceptar  = SI
            
            Command1(CML_Si).Visible = True
            Command1(CML_Si).Left = 360
            Command1(CML_No).Visible = True
            Command1(CML_No).Left = 2040
            Command1(CML_Cancel).Visible = False
            lResultado = vbNo
            
    End Select
    
End Sub

