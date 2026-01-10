'要求書,争議行為

Imports Microsoft.Office.Interop.Excel
Imports System
Imports System.Reflection


Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common
Imports UnionAct.Business.Common
Imports UnionAct.Framework.Command

Public Class DocTemplate005
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
            Dim lfSplitStrArray As String() = DocTemplateBase.GetLfSplitStrArray(noticeSentence)
            MyBase.SetArrayDataAppointCell(MyBase.GetNoticeSentenceCell, lfSplitStrArray, 1)
            MyBase.SetDataAppointCell(MyBase.GetDescriptionCell, description)
        Catch ex As Exception
            MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End Try
    End Sub

End Class
