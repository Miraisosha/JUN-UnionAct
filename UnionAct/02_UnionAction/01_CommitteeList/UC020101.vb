#Region "UC020101"
'===========================================================================================================
'   クラスＩＤ　　：UC020101
'   クラス名称　　：部／委員会名簿
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo
Imports UnionAct.DAO.Master

Public Class UC020101

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC020101
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC020101

    Private Const STR_COLUMNNAME_NAME_KNA As String = "名前カナ"
    Private Const STR_COLUMNNAME_STAFFID As String = "社員番号"
    Private Const STR_COLUMNNAME_NAME As String = "名前"
    Private Const STR_COLUMNNAME_KIND As String = "組合員種別"
    Private Const STR_COLUMNNAME_STATUS As String = "ステータス"
    Private Const STR_COLUMNNAME_BELONGING As String = "組合支部"
    Private Const STR_COLUMNNAME_QUOLIFICATION As String = "資格"
    Private Const STR_COLUMNNAME_MODEL As String = "機種"
    Private Const STR_COLUMNNAME_LOCAL As String = "会社所属"
    Private Const S_MODEL_NAME As String = "機種略称"
    Private Const L_POST_NAME As String = "役職"
    Private Const C_TELL_1 As String = "電話番号1"
    Private Const S_MAIL_PC As String = "メール"
    Private Const S_COMMITTEE_SEQ = "役職ID"
    Private Const S_ATT_BIKO = "備考"
    Private Const FLAG = "flag"
    Public Const STR_SELECT_BASE As String = "SELECT attrbute_new.名前," + _
                                       "attrbute_new.社員番号," + _
                                       "attrbute_new.機種," + _
                                       "attrbute_new.資格," + _
                                       "attrbute_new.組合支部," + _
                                       "attrbute_new.会社所属," + _
                                       "add_new.電話番号1, " + _
                                       "attrbute_new.機種略称," + _
                                       "add_new.メール, " + _
                                       "attrbute_new.ステータス," + _
                                       "attrbute_new.組合員種別, " + _
                                       "attrbute_new.備考 " + _
                                       "FROM "

    Public Const STR_SELECT_ATTRIBUTE_MAIN As String = "(SELECT " + _
                                        "x.l_name AS 名前," + _
                                        "x.c_staf_id AS 社員番号," + _
                                        " x.l_name_kna AS 名前カナ," + _
                                        " x.k_user_status AS ステータス," + _
                                        " x.k_staf_kind AS 組合員種別," + _
                                        "cd1.l_name as 組合支部, " + _
                                        "cd2.l_omission_name as 資格, " + _
                                        "cd3.l_name as 機種," + _
                                        "cd3.l_omission_name as 機種略称," + _
                                        "cd4.l_name as 会社所属," + _
                                        "'' as 備考 " + _
                                        "FROM staf_attribute AS x, " + _
                                        "constant_dtl cd1," + _
                                        "constant_dtl cd2," + _
                                        "constant_dtl cd3," + _
                                        "constant_dtl cd4, " + _
                                        "   (SELECT c_user_id , max(d_from) AS new_from " + _
                                        "    FROM staf_attribute "

    Public Const STR_SELECT_ATTRIBUTE_SUB As String = " GROUP BY c_user_id ) AS y " + _
                                       "WHERE x.c_user_id=y.c_user_id " + _
                                       " AND x.d_from=y.new_from " + _
                                       " AND cd1.c_constant = 'BELONGING' " + _
                                       " AND cd1.c_constant_seq = x.k_belonging" + _
                                       " AND cd2.c_constant = 'QUALIFICATION' " + _
                                       " AND cd2.c_constant_seq = x.k_qualification" + _
                                       " AND cd3.c_constant = 'MODEL' " + _
                                       " AND cd3.c_constant_seq = x.k_model" + _
                                       " AND cd4.c_constant = 'AREA_LOCAL' " + _
                                       " AND cd4.c_constant_seq = x.k_local "
    Public Const STR_SELECT_ATTRIBUTE_SUB2 As String = " ) AS attrbute_new "
    Public Const STR_LEFT_JOIN As String = "LEFT JOIN "
    Public Const STR_SELECT_ADDRESS_MAIN As String = "(SELECT add1.c_user_id AS 社員番号, " + _
                                        "add1.l_tell_1 AS 電話番号1," + _
                                        "add1.l_mail_pc AS メール " + _
                                        "FROM staf_address AS add1 ," + _
                                        " (SELECT c_user_id, s_seq , max(d_from) AS now_from " + _
                                        "  FROM staf_address "
    Public Const STR_SELECT_ADDRESS_SUB As String = "GROUP BY c_user_id, s_seq) AS newadd " + _
                                        " WHERE add1.c_user_id = newadd.c_user_id " + _
                                        " AND add1.s_seq = newadd.s_seq " + _
                                        " AND add1.d_from = newadd.now_from " + _
                                        " AND add1.k_main_add = 'True' " + _
                                        ") AS add_new "
    Public Const STR_JOIN_ON As String = " ON attrbute_new.社員番号 = add_new.社員番号 "
    Private ReadOnly ARR_DATAGRIDVIEW As DataGridView() = {New DataGridView, New DataGridView, New DataGridView, _
                                                           New DataGridView, New DataGridView, New DataGridView, _
                                                           New DataGridView, New DataGridView, New DataGridView, _
                                                           New DataGridView, New DataGridView}
    Private ReadOnly ARR_STR_SQLWHERE As String() = {"AND x.l_name_kna LIKE '[ｱ-ｵ]%'", _
                                             "AND x.l_name_kna LIKE '[ｶ-ｺﾞ]%'", _
                                             "AND x.l_name_kna LIKE '[ｻ-ｿﾞ]%'", _
                                             "AND x.l_name_kna LIKE '[ﾀ-ﾄﾞ]%'", _
                                             "AND x.l_name_kna LIKE '[ﾅ-ﾉ]%'", _
                                             "AND x.l_name_kna LIKE '[ﾊ-ﾎﾞ]%'", _
                                             "AND x.l_name_kna LIKE '[ﾏ-ﾓ]%'", _
                                             "AND x.l_name_kna LIKE '[ﾔ-ﾖ]%'", _
                                             "AND x.l_name_kna LIKE '[ﾗ-ﾛ]%'", _
                                             "AND x.l_name_kna LIKE '[ﾜ-ﾝ]%'", _
                                             "AND NOT x.l_name_kna LIKE '[ｱ-ﾝ]%'"}
    'データグリッドの設定
    Private Const DGD_HEADER_CENTER As DataGridViewContentAlignment = DataGridViewContentAlignment.MiddleCenter
    Private ReadOnly DGD_FORECOLOR_BLACK As System.Drawing.Color = System.Drawing.Color.Black
    Private ReadOnly DGD_ROWHEADER_NONVISIBLE As Boolean = False
    Private ReadOnly DGD_ADDROW_NON As Boolean = False
    Private ReadOnly DGD_DELETEROW_NON As Boolean = False
    Private ReadOnly DGD_SELECTIONMODE_FULLRAW As DataGridViewSelectionMode = DataGridViewSelectionMode.FullRowSelect
    Private Const D_FROM = "d_from"
    Private Const D_TO = "d_to"
    Private ReadOnly LOCATION_UPDATEBUTTON As System.Drawing.Point = New System.Drawing.Point(866, 763)
    'Private ReadOnly LOCATION_COPYLASTPERIODBUTTON As System.Drawing.Point = New System.Drawing.Point(290, 537)
    Private ReadOnly LOCATION_DELETELISTBUTTON As System.Drawing.Point = New System.Drawing.Point(318, 763)
    '列名
    Private ReadOnly ARR_CULUMNSNAME_LIST_UNION As String() = {STR_COLUMNNAME_NAME, STR_COLUMNNAME_STAFFID, STR_COLUMNNAME_MODEL, _
                                                               STR_COLUMNNAME_QUOLIFICATION, STR_COLUMNNAME_BELONGING, STR_COLUMNNAME_LOCAL, _
                                                               C_TELL_1, S_MODEL_NAME, S_MAIL_PC, STR_COLUMNNAME_STATUS, STR_COLUMNNAME_KIND, S_ATT_BIKO}
    Private ReadOnly ARR_CULUMNSVISIBLE_LIST_UNION As Boolean() = {True, True, True, True, True, True, False, False, False, False, False, False}
    Private ReadOnly ARR_CULUMNSWIDTH_LIST_UNION As Integer() = {160, 100, 100, 80, 80, 80, 80, 50, 50, 50, 50, 50}
    '委員会構成員列
    Private ReadOnly ARR_CULUMNSNAME_LIST_COMMITTEE As String() = {L_POST_NAME, STR_COLUMNNAME_NAME, STR_COLUMNNAME_STAFFID, _
                                                                   STR_COLUMNNAME_MODEL, STR_COLUMNNAME_QUOLIFICATION, STR_COLUMNNAME_BELONGING, _
                                                                   STR_COLUMNNAME_LOCAL, C_TELL_1, S_MODEL_NAME, S_MAIL_PC, S_COMMITTEE_SEQ, _
                                                                   FLAG, STR_COLUMNNAME_STATUS, STR_COLUMNNAME_KIND, S_ATT_BIKO}
    Private ReadOnly ARR_CULUMNSVISIBLE_LIST_COMMITTEE As Boolean() = {True, True, True, True, True, True, False, False, False, False, False, False, False, False, False}
    Private ReadOnly ARR_CULUMNSWIDTH_LIST_COMMITTEE As Integer() = {160, 100, 100, 80, 80, 80, 100, 100, 50, 50, 100, 100, 50, 50, 50}
#End Region

#Region "ログ出力オブジェクト"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "内部プロパティ"
    Private showDataGridView As DataGridView = Nothing  '（右グリッドから左グリッドへデータ移行時に使用）
    Private strDefaultCommitteeSeq = "3"                '委員会追加時の初期値
    Private searchStandardDay As String = ""            '基準日
    Private tbBefore As DataTable                       '変更前情報
    Private dtDeleteList As New DataTable               '削除者リスト
    Private dicPost As Dictionary(Of String, String) = New Dictionary(Of String, String)       '役職名
    Private dicOutTerm As Dictionary(Of String, DataRow) = New Dictionary(Of String, DataRow)  '対象年月期間外の役職リスト
    Private dicStafKind As Dictionary(Of String, String) = Nothing
    '参照権限
    Private _strGrantReference As String = String.Empty
    '登録権限
    Private _strGrantInsert As String = String.Empty
    '印刷権限
    Private _strGrantPrint As String = String.Empty
    'ファイル出力権限
    Private _strGrantFileOutput As String = String.Empty
#End Region

#Region "イベント"
#Region "イベント：UC020101_Load"
    '***************************************************************************************************
    '   ＩＤ　：UC020101_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：
    '***************************************************************************************************
    Private Sub UC020101_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim db_inf As CLAccessMdb = Nothing 'DB接続用
        Dim tbResultSql As New DataTable    'SQL結果取得用
        Dim dtGrant As DataTable = Nothing
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            '権限の取得
            dtGrant = getGrant(MENU_ID_UC020101)

            If dtGrant.Rows.Count > 0 Then
                _strGrantReference = dtGrant.Rows(0).Item(3).ToString
                _strGrantInsert = dtGrant.Rows(0).Item(4).ToString
                _strGrantPrint = dtGrant.Rows(0).Item(5).ToString
                _strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString
            End If

            If _strGrantPrint <> GRANT_VALID Then
                '印刷権限がない場合
                Me.btnPrePrint.Enabled = False
            End If

            If _strGrantInsert <> GRANT_VALID Then
                '登録権限がない場合
                Me.btnUpdate.Enabled = False
            End If

            db_inf = New CLAccessMdb()
            Call db_inf.Connect()
            '委員会コンボボックス作成処理
            If MakeCommitteeCmbBox(db_inf) = False Then
                Exit Sub
            End If
            cboCommittee.SelectedIndex = 0

            ' コンボボックス（年）作成処理
            If NSMDCommon.CreateComboBoxYear(Me.cboYear, False) = False Then
                Exit Sub
            End If
            'コンボボックス（月）設定処理
            Call Me.SetMonth()

            Me.cboYear.Text = System.DateTime.Now().Year
            Me.cboMonth.Text = System.DateTime.Now().Month
            If ChkNull(Me.cboYear.Text) = False AndAlso ChkNull(Me.cboMonth.Text) = False Then
                '基準日を1日に設定
                Me.dtpTargetDate.Value = CDate(Me.cboYear.Text & "/" & Me.cboMonth.Text & "/01")
            End If

            'Me.btnCopyLastPeriod.Location = LOCATION_COPYLASTPERIODBUTTON
            Me.btnUpdate.Location = LOCATION_UPDATEBUTTON
            Me.btnDelList.Location = LOCATION_DELETELISTBUTTON

            'メッセージ表示用に組合員種別一覧を作成
            Call Me.SetStafKindList()
            '出力項目デフォルトチェック
            chkMemberNumber.Checked = True
            chkName.Checked = True
            chkPost.Checked = True
            chkTel.Checked = True
            chkBranch.Checked = True
            chkModel.Checked = True
            chkQualification.Checked = True
            chkMail.Checked = True
            chkNote.Checked = True

            Me.Focus()

            ARR_DATAGRIDVIEW.SetValue(Me.dgdALine, 0)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdKALine, 1)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdSALine, 2)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdTALine, 3)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdNALine, 4)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdHALine, 5)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdMALine, 6)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdYALine, 7)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdRALine, 8)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdWALine, 9)
            ARR_DATAGRIDVIEW.SetValue(Me.dgdOther, 10)

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "UC020101_Load")
        Finally
            db_inf.Disconnect()
        End Try
    End Sub
#End Region

#Region "イベント：btnHistory_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnHistory_Click
    '   名称　：履歴ボタンクリック処理
    '   概要  ：
    '   作成日：2011/11/15(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHistory.Click

        Dim clsFM000206 As FM000206 = Nothing       ' 委員会名簿履歴一覧画面クラス

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            '委員会名簿履歴が存在するかチェック
            If Me.ChkExistHistory() = False Then
                CLMsg.Show("DI0002")
                Exit Sub
            End If

            clsFM000206 = New FM000206              ' 委員会名簿履歴一覧画面クラス生成

            clsFM000206.strSearchPeriodId = MDLoginInfo.PeriodId
            clsFM000206.strSearchCommitteeId = Me.cboCommittee.SelectedValue

            Call clsFM000206.ShowDialog()           ' 委員会名簿履歴一覧画面表示

            If clsFM000206.DialogResult = DialogResult.OK Then
                Me.cboYear.Text = clsFM000206.strYear
                Me.cboMonth.Text = clsFM000206.strMonth
            End If

            clsFM000206.Dispose()                   ' 委員会名簿履歴一覧画面クラス破棄

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnHistory_Click")
        Finally
            ' カーソルを矢印に設定
            Cursor.Current = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：btnSearch_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要  ：
    '   作成日：
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

            Call Me.SearchMain()

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnSearch_Click")
        Finally
            'カーソルを元に戻す
            Me.Cursor = Cursors.Default
        End Try

    End Sub
#End Region

#Region "イベント：btnSet_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnSet_Click
    '   名称　：設定ボタンクリック処理
    '   概要  ：
    '   作成日：
    '***************************************************************************************************
    Private Sub btnSet_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSet.Click
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            searchStandardDay = Me.cboYear.Text & Me.cboMonth.Text & Me.dtpTargetDate.Value.Day.ToString.PadLeft(2, "0")
            '組合員一覧をクリア
            Call Me.ClearGrpUnionMember()

            '組合員一覧を再検索
            clsDb.Connect()
            Call Me.SetJpnSyllabaryEachPage(clsDb)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnSet_Click")
        Finally
            clsDb.Disconnect()
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

#Region "イベント：btnUpdate_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnUpdate_Click
    '   名称　：内容更新ボタンクリック処理
    '   概要  ：
    '   作成日：
    '***************************************************************************************************
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim blnShowCopyLastPeriod As Boolean = True
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            Me.Cursor = Cursors.WaitCursor
            If ChkExistActivity() = False Then '出欠簿の登録チェック
                blnShowCopyLastPeriod = False
                If CLMsg.Show("GW0011", Me.cboCommittee.Text, Me.cboYear.Text, Me.cboMonth.Text) = DialogResult.No Then
                    Exit Sub
                End If
            End If

            '役職の任期が対象年月の範囲内であるかチェック
            If ChkPostTerm() = False Then
                Exit Sub
            End If

            '登録モードに切り替え
            Call Me.EditModeChange(True, blnShowCopyLastPeriod)
            '初めに表示するタブを設定（あ行）
            showDataGridView = Me.dgdALine
            '抽出機能側の表示処理
            Call Me.ShowDgdUnionMemberList()

            '非表示になっているメンバーを表示する
            Call Me.iniDgdCommitteeListShow()

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnUpdate_Click")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub
#End Region

#Region "イベント：btnPrePrint_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnPrePrint_Click
    '   名称　：プレ印刷ボタンクリック処理
    '   概要  ：
    '   作成日：
    '***************************************************************************************************
    Private Sub btnPrePrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrePrint.Click

        Dim dtCommitteeCompositionList As DataTable

        Dim fmPrint As New FM000203     '印刷画面
        Dim dsReportInf As New DS0201P1 'レポート用データテーブル
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument  'レポート形式
        Dim flgCheck As Boolean = False
        Dim lstCheck As New List(Of String)
        Dim iListCounter As Integer
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

            'If lstCheck.Count > 8 Then
            '    MsgBox("出力項目を８項目以下にしてください。", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
            '    Exit Sub
            'End If
            Me.Cursor = Cursors.WaitCursor
            dtCommitteeCompositionList = GetCommitteeCompositionList()
            If dtCommitteeCompositionList.Rows.Count > 0 Then
                '出力項目デフォルトチェック
                If chkMemberNumber.Checked Then
                    flgCheck = True
                    lstCheck.Add(STR_COLUMNNAME_STAFFID)
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkName.Checked Then
                    flgCheck = True
                    lstCheck.Add(STR_COLUMNNAME_NAME)
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkPost.Checked Then
                    flgCheck = True
                    lstCheck.Add(L_POST_NAME)
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkTel.Checked Then
                    flgCheck = True
                    lstCheck.Add(C_TELL_1)
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkBranch.Checked Then
                    flgCheck = True
                    lstCheck.Add(STR_COLUMNNAME_BELONGING)
                    lstCheck.Add("")
                End If
                If chkModel.Checked Then
                    flgCheck = True
                    lstCheck.Add(S_MODEL_NAME)
                    lstCheck.Add("")
                End If
                If chkQualification.Checked Then
                    flgCheck = True
                    lstCheck.Add(STR_COLUMNNAME_QUOLIFICATION)
                    lstCheck.Add("")
                End If
                If chkMail.Checked Then
                    flgCheck = True
                    lstCheck.Add(S_MAIL_PC)
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkNote.Checked Then
                    flgCheck = True
                    lstCheck.Add(S_ATT_BIKO)
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If flgCheck = False Then
                    MsgBox("出力項目をチェックしてください。", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
                    Exit Sub
                End If
                '帳票出力
                reportObj = New CR0201P1(lstCheck)
                '組合員情報格納
                Dim drDetail As DS0201P1.list_titleRow
                For Each row As DataRow In dtCommitteeCompositionList.Rows
                    drDetail = dsReportInf.list_title.NewRow
                    drDetail.BeginEdit()
                    For iListCounter = 0 To lstCheck.Count - 1
                        If lstCheck(iListCounter) = L_POST_NAME Then
                            If dicPost.ContainsKey(row(L_POST_NAME)) = True Then
                                drDetail("t" + CStr(iListCounter)) = dicPost(row(lstCheck(iListCounter)))
                            End If
                        ElseIf lstCheck(iListCounter) <> "" Then
                            drDetail("t" + CStr(iListCounter)) = row(lstCheck(iListCounter))
                        Else
                            drDetail("t" + CStr(iListCounter)) = ""
                        End If
                    Next
                    drDetail.EndEdit()
                    dsReportInf.list_title.Rows.Add(drDetail)
                Next

                'ヘッダー格納
                Dim drHeader As DS0201P1.dtHeaderRow
                drHeader = dsReportInf.dtHeader.NewRow
                drHeader.BeginEdit()
                drHeader.c_committee_name = Me.cboCommittee.Text
                drHeader.k_belonging = ""
                drHeader.EndEdit()
                dsReportInf.dtHeader.Rows.Add(drHeader)

                Dim drHeader1 As DS0201P1.list_itemRow
                drHeader1 = dsReportInf.list_item.NewRow
                drHeader1.BeginEdit()
                For iListCounter = 0 To lstCheck.Count - 1
                    If lstCheck(iListCounter) = "メール" Then
                        drHeader1("i" + CStr(iListCounter)) = "E-Mail"
                    ElseIf lstCheck(iListCounter) = "組合支部" Then
                        drHeader1("i" + CStr(iListCounter)) = "支部"
                    ElseIf lstCheck(iListCounter) = "機種略称" Then
                        drHeader1("i" + CStr(iListCounter)) = "機種"
                    ElseIf lstCheck(iListCounter) = "電話番号1" Then
                        drHeader1("i" + CStr(iListCounter)) = "電話番号"
                    Else
                        drHeader1("i" + CStr(iListCounter)) = lstCheck(iListCounter)
                    End If
                Next
                drHeader1.EndEdit()
                dsReportInf.list_item.Rows.Add(drHeader1)

                fmPrint.ObjResource = reportObj
                reportObj.SetDataSource(dsReportInf)
                fmPrint.ButtonShowType = 3
                fmPrint.ShowDialog()
                If fmPrint.IntQlickBtnFlag = 3 Then
                    Call fmPrint.PrintOut()
                End If
            Else
                'データなし
                CLMsg.Show("GE0202")
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnPrePrint_Click")
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub
#End Region

#Region "イベント：エンターキー押下検索対応"
    Private Sub cboCommittee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboCommittee.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
                '検索処理
                Call SearchMain()
                log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
            Catch ex As Exception
                log.Fatal(ex.Message)
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "cboCommittee_KeyPress")
            Finally
                'カーソルを元に戻す
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub cboYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboYear.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
                '検索処理
                Call SearchMain()
                log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "cboYear_KeyPress")
            Finally
                'カーソルを元に戻す
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub

    Private Sub cboMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboMonth.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
                '検索処理
                Call SearchMain()
                log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnSearch_Click")
            Finally
                'カーソルを元に戻す
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub
#End Region

#Region "イベント：コンボボックス変更データクリア対応"
    Private Sub cboCommittee_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboCommittee.SelectedIndexChanged
        '委員会構成一覧のデータクリア
        ClearCommitteeMemberGrid()
    End Sub

    Private Sub cboYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYear.SelectedIndexChanged
        '委員会構成一覧のデータクリア
        ClearCommitteeMemberGrid()

        If Me.cboMonth.Items.Count > 0 Then
            Me.cboMonth.SelectedIndex = 0
        End If
    End Sub

    Private Sub cboMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMonth.SelectedIndexChanged
        '委員会構成一覧のデータクリア
        ClearCommitteeMemberGrid()
    End Sub
#End Region

#Region "イベント：btnDelList_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnDelList_Click
    '   名称　：削除者リストボタンクリック処理
    '   概要  ：
    '   作成日：2011/12/02 
    '***************************************************************************************************
    Private Sub btnDelList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelList.Click
        Dim fm As FM020104 = New FM020104()
        Dim intCnt = 0
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            If dtDeleteList.Rows.Count > 0 Then
                'リスト件数からグリッドの列数設定
                fm.dgdDelMemberList.RowCount = dtDeleteList.Rows.Count
                For Each row As DataRow In dtDeleteList.Rows
                    If dicPost.ContainsKey(row.Item(L_POST_NAME)) Then
                        '役職
                        fm.dgdDelMemberList.Rows(intCnt).Cells(0).Value = dicPost(row.Item(L_POST_NAME))
                    End If
                    '名前
                    fm.dgdDelMemberList.Rows(intCnt).Cells(1).Value = row.Item(STR_COLUMNNAME_NAME)
                    '社員番号
                    fm.dgdDelMemberList.Rows(intCnt).Cells(2).Value = row.Item(STR_COLUMNNAME_STAFFID)
                    '機種
                    fm.dgdDelMemberList.Rows(intCnt).Cells(3).Value = row.Item(STR_COLUMNNAME_MODEL)
                    '資格
                    fm.dgdDelMemberList.Rows(intCnt).Cells(4).Value = row.Item(STR_COLUMNNAME_QUOLIFICATION)
                    '組合支部
                    fm.dgdDelMemberList.Rows(intCnt).Cells(5).Value = row.Item(STR_COLUMNNAME_BELONGING)
                    intCnt = intCnt + 1
                Next
            End If

            fm.grpDeleteMember.Text = "委員会名簿 削除者(" & dtDeleteList.Rows.Count.ToString() & "件)"
            '削除者一覧画面の表示
            fm.ShowDialog()

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnDelList_Click")
        End Try
    End Sub
#End Region

#Region "イベント：btnLeft_Cick"
    '***************************************************************************************************
    '   ＩＤ　：btnLeft_Cick
    '   名称　：＜ボタンクリック処理
    '   概要  ：
    '   作成日：2011/11/20
    '***************************************************************************************************
    Private Sub btnLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLeft.Click
        '委員会構成一覧へメンバー追加
        Call Me.AddUnionMember()
    End Sub
#End Region

#Region "イベント：btnRight_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnRight_Click
    '   名称　：＞ボタンクリック処理
    '   概要  ：
    '   作成日：2011/12/02 
    '***************************************************************************************************
    Private Sub btnRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRight.Click
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            If ChkUnionMemberActivity(Me.dgdCommitteeComposition.SelectedRows) = False Then
                Exit Sub
            End If

            '列追加
            If dtDeleteList Is Nothing Then
                dtDeleteList = New DataTable
            End If
            If dtDeleteList.Columns.Count = 0 Then
                For i = 0 To dgdCommitteeComposition.ColumnCount - 1
                    dtDeleteList.Columns.Add(dgdCommitteeComposition.Columns.Item(i).Name)
                Next
            End If

            'データベース登録済ユーザの場合は削除リストへ追加
            For Each selectRow As DataGridViewRow In Me.dgdCommitteeComposition.SelectedRows
                Dim InsertRow As Integer = dtDeleteList.Rows.Count
                If selectRow.Cells(FLAG).Value.ToString = "0" Then '既存組合員
                    Call dtDeleteList.Rows.Add()    '削除リスト行追加
                    For colAddCnt = 0 To dgdCommitteeComposition.ColumnCount - 1
                        Dim strInsertData As String = ""
                        If Not (IsDBNull(selectRow.Cells(dgdCommitteeComposition.Columns(colAddCnt).Name))) Then
                            'If dgdCommitteeComposition.Columns(colAddCnt).Name = L_POST_NAME Then
                            '    strInsertData = selectRow.Cells(dgdCommitteeComposition.Columns(colAddCnt).Name).
                            'Else
                            strInsertData = selectRow.Cells(dgdCommitteeComposition.Columns(colAddCnt).Name).Value.ToString
                            'End If
                        End If
                        dtDeleteList.Rows.Item(InsertRow).Item(dgdCommitteeComposition.Columns(colAddCnt).Name) = strInsertData
                    Next
                    selectRow.Visible = False
                Else
                    Me.dgdCommitteeComposition.Rows.Remove(selectRow)
                End If
            Next
            Call Me.btnDeleteListVisibleControl()
            '表示中の組合員人数を設定
            Call Me.CountVisibleMember()

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnAdd_Click")
        End Try

    End Sub
#End Region

#Region "イベント：btnCopyLastPeriod_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnCopyLastPeriod_Click
    '   名称　：前期からコピーボタンクリック処理
    '   概要  ：
    '   作成日：2011/12/06  onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/06  onuma  新規作成
    '         ：2012/08/21  Fujisaku flag初期値を1に修正
    '***************************************************************************************************
    Private Sub btnCopyLastPeriod_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCopyLastPeriod.Click
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        '前期ID
        Dim lastPeriodId As String = String.Empty
        '前期ID取得用SQL
        Dim strSql As String = String.Empty
        '委員会名簿明細取得用SQL
        Dim strSqlCommitteeList As String = String.Empty
        Dim dtRet As DataTable = New DataTable()
        Dim dtCommitteeDtl As DataTable = New DataTable()

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

            If CLMsg.Show("GQ0021", "組合員", "役職") = DialogResult.Yes Then

                Me.Cursor = Cursors.WaitCursor
                'DB接続開始
                clsDb.Connect()

                '前期IDを取得
                strSql = "SELECT c_period_id FROM period WHERE d_to = " &
                         " (SELECT MAX(d_to) FROM period WHERE CLng(d_to) < " &
                         " (SELECT d_from FROM period WHERE c_period_id = '" & MDLoginInfo.PeriodId & "'))"
                dtRet = clsDb.ExecuteSql(strSql)

                If dtRet.Rows.Count = 1 Then
                    lastPeriodId = dtRet.Rows(0).Item(0)

                    strSqlCommitteeList = "SELECT staf.c_user_id AS 社員番号, " &
                                          "       staf.l_name AS 名前, " &
                                          "       staf.k_user_status AS ステータス, " &
                                          "       staf.k_staf_kind AS 組合員種別, " &
                                          "       staf.組合支部 AS 組合支部, " &
                                          "       cld.役職 AS 役職, " &
                                          "       staf.機種 AS 機種, " &
                                          "       staf.資格 AS 資格, " &
                                          "       staf.会社所属 AS 会社所属, " &
                                          "       '' AS 備考, " &
                                          "       address.l_tell_1 AS 電話番号1, " &
                                          "       staf.機種略称 AS 機種略称, " &
                                          "       address.l_mail_pc As メール, " &
                                          "       cld.s_committee_seq AS 役職ID, " &
                                          "       1 As flag " &
                                          " FROM (SELECT com_list_dtl.* ,com_dtl.l_name AS 役職 " &
                                          "       FROM committee_list_dtl com_list_dtl, committee_dtl com_dtl, " &
                                          "            (SELECT t1.c_committee_list" &
                                          "             FROM committee_list AS t1, " &
                                          "                  (SELECT c_committee_id, c_period_id, MAX(d_from) AS max_from " &
                                          "                   FROM committee_list " &
                                          "                   WHERE c_period_id ='" & lastPeriodId & "' " &
                                          "                    AND  c_committee_id = '" & Me.cboCommittee.SelectedValue & "' " &
                                          "                    GROUP BY c_period_id,c_committee_id) AS t2 " &
                                          "             WHERE t1.c_committee_id = t2.c_committee_id " &
                                          "              AND  t1.d_from = t2.max_from " &
                                          "              AND  t1.c_period_id = t2.c_period_id " &
                                          "              AND  t1.c_period_id ='" & lastPeriodId & "' " &
                                          "              AND  t1.c_committee_id = '" & Me.cboCommittee.SelectedValue & "') AS listMax " &
                                          "       WHERE com_list_dtl.c_committee_id = com_dtl.c_committee_id " &
                                          "        AND  com_list_dtl.s_committee_seq = com_dtl.s_committee_seq " &
                                          "        AND  com_list_dtl.c_committee_list = listMax.c_committee_list " &
                                          "        AND  com_dtl.d_from <= '" & Me.searchStandardDay & "'" &
                                          "        AND  com_dtl.d_to >= '" & Me.searchStandardDay & "'" &
                                          "         ) cld INNER JOIN  " &
                                          "        ( " &
                                          "          (SELECT attr.*, dtl1.l_name AS 組合支部, dtl2.l_name AS 機種, dtl2.l_omission_name AS 機種略称, " &
                                          "                  dtl3.l_omission_name AS 資格, dtl4.l_name AS 会社所属 " &
                                          "           FROM staf_attribute attr , " &
                                          "                constant_dtl dtl1, " &
                                          "                constant_dtl dtl2, " &
                                          "                constant_dtl dtl3, " &
                                          "                constant_dtl dtl4, " &
                                          "                (SELECT c_user_id, MAX(d_from) AS MAXDAY " &
                                          "                 FROM staf_attribute " &
                                          "                 WHERE d_from <= '" & Me.searchStandardDay & "' " &
                                          "                 GROUP BY c_user_id, c_ksh, c_staf_id " &
                                          "                ) attr2 " &
                                          "           WHERE attr.c_user_id = attr2.c_user_id " &
                                          "           AND attr.d_from = attr2.MAXDAY " &
                                          "           AND dtl1.c_constant = 'BELONGING' AND dtl1.c_constant_seq = attr.k_belonging " &
                                          "           AND dtl2.c_constant = 'MODEL' AND dtl2.c_constant_seq = attr.k_model " &
                                          "           AND dtl3.c_constant = 'QUALIFICATION' AND dtl3.c_constant_seq = attr.k_qualification " &
                                          "           AND dtl4.c_constant = 'AREA_LOCAL' AND dtl4.c_constant_seq = attr.k_local " &
                                          "           ) staf LEFT OUTER JOIN  " &
                                          "             (SELECT ad.*  " &
                                          "              FROM staf_address ad , " &
                                          "                  (SELECT c_user_id, MAX(d_from) AS MAXDAY " &
                                          "                   FROM staf_address " &
                                          "                   WHERE d_from <= '" & searchStandardDay & "' " &
                                          "                   GROUP BY c_user_id " &
                                          "                   ) ad2               " &
                                          "              WHERE ad.c_user_id = ad2.c_user_id " &
                                          "              AND ad.d_from = ad2.MAXDAY " &
                                          "              AND ad.k_main_add ='True'  " &
                                          "             ) address ON staf.c_user_id = address.c_user_id " &
                                          "            ) ON cld.c_user_id = staf.c_user_id  " &
                                          "ORDER BY cld.s_committee_seq, CLng(staf.c_user_id) " & UtDb.DbOrderOffset

                    dtCommitteeDtl = clsDb.ExecuteSql(strSqlCommitteeList)

                    If dtCommitteeDtl.Rows.Count > 0 Then
                        '委員会名簿一覧表示処理
                        SetCommitteeCompositionList(dtCommitteeDtl, True)
                        Me.grpCommittee.Text = "委員会構成一覧 (" + dtCommitteeDtl.Rows.Count.ToString + "件)"
                    Else
                        '該当データが存在しなかった場合
                        CLMsg.Show("GE0015")
                    End If
                End If
            End If
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnCopyLastPeriod_Click")
        Finally
            'DB接続終了
            clsDb.Disconnect()
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region "イベント：btnDelete_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnDelete_Click
    '   名称　：削除ボタンクリック処理
    '   概要  ：
    '   作成日：2011/12/07  onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/07  onuma  新規作成
    '***************************************************************************************************
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim intRet As Integer = -1
        Dim strCommitteeListKey As String
        strCommitteeListKey = MDLoginInfo.PeriodId & "_" & Me.cboCommittee.SelectedValue & "_" & Me.cboYear.Text + Me.cboMonth.Text + "01"

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        If CLMsg.Show("GQ0005") = DialogResult.No Then
            'いいえが選択された場合処理を終了
            Exit Sub
        End If

        Try
            Me.Cursor = Cursors.WaitCursor
            '活動が出欠簿または中執活動報告に登録されている場合は削除不可
            If ChkUnionMemberActivity(Me.dgdCommitteeComposition.Rows) = False Then
                Exit Sub
            End If

            strSql = "DELETE FROM committee_list WHERE" &
                    " c_committee_list = '" & strCommitteeListKey & "' "

            clsDb.Connect()
            'トランザクション開始
            clsDb.BeginTran()

            '削除処理実行
            intRet = clsDb.ExecuteNonQuery(strSql)

            If intRet < 1 Then
                clsDb.RollbackTran()
                Exit Sub
            End If

            strSql = "DELETE FROM committee_list_dtl WHERE" &
                    " c_committee_list = '" & strCommitteeListKey & "' "
            intRet = clsDb.ExecuteNonQuery(strSql)

            If intRet < 1 Then
                clsDb.RollbackTran()
                Exit Sub
            Else
                '問題なく削除できたらコミット
                clsDb.CommitTran()
                '検索実行
                Call Me.getSearchData()
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnDelete_Click")
            clsDb.RollbackTran()
        Finally
            clsDb.Disconnect()
            'カーソルを元に戻す
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region "イベント：cboUnionBranch_SelectionChangeCommitted"
    '***************************************************************************************************
    '   ＩＤ　：cboUnionBranch_SelectionChangeCommitted
    '   名称　：組合支部コンボボックス値変更
    '   概要  ：
    '   作成日：2011/12/05  somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/05  somezaki  新規作成
    '***************************************************************************************************
    Private Sub cboUnionBranch_SelectionChangeCommitted(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboUnionBranch.SelectionChangeCommitted
        Dim intCnt As Integer = 0
        Try
            If Not ChkNull(Me.cboUnionBranch.Text) Then
                For i = 0 To Me.dgdCommitteeComposition.Rows.Count - 1
                    If Me.dgdCommitteeComposition.Rows(i).Cells(STR_COLUMNNAME_BELONGING).Value <> Me.cboUnionBranch.Text Then
                        Me.dgdCommitteeComposition.Rows(i).Visible = False
                    Else
                        Me.dgdCommitteeComposition.Rows(i).Visible = True
                        intCnt = intCnt + 1
                    End If
                Next
            Else
                For i = 0 To Me.dgdCommitteeComposition.Rows.Count - 1
                    Me.dgdCommitteeComposition.Rows(i).Visible = True
                    intCnt = intCnt + 1
                Next
            End If

            Me.grpCommittee.Text = "委員会構成一覧 (" + intCnt.ToString + "件)"
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "cboUnionBranch_SelectedIndexChanged")
        End Try
    End Sub
#End Region

#Region "イベント：tclUnionMemberExtraction_SelectedIndexChanged"
    '***************************************************************************************************
    '   ＩＤ　：tclUnionMemberExtraction_SelectedIndexChanged
    '   名称　：タブコントロールインデックス変更
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/04 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/04 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub tclJpnSyllabary_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tclJpnSyllabary.SelectedIndexChanged
        Try
            For Each cControl As Control In tclJpnSyllabary.SelectedTab.Controls()
                If TypeOf cControl Is DataGridView Then ' グリッドコントロールを取得
                    showDataGridView = cControl
                End If
            Next cControl
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "tclJpnSyllabary_SelectedIndexChanged")
        End Try
    End Sub
#End Region

#Region "イベント：tclUnionMemberExtraction_SelectedIndexChanged"
    '***************************************************************************************************
    '   ＩＤ　：tclUnionMemberExtraction_SelectedIndexChanged
    '   名称　：タブコントロールインデックス変更
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/04 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/04 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub tclUnionMemberExtraction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tclUnionMemberExtraction.SelectedIndexChanged
        'どのタブが選択されたかチェック
        If tclUnionMemberExtraction.SelectedTab.Equals(tbpJpnSyllabaryPage) Then '50音タブ
            'さらにこのページに含まれるタブで選択されているページを取得する。
            For Each cControl As Control In tclJpnSyllabary.SelectedTab.Controls()
                ' 列挙したコントロールのグリッドを取得
                If TypeOf cControl Is DataGridView Then
                    showDataGridView = cControl
                End If
            Next cControl
        ElseIf tclUnionMemberExtraction.SelectedTab.Equals(tbpStafIdPage) Then
            showDataGridView = dgdStafIDResult '社員番号タブ
        End If
    End Sub
#End Region

#Region "イベント：btnCancel_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要  ：キャンセルボタンクリック
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/01 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/01 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            '組合員抽出機能側をクリア
            Call Me.ClearGrpUnionMember()
            '編集モード変更
            Call Me.EditModeChange(False)
            '検索結果クリア
            Call Me.ClearCommitteeMemberGrid()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnCancel_Click")
        End Try
    End Sub
#End Region

#Region "イベント：btnInsert_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnInsert_Click
    '   名称　：登録ボタンクリック
    '   概要  ：登録ボタンクリック
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/02 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02 m.somezaki  新規作成
    '         ：2012/02/16 Fujisaku 更新　委員会名簿更新情報対応
    '***************************************************************************************************

    Private Sub btnInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsert.Click

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim strSql As String                ' SQL文
        Dim insSql As String                ' SQL文
        Dim delSql As String                ' SQL文
        Dim intRet As Integer = 0           ' 件数取得
        Dim dtRet As DataTable = New DataTable
        Dim kshRet As DataTable = New DataTable
        Dim blnCommit = True                    ' コミットフラグ
        Dim strCommitteeListID As String        ' 委員会名簿ID
        Dim strCommitteeUpdate As String        ' 委員会名簿変更ID
        Dim strCommitteeksh As String = ""      ' 会社コード
        Dim nowDate As Date = Now               ' 現在日
        Dim strNowDate As String                ' 現在日(作成日/更新日用)
        Dim strSelDFrom As String               ' 選択年月(d_from:適用開始年月日)
        Dim intCntUpdate As Integer             ' 更新回数(s_up)
        Dim strAftUserId As String              ' 社員番号
        Dim strAftComSeq As String              ' 役職ID
        Dim strDelUserId As String              ' 社員番号
        Dim strDelComSeq As String              ' 役職ID
        Dim strHeadFlg As String                ' 長フラグ

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            Me.Cursor = Cursors.WaitCursor
            '委員会構成員が登録可能であるかチェック
            If Me.ChkCanInsert() = False Then
                Exit Sub
            End If

            Dim fm As New FM000203
            'プレビュー画面表示
            Dim intBtnRet As Integer = ShowPrintPreview(fm)

            If intBtnRet = 0 OrElse intBtnRet = 1 Then '登録のみ、または登録＆印刷ボタンが選択された場合登録処理を行う

                '同期処理による最新データの取得
                ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)

                Call clsDb.Connect()            ' データベース接続
                Call clsDb.BeginTran()          ' トランザクション開始処理

                '委員会名簿IDの生成
                strCommitteeListID = MDLoginInfo.PeriodId & "_" & Me.cboCommittee.SelectedValue & "_" & Me.cboYear.Text + Me.cboMonth.Text + "01"

                '委員会名簿変更IDの生成
                strCommitteeUpdate = Me.getCommitteeUpdate(clsDb)

                '会社コードを取得
                strSql = "SELECT committee.c_ksh FROM committee"
                strSql = strSql & " WHERE c_committee_id = '" & Me.cboCommittee.SelectedValue & "'"
                kshRet = clsDb.ExecuteSql(strSql)
                If kshRet.Rows.Count > 0 Then
                    strCommitteeksh = kshRet.Rows(0).Item("c_ksh").ToString
                End If

                'd_from値を設定(コンボボックス選択年＋月＋01)
                strSelDFrom = Me.cboYear.Text & Me.cboMonth.Text & "01"

                '現在日付を設定
                strNowDate = nowDate.ToString

                '委員会マスタ詳細(役職マスタ)を取得
                Dim committeeDtlDao As New CommitteeDtlDao
                Dim tblCommitteeDtl As DataTable
                tblCommitteeDtl = committeeDtlDao.GetDataOfDate(strSelDFrom).Tables.Item("committee_dtl").Copy()


                ' **************************************************
                '  ヘッダ情報のINSERT/UPDATE
                ' **************************************************
                '選択している委員会名簿IDの存在確認
                strSql = "SELECT committee_list.c_committee_list, committee_list.s_up FROM committee_list " & vbCrLf
                strSql = strSql & "WHERE committee_list.c_period_id = '" & MDLoginInfo.PeriodId & "' " & vbCrLf
                strSql = strSql & "AND committee_list.c_committee_id = '" & Me.cboCommittee.SelectedValue & "' "
                strSql = strSql & "AND committee_list.d_from = '" & strSelDFrom & "' "

                dtRet = clsDb.ExecuteSql(strSql)

                If dtRet.Rows.Count = 0 Then
                    '委員会一覧のINSERT
                    insSql = "INSERT INTO committee_list ( " & vbCrLf
                    insSql = insSql & "c_committee_list," & vbCrLf
                    insSql = insSql & "c_ksh," & vbCrLf
                    insSql = insSql & "c_period_id," & vbCrLf
                    insSql = insSql & "c_committee_id," & vbCrLf
                    insSql = insSql & "d_from," & vbCrLf
                    insSql = insSql & "l_biko," & vbCrLf
                    insSql = insSql & "d_ins," & vbCrLf
                    insSql = insSql & "c_user_id_ins," & vbCrLf
                    insSql = insSql & "d_up," & vbCrLf
                    insSql = insSql & "c_user_id_up," & vbCrLf
                    insSql = insSql & "s_up" & vbCrLf
                    insSql = insSql & ") VALUES (" & vbCrLf
                    insSql = insSql & "'" & strCommitteeListID & "', " & vbCrLf
                    insSql = insSql & "'" & strCommitteeksh & "', " & vbCrLf
                    insSql = insSql & "'" & MDLoginInfo.PeriodId & "', " & vbCrLf
                    insSql = insSql & "'" & Me.cboCommittee.SelectedValue & "', " & vbCrLf
                    insSql = insSql & "'" & strSelDFrom & "', " & vbCrLf
                    insSql = insSql & "'', " & vbCrLf
                    insSql = insSql & "'" & strNowDate & "', " & vbCrLf
                    insSql = insSql & "'" & MDLoginInfo.UserId & "', " & vbCrLf
                    insSql = insSql & "'" & strNowDate & "', " & vbCrLf
                    insSql = insSql & " '" & MDLoginInfo.UserId & "', " & vbCrLf
                    insSql = insSql & "0" & vbCrLf
                    insSql = insSql & ")" & vbCrLf
                Else
                    '委員会名簿IDは既存のレコードの値を使用
                    strCommitteeListID = dtRet.Rows.Item(0).Item("c_committee_list")

                    '更新回数のカウントアップ
                    intCntUpdate = Integer.Parse(dtRet.Rows(0).Item("s_up").ToString)
                    intCntUpdate = intCntUpdate + 1

                    '委員会一覧のUPDATE
                    insSql = "UPDATE committee_list " & vbCrLf
                    insSql = insSql & " SET d_up = '" & strNowDate & "'" & vbCrLf
                    insSql = insSql & "    ,c_user_id_up = '" & MDLoginInfo.UserId & "'" & vbCrLf
                    insSql = insSql & "    ,s_up = " & intCntUpdate.ToString & vbCrLf
                    insSql = insSql & "WHERE committee_list.c_committee_list = '" & strCommitteeListID & "' " & vbCrLf
                    insSql = insSql & "  AND committee_list.c_ksh = '" & strCommitteeksh & "' "
                    insSql = insSql & "  AND committee_list.c_period_id = '" & MDLoginInfo.PeriodId & "' " & vbCrLf
                    insSql = insSql & "  AND committee_list.c_committee_id = '" & Me.cboCommittee.SelectedValue & "' "
                    insSql = insSql & "  AND committee_list.d_from = '" & strSelDFrom & "' "
                End If

                ' 委員会一覧のINSERT/UPDATE実行
                intRet = clsDb.ExecuteNonQuery(insSql)
                If intRet = -1 Then
                    blnCommit = False
                End If

                If blnCommit Then
                    '委員会更新一覧のINSERT
                    insSql = "INSERT INTO committee_update_list ( " & vbCrLf
                    insSql = insSql & "c_committee_update," & vbCrLf
                    insSql = insSql & "c_ksh," & vbCrLf
                    insSql = insSql & "c_period_id," & vbCrLf
                    insSql = insSql & "c_committee_id," & vbCrLf
                    insSql = insSql & "d_from," & vbCrLf
                    insSql = insSql & "k_document_out," & vbCrLf
                    insSql = insSql & "l_biko," & vbCrLf
                    insSql = insSql & "d_ins," & vbCrLf
                    insSql = insSql & "c_user_id_ins" & vbCrLf
                    insSql = insSql & ") VALUES (" & vbCrLf
                    insSql = insSql & "'" & strCommitteeUpdate & "', " & vbCrLf
                    insSql = insSql & "'" & strCommitteeksh & "', " & vbCrLf
                    insSql = insSql & "'" & MDLoginInfo.PeriodId & "', " & vbCrLf
                    insSql = insSql & "'" & Me.cboCommittee.SelectedValue & "', " & vbCrLf
                    insSql = insSql & "'" & strSelDFrom & "', " & vbCrLf
                    insSql = insSql & "0, " & vbCrLf
                    insSql = insSql & "'', " & vbCrLf
                    insSql = insSql & "'" & strNowDate & "', " & vbCrLf
                    insSql = insSql & "'" & MDLoginInfo.UserId & "' " & vbCrLf
                    insSql = insSql & ")" & vbCrLf

                    intRet = clsDb.ExecuteNonQuery(insSql)
                    If intRet = -1 Then
                        blnCommit = False
                    End If
                End If

                ' **************************************************
                '  明細情報のINSERT/UPDATE
                ' **************************************************
                '名簿が存在していれば組合員情報を登録する
                If blnCommit Then
                    For Each updateRow As DataGridViewRow In dgdCommitteeComposition.Rows

                        ' 委員会構成一覧の情報を取得
                        strAftUserId = updateRow.Cells(STR_COLUMNNAME_STAFFID).Value
                        strAftComSeq = updateRow.Cells(L_POST_NAME).Value
                        'strAftComSeq = updateRow.Cells(S_COMMITTEE_SEQ).Value

                        ' 長フラグの取得
                        strSql = "c_committee_id = '" & Me.cboCommittee.SelectedValue & "' AND s_committee_seq = '" & strAftComSeq & "'"
                        Dim rowArray As DataRow() = tblCommitteeDtl.Select(strSql)
                        If (rowArray.Length > 0) Then
                            strHeadFlg = rowArray(0).Item("k_head_flg").ToString
                        Else
                            strHeadFlg = "0"
                        End If


                        ' 現在の委員会一覧明細の取得
                        strSql = "SELECT committee_list_dtl.c_user_id , committee_list_dtl.s_committee_seq ,committee_list_dtl.s_up " & vbCrLf
                        strSql = strSql & "FROM committee_list_dtl " & vbCrLf
                        strSql = strSql & "WHERE committee_list_dtl.c_committee_list = '" & strCommitteeListID & "'" & vbCrLf
                        strSql = strSql & " AND committee_list_dtl.c_user_id = '" & strAftUserId & "'" & vbCrLf
                        strSql = strSql & " AND committee_list_dtl.c_committee_id = '" & Me.cboCommittee.SelectedValue & "'" & vbCrLf
                        strSql = strSql & " AND committee_list_dtl.d_from = '" & strSelDFrom & "' "
                        dtRet = clsDb.ExecuteSql(strSql)

                        If dtRet.Rows.Count = 0 Then
                            '委員会一覧明細のINSERT
                            insSql = "INSERT INTO committee_list_dtl (" & vbCrLf
                            insSql = insSql & " c_committee_list, " & vbCrLf
                            insSql = insSql & "c_user_id, " & vbCrLf
                            insSql = insSql & "d_from, " & vbCrLf
                            insSql = insSql & "c_committee_id, " & vbCrLf
                            insSql = insSql & "s_committee_seq, " & vbCrLf
                            insSql = insSql & "l_biko, " & vbCrLf
                            insSql = insSql & "d_ins, " & vbCrLf
                            insSql = insSql & "c_user_id_ins, " & vbCrLf
                            insSql = insSql & "d_up, " & vbCrLf
                            insSql = insSql & "c_user_id_up, " & vbCrLf
                            insSql = insSql & "s_up) " & vbCrLf
                            insSql = insSql & "VALUES (" & vbCrLf
                            insSql = insSql & "'" & strCommitteeListID & "'," & vbCrLf
                            insSql = insSql & "'" & strAftUserId & "'," & vbCrLf
                            insSql = insSql & "'" & strSelDFrom & "'," & vbCrLf
                            insSql = insSql & "'" & Me.cboCommittee.SelectedValue & "'," & vbCrLf
                            insSql = insSql & "'" & strAftComSeq & "'," & vbCrLf
                            insSql = insSql & "''," & vbCrLf
                            insSql = insSql & "'" & strNowDate & "'," & vbCrLf
                            insSql = insSql & "'" & MDLoginInfo.UserId & "'," & vbCrLf
                            insSql = insSql & "'" & strNowDate & "'," & vbCrLf
                            insSql = insSql & "'" & MDLoginInfo.UserId & "'," & vbCrLf
                            insSql = insSql & "0 )" & vbCrLf
                            intRet = clsDb.ExecuteNonQuery(insSql)      ' SQL実行
                            If intRet = -1 Then
                                blnCommit = False
                                Exit For
                            End If

                        ElseIf dtRet.Rows.Item(0).Item("s_committee_seq").ToString <> strAftComSeq Then
                            '更新回数のカウントアップ
                            intCntUpdate = Integer.Parse(dtRet.Rows(0).Item("s_up").ToString)
                            intCntUpdate = intCntUpdate + 1

                            '委員会一覧明細のUPDATE
                            insSql = "UPDATE committee_list_dtl " & vbCrLf
                            insSql = insSql & " SET s_committee_seq = '" & strAftComSeq & "'" & vbCrLf
                            insSql = insSql & "     ,d_up = '" & strNowDate & "'" & vbCrLf
                            insSql = insSql & "     ,c_user_id_up = '" & MDLoginInfo.UserId & "'" & vbCrLf
                            insSql = insSql & "     ,s_up = " & intCntUpdate.ToString & vbCrLf
                            insSql = insSql & "  WHERE committee_list_dtl.c_committee_list = '" & strCommitteeListID & "'" & vbCrLf
                            insSql = insSql & "    AND committee_list_dtl.c_user_id = '" & strAftUserId & "'" & vbCrLf
                            insSql = insSql & "    AND committee_list_dtl.d_from = '" & strSelDFrom & "'"

                            intRet = clsDb.ExecuteNonQuery(insSql)      ' SQL実行
                            If intRet = -1 Then
                                blnCommit = False
                                Exit For
                            End If
                        End If

                        '変更前情報との比較を行う
                        '社員番号が一致するレコードが変更前情報に含まれるか確認
                        strSql = STR_COLUMNNAME_STAFFID & " = '" & strAftUserId & "'"
                        Dim beforeRow As DataRow() = Nothing
                        If Not tbBefore Is Nothing Then
                            beforeRow = tbBefore.Select(strSql)
                        End If

                        If beforeRow Is Nothing OrElse beforeRow.Length() = 0 Then
                            '委員会更新一覧のINSERT（0:追加）
                            insSql = "INSERT INTO committee_update_list_dtl ( " & vbCrLf
                            insSql = insSql & "c_committee_update," & vbCrLf
                            insSql = insSql & "c_user_id," & vbCrLf
                            insSql = insSql & "k_committee_insert," & vbCrLf
                            insSql = insSql & "c_committee_id," & vbCrLf
                            insSql = insSql & "s_committee_seq," & vbCrLf
                            insSql = insSql & "k_related_head," & vbCrLf
                            insSql = insSql & "s_order," & vbCrLf
                            insSql = insSql & "l_biko," & vbCrLf
                            insSql = insSql & "d_ins," & vbCrLf
                            insSql = insSql & "c_user_id_ins " & vbCrLf
                            insSql = insSql & ") VALUES (" & vbCrLf
                            insSql = insSql & "'" & strCommitteeUpdate & "', " & vbCrLf
                            insSql = insSql & "'" & strAftUserId & "', " & vbCrLf
                            insSql = insSql & "'" & K_COMMITTEE_INSERT_ADD & "', " & vbCrLf
                            insSql = insSql & "'" & Me.cboCommittee.SelectedValue & "', " & vbCrLf
                            insSql = insSql & "'" & strAftComSeq & "', " & vbCrLf
                            insSql = insSql & "'" & strHeadFlg & "', " & vbCrLf
                            insSql = insSql & "'" & "1" & "', " & vbCrLf  ' 追加の時は s_order = 1
                            insSql = insSql & "'', " & vbCrLf
                            insSql = insSql & "'" & strNowDate & "', " & vbCrLf
                            insSql = insSql & "'" & MDLoginInfo.UserId & "' " & vbCrLf
                            insSql = insSql & ")" & vbCrLf

                            intRet = clsDb.ExecuteNonQuery(insSql)      ' SQL実行
                            If intRet = -1 Then
                                blnCommit = False
                                Exit For
                            End If

                        ElseIf Not beforeRow(0).Item(S_COMMITTEE_SEQ).Equals(updateRow.Cells(L_POST_NAME).Value) Then
                            '委員会更新一覧のINSERT（2:変更）
                            insSql = "INSERT INTO committee_update_list_dtl ( " & vbCrLf
                            insSql = insSql & "c_committee_update," & vbCrLf
                            insSql = insSql & "c_user_id," & vbCrLf
                            insSql = insSql & "k_committee_insert," & vbCrLf
                            insSql = insSql & "c_committee_id," & vbCrLf
                            insSql = insSql & "s_committee_seq," & vbCrLf
                            insSql = insSql & "k_related_head," & vbCrLf
                            insSql = insSql & "s_order," & vbCrLf
                            insSql = insSql & "l_biko," & vbCrLf
                            insSql = insSql & "d_ins," & vbCrLf
                            insSql = insSql & "c_user_id_ins " & vbCrLf
                            insSql = insSql & ") VALUES (" & vbCrLf
                            insSql = insSql & "'" & strCommitteeUpdate & "', " & vbCrLf
                            insSql = insSql & "'" & strAftUserId & "', " & vbCrLf
                            insSql = insSql & "'" & K_COMMITTEE_INSERT_CHANGE & "', " & vbCrLf
                            insSql = insSql & "'" & Me.cboCommittee.SelectedValue & "', " & vbCrLf
                            insSql = insSql & "'" & strAftComSeq & "', " & vbCrLf
                            insSql = insSql & "'" & strHeadFlg & "', " & vbCrLf
                            insSql = insSql & "'" & "1" & "', " & vbCrLf  ' 変更の時は s_order = 1
                            insSql = insSql & "'', " & vbCrLf
                            insSql = insSql & "'" & strNowDate & "', " & vbCrLf
                            insSql = insSql & "'" & MDLoginInfo.UserId & "' " & vbCrLf
                            insSql = insSql & ")" & vbCrLf

                            intRet = clsDb.ExecuteNonQuery(insSql)      ' SQL実行
                            If intRet = -1 Then
                                blnCommit = False
                                Exit For
                            End If
                        End If
                    Next
                End If

                ' **************************************************
                '  削除情報のDELETE/INSERT
                ' **************************************************
                If Not dtDeleteList Is Nothing Then
                    For Each dtDeleteRaw As DataRow In dtDeleteList.Rows

                        ' 削除者リストの情報を取得
                        strDelUserId = dtDeleteRaw.Item(STR_COLUMNNAME_STAFFID).ToString
                        strDelComSeq = dtDeleteRaw.Item(S_COMMITTEE_SEQ).ToString

                        ' 長フラグの取得
                        strSql = "c_committee_id = '" & Me.cboCommittee.SelectedValue & "' AND s_committee_seq = '" & strDelComSeq & "'"
                        Dim rowArray As DataRow() = tblCommitteeDtl.Select(strSql)
                        If (rowArray.Length > 0) Then
                            strHeadFlg = rowArray(0).Item("k_head_flg").ToString
                        Else
                            strHeadFlg = "0"
                        End If

                        '委員会一覧明細のDELETE
                        delSql = "DELETE FROM committee_list_dtl " & vbCrLf
                        delSql = delSql & "  WHERE committee_list_dtl.c_committee_list = '" + strCommitteeListID + "'" & vbCrLf
                        delSql = delSql & "    AND committee_list_dtl.c_user_id = '" & strDelUserId & "'" & vbCrLf
                        delSql = delSql & "    AND committee_list_dtl.d_from = '" & strSelDFrom & "'"

                        intRet = clsDb.ExecuteNonQuery(delSql)      ' SQL実行
                        If intRet = -1 Then
                            blnCommit = False
                            Exit For
                        End If

                        '委員会更新一覧のINSERT（1:削除）
                        insSql = "INSERT INTO committee_update_list_dtl ( " & vbCrLf
                        insSql = insSql & "c_committee_update," & vbCrLf
                        insSql = insSql & "c_user_id," & vbCrLf
                        insSql = insSql & "k_committee_insert," & vbCrLf
                        insSql = insSql & "c_committee_id," & vbCrLf
                        insSql = insSql & "s_committee_seq," & vbCrLf
                        insSql = insSql & "k_related_head," & vbCrLf
                        insSql = insSql & "s_order," & vbCrLf
                        insSql = insSql & "l_biko," & vbCrLf
                        insSql = insSql & "d_ins," & vbCrLf
                        insSql = insSql & "c_user_id_ins " & vbCrLf
                        insSql = insSql & ") VALUES (" & vbCrLf
                        insSql = insSql & "'" & strCommitteeUpdate & "', " & vbCrLf
                        insSql = insSql & "'" & strDelUserId & "', " & vbCrLf
                        insSql = insSql & "'" & K_COMMITTEE_INSERT_DELETE & "', " & vbCrLf
                        insSql = insSql & "'" & Me.cboCommittee.SelectedValue & "', " & vbCrLf
                        insSql = insSql & "'" & strDelComSeq & "', " & vbCrLf
                        insSql = insSql & "'" & strHeadFlg & "', " & vbCrLf
                        insSql = insSql & "'" & "2" & "', " & vbCrLf  ' 削除の時は s_order = 2
                        insSql = insSql & "'', " & vbCrLf
                        insSql = insSql & "'" & strNowDate & "', " & vbCrLf
                        insSql = insSql & "'" & MDLoginInfo.UserId & "' " & vbCrLf
                        insSql = insSql & ")" & vbCrLf

                        intRet = clsDb.ExecuteNonQuery(insSql)      ' SQL実行
                        If intRet = -1 Then
                            blnCommit = False
                            Exit For
                        End If
                    Next
                End If

                If blnCommit Then
                    clsDb.CommitTran()      ' トランザクションコミット
                    ''同期処理による最新データの反映
                    'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    CLMsg.Show("GI0015")
                Else
                    clsDb.RollbackTran()
                End If

                If intBtnRet = 0 Then
                    '登録＆印刷が選択されていた場合は印刷処理実行
                    fm.PrintOut()
                End If

                '組合員抽出機能側をクリア
                ClearGrpUnionMember()
                '編集モード変更
                Call Me.EditModeChange(False)
                '検索結果クリア
                Call Me.ClearCommitteeMemberGrid()

            End If
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnInsert_Click")
        Finally
            Call clsDb.Disconnect()
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region "イベント：btnSearchUnionMember_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnSearchUnionMember_Click
    '   名称　：検索ボタンクリック処理
    '   概要  ：
    '   作成日：2011/12/02 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02 somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnSearchUnionMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchUnionMember.Click
        Call Me.GetSearchUnionMember()
    End Sub
#End Region

#Region "イベント：btnSearchStafID_Click"
    '***************************************************************************************************
    '   ＩＤ　：btnSearchStafID_Click
    '   名称　：社員番号検索ボタン
    '   概要  ：社員番号検索ボタンクリック
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/02 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnSearchStafID_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchStafID.Click
        Call Me.GetSeachStafID()
    End Sub
#End Region

#Region "イベント：エンター押下対応 ユーザー追加"
    '***************************************************************************************************
    '   ＩＤ　：txtKana_KeyDown
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtKana_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtKana.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call Me.GetSearchUnionMember()
        End If
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtStafID_KeyDown
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtStafID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStafID.KeyDown
        If e.KeyCode = Keys.Enter Then
            Call Me.GetSeachStafID()
        End If
    End Sub
#End Region

#Region "イベント：ダブルクリック対応 ユーザー追加"
    '***************************************************************************************************
    '   ＩＤ　：dgdALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdKALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdKALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdKALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdSALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdSALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdSALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdTALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdTALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdTALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdNALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdNALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdNALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdHALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdHALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdHALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdMALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdMALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdMALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdYALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdYALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdYALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdRALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdRALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdRALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdWALine_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdWALine_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdWALine.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdOther_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdOther_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdOther.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdKanaSearchResult_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdKanaSearchResult_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdKanaSearchResult.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdStafIDResult_CellDoubleClick
    '   名称　：
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdStafIDResult_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdStafIDResult.CellDoubleClick
        '選択メンバーを構成一覧へ追加
        Call Me.AddUnionMember()
    End Sub
#End Region
#End Region

#Region "関数"
#Region "関数：MakeCommitteeCmbBox"
    '***************************************************************************************************
    '   ＩＤ　：MakeCommitteeCmbBox
    '   名称　：委員会コンボボックス作成
    '   概要  ：
    '   作成日：2011/12/01 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/01 somezaki  新規作成
    '***************************************************************************************************
    Private Function MakeCommitteeCmbBox(ByVal clsDb) As Boolean
        Dim blnRet = False
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            Dim strSql As String

            strSql = ""
            strSql = strSql & "SELECT l_name         AS DisplayName," & vbCrLf
            strSql = strSql & "       c_committee_id AS ValueName" & vbCrLf
            strSql = strSql & "FROM committee" & vbCrLf
            strSql = strSql & "WHERE FORMAT(GETDATE(), 'yyyyMMdd') BETWEEN d_from AND d_to" & vbCrLf
            strSql = strSql & "ORDER BY c_committee_id" 'chk

            ' 委員会コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, Me.cboCommittee, strSql, "DisplayName", "ValueName", False) = False Then
                Return blnRet
            End If
            blnRet = True

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "MakeCommitteeCmbBox")
        End Try
        Return blnRet

    End Function
#End Region

#Region "関数：SetPostList"
    '***************************************************************************************************
    '   ＩＤ　：SetPostList
    '   名称　：役職IDより役職リストの作成
    '   概要  ：
    '   作成日：2011/12/05 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/05 onuma  新規作成
    '***************************************************************************************************
    Private Sub SetPostList()
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing
        Dim strSql As String

        dicPost = New Dictionary(Of String, String)
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            '役職IDから表示する役職名を取得する
            strSql = "SELECT l_name AS DisplayName , s_committee_seq AS ValueMember " & vbCrLf
            strSql = strSql & "FROM committee_dtl " & vbCrLf
            strSql = strSql & "WHERE c_committee_id = '" + Me.cboCommittee.SelectedValue + "' " & vbCrLf
            strSql = strSql & " AND  d_from <= '" & Me.searchStandardDay & "' " & vbCrLf
            strSql = strSql & " AND  d_to >= '" & Me.searchStandardDay & "'"
            'DB接続
            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)

            '役職IDをキーに役職名を取得
            For Each row As DataRow In dtRet.Rows
                dicPost.Add(row.Item("ValueMember").ToString, row.Item("DisplayName").ToString)
            Next

            strSql = "SELECT MAX(CInt(s_committee_seq)) AS MAXSEQ FROM committee_dtl " & vbCrLf
            strSql = strSql & " WHERE c_committee_id = '" + Me.cboCommittee.SelectedValue + "' "
            strSql = strSql & " AND  d_from <= '" & Me.searchStandardDay & "' " & vbCrLf
            strSql = strSql & " AND  d_to >= '" & Me.searchStandardDay & "'"

            dtRet = clsDb.ExecuteSql(strSql)

            'デフォルトの選択行を最下位の役職にする
            If dtRet.Rows(0).Item(0) IsNot DBNull.Value Then
                strDefaultCommitteeSeq = dtRet.Rows(0).Item(0)
            End If
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.ConstructorInfo.GetCurrentMethod().Name)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

    End Sub
#End Region

#Region "関数：SetStafKindList"
    '***************************************************************************************************
    '   ＩＤ　：SetStafKindList
    '   名称　：組合員種別名のリストを作成
    '   概要  ：
    '   作成日：2011/12/07 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/07 onuma  新規作成
    '***************************************************************************************************
    Private Sub SetStafKindList()
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing

        dicStafKind = New Dictionary(Of String, String)
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            '組合員種別の取得
            Dim strSql As String = "SELECT l_name , c_constant_seq From constant_dtl "
            strSql = strSql + "WHERE c_constant = 'STAF_KIND' "
            'DB接続
            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)

            'シーケンス番号をキーに組合員名をセット
            For Each row As DataRow In dtRet.Rows
                dicStafKind.Add(row.Item("c_constant_seq").ToString, row.Item("l_name").ToString)
            Next

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.ConstructorInfo.GetCurrentMethod().Name)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

    End Sub
#End Region

#Region "関数：ChkPostTerm"
    '***************************************************************************************************
    '   ＩＤ　：ChkPostTerm
    '   名称　：役職の任期が対象年月内かチェックする
    '   概要  ：
    '   作成日：2011/12/05 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/05 onuma  新規作成
    '***************************************************************************************************
    Private Function ChkPostTerm() As Boolean
        Dim blnRet As Boolean = False
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty         '対象年月内の役職取得用
        Dim strSqlAllTarget As String = String.Empty   '対象委員会の全役職取得用
        Dim dtInTarget As DataTable = Nothing
        Dim dtAll As DataTable = Nothing

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        '対象年月のセット
        Dim intTargetYearMonth As Integer = CInt(Me.cboYear.Text & Me.cboMonth.Text)
        Try
            strSql = "SELECT l_name,d_service_from,d_service_to " &
                     " FROM committee_dtl WHERE c_committee_id ='" & Me.cboCommittee.SelectedValue() & "' " &
                     " AND CONVERT(int,'" & MDLoginInfo.PeriodFrom.ToString.Substring(0, 4) & "' + d_service_from) <= " & intTargetYearMonth &
                     " AND CONVERT(int,'" & MDLoginInfo.PeriodTo.ToString.Substring(0, 4) & "' + d_service_to) >= " & intTargetYearMonth &
                     " AND d_from <= '" & Me.searchStandardDay & "' " &
                     " AND d_to >= '" & Me.searchStandardDay & "' "
            'DB接続開始
            clsDb.Connect()

            dtInTarget = clsDb.ExecuteSql(strSql)
            If dtInTarget.Rows.Count = 0 Then
                '該当データなしの旨を通知し処理終了
                CLMsg.Show("BE0023")
                Return blnRet
            Else
                blnRet = True
                dicOutTerm = New Dictionary(Of String, DataRow)

                '該当データが存在する場合、対象外となるデータがあるかチェック
                '対象年月の範囲を指定せず役職取得
                strSqlAllTarget = "SELECT l_name,d_service_from,d_service_to " &
                                  "FROM committee_dtl " &
                                  "WHERE c_committee_id ='" & Me.cboCommittee.SelectedValue() & "' " &
                                  " AND  d_from <= '" & Me.searchStandardDay & "' " &
                                  " AND  d_to >= '" & Me.searchStandardDay & "' "

                dtAll = clsDb.ExecuteSql(strSqlAllTarget)

                If dtAll.Rows.Count > dtInTarget.Rows.Count Then
                    '対象外となるデータを取得
                    Dim dtPart As DataTable = Nothing
                    Dim aryOutTarget As ArrayList = New ArrayList
                    strSql = "SELECT s_committee_seq, l_name,d_service_from,d_service_to " &
                             " FROM committee_dtl WHERE c_committee_id ='" & Me.cboCommittee.SelectedValue() & "' " &
                             " AND ( CLng('" & MDLoginInfo.PeriodFrom.ToString.Substring(0, 4) & "' + d_service_from) > " & intTargetYearMonth &
                             " OR CLng('" & MDLoginInfo.PeriodTo.ToString.Substring(0, 4) & "' + d_service_to) < " & intTargetYearMonth & ") " &
                             " AND  d_from <= '" & Me.searchStandardDay & "' " &
                             " AND  d_to >= '" & Me.searchStandardDay & "' "
                    dtPart = clsDb.ExecuteSql(strSql)

                    For Each row As DataRow In dtPart.Rows
                        aryOutTarget.Add(CLMsg.GetMsg("BI0005", row.Item("l_name"),
                                                      MDLoginInfo.PeriodFrom.ToString.Substring(0, 4) & "/" & row.Item("d_service_from"),
                                                      MDLoginInfo.PeriodTo.ToString.Substring(0, 4) & "/" & row.Item("d_service_to")))
                        '対象外役職チェック用に取得
                        dicOutTerm.Add(row.Item("s_committee_seq"), row)
                    Next
                    Dim clsinfoMsg As New UCInfoMsg
                    UCInfoMsg.msgList = aryOutTarget
                    '対象外となる役職の表示
                    UCInfoMsg.ShowDialog()

                End If
            End If
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.ConstructorInfo.GetCurrentMethod().Name)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function
#End Region

#Region "関数：ChkExistActivity"
    '***************************************************************************************************
    '   ＩＤ　：ChkExistActivity
    '   名称　：活動が登録されている組合員であるかチェックする
    '   概要  ：
    '   作成日：2011/12/13 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/13 onuma  新規作成
    '***************************************************************************************************
    Private Function ChkExistActivity() As Boolean
        Dim blnRet As Boolean = True
        Dim strSql As String = String.Empty
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            'DB接続開始
            clsDb.Connect()
            '対象年月、選択委員会より出欠簿の登録有無を確認
            strSql = "SELECT c_user_id FROM call_roll_user_dtl " &
                         "WHERE  d_years LIKE '" & Me.cboYear.Text & "/" & Me.cboMonth.Text & "%' " &
                         "AND c_committee_id = '" & Me.cboCommittee.SelectedValue & "' "

            dtRet = clsDb.ExecuteSql(strSql)

            If dtRet.Rows.Count > 0 Then
                blnRet = False
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
            Return blnRet
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.ConstructorInfo.GetCurrentMethod().Name)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try
    End Function
#End Region

#Region "関数：ChkUnionMemberActivity"
    '***************************************************************************************************
    '   ＩＤ　：ChkUnionMemberActivity
    '   名称　：活動が登録されている組合員であるかチェックする
    '   概要  ：
    '   作成日：2011/12/06 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/06 onuma  新規作成
    '***************************************************************************************************
    Private Function ChkUnionMemberActivity(ByVal gridRows As Object) As Boolean
        Dim blnRet As Boolean = True
        Dim strSql As String = String.Empty
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing
        Dim aryNameList As ArrayList = New ArrayList()

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            'DB接続開始
            clsDb.Connect()

            For Each row As DataGridViewRow In gridRows
                '出欠簿の登録がないか一人ずつチェック
                strSql = "SELECT c_user_id FROM call_roll_user_dtl " &
                         "WHERE c_user_id = '" & row.Cells(STR_COLUMNNAME_STAFFID).Value & "' " &
                         "AND d_years LIKE '" & Me.cboYear.Text & "/" & Me.cboMonth.Text & "%' " &
                         "AND c_committee_id = '" & Me.cboCommittee.SelectedValue & "' "

                dtRet = clsDb.ExecuteSql(strSql)

                If dtRet.Rows.Count > 0 Then
                    '登録がある場合リストに追加
                    aryNameList.Add(row.Cells(STR_COLUMNNAME_NAME).Value)
                End If
            Next

            If aryNameList.Count > 0 Then
                blnRet = False
                If aryNameList.Count = 1 Then
                    '一人のみのときはメッセージボックス表示
                    CLMsg.Show("GE0033", aryNameList(0))
                Else
                    Dim iCnt As Integer = 0 'カウント用
                    Dim aryErrList As ArrayList = New ArrayList() 'エラーメッセージ画面へ引き渡し用
                    Dim clsUC999999 As New UC999999 '複数エラーメッセージ表示画面

                    For Each strName As String In aryNameList
                        aryErrList.Add(CLMsg.GetMsg("GE0033", aryNameList(iCnt)))
                        iCnt = iCnt + 1
                    Next
                    clsUC999999.errMsgList = aryErrList
                    'エラーメッセージリストの表示
                    clsUC999999.ShowDialog()

                End If
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.ConstructorInfo.GetCurrentMethod().Name)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Return blnRet
    End Function
#End Region

#Region "関数：ChkExistHistory"
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
            strSql = strSql & "WHERE c_period_id = '" & MDLoginInfo.PeriodId & "' " & vbCrLf '期ID
            strSql = strSql & "AND c_committee_id = '" & Me.cboCommittee.SelectedValue & "' " & vbCrLf  '委員会ID

            clsDb.Connect()

            dtRet = clsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                '履歴が存在する場合Trueを返却
                blnRet = True
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.ConstructorInfo.GetCurrentMethod().Name)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function
#End Region

#Region "関数：ChkCanDeleteHistory"
    '***************************************************************************************************
    '   ＩＤ　：ChkCanDeleteHistory
    '   名称　：現在表示中の委員会名簿データが削除可能かチェックする
    '   概要  ：
    '   作成日：2011/12/07 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/07 onuma  新規作成
    '***************************************************************************************************
    Private Function ChkCanDeleteHistory() As Boolean
        Dim blnRet As Boolean = False
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim dtRet As DataTable = New DataTable
        Dim strSelectDate As String = Me.cboYear.Text.Trim() & Me.cboMonth.Text.Trim() & "01"

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            strSql = "SELECT c_committee_list FROM committee_list WHERE c_period_id ='" & MDLoginInfo.PeriodId & "' " &
                     "AND c_committee_id ='" & Me.cboCommittee.SelectedValue & "' "
            'DB接続開始
            clsDb.Connect()

            'SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            If dtRet.Rows.Count > 1 Then
                '該当データが1件より多い場合、対象年月と適用開始年月が一致しているかチェック
                strSql = "SELECT c_committee_list FROM committee_list WHERE c_period_id ='" & MDLoginInfo.PeriodId & "' " &
                         "AND c_committee_id ='" & Me.cboCommittee.SelectedValue & "' " &
                         "AND d_from = '" & strSelectDate & "' "
                dtRet = clsDb.ExecuteSql(strSql)

                If dtRet.Rows.Count > 0 Then
                    '一致している場合削除可能とする
                    blnRet = True
                End If
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.ConstructorInfo.GetCurrentMethod().Name)
        Finally
            'DB接続終了
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function
#End Region

#Region "関数：ChkCanInsert"
    '***************************************************************************************************
    '   ＩＤ　：ChkCanInsert
    '   名称　：委員会構成一覧メンバーが登録可能なメンバー、役職かチェックする
    '   概要  ：
    '   作成日：2011/12/06 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/06 onuma  新規作成
    '       　：2013/04/25 Fujisaku　更新 GW0038チェック追加
    '***************************************************************************************************
    Private Function ChkCanInsert() As Boolean
        Dim blnRet As Boolean = False
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        For Each row As DataGridViewRow In Me.dgdCommitteeComposition.Rows
            '役職の任期が対象年月外となっているか確認する
            If row.Visible = True Then '表示となっている行のみチェック
                If dicOutTerm.Count > 0 Then
                    If dicOutTerm.ContainsKey(row.Cells(L_POST_NAME).Value) Then '設定された役職の任期をチェック
                        Dim showItemRow As DataRow = dicOutTerm(row.Cells(L_POST_NAME).Value)
                        '任期対象外の役職が設定されている組合員を表示
                        CLMsg.Show("BE0007", row.Cells(STR_COLUMNNAME_NAME).Value, showItemRow.Item("l_name"),
                                   MDLoginInfo.PeriodFrom.ToString.Substring(0, 4) & "/" & showItemRow.Item("d_service_from"),
                                   MDLoginInfo.PeriodTo.ToString.Substring(0, 4) & "/" & showItemRow.Item("d_service_to"))
                        Return blnRet
                    End If
                End If

                If (row.Cells(STR_COLUMNNAME_STATUS).Value <> MDConst.USER_STATUS_ENTRY) _
                    OrElse (row.Cells(STR_COLUMNNAME_KIND).Value <> MDConst.STAF_KIND_REGULAR _
                    AndAlso row.Cells(STR_COLUMNNAME_KIND).Value <> MDConst.STAF_KIND_SENIOR) Then '組合員種別、ステータスをチェック

                    Dim strKind As String = String.Empty
                    If dicStafKind.ContainsKey(row.Cells(STR_COLUMNNAME_KIND).Value) Then
                        '組合員種別名の取得
                        strKind = dicStafKind(row.Cells(STR_COLUMNNAME_KIND).Value)
                    End If

                    '正組合員・シニア組合員以外の場合、もしくはステータスが脱退・地位喪失の場合登録不可とする
                    'ただし、組合員以外(専従,管理部)の場合は警告メッセージを表示のうえ登録可能にする
                    If MDLoginInfo.CommitteeStatusFlg = 1 Then
                        CLMsg.Show("BE0012", row.Cells(STR_COLUMNNAME_STAFFID).Value, row.Cells(STR_COLUMNNAME_NAME).Value,
                                   strKind)
                        Return blnRet
                    Else
                        Dim msgRtn As DialogResult
                        Dim a As Object = MDLoginInfo.CommitteeStatusFlg
                        msgRtn = CLMsg.Show("GW0038", row.Cells(STR_COLUMNNAME_STAFFID).Value, row.Cells(STR_COLUMNNAME_NAME).Value,
                                   strKind)
                        If Not msgRtn = DialogResult.Yes Then
                            Return blnRet
                        End If
                    End If
                End If
            End If
        Next

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        '登録不可の組合員なし
        blnRet = True
        Return blnRet
    End Function
#End Region

#Region "関数：ShowPrintPreview"
    '***************************************************************************************************
    '   ＩＤ　：ShowPrintPreview
    '   名称　：委員会名簿プレビュー画面を表示します
    '   概要  ：
    '   作成日：2011/12/06 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/06 onuma  新規作成
    '***************************************************************************************************
    Private Function ShowPrintPreview(ByVal fm As FM000203) As Integer
        Dim intBtnRet As Integer = -1
        Dim dtCommitteeCompositionList As DataTable

        'Dim fmPrint As New FM000203     '印刷画面
        Dim fmPrint As FM000203     '印刷画面
        Dim dsReportInf As New DS0201P1 'レポート用データテーブル
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument  'レポート形式
        Dim lstCheck As New List(Of String)
        Dim flgCheck As Boolean
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            fmPrint = fm
            dtCommitteeCompositionList = GetCommitteeCompositionList()
            If dtCommitteeCompositionList.Rows.Count > 0 Then
                '出力項目デフォルトチェック
                If chkMemberNumber.Checked Then
                    flgCheck = True
                    lstCheck.Add(STR_COLUMNNAME_STAFFID)
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkName.Checked Then
                    flgCheck = True
                    lstCheck.Add(STR_COLUMNNAME_NAME)
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkPost.Checked Then
                    flgCheck = True
                    lstCheck.Add(L_POST_NAME)
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkTel.Checked Then
                    flgCheck = True
                    lstCheck.Add(C_TELL_1)
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkBranch.Checked Then
                    flgCheck = True
                    lstCheck.Add(STR_COLUMNNAME_BELONGING)
                    lstCheck.Add("")
                End If
                If chkModel.Checked Then
                    flgCheck = True
                    lstCheck.Add(S_MODEL_NAME)
                    lstCheck.Add("")
                End If
                If chkQualification.Checked Then
                    flgCheck = True
                    lstCheck.Add(STR_COLUMNNAME_QUOLIFICATION)
                    lstCheck.Add("")
                End If
                If chkMail.Checked Then
                    flgCheck = True
                    lstCheck.Add(S_MAIL_PC)
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If chkNote.Checked Then
                    flgCheck = True
                    lstCheck.Add(S_ATT_BIKO)
                    lstCheck.Add("")
                    lstCheck.Add("")
                    lstCheck.Add("")
                End If
                If flgCheck = False Then
                    MsgBox("出力項目をチェックしてください。", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation)
                    Return -1
                    'Exit Function
                End If

                '帳票出力
                reportObj = New CR0201P1(lstCheck)
                '組合員情報格納
                Dim drDetail As DS0201P1.list_titleRow
                For Each row As DataRow In dtCommitteeCompositionList.Rows
                    drDetail = dsReportInf.list_title.NewRow
                    drDetail.BeginEdit()
                    For iListCounter = 0 To lstCheck.Count - 1
                        If lstCheck(iListCounter) = L_POST_NAME Then
                            If dicPost.ContainsKey(row(L_POST_NAME)) = True Then
                                drDetail("t" + CStr(iListCounter)) = dicPost(row(lstCheck(iListCounter)))
                            End If
                        ElseIf lstCheck(iListCounter) <> "" Then
                            drDetail("t" + CStr(iListCounter)) = row(lstCheck(iListCounter))
                        Else
                            drDetail("t" + CStr(iListCounter)) = ""
                        End If
                    Next
                    drDetail.EndEdit()
                    dsReportInf.list_title.Rows.Add(drDetail)
                Next

                'ヘッダー格納
                Dim drHeader As DS0201P1.dtHeaderRow
                drHeader = dsReportInf.dtHeader.NewRow
                drHeader.BeginEdit()
                drHeader.c_committee_name = Me.cboCommittee.Text
                drHeader.k_belonging = ""
                drHeader.EndEdit()
                dsReportInf.dtHeader.Rows.Add(drHeader)

                Dim drHeader1 As DS0201P1.list_itemRow
                drHeader1 = dsReportInf.list_item.NewRow
                drHeader1.BeginEdit()
                For iListCounter = 0 To lstCheck.Count - 1
                    If lstCheck(iListCounter) = "メール" Then
                        drHeader1("i" + CStr(iListCounter)) = "E-Mail"
                    ElseIf lstCheck(iListCounter) = "組合支部" Then
                        drHeader1("i" + CStr(iListCounter)) = "支部"
                    ElseIf lstCheck(iListCounter) = "機種略称" Then
                        drHeader1("i" + CStr(iListCounter)) = "機種"
                    ElseIf lstCheck(iListCounter) = "電話番号1" Then
                        drHeader1("i" + CStr(iListCounter)) = "電話番号"
                    Else
                        drHeader1("i" + CStr(iListCounter)) = lstCheck(iListCounter)
                    End If
                Next
                drHeader1.EndEdit()
                dsReportInf.list_item.Rows.Add(drHeader1)

                fmPrint.ObjResource = reportObj
                reportObj.SetDataSource(dsReportInf)
                fmPrint.ButtonShowType = 1
                fmPrint.ShowDialog()

                'ボタン押下結果を受け取る
                intBtnRet = fmPrint.IntQlickBtnFlag
            Else
                'データなし
                CLMsg.Show("GE0202")
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "ShowPrintPreview")
        End Try

        Return intBtnRet
    End Function
#End Region

#Region "関数：getSearchData"
    '***************************************************************************************************
    '   ＩＤ　：getSearchData
    '   名称　：検索データ表示処理
    '   概要  ：
    '   作成日：2011/12/01 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/01 somezaki  新規作成
    '***************************************************************************************************
    Private Function getSearchData() As Boolean
        Dim blnRet As Boolean = False               ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim strYearSearch As String                 ' 年
        Dim strMonthSearch As String                ' 月
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

            'グリッドを初期化
            Call Me.dgdCommitteeComposition.Rows.Clear()

            ' 対象年月
            strYearSearch = Me.cboYear.Text
            strMonthSearch = Me.cboMonth.Text
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = ""
            strSql = strSql & "SELECT staf.c_user_id AS 社員番号,  " & vbCrLf
            strSql = strSql & "       staf.l_name AS 名前,  " & vbCrLf
            strSql = strSql & "       staf.k_user_status AS ステータス,  " & vbCrLf
            strSql = strSql & "       staf.k_staf_kind AS 組合員種別,  " & vbCrLf
            strSql = strSql & "       staf.組合支部 AS 組合支部, " & vbCrLf
            strSql = strSql & "       staf.役職 AS 役職,  " & vbCrLf
            strSql = strSql & "       staf.機種 AS 機種, " & vbCrLf
            strSql = strSql & "       staf.資格 AS 資格, " & vbCrLf
            strSql = strSql & "       staf.会社所属 AS 会社所属, " & vbCrLf
            strSql = strSql & "       '' AS 備考, " & vbCrLf
            strSql = strSql & "       address.l_tell_1 As 電話番号1,  " & vbCrLf
            strSql = strSql & "       address.l_mail_pc As メール,  " & vbCrLf
            strSql = strSql & "       staf.s_committee_seq As 役職ID,  " & vbCrLf
            strSql = strSql & "       staf.機種略称 As 機種略称,  " & vbCrLf
            strSql = strSql & "       0 As flag " & vbCrLf
            strSql = strSql & "FROM   " & vbCrLf
            strSql = strSql & "(SELECT t7.c_committee_list,t7.c_period_id,t7.c_committee_id,t7.l_biko " & vbCrLf
            strSql = strSql & "       ,t3.*,com_dtl.l_name AS 役職,com_list_dtl.s_committee_seq " & vbCrLf
            strSql = strSql & "       ,dtl1.l_name AS 組合支部,dtl2.l_name AS 機種,dtl3.l_omission_name AS 資格 " & vbCrLf
            strSql = strSql & "       ,dtl4.l_name AS 会社所属,dtl2.l_omission_name AS 機種略称 " & vbCrLf
            strSql = strSql & "    FROM committee_list AS t7,  " & vbCrLf
            strSql = strSql & "         committee_list_dtl AS com_list_dtl, " & vbCrLf
            strSql = strSql & "         committee_dtl AS com_dtl, " & vbCrLf
            strSql = strSql & "         (SELECT c_committee_id,c_period_id, MAX(d_from) AS now_from  " & vbCrLf
            strSql = strSql & "	        FROM committee_list  " & vbCrLf
            strSql = strSql & "          WHERE d_from <= '" & searchStandardDay & "'  " & vbCrLf '適用開始日内の最新の委員会名簿
            strSql = strSql & "           and c_period_id = '" & MDLoginInfo.PeriodId & "' " & vbCrLf    '期ID
            strSql = strSql & "          GROUP BY c_committee_id,c_period_id " & vbCrLf
            strSql = strSql & "         ) AS t8, " & vbCrLf
            strSql = strSql & "         staf_attribute AS t3,  " & vbCrLf
            strSql = strSql & "         constant_dtl dtl1, " & vbCrLf
            strSql = strSql & "         constant_dtl dtl2, " & vbCrLf
            strSql = strSql & "         constant_dtl dtl3, " & vbCrLf
            strSql = strSql & "         constant_dtl dtl4, " & vbCrLf
            strSql = strSql & "         (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from  " & vbCrLf
            strSql = strSql & "          FROM staf_attribute " & vbCrLf
            strSql = strSql & "          WHERE d_from <= '" & Me.searchStandardDay & "' " & vbCrLf '適用開始日内の最新の組合員情報
            strSql = strSql & "          GROUP BY c_user_id, c_ksh, c_staf_id " & vbCrLf
            strSql = strSql & "         ) AS t4  " & vbCrLf
            strSql = strSql & "    WHERE t7.c_committee_id = t8.c_committee_id  " & vbCrLf
            strSql = strSql & "    AND   t7.d_from = t8.now_from " & vbCrLf
            strSql = strSql & "    AND   t7.c_period_id = t8.c_period_id " & vbCrLf
            strSql = strSql & "    AND   t7.c_committee_list = com_list_dtl.c_committee_list " & vbCrLf
            strSql = strSql & "    AND   com_list_dtl.c_committee_id = '" & Me.cboCommittee.SelectedValue & "' " & vbCrLf '委員会ID
            strSql = strSql & "    AND   com_list_dtl.c_committee_id = com_dtl.c_committee_id  " & vbCrLf
            strSql = strSql & "    AND   com_list_dtl.s_committee_seq = com_dtl.s_committee_seq " & vbCrLf
            strSql = strSql & "    AND   com_dtl.d_from <= '" & Me.searchStandardDay & "' "
            strSql = strSql & "    AND   com_dtl.d_to >= '" & Me.searchStandardDay & "' "
            strSql = strSql & "    AND   t3.c_user_id = t4.c_user_id " & vbCrLf
            strSql = strSql & "    AND   t3.c_ksh     = t4.c_ksh " & vbCrLf
            strSql = strSql & "    AND   t3.d_from    = t4.now_from " & vbCrLf
            strSql = strSql & "    AND   t3.c_user_id = com_list_dtl.c_user_id " & vbCrLf
            strSql = strSql & "    AND dtl1.c_constant = 'BELONGING' AND dtl1.c_constant_seq = t3.k_belonging " & vbCrLf
            strSql = strSql & "    AND dtl2.c_constant = 'MODEL' AND dtl2.c_constant_seq = t3.k_model " & vbCrLf
            strSql = strSql & "    AND dtl3.c_constant = 'QUALIFICATION' AND dtl3.c_constant_seq = t3.k_qualification " & vbCrLf
            strSql = strSql & "    AND dtl4.c_constant = 'AREA_LOCAL' AND dtl4.c_constant_seq = t3.k_local " & vbCrLf
            strSql = strSql & ") AS staf " & vbCrLf
            strSql = strSql & "LEFT JOIN  " & vbCrLf
            strSql = strSql & "(SELECT t5.*  " & vbCrLf
            strSql = strSql & "            FROM staf_address AS t5,  " & vbCrLf
            strSql = strSql & "                 (SELECT c_user_id, MAX(d_from) AS now_from  " & vbCrLf
            strSql = strSql & "            FROM staf_address " & vbCrLf
            strSql = strSql & "                 WHERE d_from <= '" & searchStandardDay & "'  " & vbCrLf '適用開始日内の最新の住所情報
            strSql = strSql & "                  GROUP BY c_user_id) AS t6  " & vbCrLf
            strSql = strSql & "            WHERE t5.c_user_id = t6.c_user_id " & vbCrLf
            strSql = strSql & "                 AND t5.d_from = t6.now_from " & vbCrLf
            strSql = strSql & "                 AND t5.k_main_add = 'True'  " & vbCrLf
            strSql = strSql & ")  AS address ON staf.c_user_id = address.c_user_id " & vbCrLf
            strSql = strSql & "ORDER BY staf.s_committee_seq, CLng(staf.c_user_id) " & UtDb.DbOrderOffset '役職、社員番号でソート  'ok

            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' 件数チェック
            If intRetCnt = 0 Then
                ' 0件の処理
                CLMsg.Show("GI0003")                 ' 対象データなしメッセージボックス表示
            Else
                ' 検索結果を変更前テーブルとして保存、差分情報作成に使用
                Me.tbBefore = tbRet
            End If

            dtDeleteList = Nothing

            Me.grpCommittee.Text = "委員会構成一覧 (" + intRetCnt.ToString + "件)"
            Call SetCommitteeCompositionList(tbRet, True)
            Call SetDatagridDefault(Me.dgdCommitteeComposition, ARR_CULUMNSNAME_LIST_COMMITTEE, ARR_CULUMNSWIDTH_LIST_COMMITTEE, ARR_CULUMNSVISIBLE_LIST_COMMITTEE)

            Call Me.EditModeChange(False)

            If CreateCboConstantDtl(clsDb, Me.cboUnionBranch, CONSTANT_ID_BELONGING) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "getSearchData")
        Finally
            clsDb.Disconnect()
        End Try
    End Function
#End Region

#Region "関数：GetSearchUnionMember"
    '***************************************************************************************************
    '   ＩＤ　：GetSearchUnionMember
    '   名称　：組合員一覧（カナ検索）
    '   概要  ：
    '   作成日：2011/12/22 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/22 onuma  新規作成
    '***************************************************************************************************
    Private Sub GetSearchUnionMember()
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            clsDb.Connect()
            Dim tbResultSql As New DataTable    'SQL結果取得用

            Dim strSql As String = "" 'SQL文格納用

            If ChkNull(Me.txtKana.Text) = True Then
                CLMsg.Show("GE0011")
                'テキストボックスをピンク
                SetErr(Me.txtKana)
                Exit Sub
            End If
            'SQL文
            strSql = STR_SELECT_BASE & vbCrLf
            strSql = strSql & STR_SELECT_ATTRIBUTE_MAIN + " WHERE d_from <= '" + searchStandardDay + "' " + STR_SELECT_ATTRIBUTE_SUB & vbCrLf
            strSql = strSql & " AND x.l_name_kna LIKE '%" & Me.txtKana.Text & "%'" & STR_SELECT_ATTRIBUTE_SUB2 & vbCrLf
            strSql = strSql & STR_LEFT_JOIN & vbCrLf
            strSql = strSql & STR_SELECT_ADDRESS_MAIN + " WHERE d_from <= '" + searchStandardDay + "' " + STR_SELECT_ADDRESS_SUB & vbCrLf
            strSql = strSql & STR_JOIN_ON & vbCrLf
            strSql = strSql & " ORDER BY " & STR_COLUMNNAME_NAME_KNA & " ASC" & UtDb.DbOrderOffset

            Dim bsSource As New BindingSource
            'todo:
            tbResultSql = clsDb.ExecuteSql(strSql)
            bsSource.DataSource = tbResultSql
            dgdKanaSearchResult.DataSource = bsSource
            Call SetDatagridDefault(dgdKanaSearchResult, ARR_CULUMNSNAME_LIST_UNION, ARR_CULUMNSWIDTH_LIST_UNION, ARR_CULUMNSVISIBLE_LIST_UNION)

            Me.dgdKanaSearchResult.Visible = True
            Me.txtKana.BackColor = Color.White
            '検索タブを選択
            Me.tclJpnSyllabary.SelectedIndex = Me.tclJpnSyllabary.TabCount - 1

            If tbResultSql.Rows.Count = 0 Then
                CLMsg.Show("GE0007")
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "GetSearchUnionMember")
        Finally
            clsDb.Disconnect()
        End Try

    End Sub
#End Region

#Region "関数：GetSeachStafID"
    '***************************************************************************************************
    '   ＩＤ　：GetSeachStafID
    '   名称　：組合員一覧（社員番号検索）
    '   概要  ：
    '   作成日：2011/12/22 onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/22 onuma  新規作成
    '***************************************************************************************************
    Private Sub GetSeachStafID()
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            Me.Cursor = Cursors.WaitCursor
            clsDb.Connect()
            Dim tbResultSql As New DataTable    'SQL結果取得用

            Dim strSql As String = "" 'SQL文格納用

            If ChkNull(Me.txtStafID.Text) = True Then
                CLMsg.Show("GE0011")
                SetErr(Me.txtStafID)
                Exit Sub
            ElseIf ChkNumber(Me.txtStafID.Text) = False Then
                CLMsg.Show("GE0019", "社員番号")
                Me.txtStafID.BackColor = Color.Orange
                Exit Sub
            End If

            'SQL文
            strSql = STR_SELECT_BASE & vbCrLf
            strSql = strSql & STR_SELECT_ATTRIBUTE_MAIN + " WHERE d_from <= '" + searchStandardDay + "' " + STR_SELECT_ATTRIBUTE_SUB & vbCrLf
            strSql = strSql & " AND x.c_user_id LIKE '" & Me.txtStafID.Text.Trim & "%'" & STR_SELECT_ATTRIBUTE_SUB2 & vbCrLf
            strSql = strSql & STR_LEFT_JOIN & vbCrLf
            strSql = strSql & STR_SELECT_ADDRESS_MAIN + " WHERE d_from <= '" + searchStandardDay + "' " + STR_SELECT_ADDRESS_SUB & vbCrLf
            strSql = strSql & STR_JOIN_ON & vbCrLf
            strSql = strSql & " ORDER BY CLng(attrbute_new.社員番号) ASC" & UtDb.DbOrderOffset

            Dim bsSource As New BindingSource
            'todo:
            tbResultSql = clsDb.ExecuteSql(strSql)
            bsSource.DataSource = tbResultSql
            dgdStafIDResult.DataSource = bsSource
            Call SetDatagridDefault(dgdStafIDResult, ARR_CULUMNSNAME_LIST_UNION, ARR_CULUMNSWIDTH_LIST_UNION, ARR_CULUMNSVISIBLE_LIST_UNION)

            Me.dgdStafIDResult.Visible = True
            Me.txtStafID.BackColor = Color.White

            If tbResultSql.Rows.Count = 0 Then
                CLMsg.Show("GE0007")
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnSearchUnionMember_Click")
        Finally
            clsDb.Disconnect()
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region

#Region "関数：SetCommitteeCompositionList"
    '***************************************************************************************************
    '   ＩＤ　：SetCommitteeCompositionList
    '   名称　：委員会構成一覧追加処理
    '   概要  ：データテーブルの情報（組合員情報）を委員会構成一覧のグリッドに追加する
    '   作成日：2011/12/01 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/01 somezaki  新規作成
    '***************************************************************************************************
    Private Function SetCommitteeCompositionList(ByVal showData As DataTable, Optional ByVal bleClearData As Boolean = False) As Boolean
        Dim blnRet As Boolean = False
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            Dim row As DataRow                  'データ行
            Dim nowRowCnt = 0
            Dim intInsertRow As Integer = 0

            If showData.Rows.Count > 0 Then
                '既存データクリア指定で、データがあれば削除する
                If bleClearData And Me.dgdCommitteeComposition.RowCount > 0 Then
                    Me.dgdCommitteeComposition.Rows.Clear()
                    Me.dgdCommitteeComposition.Columns.Clear()

                End If
                '列がなければ追加
                If Me.dgdCommitteeComposition.Columns.Count = 0 Then
                    '列作成
                    Dim intCnt As Integer = 0
                    For Each colname As String In ARR_CULUMNSNAME_LIST_COMMITTEE
                        If colname = L_POST_NAME Then
                            Dim comboBoxColumn As New DataGridViewComboBoxColumn
                            MakePostnameCombobox(comboBoxColumn, Me.cboCommittee.SelectedValue)
                            Me.dgdCommitteeComposition.Columns.Insert(dgdCommitteeComposition.Columns.Count, comboBoxColumn)
                            comboBoxColumn.Name = L_POST_NAME
                            comboBoxColumn.HeaderText = L_POST_NAME
                        Else
                            Me.dgdCommitteeComposition.Columns.Add(colname, colname)
                            Me.dgdCommitteeComposition.Columns.Item(colname).Visible = ARR_CULUMNSVISIBLE_LIST_COMMITTEE(intCnt)
                            Me.dgdCommitteeComposition.Columns.Item(colname).Width = ARR_CULUMNSWIDTH_LIST_COMMITTEE(intCnt)
                            Me.dgdCommitteeComposition.Columns.Item(colname).SortMode = DataGridViewColumnSortMode.NotSortable
                        End If

                        If (ARR_CULUMNSNAME_LIST_COMMITTEE(intCnt) = L_POST_NAME) OrElse
                           (ARR_CULUMNSNAME_LIST_COMMITTEE(intCnt) = STR_COLUMNNAME_NAME) OrElse
                           (ARR_CULUMNSNAME_LIST_COMMITTEE(intCnt) = STR_COLUMNNAME_KIND) Then
                            Me.dgdCommitteeComposition.Columns.Item(colname).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
                        ElseIf (ARR_CULUMNSNAME_LIST_COMMITTEE(intCnt) = STR_COLUMNNAME_STAFFID) Then
                            Me.dgdCommitteeComposition.Columns.Item(colname).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        Else
                            Me.dgdCommitteeComposition.Columns.Item(colname).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                        End If
                        intCnt = intCnt + 1
                    Next
                End If

                '追加するデータ
                nowRowCnt = Me.dgdCommitteeComposition.Rows.Count

                For i = 0 To showData.Rows.Count - 1
                    '投入データ
                    Dim blnIsExist = False
                    '追加する
                    If Not blnIsExist Then
                        '行作成
                        Me.dgdCommitteeComposition.Rows.Add()
                        '追加行データ取得
                        row = showData.Rows(i)
                        'データ投入
                        intInsertRow = i + nowRowCnt
                        For intColCnt = 0 To Me.dgdCommitteeComposition.ColumnCount - 1
                            '役職は初期値を指定
                            If ARR_CULUMNSNAME_LIST_COMMITTEE(intColCnt) = L_POST_NAME Then
                                Me.dgdCommitteeComposition.Rows(intInsertRow).Cells.Item(ARR_CULUMNSNAME_LIST_COMMITTEE(intColCnt)).Value =
                                NVL(showData.Rows(i).Item(S_COMMITTEE_SEQ))
                            Else
                                Me.dgdCommitteeComposition.Rows(intInsertRow).Cells.Item(ARR_CULUMNSNAME_LIST_COMMITTEE(intColCnt)).Value =
                                NVL(showData.Rows(i).Item(ARR_CULUMNSNAME_LIST_COMMITTEE(intColCnt)))
                            End If
                        Next

                        If showData.Rows(i).Item(STR_COLUMNNAME_STATUS) = MDConst.USER_STATUS_POSITION_LOSS Then
                            'ステータスが地位喪失となっている組合員は文字色変更
                            Me.dgdCommitteeComposition.Rows(intInsertRow).DefaultCellStyle.ForeColor = Color.DeepPink
                            Me.dgdCommitteeComposition.Rows(intInsertRow).DefaultCellStyle.SelectionForeColor = Color.DeepPink
                        End If
                    End If
                Next

            End If
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, "GetDgdUnionSelectListDataTable")
        End Try
        Return blnRet

    End Function
#End Region

#Region "関数：GetCommitteeCompositionList"
    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeCompositionList
    '   名称　：組合員抽出リスト（画面左グリッド）に表示されている情報をデータテーブルで返す
    '   概要  ：組合員抽出リスト（画面左グリッド）に表示されている組合員の情報をデータテーブルに格納し返す（フレックスグリッド不使用対応）
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/26 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/26 somezaki  新規作成
    '***************************************************************************************************
    Private Function GetCommitteeCompositionList() As DataTable

        Dim dtReturn As New DataTable
        '実際の追加行カウント用
        Dim intAddCnt As Integer = 0
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            If Me.dgdCommitteeComposition.Rows.Count > 0 Then

                '列追加
                For i = 0 To Me.dgdCommitteeComposition.ColumnCount - 1
                    dtReturn.Columns.Add(dgdCommitteeComposition.Columns.Item(i).Name)
                Next

                '値投入
                For rowAddCnt = 0 To dgdCommitteeComposition.Rows.Count - 1
                    If Me.dgdCommitteeComposition.Rows.Item(rowAddCnt).Visible = True Then '現在表示中の列のみプレビュー画面へ
                        Call dtReturn.Rows.Add()    '行追加
                        For colAddCnt = 0 To dgdCommitteeComposition.ColumnCount - 1
                            Dim strInsertData As String = ""
                            If Not (IsDBNull(dgdCommitteeComposition.Rows.Item(rowAddCnt).Cells(ARR_CULUMNSNAME_LIST_COMMITTEE(colAddCnt)))) Then
                                strInsertData = dgdCommitteeComposition.Rows.Item(rowAddCnt).Cells(ARR_CULUMNSNAME_LIST_COMMITTEE(colAddCnt)).Value.ToString
                            End If
                            dtReturn.Rows.Item(intAddCnt).Item(ARR_CULUMNSNAME_LIST_COMMITTEE(colAddCnt)) = strInsertData
                        Next
                        intAddCnt = intAddCnt + 1
                    End If
                Next
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, "GetDgdUnionSelectListDataTable")
        End Try
        Return dtReturn

    End Function
#End Region

#Region "関数：MakePostnameCombobox"
    Private Function MakePostnameCombobox(ByVal dgdCmbBox As DataGridViewComboBoxColumn, ByVal committeeId As String) As Boolean
        Dim retVal As Boolean = False
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim strSql As String                 ' SQL文
        Dim tbRet As DataTable              ' SQL結果
        Dim intRetCnt As Integer            ' SQL件数
        Try
            ' 初期処理
            strSql = "SELECT l_name AS DisplayName , s_committee_seq AS ValueMember " & vbCrLf
            strSql = strSql & "FROM committee_dtl " & vbCrLf
            strSql = strSql & "WHERE c_committee_id = '" + committeeId + "' " & vbCrLf
            strSql = strSql & " AND  d_from <= '" & Me.searchStandardDay & "' " & vbCrLf
            strSql = strSql & " AND  d_to >= '" & Me.searchStandardDay & "'"

            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count
            If intRetCnt = 0 Then
                Return retVal
            End If

            'コンボボックスの表示設定
            dgdCmbBox.DataSource = tbRet
            dgdCmbBox.DisplayMember = "DisplayName"
            dgdCmbBox.ValueMember = "ValueMember"

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "MakePostnameCombobox")
        Finally
            clsDb.Disconnect()
        End Try

        Return retVal

    End Function
#End Region

#Region "関数：ShowDgdUnionMemberList"
    Private Function ShowDgdUnionMemberList() As Boolean
        Dim retVal As Boolean = False
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        'Dim tbRet As DataTable              ' SQL結果
        'Dim intRetCnt As Integer            ' SQL件数

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            '画面初期（あ行～データグリッド）*後でループで処理するため配列
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdALine, 0)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdKALine, 1)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdSALine, 2)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdTALine, 3)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdNALine, 4)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdHALine, 5)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdMALine, 6)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdYALine, 7)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdRALine, 8)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdWALine, 9)
            'ARR_DATAGRIDVIEW.SetValue(Me.dgdOther, 10)

            'DB接続
            clsDb.Connect()

            '▼*****SQL実行（あ～その他タブ結果表示）*****
            Call SetJpnSyllabaryEachPage(clsDb)

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "MakePostnameCombobox")
        Finally
            clsDb.Disconnect()
        End Try

        Return retVal

    End Function
#End Region

#Region "関数：SetJpnSyllabaryEachPage"
    '***************************************************************************************************
    '   ＩＤ　：SetJpnSyllabaryEachPage
    '   名称　：50音順タブページの初期表示
    '   概要  ：50音順タブページのデータグリッドビューを初期表示する
    '   引数　：CLAccessMdb
    '   戻り値：なし
    '   作成日：2011/11/15 somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/15 somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員一覧件数表示</summary>
    Private Sub SetJpnSyllabaryEachPage(ByVal db_inf As CLAccessMdb)
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            Dim tbResultSql As New DataTable    'SQL結果取得用

            Dim strSql As String = "" 'SQL文格納用
            Dim intCnt As Integer = 0 '配列用ループカウンタ
            intCnt = 0
            For Each setDataGrid As DataGridView In ARR_DATAGRIDVIEW
                strSql = STR_SELECT_BASE & vbCrLf
                strSql = strSql & STR_SELECT_ATTRIBUTE_MAIN + " WHERE d_from <= '" + searchStandardDay + "' " + STR_SELECT_ATTRIBUTE_SUB & vbCrLf
                strSql = strSql & ARR_STR_SQLWHERE(intCnt) & STR_SELECT_ATTRIBUTE_SUB2 & vbCrLf
                strSql = strSql & STR_LEFT_JOIN & vbCrLf
                strSql = strSql & STR_SELECT_ADDRESS_MAIN + " WHERE d_from <= '" + searchStandardDay + "' " + STR_SELECT_ADDRESS_SUB & vbCrLf
                strSql = strSql & STR_JOIN_ON & vbCrLf
                strSql = strSql & " ORDER BY " & STR_COLUMNNAME_NAME_KNA & " ASC" & UtDb.DbOrderOffset

                Dim bsSource As New BindingSource
                'todo:
                tbResultSql = db_inf.ExecuteSql(strSql)
                bsSource.DataSource = tbResultSql
                setDataGrid.DataSource = bsSource
                intCnt = intCnt + 1
                Call SetDatagridDefault(setDataGrid, ARR_CULUMNSNAME_LIST_UNION, ARR_CULUMNSWIDTH_LIST_UNION, ARR_CULUMNSVISIBLE_LIST_UNION)
            Next
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "SetJpnSyllabaryEachPage")
        End Try
    End Sub
#End Region

#Region "関数：SetDatagridDefault"
    '***************************************************************************************************
    '   ＩＤ　：SetDatagridDefault
    '   名称　：データグリッドビューヘッダー設定処理
    '   概要　：データグリッドビューの設定を行う
    '   引数　：ByVal setDatagridview        As DataGridView = ,
    '           ByVal arrSetColumnsNameInf   As String()     = ,
    '           ByVal arrSetColumnWidthInf   As Integer()    = ,
    '           ByVal arrSetColumnVisibleInf As Boolean()    = 
    '   戻り値：なし
    '   作成日：2011/12/01 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/01 m.somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>データグリッドビューのヘッダー設定</summary>
    ''' <param name="setDatagridview"></param>
    ''' <param name="arrSetColumnsNameInf"></param>
    ''' <param name="arrSetColumnWidthInf"></param>
    ''' <param name="arrSetColumnVisibleInf"></param>
    ''' <remarks></remarks>
    Private Sub SetDatagridDefault(ByVal setDatagridview As DataGridView, ByVal arrSetColumnsNameInf As String(),
                                   ByVal arrSetColumnWidthInf As Integer(), ByVal arrSetColumnVisibleInf As Boolean())
        Try
            setDatagridview.Visible = True
            setDatagridview.ColumnHeadersDefaultCellStyle.Alignment = DGD_HEADER_CENTER 'ヘッダーセンター
            setDatagridview.DefaultCellStyle.ForeColor = DGD_FORECOLOR_BLACK            '文字黒
            setDatagridview.RowHeadersVisible = DGD_ROWHEADER_NONVISIBLE                'ヘッダー非表示
            setDatagridview.AllowUserToAddRows = DGD_ADDROW_NON                         '追加不可
            setDatagridview.AllowUserToDeleteRows = DGD_DELETEROW_NON                   '削除不可
            setDatagridview.SelectionMode = DGD_SELECTIONMODE_FULLRAW                   '行選択
            'setDatagridview.ReadOnly = True                                             '編集不可
            setDatagridview.ReadOnly = True                                             '編集不可
            setDatagridview.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            setDatagridview.ColumnHeadersHeight = 23
            setDatagridview.ScrollBars = ScrollBars.Both

            For intColCnt As Integer = 0 To setDatagridview.Columns.Count - 1
                setDatagridview.Columns(intColCnt).Name = arrSetColumnsNameInf(intColCnt)
                setDatagridview.Columns(intColCnt).HeaderText = arrSetColumnsNameInf(intColCnt)
                setDatagridview.Columns(intColCnt).Width = arrSetColumnWidthInf(intColCnt)
                setDatagridview.Columns(intColCnt).Visible = arrSetColumnVisibleInf(intColCnt)
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "SetDatagridDefault")
        End Try
    End Sub
#End Region

#Region "関数：btnDeleteListVisibleControl"
    '***************************************************************************************************
    '   ＩＤ　：btnDeleteListVisibleControl
    '   名称　：削除者リストボタンの表示非表示を行う
    '   概要  ：削除者リストボタンの表示非表示を行う
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/02 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnDeleteListVisibleControl()
        Try
            If dtDeleteList IsNot Nothing Then
                If dtDeleteList.Rows.Count > 0 Then
                    If Not btnDelList.Visible Then
                        btnDelList.Visible = True
                    End If
                Else
                    If btnDelList.Visible Then
                        btnDelList.Visible = False
                    End If
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnDeleteListVisibleControl")
        End Try
    End Sub
#End Region

#Region "関数：SetRowCnt"
    '***************************************************************************************************
    '   ＩＤ　：SetRowCnt
    '   名称　：組合員一覧件数表示
    '   概要  ：組合員一覧件数表示
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/24 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24 m.somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員一覧件数表示</summary>
    Private Sub SetRowCnt()
        Try
            Me.grpCommittee.Text = "組合員一覧(" + (dgdCommitteeComposition.Rows.Count).ToString + "件)"
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "SetRowCnt")
        End Try
    End Sub
#End Region

#Region "関数：disappearDeleteList"
    '***************************************************************************************************
    '   ＩＤ　：disappearDeleteList
    '   名称　：削除者リストから対象者を消去する
    '   概要  ：削除者リストから対象の社員番号行を削除する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/02 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/02 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub disappearDeleteList(ByVal staffId As String)
        Try
            If dtDeleteList.Rows.Count > 0 Then

                Dim intCnt = dtDeleteList.Rows.Count - 1
                Do While (intCnt > -1)
                    If dtDeleteList.Rows(intCnt).Item(STR_COLUMNNAME_STAFFID).ToString().Trim = staffId.Trim Then
                        dtDeleteList.Rows.Remove(dtDeleteList.Rows(intCnt)) '削除
                    End If
                    intCnt = intCnt - 1
                Loop

            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "disappearDeleteList")
        End Try
    End Sub
#End Region

#Region "関数：ClearGrpUnionMember"
    '組合員抽出機能グループを初期化する
    '***************************************************************************************************
    '   ＩＤ　：ClearGrpUnionMember
    '   名称　：組合員抽出機能グループ初期化
    '   概要  ：組合員抽出機能グループを初期化する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ClearGrpUnionMember()
        Try
            Me.dgdALine.DataSource = Nothing
            Me.dgdKALine.DataSource = Nothing
            Me.dgdSALine.DataSource = Nothing
            Me.dgdTALine.DataSource = Nothing
            Me.dgdNALine.DataSource = Nothing
            Me.dgdHALine.DataSource = Nothing
            Me.dgdMALine.DataSource = Nothing
            Me.dgdYALine.DataSource = Nothing
            Me.dgdRALine.DataSource = Nothing
            Me.dgdWALine.DataSource = Nothing
            Me.dgdOther.DataSource = Nothing
            Me.dgdKanaSearchResult.DataSource = Nothing
            Me.dgdStafIDResult.DataSource = Nothing
            Me.dgdKanaSearchResult.Visible = False
            Me.dgdStafIDResult.Visible = False

            Me.txtKana.Text = String.Empty
            Me.txtStafID.Text = String.Empty

            Me.tclUnionMemberExtraction.SelectedIndex = 0
            Me.tclJpnSyllabary.SelectedIndex = 0

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "ClearGrpUnionMember")
        End Try
    End Sub
#End Region

#Region "関数：EditModeChange"
    Private Sub EditModeChange(ByVal PblnEditMode As Boolean, Optional ByVal blnShowCopyLastPeriod As Boolean = False)
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            '組合員抽出機能グループを初期化
            'Call Me.ClearGrpUnionMember()

            Me.grpOutput.Visible = True
            Me.grpCommittee.Visible = True
            Me.grpUnionMember.Visible = True
            Me.btnLeft.Visible = True
            Me.btnRight.Visible = True

            If PblnEditMode = True Then
                '表示
                Me.btnCancel.Visible = True 'キャンセル
                Me.btnInsert.Visible = True '登録確認
                Me.btnDelete.Visible = True '削除

                '非表示
                Me.btnUpdate.Visible = False      '内容変更
                Me.lblUnionBranch.Visible = False '組合支部ラベル
                Me.cboUnionBranch.Visible = False '組合支部コンボボックス
                Me.btnPrePrint.Visible = False    'プレ印刷

                ''検索結果が０件のときは前期からコピーを表示
                'If Me.dgdCommitteeComposition.Rows.Count = 0 Then
                Me.btnCopyLastPeriod.Visible = blnShowCopyLastPeriod
                'Else
                '    Me.btnCopyLastPeriod.Visible = False
                'End If

                '履歴表示中は削除ボタンを有効化
                If ChkCanDeleteHistory() = True Then
                    Me.btnDelete.Enabled = True
                Else
                    Me.btnDelete.Enabled = False
                End If

                'ロック
                Me.grpSearch.Enabled = False     '検索
                Me.grpUnionMember.Enabled = True '組合員抽出機能グループ
                Me.btnRight.Enabled = True       '＞
                Me.btnLeft.Enabled = True        '＜

                'グリッドの役職列だけ編集可能
                Me.dgdCommitteeComposition.ReadOnly = False
                For Each col As DataGridViewColumn In Me.dgdCommitteeComposition.Columns
                    If col.Name = L_POST_NAME Then
                        col.ReadOnly = False
                    Else
                        col.ReadOnly = True
                    End If
                Next

                Me.dtpTargetDate.MinDate = CDate(Me.cboYear.Text & "/" & Me.cboMonth.Text & "/01")
                Me.dtpTargetDate.MaxDate = CDate(Me.cboYear.Text & "/" & Me.cboMonth.Text & "/01").AddMonths(1).AddDays(-1)
                '基準日の最大日付、最小日付を対象年月内にする
                Me.dtpTargetDate.Value = CDate(Me.cboYear.Text & "/" & Me.cboMonth.Text & "/01")

            Else
                '非表示
                Me.btnCancel.Visible = False         'キャンセル
                Me.btnInsert.Visible = False         '登録確認
                Me.btnDelete.Visible = False         '削除
                Me.btnDelList.Visible = False        '削除者リスト
                Me.btnCopyLastPeriod.Visible = False '前期からコピー

                '表示
                Me.btnUpdate.Visible = True      '内容変更
                Me.lblUnionBranch.Visible = True '組合支部ラベル
                Me.cboUnionBranch.Visible = True '組合支部コンボボックス

                '検索結果が０件でないときはプレ印刷を表示
                If Me.dgdCommitteeComposition.Rows.Count > 0 Then
                    Me.btnPrePrint.Visible = True
                Else
                    Me.btnPrePrint.Visible = False
                End If

                Me.grpSearch.Enabled = True       '検索
                Me.grpUnionMember.Enabled = False '組合員抽出機能グループ
                Me.btnRight.Enabled = False       '＞
                Me.btnLeft.Enabled = False        '＜

                'グリッドを編集不可
                Me.dgdCommitteeComposition.ReadOnly = True
                Me.dtpTargetDate.MinDate = "1753/01/01"
                Me.dtpTargetDate.MaxDate = "9998/12/31"
                If ChkNull(Me.cboYear.Text) = False AndAlso ChkNull(Me.cboMonth.Text) = False Then
                    '基準日を1日に設定
                    Me.dtpTargetDate.Value = CDate(Me.cboYear.Text & "/" & Me.cboMonth.Text & "/01")
                End If

            End If
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "EditModeChange")
        End Try
    End Sub
#End Region

#Region "関数：iniDgdCommitteeListShow"
    '***************************************************************************************************
    '   ＩＤ　：iniDgdCommitteeListShow
    '   名称　：委員会構成一覧を初期表示する
    '   概要  ：委員会構成一覧を初期表示する（組合支部で非表示化されたデータを再表示する）
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/05 m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/05 m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub iniDgdCommitteeListShow()
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            For i = 0 To Me.dgdCommitteeComposition.Rows.Count - 1
                If Me.dgdCommitteeComposition.Rows(i).Cells(STR_COLUMNNAME_BELONGING).Value <> Me.cboUnionBranch.Text Then
                    Me.dgdCommitteeComposition.Rows(i).Visible = True
                End If
            Next
            '件数を再設定
            Me.grpCommittee.Text = "委員会構成一覧(" + (dgdCommitteeComposition.Rows.Count).ToString + "件)"
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "iniDgdCommitteeListShow")
        End Try
    End Sub
#End Region

#Region "関数：CountVisibleMember"
    '***************************************************************************************************
    '   ＩＤ　：CountVisibleMember
    '   名称　：
    '   概要  ：委員会構成一覧の表示データ件数を返却する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub CountVisibleMember()
        Dim intCnt As Integer = 0

        If Me.dgdCommitteeComposition.Visible = True Then
            For Each dgdRow As DataGridViewRow In Me.dgdCommitteeComposition.Rows
                If dgdRow.Visible = True Then
                    '表示行のみカウント
                    intCnt = intCnt + 1
                End If
            Next
        End If

        '委員会構成一覧のグリッドが非表示になっている場合は0件とする
        Me.grpCommittee.Text = "委員会構成一覧 (" + intCnt.ToString + "件)"

    End Sub
#End Region

#Region "関数：ClearCommitteeMemberGrid"
    '***************************************************************************************************
    '   ＩＤ　：ClearCommitteeMemberGrid
    '   名称　：委員会構成一覧のデータクリア
    '   概要  ：委員会構成一覧のデータをクリアする
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ClearCommitteeMemberGrid()
        If Me.dgdCommitteeComposition.Visible = True Then
            Me.dgdCommitteeComposition.Rows.Clear()
            Me.dgdCommitteeComposition.Columns.Clear()
            Me.grpCommittee.Text = "委員会構成一覧 (" + dgdCommitteeComposition.RowCount.ToString + "件)"

            '委員会構成一覧、組合員抽出機能、変更ボタン、左ボタン、右ボタンを非表示
            Me.dgdCommitteeComposition.Visible = False
            Me.grpOutput.Visible = False
            Me.grpCommittee.Visible = False
            Me.grpUnionMember.Visible = False
            Me.btnUpdate.Visible = False
            Me.btnLeft.Visible = False
            Me.btnRight.Visible = False

        End If
    End Sub
#End Region

#Region "関数：SetMonth"
    '***************************************************************************************************
    '   ＩＤ　：SetMonth
    '   名称　：月コンボボックス設定処理
    '   概要  ：月コンボボックスの値を設定する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/21 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetMonth()
        ' 初期処理
        Me.cboMonth.BeginUpdate()                                           ' チラつき防止の為、最後まで描写しない
        Me.cboMonth.Items.Clear()                                           ' 月コンボボックスクリア
        Me.cboMonth.Items.Add("01")
        Me.cboMonth.Items.Add("02")
        Me.cboMonth.Items.Add("03")
        Me.cboMonth.Items.Add("04")
        Me.cboMonth.Items.Add("05")
        Me.cboMonth.Items.Add("06")
        Me.cboMonth.Items.Add("07")
        Me.cboMonth.Items.Add("08")
        Me.cboMonth.Items.Add("09")
        Me.cboMonth.Items.Add("10")
        Me.cboMonth.Items.Add("11")
        Me.cboMonth.Items.Add("12")
        Me.cboMonth.EndUpdate()                                             ' チラつき防止の為、最後に描写する

    End Sub
#End Region

#Region "関数：AddUnionMember"
    '***************************************************************************************************
    '   ＩＤ　：AddUnionMember
    '   名称　：選択した組合員の追加
    '   概要  ：委員会構成一覧のに選択した組合員を追加する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/09 a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/09 a.onuma  新規作成
    '***************************************************************************************************
    Private Sub AddUnionMember()
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            Dim addData As DataTable = Nothing
            'エラーメッセージ用
            Dim aryNameList As ArrayList = New ArrayList()
            Dim errorCnt As Integer = 0

            If showDataGridView.SelectedRows.Count > 0 Then

                '選択行の数だけループ
                For Each selectRaw As DataGridViewRow In showDataGridView.SelectedRows

                    'ヘッダーの下行からすでに存在するデータではないかチェック
                    Dim blnIsExist = False
                    For intRow As Integer = 0 To dgdCommitteeComposition.Rows.Count - 1
                        'すでに選択されている組合員リストの社員番号のセルと比較
                        Dim strCheckVal As String = dgdCommitteeComposition.Rows.Item(intRow).Cells(STR_COLUMNNAME_STAFFID).Value.ToString.Trim()
                        Dim addStaff As String = selectRaw.Cells.Item(STR_COLUMNNAME_STAFFID).Value.ToString.Trim()
                        If strCheckVal = addStaff Then
                            If dgdCommitteeComposition.Rows.Item(intRow).Visible = False Then
                                dgdCommitteeComposition.Rows.Item(intRow).Visible = True
                                'データベース登録済ユーザの場合は削除リストから削除
                                Call disappearDeleteList(selectRaw.Cells.Item(STR_COLUMNNAME_STAFFID).Value.ToString.Trim())
                            Else
                                '重複メッセージ
                                aryNameList.Add(selectRaw.Cells.Item(STR_COLUMNNAME_NAME).Value.ToString.Trim())
                                errorCnt = errorCnt + 1
                            End If
                            blnIsExist = True
                            Exit For
                        End If
                    Next

                    '最下行にデータ挿入
                    If Not blnIsExist Then
                        '列追加

                        addData = New DataTable
                        'For i = 0 To dgdCommitteeComposition.ColumnCount - 1
                        '    addData.Columns.Add(dgdCommitteeComposition.Columns.Item(i).Name)
                        'Next
                        For i = 0 To UBound(ARR_CULUMNSNAME_LIST_COMMITTEE)
                            Call addData.Columns.Add(ARR_CULUMNSNAME_LIST_COMMITTEE(i))
                        Next

                        addData.Rows.Add()    '行追加
                        '追加する委員会構成一覧のグリッドの列に合わせてデータを作成
                        For colAddCnt = 0 To UBound(ARR_CULUMNSNAME_LIST_COMMITTEE)
                            Dim strInsertData As String = ""
                            If ARR_CULUMNSNAME_LIST_COMMITTEE(colAddCnt) = L_POST_NAME Or ARR_CULUMNSNAME_LIST_COMMITTEE(colAddCnt) = S_COMMITTEE_SEQ Then   '役職は初期値をして役職ID最大値を使用
                                strInsertData = strDefaultCommitteeSeq
                            ElseIf ARR_CULUMNSNAME_LIST_COMMITTEE(colAddCnt) = FLAG Then
                                strInsertData = "1"
                            Else
                                strInsertData = selectRaw.Cells.Item(ARR_CULUMNSNAME_LIST_COMMITTEE(colAddCnt)).Value.ToString.Trim()
                            End If
                            addData.Rows.Item(addData.Rows.Count - 1).Item(ARR_CULUMNSNAME_LIST_COMMITTEE(colAddCnt)) = strInsertData
                        Next

                        Call SetCommitteeCompositionList(addData, False)
                        Call SetRowCnt()

                        '委員会構成一覧に表示中の人数を表示
                        Call CountVisibleMember()
                    End If
                Next

                '重複データはエラー表示する
                If aryNameList.Count > 0 Then
                    If aryNameList.Count = 1 Then
                        CLMsg.Show("GE0009", aryNameList(0))
                    Else
                        Dim iCnt As Integer = 0
                        Dim aryErrList As ArrayList = New ArrayList()
                        Dim clsUC999999 As New UC999999

                        For Each strName As String In aryNameList
                            aryErrList.Add(CLMsg.GetMsg("GE0009", aryNameList(iCnt)))
                            iCnt = iCnt + 1
                        Next
                        clsUC999999.errMsgList = aryErrList
                        'エラーメッセージリストの表示
                        clsUC999999.ShowDialog()

                    End If
                End If

            End If

            '削除者リストボタンの表示非表示を行う
            Call btnDeleteListVisibleControl()
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnDel_Click")
            log.Fatal(ex.Message)
        End Try
    End Sub
#End Region

#Region "関数：SearchMain"
    Private Sub SearchMain()
        Try
            '入力チェック
            If Me.cboCommittee.Text.Length = 0 Or Me.cboYear.Text.Length = 0 Or Me.cboMonth.Text.Length = 0 Then
                CLMsg.Show("GI0003")
                Exit Sub
            End If

            '基準日設定前はその月の最終日とする
            Me.searchStandardDay = Me.cboYear.Text & Me.cboMonth.Text & Date.DaysInMonth(CInt(Me.cboYear.Text), CInt(Me.cboMonth.Text)).ToString

            '対象日付のチェック
            If ChkTargetDate(MDLoginInfo.PeriodId, Me.cboCommittee.SelectedValue, searchStandardDay) = False Then
                Exit Sub
            End If

            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            '委員会構成一覧 検索処理
            Call Me.getSearchData()

            '選択委員会の役職リストの作成
            Call Me.SetPostList()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "SearchMain")
        Finally
            ' カーソルを矢印に設定
            Me.Cursor = Cursors.Default
        End Try

    End Sub
#End Region

#Region "関数：getCommitteeUpdate"
    '***************************************************************************************************
    '   ＩＤ　：getCommitteeUpdate
    '   名称　：委員会名簿変更IDの取得
    '   概要  ：最新の委員会名簿変更IDを取得
    '   引数　：clsDb As CLAccessMdb ：データベースクラス
    '   戻り値：strRet AS String ：委員会名簿変更IDの最大値+1
    '   作成日：2012/02/15 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/15 Fujisaku  新規作成
    '***************************************************************************************************
    Private Function getCommitteeUpdate(ByVal clsDb As CLAccessMdb) As String
        Dim strRet As String = "1"
        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
            ' SQL作成
            strSql = "SELECT (MAX(CLng(c_committee_update)) + 1)" & vbCrLf
            strSql = strSql & "FROM committee_update_list"

            tbRet = clsDb.ExecuteSql(strSql)    ' SQL実行
            If tbRet.Rows.Count > 0 Then        ' 処理件数判定                                                                
                strRet = tbRet.Rows(0).Item(0).ToString()
            Else
                strRet = "1"
            End If
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return strRet
    End Function
#End Region
#End Region

End Class
#End Region
