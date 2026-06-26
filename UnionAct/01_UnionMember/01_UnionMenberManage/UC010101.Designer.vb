<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC010101
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.grpSearchResult = New System.Windows.Forms.GroupBox()
        Me.dgvResult = New System.Windows.Forms.DataGridView()
        Me.MemberNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.MemberName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Belonging = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.StafKind = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Model = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Local = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Join = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.From = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Qualification = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.UserId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Ksh = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SeniorFlg = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.chkSeniorPrevious = New System.Windows.Forms.CheckBox()
        Me.btnAddress = New System.Windows.Forms.Button()
        Me.btnBase = New System.Windows.Forms.Button()
        Me.grpEtc = New System.Windows.Forms.GroupBox()
        Me.btnMemberManage = New System.Windows.Forms.Button()
        Me.btnNewInsert = New System.Windows.Forms.Button()
        Me.grpSearch = New System.Windows.Forms.GroupBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.cboUnionMember = New System.Windows.Forms.ComboBox()
        Me.txtNameKana = New System.Windows.Forms.TextBox()
        Me.lblUnionMenber = New System.Windows.Forms.Label()
        Me.lblNameKana = New System.Windows.Forms.Label()
        Me.txtMemberNo = New System.Windows.Forms.TextBox()
        Me.lblMemberNo = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.grpSearchResult.SuspendLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpEtc.SuspendLayout()
        Me.grpSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpSearchResult
        '
        Me.grpSearchResult.Controls.Add(Me.dgvResult)
        Me.grpSearchResult.Controls.Add(Me.chkSeniorPrevious)
        Me.grpSearchResult.Controls.Add(Me.btnAddress)
        Me.grpSearchResult.Controls.Add(Me.btnBase)
        Me.grpSearchResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearchResult.Location = New System.Drawing.Point(97, 248)
        Me.grpSearchResult.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.grpSearchResult.Name = "grpSearchResult"
        Me.grpSearchResult.Padding = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.grpSearchResult.Size = New System.Drawing.Size(1656, 1032)
        Me.grpSearchResult.TabIndex = 10
        Me.grpSearchResult.TabStop = False
        Me.grpSearchResult.Text = "検索結果（ XXXXX 件 ）"
        Me.grpSearchResult.Visible = False
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
        Me.dgvResult.ColumnHeadersHeight = 40
        Me.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgvResult.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.MemberNo, Me.MemberName, Me.Belonging, Me.StafKind, Me.Model, Me.UserStatus, Me.Local, Me.Join, Me.From, Me.Qualification, Me.UserId, Me.Ksh, Me.SeniorFlg})
        Me.dgvResult.Location = New System.Drawing.Point(11, 38)
        Me.dgvResult.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
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
        Me.dgvResult.RowHeadersWidth = 72
        Me.dgvResult.RowTemplate.Height = 21
        Me.dgvResult.Size = New System.Drawing.Size(1613, 858)
        Me.dgvResult.TabIndex = 11
        '
        'MemberNo
        '
        Me.MemberNo.HeaderText = "社員番号"
        Me.MemberNo.MinimumWidth = 9
        Me.MemberNo.Name = "MemberNo"
        Me.MemberNo.ReadOnly = True
        Me.MemberNo.Width = 175
        '
        'MemberName
        '
        Me.MemberName.HeaderText = "名前"
        Me.MemberName.MinimumWidth = 9
        Me.MemberName.Name = "MemberName"
        Me.MemberName.ReadOnly = True
        Me.MemberName.Width = 175
        '
        'Belonging
        '
        Me.Belonging.HeaderText = "組合支部"
        Me.Belonging.MinimumWidth = 9
        Me.Belonging.Name = "Belonging"
        Me.Belonging.ReadOnly = True
        Me.Belonging.Width = 175
        '
        'StafKind
        '
        Me.StafKind.HeaderText = "組合員種別"
        Me.StafKind.MinimumWidth = 9
        Me.StafKind.Name = "StafKind"
        Me.StafKind.ReadOnly = True
        Me.StafKind.Width = 175
        '
        'Model
        '
        Me.Model.HeaderText = "機種"
        Me.Model.MinimumWidth = 9
        Me.Model.Name = "Model"
        Me.Model.ReadOnly = True
        Me.Model.Width = 175
        '
        'UserStatus
        '
        Me.UserStatus.HeaderText = "ステータス"
        Me.UserStatus.MinimumWidth = 9
        Me.UserStatus.Name = "UserStatus"
        Me.UserStatus.ReadOnly = True
        Me.UserStatus.Width = 175
        '
        'Local
        '
        Me.Local.HeaderText = "会社所属"
        Me.Local.MinimumWidth = 9
        Me.Local.Name = "Local"
        Me.Local.ReadOnly = True
        Me.Local.Width = 175
        '
        'Join
        '
        Me.Join.HeaderText = "加入年月日"
        Me.Join.MinimumWidth = 9
        Me.Join.Name = "Join"
        Me.Join.ReadOnly = True
        Me.Join.Width = 175
        '
        'From
        '
        Me.From.HeaderText = "適用日付"
        Me.From.MinimumWidth = 9
        Me.From.Name = "From"
        Me.From.ReadOnly = True
        Me.From.Width = 175
        '
        'Qualification
        '
        Me.Qualification.HeaderText = "資格"
        Me.Qualification.MinimumWidth = 9
        Me.Qualification.Name = "Qualification"
        Me.Qualification.ReadOnly = True
        Me.Qualification.Width = 175
        '
        'UserId
        '
        Me.UserId.HeaderText = "個人認証ID"
        Me.UserId.MinimumWidth = 9
        Me.UserId.Name = "UserId"
        Me.UserId.ReadOnly = True
        Me.UserId.Visible = False
        Me.UserId.Width = 175
        '
        'Ksh
        '
        Me.Ksh.HeaderText = "会社コード"
        Me.Ksh.MinimumWidth = 9
        Me.Ksh.Name = "Ksh"
        Me.Ksh.ReadOnly = True
        Me.Ksh.Visible = False
        Me.Ksh.Width = 175
        '
        'SeniorFlg
        '
        Me.SeniorFlg.HeaderText = "シニア前情報フラグ"
        Me.SeniorFlg.MinimumWidth = 9
        Me.SeniorFlg.Name = "SeniorFlg"
        Me.SeniorFlg.ReadOnly = True
        Me.SeniorFlg.Visible = False
        Me.SeniorFlg.Width = 175
        '
        'chkSeniorPrevious
        '
        Me.chkSeniorPrevious.AutoSize = True
        Me.chkSeniorPrevious.Checked = True
        Me.chkSeniorPrevious.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSeniorPrevious.Location = New System.Drawing.Point(1236, 943)
        Me.chkSeniorPrevious.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.chkSeniorPrevious.Name = "chkSeniorPrevious"
        Me.chkSeniorPrevious.Size = New System.Drawing.Size(271, 32)
        Me.chkSeniorPrevious.TabIndex = 6
        Me.chkSeniorPrevious.Text = "シニア前情報非表示"
        Me.chkSeniorPrevious.UseVisualStyleBackColor = True
        '
        'btnAddress
        '
        Me.btnAddress.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddress.Location = New System.Drawing.Point(893, 933)
        Me.btnAddress.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnAddress.Name = "btnAddress"
        Me.btnAddress.Size = New System.Drawing.Size(202, 52)
        Me.btnAddress.TabIndex = 5
        Me.btnAddress.Text = "住所情報照会"
        Me.btnAddress.UseVisualStyleBackColor = True
        '
        'btnBase
        '
        Me.btnBase.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBase.Location = New System.Drawing.Point(677, 933)
        Me.btnBase.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnBase.Name = "btnBase"
        Me.btnBase.Size = New System.Drawing.Size(202, 52)
        Me.btnBase.TabIndex = 4
        Me.btnBase.Text = "基本情報照会"
        Me.btnBase.UseVisualStyleBackColor = True
        '
        'grpEtc
        '
        Me.grpEtc.Controls.Add(Me.btnMemberManage)
        Me.grpEtc.Controls.Add(Me.btnNewInsert)
        Me.grpEtc.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpEtc.Location = New System.Drawing.Point(97, 1312)
        Me.grpEtc.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.grpEtc.Name = "grpEtc"
        Me.grpEtc.Padding = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.grpEtc.Size = New System.Drawing.Size(1656, 91)
        Me.grpEtc.TabIndex = 12
        Me.grpEtc.TabStop = False
        '
        'btnMemberManage
        '
        Me.btnMemberManage.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnMemberManage.Location = New System.Drawing.Point(1423, 28)
        Me.btnMemberManage.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnMemberManage.Name = "btnMemberManage"
        Me.btnMemberManage.Size = New System.Drawing.Size(202, 52)
        Me.btnMemberManage.TabIndex = 8
        Me.btnMemberManage.Text = "組合員人数"
        Me.btnMemberManage.UseVisualStyleBackColor = True
        '
        'btnNewInsert
        '
        Me.btnNewInsert.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnNewInsert.Location = New System.Drawing.Point(779, 28)
        Me.btnNewInsert.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnNewInsert.Name = "btnNewInsert"
        Me.btnNewInsert.Size = New System.Drawing.Size(202, 52)
        Me.btnNewInsert.TabIndex = 7
        Me.btnNewInsert.Text = "新規登録"
        Me.btnNewInsert.UseVisualStyleBackColor = True
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.cboUnionMember)
        Me.grpSearch.Controls.Add(Me.txtNameKana)
        Me.grpSearch.Controls.Add(Me.lblUnionMenber)
        Me.grpSearch.Controls.Add(Me.lblNameKana)
        Me.grpSearch.Controls.Add(Me.txtMemberNo)
        Me.grpSearch.Controls.Add(Me.lblMemberNo)
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.Location = New System.Drawing.Point(97, 122)
        Me.grpSearch.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Padding = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.grpSearch.Size = New System.Drawing.Size(1656, 91)
        Me.grpSearch.TabIndex = 9
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "検索条件"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.Location = New System.Drawing.Point(1399, 30)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(202, 52)
        Me.btnSearch.TabIndex = 3
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cboUnionMember
        '
        Me.cboUnionMember.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboUnionMember.FormattingEnabled = True
        Me.cboUnionMember.Location = New System.Drawing.Point(1173, 37)
        Me.cboUnionMember.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.cboUnionMember.Name = "cboUnionMember"
        Me.cboUnionMember.Size = New System.Drawing.Size(182, 36)
        Me.cboUnionMember.TabIndex = 2
        '
        'txtNameKana
        '
        Me.txtNameKana.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNameKana.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.txtNameKana.Location = New System.Drawing.Point(631, 37)
        Me.txtNameKana.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtNameKana.MaxLength = 200
        Me.txtNameKana.Name = "txtNameKana"
        Me.txtNameKana.Size = New System.Drawing.Size(263, 35)
        Me.txtNameKana.TabIndex = 1
        '
        'lblUnionMenber
        '
        Me.lblUnionMenber.AutoSize = True
        Me.lblUnionMenber.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblUnionMenber.Location = New System.Drawing.Point(964, 46)
        Me.lblUnionMenber.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblUnionMenber.Name = "lblUnionMenber"
        Me.lblUnionMenber.Size = New System.Drawing.Size(152, 28)
        Me.lblUnionMenber.TabIndex = 14
        Me.lblUnionMenber.Text = "組合員種別"
        '
        'lblNameKana
        '
        Me.lblNameKana.AutoSize = True
        Me.lblNameKana.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNameKana.Location = New System.Drawing.Point(435, 40)
        Me.lblNameKana.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblNameKana.Name = "lblNameKana"
        Me.lblNameKana.Size = New System.Drawing.Size(176, 28)
        Me.lblNameKana.TabIndex = 13
        Me.lblNameKana.Text = "名前(半角ｶﾅ)"
        '
        'txtMemberNo
        '
        Me.txtMemberNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtMemberNo.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtMemberNo.Location = New System.Drawing.Point(200, 37)
        Me.txtMemberNo.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtMemberNo.MaxLength = 10
        Me.txtMemberNo.Name = "txtMemberNo"
        Me.txtMemberNo.Size = New System.Drawing.Size(187, 35)
        Me.txtMemberNo.TabIndex = 0
        '
        'lblMemberNo
        '
        Me.lblMemberNo.AutoSize = True
        Me.lblMemberNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMemberNo.Location = New System.Drawing.Point(57, 42)
        Me.lblMemberNo.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblMemberNo.Name = "lblMemberNo"
        Me.lblMemberNo.Size = New System.Drawing.Size(124, 28)
        Me.lblMemberNo.TabIndex = 12
        Me.lblMemberNo.Text = "社員番号"
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(343, 24)
        Me.lblTitle.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(1155, 61)
        Me.lblTitle.TabIndex = 13
        Me.lblTitle.Text = "組合員検索"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UC010101
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpSearchResult)
        Me.Controls.Add(Me.grpEtc)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.lblTitle)
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "UC010101"
        Me.Size = New System.Drawing.Size(1877, 1435)
        Me.grpSearchResult.ResumeLayout(False)
        Me.grpSearchResult.PerformLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpEtc.ResumeLayout(False)
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSearchResult As System.Windows.Forms.GroupBox
    Friend WithEvents chkSeniorPrevious As System.Windows.Forms.CheckBox
    Friend WithEvents btnAddress As System.Windows.Forms.Button
    Friend WithEvents btnBase As System.Windows.Forms.Button
    Friend WithEvents grpEtc As System.Windows.Forms.GroupBox
    Friend WithEvents btnMemberManage As System.Windows.Forms.Button
    Friend WithEvents btnNewInsert As System.Windows.Forms.Button
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cboUnionMember As System.Windows.Forms.ComboBox
    Friend WithEvents txtNameKana As System.Windows.Forms.TextBox
    Friend WithEvents lblUnionMenber As System.Windows.Forms.Label
    Friend WithEvents lblNameKana As System.Windows.Forms.Label
    Friend WithEvents txtMemberNo As System.Windows.Forms.TextBox
    Friend WithEvents lblMemberNo As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents dgvResult As System.Windows.Forms.DataGridView
    Friend WithEvents MemberNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents MemberName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Belonging As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents StafKind As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Model As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UserStatus As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Local As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Join As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents From As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Qualification As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents UserId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Ksh As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents SeniorFlg As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
