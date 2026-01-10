<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040302
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC040302))
        Me.btnRemove = New System.Windows.Forms.Button
        Me.btnAdd = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnConfirm = New System.Windows.Forms.Button
        Me.txtComExe = New System.Windows.Forms.TextBox
        Me.lblKikan = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lklMemo = New System.Windows.Forms.LinkLabel
        Me.lblOmission = New System.Windows.Forms.Label
        Me.txtApplyClassify = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtApplyDate = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtApplyNumber = New System.Windows.Forms.TextBox
        Me.lblApplyNumber1 = New System.Windows.Forms.Label
        Me.txtApplyArea = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.txtStandName = New System.Windows.Forms.TextBox
        Me.lblStandName = New System.Windows.Forms.Label
        Me.grpReplace = New System.Windows.Forms.GroupBox
        Me.cmbReplaceNumber = New System.Windows.Forms.ComboBox
        Me.btnReplaceNumber = New System.Windows.Forms.Button
        Me.lblReplaceNumber1 = New System.Windows.Forms.Label
        Me.chkReplace = New System.Windows.Forms.CheckBox
        Me.lblKindTitle = New System.Windows.Forms.Label
        Me.grpNameAndDate = New System.Windows.Forms.GroupBox
        Me.flxNameAndDate = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.lblSUp = New System.Windows.Forms.Label
        Me.lblTermID = New System.Windows.Forms.Label
        Me.btnPrinting = New System.Windows.Forms.Button
        Me.lblApplyArea = New System.Windows.Forms.Label
        Me.lblApplyClassify = New System.Windows.Forms.Label
        Me.lblStrikeID = New System.Windows.Forms.Label
        Me.txtMeetingNo = New System.Windows.Forms.TextBox
        Me.lblMeetingNo = New System.Windows.Forms.Label
        Me.btnModify = New System.Windows.Forms.Button
        Me.grpFrameCount = New System.Windows.Forms.GroupBox
        Me.lblUsedFrameCount = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblRestFrameCount = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblDateTo = New System.Windows.Forms.Label
        Me.lblFrom = New System.Windows.Forms.Label
        Me.lblDateFrom = New System.Windows.Forms.Label
        Me.cmbApplyMeetinglist = New System.Windows.Forms.ComboBox
        Me.lblISDelete = New System.Windows.Forms.Label
        Me.grpReplace.SuspendLayout()
        Me.grpNameAndDate.SuspendLayout()
        CType(Me.flxNameAndDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpFrameCount.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnRemove
        '
        Me.btnRemove.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnRemove.Location = New System.Drawing.Point(484, 484)
        Me.btnRemove.Name = "btnRemove"
        Me.btnRemove.Size = New System.Drawing.Size(110, 30)
        Me.btnRemove.TabIndex = 32
        Me.btnRemove.Text = "選択行の削除"
        Me.btnRemove.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAdd.Location = New System.Drawing.Point(307, 484)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(110, 30)
        Me.btnAdd.TabIndex = 1
        Me.btnAdd.Text = "組合員の追加"
        Me.btnAdd.UseVisualStyleBackColor = True
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
        Me.Label11.TabIndex = 52
        Me.Label11.Text = "時間内組合活動 - 申請画面"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(842, 762)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 73
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnConfirm
        '
        Me.btnConfirm.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnConfirm.Location = New System.Drawing.Point(711, 762)
        Me.btnConfirm.Name = "btnConfirm"
        Me.btnConfirm.Size = New System.Drawing.Size(110, 30)
        Me.btnConfirm.TabIndex = 72
        Me.btnConfirm.Text = "登録確認"
        Me.btnConfirm.UseVisualStyleBackColor = True
        '
        'txtComExe
        '
        Me.txtComExe.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtComExe.Location = New System.Drawing.Point(497, 133)
        Me.txtComExe.Name = "txtComExe"
        Me.txtComExe.Size = New System.Drawing.Size(192, 23)
        Me.txtComExe.TabIndex = 101
        '
        'lblKikan
        '
        Me.lblKikan.AutoSize = True
        Me.lblKikan.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblKikan.Location = New System.Drawing.Point(428, 136)
        Me.lblKikan.Name = "lblKikan"
        Me.lblKikan.Size = New System.Drawing.Size(72, 16)
        Me.lblKikan.TabIndex = 100
        Me.lblKikan.Text = "開催期間"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label7.Location = New System.Drawing.Point(77, 136)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(56, 16)
        Me.Label7.TabIndex = 98
        Me.Label7.Text = "会議名"
        '
        'lklMemo
        '
        Me.lklMemo.AutoSize = True
        Me.lklMemo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lklMemo.Location = New System.Drawing.Point(507, 103)
        Me.lklMemo.Name = "lklMemo"
        Me.lklMemo.Size = New System.Drawing.Size(84, 16)
        Me.lklMemo.TabIndex = 97
        Me.lklMemo.TabStop = True
        Me.lklMemo.Text = "覚書を表示"
        '
        'lblOmission
        '
        Me.lblOmission.AutoSize = True
        Me.lblOmission.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblOmission.Location = New System.Drawing.Point(408, 103)
        Me.lblOmission.Name = "lblOmission"
        Me.lblOmission.Size = New System.Drawing.Size(69, 16)
        Me.lblOmission.TabIndex = 96
        Me.lblOmission.Text = "覚書（へ）"
        '
        'txtApplyClassify
        '
        Me.txtApplyClassify.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApplyClassify.Location = New System.Drawing.Point(139, 101)
        Me.txtApplyClassify.Name = "txtApplyClassify"
        Me.txtApplyClassify.Size = New System.Drawing.Size(264, 23)
        Me.txtApplyClassify.TabIndex = 95
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.Location = New System.Drawing.Point(93, 104)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 16)
        Me.Label5.TabIndex = 94
        Me.Label5.Text = "種類"
        '
        'txtApplyDate
        '
        Me.txtApplyDate.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApplyDate.Location = New System.Drawing.Point(479, 72)
        Me.txtApplyDate.Name = "txtApplyDate"
        Me.txtApplyDate.Size = New System.Drawing.Size(100, 23)
        Me.txtApplyDate.TabIndex = 93
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(417, 75)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 92
        Me.Label3.Text = "申請日"
        '
        'txtApplyNumber
        '
        Me.txtApplyNumber.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApplyNumber.Location = New System.Drawing.Point(304, 72)
        Me.txtApplyNumber.Name = "txtApplyNumber"
        Me.txtApplyNumber.Size = New System.Drawing.Size(100, 23)
        Me.txtApplyNumber.TabIndex = 91
        '
        'lblApplyNumber1
        '
        Me.lblApplyNumber1.AutoSize = True
        Me.lblApplyNumber1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblApplyNumber1.Location = New System.Drawing.Point(228, 75)
        Me.lblApplyNumber1.Name = "lblApplyNumber1"
        Me.lblApplyNumber1.Size = New System.Drawing.Size(72, 16)
        Me.lblApplyNumber1.TabIndex = 90
        Me.lblApplyNumber1.Text = "申請番号"
        '
        'txtApplyArea
        '
        Me.txtApplyArea.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtApplyArea.Location = New System.Drawing.Point(139, 72)
        Me.txtApplyArea.Name = "txtApplyArea"
        Me.txtApplyArea.Size = New System.Drawing.Size(87, 23)
        Me.txtApplyArea.TabIndex = 89
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(93, 75)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 16)
        Me.Label1.TabIndex = 88
        Me.Label1.Text = "支部"
        '
        'txtStandName
        '
        Me.txtStandName.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStandName.Location = New System.Drawing.Point(695, 191)
        Me.txtStandName.Name = "txtStandName"
        Me.txtStandName.Size = New System.Drawing.Size(267, 23)
        Me.txtStandName.TabIndex = 105
        '
        'lblStandName
        '
        Me.lblStandName.AutoSize = True
        Me.lblStandName.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStandName.Location = New System.Drawing.Point(692, 171)
        Me.lblStandName.Name = "lblStandName"
        Me.lblStandName.Size = New System.Drawing.Size(56, 16)
        Me.lblStandName.TabIndex = 104
        Me.lblStandName.Text = "申請者"
        '
        'grpReplace
        '
        Me.grpReplace.Controls.Add(Me.cmbReplaceNumber)
        Me.grpReplace.Controls.Add(Me.btnReplaceNumber)
        Me.grpReplace.Controls.Add(Me.lblReplaceNumber1)
        Me.grpReplace.Controls.Add(Me.chkReplace)
        Me.grpReplace.Location = New System.Drawing.Point(695, 63)
        Me.grpReplace.Name = "grpReplace"
        Me.grpReplace.Size = New System.Drawing.Size(267, 100)
        Me.grpReplace.TabIndex = 103
        Me.grpReplace.TabStop = False
        '
        'cmbReplaceNumber
        '
        Me.cmbReplaceNumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbReplaceNumber.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        Me.cmbReplaceNumber.FormattingEnabled = True
        Me.cmbReplaceNumber.Location = New System.Drawing.Point(132, 38)
        Me.cmbReplaceNumber.Name = "cmbReplaceNumber"
        Me.cmbReplaceNumber.Size = New System.Drawing.Size(128, 24)
        Me.cmbReplaceNumber.TabIndex = 31
        '
        'btnReplaceNumber
        '
        Me.btnReplaceNumber.Font = New System.Drawing.Font("ＭＳ ゴシック", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnReplaceNumber.Location = New System.Drawing.Point(131, 70)
        Me.btnReplaceNumber.Name = "btnReplaceNumber"
        Me.btnReplaceNumber.Size = New System.Drawing.Size(130, 23)
        Me.btnReplaceNumber.TabIndex = 30
        Me.btnReplaceNumber.Text = "差替え内容表示"
        Me.btnReplaceNumber.UseVisualStyleBackColor = True
        '
        'lblReplaceNumber1
        '
        Me.lblReplaceNumber1.AutoSize = True
        Me.lblReplaceNumber1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblReplaceNumber1.Location = New System.Drawing.Point(5, 40)
        Me.lblReplaceNumber1.Name = "lblReplaceNumber1"
        Me.lblReplaceNumber1.Size = New System.Drawing.Size(117, 16)
        Me.lblReplaceNumber1.TabIndex = 29
        Me.lblReplaceNumber1.Text = "差替え申請番号"
        '
        'chkReplace
        '
        Me.chkReplace.AutoSize = True
        Me.chkReplace.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.chkReplace.Location = New System.Drawing.Point(8, 11)
        Me.chkReplace.Name = "chkReplace"
        Me.chkReplace.Size = New System.Drawing.Size(75, 20)
        Me.chkReplace.TabIndex = 0
        Me.chkReplace.Text = "差替え"
        Me.chkReplace.UseVisualStyleBackColor = True
        '
        'lblKindTitle
        '
        Me.lblKindTitle.AutoSize = True
        Me.lblKindTitle.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblKindTitle.Location = New System.Drawing.Point(609, 75)
        Me.lblKindTitle.Name = "lblKindTitle"
        Me.lblKindTitle.Size = New System.Drawing.Size(56, 16)
        Me.lblKindTitle.TabIndex = 102
        Me.lblKindTitle.Text = "【申請】"
        '
        'grpNameAndDate
        '
        Me.grpNameAndDate.Controls.Add(Me.flxNameAndDate)
        Me.grpNameAndDate.Controls.Add(Me.lblSUp)
        Me.grpNameAndDate.Controls.Add(Me.btnRemove)
        Me.grpNameAndDate.Controls.Add(Me.btnAdd)
        Me.grpNameAndDate.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpNameAndDate.Location = New System.Drawing.Point(62, 229)
        Me.grpNameAndDate.Name = "grpNameAndDate"
        Me.grpNameAndDate.Size = New System.Drawing.Size(900, 527)
        Me.grpNameAndDate.TabIndex = 106
        Me.grpNameAndDate.TabStop = False
        Me.grpNameAndDate.Text = "氏名及び月日"
        '
        'flxNameAndDate
        '
        Me.flxNameAndDate.ColumnInfo = resources.GetString("flxNameAndDate.ColumnInfo")
        Me.flxNameAndDate.Location = New System.Drawing.Point(3, 19)
        Me.flxNameAndDate.Name = "flxNameAndDate"
        Me.flxNameAndDate.Rows.Count = 1
        Me.flxNameAndDate.Rows.DefaultSize = 22
        Me.flxNameAndDate.Size = New System.Drawing.Size(897, 459)
        Me.flxNameAndDate.StyleInfo = resources.GetString("flxNameAndDate.StyleInfo")
        Me.flxNameAndDate.TabIndex = 33
        '
        'lblSUp
        '
        Me.lblSUp.AutoSize = True
        Me.lblSUp.Location = New System.Drawing.Point(840, 498)
        Me.lblSUp.Name = "lblSUp"
        Me.lblSUp.Size = New System.Drawing.Size(50, 16)
        Me.lblSUp.TabIndex = 2
        Me.lblSUp.Text = "・・A・・"
        '
        'lblTermID
        '
        Me.lblTermID.AutoSize = True
        Me.lblTermID.Location = New System.Drawing.Point(5, 44)
        Me.lblTermID.Name = "lblTermID"
        Me.lblTermID.Size = New System.Drawing.Size(42, 12)
        Me.lblTermID.TabIndex = 107
        Me.lblTermID.Text = "TermID"
        Me.lblTermID.Visible = False
        '
        'btnPrinting
        '
        Me.btnPrinting.Location = New System.Drawing.Point(65, 762)
        Me.btnPrinting.Name = "btnPrinting"
        Me.btnPrinting.Size = New System.Drawing.Size(98, 29)
        Me.btnPrinting.TabIndex = 108
        Me.btnPrinting.Text = "印刷"
        Me.btnPrinting.UseVisualStyleBackColor = True
        '
        'lblApplyArea
        '
        Me.lblApplyArea.AutoSize = True
        Me.lblApplyArea.Location = New System.Drawing.Point(3, 8)
        Me.lblApplyArea.Name = "lblApplyArea"
        Me.lblApplyArea.Size = New System.Drawing.Size(58, 12)
        Me.lblApplyArea.TabIndex = 111
        Me.lblApplyArea.Text = "ApplyArea"
        Me.lblApplyArea.Visible = False
        '
        'lblApplyClassify
        '
        Me.lblApplyClassify.AutoSize = True
        Me.lblApplyClassify.Location = New System.Drawing.Point(3, 20)
        Me.lblApplyClassify.Name = "lblApplyClassify"
        Me.lblApplyClassify.Size = New System.Drawing.Size(76, 12)
        Me.lblApplyClassify.TabIndex = 112
        Me.lblApplyClassify.Text = "ApplyClassify"
        Me.lblApplyClassify.Visible = False
        '
        'lblStrikeID
        '
        Me.lblStrikeID.AutoSize = True
        Me.lblStrikeID.Location = New System.Drawing.Point(18, 111)
        Me.lblStrikeID.Name = "lblStrikeID"
        Me.lblStrikeID.Size = New System.Drawing.Size(0, 12)
        Me.lblStrikeID.TabIndex = 113
        Me.lblStrikeID.Visible = False
        '
        'txtMeetingNo
        '
        Me.txtMeetingNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtMeetingNo.Location = New System.Drawing.Point(542, 133)
        Me.txtMeetingNo.Name = "txtMeetingNo"
        Me.txtMeetingNo.Size = New System.Drawing.Size(126, 23)
        Me.txtMeetingNo.TabIndex = 115
        '
        'lblMeetingNo
        '
        Me.lblMeetingNo.AutoSize = True
        Me.lblMeetingNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMeetingNo.Location = New System.Drawing.Point(429, 136)
        Me.lblMeetingNo.Name = "lblMeetingNo"
        Me.lblMeetingNo.Size = New System.Drawing.Size(104, 16)
        Me.lblMeetingNo.TabIndex = 114
        Me.lblMeetingNo.Text = "組合大会番号"
        '
        'btnModify
        '
        Me.btnModify.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnModify.Location = New System.Drawing.Point(711, 762)
        Me.btnModify.Name = "btnModify"
        Me.btnModify.Size = New System.Drawing.Size(110, 30)
        Me.btnModify.TabIndex = 116
        Me.btnModify.Text = "内容変更"
        Me.btnModify.UseVisualStyleBackColor = True
        '
        'grpFrameCount
        '
        Me.grpFrameCount.Controls.Add(Me.lblUsedFrameCount)
        Me.grpFrameCount.Controls.Add(Me.Label6)
        Me.grpFrameCount.Controls.Add(Me.lblRestFrameCount)
        Me.grpFrameCount.Controls.Add(Me.Label2)
        Me.grpFrameCount.Controls.Add(Me.lblDateTo)
        Me.grpFrameCount.Controls.Add(Me.lblFrom)
        Me.grpFrameCount.Controls.Add(Me.lblDateFrom)
        Me.grpFrameCount.Location = New System.Drawing.Point(65, 161)
        Me.grpFrameCount.Name = "grpFrameCount"
        Me.grpFrameCount.Size = New System.Drawing.Size(621, 62)
        Me.grpFrameCount.TabIndex = 117
        Me.grpFrameCount.TabStop = False
        Me.grpFrameCount.Visible = False
        '
        'lblUsedFrameCount
        '
        Me.lblUsedFrameCount.AutoSize = True
        Me.lblUsedFrameCount.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblUsedFrameCount.Location = New System.Drawing.Point(444, 27)
        Me.lblUsedFrameCount.Name = "lblUsedFrameCount"
        Me.lblUsedFrameCount.Size = New System.Drawing.Size(44, 16)
        Me.lblUsedFrameCount.TabIndex = 95
        Me.lblUsedFrameCount.Text = "XX回"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label6.Location = New System.Drawing.Point(344, 27)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(96, 16)
        Me.Label6.TabIndex = 94
        Me.Label6.Text = "使用済枠数："
        '
        'lblRestFrameCount
        '
        Me.lblRestFrameCount.AutoSize = True
        Me.lblRestFrameCount.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblRestFrameCount.Location = New System.Drawing.Point(190, 27)
        Me.lblRestFrameCount.Name = "lblRestFrameCount"
        Me.lblRestFrameCount.Size = New System.Drawing.Size(88, 16)
        Me.lblRestFrameCount.TabIndex = 93
        Me.lblRestFrameCount.Text = "XX回/XX回"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(124, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 16)
        Me.Label2.TabIndex = 92
        Me.Label2.Text = "枠残数："
        '
        'lblDateTo
        '
        Me.lblDateTo.AutoSize = True
        Me.lblDateTo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblDateTo.Location = New System.Drawing.Point(28, 41)
        Me.lblDateTo.Name = "lblDateTo"
        Me.lblDateTo.Size = New System.Drawing.Size(64, 16)
        Me.lblDateTo.TabIndex = 91
        Me.lblDateTo.Text = "2011/12"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblFrom.Location = New System.Drawing.Point(45, 27)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(24, 16)
        Me.lblFrom.TabIndex = 90
        Me.lblFrom.Text = "～"
        Me.lblFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblDateFrom
        '
        Me.lblDateFrom.AutoSize = True
        Me.lblDateFrom.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblDateFrom.Location = New System.Drawing.Point(28, 11)
        Me.lblDateFrom.Name = "lblDateFrom"
        Me.lblDateFrom.Size = New System.Drawing.Size(64, 16)
        Me.lblDateFrom.TabIndex = 89
        Me.lblDateFrom.Text = "2011/12"
        '
        'cmbApplyMeetinglist
        '
        Me.cmbApplyMeetinglist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbApplyMeetinglist.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        Me.cmbApplyMeetinglist.FormattingEnabled = True
        Me.cmbApplyMeetinglist.Location = New System.Drawing.Point(139, 133)
        Me.cmbApplyMeetinglist.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cmbApplyMeetinglist.Name = "cmbApplyMeetinglist"
        Me.cmbApplyMeetinglist.Size = New System.Drawing.Size(264, 24)
        Me.cmbApplyMeetinglist.TabIndex = 118
        '
        'lblISDelete
        '
        Me.lblISDelete.AutoSize = True
        Me.lblISDelete.Location = New System.Drawing.Point(5, 32)
        Me.lblISDelete.Name = "lblISDelete"
        Me.lblISDelete.Size = New System.Drawing.Size(47, 12)
        Me.lblISDelete.TabIndex = 119
        Me.lblISDelete.Text = "IsDelete"
        Me.lblISDelete.Visible = False
        '
        'UC040302
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblISDelete)
        Me.Controls.Add(Me.cmbApplyMeetinglist)
        Me.Controls.Add(Me.grpFrameCount)
        Me.Controls.Add(Me.btnModify)
        Me.Controls.Add(Me.txtMeetingNo)
        Me.Controls.Add(Me.lblMeetingNo)
        Me.Controls.Add(Me.lblStrikeID)
        Me.Controls.Add(Me.lblApplyClassify)
        Me.Controls.Add(Me.lblApplyArea)
        Me.Controls.Add(Me.btnPrinting)
        Me.Controls.Add(Me.lblTermID)
        Me.Controls.Add(Me.grpNameAndDate)
        Me.Controls.Add(Me.txtStandName)
        Me.Controls.Add(Me.lblStandName)
        Me.Controls.Add(Me.grpReplace)
        Me.Controls.Add(Me.lblKindTitle)
        Me.Controls.Add(Me.txtComExe)
        Me.Controls.Add(Me.lblKikan)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lklMemo)
        Me.Controls.Add(Me.lblOmission)
        Me.Controls.Add(Me.txtApplyClassify)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.txtApplyDate)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtApplyNumber)
        Me.Controls.Add(Me.lblApplyNumber1)
        Me.Controls.Add(Me.txtApplyArea)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnConfirm)
        Me.Controls.Add(Me.Label11)
        Me.Name = "UC040302"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpReplace.ResumeLayout(False)
        Me.grpReplace.PerformLayout()
        Me.grpNameAndDate.ResumeLayout(False)
        Me.grpNameAndDate.PerformLayout()
        CType(Me.flxNameAndDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpFrameCount.ResumeLayout(False)
        Me.grpFrameCount.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnRemove As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnConfirm As System.Windows.Forms.Button
    Friend WithEvents txtComExe As System.Windows.Forms.TextBox
    Friend WithEvents lblKikan As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lklMemo As System.Windows.Forms.LinkLabel
    Friend WithEvents lblOmission As System.Windows.Forms.Label
    Friend WithEvents txtApplyClassify As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtApplyDate As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtApplyNumber As System.Windows.Forms.TextBox
    Friend WithEvents lblApplyNumber1 As System.Windows.Forms.Label
    Friend WithEvents txtApplyArea As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtStandName As System.Windows.Forms.TextBox
    Friend WithEvents lblStandName As System.Windows.Forms.Label
    Friend WithEvents grpReplace As System.Windows.Forms.GroupBox
    Friend WithEvents btnReplaceNumber As System.Windows.Forms.Button
    Friend WithEvents lblReplaceNumber1 As System.Windows.Forms.Label
    Friend WithEvents chkReplace As System.Windows.Forms.CheckBox
    Friend WithEvents lblKindTitle As System.Windows.Forms.Label
    Friend WithEvents grpNameAndDate As System.Windows.Forms.GroupBox
    Friend WithEvents lblSUp As System.Windows.Forms.Label
    Friend WithEvents lblTermID As System.Windows.Forms.Label
    Friend WithEvents flxNameAndDate As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnPrinting As System.Windows.Forms.Button
    Friend WithEvents lblApplyArea As System.Windows.Forms.Label
    Friend WithEvents lblApplyClassify As System.Windows.Forms.Label
    Friend WithEvents cmbReplaceNumber As System.Windows.Forms.ComboBox
    Friend WithEvents lblStrikeID As System.Windows.Forms.Label
    Friend WithEvents txtMeetingNo As System.Windows.Forms.TextBox
    Friend WithEvents lblMeetingNo As System.Windows.Forms.Label
    Friend WithEvents btnModify As System.Windows.Forms.Button
    Friend WithEvents grpFrameCount As System.Windows.Forms.GroupBox
    Friend WithEvents lblDateFrom As System.Windows.Forms.Label
    Friend WithEvents lblDateTo As System.Windows.Forms.Label
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents lblUsedFrameCount As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblRestFrameCount As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmbApplyMeetinglist As System.Windows.Forms.ComboBox
    Friend WithEvents lblISDelete As System.Windows.Forms.Label

End Class
