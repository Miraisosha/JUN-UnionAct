<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM040602
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
        Me.cboSubject = New System.Windows.Forms.ComboBox
        Me.lblDocCode = New System.Windows.Forms.Label
        Me.lblPeriod = New System.Windows.Forms.Label
        Me.lblSubject = New System.Windows.Forms.Label
        Me.lblApplyDate = New System.Windows.Forms.Label
        Me.lblSubjectManual = New System.Windows.Forms.Label
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.lblKara = New System.Windows.Forms.Label
        Me.dtpTo = New System.Windows.Forms.DateTimePicker
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker
        Me.lblTo = New System.Windows.Forms.Label
        Me.lblFrom = New System.Windows.Forms.Label
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.dgvResult = New System.Windows.Forms.DataGridView
        Me.col_check = New System.Windows.Forms.DataGridViewCheckBoxColumn
        Me.commitee_name = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.apply_year_month = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.update_date = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.update_user = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.condition = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.committee_update_id = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.doc_out = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.committee_id = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.update_user_id = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnAllNoCheck = New System.Windows.Forms.Button
        Me.btnAllCheck = New System.Windows.Forms.Button
        Me.btnSelectCancel = New System.Windows.Forms.Button
        Me.btnCreate = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.cboDocCode = New System.Windows.Forms.ComboBox
        Me.cboPeriod = New System.Windows.Forms.ComboBox
        Me.lblIndispensableMemberNo = New System.Windows.Forms.Label
        Me.lblIndispensableDocCode = New System.Windows.Forms.Label
        Me.lblIndispensableSubject = New System.Windows.Forms.Label
        Me.lblIndispensableApplyDate = New System.Windows.Forms.Label
        Me.dtpApplyDate = New System.Windows.Forms.DateTimePicker
        Me.txtSubjectManual = New System.Windows.Forms.TextBox
        Me.grpSearch.SuspendLayout()
        Me.grpResult.SuspendLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cboSubject
        '
        Me.cboSubject.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboSubject.FormattingEnabled = True
        Me.cboSubject.Location = New System.Drawing.Point(239, 93)
        Me.cboSubject.Name = "cboSubject"
        Me.cboSubject.Size = New System.Drawing.Size(482, 24)
        Me.cboSubject.TabIndex = 11
        '
        'lblDocCode
        '
        Me.lblDocCode.AutoSize = True
        Me.lblDocCode.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblDocCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocCode.Location = New System.Drawing.Point(137, 55)
        Me.lblDocCode.Name = "lblDocCode"
        Me.lblDocCode.Size = New System.Drawing.Size(76, 16)
        Me.lblDocCode.TabIndex = 7
        Me.lblDocCode.Text = "管理コード"
        '
        'lblPeriod
        '
        Me.lblPeriod.AutoSize = True
        Me.lblPeriod.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriod.Location = New System.Drawing.Point(138, 15)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.Size = New System.Drawing.Size(24, 16)
        Me.lblPeriod.TabIndex = 1
        Me.lblPeriod.Text = "期"
        '
        'lblSubject
        '
        Me.lblSubject.AutoSize = True
        Me.lblSubject.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSubject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubject.Location = New System.Drawing.Point(137, 96)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.Size = New System.Drawing.Size(40, 16)
        Me.lblSubject.TabIndex = 10
        Me.lblSubject.Text = "標題"
        '
        'lblApplyDate
        '
        Me.lblApplyDate.AutoSize = True
        Me.lblApplyDate.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblApplyDate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblApplyDate.Location = New System.Drawing.Point(507, 15)
        Me.lblApplyDate.Name = "lblApplyDate"
        Me.lblApplyDate.Size = New System.Drawing.Size(72, 16)
        Me.lblApplyDate.TabIndex = 4
        Me.lblApplyDate.Text = "適用日付"
        Me.lblApplyDate.Visible = False
        '
        'lblSubjectManual
        '
        Me.lblSubjectManual.AutoSize = True
        Me.lblSubjectManual.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSubjectManual.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubjectManual.Location = New System.Drawing.Point(109, 137)
        Me.lblSubjectManual.Name = "lblSubjectManual"
        Me.lblSubjectManual.Size = New System.Drawing.Size(104, 16)
        Me.lblSubjectManual.TabIndex = 12
        Me.lblSubjectManual.Text = "（任意入力欄）"
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.lblKara)
        Me.grpSearch.Controls.Add(Me.dtpTo)
        Me.grpSearch.Controls.Add(Me.dtpFrom)
        Me.grpSearch.Controls.Add(Me.lblTo)
        Me.grpSearch.Controls.Add(Me.lblFrom)
        Me.grpSearch.Enabled = False
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.Location = New System.Drawing.Point(100, 170)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(653, 100)
        Me.grpSearch.TabIndex = 14
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "【委員会名簿】登録年月日"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(530, 50)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 21
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'lblKara
        '
        Me.lblKara.AutoSize = True
        Me.lblKara.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblKara.Location = New System.Drawing.Point(242, 57)
        Me.lblKara.Name = "lblKara"
        Me.lblKara.Size = New System.Drawing.Size(24, 16)
        Me.lblKara.TabIndex = 18
        Me.lblKara.Text = "～"
        '
        'dtpTo
        '
        Me.dtpTo.Location = New System.Drawing.Point(298, 54)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(142, 23)
        Me.dtpTo.TabIndex = 20
        '
        'dtpFrom
        '
        Me.dtpFrom.Location = New System.Drawing.Point(77, 54)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(136, 23)
        Me.dtpFrom.TabIndex = 17
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTo.Location = New System.Drawing.Point(396, 31)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(44, 16)
        Me.lblTo.TabIndex = 19
        Me.lblTo.Text = "（TO）"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFrom.Location = New System.Drawing.Point(147, 31)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(66, 16)
        Me.lblFrom.TabIndex = 15
        Me.lblFrom.Text = "（FROM）"
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.dgvResult)
        Me.grpResult.Controls.Add(Me.btnDelete)
        Me.grpResult.Controls.Add(Me.btnAllNoCheck)
        Me.grpResult.Controls.Add(Me.btnAllCheck)
        Me.grpResult.Controls.Add(Me.btnSelectCancel)
        Me.grpResult.Enabled = False
        Me.grpResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpResult.Location = New System.Drawing.Point(45, 285)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(752, 396)
        Me.grpResult.TabIndex = 22
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "追加・削除データ一覧"
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
        Me.dgvResult.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.col_check, Me.commitee_name, Me.apply_year_month, Me.update_date, Me.update_user, Me.condition, Me.committee_update_id, Me.doc_out, Me.committee_id, Me.update_user_id})
        Me.dgvResult.Location = New System.Drawing.Point(21, 32)
        Me.dgvResult.Name = "dgvResult"
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
        Me.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResult.Size = New System.Drawing.Size(712, 321)
        Me.dgvResult.TabIndex = 28
        '
        'col_check
        '
        Me.col_check.HeaderText = ""
        Me.col_check.Name = "col_check"
        Me.col_check.ReadOnly = True
        Me.col_check.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.col_check.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.col_check.Width = 45
        '
        'commitee_name
        '
        Me.commitee_name.HeaderText = "委員会名"
        Me.commitee_name.Name = "commitee_name"
        Me.commitee_name.Width = 200
        '
        'apply_year_month
        '
        Me.apply_year_month.HeaderText = "適用年月"
        Me.apply_year_month.Name = "apply_year_month"
        '
        'update_date
        '
        Me.update_date.HeaderText = "登録日時"
        Me.update_date.Name = "update_date"
        Me.update_date.Width = 150
        '
        'update_user
        '
        Me.update_user.HeaderText = "登録者"
        Me.update_user.Name = "update_user"
        Me.update_user.Width = 140
        '
        'condition
        '
        Me.condition.HeaderText = "状態"
        Me.condition.Name = "condition"
        Me.condition.Width = 80
        '
        'committee_update_id
        '
        Me.committee_update_id.HeaderText = "委員会変更ID"
        Me.committee_update_id.Name = "committee_update_id"
        Me.committee_update_id.Width = 50
        '
        'doc_out
        '
        Me.doc_out.HeaderText = "出力"
        Me.doc_out.Name = "doc_out"
        Me.doc_out.Width = 50
        '
        'committee_id
        '
        Me.committee_id.HeaderText = "委員会ID"
        Me.committee_id.Name = "committee_id"
        Me.committee_id.Width = 50
        '
        'update_user_id
        '
        Me.update_user_id.HeaderText = "変更者ID"
        Me.update_user_id.Name = "update_user_id"
        Me.update_user_id.Width = 50
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDelete.Location = New System.Drawing.Point(608, 359)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(110, 30)
        Me.btnDelete.TabIndex = 27
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnAllNoCheck
        '
        Me.btnAllNoCheck.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAllNoCheck.Location = New System.Drawing.Point(71, 360)
        Me.btnAllNoCheck.Name = "btnAllNoCheck"
        Me.btnAllNoCheck.Size = New System.Drawing.Size(30, 29)
        Me.btnAllNoCheck.TabIndex = 25
        Me.btnAllNoCheck.Text = "□"
        Me.btnAllNoCheck.UseVisualStyleBackColor = True
        '
        'btnAllCheck
        '
        Me.btnAllCheck.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAllCheck.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnAllCheck.Location = New System.Drawing.Point(35, 360)
        Me.btnAllCheck.Name = "btnAllCheck"
        Me.btnAllCheck.Size = New System.Drawing.Size(30, 30)
        Me.btnAllCheck.TabIndex = 24
        Me.btnAllCheck.Text = "☑"
        Me.btnAllCheck.UseVisualStyleBackColor = True
        '
        'btnSelectCancel
        '
        Me.btnSelectCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSelectCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSelectCancel.Location = New System.Drawing.Point(35, 359)
        Me.btnSelectCancel.Name = "btnSelectCancel"
        Me.btnSelectCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnSelectCancel.TabIndex = 26
        Me.btnSelectCancel.Text = "選択を解除"
        Me.btnSelectCancel.UseVisualStyleBackColor = True
        '
        'btnCreate
        '
        Me.btnCreate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCreate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCreate.Location = New System.Drawing.Point(276, 687)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(110, 30)
        Me.btnCreate.TabIndex = 28
        Me.btnCreate.Text = "作成"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancel.Location = New System.Drawing.Point(455, 687)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 29
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'cboDocCode
        '
        Me.cboDocCode.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboDocCode.FormattingEnabled = True
        Me.cboDocCode.Location = New System.Drawing.Point(239, 53)
        Me.cboDocCode.Name = "cboDocCode"
        Me.cboDocCode.Size = New System.Drawing.Size(300, 24)
        Me.cboDocCode.TabIndex = 8
        '
        'cboPeriod
        '
        Me.cboPeriod.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboPeriod.FormattingEnabled = True
        Me.cboPeriod.Location = New System.Drawing.Point(239, 12)
        Me.cboPeriod.Name = "cboPeriod"
        Me.cboPeriod.Size = New System.Drawing.Size(166, 24)
        Me.cboPeriod.TabIndex = 2
        '
        'lblIndispensableMemberNo
        '
        Me.lblIndispensableMemberNo.AutoSize = True
        Me.lblIndispensableMemberNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableMemberNo.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableMemberNo.Location = New System.Drawing.Point(116, 15)
        Me.lblIndispensableMemberNo.Name = "lblIndispensableMemberNo"
        Me.lblIndispensableMemberNo.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableMemberNo.TabIndex = 0
        Me.lblIndispensableMemberNo.Text = "*"
        '
        'lblIndispensableDocCode
        '
        Me.lblIndispensableDocCode.AutoSize = True
        Me.lblIndispensableDocCode.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableDocCode.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableDocCode.Location = New System.Drawing.Point(115, 55)
        Me.lblIndispensableDocCode.Name = "lblIndispensableDocCode"
        Me.lblIndispensableDocCode.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableDocCode.TabIndex = 6
        Me.lblIndispensableDocCode.Text = "*"
        '
        'lblIndispensableSubject
        '
        Me.lblIndispensableSubject.AutoSize = True
        Me.lblIndispensableSubject.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableSubject.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableSubject.Location = New System.Drawing.Point(115, 96)
        Me.lblIndispensableSubject.Name = "lblIndispensableSubject"
        Me.lblIndispensableSubject.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableSubject.TabIndex = 9
        Me.lblIndispensableSubject.Text = "*"
        '
        'lblIndispensableApplyDate
        '
        Me.lblIndispensableApplyDate.AutoSize = True
        Me.lblIndispensableApplyDate.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableApplyDate.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableApplyDate.Location = New System.Drawing.Point(485, 15)
        Me.lblIndispensableApplyDate.Name = "lblIndispensableApplyDate"
        Me.lblIndispensableApplyDate.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableApplyDate.TabIndex = 3
        Me.lblIndispensableApplyDate.Text = "*"
        Me.lblIndispensableApplyDate.Visible = False
        '
        'dtpApplyDate
        '
        Me.dtpApplyDate.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpApplyDate.Location = New System.Drawing.Point(585, 12)
        Me.dtpApplyDate.Name = "dtpApplyDate"
        Me.dtpApplyDate.Size = New System.Drawing.Size(136, 23)
        Me.dtpApplyDate.TabIndex = 5
        Me.dtpApplyDate.Visible = False
        '
        'txtSubjectManual
        '
        Me.txtSubjectManual.BackColor = System.Drawing.Color.White
        Me.txtSubjectManual.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtSubjectManual.Location = New System.Drawing.Point(239, 135)
        Me.txtSubjectManual.MaxLength = 100
        Me.txtSubjectManual.Name = "txtSubjectManual"
        Me.txtSubjectManual.Size = New System.Drawing.Size(482, 23)
        Me.txtSubjectManual.TabIndex = 13
        '
        'FM040602
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(851, 729)
        Me.Controls.Add(Me.txtSubjectManual)
        Me.Controls.Add(Me.dtpApplyDate)
        Me.Controls.Add(Me.lblIndispensableApplyDate)
        Me.Controls.Add(Me.lblIndispensableSubject)
        Me.Controls.Add(Me.lblIndispensableDocCode)
        Me.Controls.Add(Me.lblIndispensableMemberNo)
        Me.Controls.Add(Me.cboPeriod)
        Me.Controls.Add(Me.cboDocCode)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnCreate)
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.lblSubjectManual)
        Me.Controls.Add(Me.lblApplyDate)
        Me.Controls.Add(Me.lblSubject)
        Me.Controls.Add(Me.lblPeriod)
        Me.Controls.Add(Me.cboSubject)
        Me.Controls.Add(Me.lblDocCode)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM040602"
        Me.ShowInTaskbar = False
        Me.Text = "作成する文書の詳細を決定してください。"
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cboSubject As System.Windows.Forms.ComboBox
    Friend WithEvents lblDocCode As System.Windows.Forms.Label
    Friend WithEvents lblPeriod As System.Windows.Forms.Label
    Friend WithEvents lblSubject As System.Windows.Forms.Label
    Friend WithEvents lblApplyDate As System.Windows.Forms.Label
    Friend WithEvents lblSubjectManual As System.Windows.Forms.Label
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents lblKara As System.Windows.Forms.Label
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents btnCreate As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents cboDocCode As System.Windows.Forms.ComboBox
    Friend WithEvents cboPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents lblIndispensableMemberNo As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableDocCode As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableSubject As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableApplyDate As System.Windows.Forms.Label
    Friend WithEvents dtpApplyDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtSubjectManual As System.Windows.Forms.TextBox
    Friend WithEvents btnAllCheck As System.Windows.Forms.Button
    Friend WithEvents btnSelectCancel As System.Windows.Forms.Button
    Friend WithEvents btnAllNoCheck As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents dgvResult As System.Windows.Forms.DataGridView
    Friend WithEvents col_check As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents commitee_name As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents apply_year_month As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents update_date As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents update_user As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents condition As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents committee_update_id As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents doc_out As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents committee_id As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents update_user_id As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
