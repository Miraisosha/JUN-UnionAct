<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040203
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
        Me.lblTitle = New System.Windows.Forms.Label
        Me.fraDetail = New System.Windows.Forms.GroupBox
        Me.lblIchiji = New System.Windows.Forms.Label
        Me.txtShinseiDate = New System.Windows.Forms.TextBox
        Me.lblShinseiDate = New System.Windows.Forms.Label
        Me.txtSougiDate = New System.Windows.Forms.TextBox
        Me.dtpSougiDate = New System.Windows.Forms.DateTimePicker
        Me.lblStrikeKind = New System.Windows.Forms.Label
        Me.fraKumiaiList = New System.Windows.Forms.GroupBox
        Me.btnPickup = New System.Windows.Forms.Button
        Me.dgvResult = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.txtKenmei = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnSampleText = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtUser = New System.Windows.Forms.TextBox
        Me.txtNoKind = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtNo = New System.Windows.Forms.TextBox
        Me.btnSaveTmp = New System.Windows.Forms.Button
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.fraDetail.SuspendLayout()
        Me.fraKumiaiList.SuspendLayout()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.lblTitle.Size = New System.Drawing.Size(620, 35)
        Me.lblTitle.TabIndex = 11
        Me.lblTitle.Text = "労働協約第47条申し入れ - 詳細"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fraDetail
        '
        Me.fraDetail.Controls.Add(Me.lblIchiji)
        Me.fraDetail.Controls.Add(Me.txtShinseiDate)
        Me.fraDetail.Controls.Add(Me.lblShinseiDate)
        Me.fraDetail.Controls.Add(Me.txtSougiDate)
        Me.fraDetail.Controls.Add(Me.dtpSougiDate)
        Me.fraDetail.Controls.Add(Me.lblStrikeKind)
        Me.fraDetail.Controls.Add(Me.fraKumiaiList)
        Me.fraDetail.Controls.Add(Me.txtKenmei)
        Me.fraDetail.Controls.Add(Me.Label3)
        Me.fraDetail.Controls.Add(Me.Label5)
        Me.fraDetail.Controls.Add(Me.Label6)
        Me.fraDetail.Controls.Add(Me.btnSampleText)
        Me.fraDetail.Controls.Add(Me.Label4)
        Me.fraDetail.Controls.Add(Me.txtUser)
        Me.fraDetail.Controls.Add(Me.txtNoKind)
        Me.fraDetail.Controls.Add(Me.Label1)
        Me.fraDetail.Controls.Add(Me.Label7)
        Me.fraDetail.Controls.Add(Me.Label2)
        Me.fraDetail.Controls.Add(Me.txtNo)
        Me.fraDetail.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.fraDetail.Location = New System.Drawing.Point(25, 60)
        Me.fraDetail.Name = "fraDetail"
        Me.fraDetail.Size = New System.Drawing.Size(975, 640)
        Me.fraDetail.TabIndex = 12
        Me.fraDetail.TabStop = False
        '
        'lblIchiji
        '
        Me.lblIchiji.AutoSize = True
        Me.lblIchiji.ForeColor = System.Drawing.Color.Red
        Me.lblIchiji.Location = New System.Drawing.Point(12, 612)
        Me.lblIchiji.Name = "lblIchiji"
        Me.lblIchiji.Size = New System.Drawing.Size(120, 16)
        Me.lblIchiji.TabIndex = 60
        Me.lblIchiji.Text = "※一時保存文書"
        '
        'txtShinseiDate
        '
        Me.txtShinseiDate.BackColor = System.Drawing.SystemColors.Info
        Me.txtShinseiDate.Location = New System.Drawing.Point(668, 59)
        Me.txtShinseiDate.Name = "txtShinseiDate"
        Me.txtShinseiDate.Size = New System.Drawing.Size(127, 23)
        Me.txtShinseiDate.TabIndex = 15
        '
        'lblShinseiDate
        '
        Me.lblShinseiDate.AutoSize = True
        Me.lblShinseiDate.Location = New System.Drawing.Point(606, 61)
        Me.lblShinseiDate.Name = "lblShinseiDate"
        Me.lblShinseiDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblShinseiDate.Size = New System.Drawing.Size(56, 16)
        Me.lblShinseiDate.TabIndex = 58
        Me.lblShinseiDate.Text = "申請日"
        '
        'txtSougiDate
        '
        Me.txtSougiDate.BackColor = System.Drawing.Color.White
        Me.txtSougiDate.Location = New System.Drawing.Point(175, 104)
        Me.txtSougiDate.Name = "txtSougiDate"
        Me.txtSougiDate.Size = New System.Drawing.Size(135, 23)
        Me.txtSougiDate.TabIndex = 13
        '
        'dtpSougiDate
        '
        Me.dtpSougiDate.Location = New System.Drawing.Point(175, 103)
        Me.dtpSougiDate.Name = "dtpSougiDate"
        Me.dtpSougiDate.Size = New System.Drawing.Size(135, 23)
        Me.dtpSougiDate.TabIndex = 14
        '
        'lblStrikeKind
        '
        Me.lblStrikeKind.AutoSize = True
        Me.lblStrikeKind.Location = New System.Drawing.Point(477, 21)
        Me.lblStrikeKind.Name = "lblStrikeKind"
        Me.lblStrikeKind.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblStrikeKind.Size = New System.Drawing.Size(82, 16)
        Me.lblStrikeKind.TabIndex = 48
        Me.lblStrikeKind.Text = "【申し入れ】"
        '
        'fraKumiaiList
        '
        Me.fraKumiaiList.Controls.Add(Me.btnPickup)
        Me.fraKumiaiList.Controls.Add(Me.dgvResult)
        Me.fraKumiaiList.Location = New System.Drawing.Point(175, 245)
        Me.fraKumiaiList.Name = "fraKumiaiList"
        Me.fraKumiaiList.Size = New System.Drawing.Size(609, 363)
        Me.fraKumiaiList.TabIndex = 55
        Me.fraKumiaiList.TabStop = False
        Me.fraKumiaiList.Text = "組合員選択一覧"
        '
        'btnPickup
        '
        Me.btnPickup.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPickup.Location = New System.Drawing.Point(350, 322)
        Me.btnPickup.Name = "btnPickup"
        Me.btnPickup.Size = New System.Drawing.Size(110, 30)
        Me.btnPickup.TabIndex = 19
        Me.btnPickup.Text = "組合員選出"
        Me.btnPickup.UseVisualStyleBackColor = True
        '
        'dgvResult
        '
        Me.dgvResult.AllowUserToAddRows = False
        Me.dgvResult.AllowUserToDeleteRows = False
        Me.dgvResult.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvResult.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvResult.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4})
        Me.dgvResult.Location = New System.Drawing.Point(28, 22)
        Me.dgvResult.Name = "dgvResult"
        Me.dgvResult.RowHeadersVisible = False
        Me.dgvResult.RowTemplate.Height = 21
        Me.dgvResult.Size = New System.Drawing.Size(554, 290)
        Me.dgvResult.TabIndex = 18
        '
        'Column1
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column1.FillWeight = 60.0!
        Me.Column1.HeaderText = "社員番号"
        Me.Column1.Name = "Column1"
        '
        'Column2
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column2.DefaultCellStyle = DataGridViewCellStyle3
        Me.Column2.FillWeight = 120.0!
        Me.Column2.HeaderText = "名前"
        Me.Column2.Name = "Column2"
        Me.Column2.Width = 200
        '
        'Column3
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle4
        Me.Column3.HeaderText = "機種"
        Me.Column3.Name = "Column3"
        '
        'Column4
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column4.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column4.FillWeight = 120.0!
        Me.Column4.HeaderText = "組合支部"
        Me.Column4.Name = "Column4"
        '
        'txtKenmei
        '
        Me.txtKenmei.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtKenmei.Location = New System.Drawing.Point(175, 151)
        Me.txtKenmei.Multiline = True
        Me.txtKenmei.Name = "txtKenmei"
        Me.txtKenmei.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtKenmei.Size = New System.Drawing.Size(620, 65)
        Me.txtKenmei.TabIndex = 17
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.Red
        Me.Label3.Location = New System.Drawing.Point(12, 154)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(16, 16)
        Me.Label3.TabIndex = 53
        Me.Label3.Text = "*"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(34, 154)
        Me.Label5.Name = "Label5"
        Me.Label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label5.Size = New System.Drawing.Size(40, 16)
        Me.Label5.TabIndex = 54
        Me.Label5.Text = "件名"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(474, 105)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 16)
        Me.Label6.TabIndex = 43
        Me.Label6.Text = "*"
        '
        'btnSampleText
        '
        Me.btnSampleText.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSampleText.Location = New System.Drawing.Point(335, 100)
        Me.btnSampleText.Name = "btnSampleText"
        Me.btnSampleText.Size = New System.Drawing.Size(110, 30)
        Me.btnSampleText.TabIndex = 52
        Me.btnSampleText.Text = "例文出力"
        Me.btnSampleText.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(495, 105)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label4.Size = New System.Drawing.Size(56, 16)
        Me.Label4.TabIndex = 44
        Me.Label4.Text = "申請者"
        '
        'txtUser
        '
        Me.txtUser.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtUser.Location = New System.Drawing.Point(568, 104)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(227, 23)
        Me.txtUser.TabIndex = 16
        '
        'txtNoKind
        '
        Me.txtNoKind.BackColor = System.Drawing.SystemColors.Info
        Me.txtNoKind.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNoKind.Location = New System.Drawing.Point(175, 18)
        Me.txtNoKind.Name = "txtNoKind"
        Me.txtNoKind.Size = New System.Drawing.Size(148, 23)
        Me.txtNoKind.TabIndex = 11
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label1.Size = New System.Drawing.Size(104, 16)
        Me.Label1.TabIndex = 46
        Me.Label1.Text = "通告番号種別"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 105)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label7.Size = New System.Drawing.Size(72, 16)
        Me.Label7.TabIndex = 47
        Me.Label7.Text = "争議日付"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 62)
        Me.Label2.Name = "Label2"
        Me.Label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label2.Size = New System.Drawing.Size(136, 16)
        Me.Label2.TabIndex = 49
        Me.Label2.Text = "争議行為通告番号"
        '
        'txtNo
        '
        Me.txtNo.BackColor = System.Drawing.SystemColors.Info
        Me.txtNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNo.Location = New System.Drawing.Point(175, 59)
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(104, 23)
        Me.txtNo.TabIndex = 12
        '
        'btnSaveTmp
        '
        Me.btnSaveTmp.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSaveTmp.Location = New System.Drawing.Point(555, 729)
        Me.btnSaveTmp.Name = "btnSaveTmp"
        Me.btnSaveTmp.Size = New System.Drawing.Size(110, 30)
        Me.btnSaveTmp.TabIndex = 21
        Me.btnSaveTmp.Text = "一時保存確認"
        Me.btnSaveTmp.UseVisualStyleBackColor = True
        '
        'btnChange
        '
        Me.btnChange.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnChange.Location = New System.Drawing.Point(710, 729)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(110, 30)
        Me.btnChange.TabIndex = 23
        Me.btnChange.Text = "登録確認"
        Me.btnChange.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(870, 729)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 25
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBack.Location = New System.Drawing.Point(870, 729)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(110, 30)
        Me.btnBack.TabIndex = 24
        Me.btnBack.Text = "戻る"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.Location = New System.Drawing.Point(710, 729)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(110, 30)
        Me.btnUpdate.TabIndex = 22
        Me.btnUpdate.Text = "内容変更"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(63, 729)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 20
        Me.btnPrint.Text = "プレ印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'UC040203
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.btnSaveTmp)
        Me.Controls.Add(Me.fraDetail)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC040203"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.fraDetail.ResumeLayout(False)
        Me.fraDetail.PerformLayout()
        Me.fraKumiaiList.ResumeLayout(False)
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents fraDetail As System.Windows.Forms.GroupBox
    Friend WithEvents btnSampleText As System.Windows.Forms.Button
    Friend WithEvents txtNoKind As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNo As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents txtKenmei As System.Windows.Forms.TextBox
    Friend WithEvents fraKumiaiList As System.Windows.Forms.GroupBox
    Friend WithEvents dgvResult As System.Windows.Forms.DataGridView
    Friend WithEvents btnPickup As System.Windows.Forms.Button
    Friend WithEvents btnSaveTmp As System.Windows.Forms.Button
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblStrikeKind As System.Windows.Forms.Label
    Friend WithEvents dtpSougiDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents txtSougiDate As System.Windows.Forms.TextBox
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents txtShinseiDate As System.Windows.Forms.TextBox
    Friend WithEvents lblShinseiDate As System.Windows.Forms.Label
    Friend WithEvents lblIchiji As System.Windows.Forms.Label

End Class
