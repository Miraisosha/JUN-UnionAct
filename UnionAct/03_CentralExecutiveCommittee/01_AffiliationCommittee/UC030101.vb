#Region "UC030101"
'===========================================================================================================
'   クラスＩＤ　　：UC030101
'   クラス名称　　：委員会所属
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Document


Public Class UC030101

#Region "定数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UC030101_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC030101_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim blnRet As Boolean = False   ' 処理結果
        Dim clsDb As New CLAccessMdb       ' データベースクラス生成

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' データグリッドビュー初期化
            Call Me.DataGridViewIni(Me.dgdResult)

            ' データベース接続
            Call clsDb.Connect()

            ' 支部(検索条件) コンボボックス作成
            Call MDCommon.CreateCboConstantDtl(clsDb, Me.cboBelonging, "BELONGING")

            ' フォーカス設定
            Call Me.cboBelonging.Focus()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "UC030101_Load")
        Finally
            Call clsDb.Disconnect()

            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "コンボボックス選択肢変更"
#Region "支部"

    Private Sub cboBelonging_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboBelonging.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor
                ' しばらくお待ちくださいフォーム表示
                FrmWaitInfo.ShowWaitForm(Nothing)
                '検索実行
                Call Me.GetSearchData()

            Catch ex As Exception

                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "cboBelonging_KeyPress")

            Finally
                ' しばらくお待ちくださいフォーム非表示
                FrmWaitInfo.CloseWaitForm()
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default

            End Try

        End If

    End Sub
    '***************************************************************************************************
    '   ＩＤ　：cboBelonging_SelectionChangeCommitted
    '   名称　：支部コンボボックス選択肢変更
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboBelonging_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboBelonging.SelectionChangeCommitted

        ' 検索結果グリッドとプレ印刷を非表示
        Me.grpList.Visible = False
        Me.grpNewEntry.Visible = False

    End Sub
#End Region
#End Region

#Region "検索ボタン"
    '検索ボタン
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' しばらくお待ちくださいフォーム表示
            FrmWaitInfo.ShowWaitForm(Nothing)

            '検索実行
            Call Me.GetSearchData()

        Catch ex As Exception

            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "btnSearch_Click")

        Finally
            ' しばらくお待ちくださいフォーム非表示
            FrmWaitInfo.CloseWaitForm()

            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default

        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "印刷ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnPrinting_Click
    '   名称　：印刷ボタンクリック
    '   概要　：
    '   作成日：2011/11/12(土) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/12(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnPrinting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrinting.Click

        Dim clsDb As New CLAccessMdb   ' データベースクラス生成
        Dim fmPrint As New FM000203    '印刷プレビューフォーム
        Dim ds As New DS0301P1         'データセット
        Dim strDetailRow(11) As String '明細行配列

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' データベース接続
            clsDb.Connect()

            '明細のデータを作成
            For i = 0 To Me.dgdResult.Rows.Count - 1
                strDetailRow(0) = ""
                strDetailRow(1) = Me.dgdResult.Rows(i).Cells(0).Value   '社員番号
                strDetailRow(2) = Me.dgdResult.Rows(i).Cells(1).Value   '氏名
                strDetailRow(3) = Me.dgdResult.Rows(i).Cells(2).Value   '支部
                strDetailRow(4) = Me.GetTelephoneNumber(clsDb, Me.dgdResult.Rows(i).Cells(0).Value) '電話番号
                strDetailRow(5) = Me.dgdResult.Rows(i).Cells(4).Value   '委員会名0
                strDetailRow(6) = Me.dgdResult.Rows(i).Cells(5).Value   '委員会名1
                strDetailRow(7) = Me.dgdResult.Rows(i).Cells(6).Value   '委員会名2
                strDetailRow(8) = Me.dgdResult.Rows(i).Cells(7).Value   '委員会名3
                strDetailRow(9) = Me.dgdResult.Rows(i).Cells(8).Value   '委員会名4
                strDetailRow(10) = Me.dgdResult.Rows(i).Cells(9).Value  '委員会名5
                strDetailRow(11) = Me.dgdResult.Rows(i).Cells(10).Value '委員会名6

                Call ds.dtDetail.Rows.Add(strDetailRow)
            Next

            '印刷プレビュー準備
            fmPrint.ButtonShowType = 3            '印刷、キャンセルボタン
            fmPrint.PrintCntVisible = False       '印刷部数非表示
            fmPrint.ObjResource = New CR0301P1    '帳票インスタンスを作成

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
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "btnPrinting_Click")
        Finally
            ' データベース切断
            clsDb.Disconnect()

            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
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

        Dim blnRet As Boolean = False               ' 処理実行結果
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbMem As DataTable

        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Dim strBelonging As String = ""
        Dim strStafId As String = ""

        Dim strCommitteeID As String

        Dim booNextMember As Boolean
        Dim intComNum As Integer
        Dim strDataRow(10) As String
        Dim lngRowCnt As Long

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            '-------------------------------------------------------------------
            '   検索項目設定
            '-------------------------------------------------------------------
            Call Me.dgdResult.Rows.Clear()

            ' 組合支部
            If Me.cboBelonging.SelectedIndex > 0 Then
                strBelonging = Me.cboBelonging.SelectedValue
            End If

            strStafId = txtStafId.Text ' 社員番号

            ' データベース接続
            clsDb.Connect()

            ' SQL実行
            tbMem = SearchMember(clsDb, strBelonging, strStafId)

            ' 件数取得
            intRetCnt = tbMem.Rows.Count

            ' 件数チェック
            If intRetCnt = 0 Then
                ' 0件の処理

                ' グリッドを非表示
                Me.grpList.Visible = False
                Me.grpNewEntry.Visible = False

                CLMsg.Show("DI0001")                                    ' 対象データなしメッセージボックス表示
            Else
                ' 1件以上の処理

                booNextMember = True '次のメンバーか？フラグ
                intComNum = 0        '委員会数
                lngRowCnt = 0        'メンバー数
                For i = 0 To tbMem.Rows.Count - 1

                    '次のメンバーに変わったか？
                    If booNextMember = True Then
                        '出力用配列をクリア
                        For j = 0 To UBound(strDataRow)
                            strDataRow(j) = ""
                        Next

                        '社員番号、氏名、支部、機種を出力用配列に設定
                        For j = 0 To 3
                            strDataRow(j) = NSMDCommon.NVL(tbMem.Rows(i).Item(j))
                        Next
                    End If

                    strCommitteeID = NSMDCommon.NVL(tbMem.Rows(i).Item(4)) '委員会ID
                    '出力用配列が溢れていないか？
                    If intComNum + 4 <= UBound(strDataRow) Then
                        '出力用配列に委員会名を設定
                        strDataRow(intComNum + 4) = Me.GetCommitteeName(clsDb, strCommitteeID)
                    End If

                    '次の行は違うメンバーか？
                    If i = tbMem.Rows.Count - 1 OrElse tbMem.Rows(i).Item(0) <> tbMem.Rows(i + 1).Item(0) Then
                        'グリッドに行を追加
                        Me.dgdResult.Rows.Add(strDataRow)

                        booNextMember = True  '次のメンバーか？フラグをON
                        intComNum = 0         '委員会数をリセット
                        lngRowCnt += 1        'メンバー数をインクリメント
                    Else
                        booNextMember = False '次のメンバーか？フラグをOFF
                        intComNum += 1        '委員会数をインクリメント
                    End If
                Next

                ' グループボックス件数設定
                Me.grpList.Text = "検索結果（ " & lngRowCnt & " 件 ）"

                'グリッドを表示
                Me.grpList.Visible = True
                Me.grpNewEntry.Visible = True
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "GetSearchData")
        Finally
            ' データベース切断
            clsDb.Disconnect()
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値設定
        Return blnRet
    End Function
#End Region

#Region "組合員検索"
    '***************************************************************************************************
    '   ＩＤ　：SearchMember
    '   名称　：組合員検索
    '   概要  ：支部、社員番号で組合員を検索する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '           2012/05/01(火) Fujisaku　SQL性能改善
    '***************************************************************************************************
    Function SearchMember(ByVal clsDb As CLAccessMdb, ByVal strBelonging As String, ByVal strStafId As String) As DataTable

        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = ""
            strSql = strSql & "SELECT staf.c_user_id AS 社員番号," & vbCrLf
            strSql = strSql & "       staf.l_name AS 氏名," & vbCrLf
            strSql = strSql & "       (SELECT const.l_name FROM constant_dtl const WHERE const.c_constant = 'BELONGING' AND const.c_constant_seq = staf.k_belonging) AS 支部," & vbCrLf
            strSql = strSql & "       (SELECT const.l_name FROM constant_dtl const WHERE const.c_constant = 'MODEL' AND const.c_constant_seq = staf.k_model) AS 機種," & vbCrLf
            strSql = strSql & "       com_list_dtl.c_committee_id" & vbCrLf

            strSql = strSql & "FROM ((((SELECT c_committee_id FROM committee" & vbCrLf
            strSql = strSql & "        WHERE FORMAT(GETDATE(), 'yyyyMMdd') BETWEEN d_from AND d_to) AS com" & vbCrLf '委員会マスタの最新データのテーブル

            strSql = strSql & "INNER JOIN (SELECT t1.c_committee_id, t1.c_committee_list, t1.c_period_id" & vbCrLf
            strSql = strSql & "            FROM committee_list AS t1," & vbCrLf
            strSql = strSql & "                 (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
            strSql = strSql & "                  FROM committee_list" & vbCrLf
            strSql = strSql & "                  WHERE d_from <= FORMAT(GETDATE(), 'yyyyMMdd')" & vbCrLf
            strSql = strSql & "                  GROUP BY c_committee_id) AS t2" & vbCrLf
            strSql = strSql & "            WHERE t1.c_committee_id = t2.c_committee_id " & vbCrLf
            strSql = strSql & "             AND  t1.d_from         = t2.now_from) AS com_list" & vbCrLf    '委員会名簿の最新データのテーブル
            strSql = strSql & "        ON com.c_committee_id = com_list.c_committee_id)" & vbCrLf

            strSql = strSql & "INNER JOIN committee_list_dtl AS com_list_dtl" & vbCrLf                         '委員会名簿明細
            strSql = strSql & "        ON com_list.c_committee_list = com_list_dtl.c_committee_list)" & vbCrLf

            strSql = strSql & "INNER JOIN (SELECT t3.c_user_id, t3.l_name, t3.c_staf_id, t3.k_belonging, t3.k_model" & vbCrLf
            strSql = strSql & "            FROM staf_attribute AS t3," & vbCrLf
            strSql = strSql & "                 (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from" & vbCrLf
            strSql = strSql & "                  FROM staf_attribute" & vbCrLf
            strSql = strSql & "                  WHERE d_from <= FORMAT(GETDATE(), 'yyyyMMdd')" & vbCrLf
            strSql = strSql & "                  GROUP BY c_user_id, c_ksh, c_staf_id) AS t4" & vbCrLf
            strSql = strSql & "            WHERE t3.c_user_id = t4.c_user_id" & vbCrLf
            strSql = strSql & "             AND  t3.c_ksh     = t4.c_ksh" & vbCrLf
            strSql = strSql & "             AND  t3.d_from    = t4.now_from) AS staf" & vbCrLf             '組合員基本情報の最新データのテーブル
            strSql = strSql & "        ON com_list_dtl.c_user_id = staf.c_user_id)" & vbCrLf

            strSql = strSql & "INNER JOIN (SELECT t5.c_committee_id, t5.s_committee_seq, t6.c_period_id" & vbCrLf
            strSql = strSql & "            FROM committee_dtl AS t5, period_service_diff AS t6" & vbCrLf
            strSql = strSql & "            WHERE t5.s_from_diff = t6.service_diff" & vbCrLf
            strSql = strSql & "             AND t6.service_from <= FORMAT(GETDATE(), 'yyyyMMdd')" & vbCrLf
            strSql = strSql & "             AND t6.service_to >= FORMAT(GETDATE(), 'yyyyMMdd')" & vbCrLf
            strSql = strSql & "             AND t5.d_from  <= FORMAT(GETDATE(), 'yyyyMMdd')" & vbCrLf
            strSql = strSql & "             AND t5.d_to >= FORMAT(GETDATE(), 'yyyyMMdd')) AS per_diff" & vbCrLf
            strSql = strSql & "        ON com_list.c_committee_id = per_diff.c_committee_id" & vbCrLf
            strSql = strSql & "        AND com_list_dtl.s_committee_seq = per_diff.s_committee_seq" & vbCrLf
            strSql = strSql & "        AND com_list.c_period_id = per_diff.c_period_id " & vbCrLf

            strSql = strSql & "WHERE  1 = 1" & vbCrLf

            '所属支部が選択されているか？
            If strBelonging <> "" Then
                strSql = strSql & "  AND staf.k_belonging = '" & strBelonging & "'" & vbCrLf '所属支部ID
            End If

            '社員番号が指定されているか？
            If strStafId <> "" Then
                strSql = strSql & "  AND staf.c_staf_id LIKE '" & strStafId & "%'" & vbCrLf
            End If

            strSql = strSql & "ORDER BY CLng(staf.c_user_id)," & vbCrLf   '社員番号
            strSql = strSql & "         com_list_dtl.c_committee_id"      '委員会ID
            'todo:
            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "SearchMember")
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値設定
        Return tbRet
    End Function
#End Region

#Region "委員会名取得"
    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeName
    '   名称　：委員会名取得
    '   概要  ：委員会IDから委員会名を取得する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Function GetCommitteeName(ByVal clsDB As CLAccessMdb, ByVal pStrCommitteeID As String) As String

        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim strComName As String = ""               ' 委員会名

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = ""
            strSql = strSql & "SELECT l_name" & vbCrLf
            strSql = strSql & "FROM committee" & vbCrLf
            strSql = strSql & "WHERE c_committee_id = '" & pStrCommitteeID & "' AND" & vbCrLf
            strSql = strSql & "      FORMAT(GETDATE(), 'yyyyMMdd') BETWEEN d_from AND d_to"         '最新の委員会を取得

            ' SQL実行
            tbRet = clsDB.ExecuteSql(strSql)

            If tbRet.Rows.Count > 0 Then
                strComName = NSMDCommon.NVL(tbRet.Rows(0).Item(0))
            Else
                strComName = ""
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "GetCommitteeName")
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return strComName
    End Function
#End Region

#Region "電話番号取得"
    '***************************************************************************************************
    '   ＩＤ　：GetTelephoneNumber
    '   名称　：電話番号取得
    '   概要  ：社員番号から組合員の電話番号を取得する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Function GetTelephoneNumber(ByVal clsDB As CLAccessMdb, ByVal pStrStafID As String) As String

        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim strTelNo As String = ""                 ' 電話番号

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = ""
            strSql = strSql & "SELECT l_tell_1" & vbCrLf
            strSql = strSql & "FROM staf_address AS t1" & vbCrLf
            strSql = strSql & "WHERE EXISTS" & vbCrLf
            strSql = strSql & "      (SELECT * FROM" & vbCrLf
            strSql = strSql & "         (SELECT c_user_id, s_seq, MAX(d_from) AS now_from" & vbCrLf
            strSql = strSql & "          FROM staf_address" & vbCrLf
            strSql = strSql & "          WHERE d_from <= FORMAT(GETDATE(), 'yyyyMMdd')" & vbCrLf
            strSql = strSql & "          GROUP BY c_user_id, s_seq) AS t2" & vbCrLf
            strSql = strSql & "       WHERE t1.c_user_id = t2.c_user_id" & vbCrLf
            strSql = strSql & "         AND t1.s_seq     = t2.s_seq" & vbCrLf
            strSql = strSql & "         AND t1.d_from    = t2.now_from)" & vbCrLf
            strSql = strSql & "  AND c_user_id  = '" & pStrStafID & "'" & vbCrLf
            strSql = strSql & "  AND k_main_add = 'True'" & vbCrLf '現住所フラグ とぅるぅ～
            strSql = strSql & "ORDER BY s_seq"

            ' SQL実行
            tbRet = clsDB.ExecuteSql(strSql)

            If tbRet.Rows.Count > 0 Then
                strTelNo = NSMDCommon.NVL(tbRet.Rows(0).Item(0))
            Else
                strTelNo = ""
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "GetTelephoneNumber")
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return strTelNo
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
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM000206, SCREEN_NAME_FM000206, "DataGridViewIni")
        End Try

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#End Region

    Private Sub txtStafId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStafId.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor
                ' しばらくお待ちくださいフォーム表示
                FrmWaitInfo.ShowWaitForm(Nothing)
                '検索実行
                Call Me.GetSearchData()

            Catch ex As Exception

                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC030101, SCREEN_NAME_UC030101, "txtStafId_KeyPress")

            Finally
                ' しばらくお待ちくださいフォーム非表示
                FrmWaitInfo.CloseWaitForm()
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default

            End Try

        End If

    End Sub

End Class
#End Region
