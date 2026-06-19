#Region "FM000101"
'===========================================================================================================
'   クラスＩＤ　　：FM000101
'   クラス名称　　：ユーザ認証
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDChk
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLEncrypt
Imports UnionAct.NSMDConst

Public Class FM000101

#Region "定数・変数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM000101          ' FM000101
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM000101      ' ユーザ認証画面
    ' 専従職員フラグ
    Private blnSenjyuFlg As Boolean = False
    ' 再ログイン処理フラグ
    Private blnReloginFlg As Boolean = False
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000101_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '       　：2012/08/14(火)  Fujisaku  同期処理とログイン判定処理追加
    '***************************************************************************************************
    Private Sub FM000101_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            '-------------------------------------------------------------------------------
            '   再ログイン判定処理
            '-------------------------------------------------------------------------------
            ' 前回ログインユーザーが残っているか判定
            If Not MDLoginInfo.UserId = "" Then
                blnReloginFlg = True
            End If
            '-------------------------------------------------------------------------------
            '   初期化処理
            '-------------------------------------------------------------------------------
            If Initialize() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   システム情報格納
            '-------------------------------------------------------------------------------
            If SetSystemInfo() = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   二重起動チェック処理
            '-------------------------------------------------------------------------------
            If ChkDualProcess() = False Then
                Call CLMsg.Show("BE0002", "統合OA")
                End
            End If
            '-------------------------------------------------------------------------------
            '   画面中央表示処理
            '-------------------------------------------------------------------------------
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   画面解像度チェック処理
            '-------------------------------------------------------------------------------
            ' 画面サイズチェックフラグが "1" の場合、チェック処理を行う
            If MDSystemInfo.FlgChkDisplaySize = "1" Then
                If ChkDisplaySize() = False Then
                    Call CLMsg.Show("GE0005")
                    Me.Close()
                End If
            End If
            '-------------------------------------------------------------------------------
            '   システム時刻調整処理
            '-------------------------------------------------------------------------------
            ' 既存システムは、postgresqlのデータベースサーバのcurrent_dateから取得している。
            ' どうする？
            'If ChkSystemTime() = False Then
            '    Dim result As DialogResult = clMsg.Show("BQ0001")
            '    If result = Windows.Forms.DialogResult.Yes Then
            '        ' 「はい」ボタン押下
            '        If SetSystemTime() = False Then
            '            Exit Sub
            '        End If
            '    ElseIf result = Windows.Forms.DialogResult.No Then
            '        ' 「いいえ」ボタン押下
            '        Exit Sub
            '    End If
            'End If
            ''-------------------------------------------------------------------------------
            ''   ソフトウェア更新処理
            ''-------------------------------------------------------------------------------
            '' マスタデータと同期を行う
            'If MDSystemInfo.DebugMode = False And Not blnReloginFlg Then
            '    If CLMsg.Show("GQ0100") = vbYes Then
            '        If Not syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, True) Then
            '            Me.Close()
            '        End If
            '    End If
            'Else
            '    blnReloginFlg = False
            'End If
            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            ' 期コンボボックスデータ取得・お知らせデータ取得
            If GetData() = False Then
                Exit Sub
            End If


      '*******************************************************************************
      '   開発用
      '*******************************************************************************
#If DEBUG Then
      Me.txtMemberNo.Text = "sysadmin"
      Me.txtPwd.Text = "admin"
      me.lblTitle.text = "総合ＯＡシステム"
      me.text = "ユーザ認証［Debug］"
#ElseIf STAGING Then
      Me.lblTitle.Text = "総合ＯＡシステム"
      Me.Text = "ユーザ認証［Staging］"
#ElseIf RELEASE Then
      Me.lblTitle.Text = "総合ＯＡシステム"
      Me.Text = "ユーザ認証"
#End If
      ''*******************************************************************************
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
    '   ＩＤ　：btnBegin_Click
    '   名称　：開始ボタンクリック処理
    '   概要  ：
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnBegin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBegin.Click

        Dim clsDb As New CLAccessMdb                    ' データベースクラス生成
        Dim clsFM000102 As New FM000102                 ' メインメニュー画面クラス生成
        Dim clsFM000104 As New FM000104                 ' 部／委員会選択画面クラス生成
        Dim clsFM000103 As New FM000103                 ' パスワード変更画面クラス生成
        Dim intCommitteeCnt As Integer = Nothing        ' 所属委員会件数
        Dim diaRet As DialogResult = Nothing            ' 戻りボタン判定

        Try
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            '-------------------------------------------------------------------------------
            '   入力チェック処理
            '-------------------------------------------------------------------------------
            If ChkInput() = False Then
                Exit Sub
            End If

            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------------------------------
            '   ユーザ認証処理
            '-------------------------------------------------------------------------------
            If ChkUserCertify(clsDb) = False Then
                Call CLMsg.Show("BE0009")
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   ログイン情報格納
            '-------------------------------------------------------------------------------
            If SetLoginInfo(clsDb) = False Then
                Call MessageBox.Show("ログイン情報が取得できません。", _
                                     "エラー", MessageBoxButtons.OK, _
                                     MessageBoxIcon.Error, _
                                     MessageBoxDefaultButton.Button1)
                Exit Sub
            End If
            '-------------------------------------------------------------------------------
            '   パスワード変更有無処理
            '-------------------------------------------------------------------------------
            If Me.txtMemberNo.Text = Me.txtPwd.Text Then                                    ' 初期パスワードの場合（社員番号とパスワードが同じ場合）
                diaRet = CLMsg.Show("GW0007")                                               ' パスワード変更メッセージ表示
                If diaRet = Windows.Forms.DialogResult.Yes Then
                    ' OK（変更）ボタン押下時
                    clsFM000103 = New FM000103                                              ' パスワード変更画面クラス生成
                    Call clsFM000103.ShowDialog()                                           ' パスワード変更画面表示
                    If clsFM000103.DialogResult = Windows.Forms.DialogResult.Cancel Then
                        ' キャンセル押下時
                        Call CLMsg.Show("BE0034")
                        Exit Sub
                    End If
                    clsFM000103.Dispose()                                                   ' パスワード変更画面クラス破棄
                ElseIf diaRet = Windows.Forms.DialogResult.No Then
                    ' キャンセル押下時
                    Call CLMsg.Show("BE0034")
                    Exit Sub
                End If
            End If
            '-------------------------------------------------------------------------------
            '   所属委員会情報設定処理
            '-------------------------------------------------------------------------------
            ' 専従職員かチェック
            If blnSenjyuFlg Then
                If GetCommitteeInfoSenjyu(clsDb) = False Then
                    Exit Sub
                End If
            Else
                ' 所属委員会が1件の場合、所属委員会情報を設定、1件以外は件数のみ返す。
                intCommitteeCnt = GetCommitteeInfo(clsDb)
                If intCommitteeCnt <> 1 Then
                    If intCommitteeCnt = 0 Then
                        '-------------------------------------------------------------------
                        '   0件
                        '-------------------------------------------------------------------
                        Call MessageBox.Show("所属している委員会が１つもありません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                        Exit Sub
                    Else
                        '-------------------------------------------------------------------
                        '   複数件
                        '-------------------------------------------------------------------
                        ' 部／委員会選択画面表示
                        clsFM000104 = New FM000104                                          ' 部／委員会選択画面クラス生成
                        Call clsFM000104.ShowDialog()                                       ' 部／委員会選択画面表示
                        diaRet = clsFM000104.DialogResult
                        Call clsFM000104.Dispose()                                          ' 部／委員会選択画面クラス破棄
                        If diaRet = Windows.Forms.DialogResult.Cancel Then
                            Exit Sub
                        End If
                        'If diaRet = Windows.Forms.DialogResult.OK Then                      ' OKボタン押下時
                        '    ' OKボタン押下
                        '    MDLoginInfo.CommitteeName = clsFM000104.strCommitteeName        ' 所属委員会名称
                        '    MDLoginInfo.CommitteeId = clsFM000104.strCommitteeId            ' 所属委員会ID
                        '    MDLoginInfo.PostId = clsFM000104.strPostId                      ' 役職ID
                        '    MDLoginInfo.PostName = clsFM000104.strPostName                  ' 役職名称

                        '    ' 委員会ステータスフラグと管理部用委員会IDリスト取得
                        '    If MDLoginInfo.CommitteeId.Substring(0, 1) = "M" Then
                        '        MDLoginInfo.CommitteeStatusFlg = 2                          ' 委員会ステータスフラグ（2：管理部）
                        '        ' 管理部委員会IDリスト取得処理
                        '        If getDepartmentCommitteeIdList(clsDb, MDLoginInfo.CommitteeId) = False Then
                        '            Exit Sub
                        '        End If
                        '    Else
                        '        MDLoginInfo.CommitteeStatusFlg = 1                          ' 委員会ステータスフラグ（1：委員会）
                        '        MDLoginInfo.CommitteeIdList.Clear()
                        '    End If

                        'ElseIf diaRet = Windows.Forms.DialogResult.Cancel Then              ' キャンセルボタン押下時
                        '    ' キャンセルボタン押下
                        '    MDLoginInfo.CommitteeName = ""                                  ' 所属委員会名称
                        '    MDLoginInfo.CommitteeId = ""                                    ' 所属委員会ID
                        '    MDLoginInfo.PostId = ""                                         ' 役職ID
                        '    MDLoginInfo.PostName = ""                                       ' 役職名称
                        '    MDLoginInfo.CommitteeStatusFlg = 0                              ' 委員会ステータスフラグ（0：専従職員）
                        '    MDLoginInfo.CommitteeIdList.Clear()
                        '    Exit Sub                                                        ' 処理抜ける
                        'End If
                    End If
                End If
            End If

            '-------------------------------------------------------------------------------
            '   メインメニュー画面表示
            '-------------------------------------------------------------------------------
            clsFM000102 = New FM000102
            If (intCommitteeCnt = 1) Or (intCommitteeCnt = 0) Then
                clsFM000102.btnChangeAuthority.Enabled = False
            End If
            Call clsFM000102.Show()
            ' ログイン画面閉じる
            Call Me.Close()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()                 ' データベース切断
            Cursor.Current = Cursors.Default        ' カーソルを矢印に戻す
        End Try

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnEnd_Click
    '   名称　：終了ボタンクリック処理
    '   概要  ：
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnd.Click

        Dim diaRet As DialogResult                  ' メッセージボタン押下結果

        Try
            ' 終了確認メッセージボックス表示
            diaRet = CLMsg.Show("GQ0025")
            If diaRet = vbYes Then                  ' 「はい」押下時
                Me.Close()                          ' 終了
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
    '   ＩＤ　：txtMemberNo_GotFocus
    '   名称　：社員番号テキストボックスフォーカス取得処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtMemberNo_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtMemberNo.GotFocus

        Try
            ' 全選択
            Me.txtMemberNo.SelectAll()

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
    '   ＩＤ　：txtPwd_GotFocus
    '   名称　：パスワードテキストボックスフォーカス取得処理
    '   概要  ：
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtPwd_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPwd.GotFocus

        Try
            ' 全選択
            Me.txtPwd.SelectAll()

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
    '   概要  ：
    '   作成日：2011/11/14(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtMemberNo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMemberNo.KeyDown

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
    '   ＩＤ　：txtMemberNo_KeyDown
    '   名称　：パスワードテキストボックスキーダウン処理
    '   概要  ：
    '   作成日：2011/11/14(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtPwd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPwd.KeyDown

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
    '   ＩＤ　：cboTerm_KeyDown
    '   名称　：期コンボボックスキーダウン処理
    '   概要  ：
    '   作成日：2011/11/14(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub cboTerm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboPeriod.KeyDown

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
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：入力チェック処理
    '   概要  ：フォーム上の入力チェックを行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            '-------------------------------------------------------------------------------
            '   未入力チェック
            '-------------------------------------------------------------------------------
            ' 社員番号
            If Me.txtMemberNo.Text.Length = 0 Then
                CLMsg.Show("BE0009")
                Me.txtMemberNo.Focus()
                Return False
            End If
            ' パスワード
            If MDSystemInfo.DebugMode = False And Me.txtPwd.Text.Length = 0 Then
                CLMsg.Show("BE0009")
                Me.txtPwd.Focus()
                Return False
            End If
            '-------------------------------------------------------------------------------
            '   未選択チェック
            '-------------------------------------------------------------------------------
            ' 期コンボボックス
            If Me.cboPeriod.SelectedIndex <= 0 Then
                CLMsg.Show("GW0001", "期")
                Me.cboPeriod.Focus()
                Return False
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
    '   ＩＤ　：ChkUserCertify
    '   名称　：ユーザ認証処理
    '   概要  ：社員番号とパスワードでユーザ認証を行う。
    '   引数　：ByVal pClsMdb As CLAccessMdb
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>ユーザ認証処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkUserCertify(ByVal pClsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim tbRet As DataTable = Nothing                ' 処理結果格納データテーブル
        Dim strSql As String = Nothing                  ' SQL文
        Dim strPwd As String = Nothing                  ' 入力されたパスワード
        Dim strUserId As String = Nothing               ' 入力された社員番号
        Dim strPwdEncrypt As String = Nothing           ' 入力されたパスワードを暗号化したもの
        Dim strNow As String = Nothing                  ' 現在日付（yyyyMMdd）

        Try
            '-------------------------------------------------------------------------------
            '   入力項目取得
            '-------------------------------------------------------------------------------
            strUserId = Me.txtMemberNo.Text             ' 社員番号
            strPwd = Me.txtPwd.Text                     ' パスワード
            strNow = Now.ToString("yyyMMdd")            ' 現在日付（yyyyMMdd）

            '-------------------------------------------------------------------------------
            '   パスワード暗号化処理
            '-------------------------------------------------------------------------------
            strPwdEncrypt = CLEncrypt.Encrypt(strPwd, MDSystemInfo.EncryptKey)
            If MDSystemInfo.DebugMode = False And MDChk.ChkNull(strPwdEncrypt) Then
                Return blnRet
            End If

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT certify.c_user_id" & vbCrLf                          ' 個人認証ID
            strSql = strSql & "       ,certify.d_from" & vbCrLf                             ' 適用開始年月日
            strSql = strSql & "       ,certify.d_to" & vbCrLf                               ' 適用終了年月日
            strSql = strSql & "       ,certify.c_pwd" & vbCrLf                              ' パスワード
            strSql = strSql & "       ,certify.d_ins" & vbCrLf                              ' 作成日
            strSql = strSql & "       ,certify.c_user_id_ins" & vbCrLf                      ' 作成者個人ID
            strSql = strSql & "   FROM certify" & vbCrLf                                    ' パスワードマスタ
            strSql = strSql & "  WHERE certify.c_user_id = '" & strUserId & "'" & vbCrLf    ' パスワードマスタの個人認証IDと入力した社員番号が同じもの
            If MDSystemInfo.DebugMode = False Then
                strSql = strSql & "    AND certify.c_pwd = '" & strPwdEncrypt & "'" & vbCrLf    ' パスワードマスタの入力したパスワード（暗号化後）が同じもの
            End If
            strSql = strSql & "    AND certify.d_from <= '" & strNow & "'" & vbCrLf         ' パスワードマスタの適用開始年月日が現在日付よりも小さいもの
            strSql = strSql & "    AND certify.d_to >= '" & strNow & "'" & vbCrLf           ' パスワードマスタの適用終了年月日が現在日付よりも大きいもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = pClsDb.ExecuteSql(strSql)

            ' 件数チェック
            If tbRet.Rows.Count <> 1 Then
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
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：お知らせデータ取得処理・期コンボボックス作成処理を呼び出す。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsDb As New CLAccessMdb            ' データベースクラス生成

        Try
            ' データベース接続
            Call clsDb.Connect()

            '-------------------------------------------------------------------------------
            '   お知らせデータ取得処理
            '-------------------------------------------------------------------------------
            If GetInformation(clsDb) = False Then
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   期コンボボックス作成処理
            '-------------------------------------------------------------------------------
            If CreateCboTerm(clsDb) = False Then
                Return False
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
    '   ＩＤ　：GetInformation
    '   名称　：お知らせデータ取得処理
    '   概要  ：お知らせデータを取得する。
    '   引数　：ByVal clsMdb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>お知らせデータ取得処理</summary>
    ''' <param name="clsMdb"></param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetInformation(ByVal clsMdb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim strRet As String = ""               ' 取得文字列
        Dim sql As String = ""                  ' SQL文
        Dim tb As DataTable = Nothing           ' 処理結果データテーブル
        Dim i As Integer = 0                    ' カウンタ
        Dim strNow As String = Nothing          ' 現在日付（yyyyMMdd）

        Try
            strNow = Now.ToString("yyyyMMdd")                                               ' 現在日付取得（yyyyMMdd）

            ' SQL作成
            sql = "" & vbCrLf
            sql = sql & " SELECT Information.c_ksh" & vbCrLf                                ' 会社コード
            sql = sql & "       ,Information.d_from" & vbCrLf                               ' 適用開始年月日
            sql = sql & "       ,Information.d_to" & vbCrLf                                 ' 適用終了年月日
            sql = sql & "       ,Information.l_message" & vbCrLf                            ' 出力メッセージ
            sql = sql & "       ,Information.l_biko" & vbCrLf                               ' 備考
            sql = sql & "       ,Information.d_ins" & vbCrLf                                ' 作成日
            sql = sql & "       ,Information.c_user_id_ins" & vbCrLf                        ' 作成者個人ID
            sql = sql & "   FROM Information" & vbCrLf                                      ' 情報テーブル
            sql = sql & "  WHERE Information.d_from <= '" & strNow & "'" & vbCrLf           ' 情報テーブルの適用開始年月日が現在日付よりも小さいもの
            sql = sql & "    AND Information.d_to >= '" & strNow & "'" & vbCrLf             ' 情報テーブルの適用終了年月日が現在日付よりも大きいもの
            sql = sql & ";" & vbCrLf

            ' SQL実行
            tb = clsMdb.ExecuteSql(sql)
            ' 件数チェック
            If tb.Rows.Count <> 0 Then
                For i = 0 To tb.Rows.Count - 1                                              ' データ件数分ループ
                    If i <> 0 Then                                                          ' 1レコード目以外は、改行を入れる
                        strRet = strRet & vbCrLf
                    End If
                    strRet = strRet & tb.Rows(i).Item(3).ToString
                Next
                Me.txtInfomation.Text = strRet                                              ' お知らせテキストボックスに設定
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
    '   ＩＤ　：CreateCboTerm
    '   名称　：期コンボボックス作成処理
    '   概要  ：期コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/01(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/01(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>期コンボボックス作成処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboTerm(ByVal clsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果
        Dim dtRet As DataTable = Nothing                ' 処理結果データテーブル
        Dim strSql As String = ""                       ' SQL文
        Dim intRetCnt As Integer = Nothing              ' 処理結果件数
        Dim strNow As String = Nothing                  ' 現在日付（yyyyMMdd）
        Dim intIndex As Integer = Nothing               ' 選択されたコンボボックスリストインデックス

        Try
            '-------------------------------------------------------------------------------
            '   初期処理
            '-------------------------------------------------------------------------------
            strNow = Now.ToString("yyyyMMdd")                                               ' 現在日付取得（yyyyMMdd）
            Me.cboPeriod.DataSource = Nothing                                               ' 期コンボボックスクリア
            Me.cboPeriod.Text = ""

            '-------------------------------------------------------------------------------
            '   期コンボボックスリスト作成
            '-------------------------------------------------------------------------------
            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT period.l_name      AS DisplayName" & vbCrLf          ' 期名称
            strSql = strSql & "       ,period.c_period_id AS ValueName" & vbCrLf            ' 期ID
            strSql = strSql & "   FROM period" & vbCrLf                                     ' 期マスタ
            strSql = strSql & "  ORDER BY period.c_period_id DESC" & vbCrLf                 ' 期IDで並替    'chk
            strSql = strSql & ";" & vbCrLf
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, Me.cboPeriod, strSql, "DisplayName", "ValueName") = False Then
                Return False
            End If

            '-------------------------------------------------------------------------------
            '   現在日付からあてはまる期を取得して、期コンボボックスの表示を設定
            '-------------------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT period.l_name" & vbCrLf                              ' 期名称
            strSql = strSql & "   FROM period" & vbCrLf                                     ' 期マスタ
            strSql = strSql & "  WHERE period.d_from <= '" & strNow & "'" & vbCrLf          ' 期マスタの適用開始年月日が現在日付よりも小さいもの
            strSql = strSql & "    AND period.d_to >= '" & strNow & "'" & vbCrLf            ' 期マスタの適用終了年月日が現在日付よりも大きいもの
            strSql = strSql & ";" & vbCrLf
            dtRet = clsDb.ExecuteSql(strSql)                                                ' SQL実行
            intRetCnt = dtRet.Rows.Count                                                    ' 件数取得
            If intRetCnt = 1 Then
                intIndex = Me.cboPeriod.FindString(dtRet.Rows(0).Item(0).ToString)          ' 取得した名称のインデックス取得
                Me.cboPeriod.SelectedIndex = intIndex                                       ' 取得したインデックスのデータ表示
            Else
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
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：SetSystemInfo
    '   名称　：システム情報取得処理
    '   概要  ：システム情報をグローバル変数に格納する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/03(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/03(木)  m.suzuki  新規作成
    '         ：2013/04/19(金)  Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    ''' <summary>システム情報取得</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SetSystemInfo() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            '-------------------------------------------------------------------
            '   システム情報格納
            '-------------------------------------------------------------------
            'OleDB Connection String
            MDSystemInfo.DbConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("UnionActDb").ConnectionString()
            'MDSystemInfo.DbConnectionString = System.Configuration.ConfigurationManager.AppSettings("OleDbConnectionString").ToString()
            'SQLType(0=MDB,1=SQL Server)
            'MDSystemInfo.SQLType = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings("SQLType").ToString())
            'If MDSystemInfo.SQLType = 0 Then
            '    MDSystemInfo.DbConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\data\miraishosha\ACAOA\MDB\acaoa.mdb;Persist Security Info=False;Jet OLEDB:Database Password=acaoa"
            'End If
            '' ACCESS MDB プロバイダ
            'MDSystemInfo.AccessProvider = System.Configuration.ConfigurationManager.AppSettings("AccessProvider").ToString()
            '' ACCESS MDB ファイルパス
            'MDSystemInfo.AccessPath = System.Configuration.ConfigurationManager.AppSettings("AccessPath").ToString()
            '' ACCESS MDB ファイル名
            'MDSystemInfo.AccessName = System.Configuration.ConfigurationManager.AppSettings("AccessName").ToString()
            '' ACCESS MDB デザインマスタファイルパス
            'MDSystemInfo.AccessMstPath = System.Configuration.ConfigurationManager.AppSettings("AccessMstPath").ToString()
            '' ACCESS MDB デザインマスタファイル名
            'MDSystemInfo.AccessMstName = System.Configuration.ConfigurationManager.AppSettings("AccessMstName").ToString()
            '' ACCESS MDB セキュリティ警告
            'MDSystemInfo.AccessPersistSecurity = System.Configuration.ConfigurationManager.AppSettings("AccessPersistSecurity").ToString()
            '' ACCESS MDB パスワード
            'MDSystemInfo.AccessPassword = System.Configuration.ConfigurationManager.AppSettings("AccessPassword").ToString()
            ' メッセージファイルパス
            'MDSystemInfo.MessagePath = System.Configuration.ConfigurationManager.AppSettings("MessagePath").ToString()
            MDSystemInfo.MessagePath = Application.StartupPath & "\"
            ' メッセージファイル名
            'MDSystemInfo.MessageName = System.Configuration.ConfigurationManager.AppSettings("MessageName").ToString()
            MDSystemInfo.MessageName = "ExceptionMsgData.xml"
            ' SEQUENCE ファイルパス
            MDSystemInfo.SequencePath = System.Configuration.ConfigurationManager.AppSettings("SequencePath").ToString()
            ' 暗号化キー（公開キー）
            MDSystemInfo.EncryptKey = System.Configuration.ConfigurationManager.AppSettings("EncryptKey").ToString()
            ' 画面サイズチェックフラグ
            MDSystemInfo.FlgChkDisplaySize = System.Configuration.ConfigurationManager.AppSettings("FlgChkDisplaySize").ToString()
            ' アプリケーション実行パス
            MDSystemInfo.AppPath = My.Application.Info.DirectoryPath.ToString() & "\"

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
    '   ＩＤ　：SetLoginInfo
    '   名称　：ログイン情報取得処理
    '   概要  ：ログイン情報をグローバル変数に格納する。
    '   引数　：ByVal pClsDb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/03(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/03(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>ログイン情報取得</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function SetLoginInfo(ByVal pClsDb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False                   ' 処理結果

        Try
            '-------------------------------------------------------------------------------
            '   個人認証ID取得
            '-------------------------------------------------------------------------------
            MDLoginInfo.UserId = Me.txtMemberNo.Text    ' 個人認証ID取得

            '-------------------------------------------------------------------------------
            '   会社・期情報取得（期ID / 期名称 / 期（数値）/ 期From / 期To / 会社コード / 会社名）
            '-------------------------------------------------------------------------------
            If GetOfficeTermInfo(pClsDb) = False Then
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   操作者取得（専従職員）
            '-------------------------------------------------------------------------------
            ' 専従職員の場合、専従職員フラグをTrueにする。
            If GetOperatorNamefullTimeStaf(pClsDb) = False Then
                Call CLMsg.Show("FE0001")
                Return blnRet
            End If

            '-------------------------------------------------------------------------------
            '   操作者取得（組合員属性）
            '-------------------------------------------------------------------------------
            ' 専従職員フラグが False の場合、組合員属性から名前を取得する。
            If blnSenjyuFlg = False Then
                If GetOperatorNameAttribute(pClsDb) = False Then
                    Call CLMsg.Show("BE0009")
                    Return blnRet
                End If
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
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetOfficeTermInfo
    '   名称　：会社・期情報取得処理
    '   概要  ：選択された期を元に会社コード、会社名、期From、期Toを取得する。
    '   引数　：ByVal clsMdb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>会社・期情報取得処理</summary>
    ''' <param name="clsMdb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetOfficeTermInfo(ByVal clsMdb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim strSql As String = ""                                                           ' SQL文
        Dim tbRet As DataTable = Nothing                                                    ' 処理結果データテーブル
        Dim intRet As Integer = Nothing                                                     ' 処理結果件数
        Dim strPeriodId As String = ""                                                      ' 期ID

        Try
            '-------------------------------------------------------------------------------
            '   最新期フラグ取得
            '-------------------------------------------------------------------------------
            If Me.cboPeriod.SelectedIndex = 1 Then
                MDLoginInfo.PeriodNewFlg = 1                                                ' 最新期の場合、1
            Else
                MDLoginInfo.PeriodNewFlg = 0                                                ' 最新期ではない場合、0
            End If

            '-------------------------------------------------------------------------------
            '   会社・期情報取得
            '-------------------------------------------------------------------------------
            strPeriodId = Me.cboPeriod.SelectedValue.ToString()                             ' 期ID取得

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT period.c_period_id" & vbCrLf                         ' 期ID
            strSql = strSql & "       ,period.l_name" & vbCrLf                              ' 期名称（第？？期）
            strSql = strSql & "       ,period.l_omission_name" & vbCrLf                     ' 略名称（期数値のみ）
            strSql = strSql & "       ,period.d_from" & vbCrLf                              ' 期From
            strSql = strSql & "       ,period.d_to" & vbCrLf                                ' 期To
            strSql = strSql & "       ,ksh.c_ksh" & vbCrLf                                  ' 会社コード
            strSql = strSql & "       ,ksh.n_ksh" & vbCrLf                                  ' 会社名
            strSql = strSql & "   FROM period" & vbCrLf                                     ' 期マスタ
            strSql = strSql & "       ,ksh" & vbCrLf                                        ' 会社マスタ
            strSql = strSql & "  WHERE period.c_ksh = ksh.c_ksh" & vbCrLf                   ' 期マスタの会社コードと会社マスタの会社コードが同じもの
            strSql = strSql & "    AND period.c_period_id = '" & strPeriodId & "'" & vbCrLf ' 期マスタの期IDが選択期IDと同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)

            ' 処理結果件数取得
            intRet = tbRet.Rows.Count

            ' 件数チェック
            If intRet = 1 Then
                MDLoginInfo.PeriodId = tbRet.Rows(0).Item(0).ToString       ' 期ID
                MDLoginInfo.PeriodName = tbRet.Rows(0).Item(1).ToString     ' 期名称（第？？期）
                MDLoginInfo.Period = tbRet.Rows(0).Item(2).ToString         ' 略名称（期数値のみ）
                MDLoginInfo.PeriodFrom = tbRet.Rows(0).Item(3).ToString     ' 期From
                MDLoginInfo.PeriodTo = tbRet.Rows(0).Item(4).ToString       ' 期To
                MDLoginInfo.Ksh = tbRet.Rows(0).Item(5).ToString            ' 会社コード
                MDLoginInfo.KshName = tbRet.Rows(0).Item(6).ToString        ' 会社名
            ElseIf intRet = 0 Then
                Call MessageBox.Show("選択された期IDに対するデータが存在しません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Return blnRet
            Else
                Call MessageBox.Show("選択された期IDに対するデータが複数存在します", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
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

        ' 戻り値格納
        Return blnRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeInfo
    '   名称　：所属委員会情報設定処理
    '   概要  ：所属委員会が1件の場合、所属委員会情報をログイン情報に設定する。
    '           複数件ある場合、ログイン情報は設定せず、件数のみを返す。
    '   引数　：ByVal clsMdb As CLAccessMdb = データベースクラス
    '   戻り値：GetCommitteeCnt = 所属委員会件数
    '   作成日：2011/11/20(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/20(日)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>所属委員会情報設定処理</summary>
    ''' <returns>所属委員会件数</returns>
    ''' <remarks></remarks>
    Private Function GetCommitteeInfo(ByVal clsMdb As CLAccessMdb) As Integer

        Dim intRet As Integer = Nothing             ' 処理結果件数
        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim strTargetDate As String = ""            ' 対象日付（yyyyMMdd）
        Dim strDateNow As String = ""               ' 対象日付現在日（yyyyMMdd）

        Try
            ' 現在日取得
            strDateNow = Now.ToString("yyyyMMdd")

            ' 期マスタから各期の適用開始日を取得する
            strSql = strSql & "" & vbCrLf
            strSql = strSql & " SELECT c_period_id" & vbCrLf
            strSql = strSql & "   FROM period " & vbCrLf
            strSql = strSql & "  WHERE d_from <= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & "    AND d_to   >= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)

            ' 件数取得
            intRet = tbRet.Rows.Count

            ' 件数チェック
            If intRet = 1 Then
                If tbRet.Rows(0).Item(0).ToString() = MDLoginInfo.PeriodId Then
                    strTargetDate = strDateNow
                Else
                    strTargetDate = MDLoginInfo.PeriodTo
                End If
            Else
                strTargetDate = MDLoginInfo.PeriodTo
            End If

            ' SQL作成
            strSql = "" & vbCrLf
            strSql = strSql & "   SELECT DISTINCT " & vbCrLf
            strSql = strSql & "          prod.l_omission_name AS period_no" & vbCrLf
            strSql = strSql & "         ,comt.l_name AS CommitteeName" & vbCrLf
            strSql = strSql & "         ,cmtd.l_name AS PostName" & vbCrLf
            strSql = strSql & "         ,comt.d_from" & vbCrLf
            strSql = strSql & "         ,comt.d_to" & vbCrLf
            strSql = strSql & "         ,comt.c_committee_id" & vbCrLf
            strSql = strSql & "         ,cmtd.s_committee_seq" & vbCrLf
            strSql = strSql & "     FROM period AS prod" & vbCrLf
            strSql = strSql & "         ,( SELECT a2.c_committee_list" & vbCrLf
            strSql = strSql & "                  ,a2.c_period_id" & vbCrLf
            strSql = strSql & "                  ,a2.c_committee_id" & vbCrLf
            strSql = strSql & "                  ,a2.d_from" & vbCrLf
            strSql = strSql & "              FROM committee_list AS a2" & vbCrLf
            strSql = strSql & "                  ,( SELECT a.c_period_id" & vbCrLf
            strSql = strSql & "                           ,a.c_committee_id" & vbCrLf
            strSql = strSql & "                           ,MAX(a.d_from) AS d_from" & vbCrLf
            strSql = strSql & "                       FROM committee_list AS a" & vbCrLf
            strSql = strSql & "                      WHERE a.d_from <= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & "                      GROUP BY a.c_period_id" & vbCrLf
            strSql = strSql & "                              ,a.c_committee_id" & vbCrLf
            strSql = strSql & "                      ORDER BY a.c_period_id" & vbCrLf   'ok
            strSql = strSql & "                              ,a.c_committee_id" & vbCrLf
            strSql = strSql & "                      " & UtDb.DbOrderOffset() & " ) AS a1" & vbCrLf
            strSql = strSql & "             WHERE a1.c_period_id = a2.c_period_id" & vbCrLf
            strSql = strSql & "               AND a1.c_committee_id = a2.c_committee_id" & vbCrLf
            strSql = strSql & "               AND a1.d_from = a2.d_from ) AS cmtl" & vbCrLf
            strSql = strSql & "         ,( SELECT b2.c_committee_list" & vbCrLf
            strSql = strSql & "                  ,b2.c_user_id" & vbCrLf
            strSql = strSql & "                  ,b2.c_committee_id" & vbCrLf
            strSql = strSql & "                  ,b2.s_committee_seq" & vbCrLf
            strSql = strSql & "                  ,b2.d_from" & vbCrLf
            strSql = strSql & "              FROM committee_list_dtl AS b2" & vbCrLf
            strSql = strSql & "                  ,( SELECT b.c_user_id" & vbCrLf
            strSql = strSql & "                           ,b.c_committee_id" & vbCrLf
            strSql = strSql & "                           ,b.s_committee_seq" & vbCrLf
            strSql = strSql & "                           ,b.d_from" & vbCrLf
            strSql = strSql & "                       FROM committee_list_dtl AS b" & vbCrLf
            strSql = strSql & "                      WHERE b.d_from <= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & "                      ORDER BY b.c_user_id" & vbCrLf 'ok
            strSql = strSql & "                              ,b.c_committee_id" & vbCrLf
            strSql = strSql & "                              ,b.s_committee_seq" & vbCrLf
            strSql = strSql & "                      " & UtDb.DbOrderOffset() & " ) AS b1" & vbCrLf
            strSql = strSql & "             WHERE b1.c_user_id = b2.c_user_id" & vbCrLf
            strSql = strSql & "               AND b1.c_committee_id = b2.c_committee_id" & vbCrLf
            strSql = strSql & "               AND b1.s_committee_seq = b2.s_committee_seq" & vbCrLf
            strSql = strSql & "               AND b1.d_from = b2.d_from ) AS cmld" & vbCrLf
            strSql = strSql & "         ,( SELECT c.c_committee_id" & vbCrLf
            strSql = strSql & "                  ,c.s_committee_seq" & vbCrLf
            strSql = strSql & "                  ,c.l_name" & vbCrLf
            strSql = strSql & "                  ,MAX(c.d_from) AS d_from_max" & vbCrLf
            strSql = strSql & "              FROM committee_dtl AS c" & vbCrLf
            strSql = strSql & "             WHERE c.d_from <= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "             AND   c.d_to   >= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "             GROUP BY c.c_committee_id" & vbCrLf
            strSql = strSql & "                     ,c.s_committee_seq" & vbCrLf
            strSql = strSql & "                     ,c.l_name" & vbCrLf
            strSql = strSql & "                     ,c.d_from" & vbCrLf
            strSql = strSql & "             ORDER BY c.c_committee_id" & vbCrLf 'ok
            strSql = strSql & "                     ,c.s_committee_seq" & vbCrLf
            strSql = strSql & "             " & UtDb.DbOrderOffset() & vbCrLf
            strSql = strSql & "             ) AS cmtd" & vbCrLf
            strSql = strSql & "         ,( SELECT d.c_committee_id" & vbCrLf
            strSql = strSql & "                  ,d.l_name" & vbCrLf
            strSql = strSql & "                  ,d.d_to" & vbCrLf
            strSql = strSql & "                  ,MAX(d.d_from) AS d_from" & vbCrLf
            strSql = strSql & "              FROM committee AS d" & vbCrLf
            strSql = strSql & "             WHERE d.d_from <= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "             AND   d.d_to   >= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "             GROUP BY d.c_committee_id" & vbCrLf
            strSql = strSql & "                     ,d.l_name" & vbCrLf
            strSql = strSql & "                     ,d.d_to" & vbCrLf
            strSql = strSql & "             ORDER BY d.c_committee_id" & vbCrLf 'ok
            strSql = strSql & "             " & UtDb.DbOrderOffset() & " ) AS comt" & vbCrLf
            strSql = strSql & "     WHERE cmld.c_user_id   = '" & MDLoginInfo.UserId & "'" & vbCrLf
            strSql = strSql & "       AND cmtl.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            strSql = strSql & "       AND comt.d_from     <= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "       AND comt.d_to       >= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "       AND prod.c_period_id      = cmtl.c_period_id" & vbCrLf
            strSql = strSql & "       AND cmtl.c_committee_list = cmld.c_committee_list" & vbCrLf
            strSql = strSql & "       AND cmld.c_committee_id   = cmtd.c_committee_id" & vbCrLf
            strSql = strSql & "       AND cmld.s_committee_seq  = cmtd.s_committee_seq" & vbCrLf
            strSql = strSql & "       AND cmtl.c_committee_id   = comt.c_committee_id" & vbCrLf
            ' 管理部マスタ情報を連結
            strSql = strSql & "   UNION ALL" & vbCrLf
            strSql = strSql & "   SELECT prod.l_omission_name AS period_no" & vbCrLf
            strSql = strSql & "         ,dpmt.l_name AS CommitteeName" & vbCrLf
            strSql = strSql & "         ,dpmd.l_name AS PostName" & vbCrLf
            strSql = strSql & "         ,dpmt.d_from" & vbCrLf
            strSql = strSql & "         ,dpmt.d_to" & vbCrLf
            strSql = strSql & "         ,dpmt.c_department_id AS c_committee_id" & vbCrLf
            strSql = strSql & "         ,dpmd.s_department_seq AS s_committee_seq" & vbCrLf
            strSql = strSql & "     FROM period AS prod" & vbCrLf
            strSql = strSql & "         ,( SELECT e2.c_department_list" & vbCrLf
            strSql = strSql & "                  ,e2.c_period_id" & vbCrLf
            strSql = strSql & "                  ,e2.c_department_id" & vbCrLf
            strSql = strSql & "            FROM department_list AS e2" & vbCrLf
            strSql = strSql & "                  ,( SELECT e.c_period_id" & vbCrLf
            strSql = strSql & "                           ,e.c_department_id" & vbCrLf
            strSql = strSql & "                           ,MAX(e.c_department_list) AS c_department_list" & vbCrLf
            strSql = strSql & "                       FROM department_list AS e" & vbCrLf
            strSql = strSql & "                      WHERE RIGHT(e.c_department_list,8) <= '" & strDateNow & "'" & vbCrLf
            strSql = strSql & "                      GROUP BY e.c_period_id" & vbCrLf
            strSql = strSql & "                              ,e.c_department_id" & vbCrLf
            strSql = strSql & "                      ORDER BY e.c_period_id" & vbCrLf   'ok
            strSql = strSql & "                              ,e.c_department_id " & vbCrLf
            strSql = strSql & "                      " & UtDb.DbOrderOffset() & " ) AS e1" & vbCrLf
            strSql = strSql & "            WHERE e1.c_period_id = e2.c_period_id" & vbCrLf
            strSql = strSql & "              AND e1.c_department_id = e2.c_department_id" & vbCrLf
            strSql = strSql & "              AND e1.c_department_list = e2.c_department_list ) AS dpml" & vbCrLf
            strSql = strSql & "         ,department_list_dtl AS dpld" & vbCrLf
            strSql = strSql & "         ,department_dtl AS dpmd" & vbCrLf
            strSql = strSql & "         ,department AS dpmt" & vbCrLf
            strSql = strSql & "     WHERE dpld.c_user_id   = '" & MDLoginInfo.UserId & "'" & vbCrLf
            strSql = strSql & "       AND dpml.c_period_id = '" & MDLoginInfo.PeriodId & "'" & vbCrLf
            strSql = strSql & "       AND dpmt.d_from     <= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "       AND dpmt.d_to       >= '" & strTargetDate & "'" & vbCrLf
            strSql = strSql & "       AND prod.c_period_id       = dpml.c_period_id" & vbCrLf
            strSql = strSql & "       AND dpml.c_department_list = dpld.c_department_list" & vbCrLf
            strSql = strSql & "       AND dpld.c_department_id   = dpmd.c_department_id" & vbCrLf
            strSql = strSql & "       AND dpld.s_department_seq  = CStr(dpmd.s_department_seq)" & vbCrLf
            strSql = strSql & "       AND dpml.c_department_id   = dpmt.c_department_id" & vbCrLf
            strSql = strSql & " ;" & vbCrLf

            'MSSQL OK
            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)

            ' 件数取得
            intRet = tbRet.Rows.Count

            ' 件数チェック
            If intRet = 1 Then
                ' 1件のみ所属委員会情報設定
                MDLoginInfo.CommitteeName = tbRet.Rows(0).Item(1).ToString()                ' 所属委員会名称
                MDLoginInfo.CommitteeId = tbRet.Rows(0).Item(5).ToString()                  ' 所属委員会ID
                MDLoginInfo.PostId = tbRet.Rows(0).Item(6).ToString()                       ' 役職ID
                MDLoginInfo.PostName = tbRet.Rows(0).Item(2).ToString()                     ' 役職名称
                ' 委員会ステータスフラグ（0：専従, 1：委員会, 2：管理部）取得
                ' 管理部用委員会IDリスト取得
                If MDLoginInfo.CommitteeId.Substring(0, 1) = "M" Then
                    MDLoginInfo.CommitteeStatusFlg = 2                                      ' 委員会ステータスフラグ（2：管理部）
                    ' 管理部委員会IDリスト取得処理
                    If getDepartmentCommitteeIdList(clsMdb, MDLoginInfo.CommitteeId) = False Then
                        Exit Function
                    End If
                Else
                    MDLoginInfo.CommitteeStatusFlg = 1                                      ' 委員会ステータスフラグ（1：委員会）
                    MDLoginInfo.CommitteeIdList.Clear()
                End If
            End If

            ' 処理結果に件数設定
            Return intRet

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
        Return intRet

    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetCommitteeInfoSenjyu
    '   名称　：専従職員所属委員会情報設定処理
    '   概要  ：専従職員の所属委員会情報（委員会ID, 役職ID）を定数マスタ詳細から取得する。
    '   引数　：ByVal clsMdb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/14(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/14(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>専従職員所属委員会情報設定処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetCommitteeInfoSenjyu(ByVal clsMdb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim intRet As Integer = Nothing             ' 処理結果件数
        Dim strSql As String = ""                   ' SQL文
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル

        Try
            '-------------------------------------------------------------------------------
            '   所属委員会ID取得
            '-------------------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT constant_dtl.l_name" & vbCrLf
            strSql = strSql & "   FROM constant_dtl" & vbCrLf
            strSql = strSql & "  WHERE constant_dtl.c_constant = 'SENJYU_COMMITTEE_ID' " & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)

            ' 件数取得
            intRet = tbRet.Rows.Count

            ' 件数チェック
            If intRet = 1 Then
                ' 1件のみ所属委員会情報設定
                MDLoginInfo.CommitteeId = tbRet.Rows(0).Item(0).ToString()              ' 所属委員会ID
                MDLoginInfo.CommitteeName = ""                                          ' 所属委員会名称
            ElseIf intRet = 0 Then
                Call MessageBox.Show("専従職員の委員会IDが取得できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Exit Function
            Else
                Call MessageBox.Show("専従職員の委員会が複数あります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   役職ID取得
            '-------------------------------------------------------------------------------
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT constant_dtl.l_name" & vbCrLf
            strSql = strSql & "   FROM constant_dtl" & vbCrLf
            strSql = strSql & "  WHERE constant_dtl.c_constant = 'SENJYU_POST_ID' " & vbCrLf
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)

            ' 件数取得
            intRet = tbRet.Rows.Count

            ' 件数チェック
            If intRet = 1 Then
                ' 1件のみ所属委員会情報設定
                MDLoginInfo.PostId = tbRet.Rows(0).Item(0).ToString()                   ' 役職ID
                MDLoginInfo.PostName = ""                                               ' 役職名称
            ElseIf intRet = 0 Then
                Call MessageBox.Show("専従職員の役職IDが取得できません。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Exit Function
            Else
                Call MessageBox.Show("専従職員の役職IDが複数あります。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   各値取得
            '-------------------------------------------------------------------------------
            MDLoginInfo.CommitteeStatusFlg = 0      ' 委員会ステータスフラグ（0：専従, 1：委員会, 2：管理部）
            MDLoginInfo.CommitteeIdList.Clear()     ' 管理部用委員会IDリスト取得（無）

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
    '   ＩＤ　：GetOperatorNameAttribute
    '   名称　：組合員操作者取得処理（組合員属性）
    '   概要  ：入力された社員番号を元に組合員属性テーブルから名前を取得する。
    '   引数　：ByVal clsMdb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/21(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>組合員操作者取得処理（組合員属性）</summary>
    ''' <param name="clsMdb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetOperatorNameAttribute(ByVal clsMdb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim strSql As String = ""                           ' SQL文
        Dim tbRet As DataTable = Nothing                    ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing                  ' 処理結果件数
        Dim strStafId As String = ""                        ' 社員番号
        Dim strNow As String = Nothing                      ' 現在日付（yyyyMMdd）

        Try
            strStafId = Me.txtMemberNo.Text                                                         ' 社員番号取得
            strNow = Now.ToString("yyyyMMdd")                                                       ' 現在日付取得（yyyyMMdd）

            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT staf_attribute.l_name" & vbCrLf                              ' 名前
            strSql = strSql & "   FROM staf_attribute" & vbCrLf                                     ' 組合員属性
            ' 現在日付に対しての最新情報取得
            strSql = strSql & "       ,( SELECT staf.c_user_id" & vbCrLf                            ' 個人認証ID
            strSql = strSql & "                ,staf.c_ksh" & vbCrLf                                ' 会社コード
            strSql = strSql & "                ,staf.c_staf_id" & vbCrLf                            ' 社員番号
            strSql = strSql & "                ,MAX(staf.d_from) AS d_from_max" & vbCrLf            ' 適用日付条件内で最大
            strSql = strSql & "            FROM staf_attribute staf" & vbCrLf                       ' 組合員属性
            strSql = strSql & "           WHERE staf.c_user_id = '" & strStafId & "'" & vbCrLf      ' 組合員属性の個人認証IDが入力した社員番号と同じもの
            strSql = strSql & "             AND staf.c_ksh = '" & MDLoginInfo.Ksh & "'" & vbCrLf    ' 組合員属性の会社コードが選択した期の会社コードと同じもの
            strSql = strSql & "             AND staf.c_staf_id = '" & strStafId & "'" & vbCrLf      ' 組合員属性の社員番号が入力した社員番号と同じもの
            strSql = strSql & "             AND staf.d_from <= '" & strNow & "'" & vbCrLf           ' 組合員属性の適用日付が現在日付よりも小さいもの
            strSql = strSql & "           GROUP BY staf.c_user_id" & vbCrLf
            strSql = strSql & "                   ,staf.c_ksh" & vbCrLf
            strSql = strSql & "                   ,staf.c_staf_id ) AS stafPrimary" & vbCrLf
            strSql = strSql & "  WHERE staf_attribute.c_user_id = stafPrimary.c_user_id" & vbCrLf   ' 最新情報の個人認証IDと同じもの
            strSql = strSql & "    AND staf_attribute.c_ksh = stafPrimary.c_ksh" & vbCrLf           ' 最新情報の会社コードと同じもの
            strSql = strSql & "    AND staf_attribute.c_staf_id = stafPrimary.c_staf_id" & vbCrLf   ' 最新情報の社員番号と同じもの
            strSql = strSql & "    AND staf_attribute.d_from = stafPrimary.d_from_max" & vbCrLf     ' 最新情報の最大適用日付と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)

            ' 処理結果件数取得
            intCntRet = tbRet.Rows.Count

            ' 0件チェック
            If intCntRet = 1 Then
                MDLoginInfo.OperatorName = tbRet.Rows(0).Item(0).ToString       ' 名前取得
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
    '   ＩＤ　：GetOperatorNamefullTimeStaf
    '   名称　：専従職員操作者取得処理（専従職員マスタ）
    '   概要  ：入力された社員番号を元に専従職員マスタから名前を取得する。
    '   引数　：ByVal clsMdb As CLAccessMdb = データベースクラス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/08(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(金)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>操専従職員操作者取得処理（専従職員マスタ）</summary>
    ''' <param name="clsMdb">データベースクラス</param>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetOperatorNamefullTimeStaf(ByVal clsMdb As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False                                                               ' 処理結果
        Dim strSql As String = ""                                                                   ' SQL文
        Dim tbRet As DataTable = Nothing                                                            ' 処理結果データテーブル
        Dim intCntRet As Integer = Nothing                                                          ' 処理結果件数
        Dim strStafId As String = ""                                                                ' 社員番号
        Dim strNow As String = Nothing                                                              ' 現在日付（yyyyMMdd）

        Try
            strStafId = Me.txtMemberNo.Text                                                         ' 社員番号取得
            strNow = Now.ToString("yyyyMMdd")                                                       ' 現在日付取得（yyyyMMdd）
            ' SQL文作成
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT full_time_staf.l_name" & vbCrLf                              ' 名前
            strSql = strSql & "   FROM full_time_staf" & vbCrLf                                     ' 組合員属性
            ' 現在日付に対しての最新情報取得
            strSql = strSql & "       ,( SELECT senjyu.c_staf_id" & vbCrLf                          ' 認証番号
            strSql = strSql & "                ,MAX(senjyu.d_from) AS d_from_max" & vbCrLf          ' 適用日付条件内で最大
            strSql = strSql & "            FROM full_time_staf senjyu" & vbCrLf                     ' 専従職員マスタ
            strSql = strSql & "           WHERE senjyu.c_staf_id = '" & strStafId & "'" & vbCrLf    ' 専従職員マスタの認証番号が入力した社員番号と同じもの
            strSql = strSql & "             AND senjyu.d_from <= '" & strNow & "'" & vbCrLf         ' 専従職員マスタの適用日付が現在日付よりも小さいもの
            strSql = strSql & "             AND senjyu.d_to >= '" & strNow & "'" & vbCrLf           ' 専従職員マスタの適用日付が現在日付よりも小さいもの
            strSql = strSql & "           GROUP BY senjyu.c_staf_id ) AS senjyuPrimary" & vbCrLf
            strSql = strSql & "  WHERE full_time_staf.c_staf_id = senjyuPrimary.c_staf_id" & vbCrLf ' 最新情報の認証番号と同じもの
            strSql = strSql & "    AND full_time_staf.d_from = senjyuPrimary.d_from_max" & vbCrLf   ' 最新情報の最大適用日付と同じもの
            strSql = strSql & ";" & vbCrLf

            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)

            ' 処理結果件数取得
            intCntRet = tbRet.Rows.Count

            ' 0件チェック
            If intCntRet = 1 Then
                ' データ取得
                MDLoginInfo.OperatorName = tbRet.Rows(0).Item(0).ToString       ' 名前
                blnSenjyuFlg = True                                             ' 専従職員フラグをTrueにする。
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
    '   ＩＤ　：Initialize
    '   名称　：初期化処理
    '   概要  ：初期化処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function Initialize() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果

        Try
            '-------------------------------------------------------------------------------
            '   画面情報クリア処理
            '-------------------------------------------------------------------------------
            If ControlClear() = False Then
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   システム情報クリア処理
            '-------------------------------------------------------------------------------
            If ClearSystemInfo() = False Then
                Exit Function
            End If

            '-------------------------------------------------------------------------------
            '   ログイン情報クリア処理
            '-------------------------------------------------------------------------------
            If ClearLoginInfo() = False Then
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
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False                       ' 処理結果

        Try
            Me.txtMemberNo.Text = ""                        ' 社員番号クリア
            Me.txtPwd.Text = ""                             ' パスワードクリア
            Me.txtInfomation.Text = ""                      ' お知らせクリア
            Me.cboPeriod.DataSource = Nothing               ' 期リストクリア
            Me.cboPeriod.Text = ""

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
    '   ＩＤ　：ClearSystemInfo
    '   名称　：システム情報クリア処理
    '   概要  ：システム情報のグローバル変数をクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '       　：2012/08/14(火)  Fujisaku  マスタファイル情報追加
    '         ：2013/04/19(金)  Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    ''' <summary>システム情報クリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ClearSystemInfo() As Boolean

        Dim blnRet As Boolean = False                           ' 処理結果

        Try
            MDSystemInfo.DbConnectionString = ""                ' OleDbのConnectionString
            'MDSystemInfo.AccessProvider = ""                    ' ACCESS MDB プロバイダ
            'MDSystemInfo.AccessPath = ""                        ' ACCESS MDB ファイルパス
            'MDSystemInfo.AccessName = ""                        ' ACCESS MDB ファイル名
            'MDSystemInfo.AccessMstPath = ""                     ' ACCESS MDB マスタファイルパス
            'MDSystemInfo.AccessMstName = ""                     ' ACCESS MDB マスタファイル名
            'MDSystemInfo.AccessPersistSecurity = ""             ' ACCESS MDB セキュリティ警告
            'MDSystemInfo.AccessPassword = ""                    ' ACCESS MDB パスワード
            MDSystemInfo.MessagePath = ""                       ' メッセージファイルパス
            MDSystemInfo.MessageName = ""                       ' メッセージファイル名
            MDSystemInfo.SequencePath = ""                      ' SEQUENCE ファイルパス
            MDSystemInfo.EncryptKey = ""                        ' 暗号化キー（公開キー）

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
    '   ＩＤ　：ClearLoginInfo
    '   名称　：ログイン情報クリア処理
    '   概要  ：ログイン情報のグローバル変数をクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>ログイン情報クリア</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ClearLoginInfo() As Boolean

        Dim blnRet As Boolean = False                               ' 処理結果

        Try
            MDLoginInfo.UserId = ""                                 ' 個人認証ID
            MDLoginInfo.PeriodId = ""                               ' 期ID
            MDLoginInfo.PeriodName = ""                             ' 期名称
            MDLoginInfo.PeriodNewFlg = 0                            ' 最新期フラグ
            MDLoginInfo.OperatorName = ""                           ' 操作者
            MDLoginInfo.PeriodFrom = ""                             ' 期間From
            MDLoginInfo.PeriodTo = ""                               ' 期間To
            MDLoginInfo.PostId = ""                                 ' 役職ID
            MDLoginInfo.PostName = ""                               ' 役職名称
            MDLoginInfo.CommitteeName = ""                          ' 所属委員会名
            MDLoginInfo.CommitteeId = ""                            ' 所属委員会ID
            CommitteeStatusFlg = 0                                  ' 委員会ステータスフラグ（0：専従, 1：委員会, 2：管理部）
            MDLoginInfo.CommitteeIdList = New List(Of String)       ' 管理部用委員会IDリスト

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