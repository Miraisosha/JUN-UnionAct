Imports System
Imports System.Drawing.Printing

Namespace Framework
    Public Class tagPrintStatus
        ' Fields
        Public nPageFrom As Integer = -1
        Public nPageTo As Integer = -1
        Public nPrintCount As Integer = -1
        Public pSourceKind As PaperSourceKind = PaperSourceKind.Custom
    End Class
End Namespace
