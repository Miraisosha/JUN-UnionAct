#Region "UC020401"
'===========================================================================================================
'   クラスＩＤ　　：UC020401
'   クラス名称　　：出欠簿
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDCommon
Imports UnionAct.GUI.Document
Imports C1.Win.C1FlexGrid
Imports UnionAct.GUI.Common

Public Class UC020401

#Region "定数・変数"
    Private Const COMMITTEE_KI_KIKAN_NAME As String = "期の期間"                         '名称：期の期間
    Private Const COMMITTEE_MASTER_INFO = "委員会組織明細マスタの情報"                   '名称
    Private Const COMMITTEE_COMBO_COMMITTEE = "部／委員会"                               '名称
    Private Const COMMITTEE_COMBO_YEAR = "登録月　年"                                    '名称
    Private Const COMMITTEE_COMBO_MONTH = "登録月　月"                                   '名称
    Private Const DAILYPAY_MASTER_INFO = "日当明細マスタの情報"                          '名称
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private strPreYear As String                                                        '現在選択した年
    Private strPreMonth As String                                                       '現在選択した月
    Private tipCommittee As ToolTip
    Private DailyPayKind02Id As String() = New String() {"019", "029"}
    Private DailyPayKind02Seq As Integer()() = New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3}}
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC020401              ' UC020401
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC020401          ' 出欠簿画面
    Private intDays As Integer                                                          '日数字
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC020401_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/12(土)  Ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土)  Ryu  新規作成
    '***************************************************************************************************
    Private Sub UC020401_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            cmbCommittee.Enabled = True
            cmbYear.Enabled = True
            cmbMonth.Enabled = True
            Call Utilities.SetCanEditToControl(False, Me.chkLunch)
            '委員会リスト生成
            setComboboxValueFromDB(MDLoginInfo.PeriodId)
            cmbCommittee.SelectedIndex = 0
            '年選択肢リストの設定
            Call setCmbYear()
            cmbYear.SelectedIndex = 0
            cmbMonth.SelectedIndex = 0
            btnSearch.Enabled = True
            btnUpdate.Visible = True
            btnConfirm.Visible = False
            btnCancel.Visible = False
            btnUpdate.Visible = False
            tipCommittee = New ToolTip
            'セルスタイルを定義
            Call DefinitionStyles()
            '今月をデフォルト選択
            cmbYear.SelectedIndex = cmbYear.FindString(Now.Year.ToString)
            cmbMonth.SelectedIndex = cmbMonth.FindString(Now.Month.ToString.PadLeft(2, "0"c))
            strPreYear = cmbYear.Text
            strPreMonth = cmbMonth.Text
            'ログイン権限でボタン制御
            Dim dt As DataTable = Nothing
            Dim strRead As String = ""
            Dim strReg As String = ""
            Dim strPrint As String = ""
            Dim strFile As String = ""
            dt = MDCommon.getGrant(MENU_ID_UC020401)
            If dt.Rows.Count > 0 Then
                strRead = dt.Rows(0).Item(3).ToString
                strReg = dt.Rows(0).Item(4).ToString
                strPrint = dt.Rows(0).Item(5).ToString
                strFile = dt.Rows(0).Item(6).ToString
                btnSearch.Enabled = CInt(strRead)
                btnConfirm.Enabled = CInt(strReg)
                btnUpdate.Enabled = CInt(strReg)
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/10(木)  Ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/10(木)  Ryu  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        '検索主処理呼び出し
        Call Me.searchMain()

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnUpdate_Click
    '   名称　：内容変更ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/10(木)  Ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/10(木)  Ryu  新規作成
    '***************************************************************************************************
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        FrmWaitInfo.ShowWaitForm(Nothing)
        Try
            btnUpdate.Visible = False
            btnSearch.Enabled = False
            btnConfirm.Visible = True
            btnCancel.Visible = True
            cmbCommittee.Enabled = False
            cmbYear.Enabled = False
            cmbMonth.Enabled = False
            flxAttendance.AllowEditing = True
            '兼務しているユーザーのセルスタイル設定
            Call SetKenmuStyle()
            FrmWaitInfo.CloseWaitForm()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            FrmWaitInfo.CloseWaitForm()
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            FrmWaitInfo.CloseWaitForm()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnConfirm_Click
    '   名称　：登録確認ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/18(金)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  ryu  新規作成
    '***************************************************************************************************
    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim sql As String                                       'SQL文
        Dim dbAccess As New CLAccessMdb                         'DBアクセス
        Dim dt As DataTable                                     'データテーブル
        Dim rowCounter As Integer                               '行カウンター
        Dim colCounter As Integer                               '列カウンター
        Dim objValue As Object                                  'セルの値
        Dim strAttValue As String                               'セルの値、出欠種類
        Dim strYearMonth As String                              '年月
        Dim strDate As String                                   '日付
        Dim strCellStyle As String                              'セルスタイル名
        Dim intRtn As Integer                                   'SQL文の実行結果
        Dim notCheck As Boolean                                 '出欠チェック状況
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '入力確認、昼食チェックした日は出欠が未チェックの場合、メッセージ表示し処理中断
            For rowCounter = 2 To (flxAttendance.Rows.Count - 1) Step 2
                For colCounter = 5 To flxAttendance.Cols.Count - 1 - intDays
                    Dim launchData As Object = flxAttendance.GetData(rowCounter, colCounter)
                    '昼食チェックしてある
                    If launchData Then
                        Dim attendanceData As Object = flxAttendance.GetData(rowCounter - 1, colCounter)
                        If attendanceData Is Nothing Then
                            notCheck = True
                        ElseIf String.IsNullOrEmpty(Trim(attendanceData.ToString)) Then
                            notCheck = True
                        End If
                        If notCheck Then
                            MsgBox(flxAttendance.GetData(rowCounter, 1) + "さんの　" + flxAttendance.GetData(0, colCounter) + "　の出欠状況は未入力です。", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "入力エラー")
                            Dim rg As CellRange = flxAttendance.GetCellRange(rowCounter - 1, colCounter, rowCounter - 1, colCounter)
                            flxAttendance.ShowCell(rowCounter - 1, colCounter)
                            Exit Sub
                        End If
                    End If
                Next
            Next
            '確認ダイアログ
            If CLMsg.Show("GQ0006", cmbCommittee.Text, cmbYear.Text, cmbMonth.Text) = DialogResult.Yes Then
                Cursor.Current = Cursors.WaitCursor
                strYearMonth = cmbYear.SelectedText + "/" + cmbMonth.SelectedText
                strYearMonth = cmbYear.Text + "/" + cmbMonth.Text
                'DBアクセス接続
                Call dbAccess.Connect()
                For rowCounter = 1 To (flxAttendance.Rows.Count - 1) Step 2
                    For colCounter = 5 To flxAttendance.Cols.Count - 1 - intDays
                        If Not flxAttendance.GetCellStyle(rowCounter, colCounter) Is Nothing Then
                            strCellStyle = flxAttendance.GetCellStyle(rowCounter, colCounter).Name
                            '編集可セルのデータのみDB操作を行う
                            If Not strCellStyle.Equals("lightYellowCell") Then
                                objValue = flxAttendance.GetData(rowCounter, colCounter)
                                If Not objValue Is Nothing Then
                                    strAttValue = Trim(objValue.ToString)
                                    strDate = strYearMonth + "/" + Replace(CStr(flxAttendance.GetData(0, colCounter)), "日", "")
                                    strDate = Format(Date.Parse(strDate), DATE_YYYYMMDD_FORMAT)
                                    If strAttValue = "" Then
                                        '該当出欠を削除
                                        sql = "DELETE from call_roll_user_dtl where c_user_id='" + CStr(flxAttendance.GetData(rowCounter, 2)) + "'"
                                        sql = sql + " and Format(d_years,'yyyy/MM/dd')='" + strYearMonth + "/01'"
                                        sql = sql + " and Format(s_day,'yyyy/MM/dd')='" + strDate + "'"
                                    Else
                                        '該当出欠を追加か更新
                                        sql = "select * from call_roll_user_dtl where c_user_id='" + CStr(flxAttendance.GetData(rowCounter, 2)) + "'"
                                        sql = sql + " and Format(d_years,'yyyy/MM/dd')='" + strYearMonth + "/01'"
                                        sql = sql + " and Format(s_day,'yyyy/MM/dd')='" + strDate + "'"
                                        dt = dbAccess.ExecuteSql(sql)
                                        If dt.Rows.Count > 0 Then
                                            '更新
                                            '日当ＩＤ
                                            sql = "UPDATE call_roll_user_dtl set c_daily_pay_id='" + CStr(flxAttendance.GetData(rowCounter, 4)) + "'"
                                            ' 委員会枝番
                                            sql = sql + ",s_committee_seq='" + CStr(flxAttendance.GetData(rowCounter, 3)) + "'"
                                            ' 日当ＩＤ枝番
                                            sql = sql + ",c_menu_seq='" + strAttValue + "'"
                                            '中執昼食費可否
                                            If flxAttendance.GetCellCheck(rowCounter + 1, colCounter) = CheckEnum.Checked Then
                                                sql = sql + ",k_food_expenses='1'"
                                            Else
                                                sql = sql + ",k_food_expenses='0'"
                                            End If
                                            'd_up
                                            sql = sql + ",d_up='" + Now + "'"
                                            'c_user_id_up
                                            sql = sql + ",c_user_id_up='" + MDLoginInfo.UserId + "'"
                                            's_up
                                            sql = sql + ",s_up=s_up+1 "
                                            'Where
                                            sql = sql + "Where c_user_id='" + CStr(flxAttendance.GetData(rowCounter, 2)) + "'"
                                            sql = sql + " and Format(d_years,'yyyy/MM/dd')='" + strYearMonth + "/01'"
                                            sql = sql + " and Format(s_day,'yyyy/MM/dd')='" + strDate + "'"
                                        Else
                                            '新規
                                            sql = "INSERT into call_roll_user_dtl(c_user_id,d_years,s_day,c_committee_id,s_committee_seq,c_daily_pay_id,c_menu_seq,k_food_expenses,d_ins,c_user_id_ins,s_up) values('"
                                            '個人認証ID
                                            sql = sql + CStr(flxAttendance.GetData(rowCounter, 2)) + "'"
                                            '対象年月
                                            sql = sql + ",'" + strYearMonth + "/01'"
                                            '対象日付
                                            sql = sql + ",'" + strDate + "'"
                                            '委員会ID
                                            sql = sql + ",'" + cmbCommittee.SelectedValue + "'"
                                            '委員会枝番
                                            sql = sql + ",'" + CStr(flxAttendance.GetData(rowCounter, 3)) + "'"
                                            ' 日当ＩＤ
                                            sql = sql + ",'" + CStr(flxAttendance.GetData(rowCounter, 4)) + "'"
                                            ' 日当ＩＤ枝番
                                            sql = sql + ",'" + strAttValue + "'"
                                            '中執昼食費可否
                                            If flxAttendance.GetCellCheck(rowCounter + 1, colCounter) = CheckEnum.Checked Then
                                                sql = sql + ",'1'"
                                            Else
                                                sql = sql + ",'0'"
                                            End If
                                            'd_up
                                            sql = sql + ",'" + Now + "'"
                                            'c_user_id_up
                                            sql = sql + ",'" + MDLoginInfo.UserId + "'"
                                            's_up
                                            sql = sql + ",'1')"
                                        End If
                                    End If
                                    'DBトランザクション開始
                                    Call dbAccess.BeginTran()
                                    'SQL文実行
                                    intRtn = dbAccess.ExecuteNonQuery(sql)
                                    'DBトランザクションコミット
                                    dbAccess.CommitTran()
                                    If sql.StartsWith("DELETE") Then
                                        log.Info(String.Format("{0}件のデータを削除しました。", CStr(intRtn)))
                                    ElseIf sql.StartsWith("UPDATE") Then
                                        log.Info(String.Format("{0}件のデータを更新しました。", CStr(intRtn)))
                                    ElseIf sql.StartsWith("INSERT") Then
                                        log.Info(String.Format("{0}件のデータを追加しました。", CStr(intRtn)))
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
                'DB接続切断
                Call dbAccess.Disconnect()
                '表示制御
                cmbCommittee.Enabled = True
                cmbYear.Enabled = True
                cmbMonth.Enabled = True
                btnSearch.Enabled = True
                btnConfirm.Visible = False
                btnCancel.Visible = False
                btnUpdate.Visible = True
                grpResult.Visible = True
                flxAttendance.AllowEditing = False

                '再検索
                Call Me.searchMain()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Cursor.Current = Cursors.Default
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2011/11/29(火)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火)  ryu  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If CLMsg.Show("GQ0007") = DialogResult.Yes Then
                cmbCommittee.Enabled = True
                cmbYear.Enabled = True
                cmbMonth.Enabled = True
                btnSearch.Enabled = True
                btnUpdate.Visible = True
                btnConfirm.Visible = False
                btnCancel.Visible = False
                btnUpdate.Visible = False
                grpResult.Visible = False
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbCommittee_KeyPress
    '   名称　：委員会コンボボックスキープレス処理
    '   概要　：
    '   作成日：2011/11/18(金)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  ryu  新規作成
    '***************************************************************************************************
    Private Sub cmbCommittee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCommittee.KeyPress
        If e.KeyChar = vbCr Then
            btnSearch_Click(sender, Nothing)
        End If
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbYear_KeyPress
    '   名称　：年コンボボックスキープレス処理
    '   概要　：
    '   作成日：2011/11/18(金)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  ryu  新規作成
    '***************************************************************************************************
    Private Sub cmbYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbYear.KeyPress
        If e.KeyChar = vbCr Then
            btnSearch_Click(sender, Nothing)
        End If
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbMonth_KeyPress
    '   名称　：月コンボボックスキープレス処理
    '   概要　：
    '   作成日：2011/11/18(金)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  ryu  新規作成
    '***************************************************************************************************
    Private Sub cmbMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMonth.KeyPress
        If e.KeyChar = vbCr Then
            btnSearch_Click(sender, Nothing)
        End If
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbCommittee_SelectionChangeCommitted
    '   名称　：委員会コンボボックスチェンジ処理
    '   概要　：対象期選択した後に対象年を期の期間内年に設定しなおす
    '   作成日：2011/11/24(木)  Ryu
    '   更新日：
    '--------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  Ryu  新規作成
    '***************************************************************************************************
    Private Sub cmbCommittee_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCommittee.SelectionChangeCommitted
        Dim strKiValue As String                        '選択した委員会コード
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            strKiValue = cmbCommittee.SelectedValue
            If strKiValue <> Me.lblKiValue.Text Then
                lblKiValue.Text = strKiValue
                grpResult.Visible = False
                btnUpdate.Visible = False
                btnConfirm.Visible = False
                btnCancel.Visible = False
                '年の選択肢を設定し直す
                'Call setCmbYear()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbYear_SelectionChangeCommitted
    '   名称　：年コンボボックスチェンジ処理
    '   概要　：年を選択しなおした時、詳細エリアを非表示にする
    '   作成日：2011/12/05(月)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/05(月)  ryu  新規作成
    '***************************************************************************************************
    Private Sub cmbYear_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbYear.SelectionChangeCommitted
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If strPreYear <> cmbYear.Text Then
                grpResult.Visible = False
                btnUpdate.Visible = False
                strPreYear = cmbYear.Text
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbMonth_SelectionChangeCommitted
    '   名称　：月コンボボックスチェンジ処理
    '   概要　：月を選択しなおした時、詳細エリアを非表示にする
    '   作成日：2011/12/05(月)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/05(月)  ryu  新規作成
    '***************************************************************************************************
    Private Sub cmbMonth_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonth.SelectionChangeCommitted
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If strPreMonth <> cmbMonth.Text Then
                grpResult.Visible = False
                btnUpdate.Visible = False
                strPreMonth = cmbMonth.Text
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：flxAttendance_StartEdit
    '   名称　：編集不可制御
    '   概要　：編集不可セル制御
    '   作成日：2011/11/29(火) ryu
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火) ryu  新規作成
    '***************************************************************************************************
    Private Sub flxAttendance_StartEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles flxAttendance.StartEdit
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Dim strCellStyle As String                                  'セルスタイル名
        Try
            If Not flxAttendance.GetCellStyle(flxAttendance.RowSel, flxAttendance.ColSel) Is Nothing Then
                strCellStyle = flxAttendance.GetCellStyle(flxAttendance.RowSel, flxAttendance.ColSel).Name
                If strCellStyle.Equals("lightYellowCell") Or strCellStyle.Equals("boolYellow") Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：flxAttendance_BeforeEdit
    '   名称　：編集不可制御
    '   概要　：編集不可セル制御
    '   作成日：2011/11/29(火)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火)  ryu  新規作成
    '***************************************************************************************************
    Private Sub flxAttendance_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles flxAttendance.BeforeEdit
        Dim strCellStyle As String                                  'セルスタイル名
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If Not flxAttendance.GetCellStyle(flxAttendance.RowSel, flxAttendance.ColSel) Is Nothing Then
                strCellStyle = flxAttendance.GetCellStyle(flxAttendance.RowSel, flxAttendance.ColSel).Name
                If strCellStyle.Equals("lightYellowCell") Or strCellStyle.Equals("boolYellow") Then
                    e.Cancel = True
                End If
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：flxAttendance_MouseMove
    '   名称　：フレックスグリッドマウス移動処理
    '   概要　：ヒントを表す
    '   作成日：2011/12/12(月)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/12(月)  ryu  新規作成
    '***************************************************************************************************
    Private Sub flxAttendance_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles flxAttendance.MouseMove
        Dim blnFlg As Boolean                                           'ヒット出すべきか
        Dim dbAccess As New CLAccessMdb                                 'ＤＢアクセス
        Try
            blnFlg = False
            Dim info As HitTestInfo = Me.flxAttendance.HitTest(e.X, e.Y)
            If (info.Type = HitTestTypeEnum.Cell) Then
                Dim rowstyle As CellStyle = Me.flxAttendance.Rows.Item(info.Row).Style
                Dim cellStyle As CellStyle = Me.flxAttendance.GetCellStyle(info.Row, info.Column)
                Dim strCellValue As String = flxAttendance.GetData(info.Row, info.Column)
                If Not String.IsNullOrEmpty(strCellValue) Then
                    If Not rowstyle Is Nothing Then
                        If rowstyle.Name = "lightYellowCell" Or rowstyle.Name = "boolYellow" Then
                            blnFlg = True
                        End If
                    End If
                    If Not cellStyle Is Nothing Then
                        If cellStyle.Name = "lightYellowCell" Or cellStyle.Name = "boolYellow" Then
                            blnFlg = True
                        End If
                    End If
                    If blnFlg Then
                        tipCommittee.SetToolTip(flxAttendance, flxAttendance.GetData(info.Row, intDays + 4 + CInt(Replace(flxAttendance.GetData(0, info.Column), "日", ""))))
                        ''該当ユーザーの該当日付の出欠情報より委員会名を取得
                        'sql = "select l_name from call_roll_user_dtl left join committee on call_roll_user_dtl.c_committee_id=committee.c_committee_id where c_user_id='" + flxAttendance.GetData(info.Row, 2) + "' and format(s_day,'yyyyMMdd')='" + cmbYear.Text + cmbMonth.Text + Replace(flxAttendance.GetData(0, info.Column), "日", "").PadLeft(2, "0"c) + "'"
                        'dbAccess.Connect()
                        'dt = dbAccess.ExecuteSql(sql)
                        'dbAccess.Disconnect()
                        'If dt.Rows.Count > 0 Then
                        '    'ISDBNULLの判断を追加　2012/06/13　START
                        '    'tipCommittee.SetToolTip(flxAttendance, dt.Rows(0)("l_name"))
                        '    If IsDBNull(dt.Rows(0)("l_name")) Then
                        '        '委員会名がDBNULLの時、ポップアップメッセージを出したい場合ここにメッセージをセットする
                        '    Else
                        '        tipCommittee.SetToolTip(flxAttendance, dt.Rows(0)("l_name"))
                        '    End If
                        '    'ISDBNULLの判断を追加　2012/06/13　END
                        'End If
                    End If
                End If
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：setComboboxValueFromDB
    '   名称　：コンボボックス生成
    '   概要　：選択した日程情報で画面を初期化する
    '   引数　：ByVal strPeriodID As String = ログインで選択した期のID
    '   戻り値：なし
    '   作成日：2011/11/18(金)  Ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  Ryu  新規作成
    '***************************************************************************************************
    ''' <summary>コンボボックス生成</summary>
    ''' <param name="strPeriodID">ログインで選択した期のID</param>
    ''' <remarks></remarks>
    Private Sub setComboboxValueFromDB(ByVal strPeriodID As String)
        Dim dbAccess As New CLAccessMdb         'DBアクセス
        Dim dt As DataTable                     'データテーブル
        'Dim dtRow As DataRow                    '一行のデータ
        'Dim d_from As String                    '期の開始日
        Dim sql As String                       'SQL分
        Dim comStr As String                    '管理部ユーザー参照できる委員会
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '「委員会名」コンボボックスの初期化
            cmbCommittee.BeginUpdate()
            cmbCommittee.DataSource = Nothing
            cmbCommittee.Items.Clear()
            '' 期の開始日を取得
            'sql = "select d_from from period where c_period_id='" + strPeriodID + "'"
            ' データベースに接続
            dbAccess.Connect()
            '' データを取得
            'dt = dbAccess.ExecuteSql(sql)
            'If dt.Rows.Count = 1 Then
            '    dtRow = dt.Rows(0)
            '    d_from = dtRow("d_from")
            'Else
            '    ' データベースの接続を切断
            '    dbAccess.Disconnect()
            '    CLMsg.Show("GE0004", COMMITTEE_KI_KIKAN_NAME)
            '    Exit Sub
            'End If
            '基準日取得（最新期：現在日、最新期以外：期末日）
            Dim systemDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            Dim periodDTo As String = MDLoginInfo.PeriodTo
            Dim standDate As String
            If (MDLoginInfo.PeriodNewFlg = 1) Then
                standDate = systemDate
            Else
                standDate = periodDTo
            End If

            sql = "select c_committee_id, l_name from committee where d_from<='" + standDate + "' and d_to>='" + standDate + "' and c_committee_id<>'" + COMMITTEE_ID_CHUOU + "' "
            Select Case MDLoginInfo.CommitteeStatusFlg
                Case 0
                    '専従ユーザー
                Case 1
                    '一般の委員会ユーザー
                    sql = sql + "and c_committee_id='" + MDLoginInfo.CommitteeId + "'"
                Case 2
                    '管理部ユーザー
                    If MDLoginInfo.CommitteeIdList.Count > 0 Then
                        comStr = ""
                        sql = sql + "and c_committee_id in("
                        For intComm As Integer = 0 To MDLoginInfo.CommitteeIdList.Count - 1
                            comStr = comStr + "'" + MDLoginInfo.CommitteeIdList(intComm) + "'"
                            If intComm = MDLoginInfo.CommitteeIdList.Count - 1 Then
                                comStr = comStr + ")"
                            Else
                                comStr = comStr + ","
                            End If
                        Next
                        sql = sql + comStr
                    Else
                        CLMsg.Show("GE0004", COMMITTEE_KI_KIKAN_NAME)
                        Exit Sub
                    End If
            End Select
            ' データを取得
            dt = dbAccess.ExecuteSql(sql)
            ' データベースの接続を切断
            dbAccess.Disconnect()
            If dt.Rows.Count > 0 Then
                ' データソース設定
                cmbCommittee.DataSource = dt
                ' コンボボックス名称設定
                cmbCommittee.DisplayMember = "l_name"
                ' コンボボックス値設定
                cmbCommittee.ValueMember = "c_committee_id"
            Else
                CLMsg.Show("GE0004", COMMITTEE_KI_KIKAN_NAME)
            End If
            cmbCommittee.EndUpdate()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：setCmbYear
    '   名称　：対象年選択の設定
    '   概要　：選択した委員会の有効範囲年を抽出し、対象年コンボボックス選択肢に設定
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24(木)  Ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  Ryu  新規作成
    '***************************************************************************************************
    ''' <summary>対象年選択の設定</summary>
    ''' <remarks></remarks>
    Private Sub setCmbYear()
        Dim dbAccess As New CLAccessMdb             'DBアクセスクラス
        Dim d_from As String                        '期の開始日
        Dim d_to As String                          '期の終了日
        Dim dtRow As DataRow                        '一行のデータ
        Dim strKiValue As String                    '期のコード
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            strKiValue = cmbCommittee.SelectedValue
            If strKiValue <> "" Then
                '現在の選択肢をクリア
                cmbYear.Items.Clear()
                Dim table As DataTable = GetPeriodFromTo(cmbCommittee.SelectedValue, Format(Now, DATE_YYYYMMDD_8_FORMAT), cmbCommittee.SelectedText)
                If table.Rows.Count > 0 Then
                    dtRow = table.Rows(0)
                    d_from = Mid(dtRow("d_from"), 1, 4)
                    d_to = Mid(dtRow("d_to"), 1, 4)
                    Do Until d_from > d_to
                        cmbYear.Items.Add(d_from)
                        d_from = CStr(CInt(d_from) + 1)
                    Loop
                    'cmbYear.SelectedIndex = cmbYear.FindString(Now.Year.ToString)
                Else
                    '期の期間取得できなかった場合、エラー
                    CLMsg.Show("GE0004", COMMITTEE_KI_KIKAN_NAME)
                End If
                dbAccess.Disconnect()
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：GetPeriodFromTo
    '   名称　：期の開始と終了日付取得処理
    '   概要　：期の開始と終了日付取得
    '   引数　：ByVal strCommitteeId   As String = 期のID,
    '       　：ByVal strTargetDate    As String = 検索日付,
    '       　：ByVal strCommitteeName As String = 期の名称
    '   戻り値：DataTable = 期の開始と終了日付
    '   作成日：2011/11/24(木)  Ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  Ryu  新規作成
    '***************************************************************************************************
    ''' <summary>期の開始と終了日付取得処理</summary>
    ''' <param name="strCommitteeId">期のID</param>
    ''' <param name="strTargetDate">検索日付</param>
    ''' <param name="strCommitteeName">期の名称</param>
    ''' <returns>期の開始と終了日付</returns>
    ''' <remarks></remarks>
    Public Function GetPeriodFromTo(ByVal strCommitteeId As String,
                                    ByVal strTargetDate As String,
                                    ByVal strCommitteeName As String) As DataTable
        Dim table2 As New DataTable("period_from_to")                   'データテーブル
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim monthDifference As DataTable = GetMonthDifference(strTargetDate, strCommitteeId)
            If (monthDifference Is Nothing) Then
                CLMsg.Show("BE0030", strCommitteeName)
                Return Nothing
            End If
            Dim table As New DataTable
            table.TableName = "month_difference"
            table = monthDifference
            Dim time As DateTime = DateTime.ParseExact(MDLoginInfo.PeriodFrom, DATE_YYYYMMDD_8_FORMAT, Nothing).AddMonths(CInt(table.Rows.Item(0).Item("s_from_diff")))
            Dim time2 As DateTime = DateTime.ParseExact(MDLoginInfo.PeriodTo, DATE_YYYYMMDD_8_FORMAT, Nothing).AddMonths(CInt(table.Rows.Item(0).Item("s_to_diff")))
            table2.Columns.Add("d_from", GetType(String))
            table2.Columns.Add("d_to", GetType(String))
            Dim row As DataRow = table2.NewRow
            row.Item("d_from") = time.ToString(DATE_YYYYMMDD_8_FORMAT)
            row.Item("d_to") = time2.ToString(DATE_YYYYMMDD_8_FORMAT)
            table2.Rows.Add(row)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        '戻り値
        Return table2
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetMonthDifference
    '   名称　：開始と終了日付の調整処理
    '   概要　：
    '   引数　：ByVal strDate        As String = 期のID,
    '       　：ByVal strCommitteeId As String = 検索日付
    '   戻り値：DataTable = 開始と終了日付
    '   作成日：2011/11/24(木)  Ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  Ryu  新規作成
    '***************************************************************************************************
    ''' <summary>開始と終了日付の調整処理</summary>
    ''' <param name="strDate">期のID</param>
    ''' <param name="strCommitteeId">検索日付</param>
    ''' <returns>開始と終了日付</returns>
    ''' <remarks></remarks>
    Public Function GetMonthDifference(ByVal strDate As String,
                                       ByVal strCommitteeId As String) As DataTable
        Dim set2 As New DataTable                               'データテーブル
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim sql As String = "SELECT Min(committee_dtl.s_from_diff)as s_from_diff, MAX(committee_dtl.s_to_diff) as s_to_diff FROM (SELECT c_committee_id, d_from FROM committee WHERE c_committee_id = '" + strCommitteeId + "' AND d_from <= '" + strDate + "' AND d_to >= '" + strDate + "' AND c_ksh = '" + MDLoginInfo.Ksh + "' ) committee_A, committee_dtl WHERE committee_dtl.c_committee_id = committee_A.c_committee_id AND committee_dtl.d_from = committee_A.d_from"
            Dim dbAccess As New CLAccessMdb
            set2 = New DataTable("month_difference")
            dbAccess.Connect()
            set2 = dbAccess.ExecuteSql(sql)
            dbAccess.Disconnect()
            If set2.Rows.Count = 0 Then
                Return Nothing
            End If
            If (String.IsNullOrEmpty(set2.Rows.Item(0).Item("s_from_diff").ToString)) Then
                set2.Rows.Item(0).Item("s_from_diff") = "0"
            End If
            If (String.IsNullOrEmpty(set2.Rows.Item(0).Item("s_to_diff").ToString)) Then
                set2.Rows.Item(0).Item("s_to_diff") = "0"
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        '戻り値
        Return set2
    End Function

    '***************************************************************************************************
    '   ＩＤ　：CheckInput
    '   名称　：入力チェック処理
    '   概要　：入力チェック
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/24(木)  Ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  Ryu  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CheckInput() As Boolean
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If cmbCommittee.SelectedIndex < 0 Then
                CLMsg.Show("GE0010", COMMITTEE_COMBO_COMMITTEE)
                Return False
            End If
            If cmbYear.SelectedIndex < 0 Then
                CLMsg.Show("GE0010", COMMITTEE_COMBO_YEAR)
                Return False
            End If
            If cmbMonth.SelectedIndex < 0 Then
                CLMsg.Show("GE0010", COMMITTEE_COMBO_MONTH)
                Return False
            End If
            '入力した日付は期の範囲内にあるか
            Dim table As DataTable = GetPeriodFromTo(cmbCommittee.SelectedValue, Me.cmbYear.Text & Me.cmbMonth.Text & "01", cmbCommittee.Text)
            If table Is Nothing Then
                Return False
            End If
            Dim time1 As DateTime = DateTime.ParseExact(table.Rows.Item(0).Item("d_from").ToString, DATE_YYYYMMDD_8_FORMAT, Nothing)
            Dim time2 As DateTime = DateTime.ParseExact(table.Rows.Item(0).Item("d_to").ToString, DATE_YYYYMMDD_8_FORMAT, Nothing)
            Dim str As String = (Me.cmbYear.SelectedItem.ToString.PadLeft(4, "0"c) & Me.cmbMonth.SelectedItem.ToString.PadLeft(2, "0"c) & "01")
            If ((str.CompareTo(time1.ToString(DATE_YYYYMMDD_8_FORMAT)) < 0) OrElse (str.CompareTo(time2.ToString(DATE_YYYYMMDD_8_FORMAT)) > 0)) Then
                CLMsg.Show("GE0013", time1.ToString("yyyy" & "年" & "MM" & "月"), time2.ToString("yyyy" & "年" & "MM" & "月"))
                Return False
            End If
            Return True
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Function

    '***************************************************************************************************
    '   ＩＤ　：DefinitionStyles
    '   名称　：スタイル定義
    '   概要　：セルのスタイルを定義
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/29(火)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火)  ryu  新規作成
    '***************************************************************************************************
    ''' <summary>スタイル定義</summary>
    ''' <remarks></remarks>
    Private Sub DefinitionStyles()
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            'スタイルを作成
            ' 土曜日
            Dim cs As CellStyle = flxAttendance.Styles.Add("yobi6")
            cs.ForeColor = Color.White
            cs.BackColor = Color.SkyBlue
            ' 日曜日
            cs = flxAttendance.Styles.Add("yobi7")
            cs.ForeColor = Color.White
            cs.BackColor = Color.Pink
            ' チェックボックス（背景白　通常）
            cs = flxAttendance.Styles.Add("bool")
            cs.DataType = Type.GetType("System.Boolean")
            cs.ImageAlign = ImageAlignEnum.CenterCenter
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
            ' チェックボックス（背景青　通常）
            cs = flxAttendance.Styles.Add("boollightcyan")
            cs.BackColor = Color.LightCyan
            cs.DataType = Type.GetType("System.Boolean")
            cs.ImageAlign = ImageAlignEnum.CenterCenter
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
            ' チェックボックス（背景白青共通　ロック）
            cs = flxAttendance.Styles.Add("boolYellow")
            cs.DataType = Type.GetType("System.Boolean")
            cs.ImageAlign = ImageAlignEnum.CenterCenter
            cs.BackColor = Color.Yellow
            cs.Editor = Nothing
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
            ' ヘッダー部
            cs = flxAttendance.Styles.Add("normal")
            cs.DataType = Type.GetType("System.String")
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
            ' ロック
            cs = flxAttendance.Styles.Add("lightYellowCell")
            cs.BackColor = Color.Yellow
            cs.DataType = Type.GetType("C1.Win.C1FlexGrid.IC1MultiColumnDictionary")

            cs.TextAlign = TextAlignEnum.CenterCenter
            cs.Editor = Nothing
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
            ' 地位喪失脱退社員
            cs = flxAttendance.Styles.Add("FixedFontPink")
            cs.ForeColor = Color.DeepPink
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：SetKenmuStyle
    '   名称　：兼務組合員出欠情報変更不可処理
    '   概要　：中央委員会か支部委員会を兼務している組合員の出欠情報を変更不可にする。
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/29(火)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火)  ryu  新規作成
    '***************************************************************************************************
    ''' <summary>兼務組合員出欠情報変更不可処理</summary>
    ''' <remarks></remarks>
    Private Sub SetKenmuStyle()
        Dim iRowCounter As Integer                              '組合員カウンター
        Dim strComID As String                                  '現在表示されている委員会ID
        Dim strUserID As String                                 '組合員ID
        Dim dbAccess As New CLAccessMdb                         'DBアクセス
        Dim dt As DataTable                                     'データテーブル
        'Dim sql As String                                       'SQL文
        Dim lstBelonComm As Hashtable                           '所属する委員会リスト
        Dim dtrow As DataRow                                    '一行のデータ
        Dim flg As Boolean                                      '兼務しているかのフラグ
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '現在表示されている委員会ID取得
            strComID = cmbCommittee.SelectedValue
            'DBアクセス
            dbAccess.Connect()
            For iRowCounter = 1 To flxAttendance.Rows.Count - 1 Step 2
                '組合員の社員IDを取得
                strUserID = flxAttendance.GetData(iRowCounter, 2)
                If Not (String.IsNullOrEmpty(strComID) Or String.IsNullOrEmpty(strUserID)) Then
                    If strComID <> "" And strUserID <> "" Then
                        flg = False
                        dt = MyCurrentCommittee(dbAccess, strUserID, cmbYear.Text + cmbMonth.Text + "01")
                        If Not dt Is Nothing Then
                            If dt.Rows.Count > 0 Then
                                lstBelonComm = New Hashtable
                                For iCounter As Integer = 0 To dt.Rows.Count - 1
                                    dtrow = dt.Rows(iCounter)
                                    lstBelonComm.Remove(dtrow("c_committee_id"))
                                    lstBelonComm.Add(dtrow("c_committee_id"), dtrow("s_committee_seq"))
                                Next
                                '中央執行委員会を兼務している場合
                                If lstBelonComm.Contains(COMMITTEE_ID_CHUOU) Then
                                    If cmbYear.Text + cmbMonth.Text + "01" < MDLoginInfo.PeriodTo Then
                                        flg = True
                                    End If
                                End If
                                '東京支部委員会を兼務している場合
                                If lstBelonComm.Contains(COMMITTEE_ID_SHIBU1) Then
                                    '表示されている委員会は支部委員会ではない場合
                                    If strComID <> COMMITTEE_ID_SHIBU1 Then
                                        Dim shibuSEQ As String = CStr(lstBelonComm.Item(COMMITTEE_ID_SHIBU1))
                                        '三役の場合
                                        If shibuSEQ = "1" Or shibuSEQ = "2" Or shibuSEQ = "3" Then
                                            flg = True
                                        End If
                                    End If
                                End If
                                '大阪支部委員会を兼務している場合
                                If lstBelonComm.Contains(COMMITTEE_ID_SHIBU2) Then
                                    '表示されている委員会は支部委員会ではない場合
                                    If strComID <> COMMITTEE_ID_SHIBU2 Then
                                        Dim shibuSEQ As String = CStr(lstBelonComm.Item(COMMITTEE_ID_SHIBU2))
                                        '三役の場合
                                        If shibuSEQ = "1" Or shibuSEQ = "2" Or shibuSEQ = "3" Then
                                            flg = True
                                        End If
                                    End If
                                End If
                            End If
                        End If
                        '兼務していなければさらに締切日を確認
                        If Not flg Then
                            '締切日より修正可否を判断
                            If strComID = "019" Or strComID = "029" Then
                                '東京支部と大阪支部の場合、委員会レベルの締切日を取得
                                Select Case flxAttendance.GetData(iRowCounter, 3)
                                    Case "1", "2", "3"
                                        '支部三役の場合
                                        If Not getCloseDateCommOrUser(dbAccess, strComID, strUserID, "02") Then
                                            flg = True
                                        End If
                                    Case Else
                                        '支部三役以外の場合
                                        If Not getCloseDateCommOrUser(dbAccess, strComID, strUserID, "01") Then
                                            flg = True
                                        End If
                                End Select
                            Else
                                If Not getCloseDateCommOrUser(dbAccess, strComID, strUserID) Then
                                    flg = True
                                End If
                            End If
                        End If
                        '兼務あるいは締切日の関係で、該当行を修正不可にする
                        If flg Then
                            flxAttendance.Rows(iRowCounter).AllowEditing = False
                            flxAttendance.Rows(iRowCounter + 1).AllowEditing = False
                            For m = 5 To Me.flxAttendance.Cols.Count - 1 - intDays
                                '内容変更ボタン押下した場合、委員会名が番号で表示してしまう問題の修正　2012/06/13 START
                                Dim tmpCellStyle = flxAttendance.GetCellStyle(iRowCounter, m)
                                Dim objData As Object = flxAttendance.GetData(iRowCounter, m)
                                Dim dataDic As MultiColumnDictionary
                                If Not objData Is Nothing Then
                                    If Not tmpCellStyle Is Nothing Then
                                        If Not tmpCellStyle.DataMap Is Nothing Then
                                            dataDic = tmpCellStyle.DataMap
                                            Dim keyval As String = dataDic.Item(objData).ToString
                                            keyval = keyval.Substring(0, keyval.IndexOf(ControlChars.Tab))
                                            flxAttendance.SetData(iRowCounter, m, keyval)
                                        End If
                                    End If
                                End If
                                '内容変更ボタン押下した場合、委員会名が番号で表示してしまう問題の修正　2012/06/13 END
                                flxAttendance.SetCellStyle(iRowCounter, m, flxAttendance.Styles("lightYellowCell"))
                                flxAttendance.SetCellStyle(iRowCounter + 1, m, flxAttendance.Styles("boolYellow"))
                            Next m
                        End If
                    End If
                End If
            Next
            'DB接続切断
            'dbAccess.Disconnect()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call dbAccess.Disconnect()
            dbAccess = Nothing
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：checkCloseDate
    '   名称　：締切日判断処理
    '   概要　：委員会あるいはユーザーの今期開始日、前々回の締切日と前回締切日の取得
    '   引数　：ByVal dbAccess           As CLAccessMdb = データベースクラス,
    '           ByVal strCommID          As String      = 委員会ID,
    '           Optional ByVal strUserID As String      = ユーザーID,
    '           Optional ByVal pay_kind  As String      = 
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/05(月)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/05(月) ryu  新規作成
    '***************************************************************************************************
    ''' <summary>締切日判断処理</summary>
    ''' <param name="dbAccess">データベースクラス</param>
    ''' <param name="strCommID">委員会ID</param>
    ''' <param name="strUserID">ユーザーID</param>
    ''' <param name="pay_kind"></param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function getCloseDateCommOrUser(ByVal dbAccess As CLAccessMdb,
                                            ByVal strCommID As String,
                                            Optional ByVal strUserID As String = "",
                                            Optional ByVal pay_kind As String = "") As Boolean
        Dim sql As String                                   'SQL文
        Dim dt As DataTable                                 'データテーブル
        Dim strPeriodStart As String                        '現在の期の開始日付
        Dim strPreCloseDate As String                       '前回締切日
        Dim strPrePreCloseDate As String                    '前々回締切日
        Dim lstRtnValue As New List(Of String)              '戻り値リスト
        Dim strYear As String                               '参照年
        Dim strMonth As String                              '参照月
        Dim blnRtn As Boolean                               '戻り値
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            strYear = cmbYear.Text
            strMonth = cmbMonth.Text
            strPeriodStart = MDLoginInfo.PeriodFrom
            lstRtnValue.Add(Replace(strPeriodStart, "/", ""))
            If strUserID <> "" Then
                'ユーザーIDがある場合、ユーザーに対しての締切日を取得
                If strCommID = "019" Or strCommID = "029" Then
                    sql = "select DISTINCT d_daily_pay_close from daily_pay_close_dtl where c_committee_id='" + strCommID + "' and k_daily_pay_kind='" + pay_kind + "'"
                Else
                    sql = "select DISTINCT d_daily_pay_close from call_roll_user_dtl where c_committee_id='" + strCommID + "' and c_user_id='" + strUserID + "'"
                End If
            Else
                'ユーザーIDがない場合、委員会に対しての締切日を取得
                If strCommID = "019" Or strCommID = "029" Then
                    sql = "select DISTINCT d_daily_pay_close from daily_pay_close_dtl where c_committee_id='" + strCommID + "' and k_daily_pay_kind='" + pay_kind + "'"
                Else
                    sql = "select DISTINCT d_daily_pay_close from daily_pay_close_dtl where c_committee_id='" + strCommID + "'"
                End If
            End If
            sql = sql + " order by d_daily_pay_close DESC"  'chk
            dt = dbAccess.ExecuteSql(sql)
            strPreCloseDate = ""
            strPrePreCloseDate = ""
            If dt.Rows.Count = 0 Then
            ElseIf dt.Rows.Count = 1 Then
                strPreCloseDate = IIf(IsDBNull(dt.Rows(0)("d_daily_pay_close")), "", dt.Rows(0)("d_daily_pay_close"))
                lstRtnValue.Add(Replace(strPreCloseDate, "/", ""))
            ElseIf dt.Rows.Count > 1 Then
                strPreCloseDate = IIf(IsDBNull(dt.Rows(0)("d_daily_pay_close")), "", dt.Rows(0)("d_daily_pay_close"))
                strPrePreCloseDate = IIf(IsDBNull(dt.Rows(1)("d_daily_pay_close")), "", dt.Rows(1)("d_daily_pay_close"))
                lstRtnValue.Add(Replace(strPreCloseDate, "/", ""))
                lstRtnValue.Add(Replace(strPrePreCloseDate, "/", ""))
            End If

            '戻り値を初期化
            blnRtn = False

            '前々回締切日取得確認
            If lstRtnValue.Count = 3 Then
                '今期の開始日、前回締切日と前々回締切日を取得できた
                If strYear + strMonth + "01" > lstRtnValue.Item(2) Then
                    '参照年月が前々回締切よりも新しい年月の場合、修正可能にする
                    blnRtn = True
                Else
                    '参照年月は前々回締切よりも古い年月の場合、修正不可にする
                    blnRtn = False
                End If
            ElseIf lstRtnValue.Count < 3 Then
                '前々回締切日は存在しないので修正可能にする
                blnRtn = True
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        '戻り値
        Return blnRtn
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetAttendanceData
    '   名称　：出欠データ取得処理
    '   概要　：出欠データ取得
    '   引数　：ByVal strCommitteeId As String = 委員会ID,
    '           ByVal strDate        As String = 日付
    '   戻り値：DataTable = 出欠データ
    '   作成日：2011/12/12(月)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/12(月)  ryu  新規作成
    '***************************************************************************************************
    ''' <summary>出欠データ取得処理</summary>
    ''' <param name="strCommitteeId">委員会ID</param>
    ''' <param name="strDate">日付</param>
    ''' <returns>出欠データ</returns>
    ''' <remarks></remarks>
    Public Function GetAttendanceData(ByVal strCommitteeId As String,
                                      ByVal strDate As String) As DataTable
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Dim table2 As DataTable = Nothing
        Try
            Dim str2 As String
            Dim str3 As String
            Dim sql As String
            Dim dbAccess As New CLAccessMdb
            Dim dt As New DataTable("attendance_all")
            Dim str As String = (strDate.Substring(0, 6) & DateTime.DaysInMonth(Integer.Parse(strDate.Substring(0, 4)), Integer.Parse(strDate.Substring(4, 2))).ToString)
            If ((DailyPayKind02Id.Length = 0) OrElse (DailyPayKind02Seq.Length = 0)) Then
                str2 = ""
                str3 = ""
            Else
                str2 = " AND ("
                str2 = (str2 & "c_committee_id = '" & DailyPayKind02Id(0) & "' ")
                str3 = " AND ("
                str3 = ((str3 & "(c_committee_id = '" & DailyPayKind02Id(0) & "' ") & "AND (s_committee_seq = '" & DailyPayKind02Seq(0)(0).ToString & "' ")
                Dim i As Integer
                For i = 1 To DailyPayKind02Seq(0).Length - 1
                    str3 = (str3 & " OR s_committee_seq = '" & DailyPayKind02Seq(0)(i).ToString & "' ")
                Next i
                str3 = (str3 & ")) ")
                Dim j As Integer
                For j = 1 To DailyPayKind02Id.Length - 1
                    str2 = (str2 & " OR c_committee_id = '" & DailyPayKind02Id(j) & "' ")
                    str3 = ((str3 & " OR (c_committee_id = '" & DailyPayKind02Id(j) & "' ") & " AND (s_committee_seq = '" & DailyPayKind02Seq(j)(0).ToString & "' ")
                    Dim k As Integer
                    For k = 1 To DailyPayKind02Seq(j).Length - 1
                        str3 = (str3 & " OR s_committee_seq = '" & DailyPayKind02Seq(j)(k).ToString & "' ")
                    Next k
                    str3 = (str3 & ")) ")
                Next j
                str2 = (str2 & ") ")
                str3 = (str3 & ") ")
            End If
            sql = "SELECT committee_list_info.c_committee_list  AS c_committee_list, "
            sql = sql + "committee_list_info.c_user_id       AS c_user_id, "
            sql = sql + "committee_list_info.c_committee_id  AS c_committee_id, "
            sql = sql + "committee_list_info.s_committee_seq AS s_committee_seq, "
            sql = sql + "staf_info.c_staf_id                 AS c_staf_id, "
            sql = sql + "staf_info.l_name                    AS l_name, "
            sql = sql + "staf_info.k_user_status             AS k_user_status, "
            sql = sql + "staf_info.k_belonging               AS k_belonging, "
            sql = sql + "committee_dtl.c_daily_pay_id        AS c_daily_pay_id, "
            sql = sql + "committee_dtl.d_service_from        AS d_service_from, "
            sql = sql + "committee_dtl.d_service_to          AS d_service_to, "
            sql = sql + "committee_dtl.s_from_diff           AS s_from_diff, "
            sql = sql + "committee_dtl.s_to_diff             AS s_to_diff  "
            sql = sql + "FROM   committee_dtl, "
            sql = sql + "(SELECT  committee_list_dtl.c_committee_list, "
            sql = sql + "committee_list_dtl.c_user_id, "
            sql = sql + "committee_list_dtl.d_from, "
            sql = sql + "committee_list_dtl.c_committee_id, "
            sql = sql + "committee_list_dtl.s_committee_seq "
            sql = sql + "FROM    committee_list_dtl, "
            sql = sql + "(SELECT a.c_committee_list,b.c_user_id,b.m_from "
            sql = sql + "FROM  (SELECT   c_committee_list,c_user_id,MAX(d_from) AS m_from "
            sql = sql + "FROM committee_list_dtl "
            sql = sql + "WHERE    c_committee_id = '" + strCommitteeId + "' "
            sql = sql + "AND      d_from        <= '" + str + "' "
            sql = sql + "GROUP BY c_committee_list,c_user_id"
            sql = sql + ") a,"
            sql = sql + "(SELECT   c_user_id,MAX(d_from) AS m_from "
            sql = sql + "FROM committee_list_dtl "
            sql = sql + "WHERE    c_committee_id = '" + strCommitteeId + "' "
            sql = sql + "AND      d_from        <= '" + str + "' "
            sql = sql + "GROUP BY c_user_id "
            sql = sql + ") b "
            sql = sql + "WHERE(a.c_user_id = b.c_user_id) "
            sql = sql + "AND   a.m_from    = b.m_from "
            sql = sql + ") committee_list_dtl_a, "
            sql = sql + "(SELECT   c_committee_list,MAX(d_from) AS m_from "
            sql = sql + "FROM committee_list  "
            sql = sql + "WHERE    c_committee_id = '" + strCommitteeId + "' "
            sql = sql + "AND      c_period_id    = '" + MDLoginInfo.PeriodId + "' "
            sql = sql + "AND      d_from        <= '" + str + "' "
            sql = sql + "AND      c_ksh          = '" + MDLoginInfo.Ksh + "' "
            sql = sql + "GROUP BY c_committee_list "
            sql = sql + ") committee_list_b,  "
            sql = sql + "(SELECT   c_committee_id,c_period_id,c_ksh,MAX(d_from) AS m_from "
            sql = sql + "FROM     committee_list "
            sql = sql + "WHERE    c_committee_id = '" + strCommitteeId + "' "
            sql = sql + "AND      c_period_id    = '" + MDLoginInfo.PeriodId + "' "
            sql = sql + "AND      d_from        <= '" + str + "' "
            sql = sql + "AND      c_ksh          = '" + MDLoginInfo.Ksh + "' "
            sql = sql + "GROUP BY c_committee_id,c_period_id,c_ksh"
            sql = sql + ") committee_list_c "
            sql = sql + "WHERE(committee_list_dtl.c_committee_list = committee_list_b.c_committee_list) "
            sql = sql + "AND    committee_list_dtl.d_from           = committee_list_b.m_from "
            sql = sql + "AND    committee_list_b.m_from             = committee_list_c.m_from "
            sql = sql + "AND    committee_list_dtl.c_committee_list = committee_list_dtl_a.c_committee_list "
            sql = sql + "AND    committee_list_dtl.c_user_id        = committee_list_dtl_a.c_user_id "
            sql = sql + "AND    committee_list_dtl.d_from           = committee_list_dtl_a.m_from "
            sql = sql + ") committee_list_info,"
            sql = sql + "(SELECT   staf_attribute.* "
            sql = sql + "FROM     staf_attribute,"
            sql = sql + "(SELECT  c_user_id, MAX(d_from) AS c_d_from "
            sql = sql + "FROM staf_attribute "
            sql = sql + "WHERE   k_del = '0' "
            sql = sql + "AND d_from <= '" + str + "' "
            sql = sql + "GROUP BY c_user_id "
            'sql = sql + "ORDER BY c_user_id "
            sql = sql + ") staf_attribute_A "
            sql = sql + "WHERE(staf_attribute.c_user_id = staf_attribute_A.c_user_id) "
            sql = sql + "AND     staf_attribute.d_from    = staf_attribute_A.c_d_from "
            sql = sql + "AND     staf_attribute.k_del     = '0' "
            sql = sql + ") staf_info "
            sql = sql + "WHERE(committee_list_info.c_user_id = staf_info.c_user_id) "
            sql = sql + "AND    committee_list_info.c_committee_id  = committee_dtl.c_committee_id "
            sql = sql + "AND    committee_list_info.s_committee_seq = committee_dtl.s_committee_seq "
            sql = sql + "AND    committee_dtl.d_from <= '" + str + "+' AND committee_dtl.d_to >= '" + str + "' "
            sql = sql + "ORDER BY committee_list_info.s_committee_seq,staf_info.k_belonging,CLng(staf_info.c_staf_id)" & UtDb.DbOrderOffset 'ok
            Call dbAccess.Connect()

            dt = dbAccess.ExecuteSql(sql)
            Call dbAccess.Disconnect()
            If dt.Rows.Count = 0 Then
                Return Nothing
            Else
                dt.TableName = "attendance_all"
                table2 = dt
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Return table2
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetBelongCommittee
    '   名称　：所属委員会取得処理
    '   概要　：所属委員会取得
    '   引数　：ByVal dbAccess  As CLAccessMdb = データベースクラス,
    '           ByVal strKsh    As String      = 会社コード,
    '           ByVal strUserId As String      = ユーザー認証ID,
    '           ByVal strDate   As String      = 日付
    '   戻り値：DataTable = 所属委員会
    '   作成日：2011/12/12(月)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/12(月)  ryu  新規作成
    '***************************************************************************************************
    ''' <summary>所属委員会取得</summary>
    ''' <param name="dbAccess">データベースクラス</param>
    ''' <param name="strKsh">会社コード</param>
    ''' <param name="strUserId">ユーザー認証ID</param>
    ''' <param name="strDate">日付</param>
    ''' <returns>所属委員会</returns>
    ''' <remarks></remarks>
    Public Function GetBelongCommittee(ByVal dbAccess As CLAccessMdb,
                                       ByVal strKsh As String,
                                       ByVal strUserId As String,
                                       ByVal strDate As String) As DataTable
        Dim table2 As DataTable
        table2 = Nothing
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim sql As String = "SELECT  PER.d_from AS PERIOD_DFROM,  PER.d_to AS PERIOD_DTO,  COMDTL.s_from_diff,  COMDTL.s_to_diff,  COMLISTDTL.c_committee_list,  COMLISTDTL.c_user_id,  COMLISTDTL.d_from,  COMLISTDTL.c_committee_id,  COMLISTDTL.s_committee_seq,  COMLISTDTL.l_biko,  COMLISTDTL.d_ins,  COMLISTDTL.c_user_id_ins,  COMLISTDTL.d_up,  COMLISTDTL.c_user_id_up,  COMLISTDTL.s_up,  COMLIST_B.c_period_id  FROM "
            sql = sql + "(select c_committee_id,c_period_id,max(d_from) as a_d_from  from committee_list where c_ksh = '" + strKsh + "' and d_from    <= '" + strDate + "'   group by c_committee_id, c_period_id ) COMLIST_A,  committee_list COMLIST_B,     committee_list_dtl COMLISTDTL,  committee_dtl COMDTL,  period PER "
            sql = sql + "where COMLIST_A.a_d_from = COMLIST_B.d_from and COMLIST_A.c_committee_id   = COMLIST_B.c_committee_id     and COMLIST_A.c_period_id      = COMLIST_B.c_period_id        and COMLIST_B.c_ksh            = '" + strKsh + "' and COMLIST_B.c_committee_list = COMLISTDTL.c_committee_list   and COMLIST_B.d_from           = COMLISTDTL.d_from             and COMLISTDTL.c_user_id       = '" + strUserId + "'  AND COMLISTDTL.c_committee_id  = COMDTL.c_committee_id   AND COMLISTDTL.s_committee_seq = COMDTL.s_committee_seq   AND COMDTL.d_from             <= COMLIST_B.d_from   AND COMLIST_B.d_from          <= COMDTL.d_to   AND PER.c_period_id = COMLIST_B.c_period_id   AND PER.k_period_kind = '01'  order by COMLISTDTL.c_committee_id" & UtDb.DbOrderOffset
            Dim dt As New DataTable("committee_list_dtl")
            dt = dbAccess.ExecuteSql(sql)
            table2 = dt
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Return table2
    End Function

    '***************************************************************************************************
    '   ＩＤ　：MyCurrentCommittee
    '   名称　：現在所属委員会取得処理
    '   概要　：現在の所属委員会取得
    '   引数　：ByVal dbAccess  As CLAccessMdb = データベースクラス,
    '           ByVal strUserId As String      = ユーザー認証ID,
    '           ByVal strDate   As String      = 日付
    '   戻り値：DataTable = 所属委員会
    '   作成日：2011/12/12(月)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/12(月)  ryu  新規作成
    '***************************************************************************************************
    ''' <summary>現在所属委員会取得処理</summary>
    ''' <param name="dbAccess">データベースクラス</param>
    ''' <param name="strUserId">ユーザー認証ID</param>
    ''' <param name="strDate">日付</param>
    ''' <returns>所属委員会</returns>
    ''' <remarks></remarks>
    Public Function MyCurrentCommittee(ByVal dbAccess As CLAccessMdb,
                                       ByVal strUserId As String,
                                       ByVal strDate As String) As DataTable
        Dim table3 As DataTable = Nothing
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim strA As String = strDate.Substring(0, 6)
            Dim ksh As String = MDLoginInfo.Ksh
            Dim table As DataTable = GetBelongCommittee(dbAccess, ksh, strUserId, strDate)
            If table Is Nothing Then
                Return Nothing
            End If
            Dim table2 As New DataTable("current_committee_list")
            table2.Columns.Add("c_period_id", GetType(String))
            table2.Columns.Add("c_committee_list", GetType(String))
            table2.Columns.Add("c_user_id", GetType(String))
            table2.Columns.Add("c_committee_id", GetType(String))
            table2.Columns.Add("s_committee_seq", GetType(Integer))
            table2.Columns.Add("d_from", GetType(String))
            Dim i As Integer
            For i = 0 To table.Rows.Count - 1
                Dim s As String = table.Rows.Item(i).Item("period_dfrom").ToString
                Dim str7 As String = table.Rows.Item(i).Item("period_dto").ToString
                Dim time As DateTime = DateTime.ParseExact(s, "yyyyMMdd", Nothing)
                Dim time2 As DateTime = DateTime.ParseExact(str7, "yyyyMMdd", Nothing)
                Dim strB As String = time.AddMonths(Convert.ToInt32(table.Rows.Item(i).Item("s_from_diff").ToString)).ToString("yyyyMM")
                Dim str5 As String = time2.AddMonths(Convert.ToInt32(table.Rows.Item(i).Item("s_to_diff").ToString)).ToString("yyyyMM")
                If ((String.Compare(strA, strB) >= 0) AndAlso (String.Compare(strA, str5) <= 0)) Then
                    Dim row As DataRow = table2.NewRow
                    Dim j As Integer
                    For j = 0 To table.Columns.Count - 1
                        Dim str As String = table.Columns.Item(j).ToString
                        Dim k As Integer
                        For k = 0 To table2.Columns.Count - 1
                            Dim str2 As String = table2.Columns.Item(k).ToString
                            If str.Equals(str2) Then
                                row.Item(str2) = table.Rows.Item(i).Item(str)
                                Exit For
                            End If
                        Next k
                    Next j
                    table2.Rows.Add(row)
                End If
            Next i
            table3 = table2
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Return table3
    End Function

    '***************************************************************************************************
    '   ＩＤ　：searchMain
    '   名称　：検索処理
    '   概要　：検索の主処理
    '   引数　：
    '   戻り値：Boolean 正常/異常
    '   作成日：2012/02/28(火) Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/28(火) Fujisaku  新規作成
    '***************************************************************************************************
    Public Function searchMain() As Boolean
        Dim retBln As Boolean = False                           '戻り値
        Dim strYear As String                                   '年
        Dim strMonth As String                                  '月
        'Dim intDays As Integer                                  '日数字
        Dim iCounter As Integer                                 'カラムカウンター
        Dim searchDate As Date                                  '日付
        Dim strDay As String                                    '日の文字列
        Dim dbAccess As New CLAccessMdb                         'DBアクセス
        Dim dt As DataTable                                     'データテーブル
        Dim dtrow As DataRow                                    '一行のデータ
        Dim sql As String                                       'SQL分
        Dim iRowCounter As Integer                              '行数カウンター
        Dim dtAtt As DataTable                                  '出欠情報データテーブル
        Dim dtrowAtt As DataRow                                 '一行のデータ
        Dim strCellStyle As String                              '委員会IDと枝番でセルスタイルと変える
        Dim strCellStyleBlue As String
        Dim oneCell As CellStyle                                'セルスタイル
        Dim iRowCounterRoll As Integer                          '行カウンター
        Dim dtRoll As DataTable                                 '日当データテーブル
        Dim dtrowRoll As DataRow                                '一行のデータ
        Dim strDate As String                                   '出欠のあった日付
        Dim intDay As Integer                                   '日
        Dim dtLName As DataTable                                'データテーブル
        Dim dtrowLName As DataRow                               '一行のデータ
        Dim intDaysList As List(Of String)                      '出欠のある日リスト
        Dim strDateStart As String                              '月の開始日付
        Dim strDateEnd As String                                '月の終了日付
        Dim strDateStand As String                              '月の開始日(yyyyMMdd型)

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '入力チェック
            If CheckInput() = False Then
                Exit Function
            End If
            '入力チェック通った場合、検索を行う
            FrmWaitInfo.ShowWaitForm(Nothing)
            strYear = cmbYear.Text
            strMonth = cmbMonth.Text
            intDays = Date.DaysInMonth(CInt(strYear), CInt(strMonth))
            strDateStand = strYear + strMonth + "01"
            strDateStart = strYear + "/" + strMonth + "/" + "01"
            strDateEnd = strYear + "/" + strMonth + "/" + CStr(intDays)
            flxAttendance.Rows.Count = 1
            flxAttendance.Cols.Count = intDays + 5 + intDays
            flxAttendance.Redraw = False
            flxAttendance.AllowMerging = AllowMergingEnum.Free
            flxAttendance.Cols(0).AllowMerging = True
            flxAttendance.Cols(1).AllowMerging = True
            'ヘッダカラムのスタイルを設定
            For iCounter = 1 To intDays
                searchDate = Date.Parse(strYear + "/" + strMonth + "/" + CStr(iCounter))
                strDay = CStr(iCounter) + KANJI_YOBI_NICHI
                flxAttendance.SetData(0, iCounter + 4, strDay, False)
                flxAttendance.SetCellStyle(0, iCounter + 4, flxAttendance.Styles.SelectedRowHeader)
                Select Case Weekday(searchDate)
                    Case 1
                        '日曜日
                        flxAttendance.SetCellStyle(0, iCounter + 4, "yobi7")
                    Case 7
                        '土曜日
                        flxAttendance.SetCellStyle(0, iCounter + 4, "yobi6")
                    Case Else
                End Select
            Next
            '委員会に所属する組合員をリストアップ
            Call dbAccess.Connect()
            dt = GetAttendanceData(cmbCommittee.SelectedValue, strYear + strMonth + "01")

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    flxAttendance.Rows.Add(dt.Rows.Count * 2)
                    For iRowCounter = 0 To dt.Rows.Count - 1
                        For iCounter = 1 To intDays
                            If iRowCounter Mod 2 = 0 Then
                                flxAttendance.SetCellStyle(iRowCounter * 2 + 1, iCounter + 4, "normal")
                                flxAttendance.SetCellStyle(iRowCounter * 2 + 2, iCounter + 4, "normal")
                            Else
                                flxAttendance.SetCellStyle(iRowCounter * 2 + 1, iCounter + 4, "boollightcyan")
                                flxAttendance.SetCellStyle(iRowCounter * 2 + 2, iCounter + 4, "boollightcyan")
                            End If
                        Next
                    Next
                    Dim str As String = (Me.cmbYear.SelectedItem.ToString.PadLeft(4, "0"c) & Me.cmbMonth.SelectedItem.ToString.PadLeft(2, "0"c) & "01")
                    For iRowCounter = 0 To dt.Rows.Count - 1
                        dtrow = dt.Rows(iRowCounter)
                        '社員番号
                        flxAttendance.SetData(iRowCounter * 2 + 1, 0, dtrow("c_staf_id"))
                        flxAttendance.SetData(iRowCounter * 2 + 2, 0, dtrow("c_staf_id"))
                        '名前
                        flxAttendance.SetData(iRowCounter * 2 + 1, 1, dtrow("l_name"))
                        flxAttendance.SetData(iRowCounter * 2 + 2, 1, dtrow("l_name"))
                        If dtrow("k_user_status").ToString.Equals("01") Then
                            Me.flxAttendance.SetCellStyle(iRowCounter * 2 + 1, 0, "Fixed")
                            Me.flxAttendance.SetCellStyle(iRowCounter * 2 + 2, 0, "Fixed")
                            Me.flxAttendance.SetCellStyle(iRowCounter * 2 + 1, 1, "Fixed")
                            Me.flxAttendance.SetCellStyle(iRowCounter * 2 + 2, 1, "Fixed")
                        Else
                            Me.flxAttendance.SetCellStyle(iRowCounter * 2 + 1, 0, "FixedFontPink")
                            Me.flxAttendance.SetCellStyle(iRowCounter * 2 + 2, 0, "FixedFontPink")
                            Me.flxAttendance.SetCellStyle(iRowCounter * 2 + 1, 1, "FixedFontPink")
                            Me.flxAttendance.SetCellStyle(iRowCounter * 2 + 2, 1, "FixedFontPink")
                        End If
                        '個人認証ID
                        flxAttendance.SetData(iRowCounter * 2 + 1, 2, dtrow("c_user_id"))
                        flxAttendance.SetData(iRowCounter * 2 + 2, 2, dtrow("c_user_id"))
                        '委員会ＩＤ枝番（役職ID）
                        flxAttendance.SetData(iRowCounter * 2 + 1, 3, dtrow("s_committee_seq"))
                        flxAttendance.SetData(iRowCounter * 2 + 2, 3, dtrow("s_committee_seq"))
                        'セルスタイル
                        strCellStyle = cmbCommittee.SelectedValue + CStr(dtrow("s_committee_seq"))
                        strCellStyleBlue = cmbCommittee.SelectedValue + CStr(dtrow("s_committee_seq")) + "blue"
                        '役職に付き日当IDと日当枝番（日当選択肢）を取得
                        sql = "select daily_pay_master_dtl.c_daily_pay_id,daily_pay_master_dtl.c_menu_seq,daily_pay_master_dtl.l_name,daily_pay_master_dtl.l_explain from daily_pay_master_dtl "
                        sql = sql & "right join (select c_daily_pay_id from committee_dtl where c_committee_id='" + cmbCommittee.SelectedValue + "' and s_committee_seq='" + CStr(dtrow("s_committee_seq")) + "' and d_from <= '" & strDateStand & "' and d_to >= '" & strDateStand & "' )as pay_id on daily_pay_master_dtl.c_daily_pay_id=pay_id.c_daily_pay_id "
                        sql = sql & "where daily_pay_master_dtl.d_from <= '" & strDateStand & "' and daily_pay_master_dtl.d_to >= '" & strDateStand & "'"
                        dtAtt = dbAccess.ExecuteSql(sql)
                        If dtAtt.Rows.Count > 0 Then
                            dtrowAtt = dtAtt.Rows(0)
                            flxAttendance.SetData(iRowCounter * 2 + 1, 4, dtrowAtt("c_daily_pay_id"))
                            flxAttendance.SetData(iRowCounter * 2 + 2, 4, dtrowAtt("c_daily_pay_id"))
                            '該当委員会IDと委員会枝番でセルスタイル作成
                            If Not flxAttendance.Styles.Contains(strCellStyle) Then
                                oneCell = flxAttendance.Styles.Add(strCellStyle)
                                oneCell.TextAlign = TextAlignEnum.CenterCenter
                                '空白行を追加
                                Dim newRow As DataRow = dtAtt.NewRow
                                newRow(0) = " "
                                newRow(1) = " "
                                newRow(2) = " "
                                newRow(3) = " "
                                dtAtt.Rows.InsertAt(newRow, 0)
                                Dim columnNames As String() = New String() {"l_name", "l_explain"}
                                Dim dictionary As New MultiColumnDictionary(dtAtt, "c_menu_seq", columnNames, 0)
                                oneCell.DataMap = dictionary
                                oneCell.Border.Direction = BorderDirEnum.Both
                                oneCell.Border.Color = Color.Black
                                '背景はブルーにするセル
                                oneCell = flxAttendance.Styles.Add(strCellStyleBlue)
                                oneCell.TextAlign = TextAlignEnum.CenterCenter
                                oneCell.BackColor = Color.LightCyan
                                oneCell.DataMap = dictionary
                                oneCell.Border.Direction = BorderDirEnum.Both
                                oneCell.Border.Color = Color.Black
                            End If
                        Else
                            CLMsg.Show("GE0004", DAILYPAY_MASTER_INFO)
                            Exit Function
                        End If
                        intDaysList = New List(Of String)
                        '登録してある出欠情報をユーザー毎に取得し、その日の出欠情報（違う委員会の活動が存在するので）にセル毎のスタイルをあわせる
                        sql = "select s_day,call_roll_user_dtl.c_committee_id as c_committee_id,s_committee_seq,c_daily_pay_id,c_menu_seq,k_food_expenses,l_name from "
                        sql = sql & "(select format(call_roll_user_dtl.s_day,'yyyy/MM/dd')as s_day,call_roll_user_dtl.c_committee_id,call_roll_user_dtl.s_committee_seq,call_roll_user_dtl.c_daily_pay_id,call_roll_user_dtl.c_menu_seq,call_roll_user_dtl.k_food_expenses "
                        sql = sql & "from call_roll_user_dtl where call_roll_user_dtl.c_user_id='" + dtrow("c_user_id") + "' and format(call_roll_user_dtl.s_day,'yyyy/MM/dd')>='" + strDateStart + "' and format(call_roll_user_dtl.s_day,'yyyy/MM/dd')<='" + strDateEnd + "'"
                        sql = sql & ")as call_roll_user_dtl left join committee on call_roll_user_dtl.c_committee_id=committee.c_committee_id"
                        dtRoll = dbAccess.ExecuteSql(sql)
                        If dtRoll.Rows.Count > 0 Then
                            '日当IDを記録
                            For iRowCounterRoll = 0 To dtRoll.Rows.Count - 1
                                dtrowRoll = dtRoll.Rows(iRowCounterRoll)
                                strDate = dtrowRoll("s_day")
                                intDay = Date.Parse(strDate).Day
                                intDaysList.Add(CStr(intDay))
                                '委員会名称をセット2012/06/13 START
                                If Not IsDBNull(dtrowRoll("l_name")) Then
                                    flxAttendance.SetData(iRowCounter * 2 + 1, intDay + intDays + 4, CStr(dtrowRoll("l_name")))
                                End If
                                'flxAttendance.SetData(iRowCounter * 2 + 1, intDay + intDays + 4, CStr(dtrowRoll("l_name")))
                                '委員会名称をセット2012/06/13 END
                                '委員会枝番は見ない 2017/09/04
                                '現在委員会での活動の場合
                                If CStr(dtrowRoll("c_committee_id")) = cmbCommittee.SelectedValue Then
                                    If iRowCounter Mod 2 = 0 Then
                                        flxAttendance.SetCellStyle(iRowCounter * 2 + 1, intDay + 4, strCellStyleBlue)
                                        flxAttendance.SetCellStyle(iRowCounter * 2 + 2, intDay + 4, "boollightcyan")
                                    Else
                                        flxAttendance.SetCellStyle(iRowCounter * 2 + 1, intDay + 4, strCellStyle)
                                        flxAttendance.SetCellStyle(iRowCounter * 2 + 2, intDay + 4, "bool")
                                    End If
                                    flxAttendance.SetData(iRowCounter * 2 + 1, intDay + 4, dtrowRoll("c_menu_seq"))
                                    '昼食あり
                                    If Not IsDBNull(dtrowRoll("k_food_expenses")) Then
                                        If CStr(dtrowRoll("k_food_expenses")) = "1" Then
                                            flxAttendance.SetCellCheck(iRowCounter * 2 + 2, intDay + 4, CheckEnum.Checked)
                                        Else
                                            flxAttendance.SetCellCheck(iRowCounter * 2 + 2, intDay + 4, CheckEnum.Unchecked)
                                        End If
                                    Else
                                        flxAttendance.SetCellCheck(iRowCounter * 2 + 2, intDay + 4, CheckEnum.Unchecked)
                                    End If
                                Else
                                    '他の部／委員会での活動の場合、記号が表示され、セルは修正不可にする
                                    flxAttendance.SetCellStyle(iRowCounter * 2 + 1, intDay + 4, "lightYellowCell")
                                    flxAttendance.SetCellStyle(iRowCounter * 2 + 2, intDay + 4, "boolYellow")
                                    sql = "select l_name from daily_pay_master_dtl where c_daily_pay_id='" + dtrowRoll("c_daily_pay_id") + "' and c_menu_seq='" + dtrowRoll("c_menu_seq") + "' and d_from<='" + Replace(strDate, "/", "") + "' and d_to>='" + Replace(strDate, "/", "") + "'"
                                    dtLName = dbAccess.ExecuteSql(sql)
                                    If dtLName.Rows.Count > 0 Then
                                        dtrowLName = dtLName.Rows(0)
                                        flxAttendance.SetData(iRowCounter * 2 + 1, intDay + 4, CStr(dtrowLName("l_name")))
                                    End If
                                    '昼食あり
                                    If Not IsDBNull(dtrowRoll("k_food_expenses")) Then
                                        If CStr(dtrowRoll("k_food_expenses")) = "1" Then
                                            flxAttendance.SetCellCheck(iRowCounter * 2 + 2, intDay + 4, CheckEnum.Checked)
                                        Else
                                            flxAttendance.SetCellCheck(iRowCounter * 2 + 2, intDay + 4, CheckEnum.Unchecked)
                                        End If
                                    Else
                                        flxAttendance.SetCellCheck(iRowCounter * 2 + 2, intDay + 4, CheckEnum.Unchecked)
                                    End If
                                End If
                            Next
                        End If
                        'ドロップダウンセル設定とチェックボックスセルを設定
                        For iCounter = 1 To intDays
                            If intDaysList.IndexOf(CStr(iCounter)) < 0 Then
                                If iRowCounter Mod 2 = 0 Then
                                    flxAttendance.SetCellStyle(iRowCounter * 2 + 1, iCounter + 4, strCellStyleBlue)
                                    flxAttendance.SetCellStyle(iRowCounter * 2 + 2, iCounter + 4, "boollightcyan")
                                Else
                                    flxAttendance.SetCellStyle(iRowCounter * 2 + 1, iCounter + 4, strCellStyle)
                                    flxAttendance.SetCellStyle(iRowCounter * 2 + 2, iCounter + 4, "bool")
                                End If
                            End If
                        Next
                    Next
                End If
            End If
            '個人認証ID、委員会ＩＤ枝番（役職ID）カラムを非表示
            flxAttendance.Cols(2).Visible = False
            flxAttendance.Cols(3).Visible = False
            flxAttendance.Cols(4).Visible = False
            Dim nnn As Integer

            For nnn = 5 To intDays + 4
                flxAttendance.Cols(nnn).Visible = True
            Next

            For nnn = intDays + 5 To intDays + 4 + intDays
                flxAttendance.Cols(nnn).Visible = False
                flxAttendance.Cols.Item(nnn).AllowMerging = False
            Next

            '編集不可
            flxAttendance.AllowEditing = False
            'サイズ調整
            flxAttendance.Cols.Item(0).Width = 100
            flxAttendance.Cols.Item(1).Width = &H87
            flxAttendance.Cols.Item(0).TextAlign = TextAlignEnum.RightCenter
            flxAttendance.Cols.Item(1).TextAlign = TextAlignEnum.LeftCenter
            flxAttendance.Rows.Item(0).TextAlign = TextAlignEnum.CenterCenter
            '一列と二列目以外はマージさせない
            flxAttendance.Cols.Item(0).AllowMerging = True
            flxAttendance.Cols.Item(1).AllowMerging = True
            Dim m As Integer
            For m = 2 To Me.flxAttendance.Cols.Count - 1 - intDays
                flxAttendance.Cols.Item(m).Width = 60
                flxAttendance.Cols.Item(m).TextAlign = TextAlignEnum.CenterCenter
                flxAttendance.Cols.Item(m).AllowMerging = False
            Next m
            '属性設定
            flxAttendance.SelectionMode = SelectionModeEnum.Cell
            flxAttendance.AllowDragging = AllowDraggingEnum.None
            flxAttendance.AllowResizing = AllowResizingEnum.None
            flxAttendance.AllowSorting = AllowSortingEnum.None
            flxAttendance.AutoResize = False
            '内容変更ボタン表示するか判断処理
            If cmbCommittee.SelectedValue = "019" Or cmbCommittee.SelectedValue = "029" Then
                btnUpdate.Visible = getCloseDateCommOrUser(dbAccess, cmbCommittee.SelectedValue, "", "01") Or getCloseDateCommOrUser(dbAccess, cmbCommittee.SelectedValue, "", "02")
            Else
                btnUpdate.Visible = getCloseDateCommOrUser(dbAccess, cmbCommittee.SelectedValue, "", "")
            End If
            '所属委員がいないとき、内容変更ボタンを非表示
            If flxAttendance.Rows.Count <= 1 Then
                btnUpdate.Visible = False
            End If
            'GRID再描化
            flxAttendance.Redraw = True
            grpResult.Visible = True
            '戻り値設定
            retBln = True
            'Catch ex As Exception
            '    ' ログ出力（致命的エラー）
            '    log.Fatal(ex.Message)
            '    ' 致命的エラーメッセージボックス表示
            '    Call CLMsg.ShowEtarnal(Err.Number, _
            '                           Err.Description, _
            '                           SCREEN_ID, SCREEN_NAME, _
            '                           System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call dbAccess.Disconnect()
            dbAccess = Nothing
            FrmWaitInfo.CloseWaitForm()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Return retBln
    End Function
#End Region

    Private Sub flxAttendance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flxAttendance.Click

    End Sub
End Class

#End Region
