<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM000105
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnYes = New System.Windows.Forms.Button
        Me.btnNo = New System.Windows.Forms.Button
        Me.txtErr = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'btnYes
        '
        Me.btnYes.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnYes.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnYes.Location = New System.Drawing.Point(32, 226)
        Me.btnYes.Name = "btnYes"
        Me.btnYes.Size = New System.Drawing.Size(110, 30)
        Me.btnYes.TabIndex = 0
        Me.btnYes.Text = "はい"
        '
        'btnNo
        '
        Me.btnNo.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnNo.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnNo.Location = New System.Drawing.Point(396, 226)
        Me.btnNo.Name = "btnNo"
        Me.btnNo.Size = New System.Drawing.Size(110, 30)
        Me.btnNo.TabIndex = 1
        Me.btnNo.Text = "いいえ"
        '
        'txtErr
        '
        Me.txtErr.BackColor = System.Drawing.SystemColors.Control
        Me.txtErr.Font = New System.Drawing.Font("ＭＳ ゴシック", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtErr.ForeColor = System.Drawing.Color.Black
        Me.txtErr.Location = New System.Drawing.Point(14, 12)
        Me.txtErr.Multiline = True
        Me.txtErr.Name = "txtErr"
        Me.txtErr.ReadOnly = True
        Me.txtErr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtErr.Size = New System.Drawing.Size(510, 200)
        Me.txtErr.TabIndex = 69
        Me.txtErr.TabStop = False
        '
        'FM000105
        '
        Me.AcceptButton = Me.btnYes
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnNo
        Me.ClientSize = New System.Drawing.Size(539, 272)
        Me.Controls.Add(Me.txtErr)
        Me.Controls.Add(Me.btnNo)
        Me.Controls.Add(Me.btnYes)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM000105"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "問合せ"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnYes As System.Windows.Forms.Button
    Friend WithEvents btnNo As System.Windows.Forms.Button
    Friend WithEvents txtErr As System.Windows.Forms.TextBox

End Class
