#Region "UC010101"
'===========================================================================================================
'   クラスＩＤ　　：UC010101
'   クラス名称　　：組合員検索
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Document

Public Class UC010101

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面種別
    Private Const SCREEN_SEARCH As Byte = 1                                             ' 組合員検索画面
    Private Const SCREEN_HISTORY As Byte = 2                                            ' 適用日付選択画面
    ' 基本住所種別
    Private Const KIND_BASE As Byte = 1                                                 ' 基本情報
    Private Const KIND_ADDRESS As Byte = 2                                              ' 住所情報
    ' ステータス
    Private Const STATUS_INSERT As Byte = 1                                             ' 新規登録
    Private Const STATUS_UPDATE As Byte = 2                                             ' 内容変更
    ' 履歴新規登録フラグ
    Private Const HISTORY_FLG As Byte = 0                                               ' 0：通常新規登録
    ' イベント
    Private Const EVENT_FORM_LOAD As Byte = 0                                           ' フォームロード
    Private Const EVENT_SEARCH_DATA As Byte = 1                                         ' 検索結果1件以上
    Private Const EVENT_SEARCH_NODATA As Byte = 2                                       ' 検索結果0件
    ' タイトル
    Private Const TITLE_ENTRY_DATE As String = "基本情報履歴 - 加入年月日選択画面"      ' 加入年月日選択画面
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC010101                              ' UC010101
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC010101                          ' 組合員検索画面
    ' 権限
    Private strGrantReference As String = "0"                                           ' 参照権限
    Private strGrantInsert As String = "0"                                              ' 登録権限
    Private strGrantPrint As String = "0"                                               ' 印刷権限
    Private strGrantFileOutput As String = "0"                                          ' ファイル出力権限
#End Region

#Region "プロパティ"
    Public _blnSearchFlg As Boolean = False                             ' 再検索フラグ（True：再検索有り, False：再検索無し）

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
    '   ＩＤ　：UC010101_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC010101_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

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
            If ControlClear(EVENT_FORM_LOAD) = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   グリッド初期化処理
            '-------------------------------------------------------------------------------
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            ' 組合員種別コンボボックスデータ取得
            If GetData() = False Then
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
    '   ＩＤ　：UC010101_VisibleChanged
    '   名称　：フォームチェンジ処理
    '   概要　：
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC010101_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged

        Try
            ' フォーム表示で検索フラグがTrueの場合、再検索
            ' 組合員管理 - 基本情報画面 or 組合員管理 - 住所情報画面で更新処理（登録・内容変更）後に再検索処理を行う。
            If Me.Visible And Me.blnSearchFlg Then
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor
                ' しばらくお待ちくださいフォーム表示
                FrmWaitInfo.ShowWaitForm(Nothing)
                '-----------------------------------------------
                '   グリッド初期化
                '-----------------------------------------------
                If DataGridViewIni() = False Then
                    Exit Sub
                End If
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
    '   ＩＤ　：btnMemberManage_Click
    '   名称　：組合員人数ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnMemberManage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMemberManage.Click

        Dim clsFM010105 As FM010105 = Nothing                                               ' 基準日入力画面

        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソルを砂時計に設定
            Call FM010105.ShowDialog()                                                      ' 基準日入力画面表示

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            Cursor.Current = Cursors.Default                                                ' カーソルを矢印に戻す
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            FM010105.Dispose()                                                              ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
            Cursor.Current = Cursors.Default                                                ' カーソルを矢印に戻す
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' しばらくお待ちくださいフォーム表示
            FrmWaitInfo.ShowWaitForm(Nothing)

            '-------------------------------------------------------------------------------
            '   グリッド初期化
            '-------------------------------------------------------------------------------
            If DataGridViewIni() = False Then
                Exit Sub
            End If

            '-------------------------------------------------------------------------------
            '   検索データ取得処理
            '-------------------------------------------------------------------------------
            If GetSearchData() = False Then
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
            ' しばらくお待ちくださいフォーム非表示
            FrmWaitInfo.CloseWaitForm()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnNewInsert_Click
    '   名称　：新規登録ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnNewInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewInsert.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 適用日付画面遷移処理
            If ShowFM010104() = False Then
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
    '   ＩＤ　：btnBase_Click
    '   名称　：基本情報照会ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnBase_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBase.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面遷移処理（組合員管理 - 住所情報）
            If TransitionScreen(KIND_BASE) = False Then
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
    '   ＩＤ　：btnNewInsert_Click
    '   名称　：住所情報照会ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnAddress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddress.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面遷移処理（組亜員管理 - 住所情報）
            If TransitionScreen(KIND_ADDRESS) = False Then
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
    '   ＩＤ　：dgvResult_CellDoubleClick()
    '   名称　：グリッドセルダブルクリック処理
    '   概要  ：ダブルクリックした行データの詳細画面を表示する。
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResult.CellDoubleClick

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' ヘッダー行かチェック
            If e.RowIndex <> -1 Then
                ' 画面遷移処理（組合員管理 - 基本情報）
                If TransitionScreen(KIND_BASE) = False Then
                    Exit Sub
                End If
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
    '   ＩＤ　：cfgResult_DoubleClick()
    '   名称　：グリッドキーダウン処理
    '   概要  ：選択されたデータの詳細画面を表示する。
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvResult.KeyDown

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' Enterキーかチェック
            If e.KeyCode = Keys.Enter Then
                ' 画面遷移処理（組合員管理 - 基本情報）
                If TransitionScreen(KIND_BASE) = False Then
                    Exit Sub
                End If
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
    '   ＩＤ　：chkDiplay_Click
    '   名称　：シニア前情報非表示チェックボックスクリック処理
    '   概要　：
    '   作成日：2011/11/21(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub chkDiplay_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSeniorPrevious.Click

        Dim intSeniorCnt As Integer = 0     ' シニア前情報件数

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            If Me.grpSearchResult.Visible Then
                Me.dgvResult.Visible = False
                For i = 0 To Me.dgvResult.RowCount - 1
                    ' シニア前情報非表示チェック有で、シニア前情報のもの（隠し項目がTrueのもの）
                    If Me.chkSeniorPrevious.Checked AndAlso CBool(Me.dgvResult.Rows(i).Cells.Item(12).Value) Then
                        Me.dgvResult.Rows(i).Visible = False
                        intSeniorCnt = intSeniorCnt + 1
                    Else
                        Me.dgvResult.Rows(i).Visible = True
                    End If
                Next i
                Me.dgvResult.Visible = True
                Me.SetTotalCount((Me.dgvResult.RowCount - intSeniorCnt))
            End If

            ' 件数表示
            If SetTotalCount(Me.dgvResult.RowCount - intSeniorCnt) Then
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
    '   ＩＤ　：dgvResult_PreviewKeyDown
    '   名称　：グリッドキーダウン処理
    '   概要　：
    '   作成日：2011/12/07(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/07(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_PreviewKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.PreviewKeyDownEventArgs) Handles dgvResult.PreviewKeyDown

        Try
            If e.KeyCode = Keys.Tab Then
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
    '   ＩＤ　：txtMemberNo_KeyDown
    '   名称　：社員番号テキストボックスキーダウン処理
    '   概要　：
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtMemberNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMemberNo.KeyDown

        Try
            ' データグリッドビュー初期化
            If DataGridViewIni() Then
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
    '   ＩＤ　：txtNameKana_KeyDown
    '   名称　：名前(半角ｶﾅ)テキストボックスキーダウン処理
    '   概要　：
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtNameKana_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNameKana.KeyDown

        Try
            ' データグリッドビュー初期化
            If DataGridViewIni() Then
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

    Private Sub cboUnionMember_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboUnionMember.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor
                ' しばらくお待ちくださいフォーム表示
                FrmWaitInfo.ShowWaitForm(Nothing)

                '-------------------------------------------------------------------------------
                '   グリッド初期化
                '-------------------------------------------------------------------------------
                If DataGridViewIni() = False Then
                    Exit Sub
                End If

                '-------------------------------------------------------------------------------
                '   検索データ取得処理
                '-------------------------------------------------------------------------------
                If GetSearchData() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "cboUnionMember_KeyPress")
            Finally
                ' しばらくお待ちくださいフォーム非表示
                FrmWaitInfo.CloseWaitForm()
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboUnionMember_SelectedIndexChanged
    '   名称　：組合員種別コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2011/12/08(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboUnionMember_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboUnionMember.SelectedIndexChanged

        Try
            ' データグリッドビュー初期化
            If DataGridViewIni() Then
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
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：ByVal pBytEvent As Byte = 0：フォームロード時
    '                                     1：検索結果1件以上
    '                                     2：検索結果0件
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <param name="pBytEvent">イベント</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear(ByVal pBytEvent As Byte) As Boolean

        Dim blnRet As Boolean = False                           ' 処理結果

        Try
            '---------------------------------------------------
            '   検索項目
            '---------------------------------------------------
            If pBytEvent = EVENT_FORM_LOAD Then
                Me.txtMemberNo.Text = ""                        ' 社員番号
                Me.txtNameKana.Text = ""                        ' 名前ｶﾅ
                Me.cboUnionMember.DataSource = Nothing          ' 組合員種別
                Me.cboUnionMember.Text = ""
            End If

            '---------------------------------------------------
            '   検索結果
            '---------------------------------------------------
            If (pBytEvent = EVENT_FORM_LOAD) Or (pBytEvent = EVENT_SEARCH_NODATA) Then
                ' フォームロード or 検索結果0件
                Me.grpSearchResult.Visible = False              ' 検索結果非表示
                Me.dgvResult.Visible = False                    ' DataGridView非表示
                Me.btnSearch.Visible = True                     ' 検索ボタン表示
                Me.btnBase.Visible = False                      ' 基本情報照会ボタン非表示
                Me.btnAddress.Visible = False                   ' 住所情報照会ボタン非表示
                Me.chkSeniorPrevious.Visible = False            ' シニア前情報非表示非表示
                Me.chkSeniorPrevious.Checked = False

            ElseIf pBytEvent = EVENT_SEARCH_DATA Then

                ' 検索結果1件以上
                Me.grpSearchResult.Visible = True               ' 検索結果表示
                Me.dgvResult.Visible = True                     ' DataGridView表示
                Me.btnSearch.Visible = True                     ' 検索ボタン表示
                Me.btnBase.Visible = True                       ' 基本情報照会ボタン非表示
                Me.btnAddress.Visible = True                    ' 住所情報照会ボタン非表示
                Me.chkSeniorPrevious.Visible = True             ' シニア前情報非表示非表示
                Me.chkSeniorPrevious.Checked = True

            End If

            '---------------------------------------------------
            '   権限設定
            '---------------------------------------------------
            ' 登録権限
            If strGrantInsert = GRANT_VALID Then
                Me.btnNewInsert.Enabled = True                  ' 押下可
            ElseIf strGrantInsert = GRANT_VOID Then
                Me.btnNewInsert.Enabled = False                 ' 押下不可
            End If
            ' 印刷権限
            If strGrantPrint = GRANT_VALID Then
                Me.btnMemberManage.Enabled = True               ' 押下可
            ElseIf strGrantPrint = GRANT_VOID Then
                Me.btnMemberManage.Enabled = False              ' 押下不可
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
    '   ＩＤ　：DataGridViewIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
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
                .ColumnCount = 13                                                                   ' 横
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
                .Columns(0).HeaderText = "社員番号"                                                 ' 社員番号
                .Columns(1).HeaderText = "名前"                                                     ' 名前
                .Columns(2).HeaderText = "組合支部"                                                 ' 組合支部
                .Columns(3).HeaderText = "組合員種別"                                               ' 組合員種別
                .Columns(4).HeaderText = "機種"                                                     ' 機種
                .Columns(5).HeaderText = "資格"                                                     ' 資格
                .Columns(6).HeaderText = "ステータス"                                               ' ステータス
                .Columns(7).HeaderText = "会社所属"                                                 ' 会社所属
                .Columns(8).HeaderText = "加入年月日"                                               ' 加入年月日
                .Columns(9).HeaderText = "適用日付"                                                 ' 適用日付
                .Columns(10).HeaderText = "個人認証ID"                                              ' 個人認証ID
                .Columns(11).HeaderText = "会社コード"                                              ' 会社コード
                .Columns(12).HeaderText = "シニア前情報フラグ"                                      ' シニア前情報フラグ
                ' ヘッダー文字位置
                .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 社員番号
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 名前
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 組合支部
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 組合員種別
                .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 機種
                .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 資格
                .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' ステータス
                .Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 会社所属
                .Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 加入年月日
                .Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 適用日付
                .Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 個人認証ID
                .Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 会社コード
                .Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' シニア前情報フラグ

                '-----------------------------------------------------------------------------------
                '   カラム部設定
                '-----------------------------------------------------------------------------------
                ' カラム文字位置
                .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight   ' 社員番号
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 名前
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 組合支部
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 組合員種別
                .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 機種
                .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 資格
                .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' ステータス
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 会社所属
                .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 加入年月日
                .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 適用日付
                .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' 個人認証ID
                .Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' 会社コード
                .Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight  ' シニア前情報フラグ
                ' カラム幅
                .Columns(0).Width = 80                                                              ' 社員番号
                .Columns(1).Width = 120                                                             ' 名前
                .Columns(2).Width = 75                                                              ' 組合支部
                .Columns(3).Width = 80                                                              ' 組合員種別
                .Columns(4).Width = 70                                                              ' 機種
                .Columns(5).Width = 80                                                              ' 資格
                .Columns(6).Width = 75                                                              ' ステータス
                .Columns(7).Width = 80                                                              ' 会社所属
                .Columns(8).Width = 100                                                             ' 加入年月日
                .Columns(9).Width = 100                                                             ' 適用日付
                .Columns(10).Width = 100                                                            ' 個人認証ID
                .Columns(11).Width = 100                                                            ' 会社コード
                .Columns(12).Width = 100                                                            ' シニア前情報フラグ
                ' カラム表示有無
                .Columns(0).Visible = True                                                          ' 社員番号
                .Columns(1).Visible = True                                                          ' 名前
                .Columns(2).Visible = True                                                          ' 組合支部
                .Columns(3).Visible = True                                                          ' 組合員種別
                .Columns(4).Visible = True                                                          ' 機種
                .Columns(5).Visible = True                                                          ' 資格
                .Columns(6).Visible = True                                                          ' ステータス
                .Columns(7).Visible = True                                                          ' 会社所属
                .Columns(8).Visible = True                                                          ' 加入年月日
                .Columns(9).Visible = True                                                          ' 適用日付
                .Columns(10).Visible = False                                                        ' 個人認証ID
                .Columns(11).Visible = False                                                        ' 会社コード
                .Columns(12).Visible = False                                                        ' シニア前情報フラグ
            End With

            ' 検索結果
            If SetTotalCount(0) = False Then
                Exit Function
            End If

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
    '   概要  ：組合員種別コンボボックス作成処理を呼び出す。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim clsDb As New CLAccessMdb        ' データベースクラス

        Try
            ' データベース接続
            Call clsDb.Connect()

            ' 定数マスタ詳細（組合員種別）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, Me.cboUnionMember, CONSTANT_ID_STAF_KIND) = False Then
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
    '   ＩＤ　：GetSearchData
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金)  m.suzuki  新規作成
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
        Dim intSeniorCnt As Integer = 0                 ' シニア前情報件数
        Dim strMemberNo As String = ""                  ' 社員番号
        Dim strNameKana As String = ""                  ' 名前半角カナ
        Dim strUnionMember As String = ""               ' 組合員種別

        Try
            ' 社員番号
            If ChkNull(Me.txtMemberNo.Text.Trim()) = False Then
                strMemberNo = Me.txtMemberNo.Text.Trim()
            End If

            ' 名前半角カナ
            If ChkNull(Me.txtNameKana.Text.Trim()) = False Then
                strNameKana = Me.txtNameKana.Text.Trim()
            End If
            ' 組合員種別
            If Me.cboUnionMember.SelectedIndex > 0 Then
                strUnionMember = Me.cboUnionMember.SelectedValue.ToString()
            End If

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
            strSql = strSql & "                     ) AS stat3" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd0" & vbCrLf
            'strSql = strSql & "                ,ksh" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd1" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd2" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd3" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd4" & vbCrLf
            strSql = strSql & "                ,constant_dtl AS ctd5" & vbCrLf
            strSql = strSql & "           WHERE stat2.c_user_id = stat3.c_user_id" & vbCrLf
            strSql = strSql & "             AND stat2.d_from    = stat3.c_d_from " & vbCrLf
            strSql = strSql & "             AND stat2.k_del     = '0'" & vbCrLf
            ' 社員番号
            If ChkNull(strMemberNo) = False Then
                strSql = strSql & "             AND stat2.c_staf_id LIKE '" & strMemberNo & "%'" & vbCrLf
            End If
            ' 名前(半角ｶﾅ)
            If ChkNull(strNameKana) = False Then
                strSql = strSql & "             AND stat2.l_name_kna LIKE '%" & strNameKana & "%'" & vbCrLf
            End If
            ' 組合員種別
            If ChkNull(strUnionMember) = False Then
                strSql = strSql & "             AND stat2.k_staf_kind = '" & strUnionMember & "'" & vbCrLf
            End If
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
            strSql = strSql & "  ORDER BY CLng(join1.c_user_id)" & UtDb.DbOrderOffset & vbCrLf  'ok
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

                ' コントロールクリア
                If ControlClear(EVENT_SEARCH_DATA) = False Then
                    Return blnRet
                End If

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
                            .Item(8).Value = Format(CDate(tbRet.Rows(i).Item("d_join")), "yyyy/MM/dd")
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

                        ' シニア前情報非表示チェック有で、シニア前情報のもの（隠し項目がTrueのもの）
                        If Me.chkSeniorPrevious.Checked AndAlso CBool(.Item(12).Value) Then
                            Me.dgvResult.Rows(i).Visible = False
                            intSeniorCnt = intSeniorCnt + 1
                        Else
                            Me.dgvResult.Rows(i).Visible = True
                        End If

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

                ' コントロールクリア
                If ControlClear(EVENT_SEARCH_NODATA) = False Then
                    Return blnRet
                End If

                ' 対象データなしメッセージボックス表示
                CLMsg.Show("DI0001")

            End If

            ' グループボックス件数設定
            If SetTotalCount(intRetCnt - intSeniorCnt) = False Then
                Exit Function
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
    '   ＩＤ　：ExistsStafAttribute
    '   名称　：基本情報存在チェック処理
    '   概要  ：
    '   引数　：ByVal pStrUserId  As String = 個人認証ID,
    '           ByVal pStrKsh     As String = 会社コード,
    '           ByVal pStrStafId  As String = 社員番号,
    '           ByVal pStrUseDate As String = 適用日付
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>基本情報存在チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsStafAttribute(ByVal pStrUserId As String, _
                                         ByVal pStrKsh As String, _
                                         ByVal pStrStafId As String, _
                                         ByVal pStrUseDate As String) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsDb As New CLAccessMdb            ' データベースクラス
        Dim strSql As String = ""               ' SQL文
        Dim dtRet As DataTable = Nothing        ' 処理結果格納データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT staf_attribute.c_user_id" & vbCrLf                           ' 01. 個人認証ID
            strSql = strSql & "       ,staf_attribute.c_ksh" & vbCrLf                               ' 02. 会社コード
            strSql = strSql & "       ,staf_attribute.c_staf_id" & vbCrLf                           ' 03. 社員番号
            strSql = strSql & "       ,staf_attribute.d_from" & vbCrLf                              ' 04. 適用日付
            strSql = strSql & "   FROM staf_attribute" & vbCrLf                                     ' 組合員属性
            strSql = strSql & "  WHERE staf_attribute.c_user_id = '" & pStrUserId & "'" & vbCrLf    ' 個人認証IDが同じもの
            strSql = strSql & "    AND staf_attribute.c_ksh = '" & pStrKsh & "'" & vbCrLf           ' 会社コードが同じもの
            strSql = strSql & "    AND staf_attribute.c_staf_id = '" & pStrStafId & "'" & vbCrLf    ' 社員番号が同じもの
            strSql = strSql & "    AND staf_attribute.d_from = '" & pStrUseDate & "'" & vbCrLf      ' 適用日付が同じもの
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' データ存在チェック
            If dtRet.Rows.Count <> 1 Then
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
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsStafAddress
    '   名称　：住所情報存在チェック処理
    '   概要  ：
    '   引数　：ByVal pStrUserId  As String = 個人認証ID,
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>住所情報存在チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ExistsStafAddress(ByVal pStrUserId As String) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsDb As New CLAccessMdb                ' データベースクラス
        Dim strSql As String = ""                   ' SQL文
        Dim dtRet As DataTable = Nothing            ' 処理結果格納データテーブル

        Try
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT staf_address.c_user_id" & vbCrLf                             ' 01. 個人認証ID
            strSql = strSql & "       ,staf_address.s_seq" & vbCrLf                                 ' 02. 住所SEQ
            strSql = strSql & "   FROM staf_address" & vbCrLf                                       ' 組合員住所
            strSql = strSql & "  WHERE staf_address.c_user_id = '" & pStrUserId & "'" & vbCrLf      ' 個人認証IDが同じもの
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            dtRet = clsDb.ExecuteSql(strSql)

            ' データ存在チェック
            If dtRet.Rows.Count = 0 Then
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
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：ByVal bytKind As Byte = 1：組合員管理 - 基本情報, 
    '                                   2：組合員管理 - 住所情報
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/07(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen(ByVal bytKind As Byte) As Boolean

        Dim blnRet As Boolean = False                           ' 処理結果
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)   ' パネルオブジェクト
        Dim clsUC010102 As UC010102 = Nothing                   ' 組合員管理 - 基本情報クラス
        Dim clsUC010103 As UC010103 = Nothing                   ' 組合員管理 - 住所情報クラス
        Dim strUserId As String = ""                            ' 個人認証ID
        Dim strKsh As String = ""                               ' 会社コード
        Dim strStafId As String = ""                            ' 社員番号
        Dim strUseDate As String = ""                           ' 適用日付

        Try
            '-------------------------------------------------------------------------------
            '   グリッド選択チェック
            '-------------------------------------------------------------------------------
            If Me.dgvResult.SelectedRows.Count < 0 Then                                     ' 選択されているかチェック
                Call CLMsg.Show("GW0001", "データ")                                         ' 未選択の場合、エラーメッセージ表示
                Return blnRet
            Else
                ' 各データ取得
                With Me.dgvResult.CurrentRow.Cells
                    strUserId = .Item(10).Value.ToString()                                  ' 個人認証ID
                    strKsh = .Item(11).Value.ToString()                                     ' 会社コード
                    strStafId = .Item(0).Value.ToString()                                   ' 社員番号
                    strUseDate = .Item(9).Value.ToString().Replace("/", "")                 ' 適用日付
                End With
            End If

            ' 基本情報 or 住所情報チェック
            If bytKind = KIND_BASE Then
                '---------------------------------------------------------------------------
                '   基本情報存在チェック
                '---------------------------------------------------------------------------
                If ExistsStafAttribute(strUserId, strKsh, strStafId, strUseDate) = False Then   ' 基本情報存在チェック処理
                    Call CLMsg.Show("GE0090", "基本情報")                                       ' 存在しない場合、エラーメッセージ表示
                    Return blnRet
                End If

                '---------------------------------------------------------------------------
                '   組合員管理 - 基本情報
                '---------------------------------------------------------------------------
                ' 画面間パラメータ情報設定
                clsUC010102 = New UC010102                                                  ' 組合員管理 - 基本情報
                clsUC010102.bytStatus = STATUS_UPDATE                                       ' ステータス（内容変更）
                clsUC010102.bytHistoryFlg = HISTORY_FLG                                     ' 履歴新規登録フラグ
                clsUC010102.strUserId = strUserId                                           ' 個人認証ID
                clsUC010102.strKsh = strKsh                                                 ' 会社コード
                clsUC010102.strStafId = strStafId                                           ' 社員番号
                clsUC010102.strUseDate = strUseDate                                         ' 適用日付
                clsUC010102.strPreScreenId = SCREEN_ID                                      ' 呼び元画面ID
                Call pnl.Controls.Add(clsUC010102)                                          ' 組合員管理 - 基本情報画面表示

            ElseIf bytKind = KIND_ADDRESS Then
                '---------------------------------------------------------------------------
                '   住所情報存在チェック
                '---------------------------------------------------------------------------
                If ExistsStafAddress(strUserId) = False Then                                ' 基本情報存在チェック処理
                    Call CLMsg.Show("GI0002")                                               ' 存在しない場合、エラーメッセージ表示
                    '-----------------------------------------------------------------------
                    '   組合員管理 - 住所情報（新規登録）
                    '-----------------------------------------------------------------------
                    ' 画面間パラメータ情報設定
                    clsUC010103 = New UC010103                                              ' 組合員管理 - 住所情報
                    clsUC010103.bytStatus = STATUS_INSERT                                   ' ステータス（新規登録）
                    clsUC010103.bytHistoryFlg = HISTORY_FLG                                 ' 履歴新規登録フラグ
                    clsUC010103.strUserId = strUserId                                       ' 個人認証ID
                    clsUC010103.strKsh = strKsh                                             ' 会社コード
                    clsUC010103.strStafId = strStafId                                       ' 社員番号
                    clsUC010103.strUseDateAtt = strUseDate                                  ' 適用日付（基本情報）
                    clsUC010103.strPreScreenId = SCREEN_ID                                  ' 呼び元画面ID
                    Call pnl.Controls.Add(clsUC010103)                                      ' 組合員管理 - 基本情報画面表示

                Else
                    '-----------------------------------------------------------------------
                    '   組合員管理 - 住所情報（内容変更）
                    '-----------------------------------------------------------------------
                    ' 画面間パラメータ情報設定
                    clsUC010103 = New UC010103                                              ' 組合員管理 - 住所情報
                    clsUC010103.bytStatus = STATUS_UPDATE                                   ' ステータス（内容変更）
                    clsUC010103.bytHistoryFlg = HISTORY_FLG                                 ' 履歴新規登録フラグ
                    clsUC010103.strUserId = strUserId                                       ' 個人認証ID
                    clsUC010103.strKsh = strKsh                                             ' 会社コード
                    clsUC010103.strStafId = strStafId                                       ' 社員番号
                    clsUC010103.strUseDateAtt = strUseDate                                  ' 適用日付（基本情報）
                    clsUC010103.strPreScreenId = SCREEN_ID                                  ' 呼び元画面ID
                    Call pnl.Controls.Add(clsUC010103)                                      ' 組合員管理 - 基本情報画面表示

                End If
            End If

            ' 組合員検索画面非表示
            Me.Visible = False

            ' 戻り値設定
            blnRet = True

        Catch ex As Exception
            ' パネル非表示
            pnl.Visible = False
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
    '   ＩＤ　：ShowFM010104
    '   名称　：適用日付画面遷移処理
    '   概要  ：適用日付画面遷移を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/08(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>適用日付画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ShowFM010104() As Boolean

        Dim blnRet As Boolean = Nothing                     ' 処理結果
        Dim clsFM010104 As FM010104 = Nothing               ' 適用日付選択画面
        Dim strSql As String = ""                           ' SQL文
        Dim strUseDate As String = ""                       ' 適用日付

        Try
            '---------------------------------------------------
            '   適用日付選択画面表示
            '---------------------------------------------------
            clsFM010104 = New FM010104                                                  ' インスタンス作成
            clsFM010104.Text = TITLE_ENTRY_DATE                                         ' タイトル設定

            ' プロパティ設定
            clsFM010104.strSqlSentence = ""                                             ' SQLなし
            clsFM010104.SetCulumnsName = Nothing                                        ' 適用日付選択画面カラム名
            clsFM010104.SetCulumnsWidth = Nothing                                       ' 適用日付選択画面カラム幅
            clsFM010104.SetCulumnsShow = Nothing                                        ' 適用日付選択画面カラム表示有無
            clsFM010104.ShowInsertDataMessage = True                                    ' 適用日付選択画面
            clsFM010104.EnableChkDirectSpecify = True                                   ' 直接指定使用可能フラグ
            Call clsFM010104.ShowDialog()                                               ' 適用日付選択画面表示メッセージ表示フラグ有
            If clsFM010104.IntQlickBtnFlag = 0 Then                                     ' クリックボタン判定
                If clsFM010104.IsDirectInsert Then                                      ' OKボタン押下で直接指定にチェックがある場合
                    strUseDate = clsFM010104.DirectInsertValue.ToString("yyyyMMdd")     ' 日付取得
                    ' 組合員管理 - 基本情報（新規登録）画面遷移処理
                    If ShowUC010102(strUseDate) = False Then
                        Exit Function
                    End If
                End If
            End If

            ' 不要になった時点で破棄
            clsFM010104.Close()
            clsFM010104.Dispose()

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
    '   ＩＤ　：ShowUC010102
    '   名称　：組合員管理 - 基本情報（新規登録）画面遷移処理
    '   概要  ：適用日付を引き継いで組合員管理 - 基本情報（新規登録）画面遷移を行う。
    '   引数　：ByVal pStrUseDate As String = 適用日付
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/08(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員管理 - 基本情報（新規登録）画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ShowUC010102(ByVal pStrUseDate As String) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim pnl As Panel                            ' パネルオブジェクト
        Dim clsUC010102 As UC010102 = Nothing       ' 組合員管理 - 基本情報クラス

        Try
            Me.Visible = False
            pnl = ParentForm.Controls(MAIN_PANEL_ID)
            '---------------------------------------------------------------------------
            '   組合員管理 - 基本情報
            '---------------------------------------------------------------------------
            clsUC010102 = pnl.Controls(SCREEN_ID_UC010102)
            clsUC010102 = New UC010102                                                  ' 組合員管理 - 基本情報
            ' 画面間パラメータ情報設定
            clsUC010102.bytStatus = STATUS_INSERT                                       ' ステータス（1：新規登録）
            clsUC010102.bytHistoryFlg = HISTORY_FLG                                     ' 履歴新規登録フラグ
            clsUC010102.strUserId = ""                                                  ' 個人認証ID
            clsUC010102.strKsh = ""                                                     ' 会社コード
            clsUC010102.strStafId = ""                                                  ' 社員番号
            clsUC010102.strUseDate = pStrUseDate                                        ' 適用日付
            Call pnl.Controls.Add(clsUC010102)                                          ' パネルに組合員管理 - 基本情報画面を設定

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
    '   ＩＤ　：setGrant
    '   名称　：権限処理
    '   概要  ：権限を取得してボタン制御を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function setGrant() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim dtGrant As DataTable = Nothing          ' 権限取得データテーブル

        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC010101)
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
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetTotalCount
    '   名称　：件数設定処理
    '   概要  ：検索件数を設定する。
    '   引数　：ByVal iRowsCount As Integer = 検索件数
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/23(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>件数設定処理</summary>
    ''' <param name="iRowsCount"></param>
    ''' <remarks></remarks>
    Private Function SetTotalCount(ByVal iRowsCount As Integer) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            Me.grpSearchResult.Text = ("検索結果" & "( " & iRowsCount.ToString & " " & "件" & ")")

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

    Private Sub txtMemberNo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMemberNo.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor
                ' しばらくお待ちくださいフォーム表示
                FrmWaitInfo.ShowWaitForm(Nothing)

                '-------------------------------------------------------------------------------
                '   グリッド初期化
                '-------------------------------------------------------------------------------
                If DataGridViewIni() = False Then
                    Exit Sub
                End If

                '-------------------------------------------------------------------------------
                '   検索データ取得処理
                '-------------------------------------------------------------------------------
                If GetSearchData() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "txtMemberNo_KeyPress")
            Finally
                ' しばらくお待ちくださいフォーム非表示
                FrmWaitInfo.CloseWaitForm()
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    Private Sub txtNameKana_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNameKana.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor
                ' しばらくお待ちくださいフォーム表示
                FrmWaitInfo.ShowWaitForm(Nothing)

                '-------------------------------------------------------------------------------
                '   グリッド初期化
                '-------------------------------------------------------------------------------
                If DataGridViewIni() = False Then
                    Exit Sub
                End If

                '-------------------------------------------------------------------------------
                '   検索データ取得処理
                '-------------------------------------------------------------------------------
                If GetSearchData() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                ' ログ出力（致命的エラー）
                log.Fatal(ex.Message)
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "txtNameKana_KeyPress")
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
