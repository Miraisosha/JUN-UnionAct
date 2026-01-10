<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC080202
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
        Me.txtStafKind = New System.Windows.Forms.TextBox
        Me.txtStatus = New System.Windows.Forms.TextBox
        Me.txtStafName = New System.Windows.Forms.TextBox
        Me.txtNameKana = New System.Windows.Forms.TextBox
        Me.txtOldStafIdDezit = New System.Windows.Forms.TextBox
        Me.txtStafIdDezit = New System.Windows.Forms.TextBox
        Me.txtOldStafId = New System.Windows.Forms.TextBox
        Me.txtStafId = New System.Windows.Forms.TextBox
        Me.lblOldStafIdDezit = New System.Windows.Forms.Label
        Me.lblStafName = New System.Windows.Forms.Label
        Me.lblStafKind = New System.Windows.Forms.Label
        Me.lblStatus = New System.Windows.Forms.Label
        Me.lblNameKana = New System.Windows.Forms.Label
        Me.lblStafId = New System.Windows.Forms.Label
        Me.grpUseDate = New System.Windows.Forms.GroupBox
        Me.btnHistory = New System.Windows.Forms.Button
        Me.lblUseDate = New System.Windows.Forms.Label
        Me.lblIndispensableUseDateddd = New System.Windows.Forms.Label
        Me.txtUseDate = New System.Windows.Forms.TextBox
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.grpRokinCir = New System.Windows.Forms.GroupBox
        Me.txtRokinCir = New System.Windows.Forms.TextBox
        Me.lblRokinCir = New System.Windows.Forms.Label
        Me.grpAccount = New System.Windows.Forms.GroupBox
        Me.lblBankOfficeNameKana = New System.Windows.Forms.Label
        Me.lblBankNameKana = New System.Windows.Forms.Label
        Me.txtAccountNameKana = New System.Windows.Forms.TextBox
        Me.txtAccountName = New System.Windows.Forms.TextBox
        Me.txtBankAccount = New System.Windows.Forms.TextBox
        Me.lblBankOffice = New System.Windows.Forms.Label
        Me.lblBank = New System.Windows.Forms.Label
        Me.lblAccountNameKana = New System.Windows.Forms.Label
        Me.cboDepositItems = New System.Windows.Forms.ComboBox
        Me.lblAccountName = New System.Windows.Forms.Label
        Me.lblBankAccount = New System.Windows.Forms.Label
        Me.lblDepositItems = New System.Windows.Forms.Label
        Me.cboBankOfficeName = New System.Windows.Forms.ComboBox
        Me.lblBankOfficeName = New System.Windows.Forms.Label
        Me.cboBankName = New System.Windows.Forms.ComboBox
        Me.lblLabelBankOffice = New System.Windows.Forms.Label
        Me.lblLabelBank = New System.Windows.Forms.Label
        Me.lblBankName = New System.Windows.Forms.Label
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnInsertChk = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grpUseDate.SuspendLayout()
        Me.grpResult.SuspendLayout()
        Me.grpRokinCir.SuspendLayout()
        Me.grpAccount.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(200, 20)
        Me.lblTitle.MinimumSize = New System.Drawing.Size(630, 35)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Padding = New System.Windows.Forms.Padding(14, 0, 14, 0)
        Me.lblTitle.Size = New System.Drawing.Size(630, 35)
        Me.lblTitle.TabIndex = 46
        Me.lblTitle.Text = "組合員口座情報 - 詳細"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtStafKind
        '
        Me.txtStafKind.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtStafKind.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStafKind.Location = New System.Drawing.Point(761, 180)
        Me.txtStafKind.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtStafKind.Name = "txtStafKind"
        Me.txtStafKind.Size = New System.Drawing.Size(136, 23)
        Me.txtStafKind.TabIndex = 7
        Me.txtStafKind.Text = "永年組合員"
        '
        'txtStatus
        '
        Me.txtStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtStatus.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStatus.Location = New System.Drawing.Point(634, 180)
        Me.txtStatus.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(112, 23)
        Me.txtStatus.TabIndex = 6
        Me.txtStatus.Text = "加入"
        '
        'txtStafName
        '
        Me.txtStafName.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtStafName.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStafName.Location = New System.Drawing.Point(441, 180)
        Me.txtStafName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtStafName.Name = "txtStafName"
        Me.txtStafName.Size = New System.Drawing.Size(173, 23)
        Me.txtStafName.TabIndex = 5
        Me.txtStafName.Text = "ＷＷＷＷＷＷＷＷＷＷ"
        '
        'txtNameKana
        '
        Me.txtNameKana.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtNameKana.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNameKana.Location = New System.Drawing.Point(441, 151)
        Me.txtNameKana.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtNameKana.Name = "txtNameKana"
        Me.txtNameKana.Size = New System.Drawing.Size(173, 23)
        Me.txtNameKana.TabIndex = 2
        Me.txtNameKana.Text = "XXXXXXXXXXXXXXXXXXXX"
        '
        'txtOldStafIdDezit
        '
        Me.txtOldStafIdDezit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtOldStafIdDezit.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtOldStafIdDezit.Location = New System.Drawing.Point(357, 180)
        Me.txtOldStafIdDezit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtOldStafIdDezit.Name = "txtOldStafIdDezit"
        Me.txtOldStafIdDezit.Size = New System.Drawing.Size(18, 23)
        Me.txtOldStafIdDezit.TabIndex = 4
        Me.txtOldStafIdDezit.Text = "X"
        '
        'txtStafIdDezit
        '
        Me.txtStafIdDezit.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtStafIdDezit.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStafIdDezit.Location = New System.Drawing.Point(357, 150)
        Me.txtStafIdDezit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtStafIdDezit.Name = "txtStafIdDezit"
        Me.txtStafIdDezit.Size = New System.Drawing.Size(18, 23)
        Me.txtStafIdDezit.TabIndex = 1
        Me.txtStafIdDezit.Text = "X"
        '
        'txtOldStafId
        '
        Me.txtOldStafId.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtOldStafId.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtOldStafId.Location = New System.Drawing.Point(244, 180)
        Me.txtOldStafId.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtOldStafId.Name = "txtOldStafId"
        Me.txtOldStafId.Size = New System.Drawing.Size(114, 23)
        Me.txtOldStafId.TabIndex = 3
        Me.txtOldStafId.Text = "999999"
        Me.txtOldStafId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'txtStafId
        '
        Me.txtStafId.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtStafId.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStafId.Location = New System.Drawing.Point(244, 150)
        Me.txtStafId.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtStafId.Name = "txtStafId"
        Me.txtStafId.Size = New System.Drawing.Size(114, 23)
        Me.txtStafId.TabIndex = 0
        Me.txtStafId.Text = "999999"
        Me.txtStafId.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblOldStafIdDezit
        '
        Me.lblOldStafIdDezit.AutoSize = True
        Me.lblOldStafIdDezit.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblOldStafIdDezit.Location = New System.Drawing.Point(133, 184)
        Me.lblOldStafIdDezit.Name = "lblOldStafIdDezit"
        Me.lblOldStafIdDezit.Size = New System.Drawing.Size(104, 16)
        Me.lblOldStafIdDezit.TabIndex = 23
        Me.lblOldStafIdDezit.Text = "（旧社員番号）"
        '
        'lblStafName
        '
        Me.lblStafName.AutoSize = True
        Me.lblStafName.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStafName.Location = New System.Drawing.Point(387, 184)
        Me.lblStafName.Name = "lblStafName"
        Me.lblStafName.Size = New System.Drawing.Size(40, 16)
        Me.lblStafName.TabIndex = 24
        Me.lblStafName.Text = "名前"
        '
        'lblStafKind
        '
        Me.lblStafKind.AutoSize = True
        Me.lblStafKind.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStafKind.Location = New System.Drawing.Point(758, 152)
        Me.lblStafKind.Name = "lblStafKind"
        Me.lblStafKind.Size = New System.Drawing.Size(88, 16)
        Me.lblStafKind.TabIndex = 26
        Me.lblStafKind.Text = "組合員種別"
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStatus.Location = New System.Drawing.Point(631, 153)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(68, 16)
        Me.lblStatus.TabIndex = 25
        Me.lblStatus.Text = "ステータス"
        '
        'lblNameKana
        '
        Me.lblNameKana.AutoSize = True
        Me.lblNameKana.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNameKana.Location = New System.Drawing.Point(385, 154)
        Me.lblNameKana.Name = "lblNameKana"
        Me.lblNameKana.Size = New System.Drawing.Size(48, 16)
        Me.lblNameKana.TabIndex = 22
        Me.lblNameKana.Text = "ﾌﾘｶﾞﾅ"
        '
        'lblStafId
        '
        Me.lblStafId.AutoSize = True
        Me.lblStafId.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblStafId.Location = New System.Drawing.Point(165, 153)
        Me.lblStafId.Name = "lblStafId"
        Me.lblStafId.Size = New System.Drawing.Size(72, 16)
        Me.lblStafId.TabIndex = 21
        Me.lblStafId.Text = "社員番号"
        '
        'grpUseDate
        '
        Me.grpUseDate.Controls.Add(Me.btnHistory)
        Me.grpUseDate.Controls.Add(Me.lblUseDate)
        Me.grpUseDate.Controls.Add(Me.lblIndispensableUseDateddd)
        Me.grpUseDate.Controls.Add(Me.txtUseDate)
        Me.grpUseDate.Location = New System.Drawing.Point(157, 237)
        Me.grpUseDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpUseDate.Name = "grpUseDate"
        Me.grpUseDate.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpUseDate.Size = New System.Drawing.Size(380, 58)
        Me.grpUseDate.TabIndex = 27
        Me.grpUseDate.TabStop = False
        '
        'btnHistory
        '
        Me.btnHistory.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnHistory.Location = New System.Drawing.Point(306, 18)
        Me.btnHistory.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnHistory.Name = "btnHistory"
        Me.btnHistory.Size = New System.Drawing.Size(50, 30)
        Me.btnHistory.TabIndex = 9
        Me.btnHistory.Text = "履歴"
        Me.btnHistory.UseVisualStyleBackColor = True
        '
        'lblUseDate
        '
        Me.lblUseDate.AutoSize = True
        Me.lblUseDate.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblUseDate.Location = New System.Drawing.Point(65, 25)
        Me.lblUseDate.Name = "lblUseDate"
        Me.lblUseDate.Size = New System.Drawing.Size(72, 16)
        Me.lblUseDate.TabIndex = 29
        Me.lblUseDate.Text = "適用日付"
        '
        'lblIndispensableUseDateddd
        '
        Me.lblIndispensableUseDateddd.AutoSize = True
        Me.lblIndispensableUseDateddd.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableUseDateddd.Location = New System.Drawing.Point(43, 25)
        Me.lblIndispensableUseDateddd.Name = "lblIndispensableUseDateddd"
        Me.lblIndispensableUseDateddd.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableUseDateddd.TabIndex = 28
        Me.lblIndispensableUseDateddd.Text = "*"
        '
        'txtUseDate
        '
        Me.txtUseDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.txtUseDate.Location = New System.Drawing.Point(146, 22)
        Me.txtUseDate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtUseDate.Name = "txtUseDate"
        Me.txtUseDate.Size = New System.Drawing.Size(146, 23)
        Me.txtUseDate.TabIndex = 8
        Me.txtUseDate.Text = "2011年09月01日"
        Me.txtUseDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.grpRokinCir)
        Me.grpResult.Controls.Add(Me.grpAccount)
        Me.grpResult.Location = New System.Drawing.Point(157, 313)
        Me.grpResult.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpResult.Size = New System.Drawing.Size(765, 401)
        Me.grpResult.TabIndex = 30
        Me.grpResult.TabStop = False
        '
        'grpRokinCir
        '
        Me.grpRokinCir.Controls.Add(Me.txtRokinCir)
        Me.grpRokinCir.Controls.Add(Me.lblRokinCir)
        Me.grpRokinCir.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpRokinCir.Location = New System.Drawing.Point(7, 323)
        Me.grpRokinCir.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpRokinCir.Name = "grpRokinCir"
        Me.grpRokinCir.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpRokinCir.Size = New System.Drawing.Size(750, 68)
        Me.grpRokinCir.TabIndex = 44
        Me.grpRokinCir.TabStop = False
        Me.grpRokinCir.Text = "労金CIR"
        '
        'txtRokinCir
        '
        Me.txtRokinCir.BackColor = System.Drawing.Color.White
        Me.txtRokinCir.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtRokinCir.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtRokinCir.Location = New System.Drawing.Point(178, 25)
        Me.txtRokinCir.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtRokinCir.MaxLength = 20
        Me.txtRokinCir.Name = "txtRokinCir"
        Me.txtRokinCir.Size = New System.Drawing.Size(114, 23)
        Me.txtRokinCir.TabIndex = 16
        Me.txtRokinCir.Text = "99999999"
        '
        'lblRokinCir
        '
        Me.lblRokinCir.AutoSize = True
        Me.lblRokinCir.Location = New System.Drawing.Point(107, 28)
        Me.lblRokinCir.Name = "lblRokinCir"
        Me.lblRokinCir.Size = New System.Drawing.Size(65, 16)
        Me.lblRokinCir.TabIndex = 45
        Me.lblRokinCir.Text = "労金CIR"
        '
        'grpAccount
        '
        Me.grpAccount.Controls.Add(Me.lblBankOfficeNameKana)
        Me.grpAccount.Controls.Add(Me.lblBankNameKana)
        Me.grpAccount.Controls.Add(Me.txtAccountNameKana)
        Me.grpAccount.Controls.Add(Me.txtAccountName)
        Me.grpAccount.Controls.Add(Me.txtBankAccount)
        Me.grpAccount.Controls.Add(Me.lblBankOffice)
        Me.grpAccount.Controls.Add(Me.lblBank)
        Me.grpAccount.Controls.Add(Me.lblAccountNameKana)
        Me.grpAccount.Controls.Add(Me.cboDepositItems)
        Me.grpAccount.Controls.Add(Me.lblAccountName)
        Me.grpAccount.Controls.Add(Me.lblBankAccount)
        Me.grpAccount.Controls.Add(Me.lblDepositItems)
        Me.grpAccount.Controls.Add(Me.cboBankOfficeName)
        Me.grpAccount.Controls.Add(Me.lblBankOfficeName)
        Me.grpAccount.Controls.Add(Me.cboBankName)
        Me.grpAccount.Controls.Add(Me.lblLabelBankOffice)
        Me.grpAccount.Controls.Add(Me.lblLabelBank)
        Me.grpAccount.Controls.Add(Me.lblBankName)
        Me.grpAccount.Location = New System.Drawing.Point(7, 16)
        Me.grpAccount.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpAccount.Name = "grpAccount"
        Me.grpAccount.Padding = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.grpAccount.Size = New System.Drawing.Size(750, 300)
        Me.grpAccount.TabIndex = 31
        Me.grpAccount.TabStop = False
        Me.grpAccount.Text = "口座情報"
        '
        'lblBankOfficeNameKana
        '
        Me.lblBankOfficeNameKana.AutoSize = True
        Me.lblBankOfficeNameKana.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblBankOfficeNameKana.Location = New System.Drawing.Point(374, 77)
        Me.lblBankOfficeNameKana.Name = "lblBankOfficeNameKana"
        Me.lblBankOfficeNameKana.Size = New System.Drawing.Size(85, 16)
        Me.lblBankOfficeNameKana.TabIndex = 37
        Me.lblBankOfficeNameKana.Text = "( ﾖｳｺｳﾀﾞｲ )"
        '
        'lblBankNameKana
        '
        Me.lblBankNameKana.AutoSize = True
        Me.lblBankNameKana.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblBankNameKana.Location = New System.Drawing.Point(374, 27)
        Me.lblBankNameKana.Name = "lblBankNameKana"
        Me.lblBankNameKana.Size = New System.Drawing.Size(64, 16)
        Me.lblBankNameKana.TabIndex = 33
        Me.lblBankNameKana.Text = "( ﾖｺﾊﾏ )"
        '
        'txtAccountNameKana
        '
        Me.txtAccountNameKana.BackColor = System.Drawing.Color.White
        Me.txtAccountNameKana.ImeMode = System.Windows.Forms.ImeMode.KatakanaHalf
        Me.txtAccountNameKana.Location = New System.Drawing.Point(178, 255)
        Me.txtAccountNameKana.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAccountNameKana.MaxLength = 40
        Me.txtAccountNameKana.Name = "txtAccountNameKana"
        Me.txtAccountNameKana.Size = New System.Drawing.Size(342, 23)
        Me.txtAccountNameKana.TabIndex = 15
        Me.txtAccountNameKana.Text = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX"
        '
        'txtAccountName
        '
        Me.txtAccountName.BackColor = System.Drawing.Color.White
        Me.txtAccountName.ImeMode = System.Windows.Forms.ImeMode.NoControl
        Me.txtAccountName.Location = New System.Drawing.Point(178, 210)
        Me.txtAccountName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtAccountName.MaxLength = 40
        Me.txtAccountName.Name = "txtAccountName"
        Me.txtAccountName.ReadOnly = True
        Me.txtAccountName.Size = New System.Drawing.Size(342, 23)
        Me.txtAccountName.TabIndex = 14
        Me.txtAccountName.Text = "ＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷＷ"
        '
        'txtBankAccount
        '
        Me.txtBankAccount.BackColor = System.Drawing.Color.White
        Me.txtBankAccount.ImeMode = System.Windows.Forms.ImeMode.Alpha
        Me.txtBankAccount.Location = New System.Drawing.Point(178, 166)
        Me.txtBankAccount.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.txtBankAccount.MaxLength = 10
        Me.txtBankAccount.Name = "txtBankAccount"
        Me.txtBankAccount.Size = New System.Drawing.Size(114, 23)
        Me.txtBankAccount.TabIndex = 13
        Me.txtBankAccount.Text = "99999999"
        '
        'lblBankOffice
        '
        Me.lblBankOffice.AutoSize = True
        Me.lblBankOffice.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblBankOffice.Location = New System.Drawing.Point(654, 77)
        Me.lblBankOffice.Name = "lblBankOffice"
        Me.lblBankOffice.Size = New System.Drawing.Size(32, 16)
        Me.lblBankOffice.TabIndex = 39
        Me.lblBankOffice.Text = "343"
        '
        'lblBank
        '
        Me.lblBank.AutoSize = True
        Me.lblBank.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblBank.Location = New System.Drawing.Point(654, 31)
        Me.lblBank.Name = "lblBank"
        Me.lblBank.Size = New System.Drawing.Size(40, 16)
        Me.lblBank.TabIndex = 35
        Me.lblBank.Text = "0138"
        '
        'lblAccountNameKana
        '
        Me.lblAccountNameKana.AutoSize = True
        Me.lblAccountNameKana.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblAccountNameKana.Location = New System.Drawing.Point(59, 259)
        Me.lblAccountNameKana.Name = "lblAccountNameKana"
        Me.lblAccountNameKana.Size = New System.Drawing.Size(112, 16)
        Me.lblAccountNameKana.TabIndex = 43
        Me.lblAccountNameKana.Text = "口座名義（カナ）"
        '
        'cboDepositItems
        '
        Me.cboDepositItems.FormattingEnabled = True
        Me.cboDepositItems.Location = New System.Drawing.Point(178, 123)
        Me.cboDepositItems.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboDepositItems.Name = "cboDepositItems"
        Me.cboDepositItems.Size = New System.Drawing.Size(190, 24)
        Me.cboDepositItems.TabIndex = 12
        Me.cboDepositItems.Text = "普通預金"
        '
        'lblAccountName
        '
        Me.lblAccountName.AutoSize = True
        Me.lblAccountName.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblAccountName.Location = New System.Drawing.Point(99, 214)
        Me.lblAccountName.Name = "lblAccountName"
        Me.lblAccountName.Size = New System.Drawing.Size(72, 16)
        Me.lblAccountName.TabIndex = 42
        Me.lblAccountName.Text = "口座名義"
        '
        'lblBankAccount
        '
        Me.lblBankAccount.AutoSize = True
        Me.lblBankAccount.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblBankAccount.Location = New System.Drawing.Point(99, 170)
        Me.lblBankAccount.Name = "lblBankAccount"
        Me.lblBankAccount.Size = New System.Drawing.Size(72, 16)
        Me.lblBankAccount.TabIndex = 41
        Me.lblBankAccount.Text = "口座番号"
        '
        'lblDepositItems
        '
        Me.lblDepositItems.AutoSize = True
        Me.lblDepositItems.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblDepositItems.Location = New System.Drawing.Point(99, 126)
        Me.lblDepositItems.Name = "lblDepositItems"
        Me.lblDepositItems.Size = New System.Drawing.Size(72, 16)
        Me.lblDepositItems.TabIndex = 40
        Me.lblDepositItems.Text = "預金種目"
        '
        'cboBankOfficeName
        '
        Me.cboBankOfficeName.FormattingEnabled = True
        Me.cboBankOfficeName.Location = New System.Drawing.Point(178, 73)
        Me.cboBankOfficeName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboBankOfficeName.Name = "cboBankOfficeName"
        Me.cboBankOfficeName.Size = New System.Drawing.Size(190, 24)
        Me.cboBankOfficeName.TabIndex = 11
        Me.cboBankOfficeName.Text = "洋光台"
        '
        'lblBankOfficeName
        '
        Me.lblBankOfficeName.AutoSize = True
        Me.lblBankOfficeName.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblBankOfficeName.Location = New System.Drawing.Point(115, 77)
        Me.lblBankOfficeName.Name = "lblBankOfficeName"
        Me.lblBankOfficeName.Size = New System.Drawing.Size(56, 16)
        Me.lblBankOfficeName.TabIndex = 36
        Me.lblBankOfficeName.Text = "支店名"
        '
        'cboBankName
        '
        Me.cboBankName.FormattingEnabled = True
        Me.cboBankName.Location = New System.Drawing.Point(178, 24)
        Me.cboBankName.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.cboBankName.Name = "cboBankName"
        Me.cboBankName.Size = New System.Drawing.Size(190, 24)
        Me.cboBankName.TabIndex = 10
        Me.cboBankName.Text = "横浜"
        '
        'lblLabelBankOffice
        '
        Me.lblLabelBankOffice.AutoSize = True
        Me.lblLabelBankOffice.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblLabelBankOffice.Location = New System.Drawing.Point(567, 77)
        Me.lblLabelBankOffice.Name = "lblLabelBankOffice"
        Me.lblLabelBankOffice.Size = New System.Drawing.Size(80, 16)
        Me.lblLabelBankOffice.TabIndex = 38
        Me.lblLabelBankOffice.Text = "支店番号："
        '
        'lblLabelBank
        '
        Me.lblLabelBank.AutoSize = True
        Me.lblLabelBank.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblLabelBank.Location = New System.Drawing.Point(531, 31)
        Me.lblLabelBank.Name = "lblLabelBank"
        Me.lblLabelBank.Size = New System.Drawing.Size(116, 16)
        Me.lblLabelBank.TabIndex = 34
        Me.lblLabelBank.Text = "金融機関コード："
        '
        'lblBankName
        '
        Me.lblBankName.AutoSize = True
        Me.lblBankName.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblBankName.Location = New System.Drawing.Point(115, 31)
        Me.lblBankName.Name = "lblBankName"
        Me.lblBankName.Size = New System.Drawing.Size(56, 16)
        Me.lblBankName.TabIndex = 32
        Me.lblBankName.Text = "銀行名"
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.Location = New System.Drawing.Point(696, 722)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(110, 30)
        Me.btnUpdate.TabIndex = 17
        Me.btnUpdate.Text = "内容変更"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBack.Location = New System.Drawing.Point(812, 722)
        Me.btnBack.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(110, 30)
        Me.btnBack.TabIndex = 18
        Me.btnBack.Text = "戻る"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnInsertChk
        '
        Me.btnInsertChk.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsertChk.Location = New System.Drawing.Point(696, 722)
        Me.btnInsertChk.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnInsertChk.Name = "btnInsertChk"
        Me.btnInsertChk.Size = New System.Drawing.Size(110, 30)
        Me.btnInsertChk.TabIndex = 19
        Me.btnInsertChk.Text = "登録確認"
        Me.btnInsertChk.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(812, 722)
        Me.btnCancel.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 20
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'UC080202
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.grpUseDate)
        Me.Controls.Add(Me.txtStafKind)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.txtStafName)
        Me.Controls.Add(Me.txtNameKana)
        Me.Controls.Add(Me.txtOldStafIdDezit)
        Me.Controls.Add(Me.txtStafIdDezit)
        Me.Controls.Add(Me.txtOldStafId)
        Me.Controls.Add(Me.txtStafId)
        Me.Controls.Add(Me.lblOldStafIdDezit)
        Me.Controls.Add(Me.lblStafName)
        Me.Controls.Add(Me.lblStafKind)
        Me.Controls.Add(Me.lblStatus)
        Me.Controls.Add(Me.lblNameKana)
        Me.Controls.Add(Me.lblStafId)
        Me.Controls.Add(Me.lblTitle)
        Me.Controls.Add(Me.btnInsertChk)
        Me.Controls.Add(Me.btnCancel)
        Me.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "UC080202"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpUseDate.ResumeLayout(False)
        Me.grpUseDate.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        Me.grpRokinCir.ResumeLayout(False)
        Me.grpRokinCir.PerformLayout()
        Me.grpAccount.ResumeLayout(False)
        Me.grpAccount.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents txtStafKind As System.Windows.Forms.TextBox
    Friend WithEvents txtStatus As System.Windows.Forms.TextBox
    Friend WithEvents txtStafName As System.Windows.Forms.TextBox
    Friend WithEvents txtNameKana As System.Windows.Forms.TextBox
    Friend WithEvents txtOldStafIdDezit As System.Windows.Forms.TextBox
    Friend WithEvents txtStafIdDezit As System.Windows.Forms.TextBox
    Friend WithEvents txtOldStafId As System.Windows.Forms.TextBox
    Friend WithEvents txtStafId As System.Windows.Forms.TextBox
    Friend WithEvents lblOldStafIdDezit As System.Windows.Forms.Label
    Friend WithEvents lblStafName As System.Windows.Forms.Label
    Friend WithEvents lblStafKind As System.Windows.Forms.Label
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents lblNameKana As System.Windows.Forms.Label
    Friend WithEvents lblStafId As System.Windows.Forms.Label
    Friend WithEvents grpUseDate As System.Windows.Forms.GroupBox
    Friend WithEvents btnHistory As System.Windows.Forms.Button
    Friend WithEvents lblUseDate As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableUseDateddd As System.Windows.Forms.Label
    Friend WithEvents txtUseDate As System.Windows.Forms.TextBox
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents grpRokinCir As System.Windows.Forms.GroupBox
    Friend WithEvents grpAccount As System.Windows.Forms.GroupBox
    Friend WithEvents txtRokinCir As System.Windows.Forms.TextBox
    Friend WithEvents lblRokinCir As System.Windows.Forms.Label
    Friend WithEvents txtAccountNameKana As System.Windows.Forms.TextBox
    Friend WithEvents txtAccountName As System.Windows.Forms.TextBox
    Friend WithEvents txtBankAccount As System.Windows.Forms.TextBox
    Friend WithEvents lblBankOffice As System.Windows.Forms.Label
    Friend WithEvents lblBank As System.Windows.Forms.Label
    Friend WithEvents lblAccountNameKana As System.Windows.Forms.Label
    Friend WithEvents cboDepositItems As System.Windows.Forms.ComboBox
    Friend WithEvents lblAccountName As System.Windows.Forms.Label
    Friend WithEvents lblBankAccount As System.Windows.Forms.Label
    Friend WithEvents lblDepositItems As System.Windows.Forms.Label
    Friend WithEvents cboBankOfficeName As System.Windows.Forms.ComboBox
    Friend WithEvents lblBankOfficeName As System.Windows.Forms.Label
    Friend WithEvents cboBankName As System.Windows.Forms.ComboBox
    Friend WithEvents lblLabelBankOffice As System.Windows.Forms.Label
    Friend WithEvents lblLabelBank As System.Windows.Forms.Label
    Friend WithEvents lblBankName As System.Windows.Forms.Label
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnInsertChk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblBankOfficeNameKana As System.Windows.Forms.Label
    Friend WithEvents lblBankNameKana As System.Windows.Forms.Label

End Class
