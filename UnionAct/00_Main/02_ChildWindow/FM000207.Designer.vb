<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM000207
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
        Me.lblNote1 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnSync = New System.Windows.Forms.Button
        Me.chkMaint1 = New System.Windows.Forms.CheckBox
        Me.grpMain1 = New System.Windows.Forms.GroupBox
        Me.groSub2 = New System.Windows.Forms.GroupBox
        Me.lblNote5 = New System.Windows.Forms.Label
        Me.btnCompServer = New System.Windows.Forms.Button
        Me.grpSub1 = New System.Windows.Forms.GroupBox
        Me.lblNote4 = New System.Windows.Forms.Label
        Me.lblNote3 = New System.Windows.Forms.Label
        Me.lblNote2 = New System.Windows.Forms.Label
        Me.btnComp = New System.Windows.Forms.Button
        Me.btnSubmit = New System.Windows.Forms.Button
        Me.btnRenew = New System.Windows.Forms.Button
        Me.grpMain1.SuspendLayout()
        Me.groSub2.SuspendLayout()
        Me.grpSub1.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblNote1
        '
        Me.lblNote1.AutoSize = True
        Me.lblNote1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNote1.Location = New System.Drawing.Point(40, 21)
        Me.lblNote1.Name = "lblNote1"
        Me.lblNote1.Size = New System.Drawing.Size(262, 16)
        Me.lblNote1.TabIndex = 0
        Me.lblNote1.Text = "マスタデータとのレプリケーションを行います"
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(434, 417)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 3
        Me.btnCancel.Text = "閉じる"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnSync
        '
        Me.btnSync.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSync.Location = New System.Drawing.Point(385, 14)
        Me.btnSync.Name = "btnSync"
        Me.btnSync.Size = New System.Drawing.Size(110, 30)
        Me.btnSync.TabIndex = 1
        Me.btnSync.Text = "情報更新"
        Me.btnSync.UseVisualStyleBackColor = True
        '
        'chkMaint1
        '
        Me.chkMaint1.AutoSize = True
        Me.chkMaint1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.chkMaint1.Location = New System.Drawing.Point(10, 11)
        Me.chkMaint1.Name = "chkMaint1"
        Me.chkMaint1.Size = New System.Drawing.Size(99, 20)
        Me.chkMaint1.TabIndex = 0
        Me.chkMaint1.Text = "メンテナンス"
        Me.chkMaint1.UseVisualStyleBackColor = True
        '
        'grpMain1
        '
        Me.grpMain1.Controls.Add(Me.groSub2)
        Me.grpMain1.Controls.Add(Me.grpSub1)
        Me.grpMain1.Controls.Add(Me.chkMaint1)
        Me.grpMain1.Font = New System.Drawing.Font("MS UI Gothic", 2.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpMain1.Location = New System.Drawing.Point(12, 64)
        Me.grpMain1.Name = "grpMain1"
        Me.grpMain1.Size = New System.Drawing.Size(532, 338)
        Me.grpMain1.TabIndex = 2
        Me.grpMain1.TabStop = False
        '
        'groSub2
        '
        Me.groSub2.Controls.Add(Me.lblNote5)
        Me.groSub2.Controls.Add(Me.btnCompServer)
        Me.groSub2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.groSub2.Location = New System.Drawing.Point(31, 232)
        Me.groSub2.Name = "groSub2"
        Me.groSub2.Size = New System.Drawing.Size(474, 85)
        Me.groSub2.TabIndex = 2
        Me.groSub2.TabStop = False
        Me.groSub2.Text = "サーバー関連"
        '
        'lblNote5
        '
        Me.lblNote5.AutoSize = True
        Me.lblNote5.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNote5.Location = New System.Drawing.Point(18, 43)
        Me.lblNote5.Name = "lblNote5"
        Me.lblNote5.Size = New System.Drawing.Size(303, 16)
        Me.lblNote5.TabIndex = 0
        Me.lblNote5.Text = "サーバー上のデータベースを修復・最適化します"
        '
        'btnCompServer
        '
        Me.btnCompServer.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCompServer.Location = New System.Drawing.Point(342, 36)
        Me.btnCompServer.Name = "btnCompServer"
        Me.btnCompServer.Size = New System.Drawing.Size(110, 30)
        Me.btnCompServer.TabIndex = 1
        Me.btnCompServer.Text = "修復・最適化"
        Me.btnCompServer.UseVisualStyleBackColor = True
        '
        'grpSub1
        '
        Me.grpSub1.Controls.Add(Me.lblNote4)
        Me.grpSub1.Controls.Add(Me.lblNote3)
        Me.grpSub1.Controls.Add(Me.lblNote2)
        Me.grpSub1.Controls.Add(Me.btnComp)
        Me.grpSub1.Controls.Add(Me.btnSubmit)
        Me.grpSub1.Controls.Add(Me.btnRenew)
        Me.grpSub1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.grpSub1.Location = New System.Drawing.Point(31, 37)
        Me.grpSub1.Name = "grpSub1"
        Me.grpSub1.Size = New System.Drawing.Size(474, 179)
        Me.grpSub1.TabIndex = 1
        Me.grpSub1.TabStop = False
        Me.grpSub1.Text = "ローカル関連"
        '
        'lblNote4
        '
        Me.lblNote4.AutoSize = True
        Me.lblNote4.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNote4.Location = New System.Drawing.Point(18, 133)
        Me.lblNote4.Name = "lblNote4"
        Me.lblNote4.Size = New System.Drawing.Size(309, 16)
        Me.lblNote4.TabIndex = 4
        Me.lblNote4.Text = "PC上のDBを破棄し、正常なDBを再取得します"
        '
        'lblNote3
        '
        Me.lblNote3.AutoSize = True
        Me.lblNote3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNote3.Location = New System.Drawing.Point(18, 90)
        Me.lblNote3.Name = "lblNote3"
        Me.lblNote3.Size = New System.Drawing.Size(281, 16)
        Me.lblNote3.TabIndex = 2
        Me.lblNote3.Text = "PC上のデータベースをサーバーに提出します"
        '
        'lblNote2
        '
        Me.lblNote2.AutoSize = True
        Me.lblNote2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNote2.Location = New System.Drawing.Point(18, 47)
        Me.lblNote2.Name = "lblNote2"
        Me.lblNote2.Size = New System.Drawing.Size(271, 16)
        Me.lblNote2.TabIndex = 0
        Me.lblNote2.Text = "PC上のデータベースを修復・最適化します"
        '
        'btnComp
        '
        Me.btnComp.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnComp.Location = New System.Drawing.Point(342, 40)
        Me.btnComp.Name = "btnComp"
        Me.btnComp.Size = New System.Drawing.Size(110, 30)
        Me.btnComp.TabIndex = 1
        Me.btnComp.Text = "修復・最適化"
        Me.btnComp.UseVisualStyleBackColor = True
        '
        'btnSubmit
        '
        Me.btnSubmit.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSubmit.Location = New System.Drawing.Point(342, 83)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(110, 30)
        Me.btnSubmit.TabIndex = 3
        Me.btnSubmit.Text = "調査提出"
        Me.btnSubmit.UseVisualStyleBackColor = True
        '
        'btnRenew
        '
        Me.btnRenew.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnRenew.Location = New System.Drawing.Point(342, 126)
        Me.btnRenew.Name = "btnRenew"
        Me.btnRenew.Size = New System.Drawing.Size(110, 30)
        Me.btnRenew.TabIndex = 5
        Me.btnRenew.Text = "破棄・再取得"
        Me.btnRenew.UseVisualStyleBackColor = True
        '
        'FM000207
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(558, 463)
        Me.Controls.Add(Me.grpMain1)
        Me.Controls.Add(Me.lblNote1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSync)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM000207"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "データベースメンテナンス"
        Me.grpMain1.ResumeLayout(False)
        Me.grpMain1.PerformLayout()
        Me.groSub2.ResumeLayout(False)
        Me.groSub2.PerformLayout()
        Me.grpSub1.ResumeLayout(False)
        Me.grpSub1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblNote1 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnSync As System.Windows.Forms.Button
    Friend WithEvents chkMaint1 As System.Windows.Forms.CheckBox
    Friend WithEvents grpMain1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblNote5 As System.Windows.Forms.Label
    Friend WithEvents btnCompServer As System.Windows.Forms.Button
    Friend WithEvents groSub2 As System.Windows.Forms.GroupBox
    Friend WithEvents grpSub1 As System.Windows.Forms.GroupBox
    Friend WithEvents lblNote3 As System.Windows.Forms.Label
    Friend WithEvents lblNote2 As System.Windows.Forms.Label
    Friend WithEvents lblNote4 As System.Windows.Forms.Label
    Friend WithEvents btnComp As System.Windows.Forms.Button
    Friend WithEvents btnRenew As System.Windows.Forms.Button
    Friend WithEvents btnSubmit As System.Windows.Forms.Button
End Class
