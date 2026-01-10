Imports UnionAct.Business.FinancialAffairs.WithHolding
Imports C1.Win.C1FlexGrid
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
Imports CrystalDecisions.CrystalReports.Engine

Namespace GUI.FinancialAffairs.WithHolding
    Public Class CtlCutDivTaxableDetail
        Inherits CtlWithHoldingDetailBase
        ' Methods
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal strYear As String, ByVal strMonth As String, ByVal strNameForRight As String, ByVal cancel As EventHandler)
            MyBase.New(strYear, strMonth, strNameForRight, cancel)
            Me.InitializeComponent()
            Me.AddFlexGridStyle()
            MyBase._settingInRef = Me.GetGridSetting(MODE.REFER)
            MyBase._settingInEdit = Me.GetGridSetting(MODE.EDIT)
        End Sub

        Protected Overrides Sub AddFlexGridStyle()
            Dim style As CellStyle = Nothing
            style = MyBase.flxList.Styles.Add("directors_remuneration_col")
            style.Font = FinancialAffairsUtility.GetGridFontNormal
            style.DataType = GetType(Long)
            style.TextAlign = TextAlignEnum.RightCenter
            style.Format = "N0"
            MyBase.flxList.Styles.Add("error_cell").BackColor = Color.LightPink
            MyBase.flxList.Styles.Add("changed_cell").ForeColor = Color.Red
            MyBase.flxList.Styles.Add("original_cell").ForeColor = SystemColors.WindowText
            MyBase.AddFlexGridStyle()
        End Sub

        Protected Overrides Sub CalcTotal(ByVal isError As Boolean)
            Me.CalcTotal(isError, Nothing)
        End Sub

        Protected Overloads Sub CalcTotal(ByVal isError As Boolean, ByVal rowLoopAct As SingleParameterDelegate(Of Integer))
            If isError Then
                Me.lblSumTruncate.Text = "#VALUE!"
                Me.lblSumWithholding.Text = "#VALUE!"
                Me.lblSumPayOut.Text = "#VALUE!"
            Else
                Dim num As Long = 0
                Dim num2 As Long = 0
                Dim num3 As Long = 0
                Dim i As Integer
                For i = 1 To MyBase.flxList.Rows.Count - 1
                    num = (num + MyBase.GetMoneyValue(Of Long)(i, COLIDX.CUT_OFF))
                    num2 = (num2 + MyBase.GetMoneyValue(Of Long)(i, COLIDX.WITHHOLDING))
                    num3 = (num3 + MyBase.GetMoneyValue(Of Long)(i, COLIDX.ALLOWANCE))
                    If (Not rowLoopAct Is Nothing) Then
                        rowLoopAct.Invoke(i)
                    End If
                Next i
                Me.lblSumTruncate.Text = num.ToString("###,###,##0")
                Me.lblSumWithholding.Text = num2.ToString("###,###,##0")
                Me.lblSumPayOut.Text = num3.ToString("###,###,##0")
            End If
        End Sub

        Protected Overrides Sub ChangeColumnOrder(ByRef dTblGrid As DataTable)
        End Sub

        Private Function Convert2ListReportData() As DataTable
            Dim table2 As DataTable
            Try
                'Dim table As DataTable = New WithholdingMonthlyTaxableReportListMap().CreateDataTablePhysName("dtDetail")
                Dim table As DataTable = New WithholdingMonthlyTaxableReportListMap().CreateDataTablePhysName("dtDetail")
                Dim objArray As Object() = New Object() {COLIDX.EMPLOYEE_NUMBER, COLIDX.NAME, MyBase.cmbBelonging.Text, COLIDX.LICENSE, COLIDX.DIRECTORS_REMUNERATION, COLIDX.MONTHLY_DEDUCTION, COLIDX.BONUS_DEDUCTION, 0, 0, COLIDX.CUT_OFF, 0, COLIDX.WITHHOLDING, COLIDX.CUT_OFF}
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

        Protected Overridable Function GetGridSetting(ByVal mode As Integer) As GridSettingInfo()
            Return Nothing
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
            Me.lblSumPayOut = New Label
            Me.lblSumWithholding = New Label
            Me.lblSumTruncate = New Label
            MyBase.flxList.BeginInit()
            MyBase.SuspendLayout()
            MyBase.cmbBelonging.Size = New Size(&H53, &H18)
            MyBase.flxList.Rows.Count = 2
            MyBase.flxList.Rows.DefaultSize = 20
            Me.lblSumPayOut.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumPayOut.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumPayOut.ForeColor = Color.Blue
            Me.lblSumPayOut.Location = New Point(&H394, &H2A1)
            Me.lblSumPayOut.Name = "lblSumPayOut"
            Me.lblSumPayOut.Size = New Size(&H63, &H17)
            Me.lblSumPayOut.TabIndex = &H13
            Me.lblSumPayOut.Text = "999,999,999"
            Me.lblSumPayOut.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumWithholding.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumWithholding.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumWithholding.ForeColor = Color.Blue
            Me.lblSumWithholding.Location = New Point(&H330, &H2A1)
            Me.lblSumWithholding.Name = "lblSumWithholding"
            Me.lblSumWithholding.Size = New Size(&H63, &H17)
            Me.lblSumWithholding.TabIndex = &H12
            Me.lblSumWithholding.Text = "999,999,999"
            Me.lblSumWithholding.TextAlign = ContentAlignment.MiddleRight
            Me.lblSumTruncate.BorderStyle = BorderStyle.Fixed3D
            Me.lblSumTruncate.Font = New Font("ÇlÇr" & " " & "ÉSÉVÉbÉN", 12.0!, FontStyle.Regular, GraphicsUnit.Point, &H80)
            Me.lblSumTruncate.ForeColor = Color.Blue
            Me.lblSumTruncate.Location = New Point(740, &H2A1)
            Me.lblSumTruncate.Name = "lblSumTruncate"
            Me.lblSumTruncate.Size = New Size(&H4B, &H17)
            Me.lblSumTruncate.TabIndex = &H11
            Me.lblSumTruncate.Text = "999,999"
            Me.lblSumTruncate.TextAlign = ContentAlignment.MiddleRight
            MyBase.Controls.Add(Me.lblSumPayOut)
            MyBase.Controls.Add(Me.lblSumWithholding)
            MyBase.Controls.Add(Me.lblSumTruncate)
            MyBase.Name = "CtlCutDivTaxableDetail"
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
            MyBase.Controls.SetChildIndex(MyBase.btnOutputFile, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumTruncate, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumWithholding, 0)
            MyBase.Controls.SetChildIndex(Me.lblSumPayOut, 0)
            MyBase.flxList.EndInit()
            MyBase.ResumeLayout(False)
            MyBase.PerformLayout()
        End Sub

        ' åπêÚí•é˚ - â€ê≈ëŒè€é“åéó·í¿ã‡
        ' åπêÚí•é˚ - â€ê≈ëŒè€é“àÍéûã‡
        Protected Overrides Sub PreviewDetail(ByVal row As Integer)
            Dim selectedMembers As New ArrayList
            selectedMembers.Add(MyBase.flxList.Item(row, COLIDX.USER_ID).ToString)
            ' ì˙ìñåvéZãÊï™îªíË
            If Me.CutDiv.Equals("05") Then
                ' åπêÚí•é˚ - â€ê≈ëŒè€é“åéó·í¿ã‡
                Me.PrintDetail2(selectedMembers, True)
            Else
                ' åπêÚí•é˚ - â€ê≈ëŒè€é“àÍéûã‡
                Me.PrintDetail(selectedMembers, True)
            End If
        End Sub

        ' ñæç◊àÛç¸É{É^Éìâüâ∫
        Protected Overrides Sub PrintDetailBySelected()
            Try
                Dim selectedMembers As ArrayList = MyBase.GetSelectedMembers(0, COLIDX.USER_ID)
                ' ì˙ìñåvéZãÊï™îªíË
                If Me.CutDiv.Equals("05") Then
                    ' åπêÚí•é˚ - â€ê≈ëŒè€é“åéó·í¿ã‡
                    Me.PrintDetail2(selectedMembers, False)
                Else
                    ' åπêÚí•é˚ - â€ê≈ëŒè€é“àÍéûã‡
                    Me.PrintDetail(selectedMembers, False)
                End If
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
                MyBase._original = MyBase._business.GetMonthlyTaxableData(TargetYear, TargetMonth, UnionBranch).Copy
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        Protected Overrides Sub ResetTotalLabels()
            FinancialAffairsUtility.SetZeroValueToLabels(New Label() {Me.lblSumPayOut, Me.lblSumTruncate, Me.lblSumWithholding})
        End Sub

        ' Properties
        Protected Overridable ReadOnly Property CutDiv() As String
            Get
                Return Nothing
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
        Protected lblSumWithholding As Label
        Protected Const STYLE_CHANGED_CELL As String = "changed_cell"
        Protected Const STYLE_ORIGINAL_CELL As String = "original_cell"

        ' Nested Types
        Protected Enum COLIDX
            ' Fields
            CHECK = 0                       ' É`ÉFÉbÉNÉ{ÉbÉNÉX
            EMPLOYEE_NUMBER = 2             ' é–àıî‘çÜ
            NAME = 3                        ' éÅñº
            LICENSE = 4                     ' éëäi
            DIRECTORS_REMUNERATION = 5      ' ñàıéËìñ
            MONTHLY_DEDUCTION = 6           ' åéó·çTèú
            BONUS_DEDUCTION = 7
            TAXABLE = 8                     ' â€ê≈ëŒè€äz
            CUT_OFF = 9                     ' êÿéÃÇƒäz
            WITHHOLDING = 10
            WITHHOLDING_MONTHLY = 11        ' åπêÚí•é˚äz
            WITHHOLDING_BONUS = 12
            ALLOWANCE = 13                  ' ç∑à¯éxããäz
            USER_ID = 14
            TAXABLE_FLAG = 15               ' â€ê≈ÉtÉâÉO
        End Enum
    End Class
End Namespace
