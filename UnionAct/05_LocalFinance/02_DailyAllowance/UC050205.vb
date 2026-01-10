Imports C1.Win.C1FlexGrid
Imports UnionAct.Framework.UnionException
Imports System.Reflection
Imports UnionAct.GUI.Common
Imports UnionAct.Business.FinancialAffairs.DailyAllowance
Imports UnionAct.GUI.Document
Imports UnionAct.NpgsqlDummy
Imports UnionAct.Framework.Command
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg

Public Class UC050205

    Private dsDailyAllowance As DataSet
    Private OwnerInstance As UC050201
    Private tabPageIdx As Integer
    Private fClosed As Boolean
    Private strPrevCloseDate As String = ""

    Public Sub New(ByVal OwnerInstance As UC050201, ByVal tabPageIdx As Integer, ByVal fClosed As Boolean)
        Me.InitializeComponent()
        Me.OwnerInstance = OwnerInstance
        Me.dsDailyAllowance = OwnerInstance.dsDailyAllowance.Copy
        Me.tabPageIdx = tabPageIdx
        Me.fClosed = fClosed
    End Sub

    Private Sub InitData()
        Try
            Dim command As New DailyAllowanceCloseListCommand
            Dim strCloseDate As String = ""
            Dim strBelongingId As String = ""
            If Not Me.fClosed Then
                If Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list").Rows.Count > 0 Then
                    Dim time As DateTime = DateTime.ParseExact(Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
                    strCloseDate = (time.Year.ToString.PadLeft(4, "0"c) & time.Month.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(time.Year, time.Month).ToString.PadLeft(2, "0"c))
                    Dim time2 As DateTime = CDate(Me.dsDailyAllowance.Tables.Item("dgm_close_list").Rows.Item((Me.OwnerInstance.flxDgmCloseList.Row - 1)).Item("d_begin"))
                    strBelongingId = Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("k_belonging").ToString
                    Dim str3 As String = Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("支部").ToString
                    Me.lblCloseYear.Text = time.Year.ToString.PadLeft(4, "0"c)
                    Me.lblCloseMonth.Text = time.Month.ToString.PadLeft(2, "0"c)
                    Me.lblYearFrom.Text = time2.Year.ToString.PadLeft(4, "0"c)
                    Me.lblMonthFrom.Text = time2.Month.ToString.PadLeft(2, "0"c)
                    Me.lblYearTo.Text = time.Year.ToString.PadLeft(4, "0"c)
                    Me.lblMonthTo.Text = time.Month.ToString.PadLeft(2, "0"c)
                    Me.lblBelonging.Text = str3
                End If
            End If
            Dim table As DataTable
            Try
                If strCloseDate = "" Then
                    strCloseDate = MDMasterCommon.GetKeyDate()
                End If
                strPrevCloseDate = command.GetPrevDailyPayClose("04", strCloseDate)
                table = command.GetCommitteeCloseDtlDgm(strCloseDate, fClosed).Tables.Item("dgm_close_dtl").Copy
                If table.Columns.Count = 0 Then
                    table = MDFinanceCommon.MakeEmptyDetailTable("dgm_close_dtl")
                End If
            Catch ex As Exception
                table = MDFinanceCommon.MakeEmptyDetailTable("dgm_close_dtl")
            End Try
            If Me.dsDailyAllowance.Tables.Contains("dgm_close_dtl") Then
                Me.dsDailyAllowance.Tables.Remove("dgm_close_dtl")
            End If
            Me.dsDailyAllowance.Tables.Add(table)
            Me.flxDailyPayClose.DataSource = Me.dsDailyAllowance.Tables.Item("dgm_close_dtl")
            CalcTotal()

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
            dicColWidthPair.Add("開催日", 80)
            dicColWidthPair.Add("社員番号", 60)
            dicColWidthPair.Add("氏名", 130)
            dicColWidthPair.Add("資格", 60)
            dicColWidthPair.Add("機種", 60)
            dicColWidthPair.Add("支部", 80)
            dicColWidthPair.Add("今回日当計", 115)
            dicColWidthPair.Add("前回差分計", 115)
            dicColWidthPair.Add("日当計", 115)
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
            Me.flxDailyPayClose.Cols.Item("開催日").AllowEditing = Me.fClosed
            Me.flxDailyPayClose.Cols(2).DataType = GetType(DateTime)
            Me.flxDailyPayClose.Cols.Item("社員番号").AllowEditing = Me.fClosed
            Me.flxDailyPayClose.Cols("氏名").Style.BackColor = Color.LightYellow
            Me.flxDailyPayClose.Cols("資格").Style.BackColor = Color.LightYellow
            Me.flxDailyPayClose.Cols("機種").Style.BackColor = Color.LightYellow
            Me.flxDailyPayClose.Cols("支部").Style.BackColor = Color.LightYellow
            Me.flxDailyPayClose.Cols("日当計").Style.BackColor = Color.LightYellow
            Me.flxDailyPayClose.Cols.Item("今回日当計").AllowEditing = Me.fClosed
            Me.flxDailyPayClose.Cols.Item("今回日当計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("前回差分計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("日当計").Format = "N0"
            Me.flxDailyPayClose.Cols.Item("前回差分計").AllowEditing = Me.fClosed
            'Me.flxDailyPayClose.Cols.Item("社員番号").AllowSorting = True
            'Me.flxDailyPayClose.Cols.Item("機種").AllowSorting = True
            MDFinanceCommon.SetColsWidth(Me.flxDailyPayClose, dicColWidthPair)
            MDFinanceCommon.AdjustTextAlign(Me.flxDailyPayClose)

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
            If Me.fClosed Then
                Me.btnDailyPayPrint.Visible = False
                Me.btnUpdate.Visible = MDFinanceCommon.GetEntryPower("UC050201")
                Me.lblCloseYear.Text = ""
                Me.lblCloseMonth.Text = ""
                Me.lblYearFrom.Text = ""
                Me.lblMonthFrom.Text = ""
                Me.lblYearTo.Text = ""
                Me.lblMonthTo.Text = ""
            Else
                Me.btnDailyPayPrint.Visible = MDFinanceCommon.GetPrintPower("UC050201")
                Me.btnUpdate.Visible = False
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
            Dim strCloseDate As String = ""
            If Not Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list") Is Nothing Then
                Dim time As DateTime = DateTime.ParseExact(Me.dsDailyAllowance.Tables.Item("dgm_belong_close_list").Rows.Item(Me.OwnerInstance.grdSelectedIdx).Item("d_daily_pay_close").ToString, "yyyyMMdd", Nothing)
                strCloseDate = (time.Year.ToString.PadLeft(4, "0"c) & time.Month.ToString.PadLeft(2, "0"c) & DateTime.DaysInMonth(time.Year, time.Month).ToString.PadLeft(2, "0"c))
            End If
            If strCloseDate = "" Then
                strCloseDate = MDMasterCommon.GetKeyDate()
            End If
            Dim command As New DailyAllowanceCloseListCommand
            Dim viewer As New ReportViewer(command.GetCommitteeCloseDtlPrint(Me.dsDailyAllowance.Tables.Item("dgm_close_dtl"), printData, "", "04", strCloseDate), New CR0502P1)
            viewer.ReportViewerShow()

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
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            'Utilities.RestoreUserControl()
            Dim pn As Panel
            Dim uc As Control

            If Me.fClosed Then
                ' 入力・変更内容破棄メッセージボックス表示
                If CLMsg.Show("GQ0007") = DialogResult.No Then
                    ' 「いいえ」ボタン押下時、処理を抜ける
                    Exit Sub
                End If
            End If

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

    Private Sub flxDailyPayClose_ValidateEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.ValidateEditEventArgs) Handles flxDailyPayClose.ValidateEdit
        If e.Row > 0 And e.Col = 3 Then
            Dim flx As C1FlexGrid = sender
            Dim strDate As String = Format(CDate(flx.Editor.Text), "yyyyMMdd")
            If strDate <= strPrevCloseDate Then
                MsgBox(String.Format("前回締日 {0} 以前の開催日は入力できません。", strPrevCloseDate))
                e.Cancel = True
            End If
        End If
    End Sub

    'Private Sub flxDailyPayClose_ComboCloseUp(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles flxDailyPayClose.ComboCloseUp
    '    Dim flx As C1FlexGrid = sender
    '    Dim strDate As String = Format(CDate(flx.Editor.Text), "yyyyMMdd")
    '    If strDate <= strPrevCloseDate Then
    '        MsgBox(String.Format("前回締日 {0} 以前の開催日は入力できません。", strPrevCloseDate))
    '        e.Cancel = True
    '    End If
    'End Sub

    'Private Sub flxDailyPayClose_ComboDropDown(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles flxDailyPayClose.ComboDropDown
    '    Me.flxDailyPayClose.SetData(e.Row, e.Col, DateValue("2011/12/01"))
    'End Sub

    Private Sub flxDailyPayClose_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles flxDailyPayClose.KeyPress
        If e.KeyChar = Chr(Keys.Enter) And Me.flxDailyPayClose.Rows.Count - 1 = Me.flxDailyPayClose.Row Then
            Dim index As Integer = Me.flxDailyPayClose.Row
            Dim strStaffId As Object = Me.flxDailyPayClose.Item(index, "社員番号")
            Dim aDay As Date = Me.flxDailyPayClose.Item(index, "開催日")
            If Not strStaffId Is System.DBNull.Value Then
                If SetStaffData(aDay, strStaffId.ToString, index) Then
                    Dim row As C1.Win.C1FlexGrid.Row = Me.flxDailyPayClose.Rows.Add
                    row("print_check") = False
                    If aDay = Nothing Then
                        row("開催日") = Date.Today
                    Else
                        row("開催日") = aDay
                    End If
                    row("sum_daily_pay") = 0
                    row("今回日当計") = 3000
                    row("sum_balance_daily_pay") = 0
                    row("前回差分計") = 0
                    row("日当計") = 0
                End If
            End If
        End If
    End Sub

    ' 2016/09/23 Added 入力済み変更時も処理を入れる
    Private Sub flxDailyPayClose_AfterEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles flxDailyPayClose.AfterEdit
        Dim index As Integer = Me.flxDailyPayClose.Row
        Dim strStaffId As Object = Me.flxDailyPayClose.Item(index, "社員番号")
        Dim aDay As Date = Me.flxDailyPayClose.Item(index, "開催日")
        If strStaffId Is System.DBNull.Value OrElse strStaffId.ToString = String.Empty Then
            ' 空欄時情報クリア
            Me.flxDailyPayClose.Item(index, "氏名") = ""
            Me.flxDailyPayClose.Item(index, "資格") = ""
            Me.flxDailyPayClose.Item(index, "機種") = ""
            Me.flxDailyPayClose.Item(index, "支部") = ""
        Else
            SetStaffData(aDay, strStaffId.ToString, index)
        End If
    End Sub


    Private Function SetStaffData(ByVal aDay As Date, ByVal strStaffId As String, ByVal iRowIndex As Integer)
        If Me.flxDailyPayClose.Item(iRowIndex, "社員番号") Is System.DBNull.Value Then
            Return False
        Else
            Dim table1 As DataTable = GetStaffAttribute(strStaffId)
            If table1.Rows.Count > 0 Then
                Me.flxDailyPayClose.Item(iRowIndex, "個人認証ＩＤ") = table1.Rows(0).Item(0)
                Me.flxDailyPayClose.Item(iRowIndex, "氏名") = table1.Rows(0).Item(5)
                Me.flxDailyPayClose.Item(iRowIndex, "資格") = table1.Rows(0).Item(11)
                Me.flxDailyPayClose.Item(iRowIndex, "機種") = table1.Rows(0).Item(13)
                Me.flxDailyPayClose.Item(iRowIndex, "支部") = table1.Rows(0).Item(7)
                ' 2016/09/23 Modified 他委員会活動チェック
                If CheckCallRollUserDtl(aDay, strStaffId) > 0 Then
                    Dim msgdtl As String = "　" + aDay + "　" + strStaffId + "：" + table1.Rows(0).Item(5)
                    Call CLMsg.Show("GE0186", "出欠簿", msgdtl)
                    Return False
                End If
            ElseIf strStaffId.Length > 0 Then
                CLMsg.Show("GI0031", strStaffId)
                Return False
            Else
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim table As DataTable = Me.dsDailyAllowance.Tables.Item("dgm_close_dtl")
        Dim aDay As Date
        Dim strStaffId As Object
        Dim strName As Object
        Dim i As Integer

        Try
            For i = Me.flxDailyPayClose.Rows.Count - 1 To 1 Step -1
                aDay = Me.flxDailyPayClose.Item(i, "開催日")
                strStaffId = Me.flxDailyPayClose.Item(i, "社員番号")
                strName = Me.flxDailyPayClose.Item(i, "氏名")

                If Not strStaffId Is System.DBNull.Value And Len(strStaffId.ToString) > 0 Then
                    If Not SetStaffData(aDay, strStaffId.ToString, i) Then
                        Return
                    End If
                    Me.flxDailyPayClose.Item(i, "日当計") = CInt(Me.flxDailyPayClose.Item(i, "今回日当計").ToString) + CInt(Me.flxDailyPayClose.Item(i, "前回差分計").ToString)
                ElseIf Me.flxDailyPayClose.Rows.Count > 2 Then
                    Me.flxDailyPayClose.RemoveItem(i)
                End If
            Next i
            CalcTotal()

            If Me.flxDailyPayClose.Item(Me.flxDailyPayClose.Rows.Count - 1, "社員番号") Is System.DBNull.Value Then
                MsgBox("社員番号を入力して下さい。")
                Return
            End If
            If CLMsg.Show("GQ0001") = DialogResult.Yes Then
                InsertCallRollUserDtl()
                Call CLMsg.Show("GI0015")
            End If

        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    Private Function GetStaffAttribute(ByVal strStaffId As String) As DataTable
        Dim command As NpgsqlCommand
        Dim npgsqlCon As NpgsqlConnection = Nothing
        Try
            npgsqlCon = New NpgsqlConnection()
            command = New NpgsqlCommand("SELECT * from staf_attribute_latest_view WHERE c_staf_id=:c_staf_id AND c_ksh=:c_ksh", npgsqlCon)
            command.Parameters.Add(New NpgsqlParameter("c_staf_id", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
            command.Parameters.Item("c_staf_id").Value = strStaffId
            command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh

            Dim dReader As NpgsqlDataReader = command.ExecuteReader
            If Not dReader.HasRows Then
                'Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
            End If

            Return dReader.getTable
        Finally
            If Not npgsqlCon Is Nothing Then
                npgsqlCon.close()
            End If
        End Try
    End Function

    Private Function CheckCallRollUserDtl(ByVal aDay As Date, ByVal strStaffId As String) As Integer
        Dim command As NpgsqlCommand
        Dim npgsqlCon As NpgsqlConnection = Nothing
        Try
            npgsqlCon = New NpgsqlConnection()
            command = New NpgsqlCommand("SELECT count(*) from call_roll_user_dtl WHERE c_user_id=:c_staf_id AND FORMAT(s_day, 'yyyy/MM/dd')=:d_day AND c_committee_id<>'DGM'", npgsqlCon)
            command.Parameters.Add(New NpgsqlParameter("c_staf_id", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("d_day", DbType.Date))
            command.Parameters.Item("c_staf_id").Value = strStaffId
            command.Parameters.Item("d_day").Value = aDay

            Dim dReader As NpgsqlDataReader = command.ExecuteReader

            Return dReader.getTable.Rows(0).Item(0)
        Finally
            If Not npgsqlCon Is Nothing Then
                npgsqlCon.close()
            End If
        End Try
    End Function

    Private Sub InsertCallRollUserDtl()
        Dim con As New NpgsqlConnection
        Dim tran As NpgsqlTransaction = con.BeginTransaction

        Try
            Dim strSqlUpd As String
            Dim strSqlIns As String
            strSqlUpd = "UPDATE call_roll_user_dtl SET s_daily_pay=:s_daily_pay,s_next_balance_daily_pay=:s_next_balance_daily_pay,d_ins=CONVERT(DATE,:d_ins,111),c_user_id_ins=:c_user_ins " &
                        "WHERE k_daily_pay_kind=:k_daily_pay_kind AND c_user_id=:c_user_id AND d_years=CONVERT(DATE,:d_years,111) AND s_day=CONVERT(DATE,:s_day,111) AND c_committee_id=:c_committee_id AND s_committee_seq=:s_committee_seq"
            strSqlIns = "INSERT INTO call_roll_user_dtl(c_user_id,d_years,s_day,c_committee_id,s_committee_seq,c_daily_pay_id,c_menu_seq,k_food_expenses,k_daily_pay_kind,s_daily_pay,s_food_expenses,s_next_balance_daily_pay,s_next_balance_food_expenses,d_ins,c_user_id_ins,s_up) " &
                        "VALUES(:c_user_id,CONVERT(DATE,:d_years,111),CONVERT(DATE,:s_day,111),:c_committee_id,:s_committee_seq,'','1','0',:k_daily_pay_kind,:s_daily_pay,0,:s_next_balance_daily_pay,0,CONVERT(DATE,:d_ins,111),:c_user_ins,0)"
            Dim command As New NpgsqlCommand(strSqlUpd, con, tran)

            For i = 1 To Me.flxDailyPayClose.Rows.Count - 1
                command.Parameters.Clear()
                command.SetSql(strSqlUpd)
                command.Parameters.Add(New NpgsqlParameter("s_daily_pay", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("s_next_balance_daily_pay", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_ins", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_ins", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_day", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.String))
                command.Parameters.Item("s_daily_pay").Value = CInt(Me.flxDailyPayClose.Item(i, "今回日当計").ToString)
                command.Parameters.Item("s_next_balance_daily_pay").Value = CInt(Me.flxDailyPayClose.Item(i, "前回差分計").ToString)
                command.Parameters.Item("d_ins").Value = Date.Today
                command.Parameters.Item("c_user_ins").Value = MDLoginInfo.UserId
                command.Parameters.Item("k_daily_pay_kind").Value = "04"
                command.Parameters.Item("c_user_id").Value = Me.flxDailyPayClose.Item(i, "個人認証ＩＤ").ToString
                command.Parameters.Item("d_years").Value = Format(CDate(Me.flxDailyPayClose.Item(i, "開催日").ToString), "yyyy/MM/01")
                command.Parameters.Item("s_day").Value = CDate(Me.flxDailyPayClose.Item(i, "開催日").ToString)
                command.Parameters.Item("c_committee_id").Value = "DGM"
                command.Parameters.Item("s_committee_seq").Value = 1

                If command.ExecuteNonQuery() <> 1 Then
                    command.Parameters.Clear()
                    command.SetSql(strSqlIns)
                    command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("d_years", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("s_day", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("k_daily_pay_kind", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("s_daily_pay", DbType.Int32))
                    command.Parameters.Add(New NpgsqlParameter("s_next_balance_daily_pay", DbType.Int32))
                    command.Parameters.Add(New NpgsqlParameter("d_ins", DbType.String))
                    command.Parameters.Add(New NpgsqlParameter("c_user_ins", DbType.String))
                    command.Parameters.Item("c_user_id").Value = Me.flxDailyPayClose.Item(i, "個人認証ＩＤ").ToString
                    command.Parameters.Item("d_years").Value = Format(CDate(Me.flxDailyPayClose.Item(i, "開催日").ToString), "yyyy/MM/01")
                    command.Parameters.Item("s_day").Value = CDate(Me.flxDailyPayClose.Item(i, "開催日").ToString)
                    command.Parameters.Item("c_committee_id").Value = "DGM"
                    command.Parameters.Item("s_committee_seq").Value = 1
                    command.Parameters.Item("k_daily_pay_kind").Value = "04"
                    command.Parameters.Item("s_daily_pay").Value = CInt(Me.flxDailyPayClose.Item(i, "今回日当計").ToString)
                    command.Parameters.Item("s_next_balance_daily_pay").Value = CInt(Me.flxDailyPayClose.Item(i, "前回差分計").ToString)
                    command.Parameters.Item("d_ins").Value = Date.Today
                    command.Parameters.Item("c_user_ins").Value = MDLoginInfo.UserId
                    command.ExecuteNonQuery()
                End If
            Next i

            tran.Commit()
        Catch ex As Exception
            tran.Rollback()
            CLMsg.Show("GE0001")
        End Try
    End Sub

    'Private Sub CalcTotal()
    '    Dim table As DataTable = Me.dsDailyAllowance.Tables.Item("dgm_close_dtl")
    '    Dim num As Integer = 0
    '    Dim num2 As Integer = 0
    '    Dim num3 As Integer = 0
    '    Dim i As Integer

    '    For i = 0 To table.Rows.Count - 1
    '        num = (num + Integer.Parse(table.Rows.Item(i).Item("日当計").ToString))
    '        num2 = (num2 + Integer.Parse(table.Rows.Item(i).Item("今回日当計").ToString))
    '        num3 = (num3 + Integer.Parse(table.Rows.Item(i).Item("前回差分計").ToString))
    '    Next i

    '    Me.lblSumAllDailyPay.Text = num.ToString("N0")
    '    Me.lblBeforeDailyPay.Text = num3.ToString("N0")
    '    Me.lblSumDailyPay.Text = num2.ToString("N0")
    'End Sub

    Private Sub CalcTotal()
        Dim num As Integer = 0
        Dim num2 As Integer = 0
        Dim num3 As Integer = 0
        Dim i As Integer

        For i = 1 To Me.flxDailyPayClose.Rows.Count - 1
            num = (num + CInt(Me.flxDailyPayClose.Item(i, "日当計").ToString))
            num2 = (num2 + CInt(Me.flxDailyPayClose.Item(i, "今回日当計").ToString))
            num3 = (num3 + CInt(Me.flxDailyPayClose.Item(i, "前回差分計").ToString))
        Next i

        Me.lblSumAllDailyPay.Text = num.ToString("N0")
        Me.lblBeforeDailyPay.Text = num3.ToString("N0")
        Me.lblSumDailyPay.Text = num2.ToString("N0")
    End Sub

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

        Dim tb As DataTable = Me.dsDailyAllowance.Tables.Item("dgm_close_dtl")
        If Not tb Is Nothing Then
            Dim rows As DataRow() = tb.Select(Nothing, strSortOrder).Clone()
            Dim ta As New DataTable()
            ta = tb.Clone()
            For Each row As DataRow In rows
                ta.ImportRow(row)
            Next
            Me.dsDailyAllowance.Tables.Remove("dgm_close_dtl")
            ta.TableName = "dgm_close_dtl"
            Me.dsDailyAllowance.Tables.Add(ta)
        End If
    End Sub
End Class
