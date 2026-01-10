Imports System
Imports System.Data
Imports System.Windows.Forms

Namespace Business.Common
    Public Interface ICSVFileCommand
        ' Methods
        Function ReadCSVFile(ByVal ofd As OpenFileDialog, ByVal strTableName As String) As DataTable
        Function ShowOpenCSVFileDialog(ByVal strDefaultFileName As String) As OpenFileDialog
        Function ShowSaveCSVFileDialog(ByVal strDefaultFileName As String) As SaveFileDialog
        Sub WriteCSVFile(ByVal sfd As SaveFileDialog, ByVal dTable As DataTable, ByVal addDoubleQuotation As Boolean)
    End Interface
End Namespace
