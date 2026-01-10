Imports System
Imports System.Data

Namespace Business.FinancialAffairs.WageReduction
    Public Interface IWageReductionBonusCommand
        ' Methods
        Function GetData(ByVal TargetYM As String) As DataTable
        Function GetListPrintData(ByVal TargetYear As String, ByVal TargetMonth As String) As DataSet
        Function GetMaxYM() As String
        Function GetMinYear() As Integer
        Function GetSummary(ByVal TargetYear As String, ByVal TargetMonth As String) As DataTable
        Function GetSummaryPrintData(ByVal TargetYear As String, ByVal TargetMonth As String) As DataSet
        Function IsTargetYearsExists(ByVal TargetYM As String) As Boolean
        Sub SaveData(ByVal SaveData As DataSet, ByVal Register As String)
        Sub UpdateData(ByVal TargetYM As String, ByVal SaveData As DataSet, ByVal register As String)
    End Interface
End Namespace

