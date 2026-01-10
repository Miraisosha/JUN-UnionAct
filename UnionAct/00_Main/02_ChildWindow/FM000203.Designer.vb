<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM000203
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
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.crvReportMain = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        Me.btnInsertPrint = New System.Windows.Forms.Button
        Me.btnInsert = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.lblPrintCount = New System.Windows.Forms.Label
        Me.nudPrintCount = New System.Windows.Forms.NumericUpDown
        Me.btnInsertParenthesisPrint = New System.Windows.Forms.Button
        Me.btnPrint = New System.Windows.Forms.Button
        Me.Panel1.SuspendLayout()
        CType(Me.nudPrintCount, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.crvReportMain)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1004, 624)
        Me.Panel1.TabIndex = 0
        '
        'crvReportMain
        '
        Me.crvReportMain.ActiveViewIndex = -1
        Me.crvReportMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crvReportMain.DisplayGroupTree = False
        Me.crvReportMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.crvReportMain.Location = New System.Drawing.Point(0, 0)
        Me.crvReportMain.Name = "crvReportMain"
        Me.crvReportMain.SelectionFormula = ""
        Me.crvReportMain.ShowCloseButton = False
        Me.crvReportMain.ShowExportButton = False
        Me.crvReportMain.ShowGroupTreeButton = False
        Me.crvReportMain.ShowPrintButton = False
        Me.crvReportMain.ShowRefreshButton = False
        Me.crvReportMain.ShowTextSearchButton = False
        Me.crvReportMain.Size = New System.Drawing.Size(1004, 624)
        Me.crvReportMain.TabIndex = 0
        Me.crvReportMain.ViewTimeSelectionFormula = ""
        '
        'btnInsertPrint
        '
        Me.btnInsertPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsertPrint.Location = New System.Drawing.Point(315, 643)
        Me.btnInsertPrint.Name = "btnInsertPrint"
        Me.btnInsertPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnInsertPrint.TabIndex = 2
        Me.btnInsertPrint.Text = "登録＆印刷"
        Me.btnInsertPrint.UseVisualStyleBackColor = True
        Me.btnInsertPrint.Visible = False
        '
        'btnInsert
        '
        Me.btnInsert.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsert.Location = New System.Drawing.Point(448, 643)
        Me.btnInsert.Name = "btnInsert"
        Me.btnInsert.Size = New System.Drawing.Size(110, 30)
        Me.btnInsert.TabIndex = 3
        Me.btnInsert.Text = "登録のみ"
        Me.btnInsert.UseVisualStyleBackColor = True
        Me.btnInsert.Visible = False
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(582, 643)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 4
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'lblPrintCount
        '
        Me.lblPrintCount.AutoSize = True
        Me.lblPrintCount.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblPrintCount.Location = New System.Drawing.Point(35, 650)
        Me.lblPrintCount.Name = "lblPrintCount"
        Me.lblPrintCount.Size = New System.Drawing.Size(80, 16)
        Me.lblPrintCount.TabIndex = 2
        Me.lblPrintCount.Text = "印刷部数："
        Me.lblPrintCount.Visible = False
        '
        'nudPrintCount
        '
        Me.nudPrintCount.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.nudPrintCount.Location = New System.Drawing.Point(120, 650)
        Me.nudPrintCount.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudPrintCount.Name = "nudPrintCount"
        Me.nudPrintCount.Size = New System.Drawing.Size(59, 23)
        Me.nudPrintCount.TabIndex = 1
        Me.nudPrintCount.Value = New Decimal(New Integer() {1, 0, 0, 0})
        Me.nudPrintCount.Visible = False
        '
        'btnInsertParenthesisPrint
        '
        Me.btnInsertParenthesisPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnInsertParenthesisPrint.Location = New System.Drawing.Point(724, 643)
        Me.btnInsertParenthesisPrint.Name = "btnInsertParenthesisPrint"
        Me.btnInsertParenthesisPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnInsertParenthesisPrint.TabIndex = 5
        Me.btnInsertParenthesisPrint.Text = "登録（印刷）"
        Me.btnInsertParenthesisPrint.UseVisualStyleBackColor = True
        Me.btnInsertParenthesisPrint.Visible = False
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(862, 643)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 6
        Me.btnPrint.Text = "印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        Me.btnPrint.Visible = False
        '
        'FM000203
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1004, 687)
        Me.ControlBox = False
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.btnInsertParenthesisPrint)
        Me.Controls.Add(Me.nudPrintCount)
        Me.Controls.Add(Me.lblPrintCount)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnInsert)
        Me.Controls.Add(Me.btnInsertPrint)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FM000203"
        Me.Text = "印刷プレビュー"
        Me.Panel1.ResumeLayout(False)
        CType(Me.nudPrintCount, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents crvReportMain As CrystalDecisions.Windows.Forms.CrystalReportViewer
    Friend WithEvents btnInsertPrint As System.Windows.Forms.Button
    Friend WithEvents btnInsert As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents lblPrintCount As System.Windows.Forms.Label
    Friend WithEvents nudPrintCount As System.Windows.Forms.NumericUpDown
    Friend WithEvents btnInsertParenthesisPrint As System.Windows.Forms.Button
    Friend WithEvents btnPrint As System.Windows.Forms.Button
End Class
