Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionException
Imports UnionAct.GUI.Common
Imports UnionAct.GUI.FinancialAffairs
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Windows.Forms
Imports UnionAct.Business.FinancialAffairs.WithHolding
Imports CrystalDecisions.CrystalReports.Engine

Namespace GUI.FinancialAffairs.WithHolding
    Public Class CtlCutDivNonTaxableDetail
        Inherits CtlWithHoldingDetailBase
        ' Methods
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal strYear As String, ByVal strMonth As String, ByVal strNameForRight As String, ByVal cancel As EventHandler)
            MyBase.New(strYear, strMonth, strNameForRight, cancel)
            Me.InitializeComponent()
            MyBase.AddFlexGridStyle()
            MyBase._settingInRef = New GridSettingInfo() {New GridSettingInfo(20, "check_col", False, False, False, True, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False), New GridSettingInfo(90, "employee_number_col_link", False, False, True, False, True), New GridSettingInfo(150, "name_col", False, False, False, False, True), New GridSettingInfo(60, "readonly_col", False, False, True, False, True), New GridSettingInfo(If(Me.IsMonthly, 100, 0), "noedit_money_col", False, False, True, False, Me.IsMonthly), New GridSettingInfo(If(Me.IsMonthly, 0, 100), "noedit_money_col", False, False, True, False, Not Me.IsMonthly), New GridSettingInfo(&H4B, "noedit_money_col", False, False, True, False, True), New GridSettingInfo(100, "noedit_money_col", False, False, False, False, True), New GridSettingInfo(0, "readonly_col", False, False, False, False, False)}
        End Sub

        Protected Overrides Sub CalcTotal(ByVal isError As Boolean)
            Me.CalcTotal(isError, Nothing)
        End Sub

        Protected Overloads Sub CalcTotal(ByVal isError As Boolean, ByVal rowLoopAct As SingleParameterDelegate(Of Integer))
            Dim num As Long = 0
            Dim num2 As Long = 0
            Dim i As Integer
            For i = 1 To MyBase.flxList.Rows.Count - 1
                num = (num + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.CUT_OFF) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.CUT_OFF))))
                num2 = (num2 + If((MyBase.flxList.Rows.Item(i).Item(COLIDX.ALLOWANCE) Is Nothing), 0, CLng(MyBase.flxList.Rows.Item(i).Item(COLIDX.ALLOWANCE))))
                If (Not rowLoopAct Is Nothing) Then
                    rowLoopAct.Invoke(i)
                End If
            Next i
            Me.lblSumTruncate.Text = num.ToString("###,###,##0")
            Me.lblSumPayOut.Text = num2.ToString("###,###,##0")
        End Sub

        Private Function Convert2ListReportData() As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = New WithholdingMonthlyNonTaxableReportListMap().CreateDataTablePhysName("dtDetail")
                Dim objArray As Object() = New Object() {COLIDX.EMPLOYEE_NUMBER, COLIDX.NAME, MyBase.cmbBelonging.Text, COLIDX.LICENSE, COLIDX.MONTHLY_DEDUCTION, COLIDX.BONUS_DEDUCTION, COLIDX.CUT_OFF}
                Dim i As Integer
                For i = 1 To MyBase.flxList.Rows.Count - 1
                    Dim row As DataRow = table.NewRow
                    Dim j As Integer
                    For j = 0 To objArray.Length - 1
                        row.Item(j) = If(TypeOf objArray(j) Is COLIDX, MyBase.flxList.Rows.Item(i).Item(CInt(objArray(j))), objArray(j))
                    Next j
                    table.Rows.Add(row)
                Next i
                table2 = table
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Function GetBusObj() As WithholdingCommand
            Return MyBase.CreateBusinessObject(New Object() {Me.CutDiv})
        End Function

        Private Function GetListData() As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim table As DataTable = FinancialAffairsUtility.CreateWithholdingReportHeader(MyBase.TargetYear, MyBase.TargetMonth)
                ds.Tables.Add(table)
                Dim table2 As DataTable = Me.Convert2ListReportData
                ds.Tables.Add(table2)
                set2 = ds
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
            Return set2
        End Function

        Protected Overrides Function GetNotOutputFileColumns() As String()
            Dim list As List(Of String) = MyBase.SelectUnVisibleColumns(MyBase.flxList)
            list.Add("Å@")
            Return list.ToArray
        End Function

        Private Sub InitializeComponent()
            Me.lblSumPayOut = New System.Windows.Forms.Label
            Me.lblSumTruncate = New System.Windows.Forms.Label
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'cmbBelonging
            '
            Me.cmbBelonging.Size = New System.Drawing.Size(83, 24)
            '
            'flxList
            '
            Me.flxList.Rows.Count = 2
            Me.flxList.Rows.DefaultSize = 20
            '
            'lblSumPayOut
            '
            Me.lblSumPayOut.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSumPayOut.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumPayOut.Location = New System.Drawing.Point(775, 676)
            Me.lblSumPayOut.Name = "lblSumPayOut"
            Me.lblSumPayOut.Size = New System.Drawing.Size(99, 23)
            Me.lblSumPayOut.TabIndex = 15
            Me.lblSumPayOut.Text = "999,999,999"
            Me.lblSumPayOut.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'lblSumTruncate
            '
            Me.lblSumTruncate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSumTruncate.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumTruncate.Location = New System.Drawing.Point(699, 676)
            Me.lblSumTruncate.Name = "lblSumTruncate"
            Me.lblSumTruncate.Size = New System.Drawing.Size(75, 23)
            Me.lblSumTruncate.TabIndex = 14
            Me.lblSumTruncate.Text = "999,999"
            Me.lblSumTruncate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'CtlCutDivNonTaxableDetail
            '
            Me.Controls.Add(Me.lblSumPayOut)
            Me.Controls.Add(Me.lblSumTruncate)
            Me.Name = "CtlCutDivNonTaxableDetail"
            Me.Controls.SetChildIndex(Me.label7, 0)
            Me.Controls.SetChildIndex(Me.label6, 0)
            Me.Controls.SetChildIndex(Me.lblYear, 0)
            Me.Controls.SetChildIndex(Me.lblMonth, 0)
            Me.Controls.SetChildIndex(Me.lblBelongLocal, 0)
            Me.Controls.SetChildIndex(Me.cmbBelonging, 0)
            Me.Controls.SetChildIndex(Me.btnShow, 0)
            Me.Controls.SetChildIndex(Me.btnBackOrCancel, 0)
            Me.Controls.SetChildIndex(Me.btnPrintList, 0)
            Me.Controls.SetChildIndex(Me.btnPrintDetails, 0)
            Me.Controls.SetChildIndex(Me.flxList, 0)
            Me.Controls.SetChildIndex(Me.btnAllCheckOn, 0)
            Me.Controls.SetChildIndex(Me.btnAllCheckOff, 0)
            Me.Controls.SetChildIndex(Me.btnOutputFile, 0)
            Me.Controls.SetChildIndex(Me.lblSumTruncate, 0)
            Me.Controls.SetChildIndex(Me.lblSumPayOut, 0)
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Protected Overrides Sub PreviewDetail(ByVal row As Integer)
            Dim selectedMembers As New ArrayList
            selectedMembers.Add(MyBase.flxList.Item(row, COLIDX.USER_ID).ToString)
            Me.PrintDetail(selectedMembers, True)
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
            Dim viewer As New ReportViewer(Me.GetListData, Me.ListReportName) 'CR0503PX
            viewer.ReportViewerShow()
        End Sub

        Protected Overrides Sub Query(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String)
            Try
                MyBase._original = MyBase._business.GetMonthlyNonTaxableData(TargetYear, TargetMonth, UnionBranch).Copy
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Protected Overrides Sub ResetTotalLabels()
            FinancialAffairsUtility.SetZeroValueToLabels(New Label() {Me.lblSumPayOut, Me.lblSumTruncate})
        End Sub


        ' Properties
        Protected Overridable ReadOnly Property CutDiv() As String
            Get
                Return Nothing
            End Get
        End Property

        Private ReadOnly Property IsMonthly() As Boolean
            Get
                Return Me.CutDiv.Equals("05")
            End Get
        End Property

        Protected Overridable ReadOnly Property ListReportName() As ReportClass
            Get
                Return Nothing
            End Get
        End Property

        Public Sub SetValidator(ByVal NewValidator As ValidateDelegate)
            Me._validator = NewValidator
        End Sub

        Public Overridable Sub ValidateFields()
            If (Not Me._validator Is Nothing) Then
                Me._validator.Invoke()
            End If
        End Sub


        ' Fields
        Private _validator As ValidateDelegate
        Private components As IContainer
        Protected lblSumPayOut As Label
        Protected lblSumTruncate As Label

        ' Nested Types
        Protected Enum COLIDX
            ' Fields
            CHECK = 0
            EMPLOYEE_NUMBER = 2
            NAME = 3
            LICENSE = 4
            MONTHLY_DEDUCTION = 5
            BONUS_DEDUCTION = 6
            CUT_OFF = 7
            ALLOWANCE = 8
            USER_ID = 9
        End Enum
    End Class
End Namespace
