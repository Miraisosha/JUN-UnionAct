<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM010104
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
        Me.gbHistoryList = New System.Windows.Forms.GroupBox
        Me.dgdHistoryList = New System.Windows.Forms.DataGridView
        Me.gbDirectSpecify = New System.Windows.Forms.GroupBox
        Me.dtpSpecifyTime = New System.Windows.Forms.DateTimePicker
        Me.chkDirectSpecify = New System.Windows.Forms.CheckBox
        Me.btnOK = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblInsertMessage = New System.Windows.Forms.Label
        Me.gbHistoryList.SuspendLayout()
        CType(Me.dgdHistoryList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.gbDirectSpecify.SuspendLayout()
        Me.SuspendLayout()
        '
        'gbHistoryList
        '
        Me.gbHistoryList.Controls.Add(Me.dgdHistoryList)
        Me.gbHistoryList.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.gbHistoryList.Location = New System.Drawing.Point(37, 15)
        Me.gbHistoryList.Name = "gbHistoryList"
        Me.gbHistoryList.Size = New System.Drawing.Size(408, 322)
        Me.gbHistoryList.TabIndex = 0
        Me.gbHistoryList.TabStop = False
        Me.gbHistoryList.Text = "履歴一覧より選択"
        '
        'dgdHistoryList
        '
        Me.dgdHistoryList.AllowUserToAddRows = False
        Me.dgdHistoryList.AllowUserToDeleteRows = False
        Me.dgdHistoryList.AllowUserToResizeRows = False
        Me.dgdHistoryList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgdHistoryList.Location = New System.Drawing.Point(6, 22)
        Me.dgdHistoryList.MultiSelect = False
        Me.dgdHistoryList.Name = "dgdHistoryList"
        Me.dgdHistoryList.ReadOnly = True
        Me.dgdHistoryList.RowHeadersVisible = False
        Me.dgdHistoryList.RowTemplate.Height = 21
        Me.dgdHistoryList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdHistoryList.Size = New System.Drawing.Size(382, 272)
        Me.dgdHistoryList.TabIndex = 1
        '
        'gbDirectSpecify
        '
        Me.gbDirectSpecify.Controls.Add(Me.dtpSpecifyTime)
        Me.gbDirectSpecify.Controls.Add(Me.chkDirectSpecify)
        Me.gbDirectSpecify.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.gbDirectSpecify.Location = New System.Drawing.Point(37, 356)
        Me.gbDirectSpecify.Name = "gbDirectSpecify"
        Me.gbDirectSpecify.Size = New System.Drawing.Size(408, 73)
        Me.gbDirectSpecify.TabIndex = 2
        Me.gbDirectSpecify.TabStop = False
        Me.gbDirectSpecify.Text = "直接指定して選択"
        '
        'dtpSpecifyTime
        '
        Me.dtpSpecifyTime.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.dtpSpecifyTime.Location = New System.Drawing.Point(184, 25)
        Me.dtpSpecifyTime.Name = "dtpSpecifyTime"
        Me.dtpSpecifyTime.Size = New System.Drawing.Size(172, 23)
        Me.dtpSpecifyTime.TabIndex = 3
        '
        'chkDirectSpecify
        '
        Me.chkDirectSpecify.AutoSize = True
        Me.chkDirectSpecify.Location = New System.Drawing.Point(74, 28)
        Me.chkDirectSpecify.Name = "chkDirectSpecify"
        Me.chkDirectSpecify.Size = New System.Drawing.Size(91, 20)
        Me.chkDirectSpecify.TabIndex = 2
        Me.chkDirectSpecify.Text = "直接指定"
        Me.chkDirectSpecify.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOK.Location = New System.Drawing.Point(123, 486)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(110, 30)
        Me.btnOK.TabIndex = 4
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(247, 486)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblInsertMessage
        '
        Me.lblInsertMessage.AutoSize = True
        Me.lblInsertMessage.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblInsertMessage.ForeColor = System.Drawing.Color.Red
        Me.lblInsertMessage.Location = New System.Drawing.Point(130, 448)
        Me.lblInsertMessage.Name = "lblInsertMessage"
        Me.lblInsertMessage.Size = New System.Drawing.Size(218, 16)
        Me.lblInsertMessage.TabIndex = 4
        Me.lblInsertMessage.Text = "※加入年月日を入力してください"
        Me.lblInsertMessage.Visible = False
        '
        'FM010104
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(482, 538)
        Me.Controls.Add(Me.lblInsertMessage)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.gbDirectSpecify)
        Me.Controls.Add(Me.gbHistoryList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM010104"
        Me.Text = "基本情報履歴 - 適用日付選択画面"
        Me.gbHistoryList.ResumeLayout(False)
        CType(Me.dgdHistoryList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.gbDirectSpecify.ResumeLayout(False)
        Me.gbDirectSpecify.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents gbHistoryList As System.Windows.Forms.GroupBox
    Friend WithEvents dgdHistoryList As System.Windows.Forms.DataGridView
    Friend WithEvents gbDirectSpecify As System.Windows.Forms.GroupBox
    Friend WithEvents dtpSpecifyTime As System.Windows.Forms.DateTimePicker
    Friend WithEvents chkDirectSpecify As System.Windows.Forms.CheckBox
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblInsertMessage As System.Windows.Forms.Label
End Class
