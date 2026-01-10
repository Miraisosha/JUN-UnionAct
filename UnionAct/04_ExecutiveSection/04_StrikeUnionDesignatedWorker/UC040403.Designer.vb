<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040403
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC040403))
        Me.lblTitle = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.flxNameStrikeMemberCancel = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.txtRelatedNameStrikeNumber = New System.Windows.Forms.TextBox
        Me.lblRelatedNameStrikeNumber = New System.Windows.Forms.Label
        Me.lblNoticeWork = New System.Windows.Forms.Label
        Me.lblHowToRelease = New System.Windows.Forms.Label
        Me.lblCount = New System.Windows.Forms.Label
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker
        Me.DataGridView2 = New System.Windows.Forms.DataGridView
        Me.DataGridViewTextBoxColumn1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.DataGridViewTextBoxColumn3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.txtNote = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.Label16 = New System.Windows.Forms.Label
        Me.grpTimeFrame = New System.Windows.Forms.GroupBox
        Me.opt24Frame = New System.Windows.Forms.RadioButton
        Me.opt72Frame = New System.Windows.Forms.RadioButton
        Me.grpFightCancel = New System.Windows.Forms.GroupBox
        Me.btnFightCancelPrint = New System.Windows.Forms.Button
        Me.txtFightCanceNumber = New System.Windows.Forms.TextBox
        Me.lblFightCancelNumber = New System.Windows.Forms.Label
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
        Me.btnReturn = New System.Windows.Forms.Button
        Me.btnRelease = New System.Windows.Forms.Button
        Me.btnWork = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        CType(Me.flxNameStrikeMemberCancel, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpTimeFrame.SuspendLayout()
        Me.grpFightCancel.SuspendLayout()
        Me.SuspendLayout()
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
        Me.lblTitle.TabIndex = 15
        Me.lblTitle.Text = "指名ストライキ - 一部解除画面"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.flxNameStrikeMemberCancel)
        Me.GroupBox1.Controls.Add(Me.txtRelatedNameStrikeNumber)
        Me.GroupBox1.Controls.Add(Me.lblRelatedNameStrikeNumber)
        Me.GroupBox1.Controls.Add(Me.lblNoticeWork)
        Me.GroupBox1.Controls.Add(Me.lblHowToRelease)
        Me.GroupBox1.Controls.Add(Me.lblCount)
        Me.GroupBox1.Controls.Add(Me.dtpEndDate)
        Me.GroupBox1.Controls.Add(Me.dtpStartDate)
        Me.GroupBox1.Controls.Add(Me.DataGridView2)
        Me.GroupBox1.Controls.Add(Me.txtNote)
        Me.GroupBox1.Controls.Add(Me.Label17)
        Me.GroupBox1.Controls.Add(Me.Label16)
        Me.GroupBox1.Controls.Add(Me.grpTimeFrame)
        Me.GroupBox1.Controls.Add(Me.grpFightCancel)
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
        Me.GroupBox1.Size = New System.Drawing.Size(900, 687)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        '
        'flxNameStrikeMemberCancel
        '
        Me.flxNameStrikeMemberCancel.ColumnInfo = "3,0,0,0,0,110,Columns:2{AllowSorting:False;}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxNameStrikeMemberCancel.Location = New System.Drawing.Point(98, 335)
        Me.flxNameStrikeMemberCancel.Name = "flxNameStrikeMemberCancel"
        Me.flxNameStrikeMemberCancel.Rows.Count = 0
        Me.flxNameStrikeMemberCancel.Rows.DefaultSize = 22
        Me.flxNameStrikeMemberCancel.Rows.Fixed = 0
        Me.flxNameStrikeMemberCancel.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
        Me.flxNameStrikeMemberCancel.Size = New System.Drawing.Size(700, 310)
        Me.flxNameStrikeMemberCancel.StyleInfo = resources.GetString("flxNameStrikeMemberCancel.StyleInfo")
        Me.flxNameStrikeMemberCancel.TabIndex = 16
        '
        'txtRelatedNameStrikeNumber
        '
        Me.txtRelatedNameStrikeNumber.BackColor = System.Drawing.Color.Cornsilk
        Me.txtRelatedNameStrikeNumber.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtRelatedNameStrikeNumber.Location = New System.Drawing.Point(445, 77)
        Me.txtRelatedNameStrikeNumber.Name = "txtRelatedNameStrikeNumber"
        Me.txtRelatedNameStrikeNumber.ReadOnly = True
        Me.txtRelatedNameStrikeNumber.Size = New System.Drawing.Size(138, 23)
        Me.txtRelatedNameStrikeNumber.TabIndex = 4
        Me.txtRelatedNameStrikeNumber.TabStop = False
        Me.txtRelatedNameStrikeNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.txtRelatedNameStrikeNumber.Visible = False
        '
        'lblRelatedNameStrikeNumber
        '
        Me.lblRelatedNameStrikeNumber.AutoSize = True
        Me.lblRelatedNameStrikeNumber.Location = New System.Drawing.Point(317, 80)
        Me.lblRelatedNameStrikeNumber.Name = "lblRelatedNameStrikeNumber"
        Me.lblRelatedNameStrikeNumber.Size = New System.Drawing.Size(120, 16)
        Me.lblRelatedNameStrikeNumber.TabIndex = 41
        Me.lblRelatedNameStrikeNumber.Text = "関連通告書番号"
        Me.lblRelatedNameStrikeNumber.Visible = False
        '
        'lblNoticeWork
        '
        Me.lblNoticeWork.AutoSize = True
        Me.lblNoticeWork.ForeColor = System.Drawing.Color.Red
        Me.lblNoticeWork.Location = New System.Drawing.Point(6, 657)
        Me.lblNoticeWork.Name = "lblNoticeWork"
        Me.lblNoticeWork.Size = New System.Drawing.Size(120, 16)
        Me.lblNoticeWork.TabIndex = 40
        Me.lblNoticeWork.Text = "※一時保存文書"
        Me.lblNoticeWork.Visible = False
        '
        'lblHowToRelease
        '
        Me.lblHowToRelease.AutoSize = True
        Me.lblHowToRelease.Location = New System.Drawing.Point(204, 657)
        Me.lblHowToRelease.Name = "lblHowToRelease"
        Me.lblHowToRelease.Size = New System.Drawing.Size(492, 16)
        Me.lblHowToRelease.TabIndex = 39
        Me.lblHowToRelease.Text = "表示されている日付を、マウスクリックすることで解除設定を行うことができます。"
        '
        'lblCount
        '
        Me.lblCount.AutoSize = True
        Me.lblCount.Location = New System.Drawing.Point(824, 657)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(50, 16)
        Me.lblCount.TabIndex = 38
        Me.lblCount.Text = "・・A・・"
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpEndDate.Location = New System.Drawing.Point(188, 164)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(150, 23)
        Me.dtpEndDate.TabIndex = 8
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpStartDate.Location = New System.Drawing.Point(188, 135)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(150, 23)
        Me.dtpStartDate.TabIndex = 6
        '
        'DataGridView2
        '
        Me.DataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView2.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.DataGridViewTextBoxColumn1, Me.DataGridViewTextBoxColumn2, Me.DataGridViewTextBoxColumn3, Me.Column4, Me.Column5})
        Me.DataGridView2.Location = New System.Drawing.Point(98, 335)
        Me.DataGridView2.Name = "DataGridView2"
        Me.DataGridView2.RowHeadersVisible = False
        Me.DataGridView2.RowTemplate.Height = 21
        Me.DataGridView2.Size = New System.Drawing.Size(0, 0)
        Me.DataGridView2.TabIndex = 33
        '
        'DataGridViewTextBoxColumn1
        '
        Me.DataGridViewTextBoxColumn1.HeaderText = "社員番号"
        Me.DataGridViewTextBoxColumn1.Name = "DataGridViewTextBoxColumn1"
        '
        'DataGridViewTextBoxColumn2
        '
        Me.DataGridViewTextBoxColumn2.HeaderText = "氏名"
        Me.DataGridViewTextBoxColumn2.Name = "DataGridViewTextBoxColumn2"
        '
        'DataGridViewTextBoxColumn3
        '
        Me.DataGridViewTextBoxColumn3.HeaderText = "会社所属"
        Me.DataGridViewTextBoxColumn3.Name = "DataGridViewTextBoxColumn3"
        '
        'Column4
        '
        Me.Column4.HeaderText = "機種"
        Me.Column4.Name = "Column4"
        '
        'Column5
        '
        Me.Column5.HeaderText = "資格"
        Me.Column5.Name = "Column5"
        '
        'txtNote
        '
        Me.txtNote.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNote.Location = New System.Drawing.Point(188, 246)
        Me.txtNote.Multiline = True
        Me.txtNote.Name = "txtNote"
        Me.txtNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtNote.Size = New System.Drawing.Size(580, 70)
        Me.txtNote.TabIndex = 15
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
        Me.grpTimeFrame.TabIndex = 14
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
        Me.opt24Frame.TabIndex = 31
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
        Me.opt72Frame.TabIndex = 0
        Me.opt72Frame.TabStop = True
        Me.opt72Frame.Text = "７２時間枠"
        Me.opt72Frame.UseVisualStyleBackColor = True
        '
        'grpFightCancel
        '
        Me.grpFightCancel.Controls.Add(Me.btnFightCancelPrint)
        Me.grpFightCancel.Controls.Add(Me.txtFightCanceNumber)
        Me.grpFightCancel.Controls.Add(Me.lblFightCancelNumber)
        Me.grpFightCancel.Location = New System.Drawing.Point(632, 87)
        Me.grpFightCancel.Name = "grpFightCancel"
        Me.grpFightCancel.Size = New System.Drawing.Size(233, 100)
        Me.grpFightCancel.TabIndex = 11
        Me.grpFightCancel.TabStop = False
        '
        'btnFightCancelPrint
        '
        Me.btnFightCancelPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnFightCancelPrint.Location = New System.Drawing.Point(53, 58)
        Me.btnFightCancelPrint.Name = "btnFightCancelPrint"
        Me.btnFightCancelPrint.Size = New System.Drawing.Size(133, 30)
        Me.btnFightCancelPrint.TabIndex = 13
        Me.btnFightCancelPrint.Text = "闘争プレ指令印刷"
        Me.btnFightCancelPrint.UseVisualStyleBackColor = True
        '
        'txtFightCanceNumber
        '
        Me.txtFightCanceNumber.BackColor = System.Drawing.Color.Cornsilk
        Me.txtFightCanceNumber.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtFightCanceNumber.Location = New System.Drawing.Point(116, 19)
        Me.txtFightCanceNumber.Name = "txtFightCanceNumber"
        Me.txtFightCanceNumber.ReadOnly = True
        Me.txtFightCanceNumber.Size = New System.Drawing.Size(100, 23)
        Me.txtFightCanceNumber.TabIndex = 12
        Me.txtFightCanceNumber.TabStop = False
        Me.txtFightCanceNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblFightCancelNumber
        '
        Me.lblFightCancelNumber.AutoSize = True
        Me.lblFightCancelNumber.Location = New System.Drawing.Point(5, 22)
        Me.lblFightCancelNumber.Name = "lblFightCancelNumber"
        Me.lblFightCancelNumber.Size = New System.Drawing.Size(72, 16)
        Me.lblFightCancelNumber.TabIndex = 29
        Me.lblFightCancelNumber.Text = "闘争番号"
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
        Me.txtScale.TabIndex = 10
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
        Me.txtEndTime.Location = New System.Drawing.Point(344, 164)
        Me.txtEndTime.Name = "txtEndTime"
        Me.txtEndTime.Size = New System.Drawing.Size(40, 23)
        Me.txtEndTime.TabIndex = 9
        Me.txtEndTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtStartTime
        '
        Me.txtStartTime.BackColor = System.Drawing.Color.White
        Me.txtStartTime.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStartTime.Location = New System.Drawing.Point(345, 135)
        Me.txtStartTime.Name = "txtStartTime"
        Me.txtStartTime.Size = New System.Drawing.Size(40, 23)
        Me.txtStartTime.TabIndex = 7
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
        Me.txtApply.TabIndex = 5
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
        'btnReturn
        '
        Me.btnReturn.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnReturn.Location = New System.Drawing.Point(852, 769)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(110, 30)
        Me.btnReturn.TabIndex = 22
        Me.btnReturn.Text = "戻る"
        Me.btnReturn.UseVisualStyleBackColor = True
        Me.btnReturn.Visible = False
        '
        'btnRelease
        '
        Me.btnRelease.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnRelease.Location = New System.Drawing.Point(734, 769)
        Me.btnRelease.Name = "btnRelease"
        Me.btnRelease.Size = New System.Drawing.Size(110, 30)
        Me.btnRelease.TabIndex = 19
        Me.btnRelease.Text = "一部解除確認"
        Me.btnRelease.UseVisualStyleBackColor = True
        '
        'btnWork
        '
        Me.btnWork.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnWork.Location = New System.Drawing.Point(617, 769)
        Me.btnWork.Name = "btnWork"
        Me.btnWork.Size = New System.Drawing.Size(110, 30)
        Me.btnWork.TabIndex = 18
        Me.btnWork.Text = "一時保存確認"
        Me.btnWork.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(62, 769)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 17
        Me.btnPrint.Text = "プレ印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        Me.btnPrint.Visible = False
        '
        'btnChange
        '
        Me.btnChange.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnChange.Location = New System.Drawing.Point(734, 769)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(110, 30)
        Me.btnChange.TabIndex = 21
        Me.btnChange.Text = "内容変更"
        Me.btnChange.UseVisualStyleBackColor = True
        Me.btnChange.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(850, 769)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'UC040403
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnReturn)
        Me.Controls.Add(Me.btnRelease)
        Me.Controls.Add(Me.btnWork)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnChange)
        Me.Name = "UC040403"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.flxNameStrikeMemberCancel, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataGridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpTimeFrame.ResumeLayout(False)
        Me.grpTimeFrame.PerformLayout()
        Me.grpFightCancel.ResumeLayout(False)
        Me.grpFightCancel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblHowToRelease As System.Windows.Forms.Label
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents DataGridView2 As System.Windows.Forms.DataGridView
    Friend WithEvents DataGridViewTextBoxColumn1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DataGridViewTextBoxColumn3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents txtNote As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents grpTimeFrame As System.Windows.Forms.GroupBox
    Friend WithEvents opt24Frame As System.Windows.Forms.RadioButton
    Friend WithEvents opt72Frame As System.Windows.Forms.RadioButton
    Friend WithEvents grpFightCancel As System.Windows.Forms.GroupBox
    Friend WithEvents btnFightCancelPrint As System.Windows.Forms.Button
    Friend WithEvents txtFightCanceNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblFightCancelNumber As System.Windows.Forms.Label
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
    Friend WithEvents btnReturn As System.Windows.Forms.Button
    Friend WithEvents btnRelease As System.Windows.Forms.Button
    Friend WithEvents btnWork As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblNoticeWork As System.Windows.Forms.Label
    Friend WithEvents txtRelatedNameStrikeNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblRelatedNameStrikeNumber As System.Windows.Forms.Label
    Friend WithEvents flxNameStrikeMemberCancel As C1.Win.C1FlexGrid.C1FlexGrid

End Class
