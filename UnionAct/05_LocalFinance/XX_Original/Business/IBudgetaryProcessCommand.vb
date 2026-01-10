Imports System
Imports System.Data

Namespace Business.RevenueExpenditure
    Public Interface IBudgetaryProcessCommand
        ' Methods
        Sub Entry(ByVal dSetNew As DataSet, ByVal isNewEntry As Boolean, ByVal isRevise As Boolean, ByVal dTimeUp As DateTime)
        Function GetRevenueBudgetary(ByVal strRevenueDate As String, ByVal strLastRevenueDate As String, ByVal isNewEntry As Boolean, ByVal isRevise As Boolean, ByVal lngReviseRevenueTotal As Long) As DataSet
        Sub UpdateRevenueSeton(ByVal strRevenueDate As String, ByVal dTimeUp As DateTime, ByVal strUserIDup As String)
    End Interface
End Namespace
