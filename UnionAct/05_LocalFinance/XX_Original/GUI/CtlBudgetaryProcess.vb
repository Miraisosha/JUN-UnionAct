Imports C1.Win.C1FlexGrid
Imports C1.Win.C1FlexGrid.Util.BaseControls
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionException
Imports UnionAct.NSCLMsg
Imports UnionAct.GUI.Common
Imports UnionAct.Framework.Command
Imports UnionAct.GUI.UnionComponent
Imports UnionAct.Business.RevenueExpenditure
Imports UnionAct.NSMDInfo

Namespace GUI.RevenueExpenditure.UnionForm
    Public Class CtlBudgetaryProcess
        Inherits RevenueExpenditureBase
        ' Methods
        Private Sub New()
            MyBase.New()
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal _IsRevise As Boolean, ByVal _strRevenueStart As String, ByVal _strRevenueEnd As String, ByVal _strLastRevenueStart As String, ByVal _IsNewFlg As Boolean, ByVal _IsChangeFlg As Boolean, ByVal _strRevenueTitle As String, ByVal _iReviseRevenueTtl As Long, ByVal _iBudgetSub As Long, ByVal _iBudgetTotal As Long, ByVal _iReviseBudgetSub As Long, ByVal _iReviseBudgetTotal As Long, ByVal _dtDup As Object, ByVal _IsReferenceRight As Boolean, ByVal _IsGetEntryRight As Boolean, ByVal _IsPrintRight As Boolean, ByVal parent As UC050401)
            MyBase.New(_IsGetEntryRight, _IsPrintRight, _IsReferenceRight, _strRevenueTitle, CDate(_dtDup), _strRevenueStart, _strRevenueEnd, _strLastRevenueStart, _IsNewFlg, _IsChangeFlg)
            Me.InitializeComponent()
            Me._isEdit = _IsNewFlg
            Me._lngReviseRevenueTotal = _iReviseRevenueTtl
            Me._isReviese = _IsRevise
            Me._parent = parent
        End Sub

        Private Sub AddNewRowToGrid(ByVal flxTarget As C1FlexGrid)
            Try
                Dim dataSource As DataTable = DirectCast(flxTarget.DataSource, DataTable)
                dataSource.Rows.Add(dataSource.NewRow)
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

        Private Sub BtnAddRow_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim gridFromEventFirer As C1FlexGrid = Me.GetGridFromEventFirer(sender)
                Dim dataSource As DataTable = DirectCast(gridFromEventFirer.DataSource, DataTable)
                If MDFinanceCommon.CheckRowIsSelected(gridFromEventFirer) Then
                    dataSource.Rows.InsertAt(dataSource.NewRow, gridFromEventFirer.Row)
                    gridFromEventFirer.Select((gridFromEventFirer.Row + 1), gridFromEventFirer.Cols.Item("çÄñ⁄î‘çÜ").Index)
                Else
                    dataSource.Rows.Add(dataSource.NewRow)
                    gridFromEventFirer.Select((gridFromEventFirer.Rows.Count - 1), gridFromEventFirer.Cols.Item("çÄñ⁄î‘çÜ").Index)
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

        Private Sub btnAllotNumber_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim gridFromEventFirer As C1FlexGrid = Me.GetGridFromEventFirer(sender)
                If CLMsg.Show("GQ0051", "çÄñ⁄î‘çÜ").Equals(DialogResult.Yes) Then
                    Dim i As Integer
                    For i = 1 To gridFromEventFirer.Rows.Count - 1
                        gridFromEventFirer.Item(i, "çÄñ⁄î‘çÜ") = i.ToString
                    Next i
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

        Private Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Utilities.RestoreUserControl()
                _parent.ActionAfterResotreUserControl()
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

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If CLMsg.Show("GQ0007").Equals(DialogResult.Yes) Then
                    If MyBase._IsNewFlg Then
                        Utilities.RestoreUserControl()
                        _parent.ActionAfterResotreUserControl()
                    Else
                        Me._isEdit = False
                        Me.SetInitialData()
                        Me.ShowControlInEachMode()
                    End If
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

        Private Sub btnChange_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me._isEdit = True
                Me.ShowControlInEachMode()
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

        Private Sub btnDecideBudget_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If CLMsg.Show("GQ0052", DateTime.ParseExact(MyBase._RevenueStartDate, "yyyyMMdd", Nothing).ToString("yyyy/MM/dd"), DateTime.ParseExact(MyBase._RevenueEndDate, "yyyyMMdd", Nothing).ToString("yyyy/MM/dd")).Equals(DialogResult.Yes) Then
                    Dim command As New BudgetaryProcessCommand
                    command.UpdateRevenueSeton(MyBase._RevenueStartDate, MyBase._dtDup, MDLoginInfo.UserId)
                    CLMsg.Show("GI0036")
                    Utilities.RestoreUserControl()
                    _parent.ActionAfterResotreUserControl()
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

        Private Sub BtnDeleteRow_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim gridFromEventFirer As C1FlexGrid = Me.GetGridFromEventFirer(sender)
                If MDFinanceCommon.CheckRowIsSelected(gridFromEventFirer) Then
                    If (gridFromEventFirer.Equals(Me.flxIncomeBudgetaryProcess) AndAlso (gridFromEventFirer.Row = 1)) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0164", New String() {If(MDFinanceCommon.IsEmptyCell(gridFromEventFirer, 1, "çÄñ⁄ñº"), "ëgçáîÔ", gridFromEventFirer.Item(1, "çÄñ⁄ñº").ToString)})
                    End If
                    If CLMsg.Show("GQ0011").Equals(DialogResult.Yes) Then
                        DirectCast(gridFromEventFirer.DataSource, DataTable).Rows.RemoveAt(gridFromEventFirer.Rows.Item(gridFromEventFirer.Row).DataIndex)
                        Me.ShowSubTotal(If(sender.Equals(Me.btnDellIncome), Me.lblRevenueTotal, Me.lblExpend), Me.GetSubTotalPay(gridFromEventFirer))
                    End If
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

        Private Sub btnEntryConfirm_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim num As Integer
                Me.Cursor = Cursors.WaitCursor
                Me.CheckBeforeEntry()
                Dim printData As DataSet = Me.GetPrintData
                Dim viewer As New ReportViewer(printData, New CR0504P1, 1)
                Dim num2 As Integer = viewer.ConfirmViewerShow(num)
                viewer.RptDataDispose()
                If Not num2.Equals(2) Then
                    Dim entryData As DataSet = Me.GetEntryData
                    Dim command As New BudgetaryProcessCommand
                    command.Entry(entryData, MyBase.IsNewFlg, Me._isReviese, MyBase._dtDup)
                    If (num2 = 0) Then
                        viewer = New ReportViewer(printData, New CR0504P1, num)
                        viewer.PrintOut()
                        viewer.RptDataDispose()
                    End If
                    Utilities.RestoreUserControl()
                    _parent.ActionAfterResotreUserControl()
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                If exception.lstExMsgText.Item(0).MessageId.Equals("BE0005") Then
                    Utilities.RestoreUserControl()
                End If
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

        Private Sub btnPrinting_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim viewer As New ReportViewer(Me.GetPrintData, New CR0504P1, 3)
                viewer.ReportViewerShow()
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

        Private Sub btnToBottom_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me.MoveRow(Me.GetGridFromEventFirer(sender), False)
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

        Private Sub btnToTop_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me.MoveRow(Me.GetGridFromEventFirer(sender), True)
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

        Private Function CanMoveRow(ByVal flxTarget As C1FlexGrid, ByVal iMovedRowIndex As Integer) As Boolean
            Dim flag As Boolean
            Try
                If (flxTarget.Equals(Me.flxIncomeBudgetaryProcess) AndAlso ((flxTarget.Row = 1) OrElse (iMovedRowIndex = 0))) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0177", New String() {If(MDFinanceCommon.IsEmptyCell(flxTarget, 1, "çÄñ⁄ñº"), "ëgçáîÔ", flxTarget.Item(1, "çÄñ⁄ñº").ToString)})
                End If
                Dim dataSource As DataTable = DirectCast(flxTarget.DataSource, DataTable)
                flag = ((0 <= iMovedRowIndex) AndAlso (iMovedRowIndex < dataSource.Rows.Count))
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
            End Try
            Return flag
        End Function

        Private Sub CheckBeforeEntry()
            Try
                Dim appEx As AppUnionException = Nothing
                Me.CheckRequire(appEx, Me.flxIncomeBudgetaryProcess)
                Me.CheckRequire(appEx, Me.flxExpensesBudgetaryProcess)
                If (Not appEx Is Nothing) Then
                    Throw appEx
                End If
                Me.CheckPayByteForPrint()
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub CheckCell(ByRef appEx As AppUnionException, ByVal flxTarget As C1FlexGrid, ByVal iRowIndex As Integer, ByVal strColName As String)
            Try
                If MDFinanceCommon.IsEmptyCell(flxTarget, iRowIndex, strColName) Then
                    PublicCommand.AddAppUnionExceptionData(appEx, "GE0163", New String() {flxTarget.Parent.Text, iRowIndex.ToString, strColName})
                    flxTarget.SetCellStyle(iRowIndex, flxTarget.Cols.Item(strColName).Index, MDFinanceCommon.GetErrorStyle(flxTarget, True))
                Else
                    flxTarget.SetCellStyle(iRowIndex, flxTarget.Cols.Item(strColName).Index, flxTarget.Cols.Item(strColName).Style)
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

        Private Sub CheckCharByte(ByRef appEx As AppUnionException, ByVal flxTarget As C1FlexGrid)
            Try
                Dim i As Integer
                For i = 1 To flxTarget.Rows.Count - 1
                    Me.CheckCharByteForCell(appEx, flxTarget, i, "çÄñ⁄î‘çÜ", 6)
                    Me.CheckCharByteForCell(appEx, flxTarget, i, "çÄñ⁄ñº", &H26)
                    Me.CheckCharByteForCell(appEx, flxTarget, i, "îıçl", &H60)
                Next i
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub CheckCharByteForCell(ByRef appEx As AppUnionException, ByVal flxTarget As C1FlexGrid, ByVal iRowIndex As Integer, ByVal strColumnName As String, ByVal iMaxByte As Integer)
            Try
                If Not MDFinanceCommon.IsEmptyCell(flxTarget, iRowIndex, strColumnName) Then
                    Dim byteLength As Integer = PublicCommand.GetByteLength(flxTarget.Item(iRowIndex, strColumnName).ToString)
                    If (iMaxByte < byteLength) Then
                        Dim num2 As Integer = (iMaxByte / 2)
                        PublicCommand.AddAppUnionExceptionData(appEx, "GW0025", New String() {flxTarget.Parent.Text, iRowIndex.ToString, strColumnName, iMaxByte.ToString, num2.ToString})
                    End If
                End If
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub CheckPayByteForPrint()
            Try
                Dim appEx As AppUnionException = Nothing
                Me.CheckCharByte(appEx, Me.flxIncomeBudgetaryProcess)
                Me.CheckCharByte(appEx, Me.flxExpensesBudgetaryProcess)
                Me.CheckSubTotalPayByte(appEx, Me.lblRevenueTotal)
                Me.CheckSubTotalPayByte(appEx, Me.lblExpend)
                If (Not appEx Is Nothing) Then
                    Dim msg As New ExceptionMsg(appEx)
                    If Not msg.ShowMessage.Equals(DialogResult.Yes) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE9000", New String(0 - 1) {})
                    End If
                End If
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub CheckRequire(ByRef appEx As AppUnionException, ByVal flxTarget As C1FlexGrid)
            Try
                Dim i As Integer
                For i = 1 To flxTarget.Rows.Count - 1
                    Me.CheckCell(appEx, flxTarget, i, "çÄñ⁄ñº")
                    Me.CheckCell(appEx, flxTarget, i, "ã‡äz")
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

        Private Sub CheckSubTotalPayByte(ByRef appEx As AppUnionException, ByVal lblTarget As Label)
            Try
                Dim byteLength As Integer = PublicCommand.GetByteLength(lblTarget.Text)
                If (13 < byteLength) Then
                    Dim strErrorMessage As String() = New String() {(lblTarget.Parent.Text & "ÅEè¨åv"), 13.ToString}
                    PublicCommand.AddAppUnionExceptionData(appEx, "GW0022", strErrorMessage)
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

        Private Sub CtlBudgetaryProcess_Load(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me.SetHeaderData()
                Me._dSetAtLoad = (New BudgetaryProcessCommand).GetRevenueBudgetary(MyBase._RevenueStartDate, MyBase._LastRevenueStartDate, MyBase.IsNewFlg, Me._isReviese, Me._lngReviseRevenueTotal)
                Me.SetInitialData()
                If (((Not Convert.ToInt64(Me._dSetAtLoad.Tables.Item("Revenue").Rows.Item(0).Item("ã‡äz")).Equals(Me._lngReviseRevenueTotal) AndAlso Not MyBase._IsNewFlg) AndAlso (Not Me._isReviese AndAlso MyBase._IsChangeFlg)) AndAlso (MyBase._IsGetEntryRight AndAlso CLMsg.Show("GQ0053", Me.grpIncomeTable.Text, "ëgçáîÔ").Equals(DialogResult.Yes))) Then
                    Me._isEdit = True
                    Me.flxIncomeBudgetaryProcess.Item(1, "ã‡äz") = Me._lngReviseRevenueTotal
                    Me.ShowSubTotal(Me.lblRevenueTotal, Me.GetSubTotalPay(Me.flxIncomeBudgetaryProcess))
                End If
                Me.ShowControlInEachMode()
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

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Private Function GetEntryData() As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                ds.Tables.Add(Me.GetEntryHeaderTable)
                ds.Tables.Add(Me.GetEntryDetailTable)
                ds.Tables.Add(Me.GetEntryUpdateDataForReveneueExpenditure)
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

        Private Sub GetEntryDetailData(ByVal map As EntityMap, ByRef dTable As DataTable, ByVal flxTarget As C1FlexGrid, ByVal strBudgetaryProcess As String)
            Try
                Dim i As Integer
                For i = 1 To flxTarget.Rows.Count - 1
                    Dim row As DataRow = dTable.NewRow
                    row.Item(map.GetPhysicalName(0)) = MyBase._RevenueStartDate
                    row.Item(map.GetPhysicalName(1)) = If(Me._isReviese, "02", "01")
                    row.Item(map.GetPhysicalName(2)) = strBudgetaryProcess
                    row.Item(map.GetPhysicalName(3)) = (i - 1)
                    row.Item(map.GetPhysicalName(4)) = flxTarget.Item(i, "çÄñ⁄î‘çÜ")
                    row.Item(map.GetPhysicalName(5)) = flxTarget.Item(i, "çÄñ⁄ñº")
                    row.Item(map.GetPhysicalName(6)) = flxTarget.Item(i, "ã‡äz")
                    row.Item(map.GetPhysicalName(7)) = flxTarget.Item(i, "îıçl")
                    row.Item(map.GetPhysicalName(8)) = PublicCommand.GetNow
                    row.Item(map.GetPhysicalName(10)) = PublicCommand.GetNow
                    row.Item(map.GetPhysicalName(9)) = UserId
                    row.Item(map.GetPhysicalName(11)) = UserId
                    row.Item(map.GetPhysicalName(12)) = 0
                    dTable.Rows.Add(row)
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

        Private Function GetEntryDetailTable() As DataTable
            Dim table2 As DataTable
            Try
                Dim map As New RevenueBudgetaryProcessDtlMap
                Dim dTable As DataTable = map.CreateDataTablePhysName("revenue_budgetary_process_dtl")
                Me.GetEntryDetailData(map, dTable, Me.flxIncomeBudgetaryProcess, "01")
                Me.GetEntryDetailData(map, dTable, Me.flxExpensesBudgetaryProcess, "02")
                table2 = dTable
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Private Sub GetEntryHeaderData(ByVal map As EntityMap, ByRef dTable As DataTable, ByVal strBudgetaryProcess As String, ByVal strSubTotal As String)
            Try
                Dim row As DataRow = dTable.NewRow
                row.Item(map.GetPhysicalName(0)) = MyBase._RevenueStartDate
                row.Item(map.GetPhysicalName(1)) = If(Me._isReviese, "02", "01")
                row.Item(map.GetPhysicalName(2)) = strBudgetaryProcess
                row.Item(map.GetPhysicalName(3)) = Convert.ToInt64(strSubTotal.Replace(",", ""))
                row.Item(map.GetPhysicalName(5)) = PublicCommand.GetNow
                row.Item(map.GetPhysicalName(7)) = PublicCommand.GetNow
                row.Item(map.GetPhysicalName(6)) = MDLoginInfo.UserId
                row.Item(map.GetPhysicalName(8)) = MDLoginInfo.UserId
                row.Item(map.GetPhysicalName(9)) = 0
                dTable.Rows.Add(row)
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

        Private Function GetEntryHeaderTable() As DataTable
            Dim table2 As DataTable
            Try
                Dim map As New RevenueBudgetaryProcessMap
                Dim dTable As DataTable = map.CreateDataTablePhysName("revenue_budgetary_process")
                Me.GetEntryHeaderData(map, dTable, "01", Me.lblRevenueTotal.Text)
                Me.GetEntryHeaderData(map, dTable, "02", Me.lblExpend.Text)
                table2 = dTable
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Private Function GetEntryUpdateDataForReveneueExpenditure() As DataTable
            Dim table2 As DataTable
            Try
                Dim table As New DataTable("revenue_expenditure")
                table.Columns.Add("d_revenue_str", GetType(String))
                table.Columns.Add("s_budget_sub", GetType(Long))
                table.Columns.Add("s_budget_total", GetType(Long))
                table.Columns.Add("s_revise_budget_sub", GetType(Long))
                table.Columns.Add("s_revise_budget_total", GetType(Long))
                table.Columns.Add("d_up", GetType(DateTime))
                table.Columns.Add("c_user_id_up", GetType(String))
                Dim row As DataRow = table.NewRow
                row.Item("d_revenue_str") = MyBase._RevenueStartDate
                row.Item("s_budget_sub") = Convert.ToInt64(Me.lblBudgetSub.Text.Replace(",", ""))
                row.Item("s_revise_budget_sub") = Convert.ToInt64(Me.lblBudgetSub.Text.Replace(",", ""))
                row.Item("s_budget_total") = Convert.ToInt64(Me.lblBudgetTotal.Text.Replace(",", ""))
                row.Item("s_revise_budget_total") = Convert.ToInt64(Me.lblBudgetTotal.Text.Replace(",", ""))
                row.Item("d_up") = PublicCommand.GetNow
                row.Item("c_user_id_up") = MDLoginInfo.UserId
                table.Rows.Add(row)
                table2 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Private Function GetGridFromEventFirer(ByVal objSender As Object) As C1FlexGrid
            Dim grid As C1FlexGrid
            Try
                Dim control As Control
                For Each control In DirectCast(objSender, Control).Parent.Controls
                    If TypeOf control Is C1FlexGrid Then
                        Return DirectCast(control, C1FlexGrid)
                    End If
                Next
                grid = Nothing
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return grid
        End Function

        Private Function GetPrintData() As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet("dsBudgetary")
                ds.Tables.Add(Me.GetPrintHeaderTable)
                ds.Tables.Add(Me.GetPrintDetailTable)
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

        Private Sub GetPrintDetailData(ByRef dTableDtl As DataTable, ByVal flxTarget As C1FlexGrid, ByVal iGroupNumber As Integer, ByVal strBudgetaryProcess As String)
            Try
                Dim constant As New InfoConstant(MyBase._RevenueStartDate)
                Dim str As String = (constant.GetShortNameByCode("BUDGETARY_PROCESS", strBudgetaryProcess) & " - ")
                Dim i As Integer
                For i = 1 To flxTarget.Rows.Count - 1
                    Dim row As DataRow = dTableDtl.NewRow
                    row.Item("s_number") = iGroupNumber
                    row.Item("l_number") = If(MDFinanceCommon.IsEmptyCell(flxTarget, i, "çÄñ⁄î‘çÜ"), "", (str & flxTarget.Item(i, "çÄñ⁄î‘çÜ").ToString))
                    row.Item("l_name") = flxTarget.Item(i, "çÄñ⁄ñº")
                    row.Item("s_budgetary_money") = flxTarget.Item(i, "ã‡äz")
                    row.Item("l_biko") = flxTarget.Item(i, "îıçl")
                    dTableDtl.Rows.Add(row)
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

        Private Function GetPrintDetailTable() As DataTable
            Dim table2 As DataTable
            Try
                Dim dTableDtl As New DataTable("dtDetail")
                dTableDtl.Columns.Add("s_number", GetType(Integer))
                dTableDtl.Columns.Add("l_number", GetType(String))
                dTableDtl.Columns.Add("l_name", GetType(String))
                dTableDtl.Columns.Add("s_budgetary_money", GetType(Long))
                dTableDtl.Columns.Add("l_biko", GetType(String))
                Me.GetPrintDetailData(dTableDtl, Me.flxIncomeBudgetaryProcess, 0, "01")
                Me.GetPrintDetailData(dTableDtl, Me.flxExpensesBudgetaryProcess, 1, "02")
                table2 = dTableDtl
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Private Function GetPrintHeaderTable() As DataTable
            Dim table2 As DataTable
            Try
                Dim table As New DataTable("dtHeader")
                table.Columns.Add("l_period", GetType(String))
                table.Columns.Add("l_revise_day", GetType(String))
                Dim row As DataRow = table.NewRow
                row.Item("l_period") = MyBase._RevenueTitle
                If Me._isReviese Then
                    row.Item("l_revise_day") = ("ó\éZèCê≥ì˙Å@" & If(Me._isEdit, PublicCommand.GetNow.ToString("yyyy" & "îN" & "MM" & "åé" & "dd" & "ì˙"), Convert.ToDateTime(Me._dSetAtLoad.Tables.Item("Revenue").Rows.Item(0).Item("revise_day")).ToString("yyyy" & "îN" & "MM" & "åé" & "dd" & "ì˙")))
                End If
                table.Rows.Add(row)
                table2 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Private Function GetSubTotalPay(ByVal flxTarget As C1FlexGrid) As Long
            Dim num3 As Long
            Try
                Dim num As Long = 0
                Dim i As Integer
                For i = 1 To flxTarget.Rows.Count - 1
                    If Not MDFinanceCommon.IsEmptyCell(flxTarget, i, "ã‡äz") Then
                        num = (num + Convert.ToInt64(flxTarget.Item(i, "ã‡äz")))
                    End If
                Next i
                num3 = num
            Catch exception As OverflowException
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "GE0174", New String() {"çáåv"})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "GE0001", New String(0 - 1) {})
            End Try
            Return num3
        End Function

        Private Sub Grid_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim grid As C1FlexGrid = DirectCast(sender, C1FlexGrid)
                If (MDFinanceCommon.CheckMouseCursorPoint(grid) AndAlso Me._isEdit) Then
                    grid.StartEditing()
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

        Private Sub Grid_SetupEditor(ByVal sender As Object, ByVal e As RowColEventArgs)
            Try
                Dim grid As C1FlexGrid = DirectCast(sender, C1FlexGrid)
                If grid.Cols.Item(e.Col).Name.Equals("ã‡äz") Then
                    DirectCast(grid.Editor, TextBox).ImeMode = ImeMode.Disable
                    DirectCast(grid.Editor, TextBox).MaxLength = 11
                    DirectCast(grid.DataSource, DataTable).AcceptChanges()
                Else
                    Dim num As Integer = 0
                    If grid.Cols.Item(e.Col).Name.Equals("çÄñ⁄î‘çÜ") Then
                        num = 10
                        DirectCast(grid.Editor, TextBox).ImeMode = ImeMode.Disable
                    Else
                        num = 100
                        DirectCast(grid.Editor, TextBox).ImeMode = ImeMode.Hiragana
                    End If
                    DirectCast(grid.Editor, TextBox).MaxLength = num
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

        Private Sub Grid_StartEdit(ByVal sender As Object, ByVal e As RowColEventArgs)
            Try
                If Not Me._isEdit Then
                    e.Cancel = True
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

        Private Sub Grid_ValidateEdit(ByVal sender As Object, ByVal e As ValidateEditEventArgs)
            Try
                Dim flxTarget As C1FlexGrid = DirectCast(sender, C1FlexGrid)
                If flxTarget.Cols.Item(e.Col).Name.Equals("ã‡äz") Then
                    Dim num As Long
                    If ValidatorUtility.ValidateNumericValue(flxTarget.Editor.Text, num) Then
                        If ((flxTarget.Equals(Me.flxExpensesBudgetaryProcess) AndAlso (num < 0)) AndAlso CLMsg.Show("GW0023", flxTarget.Parent.Text).Equals(DialogResult.No)) Then
                            flxTarget.FinishEditing(True)
                            Return
                        End If
                        flxTarget.Item(e.Row, e.Col) = num
                    ElseIf String.IsNullOrEmpty(flxTarget.Editor.Text) Then
                        flxTarget.Item(e.Row, e.Col) = DBNull.Value
                    Else
                        flxTarget.FinishEditing(True)
                    End If
                    Dim lblTarget As Label = If(flxTarget.Equals(Me.flxIncomeBudgetaryProcess), Me.lblRevenueTotal, Me.lblExpend)
                    Me.ShowSubTotal(lblTarget, Me.GetSubTotalPay(flxTarget))
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                Dim grid2 As C1FlexGrid = DirectCast(sender, C1FlexGrid)
                If grid2.Cols.Item(e.Col).Name.Equals("ã‡äz") Then
                    DirectCast(DirectCast(sender, C1FlexGrid).DataSource, DataTable).RejectChanges()
                    grid2.FinishEditing(True)
                End If
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub InitializeComponent()
            'TODO Dim manager As New ComponentResourceManager(GetType(CtlBudgetaryProcess))
            Me.grpExpenditure = New GroupBox
            Me.lbltitle = New Label
            Me.txtRevenueStrDate = New PersonalTextBox
            Me.txtRevenueEndDate = New PersonalTextBox
            Me.label1 = New Label
            Me.label2 = New Label
            Me.grpIncomeTable = New GroupBox
            Me.btnToBottomIncome = New Button
            Me.btnToTopIncome = New Button
            Me.btnAllotNumberIncome = New Button
            Me.label14 = New Label
            Me.lblRevenueTotal = New Label
            Me.btnAddIncome = New Button
            Me.btnDellIncome = New Button
            Me.flxIncomeBudgetaryProcess = New C1FlexGrid
            Me.btnPrinting = New Button
            Me.btnChange = New Button
            Me.btnCancel = New Button
            Me.grpExpensesTable = New GroupBox
            Me.btnToBottomExpenses = New Button
            Me.btnToTopExpenses = New Button
            Me.btnAllotNumberExpenses = New Button
            Me.label3 = New Label
            Me.lblExpend = New Label
            Me.btnAddExpenses = New Button
            Me.btnDellExpenses = New Button
            Me.flxExpensesBudgetaryProcess = New C1FlexGrid
            Me.label5 = New Label
            Me.lblBudgetTotal = New Label
            Me.label7 = New Label
            Me.lblBudgetSub = New Label
            Me.btnEntryConfirm = New Button
            Me.btnBack = New Button
            Me.btnDecideBudget = New Button
            Me.grpExpenditure.SuspendLayout()
            Me.grpIncomeTable.SuspendLayout()
            Me.flxIncomeBudgetaryProcess.BeginInit()
            Me.grpExpensesTable.SuspendLayout()
            Me.flxExpensesBudgetaryProcess.BeginInit()
            MyBase.SuspendLayout()
            Me.grpExpenditure.Controls.Add(Me.lbltitle)
            Me.grpExpenditure.Controls.Add(Me.txtRevenueStrDate)
            Me.grpExpenditure.Controls.Add(Me.txtRevenueEndDate)
            Me.grpExpenditure.Controls.Add(Me.label1)
            Me.grpExpenditure.Controls.Add(Me.label2)
            Me.grpExpenditure.Location = New Point(&HA7, 3)
            Me.grpExpenditure.Name = "grpExpenditure"
            Me.grpExpenditure.Size = New Size(670, &H3F)
            Me.grpExpenditure.TabIndex = 0
            Me.grpExpenditure.TabStop = False
            Me.lbltitle.AutoSize = True
            Me.lbltitle.Location = New Point(&H36, &H19)
            Me.lbltitle.Name = "lbltitle"
            Me.lbltitle.Size = New Size(&H5E, &H10)
            Me.lbltitle.TabIndex = &H83
            Me.lbltitle.Text = "ëÊÇTÇRä˙ó\ëz"
            Me.txtRevenueStrDate.BackColor = Color.LightYellow
            Me.txtRevenueStrDate.FieldAttribute = EFieldAttribute.NONE
            Me.txtRevenueStrDate.Location = New Point(&H11D, &H16)
            Me.txtRevenueStrDate.Name = "txtRevenueStrDate"
            Me.txtRevenueStrDate.ReadOnly = True
            Me.txtRevenueStrDate.Require = False
            Me.txtRevenueStrDate.Size = New Size(&H95, &H17)
            Me.txtRevenueStrDate.TabIndex = 130
            Me.txtRevenueStrDate.TabStop = False
            Me.txtRevenueStrDate.TextAlign = HorizontalAlignment.Center
            Me.txtRevenueEndDate.BackColor = Color.LightYellow
            Me.txtRevenueEndDate.FieldAttribute = EFieldAttribute.NONE
            Me.txtRevenueEndDate.Location = New Point(470, &H16)
            Me.txtRevenueEndDate.Name = "txtRevenueEndDate"
            Me.txtRevenueEndDate.ReadOnly = True
            Me.txtRevenueEndDate.Require = False
            Me.txtRevenueEndDate.Size = New Size(&H95, &H17)
            Me.txtRevenueEndDate.TabIndex = &H7D
            Me.txtRevenueEndDate.TabStop = False
            Me.txtRevenueEndDate.TextAlign = HorizontalAlignment.Center
            Me.label1.AutoSize = True
            Me.label1.Location = New Point(440, &H19)
            Me.label1.Name = "label1"
            Me.label1.Size = New Size(&H18, &H10)
            Me.label1.TabIndex = &H7B
            Me.label1.Text = "Å`"
            Me.label2.AutoSize = True
            Me.label2.Location = New Point(&HCF, &H19)
            Me.label2.Name = "label2"
            Me.label2.Size = New Size(&H48, &H10)
            Me.label2.TabIndex = &H77
            Me.label2.Text = "ó\ëzä˙ä‘"
            Me.grpIncomeTable.Controls.Add(Me.btnToBottomIncome)
            Me.grpIncomeTable.Controls.Add(Me.btnToTopIncome)
            Me.grpIncomeTable.Controls.Add(Me.btnAllotNumberIncome)
            Me.grpIncomeTable.Controls.Add(Me.label14)
            Me.grpIncomeTable.Controls.Add(Me.lblRevenueTotal)
            Me.grpIncomeTable.Controls.Add(Me.btnAddIncome)
            Me.grpIncomeTable.Controls.Add(Me.btnDellIncome)
            Me.grpIncomeTable.Controls.Add(Me.flxIncomeBudgetaryProcess)
            Me.grpIncomeTable.Location = New Point(&H16, &H48)
            Me.grpIncomeTable.Name = "grpIncomeTable"
            Me.grpIncomeTable.Size = New Size(&H3D7, &HD6)
            Me.grpIncomeTable.TabIndex = 1
            Me.grpIncomeTable.TabStop = False
            Me.grpIncomeTable.Text = "é˚ì¸ÇÃïîèÓïÒ"
            Me.btnToBottomIncome.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Bold, GraphicsUnit.Point, &H80)
            Me.btnToBottomIncome.Location = New Point(&H3B2, &H65)
            Me.btnToBottomIncome.Name = "btnToBottomIncome"
            Me.btnToBottomIncome.Size = New Size(&H1B, &H23)
            Me.btnToBottomIncome.TabIndex = 5
            Me.btnToBottomIncome.Text = "Å´"
            Me.btnToBottomIncome.UseVisualStyleBackColor = True
            AddHandler Me.btnToBottomIncome.Click, New EventHandler(AddressOf Me.btnToBottom_Click)
            Me.btnToTopIncome.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Bold, GraphicsUnit.Point, &H80)
            Me.btnToTopIncome.Location = New Point(&H3B2, &H30)
            Me.btnToTopIncome.Name = "btnToTopIncome"
            Me.btnToTopIncome.Size = New Size(&H1B, &H23)
            Me.btnToTopIncome.TabIndex = 4
            Me.btnToTopIncome.Text = "Å™"
            Me.btnToTopIncome.UseVisualStyleBackColor = True
            AddHandler Me.btnToTopIncome.Click, New EventHandler(AddressOf Me.btnToTop_Click)
            Me.btnAllotNumberIncome.Location = New Point(&H1C, &HAC)
            Me.btnAllotNumberIncome.Name = "btnAllotNumberIncome"
            Me.btnAllotNumberIncome.Size = New Size(&H74, &H20)
            Me.btnAllotNumberIncome.TabIndex = 1
            Me.btnAllotNumberIncome.Text = "é©ìÆçÃî‘"
            Me.btnAllotNumberIncome.UseVisualStyleBackColor = True
            AddHandler Me.btnAllotNumberIncome.Click, New EventHandler(AddressOf Me.btnAllotNumber_Click)
            Me.label14.AutoSize = True
            Me.label14.Location = New Point(&H2F3, 180)
            Me.label14.Name = "label14"
            Me.label14.Size = New Size(40, &H10)
            Me.label14.TabIndex = &H98
            Me.label14.Text = "çáåv"
            Me.lblRevenueTotal.BorderStyle = BorderStyle.Fixed3D
            Me.lblRevenueTotal.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblRevenueTotal.Location = New Point(&H31C, &HB2)
            Me.lblRevenueTotal.Name = "lblRevenueTotal"
            Me.lblRevenueTotal.Size = New Size(&H81, &H15)
            Me.lblRevenueTotal.TabIndex = &H97
            Me.lblRevenueTotal.Text = "0"
            Me.lblRevenueTotal.TextAlign = ContentAlignment.MiddleRight
            AddHandler Me.lblRevenueTotal.TextChanged, New EventHandler(AddressOf Me.LblSubTotal_TextChanged)
            Me.btnAddIncome.Location = New Point(&H165, &HAC)
            Me.btnAddIncome.Name = "btnAddIncome"
            Me.btnAddIncome.Size = New Size(&H74, &H20)
            Me.btnAddIncome.TabIndex = 2
            Me.btnAddIncome.Text = "çsí«â¡"
            Me.btnAddIncome.UseVisualStyleBackColor = True
            AddHandler Me.btnAddIncome.Click, New EventHandler(AddressOf Me.BtnAddRow_Click)
            Me.btnDellIncome.Location = New Point(&H1E7, &HAC)
            Me.btnDellIncome.Name = "btnDellIncome"
            Me.btnDellIncome.Size = New Size(&H74, &H20)
            Me.btnDellIncome.TabIndex = 3
            Me.btnDellIncome.Text = "çsçÌèú"
            Me.btnDellIncome.UseVisualStyleBackColor = True
            AddHandler Me.btnDellIncome.Click, New EventHandler(AddressOf Me.BtnDeleteRow_Click)
            Me.flxIncomeBudgetaryProcess.AllowDragging = AllowDraggingEnum.None
            Me.flxIncomeBudgetaryProcess.AllowEditing = False
            Me.flxIncomeBudgetaryProcess.AllowSorting = AllowSortingEnum.None
            Me.flxIncomeBudgetaryProcess.AutoResize = False
            Me.flxIncomeBudgetaryProcess.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
            'TODO Me.flxIncomeBudgetaryProcess.ColumnInfo = manager.GetString("flxIncomeBudgetaryProcess.ColumnInfo")
            Me.flxIncomeBudgetaryProcess.FocusRect = FocusRectEnum.None
            Me.flxIncomeBudgetaryProcess.KeyActionTab = KeyActionEnum.MoveAcrossOut
            Me.flxIncomeBudgetaryProcess.Location = New Point(&H1C, &H16)
            Me.flxIncomeBudgetaryProcess.Name = "flxIncomeBudgetaryProcess"
            Me.flxIncomeBudgetaryProcess.Rows.Count = 8
            Me.flxIncomeBudgetaryProcess.Rows.DefaultSize = 20
            Me.flxIncomeBudgetaryProcess.SelectionMode = SelectionModeEnum.Row
            Me.flxIncomeBudgetaryProcess.Size = New Size(&H38D, &H90)
            'TODO Me.flxIncomeBudgetaryProcess.StyleInfo = manager.GetString("flxIncomeBudgetaryProcess.StyleInfo")
            Me.flxIncomeBudgetaryProcess.TabIndex = 0
            AddHandler Me.flxIncomeBudgetaryProcess.StartEdit, New RowColEventHandler(AddressOf Me.Grid_StartEdit)
            AddHandler Me.flxIncomeBudgetaryProcess.ValidateEdit, New ValidateEditEventHandler(AddressOf Me.Grid_ValidateEdit)
            AddHandler Me.flxIncomeBudgetaryProcess.SetupEditor, New RowColEventHandler(AddressOf Me.Grid_SetupEditor)
            AddHandler Me.flxIncomeBudgetaryProcess.Click, New EventHandler(AddressOf Me.Grid_Click)
            Me.btnPrinting.Location = New Point(&H26, &H2CB)
            Me.btnPrinting.Name = "btnPrinting"
            Me.btnPrinting.Size = New Size(&H74, &H20)
            Me.btnPrinting.TabIndex = 3
            Me.btnPrinting.Text = "ÉvÉåàÛç¸"
            Me.btnPrinting.UseVisualStyleBackColor = True
            AddHandler Me.btnPrinting.Click, New EventHandler(AddressOf Me.btnPrinting_Click)
            Me.btnChange.Location = New Point(&H2C3, &H2CB)
            Me.btnChange.Name = "btnChange"
            Me.btnChange.Size = New Size(&H74, &H20)
            Me.btnChange.TabIndex = 5
            Me.btnChange.Text = "ì‡óeïœçX"
            Me.btnChange.UseVisualStyleBackColor = True
            AddHandler Me.btnChange.Click, New EventHandler(AddressOf Me.btnChange_Click)
            Me.btnCancel.Location = New Point(&H345, &H2CB)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(&H74, &H20)
            Me.btnCancel.TabIndex = 6
            Me.btnCancel.Text = "ÉLÉÉÉìÉZÉã"
            Me.btnCancel.UseVisualStyleBackColor = True
            AddHandler Me.btnCancel.Click, New EventHandler(AddressOf Me.btnCancel_Click)
            Me.grpExpensesTable.Controls.Add(Me.btnToBottomExpenses)
            Me.grpExpensesTable.Controls.Add(Me.btnToTopExpenses)
            Me.grpExpensesTable.Controls.Add(Me.btnAllotNumberExpenses)
            Me.grpExpensesTable.Controls.Add(Me.label3)
            Me.grpExpensesTable.Controls.Add(Me.lblExpend)
            Me.grpExpensesTable.Controls.Add(Me.btnAddExpenses)
            Me.grpExpensesTable.Controls.Add(Me.btnDellExpenses)
            Me.grpExpensesTable.Controls.Add(Me.flxExpensesBudgetaryProcess)
            Me.grpExpensesTable.Location = New Point(&H16, &H124)
            Me.grpExpensesTable.Name = "grpExpensesTable"
            Me.grpExpensesTable.Size = New Size(&H3D7, &H16C)
            Me.grpExpensesTable.TabIndex = 2
            Me.grpExpensesTable.TabStop = False
            Me.grpExpensesTable.Text = "éxèoÇÃïîèÓïÒ"
            Me.btnToBottomExpenses.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Bold, GraphicsUnit.Point, &H80)
            Me.btnToBottomExpenses.Location = New Point(&H3B2, &HA7)
            Me.btnToBottomExpenses.Name = "btnToBottomExpenses"
            Me.btnToBottomExpenses.Size = New Size(&H1B, &H23)
            Me.btnToBottomExpenses.TabIndex = 5
            Me.btnToBottomExpenses.Text = "Å´"
            Me.btnToBottomExpenses.UseVisualStyleBackColor = True
            AddHandler Me.btnToBottomExpenses.Click, New EventHandler(AddressOf Me.btnToBottom_Click)
            Me.btnToTopExpenses.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Bold, GraphicsUnit.Point, &H80)
            Me.btnToTopExpenses.Location = New Point(&H3B2, &H72)
            Me.btnToTopExpenses.Name = "btnToTopExpenses"
            Me.btnToTopExpenses.Size = New Size(&H1B, &H23)
            Me.btnToTopExpenses.TabIndex = 4
            Me.btnToTopExpenses.Text = "Å™"
            Me.btnToTopExpenses.UseVisualStyleBackColor = True
            AddHandler Me.btnToTopExpenses.Click, New EventHandler(AddressOf Me.btnToTop_Click)
            Me.btnAllotNumberExpenses.Location = New Point(&H1C, 320)
            Me.btnAllotNumberExpenses.Name = "btnAllotNumberExpenses"
            Me.btnAllotNumberExpenses.Size = New Size(&H74, &H20)
            Me.btnAllotNumberExpenses.TabIndex = 1
            Me.btnAllotNumberExpenses.Text = "é©ìÆçÃî‘"
            Me.btnAllotNumberExpenses.UseVisualStyleBackColor = True
            AddHandler Me.btnAllotNumberExpenses.Click, New EventHandler(AddressOf Me.btnAllotNumber_Click)
            Me.label3.AutoSize = True
            Me.label3.Location = New Point(&H2F3, 330)
            Me.label3.Name = "label3"
            Me.label3.Size = New Size(40, &H10)
            Me.label3.TabIndex = &H98
            Me.label3.Text = "çáåv"
            Me.lblExpend.BorderStyle = BorderStyle.Fixed3D
            Me.lblExpend.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblExpend.Location = New Point(&H322, &H148)
            Me.lblExpend.Name = "lblExpend"
            Me.lblExpend.Size = New Size(&H81, &H15)
            Me.lblExpend.TabIndex = &H97
            Me.lblExpend.Text = "0"
            Me.lblExpend.TextAlign = ContentAlignment.MiddleRight
            AddHandler Me.lblExpend.TextChanged, New EventHandler(AddressOf Me.LblSubTotal_TextChanged)
            Me.btnAddExpenses.Location = New Point(&H165, 320)
            Me.btnAddExpenses.Name = "btnAddExpenses"
            Me.btnAddExpenses.Size = New Size(&H74, &H20)
            Me.btnAddExpenses.TabIndex = 2
            Me.btnAddExpenses.Text = "çsí«â¡"
            Me.btnAddExpenses.UseVisualStyleBackColor = True
            AddHandler Me.btnAddExpenses.Click, New EventHandler(AddressOf Me.BtnAddRow_Click)
            Me.btnDellExpenses.Location = New Point(&H1E7, 320)
            Me.btnDellExpenses.Name = "btnDellExpenses"
            Me.btnDellExpenses.Size = New Size(&H74, &H20)
            Me.btnDellExpenses.TabIndex = 3
            Me.btnDellExpenses.Text = "çsçÌèú"
            Me.btnDellExpenses.UseVisualStyleBackColor = True
            AddHandler Me.btnDellExpenses.Click, New EventHandler(AddressOf Me.BtnDeleteRow_Click)
            Me.flxExpensesBudgetaryProcess.AllowDragging = AllowDraggingEnum.None
            Me.flxExpensesBudgetaryProcess.AllowEditing = False
            Me.flxExpensesBudgetaryProcess.AllowSorting = AllowSortingEnum.None
            Me.flxExpensesBudgetaryProcess.AutoResize = False
            Me.flxExpensesBudgetaryProcess.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
            'TODO Me.flxExpensesBudgetaryProcess.ColumnInfo = manager.GetString("flxExpensesBudgetaryProcess.ColumnInfo")
            Me.flxExpensesBudgetaryProcess.FocusRect = FocusRectEnum.None
            Me.flxExpensesBudgetaryProcess.KeyActionTab = KeyActionEnum.MoveAcrossOut
            Me.flxExpensesBudgetaryProcess.Location = New Point(&H1C, &H16)
            Me.flxExpensesBudgetaryProcess.Name = "flxExpensesBudgetaryProcess"
            Me.flxExpensesBudgetaryProcess.Rows.Count = 14
            Me.flxExpensesBudgetaryProcess.Rows.DefaultSize = 20
            Me.flxExpensesBudgetaryProcess.SelectionMode = SelectionModeEnum.Row
            Me.flxExpensesBudgetaryProcess.Size = New Size(&H38D, &H124)
            'TODO Me.flxExpensesBudgetaryProcess.StyleInfo = manager.GetString("flxExpensesBudgetaryProcess.StyleInfo")
            Me.flxExpensesBudgetaryProcess.TabIndex = 0
            AddHandler Me.flxExpensesBudgetaryProcess.StartEdit, New RowColEventHandler(AddressOf Me.Grid_StartEdit)
            AddHandler Me.flxExpensesBudgetaryProcess.ValidateEdit, New ValidateEditEventHandler(AddressOf Me.Grid_ValidateEdit)
            AddHandler Me.flxExpensesBudgetaryProcess.SetupEditor, New RowColEventHandler(AddressOf Me.Grid_SetupEditor)
            AddHandler Me.flxExpensesBudgetaryProcess.Click, New EventHandler(AddressOf Me.Grid_Click)
            Me.label5.AutoSize = True
            Me.label5.Location = New Point(&H2F1, &H2A3)
            Me.label5.Name = "label5"
            Me.label5.Size = New Size(&H48, &H10)
            Me.label5.TabIndex = &H9B
            Me.label5.Text = "ó\éZçáåv"
            Me.lblBudgetTotal.BorderStyle = BorderStyle.Fixed3D
            Me.lblBudgetTotal.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblBudgetTotal.Location = New Point(830, &H2A1)
            Me.lblBudgetTotal.Name = "lblBudgetTotal"
            Me.lblBudgetTotal.Size = New Size(&H81, &H15)
            Me.lblBudgetTotal.TabIndex = &H9A
            Me.lblBudgetTotal.Text = "0"
            Me.lblBudgetTotal.TextAlign = ContentAlignment.MiddleRight
            Me.label7.AutoSize = True
            Me.label7.Location = New Point(550, &H2A3)
            Me.label7.Name = "label7"
            Me.label7.Size = New Size(&H38, &H10)
            Me.label7.TabIndex = &H9D
            Me.label7.Text = "ó\îıîÔ"
            Me.lblBudgetSub.BorderStyle = BorderStyle.Fixed3D
            Me.lblBudgetSub.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblBudgetSub.Location = New Point(&H264, &H2A1)
            Me.lblBudgetSub.Name = "lblBudgetSub"
            Me.lblBudgetSub.Size = New Size(&H81, &H15)
            Me.lblBudgetSub.TabIndex = &HA3
            Me.lblBudgetSub.Text = "0"
            Me.lblBudgetSub.TextAlign = ContentAlignment.MiddleRight
            Me.btnEntryConfirm.Location = New Point(&H2C3, &H2CB)
            Me.btnEntryConfirm.Name = "btnEntryConfirm"
            Me.btnEntryConfirm.Size = New Size(&H74, &H20)
            Me.btnEntryConfirm.TabIndex = 5
            Me.btnEntryConfirm.Text = "ìoò^ämîF"
            Me.btnEntryConfirm.UseVisualStyleBackColor = True
            AddHandler Me.btnEntryConfirm.Click, New EventHandler(AddressOf Me.btnEntryConfirm_Click)
            Me.btnBack.Location = New Point(&H345, &H2CB)
            Me.btnBack.Name = "btnBack"
            Me.btnBack.Size = New Size(&H74, &H20)
            Me.btnBack.TabIndex = 6
            Me.btnBack.Text = "ñﬂÇÈ"
            Me.btnBack.UseVisualStyleBackColor = True
            AddHandler Me.btnBack.Click, New EventHandler(AddressOf Me.btnBack_Click)
            Me.btnDecideBudget.Location = New Point(&HA7, &H2CB)
            Me.btnDecideBudget.Name = "btnDecideBudget"
            Me.btnDecideBudget.Size = New Size(&H74, &H20)
            Me.btnDecideBudget.TabIndex = 4
            Me.btnDecideBudget.Text = "ó\éZämíË"
            Me.btnDecideBudget.UseVisualStyleBackColor = True
            AddHandler Me.btnDecideBudget.Click, New EventHandler(AddressOf Me.btnDecideBudget_Click)
            MyBase.Controls.Add(Me.btnDecideBudget)
            MyBase.Controls.Add(Me.lblBudgetSub)
            MyBase.Controls.Add(Me.label7)
            MyBase.Controls.Add(Me.lblBudgetTotal)
            MyBase.Controls.Add(Me.grpExpensesTable)
            MyBase.Controls.Add(Me.btnPrinting)
            MyBase.Controls.Add(Me.grpIncomeTable)
            MyBase.Controls.Add(Me.grpExpenditure)
            MyBase.Controls.Add(Me.label5)
            MyBase.Controls.Add(Me.btnEntryConfirm)
            MyBase.Controls.Add(Me.btnCancel)
            MyBase.Controls.Add(Me.btnChange)
            MyBase.Controls.Add(Me.btnBack)
            Me.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            MyBase.Name = "CtlBudgetaryProcess"
            MyBase.Size = New Size(&H402, &H2F7)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.CtlBudgetaryProcess_Load)
            Me.grpExpenditure.ResumeLayout(False)
            Me.grpExpenditure.PerformLayout()
            Me.grpIncomeTable.ResumeLayout(False)
            Me.grpIncomeTable.PerformLayout()
            Me.flxIncomeBudgetaryProcess.EndInit()
            Me.grpExpensesTable.ResumeLayout(False)
            Me.grpExpensesTable.PerformLayout()
            Me.flxExpensesBudgetaryProcess.EndInit()
            MyBase.ResumeLayout(False)
            MyBase.PerformLayout()
        End Sub

        Private Sub LblSubTotal_TextChanged(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If sender.Equals(Me.lblRevenueTotal) Then
                    Me.lblBudgetTotal.Text = Me.lblRevenueTotal.Text
                End If
                Me.ShowBudgetSubLabel()
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

        Private Sub MoveRow(ByVal flxTarget As C1FlexGrid, ByVal toTop As Boolean)
            Try
                If MDFinanceCommon.CheckRowIsSelected(flxTarget) Then
                    Dim dataSource As DataTable = DirectCast(flxTarget.DataSource, DataTable)
                    Dim dataIndex As Integer = flxTarget.Rows.Item(flxTarget.Row).DataIndex
                    Dim iMovedRowIndex As Integer = (dataIndex + If(toTop, -1, 1))
                    If Me.CanMoveRow(flxTarget, iMovedRowIndex) Then
                        Dim itemArray As Object() = dataSource.Rows.Item(dataIndex).ItemArray
                        dataSource.Rows.RemoveAt(dataIndex)
                        Dim row As DataRow = dataSource.NewRow
                        row.ItemArray = itemArray
                        dataSource.Rows.InsertAt(row, iMovedRowIndex)
                        flxTarget.Select((iMovedRowIndex + 1), flxTarget.Col)
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

        Private Sub SetGridOutline(ByVal flxTarget As C1FlexGrid)
            Try
                Dim dicColWidthPair As New Dictionary(Of String, Integer)
                dicColWidthPair.Add("çÄñ⁄î‘çÜ", 100)
                dicColWidthPair.Add("çÄñ⁄ñº", 250)
                dicColWidthPair.Add("ã‡äz", 120)
                dicColWidthPair.Add("îıçl", &H19F)
                MDFinanceCommon.SetColsWidth(flxTarget, dicColWidthPair)
                MDFinanceCommon.AdjustTextAlign(flxTarget)
                flxTarget.Cols.Item("ã‡äz").Format = "N0"
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

        Private Sub SetHeaderData()
            Try
                Me.txtRevenueStrDate.Text = DateTime.ParseExact(MyBase._RevenueStartDate, "yyyyMMdd", Nothing).ToString("yyyy" & "îN" & "MM" & "åé" & "dd" & "ì˙")
                Me.txtRevenueEndDate.Text = DateTime.ParseExact(MyBase._RevenueEndDate, "yyyyMMdd", Nothing).ToString("yyyy" & "îN" & "MM" & "åé" & "dd" & "ì˙")
                Me.lbltitle.Text = MyBase._RevenueTitle
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

        Private Sub SetInitialData()
            Try
                Me.flxIncomeBudgetaryProcess.DataSource = Me._dSetAtLoad.Tables.Item("Revenue").Copy
                Me.flxExpensesBudgetaryProcess.DataSource = Me._dSetAtLoad.Tables.Item("Expend").Copy
                Me.SetGridOutline(Me.flxIncomeBudgetaryProcess)
                Me.SetGridOutline(Me.flxExpensesBudgetaryProcess)
                Me.ShowSubTotal(Me.lblRevenueTotal, Me.GetSubTotalPay(Me.flxIncomeBudgetaryProcess))
                Me.ShowSubTotal(Me.lblExpend, Me.GetSubTotalPay(Me.flxExpensesBudgetaryProcess))
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

        Private Sub ShowBudgetSubLabel()
            Try
                Dim num As Long
                Dim num2 As Long
                If (Long.TryParse(Me.lblRevenueTotal.Text.Replace(",", ""), num) AndAlso Long.TryParse(Me.lblExpend.Text.Replace(",", ""), num2)) Then
                    Me.lblBudgetSub.Text = String.Format("{0:N0}", (num - num2))
                Else
                    Me.lblBudgetSub.Text = ""
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

        Private Sub ShowControlInEachMode()
            Try
                Utilities.SetVisibleProperty(Me._isEdit, New Control() {Me.btnEntryConfirm, Me.btnCancel})
                Utilities.SetVisibleProperty(Not Me._isEdit, New Control() {Me.btnBack})
                Utilities.SetVisibleProperty(((Not Me._isEdit AndAlso MyBase.IsChangeFlg) AndAlso MyBase.IsGetEntryRight), New Control() {Me.btnChange})
                Utilities.SetVisibleProperty((((Not Me._isEdit AndAlso MyBase.IsChangeFlg) AndAlso MyBase.IsGetEntryRight) AndAlso Not Me._isReviese), New Control() {Me.btnDecideBudget})
                Utilities.SetVisibleProperty((Not Me._isEdit AndAlso MyBase.IsPrintRight), New Control() {Me.btnPrinting})
                Me.flxIncomeBudgetaryProcess.AllowEditing = Me.flxExpensesBudgetaryProcess.AllowEditing = Me._isEdit
                Utilities.SetEnabledProperty(Me._isEdit, New Control() {Me.btnAddExpenses, Me.btnAddIncome, Me.btnDellExpenses, Me.btnDellIncome, Me.btnAllotNumberIncome, Me.btnAllotNumberExpenses, Me.btnToTopExpenses, Me.btnToTopIncome, Me.btnToBottomIncome, Me.btnToBottomExpenses})
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

        Private Sub ShowSubTotal(ByVal lblTarget As Label, ByVal lngSubTotal As Long)
            Try
                lblTarget.Text = String.Format("{0:N0}", lngSubTotal)
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
        Private _dSetAtLoad As DataSet
        Private _isEdit As Boolean
        Private _isReviese As Boolean
        Private _lngReviseRevenueTotal As Long
        Private btnAddExpenses As Button
        Private btnAddIncome As Button
        Private btnAllotNumberExpenses As Button
        Private btnAllotNumberIncome As Button
        Private btnBack As Button
        Private btnCancel As Button
        Private btnChange As Button
        Private btnDecideBudget As Button
        Private btnDellExpenses As Button
        Private btnDellIncome As Button
        Private btnEntryConfirm As Button
        Private btnPrinting As Button
        Private btnToBottomExpenses As Button
        Private btnToBottomIncome As Button
        Private btnToTopExpenses As Button
        Private btnToTopIncome As Button
        Private Const COL_BIKO As String = "îıçl"
        Private Const COL_NAME As String = "çÄñ⁄ñº"
        Private Const COL_NUMBER As String = "çÄñ⁄î‘çÜ"
        Private Const COL_PAY As String = "ã‡äz"
        Private Const COL_REVISE_DAY As String = "revise_day"
        Private components As IContainer
        Private Const CONS_REVISE_DAY As String = "ó\éZèCê≥ì˙"
        Private flxExpensesBudgetaryProcess As C1FlexGrid
        Private flxIncomeBudgetaryProcess As C1FlexGrid
        Private grpExpenditure As GroupBox
        Private grpExpensesTable As GroupBox
        Private grpIncomeTable As GroupBox
        Private label1 As Label
        Private label14 As Label
        Private label2 As Label
        Private label3 As Label
        Private label5 As Label
        Private label7 As Label
        Private lblBudgetSub As Label
        Private lblBudgetTotal As Label
        Private lblExpend As Label
        Private lblRevenueTotal As Label
        Private lbltitle As Label
        Private Const REP_L_BIKO As String = "l_biko"
        Private Const REP_L_NAME As String = "l_name"
        Private Const REP_L_NUMBER As String = "l_number"
        Private Const REP_L_PERIOD As String = "l_period"
        Private Const REP_L_REVISE_DAY As String = "l_revise_day"
        Private Const REP_NAME_DATASET As String = "dsBudgetary"
        Private Const REP_NAME_FILE As String = "Report.RevenueExpenditure.RptBudgetary"
        Private Const REP_S_BUDGETARY_MONEY As String = "s_budgetary_money"
        Private Const REP_S_NUMBER As String = "s_number"
        Private Const REVISE_REVENUE As String = "ëgçáîÔ"
        Private Const TABLE_EXPEND As String = "Expend"
        Private Const TABLE_REVENUE As String = "Revenue"
        Private txtRevenueEndDate As PersonalTextBox
        Private txtRevenueStrDate As PersonalTextBox
        Private _parent As UC050401
    End Class
End Namespace
