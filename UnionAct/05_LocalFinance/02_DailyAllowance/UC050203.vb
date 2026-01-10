Imports UnionAct.Framework.UnionException
Imports System.Reflection
Imports UnionAct.GUI.Common
Imports UnionAct.Business.FinancialAffairs.DailyAllowance
Imports UnionAct.Business.Common
Imports UnionAct.Framework.Command
Imports UnionAct.GUI.Document
Imports C1.Win.C1FlexGrid
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLMsg

Public Class UC050203
    'キャンセルボタン
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
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
    End Sub

    Public Sub New(ByVal OwnerInstance As UC050201, ByVal tabPageIdx As Integer)
        Me.InitializeComponent()
        Me.OwnerInstance = OwnerInstance
        Me.dsDailyAllowance = OwnerInstance.dsDailyAllowance.Copy
        Me.tabPageIdx = tabPageIdx
        Me.infConst = New InfoConstant(PublicCommand.GetSystemDate)
    End Sub

    Private Sub btnAllCheckOff_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAllCheckOff.Click
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

    Private Sub btnAllCheckOn_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAllCheckOn.Click
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

    Private Sub btnExecutivePayReportPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExecutivePayReportPrint.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim printData As New SelectedAllowancePrintData
            If (Me.flxDailyPayClose.Row < 0) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0088", New String() {"印刷"})
            End If
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
                    'printData.noCheckedRec(num) = Me.flxDailyPayClose.Rows.Item(j).SafeIndex - 1
                    num = num + 1
                End If
            Next j
            If (printData.noCheckedRec.Length < 1) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0088", New String() {"印刷"})
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            'Dim viewer As New ReportViewer(command.GetExecutiveCloseDtlPrint(Me.dsDailyAllowance.Tables.Item("executive_close_dtl"), printData, Me.cmbBelonging.SelectedValue, Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabPageIdx).Item("c_constant_seq").ToString, (Me.lblCloseYear.Text.PadLeft(4, "0"c) & Me.lblCloseMonth.Text.PadLeft(2, "0"c) & DateTime.DaysInMonth(Integer.Parse(Me.lblCloseYear.Text), Integer.Parse(Me.lblCloseMonth.Text)).ToString.PadLeft(2, "0"c)), (Me.lblYear.Text.PadLeft(4, "0"c) & Me.lblMonth.Text.PadLeft(2, "0"c) & "01")), New CR0502P4)
            Dim viewer As New ReportViewer(command.GetExecutiveCloseDtlPrint(Me.dsDailyAllowance.Tables.Item("executive_close_dtl"), printData, Me.cmbBelonging.SelectedValue, "03", (Me.lblCloseYear.Text.PadLeft(4, "0"c) & Me.lblCloseMonth.Text.PadLeft(2, "0"c) & DateTime.DaysInMonth(Integer.Parse(Me.lblCloseYear.Text), Integer.Parse(Me.lblCloseMonth.Text)).ToString.PadLeft(2, "0"c)), (Me.lblYear.Text.PadLeft(4, "0"c) & Me.lblMonth.Text.PadLeft(2, "0"c) & "01")), New CR0502P4)
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

    Private Sub btnShow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnShow.Click
        Cursor.Current = Cursors.WaitCursor
        Me.flxDailyPayClose.Redraw = False
        Try
            FrmWaitInfo.ShowWaitForm(Nothing)
            'Me.ValidateFields() TODO
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim time As DateTime = DateTime.ParseExact(Me.dsDailyAllowance.Tables.Item("executive_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
            Dim strCloseDate As String = (time.Year.ToString.PadLeft(4, "0"c) & time.Month.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(time.Year, time.Month).ToString.PadLeft(2, "0"c))
            Dim strDate As String = CDate(Me.dsDailyAllowance.Tables.Item("executive_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("d_years")).ToString("yyyyMMdd")
            'Dim table As DataTable = command.GetExecutiveCloseDtl(Me.cmbBelonging.GetSelectedItem("c_constant_seq"), Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabPageIdx).Item("c_constant_seq").ToString, strCloseDate, strDate).Tables.Item("executive_close_dtl").Copy
            Dim strBelongingId As String
            If IsDBNull(Me.cmbBelonging.SelectedValue) Then
                strBelongingId = ""
            Else
                strBelongingId = Me.cmbBelonging.SelectedValue
            End If
            Dim table As DataTable = command.GetExecutiveCloseDtl(Me.cmbBelonging.SelectedValue, "03", strCloseDate, strDate).Tables.Item("executive_close_dtl").Copy 'TODO
            If Me.dsDailyAllowance.Tables.Contains("executive_close_dtl") Then
                Me.dsDailyAllowance.Tables.Remove("executive_close_dtl")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxDailyPayClose.DataSource = Me.dsDailyAllowance.Tables.Item("executive_close_dtl")
            Me.SetFlexGridStyle()
            Dim num As Integer = 0
            Dim num2 As Integer = 0
            Dim num3 As Integer = 0
            Dim num4 As Integer = 0
            Dim num5 As Integer = 0
            Dim i As Integer
            For i = 0 To table.Rows.Count - 1
                num = (num + Integer.Parse(table.Rows.Item(i).Item("日当計").ToString))
                num2 = (num2 + Integer.Parse(table.Rows.Item(i).Item("当月日当計").ToString))
                num3 = (num3 + Integer.Parse(table.Rows.Item(i).Item("前回差分計").ToString))
                num4 = (num4 + Integer.Parse(table.Rows.Item(i).Item("中執昼食費計").ToString))
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
            FrmWaitInfo.CloseWaitForm()
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        Finally
            Me.flxDailyPayClose.Redraw = True
            FrmWaitInfo.CloseWaitForm()
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub cmbBelonging_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If PublicCommand.PressedEnterKey(e) Then
            Me.btnShow_Click(Me.btnShow, EventArgs.Empty)
        End If
    End Sub

    Private Sub cmbBelonging_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If (Not Me.flxDailyPayClose.DataSource Is Nothing) Then
                DirectCast(Me.flxDailyPayClose.DataSource, DataTable).Rows.Clear()
            End If
            Me.lblSumDailyPay.Text = "0"
            Me.lblSumBeforeDailyPay.Text = "0"
            Me.lblSumFoodExpenses.Text = "0"
            Me.lblSumBeforeFoodExpenses.Text = "0"
            Me.lblSumAllDailyPay.Text = "0"

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

    Private Sub UC050203_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                Me.btnExecutivePayReportPrint.Enabled = False
            Else
                Me.btnExecutivePayReportPrint.Enabled = True
            End If
            Me.InitControl()

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

    Private Sub InitControl()
        Dim clsMdb As New CLAccessMdb
        Try
            clsMdb.Connect()
            NSMDCommon.CreateCboConstantDtl(clsMdb, cmbBelonging, "BELONGING", False)
            'Me.cmbBelonging.SetItems(Me.infConst)
            If (Me.cmbBelonging.Items.Count > 0) Then
                Me.cmbBelonging.SelectedIndex = 0
            End If
            Dim time As DateTime = DateTime.ParseExact(Me.dsDailyAllowance.Tables.Item("executive_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
            Dim text1 As String = (time.Year.ToString.PadLeft(4, "0"c) & time.Month.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(time.Year, time.Month).ToString.PadLeft(2, "0"c))
            Dim time2 As DateTime = CDate(Me.dsDailyAllowance.Tables.Item("executive_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("d_years"))
            Me.lblCloseYear.Text = time.Year.ToString.PadLeft(4, "0"c)
            Me.lblCloseMonth.Text = time.Month.ToString.PadLeft(2, "0"c)
            Me.lblYear.Text = time2.Year.ToString.PadLeft(4, "0"c)
            Me.lblMonth.Text = time2.Month.ToString.PadLeft(2, "0"c)
            If (Me.cmbBelonging.SelectedIndex >= 0) Then
                Me.btnShow_Click(Me.btnShow, EventArgs.Empty)
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
        Finally
            clsMdb.Disconnect()
        End Try
    End Sub

    Private Sub SetFlexGridStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("print_check", 20)
            dicColWidthPair.Add("社員番号", 90)
            dicColWidthPair.Add("氏名", &H87)
            dicColWidthPair.Add("資格", 60)
            dicColWidthPair.Add("機種", 60)
            dicColWidthPair.Add("当月日当計", &H73)
            dicColWidthPair.Add("中執昼食費計", &H73)
            dicColWidthPair.Add("前回差分計", &H73)
            dicColWidthPair.Add("前回差分昼食費計", &H9B)
            dicColWidthPair.Add("日当計", &H73)
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
            Me.flxDailyPayClose.Cols.Item("当月日当計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("中執昼食費計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("前回差分計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("前回差分昼食費計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("日当計").Format = "N0"
            'Me.flxDailyPayClose.Cols.Item("社員番号").AllowSorting = True
            'Me.flxDailyPayClose.ShowSortAt(SortFlags.Ascending, 2)
            'Me.flxDailyPayClose.Cols.Item("機種").AllowSorting = True
            SetColsWidth(Me.flxDailyPayClose, dicColWidthPair)
            'Me.flxDailyPayClose.AdjustTextAlign()
            'Me.flxDailyPayClose.SetColsWidth(dicColWidthPair)

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

    'Public Function GetSelectedItem(ByVal strColumnName As String) As String
    '    Dim str As String
    '    Try
    '        If (Me.dTableComboBox Is Nothing) Then
    '            Return Nothing
    '        End If
    '        If (Me.dTableComboBox.Rows.Count <= Me.SelectedIndex) Then
    '            Return Nothing
    '        End If
    '        If (Me.SelectedIndex < 0) Then
    '            Return Nothing
    '        End If
    '        str = Me.dTableComboBox.Rows.Item(Me.SelectedIndex).Item(strColumnName).ToString
    '    Catch exception As AppUnionException
    '        exception.AddMethodName(MethodBase.GetCurrentMethod)
    '        Throw exception
    '    Catch exception2 As SysUnionException
    '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '        Throw exception2
    '    Catch exception3 As Exception
    '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
    '    End Try
    '    Return str
    'End Function

    Private dsDailyAllowance As DataSet
    Private OwnerInstance As UC050201
    Private Const REPORT_CENTRALEXECUTIVEACTIVITYDA As String = "Report.CentralExecutiveActivityDA.RptCentralExecutiveActivityDA"
    Private tabPageIdx As Integer
    Private infConst As InfoConstant

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

        Dim tb As DataTable = Me.dsDailyAllowance.Tables.Item("executive_close_dtl")
        If Not tb Is Nothing Then
            Dim rows As DataRow() = tb.Select(Nothing, strSortOrder).Clone()
            Dim ta As New DataTable()
            ta = tb.Clone()
            For Each row As DataRow In rows
                ta.ImportRow(row)
            Next
            Me.dsDailyAllowance.Tables.Remove("executive_close_dtl")
            ta.TableName = "executive_close_dtl"
            Me.dsDailyAllowance.Tables.Add(ta)
        End If
    End Sub
End Class
