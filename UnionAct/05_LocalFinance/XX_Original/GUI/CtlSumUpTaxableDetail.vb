Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.FinancialAffairs
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.NSCLMsg

Namespace GUI.FinancialAffairs.WithHolding
    Public Class CtlSumUpTaxableDetail
        Inherits CtlWithHoldingDetailBase
        ' Methods
        Public Sub New(ByVal Year As String, ByVal strNameForRight As String, ByVal CancelHandler As EventHandler)
            MyBase.New(Year, "12", strNameForRight, CancelHandler)
            Me.InitializeComponent()
            MyBase.AddFlexGridStyle()
            MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(20, "check_col", False, False, False, True, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(90, "employee_number_col_link", False, False, True, False, True), New GridSettingInfo(150, "name_col", False, False, False, False, True), New GridSettingInfo(60, "readonly_col", False, False, True, False, True), New GridSettingInfo(90, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(120, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(120, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(120, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(100, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(110, "noedit_money_col", False, False, False, False, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(0, "readonly_col", False, False, False, False, False)}
        End Sub

        Protected Overrides Sub CalcTotal(ByVal isError As Boolean)
            Try
                Dim num As Long = 0
                Dim num2 As Long = 0
                Dim num3 As Long = 0
                Dim num4 As Long = 0
                Dim num5 As Long = 0
                Dim num6 As Long = 0
                Dim i As Integer
                For i = 1 To MyBase.flxList.Rows.Count - 1
                    num = (num + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.REMUNERATION) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.REMUNERATION))))
                    num2 = (num2 + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.MONTHLY_COVER) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.MONTHLY_COVER))))
                    num3 = (num3 + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.BONUS_COVER) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.BONUS_COVER))))
                    num4 = (num4 + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.PAYTOTAL) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.PAYTOTAL))))
                    num5 = (num5 + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.WITHHOLDING) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.WITHHOLDING))))
                    num6 = (num6 + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.PAYOUT) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.PAYOUT))))
                Next i
                Me.lblSumRemuneration.Text = num.ToString("###,###,##0")
                Me.lblSumMonthly.Text = num2.ToString("###,###,##0")
                Me.lblSumBonus.Text = num3.ToString("###,###,##0")
                Me.lblSumPayTotal.Text = num4.ToString("###,###,##0")
                Me.lblSumWithholding.Text = num5.ToString("###,###,##0")
                Me.lblSumPayOut.Text = num6.ToString("###,###,##0")
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Protected Overrides Sub ChangeColumnOrder(ByRef dTblGrid As DataTable)
            Dim ordinal As Integer = dTblGrid.Columns.Item("åπêÚí•é˚äz").Ordinal
            dTblGrid.Columns.Item("åπêÚí•é˚äz" & "(" & "åéó·" & ")").SetOrdinal(ordinal)
            dTblGrid.Columns.Item("åπêÚí•é˚äz" & "(" & "àÍéûã‡" & ")").SetOrdinal((ordinal + 1))
            dTblGrid.Columns.Item("åπêÚí•é˚äz").ColumnName = "åπêÚí•é˚äz" & "(" & "åv" & ")"
        End Sub

        Private Function Convert2SumUpListReportData() As DataTable
            Dim table As DataTable = New WithholdingSumUpTaxableListReportMap().CreateDataTablePhysName("dtDetail")
            Dim objArray As Object() = New Object() {COLIDX.EMPLOYEE_NUMBER, COLIDX.NAME, MyBase.cmbBelonging.Text, COLIDX.LICENSE, COLIDX.REMUNERATION, COLIDX.MONTHLY_COVER, COLIDX.BONUS_COVER, COLIDX.WITHHOLDING}
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
            Return ("ó›åv" & " - " & "â€ê≈ëŒè€é“" & MyBase.lblYear.Text & "îN" & " " & MyBase.cmbBelonging.Text)
        End Function

        Private Function GetSumUpTaxableListReportData(ByVal TargetYear As String) As DataSet
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
            Dim manager As New ComponentResourceManager(GetType(CtlSumUpTaxableDetail))
            Me.lblSumPayTotal = New Label
            Me.lblSumBonus = New Label
            Me.lblSumMonthly = New Label
            Me.lblSumRemuneration = New Label
            Me.lblSumWithholding = New Label
            Me.lblSumPayOut = New Label
            MyBase.flxList.BeginInit()
            MyBase.SuspendLayout()
            MyBase.btnShow.Location = New Point(&HFE, 15)
            MyBase.btnShow.TabIndex = 4
            MyBase.cmbBelonging.Location = New Point(&HA2, 15)
            MyBase.cmbBelonging.Size = New Size(&H53, &H18)
            MyBase.cmbBelonging.TabIndex = 3
            MyBase.lblBelongLocal.Location = New Point(&H77, 20)
            MyBase.lblBelongLocal.TabIndex = 2
            MyBase.lblMonth.Location = New Point(&H32E, &H2D2)
            MyBase.lblMonth.TabIndex = &H11
            MyBase.lblMonth.Visible = False
            MyBase.label7.Location = New Point(&H351, &H2D7)
            MyBase.label7.TabIndex = &H12
            MyBase.label7.Visible = False
            MyBase.btnPrintDetails.Location = New Point(&H16E, &H2CD)
            MyBase.btnPrintDetails.TabIndex = 14
            MyBase.btnPrintList.Location = New Point(&H21E, &H2CD)
            MyBase.btnPrintList.TabIndex = 15
            MyBase.btnBackOrCancel.Location = New Point(&H2CE, &H2CD)
            MyBase.btnBackOrCancel.TabIndex = &H10
            'MyBase.flxList.ColumnInfo = manager.GetString("flxList.ColumnInfo")
            MyBase.flxList.Location = New Point(14, &H3B)
            MyBase.flxList.Rows.Count = 2
            MyBase.flxList.Rows.DefaultSize = 20
            MyBase.flxList.Size = New Size(&H3E6, &H263)
            MyBase.flxList.TabIndex = 5
            MyBase.btnAllCheckOff.Location = New Point(11, &H2BE)
            MyBase.btnAllCheckOff.TabIndex = 7
            MyBase.btnAllCheckOn.Location = New Point(11, &H2A2)
            MyBase.btnAllCheckOn.TabIndex = 6
            MyBase.btnOutputFile.Location = New Point(&HC0, &H2CD)
            Me.lblSumPayTotal.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumPayTotal.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumPayTotal.ForeColor = Color.Blue
            Me.lblSumPayTotal.Location = New Point(&H299, &H2A1)
            Me.lblSumPayTotal.Name = "lblSumPayTotal"
            Me.lblSumPayTotal.Size = New Size(&H77, &H17)
            Me.lblSumPayTotal.TabIndex = 11
            Me.lblSumPayTotal.Text = "999,999,999"
            Me.lblSumPayTotal.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumBonus.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumBonus.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumBonus.ForeColor = Color.Blue
            Me.lblSumBonus.Location = New Point(&H221, &H2A1)
            Me.lblSumBonus.Name = "lblSumBonus"
            Me.lblSumBonus.Size = New Size(&H77, &H17)
            Me.lblSumBonus.TabIndex = 10
            Me.lblSumBonus.Text = "999,999,999"
            Me.lblSumBonus.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumMonthly.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumMonthly.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumMonthly.ForeColor = Color.Blue
            Me.lblSumMonthly.Location = New Point(&H1A9, &H2A1)
            Me.lblSumMonthly.Name = "lblSumMonthly"
            Me.lblSumMonthly.Size = New Size(&H77, &H17)
            Me.lblSumMonthly.TabIndex = 9
            Me.lblSumMonthly.Text = "999,999,999"
            Me.lblSumMonthly.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumRemuneration.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumRemuneration.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumRemuneration.ForeColor = Color.Blue
            Me.lblSumRemuneration.Location = New Point(&H133, &H2A1)
            Me.lblSumRemuneration.Name = "lblSumRemuneration"
            Me.lblSumRemuneration.Size = New Size(&H75, &H17)
            Me.lblSumRemuneration.TabIndex = 8
            Me.lblSumRemuneration.Text = "999,999"
            Me.lblSumRemuneration.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumWithholding.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumWithholding.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumWithholding.ForeColor = Color.Blue
            Me.lblSumWithholding.Location = New Point(&H311, &H2A1)
            Me.lblSumWithholding.Name = "lblSumWithholding"
            Me.lblSumWithholding.Size = New Size(&H63, &H17)
            Me.lblSumWithholding.TabIndex = 12
            Me.lblSumWithholding.Text = "999,999,999"
            Me.lblSumWithholding.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumPayOut.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumPayOut.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumPayOut.ForeColor = Color.Blue
            Me.lblSumPayOut.Location = New Point(&H375, &H2A1)
            Me.lblSumPayOut.Name = "lblSumPayOut"
            Me.lblSumPayOut.Size = New Size(&H6D, &H17)
            Me.lblSumPayOut.TabIndex = 13
            Me.lblSumPayOut.Text = "999,999,999"
            Me.lblSumPayOut.TextAlign = ContentAlignment.MiddleRight
            MyBase.Controls.Add(Me.lblSumPayOut)
            MyBase.Controls.Add(Me.lblSumWithholding)
            MyBase.Controls.Add(Me.lblSumPayTotal)
            MyBase.Controls.Add(Me.lblSumBonus)
            MyBase.Controls.Add(Me.lblSumMonthly)
            MyBase.Controls.Add(Me.lblSumRemuneration)
            MyBase.Name = "CtlSumUpTaxableDetail"
            MyBase.Controls.SetChildIndex(MyBase.btnOutputFile, 0)
            MyBase.Controls.SetChildIndex(MyBase.label7, 0)
            MyBase.Controls.SetChildIndex(MyBase.label6, 0)
            MyBase.Controls.SetChildIndex(MyBase.lblYear, 0)
            MyBase.Controls.SetChildIndex(MyBase.lblMonth, 0)
            MyBase.Controls.SetChildIndex(MyBase.lblBelongLocal, 0)
            MyBase.Controls.SetChildIndex(MyBase.cmbBelonging, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnShow, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnBackOrCancel, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnPrintList, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnPrintDetails, 0)
            MyBase.Controls.SetChildIndex(MyBase.flxList, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnAllCheckOn, 0)
            MyBase.Controls.SetChildIndex(MyBase.btnAllCheckOff, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumRemuneration, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumMonthly, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumBonus, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumPayTotal, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumWithholding, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumPayOut, 0)
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
                If (CLMsg.Show("GQ0018") = DialogResult.No) Then
                    Return
                End If
            End If
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim sumUpTaxableDetailReportData As DataSet = MyBase._business.GetSumUpTaxableDetailReportData(MyBase.TargetYear, SelectedMembers)
                Me.Cursor = Cursors.Default
                'Dim viewer As New ReportViewer(sumUpTaxableDetailReportData, "Report.Withholding.RptWithholdingSumTable")
                Dim viewer As New ReportViewer(sumUpTaxableDetailReportData, New CR0503PA)
                If Preview Then
                    viewer.ReportViewerShow()
                Else
                    viewer.PrintOut()
                End If
                'viewer.RptDataDispose()
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
            'Dim viewer As New ReportViewer(Me.GetSumUpTaxableListReportData(MyBase.TargetYear), "Report.Withholding.RptWithholdingCumulativeTable_local")
            Dim viewer As New ReportViewer(Me.GetSumUpTaxableListReportData(MyBase.TargetYear), New CR0503PB)
            viewer.ReportViewerShow()
        End Sub

        Protected Overrides Sub Query(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String)
            Try
                MyBase._original = MyBase._business.GetSumUpTaxableData(TargetYear, UnionBranch).Copy
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Protected Overrides Sub ResetTotalLabels()
            FinancialAffairsUtility.SetZeroValueToLabels(New Label() {Me.lblSumBonus, Me.lblSumMonthly, Me.lblSumPayOut, Me.lblSumPayTotal, Me.lblSumRemuneration, Me.lblSumWithholding})
        End Sub


        ' Fields
        Private components As IContainer
        Private lblSumBonus As Label
        Private lblSumMonthly As Label
        Private lblSumPayOut As Label
        Private lblSumPayTotal As Label
        Private lblSumRemuneration As Label
        Private lblSumWithholding As Label

        ' Nested Types
        Private Enum COLIDX
            ' Fields
            CHECK = 0
            EMPLOYEE_NUMBER = 2
            NAME = 3
            LICENSE = 4
            REMUNERATION = 5
            MONTHLY_COVER = 6
            BONUS_COVER = 7
            PAYTOTAL = 8
            WITHHOLDING = 9
            PAYOUT = 10
            USER_ID = 11
            WITHHOLDING_MONTHLY = 12
            WITHHOLDING_ONCE = 13
        End Enum
    End Class
End Namespace
