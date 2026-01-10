Imports System
Imports System.Data
Imports UnionAct.Framework.UnionConst

Namespace Business.FinancialAffairs.WageReduction
    Public Interface IWageReductionMonthlyCommand
        ' Methods
        Function FindMember(ByVal EmployeeNumber As String, ByVal TargetYM As String) As DataTable
        Function GetData(ByVal kind As WAGE_REDUCTION_KIND, ByVal TargetYM As String) As DataTable
        Function GetInTimeSummary(ByVal targetYM As String) As DataTable
        Function GetListPrintData(ByVal kind As WAGE_REDUCTION_KIND, ByVal TargetYear As String, ByVal TargetMonth As String) As DataSet
        Function GetMaxYM(ByVal kind As WAGE_REDUCTION_KIND) As String
        Function GetMinYear() As Integer
        Function GetMinYear(ByVal kind As WAGE_REDUCTION_KIND) As Integer
        Function GetStrikeSummary(ByVal targetYM As String) As DataTable
        Function GetSummaryPrintData(ByVal kind As WAGE_REDUCTION_KIND, ByVal TargetYear As String, ByVal TargetMonth As String) As DataSet
        Function IsTargetYearsExists(ByVal kind As WAGE_REDUCTION_KIND, ByVal TargetYM As String) As Boolean
        Sub SaveData(ByVal kind As WAGE_REDUCTION_KIND, ByVal SaveData As DataTable, ByVal register As String)
        Sub UpdateData(ByVal kind As WAGE_REDUCTION_KIND, ByVal TargetYM As String, ByVal SaveData As DataTable, ByVal register As String)
    End Interface
End Namespace

