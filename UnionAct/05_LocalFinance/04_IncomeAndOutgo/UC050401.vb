Imports C1.Win.C1FlexGrid
Imports C1.Win.C1FlexGrid.Util.BaseControls
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Framework.UnionException
Imports UnionAct.NSCLMsg
Imports UnionAct.Framework.Command
Imports UnionAct.GUI.UnionComponent
Imports UnionAct.GUI.Common
Imports UnionAct.Framework.Mapping
Imports UnionAct.Business.RevenueExpenditure
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.RevenueExpenditure
Imports UnionAct.GUI.RevenueExpenditure.UnionForm

Public Class UC050401
    Inherits RevenueExpenditureBase
    ' Methods
    Public Sub New()
        Me.InitializeComponent()
    End Sub

    Public Sub New(ByVal IsContinue As Boolean)
        Me.InitializeComponent()
    End Sub

    Public Sub ActionAfterResotreUserControl()
        Try
            Me.SearchListData()
            Me.txtRevenueEndDate.BackColor = Color.LightYellow
            Me.Cursor = Cursors.Default
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub btnSalaryRenew_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSalaryRenew.Click
        Try
            Dim str As String = Me.dtpRevenueStartDate.Value.Year.ToString
            Dim str2 As String = Me.dtpRevenueStartDate.Value.Month.ToString("00")
            Dim str3 As String = Me.dtpRevenueStartDate.Value.Day.ToString("00")
            Dim strRevenueStart As String = (str & str2 & str3)
            If (Me.dTRevenueExpenditureList.Rows.Count > 0) Then
                If Not Me.CheckRevenueStr(strRevenueStart) Then
                    If Me.dTRevenueExpenditureList.Rows.Item(0).Item("k_revenue_seton").ToString.Equals("1") Then
                        Dim str5 As String = DateTime.ParseExact(Me.dtpRevenueStartDate.Value.ToString("yyyyMMdd"), "yyyyMMdd", Nothing).ToString("yyyy" & "îN" & "MM" & "åé" & "dd" & "ì˙")
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0176", New String() {str5})
                    End If
                    If (CLMsg.Show("GQ0043", str, str2, str3) = DialogResult.No) Then
                        Return
                    End If
                    Me.DeleteRevenueExpenditure(strRevenueStart)
                Else
                    Me.CheckDuplication(strRevenueStart)
                    Me.CheckSalaryRenew()
                    If (CLMsg.Show("GQ0044", "ó\ëzäJénîNåéì˙", str, str2, str3, "é˚ì¸ó\ëz") = DialogResult.No) Then
                        Return
                    End If
                End If
            End If
            Me.ValidateFields()
            If Me.txtRevenueTitle.Text = "" Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0006", "ëËñ⁄")
            End If
            If (PublicCommand.GetByteLength(Me.txtRevenueTitle.Text) > 50) Then
                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0137", New String() {"ëËñ⁄", "50"})
            End If
            MyBase.RevenueTitle = Me.txtRevenueTitle.Text
            MyBase.RevenueStartDate = strRevenueStart
            Dim text As String = Me.txtRevenueEndDate.Text
            MyBase.RevenueEndDate = ([text].Substring(0, 4) & [text].Substring(5, 2) & [text].Substring(8, 2))
            Me.MoveToIncomeExpect(True)
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
            'Throw exception2
            MsgBox(exception2.ToString)
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub CellStyleSetting()
        Try
            Dim style As CellStyle = Nothing
            style = Me.flxRevenueExpenditureList.Styles.Add("col_link")
            style.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Underline)
            style.DataType = GetType(Long)
            style.TextAlign = TextAlignEnum.CenterCenter
            style.BackColor = Color.White
            style.ForeColor = Color.Blue
            style = Me.flxRevenueExpenditureList.Styles.Add("col_nolink")
            style.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!)
            style.DataType = GetType(Long)
            style.TextAlign = TextAlignEnum.CenterCenter
            style.BackColor = Color.White
            style.ForeColor = SystemColors.WindowText
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub CheckDuplication(ByVal strRevenueStart As String)
        Try
            Dim i As Integer
            For i = 0 To Me.dTRevenueExpenditureList.Rows.Count - 1
                Dim str As String = Me.dTRevenueExpenditureList.Rows.Item(i).Item("d_revenue_str").ToString
                If strRevenueStart.Equals(str) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0156", New String() {strRevenueStart.Substring(0, 4), strRevenueStart.Substring(4, 2), strRevenueStart.Substring(6, 2), "é˚éxó\ëzÉfÅ[É^"})
                End If
            Next i
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Function CheckRevenueStr(ByVal strRevenueStart As String) As Boolean
        Dim flag As Boolean
        Try
            Dim str As String = Me.dTRevenueExpenditureList.Rows.Item(0).Item("d_revenue_str").ToString
            If strRevenueStart.Equals(str) Then
                Return False
            End If
            flag = True
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return flag
    End Function

    Private Sub CheckSalaryRenew()
        Try
            If (Me.flxRevenueExpenditureList.Rows.Count > 1) Then
                If ((Me.dTRevenueExpenditureList.Rows.Item(0).Item("k_revenue_expenditure").ToString.Equals("0") OrElse Me.dTRevenueExpenditureList.Rows.Item(0).Item("k_revenue_member").ToString.Equals("0")) OrElse (Me.dTRevenueExpenditureList.Rows.Item(0).Item("k_revenue_allotted_charge").ToString.Equals("0") OrElse Me.dTRevenueExpenditureList.Rows.Item(0).Item("k_revenue_budgetary_process").ToString.Equals("0"))) Then
                    Dim str As String = Me.dTRevenueExpenditureList.Rows.Item(0).Item("l_title").ToString
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0139", New String() {str})
                End If
                Dim str2 As String = (Me.dtpRevenueStartDate.Value.Year.ToString & Me.dtpRevenueStartDate.Value.Month.ToString("00") & Me.dtpRevenueStartDate.Value.Day.ToString("00"))
                Dim strB As String = Me.dTRevenueExpenditureList.Rows.Item(0).Item("d_revenue_str").ToString
                If (str2.CompareTo(strB) <= 0) Then
                    Dim str4 As String = (DateTime.ParseExact(Me.dTRevenueExpenditureList.Rows.Item(0).Item("ó\ëzäJénì˙").ToString, "yyyy/MM/dd", Nothing).AddYears(1).ToString("yyyMMdd").Substring(0, 4) & "îN" & "08" & "åé" & "01" & "ì˙")
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0121", New String() {"ó\ëzäJénîNåéì˙", str4})
                End If
            End If
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub CtlRevenueExpenditureList_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            Me.Cursor = Cursors.WaitCursor
            MyBase.IsGetEntryRight = MDFinanceCommon.GetEntryPower("UC050401")
            MyBase.IsPrintRight = MDFinanceCommon.GetPrintPower("UC050401")
            MyBase.IsReferenceRight = MDFinanceCommon.GetReferencePower("UC050401")
            Me.CellStyleSetting()
            Me.SearchListData()
            Me.SetProperty()
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
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub DeleteRevenueExpenditure(ByVal strRevenueStr As String)
        Try
            Dim command As New RevenueExpenditureListCommand
            command.DeleteRevenueExpenditureCommand(strRevenueStr)
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub dtpRevenueStartDate_ValueChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dtpRevenueStartDate.ValueChanged
        Try
            Me.dtpRevenueStartDate.Value = DateTime.ParseExact((Me.dtpRevenueStartDate.Value.Year.ToString & "/08/01"), "yyyy/MM/dd", Nothing)
            Dim str2 As String = (Me.dtpRevenueStartDate.Value.AddYears(1).ToString("yyyMMdd").Substring(0, 4) & "îN" & "07" & "åé" & "31" & "ì˙")
            Me.txtRevenueEndDate.Text = str2
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

    Private Sub FlexGridSetting()
        Try
            Dim dicColWidthPair As New Dictionary(Of String, Integer)
            dicColWidthPair.Add("ëËñ⁄", 80)
            dicColWidthPair.Add("ó\ëzäJénì˙", 90)
            dicColWidthPair.Add("ó\ëzèIóπì˙", 90)
            dicColWidthPair.Add("é˚ì¸ó\ëzèÛãµ", 110)
            dicColWidthPair.Add("èÊàıåvâÊèÛãµ", 110)
            dicColWidthPair.Add("ï™íSã‡èÛãµ", 110)
            dicColWidthPair.Add("ó\éZìoò^èÛãµ", 110)
            dicColWidthPair.Add("èCê≥ó\éZèÛãµ", 110)
            dicColWidthPair.Add("íSìñé“", 110)
            dicColWidthPair.Add("ìoò^ì˙", 90)
            MDFinanceCommon.SetColsWidth(Me.flxRevenueExpenditureList, dicColWidthPair)
            MDFinanceCommon.AdjustTextAlign(Me.flxRevenueExpenditureList)
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub flxRevenueExpenditureList_Click(ByVal sender As Object, ByVal e As EventArgs) Handles flxRevenueExpenditureList.Click
        Try
            If (MDFinanceCommon.CheckMouseCursorPoint(Me.flxRevenueExpenditureList) AndAlso Me.IsOnHyperLinkCell) Then
                Select Case Me.flxRevenueExpenditureList.Col
                    Case 4
                        Me.MoveToIncomeExpect(False)
                        Return
                    Case 5
                        Me.MoveToCrewPlan()
                        Return
                    Case 6
                        Me.MoveToAllottedCharge()
                        Return
                    Case 7
                        Me.MoveToBudgetaryProcess(False)
                        Return
                    Case 8
                        Me.MoveToBudgetaryProcess(True)
                        Return
                End Select
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
            'Throw exception2
            MsgBox(exception2.ToString)
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub flxRevenueExpenditureList_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles flxRevenueExpenditureList.MouseMove
        Try
            Dim grid As C1FlexGrid = DirectCast(sender, C1FlexGrid)
            Dim info As HitTestInfo = grid.HitTest
            If ((info.Type.Equals(HitTestTypeEnum.Cell) AndAlso (Not grid.GetCellStyle(info.Row, info.Column) Is Nothing)) AndAlso grid.GetCellStyle(info.Row, info.Column).ForeColor.Equals(Color.Blue)) Then
                Me.Cursor = Cursors.Hand
            Else
                Me.Cursor = Cursors.Default
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

    Private Function GetIsChangeFlg(ByVal IsRevise As Boolean) As Boolean
        Dim flag As Boolean
        Try
            If (Me.flxRevenueExpenditureList.Row = 1) Then
                If Not IsRevise Then
                    If Me.dTRevenueExpenditureList.Rows.Item(0).Item("k_revenue_seton").ToString.Equals("1") Then
                        Return False
                    End If
                    Return True
                End If
                Return True
            End If
            flag = False
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return flag
    End Function

    Private Function GetLastRevenueStart() As String
        Dim str2 As String
        Try
            If (Me.dTRevenueExpenditureList.Rows.Count <= 0) Then
                Return ""
            End If
            str2 = Me.dTRevenueExpenditureList.Rows.Item(0).Item("d_revenue_str").ToString
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return str2
    End Function

    Private Function GetLastRevenueStart2() As String
        Dim str2 As String
        Try
            Dim str As String = Nothing
            If (Me.GetRevExpend("d_revenue_str", 1) Is Nothing) Then
                str = ""
            Else
                str = Me.GetRevExpend("d_revenue_str", 1).ToString
            End If
            str2 = str
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return str2
    End Function

    Private Function GetRevExpend(ByVal strColumnsName As String, ByVal iBackLine As Integer) As Object
        Dim obj2 As Object
        Try
            Dim num As Integer = ((Me.flxRevenueExpenditureList.Row - 1) + iBackLine)
            If ((num < 0) OrElse (num >= (Me.flxRevenueExpenditureList.Rows.Count - 1))) Then
                Return Nothing
            End If
            obj2 = Me.dTRevenueExpenditureList.Rows.Item(num).Item(strColumnsName)
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return obj2
    End Function

    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UC050401))
        Me.grpExpenditure = New System.Windows.Forms.GroupBox
        Me.label8 = New System.Windows.Forms.Label
        Me.label5 = New System.Windows.Forms.Label
        Me.label7 = New System.Windows.Forms.Label
        Me.lblAsta1 = New System.Windows.Forms.Label
        Me.txtRevenueTitle = New UnionAct.GUI.UnionComponent.PersonalTextBox
        Me.label6 = New System.Windows.Forms.Label
        Me.txtRevenueEndDate = New UnionAct.GUI.UnionComponent.PersonalTextBox
        Me.label3 = New System.Windows.Forms.Label
        Me.label1 = New System.Windows.Forms.Label
        Me.dtpRevenueStartDate = New System.Windows.Forms.DateTimePicker
        Me.label2 = New System.Windows.Forms.Label
        Me.btnSalaryRenew = New System.Windows.Forms.Button
        Me.grpRevenueExpenditureList = New System.Windows.Forms.GroupBox
        Me.label4 = New System.Windows.Forms.Label
        Me.flxRevenueExpenditureList = New C1.Win.C1FlexGrid.C1FlexGrid
        Me.Label11 = New System.Windows.Forms.Label
        Me.grpExpenditure.SuspendLayout()
        Me.grpRevenueExpenditureList.SuspendLayout()
        CType(Me.flxRevenueExpenditureList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'grpExpenditure
        '
        Me.grpExpenditure.Controls.Add(Me.label8)
        Me.grpExpenditure.Controls.Add(Me.label5)
        Me.grpExpenditure.Controls.Add(Me.label7)
        Me.grpExpenditure.Controls.Add(Me.lblAsta1)
        Me.grpExpenditure.Controls.Add(Me.txtRevenueTitle)
        Me.grpExpenditure.Controls.Add(Me.label6)
        Me.grpExpenditure.Controls.Add(Me.txtRevenueEndDate)
        Me.grpExpenditure.Controls.Add(Me.label3)
        Me.grpExpenditure.Controls.Add(Me.label1)
        Me.grpExpenditure.Controls.Add(Me.dtpRevenueStartDate)
        Me.grpExpenditure.Controls.Add(Me.label2)
        Me.grpExpenditure.Controls.Add(Me.btnSalaryRenew)
        Me.grpExpenditure.Location = New System.Drawing.Point(171, 572)
        Me.grpExpenditure.Name = "grpExpenditure"
        Me.grpExpenditure.Size = New System.Drawing.Size(677, 177)
        Me.grpExpenditure.TabIndex = 1
        Me.grpExpenditure.TabStop = False
        Me.grpExpenditure.Text = "é˚ì¸ó\ëzêVãKê›íË"
        '
        'label8
        '
        Me.label8.AutoSize = True
        Me.label8.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.label8.Location = New System.Drawing.Point(495, 98)
        Me.label8.Name = "label8"
        Me.label8.Size = New System.Drawing.Size(156, 13)
        Me.label8.TabIndex = 162
        Me.label8.Text = "(07åé31ì˙ÇÃì˙ïtÇ…Ç»ÇËÇ‹Ç∑)"
        '
        'label5
        '
        Me.label5.AutoSize = True
        Me.label5.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.label5.Location = New System.Drawing.Point(209, 98)
        Me.label5.Name = "label5"
        Me.label5.Size = New System.Drawing.Size(156, 13)
        Me.label5.TabIndex = 161
        Me.label5.Text = "(08åé01ì˙ÇÃì˙ïtÇ…Ç»ÇËÇ‹Ç∑)"
        '
        'label7
        '
        Me.label7.AutoSize = True
        Me.label7.ForeColor = System.Drawing.Color.Red
        Me.label7.Location = New System.Drawing.Point(71, 74)
        Me.label7.Name = "label7"
        Me.label7.Size = New System.Drawing.Size(16, 16)
        Me.label7.TabIndex = 159
        Me.label7.Text = "*"
        '
        'lblAsta1
        '
        Me.lblAsta1.AutoSize = True
        Me.lblAsta1.ForeColor = System.Drawing.Color.Red
        Me.lblAsta1.Location = New System.Drawing.Point(147, 25)
        Me.lblAsta1.Name = "lblAsta1"
        Me.lblAsta1.Size = New System.Drawing.Size(16, 16)
        Me.lblAsta1.TabIndex = 158
        Me.lblAsta1.Text = "*"
        '
        'txtRevenueTitle
        '
        Me.txtRevenueTitle.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
        Me.txtRevenueTitle.Location = New System.Drawing.Point(209, 22)
        Me.txtRevenueTitle.MaxLength = 8
        Me.txtRevenueTitle.Name = "txtRevenueTitle"
        Me.txtRevenueTitle.Require = True
        Me.txtRevenueTitle.Size = New System.Drawing.Size(133, 23)
        Me.txtRevenueTitle.TabIndex = 2
        Me.txtRevenueTitle.Tag = "ëËñ⁄"
        '
        'label6
        '
        Me.label6.AutoSize = True
        Me.label6.Location = New System.Drawing.Point(163, 25)
        Me.label6.Name = "label6"
        Me.label6.Size = New System.Drawing.Size(40, 16)
        Me.label6.TabIndex = 130
        Me.label6.Text = "ëËñ⁄"
        '
        'txtRevenueEndDate
        '
        Me.txtRevenueEndDate.BackColor = System.Drawing.Color.LightYellow
        Me.txtRevenueEndDate.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
        Me.txtRevenueEndDate.Location = New System.Drawing.Point(498, 72)
        Me.txtRevenueEndDate.Name = "txtRevenueEndDate"
        Me.txtRevenueEndDate.ReadOnly = True
        Me.txtRevenueEndDate.Require = False
        Me.txtRevenueEndDate.Size = New System.Drawing.Size(121, 23)
        Me.txtRevenueEndDate.TabIndex = 4
        Me.txtRevenueEndDate.TabStop = False
        Me.txtRevenueEndDate.Text = "2006îN07åé31ì˙"
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(373, 75)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(120, 16)
        Me.label3.TabIndex = 124
        Me.label3.Text = "ó\ëzèIóπîNåéì˙"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(350, 77)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(24, 16)
        Me.label1.TabIndex = 123
        Me.label1.Text = "Å`"
        '
        'dtpRevenueStartDate
        '
        Me.dtpRevenueStartDate.Location = New System.Drawing.Point(209, 72)
        Me.dtpRevenueStartDate.Name = "dtpRevenueStartDate"
        Me.dtpRevenueStartDate.Size = New System.Drawing.Size(133, 23)
        Me.dtpRevenueStartDate.TabIndex = 3
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(84, 75)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(120, 16)
        Me.label2.TabIndex = 119
        Me.label2.Text = "ó\ëzäJénîNåéì˙"
        '
        'btnSalaryRenew
        '
        Me.btnSalaryRenew.Location = New System.Drawing.Point(286, 134)
        Me.btnSalaryRenew.Name = "btnSalaryRenew"
        Me.btnSalaryRenew.Size = New System.Drawing.Size(116, 32)
        Me.btnSalaryRenew.TabIndex = 5
        Me.btnSalaryRenew.Text = "êVãKçÏê¨"
        Me.btnSalaryRenew.UseVisualStyleBackColor = True
        '
        'grpRevenueExpenditureList
        '
        Me.grpRevenueExpenditureList.Controls.Add(Me.label4)
        Me.grpRevenueExpenditureList.Controls.Add(Me.flxRevenueExpenditureList)
        Me.grpRevenueExpenditureList.Location = New System.Drawing.Point(20, 84)
        Me.grpRevenueExpenditureList.Name = "grpRevenueExpenditureList"
        Me.grpRevenueExpenditureList.Size = New System.Drawing.Size(982, 470)
        Me.grpRevenueExpenditureList.TabIndex = 6
        Me.grpRevenueExpenditureList.TabStop = False
        Me.grpRevenueExpenditureList.Text = "é˚éxó\ëzàÍóó"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(490, 443)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(485, 16)
        Me.label4.TabIndex = 125
        Me.label4.Text = "Å¶èCê≥ó\éZÇçsÇ§Ç…ÇÕó\éZìoò^âÊñ Ç≈Åuó\éZämíËÅvÇçsÇ§ïKóvÇ™Ç†ÇËÇ‹Ç∑ÅB"
        '
        'flxRevenueExpenditureList
        '
        Me.flxRevenueExpenditureList.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
        Me.flxRevenueExpenditureList.AllowEditing = False
        Me.flxRevenueExpenditureList.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
        Me.flxRevenueExpenditureList.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
        Me.flxRevenueExpenditureList.ColumnInfo = "10,1,0,0,0,110,Columns:"
        Me.flxRevenueExpenditureList.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
        Me.flxRevenueExpenditureList.KeyActionEnter = C1.Win.C1FlexGrid.KeyActionEnum.None
        Me.flxRevenueExpenditureList.Location = New System.Drawing.Point(17, 42)
        Me.flxRevenueExpenditureList.Name = "flxRevenueExpenditureList"
        Me.flxRevenueExpenditureList.Rows.Count = 1
        Me.flxRevenueExpenditureList.Rows.DefaultSize = 22
        Me.flxRevenueExpenditureList.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
        Me.flxRevenueExpenditureList.Size = New System.Drawing.Size(947, 391)
        Me.flxRevenueExpenditureList.StyleInfo = resources.GetString("flxRevenueExpenditureList.StyleInfo")
        Me.flxRevenueExpenditureList.TabIndex = 7
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
        Me.Label11.Padding = New System.Windows.Forms.Padding(10, 0, 10, 0)
        Me.Label11.Size = New System.Drawing.Size(630, 35)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "é˚éxó\ëz"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'UC050401
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.grpExpenditure)
        Me.Controls.Add(Me.grpRevenueExpenditureList)
        Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.Name = "UC050401"
        Me.Size = New System.Drawing.Size(1026, 759)
        Me.grpExpenditure.ResumeLayout(False)
        Me.grpExpenditure.PerformLayout()
        Me.grpRevenueExpenditureList.ResumeLayout(False)
        Me.grpRevenueExpenditureList.PerformLayout()
        CType(Me.flxRevenueExpenditureList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private Function IsOnHyperLinkCell() As Boolean
        Dim flag2 As Boolean
        Try
            Dim flag As Boolean = False
            Dim row As Integer = Me.flxRevenueExpenditureList.Row
            Dim col As Integer = Me.flxRevenueExpenditureList.Col
            Dim cellStyle As CellStyle = Me.flxRevenueExpenditureList.GetCellStyle(row, col)
            If (((Not cellStyle Is Nothing) AndAlso (Not cellStyle.Name Is Nothing)) AndAlso cellStyle.Name.Equals("col_link")) Then
                flag = True
            End If
            flag2 = flag
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
        Return flag2
    End Function

    Private Sub MoveToAllottedCharge()
        Try
            Dim str As String = ""
            Dim flag As Boolean = False
            If Not Me.GetRevExpend("k_revenue_allotted_charge", 0).ToString.Equals("1") Then
                flag = True
                str = "êVãKìoò^"
            Else
                flag = False
                str = "è⁄ç◊ï\é¶"
            End If
            Dim charge As New CtlAllottedCharge(Me.GetRevExpend("d_revenue_str", 0).ToString, Me.GetRevExpend("d_revenue_end", 0).ToString, Me.GetLastRevenueStart2, flag, Me.GetIsChangeFlg(False), Me.GetRevExpend("l_title", 0).ToString, Me.GetRevExpend("d_up", 0), MyBase.IsReferenceRight, MyBase.IsGetEntryRight, MyBase.IsPrintRight, Me)
            Utilities.OverlayUserControl(ParentForm.Controls(MDConst.MAIN_PANEL_ID), ("ï™íSã‡èÛãµ" & " - " & str), New UserControl() {charge})
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub MoveToBudgetaryProcess(ByVal IsRevise As Boolean)
        Try
            Dim str As String = ""
            Dim str2 As String = ""
            Dim flag As Boolean = False
            Dim strColumnsName As String = ""
            If Not IsRevise Then
                strColumnsName = "k_revenue_budgetary_process"
                str2 = "ó\éZìoò^" & " - "
            Else
                strColumnsName = "k_revenue_budgetary_revise"
                str2 = "èCê≥ó\éZ" & " - "
            End If
            If Not Me.GetRevExpend(strColumnsName, 0).ToString.Equals("1") Then
                flag = True
                str = "êVãKìoò^"
            Else
                flag = False
                str = "è⁄ç◊ï\é¶"
            End If
            Dim process As New CtlBudgetaryProcess(IsRevise, Me.GetRevExpend("d_revenue_str", 0).ToString, Me.GetRevExpend("d_revenue_end", 0).ToString, Me.GetLastRevenueStart2, flag, Me.GetIsChangeFlg(IsRevise), Me.GetRevExpend("l_title", 0).ToString, Convert.ToInt64(Me.GetRevExpend("s_revise_revenue_ttl", 0)), Convert.ToInt64(Me.GetRevExpend("s_budget_sub", 0)), Convert.ToInt64(Me.GetRevExpend("s_budget_total", 0)), Convert.ToInt64(Me.GetRevExpend("s_revise_budget_sub", 0)), Convert.ToInt64(Me.GetRevExpend("s_revise_budget_total", 0)), Me.GetRevExpend("d_up", 0), MyBase.IsReferenceRight, MyBase.IsGetEntryRight, MyBase.IsPrintRight, Me)
            Utilities.OverlayUserControl(ParentForm.Controls(MDConst.MAIN_PANEL_ID), (str2 & str), New UserControl() {process})
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub MoveToCrewPlan()
        Try
            Dim str As String = ""
            Dim flag As Boolean = False
            Dim rowArray As DataRow() = Me.dTRevenueExpenditureList.Select(("d_revenue_str = " & Me.GetRevExpend("d_revenue_str", 0).ToString))
            Dim table As DataTable = New RevenueExpenditureListMap().CreateDataTablePhysName("revenue_expenditure")
            table.ImportRow(rowArray(0))
            If Not Me.GetRevExpend("k_revenue_member", 0).ToString.Equals("1") Then
                Dim revExpend As Object = Me.GetRevExpend("s_new_staff_average", 1)
                Dim obj3 As Object = Me.GetRevExpend("s_cap_promotion_average", 1)
                Dim obj4 As Object = Me.GetRevExpend("s_unpromotion_rate", 1)
                Dim obj5 As Object = Me.GetRevExpend("s_unpromotion_average", 1)
                Dim obj6 As Object = Me.GetRevExpend("s_senior_stay_rate", 1)
                Dim obj7 As Object = Me.GetRevExpend("s_senior_average", 1)
                If (revExpend Is Nothing) Then
                    revExpend = &H19
                End If
                If (obj3 Is Nothing) Then
                    obj3 = 40
                End If
                If (obj4 Is Nothing) Then
                    obj4 = 6
                End If
                If (obj5 Is Nothing) Then
                    obj5 = 50
                End If
                If (obj7 Is Nothing) Then
                    obj7 = 60
                End If
                Dim plan As New FrmCrewPlan(Me.GetRevExpend("l_title", 0), Me.GetRevExpend("d_revenue_str", 0), Me.GetRevExpend("d_revenue_end", 0), revExpend, obj3, obj4, obj5, obj6, obj7)
                plan.ShowDialog()
                If plan.IsOk Then
                    str = "êVãKìoò^"
                    table.Rows.Item(0).Item("s_new_staff_average") = plan.iNewStaffAverage
                    table.Rows.Item(0).Item("s_cap_promotion_average") = plan.iCapPromotionAverage
                    table.Rows.Item(0).Item("s_unpromotion_rate") = plan.dUnpromotionRate
                    table.Rows.Item(0).Item("s_unpromotion_average") = plan.iUnpromotionAverage
                    table.Rows.Item(0).Item("s_senior_stay_rate") = plan.dSeniorRetire
                    table.Rows.Item(0).Item("s_senior_average") = plan.iSeniorAverage
                    If (Me.GetRevExpend("s_unpromotion_persons", 1) Is Nothing) Then
                        table.Rows.Item(0).Item("s_unpromotion_persons") = 0
                    Else
                        table.Rows.Item(0).Item("s_unpromotion_persons") = Me.GetRevExpend("s_unpromotion_persons", 1)
                    End If
                    flag = True
                End If
                plan.Dispose()
            Else
                str = "è⁄ç◊ï\é¶"
                flag = True
            End If
            If flag Then
                Dim flag2 As Boolean = False
                If Not Me.GetRevExpend("k_revenue_member", 0).ToString.Equals("1") Then
                    flag2 = True
                End If
                Dim plan2 As New CtlCrewPlan(MyBase.IsReferenceRight, MyBase.IsGetEntryRight, MyBase.IsPrintRight, flag2, Me.GetIsChangeFlg(False), table, Me.GetLastRevenueStart2, Me)
                Utilities.OverlayUserControl(ParentForm.Controls(MDConst.MAIN_PANEL_ID), ("èÊàıåvâÊèÛãµ" & " - " & str), New UserControl() {plan2})
            End If
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub MoveToIncomeExpect(ByVal IsNewFlg As Boolean)
        Try
            Dim num As Long
            Dim str As String = ""
            Dim isChangeFlg As Boolean = False
            Dim str2 As String = ""
            Dim str3 As String = ""
            Dim str4 As String = ""
            Dim lastRevenueStart As String = ""
            Dim revExpend As Object = Nothing
            If IsNewFlg Then
                str = "êVãKìoò^"
                isChangeFlg = True
                str2 = MyBase._RevenueTitle
                str3 = MyBase._RevenueStartDate
                str4 = MyBase._RevenueEndDate
                lastRevenueStart = Me.GetLastRevenueStart
                num = 0
            Else
                str = "è⁄ç◊ï\é¶"
                isChangeFlg = Me.GetIsChangeFlg(False)
                str2 = Me.GetRevExpend("l_title", 0).ToString
                str3 = Me.GetRevExpend("d_revenue_str", 0).ToString
                str4 = Me.GetRevExpend("d_revenue_end", 0).ToString
                lastRevenueStart = Me.GetLastRevenueStart2
                num = Convert.ToInt64(Me.GetRevExpend("s_revenue_expenditure_ttl", 0))
                revExpend = Me.GetRevExpend("d_up", 0)
            End If
            Dim expect As New CtlIncomeExpect(str3, str4, lastRevenueStart, IsNewFlg, isChangeFlg, str2, num, revExpend, MyBase.IsReferenceRight, MyBase.IsGetEntryRight, MyBase.IsPrintRight, Me)
            Utilities.OverlayUserControl(ParentForm.Controls(MDConst.MAIN_PANEL_ID), ("é˚ì¸ó\ëz" & " - " & str), New UserControl() {expect})
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub SearchListData()
        Try
            'Me.dTRevenueExpenditureList = DirectCast(MyBase._factory.GetObject("Business.RevenueExpenditure.RevenueExpenditureListCommand"), IRevenueExpenditureListCommand).GetRevenueExpenditureList(PublicCommand.GetSystemDate)
            Me.dTRevenueExpenditureList = (New Business.RevenueExpenditure.RevenueExpenditureListCommand).GetRevenueExpenditureList(PublicCommand.GetSystemDate)
            Me.flxRevenueExpenditureList.DataSource = Me.dTRevenueExpenditureList
            Me.FlexGridSetting()
            Me.SetHyperLink()
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub SetHyperLink()
        Try
            Dim map As New RevenueExpenditureListMap
            Dim physicalIndex As Integer = map.GetPhysicalIndex("é˚ì¸ó\ëzèÛãµ")
            Dim num2 As Integer = map.GetPhysicalIndex("èCê≥ó\éZèÛãµ") + 1
            map.GetPhysicalIndex("k_revenue_seton")
            Dim num3 As Integer = map.GetPhysicalIndex("èCê≥ó\éZèÛãµ")
            Dim styleName As String = Nothing
            Dim i As Integer
            For i = 1 To Me.flxRevenueExpenditureList.Rows.Count - 1
                Dim str As String = Me.dTRevenueExpenditureList.Rows.Item((i - 1)).Item("k_revenue_seton").ToString
                Dim j As Integer = physicalIndex
                Do While (j <= num2)
                    If Me.flxRevenueExpenditureList.Item(i, j).ToString.Equals("è⁄ç◊ï\é¶") Then
                        If MyBase.IsReferenceRight Then
                            styleName = "col_link"
                        Else
                            styleName = "col_nolink"
                        End If
                    ElseIf Me.flxRevenueExpenditureList.Item(i, (j - 1)).ToString.Equals("è⁄ç◊ï\é¶") Then
                        If MyBase.IsGetEntryRight Then
                            styleName = "col_link"
                            If ((j = num2) AndAlso str.Equals("")) Then
                                styleName = "col_nolink"
                            End If
                        Else
                            styleName = "col_nolink"
                        End If
                    Else
                        styleName = "col_nolink"
                    End If
                    Me.flxRevenueExpenditureList.SetCellStyle(i, j, styleName)
                    j += 1
                Loop
            Next i
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub

    Private Sub SetProperty()
        Try
            Dim str As String
            Try
                str = PublicCommand.ConvertHanToZen((Convert.ToInt32(MDLoginInfo.Period) + 1).ToString)
                str = ("ëÊ" & str & "ä˙")
            Catch obj1 As Exception
                str = ""
            End Try
            Me.txtRevenueTitle.Text = str
            Dim systemDate As String = PublicCommand.GetSystemDate
            Dim str3 As String = systemDate.Substring(0, 4)
            If (systemDate.Substring(4, 2).CompareTo("08") >= 0) Then
                str3 = (Convert.ToInt32(str3) + 1).ToString
            End If
            Me.dtpRevenueStartDate.Value = DateTime.ParseExact((str3 & "/08/01"), "yyyy/MM/dd", Nothing)
            Me.txtRevenueEndDate.BackColor = Color.LightYellow
            Utilities.SetEnabledProperty(MyBase.IsGetEntryRight, New Control() {Me.grpExpenditure})
        Catch exception As AppUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception
        Catch exception2 As SysUnionException
            exception2.AddMethodName(MethodBase.GetCurrentMethod)
            Throw exception2
        Catch exception3 As Exception
            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
        End Try
    End Sub


    ' Fields
    Private WithEvents btnSalaryRenew As Button
    Private components As IContainer
    Private Const DETAIL_CODE As String = "è⁄ç◊ï\é¶"
    Private Const DETAIL_VIEW As Boolean = False
    Private WithEvents dtpRevenueStartDate As DateTimePicker
    Private dTRevenueExpenditureList As DataTable
    Private WithEvents flxRevenueExpenditureList As C1FlexGrid
    Private grpExpenditure As GroupBox
    Private grpRevenueExpenditureList As GroupBox
    Private Const HYPER_LINK_END_COL As String = "èCê≥ó\éZèÛãµ"
    Private Const HYPER_LINK_START_COL As String = "é˚ì¸ó\ëzèÛãµ"
    Private label1 As Label
    Private label2 As Label
    Private label3 As Label
    Private label4 As Label
    Private label5 As Label
    Private label6 As Label
    Private label7 As Label
    Private label8 As Label
    Private lblAsta1 As Label
    Private Const NEW_ENTRY As Boolean = True
    Private Const NEWENTRY_CODE As String = "êVãKí«â¡"
    Private txtRevenueEndDate As PersonalTextBox
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Private txtRevenueTitle As PersonalTextBox
End Class
