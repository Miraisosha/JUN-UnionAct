#Region "UC030201"
'===========================================================================================================
'   クラスＩＤ　　：UC030201
'   クラス名称　　：期別名簿検索
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst

Public Class UC030201

    '検索結果 DataGridView の列幅
    Private Const COLUMN_WIDTH_0% = 100 '社員番号
    Private Const COLUMN_WIDTH_1% = 150 '名前
    Private Const COLUMN_WIDTH_2% = 100 '支部
    Private Const COLUMN_WIDTH_3% = 150 '役職
    Private Const COLUMN_WIDTH_4% = 100 '機種
    Private Const COLUMN_WIDTH_5% = 100 '資格
#Region "ログ出力オブジェクト"
    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UC030201_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC030201_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim blnRet As Boolean = False      ' 処理結果
        Dim clsDb As New CLAccessMdb       ' データベースクラス生成
        Dim dt As DataTable

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' データグリッドビュー初期化
            Call Me.DataGridViewIni(Me.dgdResult)

            ' データベース接続
            Call clsDb.Connect()

            ' 期(検索条件) コンボボックス作成
            Call Me.CreateCboPeriod(clsDb, Me.cboPeriod, False)

            ' 年選択コンボボックス作成処理呼び出し
            Call Me.CreateCboYearSearch(clsDb, Me.cboYearSearch, False, 2, Me.cboPeriod.SelectedValue)
            Me.cboYearSearch.Text = Now.ToString("yyyy")

            ' 月選択コンボボックス作成処理呼び出し
            Me.cboMonthSearch.Text = Now.ToString("MM")

            ' 委員会選択コンボボックス作成処理呼び出し
            Call Me.CreateCboCommittee(clsDb, Me.cboCommittee, False, 2, Me.cboPeriod.SelectedValue)
            cboCommittee.SelectedIndex = 0

            ' 支部(検索条件) コンボボックス作成
            Call MDCommon.CreateCboConstantDtl(clsDb, Me.cboUnionBranchSearch, "BELONGING")

            ' 期(専門委員・部会名簿印刷) コンボボックス作成
            Call Me.CreateCboPeriod(clsDb, Me.cboTerm, False)

            ' 年選択コンボボックス作成処理呼び出し
            Call Me.CreateCboYearSearch(clsDb, Me.cboYear, False, 2, Me.cboTerm.SelectedValue)
            Me.cboYear.Text = Now.ToString("yyyy")

            ' 月選択コンボボックス作成処理呼び出し
            Me.cboMonth.Text = Now.ToString("MM")

            ' フォーカス設定
            Me.cboPeriod.Focus()

            '権限設定
            dt = MDCommon.getGrant(SCREEN_ID_UC030201)
            If dt.Rows(0).Item(5) = 0 Then
                Me.btnPrint.Enabled = False
                Me.btnPrintCommittee.Enabled = False
            End If


        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "UC030201_Load")

            log.Fatal(ex.Message)
        Finally
            Call clsDb.Disconnect()

            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "クリック"
#Region "検索ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' データ検索
            Call Me.GetSearchData()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "btnSearch_Click")

            log.Fatal(ex.Message)
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "履歴ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnHistory_Click
    '   名称　：履歴ボタンクリック
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHistory.Click

        Dim clsFM000206 As FM000206 = Nothing       ' 委員会名簿履歴一覧画面クラス

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            If Me.ChkExistHistory() = False Then
                CLMsg.Show("DI0001")
                Exit Sub
            End If

            clsFM000206 = New FM000206              ' 委員会名簿履歴一覧画面クラス生成

            clsFM000206.strSearchPeriodId = Me.cboPeriod.SelectedValue
            clsFM000206.strSearchCommitteeId = Me.cboCommittee.SelectedValue

            Call clsFM000206.ShowDialog()           ' 委員会名簿履歴一覧画面表示

            If clsFM000206.DialogResult = DialogResult.OK Then
                Me.cboYearSearch.Text = clsFM000206.strYear
                Me.cboMonthSearch.Text = clsFM000206.strMonth
            End If

            clsFM000206.Dispose()                   ' 委員会名簿履歴一覧画面クラス破棄

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "btnHistory_Click")

            log.Fatal(ex.Message)

            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "印刷ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：印刷ボタンクリック
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Dim fmPrint As New FM000203   '印刷プレビューフォーム
        Dim ds As New DS0302P1        'データセット
        Dim strHeaderRow(1) As String 'ヘッダー行配列
        Dim strDetailRow(3) As String '明細行配列

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            'ヘッダーのデータを作成
            strHeaderRow(0) = Me.cboPeriod.Text    '期
            strHeaderRow(1) = Me.cboCommittee.Text '委員会

            Call ds.dtHeader.Rows.Add(strHeaderRow)

            '明細のデータを作成
            For i = 0 To Me.dgdResult.Rows.Count - 1
                strDetailRow(0) = NSMDCommon.NVL(Me.dgdResult.Rows(i).Cells(0).Value) '社員番号
                strDetailRow(1) = NSMDCommon.NVL(Me.dgdResult.Rows(i).Cells(1).Value) '名前
                strDetailRow(2) = NSMDCommon.NVL(Me.dgdResult.Rows(i).Cells(2).Value) '支部
                strDetailRow(3) = NSMDCommon.NVL(Me.dgdResult.Rows(i).Cells(3).Value) '役職

                Call ds.dtDetail.Rows.Add(strDetailRow)
            Next

            '印刷プレビュー準備
            fmPrint.ButtonShowType = 3         '印刷、キャンセルボタン
            fmPrint.PrintCntVisible = False    '印刷部数非表示
            fmPrint.ObjResource = New CR0302P1 '帳票インスタンスを作成

            fmPrint.ObjResource.SetDataSource(ds) 'データセットをセット

            'フォームを表示
            Call fmPrint.ShowDialog()

            Select Case fmPrint.IntQlickBtnFlag
                Case 1
                Case 2
                    'キャンセル
                Case 3
                    '印刷
                    fmPrint.PrintOut()
            End Select

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "btnPrint_Click")

            log.Fatal(ex.Message)
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "データセット作成"

    Function funcCreateDS() As Boolean

    End Function

#End Region



#Region "印刷（専門委員・部会名簿）ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnPrintCommittee_Click
    '   名称　：印刷（専門委員・部会名簿）ボタンクリック
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnPrintCommittee_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrintCommittee.Click

        Dim clsDb As New CLAccessMdb  ' データベースクラス生成

        Dim fmPrint As New FM000203   '印刷プレビューフォーム
        Dim ds As New DS0302P2        'データセット

        Dim strSql As String

        Dim dtPeriod As DataTable
        Dim dtCommittee As DataTable
        Dim dtModel As DataTable
        Dim dtMemberTeate As DataTable
        Dim dtMemberModel(6) As DataTable

        Dim strPeriodID As String = ""
        Dim strYear As String = ""
        Dim strMonth As String = ""

        Dim strPeriodOmission As String
        Dim strCommitteeID As String
        Dim strCommitteeName As String
        Dim strModelID As String
        Dim strModelName As String
        Dim strModelArray(4) As String
        Dim strStafID As String
        Dim strStafName As String

        Dim intMaxRow As Integer
        Dim blnRptRow As Integer

        Dim strHeaderRow(1) As String  'ヘッダー行配列
        Dim strDetailRow(23) As String '明細行配列

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------
            '   検索項目設定
            '-------------------------------------------------------------------
            ' 期ID
            If Me.cboTerm.SelectedIndex <> -1 Then
                strPeriodID = Me.cboTerm.SelectedValue
            Else
                MsgBox("期が取得できませんでした。")
                Exit Sub
            End If

            ' 対象年月
            strYear = Me.cboYear.SelectedItem
            strMonth = Me.cboMonth.SelectedItem

            ' データベース接続
            Call clsDb.Connect()

            ' 対象年月が適切であるかチェック
            If CheckPeriodKikan(clsDb, strPeriodID, strYear & strMonth & "01") = False Then
                Exit Try
            End If

            '実行の可否を問い合わせる
            If MsgBox("全委員会の名簿データを取得するため、時間がかかります。" & Chr(10) & "実行してよろしいでしょうか。", MsgBoxStyle.OkCancel, "問合せ") = MsgBoxResult.Cancel Then
                Exit Sub
            End If

            '-------------------------------------------------------------------
            '   委員会名取得
            '-------------------------------------------------------------------
            strSql = ""
            strSql = strSql & "SELECT c_committee_id, l_name " & vbCrLf
            strSql = strSql & "FROM committee " & vbCrLf
            strSql = strSql & "WHERE '" & strYear & strMonth & "01' BETWEEN d_from AND d_to" & vbCrLf
            strSql = strSql & "ORDER BY c_committee_id" 'chk

            dtCommittee = clsDb.ExecuteSql(strSql)

            If dtCommittee.Rows.Count = 0 Then
                MsgBox("委員会が取得できませんでした。")
                Exit Sub
            End If

            '-------------------------------------------------------------------
            '   機種名取得
            '-------------------------------------------------------------------
            strSql = "SELECT c_constant_seq, l_name " &
                     "FROM constant_dtl " &
                     "WHERE c_constant = 'MODEL' " &
                     "ORDER BY s_order" 'chk

            dtModel = clsDb.ExecuteSql(strSql)

            If dtModel.Rows.Count = 0 Then
                MsgBox("機種が取得できませんでした。")
                Exit Sub
            End If

            '-------------------------------------------------------------------
            '   期 短縮名取得
            '-------------------------------------------------------------------
            strSql = "SELECT l_omission_name FROM period WHERE c_period_id = '" & strPeriodID & "'"

            dtPeriod = clsDb.ExecuteSql(strSql)

            If dtPeriod.Rows.Count > 0 Then
                strPeriodOmission = dtPeriod.Rows(0).Item(0)
            Else
                MsgBox("期の短縮名が取得できませんでした。")
                Exit Sub
            End If

            '-------------------------------------------------------------------
            '   ヘッダー情報設定
            '-------------------------------------------------------------------
            strHeaderRow(0) = strPeriodOmission
            strHeaderRow(1) = strYear & "/" & strMonth

            Call ds.dtHeader.Rows.Add(strHeaderRow)

            For intModelCnt = 0 To dtModel.Rows.Count - 1 Step 5
                '-------------------------------------------------------------------
                '   明細情報設定
                '-------------------------------------------------------------------
                ' 委員会単位で明細情報を作成する
                For i = 0 To dtCommittee.Rows.Count - 1
                    strCommitteeID = dtCommittee.Rows(i).Item(0)
                    strCommitteeName = dtCommittee.Rows(i).Item(1)

                    If intModelCnt = 0 Then

                        '委員会で役員手当ての対象になっている人を検索
                        strSql = ""
                        strSql = strSql & "SELECT join1.c_user_id," & vbCrLf        '0.個人認証ID
                        strSql = strSql & "       join1.l_name," & vbCrLf           '1.氏名
                        strSql = strSql & "       join1.c_committee_id," & vbCrLf   '2.委員会ID
                        strSql = strSql & "       join1.s_committee_seq," & vbCrLf  '3.役職ID
                        strSql = strSql & "       join2.c_user_id" & vbCrLf         '4.支部委員会所属フラグ（有：値あり/無：値なし）

                        strSql = strSql & "FROM  " & vbCrLf
                        strSql = strSql & "     (SELECT staf_att.c_user_id,staf_att.l_name,com_list.c_committee_id,com_list.s_committee_seq" & vbCrLf
                        strSql = strSql & "      FROM committee_dtl      AS com_dtl," & vbCrLf

                        strSql = strSql & "           (SELECT pay.c_officer_pay_id,pay.s_officer_pay,pay.d_from,pay.d_to" & vbCrLf
                        strSql = strSql & "            FROM   officer_pay_master     AS pay," & vbCrLf

                        strSql = strSql & "                  (SELECT c_officer_pay_id, MAX(d_from) AS now_from" & vbCrLf
                        strSql = strSql & "                   FROM   officer_pay_master" & vbCrLf
                        strSql = strSql & "                   WHERE  s_officer_pay  > 0 " & vbCrLf
                        strSql = strSql & "                   AND    '" & strYear & strMonth & "' BETWEEN left(d_from,6) AND left(d_to,6)" & vbCrLf
                        strSql = strSql & "                   GROUP BY c_officer_pay_id" & vbCrLf
                        strSql = strSql & "                  ) AS t1" & vbCrLf

                        strSql = strSql & "            WHERE pay.c_officer_pay_id  = t1.c_officer_pay_id " & vbCrLf
                        strSql = strSql & "            AND   pay.d_from            = t1.now_from" & vbCrLf
                        strSql = strSql & "           ) AS pay," & vbCrLf

                        strSql = strSql & "           (SELECT com_list.c_period_id,com_list.c_committee_id," & vbCrLf
                        strSql = strSql & "                   com_list_dtl.c_user_id,com_list_dtl.s_committee_seq" & vbCrLf
                        strSql = strSql & "            FROM   committee_list     AS com_list," & vbCrLf
                        strSql = strSql & "                   committee_list_dtl AS com_list_dtl," & vbCrLf

                        strSql = strSql & "                  (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
                        strSql = strSql & "                   FROM committee_list" & vbCrLf
                        strSql = strSql & "                   WHERE left(d_from,6) <= '" & strYear & strMonth & "'" & vbCrLf
                        strSql = strSql & "                   AND   c_committee_id  = '" & strCommitteeID & "'" & vbCrLf
                        strSql = strSql & "                   AND   c_period_id     = '" & strPeriodID & "'" & vbCrLf
                        strSql = strSql & "                   GROUP BY c_committee_id" & vbCrLf
                        strSql = strSql & "                  ) AS t2" & vbCrLf

                        strSql = strSql & "            WHERE com_list.c_committee_id   = t2.c_committee_id " & vbCrLf
                        strSql = strSql & "            AND   com_list.d_from           = t2.now_from" & vbCrLf
                        strSql = strSql & "            AND   com_list.c_committee_list = com_list_dtl.c_committee_list" & vbCrLf
                        strSql = strSql & "           ) AS com_list," & vbCrLf

                        strSql = strSql & "           (SELECT staf_at1.c_user_id,staf_at1.l_name" & vbCrLf
                        strSql = strSql & "            FROM   staf_attribute AS staf_at1," & vbCrLf
                        strSql = strSql & "                  (SELECT   c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
                        strSql = strSql & "                   FROM     staf_attribute " & vbCrLf
                        strSql = strSql & "                   WHERE    left(d_from,6) <= '" & strYear & strMonth & "'" & vbCrLf
                        strSql = strSql & "                   GROUP BY c_user_id, c_ksh, c_staf_id" & vbCrLf
                        strSql = strSql & "                  ) AS t4 " & vbCrLf
                        strSql = strSql & "            WHERE staf_at1.c_user_id = t4.c_user_id " & vbCrLf
                        strSql = strSql & "            AND   staf_at1.c_ksh     = t4.c_ksh " & vbCrLf
                        strSql = strSql & "            AND   staf_at1.d_from    = t4.now_from" & vbCrLf
                        strSql = strSql & "           ) AS staf_att" & vbCrLf

                        strSql = strSql & "      WHERE '" & strYear & strMonth & "' BETWEEN left(com_dtl.d_from,6) AND left(com_dtl.d_to,6)" & vbCrLf
                        strSql = strSql & "      AND   com_list.c_user_id       = staf_att.c_user_id" & vbCrLf
                        strSql = strSql & "      AND   com_dtl.c_officer_pay_id = pay.c_officer_pay_id" & vbCrLf
                        strSql = strSql & "      AND   com_list.c_committee_id  = com_dtl.c_committee_id " & vbCrLf
                        strSql = strSql & "      AND   com_list.s_committee_seq = com_dtl.s_committee_seq" & vbCrLf
                        strSql = strSql & "     ) join1" & vbCrLf

                        strSql = strSql & "     LEFT JOIN" & vbCrLf
                        strSql = strSql & "     (SELECT staf.c_user_id,staf.l_name" & vbCrLf
                        strSql = strSql & "      FROM   committee_list_dtl AS com_list_dtl," & vbCrLf

                        strSql = strSql & "            (SELECT t1.c_committee_list,t1.c_period_id" & vbCrLf
                        strSql = strSql & "             FROM   committee_list AS t1," & vbCrLf
                        strSql = strSql & "                   (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
                        strSql = strSql & "                    FROM committee_list" & vbCrLf
                        strSql = strSql & "                    WHERE left(d_from,6) <= '" & strYear & strMonth & "'" & vbCrLf
                        strSql = strSql & "                    AND   c_period_id     = '" & strPeriodID & "'" & vbCrLf
                        strSql = strSql & "                    AND   c_committee_id  IN ('019', '029')" & vbCrLf
                        strSql = strSql & "                    GROUP BY c_committee_id" & vbCrLf
                        strSql = strSql & "                   ) AS t2" & vbCrLf
                        strSql = strSql & "             WHERE t1.c_committee_id = t2.c_committee_id " & vbCrLf
                        strSql = strSql & "             AND   t1.d_from         = t2.now_from" & vbCrLf
                        strSql = strSql & "            ) AS com_list," & vbCrLf

                        strSql = strSql & "            (SELECT t3.c_staf_id,t3.c_user_id,t3.l_name" & vbCrLf
                        strSql = strSql & "             FROM   staf_attribute AS t3," & vbCrLf
                        strSql = strSql & "                   (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from" & vbCrLf
                        strSql = strSql & "                    FROM   staf_attribute" & vbCrLf
                        strSql = strSql & "                    WHERE  left(d_from,6) <= '" & strYear & strMonth & "'" & vbCrLf
                        strSql = strSql & "                    GROUP  BY c_user_id, c_ksh, c_staf_id" & vbCrLf
                        strSql = strSql & "                   ) AS t4" & vbCrLf
                        strSql = strSql & "             WHERE t3.c_user_id = t4.c_user_id " & vbCrLf
                        strSql = strSql & "             AND   t3.c_ksh     = t4.c_ksh " & vbCrLf
                        strSql = strSql & "             AND   t3.d_from    = t4.now_from" & vbCrLf
                        strSql = strSql & "            ) AS staf " & vbCrLf

                        strSql = strSql & "      WHERE com_list_dtl.c_committee_list = com_list.c_committee_list" & vbCrLf
                        strSql = strSql & "      AND   com_list_dtl.c_user_id    = staf.c_user_id " & vbCrLf
                        strSql = strSql & "     ) join2" & vbCrLf
                        strSql = strSql & "ON join1.c_user_id = join2.c_user_id" & vbCrLf

                        strSql = strSql & "ORDER BY join1.s_committee_seq, CLng(join1.c_user_id)" & UtDb.DbOrderOffset() & vbCrLf   'ok

                        dtMemberTeate = clsDb.ExecuteSql(strSql)
                    Else
                        dtMemberTeate = New DataTable
                    End If

                    '機種単位で委員会に所属する人を検索する
                    For j = 0 To 4
                        strModelArray(j) = ""
                    Next

                    For j = 0 To 4
                        If dtModel.Rows.Count - 1 < intModelCnt + j Then
                            Exit For
                        End If

                        strModelID = dtModel.Rows(intModelCnt + j).Item(0)
                        strModelName = dtModel.Rows(intModelCnt + j).Item(1)
                        strModelArray(j) = strModelName

                        strSql = ""
                        strSql = strSql & "SELECT join1.c_user_id," & vbCrLf        '0.個人認証ID
                        strSql = strSql & "       join1.l_name," & vbCrLf           '1.氏名
                        strSql = strSql & "       join1.c_committee_id," & vbCrLf   '2.委員会ID
                        strSql = strSql & "       join1.s_committee_seq," & vbCrLf  '3.役職ID
                        strSql = strSql & "       join2.c_user_id" & vbCrLf         '4.支部委員会所属フラグ（有：値あり/無：値なし）

                        strSql = strSql & "FROM  " & vbCrLf
                        strSql = strSql & "     (SELECT staf_att.c_user_id,staf_att.l_name,com_list.c_committee_id,com_list.s_committee_seq" & vbCrLf
                        strSql = strSql & "      FROM committee_dtl      AS com_dtl," & vbCrLf

                        strSql = strSql & "           (SELECT pay.c_officer_pay_id,pay.s_officer_pay,pay.d_from,pay.d_to" & vbCrLf
                        strSql = strSql & "            FROM   officer_pay_master     AS pay," & vbCrLf

                        strSql = strSql & "                  (SELECT c_officer_pay_id, MAX(d_from) AS now_from" & vbCrLf
                        strSql = strSql & "                   FROM   officer_pay_master" & vbCrLf
                        strSql = strSql & "                   WHERE  s_officer_pay  > 0 " & vbCrLf
                        strSql = strSql & "                   AND    '" & strYear & strMonth & "' BETWEEN left(d_from,6) AND left(d_to,6)" & vbCrLf
                        strSql = strSql & "                   GROUP BY c_officer_pay_id" & vbCrLf
                        strSql = strSql & "                  ) AS t1" & vbCrLf

                        strSql = strSql & "            WHERE pay.c_officer_pay_id  = t1.c_officer_pay_id " & vbCrLf
                        strSql = strSql & "            AND   pay.d_from            = t1.now_from" & vbCrLf
                        strSql = strSql & "           ) AS pay," & vbCrLf

                        strSql = strSql & "           (SELECT com_list.c_period_id,com_list.c_committee_id," & vbCrLf
                        strSql = strSql & "                   com_list_dtl.c_user_id,com_list_dtl.s_committee_seq" & vbCrLf
                        strSql = strSql & "            FROM   committee_list     AS com_list," & vbCrLf
                        strSql = strSql & "                   committee_list_dtl AS com_list_dtl," & vbCrLf

                        strSql = strSql & "                  (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
                        strSql = strSql & "                   FROM committee_list" & vbCrLf
                        strSql = strSql & "                   WHERE left(d_from,6) <= '" & strYear & strMonth & "'" & vbCrLf
                        strSql = strSql & "                   AND   c_committee_id  = '" & strCommitteeID & "'" & vbCrLf
                        strSql = strSql & "                   AND   c_period_id     = '" & strPeriodID & "'" & vbCrLf
                        strSql = strSql & "                   GROUP BY c_committee_id" & vbCrLf
                        strSql = strSql & "                  ) AS t2" & vbCrLf

                        strSql = strSql & "            WHERE com_list.c_committee_id   = t2.c_committee_id " & vbCrLf
                        strSql = strSql & "            AND   com_list.d_from           = t2.now_from" & vbCrLf
                        strSql = strSql & "            AND   com_list.c_committee_list = com_list_dtl.c_committee_list" & vbCrLf
                        strSql = strSql & "           ) AS com_list," & vbCrLf

                        strSql = strSql & "           (SELECT staf_at1.c_user_id,staf_at1.l_name" & vbCrLf
                        strSql = strSql & "            FROM   staf_attribute AS staf_at1," & vbCrLf
                        strSql = strSql & "                  (SELECT   c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
                        strSql = strSql & "                   FROM     staf_attribute " & vbCrLf
                        strSql = strSql & "                   WHERE    left(d_from,6) <= '" & strYear & strMonth & "'" & vbCrLf
                        strSql = strSql & "                   GROUP BY c_user_id, c_ksh, c_staf_id" & vbCrLf
                        strSql = strSql & "                  ) AS t4 " & vbCrLf
                        strSql = strSql & "            WHERE staf_at1.c_user_id = t4.c_user_id " & vbCrLf
                        strSql = strSql & "            AND   staf_at1.c_ksh     = t4.c_ksh " & vbCrLf
                        strSql = strSql & "            AND   staf_at1.d_from    = t4.now_from" & vbCrLf
                        strSql = strSql & "            AND   staf_at1.k_model   = '" & strModelID & "'" & vbCrLf
                        strSql = strSql & "           ) AS staf_att" & vbCrLf

                        strSql = strSql & "      WHERE '" & strYear & strMonth & "' BETWEEN left(com_dtl.d_from,6) AND left(com_dtl.d_to,6)" & vbCrLf
                        strSql = strSql & "      AND   com_list.c_user_id       = staf_att.c_user_id" & vbCrLf
                        strSql = strSql & "      AND   com_dtl.c_officer_pay_id = pay.c_officer_pay_id" & vbCrLf
                        strSql = strSql & "      AND   com_list.c_committee_id  = com_dtl.c_committee_id " & vbCrLf
                        strSql = strSql & "      AND   com_list.s_committee_seq = com_dtl.s_committee_seq" & vbCrLf
                        strSql = strSql & "     ) join1" & vbCrLf

                        strSql = strSql & "     LEFT JOIN" & vbCrLf
                        strSql = strSql & "     (SELECT staf.c_user_id,staf.l_name" & vbCrLf
                        strSql = strSql & "      FROM   committee_list_dtl AS com_list_dtl," & vbCrLf

                        strSql = strSql & "            (SELECT t1.c_committee_list,t1.c_period_id" & vbCrLf
                        strSql = strSql & "             FROM   committee_list AS t1," & vbCrLf
                        strSql = strSql & "                   (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
                        strSql = strSql & "                    FROM committee_list" & vbCrLf
                        strSql = strSql & "                    WHERE left(d_from,6) <= '" & strYear & strMonth & "'" & vbCrLf
                        strSql = strSql & "                    AND   c_period_id     = '" & strPeriodID & "'" & vbCrLf
                        strSql = strSql & "                    AND   c_committee_id  IN ('019', '029')" & vbCrLf
                        strSql = strSql & "                    GROUP BY c_committee_id" & vbCrLf
                        strSql = strSql & "                   ) AS t2" & vbCrLf
                        strSql = strSql & "             WHERE t1.c_committee_id = t2.c_committee_id " & vbCrLf
                        strSql = strSql & "             AND   t1.d_from         = t2.now_from" & vbCrLf
                        strSql = strSql & "            ) AS com_list," & vbCrLf

                        strSql = strSql & "            (SELECT t3.c_staf_id,t3.c_user_id,t3.l_name" & vbCrLf
                        strSql = strSql & "             FROM   staf_attribute AS t3," & vbCrLf
                        strSql = strSql & "                   (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from" & vbCrLf
                        strSql = strSql & "                    FROM   staf_attribute" & vbCrLf
                        strSql = strSql & "                    WHERE  left(d_from,6) <= '" & strYear & strMonth & "'" & vbCrLf
                        strSql = strSql & "                    GROUP  BY c_user_id, c_ksh, c_staf_id" & vbCrLf
                        strSql = strSql & "                   ) AS t4" & vbCrLf
                        strSql = strSql & "             WHERE t3.c_user_id = t4.c_user_id " & vbCrLf
                        strSql = strSql & "             AND   t3.c_ksh     = t4.c_ksh " & vbCrLf
                        strSql = strSql & "             AND   t3.d_from    = t4.now_from" & vbCrLf
                        strSql = strSql & "            ) AS staf " & vbCrLf

                        strSql = strSql & "      WHERE com_list_dtl.c_committee_list = com_list.c_committee_list" & vbCrLf
                        strSql = strSql & "      AND   com_list_dtl.c_user_id    = staf.c_user_id " & vbCrLf
                        strSql = strSql & "     ) join2" & vbCrLf
                        strSql = strSql & "ON join1.c_user_id = join2.c_user_id" & vbCrLf

                        strSql = strSql & "ORDER BY join1.s_committee_seq, CLng(join1.c_user_id)" & UtDb.DbOrderOffset() & vbCrLf   'ok

                        dtMemberModel(j) = clsDb.ExecuteSql(strSql)

                    Next

                    blnRptRow = False
                    If dtMemberTeate.Rows.Count > 0 Then
                        blnRptRow = True
                    Else
                        For j = 0 To 4
                            If dtMemberModel(j).Rows.Count > 0 Then
                                blnRptRow = True
                                Exit For
                            End If
                        Next
                    End If

                    If blnRptRow = True Then
                        '役員手当て、各機種の中で、最も人数(出力行)の多いものは？
                        If dtMemberTeate.Rows.Count + 1 < dtMemberModel(0).Rows.Count Then
                            intMaxRow = dtMemberModel(0).Rows.Count
                        Else
                            intMaxRow = dtMemberTeate.Rows.Count + 1
                        End If

                        For j = 1 To 4
                            If intMaxRow < dtMemberModel(j).Rows.Count Then
                                intMaxRow = dtMemberModel(j).Rows.Count
                            End If
                        Next

                        '明細情報の作成 (出力行が最も多いものに合わせて行を作成)
                        For j = 0 To intMaxRow - 1
                            '配列を初期化
                            For k = 0 To UBound(strDetailRow)
                                strDetailRow(k) = ""
                            Next

                            '委員会名 (グループキーの為、必ず設定)
                            strDetailRow(0) = strCommitteeName

                            '役員手当て対象者
                            If j <> 0 And dtMemberTeate.Rows.Count >= j Then '出力対象の行がまだあるときは処理

                                strStafID = dtMemberTeate.Rows(j - 1).Item(0)   '社員番号
                                strStafName = dtMemberTeate.Rows(j - 1).Item(1) '氏名

                                '支部委員会に所属しているか？
                                If dtMemberTeate.Rows(j - 1).Item(4) Is DBNull.Value Then
                                    strDetailRow(1) = "*" '所属してなければ「*」を付ける
                                Else
                                    strDetailRow(1) = ""
                                End If

                                strDetailRow(2) = strStafID   '社員番号
                                strDetailRow(3) = strStafName '氏名
                            End If

                            '各機種
                            For k = 0 To 4
                                If dtMemberModel(k).Rows.Count - 1 >= j Then '出力対象の行がまだあるか？
                                    strStafID = dtMemberModel(k).Rows(j).Item(0)   '社員番号
                                    strStafName = dtMemberModel(k).Rows(j).Item(1) '氏名

                                    '支部委員会に所属しているか？
                                    If dtMemberModel(k).Rows(j).Item(4) Is DBNull.Value Then
                                        strDetailRow(5 + k * 4) = "*" '所属してなければ「*」を付ける
                                    Else
                                        strDetailRow(5 + k * 4) = ""
                                    End If

                                    '機種の値が空欄でないか？
                                    If strModelArray(k) <> "" Then
                                        strDetailRow(4 + k * 4) = strModelArray(k) '機種
                                        strDetailRow(6 + k * 4) = strStafID        '社員番号
                                        strDetailRow(7 + k * 4) = strStafName      '氏名
                                    Else
                                        strDetailRow(4 + k * 4) = strModelArray(k) '機種
                                        strDetailRow(5 + k * 4) = ""               '*印
                                        strDetailRow(6 + k * 4) = ""               '社員番号
                                        strDetailRow(7 + k * 4) = ""               '氏名
                                    End If

                                Else
                                    strDetailRow(4 + k * 4) = strModelArray(k) '機種 (改ページキーになっているので、出力対象の行がなくても出力する)
                                End If
                            Next

                            'データソースのデータテーブルに行を追加
                            Call ds.dtDetail.Rows.Add(strDetailRow)
                        Next
                    End If
                Next
            Next

            '印刷プレビュー準備
            fmPrint.ButtonShowType = 3         '印刷、キャンセルボタン
            fmPrint.PrintCntVisible = False    '印刷部数非表示
            fmPrint.ObjResource = New CR0302P2 '帳票インスタンスを作成

            fmPrint.ObjResource.SetDataSource(ds) 'データセットをセット

            'フォームを表示
            Call fmPrint.ShowDialog()

            Select Case fmPrint.IntQlickBtnFlag
                Case 1
                Case 2
                    'キャンセル
                Case 3
                    '印刷
                    fmPrint.PrintOut()
            End Select

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "btnPrintCommittee_Click")

            log.Fatal(ex.Message)
        Finally
            'データベースから切断
            Call clsDb.Disconnect()

            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region
#End Region

#Region "コンボボックス選択肢変更"
#Region "期"

    Private Sub cboPeriod_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboPeriod.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' データ検索
                Call Me.GetSearchData()

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "cboPeriod_KeyPress")

                log.Fatal(ex.Message)
            Finally
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try

        End If
    End Sub
    '***************************************************************************************************
    '   ＩＤ　：cboPeriod_SelectionChangeCommitted
    '   名称　：期コンボボックス選択肢変更
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuk
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboPeriod_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectionChangeCommitted
        Dim blnRet As Boolean = False   ' 処理結果
        Dim clsDb As New CLAccessMdb    ' データベースクラス生成

        Try
            ' 検索結果グリッドを非表示
            Me.grpResult.Visible = False

            ' データベース接続
            Call clsDb.Connect()

            ' 委員会選択コンボボックス作成処理呼び出し
            Call Me.CreateCboCommittee(clsDb, Me.cboCommittee, False, 2, Me.cboPeriod.SelectedValue)
            cboCommittee.SelectedIndex = 0

            ' 年選択コンボボックス作成処理呼び出し
            Call Me.CreateCboYearSearch(clsDb, Me.cboYearSearch, False, 2, Me.cboPeriod.SelectedValue)
            cboYearSearch.SelectedIndex = 0

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "cboPeriod_SelectionChangeCommitted")
        Finally
            Call clsDb.Disconnect()
        End Try
    End Sub
#End Region

#Region "部・委員会"

    Private Sub cboCommittee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboCommittee.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' データ検索
                Call Me.GetSearchData()

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "cboCommittee_KeyPress")

                log.Fatal(ex.Message)
            Finally
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try

        End If


    End Sub
    Private Sub cboCommittee_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCommittee.SelectionChangeCommitted

        ' 検索結果グリッドを非表示
        Me.grpResult.Visible = False

    End Sub
#End Region

#Region "年"

    Private Sub cboYearSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboYearSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' データ検索
                Call Me.GetSearchData()

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "cboYearSearch_KeyPress")

                log.Fatal(ex.Message)
            Finally
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try

        End If

    End Sub
    Private Sub cboYearSearch_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYearSearch.SelectionChangeCommitted

        ' 検索結果グリッドを非表示
        Me.grpResult.Visible = False

    End Sub
#End Region

#Region "月"

    Private Sub cboMonthSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboMonthSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' データ検索
                Call Me.GetSearchData()

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "cboMonthSearch_KeyPress")

                log.Fatal(ex.Message)
            Finally
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try

        End If


    End Sub
    Private Sub cboMonthSearch_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMonthSearch.SelectionChangeCommitted

        ' 検索結果グリッドを非表示
        Me.grpResult.Visible = False

    End Sub
#End Region

#Region "支部"

    Private Sub cboUnionBranchSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboUnionBranchSearch.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' データ検索
                Call Me.GetSearchData()

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "cboUnionBranchSearch_KeyPress")

                log.Fatal(ex.Message)
            Finally
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try

        End If

    End Sub
    Private Sub cboUnionBranchSearch_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboUnionBranchSearch.SelectionChangeCommitted

        ' 検索結果グリッドを非表示
        Me.grpResult.Visible = False

    End Sub
#End Region

#Region "期(専門委員・部会名簿印刷)"
    '***************************************************************************************************
    '   ＩＤ　：cboTerm_SelectionChangeCommitted
    '   名称　：期コンボボックス選択肢変更
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboTerm_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboTerm.SelectionChangeCommitted
        Dim blnRet As Boolean = False   ' 処理結果
        Dim clsDb As New CLAccessMdb       ' データベースクラス生成

        Try
            ' データベース接続
            Call clsDb.Connect()

            ' 年選択コンボボックス作成処理呼び出し
            Call Me.CreateCboYearSearch(clsDb, Me.cboYear, False, 2, Me.cboTerm.SelectedValue)
            cboYear.SelectedIndex = 0

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "cboTerm_SelectionChangeCommitted")
        Finally
            Call clsDb.Disconnect()
        End Try
    End Sub
#End Region

#End Region
#End Region

#Region "関数"

#Region "検索データ取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetSearchData
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>検索データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSearchData() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル

        Dim intRetCnt As Integer = 0                ' 検索結果件数
        Dim strMemberNo As String = ""              ' 検索項目　社員番号
        Dim strNameKana As String = ""              ' 検索項目　名前(半角ｶﾅ)
        Dim strKindValue As String = ""             ' 検索項目　組合員種別コード

        Dim strPeriodID As String = ""
        Dim strCommitteeID As String = ""
        Dim strYearSearch As String = ""
        Dim strMonthSearch As String = ""
        Dim strStandDate As String = ""
        Dim strUnionBranch As String = ""

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            '-------------------------------------------------------------------
            '   検索項目設定
            '-------------------------------------------------------------------
            ' 期ID
            If Me.cboPeriod.SelectedIndex <> -1 Then
                strPeriodID = Me.cboPeriod.SelectedValue
            End If

            ' 部／委員会
            If Me.cboCommittee.SelectedIndex <> -1 Then
                strCommitteeID = Me.cboCommittee.SelectedValue
            End If

            ' 対象年月
            strYearSearch = Me.cboYearSearch.SelectedItem
            strMonthSearch = Me.cboMonthSearch.SelectedItem
            strStandDate = strYearSearch & strMonthSearch & "01"

            ' 組合支部
            If Me.cboUnionBranchSearch.SelectedIndex > 0 Then
                strUnionBranch = Me.cboUnionBranchSearch.SelectedValue
            End If

            ' 対象年月が適切であるかチェック
            If ChkTargetDate(strPeriodID, strCommitteeID, strYearSearch & strMonthSearch & "01") = False Then
                blnRet = False
                Exit Try
            End If

            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = ""
            strSql = strSql + "SELECT staf.c_user_id AS 社員番号," & vbCrLf
            strSql = strSql + "       staf.l_name    AS 氏名," & vbCrLf
            strSql = strSql + "       cons1.l_name   AS 支部," & vbCrLf
            strSql = strSql + "       com_dtl.l_name AS 役職," & vbCrLf
            strSql = strSql + "       cons2.l_name   AS 機種," & vbCrLf
            strSql = strSql + "       cons3.l_omission_name AS 資格" & vbCrLf

            strSql = strSql + "FROM   committee_list_dtl AS com_list_dtl," & vbCrLf
            strSql = strSql + "       committee_dtl AS com_dtl," & vbCrLf
            strSql = strSql + "       constant_dtl  AS cons1," & vbCrLf
            strSql = strSql + "       constant_dtl  AS cons2," & vbCrLf
            strSql = strSql + "       constant_dtl  AS cons3," & vbCrLf

            strSql = strSql + "       (SELECT t1.*" & vbCrLf
            strSql = strSql + "        FROM committee_list AS t1," & vbCrLf
            strSql = strSql + "             (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
            strSql = strSql + "              FROM committee_list" & vbCrLf
            strSql = strSql + "              WHERE left(d_from,6) <= '" & strStandDate & "'" & vbCrLf
            strSql = strSql + "              GROUP BY c_committee_id" & vbCrLf
            strSql = strSql + "             ) AS t2" & vbCrLf
            strSql = strSql + "        WHERE t1.c_committee_id = t2.c_committee_id " & vbCrLf
            strSql = strSql + "        AND   t1.d_from         = t2.now_from" & vbCrLf
            strSql = strSql + "       ) AS com_list," & vbCrLf

            strSql = strSql + "       (SELECT t3.*" & vbCrLf
            strSql = strSql + "        FROM staf_attribute AS t3," & vbCrLf
            strSql = strSql + "            (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
            strSql = strSql + "             FROM staf_attribute " & vbCrLf
            strSql = strSql + "             WHERE left(d_from,6) <= '" & strStandDate & "'" & vbCrLf
            strSql = strSql + "             GROUP BY c_user_id, c_ksh, c_staf_id" & vbCrLf
            strSql = strSql + "            ) AS t4" & vbCrLf
            strSql = strSql + "        WHERE t3.c_user_id = t4.c_user_id " & vbCrLf
            strSql = strSql + "        AND   t3.c_ksh     = t4.c_ksh " & vbCrLf
            strSql = strSql + "        AND   t3.d_from    = t4.now_from" & vbCrLf
            strSql = strSql + "       ) AS staf" & vbCrLf

            strSql = strSql + "WHERE com_list.c_period_id         = '" & strPeriodID & " '" & vbCrLf
            strSql = strSql + "AND   com_list_dtl.c_committee_id  = '" & strCommitteeID & "'" & vbCrLf
            strSql = strSql + "AND   com_list.c_committee_list    = com_list_dtl.c_committee_list" & vbCrLf
            strSql = strSql + "AND   com_list_dtl.c_user_id       = staf.c_user_id " & vbCrLf
            strSql = strSql + "AND   cons1.c_constant             = 'AREA_LOCAL'" & vbCrLf
            strSql = strSql + "AND   cons1.c_constant_seq         = staf.k_belonging" & vbCrLf

            '所属支部が選択されているか？
            If strUnionBranch <> "" Then
                strSql = strSql & "  AND staf.k_belonging            = '" & strUnionBranch & "'" & vbCrLf '所属支部ID
            End If

            strSql = strSql + "AND   com_list_dtl.c_committee_id  = com_dtl.c_committee_id " & vbCrLf
            strSql = strSql + "AND   com_list_dtl.s_committee_seq = com_dtl.s_committee_seq" & vbCrLf
            strSql = strSql + "AND   com_dtl.d_from              <= '" & strStandDate & "' " & vbCrLf
            strSql = strSql + "AND   com_dtl.d_to                >= '" & strStandDate & "'" & vbCrLf
            strSql = strSql + "AND   cons2.c_constant             = 'MODEL' " & vbCrLf
            strSql = strSql + "AND   cons2.c_constant_seq         = staf.k_model" & vbCrLf
            strSql = strSql + "AND   cons3.c_constant             = 'QUALIFICATION' " & vbCrLf
            strSql = strSql + "AND   cons3.c_constant_seq         = staf.k_qualification" & vbCrLf

            strSql = strSql + "ORDER BY com_list_dtl.s_committee_seq, staf.k_belonging, CLng(staf.c_user_id) " & UtDb.DbOrderOffset() & vbCrLf  'ok

            ' データベース接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' DataGridViewにデータテーブルをセット
            dgdResult.DataSource = tbRet

            ' DataGridViewのスタイルを設定
            Call Me.SetDataGridStyle(dgdResult)

            ' 件数チェック
            If intRetCnt > 0 Then
                ' 1件以上の処理

                ' グループボックス件数設定
                Me.grpResult.Text = "検索結果（ " & intRetCnt & " 件 ）"

                Me.grpResult.Visible = True                              ' グループボックス表示

                ' 処理結果に正常を設定
                blnRet = True
            Else
                ' 0件の処理
                Me.grpResult.Visible = False                              ' グループボックス表示
                CLMsg.Show("DI0001")                                      ' 対象データなしメッセージボックス表示

                blnRet = False
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "GetSearchData")

            log.Fatal(ex.Message)
        Finally
            ' データベース切断
            clsDb.Disconnect()
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値設定
        Return blnRet
    End Function
#End Region

#Region "期選択コンボボックス作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateCboPeriod
    '   名称　：期選択コンボボックス作成
    '   概要  ：期マスタからすべての期を取得し、コンボボックスの選択肢を作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function CreateCboPeriod(ByVal pClsDb As CLAccessMdb,
                                             ByVal pCboObj As System.Windows.Forms.ComboBox,
                                             Optional ByVal pBlnFirstItem As Boolean = True,
                                             Optional ByVal pBytComboBoxStyle As Byte = 2) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim dtRet As DataTable = Nothing        ' データテーブル
        Dim dtRetNow As DataTable = Nothing
        Dim dtBlank As DataRow = Nothing        ' データロー
        Dim i As Integer

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' 初期処理
            pCboObj.BeginUpdate()                                      ' チラつき防止の為、最後まで描写しない
            pCboObj.DataSource = Nothing                               ' データソース初期化
            pCboObj.Items.Clear()                                      ' コンボボックスリストクリア

            ' SQL文
            strSql = ""
            strSql = strSql & "   SELECT c_period_id" & vbCrLf
            strSql = strSql & "         ,l_name" & vbCrLf
            strSql = strSql & "     FROM period" & vbCrLf
            strSql = strSql & " ORDER BY c_period_id"   'chk

            ' SQL実行
            dtRet = pClsDb.ExecuteSql(strSql)

            ' 0件チェック
            If dtRet.Rows.Count = 0 Then
                Return blnRet
            End If

            ' 先頭空白
            If pBlnFirstItem = True Then
                dtBlank = dtRet.NewRow()
                dtRet.Rows.InsertAt(dtBlank, 0)
            End If

            pCboObj.DropDownStyle = pBytComboBoxStyle                   ' 外観機能（Simple, DropDown, DropDownList）
            pCboObj.DataSource = dtRet                                  ' データソース設定
            pCboObj.DisplayMember = "l_name"                            ' コンボボックス名称設定
            pCboObj.ValueMember = "c_period_id"                         ' コンボボックス値設定
            pCboObj.SelectedIndex = -1                                  ' 未選択

            If pBlnFirstItem = False Then
                strSql = ""
                strSql = strSql & "SELECT c_period_id" & vbCrLf
                strSql = strSql & "FROM period" & vbCrLf
                strSql = strSql & "WHERE FORMAT(GETDATE(), 'yyyyMMdd') BETWEEN d_from AND d_to" & vbCrLf
                strSql = strSql & "ORDER BY c_period_id"    'chk

                ' SQL実行
                dtRetNow = pClsDb.ExecuteSql(strSql)

                ' 0件チェック
                If dtRet.Rows.Count > 0 Then
                    For i = 0 To dtRet.Rows.Count - 1
                        If dtRet.Rows(i).Item(0) = dtRetNow.Rows(0).Item(0) Then
                            Exit For
                        End If
                    Next

                    pCboObj.SelectedIndex = i
                End If
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            pCboObj.DataSource = Nothing                                ' コンボボックスデータソース削除
            pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_MDCOMMON, SCREEN_NAME_UC030201, "CreateCboPeriod")

            log.Fatal(ex.Message)
        Finally
            pCboObj.EndUpdate()                                         ' チラつき防止の為、最後に描写する
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet
    End Function
#End Region

#Region "年選択コンボボックス作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateCboYearSearch
    '   名称　：年選択コンボボックス作成
    '   概要  ：期マスタから指定された期の年を取得し、コンボボックスの選択肢を作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function CreateCboYearSearch(ByVal pClsDb As CLAccessMdb,
                                             ByVal pCboObj As System.Windows.Forms.ComboBox,
                                             Optional ByVal pBlnFirstItem As Boolean = True,
                                             Optional ByVal pBytComboBoxStyle As Byte = 2,
                                             Optional ByVal pStrPeriodId As String = "") As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim dtRet As DataTable = Nothing        ' データテーブル
        Dim dtBlank As DataRow = Nothing        ' データロー

        Dim intFrom As Integer
        Dim intTo As Integer
        Dim intCnt As Integer

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' 初期処理
            pCboObj.BeginUpdate()                                      ' チラつき防止の為、最後まで描写しない
            pCboObj.DataSource = Nothing                               ' データソース初期化
            pCboObj.Items.Clear()                                      ' コンボボックスリストクリア

            If pStrPeriodId = "" Then
                pStrPeriodId = MDLoginInfo.PeriodId
            End If

            ' SQL文
            strSql = ""
            strSql = strSql & "   SELECT d_from" & vbCrLf
            strSql = strSql & "         ,d_to" & vbCrLf
            strSql = strSql & "     FROM period" & vbCrLf
            strSql = strSql & "     WHERE c_period_id = '" & pStrPeriodId & "'"

            ' SQL実行
            dtRet = pClsDb.ExecuteSql(strSql)

            ' 0件チェック
            If dtRet.Rows.Count = 0 Then
                Return blnRet
            End If

            intFrom = CInt(Mid(dtRet.Rows(0).Item(0), 1, 4))
            intTo = CInt(Mid(dtRet.Rows(0).Item(1), 1, 4))

            intCnt = 0
            For i = intFrom To intTo
                Call pCboObj.Items.Add(CStr(i))
            Next

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            pCboObj.DataSource = Nothing                                ' コンボボックスデータソース削除
            pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "CreateCboYearSearch")

            log.Fatal(ex.Message)
        Finally
            pCboObj.EndUpdate()                                         ' チラつき防止の為、最後に描写する
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet
    End Function
#End Region

#Region "委員会選択コンボボックス作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateCboCommittee
    '   名称　：委員会選択コンボボックス作成
    '   概要  ：委員会マスタから指定された日で有効な委員会を取得し、コンボボックスの選択肢を作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function CreateCboCommittee(ByVal pClsDb As CLAccessMdb,
                                        ByVal pCboObj As System.Windows.Forms.ComboBox,
                                        Optional ByVal pBlnFirstItem As Boolean = True,
                                        Optional ByVal pBytComboBoxStyle As Byte = 2,
                                        Optional ByVal pStrPeriodID As String = "") As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL文
        Dim dtRet As DataTable = Nothing        ' データテーブル
        Dim dtBlank As DataRow = Nothing        ' データロー

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            If pStrPeriodID = "" Then
                pStrPeriodID = MDLoginInfo.PeriodId
            End If

            ' 初期処理
            pCboObj.BeginUpdate()                                      ' チラつき防止の為、最後まで描写しない
            pCboObj.DataSource = Nothing                               ' データソース初期化
            pCboObj.Items.Clear()                                      ' コンボボックスリストクリア

            ' SQL文
            strSql = ""
            strSql = strSql & "SELECT c_committee_id, l_name" & vbCrLf
            strSql = strSql & "FROM committee t1" & vbCrLf
            strSql = strSql & "WHERE EXISTS" & vbCrLf
            strSql = strSql & "  (SELECT * FROM period t2" & vbCrLf
            strSql = strSql & "   WHERE c_period_id = '" & pStrPeriodID & "' AND" & vbCrLf
            strSql = strSql & "         NOT ((t1.d_from < t2.d_from AND t1.d_to < t2.d_from) OR" & vbCrLf
            strSql = strSql & "              (t1.d_from > t2.d_to   AND t1.d_to > t2.d_to))" & vbCrLf
            strSql = strSql & "  )" & vbCrLf
            strSql = strSql & "GROUP BY t1.c_committee_id, t1.l_name" & vbCrLf
            strSql = strSql & "ORDER BY t1.c_committee_id"  'chk

            ' SQL実行
            dtRet = pClsDb.ExecuteSql(strSql)

            ' 0件チェック
            If dtRet.Rows.Count = 0 Then
                Return blnRet
            End If

            ' 先頭空白
            If pBlnFirstItem Then
                dtBlank = dtRet.NewRow()
                dtRet.Rows.InsertAt(dtBlank, 0)
            End If

            pCboObj.DropDownStyle = pBytComboBoxStyle                   ' 外観機能（Simple, DropDown, DropDownList）
            pCboObj.DataSource = dtRet                                  ' データソース設定
            pCboObj.DisplayMember = "l_name"                            ' コンボボックス名称設定
            pCboObj.ValueMember = "c_committee_id"                      ' コンボボックス値設定
            pCboObj.SelectedIndex = -1                                  ' 未選択

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            pCboObj.DataSource = Nothing                                ' コンボボックスデータソース削除
            pCboObj.Items.Clear()                                       ' コンボボックスリストクリア
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "CreateCboCommittee")

            log.Fatal(ex.Message)
        Finally
            pCboObj.EndUpdate()                                         ' チラつき防止の為、最後に描写する
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet
    End Function
#End Region

#Region "支部委員会所属確認"
    '***************************************************************************************************
    '   ＩＤ　：IsShibuMember
    '   名称　：支部委員会所属確認
    '   概要  ：指定された組合員が支部委員会に所属しているかを確認する
    '   引数　：なし
    '   戻り値：True = 所属している, False = 所属していない
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function IsShibuMember(ByVal pClsDb As CLAccessMdb,
                                   ByVal pStrStafID As String,
                                   ByVal pStrPeriodID As String,
                                   ByVal pStrDate As String) As Boolean
        Dim strSql As String
        Dim dtShibu As DataTable
        Dim blnRet As Boolean = False

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            strSql = ""
            strSql = strSql & "SELECT staf.c_user_id,staf.l_name " & vbCrLf

            strSql = strSql & "FROM   committee_list_dtl AS com_list_dtl," & vbCrLf

            strSql = strSql & "      (SELECT t1.c_committee_list,t1.c_period_id" & vbCrLf
            strSql = strSql & "       FROM   committee_list AS t1," & vbCrLf
            strSql = strSql & "             (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
            strSql = strSql & "              FROM committee_list" & vbCrLf
            strSql = strSql & "              WHERE left(d_from,6) <= '" & pStrDate & "'" & vbCrLf
            strSql = strSql & "              AND   c_period_id     = '" & pStrPeriodID & "'" & vbCrLf
            strSql = strSql & "              AND   c_committee_id  IN ('019', '029')" & vbCrLf
            strSql = strSql & "              GROUP BY c_committee_id" & vbCrLf
            strSql = strSql & "             ) AS t2" & vbCrLf
            strSql = strSql & "       WHERE t1.c_committee_id = t2.c_committee_id " & vbCrLf
            strSql = strSql & "       AND   t1.d_from         = t2.now_from" & vbCrLf
            strSql = strSql & "      ) AS com_list," & vbCrLf

            strSql = strSql & "      (SELECT t3.c_staf_id,t3.c_user_id,t3.l_name" & vbCrLf
            strSql = strSql & "       FROM   staf_attribute AS t3," & vbCrLf
            strSql = strSql & "             (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from" & vbCrLf
            strSql = strSql & "              FROM   staf_attribute" & vbCrLf
            strSql = strSql & "              WHERE  left(d_from,6) <= '" & pStrDate & "'" & vbCrLf
            strSql = strSql & "              AND    c_staf_id       = '" & pStrStafID & "'" & vbCrLf
            strSql = strSql & "              GROUP  BY c_user_id, c_ksh, c_staf_id" & vbCrLf
            strSql = strSql & "             ) AS t4" & vbCrLf
            strSql = strSql & "       WHERE t3.c_user_id = t4.c_user_id " & vbCrLf
            strSql = strSql & "       AND   t3.c_ksh     = t4.c_ksh " & vbCrLf
            strSql = strSql & "       AND   t3.d_from    = t4.now_from" & vbCrLf
            strSql = strSql & "      ) AS staf " & vbCrLf

            strSql = strSql & "WHERE com_list.c_committee_list = com_list_dtl.c_committee_list" & vbCrLf

            strSql = strSql & "AND   com_list_dtl.c_user_id    = staf.c_user_id " & vbCrLf





            'strSql = ""
            'strSql = strSql & "SELECT staf.c_user_id, " & vbCrLf
            'strSql = strSql & "       staf.l_name " & vbCrLf

            'strSql = strSql & "FROM ((SELECT *" & vbCrLf
            'strSql = strSql & "      FROM committee_list AS t1" & vbCrLf
            'strSql = strSql & "      WHERE EXISTS" & vbCrLf
            'strSql = strSql & "              (SELECT * FROM" & vbCrLf
            'strSql = strSql & "                 (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
            'strSql = strSql & "                  FROM committee_list" & vbCrLf
            'strSql = strSql & "                  WHERE d_from <= '" & pStrDate & "'" & vbCrLf
            'strSql = strSql & "                  GROUP BY c_committee_id) AS t2" & vbCrLf
            'strSql = strSql & "               WHERE t1.c_committee_id = t2.c_committee_id AND"
            'strSql = strSql & "                     t1.d_from         = t2.now_from)) AS com_list" & vbCrLf '委員会名簿を基準日で検索したテーブル

            'strSql = strSql & "INNER JOIN committee_list_dtl AS com_list_dtl" & vbCrLf '委員会名簿明細
            'strSql = strSql & "        ON com_list.c_committee_list = com_list_dtl.c_committee_list) " & vbCrLf

            'strSql = strSql & "INNER JOIN (SELECT * " & vbCrLf
            'strSql = strSql & "        FROM staf_attribute AS t3 " & vbCrLf
            'strSql = strSql & "        WHERE EXISTS " & vbCrLf
            'strSql = strSql & "          (SELECT * FROM " & vbCrLf
            'strSql = strSql & "             (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
            'strSql = strSql & "              FROM staf_attribute " & vbCrLf
            'strSql = strSql & "              WHERE d_from <= '" & pStrDate & "'" & vbCrLf
            'strSql = strSql & "              GROUP BY c_user_id, c_ksh, c_staf_id) AS t4 " & vbCrLf
            'strSql = strSql & "           WHERE t3.c_user_id = t4.c_user_id " & vbCrLf
            'strSql = strSql & "             AND t3.c_ksh     = t4.c_ksh " & vbCrLf
            'strSql = strSql & "             AND t3.d_from    = t4.now_from)) AS staf " & vbCrLf '組合員基本情報を基準日で検索したテーブル
            'strSql = strSql & "        ON com_list_dtl.c_user_id = staf.c_user_id " & vbCrLf

            'strSql = strSql & "WHERE com_list.c_period_id = '" & pStrPeriodID & "'" & vbCrLf '期ID
            'strSql = strSql & "AND   com_list.c_committee_id IN ('019', '029')" & vbCrLf     '委員会IDが019(支部委員会(東京))か029(支部委員会(大阪))
            'strSql = strSql & "AND   staf.c_staf_id = '" & pStrStafID & "'"
            'SQL実行
            dtShibu = pClsDb.ExecuteSql(strSql)

            '該当するレコードが存在したか？
            If dtShibu.Rows.Count > 0 Then
                blnRet = True
            Else
                blnRet = False
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "IsShibuMember")

            log.Fatal(ex.Message)
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet
    End Function
#End Region

#Region "検索結果 FlexGridView スタイル設定"
    '***************************************************************************************************
    '   ＩＤ　：SetDataGridStyle
    '   名称　：検索結果 FlexGridView スタイル設定
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub SetDataGridStyle(ByVal PdgdGrid As DataGridView)

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        PdgdGrid.Columns(0).Width = COLUMN_WIDTH_0
        PdgdGrid.Columns(1).Width = COLUMN_WIDTH_1
        PdgdGrid.Columns(2).Width = COLUMN_WIDTH_2
        PdgdGrid.Columns(3).Width = COLUMN_WIDTH_3
        PdgdGrid.Columns(4).Width = COLUMN_WIDTH_4
        PdgdGrid.Columns(5).Width = COLUMN_WIDTH_5

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "対象年月チェック"
    '***************************************************************************************************
    '   ＩＤ　：CheckPeriodKikan
    '   名称　：対象年月チェック
    '   概要  ：指定された日付が指定された期の期間内かチェックする。
    '   引数　：なし
    '   戻り値：True = 期間内, False = 期間内でない
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function CheckPeriodKikan(ByVal PclsDb As CLAccessMdb, ByVal PstrPeriodID As String, ByVal PstrDate As String) As Boolean
        Dim strSql As String
        Dim dtRet As DataTable
        Dim blnRet As Boolean = False
        Dim strFrom As String
        Dim strTo As String

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            strSql = ""
            strSql = strSql & "SELECT d_from, d_to" & vbCrLf
            strSql = strSql & "FROM period" & vbCrLf
            strSql = strSql & "WHERE c_period_id = '" & PstrPeriodID & "'"

            'SQL実行
            dtRet = PclsDb.ExecuteSql(strSql)

            '該当するレコードが存在したか？
            If dtRet.Rows.Count > 0 Then
                blnRet = True

                strFrom = dtRet.Rows(0).Item("d_from")
                strTo = dtRet.Rows(0).Item("d_to")

                If strFrom <= PstrDate And PstrDate <= strTo Then
                    blnRet = True
                Else
                    strFrom = Mid(strFrom, 1, 4) & "/" & Mid(strTo, 5, 2) & "/" & Mid(strTo, 7, 2)
                    strTo = Mid(strTo, 1, 4) & "/" & Mid(strTo, 5, 2) & "/" & Mid(strTo, 7, 2)

                    Call CLMsg.Show("GE0013", strFrom, strTo)

                    blnRet = False
                End If
            Else
                Call CLMsg.Show("GE0004", "期")
                blnRet = False
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030201, SCREEN_NAME_UC030201, "CheckPeriodKikan")

            log.Fatal(ex.Message)
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet
    End Function
#End Region

#Region "データグリッドビュー初期化処理"
    '***************************************************************************************************
    '   ＩＤ　：DataGridViewIni
    '   名称　：データグリッドビュー初期化処理
    '   概要　：データグリッドビューの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/26(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26(土) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DataGridViewIni(ByVal Pdgd As DataGridView) As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            '-----------------------------------------------------------------------------------
            '   グリッド全体設定
            '-----------------------------------------------------------------------------------
            ' フォント
            Pdgd.DefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText 'フォントカラー

            ' 戻り値格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM000206, SCREEN_NAME_FM000206, "DataGridViewIni")

            ' 戻り値格納
            blnRet = False
        End Try

        ' 戻り値格納
        Return blnRet
    End Function
#End Region

#Region "関数：ChkExistHistory"
    ' ※020101の関数をコピーして期を変更
    '***************************************************************************************************
    '   ＩＤ　：ChkExistHistory
    '   名称　：委員会名簿の履歴が存在するかチェックする
    '   概要  ：
    '   作成日：2011/12/15 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/15 onuma  新規作成
    '***************************************************************************************************
    Private Function ChkExistHistory() As Boolean
        Dim blnRet As Boolean = False
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing
        Dim strSql As String = String.Empty

        Try
            '期ＩＤ、委員会ＩＤより履歴が存在するかチェック
            strSql = ""
            strSql = strSql & "SELECT c_committee_list " & vbCrLf
            strSql = strSql & "FROM committee_list " & vbCrLf
            strSql = strSql & "WHERE c_period_id = '" & Me.cboPeriod.SelectedValue & "' " & vbCrLf '期ID
            strSql = strSql & "AND c_committee_id = '" & Me.cboCommittee.SelectedValue & "' " & vbCrLf  '委員会ID

            clsDb.Connect()

            dtRet = clsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                '履歴が存在する場合Trueを返却
                blnRet = True
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM000206, SCREEN_NAME_FM000206, System.Reflection.ConstructorInfo.GetCurrentMethod().Name)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function
#End Region

#End Region

End Class

#End Region
