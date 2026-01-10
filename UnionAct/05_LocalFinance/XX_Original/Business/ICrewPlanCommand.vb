Imports System
Imports System.Data

Namespace Business.RevenueExpenditure
    Public Interface ICrewPlanCommand
        ' Methods
        Sub CreateRevenueExpenditureRetireDtlWork(ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strEnd As String, ByVal strKsh As String)
        Function GetRevenueExpenditurePromotionDtl(ByVal strRevenueStr As String) As DataTable
        Function GetRevenueExpenditureRetireDtlWork(ByVal strMac As String, ByVal strControlName As String) As DataTable
        Function GetUnionDues(ByVal iNewStaffAV As Integer, ByVal iCapUpAV As Integer, ByVal iRetireAV As Integer, ByVal iUnPromotionAV As Integer, ByVal iSeniorAV As Integer, ByVal strDfrom As String) As DataTable
        Sub InsertRevenueExpenditurePromotionDtl(ByVal dtParameter As DataTable, ByVal dtKeyData As DataTable, ByVal dtUpdate As DataTable)
        Sub UpdateRevenueExpenditure(ByVal dtKeyData As DataTable, ByVal dtParameter As DataTable, ByVal dtUpdate As DataTable)
    End Interface
End Namespace
