<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC020401
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC020401))
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.cmbMonth = New System.Windows.Forms.ComboBox
        Me.cmbYear = New System.Windows.Forms.ComboBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.lblMonth = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.Label
        Me.lblIndispensableEntryMonth = New System.Windows.Forms.Label
        Me.lblEntryMonth = New System.Windows.Forms.Label
        Me.cmbCommittee = New System.Windows.Forms.ComboBox
        Me.lblIndispensableCommittee = New System.Windows.Forms.Label
        Me.lblCommittee = New System.Windows.Forms.Label
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.chkLunch = New System.Windows.Forms.CheckBox
        Me.lblDown = New System.Windows.Forms.Label
        Me.lblUpNote = New System.Windows.Forms.Label
        Me.lblUp = New System.Windows.Forms.Label
        Me.flxAttendance = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.lblMome = New System.Windows.Forms.Label
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnConfirm = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblKiValue = New System.Windows.Forms.Label
        Me.grpSearch.SuspendLayout()
        Me.grpResult.SuspendLayout()
        CType(Me.flxAttendance, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(200, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(630, 35)
        Me.lblTitle.TabIndex = 5
        Me.lblTitle.Text = "出欠簿"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.cmbMonth)
        Me.grpSearch.Controls.Add(Me.cmbYear)
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.lblMonth)
        Me.grpSearch.Controls.Add(Me.lblYear)
        Me.grpSearch.Controls.Add(Me.lblIndispensableEntryMonth)
        Me.grpSearch.Controls.Add(Me.lblEntryMonth)
        Me.grpSearch.Controls.Add(Me.cmbCommittee)
        Me.grpSearch.Controls.Add(Me.lblIndispensableCommittee)
        Me.grpSearch.Controls.Add(Me.lblCommittee)
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.ForeColor = System.Drawing.Color.Blue
        Me.grpSearch.Location = New System.Drawing.Point(61, 82)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(894, 65)
        Me.grpSearch.TabIndex = 8
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "検索条件"
        '
        'cmbMonth
        '
        Me.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMonth.FormattingEnabled = True
        Me.cmbMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cmbMonth.Location = New System.Drawing.Point(659, 26)
        Me.cmbMonth.MaxDropDownItems = 13
        Me.cmbMonth.Name = "cmbMonth"
        Me.cmbMonth.Size = New System.Drawing.Size(42, 24)
        Me.cmbMonth.TabIndex = 3
        '
        'cmbYear
        '
        Me.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbYear.FormattingEnabled = True
        Me.cmbYear.Location = New System.Drawing.Point(566, 26)
        Me.cmbYear.Name = "cmbYear"
        Me.cmbYear.Size = New System.Drawing.Size(62, 24)
        Me.cmbYear.TabIndex = 2
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(778, 25)
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
        Me.lblMonth.Location = New System.Drawing.Point(708, 30)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(24, 16)
        Me.lblMonth.TabIndex = 10
        Me.lblMonth.Text = "月"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYear.Location = New System.Drawing.Point(631, 30)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(24, 16)
        Me.lblYear.TabIndex = 9
        Me.lblYear.Text = "年"
        '
        'lblIndispensableEntryMonth
        '
        Me.lblIndispensableEntryMonth.AutoSize = True
        Me.lblIndispensableEntryMonth.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableEntryMonth.Location = New System.Drawing.Point(499, 30)
        Me.lblIndispensableEntryMonth.Name = "lblIndispensableEntryMonth"
        Me.lblIndispensableEntryMonth.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableEntryMonth.TabIndex = 6
        Me.lblIndispensableEntryMonth.Text = "*"
        '
        'lblEntryMonth
        '
        Me.lblEntryMonth.AutoSize = True
        Me.lblEntryMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEntryMonth.Location = New System.Drawing.Point(516, 30)
        Me.lblEntryMonth.Name = "lblEntryMonth"
        Me.lblEntryMonth.Size = New System.Drawing.Size(56, 16)
        Me.lblEntryMonth.TabIndex = 5
        Me.lblEntryMonth.Text = "登録月"
        '
        'cmbCommittee
        '
        Me.cmbCommittee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCommittee.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbCommittee.FormattingEnabled = True
        Me.cmbCommittee.Location = New System.Drawing.Point(142, 26)
        Me.cmbCommittee.Name = "cmbCommittee"
        Me.cmbCommittee.Size = New System.Drawing.Size(204, 24)
        Me.cmbCommittee.TabIndex = 1
        '
        'lblIndispensableCommittee
        '
        Me.lblIndispensableCommittee.AutoSize = True
        Me.lblIndispensableCommittee.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableCommittee.Location = New System.Drawing.Point(22, 30)
        Me.lblIndispensableCommittee.Name = "lblIndispensableCommittee"
        Me.lblIndispensableCommittee.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableCommittee.TabIndex = 2
        Me.lblIndispensableCommittee.Text = "*"
        '
        'lblCommittee
        '
        Me.lblCommittee.AutoSize = True
        Me.lblCommittee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommittee.Location = New System.Drawing.Point(39, 30)
        Me.lblCommittee.Name = "lblCommittee"
        Me.lblCommittee.Size = New System.Drawing.Size(88, 16)
        Me.lblCommittee.TabIndex = 1
        Me.lblCommittee.Text = "部／委員会"
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.chkLunch)
        Me.grpResult.Controls.Add(Me.lblDown)
        Me.grpResult.Controls.Add(Me.lblUpNote)
        Me.grpResult.Controls.Add(Me.lblUp)
        Me.grpResult.Controls.Add(Me.flxAttendance)
        Me.grpResult.Controls.Add(Me.lblMome)
        Me.grpResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpResult.ForeColor = System.Drawing.Color.Blue
        Me.grpResult.Location = New System.Drawing.Point(61, 174)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(888, 575)
        Me.grpResult.TabIndex = 9
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "出欠簿"
        Me.grpResult.Visible = False
        '
        'chkLunch
        '
        Me.chkLunch.AutoSize = True
        Me.chkLunch.Checked = True
        Me.chkLunch.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkLunch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkLunch.Location = New System.Drawing.Point(626, 35)
        Me.chkLunch.Name = "chkLunch"
        Me.chkLunch.Size = New System.Drawing.Size(91, 20)
        Me.chkLunch.TabIndex = 15
        Me.chkLunch.TabStop = False
        Me.chkLunch.Text = "昼食費可"
        Me.chkLunch.UseVisualStyleBackColor = True
        '
        'lblDown
        '
        Me.lblDown.AutoSize = True
        Me.lblDown.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDown.Location = New System.Drawing.Point(585, 36)
        Me.lblDown.Name = "lblDown"
        Me.lblDown.Size = New System.Drawing.Size(48, 16)
        Me.lblDown.TabIndex = 14
        Me.lblDown.Text = "下段："
        '
        'lblUpNote
        '
        Me.lblUpNote.AutoSize = True
        Me.lblUpNote.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUpNote.Location = New System.Drawing.Point(626, 15)
        Me.lblUpNote.Name = "lblUpNote"
        Me.lblUpNote.Size = New System.Drawing.Size(116, 16)
        Me.lblUpNote.TabIndex = 13
        Me.lblUpNote.Text = "出欠情報を表記"
        '
        'lblUp
        '
        Me.lblUp.AutoSize = True
        Me.lblUp.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUp.Location = New System.Drawing.Point(585, 15)
        Me.lblUp.Name = "lblUp"
        Me.lblUp.Size = New System.Drawing.Size(48, 16)
        Me.lblUp.TabIndex = 12
        Me.lblUp.Text = "上段："
        '
        'flxAttendance
        '
        Me.flxAttendance.ColumnInfo = resources.GetString("flxAttendance.ColumnInfo")
        Me.flxAttendance.Location = New System.Drawing.Point(7, 55)
        Me.flxAttendance.Name = "flxAttendance"
        Me.flxAttendance.Rows.DefaultSize = 22
        Me.flxAttendance.Size = New System.Drawing.Size(875, 445)
        Me.flxAttendance.StyleInfo = resources.GetString("flxAttendance.StyleInfo")
        Me.flxAttendance.TabIndex = 2
        '
        'lblMome
        '
        Me.lblMome.AutoSize = True
        Me.lblMome.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMome.Location = New System.Drawing.Point(262, 514)
        Me.lblMome.Name = "lblMome"
        Me.lblMome.Size = New System.Drawing.Size(605, 48)
        Me.lblMome.TabIndex = 1
        Me.lblMome.Text = "※中央執行委員会に所属している組合員は、[中執活動報告]画面から登録を行ってください。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "　 支部委員会（三役）に所属している組合員は、[支部委員会]の欄から登録" & _
            "を行ってください。" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "　 ２つ前の締め日以前の内容変更は出来ません。"
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnUpdate.Location = New System.Drawing.Point(839, 771)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(110, 30)
        Me.btnUpdate.TabIndex = 3
        Me.btnUpdate.Text = "内容変更"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnConfirm
        '
        Me.btnConfirm.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnConfirm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnConfirm.Location = New System.Drawing.Point(721, 771)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(110, 30)
        Me.btnConfirm.TabIndex = 5
        Me.btnConfirm.Text = "登録確認"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancel.Location = New System.Drawing.Point(839, 771)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 6
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblKiValue
        '
        Me.lblKiValue.AutoSize = True
        Me.lblKiValue.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblKiValue.Location = New System.Drawing.Point(259, 150)
        Me.lblKiValue.Name = "lblKiValue"
        Me.lblKiValue.Size = New System.Drawing.Size(17, 12)
        Me.lblKiValue.TabIndex = 12
        Me.lblKiValue.Text = "月"
        Me.lblKiValue.Visible = False
        '
        'UC020401
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblKiValue)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC020401"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        Me.grpResult.PerformLayout()
        CType(Me.flxAttendance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lblMonth As System.Windows.Forms.Label
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableEntryMonth As System.Windows.Forms.Label
    Friend WithEvents lblEntryMonth As System.Windows.Forms.Label
    Friend WithEvents cmbCommittee As System.Windows.Forms.ComboBox
    Friend WithEvents lblIndispensableCommittee As System.Windows.Forms.Label
    Friend WithEvents lblCommittee As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents lblMome As System.Windows.Forms.Label
    Friend WithEvents cmbMonth As System.Windows.Forms.ComboBox
    Friend WithEvents cmbYear As System.Windows.Forms.ComboBox
    Friend WithEvents flxAttendance As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnConfirm As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblKiValue As System.Windows.Forms.Label
    Friend WithEvents chkLunch As System.Windows.Forms.CheckBox
    Friend WithEvents lblDown As System.Windows.Forms.Label
    Friend WithEvents lblUpNote As System.Windows.Forms.Label
    Friend WithEvents lblUp As System.Windows.Forms.Label

End Class
