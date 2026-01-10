Imports UnionAct.Business.FinancialAffairs
Imports UnionAct.Framework.UnionException
Imports System.Reflection
Imports UnionAct.GUI.Common
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.Framework.Command
Imports C1.Win.C1FlexGrid
Imports C1.Win.C1FlexGrid.Util.BaseControls.BorderStyleEnum
Imports System.Text.RegularExpressions
Imports UnionAct.Framework
Imports UnionAct.GUI.FinancialAffairs
Imports UnionAct.Business.Common
Imports UnionAct.Framework.Mapping
Imports UnionAct.GUI.FinancialAffairs.WithHolding
Imports CrystalDecisions.CrystalReports.Engine
Imports UnionAct.Business.FinancialAffairs.WithHolding
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLAccessMdb

Public Class UC050301
    Inherits FinancialAffairsBase
    ' Methods

    Public Sub New()
        Me._tabActMapping = New Dictionary(Of String, ControlActManager)
        Me.InitializeComponent()
        Me._tabActMapping.Add(Me.pageMonthly.Name, New MonthlyControlActer(Me))
        Me._tabActMapping.Add(Me.pageOnce.Name, New OnceControlActer(Me))
        Me._tabActMapping.Add(Me.pageYearly.Name, New SumUpControlActer(Me))
        Dim existYear As DataSet = Me.GetBusObj(New Object(0 - 1) {}).GetExistYear
        Dim manager As ControlActManager
        For Each manager In Me._tabActMapping.Values
            manager.AddGridStyle()
            manager.InitButtons()
            manager.InitConditionCombo(existYear)
        Next
    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　新規集計ボタン押下処理
    ''' 一時金集計タブ　新規集計ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnNewCalc_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnNewCalc.Click, _
        btnOnceNewCalc.Click

        Dim acterFromControl As CutDivControlActer = TryCast(Me.GetActerFromControl(sender), CutDivControlActer)
        Dim form As New FM050303(New SelectMonthEventHandler(AddressOf acterFromControl.FrmSelectCalcMonth_Execute), New EventHandler(AddressOf Me.FrmSelectCalcMonth_Cancel))
        form.ShowDialog(MyBase.ParentForm)

    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　課税非対象者の一覧プレ印刷ボタン押下処理
    ''' 一時金集計タブ　課税非対象者の一覧プレ印刷ボタン押下処理
    ''' 累計タブ　課税非対象者の一覧プレ印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintListNonTaxable_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnPrintListNonTaxableMonthly.Click, _
        btnPrintListNonTaxableOnce.Click, _
        btnPrintListNonTaxableSumUp.Click

        Try
            Me.ShowReportViewer(sender, ReportKind.LIST, False)
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            'CLMsg.Show("GE0001")
            MsgBox(ex.ToString, vbExclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　課税対象者の一覧プレ印刷ボタン押下処理
    ''' 一時金集計タブ　課税対象者の一覧プレ印刷ボタン押下処理
    ''' 累計タブ　課税対象者の一覧プレ印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintListTaxable_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnPrintListTaxableMonthly.Click, _
        btnPrintListTaxableOnce.Click, _
        btnPrintListTaxableSumUp.Click

        Try
            Me.ShowReportViewer(sender, ReportKind.LIST, True)
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            'CLMsg.Show("GE0001")
            MsgBox(ex.ToString, vbExclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　課税非対象者の合計プレ印刷ボタン押下処理
    ''' 一時金集計タブ　課税非対象者の合計プレ印刷ボタン押下処理
    ''' 累計タブ　課税非対象者の合計プレ印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintSumNonTaxable_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnPrintSumNonTaxableMonthly.Click, _
        btnPrintSumNonTaxableOnce.Click, _
        btnPrintSumNonTaxableSumUp.Click

        Try
            Me.ShowReportViewer(sender, ReportKind.SUM, False)
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            'CLMsg.Show("GE0001")
            MsgBox(ex.ToString, vbExclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　課税対象者の合計プレ印刷ボタン押下処理
    ''' 一時金集計タブ　課税対象者の合計プレ印刷ボタン押下処理
    ''' 累計タブ　課税対象者の合計プレ印刷ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrintSumTaxable_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnPrintSumTaxableOnce.Click, _
        btnPrintSumTaxableMonthly.Click, _
        btnPrintSumTaxableSumUp.Click

        Try
            Me.ShowReportViewer(sender, ReportKind.SUM, True)
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            'CLMsg.Show("GE0001")
            MsgBox(ex.ToString, vbExclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　検索ボタン押下処理
    ''' 一時金集計タブ　検索ボタン押下処理
    ''' 累計タブ　検索ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnQuery_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnMonthlyQuery.Click, _
        btnOnceQuery.Click, _
        btnSumUpQuery.Click

        Try
            Me.GetActerFromControl(sender).QueryWithholding()
        Catch ex As Exception
            Dim msg As New ExceptionMsg(ex)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
            MsgBox(ex.ToString, vbExclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　再集計ボタン押下処理
    ''' 一時金集計タブ　再集計ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnReCalc_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnReCalc.Click, _
        btnOnceReCalc.Click

        Try
            TryCast(Me.GetActerFromControl(sender), CutDivControlActer).ClickRecalc()
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            'CLMsg.Show("GE0001")
            MsgBox(ex.ToString, vbExclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　課税非対象者の照会ボタン押下処理
    ''' 一時金集計タブ　課税非対象者の照会ボタン押下処理
    ''' 累計タブ　課税非対象者の照会ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRefNonTaxable_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnRefNonTaxableSumUp.Click, _
        btnRefNonTaxableOnce.Click, _
        btnRefNonTaxableMonthly.Click

        Try
            Dim acterFromControl As ControlActManager = Me.GetActerFromControl(sender)
            Me._detail = acterFromControl.GetNonTaxableDetailControl
            Utilities.OverlayUserControl(ParentForm.Controls(MDConst.MAIN_PANEL_ID), acterFromControl.FUNCTION_NAME_NONTAXABLE, New UserControl() {Me._detail})
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            'CLMsg.Show("GE0001")
            MsgBox(ex.ToString, vbExclamation)
        End Try
    End Sub

    ''' <summary>
    ''' 月例賃金集計タブ　課税対象者の照会ボタン押下処理
    ''' 一時金集計タブ　課税対象者の照会ボタン押下処理
    ''' 累計タブ　課税対象者の照会ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnRefTaxable_Click( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        btnRefTaxableSumUp.Click, _
        btnRefTaxableOnce.Click, _
        btnRefTaxableMonthly.Click

        Try
            Dim acterFromControl As ControlActManager = Me.GetActerFromControl(sender)
            Me._detail = acterFromControl.GetTaxableDetailControl
            Utilities.OverlayUserControl(ParentForm.Controls(MDConst.MAIN_PANEL_ID), acterFromControl.FUNCTION_NAME_TAXABLE, New UserControl() {Me._detail})
        Catch exception As SysUnionException
            exception.AddMethodName(MethodBase.GetCurrentMethod)
            'CLMsg.Show("GE0001")
            MsgBox(exception.ToString, vbExclamation)
        End Try
    End Sub

    Private Sub CtlWithHoldingDetail_Cancel( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    )
        If (Not Me._detail Is Nothing) Then
            Me._detail.Dispose()
            Me._detail = Nothing
            Utilities.RestoreUserControl()
            If TypeOf sender Is CtlMonthlyTaxableDetail Then
                Me._tabActMapping.Item(Me.tabWithholding.SelectedTab.Name).QueryWithholding()
            End If
        End If
    End Sub

    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If (disposing AndAlso (Not Me.components Is Nothing)) Then
            Me.components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub FrmSelectCalcMonth_Cancel(ByVal sender As Object, ByVal e As EventArgs)
        DirectCast(sender, Form).Dispose()
    End Sub

    Private Function GetActerFromControl(ByVal sender As Object) As ControlActManager
        Dim manager As ControlActManager = Nothing
        Try
            Dim ctl As Control = TryCast(sender, Control)
            Dim name As String = Nothing
            If Me.pageMonthly.Contains(ctl) Then
                name = Me.pageMonthly.Name
            ElseIf Me.pageOnce.Contains(ctl) Then
                name = Me.pageOnce.Name
            ElseIf Me.pageYearly.Contains(ctl) Then
                name = Me.pageYearly.Name
            End If
            manager = Me._tabActMapping.Item(name)
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return Nothing
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
        Return manager
    End Function

    Private Function GetBusFactory() As FactoryBusClass
        Return New FactoryBusClass
    End Function

    Private Function GetBusObj(ByVal ParamArray args As Object()) As WithholdingCommand
        'Return AbstractGui.GetBusinessObject(Of IWithholdingCommand)("Business.FinancialAffairs.WithHolding.WithholdingCommand", args)
        If args.Length = 0 Then
            Return New WithholdingCommand()
        Else
            Return New WithholdingCommand(args(0))
        End If
    End Function

    Private Sub cmbMonthlyYear_KeyPress( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyPressEventArgs _
    ) Handles _
        cmbMonthlyYear.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                Me.GetActerFromControl(sender).QueryWithholding()
            Catch ex As Exception
                Dim msg As New ExceptionMsg(ex)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                MsgBox(ex.ToString, vbExclamation)
            End Try
        End If

    End Sub

    Private Sub cmbMonthlyMonth_KeyPress( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyPressEventArgs _
    ) Handles _
        cmbMonthlyMonth.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                Me.GetActerFromControl(sender).QueryWithholding()
            Catch ex As Exception
                Dim msg As New ExceptionMsg(ex)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                MsgBox(ex.ToString, vbExclamation)
            End Try
        End If

    End Sub

    Private Sub cmbOnceYear_KeyPress( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyPressEventArgs _
    ) Handles _
        cmbOnceYear.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                Me.GetActerFromControl(sender).QueryWithholding()
            Catch ex As Exception
                Dim msg As New ExceptionMsg(ex)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                MsgBox(ex.ToString, vbExclamation)
            End Try
        End If

    End Sub

    Private Sub cmbOnceMonth_KeyPress( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyPressEventArgs _
    ) Handles _
        cmbOnceMonth.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                Me.GetActerFromControl(sender).QueryWithholding()
            Catch ex As Exception
                Dim msg As New ExceptionMsg(ex)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                MsgBox(ex.ToString, vbExclamation)
            End Try
        End If

    End Sub

    Private Sub cmbSumUpYear_KeyPress( _
        ByVal sender As Object, _
        ByVal e As System.Windows.Forms.KeyPressEventArgs _
    ) Handles _
        cmbSumUpYear.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                Me.GetActerFromControl(sender).QueryWithholding()
            Catch ex As Exception
                Dim msg As New ExceptionMsg(ex)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return
                End If
                msg.ShowMessage()
                MsgBox(ex.ToString, vbExclamation)
            End Try
        End If

    End Sub

    Private Sub QueryMonthlyConditionChanged( _
        ByVal sender As Object, _
        ByVal e As EventArgs _
    ) Handles _
        cmbSumUpYear.TextChanged, _
        cmbOnceYear.TextChanged, _
        cmbOnceMonth.TextChanged, _
        cmbMonthlyYear.TextChanged, _
        cmbMonthlyMonth.TextChanged

        Me.GetActerFromControl(sender).ChangedCondition()

    End Sub

    Private Sub ShowReportViewer( _
        ByVal sender As Object, _
        ByVal reportKind As ReportKind, _
        ByVal isTaxable As Boolean _
    )
        Try
            Me.GetActerFromControl(sender).PrintReport(reportKind, isTaxable)
        Catch exception As AppUnionException
            Dim msg As New ExceptionMsg(exception)
            If msg.IsNotContinue Then
                CLMsg.Show("GE0001")
                Return
            End If
            msg.ShowMessage()
        Catch ex As Exception
            CLMsg.Show("GE0001")
        End Try
    End Sub

    ' Nested Types
    Private MustInherit Class ControlActManager
        ' Methods
        Public Sub New(ByVal UC050301 As UC050301)
            Me._ctlList = UC050301
            Me._setteing_taxable = Me.GetTaxableSettingInfo
            Me._setteing_nontaxable = Me.GetNonTaxableSettingInfo
        End Sub

        Public Sub AddGridStyle()
            Dim grids As C1FlexGrid() = Me.Grids
            Dim i As Integer
            For i = 0 To grids.Length - 1
                grids(i).Styles.Add("data_row").Font = FinancialAffairsUtility.GetGridFontNormal
                Dim style As CellStyle = grids(i).Styles.Add("total_row")
                style.Font = FinancialAffairsUtility.GetGridFontBold
                style.BackColor = Color.Khaki
                style = grids(i).Styles.Add("readonly_col")
                style.DataType = GetType(String)
                style.TextAlign = TextAlignEnum.CenterCenter
                style = grids(i).Styles.Add("noedit_money_col")
                style.DataType = GetType(Long)
                style.TextAlign = TextAlignEnum.RightCenter
                style.Format = "N0"
            Next i
        End Sub

        Public Overridable Sub ChangedCondition()
            Utilities.SetEnabledProperty(False, Me.SelectChildControls(Of Button)(New GroupBox() {Me.TaxableGroup, Me.NonTaxableGroup}).ToArray)
        End Sub

        Protected Sub ClearComboItems(ByVal cmbTarget As ComboBox)
            cmbTarget.Items.Clear()
        End Sub

        Protected Overridable Sub ClearConditionCombo()
            Me.ClearComboItems(Me.YearCombo)
        End Sub

        Protected Function Convert2ReportData( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As DataTable

            Dim table2 As DataTable = Nothing

            Try
                Dim table As DataTable = Me.CreateReportMap(reportKind, isTaxable).CreateDataTablePhysName("dtDetail")
                Dim grid As C1FlexGrid = Me.GetGrid(isTaxable)
                Dim reportOutputColumnIndexes As Integer() = Me.GetReportOutputColumnIndexes(reportKind, isTaxable)
                Dim i As Integer
                For i = 1 To grid.Rows.Count - 1
                    Dim row As DataRow = table.NewRow
                    Dim j As Integer
                    For j = 0 To reportOutputColumnIndexes.Length - 2
                        row.Item(j) = grid.Item(i, reportOutputColumnIndexes(j + 1))
                    Next j
                    table.Rows.Add(row)
                Next i
                table2 = table
            Catch exception As AppUnionException
                Dim msg As New ExceptionMsg(exception)
                If msg.IsNotContinue Then
                    CLMsg.Show("GE0001")
                    Return Nothing
                End If
                msg.ShowMessage()
            Catch ex As Exception
                CLMsg.Show("GE0001")
            End Try
            Return table2

        End Function

        Protected Function CreateList(Of T)(ByVal ParamArray contents As T()) As List(Of T)
            Return New List(Of T)(contents)
        End Function

        Protected MustOverride Function CreateReportDetail(ByVal reportKind As ReportKind, ByVal isTaxable As Boolean) As DataTable
        Protected MustOverride Function CreateReportHeader(ByVal reportKind As ReportKind, ByVal isTaxable As Boolean) As DataTable
        Protected MustOverride Function CreateReportMap(ByVal reportKind As ReportKind, ByVal isTaxable As Boolean) As EntityMap

        Private Sub FormatGrid( _
            ByVal flxGrid As C1FlexGrid, _
            ByVal Settings As GridSettingInfo() _
        )
            If (Not Settings Is Nothing) Then
                FinancialAffairsUtility.ApplyGridStyle(flxGrid, Settings)
            End If
            If (flxGrid.Rows.Count > 1) Then
                Dim i As Integer
                For i = 1 To (flxGrid.Rows.Count - 1) - 1
                    flxGrid.Rows.Item(i).Style = flxGrid.Styles.Item("data_row")
                Next i
                flxGrid.Rows.Item((flxGrid.Rows.Count - 1)).Style = flxGrid.Styles.Item("total_row")
            End If
            flxGrid.Refresh()
        End Sub

        Private Function GetExistYearValueFromTable( _
            ByVal dTblExistYears As DataTable, _
            ByVal strCut As String, _
            ByVal strColName As String _
        ) As Integer

            Dim target As DataRow() = Me.SelectExistYear(dTblExistYears, strCut)
            If Not Me.IsEmptyArray(target) Then
                Return Convert.ToInt32(target(0).Item(strColName))
            End If
            Return PublicCommand.GetNow.Year

        End Function

        Protected Function GetFullName(Of T As Class)() As String
            Return GetType(T).FullName
        End Function

        Protected Function GetGrid(ByVal isTaxable As Boolean) As C1FlexGrid
            If Not isTaxable Then
                Return Me.NonTaxableGrid
            End If
            Return Me.TaxableGrid
        End Function

        Protected Function GetMaxYearValue( _
            ByVal dTblExistYears As DataTable, _
            ByVal strCut As String _
        ) As Integer

            Return Me.GetExistYearValueFromTable(dTblExistYears, strCut, "max")

        End Function

        Protected Function GetMinYearValue( _
            ByVal dTblExistYears As DataTable, _
            ByVal strCut As String _
        ) As Integer

            Return Me.GetExistYearValueFromTable(dTblExistYears, strCut, "min")

        End Function

        Public MustOverride Function GetNonTaxableDetailControl() As UserControl
        Protected MustOverride Function GetNonTaxableResultCaption() As String
        Protected MustOverride Function GetNonTaxableSettingInfo() As GridSettingInfo()

        Protected Overridable Function GetReportData( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
            ) As DataSet

            Dim set2 As DataSet

            Try
                Dim ds As New DataSet
                Dim table As DataTable = Me.CreateReportHeader(reportKind, isTaxable)
                ds.Tables.Add(table)
                Dim table2 As DataTable = Me.CreateReportDetail(reportKind, isTaxable)
                ds.Tables.Add(table2)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try

            Return set2

        End Function

        Protected MustOverride Function GetReportName( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As ReportClass

        Protected Overridable Function GetReportOutputColumnIndexes( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As Integer()

            Dim grid As C1FlexGrid = Me.GetGrid(isTaxable)
            Return Me.GetSimpleList(0, (grid.Cols.Count - 1)).ToArray

        End Function

        Protected Function GetSimpleList( _
            ByVal min As Integer, _
            ByVal max As Integer _
        ) As List(Of Integer)

            Dim list As New List(Of Integer)
            Dim i As Integer = min

            Do While (i <= max)
                list.Add(i)
                i += 1
            Loop

            Return list

        End Function

        Protected Function GetStringArray( _
            ByVal min As Integer, _
            ByVal max As Integer _
        ) As String()

            Dim list As List(Of Integer) = Me.GetSimpleList(min, max)
            Dim array As String() = New String(list.Count - 1) {}
            Dim index As Integer = 0

            For Each elem As Integer In list
                array(index) = elem
                index += 1
            Next

            Return array

        End Function

        'Protected Function GetStringArray(ByVal min As Integer, ByVal max As Integer) As String()
        '    Dim target As Integer = 0
        '    Dim converter As Converter(Of Integer, String) = target >= target.ToString
        '    Return Me.GetSimpleList(min, max).ConvertAll(Of String)(converter).ToArray
        'End Function

        Public MustOverride Function GetTaxableDetailControl() As UserControl
        Protected MustOverride Function GetTaxableResultCaption() As String
        Protected MustOverride Function GetTaxableSettingInfo() As GridSettingInfo()

        Protected Function GetYearsArrayFromTable( _
            ByVal dTblExistYears As DataTable, _
            ByVal strCut As String _
        ) As String()

            Dim stringArray As String()

            Try
                Dim minYearValue As Integer = Me.GetMinYearValue(dTblExistYears, strCut)
                Dim maxYearValue As Integer = Me.GetMaxYearValue(dTblExistYears, strCut)
                stringArray = Me.GetStringArray(minYearValue, maxYearValue)
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try

            Return stringArray

        End Function

        Protected Function HasRows( _
            ByVal target As DataTable _
        ) As Boolean

            Return target.CreateDataReader.HasRows

        End Function

        Public Overridable Sub InitButtons()
            Utilities.SetEnabledProperty(Me.CanRef, New Control() {Me.QueryBtn})
            Utilities.SetEnabledProperty(False, Me.SelectChildControls(Of Button)(New GroupBox() {Me.TaxableGroup, Me.NonTaxableGroup}).ToArray)
        End Sub

        Public Sub InitConditionCombo( _
            ByVal dSetExistYears As DataSet _
        )
            Me.ClearConditionCombo()
            Me.SetConditionComboItems(dSetExistYears)
        End Sub

        Protected Function IsEmptyArray( _
            ByVal target As Object() _
        ) As Boolean
            If Not Me.IsNull(target) Then
                Return (target.Length = 0)
            End If
            Return True
        End Function

        Private Function IsMatch( _
            ByVal target As String, _
            ByVal pattern As String _
        ) As Boolean
            Return Regex.IsMatch(target, pattern)
        End Function

        Protected Function IsNotNull( _
            ByVal target As Object _
        ) As Boolean
            Return Not Me.IsNull(target)
        End Function

        Private Function IsNotPrintBtn( _
            ByVal target As Button _
        ) As Boolean
            Return Not Me.IsPrintBtn(target)
        End Function

        Private Function IsNotReferenceBtn( _
            ByVal target As Button _
        ) As Boolean
            Return Not Me.IsReferenceBtn(target)
        End Function

        Protected Function IsNull( _
            ByVal target As Object _
        ) As Boolean
            Return (target Is Nothing)
        End Function

        Protected Function IsNullOrEmpty( _
            ByVal target As Object _
        ) As Boolean
            If Not Me.IsNull(target) Then
                Return (target.ToString.Length = 0)
            End If
            Return True
        End Function

        Private Function IsPrintBtn( _
            ByVal target As Button _
        ) As Boolean
            Return Me.IsMatch(target.Text, "印刷")
        End Function

        Private Function IsReferenceBtn( _
            ByVal target As Button _
        ) As Boolean
            Return Me.IsMatch(target.Text, "照会")
        End Function

        Public Sub PrintReport( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        )
            Dim viewer As New ReportViewer(Me.GetReportData(reportKind, isTaxable), Me.GetReportName(reportKind, isTaxable))
            viewer.ReportViewerShow()
        End Sub

        Public Overridable Sub QueryWithholding()
            Try
                Dim result As DataSet = Me.SearchWithholding
                Me.ShowResult(result)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Protected MustOverride Function SearchWithholding() As DataSet

        Private Function SelectButtons( _
            ByVal group As GroupBox, _
            ByVal remover As Predicate(Of Button) _
        ) As List(Of Button)
            Dim list As List(Of Button) = Me.SelectChildControls(Of Button)(New GroupBox() {group})
            list.RemoveAll(remover)
            Return list
        End Function

        Protected Function SelectChildControls(Of T As Control)(ByVal ParamArray containers As GroupBox()) As List(Of T)
            Dim list2 As List(Of T)
            Try
                Dim list As New List(Of T)
                Dim box As GroupBox
                For Each box In containers
                    Dim control As Control
                    For Each control In box.Controls
                        Dim target As T = TryCast(control, T)
                        If Me.IsNotNull(target) Then
                            list.Add(target)
                        End If
                    Next
                Next
                list2 = list
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return list2
        End Function

        Protected Function SelectExistYear( _
            ByVal dTblExistYears As DataTable, _
            ByVal strCut As String _
        ) As DataRow()

            Return dTblExistYears.Select(("k_daily_pay_kind = '" & strCut & "'"))

        End Function

        Protected MustOverride Sub SetConditionComboItems( _
            ByVal dSetExistYears As DataSet _
        )

        Protected Sub SetDefaultYear( _
            ByVal cmbTarget As ComboBox _
        )
            cmbTarget.Text = PublicCommand.GetNow.Year.ToString
            If Me.IsNullOrEmpty(cmbTarget.Text) Then
                cmbTarget.SelectedIndex = (cmbTarget.Items.Count - 1)
            End If
        End Sub

        Protected Sub SetMonthComboItems( _
            ByVal cmbTarget As ComboBox _
        )
            Me.ClearComboItems(cmbTarget)
            cmbTarget.Items.AddRange(UnionConst.MONTH_RANGE)
        End Sub

        Protected Overridable Sub ShowResult( _
            ByVal result As DataSet _
        )
            Me.TaxableGroup.Text = Me.GetTaxableResultCaption
            Me.TaxableGrid.DataSource = result.Tables.Item(0)
            Me.FormatGrid(Me.TaxableGrid, Me._setteing_taxable)
            Me.TaxableReferenceBtn.Enabled = (Me.HasRows(result.Tables.Item(0)) AndAlso Me.CanRef)
            Utilities.SetEnabledProperty((Me.HasRows(result.Tables.Item(0)) AndAlso Me.CanPrint), Me.TaxablePrintBtn)
            Me.NonTaxableGroup.Text = Me.GetNonTaxableResultCaption
            Me.NonTaxableGrid.DataSource = result.Tables.Item(1)
            Me.FormatGrid(Me.NonTaxableGrid, Me._setteing_nontaxable)
            Me.NonTaxableReferenceBtn.Enabled = (Me.HasRows(result.Tables.Item(1)) AndAlso Me.CanRef)
            Utilities.SetEnabledProperty((Me.HasRows(result.Tables.Item(1)) AndAlso Me.CanPrint), Me.NonTaxablePrintBtn)
        End Sub

        ' Properties
        Protected Overridable ReadOnly Property Business() As WithholdingCommand
            Get
                Return Me.WithHoldingList.GetBusObj(New Object(0 - 1) {})
            End Get
        End Property

        Protected ReadOnly Property CanEntry() As Boolean
            Get
                'Return MDFinanceCommon.GetEntryPower(Me._ctlList.Name)
                Return MDFinanceCommon.GetEntryPower("UC050301")
            End Get
        End Property

        Protected ReadOnly Property CanPrint() As Boolean
            Get
                'Return MDFinanceCommon.GetPrintPower(Me._ctlList.Name)
                Return MDFinanceCommon.GetPrintPower("UC050301")
            End Get
        End Property

        Protected ReadOnly Property CanRef() As Boolean
            Get
                'Return MDFinanceCommon.GetReferencePower(Me._ctlList.Name)
                Return MDFinanceCommon.GetReferencePower("UC050301")
            End Get
        End Property

        Public MustOverride ReadOnly Property FUNCTION_NAME_NONTAXABLE() As String
        Public MustOverride ReadOnly Property FUNCTION_NAME_TAXABLE() As String

        Private ReadOnly Property Grids() As C1FlexGrid()
            Get
                Return Me.CreateList(Of C1FlexGrid)(New C1FlexGrid() {Me.TaxableGrid, Me.NonTaxableGrid}).ToArray
            End Get
        End Property

        Private ReadOnly Property NonTaxableGrid() As C1FlexGrid
            Get
                Return Me.SelectChildControls(Of C1FlexGrid)(New GroupBox() {Me.NonTaxableGroup}).Item(0)
            End Get
        End Property

        Protected MustOverride ReadOnly Property NonTaxableGroup() As GroupBox

        Private ReadOnly Property NonTaxablePrintBtn() As Button()
            Get
                Return Me.SelectButtons(Me.NonTaxableGroup, New Predicate(Of Button)(AddressOf Me.IsNotPrintBtn)).ToArray
            End Get
        End Property

        Private ReadOnly Property NonTaxableReferenceBtn() As Button
            Get
                Return Me.SelectButtons(Me.NonTaxableGroup, New Predicate(Of Button)(AddressOf Me.IsNotReferenceBtn)).Item(0)
            End Get
        End Property

        Protected MustOverride ReadOnly Property QueryBtn() As Button

        Private ReadOnly Property TaxableBtn() As List(Of Button)
            Get
                Return Me.SelectChildControls(Of Button)(New GroupBox() {Me.TaxableGroup})
            End Get
        End Property

        Private ReadOnly Property TaxableGrid() As C1FlexGrid
            Get
                Return Me.SelectChildControls(Of C1FlexGrid)(New GroupBox() {Me.TaxableGroup}).Item(0)
            End Get
        End Property

        Protected MustOverride ReadOnly Property TaxableGroup() As GroupBox

        Private ReadOnly Property TaxablePrintBtn() As Button()
            Get
                Return Me.SelectButtons(Me.TaxableGroup, New Predicate(Of Button)(AddressOf Me.IsNotPrintBtn)).ToArray
            End Get
        End Property

        Private ReadOnly Property TaxableReferenceBtn() As Button
            Get
                Return Me.SelectButtons(Me.TaxableGroup, New Predicate(Of Button)(AddressOf Me.IsNotReferenceBtn)).Item(0)
            End Get
        End Property

        Protected ReadOnly Property WithHoldingList() As UC050301
            Get
                Return Me._ctlList
            End Get
        End Property

        Protected MustOverride ReadOnly Property YearCombo() As ComboBox

        ' Fields
        Private _ctlList As UC050301
        Protected ReadOnly _setteing_nontaxable As GridSettingInfo()
        Protected ReadOnly _setteing_taxable As GridSettingInfo()
        Protected Const GROUP_CAPTION_NON_TAXABLE As String = "課税非対象者"
        Protected Const GROUP_CAPTION_TAXABLE As String = "課税対象者"
    End Class

    Private MustInherit Class CutDivControlActer
        Inherits ControlActManager
        ' Methods
        Public Sub New(ByVal UC050301 As UC050301)
            MyBase.New(UC050301)
        End Sub

        Public Overrides Sub ChangedCondition()
            MyBase.ChangedCondition()
            Me.ReCalcBtn.Enabled = False
        End Sub

        Protected Overrides Sub ClearConditionCombo()
            MyBase.ClearConditionCombo()
            MyBase.ClearComboItems(Me.MonthCombo)
        End Sub

        Public Sub ClickRecalc()
            Try
                FinancialAffairsUtility.CheckNetBankDataHasMade((Me.YearCombo.Text & Me.MonthCombo.Text), Me.CutDiv)
                If (CLMsg.Show("GQ0013", Me.YearCombo.Text, Me.MonthCombo.Text) <> DialogResult.No) Then
                    Me.ExecuteCalcuration(Me.YearCombo.Text, Me.MonthCombo.Text)
                    Me.QueryWithholding()
                End If
            Catch exception As BaseUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Private Function CreateDetailObject( _
            ByVal strControlName As String _
        ) As UserControl

            ' MOD 2012/06/24
            If strControlName.EndsWith("CtlOnceTaxableDetail") _
            Or strControlName.EndsWith("CtlOnceNonTaxableDetail") Then
                Return TryCast( _
                    Utilities.CreateObject( _
                        strControlName, _
                        New Object() { _
                            Me.YearCombo.Text, _
                            Me.MonthCombo.Text, _
                            MyBase.WithHoldingList.Name, _
                            New EventHandler(AddressOf MyBase.WithHoldingList.CtlWithHoldingDetail_Cancel), _
                            WithHoldingList.cmbCutOnceName.Text _
                        } _
                    ),  _
                    UserControl _
                )
            Else
                Return TryCast( _
                    Utilities.CreateObject( _
                        strControlName, _
                        New Object() { _
                            Me.YearCombo.Text, _
                            Me.MonthCombo.Text, _
                            MyBase.WithHoldingList.Name, _
                            New EventHandler(AddressOf MyBase.WithHoldingList.CtlWithHoldingDetail_Cancel) _
                        } _
                    ),  _
                    UserControl _
                )
            End If
            ' MOD 2012/06/24

        End Function

        Protected Overrides Function CreateReportDetail( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As DataTable

            Select Case reportKind
                Case reportKind.SUM
                    Return MyBase.Convert2ReportData( _
                        reportKind, _
                        isTaxable _
                    )
                Case reportKind.LIST
                    If Not isTaxable Then
                        Exit Select
                    End If
                    ' MOD 2012/06/15
                    Return Me.Business.GetMonthlyReportListData( _
                        Me.YearCombo.Text, _
                        Me.MonthCombo.Text, _
                        WithHoldingList.cmbCutOnceName.Text _
                    )
            End Select
            Return Nothing

        End Function

        Protected Overrides Function CreateReportHeader( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As DataTable

            Return FinancialAffairsUtility.CreateWithholdingReportHeader( _
                Me.YearCombo.Text, _
                Me.MonthCombo.Text _
            )

        End Function

        Protected Overrides Function CreateReportMap( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As EntityMap

            If isTaxable Then
                Select Case reportKind
                    Case reportKind.SUM
                        Return New WithholdingMonthlyTaxableSumListMap
                    Case reportKind.LIST
                        Return New WithholdingMonthlyTaxableReportListMap
                End Select
            Else
                Select Case reportKind
                    Case reportKind.SUM
                        Return New WithholdingMonthlyNonTaxableSumListMap
                    Case reportKind.LIST
                        Return New WithholdingMonthlyNonTaxableReportListMap
                End Select
            End If
            Return Nothing

        End Function

        Private Function ExecuteCalcuration( _
            ByVal TargetYear As String, _
            ByVal TargetMonth As String _
        ) As Integer

            Dim num As Integer

            Try
                MyBase.WithHoldingList.Cursor = Cursors.WaitCursor
                num = Me.Business.ExecuteCalcWithholding( _
                    TargetYear, _
                    TargetMonth, _
                    MDLoginInfo.UserId _
                )
            Catch exception As Exception
                MyBase.WithHoldingList.Cursor = Cursors.Default
                If TypeOf exception Is SysUnionException Then
                    DirectCast(exception, SysUnionException).AddMethodName(MethodBase.GetCurrentMethod)
                    Throw exception
                End If
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            Finally
                MyBase.WithHoldingList.Cursor = Cursors.Default
            End Try
            Return num

        End Function

        Public Sub FrmSelectCalcMonth_Execute( _
            ByVal sender As Object, _
            ByVal e As SelectMonthEventArgs _
        )
            Try
                If Me.Business.IsWithholdingExists(e.Year, e.Month) Then
                    CLMsg.Show("GE0044", e.Year, e.Month)
                Else
                    DirectCast(sender, Form).Dispose()
                    If (Me.ExecuteCalcuration(e.Year, e.Month) = 0) Then
                        CLMsg.Show("GI0008", e.Year, e.Month)
                    Else
                        MyBase.InitConditionCombo(Me.Business.GetExistYear)
                        Me.YearCombo.Text = e.Year
                        Me.MonthCombo.Text = e.Month
                        Me.QueryWithholding()
                    End If
                End If
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Private Function GetGroupYearMonthCaption() As String
            Return String.Concat(New String() {"（", Me.YearCombo.Text, "年", Me.MonthCombo.Text, "月分）"})
        End Function

        Public Overrides Function GetNonTaxableDetailControl() As UserControl
            Return Me.CreateDetailObject(Me.NonTaxableDetailControlName)
        End Function

        Protected Overrides Function GetNonTaxableResultCaption() As String
            Return ("課税非対象者" & Me.GetGroupYearMonthCaption)
        End Function

        Protected Overrides Function GetReportData( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As DataSet
            If (Not isTaxable AndAlso reportKind.Equals(reportKind.LIST)) Then
                Return Me.Business.GetMonthlyNonTaxableListReportData(Me.YearCombo.Text, Me.MonthCombo.Text, WithHoldingList.cmbCutOnceName.Text) ' MOD 2012/06/15
            End If
            Return MyBase.GetReportData(reportKind, isTaxable)
        End Function

        Public Overrides Function GetTaxableDetailControl() As UserControl
            Return Me.CreateDetailObject(Me.TaxableDetailControlName)
        End Function

        Protected Overrides Function GetTaxableResultCaption() As String
            Return ("課税対象者" & Me.GetGroupYearMonthCaption)
        End Function

        Public Overrides Sub InitButtons()
            MyBase.InitButtons()
            Me.InitCalculationButtons()
        End Sub

        Private Sub InitCalculationButtons()
            Me.ReCalcBtn.Enabled = False
            Me.NewCalcBtn.Enabled = MyBase.CanEntry
        End Sub

        Protected Overrides Function SearchWithholding() As DataSet
            ' MOD 2012/06/24
            If Me.TaxableDetailControlName.EndsWith("CtlOnceTaxableDetail") _
            Or Me.TaxableDetailControlName.EndsWith("CtlOnceNonTaxableDetail") Then
                ' MOD 2012/06/15
                Return Me.Business.QueryMonthlySummary( _
                    Me.YearCombo.Text, _
                    Me.MonthCombo.Text, _
                    WithHoldingList.cmbCutOnceName.Text _
                )
            Else
                Return Me.Business.QueryMonthlySummary( _
                    Me.YearCombo.Text, _
                    Me.MonthCombo.Text, _
                    "" _
                )
            End If
            ' MOD 2012/06/24
        End Function

        Protected Overrides Sub SetConditionComboItems( _
            ByVal dSetExistYears As DataSet _
        )
            Dim yearsArrayFromTable As String() = MyBase.GetYearsArrayFromTable(dSetExistYears.Tables.Item(0), Me.CutDiv)
            Me.YearCombo.Items.AddRange(yearsArrayFromTable)
            MyBase.SetMonthComboItems(Me.MonthCombo)
            MyBase.SetDefaultYear(Me.YearCombo)
            Me.SetDefaultMonth(Me.MonthCombo)
            'Utilities.SetCanEditToControl(False, Me.YearCombo)
        End Sub

        Private Sub SetDefaultMonth( _
            ByVal cmbMonth As ComboBox _
        )
            cmbMonth.Text = PublicCommand.GetNow.ToString("MM")
        End Sub

        Protected Overrides Sub ShowResult( _
            ByVal result As DataSet _
        )
            Try
                MyBase.ShowResult(result)
                Dim flag As Boolean = False
                Dim table As DataTable
                For Each table In result.Tables
                    If MyBase.HasRows(table) Then
                        flag = True
                        Exit For
                    End If
                Next
                Dim flag2 As Boolean = True
                If flag Then
                    flag2 = Me.Business.IsGreaterThanExists(Me.YearCombo.Text, Me.MonthCombo.Text)
                End If
                Me.ReCalcBtn.Enabled = (Not flag2 AndAlso MyBase.CanEntry)
            Catch exception As BaseUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        ' Properties
        Protected Overrides ReadOnly Property Business() As WithholdingCommand
            Get
                Return MyBase.WithHoldingList.GetBusObj(New Object() {Me.CutDiv})
            End Get
        End Property

        Protected MustOverride ReadOnly Property CutDiv() As String
        Protected MustOverride ReadOnly Property MonthCombo() As ComboBox
        Protected MustOverride ReadOnly Property NewCalcBtn() As Button
        Protected MustOverride ReadOnly Property NonTaxableDetailControlName() As String
        Protected MustOverride ReadOnly Property ReCalcBtn() As Button
        Protected MustOverride ReadOnly Property TaxableDetailControlName() As String

    End Class

    Private Class MonthlyControlActer
        Inherits CutDivControlActer
        ' Methods
        Public Sub New(ByVal UC050301 As UC050301)
            MyBase.New(UC050301)
        End Sub

        Protected Overrides Function GetNonTaxableSettingInfo() As GridSettingInfo()

            Return New GridSettingInfo() { _
                New GridSettingInfo(0, "readonly_col", False, False, False, False, False), _
                New GridSettingInfo(80, "readonly_col", False, False, False, False, True), _
                New GridSettingInfo(140, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(0, "noedit_money_col", False, False, True, False, False), _
                New GridSettingInfo(0, "noedit_money_col", False, False, True, False, False), _
                New GridSettingInfo(90, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(130, "noedit_money_col", False, False, True, False, True) _
            }

        End Function

        Protected Overrides Function GetReportName( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As ReportClass

            If isTaxable Then
                Select Case reportKind
                    Case reportKind.SUM
                        'Return "Report.Withholding.RptWithholdingTable_sum_cut_monthly"
                        Return New CR0503P5
                    Case reportKind.LIST
                        'Return "Report.Withholding.RptWithholdingTable_local_cut_monthly"
                        Return New CR0503P4
                End Select
            Else
                Select Case reportKind
                    Case reportKind.SUM
                        'Return "Report.Withholding.RptTaxNoIntendedPayTable_sum_cut_monthly"
                        Return New CR0503PE
                    Case reportKind.LIST
                        'Return "Report.Withholding.RptTaxNoIntendedPayTable_local_cut_monthly"
                        Return New CR0503PD
                End Select
            End If
            Return Nothing

        End Function

        Protected Overrides Function GetTaxableSettingInfo() As GridSettingInfo()

            Return New GridSettingInfo() { _
                New GridSettingInfo(0, "readonly_col", False, False, False, False, False), _
                New GridSettingInfo(80, "readonly_col", False, False, False, False, True), _
                New GridSettingInfo(110, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(140, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(0, "noedit_money_col", False, False, True, False, False), _
                New GridSettingInfo(140, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(90, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(130, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(130, "noedit_money_col", False, False, True, False, True) _
            }

        End Function

        ' Properties
        Protected Overrides ReadOnly Property CutDiv() As String
            Get
                'Return "04"
                Return "05"
            End Get
        End Property

        Public Overrides ReadOnly Property FUNCTION_NAME_NONTAXABLE() As String
            Get
                Return "源泉徴収" & " - " & "課税非対象者月例賃金"
            End Get
        End Property

        Public Overrides ReadOnly Property FUNCTION_NAME_TAXABLE() As String
            Get
                Return "源泉徴収" & " - " & "課税対象者月例賃金"
            End Get
        End Property

        Protected Overrides ReadOnly Property MonthCombo() As ComboBox
            Get
                Return MyBase.WithHoldingList.cmbMonthlyMonth
            End Get
        End Property

        Protected Overrides ReadOnly Property NewCalcBtn() As Button
            Get
                Return MyBase.WithHoldingList.btnNewCalc
            End Get
        End Property

        Protected Overrides ReadOnly Property NonTaxableDetailControlName() As String
            Get
                Return MyBase.GetFullName(Of CtlMonthlyNonTaxableDetail)()
            End Get
        End Property

        Protected Overrides ReadOnly Property NonTaxableGroup() As GroupBox
            Get
                Return MyBase.WithHoldingList.grpResultNonTaxable
            End Get
        End Property

        Protected Overrides ReadOnly Property QueryBtn() As Button
            Get
                Return MyBase.WithHoldingList.btnMonthlyQuery
            End Get
        End Property

        Protected Overrides ReadOnly Property ReCalcBtn() As Button
            Get
                Return MyBase.WithHoldingList.btnReCalc
            End Get
        End Property

        Protected Overrides ReadOnly Property TaxableDetailControlName() As String
            Get
                Return MyBase.GetFullName(Of CtlMonthlyTaxableDetail)()
            End Get
        End Property

        Protected Overrides ReadOnly Property TaxableGroup() As GroupBox
            Get
                Return MyBase.WithHoldingList.grpResultTaxable
            End Get
        End Property

        Protected Overrides ReadOnly Property YearCombo() As ComboBox
            Get
                Return MyBase.WithHoldingList.cmbMonthlyYear
            End Get
        End Property

        ' Fields
        Private Const FUNCTION_NAME_NOWITHHOLDING As String = "源泉徴収" & " - " & "課税非対象者月例賃金"
        Private Const FUNCTION_NAME_WITHHOLDING As String = "源泉徴収" & " - " & "課税対象者月例賃金"
    End Class

    Private Class OnceControlActer
        Inherits CutDivControlActer
        ' Methods
        Public Sub New(ByVal UC050301 As UC050301)

            MyBase.New(UC050301)
            ' ADD 2012/06/15
            Dim clsMdb As New CLAccessMdb
            Dim table As DataTable
            Dim strSql As String = ""

            ' DB接続
            clsMdb.Connect()

            ' SQL文作成
            strSql = ""
            strSql += "SELECT DISTINCT c_pay_once_name" & vbCrLf
            strSql += "  FROM (" & vbCrLf
            strSql += "        SELECT c_pay_once_name" & vbCrLf
            strSql += "          FROM pay_strike_cut_once" & vbCrLf
            strSql += "        UNION" & vbCrLf
            strSql += "        SELECT c_pay_once_name" & vbCrLf
            strSql += "          FROM pay_time_cut_once" & vbCrLf
            strSql += "       ) AS CUT" & vbCrLf

            ' SQL実行
            table = clsMdb.ExecuteSql(strSql)
            'table = clsMdb.ExecuteSql("SELECT DISTINCT c_pay_once_name FROM (SELECT c_pay_once_name FROM pay_strike_cut_once UNION SELECT c_pay_once_name FROM pay_time_cut_once)  AS CUT")
            If table.Rows.Count > 0 Then
                If table.Rows(0).Item(0) Is DBNull.Value Then
                ElseIf Trim(table.Rows(0).Item(0)) <> "" Then
                    table.Rows.InsertAt(table.NewRow, 0)
                End If
            End If

            UC050301.cmbCutOnceName.DataSource = table
            UC050301.cmbCutOnceName.DisplayMember = "c_pay_once_name"
            UC050301.cmbCutOnceName.Text = ""

            ' DB切断
            clsMdb.Disconnect()
            ' ADD 2012/06/15
        End Sub

        Protected Overrides Function GetNonTaxableSettingInfo() As GridSettingInfo()

            Return New GridSettingInfo() { _
                New GridSettingInfo(0, "readonly_col", False, False, False, False, False), _
                New GridSettingInfo(80, "readonly_col", False, False, False, False, True), _
                New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), _
                New GridSettingInfo(140, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), _
                New GridSettingInfo(90, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(130, "noedit_money_col", False, False, True, False, True) _
            }

        End Function

        Protected Overrides Function GetReportName( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As ReportClass

            If isTaxable Then
                Select Case reportKind
                    Case reportKind.SUM
                        'Return "Report.Withholding.RptWithholdingTable_sum_cut_once"
                        Return New CR0503P8
                    Case reportKind.LIST
                        'Return "Report.Withholding.RptWithholdingTable_local_cut_once"
                        Return New CR0503P7
                End Select
            Else
                Select Case reportKind
                    Case reportKind.SUM
                        'Return "Report.Withholding.RptTaxNoIntendedPayTable_sum_cut_once"
                        Return New CR0503PG
                    Case reportKind.LIST
                        'Return "Report.Withholding.RptTaxNoIntendedPayTable_local_cut_once"
                        Return New CR0503PF
                End Select
            End If
            Return Nothing

        End Function

        Protected Overrides Function GetTaxableSettingInfo() As GridSettingInfo()

            Return New GridSettingInfo() { _
                New GridSettingInfo(0, "readonly_col", False, False, False, False, False), _
                New GridSettingInfo(80, "readonly_col", False, False, False, False, True), _
                New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), _
                New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), _
                New GridSettingInfo(140, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(0, "noedit_money_col", False, False, False, False, False), _
                New GridSettingInfo(90, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(130, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(130, "noedit_money_col", False, False, True, False, True) _
            }

        End Function

        ' Properties
        Protected Overrides ReadOnly Property CutDiv() As String
            Get
                'Return "05"
                Return "06"
            End Get
        End Property

        Public Overrides ReadOnly Property FUNCTION_NAME_NONTAXABLE() As String
            Get
                Return "源泉徴収" & " - " & "課税非対象者一時金"
            End Get
        End Property

        Public Overrides ReadOnly Property FUNCTION_NAME_TAXABLE() As String
            Get
                Return "源泉徴収" & " - " & "課税対象者一時金"
            End Get
        End Property

        Protected Overrides ReadOnly Property MonthCombo() As ComboBox
            Get
                Return MyBase.WithHoldingList.cmbOnceMonth
            End Get
        End Property

        Protected Overrides ReadOnly Property NewCalcBtn() As Button
            Get
                Return MyBase.WithHoldingList.btnOnceNewCalc
            End Get
        End Property

        Protected Overrides ReadOnly Property NonTaxableDetailControlName() As String
            Get
                Return MyBase.GetFullName(Of CtlOnceNonTaxableDetail)()
            End Get
        End Property

        Protected Overrides ReadOnly Property NonTaxableGroup() As GroupBox
            Get
                Return MyBase.WithHoldingList.grpOnceResultNonTaxable
            End Get
        End Property

        Protected Overrides ReadOnly Property QueryBtn() As Button
            Get
                Return MyBase.WithHoldingList.btnOnceQuery
            End Get
        End Property

        Protected Overrides ReadOnly Property ReCalcBtn() As Button
            Get
                Return MyBase.WithHoldingList.btnOnceReCalc
            End Get
        End Property

        Protected Overrides ReadOnly Property TaxableDetailControlName() As String
            Get
                Return MyBase.GetFullName(Of CtlOnceTaxableDetail)()
            End Get
        End Property

        Protected Overrides ReadOnly Property TaxableGroup() As GroupBox
            Get
                Return MyBase.WithHoldingList.grpOnceResultTaxable
            End Get
        End Property

        Protected Overrides ReadOnly Property YearCombo() As ComboBox
            Get
                Return MyBase.WithHoldingList.cmbOnceYear
            End Get
        End Property

        ' Fields
        Private Const FUNCTION_NAME_ONCE_NONTAXABLE As String = "源泉徴収" & " - " & "課税非対象者一時金"
        Private Const FUNCTION_NAME_ONCE_TAXABLE As String = "源泉徴収" & " - " & "課税対象者一時金"
    End Class

    Private Enum ReportKind
        ' Fields
        LIST = 1
        SUM = 0
    End Enum

    Private Class SumUpControlActer
        Inherits ControlActManager
        ' Methods
        Public Sub New(ByVal UC050301 As UC050301)
            MyBase.New(UC050301)
        End Sub

        Protected Overrides Function CreateReportDetail( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As DataTable

            Select Case reportKind
                Case reportKind.SUM
                    Return MyBase.Convert2ReportData( _
                        reportKind, _
                        isTaxable _
                    )
                Case reportKind.LIST
                    If Not isTaxable Then
                        Return Me.Business.GetSumUpNonTaxableListReportData( _
                            Me.YearCombo.Text _
                        )
                    End If
                    Return Me.Business.GetSumUpTaxableListReportData( _
                        Me.YearCombo.Text _
                    )
            End Select
            Return Nothing

        End Function

        Protected Overrides Function CreateReportHeader( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As DataTable

            Dim table As New DataTable("dtHeader")
            table.Columns.Add("year", GetType(String))
            Dim row As DataRow = table.NewRow
            row.Item("year") = Me.YearCombo.Text
            table.Rows.Add(row)
            Return table

        End Function

        Protected Overrides Function CreateReportMap( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As EntityMap

            If isTaxable Then
                Select Case reportKind
                    Case reportKind.SUM
                        Return New WithholdingSumUpTaxableSumReportMap
                    Case reportKind.LIST
                        Return New WithholdingSumUpTaxableListReportMap
                End Select
            Else
                Select Case reportKind
                    Case reportKind.SUM
                        Return New WithholdingSumUpNonTaxableSumReportMap
                    Case reportKind.LIST
                        Return New WithholdingSumUpNonTaxableListReportMap
                End Select
            End If
            Return Nothing

        End Function

        Private Function CreateSumupDetailObject(Of TypeOfDetail As UserControl)() As UserControl

            Return TryCast( _
                Utilities.CreateObject( _
                    MyBase.GetFullName(Of TypeOfDetail), _
                        New Object() { _
                            Me.YearCombo.Text, _
                            MyBase.WithHoldingList.Name, _
                            New EventHandler(AddressOf MyBase.WithHoldingList.CtlWithHoldingDetail_Cancel) _
                        } _
                    ),  _
                    UserControl _
                )

        End Function

        Public Overrides Function GetNonTaxableDetailControl() As UserControl
            Return Me.CreateSumupDetailObject(Of CtlSumUpNonTaxableDetail)()
        End Function

        Protected Overrides Function GetNonTaxableResultCaption() As String
            Return ("課税非対象者（" & Me.YearCombo.Text & "年分）")
        End Function

        Protected Overrides Function GetNonTaxableSettingInfo() As GridSettingInfo()

            Return New GridSettingInfo() { _
                New GridSettingInfo(0, "readonly_col", False, False, False, False, False), _
                New GridSettingInfo(80, "readonly_col", False, False, False, False, True), _
                New GridSettingInfo(160, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(160, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(160, "noedit_money_col", False, False, True, False, True) _
            }

        End Function

        Protected Overrides Function GetReportName( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As ReportClass

            If isTaxable Then
                Select Case reportKind
                    Case reportKind.SUM
                        'Return "Report.Withholding.RptWithholdingCumulativeTable_sum"
                        Return New CR0503PC
                    Case reportKind.LIST
                        'Return "Report.Withholding.RptWithholdingCumulativeTable_local"
                        Return New CR0503PB
                End Select
            Else
                Select Case reportKind
                    Case reportKind.SUM
                        'Return "Report.Withholding.RptTaxNoIntendedPayCumulativeTable_sum"
                        Return New CR0503PI
                    Case reportKind.LIST
                        'Return "Report.Withholding.RptTaxNoIntendedPayCumulativeTable_local"
                        Return New CR0503PH
                End Select
            End If
            Return Nothing

        End Function

        Protected Overrides Function GetReportOutputColumnIndexes( _
            ByVal reportKind As ReportKind, _
            ByVal isTaxable As Boolean _
        ) As Integer()

            If isTaxable Then
                If (reportKind = reportKind.SUM) Then
                    Return New Integer() {0, 1, 2, 3, 4, 6}
                End If
            ElseIf (reportKind = reportKind.SUM) Then
                'Return MyBase.GetSimpleList(0, (Me.CreateReportMap(reportKind, isTaxable).ColumnCount - 1)).ToArray
                Return New Integer() {0, 1, 2, 3}
            End If
            Return Nothing

        End Function

        Public Overrides Function GetTaxableDetailControl() As UserControl
            Return Me.CreateSumupDetailObject(Of CtlSumUpTaxableDetail)()
        End Function

        Protected Overrides Function GetTaxableResultCaption() As String
            Return ("課税対象者（" & Me.YearCombo.Text & "年分）")
        End Function

        Protected Overrides Function GetTaxableSettingInfo() As GridSettingInfo()

            Return New GridSettingInfo() { _
                New GridSettingInfo(0, "readonly_col", False, False, False, False, False), _
                New GridSettingInfo(80, "readonly_col", False, False, False, False, True), _
                New GridSettingInfo(110, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True), _
                New GridSettingInfo(150, "noedit_money_col", False, False, True, False, True) _
            }

        End Function

        Protected Overrides Function SearchWithholding() As DataSet
            Return Me.Business.QuerySumUpSummary(Me.YearCombo.Text)
        End Function

        Protected Overrides Sub SetConditionComboItems( _
            ByVal dSetExistYears As DataSet _
        )

            Dim list As New List(Of Integer)
            Dim list2 As New List(Of Integer)
            Dim row As DataRow

            For Each row In dSetExistYears.Tables.Item(0).Rows
                list.Add(Convert.ToInt32(row.Item("min")))
                list2.Add(Convert.ToInt32(row.Item("max")))
            Next
            If list.Count > 0 And list2.Count > 0 Then
                list.Sort()
                list2.Sort()
                Dim stringArray As String() = MyBase.GetStringArray(list.Item(0), list2.Item((list2.Count - 1)))
                Me.YearCombo.Items.AddRange(stringArray)
                MyBase.SetDefaultYear(Me.YearCombo)
            End If

        End Sub

        ' Properties
        Public Overrides ReadOnly Property FUNCTION_NAME_NONTAXABLE() As String
            Get
                Return "源泉徴収" & " - " & "課税非対象者累計"
            End Get
        End Property

        Public Overrides ReadOnly Property FUNCTION_NAME_TAXABLE() As String
            Get
                Return "源泉徴収" & " - " & "課税対象者累計"
            End Get
        End Property

        Protected Overrides ReadOnly Property NonTaxableGroup() As GroupBox
            Get
                Return MyBase.WithHoldingList.grpSumUpNonTaxable
            End Get
        End Property

        Protected Overrides ReadOnly Property QueryBtn() As Button
            Get
                Return MyBase.WithHoldingList.btnSumUpQuery
            End Get
        End Property

        Protected Overrides ReadOnly Property TaxableGroup() As GroupBox
            Get
                Return MyBase.WithHoldingList.grpSumUpTaxable
            End Get
        End Property

        Protected Overrides ReadOnly Property YearCombo() As ComboBox
            Get
                Return MyBase.WithHoldingList.cmbSumUpYear
            End Get
        End Property

        ' Fields
        Private Const FUNCTION_NAME_SUMUP_NONTAXABLE As String = "源泉徴収" & " - " & "課税非対象者累計"
        Private Const FUNCTION_NAME_SUMUP_TAXABLE As String = "源泉徴収" & " - " & "課税対象者累計"
    End Class

    '' 月例タブ-課税対象者照会ボタン
    'Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
    '    Dim pn As Panel
    '    Dim uc As Control

    '    Me.Visible = False

    '    pn = ParentForm.Controls("Panel2")
    '    uc = pn.Controls("UC050302")

    '    If uc Is Nothing Then
    '        uc = New UC050302
    '        uc.Controls("label11").Text = "源泉徴収 - 課税対象者"
    '        Call pn.Controls.Add(uc)
    '    Else
    '        uc.Visible = True
    '    End If
    'End Sub

    '' 月例賃金集計タブ-課税非対象者照会ボタン
    'Private Sub Button16_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button16.Click
    '    Dim pn As Panel
    '    Dim uc As Control

    '    Me.Visible = False

    '    pn = ParentForm.Controls("Panel2")
    '    uc = pn.Controls("UC050302")

    '    If uc Is Nothing Then
    '        uc = New UC050302
    '        uc.Controls("label11").Text = "源泉徴収 - 課税非対象者"
    '        uc.Controls("TextBox1").Visible = False
    '        uc.Controls("TextBox2").Visible = False
    '        uc.Controls("TextBox3").Visible = False
    '        Call pn.Controls.Add(uc)
    '    Else
    '        uc.Visible = True
    '    End If
    'End Sub

    ''月例賃金集計タブ-新規集計ボタン
    'Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
    '    Dim cForm1 As New FM050303()
    '    ' モーダルで表示する
    '    cForm1.ShowDialog()

    '    ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
    '    cForm1.Dispose()
    'End Sub

    ''一時金タブ-新規集計ボタン
    'Private Sub Button9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button9.Click
    '    Dim cForm1 As New FM050303()
    '    ' モーダルで表示する
    '    cForm1.ShowDialog()

    '    ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
    '    cForm1.Dispose()
    'End Sub

    ''一時金集計タブ-課税対象者照会ボタン
    'Private Sub Button12_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button12.Click
    '    Dim pn As Panel
    '    Dim uc As Control

    '    Me.Visible = False

    '    pn = ParentForm.Controls("Panel2")
    '    uc = pn.Controls("UC050304")

    '    If uc Is Nothing Then
    '        uc = New UC050304
    '        uc.Controls("label11").Text = "源泉徴収 - 課税対象者一時金"
    '        Call pn.Controls.Add(uc)
    '    Else
    '        uc.Visible = True
    '    End If
    'End Sub
    ''一時金集計タブ-課税非対象者照会ボタン
    'Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
    '    Dim pn As Panel
    '    Dim uc As Control

    '    Me.Visible = False

    '    pn = ParentForm.Controls("Panel2")
    '    uc = pn.Controls("UC050304")

    '    If uc Is Nothing Then
    '        uc = New UC050304
    '        uc.Controls("label11").Text = "源泉徴収 - 課税非対象者一時金"
    '        uc.Controls("TextBox1").Visible = False
    '        Call pn.Controls.Add(uc)
    '    Else
    '        uc.Visible = True
    '    End If
    'End Sub

    ''累計タブ-課税対象者照会ボタン
    'Private Sub Button26_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button26.Click
    '    Dim pn As Panel
    '    Dim uc As Control

    '    Me.Visible = False

    '    pn = ParentForm.Controls("Panel2")
    '    uc = pn.Controls("UC050302")

    '    If uc Is Nothing Then
    '        uc = New UC050302
    '        uc.Controls("label11").Text = "源泉徴収 - 課税対象者累計"
    '        uc.Controls("TextBox1").Visible = False
    '        uc.Controls("Label7").Visible = False
    '        uc.Controls("TextBox11").Visible = False
    '        uc.Controls("Button5").Visible = False
    '        uc.Controls("Button4").Location = uc.Controls("Button5").Location
    '        Call pn.Controls.Add(uc)
    '    Else
    '        uc.Visible = True
    '    End If
    'End Sub

    ''累計タブ-課税非対象者照会ボタン
    'Private Sub Button21_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button21.Click
    '    Dim pn As Panel
    '    Dim uc As Control

    '    Me.Visible = False

    '    pn = ParentForm.Controls("Panel2")
    '    uc = pn.Controls("UC050302")

    '    If uc Is Nothing Then
    '        uc = New UC050302
    '        uc.Controls("label11").Text = "源泉徴収 - 課税非対象者累計"
    '        uc.Controls("TextBox1").Visible = False
    '        uc.Controls("TextBox2").Visible = False
    '        uc.Controls("TextBox3").Visible = False
    '        uc.Controls("TextBox4").Visible = False
    '        uc.Controls("Label7").Visible = False
    '        uc.Controls("TextBox11").Visible = False
    '        uc.Controls("Button5").Visible = False
    '        uc.Controls("Button4").Location = uc.Controls("Button5").Location
    '        Call pn.Controls.Add(uc)
    '    Else
    '        uc.Visible = True
    '    End If
    'End Sub

End Class
