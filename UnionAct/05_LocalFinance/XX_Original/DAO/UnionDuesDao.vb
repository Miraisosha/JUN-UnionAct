Imports UnionAct.Framework.UnionException

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection

Namespace DAO.Master
    Public Class UnionDuesDao
        Inherits AbstractDao
        'Implements IUnionDuesDao
        ' Methods
        Public Function GetUnionDues(ByVal strUnionDues As String, ByVal strQualification As String, ByVal strStafKind As String, ByVal iAge As Integer, ByVal strKeyDate As String) As Long
            Dim num As Long
            Try
                Dim cmdText As String = "SELECT  DTL.s_union_dues FROM union_dues UNI, union_dues_dtl DTL WHERE     UNI.k_union_dues = DTL.k_union_dues AND UNI.k_qualification = DTL.k_qualification AND UNI.k_staf_kind = DTL.k_staf_kind AND UNI.d_from = DTL.d_from AND UNI.k_union_dues = :strUnionDues AND UNI.k_qualification = :strQualification AND UNI.k_staf_kind = :strStafKind AND UNI.d_from <= :KeyDate AND UNI.d_to >= :KeyDate AND DTL.s_age_str <= :iAge AND DTL.s_age_end >= :iAge  "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("strUnionDues", DbType.String)
                command.Parameters.Add("strQualification", DbType.String)
                command.Parameters.Add("strStafKind", DbType.String)
                command.Parameters.Add("iAge", DbType.Int32)
                command.Parameters.Add("KeyDate", DbType.String)
                command.Parameters.Item("strUnionDues").Value = strUnionDues
                command.Parameters.Item("strQualification").Value = strQualification
                command.Parameters.Item("strStafKind").Value = strStafKind
                command.Parameters.Item("iAge").Value = iAge
                command.Parameters.Item("KeyDate").Value = strKeyDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("revenue_expenditure_retire_dtl_work", dReader)
                If (table Is Nothing) Then
                    Return 0
                End If
                If (table.Rows.Count <= 0) Then
                    Return 0
                End If
                If table.Rows.Item(0).Item(0).ToString.Equals("") Then
                    Return 0
                End If
                num = Convert.ToInt64(table.Rows.Item(0).Item(0))
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return num
        End Function

    End Class
End Namespace
