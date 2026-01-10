<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC030101
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
        Me.grpCondition = New System.Windows.Forms.GroupBox
        Me.txtStafId = New System.Windows.Forms.TextBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.cboBelonging = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.grpList = New System.Windows.Forms.GroupBox
        Me.dgdResult = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column7 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column8 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column9 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column10 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column11 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.grpNewEntry = New System.Windows.Forms.GroupBox
        Me.btnPrinting = New System.Windows.Forms.Button
        Me.grpCondition.SuspendLayout()
        Me.grpList.SuspendLayout()
        CType(Me.dgdResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpNewEntry.SuspendLayout()
        Me.SuspendLayout()
        '
        'grpCondition
        '
        Me.grpCondition.Controls.Add(Me.txtStafId)
        Me.grpCondition.Controls.Add(Me.btnSearch)
        Me.grpCondition.Controls.Add(Me.Label3)
        Me.grpCondition.Controls.Add(Me.cboBelonging)
        Me.grpCondition.Controls.Add(Me.Label1)
        Me.grpCondition.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpCondition.ForeColor = System.Drawing.Color.Blue
        Me.grpCondition.Location = New System.Drawing.Point(87, 86)
        Me.grpCondition.Name = "grpCondition"
        Me.grpCondition.Size = New System.Drawing.Size(824, 65)
        Me.grpCondition.TabIndex = 11
        Me.grpCondition.TabStop = False
        Me.grpCondition.Text = "検索条件"
        '
        'txtStafId
        '
        Me.txtStafId.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtStafId.ImeMode = System.Windows.Forms.ImeMode.Off
        Me.txtStafId.Location = New System.Drawing.Point(397, 27)
        Me.txtStafId.Name = "txtStafId"
        Me.txtStafId.Size = New System.Drawing.Size(168, 23)
        Me.txtStafId.TabIndex = 1
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(633, 26)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(319, 32)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "社員番号"
        '
        'cboBelonging
        '
        Me.cboBelonging.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cboBelonging.FormattingEnabled = True
        Me.cboBelonging.Location = New System.Drawing.Point(127, 27)
        Me.cboBelonging.Name = "cboBelonging"
        Me.cboBelonging.Size = New System.Drawing.Size(120, 24)
        Me.cboBelonging.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(49, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "組合支部"
        '
        'Label11
        '
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label11.Location = New System.Drawing.Point(200, 20)
        Me.Label11.Name = "Label11"
        Me.Label11.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.Label11.Size = New System.Drawing.Size(630, 35)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "所属委員会"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpList
        '
        Me.grpList.Controls.Add(Me.dgdResult)
        Me.grpList.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpList.ForeColor = System.Drawing.Color.Blue
        Me.grpList.Location = New System.Drawing.Point(47, 198)
        Me.grpList.Name = "grpList"
        Me.grpList.Size = New System.Drawing.Size(919, 535)
        Me.grpList.TabIndex = 12
        Me.grpList.TabStop = False
        Me.grpList.Text = "検索結果（ xx 件）"
        Me.grpList.Visible = False
        '
        'dgdResult
        '
        Me.dgdResult.AllowUserToAddRows = False
        Me.dgdResult.AllowUserToDeleteRows = False
        Me.dgdResult.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgdResult.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgdResult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgdResult.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6, Me.Column7, Me.Column8, Me.Column9, Me.Column10, Me.Column11})
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgdResult.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgdResult.Location = New System.Drawing.Point(20, 33)
        Me.dgdResult.MultiSelect = False
        Me.dgdResult.Name = "dgdResult"
        Me.dgdResult.ReadOnly = True
        Me.dgdResult.RowHeadersVisible = False
        Me.dgdResult.RowTemplate.Height = 21
        Me.dgdResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdResult.Size = New System.Drawing.Size(878, 480)
        Me.dgdResult.StandardTab = True
        Me.dgdResult.TabIndex = 3
        '
        'Column1
        '
        Me.Column1.HeaderText = "社員番号"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.HeaderText = "氏名"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 150
        '
        'Column3
        '
        Me.Column3.HeaderText = "支部"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.Width = 80
        '
        'Column4
        '
        Me.Column4.HeaderText = "機種"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.Width = 70
        '
        'Column5
        '
        Me.Column5.HeaderText = "委員会名0"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        Me.Column5.Width = 150
        '
        'Column6
        '
        Me.Column6.HeaderText = "委員会名1"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        Me.Column6.Width = 150
        '
        'Column7
        '
        Me.Column7.HeaderText = "委員会名2"
        Me.Column7.Name = "Column7"
        Me.Column7.ReadOnly = True
        Me.Column7.Width = 150
        '
        'Column8
        '
        Me.Column8.HeaderText = "委員会名3"
        Me.Column8.Name = "Column8"
        Me.Column8.ReadOnly = True
        Me.Column8.Width = 150
        '
        'Column9
        '
        Me.Column9.HeaderText = "委員会名4"
        Me.Column9.Name = "Column9"
        Me.Column9.ReadOnly = True
        Me.Column9.Width = 150
        '
        'Column10
        '
        Me.Column10.HeaderText = "委員会名5"
        Me.Column10.Name = "Column10"
        Me.Column10.ReadOnly = True
        Me.Column10.Width = 150
        '
        'Column11
        '
        Me.Column11.HeaderText = "委員会名6"
        Me.Column11.Name = "Column11"
        Me.Column11.ReadOnly = True
        Me.Column11.Width = 150
        '
        'grpNewEntry
        '
        Me.grpNewEntry.Controls.Add(Me.btnPrinting)
        Me.grpNewEntry.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpNewEntry.ForeColor = System.Drawing.Color.Blue
        Me.grpNewEntry.Location = New System.Drawing.Point(47, 739)
        Me.grpNewEntry.Name = "grpNewEntry"
        Me.grpNewEntry.Size = New System.Drawing.Size(919, 54)
        Me.grpNewEntry.TabIndex = 13
        Me.grpNewEntry.TabStop = False
        Me.grpNewEntry.Visible = False
        '
        'btnPrinting
        '
        Me.btnPrinting.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrinting.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPrinting.Location = New System.Drawing.Point(416, 13)
        Me.btnPrinting.Name = "btnPrinting"
        Me.btnPrinting.Size = New System.Drawing.Size(110, 30)
        Me.btnPrinting.TabIndex = 4
        Me.btnPrinting.Text = "プレ印刷"
        Me.btnPrinting.UseVisualStyleBackColor = True
        '
        'UC030101
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpNewEntry)
        Me.Controls.Add(Me.grpList)
        Me.Controls.Add(Me.grpCondition)
        Me.Controls.Add(Me.Label11)
        Me.Name = "UC030101"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpCondition.ResumeLayout(False)
        Me.grpCondition.PerformLayout()
        Me.grpList.ResumeLayout(False)
        CType(Me.dgdResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNewEntry.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents grpCondition As System.Windows.Forms.GroupBox
    Friend WithEvents txtStafId As System.Windows.Forms.TextBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cboBelonging As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents grpList As System.Windows.Forms.GroupBox
    Friend WithEvents grpNewEntry As System.Windows.Forms.GroupBox
    Friend WithEvents btnPrinting As System.Windows.Forms.Button
    Friend WithEvents dgdResult As System.Windows.Forms.DataGridView
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column7 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column8 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column9 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column10 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column11 As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
