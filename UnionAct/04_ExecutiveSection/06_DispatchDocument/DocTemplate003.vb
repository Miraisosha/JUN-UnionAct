'専門部、専門委員の追加について（Temlate003）

Imports Microsoft.Office.Interop.Excel
Imports System
Imports System.Reflection

Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common
Imports UnionAct.Business.Common
Imports UnionAct.Framework.Command

Public Class DocTemplate003
    Inherits DocTemplateBase

    Public Overrides Sub ApplyOfficers()
        
    End Sub

    Public Overrides Sub InitNewDocument()

        Try
            'Me.SetFixedSentence()テンプレート側に記載する
            MyBase.InitNewDocument()
            Call Me.ApplyAddDeleteMembers()

        Catch ex As Exception
            Call MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, _
                                 "エラー", _
                                 MessageBoxButtons.OK, _
                                 MessageBoxIcon.Hand)
        End Try

    End Sub

    Public Overloads Sub ApplyAddDeleteMembers()

        Try
            Dim nExcelRowNum As Integer = PublicCommand.StrnumToInt(MyBase.GetMemberTblCell)
            MyBase.ApplyAddDeleteMembers(nExcelRowNum)
            'MyBase.ApplySubject(MyBase._docInf.strSubject)
            Dim str As String = Nothing
            str = MyBase.GetClosingRemarksCell.Replace("{R}", nExcelRowNum.ToString)
            MyBase.SetTextHAlign(String.Format("{0}:{0}", str), Constants.xlRight)
            MyBase.SetDataAppointCell(str, MyBase.GetClosingRemarks)
            MyBase.SelectRange("A1:A1")

        Catch ex As Exception
            Call MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, _
                                 "エラー", _
                                 MessageBoxButtons.OK, _
                                 MessageBoxIcon.Hand)
        End Try
    End Sub

    Public Overrides Sub SetFixedSentence()

        Try
            Dim noticeSentence As String = MyBase.GetNoticeSentence
            Dim description As String = MyBase.GetDescription
            MyBase.SetDataAppointCell(MyBase.GetNoticeSentenceCell, noticeSentence)
            MyBase.SetDataAppointCell(MyBase.GetDescriptionCell, description)

        Catch ex As Exception
            Call MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, _
                                 "エラー", _
                                 MessageBoxButtons.OK, _
                                 MessageBoxIcon.Hand)
        End Try

    End Sub
End Class
