Public Class UC040502
    '組合員の追加
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim cForm1 As New FM000204()
        ' モーダルで表示する
        cForm1.ShowDialog()

        ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
        cForm1.Dispose()
    End Sub
End Class
