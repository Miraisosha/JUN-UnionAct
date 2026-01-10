Imports Microsoft.Office.Interop.Excel
Imports System
Imports System.Reflection

Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common
Imports UnionAct.Business.Common
Imports UnionAct.Framework.Command

Public Class DocTemplate013

    Inherits DocTemplateBase

    Public Overrides Sub ApplyOfficers()

    End Sub

    Public Overrides Sub InitNewDocument()
        Try
            Me.SetFixedSentence()
            MyBase.InitNewDocument()
            Call Me.ApplyChangeMembers(16)

        Catch ex As Exception
            MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Hand)
        End Try
    End Sub


    Public Overrides Sub ApplyChangeMembers(ByRef nExcelRowNum As Integer)
        Try
            MyBase.ApplyChangeMembers(nExcelRowNum)
            'MyBase.ApplySubject(MyBase._docInf.strSubject)
            Dim str As String = Nothing
            Str = MyBase.GetClosingRemarksCell.Replace("{R}", nExcelRowNum.ToString)
            MyBase.SetTextHAlign(String.Format("{0}:{0}", Str), Constants.xlRight)
            MyBase.SetDataAppointCell(Str, MyBase.GetClosingRemarks)
            MyBase.SelectRange("A1:A1")

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
