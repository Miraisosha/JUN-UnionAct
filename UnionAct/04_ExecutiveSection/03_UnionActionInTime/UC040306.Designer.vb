<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040306
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC040306))
        Me.grpNameAndDate = New System.Windows.Forms.GroupBox
        Me.cfgResult = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.lblVersion = New System.Windows.Forms.Label
        Me.lblMemo = New System.Windows.Forms.Label
        Me.txtStandName = New System.Windows.Forms.TextBox
        Me.lblStandName = New System.Windows.Forms.Label
        Me.grpReplace = New System.Windows.Forms.GroupBox
        Me.btnReplace = New System.Windows.Forms.Button
        Me.lblReplaceNo = New System.Windows.Forms.Label
        Me.txtReplaceNo = New System.Windows.Forms.TextBox
        Me.chkReplace = New System.Windows.Forms.CheckBox
        Me.txtTerm = New System.Windows.Forms.TextBox
        Me.lblTerm = New System.Windows.Forms.Label
        Me.lblMeeting = New System.Windows.Forms.Label
        Me.likMemo = New System.Windows.Forms.LinkLabel
        Me.lblOmissionName = New System.Windows.Forms.Label
        Me.txtApplyClassify = New System.Windows.Forms.TextBox
        Me.lblApplyClassify = New System.Windows.Forms.Label
        Me.lblApply = New System.Windows.Forms.Label
        Me.txtApplyDate = New System.Windows.Forms.TextBox
        Me.lblApplyDate = New System.Windows.Forms.Label
        Me.txtApplyNo = New System.Windows.Forms.TextBox
        Me.lblApplyNo = New System.Windows.Forms.Label
        Me.txtApplyArea = New System.Windows.Forms.TextBox
        Me.lblApplyArea = New System.Windows.Forms.Label
        Me.lblTitle = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnCancelChk = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.lblMeetingNo = New System.Windows.Forms.Label
        Me.txtMeetingNo = New System.Windows.Forms.TextBox
        Me.cboMeeting = New System.Windows.Forms.ComboBox
        Me.grpNameAndDate.SuspendLayout()
        CType(Me.cfgResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpReplace.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpNameAndDate
        '
        Me.grpNameAndDate.Controls.Add(Me.cfgResult)
        Me.grpNameAndDate.Controls.Add(Me.lblVersion)
        Me.grpNameAndDate.Controls.Add(Me.lblMemo)
        Me.grpNameAndDate.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpNameAndDate.Location = New System.Drawing.Point(62, 229)
        Me.grpNameAndDate.Name = "grpNameAndDate"
        Me.grpNameAndDate.Size = New System.Drawing.Size(900, 527)
        Me.grpNameAndDate.TabIndex = 91
        Me.grpNameAndDate.TabStop = False
        Me.grpNameAndDate.Text = "氏名及び月日"
        '
        'cfgResult
        '
        Me.cfgResult.AllowEditing = False
        Me.cfgResult.ColumnInfo = resources.GetString("cfgResult.ColumnInfo")
        Me.cfgResult.Location = New System.Drawing.Point(10, 22)
        Me.cfgResult.Name = "cfgResult"
        Me.cfgResult.Rows.Count = 1
        Me.cfgResult.Rows.DefaultSize = 22
        Me.cfgResult.Size = New System.Drawing.Size(884, 470)
        Me.cfgResult.StyleInfo = resources.GetString("cfgResult.StyleInfo")
        Me.cfgResult.TabIndex = 3
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.Location = New System.Drawing.Point(840, 498)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(50, 16)
        Me.lblVersion.TabIndex = 2
        Me.lblVersion.Text = "・・A・・"
        '
        'lblMemo
        '
        Me.lblMemo.AutoSize = True
        Me.lblMemo.Location = New System.Drawing.Point(211, 498)
        Me.lblMemo.Name = "lblMemo"
        Me.lblMemo.Size = New System.Drawing.Size(478, 16)
        Me.lblMemo.TabIndex = 1
        Me.lblMemo.Text = "表示されている日付を、クリックすることで取り消し設定を行うことができます。"
        '
        'txtStandName
        '
        Me.txtStandName.BackColor = System.Drawing.Color.White
        Me.txtStandName.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStandName.Location = New System.Drawing.Point(695, 192)
        Me.txtStandName.Name = "txtStandName"
        Me.txtStandName.ReadOnly = True
        Me.txtStandName.Size = New System.Drawing.Size(267, 23)
        Me.txtStandName.TabIndex = 90
        Me.txtStandName.TabStop = False
        Me.txtStandName.Text = "組合長　　ＷＷＷＷＷＷＷＷＷＷ"
        '
        'lblStandName
        '
        Me.lblStandName.AutoSize = True
        Me.lblStandName.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStandName.Location = New System.Drawing.Point(692, 171)
        Me.lblStandName.Name = "lblStandName"
        Me.lblStandName.Size = New System.Drawing.Size(56, 16)
        Me.lblStandName.TabIndex = 89
        Me.lblStandName.Text = "申請者"
        '
        'grpReplace
        '
        Me.grpReplace.Controls.Add(Me.btnReplace)
        Me.grpReplace.Controls.Add(Me.lblReplaceNo)
        Me.grpReplace.Controls.Add(Me.txtReplaceNo)
        Me.grpReplace.Controls.Add(Me.chkReplace)
        Me.grpReplace.Location = New System.Drawing.Point(695, 63)
        Me.grpReplace.Name = "grpReplace"
        Me.grpReplace.Size = New System.Drawing.Size(267, 100)
        Me.grpReplace.TabIndex = 88
        Me.grpReplace.TabStop = False
        '
        'btnReplace
        '
        Me.btnReplace.Enabled = False
        Me.btnReplace.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnReplace.Location = New System.Drawing.Point(131, 70)
        Me.btnReplace.Name = "btnReplace"
        Me.btnReplace.Size = New System.Drawing.Size(130, 23)
        Me.btnReplace.TabIndex = 30
        Me.btnReplace.TabStop = False
        Me.btnReplace.Text = "差替え内容表示"
        Me.btnReplace.UseVisualStyleBackColor = True
        '
        'lblReplaceNo
        '
        Me.lblReplaceNo.AutoSize = True
        Me.lblReplaceNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblReplaceNo.Location = New System.Drawing.Point(5, 40)
        Me.lblReplaceNo.Name = "lblReplaceNo"
        Me.lblReplaceNo.Size = New System.Drawing.Size(117, 16)
        Me.lblReplaceNo.TabIndex = 29
        Me.lblReplaceNo.Text = "差替え申請番号"
        '
        'txtReplaceNo
        '
        Me.txtReplaceNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtReplaceNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        Me.txtReplaceNo.Location = New System.Drawing.Point(131, 38)
        Me.txtReplaceNo.Name = "txtReplaceNo"
        Me.txtReplaceNo.ReadOnly = True
        Me.txtReplaceNo.Size = New System.Drawing.Size(128, 23)
        Me.txtReplaceNo.TabIndex = 29
        Me.txtReplaceNo.TabStop = False
        Me.txtReplaceNo.Text = "99-99"
        '
        'chkReplace
        '
        Me.chkReplace.AutoSize = True
        Me.chkReplace.Enabled = False
        Me.chkReplace.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.chkReplace.Location = New System.Drawing.Point(8, 11)
        Me.chkReplace.Name = "chkReplace"
        Me.chkReplace.Size = New System.Drawing.Size(75, 20)
        Me.chkReplace.TabIndex = 0
        Me.chkReplace.TabStop = False
        Me.chkReplace.Text = "差替え"
        Me.chkReplace.UseVisualStyleBackColor = True
        '
        'txtTerm
        '
        Me.txtTerm.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtTerm.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTerm.Location = New System.Drawing.Point(497, 136)
        Me.txtTerm.Name = "txtTerm"
        Me.txtTerm.ReadOnly = True
        Me.txtTerm.Size = New System.Drawing.Size(192, 23)
        Me.txtTerm.TabIndex = 87
        Me.txtTerm.TabStop = False
        Me.txtTerm.Text = "9999/99/99－9999/99/99"
        '
        'lblTerm
        '
        Me.lblTerm.AutoSize = True
        Me.lblTerm.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTerm.Location = New System.Drawing.Point(429, 139)
        Me.lblTerm.Name = "lblTerm"
        Me.lblTerm.Size = New System.Drawing.Size(72, 16)
        Me.lblTerm.TabIndex = 86
        Me.lblTerm.Text = "開催期間"
        '
        'lblMeeting
        '
        Me.lblMeeting.AutoSize = True
        Me.lblMeeting.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMeeting.Location = New System.Drawing.Point(77, 139)
        Me.lblMeeting.Name = "lblMeeting"
        Me.lblMeeting.Size = New System.Drawing.Size(56, 16)
        Me.lblMeeting.TabIndex = 84
        Me.lblMeeting.Text = "会議名"
        '
        'likMemo
        '
        Me.likMemo.AutoSize = True
        Me.likMemo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.likMemo.Location = New System.Drawing.Point(503, 103)
        Me.likMemo.Name = "likMemo"
        Me.likMemo.Size = New System.Drawing.Size(84, 16)
        Me.likMemo.TabIndex = 83
        Me.likMemo.TabStop = True
        Me.likMemo.Text = "覚書を表示"
        '
        'lblOmissionName
        '
        Me.lblOmissionName.AutoSize = True
        Me.lblOmissionName.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblOmissionName.Location = New System.Drawing.Point(408, 103)
        Me.lblOmissionName.Name = "lblOmissionName"
        Me.lblOmissionName.Size = New System.Drawing.Size(69, 16)
        Me.lblOmissionName.TabIndex = 82
        Me.lblOmissionName.Text = "覚書（へ）"
        '
        'txtApplyClassify
        '
        Me.txtApplyClassify.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtApplyClassify.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApplyClassify.Location = New System.Drawing.Point(139, 101)
        Me.txtApplyClassify.Name = "txtApplyClassify"
        Me.txtApplyClassify.ReadOnly = True
        Me.txtApplyClassify.Size = New System.Drawing.Size(264, 23)
        Me.txtApplyClassify.TabIndex = 81
        Me.txtApplyClassify.TabStop = False
        Me.txtApplyClassify.Text = "ＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷ"
        '
        'lblApplyClassify
        '
        Me.lblApplyClassify.AutoSize = True
        Me.lblApplyClassify.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblApplyClassify.Location = New System.Drawing.Point(93, 104)
        Me.lblApplyClassify.Name = "lblApplyClassify"
        Me.lblApplyClassify.Size = New System.Drawing.Size(40, 16)
        Me.lblApplyClassify.TabIndex = 80
        Me.lblApplyClassify.Text = "種類"
        '
        'lblApply
        '
        Me.lblApply.AutoSize = True
        Me.lblApply.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblApply.Location = New System.Drawing.Point(609, 75)
        Me.lblApply.Name = "lblApply"
        Me.lblApply.Size = New System.Drawing.Size(56, 16)
        Me.lblApply.TabIndex = 79
        Me.lblApply.Text = "【申請】"
        '
        'txtApplyDate
        '
        Me.txtApplyDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtApplyDate.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApplyDate.Location = New System.Drawing.Point(479, 72)
        Me.txtApplyDate.Name = "txtApplyDate"
        Me.txtApplyDate.ReadOnly = True
        Me.txtApplyDate.Size = New System.Drawing.Size(100, 23)
        Me.txtApplyDate.TabIndex = 78
        Me.txtApplyDate.TabStop = False
        Me.txtApplyDate.Text = "9999/99/99"
        '
        'lblApplyDate
        '
        Me.lblApplyDate.AutoSize = True
        Me.lblApplyDate.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblApplyDate.Location = New System.Drawing.Point(417, 75)
        Me.lblApplyDate.Name = "lblApplyDate"
        Me.lblApplyDate.Size = New System.Drawing.Size(56, 16)
        Me.lblApplyDate.TabIndex = 77
        Me.lblApplyDate.Text = "申請日"
        '
        'txtApplyNo
        '
        Me.txtApplyNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtApplyNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApplyNo.Location = New System.Drawing.Point(304, 72)
        Me.txtApplyNo.Name = "txtApplyNo"
        Me.txtApplyNo.ReadOnly = True
        Me.txtApplyNo.Size = New System.Drawing.Size(100, 23)
        Me.txtApplyNo.TabIndex = 76
        Me.txtApplyNo.TabStop = False
        Me.txtApplyNo.Text = "99999"
        '
        'lblApplyNo
        '
        Me.lblApplyNo.AutoSize = True
        Me.lblApplyNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblApplyNo.Location = New System.Drawing.Point(228, 75)
        Me.lblApplyNo.Name = "lblApplyNo"
        Me.lblApplyNo.Size = New System.Drawing.Size(72, 16)
        Me.lblApplyNo.TabIndex = 75
        Me.lblApplyNo.Text = "申請番号"
        '
        'txtApplyArea
        '
        Me.txtApplyArea.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtApplyArea.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApplyArea.Location = New System.Drawing.Point(139, 72)
        Me.txtApplyArea.Name = "txtApplyArea"
        Me.txtApplyArea.ReadOnly = True
        Me.txtApplyArea.Size = New System.Drawing.Size(87, 23)
        Me.txtApplyArea.TabIndex = 74
        Me.txtApplyArea.TabStop = False
        Me.txtApplyArea.Text = "ＷＷＷＷＷ"
        '
        'lblApplyArea
        '
        Me.lblApplyArea.AutoSize = True
        Me.lblApplyArea.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblApplyArea.Location = New System.Drawing.Point(93, 75)
        Me.lblApplyArea.Name = "lblApplyArea"
        Me.lblApplyArea.Size = New System.Drawing.Size(40, 16)
        Me.lblApplyArea.TabIndex = 73
        Me.lblApplyArea.Text = "支部"
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
        Me.lblTitle.TabIndex = 72
        Me.lblTitle.Text = "時間内組合活動 - 取消画面"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(842, 762)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 93
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnCancelChk
        '
        Me.btnCancelChk.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancelChk.Location = New System.Drawing.Point(711, 762)
        Me.btnCancelChk.Name = "btnCancelChk"
        Me.btnCancelChk.Size = New System.Drawing.Size(110, 30)
        Me.btnCancelChk.TabIndex = 92
        Me.btnCancelChk.Text = "取消確認"
        Me.btnCancelChk.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Enabled = False
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(65, 762)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 94
        Me.btnPrint.Text = "印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        Me.btnPrint.Visible = False
        '
        'lblMeetingNo
        '
        Me.lblMeetingNo.AutoSize = True
        Me.lblMeetingNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMeetingNo.Location = New System.Drawing.Point(429, 139)
        Me.lblMeetingNo.Name = "lblMeetingNo"
        Me.lblMeetingNo.Size = New System.Drawing.Size(104, 16)
        Me.lblMeetingNo.TabIndex = 115
        Me.lblMeetingNo.Text = "組合大会番号"
        '
        'txtMeetingNo
        '
        Me.txtMeetingNo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtMeetingNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtMeetingNo.Location = New System.Drawing.Point(542, 136)
        Me.txtMeetingNo.Name = "txtMeetingNo"
        Me.txtMeetingNo.ReadOnly = True
        Me.txtMeetingNo.Size = New System.Drawing.Size(126, 23)
        Me.txtMeetingNo.TabIndex = 116
        Me.txtMeetingNo.TabStop = False
        Me.txtMeetingNo.Text = "99-9"
        '
        'cboMeeting
        '
        Me.cboMeeting.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.cboMeeting.Enabled = False
        Me.cboMeeting.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        Me.cboMeeting.FormattingEnabled = True
        Me.cboMeeting.Location = New System.Drawing.Point(139, 136)
        Me.cboMeeting.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboMeeting.Name = "cboMeeting"
        Me.cboMeeting.Size = New System.Drawing.Size(264, 24)
        Me.cboMeeting.TabIndex = 117
        Me.cboMeeting.Text = "ＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷ"
        '
        'UC040306
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.cboMeeting)
        Me.Controls.Add(Me.txtMeetingNo)
        Me.Controls.Add(Me.lblMeetingNo)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnCancelChk)
        Me.Controls.Add(Me.grpNameAndDate)
        Me.Controls.Add(Me.txtStandName)
        Me.Controls.Add(Me.lblStandName)
        Me.Controls.Add(Me.grpReplace)
        Me.Controls.Add(Me.txtTerm)
        Me.Controls.Add(Me.lblTerm)
        Me.Controls.Add(Me.lblMeeting)
        Me.Controls.Add(Me.likMemo)
        Me.Controls.Add(Me.lblOmissionName)
        Me.Controls.Add(Me.txtApplyClassify)
        Me.Controls.Add(Me.lblApplyClassify)
        Me.Controls.Add(Me.lblApply)
        Me.Controls.Add(Me.txtApplyDate)
        Me.Controls.Add(Me.lblApplyDate)
        Me.Controls.Add(Me.txtApplyNo)
        Me.Controls.Add(Me.lblApplyNo)
        Me.Controls.Add(Me.txtApplyArea)
        Me.Controls.Add(Me.lblApplyArea)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC040306"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpNameAndDate.ResumeLayout(False)
        Me.grpNameAndDate.PerformLayout()
        CType(Me.cfgResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpReplace.ResumeLayout(False)
        Me.grpReplace.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents grpNameAndDate As System.Windows.Forms.GroupBox
    Friend WithEvents txtStandName As System.Windows.Forms.TextBox
    Friend WithEvents lblStandName As System.Windows.Forms.Label
    Friend WithEvents grpReplace As System.Windows.Forms.GroupBox
    Friend WithEvents btnReplace As System.Windows.Forms.Button
    Friend WithEvents lblReplaceNo As System.Windows.Forms.Label
    Friend WithEvents txtReplaceNo As System.Windows.Forms.TextBox
    Friend WithEvents chkReplace As System.Windows.Forms.CheckBox
    Friend WithEvents txtTerm As System.Windows.Forms.TextBox
    Friend WithEvents lblTerm As System.Windows.Forms.Label
    Friend WithEvents lblMeeting As System.Windows.Forms.Label
    Friend WithEvents likMemo As System.Windows.Forms.LinkLabel
    Friend WithEvents lblOmissionName As System.Windows.Forms.Label
    Friend WithEvents txtApplyClassify As System.Windows.Forms.TextBox
    Friend WithEvents lblApplyClassify As System.Windows.Forms.Label
    Friend WithEvents lblApply As System.Windows.Forms.Label
    Friend WithEvents txtApplyDate As System.Windows.Forms.TextBox
    Friend WithEvents lblApplyDate As System.Windows.Forms.Label
    Friend WithEvents txtApplyNo As System.Windows.Forms.TextBox
    Friend WithEvents lblApplyNo As System.Windows.Forms.Label
    Friend WithEvents txtApplyArea As System.Windows.Forms.TextBox
    Friend WithEvents lblApplyArea As System.Windows.Forms.Label
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnCancelChk As System.Windows.Forms.Button
    Friend WithEvents lblMemo As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents cfgResult As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblMeetingNo As System.Windows.Forms.Label
    Friend WithEvents txtMeetingNo As System.Windows.Forms.TextBox
    Friend WithEvents cboMeeting As System.Windows.Forms.ComboBox

End Class
