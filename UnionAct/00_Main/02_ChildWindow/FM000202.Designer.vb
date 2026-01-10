<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM000202
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
        Me.gbxResult = New System.Windows.Forms.GroupBox
        Me.dgdResultSQL = New System.Windows.Forms.DataGridView
        Me.btnDecide = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.gbxResult.SuspendLayout()
        CType(Me.dgdResultSQL, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'gbxResult
        '
        Me.gbxResult.Controls.Add(Me.dgdResultSQL)
        Me.gbxResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.gbxResult.Location = New System.Drawing.Point(24, 12)
        Me.gbxResult.Name = "gbxResult"
        Me.gbxResult.Size = New System.Drawing.Size(732, 562)
        Me.gbxResult.TabIndex = 0
        Me.gbxResult.TabStop = False
        Me.gbxResult.Text = "検索結果（ 件）"
        '
        'dgdResultSQL
        '
        Me.dgdResultSQL.AllowUserToAddRows = False
        Me.dgdResultSQL.AllowUserToDeleteRows = False
        Me.dgdResultSQL.AllowUserToResizeRows = False
        Me.dgdResultSQL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgdResultSQL.Location = New System.Drawing.Point(18, 34)
        Me.dgdResultSQL.MultiSelect = False
        Me.dgdResultSQL.Name = "dgdResultSQL"
        Me.dgdResultSQL.ReadOnly = True
        Me.dgdResultSQL.RowHeadersVisible = False
        Me.dgdResultSQL.RowTemplate.Height = 21
        Me.dgdResultSQL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdResultSQL.Size = New System.Drawing.Size(697, 509)
        Me.dgdResultSQL.TabIndex = 0
        '
        'btnDecide
        '
        Me.btnDecide.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDecide.Location = New System.Drawing.Point(254, 589)
        Me.btnDecide.Name = "btnDecide"
        Me.btnDecide.Size = New System.Drawing.Size(110, 30)
        Me.btnDecide.TabIndex = 1
        Me.btnDecide.Text = "決定"
        Me.btnDecide.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(406, 589)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 2
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'FM000202
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(781, 640)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDecide)
        Me.Controls.Add(Me.gbxResult)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM000202"
        Me.Text = "検索結果"
        Me.gbxResult.ResumeLayout(False)
        CType(Me.dgdResultSQL, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents gbxResult As System.Windows.Forms.GroupBox
    Friend WithEvents btnDecide As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents dgdResultSQL As System.Windows.Forms.DataGridView
End Class
