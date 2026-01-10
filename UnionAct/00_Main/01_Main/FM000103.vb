#Region "FM000103"
'===========================================================================================================
'   クラスＩＤ　　：FM000103
'   クラス名称　　：パスワード変更
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk
Imports UnionAct.NSCLEncrypt
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDCommon

Public Class FM000103

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM000103          ' FM000103
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM000103      ' パスワード変更画面
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000103_Load
    '   名称　：フォームロード処理
    '   概要　：フォームロード処理を行う。
    '   作成日：2011/11/07(月)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub FM000103_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            '-------------------------------------------------------------------------------
            '   画面中央表示処理
            '-------------------------------------------------------------------------------
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：Change_Click
    '   名称　：パスワード変更ボタンクリック処理
    '   概要　：パスワードの変更を行う。
    '   作成日：2011/11/07(月)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub Change_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Change.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Try
            ' 背景色クリア
            Me.txtNowPassword.BackColor = System.Drawing.SystemColors.Window
            Me.txtNewPassword.BackColor = System.Drawing.SystemColors.Window
            Me.txtCheckPassword.BackColor = System.Drawing.SystemColors.Window
            '-------------------------------------------------------------------------------
            '   入力チェック処理
            '-------------------------------------------------------------------------------
            If ChkInput() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   パスワード変更処理
            '-------------------------------------------------------------------------------
            If UpdatePassword() = False Then
                Exit Sub
            End If
            ' OK（変更）ボタン押下設定
            Me.DialogResult = Windows.Forms.DialogResult.OK
            ' 画面閉じる
            Me.Close()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：Cancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：画面を閉じる。
    '   作成日：2011/11/07(月)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Try
            ' キャンセルボタン押下設定
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            ' 画面閉じる
            Me.Close()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtNowPassword_GotFocus
    '   名称　：現在のパスワードテキストボックスフォーカス取得処理
    '   概要　：現在のパスワードテキストボックスの全選択を行う。
    '   作成日：2011/11/07(月)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtNowPassword_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNowPassword.GotFocus
        Try
            ' 全選択
            Me.txtNowPassword.SelectAll()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtNewPassword_GotFocus
    '   名称　：新しいパスワードテキストボックスフォーカス取得処理
    '   概要　：新しいパスワードテキストボックスの全選択を行う。
    '   作成日：2011/11/07(月)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtNewPassword_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNewPassword.GotFocus
        Try
            ' 全選択
            Me.txtNewPassword.SelectAll()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtNewPassword_GotFocus
    '   名称　：新しいパスワードの確認テキストボックスフォーカス取得処理
    '   概要　：新しいパスワードの確認テキストボックスの全選択を行う。
    '   作成日：2011/11/07(月)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtCheckPassword_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCheckPassword.GotFocus
        Try
            ' 全選択
            Me.txtCheckPassword.SelectAll()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtNowPassword_KeyDown
    '   名称　：現在のパスワードテキストボックスキーダウン処理
    '   概要  ：
    '   作成日：2011/11/17(木)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtNowPassword_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNowPassword.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtNewPassword_KeyDown
    '   名称　：新しいパスワードテキストボックスキーダウン処理
    '   概要  ：
    '   作成日：2011/11/17(木)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtNewPassword_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNewPassword.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：txtCheckPassword_KeyDown
    '   名称　：新しいパスワードテキストボックスキーダウン処理
    '   概要  ：
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtCheckPassword_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCheckPassword.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
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
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要  ：フォーム上の入力チェックを行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False       ' 処理結果
        Try
            '-------------------------------------------------------------------------------
            '   未入力チェック
            '-------------------------------------------------------------------------------
            ' 現在のパスワード
            If ChkNull(Me.txtNowPassword.Text) Then
                CLMsg.Show("GE0006", "現在のパスワード")
                Me.txtNowPassword.Focus()
                SetErr(Me.txtNowPassword)
                Return blnRet
            End If
            ' 新しいパスワード
            If ChkNull(Me.txtNewPassword.Text) Then
                CLMsg.Show("GE0006", "新しいパスワード")
                Me.txtNewPassword.Focus()
                SetErr(Me.txtNewPassword)
                Return blnRet
            End If
            ' 新しいパスワードの確認
            If ChkNull(Me.txtCheckPassword.Text) Then
                CLMsg.Show("GE0006", "新しいパスワードの確認")
                Me.txtCheckPassword.Focus()
                SetErr(Me.txtCheckPassword)
                Return blnRet
            End If
            '-------------------------------------------------------------------------------
            '   現在のパスワードチェック
            '-------------------------------------------------------------------------------
            If ChkNowPass() = False Then
                CLMsg.Show("GE0092")
                Me.txtNowPassword.Focus()
                SetErr(Me.txtNowPassword)
                Return blnRet
            End If
            '-------------------------------------------------------------------------------
            '   新しいパスワードと新しいパスワードの確認同一チェック
            '-------------------------------------------------------------------------------
            If Me.txtNewPassword.Text <> Me.txtCheckPassword.Text Then
                CLMsg.Show("GE0091")
                Me.txtCheckPassword.Focus()
                SetErr(Me.txtCheckPassword)
                Return blnRet
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：ChkNowPass
    '   名称　：現在パスワードチェック処理
    '   概要  ：現在のパスワードがあっているかチェックを行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function ChkNowPass() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsMdb As New CLAccessMdb                                                       ' データベースクラス生成
        Dim tbRet As DataTable = Nothing                                                    ' 処理結果格納データテーブル
        Dim strSql As String = ""                                                           ' SQL文
        Dim strNowPassEncrypt As String = ""                                                ' 暗号化した現在パスワード
        Dim strNow As String = Nothing                                                      ' 現在日付（yyyyMMdd）
        Try
            ' 入力された現在パスワードを暗号化
            strNowPassEncrypt = CLEncrypt.Encrypt(Me.txtNowPassword.Text, MDSystemInfo.EncryptKey)
            ' 現在日付（yyyyMMdd）
            strNow = Now.ToString("yyyMMdd")
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT certify.c_user_id" & vbCrLf
            strSql = strSql & "   FROM certify" & vbCrLf
            strSql = strSql & "  WHERE certify.c_user_id = '" & MDLoginInfo.UserId & "'" & vbCrLf
            strSql = strSql & "    AND certify.c_pwd = '" & strNowPassEncrypt & "'" & vbCrLf
            strSql = strSql & "    AND certify.d_from <= '" & strNow & "'" & vbCrLf         ' パスワードマスタの適用開始年月日が現在日付よりも小さいもの
            strSql = strSql & "    AND certify.d_to >= '" & strNow & "'" & vbCrLf           ' パスワードマスタの適用終了年月日が現在日付よりも大きいもの
            Call clsMdb.Connect()                                                           ' データベース接続
            tbRet = clsMdb.ExecuteSql(strSql)                                               ' SQL実行
            ' 対象データを取得
            If tbRet.Rows.Count = 0 Then
                Return blnRet
            End If
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsMdb.Disconnect()                                                        ' データベース切断
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：UpdatePassword
    '   名称　：パスワード変更処理
    '   概要  ：パスワード変更処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function UpdatePassword() As Boolean
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsMdb As New CLAccessMdb                                                       ' データベースクラス生成
        Dim intRet As Integer = -1                                                          ' 処理結果件数
        Dim strSql As String = ""                                                           ' SQL文
        Dim strNowPassEncrypt As String = ""                                                ' 暗号化した現在パスワード
        Dim strNewPassEncrypt As String = ""                                                ' 暗号化した新しいパスワード
        Dim strNow As String = Nothing                                                      ' 現在日付（yyyyMMdd）
        Try
            ' 入力された現在パスワードを暗号化
            strNowPassEncrypt = CLEncrypt.Encrypt(Me.txtNowPassword.Text, MDSystemInfo.EncryptKey)
            ' 入力された新しいパスワードを暗号化
            strNewPassEncrypt = CLEncrypt.Encrypt(Me.txtNewPassword.Text, MDSystemInfo.EncryptKey)
            ' 現在日付取得（yyyyMMdd）
            strNow = Now.ToString("yyyyMMdd")
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " UPDATE certify" & vbCrLf                                                                        ' パスワードマスタ
            strSql = strSql & "    SET certify.c_pwd = '" & strNewPassEncrypt & "'" & vbCrLf                                    ' パスワード
            strSql = strSql & "       ,certify.d_up = '" & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") & "'" & vbCrLf   ' 更新日
            strSql = strSql & "       ,certify.c_user_id_up = '" & MDLoginInfo.UserId & "'" & vbCrLf                            ' 更新者個人ID
            strSql = strSql & "  WHERE certify.c_user_id = '" & MDLoginInfo.UserId & "'" & vbCrLf   ' パスワードマスタの個人認証IDと入力した社員番号と同じもの
            strSql = strSql & "    AND certify.c_pwd = '" & strNowPassEncrypt & "'" & vbCrLf        ' パスワードマスタのパスワードと変更前パスワードと同じもの
            strSql = strSql & "    AND certify.d_from <= '" & strNow & "'" & vbCrLf                 ' パスワードマスタの適用開始年月日が現在日付よりも小さいもの
            strSql = strSql & "    AND certify.d_to >= '" & strNow & "'" & vbCrLf                   ' パスワードマスタの適用終了年月日が現在日付よりも大きいもの
            Call clsMdb.Connect()                                                           ' データベース接続
            Call clsMdb.BeginTran()                                                         ' トランザクション開始
            intRet = clsMdb.ExecuteNonQuery(strSql)                                         ' SQL実行
            If intRet = 0 Then
                Call clsMdb.RollbackTran()                                                  ' ロールバック
                Call CLMsg.Show("FE0001")
                Return blnRet
            End If
            Call clsMdb.CommitTran()                                                        ' コミット
            Call CLMsg.Show("GI0018")                                                       ' 変更完了メッセージ出力
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            Call clsMdb.RollbackTran()                                                      ' ロールバック
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsMdb.Disconnect()                                                        ' データベース切断
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値設定
    End Function
#End Region

End Class
#End Region
