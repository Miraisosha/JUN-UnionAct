<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM040604
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
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnInsertDb = New System.Windows.Forms.Button
        Me.grpDocumentNo = New System.Windows.Forms.GroupBox
        Me.btnDocNumber = New System.Windows.Forms.Button
        Me.grpIssueDate = New System.Windows.Forms.GroupBox
        Me.dtpIssueDate = New System.Windows.Forms.DateTimePicker
        Me.chkWareki = New System.Windows.Forms.CheckBox
        Me.btnApply = New System.Windows.Forms.Button
        Me.grpDocumentNo.SuspendLayout()
        Me.grpIssueDate.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBack.Location = New System.Drawing.Point(174, 188)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(120, 28)
        Me.btnBack.TabIndex = 7
        Me.btnBack.Text = "総合OAに戻る"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnInsertDb
        '
        Me.btnInsertDb.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsertDb.Location = New System.Drawing.Point(41, 188)
        Me.btnInsertDb.Name = "btnInsertDb"
        Me.btnInsertDb.Size = New System.Drawing.Size(109, 28)
        Me.btnInsertDb.TabIndex = 6
        Me.btnInsertDb.Text = "DB登録"
        Me.btnInsertDb.UseVisualStyleBackColor = True
        '
        'grpDocumentNo
        '
        Me.grpDocumentNo.Controls.Add(Me.btnDocNumber)
        Me.grpDocumentNo.ForeColor = System.Drawing.Color.Blue
        Me.grpDocumentNo.Location = New System.Drawing.Point(22, 12)
        Me.grpDocumentNo.Name = "grpDocumentNo"
        Me.grpDocumentNo.Size = New System.Drawing.Size(292, 60)
        Me.grpDocumentNo.TabIndex = 0
        Me.grpDocumentNo.TabStop = False
        Me.grpDocumentNo.Text = "文書番号"
        '
        'btnDocNumber
        '
        Me.btnDocNumber.Font = New System.Drawing.Font("MS UI Gothic", 11.25!)
        Me.btnDocNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDocNumber.Location = New System.Drawing.Point(76, 19)
        Me.btnDocNumber.Name = "btnDocNumber"
        Me.btnDocNumber.Size = New System.Drawing.Size(120, 28)
        Me.btnDocNumber.TabIndex = 1
        Me.btnDocNumber.Text = "文書番号の採番"
        Me.btnDocNumber.UseVisualStyleBackColor = True
        '
        'grpIssueDate
        '
        Me.grpIssueDate.Controls.Add(Me.dtpIssueDate)
        Me.grpIssueDate.Controls.Add(Me.chkWareki)
        Me.grpIssueDate.Controls.Add(Me.btnApply)
        Me.grpIssueDate.ForeColor = System.Drawing.Color.Blue
        Me.grpIssueDate.Location = New System.Drawing.Point(22, 78)
        Me.grpIssueDate.Name = "grpIssueDate"
        Me.grpIssueDate.Size = New System.Drawing.Size(292, 93)
        Me.grpIssueDate.TabIndex = 2
        Me.grpIssueDate.TabStop = False
        Me.grpIssueDate.Text = "発行日"
        '
        'dtpIssueDate
        '
        Me.dtpIssueDate.CustomFormat = "yyyy/MM/dd"
        Me.dtpIssueDate.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpIssueDate.Location = New System.Drawing.Point(9, 18)
        Me.dtpIssueDate.Name = "dtpIssueDate"
        Me.dtpIssueDate.Size = New System.Drawing.Size(177, 22)
        Me.dtpIssueDate.TabIndex = 3
        '
        'chkWareki
        '
        Me.chkWareki.AutoSize = True
        Me.chkWareki.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWareki.Location = New System.Drawing.Point(206, 20)
        Me.chkWareki.Name = "chkWareki"
        Me.chkWareki.Size = New System.Drawing.Size(48, 16)
        Me.chkWareki.TabIndex = 4
        Me.chkWareki.Text = "和暦"
        Me.chkWareki.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Font = New System.Drawing.Font("MS UI Gothic", 11.25!)
        Me.btnApply.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnApply.Location = New System.Drawing.Point(98, 54)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(79, 28)
        Me.btnApply.TabIndex = 5
        Me.btnApply.Text = "適用"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'FM040604
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(336, 238)
        Me.Controls.Add(Me.grpIssueDate)
        Me.Controls.Add(Me.grpDocumentNo)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnInsertDb)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM040604"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "文書操作ウィンドウ"
        Me.grpDocumentNo.ResumeLayout(False)
        Me.grpIssueDate.ResumeLayout(False)
        Me.grpIssueDate.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnInsertDb As System.Windows.Forms.Button
    Friend WithEvents grpDocumentNo As System.Windows.Forms.GroupBox
    Friend WithEvents btnDocNumber As System.Windows.Forms.Button
    Friend WithEvents grpIssueDate As System.Windows.Forms.GroupBox
    Friend WithEvents dtpIssueDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkWareki As System.Windows.Forms.CheckBox
    Friend WithEvents btnApply As System.Windows.Forms.Button

End Class
