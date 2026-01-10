<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC090114
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Label11 = New System.Windows.Forms.Label
        Me.btnInsert = New System.Windows.Forms.Button
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.gbxGridView1 = New System.Windows.Forms.GroupBox
        Me.cboToDate = New System.Windows.Forms.DateTimePicker
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnSetEnd = New System.Windows.Forms.Button
        Me.btnInsertHistory = New System.Windows.Forms.Button
        Me.btnShowDetail = New System.Windows.Forms.Button
        Me.DataGridView1 = New System.Windows.Forms.DataGridView
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.gbxGridView2 = New System.Windows.Forms.GroupBox
        Me.btnShowHistory = New System.Windows.Forms.Button
        Me.btnDelete = New System.Windows.Forms.Button
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.gbxGridView1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.gbxGridView2.SuspendLayout()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label11.Location = New System.Drawing.Point(200, 20)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(630, 35)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "昼食費マスタメンテナンス"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnInsert.Location = New System.Drawing.Point(718, 775)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(110, 30)
        Me.btnInsert.TabIndex = 1
        Me.btnInsert.Text = "新規登録"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.TabControl1.Location = New System.Drawing.Point(27, 63)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(980, 700)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.TabPage1.Controls.Add(Me.gbxGridView1)
        Me.TabPage1.Location = New System.Drawing.Point(4, 26)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(972, 670)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "一覧"
        '
        'gbxGridView1
        '
        Me.gbxGridView1.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.gbxGridView1.Controls.Add(Me.cboToDate)
        Me.gbxGridView1.Controls.Add(Me.Label8)
        Me.gbxGridView1.Controls.Add(Me.btnSetEnd)
        Me.gbxGridView1.Controls.Add(Me.btnInsertHistory)
        Me.gbxGridView1.Controls.Add(Me.btnShowDetail)
        Me.gbxGridView1.Controls.Add(Me.DataGridView1)
        Me.gbxGridView1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.gbxGridView1.ForeColor = System.Drawing.Color.Blue
        Me.gbxGridView1.Location = New System.Drawing.Point(16, 10)
        Me.gbxGridView1.Name = "gbxGridView1"
        Me.gbxGridView1.Size = New System.Drawing.Size(940, 646)
        Me.gbxGridView1.TabIndex = 0
        Me.gbxGridView1.TabStop = False
        Me.gbxGridView1.Text = "マスタデータ一覧"
        '
        'cboToDate
        '
        Me.cboToDate.CustomFormat = "yyyy年MM月"
        Me.cboToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.cboToDate.Location = New System.Drawing.Point(111, 614)
        Me.cboToDate.MaxDate = New Date(9998, 12, 1, 0, 0, 0, 0)
        Me.cboToDate.MinDate = New Date(1900, 1, 1, 0, 0, 0, 0)
        Me.cboToDate.Name = "cboToDate"
        Me.cboToDate.ShowUpDown = True
        Me.cboToDate.Size = New System.Drawing.Size(140, 23)
        Me.cboToDate.TabIndex = 2
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(17, 617)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 16)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "適用終了月"
        '
        'btnSetEnd
        '
        Me.btnSetEnd.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSetEnd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSetEnd.Location = New System.Drawing.Point(261, 610)
        Me.btnSetEnd.Name = "btnSetEnd"
        Me.btnSetEnd.Size = New System.Drawing.Size(110, 30)
        Me.btnSetEnd.TabIndex = 3
        Me.btnSetEnd.Text = "終了設定"
        Me.btnSetEnd.UseVisualStyleBackColor = True
        '
        'btnInsertHistory
        '
        Me.btnInsertHistory.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsertHistory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnInsertHistory.Location = New System.Drawing.Point(547, 610)
        Me.btnInsertHistory.Name = "btnInsertHistory"
        Me.btnInsertHistory.Size = New System.Drawing.Size(110, 30)
        Me.btnInsertHistory.TabIndex = 4
        Me.btnInsertHistory.Text = "履歴登録"
        Me.btnInsertHistory.UseVisualStyleBackColor = True
        '
        'btnShowDetail
        '
        Me.btnShowDetail.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnShowDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnShowDetail.Location = New System.Drawing.Point(671, 610)
        Me.btnShowDetail.Name = "btnShowDetail"
        Me.btnShowDetail.Size = New System.Drawing.Size(110, 30)
        Me.btnShowDetail.TabIndex = 5
        Me.btnShowDetail.Text = "詳細"
        Me.btnShowDetail.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridView1.Location = New System.Drawing.Point(20, 23)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.ReadOnly = True
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black
        Me.DataGridView1.RowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView1.RowTemplate.Height = 21
        Me.DataGridView1.Size = New System.Drawing.Size(900, 581)
        Me.DataGridView1.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.TabPage2.Controls.Add(Me.gbxGridView2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 26)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(972, 670)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "履歴検索"
        '
        'gbxGridView2
        '
        Me.gbxGridView2.Controls.Add(Me.btnShowHistory)
        Me.gbxGridView2.Controls.Add(Me.btnDelete)
        Me.gbxGridView2.Controls.Add(Me.DataGridView2)
        Me.gbxGridView2.ForeColor = System.Drawing.Color.Blue
        Me.gbxGridView2.Location = New System.Drawing.Point(16, 6)
        Me.gbxGridView2.Name = "gbxGridView2"
        Me.gbxGridView2.Size = New System.Drawing.Size(940, 648)
        Me.gbxGridView2.TabIndex = 2
        Me.gbxGridView2.TabStop = False
        Me.gbxGridView2.Text = "履歴一覧"
        '
        'btnShowHistory
        '
        Me.btnShowHistory.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnShowHistory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnShowHistory.Location = New System.Drawing.Point(671, 609)
        Me.btnShowHistory.Name = "btnShowHistory"
        Me.btnShowHistory.Size = New System.Drawing.Size(110, 30)
        Me.btnShowHistory.TabIndex = 6
        Me.btnShowHistory.Text = "詳細"
        Me.btnShowHistory.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDelete.Location = New System.Drawing.Point(545, 609)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(110, 30)
        Me.btnDelete.TabIndex = 5
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridView2.Location = New System.Drawing.Point(20, 23)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.ReadOnly = True
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black
        Me.DataGridView2.RowsDefaultCellStyle = DataGridViewCellStyle2
        Me.DataGridView2.RowTemplate.Height = 21
        Me.DataGridView2.Size = New System.Drawing.Size(900, 580)
        Me.DataGridView2.TabIndex = 0
        '
        'UC090114
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Label11)
        Me.Name = "UC090114"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.gbxGridView1.ResumeLayout(False)
        Me.gbxGridView1.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.gbxGridView2.ResumeLayout(False)
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnInsert As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents gbxGridView1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnSetEnd As System.Windows.Forms.Button
    Friend WithEvents btnInsertHistory As System.Windows.Forms.Button
    Friend WithEvents btnShowDetail As System.Windows.Forms.Button
    Friend WithEvents DataGridView1 As System.Windows.Forms.DataGridView
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents gbxGridView2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnShowHistory As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents cboToDate As System.Windows.Forms.DateTimePicker

End Class
