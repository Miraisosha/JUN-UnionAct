Imports UnionAct.GUI.FinancialAffairs.WithHolding
Imports UnionAct.Framework
Imports System.ComponentModel

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FM050303
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    '<System.Diagnostics.DebuggerNonUserCode()> _
    'Protected Overrides Sub Dispose(ByVal disposing As Boolean)
    '    Try
    '        If disposing AndAlso components IsNot Nothing Then
    '            components.Dispose()
    '        End If
    '    Finally
    '        MyBase.Dispose(disposing)
    '    End Try
    'End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
        Dim manager As New ComponentResourceManager(GetType(FM050303))
        Me.cmbYear = New ComboBox
        Me.label6 = New Label
        Me.cmbMonth = New ComboBox
        Me.label7 = New Label
        Me.label5 = New Label
        Me.btnCancel = New Button
        Me.btnExecute = New Button
        MyBase.SuspendLayout()
        Me.cmbYear.DropDownStyle = ComboBoxStyle.DropDownList
        Me.cmbYear.FormattingEnabled = True
        Me.cmbYear.Items.AddRange(New Object() {"2008", "2009", "2010", "2011", "2012"}) 'TODO
        Me.cmbYear.Location = New Point(&HA5, &H22)
        Me.cmbYear.Name = "cmbYear"
        Me.cmbYear.Size = New Size(&H3F, &H18)
        Me.cmbYear.TabIndex = 6
        Me.label6.AutoSize = True
        Me.label6.Location = New Point(&HEA, &H26)
        Me.label6.Name = "label6"
        Me.label6.Size = New Size(&H18, &H10)
        Me.label6.TabIndex = 7
        Me.label6.Text = "年"
        Me.cmbMonth.DropDownStyle = ComboBoxStyle.DropDownList
        Me.cmbMonth.FormattingEnabled = True
        Me.cmbMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
        Me.cmbMonth.Location = New Point(&H103, &H22)
        Me.cmbMonth.MaxDropDownItems = 12
        Me.cmbMonth.Name = "cmbMonth"
        Me.cmbMonth.Size = New Size(50, &H18)
        Me.cmbMonth.TabIndex = 8
        Me.label7.AutoSize = True
        Me.label7.Location = New Point(&H13B, &H27)
        Me.label7.Name = "label7"
        Me.label7.Size = New Size(&H18, &H10)
        Me.label7.TabIndex = 9
        Me.label7.Text = "月"
        Me.label5.AutoSize = True
        Me.label5.Location = New Point(&H3B, &H26)
        Me.label5.Name = "label5"
        Me.label5.Size = New Size(&H68, &H10)
        Me.label5.TabIndex = 5
        Me.label5.Text = "集計対象年月"
        Me.btnCancel.Location = New Point(&HDF, 110)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New Size(&H74, &H20)
        Me.btnCancel.TabIndex = 11
        Me.btnCancel.Text = "ｷｬﾝｾﾙ"
        Me.btnCancel.UseVisualStyleBackColor = True
        AddHandler Me.btnCancel.Click, New EventHandler(AddressOf Me.btnCancel_Click)
        Me.btnExecute.Location = New Point(&H3E, 110)
        Me.btnExecute.Name = "btnExecute"
        Me.btnExecute.Size = New Size(&H74, &H20)
        Me.btnExecute.TabIndex = 10
        Me.btnExecute.Text = "実行"
        Me.btnExecute.UseVisualStyleBackColor = True
        AddHandler Me.btnExecute.Click, New EventHandler(AddressOf Me.btnExecute_Click)
        MyBase.AutoScaleDimensions = New SizeF(9.0!, 16.0!)
        MyBase.AutoScaleMode = AutoScaleMode.Font
        MyBase.ClientSize = New Size(&H18E, &HB6)
        MyBase.Controls.Add(Me.btnCancel)
        MyBase.Controls.Add(Me.btnExecute)
        MyBase.Controls.Add(Me.cmbYear)
        MyBase.Controls.Add(Me.label6)
        MyBase.Controls.Add(Me.cmbMonth)
        MyBase.Controls.Add(Me.label7)
        MyBase.Controls.Add(Me.label5)
        Me.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
        'MyBase.Icon = DirectCast(manager.GetObject("$this.Icon"), Icon)
        MyBase.Margin = New Padding(4)
        MyBase.Name = "FrmSelectCalcMonth"
        MyBase.ShowIcon = False
        MyBase.StartPosition = FormStartPosition.CenterParent
        Me.Text = "集計対象年月の選択"
        MyBase.ResumeLayout(False)
        MyBase.PerformLayout()
    End Sub

    ' Fields
    Private btnCancel As Button
    Private btnExecute As Button
    Private cmbMonth As ComboBox
    Private cmbYear As ComboBox
    'Private components As IContainer
    Public Execute As SelectMonthEventHandler
    Private label5 As Label
    Private label6 As Label
    Private label7 As Label
    Private UnionMessage As UnionMessage
End Class
