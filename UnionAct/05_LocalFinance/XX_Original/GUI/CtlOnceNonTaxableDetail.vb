Imports UnionAct.Business.FinancialAffairs.WithHolding
Imports UnionAct.Framework.Interface
Imports UnionAct.GUI.FinancialAffairs
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports CrystalDecisions.CrystalReports.Engine
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.Framework.UnionException
Imports System.Reflection

Namespace GUI.FinancialAffairs.WithHolding
    Public Class CtlOnceNonTaxableDetail
        Inherits CtlCutDivNonTaxableDetail
        ' Methods
        Public Sub New()
            Me.InitializeComponent()
        End Sub

        Public Sub New(ByVal strYear As String, ByVal strMonth As String, ByVal strNameForRight As String, ByVal cancel As EventHandler, ByVal strOnceName As String)
            MyBase.New(strYear, strMonth, strNameForRight, cancel)
            Me.InitializeComponent()
            ' ADD 2012/06/24
            Dim clsMdb As New CLAccessMdb
            Dim table As DataTable
            clsMdb.Connect()
            table = clsMdb.ExecuteSql("SELECT DISTINCT c_pay_once_name FROM taxation_total AS CUT")
            If table.Rows.Count > 0 Then
                If table.Rows(0).Item(0) Is DBNull.Value Then
                ElseIf Trim(table.Rows(0).Item(0)) <> "" Then
                    table.Rows.InsertAt(table.NewRow, 0)
                End If
            End If
            Me.cmbCutOnceName.DataSource = table
            Me.cmbCutOnceName.DisplayMember = "c_pay_once_name"
            clsMdb.Disconnect()
            Me.cmbCutOnceName.Text = strOnceName
            ' ADD 2012/06/24
        End Sub

        Protected Overrides Sub CalcTotal(ByVal isError As Boolean)
            Dim nBonus As Long = 0
            For i = 1 To MyBase.flxList.Rows.Count - 1
                nBonus = (nBonus + MyBase.GetMoneyValue(Of Long)(i, COLIDX.BONUS_DEDUCTION))
            Next i
            MyBase.CalcTotal(isError)
            Me.lblSumBonus.Text = nBonus.ToString("###,###,##0")
        End Sub

        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If (disposing AndAlso (Not Me.components Is Nothing)) Then
                Me.components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        Protected Overrides Function GetBusObj() As WithholdingCommand
            Return MyBase.CreateBusinessObject(New Object() {"06"})
        End Function

        Protected Overrides Function GetOutputFileName() As String
            Return String.Concat(New String() {"àÍéûã‡" & " - " & "â€ê≈îÒëŒè€é“", MyBase.lblYear.Text, "îN", MyBase.lblMonth.Text, "åéï™" & " ", MyBase.cmbBelonging.Text})
        End Function

        Private Sub InitializeComponent()
            Me.lblSumBonus = New System.Windows.Forms.Label
            Me.cmbCutOnceName = New System.Windows.Forms.ComboBox
            Me.Label9 = New System.Windows.Forms.Label
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'lblSumPayOut
            '
            Me.lblSumPayOut.ForeColor = System.Drawing.SystemColors.ControlText
            Me.lblSumPayOut.Location = New System.Drawing.Point(709, 676)
            '
            'lblSumTruncate
            '
            Me.lblSumTruncate.ForeColor = System.Drawing.SystemColors.ControlText
            Me.lblSumTruncate.Location = New System.Drawing.Point(633, 676)
            '
            'btnAllCheckOff
            '
            Me.btnAllCheckOff.Location = New System.Drawing.Point(243, 676)
            '
            'btnAllCheckOn
            '
            Me.btnAllCheckOn.Location = New System.Drawing.Point(211, 676)
            '
            'btnShow
            '
            Me.btnShow.Location = New System.Drawing.Point(759, 14)
            '
            'cmbBelonging
            '
            Me.cmbBelonging.Location = New System.Drawing.Point(424, 15)
            '
            'flxList
            '
            Me.flxList.Location = New System.Drawing.Point(211, 59)
            Me.flxList.Rows.Count = 2
            Me.flxList.Rows.DefaultSize = 20
            Me.flxList.Size = New System.Drawing.Size(614, 611)
            '
            'label6
            '
            Me.label6.Location = New System.Drawing.Point(271, 19)
            '
            'label7
            '
            Me.label7.Location = New System.Drawing.Point(336, 20)
            '
            'lblBelongLocal
            '
            Me.lblBelongLocal.Location = New System.Drawing.Point(381, 20)
            '
            'lblMonth
            '
            Me.lblMonth.Location = New System.Drawing.Point(301, 15)
            '
            'lblYear
            '
            Me.lblYear.Location = New System.Drawing.Point(217, 15)
            '
            'lblSumBonus
            '
            Me.lblSumBonus.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblSumBonus.Font = New System.Drawing.Font("ÇlÇr ÉSÉVÉbÉN", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
            Me.lblSumBonus.ForeColor = System.Drawing.SystemColors.ControlText
            Me.lblSumBonus.Location = New System.Drawing.Point(532, 676)
            Me.lblSumBonus.Name = "lblSumBonus"
            Me.lblSumBonus.Size = New System.Drawing.Size(99, 23)
            Me.lblSumBonus.TabIndex = 17
            Me.lblSumBonus.Text = "999,999,999"
            Me.lblSumBonus.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'cmbCutOnceName
            '
            Me.cmbCutOnceName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCutOnceName.FormattingEnabled = True
            Me.cmbCutOnceName.Location = New System.Drawing.Point(600, 15)
            Me.cmbCutOnceName.Name = "cmbCutOnceName"
            Me.cmbCutOnceName.Size = New System.Drawing.Size(153, 24)
            Me.cmbCutOnceName.TabIndex = 23
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.Location = New System.Drawing.Point(511, 19)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(88, 16)
            Me.Label9.TabIndex = 24
            Me.Label9.Text = "àÍéûã‡ñºèÃ"
            '
            'CtlOnceNonTaxableDetail
            '
            Me.Controls.Add(Me.cmbCutOnceName)
            Me.Controls.Add(Me.Label9)
            Me.Controls.Add(Me.lblSumBonus)
            Me.Name = "CtlOnceNonTaxableDetail"
            Me.Controls.SetChildIndex(Me.lblSumTruncate, 0)
            Me.Controls.SetChildIndex(Me.lblSumPayOut, 0)
            Me.Controls.SetChildIndex(Me.btnAllCheckOff, 0)
            Me.Controls.SetChildIndex(Me.btnAllCheckOn, 0)
            Me.Controls.SetChildIndex(Me.btnOutputFile, 0)
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
            Me.Controls.SetChildIndex(Me.lblSumBonus, 0)
            Me.Controls.SetChildIndex(Me.Label9, 0)
            Me.Controls.SetChildIndex(Me.cmbCutOnceName, 0)
            CType(Me.flxList, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Protected Overrides Sub PrintDetail(ByVal SelectedMembers As ArrayList, ByVal Preview As Boolean)
            'MyBase.PrintDetail(SelectedMembers, Preview, "Report.Withholding.RptWageCutCoverNews_cut_once")
            MyBase.PrintDetailPreview(SelectedMembers, Preview, New CR0503P1)
        End Sub

        Protected Overrides Sub ResetTotalLabels()
            MyBase.ResetTotalLabels()
            FinancialAffairsUtility.SetZeroValueToLabels(New Label() {Me.lblSumBonus})
        End Sub

        ' ADD 2012/06/24
        Protected Overrides Sub Query(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String)
            Try
                MyBase._original = MyBase._business.GetMonthlyNonTaxableData(TargetYear, TargetMonth, UnionBranch, Me.cmbCutOnceName.Text).Copy
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
        End Sub

        ' Properties
        Protected Overrides ReadOnly Property CutDiv() As String
            Get
                Return "06"
            End Get
        End Property

        Protected Overrides ReadOnly Property ListReportName() As ReportClass
            Get
                'Return "Report.Withholding.RptTaxNoIntendedPayTable_local_cut_once"
                Return New CR0503PF
            End Get
        End Property


        ' Fields
        Private components As IContainer
        Private WithEvents cmbCutOnceName As System.Windows.Forms.ComboBox
        Private WithEvents Label9 As System.Windows.Forms.Label
        Private lblSumBonus As Label
    End Class
End Namespace
