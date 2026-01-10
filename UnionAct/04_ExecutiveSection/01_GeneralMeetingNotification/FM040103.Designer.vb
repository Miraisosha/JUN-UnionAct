<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM040103
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.grpSelect = New System.Windows.Forms.GroupBox
        Me.cboApplyArea = New System.Windows.Forms.ComboBox
        Me.lblApplyArea = New System.Windows.Forms.Label
        Me.cboKind = New System.Windows.Forms.ComboBox
        Me.lblIndispensableApplyArea = New System.Windows.Forms.Label
        Me.lblKind = New System.Windows.Forms.Label
        Me.lblIndispensableKind = New System.Windows.Forms.Label
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.dgvResult = New System.Windows.Forms.DataGridView
        Me.UserStatus = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Local = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Join = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.From = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.grpInsert = New System.Windows.Forms.GroupBox
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.chkInsert = New System.Windows.Forms.CheckBox
        Me.lblStartDate = New System.Windows.Forms.Label
        Me.btnCancel2 = New System.Windows.Forms.Button
        Me.btnOK2 = New System.Windows.Forms.Button
        Me.btnCancel1 = New System.Windows.Forms.Button
        Me.btnOK1 = New System.Windows.Forms.Button
        Me.grpSelect.SuspendLayout()
        Me.grpResult.SuspendLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpInsert.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpSelect
        '
        Me.grpSelect.Controls.Add(Me.cboApplyArea)
        Me.grpSelect.Controls.Add(Me.lblApplyArea)
        Me.grpSelect.Controls.Add(Me.cboKind)
        Me.grpSelect.Controls.Add(Me.lblIndispensableApplyArea)
        Me.grpSelect.Controls.Add(Me.lblKind)
        Me.grpSelect.Controls.Add(Me.lblIndispensableKind)
        Me.grpSelect.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSelect.Location = New System.Drawing.Point(47, 12)
        Me.grpSelect.Name = "grpSelect"
        Me.grpSelect.Size = New System.Drawing.Size(502, 81)
        Me.grpSelect.TabIndex = 0
        Me.grpSelect.TabStop = False
        Me.grpSelect.Text = "登録を行う種別・支部の選択"
        '
        'cboApplyArea
        '
        Me.cboApplyArea.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboApplyArea.FormattingEnabled = True
        Me.cboApplyArea.ItemHeight = 16
        Me.cboApplyArea.Location = New System.Drawing.Point(337, 30)
        Me.cboApplyArea.Name = "cboApplyArea"
        Me.cboApplyArea.Size = New System.Drawing.Size(128, 24)
        Me.cboApplyArea.TabIndex = 6
        '
        'lblApplyArea
        '
        Me.lblApplyArea.AutoSize = True
        Me.lblApplyArea.Location = New System.Drawing.Point(291, 34)
        Me.lblApplyArea.Name = "lblApplyArea"
        Me.lblApplyArea.Size = New System.Drawing.Size(40, 16)
        Me.lblApplyArea.TabIndex = 5
        Me.lblApplyArea.Text = "支部"
        '
        'cboKind
        '
        Me.cboKind.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboKind.FormattingEnabled = True
        Me.cboKind.Location = New System.Drawing.Point(97, 30)
        Me.cboKind.Name = "cboKind"
        Me.cboKind.Size = New System.Drawing.Size(117, 24)
        Me.cboKind.TabIndex = 3
        '
        'lblIndispensableApplyArea
        '
        Me.lblIndispensableApplyArea.AutoSize = True
        Me.lblIndispensableApplyArea.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableApplyArea.Location = New System.Drawing.Point(273, 34)
        Me.lblIndispensableApplyArea.Name = "lblIndispensableApplyArea"
        Me.lblIndispensableApplyArea.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableApplyArea.TabIndex = 4
        Me.lblIndispensableApplyArea.Text = "*"
        '
        'lblKind
        '
        Me.lblKind.AutoSize = True
        Me.lblKind.Location = New System.Drawing.Point(51, 34)
        Me.lblKind.Name = "lblKind"
        Me.lblKind.Size = New System.Drawing.Size(40, 16)
        Me.lblKind.TabIndex = 2
        Me.lblKind.Text = "種別"
        '
        'lblIndispensableKind
        '
        Me.lblIndispensableKind.AutoSize = True
        Me.lblIndispensableKind.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableKind.Location = New System.Drawing.Point(33, 34)
        Me.lblIndispensableKind.Name = "lblIndispensableKind"
        Me.lblIndispensableKind.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableKind.TabIndex = 1
        Me.lblIndispensableKind.Text = "*"
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.dgvResult)
        Me.grpResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpResult.Location = New System.Drawing.Point(47, 186)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(502, 257)
        Me.grpResult.TabIndex = 13
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "一覧から選択"
        '
        'dgvResult
        '
        Me.dgvResult.AllowUserToAddRows = False
        Me.dgvResult.AllowUserToResizeColumns = False
        Me.dgvResult.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResult.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvResult.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.UserStatus, Me.Local, Me.Join, Me.From})
        Me.dgvResult.Location = New System.Drawing.Point(8, 22)
        Me.dgvResult.MultiSelect = False
        Me.dgvResult.Name = "dgvResult"
        Me.dgvResult.ReadOnly = True
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResult.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgvResult.RowHeadersVisible = False
        Me.dgvResult.RowTemplate.Height = 21
        Me.dgvResult.Size = New System.Drawing.Size(488, 225)
        Me.dgvResult.TabIndex = 14
        '
        'UserStatus
        '
        Me.UserStatus.HeaderText = "通知番号"
        Me.UserStatus.Name = "UserStatus"
        Me.UserStatus.ReadOnly = True
        '
        'Local
        '
        Me.Local.HeaderText = "開催開始日付"
        Me.Local.Name = "Local"
        Me.Local.ReadOnly = True
        Me.Local.Width = 120
        '
        'Join
        '
        Me.Join.HeaderText = "会議場所"
        Me.Join.Name = "Join"
        Me.Join.ReadOnly = True
        Me.Join.Width = 220
        '
        'From
        '
        Me.From.HeaderText = "登録日"
        Me.From.Name = "From"
        Me.From.ReadOnly = True
        '
        'grpInsert
        '
        Me.grpInsert.Controls.Add(Me.dtpStartDate)
        Me.grpInsert.Controls.Add(Me.chkInsert)
        Me.grpInsert.Controls.Add(Me.lblStartDate)
        Me.grpInsert.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpInsert.Location = New System.Drawing.Point(47, 108)
        Me.grpInsert.Name = "grpInsert"
        Me.grpInsert.Size = New System.Drawing.Size(502, 63)
        Me.grpInsert.TabIndex = 9
        Me.grpInsert.TabStop = False
        Me.grpInsert.Text = "新規申請"
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Location = New System.Drawing.Point(319, 26)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(151, 23)
        Me.dtpStartDate.TabIndex = 12
        '
        'chkInsert
        '
        Me.chkInsert.AutoSize = True
        Me.chkInsert.Location = New System.Drawing.Point(36, 29)
        Me.chkInsert.Name = "chkInsert"
        Me.chkInsert.Size = New System.Drawing.Size(91, 20)
        Me.chkInsert.TabIndex = 10
        Me.chkInsert.Text = "新規登録"
        Me.chkInsert.UseVisualStyleBackColor = True
        '
        'lblStartDate
        '
        Me.lblStartDate.AutoSize = True
        Me.lblStartDate.Location = New System.Drawing.Point(189, 30)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(104, 16)
        Me.lblStartDate.TabIndex = 11
        Me.lblStartDate.Text = "開催開始日付"
        '
        'btnCancel2
        '
        Me.btnCancel2.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel2.Location = New System.Drawing.Point(310, 459)
        Me.btnCancel2.Name = "btnCancel2"
        Me.btnCancel2.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel2.TabIndex = 16
        Me.btnCancel2.Text = "キャンセル"
        Me.btnCancel2.UseVisualStyleBackColor = True
        '
        'btnOK2
        '
        Me.btnOK2.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOK2.Location = New System.Drawing.Point(180, 459)
        Me.btnOK2.Name = "btnOK2"
        Me.btnOK2.Size = New System.Drawing.Size(110, 30)
        Me.btnOK2.TabIndex = 15
        Me.btnOK2.Text = "OK"
        Me.btnOK2.UseVisualStyleBackColor = True
        '
        'btnCancel1
        '
        Me.btnCancel1.Font = New System.Drawing.Font("MS UI Gothic", 11.0!)
        Me.btnCancel1.Location = New System.Drawing.Point(310, 108)
        Me.btnCancel1.Name = "btnCancel1"
        Me.btnCancel1.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel1.TabIndex = 8
        Me.btnCancel1.Text = "キャンセル"
        Me.btnCancel1.UseVisualStyleBackColor = True
        '
        'btnOK1
        '
        Me.btnOK1.Font = New System.Drawing.Font("MS UI Gothic", 11.0!)
        Me.btnOK1.Location = New System.Drawing.Point(180, 108)
        Me.btnOK1.Name = "btnOK1"
        Me.btnOK1.Size = New System.Drawing.Size(110, 30)
        Me.btnOK1.TabIndex = 7
        Me.btnOK1.Text = "OK"
        Me.btnOK1.UseVisualStyleBackColor = True
        '
        'FM040103
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(594, 508)
        Me.Controls.Add(Me.btnCancel1)
        Me.Controls.Add(Me.btnOK1)
        Me.Controls.Add(Me.btnCancel2)
        Me.Controls.Add(Me.btnOK2)
        Me.Controls.Add(Me.grpInsert)
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.grpSelect)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "FM040103"
        Me.Text = "開催登録"
        Me.grpSelect.ResumeLayout(False)
        Me.grpSelect.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpInsert.ResumeLayout(False)
        Me.grpInsert.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSelect As System.Windows.Forms.GroupBox
    Friend WithEvents cboApplyArea As System.Windows.Forms.ComboBox
    Friend WithEvents lblApplyArea As System.Windows.Forms.Label
    Friend WithEvents cboKind As System.Windows.Forms.ComboBox
    Friend WithEvents lblIndispensableApplyArea As System.Windows.Forms.Label
    Friend WithEvents lblKind As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableKind As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents grpInsert As System.Windows.Forms.GroupBox
    Friend WithEvents lblStartDate As System.Windows.Forms.Label
    Friend WithEvents btnCancel2 As System.Windows.Forms.Button
    Friend WithEvents btnOK2 As System.Windows.Forms.Button
    Friend WithEvents chkInsert As System.Windows.Forms.CheckBox
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dgvResult As System.Windows.Forms.DataGridView
    Friend WithEvents UserStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Local As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Join As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents From As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnOK1 As System.Windows.Forms.Button
    Friend WithEvents btnCancel1 As System.Windows.Forms.Button
End Class
