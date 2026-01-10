#Region "FM040603"
'===========================================================================================================
'   クラスＩＤ　　：FM040603
'   クラス名称　　：発信文書ファイル名入力画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDCommon
Imports UnionAct.GUI.Document

Public Class FM040603

#Region "定数"
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM040603      ' FM040603
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM040603  ' 発信文書ファイル名入力画面
#End Region

#Region "プロパティ"
    Public _strFileName As String = ""                          ' ファイル名

    ' ファイル名
    Public Property strFileName() As String
        Get
            Return _strFileName
        End Get
        Set(ByVal value As String)
            _strFileName = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM040603_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2012/03/18(日)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/18(日)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM040603_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            '-------------------------------------------------------
            '   画面中央表示処理
            '-------------------------------------------------------
            If MDCommon.SetFormCenter(Me) = False Then
                Exit Sub
            End If

            '-------------------------------------------------------
            '   コントロールクリア処理
            '-------------------------------------------------------
            If Me.ControlClear() = False Then
                Exit Sub
            End If

            ' 常に最前面に表示
            Me.TopMost = True

            ' ファイル名テキストボックスにフォーカスセット
            Me.txtFileName.Focus()

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
    '   ＩＤ　：btnOK_Click
    '   名称　：OKボタンクリック処理
    '   概要　：
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim strWork As String = ""      ' 入力されたファイル名

        Try
            ' 入力チェック
            If Me.ChkInput() = False Then
                Exit Sub
            End If

            ' 拡張子付与
            strWork = Me.txtFileName.Text.Trim()
            If Not strWork.EndsWith(".xls", StringComparison.CurrentCultureIgnoreCase) Then
                strWork = (strWork & ".xls")
            End If

            ' ファイル名取得
            Me.strFileName = strWork

            ' ダイアログ結果（OK）
            Me.DialogResult = Windows.Forms.DialogResult.OK

            ' 画面非表示
            Me.Visible = False

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
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            ' ファイル名クリア
            Me.strFileName = ""

            ' ダイアログ結果（キャンセル）
            Me.DialogResult = Windows.Forms.DialogResult.Cancel

            ' 画面非表示
            Me.Visible = False

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
    '   ＩＤ　：txtFileName_GotFocus
    '   名称　：ファイル名テキストボックスフォーカス取得処理
    '   概要　：
    '   作成日：2012/03/16(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtFileName_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFileName.GotFocus

        Try
            ' 全選択
            Me.txtFileName.SelectAll()

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
    '   ＩＤ　：txtFileName_KeyDown
    '   名称　：ファイル名テキストボックスキーダウン処理
    '   概要  ：
    '   作成日：2012/03/21(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/21(水)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub txtFileName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFileName.KeyDown

        Try
            If e.KeyCode = Keys.Enter Then
                Call Me.btnOK_Click(sender, e)
                'Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
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
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果

        Try
            ' Label
            Me.lblMemo1.Visible = True          ' メモ1
            Me.lblMemo1.Enabled = True
            Me.lblMemo2.Visible = True          ' メモ2
            Me.lblMemo2.Enabled = True

            ' TextBox
            Me.txtFileName.Visible = True       ' ファイル名
            Me.txtFileName.Enabled = True
            Me.txtFileName.Text = ""

            ' Button
            Me.btnOK.Visible = True             ' OK
            Me.btnOK.Enabled = True
            Me.btnCancel.Visible = True         ' キャンセル
            Me.btnCancel.Enabled = True

            ' ファイル名入力テキストボックスにフォーカスセット
            Me.txtFileName.Focus()

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
    '   概要　：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/03/12(月)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/12(月)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ChkInput() As Boolean

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim blnRet As Boolean = False   ' 処理結果
        Dim strChk As String = ""       ' チェック文字列

        Try
            ' ファイル名取得
            strChk = Me.txtFileName.Text.Trim()

            ' 未入力チェック
            If MDChk.ChkNull(strChk) Then
                Call CLMsg.Show("GE0147")
                Return blnRet
            End If

            ' 文字数チェック
            If MDChk.ChkLength(strChk, 50) = False Then
                Call MessageBox.Show(("ファイル名は、50文字以内で入力してください"), _
                                     "エラー", _
                                     MessageBoxButtons.OK, _
                                     MessageBoxIcon.Warning, _
                                     MessageBoxDefaultButton.Button1)
                Return blnRet
            End If

            ' 禁止文字チェック
            'If MDChk.ChkProhibitionString(strChk) = False Then
            '    Call CLMsg.Show("GE0148")
            '    Return blnRet
            'End If
            If strChk.IndexOfAny(New Char() {"\"c, "/"c, ":"c, "*"c, "?"c, """"c, "<"c, ">"c, "["c, "]"c, "|"c, "'"c, ""}) >= 0 Then
                Call MessageBox.Show("ファイル名には次の文字は使えません。「" & "\" & "」「" & "/" & "」「" & ":" & "」「" & "*" & "」「" & "?" & "」「" & """" & "」「" & "<" & "」「" & ">" & "」「" & "[" & "」「" & "]" & "」「" & "|" & "」「" & "'" & "」「」", _
                                     "エラー", _
                                     MessageBoxButtons.OK, _
                                     MessageBoxIcon.Warning, _
                                     MessageBoxDefaultButton.Button1)
                Return blnRet
            End If

            '' ファイル名チェック
            'If (String.IsNullOrEmpty(strChk) _
            'OrElse strChk.Equals(".xls", StringComparison.CurrentCultureIgnoreCase)) Then
            '    Call MessageBox.Show("ファイル名を入力してください", _
            '                         "エラー", _
            '                         MessageBoxButtons.OK, _
            '                         MessageBoxIcon.Hand)
            '    Return blnRet

            'ElseIf (strChk.Length > 50) Then
            '    Call MessageBox.Show(("ファイル名は" & 50.ToString & "文字以内で入力してください"), _
            '                         "エラー", _
            '                         MessageBoxButtons.OK, _
            '                         MessageBoxIcon.Hand)
            '    Return blnRet

            'ElseIf (strChk.Length = 0) _
            'OrElse strChk.Equals(".xls", StringComparison.CurrentCultureIgnoreCase) Then
            '    Call MessageBox.Show("ファイル名を入力してください", _
            '                         "エラー", _
            '                         MessageBoxButtons.OK, _
            '                         MessageBoxIcon.Hand)
            '    Return blnRet

            'ElseIf (strChk.IndexOfAny(New Char() {"\"c, "/"c, ":"c, "*"c, "?"c, """"c, "<"c, ">"c, "["c, "]"c, "|"c, "'"c, ""}) >= 0) Then
            '    Call MessageBox.Show("ファイル名には次の文字は使えません。「" & "\" & "」「" & "/" & "」「" & ":" & "」「" & "*" & "」「" & "?" & "」「" & """" & "」「" & "<" & "」「" & ">" & "」「" & "[" & "」「" & "]" & "」「" & "|" & "」「" & "'" & "」「」", _
            '                         "エラー", _
            '                         MessageBoxButtons.OK, _
            '                         MessageBoxIcon.Hand)
            '    Return blnRet

            'End If

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