<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC090137
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC090137))
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.cmbComSeq = New System.Windows.Forms.ComboBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.lblIndispensableEntryMonth = New System.Windows.Forms.Label
        Me.lblCommitteeSeq = New System.Windows.Forms.Label
        Me.cmbCommittee = New System.Windows.Forms.ComboBox
        Me.lblIndispensableCommittee = New System.Windows.Forms.Label
        Me.lblCommittee = New System.Windows.Forms.Label
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmbComSeq2 = New System.Windows.Forms.ComboBox
        Me.flxAttendance = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.btnCopy = New System.Windows.Forms.Button
        Me.lblCommitteeSeq2 = New System.Windows.Forms.Label
        Me.lblCommittee2 = New System.Windows.Forms.Label
        Me.cmbCommittee2 = New System.Windows.Forms.ComboBox
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnConfirm = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
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
        Me.lblTitle.Text = "権限マスタメンテナンス"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.cmbComSeq)
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.lblIndispensableEntryMonth)
        Me.grpSearch.Controls.Add(Me.lblCommitteeSeq)
        Me.grpSearch.Controls.Add(Me.cmbCommittee)
        Me.grpSearch.Controls.Add(Me.lblIndispensableCommittee)
        Me.grpSearch.Controls.Add(Me.lblCommittee)
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.ForeColor = System.Drawing.Color.Blue
        Me.grpSearch.Location = New System.Drawing.Point(61, 79)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(894, 65)
        Me.grpSearch.TabIndex = 8
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "検索条件"
        '
        'cmbComSeq
        '
        Me.cmbComSeq.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbComSeq.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbComSeq.FormattingEnabled = True
        Me.cmbComSeq.Location = New System.Drawing.Point(506, 25)
        Me.cmbComSeq.Name = "cmbComSeq"
        Me.cmbComSeq.Size = New System.Drawing.Size(138, 24)
        Me.cmbComSeq.TabIndex = 2
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(681, 21)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'lblIndispensableEntryMonth
        '
        Me.lblIndispensableEntryMonth.AutoSize = True
        Me.lblIndispensableEntryMonth.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableEntryMonth.Location = New System.Drawing.Point(447, 30)
        Me.lblIndispensableEntryMonth.Name = "lblIndispensableEntryMonth"
        Me.lblIndispensableEntryMonth.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableEntryMonth.TabIndex = 6
        Me.lblIndispensableEntryMonth.Text = "*"
        '
        'lblCommitteeSeq
        '
        Me.lblCommitteeSeq.AutoSize = True
        Me.lblCommitteeSeq.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommitteeSeq.Location = New System.Drawing.Point(460, 30)
        Me.lblCommitteeSeq.Name = "lblCommitteeSeq"
        Me.lblCommitteeSeq.Size = New System.Drawing.Size(40, 16)
        Me.lblCommitteeSeq.TabIndex = 5
        Me.lblCommitteeSeq.Text = "役職"
        '
        'cmbCommittee
        '
        Me.cmbCommittee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCommittee.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbCommittee.FormattingEnabled = True
        Me.cmbCommittee.Location = New System.Drawing.Point(189, 25)
        Me.cmbCommittee.Name = "cmbCommittee"
        Me.cmbCommittee.Size = New System.Drawing.Size(219, 24)
        Me.cmbCommittee.TabIndex = 1
        '
        'lblIndispensableCommittee
        '
        Me.lblIndispensableCommittee.AutoSize = True
        Me.lblIndispensableCommittee.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableCommittee.Location = New System.Drawing.Point(72, 30)
        Me.lblIndispensableCommittee.Name = "lblIndispensableCommittee"
        Me.lblIndispensableCommittee.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableCommittee.TabIndex = 2
        Me.lblIndispensableCommittee.Text = "*"
        '
        'lblCommittee
        '
        Me.lblCommittee.AutoSize = True
        Me.lblCommittee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommittee.Location = New System.Drawing.Point(86, 30)
        Me.lblCommittee.Name = "lblCommittee"
        Me.lblCommittee.Size = New System.Drawing.Size(88, 16)
        Me.lblCommittee.TabIndex = 1
        Me.lblCommittee.Text = "部／委員会"
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.Label1)
        Me.grpResult.Controls.Add(Me.cmbComSeq2)
        Me.grpResult.Controls.Add(Me.flxAttendance)
        Me.grpResult.Controls.Add(Me.btnCopy)
        Me.grpResult.Controls.Add(Me.lblCommitteeSeq2)
        Me.grpResult.Controls.Add(Me.lblCommittee2)
        Me.grpResult.Controls.Add(Me.cmbCommittee2)
        Me.grpResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpResult.ForeColor = System.Drawing.Color.Blue
        Me.grpResult.Location = New System.Drawing.Point(61, 163)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(888, 586)
        Me.grpResult.TabIndex = 9
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "権限マスタ詳細"
        Me.grpResult.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(18, 35)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(201, 16)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "既存の委員会/役職からコピー"
        '
        'cmbComSeq2
        '
        Me.cmbComSeq2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbComSeq2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbComSeq2.FormattingEnabled = True
        Me.cmbComSeq2.Location = New System.Drawing.Point(615, 32)
        Me.cmbComSeq2.Name = "cmbComSeq2"
        Me.cmbComSeq2.Size = New System.Drawing.Size(138, 24)
        Me.cmbComSeq2.TabIndex = 9
        '
        'flxAttendance
        '
        Me.flxAttendance.ColumnInfo = resources.GetString("flxAttendance.ColumnInfo")
        Me.flxAttendance.Location = New System.Drawing.Point(7, 80)
        Me.flxAttendance.Name = "flxAttendance"
        Me.flxAttendance.Rows.DefaultSize = 22
        Me.flxAttendance.Size = New System.Drawing.Size(875, 500)
        Me.flxAttendance.StyleInfo = resources.GetString("flxAttendance.StyleInfo")
        Me.flxAttendance.TabIndex = 11
        '
        'btnCopy
        '
        Me.btnCopy.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCopy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCopy.Location = New System.Drawing.Point(769, 30)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(110, 30)
        Me.btnCopy.TabIndex = 10
        Me.btnCopy.Text = "コピー"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'lblCommitteeSeq2
        '
        Me.lblCommitteeSeq2.AutoSize = True
        Me.lblCommitteeSeq2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommitteeSeq2.Location = New System.Drawing.Point(569, 37)
        Me.lblCommitteeSeq2.Name = "lblCommitteeSeq2"
        Me.lblCommitteeSeq2.Size = New System.Drawing.Size(40, 16)
        Me.lblCommitteeSeq2.TabIndex = 5
        Me.lblCommitteeSeq2.Text = "役職"
        '
        'lblCommittee2
        '
        Me.lblCommittee2.AutoSize = True
        Me.lblCommittee2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCommittee2.Location = New System.Drawing.Point(238, 35)
        Me.lblCommittee2.Name = "lblCommittee2"
        Me.lblCommittee2.Size = New System.Drawing.Size(88, 16)
        Me.lblCommittee2.TabIndex = 1
        Me.lblCommittee2.Text = "部／委員会"
        '
        'cmbCommittee2
        '
        Me.cmbCommittee2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCommittee2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbCommittee2.FormattingEnabled = True
        Me.cmbCommittee2.Location = New System.Drawing.Point(332, 32)
        Me.cmbCommittee2.Name = "cmbCommittee2"
        Me.cmbCommittee2.Size = New System.Drawing.Size(219, 24)
        Me.cmbCommittee2.TabIndex = 8
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnUpdate.Location = New System.Drawing.Point(839, 771)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(110, 30)
        Me.btnUpdate.TabIndex = 5
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
        Me.btnConfirm.TabIndex = 6
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
        Me.btnCancel.TabIndex = 7
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'UC090137
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC090137"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        Me.grpResult.PerformLayout()
        CType(Me.flxAttendance, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lblIndispensableEntryMonth As System.Windows.Forms.Label
    Friend WithEvents cmbCommittee As System.Windows.Forms.ComboBox
    Friend WithEvents lblIndispensableCommittee As System.Windows.Forms.Label
    Friend WithEvents lblCommittee As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents flxAttendance As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnConfirm As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblCommitteeSeq As System.Windows.Forms.Label
    Friend WithEvents cmbComSeq As System.Windows.Forms.ComboBox
    Friend WithEvents cmbComSeq2 As System.Windows.Forms.ComboBox
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents lblCommitteeSeq2 As System.Windows.Forms.Label
    Friend WithEvents cmbCommittee2 As System.Windows.Forms.ComboBox
    Friend WithEvents lblCommittee2 As System.Windows.Forms.Label


    Friend WithEvents Label1 As System.Windows.Forms.Label

End Class
