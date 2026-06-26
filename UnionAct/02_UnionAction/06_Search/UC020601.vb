#Region "UC020601"
'===========================================================================================================
'   クラスＩＤ　　：UC020601
'   クラス名称　　：検索画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo

Public Class UC020601

#Region "定数"
    '定数マスタID
    Private Const CONSTANT_WORKFORM As String = "WORK_STATE"                            ' 勤務形態
    '使用ビュー名
    Private Const CONSTANT_VIEW_SEARCH As String = "search_view"
    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC020601                              ' UC020601
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC020601                          ' 検索画面
#End Region

#Region "イベント"
#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UC020601_Load
    '   名称　：フォームロード
    '   概要　：コンボボックスの作成を行う。
    '   作成日：2011/11/10(木)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/08(火)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>フォームロード</summary>
    ''' <remarks></remarks>
    Private Sub UC020601_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)      ' ログ出力（処理開始）
        Dim clsADB As New CLAccessMdb
        Try
            'フォーカスを社員番号に設定
            txtEmpNember.Focus()
            '名前（半角カナ）の入力文字を半角カナに設定
            txtEmpName.ImeMode = ImeMode.KatakanaHalf
            'コンボボックス作成処理
            If funcCreateCmb() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)        ' ログ出力（処理正常終了）
    End Sub
#End Region

#Region "検索ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック
    '   概要　：検索項目をチェックし、検索結果を表示する。
    '   作成日：2011/11/25(木)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(火)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If funcSearch() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "CAP経験最小値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtCapBottom_KeyPress
    '   名称　：CAP経験最小値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtCapBottom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCapBottom.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "CAP経験最大値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtCapTop_KeyPress
    '   名称　：CAP経験最大値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtCapTop_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCapTop.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "組合経験最小値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtMemberBottom_KeyPress
    '   名称　：組合経験最小値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtMemberBottom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMemberBottom.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "組合経験最大値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtMemberTop_KeyPress
    '   名称　：組合経験最大値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtMemberTop_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtMemberTop.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "誕生年最小値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtBirthYearBottom_KeyPress
    '   名称　：誕生年最小値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtBirthYearBottom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBirthYearBottom.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "誕生月最小値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtCapTop_KeyPress
    '   名称　：誕生月最小値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtBirthMonthBottom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBirthMonthBottom.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "誕生年最大値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtBirthYearTop_KeyPress
    '   名称　：誕生年最大値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtBirthYearTop_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBirthYearTop.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "誕生月最大値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtBirthMonthTop_KeyPress
    '   名称　：誕生月最大値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtBirthMonthTop_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtBirthMonthTop.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "退職年最小値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtRetireYeraBottom_KeyPress
    '   名称　：退職年最小値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtRetireYeraBottom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRetireYeraBottom.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "退職月最小値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtRetireMonthBottom_KeyPress
    '   名称　：退職月最小値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtRetireMonthBottom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRetireMonthBottom.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "退職年最大値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtRetireYeraTop_KeyPress
    '   名称　：退職年最大値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtRetireYeraTop_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRetireYeraTop.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "退職月最大値キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtRetireMonthTop_KeyPress
    '   名称　：退職月最大値キープレスイベント
    '   概要　：数値（正数）のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtRetireMonthTop_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtRetireMonthTop.KeyPress
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "郵便番号1キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtPostCode1_KeyPress
    '   名称　：郵便番号1キープレスイベント
    '   概要　：数値（正数）を3桁のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtPostCode1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPostCode1.KeyPress
        Me.txtPostCode1.MaxLength = 3
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "郵便番号2キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtPostCode2_KeyPress
    '   名称　：郵便番号2キープレスイベント
    '   概要　：数値（正数）を4桁のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtPostCode2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPostCode2.KeyPress
        Me.txtPostCode2.MaxLength = 4
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "電話番号1キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtTelNumber1_KeyPress
    '   名称　：電話番号1キープレスイベント
    '   概要　：数値（正数）を4桁のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtTelNumber1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTelNumber1.KeyPress
        Me.txtTelNumber1.MaxLength = 4
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "電話番号2キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtTelNumber2_KeyPress
    '   名称　：電話番号2キープレスイベント
    '   概要　：数値（正数）を4桁のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtTelNumber2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTelNumber2.KeyPress
        Me.txtTelNumber2.MaxLength = 4
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "電話番号3キープレスイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtTelNumber3_KeyPress
    '   名称　：電話番号3キープレスイベント
    '   概要　：数値（正数）を4桁のみ入力可能とする。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtTelNumber3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTelNumber3.KeyPress
        Me.txtTelNumber3.MaxLength = 4
        If (e.KeyChar < "0"c Or e.KeyChar > "9"c) And (e.KeyChar <> vbBack) Then
            e.Handled = True
        End If
    End Sub
#End Region

#Region "電話番号1テキストチェンジ"
    '***************************************************************************************************
    '   ＩＤ　：txtTelNumber1_TextChanged
    '   名称　：電話番号1テキストチェンジ
    '   概要　：電話番号1の値が変更された場合に、電話番号2・3のプロパティ、値を変更する。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Protected Sub txtTelNumber1_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTelNumber1.TextChanged
        If ChkNull(txtTelNumber1.Text) = True Then
            '電話番号2のテキストを削除し、入力できないように変更する。
            Me.txtTelNumber2.ReadOnly = True
            Me.txtTelNumber2.Text = ""
            Me.txtTelNumber2.BackColor = System.Drawing.SystemColors.Control
            Me.txtTelNumber2.BorderStyle = Windows.Forms.BorderStyle.FixedSingle

            '電話番号3のテキストを削除し、入力できないように変更する。
            Me.txtTelNumber3.ReadOnly = True
            Me.txtTelNumber3.Text = ""
            Me.txtTelNumber3.BackColor = System.Drawing.SystemColors.Control
            Me.txtTelNumber3.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Else
            '電話番号2を入力可能とする。
            Me.txtTelNumber2.ReadOnly = False
            Me.txtTelNumber2.BackColor = Color.White
            Me.txtTelNumber2.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
        End If
    End Sub
#End Region

#Region "電話番号2テキストチェンジ"
    '***************************************************************************************************
    '   ＩＤ　：txtTelNumber2_TextChanged
    '   名称　：電話番号2テキストチェンジ
    '   概要　：電話番号2の値が変更された場合に、電話番号3のプロパティ、値を変更する。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Protected Sub txtTelNumber2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTelNumber2.TextChanged
        If ChkNull(txtTelNumber2.Text) = True Then
            '電話番号3のテキストを削除し、入力できないように変更する。
            Me.txtTelNumber3.ReadOnly = True
            Me.txtTelNumber3.Text = ""
            Me.txtTelNumber3.BackColor = System.Drawing.SystemColors.Control
            Me.txtTelNumber3.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Else
            '電話番号3を入力可能とする。
            Me.txtTelNumber3.ReadOnly = False
            Me.txtTelNumber3.BackColor = Color.White
            Me.txtTelNumber3.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
        End If
    End Sub
#End Region

#Region "郵便番号1テキストチェンジ"
    '***************************************************************************************************
    '   ＩＤ　：txtPostCode2_TextChanged
    '   名称　：郵便番号1テキストチェンジ
    '   概要　：郵便番号1の値が変更された場合に、郵便番号2のプロパティ、値を変更する。
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    Protected Sub txtPostCode2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPostCode1.TextChanged
        If ChkNull(txtPostCode1.Text) = True Then
            '郵便番号2のテキストを削除し、入力できないように変更する。
            Me.txtPostCode2.ReadOnly = True
            Me.txtPostCode2.Text = ""
            Me.txtPostCode2.BackColor = System.Drawing.SystemColors.Control
            Me.txtPostCode2.BorderStyle = Windows.Forms.BorderStyle.FixedSingle
        Else
            '郵便番号2を入力可能とする。
            Me.txtPostCode2.ReadOnly = False
            Me.txtPostCode2.BackColor = Color.White
            Me.txtPostCode2.BorderStyle = Windows.Forms.BorderStyle.Fixed3D
        End If
    End Sub
#End Region

#Region "社員番号キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtEmpNember_KeyDown
    '   名称　：社員番号キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtEmpNember_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmpNember.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "名前(ｶﾅ)キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtEmpName_KeyDown
    '   名称　：名前(ｶﾅ)キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtEmpName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEmpName.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "正組合員キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtEmpName_KeyDown
    '   名称　：正組合員キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：2017/10/17(火)　y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkMemberRegular_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkMemberRegular.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "シニア組合員キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtEmpName_KeyDown
    '   名称　：シニア組合員キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：2017/10/17(火)　y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkMemberSenior_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkMemberSenior.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "永年組合員キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkMemberLongtime_KeyDown
    '   名称　：永年組合員キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：2017/10/17(火)　y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkMemberLongtime_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkMemberLongtime.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "準組合員キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkMemberSubRegular_KeyDown
    '   名称　：準組合員キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：2017/10/17(火)　y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkMemberSubRegular_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkMemberSubRegular.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "非組合員キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkMemberSubRegular_KeyDown
    '   名称　：非組合員キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：2017/10/17(火)　y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkMemberNot_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkMemberNot.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "脱退組合員キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkMemberDropOut_KeyDown
    '   名称　：脱退組合員キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：2017/10/17(火)　y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkMemberDropOut_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkMemberDropOut.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "その他キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkMemberOther_KeyDown
    '   名称　：その他キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：2017/10/17(火)　y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkMemberOther_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkMemberOther.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "ステータス加入キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkStatusAdd_KeyDown
    '   名称　：ステータス加入キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkStatusAdd_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkStatusAdd.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "ステータス地位喪失キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkStatusAdd_KeyDown
    '   名称　：ステータス地位喪失キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkStatusLost_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkStatusLost.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "ステータス脱退キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkStatusDropOut_KeyDown
    '   名称　：ステータス脱退キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkStatusDropOut_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkStatusDropOut.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "組合支部東京キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkBranchTokyo_KeyDown
    '   名称　：組合支部東京キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkBranchTokyo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkBranchTokyo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "組合支部大阪キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkBranchOosaka_KeyDown
    '   名称　：組合支部大阪キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkBranchOosaka_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkBlanchOosaka.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "組合支部その他キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkBranchOther_KeyDown
    '   名称　：組合支部その他キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkBranchOther_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkBlanchOther.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "乗務資格　機長キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkLicencePilot_KeyDown
    '   名称　：乗務資格機長キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkLicencePilot_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkLicencePilot.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "乗務資格　副操縦士キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkLicenceCopilot_KeyDown
    '   名称　：乗務資格副操縦士キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkLicenceCopilot_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkLicenceCopilot.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "乗務資格　航空機関士キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkLicenceFlightEngineer_KeyDown
    '   名称　：キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/16(金)
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金) 新規作成
    '***************************************************************************************************
    Private Sub chkLicenceFlightEngineer_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkLicenceFlightEngineer.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "乗務資格　教官機長キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkLicenceTeacherPilot_KeyDown
    '   名称　：キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkLicenceTeacherPilot_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkLicenceTeacherPilot.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "乗務資格　その他キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkLicenceOther_KeyDown
    '   名称　：乗務資格　その他キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkLicenceOther_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkLicenceOther.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　B787キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelB787_KeyDown
    '   名称　：機種　B787キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkModelB787_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelB787.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　B777キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelB777_KeyDown
    '   名称　：機種　B777キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkModelB777_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelB777.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　B767キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelB767_KeyDown
    '   名称　：機種　B767キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkModelB767_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelB767.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　A320キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelA320_KeyDown
    '   名称　：機種　A320キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkModelA320_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelA320.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　B744キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelB744_KeyDown
    '   名称　：キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkModelB744_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelB744.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　訓練機キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelPractice_KeyDown
    '   名称　：機種　訓練機キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkModelPractice_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelPractice.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　B747キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelB747_KeyDown
    '   名称　：機種　B747キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkModelB747_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelB747.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　B737キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelB737_KeyDown
    '   名称　：機種　B737キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2018/04/06(金)  y.Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2018/04/06(金)  y.Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub chkModelB737_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelB737.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　A380キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelA380_KeyDown
    '   名称　：機種　A380キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2019/04/04(木)  y.Fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2019/04/04(木)  y.Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub chkModelBA380_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelA380.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "機種　その他キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkModelOther_KeyDown
    '   名称　：機種　その他キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkModelOther_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkModelOther.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "会社所属　東京キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkCompanyTokyo_KeyDown
    '   名称　：会社所属　東京キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkCompanyTokyo_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkCompanyTokyo.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "会社所属　大阪キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkCompanyOosaka_KeyDown
    '   名称　：会社所属　大阪キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkCompanyOosaka_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkCompanyOosaka.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "会社所属　その他キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkCompanyOther_KeyDown
    '   名称　：会社所属　その他キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkCompanyOther_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkCompanyOther.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "職場　乗員室キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkOfficeCrew_KeyDown
    '   名称　：職場　乗員室キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkOfficeCrew_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkOfficeCrew.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "職場　訓練センターキーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkOfficeTraining_KeyDown
    '   名称　：職場　訓練センターキーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkOfficeTraining_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkOfficeTraining.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "職場　本部キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkOfficeHead_KeyDown
    '   名称　：職場　本部キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkOfficeHead_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkOfficeHead.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "職場　その他キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkOfficeOther_KeyDown
    '   名称　：職場　その他キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkOfficeOther_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkOfficeOther.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "所属会社　ANAキーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkAttachCompanyANA_KeyDown
    '   名称　：所属会社　ANAキーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkAttachCompanyANA_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkAttachCompanyANA.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "所属会社　その他キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkAttachCompanyOther_KeyDown
    '   名称　：所属会社　その他キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkAttachCompanyOther_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkAttachCompanyOther.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "性別　男キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkAttachCompanyANA_KeyDown
    '   名称　：性別　男キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkSexMen_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkSexMen.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "性別　女キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：chkAttachCompanyOther_KeyDown
    '   名称　：性別　女キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub chkSexWomen_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles chkSexWomen.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "CAP経験　最小キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtCapBottom_KeyDown
    '   名称　：CAP経験最小キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtCapBottom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCapBottom.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "CAP経験　最大キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtCapTop_KeyDown
    '   名称　：CAP経験　最大キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtCapTop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtCapTop.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "組合経験　最小キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtMemberBottom_KeyDown
    '   名称　：組合経験　最小キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtMemberBottom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMemberBottom.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "組合経験　最大キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtMemberTop_KeyDown
    '   名称　：組合経験　最大キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtMemberTop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtMemberTop.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "誕生年月　誕生年最小キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtBirthYearBottom_KeyDown
    '   名称　：誕生年月　誕生年最小キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtBirthYearBottom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBirthYearBottom.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "誕生年月　誕生月最小キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtBirthMonthBottom_KeyDown
    '   名称　：誕生年月　誕生月最小キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtBirthMonthBottom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBirthMonthBottom.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "誕生年月　誕生年最大キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtBirthYearTop_KeyDown
    '   名称　：誕生年月　誕生年最大キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtBirthYearTop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBirthYearTop.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "誕生年月　誕生月最大キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtBirthMonthTop_KeyDown
    '   名称　：誕生年月　誕生月最大キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtBirthMonthTop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtBirthMonthTop.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "入社年月　入社年最小キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtEntryYearBottom_KeyDown
    '   名称　：入社年月　入社年最小キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtEntryYearBottom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEntryYearBottom.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "入社年月　入社月最小キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtEntryMonthBottom_KeyDown
    '   名称　：入社年月　入社月最小キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtEntryMonthBottom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEntryMonthBottom.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "入社年月　入社年最大キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtEntryYearTop_KeyDown
    '   名称　：入社年月　入社年最大キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtEntryYearTop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEntryYearTop.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "入社年月　入社月最大キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtEntryMonthTop_KeyDown
    '   名称　：入社年月　入社月最大キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtEntryMonthTop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtEntryMonthTop.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "退職年月　退職年小キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtRetireYeraBottom_KeyDown
    '   名称　：退職年月　退職年小キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金) 新規作成
    '***************************************************************************************************
    Private Sub txtRetireYeraBottom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRetireYeraBottom.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "退職年月　退職月最小キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtRetireMonthBottom_KeyDown
    '   名称　：キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtRetireMonthBottom_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRetireMonthBottom.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "退職年月　退職年最大キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtRetireYeraTop_KeyDown
    '   名称　：退職年月　退職年最大キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtRetireYeraTop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRetireYeraTop.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "退職年月　退職月最大キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtRetireMonthTop_KeyDown
    '   名称　：退職年月　退職月最大キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtRetireMonthTop_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtRetireMonthTop.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "勤務形態キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：cboWorkForm_KeyDown
    '   名称　：勤務形態キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub cboWorkForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboWorkForm.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "電話番号1キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtTelNumber1_KeyDown
    '   名称　：電話番号1キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtTelNumber1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTelNumber1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "電話番号2キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtTelNumber2_KeyDown
    '   名称　：電話番号2キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtTelNumber2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTelNumber2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "電話番号3キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtTelNumber3_KeyDown
    '   名称　：電話番号3キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtTelNumber3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTelNumber3.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "郵便番号1キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtPostCode1_KeyDown
    '   名称　：郵便番号1キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtPostCode1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPostCode1.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "電話番号2キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：txtPostCode2_KeyDown
    '   名称　：電話番号2キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub txtPostCode2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPostCode2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "所属委員会キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：cboCommittee_KeyDown
    '   名称　：所属委員会キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub cboCommittee_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cboCommittee.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "社員番号順キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：optEmpNumber_KeyDown
    '   名称　：社員番号順キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub optEmpNumber_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles optEmpNumber.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region

#Region "名前順キーダウン"
    '***************************************************************************************************
    '   ＩＤ　：optEmpName_KeyDown
    '   名称　：名前順キーダウン
    '   概要　：対象のコントロールでエンターが押されたら、検索結果を表示する。
    '   作成日：2011/12/16(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '***************************************************************************************************
    Private Sub optEmpName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles optEmpName.KeyDown
        If e.KeyCode = Keys.Enter Then
            If funcSearch() = False Then
                Exit Sub
            End If
        End If
    End Sub
#End Region
#End Region

#Region "関数"
#Region "検索処理"
    '***************************************************************************************************
    '   ＩＤ　：funcSearch
    '   名称　：検索処理
    '   概要　：検索項目をチェックし、検索結果を表示する。
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/16(金)
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/26(金)  y.nakano  新規作成
    '           2012/02/01(水)  m.suzuki  メールアドレス1・2追加
    '***************************************************************************************************
    Function funcSearch() As Boolean
        Dim cForm1 As New FM020602()
        Dim dgv As DataGridView
        Dim strSql As String = ""           '検索結果取得SQL
        Dim strSqlTKsh As String = ""       '会社所属名称取得SQL
        Dim dt As DataTable                 '検索結果
        Dim dtTKsh As DataTable = Nothing   '会社所属取得結果
        Dim dtSKind As DataTable = Nothing  '組合員種別取得結果
        Dim clsdb As New CLAccessMdb        'アクセスDBクラス
        Dim booRet As Boolean = False       '処理結果
        Try
            Cursor.Current = Cursors.WaitCursor
            'エラー項目の背景色をもとに戻す。
            Me.txtEmpName.BackColor = System.Drawing.SystemColors.Window
            Me.txtEmpNember.BackColor = System.Drawing.SystemColors.Window
            '入力チェック
            If funcInputChk() = False Then
                Exit Function
            End If
            'DB接続
            Call clsdb.Connect()
            '情報取得SQL
            If funcCreateSql(clsdb, strSql) = False Then
                Return booRet
            End If
            'SQL実行
            dt = clsdb.ExecuteSql(strSql, 240)
            If dt.Rows.Count = 0 Then
                CLMsg.Show("DI0001")
                Return booRet
            End If
            '検索結果を表示するグリッドビューを定義
            dgv = cForm1.dgdSearchResult
            '検索結果の件数を表示
            cForm1.grbSearchResult.Text = "検索結果一覧(" & CStr(dt.Rows.Count) & "件)"
            '取得した検索結果をグリッドビューに表示
            dgv.DataSource = dt
            '名前カナ・都道府県・市区町村・番地等・建物等・海外アドレス1～5・住所区分は、非表示にする。
            dgv.Columns("名前カナ").Visible = False
            dgv.Columns("都道府県").Visible = False
            dgv.Columns("市区町村").Visible = False
            dgv.Columns("番地等").Visible = False
            dgv.Columns("建物等").Visible = False
            dgv.Columns("海外アドレス1").Visible = False
            dgv.Columns("海外アドレス2").Visible = False
            dgv.Columns("海外アドレス3").Visible = False
            dgv.Columns("海外アドレス4").Visible = False
            dgv.Columns("海外アドレス5").Visible = False
            dgv.Columns("住所区分").Visible = False
            ' Insert 20120201(水) m.suzuki  メールアドレス1・2追加 Start
            dgv.Columns("メールアドレス１").Visible = True
            dgv.Columns("メールアドレス２").Visible = True
            ' Insert 20120201(水) m.suzuki  メールアドレス1・2追加 End
            dgv.Columns("組合員種別略称").Visible = False
            dgv.Columns("資格略称").Visible = False
            dgv.Columns("機種略称").Visible = False
            'テキスト配置位置変更
            dgv.Columns("社員番号").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            dgv.Columns("ステータス").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns("組合支部").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns("資格").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns("機種").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns("会社所属").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            dgv.Columns("所属会社").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            'datagridの1列目はチェックボックス
            Dim column As New DataGridViewCheckBoxColumn
            column.Width = 30
            dgv.Columns.Insert(0, column)
            'チェックボックスの列のみスクロールさせないように設定
            dgv.Columns(0).Frozen = True
            '編集制御
            For i = 0 To dgv.Columns.Count - 1
                'チェックボックスの列のみ編集可能とする。
                If i = 0 Then
                    dgv.Columns(i).ReadOnly = False
                Else
                    dgv.Columns(i).ReadOnly = True
                End If
            Next
            '新しい行の追加をできないようにする。
            dgv.AllowUserToAddRows = False
            ' Form1 をモーダルで表示する
            cForm1.ShowDialog()
            ' 不要になった時点で破棄する
            cForm1.Dispose()
            booRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Return booRet
        Finally
            'DB切断
            Call clsdb.Disconnect()
        End Try
        Return booRet
    End Function

#Region "入力チェック"
    '***************************************************************************************************
    '   ＩＤ　：funcInputChk
    '   名称　：入力チェック
    '   概要　：名前(半角ｶﾅ)に半角ｶﾅ以外の文字が設定されていないか確認する。
    '   戻り値：booRet As boolean       = 確認結果
    '   作成日：2011/11/25(金)
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) 新規作成
    '***************************************************************************************************
    ''' <summary>入力チェック</summary>
    ''' <returns>確認結果</returns>
    ''' <remarks></remarks>
    Function funcInputChk() As Boolean
        Dim strKana As String = ""
        Dim booRet As Boolean = False
        Try
            strKana = Me.txtEmpName.Text
            If ChkNull(strKana) = False Then
                '検索項目-名前（半角カナ）に入力されている値が半角カタカナか確認する
                If ChkHankakuKana(strKana) = False Then
                    Call CLMsg.Show("GE0019", "名前（半角カナ）")
                    Call MDCommon.SetErr(Me.txtEmpName)
                    Return booRet
                End If
            End If
            '結果を設定
            booRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '結果を返却
        Return booRet
    End Function
#End Region

#Region "SQL主文作成"
    '***************************************************************************************************
    '   ＩＤ　：funcCreateSql
    '   名称　：SQL主文作成
    '   概要　：SQLの主文を作成する。
    '   引数　：ByVal db As CLAccessMdb = アクセスDBクラス
    '   戻り値：ExecuteSql As String = SQL文
    '   作成日：2011/11/25(金)
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '           2012/02/01(水)  m.suzuki  メールアドレス１・２追加
    '***************************************************************************************************
    ''' <summary>SQL主文作成</summary>
    ''' <param name="db">アクセスDBクラス</param>
    ''' <returns>SQL文</returns>
    ''' <remarks></remarks>
    Private Function funcCreateSql(ByVal db As CLAccessMdb, ByRef strRet As String) As Boolean
        Dim strSql As String = ""
        Dim strRegMemCode As String = ""
        Dim strChkSql As String = ""
        Dim strEmpNo As String = ""
        Dim strEmpName As String = ""
        Dim strWorkForm As String = ""
        Dim strTel1 As String = ""
        Dim strTel2 As String = ""
        Dim strTel3 As String = ""
        Dim strTel As String = ""
        Dim strPostCode As String = ""
        Dim strPostCode1 As String = ""
        Dim strPostCode2 As String = ""
        Dim strCommittee As String = ""
        Dim strOrder As String = ""
        Dim booRet As Boolean = False
        Dim strSelectSql As String = ""
        Dim strFromSql As String = ""
        Dim strOrderSql As String = ""
        Dim strGroupSql As String = ""
        Dim strEmpNoSql As String = ""
        Try
            '-------------------------------------------------------------------------------------------
            '   SELECT句
            '-------------------------------------------------------------------------------------------
            strCommittee = Me.cboCommittee.Text
            If ChkNull(strCommittee) = True Then
                strSelectSql = strSelectSql & "SELECT " & CONSTANT_VIEW_SEARCH & ".社員番号 AS 社員番号,"                           '社員番号(グループ化)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".名前 AS 名前,"                                   '名前(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".名前カナ AS 名前カナ,"                                       '名前カナ(グループ化)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".組合員種別 AS 組合員種別,"                       '組合員種別(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".ステータス AS ステータス,"                       'ステータス(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".組合支部 AS 組合支部,"                           '組合員支部(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".資格 AS 資格,"                                   '資格(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".機種 AS 機種,"                                   '機種(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".会社所属 AS 会社所属,"                           '会社所属(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".職場 AS 職場,"                                   '職場(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".所属会社 AS 所属会社,"                           '所属会社(先頭)
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".機長年月日,'yyyy/MM/dd') AS 機長年月日,"  '機長年月日(先頭)
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".加入年月日,'yyyy/MM/dd') AS 加入年月日,"  '加入年月日(先頭)
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".生年月日,'yyyy/MM/dd') AS 生年月日,"      '生年月日(先頭)
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".入社年月日,'yyyy/MM/dd') AS 入社年月日,"  '入社年月日(先頭)
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".退職年月日,'yyyy/MM/dd') AS 退職年月日,"  '退職年月日(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".勤務状態 AS 勤務状態,"                           '勤務状態(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".郵便番号 AS 郵便番号,"                           '郵便番号(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".住所 AS 住所,"                                   '住所(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".電話番号1 AS 電話番号１,"                         '電話番号1(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".電話番号2 AS 電話番号２,"                         '電話番号2(先頭)
                'ここから先の項目は検索結果に表示しない。（タックシール出力に使用するため取得）
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".都道府県 AS 都道府県,"                           '都道府県(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".市区町村 AS 市区町村,"                           '市区町村(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".番地等 AS 番地等,"                               '番地等(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".建物等 AS 建物等,"                               '建物等(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス1 AS 海外アドレス1,"                 '海外アドレス1(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス2 AS 海外アドレス2,"                 '海外アドレス2(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス3 AS 海外アドレス3,"                 '海外アドレス3(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス4 AS 海外アドレス4,"                 '海外アドレス4(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス5 AS 海外アドレス5,"                 '海外アドレス5(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".住所区分 AS 住所区分,"                           '住所区分(先頭)
                ' 2012/02/01(水) m.suzuki メールアドレス1・2追加 Start
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".メールアドレス1 AS メールアドレス１,"             'メールアドレス1(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".メールアドレス2 AS メールアドレス２,"             'メールアドレス2(先頭)
                ' 2012/02/01(水) m.suzuki メールアドレス1・2追加 End
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".組合員種別略称 AS 組合員種別略称,"               '組合員種別略称(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".資格略称 AS 資格略称,"                           '資格略称(先頭)
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".機種略称 AS 機種略称,"                            '機種略称(先頭)
                strSelectSql = strSelectSql & "ROW_NUMBER() OVER(PARTITION BY " & CONSTANT_VIEW_SEARCH & ".社員番号 ORDER BY " & CONSTANT_VIEW_SEARCH & ".社員番号) AS sqno"                                        '機種略称
            Else
                strSelectSql = strSelectSql & "SELECT " & CONSTANT_VIEW_SEARCH & ".社員番号 AS 社員番号,"                           '社員番号
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".名前 AS 名前,"                                               '名前
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".名前カナ AS 名前カナ,"                                       '名前カナ
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".組合員種別 AS 組合員種別,"                                   '組合員種別
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".ステータス AS ステータス,"                                   'ステータス
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".組合支部 AS 組合支部,"                                       '組合員支部
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".資格 AS 資格,"                                               '資格
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".機種 AS 機種,"                                               '機種
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".会社所属 AS 会社所属,"                                       '会社所属
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".職場 AS 職場,"                                               '職場
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".所属会社 AS 所属会社,"                                       '所属会社
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".機長年月日,'yyyy/MM/dd') AS 機長年月日,"         '機長年月日
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".加入年月日,'yyyy/MM/dd') AS 加入年月日,"         '加入年月日
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".生年月日,'yyyy/MM/dd') AS 生年月日,"             '生年月日
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".入社年月日,'yyyy/MM/dd') AS 入社年月日,"         '入社年月日
                strSelectSql = strSelectSql & "FORMAT(" & CONSTANT_VIEW_SEARCH & ".退職年月日,'yyyy/MM/dd') AS 退職年月日,"         '退職年月日
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".勤務状態 AS 勤務状態,"                                       '勤務状態
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".郵便番号 AS 郵便番号,"                                       '郵便番号
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".住所 AS 住所,"                                               '住所
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".電話番号1 AS 電話番号１,"                                    '電話番号1
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".電話番号2 AS 電話番号２,"                                    '電話番号2
                'ここから先の項目は検索結果に表示しない。（タックシール出力に使用するため取得）
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".都道府県 AS 都道府県,"                                       '都道府県
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".市区町村 AS 市区町村,"                                       '市区町村
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".番地等 AS 番地等,"                                           '番地等
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".建物等 AS 建物等,"                                           '建物等
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス1 AS 海外アドレス1,"                             '海外アドレス1
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス2 AS 海外アドレス2,"                             '海外アドレス2
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス3 AS 海外アドレス3,"                             '海外アドレス3
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス4 AS 海外アドレス4,"                             '海外アドレス4
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".海外アドレス5 AS 海外アドレス5,"                             '海外アドレス5
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".住所区分 AS 住所区分,"                                       '住所区分
                ' 2012/02/01(水) m.suzuki メールアドレス1・2追加 Start
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".メールアドレス1 AS メールアドレス１,"                         'メールアドレス1
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".メールアドレス2 AS メールアドレス２,"                         'メールアドレス2
                ' 2012/02/01(水) m.suzuki メールアドレス1・2追加 End
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".組合員種別略称 AS 組合員種別略称,"                           '組合員種別略称
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".資格略称 AS 資格略称,"                                       '資格略称
                strSelectSql = strSelectSql & CONSTANT_VIEW_SEARCH & ".機種略称 AS 機種略称,"                                        '機種略称
                strSelectSql = strSelectSql & "ROW_NUMBER() OVER(PARTITION BY " & CONSTANT_VIEW_SEARCH & ".社員番号 ORDER BY " & CONSTANT_VIEW_SEARCH & ".社員番号) AS sqno"                                        '機種略称
            End If
            '-------------------------------------------------------------------------------------------
            '   FROM句
            '-------------------------------------------------------------------------------------------
            strFromSql = strFromSql & "FROM " & CONSTANT_VIEW_SEARCH
            '-------------------------------------------------------------------------------------------
            '   WHERE句
            '-------------------------------------------------------------------------------------------
            '社員番号のチェック
            If funcEmpNoSql(strEmpNoSql) = False Then
                Return booRet
            End If
            If ChkNull(strEmpNoSql) = False Then
                strSql = strSql & " AND (" & strEmpNoSql & ")"
            End If
            '名前（半角カナ）のチェック
            strEmpName = Me.txtEmpName.Text()
            If ChkNull(strEmpName) = False Then
                strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & ".名前カナ LIKE '%" & strEmpName & "%')"
            End If
            '組合員種別チェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("組合員種別", db, Me.chkMemberRegular, Me.chkMemberSenior,
                                     Me.chkMemberLongtime, Me.chkMemberSubRegular,
                                     Me.chkMemberNot, Me.chkMemberDropOut, Me.chkMemberOther)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            'ステータスチェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("ステータス", db, Me.chkStatusAdd, Me.chkStatusDropOut, Me.chkStatusLost)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            '組合支部チェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("組合支部", db, Me.chkBranchTokyo, Me.chkBlanchOosaka, Me.chkBlanchOther)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            '乗務資格チェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("資格", db, Me.chkLicencePilot, Me.chkLicenceCopilot, Me.chkLicenceFlightEngineer,
                                     Me.chkLicenceTeacherPilot, chkLicenceOther)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            '機種チェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("機種", db, Me.chkModelB787, Me.chkModelB777, Me.chkModelB767,
                                     Me.chkModelA320, Me.chkModelB744, Me.chkModelPractice,
                                     Me.chkModelB747, Me.chkModelB737, Me.chkModelA380, Me.chkModelOther)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            '会社所属チェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("会社所属", db, Me.chkCompanyTokyo, Me.chkCompanyOosaka, Me.chkCompanyOther)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            '職場チェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("職場", db, Me.chkOfficeCrew, Me.chkOfficeTraining, Me.chkOfficeHead, Me.chkOfficeOther)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            '所属会社チェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("所属会社", db, Me.chkAttachCompanyANA, Me.chkAttachCompanyOther)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            '性別チェックボックス群の確認
            '前のチェックボックス群の確認結果をクリアする。
            strChkSql = ""
            strChkSql = funcCheckBox("性別", db, Me.chkSexMen, Me.chkSexWomen)
            If ChkNull(strChkSql) = False Then
                strSql = strSql & " AND (" & strChkSql & ")"
            End If
            'CAP経験の確認
            If funcChkDate(strSql, Me.txtCapBottom.Text, Me.txtCapTop.Text, "CAP経験") = False Then
                Return strSql
            End If
            '組合経験の確認
            If funcChkDate(strSql, Me.txtMemberBottom.Text, Me.txtMemberTop.Text, "組合経験") = False Then
                Return strSql
            End If
            '誕生年月の確認
            If funcChkDate(strSql, Me.txtBirthYearBottom.Text, Me.txtBirthYearTop.Text, "誕生年月",
                           Me.txtBirthMonthBottom.Text, Me.txtBirthMonthTop.Text) = False Then
                Return strSql
            End If
            '入社年月の確認
            If funcChkDate(strSql, Me.txtEntryYearBottom.Text, Me.txtEntryYearTop.Text, "入社年月",
                           Me.txtEntryMonthBottom.Text, Me.txtEntryMonthTop.Text) = False Then
                Return strSql
            End If
            '退職年月の確認
            If funcChkDate(strSql, Me.txtRetireYeraBottom.Text, Me.txtRetireYeraTop.Text, "退職年月",
                           Me.txtRetireMonthBottom.Text, Me.txtRetireMonthTop.Text) = False Then
                Return strSql
            End If
            '勤務形態の確認
            strWorkForm = cboWorkForm.Text
            If ChkNull(strWorkForm) = False Then
                strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & ".勤務状態='" & strWorkForm & "')"
            End If
            '電話番号の確認
            strTel1 = Me.txtTelNumber1.Text
            strTel2 = Me.txtTelNumber2.Text
            strTel3 = Me.txtTelNumber3.Text
            If (ChkNull(strTel1) = False) Then
                If (ChkNull(strTel2) = False) And (ChkNull(strTel3) = False) Then
                    strTel = strTel1 & "-" & strTel2 & "-" & strTel3
                ElseIf (ChkNull(strTel2) = False) And (ChkNull(strTel3) = True) Then
                    strTel = strTel1 & "-" & strTel2
                ElseIf (ChkNull(strTel2) = True) And (ChkNull(strTel3) = True) Then
                    strTel = strTel1
                End If
                strSql = strSql & " AND ((" & CONSTANT_VIEW_SEARCH & ".電話番号1 LIKE '" & strTel & "%') or"
                strSql = strSql & "(" & CONSTANT_VIEW_SEARCH & ".電話番号2 LIKE '" & strTel & "%') or"
                strSql = strSql & "(" & CONSTANT_VIEW_SEARCH & ".電話番号3 LIKE '" & strTel & "%'))"
            End If
            '郵便番号の確認
            strPostCode1 = Me.txtPostCode1.Text
            strPostCode2 = Me.txtPostCode2.Text
            If (ChkNull(strPostCode1) = False) Then
                If (ChkNull(strPostCode2) = False) Then
                    strPostCode = strPostCode1 & "-" & strPostCode2
                ElseIf (ChkNull(strPostCode2) = True) Then
                    strPostCode = strPostCode1
                End If
                strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & ".郵便番号 LIKE '" & strPostCode & "%')"
            End If
            '所属委員会
            If ChkNull(strCommittee) = False Then
                strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & ".所属委員会='" & strCommittee & "')"
            End If
            ''-------------------------------------------------------------------------------------------
            ''   GROUP句
            ''-------------------------------------------------------------------------------------------
            ''検索条件に委員会項目が選択されていない場合に、所属委員会が複数あるユーザーは検索結果に情報が複数表示される。
            ''そのため、グループ化して表示する情報は１つとする。
            'strGroupSql = "GROUP BY " & CONSTANT_VIEW_SEARCH & ".社員番号"
            '-------------------------------------------------------------------------------------------
            '   ORDER句
            '-------------------------------------------------------------------------------------------
            If Me.optEmpName.Checked = True Then
                strOrder = "名前カナ"
            End If
            If Me.optEmpNumber.Checked = True Then
                strOrder = "CLng(社員番号)"
            End If
            strOrderSql = "ORDER BY " & strOrder    'chk
            'WHERE句の先頭にある" AND"を削除する。
            If ChkNull(strSql) = True Then
                strSql = "SELECT * FROM (" & strSelectSql & " " & strFromSql & " ) AS SLIST WHERE sqno=1 " & strOrderSql
            Else
                strSql = strSql.Remove(1, 4)
                If ChkNull(strCommittee) = True Then
                    strSql = "SELECT * FROM (" & strSelectSql & " " & strFromSql & " WHERE" & strSql & " ) AS SLIST WHERE sqno=1 " & strOrderSql
                Else
                    strSql = "SELECT * FROM (" & strSelectSql & " " & strFromSql & " WHERE" & strSql & " ) AS SLIST WHERE sqno=1 " & strOrderSql
                End If
            End If
            '関数の結果として作成した値を設定する。
            strRet = strSql
            booRet = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020601, SCREEN_NAME_UC020601, "funcCreateSql")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        '結果返却
        Return booRet
    End Function
#End Region
#End Region

#Region "社員番号チェック"
    '***************************************************************************************************
    '   ＩＤ　：funcEmpNoSql
    '   名称　：社員番号チェック
    '   概要　：社員番号の検索SQLを作成する。
    '   引数　：なし
    '   戻り値：strRet As string = SQL文
    '   作成日：2011/12/03(土)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/03(土)  新規作成
    '***************************************************************************************************
    Function funcEmpNoSql(ByRef strSql As String) As Boolean
        Dim strEmpNo As String = ""
        Dim strEmpNoList As String() = Nothing
        Dim booRet As Boolean = False
        Try
            '社員番号が設定されている場合
            strEmpNo = Me.txtEmpNember.Text
            If ChkNull(strEmpNo) = False Then
                '文字列に"/"が設定されているか確認する
                If strEmpNo.IndexOf("/"c) <> -1 Then
                    '文字列を"/"区切りで文字列配列に変換する
                    strEmpNoList = Me.txtEmpNember.Text.Split("/"c)
                    If strEmpNoList.Count > 24 Then
                        CLMsg.Show("GE0103")
                        MDCommon.SetErr(Me.txtEmpNember)
                        Return booRet
                    End If
                End If
                '文字列配列の値を確認
                If Not (strEmpNoList Is Nothing) Then
                    '配列のすべての社員番号をOR条件接続する
                    For Each Array As String In strEmpNoList
                        If ChkNull(strSql) Then
                            strSql = strSql & "(" & CONSTANT_VIEW_SEARCH & ".社員番号 LIKE '" & Array & "%')"
                        Else
                            strSql = strSql & " OR (" & CONSTANT_VIEW_SEARCH & ".社員番号 LIKE '" & Array & "%')"
                        End If
                    Next
                Else
                    strSql = strSql & CONSTANT_VIEW_SEARCH & ".社員番号 LIKE '" & strEmpNo & "%'"
                End If
            End If
            booRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return booRet
    End Function
#End Region

#Region "各チェックボックス確認"
    '***************************************************************************************************
    '   ＩＤ　：funcCheckBox
    '   名称　：チェックボックス群確認
    '   概要　：検索項目のチェックボックス群の確認を行い、検索条件を追加したSQL文を返却する。
    '   引数　：ByVal          strColumnName    As String       = 検索対象カラム名
    '   　　　：ByVal          db               As CLAccessMdb  = アクセスDBクラス
    '   　　　：ByVal          chk1             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk2             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk3             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk4             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk5             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk6             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk7             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk8             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk9             As CheckBox     = チェックボックス
    '   　　　：Optional ByVal chk10            As CheckBox     = チェックボックス
    '   戻り値：ExecuteSql As String = SQL文
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：2019/04/04(木)  y.Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    ' 　　　　：2018/04/06(金)  y.Fujisaku　引数追加 chk9
    ' 　　　　：2019/04/04(木)  y.Fujisaku　引数追加 chk10
    '***************************************************************************************************
    ''' <summary>SQL主文作成</summary>
    ''' <param name="strColumnName">検索対象カラム名</param> 
    ''' <param name="db">アクセスDBクラス</param>
    ''' <param name="chk1">チェックボックス1</param>
    ''' <param name="chk2">チェックボックス2</param>
    ''' <param name="chk3">チェックボックス3</param>
    ''' <param name="chk4">チェックボックス4</param>
    ''' <param name="chk5">チェックボックス5</param>
    ''' <param name="chk6">チェックボックス6</param>
    ''' <param name="chk7">チェックボックス7</param>
    ''' <param name="chk8">チェックボックス8</param>
    ''' <param name="chk9">チェックボックス9</param>
    ''' <param name="chk10">チェックボックス10</param>
    ''' <returns>SQL文</returns>
    ''' <remarks></remarks>
    Function funcCheckBox(ByVal strColumnName As String, ByVal db As CLAccessMdb, ByVal chk1 As CheckBox, _
                          Optional ByVal chk2 As CheckBox = Nothing, Optional ByVal chk3 As CheckBox = Nothing, _
                          Optional ByVal chk4 As CheckBox = Nothing, Optional ByVal chk5 As CheckBox = Nothing, _
                          Optional ByVal chk6 As CheckBox = Nothing, Optional ByVal chk7 As CheckBox = Nothing, _
                          Optional ByVal chk8 As CheckBox = Nothing, Optional ByVal chk9 As CheckBox = Nothing, _
                          Optional ByVal chk10 As CheckBox = Nothing) As String
        Dim strKey As String = ""
        Dim strSql As String = ""
        Try
            'チェックボックス１の確認
            If funcChkSQL(chk1, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス２の確認
            If funcChkSQL(chk2, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス３の確認
            If funcChkSQL(chk3, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス４の確認
            If funcChkSQL(chk4, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス５の確認
            If funcChkSQL(chk5, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス６の確認
            If funcChkSQL(chk6, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス７の確認
            If funcChkSQL(chk7, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス８の確認
            If funcChkSQL(chk8, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス９の確認
            If funcChkSQL(chk9, strColumnName, strSql, db) = False Then
                Return strSql
            End If
            'チェックボックス１０の確認
            If funcChkSQL(chk10, strColumnName, strSql, db) = False Then
                Return strSql
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '結果を返却する
        Return strSql
    End Function
#End Region

#Region "チェックボックスSQL作成"
    '***************************************************************************************************
    '   ＩＤ　：funcChkSQL
    '   名称　：チェックボックスSQL作成
    '   概要　：各チェックボックスに対応する、検索条件を追加したSQL文を返却する。
    '   引数　：ByVal chk1          As CheckBox    = チェックボックス
    '       　：ByVal strColumnName As String      = 検索対象カラム名
    '       　：ByRef strColumnName As String      = SQL文
    '   　　　：ByVal db            As CLAccessMdb = アクセスDBクラス
    '   戻り値：booRet As boolean = True/False
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>SQL主文作成</summary>
    ''' <param name="chk">チェックボックス1</param>
    ''' <param name="strColumnName">検索対象カラム名</param> 
    ''' <param name="strSql">条件追加SQL文</param>
    ''' <param name="db">アクセスDBクラス</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function funcChkSQL(ByVal chk As CheckBox, ByVal strColumnName As String, _
                        ByRef strSql As String, ByVal db As CLAccessMdb) As Boolean
        Dim booRet As Boolean = False
        Dim strKey As String = ""
        Try
            If Not (chk Is Nothing) Then
                If chk.Checked = True Then
                    '検索キーを設定する。
                    strKey = chk.Text
                    '性別の場合は男性→男、女性→女に変換する
                    If strColumnName = "性別" Then
                        If strKey = "男性" Then
                            strKey = "男"
                        ElseIf strKey = "女性" Then
                            strKey = "女"
                        End If
                    End If
                    '前に他の検索条件が設定されている場合はOR条件として追加する。
                    If ChkNull(strSql) = False Then
                        strSql = strSql & " OR "
                    End If
                    'チェックボックスの値に応じて付加するSQL文を決定
                    If strKey = "その他" Then
                        strSql = strSql & "(" & funcOtherString(strColumnName, db) & ")"
                    Else
                        strSql = strSql & CONSTANT_VIEW_SEARCH & "." & strColumnName & "='" & strKey & "'"
                    End If
                End If
            End If
            booRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '結果返却
        Return booRet
    End Function
#End Region

#Region "その他の処理"
    '***************************************************************************************************
    '   ＩＤ　：funcOtherString
    '   名称　：その他の処理
    '   概要　：チェックボックス項目（その他）に対応する、検索条件SQLを作成する。
    '   引数　：ByVal strColumnName As String      = 検索対象カラム名
    '   　　　：ByVal db            As CLAccessMdb = アクセスDBクラス
    '   戻り値：strRet As String = SQL文
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>その他の処理</summary>
    ''' <param name="strColumnName">検索対象カラム名</param> 
    ''' <param name="db">アクセスDBクラス</param>
    ''' <returns>SQL文</returns>
    ''' <remarks></remarks>
    Function funcOtherString(ByVal strColumnName As String, ByVal db As CLAccessMdb) As String
        Dim strRet As String = ""
        Dim strTKsh As String = ""
        Try
            '組合員種別の場合
            If strColumnName = "組合員種別" Then
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".組合員種別<>'正組合員') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".組合員種別<>'シニア組合員') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".組合員種別<>'永年組合員') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".組合員種別<>'準組合員') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".組合員種別<>'非組合員') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".組合員種別<>'脱退組合員')"
            End If
            '組合支部の場合
            If strColumnName = "組合支部" Then
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".組合支部<>'東京') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".組合支部<>'大阪')"
            End If
            '乗務資格の場合
            If strColumnName = "資格" Then
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".資格<>'機長') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".資格<>'副操縦士') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".資格<>'航空機関士') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".資格<>'教官機長')"
            End If
            '機種の場合
            If strColumnName = "機種" Then
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'B787') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'B777') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'B767') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'A320') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'B744') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'訓練機') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'B747') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'B737') and"
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".機種<>'A380')"
            End If
            '会社所属の場合は検索対象がコードのため、定数マスタからコードを取得する。
            If strColumnName = "会社所属" Then
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".会社所属<>'東京') and"
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".会社所属<>'大阪')"
            End If
            '職場の場合
            If strColumnName = "職場" Then
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".職場<>'乗員室') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".職場<>'訓練センター') and "
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".職場<>'本部')"
            End If
            '所属会社の場合
            If strColumnName = "所属会社" Then
                strRet = strRet & "(" & CONSTANT_VIEW_SEARCH & ".所属会社<>'ANA')"
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '結果返却
        Return strRet
    End Function
#End Region

#Region "日付項目確認"
    '***************************************************************************************************
    '   ＩＤ　：funcChkDate
    '   名称　：日付項目の確認
    '   概要　：日付項目の確認を行い、検索条件SQL文を作成する。
    '   引数　：ByRef          strSql     As String = 検索条件追加SQL
    '   　　　：ByVal          strBottom  As String = 検索日付最小値（年数/年）
    '   　　　：ByVal          strTop     As String = 検索日付最大値（年数/年）
    '   　　　：ByVal          chk        As String = 検索対象項目名
    '   　　　：Optional ByVal strBottom2 As String = 検索日付最小値（月）
    '   　　　：Optional ByVal strBottom2 As String = 検索日付最大値（月）
    '   戻り値：ExecuteSql As String = SQL文
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>SQL主文作成</summary>
    ''' <param name="strSql">検索条件追加SQL</param> 
    ''' <param name="strBottom">検索日付最小値（年数/年）</param>
    ''' <param name="strTop">検索日付最大値（年数/年）</param>
    ''' <param name="strChk">検索対象項目名</param>
    ''' <param name="strBottom2">検索日付最小値（月）</param>
    ''' <param name="strTop2">検索日付最大値（月）</param>
    ''' <returns>SQL文</returns>
    ''' <remarks></remarks>
    Function funcChkDate(ByRef strSql As String, _
                         ByVal strBottom As String, _
                         ByVal strTop As String, _
                         ByVal strChk As String, _
                         Optional ByVal strBottom2 As String = "", _
                         Optional ByVal strTop2 As String = "") As Boolean
        Dim booRet As Boolean = False
        Try
            If strChk = "CAP経験" Or strChk = "組合経験" Then

                '最小値・最大値ともに設定されている場合は、設定年数の間を取得する。
                If (ChkNull(strBottom) = False) And (ChkNull(strTop) = False) Then
                    strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & "." & strChk & ">=" & Integer.Parse(strBottom) & _
                    " AND " & CONSTANT_VIEW_SEARCH & "." & strChk & "<" & Integer.Parse(strTop) & ")"
                End If
                '最小値のみ設定されている場合
                If (ChkNull(strBottom) = False) And (ChkNull(strTop) = True) Then
                    strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & "." & strChk & ">=" & Integer.Parse(strBottom) & ")"
                End If
                '最大値のみ設定されている場合
                If (ChkNull(strBottom) = True) And (ChkNull(strTop) = False) Then
                    strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & "." & strChk & "<" & Integer.Parse(strTop) & ")"
                End If
            Else
                '最小値・最大値ともに設定されている場合は、設定年数の間を取得する
                If (ChkNull(strBottom) = False) And (ChkNull(strTop) = False) Then
                    If (ChkNull(strBottom2) = False) Then
                        strBottom = strBottom & "/" & strBottom2
                    Else
                        strBottom = strBottom & "/1"
                    End If
                    If (ChkNull(strTop2) = False) Then
                        strTop = strTop & "/" & strTop2
                    Else
                        strTop = strTop & "/12"
                    End If
                    strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & "." & strChk & ">='" & Format(Date.Parse(strBottom), "yyyy/MM") & _
                    "' AND " & CONSTANT_VIEW_SEARCH & "." & strChk & "<'" & Format(Date.Parse(strTop), "yyyy/MM") & "')"
                End If
                '最小値のみ設定されている場合
                If (ChkNull(strBottom) = False) And (ChkNull(strTop) = True) Then
                    If (ChkNull(strBottom2) = False) Then
                        strBottom = strBottom & "/" & strBottom2
                    Else
                        strBottom = strBottom & "/1"
                    End If
                    strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & "." & strChk & ">='" & Format(Date.Parse(strBottom), "yyyy/MM") & "')"
                End If
                '最大値のみ設定されている場合
                If (ChkNull(strBottom) = True) And (ChkNull(strTop) = False) Then
                    If (ChkNull(strTop2) = False) Then
                        strTop = strTop & "/" & strTop2
                    Else
                        strTop = strTop & "/12"
                    End If
                    strSql = strSql & " AND (" & CONSTANT_VIEW_SEARCH & "." & strChk & "<'" & Format(Date.Parse(strTop), "yyyy/MM") & "')"
                End If
            End If
            booRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        '結果返却
        Return booRet
    End Function
#End Region

#Region "コンボボックス作成"
    '***************************************************************************************************
    '   ＩＤ　：funcCreateCmb
    '   名称　：コンボボックス作成
    '   概要　：勤務形態→定数マスタから作成、所属委員会→委員会マスタから作成
    '   作成日：2011/11/25(金)  y.nakano
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金)  y.nakano  新規作成
    '***************************************************************************************************
    ''' <summary>コンボボックス作成</summary>
    ''' <remarks></remarks>
    Private Function funcCreateCmb() As Boolean
        Dim blnRet As Boolean = False       '処理結果
        Dim clsADB As New CLAccessMdb       'DBクラス生成
        Dim strSql As String                '所属委員会抽出SQL文
        Try
            'データベース接続
            clsADB.Connect()
            '勤務形態コンボボックス作成
            If CreateCboConstantDtl(clsADB, Me.cboWorkForm, CONSTANT_WORKFORM) = False Then
                Return blnRet
            End If
            '所属委員会コンボボックス作成
            strSql = ""
            strSql = strSql & " SELECT c_committee_id as ValueName, l_name as DisplayName FROM committee"
            strSql = strSql & "  WHERE (d_from<=FORMAT(GETDATE(),'yyyyMMdd')) AND (d_To>=FORMAT(GETDATE(),'yyyyMMdd'))"
            strSql = strSql & "  ORDER BY c_committee_id"   'chk
            If MDCommon.CreateComboBoxNew(clsADB, Me.cboCommittee, strSql, "DisplayName", "ValueName") = False Then
                Return False
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            'データベース切断
            clsADB.Disconnect()
        End Try
    End Function
#End Region
#End Region

End Class

#End Region
