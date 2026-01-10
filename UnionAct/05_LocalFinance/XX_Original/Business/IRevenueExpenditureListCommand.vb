Imports System
Imports System.Data

Namespace Business.RevenueExpenditure
    Public Interface IRevenueExpenditureListCommand
        ' Methods
        Sub DeleteRevenueExpenditureCommand(ByVal strDFrom As String)
        Function GetRevenueCommand(ByVal strDFrom As String, ByVal strControlName As String) As String
        Function GetRevenueExpenditureList(ByVal strDFrom As String) As DataTable
    End Interface
End Namespace
