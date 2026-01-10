
'役員改選について(Template001)

Imports Microsoft.Office.Interop.Excel
Imports System
Imports System.Reflection

    Public Class DocTemplate001
        Inherits DocTemplateBase
    ' Methods

    Public Overrides Sub ApplyOfficers()
        'Try
        '    Dim nExcelRowNum As Integer = PublicCommand.StrnumToInt(MyBase.objExcelConst.GetMemberTblCell)
        '    MyBase.WriteExecutiveMembers(nExcelRowNum, True)
        '    MyBase.WriteAuditorMembers(nExcelRowNum, True)
        '    MyBase.WriteBranchMembers(nExcelRowNum, True)
        '    MyBase.objExcelCommand.AddTurningPage(++nExcelRowNum)
        '    MyBase.WriteDelegateMembers(nExcelRowNum)
        '    Dim strCell As String = MyBase.objSentenceData.FinishSentenceCell.Replace("{R}", nExcelRowNum.ToString)
        '    MyBase.objExcelCommand.SetDataAppointCell(strCell, MyBase.objSentenceData.FinishSentence)
        '    nExcelRowNum = (nExcelRowNum + 2)
        '    strCell = MyBase.objSentenceData.ClosingRemarksCell.Replace("{R}", nExcelRowNum.ToString)
        '    MyBase.objExcelCommand.SetTextHAlign(String.Format("{0}:{0}", strCell), Constants.xlRight)
        '    MyBase.objExcelCommand.SetDataAppointCell(strCell, MyBase.objSentenceData.ClosingRemarks)
        '    MyBase.objExcelCommand.SelectRange("A1:A1")
        'Catch exception As AppUnionException
        '    exception.AddMethodName(MethodBase.GetCurrentMethod)
        '    Throw exception
        'Catch exception2 As SysUnionException
        '    exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '    Throw exception2
        'Catch exception3 As Exception
        '    Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        'End Try
    End Sub

    'ドキュメントの初期化処理
    Public Overrides Sub InitNewDocument()
        MyBase.InitNewDocument()
        
    End Sub

    End Class

