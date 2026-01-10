<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM020104
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
        Me.lblNotice = New System.Windows.Forms.Label
        Me.lblNotice2 = New System.Windows.Forms.Label
        Me.grpDeleteMember = New System.Windows.Forms.GroupBox
        Me.dgdDelMemberList = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column5 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column6 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.btnClose = New System.Windows.Forms.Button
        Me.grpDeleteMember.SuspendLayout()
        CType(Me.dgdDelMemberList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblNotice
        '
        Me.lblNotice.AutoSize = True
        Me.lblNotice.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNotice.Location = New System.Drawing.Point(9, 10)
        Me.lblNotice.Name = "lblNotice"
        Me.lblNotice.Size = New System.Drawing.Size(349, 16)
        Me.lblNotice.TabIndex = 1
        Me.lblNotice.Text = "委員会名簿から削除された組合員を表示しています。"
        '
        'lblNotice2
        '
        Me.lblNotice2.AutoSize = True
        Me.lblNotice2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblNotice2.Location = New System.Drawing.Point(9, 29)
        Me.lblNotice2.Name = "lblNotice2"
        Me.lblNotice2.Size = New System.Drawing.Size(504, 16)
        Me.lblNotice2.TabIndex = 2
        Me.lblNotice2.Text = "※現時点ではデータは確定していません。登録確認ボタンを押して確定します。"
        '
        'grpDeleteMember
        '
        Me.grpDeleteMember.Controls.Add(Me.dgdDelMemberList)
        Me.grpDeleteMember.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpDeleteMember.Location = New System.Drawing.Point(12, 66)
        Me.grpDeleteMember.Name = "grpDeleteMember"
        Me.grpDeleteMember.Size = New System.Drawing.Size(501, 430)
        Me.grpDeleteMember.TabIndex = 3
        Me.grpDeleteMember.TabStop = False
        Me.grpDeleteMember.Text = "委員会名簿　削除者(XX件)"
        '
        'dgdDelMemberList
        '
        Me.dgdDelMemberList.AllowUserToAddRows = False
        Me.dgdDelMemberList.AllowUserToDeleteRows = False
        Me.dgdDelMemberList.AllowUserToResizeRows = False
        Me.dgdDelMemberList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgdDelMemberList.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2, Me.Column3, Me.Column4, Me.Column5, Me.Column6})
        Me.dgdDelMemberList.Location = New System.Drawing.Point(6, 22)
        Me.dgdDelMemberList.MultiSelect = False
        Me.dgdDelMemberList.Name = "dgdDelMemberList"
        Me.dgdDelMemberList.ReadOnly = True
        Me.dgdDelMemberList.RowHeadersVisible = False
        Me.dgdDelMemberList.RowTemplate.Height = 21
        Me.dgdDelMemberList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdDelMemberList.Size = New System.Drawing.Size(489, 402)
        Me.dgdDelMemberList.StandardTab = True
        Me.dgdDelMemberList.TabIndex = 1
        '
        'Column1
        '
        Me.Column1.HeaderText = "役職"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.HeaderText = "名前"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        '
        'Column3
        '
        Me.Column3.HeaderText = "社員番号"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        '
        'Column4
        '
        Me.Column4.FillWeight = 50.0!
        Me.Column4.HeaderText = "機種"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        '
        'Column5
        '
        Me.Column5.HeaderText = "資格"
        Me.Column5.Name = "Column5"
        Me.Column5.ReadOnly = True
        '
        'Column6
        '
        Me.Column6.HeaderText = "組合支部"
        Me.Column6.Name = "Column6"
        Me.Column6.ReadOnly = True
        '
        'btnClose
        '
        Me.btnClose.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnClose.Location = New System.Drawing.Point(209, 505)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(110, 30)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "閉じる"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'FM020104
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(525, 544)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.grpDeleteMember)
        Me.Controls.Add(Me.lblNotice2)
        Me.Controls.Add(Me.lblNotice)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM020104"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "委員会名簿 削除者一覧"
        Me.grpDeleteMember.ResumeLayout(False)
        CType(Me.dgdDelMemberList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblNotice As System.Windows.Forms.Label
    Friend WithEvents lblNotice2 As System.Windows.Forms.Label
    Friend WithEvents grpDeleteMember As System.Windows.Forms.GroupBox
    Friend WithEvents dgdDelMemberList As System.Windows.Forms.DataGridView
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column5 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column6 As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
