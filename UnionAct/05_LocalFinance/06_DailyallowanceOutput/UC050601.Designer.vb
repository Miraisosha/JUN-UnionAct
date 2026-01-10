<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC050601
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
        Me.grpDaily = New System.Windows.Forms.GroupBox
        Me.LblDailyLabel = New System.Windows.Forms.Label
        Me.dtpDailyTo = New System.Windows.Forms.DateTimePicker
        Me.LblDailySpecifyPeriod = New System.Windows.Forms.Label
        Me.dtpDailyFrom = New System.Windows.Forms.DateTimePicker
        Me.btnDailyOutput = New System.Windows.Forms.Button
        Me.grpMonthly = New System.Windows.Forms.GroupBox
        Me.LblMonthlyLabel = New System.Windows.Forms.Label
        Me.LblMonthlySpecifyPeriod = New System.Windows.Forms.Label
        Me.btnMonthlyOutput = New System.Windows.Forms.Button
        Me.dtpMonthlyFrom = New System.Windows.Forms.DateTimePicker
        Me.dtpMonthlyTo = New System.Windows.Forms.DateTimePicker
        Me.grpDaily.SuspendLayout()
        Me.grpMonthly.SuspendLayout()
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
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "日当データ出力"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpDaily
        '
        Me.grpDaily.Controls.Add(Me.LblDailyLabel)
        Me.grpDaily.Controls.Add(Me.dtpDailyTo)
        Me.grpDaily.Controls.Add(Me.LblDailySpecifyPeriod)
        Me.grpDaily.Controls.Add(Me.dtpDailyFrom)
        Me.grpDaily.Controls.Add(Me.btnDailyOutput)
        Me.grpDaily.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.grpDaily.Location = New System.Drawing.Point(108, 114)
        Me.grpDaily.Name = "grpDaily"
        Me.grpDaily.Size = New System.Drawing.Size(780, 120)
        Me.grpDaily.TabIndex = 1
        Me.grpDaily.TabStop = False
        Me.grpDaily.Text = "日単位出力"
        '
        'LblDailyLabel
        '
        Me.LblDailyLabel.AutoSize = True
        Me.LblDailyLabel.Location = New System.Drawing.Point(323, 57)
        Me.LblDailyLabel.Name = "LblDailyLabel"
        Me.LblDailyLabel.Size = New System.Drawing.Size(26, 18)
        Me.LblDailyLabel.TabIndex = 4
        Me.LblDailyLabel.Text = "～"
        '
        'dtpDailyTo
        '
        Me.dtpDailyTo.Location = New System.Drawing.Point(378, 54)
        Me.dtpDailyTo.Name = "dtpDailyTo"
        Me.dtpDailyTo.Size = New System.Drawing.Size(153, 25)
        Me.dtpDailyTo.TabIndex = 5
        '
        'LblDailySpecifyPeriod
        '
        Me.LblDailySpecifyPeriod.AutoSize = True
        Me.LblDailySpecifyPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblDailySpecifyPeriod.Location = New System.Drawing.Point(29, 57)
        Me.LblDailySpecifyPeriod.Name = "LblDailySpecifyPeriod"
        Me.LblDailySpecifyPeriod.Size = New System.Drawing.Size(80, 18)
        Me.LblDailySpecifyPeriod.TabIndex = 2
        Me.LblDailySpecifyPeriod.Text = "指定期間"
        '
        'dtpDailyFrom
        '
        Me.dtpDailyFrom.Location = New System.Drawing.Point(140, 54)
        Me.dtpDailyFrom.Name = "dtpDailyFrom"
        Me.dtpDailyFrom.Size = New System.Drawing.Size(153, 25)
        Me.dtpDailyFrom.TabIndex = 3
        '
        'btnDailyOutput
        '
        Me.btnDailyOutput.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDailyOutput.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDailyOutput.Location = New System.Drawing.Point(637, 54)
        Me.btnDailyOutput.Name = "btnDailyOutput"
        Me.btnDailyOutput.Size = New System.Drawing.Size(110, 25)
        Me.btnDailyOutput.TabIndex = 6
        Me.btnDailyOutput.Text = "ファイル出力"
        Me.btnDailyOutput.UseVisualStyleBackColor = True
        '
        'grpMonthly
        '
        Me.grpMonthly.Controls.Add(Me.dtpMonthlyTo)
        Me.grpMonthly.Controls.Add(Me.dtpMonthlyFrom)
        Me.grpMonthly.Controls.Add(Me.LblMonthlyLabel)
        Me.grpMonthly.Controls.Add(Me.LblMonthlySpecifyPeriod)
        Me.grpMonthly.Controls.Add(Me.btnMonthlyOutput)
        Me.grpMonthly.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.grpMonthly.Location = New System.Drawing.Point(108, 369)
        Me.grpMonthly.Name = "grpMonthly"
        Me.grpMonthly.Size = New System.Drawing.Size(780, 120)
        Me.grpMonthly.TabIndex = 7
        Me.grpMonthly.TabStop = False
        Me.grpMonthly.Text = "月単位出力"
        '
        'LblMonthlyLabel
        '
        Me.LblMonthlyLabel.AutoSize = True
        Me.LblMonthlyLabel.Location = New System.Drawing.Point(323, 54)
        Me.LblMonthlyLabel.Name = "LblMonthlyLabel"
        Me.LblMonthlyLabel.Size = New System.Drawing.Size(26, 18)
        Me.LblMonthlyLabel.TabIndex = 10
        Me.LblMonthlyLabel.Text = "～"
        '
        'LblMonthlySpecifyPeriod
        '
        Me.LblMonthlySpecifyPeriod.AutoSize = True
        Me.LblMonthlySpecifyPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.LblMonthlySpecifyPeriod.Location = New System.Drawing.Point(29, 54)
        Me.LblMonthlySpecifyPeriod.Name = "LblMonthlySpecifyPeriod"
        Me.LblMonthlySpecifyPeriod.Size = New System.Drawing.Size(80, 18)
        Me.LblMonthlySpecifyPeriod.TabIndex = 8
        Me.LblMonthlySpecifyPeriod.Text = "指定期間"
        '
        'btnMonthlyOutput
        '
        Me.btnMonthlyOutput.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnMonthlyOutput.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnMonthlyOutput.Location = New System.Drawing.Point(637, 51)
        Me.btnMonthlyOutput.Name = "btnMonthlyOutput"
        Me.btnMonthlyOutput.Size = New System.Drawing.Size(110, 25)
        Me.btnMonthlyOutput.TabIndex = 12
        Me.btnMonthlyOutput.Text = "ファイル出力"
        Me.btnMonthlyOutput.UseVisualStyleBackColor = True
        '
        'dtpMonthlyFrom
        '
        Me.dtpMonthlyFrom.CustomFormat = "yyyy年 MM月"
        Me.dtpMonthlyFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpMonthlyFrom.Location = New System.Drawing.Point(140, 48)
        Me.dtpMonthlyFrom.Name = "dtpMonthlyFrom"
        Me.dtpMonthlyFrom.Size = New System.Drawing.Size(153, 25)
        Me.dtpMonthlyFrom.TabIndex = 9
        '
        'dtpMonthlyTo
        '
        Me.dtpMonthlyTo.CustomFormat = "yyyy年 MM月"
        Me.dtpMonthlyTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpMonthlyTo.Location = New System.Drawing.Point(378, 48)
        Me.dtpMonthlyTo.Name = "dtpMonthlyTo"
        Me.dtpMonthlyTo.Size = New System.Drawing.Size(153, 25)
        Me.dtpMonthlyTo.TabIndex = 11
        '
        'UC050601
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpMonthly)
        Me.Controls.Add(Me.grpDaily)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC050601"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpDaily.ResumeLayout(False)
        Me.grpDaily.PerformLayout()
        Me.grpMonthly.ResumeLayout(False)
        Me.grpMonthly.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpDaily As System.Windows.Forms.GroupBox
    Friend WithEvents LblDailyLabel As System.Windows.Forms.Label
    Friend WithEvents dtpDailyTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents LblDailySpecifyPeriod As System.Windows.Forms.Label
    Friend WithEvents dtpDailyFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnDailyOutput As System.Windows.Forms.Button
    Friend WithEvents grpMonthly As System.Windows.Forms.GroupBox
    Friend WithEvents LblMonthlyLabel As System.Windows.Forms.Label
    Friend WithEvents LblMonthlySpecifyPeriod As System.Windows.Forms.Label
    Friend WithEvents btnMonthlyOutput As System.Windows.Forms.Button
    Friend WithEvents dtpMonthlyTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpMonthlyFrom As System.Windows.Forms.DateTimePicker

End Class
