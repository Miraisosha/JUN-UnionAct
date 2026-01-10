Imports System
Imports System.Collections
Imports System.Data

Namespace Business.FinancialAffairs.WithHolding
    Public Interface IWithholdingCommand
        ' Methods
        Function CalcWithholding(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal Taxable As Long) As Long
        Function ExecuteCalcWithholding(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UserId As String) As Integer
        Function GetExistYear() As DataSet
        Function GetMinYear() As Integer
        Function GetMonthlyNonTaxableData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String) As DataTable
        Function GetMonthlyNonTaxableListReportData(ByVal TargetYear As String, ByVal TargetMonth As String) As DataSet
        Function GetMonthlyReportDetailData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal Selectedmembers As ArrayList) As DataSet
        Function GetMonthlyReportListData(ByVal TargetYear As String, ByVal TargetMonth As String) As DataTable
        Function GetMonthlyTaxableData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal UnionBranch As String) As DataTable
        Function GetSumUpNonTaxableData(ByVal TargetYear As String, ByVal UnionBranch As String) As DataTable
        Function GetSumUpNonTaxableDetailReportData(ByVal TargetYear As String, ByVal SelectedMembers As ArrayList) As DataSet
        Function GetSumUpNonTaxableListReportData(ByVal TargetYear As String) As DataTable
        Function GetSumUpTaxableData(ByVal TargetYear As String, ByVal UnionBranch As String) As DataTable
        Function GetSumUpTaxableDetailReportData(ByVal TargetYear As String, ByVal SelectedMembers As ArrayList) As DataSet
        Function GetSumUpTaxableListReportData(ByVal TargetYear As String) As DataTable
        Function GetTruncateAmount(ByVal CriterionDate As String) As Integer
        Function IsGreaterThanExists(ByVal TargetYear As String, ByVal TargetMonth As String) As Boolean
        Function IsWithholdingExists(ByVal TargetYear As String, ByVal TargetMonth As String) As Boolean
        Function QueryMonthlySummary(ByVal TargetYear As String, ByVal TargetMonth As String) As DataSet
        Function QuerySumUpSummary(ByVal TargetYear As String) As DataSet
        Function UpdateData(ByVal TargetYear As String, ByVal TargetMonth As String, ByVal SaveData As DataTable, ByVal User As String) As Integer
    End Interface
End Namespace

