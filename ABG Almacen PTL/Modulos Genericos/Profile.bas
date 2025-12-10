Attribute VB_Name = "Profile"
'*****************************************************************************************
' Profile.bas
'
' Fecha inicio: 03/10/2001
'
' Módulo genérico para las llamadas al API
' usando xxxProfileString
'=========================================================================================
Option Explicit


' ---- Ruta y clave de registro para Archivos de programa ---------------------
Public Const ProgramasDir = "SOFTWARE\Microsoft\Windows\CurrentVersion"
Public Const ClaveRegistroProgramas = "ProgramFilesDir"
'------------------------------------------------------------------------------

' ---- Ruta y clave de registro para Ruta DSN ---------------------------------
Public Const DSNDir = "SOFTWARE\ODBC\ODBC.INI\ODBC File DSN"
Public Const ClaveRegistroDSN = "DefaultDSNDir"
'------------------------------------------------------------------------------


Private Declare Function RegOpenKeyEx Lib "advapi32" Alias "RegOpenKeyExA" (ByVal hKey As Long, ByVal lpSubKey As String, ByVal ulOptions As Long, ByVal samDesired As Long, ByRef phkResult As Long) As Long
Private Declare Function RegQueryValueEx Lib "advapi32" Alias "RegQueryValueExA" (ByVal hKey As Long, ByVal lpValueName As String, ByVal lpReserved As Long, ByRef lpType As Long, ByVal lpData As String, ByRef lpcbData As Long) As Long
Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Long) As Long

' Tipos ROOT de clave del Registro...
Public Const HKEY_CURRENT_USER = &H80000001
Public Const HKEY_LOCAL_MACHINE = &H80000002
Public Const HKEY_USERS = &H80000003


' Opciones de seguridad de clave del Registro...
Private Const READ_CONTROL = &H20000
Private Const KEY_QUERY_VALUE = &H1
Private Const KEY_SET_VALUE = &H2
Private Const KEY_CREATE_SUB_KEY = &H4
Private Const KEY_ENUMERATE_SUB_KEYS = &H8
Private Const KEY_NOTIFY = &H10
Private Const KEY_CREATE_LINK = &H20
Private Const KEY_ALL_ACCESS = KEY_QUERY_VALUE + KEY_SET_VALUE + _
                       KEY_CREATE_SUB_KEY + KEY_ENUMERATE_SUB_KEYS + _
                       KEY_NOTIFY + KEY_CREATE_LINK + READ_CONTROL
                       


Private Const ERROR_SUCCESS = 0
Private Const REG_SZ = 1                         ' Cadena Unicode terminada en valor nulo
Private Const REG_DWORD = 4                      ' Número de 32 bits


#If Win32 Then
    'Declaraciones para 32 bits
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" _
        (ByVal lpApplicationName As String, ByVal lpKeyName As Any, _
         ByVal lpDefault As String, ByVal lpReturnedString As String, _
         ByVal nSize As Long, ByVal lpFileName As String) As Long
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" _
        (ByVal lpApplicationName As String, ByVal lpKeyName As Any, _
         ByVal lpString As Any, ByVal lpFileName As String) As Long
'    Private Declare Function WritePrivateProfileSection Lib "kernel32" Alias "WritePrivateProfileSectionA" _
'        (ByVal lpApplicationName As String, ByVal lpString As Any, ByVal lpFileName As String) As Long
        
    ' Leer una sección completa
    Private Declare Function GetPrivateProfileSection Lib "kernel32" Alias "GetPrivateProfileSectionA" _
        (ByVal lpAppName As String, ByVal lpReturnedString As String, _
        ByVal nSize As Long, ByVal lpFileName As String) As Long
        
#Else
    'Declaraciones para 16 bits
    Private Declare Function GetPrivateProfileString Lib "Kernel" _
        (ByVal lpApplicationName As String, ByVal lpKeyName As Any, _
         ByVal lpDefault As String, ByVal lpReturnedString As String, _
         ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Private Declare Function WritePrivateProfileString Lib "Kernel" _
        (ByVal lpApplicationName As String, ByVal lpKeyName As Any, _
         ByVal lpString As Any, ByVal lplFileName As String) As Integer
         
''    Private Declare Function WritePrivateProfileSection Lib "Kernel" _
''        (ByVal lpApplicationName As String, ByVal lpString As Any, _
''        ByVal lpFileName As String) As Integer

#End If

'
Public Function LeerIni(lpFileName As String, lpAppName As String, lpKeyName As String, Optional vDefault) As String
    'Los parámetros son:
    'lpFileName:    La Aplicación (fichero INI)
    'lpAppName:     La sección que suele estar entrre corchetes
    'lpKeyName:     Clave
    'vDefault:      Valor opcional que devolverá
    '               si no se encuentra la clave.
    '
    Dim lpString As String
    Dim LTmp As Long
    Dim sRetVal As String
    
    'Si no se especifica el valor por defecto,
    'asignar incialmente una cadena vacía
    If IsMissing(vDefault) Then
        lpString = ""
    Else
        lpString = vDefault
    End If
    
    sRetVal = String$(255, 0)
    
    LTmp = GetPrivateProfileString(lpAppName, lpKeyName, lpString, sRetVal, Len(sRetVal), lpFileName)
    If LTmp = 0 Then
        LeerIni = lpString
    Else
        LeerIni = Left(sRetVal, LTmp)
    End If
End Function
'
Sub GuardarIni(lpFileName As String, lpAppName As String, lpKeyName As String, lpString As String)
    'Guarda los datos de configuración
    'Los parámetros son los mismos que en LeerIni
    'Siendo lpString el valor a guardar
    '
    Dim LTmp As Long

    LTmp = WritePrivateProfileString(lpAppName, lpKeyName, lpString, lpFileName)
End Sub

'Sub GuardarSeccionINI(lpFileName As String, lpAppName As String, lpString As String)
'
'    Dim LTmp As Long
'
'    LTmp = WritePrivateProfileSection(lpAppName, lpString, lpFileName)
'
'End Sub
Function LeerSeccionINI(ByVal lpFileName As String, _
                              ByVal lpAppName As String) As Variant
    '
    ' Lee una sección entera de un fichero INI                          (27/Feb/99)
    '
    ' Usando Collection en lugar de cParrafos y cContenido              (06/Mar/99)
    '
    ' Esta función devolverá una colección con cada una de las claves y valores
    ' que haya en esa sección.
    ' Parámetros de entrada:
    '   lpFileName  Nombre del fichero INI
    '   lpAppName   Nombre de la sección a leer
    ' Devuelve:
    '   Una colección con el Valor y el contenido
    '   Para leer los datos:
    '       For i = 1 To tContenidos Step 2
    '           sClave = tContenidos(i)
    '           sValor = tContenidos(i+1)
    '       Next
    '
    Dim tContenidos As Collection
    Dim nSize As Long
    Dim i As Long
    Dim j As Long
    Dim sTmp As String
    Dim sClave As String
    Dim sValor As String
    Dim sBuffer As String
    
    ' El tamaño máximo para Windows 95
    sBuffer = String$(32767, Chr$(0))
    
    nSize = GetPrivateProfileSection(lpAppName, sBuffer, Len(sBuffer), lpFileName)
        
    If nSize Then
        Set tContenidos = New Collection
        
        ' Cortar la cadena al número de caracteres devueltos
        sBuffer = Left$(sBuffer, nSize)
        ' Quitar los vbNullChar extras del final
        i = InStr(sBuffer, vbNullChar & vbNullChar)
        If i Then
            sBuffer = Left$(sBuffer, i - 1)
        End If
        
        ' Cada una de las entradas estará separada por un Chr$(0)
        Do
            i = InStr(sBuffer, Chr$(0))
            If i Then
                sTmp = LTrim$(Left$(sBuffer, i - 1))
                If Len(sTmp) Then
                    ' Comprobar si tiene el signo igual
                    j = InStr(sTmp, "=")
                    If j Then
                        sClave = Left$(sTmp, j - 1)
                        sValor = LTrim$(Mid$(sTmp, j + 1))
                        ' Asignar la clave y el valor
                        tContenidos.Add sClave
                        tContenidos.Add sValor
                    End If
                End If
                sBuffer = Mid$(sBuffer, i + 1)
            End If
        Loop While i
        ' Por si aún queda algo...
        If Len(sBuffer) Then
            j = InStr(sBuffer, "=")
            If j Then
                sClave = Left$(sBuffer, j - 1)
                sValor = LTrim$(Mid$(sBuffer, j + 1))
                tContenidos.Add sClave
                tContenidos.Add sValor
            End If
        End If
    End If
    Set LeerSeccionINI = tContenidos
    
End Function



'/*************** Lectura de Registro de Windows ********************/




Public Function LeerClave(KeyRoot As Long, KeyName As String, SubKeyRef As String, ByRef KeyVal As String) As Boolean
    Dim i As Long                                           ' Contador de bucle
    Dim rc As Long                                          ' Código de retorno
    Dim hKey As Long                                        ' Controlador de una clave de Registro abierta
    Dim hDepth As Long                                      '
    Dim KeyValType As Long                                  ' Tipo de datos de una clave de Registro
    Dim tmpVal As String                                    ' Almacenamiento temporal para un valor de clave de Registro
    Dim KeyValSize As Long                                  ' Tamaño de variable de clave de Registro
    '------------------------------------------------------------
    ' Abrir clave de registro bajo KeyRoot {HKEY_LOCAL_MACHINE...}
    '------------------------------------------------------------

    rc = RegOpenKeyEx(KeyRoot, KeyName, 0, KEY_ALL_ACCESS, hKey) ' Abrir clave de Registro

    If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError          ' Error de controlador...

    tmpVal = String$(1024, 0)                             ' Asignar espacio de variable
    KeyValSize = 1024                                       ' Marcar tamaño de variable

    '------------------------------------------------------------
    ' Obtener valor de clave de Registro...
    '------------------------------------------------------------
    rc = RegQueryValueEx(hKey, SubKeyRef, 0, _
                         KeyValType, tmpVal, KeyValSize)    ' Obtener o crear valor de clave


    If (rc <> ERROR_SUCCESS) Then GoTo GetKeyError          ' Controlar errores

    If (Asc(Mid(tmpVal, KeyValSize, 1)) = 0) Then           ' Win95 agregar cadena terminada en valor nulo...
        tmpVal = Left(tmpVal, KeyValSize - 1)               ' Encontrado valor nulo, se va a quitar de la cadena
    Else                                                    ' En WinNT las cadenas no terminan en valor nulo...
        tmpVal = Left(tmpVal, KeyValSize)                   ' No se ha encontrado valor nulo, sólo se va a extraer la cadena
    End If
    '------------------------------------------------------------
    ' Determinar tipo de valor de clave para conversión...
    '------------------------------------------------------------
    Select Case KeyValType                                  ' Buscar tipos de datos...
    Case REG_SZ                                             ' Tipo de datos String de clave de Registro
        KeyVal = tmpVal                                     ' Copiar valor de cadena
    Case REG_DWORD                                          ' Tipo de datos Double Word de clave del Registro
        For i = Len(tmpVal) To 1 Step -1                    ' Convertir cada bit
            KeyVal = KeyVal + Hex(Asc(Mid(tmpVal, i, 1)))   ' Generar valor carácter a carácter
        Next
        KeyVal = Format$("&h" + KeyVal)                     ' Convertir Double Word a cadena
    End Select

    LeerClave = True                                      ' Se ha devuelto correctamente
    rc = RegCloseKey(hKey)                                  ' Cerrar clave de Registro
    Exit Function                                           ' Salir

GetKeyError:      ' Borrar después de que se produzca un error...
    KeyVal = ""                                             ' Establecer valor a cadena vacía
    LeerClave = False                                     ' Fallo de retorno
    rc = RegCloseKey(hKey)                                  ' Cerrar clave de Registro
End Function




