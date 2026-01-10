Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDFile
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports System.Text.RegularExpressions
Public Class UC040701

#Region "定数・メンバ変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    'csvファイルヘッダー
    Private _strCsvHeaderArray As String() = {"社員番号", "氏名", "住所更新日"}
    'ファイル保存ダイアログタイトル
    Private Const SAVE_DIALOG_TITLE As String = "ファイルの保存先を選択してください"

    Private _strTitle As String = String.Empty
    Private _datePayDay As DateTime = Nothing

    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC040701                              ' UC080104
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040701                          ' 労金データ作成 － 新規登録画面
#End Region

#Region "住所変更ファイル出力ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnBankOutput_Click
    '   名称　：ファイル出力ボタンクリック
    '   概要　：
    '   作成日：2021/08/04(水)  w.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2021/08/04(水)  w.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnBankOutput_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBankOutput.Click
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
    '   概要　：住所変更履歴のCSVファイルを出力します
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
        Me._strTitle = "住所変更日"
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
            strSql = "" & vbCrLf
            strSql = strSql & "SELECT stad2.c_user_id, stat2.l_name, stad2.M_d_from AS 更新日" & vbCrLf         ' 社員番号,名前,更新日
            strSql = strSql & " FROM (SELECT T.c_user_id, T.Max_d_from, M.l_name FROM (SELECT c_user_id, MAX(d_from) AS Max_d_from FROM staf_attribute WHERE d_from <= '" & st_date & "' GROUP BY c_user_id) AS T" & vbCrLf '変数に変更
            strSql = strSql & " ,staf_attribute AS M WHERE T.c_user_id = M.c_user_id AND T.Max_d_from = M.d_from) AS stat2" & vbCrLf         ' 組合員基本情報
            strSql = strSql & " ,(SELECT stad1.c_user_id, MAX(stad1.d_from) AS M_d_from FROM staf_address AS stad1 WHERE d_from <= '" & st_date & "' AND (not stad1.k_del = '1' OR stad1.k_del is null) GROUP BY stad1.c_user_id) AS stad2" '住所情報　変数に変更予定
            strSql = strSql & " WHERE stat2.c_user_id = stad2.c_user_id ORDER BY stad2.M_d_from DESC,stad2.c_user_id"
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtOutput = clsDb.ExecuteSql(strSql)

            'カラムの設定
            dtOutput.Columns("c_user_id").ColumnName = "社員番号"
            dtOutput.Columns("l_name").ColumnName = "氏名"
            dtOutput.Columns("更新日").ColumnName = "住所更新日"

            ' 件数取得
            'intRetCnt = dtOutput.Rows.Count

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
