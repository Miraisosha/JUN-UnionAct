<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM000102
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
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.lblLogInTerm = New System.Windows.Forms.Label()
        Me.lblLogInUserInfo = New System.Windows.Forms.Label()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.lblSelectMenu = New System.Windows.Forms.Label()
        Me.DataTable1BindingSource1 = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSet2 = New UnionAct.DataSet2()
        Me.btnLogOff = New System.Windows.Forms.Button()
        Me.btnEnd = New System.Windows.Forms.Button()
        Me.btnChangeAuthority = New System.Windows.Forms.Button()
        Me.txtInfoMessage = New System.Windows.Forms.TextBox()
        Me.llbChangePassword = New System.Windows.Forms.LinkLabel()
        Me.DataTable1BindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.DataSet1 = New UnionAct.DataSet1()
        Me.dgdMenu = New System.Windows.Forms.DataGridView()
        Me.grbAuthority = New System.Windows.Forms.GroupBox()
        Me.lblLogInAuthority = New System.Windows.Forms.Label()
        Me.btnSync = New System.Windows.Forms.Button()
        Me.pnlMain.SuspendLayout()
        CType(Me.DataTable1BindingSource1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSet2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataTable1BindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DataSet1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgdMenu, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grbAuthority.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblLogInTerm
        '
        Me.lblLogInTerm.AutoSize = True
        Me.lblLogInTerm.Font = New System.Drawing.Font("MS UI Gothic", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblLogInTerm.Location = New System.Drawing.Point(27, 23)
        Me.lblLogInTerm.Name = "lblLogInTerm"
        Me.lblLogInTerm.Size = New System.Drawing.Size(73, 20)
        Me.lblLogInTerm.TabIndex = 0
        Me.lblLogInTerm.Text = "第**期"
        '
        'lblLogInUserInfo
        '
        Me.lblLogInUserInfo.AutoSize = True
        Me.lblLogInUserInfo.Font = New System.Drawing.Font("MS UI Gothic", 15.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblLogInUserInfo.Location = New System.Drawing.Point(175, 23)
        Me.lblLogInUserInfo.Name = "lblLogInUserInfo"
        Me.lblLogInUserInfo.Size = New System.Drawing.Size(133, 20)
        Me.lblLogInUserInfo.TabIndex = 0
        Me.lblLogInUserInfo.Text = "*****　*****"
        '
        'pnlMain
        '
        Me.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlMain.Controls.Add(Me.lblSelectMenu)
        Me.pnlMain.Location = New System.Drawing.Point(238, 65)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1024, 820)
        Me.pnlMain.TabIndex = 100
        '
        'lblSelectMenu
        '
        Me.lblSelectMenu.AutoSize = True
        Me.lblSelectMenu.Font = New System.Drawing.Font("MS UI Gothic", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSelectMenu.Location = New System.Drawing.Point(255, 385)
        Me.lblSelectMenu.Name = "lblSelectMenu"
        Me.lblSelectMenu.Size = New System.Drawing.Size(512, 48)
        Me.lblSelectMenu.TabIndex = 0
        Me.lblSelectMenu.Text = "メニューを選択してください"
        '
        'DataTable1BindingSource1
        '
        Me.DataTable1BindingSource1.DataMember = "DataTable1"
        Me.DataTable1BindingSource1.DataSource = Me.DataSet2
        '
        'DataSet2
        '
        Me.DataSet2.DataSetName = "DataSet2"
        Me.DataSet2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'btnLogOff
        '
        Me.btnLogOff.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnLogOff.Location = New System.Drawing.Point(12, 905)
        Me.btnLogOff.Name = "btnLogOff"
        Me.btnLogOff.Size = New System.Drawing.Size(110, 30)
        Me.btnLogOff.TabIndex = 101
        Me.btnLogOff.Text = "ログオフ"
        Me.btnLogOff.UseVisualStyleBackColor = True
        '
        'btnEnd
        '
        Me.btnEnd.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnEnd.Location = New System.Drawing.Point(138, 905)
        Me.btnEnd.Name = "btnEnd"
        Me.btnEnd.Size = New System.Drawing.Size(110, 30)
        Me.btnEnd.TabIndex = 102
        Me.btnEnd.Text = "終了"
        Me.btnEnd.UseVisualStyleBackColor = True
        '
        'btnChangeAuthority
        '
        Me.btnChangeAuthority.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnChangeAuthority.Location = New System.Drawing.Point(260, 905)
        Me.btnChangeAuthority.Name = "btnChangeAuthority"
        Me.btnChangeAuthority.Size = New System.Drawing.Size(126, 30)
        Me.btnChangeAuthority.TabIndex = 103
        Me.btnChangeAuthority.Text = "操作権限の変更"
        Me.btnChangeAuthority.UseVisualStyleBackColor = True
        '
        'txtInfoMessage
        '
        Me.txtInfoMessage.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.txtInfoMessage.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtInfoMessage.Location = New System.Drawing.Point(398, 908)
        Me.txtInfoMessage.Name = "txtInfoMessage"
        Me.txtInfoMessage.ReadOnly = True
        Me.txtInfoMessage.Size = New System.Drawing.Size(742, 23)
        Me.txtInfoMessage.TabIndex = 102
        Me.txtInfoMessage.TabStop = False
        '
        'llbChangePassword
        '
        Me.llbChangePassword.AutoSize = True
        Me.llbChangePassword.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.llbChangePassword.Location = New System.Drawing.Point(1153, 23)
        Me.llbChangePassword.Name = "llbChangePassword"
        Me.llbChangePassword.Size = New System.Drawing.Size(101, 16)
        Me.llbChangePassword.TabIndex = 103
        Me.llbChangePassword.TabStop = True
        Me.llbChangePassword.Text = "パスワード変更"
        '
        'DataTable1BindingSource
        '
        Me.DataTable1BindingSource.DataMember = "DataTable1"
        Me.DataTable1BindingSource.DataSource = Me.DataSet1
        '
        'DataSet1
        '
        Me.DataSet1.DataSetName = "DataSet1"
        Me.DataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema
        '
        'dgdMenu
        '
        Me.dgdMenu.AllowUserToAddRows = False
        Me.dgdMenu.AllowUserToDeleteRows = False
        Me.dgdMenu.AllowUserToResizeColumns = False
        Me.dgdMenu.AllowUserToResizeRows = False
        Me.dgdMenu.BackgroundColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgdMenu.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgdMenu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgdMenu.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgdMenu.Location = New System.Drawing.Point(12, 65)
        Me.dgdMenu.MultiSelect = False
        Me.dgdMenu.Name = "dgdMenu"
        Me.dgdMenu.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgdMenu.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgdMenu.RowTemplate.Height = 21
        Me.dgdMenu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdMenu.Size = New System.Drawing.Size(220, 820)
        Me.dgdMenu.TabIndex = 104
        '
        'grbAuthority
        '
        Me.grbAuthority.Controls.Add(Me.lblLogInAuthority)
        Me.grbAuthority.Location = New System.Drawing.Point(438, 2)
        Me.grbAuthority.Name = "grbAuthority"
        Me.grbAuthority.Size = New System.Drawing.Size(630, 51)
        Me.grbAuthority.TabIndex = 105
        Me.grbAuthority.TabStop = False
        '
        'lblLogInAuthority
        '
        Me.lblLogInAuthority.AutoSize = True
        Me.lblLogInAuthority.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblLogInAuthority.Location = New System.Drawing.Point(31, 21)
        Me.lblLogInAuthority.Name = "lblLogInAuthority"
        Me.lblLogInAuthority.Size = New System.Drawing.Size(289, 16)
        Me.lblLogInAuthority.TabIndex = 0
        Me.lblLogInAuthority.Text = "『***（***）』の操作権限でログインしています"
        '
        'btnSync
        '
        Me.btnSync.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSync.Location = New System.Drawing.Point(1152, 904)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(110, 30)
        Me.btnSync.TabIndex = 106
        Me.btnSync.Text = "情報更新"
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'FM000102
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1272, 961)
        Me.Controls.Add(Me.btnSync)
        Me.Controls.Add(Me.grbAuthority)
        Me.Controls.Add(Me.dgdMenu)
        Me.Controls.Add(Me.llbChangePassword)
        Me.Controls.Add(Me.txtInfoMessage)
        Me.Controls.Add(Me.btnChangeAuthority)
        Me.Controls.Add(Me.btnEnd)
        Me.Controls.Add(Me.btnLogOff)
        Me.Controls.Add(Me.pnlMain)
        Me.Controls.Add(Me.lblLogInUserInfo)
        Me.Controls.Add(Me.lblLogInTerm)
        Me.MaximizeBox = False
        Me.Name = "FM000102"
        Me.Text = "全日本空輸乗員組合　総合ＯＡシステム"
        Me.pnlMain.ResumeLayout(False)
        Me.pnlMain.PerformLayout()
        CType(Me.DataTable1BindingSource1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSet2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataTable1BindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DataSet1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgdMenu, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grbAuthority.ResumeLayout(False)
        Me.grbAuthority.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblLogInTerm As System.Windows.Forms.Label
    Friend WithEvents lblLogInUserInfo As System.Windows.Forms.Label
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents btnLogOff As System.Windows.Forms.Button
    Friend WithEvents btnEnd As System.Windows.Forms.Button
    Friend WithEvents btnChangeAuthority As System.Windows.Forms.Button
    Friend WithEvents txtInfoMessage As System.Windows.Forms.TextBox
    Friend WithEvents llbChangePassword As System.Windows.Forms.LinkLabel
    Friend WithEvents lblSelectMenu As System.Windows.Forms.Label
    Friend WithEvents DataTable1BindingSource As System.Windows.Forms.BindingSource
    Friend WithEvents DataSet1 As UnionAct.DataSet1
    Friend WithEvents DataTable1BindingSource1 As System.Windows.Forms.BindingSource
    Friend WithEvents DataSet2 As UnionAct.DataSet2
    Friend WithEvents dgdMenu As System.Windows.Forms.DataGridView
    Friend WithEvents grbAuthority As System.Windows.Forms.GroupBox
    Friend WithEvents lblLogInAuthority As System.Windows.Forms.Label
    Friend WithEvents btnSync As System.Windows.Forms.Button
End Class
