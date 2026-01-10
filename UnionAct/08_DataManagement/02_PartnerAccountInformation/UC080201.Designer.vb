<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC080201
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
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.txtNameKana = New System.Windows.Forms.TextBox
        Me.txtStafId = New System.Windows.Forms.TextBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cboStatus = New System.Windows.Forms.ComboBox
        Me.cboStafKind = New System.Windows.Forms.ComboBox
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblNameKana = New System.Windows.Forms.Label
        Me.lblStafKind = New System.Windows.Forms.Label
        Me.lblStafId = New System.Windows.Forms.Label
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.dgvResult = New System.Windows.Forms.DataGridView
        Me.StafId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Dezit = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.StafName = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Belonging = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.StafKind = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Model = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Status = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Local = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.AccountFlg = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.SeniorFlg = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UserId = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.UseDate = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Bank = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.BankOffice = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.attr_c_user_id = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.c_ksh = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.attr_d_from = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.chkAccount = New System.Windows.Forms.CheckBox
        Me.chkSenior = New System.Windows.Forms.CheckBox
        Me.btnDetail = New System.Windows.Forms.Button
        Me.grpSearch.SuspendLayout()
        Me.grpResult.SuspendLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(200, 20)
        Me.lblTitle.MinimumSize = New System.Drawing.Size(630, 35)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.lblTitle.Size = New System.Drawing.Size(630, 35)
        Me.lblTitle.TabIndex = 66
        Me.lblTitle.Text = "組合員口座情報"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.txtNameKana)
        Me.grpSearch.Controls.Add(Me.txtStafId)
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.cboStatus)
        Me.grpSearch.Controls.Add(Me.cboStafKind)
        Me.grpSearch.Controls.Add(Me.lblStatus)
        Me.grpSearch.Controls.Add(Me.lblNameKana)
        Me.grpSearch.Controls.Add(Me.lblStafKind)
        Me.grpSearch.Controls.Add(Me.lblStafId)
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.Location = New System.Drawing.Point(22, 75)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(980, 80)
        Me.grpSearch.TabIndex = 68
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "検索条件"
        '
        'txtNameKana
        '
        Me.txtNameKana.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNameKana.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.txtNameKana.Location = New System.Drawing.Point(508, 16)
        Me.txtNameKana.Name = "txtNameKana"
        Me.txtNameKana.Size = New System.Drawing.Size(233, 23)
        Me.txtNameKana.TabIndex = 2
        '
        'txtStafId
        '
        Me.txtStafId.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStafId.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtStafId.Location = New System.Drawing.Point(226, 16)
        Me.txtStafId.Name = "txtStafId"
        Me.txtStafId.Size = New System.Drawing.Size(96, 23)
        Me.txtStafId.TabIndex = 1
        Me.txtStafId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(801, 27)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 5
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cboStatus
        '
        Me.cboStatus.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboStatus.FormattingEnabled = True
        Me.cboStatus.Location = New System.Drawing.Point(508, 47)
        Me.cboStatus.Name = "cboStatus"
        Me.cboStatus.Size = New System.Drawing.Size(129, 24)
        Me.cboStatus.TabIndex = 4
        Me.cboStatus.Text = "加入"
        '
        'cboStafKind
        '
        Me.cboStafKind.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboStafKind.FormattingEnabled = True
        Me.cboStafKind.Location = New System.Drawing.Point(226, 47)
        Me.cboStafKind.Name = "cboStafKind"
        Me.cboStafKind.Size = New System.Drawing.Size(133, 24)
        Me.cboStafKind.TabIndex = 3
        Me.cboStafKind.Text = "正組合員"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(386, 50)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(68, 16)
        Me.lblStatus.TabIndex = 6
        Me.lblStatus.Text = "ステータス"
        '
        'lblNameKana
        '
        Me.lblNameKana.AutoSize = True
        Me.lblNameKana.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNameKana.Location = New System.Drawing.Point(386, 19)
        Me.lblNameKana.Name = "lblNameKana"
        Me.lblNameKana.Size = New System.Drawing.Size(112, 16)
        Me.lblNameKana.TabIndex = 5
        Me.lblNameKana.Text = "名前（半角カナ）"
        '
        'lblStafKind
        '
        Me.lblStafKind.AutoSize = True
        Me.lblStafKind.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStafKind.Location = New System.Drawing.Point(123, 50)
        Me.lblStafKind.Name = "lblStafKind"
        Me.lblStafKind.Size = New System.Drawing.Size(88, 16)
        Me.lblStafKind.TabIndex = 8
        Me.lblStafKind.Text = "組合員種別"
        '
        'lblStafId
        '
        Me.lblStafId.AutoSize = True
        Me.lblStafId.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStafId.Location = New System.Drawing.Point(123, 19)
        Me.lblStafId.Name = "lblStafId"
        Me.lblStafId.Size = New System.Drawing.Size(72, 16)
        Me.lblStafId.TabIndex = 7
        Me.lblStafId.Text = "社員番号"
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.dgvResult)
        Me.grpResult.Controls.Add(Me.chkAccount)
        Me.grpResult.Controls.Add(Me.chkSenior)
        Me.grpResult.Controls.Add(Me.btnDetail)
        Me.grpResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpResult.Location = New System.Drawing.Point(87, 175)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(850, 620)
        Me.grpResult.TabIndex = 69
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "振込情報の一覧（ 999 件）"
        Me.grpResult.Visible = False
        '
        'dgvResult
        '
        Me.dgvResult.AllowUserToAddRows = False
        Me.dgvResult.AllowUserToDeleteRows = False
        Me.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResult.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.StafId, Me.Dezit, Me.StafName, Me.Belonging, Me.StafKind, Me.Model, Me.Status, Me.Local, Me.AccountFlg, Me.SeniorFlg, Me.UserId, Me.UseDate, Me.Bank, Me.BankOffice, Me.attr_c_user_id, Me.c_ksh, Me.attr_d_from})
        Me.dgvResult.Location = New System.Drawing.Point(10, 30)
        Me.dgvResult.MultiSelect = False
        Me.dgvResult.Name = "dgvResult"
        Me.dgvResult.ReadOnly = True
        Me.dgvResult.RowHeadersVisible = False
        Me.dgvResult.RowTemplate.Height = 21
        Me.dgvResult.Size = New System.Drawing.Size(830, 530)
        Me.dgvResult.TabIndex = 16
        '
        'StafId
        '
        Me.StafId.HeaderText = "社員番号"
        Me.StafId.Name = "StafId"
        Me.StafId.ReadOnly = True
        Me.StafId.Width = 95
        '
        'Dezit
        '
        Me.Dezit.HeaderText = "CD"
        Me.Dezit.MaxInputLength = 1
        Me.Dezit.Name = "Dezit"
        Me.Dezit.ReadOnly = True
        Me.Dezit.Width = 29
        '
        'StafName
        '
        Me.StafName.HeaderText = "名前"
        Me.StafName.Name = "StafName"
        Me.StafName.ReadOnly = True
        Me.StafName.Width = 104
        '
        'Belonging
        '
        Me.Belonging.HeaderText = "組合支部"
        Me.Belonging.Name = "Belonging"
        Me.Belonging.ReadOnly = True
        Me.Belonging.Width = 95
        '
        'StafKind
        '
        Me.StafKind.HeaderText = "組合員種別"
        Me.StafKind.Name = "StafKind"
        Me.StafKind.ReadOnly = True
        Me.StafKind.Width = 111
        '
        'Model
        '
        Me.Model.HeaderText = "機種"
        Me.Model.Name = "Model"
        Me.Model.ReadOnly = True
        Me.Model.Width = 63
        '
        'Status
        '
        Me.Status.HeaderText = "ステータス"
        Me.Status.Name = "Status"
        Me.Status.ReadOnly = True
        Me.Status.Width = 91
        '
        'Local
        '
        Me.Local.HeaderText = "会社所属"
        Me.Local.Name = "Local"
        Me.Local.ReadOnly = True
        Me.Local.Width = 95
        '
        'AccountFlg
        '
        Me.AccountFlg.HeaderText = "口座登録状況"
        Me.AccountFlg.Name = "AccountFlg"
        Me.AccountFlg.ReadOnly = True
        Me.AccountFlg.Width = 127
        '
        'SeniorFlg
        '
        Me.SeniorFlg.HeaderText = "シニアフラグ"
        Me.SeniorFlg.Name = "SeniorFlg"
        Me.SeniorFlg.ReadOnly = True
        Me.SeniorFlg.Visible = False
        Me.SeniorFlg.Width = 200
        '
        'UserId
        '
        Me.UserId.HeaderText = "個人認証ID"
        Me.UserId.Name = "UserId"
        Me.UserId.ReadOnly = True
        Me.UserId.Visible = False
        Me.UserId.Width = 200
        '
        'UseDate
        '
        Me.UseDate.HeaderText = "適用日付"
        Me.UseDate.Name = "UseDate"
        Me.UseDate.ReadOnly = True
        Me.UseDate.Visible = False
        Me.UseDate.Width = 200
        '
        'Bank
        '
        Me.Bank.HeaderText = "金融機関コード"
        Me.Bank.Name = "Bank"
        Me.Bank.ReadOnly = True
        Me.Bank.Width = 200
        '
        'BankOffice
        '
        Me.BankOffice.HeaderText = "支店番号"
        Me.BankOffice.Name = "BankOffice"
        Me.BankOffice.ReadOnly = True
        Me.BankOffice.Visible = False
        Me.BankOffice.Width = 200
        '
        'attr_c_user_id
        '
        Me.attr_c_user_id.HeaderText = "個人認証ID"
        Me.attr_c_user_id.Name = "attr_c_user_id"
        Me.attr_c_user_id.ReadOnly = True
        Me.attr_c_user_id.Visible = False
        '
        'c_ksh
        '
        Me.c_ksh.HeaderText = "会社コード"
        Me.c_ksh.Name = "c_ksh"
        Me.c_ksh.ReadOnly = True
        Me.c_ksh.Visible = False
        '
        'attr_d_from
        '
        Me.attr_d_from.HeaderText = "適用日付"
        Me.attr_d_from.Name = "attr_d_from"
        Me.attr_d_from.ReadOnly = True
        Me.attr_d_from.Visible = False
        '
        'chkAccount
        '
        Me.chkAccount.AutoSize = True
        Me.chkAccount.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.chkAccount.Location = New System.Drawing.Point(652, 591)
        Me.chkAccount.Name = "chkAccount"
        Me.chkAccount.Size = New System.Drawing.Size(187, 20)
        Me.chkAccount.TabIndex = 8
        Me.chkAccount.Text = "口座登録済情報非表示"
        Me.chkAccount.UseVisualStyleBackColor = True
        '
        'chkSenior
        '
        Me.chkSenior.AutoSize = True
        Me.chkSenior.Checked = True
        Me.chkSenior.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSenior.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.chkSenior.Location = New System.Drawing.Point(652, 567)
        Me.chkSenior.Name = "chkSenior"
        Me.chkSenior.Size = New System.Drawing.Size(171, 20)
        Me.chkSenior.TabIndex = 7
        Me.chkSenior.Text = "シニア前情報非表示"
        Me.chkSenior.UseVisualStyleBackColor = True
        '
        'btnDetail
        '
        Me.btnDetail.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDetail.Location = New System.Drawing.Point(370, 574)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(110, 30)
        Me.btnDetail.TabIndex = 6
        Me.btnDetail.Text = "詳細"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'UC080201
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.lblTitle)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Name = "UC080201"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        Me.grpResult.PerformLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents txtNameKana As System.Windows.Forms.TextBox
    Friend WithEvents txtStafId As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cboStatus As System.Windows.Forms.ComboBox
    Friend WithEvents cboStafKind As System.Windows.Forms.ComboBox
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblNameKana As System.Windows.Forms.Label
    Friend WithEvents lblStafKind As System.Windows.Forms.Label
    Friend WithEvents lblStafId As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents chkSenior As System.Windows.Forms.CheckBox
    Friend WithEvents btnDetail As System.Windows.Forms.Button
    Friend WithEvents dgvResult As System.Windows.Forms.DataGridView
    Friend WithEvents chkAccount As System.Windows.Forms.CheckBox
    Friend WithEvents StafId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Dezit As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StafName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Belonging As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StafKind As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Model As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Status As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Local As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents AccountFlg As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SeniorFlg As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UserId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UseDate As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Bank As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents BankOffice As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents attr_c_user_id As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents c_ksh As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents attr_d_from As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
