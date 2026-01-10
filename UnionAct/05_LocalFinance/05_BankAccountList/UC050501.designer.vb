<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC050501
    Inherits System.Windows.Forms.UserControl

    'UserControl はコンポーネント一覧をクリーンアップするために dispose をオーバーライドします。
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
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpJpnSyllabary = New System.Windows.Forms.GroupBox
        Me.btnPrintJpnSyllabary = New System.Windows.Forms.Button
        Me.dtpTargetDate = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.grpJpnSyllabary.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(200, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(620, 35)
        Me.lblTitle.TabIndex = 7
        Me.lblTitle.Text = "銀行口座一覧出力"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpJpnSyllabary
        '
        Me.grpJpnSyllabary.Controls.Add(Me.Label1)
        Me.grpJpnSyllabary.Controls.Add(Me.dtpTargetDate)
        Me.grpJpnSyllabary.Controls.Add(Me.btnPrintJpnSyllabary)
        Me.grpJpnSyllabary.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.grpJpnSyllabary.Location = New System.Drawing.Point(125, 303)
        Me.grpJpnSyllabary.Name = "grpJpnSyllabary"
        Me.grpJpnSyllabary.Size = New System.Drawing.Size(777, 188)
        Me.grpJpnSyllabary.TabIndex = 13
        Me.grpJpnSyllabary.TabStop = False
        Me.grpJpnSyllabary.Text = "組合員銀行口座一覧出力"
        '
        'btnPrintJpnSyllabary
        '
        Me.btnPrintJpnSyllabary.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrintJpnSyllabary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPrintJpnSyllabary.Location = New System.Drawing.Point(532, 78)
        Me.btnPrintJpnSyllabary.Name = "btnPrintJpnSyllabary"
        Me.btnPrintJpnSyllabary.Size = New System.Drawing.Size(110, 25)
        Me.btnPrintJpnSyllabary.TabIndex = 5
        Me.btnPrintJpnSyllabary.Text = "ファイル出力"
        Me.btnPrintJpnSyllabary.UseVisualStyleBackColor = True
        '
        'dtpTargetDate
        '
        Me.dtpTargetDate.Location = New System.Drawing.Point(219, 78)
        Me.dtpTargetDate.Name = "dtpTargetDate"
        Me.dtpTargetDate.Size = New System.Drawing.Size(200, 25)
        Me.dtpTargetDate.TabIndex = 15
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(116, 84)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(62, 18)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "基準日"
        '
        'UC050501
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpJpnSyllabary)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC050501"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpJpnSyllabary.ResumeLayout(False)
        Me.grpJpnSyllabary.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpJpnSyllabary As System.Windows.Forms.GroupBox
    Friend WithEvents btnPrintJpnSyllabary As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpTargetDate As System.Windows.Forms.DateTimePicker

End Class
