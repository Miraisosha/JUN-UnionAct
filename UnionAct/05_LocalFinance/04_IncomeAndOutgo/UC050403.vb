Public Class UC050403
    'キャンセルボタン
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim pn As Panel
        Dim uc As Control

        Me.Visible = False

        pn = ParentForm.Controls("Panel2")
        uc = pn.Controls("UC050401")

        If uc Is Nothing Then
            uc = New UC050401

            Call pn.Controls.Add(uc)
        Else
            uc.Visible = True
        End If
        Me.Dispose()
    End Sub

    Private Sub Label7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label7.Click

    End Sub

    Private Sub TextBox4_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox4.TextChanged

    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

    End Sub
End Class
