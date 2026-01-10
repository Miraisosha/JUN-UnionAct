<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040206
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
        Me.btnCancel = New System.Windows.Forms.Button
        Me.fraDetail = New System.Windows.Forms.GroupBox
        Me.txtShinseiDate = New System.Windows.Forms.TextBox
        Me.lblShinseiDate = New System.Windows.Forms.Label
        Me.lblIchiji = New System.Windows.Forms.Label
        Me.txtSougiDate = New System.Windows.Forms.TextBox
        Me.dtpSougiDate = New System.Windows.Forms.DateTimePicker
        Me.txtHonbun = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtNoKanren = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnSampleText = New System.Windows.Forms.Button
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtUser = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtNo = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtNoKind = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnSaveTmp = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnBack = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.fraDetail.SuspendLayout()
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
        Me.lblTitle.Text = "争議行為終結通知 -　詳細"
        Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(855, 729)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 25
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'fraDetail
        '
        Me.fraDetail.Controls.Add(Me.txtShinseiDate)
        Me.fraDetail.Controls.Add(Me.lblShinseiDate)
        Me.fraDetail.Controls.Add(Me.lblIchiji)
        Me.fraDetail.Controls.Add(Me.txtSougiDate)
        Me.fraDetail.Controls.Add(Me.dtpSougiDate)
        Me.fraDetail.Controls.Add(Me.txtHonbun)
        Me.fraDetail.Controls.Add(Me.Label10)
        Me.fraDetail.Controls.Add(Me.Label12)
        Me.fraDetail.Controls.Add(Me.txtNoKanren)
        Me.fraDetail.Controls.Add(Me.Label7)
        Me.fraDetail.Controls.Add(Me.Label5)
        Me.fraDetail.Controls.Add(Me.Label6)
        Me.fraDetail.Controls.Add(Me.btnSampleText)
        Me.fraDetail.Controls.Add(Me.Label4)
        Me.fraDetail.Controls.Add(Me.txtUser)
        Me.fraDetail.Controls.Add(Me.Label3)
        Me.fraDetail.Controls.Add(Me.txtNo)
        Me.fraDetail.Controls.Add(Me.Label2)
        Me.fraDetail.Controls.Add(Me.txtNoKind)
        Me.fraDetail.Controls.Add(Me.Label1)
        Me.fraDetail.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.fraDetail.Location = New System.Drawing.Point(25, 60)
        Me.fraDetail.Name = "fraDetail"
        Me.fraDetail.Size = New System.Drawing.Size(975, 640)
        Me.fraDetail.TabIndex = 23
        Me.fraDetail.TabStop = False
        '
        'txtShinseiDate
        '
        Me.txtShinseiDate.BackColor = System.Drawing.SystemColors.Info
        Me.txtShinseiDate.Location = New System.Drawing.Point(813, 18)
        Me.txtShinseiDate.Name = "txtShinseiDate"
        Me.txtShinseiDate.Size = New System.Drawing.Size(127, 23)
        Me.txtShinseiDate.TabIndex = 16
        '
        'lblShinseiDate
        '
        Me.lblShinseiDate.AutoSize = True
        Me.lblShinseiDate.Location = New System.Drawing.Point(751, 21)
        Me.lblShinseiDate.Name = "lblShinseiDate"
        Me.lblShinseiDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblShinseiDate.Size = New System.Drawing.Size(56, 16)
        Me.lblShinseiDate.TabIndex = 67
        Me.lblShinseiDate.Text = "申請日"
        '
        'lblIchiji
        '
        Me.lblIchiji.AutoSize = True
        Me.lblIchiji.ForeColor = System.Drawing.Color.Red
        Me.lblIchiji.Location = New System.Drawing.Point(13, 611)
        Me.lblIchiji.Name = "lblIchiji"
        Me.lblIchiji.Size = New System.Drawing.Size(120, 16)
        Me.lblIchiji.TabIndex = 66
        Me.lblIchiji.Text = "※一時保存文書"
        '
        'txtSougiDate
        '
        Me.txtSougiDate.BackColor = System.Drawing.Color.White
        Me.txtSougiDate.Location = New System.Drawing.Point(176, 102)
        Me.txtSougiDate.Name = "txtSougiDate"
        Me.txtSougiDate.Size = New System.Drawing.Size(135, 23)
        Me.txtSougiDate.TabIndex = 13
        '
        'dtpSougiDate
        '
        Me.dtpSougiDate.Location = New System.Drawing.Point(175, 102)
        Me.dtpSougiDate.Name = "dtpSougiDate"
        Me.dtpSougiDate.Size = New System.Drawing.Size(136, 23)
        Me.dtpSougiDate.TabIndex = 14
        '
        'txtHonbun
        '
        Me.txtHonbun.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtHonbun.Location = New System.Drawing.Point(175, 198)
        Me.txtHonbun.Multiline = True
        Me.txtHonbun.Name = "txtHonbun"
        Me.txtHonbun.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtHonbun.Size = New System.Drawing.Size(630, 369)
        Me.txtHonbun.TabIndex = 19
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(11, 270)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(16, 16)
        Me.Label10.TabIndex = 60
        Me.Label10.Text = "*"
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(28, 270)
        Me.Label12.Name = "Label12"
        Me.Label12.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label12.Size = New System.Drawing.Size(40, 16)
        Me.Label12.TabIndex = 61
        Me.Label12.Text = "本文"
        '
        'txtNoKanren
        '
        Me.txtNoKanren.BackColor = System.Drawing.SystemColors.Info
        Me.txtNoKanren.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNoKanren.Location = New System.Drawing.Point(653, 59)
        Me.txtNoKanren.Name = "txtNoKanren"
        Me.txtNoKanren.Size = New System.Drawing.Size(109, 23)
        Me.txtNoKanren.TabIndex = 17
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(479, 62)
        Me.Label7.Name = "Label7"
        Me.Label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label7.Size = New System.Drawing.Size(168, 16)
        Me.Label7.TabIndex = 54
        Me.Label7.Text = "関連争議行為通告番号"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(477, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(152, 16)
        Me.Label5.TabIndex = 53
        Me.Label5.Text = "【争議行為終結通知】"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label6.ForeColor = System.Drawing.Color.Red
        Me.Label6.Location = New System.Drawing.Point(479, 105)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(16, 16)
        Me.Label6.TabIndex = 46
        Me.Label6.Text = "*"
        '
        'btnSampleText
        '
        Me.btnSampleText.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSampleText.Location = New System.Drawing.Point(329, 100)
        Me.btnSampleText.Name = "btnSampleText"
        Me.btnSampleText.Size = New System.Drawing.Size(110, 30)
        Me.btnSampleText.TabIndex = 15
        Me.btnSampleText.Text = "例文出力"
        Me.btnSampleText.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(496, 105)
        Me.Label4.Name = "Label4"
        Me.Label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label4.Size = New System.Drawing.Size(56, 16)
        Me.Label4.TabIndex = 47
        Me.Label4.Text = "申請者"
        '
        'txtUser
        '
        Me.txtUser.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtUser.Location = New System.Drawing.Point(568, 102)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(237, 23)
        Me.txtUser.TabIndex = 18
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 105)
        Me.Label3.Name = "Label3"
        Me.Label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 50
        Me.Label3.Text = "争議日付"
        '
        'txtNo
        '
        Me.txtNo.BackColor = System.Drawing.SystemColors.Info
        Me.txtNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNo.Location = New System.Drawing.Point(175, 59)
        Me.txtNo.Name = "txtNo"
        Me.txtNo.Size = New System.Drawing.Size(121, 23)
        Me.txtNo.TabIndex = 12
        Me.txtNo.Text = "****"
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
        'txtNoKind
        '
        Me.txtNoKind.BackColor = System.Drawing.SystemColors.Info
        Me.txtNoKind.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtNoKind.Location = New System.Drawing.Point(175, 18)
        Me.txtNoKind.Name = "txtNoKind"
        Me.txtNoKind.Size = New System.Drawing.Size(148, 23)
        Me.txtNoKind.TabIndex = 11
        Me.txtNoKind.Text = "B(ANA宛)"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(16, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label1.Size = New System.Drawing.Size(104, 16)
        Me.Label1.TabIndex = 49
        Me.Label1.Text = "通告番号種別"
        '
        'btnChange
        '
        Me.btnChange.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnChange.Location = New System.Drawing.Point(705, 729)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(110, 30)
        Me.btnChange.TabIndex = 23
        Me.btnChange.Text = "登録確認"
        Me.btnChange.UseVisualStyleBackColor = True
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
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.Location = New System.Drawing.Point(705, 729)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(110, 30)
        Me.btnUpdate.TabIndex = 22
        Me.btnUpdate.Text = "内容変更"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnBack
        '
        Me.btnBack.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBack.Location = New System.Drawing.Point(855, 729)
        Me.btnBack.Name = "btnBack"
        Me.btnBack.Size = New System.Drawing.Size(110, 30)
        Me.btnBack.TabIndex = 24
        Me.btnBack.Text = "戻る"
        Me.btnBack.UseVisualStyleBackColor = True
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(41, 729)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 20
        Me.btnPrint.Text = "プレ印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'UC040206
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnBack)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.btnSaveTmp)
        Me.Controls.Add(Me.fraDetail)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblTitle)
        Me.Name = "UC040206"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.fraDetail.ResumeLayout(False)
        Me.fraDetail.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents fraDetail As System.Windows.Forms.GroupBox
    Friend WithEvents txtHonbun As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents txtNoKanren As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnSampleText As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtUser As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNo As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNoKind As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents btnSaveTmp As System.Windows.Forms.Button
    Friend WithEvents dtpSougiDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnBack As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents txtSougiDate As System.Windows.Forms.TextBox
    Friend WithEvents lblIchiji As System.Windows.Forms.Label
    Friend WithEvents txtShinseiDate As System.Windows.Forms.TextBox
    Friend WithEvents lblShinseiDate As System.Windows.Forms.Label

End Class
