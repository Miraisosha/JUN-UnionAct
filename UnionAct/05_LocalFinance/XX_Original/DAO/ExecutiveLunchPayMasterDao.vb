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
    Public Class ExecutiveLunchPayMasterDao
        Inherits AbstractDao
        'Implements IExecutiveLunchPayMasterDao
        ' Methods
        Public Function GetExecutiveLunchPay(ByVal strCommitteeId As String, ByVal strCommitteeSeq As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "SELECT executive_lunch_pay_master.c_executive_lunch_pay_id, executive_lunch_pay_master.s_pay FROM executive_lunch_pay_master, (SELECT c_executive_lunch_pay_id FROM committee_dtl WHERE c_committee_id = :c_committee_id AND s_committee_seq = :s_committee_seq AND d_from <= :d_date AND d_to >= :d_date GROUP BY c_executive_lunch_pay_id ) committee_dtl_A WHERE d_from <= :d_date AND d_to >= :d_date AND Format(executive_lunch_pay_master.c_executive_lunch_pay_id, '000') = committee_dtl_A.c_executive_lunch_pay_id ORDER BY executive_lunch_pay_master.c_executive_lunch_pay_id "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("s_committee_seq").Value = Integer.Parse(strCommitteeSeq)
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "中央執行昼食費マスタの情報" })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("executive_lunch_pay_master", dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetExecutiveLunchPayAll(ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "SELECT c_executive_lunch_pay_id, s_pay FROM executive_lunch_pay_master WHERE d_from <= :d_date AND d_to >= :d_date "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "中央執行昼食費マスタの情報" })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("executive_lunch_pay_master", dReader))
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

    End Class
End Namespace
