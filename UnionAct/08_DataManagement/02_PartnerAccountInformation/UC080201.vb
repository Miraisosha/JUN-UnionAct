'===========================================================================================================
'   クラスＩＤ　　：UC080201
'   クラス名称　　：組合員口座情報
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Document

Public Class UC080201

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC080201                                  ' UC080201
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC080201                              ' 組合員口座情報画面
    ' ステータス
    Private Const STATUS_INSERT As Byte = 1                                                 ' 新規登録
    Private Const STATUS_UPDATE As Byte = 2                                                 ' 内容変更
    ' イベント
    Private Const EVENT_FORM_LOAD As Byte = 0                                               ' フォームロード
    Private Const EVENT_SEARCH_DATA As Byte = 1                                             ' 検索結果1件以上
    Private Const EVENT_SEARCH_NODATA As Byte = 2                                           ' 検索結果0件
    ' タイトル
    Private Const TITLE_SELECT_USE_DATE As String = "組合員口座情報履歴 - 適用日付選択画面" ' 適用日付選択画面
    ' 権限
    Private strGrantReference As String = "0"                                               ' 参照権限
    Private strGrantInsert As String = "0"                                                  ' 登録権限
    Private strGrantPrint As String = "0"                                                   ' 印刷権限
    Private strGrantFileOutput As String = "0"                                              ' ファイル出力権限
#End Region

#Region "プロパティ"
    Public _blnSearchFlg As Boolean = False                 ' 再検索フラグ（True：再検索有り, False：再検索無し）

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
    '   ＩＤ　：UC080201_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC080201_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
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
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：UC080201_VisibleChanged
    '   名称　：フォーム表示チェンジ処理
    '   概要  ：
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC080201_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged

        Try
            ' フォーム表示で検索フラグがTrueの場合、再検索
            ' 組合口座情報 - 詳細画面更新処理（新規登録・内容変更）後に再検索処理を行う。
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
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要  ：
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' しばらくお待ちくださいフォーム表示
            Call FrmWaitInfo.ShowWaitForm(Nothing)

            '-------------------------------------------------------------------------------
            '   グリッド初期化処理
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
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "btnSearch_Click")
        Finally
            ' しばらくお待ちくださいフォームクローズ
            Call FrmWaitInfo.CloseWaitForm()
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnDetail_Click
    '   名称　：詳細ボタンクリック処理
    '   概要  ：
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetail.Click

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' 画面遷移処理（組合員管理 - 住所情報）
            If TransitionScreen() = False Then
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
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：chkSenior_Click
    '   名称　：シニア前情報非表示チェックボックスクリック処理
    '   概要  ：
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub chkSenior_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSenior.Click

        Try
            ' 表示非表示処理
            If DisplayOnOff() Then
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
    '   ＩＤ　：chkAccount_Click
    '   名称　：口座登録済情報非表示チェックボックスクリック処理
    '   概要  ：
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub chkAccount_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAccount.Click

        Try
            ' 表示非表示処理
            If DisplayOnOff() Then
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
    '   ＩＤ　：txtStafId_KeyPress
    '   名称　：社員番号テキストボックスキープレス処理
    '   概要  ：数値以外入力無効
    '   作成日：2012/01/06(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtStafId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtStafId.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' しばらくお待ちくださいフォーム表示
                Call FrmWaitInfo.ShowWaitForm(Nothing)

                '-------------------------------------------------------------------------------
                '   グリッド初期化処理
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
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "txtStafId_KeyPress")
            Finally
                ' しばらくお待ちくださいフォームクローズ
                Call FrmWaitInfo.CloseWaitForm()
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtNameKana_KeyPress
    '   名称　：名前（半角カナ）テキストボックスキープレス処理
    '   概要  ：
    '   作成日：2012/01/10(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtNameKana_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNameKana.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' しばらくお待ちくださいフォーム表示
                Call FrmWaitInfo.ShowWaitForm(Nothing)

                '-------------------------------------------------------------------------------
                '   グリッド初期化処理
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
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "txtNameKana_KeyPress")
            Finally
                ' しばらくお待ちくださいフォームクローズ
                Call FrmWaitInfo.CloseWaitForm()
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtNameKana_GotFocus
    '   名称　：名前（半角カナ）テキストボックスフォーカス取得処理
    '   概要  ：名前（半角カナ）テキストボックスを全選択する。
    '   作成日：2012/01/10(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtNameKana_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNameKana.GotFocus

        Try
            Me.txtNameKana.SelectAll()

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
    '   ＩＤ　：txtStafId_GotFocus
    '   名称　：社員番号テキストボックスフォーカス取得処理
    '   概要  ：社員番号テキストボックスを全選択する。
    '   作成日：2012/01/10(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtStafId_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStafId.GotFocus

        Try
            Me.txtStafId.SelectAll()

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

    Private Sub cboStafKind_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboStafKind.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' しばらくお待ちくださいフォーム表示
                Call FrmWaitInfo.ShowWaitForm(Nothing)

                '-------------------------------------------------------------------------------
                '   グリッド初期化処理
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
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "cboStafKind_KeyPress")
            Finally
                ' しばらくお待ちくださいフォームクローズ
                Call FrmWaitInfo.CloseWaitForm()
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboStafKind_SelectedIndexChanged
    '   名称　：組合員種別コンボボックスチェンジ処理
    '   概要  ：検索結果をクリアする。
    '   作成日：2012/01/10(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboStafKind_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStafKind.SelectedIndexChanged

        Try
            ' 検索結果クリア
            If ControlClear(EVENT_SEARCH_NODATA) Then
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

    Private Sub cboStatus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboStatus.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' カーソルを砂時計に設定
                Cursor.Current = Cursors.WaitCursor

                ' しばらくお待ちくださいフォーム表示
                Call FrmWaitInfo.ShowWaitForm(Nothing)

                '-------------------------------------------------------------------------------
                '   グリッド初期化処理
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
                ' 致命的エラーメッセージボックス表示
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, "cboStatus_KeyPress")
            Finally
                ' しばらくお待ちくださいフォームクローズ
                Call FrmWaitInfo.CloseWaitForm()
                ' カーソルを矢印に戻す
                Cursor.Current = Cursors.Default
            End Try
        End If

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboStatus_SelectedIndexChanged
    '   名称　：ステータスコンボボックスチェンジ処理
    '   概要  ：検索結果をクリアする。
    '   作成日：2012/01/10(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboStatus_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboStatus.SelectedIndexChanged

        Try
            ' 検索結果クリア
            If ControlClear(EVENT_SEARCH_NODATA) Then
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
    '   ＩＤ　：dgvResult_CellDoubleClick()
    '   名称　：グリッドセルダブルクリック処理
    '   概要  ：ダブルクリックした行データの詳細画面を表示する。
    '   作成日：2012/01/19(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvResult.CellDoubleClick

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' ヘッダー行かチェック
            If e.RowIndex <> -1 Then
                ' 画面遷移処理（組合員口座情報 - 詳細）
                If TransitionScreen() = False Then
                    Exit Sub
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
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cfgResult_DoubleClick()
    '   名称　：グリッドキーダウン処理
    '   概要  ：選択されたデータの詳細画面を表示する。
    '   作成日：2011/11/07(月) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub dgvResult_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvResult.KeyDown

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            ' Enterキーかチェック
            If e.KeyCode = Keys.Enter Then
                ' 画面遷移処理（組合員管理 - 基本情報）
                If TransitionScreen() = False Then
                    Exit Sub
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
        Finally
            ' カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
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
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <param name="pBytEvent">イベント</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear(ByVal pBytEvent As Byte) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            '---------------------------------------------------
            '   検索項目
            '---------------------------------------------------
            If pBytEvent = EVENT_FORM_LOAD Then
                Me.txtStafId.Text = ""                          ' 社員番号
                Me.txtNameKana.Text = ""                        ' 名前ｶﾅ
                Me.cboStafKind.DataSource = Nothing             ' 組合員種別
                Me.cboStafKind.Text = ""
                Me.cboStatus.DataSource = Nothing               ' ステータス
                Me.cboStatus.Text = ""
            End If

            '---------------------------------------------------
            '   検索結果
            '---------------------------------------------------
            If (pBytEvent = EVENT_FORM_LOAD) Or (pBytEvent = EVENT_SEARCH_NODATA) Then
                ' フォームロード or 検索結果0件
                Me.grpResult.Visible = False                    ' 検索結果非表示
                Me.dgvResult.Visible = False                    ' DataGridView非表示
                Me.btnSearch.Visible = True                     ' 検索ボタン表示
                Me.btnDetail.Visible = False                    ' 詳細ボタン表示
                Me.chkSenior.Visible = False                    ' シニア前情報非表示非表示
                Me.chkSenior.Checked = False
                Me.chkAccount.Visible = False                   ' 口座登録済情報非表示非表示
                Me.chkAccount.Checked = False
            ElseIf pBytEvent = EVENT_SEARCH_DATA Then
                ' 検索結果1件以上
                Me.grpResult.Visible = True                     ' 検索結果表示
                Me.dgvResult.Visible = True                     ' DataGridView表示
                Me.btnSearch.Visible = True                     ' 検索ボタン表示
                Me.btnDetail.Visible = True                     ' 詳細ボタン表示
                Me.chkSenior.Visible = True                     ' シニア前情報非表示表示
                Me.chkSenior.Checked = True
                Me.chkAccount.Visible = True                    ' 口座登録済情報非表示表示
                Me.chkAccount.Checked = False
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
    '   ＩＤ　：DataGridViewIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
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
                .ColumnCount = 17                                                                   ' 横
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
                .Columns(0).HeaderText = "社員番号"                                                 ' 01. 社員番号
                .Columns(1).HeaderText = "CD"                                                       ' 02. CD
                .Columns(2).HeaderText = "名前"                                                     ' 03. 名前
                .Columns(3).HeaderText = "組合支部"                                                 ' 04. 組合支部
                .Columns(4).HeaderText = "組合員種別"                                               ' 05. 組合員種別
                .Columns(5).HeaderText = "機種"                                                     ' 06. 機種
                .Columns(6).HeaderText = "ステータス"                                               ' 07. ステータス
                .Columns(7).HeaderText = "会社所属"                                                 ' 08. 会社所属
                .Columns(8).HeaderText = "口座登録状況"                                             ' 09. 口座登録状況
                .Columns(9).HeaderText = "シニア前情報フラグ"                                       ' 10. シニア前情報フラグ
                .Columns(10).HeaderText = "個人認証ID"                                              ' 11. 組合員口座情報の個人認証ID
                .Columns(11).HeaderText = "適用日付"                                                ' 12. 組合員口座情報の適用日付
                .Columns(12).HeaderText = "金融機関コード"                                          ' 13. 金融機関コード
                .Columns(13).HeaderText = "支店番号"                                                ' 14. 支店番号
                .Columns(14).HeaderText = "個人認証ID"                                              ' 15. 組合員属性情報の個人認証ID
                .Columns(15).HeaderText = "会社コード"                                              ' 16. 会社コード
                .Columns(16).HeaderText = "適用日付"                                                ' 17. 組合員属性情報の適用日付
                ' ヘッダー文字位置
                .Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 01. 社員番号
                .Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 02. CD
                .Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 03. 名前
                .Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 04. 組合支部
                .Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 05. 組合員種別
                .Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 06. 機種
                .Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 07. ステータス
                .Columns(7).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 08. 会社所属
                .Columns(8).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 09. 口座登録状況
                .Columns(9).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 10. シニア前情報フラグ
                .Columns(10).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 11. 組合員口座情報の個人認証ID
                .Columns(11).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 12. 組合員口座情報の適用日付
                .Columns(12).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 13. 金融機関コード
                .Columns(13).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 14. 支店番号
                .Columns(14).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 15. 組合員属性情報の個人認証ID
                .Columns(15).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 16. 会社コード
                .Columns(16).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter ' 17. 組合員属性情報の適用日付

                '-----------------------------------------------------------------------------------
                '   カラム部設定
                '-----------------------------------------------------------------------------------
                ' カラム文字位置
                .Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight   ' 01. 社員番号
                .Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 02. CD
                .Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 03. 名前
                .Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 04. 組合支部
                .Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft    ' 05. 組合員種別
                .Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 06. 機種
                .Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 07. ステータス
                .Columns(7).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 08. 会社所属
                .Columns(8).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 09. 口座登録状況
                .Columns(9).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter  ' 10. シニア前情報フラグ
                .Columns(10).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight  ' 11. 組合員口座情報の個人認証ID
                .Columns(11).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter ' 12. 組合員口座情報の適用日付
                .Columns(12).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 13. 金融機関コード
                .Columns(13).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 14. 支店番号
                .Columns(14).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 15. 組合員属性情報の個人認証ID
                .Columns(15).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 16. 会社コード
                .Columns(16).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft   ' 17. 組合員属性情報の適用日付
                ' カラム幅
                .AutoResizeColumn(0)                                                                ' 01. 社員番号
                .AutoResizeColumn(1)                                                                ' 02. CD
                .AutoResizeColumn(2)                                                                ' 03. 名前
                .AutoResizeColumn(3)                                                                ' 04. 組合支部
                .AutoResizeColumn(4)                                                                ' 05. 組合員種別
                .AutoResizeColumn(5)                                                                ' 06. 機種
                .AutoResizeColumn(6)                                                                ' 07. ステータス
                .AutoResizeColumn(7)                                                                ' 08. 会社所属
                .AutoResizeColumn(8)                                                                ' 09. 口座登録状況
                .AutoResizeColumn(9)                                                                ' 10. シニア前情報フラグ
                .AutoResizeColumn(10)                                                               ' 11. 組合員口座情報の個人認証ID
                .AutoResizeColumn(11)                                                               ' 12. 組合員口座情報の適用日付
                .AutoResizeColumn(12)                                                               ' 13. 金融期間コード
                .AutoResizeColumn(13)                                                               ' 14. 支店番号
                .AutoResizeColumn(14)                                                               ' 15. 組合員属性情報の個人認証ID
                .AutoResizeColumn(15)                                                               ' 16. 会社コード
                .AutoResizeColumn(16)                                                               ' 17. 組合員属性情報の適用日付
                '.Columns(0).Width = 100                                                            ' 01. 社員番号
                '.Columns(1).Width = 30                                                              ' 02. CD
                '.Columns(2).Width = 100                                                             ' 03. 名前
                '.Columns(3).Width = 100                                                             ' 04. 組合支部
                '.Columns(4).Width = 130                                                             ' 05. 組合員種別
                '.Columns(5).Width = 100                                                             ' 06. 機種
                '.Columns(6).Width = 100                                                             ' 07. ステータス
                '.Columns(7).Width = 100                                                             ' 08. 会社所属
                '.Columns(8).Width = 130                                                             ' 09. 口座登録状況
                '.Columns(9).Width = 300                                                             ' 10. シニア前情報フラグ
                '.Columns(10).Width = 300                                                            ' 11. 組合員口座情報の個人認証ID
                '.Columns(11).Width = 300                                                            ' 12. 組合員口座情報の適用日付
                '.Columns(12).Width = 300                                                            ' 13. 金融期間コード
                '.Columns(13).Width = 300                                                            ' 14. 支店番号
                '.Columns(14).Width = 300                                                            ' 15. 組合員属性情報の個人認証ID
                '.Columns(15).Width = 300                                                            ' 16. 会社コード
                '.Columns(16).Width = 300                                                            ' 17. 組合員属性情報の適用日付
                ' カラム表示有無
                .Columns(0).Visible = True                                                          ' 01. 社員番号
                .Columns(1).Visible = True                                                          ' 02. CD
                .Columns(2).Visible = True                                                          ' 03. 名前
                .Columns(3).Visible = True                                                          ' 04. 組合支部
                .Columns(4).Visible = True                                                          ' 05. 組合員種別
                .Columns(5).Visible = True                                                          ' 06. 機種
                .Columns(6).Visible = True                                                          ' 07. ステータス
                .Columns(7).Visible = True                                                          ' 08. 会社所属
                .Columns(8).Visible = True                                                          ' 09. 口座登録状況
                .Columns(9).Visible = False                                                         ' 10. シニア前情報フラグ
                .Columns(10).Visible = False                                                        ' 11. 組合員口座情報の個人認証ID
                .Columns(11).Visible = False                                                        ' 12. 組合員口座情報の適用日付
                .Columns(12).Visible = False                                                        ' 13. 金融機関コード
                .Columns(13).Visible = False                                                        ' 14. 支店番号
                .Columns(14).Visible = False                                                        ' 15. 組合員属性情報の個人認証ID
                .Columns(15).Visible = False                                                        ' 16. 会社コード
                .Columns(16).Visible = False                                                        ' 17. 組合員属性情報の適用日付
            End With

            ' 検索結果
            If SetTotalCount(0) = False Then
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
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsDb As New CLAccessMdb            ' データベースクラス

        Try
            ' データベース接続
            Call clsDb.Connect()

            ' 定数マスタ詳細（組合員種別）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, Me.cboStafKind, CONSTANT_ID_STAF_KIND) = False Then
                Return blnRet
            End If

            ' 定数マスタ詳細（スタータス）コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(clsDb, Me.cboStatus, CONSTANT_ID_USER_STATUS) = False Then
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

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetSearchData
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>検索データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSearchData() As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim strSql As String = ""                       ' SQL文
        Dim clsDb As New CLAccessMdb                    ' データベースクラス生成
        Dim tbRet As DataTable = Nothing                ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                    ' 検索結果件数
        Dim intSeniorCnt As Integer = 0                 ' シニア前情報件数
        Dim intAccountCnt As Integer = 0                ' 口座登録済情報件数
        Dim clrBack As Color = Nothing                  ' データバックカラー

        Try
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT attrd.c_staf_id as c_staf_id" & vbCrLf                                   ' 01. 社員番号
            strSql = strSql & "       ,attrd.c_dezit   as c_dezit" & vbCrLf                                     ' 02. CD
            strSql = strSql & "       ,attrd.l_name    as l_name" & vbCrLf                                      ' 03. 名前
            strSql = strSql & "       ,attrd.l_name1   AS k_belonging" & vbCrLf                                 ' 04. 組合支部
            strSql = strSql & "       ,attrd.l_name2   AS k_staf_kind" & vbCrLf                                 ' 05. 組合員種別
            strSql = strSql & "       ,attrd.l_name3   AS k_model" & vbCrLf                                     ' 06. 機種
            strSql = strSql & "       ,attrd.l_name4   AS k_user_status" & vbCrLf                               ' 07. スタータス
            strSql = strSql & "       ,attrd.l_name5   AS k_local" & vbCrLf                                     ' 08. 会社所属
            'strSql = strSql & "       ,attrd.l_name5   AS k_local" & vbCrLf                                     ' 08. 会社所属
            strSql = strSql & "       ,IIF( acntd.c_user_id <> '', '有' ,'未' ) AS AccountFlg" & vbCrLf         ' 09. 口座登録状況フラグ
            strSql = strSql & "       ,IIF( attrdf.c_user_id <> '', 'True' ,'False' ) AS SeniorFlg" & vbCrLf    ' 10. シニア前情報フラグ
            strSql = strSql & "       ,acntd.c_user_id     AS acnt_c_user_id"                                   ' 11. 組合員口座情報の個人認証ID
            strSql = strSql & "       ,acntd.d_from        AS acnt_d_from"                                      ' 12. 組合員口座情報の適用日付
            strSql = strSql & "       ,acntd.c_bank        AS c_bank" & vbCrLf                                  ' 13. 金融機関コード
            strSql = strSql & "       ,acntd.c_bank_office AS c_bank_office" & vbCrLf                           ' 14. 支店番号
            strSql = strSql & "       ,attrd.c_user_id AS attr_c_user_id" & vbCrLf                              ' 15. 組合員属性情報の個人認証ID
            strSql = strSql & "       ,attrd.c_ksh AS c_ksh" & vbCrLf                                           ' 16. 会社コード
            strSql = strSql & "       ,attrd.d_from AS attr_d_from" & vbCrLf                                    ' 17. 組合員属性情報の適用日付
            strSql = strSql & "   FROM (( SELECT attrb.c_user_id" & vbCrLf
            strSql = strSql & "                 ,attrb.c_ksh" & vbCrLf
            strSql = strSql & "                 ,attrb.c_staf_id" & vbCrLf
            strSql = strSql & "                 ,attrb.d_from" & vbCrLf
            strSql = strSql & "                 ,attrb.c_dezit" & vbCrLf
            strSql = strSql & "                 ,attrb.l_name" & vbCrLf
            strSql = strSql & "                 ,ctd1.l_name     AS l_name1" & vbCrLf
            strSql = strSql & "                 ,ctd2.l_name     AS l_name2" & vbCrLf
            strSql = strSql & "                 ,ctd3.l_name     AS l_name3" & vbCrLf
            strSql = strSql & "                 ,ctd4.l_name     AS l_name4" & vbCrLf
            strSql = strSql & "                 ,ctd5.l_name     AS l_name5" & vbCrLf
            'strSql = strSql & "                 ,ctd5.n_ksh      AS l_name5" & vbCrLf
            strSql = strSql & "             FROM staf_attribute AS attrb" & vbCrLf
            strSql = strSql & "                 ,( SELECT attra.c_user_id" & vbCrLf
            'strSql = strSql & "                          ,attra.c_ksh" & vbCrLf
            strSql = strSql & "                          ,attra.c_staf_id" & vbCrLf
            strSql = strSql & "                          ,max( attra.d_from ) AS d_from" & vbCrLf
            strSql = strSql & "                      FROM staf_attribute AS attra" & vbCrLf
            strSql = strSql & "                     WHERE attra.k_del = '0'" & vbCrLf
            ' 社員番号
            If ChkNull(Me.txtStafId.Text.Trim) = False Then
                strSql = strSql & "                       AND attra.c_staf_id LIKE '" & Me.txtStafId.Text.Trim & "%'" & vbCrLf
            End If
            ' 名前(半角ｶﾅ)
            If ChkNull(Me.txtNameKana.Text.Trim) = False Then
                strSql = strSql & "                       AND attra.l_name_kna LIKE '%" & Me.txtNameKana.Text.Trim & "%'" & vbCrLf
            End If
            ' 組合員種別
            If Me.cboStafKind.SelectedIndex > 0 Then
                strSql = strSql & "                       AND attra.k_staf_kind = '" & Me.cboStafKind.SelectedValue.ToString() & "'" & vbCrLf
            End If
            ' ステータス
            If Me.cboStatus.SelectedIndex > 0 Then
                strSql = strSql & "                       AND attra.k_user_status = '" & Me.cboStatus.SelectedValue.ToString() & "'" & vbCrLf
            End If
            strSql = strSql & "                     GROUP BY attra.c_user_id" & vbCrLf
            'strSql = strSql & "                             ,attra.c_ksh" & vbCrLf
            strSql = strSql & "                             ,attra.c_staf_id ) AS attrc" & vbCrLf
            strSql = strSql & "                 ,constant_dtl AS ctd1" & vbCrLf
            strSql = strSql & "                 ,constant_dtl AS ctd2" & vbCrLf
            strSql = strSql & "                 ,constant_dtl AS ctd3" & vbCrLf
            strSql = strSql & "                 ,constant_dtl AS ctd4" & vbCrLf
            strSql = strSql & "                 ,constant_dtl AS ctd5" & vbCrLf
            'strSql = strSql & "                 ,ksh          AS ctd5" & vbCrLf
            strSql = strSql & "            WHERE attrb.c_user_id     = attrc.c_user_id" & vbCrLf
            'strSql = strSql & "              AND attrb.c_ksh         = attrc.c_ksh" & vbCrLf
            strSql = strSql & "              AND attrb.c_staf_id     = attrc.c_staf_id" & vbCrLf
            strSql = strSql & "              AND attrb.d_from        = attrc.d_from" & vbCrLf
            strSql = strSql & "              AND attrb.k_del         = '0'" & vbCrLf
            strSql = strSql & "              AND ctd1.c_constant     = 'BELONGING'" & vbCrLf
            strSql = strSql & "              AND ctd1.c_constant_seq = attrb.k_belonging" & vbCrLf
            strSql = strSql & "              AND ctd2.c_constant     = 'STAF_KIND'" & vbCrLf
            strSql = strSql & "              AND ctd2.c_constant_seq = attrb.k_staf_kind" & vbCrLf
            strSql = strSql & "              AND ctd3.c_constant     = 'MODEL'" & vbCrLf
            strSql = strSql & "              AND ctd3.c_constant_seq = attrb.k_model" & vbCrLf
            strSql = strSql & "              AND ctd4.c_constant     = 'USER_STATUS'" & vbCrLf
            strSql = strSql & "              AND ctd4.c_constant_seq = attrb.k_user_status" & vbCrLf
            strSql = strSql & "              AND ctd5.c_constant     = 'AREA_LOCAL'" & vbCrLf
            strSql = strSql & "              AND ctd5.c_constant_seq = attrb.k_local" & vbCrLf
            'strSql = strSql & "              AND attrb.c_ksh         = ctd5.c_ksh" & vbCrLf
            ' 社員番号
            If ChkNull(Me.txtStafId.Text.Trim) = False Then
                strSql = strSql & "              AND attrb.c_staf_id LIKE '" & Me.txtStafId.Text.Trim & "%'" & vbCrLf
            End If
            ' 名前(半角ｶﾅ)
            If ChkNull(Me.txtNameKana.Text.Trim) = False Then
                strSql = strSql & "              AND attrb.l_name_kna LIKE '%" & Me.txtNameKana.Text.Trim & "%'" & vbCrLf
            End If
            ' 組合員種別
            If Me.cboStafKind.SelectedIndex > 0 Then
                strSql = strSql & "              AND attrb.k_staf_kind = '" & Me.cboStafKind.SelectedValue.ToString() & "'" & vbCrLf
            End If
            ' ステータス
            If Me.cboStatus.SelectedIndex > 0 Then
                If ChkNull(Me.cboStatus.SelectedValue.ToString()) = False Then
                    strSql = strSql & "              AND attrb.k_user_status = '" & Me.cboStatus.SelectedValue.ToString() & "'" & vbCrLf
                End If
            End If
            strSql = strSql & "            ORDER BY attrb.c_user_id " & UtDb.DbOrderOffset & " ) AS attrd" & vbCrLf
            ' 口座情報登録済
            strSql = strSql & "        LEFT JOIN ( SELECT acntb.c_user_id" & vbCrLf
            strSql = strSql & "                          ,acntb.d_from" & vbCrLf
            strSql = strSql & "                          ,acntb.c_bank" & vbCrLf
            strSql = strSql & "                          ,acntb.c_bank_office" & vbCrLf
            strSql = strSql & "                      FROM staf_account AS acntb" & vbCrLf
            strSql = strSql & "                          ,( SELECT acnta.c_user_id" & vbCrLf
            strSql = strSql & "                                   ,max(acnta.d_from) as d_from" & vbCrLf
            strSql = strSql & "                               FROM staf_account AS acnta" & vbCrLf
            strSql = strSql & "                              GROUP BY acnta.c_user_id ) AS acntc" & vbCrLf
            strSql = strSql & "                     WHERE acntb.c_user_id = acntc.c_user_id" & vbCrLf
            strSql = strSql & "                       AND acntb.d_from = acntc.d_from ) AS acntd" & vbCrLf
            strSql = strSql & "        ON attrd.c_user_id = acntd.c_user_id )" & vbCrLf
            ' シニア前情報
            strSql = strSql & "            LEFT JOIN ( SELECT attrde.c_user_id" & vbCrLf
            strSql = strSql & "                              ,attrde.c_staf_id_old" & vbCrLf
            strSql = strSql & "                          FROM staf_attribute AS attrde" & vbCrLf
            strSql = strSql & "                         WHERE attrde.k_del = '0'" & vbCrLf
            strSql = strSql & "                           AND attrde.c_staf_id_old <> attrde.c_user_id" & vbCrLf
            strSql = strSql & "                           AND attrde.c_staf_id_old <> '' ) AS attrdf" & vbCrLf
            strSql = strSql & "            ON attrd.c_user_id = attrdf.c_staf_id_old" & vbCrLf
            ' ORDER BY句（社員番号で並替）
            strSql = strSql & "  ORDER BY CLng( attrd.c_staf_id )" & UtDb.DbOrderOffset & vbCrLf    'ok
            strSql = strSql & ";" & vbCrLf

            ' データベース接続
            Call clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRetCnt = tbRet.Rows.Count

            ' 件数件数チェック
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
                        .Item(1).Value = NVL(tbRet.Rows(i).Item("c_dezit"))                             ' 02. CD
                        .Item(2).Value = NVL(tbRet.Rows(i).Item("l_name"))                              ' 03. 名前
                        .Item(3).Value = NVL(tbRet.Rows(i).Item("k_belonging"))                         ' 04. 組合支部
                        .Item(4).Value = NVL(tbRet.Rows(i).Item("k_staf_kind"))                         ' 05. 組合員種別
                        .Item(5).Value = NVL(tbRet.Rows(i).Item("k_model"))                             ' 06. 機種
                        .Item(6).Value = NVL(tbRet.Rows(i).Item("k_user_status"))                       ' 07. スタータス
                        .Item(7).Value = NVL(tbRet.Rows(i).Item("k_local"))                             ' 08. 会社所属
                        .Item(8).Value = NVL(tbRet.Rows(i).Item("AccountFlg"))                          ' 09. 口座登録状況
                        .Item(9).Value = NVL(tbRet.Rows(i).Item("SeniorFlg"))                           ' 10. シニア前情報フラグ
                        .Item(10).Value = NVL(tbRet.Rows(i).Item("acnt_c_user_id"))                     ' 11. 組合員口座情報の個人認証ID
                        ' 12. 組合員口座情報の適用日付
                        If IsDBNull(tbRet.Rows(i).Item("acnt_d_from")) Then
                            .Item(11).Value = ""
                        Else
                            .Item(11).Value = Date.Parse(Format(CInt(tbRet.Rows(i).Item("acnt_d_from").ToString()), "0000/00/00")).ToString("yyyy/MM/dd")
                        End If
                        .Item(12).Value = NVL(tbRet.Rows(i).Item("c_bank"))                             ' 13. 金融機関コード
                        .Item(13).Value = NVL(tbRet.Rows(i).Item("c_bank_office"))                      ' 14. 支店番号
                        .Item(14).Value = NVL(tbRet.Rows(i).Item("attr_c_user_id"))                     ' 15. 組合員属性情報の個人認証ID
                        .Item(15).Value = NVL(tbRet.Rows(i).Item("c_ksh"))                              ' 16. 会社コード
                        .Item(16).Value = NVL(tbRet.Rows(i).Item("attr_d_from"))                        ' 17. 組合員属性情報の適用日付
                        ' 口座登録済情報非表示チェック有で、口座登録済情報のもの（口座登録状況が "未" のもの）
                        If Me.chkAccount.Checked AndAlso .Item(8).Value.ToString() = "未" Then
                            Me.dgvResult.Rows(i).Visible = False
                            intAccountCnt = intAccountCnt + 1
                        Else
                            Me.dgvResult.Rows(i).Visible = True
                        End If
                        ' シニア前情報非表示チェック有で、シニア前情報のもの（隠し項目が "True" のもの）
                        If Me.chkSenior.Checked AndAlso CBool(.Item(9).Value.ToString()) Then
                            Me.dgvResult.Rows(i).Visible = False
                            intSeniorCnt = intSeniorCnt + 1
                        End If
                        ' バックカラー設定
                        If IsDBNull(tbRet.Rows(i).Item("acnt_d_from")) = False Then
                            If tbRet.Rows(i).Item("acnt_d_from").ToString() > Now.ToString("yyyyMMdd") Then
                                clrBack = Color.LightGreen                                              ' 組合員口座情報の適用日付が未来日の場合、バックカラーライトグリーン
                            Else
                                clrBack = Color.White                                                   ' 組合員口座情報の適用日付が未来日ではない場合、バックカラーホワイト
                            End If
                        Else
                            clrBack = Color.White                                                       ' 組合員口座情報の適用日付がない場合、バックカラーホワイト
                        End If
                        For j = 0 To 13
                            Me.dgvResult(j, i).Style.BackColor = clrBack                                ' バックカラー設定
                        Next
                    End With
                Next
            Else
                ' 0件の処理

                ' コントロールクリア
                If ControlClear(EVENT_SEARCH_NODATA) = False Then
                    Return blnRet
                End If

                ' しばらくお待ちください非表示
                Call FrmWaitInfo.CloseWaitForm()

                ' 対象データなしメッセージボックス表示
                Call CLMsg.Show("DI0001")

            End If

            ' グループボックス件数設定
            If SetTotalCount(intRetCnt - intAccountCnt - intSeniorCnt) = False Then
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
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：ExistsStafAccount
    '   名称　：組合員口座情報存在チェック処理
    '   概要  ：組合員口座情報が存在しているかチェックを行う。
    '   引数　：ByVal iStrUserId     As String = 個人認証ID,
    '           ByVal iStrUseDate    As String = 適用日付,
    '           ByVal iStrBank       As String = 金融機関コード,
    '           ByVal iStrBankOffice As String = 支店番号
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/10(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/10(火) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function ExistsStafAccount(ByVal iStrUserId As String, _
                                       ByVal iStrUseDate As String, _
                                       ByVal iStrBank As String, _
                                       ByVal iStrBankOffice As String) As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False               ' 処理結果
        Dim clsDb As New CLAccessMdb                ' データベースクラス
        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        Try
            ' データベース接続
            Call clsDb.Connect()

            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT c_user_id" & vbCrLf
            strSql = strSql & "   FROM staf_account" & vbCrLf
            strSql = strSql & "  WHERE c_user_id = '" & iStrUserId & "'" & vbCrLf
            strSql = strSql & "    AND d_from = '" & iStrUseDate & "'" & vbCrLf
            strSql = strSql & "    AND c_bank = '" & iStrBank & "'" & vbCrLf
            strSql = strSql & "    AND c_bank_office = '" & iStrBankOffice & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 処理件数取得
            intRetCnt = tbRet.Rows.Count

            ' 処理件数チェック
            If intRetCnt <> 1 Then
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
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            Call clsDb.Disconnect()
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値設定
        Return blnRet
    End Function

    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen() As Boolean

        Dim blnRet As Boolean = False                           ' 処理結果
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)   ' パネルオブジェクト
        Dim clsUC080202 As UC080202 = Nothing                   ' 組合員口座情報 - 詳細クラス
        Dim clsFM010104 As FM010104 = Nothing                   ' 適用日付選択画面
        Dim bytStatus As Byte = 0                               ' ステータス
        Dim strAttrUserId As String = ""                        ' 組合員属性情報の個人認証ID
        Dim strAttrKsh As String = ""                           ' 組合員属性情報の会社コード
        Dim strAttrStafId As String = ""                        ' 組合員属性情報の社員番号
        Dim strAttrUseDate As String = ""                       ' 組合員属性情報の適用日付
        Dim strAcntUserId As String = ""                        ' 組合員口座情報の個人認証ID
        Dim strAcntUseDate As String = ""                       ' 組合員口座情報の適用日付
        Dim strAcntBank As String = ""                          ' 組合員口座情報の金融機関コード
        Dim strAcntBankOffice As String = ""                    ' 組合員口座情報の支店番号

        Try
            '-------------------------------------------------------------------------------
            '   グリッド選択チェック
            '-------------------------------------------------------------------------------
            If Me.dgvResult.SelectedRows.Count < 0 Then                                     ' 選択されているかチェック
                Call CLMsg.Show("GW0001", "データ")                                         ' 未選択の場合、エラーメッセージ表示
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得
            '-------------------------------------------------------------------------------
            If Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString() = "有" Then
                '===========================================================================
                '   内容変更
                '===========================================================================
                bytStatus = STATUS_UPDATE                                                   ' ステータス（内容変更）
                With Me.dgvResult.CurrentRow.Cells
                    strAttrUserId = .Item(14).Value.ToString()                              ' 組合員属性情報の個人認証ID
                    strAttrKsh = .Item(15).Value.ToString()                                 ' 組合員属性情報の会社コード
                    strAttrStafId = .Item(0).Value.ToString()                               ' 組合員属性情報の社員番号
                    strAttrUseDate = .Item(16).Value.ToString().Replace("/", "")            ' 組合員属性情報の適用日付
                    strAcntUserId = .Item(10).Value.ToString()                              ' 組合員口座情報の個人認証ID
                    strAcntUseDate = .Item(11).Value.ToString().Replace("/", "")            ' 組合員口座情報の適用日付
                    strAcntBank = .Item(12).Value.ToString()                                ' 組合員口座情報の金融機関コード
                    strAcntBankOffice = .Item(13).Value.ToString()                          ' 組合員口座情報の支店番号
                End With

            ElseIf Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString() = "未" Then
                '===========================================================================
                '   新規登録
                '===========================================================================
                clsFM010104 = New FM010104                                                  ' インスタンス作成
                clsFM010104.Text = TITLE_SELECT_USE_DATE                                    ' タイトル設定

                ' プロパティ設定
                clsFM010104.strSqlSentence = ""                                             ' SQLなし
                clsFM010104.SetCulumnsName = Nothing                                        ' カラム名
                clsFM010104.SetCulumnsWidth = Nothing                                       ' カラム幅
                clsFM010104.SetCulumnsShow = Nothing                                        ' カラム表示有無
                clsFM010104.ShowInsertDataMessage = False                                   ' メッセージ表示フラグ
                clsFM010104.EnableChkDirectSpecify = True                                   ' 直接指定使用可能フラグ
                Call clsFM010104.ShowDialog()                                               ' 適用日付選択画面表示メッセージ表示フラグ有
                If clsFM010104.IntQlickBtnFlag = 0 Then                                     ' クリックボタン判定
                    ' OKボタン押下
                    If clsFM010104.IsDirectInsert Then                                      ' OKボタン押下で直接指定にチェックがある場合
                        strAcntUseDate = clsFM010104.DirectInsertValue.ToString("yyyyMMdd") ' 日付取得
                    End If
                    clsFM010104.Close()                                                     ' 不要になった時点で破棄
                    clsFM010104.Dispose()
                ElseIf clsFM010104.IntQlickBtnFlag = 1 Then
                    ' キャンセルボタン押下
                    strAcntUseDate = ""
                    clsFM010104.Close()                                                     ' 不要になった時点で破棄
                    clsFM010104.Dispose()
                    Return blnRet
                End If

                ' ステータス（新規登録）
                bytStatus = STATUS_INSERT
                With Me.dgvResult.CurrentRow.Cells
                    strAttrUserId = .Item(14).Value.ToString()                              ' 組合員属性情報の個人認証ID
                    strAttrKsh = .Item(15).Value.ToString()                                 ' 組合員属性情報の会社コード
                    strAttrStafId = .Item(0).Value.ToString()                               ' 組合員属性情報の社員番号
                    strAttrUseDate = .Item(16).Value.ToString().Replace("/", "")            ' 組合員属性情報の適用日付
                End With

                strAcntUserId = ""                                                          ' 組合員口座情報の個人認証ID
                strAcntBank = ""                                                            ' 組合員口座情報の金融機関コード
                strAcntBankOffice = ""                                                      ' 組合員口座情報の支店番号
            End If

            '-------------------------------------------------------------------------------
            '   組合員口座情報 - 詳細
            '-------------------------------------------------------------------------------
            Me.Visible = False                                                              ' 組合員口座情報画面非表示
            ' 画面間パラメータ情報設定
            clsUC080202 = New UC080202                                                      ' 組合員口座情報 - 詳細画面
            clsUC080202.bytStatus = bytStatus                                               ' ステータス
            clsUC080202.strAcntUserId = strAcntUserId                                       ' 組合員口座情報の個人認証ID
            clsUC080202.strAcntUseDate = strAcntUseDate                                     ' 組合員口座情報の適用日付
            clsUC080202.strAcntBank = strAcntBank                                           ' 組合員口座情報の金融機関コード
            clsUC080202.strAcntBankOffice = strAcntBankOffice                               ' 組合員口座情報の支店番号
            clsUC080202.strAttrUserId = strAttrUserId                                       ' 組合員属性情報の個人認証ID
            clsUC080202.strAttrKsh = strAttrKsh                                             ' 組合員属性情報の会社コード
            clsUC080202.strAttrStafId = strAttrStafId                                       ' 組合員属性情報の社員番号
            clsUC080202.strAttrUseDate = strAttrUseDate                                     ' 組合員属性情報の適用日付
            Call pnl.Controls.Add(clsUC080202)                                              ' 組合員管理 - 組合員口座情報 - 詳細画面表示

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
                                   SCREEN_ID, _
                                   SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値設定
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：setGrant
    '   名称　：権限取得処理
    '   概要  ：権限を取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>権限取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function setGrant() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim dtGrant As DataTable = Nothing          ' 権限取得データテーブル

        Try
            ' 権限取得
            dtGrant = getGrant(MENU_ID_UC080201)
            If dtGrant.Rows.Count > 0 Then
                strGrantReference = dtGrant.Rows(0).Item(3).ToString                    ' 参権限照
                strGrantInsert = dtGrant.Rows(0).Item(4).ToString                       ' 登録権限
                strGrantPrint = dtGrant.Rows(0).Item(5).ToString                        ' 印刷権限
                strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString                   ' ファイル出力権限
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
    '   引数　：ByVal iRowsCount As Integer = 検索件数
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/05(木) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/05(木) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>件数設定処理</summary>
    ''' <param name="iRowsCount">検索件数</param>
    ''' <remarks></remarks>
    Private Function SetTotalCount(ByVal iRowsCount As Integer) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            Me.grpResult.Text = ("振込情報の一覧 ( " & iRowsCount.ToString & " " & "件" & " )")

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
    '   ＩＤ　：DisplayOnOff
    '   名称　：表示非表示処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/13(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>表示非表示処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DisplayOnOff() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim intCnt As Integer = 0               ' 非表示件数

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor

            If Me.grpResult.Visible Then
                Me.dgvResult.Visible = False
                For i = 0 To Me.dgvResult.RowCount - 1
                    ' 口座登録済情報非表示チェック有で、口座登録未情報のもの（隠し項目が "有" のもの）か
                    ' シニア前情報非表示チェック有で、シニア前情報のもの（隠し項目が "True" のもの）
                    If (Me.chkAccount.Checked AndAlso (Me.dgvResult.Rows(i).Cells.Item(8).Value = "有")) _
                    Or (Me.chkSenior.Checked AndAlso CBool(Me.dgvResult.Rows(i).Cells.Item(9).Value)) Then
                        Me.dgvResult.Rows(i).Visible = False
                        intCnt = intCnt + 1
                    Else
                        Me.dgvResult.Rows(i).Visible = True
                    End If
                Next
                Me.dgvResult.Visible = True
            End If

            ' 件数表示
            If SetTotalCount(Me.dgvResult.RowCount - intCnt) Then
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
    '   ＩＤ　：FormClose
    '   名称　：フォームクローズ処理
    '   概要  ：
    '   引数　：
    '   戻り値：なし
    '   作成日：2012/01/20(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function FormClose() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim pn As Panel                             ' メインパネルオブジェクト
        Dim uc As Control                           ' ユーザコントロールオブジェクト
        Dim clsUC080202 As UC080202                 ' 組合員口座情報 - 詳細クラス

        Try
            Me.Visible = False                                                          ' 組合員情報口座情報非表示
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)                             ' メインパネル生成
            uc = pn.Controls(SCREEN_ID_UC080202)                                        ' ユーザコントロール生成

            ' 組合員口座情報がパネルにあるかチェック
            If uc Is Nothing Then
                ' ない場合、生成してパネルに追加
                uc = New UC080202
                Call pn.Controls.Add(uc)
            Else
                ' ある場合、組合員口座情報表示
                clsUC080202 = pn.Controls(SCREEN_ID_UC080202)
                uc.Visible = True
            End If
            Me.Dispose()

            ' 戻り値に正常を設定
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
#End Region

End Class
