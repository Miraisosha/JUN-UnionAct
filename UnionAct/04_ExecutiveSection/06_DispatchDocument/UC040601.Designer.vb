<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040601
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
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpIssueDocList = New System.Windows.Forms.GroupBox
        Me.TabDoc = New System.Windows.Forms.TabControl
        Me.tbpIssued = New System.Windows.Forms.TabPage
        Me.dgvResult = New System.Windows.Forms.DataGridView
        Me.DocCode = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Period = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DocNo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.FileName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.IssueDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CreateDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CreatePerson = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UpdateDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UpdatePerson = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.sDocId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.iDocId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.PeriodCode = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SubjectSeq = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Subject = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Template = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Biko = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.CreatePorsonId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UpdatePersonId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DocName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.tbpTemp = New System.Windows.Forms.TabPage
        Me.btnDelete = New System.Windows.Forms.Button
        Me.dgvResultTmp = New System.Windows.Forms.DataGridView
        Me.TmpDocCode = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpPeriod = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpDocNo = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpFileName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpIssueDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpCreateDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpCreatePorson = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpUpdateDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpUpdatePerson = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpsDodId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpiDocId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpPeriodCode = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpSubjectSeq = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpTemplate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpSubject = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpBiko = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpCreatePersonId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpUpdatePersonId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.TmpDocName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnCreate = New System.Windows.Forms.Button
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.grpIssueDate = New System.Windows.Forms.GroupBox
        Me.dtpIssueDate = New System.Windows.Forms.DateTimePicker
        Me.optSpecify = New System.Windows.Forms.RadioButton
        Me.optNotSpecify = New System.Windows.Forms.RadioButton
        Me.lblDocNo = New System.Windows.Forms.Label
        Me.txtDocNo = New System.Windows.Forms.TextBox
        Me.lblSubjectKind = New System.Windows.Forms.Label
        Me.cboSubjectSeq = New System.Windows.Forms.ComboBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.lblHyphen = New System.Windows.Forms.Label
        Me.lblGo = New System.Windows.Forms.Label
        Me.lblPeriod = New System.Windows.Forms.Label
        Me.cboDocCode = New System.Windows.Forms.ComboBox
        Me.cboPeriod = New System.Windows.Forms.ComboBox
        Me.lblManageCode = New System.Windows.Forms.Label
        Me.btnShow = New System.Windows.Forms.Button
        Me.btnEdit = New System.Windows.Forms.Button
        Me.btnCopyEdit = New System.Windows.Forms.Button
        Me.grpIssueDocList.SuspendLayout()
        Me.TabDoc.SuspendLayout()
        Me.tbpIssued.SuspendLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tbpTemp.SuspendLayout()
        CType(Me.dgvResultTmp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSearch.SuspendLayout()
        Me.grpIssueDate.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(200, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(630, 35)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "発信文書"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpIssueDocList
        '
        Me.grpIssueDocList.Controls.Add(Me.TabDoc)
        Me.grpIssueDocList.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpIssueDocList.ForeColor = System.Drawing.Color.Blue
        Me.grpIssueDocList.Location = New System.Drawing.Point(7, 210)
        Me.grpIssueDocList.Name = "grpIssueDocList"
        Me.grpIssueDocList.Size = New System.Drawing.Size(1010, 532)
        Me.grpIssueDocList.TabIndex = 17
        Me.grpIssueDocList.TabStop = False
        Me.grpIssueDocList.Text = "発信文書一覧"
        '
        'TabDoc
        '
        Me.TabDoc.Controls.Add(Me.tbpIssued)
        Me.TabDoc.Controls.Add(Me.tbpTemp)
        Me.TabDoc.Location = New System.Drawing.Point(10, 18)
        Me.TabDoc.Name = "TabDoc"
        Me.TabDoc.SelectedIndex = 0
        Me.TabDoc.Size = New System.Drawing.Size(990, 508)
        Me.TabDoc.TabIndex = 18
        '
        'tbpIssued
        '
        Me.tbpIssued.Controls.Add(Me.dgvResult)
        Me.tbpIssued.Location = New System.Drawing.Point(4, 25)
        Me.tbpIssued.Name = "tbpIssued"
        Me.tbpIssued.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpIssued.Size = New System.Drawing.Size(982, 479)
        Me.tbpIssued.TabIndex = 0
        Me.tbpIssued.Text = "発信済"
        Me.tbpIssued.UseVisualStyleBackColor = True
        '
        'dgvResult
        '
        Me.dgvResult.AllowUserToAddRows = False
        Me.dgvResult.AllowUserToDeleteRows = False
        Me.dgvResult.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResult.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResult.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DocCode, Me.Period, Me.DocNo, Me.FileName, Me.IssueDate, Me.CreateDate, Me.CreatePerson, Me.UpdateDate, Me.UpdatePerson, Me.sDocId, Me.iDocId, Me.PeriodCode, Me.SubjectSeq, Me.Subject, Me.Template, Me.Biko, Me.CreatePorsonId, Me.UpdatePersonId, Me.DocName})
        Me.dgvResult.Location = New System.Drawing.Point(6, 6)
        Me.dgvResult.MultiSelect = False
        Me.dgvResult.Name = "dgvResult"
        Me.dgvResult.ReadOnly = True
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle5.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResult.RowHeadersDefaultCellStyle = DataGridViewCellStyle5
        Me.dgvResult.RowHeadersWidth = 45
        Me.dgvResult.RowTemplate.Height = 21
        Me.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResult.Size = New System.Drawing.Size(970, 467)
        Me.dgvResult.TabIndex = 6
        '
        'DocCode
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.DocCode.DefaultCellStyle = DataGridViewCellStyle2
        Me.DocCode.HeaderText = "管理CD"
        Me.DocCode.Name = "DocCode"
        Me.DocCode.ReadOnly = True
        Me.DocCode.Width = 90
        '
        'Period
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Period.DefaultCellStyle = DataGridViewCellStyle3
        Me.Period.HeaderText = "期"
        Me.Period.Name = "Period"
        Me.Period.ReadOnly = True
        Me.Period.Width = 60
        '
        'DocNo
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.DocNo.DefaultCellStyle = DataGridViewCellStyle4
        Me.DocNo.HeaderText = "文書No"
        Me.DocNo.Name = "DocNo"
        Me.DocNo.ReadOnly = True
        Me.DocNo.Width = 85
        '
        'FileName
        '
        Me.FileName.HeaderText = "ファイル名"
        Me.FileName.Name = "FileName"
        Me.FileName.ReadOnly = True
        Me.FileName.Width = 160
        '
        'IssueDate
        '
        Me.IssueDate.HeaderText = "発行日"
        Me.IssueDate.Name = "IssueDate"
        Me.IssueDate.ReadOnly = True
        '
        'CreateDate
        '
        Me.CreateDate.HeaderText = "作成日"
        Me.CreateDate.Name = "CreateDate"
        Me.CreateDate.ReadOnly = True
        '
        'CreatePerson
        '
        Me.CreatePerson.HeaderText = "作成者"
        Me.CreatePerson.Name = "CreatePerson"
        Me.CreatePerson.ReadOnly = True
        '
        'UpdateDate
        '
        Me.UpdateDate.HeaderText = "更新日"
        Me.UpdateDate.Name = "UpdateDate"
        Me.UpdateDate.ReadOnly = True
        '
        'UpdatePerson
        '
        Me.UpdatePerson.HeaderText = "更新者"
        Me.UpdatePerson.Name = "UpdatePerson"
        Me.UpdatePerson.ReadOnly = True
        '
        'sDocId
        '
        Me.sDocId.HeaderText = "文書フルパス"
        Me.sDocId.Name = "sDocId"
        Me.sDocId.ReadOnly = True
        Me.sDocId.Visible = False
        '
        'iDocId
        '
        Me.iDocId.HeaderText = "文書識別コード"
        Me.iDocId.Name = "iDocId"
        Me.iDocId.ReadOnly = True
        Me.iDocId.Visible = False
        '
        'PeriodCode
        '
        Me.PeriodCode.HeaderText = "期コード"
        Me.PeriodCode.Name = "PeriodCode"
        Me.PeriodCode.ReadOnly = True
        Me.PeriodCode.Visible = False
        '
        'SubjectSeq
        '
        Me.SubjectSeq.HeaderText = "標題枝番"
        Me.SubjectSeq.Name = "SubjectSeq"
        Me.SubjectSeq.ReadOnly = True
        Me.SubjectSeq.Visible = False
        '
        'Subject
        '
        Me.Subject.HeaderText = "標題"
        Me.Subject.Name = "Subject"
        Me.Subject.ReadOnly = True
        Me.Subject.Visible = False
        '
        'Template
        '
        Me.Template.HeaderText = "テンプレート"
        Me.Template.Name = "Template"
        Me.Template.ReadOnly = True
        Me.Template.Visible = False
        '
        'Biko
        '
        Me.Biko.HeaderText = "備考"
        Me.Biko.Name = "Biko"
        Me.Biko.ReadOnly = True
        Me.Biko.Visible = False
        '
        'CreatePorsonId
        '
        Me.CreatePorsonId.HeaderText = "作成者ID"
        Me.CreatePorsonId.Name = "CreatePorsonId"
        Me.CreatePorsonId.ReadOnly = True
        Me.CreatePorsonId.Visible = False
        '
        'UpdatePersonId
        '
        Me.UpdatePersonId.HeaderText = "更新者ID"
        Me.UpdatePersonId.Name = "UpdatePersonId"
        Me.UpdatePersonId.ReadOnly = True
        Me.UpdatePersonId.Visible = False
        '
        'DocName
        '
        Me.DocName.HeaderText = "管理名"
        Me.DocName.Name = "DocName"
        Me.DocName.ReadOnly = True
        Me.DocName.Visible = False
        '
        'tbpTemp
        '
        Me.tbpTemp.Controls.Add(Me.btnDelete)
        Me.tbpTemp.Controls.Add(Me.dgvResultTmp)
        Me.tbpTemp.Location = New System.Drawing.Point(4, 25)
        Me.tbpTemp.Name = "tbpTemp"
        Me.tbpTemp.Padding = New System.Windows.Forms.Padding(3)
        Me.tbpTemp.Size = New System.Drawing.Size(982, 479)
        Me.tbpTemp.TabIndex = 1
        Me.tbpTemp.Text = "一時保存"
        Me.tbpTemp.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDelete.Location = New System.Drawing.Point(26, 437)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(110, 30)
        Me.btnDelete.TabIndex = 20
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'dgvResultTmp
        '
        Me.dgvResultTmp.AllowUserToAddRows = False
        Me.dgvResultTmp.AllowUserToDeleteRows = False
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle6.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResultTmp.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle6
        Me.dgvResultTmp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResultTmp.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.TmpDocCode, Me.TmpPeriod, Me.TmpDocNo, Me.TmpFileName, Me.TmpIssueDate, Me.TmpCreateDate, Me.TmpCreatePorson, Me.TmpUpdateDate, Me.TmpUpdatePerson, Me.TmpsDodId, Me.TmpiDocId, Me.TmpPeriodCode, Me.TmpSubjectSeq, Me.TmpTemplate, Me.TmpSubject, Me.TmpBiko, Me.TmpCreatePersonId, Me.TmpUpdatePersonId, Me.TmpDocName})
        Me.dgvResultTmp.Location = New System.Drawing.Point(6, 6)
        Me.dgvResultTmp.Name = "dgvResultTmp"
        Me.dgvResultTmp.ReadOnly = True
        Me.dgvResultTmp.RowTemplate.Height = 21
        Me.dgvResultTmp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResultTmp.Size = New System.Drawing.Size(980, 421)
        Me.dgvResultTmp.TabIndex = 19
        '
        'TmpDocCode
        '
        Me.TmpDocCode.HeaderText = "管理CD"
        Me.TmpDocCode.Name = "TmpDocCode"
        Me.TmpDocCode.ReadOnly = True
        '
        'TmpPeriod
        '
        Me.TmpPeriod.HeaderText = "期"
        Me.TmpPeriod.Name = "TmpPeriod"
        Me.TmpPeriod.ReadOnly = True
        '
        'TmpDocNo
        '
        Me.TmpDocNo.HeaderText = "文書No"
        Me.TmpDocNo.Name = "TmpDocNo"
        Me.TmpDocNo.ReadOnly = True
        Me.TmpDocNo.Visible = False
        '
        'TmpFileName
        '
        Me.TmpFileName.HeaderText = "ファイル名"
        Me.TmpFileName.Name = "TmpFileName"
        Me.TmpFileName.ReadOnly = True
        '
        'TmpIssueDate
        '
        Me.TmpIssueDate.HeaderText = "発行日"
        Me.TmpIssueDate.Name = "TmpIssueDate"
        Me.TmpIssueDate.ReadOnly = True
        '
        'TmpCreateDate
        '
        Me.TmpCreateDate.HeaderText = "作成日"
        Me.TmpCreateDate.Name = "TmpCreateDate"
        Me.TmpCreateDate.ReadOnly = True
        '
        'TmpCreatePorson
        '
        Me.TmpCreatePorson.HeaderText = "作成者"
        Me.TmpCreatePorson.Name = "TmpCreatePorson"
        Me.TmpCreatePorson.ReadOnly = True
        '
        'TmpUpdateDate
        '
        Me.TmpUpdateDate.HeaderText = "更新日"
        Me.TmpUpdateDate.Name = "TmpUpdateDate"
        Me.TmpUpdateDate.ReadOnly = True
        '
        'TmpUpdatePerson
        '
        Me.TmpUpdatePerson.HeaderText = "更新者"
        Me.TmpUpdatePerson.Name = "TmpUpdatePerson"
        Me.TmpUpdatePerson.ReadOnly = True
        '
        'TmpsDodId
        '
        Me.TmpsDodId.HeaderText = "文書フルパス"
        Me.TmpsDodId.Name = "TmpsDodId"
        Me.TmpsDodId.ReadOnly = True
        Me.TmpsDodId.Visible = False
        '
        'TmpiDocId
        '
        Me.TmpiDocId.HeaderText = "文書識別コード"
        Me.TmpiDocId.Name = "TmpiDocId"
        Me.TmpiDocId.ReadOnly = True
        Me.TmpiDocId.Visible = False
        '
        'TmpPeriodCode
        '
        Me.TmpPeriodCode.HeaderText = "期コード"
        Me.TmpPeriodCode.Name = "TmpPeriodCode"
        Me.TmpPeriodCode.ReadOnly = True
        Me.TmpPeriodCode.Visible = False
        '
        'TmpSubjectSeq
        '
        Me.TmpSubjectSeq.HeaderText = "標題枝番"
        Me.TmpSubjectSeq.Name = "TmpSubjectSeq"
        Me.TmpSubjectSeq.ReadOnly = True
        Me.TmpSubjectSeq.Visible = False
        '
        'TmpTemplate
        '
        Me.TmpTemplate.HeaderText = "テンプレート"
        Me.TmpTemplate.Name = "TmpTemplate"
        Me.TmpTemplate.ReadOnly = True
        Me.TmpTemplate.Visible = False
        '
        'TmpSubject
        '
        Me.TmpSubject.HeaderText = "標題"
        Me.TmpSubject.Name = "TmpSubject"
        Me.TmpSubject.ReadOnly = True
        Me.TmpSubject.Visible = False
        '
        'TmpBiko
        '
        Me.TmpBiko.HeaderText = "備考"
        Me.TmpBiko.Name = "TmpBiko"
        Me.TmpBiko.ReadOnly = True
        Me.TmpBiko.Visible = False
        '
        'TmpCreatePersonId
        '
        Me.TmpCreatePersonId.HeaderText = "作成者ID"
        Me.TmpCreatePersonId.Name = "TmpCreatePersonId"
        Me.TmpCreatePersonId.ReadOnly = True
        Me.TmpCreatePersonId.Visible = False
        '
        'TmpUpdatePersonId
        '
        Me.TmpUpdatePersonId.HeaderText = "更新者ID"
        Me.TmpUpdatePersonId.Name = "TmpUpdatePersonId"
        Me.TmpUpdatePersonId.ReadOnly = True
        Me.TmpUpdatePersonId.Visible = False
        '
        'TmpDocName
        '
        Me.TmpDocName.HeaderText = "管理名"
        Me.TmpDocName.Name = "TmpDocName"
        Me.TmpDocName.ReadOnly = True
        Me.TmpDocName.Visible = False
        '
        'btnCreate
        '
        Me.btnCreate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCreate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCreate.Location = New System.Drawing.Point(42, 763)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(110, 30)
        Me.btnCreate.TabIndex = 21
        Me.btnCreate.Text = "新規作成"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.grpIssueDate)
        Me.grpSearch.Controls.Add(Me.lblDocNo)
        Me.grpSearch.Controls.Add(Me.txtDocNo)
        Me.grpSearch.Controls.Add(Me.lblSubjectKind)
        Me.grpSearch.Controls.Add(Me.cboSubjectSeq)
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.lblHyphen)
        Me.grpSearch.Controls.Add(Me.lblGo)
        Me.grpSearch.Controls.Add(Me.lblPeriod)
        Me.grpSearch.Controls.Add(Me.cboDocCode)
        Me.grpSearch.Controls.Add(Me.cboPeriod)
        Me.grpSearch.Controls.Add(Me.lblManageCode)
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.ForeColor = System.Drawing.Color.Blue
        Me.grpSearch.Location = New System.Drawing.Point(27, 70)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(970, 134)
        Me.grpSearch.TabIndex = 1
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "検索条件"
        '
        'grpIssueDate
        '
        Me.grpIssueDate.Controls.Add(Me.dtpIssueDate)
        Me.grpIssueDate.Controls.Add(Me.optSpecify)
        Me.grpIssueDate.Controls.Add(Me.optNotSpecify)
        Me.grpIssueDate.Location = New System.Drawing.Point(569, 19)
        Me.grpIssueDate.Name = "grpIssueDate"
        Me.grpIssueDate.Size = New System.Drawing.Size(262, 86)
        Me.grpIssueDate.TabIndex = 12
        Me.grpIssueDate.TabStop = False
        Me.grpIssueDate.Text = "発行年月"
        '
        'dtpIssueDate
        '
        Me.dtpIssueDate.CustomFormat = "yyyy年 MM月"
        Me.dtpIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpIssueDate.Location = New System.Drawing.Point(131, 55)
        Me.dtpIssueDate.Name = "dtpIssueDate"
        Me.dtpIssueDate.Size = New System.Drawing.Size(108, 23)
        Me.dtpIssueDate.TabIndex = 15
        '
        'optSpecify
        '
        Me.optSpecify.AutoSize = True
        Me.optSpecify.Checked = True
        Me.optSpecify.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.optSpecify.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optSpecify.Location = New System.Drawing.Point(8, 56)
        Me.optSpecify.Name = "optSpecify"
        Me.optSpecify.Size = New System.Drawing.Size(90, 20)
        Me.optSpecify.TabIndex = 14
        Me.optSpecify.TabStop = True
        Me.optSpecify.Text = "指定する"
        Me.optSpecify.UseVisualStyleBackColor = True
        '
        'optNotSpecify
        '
        Me.optNotSpecify.AutoSize = True
        Me.optNotSpecify.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.optNotSpecify.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optNotSpecify.Location = New System.Drawing.Point(8, 25)
        Me.optNotSpecify.Name = "optNotSpecify"
        Me.optNotSpecify.Size = New System.Drawing.Size(106, 20)
        Me.optNotSpecify.TabIndex = 13
        Me.optNotSpecify.TabStop = True
        Me.optNotSpecify.Text = "指定しない"
        Me.optNotSpecify.UseVisualStyleBackColor = True
        '
        'lblDocNo
        '
        Me.lblDocNo.AutoSize = True
        Me.lblDocNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocNo.Location = New System.Drawing.Point(461, 19)
        Me.lblDocNo.Name = "lblDocNo"
        Me.lblDocNo.Size = New System.Drawing.Size(72, 16)
        Me.lblDocNo.TabIndex = 7
        Me.lblDocNo.Text = "文書番号"
        '
        'txtDocNo
        '
        Me.txtDocNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtDocNo.Location = New System.Drawing.Point(461, 40)
        Me.txtDocNo.Name = "txtDocNo"
        Me.txtDocNo.Size = New System.Drawing.Size(72, 23)
        Me.txtDocNo.TabIndex = 8
        '
        'lblSubjectKind
        '
        Me.lblSubjectKind.AutoSize = True
        Me.lblSubjectKind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubjectKind.Location = New System.Drawing.Point(17, 73)
        Me.lblSubjectKind.Name = "lblSubjectKind"
        Me.lblSubjectKind.Size = New System.Drawing.Size(72, 16)
        Me.lblSubjectKind.TabIndex = 10
        Me.lblSubjectKind.Text = "標題分類"
        '
        'cboSubjectSeq
        '
        Me.cboSubjectSeq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSubjectSeq.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboSubjectSeq.FormattingEnabled = True
        Me.cboSubjectSeq.Location = New System.Drawing.Point(19, 92)
        Me.cboSubjectSeq.Name = "cboSubjectSeq"
        Me.cboSubjectSeq.Size = New System.Drawing.Size(472, 24)
        Me.cboSubjectSeq.TabIndex = 11
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(848, 75)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 16
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'lblHyphen
        '
        Me.lblHyphen.AutoSize = True
        Me.lblHyphen.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblHyphen.Location = New System.Drawing.Point(439, 43)
        Me.lblHyphen.Name = "lblHyphen"
        Me.lblHyphen.Size = New System.Drawing.Size(16, 16)
        Me.lblHyphen.TabIndex = 6
        Me.lblHyphen.Text = "-"
        '
        'lblGo
        '
        Me.lblGo.AutoSize = True
        Me.lblGo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblGo.Location = New System.Drawing.Point(539, 43)
        Me.lblGo.Name = "lblGo"
        Me.lblGo.Size = New System.Drawing.Size(24, 16)
        Me.lblGo.TabIndex = 9
        Me.lblGo.Text = "号"
        '
        'lblPeriod
        '
        Me.lblPeriod.AutoSize = True
        Me.lblPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriod.Location = New System.Drawing.Point(340, 19)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.Size = New System.Drawing.Size(24, 16)
        Me.lblPeriod.TabIndex = 4
        Me.lblPeriod.Text = "期"
        '
        'cboDocCode
        '
        Me.cboDocCode.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboDocCode.FormattingEnabled = True
        Me.cboDocCode.Location = New System.Drawing.Point(19, 40)
        Me.cboDocCode.Name = "cboDocCode"
        Me.cboDocCode.Size = New System.Drawing.Size(300, 24)
        Me.cboDocCode.TabIndex = 3
        '
        'cboPeriod
        '
        Me.cboPeriod.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboPeriod.FormattingEnabled = True
        Me.cboPeriod.Location = New System.Drawing.Point(343, 40)
        Me.cboPeriod.Name = "cboPeriod"
        Me.cboPeriod.Size = New System.Drawing.Size(90, 24)
        Me.cboPeriod.TabIndex = 5
        '
        'lblManageCode
        '
        Me.lblManageCode.AutoSize = True
        Me.lblManageCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblManageCode.Location = New System.Drawing.Point(17, 19)
        Me.lblManageCode.Name = "lblManageCode"
        Me.lblManageCode.Size = New System.Drawing.Size(76, 16)
        Me.lblManageCode.TabIndex = 2
        Me.lblManageCode.Text = "管理コード"
        '
        'btnShow
        '
        Me.btnShow.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnShow.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnShow.Location = New System.Drawing.Point(520, 763)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(110, 30)
        Me.btnShow.TabIndex = 22
        Me.btnShow.Text = "表示"
        Me.btnShow.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEdit.Location = New System.Drawing.Point(680, 763)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(110, 30)
        Me.btnEdit.TabIndex = 23
        Me.btnEdit.Text = "編集"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnCopyEdit
        '
        Me.btnCopyEdit.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCopyEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCopyEdit.Location = New System.Drawing.Point(840, 763)
        Me.btnCopyEdit.Name = "btnCopyEdit"
        Me.btnCopyEdit.Size = New System.Drawing.Size(110, 30)
        Me.btnCopyEdit.TabIndex = 24
        Me.btnCopyEdit.Text = "コピーして編集"
        Me.btnCopyEdit.UseVisualStyleBackColor = True
        '
        'UC040601
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnCopyEdit)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.grpIssueDocList)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.btnCreate)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC040601"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpIssueDocList.ResumeLayout(False)
        Me.TabDoc.ResumeLayout(False)
        Me.tbpIssued.ResumeLayout(False)
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tbpTemp.ResumeLayout(False)
        CType(Me.dgvResultTmp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.grpIssueDate.ResumeLayout(False)
        Me.grpIssueDate.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpIssueDocList As System.Windows.Forms.GroupBox
    Friend WithEvents btnCreate As System.Windows.Forms.Button
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents grpIssueDate As System.Windows.Forms.GroupBox
    Friend WithEvents optSpecify As System.Windows.Forms.RadioButton
    Friend WithEvents optNotSpecify As System.Windows.Forms.RadioButton
    Friend WithEvents lblDocNo As System.Windows.Forms.Label
    Friend WithEvents txtDocNo As System.Windows.Forms.TextBox
    Friend WithEvents lblSubjectKind As System.Windows.Forms.Label
    Friend WithEvents cboSubjectSeq As System.Windows.Forms.ComboBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lblHyphen As System.Windows.Forms.Label
    Friend WithEvents lblGo As System.Windows.Forms.Label
    Friend WithEvents lblPeriod As System.Windows.Forms.Label
    Friend WithEvents cboDocCode As System.Windows.Forms.ComboBox
    Friend WithEvents cboPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents lblManageCode As System.Windows.Forms.Label
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnCopyEdit As System.Windows.Forms.Button
    Friend WithEvents TabDoc As System.Windows.Forms.TabControl
    Friend WithEvents tbpIssued As System.Windows.Forms.TabPage
    Friend WithEvents dgvResult As System.Windows.Forms.DataGridView
    Friend WithEvents tbpTemp As System.Windows.Forms.TabPage
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents dgvResultTmp As System.Windows.Forms.DataGridView
    Friend WithEvents dtpIssueDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents DocCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Period As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DocNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents FileName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents IssueDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreateDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreatePerson As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UpdateDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UpdatePerson As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents sDocId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents iDocId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PeriodCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SubjectSeq As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Subject As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Template As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Biko As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CreatePorsonId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UpdatePersonId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DocName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpDocCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpPeriod As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpDocNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpFileName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpIssueDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpCreateDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpCreatePorson As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpUpdateDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpUpdatePerson As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpsDodId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpiDocId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpPeriodCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpSubjectSeq As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpTemplate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpSubject As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpBiko As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpCreatePersonId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpUpdatePersonId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents TmpDocName As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
