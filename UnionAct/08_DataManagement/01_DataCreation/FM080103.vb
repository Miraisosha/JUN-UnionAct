'===========================================================================================================
'   クラスＩＤ　　：FM080103
'   クラス名称　　：労金データ作成－振込データ新規作成
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Common

Public Class FM080103
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM080103
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM080103

    'クリックボタン判別用
    Private _intClickBtnFlg As Integer = -1
    '0 = OKボタン
    '1 = キャンセルボタン

    Private _strPayStatus As String = String.Empty
    Private _strPayStatusCd As String = String.Empty
    Private _strTitle As String = String.Empty
    Private _datePayDate As String = String.Empty

    Public ReadOnly Property intClickBtn() As Integer 'クリックボタン返却用
        Get
            Return _intClickBtnFlg
        End Get
    End Property

    Public ReadOnly Property strPayStatus() As String '支払方法返却用
        Get
            Return _strPayStatus
        End Get
    End Property

    Public ReadOnly Property strPayStatusCd() As String '支払方法コード返却用
        Get
            Return _strPayStatusCd
        End Get
    End Property

    Public ReadOnly Property strTitle() As String '題目返却用
        Get
            Return _strTitle
        End Get
    End Property

    Public ReadOnly Property datePayDate() As DateTime '振込日返却用
        Get
            Return _datePayDate
        End Get
    End Property

#Region "イベント"

#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：FM080103_Load
    '   名称　：フォームロード
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub FM080103_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        'コンボボックス作成処理
        If Me.SetCombo() = False Then
            Exit Sub
        End If
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "OKボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnOK_Click
    '   名称　：OKボタンクリック
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
            If Me.ChkInput() = False Then
                Exit Sub
            End If

            '振込日に過去日が選択されていないかチェック
            If (DateTime.Compare(Me.dtpayDay.Value.Date, Now.Date) < 0) Then
                CLMsg.Show("GE0141", "振込日")
                Exit Sub
            End If

            '画面遷移処理
            Call Me.TransitionScreen()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        End Try
    End Sub
#End Region

#Region "キャンセルボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        _intClickBtnFlg = 1
        Me.Close()
    End Sub
#End Region

#Region "支払方法KeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbPayStatus_KeyPress
    '   名称　：支払方法KeyPressイベント
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPayStatus_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPayStatus.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            If Me.ChkInput() = False Then
                Exit Sub
            End If

            If (DateTime.Compare(Me.dtpayDay.Value.Date, Now.Date) < 0) Then
                CLMsg.Show("GE0141", "振込日")
                Exit Sub
            End If

            '画面遷移処理
            Call Me.TransitionScreen()
        End If
    End Sub
#End Region

#Region "題目KeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtTitle_KeyPress
    '   名称　：題目KeyPressイベント
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtTitle_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTitle.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            If Me.ChkInput() = False Then
                Exit Sub
            End If

            If (DateTime.Compare(Me.dtpayDay.Value.Date, Now.Date) < 0) Then
                CLMsg.Show("GE0141", "振込日")
                Exit Sub
            End If

            '画面遷移処理
            Call Me.TransitionScreen()
        End If
    End Sub
#End Region

#Region "振込日KeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：dtpayDay_KeyPress
    '   名称　：振込日KeyPressイベント
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dtpayDay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles dtpayDay.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            If Me.ChkInput() = False Then
                Exit Sub
            End If

            If (DateTime.Compare(Me.dtpayDay.Value.Date, Now.Date) < 0) Then
                CLMsg.Show("GE0141", "振込日")
                Exit Sub
            End If

            '画面遷移処理
            Call Me.TransitionScreen()
        End If
    End Sub
#End Region

#End Region

#Region "関数"

#Region "コンボボックスに値をセット"
    '***************************************************************************************************
    '   ＩＤ　：SetCombo
    '   名称　：コンボボックスに値をセット
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function SetCombo() As Boolean
        '支払方法の取得
        Dim strSql As String = "SELECT c_constant_seq,l_name FROM constant_dtl WHERE c_constant = 'BANK_SEND_MARGIN' "
        Dim clsDb As CLAccessMdb = New CLAccessMdb

        Try
            clsDb.Connect()
            '取得内容をコンボボックスにセット
            If MDCommon.CreateComboBoxNew(clsDb, Me.cmbPayStatus, strSql, "l_name", "c_constant_seq", True) = False Then
                Return False
            End If
            Return True
        Catch ex As Exception
        Finally
            clsDb.Disconnect()
        End Try
    End Function
#End Region

#Region "画面遷移処理"
    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub TransitionScreen()
        '支払方法をセット
        _strPayStatus = Me.cmbPayStatus.Text
        '支払方法コードをセット
        _strPayStatusCd = Me.cmbPayStatus.SelectedValue
        '題目をセット
        _strTitle = Me.txtTitle.Text
        '振込日をセット
        _datePayDate = Me.dtpayDay.Value.Date
        'クリックボタンフラグをセット
        _intClickBtnFlg = 0
        Me.Close()
    End Sub

#End Region

#Region "必須項目入力チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：必須項目入力チェック
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkInput() As Boolean
        'エラーフラグ
        Dim blnRet As Boolean = True
        'エラーメッセージリスト
        Dim errMsg As ArrayList = New ArrayList
        'エラー項目リスト
        Dim aryErrorTitle As ArrayList = New ArrayList

        If errMsg.Count > 0 Then
            errMsg.Clear()
        End If

        '各入力項目に不正な値がないかチェック
        If Me.cmbPayStatus.Text = String.Empty Then
            aryErrorTitle.Add("支払方法")
        End If
        If Me.txtTitle.Text = String.Empty Then
            aryErrorTitle.Add("題目")
        End If

        'エラーメッセージが一つでも格納された場合、falseを返却
        If aryErrorTitle.Count > 0 Then
            blnRet = False
            If aryErrorTitle.Count = 1 Then
                CLMsg.Show("GE0006", aryErrorTitle(0))
                Return blnRet
            Else
                For iCnt As Integer = 0 To aryErrorTitle.Count - 1
                    errMsg.Add(CLMsg.GetMsg("GE0006", aryErrorTitle(iCnt)))
                Next

                ' エラーメッセージボックス表示
                Dim clsUC999999 As New UC999999
                clsUC999999.errMsgList = errMsg

                ' メインメニュー画面表示
                Call clsUC999999.ShowDialog()
                Return blnRet
            End If

        End If
        Return blnRet
    End Function

#End Region

#End Region

End Class