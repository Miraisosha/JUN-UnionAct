<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC040101
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC040101))
        Me.lblTile = New System.Windows.Forms.Label
        Me.grpInsert = New System.Windows.Forms.GroupBox
        Me.btnInsert = New System.Windows.Forms.Button
        Me.grpRef = New System.Windows.Forms.GroupBox
        Me.btnSearch = New System.Windows.Forms.Button
        Me.lblMonth = New System.Windows.Forms.Label
        Me.lblKind = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.Label
        Me.cboYear = New System.Windows.Forms.ComboBox
        Me.cboKind = New System.Windows.Forms.ComboBox
        Me.cboMonth = New System.Windows.Forms.ComboBox
        Me.lblStart = New System.Windows.Forms.Label
        Me.grpResult = New System.Windows.Forms.GroupBox
        Me.btnStop = New System.Windows.Forms.Button
        Me.btnUpdate = New System.Windows.Forms.Button
        Me.btnDetail = New System.Windows.Forms.Button
        Me.cfgResult = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.grpInsert.SuspendLayout()
        Me.grpRef.SuspendLayout()
        Me.grpResult.SuspendLayout()
        CType(Me.cfgResult, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTile
        '
        Me.lblTile.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTile.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTile.Font = New System.Drawing.Font("MS UI Gothic", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTile.Location = New System.Drawing.Point(187, 14)
        Me.lblTile.Name = "lblTile"
        Me.lblTile.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTile.Size = New System.Drawing.Size(620, 35)
        Me.lblTile.TabIndex = 8
        Me.lblTile.Text = "組合大会通知"
        Me.lblTile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpInsert
        '
        Me.grpInsert.Controls.Add(Me.btnInsert)
        Me.grpInsert.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.grpInsert.Location = New System.Drawing.Point(59, 737)
        Me.grpInsert.Name = "grpInsert"
        Me.grpInsert.Size = New System.Drawing.Size(912, 50)
        Me.grpInsert.TabIndex = 13
        Me.grpInsert.TabStop = False
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsert.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnInsert.Location = New System.Drawing.Point(381, 14)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(110, 30)
        Me.btnInsert.TabIndex = 8
        Me.btnInsert.Text = "開催登録"
        Me.btnInsert.UseVisualStyleBackColor = True
        '
        'grpRef
        '
        Me.grpRef.Controls.Add(Me.btnSearch)
        Me.grpRef.Controls.Add(Me.lblMonth)
        Me.grpRef.Controls.Add(Me.lblKind)
        Me.grpRef.Controls.Add(Me.lblYear)
        Me.grpRef.Controls.Add(Me.cboYear)
        Me.grpRef.Controls.Add(Me.cboKind)
        Me.grpRef.Controls.Add(Me.cboMonth)
        Me.grpRef.Controls.Add(Me.lblStart)
        Me.grpRef.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.grpRef.ForeColor = System.Drawing.Color.Blue
        Me.grpRef.Location = New System.Drawing.Point(59, 115)
        Me.grpRef.Name = "grpRef"
        Me.grpRef.Size = New System.Drawing.Size(912, 65)
        Me.grpRef.TabIndex = 14
        Me.grpRef.TabStop = False
        Me.grpRef.Text = "検索条件"
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(777, 23)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'lblMonth
        '
        Me.lblMonth.AutoSize = True
        Me.lblMonth.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblMonth.Location = New System.Drawing.Point(430, 30)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.Size = New System.Drawing.Size(24, 16)
        Me.lblMonth.TabIndex = 10
        Me.lblMonth.Text = "月"
        '
        'lblKind
        '
        Me.lblKind.AutoSize = True
        Me.lblKind.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblKind.Location = New System.Drawing.Point(588, 30)
        Me.lblKind.Name = "lblKind"
        Me.lblKind.Size = New System.Drawing.Size(40, 16)
        Me.lblKind.TabIndex = 5
        Me.lblKind.Text = "種別"
        '
        'lblYear
        '
        Me.lblYear.AutoSize = True
        Me.lblYear.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblYear.Location = New System.Drawing.Point(339, 30)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.Size = New System.Drawing.Size(24, 16)
        Me.lblYear.TabIndex = 9
        Me.lblYear.Text = "年"
        '
        'cboYear
        '
        Me.cboYear.FormattingEnabled = True
        Me.cboYear.Location = New System.Drawing.Point(256, 27)
        Me.cboYear.Name = "cboYear"
        Me.cboYear.Size = New System.Drawing.Size(76, 24)
        Me.cboYear.TabIndex = 1
        '
        'cboKind
        '
        Me.cboKind.FormattingEnabled = True
        Me.cboKind.Location = New System.Drawing.Point(634, 27)
        Me.cboKind.Name = "cboKind"
        Me.cboKind.Size = New System.Drawing.Size(125, 24)
        Me.cboKind.TabIndex = 3
        '
        'cboMonth
        '
        Me.cboMonth.FormattingEnabled = True
        Me.cboMonth.Location = New System.Drawing.Point(369, 27)
        Me.cboMonth.Name = "cboMonth"
        Me.cboMonth.Size = New System.Drawing.Size(54, 24)
        Me.cboMonth.TabIndex = 2
        '
        'lblStart
        '
        Me.lblStart.AutoSize = True
        Me.lblStart.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.lblStart.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblStart.Location = New System.Drawing.Point(146, 30)
        Me.lblStart.Name = "lblStart"
        Me.lblStart.Size = New System.Drawing.Size(104, 16)
        Me.lblStart.TabIndex = 1
        Me.lblStart.Text = "開催開始日付"
        '
        'grpResult
        '
        Me.grpResult.Controls.Add(Me.btnStop)
        Me.grpResult.Controls.Add(Me.btnUpdate)
        Me.grpResult.Controls.Add(Me.btnDetail)
        Me.grpResult.Controls.Add(Me.cfgResult)
        Me.grpResult.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpResult.ForeColor = System.Drawing.Color.Blue
        Me.grpResult.Location = New System.Drawing.Point(59, 202)
        Me.grpResult.Name = "grpResult"
        Me.grpResult.Size = New System.Drawing.Size(912, 529)
        Me.grpResult.TabIndex = 18
        Me.grpResult.TabStop = False
        Me.grpResult.Text = "検索結果（ xx 件）"
        Me.grpResult.Visible = False
        '
        'btnStop
        '
        Me.btnStop.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnStop.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnStop.Location = New System.Drawing.Point(777, 493)
        Me.btnStop.Name = "btnStop"
        Me.btnStop.Size = New System.Drawing.Size(110, 30)
        Me.btnStop.TabIndex = 7
        Me.btnStop.Text = "中止"
        Me.btnStop.UseVisualStyleBackColor = True
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnUpdate.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnUpdate.Location = New System.Drawing.Point(649, 493)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(110, 30)
        Me.btnUpdate.TabIndex = 6
        Me.btnUpdate.Text = "内容変更"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDetail
        '
        Me.btnDetail.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnDetail.Location = New System.Drawing.Point(381, 493)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(110, 30)
        Me.btnDetail.TabIndex = 5
        Me.btnDetail.Text = "詳細"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'cfgResult
        '
        Me.cfgResult.ColumnInfo = resources.GetString("cfgResult.ColumnInfo")
        Me.cfgResult.Location = New System.Drawing.Point(27, 31)
        Me.cfgResult.Name = "cfgResult"
        Me.cfgResult.Rows.Count = 1
        Me.cfgResult.Rows.DefaultSize = 22
        Me.cfgResult.Size = New System.Drawing.Size(860, 456)
        Me.cfgResult.StyleInfo = resources.GetString("cfgResult.StyleInfo")
        Me.cfgResult.TabIndex = 8
        '
        'UC040101
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.grpResult)
        Me.Controls.Add(Me.grpRef)
        Me.Controls.Add(Me.grpInsert)
        Me.Controls.Add(Me.lblTile)
        Me.Name = "UC040101"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpInsert.ResumeLayout(False)
        Me.grpRef.ResumeLayout(False)
        Me.grpRef.PerformLayout()
        Me.grpResult.ResumeLayout(False)
        CType(Me.cfgResult, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblTile As System.Windows.Forms.Label
    Friend WithEvents grpInsert As System.Windows.Forms.GroupBox
    Friend WithEvents btnInsert As System.Windows.Forms.Button
    Friend WithEvents grpRef As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents lblMonth As System.Windows.Forms.Label
    Friend WithEvents lblKind As System.Windows.Forms.Label
    Friend WithEvents lblYear As System.Windows.Forms.Label
    Friend WithEvents cboYear As System.Windows.Forms.ComboBox
    Friend WithEvents cboKind As System.Windows.Forms.ComboBox
    Friend WithEvents cboMonth As System.Windows.Forms.ComboBox
    Friend WithEvents lblStart As System.Windows.Forms.Label
    Friend WithEvents grpResult As System.Windows.Forms.GroupBox
    Friend WithEvents cfgResult As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnStop As System.Windows.Forms.Button
    Friend WithEvents btnUpdate As System.Windows.Forms.Button
    Friend WithEvents btnDetail As System.Windows.Forms.Button

End Class
