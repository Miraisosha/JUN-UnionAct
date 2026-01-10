Public Class UC040501
    '新規登録
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim pn As Panel
        Dim uc As Control

        Me.Visible = False

        pn = ParentForm.Controls("Panel2")
        uc = pn.Controls("UC040502")

        If uc Is Nothing Then
            uc = New UC040502

            Call pn.Controls.Add(uc)
        Else
            uc.Visible = True
        End If
    End Sub

    '詳細
    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim pn As Panel
        Dim uc As Control

        Me.Visible = False

        pn = ParentForm.Controls("Panel2")
        uc = pn.Controls("UC040502")

        If uc Is Nothing Then
            uc = New UC040502
            uc.Controls("GroupBox2").Controls("Button1").Enabled = False
            uc.Controls("GroupBox2").Controls("Button2").Enabled = False
            Call pn.Controls.Add(uc)
        Else
            uc.Visible = True
        End If
    End Sub

    '印刷ボタン
    Private Sub Button11_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button11.Click
        Dim cForm1 As New FM040503()
        ' モーダルで表示する
        cForm1.ShowDialog()

        ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
        cForm1.Dispose()
    End Sub
End Class
