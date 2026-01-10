Imports C1.Win.C1FlexGrid
Imports UnionAct.Framework.UnionException
Imports System.Reflection
Imports UnionAct.GUI.Common
Imports UnionAct.Business.FinancialAffairs.DailyAllowance
Imports UnionAct.GUI.Document
Imports UnionAct.NSCLMsg

Public Class UC050204

    Public Sub New(ByVal OwnerInstance As UC050201, ByVal tabPageIdx As Integer)
        Me.InitializeComponent()
        Me.OwnerInstance = OwnerInstance
        Me.dsDailyAllowance = OwnerInstance.dsDailyAllowance.Copy
        Me.tabPageIdx = tabPageIdx
    End Sub

    Private Sub InitData()
        Try
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim time As DateTime = DateTime.ParseExact(Me.dsDailyAllowance.Tables.Item("branch_belong_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
            Dim strCloseDate As String = (time.Year.ToString.PadLeft(4, "0"c) & time.Month.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(time.Year, time.Month).ToString.PadLeft(2, "0"c))
            Dim time2 As DateTime = CDate(Me.dsDailyAllowance.Tables.Item("branch_close_list").Rows.Item((Me.OwnerInstance.flxBranchCloseList.Row - 1)).Item("d_begin"))
            Dim strBelongingId As String = Me.dsDailyAllowance.Tables.Item("branch_belong_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("k_belonging").ToString
            Dim str3 As String = Me.dsDailyAllowance.Tables.Item("branch_belong_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("支部").ToString
            Me.lblCloseYear.Text = time.Year.ToString.PadLeft(4, "0"c)
            Me.lblCloseMonth.Text = time.Month.ToString.PadLeft(2, "0"c)
            Me.lblYearFrom.Text = time2.Year.ToString.PadLeft(4, "0"c)
            Me.lblMonthFrom.Text = time2.Month.ToString.PadLeft(2, "0"c)
            Me.lblYearTo.Text = time.Year.ToString.PadLeft(4, "0"c)
            Me.lblMonthTo.Text = time.Month.ToString.PadLeft(2, "0"c)
            Me.lblBelonging.Text = str3
            'Dim table As DataTable = command.GetCommitteeCloseDtl(strBelongingId, Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabPageIdx).Item("c_constant_seq").ToString, strCloseDate).Tables.Item("branch_close_dtl").Copy
            Dim table As DataTable = command.GetCommitteeCloseDtl(strBelongingId, "02", strCloseDate).Tables.Item("branch_close_dtl").Copy 'TODO
            If Me.dsDailyAllowance.Tables.Contains("branch_close_dtl") Then
                Me.dsDailyAllowance.Tables.Remove("branch_close_dtl")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxDailyPayClose.DataSource = Me.dsDailyAllowance.Tables.Item("branch_close_dtl")
            Dim num As Integer = 0
            Dim num2 As Integer = 0
            Dim num3 As Integer = 0
            Dim num4 As Integer = 0
            Dim num5 As Integer = 0
            Dim i As Integer
            For i = 0 To table.Rows.Count - 1
                num = (num + Integer.Parse(table.Rows.Item(i).Item("日当計").ToString))
                num2 = (num2 + Integer.Parse(table.Rows.Item(i).Item("今回日当計").ToString))
                num3 = (num3 + Integer.Parse(table.Rows.Item(i).Item("前回差分計").ToString))
                num4 = (num4 + Integer.Parse(table.Rows.Item(i).Item("昼食費計").ToString))
                num5 = (num5 + Integer.Parse(table.Rows.Item(i).Item("前回差分昼食費計").ToString))
            Next i
            Me.lblSumAllDailyPay.Text = num.ToString("N0")
            Me.lblSumDailyPay.Text = num2.ToString("N0")
            Me.lblSumBeforeDailyPay.Text = num3.ToString("N0")
            Me.lblSumFoodExpenses.Text = num4.ToString("N0")
            Me.lblSumBeforeFoodExpenses.Text = num5.ToString("N0")

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub SetFlexGridStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("print_check", 20)
            dicColWidthPair.Add("社員番号", 65)
            dicColWidthPair.Add("氏名", 120)
            dicColWidthPair.Add("資格", 50)
            dicColWidthPair.Add("機種", 50)
            dicColWidthPair.Add("支部", 50)
            dicColWidthPair.Add("今回日当計", 90)
            dicColWidthPair.Add("昼食費計", 90)
            dicColWidthPair.Add("前回差分計", 90)
            dicColWidthPair.Add("前回差分昼食費計", 120)
            dicColWidthPair.Add("日当計", 90)
            Me.flxDailyPayClose.Enabled = True
            Me.flxDailyPayClose.Row = 1
            Me.flxDailyPayClose.AllowSorting = AllowSortingEnum.SingleColumn
            Me.flxDailyPayClose.AllowEditing = True
            Me.flxDailyPayClose.AllowResizing = AllowResizingEnum.Columns
            Dim i As Integer
            For i = 0 To Me.flxDailyPayClose.Cols.Count - 1
                Me.flxDailyPayClose.Cols.Item(i).AllowEditing = False
                'Me.flxDailyPayClose.Cols.Item(i).AllowSorting = False
            Next i
            Me.flxDailyPayClose.Cols.Item("print_check").Caption = ""
            Me.flxDailyPayClose.Cols.Item("print_check").AllowEditing = True
            Me.flxDailyPayClose.Cols.Item("print_check").AllowSorting = False
            Me.flxDailyPayClose.Cols(1).DataType = GetType(Boolean)
            Me.flxDailyPayClose.Cols.Item("今回日当計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("昼食費計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("前回差分計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("前回差分昼食費計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("日当計").Format = "N0"
            'Me.flxDailyPayClose.Cols.Item("社員番号").AllowSorting = True
            'Me.flxDailyPayClose.Cols.Item("機種").AllowSorting = True
            SetColsWidth(Me.flxDailyPayClose, dicColWidthPair)
            'TODO Me.flxDailyPayClose.SetColsWidth(dicColWidthPair)
            'TODO Me.flxDailyPayClose.AdjustTextAlign()

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub UC050202_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.flxDailyPayClose.Redraw = False
            FrmWaitInfo.ShowWaitForm(Nothing)
            If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                Me.btnDailyPayPrint.Enabled = False
            Else
                Me.btnDailyPayPrint.Enabled = True
            End If
            Me.InitData()
            Me.SetFlexGridStyle()

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return
            End If
            FrmWaitInfo.CloseWaitForm()
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        Finally
            Me.flxDailyPayClose.Redraw = True
            FrmWaitInfo.CloseWaitForm()
        End Try
    End Sub

    Private Sub btnAllCheckOn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllCheckOn.Click
        Try
            If (Not Me.flxDailyPayClose.DataSource Is Nothing) Then
                Dim i As Integer
                For i = 1 To Me.flxDailyPayClose.Rows.Count - 1
                    Me.flxDailyPayClose.Item(i, 1) = True
                Next i
            End If

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub btnAllCheckOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllCheckOff.Click
        Try
            If (Not Me.flxDailyPayClose.DataSource Is Nothing) Then
                Dim i As Integer
                For i = 1 To Me.flxDailyPayClose.Rows.Count - 1
                    Me.flxDailyPayClose.Item(i, 1) = False
                Next i
            End If

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub btnDailyPayPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDailyPayPrint.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim printData As New SelectedAllowancePrintData
            Dim num As Integer = 0
            Dim i As Integer
            For i = 1 To Me.flxDailyPayClose.Rows.Count - 1
                If CBool(Me.flxDailyPayClose.Rows.Item(i).Item("print_check")) Then
                    num += 1
                End If
            Next i
            printData.noCheckedRec = New Integer(num - 1) {}
            num = 0
            Dim j As Integer
            For j = 1 To Me.flxDailyPayClose.Rows.Count - 1
                If CBool(Me.flxDailyPayClose.Rows.Item(j).Item("print_check")) Then
                    printData.noCheckedRec(num) = (j - 1)
                    num = num + 1
                End If
            Next j
            If (printData.noCheckedRec.Length < 1) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0088", New String() {"印刷"})
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim strBelongingName As String = Me.dsDailyAllowance.Tables.Item("branch_belong_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("支部").ToString
            'Dim viewer As New ReportViewer(command.GetCommitteeCloseDtlPrint(Me.dsDailyAllowance.Tables.Item("branch_close_dtl"), printData, strBelongingName, Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabPageIdx).Item("c_constant_seq").ToString, (Me.lblCloseYear.Text.PadLeft(4, "0"c) & Me.lblCloseMonth.Text.PadLeft(2, "0"c) & DateTime.DaysInMonth(Integer.Parse(Me.lblCloseYear.Text), Integer.Parse(Me.lblCloseMonth.Text)))), "Report.CommitteeDailyAllowance.RptCommitteeDailyAllowance")
            Dim viewer As New ReportViewer(command.GetCommitteeCloseDtlPrint(Me.dsDailyAllowance.Tables.Item("branch_close_dtl"), printData, strBelongingName, "02", (Me.lblCloseYear.Text.PadLeft(4, "0"c) & Me.lblCloseMonth.Text.PadLeft(2, "0"c) & DateTime.DaysInMonth(Integer.Parse(Me.lblCloseYear.Text), Integer.Parse(Me.lblCloseMonth.Text)))), New CR0502P1) 'TODO
            viewer.ReportViewerShow()

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            'Utilities.RestoreUserControl()
            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls("pnlMain")
            uc = pn.Controls("UC050201")

            If uc Is Nothing Then
                uc = New UC050201

                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If
            Me.Dispose()

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Public Sub SetColsWidth(ByRef flxList As C1FlexGrid, ByVal dicColWidthPair As Dictionary(Of String, Integer))
        Try
            Dim i As Integer
            For i = 0 To flxList.Cols.Count - 1
                flxList.Cols.Item(i).Visible = False
            Next i
            Dim str As String
            For Each str In dicColWidthPair.Keys
                If (dicColWidthPair.Item(str) = 0) Then
                    flxList.Cols.Item(str).Visible = False
                Else
                    flxList.Cols.Item(str).Visible = True
                    flxList.Cols.Item(str).Width = dicColWidthPair.Item(str)
                End If
            Next

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private dsDailyAllowance As DataSet
    Private OwnerInstance As UC050201
    Private Const REPORT_COMMITTEEDAILYALLOWANCE As String = "Report.CommitteeDailyAllowance.RptCommitteeDailyAllowance"
    Private tabPageIdx As Integer

    Private Sub flxDailyPayClose_BeforeSort(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.SortColEventArgs) Handles flxDailyPayClose.BeforeSort
        Dim strSortKey As String
        If Not e Is Nothing Then
            strSortKey = Me.flxDailyPayClose.Cols(e.Col).Caption
            If Not strSortKey.Contains("社員番号") Then
                Me.flxDailyPayClose.Cols(e.Col).Sort = e.Order
                Me.flxDailyPayClose.Cols.Item("社員番号").Sort = SortFlags.Ascending
                Me.flxDailyPayClose.Sort(SortFlags.UseColSort, e.Col)
            End If
        End If
    End Sub

    Private Sub flxDailyPayClose_AfterSort(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.SortColEventArgs) Handles flxDailyPayClose.AfterSort
        Dim strSortOrder As String = ""
        Dim strSortKey As String
        If Not e Is Nothing Then
            strSortKey = Me.flxDailyPayClose.Cols(e.Col).Caption
            If Not strSortKey Is Nothing Then
                strSortOrder += strSortKey
            End If
            If e.Order = SortFlags.Descending Then
                strSortOrder += " DESC"
            Else
                strSortOrder += " ASC"
            End If
            If Not strSortKey.Contains("社員番号") Then
                strSortOrder += ",社員番号 ASC"
            End If
        End If

        Dim tb As DataTable = Me.dsDailyAllowance.Tables.Item("branch_close_dtl")
        If Not tb Is Nothing Then
            Dim rows As DataRow() = tb.Select(Nothing, strSortOrder).Clone()
            Dim ta As New DataTable()
            ta = tb.Clone()
            For Each row As DataRow In rows
                ta.ImportRow(row)
            Next
            Me.dsDailyAllowance.Tables.Remove("branch_close_dtl")
            ta.TableName = "branch_close_dtl"
            Me.dsDailyAllowance.Tables.Add(ta)
        End If
    End Sub
End Class
