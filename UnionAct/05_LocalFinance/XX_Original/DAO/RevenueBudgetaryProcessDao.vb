Imports UnionAct.Framework.Mapping

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException

Namespace DAO.RevenueExpenditure
    Public Class RevenueBudgetaryProcessDao
        Inherits AbstractDao
        'Implements IRevenueBudgetaryProcessDao
        ' Methods
        Public Sub DeleteRevenueBudgetaryProcessDtl(ByVal command As NpgsqlCommand, ByVal strRevenue As String, ByVal strBudgetaryKind As String, ByVal strBudgetaryProcess As String)
            Try
                Dim cmdText As String = "delete from revenue_budgetary_process_dtl where d_revenue_str = :d_revenue_str and k_budgetary_kind = :k_budgetary_kind and k_budgetary_process = :k_budgetary_process "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Add("k_budgetary_kind", DbType.String)
                command.Parameters.Add("k_budgetary_process", DbType.String)
                command.Parameters.Item("d_revenue_str").Value = strRevenue
                command.Parameters.Item("k_budgetary_kind").Value = strBudgetaryKind
                command.Parameters.Item("k_budgetary_process").Value = strBudgetaryProcess
                command.ExecuteNonQuery()
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
        End Sub

        Public Function GetRevenueBudgetaryProcessData(ByVal strRevenueDate As String, ByVal strBudgetaryKind As String, ByVal strBudgetaryProcess As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "select HED.d_revenue_str, HED.k_budgetary_kind, HED.k_budgetary_process, DTL.s_budgetary_process_seq, DTL.l_number as " & "çÄñ⁄î‘çÜ" & ", DTL.l_name as " & "çÄñ⁄ñº" & ", DTL.s_budgetary_money as " & "ã‡äz" & ", DTL.l_biko_1 as " & "îıçl" & ", IIF(HED.d_up IS NULL, HED.d_up, HED.d_ins) as revise_day from revenue_budgetary_process HED, revenue_budgetary_process_dtl DTL where HED.d_revenue_str = :d_revenue_str and HED.k_budgetary_kind = :k_budgetary_kind and HED.k_budgetary_process = :k_budgetary_process and HED.d_revenue_str = DTL.d_revenue_str and HED.k_budgetary_kind = DTL.k_budgetary_kind and HED.k_budgetary_process = DTL.k_budgetary_process order by DTL.s_budgetary_process_seq "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Add("k_budgetary_kind", DbType.String)
                command.Parameters.Add("k_budgetary_process", DbType.String)
                command.Parameters.Item("d_revenue_str").Value = strRevenueDate
                command.Parameters.Item("k_budgetary_kind").Value = strBudgetaryKind
                command.Parameters.Item("k_budgetary_process").Value = strBudgetaryProcess
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("revenue_budgetary_process", dReader))
                set2 = ds
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
            Return set2
        End Function

        Public Sub InsertRevenueBudgetaryProcess(ByVal command As NpgsqlCommand, ByVal dSetNew As DataSet)
            Try
                MyBase.InsertData(command, New RevenueBudgetaryProcessMap, dSetNew, "revenue_budgetary_process")
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
        End Sub

        Public Sub InsertRevenueBudgetaryProcessDtl(ByVal command As NpgsqlCommand, ByVal dSetNew As DataSet)
            Try
                MyBase.InsertData(command, New RevenueBudgetaryProcessDtlMap, dSetNew, "revenue_budgetary_process_dtl")
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
        End Sub

        Public Sub UpdateRevenueBudgetaryProcess(ByVal command As NpgsqlCommand, ByVal dSetNew As DataSet)
            Try
                Dim cmdText As String = "update revenue_budgetary_process set s_sub_sum = :s_sub_sum, d_up = CONVERT(DATE,:d_up), c_user_id_up = :c_user_id_up, s_up = s_up + 1 where d_revenue_str = :d_revenue_str and k_budgetary_kind = :k_budgetary_kind and k_budgetary_process = :k_budgetary_process "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                Dim i As Integer
                For i = 0 To dSetNew.Tables.Item("revenue_budgetary_process").Rows.Count - 1
                    command.SetSql(cmdText)
                    command.Parameters.Clear()
                    command.Parameters.Add("s_sub_sum", DbType.Int32)
                    command.Parameters.Add("d_up", DbType.String)
                    command.Parameters.Add("c_user_id_up", DbType.String)
                    command.Parameters.Add("d_revenue_str", DbType.String)
                    command.Parameters.Add("k_budgetary_kind", DbType.String)
                    command.Parameters.Add("k_budgetary_process", DbType.String)
                    command.Parameters.Item("s_sub_sum").Value = dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("s_sub_sum")
                    command.Parameters.Item("d_up").Value = dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("d_up")
                    command.Parameters.Item("c_user_id_up").Value = dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("c_user_id_up")
                    command.Parameters.Item("d_revenue_str").Value = dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("d_revenue_str")
                    command.Parameters.Item("k_budgetary_kind").Value = dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("k_budgetary_kind")
                    command.Parameters.Item("k_budgetary_process").Value = dSetNew.Tables.Item("revenue_budgetary_process").Rows.Item(i).Item("k_budgetary_process")
                    If (command.ExecuteNonQuery <> 1) Then
                        Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0002", New String(0 - 1) {})
                    End If
                Next i
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
        End Sub

    End Class
End Namespace
