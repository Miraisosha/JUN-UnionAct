Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDFile
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports System.Text.RegularExpressions
Public Class UC030401

#Region "定数・メンバ変数"

    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    'ファイル保存ダイアログタイトル
    Private Const SAVE_DIALOG_TITLE As String = "ファイルの保存先を選択してください"

    Private _strTitle As String = String.Empty
    Private _datePayDay As DateTime = Nothing

    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC030401                              ' UC030401
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC030401                         ' 委員会データ出力 － 新規登録画面

#End Region

#Region "委員会データファイル出力ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnCommitteeOutput_Click
    '   名称　：ファイル出力ボタンクリック
    '   概要　：
    '   作成日：2021/08/04(水)  w.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/08/04(水)  w.suzuki  新規作成
    '***************************************************************************************************

    Private Sub btnCommitteeOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommitteeOutput.Click
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


#Region "CSVファイル出力処理"
    '***************************************************************************************************
    '   ＩＤ　：SaveCsvFile
    '   名称　：CSVファイル出力処理
    '   概要　：委員会データのCSVファイルを出力します
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>CSVファイル出力処理</summary>
    ''' <remarks></remarks>
    Private Sub SaveCsvFile()
        'csv出力データ
        Dim dtOutputData As DataTable = Nothing
        Dim sfd As New System.Windows.Forms.SaveFileDialog
        Me._strTitle = "委員会データ"
        Me._datePayDay = CDate(Me.dtpDate.Text)
        'デフォルトファイル名（振込日_題目.csv）
        Dim strFileName As String = Format(Me._datePayDay, "yyyy-MM-dd") & "_" & Regex.Replace(Me._strTitle, "[\\/:*?\""<>|]", "") & ".csv"

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
    '   戻り値：
    '   作成日：2012/02/08(水)  a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/08(水)  a.onuma  新規作成
    '***************************************************************************************************
    Private Function SetCsvData() As DataTable
        Dim dtOutput As DataTable = New DataTable
        Dim iCurrentRow As Integer = 0
        Dim strSql As String = ""                       ' SQL文
        Dim clsDb As New CLAccessMdb                    ' データベースクラス
        Dim tbRet As DataTable = Nothing                ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                    ' 検索結果件数
        '基準日の入力
        Dim dt_date As DateTime
        Dim st_date As String
        '日付型を文字列変換
        dt_date = CDate(Me.dtpDate.Text) '日付型に変換
        st_date = dt_date.ToString("yyyyMMdd") 'yyyyMMdd文字列に変換


        Try

            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = "SELECT " &
"    comall.*, " &
"    opm.s_officer_pay AS 役員手当額 " &
"FROM " &
"    ( " &
"        SELECT " &
"            cld.c_committee_list, " &
"            cld.c_committee_id AS 委員会ID, " &
"            cld.s_committee_seq AS 役職ID, " &
"            com.l_name AS 委員会名, " &
"            comd.l_name AS 役職名, " &
"            cld.c_user_id AS 社番, " &
"            salv.l_name AS 氏名, " &
"            salv.s_name AS 所属, " &
"            salv.m_name AS 機種, " &
"            salv.l_omission_name AS 機種略称, " &
"            comd.c_officer_pay_id AS 役員手当ID, " &
"            cld.d_from AS 適用開始日 " &
"        FROM " &
"            committee_list_dtl AS cld, " &
"            ( " &
"                SELECT " &
"                    MAX(c_committee_list) AS MAX_ccl, " &
"                    c_committee_id " &
"                FROM " &
"                    committee_list " &
"                WHERE " &
"                    d_from <= '" & st_date & "' " &
"                GROUP BY " &
"                    c_committee_id " &
"            ) Mcd, " &
"            ( " &
"                SELECT " &
"                    sa.*, " &
"                    cd2.l_name AS s_name " &
"                FROM " &
"                    ( " &
"                        SELECT " &
"                            sa3.*, " &
"                            cd1.l_name AS m_name, " &
"                            cd1.l_omission_name " &
"                        FROM " &
"                            ( " &
"                                SELECT " &
"                                    sa1.* " &
"                                FROM " &
"                                    staf_attribute AS sa1, " &
"                                    ( " &
"                                        SELECT " &
"                                            c_user_id, " &
"                                            MAX(d_from) AS MAX_dfrom " &
"                                        FROM " &
"                                            staf_attribute " &
"                                        WHERE " &
"                                            d_from <= '" & st_date & "' " &
"                                        GROUP BY " &
"                                            c_user_id " &
"                                    ) AS sa2 " &
"                                WHERE " &
"                                    sa1.c_user_id = sa2.c_user_id " &
"                                    AND sa1.d_from = sa2.MAX_dfrom " &
"                            ) AS sa3 " &
"                            LEFT JOIN ( " &
"                                SELECT " &
"                                    * " &
"                                FROM " &
"                                    constant_dtl " &
"                                WHERE " &
"                                    c_constant = 'MODEL' " &
"                            ) AS cd1 ON sa3.k_model = cd1.c_constant_seq " &
"                    ) AS sa " &
"                    LEFT JOIN ( " &
"                        SELECT " &
"                            * " &
"                        FROM " &
"                            constant_dtl " &
"                        WHERE " &
"                            c_constant = 'BELONGING' " &
"                    ) AS cd2 ON sa.k_belonging = cd2.c_constant_seq " &
"            ) AS salv, " &
"            ( " &
"                SELECT " &
"                    * " &
"                FROM " &
"                    committee " &
"                WHERE " &
"                    d_from <= '" & st_date & "' " &
"                    AND d_to >= '" & st_date & "' " &
"                ORDER BY " &
"                    c_committee_id " & UtDb.DbOrderOffset &
"            ) AS com, " &
"            ( " &
"                SELECT " &
"                    * " &
"                FROM " &
"                    committee_dtl " &
"                WHERE " &
"                    d_from <= '" & st_date & "' " &
"                    AND d_to >= '" & st_date & "' " &
"                ORDER BY " &
"                    c_committee_id " & UtDb.DbOrderOffset &
"            ) AS comd " &
"        WHERE " &
"            cld.c_user_id = salv.c_user_id " &
"            AND cld.c_committee_id = com.c_committee_id " &
"            AND cld.c_committee_id = comd.c_committee_id " &
"            AND cld.s_committee_seq = comd.s_committee_seq " &
"            AND cld.c_committee_list = Mcd.MAX_ccl " &
"            AND cld.c_committee_id = Mcd.c_committee_id " &
"        ORDER BY " &
"            cld.c_committee_id, " &
"            cld.s_committee_seq, " &
"            salv.k_belonging, " &
"            cld.c_user_id " & UtDb.DbOrderOffset &
"    ) AS comall "
            If Me.Officers.Checked = True Then
                strSql = strSql & "    INNER JOIN ( "
            Else
                strSql = strSql & "            LEFT JOIN ( "
            End If
            strSql = strSql & "        SELECT " &
"            * " &
"        FROM " &
"            officer_pay_master " &
"        WHERE " &
"            d_from <= '" & st_date & "' " &
"            AND d_to >= '" & st_date & "' " &
"        ORDER BY " &
"            c_officer_pay_id " & UtDb.DbOrderOffset &
"    ) AS opm ON comall.役員手当ID = opm.c_officer_pay_id " &
"ORDER BY " &
"    委員会ID, " &
"    役職ID, " &
"    所属 DESC, " &
"    社番" & UtDb.DbOrderOffset & ";" 'ok

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtOutput = clsDb.ExecuteSql(strSql)

            '出力順の入れ替え
            dtOutput.Columns("適用開始日").SetOrdinal(12)

            ' 件数取得
            intRetCnt = dtOutput.Rows.Count

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
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
End Class
