<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM020602
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
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
        Me.grbSearchResult = New System.Windows.Forms.GroupBox
        Me.dgdSearchResult = New System.Windows.Forms.DataGridView
        Me.btnAllCheck = New System.Windows.Forms.Button
        Me.btnAllNoCheck = New System.Windows.Forms.Button
        Me.btnPrintAddress = New System.Windows.Forms.Button
        Me.btnTack = New System.Windows.Forms.Button
        Me.lblTack = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.cboTack = New System.Windows.Forms.ComboBox
        Me.grbSelect = New System.Windows.Forms.GroupBox
        Me.pnlSelect = New System.Windows.Forms.Panel
        Me.chkMail2 = New System.Windows.Forms.CheckBox
        Me.chkMail1 = New System.Windows.Forms.CheckBox
        Me.chkTell2 = New System.Windows.Forms.CheckBox
        Me.chkTell1 = New System.Windows.Forms.CheckBox
        Me.chkAddress = New System.Windows.Forms.CheckBox
        Me.chkAddNumber = New System.Windows.Forms.CheckBox
        Me.chkWorkForm = New System.Windows.Forms.CheckBox
        Me.chkRetire = New System.Windows.Forms.CheckBox
        Me.chkEntry = New System.Windows.Forms.CheckBox
        Me.chkBirth = New System.Windows.Forms.CheckBox
        Me.chkJoin = New System.Windows.Forms.CheckBox
        Me.chkCap = New System.Windows.Forms.CheckBox
        Me.chkAttachCompany = New System.Windows.Forms.CheckBox
        Me.chkOffice = New System.Windows.Forms.CheckBox
        Me.chkCompany = New System.Windows.Forms.CheckBox
        Me.chkModel = New System.Windows.Forms.CheckBox
        Me.chkCrewLicence = New System.Windows.Forms.CheckBox
        Me.chkBranch = New System.Windows.Forms.CheckBox
        Me.chkStatus = New System.Windows.Forms.CheckBox
        Me.chkMemberKind = New System.Windows.Forms.CheckBox
        Me.chkEmpNameKna = New System.Windows.Forms.CheckBox
        Me.chkEmpName = New System.Windows.Forms.CheckBox
        Me.chkEmpNo = New System.Windows.Forms.CheckBox
        Me.lblSelect10 = New System.Windows.Forms.Label
        Me.lblSelect7 = New System.Windows.Forms.Label
        Me.lblPrint1 = New System.Windows.Forms.Label
        Me.btnFileOutPut = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.grbSearchResult.SuspendLayout()
        CType(Me.dgdSearchResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grbSelect.SuspendLayout()
        Me.pnlSelect.SuspendLayout()
        Me.SuspendLayout()
        '
        'grbSearchResult
        '
        Me.grbSearchResult.Controls.Add(Me.dgdSearchResult)
        Me.grbSearchResult.Controls.Add(Me.btnAllCheck)
        Me.grbSearchResult.Controls.Add(Me.btnAllNoCheck)
        Me.grbSearchResult.Controls.Add(Me.btnPrintAddress)
        Me.grbSearchResult.Controls.Add(Me.btnTack)
        Me.grbSearchResult.Controls.Add(Me.lblTack)
        Me.grbSearchResult.Controls.Add(Me.btnPrint)
        Me.grbSearchResult.Controls.Add(Me.cboTack)
        Me.grbSearchResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grbSearchResult.Location = New System.Drawing.Point(12, 12)
        Me.grbSearchResult.Name = "grbSearchResult"
        Me.grbSearchResult.Size = New System.Drawing.Size(884, 682)
        Me.grbSearchResult.TabIndex = 0
        Me.grbSearchResult.TabStop = False
        Me.grbSearchResult.Text = "検索結果一覧（XXX件）"
        '
        'dgdSearchResult
        '
        Me.dgdSearchResult.AllowUserToResizeRows = False
        Me.dgdSearchResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgdSearchResult.Location = New System.Drawing.Point(6, 29)
        Me.dgdSearchResult.Name = "dgdSearchResult"
        Me.dgdSearchResult.RowHeadersVisible = False
        Me.dgdSearchResult.RowTemplate.Height = 21
        Me.dgdSearchResult.Size = New System.Drawing.Size(859, 504)
        Me.dgdSearchResult.TabIndex = 0
        '
        'btnAllCheck
        '
        Me.btnAllCheck.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAllCheck.Location = New System.Drawing.Point(19, 576)
        Me.btnAllCheck.Name = "btnAllCheck"
        Me.btnAllCheck.Size = New System.Drawing.Size(30, 30)
        Me.btnAllCheck.TabIndex = 1
        Me.btnAllCheck.Text = "☑"
        Me.btnAllCheck.UseVisualStyleBackColor = True
        '
        'btnAllNoCheck
        '
        Me.btnAllNoCheck.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAllNoCheck.Location = New System.Drawing.Point(19, 612)
        Me.btnAllNoCheck.Name = "btnAllNoCheck"
        Me.btnAllNoCheck.Size = New System.Drawing.Size(30, 30)
        Me.btnAllNoCheck.TabIndex = 2
        Me.btnAllNoCheck.Text = "□"
        Me.btnAllNoCheck.UseVisualStyleBackColor = True
        '
        'btnPrintAddress
        '
        Me.btnPrintAddress.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrintAddress.Location = New System.Drawing.Point(673, 630)
        Me.btnPrintAddress.Name = "btnPrintAddress"
        Me.btnPrintAddress.Size = New System.Drawing.Size(120, 30)
        Me.btnPrintAddress.TabIndex = 7
        Me.btnPrintAddress.Text = "住所録プレ印刷"
        Me.btnPrintAddress.UseVisualStyleBackColor = True
        '
        'btnTack
        '
        Me.btnTack.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnTack.Location = New System.Drawing.Point(673, 594)
        Me.btnTack.Name = "btnTack"
        Me.btnTack.Size = New System.Drawing.Size(120, 30)
        Me.btnTack.TabIndex = 6
        Me.btnTack.Text = "タックシール"
        Me.btnTack.UseVisualStyleBackColor = True
        '
        'lblTack
        '
        Me.lblTack.AutoSize = True
        Me.lblTack.Location = New System.Drawing.Point(496, 573)
        Me.lblTack.Name = "lblTack"
        Me.lblTack.Size = New System.Drawing.Size(142, 16)
        Me.lblTack.TabIndex = 3
        Me.lblTack.Text = "タックシール給紙方法"
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(673, 558)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(120, 30)
        Me.btnPrint.TabIndex = 5
        Me.btnPrint.Text = "プレ印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'cboTack
        '
        Me.cboTack.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboTack.FormattingEnabled = True
        Me.cboTack.Location = New System.Drawing.Point(482, 598)
        Me.cboTack.Name = "cboTack"
        Me.cboTack.Size = New System.Drawing.Size(173, 24)
        Me.cboTack.TabIndex = 4
        '
        'grbSelect
        '
        Me.grbSelect.Controls.Add(Me.pnlSelect)
        Me.grbSelect.Controls.Add(Me.lblSelect10)
        Me.grbSelect.Controls.Add(Me.lblSelect7)
        Me.grbSelect.Controls.Add(Me.lblPrint1)
        Me.grbSelect.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grbSelect.Location = New System.Drawing.Point(921, 12)
        Me.grbSelect.Name = "grbSelect"
        Me.grbSelect.Size = New System.Drawing.Size(288, 623)
        Me.grbSelect.TabIndex = 8
        Me.grbSelect.TabStop = False
        Me.grbSelect.Text = "印刷項目選択"
        '
        'pnlSelect
        '
        Me.pnlSelect.BackColor = System.Drawing.Color.White
        Me.pnlSelect.Controls.Add(Me.chkMail2)
        Me.pnlSelect.Controls.Add(Me.chkMail1)
        Me.pnlSelect.Controls.Add(Me.chkTell2)
        Me.pnlSelect.Controls.Add(Me.chkTell1)
        Me.pnlSelect.Controls.Add(Me.chkAddress)
        Me.pnlSelect.Controls.Add(Me.chkAddNumber)
        Me.pnlSelect.Controls.Add(Me.chkWorkForm)
        Me.pnlSelect.Controls.Add(Me.chkRetire)
        Me.pnlSelect.Controls.Add(Me.chkEntry)
        Me.pnlSelect.Controls.Add(Me.chkBirth)
        Me.pnlSelect.Controls.Add(Me.chkJoin)
        Me.pnlSelect.Controls.Add(Me.chkCap)
        Me.pnlSelect.Controls.Add(Me.chkAttachCompany)
        Me.pnlSelect.Controls.Add(Me.chkOffice)
        Me.pnlSelect.Controls.Add(Me.chkCompany)
        Me.pnlSelect.Controls.Add(Me.chkModel)
        Me.pnlSelect.Controls.Add(Me.chkCrewLicence)
        Me.pnlSelect.Controls.Add(Me.chkBranch)
        Me.pnlSelect.Controls.Add(Me.chkStatus)
        Me.pnlSelect.Controls.Add(Me.chkMemberKind)
        Me.pnlSelect.Controls.Add(Me.chkEmpNameKna)
        Me.pnlSelect.Controls.Add(Me.chkEmpName)
        Me.pnlSelect.Controls.Add(Me.chkEmpNo)
        Me.pnlSelect.Location = New System.Drawing.Point(29, 29)
        Me.pnlSelect.Name = "pnlSelect"
        Me.pnlSelect.Size = New System.Drawing.Size(229, 504)
        Me.pnlSelect.TabIndex = 24
        '
        'chkMail2
        '
        Me.chkMail2.AutoSize = True
        Me.chkMail2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMail2.Location = New System.Drawing.Point(6, 404)
        Me.chkMail2.Name = "chkMail2"
        Me.chkMail2.Size = New System.Drawing.Size(123, 20)
        Me.chkMail2.TabIndex = 45
        Me.chkMail2.Text = "メールアドレス２"
        Me.chkMail2.UseVisualStyleBackColor = True
        '
        'chkMail1
        '
        Me.chkMail1.AutoSize = True
        Me.chkMail1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMail1.Location = New System.Drawing.Point(6, 386)
        Me.chkMail1.Name = "chkMail1"
        Me.chkMail1.Size = New System.Drawing.Size(123, 20)
        Me.chkMail1.TabIndex = 44
        Me.chkMail1.Text = "メールアドレス１"
        Me.chkMail1.UseVisualStyleBackColor = True
        '
        'chkTell2
        '
        Me.chkTell2.AutoSize = True
        Me.chkTell2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTell2.Location = New System.Drawing.Point(6, 368)
        Me.chkTell2.Name = "chkTell2"
        Me.chkTell2.Size = New System.Drawing.Size(102, 20)
        Me.chkTell2.TabIndex = 43
        Me.chkTell2.Text = "電話番号２"
        Me.chkTell2.UseVisualStyleBackColor = True
        '
        'chkTell1
        '
        Me.chkTell1.AutoSize = True
        Me.chkTell1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkTell1.Location = New System.Drawing.Point(6, 350)
        Me.chkTell1.Name = "chkTell1"
        Me.chkTell1.Size = New System.Drawing.Size(102, 20)
        Me.chkTell1.TabIndex = 42
        Me.chkTell1.Text = "電話番号１"
        Me.chkTell1.UseVisualStyleBackColor = True
        '
        'chkAddress
        '
        Me.chkAddress.AutoSize = True
        Me.chkAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAddress.Location = New System.Drawing.Point(6, 331)
        Me.chkAddress.Name = "chkAddress"
        Me.chkAddress.Size = New System.Drawing.Size(59, 20)
        Me.chkAddress.TabIndex = 41
        Me.chkAddress.Text = "住所"
        Me.chkAddress.UseVisualStyleBackColor = True
        '
        'chkAddNumber
        '
        Me.chkAddNumber.AutoSize = True
        Me.chkAddNumber.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAddNumber.Location = New System.Drawing.Point(6, 312)
        Me.chkAddNumber.Name = "chkAddNumber"
        Me.chkAddNumber.Size = New System.Drawing.Size(91, 20)
        Me.chkAddNumber.TabIndex = 40
        Me.chkAddNumber.Text = "郵便番号"
        Me.chkAddNumber.UseVisualStyleBackColor = True
        '
        'chkWorkForm
        '
        Me.chkWorkForm.AutoSize = True
        Me.chkWorkForm.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkWorkForm.Location = New System.Drawing.Point(6, 293)
        Me.chkWorkForm.Name = "chkWorkForm"
        Me.chkWorkForm.Size = New System.Drawing.Size(91, 20)
        Me.chkWorkForm.TabIndex = 39
        Me.chkWorkForm.Text = "勤務状態"
        Me.chkWorkForm.UseVisualStyleBackColor = True
        '
        'chkRetire
        '
        Me.chkRetire.AutoSize = True
        Me.chkRetire.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkRetire.Location = New System.Drawing.Point(6, 275)
        Me.chkRetire.Name = "chkRetire"
        Me.chkRetire.Size = New System.Drawing.Size(107, 20)
        Me.chkRetire.TabIndex = 37
        Me.chkRetire.Text = "退職年月日"
        Me.chkRetire.UseVisualStyleBackColor = True
        '
        'chkEntry
        '
        Me.chkEntry.AutoSize = True
        Me.chkEntry.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEntry.Location = New System.Drawing.Point(6, 256)
        Me.chkEntry.Name = "chkEntry"
        Me.chkEntry.Size = New System.Drawing.Size(107, 20)
        Me.chkEntry.TabIndex = 36
        Me.chkEntry.Text = "入社年月日"
        Me.chkEntry.UseVisualStyleBackColor = True
        '
        'chkBirth
        '
        Me.chkBirth.AutoSize = True
        Me.chkBirth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBirth.Location = New System.Drawing.Point(6, 237)
        Me.chkBirth.Name = "chkBirth"
        Me.chkBirth.Size = New System.Drawing.Size(91, 20)
        Me.chkBirth.TabIndex = 35
        Me.chkBirth.Text = "生年月日"
        Me.chkBirth.UseVisualStyleBackColor = True
        '
        'chkJoin
        '
        Me.chkJoin.AutoSize = True
        Me.chkJoin.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkJoin.Location = New System.Drawing.Point(6, 218)
        Me.chkJoin.Name = "chkJoin"
        Me.chkJoin.Size = New System.Drawing.Size(107, 20)
        Me.chkJoin.TabIndex = 34
        Me.chkJoin.Text = "加入年月日"
        Me.chkJoin.UseVisualStyleBackColor = True
        '
        'chkCap
        '
        Me.chkCap.AutoSize = True
        Me.chkCap.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCap.Location = New System.Drawing.Point(6, 199)
        Me.chkCap.Name = "chkCap"
        Me.chkCap.Size = New System.Drawing.Size(107, 20)
        Me.chkCap.TabIndex = 33
        Me.chkCap.Text = "機長年月日"
        Me.chkCap.UseVisualStyleBackColor = True
        '
        'chkAttachCompany
        '
        Me.chkAttachCompany.AutoSize = True
        Me.chkAttachCompany.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkAttachCompany.Location = New System.Drawing.Point(6, 181)
        Me.chkAttachCompany.Name = "chkAttachCompany"
        Me.chkAttachCompany.Size = New System.Drawing.Size(91, 20)
        Me.chkAttachCompany.TabIndex = 32
        Me.chkAttachCompany.Text = "所属会社"
        Me.chkAttachCompany.UseVisualStyleBackColor = True
        '
        'chkOffice
        '
        Me.chkOffice.AutoSize = True
        Me.chkOffice.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkOffice.Location = New System.Drawing.Point(6, 163)
        Me.chkOffice.Name = "chkOffice"
        Me.chkOffice.Size = New System.Drawing.Size(59, 20)
        Me.chkOffice.TabIndex = 31
        Me.chkOffice.Text = "職場"
        Me.chkOffice.UseVisualStyleBackColor = True
        '
        'chkCompany
        '
        Me.chkCompany.AutoSize = True
        Me.chkCompany.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCompany.Location = New System.Drawing.Point(6, 145)
        Me.chkCompany.Name = "chkCompany"
        Me.chkCompany.Size = New System.Drawing.Size(91, 20)
        Me.chkCompany.TabIndex = 30
        Me.chkCompany.Text = "会社所属"
        Me.chkCompany.UseVisualStyleBackColor = True
        '
        'chkModel
        '
        Me.chkModel.AutoSize = True
        Me.chkModel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkModel.Location = New System.Drawing.Point(6, 128)
        Me.chkModel.Name = "chkModel"
        Me.chkModel.Size = New System.Drawing.Size(59, 20)
        Me.chkModel.TabIndex = 29
        Me.chkModel.Text = "機種"
        Me.chkModel.UseVisualStyleBackColor = True
        '
        'chkCrewLicence
        '
        Me.chkCrewLicence.AutoSize = True
        Me.chkCrewLicence.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkCrewLicence.Location = New System.Drawing.Point(6, 111)
        Me.chkCrewLicence.Name = "chkCrewLicence"
        Me.chkCrewLicence.Size = New System.Drawing.Size(59, 20)
        Me.chkCrewLicence.TabIndex = 28
        Me.chkCrewLicence.Text = "資格"
        Me.chkCrewLicence.UseVisualStyleBackColor = True
        '
        'chkBranch
        '
        Me.chkBranch.AutoSize = True
        Me.chkBranch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkBranch.Location = New System.Drawing.Point(6, 93)
        Me.chkBranch.Name = "chkBranch"
        Me.chkBranch.Size = New System.Drawing.Size(91, 20)
        Me.chkBranch.TabIndex = 27
        Me.chkBranch.Text = "組合支部"
        Me.chkBranch.UseVisualStyleBackColor = True
        '
        'chkStatus
        '
        Me.chkStatus.AutoSize = True
        Me.chkStatus.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkStatus.Location = New System.Drawing.Point(6, 76)
        Me.chkStatus.Name = "chkStatus"
        Me.chkStatus.Size = New System.Drawing.Size(87, 20)
        Me.chkStatus.TabIndex = 26
        Me.chkStatus.Text = "ステータス"
        Me.chkStatus.UseVisualStyleBackColor = True
        '
        'chkMemberKind
        '
        Me.chkMemberKind.AutoSize = True
        Me.chkMemberKind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkMemberKind.Location = New System.Drawing.Point(6, 59)
        Me.chkMemberKind.Name = "chkMemberKind"
        Me.chkMemberKind.Size = New System.Drawing.Size(107, 20)
        Me.chkMemberKind.TabIndex = 25
        Me.chkMemberKind.Text = "組合員種別"
        Me.chkMemberKind.UseVisualStyleBackColor = True
        '
        'chkEmpNameKna
        '
        Me.chkEmpNameKna.AutoSize = True
        Me.chkEmpNameKna.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEmpNameKna.Location = New System.Drawing.Point(6, 41)
        Me.chkEmpNameKna.Name = "chkEmpNameKna"
        Me.chkEmpNameKna.Size = New System.Drawing.Size(83, 20)
        Me.chkEmpNameKna.TabIndex = 24
        Me.chkEmpNameKna.Text = "名前カナ"
        Me.chkEmpNameKna.UseVisualStyleBackColor = True
        '
        'chkEmpName
        '
        Me.chkEmpName.AutoSize = True
        Me.chkEmpName.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEmpName.Location = New System.Drawing.Point(6, 23)
        Me.chkEmpName.Name = "chkEmpName"
        Me.chkEmpName.Size = New System.Drawing.Size(59, 20)
        Me.chkEmpName.TabIndex = 23
        Me.chkEmpName.Text = "名前"
        Me.chkEmpName.UseVisualStyleBackColor = True
        '
        'chkEmpNo
        '
        Me.chkEmpNo.AutoSize = True
        Me.chkEmpNo.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkEmpNo.Location = New System.Drawing.Point(6, 5)
        Me.chkEmpNo.Name = "chkEmpNo"
        Me.chkEmpNo.Size = New System.Drawing.Size(91, 20)
        Me.chkEmpNo.TabIndex = 22
        Me.chkEmpNo.Text = "社員番号"
        Me.chkEmpNo.UseVisualStyleBackColor = True
        '
        'lblSelect10
        '
        Me.lblSelect10.AutoSize = True
        Me.lblSelect10.Location = New System.Drawing.Point(52, 586)
        Me.lblSelect10.Name = "lblSelect10"
        Me.lblSelect10.Size = New System.Drawing.Size(201, 16)
        Me.lblSelect10.TabIndex = 3
        Me.lblSelect10.Text = "「住所」未選択時：10項目まで"
        '
        'lblSelect7
        '
        Me.lblSelect7.AutoSize = True
        Me.lblSelect7.Location = New System.Drawing.Point(52, 565)
        Me.lblSelect7.Name = "lblSelect7"
        Me.lblSelect7.Size = New System.Drawing.Size(199, 16)
        Me.lblSelect7.TabIndex = 2
        Me.lblSelect7.Text = "「住所」選択時　：　7項目まで"
        '
        'lblPrint1
        '
        Me.lblPrint1.AutoSize = True
        Me.lblPrint1.Location = New System.Drawing.Point(35, 547)
        Me.lblPrint1.Name = "lblPrint1"
        Me.lblPrint1.Size = New System.Drawing.Size(150, 16)
        Me.lblPrint1.TabIndex = 1
        Me.lblPrint1.Text = "※印刷可能な項目数"
        '
        'btnFileOutPut
        '
        Me.btnFileOutPut.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnFileOutPut.Location = New System.Drawing.Point(937, 664)
        Me.btnFileOutPut.Name = "btnFileOutPut"
        Me.btnFileOutPut.Size = New System.Drawing.Size(110, 30)
        Me.btnFileOutPut.TabIndex = 9
        Me.btnFileOutPut.Text = "ファイル出力"
        Me.btnFileOutPut.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(1090, 664)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'FM020602
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1221, 706)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnFileOutPut)
        Me.Controls.Add(Me.grbSelect)
        Me.Controls.Add(Me.grbSearchResult)
        Me.Name = "FM020602"
        Me.Text = "検索結果"
        Me.grbSearchResult.ResumeLayout(False)
        Me.grbSearchResult.PerformLayout()
        CType(Me.dgdSearchResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grbSelect.ResumeLayout(False)
        Me.grbSelect.PerformLayout()
        Me.pnlSelect.ResumeLayout(False)
        Me.pnlSelect.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grbSearchResult As System.Windows.Forms.GroupBox
    Friend WithEvents dgdSearchResult As System.Windows.Forms.DataGridView
    Friend WithEvents btnAllCheck As System.Windows.Forms.Button
    Friend WithEvents btnAllNoCheck As System.Windows.Forms.Button
    Friend WithEvents lblTack As System.Windows.Forms.Label
    Friend WithEvents cboTack As System.Windows.Forms.ComboBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnTack As System.Windows.Forms.Button
    Friend WithEvents btnPrintAddress As System.Windows.Forms.Button
    Friend WithEvents grbSelect As System.Windows.Forms.GroupBox
    Friend WithEvents lblSelect10 As System.Windows.Forms.Label
    Friend WithEvents lblSelect7 As System.Windows.Forms.Label
    Friend WithEvents lblPrint1 As System.Windows.Forms.Label
    Friend WithEvents btnFileOutPut As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents pnlSelect As System.Windows.Forms.Panel
    Friend WithEvents chkTell2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkTell1 As System.Windows.Forms.CheckBox
    Friend WithEvents chkAddress As System.Windows.Forms.CheckBox
    Friend WithEvents chkAddNumber As System.Windows.Forms.CheckBox
    Friend WithEvents chkWorkForm As System.Windows.Forms.CheckBox
    Friend WithEvents chkRetire As System.Windows.Forms.CheckBox
    Friend WithEvents chkEntry As System.Windows.Forms.CheckBox
    Friend WithEvents chkBirth As System.Windows.Forms.CheckBox
    Friend WithEvents chkJoin As System.Windows.Forms.CheckBox
    Friend WithEvents chkCap As System.Windows.Forms.CheckBox
    Friend WithEvents chkAttachCompany As System.Windows.Forms.CheckBox
    Friend WithEvents chkOffice As System.Windows.Forms.CheckBox
    Friend WithEvents chkCompany As System.Windows.Forms.CheckBox
    Friend WithEvents chkModel As System.Windows.Forms.CheckBox
    Friend WithEvents chkCrewLicence As System.Windows.Forms.CheckBox
    Friend WithEvents chkBranch As System.Windows.Forms.CheckBox
    Friend WithEvents chkStatus As System.Windows.Forms.CheckBox
    Friend WithEvents chkMemberKind As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmpNameKna As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmpName As System.Windows.Forms.CheckBox
    Friend WithEvents chkEmpNo As System.Windows.Forms.CheckBox
    Friend WithEvents chkMail2 As System.Windows.Forms.CheckBox
    Friend WithEvents chkMail1 As System.Windows.Forms.CheckBox
End Class
