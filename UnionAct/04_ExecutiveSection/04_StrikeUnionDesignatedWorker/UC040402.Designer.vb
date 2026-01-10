<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040402
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Label11 = New System.Windows.Forms.Label
        Me.btnReturn = New System.Windows.Forms.Button
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.lblNoticeWork = New System.Windows.Forms.Label
        Me.lblCount = New System.Windows.Forms.Label
        Me.btnDeleteMember = New System.Windows.Forms.Button
        Me.btnAddMember = New System.Windows.Forms.Button
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.dgdStrikeMember = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.txtNote = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.grpTimeFrame = New System.Windows.Forms.GroupBox
        Me.opt24Frame = New System.Windows.Forms.RadioButton
        Me.opt72Frame = New System.Windows.Forms.RadioButton
        Me.grpFight = New System.Windows.Forms.GroupBox
        Me.btnFightPrint = New System.Windows.Forms.Button
        Me.txtFightNumber = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtScale = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtEndTime = New System.Windows.Forms.TextBox
        Me.txtStartTime = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtApply = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.lblNoticeKind = New System.Windows.Forms.Label
        Me.txtStrikeNumber = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtNoticeNumber = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtNoticeKind = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtNoticeDate = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnConfirm = New System.Windows.Forms.Button
        Me.btnWork = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        CType(Me.dgdStrikeMember, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTimeFrame.SuspendLayout()
        Me.grpFight.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label11.Location = New System.Drawing.Point(200, 20)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(630, 35)
        Me.Label11.TabIndex = 13
        Me.Label11.Text = "指名ストライキ - 通告画面"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnReturn
        '
        Me.btnReturn.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnReturn.Location = New System.Drawing.Point(852, 775)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(110, 30)
        Me.btnReturn.TabIndex = 17
        Me.btnReturn.Text = "戻る"
        Me.btnReturn.UseVisualStyleBackColor = True
        Me.btnReturn.Visible = False
        '
        'btnChange
        '
        Me.btnChange.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnChange.Location = New System.Drawing.Point(737, 775)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(110, 30)
        Me.btnChange.TabIndex = 16
        Me.btnChange.Text = "内容変更"
        Me.btnChange.UseVisualStyleBackColor = True
        Me.btnChange.Visible = False
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(62, 775)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 20
        Me.btnPrint.Text = "プレ印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        Me.btnPrint.Visible = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblNoticeWork)
        Me.GroupBox1.Controls.Add(Me.lblCount)
        Me.GroupBox1.Controls.Add(Me.btnDeleteMember)
        Me.GroupBox1.Controls.Add(Me.btnAddMember)
        Me.GroupBox1.Controls.Add(Me.dtpEndDate)
        Me.GroupBox1.Controls.Add(Me.dtpStartDate)
        Me.GroupBox1.Controls.Add(Me.dgdStrikeMember)
        Me.GroupBox1.Controls.Add(Me.txtNote)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.grpTimeFrame)
        Me.GroupBox1.Controls.Add(Me.grpFight)
        Me.GroupBox1.Controls.Add(Me.Label14)
        Me.GroupBox1.Controls.Add(Me.txtScale)
        Me.GroupBox1.Controls.Add(Me.Label13)
        Me.GroupBox1.Controls.Add(Me.Label12)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.txtEndTime)
        Me.GroupBox1.Controls.Add(Me.txtStartTime)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.txtApply)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.lblNoticeKind)
        Me.GroupBox1.Controls.Add(Me.txtStrikeNumber)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.txtNoticeNumber)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtNoticeKind)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtNoticeDate)
        Me.GroupBox1.Controls.Add(Me.Label18)
        Me.GroupBox1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(62, 67)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(900, 702)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        '
        'lblNoticeWork
        '
        Me.lblNoticeWork.AutoSize = True
        Me.lblNoticeWork.ForeColor = System.Drawing.Color.Red
        Me.lblNoticeWork.Location = New System.Drawing.Point(6, 677)
        Me.lblNoticeWork.Name = "lblNoticeWork"
        Me.lblNoticeWork.Size = New System.Drawing.Size(120, 16)
        Me.lblNoticeWork.TabIndex = 39
        Me.lblNoticeWork.Text = "※一時保存文書"
        Me.lblNoticeWork.Visible = False
        '
        'lblCount
        '
        Me.lblCount.AutoSize = True
        Me.lblCount.Location = New System.Drawing.Point(824, 675)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(50, 16)
        Me.lblCount.TabIndex = 38
        Me.lblCount.Text = "・・A・・"
        '
        'btnDeleteMember
        '
        Me.btnDeleteMember.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDeleteMember.Location = New System.Drawing.Point(475, 654)
        Me.btnDeleteMember.Name = "btnDeleteMember"
        Me.btnDeleteMember.Size = New System.Drawing.Size(110, 30)
        Me.btnDeleteMember.TabIndex = 19
        Me.btnDeleteMember.Text = "選択行の削除"
        Me.btnDeleteMember.UseVisualStyleBackColor = True
        '
        'btnAddMember
        '
        Me.btnAddMember.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddMember.Location = New System.Drawing.Point(315, 654)
        Me.btnAddMember.Name = "btnAddMember"
        Me.btnAddMember.Size = New System.Drawing.Size(110, 30)
        Me.btnAddMember.TabIndex = 18
        Me.btnAddMember.Text = "組合員の追加"
        Me.btnAddMember.UseVisualStyleBackColor = True
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpEndDate.Location = New System.Drawing.Point(188, 164)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(150, 23)
        Me.dtpEndDate.TabIndex = 7
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpStartDate.Location = New System.Drawing.Point(188, 135)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(150, 23)
        Me.dtpStartDate.TabIndex = 5
        '
        'dgdStrikeMember
        '
        Me.dgdStrikeMember.AllowUserToAddRows = False
        Me.dgdStrikeMember.AllowUserToDeleteRows = False
        Me.dgdStrikeMember.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgdStrikeMember.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgdStrikeMember.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgdStrikeMember.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8})
        Me.dgdStrikeMember.Location = New System.Drawing.Point(126, 322)
        Me.dgdStrikeMember.MultiSelect = False
        Me.dgdStrikeMember.Name = "dgdStrikeMember"
        Me.dgdStrikeMember.RowHeadersVisible = False
        Me.dgdStrikeMember.RowTemplate.Height = 21
        Me.dgdStrikeMember.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdStrikeMember.Size = New System.Drawing.Size(672, 326)
        Me.dgdStrikeMember.StandardTab = True
        Me.dgdStrikeMember.TabIndex = 17
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.HeaderText = "社員番号"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column2
        '
        Me.Column2.HeaderText = "氏名"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column2.Width = 150
        '
        'Column3
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column3.HeaderText = "会社所属"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        '
        'Column4
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column4.HeaderText = "機種"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column4.Width = 80
        '
        'Column5
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column5.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column5.HeaderText = "資格"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column5.Width = 80
        '
        'Column6
        '
        Me.Column6.HeaderText = "会社所属省略名"
        Me.Column6.Name = "Column6"
        Me.Column6.Visible = False
        '
        'Column7
        '
        Me.Column7.HeaderText = "機種省略名"
        Me.Column7.Name = "Column7"
        Me.Column7.Visible = False
        '
        'Column8
        '
        Me.Column8.HeaderText = "組合支部"
        Me.Column8.Name = "Column8"
        Me.Column8.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.Column8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column8.Visible = False
        '
        'txtNote
        '
        Me.txtNote.BackColor = System.Drawing.Color.White
        Me.txtNote.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNote.Location = New System.Drawing.Point(188, 246)
        Me.txtNote.Multiline = True
        Me.txtNote.Name = "txtNote"
        Me.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNote.Size = New System.Drawing.Size(580, 70)
        Me.txtNote.TabIndex = 16
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label17.Location = New System.Drawing.Point(135, 271)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(40, 16)
        Me.Label17.TabIndex = 31
        Me.Label17.Text = "備考"
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label16.ForeColor = System.Drawing.Color.Red
        Me.Label16.Location = New System.Drawing.Point(164, 213)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(16, 16)
        Me.Label16.TabIndex = 30
        Me.Label16.Text = "*"
        '
        'grpTimeFrame
        '
        Me.grpTimeFrame.Controls.Add(Me.opt24Frame)
        Me.grpTimeFrame.Controls.Add(Me.opt72Frame)
        Me.grpTimeFrame.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpTimeFrame.Location = New System.Drawing.Point(188, 193)
        Me.grpTimeFrame.Name = "grpTimeFrame"
        Me.grpTimeFrame.Size = New System.Drawing.Size(285, 47)
        Me.grpTimeFrame.TabIndex = 13
        Me.grpTimeFrame.TabStop = False
        Me.grpTimeFrame.Text = "時間枠"
        '
        'opt24Frame
        '
        Me.opt24Frame.AutoSize = True
        Me.opt24Frame.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.opt24Frame.Location = New System.Drawing.Point(163, 20)
        Me.opt24Frame.Name = "opt24Frame"
        Me.opt24Frame.Size = New System.Drawing.Size(106, 20)
        Me.opt24Frame.TabIndex = 15
        Me.opt24Frame.TabStop = True
        Me.opt24Frame.Text = "２４時間枠"
        Me.opt24Frame.UseVisualStyleBackColor = True
        '
        'opt72Frame
        '
        Me.opt72Frame.AutoSize = True
        Me.opt72Frame.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.opt72Frame.Location = New System.Drawing.Point(38, 20)
        Me.opt72Frame.Name = "opt72Frame"
        Me.opt72Frame.Size = New System.Drawing.Size(106, 20)
        Me.opt72Frame.TabIndex = 14
        Me.opt72Frame.TabStop = True
        Me.opt72Frame.Text = "７２時間枠"
        Me.opt72Frame.UseVisualStyleBackColor = True
        '
        'grpFight
        '
        Me.grpFight.Controls.Add(Me.btnFightPrint)
        Me.grpFight.Controls.Add(Me.txtFightNumber)
        Me.grpFight.Controls.Add(Me.Label15)
        Me.grpFight.Location = New System.Drawing.Point(632, 87)
        Me.grpFight.Name = "grpFight"
        Me.grpFight.Size = New System.Drawing.Size(233, 100)
        Me.grpFight.TabIndex = 10
        Me.grpFight.TabStop = False
        Me.grpFight.Visible = False
        '
        'btnFightPrint
        '
        Me.btnFightPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnFightPrint.Location = New System.Drawing.Point(43, 58)
        Me.btnFightPrint.Name = "btnFightPrint"
        Me.btnFightPrint.Size = New System.Drawing.Size(128, 30)
        Me.btnFightPrint.TabIndex = 12
        Me.btnFightPrint.Text = "闘争指令プレ印刷"
        Me.btnFightPrint.UseVisualStyleBackColor = True
        '
        'txtFightNumber
        '
        Me.txtFightNumber.BackColor = System.Drawing.Color.Cornsilk
        Me.txtFightNumber.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtFightNumber.Location = New System.Drawing.Point(116, 19)
        Me.txtFightNumber.Name = "txtFightNumber"
        Me.txtFightNumber.ReadOnly = True
        Me.txtFightNumber.Size = New System.Drawing.Size(100, 23)
        Me.txtFightNumber.TabIndex = 11
        Me.txtFightNumber.TabStop = False
        Me.txtFightNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label15
        '
        Me.Label15.AutoSize = True
        Me.Label15.Location = New System.Drawing.Point(20, 22)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(72, 16)
        Me.Label15.TabIndex = 29
        Me.Label15.Text = "闘争番号"
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(564, 167)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(40, 16)
        Me.Label14.TabIndex = 27
        Me.Label14.Text = "時間"
        '
        'txtScale
        '
        Me.txtScale.BackColor = System.Drawing.Color.Cornsilk
        Me.txtScale.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtScale.Location = New System.Drawing.Point(518, 164)
        Me.txtScale.Name = "txtScale"
        Me.txtScale.ReadOnly = True
        Me.txtScale.Size = New System.Drawing.Size(40, 23)
        Me.txtScale.TabIndex = 9
        Me.txtScale.TabStop = False
        Me.txtScale.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(472, 167)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(40, 16)
        Me.Label13.TabIndex = 25
        Me.Label13.Text = "規模"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(390, 167)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(49, 16)
        Me.Label12.TabIndex = 24
        Me.Label12.Text = "時まで"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(391, 138)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(46, 16)
        Me.Label10.TabIndex = 23
        Me.Label10.Text = "時より"
        '
        'txtEndTime
        '
        Me.txtEndTime.BackColor = System.Drawing.Color.White
        Me.txtEndTime.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtEndTime.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtEndTime.Location = New System.Drawing.Point(345, 164)
        Me.txtEndTime.Name = "txtEndTime"
        Me.txtEndTime.Size = New System.Drawing.Size(40, 23)
        Me.txtEndTime.TabIndex = 8
        Me.txtEndTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtStartTime
        '
        Me.txtStartTime.BackColor = System.Drawing.Color.White
        Me.txtStartTime.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStartTime.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtStartTime.Location = New System.Drawing.Point(345, 135)
        Me.txtStartTime.Name = "txtStartTime"
        Me.txtStartTime.Size = New System.Drawing.Size(40, 23)
        Me.txtStartTime.TabIndex = 6
        Me.txtStartTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(140, 138)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 16)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "日付"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(123, 138)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(16, 16)
        Me.Label8.TabIndex = 17
        Me.Label8.Text = "*"
        '
        'txtApply
        '
        Me.txtApply.BackColor = System.Drawing.Color.White
        Me.txtApply.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApply.Location = New System.Drawing.Point(615, 43)
        Me.txtApply.Name = "txtApply"
        Me.txtApply.Size = New System.Drawing.Size(250, 23)
        Me.txtApply.TabIndex = 4
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(629, 22)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(56, 16)
        Me.Label6.TabIndex = 16
        Me.Label6.Text = "申請者"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.ForeColor = System.Drawing.Color.Red
        Me.Label9.Location = New System.Drawing.Point(612, 22)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(16, 16)
        Me.Label9.TabIndex = 15
        Me.Label9.Text = "*"
        '
        'lblNoticeKind
        '
        Me.lblNoticeKind.AutoSize = True
        Me.lblNoticeKind.Location = New System.Drawing.Point(417, 22)
        Me.lblNoticeKind.Name = "lblNoticeKind"
        Me.lblNoticeKind.Size = New System.Drawing.Size(67, 16)
        Me.lblNoticeKind.TabIndex = 8
        Me.lblNoticeKind.Text = "【通　告】"
        '
        'txtStrikeNumber
        '
        Me.txtStrikeNumber.BackColor = System.Drawing.Color.Cornsilk
        Me.txtStrikeNumber.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStrikeNumber.Location = New System.Drawing.Point(188, 106)
        Me.txtStrikeNumber.Name = "txtStrikeNumber"
        Me.txtStrikeNumber.ReadOnly = True
        Me.txtStrikeNumber.Size = New System.Drawing.Size(112, 23)
        Me.txtStrikeNumber.TabIndex = 3
        Me.txtStrikeNumber.TabStop = False
        Me.txtStrikeNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(39, 109)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(136, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "争議行為通告番号"
        '
        'txtNoticeNumber
        '
        Me.txtNoticeNumber.BackColor = System.Drawing.Color.Cornsilk
        Me.txtNoticeNumber.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNoticeNumber.Location = New System.Drawing.Point(188, 77)
        Me.txtNoticeNumber.Name = "txtNoticeNumber"
        Me.txtNoticeNumber.ReadOnly = True
        Me.txtNoticeNumber.Size = New System.Drawing.Size(112, 23)
        Me.txtNoticeNumber.TabIndex = 2
        Me.txtNoticeNumber.TabStop = False
        Me.txtNoticeNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(87, 80)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "通告書番号"
        '
        'txtNoticeKind
        '
        Me.txtNoticeKind.BackColor = System.Drawing.Color.Cornsilk
        Me.txtNoticeKind.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNoticeKind.Location = New System.Drawing.Point(188, 48)
        Me.txtNoticeKind.Name = "txtNoticeKind"
        Me.txtNoticeKind.ReadOnly = True
        Me.txtNoticeKind.Size = New System.Drawing.Size(112, 23)
        Me.txtNoticeKind.TabIndex = 1
        Me.txtNoticeKind.TabStop = False
        Me.txtNoticeKind.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(71, 51)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(104, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "通告番号種別"
        '
        'txtNoticeDate
        '
        Me.txtNoticeDate.BackColor = System.Drawing.Color.Cornsilk
        Me.txtNoticeDate.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNoticeDate.Location = New System.Drawing.Point(188, 19)
        Me.txtNoticeDate.Name = "txtNoticeDate"
        Me.txtNoticeDate.ReadOnly = True
        Me.txtNoticeDate.Size = New System.Drawing.Size(112, 23)
        Me.txtNoticeDate.TabIndex = 0
        Me.txtNoticeDate.TabStop = False
        Me.txtNoticeDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label18
        '
        Me.Label18.AutoSize = True
        Me.Label18.Location = New System.Drawing.Point(119, 22)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(56, 16)
        Me.Label18.TabIndex = 0
        Me.Label18.Text = "通告日"
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(853, 775)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 23
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnConfirm
        '
        Me.btnConfirm.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnConfirm.Location = New System.Drawing.Point(736, 775)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(110, 30)
        Me.btnConfirm.TabIndex = 22
        Me.btnConfirm.Text = "登録確認"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'btnWork
        '
        Me.btnWork.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnWork.Location = New System.Drawing.Point(620, 775)
        Me.btnWork.Name = "btnWork"
        Me.btnWork.Size = New System.Drawing.Size(110, 30)
        Me.btnWork.TabIndex = 21
        Me.btnWork.Text = "一時保存確認"
        Me.btnWork.UseVisualStyleBackColor = True
        '
        'UC040402
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnWork)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.Label11)
        Me.Name = "UC040402"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.dgdStrikeMember, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTimeFrame.ResumeLayout(False)
        Me.grpTimeFrame.PerformLayout()
        Me.grpFight.ResumeLayout(False)
        Me.grpFight.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnReturn As System.Windows.Forms.Button
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents btnDeleteMember As System.Windows.Forms.Button
    Friend WithEvents btnAddMember As System.Windows.Forms.Button
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dgdStrikeMember As System.Windows.Forms.DataGridView
    Friend WithEvents txtNote As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents grpTimeFrame As System.Windows.Forms.GroupBox
    Friend WithEvents opt24Frame As System.Windows.Forms.RadioButton
    Friend WithEvents opt72Frame As System.Windows.Forms.RadioButton
    Friend WithEvents grpFight As System.Windows.Forms.GroupBox
    Friend WithEvents btnFightPrint As System.Windows.Forms.Button
    Friend WithEvents txtFightNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtScale As System.Windows.Forms.TextBox
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents txtEndTime As System.Windows.Forms.TextBox
    Friend WithEvents txtStartTime As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtApply As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents lblNoticeKind As System.Windows.Forms.Label
    Friend WithEvents txtStrikeNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtNoticeNumber As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNoticeKind As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNoticeDate As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnConfirm As System.Windows.Forms.Button
    Friend WithEvents btnWork As System.Windows.Forms.Button
    Friend WithEvents lblNoticeWork As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
