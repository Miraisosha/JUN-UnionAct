Imports System.Reflection
Imports UnionAct.Business.FinancialAffairs
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.Business.FinancialAffairs.DailyAllowance
Imports UnionAct.Business.Common
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.GUI.FinancialAffairs
Imports UnionAct.GUI.Document
Imports C1.Win.C1FlexGrid
Imports UnionAct.GUI.Common

Public Class UC050201
    Inherits FinancialAffairsBase
    Private Sub InitControls()
        Try
            Dim str2 As String
            Dim str3 As String
            Dim strKeyDate = MDMasterCommon.GetKeyDate
            Dim command As New DailyAllowanceCloseListCommand
            Me.dsDailyAllowance.Tables.Add(command.GetDailyPayKindAll(strKeyDate).Tables.Item("constant_dtl").Copy)
            Dim table As DataTable = Me.dsDailyAllowance.Tables.Item("constant_dtl")
            Dim rowArray As DataRow() = table.Select("c_constant_seq = '01'")
            If (rowArray.Length > 0) Then
                Me.tabDailyAllowanceClose.TabPages.Item(0).Text = rowArray(0).Item("l_name").ToString
            End If
            rowArray = table.Select("c_constant_seq = '02'")
            If (rowArray.Length > 0) Then
                Me.tabDailyAllowanceClose.TabPages.Item(1).Text = rowArray(0).Item("l_name").ToString
            End If
            rowArray = table.Select("c_constant_seq = '03'")
            If (rowArray.Length > 0) Then
                Me.tabDailyAllowanceClose.TabPages.Item(2).Text = rowArray(0).Item("l_name").ToString
            End If
            rowArray = table.Select("c_constant_seq = '04'")
            If (rowArray.Length > 0) Then
                Me.tabDailyAllowanceClose.TabPages.Item(3).Text = rowArray(0).Item("l_name").ToString
            End If
            Me.tabDailyAllowanceClose.SelectedIndex = 0
            Me.tabSelectedIdx = Me.tabDailyAllowanceClose.SelectedIndex
            Dim time As DateTime = DateTime.ParseExact(MDLoginInfo.PeriodFrom, "yyyyMMdd", Nothing)
            Dim time2 As DateTime = DateTime.ParseExact(MDLoginInfo.PeriodTo, "yyyyMMdd", Nothing)
            Dim i As Integer = time.Year
            Do While (i <= time2.Year)
                Me.cmbCommitteeYearSearch.Items.Add(i.ToString)
                Me.cmbCommitteeCloseYear.Items.Add(i.ToString)
                Me.cmbDgmYearSearch.Items.Add(i.ToString)
                Me.cmbDgmCloseYear.Items.Add(i.ToString)
                Me.cmbBranchYearSearch.Items.Add(i.ToString)
                Me.cmbBranchCloseYear.Items.Add(i.ToString)
                Me.cmbExecutiveYearSearch.Items.Add(i.ToString)
                Me.cmbExecutiveCloseYear.Items.Add(i.ToString)
                i += 1
            Loop
            Me.cmbCommitteeYearSearch.SelectedItem = strKeyDate.Substring(0, 4)
            Me.cmbDgmYearSearch.SelectedItem = strKeyDate.Substring(0, 4)
            Me.cmbBranchYearSearch.SelectedItem = strKeyDate.Substring(0, 4)
            Me.cmbExecutiveYearSearch.SelectedItem = strKeyDate.Substring(0, 4)
            Me.cmbCommitteeCloseYear.SelectedItem = strKeyDate.Substring(0, 4)
            Me.cmbCommitteeCloseMonth.SelectedItem = strKeyDate.Substring(4, 2)
            Me.cmbDgmCloseYear.SelectedItem = strKeyDate.Substring(0, 4)
            Me.cmbDgmCloseMonth.SelectedItem = strKeyDate.Substring(4, 2)
            Dim time3 As DateTime = DateTime.ParseExact(Me.GetMaxDailyPayClose("02"), "yyyyMMdd", Nothing).AddDays(1)
            If (time3 > time2) Then
                str2 = time2.ToString("yyyyMMdd")
            Else
                str2 = time3.ToString("yyyyMMdd")
            End If
            Me.cmbBranchCloseYear.SelectedItem = str2.Substring(0, 4)
            Me.cmbBranchCloseMonth.SelectedItem = str2.Substring(4, 2)
            Dim time4 As DateTime = DateTime.ParseExact(Me.GetMaxDailyPayClose("03"), "yyyyMMdd", Nothing).AddDays(1)
            If (time4 > time2) Then
                str3 = time2.ToString("yyyyMMdd")
            Else
                str3 = time4.ToString("yyyyMMdd")
            End If
            Me.cmbExecutiveCloseYear.SelectedItem = str3.Substring(0, 4)
            Me.cmbExecutiveCloseMonth.SelectedItem = str3.Substring(4, 2)

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

    Private Sub UC050201_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not MDFinanceCommon.GetEntryPower("UC050201") Then
                Utilities.SetVisibleProperty(False, New Control() {Me.grpCommitteeCloseRenew})
                Utilities.SetVisibleProperty(False, New Control() {Me.grpDgmCloseRenew})
                Utilities.SetVisibleProperty(False, New Control() {Me.grpBranchCloseRenew})
                Utilities.SetVisibleProperty(False, New Control() {Me.grpExecutiveCloseRenew})
            End If
            If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                Me.btnCommitteeBelongPrint.Enabled = False
                Me.btnDgmBelongPrint.Enabled = False
                Me.btnBranchBelongPrint.Enabled = False
                Me.btnExecutiveMonthPrint.Enabled = False
            End If
            Utilities.ControlEdit(False, Me.grpCommitteeCloseList)
            Utilities.ControlEdit(False, Me.grpDgmCloseList)
            Utilities.ControlEdit(False, Me.grpBranchCloseList)
            Utilities.ControlEdit(False, Me.grpExecutiveCloseList)
            Utilities.ControlEdit(False, Me.grpDgmBelongCloseList)
            Me.InitControls()

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

    Private Sub btnCommitteeSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommitteeSearch.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.cmbCommitteeYearSearch.SelectedIndex < 0) Then
                Me.cmbCommitteeYearSearch.BackColor = Color.LightPink
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"検索条件・締め日「年」"})
                CLMsg.Show("GE0006", "検索条件・締め日「年」")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim strDate As String = (Me.cmbCommitteeYearSearch.SelectedItem.ToString & If((Me.cmbCommitteeMonthSearch.SelectedIndex < 0), "", Me.cmbCommitteeMonthSearch.SelectedItem.ToString))
            Dim table As DataTable = command.GetCommitteeCloseList("01", strDate).Tables.Item("committee_close_list").Copy
            'TODO Dim table As DataTable = command.GetCommitteeCloseList(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate).Tables.Item("committee_close_list").Copy
            If Me.dsDailyAllowance.Tables.Contains("committee_close_list") Then
                Me.dsDailyAllowance.Tables.Remove("committee_close_list")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxCommitteeCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("committee_close_list")
            Utilities.ControlEdit(True, Me.grpCommitteeCloseList)
            If (Not Me.flxCommitteeBelongCloseList.DataSource Is Nothing) Then
                DirectCast(Me.flxCommitteeBelongCloseList.DataSource, DataTable).Rows.Clear()
            End If
            Utilities.ControlEdit(False, Me.grpCommitteeBelongCloseList)
            Me.SetCommitteeCloseListStyle()

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

    Private Sub btnCommitteeBelongShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommitteeBelongShow.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxCommitteeCloseList.Row < 0) Then
                Me.cmbCommitteeYearSearch.BackColor = Color.LightPink
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧", "支部別の表示"})
                CLMsg.Show("GE0070", "締め日一覧", "支部別の表示")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim time As DateTime = DateTime.ParseExact(Me.dsDailyAllowance.Tables.Item("committee_close_list").Rows.Item((Me.flxCommitteeCloseList.Row - 1)).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
            Dim strCloseDate As String = (time.Year.ToString.PadLeft(4, "0"c) & time.Month.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(time.Year, time.Month).ToString.PadLeft(2, "0"c))
            'TODO Dim table As DataTable = command.GetCommitteeBelongCloseList(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strCloseDate).Tables.Item("committee_belong_close_list").Copy
            Dim table As DataTable = command.GetCommitteeBelongCloseList("01", strCloseDate).Tables.Item("committee_belong_close_list").Copy
            If Me.dsDailyAllowance.Tables.Contains("committee_belong_close_list") Then
                Me.dsDailyAllowance.Tables.Remove("committee_belong_close_list")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxCommitteeBelongCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("committee_belong_close_list")
            Utilities.ControlEdit(True, Me.grpCommitteeBelongCloseList)
            If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                Me.btnCommitteeBelongPrint.Enabled = False
            End If
            Me.SetCommitteeBelongCloseListStyle()

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

    Private Sub btnCommitteeBelongPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommitteeBelongPrint.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxCommitteeCloseList.Row < 1) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧・支部別", "委員会別印刷"})
                CLMsg.Show("GE0070", "締め日一覧・支部別", "委員会別印刷")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            'Dim viewer As New ReportViewer(command.GetCommitteeBelongCloseListPrint(Me.dsDailyAllowance.Tables.Item("committee_belong_close_list"), (Me.flxCommitteeBelongCloseList.Row - 1), Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString), "Report.CommitteeDailyAllowance.RptLocalTotal")
            Dim viewer As New ReportViewer(command.GetCommitteeBelongCloseListPrint(Me.dsDailyAllowance.Tables.Item("committee_belong_close_list"), (Me.flxCommitteeBelongCloseList.Row - 1), "01"), New CR0502P2)
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

    Private Sub btnCommitteeRefIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommitteeRefIn.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxCommitteeCloseList.Row < 1) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧・支部別", "委員日当詳細の表示"})
                CLMsg.Show("GE0070", "締め日一覧・支部別", "委員日当詳細の表示")
                Return
            End If
            Dim num As Integer = Integer.Parse(Me.flxCommitteeBelongCloseList.Item(Me.flxCommitteeBelongCloseList.Row, "今回日当計").ToString)
            Dim num2 As Integer = Integer.Parse(Me.flxCommitteeBelongCloseList.Item(Me.flxCommitteeBelongCloseList.Row, "前回差分計").ToString)
            If ((num = 0) AndAlso (num2 = 0)) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GI0027", New String(0 - 1) {})
                CLMsg.Show("GI0027")
                Return
            End If
            Me.grdSelectedIdx = (Me.flxCommitteeBelongCloseList.Row - 1)

            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls("pnlMain")
            uc = pn.Controls("UC050202")

            If uc Is Nothing Then
                uc = New UC050202(Me, Me.grdSelectedIdx)
                uc.Controls("label11").Text = "委員日当計算 - 詳細"
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
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
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    '部／委員会日当タブ　委員日当詳細ボタン
    Private Sub btnCommitteeRenew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommitteeRenew.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim exception As AppUnionException = Nothing
            If (Me.cmbCommitteeCloseYear.SelectedIndex < 0) Then
                Me.cmbCommitteeCloseYear.BackColor = Color.LightPink
                If (exception Is Nothing) Then
                    'exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"締め日設定「年」"})
                    CLMsg.Show("GE0006", "締め日設定「年」")
                    Return
                Else
                    'exception.AddExceptionData("GE0006", New String() {"締め日設定「年」"})
                    CLMsg.Show("GE0006", "締め日設定「年」")
                    Return
                End If
            End If
            If (Me.cmbCommitteeCloseMonth.SelectedIndex < 0) Then
                Me.cmbCommitteeCloseMonth.BackColor = Color.LightPink
                If (exception Is Nothing) Then
                    'exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"締め日設定「月」"})
                    CLMsg.Show("GE0006", "締め日設定「月」")
                    Return
                Else
                    'exception.AddExceptionData("GE0006", New String() {"締め日設定「月」"})
                    CLMsg.Show("GE0006", "締め日設定「月」")
                    Return
                End If
            End If
            If (Not exception Is Nothing) Then
                Throw exception
            End If
            Dim time As New DateTime(Integer.Parse(Me.cmbCommitteeCloseYear.SelectedItem.ToString), Integer.Parse(Me.cmbCommitteeCloseMonth.SelectedItem.ToString), 1)
            Dim today As DateTime = PublicCommand.GetToday
            If (time > today) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0185", New String(0 - 1) {})
                CLMsg.Show("GE0185")
                Return
            End If
            FinancialAffairsUtility.CheckNetBankDataHasMade((Me.cmbCommitteeCloseYear.Text & Me.cmbCommitteeCloseMonth.Text), "01")
            Dim strDate As String = (Me.cmbCommitteeCloseYear.SelectedItem.ToString.PadLeft(4, "0"c) & Me.cmbCommitteeCloseMonth.SelectedItem.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(Integer.Parse(Me.cmbCommitteeCloseYear.SelectedItem.ToString), Integer.Parse(Me.cmbCommitteeCloseMonth.SelectedItem.ToString)).ToString.PadLeft(2, "0"c))
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim maxDailyPayClose As String = Me.GetMaxDailyPayClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString)
            If (strDate.CompareTo(maxDailyPayClose) = 0) Then
                'If (MyBase.UnionMessage.ShowMessage(False, "GQ0032", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2)}) <> DialogResult.Yes) Then
                If CLMsg.Show("GQ0032", strDate.Substring(0, 4), strDate.Substring(4, 2)) <> DialogResult.Yes Then
                    Return
                End If
                Try
                    If CLMsg.Show("GQ0063") <> DialogResult.Yes Then
                        'If (MyBase.UnionMessage.ShowMessage(False, "GQ0063", New String(0 - 1) {}) <> DialogResult.Yes) Then
                        Return
                    End If
                    FrmWaitInfo.ShowWaitForm(Nothing)
                    command.CalcDailyAllowanceClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, True)
                Finally
                    FrmWaitInfo.CloseWaitForm()
                End Try
            Else
                If (strDate.CompareTo(maxDailyPayClose) > 0) Then
                    command.CheckAttendanceEntry(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, maxDailyPayClose)
                    If CLMsg.Show("GQ0030", strDate.Substring(0, 4), strDate.Substring(4, 2), "委員会") <> DialogResult.Yes Then
                        'If (MyBase.UnionMessage.ShowMessage(False, "GQ0030", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2), "委員会"}) <> DialogResult.Yes) Then
                        Return
                    End If
                    Try
                        If CLMsg.Show("GQ0063") <> DialogResult.Yes Then
                            'If (MyBase.UnionMessage.ShowMessage(False, "GQ0063", New String(0 - 1) {}) <> DialogResult.Yes) Then
                            Return
                        End If
                        FrmWaitInfo.ShowWaitForm(Nothing)
                        command.CalcDailyAllowanceClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, False)
                        GoTo Label_0431
                    Finally
                        FrmWaitInfo.CloseWaitForm()
                    End Try
                End If
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0069", New String() {DateTime.ParseExact(maxDailyPayClose, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月")})
                CLMsg.Show("GE0069", DateTime.ParseExact(maxDailyPayClose, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月"))
                Return
            End If
Label_0431:
            If Me.grpCommitteeCloseList.Enabled Then
                If (Not Me.flxCommitteeCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxCommitteeCloseList.DataSource, DataTable).Rows.Clear()
                End If
                If (Not Me.flxCommitteeBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxCommitteeBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpCommitteeCloseList)
            End If
            'MessageBox.Show("GI0017", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2), "委員会"})
            CLMsg.Show("GI0017", strDate.Substring(0, 4), strDate.Substring(4, 2), "委員会")

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

    Private Function GetMaxDailyPayClose(ByVal strDailyPayKind As String) As String
        Dim prevDailyPayClose As String = ""
        Try
            Dim strCloseDate As String = (PublicCommand.GetToday.AddMonths(1).ToString("yyyyMM") & "01")
            Dim class2 As New FactoryBusClass
            'prevDailyPayClose = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand).GetPrevDailyPayClose(strDailyPayKind, strCloseDate)
            Dim dao As New DailyAllowanceCloseListCommand
            prevDailyPayClose = dao.GetPrevDailyPayClose(strDailyPayKind, strCloseDate)

        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                'exception.AddMethodName(MethodBase.GetCurrentMethod)
                CLMsg.Show("GE0001")
                Return Nothing
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
        Return prevDailyPayClose
    End Function

    Private Sub SetCommitteeCloseListStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("締め日", 110)
            dicColWidthPair.Add("対象年月" & "(" & "始" & ")", 120)
            dicColWidthPair.Add("対象年月" & "(" & "終" & ")", 120)
            dicColWidthPair.Add("日当額計", 110)
            dicColWidthPair.Add("昼食費計", 110)
            dicColWidthPair.Add("登録年月日", 110)
            dicColWidthPair.Add("担当者", 110)
            Me.flxCommitteeCloseList.Enabled = True
            Me.flxCommitteeCloseList.Row = 1
            Me.flxCommitteeCloseList.AllowSorting = AllowSortingEnum.None
            Me.flxCommitteeCloseList.AllowEditing = False
            Me.flxCommitteeCloseList.AllowResizing = AllowResizingEnum.Columns
            Me.flxCommitteeCloseList.Cols.Item("締め日").Format = "yyyy/MM/dd"
            Me.flxCommitteeCloseList.Cols.Item("日当額計").Format = "N0"
            Me.flxCommitteeCloseList.Cols.Item("昼食費計").Format = "N0"
            SetColsWidth(flxCommitteeCloseList, dicColWidthPair)
            Me.grpCommitteeCloseList.Visible = True
            Me.flxCommitteeCloseList.Visible = True
            'TODO Me.flxCommitteeCloseList.SetColsWidth(dicColWidthPair)
            'TODO Me.flxCommitteeCloseList.AdjustTextAlign()

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

        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub SetCommitteeBelongCloseListStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("締め日", 110)
            dicColWidthPair.Add("支部", 60)
            dicColWidthPair.Add("今回日当計", 100)
            dicColWidthPair.Add("昼食費計", 100)
            dicColWidthPair.Add("前回差分計", 100)
            dicColWidthPair.Add("前回差分昼食費計", 135)
            dicColWidthPair.Add("支部別日当額計", 130)
            Me.flxCommitteeBelongCloseList.Enabled = True
            Me.flxCommitteeBelongCloseList.Row = 1
            Me.flxCommitteeBelongCloseList.AllowSorting = AllowSortingEnum.None
            Me.flxCommitteeBelongCloseList.AllowEditing = False
            Me.flxCommitteeBelongCloseList.AllowResizing = AllowResizingEnum.Columns
            Me.flxCommitteeBelongCloseList.Cols.Item("締め日").Format = "yyyy/MM/dd"
            Me.flxCommitteeBelongCloseList.Cols.Item("今回日当計").Format = "N0"
            Me.flxCommitteeBelongCloseList.Cols.Item("昼食費計").Format = "N0"
            Me.flxCommitteeBelongCloseList.Cols.Item("前回差分計").Format = "N0"
            Me.flxCommitteeBelongCloseList.Cols.Item("前回差分昼食費計").Format = "N0"
            Me.flxCommitteeBelongCloseList.Cols.Item("支部別日当額計").Format = "N0"
            SetColsWidth(flxCommitteeBelongCloseList, dicColWidthPair)
            Me.grpCommitteeBelongCloseList.Visible = True
            'Me.flxCommitteeBelongCloseList.SetColsWidth(dicColWidthPair)
            'Me.flxCommitteeBelongCloseList.AdjustTextAlign()

        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Public dsDailyAllowance As New DataSet
    Public tabSelectedIdx As Integer
    Public grdSelectedIdx As Integer

    Private Sub btnBranchSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBranchSearch.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.cmbBranchYearSearch.SelectedIndex < 0) Then
                Me.cmbBranchYearSearch.BackColor = Color.LightPink
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"検索条件・締め日「年」"})
                CLMsg.Show("GE0006", "検索条件・締め日「年」")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim strDate As String = (Me.cmbBranchYearSearch.SelectedItem.ToString & If((Me.cmbBranchMonthSearch.SelectedIndex < 0), "", Me.cmbBranchMonthSearch.SelectedItem.ToString))
            'Dim table As DataTable = command.GetCommitteeCloseList(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate).Tables.Item("branch_close_list").Copy
            Dim table As DataTable = command.GetCommitteeCloseList("02", strDate).Tables.Item("branch_close_list").Copy 'TODO
            If Me.dsDailyAllowance.Tables.Contains("branch_close_list") Then
                Me.dsDailyAllowance.Tables.Remove("branch_close_list")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxBranchCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("branch_close_list")
            Utilities.ControlEdit(True, Me.grpBranchCloseList)
            If (Not Me.flxBranchBelongCloseList.DataSource Is Nothing) Then
                DirectCast(Me.flxBranchBelongCloseList.DataSource, DataTable).Rows.Clear()
            End If
            Utilities.ControlEdit(False, Me.grpBranchBelongCloseList)
            Me.SetBranchCloseListStyle()

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

    Private Sub btnBranchBelongShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBranchBelongShow.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxBranchCloseList.Row < 0) Then
                Me.cmbBranchYearSearch.BackColor = Color.LightPink
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧", "支部別の表示"})
                CLMsg.Show("GE0070", "締め日一覧", "支部別の表示")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim time As DateTime = DateTime.ParseExact(Me.dsDailyAllowance.Tables.Item("branch_close_list").Rows.Item((Me.flxBranchCloseList.Row - 1)).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
            Dim strCloseDate As String = (time.Year.ToString.PadLeft(4, "0"c) & time.Month.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(time.Year, time.Month).ToString.PadLeft(2, "0"c))
            'Dim table As DataTable = command.GetCommitteeBelongCloseList(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strCloseDate).Tables.Item("branch_belong_close_list").Copy
            Dim table As DataTable = command.GetCommitteeBelongCloseList("02", strCloseDate).Tables.Item("branch_belong_close_list").Copy 'TODO
            If Me.dsDailyAllowance.Tables.Contains("branch_belong_close_list") Then
                Me.dsDailyAllowance.Tables.Remove("branch_belong_close_list")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxBranchBelongCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("branch_belong_close_list")
            Utilities.ControlEdit(True, Me.grpBranchBelongCloseList)
            If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                Me.btnBranchBelongPrint.Enabled = False
            End If
            Me.SetBranchBelongCloseListStyle()

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

    Private Sub btnBranchBelongPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBranchBelongPrint.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxBranchCloseList.Row < 1) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧・支部別", "支部別印刷"})
                CLMsg.Show("GE0070", "締め日一覧・支部別", "支部別印刷")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            'Dim viewer As New ReportViewer(command.GetCommitteeBelongCloseListPrint(Me.dsDailyAllowance.Tables.Item("branch_belong_close_list"), (Me.flxBranchBelongCloseList.Row - 1), Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString), "Report.CommitteeDailyAllowance.RptLocalTotal")
            Dim viewer As New ReportViewer(command.GetCommitteeBelongCloseListPrint(Me.dsDailyAllowance.Tables.Item("branch_belong_close_list"), (Me.flxBranchBelongCloseList.Row - 1), "02"), New CR0502P2)
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

    Private Sub btnBranchRefIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBranchRefIn.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxBranchCloseList.Row < 1) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧・支部別", "支部三役日当詳細の表示"})
                CLMsg.Show("GE0070", "締め日一覧・支部別", "支部三役日当詳細の表示")
                Return
            End If
            Dim num As Integer = Integer.Parse(Me.flxBranchBelongCloseList.Item(Me.flxBranchBelongCloseList.Row, "今回日当計").ToString)
            Dim num2 As Integer = Integer.Parse(Me.flxBranchBelongCloseList.Item(Me.flxBranchBelongCloseList.Row, "前回差分計").ToString)
            If ((num = 0) AndAlso (num2 = 0)) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GI0027", New String(0 - 1) {})
                CLMsg.Show("GI0027")
                Return
            End If
            Me.grdSelectedIdx = (Me.flxBranchBelongCloseList.Row - 1)
            'TODO Utilities.OverlayUserControl("支部三役日当計算" & " - " & "詳細", New UserControl() {New CtlDailyAllowanceBranchDtl(Me, MyBase.objLoginSession, Me.tabSelectedIdx)})

            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls("pnlMain")
            uc = pn.Controls("UC050204")

            If uc Is Nothing Then
                uc = New UC050204(Me, Me.grdSelectedIdx)
                uc.Controls("label11").Text = "支部三役日当計算 - 詳細"
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
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

    Private Sub btnBranchRenew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBranchRenew.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim exception As AppUnionException = Nothing
            If (Me.cmbBranchCloseYear.SelectedIndex < 0) Then
                Me.cmbBranchCloseYear.BackColor = Color.LightPink
                If (exception Is Nothing) Then
                    'exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"締め日設定「年」"})
                    CLMsg.Show("GE0006", "締め日設定「年」")
                    Return
                Else
                    'exception.AddExceptionData("GE0006", New String() {"締め日設定「年」"})
                    CLMsg.Show("GE0006", "締め日設定「年」")
                    Return
                End If
            End If
            If (Me.cmbBranchCloseMonth.SelectedIndex < 0) Then
                Me.cmbBranchCloseMonth.BackColor = Color.LightPink
                If (exception Is Nothing) Then
                    'exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"締め日設定「月」"})
                    CLMsg.Show("GE0006", "締め日設定「月」")
                    Return
                Else
                    'exception.AddExceptionData("GE0006", New String() {"締め日設定「月」"})
                    CLMsg.Show("GE0006", "締め日設定「月」")
                    Return
                End If
            End If
            If (Not exception Is Nothing) Then
                Throw exception
            End If
            Dim time As New DateTime(Integer.Parse(Me.cmbBranchCloseYear.SelectedItem.ToString), Integer.Parse(Me.cmbBranchCloseMonth.SelectedItem.ToString), 1)
            Dim today As DateTime = PublicCommand.GetToday
            If (time > today) Then
                CLMsg.Show("GE0185")
                Return
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0185", New String(0 - 1) {})
            End If
            FinancialAffairsUtility.CheckNetBankDataHasMade((Me.cmbBranchCloseYear.Text & Me.cmbBranchCloseMonth.Text), "02")
            Dim strDate As String = (Me.cmbBranchCloseYear.SelectedItem.ToString.PadLeft(4, "0"c) & Me.cmbBranchCloseMonth.SelectedItem.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(Integer.Parse(Me.cmbBranchCloseYear.SelectedItem.ToString), Integer.Parse(Me.cmbBranchCloseMonth.SelectedItem.ToString)).ToString.PadLeft(2, "0"c))
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim maxDailyPayClose As String = Me.GetMaxDailyPayClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString)
            If (strDate.CompareTo(maxDailyPayClose) = 0) Then
                If CLMsg.Show("GQ0032", strDate.Substring(0, 4), strDate.Substring(4, 2)) <> DialogResult.Yes Then
                    'TODO If (MyBase.UnionMessage.ShowMessage(False, "GQ0032", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2)}) <> DialogResult.Yes) Then
                    Return
                End If
                Try
                    FrmWaitInfo.ShowWaitForm(Nothing)
                    command.CalcDailyAllowanceClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, True)
                Finally
                    FrmWaitInfo.CloseWaitForm()
                End Try
            Else
                If (strDate.CompareTo(maxDailyPayClose) > 0) Then
                    command.CheckAttendanceEntry(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, maxDailyPayClose)
                    If CLMsg.Show("GQ0030", strDate.Substring(0, 4), strDate.Substring(4, 2), "支部委員（三役）") <> DialogResult.Yes Then
                        'TODO If (MyBase.UnionMessage.ShowMessage(False, "GQ0030", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2), "支部委員（三役）"}) <> DialogResult.Yes) Then
                        Return
                    End If
                    Try
                        FrmWaitInfo.ShowWaitForm(Nothing)
                        command.CalcDailyAllowanceClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, False)
                        GoTo Label_03F3
                    Finally
                        FrmWaitInfo.CloseWaitForm()
                    End Try
                End If
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0069", New String() {DateTime.ParseExact(maxDailyPayClose, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月")})
                CLMsg.Show("GE0069", DateTime.ParseExact(maxDailyPayClose, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月"))
                Return
            End If
Label_03F3:
            If Me.grpBranchCloseList.Enabled Then
                If (Not Me.flxBranchCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxBranchCloseList.DataSource, DataTable).Rows.Clear()
                End If
                If (Not Me.flxBranchBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxBranchBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpBranchCloseList)
            End If
            CLMsg.Show("GI0017", strDate.Substring(0, 4), strDate.Substring(4, 2), "支部委員（三役）")
            'TODO MyBase.UnionMessage.ShowMessage("GI0017", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2), "支部委員（三役）"})

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

    Private Sub SetBranchCloseListStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("締め日", 110)
            dicColWidthPair.Add("対象年月" & "(" & "始" & ")", 120)
            dicColWidthPair.Add("対象年月" & "(" & "終" & ")", 120)
            dicColWidthPair.Add("日当額計", 110)
            dicColWidthPair.Add("昼食費計", 110)
            dicColWidthPair.Add("登録年月日", 110)
            dicColWidthPair.Add("担当者", 110)
            Me.flxBranchCloseList.Enabled = True
            Me.flxBranchCloseList.Row = 1
            Me.flxBranchCloseList.AllowSorting = AllowSortingEnum.None
            Me.flxBranchCloseList.AllowEditing = False
            Me.flxBranchCloseList.AllowResizing = AllowResizingEnum.Columns
            Me.flxBranchCloseList.Cols.Item("締め日").Format = "yyyy/MM/dd"
            Me.flxBranchCloseList.Cols.Item("日当額計").Format = "N0"
            Me.flxBranchCloseList.Cols.Item("昼食費計").Format = "N0"
            SetColsWidth(flxBranchCloseList, dicColWidthPair)
            Me.grpBranchCloseList.Visible = True
            Me.flxBranchCloseList.Visible = True
            'TODO Me.flxCommitteeCloseList.SetColsWidth(dicColWidthPair)
            'TODO Me.flxCommitteeCloseList.AdjustTextAlign()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub SetBranchBelongCloseListStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("締め日", 110)
            dicColWidthPair.Add("支部", 60)
            dicColWidthPair.Add("今回日当計", 100)
            dicColWidthPair.Add("昼食費計", 100)
            dicColWidthPair.Add("前回差分計", 100)
            dicColWidthPair.Add("前回差分昼食費計", 135)
            dicColWidthPair.Add("支部別日当額計", 130)
            Me.flxBranchBelongCloseList.Enabled = True
            Me.flxBranchBelongCloseList.Row = 1
            Me.flxBranchBelongCloseList.AllowSorting = AllowSortingEnum.None
            Me.flxBranchBelongCloseList.AllowEditing = False
            Me.flxBranchBelongCloseList.AllowResizing = AllowResizingEnum.Columns
            Me.flxBranchBelongCloseList.Cols.Item("締め日").Format = "yyyy/MM/dd"
            Me.flxBranchBelongCloseList.Cols.Item("今回日当計").Format = "N0"
            Me.flxBranchBelongCloseList.Cols.Item("昼食費計").Format = "N0"
            Me.flxBranchBelongCloseList.Cols.Item("前回差分計").Format = "N0"
            Me.flxBranchBelongCloseList.Cols.Item("前回差分昼食費計").Format = "N0"
            Me.flxBranchBelongCloseList.Cols.Item("支部別日当額計").Format = "N0"
            SetColsWidth(flxBranchBelongCloseList, dicColWidthPair)
            Me.grpBranchBelongCloseList.Visible = True
            'Me.flxBranchBelongCloseList.SetColsWidth(dicColWidthPair)
            'Me.flxBranchBelongCloseList.AdjustTextAlign()

        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub btnExecutiveSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecutiveSearch.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.cmbExecutiveYearSearch.SelectedIndex < 0) Then
                Me.cmbExecutiveYearSearch.BackColor = Color.LightPink
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"検索条件・締め日「年」"})
                CLMsg.Show("GE0006", "検索条件・締め日「年」")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim strDate As String = (Me.cmbExecutiveYearSearch.SelectedItem.ToString & If((Me.cmbExecutiveMonthSearch.SelectedIndex < 0), "", Me.cmbExecutiveMonthSearch.SelectedItem.ToString))
            'Dim table As DataTable = command.GetExecutiveCloseList(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate).Tables.Item("executive_close_list").Copy
            Dim table As DataTable = command.GetExecutiveCloseList("03", strDate).Tables.Item("executive_close_list").Copy
            If Me.dsDailyAllowance.Tables.Contains("executive_close_list") Then
                Me.dsDailyAllowance.Tables.Remove("executive_close_list")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxExecutiveCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("executive_close_list")
            Utilities.ControlEdit(True, Me.grpExecutiveCloseList)
            If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                Me.btnExecutiveMonthPrint.Enabled = False
            End If
            Me.SetExecutiveCloseListStyle()

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

    Private Sub btnExecutiveMonthPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecutiveMonthPrint.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxExecutiveCloseList.Row < 1) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"中央執行締め日一覧", "中執月報印刷"})
                CLMsg.Show("GE0070", "中央執行締め日一覧", "中執月報印刷")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            'Dim viewer As New ReportViewer(command.GetExecutiveCloseListPrint(Me.dsDailyAllowance.Tables.Item("executive_close_list"), (Me.flxExecutiveCloseList.Row - 1), Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString), "Report.CentralExecutiveActivityDA.RptActivityDailyReport")
            Dim viewer As New ReportViewer(command.GetExecutiveCloseListPrint(Me.dsDailyAllowance.Tables.Item("executive_close_list"), (Me.flxExecutiveCloseList.Row - 1), "03"), New CR0502P3)
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

    Private Sub btnExecutiveRefIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecutiveRefIn.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxExecutiveCloseList.Row < 1) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"中央執行締め日一覧", "中執日当詳細の表示"})
                CLMsg.Show("GE0070", "中央執行締め日一覧", "中執日当詳細の表示")
                Return
            End If
            Dim num As Integer = Integer.Parse(Me.flxExecutiveCloseList.Item(Me.flxExecutiveCloseList.Row, "s_daily_pay").ToString)
            Dim num2 As Integer = Integer.Parse(Me.flxExecutiveCloseList.Item(Me.flxExecutiveCloseList.Row, "s_balance_daily_pay").ToString)
            Dim num3 As Integer = Integer.Parse(Me.flxExecutiveCloseList.Item(Me.flxExecutiveCloseList.Row, "s_food_expenses").ToString)
            Dim num4 As Integer = Integer.Parse(Me.flxExecutiveCloseList.Item(Me.flxExecutiveCloseList.Row, "s_balance_food_expenses").ToString)
            If (((num = 0) AndAlso (num2 = 0)) AndAlso ((num3 = 0) AndAlso (num4 = 0))) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GI0027", New String(0 - 1) {})
                CLMsg.Show("GI0027")
                Return
            End If
            Me.grdSelectedIdx = (Me.flxExecutiveCloseList.Row - 1)
            'TODO Utilities.OverlayUserControl("中執日当計算" & " - " & "詳細", New UserControl() {New CtlDailyAllowanceExecutiveDtl(Me, MyBase.objLoginSession, Me.tabSelectedIdx)})

            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls("pnlMain")
            uc = pn.Controls("UC050203")

            If uc Is Nothing Then
                uc = New UC050203(Me, Me.grdSelectedIdx)
                uc.Controls("label11").Text = "中執日当計算 - 詳細"
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
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

    Private Sub btnExecutiveRenew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExecutiveRenew.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim exception As AppUnionException = Nothing
            If (Me.cmbExecutiveCloseYear.SelectedIndex < 0) Then
                Me.cmbExecutiveCloseYear.BackColor = Color.LightPink
                If (exception Is Nothing) Then
                    'exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"締め日設定「年」"})
                    CLMsg.Show("GE0006", "締め日設定「年」")
                    Return
                Else
                    'exception.AddExceptionData("GE0006", New String() {"締め日設定「年」"})
                    CLMsg.Show("GE0006", "締め日設定「年」")
                    Return
                End If
            End If
            If (Me.cmbExecutiveCloseMonth.SelectedIndex < 0) Then
                Me.cmbExecutiveCloseMonth.BackColor = Color.LightPink
                If (exception Is Nothing) Then
                    'exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"締め日設定「月」"})
                    CLMsg.Show("GE0006", "締め日設定「月」")
                    Return
                Else
                    'exception.AddExceptionData("GE0006", New String() {"締め日設定「月」"})
                    CLMsg.Show("GE0006", "締め日設定「月」")
                    Return
                End If
            End If
            If (Not exception Is Nothing) Then
                Throw exception
            End If
            Dim time As New DateTime(Integer.Parse(Me.cmbExecutiveCloseYear.SelectedItem.ToString), Integer.Parse(Me.cmbExecutiveCloseMonth.SelectedItem.ToString), 1)
            Dim today As DateTime = PublicCommand.GetToday
            If (time > today) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0185", New String(0 - 1) {})
                CLMsg.Show("GE0185")
                Return
            End If
            FinancialAffairsUtility.CheckNetBankDataHasMade((Me.cmbExecutiveCloseYear.Text & Me.cmbExecutiveCloseMonth.Text), "03")
            Dim strDate As String = (Me.cmbExecutiveCloseYear.SelectedItem.ToString.PadLeft(4, "0"c) & Me.cmbExecutiveCloseMonth.SelectedItem.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(Integer.Parse(Me.cmbExecutiveCloseYear.SelectedItem.ToString), Integer.Parse(Me.cmbExecutiveCloseMonth.SelectedItem.ToString)).ToString.PadLeft(2, "0"c))
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim maxDailyPayClose As String = Me.GetMaxDailyPayClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString)
            If (strDate.CompareTo(maxDailyPayClose) = 0) Then
                If CLMsg.Show("GQ0032", strDate.Substring(0, 4), strDate.Substring(4, 2), "支部委員（三役）") <> DialogResult.Yes Then
                    'TODO If (MyBase.UnionMessage.ShowMessage(False, "GQ0032", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2)}) <> DialogResult.Yes) Then
                    Return
                End If
                Try
                    FrmWaitInfo.ShowWaitForm(Nothing)
                    command.CalcDailyAllowanceClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, True)
                Finally
                    FrmWaitInfo.CloseWaitForm()
                End Try
            Else
                If (strDate.CompareTo(maxDailyPayClose) > 0) Then
                    command.CheckAttendanceEntry(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, maxDailyPayClose)
                    If CLMsg.Show("GQ0030", strDate.Substring(0, 4), strDate.Substring(4, 2), "中央執行委員会") <> DialogResult.Yes Then
                        'TODO If (MyBase.UnionMessage.ShowMessage(False, "GQ0030", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2), "中央執行委員会"}) <> DialogResult.Yes) Then
                        Return
                    End If
                    Try
                        FrmWaitInfo.ShowWaitForm(Nothing)
                        command.CalcDailyAllowanceClose(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate, False)
                        GoTo Label_03F3
                    Finally
                        FrmWaitInfo.CloseWaitForm()
                    End Try
                End If
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0069", New String() {DateTime.ParseExact(maxDailyPayClose, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月")})
                CLMsg.Show("GE0069", DateTime.ParseExact(maxDailyPayClose, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月"))
                Return
            End If
Label_03F3:
            If Me.grpExecutiveCloseList.Enabled Then
                If (Not Me.flxExecutiveCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxExecutiveCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpExecutiveCloseList)
            End If
            'TODO MyBase.UnionMessage.ShowMessage("GI0017", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2), "中央執行委員会"})
            CLMsg.Show("GI0017", strDate.Substring(0, 4), strDate.Substring(4, 2), "中央執行委員会")

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

    Private Sub SetExecutiveCloseListStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("締め日", 120)
            dicColWidthPair.Add("精算年月", 120)
            dicColWidthPair.Add("中執日当額計", 150)
            dicColWidthPair.Add("中執昼食費計", 150)
            dicColWidthPair.Add("登録年月日", 120)
            dicColWidthPair.Add("担当者", 130)
            Me.flxExecutiveCloseList.Enabled = True
            Me.flxExecutiveCloseList.Row = 1
            Me.flxExecutiveCloseList.AllowSorting = AllowSortingEnum.None
            Me.flxExecutiveCloseList.AllowEditing = False
            Me.flxExecutiveCloseList.AllowResizing = AllowResizingEnum.Columns
            Me.flxExecutiveCloseList.Cols.Item("締め日").Format = "yyyy/MM/dd"
            Me.flxExecutiveCloseList.Cols.Item("精算年月").Format = "yyyy/MM"
            Me.flxExecutiveCloseList.Cols.Item("中執日当額計").Format = "N0"
            Me.flxExecutiveCloseList.Cols.Item("中執昼食費計").Format = "N0"
            SetColsWidth(flxExecutiveCloseList, dicColWidthPair)
            Me.grpExecutiveCloseList.Visible = True
            'Me.flxExecutiveCloseList.SetColsWidth(dicColWidthPair)
            'Me.flxExecutiveCloseList.AdjustTextAlign()

        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub btnDgmSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDgmSearch.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.cmbDgmYearSearch.SelectedIndex < 0) Then
                Me.cmbDgmYearSearch.BackColor = Color.LightPink
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"検索条件・締め日「年」"})
                CLMsg.Show("GE0006", "検索条件・締め日「年」")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim strDate As String = (Me.cmbDgmYearSearch.SelectedItem.ToString & If((Me.cmbDgmMonthSearch.SelectedIndex < 0), "", Me.cmbDgmMonthSearch.SelectedItem.ToString))
            Dim table As DataTable = command.GetCommitteeCloseList("04", strDate).Tables.Item("dgm_close_list").Copy
            'TODO Dim table As DataTable = command.GetCommitteeCloseList(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strDate).Tables.Item("committee_close_list").Copy
            If Me.dsDailyAllowance.Tables.Contains("dgm_close_list") Then
                Me.dsDailyAllowance.Tables.Remove("dgm_close_list")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxDgmCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("dgm_close_list")
            Utilities.ControlEdit(True, Me.grpDgmCloseList)
            If (Not Me.flxDgmBelongCloseList.DataSource Is Nothing) Then
                DirectCast(Me.flxDgmBelongCloseList.DataSource, DataTable).Rows.Clear()
            End If
            Utilities.ControlEdit(False, Me.grpDgmBelongCloseList)
            Me.SetDgmCloseListStyle()

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

    Private Sub btnDgmBelongShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDgmBelongShow.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxDgmCloseList.Row < 0) Then
                Me.cmbDgmYearSearch.BackColor = Color.LightPink
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧", "支部別の表示"})
                CLMsg.Show("GE0070", "締め日一覧", "支部別の表示")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim time As DateTime = DateTime.ParseExact(Me.dsDailyAllowance.Tables.Item("dgm_close_list").Rows.Item((Me.flxDgmCloseList.Row - 1)).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
            Dim strCloseDate As String = (time.Year.ToString.PadLeft(4, "0"c) & time.Month.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(time.Year, time.Month).ToString.PadLeft(2, "0"c))
            'TODO Dim table As DataTable = command.GetCommitteeBelongCloseList(Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString, strCloseDate).Tables.Item("committee_belong_close_list").Copy
            Dim table As DataTable = command.GetCommitteeBelongCloseList("04", strCloseDate).Tables.Item("dgm_belong_close_list").Copy
            If Me.dsDailyAllowance.Tables.Contains("dgm_belong_close_list") Then
                Me.dsDailyAllowance.Tables.Remove("dgm_belong_close_list")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxDgmBelongCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list")
            Utilities.ControlEdit(True, Me.grpDgmBelongCloseList)
            If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                Me.btnCommitteeBelongPrint.Enabled = False
            End If
            Me.SetDgmBelongCloseListStyle()

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

    Private Sub btnDgmBelongPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDgmBelongPrint.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxDgmCloseList.Row < 1) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧・支部別", "支部別印刷"})
                CLMsg.Show("GE0070", "締め日一覧・支部別", "支部別印刷")
                Return
            End If
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            'Dim viewer As New ReportViewer(command.GetCommitteeBelongCloseListPrint(Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list"), (Me.flxDgmBelongCloseList.Row - 1), Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString), "Report.CommitteeDailyAllowance.RptLocalTotal")
            Dim viewer As New ReportViewer(command.GetCommitteeBelongCloseListPrint(Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list"), (Me.flxDgmBelongCloseList.Row - 1), "04"), New CR0502P2)
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

    Private Sub btnDgmRefIn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDgmRefIn.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            If (Me.flxDgmCloseList.Row < 1) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0070", New String() {"締め日一覧・支部別", "委員日当詳細の表示"})
                CLMsg.Show("GE0070", "締め日一覧・支部別", "委員日当詳細の表示")
                Return
            End If

            Dim num As Integer = 0
            Dim num2 As Integer = 0
            Try
                num = Integer.Parse(Me.flxDgmBelongCloseList.Item(Me.flxDgmBelongCloseList.Row, "今回日当計").ToString)
                num2 = Integer.Parse(Me.flxDgmBelongCloseList.Item(Me.flxDgmBelongCloseList.Row, "前回差分計").ToString)
                If ((num = 0) AndAlso (num2 = 0)) Then
                    'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GI0027", New String(0 - 1) {})
                    CLMsg.Show("GI0027")
                    'Return
                End If
            Catch ex As Exception
                ' NOP
            End Try
            Me.grdSelectedIdx = (Me.flxDgmBelongCloseList.Row - 1)

            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls("pnlMain")
            uc = pn.Controls("UC050205")

            If uc Is Nothing Then
                uc = New UC050205(Me, Me.grdSelectedIdx, True)
                uc.Controls("label11").Text = "DGM日当計算 - 入力"
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
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
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Private Sub btnDgmRenew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDgmRenew.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim strDailyPayKind As String = Me.dsDailyAllowance.Tables.Item("constant_dtl").Rows.Item(Me.tabSelectedIdx).Item("c_constant_seq").ToString
            Dim exception As AppUnionException = Nothing
            If (Me.cmbDgmCloseYear.SelectedIndex < 0) Then
                Me.cmbDgmCloseYear.BackColor = Color.LightPink
                If (exception Is Nothing) Then
                    'exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"締め日設定「年」"})
                    CLMsg.Show("GE0006", "締め日設定「年」")
                    Return
                Else
                    'exception.AddExceptionData("GE0006", New String() {"締め日設定「年」"})
                    CLMsg.Show("GE0006", "締め日設定「年」")
                    Return
                End If
            End If
            If (Me.cmbDgmCloseMonth.SelectedIndex < 0) Then
                Me.cmbDgmCloseMonth.BackColor = Color.LightPink
                If (exception Is Nothing) Then
                    'exception = New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", New String() {"締め日設定「月」"})
                    CLMsg.Show("GE0006", "締め日設定「月」")
                    Return
                Else
                    'exception.AddExceptionData("GE0006", New String() {"締め日設定「月」"})
                    CLMsg.Show("GE0006", "締め日設定「月」")
                    Return
                End If
            End If
            If (Not exception Is Nothing) Then
                Throw exception
            End If
            Dim time As New DateTime(Integer.Parse(Me.cmbDgmCloseYear.SelectedItem.ToString), Integer.Parse(Me.cmbDgmCloseMonth.SelectedItem.ToString), 1)
            Dim today As DateTime = PublicCommand.GetToday
            If (time > today) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0185", New String(0 - 1) {})
                CLMsg.Show("GE0185")
                Return
            End If
            FinancialAffairsUtility.CheckNetBankDataHasMade((Me.cmbDgmCloseYear.Text & Me.cmbDgmCloseMonth.Text), strDailyPayKind)
            Dim strDate As String = (Me.cmbDgmCloseYear.SelectedItem.ToString.PadLeft(4, "0"c) & Me.cmbDgmCloseMonth.SelectedItem.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(Integer.Parse(Me.cmbDgmCloseYear.SelectedItem.ToString), Integer.Parse(Me.cmbDgmCloseMonth.SelectedItem.ToString)).ToString.PadLeft(2, "0"c))
            'Dim class2 As New FactoryBusClass
            'Dim command As IDailyAllowanceCloseListCommand = DirectCast(class2.GetObject("Business.FinancialAffairs.DailyAllowance.DailyAllowanceCloseListCommand", New Object() {MyBase.objLoginSession}), IDailyAllowanceCloseListCommand)
            Dim command As New DailyAllowanceCloseListCommand
            Dim maxDailyPayClose As String = Me.GetMaxDailyPayClose(strDailyPayKind)
            If (strDate.CompareTo(maxDailyPayClose) = 0) Then
                'If (MyBase.UnionMessage.ShowMessage(False, "GQ0032", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2)}) <> DialogResult.Yes) Then
                If CLMsg.Show("GQ0032", strDate.Substring(0, 4), strDate.Substring(4, 2)) <> DialogResult.Yes Then
                    Return
                End If
                Try
                    If CLMsg.Show("GQ0063") <> DialogResult.Yes Then
                        'If (MyBase.UnionMessage.ShowMessage(False, "GQ0063", New String(0 - 1) {}) <> DialogResult.Yes) Then
                        Return
                    End If
                    FrmWaitInfo.ShowWaitForm(Nothing)
                    command.CalcDailyAllowanceClose(strDailyPayKind, strDate, True)
                Finally
                    FrmWaitInfo.CloseWaitForm()
                End Try
            Else
                If (strDate.CompareTo(maxDailyPayClose) > 0) Then
                    'command.CheckAttendanceEntry(strDailyPayKind, strDate, maxDailyPayClose) TODO
                    If CLMsg.Show("GQ0030", strDate.Substring(0, 4), strDate.Substring(4, 2), "ＤＧＭ日当") <> DialogResult.Yes Then
                        'If (MyBase.UnionMessage.ShowMessage(False, "GQ0030", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2), "ＤＧＭ日当"}) <> DialogResult.Yes) Then
                        Return
                    End If
                    Try
                        If CLMsg.Show("GQ0063") <> DialogResult.Yes Then
                            'If (MyBase.UnionMessage.ShowMessage(False, "GQ0063", New String(0 - 1) {}) <> DialogResult.Yes) Then
                            Return
                        End If
                        FrmWaitInfo.ShowWaitForm(Nothing)
                        command.CalcDailyAllowanceClose(strDailyPayKind, strDate, False)
                        GoTo Label_0431
                    Finally
                        FrmWaitInfo.CloseWaitForm()
                    End Try
                End If
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0069", New String() {DateTime.ParseExact(maxDailyPayClose, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月")})
                CLMsg.Show("GE0069", DateTime.ParseExact(maxDailyPayClose, "yyyyMMdd", Nothing).ToString("yyyy" & "年" & "MM" & "月"))
                Return
            End If
Label_0431:
            If Me.grpCommitteeCloseList.Enabled Then
                If (Not Me.flxDgmCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxDgmCloseList.DataSource, DataTable).Rows.Clear()
                End If
                If (Not Me.flxDgmBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxDgmBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpDgmCloseList)
            End If
            'MessageBox.Show("GI0017", New String() {strDate.Substring(0, 4), strDate.Substring(4, 2), "ＤＧＭ日当"})
            CLMsg.Show("GI0017", strDate.Substring(0, 4), strDate.Substring(4, 2), "ＤＧＭ日当")

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

    Private Sub SetDgmCloseListStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("締め日", 120)
            dicColWidthPair.Add("対象年月" & "(" & "始" & ")", 120)
            dicColWidthPair.Add("対象年月" & "(" & "終" & ")", 120)
            dicColWidthPair.Add("日当額計", 150)
            dicColWidthPair.Add("登録年月日", 120)
            dicColWidthPair.Add("担当者", 140)
            Me.flxDgmCloseList.Enabled = True
            Me.flxDgmCloseList.Row = 1
            Me.flxDgmCloseList.AllowSorting = AllowSortingEnum.None
            Me.flxDgmCloseList.AllowEditing = False
            Me.flxDgmCloseList.AllowResizing = AllowResizingEnum.Columns
            Me.flxDgmCloseList.Cols.Item("締め日").Format = "yyyy/MM/dd"
            Me.flxDgmCloseList.Cols.Item("日当額計").Format = "N0"
            SetColsWidth(flxDgmCloseList, dicColWidthPair)
            Me.grpDgmCloseList.Visible = True
            Me.flxDgmCloseList.Visible = True
            'TODO Me.flxCommitteeCloseList.SetColsWidth(dicColWidthPair)
            'TODO Me.flxCommitteeCloseList.AdjustTextAlign()

        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub SetDgmBelongCloseListStyle()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("締め日", 120)
            dicColWidthPair.Add("支部", 80)
            dicColWidthPair.Add("今回日当計", 150)
            dicColWidthPair.Add("前回差分計", 150)
            dicColWidthPair.Add("支部別日当額計", 150)
            Me.flxDgmBelongCloseList.Enabled = True
            Me.flxDgmBelongCloseList.Row = 1
            Me.flxDgmBelongCloseList.AllowSorting = AllowSortingEnum.None
            Me.flxDgmBelongCloseList.AllowEditing = False
            Me.flxDgmBelongCloseList.AllowResizing = AllowResizingEnum.Columns
            Me.flxDgmBelongCloseList.Cols.Item("締め日").Format = "yyyy/MM/dd"
            Me.flxDgmBelongCloseList.Cols.Item("今回日当計").Format = "N0"
            Me.flxDgmBelongCloseList.Cols.Item("前回差分計").Format = "N0"
            Me.flxDgmBelongCloseList.Cols.Item("支部別日当額計").Format = "N0"
            SetColsWidth(flxDgmBelongCloseList, dicColWidthPair)
            Me.grpDgmBelongCloseList.Visible = True
            'Me.flxCommitteeBelongCloseList.SetColsWidth(dicColWidthPair)
            'Me.flxCommitteeBelongCloseList.AdjustTextAlign()

        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub tabDailyAllowanceClose_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabDailyAllowanceClose.SelectedIndexChanged
        Try
            Me.cmbCommitteeYearSearch.BackColor = SystemColors.Window
            Me.cmbBranchYearSearch.BackColor = SystemColors.Window
            Me.cmbExecutiveYearSearch.BackColor = SystemColors.Window
            Me.cmbDgmYearSearch.BackColor = SystemColors.Window
            Me.tabSelectedIdx = Me.tabDailyAllowanceClose.SelectedIndex

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

    Private Sub flxCommitteeCloseList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxCommitteeCloseList.DoubleClick
        Try
            If Me.flxCommitteeCloseList.HitTest.Type.Equals(HitTestTypeEnum.Cell) And Me.flxCommitteeCloseList.Row > 0 Then
                Me.btnCommitteeBelongShow_Click(Me.btnCommitteeBelongShow, Nothing)
            End If
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub flxCommitteeBelongCloseList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxCommitteeBelongCloseList.DoubleClick
        Try
            If Me.flxCommitteeBelongCloseList.HitTest.Type.Equals(HitTestTypeEnum.Cell) And Me.flxCommitteeBelongCloseList.Row > 0 Then
                Me.btnCommitteeRefIn_Click(Me.btnCommitteeRefIn, Nothing)
            End If
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub flxBranchCloseList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxBranchCloseList.DoubleClick
        Try
            If Me.flxBranchCloseList.HitTest.Type.Equals(HitTestTypeEnum.Cell) And Me.flxBranchCloseList.Row > 0 Then
                Me.btnBranchBelongShow_Click(Me.btnBranchBelongShow, Nothing)
            End If
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub flxBranchBelongCloseList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxBranchBelongCloseList.DoubleClick
        Try
            If Me.flxBranchBelongCloseList.HitTest.Type.Equals(HitTestTypeEnum.Cell) And Me.flxBranchBelongCloseList.Row > 0 Then
                Me.btnBranchRefIn_Click(Me.btnBranchRefIn, Nothing)
            End If
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub flxExecutiveCloseList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxExecutiveCloseList.DoubleClick
        Try
            If Me.flxExecutiveCloseList.HitTest.Type.Equals(HitTestTypeEnum.Cell) And Me.flxExecutiveCloseList.Row > 0 Then
                Me.btnExecutiveRefIn_Click(Me.btnExecutiveRefIn, Nothing)
            End If
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub flxDgmCloseList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxDgmCloseList.DoubleClick
        Try
            If Me.flxDgmCloseList.HitTest.Type.Equals(HitTestTypeEnum.Cell) And Me.flxDgmCloseList.Row > 0 Then
                Me.btnDgmBelongShow_Click(Me.btnDgmBelongShow, Nothing)
            End If
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub flxDgmBelongCloseList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxDgmBelongCloseList.DoubleClick
        Try
            If Me.flxDgmBelongCloseList.HitTest.Type.Equals(HitTestTypeEnum.Cell) And Me.flxDgmBelongCloseList.Row > 0 Then
                btnDgmDtl_Click(sender, e)
            End If
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Sub btnDgmDtl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDgmDtl.Click
        If (Me.flxDgmCloseList.Row < 1) Then
            CLMsg.Show("GE0070", "締め日一覧・支部別", "委員日当詳細の表示")
            Return
        End If

        Dim num As Integer = 0
        Dim num2 As Integer = 0
        Try
            num = Integer.Parse(Me.flxDgmBelongCloseList.Item(Me.flxDgmBelongCloseList.Row, "今回日当計").ToString)
            num2 = Integer.Parse(Me.flxDgmBelongCloseList.Item(Me.flxDgmBelongCloseList.Row, "前回差分計").ToString)
            If ((num = 0) AndAlso (num2 = 0)) Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GI0027", New String(0 - 1) {})
                CLMsg.Show("GI0027")
                'Return
            End If
        Catch ex As Exception
            ' NOP
        End Try
        Me.grdSelectedIdx = (Me.flxDgmBelongCloseList.Row - 1)

        Dim pn As Panel
        Dim uc As Control

        Me.Visible = False

        pn = ParentForm.Controls("pnlMain")
        uc = pn.Controls("UC050205")

        If uc Is Nothing Then
            uc = New UC050205(Me, Me.grdSelectedIdx, False)
            uc.Controls("label11").Text = "DGM日当計算 - 詳細"
            Call pn.Controls.Add(uc)
        Else
            uc.Visible = True
        End If
    End Sub

    Private Sub cmbCommitteeYearSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCommitteeYearSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Cursor.Current = Cursors.WaitCursor
            Try
                If (Me.cmbCommitteeYearSearch.SelectedIndex < 0) Then
                    Me.cmbCommitteeYearSearch.BackColor = Color.LightPink
                    CLMsg.Show("GE0006", "検索条件・締め日「年」")
                    Return
                End If
                Dim command As New DailyAllowanceCloseListCommand
                Dim strDate As String = (Me.cmbCommitteeYearSearch.SelectedItem.ToString & If((Me.cmbCommitteeMonthSearch.SelectedIndex < 0), "", Me.cmbCommitteeMonthSearch.SelectedItem.ToString))
                Dim table As DataTable = command.GetCommitteeCloseList("01", strDate).Tables.Item("committee_close_list").Copy
                If Me.dsDailyAllowance.Tables.Contains("committee_close_list") Then
                    Me.dsDailyAllowance.Tables.Remove("committee_close_list")
                End If
                Me.dsDailyAllowance.Tables.Add(table)
                Me.flxCommitteeCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("committee_close_list")
                Utilities.ControlEdit(True, Me.grpCommitteeCloseList)
                If (Not Me.flxCommitteeBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxCommitteeBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpCommitteeBelongCloseList)
                Me.SetCommitteeCloseListStyle()

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End If

    End Sub

    Private Sub cmbCommitteeMonthSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCommitteeMonthSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Cursor.Current = Cursors.WaitCursor
            Try
                If (Me.cmbCommitteeYearSearch.SelectedIndex < 0) Then
                    Me.cmbCommitteeYearSearch.BackColor = Color.LightPink
                    CLMsg.Show("GE0006", "検索条件・締め日「年」")
                    Return
                End If
                Dim command As New DailyAllowanceCloseListCommand
                Dim strDate As String = (Me.cmbCommitteeYearSearch.SelectedItem.ToString & If((Me.cmbCommitteeMonthSearch.SelectedIndex < 0), "", Me.cmbCommitteeMonthSearch.SelectedItem.ToString))
                Dim table As DataTable = command.GetCommitteeCloseList("01", strDate).Tables.Item("committee_close_list").Copy
                If Me.dsDailyAllowance.Tables.Contains("committee_close_list") Then
                    Me.dsDailyAllowance.Tables.Remove("committee_close_list")
                End If
                Me.dsDailyAllowance.Tables.Add(table)
                Me.flxCommitteeCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("committee_close_list")
                Utilities.ControlEdit(True, Me.grpCommitteeCloseList)
                If (Not Me.flxCommitteeBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxCommitteeBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpCommitteeBelongCloseList)
                Me.SetCommitteeCloseListStyle()

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End If

    End Sub

    Private Sub cmbBranchYearSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbBranchYearSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Cursor.Current = Cursors.WaitCursor
            Try
                If (Me.cmbBranchYearSearch.SelectedIndex < 0) Then
                    Me.cmbBranchYearSearch.BackColor = Color.LightPink
                    CLMsg.Show("GE0006", "検索条件・締め日「年」")
                    Return
                End If
                Dim command As New DailyAllowanceCloseListCommand
                Dim strDate As String = (Me.cmbBranchYearSearch.SelectedItem.ToString & If((Me.cmbBranchMonthSearch.SelectedIndex < 0), "", Me.cmbBranchMonthSearch.SelectedItem.ToString))
                Dim table As DataTable = command.GetCommitteeCloseList("02", strDate).Tables.Item("branch_close_list").Copy 'TODO
                If Me.dsDailyAllowance.Tables.Contains("branch_close_list") Then
                    Me.dsDailyAllowance.Tables.Remove("branch_close_list")
                End If
                Me.dsDailyAllowance.Tables.Add(table)
                Me.flxBranchCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("branch_close_list")
                Utilities.ControlEdit(True, Me.grpBranchCloseList)
                If (Not Me.flxBranchBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxBranchBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpBranchBelongCloseList)
                Me.SetBranchCloseListStyle()

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    Private Sub cmbBranchMonthSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbBranchMonthSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Cursor.Current = Cursors.WaitCursor
            Try
                If (Me.cmbBranchYearSearch.SelectedIndex < 0) Then
                    Me.cmbBranchYearSearch.BackColor = Color.LightPink
                    CLMsg.Show("GE0006", "検索条件・締め日「年」")
                    Return
                End If
                Dim command As New DailyAllowanceCloseListCommand
                Dim strDate As String = (Me.cmbBranchYearSearch.SelectedItem.ToString & If((Me.cmbBranchMonthSearch.SelectedIndex < 0), "", Me.cmbBranchMonthSearch.SelectedItem.ToString))
                Dim table As DataTable = command.GetCommitteeCloseList("02", strDate).Tables.Item("branch_close_list").Copy 'TODO
                If Me.dsDailyAllowance.Tables.Contains("branch_close_list") Then
                    Me.dsDailyAllowance.Tables.Remove("branch_close_list")
                End If
                Me.dsDailyAllowance.Tables.Add(table)
                Me.flxBranchCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("branch_close_list")
                Utilities.ControlEdit(True, Me.grpBranchCloseList)
                If (Not Me.flxBranchBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxBranchBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpBranchBelongCloseList)
                Me.SetBranchCloseListStyle()

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    Private Sub cmbExecutiveYearSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbExecutiveYearSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Cursor.Current = Cursors.WaitCursor
            Try
                If (Me.cmbExecutiveYearSearch.SelectedIndex < 0) Then
                    Me.cmbExecutiveYearSearch.BackColor = Color.LightPink
                    CLMsg.Show("GE0006", "検索条件・締め日「年」")
                    Return
                End If
                Dim command As New DailyAllowanceCloseListCommand
                Dim strDate As String = (Me.cmbExecutiveYearSearch.SelectedItem.ToString & If((Me.cmbExecutiveMonthSearch.SelectedIndex < 0), "", Me.cmbExecutiveMonthSearch.SelectedItem.ToString))
                Dim table As DataTable = command.GetExecutiveCloseList("03", strDate).Tables.Item("executive_close_list").Copy
                If Me.dsDailyAllowance.Tables.Contains("executive_close_list") Then
                    Me.dsDailyAllowance.Tables.Remove("executive_close_list")
                End If
                Me.dsDailyAllowance.Tables.Add(table)
                Me.flxExecutiveCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("executive_close_list")
                Utilities.ControlEdit(True, Me.grpExecutiveCloseList)
                If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                    Me.btnExecutiveMonthPrint.Enabled = False
                End If
                Me.SetExecutiveCloseListStyle()

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End If

    End Sub

    Private Sub cmbExecutiveMonthSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbExecutiveMonthSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Cursor.Current = Cursors.WaitCursor
            Try
                If (Me.cmbExecutiveYearSearch.SelectedIndex < 0) Then
                    Me.cmbExecutiveYearSearch.BackColor = Color.LightPink
                    CLMsg.Show("GE0006", "検索条件・締め日「年」")
                    Return
                End If
                Dim command As New DailyAllowanceCloseListCommand
                Dim strDate As String = (Me.cmbExecutiveYearSearch.SelectedItem.ToString & If((Me.cmbExecutiveMonthSearch.SelectedIndex < 0), "", Me.cmbExecutiveMonthSearch.SelectedItem.ToString))
                Dim table As DataTable = command.GetExecutiveCloseList("03", strDate).Tables.Item("executive_close_list").Copy
                If Me.dsDailyAllowance.Tables.Contains("executive_close_list") Then
                    Me.dsDailyAllowance.Tables.Remove("executive_close_list")
                End If
                Me.dsDailyAllowance.Tables.Add(table)
                Me.flxExecutiveCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("executive_close_list")
                Utilities.ControlEdit(True, Me.grpExecutiveCloseList)
                If Not MDFinanceCommon.GetPrintPower("UC050201") Then
                    Me.btnExecutiveMonthPrint.Enabled = False
                End If
                Me.SetExecutiveCloseListStyle()

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            Finally
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    Private Sub cmbDgmYearSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDgmYearSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Cursor.Current = Cursors.WaitCursor
            Try
                If (Me.cmbDgmYearSearch.SelectedIndex < 0) Then
                    Me.cmbDgmYearSearch.BackColor = Color.LightPink
                    CLMsg.Show("GE0006", "検索条件・締め日「年」")
                    Return
                End If
                Dim command As New DailyAllowanceCloseListCommand
                Dim strDate As String = (Me.cmbDgmYearSearch.SelectedItem.ToString & If((Me.cmbDgmMonthSearch.SelectedIndex < 0), "", Me.cmbDgmMonthSearch.SelectedItem.ToString))
                Dim table As DataTable = command.GetCommitteeCloseList("04", strDate).Tables.Item("dgm_close_list").Copy
                If Me.dsDailyAllowance.Tables.Contains("dgm_close_list") Then
                    Me.dsDailyAllowance.Tables.Remove("dgm_close_list")
                End If
                Me.dsDailyAllowance.Tables.Add(table)
                Me.flxDgmCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("dgm_close_list")
                Utilities.ControlEdit(True, Me.grpDgmCloseList)
                If (Not Me.flxDgmBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxDgmBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpDgmBelongCloseList)
                Me.SetDgmCloseListStyle()

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End If

    End Sub

    Private Sub cmbDgmMonthSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDgmMonthSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Cursor.Current = Cursors.WaitCursor
            Try
                If (Me.cmbDgmYearSearch.SelectedIndex < 0) Then
                    Me.cmbDgmYearSearch.BackColor = Color.LightPink
                    CLMsg.Show("GE0006", "検索条件・締め日「年」")
                    Return
                End If
                Dim command As New DailyAllowanceCloseListCommand
                Dim strDate As String = (Me.cmbDgmYearSearch.SelectedItem.ToString & If((Me.cmbDgmMonthSearch.SelectedIndex < 0), "", Me.cmbDgmMonthSearch.SelectedItem.ToString))
                Dim table As DataTable = command.GetCommitteeCloseList("04", strDate).Tables.Item("dgm_close_list").Copy
                If Me.dsDailyAllowance.Tables.Contains("dgm_close_list") Then
                    Me.dsDailyAllowance.Tables.Remove("dgm_close_list")
                End If
                Me.dsDailyAllowance.Tables.Add(table)
                Me.flxDgmCloseList.DataSource = Me.dsDailyAllowance.Tables.Item("dgm_close_list")
                Utilities.ControlEdit(True, Me.grpDgmCloseList)
                If (Not Me.flxDgmBelongCloseList.DataSource Is Nothing) Then
                    DirectCast(Me.flxDgmBelongCloseList.DataSource, DataTable).Rows.Clear()
                End If
                Utilities.ControlEdit(False, Me.grpDgmBelongCloseList)
                Me.SetDgmCloseListStyle()

            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            Finally
                Cursor.Current = Cursors.Default
            End Try

        End If

    End Sub

End Class
