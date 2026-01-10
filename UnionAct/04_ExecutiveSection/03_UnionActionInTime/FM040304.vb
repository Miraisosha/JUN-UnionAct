Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDInfo
Imports UnionAct.GUI.Common

Public Class FM040304
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Dim agoUserControl As FM040303
    Public Sub New(ByVal setForm As FM040303)

        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()
        agoUserControl = setForm
        ' InitializeComponent() 呼び出しの後で初期化を追加します。

    End Sub

#Region "新規登録ボタン"
    'OKボタンクリック
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim dateFrom As String                              '期間の開始日
        Dim dateTo As String                                '期間の終了日
        Dim dbAccess As New CLAccessMdb                     'DBアクセス
        Dim sql As String                                   'SQL文
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            dateFrom = Format(dtpDateFrom.Value.Date, MDConst.DATE_YYYYMMDD_FORMAT)
            dateTo = Format(dtpDateTo.Value.Date, MDConst.DATE_YYYYMMDD_FORMAT)
            If CheckData(dateFrom, dateTo) Then
                Exit Sub
            End If
            'DBへデータを登録
            dbAccess.Connect()
            sql = "Insert into apply_strike_executive_term(d_from,d_to,l_biko_1,d_ins,c_user_id_ins,d_up,c_user_id_up,s_up) values("
            sql = sql + "'" + dateFrom + "',"
            sql = sql + "'" + dateTo + "',"
            sql = sql + "'" + txtBiko.Text + "',"
            sql = sql + "'" + Now.ToString + "',"
            sql = sql + "'" + MDLoginInfo.UserId + "',"
            sql = sql + "'" + Now.ToString + "',"
            sql = sql + "'" + MDLoginInfo.UserId + "',"
            sql = sql + "0)"
            dbAccess.BeginTran()
            dbAccess.ExecuteNonQuery(sql)
            dbAccess.CommitTran()

            ' 種類の選択ダイアログ表示
            agoUserControl.Visible = True
            agoUserControl.grpExecutive.Visible = False
            agoUserControl.btnOkHeto.Visible = False
            agoUserControl.btnOk.Visible = True
            agoUserControl.btnOk.Location = New Point(182, 110)
            agoUserControl.btnCancel.Location = New Point(313, 110)
            agoUserControl.Height = 189
            agoUserControl.Location = New Point(335, 224)
            Call Utilities.SetCanEditToControl(True, agoUserControl.cmbApplyArea)
            Call Utilities.SetCanEditToControl(True, agoUserControl.cmbApplyClassify)
            agoUserControl.btnOk_Click(Nothing, Nothing)
            Me.Dispose()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040304, SCREEN_NAME_FM040304, "btnOk_Click")
        Finally
            dbAccess.Disconnect()
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "キャンセルボタン"
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        ' 処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 種類の選択ダイアログ表示
            agoUserControl.Visible = True
            agoUserControl.grpExecutive.Visible = False
            agoUserControl.btnOkHeto.Visible = False
            agoUserControl.btnOk.Visible = True
            agoUserControl.btnOk.Location = New Point(182, 110)
            agoUserControl.btnCancel.Location = New Point(313, 110)
            agoUserControl.Height = 189
            agoUserControl.Location = New Point(335, 224)
            Call Utilities.SetCanEditToControl(True, agoUserControl.cmbApplyArea)
            Call Utilities.SetCanEditToControl(True, agoUserControl.cmbApplyClassify)
            Me.Dispose()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040304, SCREEN_NAME_FM040304, "btnCancel_Click")
        End Try

        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "更新ボタン"
    '更新ボタン
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Dim dbAccess As New CLAccessMdb                     'DBアクセス
        Dim sql As String                                   'SQL
        Dim dateFrom As String                              '期間の開始日
        Dim dateTo As String                                '期間の終了日
        ' 処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            dateFrom = Format(dtpDateFrom.Value.Date, MDConst.DATE_YYYYMMDD_FORMAT)
            dateTo = Format(dtpDateTo.Value.Date, MDConst.DATE_YYYYMMDD_FORMAT)
            If CheckData(dateFrom, dateTo) Then
                Exit Sub
            End If
            sql = "Update apply_strike_executive_term set "
            sql = sql + "d_from='" + dateFrom + "',"
            sql = sql + "d_to='" + dateTo + "',"
            sql = sql + "l_biko_1='" + txtBiko.Text + "',"
            sql = sql + "d_up='" + Now + "',"
            sql = sql + "c_user_id_up='" + MDLoginInfo.UserId + "',"
            sql = sql + "s_up=s_up+1 "
            sql = sql + "where c_apply_strike_term_id=" + lblTermID.Text
            dbAccess.Connect()
            dbAccess.BeginTran()
            dbAccess.ExecuteNonQuery(sql)
            dbAccess.CommitTran()

            ' 種類の選択ダイアログ表示
            agoUserControl.Visible = True
            agoUserControl.grpExecutive.Visible = False
            agoUserControl.btnOkHeto.Visible = False
            agoUserControl.btnOk.Visible = True
            agoUserControl.btnOk.Location = New Point(182, 110)
            agoUserControl.btnCancel.Location = New Point(313, 110)
            agoUserControl.Height = 189
            agoUserControl.Location = New Point(335, 224)
            Call Utilities.SetCanEditToControl(True, agoUserControl.cmbApplyArea)
            Call Utilities.SetCanEditToControl(True, agoUserControl.cmbApplyClassify)
            agoUserControl.btnOk_Click(Nothing, Nothing)
            Me.Dispose()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040304, SCREEN_NAME_FM040304, "btnUpdate_Click")
        Finally
            dbAccess.Disconnect()
        End Try

        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "フォームロード"
    Private Sub FM040304_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' 処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            Me.Location = New Point(341, 403)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040304, SCREEN_NAME_FM040304, "FM040304_Load")
        End Try

        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

    Private Function CheckData(ByVal strFrom As String, ByVal strTo As String)
        Dim blnFlg As Boolean                       '戻り値
        Try
            If (strFrom > strTo) Then
                CLMsg.Show("GE0032", "開始日", "終了日")
                Return True
            End If
            If txtBiko.Text.Equals("") Then
                txtBiko.BackColor = Color.LightPink
                CLMsg.Show("GE0006", "備考")
                Return True
            End If
            If txtBiko.TextLength > 240 Then
                CLMsg.Show("GE0112", "備考は、", "240", "240")
                txtBiko.BackColor = Color.LightPink
                Return True
            End If
            blnFlg = CheckFromToDate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040304, SCREEN_NAME_FM040304, "CheckData")
        End Try
        Return blnFlg
    End Function

    Private Function CheckFromToDate()
        Dim blnFlg As Boolean
        Try
            Dim str3 As String = ""
            Dim str4 As String = ""
            Dim strKeyDate As String = (Me.dtpDateFrom.Value.Year.ToString("00") & Me.dtpDateFrom.Value.Month.ToString("00") & Me.dtpDateFrom.Value.Day.ToString("00"))
            Dim strB As String = (Me.dtpDateTo.Value.Year.ToString("00") & Me.dtpDateTo.Value.Month.ToString("00"))
            GetTermStartEnd(lblKind.Text, strKeyDate, str3, str4)
            If (str4.CompareTo(strB) < 0) Then
                CLMsg.Show("GE0125")
                Return True
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040304, SCREEN_NAME_FM040304, "CheckFromToDate")
        End Try
        Return blnFlg
    End Function

End Class