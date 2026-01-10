#Region "FM040104"
'===========================================================================================================
'   クラスＩＤ　　：FM040103
'   クラス名称　　：開催登録　種別・支部選択画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.Document

Public Class FM040103

#Region "定数・変数"
    Private agoUserControl As System.Windows.Forms.UserControl
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM040103              ' FM040103
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM040103          ' 開催登録　種別・支部選択画面
    ' ステータス
    Private Const STATUS_INSERT As Byte = 1                             ' 開催登録（組合大会通知検索のみ）
    Private Const STATUS_SAME As Byte = 6                               ' 同番号開催登録（組合大会通知検索のみ）
    ' 画面変形用
    Private blnFlg As Boolean = False                                   ' 変形フラグ（True：第二段階, False：第一段階）
#End Region

    Public Sub New()
        'ここに初期処理を書く
        InitializeComponent()
    End Sub

    Public Sub New(ByVal setForm As System.Windows.Forms.UserControl)
        'ここに初期処理を書く
        InitializeComponent()
        agoUserControl = setForm
    End Sub

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM040104_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM040104_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------
            '   画面初期化処理
            '-------------------------------------------------------------------------------
            If DisplayInitialize() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnOK1_Click
    '   名称　：OKボタン1クリック処理
    '   概要　：
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnOK1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK1.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面変更処理
            If ChangeDisplay() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnOK2_Click
    '   名称　：OKボタン2処理
    '   概要　：
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnOK2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK2.Click

        Dim bytStatus As Byte

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 新規登録チェックボックスにチェックが無い場合
            If Me.chkInsert.Checked = False Then
                ' グリッド選択チェック
                If Me.dgvResult.SelectedRows.Count <> 1 Then
                    Call CLMsg.Show("GE0119")         ' データ未存在の場合、エラーメッセージ表示
                    Exit Sub
                End If
                bytStatus = STATUS_SAME
            Else
                bytStatus = STATUS_INSERT
            End If

            '-------------------------------------------------------------------------------
            '   画面遷移処理
            '-------------------------------------------------------------------------------
            If TransitionScreen(bytStatus) = False Then
                Exit Sub
            End If
            Me.DialogResult = Windows.Forms.DialogResult.OK     ' 処理結果にOKボタン押下設定
            Me.Dispose()                                        ' 開催登録　種別・支部選択画面破棄

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel1_Click
    '   名称　：キャンセルボタン1押下時処理
    '   概要　：キャンセルボタン1押下時処理を行う。
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel1.Click

        Try
            Me.DialogResult = Windows.Forms.DialogResult.Cancel ' 処理結果にキャンセルボタン押下設定
            Me.Dispose()                                        ' 開催登録　種別・支部選択画面破棄

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
    '   ＩＤ　：btnCancel2_Click
    '   名称　：キャンセルボタン2処理
    '   概要　：
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel2.Click

        Try
            Me.DialogResult = Windows.Forms.DialogResult.Cancel ' 処理結果にキャンセルボタン押下設定
            Me.Dispose()                                        ' 開催登録　種別・支部選択画面破棄

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
    '   ＩＤ　：chkInsert_CheckedChanged
    '   名称　：新規登録チェックボックスチェンジ処理
    '   概要　：
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub chkInsert_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInsert.CheckedChanged

        Try
            ' 新規登録チェックボックスチェック有無チェック
            If Me.chkInsert.Checked Then
                ' 新規登録チェックボックスチェック有
                Me.dtpStartDate.Enabled = True          ' 開催開始日付表示
                Me.lblStartDate.Enabled = True          ' 開催開始日付ラベル表示
                Me.dgvResult.Visible = False            ' データグリッドビュー非表示
            Else
                ' 新規登録チェックボックスチェック無
                Me.dtpStartDate.Enabled = False         ' 開催開始日付非表示
                Me.lblStartDate.Enabled = False         ' 開催開始日付ラベル非表示
                Me.dgvResult.Visible = True             ' データグリッドビュー表示
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)                       ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboKind_KeyDown
    '   名称　：種別キーダウン処理
    '   概要　：
    '   作成日：2012/03/07(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/07(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboKind_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboKind.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   ＩＤ　：cboApplyArea_KeyDown
    '   名称　：支部キーダウン処理
    '   概要　：
    '   作成日：2012/03/07(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/07(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboApplyArea_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboApplyArea.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   ＩＤ　：cboKind_SelectionChangeCommitted
    '   名称　：種別チェンジ処理
    '   概要　：
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboKind_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboKind.SelectionChangeCommitted

        Try
            ' 変形フラグが True （第二段階）の場合
            If Me.blnFlg Then

                ' 選択されいる場合
                If Me.cboKind.SelectedIndex <> -1 Then

                    '---------------------------------------------------------------------------
                    '   画面初期化処理
                    '---------------------------------------------------------------------------
                    If DisplayInitialize() = False Then
                        Exit Sub
                    End If

                End If

                ' 変形フラグを False （第一段階）
                Me.blnFlg = False

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
    '   ＩＤ　：cboApplyArea_SelectionChangeCommitted
    '   名称　：支部チェンジ処理
    '   概要　：
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboApplyArea_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboApplyArea.SelectionChangeCommitted

        Try
            ' 変形フラグが True （第二段階）の場合
            If Me.blnFlg Then

                ' 選択されいる場合
                If Me.cboApplyArea.SelectedIndex <> -1 Then

                    '---------------------------------------------------------------------------
                    '   画面初期化処理
                    '---------------------------------------------------------------------------
                    If DisplayInitialize() = False Then
                        Exit Sub
                    End If

                End If

                ' 変形フラグを False （第一段階）
                Me.blnFlg = False

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
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False                           ' 処理結果

        Try
            '---------------------------------------------------
            '   登録を行う種別・支部の選択
            '---------------------------------------------------
            If Me.blnFlg = False Then
                ' ComboBox
                Me.cboKind.DataSource = Nothing                 ' 種類
                Me.cboKind.Text = ""
                Me.cboApplyArea.DataSource = Nothing            ' 支部
                Me.cboApplyArea.Text = ""
            End If

            ' GroupBox
            Me.grpSelect.Visible = True                         ' 登録を行う種別・支部の選択

            '---------------------------------------------------
            '   新規申請
            '---------------------------------------------------
            ' CheckBox
            Me.chkInsert.Checked = False                        ' 新規登録
            ' Label
            Me.lblStartDate.Visible = False                     ' 開催開始日付ラベル
            ' DateTimePicker
            Me.dtpStartDate.Visible = False                     ' 開催開始日付
            ' GroupBox
            Me.grpInsert.Visible = False                        ' 新規申請

            '---------------------------------------------------
            '   一覧から選択
            '---------------------------------------------------
            ' DataGridView
            Me.dgvResult.Visible = False                        ' 一覧から選択
            ' GroupBox
            Me.grpResult.Visible = False                        ' 一覧から選択

            ' Button
            Me.btnOK1.Visible = True                            ' OKボタン1
            Me.btnCancel1.Visible = True                        ' キャンセルボタン1
            Me.btnOK2.Visible = False                           ' OKボタン2
            Me.btnCancel2.Visible = False                       ' キャンセルボタン2

            ' Form
            Me.Height = 180                                     ' フォーム高さ
            Me.Left = 340                                       ' フォーム位置（左）
            Me.Top = 255                                        ' フォーム位置（上）

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
    '   ＩＤ　：ControlRockUnLock
    '   名称　：コントロールロックアンロック処理
    '   概要  ：各コントロールのロック・アンロックを行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/14(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/14(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールロックアンロック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlRockUnLock() As Boolean

        Dim blnRet As Boolean = False                                   ' 処理結果
        Dim blnInsert As Boolean = False                                ' 新規登録チェックボックス用（使用可否）
        Dim blnInsertChk As Boolean = False                             ' 新規登録チェックボックス用（チェック可否）
        Dim blnInsertDate As Boolean = False                            ' 開催開始日付用（使用可否）
        Dim blnListSelect As Boolean = False                            ' 一覧選択用（表示可否）
        Dim intFormHeight As Integer = 540                              ' フォーム高さ
        Dim intOkTop As Integer = 460                                   ' OKボタン位置（トップ）
        Dim intCancelTop As Integer = 460                               ' キャンセルボタン位置（トップ）
        Dim strKind As String = ""                                      ' 種別

        Try
            strKind = Me.cboKind.SelectedValue.ToString()               ' 種別取得
            '-------------------------------------------------------------------------------
            '   表示・非表示フラグ設定
            '   各フラグは、デフォルトのFalseを設定済み
            '-------------------------------------------------------------------------------
            If strKind = UI_CIR_KIND_JOIN Then
                ' 合同
                blnInsert = True                                        ' 新規登録チェックボックス（使用可否）
                blnListSelect = True                                    ' 一覧選択（表示可否）
            ElseIf strKind = UI_CIR_KIND_TV Then
                ' TV
                blnInsert = True                                        ' 新規登録チェックボックス（使用可否）
                blnListSelect = True                                    ' 一覧選択（表示可否）
            ElseIf strKind = UI_CIR_KIND_ANY Then
                ' 任意
                blnInsertChk = True                                     ' 新規登録チェックボックス（チェック可否）
                blnInsertDate = True                                    ' 開催開始日付（使用可否）
                intFormHeight = 260                                     ' フォーム高さ
                intOkTop = 190                                          ' OKボタン位置（トップ）
                intCancelTop = 190                                      ' キャンセルボタン位置（トップ）
            End If

            '-------------------------------------------------------------------------------
            '   表示・非表示設定
            '-------------------------------------------------------------------------------
            '===========================================================
            '   登録を行う種別・支部の選択
            '===========================================================
            ' GroupBox
            Me.grpSelect.Visible = True                                 ' 登録を行う種別・支部の選択
            ' Label
            Me.lblKind.Visible = True                                   ' 種別
            Me.lblApplyArea.Visible = True                              ' 支部
            Me.lblIndispensableKind.Visible = True                      ' 種別必須（アスタリスク）
            Me.lblIndispensableApplyArea.Visible = True                 ' 支部必須（アスタリスク）
            ' ComboBox
            Me.cboKind.Visible = True                                   ' 種別
            Me.cboApplyArea.Visible = True                              ' 支部
            '===========================================================
            '   新規申請
            '===========================================================
            ' GroupBox
            Me.grpInsert.Visible = True                                 ' 新規申請
            ' CheckBox
            Me.chkInsert.Enabled = blnInsert                            ' 新規登録（使用可否）
            Me.chkInsert.Checked = blnInsertChk                         ' 新規登録チェックボックス（チェック可否）
            ' Label
            Me.lblStartDate.Visible = True                              ' 開催開始日付
            Me.lblStartDate.Enabled = blnInsertDate                     ' 開催開始日付（使用可否）
            ' DateTimePicker
            Me.dtpStartDate.Visible = True                              ' 開催開始日付
            Me.dtpStartDate.Enabled = blnInsertDate                     ' 開催開始日付（使用可否）
            '===========================================================
            '   一覧から選択
            '===========================================================
            ' GroupBox
            Me.grpResult.Visible = blnListSelect                        ' 一覧から選択（表示可否）
            ' DateGridView
            Me.dgvResult.Visible = blnListSelect                        ' 一覧から選択（表示可否）

            ' Button
            Me.btnOK1.Visible = False                                   ' OKボタン1
            Me.btnCancel1.Visible = False                               ' キャンセルボタン1
            Me.btnOK2.Visible = True                                    ' OKボタン2
            Me.btnCancel2.Visible = True                                ' キャンセルボタン2

            '-------------------------------------------------------------------------------
            '   サイズ・位置設定
            '-------------------------------------------------------------------------------
            ' Form
            Me.Height = intFormHeight                                   ' フォーム高さ
            ' Button
            Me.btnOK2.Top = intOkTop                                    ' OKボタン位置（トップ）
            Me.btnCancel2.Top = intCancelTop                            ' キャンセルボタン位置（トップ）

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
    '   ＩＤ　：DataGridViewIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DataGridViewIni() As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            With Me.dgvResult
                '-----------------------------------------------------------------------------------
                '   グリッド全体設定
                '-----------------------------------------------------------------------------------
                ' 総数
                .RowCount = 0                                                                       ' 縦
                .ColumnCount = 4                                                                    ' 横
                ' 固定行
                .ColumnHeadersVisible = True                                                        ' 列固定行有り
                .RowHeadersVisible = False                                                          ' 行固定無し
                ' スクロールバー
                .ScrollBars = ScrollBars.Both                                                       ' 縦横両方
                ' 1行選択モード
                .SelectionMode = DataGridViewSelectionMode.FullRowSelect                            ' 1行選択
                .MultiSelect = False                                                                ' 複数選択なし
                ' サイズ変更
                .AllowUserToResizeColumns = True                                                    ' 列サイズ変更可
                .AllowUserToResizeRows = False                                                      ' 行サイズ変更不可
                '-----------------------------------------------------------------------------------
                '   ヘッダー部設定
                '-----------------------------------------------------------------------------------
                ' ヘッダー文字列
                .Columns(0).HeaderText = "通知番号"                                                 ' 通知番号
                .Columns(1).HeaderText = "開催開始日付"                                             ' 開催開始日付
                .Columns(2).HeaderText = "会議場所"                                                 ' 会議場所
                .Columns(3).HeaderText = "登録日"                                                   ' 登録日
                ' ヘッダー文字位置
                .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 通知番号
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 開催開始日付
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 会議場所
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 登録日
                '-----------------------------------------------------------------------------------
                '   カラム部設定
                '-----------------------------------------------------------------------------------
                ' カラム文字位置
                .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 通知番号
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 開催開始日付
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 会議場所
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 登録日
                ' カラム幅
                .Columns(0).Width = 100                                                             ' 通知番号
                .Columns(1).Width = 120                                                             ' 開催開始日付
                .Columns(2).Width = 220                                                             ' 会議場所
                .Columns(3).Width = 100                                                             ' 登録日
                ' カラム表示有無
                .Columns(0).Visible = True                                                          ' 通知番号
                .Columns(1).Visible = True                                                          ' 開催開始日付
                .Columns(2).Visible = True                                                          ' 会議場所
                .Columns(3).Visible = True                                                          ' 登録日
            End With

            ' 戻り値格納
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

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：各種情報を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    ' 　　　　：2013/08/31(土)　Fujisaku　支部無くなり暫定対応
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成

        Try
            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------------------------------
            '   エラー箇所クリア処理
            '-------------------------------------------------------------------------------
            If ClearErr(Me) = False Then
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   コンボボックス作成
            '-------------------------------------------------------------------------------
            If Me.blnFlg = False Then
                ' 定数マスタ詳細（種別）コンボボックス作成処理呼び出し
                If CreateCboConstantDtl(clsDb, _
                                        Me.cboKind, _
                                        CONSTANT_ID_UI_CIR_KIND, _
                                        False, _
                                        MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                        -1) = False Then
                    Return blnRet
                End If

                ' 定数マスタ詳細（支部）コンボボックス作成処理呼び出し
                If CreateCboConstantDtl(clsDb, _
                                        Me.cboApplyArea, _
                                        CONSTANT_ID_APPLY_AREA, _
                                        False, _
                                        MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST, _
                                        0) = False Then
                    Return blnRet
                End If

                ' Mod 2013/09/01 暫定対応
                ' 定数マスタ詳細（支部）コンボボックス 東京固定処理
                Call Utilities.SetCanEditToControl(False, Me.cboApplyArea)
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

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetDataList
    '   名称　：一覧情報取得処理
    '   概要　：一覧情報取得
    '   引数　：ByVal iClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/17(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>一覧情報取得</summary>
    ''' <param name="iClsDb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetDataList(ByVal iClsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strSql As String = ""               ' SQL
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル
        Dim intRet As Integer = 0               ' 処理結果件数
        Dim strKsh As String = ""               ' 会社コード
        Dim strPeriodId As String = ""          ' 期ID
        Dim strApplyArea As String = ""         ' 支部
        Dim strKind As String = ""              ' 種別

        Try
            ' 各データ取得
            strKsh = MDLoginInfo.Ksh                                    ' 会社コード
            strPeriodId = MDLoginInfo.PeriodId                          ' 期ID
            strApplyArea = Me.cboApplyArea.SelectedValue.ToString()     ' 支部
            strKind = Me.cboKind.SelectedValue.ToString()               ' 種別

            '' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT u1.c_union_meeting" & vbCrLf                                     ' 01. 通知番号
            strSql = strSql & "       ,u1.d_meeting_1" & vbCrLf                                         ' 02. 開催開始日付
            strSql = strSql & "       ,u1.l_place_1" & vbCrLf                                           ' 03. 会議場所
            strSql = strSql & "       ,IIF(u1.d_ins IS NULL, u1.d_up, u1.d_ins) AS entryDate " & vbCrLf ' 04. 登録日
            strSql = strSql & "   FROM union_information AS u1" & vbCrLf                                ' 組合大会通知
            ' 会社コード、期ID、組合大会会議番号単位で件数取得
            strSql = strSql & "       ,( SELECT unif.c_ksh" & vbCrLf
            strSql = strSql & "                ,unif.c_period_id" & vbCrLf
            strSql = strSql & "                ,unif.c_union_meeting" & vbCrLf
            strSql = strSql & "                ,count(*) as ucount" & vbCrLf
            strSql = strSql & "            FROM union_information AS unif" & vbCrLf
            strSql = strSql & "           GROUP BY unif.c_ksh" & vbCrLf
            strSql = strSql & "                   ,unif.c_period_id" & vbCrLf
            strSql = strSql & "                   ,unif.c_union_meeting ) u2" & vbCrLf
            strSql = strSql & "  WHERE u1.c_ksh              = '" & strKsh & "'" & vbCrLf           ' 会社コードと同じもの
            strSql = strSql & "    AND u1.c_period_id        = '" & strPeriodId & "'" & vbCrLf      ' 期IDと同じもの
            strSql = strSql & "    AND u1.k_information_type = '" & strKind & "'" & vbCrLf          ' 種別と同じもの
            strSql = strSql & "    AND u1.k_apply_area       <> '" & strApplyArea & "'" & vbCrLf    ' 支部と同じもの
            strSql = strSql & "    AND u1.c_union_meeting    = u2.c_union_meeting" & vbCrLf         ' 組合大会会議番号と同じもの
            strSql = strSql & "    AND u2.ucount             <> 2" & vbCrLf                         ' 件数が2件以外のもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            dtRet = iClsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRet = dtRet.Rows.Count

            ' 総数設定
            Me.dgvResult.RowCount = intRet

            ' 件数チェック
            If intRet > 0 Then
                ' レコード数分ループ
                For i = 0 To intRet - 1
                    ' データ設定
                    With Me.dgvResult.Rows(i).Cells
                        ' 01. 通知番号
                        If IsDBNull(dtRet.Rows(i).Item(0)) = False Then
                            .Item(0).Value = dtRet.Rows(i).Item(0).ToString()
                        End If
                        ' 02. 開催開始日付
                        If IsDBNull(dtRet.Rows(i).Item(1)) = False Then
                            .Item(1).Value = Format(dtRet.Rows(i).Item(1), "yyyy/MM/dd")
                        End If
                        ' 03. 会議場所
                        If IsDBNull(dtRet.Rows(i).Item(2).ToString()) = False Then
                            .Item(2).Value = dtRet.Rows(i).Item(2).ToString()
                        End If
                        ' 04. 登録日
                        If IsDBNull(dtRet.Rows(i).Item(3)) = False Then
                            .Item(3).Value = Format(dtRet.Rows(i).Item(3), "yyyy/MM/dd")
                        End If
                    End With
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

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要　：メッセージIDからメッセージ内容を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            '-------------------------------------------------------------------------------
            '   初期化
            '-------------------------------------------------------------------------------
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   未選択チェック
            '-------------------------------------------------------------------------------
            ' 種別
            If Me.cboKind.SelectedIndex = -1 Then
                Call MDCommon.SetErr(Me.cboKind)
                Call CLMsg.Show("GE0006", "種別")
                Return blnRet
            End If
            ' 支部
            If Me.cboApplyArea.SelectedIndex = -1 Then
                Call MDCommon.SetErr(Me.cboApplyArea)
                Call CLMsg.Show("GE0006", "支部")
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

    End Function

    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/14(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/14(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen(ByVal bytStatus As Byte) As Boolean

        Dim blnRet As Boolean = False                                               ' 処理結果
        Dim pn As Panel = agoUserControl.ParentForm.Controls(MAIN_PANEL_ID)
        Dim clsUC040102 As UC040102 = pn.Controls(SCREEN_ID_UC040102)

        Try
            ' 画面間パラメータ情報設定
            clsUC040102 = New UC040102                                              ' 組合大会通知 - 詳細画面クラス生成
            clsUC040102.bytStatus = bytStatus                                       ' ステータス（開催登録）
            clsUC040102.strKind = Me.cboKind.SelectedValue.ToString()               ' 種別（"01"：合同, "02"：TV, "03"：任意）
            clsUC040102.strApplyArea = Me.cboApplyArea.SelectedValue.ToString()     ' 地区（申請地区区分）（"01"：東京, "02"：大阪）
            clsUC040102.strKsh = MDLoginInfo.Ksh
            clsUC040102.strPeriodId = MDLoginInfo.PeriodId
            '-------------------------------------
            '   開催開始日付
            '-------------------------------------
            ' 新規登録チェックボックスのチェック確認
            If Me.chkInsert.Checked Then
                ' チェック有の場合、開催開始日付DataTimePickerから取得
                clsUC040102.strMeetingDate = Me.dtpStartDate.Value.ToString("yyyyMMdd")
            Else
                ' チェック無の場合、DataGridViewの通知番号（1カラム目）を取得
                clsUC040102.strUnionMeeting = Me.dgvResult.CurrentRow.Cells.Item(0).Value
            End If
            Call pn.Controls.Add(clsUC040102)                                       ' 組合大会通知 - 詳細画面表示

            ' 戻り値設定
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
    '   ＩＤ　：ChangeDisplay
    '   名称　：画面変更処理
    '   概要  ：画面変更処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/20(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/20(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面変更処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChangeDisplay() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsDb As New CLAccessMdb                ' データベースクラス

        Try
            '-------------------------------------------------------------------------------
            '   入力チェック
            '-------------------------------------------------------------------------------
            If ChkInput() = False Then
                Exit Function
            End If

            ' データベース切断
            Call clsDb.Connect()

            '-------------------------------------------------------------------------------
            '   一覧情報取得処理
            '-------------------------------------------------------------------------------
            ' 種別が合同かTVの場合、一覧情報取得
            If (Me.cboKind.SelectedValue.ToString() = UI_CIR_KIND_JOIN) _
            Or (Me.cboKind.SelectedValue.ToString() = UI_CIR_KIND_TV) Then
                If GetDataList(clsDb) = False Then
                    Return blnRet
                End If
            End If

            '-------------------------------------------------------------------------------
            '   コントロールロックアンロック処理
            '-------------------------------------------------------------------------------
            If ControlRockUnLock() = False Then
                Exit Function
            End If

            ' 変形フラグを True （第二段階）
            Me.blnFlg = True

            ' 戻り値設定
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
    '   ＩＤ　：DisplayInitialize
    '   名称　：画面初期化処理
    '   概要  ：画面初期化処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DisplayInitialize() As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If ControlClear() = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   グリッド初期化処理
            '-------------------------------------------------------------------------------
            If DataGridViewIni() = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If GetData() = False Then
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
    End Function
#End Region

End Class

#End Region
