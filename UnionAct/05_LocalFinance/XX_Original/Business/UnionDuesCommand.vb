Imports System
Imports System.Reflection
Imports UnionAct.DAO.Common
Imports UnionAct.Framework.UnionException
Imports UnionAct.DAO.Master

Namespace Business.Master
    Public Class UnionDuesCommand
        'Implements IUnionDuesCommand
        ' Methods
        Public Function GetUnionDuesCommand(ByVal strUnionDues As String, ByVal strQualification As String, ByVal strStafKind As String, ByVal iAge As Integer, ByVal strKeyDate As String) As Long
            Dim num2 As Long
            Try
                num2 = (New UnionDuesDao).GetUnionDues(strUnionDues, strQualification, strStafKind, iAge, strKeyDate)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return num2
        End Function

    End Class
End Namespace
