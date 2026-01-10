<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC060101
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
        Me.Label11 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.dtpDate = New System.Windows.Forms.DateTimePicker
        Me.clbKind = New System.Windows.Forms.CheckedListBox
        Me.cmbBelong = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnPrintOnlyName = New System.Windows.Forms.Button
        Me.btnPrintIDAndName = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label11.Location = New System.Drawing.Point(200, 20)
        Me.Label11.MinimumSize = New System.Drawing.Size(630, 35)
        Me.Label11.Name = "Label11"
        Me.Label11.Padding = New System.Windows.Forms.Padding(12, 0, 12, 0)
        Me.Label11.Size = New System.Drawing.Size(630, 35)
        Me.Label11.TabIndex = 63
        Me.Label11.Text = "選挙人名簿"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.dtpDate)
        Me.GroupBox1.Controls.Add(Me.clbKind)
        Me.GroupBox1.Controls.Add(Me.cmbBelong)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(112, 155)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(800, 332)
        Me.GroupBox1.TabIndex = 65
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "抽出条件"
        '
        'dtpDate
        '
        Me.dtpDate.Location = New System.Drawing.Point(226, 115)
        Me.dtpDate.Name = "dtpDate"
        Me.dtpDate.Size = New System.Drawing.Size(142, 23)
        Me.dtpDate.TabIndex = 1
        '
        'clbKind
        '
        Me.clbKind.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.clbKind.FormattingEnabled = True
        Me.clbKind.Items.AddRange(New Object() {"正組合員", "シニア組合員", "永年組合員", "準組合員", "非組合員", "脱退組合員", "その他"})
        Me.clbKind.Location = New System.Drawing.Point(497, 116)
        Me.clbKind.Name = "clbKind"
        Me.clbKind.Size = New System.Drawing.Size(133, 166)
        Me.clbKind.TabIndex = 3
        '
        'cmbBelong
        '
        Me.cmbBelong.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbBelong.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbBelong.FormattingEnabled = True
        Me.cmbBelong.Location = New System.Drawing.Point(226, 192)
        Me.cmbBelong.Name = "cmbBelong"
        Me.cmbBelong.Size = New System.Drawing.Size(142, 24)
        Me.cmbBelong.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(494, 93)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 16)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "組合員種別"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(149, 195)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "組合支部"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.Color.Red
        Me.Label5.Location = New System.Drawing.Point(129, 195)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(16, 16)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "*"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(129, 119)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(16, 16)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(149, 119)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "適用日付"
        '
        'btnPrintOnlyName
        '
        Me.btnPrintOnlyName.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrintOnlyName.Location = New System.Drawing.Point(554, 527)
        Me.btnPrintOnlyName.Name = "btnPrintOnlyName"
        Me.btnPrintOnlyName.Size = New System.Drawing.Size(140, 30)
        Me.btnPrintOnlyName.TabIndex = 5
        Me.btnPrintOnlyName.Text = "名前のみ印刷"
        Me.btnPrintOnlyName.UseVisualStyleBackColor = True
        '
        'btnPrintIDAndName
        '
        Me.btnPrintIDAndName.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrintIDAndName.Location = New System.Drawing.Point(299, 527)
        Me.btnPrintIDAndName.Name = "btnPrintIDAndName"
        Me.btnPrintIDAndName.Size = New System.Drawing.Size(140, 30)
        Me.btnPrintIDAndName.TabIndex = 4
        Me.btnPrintIDAndName.Text = "社員番号付き印刷"
        Me.btnPrintIDAndName.UseVisualStyleBackColor = True
        '
        'UC060101
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnPrintOnlyName)
        Me.Controls.Add(Me.btnPrintIDAndName)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label11)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Name = "UC060101"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnPrintOnlyName As System.Windows.Forms.Button
    Friend WithEvents btnPrintIDAndName As System.Windows.Forms.Button
    Friend WithEvents clbKind As System.Windows.Forms.CheckedListBox
    Friend WithEvents cmbBelong As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dtpDate As System.Windows.Forms.DateTimePicker

End Class
