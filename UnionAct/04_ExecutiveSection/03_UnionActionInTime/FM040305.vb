'===========================================================================================================
'   クラスＩＤ　　：UC040305
'   クラス名称　　：時間内組合活動　申請可能残数一覧
'   備考  　　　　：
'===========================================================================================================
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst

Public Class FM040305

#Region "定数"
    Private Const SCREEN_ID As String = SCREEN_ID_FM040305
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM040305
#End Region

#Region "内部プロパティ"
    '権限フラグ
    Private IsReferenceRight As Boolean
    Private IsGetEntryRight As Boolean
    Private IsPrintRight As Boolean
    Private IsFileOutputRight As Boolean

    '表示期
    Private minPdYears As String
    Private maxPdYears As String
    Private selPdYears As String

    '種別毎上限数
    Private intHeLimit As Integer
    Private intToLimit As Integer
    Private intTiLimit As Integer
    Private intRiLimit As Integer
    Private intNuLimit As Integer
    Private intRuLimit As Integer


#End Region

#Region "ログ出力オブジェクト"
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"

#Region "イベント：フォームロード"
    '************************************************************************************
    '   ＩＤ　：FM040305_Load
    '   名称　：フォームロード
    '   概要　：フォームロード
    '   作成日：2012/01/18(水)
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) Fujisaku  新規作成
    '************************************************************************************
    Private Sub FM040305_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            ' 現在期開始年取得
            Me.selPdYears = MDLoginInfo.PeriodFrom.Substring(0, 4)

            ' 期の最小最大開始年設定(残数一覧表示可能範囲)
            Call Me.GetMaxMinPdYears()

            ' 画面制御・画面表示一括処理
            Call Me.ShowRefresh()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            'カーソルを戻す
            Me.Cursor = Cursors.Default
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：戻るボタンクリック"
    '************************************************************************************
    '   ＩＤ　：Button1_Click
    '   名称　：戻るボタンクリック
    '   概要　：戻るボタンクリック
    '   作成日：2012/01/18(水)
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) Fujisaku  新規作成
    '************************************************************************************
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
#End Region

#Region "イベント：前期「＜」ボタンクリック"
    '************************************************************************************
    '   ＩＤ　：BtnPrePeriod_Click
    '   名称　：前期「＜」ボタンクリック
    '   概要　：前期の申請残数表示
    '   作成日：2012/01/19(木)
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) Fujisaku  新規作成
    '************************************************************************************
    Private Sub BtnPrePeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPrePeriod.Click

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            ' 現在期開始年再設定処理
            Me.selPdYears = CStr(CInt(selPdYears) - 1)

            ' 画面制御・画面表示一括処理
            Call Me.ShowRefresh()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        Finally
            'カーソルを戻す
            Me.Cursor = Cursors.Default
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：次期「＞」ボタンクリック"
    '************************************************************************************
    '   ＩＤ　：BtnPostPeriod_Click
    '   名称　：次期「＞」ボタンクリック
    '   概要　：次期申請残数表示
    '   作成日：2012/01/19(木)
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) Fujisaku  新規作成
    '************************************************************************************
    Private Sub BtnPostPeriod_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPostPeriod.Click

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            ' 現在期開始年再設定処理
            Me.selPdYears = CStr(CInt(selPdYears) + 1)

            ' 画面制御・画面表示一括処理
            Call Me.ShowRefresh()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        Finally
            'カーソルを戻す
            Me.Cursor = Cursors.Default
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：覚書を表示リンククリック"
    '************************************************************************************
    '   ＩＤ　：LinkLabel1_LinkClicked
    '   名称　：覚書を表示リンククリック
    '   概要　：覚書を表示
    '   作成日：2012/01/23(月)
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/23(月) Fujisaku  新規作成
    '************************************************************************************
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            ' 「覚書を表示」の共通処理起動
            ShowOboegaki()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        Finally
            'カーソルを戻す
            Me.Cursor = Cursors.Default
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#End Region

#Region "関数"

#Region "関数：最大・最小期開始年取得"
    '************************************************************************************
    '   ＩＤ　：GetMaxMinPdYears
    '   名称　：最大・最小期開始年取得
    '   概要　：最大・最小期開始年取得
    '   作成日：2012/01/18(水)
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/18(水) Fujisaku  新規作成
    '************************************************************************************
    Private Function GetMaxMinPdYears() As Boolean

        Dim blnRet As Boolean = False               ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 期マスタ取得
            strSql = ""
            strSql = strSql & "SELECT" & vbCrLf
            strSql = strSql & "  min(d_from) AS min_period_from," & vbCrLf
            strSql = strSql & "  max(d_from) AS max_period_from" & vbCrLf
            strSql = strSql & "FROM Period"

            ' DB接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            If Not intRetCnt = 0 Then
                Me.minPdYears = NVL(tbRet.Rows(0).Item("min_period_from")).Substring(0, 4)
                Me.maxPdYears = CStr(CInt(NVL(tbRet.Rows(0).Item("max_period_from")).Substring(0, 4)) + 1)
            Else
                Me.minPdYears = Me.selPdYears
                Me.maxPdYears = Me.selPdYears
            End If

            blnRet = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        Return blnRet
    End Function
#End Region

#Region "関数：画面ラベル(年)更新処理"
    '************************************************************************************
    '   ＩＤ　：RenewLabelYear
    '   名称　：画面ラベル(年)更新処理
    '   概要　：画面ラベル(年)更新処理
    '   作成日：2012/01/19(木)
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) Fujisaku  新規作成
    '************************************************************************************
    Private Function RenewLabelYear() As Boolean

        Dim strStartYear As String
        Dim strEndYear As String
        Dim strTerm1 As String
        Dim strTerm2 As String

        ' 開始年と終了年
        strStartYear = Me.selPdYears
        strEndYear = CStr(CInt(strStartYear) + 1)

        ' 期間１（8月～7月）
        strTerm1 = strStartYear & "/08 ～ " & strEndYear & "/07"

        ' 期間２（9月～8月）
        strTerm2 = strStartYear & "/09 ～ " & strEndYear & "/08"

        ' ラベルの更新
        Me.LblHeStart.Text = strStartYear
        Me.LblHeEnd.Text = strEndYear

        Me.LblToTerm.Text = strTerm1

        Me.LblTiTerm.Text = strTerm1

        Me.LblRiTerm.Text = strTerm2

        Me.LblNuStart.Text = strStartYear
        Me.LblNuEnd.Text = strEndYear

        Me.LblRuStart.Text = strStartYear
        Me.LblRuEnd.Text = strEndYear

        Return True

    End Function
#End Region

#Region "関数：画面ラベル(Limit)更新処理"
    '************************************************************************************
    '   ＩＤ　：RenewLabelLimit
    '   名称　：画面ラベル(Limit)更新処理
    '   概要　：画面ラベル(Limit)更新処理
    '   作成日：2012/01/19(木)
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) Fujisaku  新規作成
    '************************************************************************************
    Private Function RenewLabelLimit() As Boolean
        Dim blnRet As Boolean = False               ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Try
            ' 定数詳細値取得
            strSql = ""
            strSql = strSql & "SELECT" & vbCrLf
            strSql = strSql & "  c_constant_seq," & vbCrLf      ' 種類区分
            strSql = strSql & "  l_name" & vbCrLf               ' 上限回数
            strSql = strSql & "FROM constant_dtl " & vbCrLf
            strSql = strSql & "WHERE c_constant = 'APPLY_STRIKE_LIMIT'"
            strSql = strSql & "ORDER BY s_order " & vbCrLf

            ' DB接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' レコード数分ループ
            For i = 0 To intRetCnt - 1
                Select Case tbRet.Rows(i).Item("c_constant_seq")
                    Case "01"
                        Me.intHeLimit = CInt(tbRet.Rows(i).Item("l_name"))
                    Case "03"
                        Me.intToLimit = CInt(tbRet.Rows(i).Item("l_name"))
                    Case "04"
                        Me.intTiLimit = CInt(tbRet.Rows(i).Item("l_name"))
                    Case "05"
                        Me.intRiLimit = CInt(tbRet.Rows(i).Item("l_name"))
                    Case "06"
                        Me.intNuLimit = CInt(tbRet.Rows(i).Item("l_name"))
                    Case "07"
                        Me.intRuLimit = CInt(tbRet.Rows(i).Item("l_name"))
                End Select
            Next

            ' ラベルの更新
            Me.LblHeLimit.Text = "（単位：回　　２ヶ月" & CStr(intHeLimit) & "回を限度）"
            Me.LblToLimit.Text = "（単位：回　　年" & CStr(intToLimit) & "回を限度）"
            Me.LblTiLimit.Text = "（単位：人日　　年のべ" & CStr(intTiLimit) & "人日を限度）"
            Me.LblRiLimit.Text = "（単位：人日　　年のべ" & CStr(intRiLimit) & "人日を限度）"
            Me.LblNuLimit.Text = "（単位：人日　　２ヶ月のべ" & CStr(intNuLimit) & "人日を限度）"
            Me.LblRuLimit.Text = "（単位：人日　　２ヶ月のべ" & CStr(intRuLimit) & "人日を限度）"

            blnRet = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function
#End Region

#Region "関数：申請可能残数表示処理"
    '************************************************************************************
    '   ＩＤ　：ShowRemainTimes
    '   名称　：申請可能残数表示処理
    '   概要　：申請可能残数表示処理
    '   作成日：2012/01/19(木)
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) Fujisaku  新規作成
    '************************************************************************************
    Private Function ShowRemainTimes() As Boolean

        Dim blnRet As Boolean = False               ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Try
            Me.DataGridViewHe.RowCount = 2

            ' DB接続
            clsDb.Connect()

            '-------------------------------------------------------------------
            '  種類設定
            '-------------------------------------------------------------------
            ' SQL作成
            strSql = ""
            strSql = strSql & "SELECT" & vbCrLf
            strSql = strSql & "  c_constant_seq," & vbCrLf      ' 種類区分
            strSql = strSql & "  l_name" & vbCrLf               ' 区分名称
            strSql = strSql & "FROM constant_dtl " & vbCrLf
            strSql = strSql & "WHERE c_constant = 'APPLY_CLASSIFY'"
            strSql = strSql & "ORDER BY s_order " & vbCrLf

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' レコード数分ループ
            For i = 0 To intRetCnt - 1
                Dim strCName As String = tbRet.Rows(i).Item("l_name")

                Select Case tbRet.Rows(i).Item("c_constant_seq")
                    Case "01"
                        Me.DataGridViewHe.Rows(0).Cells.Item(0).Value = strCName
                    Case "02"
                        Me.DataGridViewHe.Rows(1).Cells.Item(0).Value = strCName
                    Case "03"
                        Me.DataGridViewTo.Rows(0).Cells.Item(0).Value = strCName
                    Case "04"
                        Me.DataGridViewTi.Rows(0).Cells.Item(0).Value = strCName
                    Case "05"
                        Me.DataGridViewRi.Rows(0).Cells.Item(0).Value = strCName
                    Case "06"
                        Me.DataGridViewNu.Rows(0).Cells.Item(0).Value = strCName
                    Case "07"
                        Me.DataGridViewRu.Rows(0).Cells.Item(0).Value = strCName
                End Select
            Next

            '-------------------------------------------------------------------
            '  上限回数設定
            '-------------------------------------------------------------------
            ' 対象期間開始月の作成
            Dim strTermArray As ArrayList = New ArrayList()
            strTermArray.Add(selPdYears & "04")
            strTermArray.Add(selPdYears & "06")
            strTermArray.Add(selPdYears & "08")
            strTermArray.Add(selPdYears & "10")
            strTermArray.Add(selPdYears & "12")
            strTermArray.Add(CStr(CInt(selPdYears) + 1) & "02")

            Dim strYearArray As ArrayList = New ArrayList(3)
            strYearArray.Add(selPdYears & "08")
            strYearArray.Add(selPdYears & "08")
            strYearArray.Add(selPdYears & "09")


            Dim intLim As Integer = 0
            Dim intCnt As Integer = 0

            ' 覚書（へ）
            For i = 0 To 5
                intLim = CInt(MDApplyStrikeCommon.GetApplyStrikeLimit("01"))
                intCnt = CInt(MDApplyStrikeCommon.GetApplyCount("01", strTermArray(i), ""))

                Me.DataGridViewHe.Rows(0).Cells.Item(i + 1).Value = CStr(intLim - intCnt)
                Me.DataGridViewHe.Rows(1).Cells.Item(i + 1).Value = CStr(intLim - intCnt)
            Next

            ' 覚書（ト）:DataGridViewTo
            intLim = CInt(MDApplyStrikeCommon.GetApplyStrikeLimit("03"))
            intCnt = CInt(MDApplyStrikeCommon.GetApplyCount("03", strYearArray(0), ""))
            Me.DataGridViewTo.Rows(0).Cells.Item(1).Value = CStr(intLim - intCnt)

            ' 覚書（チ）:DataGridViewTi
            intLim = CInt(MDApplyStrikeCommon.GetApplyStrikeLimit("04"))
            intCnt = CInt(MDApplyStrikeCommon.GetApplyCount("04", strYearArray(1), ""))      ' 残数表示画面は東京大阪合算
            Me.DataGridViewTi.Rows(0).Cells.Item(1).Value = CStr(intLim - intCnt)

            ' 覚書（リ）:DataGridViewRi
            intLim = CInt(MDApplyStrikeCommon.GetApplyStrikeLimit("05"))
            intCnt = CInt(MDApplyStrikeCommon.GetApplyCount("05", strYearArray(2), ""))
            Me.DataGridViewRi.Rows(0).Cells.Item(1).Value = CStr(intLim - intCnt)

            ' 覚書（ヌ）:DataGridViewNu
            For i = 0 To 5
                intLim = CInt(MDApplyStrikeCommon.GetApplyStrikeLimit("06"))
                intCnt = CInt(MDApplyStrikeCommon.GetApplyCount("06", strTermArray(i), ""))

                Me.DataGridViewNu.Rows(0).Cells.Item(i + 1).Value = CStr(intLim - intCnt)
            Next

            ' 覚書（ル）:DataGridViewRu
            For i = 0 To 5
                intLim = CInt(MDApplyStrikeCommon.GetApplyStrikeLimit("07"))
                intCnt = CInt(MDApplyStrikeCommon.GetApplyCount("07", strTermArray(i), ""))

                Me.DataGridViewRu.Rows(0).Cells.Item(i + 1).Value = CStr(intLim - intCnt)
            Next

            blnRet = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function
#End Region

#Region "関数：画面制御・画面表示一括処理"
    '************************************************************************************
    '   ＩＤ　：ShowRefresh
    '   名称　：画面制御・画面表示一括処理
    '   概要　：画面制御・画面表示一括処理
    '   作成日：2012/01/19(木)
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) Fujisaku  新規作成
    '************************************************************************************
    Private Function ShowRefresh() As Boolean
        Dim blnRet As Boolean = False               ' 戻り値

        Try
            ' 次期ボタン表示制御
            If Me.selPdYears = Me.maxPdYears Then
                '
                Me.BtnPostPeriod.Enabled = False
            Else
                Me.BtnPostPeriod.Enabled = True
            End If

            ' 前期ボタン表示制御
            If Me.selPdYears = Me.minPdYears Then
                '
                Me.BtnPrePeriod.Enabled = False
            Else
                Me.BtnPrePeriod.Enabled = True
            End If

            ' 画面ラベル(年)更新処理
            Call Me.RenewLabelYear()

            ' 画面ラベル(Limit)更新処理
            Call Me.RenewLabelLimit()

            ' データグリッド表示処理
            Call Me.ShowRemainTimes()

            blnRet = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        Return blnRet
    End Function
#End Region

#End Region

End Class