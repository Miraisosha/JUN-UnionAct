<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040202
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
        Me.fraDetail = New System.Windows.Forms.GroupBox
        Me.lblIchiji = New System.Windows.Forms.Label
        Me.txtUser = New System.Windows.Forms.TextBox
        Me.txtSougiDate = New System.Windows.Forms.TextBox
        Me.dtpSougiDate = New System.Windows.Forms.DateTimePicker
        Me.txtDateTime = New System.Windows.Forms.TextBox
        Me.txtJiken = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.fraList = New System.Windows.Forms.GroupBox
        Me.dgvSougiList = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnSampleText = New System.Windows.Forms.Button
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtShinseiDate = New System.Windows.Forms.TextBox
        Me.lblShinseiDate = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblStrikeKind = New System.Windows.Forms.Label
        Me.txtNo = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtNoKind = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnSaveTmp = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.fraDetail.SuspendLayout()
        Me.fraList.SuspendLayout()
        CType(Me.dgvSougiList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(187, 14)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTitle.Size = New System.Drawing.Size(630, 35)
        Me.lblTitle.TabIndex = 10
        Me.lblTitle.Text = "争議行為通告 - 詳細"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fraDetail
        '
        Me.fraDetail.BackColor = System.Drawing.SystemColors.Control
        Me.fraDetail.Controls.Add(Me.lblIchiji)
        Me.fraDetail.Controls.Add(Me.txtUser)
        Me.fraDetail.Controls.Add(Me.txtSougiDate)
        Me.fraDetail.Controls.Add(Me.dtpSougiDate)
        Me.fraDetail.Controls.Add(Me.txtDateTime)
        Me.fraDetail.Controls.Add(Me.txtJiken)
        Me.fraDetail.Controls.Add(Me.Label10)
        Me.fraDetail.Controls.Add(Me.Label12)
        Me.fraDetail.Controls.Add(Me.Label9)
        Me.fraDetail.Controls.Add(Me.Label8)
        Me.fraDetail.Controls.Add(Me.fraList)
        Me.fraDetail.Controls.Add(Me.btnSampleText)
        Me.fraDetail.Controls.Add(Me.Label7)
        Me.fraDetail.Controls.Add(Me.Label6)
        Me.fraDetail.Controls.Add(Me.txtShinseiDate)
        Me.fraDetail.Controls.Add(Me.lblShinseiDate)
        Me.fraDetail.Controls.Add(Me.Label4)
        Me.fraDetail.Controls.Add(Me.lblStrikeKind)
        Me.fraDetail.Controls.Add(Me.txtNo)
        Me.fraDetail.Controls.Add(Me.Label2)
        Me.fraDetail.Controls.Add(Me.txtNoKind)
        Me.fraDetail.Controls.Add(Me.Label1)
        Me.fraDetail.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.fraDetail.Location = New System.Drawing.Point(25, 60)
        Me.fraDetail.Name = "fraDetail"
        Me.fraDetail.Size = New System.Drawing.Size(975, 677)
        Me.fraDetail.TabIndex = 11
        Me.fraDetail.TabStop = False
        '
        'lblIchiji
        '
        Me.lblIchiji.AutoSize = True
        Me.lblIchiji.ForeColor = System.Drawing.Color.Red
        Me.lblIchiji.Location = New System.Drawing.Point(11, 647)
        Me.lblIchiji.Name = "lblIchiji"
        Me.lblIchiji.Size = New System.Drawing.Size(120, 16)
        Me.lblIchiji.TabIndex = 54
        Me.lblIchiji.Text = "※一時保存文書"
        '
        'txtUser
        '
        Me.txtUser.BackColor = System.Drawing.Color.White
        Me.txtUser.Location = New System.Drawing.Point(673, 65)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(189, 23)
        Me.txtUser.TabIndex = 16
        '
        'txtSougiDate
        '
        Me.txtSougiDate.BackColor = System.Drawing.Color.White
        Me.txtSougiDate.Location = New System.Drawing.Point(169, 109)
        Me.txtSougiDate.Name = "txtSougiDate"
        Me.txtSougiDate.Size = New System.Drawing.Size(135, 23)
        Me.txtSougiDate.TabIndex = 13
        '
        'dtpSougiDate
        '
        Me.dtpSougiDate.Location = New System.Drawing.Point(169, 109)
        Me.dtpSougiDate.Name = "dtpSougiDate"
        Me.dtpSougiDate.Size = New System.Drawing.Size(135, 23)
        Me.dtpSougiDate.TabIndex = 14
        Me.dtpSougiDate.Value = New Date(2012, 1, 25, 0, 0, 0, 0)
        '
        'txtDateTime
        '
        Me.txtDateTime.BackColor = System.Drawing.Color.White
        Me.txtDateTime.Location = New System.Drawing.Point(162, 465)
        Me.txtDateTime.Multiline = True
        Me.txtDateTime.Name = "txtDateTime"
        Me.txtDateTime.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtDateTime.Size = New System.Drawing.Size(647, 183)
        Me.txtDateTime.TabIndex = 20
        '
        'txtJiken
        '
        Me.txtJiken.BackColor = System.Drawing.Color.White
        Me.txtJiken.Location = New System.Drawing.Point(162, 260)
        Me.txtJiken.Multiline = True
        Me.txtJiken.Name = "txtJiken"
        Me.txtJiken.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtJiken.Size = New System.Drawing.Size(647, 175)
        Me.txtJiken.TabIndex = 19
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(28, 465)
        Me.Label10.Name = "Label10"
        Me.Label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label10.Size = New System.Drawing.Size(110, 16)
        Me.Label10.TabIndex = 48
        Me.Label10.Text = "日時および時間"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Red
        Me.Label12.Location = New System.Drawing.Point(11, 465)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(16, 16)
        Me.Label12.TabIndex = 49
        Me.Label12.Text = "*"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(28, 260)
        Me.Label9.Name = "Label9"
        Me.Label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label9.Size = New System.Drawing.Size(40, 16)
        Me.Label9.TabIndex = 38
        Me.Label9.Text = "事件"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label8.ForeColor = System.Drawing.Color.Red
        Me.Label8.Location = New System.Drawing.Point(11, 260)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(16, 16)
        Me.Label8.TabIndex = 47
        Me.Label8.Text = "*"
        '
        'fraList
        '
        Me.fraList.Controls.Add(Me.dgvSougiList)
        Me.fraList.Location = New System.Drawing.Point(452, 102)
        Me.fraList.Name = "fraList"
        Me.fraList.Size = New System.Drawing.Size(496, 134)
        Me.fraList.TabIndex = 46
        Me.fraList.TabStop = False
        Me.fraList.Text = "関連争議行為一覧"
        '
        'dgvSougiList
        '
        Me.dgvSougiList.AllowUserToAddRows = False
        Me.dgvSougiList.AllowUserToDeleteRows = False
        Me.dgvSougiList.AllowUserToResizeRows = False
        Me.dgvSougiList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSougiList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3})
        Me.dgvSougiList.Location = New System.Drawing.Point(6, 22)
        Me.dgvSougiList.Name = "dgvSougiList"
        Me.dgvSougiList.ReadOnly = True
        Me.dgvSougiList.RowHeadersVisible = False
        Me.dgvSougiList.RowTemplate.Height = 21
        Me.dgvSougiList.Size = New System.Drawing.Size(484, 106)
        Me.dgvSougiList.TabIndex = 18
        '
        'Column1
        '
        Me.Column1.HeaderText = "分類"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.Width = 150
        '
        'Column2
        '
        Me.Column2.HeaderText = "通告番号"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 150
        '
        'Column3
        '
        Me.Column3.HeaderText = "申請日付"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 150
        '
        'btnSampleText
        '
        Me.btnSampleText.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSampleText.Location = New System.Drawing.Point(316, 105)
        Me.btnSampleText.Name = "btnSampleText"
        Me.btnSampleText.Size = New System.Drawing.Size(110, 30)
        Me.btnSampleText.TabIndex = 17
        Me.btnSampleText.Text = "例文出力"
        Me.btnSampleText.UseVisualStyleBackColor = True
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(28, 114)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label7.Size = New System.Drawing.Size(72, 16)
        Me.Label7.TabIndex = 37
        Me.Label7.Text = "争議日付"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(592, 68)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 16)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "*"
        '
        'txtShinseiDate
        '
        Me.txtShinseiDate.BackColor = System.Drawing.SystemColors.Info
        Me.txtShinseiDate.Location = New System.Drawing.Point(735, 23)
        Me.txtShinseiDate.Name = "txtShinseiDate"
        Me.txtShinseiDate.Size = New System.Drawing.Size(127, 23)
        Me.txtShinseiDate.TabIndex = 15
        '
        'lblShinseiDate
        '
        Me.lblShinseiDate.AutoSize = True
        Me.lblShinseiDate.Location = New System.Drawing.Point(673, 25)
        Me.lblShinseiDate.Name = "lblShinseiDate"
        Me.lblShinseiDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblShinseiDate.Size = New System.Drawing.Size(56, 16)
        Me.lblShinseiDate.TabIndex = 41
        Me.lblShinseiDate.Text = "申請日"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(609, 68)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label4.Size = New System.Drawing.Size(56, 16)
        Me.Label4.TabIndex = 40
        Me.Label4.Text = "申請者"
        '
        'lblStrikeKind
        '
        Me.lblStrikeKind.AutoSize = True
        Me.lblStrikeKind.Location = New System.Drawing.Point(455, 25)
        Me.lblStrikeKind.Name = "lblStrikeKind"
        Me.lblStrikeKind.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblStrikeKind.Size = New System.Drawing.Size(120, 16)
        Me.lblStrikeKind.TabIndex = 37
        Me.lblStrikeKind.Text = "【争議行為通告】"
        '
        'txtNo
        '
        Me.txtNo.BackColor = System.Drawing.SystemColors.Info
        Me.txtNo.Location = New System.Drawing.Point(169, 65)
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(104, 23)
        Me.txtNo.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(28, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label2.Size = New System.Drawing.Size(136, 16)
        Me.Label2.TabIndex = 38
        Me.Label2.Text = "争議行為通告番号"
        '
        'txtNoKind
        '
        Me.txtNoKind.BackColor = System.Drawing.SystemColors.Info
        Me.txtNoKind.Location = New System.Drawing.Point(169, 23)
        Me.txtNoKind.Name = "txtNoKind"
        Me.txtNoKind.Size = New System.Drawing.Size(148, 23)
        Me.txtNoKind.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(28, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label1.Size = New System.Drawing.Size(104, 16)
        Me.Label1.TabIndex = 36
        Me.Label1.Text = "通告番号種別"
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(25, 753)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 21
        Me.btnPrint.Text = "プレ印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBack.Location = New System.Drawing.Point(890, 753)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(110, 30)
        Me.btnBack.TabIndex = 25
        Me.btnBack.Text = "戻る"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnChange
        '
        Me.btnChange.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnChange.Location = New System.Drawing.Point(751, 753)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(110, 30)
        Me.btnChange.TabIndex = 24
        Me.btnChange.Text = "登録確認"
        Me.btnChange.UseVisualStyleBackColor = True
        '
        'btnSaveTmp
        '
        Me.btnSaveTmp.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSaveTmp.Location = New System.Drawing.Point(602, 753)
        Me.btnSaveTmp.Name = "btnSaveTmp"
        Me.btnSaveTmp.Size = New System.Drawing.Size(110, 30)
        Me.btnSaveTmp.TabIndex = 22
        Me.btnSaveTmp.Text = "一時保存確認"
        Me.btnSaveTmp.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(890, 753)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 26
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.Location = New System.Drawing.Point(751, 753)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(110, 30)
        Me.btnUpdate.TabIndex = 23
        Me.btnUpdate.Text = "内容変更"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'UC040202
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSaveTmp)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.fraDetail)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC040202"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.fraDetail.ResumeLayout(False)
        Me.fraDetail.PerformLayout()
        Me.fraList.ResumeLayout(False)
        CType(Me.dgvSougiList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents fraDetail As System.Windows.Forms.GroupBox
    Friend WithEvents txtNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNoKind As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtShinseiDate As System.Windows.Forms.TextBox
    Friend WithEvents lblShinseiDate As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblStrikeKind As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents fraList As System.Windows.Forms.GroupBox
    Friend WithEvents dgvSougiList As System.Windows.Forms.DataGridView
    Friend WithEvents btnSampleText As System.Windows.Forms.Button
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents txtDateTime As System.Windows.Forms.TextBox
    Friend WithEvents txtJiken As System.Windows.Forms.TextBox
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents dtpSougiDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnSaveTmp As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents txtSougiDate As System.Windows.Forms.TextBox
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents lblIchiji As System.Windows.Forms.Label
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
