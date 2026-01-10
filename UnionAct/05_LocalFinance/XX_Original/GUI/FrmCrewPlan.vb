Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Framework
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.GUI.Common
Imports UnionAct.NSCLMsg
Imports UnionAct.GUI.UnionComponent

Namespace GUI.RevenueExpenditure.UnionForm
    Public Class FrmCrewPlan
        Inherits Form
        ' Methods
        Public Sub New(ByVal _strRevenueTitle As Object, ByVal _strRevenueStartDate As Object, ByVal _strRevenueEndDate As Object, ByVal _iNewStaffAverage As Object, ByVal _iCapPromotionAverage As Object, ByVal _dUnpromotionRate As Object, ByVal _iUnpromotionAverage As Object, ByVal _dSeniorRetire As Object, ByVal _dSeniorAverage As Object)
            Me.InitializeComponent()
            Me.lbltitle.Text = _strRevenueTitle.ToString
            Dim str As String = _strRevenueStartDate.ToString
            Dim str2 As String = _strRevenueEndDate.ToString
            Me.txtRevenueStrDate.Text = String.Concat(New String() {str.Substring(0, 4), "/", str.Substring(4, 2), "/", str.Substring(6, 2)})
            Me.txtRevenueEndDate.Text = String.Concat(New String() {str2.Substring(0, 4), "/", str2.Substring(4, 2), "/", str2.Substring(6, 2)})
            If (Not _iNewStaffAverage Is Nothing) Then
                Me.txtNewStaffAverage.Text = _iNewStaffAverage.ToString
            End If
            If (Not _iCapPromotionAverage Is Nothing) Then
                Me.txtCapPromotionAverage.Text = _iCapPromotionAverage.ToString
            End If
            If (Not _dUnpromotionRate Is Nothing) Then
                Me.txtUnpromotionRate.Text = String.Format("{0:#0.###}", _dUnpromotionRate)
            End If
            If (Not _iUnpromotionAverage Is Nothing) Then
                Me.txtUnpromotionAverage.Text = _iUnpromotionAverage.ToString
            End If
            If (Not _dSeniorRetire Is Nothing) Then
                Me.txtSeniorStayRate.Text = _dSeniorRetire.ToString
            End If
            If (Not _dSeniorAverage Is Nothing) Then
                Me.txtSeniorAverage.Text = _dSeniorAverage.ToString
            End If
        End Sub

        Private Sub btnChange_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim message As New UnionMessage
                If message.ShowMessage("GQ0007").Equals(DialogResult.Yes) Then
                    Me.IsOk = False
                    MyBase.Close()
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub btnOK_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim num As Decimal
                ValidatorUtility.ValidateAll(Me)
                Me.iNewStaffAverage = Convert.ToInt32(Me.txtNewStaffAverage.Text)
                Me.iCapPromotionAverage = Convert.ToInt32(Me.txtCapPromotionAverage.Text)
                Me.iUnpromotionAverage = Convert.ToInt32(Me.txtUnpromotionAverage.Text)
                Me.iSeniorAverage = Convert.ToInt32(Me.txtSeniorAverage.Text)
                Dim appEx As AppUnionException = Nothing
                If Decimal.TryParse(Me.txtUnpromotionRate.Text, num).Equals(False) Then
                    Me.txtUnpromotionRate.BackColor = Color.LightSalmon
                    PublicCommand.AddAppUnionExceptionData(appEx, "GE0021", New String() {"非組合員倍率"})
                Else
                    Me.dUnpromotionRate = Convert.ToDouble(Me.txtUnpromotionRate.Text)
                    Dim num2 As Decimal = Convert.ToDecimal(Me.dUnpromotionRate)
                    Dim num3 As Integer = Convert.ToInt32(PublicCommand.ToRoundDown(Me.dUnpromotionRate, 1))
                    If ((num2 <= 0) OrElse (num2 > 100)) Then
                        Me.txtUnpromotionRate.BackColor = Color.LightSalmon
                        PublicCommand.AddAppUnionExceptionData(appEx, "GE0021", New String() {"非組合員倍率"})
                    Else
                        Dim num4 As Decimal = (num2 - num3)
                        If (num4.ToString.Length > 5) Then
                            Me.txtUnpromotionRate.BackColor = Color.LightSalmon
                            PublicCommand.AddAppUnionExceptionData(appEx, "GE0021", New String() {"非組合員倍率"})
                        End If
                    End If
                End If
                If Decimal.TryParse(Me.txtSeniorStayRate.Text, num).Equals(False) Then
                    Me.txtSeniorStayRate.BackColor = Color.LightSalmon
                    PublicCommand.AddAppUnionExceptionData(appEx, "GE0021", New String() {"シニア組合員残存率"})
                Else
                    Me.dSeniorRetire = Convert.ToDouble(Me.txtSeniorStayRate.Text)
                    Dim num5 As Decimal = Convert.ToDecimal(Me.dSeniorRetire)
                    Convert.ToInt32(PublicCommand.ToRoundDown(Me.dSeniorRetire, 1))
                    If ((num5 > 100) OrElse (num5 < 0)) Then
                        Me.txtSeniorStayRate.BackColor = Color.LightSalmon
                        PublicCommand.AddAppUnionExceptionData(appEx, "GE0021", New String() {"シニア組合員残存率"})
                    Else
                        Dim num6 As Decimal = (Convert.ToDecimal(Me.txtSeniorStayRate.Text) - Math.Truncate(Convert.ToDecimal(Me.txtSeniorStayRate.Text)))
                        If (num6.ToString.Length > 5) Then
                            Me.txtSeniorStayRate.BackColor = Color.LightSalmon
                            PublicCommand.AddAppUnionExceptionData(appEx, "GE0021", New String() {"シニア組合員残存率"})
                        End If
                    End If
                End If
                If (Not appEx Is Nothing) Then
                    Throw appEx
                End If
                Me.IsOk = True
                MyBase.Close()
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception2)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                Me.txtRevenueStrDate.BackColor = Color.LightYellow
                Me.txtRevenueEndDate.BackColor = Color.LightYellow
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub FrmCrewPlan_Load(ByVal sender As Object, ByVal e As EventArgs)
        End Sub

        Private Sub InitializeComponent()
            Me.grpFullTimeSalaryPayList = New GroupBox
            Me.label15 = New Label
            Me.label13 = New Label
            Me.label12 = New Label
            Me.label11 = New Label
            Me.label9 = New Label
            Me.label6 = New Label
            Me.label3 = New Label
            Me.label5 = New Label
            Me.label1 = New Label
            Me.label2 = New Label
            Me.label4 = New Label
            Me.lbltitle = New Label
            Me.label7 = New Label
            Me.groupBox1 = New GroupBox
            Me.label16 = New Label
            Me.label14 = New Label
            Me.label10 = New Label
            Me.label8 = New Label
            Me.label17 = New Label
            Me.label18 = New Label
            Me.label19 = New Label
            Me.label20 = New Label
            Me.txtSeniorAverage = New PersonalTextBox
            Me.txtSeniorStayRate = New PersonalTextBox
            Me.txtRevenueEndDate = New PersonalTextBox
            Me.txtRevenueStrDate = New PersonalTextBox
            Me.btnChange = New Button
            Me.btnOK = New Button
            Me.txtUnpromotionAverage = New PersonalTextBox
            Me.txtUnpromotionRate = New PersonalTextBox
            Me.txtCapPromotionAverage = New PersonalTextBox
            Me.txtNewStaffAverage = New PersonalTextBox
            Me.grpFullTimeSalaryPayList.SuspendLayout()
            Me.groupBox1.SuspendLayout()
            MyBase.SuspendLayout()
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label19)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label18)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label17)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label15)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label13)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label12)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label11)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label9)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label6)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.txtUnpromotionAverage)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label3)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.txtUnpromotionRate)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label5)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.txtCapPromotionAverage)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.txtNewStaffAverage)
            Me.grpFullTimeSalaryPayList.Controls.Add(Me.label1)
            Me.grpFullTimeSalaryPayList.Location = New Point(3, &H3E)
            Me.grpFullTimeSalaryPayList.Name = "grpFullTimeSalaryPayList"
            Me.grpFullTimeSalaryPayList.Size = New Size(&H21F, &H83)
            Me.grpFullTimeSalaryPayList.TabIndex = 0
            Me.grpFullTimeSalaryPayList.TabStop = False
            Me.label15.AutoSize = True
            Me.label15.Location = New Point(&H203, &H65)
            Me.label15.Name = "label15"
            Me.label15.Size = New Size(&H18, &H10)
            Me.label15.TabIndex = &H8D
            Me.label15.Text = "倍"
            Me.label13.AutoSize = True
            Me.label13.ForeColor = Color.Red
            Me.label13.Location = New Point(12, &H17)
            Me.label13.Name = "label13"
            Me.label13.Size = New Size(&H10, &H10)
            Me.label13.TabIndex = 140
            Me.label13.Text = "*"
            Me.label12.AutoSize = True
            Me.label12.ForeColor = Color.Red
            Me.label12.Location = New Point(&H1C, &H3E)
            Me.label12.Name = "label12"
            Me.label12.Size = New Size(&H10, &H10)
            Me.label12.TabIndex = &H8B
            Me.label12.Text = "*"
            Me.label11.AutoSize = True
            Me.label11.ForeColor = Color.Red
            Me.label11.Location = New Point(&H1C, &H65)
            Me.label11.Name = "label11"
            Me.label11.Size = New Size(&H10, &H10)
            Me.label11.TabIndex = &H8A
            Me.label11.Text = "*"
            Me.label9.AutoSize = True
            Me.label9.ForeColor = Color.Red
            Me.label9.Location = New Point(&H147, &H65)
            Me.label9.Name = "label9"
            Me.label9.Size = New Size(&H10, &H10)
            Me.label9.TabIndex = &H89
            Me.label9.Text = "*"
            Me.label6.AutoSize = True
            Me.label6.Location = New Point(50, &H65)
            Me.label6.Name = "label6"
            Me.label6.Size = New Size(&H88, &H10)
            Me.label6.TabIndex = &H88
            Me.label6.Text = "非組合員基準年齢"
            Me.label3.AutoSize = True
            Me.label3.Location = New Point(&H15D, &H65)
            Me.label3.Name = "label3"
            Me.label3.Size = New Size(&H68, &H10)
            Me.label3.TabIndex = &H86
            Me.label3.Text = "非組合員倍率"
            Me.label5.AutoSize = True
            Me.label5.Location = New Point(50, &H3E)
            Me.label5.Name = "label5"
            Me.label5.Size = New Size(&H88, &H10)
            Me.label5.TabIndex = &H83
            Me.label5.Text = "機長昇格基準年齢"
            Me.label1.AutoSize = True
            Me.label1.Location = New Point(&H22, &H17)
            Me.label1.Name = "label1"
            Me.label1.Size = New Size(&H98, &H10)
            Me.label1.TabIndex = &H80
            Me.label1.Text = "新入組合員基準年齢"
            Me.label2.AutoSize = True
            Me.label2.Location = New Point(&H185, &H18)
            Me.label2.Name = "label2"
            Me.label2.Size = New Size(&H18, &H10)
            Me.label2.TabIndex = 0
            Me.label2.Text = "～"
            Me.label4.AutoSize = True
            Me.label4.Location = New Point(&HD0, &H18)
            Me.label4.Name = "label4"
            Me.label4.Size = New Size(&H48, &H10)
            Me.label4.TabIndex = 0
            Me.label4.Text = "予想期間"
            Me.lbltitle.AutoSize = True
            Me.lbltitle.Location = New Point(&H13, &H18)
            Me.lbltitle.Name = "lbltitle"
            Me.lbltitle.Size = New Size(&H13, &H10)
            Me.lbltitle.TabIndex = 0
            Me.lbltitle.Text = "　"
            Me.label7.AutoSize = True
            Me.label7.Location = New Point(&H138, &H1D)
            Me.label7.Name = "label7"
            Me.label7.Size = New Size(&H8D, &H10)
            Me.label7.TabIndex = &H8A
            Me.label7.Text = "シニア組合員残存率"
            Me.groupBox1.Controls.Add(Me.label20)
            Me.groupBox1.Controls.Add(Me.label16)
            Me.groupBox1.Controls.Add(Me.label14)
            Me.groupBox1.Controls.Add(Me.label10)
            Me.groupBox1.Controls.Add(Me.label8)
            Me.groupBox1.Controls.Add(Me.txtSeniorAverage)
            Me.groupBox1.Controls.Add(Me.label7)
            Me.groupBox1.Controls.Add(Me.txtSeniorStayRate)
            Me.groupBox1.Location = New Point(3, &HC5)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New Size(&H21F, &H45)
            Me.groupBox1.TabIndex = 1
            Me.groupBox1.TabStop = False
            Me.label16.AutoSize = True
            Me.label16.Location = New Point(&H203, &H1D)
            Me.label16.Name = "label16"
            Me.label16.Size = New Size(&H18, &H10)
            Me.label16.TabIndex = &H8E
            Me.label16.Text = "％"
            Me.label14.AutoSize = True
            Me.label14.ForeColor = Color.Red
            Me.label14.Location = New Point(7, &H1D)
            Me.label14.Name = "label14"
            Me.label14.Size = New Size(&H10, &H10)
            Me.label14.TabIndex = &H8D
            Me.label14.Text = "*"
            Me.label10.AutoSize = True
            Me.label10.ForeColor = Color.Red
            Me.label10.Location = New Point(290, &H1D)
            Me.label10.Name = "label10"
            Me.label10.Size = New Size(&H10, &H10)
            Me.label10.TabIndex = &H8A
            Me.label10.Text = "*"
            Me.label8.AutoSize = True
            Me.label8.Location = New Point(&H1D, &H1D)
            Me.label8.Name = "label8"
            Me.label8.Size = New Size(&H9D, &H10)
            Me.label8.TabIndex = 140
            Me.label8.Text = "シニア組合員基準年齢"
            Me.label17.AutoSize = True
            Me.label17.Location = New Point(&HF8, &H17)
            Me.label17.Name = "label17"
            Me.label17.Size = New Size(&H18, &H10)
            Me.label17.TabIndex = &H8E
            Me.label17.Text = "歳"
            Me.label18.AutoSize = True
            Me.label18.Location = New Point(&HF8, &H3E)
            Me.label18.Name = "label18"
            Me.label18.Size = New Size(&H18, &H10)
            Me.label18.TabIndex = &H8F
            Me.label18.Text = "歳"
            Me.label19.AutoSize = True
            Me.label19.Location = New Point(&HF8, &H65)
            Me.label19.Name = "label19"
            Me.label19.Size = New Size(&H18, &H10)
            Me.label19.TabIndex = &H90
            Me.label19.Text = "歳"
            Me.label20.AutoSize = True
            Me.label20.Location = New Point(&HF8, &H1D)
            Me.label20.Name = "label20"
            Me.label20.Size = New Size(&H18, &H10)
            Me.label20.TabIndex = &H8F
            Me.label20.Text = "歳"
            Me.txtSeniorAverage.BackColor = Color.White
            Me.txtSeniorAverage.FieldAttribute = EFieldAttribute.NUMERIC
            Me.txtSeniorAverage.ImeMode = ImeMode.Disable
            Me.txtSeniorAverage.Location = New Point(&HC0, &H1A)
            Me.txtSeniorAverage.MaxLength = 2
            Me.txtSeniorAverage.Name = "txtSeniorAverage"
            Me.txtSeniorAverage.Require = True
            Me.txtSeniorAverage.Size = New Size(&H36, &H17)
            Me.txtSeniorAverage.TabIndex = 0
            Me.txtSeniorAverage.Tag = "シニア組合員基準年齢"
            Me.txtSeniorAverage.TextAlign = HorizontalAlignment.Right
            Me.txtSeniorStayRate.BackColor = Color.White
            Me.txtSeniorStayRate.FieldAttribute = EFieldAttribute.NONE
            Me.txtSeniorStayRate.ImeMode = ImeMode.Disable
            Me.txtSeniorStayRate.Location = New Point(&H1CB, &H1A)
            Me.txtSeniorStayRate.MaxLength = 6
            Me.txtSeniorStayRate.Name = "txtSeniorStayRate"
            Me.txtSeniorStayRate.Require = True
            Me.txtSeniorStayRate.Size = New Size(&H36, &H17)
            Me.txtSeniorStayRate.TabIndex = 1
            Me.txtSeniorStayRate.Tag = "シニア組合員残存率"
            Me.txtSeniorStayRate.TextAlign = HorizontalAlignment.Right
            Me.txtRevenueEndDate.BackColor = Color.LightYellow
            Me.txtRevenueEndDate.FieldAttribute = EFieldAttribute.NONE
            Me.txtRevenueEndDate.Location = New Point(&H1A3, &H15)
            Me.txtRevenueEndDate.Name = "txtRevenueEndDate"
            Me.txtRevenueEndDate.ReadOnly = True
            Me.txtRevenueEndDate.Require = False
            Me.txtRevenueEndDate.Size = New Size(&H61, &H17)
            Me.txtRevenueEndDate.TabIndex = 3
            Me.txtRevenueEndDate.TabStop = False
            Me.txtRevenueEndDate.TextAlign = HorizontalAlignment.Center
            Me.txtRevenueStrDate.BackColor = Color.LightYellow
            Me.txtRevenueStrDate.FieldAttribute = EFieldAttribute.NONE
            Me.txtRevenueStrDate.Location = New Point(&H11E, &H15)
            Me.txtRevenueStrDate.Name = "txtRevenueStrDate"
            Me.txtRevenueStrDate.ReadOnly = True
            Me.txtRevenueStrDate.Require = False
            Me.txtRevenueStrDate.Size = New Size(&H61, &H17)
            Me.txtRevenueStrDate.TabIndex = 0
            Me.txtRevenueStrDate.TabStop = False
            Me.txtRevenueStrDate.TextAlign = HorizontalAlignment.Center
            Me.btnChange.Location = New Point(&H126, &H11C)
            Me.btnChange.Name = "btnChange"
            Me.btnChange.Size = New Size(&H74, &H20)
            Me.btnChange.TabIndex = 3
            Me.btnChange.Text = "キャンセル"
            Me.btnChange.UseVisualStyleBackColor = True
            AddHandler Me.btnChange.Click, New EventHandler(AddressOf Me.btnChange_Click)
            Me.btnOK.Location = New Point(&H8B, &H11C)
            Me.btnOK.Name = "btnOK"
            Me.btnOK.Size = New Size(&H74, &H20)
            Me.btnOK.TabIndex = 2
            Me.btnOK.Text = "OK"
            Me.btnOK.UseVisualStyleBackColor = True
            AddHandler Me.btnOK.Click, New EventHandler(AddressOf Me.btnOK_Click)
            Me.txtUnpromotionAverage.BackColor = Color.White
            Me.txtUnpromotionAverage.FieldAttribute = EFieldAttribute.NUMERIC
            Me.txtUnpromotionAverage.ImeMode = ImeMode.Disable
            Me.txtUnpromotionAverage.Location = New Point(&HC0, &H62)
            Me.txtUnpromotionAverage.MaxLength = 2
            Me.txtUnpromotionAverage.Name = "txtUnpromotionAverage"
            Me.txtUnpromotionAverage.Require = True
            Me.txtUnpromotionAverage.Size = New Size(&H36, &H17)
            Me.txtUnpromotionAverage.TabIndex = 2
            Me.txtUnpromotionAverage.Tag = "非組合員基準年齢"
            Me.txtUnpromotionAverage.TextAlign = HorizontalAlignment.Right
            Me.txtUnpromotionRate.BackColor = Color.White
            Me.txtUnpromotionRate.FieldAttribute = EFieldAttribute.NONE
            Me.txtUnpromotionRate.ImeMode = ImeMode.Disable
            Me.txtUnpromotionRate.Location = New Point(&H1CB, &H62)
            Me.txtUnpromotionRate.MaxLength = 6
            Me.txtUnpromotionRate.Name = "txtUnpromotionRate"
            Me.txtUnpromotionRate.Require = True
            Me.txtUnpromotionRate.Size = New Size(&H36, &H17)
            Me.txtUnpromotionRate.TabIndex = 3
            Me.txtUnpromotionRate.Tag = "非組合員倍率"
            Me.txtUnpromotionRate.TextAlign = HorizontalAlignment.Right
            Me.txtCapPromotionAverage.BackColor = Color.White
            Me.txtCapPromotionAverage.FieldAttribute = EFieldAttribute.NUMERIC
            Me.txtCapPromotionAverage.ImeMode = ImeMode.Disable
            Me.txtCapPromotionAverage.Location = New Point(&HC0, &H3B)
            Me.txtCapPromotionAverage.MaxLength = 2
            Me.txtCapPromotionAverage.Name = "txtCapPromotionAverage"
            Me.txtCapPromotionAverage.Require = True
            Me.txtCapPromotionAverage.Size = New Size(&H36, &H17)
            Me.txtCapPromotionAverage.TabIndex = 1
            Me.txtCapPromotionAverage.Tag = "機長昇格基準年齢"
            Me.txtCapPromotionAverage.TextAlign = HorizontalAlignment.Right
            Me.txtNewStaffAverage.BackColor = Color.White
            Me.txtNewStaffAverage.FieldAttribute = EFieldAttribute.NUMERIC
            Me.txtNewStaffAverage.ImeMode = ImeMode.Disable
            Me.txtNewStaffAverage.Location = New Point(&HC0, 20)
            Me.txtNewStaffAverage.MaxLength = 2
            Me.txtNewStaffAverage.Name = "txtNewStaffAverage"
            Me.txtNewStaffAverage.Require = True
            Me.txtNewStaffAverage.Size = New Size(&H36, &H17)
            Me.txtNewStaffAverage.TabIndex = 0
            Me.txtNewStaffAverage.Tag = "新入組合員基準年齢"
            Me.txtNewStaffAverage.TextAlign = HorizontalAlignment.Right
            MyBase.AutoScaleDimensions = New SizeF(9.0!, 16.0!)
            MyBase.AutoScaleMode = AutoScaleMode.Font
            MyBase.ClientSize = New Size(550, &H14C)
            MyBase.Controls.Add(Me.groupBox1)
            MyBase.Controls.Add(Me.txtRevenueEndDate)
            MyBase.Controls.Add(Me.txtRevenueStrDate)
            MyBase.Controls.Add(Me.lbltitle)
            MyBase.Controls.Add(Me.label2)
            MyBase.Controls.Add(Me.btnChange)
            MyBase.Controls.Add(Me.label4)
            MyBase.Controls.Add(Me.btnOK)
            MyBase.Controls.Add(Me.grpFullTimeSalaryPayList)
            Me.Font = New Font("MS UI Gothic", 12.0!)
            MyBase.FormBorderStyle = FormBorderStyle.FixedToolWindow
            MyBase.Margin = New Padding(4)
            MyBase.MaximizeBox = False
            MyBase.MinimizeBox = False
            MyBase.Name = "FrmCrewPlan"
            MyBase.ShowInTaskbar = False
            MyBase.StartPosition = FormStartPosition.CenterScreen
            Me.Text = "乗員計画　新規登録"
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.FrmCrewPlan_Load)
            Me.grpFullTimeSalaryPayList.ResumeLayout(False)
            Me.grpFullTimeSalaryPayList.PerformLayout()
            Me.groupBox1.ResumeLayout(False)
            Me.groupBox1.PerformLayout()
            MyBase.ResumeLayout(False)
            MyBase.PerformLayout()
        End Sub


        ' Fields
        Private btnChange As Button
        Private btnOK As Button
        Private components As IContainer
        Public dSeniorRetire As Double
        Public dUnpromotionRate As Double
        Private groupBox1 As GroupBox
        Private grpFullTimeSalaryPayList As GroupBox
        Public iCapPromotionAverage As Integer
        Public iNewStaffAverage As Integer
        Public iSeniorAverage As Integer
        Public IsOk As Boolean
        Public iUnpromotionAverage As Integer
        Private label1 As Label
        Private label10 As Label
        Private label11 As Label
        Private label12 As Label
        Private label13 As Label
        Private label14 As Label
        Private label15 As Label
        Private label16 As Label
        Private label17 As Label
        Private label18 As Label
        Private label19 As Label
        Private label2 As Label
        Private label20 As Label
        Private label3 As Label
        Private label4 As Label
        Private label5 As Label
        Private label6 As Label
        Private label7 As Label
        Private label8 As Label
        Private label9 As Label
        Private lbltitle As Label
        Private txtCapPromotionAverage As PersonalTextBox
        Private txtNewStaffAverage As PersonalTextBox
        Private txtRevenueEndDate As PersonalTextBox
        Private txtRevenueStrDate As PersonalTextBox
        Private txtSeniorAverage As PersonalTextBox
        Private txtSeniorStayRate As PersonalTextBox
        Private txtUnpromotionAverage As PersonalTextBox
        Private txtUnpromotionRate As PersonalTextBox
    End Class
End Namespace
