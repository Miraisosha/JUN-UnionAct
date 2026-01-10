<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC020501
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
        Me.grpPrintCondition = New System.Windows.Forms.GroupBox
        Me.GroupBox3 = New System.Windows.Forms.GroupBox
        Me.optStaffId_C = New System.Windows.Forms.RadioButton
        Me.btnPrintStaffId = New System.Windows.Forms.Button
        Me.optStaffId_B = New System.Windows.Forms.RadioButton
        Me.optStaffId_A = New System.Windows.Forms.RadioButton
        Me.grpJpnSyllabary = New System.Windows.Forms.GroupBox
        Me.optJpnSyllabary_C = New System.Windows.Forms.RadioButton
        Me.optJpnSyllabary_B = New System.Windows.Forms.RadioButton
        Me.btnPrintJpnSyllabary = New System.Windows.Forms.Button
        Me.optJpnSyllabary_A = New System.Windows.Forms.RadioButton
        Me.cboUnionBranch = New System.Windows.Forms.ComboBox
        Me.lblUnionBranch = New System.Windows.Forms.Label
        Me.grpPrintCondition.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
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
        Me.lblTitle.TabIndex = 6
        Me.lblTitle.Text = "定型名簿"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpPrintCondition
        '
        Me.grpPrintCondition.Controls.Add(Me.GroupBox3)
        Me.grpPrintCondition.Controls.Add(Me.grpJpnSyllabary)
        Me.grpPrintCondition.Controls.Add(Me.cboUnionBranch)
        Me.grpPrintCondition.Controls.Add(Me.lblUnionBranch)
        Me.grpPrintCondition.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpPrintCondition.ForeColor = System.Drawing.Color.Blue
        Me.grpPrintCondition.Location = New System.Drawing.Point(50, 121)
        Me.grpPrintCondition.Name = "grpPrintCondition"
        Me.grpPrintCondition.Size = New System.Drawing.Size(891, 587)
        Me.grpPrintCondition.TabIndex = 9
        Me.grpPrintCondition.TabStop = False
        Me.grpPrintCondition.Text = "印刷条件"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.optStaffId_C)
        Me.GroupBox3.Controls.Add(Me.btnPrintStaffId)
        Me.GroupBox3.Controls.Add(Me.optStaffId_B)
        Me.GroupBox3.Controls.Add(Me.optStaffId_A)
        Me.GroupBox3.Location = New System.Drawing.Point(70, 355)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(777, 119)
        Me.GroupBox3.TabIndex = 21
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "社員番号順定型名簿出力"
        '
        'optStaffId_C
        '
        Me.optStaffId_C.AutoSize = True
        Me.optStaffId_C.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optStaffId_C.Location = New System.Drawing.Point(414, 59)
        Me.optStaffId_C.Name = "optStaffId_C"
        Me.optStaffId_C.Size = New System.Drawing.Size(91, 20)
        Me.optStaffId_C.TabIndex = 8
        Me.optStaffId_C.TabStop = True
        Me.optStaffId_C.Text = "社番順 Ｃ"
        Me.optStaffId_C.UseVisualStyleBackColor = True
        '
        'btnPrintStaffId
        '
        Me.btnPrintStaffId.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrintStaffId.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPrintStaffId.Location = New System.Drawing.Point(630, 53)
        Me.btnPrintStaffId.Name = "btnPrintStaffId"
        Me.btnPrintStaffId.Size = New System.Drawing.Size(110, 30)
        Me.btnPrintStaffId.TabIndex = 9
        Me.btnPrintStaffId.Text = "プレ印刷"
        Me.btnPrintStaffId.UseVisualStyleBackColor = True
        '
        'optStaffId_B
        '
        Me.optStaffId_B.AutoSize = True
        Me.optStaffId_B.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optStaffId_B.Location = New System.Drawing.Point(251, 59)
        Me.optStaffId_B.Name = "optStaffId_B"
        Me.optStaffId_B.Size = New System.Drawing.Size(97, 20)
        Me.optStaffId_B.TabIndex = 7
        Me.optStaffId_B.TabStop = True
        Me.optStaffId_B.Text = "社番順　Ｂ"
        Me.optStaffId_B.UseVisualStyleBackColor = True
        '
        'optStaffId_A
        '
        Me.optStaffId_A.AutoSize = True
        Me.optStaffId_A.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optStaffId_A.Location = New System.Drawing.Point(79, 59)
        Me.optStaffId_A.Name = "optStaffId_A"
        Me.optStaffId_A.Size = New System.Drawing.Size(90, 20)
        Me.optStaffId_A.TabIndex = 6
        Me.optStaffId_A.TabStop = True
        Me.optStaffId_A.Text = "社番順 Ａ"
        Me.optStaffId_A.UseVisualStyleBackColor = True
        '
        'grpJpnSyllabary
        '
        Me.grpJpnSyllabary.Controls.Add(Me.optJpnSyllabary_C)
        Me.grpJpnSyllabary.Controls.Add(Me.optJpnSyllabary_B)
        Me.grpJpnSyllabary.Controls.Add(Me.btnPrintJpnSyllabary)
        Me.grpJpnSyllabary.Controls.Add(Me.optJpnSyllabary_A)
        Me.grpJpnSyllabary.Location = New System.Drawing.Point(70, 173)
        Me.grpJpnSyllabary.Name = "grpJpnSyllabary"
        Me.grpJpnSyllabary.Size = New System.Drawing.Size(777, 119)
        Me.grpJpnSyllabary.TabIndex = 12
        Me.grpJpnSyllabary.TabStop = False
        Me.grpJpnSyllabary.Text = "50音順定型名簿出力"
        '
        'optJpnSyllabary_C
        '
        Me.optJpnSyllabary_C.AutoSize = True
        Me.optJpnSyllabary_C.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optJpnSyllabary_C.Location = New System.Drawing.Point(414, 56)
        Me.optJpnSyllabary_C.Name = "optJpnSyllabary_C"
        Me.optJpnSyllabary_C.Size = New System.Drawing.Size(91, 20)
        Me.optJpnSyllabary_C.TabIndex = 4
        Me.optJpnSyllabary_C.TabStop = True
        Me.optJpnSyllabary_C.Text = "50音順 Ｃ"
        Me.optJpnSyllabary_C.UseVisualStyleBackColor = True
        '
        'optJpnSyllabary_B
        '
        Me.optJpnSyllabary_B.AutoSize = True
        Me.optJpnSyllabary_B.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optJpnSyllabary_B.Location = New System.Drawing.Point(251, 56)
        Me.optJpnSyllabary_B.Name = "optJpnSyllabary_B"
        Me.optJpnSyllabary_B.Size = New System.Drawing.Size(91, 20)
        Me.optJpnSyllabary_B.TabIndex = 3
        Me.optJpnSyllabary_B.TabStop = True
        Me.optJpnSyllabary_B.Text = "50音順 Ｂ"
        Me.optJpnSyllabary_B.UseVisualStyleBackColor = True
        '
        'btnPrintJpnSyllabary
        '
        Me.btnPrintJpnSyllabary.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrintJpnSyllabary.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPrintJpnSyllabary.Location = New System.Drawing.Point(630, 54)
        Me.btnPrintJpnSyllabary.Name = "btnPrintJpnSyllabary"
        Me.btnPrintJpnSyllabary.Size = New System.Drawing.Size(110, 30)
        Me.btnPrintJpnSyllabary.TabIndex = 5
        Me.btnPrintJpnSyllabary.Text = "プレ印刷"
        Me.btnPrintJpnSyllabary.UseVisualStyleBackColor = True
        '
        'optJpnSyllabary_A
        '
        Me.optJpnSyllabary_A.AutoSize = True
        Me.optJpnSyllabary_A.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optJpnSyllabary_A.Location = New System.Drawing.Point(79, 56)
        Me.optJpnSyllabary_A.Name = "optJpnSyllabary_A"
        Me.optJpnSyllabary_A.Size = New System.Drawing.Size(90, 20)
        Me.optJpnSyllabary_A.TabIndex = 2
        Me.optJpnSyllabary_A.TabStop = True
        Me.optJpnSyllabary_A.Text = "50音順 Ａ"
        Me.optJpnSyllabary_A.UseVisualStyleBackColor = True
        '
        'cboUnionBranch
        '
        Me.cboUnionBranch.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboUnionBranch.FormattingEnabled = True
        Me.cboUnionBranch.Location = New System.Drawing.Point(119, 52)
        Me.cboUnionBranch.Name = "cboUnionBranch"
        Me.cboUnionBranch.Size = New System.Drawing.Size(177, 24)
        Me.cboUnionBranch.TabIndex = 1
        '
        'lblUnionBranch
        '
        Me.lblUnionBranch.AutoSize = True
        Me.lblUnionBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnionBranch.Location = New System.Drawing.Point(38, 55)
        Me.lblUnionBranch.Name = "lblUnionBranch"
        Me.lblUnionBranch.Size = New System.Drawing.Size(72, 16)
        Me.lblUnionBranch.TabIndex = 1
        Me.lblUnionBranch.Text = "組合支部"
        '
        'UC020501
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpPrintCondition)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC020501"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpPrintCondition.ResumeLayout(False)
        Me.grpPrintCondition.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.grpJpnSyllabary.ResumeLayout(False)
        Me.grpJpnSyllabary.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpPrintCondition As System.Windows.Forms.GroupBox
    Friend WithEvents grpJpnSyllabary As System.Windows.Forms.GroupBox
    Friend WithEvents cboUnionBranch As System.Windows.Forms.ComboBox
    Friend WithEvents lblUnionBranch As System.Windows.Forms.Label
    Friend WithEvents btnPrintJpnSyllabary As System.Windows.Forms.Button
    Friend WithEvents optJpnSyllabary_A As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents optStaffId_C As System.Windows.Forms.RadioButton
    Friend WithEvents btnPrintStaffId As System.Windows.Forms.Button
    Friend WithEvents optStaffId_B As System.Windows.Forms.RadioButton
    Friend WithEvents optStaffId_A As System.Windows.Forms.RadioButton
    Friend WithEvents optJpnSyllabary_C As System.Windows.Forms.RadioButton
    Friend WithEvents optJpnSyllabary_B As System.Windows.Forms.RadioButton

End Class
