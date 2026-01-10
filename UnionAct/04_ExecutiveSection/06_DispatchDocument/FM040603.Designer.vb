<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM040603
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
Me.btnOK = New System.Windows.Forms.Button
Me.btnCancel = New System.Windows.Forms.Button
Me.txtFileName = New System.Windows.Forms.TextBox
Me.lblMemo1 = New System.Windows.Forms.Label
Me.lblMemo2 = New System.Windows.Forms.Label
Me.SuspendLayout()
'
'btnOK
'
Me.btnOK.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
Me.btnOK.Location = New System.Drawing.Point(371, 25)
Me.btnOK.Name = "btnOK"
Me.btnOK.Size = New System.Drawing.Size(110, 30)
Me.btnOK.TabIndex = 1
Me.btnOK.Text = "OK"
Me.btnOK.UseVisualStyleBackColor = True
'
'btnCancel
'
Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
Me.btnCancel.Location = New System.Drawing.Point(371, 65)
Me.btnCancel.Name = "btnCancel"
Me.btnCancel.Size = New System.Drawing.Size(110, 30)
Me.btnCancel.TabIndex = 2
Me.btnCancel.Text = "キャンセル"
Me.btnCancel.UseVisualStyleBackColor = True
'
'txtFileName
'
Me.txtFileName.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
Me.txtFileName.Location = New System.Drawing.Point(20, 115)
Me.txtFileName.Name = "txtFileName"
Me.txtFileName.Size = New System.Drawing.Size(458, 22)
Me.txtFileName.TabIndex = 0
'
'lblMemo1
'
Me.lblMemo1.AutoSize = True
Me.lblMemo1.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
Me.lblMemo1.Location = New System.Drawing.Point(17, 25)
Me.lblMemo1.Name = "lblMemo1"
Me.lblMemo1.Size = New System.Drawing.Size(222, 15)
Me.lblMemo1.TabIndex = 3
Me.lblMemo1.Text = "文書のファイル名を入力してください。"
'
'lblMemo2
'
Me.lblMemo2.AutoSize = True
Me.lblMemo2.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
Me.lblMemo2.Location = New System.Drawing.Point(17, 51)
Me.lblMemo2.Name = "lblMemo2"
Me.lblMemo2.Size = New System.Drawing.Size(310, 15)
Me.lblMemo2.TabIndex = 4
Me.lblMemo2.Text = "　※入力したファイル名でデータベースへ登録します。"
'
'FM040603
'
Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
Me.ClientSize = New System.Drawing.Size(490, 166)
Me.Controls.Add(Me.lblMemo2)
Me.Controls.Add(Me.lblMemo1)
Me.Controls.Add(Me.txtFileName)
Me.Controls.Add(Me.btnCancel)
Me.Controls.Add(Me.btnOK)
Me.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
Me.MaximizeBox = False
Me.MinimizeBox = False
Me.Name = "FM040603"
Me.Text = "名前を付けて保存"
Me.TopMost = True
Me.ResumeLayout(False)
Me.PerformLayout()

End Sub
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtFileName As System.Windows.Forms.TextBox
    Friend WithEvents lblMemo1 As System.Windows.Forms.Label
    Friend WithEvents lblMemo2 As System.Windows.Forms.Label
End Class
