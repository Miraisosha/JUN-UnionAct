#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Mapping

Namespace DAO.Master
    Public Class DailyPayMasterDtlDao
        Inherits AbstractDao
        'Implements IDailyPayMasterDtlDao
        ' Methods
        Public Function GetDailyPayDtl(ByVal strCommitteeId As String, ByVal strCommitteeSeq As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "SELECT " & "	" & "daily_pay_master_dtl.c_daily_pay_id, " & "	" & "daily_pay_master_dtl.c_menu_seq, " & "	" & "daily_pay_master_dtl.l_name, " & "	" & "daily_pay_master_dtl.l_explain, " & "	" & "daily_pay_master_dtl.s_daily_pay FROM " & "	" & "daily_pay_master_dtl, " & "	" & "(SELECT " & "		" & "daily_pay_master.c_daily_pay_id " & "	" & "FROM " & "		" & "daily_pay_master, " & "		" & "(SELECT " & "			" & "c_daily_pay_id " & "		" & "FROM " & "			" & "committee_dtl " & "		" & "WHERE " & "			" & "c_committee_id = :c_committee_id " & "			" & "AND s_committee_seq = :s_committee_seq " & "			" & "AND d_from <= :d_date " & "			" & "AND d_to >= :d_date " & "		" & "GROUP BY " & "			" & "c_daily_pay_id " & "		" & ") committee_dtl_A " & "	" & "WHERE " & "		" & "c_ksh = :c_ksh " & "		" & "AND d_from <= :d_date " & "		" & "AND d_to >= :d_date " & "		" & "AND daily_pay_master.c_daily_pay_id = committee_dtl_A.c_daily_pay_id " & "	" & "GROUP BY " & "		" & "daily_pay_master.c_daily_pay_id " & "	" & ") daily_pay_master_A WHERE " & "	" & "d_from <= :d_date " & "	" & "AND d_to >= :d_date " & "	" & "AND daily_pay_master_dtl.c_daily_pay_id = daily_pay_master_A.c_daily_pay_id ORDER BY " & "	" & "c_menu_seq "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("s_committee_seq").Value = Integer.Parse(strCommitteeSeq)
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "日当マスタ詳細の情報" })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("daily_pay_master_dtl", dReader))
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

        Public Function GetDailyPayId() As String()
            Dim strArray2 As String()
            Try 
                Dim cmdText As String = "SELECT " & "	" & "c_daily_pay_id FROM " & "	" & "daily_pay_master WHERE " & "	" & "c_ksh = :c_ksh GROUP BY " & "	" & "c_daily_pay_id ORDER BY " & "	" & "c_daily_pay_id; "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("daily_pay_master", dReader)
                Dim strArray As String() = New String(table.Rows.Count  - 1) {}
                Dim i As Integer
                For i = 0 To table.Rows.Count - 1
                    strArray(i) = table.Rows.Item(i).Item("c_daily_pay_id").ToString
                Next i
                strArray2 = strArray
            Catch exception As NpgsqlException
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return strArray2
        End Function

        Public Function GetDailyPayId(ByVal strDate As String) As String()
            Dim strArray2 As String()
            Try 
                Dim cmdText As String = "SELECT " & "	" & "c_daily_pay_id FROM " & "	" & "daily_pay_master WHERE " & "	" & "c_ksh = :c_ksh " & "	" & "AND d_from <= :d_date " & "	" & "AND d_to >= :d_date GROUP BY " & "	" & "c_daily_pay_id ORDER BY " & "	" & "c_daily_pay_id; "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("daily_pay_master", dReader)
                Dim strArray As String() = New String(table.Rows.Count  - 1) {}
                Dim i As Integer
                For i = 0 To table.Rows.Count - 1
                    strArray(i) = table.Rows.Item(i).Item("c_daily_pay_id").ToString
                Next i
                strArray2 = strArray
            Catch exception As NpgsqlException
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return strArray2
        End Function

        Public Function GetDailyPayMasterDtl(ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New DailyPayMasterDtlMap
                map.ToPhysicalString("")
                Dim command As New NpgsqlCommand(("select " & map.ToPhysicalString("") & " from daily_pay_master_dtl where d_from <= :d_date and d_to >= :d_date order by c_daily_pay_id, c_menu_seq"), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "日当マスタ詳細の情報" })
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("daily_pay_master_dtl", dReader)
                Dim ds As New DataSet
                ds.Tables.Add(table)
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

        Public Function GetNotZeroYenDailyPay(ByVal strDailyPayId As String, ByVal nMenuSeq As Integer, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = _
					 "SELECT" & _
						 "daily_pay_master_A.c_daily_pay_id," & _
						 "daily_pay_master_A.d_from," & _
						 "daily_pay_master_A.l_name," & _
						 "daily_pay_master_dtl.c_menu_seq," & _
						 "daily_pay_master_dtl.s_daily_pay" & _
					 "FROM" & _
						 "daily_pay_master_dtl," & _
						 "(SELECT" & _
							 "c_daily_pay_id," & _
							 "d_from," & _
							 "l_name" & _
						 "FROM" & _
							 "daily_pay_master" & _
						 "WHERE" & _
							 "c_ksh = :c_ksh" & _
							 "AND c_daily_pay_id <> :c_daily_pay_id" & _
							 "AND d_to >= :d_date" & _
						 ") daily_pay_master_A" & _
					 "WHERE" & _
						 "daily_pay_master_dtl.c_menu_seq = :c_menu_seq" & _
						 "AND (daily_pay_master_dtl.s_daily_pay IS NOT NULL " & _
							 "AND daily_pay_master_dtl.s_daily_pay <> 0)" & _
						 "AND daily_pay_master_dtl.c_daily_pay_id = daily_pay_master_A.c_daily_pay_id" & _
						 "AND daily_pay_master_dtl.d_from = daily_pay_master_A.d_from;"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_daily_pay_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_menu_seq", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_daily_pay_id").Value = strDailyPayId
                command.Parameters.Item("c_menu_seq").Value = nMenuSeq
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("daily_pay_master_dtl", dReader))
                set2 = ds
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

    End Class
End Namespace
