#Region "UC020203"
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDCommon
Imports C1.Win.C1FlexGrid

Public Class UC020203

#Region "定数・変数"
    Private Const SCHEDULE_HOUR_00_FORMAT As String = "00"                              '時間を２桁に整形するためのフォーマット
    Private Const SCHEDULE_SEQ_START As String = "0001"                                 '年月に最初の日程の場合の開始シーケンス
    Private Const SCHEDULE_TIMEREQUERE_TEXT As String = "__時間__分"                    'テキスト：__時間__分
    Public bln As Boolean                                                               '入力チェック結果
    Private Const SCHEDULE_SHIBU_NAME As String = "支部"                                '名称：支部
    Private Const SCHEDULE_COMMITTE_NAME As String = "委員会名"                         '名称：委員会名
    Private Const SCHEDULE_DATETIMEPICKER_CUSTOMFORMAT_TIME As String = "HH時mm分"      'DATETIMEPICKERの時間選択フォーマット
    Private Const SCHEDULE_BUTTON_LABEL_CANCEL As String = "キャンセル"                 'ボタンのアクション定義
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
    '************************************************************************************
    '   ＩＤ　：UC020203_Load
    '   名称　：ボタン制御
    '   概要　：ログイン権限によるボタン制御
    '   作成日：2011/12/22(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/12/22(木) Ryu  新規作成
    '************************************************************************************
    Private Sub UC020203_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dt As DataTable = Nothing
        Dim strRead As String = ""
        Dim strReg As String = ""
        Dim strPrint As String = ""
        Dim strFile As String = ""
        Try
            dt = MDCommon.getGrant(MENU_ID_UC020201)
            If dt.Rows.Count > 0 Then
                strRead = dt.Rows(0).Item(3).ToString
                strReg = dt.Rows(0).Item(4).ToString
                strPrint = dt.Rows(0).Item(5).ToString
                strFile = dt.Rows(0).Item(6).ToString
                btnDelete.Enabled = CInt(strReg)
                btnChange.Enabled = CInt(strReg)
            End If
        Catch ex As Exception

        End Try
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnBack_Click
    '   名称　：戻るボタン
    '   概要　：日程表一覧画面へ遷移
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim pn As Panel                             'メインパネル
        Dim uc As Control                           '画面コントロール
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If Me.btnChange.Text = KANJI_BUTTON_LABEL_NAIYOUHENKOU Then
                Me.Visible = False

                pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                uc = pn.Controls(SCREEN_ID_UC020201)

                If uc Is Nothing Then
                    uc = New UC020201
                    Call pn.Controls.Add(uc)
                Else
                    uc.Visible = True
                End If
                Me.Dispose()
            ElseIf CLMsg.Show("GQ0007") = DialogResult.Yes Then
                Me.Visible = False

                pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                uc = pn.Controls(SCREEN_ID_UC020201)

                If uc Is Nothing Then
                    uc = New UC020201
                    Call pn.Controls.Add(uc)
                Else
                    uc.Visible = True
                End If
                uc.Controls("fraSchedule").Visible = False
                Me.Dispose()
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "btnBack_Click")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：dtpEnd_LostFocus
    '   名称　：終了時間を離れたとき
    '   概要　：入力チェック
    '           所要時間を計算
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub dtpEnd_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpEnd.LostFocus
        Dim strStartTime As String                                  '開始時間
        Dim strEndTime As String                                    '終了時間
        Dim tmpValue As Integer                                     '所要時間
        Dim strHour As String                                       '所要時間　時
        Dim strMinute As String                                     '所要時間　分
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strStartTime = Format(CInt(Me.dtpStart.Value.Hour), SCHEDULE_HOUR_00_FORMAT) + Format(CInt(Me.dtpStart.Value.Minute), SCHEDULE_HOUR_00_FORMAT)
            strEndTime = Format(CInt(Me.dtpEnd.Value.Hour), SCHEDULE_HOUR_00_FORMAT) + Format(CInt(Me.dtpEnd.Value.Minute), SCHEDULE_HOUR_00_FORMAT)
            If strEndTime < strStartTime Then
                CLMsg.Show("GI0009")
                Me.txtTime.Text = SCHEDULE_TIMEREQUERE_TEXT
            Else
                tmpValue = (DateDiff(DateInterval.Second, dtpStart.Value, dtpEnd.Value) + 1) / 60
                strHour = CStr(Format(CInt(tmpValue \ 60), SCHEDULE_HOUR_00_FORMAT))
                strMinute = CStr(Format(CInt(tmpValue Mod 60), SCHEDULE_HOUR_00_FORMAT))
                Me.txtTime.Text = strHour + MDConst.KANJI_JIKAN_HOUR + strMinute + MDConst.KANJI_JIKAN_MINUTE
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "dtpEnd_LostFocus")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
    Private Sub dtpStart_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpStart.LostFocus
        dtpEnd.Focus()
    End Sub
    '************************************************************************************
    '   ＩＤ　：cmbCommitteName_LostFocus
    '   名称　：委員会名入力を離れたとき
    '   概要　：委員会名入力を離れたとき、初回だった場合、入力した内容を表示名称にセット
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub cmbCommitteName_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCommitteName.LostFocus
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If Not bln Then
                Me.txtScheduleName.Text = Me.cmbCommitteName.Text
            End If
            bln = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "cmbCommitteName_LostFocus")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnChange_Click
    '   名称　：確認登録ボタン
    '   概要　：入力チェック
    '           画面遷移
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click

        'ボタンは登録確認の場合、入力チェックの上、画面遷移
        Dim strEndDate As String                        '終了日付
        Dim endDateChecked As Boolean                   '終了日付の入力有無
        Dim strStartDate As String                      '開始日付
        Dim dbAccess As New CLAccessMdb                 'DBアクセス
        Dim sql As String                               'SQL分
        Dim d_from As String                            '期の開始日
        Dim d_to As String                              '期の終了日
        Dim dt As DataTable                             'SQLの実行結果データセット
        Dim dtRow As DataRow                            '一行のデータ
        Dim errMsg As New ArrayList                    'エラーメッセージ配列
        Dim fmPrint As FM000203                         '印刷プレビュー画面
        Dim pn As Panel                                 'メインパネル
        Dim uc As Control                               '画面コントロール
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            Cursor.Current = Cursors.WaitCursor
            If Me.btnChange.Text = KANJI_BUTTON_LABEL_TOUROKUKAKUNIN Then
                If Trim(Me.cmbCommitteName.Text) = "" Then
                    errMsg.Add(CLMsg.GetMsg("GE0006", "委員会名"))
                    Me.cmbCommitteName.BackColor = Color.Pink
                End If
                If ChkLengthB(Me.cmbCommitteName.Text, 100) = False Then
                    errMsg.Add(CLMsg.GetMsg("GE0112", "項目『委員会名』に", "100", "50"))
                    Me.cmbCommitteName.BackColor = Color.Pink
                End If
                If Me.cmbCommitteName.Text <> "" And ChkLengthB(Me.cmbCommitteName.Text, 100) Then
                    Me.cmbCommitteName.BackColor = Color.White
                End If
                If Me.cmbBranch.SelectedIndex = -1 Then
                    errMsg.Add(CLMsg.GetMsg("GE0006", "支部"))
                    Me.cmbBranch.BackColor = Color.Pink
                Else
                    Me.cmbBranch.BackColor = Color.White
                End If
                If Me.txtScheduleName.Text = "" Then
                    errMsg.Add(CLMsg.GetMsg("GE0006", "日程表表示名称"))
                    Me.txtScheduleName.BackColor = Color.Pink
                End If
                If ChkLengthB(Me.txtScheduleName.Text, 200) = False Then
                    errMsg.Add(CLMsg.GetMsg("GE0112", "項目『日程表表示名称』に", "200", "100"))
                    Me.txtScheduleName.BackColor = Color.Pink
                End If
                If Me.txtScheduleName.Text <> "" And ChkLengthB(Me.txtScheduleName.Text, 200) Then
                    Me.txtScheduleName.BackColor = Color.White
                End If
                If ChkLengthB(Me.txtLocation.Text, 200) = False Then
                    errMsg.Add(CLMsg.GetMsg("GE0112", "項目『場所』に", "200", "100"))
                    Me.txtLocation.BackColor = Color.Pink
                Else
                    Me.txtLocation.BackColor = Color.White
                End If
                If ChkLengthB(Me.txtMokuteki.Text, 600) = False Then
                    errMsg.Add(CLMsg.GetMsg("GE0112", "項目『開催目的』に", "600", "300"))
                    Me.txtMokuteki.BackColor = Color.Pink
                Else
                    Me.txtMokuteki.BackColor = Color.White
                End If
                If ChkLengthB(Me.txtKatai.Text, 200) = False Then
                    errMsg.Add(CLMsg.GetMsg("GE0112", "項目『主な議題』に", "200", "100"))
                    Me.txtKatai.BackColor = Color.Pink
                Else
                    Me.txtKatai.BackColor = Color.White
                End If
                If ChkLengthB(Me.txtBikou.Text, 200) = False Then
                    errMsg.Add(CLMsg.GetMsg("GE0112", "項目『備考題』に", "200", "100"))
                    Me.txtBikou.BackColor = Color.Pink
                Else
                    Me.txtBikou.BackColor = Color.White
                End If

                strEndDate = Format(Me.dtpEndDate.Value.Date, MDConst.DATE_YYYYMMDD_FORMAT)
                endDateChecked = Me.dtpEndDate.Checked
                strStartDate = Format(Date.Parse(Replace(Me.lblStartDate.Text, " ", "")), MDConst.DATE_YYYYMMDD_FORMAT)
                If Trim(CStr(Me.lblSEQ.Text)) = "" Then
                    'lblSEQに値がなければ新規登録
                    '範囲の終了日をチェック
                    If endDateChecked And strEndDate < strStartDate Then
                        errMsg.Add("GE0006：終了日は開始日より遅い日付にしてください。")
                    End If

                    '終了日がチェックされている場合、終了日が期の範囲内チェック
                    If endDateChecked Then
                        dbAccess.Connect()
                        sql = "Select d_from,d_to from period where c_period_id='" + Me.lblPeriodValue.Text + "'"
                        dt = dbAccess.ExecuteSql(sql)
                        If dt.Rows.Count > 0 Then
                            dtRow = dt.Rows(0)
                            d_from = Format(Date.Parse(Mid(dtRow("d_from"), 1, 4) + "/" + Mid(dtRow("d_from"), 5, 2) + "/" + Mid(dtRow("d_from"), 7, 2)), MDConst.DATE_YYYYMMDD_FORMAT)
                            d_to = Format(Date.Parse(Mid(dtRow("d_to"), 1, 4) + "/" + Mid(dtRow("d_to"), 5, 2) + "/" + Mid(dtRow("d_to"), 7, 2)), MDConst.DATE_YYYYMMDD_FORMAT)
                            If strEndDate > d_to Or strEndDate < d_from Then
                                errMsg.Add("選択した開催日は期の開始と終了期間外のため、登録できません。（対象期間：" + d_from + "～" + d_to + "）")
                                Me.dtpEndDate.Focus()
                            End If
                        Else
                            errMsg.Add("「期」の開始と終了期間が取得できませんでした。システム管理者へ連絡してください。")
                        End If
                        dbAccess.Disconnect()
                    End If

                    If errMsg.Count > 0 Then
                        '入力チェックエラーとなった場合、メッセージ出力する
                        ' エラーメッセージボックス表示
                        Dim clsUC999999 As New UC999999     ' メッセージボックスクラス生成
                        clsUC999999.errMsgList = errMsg
                        ' メインメニュー画面表示
                        Call clsUC999999.ShowDialog()
                    Else
                        '入力チェック通った場合、日程登録前の確認プレビュー画面へ遷移
                        fmPrint = New FM000203
                        If endDateChecked Then
                            createPrePrint(fmPrint, strStartDate, strEndDate, lblKindText.Text, txtScheduleName.Text, True)
                        Else
                            createPrePrint(fmPrint, strStartDate, strStartDate, lblKindText.Text, txtScheduleName.Text, True)
                        End If
                        'DB登録
                        Select Case fmPrint.IntQlickBtnFlag
                            Case 0
                                '登録＆印刷
                                fmPrint.PrintOut()
                                '同期処理による最新データの取得
                                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                                'REV更新
                                UpdateRev(strStartDate, strEndDate, True)
                                If endDateChecked Then
                                    '終了日が選択されている場合、開始日から終了日まで毎日の分の日程を登録
                                    Dim tmpDateValue As String
                                    Dim newDate As Date
                                    tmpDateValue = strStartDate
                                    Do Until tmpDateValue > strEndDate
                                        Call inserSchedule(tmpDateValue)
                                        newDate = DateAdd(DateInterval.Day, 1, Date.Parse(tmpDateValue))
                                        tmpDateValue = Format(newDate, MDConst.DATE_YYYYMMDD_FORMAT)
                                    Loop
                                Else
                                    '終了日が選択されていない場合、開始日の分だけの日程を登録
                                    Call inserSchedule(strStartDate)
                                End If
                                '同期処理による最新データの反映
                                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                                Cursor.Current = Cursors.Default
                                '日程表一覧画面へ遷移
                                Me.Visible = False
                                pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                                uc = pn.Controls(SCREEN_ID_UC020201)

                                If uc Is Nothing Then
                                    uc = New UC020201
                                    Call pn.Controls.Add(uc)
                                Else
                                    uc.Visible = True
                                End If
                                uc.Controls("fraSchedule").Visible = False
                                Me.Dispose()
                            Case 1
                                '登録のみ
                                '同期処理による最新データの取得
                                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                                'REV更新
                                UpdateRev(strStartDate, strEndDate, False)
                                If endDateChecked Then
                                    '終了日が選択されている場合、開始日から終了日まで毎日の分の日程を登録
                                    Dim tmpDateValue As String
                                    Dim newDate As Date
                                    tmpDateValue = strStartDate
                                    Do Until tmpDateValue > strEndDate
                                        Call inserSchedule(tmpDateValue)
                                        newDate = DateAdd(DateInterval.Day, 1, Date.Parse(tmpDateValue))
                                        tmpDateValue = Format(newDate, MDConst.DATE_YYYYMMDD_FORMAT)
                                    Loop
                                Else
                                    '終了日が選択されていない場合、開始日の分だけの日程を登録
                                    Call inserSchedule(strStartDate)
                                End If
                                '同期処理による最新データの反映
                                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                                Cursor.Current = Cursors.Default
                                '日程表一覧画面へ遷移
                                Me.Visible = False
                                pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                                uc = pn.Controls(SCREEN_ID_UC020201)
                                If uc Is Nothing Then
                                    uc = New UC020201
                                    Call pn.Controls.Add(uc)
                                Else
                                    uc.Visible = True
                                End If
                                uc.Controls("fraSchedule").Visible = False
                                Me.Dispose()
                            Case 2
                                'キャンセル
                                Cursor.Current = Cursors.Default
                        End Select
                    End If
                Else
                    'lblSEQに値があれば更新
                    '入力チェック通った場合
                    If errMsg.Count > 0 Then
                        '入力チェックエラーとなった場合、メッセージ出力する
                        ' エラーメッセージボックス表示
                        Dim clsUC999999 As New UC999999     ' メッセージボックスクラス生成
                        clsUC999999.errMsgList = errMsg
                        ' メインメニュー画面表示
                        Call clsUC999999.ShowDialog()
                    Else
                        fmPrint = New FM000203
                        strStartDate = Replace(lblStartDate.Text, " ", "")
                        '変更となった日程をFLEXGRID上の内容を変更し、変更後のFLEXGRIDの内容を印刷に出力する
                        changeGridDataBySEQ(lblSEQ.Text, txtScheduleName.Text)
                        '印刷プレビュー画面表示
                        createPrePrint(fmPrint, strStartDate, strStartDate, "", "", False)
                        Select Case fmPrint.IntQlickBtnFlag
                            Case 0
                                '登録＆印刷
                                fmPrint.PrintOut()
                                '同期処理による最新データの取得
                                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                                'REVを更新
                                UpdateRev(strStartDate, strStartDate, True)
                                'lblSEQに値があれば上書き登録
                                Call updateSchedule(CStr(Me.lblSEQ.Text), Mid(strStartDate, 9, 2))
                                '同期処理による最新データの反映
                                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                                '日程表一覧画面へ遷移
                                Me.Visible = False
                                pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                                uc = pn.Controls(SCREEN_ID_UC020201)
                                If uc Is Nothing Then
                                    uc = New UC020201
                                    Call pn.Controls.Add(uc)
                                Else
                                    uc.Visible = True
                                End If
                                uc.Controls("fraSchedule").Visible = False
                                Me.Dispose()
                            Case 1
                                '登録のみ
                                '同期処理による最新データの取得
                                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                                'REVを更新
                                UpdateRev(strStartDate, strStartDate, False)
                                'lblSEQに値があれば上書き登録
                                Call updateSchedule(CStr(Me.lblSEQ.Text), Mid(strStartDate, 9, 2))
                                '同期処理による最新データの反映
                                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                                '日程表一覧画面へ遷移
                                Me.Visible = False
                                pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                                uc = pn.Controls(SCREEN_ID_UC020201)
                                If uc Is Nothing Then
                                    uc = New UC020201
                                    Call pn.Controls.Add(uc)
                                Else
                                    uc.Visible = True
                                End If
                                uc.Controls("fraSchedule").Visible = False
                                Me.Dispose()
                            Case 2
                                'キャンセル
                                Cursor.Current = Cursors.Default
                        End Select
                    End If
                End If
            Else
                'ボタンは内容変更の場合、内容変更画面
                EnabledUC020203(Me, True, Color.White)
                Me.btnDelete.Visible = False
                Me.btnChange.Text = KANJI_BUTTON_LABEL_TOUROKUKAKUNIN
                Me.btnBack.Text = SCHEDULE_BUTTON_LABEL_CANCEL
            End If
        Catch ex As Exception
            Cursor.Current = Cursors.Default
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "btnChange_Click")
            log.Fatal(ex.Message)
        Finally
            Cursor.Current = Cursors.Default
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnDelete_Click
    '   名称　：日程を削除
    '   概要　：表示されている日程を削除
    '   作成日：2011/11/15(火) Ryu
    '   更新日：2017/12/01(金) Fujisaku
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '       　：2017/12/01(金) Fujisaku  日付のキー追加
    '************************************************************************************
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim sql As String                           'SQL文
        Dim dbAccess As New CLAccessMdb             'DBアクセス
        Dim strSEQ As String                        '日程のユニックキー
        Dim strDate As String                       '日付のユニックキー
        Dim pn As Panel                             'メインパネル
        Dim uc As Control                           '画面コントロール
        Dim intRtn As Integer                       'SQL文実行結果
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        strSEQ = CStr(Me.lblSEQ.Text)
        strDate = Mid(Replace(Me.lblStartDate.Text, " ", ""), 9, 2)
        Try
            If strSEQ <> "" Then
                '確認ダイアログ表示
                If CLMsg.Show("GQ0011") = DialogResult.Yes Then
                    sql = "Delete from schedule_dtl_list where s_schedule_seq='" + strSEQ + "' and d_date='" + strDate + "'"
                    dbAccess.Connect()
                    dbAccess.BeginTran()
                    intRtn = dbAccess.ExecuteNonQuery(sql)
                    If intRtn = 1 Then
                        dbAccess.CommitTran()
                        '同期処理による最新データの反映
                        ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                        log.Info(String.Format("{0}件のデータを削除しました。", CStr(intRtn)))
                    Else
                        dbAccess.RollbackTran()
                        CLMsg.Show("GE0204")
                        log.Error("DB削除処理に異常があったためデータ削除を中止しました。")
                    End If
                    dbAccess.Disconnect()
                    '削除終了し、日程表画面へ遷移
                    Me.Visible = False
                    pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                    uc = pn.Controls(SCREEN_ID_UC020201)
                    If uc Is Nothing Then
                        uc = New UC020201
                        Call pn.Controls.Add(uc)
                    Else
                        uc.Visible = True
                    End If
                    '日程表フレームを非表示
                    uc.Controls("fraSchedule").Visible = False
                    Me.Dispose()
                End If
            Else
                CLMsg.Show("FE0001")
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "btnDelete_Click")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "関数"
    '************************************************************************************
    '   ＩＤ　：inserSchedule
    '   名称　：日程をDBに登録
    '   概要　：入力した情報を日程DBに登録
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub inserSchedule(ByVal strDate As String)
        Dim dbAccess As New CLAccessMdb
        Dim strc_ksh As String                          '会社コード（グローバル変数より取得予定）
        Dim sql As String                               'SQL分
        Dim dt As DataTable                             'SQLの実行結果データセット
        Dim dtRow As DataRow                            '一行のデータ
        Dim strSEQ As String                            '日程枝番取得
        Dim intRtn As Integer                           'SQL文実行結果
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strc_ksh = MDLoginInfo.Ksh
            sql = "select s_schedule_seq from schedule_dtl_list where d_month='" + strDate.Substring(0, 4) + strDate.Substring(5, 2) + "' Order by s_schedule_seq DESC" 'chk
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                dtRow = dt.Rows(0)
                strSEQ = dtRow("s_schedule_seq")
                strSEQ = CStr(CLng(strSEQ) + 1)
            Else
                'その月に最初の日程の場合、枝番に0001を設定
                strSEQ = strDate.Substring(0, 4) + strDate.Substring(5, 2) + SCHEDULE_SEQ_START
            End If
            sql = "Insert into schedule_dtl_list(c_ksh,c_period_id,d_month,s_schedule_seq,d_date,l_list_name,k_schedule_divide,k_union,l_information_name,l_open_object,l_subject,l_place,d_time_start,d_time_end,d_time_required,l_biko,d_ins,c_user_id_ins,d_up,c_user_id_up,s_up) values('"
            '会社コード
            sql = sql + strc_ksh + "','"
            '期コード
            sql = sql + Me.lblPeriodValue.Text + "','"
            '年月
            sql = sql + strDate.Substring(0, 4) + strDate.Substring(5, 2) + "','"
            '日程枝番
            sql = sql + strSEQ + "','"
            '日
            sql = sql + strDate.Substring(8, 2) + "','"
            '表示名称－委員会名
            sql = sql + Me.cmbCommitteName.Text + "','"
            '日程表分類区分
            sql = sql + Me.lblKindText.Text + "','"
            '合同区分
            sql = sql + Me.cmbBranch.SelectedValue + "','"
            '表示名称
            sql = sql + Me.txtScheduleName.Text + "','"
            '開催目的
            sql = sql + Me.txtMokuteki.Text + "','"
            '主な議題
            sql = sql + Me.txtKatai.Text + "','"
            '場所
            sql = sql + Me.txtLocation.Text + "','"
            '開始時間
            sql = sql + Format(Date.Parse(Me.dtpStart.Value), MDConst.DATE_HHMM_FORMAT) + "','"
            '終了時間
            sql = sql + Format(Date.Parse(Me.dtpEnd.Value), MDConst.DATE_HHMM_FORMAT) + "','"
            '所要時間
            sql = sql + Me.txtTime.Text + "','"
            '備考
            sql = sql + Me.txtBikou.Text + "','"
            '作成日
            sql = sql + Now() + "','"
            '作成者
            sql = sql + MDLoginInfo.UserId + "','"
            '更新日
            sql = sql + Now + "','"
            '更新者
            sql = sql + MDLoginInfo.UserId + "','"
            's_upを01に設定
            sql = sql + "1')"
            dbAccess.BeginTran()
            intRtn = dbAccess.ExecuteNonQuery(sql)
            If intRtn = 1 Then
                dbAccess.CommitTran()
                log.Info(String.Format("{0}件のデータを追加しました。", CStr(intRtn)))
            Else
                dbAccess.RollbackTran()
                CLMsg.Show("GE0204")
                log.Error("DB更新処理に異常があったためデータ追加を中止しました。")
            End If
            dbAccess.Disconnect()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "inserSchedule")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：updateSchedule
    '   名称　：日程を更新
    '   概要　：入力した情報を日程DBに登録
    '   作成日：2011/11/04(木) Ryu
    '   更新日：2017/12/01(金) Fujisaku  
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '       　：2017/12/01(金) Fujisaku  日付の引数追加
    '************************************************************************************
    Private Sub updateSchedule(ByVal strSEQ As String, ByVal strDate As String)
        Dim dbAccess As New CLAccessMdb                 'DBアクセス
        Dim sql As String                               'SQL文
        Dim intRtn As Integer                           'SQL文実行結果
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            sql = "Update schedule_dtl_list set l_list_name='"
            '表示名称－委員会名
            sql = sql + Me.cmbCommitteName.Text + "',"
            sql = sql + "k_union='"
            '合同区分
            sql = sql + Me.cmbBranch.SelectedValue + "',"
            '表示名称
            sql = sql + "l_information_name='"
            sql = sql + Me.txtScheduleName.Text + "',"
            '開催目的
            sql = sql + "l_open_object='"
            sql = sql + Me.txtMokuteki.Text + "',"
            '主な議題
            sql = sql + "l_subject='"
            sql = sql + Me.txtKatai.Text + "',"
            '場所
            sql = sql + "l_place='"
            sql = sql + Me.txtLocation.Text + "',"
            '開始時間
            sql = sql + "d_time_start='"
            sql = sql + Format(Date.Parse(Me.dtpStart.Value), MDConst.DATE_HHMM_FORMAT) + "',"
            '終了時間
            sql = sql + "d_time_end='"
            sql = sql + Format(Date.Parse(Me.dtpEnd.Value), MDConst.DATE_HHMM_FORMAT) + "',"
            '所要時間
            sql = sql + "d_time_required='"
            sql = sql + Me.txtTime.Text + "',"
            '備考
            sql = sql + "l_biko='"
            sql = sql + Me.txtBikou.Text + "',"
            '更新日
            sql = sql + "d_up='" + Now + "',"
            '更新者
            sql = sql + "c_user_id_up='" + MDLoginInfo.UserId + "',"
            '更新回数
            sql = sql + "s_up=s_up+1 "
            sql = sql + " where s_schedule_seq='" + strSEQ + "'"
            sql = sql + " and d_date='" + strDate + "'"
            dbAccess.Connect()
            dbAccess.BeginTran()
            intRtn = dbAccess.ExecuteNonQuery(sql)
            If intRtn = 1 Then
                dbAccess.CommitTran()
                log.Info(String.Format("{0}件のデータを更新しました。", CStr(intRtn)))
            Else
                dbAccess.RollbackTran()
                CLMsg.Show("GE0204")
                log.Error("DB更新処理に異常があったためデータ追加を中止しました。")
            End If
            dbAccess.Disconnect()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "updateSchedule")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：EnabledUC020203
    '   名称　：UC020203画面項目の編集可・不可の制御
    '   概要　：選択した日程情報で画面を初期化する
    '   作成日：2011/11/11(木) Ryu
    '   引数　：Control  画面
    '           flg 編集可true、編集不可false
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Sub EnabledUC020203(ByVal uc As Control, ByVal flg As Boolean, ByVal backColor As Color)
        Dim fraScheduleDetail As GroupBox           '分類フレーム
        Dim fraScheduleKind As GroupBox             '詳細フレーム
        Dim cmbShibu As ComboBox                    '「支部」コンボボックス
        Dim cmbCommitte As ComboBox                 '「委員会名」コンボボックス
        Dim timePickerStart As DateTimePicker       '開始時間
        Dim timePickerEnd As DateTimePicker         '終了時間
        Dim txtScheduleName As TextBox              '表示名称
        Dim txtTime As TextBox                      '所要時間
        Dim txtLocation As TextBox                  '場所
        Dim txtMokuteki As TextBox                  '目的
        Dim txtKatai As TextBox                     '議題
        Dim txtBikou As TextBox                     '備考
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            fraScheduleKind = uc.Controls("fraScheduleKind")
            fraScheduleDetail = uc.Controls("fraScheduleDetail")

            cmbCommitte = fraScheduleDetail.Controls("cmbCommitteName")
            cmbCommitte.BackColor = backColor
            cmbCommitte.Enabled = flg
            cmbShibu = fraScheduleDetail.Controls("cmbBranch")
            cmbShibu.BackColor = backColor
            cmbShibu.Enabled = flg
            txtScheduleName = fraScheduleDetail.Controls("txtScheduleName")
            txtScheduleName.BackColor = backColor
            txtScheduleName.Enabled = flg
            txtScheduleName.ReadOnly = Not (flg)
            timePickerStart = fraScheduleDetail.Controls("dtpStart")
            timePickerStart.Enabled = flg
            timePickerEnd = fraScheduleDetail.Controls("dtpEnd")
            timePickerEnd.Enabled = flg
            txtTime = fraScheduleDetail.Controls("txtTime")
            txtTime.BackColor = backColor
            txtTime.Enabled = flg
            txtTime.ReadOnly = Not (flg)
            txtLocation = fraScheduleDetail.Controls("txtLocation")
            txtLocation.BackColor = backColor
            txtLocation.Enabled = flg
            txtLocation.ReadOnly = Not (flg)
            txtMokuteki = fraScheduleDetail.Controls("txtMokuteki")
            txtMokuteki.BackColor = backColor
            txtMokuteki.Enabled = flg
            txtMokuteki.ReadOnly = Not (flg)
            txtKatai = fraScheduleDetail.Controls("txtKatai")
            txtKatai.BackColor = backColor
            txtKatai.Enabled = flg
            txtKatai.ReadOnly = Not (flg)
            txtBikou = fraScheduleDetail.Controls("txtBikou")
            txtBikou.BackColor = backColor
            txtBikou.Enabled = flg
            txtBikou.ReadOnly = Not (flg)
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "EnabledC020203")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：createPrePrint
    '   名称　：印刷プレビュー画面表示
    '   概要　：新規日程表追加時のプレビュー画面表示
    '   引数　：fmPrint     プレビュー印刷画面
    '         ：startDate   範囲日程の開始日
    '         ：endDate     範囲日程の終了日
    '         ：strKind     日程表種類  
    '       　：strName     日程表表示名称
    '       　：bIsInsert   新規登録かアップデートか
    '               true    新規登録
    '               false   アップデート、この場合、印刷内容はFLEXGRIDから取得するのみ
    '   作成日：2011/11/21(月) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) Ryu  新規作成
    '************************************************************************************
    Private Sub createPrePrint(ByVal fmPrint As FM000203, ByVal startDate As String, ByVal endDate As String, ByVal strKind As String, ByVal strName As String, ByVal bIsInsert As Boolean)

        Dim pn As Panel                                                                 'メインパネル
        Dim uc As Control                                                               '遷移先コントロール
        Dim cfg As C1FlexGrid                                                           '日程表FLEXGRID
        Dim gb1 As GroupBox                                                             '分類グループボックス
        Dim gb2 As GroupBox                                                             '分類グループボックス
        Dim resourceObj As CrystalDecisions.CrystalReports.Engine.ReportDocument        'レポート'日程表FLEXGRID
        Dim ds As DS0202P1                                                              'データセット
        Dim iCounter As Integer                                                         '行数
        Dim preLineNum As String                                                        '直前の日
        Dim c_message_1 As String                                                       '中執データ
        Dim c_message_2 As String                                                       '専門部データ
        Dim c_message_3 As String                                                       '産別データ
        Dim dateEndDate As Date                                                         '終了日の日付
        Dim strRev As String                                                            'REVの値
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020201)
            gb1 = uc.Controls("fraSchedule")
            gb2 = uc.Controls("fraSearchOption")
            cfg = gb1.Controls("cfgScheduleList")

            resourceObj = New CR0202P1
            ds = New DS0202P1

            '終了日は次の月となった場合、当月末までの分だけ、印刷プレビューに表示する
            If Mid(startDate, 1, 7) < Mid(endDate, 1, 7) Then
                dateEndDate = DateAdd(DateInterval.Month, 1, Date.Parse(startDate))
                dateEndDate = Date.Parse(CStr(dateEndDate.Year) + "/" + CStr(dateEndDate.Month) + "/01")
                dateEndDate = DateAdd(DateInterval.Day, -1, dateEndDate)
                endDate = Format(dateEndDate, MDConst.DATE_YYYYMMDD_FORMAT)
            End If

            'REV取得
            strRev = getRev(startDate)

            startDate = Mid(startDate, 9, 2)
            endDate = Mid(endDate, 9, 2)
            Dim drDetail As DS0202P1.dtDetailRow
            preLineNum = "01"
            c_message_1 = ""
            c_message_2 = ""
            c_message_3 = ""
            For iCounter = 1 To cfg.Rows.Count - 1
                If preLineNum = cfg.GetData(iCounter, 0) Then
                    If cfg.GetDataDisplay(iCounter, 3) <> "" Then
                        If c_message_1 = "" Then
                            c_message_1 = cfg.GetData(iCounter, 3)
                        Else
                            c_message_1 = c_message_1 + vbCrLf + cfg.GetData(iCounter, 3)
                        End If
                    End If

                    If cfg.GetDataDisplay(iCounter, 5) <> "" Then
                        If c_message_2 = "" Then
                            c_message_2 = cfg.GetData(iCounter, 5)
                        Else
                            c_message_2 = c_message_2 + vbCrLf + cfg.GetData(iCounter, 5)
                        End If
                    End If
                    If cfg.GetDataDisplay(iCounter, 7) <> "" Then
                        If c_message_3 = "" Then
                            c_message_3 = cfg.GetData(iCounter, 7)
                        Else
                            c_message_3 = c_message_3 + vbCrLf + cfg.GetData(iCounter, 7)
                        End If
                    End If
                Else
                    drDetail = ds.dtDetail.NewRow
                    drDetail.BeginEdit()
                    drDetail.s_day = cfg.GetData(iCounter - 1, 0)
                    If bIsInsert Then
                        '新規追加の場合、印刷内容に新規追加分を追加する
                        If cfg.GetData(iCounter - 1, 0) >= startDate And cfg.GetData(iCounter - 1, 0) <= endDate Then
                            Select Case strKind
                                Case SCHEDULE_BUNRUI_INDEX_0
                                    If c_message_1 = "" Then
                                        c_message_1 = strName
                                    Else
                                        c_message_1 = c_message_1 + vbCrLf + strName
                                    End If
                                Case SCHEDULE_BUNRUI_INDEX_1
                                    If c_message_2 = "" Then
                                        c_message_2 = strName
                                    Else
                                        c_message_2 = c_message_2 + vbCrLf + strName
                                    End If
                                Case SCHEDULE_BUNRUI_INDEX_2
                                    If c_message_3 = "" Then
                                        c_message_3 = strName
                                    Else
                                        c_message_3 = c_message_3 + vbCrLf + strName
                                    End If
                                Case SCHEDULE_BUNRUI_INDEX_3
                                    If c_message_1 = "" Then
                                        c_message_1 = strName
                                    Else
                                        c_message_1 = c_message_1 + vbCrLf + strName
                                    End If
                                Case SCHEDULE_BUNRUI_INDEX_4
                                    If c_message_3 = "" Then
                                        c_message_3 = strName
                                    Else
                                        c_message_3 = c_message_3 + vbCrLf + strName
                                    End If
                            End Select
                        End If
                    End If
                    drDetail.c_meesage_1 = c_message_1
                    drDetail.c_meesage_2 = c_message_2
                    drDetail.c_meesage_3 = c_message_3
                    drDetail.EndEdit()
                    ds.dtDetail.Rows.Add(drDetail)
                    c_message_1 = ""
                    c_message_2 = ""
                    c_message_3 = ""
                    preLineNum = cfg.GetData(iCounter, 0)
                    If cfg.GetDataDisplay(iCounter, 3) <> "" Then
                        c_message_1 = cfg.GetData(iCounter, 3)
                    End If
                    If cfg.GetDataDisplay(iCounter, 5) <> "" Then
                        c_message_2 = cfg.GetData(iCounter, 5)
                    End If
                    If cfg.GetDataDisplay(iCounter, 7) <> "" Then
                        c_message_3 = cfg.GetData(iCounter, 7)
                    End If
                End If
            Next
            '最後のデータを追加
            drDetail = ds.dtDetail.NewRow
            drDetail.BeginEdit()
            drDetail.s_day = cfg.GetData(iCounter - 1, 0)
            If bIsInsert Then
                '新規追加の場合、印刷内容に新規追加分を追加する
                If cfg.GetData(iCounter - 1, 0) >= startDate And cfg.GetData(iCounter - 1, 0) <= endDate Then
                    Select Case strKind
                        Case SCHEDULE_BUNRUI_INDEX_0
                            If c_message_1 = "" Then
                                c_message_1 = strName
                            Else
                                c_message_1 = c_message_1 + vbCrLf + strName
                            End If
                        Case SCHEDULE_BUNRUI_INDEX_1
                            If c_message_2 = "" Then
                                c_message_2 = strName
                            Else
                                c_message_2 = c_message_2 + vbCrLf + strName
                            End If
                        Case SCHEDULE_BUNRUI_INDEX_2
                            If c_message_3 = "" Then
                                c_message_3 = strName
                            Else
                                c_message_3 = c_message_3 + vbCrLf + strName
                            End If
                    End Select
                End If
            End If
            drDetail.c_meesage_1 = c_message_1
            drDetail.c_meesage_2 = c_message_2
            drDetail.c_meesage_3 = c_message_3
            drDetail.EndEdit()
            ds.dtDetail.Rows.Add(drDetail)
            Dim drHeader As DS0202P1.dtHeaderRow
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.c_period = Replace(Replace(gb2.Controls("cmbSearchPeriod").Text, "第", ""), "期", "")
            drHeader.s_year = gb2.Controls("cmbSearchYear").Text
            drHeader.s_month = gb2.Controls("cmbSearchMonth").Text
            drHeader.c_date1 = cfg.Cols(3).Caption
            drHeader.c_date2 = cfg.Cols(7).Caption
            'scheduleテーブルよりREVを取得
            drHeader.d_rev = strRev
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)
            resourceObj.SetDataSource(ds)
            fmPrint.ButtonShowType = 1
            fmPrint.PrintCntVisible = True
            fmPrint.ObjResource = resourceObj
            Call fmPrint.ShowDialog()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "createPrePrint")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：getRev
    '   名称　：REVを取得
    '   概要　：印刷REVを取得
    '   作成日：2011/11/21(月) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) Ryu  新規作成
    '************************************************************************************
    Private Function getRev(ByVal strStartDate As String) As String
        Dim rtnValue As String                                                      '戻り値
        Dim dbAccess As New CLAccessMdb                                             'DBアクセス
        Dim sql As String                                                           'SQL分
        Dim dt As DataTable                                                         'ＳＱＬの実行結果データセット
        Dim dtRow As DataRow                                                        '一行のデータ
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        rtnValue = ""
        Try
            sql = "Select s_print_rev,s_print_up,s_up from schedule where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + MDLoginInfo.PeriodId + "' and d_month='" + Format(Date.Parse(strStartDate), "yyyyMM") + "'"
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            dbAccess.Disconnect()
            If dt.Rows.Count > 0 Then
                dtRow = dt.Rows(0)
                rtnValue = CStr(dtRow("s_print_rev"))
                If CStr(dtRow("s_print_up")) <> CStr(dtRow("s_up")) Then
                    rtnValue = CStr(CInt(rtnValue) + 1)
                End If
            Else
                rtnValue = "1"
            End If
            Return rtnValue
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "getRev")
            log.Fatal(ex.Message)
        End Try
        '戻り値
        Return rtnValue
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Function

    '************************************************************************************
    '   ＩＤ　：UpdateRev
    '   名称　：REVを取得
    '   概要　：REVをアップデート
    '   引数　：ByVal strStartDate As String = 開始日
    '       　：ByVal strEndDate   As String = 終了日
    '       　：ByVal bFlg         As Boolean = True：登録＆印刷, False：登録のみ
    '   作成日：2011/11/22(火) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火) Ryu  新規作成
    '************************************************************************************
    Private Sub UpdateRev(ByVal strStartDate As String, ByVal strEndDate As String, ByVal bFlg As Boolean)
        Dim dbAccess As New CLAccessMdb                                             'DBアクセス
        Dim sql As String                                                           'SQL分
        Dim dt As DataTable                                                         'ＳＱＬの実行結果データセット
        Dim dtRow As DataRow                                                        '一行のデータ
        Dim strStartYM As String                                                    '開始年月
        Dim strEndYM As String                                                      '終了年月
        Dim strPrintRev As String                                                   'PrintRev
        Dim strPrintUp As String                                                    'PrintUp
        Dim strSUp As String                                                        'S_UP
        Dim intRtn As Integer                                                       'SQL文実行結果
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strStartYM = Mid(strStartDate, 1, 4) + Mid(strStartDate, 6, 2)
            strEndYM = Mid(strEndDate, 1, 4) + Mid(strEndDate, 6, 2)
            dbAccess.Connect()
            Do While strStartYM <= strEndYM
                sql = "Select s_print_rev,s_print_up,s_up from schedule where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + MDLoginInfo.PeriodId + "' and d_month='" + strStartYM + "'"
                dt = dbAccess.ExecuteSql(sql)
                If dt.Rows.Count > 0 Then
                    '該当月レコードがある場合
                    dtRow = dt.Rows(0)
                    strPrintRev = dtRow("s_print_rev")
                    strPrintUp = dtRow("s_print_up")
                    strSUp = dtRow("s_up")
                    If bFlg Then
                        '登録＆印刷の場合
                        sql = "update schedule set s_print_rev='" + CStr(CInt(strPrintRev) + 1) + "',"
                        sql = sql + "s_print_up='" + CStr(CInt(strPrintUp) + 1) + "',"
                    Else
                        '登録のみの場合
                        sql = "update schedule set "
                    End If
                    sql = sql + "s_up='" + CStr(CInt(strSUp) + 1) + "',"
                    sql = sql + "d_up='" + CStr(Now) + "',"
                    sql = sql + "c_user_id_up='" + MDLoginInfo.UserId + "' "
                    sql = sql + " where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + MDLoginInfo.PeriodId + "' and d_month='" + strStartYM + "'"
                Else
                    '該当月のレコードがない場合
                    strPrintRev = "0"
                    strPrintUp = "0"
                    strSUp = "0"
                    If bFlg Then
                        '登録＆印刷の場合
                        sql = "insert into schedule(c_ksh,c_period_id,d_month,s_print_rev,s_print_up,d_up,c_user_id_up,s_up) Values('"
                        sql = sql + MDLoginInfo.Ksh + "','"
                        sql = sql + MDLoginInfo.PeriodId + "','"
                        sql = sql + strStartYM + "','"
                        sql = sql + "1','"
                        sql = sql + "1','"
                        sql = sql + Now + "','"
                        sql = sql + MDLoginInfo.UserId + "','"
                        sql = sql + "1')"
                    Else
                        '登録のみの場合
                        sql = "insert into schedule(c_ksh,c_period_id,d_month,s_print_rev,s_print_up,d_up,c_user_id_up,s_up) Values('"
                        sql = sql + MDLoginInfo.Ksh + "','"
                        sql = sql + MDLoginInfo.PeriodId + "','"
                        sql = sql + strStartYM + "','"
                        sql = sql + strPrintRev + "','"
                        sql = sql + strPrintUp + "','"
                        sql = sql + Now + "','"
                        sql = sql + MDLoginInfo.UserId + "','"
                        sql = sql + "1')"
                    End If
                End If
                dbAccess.BeginTran()
                intRtn = dbAccess.ExecuteNonQuery(sql)
                If intRtn = 1 Then
                    dbAccess.CommitTran()
                    log.Info(String.Format("{0}件のデータを更新しました。", CStr(intRtn)))
                Else
                    dbAccess.RollbackTran()
                    log.Error("DB更新処理に異常があったためデータ更新を中止しました。")

                End If
                '次の月
                strStartDate = Format(DateAdd(DateInterval.Month, 1, Date.Parse(strStartDate)), DATE_YYYYMMDD_FORMAT)
                strStartYM = Mid(strStartDate, 1, 4) + Mid(strStartDate, 6, 2)
            Loop
            dbAccess.CommitTran()
            dbAccess.Disconnect()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "UpdateRev")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：UpdateRev
    '   名称　：REVを取得
    '   概要　：REVをアップデート
    '   引数　：strStartDate        開始日
    '       　：strEndDate          終了日
    '       　：bFlg                登録＆印刷か登録のみか
    '               true            登録＆印刷
    '               false           登録のみ
    '   作成日：2011/11/22(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火) Ryu  新規作成
    '************************************************************************************
    Private Sub changeGridDataBySEQ(ByVal strSEQ As String, ByVal strCellData As String)
        Dim pn As Panel                                                                 'メインパネル
        Dim uc As Control                                                               '遷移先コントロール
        Dim cfg As C1FlexGrid                                                           '日程表FLEXGRID
        Dim gb1 As GroupBox                                                             '分類グループボックス
        Dim iRowCounter As Integer                                                      '行数
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020201)
            gb1 = uc.Controls("fraSchedule")
            cfg = gb1.Controls("cfgScheduleList")
            For iRowCounter = 0 To cfg.Rows.Count - 1
                If CStr(cfg.GetData(iRowCounter, 2)) = strSEQ Then
                    cfg.SetData(iRowCounter, 3, strCellData, False)
                    Exit For
                ElseIf CStr(cfg.GetData(iRowCounter, 4)) = strSEQ Then
                    cfg.SetData(iRowCounter, 5, strCellData, False)
                    Exit For
                ElseIf CStr(cfg.GetData(iRowCounter, 6)) = strSEQ Then
                    cfg.SetData(iRowCounter, 7, strCellData, False)
                    Exit For
                End If
            Next
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020203, SCREEN_NAME_UC020203, "changeGridDataBySEQ")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

End Class
#End Region
