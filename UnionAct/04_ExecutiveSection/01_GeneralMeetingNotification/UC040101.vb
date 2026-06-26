#Region "UC040101"
'===========================================================================================================
'   クラスＩＤ　　：UC040101
'   クラス名称　　：組合大会通知画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDFile
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDCommon
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDInfo
Imports UnionAct.GUI.Document

Public Class UC040101

#Region "定数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC040101          ' UC040101
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040101      ' 組合大会通知画面
    ' 権限
    Private strGrantReference As String = "0"                       ' 参照権限
    Private strGrantInsert As String = "0"                          ' 登録権限
    Private strGrantPrint As String = "0"                           ' 印刷権限
    Private strGrantFileOutput As String = "0"                      ' ファイル出力権限
    ' C1FlexGrid
    Private Const FLEXGRID_ROWS As Byte = 1                         ' 縦総数（組合大会通知検索）
    Private Const FLEXGRID_COLS As Byte = 11                        ' 横総数（組合大会通知検索）
    Private Const FLEXGRID_ROWS_TMP As Byte = 1                     ' 縦総数（一時保存文書検索）
    Private Const FLEXGRID_COLS_TMP As Byte = 9                     ' 横総数（一時保存文書検索）
    Private Const FLEXGRID_ROWS_FIXED As Byte = 1                   ' 固定縦数（組合大会通知検索、一時保存文書検索）
    Private Const FLEXGRID_COLS_FIXED As Byte = 0                   ' 固定横数（組合大会通知検索、一時保存文書検索）
    ' ステータス
    Private Const STATUS_INSERT As Byte = 1                         ' 開催登録（組合大会通知検索のみ）
    Private Const STATUS_DETAIL As Byte = 2                         ' 詳細（組合大会通知検索・一時保存文書検索）
    Private Const STATUS_UPDATE As Byte = 3                         ' 変更（組合大会通知検索のみ）
    Private Const STATUS_STOP As Byte = 4                           ' 中止（組合大会通知検索のみ）
    Private Const STATUS_DELETE As Byte = 5                         ' 削除（一時保存文書検索のみ）
    Private Const STATUS_SAME As Byte = 6                           ' 同番号開催登録（組合大会通知検索のみ）
#End Region

#Region "プロパティ"
    Public _blnSearchFlg As Boolean = False                 ' 再検索フラグ

    ' 再検索フラグ
    Public Property blnSearchFlg() As Boolean
        Get
            Return _blnSearchFlg
        End Get
        Set(ByVal value As Boolean)
            _blnSearchFlg = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC040101_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC040101_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '-------------------------------------------------------------------------------
            '   権限取得処理
            '-------------------------------------------------------------------------------
            If setGrant() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------------------------------
            If ControlClear() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   グリッド初期化処理
            '-------------------------------------------------------------------------------
            If C1FlexGridIni() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            If GetData() = False Then
                Exit Sub
            End If

            ' 種別コンボボックスにフォーカスセット
            Me.cboYear.Focus()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：UC040101_VisibleChanged
    '   名称　：フォーム表示非表示チェンジ処理
    '   概要　：
    '   作成日：2012/02/17(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/17(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC040101_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged

        Try
            ' フォーム表示で検索フラグがTrueの場合、再検索
            ' 組合大会通知 - 更新処理（開催登録・変更・中止）後に再検索処理を行う。
            If Me.Visible And Me.blnSearchFlg Then
                '-----------------------------------------------
                '   検索データ取得処理
                '-----------------------------------------------
                If GetSearchData() = False Then
                    Exit Sub
                End If
                ' 再検索フラグを戻す
                Me.blnSearchFlg = False
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' しばらくお待ちくださいフォーム非表示
            FrmWaitInfo.CloseWaitForm()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' しばらくお待ちくださいフォーム非表示
            FrmWaitInfo.CloseWaitForm()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            '---------------------------------------------------------------------------
            '   検索データ取得処理
            '---------------------------------------------------------------------------
            If GetSearchData() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnDetail_Click
    '   名称　：詳細ボタン押下時処理
    '   概要　：詳細ボタン押下時処理を行う。
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetail.Click

        Try
            '---------------------------------------------------------------------------
            '   画面遷移処理（組合大会通知 - 詳細）
            '---------------------------------------------------------------------------
            If TransitionScreen(STATUS_DETAIL) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnInsert_Click
    '   名称　：開催登録ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsert.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            '---------------------------------------------------------------------------
            '   画面遷移処理（組合大会通知 - 開催登録）
            '---------------------------------------------------------------------------
            If TransitionScreen(STATUS_INSERT) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnUpdate_Click
    '   名称　：変更ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            '---------------------------------------------------------------------------
            '   画面遷移処理（組合大会通知 - 変更）
            '---------------------------------------------------------------------------
            If TransitionScreen(STATUS_UPDATE) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnStop_Click
    '   名称　：中止ボタンクリック処理
    '   概要　：
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnStop_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStop.Click

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            '---------------------------------------------------------------------------
            '   画面遷移処理（組合大会通知 - 中止）
            '---------------------------------------------------------------------------
            If TransitionScreen(STATUS_STOP) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cfgResult_DoubleClick
    '   名称　：フレックスグリッドダブルクリック処理
    '   概要　：
    '   作成日：2012/02/14(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/14(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cfgResult_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cfgResult.DoubleClick

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            '---------------------------------------------------------------------------
            '   画面遷移処理（組合大会通知 - 詳細）
            '---------------------------------------------------------------------------
            If TransitionScreen(STATUS_DETAIL) = False Then
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
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboYear_KeyDown
    '   名称　：年コンボボックスキーダウン処理
    '   概要　：
    '   作成日：2012/02/14(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/14(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboYear_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboYear.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                '-----------------------------------------------------------------------
                '   検索データ取得処理
                '-----------------------------------------------------------------------
                If GetSearchData() = False Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboMonth_KeyDown
    '   名称　：月コンボボックスキーダウン処理
    '   概要　：
    '   作成日：2012/02/14(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/14(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboMonth_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboMonth.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                '-----------------------------------------------------------------------
                '   検索データ取得処理
                '-----------------------------------------------------------------------
                If GetSearchData() = False Then
                    Exit Sub
                End If
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
    '   ＩＤ　：cboKind_KeyDown
    '   名称　：種別コンボボックスキーダウン処理
    '   概要　：
    '   作成日：2012/02/14(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/14(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboKind_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboKind.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                '-----------------------------------------------------------------------
                '   検索データ取得処理
                '-----------------------------------------------------------------------
                If GetSearchData() = False Then
                    Exit Sub
                End If
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
    '   ＩＤ　：cboYear_SelectedIndexChanged
    '   名称　：年コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYear.SelectedIndexChanged

        Try
            '---------------------------------------------------------------------------
            '   グリッド初期化処理
            '---------------------------------------------------------------------------
            If C1FlexGridIni() = False Then
                Exit Sub
            End If

            '---------------------------------------------------------------------------
            '   開催開始日付　月コンボボックス
            '---------------------------------------------------------------------------
            ' 開催開始日付　月コンボボックス作成処理
            If Me.cboYear.SelectedIndex > 0 Then
                If Not Me.cboMonth.SelectedIndex > 0 Then
                    If CreateComboBoxMM(Me.cboMonth, _
                                        True, _
                                        COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST) = False Then
                        Exit Sub
                    End If
                End If
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboMonth_SelectedIndexChanged
    '   名称　：月コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMonth.SelectedIndexChanged

        Try
            '---------------------------------------------------------------------------
            '   グリッド初期化処理
            '---------------------------------------------------------------------------
            If C1FlexGridIni() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboKind_SelectedIndexChanged
    '   名称　：種別コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboKind_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboKind.SelectedIndexChanged

        Try
            '---------------------------------------------------------------------------
            '   グリッド初期化処理
            '---------------------------------------------------------------------------
            If C1FlexGridIni() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
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
            '---------------------------------------------------------------------------
            '   組合大会通知検索
            '---------------------------------------------------------------------------
            ' ComboBox
            Me.cboYear.DataSource = Nothing                     ' 開催開始日付年
            Me.cboYear.Text = ""
            Me.cboMonth.DataSource = Nothing                    ' 開催開始日付月
            Me.cboMonth.Text = ""
            Me.cboKind.DataSource = Nothing                     ' 種別
            Me.cboKind.Text = ""
            ' Button
            Me.btnSearch.Visible = True                         ' 検索ボタン表示
            Me.btnDetail.Visible = False                        ' 詳細ボタン非表示
            Me.btnStop.Visible = False                          ' 中止ボタン非表示
            Me.btnUpdate.Visible = False                        ' 変更ボタン非表示
            Me.btnInsert.Visible = True                         ' 開催登録ボタン表示

            '---------------------------------------------------------------------------
            '   権限設定
            '---------------------------------------------------------------------------
            ' 登録権限
            If strGrantInsert = GRANT_VALID Then
                Me.btnUpdate.Enabled = True                     ' 変更ボタン押下可
                Me.btnStop.Enabled = True                       ' 中止ボタン押下可
                Me.btnInsert.Enabled = True                     ' 開催登録ボタン押下可
            ElseIf strGrantInsert = GRANT_VOID Then
                Me.btnUpdate.Enabled = False                    ' 変更ボタン押下不可
                Me.btnStop.Enabled = False                      ' 中止ボタン押下不可
                Me.btnInsert.Enabled = False                    ' 開催登録ボタン押下不可
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：C1FlexGridIni
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
    Private Function C1FlexGridIni() As Boolean

        Dim blnRet As Boolean = False   ' 処理結果

        Try
            With Me.cfgResult
                '-----------------------------------------------------------------------------------
                '   グリッド全体設定
                '-----------------------------------------------------------------------------------
                .Redraw = False                                                                     ' 描画なし（処理が終了した最後に描画）
                ' 総数
                .Rows.Count = FLEXGRID_ROWS                                                         ' 縦総数
                .Cols.Count = FLEXGRID_COLS                                                         ' 横総数
                ' 固定行
                .Rows.Fixed = FLEXGRID_ROWS_FIXED                                                   ' 固定縦数
                .Cols.Fixed = FLEXGRID_COLS_FIXED                                                   ' 固定横数
                ' スクロールバー
                .ScrollBars = ScrollBars.Both                                                       ' 縦横両方
                ' 選択モード
                .SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row                            ' 1行選択
                .HighLight = C1.Win.C1FlexGrid.HighLightEnum.Always                                 ' 常に選択範囲強調
                .FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None                                   ' フォーカス矩形なし
                ' サイズ変更
                .AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.Columns                        ' 列のみサイズ変更可
                ' マージ
                .AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.RestrictCols                     ' 左セルがマージされていればマージ
                ' グリッド内編集
                .AllowEditing = False                                                               ' 編集不可
                '-----------------------------------------------------------------------------------
                '   ヘッダー部設定
                '-----------------------------------------------------------------------------------
                ' マージ有無
                .Cols(0).AllowMerging = True                                                        ' 01. 通知番号
                .Cols(1).AllowMerging = True                                                        ' 02. 種別
                .Cols(2).AllowMerging = False                                                       ' 03. 支部
                .Cols(3).AllowMerging = False                                                       ' 04. 種類
                .Cols(4).AllowMerging = False                                                       ' 05. 開催開始日付
                .Cols(5).AllowMerging = False                                                       ' 06. 会議場所
                .Cols(6).AllowMerging = False                                                       ' 07. 登録日
                .Cols(7).AllowMerging = False                                                       ' 08. 会社コード
                .Cols(8).AllowMerging = False                                                       ' 09. 期ID
                .Cols(9).AllowMerging = False                                                       ' 10. 申請地区区分
                .Cols(10).AllowMerging = False                                                      ' 10. 組合大会会議SEQ
                ' ヘッダー文字列
                .Cols(0).Caption = "通知番号"                                                       ' 01. 通知番号
                .Cols(1).Caption = "種別"                                                           ' 02. 種別
                .Cols(2).Caption = "支部"                                                           ' 03. 支部
                .Cols(3).Caption = "種類"                                                           ' 04. 種類
                .Cols(4).Caption = "開催開始日付"                                                   ' 05. 開催開始日付
                .Cols(5).Caption = "会議場所"                                                       ' 06. 会議場所
                .Cols(6).Caption = "登録日"                                                         ' 07. 登録日
                .Cols(7).Caption = "会社コード"                                                     ' 08. 会社コード
                .Cols(8).Caption = "期ID"                                                           ' 09. 期ID
                .Cols(9).Caption = "申請地区区分"                                                   ' 10. 申請地区区分
                .Cols(10).Caption = "組合大会会議SEQ"                                               ' 11. 組合大会会議SEQ
                ' ヘッダー文字位置
                .Cols(0).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 01. 通知番号  
                .Cols(1).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 02. 種別
                .Cols(2).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 03. 支部
                .Cols(3).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 04. 種類
                .Cols(4).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 05. 開催開始日付
                .Cols(5).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 06. 会議場所
                .Cols(6).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 07. 登録日
                .Cols(7).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 08. 会社コード
                .Cols(8).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 09. 期ID
                .Cols(9).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter              ' 10. 申請地区区分
                .Cols(10).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter             ' 11. 組合大会会議SEQ
                '-----------------------------------------------------------------------------------
                '   カラム部設定
                '-----------------------------------------------------------------------------------
                ' カラム文字位置
                .Cols(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                     ' 01. 通知番号
                .Cols(1).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter                   ' 02. 種別
                .Cols(2).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter                   ' 03. 支部
                .Cols(3).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                     ' 04. 種類
                .Cols(4).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                     ' 05. 開催開始日付
                .Cols(5).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                     ' 06. 会議場所
                .Cols(6).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                     ' 07. 登録日
                .Cols(7).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                     ' 08. 会社コード
                .Cols(8).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                     ' 09. 期ID
                .Cols(9).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                     ' 10. 申請地区区分
                .Cols(10).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter                    ' 11. 組合大会会議SEQ
                ' カラム幅
                .Cols(0).Width = 100                                                                ' 01. 通知番号
                .Cols(1).Width = 70                                                                 ' 02. 種別
                .Cols(2).Width = 70                                                                 ' 03. 支部
                .Cols(3).Width = 70                                                                 ' 04. 種類
                .Cols(4).Width = 120                                                                ' 05. 開催開始日付
                .Cols(5).Width = 325                                                                ' 06. 会議場所
                .Cols(6).Width = 100                                                                ' 07. 登録日
                .Cols(7).Width = 100                                                                ' 08. 会社コード
                .Cols(8).Width = 100                                                                ' 09. 期ID
                .Cols(9).Width = 100                                                                ' 10. 申請地区区分
                .Cols(10).Width = 100                                                               ' 11. 組合大会会議SEQ
                ' カラム表示有無
                .Cols(0).Visible = True                                                             ' 01. 通知番号
                .Cols(1).Visible = True                                                             ' 02. 種別
                .Cols(2).Visible = True                                                             ' 03. 支部
                .Cols(3).Visible = True                                                             ' 04. 種類
                .Cols(4).Visible = True                                                             ' 05. 開催開始日付
                .Cols(5).Visible = True                                                             ' 06. 会議場所
                .Cols(6).Visible = True                                                             ' 07. 登録日
                .Cols(7).Visible = False                                                            ' 08. 会社コード
                .Cols(8).Visible = False                                                            ' 09. 期ID
                .Cols(9).Visible = False                                                            ' 10. 申請地区区分
                .Cols(10).Visible = False                                                           ' 11. 組合大会会議SEQ
                ' 描画
                Me.cfgResult.Redraw = True
            End With

            '-----------------------------------------------------------------------------------
            '   検索件数
            '-----------------------------------------------------------------------------------
            If SetTotalCount(Me.grpResult, 0) = False Then
                Exit Function
            End If

            ' ボタン
            Me.btnDetail.Visible = False        ' 詳細ボタン非表示
            Me.btnUpdate.Visible = False        ' 変更ボタン非表示
            Me.btnStop.Visible = False          ' 中止ボタン非表示

            ' 戻り値格納
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：組合員種別コンボボックス作成処理を呼び出す。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsDb As New CLAccessMdb                ' データベースクラス

        Try
            Call clsDb.Connect()                    ' データベース接続
            '-------------------------------------------------------------------------------
            '   組合大会通知検索
            '-------------------------------------------------------------------------------
            ' 開催開始日付　年コンボボックス作成処理
            Me.cboYear.DataSource = Nothing
            If CreateComboBoxYYYY(Me.cboYear, _
                                  True, _
                                  COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST) = False Then
                Return blnRet
            End If
            '' 開催開始日付　月コンボボックス作成処理
            Me.cboMonth.DataSource = Nothing
            Me.cboMonth.Items.AddRange(New Object() {""})
            Me.cboMonth.DropDownStyle = ComboBoxStyle.DropDownList   ' ドロップダウンスタイル設定

            ' 定数マスタ詳細（種別）コンボボックス作成処理
            If CreateCboConstantDtl(clsDb, _
                                    Me.cboKind, _
                                    CONSTANT_ID_UI_CIR_KIND) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
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
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>検索データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSearchData() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRet As Integer = 0                   ' 検索結果件数
        Dim strRefDate As String = ""               ' 検索条件　開催開始日付
        Dim strRefKind As String = ""               ' 検索条件　種別

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' しばらくお待ちくださいフォーム表示
            FrmWaitInfo.ShowWaitForm(Nothing)

            '-----------------------------------------------------------------------
            '   グリッド初期化
            '-----------------------------------------------------------------------
            If C1FlexGridIni() = False Then
                Exit Function
            End If

            '---------------------------------------------------------------------------
            '   検索条件取得
            '---------------------------------------------------------------------------
            ' 開催開始日付
            If (Me.cboYear.SelectedIndex > 0) _
            And (Me.cboMonth.SelectedIndex > 0) Then
                ' 開催開始日付年月選択時
                strRefDate = Me.cboYear.Text & Me.cboMonth.Text
            ElseIf Me.cboYear.SelectedIndex > 0 _
            And Me.cboMonth.SelectedIndex = 0 Then
                ' 開催開始日付年のみ選択時
                strRefDate = Me.cboYear.Text
            End If
            ' 種別
            If Me.cboKind.SelectedIndex > 0 Then
                strRefKind = Me.cboKind.SelectedValue.ToString()
            End If
            '---------------------------------------------------------------------------
            '   SQL作成
            '---------------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & "  SELECT inf.c_union_meeting     AS c_union_meeting" & vbCrLf                ' 01. 組合大会会議番号
            strSql = strSql & "        ,cd1.l_name              AS l_union_type" & vbCrLf                   ' 02. 種別
            strSql = strSql & "        ,cd2.l_name              AS apply_area" & vbCrLf                     ' 03. 支部
            strSql = strSql & "        ,IIF(inf.l_information_name = '変更', '開催', inf.l_information_name) AS information_type" & vbCrLf ' 04. 種類
            strSql = strSql & "        ,inf.d_meeting_1         AS d_meeting_1" & vbCrLf                    ' 05. 開催開始日付
            strSql = strSql & "        ,inf.l_place_1           AS l_place_1" & vbCrLf                      ' 06. 会議場所
            strSql = strSql & "        ,IIF(inf.d_ins IS NULL, inf.d_up, inf.d_ins) AS entryDate" & vbCrLf  ' 07. 登録日（作成日 or 更新日）
            strSql = strSql & "        ,inf.c_ksh               AS c_ksh" & vbCrLf                          ' 08. 会社コード
            strSql = strSql & "        ,inf.c_period_id         AS c_period_id" & vbCrLf                    ' 09. 期ID
            strSql = strSql & "        ,inf.k_apply_area        AS k_apply_area" & vbCrLf                   ' 10. 申請地区区分
            strSql = strSql & "        ,inf.s_union_meeting_seq AS s_union_meeting_seq" & vbCrLf            ' 11. 組合大会会議SEQ
            strSql = strSql & "    FROM union_information AS inf" & vbCrLf      ' 組合大会通知
            strSql = strSql & "        ,period            AS prd" & vbCrLf      ' 期マスタ
            strSql = strSql & "        ,constant_dtl      AS cd1" & vbCrLf      ' 定数マスタ詳細１（種別）
            strSql = strSql & "        ,constant_dtl      AS cd2" & vbCrLf      ' 定数マスタ詳細２（支部）
            strSql = strSql & "   WHERE inf.c_ksh = '" & MDLoginInfo.Ksh & "'" & vbCrLf
            strSql = strSql & "     AND inf.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            ' 定数マスタ詳細と結合（種別）
            strSql = strSql & "     AND cd1.c_constant = 'UI_CIR_KIND'" & vbCrLf
            strSql = strSql & "     AND cd1.c_constant_seq = inf.k_information_type" & vbCrLf
            ' 定数マスタ詳細と結合（支部）
            strSql = strSql & "     AND cd2.c_constant = 'APPLY_AREA'" & vbCrLf
            strSql = strSql & "     AND cd2.c_constant_seq = inf.k_apply_area" & vbCrLf
            ' 組合大会通常情報と期マスタを結合
            strSql = strSql & "     AND prd.c_period_id    = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            strSql = strSql & "     AND prd.c_ksh          = '" & MDLoginInfo.Ksh & "'" & vbCrLf
            strSql = strSql & "     AND inf.c_period_id    = prd.c_period_id" & vbCrLf
            ' 開催開始日付
            If strRefDate.Length = 6 Then
                strSql = strSql & "     AND FORMAT(inf.d_meeting_1, 'yyyyMM') = '" & strRefDate & "'" & vbCrLf
            ElseIf strRefDate.Length = 4 Then
                strSql = strSql & "     AND FORMAT(inf.d_meeting_1, 'yyyy') = '" & strRefDate & "'" & vbCrLf
            End If
            ' 種別
            If strRefKind.Length > 0 Then
                strSql = strSql & "     AND inf.k_information_type = '" & strRefKind & "'" & vbCrLf
            End If
            ' 通知番号, 種別で並替
            strSql = strSql & "   ORDER BY inf.s_union_meeting_seq" & vbCrLf
            strSql = strSql & "           ,inf.d_ins" & vbCrLf
            strSql = strSql & "           ,cd2.s_order" & vbCrLf
            strSql = strSql & ";" & vbCrLf
            '---------------------------------------------------------------------------
            '   データ表示
            '---------------------------------------------------------------------------
            Call clsDb.Connect()                                                        ' データベース接続
            tbRet = clsDb.ExecuteSql(strSql)                                            ' SQL実行
            intRet = tbRet.Rows.Count                                                   ' 件数取得
            If intRet > 0 Then                                                          ' 件数チェック
                ' 1件以上の処理
                Me.grpResult.Visible = True
                With Me.cfgResult
                    .Rows.Count = intRet + 1                                            ' 縦総数設定
                    .Redraw = False                                                     ' 描画なし（処理が終了した最後に描画）
                    For i = 0 To intRet - 1                                             ' レコード数分ループ
                        ' データ設定
                        .SetData(i + 1, 0, NVL(tbRet.Rows(i).Item(0)))                  ' 01. 通知番号（組合大会会議番号）
                        .SetData(i + 1, 1, NVL(tbRet.Rows(i).Item(1)))                  ' 02. 種別
                        .SetData(i + 1, 2, NVL(tbRet.Rows(i).Item(2)))                  ' 03. 支部
                        .SetData(i + 1, 3, NVL(tbRet.Rows(i).Item(3)))                  ' 04. 種類
                        .SetData(i + 1, 4, NVL(tbRet.Rows(i).Item(4)))                  ' 05. 開催開始日付
                        .SetData(i + 1, 5, NVL(tbRet.Rows(i).Item(5)))                  ' 06. 会議場所
                        .SetData(i + 1, 6, NVL(tbRet.Rows(i).Item(6)))                  ' 07. 登録日
                        .SetData(i + 1, 7, NVL(tbRet.Rows(i).Item(7)))                  ' 08. 会社コード
                        .SetData(i + 1, 8, NVL(tbRet.Rows(i).Item(8)))                  ' 09. 期ID
                        .SetData(i + 1, 9, NVL(tbRet.Rows(i).Item(9)))                  ' 10. 申請地区区分
                        .SetData(i + 1, 10, NVL(tbRet.Rows(i).Item(10)))                ' 11. 組合大会会議SEQ
                    Next
                    .Redraw = True
                    .Visible = True
                End With
                Me.btnDetail.Visible = True                                             ' 詳細ボタン表示
                Me.btnUpdate.Visible = True                                             ' 変更ボタン表示
                Me.btnStop.Visible = True                                               ' 中止ボタン表示
            Else
                ' 0件の処理
                Me.btnDetail.Visible = False                                            ' 詳細ボタン非表示
                Me.btnUpdate.Visible = False                                            ' 変更ボタン非表示
                Me.btnStop.Visible = False                                              ' 中止ボタン非表示
                CLMsg.Show("DI0001")                                                    ' 対象データなしメッセージボックス表示
            End If

            ' グループボックス件数設定
            If SetTotalCount(Me.grpResult, intRet) = False Then
                Exit Function
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' しばらくお待ちくださいフォーム非表示
            FrmWaitInfo.CloseWaitForm()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
            ' しばらくお待ちくださいフォーム非表示
            FrmWaitInfo.CloseWaitForm()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：setGrant
    '   名称　：権限処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/02(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>権限処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function setGrant() As Boolean

        Dim blnRet As Boolean = False                                           ' 処理結果
        Dim dtGrant As DataTable = Nothing                                      ' 権限取得データテーブル

        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC040101)
            If dtGrant.Rows.Count > 0 Then
                strGrantReference = dtGrant.Rows(0).Item(3).ToString            ' 参権限照
                strGrantInsert = dtGrant.Rows(0).Item(4).ToString               ' 登録権限
                strGrantPrint = dtGrant.Rows(0).Item(5).ToString                ' 印刷権限
                strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString           ' ファイル出力権限
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetTotalCount
    '   名称　：件数設定処理
    '   概要  ：検索件数を設定する。
    '   引数　：ByVal iGrpObj    As System.Windows.Forms.GroupBox = 件数表示グループボックス,
    '           ByVal iRowsCount As Integer                       = 検索件数
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/09(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/09(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>件数設定処理</summary>
    ''' <param name="iGrpObj">件数表示グループボックス</param>
    ''' <param name="iRowsCount">検索件数</param>
    ''' <remarks></remarks>
    Private Function SetTotalCount(ByVal iGrpObj As System.Windows.Forms.GroupBox, _
                                   ByVal iRowsCount As Integer) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            iGrpObj.Text = ("検索結果" & "( " & iRowsCount.ToString & " " & "件" & ")")

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：ByVal bytStatus As Byte = 1：開催登録（組合大会通知検索のみ）, 
    '                                     2：詳細（組合大会通知検索・一時保存文書検索）,
    '                                     3：変更（組合大会通知検索のみ）,
    '                                     4：中止（組合大会通知検索のみ）,
    '                                     5：削除（一時保存文書検索のみ）
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/06(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <param name="bytStatus">ステータス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen(ByVal bytStatus As Byte) As Boolean

        Dim blnRet As Boolean = False                               ' 処理結果
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)       ' パネルオブジェクト
        Dim clsUC040102 As UC040102 = Nothing                       ' 組合大会通知 - 詳細画面クラス
        Dim clsFM040103 As FM040103 = Nothing                       ' 開催登録　種別・支部選択画面クラス
        Dim strChkStartDate As String = ""                          ' チェック用開始日付（変更・中止）
        Dim strChkUnionMeetingNo As String = ""                     ' チェック用通知番号（変更・中止）
        Dim strApplyArea As String = ""                             ' チェック用申請地区区分

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            '---------------------------------------------------------------------------
            '   組合大会通知検索
            '---------------------------------------------------------------------------
            If bytStatus = STATUS_INSERT Then
                '=======================================
                '   開催登録
                '=======================================
                clsFM040103 = New FM040103(Me)
                Call clsFM040103.ShowDialog()                                           ' 開催登録　種別・支部選択画面表示
                If clsFM040103.DialogResult = DialogResult.Cancel Then
                    Return blnRet
                End If
            Else
                '=======================================
                '   詳細・変更・中止
                '=======================================
                With Me.cfgResult
                    ' グリッド選択チェック
                    If .Selection.r1 < 1 Then                                           ' 選択されているかチェック
                        Call CLMsg.Show("GW0001", "データ")                             ' 未選択の場合、エラーメッセージ表示
                        Return blnRet
                    End If
                    '-------------------------------------------------------------------
                    '   変更・中止のみチェック
                    '-------------------------------------------------------------------
                    If bytStatus = STATUS_UPDATE _
                    Or bytStatus = STATUS_STOP Then
                        ' チェック用開始日付
                        strChkStartDate = .GetData(.Row, 4).ToString().Substring(0, 10).Replace("/", "").Replace("-", "")
                        ' チェック用通知番号
                        strChkUnionMeetingNo = .GetData(.Row, 0).ToString()
                        ' チェック用申請地区区分
                        If .GetData(.Row, 2).ToString() = "東京" Then
                            strApplyArea = "01"
                        ElseIf .GetData(.Row, 2).ToString() = "大阪" Then
                            strApplyArea = "02"
                        End If
                        '-------------------------------------------------------------------
                        '   開始日付終了チェック処理
                        '-------------------------------------------------------------------
                        If ChkStartDateEnd(strChkStartDate) = False Then
                            Call CLMsg.Show("GI0021", "終了しています。")
                            Exit Function
                        End If
                        '-------------------------------------------------------------------
                        '   中止通知チェック処理
                        '-------------------------------------------------------------------
                        If ChkStopNotice(strChkUnionMeetingNo, _
                                         strApplyArea) Then
                            Call CLMsg.Show("GI0021", "中止となっています。")
                            Exit Function
                        End If
                    End If

                    ' 画面間パラメータ情報設定
                    clsUC040102 = New UC040102                                          ' 組合大会通知 - 詳細画面クラス生成
                    clsUC040102.bytStatus = bytStatus                                   ' ステータス
                    clsUC040102.strKsh = .GetData(.Row, 7).ToString()                   ' 会社コード
                    clsUC040102.strPeriodId = .GetData(.Row, 8).ToString()              ' 期ID
                    clsUC040102.strUnionMeeting = .GetData(.Row, 0).ToString()          ' 組合大会会議番号
                    clsUC040102.strApplyArea = .GetData(.Row, 9).ToString()             ' 申請地区区分
                    clsUC040102.intUnionMeetingSeq = CInt(.GetData(.Row, 10))           ' 組合大会会議SEQ
                    Call pnl.Controls.Add(clsUC040102)                                  ' 組合大会通知 - 詳細画面表示
                End With
            End If

            ' 組合大会通知検索画面非表示
            Me.Visible = False

            ' 戻り値設定
            blnRet = True

        Catch ex As Exception
            ' パネル非表示
            pnl.Visible = False
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkStartDateEnd
    '   名称　：開始日付終了チェック処理
    '   概要  ：開始日付が終了しているかチェックを行う。
    '   引数　：ByVal iStrStartDate As String = 開始日付
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/02/14(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/14(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>開始日付終了チェック処理</summary>
    ''' <param name="iStrStartDate">開始日付</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkStartDateEnd(ByVal iStrStartDate As String) As Boolean

        Dim blnRet As Boolean = False                           ' 処理結果
        Dim strNow As String = ""                               ' 現在日付（YYYY/MM/DD）

        Try
            strNow = Now.ToString("yyyyMMdd")                   ' 現在日付取得
            If strNow > iStrStartDate Then                      ' 開始日付と現在日付
                Return blnRet                                   ' 処理を抜ける
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkStopNotice
    '   名称　：中止通知チェック処理
    '   概要  ：対象の組合大会（通知番号）が中止通知が登録されているかチェックを行う。
    '   引数　：ByVal iStrUnionMeetingNo As String = 通知番号.
    '           ByVal iStrApplyArea      As String = 申請地区区分
    '   戻り値：True = 中止している, False = 中止されていない
    '   作成日：2012/02/14(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/14(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>中止通知チェック処理</summary>
    ''' <param name="iStrUnionMeetingNo">通知番号</param>
    ''' <returns>True = 中止している, False = 中止されていない</returns>
    ''' <remarks></remarks>
    Private Function ChkStopNotice(ByVal iStrUnionMeetingNo As String, _
                                   ByVal iStrApplyArea As String) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsDb As New CLAccessMdb            ' データベースクラス
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル
        Dim intRet As Integer = 0               ' 処理件数
        Dim strSql As String = ""               ' SQL

        Try
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT a.c_ksh" & vbCrLf
            strSql = strSql & "       ,a.c_period_id" & vbCrLf
            strSql = strSql & "       ,a.c_union_meeting" & vbCrLf
            strSql = strSql & "       ,a.k_apply_area" & vbCrLf
            strSql = strSql & "   FROM union_information AS a" & vbCrLf
            strSql = strSql & "  WHERE a.c_ksh              = '" & MDLoginInfo.Ksh & "'" & vbCrLf
            strSql = strSql & "    AND a.c_period_id        = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            strSql = strSql & "    AND a.c_union_meeting    = '" & iStrUnionMeetingNo & "'" & vbCrLf
            strSql = strSql & "    AND a.k_apply_area       = '" & iStrApplyArea & "'" & vbCrLf
            strSql = strSql & "    AND a.l_information_name = '中止'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' 0件チェック
            If dtRet.Rows.Count = 0 Then
                Return blnRet
            End If

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

End Class

#End Region
