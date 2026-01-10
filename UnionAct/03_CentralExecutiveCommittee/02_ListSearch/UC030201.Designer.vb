<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC030201
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
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.cboUnionBranchSearch = New System.Windows.Forms.ComboBox
        Me.lblUnionBranchSearch = New System.Windows.Forms.Label
        Me.lblMonthSearch = New System.Windows.Forms.Label
        Me.cboMonthSearch = New System.Windows.Forms.ComboBox
        Me.lblYearSearch = New System.Windows.Forms.Label
        Me.cboYearSearch = New System.Windows.Forms.ComboBox
        Me.lblIndispensableSearch = New System.Windows.Forms.Label
        Me.lblTargetSearch = New System.Windows.Forms.Label
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cboCommittee = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.cboPeriod = New System.Windows.Forms.ComboBox
        Me.btnHistory = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpCommittee = New System.Windows.Forms.GroupBox
        Me.lblMonthd = New System.Windows.Forms.Label
        Me.cboMonth = New System.Windows.Forms.ComboBox
        Me.cboTerm = New System.Windows.Forms.ComboBox
        Me.lblYear = New System.Windows.Forms.Label
        Me.cboYear = New System.Windows.Forms.ComboBox
        Me.lblIndispensable = New System.Windows.Forms.Label
        Me.lblTarget = New System.Windows.Forms.Label
        Me.btnPrintCommittee = New System.Windows.Forms.Button
        Me.lblIndispensableTerm = New System.Windows.Forms.Label
        Me.lblTerm = New System.Windows.Forms.Label
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.dgdResult = New System.Windows.Forms.DataGridView
        Me.btnPrint = New System.Windows.Forms.Button
        Me.grpSearch.SuspendLayout()
        Me.grpCommittee.SuspendLayout()
        Me.grpResult.SuspendLayout()
        CType(Me.dgdResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpSearch
        '
        Me.grpSearch.Controls.Add(Me.cboUnionBranchSearch)
        Me.grpSearch.Controls.Add(Me.lblUnionBranchSearch)
        Me.grpSearch.Controls.Add(Me.lblMonthSearch)
        Me.grpSearch.Controls.Add(Me.cboMonthSearch)
        Me.grpSearch.Controls.Add(Me.lblYearSearch)
        Me.grpSearch.Controls.Add(Me.cboYearSearch)
        Me.grpSearch.Controls.Add(Me.lblIndispensableSearch)
        Me.grpSearch.Controls.Add(Me.lblTargetSearch)
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.cboCommittee)
        Me.grpSearch.Controls.Add(Me.Label4)
        Me.grpSearch.Controls.Add(Me.Label3)
        Me.grpSearch.Controls.Add(Me.cboPeriod)
        Me.grpSearch.Controls.Add(Me.btnHistory)
        Me.grpSearch.Controls.Add(Me.Label2)
        Me.grpSearch.Controls.Add(Me.Label1)
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.ForeColor = System.Drawing.Color.Blue
        Me.grpSearch.Location = New System.Drawing.Point(81, 93)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(846, 115)
        Me.grpSearch.TabIndex = 9
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "検索条件"
        '
        'cboUnionBranchSearch
        '
        Me.cboUnionBranchSearch.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboUnionBranchSearch.FormattingEnabled = True
        Me.cboUnionBranchSearch.Location = New System.Drawing.Point(476, 71)
        Me.cboUnionBranchSearch.Name = "cboUnionBranchSearch"
        Me.cboUnionBranchSearch.Size = New System.Drawing.Size(134, 24)
        Me.cboUnionBranchSearch.TabIndex = 4
        '
        'lblUnionBranchSearch
        '
        Me.lblUnionBranchSearch.AutoSize = True
        Me.lblUnionBranchSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblUnionBranchSearch.Location = New System.Drawing.Point(381, 74)
        Me.lblUnionBranchSearch.Name = "lblUnionBranchSearch"
        Me.lblUnionBranchSearch.Size = New System.Drawing.Size(72, 16)
        Me.lblUnionBranchSearch.TabIndex = 18
        Me.lblUnionBranchSearch.Text = "組合支部"
        '
        'lblMonthSearch
        '
        Me.lblMonthSearch.AutoSize = True
        Me.lblMonthSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonthSearch.Location = New System.Drawing.Point(298, 74)
        Me.lblMonthSearch.Name = "lblMonthSearch"
        Me.lblMonthSearch.Size = New System.Drawing.Size(24, 16)
        Me.lblMonthSearch.TabIndex = 17
        Me.lblMonthSearch.Text = "月"
        '
        'cboMonthSearch
        '
        Me.cboMonthSearch.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboMonthSearch.FormattingEnabled = True
        Me.cboMonthSearch.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cboMonthSearch.Location = New System.Drawing.Point(237, 71)
        Me.cboMonthSearch.Name = "cboMonthSearch"
        Me.cboMonthSearch.Size = New System.Drawing.Size(55, 24)
        Me.cboMonthSearch.TabIndex = 3
        '
        'lblYearSearch
        '
        Me.lblYearSearch.AutoSize = True
        Me.lblYearSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYearSearch.Location = New System.Drawing.Point(214, 74)
        Me.lblYearSearch.Name = "lblYearSearch"
        Me.lblYearSearch.Size = New System.Drawing.Size(24, 16)
        Me.lblYearSearch.TabIndex = 15
        Me.lblYearSearch.Text = "年"
        '
        'cboYearSearch
        '
        Me.cboYearSearch.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboYearSearch.FormattingEnabled = True
        Me.cboYearSearch.Location = New System.Drawing.Point(135, 71)
        Me.cboYearSearch.Name = "cboYearSearch"
        Me.cboYearSearch.Size = New System.Drawing.Size(73, 24)
        Me.cboYearSearch.TabIndex = 2
        '
        'lblIndispensableSearch
        '
        Me.lblIndispensableSearch.AutoSize = True
        Me.lblIndispensableSearch.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableSearch.Location = New System.Drawing.Point(40, 74)
        Me.lblIndispensableSearch.Name = "lblIndispensableSearch"
        Me.lblIndispensableSearch.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableSearch.TabIndex = 13
        Me.lblIndispensableSearch.Text = "*"
        '
        'lblTargetSearch
        '
        Me.lblTargetSearch.AutoSize = True
        Me.lblTargetSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTargetSearch.Location = New System.Drawing.Point(57, 74)
        Me.lblTargetSearch.Name = "lblTargetSearch"
        Me.lblTargetSearch.Size = New System.Drawing.Size(72, 16)
        Me.lblTargetSearch.TabIndex = 12
        Me.lblTargetSearch.Text = "対象年月"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(712, 70)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 6
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cboCommittee
        '
        Me.cboCommittee.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboCommittee.FormattingEnabled = True
        Me.cboCommittee.Location = New System.Drawing.Point(476, 27)
        Me.cboCommittee.Name = "cboCommittee"
        Me.cboCommittee.Size = New System.Drawing.Size(213, 24)
        Me.cboCommittee.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(364, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(16, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "*"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(381, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "部／委員会"
        '
        'cboPeriod
        '
        Me.cboPeriod.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboPeriod.FormattingEnabled = True
        Me.cboPeriod.Location = New System.Drawing.Point(135, 27)
        Me.cboPeriod.Name = "cboPeriod"
        Me.cboPeriod.Size = New System.Drawing.Size(103, 24)
        Me.cboPeriod.TabIndex = 0
        '
        'btnHistory
        '
        Me.btnHistory.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnHistory.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnHistory.Location = New System.Drawing.Point(712, 26)
        Me.btnHistory.Name = "btnHistory"
        Me.btnHistory.Size = New System.Drawing.Size(80, 30)
        Me.btnHistory.TabIndex = 5
        Me.btnHistory.Text = "履歴"
        Me.btnHistory.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(40, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(57, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(24, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "期"
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
        Me.lblTitle.Text = "期別名簿検索"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpCommittee
        '
        Me.grpCommittee.Controls.Add(Me.lblMonthd)
        Me.grpCommittee.Controls.Add(Me.cboMonth)
        Me.grpCommittee.Controls.Add(Me.cboTerm)
        Me.grpCommittee.Controls.Add(Me.lblYear)
        Me.grpCommittee.Controls.Add(Me.cboYear)
        Me.grpCommittee.Controls.Add(Me.lblIndispensable)
        Me.grpCommittee.Controls.Add(Me.lblTarget)
        Me.grpCommittee.Controls.Add(Me.btnPrintCommittee)
        Me.grpCommittee.Controls.Add(Me.lblIndispensableTerm)
        Me.grpCommittee.Controls.Add(Me.lblTerm)
        Me.grpCommittee.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpCommittee.ForeColor = System.Drawing.Color.Blue
        Me.grpCommittee.Location = New System.Drawing.Point(81, 725)
        Me.grpCommittee.Name = "grpCommittee"
        Me.grpCommittee.Size = New System.Drawing.Size(846, 53)
        Me.grpCommittee.TabIndex = 20
        Me.grpCommittee.TabStop = False
        Me.grpCommittee.Text = "専門委員・部会名簿印刷"
        '
        'lblMonthd
        '
        Me.lblMonthd.AutoSize = True
        Me.lblMonthd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonthd.Location = New System.Drawing.Point(537, 23)
        Me.lblMonthd.Name = "lblMonthd"
        Me.lblMonthd.Size = New System.Drawing.Size(24, 16)
        Me.lblMonthd.TabIndex = 17
        Me.lblMonthd.Text = "月"
        '
        'cboMonth
        '
        Me.cboMonth.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboMonth.FormattingEnabled = True
        Me.cboMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cboMonth.Location = New System.Drawing.Point(476, 20)
        Me.cboMonth.Name = "cboMonth"
        Me.cboMonth.Size = New System.Drawing.Size(55, 24)
        Me.cboMonth.TabIndex = 11
        '
        'cboTerm
        '
        Me.cboTerm.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboTerm.FormattingEnabled = True
        Me.cboTerm.Location = New System.Drawing.Point(87, 20)
        Me.cboTerm.Name = "cboTerm"
        Me.cboTerm.Size = New System.Drawing.Size(103, 24)
        Me.cboTerm.TabIndex = 9
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYear.Location = New System.Drawing.Point(453, 23)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(24, 16)
        Me.lblYear.TabIndex = 15
        Me.lblYear.Text = "年"
        '
        'cboYear
        '
        Me.cboYear.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboYear.FormattingEnabled = True
        Me.cboYear.Location = New System.Drawing.Point(366, 20)
        Me.cboYear.Name = "cboYear"
        Me.cboYear.Size = New System.Drawing.Size(81, 24)
        Me.cboYear.TabIndex = 10
        '
        'lblIndispensable
        '
        Me.lblIndispensable.AutoSize = True
        Me.lblIndispensable.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensable.Location = New System.Drawing.Point(271, 23)
        Me.lblIndispensable.Name = "lblIndispensable"
        Me.lblIndispensable.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensable.TabIndex = 13
        Me.lblIndispensable.Text = "*"
        '
        'lblTarget
        '
        Me.lblTarget.AutoSize = True
        Me.lblTarget.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTarget.Location = New System.Drawing.Point(288, 23)
        Me.lblTarget.Name = "lblTarget"
        Me.lblTarget.Size = New System.Drawing.Size(72, 16)
        Me.lblTarget.TabIndex = 12
        Me.lblTarget.Text = "対象年月"
        '
        'btnPrintCommittee
        '
        Me.btnPrintCommittee.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrintCommittee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPrintCommittee.Location = New System.Drawing.Point(657, 17)
        Me.btnPrintCommittee.Name = "btnPrintCommittee"
        Me.btnPrintCommittee.Size = New System.Drawing.Size(110, 30)
        Me.btnPrintCommittee.TabIndex = 12
        Me.btnPrintCommittee.Text = "印刷"
        Me.btnPrintCommittee.UseVisualStyleBackColor = True
        '
        'lblIndispensableTerm
        '
        Me.lblIndispensableTerm.AutoSize = True
        Me.lblIndispensableTerm.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableTerm.Location = New System.Drawing.Point(40, 23)
        Me.lblIndispensableTerm.Name = "lblIndispensableTerm"
        Me.lblIndispensableTerm.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableTerm.TabIndex = 2
        Me.lblIndispensableTerm.Text = "*"
        '
        'lblTerm
        '
        Me.lblTerm.AutoSize = True
        Me.lblTerm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblTerm.Location = New System.Drawing.Point(57, 23)
        Me.lblTerm.Name = "lblTerm"
        Me.lblTerm.Size = New System.Drawing.Size(24, 16)
        Me.lblTerm.TabIndex = 1
        Me.lblTerm.Text = "期"
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.dgdResult)
        Me.grpResult.Controls.Add(Me.btnPrint)
        Me.grpResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpResult.ForeColor = System.Drawing.Color.Blue
        Me.grpResult.Location = New System.Drawing.Point(81, 237)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(846, 457)
        Me.grpResult.TabIndex = 7
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "検索結果（ xx 件）"
        Me.grpResult.Visible = False
        '
        'dgdResult
        '
        Me.dgdResult.AllowUserToAddRows = False
        Me.dgdResult.AllowUserToDeleteRows = False
        Me.dgdResult.AllowUserToResizeRows = False
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgdResult.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.dgdResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgdResult.Location = New System.Drawing.Point(60, 22)
        Me.dgdResult.MultiSelect = False
        Me.dgdResult.Name = "dgdResult"
        Me.dgdResult.ReadOnly = True
        Me.dgdResult.RowHeadersVisible = False
        Me.dgdResult.RowTemplate.Height = 21
        Me.dgdResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdResult.Size = New System.Drawing.Size(707, 366)
        Me.dgdResult.StandardTab = True
        Me.dgdResult.TabIndex = 7
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPrint.Location = New System.Drawing.Point(657, 404)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 8
        Me.btnPrint.Text = "印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'UC030201
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.grpCommittee)
        Me.Controls.Add(Me.grpSearch)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC030201"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.grpCommittee.ResumeLayout(False)
        Me.grpCommittee.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        CType(Me.dgdResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cboCommittee As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents btnHistory As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents cboMonthSearch As System.Windows.Forms.ComboBox
    Friend WithEvents lblYearSearch As System.Windows.Forms.Label
    Friend WithEvents cboYearSearch As System.Windows.Forms.ComboBox
    Friend WithEvents lblIndispensableSearch As System.Windows.Forms.Label
    Friend WithEvents lblTargetSearch As System.Windows.Forms.Label
    Friend WithEvents cboUnionBranchSearch As System.Windows.Forms.ComboBox
    Friend WithEvents lblUnionBranchSearch As System.Windows.Forms.Label
    Friend WithEvents lblMonthSearch As System.Windows.Forms.Label
    Friend WithEvents grpCommittee As System.Windows.Forms.GroupBox
    Friend WithEvents lblMonthd As System.Windows.Forms.Label
    Friend WithEvents cboMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents cboYear As System.Windows.Forms.ComboBox
    Friend WithEvents lblIndispensable As System.Windows.Forms.Label
    Friend WithEvents lblTarget As System.Windows.Forms.Label
    Friend WithEvents btnPrintCommittee As System.Windows.Forms.Button
    Friend WithEvents cboTerm As System.Windows.Forms.ComboBox
    Friend WithEvents lblIndispensableTerm As System.Windows.Forms.Label
    Friend WithEvents lblTerm As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents dgdResult As System.Windows.Forms.DataGridView
    Friend WithEvents btnPrint As System.Windows.Forms.Button

End Class
