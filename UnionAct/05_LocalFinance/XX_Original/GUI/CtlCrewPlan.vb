Imports C1.Win.C1FlexGrid
Imports C1.Win.C1FlexGrid.Util.BaseControls
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.GUI.Common
Imports UnionAct.Framework.UnionException
Imports UnionAct.NSCLMsg
Imports UnionAct.Business.RevenueExpenditure
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.Command
Imports UnionAct.GUI.UnionComponent
Imports UnionAct.NSMDInfo

Namespace GUI.RevenueExpenditure.UnionForm
    Public Class CtlCrewPlan
        Inherits RevenueExpenditureBase
        ' Methods
        Private Sub New()
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal _IsReferenceRight As Boolean, ByVal _IsGetEntryRight As Boolean, ByVal _IsPrintRight As Boolean, ByVal _IsNewFlg As Boolean, ByVal _IsChangeFlg As Boolean, ByVal _dTable As DataTable, ByVal _strLastRevenueStart As String, ByVal parent As UC050401)
            MyBase.New()
            Me.InitializeComponent()
            Me.dtTable = _dTable
            MyBase.IsReferenceRight = _IsReferenceRight
            MyBase.IsNewFlg = _IsNewFlg
            MyBase.IsChangeFlg = _IsChangeFlg
            Me.GetEntryRigth = _IsGetEntryRight
            Me.PrintRight = _IsPrintRight
            If (Me.dtTable.Rows.Count > 0) Then
                MyBase._RevenueStartDate = Me.dtTable.Rows.Item(0).Item("d_revenue_str").ToString
            End If
            Me._parent = parent
        End Sub

        Private Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
            Utilities.RestoreUserControl()
            _parent.ActionAfterResotreUserControl()
        End Sub

        Private Sub btnBaseChange_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBaseChange.Click
            Try
                Me.dtTableFlexGrid.AcceptChanges()
                Me.dtTableCalcData.AcceptChanges()
                Me.dtTable.AcceptChanges()
                Me.dtTableUnionDues.AcceptChanges()
                Me.Cursor = Cursors.WaitCursor
                Dim plan As New FrmCrewPlan(Me.dtTable.Rows.Item(0).Item("l_title"), Me.dtTable.Rows.Item(0).Item("d_revenue_str"), Me.dtTable.Rows.Item(0).Item("d_revenue_end"), Me.dtTable.Rows.Item(0).Item("s_new_staff_average"), Me.dtTable.Rows.Item(0).Item("s_cap_promotion_average"), Me.dtTable.Rows.Item(0).Item("s_unpromotion_rate"), Me.dtTable.Rows.Item(0).Item("s_unpromotion_average"), Me.dtTable.Rows.Item(0).Item("s_senior_stay_rate"), Me.dtTable.Rows.Item(0).Item("s_senior_average")) With { _
                    .Text = "èÊàıåvâÊ" & " " & "äÓñ{èÓïÒïœçX" _
                }
                plan.ShowDialog()
                If plan.IsOk Then
                    Me.dtTable.Rows.Item(0).Item("s_new_staff_average") = plan.iNewStaffAverage
                    Me.dtTable.Rows.Item(0).Item("s_cap_promotion_average") = plan.iCapPromotionAverage
                    Me.dtTable.Rows.Item(0).Item("s_unpromotion_rate") = plan.dUnpromotionRate
                    Me.dtTable.Rows.Item(0).Item("s_unpromotion_average") = plan.iUnpromotionAverage
                    Me.dtTable.Rows.Item(0).Item("s_senior_stay_rate") = plan.dSeniorRetire
                    Me.dtTable.Rows.Item(0).Item("s_senior_average") = plan.iSeniorAverage
                    Me.Load_Set(Me.dtTable)
                    Me.GetUnionDues()
                    Me.SetNewReviseRevenueTtl()
                    Me.Caluculation()
                    Me.SetLabelText()
                    Me.txtUnpromotionPersons.ReadOnly = False
                    Me.flxStatisticalTable.AllowEditing = True
                    Me.flxStatisticalTable.Cols.Item(0).AllowEditing = False
                    Me.flxStatisticalTable.Cols.Item(3).AllowEditing = False
                    Me.flxStatisticalTable.Cols.Item(4).AllowEditing = False
                    Me.flxStatisticalTable.Cols.Item(5).AllowEditing = False
                    Me.flxStatisticalTable.Cols.Item(6).AllowEditing = False
                    Me.SetButtan(Me.btnBaseChange.Name)
                End If
                plan.Dispose()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click
            Try
                If CLMsg.Show("GQ0007").Equals(DialogResult.Yes) Then
                    If (Not MyBase._IsNewFlg AndAlso MyBase._IsChangeFlg) Then
                        Dim flag As Boolean = Not Me.btnChange.Visible
                        If flag.Equals(True) Then
                            Me.Cursor = Cursors.WaitCursor
                            Me.dtTable = Me.dtTableBack.Copy
                            Me.Load_Set(Me.dtTable)
                            Me.GetUnionDues()
                            Me.btnNewEntry.Visible = MyBase._IsNewFlg
                            Me.Caluculation()
                            Me.SetDetailReviseRevenueTtl()
                            Me.FlexGridDetailSet()
                            Me.SetButtan(Me.btnCancel.Name)
                            Return
                        End If
                    End If
                    Utilities.RestoreUserControl()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnChange_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnChange.Click
            Try
                Me.Cursor = Cursors.WaitCursor
                Me.txtUnpromotionPersons.ReadOnly = False
                Me.flxStatisticalTable.AllowEditing = True
                Me.flxStatisticalTable.Cols.Item(0).AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(3).AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(4).AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(5).AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(6).AllowEditing = False
                Me.SetButtan(Me.btnChange.Name)
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnEntryCheck_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEntryCheck.Click
            Try
                Dim num As Integer
                MyBase.CheckUpdateMessage(MyBase.Name)
                Me.Cursor = Cursors.WaitCursor
                Dim dSet As DataSet = Me.MakeDataSet
                Dim viewer As New ReportViewer(dSet, New CR0504P2, 1)
                Dim num2 As Integer = viewer.ConfirmViewerShow(num)
                viewer.RptDataDispose()
                Select Case num2
                    Case 0
                        Dim command As New CrewPlanCommand
                        command.UpdateRevenueExpenditure(Me.dtTable, Me.dtTableCalcData, Me.dtTableFlexGrid)
                        viewer = New ReportViewer(dSet, New CR0504P2, num)
                        viewer.PrintOut()
                        viewer.RptDataDispose()
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                        Return
                    Case 1
                        Dim command As New CrewPlanCommand
                        command.UpdateRevenueExpenditure(Me.dtTable, Me.dtTableCalcData, Me.dtTableFlexGrid)
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                        Return
                End Select
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnExpenditureRevenuePrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExpenditureRevenuePrint.Click
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim viewer As New ReportViewer(Me.MakeDataSet, New CR0504P3, 3)
                viewer.ReportViewerShow()
                viewer.RptDataDispose()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnNewEntry_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNewEntry.Click
            Try
                Dim num As Integer
                Me.Cursor = Cursors.WaitCursor
                Dim dSet As DataSet = Me.MakeDataSet
                Dim viewer As New ReportViewer(dSet, New CR0504P2, 1)
                Dim num2 As Integer = viewer.ConfirmViewerShow(num)
                viewer.RptDataDispose()
                Select Case num2
                    Case 0
                        Dim command As New CrewPlanCommand
                        command.InsertRevenueExpenditurePromotionDtl(Me.dtTableFlexGrid, Me.dtTable, Me.dtTableCalcData)
                        viewer = New ReportViewer(dSet, New CR0504P2, num)
                        viewer.PrintOut()
                        viewer.RptDataDispose()
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                        Return
                    Case 1
                        Dim command As New CrewPlanCommand
                        command.InsertRevenueExpenditurePromotionDtl(Me.dtTableFlexGrid, Me.dtTable, Me.dtTableCalcData)
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                        Return
                End Select
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub btnPrinting_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrinting.Click
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim viewer As New ReportViewer(Me.MakeDataSet, New CR0504P2, 3)
                viewer.ReportViewerShow()
                viewer.RptDataDispose()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub Caluculation()
            Try
                Me.FlexGridMemberTotal()
                Me.FlexGridMonthWorkTotal()
                Me.FlexGridMoneyTotal()
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

        Private Sub CheckRevenueExpenditure()
            Try
                If (((Convert.ToInt64(Me.dtTable.Rows.Item(0).Item("s_revise_revenue_ttl").ToString) <> Convert.ToInt64(Me.lblReviseRevenuettl.Text.Replace(",", ""))) AndAlso MyBase.IsChangeFlg) AndAlso Me.GetEntryRigth) Then
                    CLMsg.Show("GI0040", "é˚ì¸ó\ëzëçäz", "ï‚ê≥å„é˚ì¸äz")
                    Dim sender As New Object
                    Me.btnChange_Click(sender, EventArgs.Empty)
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

        Private Sub CtlCrewPlan_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            Try
                Me.Cursor = Cursors.WaitCursor
                Me.Load_Set(Me.dtTable)
                Me.GetUnionDues()
                If MyBase.IsNewFlg Then
                    Me.SetNewReviseRevenueTtl()
                    Me.SetButtan("")
                    Me.FlexGridNewEntrySet()
                Else
                    Me.dtTableBack = Me.dtTable.Copy
                    Me.SetDetailReviseRevenueTtl()
                    Me.FlexGridDetailSet()
                    Me.SetButtan("")
                    Me.CheckRevenueExpenditure()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Sub FlexGridDetailSet()
            Try
                Dim revenueExpenditurePromotionDtl As DataTable = (New CrewPlanCommand).GetRevenueExpenditurePromotionDtl(Me.dtTable.Rows.Item(0).Item("d_revenue_str").ToString)
                Me.dtTableCalcData = New CrewPlancalCulationMap().CreateDataTablePhysName("dtCalculation")
                Dim row As DataRow = Me.dtTableCalcData.NewRow
                Me.dtTableCalcData.Rows.Add(row)
                Me.SetDetailFlexGrid(revenueExpenditurePromotionDtl)
                Me.Caluculation()
                Me.SetLabelText()
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

        Private Sub FlexGridMemberTotal()
            Try
                Dim count As Integer = Me.dtTableFlexGrid.Rows.Count
                Dim num2 As Integer = Me.dtTableFlexGrid.Columns.Count
                Dim numArray As Integer() = New Integer(7 - 1) {}
                Dim i As Integer
                For i = 1 To num2 - 1
                    Dim num4 As Integer = 0
                    Dim j As Integer
                    For j = 0 To count - 1
                        If Not (Me.dtTableFlexGrid.Rows.Item(j).Item(i).ToString = "") Then
                            num4 = (num4 + Convert.ToInt32(Me.dtTableFlexGrid.Rows.Item(j).Item(i).ToString))
                        End If
                    Next j
                    numArray(i) = num4
                Next i
                Me.dtTableCalcData.Rows.Item(0).Item("s_new_staff_member_total") = Convert.ToInt32(numArray(1).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_new_cap_member_total") = Convert.ToInt32(numArray(2).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_cap_retire_member_total") = Convert.ToInt32(numArray(3).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_cop_retire_member_total") = Convert.ToInt32(numArray(4).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_fe_retire_member_total") = Convert.ToInt32(numArray(5).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_senior_retire_member_total") = Convert.ToInt32(numArray(6).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_unpromotion_persons") = Convert.ToInt32(Me.txtUnpromotionPersons.Text.Replace(",", ""))
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

        Private Sub FlexGridMoneyTotal()
            Try
                Dim num As Long = ((Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_new_union_monthwork").ToString) - Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_new_staff_member_total").ToString)) * Convert.ToInt64(Me.dtTableUnionDues.Rows.Item(0).Item("s_new_staff_union_dues").ToString))
                Dim num2 As Long = (Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_new_staff_member_total").ToString) * Convert.ToInt64(Me.dtTableUnionDues.Rows.Item(0).Item("s_entry_money").ToString))
                Dim num3 As Long = (Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_cap_up_monthwork").ToString) * Convert.ToInt64(Me.dtTableUnionDues.Rows.Item(0).Item("s_new_cap_union_dues").ToString))
                Dim num4 As Long = (Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_cap_retire_monthwork").ToString) * Convert.ToInt64(Me.dtTableUnionDues.Rows.Item(0).Item("s_cap_retire_union_dues").ToString))
                Dim num5 As Long = (Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_cop_retire_monthwork").ToString) * Convert.ToInt64(Me.dtTableUnionDues.Rows.Item(0).Item("s_cop_retire_union_dues").ToString))
                Dim num6 As Long = (Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_fe_retire_monthwork").ToString) * Convert.ToInt64(Me.dtTableUnionDues.Rows.Item(0).Item("s_fe_retire_union_dues").ToString))
                Dim num7 As Long = (Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_unpromotion_persons_monthwork").ToString) * Convert.ToInt64(Me.dtTableUnionDues.Rows.Item(0).Item("s_unpromotion_union_dues").ToString))
                Dim num8 As Long = (Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_senior_retire_monthwork").ToString) * Convert.ToInt64(Me.dtTableUnionDues.Rows.Item(0).Item("s_senior_retire_union_dues").ToString))
                Dim num9 As Long = Convert.ToInt64(CDec((Convert.ToDecimal(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_retire_monthwork_total").ToString) * Convert.ToDecimal(Me.dtTableUnionDues.Rows.Item(0).Item("s_senior_retire_union_dues").ToString))))
                Dim num10 As Long = ((((((((Convert.ToInt64(Me.dtTable.Rows.Item(0).Item("s_revenue_expenditure_ttl").ToString) + num) + num2) + num3) - num4) - num5) - num6) - num7) + num9)
                If (num10 >= &H2540BE400) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0174", New String() {"ï‚ê≥å„é˚ì¸ó\ëz"})
                End If
                Me.dtTableCalcData.Rows.Item(0).Item("s_revise_new_staff") = num
                Me.dtTableCalcData.Rows.Item(0).Item("s_new_staff_money") = num2
                Me.dtTableCalcData.Rows.Item(0).Item("s_revise_up") = num3
                Me.dtTableCalcData.Rows.Item(0).Item("s_revise_cap_retire") = num4
                Me.dtTableCalcData.Rows.Item(0).Item("s_revise_cop_retire") = num5
                Me.dtTableCalcData.Rows.Item(0).Item("s_revise_fe_retire") = num6
                Me.dtTableCalcData.Rows.Item(0).Item("s_revenue_unpromotion") = num7
                Me.dtTableCalcData.Rows.Item(0).Item("s_revise_senior_retire") = num8
                Me.dtTableCalcData.Rows.Item(0).Item("s_senior_union_dues") = num9
                Me.dtTableCalcData.Rows.Item(0).Item("s_revise_revenue_ttl") = num10
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

        Private Sub FlexGridMonthWorkTotal()
            Try
                Dim count As Integer = Me.dtTableFlexGrid.Rows.Count
                Dim num2 As Integer = Me.dtTableFlexGrid.Columns.Count
                Dim numArray As Integer() = New Integer(7 - 1) {}
                Dim i As Integer
                For i = 1 To num2 - 1
                    Dim num4 As Integer = 0
                    Dim num5 As Integer = 12
                    Dim j As Integer
                    For j = 0 To count - 1
                        If Not (Me.dtTableFlexGrid.Rows.Item(j).Item(i).ToString = "") Then
                            num4 = (num4 + (Convert.ToInt32(Me.dtTableFlexGrid.Rows.Item(j).Item(i).ToString) * num5))
                        End If
                        num5 -= 1
                    Next j
                    numArray(i) = num4
                Next i
                Me.dtTableCalcData.Rows.Item(0).Item("s_new_union_monthwork") = Convert.ToInt32(numArray(1).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_cap_up_monthwork") = Convert.ToInt32(numArray(2).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_cap_retire_monthwork") = Convert.ToInt32(numArray(3).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_cop_retire_monthwork") = Convert.ToInt32(numArray(4).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_fe_retire_monthwork") = Convert.ToInt32(numArray(5).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_senior_retire_monthwork") = Convert.ToInt32(numArray(6).ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_senior_monthwork_total") = Convert.ToInt32(Me.dtTable.Rows.Item(0).Item("s_senior_monthwork").ToString)
                Me.dtTableCalcData.Rows.Item(0).Item("s_retire_monthwork_total") = (Convert.ToInt32(Me.dtTableCalcData.Rows.Item(0).Item("s_cap_retire_monthwork")) + Convert.ToInt32(Me.dtTableCalcData.Rows.Item(0).Item("s_cop_retire_monthwork").ToString))
                Me.dtTableCalcData.Rows.Item(0).Item("s_revise_retire_monthwork_total") = PublicCommand.ToHalfAdjust(Convert.ToDouble(CDec(((((Convert.ToDecimal(Me.dtTable.Rows.Item(0).Item("s_senior_monthwork").ToString) - Convert.ToDecimal(numArray(6).ToString)) + Convert.ToDecimal(Me.dtTableCalcData.Rows.Item(0).Item("s_retire_monthwork_total"))) * Convert.ToDecimal(Me.dtTable.Rows.Item(0).Item("s_senior_stay_rate").ToString)) / Convert.ToDecimal(100)))), 1)
                Me.dtTableCalcData.Rows.Item(0).Item("s_unpromotion_persons_monthwork") = Convert.ToInt32(Me.lblUnpromotionPersonsMonthwork.Text.ToString.Replace(",", ""))
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

        Private Sub FlexGridNewEntrySet()
            Try
                Dim command As New CrewPlanCommand
                command.CreateRevenueExpenditureRetireDtlWork(PublicCommand.GetMac, MyBase.Name, MDLoginInfo.UserId, Me.dtTable.Rows.Item(0).Item("d_revenue_str").ToString, Me.dtTable.Rows.Item(0).Item("d_revenue_end").ToString, MDLoginInfo.Ksh)
                Dim revenueExpenditureRetireDtlWork As DataTable = command.GetRevenueExpenditureRetireDtlWork(PublicCommand.GetMac, MyBase.Name)
                Me.dtTableCalcData = New CrewPlancalCulationMap().CreateDataTablePhysName("dtCalculation")
                Dim row As DataRow = Me.dtTableCalcData.NewRow
                Me.dtTableCalcData.Rows.Add(row)
                Me.SetFlexGrid(revenueExpenditureRetireDtlWork)
                Me.Caluculation()
                Me.SetLabelText()
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

        Private Sub flxStatisticalTable_AfterEdit(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxStatisticalTable.AfterEdit
            Try
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub flxStatisticalTable_Click(ByVal sender As Object, ByVal e As EventArgs) Handles flxStatisticalTable.Click
            Try
                Me.Cursor = Cursors.WaitCursor
                If MyBase._IsNewFlg Then
                    If ((Me.flxStatisticalTable.MouseCol = 2) OrElse (Me.flxStatisticalTable.MouseCol = 3)) Then
                        Me.flxStatisticalTable.StartEditing()
                    End If
                ElseIf (((Me.flxStatisticalTable.MouseCol = 2) OrElse (Me.flxStatisticalTable.MouseCol = 3)) AndAlso (Not Me.btnChange.Visible AndAlso Me.btnEntryCheck.Visible)) Then
                    Me.flxStatisticalTable.StartEditing()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub flxStatisticalTable_SetupEditor(ByVal sender As Object, ByVal e As RowColEventArgs) Handles flxStatisticalTable.SetupEditor
            Try
                Me.Cursor = Cursors.WaitCursor
                If TypeOf Me.flxStatisticalTable.Editor Is PersonalTextBox Then
                    Dim editor As PersonalTextBox = DirectCast(Me.flxStatisticalTable.Editor, PersonalTextBox)
                    editor.ImeMode = ImeMode.Disable
                    editor.MaxLength = 3
                    DirectCast(Me.flxStatisticalTable.DataSource, DataTable).AcceptChanges()
                    Me.dtTableCalcData.AcceptChanges()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub flxStatisticalTable_ValidateEdit(ByVal sender As Object, ByVal e As ValidateEditEventArgs) Handles flxStatisticalTable.ValidateEdit
            Try
                If (Me.flxStatisticalTable.Editor.Text = "") Then
                    CLMsg.Show("GE0178")
                    Me.flxStatisticalTable.FinishEditing(True)
                Else
                    Dim num As Integer
                    If Integer.TryParse(Me.flxStatisticalTable.Editor.Text, num) Then
                        If (num < 0) Then
                            CLMsg.Show("GE0173")
                            Me.flxStatisticalTable.FinishEditing(True)
                        Else
                            Me.flxStatisticalTable.Item(e.Row, e.Col) = num
                        End If
                    Else
                        CLMsg.Show("GE0178")
                        Me.flxStatisticalTable.FinishEditing(True)
                    End If
                End If
                Me.Caluculation()
                Me.SetLabelText()
            Catch exception As AppUnionException
                Me.flxStatisticalTable.FinishEditing(True)
                DirectCast(Me.flxStatisticalTable.DataSource, DataTable).RejectChanges()
                Me.dtTableCalcData.RejectChanges()
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

        Private Sub GetUnionDues()
            Try
                Dim command As New CrewPlanCommand
                Dim listDataTable As DataTable = New InfoConstant(Me.dtTable.Rows.Item(0).Item("d_revenue_str").ToString).GetListDataTable("RETIRE_AGE", "c_constant")
                Me.dtTableUnionDues = command.GetUnionDues(Convert.ToInt32(Me.txtnewStaffAverage.Text.ToString), Convert.ToInt32(Me.txtCapPromotionAverage.Text.ToString), (Convert.ToInt32(listDataTable.Rows.Item(0).Item("l_omission_name").ToString) - 1), Convert.ToInt32(Me.txtUnpromotionAverage.Text.ToString), Convert.ToInt32(Me.txtSeniorAverage.Text.ToString), Me.dtTable.Rows.Item(0).Item("d_revenue_str").ToString)
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

        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CtlCrewPlan))
            Me.grpBaseInfo = New System.Windows.Forms.GroupBox
            Me.label20 = New System.Windows.Forms.Label
            Me.label19 = New System.Windows.Forms.Label
            Me.label18 = New System.Windows.Forms.Label
            Me.label15 = New System.Windows.Forms.Label
            Me.label9 = New System.Windows.Forms.Label
            Me.label17 = New System.Windows.Forms.Label
            Me.btnBaseChange = New System.Windows.Forms.Button
            Me.label31 = New System.Windows.Forms.Label
            Me.txtSeniorAverage = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.lbltitle = New System.Windows.Forms.Label
            Me.label7 = New System.Windows.Forms.Label
            Me.txtSeniorStayRate = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.label6 = New System.Windows.Forms.Label
            Me.txtUnpromotionAverage = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.label3 = New System.Windows.Forms.Label
            Me.txtUnpromotionRate = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.txtRevenueStrDate = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.label5 = New System.Windows.Forms.Label
            Me.txtCapPromotionAverage = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.txtnewStaffAverage = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.label4 = New System.Windows.Forms.Label
            Me.txtRevenueEndDate = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.label1 = New System.Windows.Forms.Label
            Me.label2 = New System.Windows.Forms.Label
            Me.grpStatisticalTable = New System.Windows.Forms.GroupBox
            Me.btnExpenditureRevenuePrint = New System.Windows.Forms.Button
            Me.label22 = New System.Windows.Forms.Label
            Me.label21 = New System.Windows.Forms.Label
            Me.groupBox2 = New System.Windows.Forms.GroupBox
            Me.label25 = New System.Windows.Forms.Label
            Me.lblSeniorTotal = New System.Windows.Forms.Label
            Me.label11 = New System.Windows.Forms.Label
            Me.lblSeniorMonthwork = New System.Windows.Forms.Label
            Me.label16 = New System.Windows.Forms.Label
            Me.groupBox1 = New System.Windows.Forms.GroupBox
            Me.label23 = New System.Windows.Forms.Label
            Me.lblUnpromotionTotal = New System.Windows.Forms.Label
            Me.label30 = New System.Windows.Forms.Label
            Me.lblUnpromotionPersonsMonthwork = New System.Windows.Forms.Label
            Me.label13 = New System.Windows.Forms.Label
            Me.txtUnpromotionPersons = New UnionAct.GUI.UnionComponent.PersonalTextBox
            Me.label12 = New System.Windows.Forms.Label
            Me.lblSeniorRetireMoneyTotal = New System.Windows.Forms.Label
            Me.lblFeRetireMoneyTotal = New System.Windows.Forms.Label
            Me.lblCopRetireMoneyTotal = New System.Windows.Forms.Label
            Me.lblCapRetireMoneyTotal = New System.Windows.Forms.Label
            Me.lblCapUpMoneyTotal = New System.Windows.Forms.Label
            Me.label27 = New System.Windows.Forms.Label
            Me.lblNewUnionMoneyTotal = New System.Windows.Forms.Label
            Me.label8 = New System.Windows.Forms.Label
            Me.lblRevenueExpenditureTtl = New System.Windows.Forms.Label
            Me.lblSeniorRetireMonthwork = New System.Windows.Forms.Label
            Me.lblSeniorRetireMemberTotal = New System.Windows.Forms.Label
            Me.lblFeRetireMonthwork = New System.Windows.Forms.Label
            Me.lblCopRetireMonthwork = New System.Windows.Forms.Label
            Me.lblCapRetireMonthwork = New System.Windows.Forms.Label
            Me.lblCapUpMonthwork = New System.Windows.Forms.Label
            Me.label24 = New System.Windows.Forms.Label
            Me.lblNewUnionMonthwork = New System.Windows.Forms.Label
            Me.lblFeRetireMemberTotal = New System.Windows.Forms.Label
            Me.lblCopRetireMemberTotal = New System.Windows.Forms.Label
            Me.lblCapRetireMemberTotal = New System.Windows.Forms.Label
            Me.lblCapUpTotal = New System.Windows.Forms.Label
            Me.label14 = New System.Windows.Forms.Label
            Me.lblNewUnionEntryTotal = New System.Windows.Forms.Label
            Me.label10 = New System.Windows.Forms.Label
            Me.lblReviseRevenuettl = New System.Windows.Forms.Label
            Me.flxStatisticalTable = New C1.Win.C1FlexGrid.C1FlexGrid
            Me.btnPrinting = New System.Windows.Forms.Button
            Me.btnChange = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnNewEntry = New System.Windows.Forms.Button
            Me.btnEntryCheck = New System.Windows.Forms.Button
            Me.btnBack = New System.Windows.Forms.Button
            Me.grpBaseInfo.SuspendLayout()
            Me.grpStatisticalTable.SuspendLayout()
            Me.groupBox2.SuspendLayout()
            Me.groupBox1.SuspendLayout()
            CType(Me.flxStatisticalTable, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'grpBaseInfo
            '
            Me.grpBaseInfo.Controls.Add(Me.label20)
            Me.grpBaseInfo.Controls.Add(Me.label19)
            Me.grpBaseInfo.Controls.Add(Me.label18)
            Me.grpBaseInfo.Controls.Add(Me.label15)
            Me.grpBaseInfo.Controls.Add(Me.label9)
            Me.grpBaseInfo.Controls.Add(Me.label17)
            Me.grpBaseInfo.Controls.Add(Me.btnBaseChange)
            Me.grpBaseInfo.Controls.Add(Me.label31)
            Me.grpBaseInfo.Controls.Add(Me.txtSeniorAverage)
            Me.grpBaseInfo.Controls.Add(Me.lbltitle)
            Me.grpBaseInfo.Controls.Add(Me.label7)
            Me.grpBaseInfo.Controls.Add(Me.txtSeniorStayRate)
            Me.grpBaseInfo.Controls.Add(Me.label6)
            Me.grpBaseInfo.Controls.Add(Me.txtUnpromotionAverage)
            Me.grpBaseInfo.Controls.Add(Me.label3)
            Me.grpBaseInfo.Controls.Add(Me.txtUnpromotionRate)
            Me.grpBaseInfo.Controls.Add(Me.txtRevenueStrDate)
            Me.grpBaseInfo.Controls.Add(Me.label5)
            Me.grpBaseInfo.Controls.Add(Me.txtCapPromotionAverage)
            Me.grpBaseInfo.Controls.Add(Me.txtnewStaffAverage)
            Me.grpBaseInfo.Controls.Add(Me.label4)
            Me.grpBaseInfo.Controls.Add(Me.txtRevenueEndDate)
            Me.grpBaseInfo.Controls.Add(Me.label1)
            Me.grpBaseInfo.Controls.Add(Me.label2)
            Me.grpBaseInfo.Location = New System.Drawing.Point(84, 17)
            Me.grpBaseInfo.Name = "grpBaseInfo"
            Me.grpBaseInfo.Size = New System.Drawing.Size(870, 112)
            Me.grpBaseInfo.TabIndex = 0
            Me.grpBaseInfo.TabStop = False
            '
            'label20
            '
            Me.label20.AutoSize = True
            Me.label20.Location = New System.Drawing.Point(214, 53)
            Me.label20.Name = "label20"
            Me.label20.Size = New System.Drawing.Size(24, 16)
            Me.label20.TabIndex = 148
            Me.label20.Text = "çŒ"
            '
            'label19
            '
            Me.label19.AutoSize = True
            Me.label19.Location = New System.Drawing.Point(834, 85)
            Me.label19.Name = "label19"
            Me.label19.Size = New System.Drawing.Size(24, 16)
            Me.label19.TabIndex = 147
            Me.label19.Text = "Åì"
            '
            'label18
            '
            Me.label18.AutoSize = True
            Me.label18.Location = New System.Drawing.Point(198, 85)
            Me.label18.Name = "label18"
            Me.label18.Size = New System.Drawing.Size(24, 16)
            Me.label18.TabIndex = 146
            Me.label18.Text = "çŒ"
            '
            'label15
            '
            Me.label15.AutoSize = True
            Me.label15.Location = New System.Drawing.Point(599, 85)
            Me.label15.Name = "label15"
            Me.label15.Size = New System.Drawing.Size(24, 16)
            Me.label15.TabIndex = 145
            Me.label15.Text = "çŒ"
            '
            'label9
            '
            Me.label9.AutoSize = True
            Me.label9.Location = New System.Drawing.Point(396, 85)
            Me.label9.Name = "label9"
            Me.label9.Size = New System.Drawing.Size(24, 16)
            Me.label9.TabIndex = 144
            Me.label9.Text = "î{"
            '
            'label17
            '
            Me.label17.AutoSize = True
            Me.label17.Location = New System.Drawing.Point(444, 53)
            Me.label17.Name = "label17"
            Me.label17.Size = New System.Drawing.Size(24, 16)
            Me.label17.TabIndex = 143
            Me.label17.Text = "çŒ"
            '
            'btnBaseChange
            '
            Me.btnBaseChange.Location = New System.Drawing.Point(735, 18)
            Me.btnBaseChange.Name = "btnBaseChange"
            Me.btnBaseChange.Size = New System.Drawing.Size(116, 32)
            Me.btnBaseChange.TabIndex = 0
            Me.btnBaseChange.Text = "äÓèÄèÓïÒïœçX"
            Me.btnBaseChange.UseVisualStyleBackColor = True
            '
            'label31
            '
            Me.label31.AutoSize = True
            Me.label31.Location = New System.Drawing.Point(434, 85)
            Me.label31.Name = "label31"
            Me.label31.Size = New System.Drawing.Size(109, 16)
            Me.label31.TabIndex = 139
            Me.label31.Text = "ÉVÉjÉAäÓèÄîNóÓ"
            '
            'txtSeniorAverage
            '
            Me.txtSeniorAverage.BackColor = System.Drawing.Color.LightYellow
            Me.txtSeniorAverage.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtSeniorAverage.Location = New System.Drawing.Point(543, 82)
            Me.txtSeniorAverage.MaxLength = 20
            Me.txtSeniorAverage.Name = "txtSeniorAverage"
            Me.txtSeniorAverage.Require = True
            Me.txtSeniorAverage.Size = New System.Drawing.Size(54, 23)
            Me.txtSeniorAverage.TabIndex = 0
            Me.txtSeniorAverage.TabStop = False
            Me.txtSeniorAverage.Tag = "îÒëgçáàıäÓèÄîNóÓ"
            Me.txtSeniorAverage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'lbltitle
            '
            Me.lbltitle.AutoSize = True
            Me.lbltitle.Location = New System.Drawing.Point(6, 21)
            Me.lbltitle.Name = "lbltitle"
            Me.lbltitle.Size = New System.Drawing.Size(94, 16)
            Me.lbltitle.TabIndex = 137
            Me.lbltitle.Text = "ëÊÇTÇRä˙ó\ëz"
            '
            'label7
            '
            Me.label7.AutoSize = True
            Me.label7.Location = New System.Drawing.Point(637, 85)
            Me.label7.Name = "label7"
            Me.label7.Size = New System.Drawing.Size(141, 16)
            Me.label7.TabIndex = 136
            Me.label7.Text = "ÉVÉjÉAëgçáàıécë∂ó¶"
            '
            'txtSeniorStayRate
            '
            Me.txtSeniorStayRate.BackColor = System.Drawing.Color.LightYellow
            Me.txtSeniorStayRate.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtSeniorStayRate.Location = New System.Drawing.Point(778, 82)
            Me.txtSeniorStayRate.MaxLength = 20
            Me.txtSeniorStayRate.Name = "txtSeniorStayRate"
            Me.txtSeniorStayRate.Require = True
            Me.txtSeniorStayRate.Size = New System.Drawing.Size(54, 23)
            Me.txtSeniorStayRate.TabIndex = 0
            Me.txtSeniorStayRate.TabStop = False
            Me.txtSeniorStayRate.Tag = "ÉVÉjÉAëgçáàıécë∂ó¶"
            Me.txtSeniorStayRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'label6
            '
            Me.label6.AutoSize = True
            Me.label6.Location = New System.Drawing.Point(6, 85)
            Me.label6.Name = "label6"
            Me.label6.Size = New System.Drawing.Size(136, 16)
            Me.label6.TabIndex = 134
            Me.label6.Text = "îÒëgçáàıäÓèÄîNóÓ"
            '
            'txtUnpromotionAverage
            '
            Me.txtUnpromotionAverage.BackColor = System.Drawing.Color.LightYellow
            Me.txtUnpromotionAverage.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtUnpromotionAverage.Location = New System.Drawing.Point(142, 82)
            Me.txtUnpromotionAverage.MaxLength = 20
            Me.txtUnpromotionAverage.Name = "txtUnpromotionAverage"
            Me.txtUnpromotionAverage.Require = True
            Me.txtUnpromotionAverage.Size = New System.Drawing.Size(54, 23)
            Me.txtUnpromotionAverage.TabIndex = 0
            Me.txtUnpromotionAverage.TabStop = False
            Me.txtUnpromotionAverage.Tag = "îÒëgçáàıäÓèÄîNóÓ"
            Me.txtUnpromotionAverage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'label3
            '
            Me.label3.AutoSize = True
            Me.label3.Location = New System.Drawing.Point(236, 85)
            Me.label3.Name = "label3"
            Me.label3.Size = New System.Drawing.Size(104, 16)
            Me.label3.TabIndex = 132
            Me.label3.Text = "îÒëgçáàıî{ó¶"
            '
            'txtUnpromotionRate
            '
            Me.txtUnpromotionRate.BackColor = System.Drawing.Color.LightYellow
            Me.txtUnpromotionRate.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtUnpromotionRate.Location = New System.Drawing.Point(340, 82)
            Me.txtUnpromotionRate.MaxLength = 20
            Me.txtUnpromotionRate.Name = "txtUnpromotionRate"
            Me.txtUnpromotionRate.Require = True
            Me.txtUnpromotionRate.Size = New System.Drawing.Size(54, 23)
            Me.txtUnpromotionRate.TabIndex = 0
            Me.txtUnpromotionRate.TabStop = False
            Me.txtUnpromotionRate.Tag = "îÒëgçáàıî{ó¶"
            Me.txtUnpromotionRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtRevenueStrDate
            '
            Me.txtRevenueStrDate.BackColor = System.Drawing.Color.LightYellow
            Me.txtRevenueStrDate.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtRevenueStrDate.Location = New System.Drawing.Point(221, 18)
            Me.txtRevenueStrDate.Name = "txtRevenueStrDate"
            Me.txtRevenueStrDate.ReadOnly = True
            Me.txtRevenueStrDate.Require = False
            Me.txtRevenueStrDate.Size = New System.Drawing.Size(120, 23)
            Me.txtRevenueStrDate.TabIndex = 0
            Me.txtRevenueStrDate.TabStop = False
            '
            'label5
            '
            Me.label5.AutoSize = True
            Me.label5.Location = New System.Drawing.Point(252, 53)
            Me.label5.Name = "label5"
            Me.label5.Size = New System.Drawing.Size(136, 16)
            Me.label5.TabIndex = 129
            Me.label5.Text = "ã@í∑è∏äiäÓèÄîNóÓ"
            '
            'txtCapPromotionAverage
            '
            Me.txtCapPromotionAverage.BackColor = System.Drawing.Color.LightYellow
            Me.txtCapPromotionAverage.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtCapPromotionAverage.Location = New System.Drawing.Point(388, 50)
            Me.txtCapPromotionAverage.MaxLength = 20
            Me.txtCapPromotionAverage.Name = "txtCapPromotionAverage"
            Me.txtCapPromotionAverage.Require = True
            Me.txtCapPromotionAverage.Size = New System.Drawing.Size(54, 23)
            Me.txtCapPromotionAverage.TabIndex = 0
            Me.txtCapPromotionAverage.TabStop = False
            Me.txtCapPromotionAverage.Tag = "ã@í∑è∏äiäÓèÄîNóÓ"
            Me.txtCapPromotionAverage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'txtnewStaffAverage
            '
            Me.txtnewStaffAverage.BackColor = System.Drawing.Color.LightYellow
            Me.txtnewStaffAverage.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtnewStaffAverage.Location = New System.Drawing.Point(158, 50)
            Me.txtnewStaffAverage.MaxLength = 20
            Me.txtnewStaffAverage.Name = "txtnewStaffAverage"
            Me.txtnewStaffAverage.Require = True
            Me.txtnewStaffAverage.Size = New System.Drawing.Size(54, 23)
            Me.txtnewStaffAverage.TabIndex = 0
            Me.txtnewStaffAverage.TabStop = False
            Me.txtnewStaffAverage.Tag = "êVì¸ëgçáàıäÓèÄîNóÓ"
            Me.txtnewStaffAverage.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'label4
            '
            Me.label4.AutoSize = True
            Me.label4.Location = New System.Drawing.Point(6, 53)
            Me.label4.Name = "label4"
            Me.label4.Size = New System.Drawing.Size(152, 16)
            Me.label4.TabIndex = 126
            Me.label4.Text = "êVì¸ëgçáàıäÓèÄîNóÓ"
            '
            'txtRevenueEndDate
            '
            Me.txtRevenueEndDate.BackColor = System.Drawing.Color.LightYellow
            Me.txtRevenueEndDate.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtRevenueEndDate.Location = New System.Drawing.Point(377, 18)
            Me.txtRevenueEndDate.Name = "txtRevenueEndDate"
            Me.txtRevenueEndDate.ReadOnly = True
            Me.txtRevenueEndDate.Require = False
            Me.txtRevenueEndDate.Size = New System.Drawing.Size(120, 23)
            Me.txtRevenueEndDate.TabIndex = 0
            Me.txtRevenueEndDate.TabStop = False
            '
            'label1
            '
            Me.label1.AutoSize = True
            Me.label1.Location = New System.Drawing.Point(347, 21)
            Me.label1.Name = "label1"
            Me.label1.Size = New System.Drawing.Size(24, 16)
            Me.label1.TabIndex = 123
            Me.label1.Text = "Å`"
            '
            'label2
            '
            Me.label2.AutoSize = True
            Me.label2.Location = New System.Drawing.Point(146, 21)
            Me.label2.Name = "label2"
            Me.label2.Size = New System.Drawing.Size(72, 16)
            Me.label2.TabIndex = 119
            Me.label2.Text = "ó\ëzä˙ä‘"
            '
            'grpStatisticalTable
            '
            Me.grpStatisticalTable.Controls.Add(Me.btnExpenditureRevenuePrint)
            Me.grpStatisticalTable.Controls.Add(Me.label22)
            Me.grpStatisticalTable.Controls.Add(Me.label21)
            Me.grpStatisticalTable.Controls.Add(Me.groupBox2)
            Me.grpStatisticalTable.Controls.Add(Me.groupBox1)
            Me.grpStatisticalTable.Controls.Add(Me.lblSeniorRetireMoneyTotal)
            Me.grpStatisticalTable.Controls.Add(Me.lblFeRetireMoneyTotal)
            Me.grpStatisticalTable.Controls.Add(Me.lblCopRetireMoneyTotal)
            Me.grpStatisticalTable.Controls.Add(Me.lblCapRetireMoneyTotal)
            Me.grpStatisticalTable.Controls.Add(Me.lblCapUpMoneyTotal)
            Me.grpStatisticalTable.Controls.Add(Me.label27)
            Me.grpStatisticalTable.Controls.Add(Me.lblNewUnionMoneyTotal)
            Me.grpStatisticalTable.Controls.Add(Me.label8)
            Me.grpStatisticalTable.Controls.Add(Me.lblRevenueExpenditureTtl)
            Me.grpStatisticalTable.Controls.Add(Me.lblSeniorRetireMonthwork)
            Me.grpStatisticalTable.Controls.Add(Me.lblSeniorRetireMemberTotal)
            Me.grpStatisticalTable.Controls.Add(Me.lblFeRetireMonthwork)
            Me.grpStatisticalTable.Controls.Add(Me.lblCopRetireMonthwork)
            Me.grpStatisticalTable.Controls.Add(Me.lblCapRetireMonthwork)
            Me.grpStatisticalTable.Controls.Add(Me.lblCapUpMonthwork)
            Me.grpStatisticalTable.Controls.Add(Me.label24)
            Me.grpStatisticalTable.Controls.Add(Me.lblNewUnionMonthwork)
            Me.grpStatisticalTable.Controls.Add(Me.lblFeRetireMemberTotal)
            Me.grpStatisticalTable.Controls.Add(Me.lblCopRetireMemberTotal)
            Me.grpStatisticalTable.Controls.Add(Me.lblCapRetireMemberTotal)
            Me.grpStatisticalTable.Controls.Add(Me.lblCapUpTotal)
            Me.grpStatisticalTable.Controls.Add(Me.label14)
            Me.grpStatisticalTable.Controls.Add(Me.lblNewUnionEntryTotal)
            Me.grpStatisticalTable.Controls.Add(Me.label10)
            Me.grpStatisticalTable.Controls.Add(Me.lblReviseRevenuettl)
            Me.grpStatisticalTable.Controls.Add(Me.flxStatisticalTable)
            Me.grpStatisticalTable.Location = New System.Drawing.Point(84, 133)
            Me.grpStatisticalTable.Name = "grpStatisticalTable"
            Me.grpStatisticalTable.Size = New System.Drawing.Size(871, 572)
            Me.grpStatisticalTable.TabIndex = 1
            Me.grpStatisticalTable.TabStop = False
            Me.grpStatisticalTable.Text = "ìùåvèÓïÒ"
            '
            'btnExpenditureRevenuePrint
            '
            Me.btnExpenditureRevenuePrint.Location = New System.Drawing.Point(682, 532)
            Me.btnExpenditureRevenuePrint.Name = "btnExpenditureRevenuePrint"
            Me.btnExpenditureRevenuePrint.Size = New System.Drawing.Size(146, 32)
            Me.btnExpenditureRevenuePrint.TabIndex = 2
            Me.btnExpenditureRevenuePrint.Text = "ó\ëzé˚ì¸ ÉvÉåàÛç¸"
            Me.btnExpenditureRevenuePrint.UseVisualStyleBackColor = True
            '
            'label22
            '
            Me.label22.AutoSize = True
            Me.label22.Location = New System.Drawing.Point(370, 540)
            Me.label22.Name = "label22"
            Me.label22.Size = New System.Drawing.Size(24, 16)
            Me.label22.TabIndex = 9
            Me.label22.Text = "â~"
            Me.label22.Visible = False
            '
            'label21
            '
            Me.label21.AutoSize = True
            Me.label21.Location = New System.Drawing.Point(631, 540)
            Me.label21.Name = "label21"
            Me.label21.Size = New System.Drawing.Size(24, 16)
            Me.label21.TabIndex = 8
            Me.label21.Text = "â~"
            Me.label21.Visible = False
            '
            'groupBox2
            '
            Me.groupBox2.Controls.Add(Me.label25)
            Me.groupBox2.Controls.Add(Me.lblSeniorTotal)
            Me.groupBox2.Controls.Add(Me.label11)
            Me.groupBox2.Controls.Add(Me.lblSeniorMonthwork)
            Me.groupBox2.Controls.Add(Me.label16)
            Me.groupBox2.Location = New System.Drawing.Point(33, 453)
            Me.groupBox2.Name = "groupBox2"
            Me.groupBox2.Size = New System.Drawing.Size(795, 56)
            Me.groupBox2.TabIndex = 3
            Me.groupBox2.TabStop = False
            Me.groupBox2.Text = "ÉVÉjÉAëgçáàı"
            '
            'label25
            '
            Me.label25.AutoSize = True
            Me.label25.Location = New System.Drawing.Point(688, 28)
            Me.label25.Name = "label25"
            Me.label25.Size = New System.Drawing.Size(24, 16)
            Me.label25.TabIndex = 166
            Me.label25.Text = "â~"
            Me.label25.Visible = False
            '
            'lblSeniorTotal
            '
            Me.lblSeniorTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.lblSeniorTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSeniorTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSeniorTotal.Location = New System.Drawing.Point(576, 26)
            Me.lblSeniorTotal.Name = "lblSeniorTotal"
            Me.lblSeniorTotal.Size = New System.Drawing.Size(110, 21)
            Me.lblSeniorTotal.TabIndex = 165
            Me.lblSeniorTotal.Text = "999,999,999"
            Me.lblSeniorTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label11
            '
            Me.label11.AutoSize = True
            Me.label11.Location = New System.Drawing.Point(419, 28)
            Me.label11.Name = "label11"
            Me.label11.Size = New System.Drawing.Size(157, 16)
            Me.label11.TabIndex = 164
            Me.label11.Text = "ÉVÉjÉAëgçáàıçáåvã‡äz"
            '
            'lblSeniorMonthwork
            '
            Me.lblSeniorMonthwork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSeniorMonthwork.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSeniorMonthwork.Location = New System.Drawing.Point(255, 26)
            Me.lblSeniorMonthwork.Name = "lblSeniorMonthwork"
            Me.lblSeniorMonthwork.Size = New System.Drawing.Size(110, 21)
            Me.lblSeniorMonthwork.TabIndex = 163
            Me.lblSeniorMonthwork.Text = "999,999,999"
            Me.lblSeniorMonthwork.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label16
            '
            Me.label16.AutoSize = True
            Me.label16.Location = New System.Drawing.Point(98, 28)
            Me.label16.Name = "label16"
            Me.label16.Size = New System.Drawing.Size(157, 16)
            Me.label16.TabIndex = 162
            Me.label16.Text = "ÉVÉjÉAëgçáàıçáåvêlåé"
            '
            'groupBox1
            '
            Me.groupBox1.Controls.Add(Me.label23)
            Me.groupBox1.Controls.Add(Me.lblUnpromotionTotal)
            Me.groupBox1.Controls.Add(Me.label30)
            Me.groupBox1.Controls.Add(Me.lblUnpromotionPersonsMonthwork)
            Me.groupBox1.Controls.Add(Me.label13)
            Me.groupBox1.Controls.Add(Me.txtUnpromotionPersons)
            Me.groupBox1.Controls.Add(Me.label12)
            Me.groupBox1.Location = New System.Drawing.Point(33, 387)
            Me.groupBox1.Name = "groupBox1"
            Me.groupBox1.Size = New System.Drawing.Size(795, 56)
            Me.groupBox1.TabIndex = 1
            Me.groupBox1.TabStop = False
            Me.groupBox1.Text = "îÒëgçáàı"
            '
            'label23
            '
            Me.label23.AutoSize = True
            Me.label23.Location = New System.Drawing.Point(769, 26)
            Me.label23.Name = "label23"
            Me.label23.Size = New System.Drawing.Size(24, 16)
            Me.label23.TabIndex = 9
            Me.label23.Text = "â~"
            Me.label23.Visible = False
            '
            'lblUnpromotionTotal
            '
            Me.lblUnpromotionTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
            Me.lblUnpromotionTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblUnpromotionTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblUnpromotionTotal.Location = New System.Drawing.Point(657, 24)
            Me.lblUnpromotionTotal.Name = "lblUnpromotionTotal"
            Me.lblUnpromotionTotal.Size = New System.Drawing.Size(110, 21)
            Me.lblUnpromotionTotal.TabIndex = 165
            Me.lblUnpromotionTotal.Text = "999,999,999"
            Me.lblUnpromotionTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label30
            '
            Me.label30.AutoSize = True
            Me.label30.Location = New System.Drawing.Point(521, 26)
            Me.label30.Name = "label30"
            Me.label30.Size = New System.Drawing.Size(136, 16)
            Me.label30.TabIndex = 164
            Me.label30.Text = "îÒëgçáàıçáåvã‡äz"
            '
            'lblUnpromotionPersonsMonthwork
            '
            Me.lblUnpromotionPersonsMonthwork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblUnpromotionPersonsMonthwork.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblUnpromotionPersonsMonthwork.Location = New System.Drawing.Point(382, 24)
            Me.lblUnpromotionPersonsMonthwork.Name = "lblUnpromotionPersonsMonthwork"
            Me.lblUnpromotionPersonsMonthwork.Size = New System.Drawing.Size(110, 21)
            Me.lblUnpromotionPersonsMonthwork.TabIndex = 163
            Me.lblUnpromotionPersonsMonthwork.Text = "999,999,999"
            Me.lblUnpromotionPersonsMonthwork.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label13
            '
            Me.label13.AutoSize = True
            Me.label13.Location = New System.Drawing.Point(246, 26)
            Me.label13.Name = "label13"
            Me.label13.Size = New System.Drawing.Size(136, 16)
            Me.label13.TabIndex = 162
            Me.label13.Text = "îÒëgçáàıî≠ê∂êlåé"
            '
            'txtUnpromotionPersons
            '
            Me.txtUnpromotionPersons.BackColor = System.Drawing.Color.White
            Me.txtUnpromotionPersons.FieldAttribute = UnionAct.GUI.Common.EFieldAttribute.NONE
            Me.txtUnpromotionPersons.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.txtUnpromotionPersons.Location = New System.Drawing.Point(165, 23)
            Me.txtUnpromotionPersons.Margin = New System.Windows.Forms.Padding(4)
            Me.txtUnpromotionPersons.MaxLength = 3
            Me.txtUnpromotionPersons.Name = "txtUnpromotionPersons"
            Me.txtUnpromotionPersons.Require = True
            Me.txtUnpromotionPersons.Size = New System.Drawing.Size(49, 23)
            Me.txtUnpromotionPersons.TabIndex = 0
            Me.txtUnpromotionPersons.Tag = "ì˙íˆï\ï\é¶ñºèÃ"
            Me.txtUnpromotionPersons.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'label12
            '
            Me.label12.AutoSize = True
            Me.label12.Location = New System.Drawing.Point(29, 26)
            Me.label12.Name = "label12"
            Me.label12.Size = New System.Drawing.Size(136, 16)
            Me.label12.TabIndex = 160
            Me.label12.Text = "îÒëgçáàıî≠ê∂êlêî"
            '
            'lblSeniorRetireMoneyTotal
            '
            Me.lblSeniorRetireMoneyTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
            Me.lblSeniorRetireMoneyTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSeniorRetireMoneyTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSeniorRetireMoneyTotal.Location = New System.Drawing.Point(692, 351)
            Me.lblSeniorRetireMoneyTotal.Name = "lblSeniorRetireMoneyTotal"
            Me.lblSeniorRetireMoneyTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblSeniorRetireMoneyTotal.TabIndex = 170
            Me.lblSeniorRetireMoneyTotal.Text = "999,999,999"
            Me.lblSeniorRetireMoneyTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblFeRetireMoneyTotal
            '
            Me.lblFeRetireMoneyTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
            Me.lblFeRetireMoneyTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblFeRetireMoneyTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblFeRetireMoneyTotal.Location = New System.Drawing.Point(574, 351)
            Me.lblFeRetireMoneyTotal.Name = "lblFeRetireMoneyTotal"
            Me.lblFeRetireMoneyTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblFeRetireMoneyTotal.TabIndex = 169
            Me.lblFeRetireMoneyTotal.Text = "999,999,999"
            Me.lblFeRetireMoneyTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCopRetireMoneyTotal
            '
            Me.lblCopRetireMoneyTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
            Me.lblCopRetireMoneyTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCopRetireMoneyTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCopRetireMoneyTotal.Location = New System.Drawing.Point(456, 351)
            Me.lblCopRetireMoneyTotal.Name = "lblCopRetireMoneyTotal"
            Me.lblCopRetireMoneyTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblCopRetireMoneyTotal.TabIndex = 168
            Me.lblCopRetireMoneyTotal.Text = "999,999,999"
            Me.lblCopRetireMoneyTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCapRetireMoneyTotal
            '
            Me.lblCapRetireMoneyTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
            Me.lblCapRetireMoneyTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCapRetireMoneyTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCapRetireMoneyTotal.Location = New System.Drawing.Point(338, 351)
            Me.lblCapRetireMoneyTotal.Name = "lblCapRetireMoneyTotal"
            Me.lblCapRetireMoneyTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblCapRetireMoneyTotal.TabIndex = 167
            Me.lblCapRetireMoneyTotal.Text = "999,999,999"
            Me.lblCapRetireMoneyTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCapUpMoneyTotal
            '
            Me.lblCapUpMoneyTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.lblCapUpMoneyTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCapUpMoneyTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCapUpMoneyTotal.Location = New System.Drawing.Point(220, 351)
            Me.lblCapUpMoneyTotal.Name = "lblCapUpMoneyTotal"
            Me.lblCapUpMoneyTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblCapUpMoneyTotal.TabIndex = 166
            Me.lblCapUpMoneyTotal.Text = "999,999,999"
            Me.lblCapUpMoneyTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label27
            '
            Me.label27.AutoSize = True
            Me.label27.Location = New System.Drawing.Point(30, 353)
            Me.label27.Name = "label27"
            Me.label27.Size = New System.Drawing.Size(72, 16)
            Me.label27.TabIndex = 165
            Me.label27.Text = "çáåvã‡äz"
            '
            'lblNewUnionMoneyTotal
            '
            Me.lblNewUnionMoneyTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.lblNewUnionMoneyTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblNewUnionMoneyTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblNewUnionMoneyTotal.Location = New System.Drawing.Point(103, 351)
            Me.lblNewUnionMoneyTotal.Name = "lblNewUnionMoneyTotal"
            Me.lblNewUnionMoneyTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblNewUnionMoneyTotal.TabIndex = 164
            Me.lblNewUnionMoneyTotal.Text = "999,999,999"
            Me.lblNewUnionMoneyTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label8
            '
            Me.label8.AutoSize = True
            Me.label8.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.label8.Location = New System.Drawing.Point(142, 540)
            Me.label8.Name = "label8"
            Me.label8.Size = New System.Drawing.Size(110, 16)
            Me.label8.TabIndex = 163
            Me.label8.Text = "é˚ì¸ó\ëzëçäz"
            '
            'lblRevenueExpenditureTtl
            '
            Me.lblRevenueExpenditureTtl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblRevenueExpenditureTtl.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblRevenueExpenditureTtl.Location = New System.Drawing.Point(252, 538)
            Me.lblRevenueExpenditureTtl.Name = "lblRevenueExpenditureTtl"
            Me.lblRevenueExpenditureTtl.Size = New System.Drawing.Size(116, 21)
            Me.lblRevenueExpenditureTtl.TabIndex = 162
            Me.lblRevenueExpenditureTtl.Text = "999,999,999"
            Me.lblRevenueExpenditureTtl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblSeniorRetireMonthwork
            '
            Me.lblSeniorRetireMonthwork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSeniorRetireMonthwork.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSeniorRetireMonthwork.Location = New System.Drawing.Point(692, 321)
            Me.lblSeniorRetireMonthwork.Name = "lblSeniorRetireMonthwork"
            Me.lblSeniorRetireMonthwork.Size = New System.Drawing.Size(114, 21)
            Me.lblSeniorRetireMonthwork.TabIndex = 161
            Me.lblSeniorRetireMonthwork.Text = "999,999,999"
            Me.lblSeniorRetireMonthwork.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblSeniorRetireMemberTotal
            '
            Me.lblSeniorRetireMemberTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSeniorRetireMemberTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSeniorRetireMemberTotal.Location = New System.Drawing.Point(692, 291)
            Me.lblSeniorRetireMemberTotal.Name = "lblSeniorRetireMemberTotal"
            Me.lblSeniorRetireMemberTotal.Size = New System.Drawing.Size(115, 21)
            Me.lblSeniorRetireMemberTotal.TabIndex = 160
            Me.lblSeniorRetireMemberTotal.Text = "999,999,999"
            Me.lblSeniorRetireMemberTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblFeRetireMonthwork
            '
            Me.lblFeRetireMonthwork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblFeRetireMonthwork.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblFeRetireMonthwork.Location = New System.Drawing.Point(574, 321)
            Me.lblFeRetireMonthwork.Name = "lblFeRetireMonthwork"
            Me.lblFeRetireMonthwork.Size = New System.Drawing.Size(114, 21)
            Me.lblFeRetireMonthwork.TabIndex = 158
            Me.lblFeRetireMonthwork.Text = "999,999,999"
            Me.lblFeRetireMonthwork.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCopRetireMonthwork
            '
            Me.lblCopRetireMonthwork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCopRetireMonthwork.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCopRetireMonthwork.Location = New System.Drawing.Point(456, 321)
            Me.lblCopRetireMonthwork.Name = "lblCopRetireMonthwork"
            Me.lblCopRetireMonthwork.Size = New System.Drawing.Size(114, 21)
            Me.lblCopRetireMonthwork.TabIndex = 157
            Me.lblCopRetireMonthwork.Text = "999,999,999"
            Me.lblCopRetireMonthwork.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCapRetireMonthwork
            '
            Me.lblCapRetireMonthwork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCapRetireMonthwork.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCapRetireMonthwork.Location = New System.Drawing.Point(338, 321)
            Me.lblCapRetireMonthwork.Name = "lblCapRetireMonthwork"
            Me.lblCapRetireMonthwork.Size = New System.Drawing.Size(114, 21)
            Me.lblCapRetireMonthwork.TabIndex = 156
            Me.lblCapRetireMonthwork.Text = "999,999,999"
            Me.lblCapRetireMonthwork.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCapUpMonthwork
            '
            Me.lblCapUpMonthwork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCapUpMonthwork.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCapUpMonthwork.Location = New System.Drawing.Point(220, 321)
            Me.lblCapUpMonthwork.Name = "lblCapUpMonthwork"
            Me.lblCapUpMonthwork.Size = New System.Drawing.Size(114, 21)
            Me.lblCapUpMonthwork.TabIndex = 155
            Me.lblCapUpMonthwork.Text = "999,999,999"
            Me.lblCapUpMonthwork.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label24
            '
            Me.label24.AutoSize = True
            Me.label24.Location = New System.Drawing.Point(30, 323)
            Me.label24.Name = "label24"
            Me.label24.Size = New System.Drawing.Size(72, 16)
            Me.label24.TabIndex = 154
            Me.label24.Text = "çáåvêlåé"
            '
            'lblNewUnionMonthwork
            '
            Me.lblNewUnionMonthwork.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblNewUnionMonthwork.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblNewUnionMonthwork.Location = New System.Drawing.Point(103, 321)
            Me.lblNewUnionMonthwork.Name = "lblNewUnionMonthwork"
            Me.lblNewUnionMonthwork.Size = New System.Drawing.Size(114, 21)
            Me.lblNewUnionMonthwork.TabIndex = 153
            Me.lblNewUnionMonthwork.Text = "999,999,999"
            Me.lblNewUnionMonthwork.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblFeRetireMemberTotal
            '
            Me.lblFeRetireMemberTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblFeRetireMemberTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblFeRetireMemberTotal.Location = New System.Drawing.Point(574, 291)
            Me.lblFeRetireMemberTotal.Name = "lblFeRetireMemberTotal"
            Me.lblFeRetireMemberTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblFeRetireMemberTotal.TabIndex = 152
            Me.lblFeRetireMemberTotal.Text = "999,999,999"
            Me.lblFeRetireMemberTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCopRetireMemberTotal
            '
            Me.lblCopRetireMemberTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCopRetireMemberTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCopRetireMemberTotal.Location = New System.Drawing.Point(456, 291)
            Me.lblCopRetireMemberTotal.Name = "lblCopRetireMemberTotal"
            Me.lblCopRetireMemberTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblCopRetireMemberTotal.TabIndex = 151
            Me.lblCopRetireMemberTotal.Text = "999,999,999"
            Me.lblCopRetireMemberTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCapRetireMemberTotal
            '
            Me.lblCapRetireMemberTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCapRetireMemberTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCapRetireMemberTotal.Location = New System.Drawing.Point(338, 291)
            Me.lblCapRetireMemberTotal.Name = "lblCapRetireMemberTotal"
            Me.lblCapRetireMemberTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblCapRetireMemberTotal.TabIndex = 150
            Me.lblCapRetireMemberTotal.Text = "999,999,999"
            Me.lblCapRetireMemberTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblCapUpTotal
            '
            Me.lblCapUpTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblCapUpTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblCapUpTotal.Location = New System.Drawing.Point(220, 291)
            Me.lblCapUpTotal.Name = "lblCapUpTotal"
            Me.lblCapUpTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblCapUpTotal.TabIndex = 149
            Me.lblCapUpTotal.Text = "999,999,999"
            Me.lblCapUpTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label14
            '
            Me.label14.AutoSize = True
            Me.label14.Location = New System.Drawing.Point(30, 293)
            Me.label14.Name = "label14"
            Me.label14.Size = New System.Drawing.Size(72, 16)
            Me.label14.TabIndex = 148
            Me.label14.Text = "çáåvêlêî"
            '
            'lblNewUnionEntryTotal
            '
            Me.lblNewUnionEntryTotal.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblNewUnionEntryTotal.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblNewUnionEntryTotal.Location = New System.Drawing.Point(103, 291)
            Me.lblNewUnionEntryTotal.Name = "lblNewUnionEntryTotal"
            Me.lblNewUnionEntryTotal.Size = New System.Drawing.Size(114, 21)
            Me.lblNewUnionEntryTotal.TabIndex = 147
            Me.lblNewUnionEntryTotal.Text = "999,999,999"
            Me.lblNewUnionEntryTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'label10
            '
            Me.label10.AutoSize = True
            Me.label10.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.label10.Location = New System.Drawing.Point(403, 540)
            Me.label10.Name = "label10"
            Me.label10.Size = New System.Drawing.Size(110, 16)
            Me.label10.TabIndex = 144
            Me.label10.Text = "ï‚ê≥å„é˚ì¸äz"
            '
            'lblReviseRevenuettl
            '
            Me.lblReviseRevenuettl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblReviseRevenuettl.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblReviseRevenuettl.Location = New System.Drawing.Point(513, 538)
            Me.lblReviseRevenuettl.Name = "lblReviseRevenuettl"
            Me.lblReviseRevenuettl.Size = New System.Drawing.Size(116, 21)
            Me.lblReviseRevenuettl.TabIndex = 143
            Me.lblReviseRevenuettl.Text = "999,999,999"
            Me.lblReviseRevenuettl.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'flxStatisticalTable
            '
            Me.flxStatisticalTable.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
            Me.flxStatisticalTable.AllowEditing = False
            Me.flxStatisticalTable.AllowMerging = C1.Win.C1FlexGrid.AllowMergingEnum.Free
            Me.flxStatisticalTable.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
            Me.flxStatisticalTable.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
            Me.flxStatisticalTable.ColumnInfo = "10,1,0,0,0,110,Columns:"
            Me.flxStatisticalTable.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
            Me.flxStatisticalTable.Location = New System.Drawing.Point(29, 22)
            Me.flxStatisticalTable.Name = "flxStatisticalTable"
            Me.flxStatisticalTable.Rows.Count = 13
            Me.flxStatisticalTable.Rows.DefaultSize = 22
            Me.flxStatisticalTable.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Cell
            Me.flxStatisticalTable.Size = New System.Drawing.Size(792, 262)
            Me.flxStatisticalTable.StyleInfo = resources.GetString("flxStatisticalTable.StyleInfo")
            Me.flxStatisticalTable.TabIndex = 0
            '
            'btnPrinting
            '
            Me.btnPrinting.Location = New System.Drawing.Point(84, 711)
            Me.btnPrinting.Name = "btnPrinting"
            Me.btnPrinting.Size = New System.Drawing.Size(116, 32)
            Me.btnPrinting.TabIndex = 2
            Me.btnPrinting.Text = "ÉvÉåàÛç¸"
            Me.btnPrinting.UseVisualStyleBackColor = True
            '
            'btnChange
            '
            Me.btnChange.Location = New System.Drawing.Point(719, 711)
            Me.btnChange.Name = "btnChange"
            Me.btnChange.Size = New System.Drawing.Size(116, 32)
            Me.btnChange.TabIndex = 3
            Me.btnChange.Text = "ì‡óeïœçX"
            Me.btnChange.UseVisualStyleBackColor = True
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(849, 711)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(116, 32)
            Me.btnCancel.TabIndex = 7
            Me.btnCancel.Text = "ÉLÉÉÉìÉZÉã"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnNewEntry
            '
            Me.btnNewEntry.Location = New System.Drawing.Point(719, 711)
            Me.btnNewEntry.Name = "btnNewEntry"
            Me.btnNewEntry.Size = New System.Drawing.Size(116, 32)
            Me.btnNewEntry.TabIndex = 6
            Me.btnNewEntry.Text = "ìoò^ämîF"
            Me.btnNewEntry.UseVisualStyleBackColor = True
            '
            'btnEntryCheck
            '
            Me.btnEntryCheck.Location = New System.Drawing.Point(719, 711)
            Me.btnEntryCheck.Name = "btnEntryCheck"
            Me.btnEntryCheck.Size = New System.Drawing.Size(116, 32)
            Me.btnEntryCheck.TabIndex = 4
            Me.btnEntryCheck.Text = "ìoò^ämîF"
            Me.btnEntryCheck.UseVisualStyleBackColor = True
            '
            'btnBack
            '
            Me.btnBack.Location = New System.Drawing.Point(849, 711)
            Me.btnBack.Name = "btnBack"
            Me.btnBack.Size = New System.Drawing.Size(116, 32)
            Me.btnBack.TabIndex = 5
            Me.btnBack.Text = "ñﬂÇÈ"
            Me.btnBack.UseVisualStyleBackColor = True
            '
            'CtlCrewPlan
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 16.0!)
            Me.Controls.Add(Me.grpStatisticalTable)
            Me.Controls.Add(Me.grpBaseInfo)
            Me.Controls.Add(Me.btnBack)
            Me.Controls.Add(Me.btnEntryCheck)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnNewEntry)
            Me.Controls.Add(Me.btnChange)
            Me.Controls.Add(Me.btnPrinting)
            Me.Font = New System.Drawing.Font("MS UI Gothic", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.Name = "CtlCrewPlan"
            Me.Size = New System.Drawing.Size(1026, 759)
            Me.grpBaseInfo.ResumeLayout(False)
            Me.grpBaseInfo.PerformLayout()
            Me.grpStatisticalTable.ResumeLayout(False)
            Me.grpStatisticalTable.PerformLayout()
            Me.groupBox2.ResumeLayout(False)
            Me.groupBox2.PerformLayout()
            Me.groupBox1.ResumeLayout(False)
            Me.groupBox1.PerformLayout()
            CType(Me.flxStatisticalTable, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Private Sub Load_Set(ByVal dtTable As DataTable)
            Try
                Dim length As Integer = dtTable.Rows.Item(0).Item("l_title").ToString.Length
                Me.lbltitle.Text = dtTable.Rows.Item(0).Item("l_title").ToString
                Me.lblUnpromotionPersonsMonthwork.Text = dtTable.Rows.Item(0).Item("s_unpromotion_persons_monthwork").ToString
                Me.lblRevenueExpenditureTtl.Text = String.Format("{0:N0}", Convert.ToInt64(dtTable.Rows.Item(0).Item("s_revenue_expenditure_ttl").ToString))
                Me.txtRevenueStrDate.Text = String.Concat(New String() {dtTable.Rows.Item(0).Item("d_revenue_str").ToString.Substring(0, 4), "îN", dtTable.Rows.Item(0).Item("d_revenue_str").ToString.Substring(4, 2), "åé", dtTable.Rows.Item(0).Item("d_revenue_str").ToString.Substring(6, 2), "ì˙"})
                Me.txtRevenueEndDate.Text = String.Concat(New String() {dtTable.Rows.Item(0).Item("d_revenue_end").ToString.Substring(0, 4), "îN", dtTable.Rows.Item(0).Item("d_revenue_end").ToString.Substring(4, 2), "åé", dtTable.Rows.Item(0).Item("d_revenue_end").ToString.Substring(6, 2), "ì˙"})
                Me.txtnewStaffAverage.Text = dtTable.Rows.Item(0).Item("s_new_staff_average").ToString
                Me.txtCapPromotionAverage.Text = dtTable.Rows.Item(0).Item("s_cap_promotion_average").ToString
                Me.txtUnpromotionAverage.Text = dtTable.Rows.Item(0).Item("s_unpromotion_average").ToString
                Me.txtUnpromotionRate.Text = String.Format("{0:#0.###}", dtTable.Rows.Item(0).Item("s_unpromotion_rate"))
                Me.txtSeniorAverage.Text = dtTable.Rows.Item(0).Item("s_senior_average").ToString
                Me.txtSeniorStayRate.Text = dtTable.Rows.Item(0).Item("s_senior_stay_rate").ToString
                Me.txtUnpromotionPersons.Text = dtTable.Rows.Item(0).Item("s_unpromotion_persons").ToString
                Me.txtRevenueStrDate.ReadOnly = True
                Me.txtRevenueEndDate.ReadOnly = True
                Me.txtnewStaffAverage.ReadOnly = True
                Me.txtCapPromotionAverage.ReadOnly = True
                Me.txtUnpromotionAverage.ReadOnly = True
                Me.txtUnpromotionRate.ReadOnly = True
                Me.txtSeniorAverage.ReadOnly = True
                Me.txtSeniorStayRate.ReadOnly = True
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

        Private Function MakeDataSet() As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet("dsMemberPlan")
                Dim table As DataTable = New CrewPlanFooterMap().CreateDataTablePhysName("dtFooter")
                Dim row As DataRow = table.NewRow
                row.Item("s_revenue_expenditure_ttl") = Convert.ToInt64(Me.dtTable.Rows.Item(0).Item("s_revenue_expenditure_ttl").ToString)
                row.Item("s_revise_new_staff") = Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_new_staff").ToString)
                row.Item("s_new_staff_money") = Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_new_staff_money").ToString)
                row.Item("s_revise_up") = Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_up").ToString)
                row.Item("s_revise_cap_retire") = Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_cap_retire").ToString)
                row.Item("s_revise_cop_retire") = Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_cop_retire").ToString)
                row.Item("s_revise_fe_retire") = Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_fe_retire").ToString)
                row.Item("s_revenue_unpromotion") = Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revenue_unpromotion").ToString)
                row.Item("s_revise_senior_retire") = Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_senior_union_dues").ToString)
                table.Rows.Add(row)
                Dim table2 As DataTable = New CrewPlanHeaderMap().CreateDataTablePhysName("dtHeader")
                Dim rowArray As DataRow() = Me.dtTable.Select
                Dim i As Integer
                For i = 0 To rowArray.Length - 1
                    table2.ImportRow(rowArray(i))
                Next i
                table2.Rows.Item(0).Item("l_unpromotion_rate") = Me.dtTable.Rows.Item(0).Item("s_unpromotion_rate").ToString.Replace(".000", "")
                table2.Rows.Item(0).Item("l_senior_stay_rate") = Me.dtTable.Rows.Item(0).Item("s_senior_stay_rate").ToString.Replace(".000", "")
                table2.Rows.Item(0).Item("s_unpromotion_persons") = Convert.ToInt32(Me.dtTableCalcData.Rows.Item(0).Item("s_unpromotion_persons").ToString)
                table2.Rows.Item(0).Item("l_period") = Me.dtTable.Rows.Item(0).Item("l_title").ToString
                Dim j As Integer
                For j = 0 To Me.dtTableFlexGrid.Rows.Count - 1
                    Dim str As String = ("l_yearmonth_" & Convert.ToInt32(CInt((j + 1))).ToString)
                    table2.Rows.Item(0).Item(str) = Me.dtTableFlexGrid.Rows.Item(j).Item("ëŒè€åé").ToString.Replace("/", "îN").Insert(Me.dtTableFlexGrid.Rows.Item(j).Item("ëŒè€åé").ToString.Length, "åé")
                Next j
                Dim table3 As DataTable = New CrewPlanDetailMap().CreateDataTablePhysName("dtDetail")
                Dim k As Integer
                For k = 0 To 6 - 1
                    row = table3.NewRow
                    table3.Rows.Add(row)
                Next k
                Dim m As Integer
                For m = 0 To Me.dtTableFlexGrid.Rows.Count - 1
                    Dim str2 As String = ("s_member_month_" & Convert.ToInt32(CInt((m + 1))).ToString)
                    table3.Rows.Item(0).Item(str2) = Me.dtTableFlexGrid.Rows.Item(m).Item("êVì¸ëgçáàıêî")
                Next m
                Dim n As Integer
                For n = 0 To Me.dtTableFlexGrid.Rows.Count - 1
                    Dim str3 As String = ("s_member_month_" & Convert.ToInt32(CInt((n + 1))).ToString)
                    table3.Rows.Item(1).Item(str3) = Me.dtTableFlexGrid.Rows.Item(n).Item("CAP" & "è∏äiêî")
                Next n
                Dim num6 As Integer
                For num6 = 0 To Me.dtTableFlexGrid.Rows.Count - 1
                    Dim str4 As String = ("s_member_month_" & Convert.ToInt32(CInt((num6 + 1))).ToString)
                    table3.Rows.Item(2).Item(str4) = Me.dtTableFlexGrid.Rows.Item(num6).Item("CAP" & "ëﬁêEé“êî")
                Next num6
                Dim num7 As Integer
                For num7 = 0 To Me.dtTableFlexGrid.Rows.Count - 1
                    Dim str5 As String = ("s_member_month_" & Convert.ToInt32(CInt((num7 + 1))).ToString)
                    table3.Rows.Item(3).Item(str5) = Me.dtTableFlexGrid.Rows.Item(num7).Item("COP" & "ëﬁêEé“êî")
                Next num7
                Dim num8 As Integer
                For num8 = 0 To Me.dtTableFlexGrid.Rows.Count - 1
                    Dim str6 As String = ("s_member_month_" & Convert.ToInt32(CInt((num8 + 1))).ToString)
                    table3.Rows.Item(4).Item(str6) = Me.dtTableFlexGrid.Rows.Item(num8).Item("FE" & "ëﬁêEé“êî")
                Next num8
                Dim num9 As Integer
                For num9 = 0 To Me.dtTableFlexGrid.Rows.Count - 1
                    Dim str7 As String = ("s_member_month_" & Convert.ToInt32(CInt((num9 + 1))).ToString)
                    table3.Rows.Item(5).Item(str7) = Me.dtTableFlexGrid.Rows.Item(num9).Item("ÉVÉjÉAëﬁêEé“êî")
                Next num9
                ds.Tables.Add(table2)
                ds.Tables.Add(table3)
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Private Sub Press_Enter(ByVal sender As Object, ByVal e As KeyPressEventArgs)
            Try
                If e.KeyChar.Equals("") Then
                    Me.SetNewReviseRevenueTtl()
                    Me.Caluculation()
                    Me.SetLabelText()
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

        Private Sub SetButtan(ByVal strButtonName As String)
            Try
                If MyBase._IsNewFlg Then
                    Me.btnPrinting.Visible = False
                    Me.btnExpenditureRevenuePrint.Visible = False
                    Me.btnChange.Visible = False
                    Me.btnBaseChange.Visible = False
                    Me.btnEntryCheck.Visible = False
                    Me.btnBack.Visible = False
                Else
                    Me.btnNewEntry.Visible = MyBase._IsNewFlg
                    Me.btnEntryCheck.Visible = False
                    Me.btnChange.Visible = MyBase._IsChangeFlg
                    Me.btnBaseChange.Visible = MyBase._IsChangeFlg
                    If Not Me.GetEntryRigth Then
                        Me.btnBaseChange.Visible = Me.GetEntryRigth
                        Me.btnChange.Visible = Me.GetEntryRigth
                    End If
                    Me.btnCancel.Visible = False
                    Me.btnPrinting.Visible = Me.PrintRight
                    Me.btnExpenditureRevenuePrint.Visible = Me.PrintRight
                    If (strButtonName = "btnBaseChange") Then
                        Me.btnChange.Visible = False
                        Me.btnBaseChange.Visible = False
                        Me.btnPrinting.Visible = False
                        Me.btnExpenditureRevenuePrint.Visible = False
                        Me.btnBack.Visible = False
                        Me.btnCancel.Visible = True
                        Me.btnEntryCheck.Visible = True
                    End If
                    If (strButtonName = "btnChange") Then
                        Me.btnChange.Visible = False
                        Me.btnPrinting.Visible = False
                        Me.btnExpenditureRevenuePrint.Visible = False
                        Me.btnBaseChange.Visible = False
                        Me.btnBack.Visible = False
                        Me.btnCancel.Visible = True
                        Me.btnEntryCheck.Visible = True
                    End If
                    If (strButtonName = "btnCancel") Then
                        Me.btnEntryCheck.Visible = False
                        Me.btnChange.Visible = True
                        Me.btnBack.Visible = True
                        Me.btnBaseChange.Visible = True
                        Me.btnExpenditureRevenuePrint.Visible = Me.PrintRight
                        Me.btnPrinting.Visible = Me.PrintRight
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

        Private Sub SetDetailFlexGrid(ByVal dtGridTable As DataTable)
            Try
                Me.dtTableFlexGrid = dtGridTable.Copy
                Me.flxStatisticalTable.DataSource = Me.dtTableFlexGrid
                Me.flxStatisticalTable.Cols.Item("ëŒè€åé").Style = Me.flxStatisticalTable.Styles.Fixed
                Me.flxStatisticalTable.Cols.Item(2).Format = "N0"
                Me.flxStatisticalTable.Cols.Item(3).Format = "N0"
                Me.flxStatisticalTable.Cols.Item(4).Format = "N0"
                Me.flxStatisticalTable.Cols.Item(5).Format = "N0"
                Me.flxStatisticalTable.Cols.Item(6).Format = "N0"
                Dim dicColWidthPair As New Dictionary(Of String, Integer)
                dicColWidthPair.Add("ëŒè€åé", 70)
                dicColWidthPair.Add("êVì¸ëgçáàıêî", 117)
                dicColWidthPair.Add("CAP" & "è∏äiêî", 117)
                dicColWidthPair.Add("CAP" & "ëﬁêEé“êî", 117)
                dicColWidthPair.Add("COP" & "ëﬁêEé“êî", 117)
                dicColWidthPair.Add("FE" & "ëﬁêEé“êî", 117)
                dicColWidthPair.Add("ÉVÉjÉAëﬁêEé“êî", 117)
                MDFinanceCommon.SetColsWidth(Me.flxStatisticalTable, dicColWidthPair)
                MDFinanceCommon.AdjustTextAlign(Me.flxStatisticalTable)
                Me.flxStatisticalTable.AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(4).Style.BackColor = Color.LightYellow
                Me.flxStatisticalTable.Cols.Item(5).Style.BackColor = Color.LightYellow
                Me.flxStatisticalTable.Cols.Item(6).Style.BackColor = Color.LightYellow
                Me.flxStatisticalTable.Cols.Item(7).Style.BackColor = Color.LightYellow
                Me.txtUnpromotionPersons.ReadOnly = True
                Me.btnEntryCheck.Visible = False
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

        Private Sub SetDetailReviseRevenueTtl()
            Try
                Me.lblReviseRevenuettl.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTable.Rows.Item(0).Item("s_revise_revenue_ttl").ToString))
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

        Private Sub SetFlexGrid(ByVal dtGridTable As DataTable)
            Try
                Me.dtTableFlexGrid = dtGridTable.Copy
                Me.flxStatisticalTable.DataSource = Me.dtTableFlexGrid
                Me.flxStatisticalTable.Cols.Item("ëŒè€åé").Style = Me.flxStatisticalTable.Styles.Fixed
                Me.flxStatisticalTable.Cols.Item(2).Format = "N0"
                Me.flxStatisticalTable.Cols.Item(3).Format = "N0"
                Me.flxStatisticalTable.Cols.Item(4).Format = "N0"
                Me.flxStatisticalTable.Cols.Item(5).Format = "N0"
                Me.flxStatisticalTable.Cols.Item(6).Format = "N0"
                Dim dicColWidthPair As New Dictionary(Of String, Integer)
                dicColWidthPair.Add("ëŒè€åé", 70)
                dicColWidthPair.Add("êVì¸ëgçáàıêî", 117)
                dicColWidthPair.Add("CAP" & "è∏äiêî", 117)
                dicColWidthPair.Add("CAP" & "ëﬁêEé“êî", 117)
                dicColWidthPair.Add("COP" & "ëﬁêEé“êî", 117)
                dicColWidthPair.Add("FE" & "ëﬁêEé“êî", 117)
                dicColWidthPair.Add("ÉVÉjÉAëﬁêEé“êî", 117)
                MDFinanceCommon.SetColsWidth(Me.flxStatisticalTable, dicColWidthPair)
                MDFinanceCommon.AdjustTextAlign(Me.flxStatisticalTable)
                Me.flxStatisticalTable.AllowEditing = True
                Me.flxStatisticalTable.Cols.Item("ëŒè€åé").AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(4).AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(5).AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(6).AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(7).AllowEditing = False
                Me.flxStatisticalTable.Cols.Item(4).Style.BackColor = Color.LightYellow
                Me.flxStatisticalTable.Cols.Item(5).Style.BackColor = Color.LightYellow
                Me.flxStatisticalTable.Cols.Item(6).Style.BackColor = Color.LightYellow
                Me.flxStatisticalTable.Cols.Item(7).Style.BackColor = Color.LightYellow
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

        Private Sub SetLabelText()
            Try
                Me.lblNewUnionEntryTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_new_staff_member_total").ToString))
                Me.lblNewUnionMonthwork.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_new_union_monthwork").ToString))
                Me.lblNewUnionMoneyTotal.Text = String.Format("{0:N0}", (Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_new_staff_money").ToString) + Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_new_staff").ToString)))
                Me.lblCapUpTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_new_cap_member_total").ToString))
                Me.lblCapUpMonthwork.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_cap_up_monthwork").ToString))
                Me.lblCapUpMoneyTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_up").ToString))
                Me.lblCapRetireMemberTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_cap_retire_member_total").ToString))
                Me.lblCapRetireMonthwork.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_cap_retire_monthwork").ToString))
                Me.lblCapRetireMoneyTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_cap_retire").ToString))
                Me.lblCopRetireMemberTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_cop_retire_member_total").ToString))
                Me.lblCopRetireMonthwork.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_cop_retire_monthwork").ToString))
                Me.lblCopRetireMoneyTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_cop_retire").ToString))
                Me.lblFeRetireMemberTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_fe_retire_member_total").ToString))
                Me.lblFeRetireMonthwork.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_fe_retire_monthwork").ToString))
                Me.lblFeRetireMoneyTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_fe_retire").ToString))
                Me.lblSeniorRetireMemberTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_senior_retire_member_total").ToString))
                Me.lblSeniorRetireMonthwork.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_senior_retire_monthwork").ToString))
                Me.lblSeniorMonthwork.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_retire_monthwork_total").ToString))
                Me.lblSeniorRetireMoneyTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_senior_retire").ToString))
                Me.lblSeniorTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_senior_union_dues").ToString))
                Me.lblUnpromotionTotal.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revenue_unpromotion").ToString))
                Me.lblReviseRevenuettl.Text = String.Format("{0:N0}", Convert.ToInt64(Me.dtTableCalcData.Rows.Item(0).Item("s_revise_revenue_ttl").ToString))
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

        Private Sub SetNewReviseRevenueTtl()
            Try
                If (Not String.IsNullOrEmpty(Me.txtUnpromotionPersons.Text) AndAlso Not String.IsNullOrEmpty(Me.txtUnpromotionRate.Text)) Then
                    Dim num As Integer
                    If Integer.TryParse(Me.txtUnpromotionPersons.Text, num) Then
                        If Not Me.txtUnpromotionPersons.Text.Contains("-") Then
                            Me.lblUnpromotionPersonsMonthwork.Text = String.Format("{0:N0}", PublicCommand.ToHalfAdjust(Convert.ToDouble(CDec((Convert.ToDecimal(Me.txtUnpromotionPersons.Text) * Convert.ToDecimal(Me.txtUnpromotionRate.Text)))), 1))
                            Me.txtUnpromotionPersons.Text = String.Format("{0:N0}", Convert.ToInt32(Me.txtUnpromotionPersons.Text))
                        Else
                            CLMsg.Show("GE0173")
                            Me.lblUnpromotionPersonsMonthwork.Text = String.Format("{0:N0}", "0")
                            Me.txtUnpromotionPersons.Text = String.Format("{0:N0}", "0")
                        End If
                    Else
                        CLMsg.Show("GE0178")
                        Me.lblUnpromotionPersonsMonthwork.Text = String.Format("{0:N0}", "0")
                        Me.txtUnpromotionPersons.Text = String.Format("{0:N0}", "0")
                    End If
                Else
                    CLMsg.Show("GE0178")
                    Me.lblUnpromotionPersonsMonthwork.Text = String.Format("{0:N0}", "0")
                    Me.txtUnpromotionPersons.Text = String.Format("{0:N0}", "0")
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

        Private Sub txtUnpromotionPersons_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles txtUnpromotionPersons.KeyPress
            Try
                Me.Cursor = Cursors.WaitCursor
                If (Not Me.txtUnpromotionPersons.ReadOnly AndAlso e.KeyChar.Equals("")) Then
                    Me.dtTable.AcceptChanges()
                    Me.dtTableUnionDues.AcceptChanges()
                    Me.SetNewReviseRevenueTtl()
                    Me.Caluculation()
                    Me.SetLabelText()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub

        Private Sub txtUnpromotionPersons_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtUnpromotionPersons.Leave
            Try
                Me.Cursor = Cursors.WaitCursor
                If Not Me.txtUnpromotionPersons.ReadOnly Then
                    Me.SetNewReviseRevenueTtl()
                    Me.Caluculation()
                    Me.SetLabelText()
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
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End Sub


        ' Fields
        Private WithEvents btnBack As Button
        Private WithEvents btnBaseChange As Button
        Private WithEvents btnCancel As Button
        Private WithEvents btnChange As Button
        Private WithEvents btnEntryCheck As Button
        Private WithEvents btnExpenditureRevenuePrint As Button
        Private WithEvents btnNewEntry As Button
        Private WithEvents btnPrinting As Button
        Private components As IContainer
        Private Const D_TARGET As String = "ëŒè€åé"
        Private dtTable As DataTable
        Private dtTableBack As DataTable
        Private dtTableCalcData As DataTable
        Private dtTableFlexGrid As DataTable
        Private dtTableUnionDues As DataTable
        Protected WithEvents flxStatisticalTable As C1FlexGrid
        Private GetEntryRigth As Boolean
        Private groupBox1 As GroupBox
        Private groupBox2 As GroupBox
        Private grpBaseInfo As GroupBox
        Protected grpStatisticalTable As GroupBox
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
        Private label21 As Label
        Private label22 As Label
        Private label23 As Label
        Private label24 As Label
        Private label25 As Label
        Private label27 As Label
        Private label3 As Label
        Private label30 As Label
        Private label31 As Label
        Private label4 As Label
        Private label5 As Label
        Private label6 As Label
        Private label7 As Label
        Private label8 As Label
        Private label9 As Label
        Private lblCapRetireMemberTotal As Label
        Private lblCapRetireMoneyTotal As Label
        Private lblCapRetireMonthwork As Label
        Private lblCapUpMoneyTotal As Label
        Private lblCapUpMonthwork As Label
        Private lblCapUpTotal As Label
        Private lblCopRetireMemberTotal As Label
        Private lblCopRetireMoneyTotal As Label
        Private lblCopRetireMonthwork As Label
        Private lblFeRetireMemberTotal As Label
        Private lblFeRetireMoneyTotal As Label
        Private lblFeRetireMonthwork As Label
        Private lblNewUnionEntryTotal As Label
        Private lblNewUnionMoneyTotal As Label
        Private lblNewUnionMonthwork As Label
        Private lblRevenueExpenditureTtl As Label
        Private lblReviseRevenuettl As Label
        Private lblSeniorMonthwork As Label
        Private lblSeniorRetireMemberTotal As Label
        Private lblSeniorRetireMoneyTotal As Label
        Private lblSeniorRetireMonthwork As Label
        Private lblSeniorTotal As Label
        Private lbltitle As Label
        Private lblUnpromotionPersonsMonthwork As Label
        Private lblUnpromotionTotal As Label
        Private PrintRight As Boolean
        Private Const RPT_CREW_PLAN As String = "Report.RevenueExpenditure.RptMemberPlan"
        Private Const RPT_CREW_PLAN_SUB As String = "Report.RevenueExpenditure.RptMemberPlanSub"
        Private Const S_CAP_RETIRE_MEMBER As String = "CAP" & "ëﬁêEé“êî"
        Private Const S_CAP_RETIRE_MEMBER_TOTAL As String = "s_cap_retire_member_total"
        Private Const S_CAP_RETIRE_MONTHWORK_TOTAL As String = "s_cap_retire_monthwork"
        Private Const S_CAP_RETIRE_UNION_DUES As String = "s_cap_retire_union_dues"
        Private Const S_COP_RETIRE_MEMBER As String = "COP" & "ëﬁêEé“êî"
        Private Const S_COP_RETIRE_MEMBER_TOTAL As String = "s_cop_retire_member_total"
        Private Const S_COP_RETIRE_MONTHWORK_TOTAL As String = "s_cop_retire_monthwork"
        Private Const S_COP_RETIRE_UNION_DUES As String = "s_cop_retire_union_dues"
        Private Const S_ENTRY_DUES As String = "s_entry_money"
        Private Const S_FE_RETIRE_MEMBER As String = "FE" & "ëﬁêEé“êî"
        Private Const S_FE_RETIRE_MEMBER_TOTAL As String = "s_fe_retire_member_total"
        Private Const S_FE_RETIRE_MONTHWORK_TOTAL As String = "s_fe_retire_monthwork"
        Private Const S_FE_RETIRE_UNION_DUES As String = "s_fe_retire_union_dues"
        Private Const S_NEW_CAP_MEMBER As String = "CAP" & "è∏äiêî"
        Private Const S_NEW_CAP_MEMBER_TOTAL As String = "s_new_cap_member_total"
        Private Const S_NEW_CAP_MONTHWORK_TOTAL As String = "s_cap_up_monthwork"
        Private Const S_NEW_CAP_UNION_DUES As String = "s_new_cap_union_dues"
        Private Const S_NEW_STAFF_MEMBER As String = "êVì¸ëgçáàıêî"
        Private Const S_NEW_STAFF_MEMBER_TOTAL As String = "s_new_staff_member_total"
        Private Const S_NEW_STAFF_MONEY As String = "s_new_staff_money"
        Private Const S_NEW_STAFF_MONTHWORK_TOTAL As String = "s_new_union_monthwork"
        Private Const S_NEW_STAFF_UNION_DUES As String = "s_new_staff_union_dues"
        Private Const S_RETIRE_MONTHWORK_TOTAL As String = "s_retire_monthwork_total"
        Private Const S_REVENUE_UNPROMOTION As String = "s_revenue_unpromotion"
        Private Const S_REVISE_CAP_RETIRE As String = "s_revise_cap_retire"
        Private Const S_REVISE_COP_RETIRE As String = "s_revise_cop_retire"
        Private Const S_REVISE_FE_RETIRE As String = "s_revise_fe_retire"
        Private Const S_REVISE_NEW_STAFF As String = "s_revise_new_staff"
        Private Const S_REVISE_RETIRE_MONTHWORK_TOTAL As String = "s_revise_retire_monthwork_total"
        Private Const S_REVISE_REVENUE_TTL As String = "s_revise_revenue_ttl"
        Private Const S_REVISE_SENIOR_RETIRE As String = "s_revise_senior_retire"
        Private Const S_REVISE_UP As String = "s_revise_up"
        Private Const S_SENIOR_MONTHWORK_TOTAL As String = "s_senior_monthwork_total"
        Private Const S_SENIOR_RETIRE_MEMBER As String = "ÉVÉjÉAëﬁêEé“êî"
        Private Const S_SENIOR_RETIRE_MEMBER_TOTAL As String = "s_senior_retire_member_total"
        Private Const S_SENIOR_RETIRE_MONTHWORK_TOTAL As String = "s_senior_retire_monthwork"
        Private Const S_SENIOR_RETIRE_UNION_DUES As String = "s_senior_retire_union_dues"
        Private Const S_SENIOR_UNION_DUES As String = "s_senior_union_dues"
        Private Const S_UNPROMOTION_PERSONS As String = "s_unpromotion_persons_monthwork"
        Private Const S_UNPROMOTION_TOTAL As String = "s_unpromotion_persons"
        Private Const S_UNPROMOTION_UNION_DUES As String = "s_unpromotion_union_dues"
        Private txtCapPromotionAverage As PersonalTextBox
        Private txtnewStaffAverage As PersonalTextBox
        Private txtRevenueEndDate As PersonalTextBox
        Private txtRevenueStrDate As PersonalTextBox
        Private txtSeniorAverage As PersonalTextBox
        Private txtSeniorStayRate As PersonalTextBox
        Private txtUnpromotionAverage As PersonalTextBox
        Private WithEvents txtUnpromotionPersons As PersonalTextBox
        Private txtUnpromotionRate As PersonalTextBox
        Private _parent As UC050401
    End Class
End Namespace
