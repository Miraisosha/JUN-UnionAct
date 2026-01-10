#Region "UC020201"
Imports UnionAct.NSCLAccessMdb
Imports C1.Win.C1FlexGrid
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDFile
Imports UnionAct.GUI.Document

Public Class UC020201

#Region "定数・変数"
    Private Const SCHEDULE_TITLE_DETAIL As String = "日程表詳細"                        '日程表詳細画面タイトル
    Private Const SCHEDULE_TITLE_REGIST As String = "日程表詳細 - 新規登録"             '日程表新規登録画面タイトル
    Private Const SCHEDULE_KI_NAME As String = "期"                                     '名称：期
    Private Const SCHEDULE_KI_KIKAN_NAME As String = "期の期間"                         '名称：期の期間
    Private Const SCHEDULE_SHIBU_NAME As String = "支部"                                '名称：支部
    Private Const SCHEDULE_COMMITTE_NAME As String = "委員会名"                         '名称：委員会名
    Private Const SCHEDULE_YEAR_TEXT As String = " 年 "                                 '名称：年
    Private Const SCHEDULE_MONTH_TEXT As String = " 月 "                                '名称：月
    Private Const SCHEDULE_DAY_TEXT As String = " 日 "                                  '名称：日
    Private Const SCHEDULE_SEARCH_YEAR As String = "対象年"                             '名称：対象年
    Private Const SCHEDULE_SEARCH_MONTH As String = "対象月"                            '名称：対象月
    Private Const SCHEDULE_DATETIMEPICKER_CUSTOMFORMAT_TIME As String = "HH時mm分"      'DATETIMEPICKERの時間選択フォーマット
    Private Const SCHEDULE_FLEXGRID_COLUMN12_WIDTH As Integer = 40                      '日程表FLEXGRIDの日列、曜日列の幅
    Private Const SCHEDULE_FLEXGRID_COLUMN345_WIDTH As Integer = 262                    '日程表FLEXGRIDのその他３列の幅
    Private Const SCHEDULE_DATE_YYYY_FORMAT As String = "yyyy"
    Private Const SCHEDULE_FILEOUTPUT_TITLE As String = "ファイルの保存先を選択してください" 'ファイル保存ダイアログタイトル
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private strPreYear As String                                                        '現在選択した年
    Private strPreMonth As String                                                       '現在選択した月
    Private strRead As String = ""                                                      'ログイン権限で参照可能か
    Private strReg As String = ""                                                       'ログイン権限で登録可能か
    Private strPrint As String = ""                                                     'ログイン権限で印刷可能か
    Private strFile As String = ""                                                      'ログイン権限でファイル出力可能か
#End Region

#Region "イベント"
    '************************************************************************************
    '   ＩＤ　：UC020201_Load
    '   名称　：画面初期表示処理
    '   概要　：画面初期表示を処理する。
    '            「期」コンボボックスのリスト値をデータベースより取得しセットする
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub UC020201_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim cmbPeriod As ComboBox                   '「期」コンボボックス
        Dim dbAccess As New CLAccessMdb             '共通ＤＢアクセスクラス
        Dim dt As DataTable                         'ＳＱＬの実行結果データセット
        Dim sql As String = Nothing                 'ＳＱＬ文
        Dim c_ksh As String                         '会社コード、グローバル変数を使用する予定で、暫定的に定義
        Dim cfg As C1FlexGrid                       'データFlexGrid
        Dim currentDate As Date                     '当日日付
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '「期」コンボボックスの初期化
            cmbPeriod = Me.cmbSearchPeriod
            cmbPeriod.BeginUpdate()
            cmbPeriod.DataSource = Nothing
            cmbPeriod.Items.Clear()
            'ログイン会社コード
            c_ksh = MDLoginInfo.Ksh
            ' SQL作成
            sql = "select c_period_id,l_name from period where c_ksh='" + c_ksh + "'"

            ' データベースに接続
            dbAccess.Connect()
            ' データを取得
            dt = dbAccess.ExecuteSql(sql)
            ' データソース設定
            cmbPeriod.DataSource = dt
            ' コンボボックス名称設定
            cmbPeriod.DisplayMember = "l_name"
            ' コンボボックス値設定
            cmbPeriod.ValueMember = "c_period_id"
            ' データベースの接続を切段
            dbAccess.Disconnect()

            ' データベースのテーブルに「期」のデータがなかった場合、エラー。以下は暫定
            If cmbPeriod.Items.Count = 0 Then
                CLMsg.Show("GE0004", SCHEDULE_KI_NAME)
                Exit Sub
            Else
                cmbPeriod.SelectedValue = MDLoginInfo.PeriodId
            End If
            cmbPeriod.EndUpdate()
            '対象年選択肢を期の範囲内に設定
            Call setCmbYear()
            Me.lblKiValue.Text = cmbPeriod.SelectedValue
            '対象年デフォルト選択
            currentDate = Now
            Me.cmbSearchYear.SelectedIndex = cmbSearchYear.FindString(CStr(currentDate.Year))
            '対象月デフォルト選択
            'Me.cmbSearchMonth.SelectedIndex = 0
            Me.cmbSearchMonth.SelectedIndex = cmbSearchMonth.FindString(CStr(currentDate.Month).PadLeft(2, "0"c))
            'GlexGridの列のサイズを調整する
            cfg = Me.cfgScheduleList
            cfg.HighLight = HighLightEnum.Always
            cfg.Cols(0).Width = SCHEDULE_FLEXGRID_COLUMN12_WIDTH
            cfg.Cols(1).Width = SCHEDULE_FLEXGRID_COLUMN12_WIDTH
            cfg.Cols(3).Width = SCHEDULE_FLEXGRID_COLUMN345_WIDTH
            cfg.Cols(3).Caption = SCHEDULE_BUNRUI_INDEX_0
            cfg.Cols(5).Width = SCHEDULE_FLEXGRID_COLUMN345_WIDTH
            cfg.Cols(5).Caption = SCHEDULE_BUNRUI_INDEX_1
            cfg.Cols(7).Width = SCHEDULE_FLEXGRID_COLUMN345_WIDTH
            cfg.Cols(7).Caption = SCHEDULE_BUNRUI_INDEX_2
            cfg.AllowMerging = AllowMergingEnum.Free
            cfg.Cols(0).AllowMerging = True
            cfg.Cols(1).AllowMerging = True
            cfg.Cols(2).AllowMerging = False
            cfg.Cols(3).AllowMerging = False
            cfg.Cols(4).AllowMerging = False
            cfg.Cols(5).AllowMerging = False
            cfg.Cols(6).AllowMerging = False
            cfg.Cols(7).AllowMerging = False
            cfg.ColSel = 3
            cfg.RowSel = 1
            strPreYear = cmbSearchYear.Text
            strPreMonth = cmbSearchMonth.Text
            Call DefinitionCellStyles()
            'ログイン権限により、ボタン操作できるかを制御
            Dim dtKengen As DataTable = Nothing
            dtKengen = MDCommon.getGrant(MENU_ID_UC020201)
            If dtKengen.Rows.Count > 0 Then
                strRead = dtKengen.Rows(0).Item(3).ToString
                strReg = dtKengen.Rows(0).Item(4).ToString
                strPrint = dtKengen.Rows(0).Item(5).ToString
                strFile = dtKengen.Rows(0).Item(6).ToString
                btnSearch.Enabled = CInt(strRead)
                btnRenzokuRegist.Enabled = CInt(strReg)
                btnRangeRegist.Enabled = CInt(strReg)
                btnPrePrint.Enabled = CInt(strPrint)
                btnFileOutput.Enabled = CInt(strFile)
                btnScheduleDetail.Enabled = CInt(strRead)
                btnSchedulePrePrint.Enabled = CInt(strRead)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "UC020201_Load")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnScheduleDetail_Click
    '   名称　：詳細ボタン
    '   概要　：日程表詳細画面へ遷移
    '   作成日：2011/11/04(木) Ryu
    '   更新日：2016/10/05(水) Fujisaku  
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '       　：2016/10/05(水) Fujisaku  日付の引数追加
    '************************************************************************************
    Private Sub btnScheduleDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnScheduleDetail.Click
        Dim pn As Panel                                             'メインパネル
        Dim uc As Control                                           '遷移先コントロール
        Dim cfg As C1FlexGrid                                       '日程表
        Dim selDay As String                                        '選択した日程の日付2桁
        Dim selSEQ As String                                        '選択した日程のシーケンス番号   
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '日程表を選択しているかチェック
            cfg = Me.cfgScheduleList
            selDay = CStr(cfg.GetData(cfg.RowSel, 0))
            selSEQ = CStr(cfg.GetData(cfg.RowSel, cfg.ColSel - 1))
            If selSEQ = "" Then
                CLMsg.Show("GI0007")
                Exit Sub
            End If
            '日程表詳細画面を表示
            Me.Visible = False
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020203)
            If uc Is Nothing Then
                uc = New UC020203
                Call pn.Controls.Add(uc)
                uc.Visible = False
            End If
            '該当日程表の内容を画面に表示
            If Not IniUC020203(uc, selDay, selSEQ) Then
                CLMsg.Show("GE0001")
                Exit Sub
            End If

            uc.Visible = True

            '画面項目は編集不可にする
            Call EnabledUC020203(uc, False, Color.White)

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "btnScheduleDetail_Click")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnRenzokuRegist_Click
    '   名称　：連続登録ボタン
    '   概要　：日程登録画面へ遷移
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnRenzokuRegist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRenzokuRegist.Click

        Dim pn As Panel                                             'メインパネル
        Dim uc As Control                                           '遷移先コントロール
        Dim strKiValue As String                                    '本画面で選択した期のコード
        Dim strKiText As String                                     '本画面で選択した期の値
        Dim strYear As String                                       '本画面で選択した年
        Dim strMonth As String                                      '本画面で選択した月
        Dim strYYMM As String                                       '本画面で選択した年月
        Dim strDYYMM As String = "202007"                           '表示切替年月 2020年07月
        Dim bln As Boolean                                          'エラーチェック結果
        Dim cmbShibu As ComboBox                                    '「支部」コンボボックス
        Dim cmbCommitte As ComboBox                                 '「委員会名」コンボボックス
        Dim strPeriodID As String                                   '期コードを暫定的に定義
        Dim timePickerStart As DateTimePicker                       '開始時間
        Dim timePickerEnd As DateTimePicker                         '終了時間
        Dim lblPeriodValue As Label                                 '期コードラベル
        Dim lblPeriodText As Label                                  '期表示文字ラベル
        Dim dtpDate As DateTimePicker                               '開催日
        Dim fraScheduleDetail As GroupBox                           '分類フレーム
        Dim fraScheduleKind As GroupBox                             '詳細フレーム
        Dim cfg As C1FlexGrid                                       '日程GRID
        Dim cmbKind As ComboBox                                     '日程表分類
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '期と対象年月が選択されたかをチェック
            bln = checkSelected()
            If Not bln Then
                '入力チェックエラー時
                Exit Sub
            End If

            strKiValue = Me.cmbSearchPeriod.SelectedValue
            strKiText = Me.cmbSearchPeriod.Text
            strYear = Me.cmbSearchYear.Text
            strMonth = Me.cmbSearchMonth.Text
            Me.Visible = False
            '遷移先画面を取得
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020202)
            If uc Is Nothing Then
                uc = New UC020202
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If
            'フォーム初期化
            strPeriodID = strKiValue
            fraScheduleDetail = uc.Controls("fraScheduleDetail")
            fraScheduleKind = uc.Controls("fraScheduleKind")
            cmbShibu = fraScheduleDetail.Controls("cmbBranch")

            'コンボボックス初期化
            setComboboxValueFromDB(uc, strPeriodID)

            ' データベースのテーブルに「支部」のデータがなかった場合
            If cmbShibu.Items.Count = 0 Then
                CLMsg.Show("GE0004", SCHEDULE_SHIBU_NAME)
                Exit Sub
            Else
                cmbShibu.SelectedIndex = 0
            End If

            '「委員会名」コンボボックスの初期化
            cmbCommitte = fraScheduleDetail.Controls("cmbCommitteName")

            ' データベースのテーブルに「委員会名」のデータがなかった場合
            If cmbCommitte.Items.Count = 0 Then
                CLMsg.Show("GE0004", SCHEDULE_COMMITTE_NAME)
                Exit Sub
            Else
                cmbCommitte.SelectedIndex = 0
            End If

            timePickerStart = fraScheduleDetail.Controls("dtpStart")
            timePickerStart.CustomFormat = SCHEDULE_DATETIMEPICKER_CUSTOMFORMAT_TIME
            timePickerStart.Format = DateTimePickerFormat.Custom
            timePickerStart.ShowUpDown = True

            timePickerEnd = fraScheduleDetail.Controls("dtpEnd")
            timePickerEnd.CustomFormat = SCHEDULE_DATETIMEPICKER_CUSTOMFORMAT_TIME
            timePickerEnd.Format = DateTimePickerFormat.Custom
            timePickerEnd.ShowUpDown = True
            '「期」の表示値とコードを設定
            lblPeriodValue = fraScheduleKind.Controls("lblPeriodValue")
            lblPeriodValue.Text = strKiValue
            lblPeriodText = fraScheduleKind.Controls("lblPeriod")
            lblPeriodText.Text = strKiText
            dtpDate = fraScheduleKind.Controls("dtpDate")
            cfg = Me.cfgScheduleList
            If IsNumeric(cfg.GetData(cfg.RowSel, 0)) Then
                dtpDate.Value = New Date(CInt(strYear), CInt(strMonth), CInt(cfg.GetData(cfg.RowSel, 0)))
            Else
                dtpDate.Value = New Date(CInt(strYear), CInt(strMonth), CInt("01"))
            End If
            '日程表分類初期設定
            cmbKind = fraScheduleKind.Controls("cmbScheduleKind")
            cmbKind.Items.Clear()
            strYYMM = Me.cmbSearchYear.Text + Me.cmbSearchMonth.Text
            If strYYMM <= strDYYMM Then
                cmbKind.Items.Add(SCHEDULE_BUNRUI_INDEX_0)
                cmbKind.Items.Add(SCHEDULE_BUNRUI_INDEX_1)
                cmbKind.Items.Add(SCHEDULE_BUNRUI_INDEX_2)
            Else
                cmbKind.Items.Add(SCHEDULE_BUNRUI_INDEX_3)
                cmbKind.Items.Add(SCHEDULE_BUNRUI_INDEX_1)
                cmbKind.Items.Add(SCHEDULE_BUNRUI_INDEX_4)
            End If

            Select Case cfg.Col
                Case 3
                    cmbKind.SelectedIndex = 0
                Case 5
                    cmbKind.SelectedIndex = 1
                Case 7
                    cmbKind.SelectedIndex = 2
            End Select
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "btnRenzokuRegist_Click")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnRangeRegist_Click
    '   名称　：範囲登録ボタン
    '   概要　：日程表新規登録画面へ遷移
    '   作成日：2011/11/04(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnRangeRegist_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRangeRegist.Click

        Dim pn As Panel                             'メインパネル
        Dim uc As Control                           '遷移先コントロール
        Dim gb As GroupBox                          '分類グループボックス
        Dim lbStandard As Label                     'ラベル
        Dim strKiValue As String                    '期のコード
        Dim strKiText As String                     '期の値
        Dim strYear As String                       '選択した対象年
        Dim strMonth As String                      '選択した対象月
        Dim bln As Boolean                          '入力チェック結果
        Dim title As Label                          '画面タイトル
        Dim lbKara As Label                         '～ラベル
        Dim lbStartday As Label                     '開始日
        Dim dtp As DateTimePicker                   '終了日付コントロール
        Dim cmbShibu As ComboBox                    '「支部」コンボボックス
        Dim cmbCommitte As ComboBox                 '「委員会名」コンボボックス
        Dim strPeriodID As String                   '期コードを暫定的に定義
        Dim timePickerStart As DateTimePicker       '開始時間
        Dim timePickerEnd As DateTimePicker         '終了時間
        Dim lblPeriodValue As Label                 '期コードラベル
        Dim lblPeriodText As Label                  '期表示文字ラベル
        Dim dtpDate As DateTimePicker               '開催日
        Dim fraScheduleDetail As GroupBox           '分類フレーム
        Dim fraScheduleKind As GroupBox             '詳細フレーム
        Dim lblStartDate As Label                   '範囲登録の開始日付
        Dim strSelectedDay As String                '選択した日、暫定
        Dim lblKindText As Label                    '分類
        Dim cfg As C1FlexGrid                       '日程表のGRID
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '期と対象年月が選択されたかをチェック
            bln = checkSelected()
            If Not bln Then
                '入力チェックエラー時
                Exit Sub
            End If

            strKiValue = Me.cmbSearchPeriod.SelectedValue
            strKiText = Me.cmbSearchPeriod.Text
            strYear = Me.cmbSearchYear.Text
            strMonth = Me.cmbSearchMonth.Text

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020203)

            If uc Is Nothing Then
                uc = New UC020203
                title = uc.Controls("lblTitle")
                title.Text = SCHEDULE_TITLE_REGIST
                uc.Controls("btnDelete").Visible = False
                uc.Controls("btnChange").Text = KANJI_BUTTON_LABEL_TOUROKUKAKUNIN
                gb = uc.Controls("fraScheduleKind")
                lbStandard = gb.Controls("lblPeriod") '第XX期の基準位置となるラベル
                Call pn.Controls.Add(uc)
                lbStartday = gb.Controls("lblStartDate")
                lbKara = gb.Controls("lblKara")
                dtp = gb.Controls("dtpEndDate")

                lbStartday.Location = New System.Drawing.Point(lbStandard.Location.X, lbStandard.Location.Y + lbStandard.Size.Height + 5)
                lbKara.Location = New System.Drawing.Point(lbStartday.Location.X + lbStartday.Size.Width + 5, lbStartday.Location.Y)
                dtp.Location = New System.Drawing.Point(lbKara.Location.X + lbKara.Size.Width + 5, lbKara.Location.Y)
                lbKara.Visible = True
                dtp.Visible = True
            Else
                uc.Visible = True
            End If
            'フォーム初期化
            ' 期ＩＤ
            strPeriodID = strKiValue

            fraScheduleDetail = uc.Controls("fraScheduleDetail")
            fraScheduleKind = uc.Controls("fraScheduleKind")

            cmbShibu = fraScheduleDetail.Controls("cmbBranch")
            'コンボボックス初期化
            setComboboxValueFromDB(uc, strPeriodID)

            ' データベースのテーブルに「支部」のデータがなかった場合、エラー。以下は暫定
            If cmbShibu.Items.Count = 0 Then
                CLMsg.Show("GE0004", SCHEDULE_SHIBU_NAME)
                Exit Sub
            Else
                cmbShibu.SelectedIndex = 0
            End If

            '「委員会名」コンボボックスの初期化
            cmbCommitte = fraScheduleDetail.Controls("cmbCommitteName")

            ' データベースのテーブルに「委員会名」のデータがなかった場合、エラー。以下は暫定
            If cmbCommitte.Items.Count = 0 Then
                CLMsg.Show("GE0004", SCHEDULE_COMMITTE_NAME)
                Exit Sub
            Else
                cmbCommitte.SelectedIndex = 0
            End If

            timePickerStart = fraScheduleDetail.Controls("dtpStart")
            timePickerStart.CustomFormat = SCHEDULE_DATETIMEPICKER_CUSTOMFORMAT_TIME
            timePickerStart.Format = DateTimePickerFormat.Custom
            timePickerStart.ShowUpDown = True

            timePickerEnd = fraScheduleDetail.Controls("dtpEnd")
            timePickerEnd.CustomFormat = SCHEDULE_DATETIMEPICKER_CUSTOMFORMAT_TIME
            timePickerEnd.Format = DateTimePickerFormat.Custom
            timePickerEnd.ShowUpDown = True
            '「期」の表示値とコードを設定
            lblPeriodValue = fraScheduleKind.Controls("lblPeriodValue")
            lblPeriodValue.Text = strKiValue
            lblPeriodText = fraScheduleKind.Controls("lblPeriod")
            lblPeriodText.Text = strKiText
            lblStartDate = fraScheduleKind.Controls("lblStartDate")
            '一覧表で選択した日
            cfg = Me.cfgScheduleList
            strSelectedDay = CStr(cfg.GetData(cfg.RowSel, 0))
            lblStartDate.Text = strYear + SCHEDULE_YEAR_TEXT + strMonth + SCHEDULE_MONTH_TEXT + strSelectedDay + SCHEDULE_DAY_TEXT
            dtpDate = fraScheduleKind.Controls("dtpEndDate")
            dtpDate.Value = New Date(CInt(strYear), CInt(strMonth), CInt(strSelectedDay))
            lblKindText = fraScheduleKind.Controls("lblKindText")
            '一覧表で選択した日程表分類
            lblKindText.Text = CStr(cfg.GetData(0, cfg.ColSel))
            fraScheduleKind.Controls("lblSEQ").Text = ""
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "btnRangeRegist_Click")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索処理
    '   概要　：期と年月をキーに日程をDBから検索
    '   作成日：2011/11/09(木) Ryu
    '   更新日：2016/10/05(水) Fujisaku
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/09(木) Ryu  新規作成
    '       　：2016/10/05(水) Fujisaku  画面構築時のHashTable使用取り止め
    '************************************************************************************
    Public Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Dim bln As Boolean                                      '入力チェック用エラーリスト
        Dim strPeriodValue As String                            '期のコード
        Dim strYear As String                                   '検索対象年
        Dim strMonth As String                                  '検索対象月
        Dim strYYMM As String                                   '検索対象年月
        Dim strDYYMM As String = "202007"                       '検索対象2020年07月切り替えよう
        Dim sql As String                                       'SQL分
        Dim dbAccess As New CLAccessMdb                         'DBアクセス
        Dim dt As DataTable                                     'データテーブル
        Dim dtRow As DataRow                                    '一行のデータ
        Dim searchDate As Date                                  '日付
        Dim intDays As Integer                                  '選択した月の日数
        Dim intCounter As Integer                               '該当月の日
        Dim cfg As C1FlexGrid                                   '日程一覧表
        Dim day6CellStyle As CellStyle                          '土曜日の日のスタイル
        Dim yobi6CellStyle As CellStyle                         '土曜日の曜日のスタイル
        Dim day7CellStyle As CellStyle                          '日曜日の日のスタイル
        Dim yobi7CellStyle As CellStyle                         '日曜日の曜日のスタイル
        Dim strDay As String                                    '日
        Dim c_ksh As String                                     '会社コード、暫定
        Dim rowCounter As Integer                               '行数
        Dim preRowCounter As Integer                            '前日の最後の行の数
        Dim SEQ0() As String                                    '一日分の中執データ
        Dim INF0() As String                                    '一日分の中執データ
        Dim iCnt0 As Integer                                    '一日分の中執データ
        Dim SEQ1() As String                                    '一日分の専門部データ
        Dim INF1() As String                                    '一日分の専門部データ
        Dim iCnt1 As Integer                                    '一日分の専門部データ
        Dim SEQ2() As String                                    '一日分の産別データ
        Dim INF2() As String                                    '一日分の産別データ
        Dim iCnt2 As Integer                                    '一日分の産別データ
        Dim addRowNum As Integer                                '一日分データの最大の行数
        Dim iCounter As Integer                                 '一日分のレコードのカウンター
        Dim rowNumber As Integer                                '処理中の行数
        Dim lastRowNum As Integer                               '最後の行の数
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try

            '期と対象年月が選択されたかをチェック
            bln = checkSelected()
            If Not bln Then
                '入力チェックエラー時
                Me.fraSchedule.Visible = False
            Else
                '入力チェック通った場合、検索を行う
                FrmWaitInfo.ShowWaitForm(Nothing)
                strPeriodValue = Me.cmbSearchPeriod.SelectedValue
                strYear = Me.cmbSearchYear.Text
                strMonth = Me.cmbSearchMonth.Text
                '選択した年月の日に登録されている日程をDBより取得
                c_ksh = MDLoginInfo.Ksh

                intDays = Date.DaysInMonth(CInt(strYear), CInt(strMonth))
                cfg = Me.cfgScheduleList
                cfg.Rows.Count = 1
                cfg.Redraw = False
                day6CellStyle = cfg.Styles.Add("day6")
                day6CellStyle.ForeColor = Color.Blue
                yobi6CellStyle = cfg.Styles.Add("yobi6")
                yobi6CellStyle.BackColor = Color.LightBlue
                day7CellStyle = cfg.Styles.Add("day7")
                day7CellStyle.ForeColor = Color.Red
                yobi7CellStyle = cfg.Styles.Add("yobi7")
                yobi7CellStyle.BackColor = Color.Pink
                '初期化
                dbAccess.Connect()
                preRowCounter = 0
                strYYMM = strYear + strMonth
                If strYYMM <= strDYYMM Then '2020年7月以前の表示形式
                    cfg.Cols(3).Caption = SCHEDULE_BUNRUI_INDEX_0
                    cfg.Cols(7).Caption = SCHEDULE_BUNRUI_INDEX_2
                    For intCounter = 1 To intDays
                        searchDate = Date.Parse(strYear + "/" + strMonth + "/" + CStr(intCounter))
                        strDay = Format(searchDate, "dd")
                        sql = "Select k_schedule_divide,l_information_name,s_schedule_seq from schedule_dtl_list where c_ksh='" + c_ksh + "' and c_period_id='" + strPeriodValue + "' and d_month='" + strYear + strMonth + "' and d_date='" + strDay + "' order by s_schedule_seq"

                        dt = dbAccess.ExecuteSql(sql)

                        iCnt0 = 0
                        iCnt1 = 0
                        iCnt2 = 0
                        ReDim SEQ0(0)
                        ReDim SEQ1(0)
                        ReDim SEQ2(0)
                        ReDim INF0(0)
                        ReDim INF1(0)
                        ReDim INF2(0)
                        If dt.Rows.Count > 0 Then
                            'DBからのデータをListにセット
                            For rowCounter = 0 To dt.Rows.Count - 1
                                dtRow = dt.Rows(rowCounter)
                                Select Case dtRow(0)
                                    Case SCHEDULE_BUNRUI_INDEX_0
                                        ReDim Preserve SEQ0(iCnt0)
                                        ReDim Preserve INF0(iCnt0)
                                        SEQ0.SetValue(dtRow(2), iCnt0)
                                        INF0.SetValue(dtRow(1), iCnt0)
                                        iCnt0 = iCnt0 + 1
                                    Case SCHEDULE_BUNRUI_INDEX_1
                                        ReDim Preserve SEQ1(iCnt1)
                                        ReDim Preserve INF1(iCnt1)
                                        SEQ1.SetValue(dtRow(2), iCnt1)
                                        INF1.SetValue(dtRow(1), iCnt1)
                                        iCnt1 = iCnt1 + 1
                                    Case SCHEDULE_BUNRUI_INDEX_2
                                        ReDim Preserve SEQ2(iCnt2)
                                        ReDim Preserve INF2(iCnt2)
                                        SEQ2.SetValue(dtRow(2), iCnt2)
                                        INF2.SetValue(dtRow(1), iCnt2)
                                        iCnt2 = iCnt2 + 1
                                End Select
                            Next
                            '一日分データの最大レコード数を取得
                            If iCnt0 >= iCnt1 Then
                                addRowNum = iCnt0
                            Else
                                addRowNum = iCnt1
                            End If
                            If addRowNum < iCnt2 Then
                                addRowNum = iCnt2
                            End If
                            'FLEXGRIDの行を最大レコード数分追加
                            cfg.Rows.Add(addRowNum)

                            '中執データを日程表に表示
                            'その日の前日のデータの所在行数
                            rowNumber = preRowCounter
                            If iCnt0 > 0 Then
                                'Dim sl As New SortedList(SEQ0)
                                For Each itemKey As String In SEQ0
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 2, itemKey)
                                Next
                                rowNumber = preRowCounter
                                For Each itemValue As String In INF0
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 3, itemValue)
                                Next
                            End If

                            '専門部データを日程表に表示
                            'その日の前日のデータの所在行数
                            rowNumber = preRowCounter
                            If iCnt1 > 0 Then
                                'Dim sl As New SortedList(SEQ1)
                                For Each itemKey As String In SEQ1
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 4, itemKey)
                                Next
                                rowNumber = preRowCounter
                                For Each itemValue As String In INF1
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 5, itemValue)
                                Next
                            End If

                            '産別データを日程表に表示
                            'その日の前日のデータの所在行数
                            rowNumber = preRowCounter
                            If iCnt2 > 0 Then
                                'Dim sl As New SortedList(SEQ2)
                                For Each itemKey As String In SEQ2
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 6, itemKey)
                                Next
                                rowNumber = preRowCounter
                                For Each itemValue As String In INF2
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 7, itemValue)
                                Next
                            End If
                        Else
                            cfg.Rows.Add(1)
                            addRowNum = 1
                        End If
                        lastRowNum = preRowCounter + addRowNum
                        For iCounter = preRowCounter + 1 To lastRowNum
                            cfg.SetData(iCounter, 0, strDay, False)
                            cfg.SetCellStyle(iCounter, 0, cfg.Styles.SelectedRowHeader)
                            cfg.SetCellStyle(iCounter, 1, cfg.Styles.SelectedRowHeader)
                            Select Case Weekday(searchDate)
                                Case 1
                                    '日曜日
                                    cfg.SetCellStyle(iCounter, 0, day7CellStyle)
                                    cfg.SetCellStyle(iCounter, 1, yobi7CellStyle)
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_NICHI, False)
                                Case 2
                                    '月曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_GETSU, False)
                                Case 3
                                    '火曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_KA, False)
                                Case 4
                                    '水曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_SUI, False)
                                Case 5
                                    '木曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_MOKU, False)
                                Case 6
                                    '金曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_KIN, False)
                                Case 7
                                    '土曜日
                                    cfg.SetCellStyle(iCounter, 0, day6CellStyle)
                                    cfg.SetCellStyle(iCounter, 1, yobi6CellStyle)
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_DO, False)
                            End Select
                        Next

                        preRowCounter = lastRowNum
                    Next
                    cfg.AllowMerging = AllowMergingEnum.Free
                    cfg.Cols(0).AllowMerging = True
                    cfg.Cols(1).AllowMerging = True
                    cfg.Cols(2).AllowMerging = False
                    cfg.Cols(3).AllowMerging = False
                    cfg.Cols(4).AllowMerging = False
                    cfg.Cols(5).AllowMerging = False
                    cfg.Cols(6).AllowMerging = False
                    cfg.Cols(7).AllowMerging = False
                    dbAccess.Disconnect()
                    Call AddDaysToFixedColumn()
                    cfg.Redraw = True
                    Me.fraSchedule.Visible = True
                    FrmWaitInfo.CloseWaitForm()

                Else    '2020年8月以降の表示形式

                    cfg.Cols(3).Caption = SCHEDULE_BUNRUI_INDEX_3
                    cfg.Cols(7).Caption = SCHEDULE_BUNRUI_INDEX_4
                    For intCounter = 1 To intDays
                        searchDate = Date.Parse(strYear + "/" + strMonth + "/" + CStr(intCounter))
                        strDay = Format(searchDate, "dd")
                        sql = "Select k_schedule_divide,l_information_name,s_schedule_seq from schedule_dtl_list where c_ksh='" + c_ksh + "' and c_period_id='" + strPeriodValue + "' and d_month='" + strYear + strMonth + "' and d_date='" + strDay + "' order by s_schedule_seq"

                        dt = dbAccess.ExecuteSql(sql)

                        iCnt0 = 0
                        iCnt1 = 0
                        iCnt2 = 0
                        ReDim SEQ0(0)
                        ReDim SEQ1(0)
                        ReDim SEQ2(0)
                        ReDim INF0(0)
                        ReDim INF1(0)
                        ReDim INF2(0)
                        If dt.Rows.Count > 0 Then
                            'DBからのデータをListにセット
                            For rowCounter = 0 To dt.Rows.Count - 1
                                dtRow = dt.Rows(rowCounter)
                                Select Case dtRow(0)
                                    Case SCHEDULE_BUNRUI_INDEX_3
                                        ReDim Preserve SEQ0(iCnt0)
                                        ReDim Preserve INF0(iCnt0)
                                        SEQ0.SetValue(dtRow(2), iCnt0)
                                        INF0.SetValue(dtRow(1), iCnt0)
                                        iCnt0 = iCnt0 + 1
                                    Case SCHEDULE_BUNRUI_INDEX_1
                                        ReDim Preserve SEQ1(iCnt1)
                                        ReDim Preserve INF1(iCnt1)
                                        SEQ1.SetValue(dtRow(2), iCnt1)
                                        INF1.SetValue(dtRow(1), iCnt1)
                                        iCnt1 = iCnt1 + 1
                                    Case SCHEDULE_BUNRUI_INDEX_4
                                        ReDim Preserve SEQ2(iCnt2)
                                        ReDim Preserve INF2(iCnt2)
                                        SEQ2.SetValue(dtRow(2), iCnt2)
                                        INF2.SetValue(dtRow(1), iCnt2)
                                        iCnt2 = iCnt2 + 1
                                End Select
                            Next
                            '一日分データの最大レコード数を取得
                            If iCnt0 >= iCnt1 Then
                                addRowNum = iCnt0
                            Else
                                addRowNum = iCnt1
                            End If
                            If addRowNum < iCnt2 Then
                                addRowNum = iCnt2
                            End If
                            'FLEXGRIDの行を最大レコード数分追加
                            cfg.Rows.Add(addRowNum)

                            '中執データを日程表に表示
                            'その日の前日のデータの所在行数
                            rowNumber = preRowCounter
                            If iCnt0 > 0 Then
                                'Dim sl As New SortedList(SEQ0)
                                For Each itemKey As String In SEQ0
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 2, itemKey)
                                Next
                                rowNumber = preRowCounter
                                For Each itemValue As String In INF0
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 3, itemValue)
                                Next
                            End If

                            '専門部データを日程表に表示
                            'その日の前日のデータの所在行数
                            rowNumber = preRowCounter
                            If iCnt1 > 0 Then
                                'Dim sl As New SortedList(SEQ1)
                                For Each itemKey As String In SEQ1
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 4, itemKey)
                                Next
                                rowNumber = preRowCounter
                                For Each itemValue As String In INF1
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 5, itemValue)
                                Next
                            End If

                            '産別データを日程表に表示
                            'その日の前日のデータの所在行数
                            rowNumber = preRowCounter
                            If iCnt2 > 0 Then
                                'Dim sl As New SortedList(SEQ2)
                                For Each itemKey As String In SEQ2
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 6, itemKey)
                                Next
                                rowNumber = preRowCounter
                                For Each itemValue As String In INF2
                                    rowNumber = rowNumber + 1
                                    cfg.SetData(rowNumber, 7, itemValue)
                                Next
                            End If
                        Else
                            cfg.Rows.Add(1)
                            addRowNum = 1
                        End If
                        lastRowNum = preRowCounter + addRowNum
                        For iCounter = preRowCounter + 1 To lastRowNum
                            cfg.SetData(iCounter, 0, strDay, False)
                            cfg.SetCellStyle(iCounter, 0, cfg.Styles.SelectedRowHeader)
                            cfg.SetCellStyle(iCounter, 1, cfg.Styles.SelectedRowHeader)
                            Select Case Weekday(searchDate)
                                Case 1
                                    '日曜日
                                    cfg.SetCellStyle(iCounter, 0, day7CellStyle)
                                    cfg.SetCellStyle(iCounter, 1, yobi7CellStyle)
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_NICHI, False)
                                Case 2
                                    '月曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_GETSU, False)
                                Case 3
                                    '火曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_KA, False)
                                Case 4
                                    '水曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_SUI, False)
                                Case 5
                                    '木曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_MOKU, False)
                                Case 6
                                    '金曜日
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_KIN, False)
                                Case 7
                                    '土曜日
                                    cfg.SetCellStyle(iCounter, 0, day6CellStyle)
                                    cfg.SetCellStyle(iCounter, 1, yobi6CellStyle)
                                    cfg.SetData(iCounter, 1, MDConst.KANJI_YOBI_DO, False)
                            End Select
                        Next

                        preRowCounter = lastRowNum
                    Next
                    cfg.AllowMerging = AllowMergingEnum.Free
                    cfg.Cols(0).AllowMerging = True
                    cfg.Cols(1).AllowMerging = True
                    cfg.Cols(2).AllowMerging = False
                    cfg.Cols(3).AllowMerging = False
                    cfg.Cols(4).AllowMerging = False
                    cfg.Cols(5).AllowMerging = False
                    cfg.Cols(6).AllowMerging = False
                    cfg.Cols(7).AllowMerging = False
                    dbAccess.Disconnect()
                    Call AddDaysToFixedColumn()
                    cfg.Redraw = True
                    Me.fraSchedule.Visible = True
                    FrmWaitInfo.CloseWaitForm()

                End If
            End If
        Catch ex As Exception
            FrmWaitInfo.CloseWaitForm()
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "btnSearch_Click")
        Finally
            Cursor.Current = Cursors.Default
            FrmWaitInfo.CloseWaitForm()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：checkSelected
    '   名称　：選択チェック
    '   概要　：検索や日程新規登録を行う前に期と対象年月が選択したかをチェック
    '   作成日：2011/11/09(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/09(木) Ryu  新規作成
    '************************************************************************************
    Public Function checkSelected() As Boolean
        Dim rtnBln As Boolean = True                '戻り値
        Dim dbAccess As New CLAccessMdb             'DBアクセスクラス
        Dim sql As String                           'SQL分
        Dim d_from As String                        '期の開始日
        Dim d_to As String                          '期の終了日
        Dim dt As DataTable                         'ＳＱＬの実行結果データセット
        Dim dtRow As DataRow                        '一行のデータ
        Dim strDate As String                       '選択した日
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '対象期選択チェック
            If Me.cmbSearchPeriod.Text = "" Then
                CLMsg.Show("GE0010", SCHEDULE_KI_NAME)
                Me.cmbSearchPeriod.BackColor = Color.Pink
                rtnBln = False
                Return rtnBln
            Else
                Me.cmbSearchPeriod.BackColor = Color.White
            End If
            '対象年月選択チェック
            If Me.cmbSearchYear.Text = "" Then
                CLMsg.Show("GE0010", SCHEDULE_SEARCH_YEAR)
                Me.cmbSearchYear.BackColor = Color.Pink
                rtnBln = False
                Return rtnBln
            Else
                Me.cmbSearchYear.BackColor = Color.White
            End If
            If Me.cmbSearchMonth.Text = "" Then
                CLMsg.Show("GE0010", SCHEDULE_SEARCH_MONTH)
                Me.cmbSearchMonth.BackColor = Color.Pink
                rtnBln = False
                Return rtnBln
            Else
                Me.cmbSearchMonth.BackColor = Color.White
            End If
            '選択した年月は対象期の期間内チェック
            If Me.cmbSearchPeriod.Text <> "" And Me.cmbSearchYear.Text <> "" And Me.cmbSearchMonth.Text <> "" Then
                strDate = Format(Date.Parse(Me.cmbSearchYear.Text + "/" + Me.cmbSearchMonth.Text + "/01"), MDConst.DATE_YYYYMMDD_FORMAT)
                dbAccess.Connect()

                sql = "Select d_from,d_to from period where c_period_id='" + Me.cmbSearchPeriod.SelectedValue + "'"
                dt = dbAccess.ExecuteSql(sql)
                If dt.Rows.Count > 0 Then
                    dtRow = dt.Rows(0)
                    If InStr(dtRow("d_from"), "/") > 0 Then
                        d_from = Format(Date.Parse(dtRow("d_from")), MDConst.DATE_YYYYMMDD_FORMAT)
                    Else
                        d_from = Format(Date.Parse(Mid(dtRow("d_from"), 1, 4) + "/" + Mid(dtRow("d_from"), 5, 2) + "/" + Mid(dtRow("d_from"), 7, 2)), MDConst.DATE_YYYYMMDD_FORMAT)
                    End If
                    If InStr(dtRow("d_to"), "/") > 0 Then
                        d_to = Format(Date.Parse(dtRow("d_to")), MDConst.DATE_HHMM_FORMAT)
                    Else
                        d_to = Format(Date.Parse(Mid(dtRow("d_to"), 1, 4) + "/" + Mid(dtRow("d_to"), 5, 2) + "/" + Mid(dtRow("d_to"), 7, 2)), MDConst.DATE_YYYYMMDD_FORMAT)
                    End If

                    dbAccess.Disconnect()
                    '期の範囲外を選択した場合、エラー
                    If strDate > d_to Or strDate < d_from Then
                        CLMsg.Show("GE0013", d_from, d_to)
                        rtnBln = False
                    End If
                Else
                    dbAccess.Disconnect()
                    '期の期間取得できなかった場合、エラー
                    CLMsg.Show("GE0004", SCHEDULE_KI_KIKAN_NAME)
                    rtnBln = False
                End If
            End If
            Return rtnBln
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "checkSelected")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Function

    '************************************************************************************
    '   ＩＤ　：btnPrePrint_Click
    '   名称　：日程表印刷
    '   概要　：日程表をそのまま印刷
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnPrePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrePrint.Click
        Dim cfg As C1FlexGrid
        Dim resourceObj As CrystalDecisions.CrystalReports.Engine.ReportDocument        'レポート'日程表FLEXGRID
        Dim fmPrint As FM000203                                                         '印刷プレビュー
        Dim ds As DS0202P1                                                              'データセット
        Dim iCounter As Integer                                                         '行数
        Dim preLineNum As String                                                        '直前の日
        Dim c_message_1 As String                                                       '中執データ
        Dim c_message_2 As String                                                       '専門部データ
        Dim c_message_3 As String                                                       '産別データ
        Dim hasDataFlg As Boolean                                                       'データ有無の判断フラグ
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            FrmWaitInfo.ShowWaitForm(Nothing)
            cfg = Me.cfgScheduleList
            resourceObj = New CR0202P1
            '印刷プレイビュー
            fmPrint = New FM000203
            ds = New DS0202P1
            fmPrint.ButtonShowType = 3
            fmPrint.PrintCntVisible = True
            fmPrint.ObjResource = resourceObj

            Dim drDetail As DS0202P1.dtDetailRow
            preLineNum = "01"
            c_message_1 = ""
            c_message_2 = ""
            c_message_3 = ""
            hasDataFlg = False
            For iCounter = 1 To cfg.Rows.Count - 1
                If preLineNum = cfg.GetData(iCounter, 0) Then
                    If cfg.GetDataDisplay(iCounter, 3) <> "" Then
                        If c_message_1 = "" Then
                            c_message_1 = cfg.GetData(iCounter, 3)
                        Else
                            c_message_1 = c_message_1 + vbCrLf + cfg.GetData(iCounter, 3)
                        End If
                        hasDataFlg = True
                    End If

                    If cfg.GetDataDisplay(iCounter, 5) <> "" Then
                        If c_message_2 = "" Then
                            c_message_2 = cfg.GetData(iCounter, 5)
                        Else
                            c_message_2 = c_message_2 + vbCrLf + cfg.GetData(iCounter, 5)
                        End If
                        hasDataFlg = True
                    End If
                    If cfg.GetDataDisplay(iCounter, 7) <> "" Then
                        If c_message_3 = "" Then
                            c_message_3 = cfg.GetData(iCounter, 7)
                        Else
                            c_message_3 = c_message_3 + vbCrLf + cfg.GetData(iCounter, 7)
                        End If
                        hasDataFlg = True
                    End If
                Else
                    drDetail = ds.dtDetail.NewRow
                    drDetail.BeginEdit()
                    drDetail.s_day = cfg.GetData(iCounter - 1, 0)
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
                        hasDataFlg = True
                    End If
                    If cfg.GetDataDisplay(iCounter, 5) <> "" Then
                        c_message_2 = cfg.GetData(iCounter, 5)
                        hasDataFlg = True
                    End If
                    If cfg.GetDataDisplay(iCounter, 7) <> "" Then
                        c_message_3 = cfg.GetData(iCounter, 7)
                        hasDataFlg = True
                    End If
                End If
            Next
            '最後のデータを追加
            drDetail = ds.dtDetail.NewRow
            drDetail.BeginEdit()
            drDetail.s_day = cfg.GetData(iCounter - 1, 0)
            drDetail.c_meesage_1 = c_message_1
            drDetail.c_meesage_2 = c_message_2
            drDetail.c_meesage_3 = c_message_3
            drDetail.EndEdit()
            ds.dtDetail.Rows.Add(drDetail)
            If Not hasDataFlg Then
                fmPrint.Dispose()
                FrmWaitInfo.CloseWaitForm()
                CLMsg.Show("GE0090", "日程表データ")
                Exit Sub
            End If
            Dim drHeader As DS0202P1.dtHeaderRow
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.c_period = Replace(Replace(cmbSearchPeriod.Text, "第", ""), "期", "")
            drHeader.d_rev = CStr(CInt(selectRev(cmbSearchPeriod.SelectedValue, cmbSearchYear.Text + cmbSearchMonth.Text)) + 1)
            drHeader.s_year = cmbSearchYear.Text
            drHeader.s_month = cmbSearchMonth.Text
            drHeader.c_date1 = Me.cfgScheduleList.Cols(3).Caption
            drHeader.c_date2 = Me.cfgScheduleList.Cols(7).Caption
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)

            resourceObj.SetDataSource(ds)
            FrmWaitInfo.CloseWaitForm()
            Call fmPrint.ShowDialog()
            Select Case fmPrint.IntQlickBtnFlag
                Case 3
                    '印刷
                    fmPrint.PrintOut()
                    '印刷VERを更新
                    updateRev(cmbSearchPeriod.SelectedValue, cmbSearchYear.Text + cmbSearchMonth.Text)
                Case 2
                    'キャンセル

            End Select

        Catch ex As Exception
            FrmWaitInfo.CloseWaitForm()
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "btnPrePrint_Click")
        Finally
            FrmWaitInfo.CloseWaitForm()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：cmbSearchPeriod_KeyPress
    '   名称　：期コンボボックスキープレス処理
    '   概要　：
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub cmbSearchPeriod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSearchPeriod.KeyPress
        If e.KeyChar = vbCr Then
            btnSearch_Click(sender, Nothing)
        End If
    End Sub

    '************************************************************************************
    '   ＩＤ　：cmbSearchYear_KeyPress
    '   名称　：対象年コンボボックスキープレス処理
    '   概要　：
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub cmbSearchYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSearchYear.KeyPress
        If e.KeyChar = vbCr Then
            btnSearch_Click(sender, Nothing)
        End If
    End Sub

    '************************************************************************************
    '   ＩＤ　：cmbSearchMonth_KeyPress
    '   名称　：対象月コンボボックスキープレス処理
    '   概要　：
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub cmbSearchMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbSearchMonth.KeyPress
        If e.KeyChar = vbCr Then
            btnSearch_Click(sender, Nothing)
        End If
    End Sub

    '************************************************************************************
    '   ＩＤ　：cmbSearchPeriod_SelectionChangeCommitted
    '   名称　：対象期選択した後
    '   概要　：対象期選択した後に対象年を期の期間内年に設定しなおす
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub cmbSearchPeriod_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearchPeriod.SelectionChangeCommitted
        Dim strKiValue As String                            '期のコード
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strKiValue = Me.cmbSearchPeriod.SelectedValue
            If strKiValue <> Me.lblKiValue.Text Then
                Call setCmbYear()
                Me.lblKiValue.Text = strKiValue
                Me.fraSchedule.Visible = False
                Me.cmbSearchYear.SelectedIndex = cmbSearchYear.FindString(Now.Year.ToString)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "cmbSearchPeriod_SelectionChangeCommitted")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：cfgScheduleList_DoubleClick
    '   名称　：日程表のダブルクリック
    '   概要　：日程表をダブルクリックしたとき、日程詳細画面へ遷移
    '   作成日：2011/11/11(木) Ryu
    '   更新日：2016/10/05(水) Fujisaku
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '       　：2016/10/05(水) Fujisaku  日付の引数追加
    '************************************************************************************
    Private Sub cfgScheduleList_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cfgScheduleList.DoubleClick
        Dim pn As Panel                                             'メインパネル
        Dim uc As Control                                           '遷移先コントロール
        Dim cfg As C1FlexGrid                                       '日程表
        Dim selDay As String                                        '選択した日程の日付2桁
        Dim selSEQ As String                                        '選択した日程のシーケンス番号   
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            'ログイン権限により参照不可の場合、処理中止
            If strRead <> "1" Then
                Exit Sub
            End If
            '日程表を選択しているかチェック
            cfg = Me.cfgScheduleList
            selDay = CStr(cfg.GetData(cfg.RowSel, 0))
            selSEQ = CStr(cfg.GetData(cfg.RowSel, cfg.ColSel - 1))
            If selSEQ = "" Or (Not IsNumeric(selSEQ)) Then
                CLMsg.Show("GI0007")
                Exit Sub
            End If
            '日程表詳細画面を表示
            Me.Visible = False
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020203)
            If uc Is Nothing Then
                uc = New UC020203
                Call pn.Controls.Add(uc)
                uc.Visible = False
            End If
            '該当日程表の内容を画面に表示
            If Not IniUC020203(uc, selDay, selSEQ) Then
                CLMsg.Show("GE0001")
                Exit Sub
            End If

            uc.Visible = True

            '画面項目は編集不可にする
            Call EnabledUC020203(uc, False, Color.White)

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "cfgScheduleList_DoubleClick")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnFileOutput_Click
    '   名称　：ファイル出力
    '   概要　：日程表をCSVファイルに出力
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnFileOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFileOutput.Click
        Dim sfd As New System.Windows.Forms.SaveFileDialog      'ファイル保存ダイアログ
        Dim fileName As String                                  '出力するファイル名
        Dim dt As New DataTable                                 '出力するデータ
        Dim iCounter As Integer                                 'データの行数
        Dim strYYMM As Integer                                  '対象年月
        Dim strDYYMM As Integer = "202007"                                 '対象年月2020年07月
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            sfd.Title = SCHEDULE_FILEOUTPUT_TITLE
            If Me.cmbSearchYear.Text = "" Or Me.cmbSearchMonth.Text = "" Then
                MsgBox("対象年月を選択し、検索してからファイル出力を行ってください。")
                Exit Sub
            End If
            fileName = "日程表-" + Format(New Date(CInt(Me.cmbSearchYear.Text), CInt(Me.cmbSearchMonth.Text), CInt("01")), "yyyy年MM月") + ".csv"
            sfd.FileName = fileName
            sfd.Filter = "CSVファイル(*.csv)|*.csv"
            If sfd.ShowDialog() = DialogResult.OK Then
                'cfg = Me.cfgScheduleList
                'cfg.SaveGrid(sfd.FileName, FileFormatEnum.TextComma, FileFlags.VisibleOnly, System.Text.Encoding.UTF8)
                'ヘッダーデータ
                dt.Columns.Add(KANJI_YOBI_NICHI)
                dt.Columns.Add(KANJI_YOBI)
                strYYMM = Me.cmbSearchYear.Text + Me.cmbSearchMonth.Text
                If strYYMM <= strDYYMM Then
                    dt.Columns.Add(SCHEDULE_BUNRUI_INDEX_0)
                    dt.Columns.Add(SCHEDULE_BUNRUI_INDEX_1)
                    dt.Columns.Add(SCHEDULE_BUNRUI_INDEX_2)
                Else
                    dt.Columns.Add(SCHEDULE_BUNRUI_INDEX_3)
                    dt.Columns.Add(SCHEDULE_BUNRUI_INDEX_1)
                    dt.Columns.Add(SCHEDULE_BUNRUI_INDEX_4)
                End If
                '詳細データ
                For iCounter = 1 To cfgScheduleList.Rows.Count - 1
                    Dim dtrow As DataRow = dt.NewRow
                    dtrow(0) = cfgScheduleList.GetData(iCounter, 0)
                    dtrow(1) = cfgScheduleList.GetData(iCounter, 1)
                    dtrow(2) = cfgScheduleList.GetData(iCounter, 3)
                    dtrow(3) = cfgScheduleList.GetData(iCounter, 5)
                    dtrow(4) = cfgScheduleList.GetData(iCounter, 7)
                    dt.Rows.Add(dtrow)
                Next
                'CSVファイル出力
                'CsvPut(dt, Replace(sfd.FileName, fileName, ""), fileName)
                If CsvPut(dt, sfd.FileName) Then
                    MsgBox("日程表のCSVファイル " + sfd.FileName + " への出力は完了しました。", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "CSVファイル出力")
                Else
                    CLMsg.Show("BE0022", sfd.FileName)
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "btnFileOutput_Click")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnPrePrint_Click
    '   名称　：組合日程表印刷
    '   概要　：組合日程表プレ印刷
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub btnSchedulePrePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSchedulePrePrint.Click
        Dim cfg As C1FlexGrid                                                                       '日程表FLEXGRID
        Dim resourceObj As CrystalDecisions.CrystalReports.Engine.ReportDocument                    'レポート
        Dim fmPrint As FM000203                                                                     '印刷プレビュー
        Dim ds As DS0202P2                                                                          'データセット
        Dim strPeriodValue As String                                                                '期のコード
        Dim strYear As String                                                                       '検索対象年
        Dim strMonth As String                                                                      '検索対象月
        Dim sql As String                                                                           'SQL分
        Dim dbAccess As New CLAccessMdb                                                             'DBアクセス
        Dim dt As DataTable                                                                         'データテーブル
        Dim dtRow As DataRow                                                                        '一行のデータ
        Dim c_ksh As String                                                                         '会社コード
        Dim drDetail As DS0202P2.dtDetailRow                                                        '一行の詳細データ
        FrmWaitInfo.ShowWaitForm(Nothing)
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            cfg = Me.cfgScheduleList
            resourceObj = New CR0202P2
            '印刷プレイビュー
            fmPrint = New FM000203
            ds = New DS0202P2
            fmPrint.ButtonShowType = 3
            fmPrint.PrintCntVisible = True
            fmPrint.ObjResource = resourceObj
            strPeriodValue = Me.cmbSearchPeriod.SelectedValue
            strYear = Me.cmbSearchYear.Text
            strMonth = Me.cmbSearchMonth.Text
            '選択した年月の日に登録されている日程をDBより取得
            c_ksh = MDLoginInfo.Ksh

            'sql = "Select k_schedule_divide,l_information_name,l_list_name,d_date,l_place,k_union from schedule_dtl_list where c_ksh='" + c_ksh + "' and c_period_id='" + strPeriodValue + "' and d_month='" + strYear + strMonth + "' order by d_date"
            sql = "select l_list_name,d_date,l_place,l_name from "
            sql = sql + "(Select k_schedule_divide,l_information_name,l_list_name,d_date,l_place,k_union from schedule_dtl_list where c_ksh='" + c_ksh + "' and c_period_id='" + strPeriodValue + "' and d_month='" + strYear + strMonth + "') as schedule_dtl_list "
            sql = sql + "left join "
            sql = sql + "(select c_constant_seq,l_name from constant_dtl where c_constant='" + CONSTANT_ID_SCD_SHIBU + "')as  constant_dtl "
            sql = sql + " on constant_dtl.c_constant_seq=schedule_dtl_list.k_union order by d_date" & UtDb.DbOrderOffset
            'MSSQL OK
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            dbAccess.Disconnect()
            If dt.Rows.Count = 0 Then
                fmPrint.Dispose()
                FrmWaitInfo.CloseWaitForm()
                CLMsg.Show("GE0090", "日程表データ")
                Exit Sub
            End If
            '詳細日程情報を設定()
            For Each dtRow In dt.Rows
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                drDetail.s_day = strYear + "/" + strMonth + "/" + dtRow("d_date")
                If Not IsDBNull(dtRow("l_list_name")) Then
                    drDetail.l_committee_name = dtRow("l_list_name")
                End If
                drDetail.k_union = dtRow("l_name")
                If Not IsDBNull(dtRow("l_place")) Then
                    drDetail.c_open_place = dtRow("l_place")
                End If
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail)
            Next
            'ヘッダー情報設定
            Dim drHeader As DS0202P2.dtHeaderRow
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.c_period = Replace(Replace(cmbSearchPeriod.Text, "第", ""), "期", "")
            drHeader.d_month = strYear + strMonth
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)
            resourceObj.SetDataSource(ds)
            FrmWaitInfo.CloseWaitForm()
            Call fmPrint.ShowDialog()
            Select Case fmPrint.IntQlickBtnFlag
                Case 3
                    '印刷
                    fmPrint.PrintOut()
                    '印刷VERを更新
                    updateRev(cmbSearchPeriod.SelectedValue, cmbSearchYear.Text + cmbSearchMonth.Text)
                Case 2
                    'キャンセル
            End Select
        Catch ex As Exception
            FrmWaitInfo.CloseWaitForm()
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "btnSchedulePrePrint_Click")
        Finally
            FrmWaitInfo.CloseWaitForm()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：cmbSearchYear_SelectionChangeCommitted
    '   名称　：対象年選択した後
    '   概要　：対象年選択した後に日程表を非表示にする
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub cmbSearchYear_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearchYear.SelectionChangeCommitted

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If strPreYear <> cmbSearchYear.Text Then
                Me.fraSchedule.Visible = False
                strPreYear = cmbSearchYear.Text
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "cmbSearchYear_SelectionChangeCommitted")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "関数"
    '************************************************************************************
    '   ＩＤ　：setCmbYear
    '   名称　：対象年選択の設定
    '   概要　：選択した期の有効範囲年を抽出し、対象年コンボボックス選択肢に設定
    '   作成日：2011/11/11(木) Ryu
    '   引数　：strKiValue  選択した期のコード
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub setCmbYear()
        Dim dbAccess As New CLAccessMdb             'DBアクセスクラス
        Dim sql As String                           'SQL分
        Dim d_from As String                        '期の開始日
        Dim d_to As String                          '期の終了日
        Dim dt As DataTable                         'ＳＱＬの実行結果データセット
        Dim dtRow As DataRow                        '一行のデータ
        Dim cmbYear As ComboBox                     '対象年コンボボックス
        Dim strKiValue As String                    '期のコード
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strKiValue = Me.cmbSearchPeriod.SelectedValue
            If strKiValue <> "" Then
                cmbYear = Me.cmbSearchYear
                '現在の選択肢をクリア
                cmbYear.Items.Clear()
                sql = "Select d_from,d_to from period where c_period_id='" + strKiValue + "'"
                dbAccess.Connect()
                dt = dbAccess.ExecuteSql(sql)
                If dt.Rows.Count > 0 Then
                    dtRow = dt.Rows(0)
                    'd_from = Format(Date.Parse(dtRow("d_from")), SCHEDULE_DATE_YYYY_FORMAT)
                    'd_to = Format(Date.Parse(dtRow("d_to")), SCHEDULE_DATE_YYYY_FORMAT)
                    d_from = Mid(dtRow("d_from"), 1, 4)
                    d_to = Mid(dtRow("d_to"), 1, 4)
                    Do Until d_from > d_to
                        cmbYear.Items.Add(d_from)
                        d_from = CStr(CInt(d_from) + 1)
                    Loop
                Else
                    '期の期間取得できなかった場合、エラー
                    CLMsg.Show("GE0004", SCHEDULE_KI_KIKAN_NAME)
                End If
                dbAccess.Disconnect()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "setCmbYear")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：IniUC020203
    '   名称　：UC020203画面の初期化
    '   概要　：選択した日程情報で画面を初期化する
    '   作成日：2011/11/11(木) Ryu
    '   引数　：uc  画面
    '           selDay  日程表で選択された日付2桁
    '           selSEQ　日程表で選択された日程のシーケンス
    '   更新日：2016/10/05(水) Fujisaku
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '       　：2016/10/05(水) Fujisaku  引数追加
    '************************************************************************************
    Private Function IniUC020203(ByVal uc As Control, ByVal selDay As String, ByVal selSEQ As String) As Boolean
        Dim dbAccess As New CLAccessMdb             '共通ＤＢアクセスクラス
        Dim dt As DataTable                         'ＳＱＬの実行結果データセット
        Dim sql As String = Nothing                 'ＳＱＬ分
        Dim dtRow As DataRow                        '一行のデータ
        Dim title As Label                          '画面タイトル
        Dim timePickerStart As DateTimePicker       '開始時間
        Dim timePickerEnd As DateTimePicker         '終了時間
        Dim fraScheduleDetail As GroupBox           '分類フレーム
        Dim fraScheduleKind As GroupBox             '詳細フレーム
        Dim rtnBln As Boolean                       '戻り値
        Dim strKiID As String                       '期のコード
        Dim strYear As String                       '日程の年
        Dim strMonth As String                      '日程の月
        Dim strDay As String                        '日程の日
        Dim strCommitteeName As String              '委員会名
        Dim strShibu As String                      '支部
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            rtnBln = False
            sql = "Select c_period_id,d_date,d_month,l_list_name,k_schedule_divide,k_union,l_information_name,l_open_object,l_subject,l_place,d_time_start,d_time_end,d_time_required,l_biko from schedule_dtl_list where s_schedule_seq='" + CStr(selSEQ) + "' and d_date='" + CStr(selDay) + "'"
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            dbAccess.Disconnect()
            If dt.Rows.Count = 1 Then
                dtRow = dt.Rows(0)
                '画面タイトル
                title = uc.Controls("lblTitle")
                title.Text = SCHEDULE_TITLE_DETAIL
                fraScheduleKind = uc.Controls("fraScheduleKind")
                '分類
                If Not IsDBNull(dtRow("k_schedule_divide")) Then
                    fraScheduleKind.Controls("lblKindText").Text = CStr(dtRow("k_schedule_divide"))
                Else
                    fraScheduleKind.Controls("lblKindText").Text = ""
                End If
                '開始日ラベルの位置
                fraScheduleKind.Controls("lblStartDate").Location = New System.Drawing.Point(fraScheduleKind.Controls("lblStartDate").Location.X + 100, fraScheduleKind.Controls("lblStartDate").Location.Y)

                fraScheduleDetail = uc.Controls("fraScheduleDetail")
                '委員会名
                If Not IsDBNull(dtRow("l_list_name")) Then
                    strCommitteeName = CStr(dtRow("l_list_name"))
                Else
                    strCommitteeName = ""
                End If
                '支部コード
                If Not IsDBNull(dtRow("k_union")) Then
                    strShibu = CStr(dtRow("k_union"))
                Else
                    strShibu = ""
                End If

                '表示名称
                If Not IsDBNull(dtRow("l_information_name")) Then
                    fraScheduleDetail.Controls("txtScheduleName").Text = CStr(dtRow("l_information_name"))
                Else
                    fraScheduleDetail.Controls("txtScheduleName").Text = ""
                End If
                If (Not IsDBNull(dtRow("d_date"))) And (Not IsDBNull(dtRow("d_month"))) Then
                    strYear = Mid(CStr(dtRow("d_month")), 1, 4)
                    strMonth = Mid(CStr(dtRow("d_month")), 5)
                    strDay = CStr(dtRow("d_date"))
                    fraScheduleKind.Controls("lblStartDate").Text = strYear + SCHEDULE_YEAR_TEXT + strMonth + SCHEDULE_MONTH_TEXT + strDay + SCHEDULE_DAY_TEXT
                Else
                    strYear = CStr(Date.Today.Year)
                    strMonth = CStr(Date.Today.Month)
                    strDay = CStr(Date.Today.Day)
                End If
                '開始時間
                timePickerStart = fraScheduleDetail.Controls("dtpStart")
                timePickerStart.CustomFormat = SCHEDULE_DATETIMEPICKER_CUSTOMFORMAT_TIME
                timePickerStart.Format = DateTimePickerFormat.Custom
                timePickerStart.ShowUpDown = True
                If Not IsDBNull(dtRow("d_time_start")) Then
                    timePickerStart.Value = Date.Parse(strYear + "/" + strMonth + "/" + strDay + " " + Mid(CStr(dtRow("d_time_start")), 1, 2) + ":" + Mid(CStr(dtRow("d_time_start")), 3, 2))
                End If
                '終了時間
                timePickerEnd = fraScheduleDetail.Controls("dtpEnd")
                timePickerEnd.CustomFormat = SCHEDULE_DATETIMEPICKER_CUSTOMFORMAT_TIME
                timePickerEnd.Format = DateTimePickerFormat.Custom
                timePickerEnd.ShowUpDown = True
                If Not IsDBNull(dtRow("d_time_end")) Then
                    timePickerEnd.Value = Date.Parse(strYear + "/" + strMonth + "/" + strDay + " " + Mid(CStr(dtRow("d_time_end")), 1, 2) + ":" + Mid(CStr(dtRow("d_time_end")), 3, 2))
                End If
                '所要時間
                If Not IsDBNull(dtRow("d_time_required")) Then
                    fraScheduleDetail.Controls("txtTime").Text = CStr(dtRow("d_time_required"))
                Else
                    fraScheduleDetail.Controls("txtTime").Text = ""
                End If
                '場所
                If Not IsDBNull(dtRow("l_place")) Then
                    fraScheduleDetail.Controls("txtLocation").Text = CStr(dtRow("l_place"))
                Else
                    fraScheduleDetail.Controls("txtLocation").Text = ""
                End If
                '目的
                If Not IsDBNull(dtRow("l_open_object")) Then
                    fraScheduleDetail.Controls("txtMokuteki").Text = CStr(dtRow("l_open_object"))
                Else
                    fraScheduleDetail.Controls("txtMokuteki").Text = ""
                End If
                '議題
                If Not IsDBNull(dtRow("l_subject")) Then
                    fraScheduleDetail.Controls("txtKatai").Text = CStr(dtRow("l_subject"))
                Else
                    fraScheduleDetail.Controls("txtKatai").Text = ""
                End If
                '備考
                If Not IsDBNull(dtRow("l_biko")) Then
                    fraScheduleDetail.Controls("txtBikou").Text = CStr(dtRow("l_biko"))
                Else
                    fraScheduleDetail.Controls("txtBikou").Text = ""
                End If
                '期のID
                If Not IsDBNull(dtRow("c_period_id")) Then
                    strKiID = CStr(dtRow("c_period_id"))
                Else
                    strKiID = ""
                End If
                '期のIDより期の表示名称を取得
                If strKiID <> "" Then
                    sql = "Select l_name from period where c_period_id='" + strKiID + "'"
                    dbAccess.Connect()
                    dt = dbAccess.ExecuteSql(sql)
                    dbAccess.Disconnect()
                    If dt.Rows.Count > 0 Then
                        dtRow = dt.Rows(0)
                        If Not IsDBNull(dtRow("l_name")) Then
                            fraScheduleKind.Controls("lblPeriod").Text = dtRow("l_name")
                        End If
                    End If
                    fraScheduleKind.Controls("lblPeriodValue").Text = strKiID
                    '画面のコンボボックス初期化
                    setComboboxValueFromDB(uc, strKiID)
                End If
                '委員会名コンボボックスの表示内容を設定
                fraScheduleDetail.Controls("cmbCommitteName").Text = strCommitteeName
                fraScheduleKind.Controls("lblSEQ").Text = selSEQ
                '支部コンボボックス表示内容を設定
                Dim cmbShibu As ComboBox = fraScheduleDetail.Controls("cmbBranch")
                Select Case strShibu
                    Case SCD_SHIBU_JOIN
                        cmbShibu.SelectedIndex = 0
                    Case SCD_SHIBU_TOKYO
                        cmbShibu.SelectedIndex = 1
                    Case SCD_SHIBU_OSAKA
                        cmbShibu.SelectedIndex = 2
                    Case Else
                        Return False
                End Select
                '戻り値
                rtnBln = True
            Else
                Return False
            End If
            Return rtnBln
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "IniUC020203")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Function

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
            cmbCommitte.Enabled = flg
            'cmbCommitte.BackColor = backColor
            cmbShibu = fraScheduleDetail.Controls("cmbBranch")
            cmbShibu.Enabled = flg
            'cmbShibu.BackColor = backColor
            txtScheduleName = fraScheduleDetail.Controls("txtScheduleName")
            txtScheduleName.ReadOnly = Not (flg)
            'txtScheduleName.BackColor = backColor
            timePickerStart = fraScheduleDetail.Controls("dtpStart")
            timePickerStart.Enabled = flg
            timePickerEnd = fraScheduleDetail.Controls("dtpEnd")
            timePickerEnd.Enabled = flg
            txtTime = fraScheduleDetail.Controls("txtTime")
            txtTime.ReadOnly = Not (flg)
            txtTime.BackColor = backColor
            txtLocation = fraScheduleDetail.Controls("txtLocation")
            txtLocation.ReadOnly = Not (flg)
            txtLocation.BackColor = backColor
            txtMokuteki = fraScheduleDetail.Controls("txtMokuteki")
            txtMokuteki.ReadOnly = Not (flg)
            txtMokuteki.BackColor = backColor
            txtKatai = fraScheduleDetail.Controls("txtKatai")
            txtKatai.ReadOnly = Not (flg)
            txtKatai.BackColor = backColor
            txtBikou = fraScheduleDetail.Controls("txtBikou")
            txtBikou.ReadOnly = Not (flg)
            txtBikou.BackColor = backColor

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "EnabledC020203")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：setComboboxValueFromDB
    '   名称　：コンボボックス生成
    '   概要　：選択した日程情報で画面を初期化する
    '   作成日：2011/11/11(木) Ryu
    '   引数　：cmb  コンボボックス
    '           sql SQL分
    '           displayName DisplayMemberの設定値
    '           valueCode　ValueMemberの設定値
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub setComboboxValueFromDB(ByVal uc As Control, ByVal strPeriodID As String)
        Dim dbAccess As New CLAccessMdb         'DBアクセス
        Dim dt As DataTable                     'データテーブル
        'Dim dtRow As DataRow                    '一行のデータ
        Dim cmbShibu As ComboBox                '支部コンボボックス
        Dim cmbCommitte As ComboBox             '委員会名コンボボックス
        Dim fraScheduleDetail As GroupBox       '日程詳細部
        'Dim d_from As String                    '期の開始日
        Dim sql As String                       'SQL文
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            fraScheduleDetail = uc.Controls("fraScheduleDetail")
            cmbShibu = fraScheduleDetail.Controls("cmbBranch")
            ' データベースに接続
            dbAccess.Connect()
            ' 支部コンボボックス
            Call CreateCboConstantDtl(dbAccess, cmbShibu, CONSTANT_ID_SCD_SHIBU, False)

            '「委員会名」コンボボックスの初期化
            cmbCommitte = fraScheduleDetail.Controls("cmbCommitteName")
            cmbCommitte.BeginUpdate()
            cmbCommitte.DataSource = Nothing
            cmbCommitte.Items.Clear()

            '期の開始日を取得()
            'sql = "select d_from from period where c_period_id='" + strPeriodID + "'"
            'データを取得()
            'dt = dbAccess.ExecuteSql(sql)
            'dtRow = dt.Rows(0)
            'd_from = dtRow("d_from")
            ' データベースの接続を切断
            dbAccess.Disconnect()

            ' 基準日取得（最新期：現在日、最新期以外：期末日）
            Dim systemDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            Dim periodDTo As String = MDLoginInfo.PeriodTo
            Dim standDate As String
            If (MDLoginInfo.PeriodNewFlg = 1) Then
                standDate = systemDate
            Else
                standDate = periodDTo
            End If
            sql = "select c_committee_id, l_name from committee where d_from<='" + standDate + "' and d_to>='" + standDate + "'"
            ' データベースに接続
            dbAccess.Connect()
            ' データを取得
            dt = dbAccess.ExecuteSql(sql)
            ' データソース設定
            cmbCommitte.DataSource = dt
            ' コンボボックス名称設定
            cmbCommitte.DisplayMember = "l_name"
            ' コンボボックス値設定
            cmbCommitte.ValueMember = "c_committee_id"
            ' データベースの接続を切断
            dbAccess.Disconnect()
            cmbCommitte.EndUpdate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "setComboboxValueFromDB")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：DefinitionCellStyles
    '   名称　：セルスタイル生成
    '   概要　：セルスタイルのカスタマイズ
    '   作成日：2011/11/14(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(木) Ryu  新規作成
    '************************************************************************************
    Private Sub DefinitionCellStyles()
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            Dim style As CellStyle = Me.cfgScheduleList.Styles.Add("Saturday")
            style.ForeColor = Color.White
            style.BackColor = Color.SkyBlue
            style = Me.cfgScheduleList.Styles.Add("Sunday")
            style.ForeColor = Color.White
            style.BackColor = Color.Pink
            Me.cfgScheduleList.Styles.Add("DayOfSaturday").ForeColor = Color.Blue
            Me.cfgScheduleList.Styles.Add("DayOfSunday").ForeColor = Color.Red
            style = Me.cfgScheduleList.Styles.Add("DayDivideLine")
            style.Border.Style = BorderStyleEnum.Flat
            style.Border.Direction = BorderDirEnum.Both
            style.Border.Width = 2
            style.Border.Color = Color.Orange
            style.WordWrap = True
            style.TextAlign = TextAlignEnum.LeftTop
            style = Me.cfgScheduleList.Styles.Add("ScheduleDivDivideLine")
            style.Border.Style = BorderStyleEnum.Flat
            style.Border.Direction = BorderDirEnum.Vertical
            style.Border.Width = 2
            style.Border.Color = Color.Orange
            style.WordWrap = True
            style.TextAlign = TextAlignEnum.LeftTop
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "DefinitionCellStyles")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：AddDaysToFixedColumn
    '   名称　：セルのスタイルを設定
    '   概要　：カスタマイズ定義したセルスタイルを設定
    '   作成日：2011/11/14(月) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) Ryu  新規作成
    '************************************************************************************
    Private Sub AddDaysToFixedColumn()
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            Dim year As Integer = Convert.ToInt32(Me.cmbSearchYear.SelectedItem)
            Dim month As Integer = Convert.ToInt32(Me.cmbSearchMonth.SelectedItem)
            Dim time As New DateTime(year, month, 1)
            Dim str As String = "01"
            Dim j As Integer
            For j = 1 To Me.cfgScheduleList.Rows.Count - 1
                Dim range As CellRange = Me.cfgScheduleList.GetCellRange((j - 1), 2, (j - 1), (Me.cfgScheduleList.Cols.Count - 1))
                If (str <> Me.cfgScheduleList.Item(j, 0).ToString) Then
                    str = Me.cfgScheduleList.Item(j, 0).ToString
                    time = time.AddDays(1)
                    range.Style = Me.cfgScheduleList.Styles.Item("DayDivideLine")
                ElseIf Not range.ContainsRow(0) Then
                    range.Style = Me.cfgScheduleList.Styles.Item("ScheduleDivDivideLine")
                End If
                Me.cfgScheduleList.Item(j, 1) = time.ToString("ddd")
            Next j
            Dim range2 As CellRange = Me.cfgScheduleList.GetCellRange((Me.cfgScheduleList.Rows.Count - 1), 2, (Me.cfgScheduleList.Rows.Count - 1), (Me.cfgScheduleList.Cols.Count - 1))
            range2.Style = Me.cfgScheduleList.Styles.Item("ScheduleDivDivideLine")
            Me.cfgScheduleList.Cols.Item(0).Name = Me.cfgScheduleList.Cols.Item(0).Caption = KANJI_YOBI_NICHI
            Me.cfgScheduleList.Cols.Item(1).Name = Me.cfgScheduleList.Cols.Item(1).Caption = KANJI_YOBI
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "AddDaysToFixedColumn")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：updateRev
    '   名称　：印刷REVを更新
    '   概要　：該当年月のレコードの印刷REVをプラス１
    '   作成日：2011/11/11(木) Ryu
    '   引数　：strKiValue  選択した期のコード
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub updateRev(ByVal strKiValue As String, ByVal strYearMonth As String)
        Dim dbAccess As New CLAccessMdb             'DBアクセスクラス
        Dim sql As String                           'SQL文
        Dim intRtn As Integer                       'SQL文実行結果
        Dim dt As DataTable                         'データテーブル
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If strKiValue <> "" And strYearMonth <> "" Then
                dbAccess.Connect()
                sql = "Select s_print_rev,s_print_up,s_up from schedule where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + strKiValue + "' and d_month='" + strYearMonth + "'"
                dt = dbAccess.ExecuteSql(sql)
                If dt.Rows.Count > 0 Then
                    sql = "Update schedule set s_print_rev=s_print_rev+1,s_print_up=s_print_up+1 where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + strKiValue + "' and d_month='" + strYearMonth + "'"
                Else
                    sql = "insert into schedule(c_ksh,c_period_id,d_month,s_print_rev,s_print_up,d_up,c_user_id_up,s_up) Values('"
                    sql = sql + MDLoginInfo.Ksh + "','"
                    sql = sql + strKiValue + "','"
                    sql = sql + strYearMonth + "','"
                    sql = sql + "1','"
                    sql = sql + "1','"
                    sql = sql + Now + "','"
                    sql = sql + MDLoginInfo.UserId + "','"
                    sql = sql + "1')"
                End If
                dbAccess.BeginTran()
                intRtn = dbAccess.ExecuteNonQuery(sql)
                If intRtn > 0 Then
                    dbAccess.CommitTran()
                    log.Info(String.Format("{0}件のデータを更新しました。", CStr(intRtn)))
                Else
                    '期の期間取得できなかった場合、エラー
                    CLMsg.Show("GE0004", "updateRev")
                End If
                dbAccess.Disconnect()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "updateRev")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '************************************************************************************
    '   ＩＤ　：selectRev
    '   名称　：印刷REVを取得
    '   概要　：該当年月のレコードの印刷REVを取得
    '   作成日：2011/11/11(木) Ryu
    '   引数　：strKiValue  選択した期のコード
    '           strYearMonth 該当年月
    '   更新日：2019/06/24(月) Fujisaku 
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '   　　　：2019/06/24(木) Fujisaku 親レコード非存在時にエラーにしないように修正
    '************************************************************************************
    Private Function selectRev(ByVal strKiValue As String, ByVal strYearMonth As String)
        Dim dbAccess As New CLAccessMdb             'DBアクセスクラス
        Dim sql As String                           'SQL文
        Dim dt As DataTable                         'データテーブル
        Dim strRtn As String                        '戻り値
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        strRtn = ""
        Try
            If strKiValue <> "" And strYearMonth <> "" Then
                sql = "select s_print_rev from schedule where c_ksh='" + MDLoginInfo.Ksh + "' and c_period_id='" + strKiValue + "' and d_month='" + strYearMonth + "'"
                dbAccess.Connect()
                dt = dbAccess.ExecuteSql(sql)
                dbAccess.Disconnect()
                If dt.Rows.Count > 0 Then
                    strRtn = IIf(IsDBNull(dt.Rows(0)("s_print_rev")), "0", dt.Rows(0)("s_print_rev"))
                Else
                    '取得できなかった場合、0を戻す
                    strRtn = "0"
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "selectRev")
        End Try
        Return strRtn
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Function

    '************************************************************************************
    '   ＩＤ　：cmbSearchYear_SelectionChangeCommitted
    '   名称　：対象年選択した後
    '   概要　：対象年選択した後に日程表を非表示にする
    '   作成日：2011/11/11(木) Ryu
    '   更新日：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/11(木) Ryu  新規作成
    '************************************************************************************
    Private Sub cmbSearchMonth_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSearchMonth.SelectionChangeCommitted
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If strPreMonth <> cmbSearchMonth.Text Then
                Me.fraSchedule.Visible = False
                strPreMonth = cmbSearchMonth.Text
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020201, SCREEN_NAME_UC020201, "cmbSearchMonth_SelectionChangeCommitted")
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
End Class
#End Region