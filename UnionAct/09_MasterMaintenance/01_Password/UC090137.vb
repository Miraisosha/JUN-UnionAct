#Region "UC090137"
'===========================================================================================================
'   クラスＩＤ　　：UC090137
'   クラス名称　　：権限マスタメンテナンス
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDCommon
Imports UnionAct.GUI.Document
Imports C1.Win.C1FlexGrid
Imports UnionAct.GUI.Common

Public Class UC090137

#Region "定数・変数"
    Private Const COMMITTEE_KI_KIKAN_NAME As String = "期の期間"                         '名称：期の期間
    Private Const COMMITTEE_MASTER_INFO = "委員会組織明細マスタの情報"                   '名称
    Private Const COMMITTEE_COMBO_COMMITTEE = "部／委員会"                               '名称
    Private Const COMMITTEE_COMBO_COM_SEQ = "役職"                                       '名称
    Private Const DAILYPAY_MASTER_INFO = "日当明細マスタの情報"                          '名称
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private strPreYear As String                                                        '現在選択した年
    Private strPreMonth As String                                                       '現在選択した月

    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC090137              ' UC090137
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC090137          ' 権限マスタメンテナンス画面
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC090137_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku新規作成
    '***************************************************************************************************
    Private Sub UC090137_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Me.cmbCommittee.Enabled = True
            Me.cmbComSeq.Enabled = True
            Me.cmbCommittee2.Enabled = False
            Me.cmbComSeq2.Enabled = False

            '委員会リスト生成
            Call Me.setComboboxValueFromDB(Me.cmbCommittee, MDLoginInfo.PeriodId)
            Call Me.setComboboxValueFromDB(Me.cmbCommittee2, MDLoginInfo.PeriodId)
            Me.cmbCommittee.SelectedIndex = 0
            Me.cmbCommittee2.SelectedIndex = 0

            Me.btnSearch.Visible = True
            Me.btnCopy.Visible = True
            Me.btnUpdate.Visible = False
            Me.btnConfirm.Visible = False
            Me.btnCancel.Visible = False
            Me.btnCopy.Enabled = False

            'セルスタイルを定義
            Call DefinitionStyles()

            'ログイン権限でボタン制御
            Dim dt As DataTable = Nothing
            Dim strRead As String = ""
            Dim strReg As String = ""
            Dim strPrint As String = ""
            Dim strFile As String = ""
            dt = MDCommon.getGrant(MENU_ID_FM090101)
            If dt.Rows.Count > 0 Then
                strRead = dt.Rows(0).Item(3).ToString
                strReg = dt.Rows(0).Item(4).ToString
                strPrint = dt.Rows(0).Item(5).ToString
                strFile = dt.Rows(0).Item(6).ToString
                btnSearch.Enabled = CInt(strRead)
                btnConfirm.Enabled = CInt(strReg)
                btnUpdate.Enabled = CInt(strReg)
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要　：
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        '検索主処理呼び出し
        Call Me.searchMain(Me.cmbCommittee, Me.cmbComSeq)

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnUpdate_Click
    '   名称　：内容変更ボタンクリック処理
    '   概要　：
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        FrmWaitInfo.ShowWaitForm(Nothing)
        Try
            Me.btnSearch.Enabled = False
            Me.btnCopy.Enabled = True
            Me.btnUpdate.Visible = False
            Me.btnConfirm.Visible = True
            Me.btnCancel.Visible = True

            Call Utilities.SetCanEditToControl(False, Me.cmbCommittee)
            Call Utilities.SetCanEditToControl(False, Me.cmbComSeq)

            Me.cmbCommittee2.Enabled = True
            Me.cmbComSeq2.Enabled = True

            ' 暫定で他からのコピーのみ
            flxAttendance.AllowEditing = False
            ' 自由入力にする場合はTrue
            'flxAttendance.AllowEditing = True

            Me.cmbCommittee2.Focus()

            FrmWaitInfo.CloseWaitForm()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            FrmWaitInfo.CloseWaitForm()
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            FrmWaitInfo.CloseWaitForm()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCopy_Click
    '   名称　：コピーボタンクリック処理
    '   概要　：
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Dim dbAccess As New CLAccessMdb                         'DBアクセス

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        ' コピー元を引数とした検索主処理呼び出し
        Call Me.searchMain(Me.cmbCommittee2, Me.cmbComSeq2)

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnConfirm_Click
    '   名称　：登録確認ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/18(金)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  ryu  新規作成
    '***************************************************************************************************
    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim sql As String                               'SQL文
        Dim dbAccess As New CLAccessMdb                 'DBアクセス
        Dim rowCounter As Integer                       '行カウンター

        Dim strCommittee As String                      '選択委員会
        Dim strCommSeq As String                        '選択役職
        Dim intRtn As Integer                           'SQL文の実行結果
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '確認ダイアログ
            If CLMsg.Show("GQ0001") = DialogResult.Yes Then
                Cursor.Current = Cursors.WaitCursor
                strCommittee = Me.cmbCommittee.SelectedValue
                strCommSeq = Me.cmbComSeq.SelectedValue
                'DBアクセス接続
                Call dbAccess.Connect()
                'DBトランザクション開始
                Call dbAccess.BeginTran()

                ' 元の権限レコードをDelete
                sql = ""
                sql = sql & "DELETE FROM committee_screen_dtl "
                sql = sql & "WHERE c_committee_id = '" & strCommittee & "'"
                sql = sql & " AND  s_committee_seq = '" & strCommSeq & "'"

                'SQL文実行
                intRtn = dbAccess.ExecuteNonQuery(sql)
                If intRtn = -1 Then
                    CLMsg.Show("GE0001")
                    Exit Sub
                End If

                For rowCounter = 1 To (flxAttendance.Rows.Count - 1)

                    ' 権限レコードのINSERT
                    sql = ""
                    sql = sql & "INSERT INTO committee_screen_dtl ("
                    sql = sql & "c_committee_id, s_committee_seq, c_menu_id, "
                    sql = sql & "c_now_reference, c_now_reg, c_now_print, c_now_output_file, "
                    sql = sql & "c_before_reference, c_before_reg, c_before_print, c_before_output_file, "
                    sql = sql & "c_two_before_reference, c_two_before_reg, c_two_before_print, c_two_before_output_file, "
                    sql = sql & "l_biko, d_ins, c_user_id_ins"
                    sql = sql & ") VALUES("
                    sql = sql & "'" & strCommittee & "'"            ' c_committee_id
                    sql = sql & ", '" & strCommSeq & "'"            ' s_committee_seq
                    sql = sql & ", '" & CStr(flxAttendance.GetData(rowCounter, 1)) & "'"    ' c_menu_id
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 2) & "'"          ' 現在期 参照
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 3) & "'"          ' 現在期 登録
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 4) & "'"          ' 現在期 印刷
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 5) & "'"          ' 現在期 ファイル出力
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 6) & "'"          ' 前期 参照
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 7) & "'"          ' 前期 登録
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 8) & "'"          ' 前期 印刷
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 9) & "'"          ' 前期 ファイル出力
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 10) & "'"         ' 前期以前 参照
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 11) & "'"         ' 前期以前 登録
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 12) & "'"         ' 前期以前 印刷
                    sql = sql & ", '" & Me.getDataFormChekBox(rowCounter, 13) & "'"         ' 前期以前 ファイル出力
                    sql = sql & ", ''"                              ' l_biko
                    sql = sql & ", '" & Now & "'"                   ' d_ins
                    sql = sql & ", '" & MDLoginInfo.UserId & "'"    ' c_user_id_ins
                    sql = sql & ")"

                    'SQL文実行
                    intRtn = dbAccess.ExecuteNonQuery(sql)
                    If intRtn = -1 Then
                        CLMsg.Show("GE0001")
                        Exit Sub
                    End If
                Next
                'DBトランザクションコミット
                dbAccess.CommitTran()
                'DB接続切断
                Call dbAccess.Disconnect()

                '表示制御
                Call Utilities.SetCanEditToControl(True, Me.cmbCommittee)
                Call Utilities.SetCanEditToControl(True, Me.cmbComSeq)
                Me.cmbCommittee2.Enabled = False
                Me.cmbCommittee2.SelectedIndex = 0
                Me.cmbComSeq2.Enabled = False
                Me.cmbComSeq2.DataSource = Nothing

                btnSearch.Enabled = True
                btnCopy.Enabled = False
                btnConfirm.Visible = False
                btnCancel.Visible = False
                grpResult.Visible = True
                flxAttendance.AllowEditing = False
                '再検索
                Call Me.searchMain(Me.cmbCommittee, Me.cmbComSeq)
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Cursor.Current = Cursors.Default
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If CLMsg.Show("GQ0007") = DialogResult.Yes Then
                Call Utilities.SetCanEditToControl(True, Me.cmbCommittee)
                Call Utilities.SetCanEditToControl(True, Me.cmbComSeq)
                Me.cmbCommittee2.Enabled = False
                Me.cmbCommittee2.SelectedIndex = 0
                Me.cmbComSeq2.Enabled = False
                Me.cmbComSeq2.DataSource = Nothing

                btnSearch.Enabled = True
                btnCopy.Enabled = False
                btnConfirm.Visible = False
                btnCancel.Visible = False
                btnUpdate.Visible = False
                grpResult.Visible = False
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbCommittee_KeyPress
    '   名称　：委員会コンボボックスキープレス処理
    '   概要　：
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub cmbCommittee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbCommittee.KeyPress
        If e.KeyChar = vbCr Then
            btnSearch_Click(sender, Nothing)
        End If
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbComSeq_KeyPress
    '   名称　：役職コンボボックスキープレス処理
    '   概要　：
    '   作成日：2011/11/18(金)  ryu
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  ryu  新規作成
    '***************************************************************************************************
    Private Sub cmbComSeq_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbComSeq.KeyPress
        If e.KeyChar = vbCr Then
            btnSearch_Click(sender, Nothing)
        End If
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbCommittee_SelectionChangeCommitted
    '   名称　：委員会コンボボックスチェンジ処理
    '   概要　：役職コンボボックスを選択した委員会の役職に設定しなおす
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '--------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub cmbCommittee_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCommittee.SelectionChangeCommitted
        Dim strKiValue As String                        '選択した委員会コード
        Try
            If Not Me.cmbCommittee.SelectedIndex = 0 Then
                ' 委員会役職コンボボックス更新
                strKiValue = Me.cmbCommittee.SelectedValue
                setComboboxSeq(Me.cmbComSeq, strKiValue)
            Else
                '「役職」コンボボックスの初期化
                Me.cmbComSeq.DataSource = Nothing
            End If

            ' 検索結果エリアの非表示化
            Me.grpResult.Visible = False
            btnUpdate.Visible = False
            btnConfirm.Visible = False
            btnCancel.Visible = False
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbComSeq_SelectionChangeCommitted
    '   名称　：役職コンボボックスチェンジ処理
    '   概要　：検索結果をクリアする
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '--------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub cmbComSeq_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbComSeq.SelectionChangeCommitted
        Try
            ' 検索結果エリアの非表示化
            Me.grpResult.Visible = False
            btnUpdate.Visible = False
            btnConfirm.Visible = False
            btnCancel.Visible = False
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbCommittee2_SelectionChangeCommitted
    '   名称　：コピー元委員会コンボボックスチェンジ処理
    '   概要　：コピー元役職コンボボックスを選択したコピー元委員会の役職に設定しなおす
    '   作成日：2012/03/27 Fujisaku
    '   更新日：
    '--------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub cmbCommittee2_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCommittee2.SelectionChangeCommitted
        Dim strKiValue As String                        '選択した委員会コード
        Try
            If Not Me.cmbCommittee2.SelectedIndex = 0 Then
                ' コピー元委員会役職コンボボックス更新
                strKiValue = Me.cmbCommittee2.SelectedValue
                setComboboxSeq(Me.cmbComSeq2, strKiValue)
            Else
                ' コピー元「役職」コンボボックスの初期化
                Me.cmbComSeq2.DataSource = Nothing
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub



#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：setComboboxValueFromDB
    '   名称　：コンボボックス生成
    '   概要　：選択した日程情報で画面を初期化する
    '   引数　：ByVal cmbCom As ComboBox = 生成する対象のコンボボックス
    '         ：ByVal strPeriodID As String = ログインで選択した期のID
    '   戻り値：なし
    '   作成日：2012/03/27 Fujisaku
    '   更新日：2018/12/18 Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/27 Fujisaku  新規作成
    '   　　　：2018/12/18 Fujisaku  新規追加した委員会の権限も編集できるよう修正
    '***************************************************************************************************
    ''' <summary>コンボボックス生成</summary>
    ''' <param name="strPeriodID">ログインで選択した期のID</param>
    ''' <remarks></remarks>
    Private Sub setComboboxValueFromDB(ByVal cmbCom As ComboBox, ByVal strPeriodID As String)
        Dim dbAccess As New CLAccessMdb         'DBアクセス
        Dim dt As DataTable                     'データテーブル
        Dim dtRow As DataRow                    '一行のデータ
        Dim d_from As String                    '期の開始日
        Dim d_to As String                      '期の終了日
        Dim systemDate As String                '現在日
        Dim sql As String                       'SQL分
        Dim comStr As String                    '管理部ユーザー参照できる委員会
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '「委員会名」コンボボックスの初期化
            cmbCom.BeginUpdate()
            cmbCom.DataSource = Nothing
            cmbCom.Items.Clear()
            ' 期の開始・終了日を取得
            sql = "select d_from, d_to from period where c_period_id='" + strPeriodID + "'"
            ' データベースに接続
            dbAccess.Connect()
            ' データを取得
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count = 1 Then
                dtRow = dt.Rows(0)
                d_from = dtRow("d_from")
                d_to = dtRow("d_to")
                systemDate = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            Else
                CLMsg.Show("GE0004", COMMITTEE_KI_KIKAN_NAME)
                Exit Sub
            End If

            sql = "select c_committee_id, l_name from committee where d_from<='" + d_to + "' and d_to>='" + systemDate + "' "
            Select Case MDLoginInfo.CommitteeStatusFlg
                Case 0
                    '専従ユーザー
                Case 1
                    '一般の委員会ユーザー
                    sql = sql + "and c_committee_id='" + MDLoginInfo.CommitteeId + "'"
                Case 2
                    '管理部ユーザー
                    If MDLoginInfo.CommitteeIdList.Count > 0 Then
                        comStr = ""
                        sql = sql + "and c_committee_id in("
                        For intComm As Integer = 0 To MDLoginInfo.CommitteeIdList.Count - 1
                            comStr = comStr + "'" + MDLoginInfo.CommitteeIdList(intComm) + "'"
                            If intComm = MDLoginInfo.CommitteeIdList.Count - 1 Then
                                comStr = comStr + ")"
                            Else
                                comStr = comStr + ","
                            End If
                        Next
                        sql = sql + comStr
                    Else
                        CLMsg.Show("GE0004", COMMITTEE_KI_KIKAN_NAME)
                        Exit Sub
                    End If
            End Select

            ' コンボボックス作成処理
            MDCommon.CreateComboBoxNew(dbAccess, cmbCom, sql, "l_name", "c_committee_id", True)

            cmbCom.EndUpdate()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベースの接続を切断
            dbAccess.Disconnect()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：setComboboxSeq
    '   名称　：コンボボックス生成
    '   概要　：選択した委員会で役職コンボボックスを初期化する
    '   引数　：ByVal comSeq As ComboBox = 値設定するコンボボックス
    '         ：ByVal strCommittee As String = 選択した委員会
    '   戻り値：なし
    '   作成日：2012/03/26 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/26 Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub setComboboxSeq(ByVal comSeq As ComboBox, ByVal strCommittee As String)
        Dim dbAccess As New CLAccessMdb         'DBアクセス
        Dim sql As String                       'SQL分
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '「役職」コンボボックスの初期化
            comSeq.BeginUpdate()
            comSeq.DataSource = Nothing
            comSeq.Items.Clear()

            ' sqlの作成
            Dim strNow As String = Format(Now, "yyyyMMdd")
            sql = "SELECT * FROM committee_dtl "
            sql = sql & "WHERE c_committee_id = '" & strCommittee & "'"
            sql = sql & " AND  d_from <= '" & strNow & "'"
            sql = sql & " AND  d_to >= '" & strNow & "'"

            ' データベースに接続
            dbAccess.Connect()

            ' コンボボックス作成処理
            MDCommon.CreateComboBoxNew(dbAccess, comSeq, sql, "l_name", "s_committee_seq", True)

            comSeq.EndUpdate()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベースの接続を切断
            dbAccess.Disconnect()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：CheckInput
    '   名称　：入力チェック処理
    '   概要　：入力チェック
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/26 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/26 Fujisaku  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CheckInput(ByVal cmbCom As ComboBox, ByVal cmbSeq As ComboBox) As Boolean
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            ' 必須選択チェック
            If cmbCom.IsDisposed OrElse cmbCom.SelectedIndex < 1 Then
                CLMsg.Show("GE0010", COMMITTEE_COMBO_COMMITTEE)
                Return False
            End If
            If cmbSeq.IsDisposed OrElse cmbSeq.SelectedIndex < 1 Then
                CLMsg.Show("GE0010", COMMITTEE_COMBO_COM_SEQ)
                Return False
            End If

            Return True
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Function

    '***************************************************************************************************
    '   ＩＤ　：DefinitionStyles
    '   名称　：スタイル定義
    '   概要　：セルのスタイルを定義
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/03/26 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/26 Fujisaku 新規作成
    '***************************************************************************************************
    ''' <summary>スタイル定義</summary>
    ''' <remarks></remarks>
    Private Sub DefinitionStyles()
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            'スタイルを作成
            Dim cs As CellStyle
            ' チェックボックス（背景白　通常）
            cs = flxAttendance.Styles.Add("bool")
            cs.DataType = Type.GetType("System.Boolean")
            cs.ImageAlign = ImageAlignEnum.CenterCenter
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black

            ' ヘッダー部
            cs = flxAttendance.Styles.Add("normal")
            cs.DataType = Type.GetType("System.String")
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
            ' ロック
            cs = flxAttendance.Styles.Add("lightYellowCell")
            cs.BackColor = Color.Yellow
            cs.TextAlign = TextAlignEnum.CenterCenter
            cs.Editor = Nothing
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
            ' 地位喪失脱退社員
            cs = flxAttendance.Styles.Add("FixedFontPink")
            cs.ForeColor = Color.DeepPink
            cs.Border.Direction = BorderDirEnum.Both
            cs.Border.Color = Color.Black
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeScreenDtl
    '   名称　：権限データ取得処理
    '   概要　：権限データ取得
    '   引数　：ByVal strCommitteeId As String = 委員会ID,
    '           ByVal strCommitteeSeq As String = 役職SEQ
    '   戻り値：DataTable = 権限データ
    '   作成日：2012/03/26  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/26  Fujisaku  新規作成
    '***************************************************************************************************
    ''' <summary>権限データ取得処理</summary>
    ''' <param name="strCommitteeId">委員会ID</param>
    ''' <param name="strCommitteeSeq">役職SEQ</param>
    ''' <returns>権限データ</returns>
    ''' <remarks></remarks>
    Public Function GetCommitteeScreenDtl(ByVal strCommitteeId As String, _
                                          ByVal strCommitteeSeq As String) As DataTable
        Dim sql As String
        Dim dbAccess As New CLAccessMdb        '処理開始ログ
        Dim table2 As DataTable = Nothing
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

            Dim dt As New DataTable("committee_screen_dtl")

            sql = "SELECT menuCtr.c_menu_id AS c_menu_id "
            sql = sql & "     , menuCtr.l_name AS l_name "
            sql = sql & "     , comScrDtl.c_now_reference   AS c_now_ref "
            sql = sql & "     , comScrDtl.c_now_reg         AS c_now_reg "
            sql = sql & "     , comScrDtl.c_now_print       AS c_now_pri "
            sql = sql & "     , comScrDtl.c_now_output_file AS c_now_out "
            sql = sql & "     , comScrDtl.c_before_reference   AS c_bef_ref "
            sql = sql & "     , comScrDtl.c_before_reg         AS c_bef_reg "
            sql = sql & "     , comScrDtl.c_before_print       AS c_bef_pri "
            sql = sql & "     , comScrDtl.c_before_output_file AS c_bef_out "
            sql = sql & "     , comScrDtl.c_two_before_reference   AS c_two_ref "
            sql = sql & "     , comScrDtl.c_two_before_reg         AS c_two_reg "
            sql = sql & "     , comScrDtl.c_two_before_print       AS c_two_pri "
            sql = sql & "     , comScrDtl.c_two_before_output_file AS c_two_out "
            sql = sql & "FROM menucontrol menuCtr "
            sql = sql & "LEFT OUTER JOIN (SELECT * "
            sql = sql & "                 FROM committee_screen_dtl "
            sql = sql & "                 WHERE c_committee_id = '" & strCommitteeId & "' "
            sql = sql & "                  AND  s_committee_seq = '" & strCommitteeSeq & "' "
            sql = sql & "                ) comScrDtl "
            sql = sql & "ON menuCtr.c_menu_id = comScrDtl.c_menu_id "
            sql = sql & "WHERE menuCtr.c_ksh = '" & MDLoginInfo.Ksh & "' "
            sql = sql & "ORDER BY menuCtr.c_menu_id"

            Call dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)

            If dt.Rows.Count = 0 Then
                Return Nothing
            Else
                dt.TableName = "committee_screen_dtl"
                table2 = dt
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call dbAccess.Disconnect()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Return table2
    End Function

    '***************************************************************************************************
    '   ＩＤ　：searchMain
    '   名称　：検索処理
    '   概要　：検索の主処理
    '   引数　：
    '   戻り値：Boolean 正常/異常
    '   作成日：2012/03/26 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/26 Fujisaku  新規作成
    '***************************************************************************************************
    Public Function searchMain(ByVal cmbCom As ComboBox, ByVal cmbSeq As ComboBox) As Boolean
        Dim retBln As Boolean = False                           '戻り値
        Dim iCounter As Integer                                 'カラムカウンター
        Dim dbAccess As New CLAccessMdb                         'DBアクセス
        Dim dt As DataTable                                     'データテーブル
        Dim dtrow As DataRow                                    '一行のデータ
        Dim iRowCounter As Integer                              '行数カウンター
        Dim intChk As Integer                                   'チェック状態

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '入力チェック
            If Me.CheckInput(cmbCom, cmbSeq) = False Then
                Exit Function
            End If
            '入力チェック通った場合、検索を行う
            FrmWaitInfo.ShowWaitForm(Nothing)

            flxAttendance.Rows.Count = 1
            flxAttendance.Cols.Count = 14
            flxAttendance.Redraw = False
            flxAttendance.AllowMerging = AllowMergingEnum.Free

            '委員会の役職に対応する権限を取得
            Call dbAccess.Connect()
            dt = Me.GetCommitteeScreenDtl(cmbCom.SelectedValue, cmbSeq.SelectedValue)

            If Not dt Is Nothing Then
                If dt.Rows.Count > 0 Then
                    ' メニュー件数分行数追加
                    flxAttendance.Rows.Add(dt.Rows.Count)

                    For iRowCounter = 0 To dt.Rows.Count - 1
                        dtrow = dt.Rows(iRowCounter)
                        'メニュー名
                        flxAttendance.SetData(iRowCounter + 1, 0, dtrow("l_name"))
                        'メニューID
                        flxAttendance.SetData(iRowCounter + 1, 1, dtrow("c_menu_id"))

                        'チェックボックスセルを設定
                        For iCounter = 2 To 13
                            If IsDBNull(dtrow(iCounter)) Then
                                intChk = CheckEnum.Unchecked
                            Else
                                intChk = dtrow(iCounter)
                            End If
                            flxAttendance.SetCellStyle(iRowCounter + 1, iCounter, "bool")
                            flxAttendance.SetCellCheck(iRowCounter + 1, iCounter, intChk)
                        Next
                    Next
                End If
            End If
            'メニューIDカラムを非表示
            flxAttendance.Cols(1).Visible = False
            '編集不可
            flxAttendance.AllowEditing = False
            'サイズ調整
            flxAttendance.Cols.Item(0).Width = 220
            Dim m As Integer
            For m = 2 To Me.flxAttendance.Cols.Count - 2
                flxAttendance.Cols.Item(m).Width = 130
                flxAttendance.Cols.Item(m).TextAlign = TextAlignEnum.CenterCenter
                flxAttendance.Cols.Item(m).AllowMerging = False
            Next m
            '属性設定
            flxAttendance.SelectionMode = SelectionModeEnum.Cell
            flxAttendance.AllowDragging = AllowDraggingEnum.None
            flxAttendance.AllowResizing = AllowResizingEnum.None
            flxAttendance.AllowSorting = AllowSortingEnum.None
            flxAttendance.AutoResize = False

            'レコードがないとき、内容変更ボタンを非表示
            If flxAttendance.Rows.Count <= 1 Then
                btnUpdate.Visible = False
            Else
                btnUpdate.Visible = True
            End If

            'GRID再描化
            flxAttendance.Redraw = True
            grpResult.Visible = True
            '戻り値設定
            retBln = True
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call dbAccess.Disconnect()
            dbAccess = Nothing
            FrmWaitInfo.CloseWaitForm()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Return retBln
    End Function

    '***************************************************************************************************
    '   ＩＤ　：getDataFormChekBox
    '   名称　：チェックボックス値取得
    '   概要　：チェックボックスのチェック状態から、DB登録値を取得する
    '   引数　：ByVal iRow As Integer = 行
    '         ：ByVal iCol As Integer = 列
    '   戻り値：rtnStr As String (チェックあり"1"/チェックなし"0")
    '   作成日：2012/03/26 Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/26 Fujisaku  新規作成
    '***************************************************************************************************
    Private Function getDataFormChekBox(ByVal iRow As Integer, ByVal iCol As Integer) As String
        Dim rtnStr As String
        If flxAttendance.GetCellCheck(iRow, iCol) = CheckEnum.Checked Then
            rtnStr = "1"
        Else
            rtnStr = "0"
        End If
        Return rtnStr
    End Function
#End Region

End Class
#End Region
