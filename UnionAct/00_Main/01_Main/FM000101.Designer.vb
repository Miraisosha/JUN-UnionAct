<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM000101
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
        Me.lblTitle = New System.Windows.Forms.Label
        Me.lblMemberNo = New System.Windows.Forms.Label
        Me.lblPwd = New System.Windows.Forms.Label
        Me.lblPeriod = New System.Windows.Forms.Label
        Me.txtMemberNo = New System.Windows.Forms.TextBox
        Me.txtPwd = New System.Windows.Forms.TextBox
        Me.cboPeriod = New System.Windows.Forms.ComboBox
        Me.btnBegin = New System.Windows.Forms.Button
        Me.btnEnd = New System.Windows.Forms.Button
        Me.txtInfomation = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("MS UI Gothic", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(60, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(238, 33)
        Me.lblTitle.TabIndex = 7
        Me.lblTitle.Text = "総合ＯＡシステム"
        '
        'lblMemberNo
        '
        Me.lblMemberNo.AutoSize = True
        Me.lblMemberNo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMemberNo.Location = New System.Drawing.Point(63, 77)
        Me.lblMemberNo.Name = "lblMemberNo"
        Me.lblMemberNo.Size = New System.Drawing.Size(72, 16)
        Me.lblMemberNo.TabIndex = 1
        Me.lblMemberNo.Text = "社員番号"
        '
        'lblPwd
        '
        Me.lblPwd.AutoSize = True
        Me.lblPwd.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPwd.Location = New System.Drawing.Point(65, 106)
        Me.lblPwd.Name = "lblPwd"
        Me.lblPwd.Size = New System.Drawing.Size(70, 16)
        Me.lblPwd.TabIndex = 1
        Me.lblPwd.Text = "パスワード"
        '
        'lblPeriod
        '
        Me.lblPeriod.AutoSize = True
        Me.lblPeriod.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPeriod.Location = New System.Drawing.Point(111, 136)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.Size = New System.Drawing.Size(24, 16)
        Me.lblPeriod.TabIndex = 1
        Me.lblPeriod.Text = "期"
        '
        'txtMemberNo
        '
        Me.txtMemberNo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtMemberNo.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtMemberNo.Location = New System.Drawing.Point(141, 74)
        Me.txtMemberNo.Name = "txtMemberNo"
        Me.txtMemberNo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.txtMemberNo.Size = New System.Drawing.Size(157, 23)
        Me.txtMemberNo.TabIndex = 0
        Me.txtMemberNo.Text = "9999999"
        '
        'txtPwd
        '
        Me.txtPwd.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtPwd.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.txtPwd.Location = New System.Drawing.Point(141, 103)
        Me.txtPwd.Name = "txtPwd"
        Me.txtPwd.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txtPwd.ShortcutsEnabled = False
        Me.txtPwd.Size = New System.Drawing.Size(157, 23)
        Me.txtPwd.TabIndex = 1
        Me.txtPwd.Text = "9999999"
        '
        'cboPeriod
        '
        Me.cboPeriod.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboPeriod.FormattingEnabled = True
        Me.cboPeriod.Location = New System.Drawing.Point(141, 132)
        Me.cboPeriod.Name = "cboPeriod"
        Me.cboPeriod.Size = New System.Drawing.Size(93, 24)
        Me.cboPeriod.TabIndex = 2
        Me.cboPeriod.Text = "第99期"
        '
        'btnBegin
        '
        Me.btnBegin.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnBegin.Location = New System.Drawing.Point(66, 174)
        Me.btnBegin.Name = "btnBegin"
        Me.btnBegin.Size = New System.Drawing.Size(88, 28)
        Me.btnBegin.TabIndex = 3
        Me.btnBegin.Text = "開始"
        Me.btnBegin.UseVisualStyleBackColor = True
        '
        'btnEnd
        '
        Me.btnEnd.Font = New System.Drawing.Font("MS UI Gothic", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnEnd.Location = New System.Drawing.Point(210, 174)
        Me.btnEnd.Name = "btnEnd"
        Me.btnEnd.Size = New System.Drawing.Size(88, 28)
        Me.btnEnd.TabIndex = 4
        Me.btnEnd.Text = "終了"
        Me.btnEnd.UseVisualStyleBackColor = True
        '
        'txtInfomation
        '
        Me.txtInfomation.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtInfomation.Location = New System.Drawing.Point(12, 219)
        Me.txtInfomation.Multiline = True
        Me.txtInfomation.Name = "txtInfomation"
        Me.txtInfomation.ReadOnly = True
        Me.txtInfomation.Size = New System.Drawing.Size(339, 95)
        Me.txtInfomation.TabIndex = 5
        Me.txtInfomation.TabStop = False
        Me.txtInfomation.Text = "YYYY/MM/DD hh:mm～hh:mmの間、システムメンテナンスのためご利用になれません。"
        '
        'FM000101
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(363, 356)
        Me.Controls.Add(Me.txtInfomation)
        Me.Controls.Add(Me.btnEnd)
        Me.Controls.Add(Me.btnBegin)
        Me.Controls.Add(Me.cboPeriod)
        Me.Controls.Add(Me.txtPwd)
        Me.Controls.Add(Me.txtMemberNo)
        Me.Controls.Add(Me.lblPeriod)
        Me.Controls.Add(Me.lblPwd)
        Me.Controls.Add(Me.lblMemberNo)
        Me.Controls.Add(Me.lblTitle)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM000101"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ユーザ認証"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblMemberNo As System.Windows.Forms.Label
    Friend WithEvents lblPwd As System.Windows.Forms.Label
    Friend WithEvents lblPeriod As System.Windows.Forms.Label
    Friend WithEvents txtMemberNo As System.Windows.Forms.TextBox
    Friend WithEvents txtPwd As System.Windows.Forms.TextBox
    Friend WithEvents cboPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents btnBegin As System.Windows.Forms.Button
    Friend WithEvents btnEnd As System.Windows.Forms.Button
    Friend WithEvents txtInfomation As System.Windows.Forms.TextBox

End Class
