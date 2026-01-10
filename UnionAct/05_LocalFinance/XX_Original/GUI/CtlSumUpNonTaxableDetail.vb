Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.FinancialAffairs
Imports UnionAct.NSCLMsg
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms

Namespace GUI.FinancialAffairs.WithHolding
    Public Class CtlSumUpNonTaxableDetail
        Inherits CtlWithHoldingDetailBase
        ' Methods
        Public Sub New(ByVal Year As String, ByVal strNameForRight As String, ByVal CancelHandler As EventHandler)
            MyBase.New(Year, "12", strNameForRight, CancelHandler)
            Me.InitializeComponent()
            MyBase.AddFlexGridStyle()
            MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(20, "check_col", False, False, False, True, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(90, "employee_number_col_link", False, False, True, False, True), New GridSettingInfo(150, "name_col", False, False, False, False, True), New GridSettingInfo(60, "readonly_col", False, False, True, False, True), New GridSettingInfo(140, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(140, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(140, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False)}
        End Sub

        Protected Overrides Sub CalcTotal(ByVal isError As Boolean)
            Dim num As Long = 0
            Dim num2 As Long = 0
            Dim num3 As Long = 0
            Dim i As Integer
            For i = 1 To MyBase.flxList.Rows.Count - 1
                num = (num + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.MONTHLY_COVER) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.MONTHLY_COVER))))
                num2 = (num2 + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.BONUS_COVER) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.BONUS_COVER))))
                num3 = (num3 + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.PAYTOTAL) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.PAYTOTAL))))
            Next i
            Me.lblSumMonthly.Text = num.ToString("###,###,##0")
            Me.lblSumBonus.Text = num2.ToString("###,###,##0")
            Me.lblSumPayTotal.Text = num3.ToString("###,###,##0")
        End Sub

        Private Function Convert2SumUpListReportData() As DataTable
            Dim table As DataTable = New WithholdingSumUpNonTaxableListReportMap().CreateDataTablePhysName("dtDetail")
            Dim objArray As Object() = New Object() {COLIDX.EMPLOYEE_NUMBER, COLIDX.NAME, MyBase.cmbBelonging.Text, COLIDX.LICENSE, COLIDX.MONTHLY_COVER, COLIDX.BONUS_COVER}
            Dim i As Integer
            For i = 1 To MyBase.flxList.Rows.Count - 1
                Dim row As DataRow = table.NewRow
                Dim j As Integer
                For j = 0 To objArray.Length - 1
                    row.Item(j) = If(TypeOf objArray(j) Is COLIDX, MyBase.flxList.Rows.Item(i).Item(CInt(objArray(j))), objArray(j))
                Next j
                table.Rows.Add(row)
            Next i
            Return table
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Function GetOutputFileName() As String
            Return ("ó›åv" & " - " & "â€ê≈îÒëŒè€é“" & MyBase.lblYear.Text & "îN" & " " & MyBase.cmbBelonging.Text)
        End Function

        Private Function GetSumUpNonTaxableListReportData(ByVal TargetYear As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim table As DataTable = MyBase.CreateSumUpReportHeader(TargetYear)
                ds.Tables.Add(table)
                Dim table2 As DataTable = Me.Convert2SumUpListReportData
                ds.Tables.Add(table2)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
            Return set2
        End Function

        Private Sub InitializeComponent()
            Dim manager As New ComponentResourceManager(GetType(CtlSumUpNonTaxableDetail))
            Me.lblSumPayTotal = New Label
            Me.lblSumBonus = New Label
            Me.lblSumMonthly = New Label
            MyBase.flxList.BeginInit()
            MyBase.SuspendLayout()
            MyBase.btnShow.Location = New Point(&H180, 15)
            MyBase.btnShow.TabIndex = 4
            MyBase.cmbBelonging.Location = New Point(&H124, 15)
            MyBase.cmbBelonging.Size = New Size(&H53, &H18)
            MyBase.cmbBelonging.TabIndex = 3
            MyBase.lblBelongLocal.Location = New Point(&HF9, 20)
            MyBase.lblBelongLocal.TabIndex = 2
            MyBase.lblMonth.Location = New Point(830, &H2D2)
            MyBase.lblMonth.TabIndex = 14
            MyBase.lblMonth.Visible = False
            MyBase.lblYear.Location = New Point(&H9D, 15)
            MyBase.label6.Location = New Point(&HD3, &H13)
            MyBase.label7.Location = New Point(&H361, &H2D7)
            MyBase.label7.TabIndex = 15
            MyBase.label7.Visible = False
            MyBase.btnPrintDetails.Location = New Point(&H174, &H2CD)
            MyBase.btnPrintDetails.TabIndex = 11
            MyBase.btnPrintList.Location = New Point(&H21A, &H2CD)
            MyBase.btnPrintList.TabIndex = 12
            MyBase.btnBackOrCancel.Location = New Point(700, &H2CD)
            MyBase.btnBackOrCancel.TabIndex = 13
            'MyBase.flxList.ColumnInfo = manager.GetString("flxList.ColumnInfo")
            MyBase.flxList.Location = New Point(&H87, &H3B)
            MyBase.flxList.Rows.Count = &H2A
            MyBase.flxList.Rows.DefaultSize = 20
            MyBase.flxList.Size = New Size(&H2F5, &H263)
            MyBase.flxList.TabIndex = 5
            MyBase.btnAllCheckOff.Location = New Point(&H84, &H2BE)
            MyBase.btnAllCheckOff.TabIndex = 7
            MyBase.btnAllCheckOn.Location = New Point(&H84, &H2A2)
            MyBase.btnAllCheckOn.TabIndex = 6
            MyBase.btnOutputFile.Location = New Point(&HD3, &H2CD)
            Me.lblSumPayTotal.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumPayTotal.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumPayTotal.Location = New Point(&H2E0, &H2A1)
            Me.lblSumPayTotal.Name = "lblSumPayTotal"
            Me.lblSumPayTotal.Size = New Size(&H8B, &H17)
            Me.lblSumPayTotal.TabIndex = 10
            Me.lblSumPayTotal.Text = "999,999,999"
            Me.lblSumPayTotal.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumBonus.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumBonus.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumBonus.Location = New Point(&H254, &H2A1)
            Me.lblSumBonus.Name = "lblSumBonus"
            Me.lblSumBonus.Size = New Size(&H8B, &H17)
            Me.lblSumBonus.TabIndex = 9
            Me.lblSumBonus.Text = "999,999,999"
            Me.lblSumBonus.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumMonthly.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumMonthly.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumMonthly.Location = New Point(&H1C7, &H2A1)
            Me.lblSumMonthly.Name = "lblSumMonthly"
            Me.lblSumMonthly.Size = New Size(140, &H17)
            Me.lblSumMonthly.TabIndex = 8
            Me.lblSumMonthly.Text = "999,999,999"
            Me.lblSumMonthly.TextAlign = ContentAlignment.MiddleRight
            MyBase.Controls.Add(Me.lblSumPayTotal)
            MyBase.Controls.Add(Me.lblSumBonus)
            MyBase.Controls.Add(Me.lblSumMonthly)
            MyBase.Name = "CtlSumUpNonTaxableDetail"
            MyBase.Controls.SetChildIndex(MyBase.btnOutputFile, 0)
            MyBase.Controls.SetChildIndex(MyBase.label7, 0)
            MyBase.Controls.SetChildIndex(MyBase.lblMonth, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnPrintList, 0)
            MyBase.Controls.SetChildIndex(MyBase.label6, 0)
            MyBase.Controls.SetChildIndex(MyBase.lblYear, 0)
            MyBase.Controls.SetChildIndex(MyBase.lblBelongLocal, 0)
            MyBase.Controls.SetChildIndex(MyBase.cmbBelonging, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnShow, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnBackOrCancel, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnPrintDetails, 0)
            MyBase.Controls.SetChildIndex(MyBase.flxList, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnAllCheckOn, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnAllCheckOff, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumMonthly, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumBonus, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumPayTotal, 0)
            MyBase.flxList.EndInit()
            MyBase.ResumeLayout(False)
            MyBase.PerformLayout()
        End Sub

        Protected Overrides Sub PreviewDetail(ByVal row As Integer)
            Dim selectedMembers As New ArrayList
            selectedMembers.Add(MyBase.flxList.Item(row, COLIDX.USER_ID).ToString)
            Me.PrintDetail(selectedMembers, True)
        End Sub

        Protected Overrides Sub PrintDetail(ByVal SelectedMembers As ArrayList, ByVal Preview As Boolean)
            If Not Preview Then
                If (SelectedMembers.Count <= 0) Then
                    CLMsg.Show("GE0065")
                    Return
                End If
                If (CLMsg.Show("GQ0019") = DialogResult.No) Then
                    Return
                End If
            End If
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim sumUpNonTaxableDetailReportData As DataSet = MyBase._business.GetSumUpNonTaxableDetailReportData(MyBase.TargetYear, SelectedMembers)
                Me.Cursor = Cursors.Default
                'Dim viewer As New ReportViewer(sumUpNonTaxableDetailReportData, "Report.Withholding.RptAllowanceSeat")
                Dim viewer As New ReportViewer(sumUpNonTaxableDetailReportData, New CR0503P9)
                If Preview Then
                    viewer.ReportViewerShow()
                Else
                    viewer.PrintOut()
                End If
                'viewer.RptDataDispose
            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            End Try
        End Sub

        Protected Overrides Sub PrintDetailBySelected()
            Try
                Dim selectedMembers As ArrayList = MyBase.GetSelectedMembers(0, COLIDX.USER_ID)
                Me.PrintDetail(selectedMembers, False)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Protected Overrides Sub PrintList()
            'Dim viewer As New ReportViewer(Me.GetSumUpNonTaxableListReportData(MyBase.TargetYear), "Report.Withholding.RptTaxNoIntendedPayCumulativeTable_local")
            Dim viewer As New ReportViewer(Me.GetSumUpNonTaxableListReportData(MyBase.TargetYear), New CR0503PH)
            viewer.ReportViewerShow()
        End Sub

        Protected Overrides Sub Query(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String)
            Try
                MyBase._original = MyBase._business.GetSumUpNonTaxableData(TargetYear, UnionBranch).Copy
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Protected Overrides Sub ResetTotalLabels()
            FinancialAffairsUtility.SetZeroValueToLabels(New Label() {Me.lblSumBonus, Me.lblSumMonthly, Me.lblSumPayTotal})
        End Sub


        ' Fields
        Private components As IContainer
        Private lblSumBonus As Label
        Private lblSumMonthly As Label
        Private lblSumPayTotal As Label

        ' Nested Types
        Private Enum COLIDX
            ' Fields
            CHECK = 0
            EMPLOYEE_NUMBER = 2
            NAME = 3
            LICENSE = 4
            MONTHLY_COVER = 5
            BONUS_COVER = 6
            PAYTOTAL = 7
            USER_ID = 8
        End Enum
    End Class
End Namespace
