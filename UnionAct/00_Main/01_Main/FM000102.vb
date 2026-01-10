#Region "FM000102"
'===========================================================================================================
'   クラスＩＤ　　：FM000102
'   クラス名称　　：メイン操作画面
'   備考  　　　　：
'===========================================================================================================

Imports System.Data.OleDb
Imports System.ComponentModel
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDCommon

Public Class FM000102

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM000102          ' FM000102
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM000102      ' メイン操作画面
    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000102_Load
    '   名称　：フォームロード
    '   概要　：ラベルの設定、メニュー生成を行う。
    '   作成日：2011/11/10(木)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/10(木)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub FM000102_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim strLog As String = ""                                                           ' ログ出力用
        Try
            Me.FormBorderStyle = FormBorderStyle.FixedSingle                                ' 画面サイズ変更不可
            If SetFormCenter(Me) = False Then                                               ' 画面中央表示処理
                Exit Sub
            End If
            Me.dgdMenu.Focus()                                                              ' フォーカスの変更
            strLog = " " & MDLoginInfo.OperatorName & "(" & UserId & ")さんが「" & CommitteeName & "」を選択されました。"
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & strLog)
            Call subSetLabel()                                                              ' ラベルの設定
            Call subCreateMenu()                                                            ' 権限に応じて表示するメニュー変更
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btEnd_Click
    '   名称　：終了ボタン
    '   概要　：メインメニューを閉じる。
    '   作成日：2011/11/08(火)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnd.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim DiaRet As DialogResult = Nothing                                                ' メッセージボタン押下結果
        Dim strLog As String = ""                                                           ' ログ用文字列
        Try
            DiaRet = CLMsg.Show("GQ0025")                                                   ' 終了確認メッセージボックス表示
            If DiaRet = vbYes Then                                                          ' 「はい」押下時
                Me.Close()
                strLog = OperatorName & "(" & UserId & ")さんが総合ＯＡを終了しました。"
                log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & strLog)
            Else                                                                            ' 「いいえ」押下時
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
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btLogOff_Click
    '   名称　：ログオフボタン
    '   概要　：ログイン画面を表示して、メインメニューを閉じる。
    '   作成日：2011/11/08(火)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnLogOff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogOff.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim DiaRet As DialogResult = Nothing                                                ' メッセージボタン押下結果
        Dim strLog As String = ""                                                           ' ログ出力用文字列
        Try
            DiaRet = CLMsg.Show("GQ0026")                                                   ' 終了確認メッセージボックス表示
            If DiaRet = vbYes Then                                                          ' 「はい」押下時
                strLog = OperatorName & "(" & UserId & ")さんがログオフされました。"
                log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & strLog)
                Call FM000101.Show()
                Call Me.Close()
            Else                                                                            ' 「いいえ」押下時
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
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：llbChangePassword_LinkClicked
    '   名称　：パスワード変更画面呼び出し
    '   概要　：パスワード変更画面を呼び出す。
    '   作成日：2011/11/08(火)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub llbChangePassword_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles llbChangePassword.LinkClicked
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim cForm1 As New FM000103()
        Try
            cForm1.ShowDialog()                                                             ' モーダルで表示する
            cForm1.Dispose()                                                                ' 不要になった時点で破棄する
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnChangeAuthority_Click
    '   名称　：操作権限変更画面呼び出し
    '   概要　：操作権限変更画面を呼び出す。
    '   作成日：2011/11/08(火)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnChangeAuthority_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChangeAuthority.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim cForm1 As New FM000104()
        Dim strAuthority As String = ""
        Dim strLog As String = ""
        Dim panel_uc As Control = Nothing
        Dim uc As Control = Nothing
        Try
            cForm1.ShowDialog()                                                             ' モーダルで表示する
            cForm1.Dispose()                                                                ' 不要になった時点で破棄する
            strAuthority = "『" & MDLoginInfo.CommitteeName & "（" & MDLoginInfo.PostName & "）』の操作権限でログインしています"
            Me.lblLogInAuthority.Text = strAuthority                                        ' ログインしているユーザーの権限を設定
            If MDLoginInfo.CommitteeStatusFlg = 1 Then
                Me.btnSync.Text = "情報更新"
            Else
                Me.btnSync.Text = "情報管理"
            End If
            Call subCreateMenu()                                                            ' メニュー表示変更
            '' 3月対応箇所グレーアウト　**********************************
            ''全項目リリース時に削除
            'If funcSetColor() = False Then
            '    Exit Sub
            'End If
            ''***********************************************************
            strLog = " " & OperatorName & "(" & UserId & ")さんが「" & CommitteeName & "」を選択されました。"
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & strLog)
            ' パネルに開かれているコントロールを閉じる
            For i = Me.pnlMain.Controls.Count - 1 To 0 Step -1
                panel_uc = Me.pnlMain.Controls(i)
                Call Me.pnlMain.Controls.Remove(panel_uc)
            Next
            Me.pnlMain.Controls.Add(Me.lblSelectMenu)
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：dgdMenu_MouseMove
    '   名称　：
    '   概要　：
    '   作成日：2011/11/08(火)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub dgdMenu_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgdMenu.MouseMove
        Try
            Cursor.Current = Cursors.Hand
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
    '   ＩＤ　：dgdMenu_RowPostPaint
    '   名称　：メニューデザイン
    '   概要　：カテゴリ列に下線を追加する。
    '   作成日：2011/11/08(火)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub dgdMenu_RowPostPaint(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewRowPostPaintEventArgs) Handles dgdMenu.RowPostPaint
        '### メインメニューのカテゴリ行に、水色の下線を引く
        Dim rowpen As New Pen(Color.SkyBlue, 2) '罫線の色、太さを設定
        Dim cb As System.Drawing.Rectangle
        Try
            ' カテゴリの行か？
            If Me.dgdMenu.Rows(e.RowIndex).Cells(1).Value = "" And _
               Me.dgdMenu.Rows(e.RowIndex).Cells(2).Value <> "" Then
                cb = e.RowBounds
                cb.Offset(0, -1) ' 太さ2ピクセルの罫線を引くため、-1ピクセルずらす
                e.Graphics.DrawLine(rowpen, cb.Left, cb.Bottom, cb.Right, cb.Bottom)        ' 罫線を引く
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
    '   ＩＤ　：dgdMenu_DoubleClick
    '   名称　：メニュー選択
    '   概要　：選択したメニューに応じた画面を表示する。
    '   作成日：2011/11/08(火)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub dgdMenu_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgdMenu.DoubleClick
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim dgv As DataGridView
        Dim row As DataGridViewRow
        Dim strGamenID As String
        Dim type As Type
        Dim uc As Control
        Dim panel_uc As Control
        Dim fm As Form
        Dim i As Integer
        Try
            dgv = Me.dgdMenu
            row = dgv.CurrentRow '選択中の行を取得
            '画面IDの値を取得
            If TypeOf row.Cells(3).Value Is DBNull Then
                strGamenID = ""
            Else
                strGamenID = row.Cells(3).Value
            End If
            '背景色初期化→白
            For Each r As DataGridViewRow In dgv.Rows
                r.Cells.Item(2).Style.BackColor = Color.White
                r.Cells.Item(2).Style.SelectionBackColor = Color.White
                r.Cells.Item(2).Style.ForeColor = Color.Black
                r.Cells.Item(2).Style.SelectionForeColor = Color.Black
            Next
            '画面IDが設定されているか？
            If strGamenID <> "" Then

                '画面IDに該当するコントロールのTypeオブジェクトを取得
                type = type.GetType("UnionAct." & strGamenID)
                'Typeオブジェクトが取得できたか？
                If Not type Is Nothing Then
                    If strGamenID.Substring(0, 2) = "UC" Then
                        'コントロールのインスタンスを生成
                        uc = CType(Activator.CreateInstance(type), Control)
                        'パネルに開かれているコントロールを閉じる
                        For i = Me.pnlMain.Controls.Count - 1 To 0 Step -1
                            panel_uc = Me.pnlMain.Controls(i)
                            Call Me.pnlMain.Controls.Remove(panel_uc)
                        Next
                        '新しいコントロールをパネルに追加する
                        Call Me.pnlMain.Controls.Add(uc)
                    Else
                        '重複表示防止
                        Dim list As Form() = Me.OwnedForms
                        Dim aform As Form = Nothing
                        For Each form As Form In list
                            If form.Name = type.Name Then
                                aform = form
                                aform.Activate()
                                Exit For
                            End If
                        Next
                        If aform Is Nothing Then
                            'フォームのインスタンスを生成
                            fm = CType(Activator.CreateInstance(type), Form)
                            'フォームを開く
                            Call fm.Show(Me)
                        End If
                    End If
                    '選択された箇所の背景色を変更する。
                    row.Cells.Item(2).Style.BackColor = System.Drawing.SystemColors.Highlight
                    row.Cells.Item(2).Style.SelectionBackColor = System.Drawing.SystemColors.Highlight
                    row.Cells.Item(2).Style.ForeColor = System.Drawing.SystemColors.HighlightText
                    row.Cells.Item(2).Style.SelectionForeColor = System.Drawing.SystemColors.HighlightText
                    'ボタン名制御
                    If strGamenID = "UC020401" Or strGamenID = "UC030301" Then
                        Me.btnSync.Text = "出欠更新"
                    Else
                        Me.btnSync.Text = "情報更新"
                    End If
                Else
                    MsgBox("該当する画面がありません (画面ID = " & strGamenID & ")")
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    ''***************************************************************************************************
    ''   ＩＤ　：BtnSync_Click
    ''   名称　：データ更新ボタン
    ''   概要　：デザインマスタとの同期を手動で行う
    ''   作成日：2012/08/14(火)  Fujisaku
    ''   更新日：
    ''---------------------------------------------------------------------------------------------------
    ''   履歴　：2012/08/14(火)  Fujisaku  新規作成
    ''***************************************************************************************************
    'Private Sub BtnSync_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSync.Click
    '    log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
    '    Try
    '        If MDLoginInfo.CommitteeStatusFlg = 1 Then
    '            ' 同期確認メッセージボックス表示
    '            If CLMsg.Show("GQ0100") = vbYes Then                                            ' 「はい」押下時
    '                syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, True)
    '            Else                                                                            ' 「いいえ」押下時
    '                Exit Sub
    '            End If
    '        Else
    '            ' データメンテ用ダイアログ表示
    '            Dim cForm1 As New FM000207()

    '            ' モーダルで表示する
    '            cForm1.ShowDialog()

    '            ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
    '            cForm1.Dispose()
    '        End If
    '    Catch ex As Exception
    '        log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
    '        ' 致命的エラーメッセージボックス表示
    '        Call CLMsg.ShowEtarnal(Err.Number, _
    '                               Err.Description, _
    '                               SCREEN_ID, SCREEN_NAME, _
    '                               System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '    End Try
    '    log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    'End Sub

    '***************************************************************************************************
    '   ＩＤ　：form_closing
    '   名称　：フォーム終了
    '   概要　：ログオフ、閉じる、[x]ボタン、Alt+F4による終了時に同期処理を実行する
    '   作成日：2012/08/14(火)  Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/08/14(火)  Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub form_closing(ByVal sender As System.Object, _
                         ByVal e As System.ComponentModel.CancelEventArgs _
                         ) Handles MyBase.Closing
        '' 同期実施
        'If MDSystemInfo.DebugMode = False AndAlso CLMsg.Show("GQ0100") = vbYes Then
        '    If Not syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, True) Then
        '        e.Cancel = True
        '    End If
        'End If
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：subSetLabel
    '   名称　：ラベル設定処理
    '   概要　：ログインユーザーの情報をラベルに設定する。
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/08(火)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>ラベル設定処理</summary>
    ''' <remarks></remarks>
    Private Sub subSetLabel()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim strTerm As String = ""
        Dim strUserInfo As String = ""
        Dim strAuthority As String = ""
        Try
            strTerm = MDLoginInfo.PeriodName
            strUserInfo = MDLoginInfo.UserId & " " & MDLoginInfo.OperatorName
            If ChkNull(MDLoginInfo.CommitteeName) And ChkNull(MDLoginInfo.PostName) Then
                strAuthority = ""
            Else
                strAuthority = "『" & MDLoginInfo.CommitteeName & "（" & MDLoginInfo.PostName & "）』の操作権限でログインしています"
            End If
            Me.lblLogInTerm.Text = strTerm                                                  ' ログインしているユーザーの期を設定
            Me.lblLogInUserInfo.Text = strUserInfo                                          ' ログインしているユーザーの情報（社番+氏名）を設定
            Me.lblLogInAuthority.Text = strAuthority                                        ' ログインしているユーザーの権限を設定
            If MDLoginInfo.CommitteeStatusFlg = 1 Then
                Me.btnSync.Text = "情報更新"
            Else
                Me.btnSync.Text = "情報管理"
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：subCreateMenu
    '   名称　：メニュー生成
    '   概要　：ログインユーザーの権限にあわせたメニューを表示する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/10(木)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/10(木)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>メニュー生成</summary>
    ''' <remarks></remarks>
    Private Sub subCreateMenu()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim clsMdb As New CLAccessMdb                                                       ' データベースクラス生成
        Dim dt As New DataTable                                                             ' SQL実行結果格納テーブル
        Dim row As DataRow                                                                  ' データ行
        Dim dgv As DataGridView                                                             ' メインメニュー
        Dim col(7) As DataGridViewTextBoxColumn                                             ' メインメニューのセルコレクション
        Dim strCategory As String                                                           ' カテゴリ名称
        Dim strPreCategory As String                                                        ' 一行前のカテゴリ名称
        Dim i As Integer
        Dim cnt As Integer
        Try
            dt = funcCreateMenu()
            If dt.Rows.Count = 0 Then
                CLMsg.Show("GE0090", "表示可能なメニュー")
            End If
            Me.dgdMenu.Columns.Clear()
            dgv = Me.dgdMenu
            dgv.ColumnHeadersVisible = False                                                ' 列ヘッダーなし
            dgv.RowHeadersVisible = False                                                   ' 行ヘッダーなし
            dgv.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.None      ' 境界線なし
            ' DataGridViewに列を追加する
            For i = 0 To dt.Columns.Count - 1
                col(i) = New DataGridViewTextBoxColumn
                dgv.Columns.Add(col(i))
                Select Case i
                    Case 2
                        ' メニュー名称のみ表示する
                        dgv.Columns(i).Width = 198
                    Case Else
                        ' それ以外の列は非表示にする
                        dgv.Columns(i).Visible = False
                End Select
            Next
            ' 初期値設定
            strCategory = ""
            strPreCategory = ""
            ' テーブルの検索結果をDataGridViewに表示する
            For i = 0 To dt.Rows.Count - 1
                row = dt.Rows(i)
                ' カテゴリ名称取得
                If strCategory <> "" Then
                    strPreCategory = strCategory
                End If
                strCategory = row.ItemArray(1)
                ' 最初の行または1行前のカテゴリ名と相違が合う場合
                If i = 0 Then
                    ' カテゴリ行の追加
                    Call dgv.Rows.Add("")
                    ' 追加したカテゴリ行にカテゴリ名称を設定
                    cnt = dgv.Rows.Count - 1
                    dgv.Rows(cnt).Cells(2).Value = strCategory
                    dgv.Rows(cnt).DefaultCellStyle.Font = New Font("Arial", 13, FontStyle.Bold) ' フォントを設定
                Else
                    If Not (strCategory = strPreCategory) Then
                        ' カテゴリ行の前は一行空ける(1行は空白、1行はカテゴリ行)
                        Call dgv.Rows.Add(2)
                        ' 追加したカテゴリ行にカテゴリ名称を設定
                        cnt = dgv.Rows.Count - 1
                        dgv.Rows(cnt).Cells(2).Value = strCategory
                        dgv.Rows(cnt).DefaultCellStyle.Font = New Font("Arial", 13, FontStyle.Bold) ' フォントを設定
                    End If
                End If
                Call dgv.Rows.Add(row.ItemArray)
            Next
            ' 後処理
            dt.Dispose()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：funcCreateMenu
    '   名称　：メニュー生成
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/10(木)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/10(木)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>メニュー生成</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function funcCreateMenu() As DataTable
        Dim dtRet As DataTable = Nothing
        Dim sql As String = ""
        Dim getSql As String = ""
        Dim dt As DataTable = Nothing
        Dim loginPeriodfrom As String = ""
        Dim PeriodFrom As String = ""
        Dim clsMDB As New CLAccessMdb
        Dim HitPeriodFrom As Integer = 0
        Dim i As Integer
        Try
            ' 期マスタから各期の適用開始日を取得する
            sql = "" & vbCrLf
            sql = sql & " SELECT d_from" & vbCrLf
            sql = sql & "   FROM period" & vbCrLf
            sql = sql & "  ORDER BY d_from DESC" & vbCrLf
            sql = sql & ";" & vbCrLf    'chk
            Call clsMDB.Connect()
            ' 情報取得
            dt = clsMDB.ExecuteSql(sql)
            ' ログイン期の適用開始日を取得
            loginPeriodfrom = MDLoginInfo.PeriodFrom
            For i = 0 To dt.Rows.Count - 1
                PeriodFrom = dt.Rows(i).Item(0).ToString
                If loginPeriodfrom = PeriodFrom Then
                    HitPeriodFrom = i
                End If
            Next
            ' メニュー用ビューから取得
            getSql = getSql & "SELECT 表示順,カテゴリ名称,メニュー名称,コントロール名称"
            If HitPeriodFrom = 0 Then
                getSql = getSql & ",現在期参照,現在期登録,現在期印刷,現在期ファイル"
            ElseIf HitPeriodFrom = 1 Then
                getSql = getSql & ",前期参照,前期登録,前期印刷,前期ファイル"
            Else
                getSql = getSql & ",旧期参照,旧期登録,旧期印刷,旧期ファイル"
            End If
            getSql = getSql & " FROM menu_view"
            getSql = getSql & " WHERE (委員会ID='" & MDLoginInfo.CommitteeId & "') AND (役職ID='" _
            & MDLoginInfo.PostId & "') AND "
            If HitPeriodFrom = 0 Then
                getSql = getSql & "(現在期参照='1')"
            ElseIf HitPeriodFrom = 1 Then
                getSql = getSql & "(前期参照='1')"
            Else
                getSql = getSql & "(旧期参照='1')"
            End If
            getSql = getSql & " ORDER BY 表示順"   'chk
            dtRet = clsMDB.ExecuteSql(getSql)
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsMDB.Disconnect()
        End Try
        Return dtRet
    End Function

    '    '***************************************************************************************************
    '    '   ＩＤ　：funcMarch
    '    '   名称　：3月対応箇所除外
    '    '   概要　：3月対応する箇所の処理を行わないようにする。
    '    '   引数　：ByVal strGamenID As String = 画面ID
    '    '   戻り値：booRet As boolean = True(除外対象)/False(除外対象でない)
    '    '   作成日：2011/12/04(土)  y.nakano
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/12/04(土)  y.nakano  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>3月対応箇所除外</summary>
    '    ''' <param name="strGamenID">画面ID</param> 
    '    ''' <returns>処理結果</returns>
    '    ''' <remarks></remarks>
    '    Private Function funcMarch(ByVal strGamenID As String) As Boolean
    '        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
    '        Dim booRet As Boolean = False
    '        'Dim strExList() As String = {"UC040101", "UC040201", "UC040301", "UC040401", "UC040601", "UC050101", "UC050301", "UC050401", "UC080101", "UC080201", "FM090201", "FM090301", "FM090401"} ' 12月リリース
    '        'Dim strExList() As String = {"UC040101", "UC040201", "UC040601", "UC050101", "UC050301", "UC050401", "UC080101", "FM090201", "FM090301", "FM090401"}    ' 1月リリース
    '        Dim strExList() As String = {"UC040101", "UC040201", "UC040601", "UC050401", "UC080101", "FM090201", "FM090301", "FM090401"}    ' 2月リリース
    '        Dim flgEx As Boolean = False
    '        Try
    '            For Each strEx As String In strExList
    '                If strGamenID = strEx Then
    '                    flgEx = True
    '                End If
    '            Next
    '            booRet = flgEx
    '        Catch ex As Exception
    '            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
    '            ' 致命的エラーメッセージボックス表示
    '            Call CLMsg.ShowEtarnal(Err.Number, _
    '                                   Err.Description, _
    '                                   SCREEN_ID, SCREEN_NAME, _
    '                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    '        Return booRet
    '    End Function

    '    '***************************************************************************************************
    '    '   ＩＤ　：funcSetColor
    '    '   名称　：文字グレーアウト
    '    '   概要　：3月対応する箇所の文字色をグレーに変更する。
    '    '   戻り値：booRet As boolean = True(変更成功)/False(例外)
    '    '   作成日：2011/12/04(土) y.nakano
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/12/04(土) y.nakano  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>文字グレーアウト</summary>
    '    ''' <returns>処理結果</returns>
    '    ''' <remarks></remarks>
    '    Private Function funcSetColor() As Boolean
    '        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
    '        'Dim strExList() As String = {"組合大会通知", "争議行為", "時間内組合活動", "指名ストライキ", "発信文書", "賃金カット", "源泉徴収", "収支予想", "労金データ作成", "組合員口座情報", "財務管理マスタメンテナンス", "管理部登録", "発信文書マスタメンテナンス"} ' 12月リリース
    '        'Dim strExList() As String = {"組合大会通知", "争議行為", "発信文書", "賃金カット", "源泉徴収", "収支予想", "労金データ作成", "財務管理マスタメンテナンス", "管理部登録", "発信文書マスタメンテナンス"} ' 1月リリース
    '        Dim strExList() As String = {"組合大会通知", "争議行為", "発信文書", "収支予想", "労金データ作成", "財務管理マスタメンテナンス", "管理部登録", "発信文書マスタメンテナンス"} ' 2月リリース
    '        Dim strText As String = ""
    '        Dim flgEx As Boolean = False
    '        Dim booRet As Boolean = False
    '        Dim Color As Color = System.Drawing.ColorTranslator.FromOle(RGB(170, 170, 170))
    '        Try
    '            For Each row As DataGridViewRow In dgdMenu.Rows
    '                ' フラグ初期化
    '                flgEx = False
    '                ' 画面IDを取得
    '                If row.Cells.Item(2).Value Is Nothing Then
    '                    strText = ""
    '                Else
    '                    strText = row.Cells.Item(2).Value.ToString
    '                End If
    '                ' 除外対象リストの値と比較する
    '                For Each strEx As String In strExList
    '                    If strText = strEx Then
    '                        flgEx = True
    '                    End If
    '                Next
    '                ' 除外リストに含まれている画面なら文字色をグレーにする
    '                If flgEx Then
    '                    row.Cells.Item(2).Style.ForeColor = Color
    '                    row.Cells.Item(2).Style.SelectionForeColor = Color
    '                End If
    '            Next
    '            booRet = True
    '        Catch ex As Exception
    '            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
    '            ' 致命的エラーメッセージボックス表示
    '            Call CLMsg.ShowEtarnal(Err.Number, _
    '                                   Err.Description, _
    '                                   SCREEN_ID, SCREEN_NAME, _
    '                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '        Return booRet
    '        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    '    End Function
#End Region

End Class
#End Region
