<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UC080104
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC080104))
        Me.Label11 = New System.Windows.Forms.Label
        Me.grpHeader = New System.Windows.Forms.GroupBox
        Me.dgdDayPayCloseDay = New System.Windows.Forms.DataGridView
        Me.Column3 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column4 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.dgdPayCutCloseDay = New System.Windows.Forms.DataGridView
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn
        Me.txtBankSendMargin = New System.Windows.Forms.TextBox
        Me.txtPayDay = New System.Windows.Forms.TextBox
        Me.txtTitle = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.btnCancel = New System.Windows.Forms.Button
        Me.btnEntryConfirm = New System.Windows.Forms.Button
        Me.grpNetBank = New System.Windows.Forms.GroupBox
        Me.lblSum = New System.Windows.Forms.Label
        Me.flxNetbank = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.btnAddMember = New System.Windows.Forms.Button
        Me.btnAllCheckOff = New System.Windows.Forms.Button
        Me.btnAllCheckOn = New System.Windows.Forms.Button
        Me.Label8 = New System.Windows.Forms.Label
        Me.btnPrint = New System.Windows.Forms.Button
        Me.btnOutputCsv = New System.Windows.Forms.Button
        Me.btnChange = New System.Windows.Forms.Button
        Me.btnReturn = New System.Windows.Forms.Button
        Me.grpHeader.SuspendLayout()
        CType(Me.dgdDayPayCloseDay, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgdPayCutCloseDay, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpNetBank.SuspendLayout()
        CType(Me.flxNetbank, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Label11.Font = New System.Drawing.Font("MS UI Gothic", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label11.Location = New System.Drawing.Point(200, 20)
        Me.Label11.MinimumSize = New System.Drawing.Size(720, 0)
        Me.Label11.Name = "Label11"
        Me.Label11.Padding = New System.Windows.Forms.Padding(14, 0, 14, 0)
        Me.Label11.Size = New System.Drawing.Size(720, 32)
        Me.Label11.TabIndex = 65
        Me.Label11.Text = "労金データ作成 - 新規登録"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'grpHeader
        '
        Me.grpHeader.Controls.Add(Me.dgdDayPayCloseDay)
        Me.grpHeader.Controls.Add(Me.dgdPayCutCloseDay)
        Me.grpHeader.Controls.Add(Me.txtBankSendMargin)
        Me.grpHeader.Controls.Add(Me.txtPayDay)
        Me.grpHeader.Controls.Add(Me.txtTitle)
        Me.grpHeader.Controls.Add(Me.Label3)
        Me.grpHeader.Controls.Add(Me.Label5)
        Me.grpHeader.Controls.Add(Me.Label4)
        Me.grpHeader.Controls.Add(Me.Label2)
        Me.grpHeader.Controls.Add(Me.Label1)
        Me.grpHeader.Location = New System.Drawing.Point(22, 60)
        Me.grpHeader.Name = "grpHeader"
        Me.grpHeader.Size = New System.Drawing.Size(980, 136)
        Me.grpHeader.TabIndex = 0
        Me.grpHeader.TabStop = False
        '
        'dgdDayPayCloseDay
        '
        Me.dgdDayPayCloseDay.AllowUserToAddRows = False
        Me.dgdDayPayCloseDay.AllowUserToDeleteRows = False
        Me.dgdDayPayCloseDay.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgdDayPayCloseDay.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgdDayPayCloseDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgdDayPayCloseDay.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column3, Me.Column4})
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgdDayPayCloseDay.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgdDayPayCloseDay.Location = New System.Drawing.Point(712, 14)
        Me.dgdDayPayCloseDay.Name = "dgdDayPayCloseDay"
        Me.dgdDayPayCloseDay.ReadOnly = True
        Me.dgdDayPayCloseDay.RowHeadersVisible = False
        Me.dgdDayPayCloseDay.RowTemplate.Height = 21
        Me.dgdDayPayCloseDay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdDayPayCloseDay.Size = New System.Drawing.Size(255, 114)
        Me.dgdDayPayCloseDay.StandardTab = True
        Me.dgdDayPayCloseDay.TabIndex = 4
        '
        'Column3
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column3.DefaultCellStyle = DataGridViewCellStyle2
        Me.Column3.HeaderText = "種別"
        Me.Column3.Name = "Column3"
        Me.Column3.ReadOnly = True
        Me.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column3.Width = 160
        '
        'Column4
        '
        Me.Column4.HeaderText = "締め日"
        Me.Column4.Name = "Column4"
        Me.Column4.ReadOnly = True
        Me.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column4.Width = 70
        '
        'dgdPayCutCloseDay
        '
        Me.dgdPayCutCloseDay.AllowUserToAddRows = False
        Me.dgdPayCutCloseDay.AllowUserToDeleteRows = False
        Me.dgdPayCutCloseDay.AllowUserToResizeRows = False
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle4.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgdPayCutCloseDay.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgdPayCutCloseDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.dgdPayCutCloseDay.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        DataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgdPayCutCloseDay.DefaultCellStyle = DataGridViewCellStyle6
        Me.dgdPayCutCloseDay.Location = New System.Drawing.Point(410, 14)
        Me.dgdPayCutCloseDay.Name = "dgdPayCutCloseDay"
        Me.dgdPayCutCloseDay.ReadOnly = True
        Me.dgdPayCutCloseDay.RowHeadersVisible = False
        Me.dgdPayCutCloseDay.RowTemplate.Height = 21
        Me.dgdPayCutCloseDay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgdPayCutCloseDay.Size = New System.Drawing.Size(256, 114)
        Me.dgdPayCutCloseDay.StandardTab = True
        Me.dgdPayCutCloseDay.TabIndex = 3
        '
        'Column1
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        Me.Column1.DefaultCellStyle = DataGridViewCellStyle5
        Me.Column1.HeaderText = "種別"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        Me.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column1.Width = 160
        '
        'Column2
        '
        Me.Column2.HeaderText = "締め日"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.Column2.Width = 70
        '
        'txtBankSendMargin
        '
        Me.txtBankSendMargin.BackColor = System.Drawing.Color.LightYellow
        Me.txtBankSendMargin.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtBankSendMargin.Location = New System.Drawing.Point(83, 77)
        Me.txtBankSendMargin.Name = "txtBankSendMargin"
        Me.txtBankSendMargin.ReadOnly = True
        Me.txtBankSendMargin.Size = New System.Drawing.Size(156, 23)
        Me.txtBankSendMargin.TabIndex = 2
        Me.txtBankSendMargin.TabStop = False
        '
        'txtPayDay
        '
        Me.txtPayDay.BackColor = System.Drawing.Color.LightYellow
        Me.txtPayDay.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtPayDay.Location = New System.Drawing.Point(83, 16)
        Me.txtPayDay.Name = "txtPayDay"
        Me.txtPayDay.ReadOnly = True
        Me.txtPayDay.Size = New System.Drawing.Size(98, 23)
        Me.txtPayDay.TabIndex = 0
        Me.txtPayDay.TabStop = False
        Me.txtPayDay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'txtTitle
        '
        Me.txtTitle.BackColor = System.Drawing.Color.LightYellow
        Me.txtTitle.Font = New System.Drawing.Font("ＭＳ ゴシック", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.txtTitle.Location = New System.Drawing.Point(83, 46)
        Me.txtTitle.Name = "txtTitle"
        Me.txtTitle.ReadOnly = True
        Me.txtTitle.Size = New System.Drawing.Size(279, 23)
        Me.txtTitle.TabIndex = 1
        Me.txtTitle.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label3.Location = New System.Drawing.Point(20, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 16)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "振込日"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label5.Location = New System.Drawing.Point(672, 17)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(40, 16)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "日当"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label4.Location = New System.Drawing.Point(367, 18)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(40, 16)
        Me.Label4.TabIndex = 13
        Me.Label4.Text = "源泉"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label2.Location = New System.Drawing.Point(36, 50)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(40, 16)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "題目"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Label1.Location = New System.Drawing.Point(4, 81)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(72, 16)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "支払方法"
        '
        'btnCancel
        '
        Me.btnCancel.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnCancel.Location = New System.Drawing.Point(882, 760)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(110, 30)
        Me.btnCancel.TabIndex = 12
        Me.btnCancel.Text = "キャンセル"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnEntryConfirm
        '
        Me.btnEntryConfirm.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnEntryConfirm.Location = New System.Drawing.Point(762, 760)
        Me.btnEntryConfirm.Name = "btnEntryConfirm"
        Me.btnEntryConfirm.Size = New System.Drawing.Size(110, 30)
        Me.btnEntryConfirm.TabIndex = 11
        Me.btnEntryConfirm.Text = "登録確認"
        Me.btnEntryConfirm.UseVisualStyleBackColor = True
        '
        'grpNetBank
        '
        Me.grpNetBank.BackColor = System.Drawing.SystemColors.Control
        Me.grpNetBank.Controls.Add(Me.lblSum)
        Me.grpNetBank.Controls.Add(Me.flxNetbank)
        Me.grpNetBank.Controls.Add(Me.Label7)
        Me.grpNetBank.Controls.Add(Me.Label6)
        Me.grpNetBank.Controls.Add(Me.btnAddMember)
        Me.grpNetBank.Controls.Add(Me.btnAllCheckOff)
        Me.grpNetBank.Controls.Add(Me.btnAllCheckOn)
        Me.grpNetBank.Controls.Add(Me.Label8)
        Me.grpNetBank.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.grpNetBank.Location = New System.Drawing.Point(23, 206)
        Me.grpNetBank.Name = "grpNetBank"
        Me.grpNetBank.Size = New System.Drawing.Size(980, 547)
        Me.grpNetBank.TabIndex = 1
        Me.grpNetBank.TabStop = False
        Me.grpNetBank.Text = "データ作成対象（xxx件）"
        '
        'lblSum
        '
        Me.lblSum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblSum.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblSum.ForeColor = System.Drawing.Color.Blue
        Me.lblSum.Location = New System.Drawing.Point(853, 508)
        Me.lblSum.Name = "lblSum"
        Me.lblSum.Size = New System.Drawing.Size(113, 20)
        Me.lblSum.TabIndex = 102
        Me.lblSum.Text = "0"
        Me.lblSum.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'flxNetbank
        '
        Me.flxNetbank.BackColor = System.Drawing.Color.Cornsilk
        Me.flxNetbank.ColumnInfo = resources.GetString("flxNetbank.ColumnInfo")
        Me.flxNetbank.Location = New System.Drawing.Point(14, 31)
        Me.flxNetbank.Name = "flxNetbank"
        Me.flxNetbank.Rows.Count = 1
        Me.flxNetbank.Rows.DefaultSize = 22
        Me.flxNetbank.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.ListBox
        Me.flxNetbank.Size = New System.Drawing.Size(952, 455)
        Me.flxNetbank.StyleInfo = resources.GetString("flxNetbank.StyleInfo")
        Me.flxNetbank.TabIndex = 5
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(84, 516)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(202, 16)
        Me.Label7.TabIndex = 93
        Me.Label7.Text = "「*」列のチェックを外してください"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(84, 500)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(232, 16)
        Me.Label6.TabIndex = 93
        Me.Label6.Text = "振込データ作成対象外の組合員は"
        '
        'btnAddMember
        '
        Me.btnAddMember.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnAddMember.Location = New System.Drawing.Point(435, 500)
        Me.btnAddMember.Name = "btnAddMember"
        Me.btnAddMember.Size = New System.Drawing.Size(110, 30)
        Me.btnAddMember.TabIndex = 8
        Me.btnAddMember.Text = "組合員の追加"
        Me.btnAddMember.UseVisualStyleBackColor = True
        '
        'btnAllCheckOff
        '
        Me.btnAllCheckOff.Location = New System.Drawing.Point(43, 503)
        Me.btnAllCheckOff.Name = "btnAllCheckOff"
        Me.btnAllCheckOff.Size = New System.Drawing.Size(29, 27)
        Me.btnAllCheckOff.TabIndex = 7
        Me.btnAllCheckOff.Text = "□"
        Me.btnAllCheckOff.UseVisualStyleBackColor = True
        '
        'btnAllCheckOn
        '
        Me.btnAllCheckOn.Location = New System.Drawing.Point(12, 503)
        Me.btnAllCheckOn.Name = "btnAllCheckOn"
        Me.btnAllCheckOn.Size = New System.Drawing.Size(29, 27)
        Me.btnAllCheckOn.TabIndex = 6
        Me.btnAllCheckOn.Text = "☑"
        Me.btnAllCheckOn.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(743, 510)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(104, 16)
        Me.Label8.TabIndex = 13
        Me.Label8.Text = "振込合計金額"
        '
        'btnPrint
        '
        Me.btnPrint.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnPrint.Location = New System.Drawing.Point(51, 760)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(110, 30)
        Me.btnPrint.TabIndex = 9
        Me.btnPrint.Text = "プレ印刷"
        Me.btnPrint.UseVisualStyleBackColor = True
        Me.btnPrint.Visible = False
        '
        'btnOutputCsv
        '
        Me.btnOutputCsv.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnOutputCsv.Location = New System.Drawing.Point(616, 760)
        Me.btnOutputCsv.Name = "btnOutputCsv"
        Me.btnOutputCsv.Size = New System.Drawing.Size(137, 30)
        Me.btnOutputCsv.TabIndex = 10
        Me.btnOutputCsv.Text = "振込ファイル出力"
        Me.btnOutputCsv.UseVisualStyleBackColor = True
        Me.btnOutputCsv.Visible = False
        '
        'btnChange
        '
        Me.btnChange.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnChange.Location = New System.Drawing.Point(762, 759)
        Me.btnChange.Name = "btnChange"
        Me.btnChange.Size = New System.Drawing.Size(110, 30)
        Me.btnChange.TabIndex = 73
        Me.btnChange.Text = "内容変更"
        Me.btnChange.UseVisualStyleBackColor = True
        Me.btnChange.Visible = False
        '
        'btnReturn
        '
        Me.btnReturn.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.btnReturn.Location = New System.Drawing.Point(882, 760)
        Me.btnReturn.Name = "btnReturn"
        Me.btnReturn.Size = New System.Drawing.Size(110, 30)
        Me.btnReturn.TabIndex = 74
        Me.btnReturn.Text = "戻る"
        Me.btnReturn.UseVisualStyleBackColor = True
        Me.btnReturn.Visible = False
        '
        'UC080104
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.btnOutputCsv)
        Me.Controls.Add(Me.btnPrint)
        Me.Controls.Add(Me.grpNetBank)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnEntryConfirm)
        Me.Controls.Add(Me.grpHeader)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.btnChange)
        Me.Controls.Add(Me.btnReturn)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Name = "UC080104"
        Me.Size = New System.Drawing.Size(1024, 820)
        Me.grpHeader.ResumeLayout(False)
        Me.grpHeader.PerformLayout()
        CType(Me.dgdDayPayCloseDay, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgdPayCutCloseDay, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNetBank.ResumeLayout(False)
        Me.grpNetBank.PerformLayout()
        CType(Me.flxNetbank, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents grpHeader As System.Windows.Forms.GroupBox
    Friend WithEvents dgdPayCutCloseDay As System.Windows.Forms.DataGridView
    Friend WithEvents txtBankSendMargin As System.Windows.Forms.TextBox
    Friend WithEvents txtPayDay As System.Windows.Forms.TextBox
    Friend WithEvents txtTitle As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents dgdDayPayCloseDay As System.Windows.Forms.DataGridView
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnEntryConfirm As System.Windows.Forms.Button
    Friend WithEvents grpNetBank As System.Windows.Forms.GroupBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents btnAddMember As System.Windows.Forms.Button
    Friend WithEvents btnAllCheckOff As System.Windows.Forms.Button
    Friend WithEvents btnAllCheckOn As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents btnOutputCsv As System.Windows.Forms.Button
    Friend WithEvents btnChange As System.Windows.Forms.Button
    Friend WithEvents flxNetbank As C1.Win.C1FlexGrid.C1FlexGrid
    Friend WithEvents btnReturn As System.Windows.Forms.Button
    Friend WithEvents Column3 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column4 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column1 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Column2 As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblSum As System.Windows.Forms.Label

End Class
