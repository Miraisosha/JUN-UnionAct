'===========================================================================================================
'   クラスＩＤ　　：UC090105
'   クラス名称　　：パスワードマスタメンテナンス - パスワードリセット
'   備考  　　　　：
'===========================================================================================================

Imports System.Data.OleDb
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLEncrypt
Imports UnionAct.NSMDInfo

Public Class UC090105

    Public Sub setData(ByVal staffNo As String, ByVal staffName As String, ByVal userId As String)
        Me.txtStaffNo.Text = staffNo
        Me.txtStaffName.Text = staffName
        Me.txtUserId.Text = userId
    End Sub

    Private Sub btnPasswordReset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPasswordReset.Click
        Dim clsMdb As New CLAccessMdb
        Try
            Dim intBtnRet As Integer = CLMsg.Show("GQ0034")

            If intBtnRet = 6 Then '「はい」ボタンが選択された場合処理を行う
                '▼データの更新
                clsMdb.Connect()
                clsMdb.ExecuteSql(String.Format("UPDATE certify SET c_pwd='{0}',c_user_id_up='{1}',d_up='{2}' WHERE c_user_id='{3}'", _
                                                  CLEncrypt.Encrypt(Me.txtUserId.Text, MDSystemInfo.EncryptKey), MDLoginInfo.UserId, Date.Today.ToString, Me.txtUserId.Text))
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

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Dim pn As Panel
            Dim uc As Control

            Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC090104)

            If uc Is Nothing Then
                uc = New UC090104
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

    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
End Class
