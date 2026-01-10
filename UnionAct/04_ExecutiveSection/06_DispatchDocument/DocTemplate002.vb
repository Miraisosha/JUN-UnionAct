''役員選任と各委員などの異動に関して(Template002)
Imports Microsoft.Office.Interop.Excel
Imports System
Imports System.Reflection


Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common
Imports UnionAct.Business.Common
Imports UnionAct.Framework.Command

Public Class DocTemplate002
    Inherits DocTemplateBase

    Public Overloads Sub ApplyMemberOfCommittee()

        Try
            Dim nExcelRowNum As Integer = PublicCommand.StrnumToInt(MyBase.GetMemberTblCell)
            MyBase.ApplyMemberOfCommittee(nExcelRowNum)
            MyBase.SelectRange("A1:A1")
            'Catch exception As AppUnionException
            '    exception.AddMethodName(MethodBase.GetCurrentMethod)
            '    Throw exception
            'Catch exception2 As SysUnionException
            '    exception2.AddMethodName(MethodBase.GetCurrentMethod)
            '    Throw exception2
        Catch ex As Exception

        End Try

    End Sub

    Public Overrides Sub InitNewDocument()

        Try
            MyBase.InitNewDocument()
            Me.SetFixedSentence()
            Me.ApplyMemberOfCommittee()

        Catch ex As Exception
            Call MessageBox.Show("エラーが発生しました。" & vbCrLf & ex.Message, _
                                 "エラー", _
                                 MessageBoxButtons.OK, _
                                 MessageBoxIcon.Hand)
        End Try

    End Sub
End Class
