Imports System
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.DAO.Master
Imports UnionAct.DAO.Common

Namespace Business.FinancialAffairs
    Public Class FinancialAffairsBase
        'Public MustInherit Class FinancialAffairsBase
        Inherits UserControl
        ' Methods
        Protected Sub New()
        End Sub

        Public Function GetTruncateAmount(ByVal CriterionDate As String) As Integer
            Dim truncPrice As Integer
            Try
                'truncPrice = DirectCast(Me._factory.GetObject("DAO.Master.PriceBreakDao"), IPriceBreakDao).GetTruncPrice(PublicCommand.GetKsh, CriterionDate)
                Dim dao As New PriceBreakDao
                truncPrice = dao.GetTruncPrice(PublicCommand.GetKsh, CriterionDate)
            Catch exception1 As DataNotFoundException
                truncPrice = 100
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            Return truncPrice
        End Function

        Protected Function GetTruncPlace(ByVal CriterionDate As String) As Integer
            Dim truncateAmount As Integer = 0
            Try
                truncateAmount = Me.GetTruncateAmount(CriterionDate)
            Catch exception1 As DataNotFoundException
                truncateAmount = 100
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "BE0001", New String(0 - 1) {})
            End Try
            If (truncateAmount <> 0) Then
                Return (CInt(Math.Log10(CDbl(truncateAmount))) * -1)
            End If
            Return truncateAmount
        End Function

        ' Fields
        Protected _factory As FactoryDaoClass = New FactoryDaoClass
        Protected Const DATATABLE_NAME_DETAIL As String = "dtDetail"
        Protected Const DATATABLE_NAME_HEADER As String = "dtHeader"
        Protected Const LAST_OF_YEAR As String = "1231"
    End Class
End Namespace
