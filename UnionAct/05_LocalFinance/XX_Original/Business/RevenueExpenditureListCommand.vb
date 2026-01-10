Imports UnionAct.DAO

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Mapping
Imports UnionAct.DAO.RevenueExpenditure

Namespace Business.RevenueExpenditure
    Public Class RevenueExpenditureListCommand
        'Implements IRevenueExpenditureListCommand
        ' Methods
        Public Sub DeleteRevenueExpenditureCommand(ByVal strDFrom As String)
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                dao.DeleteRevenueExpenditureDao(command, strDFrom)
                dao.DeleteRevenueExpenditureMemberDao(command, strDFrom)
                objTran.Commit()
            Catch exception As AppUnionException
                objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                objTran.Rollback()
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                objTran.Rollback()
                Dim exception4 As New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
                Throw exception4
            End Try
        End Sub

        Public Function GetRevenueCommand(ByVal strDFrom As String, ByVal strControlName As String) As String
            Dim str2 As String
            Try
                Dim revenueKbnDao As String = dao.GetRevenueKbnDao(strDFrom, strControlName)
                If Not revenueKbnDao.Equals("") Then
                    'TODO Dim trimChars As Char() = New Char() {&HFF0C}
                    Dim trimChars As Char = ChrW(&HFF0C)
                    revenueKbnDao = ("Åw" & revenueKbnDao.TrimEnd(trimChars) & "Åx")
                End If
                str2 = revenueKbnDao
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return str2
        End Function

        Public Function GetRevenueExpenditureList(ByVal strDFrom As String) As DataTable
            Dim table3 As DataTable
            Try
                Dim revenueExpenditure As DataTable = dao.GetRevenueExpenditure(strDFrom)
                Dim table2 As DataTable = New RevenueExpenditureListMap().CreateDataTablePhysName("revenue_expenditure")
                Dim rowArray As DataRow() = revenueExpenditure.Select
                Dim i As Integer
                For i = 0 To rowArray.Length - 1
                    table2.ImportRow(rowArray(i))
                Next i
                table3 = table2
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function


        ' Fields
        Private dao As New RevenueExpenditureDao
        'Private _factory As FactoryDaoClass
        'Private _objLoginSession As LoginSession
    End Class
End Namespace
