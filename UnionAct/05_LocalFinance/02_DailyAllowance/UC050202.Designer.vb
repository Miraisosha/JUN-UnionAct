<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC050202
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC050202))
        Me.btnAllCheckOn = New System.Windows.Forms.Button
        Me.btnAllCheckOff = New System.Windows.Forms.Button
        Me.Label11 = New System.Windows.Forms.Label
        Me.lblMonthTo = New System.Windows.Forms.TextBox
        Me.lblMonthFrom = New System.Windows.Forms.TextBox
        Me.lblCloseMonth = New System.Windows.Forms.TextBox
        Me.lblBelonging = New System.Windows.Forms.TextBox
        Me.lblYearTo = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.lblYearFrom = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.lblCloseYear = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label8 = New System.Windows.Forms.Label
        Me.lblSumDailyPay = New System.Windows.Forms.TextBox
        Me.lblSumBeforeDailyPay = New System.Windows.Forms.TextBox
        Me.lblSumAllDailyPay = New System.Windows.Forms.TextBox
        Me.btnDailyPayPrint = New System.Windows.Forms.Button
        Me.btnCancel = New System.Windows.Forms.Button
        Me.flxDailyPayClose = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.lblSumFoodExpenses = New System.Windows.Forms.TextBox
        Me.lblSumBeforeFoodExpenses = New System.Windows.Forms.TextBox
        CType(Me.flxDailyPayClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnAllCheckOn
        '
        Me.btnAllCheckOn.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAllCheckOn.Location = New System.Drawing.Point(64, 741)
        Me.btnAllCheckOn.Name = "btnAllCheckOn"
        Me.btnAllCheckOn.Size = New System.Drawing.Size(30, 30)
        Me.btnAllCheckOn.TabIndex = 8
        Me.btnAllCheckOn.Text = "☑"
        Me.btnAllCheckOn.UseVisualStyleBackColor = True
        '
        'btnAllCheckOff
        '
        Me.btnAllCheckOff.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAllCheckOff.Location = New System.Drawing.Point(64, 777)
        Me.btnAllCheckOff.Name = "btnAllCheckOff"
        Me.btnAllCheckOff.Size = New System.Drawing.Size(30, 30)
        Me.btnAllCheckOff.TabIndex = 9
        Me.btnAllCheckOff.Text = "□"
        Me.btnAllCheckOff.UseVisualStyleBackColor = True
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
        Me.Label11.TabIndex = 44
        Me.Label11.Text = "委員日当計算 - 詳細"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMonthTo
        '
        Me.lblMonthTo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblMonthTo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMonthTo.Location = New System.Drawing.Point(631, 70)
        Me.lblMonthTo.Name = "lblMonthTo"
        Me.lblMonthTo.ReadOnly = True
        Me.lblMonthTo.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblMonthTo.Size = New System.Drawing.Size(31, 23)
        Me.lblMonthTo.TabIndex = 6
        Me.lblMonthTo.Text = "09"
        '
        'lblMonthFrom
        '
        Me.lblMonthFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblMonthFrom.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblMonthFrom.Location = New System.Drawing.Point(460, 70)
        Me.lblMonthFrom.Name = "lblMonthFrom"
        Me.lblMonthFrom.ReadOnly = True
        Me.lblMonthFrom.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblMonthFrom.Size = New System.Drawing.Size(31, 23)
        Me.lblMonthFrom.TabIndex = 4
        Me.lblMonthFrom.Text = "08"
        '
        'lblCloseMonth
        '
        Me.lblCloseMonth.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblCloseMonth.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblCloseMonth.Location = New System.Drawing.Point(146, 70)
        Me.lblCloseMonth.Name = "lblCloseMonth"
        Me.lblCloseMonth.ReadOnly = True
        Me.lblCloseMonth.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblCloseMonth.Size = New System.Drawing.Size(31, 23)
        Me.lblCloseMonth.TabIndex = 2
        Me.lblCloseMonth.Text = "09"
        '
        'lblBelonging
        '
        Me.lblBelonging.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblBelonging.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblBelonging.Location = New System.Drawing.Point(882, 70)
        Me.lblBelonging.Name = "lblBelonging"
        Me.lblBelonging.ReadOnly = True
        Me.lblBelonging.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblBelonging.Size = New System.Drawing.Size(65, 23)
        Me.lblBelonging.TabIndex = 7
        Me.lblBelonging.Text = "東京"
        '
        'lblYearTo
        '
        Me.lblYearTo.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblYearTo.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblYearTo.Location = New System.Drawing.Point(560, 70)
        Me.lblYearTo.Name = "lblYearTo"
        Me.lblYearTo.ReadOnly = True
        Me.lblYearTo.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblYearTo.Size = New System.Drawing.Size(46, 23)
        Me.lblYearTo.TabIndex = 5
        Me.lblYearTo.Text = "2011"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label9.Location = New System.Drawing.Point(841, 73)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(40, 16)
        Me.Label9.TabIndex = 70
        Me.Label9.Text = "支部"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label7.Location = New System.Drawing.Point(664, 73)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(40, 16)
        Me.Label7.TabIndex = 71
        Me.Label7.Text = "月分"
        '
        'lblYearFrom
        '
        Me.lblYearFrom.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblYearFrom.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblYearFrom.Location = New System.Drawing.Point(389, 70)
        Me.lblYearFrom.Name = "lblYearFrom"
        Me.lblYearFrom.ReadOnly = True
        Me.lblYearFrom.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblYearFrom.Size = New System.Drawing.Size(46, 23)
        Me.lblYearFrom.TabIndex = 3
        Me.lblYearFrom.Text = "2011"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.Location = New System.Drawing.Point(493, 73)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 16)
        Me.Label5.TabIndex = 68
        Me.Label5.Text = "月分　～"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label6.Location = New System.Drawing.Point(608, 73)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 16)
        Me.Label6.TabIndex = 65
        Me.Label6.Text = "年"
        '
        'lblCloseYear
        '
        Me.lblCloseYear.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.lblCloseYear.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblCloseYear.Location = New System.Drawing.Point(75, 70)
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
        Me.Label4.Location = New System.Drawing.Point(437, 73)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(24, 16)
        Me.Label4.TabIndex = 66
        Me.Label4.Text = "年"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(179, 73)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(24, 16)
        Me.Label2.TabIndex = 69
        Me.Label2.Text = "月"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(123, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(24, 16)
        Me.Label3.TabIndex = 67
        Me.Label3.Text = "年"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(320, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 64
        Me.Label1.Text = "対象年月"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label8.Location = New System.Drawing.Point(23, 73)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(53, 16)
        Me.Label8.TabIndex = 63
        Me.Label8.Text = "締め日"
        '
        'lblSumDailyPay
        '
        Me.lblSumDailyPay.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumDailyPay.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblSumDailyPay.Location = New System.Drawing.Point(416, 741)
        Me.lblSumDailyPay.Name = "lblSumDailyPay"
        Me.lblSumDailyPay.ReadOnly = True
        Me.lblSumDailyPay.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumDailyPay.Size = New System.Drawing.Size(88, 23)
        Me.lblSumDailyPay.TabIndex = 10
        Me.lblSumDailyPay.Text = "999,999,999\"
        '
        'lblSumBeforeDailyPay
        '
        Me.lblSumBeforeDailyPay.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumBeforeDailyPay.Location = New System.Drawing.Point(598, 741)
        Me.lblSumBeforeDailyPay.Name = "lblSumBeforeDailyPay"
        Me.lblSumBeforeDailyPay.ReadOnly = True
        Me.lblSumBeforeDailyPay.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumBeforeDailyPay.Size = New System.Drawing.Size(88, 23)
        Me.lblSumBeforeDailyPay.TabIndex = 12
        Me.lblSumBeforeDailyPay.Text = "999,999,999\"
        '
        'lblSumAllDailyPay
        '
        Me.lblSumAllDailyPay.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumAllDailyPay.Location = New System.Drawing.Point(812, 741)
        Me.lblSumAllDailyPay.Name = "lblSumAllDailyPay"
        Me.lblSumAllDailyPay.ReadOnly = True
        Me.lblSumAllDailyPay.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumAllDailyPay.Size = New System.Drawing.Size(95, 23)
        Me.lblSumAllDailyPay.TabIndex = 14
        Me.lblSumAllDailyPay.Text = "999,999,999\"
        '
        'btnDailyPayPrint
        '
        Me.btnDailyPayPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnDailyPayPrint.Location = New System.Drawing.Point(721, 770)
        Me.btnDailyPayPrint.Name = "btnDailyPayPrint"
        Me.btnDailyPayPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnDailyPayPrint.TabIndex = 15
        Me.btnDailyPayPrint.Text = "委員日当印刷"
        Me.btnDailyPayPrint.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(837, 770)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 16
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'flxDailyPayClose
        '
        Me.flxDailyPayClose.ColumnInfo = "10,1,0,0,0,95,Columns:1{Style:""TextAlign:GeneralCenter;ImageAlign:CenterCenter;"";" & _
            "}" & Global.Microsoft.VisualBasic.ChrW(9)
        Me.flxDailyPayClose.Location = New System.Drawing.Point(64, 111)
        Me.flxDailyPayClose.Name = "flxDailyPayClose"
        Me.flxDailyPayClose.Rows.DefaultSize = 19
        Me.flxDailyPayClose.Size = New System.Drawing.Size(860, 624)
        Me.flxDailyPayClose.StyleInfo = resources.GetString("flxDailyPayClose.StyleInfo")
        Me.flxDailyPayClose.TabIndex = 83
        '
        'lblSumFoodExpenses
        '
        Me.lblSumFoodExpenses.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumFoodExpenses.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblSumFoodExpenses.Location = New System.Drawing.Point(507, 741)
        Me.lblSumFoodExpenses.Name = "lblSumFoodExpenses"
        Me.lblSumFoodExpenses.ReadOnly = True
        Me.lblSumFoodExpenses.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumFoodExpenses.Size = New System.Drawing.Size(88, 23)
        Me.lblSumFoodExpenses.TabIndex = 11
        Me.lblSumFoodExpenses.Text = "999,999,999\"
        '
        'lblSumBeforeFoodExpenses
        '
        Me.lblSumBeforeFoodExpenses.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSumBeforeFoodExpenses.Location = New System.Drawing.Point(689, 741)
        Me.lblSumBeforeFoodExpenses.Name = "lblSumBeforeFoodExpenses"
        Me.lblSumBeforeFoodExpenses.ReadOnly = True
        Me.lblSumBeforeFoodExpenses.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblSumBeforeFoodExpenses.Size = New System.Drawing.Size(120, 23)
        Me.lblSumBeforeFoodExpenses.TabIndex = 13
        Me.lblSumBeforeFoodExpenses.Text = "999,999,999\"
        '
        'UC050202
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.lblSumFoodExpenses)
        Me.Controls.Add(Me.lblSumBeforeFoodExpenses)
        Me.Controls.Add(Me.flxDailyPayClose)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnDailyPayPrint)
        Me.Controls.Add(Me.lblSumDailyPay)
        Me.Controls.Add(Me.lblSumBeforeDailyPay)
        Me.Controls.Add(Me.lblSumAllDailyPay)
        Me.Controls.Add(Me.lblMonthTo)
        Me.Controls.Add(Me.lblMonthFrom)
        Me.Controls.Add(Me.lblCloseMonth)
        Me.Controls.Add(Me.lblBelonging)
        Me.Controls.Add(Me.lblYearTo)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.lblYearFrom)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.lblCloseYear)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.btnAllCheckOff)
        Me.Controls.Add(Me.btnAllCheckOn)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Name = "UC050202"
        Me.Size = New System.Drawing.Size(1024, 820)
        CType(Me.flxDailyPayClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnAllCheckOn As System.Windows.Forms.Button
    Friend WithEvents btnAllCheckOff As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents lblMonthTo As System.Windows.Forms.TextBox
    Friend WithEvents lblMonthFrom As System.Windows.Forms.TextBox
    Friend WithEvents lblCloseMonth As System.Windows.Forms.TextBox
    Friend WithEvents lblBelonging As System.Windows.Forms.TextBox
    Friend WithEvents lblYearTo As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents lblYearFrom As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lblCloseYear As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents lblSumDailyPay As System.Windows.Forms.TextBox
    Friend WithEvents lblSumBeforeDailyPay As System.Windows.Forms.TextBox
    Friend WithEvents lblSumAllDailyPay As System.Windows.Forms.TextBox
    Friend WithEvents btnDailyPayPrint As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents flxDailyPayClose As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblSumFoodExpenses As System.Windows.Forms.TextBox
    Friend WithEvents lblSumBeforeFoodExpenses As System.Windows.Forms.TextBox

End Class
