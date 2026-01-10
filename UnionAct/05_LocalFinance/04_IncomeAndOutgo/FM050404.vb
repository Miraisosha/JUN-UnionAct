Public Class FM050404
    Dim agoUserControl As System.Windows.Forms.UserControl
    Public Sub New()
        'ここに初期処理を書く
        InitializeComponent()
    End Sub

    Public Sub New(ByVal setForm As System.Windows.Forms.UserControl)
        'ここに初期処理を書く
        InitializeComponent()
        agoUserControl = setForm
    End Sub

    'OKボタン
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim pn As Panel
        Dim uc As Control

        pn = agoUserControl.ParentForm.Controls("Panel2")
        agoUserControl.Visible = False
        'pn.Controls("UC050401").Visible = False
        uc = pn.Controls("UC050403")

        If uc Is Nothing Then
            uc = New UC050403
            uc.Controls("label11").Text = "乗員計画状況 - 新規登録"
            Call pn.Controls.Add(uc)
        Else
            uc.Visible = True
        End If
        Me.Dispose()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub
End Class