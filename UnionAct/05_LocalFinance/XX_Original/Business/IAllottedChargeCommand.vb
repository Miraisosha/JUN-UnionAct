Imports System
Imports System.Data

Namespace Business.RevenueExpenditure
    Public Interface IAllottedChargeCommand
        ' Methods
        Sub Entry(ByVal dSetNew As DataSet, ByVal isNewEntry As Boolean, ByVal dTimeUp As DateTime)
        Function GetAllottedCharge(ByVal strStartRevenue As String, ByVal strLastRevenue As String, ByVal isNewEntry As Boolean) As DataSet
        Function GetTargetMemberCount(ByVal strRevenue As String, ByVal strBasisDate As String) As Integer
    End Interface
End Namespace
