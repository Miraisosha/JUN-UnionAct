<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM040605
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
        Me.lblIndispensableSubject = New System.Windows.Forms.Label()
        Me.lblIndispensableDocCode = New System.Windows.Forms.Label()
        Me.lblIndispensablePeriod = New System.Windows.Forms.Label()
        Me.lblSubject = New System.Windows.Forms.Label()
        Me.lblPeriod = New System.Windows.Forms.Label()
        Me.lblDocCode = New System.Windows.Forms.Label()
        Me.cboPeriod = New System.Windows.Forms.ComboBox()
        Me.txtDocCode = New System.Windows.Forms.TextBox()
        Me.txtSubject = New System.Windows.Forms.TextBox()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblMemo = New System.Windows.Forms.Label()
        Me.CachedCR0801P11 = New UnionAct.CachedCR0801P1()
        Me.SuspendLayout()
        '
        'lblIndispensableSubject
        '
        Me.lblIndispensableSubject.AutoSize = True
        Me.lblIndispensableSubject.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableSubject.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableSubject.Location = New System.Drawing.Point(74, 117)
        Me.lblIndispensableSubject.Name = "lblIndispensableSubject"
        Me.lblIndispensableSubject.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableSubject.TabIndex = 6
        Me.lblIndispensableSubject.Text = "*"
        '
        'lblIndispensableDocCode
        '
        Me.lblIndispensableDocCode.AutoSize = True
        Me.lblIndispensableDocCode.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensableDocCode.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensableDocCode.Location = New System.Drawing.Point(74, 76)
        Me.lblIndispensableDocCode.Name = "lblIndispensableDocCode"
        Me.lblIndispensableDocCode.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensableDocCode.TabIndex = 3
        Me.lblIndispensableDocCode.Text = "*"
        '
        'lblIndispensablePeriod
        '
        Me.lblIndispensablePeriod.AutoSize = True
        Me.lblIndispensablePeriod.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblIndispensablePeriod.ForeColor = System.Drawing.Color.Red
        Me.lblIndispensablePeriod.Location = New System.Drawing.Point(75, 36)
        Me.lblIndispensablePeriod.Name = "lblIndispensablePeriod"
        Me.lblIndispensablePeriod.Size = New System.Drawing.Size(16, 16)
        Me.lblIndispensablePeriod.TabIndex = 0
        Me.lblIndispensablePeriod.Text = "*"
        '
        'lblSubject
        '
        Me.lblSubject.AutoSize = True
        Me.lblSubject.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSubject.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblSubject.Location = New System.Drawing.Point(96, 117)
        Me.lblSubject.Name = "lblSubject"
        Me.lblSubject.Size = New System.Drawing.Size(40, 16)
        Me.lblSubject.TabIndex = 7
        Me.lblSubject.Text = "標題"
        '
        'lblPeriod
        '
        Me.lblPeriod.AutoSize = True
        Me.lblPeriod.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPeriod.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPeriod.Location = New System.Drawing.Point(97, 36)
        Me.lblPeriod.Name = "lblPeriod"
        Me.lblPeriod.Size = New System.Drawing.Size(24, 16)
        Me.lblPeriod.TabIndex = 1
        Me.lblPeriod.Text = "期"
        '
        'lblDocCode
        '
        Me.lblDocCode.AutoSize = True
        Me.lblDocCode.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblDocCode.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblDocCode.Location = New System.Drawing.Point(96, 76)
        Me.lblDocCode.Name = "lblDocCode"
        Me.lblDocCode.Size = New System.Drawing.Size(76, 16)
        Me.lblDocCode.TabIndex = 4
        Me.lblDocCode.Text = "管理コード"
        '
        'cboPeriod
        '
        Me.cboPeriod.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboPeriod.FormattingEnabled = True
        Me.cboPeriod.Location = New System.Drawing.Point(193, 33)
        Me.cboPeriod.Name = "cboPeriod"
        Me.cboPeriod.Size = New System.Drawing.Size(147, 24)
        Me.cboPeriod.TabIndex = 2
        '
        'txtDocCode
        '
        Me.txtDocCode.BackColor = System.Drawing.Color.LightYellow
        Me.txtDocCode.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtDocCode.Location = New System.Drawing.Point(193, 76)
        Me.txtDocCode.Name = "txtDocCode"
        Me.txtDocCode.ReadOnly = True
        Me.txtDocCode.Size = New System.Drawing.Size(284, 23)
        Me.txtDocCode.TabIndex = 5
        '
        'txtSubject
        '
        Me.txtSubject.BackColor = System.Drawing.Color.LightYellow
        Me.txtSubject.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtSubject.Location = New System.Drawing.Point(193, 117)
        Me.txtSubject.Name = "txtSubject"
        Me.txtSubject.ReadOnly = True
        Me.txtSubject.Size = New System.Drawing.Size(343, 23)
        Me.txtSubject.TabIndex = 8
        '
        'btnEdit
        '
        Me.btnEdit.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnEdit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnEdit.Location = New System.Drawing.Point(163, 185)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(110, 30)
        Me.btnEdit.TabIndex = 9
        Me.btnEdit.Text = "編集"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnCancel.Location = New System.Drawing.Point(332, 185)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblMemo
        '
        Me.lblMemo.AutoSize = True
        Me.lblMemo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblMemo.Location = New System.Drawing.Point(196, 143)
        Me.lblMemo.Name = "lblMemo"
        Me.lblMemo.Size = New System.Drawing.Size(342, 16)
        Me.lblMemo.TabIndex = 11
        Me.lblMemo.Text = "（※）標題の変更は、[編集] 画面にて行ってください。"
        '
        'FM040605
        '
        Me.AcceptButton = Me.btnEdit
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(604, 249)
        Me.Controls.Add(Me.lblMemo)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnEdit)
        Me.Controls.Add(Me.txtSubject)
        Me.Controls.Add(Me.txtDocCode)
        Me.Controls.Add(Me.cboPeriod)
        Me.Controls.Add(Me.lblIndispensableSubject)
        Me.Controls.Add(Me.lblIndispensableDocCode)
        Me.Controls.Add(Me.lblIndispensablePeriod)
        Me.Controls.Add(Me.lblSubject)
        Me.Controls.Add(Me.lblPeriod)
        Me.Controls.Add(Me.lblDocCode)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM040605"
        Me.Text = "編集する文書の期を選択して下さい。"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblIndispensableSubject As System.Windows.Forms.Label
    Friend WithEvents lblIndispensableDocCode As System.Windows.Forms.Label
    Friend WithEvents lblIndispensablePeriod As System.Windows.Forms.Label
    Friend WithEvents lblSubject As System.Windows.Forms.Label
    Friend WithEvents lblPeriod As System.Windows.Forms.Label
    Friend WithEvents lblDocCode As System.Windows.Forms.Label
    Friend WithEvents cboPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents txtDocCode As System.Windows.Forms.TextBox
    Friend WithEvents txtSubject As System.Windows.Forms.TextBox
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblMemo As System.Windows.Forms.Label
    Friend WithEvents CachedCR0801P11 As UnionAct.CachedCR0801P1
End Class
