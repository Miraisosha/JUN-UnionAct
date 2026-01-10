#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException

Namespace DAO.Master
    Friend Class PriceBreakDao
        Inherits AbstractDao
        'Implements IPriceBreakDao
        ' Methods
        Public Function GetTruncPrice(ByVal CompanyCode As String, ByVal ApplyDate As String) As Integer
            Dim num As Integer
            Try 
                Dim cmdText As String = "SELECT A.s_break AS s_break FROM price_break A, (SELECT MAX(d_from) AS d_from FROM price_break WHERE c_ksh = :c_ksh AND d_from <= :d_from) B WHERE A.c_ksh = :c_ksh AND A.d_from = B.d_from"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("d_from").Value = ApplyDate
                command.Parameters.Item("c_ksh").Value = CompanyCode
                Dim obj2 As Object = command.ExecuteScalar
                If (TypeOf obj2 Is DBNull OrElse (obj2 Is Nothing)) Then
                    Throw New DataNotFoundException
                End If
                num = CInt(obj2)
            Catch exception As DataNotFoundException
                Throw exception
            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0002", New String(0  - 1) {})
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return num
        End Function

    End Class
End Namespace
