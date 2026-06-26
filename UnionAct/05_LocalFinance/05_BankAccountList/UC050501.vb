#Region "UC050501"
'===========================================================================================================
'   クラスＩＤ　　：UC050501
'   クラス名称　　：組合員の口座一覧作成
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

Public Class UC050501

#Region "定数・メンバ変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ' 画面関連
    ' TODO MDCosntに追加
    Private Const SCREEN_ID As String = "UC050501"
    Private Const SCREEN_NAME As String = "全組合員の銀行口座一覧"

    'ファイル保存ダイアログタイトル
    Private Const SAVE_DIALOG_TITLE As String = "ファイルの保存先を選択してください"

    Private Const OUTPUT_FILE_NAME As String = "銀行口座一覧"
    Private Const TARGET_DATE_NAME As String = "基準日"
#End Region

#Region "イベント"

#Region "onLoad"
    Private Sub UC050501_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
    End Sub
#End Region

#Region "出力ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPrintJpnSyllabary_Click
    '   名称　：出力ボタンクリックイベント
    '   概要　：入力チェック→csv出力先指定→csv作成
    '   作成日：2021-08-03
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021-08-03
    '***************************************************************************************************
    Private Sub btnPrintJpnSyllabary_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintJpnSyllabary.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            Me.Cursor = Cursors.WaitCursor
            'CSV出力処理実行
            Call Me.SaveCsvFile()
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

#Region "CSV出力"
    '***************************************************************************************************
    '   ＩＤ　：SaveCsvFile
    '   名称　：CSVファイル出力処理
    '   概要　：組合員の銀行口座一覧の出力処理
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2021-08-02 watanabe
    '   更新日：
    '   備考  ：TODO SQL Injection対策
    '***************************************************************************************************
    ''' <summary>CSVファイル出力処理</summary>
    ''' <remarks></remarks>
    Private Sub SaveCsvFile()
        'csv出力データ
        Dim sfd As New System.Windows.Forms.SaveFileDialog
        ' DataTable
        Dim dtOutputData As New DataTable
        'デフォルトファイル名（振込日_題目.csv）
        Dim strFileName As String = Format(Me.dtpTargetDate.Value, "yyyy-MM-dd") & "_" & OUTPUT_FILE_NAME & ".csv"

        'ファイル保存ダイアログタイトル
        sfd.Title = SAVE_DIALOG_TITLE
        sfd.Filter = "CSVファイル(*.csv)|*.csv"
        sfd.FileName = strFileName

        If sfd.ShowDialog = DialogResult.OK Then
            dtOutputData = Me.SetCsvData()
            'CSVファイル出力
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

#Region "CSV出力データ作成"
    '***************************************************************************************************
    '   ＩＤ　：SaveCsvFile
    '   名称　：CSV出力データ作成
    '   概要　：CSV出力データを作成し返却します
    '   引数　：なし
    '   戻り値：DataTable
    '   作成日：2021-08-03
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021-08-03    新規作成
    '***************************************************************************************************
    Private Function SetCsvData() As DataTable
        Dim dtOutput As DataTable = New DataTable
        Dim strSql As String
        Dim ymd As String
        Dim clsDb As New CLAccessMdb        ' データベースクラス

        ymd = Me.dtpTargetDate.Value.ToString("yyyyMMdd")

        ' query
        ' 属性情報について利用するstaf_attributeのc_user_idとd_from
        ' 適用日と適用日以降の直近の最大2件を取って日付が古い情報を利用
        ' 2件ある場合＝適用対象と指定日以降の情報がある
        ' 1件ある場合＝適用対象のみ、もしくは、指定日以降の直近情報のみ
        'strSql = "SELECT " &
        '        "    sat.c_user_id AS 社員番号, " &
        '        "    sat.l_name AS 名前, " &
        '        "    skcd.l_name AS 組合員種別, " &
        '        "    uscd.l_name AS ステータス, " &
        '        "    sac.c_bank AS 銀行コード, " &
        '        "    bi.l_bank_name AS 銀行名, " &
        '        "    bi.l_bank_name_kna AS [銀行名（フリガナ）], " &
        '        "    sac.c_bank_office AS 支店コード, " &
        '        "    bid.l_bank_office_name AS 支店名, " &
        '        "    bid.l_bank_office_name_kna AS [支店名（フリガナ）], " &
        '        "    sac.k_deposit_items AS 預金種目コード, " &
        '        "    IIf( " &
        '        "        sac.k_deposit_items = '01', '普通', " &
        '        "        IIF(sac.k_deposit_items = '02', '当座', '') " &
        '        "    ) AS 預金種目, " &
        '        "    sac.c_bank_account AS 口座番号, " &
        '        "    sac.l_account_name AS 口座名義, " &
        '        "    sac.l_account_name_kna AS 口座名義カナ " &
        '        "FROM " &
        '        "    staf_account sac, " &
        '        "    ( " &
        '        "        SELECT " &
        '        "            c_user_id, " &
        '        "            MAX(d_from) AS max_d_from " &
        '        "        FROM " &
        '        "            staf_account " &
        '        "        WHERE " &
        '        "            d_from <= '" & ymd & "' " &
        '        "        GROUP BY " &
        '        "            c_user_id " &
        '        "    ) msac, " &
        '        "    staf_attribute sat, " &
        '        "    ( " &
        '        "        SELECT " &
        '        "            c_user_id, " &
        '        "            MIN(d_from) AS d_from " &
        '        "        FROM " &
        '        "            staf_account " &
        '        "        WHERE " &
        '        "            d_from > '" & ymd & "' " &
        '        "        GROUP BY " &
        '        "            c_user_id " &
        '        "    ) msat, " &
        '        "    bank_info bi, " &
        '        "    bank_info_dtl bid, " &
        '        "    constant_dtl skcd, " &
        '        "    constant_dtl uscd " &
        '        "WHERE " &
        '        "    sat.c_user_id = sac.c_user_id " &
        '        "    AND (sat.c_user_id = msat.c_user_id OR msat.c_user_id IS NULL) " &
        '        "    AND (sat.d_from = msat.d_from AND msat.d_from IS NOT NULL OR msat.d_from IS NULL) " &
        '        "    AND sac.c_user_id = msac.c_user_id " &
        '        "    AND sac.d_from = msac.max_d_from " &
        '        "    AND sac.c_bank = bi.c_bank " &
        '        "    AND sac.c_bank = bid.c_bank " &
        '        "    AND sac.c_bank_office = bid.c_bank_office " &
        '        "    AND bi.d_from <= '" & ymd & "' " &
        '        "    AND bi.d_to >= '" & ymd & "' " &
        '        "    AND bid.d_from <= '" & ymd & "' " &
        '        "    AND bid.d_to >= '" & ymd & "' " &
        '        "    AND sat.k_staf_kind = skcd.c_constant_seq " &
        '        "    AND skcd.c_constant = 'STAF_KIND' " &
        '        "    AND skcd.d_from <= '" & ymd & "' " &
        '        "    AND skcd.d_to >= '" & ymd & "' " &
        '        "    AND sat.k_user_status = uscd.c_constant_seq " &
        '        "    AND uscd.c_constant = 'USER_STATUS' " &
        '        "    AND uscd.d_from <= '" & ymd & "' " &
        '        "    AND uscd.d_to >= '" & ymd & "' " &
        '        "ORDER BY " &
        '        "    CLng(sat.c_user_id)"   'ok
        strSql = "SELECT " &
                "    sat.c_user_id AS 社員番号, " &
                "    sat.l_name AS 名前, " &
                "    skcd.l_name AS 組合員種別, " &
                "    uscd.l_name AS ステータス, " &
                "    sac.c_bank AS 銀行コード, " &
                "    bi.l_bank_name AS 銀行名, " &
                "    bi.l_bank_name_kna AS [銀行名（フリガナ）], " &
                "    sac.c_bank_office AS 支店コード, " &
                "    bid.l_bank_office_name AS 支店名, " &
                "    bid.l_bank_office_name_kna AS [支店名（フリガナ）], " &
                "    sac.k_deposit_items AS 預金種目コード, " &
                "    IIf( " &
                "        sac.k_deposit_items = '01', '普通', " &
                "        IIF(sac.k_deposit_items = '02', '当座', '') " &
                "    ) AS 預金種目, " &
                "    sac.c_bank_account AS 口座番号, " &
                "    sac.l_account_name AS 口座名義, " &
                "    sac.l_account_name_kna AS 口座名義カナ " &
                "FROM " &
                "    staf_account sac, " &
                "    ( " &
                "        SELECT " &
                "            c_user_id, " &
                "            MAX(d_from) AS max_d_from " &
                "        FROM " &
                "            staf_account " &
                "        WHERE " &
                "            d_from <= '" & ymd & "' " &
                "        GROUP BY " &
                "            c_user_id " &
                "    ) msac, " &
                "    staf_attribute sat, " &
                "    ( " &
                "        SELECT " &
                "            TSTAF.c_user_id, " &
                "            MIN(TSTAF.target_d_from) AS d_from " &
                "        FROM  " &
                "        ( " &
                "            SELECT " &
                "                c_user_id, " &
                "                MAX(d_from) AS target_d_from " &
                "            FROM " &
                "                staf_attribute " &
                "            WHERE " &
                "                d_from <= '" & ymd & "' " &
                "            GROUP BY " &
                "                c_user_id " &
                "            UNION " &
                "            SELECT " &
                "                c_user_id, " &
                "                MIN(d_from) AS target_d_from " &
                "            FROM " &
                "                staf_attribute " &
                "            WHERE " &
                "                d_from > '" & ymd & "' " &
                "            GROUP BY " &
                "                c_user_id " &
                "        ) AS TSTAF" &
                "        GROUP BY TSTAF.c_user_id " &
                "    ) msat, " &
                "    bank_info bi, " &
                "    bank_info_dtl bid, " &
                "    constant_dtl skcd, " &
                "    constant_dtl uscd " &
                "WHERE " &
                "    sat.c_user_id = sac.c_user_id " &
                "    AND sat.c_user_id = msat.c_user_id " &
                "    AND sat.d_from = msat.d_from " &
                "    AND sac.c_user_id = msac.c_user_id " &
                "    AND sac.d_from = msac.max_d_from " &
                "    AND sac.c_bank = bi.c_bank " &
                "    AND sac.c_bank = bid.c_bank " &
                "    AND sac.c_bank_office = bid.c_bank_office " &
                "    AND bi.d_from <= '" & ymd & "' " &
                "    AND bi.d_to >= '" & ymd & "' " &
                "    AND bid.d_from <= '" & ymd & "' " &
                "    AND bid.d_to >= '" & ymd & "' " &
                "    AND sat.k_staf_kind = skcd.c_constant_seq " &
                "    AND skcd.c_constant = 'STAF_KIND' " &
                "    AND skcd.d_from <= '" & ymd & "' " &
                "    AND skcd.d_to >= '" & ymd & "' " &
                "    AND sat.k_user_status = uscd.c_constant_seq " &
                "    AND uscd.c_constant = 'USER_STATUS' " &
                "    AND uscd.d_from <= '" & ymd & "' " &
                "    AND uscd.d_to >= '" & ymd & "' " &
                "ORDER BY " &
                "    CLng(sat.c_user_id)"

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

#End Region
#End Region

End Class
#End Region
