<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM000205
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
        Me.crvReportMain = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.grpPrintArea = New System.Windows.Forms.GroupBox
        Me.lblTo = New System.Windows.Forms.Label
        Me.lblFrom = New System.Windows.Forms.Label
        Me.nudPageTo = New System.Windows.Forms.NumericUpDown
        Me.nudPageFrom = New System.Windows.Forms.NumericUpDown
        Me.optPageSet = New System.Windows.Forms.RadioButton
        Me.optAll = New System.Windows.Forms.RadioButton
        Me.lblPrintCnt = New System.Windows.Forms.Label
        Me.nudPrintCount = New System.Windows.Forms.NumericUpDown
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnInsertPrint = New System.Windows.Forms.Button
        Me.btnInsert = New System.Windows.Forms.Button
        Me.grpPrintArea.SuspendLayout()
        CType(Me.nudPageTo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPageFrom, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.nudPrintCount, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'crvReportMain
        '
        Me.crvReportMain.ActiveViewIndex = -1
        Me.crvReportMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crvReportMain.DisplayGroupTree = False
        Me.crvReportMain.Dock = System.Windows.Forms.DockStyle.Top
        Me.crvReportMain.Location = New System.Drawing.Point(0, 0)
        Me.crvReportMain.Name = "crvReportMain"
        Me.crvReportMain.SelectionFormula = ""
        Me.crvReportMain.ShowCloseButton = False
        Me.crvReportMain.ShowExportButton = False
        Me.crvReportMain.ShowGroupTreeButton = False
        Me.crvReportMain.ShowPrintButton = False
        Me.crvReportMain.ShowRefreshButton = False
        Me.crvReportMain.ShowTextSearchButton = False
        Me.crvReportMain.Size = New System.Drawing.Size(1192, 634)
        Me.crvReportMain.TabIndex = 0
        Me.crvReportMain.ViewTimeSelectionFormula = ""
        '
        'grpPrintArea
        '
        Me.grpPrintArea.Controls.Add(Me.lblTo)
        Me.grpPrintArea.Controls.Add(Me.lblFrom)
        Me.grpPrintArea.Controls.Add(Me.nudPageTo)
        Me.grpPrintArea.Controls.Add(Me.nudPageFrom)
        Me.grpPrintArea.Controls.Add(Me.optPageSet)
        Me.grpPrintArea.Controls.Add(Me.optAll)
        Me.grpPrintArea.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpPrintArea.Location = New System.Drawing.Point(35, 655)
        Me.grpPrintArea.Name = "grpPrintArea"
        Me.grpPrintArea.Size = New System.Drawing.Size(354, 86)
        Me.grpPrintArea.TabIndex = 1
        Me.grpPrintArea.TabStop = False
        Me.grpPrintArea.Text = "印刷範囲"
        Me.grpPrintArea.Visible = False
        '
        'lblTo
        '
        Me.lblTo.AutoSize = True
        Me.lblTo.Location = New System.Drawing.Point(302, 53)
        Me.lblTo.Name = "lblTo"
        Me.lblTo.Size = New System.Drawing.Size(33, 16)
        Me.lblTo.TabIndex = 3
        Me.lblTo.Text = "まで"
        '
        'lblFrom
        '
        Me.lblFrom.AutoSize = True
        Me.lblFrom.Location = New System.Drawing.Point(197, 53)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(32, 16)
        Me.lblFrom.TabIndex = 2
        Me.lblFrom.Text = "から"
        '
        'nudPageTo
        '
        Me.nudPageTo.Enabled = False
        Me.nudPageTo.Location = New System.Drawing.Point(245, 49)
        Me.nudPageTo.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudPageTo.Name = "nudPageTo"
        Me.nudPageTo.Size = New System.Drawing.Size(51, 23)
        Me.nudPageTo.TabIndex = 3
        Me.nudPageTo.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'nudPageFrom
        '
        Me.nudPageFrom.Enabled = False
        Me.nudPageFrom.Location = New System.Drawing.Point(141, 49)
        Me.nudPageFrom.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudPageFrom.Name = "nudPageFrom"
        Me.nudPageFrom.Size = New System.Drawing.Size(50, 23)
        Me.nudPageFrom.TabIndex = 2
        Me.nudPageFrom.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'optPageSet
        '
        Me.optPageSet.AutoSize = True
        Me.optPageSet.Location = New System.Drawing.Point(17, 49)
        Me.optPageSet.Name = "optPageSet"
        Me.optPageSet.Size = New System.Drawing.Size(97, 20)
        Me.optPageSet.TabIndex = 1
        Me.optPageSet.TabStop = True
        Me.optPageSet.Text = "ページ指定"
        Me.optPageSet.UseVisualStyleBackColor = True
        '
        'optAll
        '
        Me.optAll.AutoSize = True
        Me.optAll.Checked = True
        Me.optAll.Location = New System.Drawing.Point(17, 23)
        Me.optAll.Name = "optAll"
        Me.optAll.Size = New System.Drawing.Size(66, 20)
        Me.optAll.TabIndex = 0
        Me.optAll.TabStop = True
        Me.optAll.Text = "すべて"
        Me.optAll.UseVisualStyleBackColor = True
        '
        'lblPrintCnt
        '
        Me.lblPrintCnt.AutoSize = True
        Me.lblPrintCnt.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPrintCnt.Location = New System.Drawing.Point(430, 694)
        Me.lblPrintCnt.Name = "lblPrintCnt"
        Me.lblPrintCnt.Size = New System.Drawing.Size(80, 16)
        Me.lblPrintCnt.TabIndex = 3
        Me.lblPrintCnt.Text = "印刷部数："
        Me.lblPrintCnt.Visible = False
        '
        'nudPrintCount
        '
        Me.nudPrintCount.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.nudPrintCount.Location = New System.Drawing.Point(516, 689)
        Me.nudPrintCount.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudPrintCount.Name = "nudPrintCount"
        Me.nudPrintCount.Size = New System.Drawing.Size(45, 23)
        Me.nudPrintCount.TabIndex = 4
        Me.nudPrintCount.Value = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudPrintCount.Visible = False
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(581, 687)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 5
        Me.btnPrint.Text = "印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        Me.btnPrint.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(1023, 687)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 8
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnInsertPrint
        '
        Me.btnInsertPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsertPrint.Location = New System.Drawing.Point(745, 687)
        Me.btnInsertPrint.Name = "btnInsertPrint"
        Me.btnInsertPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnInsertPrint.TabIndex = 6
        Me.btnInsertPrint.Text = "登録＆印刷"
        Me.btnInsertPrint.UseVisualStyleBackColor = True
        Me.btnInsertPrint.Visible = False
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(885, 687)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(110, 30)
        Me.btnInsert.TabIndex = 7
        Me.btnInsert.Text = "登録のみ"
        Me.btnInsert.UseVisualStyleBackColor = True
        Me.btnInsert.Visible = False
        '
        'FM000205
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1192, 766)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.btnInsertPrint)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.nudPrintCount)
        Me.Controls.Add(Me.lblPrintCnt)
        Me.Controls.Add(Me.grpPrintArea)
        Me.Controls.Add(Me.crvReportMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM000205"
        Me.Text = "印刷プレビュー"
        Me.grpPrintArea.ResumeLayout(False)
        Me.grpPrintArea.PerformLayout()
        CType(Me.nudPageTo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPageFrom, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.nudPrintCount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents crvReportMain As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents grpPrintArea As System.Windows.Forms.GroupBox
    Friend WithEvents optAll As System.Windows.Forms.RadioButton
    Friend WithEvents optPageSet As System.Windows.Forms.RadioButton
    Friend WithEvents nudPageTo As System.Windows.Forms.NumericUpDown
    Friend WithEvents nudPageFrom As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblTo As System.Windows.Forms.Label
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents lblPrintCnt As System.Windows.Forms.Label
    Friend WithEvents nudPrintCount As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnInsertPrint As System.Windows.Forms.Button
    Friend WithEvents btnInsert As System.Windows.Forms.Button
End Class
