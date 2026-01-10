#Region "FM040602"
'===========================================================================================================
'   クラスＩＤ　　：FM040602
'   クラス名称　　：発信文書新規作成画面
'   備考  　　　　：
'===========================================================================================================

Imports System.Data.OleDb
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Common

Public Class FM040602

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Private m_dtDocumentSubjectInfo As DataTable = Nothing      ' 発信文書件名情報（管理コード・テンプレート区分・標題枝番・標題・詳細設定分類等）
    Private m_dtPeriodInfo As DataTable = Nothing               ' 期情報（期ID・会社コード・期区分・名称・略名称等）

    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM040602      ' FM040602
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM040602  ' 発信文書新規作成画面
    '
    Private Const MANUAL_INSERT = "↓任意入力．．．"
    Private Const STR_CHANGE_LIST = "変更データ一覧"

    Private ReadOnly CON_DGV_COL_NAME As String() = {"col_check", "commitee_name", "apply_year_month", "update_date", "update_user", "condition", "committee_update_id", "doc_out", "committee_id", "update_user_id"}
    Private ReadOnly CON_DGV_COL_TEXT As String() = {"", "委員会名", "適用年月", "登録日時", "登録者", "状態", "委員会変更ID", "帳票出力", "委員会ID", "変更者ID"}
    Private ReadOnly CON_DGV_COL_WIDTH_ADDDEL As Integer() = {30, 200, 100, 150, 140, 80, 50, 50, 50, 50}
    Private ReadOnly CON_DGV_COL_WIDTH_CHANGE As Integer() = {0, 230, 130, 180, 170, 0, 50, 50, 50, 50}
    Private ReadOnly CON_DGV_COL_VISIBLE_ADDDEL As Integer() = {True, True, True, True, True, True, False, False, False, False}
    'Private ReadOnly CON_DGV_COL_VISIBLE_ADDDEL As Integer() = {True, True, True, True, True, True, True, True, True, True}
    Private ReadOnly CON_DGV_COL_VISIBLE_CHANGE As Integer() = {False, True, True, True, True, False, False, False, False, False}
    ' 権限
    Private strGrantReference As String = "0"                   ' 参照権限
    Private strGrantInsert As String = "0"                      ' 登録権限
    Private strGrantPrint As String = "0"                       ' 印刷権限
    Private strGrantFileOutput As String = "0"                  ' ファイル出力権限
    ' 詳細設定分類
    Private Const DETAILS_NORMAL As String = "0"                ' 標準
    Private Const DETAILS_ADDDEL As String = "1"                ' 追加・削除
    Private Const DETAILS_CHANGE As String = "2"                ' 部長、委員長の交代
#End Region

#Region "プロパティ"
    Public _strTemplate As String = ""                          ' テンプレート区分
    Public _strTemplateFile As String = ""                      ' テンプレートファイル名（ファイル名のみ）
    Public _strTemplateFileFull As String = ""                  ' テンプレートファイル名（フルパス）

    Public _intSubjectSeq As Integer = 0                        ' 標題枝番
    Public _strPeriodId As String = ""                          ' 期ID
    Public _intPeriod As Integer = 0                            ' 期（数値）
    Public _strPeriodName As String = ""                        ' 期（全角期（第ＸＸ期））
    Public _strDocCode As String = ""                           ' 管理コード
    Public _strSubject As String = ""                           ' 標題（任意入力欄）
    Public _strApplyDate As String = ""                         ' 適用日付
    Public _strCommitteeUpdate As String() = Nothing            ' 変更ID
    Public _strDocumentOut As String() = Nothing                ' 帳票出力
    Public _strCommitteeDFrom As String() = Nothing             ' 委員会適用日付

    Public _strDetails As String = ""                           ' 詳細設定分類

    ' テンプレート区分
    Public Property strTemplate() As String
        Get
            Return _strTemplate
        End Get
        Set(ByVal value As String)
            _strTemplate = value
        End Set
    End Property

    ' テンプレートファイル名（ファイル名のみ）
    Public Property strTemplateFile() As String
        Get
            Return _strTemplateFile
        End Get
        Set(ByVal value As String)
            _strTemplateFile = value
        End Set
    End Property

    ' テンプレートファイル名（フルパス）
    Public Property strTemplateFileFull() As String
        Get
            Return _strTemplateFileFull
        End Get
        Set(ByVal value As String)
            _strTemplateFileFull = value
        End Set
    End Property

    ' 標題枝番
    Public Property intSubjectSeq() As Integer
        Get
            Return _intSubjectSeq
        End Get
        Set(ByVal value As Integer)
            _intSubjectSeq = value
        End Set
    End Property

    ' 期ID
    Public Property strPeriodId() As String
        Get
            Return _strPeriodId
        End Get
        Set(ByVal value As String)
            _strPeriodId = value
        End Set
    End Property

    ' 期（数値）
    Public Property intPeriod() As Integer
        Get
            Return _intPeriod
        End Get
        Set(ByVal value As Integer)
            _intPeriod = value
        End Set
    End Property

    ' 期（全角期（第ＸＸ期））
    Public Property strPeriodName() As String
        Get
            Return _strPeriodName
        End Get
        Set(ByVal value As String)
            _strPeriodName = value
        End Set
    End Property

    ' 管理コード
    Public Property strDocCode() As String
        Get
            Return _strDocCode
        End Get
        Set(ByVal value As String)
            _strDocCode = value
        End Set
    End Property

    ' 標題
    Public Property strSubject() As String
        Get
            Return _strSubject
        End Get
        Set(ByVal value As String)
            _strSubject = value
        End Set
    End Property

    ' 適用日付
    Public Property strApplyDate() As String
        Get
            Return _strApplyDate
        End Get
        Set(ByVal value As String)
            _strApplyDate = value
        End Set
    End Property

    ' 委員会変更ID
    Public Property strCommitteeUpdate() As String()
        Get
            Return _strCommitteeUpdate
        End Get
        Set(ByVal value As String())
            _strCommitteeUpdate = value
        End Set
    End Property

    ' 帳票出力
    Public Property strDocumentOut() As String()
        Get
            Return _strDocumentOut
        End Get
        Set(ByVal value As String())
            _strDocumentOut = value
        End Set
    End Property

    ' 委員会適用日付
    Public Property strCommitteeDFrom() As String()
        Get
            Return _strCommitteeDFrom
        End Get
        Set(ByVal value As String())
            _strCommitteeDFrom = value
        End Set
    End Property

    ' 詳細設定分類
    Public Property strDetails() As String
        Get
            Return _strDetails
        End Get
        Set(ByVal value As String)
            _strDetails = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM040602_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/02/29(水)  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/29(水)  m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub FM040602_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            '-------------------------------------------------------------------------------
            '   画面中央表示処理
            '-------------------------------------------------------------------------------
            If MDCommon.SetFormCenter(Me) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   権限取得処理
            '-------------------------------------------------------------------------------
            If Me.setGrant() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If Me.ControlClear() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   グリッド初期化処理
            '-------------------------------------------------------------------------------
            ' データグリッドビューの初期化
            If Me.DataGridViewIni() = False Then
                Exit Sub
            End If
            'Call InitializeDataGridStyle()

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If Me.GetData() = False Then
                Exit Sub
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCreate_Click
    '   名称　：作成ボタンクリック
    '   概要　：
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click

        Try
            '-----------------------------------------------------------
            '   入力チェック処理
            '-----------------------------------------------------------
            If Me.ChkInput() = False Then
                Exit Sub
            End If

            '-----------------------------------------------------------
            '   作成メイン処理
            '-----------------------------------------------------------
            If Me.MainInsert() = False Then
                Exit Sub
            End If

            ' ダイアログ結果格納（OK）
            Me.DialogResult = Windows.Forms.DialogResult.OK

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

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要　：
    '   作成日：2012/02/29(水)  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/29(水)  m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            ' ダイアログ結果格納（キャンセル）
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

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

    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック
    '   概要　：
    '   作成日：2012/02/29(水)  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/29(水)  m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            '登録年月日FROMとTOの大小チェック
            If (Me.dtpFrom.Value.Date > Me.dtpTo.Value.Date) Then
                CLMsg.Show("GE0207")
                Exit Sub
            End If

            ' 検索メイン処理
            If Me.MainSearch() = False Then
                Exit Sub
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnDelete_Click
    '   名称　：削除ボタンクリック
    '   概要　：
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Try
            ' 削除メイン処理
            If Me.MainDelete() = False Then
                Exit Sub
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSelectCancel_Click
    '   名称　：選択を解除ボタンクリック
    '   概要　：
    '   作成日：2012/02/29(水)  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/29(水)  m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnSelectCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectCancel.Click

        Try
            Me.dgvResult.ClearSelection()

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

    '***************************************************************************************************
    '   ＩＤ　：btnAllCheck_Click
    '   名称　：全チェックボタンクリック
    '   概要　：
    '   作成日：2012/02/29(水)  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/29(水)  m.somezaki  新規作成
    '***************************************************************************************************
    Private Sub btnAllCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllCheck.Click

        Try
            ' チェックオン・オフ処理
            If Me.SetCheckAll(True) = False Then
                Exit Sub
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnAllNoCheck_Click
    '   名称　：全チェック解除ボタンクリック
    '   概要　：
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnAllNoCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllNoCheck.Click

        Try
            ' チェックオン・オフ処理
            If Me.SetCheckAll(False) = False Then
                Exit Sub
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboDocCode_SelectionChangeCommitted
    '   名称　：管理コードコンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/03/17(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/17(土)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboDocCode_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboDocCode.SelectedIndexChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
                Exit Sub
            End If

            ' 管理コードコンボボックスが選択されている場合
            If Me.cboDocCode.SelectedIndex > -1 Then
                Dim clsDb As New CLAccessMdb        ' データベースクラス
                Try
                    ' 標題コンボボックス作成処理
                    If Me.CreateComboBoxSubject(clsDb) = False Then
                        Exit Sub
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
                    ' データベース切断
                    Call clsDb.Disconnect()
                End Try
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

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboPeriod_SelectedIndexChanged
    '   名称　：期コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/03/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboPeriod_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboPeriod.SelectedIndexChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
                Exit Sub
            End If

            ' 期が選択されていた場合
            If Me.cboPeriod.SelectedIndex > -1 Then
                Me.strPeriodName = Me.m_dtPeriodInfo.Rows(Me.cboPeriod.SelectedIndex).Item(5).ToString()    ' 期（全角期（第ＸＸ期））
                Me.intPeriod = CInt(Me.m_dtPeriodInfo.Rows(Me.cboPeriod.SelectedIndex).Item(6))             ' 期（数値）
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboSubject_SelectedIndexChanged
    '   名称　：標題コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboSubject_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubject.SelectedIndexChanged

        Try
            ' 検索結果クリア処理
            If Me.ResultClear() = False Then
                Exit Sub
            End If

            ' 詳細設定分類取得
            If Me.cboSubject.SelectedIndex > -1 Then

                Me.strDetails = Me.m_dtDocumentSubjectInfo.Rows(Me.cboSubject.SelectedIndex).Item(6).ToString()
                Me.intSubjectSeq = CInt(Me.m_dtDocumentSubjectInfo.Rows(Me.cboSubject.SelectedIndex).Item(2))

                ' 標題チェンジ処理
                If Me.ChangeSubject(Me.strDetails) = False Then
                    Exit Sub
                End If

            End If

            'If Me.SetCommitteeSearchEnable() = False Then
            '    Exit Sub
            'End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgvResult_CellContentClick_1
    '   名称　：データグリッドビューセルクリック
    '   概要　：
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_CellContentClick_1(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResult.CellContentClick

        Try
            With Me.dgvResult.SelectedRows
                ' データがある場合
                If .Count > 0 Then
                    ' チェックボックス列の場合
                    If e.ColumnIndex = 0 Then
                        For i = 0 To .Count - 1
                            .Item(i).Cells(0).Value = Not (CType(.Item(i).Cells(0).Value, Boolean))
                        Next
                    End If
                End If
            End With

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgvResult_CellContentClick
    '   名称　：データグリッドビューセルクリック
    '   概要　：
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)

        Try
            With Me.dgvResult.SelectedRows
                ' データがある場合
                If .Count > 0 Then
                    ' チェックボックス列の場合
                    If e.ColumnIndex = 0 Then
                        For i = 0 To .Count - 1
                            .Item(i).Cells(0).Value = Not (CType(.Item(i).Cells(0).Value, Boolean))
                        Next
                    End If
                End If
            End With

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dtpFrom_ValueChanged
    '   名称　：登録年月日（FROM）値変更
    '   概要　：
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dtpFrom_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpFrom.ValueChanged
        '---------------------------------------
        '   検索結果クリア
        '---------------------------------------
        With Me.dgvResult
            If .Rows.Count > 0 Then
                .Rows.Clear()
            End If
        End With

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dtpTo_ValueChanged
    '   名称　：登録年月日（TO）値変更
    '   概要　：
    '   作成日：2012/03/20(火)  a.oonuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/20(火)  a.oonuma  新規作成
    '***************************************************************************************************
    Private Sub dtpTo_ValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpTo.ValueChanged
        '---------------------------------------
        '   検索結果クリア
        '---------------------------------------
        With Me.dgvResult
            If .Rows.Count > 0 Then
                .Rows.Clear()
            End If
        End With

    End Sub

#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：SetGrant
    '   名称　：権限処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function SetGrant() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim dtGrant As DataTable = Nothing          ' 権限取得データテーブル

        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC040601)
            If dtGrant.Rows.Count > 0 Then
                strGrantReference = dtGrant.Rows(0).Item(3).ToString        ' 参権限照
                strGrantInsert = dtGrant.Rows(0).Item(4).ToString           ' 登録権限
                strGrantPrint = dtGrant.Rows(0).Item(5).ToString            ' 印刷権限
                strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString       ' ファイル出力権限
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            '---------------------------------------------------
            '   検索項目
            '---------------------------------------------------
            ' ComboBox
            Me.cboPeriod.DataSource = Nothing                   ' 期
            Me.cboPeriod.Text = ""
            Me.cboDocCode.DataSource = Nothing                  ' 管理コード
            Me.cboDocCode.Text = ""
            Me.cboSubject.DataSource = Nothing                  ' 標題
            Me.cboSubject.Text = ""
            ' DateTimePicker
            Me.dtpApplyDate.Text = ""                           ' 適用日付
            ' TextBox
            Me.txtSubjectManual.Text = ""                       ' 任意入力欄

            '---------------------------------------------------
            '   委員会名簿登録年月日
            '---------------------------------------------------
            ' GroupBox
            Me.grpSearch.Visible = True                         ' 検索項目（委員会名簿登録年月日）
            Me.grpSearch.Enabled = True
            ' DateTimePicker
            Me.dtpFrom.Text = ""                                ' 委員会名簿登録年月日From
            Me.dtpTo.Text = ""                                  ' 委員会名簿登録年月日To
            ' Button
            Me.btnSearch.Visible = True                         ' 検索

            '---------------------------------------------------
            '   追加・削除データ一覧
            '---------------------------------------------------
            ' GroupBox
            Me.grpResult.Visible = True                         ' 検索結果
            Me.grpResult.Enabled = True
            ' DataGridView
            Me.dgvResult.Visible = True                         ' 検索結果
            ' Button
            Me.btnSelectCancel.Visible = True                   ' 選択を解除
            Me.btnDelete.Visible = True                         ' 削除
            Me.btnAllCheck.Visible = True                       ' 全チェック
            Me.btnAllNoCheck.Visible = True                     ' 全解除

            ' Button
            Me.btnCreate.Visible = True                         ' 作成
            Me.btnCancel.Visible = True                         ' キャンセル

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：DataGridViewIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DataGridViewIni() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            With Me.dgvResult
                '-----------------------------------------------------------------------------------
                '   グリッド全体設定
                '-----------------------------------------------------------------------------------
                ' 総数
                .RowCount = 0                                                                       ' 縦
                .ColumnCount = 10                                                                   ' 横
                ' スクロールバー
                .ScrollBars = ScrollBars.Both                                                       ' 縦横両方
                ' 1行選択モード
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect                            ' 1行選択
                .MultiSelect = False                                                                ' 複数選択なし
                ' サイズ変更
                .AllowUserToResizeColumns = True                                                    ' 列サイズ変更可
                .AllowUserToResizeRows = False                                                      ' 行サイズ変更不可

                ' 並び替え禁止
                For intCnt = 0 To .Columns.Count - 1
                    .Columns(intCnt).SortMode = DataGridViewColumnSortMode.NotSortable
                Next

                '-----------------------------------------------------------------------------------
                '   ヘッダー部設定
                '-----------------------------------------------------------------------------------
                ' ヘッダー文字列
                .Columns(0).HeaderText = ""                                                         ' 01. チェックボックス
                .Columns(1).HeaderText = "委員会名"                                                 ' 02. 委員会名
                .Columns(2).HeaderText = "適用年月"                                                 ' 03. 適用年月
                .Columns(3).HeaderText = "登録日時"                                                 ' 04. 登録日時
                .Columns(4).HeaderText = "登録者"                                                   ' 05. 登録者
                .Columns(5).HeaderText = "状態"                                                     ' 06. 状態
                .Columns(6).HeaderText = "委員会変更ID"                                             ' 07. 委員会変更ID
                .Columns(7).HeaderText = "出力"                                                     ' 08. 帳票出力
                .Columns(8).HeaderText = "委員会ID"                                                 ' 09. 委員会ID
                .Columns(9).HeaderText = "変更者ID"                                                 ' 10. 変更者ID
                ' ヘッダー文字位置
                .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 01. チェックボックス
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 02. 委員会名
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 03. 適用年月
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 04. 登録日時
                .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 05. 登録者
                .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 06. 状態
                .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 07. 委員会変更ID
                .Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 08. 帳票出力
                .Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 09. 委員会ID
                .Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 10. 変更者ID

                '-----------------------------------------------------------------------------------
                '   カラム部設定
                '-----------------------------------------------------------------------------------
                ' カラム文字位置
                .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 01. チェックボックス
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 02. 委員会名
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 03. 適用年月
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 04. 登録日時
                .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 05. 登録者
                .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 06. 状態
                .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 07. 委員会変更ID
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 08. 帳票出力
                .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 09. 委員会ID
                .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 10. 変更者ID
                ' カラム幅
                .Columns(0).Width = 30                                                              ' 01. チェックボックス
                .Columns(1).Width = 200                                                             ' 02. 委員会名
                .Columns(2).Width = 100                                                             ' 03. 適用年月
                .Columns(3).Width = 150                                                             ' 04. 登録日時
                .Columns(4).Width = 140                                                             ' 05. 登録者
                .Columns(5).Width = 80                                                              ' 06. 状態
                .Columns(6).Width = 50                                                              ' 07. 委員会変更ID
                .Columns(7).Width = 50                                                              ' 08. 帳票出力
                .Columns(8).Width = 50                                                              ' 09. 委員会ID
                .Columns(9).Width = 50                                                              ' 10. 変更者ID
                ' カラム表示有無
                .Columns(0).Visible = True                                                          ' 01. チェックボックス
                .Columns(1).Visible = True                                                          ' 02. 委員会名
                .Columns(2).Visible = True                                                          ' 03. 適用年月
                .Columns(3).Visible = True                                                          ' 04. 登録日時
                .Columns(4).Visible = True                                                          ' 05. 登録者
                .Columns(5).Visible = True                                                          ' 06. 状態
                .Columns(6).Visible = False                                                         ' 07. 委員会変更ID
                .Columns(7).Visible = False                                                         ' 08. 帳票出力
                .Columns(8).Visible = False                                                         ' 09. 委員会ID
                .Columns(9).Visible = False                                                         ' 10. 変更者ID
                ' カラム編集有無
                .Columns(0).ReadOnly = False                                                        ' 01. チェックボックス
                .Columns(1).ReadOnly = True                                                         ' 02. 委員会名
                .Columns(2).ReadOnly = True                                                         ' 03. 適用年月
                .Columns(3).ReadOnly = True                                                         ' 04. 登録日時
                .Columns(4).ReadOnly = True                                                         ' 05. 登録者
                .Columns(5).ReadOnly = True                                                         ' 06. 状態
                .Columns(6).ReadOnly = True                                                         ' 07. 委員会変更ID
                .Columns(7).ReadOnly = True                                                         ' 08. 帳票出力
                .Columns(8).ReadOnly = True                                                         ' 09. 委員会ID
                .Columns(9).ReadOnly = True                                                         ' 10. 変更者ID
                ' カラム並び替え禁止
                .Columns(0).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 01. チェックボックス
                .Columns(1).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 02. 委員会名
                .Columns(2).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 03. 適用年月
                .Columns(3).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 04. 登録日時
                .Columns(4).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 05. 登録者
                .Columns(5).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 06. 状態
                .Columns(6).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 07. 委員会変更ID
                .Columns(7).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 08. 帳票出力
                .Columns(8).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 09. 委員会ID
                .Columns(9).SortMode = DataGridViewColumnSortMode.NotSortable                       ' 10. 変更者ID

            End With

            ' 戻り値格納
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/09(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/09(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス
        Dim strSql As String = ""           ' SQL

        Try
            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------------------------------
            '   発信文書件名情報取得処理
            '-------------------------------------------------------------------------------
            If Me.GetDocumentSubjectInfo(clsDb) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   期情報取得処理
            '-------------------------------------------------------------------------------
            If Me.GetPeriodInfo(clsDb) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   期コンボボックス
            '-------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_name      AS DisplayName" & vbCrLf
            strSql = strSql & "       ,a.c_period_id AS ValueName" & vbCrLf
            strSql = strSql & "   FROM period AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb,
                                          Me.cboPeriod,
                                          strSql,
                                          "DisplayName",
                                          "ValueName",
                                          False) = False Then
                Return False
            End If

            '-------------------------------------------------------------------------------
            '   管理コードコンボボックス作成
            '-------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_name     AS DisplayName" & vbCrLf
            strSql = strSql & "       ,a.c_doc_code AS ValueName" & vbCrLf
            strSql = strSql & "   FROM document_code_master AS a" & vbCrLf
            strSql = strSql & "  WHERE a.d_from <= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "    AND a.d_to >= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "  ORDER BY a.c_doc_code" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb,
                                          Me.cboDocCode,
                                          strSql,
                                          "DisplayName",
                                          "ValueName",
                                          False) = False Then
                Return False
            End If

            '-------------------------------------------------------------------------------
            '   標題コンボボックス作成処理
            '-------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.l_subject     AS DisplayName" & vbCrLf
            strSql = strSql & "       ,a.s_subject_seq AS ValueName" & vbCrLf
            strSql = strSql & "   FROM document_subject AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_doc_code = '" & Me.cboDocCode.SelectedValue.ToString() & "'" & vbCrLf
            strSql = strSql & "    AND a.d_from <= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "    AND a.d_to >= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "  ORDER BY a.s_subject_seq" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb,
                                          Me.cboSubject,
                                          strSql,
                                          "DisplayName",
                                          "ValueName",
                                          False) = False Then
                Return False
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetSearchData
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>検索データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSearchData() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strSql As String = ""                       ' SQL文
        Dim clsDb As New CLAccessMdb                    ' データベースクラス
        Dim tbRet As DataTable = Nothing                ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                    ' 検索結果件数

        ' 検索項目
        Dim strPeriod As String = ""                    ' 期
        Dim strApplyDate As String = ""                 ' 適用日付
        Dim strManageCode As String = ""                ' 管理コード
        Dim strSubject As String = ""                   ' 標題
        Dim strFrom As String = ""                      ' 登録年月日From
        Dim strTo As String = ""                        ' 登録年月日To

        Try
            '-------------------------------------------------------------------
            '   検索項目
            '-------------------------------------------------------------------
            ' 期
            If MDChk.ChkNull(Me.cboPeriod.SelectedValue.ToString()) = False Then
                strPeriod = Me.cboPeriod.SelectedValue.ToString()
            End If
            ' 適用日付
            Me.dtpApplyDate.Value.Date.ToString("yyyyMMdd")
            ' 管理コード
            If MDChk.ChkNull(Me.cboDocCode.SelectedValue.ToString()) = False Then
                strManageCode = Me.cboDocCode.SelectedValue.ToString()
            End If
            ' 標題
            If MDChk.ChkNull(Me.cboSubject.SelectedValue.ToString()) = False Then
                strSubject = Me.cboSubject.SelectedValue.ToString()
            End If
            ' 登録年月日From
            strFrom = Me.dtpFrom.Value.Date.ToString("yyyyMMdd")
            ' 登録年月日To
            strTo = Me.dtpTo.Value.Date.ToString("yyyyMMdd")

            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT join1.c_staf_id       AS c_staf_id" & vbCrLf         ' 01. 社員番号
            strSql = strSql & "       ,join1.l_name          AS l_name" & vbCrLf            ' 02. 名前
            strSql = strSql & "       ,join1.l_name_kna      AS l_name_kna" & vbCrLf        ' 03. 名前カナ
            strSql = strSql & "       ,join1.k_belonging     AS k_belonging" & vbCrLf       ' 03. 組合支部
            strSql = strSql & "       ,join1.k_staf_kind     AS k_staf_kind" & vbCrLf       ' 04. 組合員種別
            strSql = strSql & "       ,join1.k_model         AS k_model" & vbCrLf           ' 05. 機種
            strSql = strSql & "       ,join1.k_qualification AS k_qualification" & vbCrLf   ' 06. 資格
            strSql = strSql & "       ,join1.k_user_status   AS k_user_status" & vbCrLf     ' 07. スタータス
            strSql = strSql & "       ,join1.k_local         AS k_local" & vbCrLf           ' 08. 会社所属
            'strSql = strSql & "       ,join1.n_ksh           AS n_ksh" & vbCrLf             ' 08. 会社所属
            strSql = strSql & "       ,join1.d_join          AS d_join" & vbCrLf            ' 09. 加入年月日
            strSql = strSql & "       ,join1.d_from          AS d_from" & vbCrLf            ' 10. 適用日付
            strSql = strSql & "       ,join1.c_user_id       AS c_user_id" & vbCrLf         ' 11. 個人認証ID（隠しカラム）
            strSql = strSql & "       ,join1.c_ksh           AS c_ksh" & vbCrLf             ' 12. 会社コード（隠しカラム）
            strSql = strSql & "       ,IIF(((join2.c_user_id <> '') AND (join2.c_user_id IS NOT NULL)), 'True', 'False') as seniorFlg" & vbCrLf
            strSql = strSql & "   FROM ( SELECT stat2.c_staf_id  AS c_staf_id" & vbCrLf
            strSql = strSql & "                ,stat2.l_name     AS l_name" & vbCrLf
            strSql = strSql & "                ,stat2.l_name_kna AS l_name_kna" & vbCrLf
            strSql = strSql & "                ,ctd1.l_name      AS k_belonging" & vbCrLf
            strSql = strSql & "                ,ctd2.l_name      AS k_staf_kind" & vbCrLf
            strSql = strSql & "                ,ctd3.l_name      AS k_model" & vbCrLf
            strSql = strSql & "                ,ctd4.l_name      AS k_qualification" & vbCrLf
            strSql = strSql & "                ,ctd5.l_name      AS k_user_status" & vbCrLf
            strSql = strSql & "                ,ctd0.l_name      AS k_local" & vbCrLf
            'strSql = strSql & "                ,ksh.n_ksh        AS n_ksh" & vbCrLf
            strSql = strSql & "                ,stat2.d_join     AS d_join" & vbCrLf
            strSql = strSql & "                ,stat2.d_from     AS d_from" & vbCrLf
            strSql = strSql & "                ,stat2.c_user_id  AS c_user_id" & vbCrLf
            strSql = strSql & "                ,stat2.c_ksh      AS c_ksh" & vbCrLf
            strSql = strSql & "            FROM staf_attribute AS stat2" & vbCrLf
            strSql = strSql & "                ,( SELECT stat1.c_user_id" & vbCrLf
            strSql = strSql & "                         ,MAX(stat1.d_from ) AS c_d_from" & vbCrLf
            strSql = strSql & "                     FROM staf_attribute AS stat1" & vbCrLf
            strSql = strSql & "                    WHERE stat1.k_del = '0'" & vbCrLf
            strSql = strSql & "                    GROUP BY stat1.c_user_id" & vbCrLf
            strSql = strSql & "                    ORDER BY stat1.c_user_id ) AS stat3" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd0" & vbCrLf
            'strSql = strSql & "                ,ksh" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd1" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd2" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd3" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd4" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd5" & vbCrLf
            strSql = strSql & "           WHERE stat2.c_user_id = stat3.c_user_id" & vbCrLf
            strSql = strSql & "             AND stat2.d_from    = stat3.c_d_from " & vbCrLf
            strSql = strSql & "             AND stat2.c_ksh     = '" & MDLoginInfo.Ksh & "'" & vbCrLf
            strSql = strSql & "             AND stat2.k_del     = '0'" & vbCrLf
            strSql = strSql & "             AND ctd1.c_constant = 'BELONGING'" & vbCrLf
            strSql = strSql & "             AND ctd1.c_constant_seq = stat2.k_belonging" & vbCrLf
            strSql = strSql & "             AND ctd2.c_constant = 'STAF_KIND'" & vbCrLf
            strSql = strSql & "             AND ctd2.c_constant_seq = stat2.k_staf_kind" & vbCrLf
            strSql = strSql & "             AND ctd3.c_constant = 'MODEL'" & vbCrLf
            strSql = strSql & "             AND ctd3.c_constant_seq = stat2.k_model" & vbCrLf
            strSql = strSql & "             AND ctd4.c_constant = 'QUALIFICATION'" & vbCrLf
            strSql = strSql & "             AND ctd4.c_constant_seq = stat2.k_qualification" & vbCrLf
            strSql = strSql & "             AND ctd5.c_constant = 'USER_STATUS'" & vbCrLf
            strSql = strSql & "             AND ctd5.c_constant_seq = stat2.k_user_status" & vbCrLf
            strSql = strSql & "             AND ctd0.c_constant = 'AREA_LOCAL'" & vbCrLf
            strSql = strSql & "             AND ctd0.c_constant_seq = stat2.k_local ) AS join1" & vbCrLf
            'strSql = strSql & "             AND stat2.c_ksh = ksh.c_ksh ) AS join1" & vbCrLf
            strSql = strSql & "        LEFT JOIN ( SELECT stat4.c_user_id,stat4.c_staf_id_old" & vbCrLf
            strSql = strSql & "                      FROM staf_attribute stat4" & vbCrLf
            strSql = strSql & "                     WHERE stat4.c_staf_id_old <> stat4.c_user_id" & vbCrLf
            strSql = strSql & "                       AND stat4.c_staf_id_old <> '' ) join2" & vbCrLf
            strSql = strSql & "        ON join1.c_user_id = join2.c_staf_id_old" & vbCrLf
            ' ORDER BY句（社員番号で並替）
            strSql = strSql & "  ORDER BY CLng(join1.c_user_id)" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' 件数チェック
            If intRetCnt > 0 Then
                ' 1件以上の処理

                ' 縦総数設定
                Me.dgvResult.RowCount = intRetCnt

                ' レコード数分ループ
                For i = 0 To intRetCnt - 1
                    ' データ設定
                    With Me.dgvResult.Rows(i).Cells
                        .Item(0).Value = NVL(tbRet.Rows(i).Item("c_staf_id"))                           ' 01. 社員番号
                        .Item(1).Value = NVL(tbRet.Rows(i).Item("l_name"))                              ' 02. 名前
                        .Item(2).Value = NVL(tbRet.Rows(i).Item("k_belonging"))                         ' 03. 組合支部
                        .Item(3).Value = NVL(tbRet.Rows(i).Item("k_staf_kind"))                         ' 04. 組合員種
                        .Item(4).Value = NVL(tbRet.Rows(i).Item("k_model"))                             ' 05. 機種
                        .Item(5).Value = NVL(tbRet.Rows(i).Item("k_qualification"))                     ' 06. 資格
                        .Item(6).Value = NVL(tbRet.Rows(i).Item("k_user_status"))                       ' 07. スタータス
                        .Item(7).Value = NVL(tbRet.Rows(i).Item("k_local"))                             ' 08. 会社所属
                        '.Item(7).Value = NVL(tbRet.Rows(i).Item("n_ksh"))                               ' 08. 会社所属
                        ' 09. 加入年月日
                        If IsDBNull(tbRet.Rows(i).Item("d_join")) Then
                            .Item(8).Value = ""
                        Else
                            .Item(8).Value = Format(tbRet.Rows(i).Item("d_join"), "yyyy/MM/dd")
                        End If
                        ' 10. 適用日付
                        If IsDBNull(tbRet.Rows(i).Item("d_from")) Then
                            .Item(9).Value = ""
                        Else
                            .Item(9).Value = Date.Parse(Format(CInt(tbRet.Rows(i).Item("d_from").ToString()), "0000/00/00")).ToString("yyyy/MM/dd")
                        End If
                        .Item(10).Value = NVL(tbRet.Rows(i).Item("c_user_id"))                          ' 11. 個人認証ID（隠しカラム）
                        .Item(11).Value = NVL(tbRet.Rows(i).Item("c_ksh"))                              ' 12. 会社コード（隠しカラム）
                        .Item(12).Value = NVL(tbRet.Rows(i).Item("seniorFlg"))                          ' 13. シニア前情報フラグ

                        ' 未来日チェック
                        If tbRet.Rows(i).Item("d_from").ToString() > Now.ToString("yyyyMMdd") Then
                            ' 適用日付が未来日の場合
                            For j = 0 To 11
                                Me.dgvResult(j, i).Style.BackColor = Color.LightGreen       ' バックカラーライトグリーン
                            Next
                        Else
                            ' 適用日付が未来日ではない場合
                            For j = 0 To 11
                                Me.dgvResult(j, i).Style.BackColor = Color.White            ' バックカラー白色設定
                            Next
                        End If
                    End With
                Next
            Else
                ' 0件の処理

                ' 対象データなしメッセージボックス表示
                Call CLMsg.Show("DI0001")

            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetCheckAll
    '   名称　：チェックオン・オフ処理
    '   概要  ：
    '   引数　：ByVal iCheckFlg As Boolean = True：全チェック, False：全チェック解除
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function SetCheckAll(ByVal iCheckFlg As Boolean) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            With Me.dgvResult
                If .Rows.Count > 0 Then                         ' データが1件以上ある場合
                    For i = 0 To .Rows.Count - 1                ' 件数分ループ
                        .Rows(i).Cells(0).Value = iCheckFlg     ' 全チェック or 全チェック解除
                    Next
                End If
            End With

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateSubject
    '   名称　：標題コンボボックス作成処理
    '   概要  ：
    '   引数　：ByVal iClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function CreateComboBoxSubject(ByVal iClsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文
        'Dim strPeriod As String = ""        ' 期（全角数値）

        Try
            '' 期（全角数値）取得
            'strPeriod = Me.cboPeriod.Text.Replace("第", "").Replace("期", "")

            ' 標題コンボボックスクリア
            Me.cboSubject.DataSource = Nothing

            ' データベース接続
            Call iClsDb.Connect()

            ' SQL文作成
            strSql = "" & vbCrLf
            'strSql = strSql & " SELECT REPLACE(a.l_subject, '○', '" & strPeriod & "') AS DisplayName" & vbCrLf
            strSql = strSql & " SELECT a.l_subject     AS DisplayName" & vbCrLf
            strSql = strSql & "       ,a.s_subject_seq AS ValueName" & vbCrLf
            strSql = strSql & "   FROM document_subject AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_doc_code = '" & Me.cboDocCode.SelectedValue.ToString() & "'" & vbCrLf
            strSql = strSql & "    AND a.d_from <= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "    AND a.d_to >= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "  ORDER BY a.s_subject_seq" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(iClsDb,
                                          Me.cboSubject,
                                          strSql,
                                          "DisplayName",
                                          "ValueName",
                                          False) = False Then
                Return blnRet
            End If

            ' 発信文書件名情報取得処理
            If Me.GetDocumentSubjectInfo(iClsDb,
                                         Me.cboDocCode.SelectedValue.ToString()) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetDocumentSubjectInfo
    '   名称　：発信文書件名情報取得処理
    '   概要  ：
    '   引数　：ByVal iClsDb                  As CLAccessMdb = データベースクラス,
    '           Optional ByVal iStrManageCode As String      = 管理コード
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>発信文書件名情報取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrManageCode">管理コード</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetDocumentSubjectInfo(ByVal iClsDb As CLAccessMdb,
                                            Optional ByVal iStrManageCode As String = "") As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim dtRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRet As Integer = 0                   ' 処理件数

        Try
            ' モージュールレベルデータテーブル変数クリア
            Me.m_dtDocumentSubjectInfo = Nothing

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_doc_code" & vbCrLf                               ' 管理コード
            strSql = strSql & "       ,a.c_template" & vbCrLf                               ' テンプレート区分
            strSql = strSql & "       ,a.s_subject_seq" & vbCrLf                            ' 標題枝番
            strSql = strSql & "       ,a.d_from" & vbCrLf                                   ' 適用開始年月日
            strSql = strSql & "       ,a.d_to" & vbCrLf                                     ' 適用終了年月日
            strSql = strSql & "       ,a.l_subject" & vbCrLf                                ' 標題
            strSql = strSql & "       ,a.k_details" & vbCrLf                                ' 詳細設定分類
            strSql = strSql & "       ,a.l_biko" & vbCrLf                                   ' 備考
            strSql = strSql & "       ,a.d_ins" & vbCrLf                                    ' 作成日
            strSql = strSql & "       ,a.c_user_id_ins" & vbCrLf                            ' 作成者個人ＩＤ
            strSql = strSql & "   FROM document_subject AS a" & vbCrLf                      ' 発信文書件名
            strSql = strSql & " WHERE a.d_from <= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "   AND a.d_to >= '" & Format(Now, "yyyyMMdd") & "'" & vbCrLf
            If iStrManageCode.Length <> 0 Then
                ' 管理コードと同じもの
                strSql = strSql & "  AND a.c_doc_code = '" & iStrManageCode & "'" & vbCrLf
            End If
            strSql = strSql & "  ORDER BY a.c_doc_code" & vbCrLf                            ' 管理コード,標題枝番で並び替え
            strSql = strSql & "          ,a.s_subject_seq" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' モジュール変数にデータ確保（詳細設定分類取得用）
            Me.m_dtDocumentSubjectInfo = dtRet

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetPeriodInfo
    '   名称　：期情報取得処理
    '   概要  ：
    '   引数　：ByVal iClsDb                As CLAccessMdb = データベースクラス,
    '           Optional ByVal iStrPeriodId As String      = 期ID
    '   作成日：2012/03/17(土)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/17(土)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>期情報取得処理</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <param name="iStrPeriodId">期ID</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetPeriodInfo(ByVal iClsDb As CLAccessMdb,
                                   Optional ByVal iStrPeriodId As String = "") As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim dtRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRet As Integer = 0                   ' 処理件数
        Dim strNow As String = ""                   ' 現在日付

        Try
            ' モージュールレベルデータテーブル変数クリア
            Me.m_dtPeriodInfo = Nothing

            ' 現在日付取得
            strNow = Now.ToString("yyyyMMdd")

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_period_id" & vbCrLf          ' 01. 期ID
            strSql = strSql & "       ,a.d_from" & vbCrLf               ' 02. 適用開始年月日
            strSql = strSql & "       ,a.d_to" & vbCrLf                 ' 03. 適用終了年月日
            strSql = strSql & "       ,a.c_ksh" & vbCrLf                ' 04. 会社コード
            strSql = strSql & "       ,a.k_period_kind" & vbCrLf        ' 05. 期区分
            strSql = strSql & "       ,a.l_name" & vbCrLf               ' 06. 名称
            strSql = strSql & "       ,a.l_omission_name" & vbCrLf      ' 07. 略名称
            strSql = strSql & "       ,a.l_biko" & vbCrLf               ' 08. 備考
            strSql = strSql & "       ,a.d_ins" & vbCrLf                ' 09. 作成日
            strSql = strSql & "       ,a.c_user_id_ins" & vbCrLf        ' 10. 作成者個人ID
            strSql = strSql & "   FROM period AS a" & vbCrLf            ' 期マスタ
            strSql = strSql & "  WHERE a.c_ksh       = '" & MDLoginInfo.Ksh & "'" & vbCrLf  ' 会社コードと同じもの
            If iStrPeriodId.Length <> 0 Then
                ' 期IDと同じもの
                strSql = strSql & "    AND a.c_period_id = '" & iStrPeriodId & "'" & vbCrLf
            Else
                strSql = strSql & "    AND a.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            End If
            strSql = strSql & "  ORDER BY a.c_period_id" & vbCrLf                           ' 期IDで並び替え
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' モジュール変数にデータ確保
            Me.m_dtPeriodInfo = dtRet

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number,
                                   Err.Description,
                                   SCREEN_ID, SCREEN_NAME,
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：MainInsert
    '   名称　：作成メイン処理
    '   概要  ：作成メイン処理
    '   引数　：True = 正常, False = 異常
    '   戻り値：
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>作成メイン処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function MainInsert() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            '-------------------------------------------------------
            '   テンプレート情報取得処理
            ' 　１．テンプレート区分
            ' 　２．テンプレートファイル名（ファイル名のみ）
            ' 　３．テンプレートファイル名（フルパス）
            '-------------------------------------------------------
            If Me.GetTemplateInfo() = False Then
                Return blnRet
            End If

            ' 標題枝番
            Me.intSubjectSeq = CInt(Me.m_dtDocumentSubjectInfo.Rows(Me.cboSubject.SelectedIndex).Item(2))

            ' 期
            Me.strPeriodId = Me.cboPeriod.SelectedValue.ToString()

            ' 管理コード
            Me.strDocCode = Me.cboDocCode.SelectedValue.ToString()

            ' 標題
            If Me.txtSubjectManual.Text.Trim().Length = 0 Then
                Me.strSubject = Me.cboSubject.Text                      ' 任意入力欄が入力されていなければ、表題取得
            Else
                Me.strSubject = Me.txtSubjectManual.Text.Trim()         ' 入力されていれば、任意入力欄を表題として取得

            End If

            ' 適用日付
            Me.strApplyDate = Me.dtpApplyDate.Value.Date.ToString("yyyyMMdd")

            '-----------------------------------------------------------
            '   委員会変更ID・帳票出力
            '-----------------------------------------------------------
            ' 詳細設定分類判定
            If Me.strDetails = DETAILS_NORMAL Then
                '=======================================================
                '   標準
                '=======================================================
                Me.strCommitteeUpdate() = Nothing                       ' 委員会変更ID
                Me.strDocumentOut() = Nothing                           ' 帳票出力

            ElseIf Me.strDetails = DETAILS_ADDDEL Then
                '=======================================================
                '   追加・削除
                '=======================================================
                With Me.dgvResult
                    If .RowCount >= 0 Then                              ' 0件以上
                        ' チェックされているデータ件数取得
                        Dim intChk As Integer = 0
                        For i = 0 To .Rows.Count - 1
                            If .Rows(i).Cells.Item(0).Value Then
                                intChk = intChk + 1
                            End If
                        Next
                        If intChk > 0 Then
                            ' チェック件数分配列再生成
                            ReDim Me.strCommitteeUpdate(intChk - 1)     ' 委員会変更ID
                            ReDim Me.strDocumentOut(intChk - 1)         ' 帳票出力
                            ReDim Me.strCommitteeDFrom(intChk - 1)      ' 委員会適用日付
                            Dim j As Integer = 0
                            For k = 0 To .RowCount - 1
                                If .Rows(k).Cells.Item(0).Value Then
                                    ' 委員会変更ID
                                    Me.strCommitteeUpdate(j) = .Rows(k).Cells.Item(6).Value.ToString()
                                    ' 帳票出力
                                    Me.strDocumentOut(j) = .Rows(k).Cells.Item(7).Value.ToString()
                                    ' 委員会適用日付
                                    Me.strCommitteeDFrom(j) = .Rows(k).Cells.Item(2).Value.ToString().Replace("/", "")
                                    j = j + 1
                                End If
                            Next
                        End If
                    Else                                                ' 0件
                        Me.strCommitteeUpdate() = Nothing               ' 委員会変更ID
                        Me.strDocumentOut() = Nothing                   ' 帳票出力
                        Me.strCommitteeDFrom() = Nothing                ' 委員会適用日付
                    End If
                End With

            ElseIf Me.strDetails = DETAILS_CHANGE Then
                '=======================================================
                '   部長、委員長の交代
                '=======================================================
                With Me.dgvResult
                    If .SelectedRows.Count > 0 Then
                        ReDim Me.strCommitteeUpdate(0)                  ' 委員会変更ID
                        ReDim Me.strDocumentOut(0)                      ' 帳票出力
                        ReDim Me.strCommitteeDFrom(0)                   ' 委員会適用日付
                        ' 委員会変更ID
                        Me.strCommitteeUpdate(0) = .SelectedRows(0).Cells.Item(6).Value.ToString()
                        ' 帳票出力
                        Me.strDocumentOut(0) = .SelectedRows(0).Cells.Item(7).Value.ToString()
                        ' 委員会適用日付
                        Me.strCommitteeDFrom(0) = .SelectedRows(0).Cells.Item(2).Value.ToString().Replace("/", "")
                    Else
                        Me.strCommitteeUpdate() = Nothing               ' 委員会変更ID
                        Me.strDocumentOut() = Nothing                   ' 帳票出力
                        Me.strCommitteeDFrom() = Nothing                ' 委員会適用日付
                    End If
                End With
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetTemplateInfo
    '   名称　：テンプレート情報取得処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>テンプレート情報取得</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetTemplateInfo() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsDb As CLAccessMdb = New CLAccessMdb  ' データベースクラス
        Dim strSql As String = ""                   ' SQL
        Dim dtRet As DataTable                      ' 処理結果格納データテーブル
        Dim intRet As Integer = 0                   ' 処理結果件数
        Dim strDocCode As String = ""               ' 管理コード
        Dim strApplyDate As String = ""             ' 適用日付
        Dim intSubjectSeq As Integer = 0            ' 標題枝番

        Try
            ' 各データ取得
            strDocCode = Me.cboDocCode.SelectedValue.ToString()
            strApplyDate = Me.dtpApplyDate.Value.Date.ToString("yyyyMMdd")
            intSubjectSeq = Int(Me.m_dtDocumentSubjectInfo.Rows(Me.cboSubject.SelectedIndex).Item(2))

            ' データベース接続
            Call clsDb.Connect()

            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_template" & vbCrLf                               ' テンプレート区分
            strSql = strSql & "       ,a.l_file" & vbCrLf                                   ' ファイル名
            strSql = strSql & "       ,a.s_doc_id" & vbCrLf                                 ' 文書ID（ファイルフルパス）
            ' 適用日付内の発信文書件名情報取得
            strSql = strSql & "   FROM (" & vbCrLf
            strSql = strSql & "          SELECT c.k_extension                    " & vbCrLf ' 拡張子種別
            strSql = strSql & "                ,c.l_file                         " & vbCrLf ' ファイル名
            strSql = strSql & "                ,c.c_template                     " & vbCrLf ' テンプレート区分
            strSql = strSql & "                ,c.s_doc_id                       " & vbCrLf ' 文書ID（ファイルフルパス）
            strSql = strSql & "            FROM dispatch_template AS c           " & vbCrLf ' 発信文書件名情報
            strSql = strSql & "           WHERE c.d_from < '" & strApplyDate & "'" & vbCrLf ' 適用日付Fromより大きいもの
            strSql = strSql & "             AND c.d_to > '" & strApplyDate & "'  " & vbCrLf ' 適用日付Toより小さいもの
            strSql = strSql & "        ) AS a" & vbCrLf
            ' 適用日付内の発信テンプレート情報取得
            strSql = strSql & "       ,( SELECT d.c_doc_code                     " & vbCrLf ' 管理コード
            strSql = strSql & "                ,d.c_template                     " & vbCrLf ' テンプレート区分
            strSql = strSql & "                ,d.s_subject_seq                  " & vbCrLf ' 標題枝番
            strSql = strSql & "            FROM document_subject AS d            " & vbCrLf ' 発信テンプレート情報
            strSql = strSql & "           WHERE d.d_from < '" & strApplyDate & "'" & vbCrLf ' 適用日付Fromより大きいもの
            strSql = strSql & "             AND d.d_to > '" & strApplyDate & "'  " & vbCrLf ' 適用日付Toより小さいもの
            strSql = strSql & "        ) AS b" & vbCrLf

            strSql = strSql & "  WHERE a.k_extension = 'TPL_XLS'                 " & vbCrLf ' 拡張子種別が 'TPL_XLS' のもの
            strSql = strSql & "    AND a.c_template = b.c_template               " & vbCrLf ' 発信文書件名と発信テンプレートのテンプレート区分が同じもの
            strSql = strSql & "    AND b.c_doc_code = '" & strDocCode & "'       " & vbCrLf ' 管理コードと同じもの
            strSql = strSql & "    AND b.s_subject_seq = " & intSubjectSeq & vbCrLf         ' 標題枝番と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 処理結果件数取得
            intRet = dtRet.Rows.Count

            ' 0件チェック
            If intRet <> 1 Then
                Call CLMsg.Show("DE0013", "テンプレート")
                Return blnRet
            End If

            ' テンプレート情報設定
            Me.strTemplate = dtRet.Rows(0).Item(0).ToString()           ' テンプレート区分
            Me.strTemplateFile = dtRet.Rows(0).Item(1).ToString()       ' テンプレートファイル名（ファイル名のみ）
            Me.strTemplateFileFull = dtRet.Rows(0).Item(2).ToString()   ' テンプレートファイル名（フルパス）

            ' 処理結果に正常を設定
            blnRet = True

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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChangeSubject
    '   名称　：標題チェンジ処理
    '   概要  ：
    '   引数　：ByVal iStrDetails As String = 詳細設定分類（"0"：標準,
    '                                                       "1"：追加・削除,
    '                                                       "2"：部長、委員長の交代）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function ChangeSubject(ByVal iStrDetails As String) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            ' 任意入力欄
            If Me.cboSubject.Text = MANUAL_INSERT Then
                ' "↓任意入力．．．" の場合、使用可
                Me.txtSubjectManual.Enabled = True
                Me.txtSubjectManual.BackColor = Color.White
                Me.lblSubjectManual.Enabled = True
            Else
                ' "↓任意入力．．．" 以外の場合、使用不可
                Me.txtSubjectManual.Enabled = False
                Me.txtSubjectManual.Text = ""
                Me.txtSubjectManual.BackColor = SystemColors.Control
                Me.lblSubjectManual.Enabled = False
            End If

            ' 詳細設定分類判定
            If iStrDetails = DETAILS_NORMAL Then
                '=======================================================
                '   "0"：標準
                '=======================================================
                ' GroupBox
                Me.grpResult.Enabled = False                            ' 検索結果
                Me.grpResult.Text = "追加・削除データ一覧"              ' テキスト
                ' Label
                Me.lblFrom.Enabled = False                              ' 登録年月日From
                Me.lblTo.Enabled = False                                ' 登録年月日To
                ' DateTimePicker
                Me.dtpFrom.Enabled = False                              ' 登録年月日From
                Me.dtpTo.Enabled = False                                ' 登録年月日To
                ' Button
                Me.btnSearch.Visible = True                             ' 検索
                Me.btnSearch.Enabled = False

                ' データグリッドカラム表示有無
                For i = 0 To 5
                    Me.dgvResult.Columns(i).Visible = True              ' 各カラム（表示）
                Next
                For j = 6 To 9
                    Me.dgvResult.Columns(j).Visible = False             ' 各カラム（非表示）
                Next
                '複数行選択不可
                Me.dgvResult.MultiSelect = False

                ' Button
                Me.btnSelectCancel.Visible = False                      ' 選択を解除
                Me.btnAllCheck.Visible = False                          ' 全チェック
                Me.btnAllNoCheck.Visible = False                        ' 全チェック解除
                Me.btnDelete.Visible = True                             ' 削除
                Me.btnDelete.Enabled = False
                Me.btnCreate.Visible = True                             ' 作成
                Me.btnCreate.Enabled = True
                Me.btnCancel.Visible = True                             ' キャンセル
                Me.btnCancel.Enabled = True

            ElseIf iStrDetails = DETAILS_ADDDEL Then
                '=======================================================
                '   "1"：追加・削除
                '=======================================================
                ' GroupBox
                Me.grpResult.Enabled = True                             ' 検索結果
                Me.grpResult.Text = "追加・削除データ一覧"              ' テキスト
                ' Label
                Me.lblFrom.Enabled = True                               ' 登録年月日From
                Me.lblTo.Enabled = True                                 ' 登録年月日To
                ' DateTimePicker
                Me.dtpFrom.Enabled = True                               ' 登録年月日From
                Me.dtpTo.Enabled = True                                 ' 登録年月日To
                ' Button
                Me.btnSearch.Visible = True                             ' 検索
                Me.btnSearch.Enabled = True

                ' データグリッドカラム表示有無
                For i = 0 To 5
                    Me.dgvResult.Columns(i).Visible = True              ' 各カラム（表示）
                Next
                For j = 6 To 9
                    Me.dgvResult.Columns(j).Visible = False             ' 各カラム（非表示）
                Next
                '複数行選択可
                Me.dgvResult.MultiSelect = True

                ' Button
                Me.btnSelectCancel.Visible = False                      ' 選択を解除
                Me.btnAllCheck.Visible = True                           ' 全チェック
                Me.btnAllNoCheck.Visible = True                         ' 全チェック解除
                Me.btnDelete.Visible = True                             ' 削除
                Me.btnDelete.Enabled = True
                Me.btnCreate.Visible = True                             ' 作成
                Me.btnCreate.Enabled = True
                Me.btnCancel.Visible = True                             ' キャンセル
                Me.btnCancel.Enabled = True

            ElseIf iStrDetails = DETAILS_CHANGE Then
                '=======================================================
                '   "2"：部長、委員長の交代
                '=======================================================
                ' GroupBox
                Me.grpResult.Enabled = True                             ' 検索結果
                Me.grpResult.Text = "変更データ一覧"                    ' テキスト
                ' Label
                Me.lblFrom.Enabled = True                               ' 登録年月日From
                Me.lblTo.Enabled = True                                 ' 登録年月日To
                ' DateTimePicker
                Me.dtpFrom.Enabled = True                               ' 登録年月日From
                Me.dtpTo.Enabled = True                                 ' 登録年月日To
                ' Button
                Me.btnSearch.Visible = True                             ' 検索
                Me.btnSearch.Enabled = True

                ' データグリッドカラム表示有無
                Me.dgvResult.Columns(0).Visible = False                 ' チェックボックス（非表示）
                For i = 1 To 4
                    Me.dgvResult.Columns(i).Visible = True              ' 各表示カラム（委員会名,適用年月,登録日時,登録者）
                Next
                For j = 5 To 9
                    Me.dgvResult.Columns(j).Visible = False             ' 各非表示カラム（状態,委員会変更ID,出力,委員会番号,変更ID）
                Next
                '複数行選択不可
                Me.dgvResult.MultiSelect = False

                ' Button
                Me.btnSelectCancel.Visible = True                       ' 選択を解除
                Me.btnAllCheck.Visible = False                          ' 全チェック
                Me.btnAllNoCheck.Visible = False                        ' 全チェック解除
                Me.btnDelete.Visible = True                             ' 削除
                Me.btnDelete.Enabled = True
                Me.btnCreate.Visible = True                             ' 作成
                Me.btnCreate.Enabled = True
                Me.btnCancel.Visible = True                             ' キャンセル
                Me.btnCancel.Enabled = True

            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：MainSearch
    '   名称　：検索メイン処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function MainSearch() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim dtRet As DataTable = Nothing    ' 処理結果格納データテーブル

        Try
            ' 一覧クリア
            Me.dgvResult.Rows.Clear()

            ' 委員会検索結果取得処理
            If GetSelectCommitteeResult(dtRet) = False Then
                Return blnRet
            End If

            ' データグリッドビュー値表示処理
            If Me.strDetails = DETAILS_ADDDEL Then
                '-----------------------------------------------------------
                '   標題：追加・削除
                '-----------------------------------------------------------
                ' データ件数チェック
                If dtRet.Rows.Count > 0 Then
                    '=======================================
                    '   データ有り
                    '=======================================
                    ' データ件数分ループ
                    For i = 0 To dtRet.Rows.Count - 1
                        Me.dgvResult.Rows.Add()
                        ' カラム数分ルーブ
                        For j = 0 To Me.dgvResult.Columns.Count - 1
                            If CON_DGV_COL_NAME(j) = "condition" _
                            AndAlso dtRet.Rows.Item(i).Item(CON_DGV_COL_NAME(j)).ToString.Length = 2 Then
                                Me.dgvResult.Rows.Item(i).Cells(CON_DGV_COL_NAME(j)).Value = dtRet.Rows.Item(i).Item(CON_DGV_COL_NAME(j)).ToString.Insert(1, "／")
                            Else
                                Me.dgvResult.Rows.Item(i).Cells(CON_DGV_COL_NAME(j)).Value = dtRet.Rows.Item(i).Item(CON_DGV_COL_NAME(j)).ToString
                            End If
                        Next
                    Next
                    '' 各ボタン表示
                    'Me.btnAllCheck.Enabled = True
                    'Me.btnAllNoCheck.Enabled = True
                Else
                    '=======================================
                    '   データ無し
                    '=======================================
                    ' 各ボタン非表示
                    'Me.btnAllCheck.Enabled = False
                    'Me.btnAllNoCheck.Enabled = False
                End If

            ElseIf Me.strDetails = DETAILS_CHANGE Then
                '-----------------------------------------------------------
                '   標題：交代
                '-----------------------------------------------------------
                If dtRet.Rows.Count > 0 Then
                    '=======================================
                    '   データ有り
                    '=======================================
                    For i = 0 To dtRet.Rows.Count - 1
                        Me.dgvResult.Rows.Add()
                        '
                        For j = 0 To Me.dgvResult.Columns.Count - 1
                            If (CON_DGV_COL_NAME(j) = "col_check" OrElse CON_DGV_COL_NAME(j) = "condition") Then
                                Continue For
                            Else
                                Me.dgvResult.Rows.Item(i).Cells(CON_DGV_COL_NAME(j)).Value = dtRet.Rows.Item(i).Item(CON_DGV_COL_NAME(j)).ToString
                            End If
                        Next
                    Next
                    ' 各ボタン表示
                    'Me.btnSelectCancel.Enabled = True
                Else
                    '=======================================
                    '   データ無し
                    '=======================================
                    ' 各ボタン非表示
                    'Me.btnSelectCancel.Enabled = False
                End If
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetSelectCommitteeResult
    '   名称　：委員会検索結果取得処理
    '   概要  ：
    '   引数　：ByRef ioDtCommitteeInfo As DataTable = 委員会検索結果データテーブル
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>委員会検索結果取得処理</summary>
    ''' <param name="ioDtCommitteeInfo">委員会検索結果データテーブル</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSelectCommitteeResult(ByRef ioDtCommitteeInfo As DataTable) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsDb As CLAccessMdb = New CLAccessMdb  ' データベースクラス
        Dim strSql As String = ""                   ' SQL
        Dim strViewName As String = ""              ' ビュー名
        Dim strAliasName As String = ""             ' エイリアス名

        Try
            If Me.strDetails = DETAILS_ADDDEL Then
                ' 追加・削除
                strViewName = "committee_add_delete_list_view"
                strAliasName = "committee_add_delete_list"
            Else
                ' 長変更
                strViewName = "committee_change_list_view"
                strAliasName = "committee_change_list"
            End If

            ' 委員会検索SQL取得処理
            If Me.GetStringSearchComSql(strViewName, _
                                        strAliasName, _
                                        strSql) = False Then
                Return blnRet
            End If

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            ioDtCommitteeInfo = clsDb.ExecuteSql(strSql)

            ' 処理結果に正常を設定
            blnRet = True

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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetStringSearchComSql
    '   名称　：委員会検索SQL取得処理
    '   概要  ：
    '   引数　：ByVal strViewName  As String = ビュー名,
    '           ByVal strAliasName As String = テーブル名（ビュー内）,
    '           ByRef ioStrSql     As String = 委員会検索SQL文
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>委員会検索SQL取得処理</summary>
    ''' <param name="strViewName">ビュー名</param>
    ''' <param name="strAliasName">テーブル名（ビュー内）</param>
    ''' <param name="ioStrSql">委員会検索SQL文</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetStringSearchComSql(ByVal strViewName As String, _
                                           ByVal strAliasName As String, _
                                           ByRef ioStrSql As String) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL
        Dim strSqlUp As String = ""         ' 登録者リスト
        Dim strSqlCom As String = ""        ' 委員会名リスト
        Dim strSqlWhere As String = ""      ' WHERE句
        Dim strSqlOrder As String = ""      ' ORDER BY句

        Try
            ' 委員会名を取得する為に必要なleftJoin句
            strSqlCom = "" & vbCrLf
            strSqlCom = strSqlCom & "LEFT JOIN ( SELECT c_committee_id, l_name" & vbCrLf
            strSqlCom = strSqlCom & "              FROM committee " & vbCrLf
            strSqlCom = strSqlCom & "             WHERE d_from <= FORMAT(GETDATE(),'yyyyMMdd') " & vbCrLf
            strSqlCom = strSqlCom & "               AND d_to >= FORMAT(GETDATE(),'yyyyMMdd') " & vbCrLf
            strSqlCom = strSqlCom & "               AND c_ksh = '" & MDLoginInfo.Ksh & "') AS committee_A " & vbCrLf
            strSqlCom = strSqlCom & "ON committee_A.c_committee_id = " & strAliasName & ".c_committee_id "
            strSqlCom = strSqlCom & ") " & vbCrLf

            ' 登録者名を取得する為に必要なleftJoin句
            strSqlUp = "" & vbCrLf
            strSqlUp = strSqlUp & "LEFT JOIN ( SELECT staf_attr.c_user_id AS c_user_id" & vbCrLf
            strSqlUp = strSqlUp & "                  ,staf_attr.l_name AS l_name" & vbCrLf
            strSqlUp = strSqlUp & "              FROM staf_attribute AS staf_attr" & vbCrLf
            strSqlUp = strSqlUp & "                  ,(SELECT c_user_id" & vbCrLf
            strSqlUp = strSqlUp & "                          ,max(d_from) AS new_from" & vbCrLf
            strSqlUp = strSqlUp & "                      FROM staf_attribute " & vbCrLf
            strSqlUp = strSqlUp & "                     WHERE d_from <= FORMAT(GETDATE(),'yyyyMMdd')" & vbCrLf
            strSqlUp = strSqlUp & "                     GROUP BY c_user_id ) AS latest_attr " & vbCrLf
            strSqlUp = strSqlUp & "             WHERE staf_attr.c_user_id = latest_attr.c_user_id " & vbCrLf
            strSqlUp = strSqlUp & "               AND staf_attr.d_from    = latest_attr.new_from" & vbCrLf
            strSqlUp = strSqlUp & "    UNION SELECT" & vbCrLf
            strSqlUp = strSqlUp & "             full_staf.c_staf_id AS user_id, " & vbCrLf
            strSqlUp = strSqlUp & "             full_staf.l_name AS l_name " & vbCrLf
            strSqlUp = strSqlUp & "    FROM full_time_staf AS full_staf," & vbCrLf
            strSqlUp = strSqlUp & "     (SELECT c_staf_id , " & vbCrLf
            strSqlUp = strSqlUp & "             max(d_from) AS new_from" & vbCrLf
            strSqlUp = strSqlUp & "      FROM full_time_staf" & vbCrLf
            strSqlUp = strSqlUp & "      WHERE d_from <= FORMAT(GETDATE(),'yyyyMMdd') " & vbCrLf
            strSqlUp = strSqlUp & "      GROUP BY c_staf_id" & vbCrLf
            strSqlUp = strSqlUp & "     ) AS latest_fulltime  " & vbCrLf
            strSqlUp = strSqlUp & "    WHERE full_staf.c_staf_id = latest_fulltime.c_staf_id " & vbCrLf
            strSqlUp = strSqlUp & "    AND full_staf.d_from = latest_fulltime.new_from " & vbCrLf
            strSqlUp = strSqlUp & "    ) attr_max" & vbCrLf
            strSqlUp = strSqlUp & "ON " & strAliasName & ".c_user_id_ins = attr_max.c_user_id" & vbCrLf

            ' WHERE句設定
            strSqlWhere = "" & vbCrLf
            strSqlWhere = strSqlWhere & " WHERE CONVERT(date, FORMAT(" & strAliasName & ".d_ins, 'yyyy/MM/dd')) BETWEEN '" & Me.dtpFrom.Value.ToString("yyyy/MM/dd") & "' AND '" & Me.dtpTo.Value.ToString("yyyy/MM/dd") & "' "
            'strSqlWhere = strSqlWhere & " AND (" & strAliasName & ".k_document_out <> '" & Me.strDetails & "1'" & vbCrLf
            'strSqlWhere = strSqlWhere & "     AND " & strAliasName & ".k_document_out <> '3' ) "

            ' ORDER BY句設定
            strSqlOrder = "" & vbCrLf
            strSqlOrder = strSqlOrder & "ORDER BY " & strAliasName & ".d_ins ," & strAliasName & ".d_from "

            If Me.strDetails = DETAILS_ADDDEL Then
                ' 標題が追加・削除の場合
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT " & vbCrLf
                strSql = strSql & " 'false' AS col_check, " & vbCrLf
                strSql = strSql & strAliasName & ".c_committee_update AS committee_update_id,  " & vbCrLf
                strSql = strSql & strAliasName & ".c_committee_id AS committee_id," & vbCrLf
                strSql = strSql & "  committee_A.l_name AS commitee_name, " & vbCrLf
                strSql = strSql & " " & UtDb.DbStrYYYYMMDDtoDateText(strAliasName & ".d_from") & " AS apply_year_month, " & vbCrLf
                strSql = strSql & strAliasName & ".k_document_out AS doc_out, " & vbCrLf
                strSql = strSql & strAliasName & ".d_ins AS update_date," & vbCrLf
                strSql = strSql & strAliasName & ".c_user_id_ins AS update_user_id," & vbCrLf
                strSql = strSql & "  attr_max.l_name AS update_user ," & vbCrLf
                strSql = strSql & "  IIF((SELECT COUNT(c_committee_update)" & vbCrLf
                strSql = strSql & "       FROM committee_update_list_dtl " & vbCrLf
                strSql = strSql & "       WHERE committee_update_list_dtl.c_committee_update = " & strAliasName & ".c_committee_update " & vbCrLf
                strSql = strSql & "       AND k_committee_insert = '0' " & vbCrLf
                strSql = strSql & "       GROUP BY c_committee_update, k_committee_insert ) > 0 , '追' , '' " & vbCrLf
                strSql = strSql & "  ) + IIF( " & vbCrLf
                strSql = strSql & "      (SELECT COUNT(c_committee_update) " & vbCrLf
                strSql = strSql & "       FROM committee_update_list_dtl " & vbCrLf
                strSql = strSql & "       WHERE committee_update_list_dtl.c_committee_update = " & strAliasName & ".c_committee_update" & vbCrLf
                strSql = strSql & "       AND k_committee_insert = '1' " & vbCrLf
                strSql = strSql & "       GROUP BY c_committee_update, k_committee_insert ) > 0 , '削' , '' " & vbCrLf
                strSql = strSql & "  ) AS condition" & vbCrLf
                strSql = strSql & "FROM (" & strViewName & " AS " & strAliasName & " " & vbCrLf
                '登録者情報取得 + 'WHERE句
                strSql = strSql & strSqlCom & strSqlUp & strSqlWhere & strSqlOrder

            ElseIf Me.strDetails = DETAILS_CHANGE Then
                ' 標題が部長、委員長の交代の場合
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT " & vbCrLf
                strSql = strSql & "  " & strAliasName & ".c_committee_update AS committee_update_id,  " & vbCrLf
                strSql = strSql & "  " & strAliasName & ".c_committee_id AS committee_id," & vbCrLf
                strSql = strSql & "  committee_A.l_name AS commitee_name, " & vbCrLf
                strSql = strSql & " " & UtDb.DbStrYYYYMMDDtoDateText(strAliasName & ".d_from") & " AS apply_year_month, " & vbCrLf
                strSql = strSql & "  " & strAliasName & ".k_document_out AS doc_out, " & vbCrLf
                strSql = strSql & "  " & strAliasName & ".d_ins AS update_date," & vbCrLf
                strSql = strSql & "  " & strAliasName & ".c_user_id_ins AS update_user_id ," & vbCrLf
                strSql = strSql & "  attr_max.l_name AS update_user " & vbCrLf
                strSql = strSql & "FROM (" & strViewName & " AS " & strAliasName & " " & vbCrLf
                strSql = strSql & strSqlCom & strSqlUp & strSqlWhere & strSqlOrder
            End If

            ' 取得した委員会検索SQL設定
            ioStrSql = strSql

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetMinimumDateFrom
    '   名称　：開始日検索結果取得処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>開始日検索結果取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetMinimumDateFrom() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim clsDb As CLAccessMdb = New CLAccessMdb      ' データベースクラス
        Dim strSql As String = ""                       ' SQL
        Dim dtRet As DataTable = New DataTable          ' 処理結果格納データテーブル
        Dim intRet As Integer = 0                       ' 処理件数

        Try
            ' データべース接続
            Call clsDb.Connect()

            ' SQL文作成
            If Me.strDetails = DETAILS_ADDDEL Then
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT MIN(a.d_ins) AS Min_Up_Date " & vbCrLf
                strSql = strSql & "   FROM committee_add_delete_list_view AS a" & vbCrLf
                strSql = strSql & "  WHERE a.k_document_out <> '" & Me.strDetails & "'" & vbCrLf
                strSql = strSql & "    AND a.k_document_out <> '3'" & vbCrLf
                strSql = strSql & ";" & vbCrLf
            Else
                strSql = "" & vbCrLf
                strSql = strSql & " SELECT MIN(a.d_ins) AS Min_Up_Date " & vbCrLf
                strSql = strSql & "   FROM committee_change_list_view AS a" & vbCrLf
                strSql = strSql & "  WHERE a.k_document_out <> '" & Me.strDetails & "'" & vbCrLf
                strSql = strSql & "    AND a.k_document_out <> '3'" & vbCrLf
                strSql = strSql & ";" & vbCrLf
            End If

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            If intRet > 0 Then
                Dim strGetDate = dtRet.Rows.Item(0).ToString()
                Me.dtpFrom.Value = Date.Parse(strGetDate)
                Me.dtpFrom.Value = Me.dtpFrom.Value
            End If

            ' 処理結果に正常を設定
            blnRet = True

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

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：MainDelete
    '   名称　：削除メイン処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>削除メイン処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function MainDelete() As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False                           ' 処理結果
        Dim checkedRowsData As DataTable = New DataTable        ' チェック付データテーブル
        Dim strAryCommitteeUpdate As String() = Nothing         ' 
        Dim strAryDocumentOut As String() = Nothing             ' 
        Dim diaRet As DialogResult = Nothing                    ' ダイアログ結果

        Try
            ' 
            If Me.strDetails = DETAILS_ADDDEL Then
                '-----------------------------------------------
                '   追加・削除
                '-----------------------------------------------
                checkedRowsData.Columns.Add("committee_update_id")
                checkedRowsData.Columns.Add("doc_out")
                For intcnt = 0 To Me.dgvResult.Rows.Count - 1
                    If Me.dgvResult.Rows.Item(intcnt).Cells(0).Value = True Then
                        checkedRowsData.Rows.Add()
                        checkedRowsData.Rows.Item(checkedRowsData.Rows.Count - 1).Item("committee_update_id") = Me.dgvResult.Rows.Item(intcnt).Cells("committee_update_id").Value
                        checkedRowsData.Rows.Item(checkedRowsData.Rows.Count - 1).Item("doc_out") = Me.dgvResult.Rows.Item(intcnt).Cells("doc_out").Value
                    End If
                Next
                ' チェック行取得
                If ((checkedRowsData Is Nothing) OrElse (checkedRowsData.Rows.Count < 1)) Then
                    Call CLMsg.Show("GE0010", "削除する行")
                    Return blnRet
                End If

                ' メッセージ表示
                diaRet = CLMsg.Show("GQ0064", "チェックがつけられた")
                If diaRet <> DialogResult.Yes Then
                    Return blnRet
                End If

                strAryCommitteeUpdate = New String(checkedRowsData.Rows.Count - 1) {}
                strAryDocumentOut = New String(checkedRowsData.Rows.Count - 1) {}
                Dim i As Integer
                For i = 0 To checkedRowsData.Rows.Count - 1
                    strAryCommitteeUpdate(i) = checkedRowsData.Rows.Item(i).Item("committee_update_id").ToString
                    strAryDocumentOut(i) = checkedRowsData.Rows.Item(i).Item("doc_out").ToString
                Next i
            Else
                '-----------------------------------------------
                '   追加・削除以外
                '-----------------------------------------------
                If Me.strDetails <> DETAILS_CHANGE Then
                    Call CLMsg.Show("GE0010", "削除する行")
                    Return blnRet
                End If
                If Me.dgvResult.SelectedRows.Count = 0 Then
                    Call CLMsg.Show("GE0010", "削除する行")
                    Return blnRet
                End If
                diaRet = CLMsg.Show("GQ0064", "選択された")
                If diaRet <> DialogResult.Yes Then
                    Return blnRet
                End If
                strAryCommitteeUpdate = New String() {Me.dgvResult.SelectedRows(0).Cells("committee_update_id").Value}
                strAryDocumentOut = New String() {Me.dgvResult.SelectedRows(0).Cells("doc_out").Value}
            End If

            Dim selectedCodeItem As String = Me.cboDocCode.SelectedValue()
            Dim nSubjectSeq As Integer = Integer.Parse(Me.cboSubject.SelectedValue())

            '---------------------------------------------------
            '   削除実行（フラグ変更）処理
            '---------------------------------------------------
            If Me.DeleteCommitteeExecute(selectedCodeItem, _
                                         nSubjectSeq, _
                                         strAryCommitteeUpdate, _
                                         strAryDocumentOut) = False Then
                Return blnRet
            End If

            '---------------------------------------------------
            '   検索メイン処理
            '---------------------------------------------------
            If Me.MainSearch() = False Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Function

    '***************************************************************************************************
    '   ＩＤ　：DeleteCommitteeExecute
    '   名称　：削除実行（フラグ変更）処理
    '   概要  ：
    '   引数　：ByVal strManageCode     As String   = ,
    '           ByVal strSubjectSeq     As String   = ,
    '           ByVal strAryUpdate      As String() = ,
    '           ByVal strAryDocumentOut As String() = 
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>削除実行（フラグ変更）処理</summary>
    ''' <param name="strManageCode"></param>
    ''' <param name="strSubjectSeq"></param>
    ''' <param name="strAryUpdate"></param>
    ''' <param name="strAryDocumentOut"></param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DeleteCommitteeExecute(ByVal strManageCode As String, _
                                            ByVal strSubjectSeq As String, _
                                            ByVal strAryUpdate As String(), _
                                            ByVal strAryDocumentOut As String()) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsDb As CLAccessMdb = New CLAccessMdb  ' データベースクラス
        Dim dtRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRet As Integer = 0                   ' 処理件数
        Dim strSql As String = ""                   ' SQL文
        Dim strVal As String = ""
        Dim strNow_DocOut As String = ""

        Try
            ' データベース接続
            Call clsDb.Connect()

            ' トランザクション開始
            Call clsDb.BeginTran()

            Try
                For i = 0 To strAryUpdate.Length - 1
                    ' とりあえず現在地を取得
                    strSql = "" & vbCrLf
                    strSql = strSql & " SELECT a.k_document_out" & vbCrLf
                    strSql = strSql & "   FROM committee_update_list AS a" & vbCrLf
                    strSql = strSql & "  WHERE a.c_committee_update = '" & strAryUpdate(i) & "'" & vbCrLf
                    strSql = strSql & ";" & vbCrLf

                    ' SQL実行
                    dtRet = clsDb.ExecuteSql(strSql)

                    ' 処理件数取得
                    intRet = dtRet.Rows.Count

                    ' 処理件数チェック
                    If intRet <= 0 Then
                        ' トランザクション取消
                        Call clsDb.RollbackTran()
                        Return blnRet
                    End If

                    strNow_DocOut = dtRet.Rows(0).Item(0).ToString
                    ' 現在値をビット計算しデータ反映
                    strVal = (strNow_DocOut Or Me.strDetails).ToString
                    strSql = "" & vbCrLf
                    strSql = strSql & " UPDATE committee_update_list AS a" & vbCrLf
                    strSql = strSql & "    SET a.k_document_out     = '" & strVal & "'" & vbCrLf
                    strSql = strSql & "  WHERE a.c_committee_update = '" & strAryUpdate(i) & "'" & vbCrLf
                    strSql = strSql & "    AND a.k_document_out     = '" & strNow_DocOut & "'" & vbCrLf
                    strSql = strSql & ";" & vbCrLf

                    ' SQL実行
                    intRet = clsDb.ExecuteNonQuery(strSql)

                    ' 処理件数チェック
                    If intRet <= 0 Then
                        ' トランザクション取消
                        Call clsDb.RollbackTran()
                        Return blnRet
                    End If

                Next

                ' トランザクション確定
                Call clsDb.CommitTran()
                blnRet = True

                Return blnRet
            Catch e As Exception
                ' トランザクション取消
                Call clsDb.RollbackTran()
            End Try

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

    End Function

    '***************************************************************************************************
    '   ＩＤ　：InitializeDataGridStyle
    '   名称　：データグリッドビュー初期化処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>データグリッドビューの初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InitializeDataGridStyle() As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            Me.dgvResult.ReadOnly = False
            For intCnt = 0 To Me.dgvResult.Columns.Count - 1
                Me.dgvResult.Columns(intCnt).Name = CON_DGV_COL_NAME(intCnt)
                Me.dgvResult.Columns(intCnt).HeaderText = CON_DGV_COL_TEXT(intCnt)
                Me.dgvResult.Columns(intCnt).Visible = CON_DGV_COL_VISIBLE_ADDDEL(intCnt)
                Me.dgvResult.Columns(intCnt).Width = CON_DGV_COL_WIDTH_ADDDEL(intCnt)
            Next

            ' 処理結果に正常を設定
            blnRet = False

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChangeDataGridStyle
    '   名称　：データグリッドビュー設定変更処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>データグリッドビューの設定変更処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChangeDataGridStyle() As Boolean

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            If Me.grpResult.Text = STR_CHANGE_LIST Then
                For intCnt = 0 To Me.dgvResult.Columns.Count - 1
                    Me.dgvResult.Columns(intCnt).Visible = CON_DGV_COL_VISIBLE_CHANGE(intCnt)
                    Me.dgvResult.Columns(intCnt).Width = CON_DGV_COL_WIDTH_CHANGE(intCnt)
                Next
            Else
                For intCnt = 0 To Me.dgvResult.Columns.Count - 1
                    If intCnt = 0 Then
                        Me.dgvResult.Columns(intCnt).ReadOnly = False
                    Else
                        Me.dgvResult.Columns(intCnt).ReadOnly = True
                    End If
                    Me.dgvResult.Columns(intCnt).Visible = CON_DGV_COL_VISIBLE_ADDDEL(intCnt)
                    Me.dgvResult.Columns(intCnt).Width = CON_DGV_COL_WIDTH_ADDDEL(intCnt)
                Next
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ResultClear
    '   名称　：検索結果クリア処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>検索結果クリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ResultClear() As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            '---------------------------------------
            '   エラー箇所クリア処理
            '---------------------------------------
            If MDCommon.ClearErr(Me) = False Then
                Exit Function
            End If

            '---------------------------------------
            '   検索結果クリア
            '---------------------------------------
            With Me.dgvResult
                If .Rows.Count > 0 Then
                    .Rows.Clear()
                End If
            End With

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要　：メッセージIDからメッセージ内容を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/13(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/13(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim arlErrMsg As New ArrayList              ' エラーメッセージリスト
        Dim clsUC999999 As UC999999 = Nothing       ' メッセージボックスクラス生成

        Try
            '-------------------------------------------------------------------------------
            '   初期化
            '-------------------------------------------------------------------------------
            ' エラー箇所クリア処理
            If MDCommon.ClearErr(Me) = False Then
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   未入力・未選択チェック
            '-------------------------------------------------------------------------------
            ' 期
            If Me.cboPeriod.SelectedIndex = -1 Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "期"))
                Call SetErr(Me.cboPeriod)
            End If
            ' 適用日付
            If Me.dtpApplyDate.Value.Date.ToString("yyyyMMdd").Length = 0 Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "適用日付"))
                Call SetErr(Me.dtpApplyDate)
            End If
            ' 管理コード
            If Me.cboDocCode.SelectedIndex = -1 Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "管理コード"))
                Call SetErr(Me.cboDocCode)
            End If
            ' 標題
            If Me.cboSubject.SelectedIndex = -1 Then
                arlErrMsg.Add(CLMsg.GetMsg("GE0006", "表題"))
                Call SetErr(Me.cboSubject)
            End If
            ' 任意入力欄（"↓任意入力．．．" の場合）
            If Me.cboSubject.Text = MANUAL_INSERT Then
                If ChkNull(Me.txtSubjectManual.Text) Then
                    arlErrMsg.Add(CLMsg.GetMsg("GE0006", "任意入力欄"))
                    Call SetErr(Me.txtSubjectManual)
                End If
            End If

            '-------------------------------------------------------------------------------
            '   複数エラーメッセージ表示画面表示
            '-------------------------------------------------------------------------------
            If Not arlErrMsg.Count = 0 Then             ' エラー存在チェック
                clsUC999999 = New UC999999              ' メッセージボックスクラス生成
                clsUC999999.errMsgList = arlErrMsg      ' プロパティ設定エラーメッセージリスト
                Call clsUC999999.ShowDialog()           ' エラーメッセージ表示画面表示
                Return blnRet                           ' 処理を抜ける
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

End Class

#End Region