#Region "UC040801"
'===========================================================================================================
'   クラスＩＤ　　：UC040801
'   クラス名称　　：UPUJ活動日登録日検索
'   備考  　　　　：労金データ作成(UC080104をベースに作成）
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

Public Class UC040801

#Region "定数・メンバ変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ' 画面関連
    ' TODO MDCosntに追加
    Private Const SCREEN_ID As String = "UC040801"
    Private Const SCREEN_NAME As String = "UP・UJデータ出力"

    'ファイル保存ダイアログタイトル
    Private Const SAVE_DIALOG_TITLE As String = "ファイルの保存先を選択してください"

    Private Const OUTPUT_REGIST_DATE_FILE_NAME As String = "登録日"
    Private Const OUTPUT_ACTION_DATE_FILE_NAME As String = "活動日"
    Private Const TARGET_DATE_NAME As String = "基準日"
#End Region

#Region "イベント"

#Region "onLoad"

#End Region

#Region "活動日出力ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnOutputActionList_Click
    '   名称　：活動日出力ボタンクリックイベント
    '   概要　：入力チェック→csv出力先指定→csv作成
    '   作成日：2021-08-03
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021-08-03
    '***************************************************************************************************
    Private Sub btnOutputActionList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOutputActionList.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            Me.Cursor = Cursors.WaitCursor
            'CSV出力処理実行
            Call Me.SaveCsvFile(0)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "登録日出力ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnOutputActionList_Click
    '   名称　：登録日出力ボタンクリックイベント
    '   概要　：入力チェック→csv出力先指定→csv作成
    '   作成日：2021-08-03
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021-08-03
    '***************************************************************************************************
    Private Sub btnRegistList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRegistList.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            Me.Cursor = Cursors.WaitCursor
            'CSV出力処理実行
            Call Me.SaveCsvFile(1)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region
#End Region

#Region "関数"

#Region "活動日・登録日指定のCSV出力"
    '***************************************************************************************************
    '   ＩＤ　：SaveCsvFile
    '   名称　：活動日・登録日指定のCSV出力
    '   概要　：活動日・登録日指定のUP・UJの出力処理
    '   引数　：intActionOrRegist 0:活動日 0以外:登録日
    '   戻り値：なし
    '   作成日：2021-08-18
    '   更新日：
    '   備考  ：TODO SQL Injection対策
    '***************************************************************************************************
    ''' <summary>活動日・登録日指定のCSV出力</summary>
    ''' <remarks></remarks>
    Private Sub SaveCsvFile(ByVal intActionOrRegist As Integer)
        'csv出力データ
        Dim sfd As New System.Windows.Forms.SaveFileDialog
        ' DataTable
        Dim dtOutputData As New DataTable
        'デフォルトファイル名（振込日_題目.csv）
        Dim strFileName As String = ""
        ' 比較対象日付
        Dim dtBeginDate As DateTime
        Dim dtEndDate As DateTime

        ' 0:活動日 0以外:登録日
        If intActionOrRegist = 0 Then
            dtBeginDate = Me.dtpActionBeginDate.Value
            dtEndDate = Me.dtpActionEndDate.Value
            ' 日付の入れ替え
            Me.GetFileName(dtBeginDate, dtEndDate)

            strFileName = OUTPUT_ACTION_DATE_FILE_NAME
        Else
            dtBeginDate = Me.dtpRegistBeginDate.Value
            dtEndDate = Me.dtpRegistEndDate.Value
            ' 日付の入れ替え
            Me.GetFileName(dtBeginDate, dtEndDate)

            strFileName = OUTPUT_REGIST_DATE_FILE_NAME
        End If

        strFileName = Format(dtBeginDate, "yyyy-MM-dd") & _
                        "～" & Format(dtEndDate, "yyyy-MM-dd") & _
                        "_" & strFileName & ".csv"

        'ファイル保存ダイアログタイトル
        sfd.Title = SAVE_DIALOG_TITLE
        sfd.Filter = "CSVファイル(*.csv)|*.csv"
        sfd.FileName = strFileName

        ' ファイル選択を行って処理の継続を判定
        If sfd.ShowDialog = DialogResult.OK Then
            ' DataTableの取得
            dtOutputData = Me.SetCsvData(intActionOrRegist, dtBeginDate, dtEndDate)
            ' CSVファイル出力
            If CsvPut(dtOutputData, sfd.FileName) = True Then
                CLMsg.Show("GI0028")
            Else
                CLMsg.Show("BE0022", sfd.FileName)
            End If
        Else
            'TODO:メッセージ出す？
        End If
        Try
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
#End Region

#Region "活動日・登録日データ作成"
    '***************************************************************************************************
    '   ＩＤ　：SaveCsvFile
    '   名称　：動日・登録日データ作成
    '   概要　：CSV出力データを作成し返却します
    '   引数　：intActionOrRegist 0:活動日 0以外:登録日
    '   戻り値：DataTable
    '   作成日：2021-08-18
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021-08-18    新規作成
    '***************************************************************************************************
    Private Function SetCsvData(ByVal intActionOrRegist As Integer, ByVal beginDate As DateTime, ByVal endDate As DateTime) As DataTable
        Dim dtOutput As DataTable = New DataTable
        Dim strSql As String
        Dim clsDb As New CLAccessMdb        ' データベースクラス

        ' query
        ' 出力する職員情報は検索期間を問わずstaf_attribute_latest_viewを参照
        strSql = "SELECT " &
                "    upuj.社員番号, " &
                "    upuj.日付 as 活動日, " &
                "    upuj.区分, " &
                "    upuj.番号, " &
                "    upuj.地域, " &
                "    staf.l_name AS 氏名, " &
                "    const.l_name AS 種別, " &
                "    upuj.登録日 " &
                "FROM " &
                "    ( " &
                "        SELECT " &
                "            '時間内' AS 区分, " &
                "            asmd.c_strike_id AS 番号, " &
                "            IIF(asmd.k_apply_area = '01', '東京', '大阪') AS 地域, " &
                "            asmd.c_staf_id AS 社員番号, " &
                "            FORMAT(asmd.d_strike, 'yyyy/MM/dd') AS 日付, " &
                "            FORMAT(aply.d_application, 'yyyy/MM/dd') AS 登録日 " &
                "        FROM " &
                "            apply_strike aply, " &
                "            apply_strike_member_date asmd " &
                "        WHERE " &
                "            asmd.k_cancel <> '1' " &
                "            AND aply.k_cancel <> '1' " &
                "            AND aply.c_strike_id = asmd.c_strike_id " &
                "            AND aply.k_apply_area = asmd.k_apply_area " &
                "        UNION " &
                "        SELECT " &
                "            '指名スト' AS 区分, " &
                "            nsmd.c_name_strike_id AS 番号, " &
                "            '-' AS 地域, " &
                "            nsmd.c_user_id AS 社員番号, " &
                "            LEFT(nmst.d_operation_from, 10) AS 日付, " &
                "            LEFT(nmst.d_info, 10) AS 登録日 " &
                "        FROM " &
                "            name_strike nmst, " &
                "            name_strike_member_date nsmd " &
                "        WHERE " &
                "            nsmd.c_cancel_name_strike_id = '' " &
                "            AND nmst.c_cancel = '' " &
                "            AND nmst.c_name_strike_id = nsmd.c_name_strike_id " &
                "    ) AS upuj, " &
                "    staf_attribute AS staf, " &
                "    ( " &
                "        SELECT " &
                "            M.c_user_id AS c_user_id, " &
                "            MAX(M.d_from) AS d_from " &
                "        FROM " &
                "            staf_attribute AS M, " &
                "            daily_pay_calc_temp AS INFO " &
                "        WHERE " &
                "            M.d_from < FORMAT(GETDATE(), 'yyyyMMdd') " &
                "            AND M.c_ksh = INFO.c_ksh " &
                "        GROUP BY " &
                "            M.c_user_id " &
                "    ) AS salv, " &
                "    constant_dtl AS const " &
                "WHERE " &
                "    upuj.社員番号 = salv.c_user_id " &
                "    AND salv.c_user_id = staf.c_user_id " &
                "    AND salv.d_from = staf.d_from " &
                "    AND staf.k_staf_kind = const.c_constant_seq " &
                "    AND const.c_constant = 'STAF_KIND' "
        ' 0:活動日 0以外:登録日
        If intActionOrRegist = 0 Then
            strSql = strSql & _
                "    AND upuj.日付 BETWEEN '" & beginDate.ToString("yyyy/MM/dd") & "' AND '" & endDate.ToString("yyyy/MM/dd") & "' "
        Else
            strSql = strSql & _
                "    AND upuj.登録日 BETWEEN '" & beginDate.ToString("yyyy/MM/dd") & "' AND '" & endDate.ToString("yyyy/MM/dd") & "' "
        End If
        strSql = strSql &
                "ORDER BY " &
                "    upuj.登録日, " &
                "    upuj.日付, " &
                "    upuj.社員番号" 'ok

        Try
            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtOutput = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Error(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowConnectErr(Err.Number, _
                                       Err.Description, _
                                       SCREEN_ID, SCREEN_NAME, _
                                       System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        Return dtOutput
    End Function

#Region "活動日CSV出力データ作成"
    '***************************************************************************************************
    '   ＩＤ　：SaveCsvFile
    '   名称　：活動日CSV出力データ作成
    '   概要　：CSV出力データを作成し返却します
    '   引数　：d1:開始日 
    '         ：d2:終了日
    '   戻り値：ByRefで最終的にd1に小さい日付が渡される
    '   作成日：2021-08-18
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021-08-18    新規作成
    '***************************************************************************************************
    Private Sub GetFileName(ByRef d1 As DateTime, ByRef d2 As DateTime)
        Dim tmpDate As DateTime

        If DateTime.Compare(d1, d2) > 0 Then
            tmpDate = d1
            d1 = d2
            d2 = tmpDate
        End If
    End Sub
#End Region

#End Region
#End Region
End Class
#End Region
