Imports System
Imports System.Data
Imports System.Windows.Forms
Imports Microsoft.Office.Interop

Namespace Business.Common
    Public Interface IExcelFileCommand

        ''' <summary>Excelファイルチェック処理（時間内・争議講義）</summary>
        Function CheckExcelInTimeStrike(ByVal file As String) As Boolean

        ''' <summary>Excelファイルチェック処理（一時金）</summary>
        Function CheckExcelBounus(ByVal file As String) As Boolean

        ''' <summary>Excelファイル読み込み処理（時間内・争議行為）</summary>
        Function ReadExcelInTimeStrike(ByVal file As String) As DataTable

        ''' <summary>Excelファイル読み込み処理（一時金）</summary>
        Function ReadExcelBounus(ByVal file As String) As DataTable

        ''' <summary>セル値取得処理</summary>
        Function GetCell(ByVal sheet As Excel.Worksheet, ByVal row As Integer, ByVal col As Integer) As String

        ''' <summary>最終行取得処理</summary>
        Function GetLastRow(ByVal xlsCell As Excel.Range, ByVal targetRow As Integer, ByVal targetCol As Integer) As Integer

        ''' <summary>最終列取得処理</summary>
        Function GetLastCol(ByVal xlsCell As Excel.Range, ByVal targetRow As Integer, ByVal targetCol As Integer) As Integer

        ''' <summary>ファイル内容チェック処理（時間内・争議行為）</summary>
        Function FileCheckInTimeStrike(ByVal xlsCell As Excel.Range) As Boolean

        ''' <summary>ファイル内容チェック処理（一時金）</summary>
        Function FileCheckBounus(ByVal xlsCell As Excel.Range) As Boolean

        ''' <summary>セル値チェック処理</summary>
        Function CheckCellValue(ByVal xlsCell As Excel.Range, ByVal row As Integer, ByVal col As Integer, ByVal value As String) As Boolean

        ''' <summary>エラー箇所バックカラー設定処理</summary>
        Sub errCellBackColorSet(ByVal xlsCell As Excel.Range, ByVal row As Integer, ByVal col As Integer)

        ''' <summary>ファイルを開くダイアログボックス表示処理</summary>
        Function ShowOpenExcelFileDialog() As OpenFileDialog

        ''' <summary>Excelオブジェクト開放処理</summary>
        Sub XlsObjectFreedom(ByRef objCom As Object)

        ''' <summary>セル値取得処理</summary>
        Function GetCellValue(ByVal xlsCell As Excel.Range, ByVal row As Integer, ByVal col As Integer) As String

        ''' <summary>社員番号存在チェック</summary>
        Function FindExistMember(ByVal EmployeeNumber As String, ByVal TargetYM As String) As Boolean

    End Interface
End Namespace
