<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040102
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
        Me.lblTitle = New System.Windows.Forms.Label
        Me.grpMain = New System.Windows.Forms.GroupBox
        Me.lblUnionMeetingNo = New System.Windows.Forms.Label
        Me.lblCreateDate = New System.Windows.Forms.Label
        Me.lblIndispensableMeetingTimeFrom1 = New System.Windows.Forms.Label
        Me.lblHatsu3 = New System.Windows.Forms.Label
        Me.mtbDFlightBack2 = New System.Windows.Forms.MaskedTextBox
        Me.mtbDFlightBack1 = New System.Windows.Forms.MaskedTextBox
        Me.mtbDFlight1 = New System.Windows.Forms.MaskedTextBox
        Me.mtbMeetingTimeFrom2 = New System.Windows.Forms.MaskedTextBox
        Me.mtbMeetingTimeTo2 = New System.Windows.Forms.MaskedTextBox
        Me.mtbMeetingTimeFrom1 = New System.Windows.Forms.MaskedTextBox
        Me.mtbMeetingTimeTo1 = New System.Windows.Forms.MaskedTextBox
        Me.dtpMeeting2 = New System.Windows.Forms.DateTimePicker
        Me.dtpMeeting1 = New System.Windows.Forms.DateTimePicker
        Me.lblCreateDateLabel = New System.Windows.Forms.Label
        Me.grpInfomationName = New System.Windows.Forms.GroupBox
        Me.optInfomationName3 = New System.Windows.Forms.RadioButton
        Me.optInfomationName2 = New System.Windows.Forms.RadioButton
        Me.optInfomationName1 = New System.Windows.Forms.RadioButton
        Me.grpUnionType = New System.Windows.Forms.GroupBox
        Me.optUnionType3 = New System.Windows.Forms.RadioButton
        Me.optUnionType2 = New System.Windows.Forms.RadioButton
        Me.optUnionType1 = New System.Windows.Forms.RadioButton
        Me.txtBiko2 = New System.Windows.Forms.TextBox
        Me.lblBiko2 = New System.Windows.Forms.Label
        Me.txtSubject1 = New System.Windows.Forms.TextBox
        Me.lblSubject1 = New System.Windows.Forms.Label
        Me.txtBiko1 = New System.Windows.Forms.TextBox
        Me.lblBiko1 = New System.Windows.Forms.Label
        Me.chkExchangeMeeting2 = New System.Windows.Forms.CheckBox
        Me.chkLunch2 = New System.Windows.Forms.CheckBox
        Me.txtPlace2 = New System.Windows.Forms.TextBox
        Me.lblPlace2 = New System.Windows.Forms.Label
        Me.lblLFlightBack2 = New System.Windows.Forms.Label
        Me.lblLFlightBack1 = New System.Windows.Forms.Label
        Me.lblFlightBack2 = New System.Windows.Forms.Label
        Me.txtLFlightBack2 = New System.Windows.Forms.TextBox
        Me.lblMeeting2 = New System.Windows.Forms.Label
        Me.lblKara2 = New System.Windows.Forms.Label
        Me.chkMeeting2 = New System.Windows.Forms.CheckBox
        Me.chkExchangeMeeting1 = New System.Windows.Forms.CheckBox
        Me.chkLunch1 = New System.Windows.Forms.CheckBox
        Me.txtPlace1 = New System.Windows.Forms.TextBox
        Me.lblPlace1 = New System.Windows.Forms.Label
        Me.lblHatsu2 = New System.Windows.Forms.Label
        Me.txtLFlightBack1 = New System.Windows.Forms.TextBox
        Me.lblFlightBack1 = New System.Windows.Forms.Label
        Me.lblFlight1 = New System.Windows.Forms.Label
        Me.lblHatsu1 = New System.Windows.Forms.Label
        Me.txtLFlight1 = New System.Windows.Forms.TextBox
        Me.lblKara1 = New System.Windows.Forms.Label
        Me.lblIndispensableMeeting1 = New System.Windows.Forms.Label
        Me.lblOpenBelongingMemo = New System.Windows.Forms.Label
        Me.lblMeeting1 = New System.Windows.Forms.Label
        Me.lblIndispensableOpenBelonging = New System.Windows.Forms.Label
        Me.cboOpenBelonging = New System.Windows.Forms.ComboBox
        Me.lblOpenBelonging = New System.Windows.Forms.Label
        Me.lblIndispensableApplyArea = New System.Windows.Forms.Label
        Me.cboApplyArea = New System.Windows.Forms.ComboBox
        Me.lblUnionMeetingNoLabel = New System.Windows.Forms.Label
        Me.lblApplyArea = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnInsertChk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.grpMain.SuspendLayout()
        Me.grpInfomationName.SuspendLayout()
        Me.grpUnionType.SuspendLayout()
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
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "組合大会通知 - 詳細"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpMain
        '
        Me.grpMain.BackColor = System.Drawing.SystemColors.Control
        Me.grpMain.Controls.Add(Me.lblUnionMeetingNo)
        Me.grpMain.Controls.Add(Me.lblCreateDate)
        Me.grpMain.Controls.Add(Me.lblIndispensableMeetingTimeFrom1)
        Me.grpMain.Controls.Add(Me.lblHatsu3)
        Me.grpMain.Controls.Add(Me.mtbDFlightBack2)
        Me.grpMain.Controls.Add(Me.mtbDFlightBack1)
        Me.grpMain.Controls.Add(Me.mtbDFlight1)
        Me.grpMain.Controls.Add(Me.mtbMeetingTimeFrom2)
        Me.grpMain.Controls.Add(Me.mtbMeetingTimeTo2)
        Me.grpMain.Controls.Add(Me.mtbMeetingTimeFrom1)
        Me.grpMain.Controls.Add(Me.mtbMeetingTimeTo1)
        Me.grpMain.Controls.Add(Me.dtpMeeting2)
        Me.grpMain.Controls.Add(Me.dtpMeeting1)
        Me.grpMain.Controls.Add(Me.lblCreateDateLabel)
        Me.grpMain.Controls.Add(Me.grpInfomationName)
        Me.grpMain.Controls.Add(Me.grpUnionType)
        Me.grpMain.Controls.Add(Me.txtBiko2)
        Me.grpMain.Controls.Add(Me.lblBiko2)
        Me.grpMain.Controls.Add(Me.txtSubject1)
        Me.grpMain.Controls.Add(Me.lblSubject1)
        Me.grpMain.Controls.Add(Me.txtBiko1)
        Me.grpMain.Controls.Add(Me.lblBiko1)
        Me.grpMain.Controls.Add(Me.chkExchangeMeeting2)
        Me.grpMain.Controls.Add(Me.chkLunch2)
        Me.grpMain.Controls.Add(Me.txtPlace2)
        Me.grpMain.Controls.Add(Me.lblPlace2)
        Me.grpMain.Controls.Add(Me.lblLFlightBack2)
        Me.grpMain.Controls.Add(Me.lblLFlightBack1)
        Me.grpMain.Controls.Add(Me.lblFlightBack2)
        Me.grpMain.Controls.Add(Me.txtLFlightBack2)
        Me.grpMain.Controls.Add(Me.lblMeeting2)
        Me.grpMain.Controls.Add(Me.lblKara2)
        Me.grpMain.Controls.Add(Me.chkMeeting2)
        Me.grpMain.Controls.Add(Me.chkExchangeMeeting1)
        Me.grpMain.Controls.Add(Me.chkLunch1)
        Me.grpMain.Controls.Add(Me.txtPlace1)
        Me.grpMain.Controls.Add(Me.lblPlace1)
        Me.grpMain.Controls.Add(Me.lblHatsu2)
        Me.grpMain.Controls.Add(Me.txtLFlightBack1)
        Me.grpMain.Controls.Add(Me.lblFlightBack1)
        Me.grpMain.Controls.Add(Me.lblFlight1)
        Me.grpMain.Controls.Add(Me.lblHatsu1)
        Me.grpMain.Controls.Add(Me.txtLFlight1)
        Me.grpMain.Controls.Add(Me.lblKara1)
        Me.grpMain.Controls.Add(Me.lblIndispensableMeeting1)
        Me.grpMain.Controls.Add(Me.lblOpenBelongingMemo)
        Me.grpMain.Controls.Add(Me.lblMeeting1)
        Me.grpMain.Controls.Add(Me.lblIndispensableOpenBelonging)
        Me.grpMain.Controls.Add(Me.cboOpenBelonging)
        Me.grpMain.Controls.Add(Me.lblOpenBelonging)
        Me.grpMain.Controls.Add(Me.lblIndispensableApplyArea)
        Me.grpMain.Controls.Add(Me.cboApplyArea)
        Me.grpMain.Controls.Add(Me.lblUnionMeetingNoLabel)
        Me.grpMain.Controls.Add(Me.lblApplyArea)
        Me.grpMain.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpMain.Location = New System.Drawing.Point(79, 78)
        Me.grpMain.Name = "grpMain"
        Me.grpMain.Size = New System.Drawing.Size(900, 677)
        Me.grpMain.TabIndex = 1
        Me.grpMain.TabStop = False
        '
        'lblUnionMeetingNo
        '
        Me.lblUnionMeetingNo.AutoSize = True
        Me.lblUnionMeetingNo.Location = New System.Drawing.Point(120, 19)
        Me.lblUnionMeetingNo.Name = "lblUnionMeetingNo"
        Me.lblUnionMeetingNo.Size = New System.Drawing.Size(40, 16)
        Me.lblUnionMeetingNo.TabIndex = 2
        Me.lblUnionMeetingNo.Text = "99-9"
        '
        'lblCreateDate
        '
        Me.lblCreateDate.AutoSize = True
        Me.lblCreateDate.Location = New System.Drawing.Point(793, 19)
        Me.lblCreateDate.Name = "lblCreateDate"
        Me.lblCreateDate.Size = New System.Drawing.Size(88, 16)
        Me.lblCreateDate.TabIndex = 4
        Me.lblCreateDate.Text = "9999/99/99"
        '
        'lblIndispensableMeetingTimeFrom1
        '
        Me.lblIndispensableMeetingTimeFrom1.AutoSize = True
        Me.lblIndispensableMeetingTimeFrom1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableMeetingTimeFrom1.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableMeetingTimeFrom1.Location = New System.Drawing.Point(292, 141)
        Me.lblIndispensableMeetingTimeFrom1.Name = "lblIndispensableMeetingTimeFrom1"
        Me.lblIndispensableMeetingTimeFrom1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIndispensableMeetingTimeFrom1.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableMeetingTimeFrom1.TabIndex = 23
        Me.lblIndispensableMeetingTimeFrom1.Text = "*"
        '
        'lblHatsu3
        '
        Me.lblHatsu3.AutoSize = True
        Me.lblHatsu3.Location = New System.Drawing.Point(836, 229)
        Me.lblHatsu3.Name = "lblHatsu3"
        Me.lblHatsu3.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHatsu3.Size = New System.Drawing.Size(24, 16)
        Me.lblHatsu3.TabIndex = 48
        Me.lblHatsu3.Text = "発"
        '
        'mtbDFlightBack2
        '
        Me.mtbDFlightBack2.BackColor = System.Drawing.Color.White
        Me.mtbDFlightBack2.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.mtbDFlightBack2.Location = New System.Drawing.Point(742, 226)
        Me.mtbDFlightBack2.Mask = "90時90分"
        Me.mtbDFlightBack2.Name = "mtbDFlightBack2"
        Me.mtbDFlightBack2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.mtbDFlightBack2.Size = New System.Drawing.Size(90, 23)
        Me.mtbDFlightBack2.TabIndex = 47
        Me.mtbDFlightBack2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mtbDFlightBack2.ValidatingType = GetType(Date)
        '
        'mtbDFlightBack1
        '
        Me.mtbDFlightBack1.BackColor = System.Drawing.Color.White
        Me.mtbDFlightBack1.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.mtbDFlightBack1.Location = New System.Drawing.Point(742, 175)
        Me.mtbDFlightBack1.Mask = "90時90分"
        Me.mtbDFlightBack1.Name = "mtbDFlightBack1"
        Me.mtbDFlightBack1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.mtbDFlightBack1.Size = New System.Drawing.Size(90, 23)
        Me.mtbDFlightBack1.TabIndex = 36
        Me.mtbDFlightBack1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mtbDFlightBack1.ValidatingType = GetType(Date)
        '
        'mtbDFlight1
        '
        Me.mtbDFlight1.BackColor = System.Drawing.Color.White
        Me.mtbDFlight1.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.mtbDFlight1.Location = New System.Drawing.Point(742, 138)
        Me.mtbDFlight1.Mask = "90時90分"
        Me.mtbDFlight1.Name = "mtbDFlight1"
        Me.mtbDFlight1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.mtbDFlight1.Size = New System.Drawing.Size(90, 23)
        Me.mtbDFlight1.TabIndex = 29
        Me.mtbDFlight1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mtbDFlight1.ValidatingType = GetType(Date)
        '
        'mtbMeetingTimeFrom2
        '
        Me.mtbMeetingTimeFrom2.BackColor = System.Drawing.Color.White
        Me.mtbMeetingTimeFrom2.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.mtbMeetingTimeFrom2.Location = New System.Drawing.Point(313, 226)
        Me.mtbMeetingTimeFrom2.Mask = "90時90分"
        Me.mtbMeetingTimeFrom2.Name = "mtbMeetingTimeFrom2"
        Me.mtbMeetingTimeFrom2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.mtbMeetingTimeFrom2.Size = New System.Drawing.Size(90, 23)
        Me.mtbMeetingTimeFrom2.TabIndex = 43
        Me.mtbMeetingTimeFrom2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mtbMeetingTimeFrom2.ValidatingType = GetType(Date)
        '
        'mtbMeetingTimeTo2
        '
        Me.mtbMeetingTimeTo2.BackColor = System.Drawing.Color.White
        Me.mtbMeetingTimeTo2.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.mtbMeetingTimeTo2.Location = New System.Drawing.Point(433, 226)
        Me.mtbMeetingTimeTo2.Mask = "90時90分"
        Me.mtbMeetingTimeTo2.Name = "mtbMeetingTimeTo2"
        Me.mtbMeetingTimeTo2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.mtbMeetingTimeTo2.Size = New System.Drawing.Size(90, 23)
        Me.mtbMeetingTimeTo2.TabIndex = 44
        Me.mtbMeetingTimeTo2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mtbMeetingTimeTo2.ValidatingType = GetType(Date)
        '
        'mtbMeetingTimeFrom1
        '
        Me.mtbMeetingTimeFrom1.BackColor = System.Drawing.Color.White
        Me.mtbMeetingTimeFrom1.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.mtbMeetingTimeFrom1.Location = New System.Drawing.Point(313, 138)
        Me.mtbMeetingTimeFrom1.Mask = "90時90分"
        Me.mtbMeetingTimeFrom1.Name = "mtbMeetingTimeFrom1"
        Me.mtbMeetingTimeFrom1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.mtbMeetingTimeFrom1.Size = New System.Drawing.Size(90, 23)
        Me.mtbMeetingTimeFrom1.TabIndex = 24
        Me.mtbMeetingTimeFrom1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.mtbMeetingTimeFrom1.ValidatingType = GetType(Date)
        '
        'mtbMeetingTimeTo1
        '
        Me.mtbMeetingTimeTo1.BackColor = System.Drawing.Color.White
        Me.mtbMeetingTimeTo1.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.mtbMeetingTimeTo1.Location = New System.Drawing.Point(434, 138)
        Me.mtbMeetingTimeTo1.Mask = "90時90分"
        Me.mtbMeetingTimeTo1.Name = "mtbMeetingTimeTo1"
        Me.mtbMeetingTimeTo1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.mtbMeetingTimeTo1.Size = New System.Drawing.Size(90, 23)
        Me.mtbMeetingTimeTo1.TabIndex = 26
        Me.mtbMeetingTimeTo1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'dtpMeeting2
        '
        Me.dtpMeeting2.Location = New System.Drawing.Point(123, 226)
        Me.dtpMeeting2.Name = "dtpMeeting2"
        Me.dtpMeeting2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.dtpMeeting2.Size = New System.Drawing.Size(136, 23)
        Me.dtpMeeting2.TabIndex = 42
        '
        'dtpMeeting1
        '
        Me.dtpMeeting1.Location = New System.Drawing.Point(123, 138)
        Me.dtpMeeting1.Name = "dtpMeeting1"
        Me.dtpMeeting1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.dtpMeeting1.Size = New System.Drawing.Size(136, 23)
        Me.dtpMeeting1.TabIndex = 22
        '
        'lblCreateDateLabel
        '
        Me.lblCreateDateLabel.AutoSize = True
        Me.lblCreateDateLabel.Location = New System.Drawing.Point(723, 19)
        Me.lblCreateDateLabel.Name = "lblCreateDateLabel"
        Me.lblCreateDateLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblCreateDateLabel.Size = New System.Drawing.Size(64, 16)
        Me.lblCreateDateLabel.TabIndex = 3
        Me.lblCreateDateLabel.Text = "登録日："
        '
        'grpInfomationName
        '
        Me.grpInfomationName.Controls.Add(Me.optInfomationName3)
        Me.grpInfomationName.Controls.Add(Me.optInfomationName2)
        Me.grpInfomationName.Controls.Add(Me.optInfomationName1)
        Me.grpInfomationName.Location = New System.Drawing.Point(531, 44)
        Me.grpInfomationName.Name = "grpInfomationName"
        Me.grpInfomationName.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.grpInfomationName.Size = New System.Drawing.Size(240, 50)
        Me.grpInfomationName.TabIndex = 12
        Me.grpInfomationName.TabStop = False
        Me.grpInfomationName.Text = "種類"
        '
        'optInfomationName3
        '
        Me.optInfomationName3.AutoSize = True
        Me.optInfomationName3.Location = New System.Drawing.Point(151, 18)
        Me.optInfomationName3.Name = "optInfomationName3"
        Me.optInfomationName3.Size = New System.Drawing.Size(58, 20)
        Me.optInfomationName3.TabIndex = 15
        Me.optInfomationName3.Text = "中止"
        Me.optInfomationName3.UseVisualStyleBackColor = True
        '
        'optInfomationName2
        '
        Me.optInfomationName2.AutoSize = True
        Me.optInfomationName2.Location = New System.Drawing.Point(87, 17)
        Me.optInfomationName2.Name = "optInfomationName2"
        Me.optInfomationName2.Size = New System.Drawing.Size(58, 20)
        Me.optInfomationName2.TabIndex = 14
        Me.optInfomationName2.Text = "変更"
        Me.optInfomationName2.UseVisualStyleBackColor = True
        '
        'optInfomationName1
        '
        Me.optInfomationName1.AutoSize = True
        Me.optInfomationName1.Checked = True
        Me.optInfomationName1.Location = New System.Drawing.Point(27, 18)
        Me.optInfomationName1.Name = "optInfomationName1"
        Me.optInfomationName1.Size = New System.Drawing.Size(58, 20)
        Me.optInfomationName1.TabIndex = 13
        Me.optInfomationName1.TabStop = True
        Me.optInfomationName1.Text = "開催"
        Me.optInfomationName1.UseVisualStyleBackColor = True
        '
        'grpUnionType
        '
        Me.grpUnionType.Controls.Add(Me.optUnionType3)
        Me.grpUnionType.Controls.Add(Me.optUnionType2)
        Me.grpUnionType.Controls.Add(Me.optUnionType1)
        Me.grpUnionType.Location = New System.Drawing.Point(262, 44)
        Me.grpUnionType.Name = "grpUnionType"
        Me.grpUnionType.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.grpUnionType.Size = New System.Drawing.Size(250, 50)
        Me.grpUnionType.TabIndex = 8
        Me.grpUnionType.TabStop = False
        Me.grpUnionType.Text = "種別"
        '
        'optUnionType3
        '
        Me.optUnionType3.AutoSize = True
        Me.optUnionType3.Location = New System.Drawing.Point(162, 18)
        Me.optUnionType3.Name = "optUnionType3"
        Me.optUnionType3.Size = New System.Drawing.Size(58, 20)
        Me.optUnionType3.TabIndex = 11
        Me.optUnionType3.Text = "任意"
        Me.optUnionType3.UseVisualStyleBackColor = True
        '
        'optUnionType2
        '
        Me.optUnionType2.AutoSize = True
        Me.optUnionType2.Location = New System.Drawing.Point(100, 18)
        Me.optUnionType2.Name = "optUnionType2"
        Me.optUnionType2.Size = New System.Drawing.Size(45, 20)
        Me.optUnionType2.TabIndex = 10
        Me.optUnionType2.Text = "TV"
        Me.optUnionType2.UseVisualStyleBackColor = True
        '
        'optUnionType1
        '
        Me.optUnionType1.AutoSize = True
        Me.optUnionType1.Checked = True
        Me.optUnionType1.Location = New System.Drawing.Point(28, 18)
        Me.optUnionType1.Name = "optUnionType1"
        Me.optUnionType1.Size = New System.Drawing.Size(58, 20)
        Me.optUnionType1.TabIndex = 9
        Me.optUnionType1.TabStop = True
        Me.optUnionType1.Text = "合同"
        Me.optUnionType1.UseVisualStyleBackColor = True
        '
        'txtBiko2
        '
        Me.txtBiko2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        Me.txtBiko2.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtBiko2.Location = New System.Drawing.Point(123, 571)
        Me.txtBiko2.Multiline = True
        Me.txtBiko2.Name = "txtBiko2"
        Me.txtBiko2.Size = New System.Drawing.Size(721, 87)
        Me.txtBiko2.TabIndex = 59
        '
        'lblBiko2
        '
        Me.lblBiko2.AutoSize = True
        Me.lblBiko2.Location = New System.Drawing.Point(28, 574)
        Me.lblBiko2.Name = "lblBiko2"
        Me.lblBiko2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBiko2.Size = New System.Drawing.Size(72, 16)
        Me.lblBiko2.TabIndex = 58
        Me.lblBiko2.Text = "議題備考"
        '
        'txtSubject1
        '
        Me.txtSubject1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        Me.txtSubject1.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtSubject1.Location = New System.Drawing.Point(123, 404)
        Me.txtSubject1.Multiline = True
        Me.txtSubject1.Name = "txtSubject1"
        Me.txtSubject1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtSubject1.Size = New System.Drawing.Size(721, 156)
        Me.txtSubject1.TabIndex = 57
        '
        'lblSubject1
        '
        Me.lblSubject1.AutoSize = True
        Me.lblSubject1.Location = New System.Drawing.Point(60, 407)
        Me.lblSubject1.Name = "lblSubject1"
        Me.lblSubject1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblSubject1.Size = New System.Drawing.Size(40, 16)
        Me.lblSubject1.TabIndex = 56
        Me.lblSubject1.Text = "議題"
        '
        'txtBiko1
        '
        Me.txtBiko1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!)
        Me.txtBiko1.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtBiko1.Location = New System.Drawing.Point(123, 328)
        Me.txtBiko1.Multiline = True
        Me.txtBiko1.Name = "txtBiko1"
        Me.txtBiko1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtBiko1.Size = New System.Drawing.Size(721, 67)
        Me.txtBiko1.TabIndex = 55
        '
        'lblBiko1
        '
        Me.lblBiko1.AutoSize = True
        Me.lblBiko1.Location = New System.Drawing.Point(60, 331)
        Me.lblBiko1.Name = "lblBiko1"
        Me.lblBiko1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBiko1.Size = New System.Drawing.Size(40, 16)
        Me.lblBiko1.TabIndex = 54
        Me.lblBiko1.Text = "備考"
        '
        'chkExchangeMeeting2
        '
        Me.chkExchangeMeeting2.AutoSize = True
        Me.chkExchangeMeeting2.Location = New System.Drawing.Point(432, 286)
        Me.chkExchangeMeeting2.Name = "chkExchangeMeeting2"
        Me.chkExchangeMeeting2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExchangeMeeting2.Size = New System.Drawing.Size(139, 20)
        Me.chkExchangeMeeting2.TabIndex = 53
        Me.chkExchangeMeeting2.Text = "夕食交流会可否"
        Me.chkExchangeMeeting2.UseVisualStyleBackColor = True
        '
        'chkLunch2
        '
        Me.chkLunch2.AutoSize = True
        Me.chkLunch2.BackColor = System.Drawing.SystemColors.Control
        Me.chkLunch2.Location = New System.Drawing.Point(432, 263)
        Me.chkLunch2.Name = "chkLunch2"
        Me.chkLunch2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLunch2.Size = New System.Drawing.Size(91, 20)
        Me.chkLunch2.TabIndex = 51
        Me.chkLunch2.Text = "昼食可否"
        Me.chkLunch2.UseVisualStyleBackColor = False
        '
        'txtPlace2
        '
        Me.txtPlace2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtPlace2.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtPlace2.Location = New System.Drawing.Point(123, 263)
        Me.txtPlace2.Name = "txtPlace2"
        Me.txtPlace2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPlace2.Size = New System.Drawing.Size(280, 23)
        Me.txtPlace2.TabIndex = 50
        '
        'lblPlace2
        '
        Me.lblPlace2.AutoSize = True
        Me.lblPlace2.Location = New System.Drawing.Point(44, 266)
        Me.lblPlace2.Name = "lblPlace2"
        Me.lblPlace2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPlace2.Size = New System.Drawing.Size(56, 16)
        Me.lblPlace2.TabIndex = 49
        Me.lblPlace2.Text = "会議場"
        '
        'lblLFlightBack2
        '
        Me.lblLFlightBack2.AutoSize = True
        Me.lblLFlightBack2.Location = New System.Drawing.Point(674, 252)
        Me.lblLFlightBack2.Name = "lblLFlightBack2"
        Me.lblLFlightBack2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLFlightBack2.Size = New System.Drawing.Size(62, 16)
        Me.lblLFlightBack2.TabIndex = 52
        Me.lblLFlightBack2.Text = "(NH123)"
        '
        'lblLFlightBack1
        '
        Me.lblLFlightBack1.AutoSize = True
        Me.lblLFlightBack1.Location = New System.Drawing.Point(674, 201)
        Me.lblLFlightBack1.Name = "lblLFlightBack1"
        Me.lblLFlightBack1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblLFlightBack1.Size = New System.Drawing.Size(62, 16)
        Me.lblLFlightBack1.TabIndex = 39
        Me.lblLFlightBack1.Text = "(NH123)"
        '
        'lblFlightBack2
        '
        Me.lblFlightBack2.AutoSize = True
        Me.lblFlightBack2.Location = New System.Drawing.Point(578, 229)
        Me.lblFlightBack2.Name = "lblFlightBack2"
        Me.lblFlightBack2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFlightBack2.Size = New System.Drawing.Size(88, 16)
        Me.lblFlightBack2.TabIndex = 45
        Me.lblFlightBack2.Text = "移動（復路）"
        '
        'txtLFlightBack2
        '
        Me.txtLFlightBack2.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtLFlightBack2.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtLFlightBack2.Location = New System.Drawing.Point(672, 226)
        Me.txtLFlightBack2.MaxLength = 10
        Me.txtLFlightBack2.Name = "txtLFlightBack2"
        Me.txtLFlightBack2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLFlightBack2.Size = New System.Drawing.Size(64, 23)
        Me.txtLFlightBack2.TabIndex = 46
        '
        'lblMeeting2
        '
        Me.lblMeeting2.AutoSize = True
        Me.lblMeeting2.Location = New System.Drawing.Point(30, 229)
        Me.lblMeeting2.Name = "lblMeeting2"
        Me.lblMeeting2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMeeting2.Size = New System.Drawing.Size(72, 16)
        Me.lblMeeting2.TabIndex = 40
        Me.lblMeeting2.Text = "終了日時"
        '
        'lblKara2
        '
        Me.lblKara2.AutoSize = True
        Me.lblKara2.Location = New System.Drawing.Point(408, 229)
        Me.lblKara2.Name = "lblKara2"
        Me.lblKara2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblKara2.Size = New System.Drawing.Size(24, 16)
        Me.lblKara2.TabIndex = 40
        Me.lblKara2.Text = "～"
        '
        'chkMeeting2
        '
        Me.chkMeeting2.AutoSize = True
        Me.chkMeeting2.Location = New System.Drawing.Point(102, 231)
        Me.chkMeeting2.Name = "chkMeeting2"
        Me.chkMeeting2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkMeeting2.Size = New System.Drawing.Size(15, 14)
        Me.chkMeeting2.TabIndex = 41
        Me.chkMeeting2.UseVisualStyleBackColor = True
        '
        'chkExchangeMeeting1
        '
        Me.chkExchangeMeeting1.AutoSize = True
        Me.chkExchangeMeeting1.Location = New System.Drawing.Point(432, 198)
        Me.chkExchangeMeeting1.Name = "chkExchangeMeeting1"
        Me.chkExchangeMeeting1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkExchangeMeeting1.Size = New System.Drawing.Size(139, 20)
        Me.chkExchangeMeeting1.TabIndex = 38
        Me.chkExchangeMeeting1.Text = "夕食交流会可否"
        Me.chkExchangeMeeting1.UseVisualStyleBackColor = True
        '
        'chkLunch1
        '
        Me.chkLunch1.AutoSize = True
        Me.chkLunch1.Location = New System.Drawing.Point(432, 175)
        Me.chkLunch1.Name = "chkLunch1"
        Me.chkLunch1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.chkLunch1.Size = New System.Drawing.Size(91, 20)
        Me.chkLunch1.TabIndex = 33
        Me.chkLunch1.Text = "昼食可否"
        Me.chkLunch1.UseVisualStyleBackColor = True
        '
        'txtPlace1
        '
        Me.txtPlace1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtPlace1.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.txtPlace1.Location = New System.Drawing.Point(123, 175)
        Me.txtPlace1.MaxLength = 22
        Me.txtPlace1.Name = "txtPlace1"
        Me.txtPlace1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtPlace1.Size = New System.Drawing.Size(280, 23)
        Me.txtPlace1.TabIndex = 32
        '
        'lblPlace1
        '
        Me.lblPlace1.AutoSize = True
        Me.lblPlace1.Location = New System.Drawing.Point(46, 178)
        Me.lblPlace1.Name = "lblPlace1"
        Me.lblPlace1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPlace1.Size = New System.Drawing.Size(56, 16)
        Me.lblPlace1.TabIndex = 31
        Me.lblPlace1.Text = "会議場"
        '
        'lblHatsu2
        '
        Me.lblHatsu2.AutoSize = True
        Me.lblHatsu2.Location = New System.Drawing.Point(836, 178)
        Me.lblHatsu2.Name = "lblHatsu2"
        Me.lblHatsu2.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHatsu2.Size = New System.Drawing.Size(24, 16)
        Me.lblHatsu2.TabIndex = 37
        Me.lblHatsu2.Text = "発"
        '
        'txtLFlightBack1
        '
        Me.txtLFlightBack1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtLFlightBack1.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtLFlightBack1.Location = New System.Drawing.Point(672, 175)
        Me.txtLFlightBack1.MaxLength = 10
        Me.txtLFlightBack1.Name = "txtLFlightBack1"
        Me.txtLFlightBack1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLFlightBack1.Size = New System.Drawing.Size(64, 23)
        Me.txtLFlightBack1.TabIndex = 35
        '
        'lblFlightBack1
        '
        Me.lblFlightBack1.AutoSize = True
        Me.lblFlightBack1.Location = New System.Drawing.Point(610, 177)
        Me.lblFlightBack1.Name = "lblFlightBack1"
        Me.lblFlightBack1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFlightBack1.Size = New System.Drawing.Size(56, 16)
        Me.lblFlightBack1.TabIndex = 34
        Me.lblFlightBack1.Text = "（復路）"
        '
        'lblFlight1
        '
        Me.lblFlight1.AutoSize = True
        Me.lblFlight1.Location = New System.Drawing.Point(578, 140)
        Me.lblFlight1.Name = "lblFlight1"
        Me.lblFlight1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblFlight1.Size = New System.Drawing.Size(88, 16)
        Me.lblFlight1.TabIndex = 27
        Me.lblFlight1.Text = "移動（往路）"
        '
        'lblHatsu1
        '
        Me.lblHatsu1.AutoSize = True
        Me.lblHatsu1.Location = New System.Drawing.Point(836, 141)
        Me.lblHatsu1.Name = "lblHatsu1"
        Me.lblHatsu1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblHatsu1.Size = New System.Drawing.Size(24, 16)
        Me.lblHatsu1.TabIndex = 30
        Me.lblHatsu1.Text = "発"
        '
        'txtLFlight1
        '
        Me.txtLFlight1.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtLFlight1.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtLFlight1.Location = New System.Drawing.Point(672, 138)
        Me.txtLFlight1.MaxLength = 10
        Me.txtLFlight1.Name = "txtLFlight1"
        Me.txtLFlight1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtLFlight1.Size = New System.Drawing.Size(64, 23)
        Me.txtLFlight1.TabIndex = 28
        '
        'lblKara1
        '
        Me.lblKara1.AutoSize = True
        Me.lblKara1.Location = New System.Drawing.Point(408, 142)
        Me.lblKara1.Name = "lblKara1"
        Me.lblKara1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblKara1.Size = New System.Drawing.Size(24, 16)
        Me.lblKara1.TabIndex = 25
        Me.lblKara1.Text = "～"
        '
        'lblIndispensableMeeting1
        '
        Me.lblIndispensableMeeting1.AutoSize = True
        Me.lblIndispensableMeeting1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableMeeting1.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableMeeting1.Location = New System.Drawing.Point(8, 141)
        Me.lblIndispensableMeeting1.Name = "lblIndispensableMeeting1"
        Me.lblIndispensableMeeting1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIndispensableMeeting1.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableMeeting1.TabIndex = 20
        Me.lblIndispensableMeeting1.Text = "*"
        '
        'lblOpenBelongingMemo
        '
        Me.lblOpenBelongingMemo.AutoSize = True
        Me.lblOpenBelongingMemo.Location = New System.Drawing.Point(240, 103)
        Me.lblOpenBelongingMemo.Name = "lblOpenBelongingMemo"
        Me.lblOpenBelongingMemo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOpenBelongingMemo.Size = New System.Drawing.Size(98, 16)
        Me.lblOpenBelongingMemo.TabIndex = 19
        Me.lblOpenBelongingMemo.Text = "[直接入力可]"
        '
        'lblMeeting1
        '
        Me.lblMeeting1.AutoSize = True
        Me.lblMeeting1.Location = New System.Drawing.Point(30, 141)
        Me.lblMeeting1.Name = "lblMeeting1"
        Me.lblMeeting1.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblMeeting1.Size = New System.Drawing.Size(72, 16)
        Me.lblMeeting1.TabIndex = 21
        Me.lblMeeting1.Text = "開催日時"
        '
        'lblIndispensableOpenBelonging
        '
        Me.lblIndispensableOpenBelonging.AutoSize = True
        Me.lblIndispensableOpenBelonging.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableOpenBelonging.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableOpenBelonging.Location = New System.Drawing.Point(8, 100)
        Me.lblIndispensableOpenBelonging.Name = "lblIndispensableOpenBelonging"
        Me.lblIndispensableOpenBelonging.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIndispensableOpenBelonging.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableOpenBelonging.TabIndex = 16
        Me.lblIndispensableOpenBelonging.Text = "*"
        '
        'cboOpenBelonging
        '
        Me.cboOpenBelonging.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboOpenBelonging.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboOpenBelonging.FormattingEnabled = True
        Me.cboOpenBelonging.Location = New System.Drawing.Point(123, 100)
        Me.cboOpenBelonging.Name = "cboOpenBelonging"
        Me.cboOpenBelonging.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboOpenBelonging.Size = New System.Drawing.Size(111, 24)
        Me.cboOpenBelonging.TabIndex = 18
        '
        'lblOpenBelonging
        '
        Me.lblOpenBelonging.AutoSize = True
        Me.lblOpenBelonging.Location = New System.Drawing.Point(30, 103)
        Me.lblOpenBelonging.Name = "lblOpenBelonging"
        Me.lblOpenBelonging.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblOpenBelonging.Size = New System.Drawing.Size(72, 16)
        Me.lblOpenBelonging.TabIndex = 17
        Me.lblOpenBelonging.Text = "開催場所"
        '
        'lblIndispensableApplyArea
        '
        Me.lblIndispensableApplyArea.AutoSize = True
        Me.lblIndispensableApplyArea.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableApplyArea.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableApplyArea.Location = New System.Drawing.Point(40, 66)
        Me.lblIndispensableApplyArea.Name = "lblIndispensableApplyArea"
        Me.lblIndispensableApplyArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblIndispensableApplyArea.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableApplyArea.TabIndex = 5
        Me.lblIndispensableApplyArea.Text = "*"
        '
        'cboApplyArea
        '
        Me.cboApplyArea.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboApplyArea.FormattingEnabled = True
        Me.cboApplyArea.Location = New System.Drawing.Point(123, 62)
        Me.cboApplyArea.Name = "cboApplyArea"
        Me.cboApplyArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.cboApplyArea.Size = New System.Drawing.Size(111, 24)
        Me.cboApplyArea.TabIndex = 7
        '
        'lblUnionMeetingNoLabel
        '
        Me.lblUnionMeetingNoLabel.AutoSize = True
        Me.lblUnionMeetingNoLabel.Location = New System.Drawing.Point(8, 19)
        Me.lblUnionMeetingNoLabel.Name = "lblUnionMeetingNoLabel"
        Me.lblUnionMeetingNoLabel.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblUnionMeetingNoLabel.Size = New System.Drawing.Size(104, 16)
        Me.lblUnionMeetingNoLabel.TabIndex = 2
        Me.lblUnionMeetingNoLabel.Text = "会議通知番号"
        '
        'lblApplyArea
        '
        Me.lblApplyArea.AutoSize = True
        Me.lblApplyArea.Location = New System.Drawing.Point(62, 65)
        Me.lblApplyArea.Name = "lblApplyArea"
        Me.lblApplyArea.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblApplyArea.Size = New System.Drawing.Size(40, 16)
        Me.lblApplyArea.TabIndex = 6
        Me.lblApplyArea.Text = "支部"
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(79, 772)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 60
        Me.btnPrint.Text = "プレ印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnInsertChk
        '
        Me.btnInsertChk.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsertChk.Location = New System.Drawing.Point(720, 772)
        Me.btnInsertChk.Name = "btnInsertChk"
        Me.btnInsertChk.Size = New System.Drawing.Size(110, 30)
        Me.btnInsertChk.TabIndex = 64
        Me.btnInsertChk.Text = "登録確認"
        Me.btnInsertChk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(836, 772)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 65
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBack.Location = New System.Drawing.Point(836, 772)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(110, 30)
        Me.btnBack.TabIndex = 66
        Me.btnBack.Text = "戻る"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'UC040102
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnInsertChk)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.grpMain)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnCancel)
        Me.Name = "UC040102"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpMain.ResumeLayout(False)
        Me.grpMain.PerformLayout()
        Me.grpInfomationName.ResumeLayout(False)
        Me.grpInfomationName.PerformLayout()
        Me.grpUnionType.ResumeLayout(False)
        Me.grpUnionType.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents grpMain As System.Windows.Forms.GroupBox
    Friend WithEvents lblIndispensableApplyArea As System.Windows.Forms.Label
    Friend WithEvents cboApplyArea As System.Windows.Forms.ComboBox
    Friend WithEvents lblUnionMeetingNoLabel As System.Windows.Forms.Label
    Friend WithEvents lblApplyArea As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableOpenBelonging As System.Windows.Forms.Label
    Friend WithEvents cboOpenBelonging As System.Windows.Forms.ComboBox
    Friend WithEvents lblOpenBelonging As System.Windows.Forms.Label
    Friend WithEvents lblOpenBelongingMemo As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableMeeting1 As System.Windows.Forms.Label
    Friend WithEvents lblMeeting1 As System.Windows.Forms.Label
    Friend WithEvents lblHatsu1 As System.Windows.Forms.Label
    Friend WithEvents txtLFlight1 As System.Windows.Forms.TextBox
    Friend WithEvents lblKara1 As System.Windows.Forms.Label
    Friend WithEvents lblHatsu2 As System.Windows.Forms.Label
    Friend WithEvents txtLFlightBack1 As System.Windows.Forms.TextBox
    Friend WithEvents lblFlightBack1 As System.Windows.Forms.Label
    Friend WithEvents lblFlight1 As System.Windows.Forms.Label
    Friend WithEvents txtPlace1 As System.Windows.Forms.TextBox
    Friend WithEvents lblPlace1 As System.Windows.Forms.Label
    Friend WithEvents chkExchangeMeeting1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLunch1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkExchangeMeeting2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkLunch2 As System.Windows.Forms.CheckBox
    Friend WithEvents txtPlace2 As System.Windows.Forms.TextBox
    Friend WithEvents lblPlace2 As System.Windows.Forms.Label
    Friend WithEvents lblLFlightBack2 As System.Windows.Forms.Label
    Friend WithEvents lblLFlightBack1 As System.Windows.Forms.Label
    Friend WithEvents lblFlightBack2 As System.Windows.Forms.Label
    Friend WithEvents txtLFlightBack2 As System.Windows.Forms.TextBox
    Friend WithEvents lblMeeting2 As System.Windows.Forms.Label
    Friend WithEvents lblKara2 As System.Windows.Forms.Label
    Friend WithEvents chkMeeting2 As System.Windows.Forms.CheckBox
    Friend WithEvents txtBiko2 As System.Windows.Forms.TextBox
    Friend WithEvents lblBiko2 As System.Windows.Forms.Label
    Friend WithEvents txtSubject1 As System.Windows.Forms.TextBox
    Friend WithEvents lblSubject1 As System.Windows.Forms.Label
    Friend WithEvents txtBiko1 As System.Windows.Forms.TextBox
    Friend WithEvents lblBiko1 As System.Windows.Forms.Label
    Friend WithEvents optUnionType3 As System.Windows.Forms.RadioButton
    Friend WithEvents grpInfomationName As System.Windows.Forms.GroupBox
    Friend WithEvents grpUnionType As System.Windows.Forms.GroupBox
    Friend WithEvents optUnionType2 As System.Windows.Forms.RadioButton
    Friend WithEvents optUnionType1 As System.Windows.Forms.RadioButton
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents optInfomationName3 As System.Windows.Forms.RadioButton
    Friend WithEvents optInfomationName2 As System.Windows.Forms.RadioButton
    Friend WithEvents optInfomationName1 As System.Windows.Forms.RadioButton
    Friend WithEvents lblCreateDateLabel As System.Windows.Forms.Label
    Friend WithEvents btnInsertChk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents dtpMeeting2 As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpMeeting1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents mtbMeetingTimeTo1 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mtbDFlightBack2 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mtbDFlightBack1 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mtbDFlight1 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mtbMeetingTimeFrom2 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mtbMeetingTimeTo2 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents mtbMeetingTimeFrom1 As System.Windows.Forms.MaskedTextBox
    Friend WithEvents lblHatsu3 As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableMeetingTimeFrom1 As System.Windows.Forms.Label
    Friend WithEvents lblCreateDate As System.Windows.Forms.Label
    Friend WithEvents lblUnionMeetingNo As System.Windows.Forms.Label
    Friend WithEvents btnBack As System.Windows.Forms.Button

End Class
