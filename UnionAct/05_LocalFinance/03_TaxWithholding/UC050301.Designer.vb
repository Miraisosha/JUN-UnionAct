Imports System.ComponentModel
Imports C1.Win.C1FlexGrid
'Imports C1.Win.C1FlexGrid.Util.BaseControls

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC050301
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC050301))
        Me.tabWithholding = New System.Windows.Forms.TabControl
        Me.pageMonthly = New System.Windows.Forms.TabPage
        Me.grpCalc = New System.Windows.Forms.GroupBox
        Me.btnReCalc = New System.Windows.Forms.Button
        Me.btnNewCalc = New System.Windows.Forms.Button
        Me.grpResultNonTaxable = New System.Windows.Forms.GroupBox
        Me.btnPrintListNonTaxableMonthly = New System.Windows.Forms.Button
        Me.btnPrintSumNonTaxableMonthly = New System.Windows.Forms.Button
        Me.btnRefNonTaxableMonthly = New System.Windows.Forms.Button
        Me.flxMonthlyNonTaxable = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.grpResultTaxable = New System.Windows.Forms.GroupBox
        Me.btnPrintListTaxableMonthly = New System.Windows.Forms.Button
        Me.btnPrintSumTaxableMonthly = New System.Windows.Forms.Button
        Me.btnRefTaxableMonthly = New System.Windows.Forms.Button
        Me.flxMonthlyTaxable = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.grpCondtion = New System.Windows.Forms.GroupBox
        Me.cmbMonthlyYear = New System.Windows.Forms.ComboBox
        Me.label6 = New System.Windows.Forms.Label
        Me.cmbMonthlyMonth = New System.Windows.Forms.ComboBox
        Me.label7 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.btnMonthlyQuery = New System.Windows.Forms.Button
        Me.pageOnce = New System.Windows.Forms.TabPage
        Me.grpOnceCalc = New System.Windows.Forms.GroupBox
        Me.btnOnceReCalc = New System.Windows.Forms.Button
        Me.btnOnceNewCalc = New System.Windows.Forms.Button
        Me.grpOnceResultNonTaxable = New System.Windows.Forms.GroupBox
        Me.btnPrintListNonTaxableOnce = New System.Windows.Forms.Button
        Me.btnPrintSumNonTaxableOnce = New System.Windows.Forms.Button
        Me.btnRefNonTaxableOnce = New System.Windows.Forms.Button
        Me.flxOnceNonTaxable = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.grpOnceResultTaxable = New System.Windows.Forms.GroupBox
        Me.btnPrintListTaxableOnce = New System.Windows.Forms.Button
        Me.btnPrintSumTaxableOnce = New System.Windows.Forms.Button
        Me.btnRefTaxableOnce = New System.Windows.Forms.Button
        Me.flxOnceTaxable = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.groupBox5 = New System.Windows.Forms.GroupBox
        Me.cmbCutOnceName = New System.Windows.Forms.ComboBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.cmbOnceYear = New System.Windows.Forms.ComboBox
        Me.label2 = New System.Windows.Forms.Label
        Me.cmbOnceMonth = New System.Windows.Forms.ComboBox
        Me.label4 = New System.Windows.Forms.Label
        Me.label8 = New System.Windows.Forms.Label
        Me.btnOnceQuery = New System.Windows.Forms.Button
        Me.pageYearly = New System.Windows.Forms.TabPage
        Me.grpSumUpNonTaxable = New System.Windows.Forms.GroupBox
        Me.btnPrintListNonTaxableSumUp = New System.Windows.Forms.Button
        Me.btnPrintSumNonTaxableSumUp = New System.Windows.Forms.Button
        Me.btnRefNonTaxableSumUp = New System.Windows.Forms.Button
        Me.flxSumUpNonTaxable = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.grpSumUpTaxable = New System.Windows.Forms.GroupBox
        Me.btnPrintListTaxableSumUp = New System.Windows.Forms.Button
        Me.btnPrintSumTaxableSumUp = New System.Windows.Forms.Button
        Me.btnRefTaxableSumUp = New System.Windows.Forms.Button
        Me.flxSumUpTaxable = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.groupBox1 = New System.Windows.Forms.GroupBox
        Me.cmbSumUpYear = New System.Windows.Forms.ComboBox
        Me.label1 = New System.Windows.Forms.Label
        Me.label3 = New System.Windows.Forms.Label
        Me.btnSumUpQuery = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.tabWithholding.SuspendLayout()
        Me.pageMonthly.SuspendLayout()
        Me.grpCalc.SuspendLayout()
        Me.grpResultNonTaxable.SuspendLayout()
        CType(Me.flxMonthlyNonTaxable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpResultTaxable.SuspendLayout()
        CType(Me.flxMonthlyTaxable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpCondtion.SuspendLayout()
        Me.pageOnce.SuspendLayout()
        Me.grpOnceCalc.SuspendLayout()
        Me.grpOnceResultNonTaxable.SuspendLayout()
        CType(Me.flxOnceNonTaxable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpOnceResultTaxable.SuspendLayout()
        CType(Me.flxOnceTaxable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox5.SuspendLayout()
        Me.pageYearly.SuspendLayout()
        Me.grpSumUpNonTaxable.SuspendLayout()
        CType(Me.flxSumUpNonTaxable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSumUpTaxable.SuspendLayout()
        CType(Me.flxSumUpTaxable, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.groupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabWithholding
        '
        Me.tabWithholding.Controls.Add(Me.pageMonthly)
        Me.tabWithholding.Controls.Add(Me.pageOnce)
        Me.tabWithholding.Controls.Add(Me.pageYearly)
        Me.tabWithholding.ItemSize = New System.Drawing.Size(76, 21)
        Me.tabWithholding.Location = New System.Drawing.Point(8, 70)
        Me.tabWithholding.Multiline = True
        Me.tabWithholding.Name = "tabWithholding"
        Me.tabWithholding.SelectedIndex = 0
        Me.tabWithholding.Size = New System.Drawing.Size(1009, 734)
        Me.tabWithholding.TabIndex = 0
        '
        'pageMonthly
        '
        Me.pageMonthly.Controls.Add(Me.grpCalc)
        Me.pageMonthly.Controls.Add(Me.grpResultNonTaxable)
        Me.pageMonthly.Controls.Add(Me.grpResultTaxable)
        Me.pageMonthly.Controls.Add(Me.grpCondtion)
        Me.pageMonthly.Location = New System.Drawing.Point(4, 25)
        Me.pageMonthly.Name = "pageMonthly"
        Me.pageMonthly.Padding = New System.Windows.Forms.Padding(3)
        Me.pageMonthly.Size = New System.Drawing.Size(1001, 705)
        Me.pageMonthly.TabIndex = 0
        Me.pageMonthly.Text = "月例賃金集計"
        Me.pageMonthly.UseVisualStyleBackColor = True
        '
        'grpCalc
        '
        Me.grpCalc.Controls.Add(Me.btnReCalc)
        Me.grpCalc.Controls.Add(Me.btnNewCalc)
        Me.grpCalc.Location = New System.Drawing.Point(38, 631)
        Me.grpCalc.Name = "grpCalc"
        Me.grpCalc.Size = New System.Drawing.Size(925, 63)
        Me.grpCalc.TabIndex = 3
        Me.grpCalc.TabStop = False
        '
        'btnReCalc
        '
        Me.btnReCalc.Location = New System.Drawing.Point(313, 20)
        Me.btnReCalc.Name = "btnReCalc"
        Me.btnReCalc.Size = New System.Drawing.Size(116, 32)
        Me.btnReCalc.TabIndex = 0
        Me.btnReCalc.Text = "再集計"
        Me.btnReCalc.UseVisualStyleBackColor = True
        '
        'btnNewCalc
        '
        Me.btnNewCalc.Location = New System.Drawing.Point(495, 20)
        Me.btnNewCalc.Name = "btnNewCalc"
        Me.btnNewCalc.Size = New System.Drawing.Size(116, 32)
        Me.btnNewCalc.TabIndex = 1
        Me.btnNewCalc.Text = "新規集計"
        Me.btnNewCalc.UseVisualStyleBackColor = True
        '
        'grpResultNonTaxable
        '
        Me.grpResultNonTaxable.Controls.Add(Me.btnPrintListNonTaxableMonthly)
        Me.grpResultNonTaxable.Controls.Add(Me.btnPrintSumNonTaxableMonthly)
        Me.grpResultNonTaxable.Controls.Add(Me.btnRefNonTaxableMonthly)
        Me.grpResultNonTaxable.Controls.Add(Me.flxMonthlyNonTaxable)
        Me.grpResultNonTaxable.Location = New System.Drawing.Point(14, 321)
        Me.grpResultNonTaxable.Name = "grpResultNonTaxable"
        Me.grpResultNonTaxable.Size = New System.Drawing.Size(973, 191)
        Me.grpResultNonTaxable.TabIndex = 2
        Me.grpResultNonTaxable.TabStop = False
        Me.grpResultNonTaxable.Text = "課税非対象者"
        '
        'btnPrintListNonTaxableMonthly
        '
        Me.btnPrintListNonTaxableMonthly.Location = New System.Drawing.Point(609, 136)
        Me.btnPrintListNonTaxableMonthly.Name = "btnPrintListNonTaxableMonthly"
        Me.btnPrintListNonTaxableMonthly.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintListNonTaxableMonthly.TabIndex = 3
        Me.btnPrintListNonTaxableMonthly.Text = "一覧プレ印刷"
        Me.btnPrintListNonTaxableMonthly.UseVisualStyleBackColor = True
        '
        'btnPrintSumNonTaxableMonthly
        '
        Me.btnPrintSumNonTaxableMonthly.Location = New System.Drawing.Point(428, 136)
        Me.btnPrintSumNonTaxableMonthly.Name = "btnPrintSumNonTaxableMonthly"
        Me.btnPrintSumNonTaxableMonthly.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintSumNonTaxableMonthly.TabIndex = 2
        Me.btnPrintSumNonTaxableMonthly.Text = "合計プレ印刷"
        Me.btnPrintSumNonTaxableMonthly.UseVisualStyleBackColor = True
        '
        'btnRefNonTaxableMonthly
        '
        Me.btnRefNonTaxableMonthly.Location = New System.Drawing.Point(247, 136)
        Me.btnRefNonTaxableMonthly.Name = "btnRefNonTaxableMonthly"
        Me.btnRefNonTaxableMonthly.Size = New System.Drawing.Size(116, 32)
        Me.btnRefNonTaxableMonthly.TabIndex = 1
        Me.btnRefNonTaxableMonthly.Text = "照会"
        Me.btnRefNonTaxableMonthly.UseVisualStyleBackColor = True
        '
        'flxMonthlyNonTaxable
        '
        Me.flxMonthlyNonTaxable.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxMonthlyNonTaxable.AllowEditing = False
        Me.flxMonthlyNonTaxable.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxMonthlyNonTaxable.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxMonthlyNonTaxable.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxMonthlyNonTaxable.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.flxMonthlyNonTaxable.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxMonthlyNonTaxable.Location = New System.Drawing.Point(265, 28)
        Me.flxMonthlyNonTaxable.Name = "flxMonthlyNonTaxable"
        Me.flxMonthlyNonTaxable.Rows.Count = 1
        Me.flxMonthlyNonTaxable.Rows.DefaultSize = 22
        Me.flxMonthlyNonTaxable.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxMonthlyNonTaxable.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.flxMonthlyNonTaxable.Size = New System.Drawing.Size(442, 90)
        Me.flxMonthlyNonTaxable.StyleInfo = resources.GetString("flxMonthlyNonTaxable.StyleInfo")
        Me.flxMonthlyNonTaxable.TabIndex = 0
        '
        'grpResultTaxable
        '
        Me.grpResultTaxable.Controls.Add(Me.btnPrintListTaxableMonthly)
        Me.grpResultTaxable.Controls.Add(Me.btnPrintSumTaxableMonthly)
        Me.grpResultTaxable.Controls.Add(Me.btnRefTaxableMonthly)
        Me.grpResultTaxable.Controls.Add(Me.flxMonthlyTaxable)
        Me.grpResultTaxable.Location = New System.Drawing.Point(14, 101)
        Me.grpResultTaxable.Name = "grpResultTaxable"
        Me.grpResultTaxable.Size = New System.Drawing.Size(973, 191)
        Me.grpResultTaxable.TabIndex = 1
        Me.grpResultTaxable.TabStop = False
        Me.grpResultTaxable.Text = "課税対象者"
        '
        'btnPrintListTaxableMonthly
        '
        Me.btnPrintListTaxableMonthly.Location = New System.Drawing.Point(609, 136)
        Me.btnPrintListTaxableMonthly.Name = "btnPrintListTaxableMonthly"
        Me.btnPrintListTaxableMonthly.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintListTaxableMonthly.TabIndex = 3
        Me.btnPrintListTaxableMonthly.Text = "一覧プレ印刷"
        Me.btnPrintListTaxableMonthly.UseVisualStyleBackColor = True
        '
        'btnPrintSumTaxableMonthly
        '
        Me.btnPrintSumTaxableMonthly.Location = New System.Drawing.Point(428, 136)
        Me.btnPrintSumTaxableMonthly.Name = "btnPrintSumTaxableMonthly"
        Me.btnPrintSumTaxableMonthly.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintSumTaxableMonthly.TabIndex = 2
        Me.btnPrintSumTaxableMonthly.Text = "合計プレ印刷"
        Me.btnPrintSumTaxableMonthly.UseVisualStyleBackColor = True
        '
        'btnRefTaxableMonthly
        '
        Me.btnRefTaxableMonthly.Location = New System.Drawing.Point(247, 136)
        Me.btnRefTaxableMonthly.Name = "btnRefTaxableMonthly"
        Me.btnRefTaxableMonthly.Size = New System.Drawing.Size(116, 32)
        Me.btnRefTaxableMonthly.TabIndex = 1
        Me.btnRefTaxableMonthly.Text = "照会"
        Me.btnRefTaxableMonthly.UseVisualStyleBackColor = True
        '
        'flxMonthlyTaxable
        '
        Me.flxMonthlyTaxable.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxMonthlyTaxable.AllowEditing = False
        Me.flxMonthlyTaxable.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxMonthlyTaxable.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxMonthlyTaxable.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxMonthlyTaxable.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.flxMonthlyTaxable.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxMonthlyTaxable.Location = New System.Drawing.Point(76, 28)
        Me.flxMonthlyTaxable.Name = "flxMonthlyTaxable"
        Me.flxMonthlyTaxable.Rows.Count = 1
        Me.flxMonthlyTaxable.Rows.DefaultSize = 22
        Me.flxMonthlyTaxable.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxMonthlyTaxable.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.flxMonthlyTaxable.Size = New System.Drawing.Size(820, 90)
        Me.flxMonthlyTaxable.StyleInfo = resources.GetString("flxMonthlyTaxable.StyleInfo")
        Me.flxMonthlyTaxable.TabIndex = 0
        '
        'grpCondtion
        '
        Me.grpCondtion.Controls.Add(Me.cmbMonthlyYear)
        Me.grpCondtion.Controls.Add(Me.label6)
        Me.grpCondtion.Controls.Add(Me.cmbMonthlyMonth)
        Me.grpCondtion.Controls.Add(Me.label7)
        Me.grpCondtion.Controls.Add(Me.label5)
        Me.grpCondtion.Controls.Add(Me.btnMonthlyQuery)
        Me.grpCondtion.Location = New System.Drawing.Point(175, 13)
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
        'btnMonthlyQuery
        '
        Me.btnMonthlyQuery.Location = New System.Drawing.Point(395, 17)
        Me.btnMonthlyQuery.Name = "btnMonthlyQuery"
        Me.btnMonthlyQuery.Size = New System.Drawing.Size(116, 32)
        Me.btnMonthlyQuery.TabIndex = 5
        Me.btnMonthlyQuery.Text = "検索"
        Me.btnMonthlyQuery.UseVisualStyleBackColor = True
        '
        'pageOnce
        '
        Me.pageOnce.Controls.Add(Me.grpOnceCalc)
        Me.pageOnce.Controls.Add(Me.grpOnceResultNonTaxable)
        Me.pageOnce.Controls.Add(Me.grpOnceResultTaxable)
        Me.pageOnce.Controls.Add(Me.groupBox5)
        Me.pageOnce.Location = New System.Drawing.Point(4, 25)
        Me.pageOnce.Name = "pageOnce"
        Me.pageOnce.Size = New System.Drawing.Size(1001, 705)
        Me.pageOnce.TabIndex = 2
        Me.pageOnce.Text = "一時金集計"
        Me.pageOnce.UseVisualStyleBackColor = True
        '
        'grpOnceCalc
        '
        Me.grpOnceCalc.Controls.Add(Me.btnOnceReCalc)
        Me.grpOnceCalc.Controls.Add(Me.btnOnceNewCalc)
        Me.grpOnceCalc.Location = New System.Drawing.Point(38, 631)
        Me.grpOnceCalc.Name = "grpOnceCalc"
        Me.grpOnceCalc.Size = New System.Drawing.Size(925, 63)
        Me.grpOnceCalc.TabIndex = 7
        Me.grpOnceCalc.TabStop = False
        '
        'btnOnceReCalc
        '
        Me.btnOnceReCalc.Location = New System.Drawing.Point(313, 20)
        Me.btnOnceReCalc.Name = "btnOnceReCalc"
        Me.btnOnceReCalc.Size = New System.Drawing.Size(116, 32)
        Me.btnOnceReCalc.TabIndex = 0
        Me.btnOnceReCalc.Text = "再集計"
        Me.btnOnceReCalc.UseVisualStyleBackColor = True
        '
        'btnOnceNewCalc
        '
        Me.btnOnceNewCalc.Location = New System.Drawing.Point(495, 20)
        Me.btnOnceNewCalc.Name = "btnOnceNewCalc"
        Me.btnOnceNewCalc.Size = New System.Drawing.Size(116, 32)
        Me.btnOnceNewCalc.TabIndex = 1
        Me.btnOnceNewCalc.Text = "新規集計"
        Me.btnOnceNewCalc.UseVisualStyleBackColor = True
        '
        'grpOnceResultNonTaxable
        '
        Me.grpOnceResultNonTaxable.Controls.Add(Me.btnPrintListNonTaxableOnce)
        Me.grpOnceResultNonTaxable.Controls.Add(Me.btnPrintSumNonTaxableOnce)
        Me.grpOnceResultNonTaxable.Controls.Add(Me.btnRefNonTaxableOnce)
        Me.grpOnceResultNonTaxable.Controls.Add(Me.flxOnceNonTaxable)
        Me.grpOnceResultNonTaxable.Location = New System.Drawing.Point(14, 321)
        Me.grpOnceResultNonTaxable.Name = "grpOnceResultNonTaxable"
        Me.grpOnceResultNonTaxable.Size = New System.Drawing.Size(973, 191)
        Me.grpOnceResultNonTaxable.TabIndex = 6
        Me.grpOnceResultNonTaxable.TabStop = False
        Me.grpOnceResultNonTaxable.Text = "課税非対象者"
        '
        'btnPrintListNonTaxableOnce
        '
        Me.btnPrintListNonTaxableOnce.Location = New System.Drawing.Point(609, 136)
        Me.btnPrintListNonTaxableOnce.Name = "btnPrintListNonTaxableOnce"
        Me.btnPrintListNonTaxableOnce.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintListNonTaxableOnce.TabIndex = 3
        Me.btnPrintListNonTaxableOnce.Text = "一覧プレ印刷"
        Me.btnPrintListNonTaxableOnce.UseVisualStyleBackColor = True
        '
        'btnPrintSumNonTaxableOnce
        '
        Me.btnPrintSumNonTaxableOnce.Location = New System.Drawing.Point(428, 136)
        Me.btnPrintSumNonTaxableOnce.Name = "btnPrintSumNonTaxableOnce"
        Me.btnPrintSumNonTaxableOnce.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintSumNonTaxableOnce.TabIndex = 2
        Me.btnPrintSumNonTaxableOnce.Text = "合計プレ印刷"
        Me.btnPrintSumNonTaxableOnce.UseVisualStyleBackColor = True
        '
        'btnRefNonTaxableOnce
        '
        Me.btnRefNonTaxableOnce.Location = New System.Drawing.Point(247, 136)
        Me.btnRefNonTaxableOnce.Name = "btnRefNonTaxableOnce"
        Me.btnRefNonTaxableOnce.Size = New System.Drawing.Size(116, 32)
        Me.btnRefNonTaxableOnce.TabIndex = 1
        Me.btnRefNonTaxableOnce.Text = "照会"
        Me.btnRefNonTaxableOnce.UseVisualStyleBackColor = True
        '
        'flxOnceNonTaxable
        '
        Me.flxOnceNonTaxable.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxOnceNonTaxable.AllowEditing = False
        Me.flxOnceNonTaxable.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxOnceNonTaxable.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxOnceNonTaxable.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxOnceNonTaxable.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.flxOnceNonTaxable.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxOnceNonTaxable.Location = New System.Drawing.Point(265, 28)
        Me.flxOnceNonTaxable.Name = "flxOnceNonTaxable"
        Me.flxOnceNonTaxable.Rows.Count = 1
        Me.flxOnceNonTaxable.Rows.DefaultSize = 22
        Me.flxOnceNonTaxable.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxOnceNonTaxable.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.flxOnceNonTaxable.Size = New System.Drawing.Size(442, 90)
        Me.flxOnceNonTaxable.StyleInfo = resources.GetString("flxOnceNonTaxable.StyleInfo")
        Me.flxOnceNonTaxable.TabIndex = 0
        '
        'grpOnceResultTaxable
        '
        Me.grpOnceResultTaxable.Controls.Add(Me.btnPrintListTaxableOnce)
        Me.grpOnceResultTaxable.Controls.Add(Me.btnPrintSumTaxableOnce)
        Me.grpOnceResultTaxable.Controls.Add(Me.btnRefTaxableOnce)
        Me.grpOnceResultTaxable.Controls.Add(Me.flxOnceTaxable)
        Me.grpOnceResultTaxable.Location = New System.Drawing.Point(14, 101)
        Me.grpOnceResultTaxable.Name = "grpOnceResultTaxable"
        Me.grpOnceResultTaxable.Size = New System.Drawing.Size(973, 191)
        Me.grpOnceResultTaxable.TabIndex = 5
        Me.grpOnceResultTaxable.TabStop = False
        Me.grpOnceResultTaxable.Text = "課税対象者"
        '
        'btnPrintListTaxableOnce
        '
        Me.btnPrintListTaxableOnce.Location = New System.Drawing.Point(609, 136)
        Me.btnPrintListTaxableOnce.Name = "btnPrintListTaxableOnce"
        Me.btnPrintListTaxableOnce.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintListTaxableOnce.TabIndex = 3
        Me.btnPrintListTaxableOnce.Text = "一覧プレ印刷"
        Me.btnPrintListTaxableOnce.UseVisualStyleBackColor = True
        '
        'btnPrintSumTaxableOnce
        '
        Me.btnPrintSumTaxableOnce.Location = New System.Drawing.Point(428, 136)
        Me.btnPrintSumTaxableOnce.Name = "btnPrintSumTaxableOnce"
        Me.btnPrintSumTaxableOnce.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintSumTaxableOnce.TabIndex = 2
        Me.btnPrintSumTaxableOnce.Text = "合計プレ印刷"
        Me.btnPrintSumTaxableOnce.UseVisualStyleBackColor = True
        '
        'btnRefTaxableOnce
        '
        Me.btnRefTaxableOnce.Location = New System.Drawing.Point(247, 136)
        Me.btnRefTaxableOnce.Name = "btnRefTaxableOnce"
        Me.btnRefTaxableOnce.Size = New System.Drawing.Size(116, 32)
        Me.btnRefTaxableOnce.TabIndex = 1
        Me.btnRefTaxableOnce.Text = "照会"
        Me.btnRefTaxableOnce.UseVisualStyleBackColor = True
        '
        'flxOnceTaxable
        '
        Me.flxOnceTaxable.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxOnceTaxable.AllowEditing = False
        Me.flxOnceTaxable.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxOnceTaxable.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxOnceTaxable.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxOnceTaxable.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.flxOnceTaxable.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxOnceTaxable.Location = New System.Drawing.Point(201, 28)
        Me.flxOnceTaxable.Name = "flxOnceTaxable"
        Me.flxOnceTaxable.Rows.Count = 1
        Me.flxOnceTaxable.Rows.DefaultSize = 22
        Me.flxOnceTaxable.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxOnceTaxable.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.flxOnceTaxable.Size = New System.Drawing.Size(571, 90)
        Me.flxOnceTaxable.StyleInfo = resources.GetString("flxOnceTaxable.StyleInfo")
        Me.flxOnceTaxable.TabIndex = 0
        '
        'groupBox5
        '
        Me.groupBox5.Controls.Add(Me.cmbCutOnceName)
        Me.groupBox5.Controls.Add(Me.Label9)
        Me.groupBox5.Controls.Add(Me.cmbOnceYear)
        Me.groupBox5.Controls.Add(Me.label2)
        Me.groupBox5.Controls.Add(Me.cmbOnceMonth)
        Me.groupBox5.Controls.Add(Me.label4)
        Me.groupBox5.Controls.Add(Me.label8)
        Me.groupBox5.Controls.Add(Me.btnOnceQuery)
        Me.groupBox5.Location = New System.Drawing.Point(175, 13)
        Me.groupBox5.Name = "groupBox5"
        Me.groupBox5.Size = New System.Drawing.Size(651, 59)
        Me.groupBox5.TabIndex = 4
        Me.groupBox5.TabStop = False
        Me.groupBox5.Text = "検索条件"
        '
        'cmbCutOnceName
        '
        Me.cmbCutOnceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbCutOnceName.FormattingEnabled = True
        Me.cmbCutOnceName.Location = New System.Drawing.Point(359, 22)
        Me.cmbCutOnceName.Name = "cmbCutOnceName"
        Me.cmbCutOnceName.Size = New System.Drawing.Size(153, 24)
        Me.cmbCutOnceName.TabIndex = 18
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(270, 26)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(88, 16)
        Me.Label9.TabIndex = 19
        Me.Label9.Text = "一時金名称"
        '
        'cmbOnceYear
        '
        Me.cmbOnceYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOnceYear.FormattingEnabled = True
        Me.cmbOnceYear.Items.AddRange(New Object() {"2006", "2007", "2008", "2009", "2010"})
        Me.cmbOnceYear.Location = New System.Drawing.Point(82, 21)
        Me.cmbOnceYear.Name = "cmbOnceYear"
        Me.cmbOnceYear.Size = New System.Drawing.Size(63, 24)
        Me.cmbOnceYear.TabIndex = 1
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(151, 25)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(24, 16)
        Me.label2.TabIndex = 2
        Me.label2.Text = "年"
        '
        'cmbOnceMonth
        '
        Me.cmbOnceMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbOnceMonth.FormattingEnabled = True
        Me.cmbOnceMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cmbOnceMonth.Location = New System.Drawing.Point(176, 21)
        Me.cmbOnceMonth.MaxDropDownItems = 12
        Me.cmbOnceMonth.Name = "cmbOnceMonth"
        Me.cmbOnceMonth.Size = New System.Drawing.Size(50, 24)
        Me.cmbOnceMonth.TabIndex = 3
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(232, 26)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(24, 16)
        Me.label4.TabIndex = 4
        Me.label4.Text = "月"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Location = New System.Drawing.Point(14, 25)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(72, 16)
        Me.label8.TabIndex = 0
        Me.label8.Text = "対象年月"
        '
        'btnOnceQuery
        '
        Me.btnOnceQuery.Location = New System.Drawing.Point(522, 17)
        Me.btnOnceQuery.Name = "btnOnceQuery"
        Me.btnOnceQuery.Size = New System.Drawing.Size(116, 32)
        Me.btnOnceQuery.TabIndex = 5
        Me.btnOnceQuery.Text = "検索"
        Me.btnOnceQuery.UseVisualStyleBackColor = True
        '
        'pageYearly
        '
        Me.pageYearly.Controls.Add(Me.grpSumUpNonTaxable)
        Me.pageYearly.Controls.Add(Me.grpSumUpTaxable)
        Me.pageYearly.Controls.Add(Me.groupBox1)
        Me.pageYearly.Location = New System.Drawing.Point(4, 25)
        Me.pageYearly.Name = "pageYearly"
        Me.pageYearly.Padding = New System.Windows.Forms.Padding(3)
        Me.pageYearly.Size = New System.Drawing.Size(1001, 705)
        Me.pageYearly.TabIndex = 1
        Me.pageYearly.Text = " 累計"
        Me.pageYearly.UseVisualStyleBackColor = True
        '
        'grpSumUpNonTaxable
        '
        Me.grpSumUpNonTaxable.Controls.Add(Me.btnPrintListNonTaxableSumUp)
        Me.grpSumUpNonTaxable.Controls.Add(Me.btnPrintSumNonTaxableSumUp)
        Me.grpSumUpNonTaxable.Controls.Add(Me.btnRefNonTaxableSumUp)
        Me.grpSumUpNonTaxable.Controls.Add(Me.flxSumUpNonTaxable)
        Me.grpSumUpNonTaxable.Location = New System.Drawing.Point(14, 321)
        Me.grpSumUpNonTaxable.Name = "grpSumUpNonTaxable"
        Me.grpSumUpNonTaxable.Size = New System.Drawing.Size(973, 191)
        Me.grpSumUpNonTaxable.TabIndex = 2
        Me.grpSumUpNonTaxable.TabStop = False
        Me.grpSumUpNonTaxable.Text = "課税非対象者"
        '
        'btnPrintListNonTaxableSumUp
        '
        Me.btnPrintListNonTaxableSumUp.Location = New System.Drawing.Point(609, 136)
        Me.btnPrintListNonTaxableSumUp.Name = "btnPrintListNonTaxableSumUp"
        Me.btnPrintListNonTaxableSumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintListNonTaxableSumUp.TabIndex = 3
        Me.btnPrintListNonTaxableSumUp.Text = "一覧プレ印刷"
        Me.btnPrintListNonTaxableSumUp.UseVisualStyleBackColor = True
        '
        'btnPrintSumNonTaxableSumUp
        '
        Me.btnPrintSumNonTaxableSumUp.Location = New System.Drawing.Point(428, 136)
        Me.btnPrintSumNonTaxableSumUp.Name = "btnPrintSumNonTaxableSumUp"
        Me.btnPrintSumNonTaxableSumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintSumNonTaxableSumUp.TabIndex = 2
        Me.btnPrintSumNonTaxableSumUp.Text = "合計プレ印刷"
        Me.btnPrintSumNonTaxableSumUp.UseVisualStyleBackColor = True
        '
        'btnRefNonTaxableSumUp
        '
        Me.btnRefNonTaxableSumUp.Location = New System.Drawing.Point(247, 136)
        Me.btnRefNonTaxableSumUp.Name = "btnRefNonTaxableSumUp"
        Me.btnRefNonTaxableSumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnRefNonTaxableSumUp.TabIndex = 1
        Me.btnRefNonTaxableSumUp.Text = "照会"
        Me.btnRefNonTaxableSumUp.UseVisualStyleBackColor = True
        '
        'flxSumUpNonTaxable
        '
        Me.flxSumUpNonTaxable.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxSumUpNonTaxable.AllowEditing = False
        Me.flxSumUpNonTaxable.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxSumUpNonTaxable.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxSumUpNonTaxable.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxSumUpNonTaxable.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.flxSumUpNonTaxable.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxSumUpNonTaxable.Location = New System.Drawing.Point(205, 29)
        Me.flxSumUpNonTaxable.Name = "flxSumUpNonTaxable"
        Me.flxSumUpNonTaxable.Rows.Count = 1
        Me.flxSumUpNonTaxable.Rows.DefaultSize = 22
        Me.flxSumUpNonTaxable.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxSumUpNonTaxable.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.flxSumUpNonTaxable.Size = New System.Drawing.Size(562, 90)
        Me.flxSumUpNonTaxable.StyleInfo = resources.GetString("flxSumUpNonTaxable.StyleInfo")
        Me.flxSumUpNonTaxable.TabIndex = 0
        '
        'grpSumUpTaxable
        '
        Me.grpSumUpTaxable.Controls.Add(Me.btnPrintListTaxableSumUp)
        Me.grpSumUpTaxable.Controls.Add(Me.btnPrintSumTaxableSumUp)
        Me.grpSumUpTaxable.Controls.Add(Me.btnRefTaxableSumUp)
        Me.grpSumUpTaxable.Controls.Add(Me.flxSumUpTaxable)
        Me.grpSumUpTaxable.Location = New System.Drawing.Point(14, 101)
        Me.grpSumUpTaxable.Name = "grpSumUpTaxable"
        Me.grpSumUpTaxable.Size = New System.Drawing.Size(973, 191)
        Me.grpSumUpTaxable.TabIndex = 1
        Me.grpSumUpTaxable.TabStop = False
        Me.grpSumUpTaxable.Text = "課税対象者"
        '
        'btnPrintListTaxableSumUp
        '
        Me.btnPrintListTaxableSumUp.Location = New System.Drawing.Point(609, 136)
        Me.btnPrintListTaxableSumUp.Name = "btnPrintListTaxableSumUp"
        Me.btnPrintListTaxableSumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintListTaxableSumUp.TabIndex = 3
        Me.btnPrintListTaxableSumUp.Text = "一覧プレ印刷"
        Me.btnPrintListTaxableSumUp.UseVisualStyleBackColor = True
        '
        'btnPrintSumTaxableSumUp
        '
        Me.btnPrintSumTaxableSumUp.Location = New System.Drawing.Point(428, 136)
        Me.btnPrintSumTaxableSumUp.Name = "btnPrintSumTaxableSumUp"
        Me.btnPrintSumTaxableSumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnPrintSumTaxableSumUp.TabIndex = 2
        Me.btnPrintSumTaxableSumUp.Text = "合計プレ印刷"
        Me.btnPrintSumTaxableSumUp.UseVisualStyleBackColor = True
        '
        'btnRefTaxableSumUp
        '
        Me.btnRefTaxableSumUp.Location = New System.Drawing.Point(247, 136)
        Me.btnRefTaxableSumUp.Name = "btnRefTaxableSumUp"
        Me.btnRefTaxableSumUp.Size = New System.Drawing.Size(116, 32)
        Me.btnRefTaxableSumUp.TabIndex = 1
        Me.btnRefTaxableSumUp.Text = "照会"
        Me.btnRefTaxableSumUp.UseVisualStyleBackColor = True
        '
        'flxSumUpTaxable
        '
        Me.flxSumUpTaxable.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxSumUpTaxable.AllowEditing = False
        Me.flxSumUpTaxable.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxSumUpTaxable.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxSumUpTaxable.ColumnInfo = "10,1,0,0,0,110,Columns:0{Visible:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxSumUpTaxable.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.flxSumUpTaxable.HighLight = C1.Win.C1FlexGrid.HighLightEnum.Never
        Me.flxSumUpTaxable.Location = New System.Drawing.Point(15, 29)
        Me.flxSumUpTaxable.Name = "flxSumUpTaxable"
        Me.flxSumUpTaxable.Rows.Count = 1
        Me.flxSumUpTaxable.Rows.DefaultSize = 22
        Me.flxSumUpTaxable.ScrollBars = System.Windows.Forms.ScrollBars.None
        Me.flxSumUpTaxable.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.flxSumUpTaxable.Size = New System.Drawing.Size(942, 90)
        Me.flxSumUpTaxable.StyleInfo = resources.GetString("flxSumUpTaxable.StyleInfo")
        Me.flxSumUpTaxable.TabIndex = 0
        '
        'groupBox1
        '
        Me.groupBox1.Controls.Add(Me.cmbSumUpYear)
        Me.groupBox1.Controls.Add(Me.label1)
        Me.groupBox1.Controls.Add(Me.label3)
        Me.groupBox1.Controls.Add(Me.btnSumUpQuery)
        Me.groupBox1.Location = New System.Drawing.Point(175, 13)
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
        Me.cmbSumUpYear.Location = New System.Drawing.Point(247, 21)
        Me.cmbSumUpYear.Name = "cmbSumUpYear"
        Me.cmbSumUpYear.Size = New System.Drawing.Size(63, 24)
        Me.cmbSumUpYear.TabIndex = 1
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(316, 25)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(24, 16)
        Me.label1.TabIndex = 2
        Me.label1.Text = "年"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(179, 25)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(72, 16)
        Me.label3.TabIndex = 0
        Me.label3.Text = "対象年月"
        '
        'btnSumUpQuery
        '
        Me.btnSumUpQuery.Location = New System.Drawing.Point(356, 17)
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
        Me.Label11.TabIndex = 18
        Me.Label11.Text = "源泉徴収"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UC050301
        '
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.tabWithholding)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "UC050301"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.tabWithholding.ResumeLayout(False)
        Me.pageMonthly.ResumeLayout(False)
        Me.grpCalc.ResumeLayout(False)
        Me.grpResultNonTaxable.ResumeLayout(False)
        CType(Me.flxMonthlyNonTaxable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpResultTaxable.ResumeLayout(False)
        CType(Me.flxMonthlyTaxable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpCondtion.ResumeLayout(False)
        Me.grpCondtion.PerformLayout()
        Me.pageOnce.ResumeLayout(False)
        Me.grpOnceCalc.ResumeLayout(False)
        Me.grpOnceResultNonTaxable.ResumeLayout(False)
        CType(Me.flxOnceNonTaxable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpOnceResultTaxable.ResumeLayout(False)
        CType(Me.flxOnceTaxable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox5.ResumeLayout(False)
        Me.groupBox5.PerformLayout()
        Me.pageYearly.ResumeLayout(False)
        Me.grpSumUpNonTaxable.ResumeLayout(False)
        CType(Me.flxSumUpNonTaxable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSumUpTaxable.ResumeLayout(False)
        CType(Me.flxSumUpTaxable, System.ComponentModel.ISupportInitialize).EndInit()
        Me.groupBox1.ResumeLayout(False)
        Me.groupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    ' Fields
    Private _detail As UserControl
    Private _tabActMapping As Dictionary(Of String, ControlActManager)
    Private WithEvents btnMonthlyQuery As Button
    Private WithEvents btnNewCalc As Button
    Private WithEvents btnOnceNewCalc As Button
    Private WithEvents btnOnceQuery As Button
    Private WithEvents btnOnceReCalc As Button
    Private WithEvents btnPrintListNonTaxableMonthly As Button
    Private WithEvents btnPrintListNonTaxableOnce As Button
    Private WithEvents btnPrintListNonTaxableSumUp As Button
    Private WithEvents btnPrintListTaxableMonthly As Button
    Private WithEvents btnPrintListTaxableOnce As Button
    Private WithEvents btnPrintListTaxableSumUp As Button
    Private WithEvents btnPrintSumNonTaxableMonthly As Button
    Private WithEvents btnPrintSumNonTaxableOnce As Button
    Private WithEvents btnPrintSumNonTaxableSumUp As Button
    Private WithEvents btnPrintSumTaxableMonthly As Button
    Private WithEvents btnPrintSumTaxableOnce As Button
    Private WithEvents btnPrintSumTaxableSumUp As Button
    Private WithEvents btnReCalc As Button
    Private WithEvents btnRefNonTaxableMonthly As Button
    Private WithEvents btnRefNonTaxableOnce As Button
    Private WithEvents btnRefNonTaxableSumUp As Button
    Private WithEvents btnRefTaxableMonthly As Button
    Private WithEvents btnRefTaxableOnce As Button
    Private WithEvents btnRefTaxableSumUp As Button
    Private WithEvents btnSumUpQuery As Button
    Private WithEvents cmbMonthlyMonth As ComboBox
    Private WithEvents cmbMonthlyYear As ComboBox
    Private WithEvents cmbOnceMonth As ComboBox
    Private WithEvents cmbOnceYear As ComboBox
    Private WithEvents cmbSumUpYear As ComboBox
    'Private components As IContainer
    Private flxMonthlyNonTaxable As C1FlexGrid
    Private flxMonthlyTaxable As C1FlexGrid
    Private flxOnceNonTaxable As C1FlexGrid
    Private flxOnceTaxable As C1FlexGrid
    Private flxSumUpNonTaxable As C1FlexGrid
    Private flxSumUpTaxable As C1FlexGrid
    Private groupBox1 As GroupBox
    Private groupBox5 As GroupBox
    Private grpCalc As GroupBox
    Private grpCondtion As GroupBox
    Private grpOnceCalc As GroupBox
    Private grpOnceResultNonTaxable As GroupBox
    Private grpOnceResultTaxable As GroupBox
    Private grpResultNonTaxable As GroupBox
    Private grpResultTaxable As GroupBox
    Private grpSumUpNonTaxable As GroupBox
    Private grpSumUpTaxable As GroupBox
    Private label1 As Label
    Private label2 As Label
    Private label3 As Label
    Private label4 As Label
    Private label5 As Label
    Private label6 As Label
    Private label7 As Label
    Private label8 As Label
    Private pageMonthly As TabPage
    Private pageOnce As TabPage
    Private pageYearly As TabPage
    Private tabWithholding As TabControl
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Private WithEvents cmbCutOnceName As System.Windows.Forms.ComboBox
    Private WithEvents Label9 As System.Windows.Forms.Label

End Class
