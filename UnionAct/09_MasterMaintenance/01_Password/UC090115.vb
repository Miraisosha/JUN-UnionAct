'===========================================================================================================
'   クラスＩＤ　　：UC090115
'   クラス名称　　：昼食費マスタメンテナンス - 詳細
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo

Public Class UC090115

    Private cStrMode As String
    Private cStrFromData As String
    Private Const TITLE As String = "昼食費マスタメンテナンス"

    Public Sub setData(ByVal strId As String, ByVal strFromDate As String, ByVal strToDate As String, ByVal strPay As String, ByVal strBiko As String)
        Me.txtId.Text = strId
        Me.cboFromDate.Value = strFromDate
        cStrFromData = strFromDate
        Me.txtToDate.Text = strToDate
        Me.txtPay.Text = strPay
        Me.txtBiko.Text = strBiko
    End Sub

    Public Sub setMode(ByVal strMode As String)
        cStrMode = strMode

        If cStrMode = "READONLY" Then
            Me.lbl.Text = TITLE & " - 詳細"
            'Me.btnConfirm.Text = "内容変更"
            Me.btnConfirm.Enabled = False
            Me.btnCancel.Text = "戻る"
            Me.cboFromDate.Enabled = False

        ElseIf cStrMode = "ADDHISTORY" Then
            Me.lbl.Text = TITLE & " - 履歴登録"
            If (MDMasterCommon.GetMonthTopDate(Date.Today) > Me.cboFromDate.Value) Then
                Me.cboFromDate.MinDate = MDMasterCommon.GetMonthTopDate(Date.Today)
            Else
                Me.cboFromDate.MinDate = Me.cboFromDate.Value.AddMonths(1)
            End If
            Me.cboFromDate.Value = Me.cboFromDate.MinDate

        ElseIf cStrMode = "UPDATEHISTORY" Then
            Me.lbl.Text = TITLE & " - 詳細"
            'Me.btnConfirm.Text = "内容変更"
            Me.btnCancel.Text = "戻る"
            If MDMasterCommon.IsFutureMonth(cboFromDate.Value) = False Then
                Me.btnConfirm.Enabled = False
            Else
                Me.btnConfirm.Enabled = True
            End If
            Me.cboFromDate.Enabled = False

        Else
            Me.cboFromDate.MinDate = MDMasterCommon.GetMonthTopDate(Date.Today)
            Me.cboFromDate.Value = Me.cboFromDate.MinDate
        End If
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090114)

            If uc Is Nothing Then
                uc = New UC090114
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If
            Me.Dispose()
        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        End Try
    End Sub

    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        '▼入力データチェック
        If checkData() = False Then
            Return
        End If

        If CLMsg.Show("GQ0001") = DialogResult.No Then
            Return
        End If

        Dim clsMdb As New CLAccessMdb
        Try
            '▼データの更新
            Dim result As Integer = -1
            clsMdb.Connect()

            If cStrMode = "INSERT" Then
                Me.txtId.Text = getMaxId(clsMdb)
                clsMdb.ExecuteSql(String.Format("INSERT INTO executive_lunch_pay_master (c_executive_lunch_pay_id, d_from, d_to, s_pay, l_biko, d_ins, c_user_id_ins) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", _
                                                  Me.txtId.Text, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), Me.txtPay.Text, Me.txtBiko.Text, Date.Today.ToString, MDLoginInfo.UserId))
            ElseIf cStrMode = "ADDHISTORY" Then
                Dim strFrom As String = Format(DateValue(cStrFromData), "yyyyMMdd")
                Dim strTo As String = Format(DateValue(Me.cboFromDate.Text).AddDays(-1), "yyyyMMdd")

                result = clsMdb.ExecuteNonQuery(String.Format("UPDATE executive_lunch_pay_master SET d_to='{0}' WHERE c_executive_lunch_pay_id='{1}' AND d_from='{2}' AND d_to='99999999'", _
                                                              strTo, Me.txtId.Text, strFrom))
                If result <> -1 Then
                    clsMdb.ExecuteSql(String.Format("INSERT INTO executive_lunch_pay_master (c_executive_lunch_pay_id, d_from, d_to, s_pay, l_biko, d_ins, c_user_id_ins) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", _
                                                      Me.txtId.Text, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text), Me.txtPay.Text, Me.txtBiko.Text, Date.Today.ToString, MDLoginInfo.UserId))
                End If
            Else
                clsMdb.ExecuteSql(String.Format("UPDATE executive_lunch_pay_master SET s_pay='{0}',l_biko='{1}' WHERE c_executive_lunch_pay_id='{2}' and d_from='{3}' and d_to='{4}'", _
                                                  Me.txtPay.Text, Me.txtBiko.Text, Me.txtId.Text, MDMasterCommon.DateValueStr(Me.cboFromDate.Text), MDMasterCommon.DateValueStr(Me.txtToDate.Text)))
            End If

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            clsMdb.Disconnect()
            btnCancel_Click(sender, e)
        End Try
    End Sub

    Private Function getMaxId(ByVal clsMdb As CLAccessMdb)
        Dim table As New DataTable
        Dim intId As Integer = 1
        Try
            '▼SQL実行
            table = clsMdb.ExecuteSql("SELECT max(c_executive_lunch_pay_id) AS max_id FROM executive_lunch_pay_master")
            intId = table.Rows(0)("max_id")
            intId = intId + 1

        Catch ex As Exception
            CLMsg.Show("GE0001")
            log.Fatal(ex.Message)
        Finally
            '▼後処理
            table.Dispose()
        End Try
        Return Format(intId, "000")
    End Function

    Private Function checkData() As Boolean
        If Me.txtPay.Text.Length = 0 Then
            MsgBox("金額を入力して下さい。", vbExclamation, TITLE)
            Me.txtPay.Focus()
            Return False
        End If

        Return True
    End Function

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
End Class
