Public Class UC040405

    '組合員の追加ボタン
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim cForm1 As New FM000204()
        ' モーダルで表示する
        cForm1.ShowDialog()

        ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
        cForm1.Dispose()
    End Sub

    'キャンセルボタン
    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim pn As Panel
        Dim uc As Control

        Me.Visible = False

        pn = ParentForm.Controls("Panel2")
        uc = pn.Controls("UC040401")

        If uc Is Nothing Then
            uc = New UC040401

            Call pn.Controls.Add(uc)
        Else
            uc.Visible = True
        End If
        Me.Dispose()
    End Sub

    Private Sub DataGridView1_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick

    End Sub
End Class
