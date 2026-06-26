<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC020202
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
        Me.lblTopTitle = New System.Windows.Forms.Label()
        Me.fraScheduleKind = New System.Windows.Forms.GroupBox()
        Me.lblPeriodValue = New System.Windows.Forms.Label()
        Me.dtpDate = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbScheduleKind = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblPeriod = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.fraScheduleDetail = New System.Windows.Forms.GroupBox()
        Me.txtTime = New System.Windows.Forms.TextBox()
        Me.dtpEnd = New System.Windows.Forms.DateTimePicker()
        Me.dtpStart = New System.Windows.Forms.DateTimePicker()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.txtBikou = New System.Windows.Forms.TextBox()
        Me.Label20 = New System.Windows.Forms.Label()
        Me.txtKatai = New System.Windows.Forms.TextBox()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.txtMokuteki = New System.Windows.Forms.TextBox()
        Me.Label18 = New System.Windows.Forms.Label()
        Me.txtLocation = New System.Windows.Forms.TextBox()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Label15 = New System.Windows.Forms.Label()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.txtScheduleName = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbBranch = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbCommitteName = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.btnConfirm = New System.Windows.Forms.Button()
        Me.btnBack = New System.Windows.Forms.Button()
        Me.fraScheduleKind.SuspendLayout()
        Me.fraScheduleDetail.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTopTitle
        '
        Me.lblTopTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTopTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTopTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTopTitle.Location = New System.Drawing.Point(367, 35)
        Me.lblTopTitle.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblTopTitle.Name = "lblTopTitle"
        Me.lblTopTitle.Size = New System.Drawing.Size(1155, 61)
        Me.lblTopTitle.TabIndex = 7
        Me.lblTopTitle.Text = "日程表詳細 - 新規登録"
        Me.lblTopTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fraScheduleKind
        '
        Me.fraScheduleKind.Controls.Add(Me.lblPeriodValue)
        Me.fraScheduleKind.Controls.Add(Me.dtpDate)
        Me.fraScheduleKind.Controls.Add(Me.Label5)
        Me.fraScheduleKind.Controls.Add(Me.cmbScheduleKind)
        Me.fraScheduleKind.Controls.Add(Me.Label11)
        Me.fraScheduleKind.Controls.Add(Me.Label3)
        Me.fraScheduleKind.Controls.Add(Me.lblPeriod)
        Me.fraScheduleKind.Controls.Add(Me.Label4)
        Me.fraScheduleKind.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.fraScheduleKind.Location = New System.Drawing.Point(110, 140)
        Me.fraScheduleKind.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.fraScheduleKind.Name = "fraScheduleKind"
        Me.fraScheduleKind.Padding = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.fraScheduleKind.Size = New System.Drawing.Size(1613, 131)
        Me.fraScheduleKind.TabIndex = 8
        Me.fraScheduleKind.TabStop = False
        Me.fraScheduleKind.Text = "日程分類･開催日"
        '
        'lblPeriodValue
        '
        Me.lblPeriodValue.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPeriodValue.Location = New System.Drawing.Point(73, 60)
        Me.lblPeriodValue.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblPeriodValue.Name = "lblPeriodValue"
        Me.lblPeriodValue.Size = New System.Drawing.Size(94, 33)
        Me.lblPeriodValue.TabIndex = 21
        Me.lblPeriodValue.Text = "P001"
        Me.lblPeriodValue.Visible = False
        '
        'dtpDate
        '
        Me.dtpDate.CalendarFont = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpDate.Location = New System.Drawing.Point(1144, 51)
        Me.dtpDate.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(363, 35)
        Me.dtpDate.TabIndex = 2
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(1032, 60)
        Me.Label5.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(96, 28)
        Me.Label5.TabIndex = 18
        Me.Label5.Text = "開催日"
        '
        'cmbScheduleKind
        '
        Me.cmbScheduleKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbScheduleKind.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbScheduleKind.FormattingEnabled = True
        Me.cmbScheduleKind.Location = New System.Drawing.Point(590, 51)
        Me.cmbScheduleKind.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.cmbScheduleKind.Name = "cmbScheduleKind"
        Me.cmbScheduleKind.Size = New System.Drawing.Size(275, 36)
        Me.cmbScheduleKind.TabIndex = 1
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.ForeColor = System.Drawing.Color.Red
        Me.Label11.Location = New System.Drawing.Point(1001, 60)
        Me.Label11.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(26, 28)
        Me.Label11.TabIndex = 17
        Me.Label11.Text = "*"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(396, 61)
        Me.Label3.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(152, 28)
        Me.Label3.TabIndex = 18
        Me.Label3.Text = "日程表分類"
        '
        'lblPeriod
        '
        Me.lblPeriod.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPeriod.Location = New System.Drawing.Point(246, 61)
        Me.lblPeriod.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.Size = New System.Drawing.Size(117, 33)
        Me.lblPeriod.TabIndex = 0
        Me.lblPeriod.Text = "第52期"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(374, 61)
        Me.Label4.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(26, 28)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "*"
        '
        'fraScheduleDetail
        '
        Me.fraScheduleDetail.Controls.Add(Me.txtTime)
        Me.fraScheduleDetail.Controls.Add(Me.dtpEnd)
        Me.fraScheduleDetail.Controls.Add(Me.dtpStart)
        Me.fraScheduleDetail.Controls.Add(Me.Label21)
        Me.fraScheduleDetail.Controls.Add(Me.txtBikou)
        Me.fraScheduleDetail.Controls.Add(Me.Label20)
        Me.fraScheduleDetail.Controls.Add(Me.txtKatai)
        Me.fraScheduleDetail.Controls.Add(Me.Label19)
        Me.fraScheduleDetail.Controls.Add(Me.txtMokuteki)
        Me.fraScheduleDetail.Controls.Add(Me.Label18)
        Me.fraScheduleDetail.Controls.Add(Me.txtLocation)
        Me.fraScheduleDetail.Controls.Add(Me.Label17)
        Me.fraScheduleDetail.Controls.Add(Me.Label16)
        Me.fraScheduleDetail.Controls.Add(Me.Label15)
        Me.fraScheduleDetail.Controls.Add(Me.Label14)
        Me.fraScheduleDetail.Controls.Add(Me.txtScheduleName)
        Me.fraScheduleDetail.Controls.Add(Me.Label12)
        Me.fraScheduleDetail.Controls.Add(Me.Label13)
        Me.fraScheduleDetail.Controls.Add(Me.Label10)
        Me.fraScheduleDetail.Controls.Add(Me.cmbBranch)
        Me.fraScheduleDetail.Controls.Add(Me.Label8)
        Me.fraScheduleDetail.Controls.Add(Me.Label9)
        Me.fraScheduleDetail.Controls.Add(Me.cmbCommitteName)
        Me.fraScheduleDetail.Controls.Add(Me.Label6)
        Me.fraScheduleDetail.Controls.Add(Me.Label7)
        Me.fraScheduleDetail.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.fraScheduleDetail.Location = New System.Drawing.Point(110, 324)
        Me.fraScheduleDetail.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.fraScheduleDetail.Name = "fraScheduleDetail"
        Me.fraScheduleDetail.Padding = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.fraScheduleDetail.Size = New System.Drawing.Size(1613, 975)
        Me.fraScheduleDetail.TabIndex = 9
        Me.fraScheduleDetail.TabStop = False
        Me.fraScheduleDetail.Text = "日程詳細"
        '
        'txtTime
        '
        Me.txtTime.BackColor = System.Drawing.SystemColors.Window
        Me.txtTime.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTime.Location = New System.Drawing.Point(292, 289)
        Me.txtTime.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtTime.Name = "txtTime"
        Me.txtTime.Size = New System.Drawing.Size(162, 35)
        Me.txtTime.TabIndex = 8
        Me.txtTime.Text = "__時間__分"
        Me.txtTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'dtpEnd
        '
        Me.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpEnd.Location = New System.Drawing.Point(678, 215)
        Me.dtpEnd.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpEnd.Name = "dtpEnd"
        Me.dtpEnd.ShowUpDown = True
        Me.dtpEnd.Size = New System.Drawing.Size(162, 35)
        Me.dtpEnd.TabIndex = 7
        '
        'dtpStart
        '
        Me.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtpStart.Location = New System.Drawing.Point(292, 215)
        Me.dtpStart.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.dtpStart.Name = "dtpStart"
        Me.dtpStart.ShowUpDown = True
        Me.dtpStart.Size = New System.Drawing.Size(162, 35)
        Me.dtpStart.TabIndex = 6
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Location = New System.Drawing.Point(842, 220)
        Me.Label21.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(208, 28)
        Me.Label21.TabIndex = 36
        Me.Label21.Text = "（例：0時～22時）"
        '
        'txtBikou
        '
        Me.txtBikou.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtBikou.Location = New System.Drawing.Point(292, 836)
        Me.txtBikou.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtBikou.Multiline = True
        Me.txtBikou.Name = "txtBikou"
        Me.txtBikou.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtBikou.Size = New System.Drawing.Size(1188, 102)
        Me.txtBikou.TabIndex = 12
        '
        'Label20
        '
        Me.Label20.AutoSize = True
        Me.Label20.Location = New System.Drawing.Point(182, 842)
        Me.Label20.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(68, 28)
        Me.Label20.TabIndex = 31
        Me.Label20.Text = "備考"
        '
        'txtKatai
        '
        Me.txtKatai.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtKatai.Location = New System.Drawing.Point(292, 653)
        Me.txtKatai.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtKatai.Multiline = True
        Me.txtKatai.Name = "txtKatai"
        Me.txtKatai.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtKatai.Size = New System.Drawing.Size(1188, 144)
        Me.txtKatai.TabIndex = 11
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Location = New System.Drawing.Point(127, 653)
        Me.Label19.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(120, 28)
        Me.Label19.TabIndex = 29
        Me.Label19.Text = "主な議題"
        '
        'txtMokuteki
        '
        Me.txtMokuteki.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtMokuteki.Location = New System.Drawing.Point(292, 432)
        Me.txtMokuteki.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtMokuteki.Multiline = True
        Me.txtMokuteki.Name = "txtMokuteki"
        Me.txtMokuteki.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtMokuteki.Size = New System.Drawing.Size(1188, 186)
        Me.txtMokuteki.TabIndex = 10
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(123, 438)
        Me.Label18.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(124, 28)
        Me.Label18.TabIndex = 27
        Me.Label18.Text = "開催目的"
        '
        'txtLocation
        '
        Me.txtLocation.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtLocation.Location = New System.Drawing.Point(293, 366)
        Me.txtLocation.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtLocation.Name = "txtLocation"
        Me.txtLocation.Size = New System.Drawing.Size(1187, 35)
        Me.txtLocation.TabIndex = 9
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Location = New System.Drawing.Point(182, 366)
        Me.Label17.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(68, 28)
        Me.Label17.TabIndex = 25
        Me.Label17.Text = "場所"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Location = New System.Drawing.Point(123, 294)
        Me.Label16.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(124, 28)
        Me.Label16.TabIndex = 23
        Me.Label16.Text = "所要時間"
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(469, 220)
        Me.Label15.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(188, 28)
        Me.Label15.TabIndex = 21
        Me.Label15.Text = "～    終了時間"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(130, 224)
        Me.Label14.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(124, 28)
        Me.Label14.TabIndex = 19
        Me.Label14.Text = "開始時間"
        '
        'txtScheduleName
        '
        Me.txtScheduleName.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtScheduleName.Location = New System.Drawing.Point(292, 150)
        Me.txtScheduleName.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.txtScheduleName.Name = "txtScheduleName"
        Me.txtScheduleName.Size = New System.Drawing.Size(681, 35)
        Me.txtScheduleName.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(42, 156)
        Me.Label12.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(208, 28)
        Me.Label12.TabIndex = 17
        Me.Label12.Text = "日程表表示名称"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.ForeColor = System.Drawing.Color.Red
        Me.Label13.Location = New System.Drawing.Point(20, 158)
        Me.Label13.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(26, 28)
        Me.Label13.TabIndex = 16
        Me.Label13.Text = "*"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(735, 84)
        Me.Label10.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(180, 28)
        Me.Label10.TabIndex = 14
        Me.Label10.Text = "（直接入力可）"
        '
        'cmbBranch
        '
        Me.cmbBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBranch.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbBranch.FormattingEnabled = True
        Me.cmbBranch.Location = New System.Drawing.Point(1197, 82)
        Me.cmbBranch.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.cmbBranch.Name = "cmbBranch"
        Me.cmbBranch.Size = New System.Drawing.Size(164, 36)
        Me.cmbBranch.TabIndex = 4
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(1115, 89)
        Me.Label8.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 28)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "支部"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Red
        Me.Label9.Location = New System.Drawing.Point(1093, 91)
        Me.Label9.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(26, 28)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "*"
        '
        'cmbCommitteName
        '
        Me.cmbCommitteName.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbCommitteName.FormattingEnabled = True
        Me.cmbCommitteName.Location = New System.Drawing.Point(292, 82)
        Me.cmbCommitteName.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.cmbCommitteName.Name = "cmbCommitteName"
        Me.cmbCommitteName.Size = New System.Drawing.Size(404, 36)
        Me.cmbCommitteName.TabIndex = 3
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(123, 89)
        Me.Label6.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(124, 28)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "委員会名"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(73, 89)
        Me.Label7.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(26, 28)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "*"
        '
        'btnConfirm
        '
        Me.btnConfirm.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnConfirm.Location = New System.Drawing.Point(1256, 1330)
        Me.btnConfirm.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(202, 52)
        Me.btnConfirm.TabIndex = 13
        Me.btnConfirm.Text = "登録確認"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBack.Location = New System.Drawing.Point(1533, 1330)
        Me.btnBack.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(202, 52)
        Me.btnBack.TabIndex = 14
        Me.btnBack.Text = "戻る"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'UC020202
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(11.0!, 21.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.fraScheduleDetail)
        Me.Controls.Add(Me.fraScheduleKind)
        Me.Controls.Add(Me.lblTopTitle)
        Me.Margin = New System.Windows.Forms.Padding(6, 5, 6, 5)
        Me.Name = "UC020202"
        Me.Size = New System.Drawing.Size(1877, 1435)
        Me.fraScheduleKind.ResumeLayout(False)
        Me.fraScheduleKind.PerformLayout()
        Me.fraScheduleDetail.ResumeLayout(False)
        Me.fraScheduleDetail.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTopTitle As System.Windows.Forms.Label
    Friend WithEvents fraScheduleKind As System.Windows.Forms.GroupBox
    Friend WithEvents lblPeriod As System.Windows.Forms.Label
    Friend WithEvents fraScheduleDetail As System.Windows.Forms.GroupBox
    Friend WithEvents Label21 As System.Windows.Forms.Label
    Friend WithEvents txtBikou As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    Friend WithEvents txtKatai As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtMokuteki As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtLocation As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtScheduleName As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbBranch As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents cmbCommitteName As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents btnConfirm As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbScheduleKind As System.Windows.Forms.ComboBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStart As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtTime As System.Windows.Forms.TextBox
    Friend WithEvents dtpEnd As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblPeriodValue As System.Windows.Forms.Label

End Class
