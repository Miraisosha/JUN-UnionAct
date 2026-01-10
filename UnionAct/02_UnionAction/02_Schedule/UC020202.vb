#Region "UC020202"
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDCommon
Imports C1.Win.C1FlexGrid

Public Class UC020202
#Region "変数宣言"
    Private Const SCHEDULE_HOUR_00_FORMAT As String = "00"                          '時間を２桁に整形するためのフォーマット
    Private Const SCHEDULE_SEQ_START As String = "0001"                             '年月に最初の日程の場合の開始シーケンス
    Private Const SCHEDULE_TIMEREQUERE_TEXT As String = "__時間__分"                'テキスト：__時間__分
    Public bln As Boolean                                                           '入力チェック結果
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region
#Region "戻るボタン"
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
        Dim pn As Panel                                 'メインパネル
        Dim uc As Control                               '表示するコントロール
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If CLMsg.Show("GQ0007") = DialogResult.Yes Then
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
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020202, SCREEN_NAME_UC020202, "btnBack_Click")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
#Region "終了時間を離れたとき"
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
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020202, SCREEN_NAME_UC020202, "dtpEnd_LostFocus")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
#Region "開始時刻を離れたとき"
    Private Sub dtpStart_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpStart.LostFocus
        dtpEnd.Focus()
    End Sub
#End Region
#Region "確認登録ボタン"
    '************************************************************************************
    '   ＩＤ　：btnConfirm_Click
    '   名称　：確認登録ボタン
    '   概要　：入力チェック
    '           画面遷移
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim errMsg As New ArrayList                                                 'エラーメッセージ配列
        Dim strDate As String                                                       '開始日
        Dim dbAccess As New CLAccessMdb                                             'DBアクセス
        Dim sql As String                                                           'SQL分
        Dim d_from As String                                                        '期の開始日
        Dim d_to As String                                                          '期の終了日
        Dim dt As DataTable                                                         'ＳＱＬの実行結果データセット
        Dim dtRow As DataRow                                                        '一行のデータ
        Dim fmPrint As FM000203                                                     '印刷プレビュー画面
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            Cursor.Current = Cursors.WaitCursor
            '入力チェック
            If Me.cmbScheduleKind.SelectedIndex = -1 Then
                errMsg.Add(CLMsg.GetMsg("GE0006", "日程表分類"))
                Me.cmbScheduleKind.BackColor = Color.Pink
            Else
                Me.cmbScheduleKind.BackColor = Color.White
            End If
            If Me.dtpDate.Text = "" Then
                errMsg.Add(CLMsg.GetMsg("GE0006", "開催日"))
                Me.dtpDate.BackColor = Color.Pink
            Else
                Me.dtpDate.BackColor = Color.White
            End If
            If Me.cmbCommitteName.Text = "" Then
                errMsg.Add(CLMsg.GetMsg("GE0006", "委員会名"))
                Me.cmbCommitteName.BackColor = Color.Pink
            End If
            If ChkLengthB(Me.cmbCommitteName.Text, 100) = False Then
                errMsg.Add(CLMsg.GetMsg("GE0112", "項目『委員会名』に", "100", "50"))
                Me.cmbCommitteName.BackColor = Color.Pink
            End If
            If Me.cmbCommitteName.Text <> "" And Me.cmbCommitteName.Text.Length <= 50 Then
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
            If Me.txtScheduleName.Text <> "" And Me.txtScheduleName.TextLength <= 100 Then
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
                errMsg.Add(CLMsg.GetMsg("GE0112", "項目『備考』に", "200", "100"))
                Me.txtBikou.BackColor = Color.Pink
            Else
                Me.txtBikou.BackColor = Color.White
            End If
            '開催日は期の期間以外の選択チェック
            strDate = Format(Me.dtpDate.Value.Date, MDConst.DATE_YYYYMMDD_FORMAT)
            dbAccess.Connect()
            sql = "Select d_from,d_to from period where c_period_id='" + Me.lblPeriodValue.Text + "'"
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                dtRow = dt.Rows(0)
                If InStr(dtRow("d_from"), "/") > 0 Then
                    d_from = Format(Date.Parse(dtRow("d_from")), MDConst.DATE_YYYYMMDD_FORMAT)
                Else
                    d_from = Format(Date.Parse(Mid(dtRow("d_from"), 1, 4) + "/" + Mid(dtRow("d_from"), 5, 2) + "/" + Mid(dtRow("d_from"), 7, 2)), MDConst.DATE_YYYYMMDD_FORMAT)
                End If
                If InStr(dtRow("d_to"), "/") > 0 Then
                    d_to = Format(Date.Parse(dtRow("d_to")), MDConst.DATE_YYYYMMDD_FORMAT)
                Else
                    d_to = Format(Date.Parse(Mid(dtRow("d_to"), 1, 4) + "/" + Mid(dtRow("d_to"), 5, 2) + "/" + Mid(dtRow("d_to"), 7, 2)), MDConst.DATE_YYYYMMDD_FORMAT)
                End If

                If strDate > d_to Or strDate < d_from Then
                    errMsg.Add("選択した開催日は期の開始と終了期間外のため、登録できません。")
                End If
            Else
                errMsg.Add("「期」の開始と終了期間が取得できませんでした。システム管理者へ連絡してください。")
            End If
            dbAccess.Disconnect()

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
                Call createPrePrint(fmPrint, strDate.Substring(8, 2), Me.cmbScheduleKind.Text, Me.txtScheduleName.Text)
                Select Case fmPrint.IntQlickBtnFlag
                    Case 0
                        '登録＆印刷
                        fmPrint.PrintOut()
                        '同期処理による最新データの取得
                        ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                        '日程をテーブルに挿入
                        insertSchedule("1")
                        iniUC020202()
                        '同期処理による最新データの反映
                        ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    Case 1
                        '登録のみ
                        '同期処理による最新データの取得
                        ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                        '日程をテーブルに挿入
                        insertSchedule("0")
                        iniUC020202()
                        '同期処理による最新データの反映
                        ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    Case 2
                        'キャンセル
                End Select

            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020202, SCREEN_NAME_UC020202, "btnConfirm_Click")
            log.Fatal(ex.Message)
        Finally
            Cursor.Current = Cursors.Default
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
#Region "委員会名入力を離れたとき"
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
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020202, SCREEN_NAME_UC020202, "cmbCommitteName_LostFocus")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
#Region "印刷プレビュー画面生成"
    '************************************************************************************
    '   ＩＤ　：createPrePrint
    '   名称　：印刷プレビュー画面表示
    '   概要　：新規日程表追加時のプレビュー画面表示
    '   引数　：fmPrint     プレビュー印刷画面
    '         ：strDay      新規日程の日
    '         ：strKind     日程表種類  
    '       　：strName     日程表表示名称
    '   作成日：2011/11/21(月) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) Ryu  新規作成
    '************************************************************************************
    Private Sub createPrePrint(ByVal fmPrint As FM000203, ByVal strDay As String, ByVal strKind As String, ByVal strName As String)

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
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '日程表画面を取得
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020201)
            'グリッドのデータを最新にするため、検索ボタンを呼び出す
            Dim uc0201 As UC020201 = uc
            uc0201.btnSearch_Click(uc0201.btnSearch, Nothing)

            gb1 = uc.Controls("fraSchedule")
            gb2 = uc.Controls("fraSearchOption")
            cfg = gb1.Controls("cfgScheduleList")

            resourceObj = New CR0202P1
            '印刷プレイビュー
            'fmPrint = New FM000203
            ds = New DS0202P1
            fmPrint.ButtonShowType = 1
            fmPrint.PrintCntVisible = True
            fmPrint.ObjResource = resourceObj

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
                    If cfg.GetData(iCounter - 1, 0) = strDay Then
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
            If cfg.GetData(iCounter - 1, 0) = strDay Then
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
            drHeader.d_rev = getRev()
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)

            resourceObj.SetDataSource(ds)

            Call fmPrint.ShowDialog()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020202, SCREEN_NAME_UC020202, "createPrePrint")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
#Region "Revを取得"
    '************************************************************************************
    '   ＩＤ　：getRev
    '   名称　：REVを取得
    '   概要　：印刷REVを取得
    '   作成日：2011/11/21(月) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) Ryu  新規作成
    '************************************************************************************
    Private Function getRev() As String
        Dim rtnValue As String                                                      '戻り値
        Dim dbAccess As New CLAccessMdb                                             'DBアクセス
        Dim sql As String                                                           'SQL分
        Dim dt As DataTable                                                         'ＳＱＬの実行結果データセット
        Dim dtRow As DataRow                                                        '一行のデータ
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        rtnValue = ""
        Try
            sql = "Select s_print_rev,s_print_up,s_up from schedule where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + MDLoginInfo.PeriodId + "' and d_month='" + Format(Me.dtpDate.Value.Date, "yyyyMM") + "'"
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
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020202, SCREEN_NAME_UC020202, "getRev")
            log.Fatal(ex.Message)
        End Try
        '戻り値
        Return rtnValue
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Function
#End Region
#Region "日程表新規追加"
    '************************************************************************************
    '   ＩＤ　：insertSchedule
    '   名称　：日程表新規追加
    '   概要　：日程表をテーブルに挿入
    '   作成日：2011/11/21(月) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) Ryu  新規作成
    '************************************************************************************
    Private Sub insertSchedule(ByVal strNum As String)
        Dim strDate As String                                                       '開始日
        Dim dbAccess As New CLAccessMdb                                             'DBアクセス
        Dim sql As String                                                           'SQL分
        Dim dt As DataTable                                                         'ＳＱＬの実行結果データセット
        Dim dtRow As DataRow                                                        '一行のデータ
        Dim strc_ksh As String                                                      '会社コード
        Dim strSEQ As String                                                        '日程表枝番
        Dim strPrintRev As String                                                   'PrintRev
        Dim strPrintUp As String                                                    'PrintUp
        Dim strSUp As String                                                        'S_UP
        Dim intRnt As Integer                                                       'SQL文実行結果
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strc_ksh = MDLoginInfo.Ksh
            strDate = Format(Me.dtpDate.Value.Date, MDConst.DATE_YYYYMMDD_FORMAT)
            '日程枝番取得

            sql = "select s_schedule_seq from schedule_dtl_list where d_month='" + strDate.Substring(0, 4) + strDate.Substring(5, 2) + "' Order by s_schedule_seq DESC"
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                dtRow = dt.Rows(0)
                strSEQ = dtRow("s_schedule_seq")
                strSEQ = CStr(CLng(strSEQ) + 1)
                'その年月のレコードをアップデート
                sql = "Select s_print_rev,s_print_up,s_up from schedule where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + MDLoginInfo.PeriodId + "' and d_month='" + strDate.Substring(0, 4) + strDate.Substring(5, 2) + "'"
                dt = dbAccess.ExecuteSql(sql)
                If dt.Rows.Count > 0 Then
                    dtRow = dt.Rows(0)
                    strPrintRev = dtRow("s_print_rev")
                    strPrintUp = dtRow("s_print_up")
                    strSUp = dtRow("s_up")
                    If strNum = "1" Then
                        sql = "update schedule set s_print_rev='" + CStr(CInt(strPrintRev) + 1) + "',"
                        sql = sql + "s_print_up='" + CStr(CInt(strPrintUp) + 1) + "',"
                    Else
                        sql = "update schedule set "
                    End If
                    sql = sql + "s_up='" + CStr(CInt(strSUp) + 1) + "',"
                    sql = sql + "d_up='" + Now + "',"
                    sql = sql + "c_user_id_up='" + MDLoginInfo.UserId + "' "
                    sql = sql + " where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + MDLoginInfo.PeriodId + "' and d_month='" + strDate.Substring(0, 4) + strDate.Substring(5, 2) + "'"
                    dbAccess.BeginTran()
                    intRnt = dbAccess.ExecuteNonQuery(sql)
                    dbAccess.CommitTran()
                    log.Info(String.Format("{0}件のデータを更新しました。", CStr(intRnt)))
                Else

                End If
            Else
                'その日に最初の日程の場合、枝番に年月＋0001を設定
                strSEQ = strDate.Substring(0, 4) + strDate.Substring(5, 2) + SCHEDULE_SEQ_START
                'その年月のレコードをscheduleテーブルに挿入
                sql = "insert into schedule(c_ksh,c_period_id,d_month,s_print_rev,s_print_up,d_up,c_user_id_up,s_up) Values('"
                sql = sql + MDLoginInfo.Ksh + "','"
                sql = sql + MDLoginInfo.PeriodId + "','"
                sql = sql + strDate.Substring(0, 4) + strDate.Substring(5, 2) + "','"
                sql = sql + strNum + "','"
                sql = sql + strNum + "','"
                sql = sql + Now + "','"
                sql = sql + MDLoginInfo.UserId + "','"
                sql = sql + "1')"
                dbAccess.BeginTran()
                intRnt = dbAccess.ExecuteNonQuery(sql)
                dbAccess.CommitTran()
                log.Info(String.Format("{0}件のデータを追加しました。", CStr(intRnt)))
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
            sql = sql + Me.cmbScheduleKind.Text + "','"
            '合同区分
            sql = sql + Me.cmbBranch.SelectedValue + "','"
            '会議名
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
            sql = sql + Now + "','"
            '作成者
            sql = sql + MDLoginInfo.UserId + "','"
            '更新日
            sql = sql + Now + "','"
            '更新者
            sql = sql + MDLoginInfo.UserId + "','"
            's_upを1に設定
            sql = sql + "1')"
            dbAccess.BeginTran()
            intRnt = dbAccess.ExecuteNonQuery(sql)
            dbAccess.CommitTran()
            log.Info(String.Format("{0}件のデータを追加しました。", CStr(intRnt)))
            dbAccess.Disconnect()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020202, SCREEN_NAME_UC020202, "insertSchedule")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
#Region "画面初期化"
    '************************************************************************************
    '   ＩＤ　：insertSchedule
    '   名称　：日程表新規追加
    '   概要　：日程表をテーブルに挿入
    '   作成日：2011/11/21(月) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) Ryu  新規作成
    '************************************************************************************
    Private Sub iniUC020202()
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            cmbCommitteName.SelectedIndex = -1
            bln = False
            txtScheduleName.Text = ""
            txtLocation.Text = ""
            txtMokuteki.Text = ""
            txtKatai.Text = ""
            txtBikou.Text = ""
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020202, SCREEN_NAME_UC020202, "iniUC020202")
            log.Fatal(ex.Message)
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
End Class
#End Region