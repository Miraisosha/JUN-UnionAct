<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC020301
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC020301))
        Me.GroupBox2 = New System.Windows.Forms.GroupBox
        Me.btnKaisai = New System.Windows.Forms.Button
        Me.lblTitle = New System.Windows.Forms.Label
        Me.tclControl1 = New System.Windows.Forms.TabControl
        Me.TabPage1 = New System.Windows.Forms.TabPage
        Me.grpSearchResult = New System.Windows.Forms.GroupBox
        Me.cfgResult = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.btnTyushi = New System.Windows.Forms.Button
        Me.btnHenko = New System.Windows.Forms.Button
        Me.btnGoudou = New System.Windows.Forms.Button
        Me.btnKaigi = New System.Windows.Forms.Button
        Me.grpSearch = New System.Windows.Forms.GroupBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.cboMonth = New System.Windows.Forms.ComboBox
        Me.cboYear = New System.Windows.Forms.ComboBox
        Me.cboshibu = New System.Windows.Forms.ComboBox
        Me.CboCommittee = New System.Windows.Forms.ComboBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.TabPage2 = New System.Windows.Forms.TabPage
        Me.grpSearchResult0 = New System.Windows.Forms.GroupBox
        Me.cfgResult0 = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.btnSakujyo = New System.Windows.Forms.Button
        Me.btnShosai = New System.Windows.Forms.Button
        Me.GroupBox5 = New System.Windows.Forms.GroupBox
        Me.btnSearch0 = New System.Windows.Forms.Button
        Me.cboshibu0 = New System.Windows.Forms.ComboBox
        Me.CboCommittee0 = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.GroupBox2.SuspendLayout()
        Me.tclControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.grpSearchResult.SuspendLayout()
        CType(Me.cfgResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpSearch.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.grpSearchResult0.SuspendLayout()
        CType(Me.cfgResult0, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox5.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnKaisai)
        Me.GroupBox2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(86, 748)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(900, 53)
        Me.GroupBox2.TabIndex = 6
        Me.GroupBox2.TabStop = False
        '
        'btnKaisai
        '
        Me.btnKaisai.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnKaisai.Location = New System.Drawing.Point(368, 17)
        Me.btnKaisai.Name = "btnKaisai"
        Me.btnKaisai.Size = New System.Drawing.Size(110, 30)
        Me.btnKaisai.TabIndex = 1
        Me.btnKaisai.Text = "開催登録"
        Me.btnKaisai.UseVisualStyleBackColor = True
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(200, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(630, 35)
        Me.lblTitle.TabIndex = 4
        Me.lblTitle.Text = "会議通知"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'tclControl1
        '
        Me.tclControl1.Controls.Add(Me.TabPage1)
        Me.tclControl1.Controls.Add(Me.TabPage2)
        Me.tclControl1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.tclControl1.Location = New System.Drawing.Point(21, 77)
        Me.tclControl1.Name = "tclControl1"
        Me.tclControl1.SelectedIndex = 0
        Me.tclControl1.Size = New System.Drawing.Size(986, 665)
        Me.tclControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TabPage1.Controls.Add(Me.grpSearchResult)
        Me.TabPage1.Controls.Add(Me.grpSearch)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(978, 636)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "会議通知"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'grpSearchResult
        '
        Me.grpSearchResult.BackColor = System.Drawing.Color.Transparent
        Me.grpSearchResult.Controls.Add(Me.cfgResult)
        Me.grpSearchResult.Controls.Add(Me.btnTyushi)
        Me.grpSearchResult.Controls.Add(Me.btnHenko)
        Me.grpSearchResult.Controls.Add(Me.btnGoudou)
        Me.grpSearchResult.Controls.Add(Me.btnKaigi)
        Me.grpSearchResult.ForeColor = System.Drawing.Color.Blue
        Me.grpSearchResult.Location = New System.Drawing.Point(20, 85)
        Me.grpSearchResult.Name = "grpSearchResult"
        Me.grpSearchResult.Size = New System.Drawing.Size(943, 537)
        Me.grpSearchResult.TabIndex = 1
        Me.grpSearchResult.TabStop = False
        Me.grpSearchResult.Text = "検索結果（ xx 件）"
        Me.grpSearchResult.Visible = False
        '
        'cfgResult
        '
        Me.cfgResult.AllowEditing = False
        Me.cfgResult.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.cfgResult.ColumnInfo = "1,1,0,0,0,110,Columns:"
        Me.cfgResult.Location = New System.Drawing.Point(20, 22)
        Me.cfgResult.Name = "cfgResult"
        Me.cfgResult.Rows.Count = 0
        Me.cfgResult.Rows.DefaultSize = 22
        Me.cfgResult.Rows.Fixed = 0
        Me.cfgResult.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.cfgResult.Size = New System.Drawing.Size(902, 457)
        Me.cfgResult.StyleInfo = resources.GetString("cfgResult.StyleInfo")
        Me.cfgResult.TabIndex = 5
        '
        'btnTyushi
        '
        Me.btnTyushi.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnTyushi.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnTyushi.Location = New System.Drawing.Point(812, 492)
        Me.btnTyushi.Name = "btnTyushi"
        Me.btnTyushi.Size = New System.Drawing.Size(110, 30)
        Me.btnTyushi.TabIndex = 9
        Me.btnTyushi.Text = "中止"
        Me.btnTyushi.UseVisualStyleBackColor = True
        '
        'btnHenko
        '
        Me.btnHenko.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnHenko.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnHenko.Location = New System.Drawing.Point(696, 492)
        Me.btnHenko.Name = "btnHenko"
        Me.btnHenko.Size = New System.Drawing.Size(110, 30)
        Me.btnHenko.TabIndex = 8
        Me.btnHenko.Text = "変更"
        Me.btnHenko.UseVisualStyleBackColor = True
        '
        'btnGoudou
        '
        Me.btnGoudou.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnGoudou.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnGoudou.Location = New System.Drawing.Point(580, 492)
        Me.btnGoudou.Name = "btnGoudou"
        Me.btnGoudou.Size = New System.Drawing.Size(110, 30)
        Me.btnGoudou.TabIndex = 7
        Me.btnGoudou.Text = "合同登録"
        Me.btnGoudou.UseVisualStyleBackColor = True
        '
        'btnKaigi
        '
        Me.btnKaigi.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnKaigi.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnKaigi.Location = New System.Drawing.Point(255, 492)
        Me.btnKaigi.Name = "btnKaigi"
        Me.btnKaigi.Size = New System.Drawing.Size(110, 30)
        Me.btnKaigi.TabIndex = 6
        Me.btnKaigi.Text = "会議詳細"
        Me.btnKaigi.UseVisualStyleBackColor = True
        '
        'grpSearch
        '
        Me.grpSearch.BackColor = System.Drawing.Color.Transparent
        Me.grpSearch.Controls.Add(Me.btnSearch)
        Me.grpSearch.Controls.Add(Me.cboMonth)
        Me.grpSearch.Controls.Add(Me.cboYear)
        Me.grpSearch.Controls.Add(Me.cboshibu)
        Me.grpSearch.Controls.Add(Me.CboCommittee)
        Me.grpSearch.Controls.Add(Me.Label13)
        Me.grpSearch.Controls.Add(Me.Label12)
        Me.grpSearch.Controls.Add(Me.Label10)
        Me.grpSearch.Controls.Add(Me.Label9)
        Me.grpSearch.Controls.Add(Me.Label8)
        Me.grpSearch.Controls.Add(Me.Label7)
        Me.grpSearch.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpSearch.ForeColor = System.Drawing.Color.Blue
        Me.grpSearch.Location = New System.Drawing.Point(20, 10)
        Me.grpSearch.Name = "grpSearch"
        Me.grpSearch.Size = New System.Drawing.Size(933, 67)
        Me.grpSearch.TabIndex = 0
        Me.grpSearch.TabStop = False
        Me.grpSearch.Text = "検索条件"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(759, 22)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'cboMonth
        '
        Me.cboMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMonth.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cboMonth.FormattingEnabled = True
        Me.cboMonth.Items.AddRange(New Object() {"", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cboMonth.Location = New System.Drawing.Point(673, 23)
        Me.cboMonth.Name = "cboMonth"
        Me.cboMonth.Size = New System.Drawing.Size(52, 24)
        Me.cboMonth.TabIndex = 3
        '
        'cboYear
        '
        Me.cboYear.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cboYear.FormattingEnabled = True
        Me.cboYear.Location = New System.Drawing.Point(569, 23)
        Me.cboYear.Name = "cboYear"
        Me.cboYear.Size = New System.Drawing.Size(65, 24)
        Me.cboYear.TabIndex = 2
        '
        'cboshibu
        '
        Me.cboshibu.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboshibu.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboshibu.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cboshibu.FormattingEnabled = True
        Me.cboshibu.Location = New System.Drawing.Point(384, 23)
        Me.cboshibu.Name = "cboshibu"
        Me.cboshibu.Size = New System.Drawing.Size(101, 24)
        Me.cboshibu.TabIndex = 1
        '
        'CboCommittee
        '
        Me.CboCommittee.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.CboCommittee.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CboCommittee.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CboCommittee.FormattingEnabled = True
        Me.CboCommittee.Location = New System.Drawing.Point(113, 23)
        Me.CboCommittee.Name = "CboCommittee"
        Me.CboCommittee.Size = New System.Drawing.Size(211, 24)
        Me.CboCommittee.TabIndex = 0
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label13.Location = New System.Drawing.Point(731, 29)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(24, 16)
        Me.Label13.TabIndex = 1
        Me.Label13.Text = "月"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label12.Location = New System.Drawing.Point(640, 29)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(24, 16)
        Me.Label12.TabIndex = 1
        Me.Label12.Text = "年"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label10.Location = New System.Drawing.Point(514, 29)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(56, 16)
        Me.Label10.TabIndex = 1
        Me.Label10.Text = "開催月"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label9.Location = New System.Drawing.Point(342, 29)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(40, 16)
        Me.Label9.TabIndex = 1
        Me.Label9.Text = "支部"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label8.Location = New System.Drawing.Point(21, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(88, 16)
        Me.Label8.TabIndex = 1
        Me.Label8.Text = "部／委員会"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(4, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(16, 16)
        Me.Label7.TabIndex = 0
        Me.Label7.Text = "*"
        '
        'TabPage2
        '
        Me.TabPage2.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.TabPage2.Controls.Add(Me.grpSearchResult0)
        Me.TabPage2.Controls.Add(Me.GroupBox5)
        Me.TabPage2.Location = New System.Drawing.Point(4, 25)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(978, 636)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "一時保存会議通知"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'grpSearchResult0
        '
        Me.grpSearchResult0.BackColor = System.Drawing.Color.Transparent
        Me.grpSearchResult0.Controls.Add(Me.cfgResult0)
        Me.grpSearchResult0.Controls.Add(Me.btnSakujyo)
        Me.grpSearchResult0.Controls.Add(Me.btnShosai)
        Me.grpSearchResult0.ForeColor = System.Drawing.Color.Blue
        Me.grpSearchResult0.Location = New System.Drawing.Point(20, 85)
        Me.grpSearchResult0.Name = "grpSearchResult0"
        Me.grpSearchResult0.Size = New System.Drawing.Size(943, 537)
        Me.grpSearchResult0.TabIndex = 3
        Me.grpSearchResult0.TabStop = False
        Me.grpSearchResult0.Text = "検索結果（ xx 件）"
        Me.grpSearchResult0.Visible = False
        '
        'cfgResult0
        '
        Me.cfgResult0.AllowEditing = False
        Me.cfgResult0.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.cfgResult0.ColumnInfo = "1,1,0,0,0,110,Columns:"
        Me.cfgResult0.Location = New System.Drawing.Point(20, 22)
        Me.cfgResult0.Name = "cfgResult0"
        Me.cfgResult0.Rows.Count = 0
        Me.cfgResult0.Rows.DefaultSize = 22
        Me.cfgResult0.Rows.Fixed = 0
        Me.cfgResult0.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.cfgResult0.Size = New System.Drawing.Size(902, 457)
        Me.cfgResult0.StyleInfo = resources.GetString("cfgResult0.StyleInfo")
        Me.cfgResult0.TabIndex = 3
        '
        'btnSakujyo
        '
        Me.btnSakujyo.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSakujyo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSakujyo.Location = New System.Drawing.Point(498, 492)
        Me.btnSakujyo.Name = "btnSakujyo"
        Me.btnSakujyo.Size = New System.Drawing.Size(110, 30)
        Me.btnSakujyo.TabIndex = 5
        Me.btnSakujyo.Text = "削除"
        Me.btnSakujyo.UseVisualStyleBackColor = True
        '
        'btnShosai
        '
        Me.btnShosai.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnShosai.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnShosai.Location = New System.Drawing.Point(313, 492)
        Me.btnShosai.Name = "btnShosai"
        Me.btnShosai.Size = New System.Drawing.Size(110, 30)
        Me.btnShosai.TabIndex = 4
        Me.btnShosai.Text = "詳細"
        Me.btnShosai.UseVisualStyleBackColor = True
        '
        'GroupBox5
        '
        Me.GroupBox5.BackColor = System.Drawing.Color.Transparent
        Me.GroupBox5.Controls.Add(Me.btnSearch0)
        Me.GroupBox5.Controls.Add(Me.cboshibu0)
        Me.GroupBox5.Controls.Add(Me.CboCommittee0)
        Me.GroupBox5.Controls.Add(Me.Label4)
        Me.GroupBox5.Controls.Add(Me.Label5)
        Me.GroupBox5.Controls.Add(Me.Label6)
        Me.GroupBox5.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox5.ForeColor = System.Drawing.Color.Blue
        Me.GroupBox5.Location = New System.Drawing.Point(20, 10)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(932, 67)
        Me.GroupBox5.TabIndex = 2
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "検索条件"
        '
        'btnSearch0
        '
        Me.btnSearch0.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch0.Location = New System.Drawing.Point(658, 21)
        Me.btnSearch0.Name = "btnSearch0"
        Me.btnSearch0.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch0.TabIndex = 2
        Me.btnSearch0.Text = "検索"
        Me.btnSearch0.UseVisualStyleBackColor = True
        '
        'cboshibu0
        '
        Me.cboshibu0.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboshibu0.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboshibu0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cboshibu0.FormattingEnabled = True
        Me.cboshibu0.Location = New System.Drawing.Point(475, 26)
        Me.cboshibu0.Name = "cboshibu0"
        Me.cboshibu0.Size = New System.Drawing.Size(101, 24)
        Me.cboshibu0.TabIndex = 1
        '
        'CboCommittee0
        '
        Me.CboCommittee0.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.CboCommittee0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CboCommittee0.FormattingEnabled = True
        Me.CboCommittee0.Location = New System.Drawing.Point(201, 26)
        Me.CboCommittee0.Name = "CboCommittee0"
        Me.CboCommittee0.Size = New System.Drawing.Size(212, 24)
        Me.CboCommittee0.TabIndex = 0
        Me.CboCommittee0.Text = "中央執行委員会"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label4.Location = New System.Drawing.Point(429, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 1
        Me.Label4.Text = "支部"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(107, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(88, 16)
        Me.Label5.TabIndex = 1
        Me.Label5.Text = "部／委員会"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(90, 29)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 16)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "*"
        '
        'UC020301
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.tclControl1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC020301"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.GroupBox2.ResumeLayout(False)
        Me.tclControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.grpSearchResult.ResumeLayout(False)
        CType(Me.cfgResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpSearch.ResumeLayout(False)
        Me.grpSearch.PerformLayout()
        Me.TabPage2.ResumeLayout(False)
        Me.grpSearchResult0.ResumeLayout(False)
        CType(Me.cfgResult0, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnKaisai As System.Windows.Forms.Button
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents tclControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents grpSearchResult As System.Windows.Forms.GroupBox
    Friend WithEvents btnTyushi As System.Windows.Forms.Button
    Friend WithEvents btnHenko As System.Windows.Forms.Button
    Friend WithEvents btnGoudou As System.Windows.Forms.Button
    Friend WithEvents btnKaigi As System.Windows.Forms.Button
    Friend WithEvents grpSearch As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents cboMonth As System.Windows.Forms.ComboBox
    Friend WithEvents cboYear As System.Windows.Forms.ComboBox
    Friend WithEvents cboshibu As System.Windows.Forms.ComboBox
    Friend WithEvents CboCommittee As System.Windows.Forms.ComboBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents grpSearchResult0 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSakujyo As System.Windows.Forms.Button
    Friend WithEvents btnShosai As System.Windows.Forms.Button
    Friend WithEvents GroupBox5 As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch0 As System.Windows.Forms.Button
    Friend WithEvents cboshibu0 As System.Windows.Forms.ComboBox
    Friend WithEvents CboCommittee0 As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents cfgResult As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents cfgResult0 As C1.Win.C1FlexGrid.C1FlexGrid

End Class
