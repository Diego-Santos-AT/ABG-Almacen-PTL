VERSION 5.00
Begin VB.Form frmVerFoto 
   BorderStyle     =   1  'Fixed Single
   Caption         =   "Código: "
   ClientHeight    =   7320
   ClientLeft      =   45
   ClientTop       =   405
   ClientWidth     =   9555
   Icon            =   "frmVerFoto.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   Moveable        =   0   'False
   ScaleHeight     =   7320
   ScaleWidth      =   9555
   StartUpPosition =   1  'CenterOwner
   WindowState     =   2  'Maximized
   Begin VB.Image Imagen 
      Appearance      =   0  'Flat
      BorderStyle     =   1  'Fixed Single
      Height          =   7300
      Left            =   0
      Stretch         =   -1  'True
      Top             =   0
      Width           =   9545
   End
End
Attribute VB_Name = "frmVerFoto"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'******************************************************************************
' frmVerFoto
'
' Form para mostrar la foto de un artículo
' Este form se muestra como cuadro de dialogo modal.
'
' Creado: 7/02/00
'******************************************************************************
Option Explicit

Dim tCodigo As Long
Dim tNombre As String
Dim tRuta As String

'PROPIEDADES
Property Let Codigo(ByVal newCodigo As Long)
    tCodigo = newCodigo
End Property

Property Let Nombre(ByVal newNombre As String)
    tNombre = newNombre
End Property

Property Let Ruta(ByVal newRuta As String)
    tRuta = newRuta
End Property
'******************************************************************************

Private Sub Form_GotFocus()
    'Me.Height = 7695
    'Me.Width = 9645
    Imagen.Height = Me.Height - 395
    Imagen.Width = Me.Width - 100
End Sub

Private Sub Form_Load()
Dim Nombre
    
    Me.Caption = "Código:  " & Str(tCodigo) & " - " & tNombre
    Nombre = tRuta & Trim(Str(tCodigo)) & ".jpg"
    If Dir(Nombre) <> "" Then
        Imagen.Picture = LoadPicture(Nombre)
    Else
        Imagen.Picture = LoadPicture
    End If
End Sub

Private Sub Form_KeyDown(KeyCode As Integer, Shift As Integer)
    Unload Me
End Sub

Private Sub Imagen_Click()
    Unload Me
End Sub
