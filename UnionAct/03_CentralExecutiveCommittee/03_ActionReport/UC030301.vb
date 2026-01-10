#Region "UC030301"
'===========================================================================================================
'   クラスＩＤ　　：UC030301
'   クラス名称　　：中執活動報告
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports C1.Win.C1FlexGrid
Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common

Public Class UC030301

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 名称・コード
    Private Const COMMITTEE_NAME_CENTER As String = "中央執行委員会"    ' 中央執行委員会
    Private Const COMMITTEE_ID_CENTER As String = "001"                 ' 中央執行委員会
    ' ステータス
    Private Const STATUS_SEARCH As Byte = 1                             ' 検索
    Private Const STATUS_INSERT As Byte = 2                             ' 登録
    Private Const STATUS_UPDATE As Byte = 3                             ' 更新
    Private Const STATUS_DELETE As Byte = 4                             ' 削除
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC030301              ' UC030301
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC030301          ' 中執活動報告画面

    Private Const COMMITTEE_KI_KIKAN_NAME As String = "期の期間"        ' 名称：期の期間
    Private Const COMMITTEE_MASTER_INFO = "委員会組織明細マスタの情報"  ' 名称
    Private Const COMMITTEE_COMBO_COMMITTEE = "部／委員会"              ' 名称
    Private Const COMMITTEE_COMBO_YEAR = "登録月　年"                   ' 名称
    Private Const COMMITTEE_COMBO_MONTH = "登録月　月"                  ' 名称
    Private Const DAILYPAY_MASTER_INFO = "日当明細マスタの情報"         ' 名称
    Private strPreYear As String                                        ' 現在選択した年
    Private strPreMonth As String                                       ' 現在選択した月
    Private DailyPayKind02Id As String() = New String() {"019", "029"}
    Private DailyPayKind02Seq As Integer()() = New Integer()() {New Integer() {1, 2, 3}, New Integer() {1, 2, 3}}
    ' 権限
    Private strGrantReference As String = "0"                           ' 参照権限
    Private strGrantInsert As String = "0"                              ' 登録権限
    Private strGrantPrint As String = "0"                               ' 印刷権限
    Private strGrantFileOutput As String = "0"                          ' ファイル出力権限
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC030301_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC030301_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim sql As String                                                                   ' SQL文
        Dim dbAccess As New CLAccessMdb                                                     ' DBアクセス
        Dim dt As DataTable                                                                 ' データテーブル
        Dim dtrow As DataRow                                                                ' 一行のデータ
        Try
            '-------------------------------------------------------------------------------
            '   権限取得処理
            '-------------------------------------------------------------------------------
            If setGrant() = False Then
                Exit Sub
            End If
            Me.cmbYear.Enabled = True
            Me.cmbMonth.Enabled = True
            Me.txtCommittee.Text = "中央執行委員会"

            ' 年コンボボックス作成
            If CreateCboYear(Me.cmbYear) = False Then
                Exit Sub
            End If
            ' 月コンボボックス作成
            If CreateCboMonth(Me.cmbMonth, Me.cmbYear.Text) = False Then                    ' コンボボックス（月）
                Exit Sub
            End If

            ' 現在日付表示
            Me.cmbYear.SelectedIndex = Me.cmbYear.FindString(Now.Year.ToString)
            Me.cmbMonth.SelectedIndex = Me.cmbMonth.FindString(Now.Month.ToString.PadLeft(2, "0"c))

            Call Utilities.SetCanEditToControl(False, Me.chkDownMemo)

            Me.btnSearch.Enabled = True
            Me.btnUpdate.Visible = False
            Me.btnCancel.Visible = False
            Me.btnInsertChk.Visible = False
            'セルスタイルを定義
            Call DefinitionStyles()
            strPreYear = Me.cmbYear.Text
            strPreMonth = Me.cmbMonth.Text
            sql = "SELECT c_daily_pay_id FROM daily_pay_master WHERE c_ksh ='" + MDLoginInfo.Ksh + "' GROUP BY c_daily_pay_id ORDER BY c_daily_pay_id"  'chk
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            dbAccess.Disconnect()
            If dt.Rows.Count > 0 Then
                For iCounter = 0 To dt.Rows.Count - 1
                    dtrow = dt.Rows(iCounter)
                    flxAttendance.Styles.Add(("ComboBox" & dtrow("c_daily_pay_id"))).BackColor = SystemColors.Window
                    flxAttendance.Styles.Add(("ComboBoxYellow" & dtrow("c_daily_pay_id"))).BackColor = Color.LightYellow
                    flxAttendance.Styles.Add(("ComboBoxHighlight" & dtrow("c_daily_pay_id"))).BackColor = Color.Azure
                    'Dim style As CellStyle = Me.flxAttendance.Styles.Add(("ColorsWindow" & dtrow("c_daily_pay_id")))
                    'style.BackColor = SystemColors.Window
                    'style.TextAlign = TextAlignEnum.CenterCenter
                    'style = Me.flxAttendance.Styles.Add(("ColorsYellow" & dtrow("c_daily_pay_id")))
                    'style.BackColor = Color.LightYellow
                    'style.TextAlign = TextAlignEnum.CenterCenter
                    'style = Me.flxAttendance.Styles.Add(("ColorsHighlight" & dtrow("c_daily_pay_id")))
                    'style.BackColor = Color.Azure
                    'style.TextAlign = TextAlignEnum.CenterCenter
                Next
            End If
            ''-------------------------------------------------------------------------------
            ''   コントロールクリア処理
            ''-------------------------------------------------------------------------------
            'If ControlClear() = False Then
            '    Exit Sub
            'End If
            ''-------------------------------------------------------------------------------
            ''   セルスタイルを定義
            ''-------------------------------------------------------------------------------
            'Call DefinitionStyles()
            ''-------------------------------------------------------------------------------
            ''   各データ取得処理
            ''-------------------------------------------------------------------------------
            'If GetData() = False Then
            '    Exit Sub
            'End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/10(木)  Ryu
    '   更新日：
    '   備考  ：
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
    '   概要  ：
    '   作成日：2011/11/25(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
            Cursor.Current = Cursors.WaitCursor                                             ' マウスポイント砂時計
            FrmWaitInfo.ShowWaitForm(Nothing)                                               ' しばらくお待ちくださいフォーム表示
            Me.btnUpdate.Visible = False                                                    ' 内容更新ボタン非表示
            Me.btnSearch.Enabled = False                                                    ' 検索ボタン
            Me.btnInsertChk.Visible = True                                                  ' 登録確認ボタン
            Me.btnCancel.Visible = True                                                     ' キャンセルボタン
            Me.txtCommittee.Enabled = False                                                 ' 委員会テキストボックス
            Me.cmbYear.Enabled = False                                                      ' 年コンボボックス
            Me.cmbMonth.Enabled = False                                                     ' 月コンボボックス
            Me.flxAttendance.AllowEditing = True                                                ' グリッド編集可能
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            FrmWaitInfo.CloseWaitForm()                                                     ' しばらくお待ちくださいフォーム非表示
            Cursor.Current = Cursors.Default                                                ' マウスポイントデフォルト
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnInsertChk_Click
    '   名称　：登録確認ボタンクリック処理
    '   概要　：
    '   作成日：2011/12/10(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/10(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnInsertChk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertChk.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim strSql As String                                                                ' SQL文
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable                                                                 ' データテーブル
        Dim rowCounter As Integer                                                           ' 行カウンター
        Dim colCounter As Integer                                                           ' 列カウンター
        Dim objValue As Object                                                              ' セルの値
        Dim strAttValue As String                                                           ' セルの値、出欠種類
        Dim strYearMonth As String                                                          ' 年月
        Dim strDate As String                                                               ' 日付
        Dim strCellStyle As String                                                          ' セルスタイル名
        Dim intRtn As Integer                                                               ' SQL文の実行結果
        Dim bytStatus As Byte = Nothing                                                     ' ステータス（1：検索, 2：登録, 3：更新, 4：削除）
        Dim notCheck As Boolean = False                                                     ' 出欠チェック状況
        Try
            ' 入力確認、昼食チェックした日は出欠が未チェックの場合、メッセージ表示し処理中断
            For rowCounter = 2 To (flxAttendance.Rows.Count - 1) Step 2
                For colCounter = 5 To flxAttendance.Cols.Count - 1
                    Dim launchData As Object = flxAttendance.GetData(rowCounter, colCounter)
                    ' 昼食チェックしてある
                    If launchData Then
                        Dim attendanceData As Object = flxAttendance.GetData(rowCounter - 1, colCounter)
                        If attendanceData Is Nothing Then
                            notCheck = True
                        ElseIf String.IsNullOrEmpty(Trim(attendanceData.ToString)) Then
                            notCheck = True
                        End If
                        If notCheck Then
                            Call MessageBox.Show(flxAttendance.GetData(rowCounter, 1) + "さんの　" + flxAttendance.GetData(0, colCounter) + "　の出欠状況は未入力です。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
                            'MsgBox(flxAttendance.GetData(rowCounter, 1) + "さんの　" + flxAttendance.GetData(0, colCounter) + "　の出欠状況は未入力です。", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "入力エラー")
                            Dim rg As CellRange = flxAttendance.GetCellRange(rowCounter - 1, colCounter, rowCounter - 1, colCounter)
                            flxAttendance.ShowCell(rowCounter - 1, colCounter)
                            Exit Sub
                        End If
                    End If
                Next
            Next
            ' 確認ダイアログ
            If CLMsg.Show("GQ0006", Me.txtCommittee.Text, Me.cmbYear.Text, Me.cmbMonth.Text) = DialogResult.Yes Then
                Cursor.Current = Cursors.WaitCursor                                             ' マウスポインタ砂時計
                FrmWaitInfo.ShowWaitForm(Nothing)                                               ' しばらくお待ちくださいフォーム表示
                strYearMonth = Me.cmbYear.SelectedText & "/" & Me.cmbMonth.SelectedText
                strYearMonth = Me.cmbYear.Text & "/" & Me.cmbMonth.Text
                ' データベース接続
                Call clsDb.Connect()
                For rowCounter = 1 To (Me.flxAttendance.Rows.Count - 1) Step 2
                    For colCounter = 5 To Me.flxAttendance.Cols.Count - 1
                        If Not Me.flxAttendance.GetCellStyle(rowCounter, colCounter) Is Nothing Then
                            strCellStyle = Me.flxAttendance.GetCellStyle(rowCounter, colCounter).Name
                            ' 編集可セルのデータのみDB操作を行う
                            If Not strCellStyle.Equals("YellowCell") Then
                                'If Not strCellStyle.Equals("lightYellowCell") Then     ' 暫定
                                objValue = Me.flxAttendance.GetData(rowCounter, colCounter)
                                If Not objValue Is Nothing Then
                                    strAttValue = Trim(objValue.ToString)
                                    strDate = strYearMonth & "/" & Replace(CStr(Me.flxAttendance.GetData(0, colCounter)), "日", "")
                                    strDate = Format(Date.Parse(strDate), DATE_YYYYMMDD_FORMAT)
                                    If strAttValue = "" Then
                                        ' 該当出欠を削除
                                        strSql = "" & vbCrLf
                                        strSql = strSql & " DELETE" & vbCrLf
                                        strSql = strSql & "   FROM call_roll_user_dtl" & vbCrLf
                                        strSql = strSql & "  WHERE c_user_id = '" & CStr(Me.flxAttendance.GetData(rowCounter, 2)) & "'" & vbCrLf
                                        strSql = strSql & "    AND Format(d_years, 'yyyy/MM/dd') = '" & strYearMonth & "/01'" & vbCrLf
                                        strSql = strSql & "    AND Format(s_day, 'yyyy/MM/dd') = '" & strDate & "'" & vbCrLf
                                        strSql = strSql & ";" & vbCrLf
                                        ' ステータス設定（ログ出力判定用）
                                        bytStatus = STATUS_DELETE
                                    Else
                                        ' 該当出欠を追加か更新
                                        strSql = "" & vbCrLf
                                        strSql = strSql & " SELECT *" & vbCrLf
                                        strSql = strSql & "   FROM call_roll_user_dtl" & vbCrLf
                                        strSql = strSql & "  WHERE c_user_id = '" & CStr(Me.flxAttendance.GetData(rowCounter, 2)) & "'" & vbCrLf
                                        strSql = strSql & "    AND Format(d_years, 'yyyy/MM/dd') = '" & strYearMonth & "/01'" & vbCrLf
                                        strSql = strSql & "    AND Format(s_day, 'yyyy/MM/dd') = '" & strDate & "'" & vbCrLf
                                        strSql = strSql & ";" & vbCrLf
                                        dt = clsDb.ExecuteSql(strSql)
                                        If dt.Rows.Count > 0 Then
                                            ' 更新
                                            strSql = "" & vbCrLf
                                            strSql = strSql & " UPDATE call_roll_user_dtl" & vbCrLf
                                            strSql = strSql & "    SET c_daily_pay_id = '" & CStr(Me.flxAttendance.GetData(rowCounter, 4)) & "'" & vbCrLf   ' 日当ID
                                            strSql = strSql & "       ,s_committee_seq = '" & CStr(Me.flxAttendance.GetData(rowCounter, 3)) & "'" & vbCrLf  ' 委員会枝番
                                            strSql = strSql & "       ,c_menu_seq = '" & strAttValue & "'" & vbCrLf                                     ' 日当ID枝番
                                            ' 中執昼食費可否
                                            If Me.flxAttendance.GetCellCheck(rowCounter + 1, colCounter) = CheckEnum.Checked Then
                                                strSql = strSql & "       ,k_food_expenses = '1'" & vbCrLf
                                            Else
                                                strSql = strSql & "       ,k_food_expenses = '0'" & vbCrLf
                                            End If
                                            strSql = strSql & "       ,d_up = '" & Now & "'" & vbCrLf                              ' 更新日
                                            strSql = strSql & "       ,c_user_id_up = '" & MDLoginInfo.UserId & "'" & vbCrLf       ' 更新者個人ID
                                            strSql = strSql & "       ,s_up = s_up + 1"                                            ' 更新回数
                                            ' WHERE句
                                            strSql = strSql & " WHERE c_user_id = '" & CStr(Me.flxAttendance.GetData(rowCounter, 2)) & "'" & vbCrLf
                                            strSql = strSql & "   AND Format(d_years, 'yyyy/MM/dd') = '" & strYearMonth & "/01'" & vbCrLf
                                            strSql = strSql & "   AND Format(s_day, 'yyyy/MM/dd') = '" & strDate & "'" & vbCrLf
                                            strSql = strSql & ";" & vbCrLf
                                            ' ステータス設定（ログ出力判定用）
                                            bytStatus = STATUS_UPDATE
                                        Else
                                            ' 新規
                                            strSql = "" & vbCrLf
                                            strSql = strSql & " INSERT INTO call_roll_user_dtl (" & vbCrLf
                                            strSql = strSql & "        c_user_id" & vbCrLf
                                            strSql = strSql & "       ,d_years" & vbCrLf
                                            strSql = strSql & "       ,s_day" & vbCrLf
                                            strSql = strSql & "       ,c_committee_id" & vbCrLf
                                            strSql = strSql & "       ,s_committee_seq" & vbCrLf
                                            strSql = strSql & "       ,c_daily_pay_id" & vbCrLf
                                            strSql = strSql & "       ,c_menu_seq" & vbCrLf
                                            strSql = strSql & "       ,k_food_expenses" & vbCrLf
                                            strSql = strSql & "       ,d_ins" & vbCrLf
                                            strSql = strSql & "       ,c_user_id_ins" & vbCrLf
                                            strSql = strSql & "       ,s_up" & vbCrLf
                                            strSql = strSql & " ) values ( "
                                            strSql = strSql & "        '" & CStr(Me.flxAttendance.GetData(rowCounter, 2)) & "'" ' 個人認証ID
                                            strSql = strSql & "       ,'" & strYearMonth & "/01'"                               ' 対象年月
                                            strSql = strSql & "       ,'" & strDate & "'"                                       ' 対象日付
                                            strSql = strSql & "       ,'001'"                                                   ' 委員会ID
                                            strSql = strSql & "       ,'" & CStr(Me.flxAttendance.GetData(rowCounter, 3)) & "'" ' 委員会枝番
                                            strSql = strSql & "       ,'" & CStr(Me.flxAttendance.GetData(rowCounter, 4)) & "'" ' 日当ID
                                            strSql = strSql & "       ,'" & strAttValue & "'"                                   ' 日当ID枝番
                                            ' 中執昼食費可否
                                            If Me.flxAttendance.GetCellCheck(rowCounter + 1, colCounter) = CheckEnum.Checked Then
                                                strSql = strSql & "       ,'1'" & vbCrLf
                                            Else
                                                strSql = strSql & "       ,'0'" & vbCrLf
                                            End If
                                            strSql = strSql & "       ,'" & Now & "'" & vbCrLf                                  ' 作成日
                                            strSql = strSql & "       ,'" & MDLoginInfo.UserId & "'" & vbCrLf                   ' 作成者個人ID
                                            strSql = strSql & "       ,'1'" & vbCrLf                                            ' 更新回数
                                            strSql = strSql & ");" & vbCrLf
                                        End If
                                    End If
                                    ' トランザクション開始
                                    Call clsDb.BeginTran()
                                    ' SQL文実行
                                    intRtn = clsDb.ExecuteNonQuery(strSql)
                                    ' トランザクション確定
                                    Call clsDb.CommitTran()
                                    If bytStatus = STATUS_INSERT Then
                                        log.Info(String.Format("{0}件のデータを追加しました。", CStr(intRtn)))
                                    ElseIf bytStatus = STATUS_UPDATE Then
                                        log.Info(String.Format("{0}件のデータを更新しました。", CStr(intRtn)))
                                    ElseIf bytStatus = STATUS_DELETE Then
                                        log.Info(String.Format("{0}件のデータを削除しました。", CStr(intRtn)))
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
                ' 表示制御
                Me.txtCommittee.Enabled = True      ' 委員会名
                Me.cmbYear.Enabled = True           ' 登録月　年表示
                Me.cmbMonth.Enabled = True          ' 登録月　月表示
                Me.btnSearch.Enabled = True         ' 検索ボタン表示
                Me.btnUpdate.Visible = True         ' 内容更新ボタン表示
                Me.btnInsertChk.Visible = False     ' 登録確認ボタン非表示
                Me.btnCancel.Visible = False        ' キャンセルボタン非表示
                Me.grpResult.Visible = True         ' 出欠簿グループ表示
                Me.flxAttendance.AllowEditing = False

                '再検索
                Call Me.searchMain()
            End If
        Catch ex As Exception
            Call clsDb.RollbackTran()                                                       ' トランザクション取消
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            FrmWaitInfo.CloseWaitForm()                                                     ' しばらくお待ちくださいフォーム非表示
            Call clsDb.Disconnect()                                                         ' データベース切断
            Cursor.Current = Cursors.Default                                                ' マウスポンイタデフォルト
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub


    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            ' 入力・変更内容の破棄の旨のメッセージボックス表示
            If CLMsg.Show("GQ0007") = DialogResult.Yes Then
                ' OKボタン押下
                Me.btnUpdate.Visible = False                                                ' 内容変更ボタン非表示
                Me.btnSearch.Enabled = True                                                 ' 検索ボタン使用可
                Me.btnInsertChk.Visible = False                                             ' 登録確認ボタン非表示
                Me.btnCancel.Visible = False                                                ' キャンセルボタン非表示
                Me.txtCommittee.Enabled = True                                              ' 委員会テキストボックス使用可
                Me.cmbYear.Enabled = True                                                   ' 年コンボボックス使用可
                Me.cmbMonth.Enabled = True                                                  ' 月コンボボックス使用可
                Me.flxAttendance.AllowEditing = False                                           ' グリッド編集不可
                Me.grpResult.Visible = False
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

    Private Sub cmbYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbYear.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            '検索主処理呼び出し
            Call Me.searchMain()
        End If

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboYear_SelectionChangeCommitted
    '   名称　：年コンボボックス選択アイテム変更時
    '   概要　：
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki
    '***************************************************************************************************
    Private Sub cboYear_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbYear.SelectionChangeCommitted
        Try
            ' 月コンボボックス作成（選択されて年コンボボックスに合わせて月コンボボックスを作成）
            If CreateCboMonth(Me.cmbMonth, Me.cmbYear.Text) = False Then
                Exit Sub
            End If
            If strPreYear <> Me.cmbYear.Text Then
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
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbMonth_SelectionChangeCommitted
    '   名称　：月コンボボックスチェンジ処理
    '   概要　：月を選択しなおした時、詳細エリアを非表示にする
    '   作成日：2017/10/23(月)  y.fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2017/10/23(月)  y.fujisaku  新規作成
    '***************************************************************************************************
    Private Sub cmbMonth_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMonth.SelectionChangeCommitted
        Try
            If strPreMonth <> Me.cmbMonth.Text Then
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
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：flxAttendance_BeforeEdit
    '   名称　：編集不可制御
    '   概要　：
    '   作成日：2011/12/10(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/10(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub flxAttendance_BeforeEdit(ByVal sender As Object, ByVal e As C1.Win.C1FlexGrid.RowColEventArgs) Handles flxAttendance.BeforeEdit
        Dim strCellStyle As String                                                          ' セルスタイル名
        Try
            If Not Me.flxAttendance.GetCellStyle(Me.flxAttendance.RowSel, Me.flxAttendance.ColSel) Is Nothing Then
                strCellStyle = Me.flxAttendance.GetCellStyle(Me.flxAttendance.RowSel, Me.flxAttendance.ColSel).Name
                If strCellStyle.Equals("YellowCell") Or strCellStyle.Equals("boolYellow") Then
                    'If strCellStyle.Equals("lightYellowCell") Or strCellStyle.Equals("boolYellow") Then
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
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/06(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Try
            '===============================================================================
            '   共通
            '===============================================================================
            ' Label
            Me.lblCommittee.Visible = True                                                  ' 部／委員会
            Me.lblEntryMonth.Visible = True                                                 ' 登録月
            Me.lblYear.Visible = True                                                       ' 年
            Me.lblMonth.Visible = True                                                      ' 月
            Me.lblMandatoryEntryMonth.Visible = True                                        ' 必須（*）
            ' TextBox
            Me.txtCommittee.Text = ""                                                       ' 部／委員会
            ' TextBox BackColor
            Me.txtCommittee.BackColor = Color.LightYellow                                   ' 部／委員会
            ' ComboBox
            Me.cmbYear.Visible = True                                                       ' 登録年
            Me.cmbMonth.Visible = True                                                      ' 登録月
            ' Button
            Me.btnSearch.Visible = True                                                     ' 検索ボタン
            Me.btnInsertChk.Visible = False                                                 ' 登録確認ボタン
            Me.btnCancel.Visible = False                                                    ' キャンセルボタン
            Me.btnUpdate.Visible = True                                                     ' 内容変更ボタン
            ' GroupBox
            Me.grpSearch.Visible = True                                                     ' 検索
            '===============================================================================
            '   出欠簿
            '===============================================================================
            ' Label
            Me.lblUpTitle.Visible = False                                                   ' 上段タイトル
            Me.lblDownTitle.Visible = False                                                 ' 下段タイトル
            Me.lblUpMemo.Visible = False                                                    ' 上段メモ
            Me.lblMemo.Visible = False                                                      ' メモ
            ' CheckBox
            Me.chkDownMemo.Visible = False                                                  ' 下段メモ
            ' C1FlexGrid
            Me.flxAttendance.Visible = False                                                    ' 出欠簿グリッド
            ' GroupBox
            Me.grpResult.Visible = False                                                    ' 出欠簿

            ' クリア
            Me.txtCommittee.Text = ""                                                       ' 部／委員会
            Me.cmbYear.Items.Clear()                                                        ' 年コンボボックスリスト
            Me.cmbYear.Text = ""                                                            ' 年コンボボックス
            Me.cmbMonth.Items.Clear()                                                       ' 月コンボボックスリスト
            Me.cmbMonth.Text = ""                                                           ' 月コンボボックス
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：FlexGridIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：ByVal pStrYear  As String  = 検索年
    '           ByVal pStrMonth As String  = 検索月
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/29(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <param name="pStrYear">年</param>
    ''' <param name="pStrMonth">月</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function FlexGridIni(ByVal pStrYear As String,
                                 ByVal pStrMonth As String) As Boolean
        Dim blnRet As Boolean = False                   ' 処理結果
        Dim intDay As Integer = 0                       ' 年月に対する日数
        Dim datDate As Date = Nothing                   ' 土日判別用日付
        Try
            'Dim cs As C1.Win.C1FlexGrid.CellStyle
            'cs = Me.cfgResult.Styles.Add("SATURDAY")
            'cs.ForeColor = Color.White
            'cs.BackColor = Color.SkyBlue
            'cs = Me.cfgResult.Styles.Add("SUNDAY")
            'cs.ForeColor = Color.White
            'cs.BackColor = Color.Pink
            'cs = Me.cfgResult.Styles.Add("bool")
            'cs.DataType = Type.GetType("System.Boolean")
            'cs.ImageAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            'cs = Me.cfgResult.Styles.Add("boolYellow")
            'cs.DataType = Type.GetType("System.Boolean")
            'cs.ImageAlign = C1.Win.C1FlexGrid.ImageAlignEnum.CenterCenter
            'cs.BackColor = Color.LightYellow
            'cs.Editor = Nothing
            'cs = Me.cfgResult.Styles.Add("normal")
            'cs.DataType = Type.GetType("System.String")
            'cs = Me.cfgResult.Styles.Add("lightYellowCell")
            'cs.BackColor = Color.LightYellow
            'cs.TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
            'cs.Editor = Nothing
            '' 土曜日スタイル設定
            'csSaturday = Me.cfgResult.Styles.Add("SATURDAY")
            'csSaturday.ForeColor = Color.White
            'csSaturday.BackColor = Color.SkyBlue
            '' 日曜日スタイル設定
            'csSunDay = Me.cfgResult.Styles.Add("SUNDAY")
            'csSunDay.ForeColor = Color.White
            'csSunDay.BackColor = Color.Pink
            ' 検索年月に対する日数取得
            intDay = Date.DaysInMonth(CInt(pStrYear), CInt(pStrMonth))
            ' 描画なし（処理が終了した最後に描画）
            Me.flxAttendance.Redraw = False
            ' セルマージ
            Me.flxAttendance.AllowMerging = AllowMergingEnum.Free
            Me.flxAttendance.Cols(0).AllowMerging = True
            Me.flxAttendance.Cols(1).AllowMerging = True
            '-------------------------------------------------------------------------------
            '   グリッド全体設定
            '-------------------------------------------------------------------------------
            ' 総数
            Me.flxAttendance.Rows.Count = 1                                                     ' 縦
            Me.flxAttendance.Cols.Count = intDay + 5                                            ' 横（社員番号 + 名前 + 日）
            ' 固定行
            Me.flxAttendance.Rows.Fixed = 1                                                     ' 縦
            Me.flxAttendance.Cols.Fixed = 2                                                     ' 横
            ' スクロールバー
            Me.flxAttendance.ScrollBars = ScrollBars.Vertical = C1FLEXGRID_SCROLLBARS_BOTH      ' 縦横
            ' マージ
            Me.flxAttendance.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free             ' 
            Me.flxAttendance.Cols(0).AllowMerging = True                                        ' マージするカラム
            Me.flxAttendance.Cols(1).AllowMerging = True                                        ' マージするカラム
            '-----------------------------------------------------------------------------------
            '   ヘッダー部設定
            '-----------------------------------------------------------------------------------
            ' ヘッダー文字列（1～2カラム目）
            Me.flxAttendance.Cols(0).Caption = "社員番号"                                       ' 社員番号
            Me.flxAttendance.Cols(1).Caption = "名前"                                           ' 名前
            Me.flxAttendance.Cols(2).Caption = "名簿ID"                                         ' 名簿ID
            Me.flxAttendance.Cols(3).Caption = "委員会ID"                                       ' 委員会ID
            Me.flxAttendance.Cols(4).Caption = "名簿適用日付"                                   ' 名簿適用日付
            ' ヘッダー文字位置（1～2カラム目）
            Me.flxAttendance.Cols(0).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER   ' 社員番号  
            Me.flxAttendance.Cols(1).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER   ' 名前
            Me.flxAttendance.Cols(2).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER   ' 名簿ID
            Me.flxAttendance.Cols(3).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER   ' 委員会ID
            Me.flxAttendance.Cols(4).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER   ' 名簿適用日付
            ' 5カラム目～年月に対する日数分ループ
            For i = 1 To intDay
                Me.flxAttendance.SetData(0, 4 + i, CStr(i) + "日", False)                               ' ヘッダー文字列設定（日）
                Me.flxAttendance.SetCellStyle(0, 4 + i, Me.flxAttendance.Styles.SelectedRowHeader)          ' セルスタイル設定（日）
                datDate = Date.Parse(pStrYear + "/" + pStrMonth + "/" + CStr(i))                    ' 日付をデータ型で取得
                If Weekday(datDate) = 1 Then                                                        ' 日曜日かチェック
                    Me.flxAttendance.SetCellStyle(0, 4 + i, "SunDay")                                   ' 日曜日スタイル設定
                ElseIf Weekday(datDate) = 7 Then                                                    ' 土曜日かチェック
                    Me.flxAttendance.SetCellStyle(0, 4 + i, "Saturday")                                 ' 土曜日スタイル設定
                End If
                Me.flxAttendance.Cols(4 + i).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER   ' ヘッダー文字位置
                Me.flxAttendance.Cols(4 + i).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER        ' カラム文字位置
                Me.flxAttendance.Cols(4 + i).Width = 40                                                 ' カラム幅
                Me.flxAttendance.Cols(4 + i).Visible = True                                             ' カラム表示有無
            Next
            '-------------------------------------------------------------------------------
            '   カラム部設定
            '-------------------------------------------------------------------------------
            ' カラム文字位置
            Me.flxAttendance.Cols(0).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_RIGHTCENTER         ' 社員番号
            Me.flxAttendance.Cols(1).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER          ' 名前
            Me.flxAttendance.Cols(2).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER          ' 名簿ID
            Me.flxAttendance.Cols(3).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER          ' 委員会ID
            Me.flxAttendance.Cols(4).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER          ' 名簿適用日付
            For j = 1 To intDay
                Me.flxAttendance.Cols(4 + j).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER  ' 日
            Next
            ' カラム幅
            Me.flxAttendance.Cols(0).Width = 100                                                ' 社員番号
            Me.flxAttendance.Cols(1).Width = 100                                                ' 名前
            Me.flxAttendance.Cols(2).Width = 100                                                ' 名簿ID
            Me.flxAttendance.Cols(3).Width = 100                                                ' 委員会ID
            Me.flxAttendance.Cols(4).Width = 100                                                ' 名簿適用日付
            For k = 1 To intDay
                Me.flxAttendance.Cols(4 + k).Width = 40                                         ' 日
            Next
            ' カラム表示有無
            Me.flxAttendance.Cols(0).Visible = True                                             ' 社員番号
            Me.flxAttendance.Cols(1).Visible = True                                             ' 名前
            Me.flxAttendance.Cols(2).Visible = True                                             ' 名簿ID
            Me.flxAttendance.Cols(3).Visible = True                                             ' 委員会ID
            Me.flxAttendance.Cols(4).Visible = True                                             ' 名簿適用日付
            For l = 1 To intDay
                Me.flxAttendance.Cols(4 + l).Visible = True                                     ' 日
            Next

            Me.flxAttendance.Redraw = True                                                      ' 描画
            Me.grpResult.Visible = True                                                     ' 出欠簿グループ表示
            Me.flxAttendance.Visible = True                                                     ' フレックスグリッド表示
            blnRet = True                                                                   ' 戻り値格納
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各種情報を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/26(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Try
            '-------------------------------------------------------------------------------
            '   部／委員会
            '-------------------------------------------------------------------------------
            Me.txtCommittee.Text = COMMITTEE_NAME_CENTER                                    ' 中央執行委員会固定
            '-------------------------------------------------------------------------------
            '   コンボボックス作成
            '-------------------------------------------------------------------------------
            ' コンボボックス（年）
            If CreateCboYear(Me.cmbYear) = False Then
                Exit Function
            End If
            ' コンボボックス（月）
            If CreateCboMonth(Me.cmbMonth, Me.cmbYear.Text) = False Then
                Exit Function
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateCboYear
    '   名称　：コンボボックス（年）作成処理
    '   概要　：引数の情報でコンボボックスのリストを作成する。
    '   引数　：ByVal pCbo     As System.Windows.Forms.ComboBox = 年コンボボックス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '   備考  ：MDConstにDropDownStyleの定数あります。
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コンボボックス（年）作成処理</summary>
    ''' <param name="pCbo">年コンボボックス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboYear(ByVal pCbo As System.Windows.Forms.ComboBox) As Boolean
        Dim blnRet As Boolean = False           ' 処理結果
        Dim dtRet As DataTable = Nothing        ' データテーブル
        Dim drRet As DataRow = Nothing          ' データロー
        Dim intStart As Integer = 0             ' 開始月
        Dim intEnd As Integer = 0               ' 終了月
        Try
            ' 初期処理
            pCbo.BeginUpdate()                                                              ' チラつき防止の為、最後まで描写しない
            pCbo.DataSource = Nothing                                                       ' データソース初期化
            pCbo.Items.Clear()                                                              ' コンボボックスリストクリア

            ' 年コンボボックスが選択されているかチェック
            ' データテーブル・データロー生成
            dtRet = New DataTable
            dtRet.Columns.Add("YearValue", GetType(Integer))
            dtRet.Columns.Add("YearDisplay", GetType(String))

            For i = CInt(MDLoginInfo.PeriodFrom.Substring(0, 4)) To CInt(MDLoginInfo.PeriodTo.Substring(0, 4))
                drRet = dtRet.NewRow()                                                  ' 新しいデータロー作成
                drRet(0) = i.ToString().PadLeft(4, "0")                                 ' 値
                drRet(1) = i.ToString().PadLeft(4, "0")                                 ' 表示
                dtRet.Rows.Add(drRet)                                                   ' データ追加
            Next
            ' コンボボックス各設定
            pCbo.DropDownStyle = MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST          ' テキスト編集不可
            pCbo.DataSource = dtRet                                                     ' データソース設定
            pCbo.ValueMember = "YearValue"                                              ' コンボボックス値設定
            pCbo.DisplayMember = "YearDisplay"                                          ' コンボボックス名称設定
            pCbo.SelectedIndex = 0                                                      ' 先頭データ選択
            blnRet = True                                                                   ' 処理結果に正常を格納
        Catch ex As Exception
            pCbo.DataSource = Nothing                                                       ' コンボボックスデータソース削除
            pCbo.Items.Clear()                                                              ' コンボボックスリストクリア
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.cmbMonth.EndUpdate()                                                         ' チラつき防止の為、最後に描写する
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateCboMonth
    '   名称　：コンボボックス（月）作成処理
    '   概要　：引数の情報でコンボボックスのリストを作成する。
    '   引数　：ByVal pCbo     As System.Windows.Forms.ComboBox = 月コンボボックス
    '           ByVal pStrYear As String                        = 年コンボボックステキスト
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '   備考  ：MDConstにDropDownStyleの定数あります。
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コンボボックス（月）作成処理</summary>
    ''' <param name="pCbo">月コンボボックス</param>
    ''' <param name="pStrYear">年コンボボックステキスト</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboMonth(ByVal pCbo As System.Windows.Forms.ComboBox,
                                    ByVal pStrYear As String) As Boolean
        Dim blnRet As Boolean = False           ' 処理結果
        Dim dtRet As DataTable = Nothing        ' データテーブル
        Dim drRet As DataRow = Nothing          ' データロー
        Dim dtBlank As DataRow = Nothing        ' 空白データロー
        Dim intStart As Integer = 0             ' 開始月
        Dim intEnd As Integer = 0               ' 終了月
        Try
            ' 初期処理
            pCbo.BeginUpdate()                                                              ' チラつき防止の為、最後まで描写しない
            pCbo.DataSource = Nothing                                                       ' データソース初期化
            pCbo.Items.Clear()                                                              ' コンボボックスリストクリア
            intStart = 1                                                                    ' 開始月
            intEnd = 12                                                                     ' 終了月
            ' 年コンボボックスが選択されているかチェック
            If pStrYear.Length <> 0 Then
                ' データテーブル・データロー生成
                dtRet = New DataTable
                dtRet.Columns.Add("MonthValue", GetType(Integer))
                dtRet.Columns.Add("MonthDisplay", GetType(String))
                ' 先頭空白データ有の場合、空白データを作成
                dtBlank = dtRet.NewRow()
                dtRet.Rows.InsertAt(dtBlank, 0)
                For i = intStart To intEnd
                    drRet = dtRet.NewRow()                                                  ' 新しいデータロー作成
                    drRet(0) = i                                                            ' 値
                    drRet(1) = i.ToString().PadLeft(2, "0")                                 ' 表示
                    dtRet.Rows.Add(drRet)                                                   ' データ追加
                Next
                ' コンボボックス各設定
                pCbo.DropDownStyle = MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST          ' テキスト編集不可
                pCbo.DataSource = dtRet                                                     ' データソース設定
                pCbo.ValueMember = "MonthValue"                                             ' コンボボックス値設定
                pCbo.DisplayMember = "MonthDisplay"                                         ' コンボボックス名称設定
                pCbo.SelectedIndex = 0                                                      ' 先頭データ選択
            End If
            blnRet = True                                                                   ' 処理結果に正常を格納
        Catch ex As Exception
            pCbo.DataSource = Nothing                                                       ' コンボボックスデータソース削除
            pCbo.Items.Clear()                                                              ' コンボボックスリストクリア
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.cmbMonth.EndUpdate()                                                         ' チラつき防止の為、最後に描写する
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '#Region "入力チェック処理"
    '    '************************************************************************************
    '    '   ＩＤ　：ChkInput
    '    '   名称　：入力チェック処理
    '    '   概要　：
    '    '   作成日：2011/12/10(土)  m.suzuki
    '    '   更新日：
    '    '------------------------------------------------------------------------------------
    '    '   履歴　：2011/12/10(土)  m.suzuki  新規作成
    '    '************************************************************************************
    '    Private Function CheckInput() As Boolean
    '        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
    '        Dim blnRet As Boolean = False   ' 処理結果
    '        Try
    '            If Me.cboYear.SelectedIndex < 0 Then
    '                CLMsg.Show("GE0010", "登録月　年")
    '                Return blnRet
    '            End If
    '            If Me.cboMonth.SelectedIndex < 0 Then
    '                CLMsg.Show("GE0010", "登録月　月")
    '                Return blnRet
    '            End If
    '            ' 入力した日付は期の範囲内にあるか
    '            Dim table As DataTable = GetPeriodFromTo(cmbCommittee.SelectedValue, Me.cmbYear.Text & Me.cmbMonth.Text & "01", cmbCommittee.Text)
    '            If table Is Nothing Then
    '                Return False
    '            End If
    '            Dim time1 As DateTime = DateTime.ParseExact(table.Rows.Item(0).Item("d_from").ToString, DATE_yyyyMMdd_8_FORMAT, Nothing)
    '            Dim time2 As DateTime = DateTime.ParseExact(table.Rows.Item(0).Item("d_to").ToString, DATE_yyyyMMdd_8_FORMAT, Nothing)
    '            Dim str As String = (Me.cmbYear.SelectedItem.ToString.PadLeft(4, "0"c) & Me.cmbMonth.SelectedItem.ToString.PadLeft(2, "0"c) & "01")
    '            If ((str.CompareTo(time1.ToString(DATE_yyyyMMdd_8_FORMAT)) < 0) OrElse (str.CompareTo(time2.ToString(DATE_yyyyMMdd_8_FORMAT)) > 0)) Then
    '                CLMsg.Show("GE0013", time1.ToString("yyyy" & "年" & "MM" & "月"), time2.ToString("yyyy" & "年" & "MM" & "月"))
    '                Return False
    '            End If
    '            Return True
    '        Catch ex As Exception
    '            ' ログ出力（致命的エラー）
    '            log.Fatal(ex.Message)
    '            ' 致命的エラーメッセージボックス表示
    '            Call CLMsg.ShowEtarnal(Err.Number, _
    '                                   Err.Description, _
    '                                   SCREEN_ID, SCREEN_NAME, _
    '                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)    ' ログ出力（処理正常終了）
    '    End Function
    '#End Region

    '***************************************************************************************************
    '   ＩＤ　：GetSearchData
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/29(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>検索データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSearchData() As Boolean
        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL
        Dim clsDb As CLAccessMdb = Nothing          ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim dtAtt As DataTable = Nothing            ' 出欠情報データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数
        Dim strTergetDate As String = ""            ' 対象日付
        Dim intPos1 As Integer = 0
        Dim intPos2 As Integer = 0
        Try
            ' 対象日付取得
            strTergetDate = Me.cmbYear.Text & Me.cmbMonth.Text & "01"

            strSql = "" & vbCrLf
            strSql = strSql & " SELECT committee_list_dtl.c_user_id AS c_user_id" & vbCrLf          ' 01. 社員番号
            strSql = strSql & "       ,staf.l_name AS c_user_name" & vbCrLf                         ' 02. 名前
            strSql = strSql & "       ,committee_list.c_committee_list AS committee_list" & vbCrLf  ' 03. 名簿ID
            strSql = strSql & "       ,committee_list.c_committee_id   AS committee_id" & vbCrLf    ' 04. 委員会ID
            strSql = strSql & "       ,committee_list.d_from AS d_from" & vbCrLf                    ' 05. 名簿適用日付
            strSql = strSql & "   FROM committee_list" & vbCrLf
            strSql = strSql & "       ,committee_list_dtl" & vbCrLf
            strSql = strSql & "       ,( SELECT staf_attribute.c_user_id" & vbCrLf
            strSql = strSql & "                ,staf_attribute.l_name" & vbCrLf
            strSql = strSql & "                ,MAX(staf_attribute.d_from) AS d_from_max" & vbCrLf
            strSql = strSql & "            FROM staf_attribute" & vbCrLf
            strSql = strSql & "           WHERE d_from <= '" & strTergetDate & "'" & vbCrLf
            strSql = strSql & "           GROUP BY staf_attribute.c_user_id" & vbCrLf
            strSql = strSql & "                   ,staf_attribute.l_name" & vbCrLf
            strSql = strSql & "                   ,staf_attribute.d_from ) AS staf" & vbCrLf
            strSql = strSql & "  WHERE committee_list.c_committee_list = committee_list_dtl.c_committee_list" & vbCrLf
            strSql = strSql & "    AND committee_list.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            strSql = strSql & "    AND committee_list.c_committee_id = '001'" & vbCrLf
            strSql = strSql & "    AND committee_list.d_from = ( SELECT MAX(committee_list.d_from) AS d_from_max" & vbCrLf
            strSql = strSql & "                                        FROM committee_list" & vbCrLf
            strSql = strSql & "                                       WHERE committee_list.c_committee_id = '001'" & vbCrLf
            strSql = strSql & "                                         AND committee_list.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            strSql = strSql & "                                         AND committee_list_dtl.d_from <= '" & strTergetDate & "'" & vbCrLf
            strSql = strSql & "                                       GROUP BY committee_list_dtl.d_from )" & vbCrLf
            strSql = strSql & "    AND committee_list_dtl.c_user_id = staf.c_user_id" & vbCrLf
            strSql = strSql & "  ORDER BY committee_list_dtl.s_committee_seq" & vbCrLf
            strSql = strSql & "          ,committee_list_dtl.c_user_id" & UtDb.DbOrderOffset() & vbCrLf
            strSql = strSql & ";" & vbCrLf
            'todo:
            clsDb = New CLAccessMdb                                         ' データベースクラス生成
            Call clsDb.Connect()                                            ' データベース接続
            tbRet = clsDb.ExecuteSql(strSql)                                ' SQL実行
            ' 件数取得
            If tbRet.Rows.Count = 0 Then
                intRetCnt = 0
            Else
                intRetCnt = (tbRet.Rows.Count * 2) + 1
            End If
            Me.flxAttendance.Rows.Count = intRetCnt                                 ' C1FlexGrid行設定
            For i = 0 To tbRet.Rows.Count - 1                                   ' 件数分ループ
                intPos1 = (i * 2) + 1                                           ' 1行目位置取得
                intPos2 = (i * 2) + 2                                           ' 2行目位置取得

                ' 1行目
                Me.flxAttendance.SetData(intPos1, 0, tbRet.Rows(i).Item(0))         ' 社員番号
                Me.flxAttendance.SetData(intPos1, 1, tbRet.Rows(i).Item(1))         ' 名前
                Me.flxAttendance.SetData(intPos1, 2, tbRet.Rows(i).Item(2))         ' 名簿ID
                Me.flxAttendance.SetData(intPos1, 3, tbRet.Rows(i).Item(3))         ' 委員会ID
                Me.flxAttendance.SetData(intPos1, 4, tbRet.Rows(i).Item(4))         ' 名簿適用日付

                ' 2行目
                Me.flxAttendance.SetData(intPos2, 0, tbRet.Rows(i).Item(0))         ' 社員番号
                Me.flxAttendance.SetData(intPos2, 1, tbRet.Rows(i).Item(1))         ' 名前
                Me.flxAttendance.SetData(intPos2, 2, tbRet.Rows(i).Item(2))         ' 名簿ID
                Me.flxAttendance.SetData(intPos2, 3, tbRet.Rows(i).Item(3))         ' 委員会ID
                Me.flxAttendance.SetData(intPos2, 4, tbRet.Rows(i).Item(4))         ' 名簿適用日付

                ''役職に付き日当IDと日当枝番（日当選択肢）を取得
                'strSql = "" & vbCrLf
                'strSql = strSql & "SELECT daily_pay_master_dtl.c_daily_pay_id" & vbCrLf
                'strSql = strSql & "      ,daily_pay_master_dtl.c_menu_seq" & vbCrLf
                'strSql = strSql & "      ,daily_pay_master_dtl.l_name" & vbCrLf
                'strSql = strSql & "      ,daily_pay_master_dtl.l_explain" & vbCrLf
                'strSql = strSql & "  FROM daily_pay_master_dtl" & vbCrLf
                'strSql = strSql & "       LIGHT JOIN ( SELECT c_daily_pay_id" & vbCrLf
                'strSql = strSql & "                      FROM committee_dtl" & vbCrLf
                'strSql = strSql & "                     WHERE committee_dtl.c_committee_id = '001'" & vbCrLf
                'strSql = strSql & "                       AND committee_dtl.s_committee_seq = '" & tbRet.Rows(i).Item(0).ToString() & "') AS pay_id" & vbCrLf
                'strSql = strSql & "       ON daily_pay_master_dtl.c_daily_pay_id = pay_id.c_daily_pay_id" & vbCrLf
                'strSql = strSql & ";" & vbCrLf
                '' SQL実行
                'dtAtt = clsDb.ExecuteSql(strSql)
            Next
            Cursor.Current = Cursors.Default
            blnRet = True
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()                                                         ' データベース切断
            Cursor.Current = Cursors.Default
        End Try
        Return blnRet
    End Function

    '************************************************************************************
    '   ＩＤ　：CheckInput
    '   名称　：入力チェック処理
    '   概要　：入力チェック
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24(木)  Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  Ryu  新規作成
    '************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckInput() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Try
            If Me.cmbYear.SelectedIndex < 0 Then
                CLMsg.Show("GE0010", COMMITTEE_COMBO_YEAR)
                Return False
            End If
            If Me.cmbMonth.SelectedIndex <= 0 Then
                CLMsg.Show("GE0010", COMMITTEE_COMBO_MONTH)
                Return False
            End If
            ' 入力した日付は期の範囲内にあるか
            Dim table As DataTable = GetPeriodFromTo("001", Me.cmbYear.Text & Me.cmbMonth.Text & "01", "中央執行委員会")
            If table Is Nothing Then
                Return False
            End If
            Dim time1 As DateTime = DateTime.ParseExact(table.Rows.Item(0).Item("d_from").ToString, DATE_YYYYMMDD_8_FORMAT, Nothing)
            Dim time2 As DateTime = DateTime.ParseExact(table.Rows.Item(0).Item("d_to").ToString, DATE_YYYYMMDD_8_FORMAT, Nothing)
            Dim str As String = (Me.cmbYear.SelectedValue.ToString.PadLeft(4, "0"c) & Me.cmbMonth.SelectedValue.ToString.PadLeft(2, "0"c) & "01")
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
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Function
    ''***************************************************************************************************
    ''   ＩＤ　：CheckInput
    ''   名称　：入力チェック処理
    ''   概要  ：入力チェックを行う。
    ''   引数　：なし
    ''   戻り値：True = 正常, False = 異常
    ''   作成日：2011/11/29(火)  m.suzuki
    ''   更新日：
    ''---------------------------------------------------------------------------------------------------
    ''   履歴　：2011/11/29(火)  m.suzuki  新規作成
    ''***************************************************************************************************
    '''' <summary>入力チェック処理</summary>
    '''' <returns>True = 正常, False = 異常</returns>
    '''' <remarks></remarks>
    'Private Function CheckInput() As Boolean
    '    log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
    '    Dim blnRet As Boolean = False       ' 処理結果
    '    Try
    '        '-----------------------------------------------------------
    '        '   初期化
    '        '-----------------------------------------------------------
    '        ' エラー箇所クリア処理
    '        If ClearErr(Me) = False Then
    '            Exit Function
    '        End If
    '        ' 登録年
    '        If Me.cmbYear.SelectedIndex <= 0 Then
    '            Call CLMsg.Show("GE0006", "登録年")
    '            Call SetErr(Me.cmbYear)
    '            Exit Function
    '        End If
    '        ' 登録月
    '        If Me.cmbMonth.SelectedIndex <= 0 Then
    '            Call CLMsg.Show("GE0006", "登録月")
    '            Call SetErr(Me.cmbMonth)
    '            Exit Function
    '        End If
    '        '' 範囲チェック
    '        'If ChkRangeTerm() = False Then
    '        '    Exit Function
    '        'End If
    '        blnRet = True                                                                   ' 処理結果に正常を設定
    '    Catch ex As Exception
    '        ' ログ出力（致命的エラー）
    '        log.Fatal(ex.Message)
    '        ' 致命的エラーメッセージボックス表示
    '        Call CLMsg.ShowEtarnal(Err.Number, _
    '                               Err.Description, _
    '                               SCREEN_ID, SCREEN_NAME, _
    '                               System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '    End Try
    '    log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")         ' ログ出力（処理終了）
    '    Return blnRet                                                                       ' 戻り値格納
    'End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkRangeTerm
    '   名称　：選択年月範囲チェック処理（期範囲チェック）
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/10(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/10(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>選択年月範囲チェック処理（期範囲チェック）</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkRangeTerm() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False
        Dim dt As DataTable = Nothing
        Dim time1 As DateTime = Nothing
        Dim time2 As DateTime = Nothing
        Dim str As String = Nothing
        Try
            ' 選択した日付は期の範囲内かチェック
            dt = GetPeriodFromTo("001", Me.cmbYear.Text & Me.cmbMonth.Text & "01", Me.txtCommittee.Text)
            If dt Is Nothing Then
                Return blnRet
            End If
            time1 = DateTime.ParseExact(dt.Rows.Item(0).Item("d_from").ToString, DATE_YYYYMMDD_8_FORMAT, Nothing)
            time2 = DateTime.ParseExact(dt.Rows.Item(0).Item("d_to").ToString, DATE_YYYYMMDD_8_FORMAT, Nothing)
            str = (Me.cmbYear.SelectedItem.ToString.PadLeft(4, "0"c) & Me.cmbMonth.SelectedItem.ToString.PadLeft(2, "0"c) & "01")
            If ((str.CompareTo(time1.ToString(DATE_YYYYMMDD_8_FORMAT)) < 0) OrElse (str.CompareTo(time2.ToString(DATE_YYYYMMDD_8_FORMAT)) > 0)) Then
                CLMsg.Show("GE0013", time1.ToString("yyyy" & "年" & "MM" & "月"), time2.ToString("yyyy" & "年" & "MM" & "月"))
                Return blnRet
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
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetPeriodFromTo
    '   名称　：期の開始と終了日付取得処理
    '   概要　：期の開始と終了日付取得
    '   引数　：ByVal strCommitteeId   As String = 委員会ID
    '       　  ByVal strTargetDate    As String = 対象日付
    '       　  ByVal strCommitteeName As String = 委員会名
    '   戻り値：DataTable = 期の開始と終了日付
    '   作成日：2011/12/10(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/10(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>期の開始と終了日付取得処理</summary>
    ''' <param name="strCommitteeId">委員会ID</param>
    ''' <param name="strTargetDate">対象日付</param>
    ''' <param name="strCommitteeName">委員会名</param>
    ''' <returns>期の開始と終了日付</returns>
    ''' <remarks></remarks>
    Public Function GetPeriodFromTo(ByVal strCommitteeId As String,
                                    ByVal strTargetDate As String,
                                    ByVal strCommitteeName As String) As DataTable
        Dim table2 As New DataTable("period_from_to")                                   ' データテーブル
        Dim monthDifference As DataTable = Nothing
        Try
            monthDifference = GetMonthDifference(strTargetDate, strCommitteeId)
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
        Return table2                                                                   ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetMonthDifference
    '   名称　：期の開始と終了日付取得処理
    '   概要　：期の開始と終了日付調整
    '   引数　：ByVal strDate        As String = 日付,
    '       　  ByVal strCommitteeId As String = 委員会ID
    '   戻り値：DataTable = 期の開始と終了日付
    '   作成日：2011/12/10(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/10(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>期の開始と終了日付取得処理</summary>
    ''' <param name="strDate">日付</param>
    ''' <param name="strCommitteeId">委員会ID</param>
    ''' <returns>期の開始と終了日付</returns>
    ''' <remarks></remarks>
    Public Function GetMonthDifference(ByVal strDate As String,
                                       ByVal strCommitteeId As String) As DataTable
        Dim set2 As New DataTable                                                       ' データテーブル
        Dim sql As String = Nothing
        Dim dbAccess As New CLAccessMdb
        Try
            sql = "SELECT Min(committee_dtl.s_from_diff)as s_from_diff, MAX(committee_dtl.s_to_diff) as s_to_diff FROM (SELECT c_committee_id, d_from FROM committee WHERE c_committee_id = '" + strCommitteeId + "' AND d_from <= '" + strDate + "' AND d_to >= '" + strDate + "' AND c_ksh = '" + MDLoginInfo.Ksh + "' ) committee_A, committee_dtl WHERE committee_dtl.c_committee_id = committee_A.c_committee_id AND committee_dtl.d_from = committee_A.d_from"
            set2 = New DataTable("month_difference")
            dbAccess.Connect()
            set2 = dbAccess.ExecuteSql(sql)
            dbAccess.Disconnect()
            If set2.Rows.Count = 0 Then
                Return Nothing
            End If
            If (String.IsNullOrEmpty(set2.Rows.Item(0).Item("s_from_diff").ToString) OrElse String.IsNullOrEmpty(set2.Rows.Item(0).Item("s_to_diff").ToString)) Then
                Return Nothing
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
        Return set2                                                                     ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：checkCloseDate
    '   名称　：締切日判断処理
    '   概要　：委員会あるいはユーザーの今期開始日、前々回の締切日と前回締切日の取得
    '   引数　：ByVal strCommID          As String = 委員会ID
    '           Optional ByVal strUserID As String = ユーザーID
    '   作成日：2011/12/10(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/10(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>締切日判断処理</summary>
    ''' <param name="strCommID">委員会ID</param>
    ''' <param name="strUserID">ユーザー認証ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getCloseDateCommOrUser(ByVal strCommID As String,
                                            Optional ByVal strUserID As String = "")
        Dim dbAccess As New CLAccessMdb                     ' DBアクセス
        Dim sql As String                                   ' SQL文
        Dim dt As DataTable                                 ' データテーブル
        Dim strPeriodStart As String                        ' 現在の期の開始日付
        Dim strPreCloseDate As String                       ' 前回締切日
        Dim strPrePreCloseDate As String                    ' 前々回締切日
        Dim lstRtnValue As New List(Of String)              ' 戻り値リスト
        Dim strYear As String                               ' 参照年
        Dim strMonth As String                              ' 参照月
        Dim blnRtn As Boolean                               ' 戻り値
        Try
            dbAccess.Connect()
            strYear = Me.cmbYear.Text
            strMonth = Me.cmbMonth.Text
            strPeriodStart = MDLoginInfo.PeriodFrom
            lstRtnValue.Add(Replace(strPeriodStart, "/", ""))
            If strUserID <> "" Then
                'ユーザーIDがある場合、ユーザーに対しての締切日を取得
                sql = "select DISTINCT d_daily_pay_close from call_roll_user_dtl where c_committee_id='" + strCommID + "' and c_user_id='" + strUserID + "'"
            Else
                'ユーザーIDがない場合、委員会に対しての締切日を取得
                sql = "select DISTINCT d_daily_pay_close from daily_pay_close_dtl where c_committee_id='" + strCommID + "'"
            End If
            sql = sql + " order by d_daily_pay_close DESC"  'chk
            dt = dbAccess.ExecuteSql(sql)
            dbAccess.Disconnect()
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
        Return blnRtn                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：InsertCentralInfo
    '   名称　：中執活動報告情報登録処理
    '   概要  ：中執活動報告情報の登録を行う。
    '   引数　：ByVal pClsDb  As CLAccessMdb = データベースクラス
    '           ByVal pIntRow As Integer     = グリッドロー
    '           ByVal pIntCol As Integer     = データカラム
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/02(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function InsertCentralInfo(ByVal pClsDb As CLAccessMdb,
                                       ByVal pIntRow As Integer,
                                       ByVal pIntCol As Integer) As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim strSql As String = ""           ' SQL文
        Dim intRet As Integer = 0           ' 処理件数
        Try
            ' SQL作成
            strSql = ""
            strSql = strSql & " INSERT call_roll_user_dtl (" & vbCrLf
            strSql = strSql & "     c_user_id" & vbCrLf                     ' 01. 個人認証ID
            strSql = strSql & "    ,d_years" & vbCrLf                       ' 02. 対象年月
            strSql = strSql & "    ,s_day" & vbCrLf                         ' 03. 日付情報
            strSql = strSql & "    ,c_committee_id" & vbCrLf                ' 04. 委員会ID
            strSql = strSql & "    ,s_committee_seq" & vbCrLf               ' 05. 委員会ID枝番
            strSql = strSql & "    ,c_daily_pay_id" & vbCrLf                ' 06. 日当ID
            strSql = strSql & "    ,c_menu_seq " & vbCrLf                   ' 07. 日当ID枝番（1:勤務日 2:勤務日以外）
            strSql = strSql & "    ,k_food_expenses" & vbCrLf               ' 08. 中執昼食費可否
            strSql = strSql & "    ,k_daily_pay_kind" & vbCrLf              ' 09. 日当計算区分
            strSql = strSql & "    ,d_daily_pay_close" & vbCrLf             ' 10. 締め日付
            strSql = strSql & "    ,d_up_close" & vbCrLf                    ' 11. 締め時更新日
            strSql = strSql & "    ,s_daily_pay" & vbCrLf                   ' 12. 締め日当金額
            strSql = strSql & "    ,s_food_expenses" & vbCrLf               ' 13. 締め中執昼食費
            strSql = strSql & "    ,s_next_balance_daily_pay" & vbCrLf      ' 14. 次締め差分日当金額
            strSql = strSql & "    ,s_next_balance_food_expenses" & vbCrLf  ' 15. 次締め差分中執昼食費
            strSql = strSql & "    ,d_ins" & vbCrLf                         ' 16. 作成日
            strSql = strSql & "    ,c_user_id_ins" & vbCrLf                 ' 17. 作成者個人ID
            strSql = strSql & "    ,d_up" & vbCrLf                          ' 18. 更新日
            strSql = strSql & "    ,c_user_id_up" & vbCrLf                  ' 19. 更新者個人ID
            strSql = strSql & "    ,s_up" & vbCrLf                          ' 20. 更新回数
            strSql = strSql & " ) VALUES (" & vbCrLf
            strSql = strSql & "     '" & Me.flxAttendance.GetData(pIntRow, 2).ToString() & "'"  ' 個人認証ID
            ' 対象年月
            ' 日付情報
            strSql = strSql & ",'" & Me.flxAttendance.GetData(pIntRow, 2).ToString() & "'"  ' 委員会ID"
            strSql = strSql & ",'" & Me.flxAttendance.GetData(pIntRow, 3).ToString() & "'"  ' 委員会ID枝番
            strSql = strSql & ",'" & Me.flxAttendance.GetData(pIntRow, 4).ToString() & "'"  ' 日当ID
            strSql = strSql & ",NULL" & vbCrLf                          ' . 日当ID枝番（1:勤務日 2:勤務日以外）
            ' 日当ID枝番（1:勤務日 2:勤務日以外）
            ' 中執昼食費可否
            If Me.flxAttendance.GetCellCheck(pIntRow + 1, pIntCol) = C1.Win.C1FlexGrid.CheckEnum.Checked Then
                strSql = strSql + "    ,'1'"
            Else
                strSql = strSql + "    ,'0'"
            End If
            ' 日当計算区分
            ' 締め日付
            ' 締め時更新日
            ' 締め日当金額
            ' 締め中執昼食費
            ' 次締め差分日当金額
            ' 次締め差分中執昼食費
            strSql = strSql & ",'" & Now & "'" & vbCrLf                 ' 16. 作成日
            strSql = strSql & ",'" & MDLoginInfo.UserId & "'" & vbCrLf  ' 17. 作成者個人ID
            strSql = strSql & ",NULL" & vbCrLf                          ' 18. 更新日
            strSql = strSql & ",NULL" & vbCrLf                          ' 19. 更新者個人ID
            strSql = strSql & ",0" & vbCrLf                             ' 20. 更新回数
            strSql = strSql & " );" & vbCrLf
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()                                                     ' データベース切断
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)    ' ログ出力（処理正常終了）
        Return blnRet                                                                   ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：UpdateCentralInfo
    '   名称　：中執活動報告情報更新処理
    '   概要  ：中執活動報告情報の更新を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/26(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function UpdateCentralInfo() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス生成
        Dim strSql As String = ""                                                           ' SQL文
        Dim intRet As Integer = 0                                                           ' 処理件数
        Try
            ' SQL
            strSql = ""
            strSql = strSql & " UPDATE " & vbCrLf
            strSql = strSql & "    SET " & vbCrLf
            strSql = strSql & "        " & vbCrLf
            strSql = strSql & "  WHERE " & vbCrLf
            strSql = strSql & ";" & vbCrLf
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()                                                         ' データベース切断
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：DeleteCentralInfo
    '   名称　：中執活動報告情報削除処理
    '   概要  ：中執活動報告情報の削除を行う。
    '   引数　：ByVal pClsDb           As CLAccessMdb = データベースクラス
    '           ByVal pStrUserId       As String      = 個人認証ID
    '           ByVal pDatYears        As Date        = 対象年月
    '           ByVal pDatDay          As Date        = 日付情報
    '           ByVal pStrCommitteeId  As String      = 委員会ID
    '           ByVal pStrCommitteeSeq As String      = 委員会ID枝番
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/02(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function DeleteCentralInfo(ByVal pClsDb As CLAccessMdb,
                                       ByVal pStrUserId As String,
                                       ByVal pDatYears As Date,
                                       ByVal pDatDay As Date,
                                       ByVal pStrCommitteeId As String,
                                       ByVal pStrCommitteeSeq As String) As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim strSql As String = ""                                                           ' SQL文
        Dim intRet As Integer = 0                                                           ' 処理件数
        Try
            ' SQL作成
            strSql = ""
            strSql = strSql & " DELETE" & vbCrLf
            strSql = strSql & "   FROM call_roll_user_dtl" & vbCrLf
            strSql = strSql & "  WHERE call_roll_user_dtl.c_user_id = '" & pStrUserId & "'" & vbCrLf
            strSql = strSql + "    AND FORMAT(call_roll_user_dtl.d_years = '" & pDatYears & "'" & vbCrLf
            strSql = strSql + "    AND call_roll_user_dtl.s_day = '" & pDatDay & "'" & vbCrLf
            strSql = strSql + "    AND call_roll_user_dtl.c_committee_id = '" & pStrCommitteeId & "'" & vbCrLf
            strSql = strSql + "    AND call_roll_user_dtl.s_committee_seq = '" & pStrCommitteeSeq & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            ' SQL実行
            intRet = pClsDb.ExecuteNonQuery(strSql)
            If intRet = 1 Then
                blnRet = True                                                               ' 処理結果に正常を設定
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
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：DefinitionStyles
    '   名称　：スタイル定義処理
    '   概要　：セルのスタイルを定義
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/02(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>スタイル定義処理</summary>
    ''' <remarks></remarks>
    Private Sub DefinitionStyles()
        Try
            '-------------------------------------------------------------------
            '   スタイルを作成
            '-------------------------------------------------------------------
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
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：GetAttendanceData
    '   名称　：出欠データ取得処理
    '   概要　：出欠データ取得処理
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
            sql = sql + "FROM staf_attribute  "
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
            sql = sql + "ORDER BY committee_list_info.s_committee_seq,staf_info.k_belonging,CLng(staf_info.c_staf_id)" & UtDb.DbOrderOffset()   'ok

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
        Return table2
    End Function

    '***************************************************************************************************
    '   ＩＤ　：MyCurrentCommittee
    '   名称　：所属委員会取得処理
    '   概要　：所属委員会取得処理
    '   引数　：ByVal strUserId As String = ユーザー認証ID,
    '           ByVal strDate   As String = 日付
    '   戻り値：DataTable = 所属委員会
    '   作成日：2011/12/12(月) ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/12(月) ryu  新規作成
    '***************************************************************************************************
    ''' <summary>所属委員会取得処理</summary>
    ''' <param name="strUserId">ユーザー認証ID</param>
    ''' <param name="strDate">日付</param>
    ''' <returns>所属委員会</returns>
    ''' <remarks></remarks>
    Public Function MyCurrentCommittee(ByVal strUserId As String,
                                       ByVal strDate As String) As DataTable
        Dim table3 As DataTable = Nothing
        Try
            Dim strA As String = strDate.Substring(0, 6)
            Dim ksh As String = MDLoginInfo.Ksh
            Dim table As DataTable = GetBelongCommittee(ksh, strUserId, strDate)
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
        Return table3
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetBelongCommittee
    '   名称　：所属委員会取得処理
    '   概要　：所属委員会取得処理
    '   引数　：ByVal strKsh    As String = 会社コード,
    '           ByVal strUserId As String = ユーザー認証ID,
    '           ByVal strDate   As String = 日付
    '   戻り値：DataTable = 所属委員会
    '   作成日：2011/12/12(月)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/12(月)  ryu  新規作成
    '***************************************************************************************************
    ''' <summary>所属委員会取得処理</summary>
    ''' <param name="strKsh">会社コード</param>
    ''' <param name="strUserId">ユーザー認証ID</param>
    ''' <param name="strDate">日付</param>
    ''' <returns>所属委員会</returns>
    ''' <remarks></remarks>
    Public Function GetBelongCommittee(ByVal strKsh As String,
                                       ByVal strUserId As String,
                                       ByVal strDate As String) As DataTable
        Dim table2 As DataTable = Nothing
        Try
            Dim sql As String = "SELECT  PER.d_from AS PERIOD_DFROM,  PER.d_to AS PERIOD_DTO,  COMDTL.s_from_diff,  COMDTL.s_to_diff,  COMLISTDTL.c_committee_list,  COMLISTDTL.c_user_id,  COMLISTDTL.d_from,  COMLISTDTL.c_committee_id,  COMLISTDTL.s_committee_seq,  COMLISTDTL.l_biko,  COMLISTDTL.d_ins,  COMLISTDTL.c_user_id_ins,  COMLISTDTL.d_up,  COMLISTDTL.c_user_id_up,  COMLISTDTL.s_up,  COMLIST_B.c_period_id  FROM "
            sql = sql + "(select c_committee_id,c_period_id,max(d_from) as a_d_from  from committee_list where c_ksh = '" + strKsh + "' and d_from    <= '" + strDate + "'   group by c_committee_id, c_period_id ) COMLIST_A,  committee_list COMLIST_B,     committee_list_dtl COMLISTDTL,  committee_dtl COMDTL,  period PER "
            sql = sql + "where COMLIST_A.a_d_from = COMLIST_B.d_from and COMLIST_A.c_committee_id   = COMLIST_B.c_committee_id     and COMLIST_A.c_period_id      = COMLIST_B.c_period_id        and COMLIST_B.c_ksh            = '" + strKsh + "' and COMLIST_B.c_committee_list = COMLISTDTL.c_committee_list   and COMLIST_B.d_from           = COMLISTDTL.d_from             and COMLISTDTL.c_user_id       = '" + strUserId + "'  AND COMLISTDTL.c_committee_id  = COMDTL.c_committee_id   AND COMLISTDTL.s_committee_seq = COMDTL.s_committee_seq   AND COMDTL.d_from             <= COMLIST_B.d_from   AND COMLIST_B.d_from          <= COMDTL.d_to   AND PER.c_period_id = COMLIST_B.c_period_id   AND PER.k_period_kind = '01'  order by COMLISTDTL.c_committee_id" & UtDb.DbOrderOffset()
            'todo:
            Dim dbAccess As New CLAccessMdb
            Dim dt As New DataTable("committee_list_dtl")
            Call dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            Call dbAccess.Disconnect()
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
        Return table2
    End Function

    '************************************************************************************
    '   ＩＤ　：setCmbYear
    '   名称　：年コンボボックス設定処理
    '   概要　：選択した委員会の有効範囲年を抽出し、対象年コンボボックス選択肢に設定
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24(木)  Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木)  Ryu  新規作成
    '************************************************************************************
    ''' <summary>年コンボボックス設定処理</summary>
    ''' <remarks></remarks>
    Private Sub setCmbYear()
        Dim dbAccess As New CLAccessMdb             'DBアクセスクラス
        Dim d_from As String                        '期の開始日
        Dim d_to As String                          '期の終了日
        Dim dtRow As DataRow                        '一行のデータ
        Dim strKiValue As String                    '期のコード
        Try
            strKiValue = "001"
            If strKiValue <> "" Then
                '現在の選択肢をクリア
                cmbYear.Items.Clear()
                Dim table As DataTable = GetPeriodFromTo("001", Format(Now, DATE_YYYYMMDD_8_FORMAT), "中央執行委員会")
                If table.Rows.Count > 0 Then
                    dtRow = table.Rows(0)
                    d_from = Mid(dtRow("d_from"), 1, 4)
                    d_to = Mid(dtRow("d_to"), 1, 4)
                    Do Until d_from > d_to
                        cmbYear.Items.Add(d_from)
                        d_from = CStr(CInt(d_from) + 1)
                    Loop
                    cmbYear.SelectedIndex = 0
                Else
                    '期の期間取得できなかった場合、エラー
                    CLMsg.Show("GE0004", COMMITTEE_KI_KIKAN_NAME)
                End If
                Call dbAccess.Disconnect()
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

    '***************************************************************************************************
    '   ＩＤ　：setGrant
    '   名称　：権限取得処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>権限取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function setGrant() As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim dtGrant As DataTable = Nothing                                              ' 権限取得データテーブル
        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC030301)
            If dtGrant.Rows.Count > 0 Then
                strGrantReference = dtGrant.Rows(0).Item(3).ToString                    ' 参権限照
                strGrantInsert = dtGrant.Rows(0).Item(4).ToString                       ' 登録権限
                strGrantPrint = dtGrant.Rows(0).Item(5).ToString                        ' 印刷権限
                strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString                   ' ファイル出力権限
            End If
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                   ' 戻り値設定
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
        Dim retBln As Boolean = False                                                   '戻り値
        Dim strYear As String                                                           ' 年
        Dim strMonth As String                                                          ' 月
        Dim intDays As Integer                                                          ' 日数字
        Dim iCounter As Integer                                                         ' カラムカウンター
        Dim searchDate As Date                                                          ' 日付
        Dim strDay As String                                                            ' 日の文字列
        Dim dbAccess As New CLAccessMdb                                                 ' DBアクセス
        Dim dt As DataTable                                                             ' データテーブル
        Dim dtrow As DataRow                                                            ' 一行のデータ
        Dim sql As String                                                               ' SQL分
        Dim iRowCounter As Integer                                                      ' 行数カウンター
        Dim dtAtt As DataTable                                                          ' 出欠情報データテーブル
        Dim dtrowAtt As DataRow                                                         ' 一行のデータ
        Dim strCellStyle As String                                                      ' 委員会IDと枝番でセルスタイルと変える
        Dim strCellStyleBlue As String
        Dim oneCell As CellStyle                                                        ' セルスタイル
        Dim iRowCounterRoll As Integer                                                  ' 行カウンター
        Dim dtRoll As DataTable                                                         ' 日当データテーブル
        Dim dtrowRoll As DataRow                                                        ' 一行のデータ
        Dim strDate As String                                                           ' 出欠のあった日付
        Dim intDay As Integer                                                           ' 日
        Dim dtLName As DataTable = Nothing                                              ' データテーブル
        Dim dtrowLName As DataRow = Nothing                                             ' 一行のデータ
        Dim intDaysList As List(Of String)                                              ' 出欠のある日リスト
        Dim strDateStart As String                                                      ' 月の開始日付
        Dim strDateEnd As String                                                        ' 月の終了日付
        Dim strDateStand As String                              '月の開始日(yyyyMMdd型)

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            ' 入力チェック
            If CheckInput() = False Then
                Exit Function
            End If

            ' しばらくお待ちくださいフォーム表示
            FrmWaitInfo.ShowWaitForm(Nothing)

            ' 入力チェック通った場合、検索を行う
            Cursor.Current = Cursors.WaitCursor
            strYear = cmbYear.Text
            strMonth = cmbMonth.Text
            intDays = Date.DaysInMonth(CInt(strYear), CInt(strMonth))
            strDateStand = strYear + strMonth + "01"
            strDateStart = strYear + "/" + strMonth + "/" + "01"
            strDateEnd = strYear + "/" + strMonth + "/" + CStr(intDays)
            flxAttendance.Rows.Count = 1
            flxAttendance.Cols.Count = intDays + 5
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
            dbAccess.Connect()
            dt = GetAttendanceData("001", strYear + strMonth + "01")
            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    flxAttendance.Rows.Add(dt.Rows.Count * 2)
                    For iRowCounter = 0 To dt.Rows.Count - 1
                        For iCounter = 1 To intDays
                            flxAttendance.SetCellStyle(iRowCounter * 2 + 1, iCounter + 4, "normal")
                            flxAttendance.SetCellStyle(iRowCounter * 2 + 2, iCounter + 4, "normal")
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
                        strCellStyle = "001" + CStr(dtrow("s_committee_seq"))
                        strCellStyleBlue = "001" + CStr(dtrow("s_committee_seq")) + "blue"
                        '役職に付き日当IDと日当枝番（日当選択肢）を取得
                        sql = "select daily_pay_master_dtl.c_daily_pay_id,daily_pay_master_dtl.c_menu_seq,daily_pay_master_dtl.l_name,daily_pay_master_dtl.l_explain from daily_pay_master_dtl "
                        sql = sql & "right join (select c_daily_pay_id from committee_dtl where c_committee_id='001' and s_committee_seq='" + CStr(dtrow("s_committee_seq")) + "' and d_from <= '" & strDateStand & "' and d_to >= '" & strDateStand & "' )as pay_id on daily_pay_master_dtl.c_daily_pay_id=pay_id.c_daily_pay_id "
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
                        sql = "select format(call_roll_user_dtl.s_day,'yyyy/MM/dd')as s_day,call_roll_user_dtl.c_committee_id,call_roll_user_dtl.s_committee_seq,call_roll_user_dtl.c_daily_pay_id,call_roll_user_dtl.c_menu_seq,call_roll_user_dtl.k_food_expenses "
                        sql = sql & "from call_roll_user_dtl where call_roll_user_dtl.c_user_id='" + dtrow("c_user_id") + "' and format(call_roll_user_dtl.s_day,'yyyy/MM/dd')>='" + strDateStart + "' and format(call_roll_user_dtl.s_day,'yyyy/MM/dd')<='" + strDateEnd + "'"
                        dtRoll = dbAccess.ExecuteSql(sql)
                        If dtRoll.Rows.Count > 0 Then
                            '日当IDを記録
                            For iRowCounterRoll = 0 To dtRoll.Rows.Count - 1
                                dtrowRoll = dtRoll.Rows(iRowCounterRoll)
                                strDate = dtrowRoll("s_day")
                                intDay = Date.Parse(strDate).Day
                                intDaysList.Add(CStr(intDay))
                                ' 2016/09/29 中執活動でもＤＧＭの場合はロック対応が必要
                                ' 2017/09/01 役職まで見てしまうと、月途中に変わった場合に対応できない
                                '現在委員会での活動の場合
                                If CStr(dtrowRoll("c_committee_id")) = "001" Then
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
                                    '他の部／委員会での活動の場合セルは修正不可にする
                                    flxAttendance.SetCellStyle(iRowCounter * 2 + 1, intDay + 4, "lightYellowCell")
                                    flxAttendance.SetCellStyle(iRowCounter * 2 + 2, intDay + 4, "boolYellow")
                                    flxAttendance.SetCellCheck(iRowCounter * 2 + 2, intDay + 4, CheckEnum.Unchecked)
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
            dbAccess.Disconnect()
            '個人認証ID、委員会ＩＤ枝番（役職ID）カラムを非表示
            flxAttendance.Cols(2).Visible = False
            flxAttendance.Cols(3).Visible = False
            flxAttendance.Cols(4).Visible = False
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
            For m = 2 To Me.flxAttendance.Cols.Count - 1
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
            btnUpdate.Visible = getCloseDateCommOrUser("001")
            '所属委員がいないとき、内容変更ボタンを非表示
            If flxAttendance.Rows.Count <= 1 Then
                btnUpdate.Visible = False
            End If
            ' 権限判定
            If Me.btnUpdate.Visible Then
                If strGrantInsert = GRANT_VALID Then
                    Me.btnUpdate.Enabled = True     ' 内容変更ボタン使用可
                ElseIf strGrantInsert = GRANT_VOID Then
                    Me.btnUpdate.Enabled = False    ' 内容変更ボタン使用不可
                End If
            End If

            'GRID再描化
            flxAttendance.Redraw = True
            grpResult.Visible = True
            '戻り値設定
            retBln = True
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            FrmWaitInfo.CloseWaitForm()
            Cursor.Current = Cursors.Default
        End Try
        Return retBln
    End Function
#End Region

    Private Sub cmbMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMonth.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            '検索主処理呼び出し
            Call Me.searchMain()
        End If

    End Sub

End Class

#End Region
