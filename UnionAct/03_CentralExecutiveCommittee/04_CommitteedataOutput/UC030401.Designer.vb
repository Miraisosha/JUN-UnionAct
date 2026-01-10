<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC030401
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
        Me.Officers = New System.Windows.Forms.CheckBox
        Me.dtpDate = New System.Windows.Forms.DateTimePicker
        Me.Label2 = New System.Windows.Forms.Label
        Me.btnCommitteeOutput = New System.Windows.Forms.Button
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
        Me.lblTitle.TabIndex = 10
        Me.lblTitle.Text = "委員会データ出力"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpJpnSyllabary
        '
        Me.grpJpnSyllabary.Controls.Add(Me.Officers)
        Me.grpJpnSyllabary.Controls.Add(Me.dtpDate)
        Me.grpJpnSyllabary.Controls.Add(Me.Label2)
        Me.grpJpnSyllabary.Controls.Add(Me.btnCommitteeOutput)
        Me.grpJpnSyllabary.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.grpJpnSyllabary.Location = New System.Drawing.Point(124, 316)
        Me.grpJpnSyllabary.Name = "grpJpnSyllabary"
        Me.grpJpnSyllabary.Size = New System.Drawing.Size(777, 188)
        Me.grpJpnSyllabary.TabIndex = 16
        Me.grpJpnSyllabary.TabStop = False
        Me.grpJpnSyllabary.Text = "部/委員会出力"
        '
        'Officers
        '
        Me.Officers.AutoSize = True
        Me.Officers.Location = New System.Drawing.Point(420, 93)
        Me.Officers.Name = "Officers"
        Me.Officers.Size = New System.Drawing.Size(94, 22)
        Me.Officers.TabIndex = 16
        Me.Officers.Text = "役員のみ"
        Me.Officers.UseVisualStyleBackColor = True
        '
        'dtpDate
        '
        Me.dtpDate.CalendarFont = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpDate.Location = New System.Drawing.Point(168, 90)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(204, 25)
        Me.dtpDate.TabIndex = 15
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label2.Location = New System.Drawing.Point(74, 95)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 18)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "基準日"
        '
        'btnCommitteeOutput
        '
        Me.btnCommitteeOutput.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCommitteeOutput.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCommitteeOutput.Location = New System.Drawing.Point(630, 90)
        Me.btnCommitteeOutput.Name = "btnCommitteeOutput"
        Me.btnCommitteeOutput.Size = New System.Drawing.Size(110, 30)
        Me.btnCommitteeOutput.TabIndex = 5
        Me.btnCommitteeOutput.Text = "ファイル出力"
        Me.btnCommitteeOutput.UseVisualStyleBackColor = True
        '
        'UC030401
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpJpnSyllabary)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC030401"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpJpnSyllabary.ResumeLayout(False)
        Me.grpJpnSyllabary.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpJpnSyllabary As System.Windows.Forms.GroupBox
    Friend WithEvents Officers As System.Windows.Forms.CheckBox
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents btnCommitteeOutput As System.Windows.Forms.Button

End Class
