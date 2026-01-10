
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
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.Command
Imports UnionAct.GUI.UnionComponent
Imports UnionAct.Business.RevenueExpenditure
Imports UnionAct.NSMDInfo

Namespace GUI.RevenueExpenditure.UnionForm
    Public Class CtlAllottedCharge
        Inherits RevenueExpenditureBase
        ' Methods
        Private Sub New()
            MyBase.New()
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal _strRevenueStart As String, ByVal _strRevenueEnd As String, ByVal _strLastRevenueStart As String, ByVal _IsNewFlg As Boolean, ByVal _IsChangeFlg As Boolean, ByVal _strRevenueTitle As String, ByVal _dtDup As Object, ByVal _IsReferenceRight As Boolean, ByVal _IsGetEntryRight As Boolean, ByVal _IsPrintRight As Boolean, ByVal parent As UC050401)
            MyBase.New(_IsGetEntryRight, _IsPrintRight, _IsReferenceRight, _strRevenueTitle, _dtDup, _strRevenueStart, _strRevenueEnd, _strLastRevenueStart, _IsNewFlg, _IsChangeFlg)
            Me.InitializeComponent()
            Me._isEdit = _IsNewFlg
            Me._parent = parent
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

        Private Sub btnEntryConfirm_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Dim num As Integer
                Me.Cursor = Cursors.WaitCursor
                CalcRowTotal()
                CalcTotalPay()
                Me.CheckBeforeEntry()
                If Not MyBase._IsNewFlg Then
                    MyBase.CheckUpdateMessage(MyBase.Name)
                End If
                Dim printData As DataSet = Me.GetPrintData
                Dim viewer As New ReportViewer(printData, New CR0504P4, 1)
                Dim num2 As Integer = viewer.ConfirmViewerShow(num)
                viewer.RptDataDispose()
                If (num2 <> 2) Then
                    Dim entryData As DataSet = Me.GetEntryData
                    Me._busAlloted.Entry(entryData, MyBase._IsNewFlg, MyBase._dtDup)
                    If (num2 = 0) Then
                        viewer = New ReportViewer(printData, New CR0504P4, num)
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
                Dim viewer As New ReportViewer(Me.GetPrintData, New CR0504P4, 3)
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

        Private Sub CalcRowTotal()
            Try
                Dim i As Integer
                For i = 1 To Me.flxAllottedCharge.Rows.Count - 1
                    Me.CalcRowTotal(i)
                Next i
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub CalcRowTotal(ByVal iRowIndex As Integer)
            Try
                Dim nullable As Long? = Nothing
                nullable = ((Me.GetCellValueAllowNull(iRowIndex, "éxï•íPâø") * Me.GetCellValueAllowNull(iRowIndex, "îNä‘éxï•åéêî")) * Me.GetCellValueAllowNull(iRowIndex, "ëŒè€ëgçáàıêî"))
                Me.flxAllottedCharge.Item(iRowIndex, "çáåvã‡äz") = nullable
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub CalcTotalPay()
            Try
                Dim num As Long = 0
                Dim i As Integer
                For i = 1 To Me.flxAllottedCharge.Rows.Count - 1
                    If Not MDFinanceCommon.IsEmptyCell(Me.flxAllottedCharge, i, "çáåvã‡äz") Then
                        num = (num + Convert.ToInt64(Me.flxAllottedCharge.Item(i, "çáåvã‡äz")))
                    End If
                Next i
                Me.lblTotalSum.Text = String.Format("{0:N0}", num)
            Catch exception As OverflowException
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "GE0174", New String() {"ëççáåvã‡äz"})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub CheckBeforeEntry()
            Try
                Me.CheckRequire()
                Me.CheckTotalPayByte()
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

        Private Sub CheckRequire()
            Try
                Dim appEx As AppUnionException = Nothing
                Dim i As Integer
                For i = 1 To Me.flxAllottedCharge.Rows.Count - 1
                    Dim j As Integer = Me.flxAllottedCharge.Cols.Item("äÓèÄåé").Index
                    Do While (j <= Me.flxAllottedCharge.Cols.Item("ëŒè€ëgçáàıêî").Index)
                        If MDFinanceCommon.IsEmptyCell(Me.flxAllottedCharge, i, j) Then
                            Me.flxAllottedCharge.SetCellStyle(i, j, MDFinanceCommon.GetErrorStyle(Me.flxAllottedCharge, True))
                            PublicCommand.AddAppUnionExceptionData(appEx, "GE0170", New String() {Me.flxAllottedCharge.Item(i, "â»ñ⁄").ToString, Me.flxAllottedCharge.Item(i, "â»ñ⁄ç◊ñ⁄").ToString, Me.flxAllottedCharge.Cols.Item(j).Name})
                        Else
                            Me.flxAllottedCharge.SetCellStyle(i, j, Me.flxAllottedCharge.Cols.Item(j).Style)
                        End If
                        j += 1
                    Loop
                Next i
                If (Not appEx Is Nothing) Then
                    Throw appEx
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

        Private Sub CheckTotalPayByte()
            Try
                Dim appEx As AppUnionException = Nothing
                Dim i As Integer
                For i = 1 To Me.flxAllottedCharge.Rows.Count - 1
                    Dim byteLength As Integer = PublicCommand.GetByteLength(Me.flxAllottedCharge.Item(i, "çáåvã‡äz").ToString)
                    If (9 < byteLength) Then
                        PublicCommand.AddAppUnionExceptionData(appEx, "GW0027", New String() {Me.flxAllottedCharge.Item(i, "â»ñ⁄").ToString, Me.flxAllottedCharge.Item(i, "â»ñ⁄ç◊ñ⁄").ToString, "çáåvã‡äz"})
                    End If
                Next i
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

        Private Sub CtlAllottedCharge_Load(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Me.SetHeaderData()
                Me._dSetAtLoad = Me._busAlloted.GetAllottedCharge(MyBase._RevenueStartDate, MyBase._LastRevenueStartDate, MyBase._IsNewFlg)
                Me.SetInitialData()
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

        Private Sub flxAllottedCharge_Click(ByVal sender As Object, ByVal e As EventArgs)
            Try
                If (Me._isEdit AndAlso MDFinanceCommon.CheckMouseCursorPoint(Me.flxAllottedCharge)) Then
                    Me.flxAllottedCharge.StartEditing()
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

        Private Sub flxAllottedCharge_SetupEditor(ByVal sender As Object, ByVal e As RowColEventArgs)
            Try
                If TypeOf Me.flxAllottedCharge.Editor Is PersonalTextBox Then
                    Dim editor As PersonalTextBox = DirectCast(Me.flxAllottedCharge.Editor, PersonalTextBox)
                    editor.ImeMode = ImeMode.Disable
                    editor.MaxLength = Me.GetMaxLength(Me.flxAllottedCharge.Cols.Item(e.Col).Name)
                ElseIf TypeOf Me.flxAllottedCharge.Editor Is DateTimePicker Then
                    Dim picker As DateTimePicker = DirectCast(Me.flxAllottedCharge.Editor, DateTimePicker)
                    Dim dateTimeFromString As DateTime = Me.GetDateTimeFromString(MyBase._RevenueStartDate)
                    Dim time2 As DateTime = Me.GetDateTimeFromString(MyBase._RevenueEndDate)
                    If ((DateTime.Compare(picker.Value.Date, dateTimeFromString) < 0) OrElse (DateTime.Compare(time2, picker.Value.Date) < 0)) Then
                        picker.Value = dateTimeFromString
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
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub flxAllottedCharge_StartEdit(ByVal sender As Object, ByVal e As RowColEventArgs)
            Try
                Dim array As String() = New String() {"äÓèÄåé", "éxï•íPâø", "îNä‘éxï•åéêî", "ëŒè€ëgçáàıêî"}
                If (System.Array.IndexOf(Of String)(array, Me.flxAllottedCharge.Cols.Item(Me.flxAllottedCharge.Col).Name) < 0) Then
                    e.Cancel = True
                Else
                    DirectCast(Me.flxAllottedCharge.DataSource, DataTable).AcceptChanges()
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

        Private Sub flxAllottedCharge_ValidateEdit(ByVal sender As Object, ByVal e As ValidateEditEventArgs)
            Try
                If TypeOf Me.flxAllottedCharge.Editor Is DateTimePicker Then
                    Me.ValidateDateTime(e)
                ElseIf TypeOf Me.flxAllottedCharge.Editor Is PersonalTextBox Then
                    If String.IsNullOrEmpty(Me.flxAllottedCharge.Editor.Text) Then
                        Me.flxAllottedCharge.Item(e.Row, e.Col) = DBNull.Value
                    Else
                        Dim num As Long
                        If ValidatorUtility.ValidateNumericValue(Me.flxAllottedCharge.Editor.Text, num) Then
                            If (num < 0) Then
                                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0173", New String(0 - 1) {})
                            End If
                            Me.flxAllottedCharge.Item(e.Row, e.Col) = num
                        Else
                            Me.flxAllottedCharge.FinishEditing(True)
                        End If
                    End If
                    Me.CalcRowTotal(e.Row)
                    Me.CalcTotalPay()
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                DirectCast(Me.flxAllottedCharge.DataSource, DataTable).RejectChanges()
                Me.flxAllottedCharge.FinishEditing(True)
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Function GetCellValueAllowNull(ByVal iRowIndex As Integer, ByVal strColumnName As String) As Long?
            Dim nullable As Long?
            Try
                If MDFinanceCommon.IsEmptyCell(Me.flxAllottedCharge, iRowIndex, strColumnName) Then
                    Return Nothing
                End If
                nullable = New Long?(Convert.ToInt64(Me.flxAllottedCharge.Item(iRowIndex, strColumnName)))
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return nullable
        End Function

        Private Function GetDateTimeFromString(ByVal strDate As String) As DateTime
            Dim time As DateTime
            Try
                time = DateTime.ParseExact(strDate, "yyyyMMdd", Nothing)
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return time
        End Function

        Private Function GetEntryData() As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim map As New RevenueAllottedChargeMap
                Dim table As DataTable = map.CreateDataTablePhysName("revenue_allotted_charge")
                Dim i As Integer
                For i = 1 To Me.flxAllottedCharge.Rows.Count - 1
                    Dim row As DataRow = table.NewRow
                    row.Item(map.GetPhysicalName(0)) = MyBase._RevenueStartDate
                    row.Item(map.GetPhysicalName(1)) = Me.flxAllottedCharge.Item(i, "c_expend_item")
                    row.Item(map.GetPhysicalName(2)) = Me.flxAllottedCharge.Item(i, "c_expend_item_seq")
                    row.Item(map.GetPhysicalName(3)) = Me.flxAllottedCharge.Item(i, "éxï•íPâø")
                    row.Item(map.GetPhysicalName(4)) = Me.flxAllottedCharge.Item(i, "îNä‘éxï•åéêî")
                    row.Item(map.GetPhysicalName(5)) = CDate(Me.flxAllottedCharge.Item(i, "äÓèÄåé")).ToString("yyyyMMdd")
                    row.Item(map.GetPhysicalName(6)) = Me.flxAllottedCharge.Item(i, "ëŒè€ëgçáàıêî")
                    row.Item(map.GetPhysicalName(7)) = Me.flxAllottedCharge.Item(i, "çáåvã‡äz")
                    row.Item(map.GetPhysicalName(8)) = DBNull.Value
                    row.Item(map.GetPhysicalName(9)) = PublicCommand.GetNow
                    row.Item(map.GetPhysicalName(11)) = PublicCommand.GetNow
                    row.Item(map.GetPhysicalName(10)) = MDLoginInfo.UserId
                    row.Item(map.GetPhysicalName(12)) = MDLoginInfo.UserId
                    row.Item(map.GetPhysicalName(13)) = 0
                    table.Rows.Add(row)
                Next i
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

        Private Function GetMaxLength(ByVal strColName As String) As Integer
            Dim num As Integer
            Try
                If strColName.Equals("éxï•íPâø") Then
                    Return 9
                End If
                If strColName.Equals("îNä‘éxï•åéêî") Then
                    Return 3
                End If
                num = 6
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return num
        End Function

        Private Function GetPrintData() As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet("dsAllottedCharge")
                Dim table As New DataTable("dtHeader")
                table.Columns.Add("l_period", GetType(String))
                table.Rows.Add(New Object() {MyBase._RevenueTitle})
                Dim printDetailData As DataTable = Me.GetPrintDetailData
                Dim table3 As DataTable = Me.GetPrintDetail2Data
                ds.Tables.Add(table)
                ds.Tables.Add(printDetailData)
                ds.Tables.Add(table3)
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

        Private Function GetPrintDetail2Data() As DataTable
            Dim table3 As DataTable
            Try
                Dim table As New DataTable("dtDetail2")
                table.Columns.Add("l_expend_item_name", GetType(String))
                table.Columns.Add("s_total", GetType(Long))
                Dim table2 As DataTable = DirectCast(Me.flxAllottedCharge.DataSource, DataTable).Copy
                table2.AcceptChanges()
                Do While (table2.Rows.Count <> 0)
                    Dim row As DataRow = table.NewRow
                    Dim rowArray As DataRow() = table2.Select(("c_expend_item = '" & table2.Rows.Item(0).Item("c_expend_item").ToString & "' "))
                    row.Item("l_expend_item_name") = rowArray(0).Item("â»ñ⁄")
                    Dim num As Long = 0
                    Dim i As Integer
                    For i = 0 To rowArray.Length - 1
                        num = (num + Convert.ToInt64(rowArray(i).Item("çáåvã‡äz")))
                        rowArray(i).Delete()
                    Next i
                    row.Item("s_total") = num
                    table2.AcceptChanges()
                    table.Rows.Add(row)
                Loop
                table3 = table
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Private Function GetPrintDetailData() As DataTable
            Dim table2 As DataTable
            Try
                Dim table As New DataTable("dtDetail")
                table.Columns.Add("l_expend_item_name", GetType(String))
                table.Columns.Add("l_expend_item_seq_name", GetType(String))
                table.Columns.Add("s_unit_price", GetType(Long))
                table.Columns.Add("s_year_month", GetType(Integer))
                table.Columns.Add("s_member", GetType(Integer))
                table.Columns.Add("d_standard_date", GetType(String))
                Dim i As Integer
                For i = 1 To Me.flxAllottedCharge.Rows.Count - 1
                    Dim row As DataRow = table.NewRow
                    row.Item("l_expend_item_name") = Me.flxAllottedCharge.Item(i, "â»ñ⁄")
                    row.Item("l_expend_item_seq_name") = Me.flxAllottedCharge.Item(i, "â»ñ⁄ç◊ñ⁄")
                    row.Item("s_unit_price") = Me.flxAllottedCharge.Item(i, "éxï•íPâø")
                    row.Item("s_year_month") = Me.flxAllottedCharge.Item(i, "îNä‘éxï•åéêî")
                    row.Item("s_member") = Me.flxAllottedCharge.Item(i, "ëŒè€ëgçáàıêî")
                    row.Item("d_standard_date") = CDate(Me.flxAllottedCharge.Item(i, "äÓèÄåé")).ToString("MM" & "åé" & "dd" & "ì˙")
                    table.Rows.Add(row)
                Next i
                table2 = table
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Private Sub InitializeComponent()
            'TODO Dim manager As New ComponentResourceManager(GetType(CtlAllottedCharge))
            Me.grpExpenditure = New GroupBox
            Me.lbltitle = New Label
            Me.txtRevenueStrDate = New PersonalTextBox
            Me.txtRevenueEndDate = New PersonalTextBox
            Me.label1 = New Label
            Me.label2 = New Label
            Me.grpAllottedChargeTable = New GroupBox
            Me.label3 = New Label
            Me.label14 = New Label
            Me.lblTotalSum = New Label
            Me.flxAllottedCharge = New C1FlexGrid
            Me.btnChange = New Button
            Me.btnCancel = New Button
            Me.btnPrinting = New Button
            Me.btnEntryConfirm = New Button
            Me.btnBack = New Button
            Me.grpExpenditure.SuspendLayout()
            Me.grpAllottedChargeTable.SuspendLayout()
            Me.flxAllottedCharge.BeginInit()
            MyBase.SuspendLayout()
            Me.grpExpenditure.Controls.Add(Me.lbltitle)
            Me.grpExpenditure.Controls.Add(Me.txtRevenueStrDate)
            Me.grpExpenditure.Controls.Add(Me.txtRevenueEndDate)
            Me.grpExpenditure.Controls.Add(Me.label1)
            Me.grpExpenditure.Controls.Add(Me.label2)
            Me.grpExpenditure.Location = New Point(&HA7, 13)
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
            Me.txtRevenueStrDate.TabIndex = 0
            Me.txtRevenueStrDate.TabStop = False
            Me.txtRevenueStrDate.TextAlign = HorizontalAlignment.Center
            Me.txtRevenueEndDate.BackColor = Color.LightYellow
            Me.txtRevenueEndDate.FieldAttribute = EFieldAttribute.NONE
            Me.txtRevenueEndDate.Location = New Point(470, &H16)
            Me.txtRevenueEndDate.Name = "txtRevenueEndDate"
            Me.txtRevenueEndDate.ReadOnly = True
            Me.txtRevenueEndDate.Require = False
            Me.txtRevenueEndDate.Size = New Size(&H95, &H17)
            Me.txtRevenueEndDate.TabIndex = 1
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
            Me.grpAllottedChargeTable.Controls.Add(Me.label3)
            Me.grpAllottedChargeTable.Controls.Add(Me.label14)
            Me.grpAllottedChargeTable.Controls.Add(Me.lblTotalSum)
            Me.grpAllottedChargeTable.Controls.Add(Me.flxAllottedCharge)
            Me.grpAllottedChargeTable.Location = New Point(8, &H7D)
            Me.grpAllottedChargeTable.Name = "grpAllottedChargeTable"
            Me.grpAllottedChargeTable.Size = New Size(&H3F2, &H1D3)
            Me.grpAllottedChargeTable.TabIndex = 1
            Me.grpAllottedChargeTable.TabStop = False
            Me.grpAllottedChargeTable.Text = "ï™íSã‡ç◊ñ⁄èÓïÒ"
            Me.label3.AutoSize = True
            Me.label3.ForeColor = Color.Red
            Me.label3.Location = New Point(20, &H1AB)
            Me.label3.Name = "label3"
            Me.label3.Size = New Size(360, &H10)
            Me.label3.TabIndex = &H97
            Me.label3.Text = "Å¶Å@äÓèÄåéÇ…ÇÕó\ëzä˙ä‘ì‡ÇÃîNåéÇê›íËÇµÇƒÇ≠ÇæÇ≥Ç¢ÅB"
            Me.label14.AutoSize = True
            Me.label14.Location = New Point(720, &H1AB)
            Me.label14.Name = "label14"
            Me.label14.Size = New Size(&H58, &H10)
            Me.label14.TabIndex = 150
            Me.label14.Text = "ëççáåvã‡äz"
            Me.lblTotalSum.BorderStyle = BorderStyle.Fixed3D
            Me.lblTotalSum.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblTotalSum.Location = New Point(&H336, &H1A9)
            Me.lblTotalSum.Name = "lblTotalSum"
            Me.lblTotalSum.Size = New Size(160, &H15)
            Me.lblTotalSum.TabIndex = &H95
            Me.lblTotalSum.Text = "0"
            Me.lblTotalSum.TextAlign = ContentAlignment.MiddleRight
            Me.flxAllottedCharge.AllowDragging = AllowDraggingEnum.None
            Me.flxAllottedCharge.AllowEditing = False
            Me.flxAllottedCharge.AllowMerging = AllowMergingEnum.RestrictRows
            Me.flxAllottedCharge.AllowSorting = AllowSortingEnum.None
            Me.flxAllottedCharge.AutoResize = False
            Me.flxAllottedCharge.BorderStyle = C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum.XpThemes
            'TODO Me.flxAllottedCharge.ColumnInfo = manager.GetString("flxAllottedCharge.ColumnInfo")
            Me.flxAllottedCharge.FocusRect = FocusRectEnum.None
            Me.flxAllottedCharge.KeyActionTab = KeyActionEnum.MoveAcrossOut
            Me.flxAllottedCharge.Location = New Point(&H10, &H26)
            Me.flxAllottedCharge.Name = "flxAllottedCharge"
            Me.flxAllottedCharge.Rows.Count = 6
            Me.flxAllottedCharge.Rows.DefaultSize = 20
            Me.flxAllottedCharge.SelectionMode = SelectionModeEnum.Row
            Me.flxAllottedCharge.Size = New Size(&H3D3, 370)
            'TODO Me.flxAllottedCharge.StyleInfo = manager.GetString("flxAllottedCharge.StyleInfo")
            Me.flxAllottedCharge.TabIndex = 0
            AddHandler Me.flxAllottedCharge.StartEdit, New RowColEventHandler(AddressOf Me.flxAllottedCharge_StartEdit)
            AddHandler Me.flxAllottedCharge.ValidateEdit, New ValidateEditEventHandler(AddressOf Me.flxAllottedCharge_ValidateEdit)
            AddHandler Me.flxAllottedCharge.SetupEditor, New RowColEventHandler(AddressOf Me.flxAllottedCharge_SetupEditor)
            AddHandler Me.flxAllottedCharge.Click, New EventHandler(AddressOf Me.flxAllottedCharge_Click)
            Me.btnChange.Location = New Point(700, &H28D)
            Me.btnChange.Name = "btnChange"
            Me.btnChange.Size = New Size(&H74, &H20)
            Me.btnChange.TabIndex = 3
            Me.btnChange.Text = "ì‡óeïœçX"
            Me.btnChange.UseVisualStyleBackColor = True
            AddHandler Me.btnChange.Click, New EventHandler(AddressOf Me.btnChange_Click)
            Me.btnCancel.Location = New Point(830, &H28D)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New Size(&H74, &H20)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "ÉLÉÉÉìÉZÉã"
            Me.btnCancel.UseVisualStyleBackColor = True
            AddHandler Me.btnCancel.Click, New EventHandler(AddressOf Me.btnCancel_Click)
            Me.btnPrinting.Location = New Point(&H1F, &H28D)
            Me.btnPrinting.Name = "btnPrinting"
            Me.btnPrinting.Size = New Size(&H74, &H20)
            Me.btnPrinting.TabIndex = 2
            Me.btnPrinting.Text = "ÉvÉåàÛç¸"
            Me.btnPrinting.UseVisualStyleBackColor = True
            AddHandler Me.btnPrinting.Click, New EventHandler(AddressOf Me.btnPrinting_Click)
            Me.btnEntryConfirm.Location = New Point(700, &H28D)
            Me.btnEntryConfirm.Name = "btnEntryConfirm"
            Me.btnEntryConfirm.Size = New Size(&H74, &H20)
            Me.btnEntryConfirm.TabIndex = 3
            Me.btnEntryConfirm.Text = "ìoò^ämîF"
            Me.btnEntryConfirm.UseVisualStyleBackColor = True
            AddHandler Me.btnEntryConfirm.Click, New EventHandler(AddressOf Me.btnEntryConfirm_Click)
            Me.btnBack.Location = New Point(830, &H28D)
            Me.btnBack.Name = "btnBack"
            Me.btnBack.Size = New Size(&H74, &H20)
            Me.btnBack.TabIndex = 4
            Me.btnBack.Text = "ñﬂÇÈ"
            Me.btnBack.UseVisualStyleBackColor = True
            AddHandler Me.btnBack.Click, New EventHandler(AddressOf Me.btnBack_Click)
            MyBase.Controls.Add(Me.btnPrinting)
            MyBase.Controls.Add(Me.btnCancel)
            MyBase.Controls.Add(Me.grpAllottedChargeTable)
            MyBase.Controls.Add(Me.grpExpenditure)
            MyBase.Controls.Add(Me.btnEntryConfirm)
            MyBase.Controls.Add(Me.btnBack)
            MyBase.Controls.Add(Me.btnChange)
            Me.Font = New Font("MS UI Gothic", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            MyBase.Name = "CtlAllottedCharge"
            MyBase.Size = New Size(&H402, &H2F7)
            AddHandler MyBase.Load, New EventHandler(AddressOf Me.CtlAllottedCharge_Load)
            Me.grpExpenditure.ResumeLayout(False)
            Me.grpExpenditure.PerformLayout()
            Me.grpAllottedChargeTable.ResumeLayout(False)
            Me.grpAllottedChargeTable.PerformLayout()
            Me.flxAllottedCharge.EndInit()
            MyBase.ResumeLayout(False)
        End Sub

        Private Sub SetGridColStyle()
            Try
                Me.flxAllottedCharge.Cols.Frozen = (Me.flxAllottedCharge.Cols.Item("â»ñ⁄ç◊ñ⁄").Index + 1)
                Me.flxAllottedCharge.Cols.Item("â»ñ⁄").Style = Me.flxAllottedCharge.Styles.Fixed
                Me.flxAllottedCharge.Cols.Item("â»ñ⁄ç◊ñ⁄").Style = Me.flxAllottedCharge.Styles.Fixed
                Me.flxAllottedCharge.Cols.Item("çáåvã‡äz").Style = MDFinanceCommon.GetCantEditStyle(Me.flxAllottedCharge)
                Me.flxAllottedCharge.Cols.Item("éxï•íPâø").Format = "N0"
                Me.flxAllottedCharge.Cols.Item("çáåvã‡äz").Format = "N0"
                Me.flxAllottedCharge.Cols.Item("äÓèÄåé").Format = "yyyy" & "îN" & "MM" & "åé"
                Me.flxAllottedCharge.Cols.Item("äÓèÄåé").Style.BackColor = Color.White
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub SetGridOutline()
            Try
                Dim dicColWidthPair As New Dictionary(Of String, Integer)
                dicColWidthPair.Add("â»ñ⁄", 180)
                dicColWidthPair.Add("â»ñ⁄ç◊ñ⁄", 210)
                dicColWidthPair.Add("äÓèÄåé", 110)
                dicColWidthPair.Add("éxï•íPâø", &H55)
                dicColWidthPair.Add("îNä‘éxï•åéêî", 110)
                dicColWidthPair.Add("ëŒè€ëgçáàıêî", &H73)
                dicColWidthPair.Add("çáåvã‡äz", 130)
                MDFinanceCommon.SetColsWidth(Me.flxAllottedCharge, dicColWidthPair)
                MDFinanceCommon.AdjustTextAlign(Me.flxAllottedCharge)
                Me.SetGridColStyle()
                Me.flxAllottedCharge.Cols.Item("â»ñ⁄").AllowMerging = True
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
                Me.lbltitle.Text = MyBase._RevenueTitle
                Me.txtRevenueStrDate.Text = DateTime.ParseExact(MyBase._RevenueStartDate, "yyyyMMdd", Nothing).ToString("yyyy" & "îN" & "MM" & "åé" & "dd" & "ì˙")
                Me.txtRevenueEndDate.Text = DateTime.ParseExact(MyBase._RevenueEndDate, "yyyyMMdd", Nothing).ToString("yyyy" & "îN" & "MM" & "åé" & "dd" & "ì˙")
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub SetInitialData()
            Try
                Me.flxAllottedCharge.DataSource = Me._dSetAtLoad.Tables.Item("revenue_allotted_charge").Copy
                Me.SetGridOutline()
                Me.CalcRowTotal()
                Me.CalcTotalPay()
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
                Utilities.SetVisibleProperty((Not Me._isEdit AndAlso MyBase._IsPrintRight), New Control() {Me.btnPrinting})
                Utilities.SetVisibleProperty(((Not Me._isEdit AndAlso MyBase._IsChangeFlg) AndAlso MyBase._IsGetEntryRight), New Control() {Me.btnChange})
                Me.flxAllottedCharge.AllowEditing = Me._isEdit
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Sub ValidateDateTime(ByVal e As ValidateEditEventArgs)
            Try
                Dim time As DateTime = DirectCast(Me.flxAllottedCharge.Editor, DateTimePicker).Value
                Dim dateTimeFromString As DateTime = Me.GetDateTimeFromString(MyBase._RevenueStartDate)
                Dim time3 As DateTime = Me.GetDateTimeFromString(MyBase._RevenueEndDate)
                If (DateTime.Compare(time.Date, dateTimeFromString) < 0) Then
                    time = dateTimeFromString
                ElseIf (DateTime.Compare(time3, time) < 0) Then
                    time = time3
                End If
                Dim time4 As New DateTime(time.Year, time.Month, 1)
                DirectCast(Me.flxAllottedCharge.Editor, DateTimePicker).Value = time4
                Dim targetMemberCount As Integer = Me._busAlloted.GetTargetMemberCount(MyBase._RevenueStartDate, time4.ToString("yyyyMMdd"))
                Me.flxAllottedCharge.Item(e.Row, "ëŒè€ëgçáàıêî") = targetMemberCount
                Me.CalcRowTotal(e.Row)
                Me.CalcTotalPay()
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
        Private _busAlloted As New AllottedChargeCommand
        Private _dSetAtLoad As DataSet
        Private _isEdit As Boolean
        Private btnBack As Button
        Private btnCancel As Button
        Private btnChange As Button
        Private btnEntryConfirm As Button
        Private btnPrinting As Button
        Private Const COL_BASIS_DATE As String = "äÓèÄåé"
        Private Const COL_ITEM As String = "â»ñ⁄"
        Private Const COL_ITEMDTL As String = "â»ñ⁄ç◊ñ⁄"
        Private Const COL_PAYMONTH_NUM As String = "îNä‘éxï•åéêî"
        Private Const COL_PAYPER As String = "éxï•íPâø"
        Private Const COL_TARGET_MEMBER_NUM As String = "ëŒè€ëgçáàıêî"
        Private Const COL_TOTALPAY As String = "çáåvã‡äz"
        Private components As IContainer
        Private flxAllottedCharge As C1FlexGrid
        Private grpAllottedChargeTable As GroupBox
        Private grpExpenditure As GroupBox
        Private label1 As Label
        Private label14 As Label
        Private label2 As Label
        Private label3 As Label
        Private lbltitle As Label
        Private lblTotalSum As Label
        Private Const REP_D_STANDARD_DATE As String = "d_standard_date"
        Private Const REP_L_EXPEND_ITEM_NAME As String = "l_expend_item_name"
        Private Const REP_L_EXPEND_ITEM_SEQ_NAME As String = "l_expend_item_seq_name"
        Private Const REP_L_PERIOD As String = "l_period"
        Private Const REP_NAME_DATASET As String = "dsAllottedCharge"
        Private Const REP_NAME_FILE As String = "Report.RevenueExpenditure.RptAllottedCharge"
        Private Const REP_S_MEMBER As String = "s_member"
        Private Const REP_S_TOTAL As String = "s_total"
        Private Const REP_S_UNIT_PRICE As String = "s_unit_price"
        Private Const REP_S_YEAR_MONTH As String = "s_year_month"
        Private txtRevenueEndDate As PersonalTextBox
        Private txtRevenueStrDate As PersonalTextBox
        Private _parent As UC050401
    End Class
End Namespace
