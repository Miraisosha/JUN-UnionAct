Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.DAO.Master

Namespace Business.Master
    Public Class ConstantTblCommand
        'Implements IConstantTblCommand
        ' Methods
        Public Function GetConstantKind(ByVal strConstant As String, ByVal strDate As String) As DataSet
            Dim constantKind As DataSet
            Try
                constantKind = (New ConstantTblDao).GetConstantKind(strConstant, strDate)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return constantKind
        End Function

        Public Function GetConstantTbl(ByVal basisDate As String) As DataSet
            Dim constantDtlTbl As DataSet
            Try
                constantDtlTbl = (New ConstantTblDao).GetConstantDtlTbl(basisDate)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return constantDtlTbl
        End Function

        Public Function GetOfficerName(ByVal strKsh As String, ByVal strKeyDate As String) As String
            Dim officerName As String
            Try
                officerName = (New ConstantTblDao).GetOfficerName(strKsh, strKeyDate)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return officerName
        End Function

        Public Function GetSalaryItemDetails(ByVal strBasisDate As String) As DataSet
            Dim allData As DataSet
            Try
                allData = (New SalaryItemDetailsDao).GetAllData(strBasisDate)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return allData
        End Function

    End Class
End Namespace
