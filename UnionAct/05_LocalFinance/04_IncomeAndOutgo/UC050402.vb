Public Class UC050402
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
End Class
