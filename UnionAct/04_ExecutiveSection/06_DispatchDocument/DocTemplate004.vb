'要求書、勤務調整について（Template004,011）→
Imports Microsoft.Office.Interop.Excel
Imports System
Imports System.Reflection

Public Class DocTemplate004
    Inherits DocTemplateBase

    Public Overrides Sub InitNewDocument()
        Try
            Me.SetFixedSentence()
            MyBase.InitNewDocument()
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End Try
    End Sub

    Public Overrides Sub SetFixedSentence()
        Try
            Dim noticeSentence As String = MyBase.GetNoticeSentence
            Dim description As String = MyBase.GetDescription
            MyBase.SetDataAppointCell(MyBase.GetNoticeSentenceCell, noticeSentence)
            MyBase.SetDataAppointCell(MyBase.GetDescriptionCell, description)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End Try
    End Sub
End Class
