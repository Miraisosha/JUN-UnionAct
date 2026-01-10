<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM040303
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FM040303))
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnOk = New System.Windows.Forms.Button
        Me.grpApplyArea = New System.Windows.Forms.GroupBox
        Me.lblKind2 = New System.Windows.Forms.Label
        Me.lklMemo = New System.Windows.Forms.LinkLabel
        Me.cmbApplyClassify = New System.Windows.Forms.ComboBox
        Me.cmbApplyArea = New System.Windows.Forms.ComboBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.grpExecutive = New System.Windows.Forms.GroupBox
        Me.flxUnionInformation = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.flxExecutive = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.btnDelete = New System.Windows.Forms.Button
        Me.btnExeComUpdate = New System.Windows.Forms.Button
        Me.btnExeComInsert = New System.Windows.Forms.Button
        Me.btnOkHeto = New System.Windows.Forms.Button
        Me.lblSHIBU = New System.Windows.Forms.Label
        Me.lblKind = New System.Windows.Forms.Label
        Me.grpApplyArea.SuspendLayout()
        Me.grpExecutive.SuspendLayout()
        CType(Me.flxUnionInformation, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.flxExecutive, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(313, 500)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 18
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOk
        '
        Me.btnOk.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOk.Location = New System.Drawing.Point(182, 500)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(110, 30)
        Me.btnOk.TabIndex = 4
        Me.btnOk.Text = "OK"
        Me.btnOk.UseVisualStyleBackColor = True
        '
        'grpApplyArea
        '
        Me.grpApplyArea.Controls.Add(Me.lblKind2)
        Me.grpApplyArea.Controls.Add(Me.lklMemo)
        Me.grpApplyArea.Controls.Add(Me.cmbApplyClassify)
        Me.grpApplyArea.Controls.Add(Me.cmbApplyArea)
        Me.grpApplyArea.Controls.Add(Me.Label6)
        Me.grpApplyArea.Controls.Add(Me.Label5)
        Me.grpApplyArea.Controls.Add(Me.Label2)
        Me.grpApplyArea.Controls.Add(Me.Label7)
        Me.grpApplyArea.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpApplyArea.Location = New System.Drawing.Point(15, 6)
        Me.grpApplyArea.Name = "grpApplyArea"
        Me.grpApplyArea.Size = New System.Drawing.Size(575, 95)
        Me.grpApplyArea.TabIndex = 0
        Me.grpApplyArea.TabStop = False
        Me.grpApplyArea.Text = "支部と種類の選択"
        '
        'lblKind2
        '
        Me.lblKind2.AutoSize = True
        Me.lblKind2.Location = New System.Drawing.Point(454, 63)
        Me.lblKind2.Name = "lblKind2"
        Me.lblKind2.Size = New System.Drawing.Size(0, 16)
        Me.lblKind2.TabIndex = 18
        '
        'lklMemo
        '
        Me.lklMemo.AutoSize = True
        Me.lklMemo.Location = New System.Drawing.Point(454, 25)
        Me.lklMemo.Name = "lklMemo"
        Me.lklMemo.Size = New System.Drawing.Size(84, 16)
        Me.lklMemo.TabIndex = 20
        Me.lklMemo.TabStop = True
        Me.lklMemo.Text = "覚書を表示"
        '
        'cmbApplyClassify
        '
        Me.cmbApplyClassify.FormattingEnabled = True
        Me.cmbApplyClassify.Location = New System.Drawing.Point(97, 60)
        Me.cmbApplyClassify.Name = "cmbApplyClassify"
        Me.cmbApplyClassify.Size = New System.Drawing.Size(351, 24)
        Me.cmbApplyClassify.TabIndex = 2
        Me.cmbApplyClassify.Tag = "種類"
        '
        'cmbApplyArea
        '
        Me.cmbApplyArea.FormattingEnabled = True
        Me.cmbApplyArea.Location = New System.Drawing.Point(97, 22)
        Me.cmbApplyArea.Name = "cmbApplyArea"
        Me.cmbApplyArea.Size = New System.Drawing.Size(121, 24)
        Me.cmbApplyArea.TabIndex = 1
        Me.cmbApplyArea.Tag = "支部"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(52, 63)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(40, 16)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "種類"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(34, 63)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(16, 16)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "*"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(52, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 16)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "支部"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.ForeColor = System.Drawing.Color.Red
        Me.Label7.Location = New System.Drawing.Point(34, 26)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(16, 16)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "*"
        '
        'grpExecutive
        '
        Me.grpExecutive.Controls.Add(Me.flxUnionInformation)
        Me.grpExecutive.Controls.Add(Me.flxExecutive)
        Me.grpExecutive.Controls.Add(Me.btnDelete)
        Me.grpExecutive.Controls.Add(Me.btnExeComUpdate)
        Me.grpExecutive.Controls.Add(Me.btnExeComInsert)
        Me.grpExecutive.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpExecutive.Location = New System.Drawing.Point(15, 107)
        Me.grpExecutive.Name = "grpExecutive"
        Me.grpExecutive.Size = New System.Drawing.Size(575, 387)
        Me.grpExecutive.TabIndex = 0
        Me.grpExecutive.TabStop = False
        Me.grpExecutive.Text = "該当する中央執行・中央委員会の日程を選択"
        '
        'flxUnionInformation
        '
        Me.flxUnionInformation.ColumnInfo = resources.GetString("flxUnionInformation.ColumnInfo")
        Me.flxUnionInformation.Location = New System.Drawing.Point(8, 22)
        Me.flxUnionInformation.Name = "flxUnionInformation"
        Me.flxUnionInformation.Rows.DefaultSize = 22
        Me.flxUnionInformation.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
        Me.flxUnionInformation.Size = New System.Drawing.Size(568, 362)
        Me.flxUnionInformation.StyleInfo = resources.GetString("flxUnionInformation.StyleInfo")
        Me.flxUnionInformation.TabIndex = 18
        '
        'flxExecutive
        '
        Me.flxExecutive.ColumnInfo = resources.GetString("flxExecutive.ColumnInfo")
        Me.flxExecutive.Location = New System.Drawing.Point(9, 22)
        Me.flxExecutive.Name = "flxExecutive"
        Me.flxExecutive.Rows.DefaultSize = 22
        Me.flxExecutive.Size = New System.Drawing.Size(568, 324)
        Me.flxExecutive.StyleInfo = resources.GetString("flxExecutive.StyleInfo")
        Me.flxExecutive.TabIndex = 17
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(343, 351)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(92, 24)
        Me.btnDelete.TabIndex = 16
        Me.btnDelete.Text = "削除"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnExeComUpdate
        '
        Me.btnExeComUpdate.Location = New System.Drawing.Point(245, 351)
        Me.btnExeComUpdate.Name = "btnExeComUpdate"
        Me.btnExeComUpdate.Size = New System.Drawing.Size(92, 24)
        Me.btnExeComUpdate.TabIndex = 15
        Me.btnExeComUpdate.Text = "内容変更"
        Me.btnExeComUpdate.UseVisualStyleBackColor = True
        '
        'btnExeComInsert
        '
        Me.btnExeComInsert.Location = New System.Drawing.Point(147, 351)
        Me.btnExeComInsert.Name = "btnExeComInsert"
        Me.btnExeComInsert.Size = New System.Drawing.Size(92, 24)
        Me.btnExeComInsert.TabIndex = 14
        Me.btnExeComInsert.Text = "新規登録"
        Me.btnExeComInsert.UseVisualStyleBackColor = True
        '
        'btnOkHeto
        '
        Me.btnOkHeto.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOkHeto.Location = New System.Drawing.Point(182, 500)
        Me.btnOkHeto.Name = "btnOkHeto"
        Me.btnOkHeto.Size = New System.Drawing.Size(110, 30)
        Me.btnOkHeto.TabIndex = 17
        Me.btnOkHeto.Text = "OK"
        Me.btnOkHeto.UseVisualStyleBackColor = True
        '
        'lblSHIBU
        '
        Me.lblSHIBU.AutoSize = True
        Me.lblSHIBU.Location = New System.Drawing.Point(561, 518)
        Me.lblSHIBU.Name = "lblSHIBU"
        Me.lblSHIBU.Size = New System.Drawing.Size(11, 12)
        Me.lblSHIBU.TabIndex = 0
        Me.lblSHIBU.Text = "1"
        Me.lblSHIBU.Visible = False
        '
        'lblKind
        '
        Me.lblKind.AutoSize = True
        Me.lblKind.Location = New System.Drawing.Point(578, 518)
        Me.lblKind.Name = "lblKind"
        Me.lblKind.Size = New System.Drawing.Size(11, 12)
        Me.lblKind.TabIndex = 0
        Me.lblKind.Text = "3"
        Me.lblKind.Visible = False
        '
        'FM040303
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(604, 542)
        Me.Controls.Add(Me.lblKind)
        Me.Controls.Add(Me.lblSHIBU)
        Me.Controls.Add(Me.btnOkHeto)
        Me.Controls.Add(Me.grpExecutive)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.grpApplyArea)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(610, 576)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(610, 189)
        Me.Name = "FM040303"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "種類の選択"
        Me.grpApplyArea.ResumeLayout(False)
        Me.grpApplyArea.PerformLayout()
        Me.grpExecutive.ResumeLayout(False)
        CType(Me.flxUnionInformation, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.flxExecutive, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents grpApplyArea As System.Windows.Forms.GroupBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lklMemo As System.Windows.Forms.LinkLabel
    Friend WithEvents cmbApplyClassify As System.Windows.Forms.ComboBox
    Friend WithEvents cmbApplyArea As System.Windows.Forms.ComboBox
    Friend WithEvents grpExecutive As System.Windows.Forms.GroupBox
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnExeComUpdate As System.Windows.Forms.Button
    Friend WithEvents btnExeComInsert As System.Windows.Forms.Button
    Friend WithEvents lblKind2 As System.Windows.Forms.Label
    Friend WithEvents btnOkHeto As System.Windows.Forms.Button
    Friend WithEvents lblSHIBU As System.Windows.Forms.Label
    Friend WithEvents lblKind As System.Windows.Forms.Label
    Friend WithEvents flxExecutive As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents flxUnionInformation As C1.Win.C1FlexGrid.C1FlexGrid
End Class
