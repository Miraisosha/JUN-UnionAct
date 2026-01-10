Imports System
Imports System.Data

Namespace Business.FinancialAffairs.WageReduction
    Public Interface IWageReductionSumUpCommand
        ' Methods
        Function GetSummary(ByVal TargetYear As String) As DataTable
    End Interface
End Namespace

