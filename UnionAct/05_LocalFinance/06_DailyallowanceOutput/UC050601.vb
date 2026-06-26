#Region "UC050601"
'===========================================================================================================
'   クラスＩＤ　　：UC050601
'   クラス名称　　：日当データ出力画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDFile
Imports UnionAct.GUI.Common
Imports System.Text
Imports System.Text.RegularExpressions
Imports UnionAct.GUI.Document

Public Class UC050601

#Region " 定数・変数 "
    Private Const SCREEN_ID As String = SCREEN_ID_UC050601
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC050601

    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ' 画面関連

    ' ファイル保存ダイアログタイトル
    Private Const SAVE_DIALOG_TITLE As String = "ファイルの保存先を選択してください"

    Private _strTitle As String = String.Empty
    Private _datePayDay As DateTime = Nothing
    Private Const TARGET_DATE_NAME As String = "基準日"
#End Region

#Region " イベント "
#Region " フォームロード "
    '***************************************************************************************************
    '   ＩＤ　：UC050601_Load
    '   名称　：フォームロード
    '   概要　：
    '   作成日：2021/10/15(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/15(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UC050601_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim dtKengen As DataTable = Nothing ' 権限情報
        Dim IsFileOutput As Boolean = False

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            '---------------------------------------------------------------------------------------
            '   権限設定
            '---------------------------------------------------------------------------------------
            ' 権限の取得
            dtKengen = MDCommon.getGrant(MENU_ID_UC050601)

            If dtKengen.Rows.Count > 0 Then
                IsFileOutput = IIf(dtKengen.Rows(0).Item(6).ToString = "1", True, False)
            End If

            ' 出力権限
            If IsFileOutput Then
                Me.btnDailyOutput.Enabled = True        ' 日単位出力　ファイル出力ボタン活性
                Me.btnMonthlyOutput.Enabled = True      ' 月単位出力　ファイル出力ボタン活性
            Else
                Me.btnDailyOutput.Enabled = False       ' 日単位出力　ファイル出力ボタン活性
                Me.btnMonthlyOutput.Enabled = False     ' 月単位出力　ファイル出力ボタン活性
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region " 日単位出力ファイル出力ボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnDateOutput_Click
    '   名称　：日単位出力ファイル出力ボタンクリック
    '   概要　：
    '   作成日：2021/10/04(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDateOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDailyOutput.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' マウスカーソル砂時計
            Me.Cursor = Cursors.WaitCursor

            ' CSVファイル出力処理（日単位）
            Call Me.SaveDailyCsvFile()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region " 月単位出力ファイル出力ボタンクリック "
    '***************************************************************************************************
    '   ＩＤ　：btnMonthOutput_Click
    '   名称　：月単位出力ファイル出力ボタンクリック
    '   概要　：
    '   作成日：2021/10/15(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/15(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary></summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnMonthOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMonthlyOutput.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' マウスカーソル砂時計
            Me.Cursor = Cursors.WaitCursor

            ' ' CSVファイル出力処理（月単位）
            Call Me.SaveMonthlyCsvFile()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' マウスカーソルデフォルト
            Me.Cursor = Cursors.Default
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region
#End Region

#Region " 関数 "
#Region " CSVファイル出力処理（日単位） "
    '***************************************************************************************************
    '   ＩＤ　：SaveDailyCsvFile
    '   名称　：CSVファイル出力処理（日単位）
    '   概要　：日当データのCSVファイルを出力します
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2021/10/18(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>CSVファイル出力処理（日単位）</summary>
    ''' <remarks></remarks>
    Private Sub SaveDailyCsvFile()

        Dim dtCsv As DataTable = Nothing                    ' CSV出力データ
        Dim sfd As New System.Windows.Forms.SaveFileDialog  ' ダイアログボックス
        Dim dtFrom As DateTime = Nothing                    ' 開始日
        Dim dtTo As DateTime = Nothing                      ' 終了日

        Try
            ' 開始日・終了日取得
            dtFrom = CDate(Me.dtpDailyFrom.Text)            ' 開始日取得
            dtTo = CDate(Me.dtpDailyTo.Text)                ' 終了日取得

            ' 開始日終了日大小比較
            If DateTime.Compare(dtFrom, dtTo) > 0 Then
                ' 日付入れ替え時一時保存
                Dim tmpDate As DateTime = dtFrom            ' 開始日一時保存
                dtFrom = dtTo                               ' 開始日に終了日を設定
                dtTo = tmpDate                              ' 終了日に一時保存した日付を設定
            End If

            ' デフォルトファイル名（振込日_題目.csv）
            Dim strFileName As String = _
                Format(dtFrom, "yyyy-MM-dd") & _
                "～" & Format(dtTo, "yyyy-MM-dd") & _
                "_日当データ.csv"

            ' ファイル保存ダイアログタイトル
            sfd.Title = SAVE_DIALOG_TITLE
            sfd.Filter = "CSVファイル(*.csv)|*.csv"
            sfd.FileName = strFileName

            ' OKボタン押下時
            If sfd.ShowDialog = DialogResult.OK Then

                ' しばらくお待ちください画面表示
                FrmWaitInfo.ShowWaitForm(Nothing)

                ' CSV出力データ作成処理（日単位）
                dtCsv = Me.SetDailyCsvData(dtFrom, dtTo)

                ' CSV出力データ有無判定
                If dtCsv Is Nothing Then
                    CLMsg.Show("GE0202")
                Else
                    ' CSVファイル出力
                    If CsvPut(dtCsv, sfd.FileName) = True Then
                        CLMsg.Show("GI0028")
                    Else
                        CLMsg.Show("BE0022", sfd.FileName)
                    End If
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' しばらくお待ちください画面非表示
            FrmWaitInfo.CloseWaitForm()

        End Try

    End Sub
#End Region

#Region " CSVファイル出力処理（月単位） "
    '***************************************************************************************************
    '   ＩＤ　：SaveMonthlyCsvFile
    '   名称　：CSVファイル出力処理（月単位）
    '   概要　：日当データのCSVファイルを出力します
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2021/10/18(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>CSVファイル出力処理（月単位）</summary>
    ''' <remarks></remarks>
    Private Sub SaveMonthlyCsvFile()

        Dim dtCsv As DataTable = Nothing                    ' CSV出力データ
        Dim sfd As New System.Windows.Forms.SaveFileDialog  ' ダイアログボックス
        Dim dtFrom As DateTime = Nothing                    ' 開始日
        Dim dtTo As DateTime = Nothing                      ' 終了日

        Try
            '---------------------------------------------------------------------------------------
            '   開始日終了日取得
            '---------------------------------------------------------------------------------------
            dtFrom = CDate(Me.dtpMonthlyFrom.Text)          ' 開始日取得
            dtTo = CDate(Me.dtpMonthlyTo.Text)              ' 終了日取得

            ' 開始日終了日大小比較
            If DateTime.Compare(dtFrom, dtTo) > 0 Then
                ' 日付入れ替え時一時保存
                Dim tmpDate As DateTime = dtFrom            ' 開始日一時保存
                dtFrom = dtTo                               ' 開始日に終了日を設定
                dtTo = tmpDate                              ' 終了日に一時保存した日付を設定
            End If

            ' デフォルトファイル名（開始年月_終了年月_日当データ.csv）
            Dim strFileName As String = _
                Format(dtFrom, "yyyy-MM") & _
                "～" & Format(dtTo, "yyyy-MM") & _
                "_日当データ.csv"

            ' ファイル保存ダイアログタイトル
            sfd.Title = SAVE_DIALOG_TITLE
            sfd.Filter = "CSVファイル(*.csv)|*.csv"
            sfd.FileName = strFileName

            ' OKボタン押下時
            If sfd.ShowDialog = DialogResult.OK Then

                ' しばらくお待ちください画面表示
                FrmWaitInfo.ShowWaitForm(Nothing)

                ' CSV出力データ作成処理（月単位）
                dtCsv = Me.SetMonthlyCsvData(dtFrom, dtTo)
                'dtCsv = Me.SetMonthlyCsvDataOld(dtFrom, dtTo)

                ' CSV出力データ有無判定
                If dtCsv Is Nothing Then
                    CLMsg.Show("GE0202")
                Else
                    ' CSVファイル出力
                    If CsvPut(dtCsv, sfd.FileName) = True Then
                        CLMsg.Show("GI0028")
                    Else
                        CLMsg.Show("BE0022", sfd.FileName)
                    End If
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' しばらくお待ちください画面非表示
            FrmWaitInfo.CloseWaitForm()

        End Try

    End Sub
#End Region

#Region " CSV出力データ作成処理（日単位） "
    '***************************************************************************************************
    '   ＩＤ　：SetDailyCsvData
    '   名称　：CSV出力データ作成処理（日単位）
    '   概要　：CSV出力データを作成し返却します
    '   引数　：ByVal dtFrom As DateTime = 開始日
    '           ByVal dtTo   As DateTime = 終了日
    '   戻り値：
    '   作成日：2012/02/08(水)  w.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  w.suzuki  新規作成
    '***************************************************************************************************
    Private Function SetDailyCsvData( _
        ByVal dtFrom As DateTime, _
        ByVal dtTo As DateTime _
    ) As DataTable

        Dim dtResult As DataTable = Nothing         ' 処理結果
        Dim dtOutput As DataTable = Nothing         ' 日当データ取得
        Dim dtonce As DataTable = Nothing           ' 日当計算区分取得
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Try
            ' SQL作成
            strSql = ""
            strSql += "SELECT crud.委員会ID"                                                                    ' 01. 委員会ID
            strSql += "      ,crud.役職ID"                                                                      ' 02. 役職ID
            strSql += "      ,crud.委員会名"                                                                    ' 03. 委員会名
            strSql += "      ,crud.役職名"                                                                      ' 04. 役職名
            strSql += "      ,crud.社員番号"                                                                    ' 05. 社員番号
            strSql += "      ,sa.l_name AS 氏名"                                                                ' 06. 氏名
            strSql += "      ,FORMAT(crud.日付, 'yyyy/MM/dd') AS 日付"                                          ' 07. 日付
            strSql += "      ,crud.日当ID"                                                                      ' 08. 日当ID
            strSql += "      ,crud.勤務形態"                                                                    ' 09. 勤務形態
            strSql += "      ,crud.日当"                                                                        ' 10. 日当
            strSql += "      ,IIF(crud.昼食費可否 = '1', '可', '否') AS 昼食費可否"                             ' 11. 昼食費可否
            strSql += "      ,crud.昼食費"                                                                      ' 12. 昼食費
            strSql += "      ,cd.l_name AS 日当計算区分"                                                        ' 13. 日当計算区分
            strSql += "      ,FORMAT(crud.締め日付, 'yyyy/MM/dd') AS 締め日付"                                  ' 14. 締め日付
            strSql += "  FROM ("
            strSql += "        ("
            strSql += "         ("
            '                    DGM以外の情報
            strSql += "          SELECT crud1.c_committee_id                          AS 委員会ID"              ' 01. 委員会ID
            strSql += "                ,crud1.s_committee_seq                         AS 役職ID"                ' 02. 役職ID
            strSql += "                ,cmt1.l_name                                   AS 委員会名"              ' 03. 委員会名
            strSql += "                ,cmtd1.l_name                                  AS 役職名"                ' 04. 役職名
            strSql += "                ,crud1.c_user_id                               AS 社員番号"              ' 05. 社員番号
            strSql += "                ,crud1.s_day                                   AS 日付"                  ' 06. 日付
            strSql += "                ,(crud1.s_daily_pay + crud1.s_next_balance_daily_pay)         AS 日当"   ' 07. 日当
            strSql += "                ,(crud1.s_food_expenses + crud1.s_next_balance_food_expenses) AS 昼食費" ' 08. 昼食費
            strSql += "                ,crud1.c_daily_pay_id                          AS 日当ID"                ' 09. 日当ID
            strSql += "                ,dpmd1.l_name                                  AS 勤務形態"              ' 10. 勤務形態
            strSql += "                ,crud1.k_food_expenses                         AS 昼食費可否"            ' 11. 昼食費可否
            strSql += "                ,crud1.k_daily_pay_kind"                                                 ' 12. 日当計算区分
            strSql += "                ,crud1.d_daily_pay_close                       AS 締め日付"              ' 13. 締め日付


            strSql += "            FROM ("
            strSql += "                  ("
            strSql += "                   ("
            '                              /* 組合活動状況の子テーブル */
            strSql += "                    call_roll_user_dtl crud1"
            '                              /* 委員会マスタ */
            strSql += "                    INNER JOIN committee cmt1"
            strSql += "                    ON crud1.c_committee_id = cmt1.c_committee_id"                       ' 委員会IDが同じのもの
            strSql += "                   )"
            '                              /* 委員会マスタ詳細（役職マスタ） */
            strSql += "                    INNER JOIN committee_dtl cmtd1"
            strSql += "                    ON  crud1.c_committee_id  = cmtd1.c_committee_id"                    ' 委員会IDが同じのもの
            strSql += "                    AND crud1.s_committee_seq = cmtd1.s_committee_seq"                   ' 委員会ID枝番（役職ID）が同じのもの
            strSql += "                  )"
            '                              /* 日当マスタ詳細 */
            strSql += "                    INNER JOIN daily_pay_master_dtl dpmd1"
            strSql += "                    ON  crud1.c_daily_pay_id = dpmd1.c_daily_pay_id"                     ' 日当IDが同じのもの
            strSql += "                    AND crud1.c_menu_seq     = dpmd1.c_menu_seq"                         ' 日当ID枝番が同じのもの
            strSql += "                 )"
            strSql += "           WHERE crud1.s_day  >= CONVERT(DATE,'" & dtFrom & "',111)"                            ' 日付情報が開始日より大きいもの
            strSql += "             AND crud1.s_day  <= CONVERT(DATE,'" & dtTo & "',111)"                              ' 日付情報が終了日より小さいもの
            strSql += "             AND cmt1.d_from  <= '" & dtTo.ToString("yyyyMMdd") & "'"                    ' 適用開始年月日が終了日より小さいもの
            strSql += "             AND cmt1.d_to    >= '" & dtTo.ToString("yyyyMMdd") & "'"                    ' 適用終了年月日が終了日より大きいもの
            strSql += "             AND cmtd1.d_from <= '" & dtTo.ToString("yyyyMMdd") & "'"                    ' 適用開始年月日が終了日より小さいもの
            strSql += "             AND cmtd1.d_to   >= '" & dtTo.ToString("yyyyMMdd") & "'"                    ' 適用終了年月日が終了日より大きいもの
            strSql += "          UNION"

            '                    DGM情報
            strSql += "          SELECT crud2.c_committee_id                          AS 委員会ID"              ' 01. 委員会ID
            strSql += "                ,crud2.s_committee_seq                         AS 役職ID"                ' 02. 役職ID
            strSql += "                ,crud2.c_committee_id                          AS 委員会名"              ' 03. 委員会名
            strSql += "                ,crud2.c_committee_id                          AS 役職名"                ' 04. 役職名
            strSql += "                ,crud2.c_user_id                               AS 社員番号"              ' 05. 社員番号
            strSql += "                ,FORMAT(crud2.s_day, 'yyyy/MM/dd')             AS 日付"                  ' 06. 日付
            strSql += "                ,(crud2.s_daily_pay + crud2.s_next_balance_daily_pay)         AS 日当"   ' 07. 日当
            strSql += "                ,(crud2.s_food_expenses + crud2.s_next_balance_food_expenses) AS 昼食費" ' 08. 昼食費
            strSql += "                ,crud2.c_daily_pay_id                          AS 日当ID"                ' 09. 日当ID
            strSql += "                ,dpmd2.l_name                                  AS 勤務形態"              ' 10. 勤務形態
            strSql += "                ,crud2.k_food_expenses                         AS 昼食費可否"            ' 11. 昼食費可否
            strSql += "                ,crud2.k_daily_pay_kind"                                                 ' 12. 日当計算区分
            strSql += "                ,FORMAT(crud2.d_daily_pay_close, 'yyyy/MM/dd') AS 締め日付"              ' 13. 締め日付
            '                           組合活動状況の子テーブル
            strSql += "            FROM ("
            strSql += "                  call_roll_user_dtl crud2"
            '                            日当マスタ詳細
            strSql += "                  INNER JOIN daily_pay_master_dtl dpmd2"
            strSql += "                  ON crud2.c_menu_seq = dpmd2.c_menu_seq"                                ' 日当ID枝番が同じのもの
            strSql += "                 )"
            strSql += "           WHERE crud2.s_day          >= CONVERT(DATE,'" & dtFrom & "',111)"                    ' 日付情報が開始日より大きいもの
            strSql += "             AND crud2.s_day          <= CONVERT(DATE,'" & dtTo & "',111)"                      ' 日付情報が終了日より小さいもの
            strSql += "             AND crud2.c_committee_id  = 'DGM'"                                          ' 委員会IDが 'DGM' のもの
            strSql += "             AND dpmd2.c_daily_pay_id  = '001'"                                          ' 日当IDが '001' のもの
            strSql += "         ) As crud"

            '                   組合員属性
            strSql += "         INNER JOIN ("
            strSql += "             SELECT sa2.c_user_id"                                                       ' 個人認証ID
            strSql += "                   ,sa2.l_name"                                                          ' 名前
            strSql += "               FROM ("
            strSql += "                     staf_attribute sa2"                                                 ' /* 組合員属性 */
            '                               終了日以下で最大の対象年月取得
            strSql += "                     INNER JOIN ("
            strSql += "                         SELECT sa1.c_user_id"                                           ' 個人認証ID
            strSql += "                               ,MAX(sa1.d_from) AS MAX_dfrom"                            ' 最大対象年月
            strSql += "                           FROM staf_attribute sa1"                                      ' /* 組合員属性 */
            strSql += "                          WHERE sa1.d_from <= '" & dtTo.ToString("yyyyMMdd") & "'"       ' 対象年月が終了日よりも小さいもの
            strSql += "                          GROUP BY sa1.c_user_id"                                        ' 個人認証IDで集約
            strSql += "                     ) sa3"
            strSql += "                     ON  sa2.c_user_id = sa3.c_user_id"                                  ' 個人認証IDが同じもの
            strSql += "                     AND sa2.d_from    = sa3.MAX_dfrom"                                  ' 対象年月が最大対象年月と同じもの
            strSql += "                    )"
            strSql += "         ) sa"
            strSql += "         ON crud.社員番号 = sa.c_user_id"                                                ' 個人認証IDが同じもの
            strSql += "        )"

            '                  日当計算区分名称
            strSql += "        LEFT JOIN ("
            strSql += "            SELECT cd1.c_constant_seq"                                                   ' 定数ID枝番
            strSql += "                  ,cd1.l_name"                                                           ' 名称
            strSql += "              FROM constant_dtl cd1"                                                     '/* 定数マスタ詳細 */
            strSql += "             WHERE cd1.c_constant = 'DAILY_PAY_KIND'"                                    ' 定数IDが 'DAILY_PAY_KIND' のもの
            strSql += "        ) cd"
            strSql += "        ON crud.k_daily_pay_kind = cd.c_constant_seq)"                                   ' 日当IDと定数ID枝番が同じもの

            '                  委員会ID、役職ID、社員番号、日付で並び替え
            strSql += "  ORDER BY crud.委員会ID"
            strSql += "          ,crud.役職ID"
            strSql += "          ,crud.社員番号"
            strSql += "          ,crud.日付"
            strSql += ";"

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtOutput = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = dtOutput.Rows.Count

            ' 件数チェック
            If intRetCnt > 0 Then
                dtResult = dtOutput
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' データベース切断
            Call clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return dtResult

    End Function
#End Region

#Region " CSV出力データ作成処理（月単位） "
    '***************************************************************************************************
    '   ＩＤ　：SetMonthlyCsvData
    '   名称　：CSV出力データ作成処理（月単位）
    '   概要　：CSV出力データを作成し返却します
    '   引数　：ByVal dtFrom As DateTime = 開始日
    '           ByVal dtTo   As DateTime = 終了日
    '   戻り値：CSVデータ
    '   作成日：2021/10/18(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>CSV出力データ作成処理（月単位）</summary>
    ''' <param name="dtFrom">開始日</param>
    ''' <param name="dtTo">終了日</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetMonthlyCsvData( _
        ByVal dtFrom As DateTime, _
        ByVal dtTo As DateTime _
    ) As DataTable

        Dim dtOutput As DataTable = New DataTable           ' 日当データ基本情報
        Dim strSql As String = ""                           ' SQL文
        Dim clsDb As New CLAccessMdb                        ' データベースクラス
        Dim intRetCnt As Integer = 0                        ' 検索結果件数

        Dim dtResult As DataTable = Nothing                 ' CSVデータ用データテーブル
        Dim rowResult As DataRow = Nothing                  ' CSVデータ用データロー

        ' 取得データ
        Dim strUserId As String = ""                        ' 社員番号
        Dim strName As String = ""                          ' 氏名
        Dim strYears As String = ""                         ' 日付（文字列型）
        Dim dtYears As DateTime = Nothing                   ' 日付（日付型）
        Dim strDailyPayTotal As String = ""                 ' 月日当計
        Dim strFoodExpensesTotal As String = ""             ' 月昼食費計
        Dim strNextBalanceDailyPayTotal As String = ""      ' 前回差分月日当計
        Dim strNextBalanceFoodExpensesTotal As String = ""  ' 前回差分昼食費計
        Dim strTotal As String = ""                         ' 日当計
        Dim str3Total As String = ""                        ' 日当計（対象年月の3ヶ月）
        Dim strDailyPayKind As String = ""                  ' 日当計算区分
        Dim strDailyPayKindName As String = ""              ' 日当計算区分名称
        Dim strDailyPayClose As String = ""                 ' 締め日（文字列型）
        Dim dtDailyPayClose As DateTime = Nothing           ' 締め日（日付型）
        Dim strDailyPayCloseYyyySlashMm As String = ""      ' 締め日（文字列型："yyyyMM形"）

        Dim strTransferStatus As String = ""                ' 振込状況

        Try
            ' SQL作成
            strSql = ""
            strSql += "SELECT cru.c_user_id                               AS 社員番号"                  ' 01. 社員番号
            strSql += "      ,sa3.l_name                                  AS 氏名"                      ' 02. 氏名
            strSql += "      ,cru.d_years                                 AS 日付"                      ' 03. 日付
            strSql += "      ,cru.s_daily_pay_total                       AS 月日当計"                  ' 04. 月日当計
            strSql += "      ,cru.s_food_expenses_total                   AS 月昼食費計"                ' 05. 月昼食費計
            strSql += "      ,cru.s_next_balance_daily_pay_total          AS 前回差分月日当計"          ' 06. 前回差分月日当計
            strSql += "      ,cru.s_next_balance_food_expenses_total      AS 前回差分昼食費計"          ' 07. 前回差分昼食費計
            '                 08. 日当計 (月日当計 + 月昼食費計 + 前回差分月日当計 + 前回差分昼食費計)
            strSql += "      ,(cru.s_daily_pay_total"
            strSql += "      + cru.s_food_expenses_total"
            strSql += "      + cru.s_next_balance_daily_pay_total"
            strSql += "      + cru.s_next_balance_food_expenses_total)    AS 日当計"
            strSql += "      ,cru.k_daily_pay_kind                        AS 日当計算区分"              ' 09. 日当計算区分
            strSql += "      ,cd2.l_name                                  AS 日当計算区分名称"          ' 10. 日当計算区分名称
            strSql += "      ,cru.d_daily_pay_close                       AS 締め日"                    ' 11. 締め日
            strSql += "  FROM ("
            strSql += "        ("
            strSql += "         call_roll_user cru"                                                     ' /* 組合活動状況の親テーブル */

            '                   組合員属性情報
            strSql += "         INNER JOIN ("
            strSql += "             SELECT sa1.c_user_id"                                               ' 社員番号
            strSql += "                   ,sa1.l_name"                                                  ' 氏名
            strSql += "               FROM staf_attribute AS sa1"                                       ' /* 組合員属性テーブル */
            strSql += "                   ,("
            '                               終了日以下で最新のものを取得
            strSql += "                     SELECT sa1.c_user_id"                                       ' 社員番号
            strSql += "                           ,MAX(sa1.d_from) AS MAX_dfrom"                        ' 最大対象年月
            strSql += "                       FROM staf_attribute sa1"                                  ' /* 組合員属性テーブル */
            strSql += "                      WHERE sa1.d_from <= '" & dtTo.ToString("yyyyMMdd") & "'"   ' 対象年月が終了日よりも小さいもの
            strSql += "                      GROUP BY sa1.c_user_id"                                    ' 社員番号で集約
            strSql += "                    ) sa2"
            strSql += "              WHERE sa1.c_user_id = sa2.c_user_id"                               ' 社員番号が同じもの
            strSql += "                AND sa1.d_from    = sa2.MAX_dfrom"                               ' 最大対象年月と対象年月が同じもの
            strSql += "         ) sa3"
            strSql += "         ON cru.c_user_id = sa3.c_user_id"                                       ' 社員番号で結合
            strSql += "        )"

            '                   日当計算区分情報
            strSql += "         INNER JOIN ("
            strSql += "             SELECT cd1.c_constant_seq"                                          ' 定数ＩＤ枝番
            strSql += "                   ,cd1.l_name"                                                  ' 名称（日当計算区分名称）
            strSql += "               FROM constant_dtl cd1"                                            ' /* 定数マスタ詳細テーブル */
            strSql += "              WHERE cd1.c_constant = 'DAILY_PAY_KIND'"                           ' 定数ＩＤが 'DAILY_PAY_KIND' のもの
            strSql += "         ) cd2"
            strSql += "         ON cru.k_daily_pay_kind = cd2.c_constant_seq"                           ' 日当計算区分と定数ＩＤ枝番で結合
            strSql += "       )"

            strSql += " WHERE cru.d_years >= CONVERT(DATE,'" & dtFrom & "')"                               ' 対象年月が開始日より大きいもの
            strSql += "   AND cru.d_years <= CONVERT(DATE,'" & dtTo & "')"                                 ' 対象年月が終了日より小さいもの

            '           社員番号, 対象年月で並び替え
            strSql += " ORDER BY cru.c_user_id"
            strSql += "         ,cru.d_years"
            strSql += ";"

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtOutput = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = dtOutput.Rows.Count

            ' 件数チェック
            If intRetCnt > 0 Then

                '-----------------------------------------------------------------------------------
                '   CSVデータ用データテーブル定義
                '-----------------------------------------------------------------------------------
                dtResult = New DataTable("CsvData")
                dtResult.Columns.Add("社員番号")                    ' 01. 社員番号
                dtResult.Columns.Add("氏名")                        ' 02. 氏名
                dtResult.Columns.Add("日付")                        ' 03. 日付
                dtResult.Columns.Add("月日当計")                    ' 04. 月日当計
                dtResult.Columns.Add("月昼食費計")                  ' 05. 月昼食費計
                dtResult.Columns.Add("前回差分月日当計")            ' 06. 前回差分月日当計
                dtResult.Columns.Add("前回差分昼食費計")            ' 07. 前回差分昼食費計
                dtResult.Columns.Add("日当計")                      ' 08. 日当計
                dtResult.Columns.Add("日当計算区分")                ' 09. 日当計算区分（名称）
                dtResult.Columns.Add("締め日")                      ' 10. 締め日
                dtResult.Columns.Add("振込状況")                    ' 11. 振込状況

                ' 件数分ループ
                For i As Integer = 0 To dtOutput.Rows.Count - 1 Step 1

                    '-------------------------------------------------------------------------------
                    '   データ取得
                    '-------------------------------------------------------------------------------
                    strUserId = dtOutput.Rows(i).Item("社員番号").ToString()                                ' 社員番号
                    strName = dtOutput.Rows(i).Item("氏名").ToString()                                      ' 氏名
                    strYears = dtOutput.Rows(i).Item("日付").ToString()                                     ' 日付（文字列型）
                    dtYears = dtOutput.Rows(i).Item("日付")                                                 ' 日付（日付型）
                    strDailyPayTotal = dtOutput.Rows(i).Item("月日当計").ToString()                         ' 月日当計
                    strFoodExpensesTotal = dtOutput.Rows(i).Item("月昼食費計").ToString()                   ' 月昼食費計
                    strNextBalanceDailyPayTotal = dtOutput.Rows(i).Item("前回差分月日当計").ToString()      ' 前回差分月日当計
                    strNextBalanceFoodExpensesTotal = dtOutput.Rows(i).Item("前回差分昼食費計").ToString()  ' 前回差分昼食費計
                    strTotal = dtOutput.Rows(i).Item("日当計").ToString()                                   ' 日当計
                    strDailyPayKind = dtOutput.Rows(i).Item("日当計算区分").ToString()                      ' 日当計算区分
                    strDailyPayKindName = dtOutput.Rows(i).Item("日当計算区分名称").ToString()              ' 日当計算区分名称
                    strDailyPayClose = dtOutput.Rows(i).Item("締め日").ToString()                           ' 締め日（文字列型）
                    dtDailyPayClose = dtOutput.Rows(i).Item("締め日")                                       ' 締め日（日付型）

                    ' とりあえず、"未" を設定しておく。下記チェックですべてOKの場合、"済" を設定する。
                    strTransferStatus = "未"                                                                ' 振込状況

                    ' 日当計が 0 円以外
                    If strTotal <> "0" Then

                        '---------------------------------------------------------------------------
                        '   データロー作成
                        '---------------------------------------------------------------------------
                        rowResult = dtResult.NewRow
                        rowResult("社員番号") = strUserId                                           ' 01. 社員番号
                        rowResult("氏名") = strName                                                 ' 02. 氏名
                        rowResult("日付") = Format(dtYears, "yyyy/MM/dd")                           ' 03. 日付
                        rowResult("月日当計") = strDailyPayTotal                                    ' 04. 月日当計
                        rowResult("月昼食費計") = strFoodExpensesTotal                              ' 05. 月昼食費計
                        rowResult("前回差分月日当計") = strNextBalanceDailyPayTotal                 ' 06. 前回差分月日当計
                        rowResult("前回差分昼食費計") = strNextBalanceFoodExpensesTotal             ' 07. 前回差分昼食費計
                        rowResult("日当計") = strTotal                                              ' 08. 日当計
                        rowResult("日当計算区分") = strDailyPayKindName                             ' 09. 日当計算区分
                        rowResult("締め日") = Format(dtDailyPayClose, "yyyy/MM/dd")                 ' 10. 締め日

                        ' 締め日を "yyyyMM形" 式で取得
                        'strDailyPayCloseYyyySlashMm = Format(dtOutput.Rows(i).Item("締め日"), "yyyy/MM")
                        strDailyPayCloseYyyySlashMm = Format(dtDailyPayClose, "yyyy/MM")

                        '---------------------------------------------------------------------------
                        '   振込状況チェック
                        '---------------------------------------------------------------------------
                        ' 過去データ 2020/10/01以前は、"済" にする
                        Dim dtPrev As DateTime = New DateTime(2020, 10, 1)
                        If dtYears <= dtPrev Then
                            strTransferStatus = "済"
                        Else
                            ' 部／委員会日当の場合、3ヶ月分の日当合計金額で振込状況をチェックする
                            If strDailyPayKind = "01" Then

                                '===================================================================
                                '   日当金額合計取得処理
                                '===================================================================
                                str3Total = GetDailyPayTotal( _
                                    clsDb, _
                                    dtYears, _
                                    strUserId _
                                )

                                '===================================================================
                                '   振込チェック処理①
                                '===================================================================
                                ' 対象年月3ヶ月分の日当合計金額でチェック
                                If CheckTransfer1( _
                                    clsDb, _
                                    strDailyPayKind, _
                                    strDailyPayCloseYyyySlashMm, _
                                    strUserId, _
                                    str3Total _
                                ) = True Then
                                    strTransferStatus = "済"
                                Else
                                    '===============================================================
                                    '   振込チェック処理②
                                    '===============================================================
                                    ' 対象年月3ヶ月分の日当合計金額をあいまい検索でチェック
                                    If CheckTransfer2( _
                                        clsDb, _
                                        strDailyPayKind, _
                                        strDailyPayCloseYyyySlashMm, _
                                        strUserId, _
                                        str3Total _
                                    ) = True Then
                                        strTransferStatus = "済"
                                    Else
                                        '===========================================================
                                        '   振込チェック処理③
                                        '===========================================================
                                        ' 対象年月の日当合計金額でチェック
                                        If CheckTransfer3( _
                                            clsDb, _
                                            strDailyPayKind, _
                                            strDailyPayCloseYyyySlashMm, _
                                            strUserId, _
                                            strTotal _
                                        ) = True Then
                                            strTransferStatus = "済"
                                        Else
                                            '=======================================================
                                            '   振込チェック処理④
                                            '=======================================================
                                            ' マイナス繰越、前対象年月
                                            If CheckTransfer4( _
                                                clsDb, _
                                                strDailyPayKind, _
                                                strDailyPayCloseYyyySlashMm, _
                                                strUserId _
                                            ) = True Then
                                                strTransferStatus = "済"
                                            End If
                                        End If
                                    End If
                                End If

                            Else
                                '===================================================================
                                '   振込チェック処理①
                                '===================================================================
                                If CheckTransfer1( _
                                    clsDb, _
                                    strDailyPayKind, _
                                    strDailyPayCloseYyyySlashMm, _
                                    strUserId, _
                                    strTotal _
                                ) = True Then
                                    strTransferStatus = "済"
                                End If
                            End If
                        End If

                        ' 12. 振込状況
                        rowResult("振込状況") = strTransferStatus

                        ' データロー追加
                        dtResult.Rows.Add(rowResult)
                    End If
                Next i
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' データベース切断
            Call clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return dtResult

    End Function
#End Region

#Region " 旧）CSV出力データ作成処理（月単位） "
    '***************************************************************************************************
    '   ＩＤ　：SetMonthlyCsvDataOld
    '   名称　：CSV出力データ作成処理（月単位）
    '   概要　：CSV出力データを作成し返却します
    '   引数　：ByVal dtFrom As DateTime = 開始日
    '           ByVal dtTo   As DateTime = 終了日
    '   戻り値：CSVデータ
    '   作成日：2021/10/18(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>CSV出力データ作成処理（月単位）</summary>
    ''' <param name="dtFrom">開始日</param>
    ''' <param name="dtTo">終了日</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetMonthlyCsvDataOld( _
        ByVal dtFrom As DateTime, _
        ByVal dtTo As DateTime _
    ) As DataTable

        Dim dtOutput As DataTable = New DataTable           ' 日当データ基本情報
        Dim strSql As String = ""                           ' SQL文
        Dim clsDb As New CLAccessMdb                        ' データベースクラス
        Dim intRetCnt As Integer = 0                        ' 検索結果件数

        Dim dtResult As DataTable = Nothing                 ' CSVデータ用データテーブル
        Dim rowResult As DataRow = Nothing                  ' CSVデータ用データロー

        ' 取得データ
        Dim strUserId As String = ""                        ' 社員番号
        Dim strName As String = ""                          ' 氏名
        Dim strYears As String = ""                         ' 日付（文字列型）
        Dim dtYears As DateTime = Nothing                   ' 日付（日付型）
        Dim strDailyPayTotal As String = ""                 ' 月日当計
        Dim strFoodExpensesTotal As String = ""             ' 月昼食費計
        Dim strNextBalanceDailyPayTotal As String = ""      ' 前回差分月日当計
        Dim strNextBalanceFoodExpensesTotal As String = ""  ' 前回差分昼食費計
        Dim strTotal As String = ""                         ' 日当計
        Dim str3Total As String = ""                        ' 日当計（対象年月の3ヶ月）
        Dim strDailyPayKind As String = ""                  ' 日当計算区分
        Dim strDailyPayKindName As String = ""              ' 日当計算区分名称
        Dim strDailyPayClose As String = ""                 ' 締め日（文字列型）
        Dim dtDailyPayClose As DateTime = Nothing           ' 締め日（日付型）
        Dim strDailyPayCloseYyyySlashMm As String = ""      ' 締め日（文字列型："yyyyMM形"）

        Dim strTransferStatus As String = ""                ' 振込状況

        Try
            ' SQL作成
            strSql = ""
            strSql += "SELECT cru.c_user_id                               AS 社員番号"                  ' 01. 社員番号
            strSql += "      ,sa3.l_name                                  AS 氏名"                      ' 02. 氏名
            strSql += "      ,cru.d_years                                 AS 日付"                      ' 03. 日付
            strSql += "      ,cru.s_daily_pay_total                       AS 月日当計"                  ' 04. 月日当計
            strSql += "      ,cru.s_food_expenses_total                   AS 月昼食費計"                ' 05. 月昼食費計
            strSql += "      ,cru.s_next_balance_daily_pay_total          AS 前回差分月日当計"          ' 06. 前回差分月日当計
            strSql += "      ,cru.s_next_balance_food_expenses_total      AS 前回差分昼食費計"          ' 07. 前回差分昼食費計
            '                 08. 日当計 (月日当計 + 月昼食費計 + 前回差分月日当計 + 前回差分昼食費計)
            strSql += "      ,(cru.s_daily_pay_total"
            strSql += "      + cru.s_food_expenses_total"
            strSql += "      + cru.s_next_balance_daily_pay_total"
            strSql += "      + cru.s_next_balance_food_expenses_total)    AS 日当計"
            strSql += "      ,cru.k_daily_pay_kind                        AS 日当計算区分"              ' 09. 日当計算区分
            strSql += "      ,cd2.l_name                                  AS 日当計算区分名称"          ' 10. 日当計算区分名称
            strSql += "      ,cru.d_daily_pay_close                       AS 締め日"                    ' 11. 締め日
            strSql += "  FROM ("
            strSql += "        ("
            strSql += "         call_roll_user cru"                                                     ' /* 組合活動状況の親テーブル */

            '                   組合員属性情報
            strSql += "         INNER JOIN ("
            strSql += "             SELECT sa1.c_user_id"                                               ' 社員番号
            strSql += "                   ,sa1.l_name"                                                  ' 氏名
            strSql += "               FROM staf_attribute AS sa1"                                       ' /* 組合員属性テーブル */
            strSql += "                   ,("
            '                               終了日以下で最新のものを取得
            strSql += "                     SELECT sa1.c_user_id"                                       ' 社員番号
            strSql += "                           ,MAX(sa1.d_from) AS MAX_dfrom"                        ' 最大対象年月
            strSql += "                       FROM staf_attribute sa1"                                  ' /* 組合員属性テーブル */
            strSql += "                      WHERE sa1.d_from <= '" & dtTo.ToString("yyyyMMdd") & "'"   ' 対象年月が終了日よりも小さいもの
            strSql += "                      GROUP BY sa1.c_user_id"                                    ' 社員番号で集約
            strSql += "                    ) sa2"
            strSql += "              WHERE sa1.c_user_id = sa2.c_user_id"                               ' 社員番号が同じもの
            strSql += "                AND sa1.d_from    = sa2.MAX_dfrom"                               ' 最大対象年月と対象年月が同じもの
            strSql += "         ) sa3"
            strSql += "         ON cru.c_user_id = sa3.c_user_id"                                       ' 社員番号で結合
            strSql += "        )"

            '                   日当計算区分情報
            strSql += "         INNER JOIN ("
            strSql += "             SELECT cd1.c_constant_seq"                                          ' 定数ＩＤ枝番
            strSql += "                   ,cd1.l_name"                                                  ' 名称（日当計算区分名称）
            strSql += "               FROM constant_dtl cd1"                                            ' /* 定数マスタ詳細テーブル */
            strSql += "              WHERE cd1.c_constant = 'DAILY_PAY_KIND'"                           ' 定数ＩＤが 'DAILY_PAY_KIND' のもの
            strSql += "         ) cd2"
            strSql += "         ON cru.k_daily_pay_kind = cd2.c_constant_seq"                           ' 日当計算区分と定数ＩＤ枝番で結合
            strSql += "       )"

            strSql += " WHERE cru.d_years >= CONVERT(DATE,'" & dtFrom & "')"                               ' 対象年月が開始日より大きいもの
            strSql += "   AND cru.d_years <= CONVERT(DATE,'" & dtTo & "')"                                 ' 対象年月が終了日より小さいもの

            '           社員番号, 対象年月で並び替え
            strSql += " ORDER BY cru.c_user_id"
            strSql += "         ,cru.d_years"
            strSql += ";"

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtOutput = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = dtOutput.Rows.Count

            ' 件数チェック
            If intRetCnt > 0 Then

                '-----------------------------------------------------------------------------------
                '   CSVデータ用データテーブル定義
                '-----------------------------------------------------------------------------------
                dtResult = New DataTable("CsvData")
                dtResult.Columns.Add("社員番号")                    ' 01. 社員番号
                dtResult.Columns.Add("氏名")                        ' 02. 氏名
                dtResult.Columns.Add("日付")                        ' 03. 日付
                dtResult.Columns.Add("月日当計")                    ' 04. 月日当計
                dtResult.Columns.Add("月昼食費計")                  ' 05. 月昼食費計
                dtResult.Columns.Add("前回差分月日当計")            ' 06. 前回差分月日当計
                dtResult.Columns.Add("前回差分昼食費計")            ' 07. 前回差分昼食費計
                dtResult.Columns.Add("日当計")                      ' 08. 日当計
                dtResult.Columns.Add("日当計算区分")                ' 09. 日当計算区分（名称）
                dtResult.Columns.Add("締め日")                      ' 10. 締め日
                dtResult.Columns.Add("振込状況")                    ' 11. 振込状況

                ' 件数分ループ
                For i As Integer = 0 To dtOutput.Rows.Count - 1 Step 1

                    '-------------------------------------------------------------------------------
                    '   データ取得
                    '-------------------------------------------------------------------------------
                    strUserId = dtOutput.Rows(i).Item("社員番号").ToString()                                ' 社員番号
                    strName = dtOutput.Rows(i).Item("氏名").ToString()                                      ' 氏名
                    strYears = dtOutput.Rows(i).Item("日付").ToString()                                     ' 日付（文字列型）
                    dtYears = dtOutput.Rows(i).Item("日付")                                                 ' 日付（日付型）
                    strDailyPayTotal = dtOutput.Rows(i).Item("月日当計").ToString()                         ' 月日当計
                    strFoodExpensesTotal = dtOutput.Rows(i).Item("月昼食費計").ToString()                   ' 月昼食費計
                    strNextBalanceDailyPayTotal = dtOutput.Rows(i).Item("前回差分月日当計").ToString()      ' 前回差分月日当計
                    strNextBalanceFoodExpensesTotal = dtOutput.Rows(i).Item("前回差分昼食費計").ToString()  ' 前回差分昼食費計
                    strTotal = dtOutput.Rows(i).Item("日当計").ToString()                                   ' 日当計
                    strDailyPayKind = dtOutput.Rows(i).Item("日当計算区分").ToString()                      ' 日当計算区分
                    strDailyPayKindName = dtOutput.Rows(i).Item("日当計算区分名称").ToString()              ' 日当計算区分名称
                    strDailyPayClose = dtOutput.Rows(i).Item("締め日").ToString()                           ' 締め日（文字列型）
                    dtDailyPayClose = dtOutput.Rows(i).Item("締め日")                                       ' 締め日（日付型）

                    ' とりあえず、"未" を設定しておく。下記チェックですべてOKの場合、"済" を設定する。
                    strTransferStatus = "未"                                                                ' 振込状況

                    ' 日当計が 0 円以外
                    If strTotal <> "0" Then

                        '---------------------------------------------------------------------------
                        '   データロー作成
                        '---------------------------------------------------------------------------
                        rowResult = dtResult.NewRow
                        rowResult("社員番号") = strUserId                                           ' 01. 社員番号
                        rowResult("氏名") = strName                                                 ' 02. 氏名
                        rowResult("日付") = Format(dtYears, "yyyy/MM/dd")                           ' 03. 日付
                        rowResult("月日当計") = strDailyPayTotal                                    ' 04. 月日当計
                        rowResult("月昼食費計") = strFoodExpensesTotal                              ' 05. 月昼食費計
                        rowResult("前回差分月日当計") = strNextBalanceDailyPayTotal                 ' 06. 前回差分月日当計
                        rowResult("前回差分昼食費計") = strNextBalanceFoodExpensesTotal             ' 07. 前回差分昼食費計
                        rowResult("日当計") = strTotal                                              ' 08. 日当計
                        rowResult("日当計算区分") = strDailyPayKindName                             ' 09. 日当計算区分
                        rowResult("締め日") = Format(dtDailyPayClose, "yyyy/MM/dd")                 ' 10. 締め日

                        ' 締め日を "yyyyMM形" 式で取得
                        strDailyPayCloseYyyySlashMm = Format(dtOutput.Rows(i).Item("締め日"), "yyyy/MM")

                        '---------------------------------------------------------------------------
                        '   振込状況チェック
                        '---------------------------------------------------------------------------
                        ' 部／委員会日当の場合、3ヶ月分の日当合計金額で振込状況をチェックする
                        If strDailyPayKind = "01" Then

                            '=======================================================================
                            '   日当金額合計取得処理
                            '=======================================================================
                            str3Total = GetDailyPayTotal( _
                                clsDb, _
                                dtYears, _
                                strUserId _
                            )

                            ' 日当金額合計がマイナス値は、"済"にする
                            If str3Total < 0 Then
                                strTransferStatus = "済"
                            Else
                                '===================================================================
                                '   振込チェック処理①
                                '===================================================================
                                ' 対象年月3ヶ月分の日当合計金額でチェック
                                If CheckTransfer1( _
                                    clsDb, _
                                    strDailyPayKind, _
                                    strDailyPayCloseYyyySlashMm, _
                                    strUserId, _
                                    str3Total _
                                ) = True Then
                                    strTransferStatus = "済"
                                Else
                                    '===============================================================
                                    '   振込チェック処理②
                                    '===============================================================
                                    ' 対象年月3ヶ月分の日当合計金額をあいまい検索でチェック
                                    If CheckTransfer2( _
                                        clsDb, _
                                        strDailyPayKind, _
                                        strDailyPayCloseYyyySlashMm, _
                                        strUserId, _
                                        str3Total _
                                    ) = True Then
                                        strTransferStatus = "済"
                                    Else
                                        '===========================================================
                                        '   振込チェック処理③
                                        '===========================================================
                                        ' 対象年月の日当合計金額でチェック
                                        If CheckTransfer3( _
                                            clsDb, _
                                            strDailyPayKind, _
                                            strDailyPayCloseYyyySlashMm, _
                                            strUserId, _
                                            strTotal _
                                        ) = True Then
                                            strTransferStatus = "済"
                                        Else
                                            '=======================================================
                                            '   振込チェック処理④
                                            '=======================================================
                                            ' マイナス繰越、前対象年月
                                            If CheckTransfer4( _
                                                clsDb, _
                                                strDailyPayKind, _
                                                strDailyPayCloseYyyySlashMm, _
                                                strUserId _
                                            ) = True Then
                                                strTransferStatus = "済"
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Else
                            '=======================================================================
                            '   振込チェック処理①
                            '=======================================================================
                            If CheckTransfer1( _
                                clsDb, _
                                strDailyPayKind, _
                                strDailyPayCloseYyyySlashMm, _
                                strUserId, _
                                strTotal _
                            ) = True Then
                                strTransferStatus = "済"
                            End If
                        End If

                        ' 12. 振込状況
                        rowResult("振込状況") = strTransferStatus

                        ' データロー追加
                        dtResult.Rows.Add(rowResult)
                    End If
                Next i
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        Finally
            ' データベース切断
            Call clsDb.Disconnect()

        End Try

        ' 戻り値設定
        Return dtResult

    End Function
#End Region

#Region " 日当金額合計取得処理 "
    '***************************************************************************************************
    '   ＩＤ　：GetDailyPayTotal
    '   名称　：日当金額合計取得処理
    '   概要　：振込済みかチェックする。
    '   引数　：ByVal i_clsDb  As CLAccessMdb = データベースクラス,
    '           ByVal i_years  As String      = 対象年月（日付/時刻型）,
    '           ByVal i_userId As String      = 個人認証ID
    '   戻り値：Integer = 日当金額合計金額
    '   作成日：2021/10/26(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/26(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>日当金額合計取得処理</summary>
    ''' <param name="i_clsDb">データベースクラス</param>
    ''' <param name="i_years">対象年月（日付/時刻型）</param>
    ''' <param name="i_userId">個人認証ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDailyPayTotal( _
        ByVal i_clsDb As CLAccessMdb, _
        ByVal i_years As DateTime, _
        ByVal i_userId As String _
    ) As Integer

        Dim dtCru As DataTable = Nothing        ' 組合活動状況の親データ
        Dim strSql As String = ""               ' SQL文
        Dim dailyPayTotal As Integer = 0        ' 金額
        Dim dtFrom As DateTime = Nothing        ' 開始日
        Dim dtTo As DateTime = Nothing          ' 終了日
        Dim intMonth As Integer = 0             ' 対象年月の月

        Try
            '===========================================================================
            '   開始日終了日取得処理
            '===========================================================================
            Call GetTargetDateFromTo( _
                1, _
                i_clsDb, _
                i_years, _
                i_userId, _
                dtFrom, _
                dtTo _
            )

            ' 日当振込金額取得
            strSql = ""
            strSql += "SELECT SUM(cru.s_daily_pay_total"
            strSql += "         + cru.s_food_expenses_total"
            strSql += "         + cru.s_next_balance_daily_pay_total"
            strSql += "         + cru.s_next_balance_food_expenses_total) AS 日当計"
            strSql += "  FROM call_roll_user cru"                                       ' /* 組合活動状況の親テーブル */
            strSql += " WHERE cru.k_daily_pay_kind  = '01'"                             ' 日当計算区分が '01'：部／委員会日当のもの
            strSql += "   AND cru.d_years          >= CONVERT(DATE,'" & dtFrom & "')"      ' 対象年月が開始日より大きいもの
            strSql += "   AND cru.d_years          <= CONVERT(DATE,'" & dtTo & "')"        ' 対象年月が終了日より小さいもの
            strSql += "   AND cru.c_user_id         = '" & i_userId & "'"               ' 個人認証IDが同じもの

            ' SQL実行
            dtCru = i_clsDb.ExecuteSql(strSql)

            ' データ有無判定
            If dtCru.Rows.Count = 1 Then
                ' 日当合計金額取得
                If Not IsDBNull(dtCru.Rows(0).Item("日当計")) Then
                    dailyPayTotal = CInt(dtCru.Rows(0).Item("日当計"))
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return dailyPayTotal

    End Function
#End Region

#Region " 振込チェック処理① "
    '***************************************************************************************************
    '   ＩＤ　：CheckTransfer1
    '   名称　：振込チェック①処理
    '   概要　：日当計と振込金額が一致するかチェックする。
    '   引数　：ByVal i_clsDb         As CLAccessMdb = データベースクラス,
    '           ByVal i_dailyPayKind  As String      = 締め日種別（区分値2桁（DAILY_PAY_KIND））,
    '           ByVal i_dailyPayClose As String      = 締め日付（スラッシュあり年月7桁（yyyy/mm））,
    '           ByVal i_userId        As String      = 個人認証ID,
    '           ByVal i_dailyPayTotal As Integer     = 日当計
    '   戻り値：True：振込済み, False：未振込
    '   作成日：2021/10/18(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>振込チェック①処理</summary>
    ''' <param name="i_clsDb">データベースクラス</param>
    ''' <param name="i_dailyPayKind">締め日種別（区分値2桁（DAILY_PAY_KIND））</param>
    ''' <param name="i_dailyPayClose">締め日付（スラッシュあり年月7桁（yyyy/mm））</param>
    ''' <param name="i_userId">個人認証ID</param>
    ''' <param name="i_dailyPayTotal">日当計</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckTransfer1( _
        ByVal i_clsDb As CLAccessMdb, _
        ByVal i_dailyPayKind As String, _
        ByVal i_dailyPayClose As String, _
        ByVal i_userId As String, _
        ByVal i_dailyPayTotal As Integer _
    ) As Boolean

        Dim blnResult As Boolean = False        ' True：振込済, False：未振込
        Dim dtTrasfer As DataTable = Nothing    ' 振込データ詳細
        Dim strSql As String = ""               ' SQL文
        Dim dailyPayTotalSum As Integer = 0     ' 金額

        Try
            ' 振込金額取得
            strSql = ""
            strSql += "SELECT SUM(sbsm.s_daily_pay_total) AS dailyPayTotalSum"
            strSql += "  FROM staf_bank_send sbs"                                       ' /* 振込データ */
            strSql += "       INNER JOIN staf_bank_send_member sbsm"                    ' /* 振込データ詳細 */
            strSql += "       ON  sbs.c_staf_bank_send_id = sbsm.c_staf_bank_send_id"   ' 振込番号IDが同じもの
            strSql += "       AND sbs.d_bank_send         = sbsm.d_bank_send"           ' 振込日付が同じもの
            strSql += " WHERE sbs.c_daily_pay_kind  = '" & i_dailyPayKind & "'"         ' 日当対象締め日種別が同じもの
            strSql += "   AND sbs.c_daily_pay_close = '" & i_dailyPayClose & "'"        ' 日当対象締め日が同じもの
            strSql += "   AND sbsm.c_user_id        = '" & i_userId & "'"               ' 個人認証IDが同じもの

            ' SQL実行
            dtTrasfer = i_clsDb.ExecuteSql(strSql)

            ' データ有無判定
            If dtTrasfer.Rows.Count = 1 Then

                ' 振込金額取得
                If Not IsDBNull(dtTrasfer.Rows(0).Item("dailyPayTotalSum")) Then
                    dailyPayTotalSum = CInt(dtTrasfer.Rows(0).Item("dailyPayTotalSum"))
                    ' 振込金額と日当計が同じなら振込済
                    If i_dailyPayTotal = dailyPayTotalSum Then
                        blnResult = True
                    End If
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnResult

    End Function
#End Region

#Region " 振込チェック処理② "
    '***************************************************************************************************
    '   ＩＤ　：CheckTransfer2
    '   名称　：振込チェック②処理
    '   概要　：振込金額が日当計以上かチェックする。
    '   引数　：ByVal i_clsDb         As CLAccessMdb = データベースクラス,
    '           ByVal i_dailyPayKind  As String      = 締め日種別（区分値2桁（DAILY_PAY_KIND））,
    '           ByVal i_dailyPayClose As String      = 締め日付（スラッシュあり年月7桁（yyyy/mm））,
    '           ByVal i_userId        As String      = 個人認証ID,
    '           ByVal i_dailyPayTotal As Integer     = 日当計
    '   戻り値：True：振込済み, False：未振込
    '   作成日：2021/10/18(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>振込チェック②処理</summary>
    ''' <param name="i_clsDb">データベースクラス</param>
    ''' <param name="i_dailyPayKind">締め日種別（区分値2桁（DAILY_PAY_KIND））</param>
    ''' <param name="i_dailyPayClose">締め日付（スラッシュあり年月7桁（yyyy/mm））</param>
    ''' <param name="i_userId">個人認証ID</param>
    ''' <param name="i_dailyPayTotal">日当計</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckTransfer2( _
        ByVal i_clsDb As CLAccessMdb, _
        ByVal i_dailyPayKind As String, _
        ByVal i_dailyPayClose As String, _
        ByVal i_userId As String, _
        ByVal i_dailyPayTotal As Integer _
    ) As Boolean

        Dim blnResult As Boolean = False        ' True：振込済, False：未振込
        Dim dtTrasfer As DataTable = Nothing    ' 振込データ詳細
        Dim strSql As String = ""               ' SQL文
        Dim dailyPayTotalSum As Integer = 0     ' 金額

        Try
            ' 振込金額取得
            strSql = ""
            strSql += "SELECT SUM(sbsm.s_daily_pay_total) AS dailyPayTotalSum"
            strSql += "  FROM staf_bank_send sbs"                                       ' /* 振込データ */
            strSql += "       INNER JOIN staf_bank_send_member sbsm"                    ' /* 振込データ詳細 */
            strSql += "       ON  sbs.c_staf_bank_send_id = sbsm.c_staf_bank_send_id"   ' 振込番号IDが同じもの
            strSql += "       AND sbs.d_bank_send         = sbsm.d_bank_send"           ' 振込日付が同じもの
            strSql += " WHERE sbs.c_daily_pay_kind  LIKE '%" & i_dailyPayKind & "%'"    ' 日当対象締め日種別を含むもの
            strSql += "   AND sbs.c_daily_pay_close LIKE '%" & i_dailyPayClose & "%'"   ' 日当対象締め日を含むもの
            strSql += "   AND sbsm.c_user_id        = '" & i_userId & "'"               ' 個人認証IDが同じもの

            ' SQL実行
            dtTrasfer = i_clsDb.ExecuteSql(strSql)

            ' データ有無判定
            If dtTrasfer.Rows.Count = 1 Then

                ' 振込金額取得
                If Not IsDBNull(dtTrasfer.Rows(0).Item("dailyPayTotalSum")) Then
                    dailyPayTotalSum = CInt(dtTrasfer.Rows(0).Item("dailyPayTotalSum"))
                    ' 振込金額が日当計以上なら振込済
                    If dailyPayTotalSum >= i_dailyPayTotal Then
                        blnResult = True
                    End If
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnResult

    End Function
#End Region

#Region " 振込チェック処理③ "
    '***************************************************************************************************
    '   ＩＤ　：CheckTransfer3
    '   名称　：振込チェック③処理
    '   概要　：振込金額が日当計以上かチェックする。
    '   引数　：ByVal i_clsDb         As CLAccessMdb = データベースクラス,
    '           ByVal i_dailyPayKind  As String      = 締め日種別（区分値2桁（DAILY_PAY_KIND））,
    '           ByVal i_dailyPayClose As String      = 締め日付（スラッシュあり年月7桁（yyyy/mm））,
    '           ByVal i_userId        As String      = 個人認証ID,
    '           ByVal i_dailyPayTotal As Integer     = 日当計
    '   戻り値：True：振込済み, False：未振込
    '   作成日：2021/10/18(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>振込チェック③処理</summary>
    ''' <param name="i_clsDb">データベースクラス</param>
    ''' <param name="i_dailyPayKind">締め日種別（区分値2桁（DAILY_PAY_KIND））</param>
    ''' <param name="i_dailyPayClose">締め日付（スラッシュあり年月7桁（yyyy/mm））</param>
    ''' <param name="i_userId">個人認証ID</param>
    ''' <param name="i_dailyPayTotal">日当計</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckTransfer3( _
        ByVal i_clsDb As CLAccessMdb, _
        ByVal i_dailyPayKind As String, _
        ByVal i_dailyPayClose As String, _
        ByVal i_userId As String, _
        ByVal i_dailyPayTotal As Integer _
    ) As Boolean

        Dim blnResult As Boolean = False        ' True：振込済, False：未振込
        Dim dtTrasfer As DataTable = Nothing    ' 振込データ詳細
        Dim strSql As String = ""               ' SQL文
        Dim dailyPayTotalSum As Integer = 0     ' 金額

        Try
            ' 振込金額取得
            strSql = ""
            strSql += "SELECT SUM(sbsm.s_daily_pay_total) AS dailyPayTotalSum"
            strSql += "  FROM staf_bank_send sbs"                                       ' /* 振込データ */
            strSql += "       INNER JOIN staf_bank_send_member sbsm"                    ' /* 振込データ詳細 */
            strSql += "       ON  sbs.c_staf_bank_send_id = sbsm.c_staf_bank_send_id"   ' 振込番号IDが同じもの
            strSql += "       AND sbs.d_bank_send         = sbsm.d_bank_send"           ' 振込日付が同じもの
            strSql += " WHERE sbs.c_daily_pay_kind  LIKE '%" & i_dailyPayKind & "%'"    ' 日当対象締め日種別を含むもの
            strSql += "   AND sbs.c_daily_pay_close LIKE '%" & i_dailyPayClose & "%'"   ' 日当対象締め日を含むもの
            strSql += "   AND sbsm.c_user_id        = '" & i_userId & "'"               ' 個人認証IDが同じもの

            ' SQL実行
            dtTrasfer = i_clsDb.ExecuteSql(strSql)

            ' データ有無判定
            If dtTrasfer.Rows.Count = 1 Then

                ' 振込金額取得
                If Not IsDBNull(dtTrasfer.Rows(0).Item("dailyPayTotalSum")) Then
                    dailyPayTotalSum = CInt(dtTrasfer.Rows(0).Item("dailyPayTotalSum"))
                    ' 振込金額が日当計以上なら振込済
                    If dailyPayTotalSum >= i_dailyPayTotal Then
                        blnResult = True
                    End If
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnResult

    End Function
#End Region

#Region " 振込チェック処理④ "
    '***************************************************************************************************
    '   ＩＤ　：CheckTransfer4
    '   名称　：振込チェック④処理
    '   概要　：対象年月の日当対象締め日を取得
    '              1. レコードが有る場合
    '                 1.1. 過去の対象年月の合計日当金額取得
    '                 1.2. 対象年月の合計日当金額取得
    '                 1.2. 振込金額取得
    '                 1.3. 過去の対象年月の合計日当金額と対象年月の合計日当金額を足した金額が
    '                      振込金額が一致した場合、振込済
    '                      振込金額が一致しない場合、未振込
    '              2. レコードが有る場合
    '                 2.1. 過去の対象年月の合計日当金額取得
    '                 2.2. 過去の対象年月の合計日当金額がマイナスの場合、振込済
    '                      過去の対象年月の合計日当金額がマイナスではない場合、未振込
    '   引数　：ByVal i_clsDb         As CLAccessMdb = データベースクラス,
    '           ByVal i_dailyPayKind  As String      = 締め日種別（区分値2桁（DAILY_PAY_KIND））,
    '           ByVal i_dailyPayClose As String      = 締め日付（スラッシュあり年月7桁（yyyy/mm））,
    '           ByVal i_userId        As String      = 個人認証ID
    '   戻り値：True：存在する, False：存在しない
    '   作成日：2021/10/18(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/18(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>振込チェック④処理</summary>
    ''' <param name="i_clsDb">データベースクラス</param>
    ''' <param name="i_dailyPayKind">締め日種別（区分値2桁（DAILY_PAY_KIND））</param>
    ''' <param name="i_dailyPayClose">締め日付（スラッシュあり年月7桁（yyyy/mm））</param>
    ''' <param name="i_userId">個人認証ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckTransfer4( _
        ByVal i_clsDb As CLAccessMdb, _
        ByVal i_dailyPayKind As String, _
        ByVal i_dailyPayClose As String, _
        ByVal i_userId As String _
    ) As Boolean

        Dim blnResult As Boolean = False        ' True：振込済, False：未振込
        Dim dtTrasfer As DataTable = Nothing    ' 振込データ詳細
        Dim strSql As String = ""               ' SQL文
        Dim dailyPayCloseList() As String       ' 日当対象締め日リスト
        Dim dtFrom As DateTime = Nothing        ' 開始日
        Dim dtTo As DateTime = Nothing          ' 終了日
        Dim dtTargetDate As DateTime = Nothing  ' 対象年月（日付型）

        Try
            ' 日当対象締め日取得
            strSql = ""
            strSql += "SELECT sbs.c_daily_pay_close"
            strSql += "  FROM staf_bank_send sbs"                                       ' /* 振込データ */
            strSql += "       INNER JOIN staf_bank_send_member sbsm"                    ' /* 振込データ詳細 */
            strSql += "       ON  sbs.c_staf_bank_send_id = sbsm.c_staf_bank_send_id"   ' 振込番号IDが同じもの
            strSql += "       AND sbs.d_bank_send         = sbsm.d_bank_send"           ' 振込日付が同じもの
            strSql += " WHERE sbs.c_daily_pay_kind  LIKE '%" & i_dailyPayKind & "%'"    ' 日当対象締め日種別を含むもの
            strSql += "   AND sbs.c_daily_pay_close LIKE '%" & i_dailyPayClose & "%'"   ' 日当対象締め日を含むもの
            strSql += "   AND sbsm.c_user_id        = '" & i_userId & "'"               ' 個人認証IDが同じもの

            ' SQL実行
            dtTrasfer = i_clsDb.ExecuteSql(strSql)

            ' データ有無判定
            If dtTrasfer.Rows.Count = 1 Then

                '-----------------------------------------------------------------------------------
                '   データが有る場合
                '-----------------------------------------------------------------------------------
                ' 日当対象締め日取得
                If Not IsDBNull(dtTrasfer.Rows(0).Item("c_daily_pay_close")) Then

                    ' カンマ区切りで取得
                    dailyPayCloseList = dtTrasfer.Rows(0).Item("c_daily_pay_close").ToString.Split(",")

                    ' カンマ区切りで取得できたかチェック（相殺レコードがあるかチェック）
                    If UBound(dailyPayCloseList) = 1 Then

                        '---------------------------------------------------------------------------
                        '   取得した過去の対象年月の合計日当金額取得
                        '---------------------------------------------------------------------------
                        Dim prevPay As Integer = 0      ' 相殺レコードの合計日当金額
                        Dim nowPay As Integer = 0       ' 対象年月の合計日当金額取得
                        Dim payTotal As Integer = 0     ' 振込金額

                        ' 相殺レコードの対象年月を日付型で取得
                        dtTargetDate = dailyPayCloseList(1) & "/01 0:00:00"

                        '===================================
                        '   開始日終了日取得処理
                        '===================================
                        ' 相殺レコードの当対象年月の開始日終了日取得
                        Call GetTargetDateFromTo( _
                            1, _
                            i_clsDb, _
                            dtTargetDate, _
                            i_userId, _
                            dtFrom, _
                            dtTo _
                        )

                        ' 相殺レコードの当対象年月の合計日当金額取得
                        strSql = ""
                        strSql += "SELECT SUM(cru.s_daily_pay_total"
                        strSql += "         + cru.s_food_expenses_total"
                        strSql += "         + cru.s_next_balance_daily_pay_total"
                        strSql += "         + cru.s_next_balance_food_expenses_total) AS 日当計1"
                        strSql += "  FROM call_roll_user cru"                                       ' /* 組合活動状況の親テーブル */
                        strSql += " WHERE cru.k_daily_pay_kind  = '01'"                             ' 日当計算区分が '01'：部／委員会日当のもの
                        strSql += "   AND cru.d_years          >= CONVERT(DATE,'" & dtFrom & "')"      ' 対象年月が開始日より大きいもの
                        strSql += "   AND cru.d_years          <= CONVERT(DATE,'" & dtTo & "')"        ' 対象年月が終了日より小さいもの
                        strSql += "   AND cru.c_user_id         = '" & i_userId & "'"               ' 個人認証IDが同じもの

                        ' SQL実行
                        dtTrasfer = i_clsDb.ExecuteSql(strSql)

                        ' データ有無判定
                        If dtTrasfer.Rows.Count = 1 Then
                            ' 相殺レコードの当対象年月の合計日当金額取得
                            If Not IsDBNull(dtTrasfer.Rows(0).Item("日当計1")) Then
                                prevPay = CInt(dtTrasfer.Rows(0).Item("日当計1"))
                            End If
                        End If

                        '---------------------------------------------------------------------------
                        '   対象年月の合計日当金額取得
                        '---------------------------------------------------------------------------
                        ' 対象年月を日付型で取得
                        dtTargetDate = i_dailyPayClose & "/01 0:00:00"

                        '===================================
                        '   開始日終了日取得処理
                        '===================================
                        ' 当対象年月の開始日終了日取得
                        Call GetTargetDateFromTo( _
                            1, _
                            i_clsDb, _
                            dtTargetDate, _
                            i_userId, _
                            dtFrom, _
                            dtTo _
                        )

                        ' 対象年月の合計日当金額取得
                        strSql = ""
                        strSql += "SELECT SUM(cru.s_daily_pay_total"
                        strSql += "         + cru.s_food_expenses_total"
                        strSql += "         + cru.s_next_balance_daily_pay_total"
                        strSql += "         + cru.s_next_balance_food_expenses_total) AS 日当計2"
                        strSql += "  FROM call_roll_user cru"                                       ' /* 組合活動状況の親テーブル */
                        strSql += " WHERE cru.k_daily_pay_kind  = '01'"                             ' 日当計算区分が '01'：部／委員会日当のもの
                        strSql += "   AND cru.d_years          >= CONVERT(DATE,'" & dtFrom & "')"      ' 対象年月が開始日より大きいもの
                        strSql += "   AND cru.d_years          <= CONVERT(DATE,'" & dtTo & "')"        ' 対象年月が終了日より小さいもの
                        strSql += "   AND cru.c_user_id         = '" & i_userId & "'"               ' 個人認証IDが同じもの

                        ' SQL実行
                        dtTrasfer = i_clsDb.ExecuteSql(strSql)

                        ' データ有無判定
                        If dtTrasfer.Rows.Count = 1 Then
                            ' 対象年月の合計日当金額取得
                            If Not IsDBNull(dtTrasfer.Rows(0).Item("日当計2")) Then
                                nowPay = CInt(dtTrasfer.Rows(0).Item("日当計2"))
                            End If
                        End If

                        '---------------------------------------------------------------------------
                        '   対象年月の振込金額取得
                        '---------------------------------------------------------------------------
                        strSql = ""
                        strSql += "SELECT SUM(sbsm.s_daily_pay_total) AS dailyPayTotalSum"
                        strSql += "  FROM staf_bank_send sbs"                                       ' /* 振込データ */
                        strSql += "       INNER JOIN staf_bank_send_member sbsm"                    ' /* 振込データ詳細 */
                        strSql += "       ON  sbs.c_staf_bank_send_id = sbsm.c_staf_bank_send_id"   ' 振込番号IDが同じもの
                        strSql += "       AND sbs.d_bank_send         = sbsm.d_bank_send"           ' 振込日付が同じもの
                        strSql += " WHERE sbs.c_daily_pay_kind  LIKE '%" & i_dailyPayKind & "%'"    ' 日当対象締め日種別が含むもの
                        strSql += "   AND sbs.c_daily_pay_close LIKE '%" & i_dailyPayClose & "%'"   ' 日当対象締め日が含むもの
                        strSql += "   AND sbsm.c_user_id        = '" & i_userId & "'"               ' 個人認証IDが同じもの

                        ' SQL実行
                        dtTrasfer = i_clsDb.ExecuteSql(strSql)

                        ' データ有無判定
                        If dtTrasfer.Rows.Count = 1 Then
                            ' 対象年月の振込金額取得
                            If Not IsDBNull(dtTrasfer.Rows(0).Item("dailyPayTotalSum")) Then
                                payTotal = CInt(dtTrasfer.Rows(0).Item("dailyPayTotalSum"))
                            End If
                        End If

                        ' 対象年月の振込金額が相殺レコードの合計日当金額と対象年月の合計日当金額を足した金額と一致するかチェック
                        If payTotal = (prevPay + nowPay) Then
                            blnResult = True
                        End If
                    End If
                End If
            Else
                '-----------------------------------------------------------------------------------
                '   データが無い場合
                '-----------------------------------------------------------------------------------
                ' 前対象年月の合計日当金額がマイナスかチェック

                ' 対象年月取得
                dtTargetDate = i_dailyPayClose & "/01 0:00:00"

                '===================================================================================
                '   開始日終了日取得処理
                '===================================================================================
                ' 前対象年月の開始日終了日を取得
                Call GetTargetDateFromTo( _
                    2, _
                    i_clsDb, _
                    dtTargetDate, _
                    i_userId, _
                    dtFrom, _
                    dtTo _
                )

                ' 前対象年月の合計日当金額取得
                strSql = ""
                strSql += "SELECT SUM(cru.s_daily_pay_total"
                strSql += "         + cru.s_food_expenses_total"
                strSql += "         + cru.s_next_balance_daily_pay_total"
                strSql += "         + cru.s_next_balance_food_expenses_total) AS 日当計"
                strSql += "  FROM call_roll_user cru"                                       ' /* 組合活動状況の親テーブル */
                strSql += " WHERE cru.k_daily_pay_kind  = '01'"                             ' 日当計算区分が '01'：部／委員会日当のもの
                strSql += "   AND cru.d_years          >= CONVERT(DATE,'" & dtFrom & "')"      ' 対象年月が開始日より大きいもの
                strSql += "   AND cru.d_years          <= CONVERT(DATE,'" & dtTo & "')"        ' 対象年月が終了日より小さいもの
                strSql += "   AND cru.c_user_id         = '" & i_userId & "'"               ' 個人認証IDが同じもの

                ' SQL実行
                dtTrasfer = i_clsDb.ExecuteSql(strSql)

                ' データ有無判定
                If dtTrasfer.Rows.Count = 1 Then
                    ' 合計日当金額取得
                    If Not IsDBNull(dtTrasfer.Rows(0).Item("日当計")) Then
                        ' 合計日当金額がマイナスかチェック
                        If CInt(dtTrasfer.Rows(0).Item("日当計")) < 0 Then
                            blnResult = True
                        End If
                    End If
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

        ' 戻り値設定
        Return blnResult

    End Function
#End Region

#Region " 開始日終了日取得処理 "
    '***************************************************************************************************
    '   ＩＤ　：GetTargetDateFromTo
    '   名称　：開始日終了日取得処理
    '   概要　：対象年月（日付/時刻型）を元に開始日終了日を取得する。
    '   引数　：ByVal i_kind   As Integer     = 種別（1：通常（当対象年月）, 2：前対象年月,
    '           ByVal i_clsDb  As CLAccessMdb = データベースクラス,
    '           ByVal i_years  As DateTime    = 対象年月（日付/時刻型）,
    '           ByVal i_userId As String      = 個人認証ID,
    '           ByRef io_From  As DateTime    = 開始日,
    '           ByRef io_To    As DateTime    = 終了日
    '   戻り値：なし
    '   作成日：2021/10/29(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/10/29(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>開始日終了日取得処理</summary>
    ''' <param name="i_kind">種別（1：通常（当対象年月）, 2：前対象年月）</param>
    ''' <param name="i_clsDb">データベースクラス</param>
    ''' <param name="i_years">対象年月（日付/時刻型）</param>
    ''' <param name="i_userId">個人認証ID</param>
    ''' <param name="io_From">開始日</param>
    ''' <param name="io_To">終了日</param>
    ''' <remarks></remarks>
    Private Sub GetTargetDateFromTo( _
        ByVal i_kind As Integer, _
        ByVal i_clsDb As CLAccessMdb, _
        ByVal i_years As DateTime, _
        ByVal i_userId As String, _
        ByRef io_From As DateTime, _
        ByRef io_To As DateTime _
    )

        Dim dtFrom As DateTime = Nothing        ' 開始日
        Dim dtTo As DateTime = Nothing          ' 終了日
        Dim intMonth As Integer = 0             ' 対象月

        Try
            ' 対象年月の月を取得
            intMonth = i_years.Month

            ' 種別判定
            If i_kind = 1 Then

                '-----------------------------------------------------------------------------------
                '   当対象年月
                '-----------------------------------------------------------------------------------
                ' 対象年月の月判定
                Select Case intMonth
                    Case 1
                        ' 1月
                        dtFrom = (i_years.Year - 1).ToString() & "/11/01 0:00:00"           ' 対象年-1年の11月01日取得
                        dtTo = i_years.Year.ToString() & "/01/01 0:00:00"                   ' 対象年の01月01日取得

                    Case 2, 3, 4
                        ' 2月～4月
                        dtFrom = i_years.Year.ToString() & "/02/01 0:00:00"                 ' 対象年の02月01日取得
                        dtTo = i_years.Year.ToString() & "/04/01 0:00:00"                   ' 対象年の04月01日取得

                    Case 5, 6, 7
                        ' 5月～7月
                        dtFrom = i_years.Year.ToString() & "/05/01 0:00:00"                 ' 対象年の05月01日取得
                        dtTo = i_years.Year.ToString() & "/07/01 0:00:00"                   ' 対象年の07月01日取得

                    Case 8, 9, 10
                        ' 8月～10月
                        dtFrom = i_years.Year.ToString() & "/08/01 0:00:00"                 ' 対象年の08月01日取得
                        dtTo = i_years.Year.ToString() & "/10/01 0:00:00"                   ' 対象年の10月01日取得

                    Case 11, 12
                        ' 11月, 12月
                        dtFrom = i_years.Year.ToString() & "/11/01 0:00:00"                 ' 対象年の11月01日取得
                        dtTo = (i_years.Year + 1).ToString() & "/01/01 0:00:00"             ' 対象年+1年の01月01日取得

                End Select

            Else

                '-----------------------------------------------------------------------------------
                '   前対象年月
                '-----------------------------------------------------------------------------------
                ' 対象年月の月判定
                Select Case intMonth
                    Case 1
                        ' 1月
                        dtFrom = (i_years.Year - 1).ToString() & "/08/01 0:00:00"           ' 対象年-1年の08月01日取得
                        dtTo = (i_years.Year - 1).ToString() & "/10/01 0:00:00"             ' 対象年-1年の10月01日取得

                    Case 2, 3, 4
                        ' 2月～4月
                        dtFrom = (i_years.Year - 1).ToString() & "/11/01 0:00:00"           ' 対象年-1年の11月01日取得
                        dtTo = i_years.Year.ToString() & "/01/01 0:00:00"                   ' 対象年の01月01日取得

                    Case 5, 6, 7
                        ' 5月～7月
                        dtFrom = i_years.Year.ToString() & "/02/01 0:00:00"                 ' 対象年の02月01日取得
                        dtTo = i_years.Year.ToString() & "/04/01 0:00:00"                   ' 対象年の04月01日取得

                    Case 8, 9, 10
                        ' 8月～10月
                        dtFrom = i_years.Year.ToString() & "/05/01 0:00:00"                 ' 対象年の05月01日取得
                        dtTo = i_years.Year.ToString() & "/07/01 0:00:00"                   ' 対象年の07月01日取得

                    Case 11, 12
                        ' 11月, 12月
                        dtFrom = i_years.Year.ToString() & "/08/01 0:00:00"                 ' 対象年の08月01日取得
                        dtTo = i_years.Year.ToString() & "/10/01 0:00:00"                   ' 対象年の10月01日取得

                End Select

            End If

            ' 取得した開始日・終了日を戻り値に設定
            io_From = dtFrom        ' 開始日
            io_To = dtTo            ' 終了日

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)

            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal( _
                Err.Number, _
                Err.Description, _
                SCREEN_ID, SCREEN_NAME, _
                System.Reflection.MethodInfo.GetCurrentMethod.Name() _
            )

        End Try

    End Sub
#End Region
#End Region

End Class
#End Region
