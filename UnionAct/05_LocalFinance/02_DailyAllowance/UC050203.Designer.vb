<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC050203
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
        Me.lblMonth = New System.Windows.Forms.TextBox
        Me.lblCloseMonth = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lblYear = New System.Windows.Forms.TextBox
        Me.lblCloseYear = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.Label11 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnExecutivePayReportPrint = New System.Windows.Forms.Button
        Me.lblSumFoodExpenses = New System.Windows.Forms.TextBox
        Me.lblSumBeforeFoodExpenses = New System.Windows.Forms.TextBox
        Me.lblSumAllDailyPay = New System.Windows.Forms.TextBox
        Me.btnAllCheckOff = New System.Windows.Forms.Button
        Me.btnAllCheckOn = New System.Windows.Forms.Button
        Me.lblSumBeforeDailyPay = New System.Windows.Forms.TextBox
        Me.lblSumDailyPay = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.cmbBelonging = New System.Windows.Forms.ComboBox
        Me.btnShow = New System.Windows.Forms.Button
        Me.flxDailyPayClose = New C1.Win.C1FlexGrid.C1FlexGrid
        CType(Me.flxDailyPayClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblMonth
        '
        Me.lblMonth.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblMonth.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMonth.Location = New System.Drawing.Point(543, 69)
        Me.lblMonth.Name = "lblMonth"
        Me.lblMonth.ReadOnly = True
        Me.lblMonth.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblMonth.Size = New System.Drawing.Size(31, 23)
        Me.lblMonth.TabIndex = 4
        Me.lblMonth.Text = "08"
        '
        'lblCloseMonth
        '
        Me.lblCloseMonth.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblCloseMonth.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblCloseMonth.Location = New System.Drawing.Point(134, 69)
        Me.lblCloseMonth.Name = "lblCloseMonth"
        Me.lblCloseMonth.ReadOnly = True
        Me.lblCloseMonth.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblCloseMonth.Size = New System.Drawing.Size(31, 23)
        Me.lblCloseMonth.TabIndex = 2
        Me.lblCloseMonth.Text = "09"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label9.Location = New System.Drawing.Point(768, 72)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(40, 16)
        Me.Label9.TabIndex = 88
        Me.Label9.Text = "支部"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label7.Location = New System.Drawing.Point(581, 72)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 16)
        Me.Label7.TabIndex = 89
        Me.Label7.Text = "月分"
        '
        'lblYear
        '
        Me.lblYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblYear.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblYear.Location = New System.Drawing.Point(472, 69)
        Me.lblYear.Name = "lblYear"
        Me.lblYear.ReadOnly = True
        Me.lblYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblYear.Size = New System.Drawing.Size(46, 23)
        Me.lblYear.TabIndex = 3
        Me.lblYear.Text = "2011"
        '
        'lblCloseYear
        '
        Me.lblCloseYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblCloseYear.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblCloseYear.Location = New System.Drawing.Point(63, 69)
        Me.lblCloseYear.Name = "lblCloseYear"
        Me.lblCloseYear.ReadOnly = True
        Me.lblCloseYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblCloseYear.Size = New System.Drawing.Size(46, 23)
        Me.lblCloseYear.TabIndex = 1
        Me.lblCloseYear.Text = "2011"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.Location = New System.Drawing.Point(520, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(24, 16)
        Me.Label4.TabIndex = 84
        Me.Label4.Text = "年"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(167, 72)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(24, 16)
        Me.Label2.TabIndex = 87
        Me.Label2.Text = "月"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(111, 72)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(24, 16)
        Me.Label3.TabIndex = 85
        Me.Label3.Text = "年"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(403, 72)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 82
        Me.Label1.Text = "対象年月"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label8.Location = New System.Drawing.Point(11, 72)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 16)
        Me.Label8.TabIndex = 81
        Me.Label8.Text = "締め日"
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
        Me.Label11.TabIndex = 79
        Me.Label11.Text = "中執日当計算 - 詳細"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!)
        Me.btnCancel.Location = New System.Drawing.Point(889, 776)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 15
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnExecutivePayReportPrint
        '
        Me.btnExecutivePayReportPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!)
        Me.btnExecutivePayReportPrint.Location = New System.Drawing.Point(762, 776)
        Me.btnExecutivePayReportPrint.Name = "btnExecutivePayReportPrint"
        Me.btnExecutivePayReportPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnExecutivePayReportPrint.TabIndex = 14
        Me.btnExecutivePayReportPrint.Text = "中執日当印刷"
        Me.btnExecutivePayReportPrint.UseVisualStyleBackColor = True
        '
        'lblSumFoodExpenses
        '
        Me.lblSumFoodExpenses.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumFoodExpenses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblSumFoodExpenses.Location = New System.Drawing.Point(483, 734)
        Me.lblSumFoodExpenses.Name = "lblSumFoodExpenses"
        Me.lblSumFoodExpenses.ReadOnly = True
        Me.lblSumFoodExpenses.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumFoodExpenses.Size = New System.Drawing.Size(125, 23)
        Me.lblSumFoodExpenses.TabIndex = 10
        Me.lblSumFoodExpenses.Text = "999,999,999\"
        '
        'lblSumBeforeFoodExpenses
        '
        Me.lblSumBeforeFoodExpenses.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumBeforeFoodExpenses.Location = New System.Drawing.Point(743, 734)
        Me.lblSumBeforeFoodExpenses.Name = "lblSumBeforeFoodExpenses"
        Me.lblSumBeforeFoodExpenses.ReadOnly = True
        Me.lblSumBeforeFoodExpenses.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumBeforeFoodExpenses.Size = New System.Drawing.Size(125, 23)
        Me.lblSumBeforeFoodExpenses.TabIndex = 12
        Me.lblSumBeforeFoodExpenses.Text = "999,999,999\"
        '
        'lblSumAllDailyPay
        '
        Me.lblSumAllDailyPay.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumAllDailyPay.Location = New System.Drawing.Point(874, 734)
        Me.lblSumAllDailyPay.Name = "lblSumAllDailyPay"
        Me.lblSumAllDailyPay.ReadOnly = True
        Me.lblSumAllDailyPay.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumAllDailyPay.Size = New System.Drawing.Size(125, 23)
        Me.lblSumAllDailyPay.TabIndex = 13
        Me.lblSumAllDailyPay.Text = "999,999,999\"
        '
        'btnAllCheckOff
        '
        Me.btnAllCheckOff.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAllCheckOff.Location = New System.Drawing.Point(12, 766)
        Me.btnAllCheckOff.Name = "btnAllCheckOff"
        Me.btnAllCheckOff.Size = New System.Drawing.Size(30, 30)
        Me.btnAllCheckOff.TabIndex = 8
        Me.btnAllCheckOff.Text = "□"
        Me.btnAllCheckOff.UseVisualStyleBackColor = True
        '
        'btnAllCheckOn
        '
        Me.btnAllCheckOn.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAllCheckOn.Location = New System.Drawing.Point(12, 734)
        Me.btnAllCheckOn.Name = "btnAllCheckOn"
        Me.btnAllCheckOn.Size = New System.Drawing.Size(30, 30)
        Me.btnAllCheckOn.TabIndex = 7
        Me.btnAllCheckOn.Text = "☑"
        Me.btnAllCheckOn.UseVisualStyleBackColor = True
        '
        'lblSumBeforeDailyPay
        '
        Me.lblSumBeforeDailyPay.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumBeforeDailyPay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblSumBeforeDailyPay.Location = New System.Drawing.Point(612, 734)
        Me.lblSumBeforeDailyPay.Name = "lblSumBeforeDailyPay"
        Me.lblSumBeforeDailyPay.ReadOnly = True
        Me.lblSumBeforeDailyPay.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumBeforeDailyPay.Size = New System.Drawing.Size(125, 23)
        Me.lblSumBeforeDailyPay.TabIndex = 11
        Me.lblSumBeforeDailyPay.Text = "999,999,999\"
        '
        'lblSumDailyPay
        '
        Me.lblSumDailyPay.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumDailyPay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblSumDailyPay.Location = New System.Drawing.Point(346, 734)
        Me.lblSumDailyPay.Name = "lblSumDailyPay"
        Me.lblSumDailyPay.ReadOnly = True
        Me.lblSumDailyPay.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumDailyPay.Size = New System.Drawing.Size(125, 23)
        Me.lblSumDailyPay.TabIndex = 9
        Me.lblSumDailyPay.Text = "999,999,999\"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label10.ForeColor = System.Drawing.Color.Red
        Me.Label10.Location = New System.Drawing.Point(746, 72)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(16, 16)
        Me.Label10.TabIndex = 106
        Me.Label10.Text = "*"
        '
        'cmbBelonging
        '
        Me.cmbBelonging.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbBelonging.FormattingEnabled = True
        Me.cmbBelonging.Location = New System.Drawing.Point(814, 69)
        Me.cmbBelonging.Name = "cmbBelonging"
        Me.cmbBelonging.Size = New System.Drawing.Size(103, 24)
        Me.cmbBelonging.TabIndex = 5
        '
        'btnShow
        '
        Me.btnShow.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnShow.Location = New System.Drawing.Point(937, 66)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(75, 30)
        Me.btnShow.TabIndex = 6
        Me.btnShow.Text = "表示"
        Me.btnShow.UseVisualStyleBackColor = True
        '
        'flxDailyPayClose
        '
        Me.flxDailyPayClose.ColumnInfo = "10,1,0,0,0,90,Columns:"
        Me.flxDailyPayClose.Location = New System.Drawing.Point(12, 108)
        Me.flxDailyPayClose.Name = "flxDailyPayClose"
        Me.flxDailyPayClose.Rows.DefaultSize = 18
        Me.flxDailyPayClose.Size = New System.Drawing.Size(1000, 620)
        Me.flxDailyPayClose.TabIndex = 109
        '
        'UC050203
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.flxDailyPayClose)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.cmbBelonging)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.lblSumDailyPay)
        Me.Controls.Add(Me.lblSumBeforeDailyPay)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnExecutivePayReportPrint)
        Me.Controls.Add(Me.lblSumFoodExpenses)
        Me.Controls.Add(Me.lblSumBeforeFoodExpenses)
        Me.Controls.Add(Me.lblSumAllDailyPay)
        Me.Controls.Add(Me.btnAllCheckOff)
        Me.Controls.Add(Me.btnAllCheckOn)
        Me.Controls.Add(Me.lblMonth)
        Me.Controls.Add(Me.lblCloseMonth)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblYear)
        Me.Controls.Add(Me.lblCloseYear)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label11)
        Me.Name = "UC050203"
        Me.Size = New System.Drawing.Size(1024, 820)
        CType(Me.flxDailyPayClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblMonth As System.Windows.Forms.TextBox
    Friend WithEvents lblCloseMonth As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblYear As System.Windows.Forms.TextBox
    Friend WithEvents lblCloseYear As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnExecutivePayReportPrint As System.Windows.Forms.Button
    Friend WithEvents lblSumFoodExpenses As System.Windows.Forms.TextBox
    Friend WithEvents lblSumBeforeFoodExpenses As System.Windows.Forms.TextBox
    Friend WithEvents lblSumAllDailyPay As System.Windows.Forms.TextBox
    Friend WithEvents btnAllCheckOff As System.Windows.Forms.Button
    Friend WithEvents btnAllCheckOn As System.Windows.Forms.Button
    Friend WithEvents lblSumBeforeDailyPay As System.Windows.Forms.TextBox
    Friend WithEvents lblSumDailyPay As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents cmbBelonging As System.Windows.Forms.ComboBox
    Friend WithEvents btnShow As System.Windows.Forms.Button
    Friend WithEvents flxDailyPayClose As C1.Win.C1FlexGrid.C1FlexGrid

End Class
