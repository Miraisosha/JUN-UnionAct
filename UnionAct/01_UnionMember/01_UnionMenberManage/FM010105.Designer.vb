<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM010105
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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnPritntPreview = New System.Windows.Forms.Button
        Me.grpStandard = New System.Windows.Forms.GroupBox
        Me.cboDay = New System.Windows.Forms.ComboBox
        Me.cboMonth = New System.Windows.Forms.ComboBox
        Me.cboYear = New System.Windows.Forms.ComboBox
        Me.lblDay = New System.Windows.Forms.Label
        Me.LblMonth = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.Label
        Me.grpStandard.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(290, 235)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnPritntPreview
        '
        Me.btnPritntPreview.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPritntPreview.Location = New System.Drawing.Point(154, 235)
        Me.btnPritntPreview.Name = "btnPritntPreview"
        Me.btnPritntPreview.Size = New System.Drawing.Size(110, 30)
        Me.btnPritntPreview.TabIndex = 3
        Me.btnPritntPreview.Text = "印刷プレビュー"
        Me.btnPritntPreview.UseVisualStyleBackColor = True
        '
        'grpStandard
        '
        Me.grpStandard.Controls.Add(Me.cboDay)
        Me.grpStandard.Controls.Add(Me.cboMonth)
        Me.grpStandard.Controls.Add(Me.cboYear)
        Me.grpStandard.Controls.Add(Me.lblDay)
        Me.grpStandard.Controls.Add(Me.LblMonth)
        Me.grpStandard.Controls.Add(Me.lblYear)
        Me.grpStandard.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpStandard.Location = New System.Drawing.Point(38, 39)
        Me.grpStandard.Name = "grpStandard"
        Me.grpStandard.Size = New System.Drawing.Size(468, 158)
        Me.grpStandard.TabIndex = 5
        Me.grpStandard.TabStop = False
        Me.grpStandard.Text = "基準日"
        '
        'cboDay
        '
        Me.cboDay.FormattingEnabled = True
        Me.cboDay.Location = New System.Drawing.Point(299, 69)
        Me.cboDay.Name = "cboDay"
        Me.cboDay.Size = New System.Drawing.Size(50, 24)
        Me.cboDay.TabIndex = 2
        Me.cboDay.Text = "11"
        '
        'cboMonth
        '
        Me.cboMonth.FormattingEnabled = True
        Me.cboMonth.Location = New System.Drawing.Point(204, 69)
        Me.cboMonth.Name = "cboMonth"
        Me.cboMonth.Size = New System.Drawing.Size(50, 24)
        Me.cboMonth.TabIndex = 1
        Me.cboMonth.Text = "11"
        '
        'cboYear
        '
        Me.cboYear.FormattingEnabled = True
        Me.cboYear.Location = New System.Drawing.Point(89, 69)
        Me.cboYear.Name = "cboYear"
        Me.cboYear.Size = New System.Drawing.Size(64, 24)
        Me.cboYear.TabIndex = 0
        Me.cboYear.Text = "2011"
        '
        'lblDay
        '
        Me.lblDay.AutoSize = True
        Me.lblDay.Location = New System.Drawing.Point(355, 73)
        Me.lblDay.Name = "lblDay"
        Me.lblDay.Size = New System.Drawing.Size(24, 16)
        Me.lblDay.TabIndex = 8
        Me.lblDay.Text = "日"
        '
        'LblMonth
        '
        Me.LblMonth.AutoSize = True
        Me.LblMonth.Location = New System.Drawing.Point(260, 74)
        Me.LblMonth.Name = "LblMonth"
        Me.LblMonth.Size = New System.Drawing.Size(24, 16)
        Me.LblMonth.TabIndex = 7
        Me.LblMonth.Text = "月"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.Location = New System.Drawing.Point(159, 75)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(24, 16)
        Me.lblYear.TabIndex = 6
        Me.lblYear.Text = "年"
        '
        'FM010105
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(552, 296)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnPritntPreview)
        Me.Controls.Add(Me.grpStandard)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM010105"
        Me.Text = "基準日入力画面"
        Me.grpStandard.ResumeLayout(False)
        Me.grpStandard.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnPritntPreview As System.Windows.Forms.Button
    Friend WithEvents grpStandard As System.Windows.Forms.GroupBox
    Friend WithEvents lblDay As System.Windows.Forms.Label
    Friend WithEvents LblMonth As System.Windows.Forms.Label
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents cboDay As System.Windows.Forms.ComboBox
    Friend WithEvents cboMonth As System.Windows.Forms.ComboBox
    Friend WithEvents cboYear As System.Windows.Forms.ComboBox
End Class
