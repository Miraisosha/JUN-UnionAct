<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM000104
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.btnOk = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.lblMemo = New System.Windows.Forms.Label()
        Me.dgvResult = New System.Windows.Forms.DataGridView()
        Me.PeriodNo = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CommitteeName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PostName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Term = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.CommitteeId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.PostId = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnOk
        '
        Me.btnOk.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnOk.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOk.Location = New System.Drawing.Point(139, 321)
        Me.btnOk.Name = "btnOk"
        Me.btnOk.Size = New System.Drawing.Size(110, 30)
        Me.btnOk.TabIndex = 1
        Me.btnOk.Text = "OK"
        '
        'btnCancel
        '
        Me.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(291, 321)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "キャンセル"
        '
        'lblMemo
        '
        Me.lblMemo.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMemo.Location = New System.Drawing.Point(9, 9)
        Me.lblMemo.Name = "lblMemo"
        Me.lblMemo.Size = New System.Drawing.Size(458, 46)
        Me.lblMemo.TabIndex = 3
        Me.lblMemo.Text = "現在、複数の委員会に所属されています。委員会によって操作権限が異なりますので、操作したい委員会を選択してください。"
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
        Me.dgvResult.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.PeriodNo, Me.CommitteeName, Me.PostName, Me.Term, Me.CommitteeId, Me.PostId})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvResult.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvResult.Location = New System.Drawing.Point(26, 58)
        Me.dgvResult.MultiSelect = False
        Me.dgvResult.Name = "dgvResult"
        Me.dgvResult.ReadOnly = True
        Me.dgvResult.RowHeadersVisible = False
        Me.dgvResult.RowTemplate.Height = 21
        Me.dgvResult.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvResult.Size = New System.Drawing.Size(484, 257)
        Me.dgvResult.TabIndex = 4
        '
        'PeriodNo
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopCenter
        Me.PeriodNo.DefaultCellStyle = DataGridViewCellStyle2
        Me.PeriodNo.HeaderText = "期"
        Me.PeriodNo.Name = "PeriodNo"
        Me.PeriodNo.ReadOnly = True
        Me.PeriodNo.Width = 30
        '
        'CommitteeName
        '
        Me.CommitteeName.HeaderText = "委員会名称"
        Me.CommitteeName.Name = "CommitteeName"
        Me.CommitteeName.ReadOnly = True
        Me.CommitteeName.Width = 175
        '
        'PostName
        '
        Me.PostName.HeaderText = "役職"
        Me.PostName.Name = "PostName"
        Me.PostName.ReadOnly = True
        Me.PostName.Width = 125
        '
        'Term
        '
        Me.Term.HeaderText = "期間"
        Me.Term.Name = "Term"
        Me.Term.ReadOnly = True
        Me.Term.Width = 185
        '
        'CommitteeId
        '
        Me.CommitteeId.HeaderText = "委員会ID"
        Me.CommitteeId.Name = "CommitteeId"
        Me.CommitteeId.ReadOnly = True
        Me.CommitteeId.Visible = False
        '
        'PostId
        '
        Me.PostId.HeaderText = "役職ID"
        Me.PostId.Name = "PostId"
        Me.PostId.ReadOnly = True
        Me.PostId.Visible = False
        '
        'FM000104
        '
        Me.AcceptButton = Me.btnOk
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.btnCancel
        Me.ClientSize = New System.Drawing.Size(540, 363)
        Me.Controls.Add(Me.dgvResult)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOk)
        Me.Controls.Add(Me.lblMemo)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM000104"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "委員会を選択してください"
        CType(Me.dgvResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnOk As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblMemo As System.Windows.Forms.Label
    Friend WithEvents dgvResult As System.Windows.Forms.DataGridView
    Friend WithEvents PeriodNo As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CommitteeName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PostName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Term As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents CommitteeId As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents PostId As System.Windows.Forms.DataGridViewTextBoxColumn

End Class
