Imports C1.Win.C1FlexGrid
Imports UnionAct.GUI.FinancialAffairs
Imports UnionAct.Business.FinancialAffairs.WageReduction

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC050101
    Inherits UnionAct.Business.FinancialAffairs.FinancialAffairsBase

    'UserControl はコンポーネント一覧をクリーンアップするために dispose をオーバーライドします。
    '<System.Diagnostics.DebuggerNonUserCode()> _
    'Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    '    Try
    '        If disposing AndAlso components IsNot Nothing Then
    '            components.Dispose()
    '        End If
    '    Finally
    '        MyBase.Dispose(disposing)
    '    End Try
    'End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC050101))
        Me.tabWageReduction = New System.Windows.Forms.TabControl
        Me.tabMonthly = New System.Windows.Forms.TabPage
        Me.pnlResult = New System.Windows.Forms.Panel
        Me.btnPrintMonthlyDetail = New System.Windows.Forms.Button
        Me.btnPrintMonthlySummary = New System.Windows.Forms.Button
        Me.grpPersonalStrike = New System.Windows.Forms.GroupBox
        Me.flxStrike = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.btnNewStrike = New System.Windows.Forms.Button
        Me.btnRefStrike = New System.Windows.Forms.Button
        Me.grpInTime = New System.Windows.Forms.GroupBox
        Me.btnNewInTime = New System.Windows.Forms.Button
        Me.btnRefInTime = New System.Windows.Forms.Button
        Me.flxInTime = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.grpCondtion = New System.Windows.Forms.GroupBox
        Me.cmbMonthlyYear = New System.Windows.Forms.ComboBox
        Me.label6 = New System.Windows.Forms.Label
        Me.cmbMonthlyMonth = New System.Windows.Forms.ComboBox
        Me.label7 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.btnMonthlySearch = New System.Windows.Forms.Button
        Me.tabBonus = New System.Windows.Forms.TabPage
        Me.grpBonusResult = New System.Windows.Forms.GroupBox
        Me.btnPrintBonusDetail = New System.Windows.Forms.Button
        Me.btnPrintBonusTotal = New System.Windows.Forms.Button
        Me.btnNewBonus = New System.Windows.Forms.Button
        Me.btnRefBonus = New System.Windows.Forms.Button
        Me.flxBonus = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.grpBonusQueryCondition = New System.Windows.Forms.GroupBox
        Me.cmbCutOnceName = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cmbBonusMonth = New System.Windows.Forms.ComboBox
        Me.label2 = New System.Windows.Forms.Label
        Me.cmbBonusYear = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.btnBonusSearch = New System.Windows.Forms.Button
        Me.tabSumUp = New System.Windows.Forms.TabPage
        Me.grpSumUpNonTaxable = New System.Windows.Forms.GroupBox
        Me.btnPrintListSumUp = New System.Windows.Forms.Button
        Me.btnPrintSummarySumUp = New System.Windows.Forms.Button
        Me.btnRefSumUp = New System.Windows.Forms.Button
        Me.flxSumUp = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbSumUpYear = New System.Windows.Forms.ComboBox
        Me.label4 = New System.Windows.Forms.Label
        Me.label8 = New System.Windows.Forms.Label
        Me.btnSumUpQuery = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.tabWageReduction.SuspendLayout()
        Me.tabMonthly.SuspendLayout()
        Me.pnlResult.SuspendLayout()
        Me.grpPersonalStrike.SuspendLayout()
        CType(Me.flxStrike, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpInTime.SuspendLayout()
        CType(Me.flxInTime, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCondtion.SuspendLayout()
        Me.tabBonus.SuspendLayout()
        Me.grpBonusResult.SuspendLayout()
        CType(Me.flxBonus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpBonusQueryCondition.SuspendLayout()
        Me.tabSumUp.SuspendLayout()
        Me.grpSumUpNonTaxable.SuspendLayout()
        CType(Me.flxSumUp, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabWageReduction
        '
        Me.tabWageReduction.Controls.Add(Me.tabMonthly)
        Me.tabWageReduction.Controls.Add(Me.tabBonus)
        Me.tabWageReduction.Controls.Add(Me.tabSumUp)
        Me.tabWageReduction.ItemSize = New System.Drawing.Size(58, 21)
        Me.tabWageReduction.Location = New System.Drawing.Point(13, 70)
        Me.tabWageReduction.Margin = New System.Windows.Forms.Padding(4)
        Me.tabWageReduction.Name = "tabWageReduction"
        Me.tabWageReduction.SelectedIndex = 0
        Me.tabWageReduction.Size = New System.Drawing.Size(998, 729)
        Me.tabWageReduction.TabIndex = 0
        '
        'tabMonthly
        '
        Me.tabMonthly.Controls.Add(Me.pnlResult)
        Me.tabMonthly.Controls.Add(Me.grpCondtion)
        Me.tabMonthly.Location = New System.Drawing.Point(4, 25)
        Me.tabMonthly.Margin = New System.Windows.Forms.Padding(4)
        Me.tabMonthly.Name = "tabMonthly"
        Me.tabMonthly.Padding = New System.Windows.Forms.Padding(4)
        Me.tabMonthly.Size = New System.Drawing.Size(990, 700)
        Me.tabMonthly.TabIndex = 1
        Me.tabMonthly.Text = " 月例"
        Me.tabMonthly.UseVisualStyleBackColor = True
        '
        'pnlResult
        '
        Me.pnlResult.Controls.Add(Me.btnPrintMonthlyDetail)
        Me.pnlResult.Controls.Add(Me.btnPrintMonthlySummary)
        Me.pnlResult.Controls.Add(Me.grpPersonalStrike)
        Me.pnlResult.Controls.Add(Me.grpInTime)
        Me.pnlResult.Location = New System.Drawing.Point(149, 86)
        Me.pnlResult.Name = "pnlResult"
        Me.pnlResult.Size = New System.Drawing.Size(693, 588)
        Me.pnlResult.TabIndex = 1
        '
        'btnPrintMonthlyDetail
        '
        Me.btnPrintMonthlyDetail.Location = New System.Drawing.Point(186, 524)
        Me.btnPrintMonthlyDetail.Name = "btnPrintMonthlyDetail"
        Me.btnPrintMonthlyDetail.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintMonthlyDetail.TabIndex = 2
        Me.btnPrintMonthlyDetail.Text = "一覧プレ印刷"
        Me.btnPrintMonthlyDetail.UseVisualStyleBackColor = True
        '
        'btnPrintMonthlySummary
        '
        Me.btnPrintMonthlySummary.Location = New System.Drawing.Point(378, 524)
        Me.btnPrintMonthlySummary.Name = "btnPrintMonthlySummary"
        Me.btnPrintMonthlySummary.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintMonthlySummary.TabIndex = 3
        Me.btnPrintMonthlySummary.Text = "合計プレ印刷"
        Me.btnPrintMonthlySummary.UseVisualStyleBackColor = True
        '
        'grpPersonalStrike
        '
        Me.grpPersonalStrike.Controls.Add(Me.flxStrike)
        Me.grpPersonalStrike.Controls.Add(Me.btnNewStrike)
        Me.grpPersonalStrike.Controls.Add(Me.btnRefStrike)
        Me.grpPersonalStrike.Location = New System.Drawing.Point(21, 271)
        Me.grpPersonalStrike.Name = "grpPersonalStrike"
        Me.grpPersonalStrike.Size = New System.Drawing.Size(651, 177)
        Me.grpPersonalStrike.TabIndex = 1
        Me.grpPersonalStrike.TabStop = False
        Me.grpPersonalStrike.Text = "争議行為"
        '
        'flxStrike
        '
        Me.flxStrike.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxStrike.AllowEditing = False
        Me.flxStrike.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxStrike.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxStrike.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxStrike.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxStrike.Location = New System.Drawing.Point(39, 30)
        Me.flxStrike.Name = "flxStrike"
        Me.flxStrike.Rows.Count = 1
        Me.flxStrike.Rows.DefaultSize = 22
        Me.flxStrike.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxStrike.Size = New System.Drawing.Size(572, 90)
        Me.flxStrike.StyleInfo = resources.GetString("flxStrike.StyleInfo")
        Me.flxStrike.TabIndex = 3
        '
        'btnNewStrike
        '
        Me.btnNewStrike.Location = New System.Drawing.Point(357, 131)
        Me.btnNewStrike.Name = "btnNewStrike"
        Me.btnNewStrike.Size = New System.Drawing.Size(116, 32)
        Me.btnNewStrike.TabIndex = 2
        Me.btnNewStrike.Text = "新規登録"
        Me.btnNewStrike.UseVisualStyleBackColor = True
        '
        'btnRefStrike
        '
        Me.btnRefStrike.Location = New System.Drawing.Point(165, 131)
        Me.btnRefStrike.Name = "btnRefStrike"
        Me.btnRefStrike.Size = New System.Drawing.Size(116, 32)
        Me.btnRefStrike.TabIndex = 1
        Me.btnRefStrike.Text = "照会"
        Me.btnRefStrike.UseVisualStyleBackColor = True
        '
        'grpInTime
        '
        Me.grpInTime.Controls.Add(Me.btnNewInTime)
        Me.grpInTime.Controls.Add(Me.btnRefInTime)
        Me.grpInTime.Controls.Add(Me.flxInTime)
        Me.grpInTime.Location = New System.Drawing.Point(21, 34)
        Me.grpInTime.Name = "grpInTime"
        Me.grpInTime.Size = New System.Drawing.Size(651, 177)
        Me.grpInTime.TabIndex = 0
        Me.grpInTime.TabStop = False
        Me.grpInTime.Text = "時間内"
        '
        'btnNewInTime
        '
        Me.btnNewInTime.Location = New System.Drawing.Point(357, 133)
        Me.btnNewInTime.Name = "btnNewInTime"
        Me.btnNewInTime.Size = New System.Drawing.Size(116, 32)
        Me.btnNewInTime.TabIndex = 2
        Me.btnNewInTime.Text = "新規登録"
        Me.btnNewInTime.UseVisualStyleBackColor = True
        '
        'btnRefInTime
        '
        Me.btnRefInTime.Location = New System.Drawing.Point(165, 133)
        Me.btnRefInTime.Name = "btnRefInTime"
        Me.btnRefInTime.Size = New System.Drawing.Size(116, 32)
        Me.btnRefInTime.TabIndex = 1
        Me.btnRefInTime.Text = "照会"
        Me.btnRefInTime.UseVisualStyleBackColor = True
        '
        'flxInTime
        '
        Me.flxInTime.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxInTime.AllowEditing = False
        Me.flxInTime.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxInTime.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxInTime.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxInTime.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxInTime.Location = New System.Drawing.Point(39, 30)
        Me.flxInTime.Name = "flxInTime"
        Me.flxInTime.Rows.Count = 1
        Me.flxInTime.Rows.DefaultSize = 22
        Me.flxInTime.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxInTime.Size = New System.Drawing.Size(572, 90)
        Me.flxInTime.StyleInfo = resources.GetString("flxInTime.StyleInfo")
        Me.flxInTime.TabIndex = 0
        '
        'grpCondtion
        '
        Me.grpCondtion.Controls.Add(Me.cmbMonthlyYear)
        Me.grpCondtion.Controls.Add(Me.label6)
        Me.grpCondtion.Controls.Add(Me.cmbMonthlyMonth)
        Me.grpCondtion.Controls.Add(Me.label7)
        Me.grpCondtion.Controls.Add(Me.label5)
        Me.grpCondtion.Controls.Add(Me.btnMonthlySearch)
        Me.grpCondtion.Location = New System.Drawing.Point(170, 13)
        Me.grpCondtion.Name = "grpCondtion"
        Me.grpCondtion.Size = New System.Drawing.Size(651, 59)
        Me.grpCondtion.TabIndex = 0
        Me.grpCondtion.TabStop = False
        Me.grpCondtion.Text = "検索条件"
        '
        'cmbMonthlyYear
        '
        Me.cmbMonthlyYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMonthlyYear.FormattingEnabled = True
        Me.cmbMonthlyYear.Items.AddRange(New Object() {"2006", "2007", "2008", "2009", "2010"})
        Me.cmbMonthlyYear.Location = New System.Drawing.Point(208, 21)
        Me.cmbMonthlyYear.Name = "cmbMonthlyYear"
        Me.cmbMonthlyYear.Size = New System.Drawing.Size(63, 24)
        Me.cmbMonthlyYear.TabIndex = 1
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(277, 25)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(24, 16)
        Me.label6.TabIndex = 2
        Me.label6.Text = "年"
        '
        'cmbMonthlyMonth
        '
        Me.cmbMonthlyMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbMonthlyMonth.FormattingEnabled = True
        Me.cmbMonthlyMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cmbMonthlyMonth.Location = New System.Drawing.Point(302, 21)
        Me.cmbMonthlyMonth.MaxDropDownItems = 12
        Me.cmbMonthlyMonth.Name = "cmbMonthlyMonth"
        Me.cmbMonthlyMonth.Size = New System.Drawing.Size(50, 24)
        Me.cmbMonthlyMonth.TabIndex = 3
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.Location = New System.Drawing.Point(358, 26)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(24, 16)
        Me.label7.TabIndex = 4
        Me.label7.Text = "月"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Location = New System.Drawing.Point(140, 25)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(72, 16)
        Me.label5.TabIndex = 0
        Me.label5.Text = "対象年月"
        '
        'btnMonthlySearch
        '
        Me.btnMonthlySearch.Location = New System.Drawing.Point(395, 17)
        Me.btnMonthlySearch.Name = "btnMonthlySearch"
        Me.btnMonthlySearch.Size = New System.Drawing.Size(116, 32)
        Me.btnMonthlySearch.TabIndex = 5
        Me.btnMonthlySearch.Text = "検索"
        Me.btnMonthlySearch.UseVisualStyleBackColor = True
        '
        'tabBonus
        '
        Me.tabBonus.Controls.Add(Me.grpBonusResult)
        Me.tabBonus.Controls.Add(Me.grpBonusQueryCondition)
        Me.tabBonus.Location = New System.Drawing.Point(4, 25)
        Me.tabBonus.Name = "tabBonus"
        Me.tabBonus.Size = New System.Drawing.Size(990, 700)
        Me.tabBonus.TabIndex = 2
        Me.tabBonus.Text = "一時金"
        Me.tabBonus.UseVisualStyleBackColor = True
        '
        'grpBonusResult
        '
        Me.grpBonusResult.Controls.Add(Me.btnPrintBonusDetail)
        Me.grpBonusResult.Controls.Add(Me.btnPrintBonusTotal)
        Me.grpBonusResult.Controls.Add(Me.btnNewBonus)
        Me.grpBonusResult.Controls.Add(Me.btnRefBonus)
        Me.grpBonusResult.Controls.Add(Me.flxBonus)
        Me.grpBonusResult.Location = New System.Drawing.Point(40, 143)
        Me.grpBonusResult.Name = "grpBonusResult"
        Me.grpBonusResult.Size = New System.Drawing.Size(911, 266)
        Me.grpBonusResult.TabIndex = 1
        Me.grpBonusResult.TabStop = False
        '
        'btnPrintBonusDetail
        '
        Me.btnPrintBonusDetail.Location = New System.Drawing.Point(397, 151)
        Me.btnPrintBonusDetail.Name = "btnPrintBonusDetail"
        Me.btnPrintBonusDetail.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintBonusDetail.TabIndex = 2
        Me.btnPrintBonusDetail.Text = "一覧プレ印刷"
        Me.btnPrintBonusDetail.UseVisualStyleBackColor = True
        '
        'btnPrintBonusTotal
        '
        Me.btnPrintBonusTotal.Location = New System.Drawing.Point(555, 151)
        Me.btnPrintBonusTotal.Name = "btnPrintBonusTotal"
        Me.btnPrintBonusTotal.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintBonusTotal.TabIndex = 3
        Me.btnPrintBonusTotal.Text = "合計プレ印刷"
        Me.btnPrintBonusTotal.UseVisualStyleBackColor = True
        '
        'btnNewBonus
        '
        Me.btnNewBonus.Location = New System.Drawing.Point(397, 211)
        Me.btnNewBonus.Name = "btnNewBonus"
        Me.btnNewBonus.Size = New System.Drawing.Size(116, 32)
        Me.btnNewBonus.TabIndex = 4
        Me.btnNewBonus.Text = "新規登録"
        Me.btnNewBonus.UseVisualStyleBackColor = True
        '
        'btnRefBonus
        '
        Me.btnRefBonus.Location = New System.Drawing.Point(239, 151)
        Me.btnRefBonus.Name = "btnRefBonus"
        Me.btnRefBonus.Size = New System.Drawing.Size(116, 32)
        Me.btnRefBonus.TabIndex = 1
        Me.btnRefBonus.Text = "照会"
        Me.btnRefBonus.UseVisualStyleBackColor = True
        '
        'flxBonus
        '
        Me.flxBonus.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxBonus.AllowEditing = False
        Me.flxBonus.AllowResizing = C1.Win.C1FlexGrid.AllowResizingEnum.None
        Me.flxBonus.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxBonus.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxBonus.ColumnInfo = "10,1,0,0,0,110,Columns:"
        Me.flxBonus.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxBonus.Location = New System.Drawing.Point(19, 40)
        Me.flxBonus.Name = "flxBonus"
        Me.flxBonus.Rows.Count = 1
        Me.flxBonus.Rows.DefaultSize = 22
        Me.flxBonus.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxBonus.Size = New System.Drawing.Size(872, 90)
        Me.flxBonus.StyleInfo = resources.GetString("flxBonus.StyleInfo")
        Me.flxBonus.TabIndex = 0
        '
        'grpBonusQueryCondition
        '
        Me.grpBonusQueryCondition.Controls.Add(Me.cmbCutOnceName)
        Me.grpBonusQueryCondition.Controls.Add(Me.Label9)
        Me.grpBonusQueryCondition.Controls.Add(Me.cmbBonusMonth)
        Me.grpBonusQueryCondition.Controls.Add(Me.label2)
        Me.grpBonusQueryCondition.Controls.Add(Me.cmbBonusYear)
        Me.grpBonusQueryCondition.Controls.Add(Me.label1)
        Me.grpBonusQueryCondition.Controls.Add(Me.label3)
        Me.grpBonusQueryCondition.Controls.Add(Me.btnBonusSearch)
        Me.grpBonusQueryCondition.Location = New System.Drawing.Point(170, 13)
        Me.grpBonusQueryCondition.Name = "grpBonusQueryCondition"
        Me.grpBonusQueryCondition.Size = New System.Drawing.Size(651, 107)
        Me.grpBonusQueryCondition.TabIndex = 0
        Me.grpBonusQueryCondition.TabStop = False
        Me.grpBonusQueryCondition.Text = "検索条件"
        '
        'cmbCutOnceName
        '
        Me.cmbCutOnceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCutOnceName.FormattingEnabled = True
        Me.cmbCutOnceName.Location = New System.Drawing.Point(229, 65)
        Me.cmbCutOnceName.Name = "cmbCutOnceName"
        Me.cmbCutOnceName.Size = New System.Drawing.Size(153, 24)
        Me.cmbCutOnceName.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(140, 69)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 16)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "一時金名称"
        '
        'cmbBonusMonth
        '
        Me.cmbBonusMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBonusMonth.FormattingEnabled = True
        Me.cmbBonusMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cmbBonusMonth.Location = New System.Drawing.Point(302, 21)
        Me.cmbBonusMonth.MaxDropDownItems = 12
        Me.cmbBonusMonth.Name = "cmbBonusMonth"
        Me.cmbBonusMonth.Size = New System.Drawing.Size(50, 24)
        Me.cmbBonusMonth.TabIndex = 3
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(358, 26)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(24, 16)
        Me.label2.TabIndex = 4
        Me.label2.Text = "月"
        '
        'cmbBonusYear
        '
        Me.cmbBonusYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBonusYear.FormattingEnabled = True
        Me.cmbBonusYear.Items.AddRange(New Object() {"2006", "2007", "2008", "2009", "2010"})
        Me.cmbBonusYear.Location = New System.Drawing.Point(208, 21)
        Me.cmbBonusYear.Name = "cmbBonusYear"
        Me.cmbBonusYear.Size = New System.Drawing.Size(63, 24)
        Me.cmbBonusYear.TabIndex = 1
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(277, 25)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(24, 16)
        Me.label1.TabIndex = 2
        Me.label1.Text = "年"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(140, 25)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(72, 16)
        Me.label3.TabIndex = 0
        Me.label3.Text = "対象年月"
        '
        'btnBonusSearch
        '
        Me.btnBonusSearch.Location = New System.Drawing.Point(395, 59)
        Me.btnBonusSearch.Name = "btnBonusSearch"
        Me.btnBonusSearch.Size = New System.Drawing.Size(116, 32)
        Me.btnBonusSearch.TabIndex = 7
        Me.btnBonusSearch.Text = "検索"
        Me.btnBonusSearch.UseVisualStyleBackColor = True
        '
        'tabSumUp
        '
        Me.tabSumUp.Controls.Add(Me.grpSumUpNonTaxable)
        Me.tabSumUp.Controls.Add(Me.groupBox1)
        Me.tabSumUp.Location = New System.Drawing.Point(4, 25)
        Me.tabSumUp.Name = "tabSumUp"
        Me.tabSumUp.Size = New System.Drawing.Size(990, 700)
        Me.tabSumUp.TabIndex = 3
        Me.tabSumUp.Text = " 累計"
        Me.tabSumUp.UseVisualStyleBackColor = True
        '
        'grpSumUpNonTaxable
        '
        Me.grpSumUpNonTaxable.Controls.Add(Me.btnPrintListSumUp)
        Me.grpSumUpNonTaxable.Controls.Add(Me.btnPrintSummarySumUp)
        Me.grpSumUpNonTaxable.Controls.Add(Me.btnRefSumUp)
        Me.grpSumUpNonTaxable.Controls.Add(Me.flxSumUp)
        Me.grpSumUpNonTaxable.Location = New System.Drawing.Point(9, 110)
        Me.grpSumUpNonTaxable.Name = "grpSumUpNonTaxable"
        Me.grpSumUpNonTaxable.Size = New System.Drawing.Size(973, 227)
        Me.grpSumUpNonTaxable.TabIndex = 1
        Me.grpSumUpNonTaxable.TabStop = False
        '
        'btnPrintListSumUp
        '
        Me.btnPrintListSumUp.Location = New System.Drawing.Point(428, 170)
        Me.btnPrintListSumUp.Name = "btnPrintListSumUp"
        Me.btnPrintListSumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintListSumUp.TabIndex = 2
        Me.btnPrintListSumUp.Text = "一覧プレ印刷"
        Me.btnPrintListSumUp.UseVisualStyleBackColor = True
        '
        'btnPrintSummarySumUp
        '
        Me.btnPrintSummarySumUp.Location = New System.Drawing.Point(586, 170)
        Me.btnPrintSummarySumUp.Name = "btnPrintSummarySumUp"
        Me.btnPrintSummarySumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintSummarySumUp.TabIndex = 3
        Me.btnPrintSummarySumUp.Text = "合計プレ印刷"
        Me.btnPrintSummarySumUp.UseVisualStyleBackColor = True
        '
        'btnRefSumUp
        '
        Me.btnRefSumUp.Location = New System.Drawing.Point(270, 170)
        Me.btnRefSumUp.Name = "btnRefSumUp"
        Me.btnRefSumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnRefSumUp.TabIndex = 1
        Me.btnRefSumUp.Text = "照会"
        Me.btnRefSumUp.UseVisualStyleBackColor = True
        '
        'flxSumUp
        '
        Me.flxSumUp.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxSumUp.AllowEditing = False
        Me.flxSumUp.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxSumUp.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxSumUp.ColumnInfo = "10,1,0,0,0,110,Columns:"
        Me.flxSumUp.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.flxSumUp.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxSumUp.Location = New System.Drawing.Point(18, 40)
        Me.flxSumUp.Name = "flxSumUp"
        Me.flxSumUp.Rows.Count = 2
        Me.flxSumUp.Rows.DefaultSize = 22
        Me.flxSumUp.Rows.Fixed = 2
        Me.flxSumUp.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxSumUp.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.flxSumUp.Size = New System.Drawing.Size(937, 102)
        Me.flxSumUp.StyleInfo = resources.GetString("flxSumUp.StyleInfo")
        Me.flxSumUp.TabIndex = 0
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.cmbSumUpYear)
        Me.groupBox1.Controls.Add(Me.label4)
        Me.groupBox1.Controls.Add(Me.label8)
        Me.groupBox1.Controls.Add(Me.btnSumUpQuery)
        Me.groupBox1.Location = New System.Drawing.Point(170, 13)
        Me.groupBox1.Name = "groupBox1"
        Me.groupBox1.Size = New System.Drawing.Size(651, 59)
        Me.groupBox1.TabIndex = 0
        Me.groupBox1.TabStop = False
        Me.groupBox1.Text = "検索条件"
        '
        'cmbSumUpYear
        '
        Me.cmbSumUpYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSumUpYear.FormattingEnabled = True
        Me.cmbSumUpYear.Items.AddRange(New Object() {"2006", "2007", "2008", "2009", "2010"})
        Me.cmbSumUpYear.Location = New System.Drawing.Point(242, 21)
        Me.cmbSumUpYear.Name = "cmbSumUpYear"
        Me.cmbSumUpYear.Size = New System.Drawing.Size(63, 24)
        Me.cmbSumUpYear.TabIndex = 1
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(311, 25)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(24, 16)
        Me.label4.TabIndex = 2
        Me.label4.Text = "年"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(183, 25)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(56, 16)
        Me.label8.TabIndex = 0
        Me.label8.Text = "対象年"
        '
        'btnSumUpQuery
        '
        Me.btnSumUpQuery.Location = New System.Drawing.Point(351, 17)
        Me.btnSumUpQuery.Name = "btnSumUpQuery"
        Me.btnSumUpQuery.Size = New System.Drawing.Size(116, 32)
        Me.btnSumUpQuery.TabIndex = 3
        Me.btnSumUpQuery.Text = "検索"
        Me.btnSumUpQuery.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label11.Location = New System.Drawing.Point(200, 20)
        Me.Label11.MinimumSize = New System.Drawing.Size(630, 35)
        Me.Label11.Name = "Label11"
        Me.Label11.Padding = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.Label11.Size = New System.Drawing.Size(630, 35)
        Me.Label11.TabIndex = 19
        Me.Label11.Text = "賃金カット"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UC050101
        '
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.tabWageReduction)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "UC050101"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.tabWageReduction.ResumeLayout(False)
        Me.tabMonthly.ResumeLayout(False)
        Me.pnlResult.ResumeLayout(False)
        Me.grpPersonalStrike.ResumeLayout(False)
        CType(Me.flxStrike, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpInTime.ResumeLayout(False)
        CType(Me.flxInTime, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCondtion.ResumeLayout(False)
        Me.grpCondtion.PerformLayout()
        Me.tabBonus.ResumeLayout(False)
        Me.grpBonusResult.ResumeLayout(False)
        CType(Me.flxBonus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpBonusQueryCondition.ResumeLayout(False)
        Me.grpBonusQueryCondition.PerformLayout()
        Me.tabSumUp.ResumeLayout(False)
        Me.grpSumUpNonTaxable.ResumeLayout(False)
        CType(Me.flxSumUp, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub


    ' Fields
    Private _bonus As WageReductionBonusCommand
    Private _detail As UserControl
    Private _monthly As WageReductionMonthlyCommand
    Private _setteing_bonus As GridSettingInfo()
    Private _setteing_monthly As GridSettingInfo()
    Private _sumup As IWageReductionSumUpCommand
    Private WithEvents btnBonusSearch As Button
    Private WithEvents btnMonthlySearch As Button
    Private WithEvents btnNewBonus As Button
    Private WithEvents btnNewInTime As Button
    Private WithEvents btnNewStrike As Button
    Private WithEvents btnPrintBonusDetail As Button
    Private WithEvents btnPrintBonusTotal As Button
    Private WithEvents btnPrintListSumUp As Button
    Private WithEvents btnPrintMonthlyDetail As Button
    Private WithEvents btnPrintMonthlySummary As Button
    Private WithEvents btnPrintSummarySumUp As Button
    Private WithEvents btnRefBonus As Button
    Private WithEvents btnRefInTime As Button
    Private WithEvents btnRefStrike As Button
    Private WithEvents btnRefSumUp As Button
    Private WithEvents btnSumUpQuery As Button
    Private Const CAPTION_INTIME As String = "時間内"
    Private Const CAPTION_MONTH As String = "月分"
    Private Const CAPTION_STRIKE As String = "争議行為"
    Private Const CAPTION_YEAR As String = "年"
    Private WithEvents cmbBonusMonth As ComboBox
    Private WithEvents cmbBonusYear As ComboBox
    Private WithEvents cmbMonthlyMonth As ComboBox
    Private WithEvents cmbMonthlyYear As ComboBox
    Private WithEvents cmbSumUpYear As ComboBox
    'TODO Private components As IContainer
    Private flxBonus As C1FlexGrid
    Private flxInTime As C1FlexGrid
    Private flxStrike As C1FlexGrid
    Private flxSumUp As C1FlexGrid
    Private groupBox1 As GroupBox
    Private grpBonusQueryCondition As GroupBox
    Private grpBonusResult As GroupBox
    Private grpCondtion As GroupBox
    Private grpInTime As GroupBox
    Private grpPersonalStrike As GroupBox
    Private grpSumUpNonTaxable As GroupBox
    Private Const HEADER_CAPTION_BRANCH As String = "所属"
    Private Const HEADER_CAPTION_COVER_SUM As String = "補填額計"
    Private Const HEADER_CAPTION_CUT_SUM As String = "控除額計"
    Private Const HEADER_CAPTION_DUES_SUM As String = "切捨額計"
    Private Const HEADER_CAPTION_INTIME As String = "時間内"
    Private Const HEADER_CAPTION_PERSONS As String = "対象人数"
    Private Const HEADER_CAPTION_STRIKE As String = "争議行為"
    Private Const HEADER_CAPTION_TOTAL As String = "合計"
    Private label1 As Label
    Private label2 As Label
    Private label3 As Label
    Private label4 As Label
    Private label5 As Label
    Private label6 As Label
    Private label7 As Label
    Private label8 As Label
    Private pnlResult As Panel
    Private tabBonus As TabPage
    Private tabMonthly As TabPage
    Private tabSumUp As TabPage
    Private tabWageReduction As TabControl
    Private Const TITLE_BONUS As String = "賃金カット" & " - " & "一時金"
    Private Const TITLE_IN_TIME As String = "賃金カット" & " - " & "月例・時間内"
    Private Const TITLE_STRIKE As String = "賃金カット" & " - " & "月例・争議行為"
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Private WithEvents cmbCutOnceName As System.Windows.Forms.ComboBox
    Private WithEvents Label9 As System.Windows.Forms.Label

End Class
