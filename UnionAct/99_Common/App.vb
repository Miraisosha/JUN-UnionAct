Imports System.Runtime.InteropServices

Public Class App
    Private Shared AppPath As String
    Private Shared AppDir As String
    Private Shared IniFilePath As String

    Public Shared Property MainWindowWidth As Integer = 0
    Public Shared Property MainWindowHeight As Integer = 0

    ' API宣言
    ' ------------------------------------------------------
    ' ini ファイル書き込み
    ' ------------------------------------------------------
    <DllImport("Kernel32.dll")>
    Private Shared Function WritePrivateProfileString(
                    ByVal lpAppName As String,                      'セクション名
                    ByVal lpKeyName As String,                      'キー名
                    ByVal lpString As String,
                    ByVal lpFileName As String) As Integer
    End Function

    ' ------------------------------------------------------
    ' ini ファイル読み込み
    ' ------------------------------------------------------
    <DllImport("Kernel32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function GetPrivateProfileString(
                    ByVal lpAppName As String,                                   ' セクション名
                    ByVal lpKeyName As String,                                   ' キー名
                    ByVal lpDefault As String,                                   ' キーが見つからなかった場合に取得するデフォルト値
                    ByVal lpReturnedString As System.Text.StringBuilder,         ' 取得した文字列が入るバッファ
                    ByVal nSize As Integer,                                      ' 取得した文字列のバッファサイズ
                    ByVal lpFileName As String) As Integer
    End Function

    '-------------------------------------------------------
    'iniファイル(整数値読み込み)
    '-------------------------------------------------------
    <DllImport("Kernel32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function GetPrivateProfileInt(
                    ByVal lpAppName As String,                  ' セクション名
                    ByVal lpKeyName As String,                  ' キー名
                    ByVal nDefault As Integer,                  ' キーが見つからなかった場合に取得するデフォルト値
                    ByVal lpFileName As String) As Integer      ' iniファイル名
    End Function

    Shared Sub Start()
        AppPath = System.Reflection.Assembly.GetExecutingAssembly().Location
        AppDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
        IniFilePath = System.IO.Path.ChangeExtension(AppPath, ".ini")

        MainWindowWidth = GetPrivateProfileInt("MainWindow", "Width", 0, IniFilePath)
        MainWindowHeight = GetPrivateProfileInt("MainWindow", "Height", 0, IniFilePath)
    End Sub

    Shared Sub Shutdown()
        WritePrivateProfileString("MainWindow", "Width", $"{MainWindowWidth}", IniFilePath)
        WritePrivateProfileString("MainWindow", "Height", $"{MainWindowHeight}", IniFilePath)
    End Sub
End Class
