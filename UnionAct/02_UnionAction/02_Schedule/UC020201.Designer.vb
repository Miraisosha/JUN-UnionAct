<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC020201
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC020201))
        Me.fraSearchOption = New System.Windows.Forms.GroupBox()
        Me.lblKiValue = New System.Windows.Forms.Label()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbSearchMonth = New System.Windows.Forms.ComboBox()
        Me.cmbSearchYear = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbSearchPeriod = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lblTopTitle = New System.Windows.Forms.Label()
        Me.fraSchedule = New System.Windows.Forms.GroupBox()
        Me.cfgScheduleList = New C1.Win.C1FlexGrid.C1FlexGrid()
        Me.btnFileOutput = New System.Windows.Forms.Button()
        Me.lblVersion = New System.Windows.Forms.Label()
        Me.btnRangeRegist = New System.Windows.Forms.Button()
        Me.btnRenzokuRegist = New System.Windows.Forms.Button()
        Me.btnPrePrint = New System.Windows.Forms.Button()
        Me.btnScheduleDetail = New System.Windows.Forms.Button()
        Me.btnSchedulePrePrint = New System.Windows.Forms.Button()
        Me.fraSearchOption.SuspendLayout()
        Me.fraSchedule.SuspendLayout()
        CType(Me.cfgScheduleList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'fraSearchOption
        '
        Me.fraSearchOption.Controls.Add(Me.lblKiValue)
        Me.fraSearchOption.Controls.Add(Me.btnSearch)
        Me.fraSearchOption.Controls.Add(Me.Label6)
        Me.fraSearchOption.Controls.Add(Me.Label5)
        Me.fraSearchOption.Controls.Add(Me.cmbSearchMonth)
        Me.fraSearchOption.Controls.Add(Me.cmbSearchYear)
        Me.fraSearchOption.Controls.Add(Me.Label4)
        Me.fraSearchOption.Controls.Add(Me.Label3)
        Me.fraSearchOption.Controls.Add(Me.cmbSearchPeriod)
        Me.fraSearchOption.Controls.Add(Me.Label2)
        Me.fraSearchOption.Controls.Add(Me.Label1)
        Me.fraSearchOption.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.fraSearchOption.ForeColor = System.Drawing.Color.Blue
        Me.fraSearchOption.Location = New System.Drawing.Point(52, 72)
        Me.fraSearchOption.Name = "fraSearchOption"
        Me.fraSearchOption.Size = New System.Drawing.Size(901, 65)
        Me.fraSearchOption.TabIndex = 9
        Me.fraSearchOption.TabStop = False
        Me.fraSearchOption.Text = "検索条件"
        '
        'lblKiValue
        '
        Me.lblKiValue.AutoSize = True
        Me.lblKiValue.Location = New System.Drawing.Point(34, 30)
        Me.lblKiValue.Name = "lblKiValue"
        Me.lblKiValue.Size = New System.Drawing.Size(52, 16)
        Me.lblKiValue.TabIndex = 12
        Me.lblKiValue.Text = "Label7"
        Me.lblKiValue.Visible = False
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSearch.Location = New System.Drawing.Point(679, 23)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(110, 30)
        Me.btnSearch.TabIndex = 4
        Me.btnSearch.Text = "検索"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label6.Location = New System.Drawing.Point(528, 30)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(24, 16)
        Me.Label6.TabIndex = 10
        Me.Label6.Text = "月"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Location = New System.Drawing.Point(437, 30)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(24, 16)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "年"
        '
        'cmbSearchMonth
        '
        Me.cmbSearchMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchMonth.DropDownWidth = 42
        Me.cmbSearchMonth.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbSearchMonth.FormattingEnabled = True
        Me.cmbSearchMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cmbSearchMonth.Location = New System.Drawing.Point(467, 27)
        Me.cmbSearchMonth.MaxDropDownItems = 13
        Me.cmbSearchMonth.Name = "cmbSearchMonth"
        Me.cmbSearchMonth.Size = New System.Drawing.Size(42, 24)
        Me.cmbSearchMonth.TabIndex = 3
        '
        'cmbSearchYear
        '
        Me.cmbSearchYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchYear.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbSearchYear.FormattingEnabled = True
        Me.cmbSearchYear.Items.AddRange(New Object() {"2009", "2010", "2011", "2012", "2013", "2014", "2015"})
        Me.cmbSearchYear.Location = New System.Drawing.Point(376, 27)
        Me.cmbSearchYear.Name = "cmbSearchYear"
        Me.cmbSearchYear.Size = New System.Drawing.Size(55, 24)
        Me.cmbSearchYear.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.ForeColor = System.Drawing.Color.Red
        Me.Label4.Location = New System.Drawing.Point(284, 30)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(16, 16)
        Me.Label4.TabIndex = 6
        Me.Label4.Text = "*"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label3.Location = New System.Drawing.Point(301, 30)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(72, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "対象年月"
        '
        'cmbSearchPeriod
        '
        Me.cmbSearchPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchPeriod.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.cmbSearchPeriod.FormattingEnabled = True
        Me.cmbSearchPeriod.Location = New System.Drawing.Point(125, 27)
        Me.cmbSearchPeriod.Name = "cmbSearchPeriod"
        Me.cmbSearchPeriod.Size = New System.Drawing.Size(113, 24)
        Me.cmbSearchPeriod.TabIndex = 1
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.ForeColor = System.Drawing.Color.Red
        Me.Label2.Location = New System.Drawing.Point(85, 30)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(16, 16)
        Me.Label2.TabIndex = 2
        Me.Label2.Text = "*"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Location = New System.Drawing.Point(102, 30)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(24, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "期"
        '
        'lblTopTitle
        '
        Me.lblTopTitle.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.lblTopTitle.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblTopTitle.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTopTitle.Location = New System.Drawing.Point(200, 20)
        Me.lblTopTitle.Name = "lblTopTitle"
        Me.lblTopTitle.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblTopTitle.Size = New System.Drawing.Size(630, 35)
        Me.lblTopTitle.TabIndex = 8
        Me.lblTopTitle.Text = "日程表"
        Me.lblTopTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'fraSchedule
        '
        Me.fraSchedule.Controls.Add(Me.cfgScheduleList)
        Me.fraSchedule.Controls.Add(Me.btnFileOutput)
        Me.fraSchedule.Controls.Add(Me.lblVersion)
        Me.fraSchedule.Controls.Add(Me.btnRangeRegist)
        Me.fraSchedule.Controls.Add(Me.btnRenzokuRegist)
        Me.fraSchedule.Controls.Add(Me.btnPrePrint)
        Me.fraSchedule.Controls.Add(Me.btnScheduleDetail)
        Me.fraSchedule.Controls.Add(Me.btnSchedulePrePrint)
        Me.fraSchedule.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.fraSchedule.ForeColor = System.Drawing.Color.Blue
        Me.fraSchedule.Location = New System.Drawing.Point(52, 152)
        Me.fraSchedule.Name = "fraSchedule"
        Me.fraSchedule.Size = New System.Drawing.Size(901, 652)
        Me.fraSchedule.TabIndex = 10
        Me.fraSchedule.TabStop = False
        Me.fraSchedule.Text = "日程表"
        Me.fraSchedule.Visible = False
        '
        'cfgScheduleList
        '
        Me.cfgScheduleList.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.cfgScheduleList.AutoResize = True
        Me.cfgScheduleList.ColumnInfo = resources.GetString("cfgScheduleList.ColumnInfo")
        Me.cfgScheduleList.Font = New System.Drawing.Font("MS UI Gothic", 12.0!)
        Me.cfgScheduleList.Location = New System.Drawing.Point(6, 22)
        Me.cfgScheduleList.Name = "cfgScheduleList"
        Me.cfgScheduleList.Rows.Count = 100
        Me.cfgScheduleList.Rows.DefaultSize = 22
        Me.cfgScheduleList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.cfgScheduleList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
        Me.cfgScheduleList.Size = New System.Drawing.Size(889, 537)
        Me.cfgScheduleList.StyleInfo = resources.GetString("cfgScheduleList.StyleInfo")
        Me.cfgScheduleList.TabIndex = 20
        '
        'btnFileOutput
        '
        Me.btnFileOutput.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnFileOutput.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnFileOutput.Location = New System.Drawing.Point(175, 604)
        Me.btnFileOutput.Name = "btnFileOutput"
        Me.btnFileOutput.Size = New System.Drawing.Size(110, 30)
        Me.btnFileOutput.TabIndex = 6
        Me.btnFileOutput.Text = "ファイル出力"
        Me.btnFileOutput.UseVisualStyleBackColor = True
        '
        'lblVersion
        '
        Me.lblVersion.AutoSize = True
        Me.lblVersion.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblVersion.Location = New System.Drawing.Point(815, 581)
        Me.lblVersion.Name = "lblVersion"
        Me.lblVersion.Size = New System.Drawing.Size(85, 16)
        Me.lblVersion.TabIndex = 18
        Me.lblVersion.Text = "最新Rev.25"
        '
        'btnRangeRegist
        '
        Me.btnRangeRegist.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnRangeRegist.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnRangeRegist.Location = New System.Drawing.Point(761, 604)
        Me.btnRangeRegist.Name = "btnRangeRegist"
        Me.btnRangeRegist.Size = New System.Drawing.Size(110, 30)
        Me.btnRangeRegist.TabIndex = 10
        Me.btnRangeRegist.Text = "範囲登録"
        Me.btnRangeRegist.UseVisualStyleBackColor = True
        '
        'btnRenzokuRegist
        '
        Me.btnRenzokuRegist.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnRenzokuRegist.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnRenzokuRegist.Location = New System.Drawing.Point(645, 604)
        Me.btnRenzokuRegist.Name = "btnRenzokuRegist"
        Me.btnRenzokuRegist.Size = New System.Drawing.Size(110, 30)
        Me.btnRenzokuRegist.TabIndex = 9
        Me.btnRenzokuRegist.Text = "連続登録"
        Me.btnRenzokuRegist.UseVisualStyleBackColor = True
        '
        'btnPrePrint
        '
        Me.btnPrePrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrePrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnPrePrint.Location = New System.Drawing.Point(472, 604)
        Me.btnPrePrint.Name = "btnPrePrint"
        Me.btnPrePrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrePrint.TabIndex = 8
        Me.btnPrePrint.Text = "プレ印刷"
        Me.btnPrePrint.UseVisualStyleBackColor = True
        '
        'btnScheduleDetail
        '
        Me.btnScheduleDetail.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnScheduleDetail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnScheduleDetail.Location = New System.Drawing.Point(351, 604)
        Me.btnScheduleDetail.Name = "btnScheduleDetail"
        Me.btnScheduleDetail.Size = New System.Drawing.Size(110, 30)
        Me.btnScheduleDetail.TabIndex = 7
        Me.btnScheduleDetail.Text = "日程詳細"
        Me.btnScheduleDetail.UseVisualStyleBackColor = True
        '
        'btnSchedulePrePrint
        '
        Me.btnSchedulePrePrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnSchedulePrePrint.ForeColor = System.Drawing.SystemColors.ControlText
        Me.btnSchedulePrePrint.Location = New System.Drawing.Point(9, 604)
        Me.btnSchedulePrePrint.Name = "btnSchedulePrePrint"
        Me.btnSchedulePrePrint.Size = New System.Drawing.Size(160, 30)
        Me.btnSchedulePrePrint.TabIndex = 5
        Me.btnSchedulePrePrint.Text = "組合日程表プレ印刷"
        Me.btnSchedulePrePrint.UseVisualStyleBackColor = True
        '
        'UC020201
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.fraSchedule)
        Me.Controls.Add(Me.fraSearchOption)
        Me.Controls.Add(Me.lblTopTitle)
        Me.Name = "UC020201"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.fraSearchOption.ResumeLayout(False)
        Me.fraSearchOption.PerformLayout()
        Me.fraSchedule.ResumeLayout(False)
        Me.fraSchedule.PerformLayout()
        CType(Me.cfgScheduleList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents fraSearchOption As System.Windows.Forms.GroupBox
    Friend WithEvents btnSearch As System.Windows.Forms.Button
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents cmbSearchMonth As System.Windows.Forms.ComboBox
    Friend WithEvents cmbSearchYear As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmbSearchPeriod As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblTopTitle As System.Windows.Forms.Label
    Friend WithEvents fraSchedule As System.Windows.Forms.GroupBox
    Friend WithEvents lblVersion As System.Windows.Forms.Label
    Friend WithEvents btnRangeRegist As System.Windows.Forms.Button
    Friend WithEvents btnRenzokuRegist As System.Windows.Forms.Button
    Friend WithEvents btnPrePrint As System.Windows.Forms.Button
    Friend WithEvents btnScheduleDetail As System.Windows.Forms.Button
    Friend WithEvents btnSchedulePrePrint As System.Windows.Forms.Button
    Friend WithEvents btnFileOutput As System.Windows.Forms.Button
    Friend WithEvents cfgScheduleList As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents lblKiValue As System.Windows.Forms.Label

End Class
