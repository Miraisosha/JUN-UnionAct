<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC030301
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC030301))
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.txtCommittee = New System.Windows.Forms.TextBox
        Me.cmbMonth = New System.Windows.Forms.ComboBox
        Me.cmbYear = New System.Windows.Forms.ComboBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.lblMonth = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.Label
        Me.lblMandatoryEntryMonth = New System.Windows.Forms.Label
        Me.lblEntryMonth = New System.Windows.Forms.Label
        Me.lblMandatoryCommittee = New System.Windows.Forms.Label
        Me.lblCommittee = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.flxAttendance = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.chkDownMemo = New System.Windows.Forms.CheckBox
        Me.lblMemo = New System.Windows.Forms.Label
        Me.lblDownTitle = New System.Windows.Forms.Label
        Me.lblUpMemo = New System.Windows.Forms.Label
        Me.lblUpTitle = New System.Windows.Forms.Label
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnInsertChk = New System.Windows.Forms.Button
        Me.grpSearch.SuspendLayout()
        Me.grpResult.SuspendLayout()
        CType(Me.flxAttendance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.txtCommittee)
        Me.grpSearch.Controls.Add(Me.cmbMonth)
        Me.grpSearch.Controls.Add(Me.cmbYear)
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.lblMonth)
        Me.grpSearch.Controls.Add(Me.lblYear)
        Me.grpSearch.Controls.Add(Me.lblMandatoryEntryMonth)
        Me.grpSearch.Controls.Add(Me.lblEntryMonth)
        Me.grpSearch.Controls.Add(Me.lblMandatoryCommittee)
        Me.grpSearch.Controls.Add(Me.lblCommittee)
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.ForeColor = System.Drawing.Color.Blue
        Me.grpSearch.Location = New System.Drawing.Point(53, 79)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(930, 77)
        Me.grpSearch.TabIndex = 9
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "検索条件"
        '
        'txtCommittee
        '
        Me.txtCommittee.BackColor = System.Drawing.Color.Cornsilk
        Me.txtCommittee.Location = New System.Drawing.Point(133, 33)
        Me.txtCommittee.Name = "txtCommittee"
        Me.txtCommittee.ReadOnly = True
        Me.txtCommittee.Size = New System.Drawing.Size(188, 23)
        Me.txtCommittee.TabIndex = 1
        '
        'cmbMonth
        '
        Me.cmbMonth.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbMonth.FormattingEnabled = True
        Me.cmbMonth.Location = New System.Drawing.Point(664, 33)
        Me.cmbMonth.MaxDropDownItems = 13
        Me.cmbMonth.Name = "cmbMonth"
        Me.cmbMonth.Size = New System.Drawing.Size(41, 24)
        Me.cmbMonth.TabIndex = 3
        '
        'cmbYear
        '
        Me.cmbYear.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbYear.FormattingEnabled = True
        Me.cmbYear.Location = New System.Drawing.Point(553, 33)
        Me.cmbYear.Name = "cmbYear"
        Me.cmbYear.Size = New System.Drawing.Size(68, 24)
        Me.cmbYear.TabIndex = 2
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(785, 28)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonth.Location = New System.Drawing.Point(711, 37)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(24, 16)
        Me.lblMonth.TabIndex = 10
        Me.lblMonth.Text = "月"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYear.Location = New System.Drawing.Point(627, 37)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(24, 16)
        Me.lblYear.TabIndex = 9
        Me.lblYear.Text = "年"
        '
        'lblMandatoryEntryMonth
        '
        Me.lblMandatoryEntryMonth.AutoSize = True
        Me.lblMandatoryEntryMonth.ForeColor = System.Drawing.Color.Red
        Me.lblMandatoryEntryMonth.Location = New System.Drawing.Point(477, 37)
        Me.lblMandatoryEntryMonth.Name = "lblMandatoryEntryMonth"
        Me.lblMandatoryEntryMonth.Size = New System.Drawing.Size(16, 16)
        Me.lblMandatoryEntryMonth.TabIndex = 6
        Me.lblMandatoryEntryMonth.Text = "*"
        '
        'lblEntryMonth
        '
        Me.lblEntryMonth.AutoSize = True
        Me.lblEntryMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEntryMonth.Location = New System.Drawing.Point(494, 37)
        Me.lblEntryMonth.Name = "lblEntryMonth"
        Me.lblEntryMonth.Size = New System.Drawing.Size(56, 16)
        Me.lblEntryMonth.TabIndex = 5
        Me.lblEntryMonth.Text = "登録月"
        '
        'lblMandatoryCommittee
        '
        Me.lblMandatoryCommittee.AutoSize = True
        Me.lblMandatoryCommittee.ForeColor = System.Drawing.Color.Red
        Me.lblMandatoryCommittee.Location = New System.Drawing.Point(22, 37)
        Me.lblMandatoryCommittee.Name = "lblMandatoryCommittee"
        Me.lblMandatoryCommittee.Size = New System.Drawing.Size(16, 16)
        Me.lblMandatoryCommittee.TabIndex = 2
        Me.lblMandatoryCommittee.Text = "*"
        '
        'lblCommittee
        '
        Me.lblCommittee.AutoSize = True
        Me.lblCommittee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommittee.Location = New System.Drawing.Point(39, 37)
        Me.lblCommittee.Name = "lblCommittee"
        Me.lblCommittee.Size = New System.Drawing.Size(88, 16)
        Me.lblCommittee.TabIndex = 1
        Me.lblCommittee.Text = "部／委員会"
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
        Me.lblTitle.TabIndex = 8
        Me.lblTitle.Text = "中執活動報告"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.flxAttendance)
        Me.grpResult.Controls.Add(Me.chkDownMemo)
        Me.grpResult.Controls.Add(Me.lblMemo)
        Me.grpResult.Controls.Add(Me.lblDownTitle)
        Me.grpResult.Controls.Add(Me.lblUpMemo)
        Me.grpResult.Controls.Add(Me.lblUpTitle)
        Me.grpResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpResult.ForeColor = System.Drawing.Color.Blue
        Me.grpResult.Location = New System.Drawing.Point(53, 183)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(930, 580)
        Me.grpResult.TabIndex = 10
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "出欠簿"
        Me.grpResult.Visible = False
        '
        'flxAttendance
        '
        Me.flxAttendance.ColumnInfo = resources.GetString("flxAttendance.ColumnInfo")
        Me.flxAttendance.Location = New System.Drawing.Point(6, 57)
        Me.flxAttendance.Name = "flxAttendance"
        Me.flxAttendance.Rows.Count = 1
        Me.flxAttendance.Rows.DefaultSize = 22
        Me.flxAttendance.Size = New System.Drawing.Size(918, 483)
        Me.flxAttendance.StyleInfo = resources.GetString("flxAttendance.StyleInfo")
        Me.flxAttendance.TabIndex = 13
        '
        'chkDownMemo
        '
        Me.chkDownMemo.AutoSize = True
        Me.chkDownMemo.Checked = True
        Me.chkDownMemo.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkDownMemo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDownMemo.Location = New System.Drawing.Point(626, 35)
        Me.chkDownMemo.Name = "chkDownMemo"
        Me.chkDownMemo.Size = New System.Drawing.Size(123, 20)
        Me.chkDownMemo.TabIndex = 11
        Me.chkDownMemo.TabStop = False
        Me.chkDownMemo.Text = "中執昼食費可"
        Me.chkDownMemo.UseVisualStyleBackColor = True
        '
        'lblMemo
        '
        Me.lblMemo.AutoSize = True
        Me.lblMemo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMemo.Location = New System.Drawing.Point(585, 543)
        Me.lblMemo.Name = "lblMemo"
        Me.lblMemo.Size = New System.Drawing.Size(327, 16)
        Me.lblMemo.TabIndex = 10
        Me.lblMemo.Text = "※２つ前の締め日以前の内容変更は出来ません。"
        '
        'lblDownTitle
        '
        Me.lblDownTitle.AutoSize = True
        Me.lblDownTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDownTitle.Location = New System.Drawing.Point(585, 36)
        Me.lblDownTitle.Name = "lblDownTitle"
        Me.lblDownTitle.Size = New System.Drawing.Size(48, 16)
        Me.lblDownTitle.TabIndex = 9
        Me.lblDownTitle.Text = "下段："
        '
        'lblUpMemo
        '
        Me.lblUpMemo.AutoSize = True
        Me.lblUpMemo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUpMemo.Location = New System.Drawing.Point(626, 15)
        Me.lblUpMemo.Name = "lblUpMemo"
        Me.lblUpMemo.Size = New System.Drawing.Size(116, 16)
        Me.lblUpMemo.TabIndex = 5
        Me.lblUpMemo.Text = "出欠情報を表記"
        '
        'lblUpTitle
        '
        Me.lblUpTitle.AutoSize = True
        Me.lblUpTitle.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUpTitle.Location = New System.Drawing.Point(585, 15)
        Me.lblUpTitle.Name = "lblUpTitle"
        Me.lblUpTitle.Size = New System.Drawing.Size(48, 16)
        Me.lblUpTitle.TabIndex = 1
        Me.lblUpTitle.Text = "上段："
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnUpdate.Location = New System.Drawing.Point(867, 769)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(110, 30)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "内容変更"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancel.Location = New System.Drawing.Point(867, 769)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnInsertChk
        '
        Me.btnInsertChk.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsertChk.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnInsertChk.Location = New System.Drawing.Point(729, 769)
        Me.btnInsertChk.Name = "btnInsertChk"
        Me.btnInsertChk.Size = New System.Drawing.Size(110, 30)
        Me.btnInsertChk.TabIndex = 5
        Me.btnInsertChk.Text = "登録確認"
        Me.btnInsertChk.UseVisualStyleBackColor = True
        '
        'UC030301
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.btnInsertChk)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnCancel)
        Me.Name = "UC030301"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        Me.grpResult.PerformLayout()
        CType(Me.flxAttendance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lblMonth As System.Windows.Forms.Label
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents lblMandatoryEntryMonth As System.Windows.Forms.Label
    Friend WithEvents lblEntryMonth As System.Windows.Forms.Label
    Friend WithEvents lblMandatoryCommittee As System.Windows.Forms.Label
    Friend WithEvents lblCommittee As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents chkDownMemo As System.Windows.Forms.CheckBox
    Friend WithEvents lblMemo As System.Windows.Forms.Label
    Friend WithEvents lblDownTitle As System.Windows.Forms.Label
    Friend WithEvents lblUpMemo As System.Windows.Forms.Label
    Friend WithEvents lblUpTitle As System.Windows.Forms.Label
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents cmbMonth As System.Windows.Forms.ComboBox
    Friend WithEvents cmbYear As System.Windows.Forms.ComboBox
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnInsertChk As System.Windows.Forms.Button
    Friend WithEvents flxAttendance As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents txtCommittee As System.Windows.Forms.TextBox

End Class
