<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040801
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
        Me.grpActionDate = New System.Windows.Forms.GroupBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.dtpActionEndDate = New System.Windows.Forms.DateTimePicker
        Me.Label1 = New System.Windows.Forms.Label
        Me.dtpActionBeginDate = New System.Windows.Forms.DateTimePicker
        Me.btnOutputActionList = New System.Windows.Forms.Button
        Me.grpRegistDate = New System.Windows.Forms.GroupBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.dtpRegistEndDate = New System.Windows.Forms.DateTimePicker
        Me.Label4 = New System.Windows.Forms.Label
        Me.dtpRegistBeginDate = New System.Windows.Forms.DateTimePicker
        Me.btnRegistList = New System.Windows.Forms.Button
        Me.grpActionDate.SuspendLayout()
        Me.grpRegistDate.SuspendLayout()
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
        Me.lblTitle.Text = "UP.UJデータ出力"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpActionDate
        '
        Me.grpActionDate.Controls.Add(Me.Label2)
        Me.grpActionDate.Controls.Add(Me.dtpActionEndDate)
        Me.grpActionDate.Controls.Add(Me.Label1)
        Me.grpActionDate.Controls.Add(Me.dtpActionBeginDate)
        Me.grpActionDate.Controls.Add(Me.btnOutputActionList)
        Me.grpActionDate.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.grpActionDate.Location = New System.Drawing.Point(125, 137)
        Me.grpActionDate.Name = "grpActionDate"
        Me.grpActionDate.Size = New System.Drawing.Size(777, 188)
        Me.grpActionDate.TabIndex = 13
        Me.grpActionDate.TabStop = False
        Me.grpActionDate.Text = "活動日検索"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(354, 83)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(26, 18)
        Me.Label2.TabIndex = 18
        Me.Label2.Text = "～"
        '
        'dtpActionEndDate
        '
        Me.dtpActionEndDate.Location = New System.Drawing.Point(409, 79)
        Me.dtpActionEndDate.Name = "dtpActionEndDate"
        Me.dtpActionEndDate.Size = New System.Drawing.Size(153, 25)
        Me.dtpActionEndDate.TabIndex = 17
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(38, 83)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 18)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "指定期間"
        '
        'dtpActionBeginDate
        '
        Me.dtpActionBeginDate.Location = New System.Drawing.Point(171, 78)
        Me.dtpActionBeginDate.Name = "dtpActionBeginDate"
        Me.dtpActionBeginDate.Size = New System.Drawing.Size(153, 25)
        Me.dtpActionBeginDate.TabIndex = 15
        '
        'btnOutputActionList
        '
        Me.btnOutputActionList.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOutputActionList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnOutputActionList.Location = New System.Drawing.Point(613, 79)
        Me.btnOutputActionList.Name = "btnOutputActionList"
        Me.btnOutputActionList.Size = New System.Drawing.Size(110, 25)
        Me.btnOutputActionList.TabIndex = 5
        Me.btnOutputActionList.Text = "ファイル出力"
        Me.btnOutputActionList.UseVisualStyleBackColor = True
        '
        'grpRegistDate
        '
        Me.grpRegistDate.Controls.Add(Me.Label3)
        Me.grpRegistDate.Controls.Add(Me.dtpRegistEndDate)
        Me.grpRegistDate.Controls.Add(Me.Label4)
        Me.grpRegistDate.Controls.Add(Me.dtpRegistBeginDate)
        Me.grpRegistDate.Controls.Add(Me.btnRegistList)
        Me.grpRegistDate.Font = New System.Drawing.Font("MS UI Gothic", 13.0!)
        Me.grpRegistDate.Location = New System.Drawing.Point(125, 428)
        Me.grpRegistDate.Name = "grpRegistDate"
        Me.grpRegistDate.Size = New System.Drawing.Size(777, 188)
        Me.grpRegistDate.TabIndex = 19
        Me.grpRegistDate.TabStop = False
        Me.grpRegistDate.Text = "登録日検索"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(354, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(26, 18)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "～"
        '
        'dtpRegistEndDate
        '
        Me.dtpRegistEndDate.Location = New System.Drawing.Point(409, 79)
        Me.dtpRegistEndDate.Name = "dtpRegistEndDate"
        Me.dtpRegistEndDate.Size = New System.Drawing.Size(153, 25)
        Me.dtpRegistEndDate.TabIndex = 17
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(38, 83)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(80, 18)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "指定期間"
        '
        'dtpRegistBeginDate
        '
        Me.dtpRegistBeginDate.Location = New System.Drawing.Point(171, 78)
        Me.dtpRegistBeginDate.Name = "dtpRegistBeginDate"
        Me.dtpRegistBeginDate.Size = New System.Drawing.Size(153, 25)
        Me.dtpRegistBeginDate.TabIndex = 15
        '
        'btnRegistList
        '
        Me.btnRegistList.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnRegistList.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnRegistList.Location = New System.Drawing.Point(613, 79)
        Me.btnRegistList.Name = "btnRegistList"
        Me.btnRegistList.Size = New System.Drawing.Size(110, 25)
        Me.btnRegistList.TabIndex = 5
        Me.btnRegistList.Text = "ファイル出力"
        Me.btnRegistList.UseVisualStyleBackColor = True
        '
        'UC040801
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpRegistDate)
        Me.Controls.Add(Me.grpActionDate)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC040801"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpActionDate.ResumeLayout(False)
        Me.grpActionDate.PerformLayout()
        Me.grpRegistDate.ResumeLayout(False)
        Me.grpRegistDate.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpActionDate As System.Windows.Forms.GroupBox
    Friend WithEvents btnOutputActionList As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpActionBeginDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents dtpActionEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents grpRegistDate As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents dtpRegistEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dtpRegistBeginDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnRegistList As System.Windows.Forms.Button

End Class
