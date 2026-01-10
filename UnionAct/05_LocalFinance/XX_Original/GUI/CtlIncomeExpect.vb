Imports C1.Win.C1FlexGrid
Imports C1.Win.C1FlexGrid.Util.BaseControls
Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
Imports UnionAct.NSCLMsg
Imports UnionAct.GUI.UnionComponent
Imports UnionAct.Framework.Command
Imports UnionAct.Business.Master
Imports UnionAct.Framework.Mapping
Imports UnionAct.Business.RevenueExpenditure
Imports UnionAct.NSMDInfo
Imports log4net

Namespace GUI.RevenueExpenditure.UnionForm
    Public Class CtlIncomeExpect
        Inherits RevenueExpenditureBase
        ' Methods
        Private Sub New()
            MyBase.New()
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal _strRevenueStart As String, ByVal _strRevenueEnd As String, ByVal _strLastRevenueStart As String, ByVal _IsNewFlg As Boolean, ByVal _IsChangeFlg As Boolean, ByVal _strRevenueTitle As String, ByVal _lRevenueExpenditureTtl As Long, ByVal _dtDup As Object, ByVal _IsReferenceRight As Boolean, ByVal _IsGetEntryRight As Boolean, ByVal _IsPrintRight As Boolean, ByVal parent As UC050401)
            MyBase.New(_IsGetEntryRight, _IsPrintRight, _IsReferenceRight, _strRevenueTitle, _dtDup, _strRevenueStart, _strRevenueEnd, _strLastRevenueStart, _IsNewFlg, _IsChangeFlg)
            Try
                Me.InitializeComponent()
                Me.lbltitle.Text = _strRevenueTitle
                Me.txtRevenueStrDate.Text = DateTime.ParseExact(_strRevenueStart, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月" & "dd" & "日")
                Me.txtRevenueEndDate.Text = DateTime.ParseExact(_strRevenueEnd, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月" & "dd" & "日")
                Me.grpRevenueExpenditureList.Text = (Me.grpRevenueExpenditureList.Text & "(" & Me.txtRevenueStrDate.Text & "の予想組織人数と組合費総額）")
                If (Not _dtDup Is Nothing) Then
                    Me.strDataUpTime = _dtDup.ToString
                Else
                    Me.strDataUpTime = ""
                End If
                Me._parent = parent
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Utilities.RestoreUserControl()
                _parent.ActionAfterResotreUserControl()
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                'Throw exception2
                MsgBox(exception2.ToString)
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If CLMsg.Show("GQ0007").Equals(DialogResult.Yes) Then
                    If MyBase._IsNewFlg Then
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                    Else
                        Me.SetNewComponent()
                        Me.SetDetailData()
                    End If
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                'Throw exception2
                MsgBox(exception2.ToString)
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub btnChange_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me.btnChange.Visible = False
                Me.btnNewEntry2.Visible = True
                Me.btnCancel.Visible = True
                Me.btnBack.Visible = False
                Me.btnPrinting.Visible = False
                Me.FlexAllowEditing(True)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                'Throw exception2
                MsgBox(exception2.ToString)
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub btnNewEntry_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim ds As DataSet = Nothing
                Dim set2 As DataSet = Nothing
                Me.Cursor = Cursors.WaitCursor
                Me.ConvertFlexGridToDb(ds, set2)
                Dim viewer As New ReportViewer(set2, New CR0504P5, 1)
                Select Case viewer.ConfirmViewerShow
                    Case 0
                        command.ClickNewEntrButton(ds)
                        viewer.PrintOut()
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                        Exit Select
                    Case 1
                        command.ClickNewEntrButton(ds)
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                        Exit Select
                End Select
                viewer.RptDataDispose()
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                'Throw exception2
                MsgBox(exception2.ToString)
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnNewEntry2_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim ds As DataSet = Nothing
                Dim set2 As DataSet = Nothing
                MyBase.CheckUpdateMessage(MyBase.Name)
                Me.Cursor = Cursors.WaitCursor
                Me.ConvertFlexGridToDb(ds, set2)
                Dim viewer As New ReportViewer(set2, New CR0504P5, 1)
                Select Case viewer.ConfirmViewerShow
                    Case 0
                        command.ClickChangeButton(ds, Me.strDataUpTime)
                        viewer.PrintOut()
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                        Exit Select
                    Case 1
                        command.ClickChangeButton(ds, Me.strDataUpTime)
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                        Exit Select
                End Select
                viewer.RptDataDispose()
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                'Throw exception2
                MsgBox(exception2.ToString)
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnPrinting_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim ds As DataSet = Nothing
                Dim set2 As DataSet = Nothing
                Me.Cursor = Cursors.WaitCursor
                Me.ConvertFlexGridToDb(ds, set2)
                Dim viewer As New ReportViewer(set2, New CR0504P5, 3)
                viewer.ReportViewerShow()
                viewer.RptDataDispose()
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                'Throw exception2
                MsgBox(exception2.ToString)
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub CalcTotalNumberUnionDues(ByVal iRow As Integer)
            Try
                Dim num As Integer = 0
                Dim num2 As Long = 0
                Dim map As New IncomeExpectMap
                Dim i As Integer
                For i = 0 To Me.dTableFlexGrid.Columns.Count - 1
                    Dim physicalName As String = map.GetPhysicalName(i)
                    If (physicalName.Length >= 9) Then
                        physicalName = physicalName.Substring(2, 7)
                        If physicalName.Equals("_number") Then
                            Try
                                num = (num + Convert.ToInt32(Me.dTableFlexGrid.Rows.Item(iRow).Item(i)))
                            Catch obj1 As Exception
                            End Try
                        End If
                        If physicalName.Equals("_union_") Then
                            Try
                                num2 = (num2 + Convert.ToInt64(Me.dTableFlexGrid.Rows.Item(iRow).Item(i)))
                            Catch exception As OverflowException
                                If (Not Me.flxIncomeExpectList.Editor Is Nothing) Then
                                    Me.flxIncomeExpectList.FinishEditing(True)
                                End If
                                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "GE0174", New String() {"合計"})
                            Catch obj2 As Exception
                            End Try
                        End If
                    End If
                Next i
                Me.dTableFlexGrid.Rows.Item(iRow).Item("合計　人数") = num
                Me.dTableFlexGrid.Rows.Item(iRow).Item("合計　組合費") = num2
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Function CalcUnionDues(ByVal iAge As Integer, ByVal strQualification As String, ByVal iNumber As Integer) As Long
            Dim num2 As Long
            Try
                Dim command As New UnionDuesCommand
                num2 = ((command.GetUnionDuesCommand("01", strQualification, "01", iAge, MyBase.RevenueStartDate) * iNumber) * 12)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

        Private Sub ConvertFlexGridToDb(<Out()> ByRef dSetDB As DataSet, <Out()> ByRef dSetReport As DataSet)
            Try
                dSetDB = command.CreateDataBaseStyle(MyBase.RevenueStartDate, Me.dTableFlexGrid, Me.lRevenueExpendtureTtl, Me.iSeniorMonthwork, MyBase.RevenueStartDate, MyBase.RevenueEndDate, MyBase.RevenueTitle)
                dSetReport = dSetDB.Copy
                Dim table As DataTable = Me.CreateReportData(dSetReport.Tables.Item("dtDetail")).Copy
                dSetReport.Tables.Remove(dSetReport.Tables.Item("dtDetail"))
                dSetReport.Tables.Add(table)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Function CreateReportData(ByVal dTableIn As DataTable) As DataTable
            Dim table2 As DataTable
            Try
                Dim num As Integer
                Dim listDataTable As DataTable = New InfoConstant(MyBase.RevenueStartDate).GetListDataTable("QUALIFICATION", "c_constant")
                'num()
                For num = 0 To dTableIn.Rows.Count - 1
                    Dim rowArray As DataRow() = listDataTable.Select(("c_constant_seq = '" & dTableIn.Rows.Item(num).Item("k_qualification").ToString & "'"))
                    dTableIn.Rows.Item(num).Item("k_qualification") = rowArray(0).Item("l_omission_name").ToString
                Next num
                Dim iAgeStart As Integer = &H16
                Dim iAgeEnd As Integer = 70
                command.GetRegularShowTerm(MyBase.RevenueStartDate, iAgeStart, iAgeEnd)
                'num()
                For num = 0 To dTableIn.Rows.Count - 1
                    Dim i As Integer
                    For i = 0 To dTableIn.Columns.Count - 1
                        Dim caption As String = dTableIn.Columns.Item(i).Caption
                        If ((caption.Length = 11) AndAlso caption.Substring(0, 9).Equals("s_number_")) Then
                            Try
                                If (Convert.ToInt32(caption.Substring(9, 2)) > iAgeEnd) Then
                                    dTableIn.Rows.Item(num).Item(i) = 0
                                    dTableIn.Rows.Item(num).Item((i + 1)) = 0
                                End If
                            Catch obj1 As Exception
                            End Try
                        End If
                    Next i
                Next num
                table2 = dTableIn
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Private Sub CtlIncomeExpect_Load(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me.SetNewComponent()
                If MyBase._IsNewFlg Then
                    Me.SetNewEntryData()
                Else
                    Me.SetDetailData()
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub FlexAllowEditing(ByVal IsChangeOk As Boolean)
            Try
                Dim num As Integer
                Me.flxIncomeExpectList.AllowEditing = False
                If IsChangeOk Then
                    Me.flxIncomeExpectList.AllowEditing = True
                    'num()
                    For num = 0 To Me.flxIncomeExpectList.Cols.Count - 1
                        Me.flxIncomeExpectList.Cols.Item(num).AllowEditing = True
                    Next num
                    'num()
                    For num = 0 To 4 - 1
                        Me.flxIncomeExpectList.Cols.Item((3 + (2 * num))).AllowEditing = False
                    Next num
                ElseIf (Me.btnChange.Visible OrElse Me.btnChange.Enabled) Then
                    'num()
                    For num = 0 To Me.flxIncomeExpectList.Cols.Count - 1
                        Me.flxIncomeExpectList.Cols.Item(num).AllowEditing = False
                    Next num
                End If
                'num()
                For num = 0 To 4 - 1
                    Me.flxIncomeExpectList.Cols.Item((3 + (2 * num))).Style.BackColor = Color.LightYellow
                    If (num = 3) Then
                        Me.flxIncomeExpectList.Cols.Item((2 + (2 * num))).AllowEditing = False
                        Me.flxIncomeExpectList.Cols.Item((2 + (2 * num))).Style.BackColor = Color.PaleGoldenrod
                        Me.flxIncomeExpectList.Cols.Item((3 + (2 * num))).AllowEditing = False
                        Me.flxIncomeExpectList.Cols.Item((3 + (2 * num))).Style.BackColor = Color.PaleGoldenrod
                    End If
                Next num
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub FlexGridDataSet(ByVal dTableIn As DataTable, ByVal IsNewFlg As Boolean)
            Try
                Dim num As Integer
                Me.dTableFlexGrid = dTableIn.Copy
                Me.flxIncomeExpectList.DataSource = Me.dTableFlexGrid
                Me.flxIncomeExpectList.Item(0, 0) = "年齢"
                Me.flxIncomeExpectList.Item(1, 0) = "年齢"
                Me.flxIncomeExpectList.Item(0, 2) = "機長"
                Me.flxIncomeExpectList.Item(0, 3) = "機長"
                Me.flxIncomeExpectList.Item(0, 4) = "副操縦士"
                Me.flxIncomeExpectList.Item(0, 5) = "副操縦士"
                Me.flxIncomeExpectList.Item(0, 6) = "航空機関士"
                Me.flxIncomeExpectList.Item(0, 7) = "航空機関士"
                Me.flxIncomeExpectList.Item(0, 8) = "合計"
                Me.flxIncomeExpectList.Item(0, 9) = "合計"
                'num()
                For num = 0 To 4 - 1
                    Me.flxIncomeExpectList.Item(1, (2 + (2 * num))) = "人数"
                    Me.flxIncomeExpectList.Item(1, (3 + (2 * num))) = "組合費"
                Next num
                Me.flxIncomeExpectList.Rows.Item(0).AllowMerging = True
                Me.flxIncomeExpectList.Cols.Item(0).AllowMerging = True
                'num()
                For num = 2 To Me.flxIncomeExpectList.Rows.Count - 1
                    Me.flxIncomeExpectList.Item(num, 0) = Me.dTableFlexGrid.Rows.Item((num - 2)).Item("年齢")
                Next num
                Me.flxIncomeExpectList.Cols.Item(0).Width = 50
                Me.flxIncomeExpectList.Cols.Item(1).Width = 0
                'num()
                For num = 0 To 4 - 1
                    Me.flxIncomeExpectList.Cols.Item((2 + (2 * num))).Width = 100
                    Me.flxIncomeExpectList.Cols.Item((3 + (2 * num))).Width = 110
                Next num
                MDFinanceCommon.AdjustTextAlign(Me.flxIncomeExpectList)
                Me.SetFlexGridHight()
                'num()
                For num = 0 To Me.flxIncomeExpectList.Cols.Count - 1
                    Me.flxIncomeExpectList.Cols.Item(num).Style.Border.Color = Color.Silver
                Next num
                Me.FlexAllowEditing(IsNewFlg)
                'num()
                For num = 0 To 4 - 1
                    Me.flxIncomeExpectList.Cols.Item((2 + (2 * num))).Format = "N0"
                    Me.flxIncomeExpectList.Cols.Item((3 + (2 * num))).Format = "N0"
                Next num
                Me.SetTotalText()
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub FlxGridEntry()
            Try
                Dim iRow As Integer = (Me.flxIncomeExpectList.Row - 2)
                Dim num2 As Integer = (Me.flxIncomeExpectList.Col - 1)
                If ((iRow >= 0) AndAlso (num2 >= 0)) Then
                    Me.CalcTotalNumberUnionDues(iRow)
                    Me.SetTotalText()
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub FlxGridValidateEdit()
            Try
                Dim flag As Boolean = False
                Dim num As Integer = (Me.flxIncomeExpectList.Row - 2)
                Dim num2 As Integer = (Me.flxIncomeExpectList.Col - 1)
                If ((num >= 0) AndAlso (num2 >= 0)) Then
                    Dim map As New IncomeExpectMap
                    Dim iNumber As Integer = 0
                    Dim iAge As Integer = Convert.ToInt32(Me.dTableFlexGrid.Rows.Item(num).Item("年齢"))
                    Try
                        iNumber = Convert.ToInt32(Me.flxIncomeExpectList.Editor.Text)
                    Catch obj1 As Exception
                        Dim text As String = Me.flxIncomeExpectList.Editor.Text
                        CLMsg.Show("GE0178")
                        Me.flxIncomeExpectList.FinishEditing(True)
                        flag = True
                    End Try
                    If (iNumber < 0) Then
                        CLMsg.Show("GE0173")
                        Me.flxIncomeExpectList.FinishEditing(True)
                        flag = True
                    End If
                    If Not flag Then
                        Dim caption As String = Me.dTableFlexGrid.Columns.Item(num2).Caption
                        Dim strQualification As String = map.GetPhysicalName(map.GetLogicalIndex(caption)).Substring(0, 2)
                        Dim num5 As Long = Me.CalcUnionDues(iAge, strQualification, iNumber)
                        Me.flxIncomeExpectList.Item(Me.flxIncomeExpectList.Row, (Me.flxIncomeExpectList.Col + 1)) = num5
                    End If
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub flxIncomeExpectList_AfterEdit(ByVal sender As Object, ByVal e As RowColEventArgs)
            Try
                Me.FlxGridEntry()
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub flxIncomeExpectList_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If Me.flxIncomeExpectList.Cols.Item(Me.flxIncomeExpectList.Col).AllowEditing Then
                    Me.flxIncomeExpectList.StartEditing(Me.flxIncomeExpectList.Row, Me.flxIncomeExpectList.Col)
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub flxIncomeExpectList_SetupEditor(ByVal sender As Object, ByVal e As RowColEventArgs)
            Try
                Dim editor As TextBox = DirectCast(Me.flxIncomeExpectList.Editor, TextBox)
                editor.ImeMode = ImeMode.Disable
                editor.MaxLength = 3
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub flxIncomeExpectList_ValidateEdit(ByVal sender As Object, ByVal e As ValidateEditEventArgs)
            Try
                Me.FlxGridValidateEdit()
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub InitializeComponent()
            Dim manager As New ComponentResourceManager(GetType(CtlIncomeExpect))
            Me.grpExpenditure = New GroupBox
            Me.lbltitle = New Label
            Me.txtRevenueStrDate = New PersonalTextBox
            Me.txtRevenueEndDate = New PersonalTextBox
            Me.label1 = New Label
            Me.label2 = New Label
            Me.grpRevenueExpenditureList = New GroupBox
            Me.label3 = New Label
            Me.lblTotalNumber = New Label
            Me.label8 = New Label
            Me.lblRevenueExpenditureTtl = New Label
            Me.flxIncomeExpectList = New C1FlexGrid
            Me.btnChange = New Button
            Me.btnPrinting = New Button
            Me.btnCancel = New Button
            Me.btnNewEntry = New Button
            Me.btnBack = New Button
            Me.btnNewEntry2 = New Button
            Me.grpExpenditure.SuspendLayout()
            Me.grpRevenueExpenditureList.SuspendLayout()
            Me.flxIncomeExpectList.BeginInit()
            MyBase.SuspendLayout()
            Me.grpExpenditure.Controls.Add(Me.lbltitle)
            Me.grpExpenditure.Controls.Add(Me.txtRevenueStrDate)
            Me.grpExpenditure.Controls.Add(Me.txtRevenueEndDate)
            Me.grpExpenditure.Controls.Add(Me.label1)
            Me.grpExpenditure.Controls.Add(Me.label2)
            Me.grpExpenditure.Location = New Point(30, &H10)
            Me.grpExpenditure.Name = "grpExpenditure"
            Me.grpExpenditure.Size = New Size(&H3CC, &H3F)
            Me.grpExpenditure.TabIndex = 7
            Me.grpExpenditure.TabStop = False
            Me.lbltitle.AutoSize = True
            Me.lbltitle.Location = New Point(&H36, &H1B)
            Me.lbltitle.Name = "lbltitle"
            Me.lbltitle.Size = New Size(&H13, &H10)
            Me.lbltitle.TabIndex = &H83
            Me.lbltitle.Text = "　"
            Me.txtRevenueStrDate.BackColor = Color.LightYellow
            Me.txtRevenueStrDate.FieldAttribute = EFieldAttribute.NONE
            Me.txtRevenueStrDate.Location = New Point(&H145, &H19)
            Me.txtRevenueStrDate.Name = "txtRevenueStrDate"
            Me.txtRevenueStrDate.ReadOnly = True
            Me.txtRevenueStrDate.Require = False
            Me.txtRevenueStrDate.Size = New Size(&H95, &H17)
            Me.txtRevenueStrDate.TabIndex = 130
            Me.txtRevenueStrDate.TabStop = False
            Me.txtRevenueStrDate.TextAlign = HorizontalAlignment.Center
            Me.txtRevenueEndDate.BackColor = Color.LightYellow
            Me.txtRevenueEndDate.FieldAttribute = EFieldAttribute.NONE
            Me.txtRevenueEndDate.Location = New Point(510, &H19)
            Me.txtRevenueEndDate.Name = "txtRevenueEndDate"
            Me.txtRevenueEndDate.ReadOnly = True
            Me.txtRevenueEndDate.Require = False
            Me.txtRevenueEndDate.Size = New Size(&H95, &H17)
            Me.txtRevenueEndDate.TabIndex = &H7D
            Me.txtRevenueEndDate.TabStop = False
            Me.txtRevenueEndDate.TextAlign = HorizontalAlignment.Center
            Me.label1.AutoSize = True
            Me.label1.Location = New Point(480, &H1C)
            Me.label1.Name = "label1"
            Me.label1.Size = New Size(&H18, &H10)
            Me.label1.TabIndex = &H7B
            Me.label1.Text = "～"
            Me.label2.AutoSize = True
            Me.label2.Location = New Point(&HF7, &H1D)
            Me.label2.Name = "label2"
            Me.label2.Size = New Size(&H48, &H10)
            Me.label2.TabIndex = &H77
            Me.label2.Text = "予想期間"
            Me.grpRevenueExpenditureList.Controls.Add(Me.label3)
            Me.grpRevenueExpenditureList.Controls.Add(Me.lblTotalNumber)
            Me.grpRevenueExpenditureList.Controls.Add(Me.label8)
            Me.grpRevenueExpenditureList.Controls.Add(Me.lblRevenueExpenditureTtl)
            Me.grpRevenueExpenditureList.Controls.Add(Me.flxIncomeExpectList)
            Me.grpRevenueExpenditureList.Location = New Point(30, &H60)
            Me.grpRevenueExpenditureList.Name = "grpRevenueExpenditureList"
            Me.grpRevenueExpenditureList.Size = New Size(&H3CC, &H257)
            Me.grpRevenueExpenditureList.TabIndex = 5
            Me.grpRevenueExpenditureList.TabStop = False
            Me.grpRevenueExpenditureList.Text = "収入予想　・　予想組織人数一覧　"
            Me.label3.AutoSize = True
            Me.label3.Location = New Point(&H228, &H23D)
            Me.label3.Name = "label3"
            Me.label3.Size = New Size(&H38, &H10)
            Me.label3.TabIndex = 140
            Me.label3.Text = "総人数"
            Me.lblTotalNumber.BorderStyle = BorderStyle.Fixed3D
            Me.lblTotalNumber.Font = New Font("ＭＳ" & " " & "ゴシック", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblTotalNumber.Location = New Point(610, &H23B)
            Me.lblTotalNumber.Name = "lblTotalNumber"
            Me.lblTotalNumber.Size = New Size(110, &H15)
            Me.lblTotalNumber.TabIndex = &H8B
            Me.lblTotalNumber.Text = "999,999,999"
            Me.lblTotalNumber.TextAlign = ContentAlignment.MiddleRight
            Me.label8.AutoSize = True
            Me.label8.Location = New Point(&H2E0, &H23D)
            Me.label8.Name = "label8"
            Me.label8.Size = New Size(&H33, &H10)
            Me.label8.TabIndex = &H8A
            Me.label8.Text = "総　額"
            Me.lblRevenueExpenditureTtl.BorderStyle = BorderStyle.Fixed3D
            Me.lblRevenueExpenditureTtl.Font = New Font("ＭＳ" & " " & "ゴシック", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblRevenueExpenditureTtl.Location = New Point(&H317, &H23B)
            Me.lblRevenueExpenditureTtl.Name = "lblRevenueExpenditureTtl"
            Me.lblRevenueExpenditureTtl.Size = New Size(&H84, &H15)
            Me.lblRevenueExpenditureTtl.TabIndex = &H89
            Me.lblRevenueExpenditureTtl.Text = "999,999,999,999"
            Me.lblRevenueExpenditureTtl.TextAlign = ContentAlignment.MiddleRight
            Me.flxIncomeExpectList.AllowDragging = AllowDraggingEnum.None
            Me.flxIncomeExpectList.AllowEditing = False
            Me.flxIncomeExpectList.AllowMerging = AllowMergingEnum.Free
            Me.flxIncomeExpectList.AllowSorting = AllowSortingEnum.None
            Me.flxIncomeExpectList.AutoResize = False
            Me.flxIncomeExpectList.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
            'TODO Me.flxIncomeExpectList.ColumnInfo = manager.GetString("flxIncomeExpectList.ColumnInfo")
            Me.flxIncomeExpectList.FocusRect = FocusRectEnum.None
            Me.flxIncomeExpectList.Location = New Point(30, &H16)
            Me.flxIncomeExpectList.Name = "flxIncomeExpectList"
            Me.flxIncomeExpectList.Rows.Count = &H26
            Me.flxIncomeExpectList.Rows.DefaultSize = 20
            Me.flxIncomeExpectList.Rows.Fixed = 2
            Me.flxIncomeExpectList.SelectionMode = SelectionModeEnum.Cell
            Me.flxIncomeExpectList.Size = New Size(&H38F, &H21F)
            'TODO Me.flxIncomeExpectList.StyleInfo = manager.GetString("flxIncomeExpectList.StyleInfo")
            Me.flxIncomeExpectList.TabIndex = 6
            AddHandler Me.flxIncomeExpectList.AfterEdit, New RowColEventHandler(AddressOf Me.flxIncomeExpectList_AfterEdit)
            AddHandler Me.flxIncomeExpectList.ValidateEdit, New ValidateEditEventHandler(AddressOf Me.flxIncomeExpectList_ValidateEdit)
            AddHandler Me.flxIncomeExpectList.SetupEditor, New RowColEventHandler(AddressOf Me.flxIncomeExpectList_SetupEditor)
            AddHandler Me.flxIncomeExpectList.Click, New EventHandler(AddressOf Me.flxIncomeExpectList_Click)
            Me.btnChange.Location = New Point(&H2D5, &H2C9)
            Me.btnChange.Name = "btnChange"
            Me.btnChange.Size = New Size(&H74, &H20)
            Me.btnChange.TabIndex = 1
            Me.btnChange.Text = "内容変更"
            Me.btnChange.UseVisualStyleBackColor = True
            AddHandler Me.btnChange.Click, New EventHandler(AddressOf Me.btnChange_Click)
            Me.btnPrinting.Location = New Point(&H3D, &H2C9)
            Me.btnPrinting.Name = "btnPrinting"
            Me.btnPrinting.Size = New Size(&H74, &H20)
            Me.btnPrinting.TabIndex = 4
            Me.btnPrinting.Text = "プレ印刷"
            Me.btnPrinting.UseVisualStyleBackColor = True
            AddHandler Me.btnPrinting.Click, New EventHandler(AddressOf Me.btnPrinting_Click)
            Me.btnCancel.Location = New Point(&H357, &H2C9)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(&H74, &H20)
            Me.btnCancel.TabIndex = 2
            Me.btnCancel.Text = "キャンセル"
            Me.btnCancel.UseVisualStyleBackColor = True
            AddHandler Me.btnCancel.Click, New EventHandler(AddressOf Me.btnCancel_Click)
            Me.btnNewEntry.Location = New Point(&H2D5, &H2C9)
            Me.btnNewEntry.Name = "btnNewEntry"
            Me.btnNewEntry.Size = New Size(&H74, &H20)
            Me.btnNewEntry.TabIndex = 0
            Me.btnNewEntry.Text = "登録確認"
            Me.btnNewEntry.UseVisualStyleBackColor = True
            AddHandler Me.btnNewEntry.Click, New EventHandler(AddressOf Me.btnNewEntry_Click)
            Me.btnBack.Location = New Point(&H356, &H2C9)
            Me.btnBack.Name = "btnBack"
            Me.btnBack.Size = New Size(&H74, &H20)
            Me.btnBack.TabIndex = 3
            Me.btnBack.Text = "戻る"
            Me.btnBack.UseVisualStyleBackColor = True
            AddHandler Me.btnBack.Click, New EventHandler(AddressOf Me.btnBack_Click)
            Me.btnNewEntry2.Location = New Point(&H2D5, &H2C9)
            Me.btnNewEntry2.Name = "btnNewEntry2"
            Me.btnNewEntry2.Size = New Size(&H74, &H20)
            Me.btnNewEntry2.TabIndex = 8
            Me.btnNewEntry2.Text = "登録確認"
            Me.btnNewEntry2.UseVisualStyleBackColor = True
            AddHandler Me.btnNewEntry2.Click, New EventHandler(AddressOf Me.btnNewEntry2_Click)
            MyBase.Controls.Add(Me.btnNewEntry2)
            MyBase.Controls.Add(Me.btnBack)
            MyBase.Controls.Add(Me.btnNewEntry)
            MyBase.Controls.Add(Me.btnChange)
            MyBase.Controls.Add(Me.btnPrinting)
            MyBase.Controls.Add(Me.btnCancel)
            MyBase.Controls.Add(Me.grpRevenueExpenditureList)
            MyBase.Controls.Add(Me.grpExpenditure)
            Me.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            MyBase.Name = "CtlIncomeExpect"
            MyBase.Size = New Size(&H402, &H2F7)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.CtlIncomeExpect_Load)
            Me.grpExpenditure.ResumeLayout(False)
            Me.grpExpenditure.PerformLayout()
            Me.grpRevenueExpenditureList.ResumeLayout(False)
            Me.grpRevenueExpenditureList.PerformLayout()
            Me.flxIncomeExpectList.EndInit()
            MyBase.ResumeLayout(False)
        End Sub

        Private Sub SetDetailData()
            Try
                Dim dTableIn As DataTable = command.GetDetailData(PublicCommand.GetMac, MyBase.Name, MDLoginInfo.UserId, MyBase.RevenueStartDate, MDLoginInfo.Ksh, Me.iSeniorMonthwork)
                Me.FlexGridDataSet(dTableIn, False)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub SetFlexGridHight()
            Try
                Dim iAgeStart As Integer = &H16
                Dim iAgeEnd As Integer = 70
                command.GetRegularShowTerm(MyBase.RevenueStartDate, iAgeStart, iAgeEnd)
                If (iAgeStart < &H16) Then
                    iAgeStart = &H16
                End If
                If (iAgeEnd > 70) Then
                    iAgeEnd = 70
                End If
                If (Me.dTableFlexGrid.Rows.Count > 0) Then
                    Dim num As Integer
                    Dim physicalIndex As Integer = New IncomeExpectMap().GetPhysicalIndex("total_number")
                    num = &H16
                    Do While (num <= iAgeStart)
                        Dim rowArray As DataRow() = Me.dTableFlexGrid.Select(("年齢" & " = '" & Convert.ToInt32(num) & "' "))
                        If (((rowArray.Length > 0) AndAlso Not rowArray(0).Item(physicalIndex).ToString.Equals("")) AndAlso (Convert.ToInt32(rowArray(0).Item(physicalIndex).ToString) > 0)) Then
                            iAgeStart = num
                            Exit Do
                        End If
                        num += 1
                    Loop
                    'num()
                    For num = 2 To Me.flxIncomeExpectList.Rows.Count - 1
                        Try
                            Dim num5 As Integer = Convert.ToInt32(Me.flxIncomeExpectList.Item(num, 0))
                            If ((num5 > iAgeEnd) OrElse (num5 < iAgeStart)) Then
                                Me.flxIncomeExpectList.Rows.Item(num).Visible = False
                            End If
                        Catch obj1 As Exception
                        End Try
                    Next num
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub SetNewComponent()
            Try
                Me.btnPrinting.Visible = False
                Me.btnChange.Visible = False
                Me.btnNewEntry.Visible = False
                Me.btnNewEntry2.Visible = False
                Me.btnBack.Visible = False
                Me.btnCancel.Visible = False
                If MyBase._IsNewFlg Then
                    Me.btnCancel.Visible = True
                Else
                    Me.btnBack.Visible = True
                End If
                Dim isReferenceRight As Boolean = MyBase.IsReferenceRight
                If MyBase.IsGetEntryRight Then
                    Me.btnNewEntry.Visible = MyBase._IsNewFlg
                    If Not MyBase._IsNewFlg Then
                        Me.btnChange.Visible = MyBase.IsChangeFlg
                    End If
                End If
                If MyBase.IsPrintRight Then
                    Me.btnPrinting.Visible = Not MyBase._IsNewFlg
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub SetNewEntryData()
            Try
                Dim dTableIn As DataTable = command.GetNewEntryData(PublicCommand.GetMac, MyBase.Name, MDLoginInfo.UserId, MyBase.RevenueStartDate, MDLoginInfo.Ksh, Me.iSeniorMonthwork)
                Me.FlexGridDataSet(dTableIn, True)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub SetTotalText()
            Try
                If (Me.dTableFlexGrid Is Nothing) Then
                    Me.lblTotalNumber.Text = "0"
                    Me.lblRevenueExpenditureTtl.Text = "0"
                ElseIf (Me.dTableFlexGrid.Rows.Count = 0) Then
                    Me.lblTotalNumber.Text = "0"
                    Me.lblRevenueExpenditureTtl.Text = "0"
                Else
                    Dim num As Integer = 0
                    Dim num2 As Long = 0
                    Dim i As Integer
                    For i = 0 To Me.dTableFlexGrid.Rows.Count - 1
                        If Me.flxIncomeExpectList.Rows.Item((i + 2)).Visible Then
                            Try
                                num = (num + Convert.ToInt32(Me.dTableFlexGrid.Rows.Item(i).Item("合計　人数")))
                                num2 = (num2 + Convert.ToInt64(Me.dTableFlexGrid.Rows.Item(i).Item("合計　組合費")))
                                _logger.Info("num=" & num)
                            Catch exception As OverflowException
                                If (Not Me.flxIncomeExpectList.Editor Is Nothing) Then
                                    Me.flxIncomeExpectList.FinishEditing(True)
                                End If
                                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "GE0174", New String() {"合計"})
                            Catch obj1 As Exception
                            End Try
                        End If
                    Next i
                    Me.lblTotalNumber.Text = String.Format("{0:N0}", num)
                    Me.lblRevenueExpenditureTtl.Text = String.Format("{0:N0}", num2)
                    Me.lRevenueExpendtureTtl = num2
                End If
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "GE0001", New String(0 - 1) {})
            End Try
        End Sub


        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Private command As New IncomeExpectCommand
        Private Const AGE_COL As String = "年齢"
        Private Const AGE_DB_MAX As Integer = 70
        Private Const AGE_DB_MIN As Integer = &H16
        Private btnBack As Button
        Private btnCancel As Button
        Private btnChange As Button
        Private btnNewEntry As Button
        Private btnNewEntry2 As Button
        Private btnPrinting As Button
        Private components As IContainer
        Private dTableFlexGrid As DataTable
        Private flxIncomeExpectList As C1FlexGrid
        Private grpExpenditure As GroupBox
        Private grpRevenueExpenditureList As GroupBox
        Private iSeniorMonthwork As Integer
        Private label1 As Label
        Private label2 As Label
        Private label3 As Label
        Private label8 As Label
        Private lblRevenueExpenditureTtl As Label
        Private lbltitle As Label
        Private lblTotalNumber As Label
        Private lRevenueExpendtureTtl As Long
        Private strDataUpTime As String
        Private txtRevenueEndDate As PersonalTextBox
        Private txtRevenueStrDate As PersonalTextBox
        Private _parent As UC050401
    End Class
End Namespace
