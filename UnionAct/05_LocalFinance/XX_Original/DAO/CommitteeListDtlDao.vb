#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports System.Text
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.Mapping

Namespace DAO.Activity
    Public Class CommitteeListDtlDao
        Inherits AbstractDao
        'Implements ICommitteeListDtlDao
        ' Methods
        Public Function CreateCommitteeSectionList(ByVal Terminal As String, ByVal CompanyCode As String, ByVal PeriodId As String, ByVal GetFrom As String, ByVal UpUserId As String, ByVal Difference As Integer) As Integer
            Dim num As Integer
            Try 
                Dim cmdText As String = "CommitteeSectionListCreate(:Terminal, :CompanyCode, :PeriodId, :GetFrom, :UpUserId, :Difference)"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection) With { _
                    .CommandType = CommandType.StoredProcedure _
                }
                command.Parameters.Add(New NpgsqlParameter("Terminal", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("CompanyCode", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("PeriodId", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("GetFrom", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("UpUserId", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("Difference", DbType.Int32))
                command.Parameters.Item("Terminal").Value = Terminal
                command.Parameters.Item("CompanyCode").Value = CompanyCode
                command.Parameters.Item("PeriodId").Value = PeriodId
                command.Parameters.Item("GetFrom").Value = GetFrom
                command.Parameters.Item("UpUserId").Value = UpUserId
                command.Parameters.Item("Difference").Value = Difference
                Dim obj2 As Object = command.ExecuteScalar
                num = If(((obj2 Is Nothing) OrElse TypeOf obj2 Is DBNull), 0, CInt(obj2))
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0004", New String(0  - 1) {})
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0  - 1) {})
            End Try
            Return num
        End Function

        Public Sub DeleteData(ByVal strCommitteeList As String, ByVal strDFrom As String)
            Try 
                Dim cmdText As String = "delete from committee_list_dtl  where c_committee_list = :c_committee_list   and d_from           = :d_from "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_list", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("c_committee_list").Value = strCommitteeList
                command.Parameters.Item("d_from").Value = strDFrom
                command.ExecuteNonQuery
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
        End Sub

        'Public Function GetBelongCommittee(ByVal strKsh As String, ByVal strUserId As String, ByVal strDate As String) As DataTable
        '    Dim table2 As DataTable
        '    Try 
        '        dim map As New CommitteeListDtlMap
        '        map.ToPhysicalString(" c.")
        '        Dim cmdText As String = " SELECT  PER.d_from AS PERIOD_DFROM,  PER.d_to AS PERIOD_DTO,  COMDTL.s_from_diff,  COMDTL.s_to_diff,  COMLISTDTL.c_committee_list,  COMLISTDTL.c_user_id,  COMLISTDTL.d_from,  COMLISTDTL.c_committee_id,  COMLISTDTL.s_committee_seq,  COMLISTDTL.l_biko,  COMLISTDTL.d_ins,  COMLISTDTL.c_user_id_ins,  COMLISTDTL.d_up,  COMLISTDTL.c_user_id_up,  COMLISTDTL.s_up,  COMLIST_B.c_period_id  FROM    (select c_committee_id,                 c_period_id,                    max(d_from) as d_from    from committee_list             where c_ksh          = :c_ksh         and d_from    <= :d_to   group by c_committee_id, c_period_id ) COMLIST_A,  committee_list COMLIST_B,     committee_list_dtl COMLISTDTL,  committee_dtl COMDTL,  period PER where COMLIST_A.d_from           = COMLIST_B.d_from             and COMLIST_A.c_committee_id   = COMLIST_B.c_committee_id     and COMLIST_A.c_period_id      = COMLIST_B.c_period_id        and COMLIST_B.c_ksh            = :c_ksh   and COMLIST_B.c_committee_list = COMLISTDTL.c_committee_list   and COMLIST_B.d_from           = COMLISTDTL.d_from             and COMLISTDTL.c_user_id       = :c_user_id   AND COMLISTDTL.c_committee_id  = COMDTL.c_committee_id   AND COMLISTDTL.s_committee_seq = COMDTL.s_committee_seq   AND COMDTL.d_from             <= COMLIST_B.d_from   AND COMLIST_B.d_from          <= COMDTL.d_to   AND PER.c_period_id            = COMLIST_B.c_period_id   AND PER.k_period_kind          = :k_period_kind  order by COMLISTDTL.c_committee_id "
        '        Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
        '        command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("d_to", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("k_period_kind", DbType.String))
        '        command.Parameters.Item("c_ksh").Value = strKsh
        '        command.Parameters.Item("d_to").Value = strDate
        '        command.Parameters.Item("c_user_id").Value = strUserId
        '        command.Parameters.Item("k_period_kind").Value = "01"
        '        Dim dReader As NpgsqlDataReader = command.ExecuteReader
        '        table2 = MyBase.CreateSomeDataSet("committee_list_dtl", dReader)
        '    Catch exception As AppUnionException
        '        exception.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception
        '    Catch exception2 As SysUnionException
        '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception2
        '    Catch exception3 As NpgsqlException
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
        '    Catch exception4 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
        '    End Try
        '    Return table2
        'End Function

        'Public Function GetCommitteeMember(ByVal strPeriodId As String, ByVal strCommitteeId As String, ByVal strDate As String) As DataSet
        '    Dim set2 As DataSet
        '    Try 
        '        Dim str As String = If(String.IsNullOrEmpty(strCommitteeId), "", " AND c_committee_id = :c_committee_id ")
        '        Dim command As New NpgsqlCommand(("SELECT " & "	" & "committee_list_dtl_A.c_committee_id, " & "	" & "committee_list_dtl_A.s_committee_seq, " & "	" & "committee_list_dtl_A.c_committee_list, " & "	" & "staf_attribute_A.c_user_id, " & "	" & "staf_attribute_A.c_staf_id AS " & "é–àıî‘çÜ" & ", " & "	" & "TO_NUMBER(staf_attribute_A.c_staf_id, '9999999999') AS s_staf_id, " & "	" & "staf_attribute_A.l_name, " & "	" & "staf_attribute_A.l_name AS " & "ñºëO" & ", " & "	" & "staf_attribute_A.l_name_kna, " & "	" & "staf_attribute_A.k_belonging, " & "	" & "belonging_view_A.l_name AS " & "éxïî" & ", " & "	" & "(CASE WHEN " & "		" & "committee_list_dtl_A.c_committee_id = :executive_committee_id " & "		" & "AND committee_list_dtl_A.s_committee_seq = :executive_committee_seq " & "	" & "THEN " & "		" & "(SELECT  " & "			" & "(SUBSTRING( " & "				" & "committee_name.l_committee_name " & "			" & "FROM " & "				" & "1 " & "			" & "FOR " & "				" & "(CASE WHEN " & "					" & "POSITION('" & "ïî" & "' IN committee_name.l_committee_name) = 0 " & "				" & "THEN " & "					" & "CHAR_LENGTH(committee_name.l_committee_name) " & "				" & "ELSE " & "					" & "POSITION('" & "ïî" & "' IN committee_name.l_committee_name) - 1 " & "				" & "END) " & "			" & ") " & "			" & "|| " & "			" & "committee_name.l_post_name) " & "		" & "FROM " & "			" & "(SELECT " & "				" & "committee_dtl_B.c_committee_id, " & "				" & "committee_dtl_B.s_committee_seq, " & "				" & "committee_list_dtl_B.c_committee_list, " & "				" & "committee_list_dtl_B.c_user_id, " & "				" & "committee_dtl_B.l_committee_name, " & "				" & "committee_dtl_B.l_post_name " & "			" & "FROM " & "				" & "(SELECT " & "					" & "committee_list_B.c_committee_list, " & "					" & "committee_list_B.c_committee_id, " & "					" & "committee_list_dtl.s_committee_seq, " & "					" & "committee_list_dtl.c_user_id " & "				" & "FROM " & "					" & "committee_list_dtl, " & "					" & "(SELECT " & "						" & "committee_list.c_committee_list, " & "						" & "committee_list.d_from, " & "						" & "committee_list.c_committee_id " & "					" & "FROM " & "						" & "committee_list, " & "						" & "(SELECT " & "							" & "c_committee_list, " & "							" & "MAX(d_from) AS d_from " & "						" & "FROM " & "							" & "committee_list " & "						" & "WHERE " & "							" & "c_ksh = :c_ksh " & "							" & "AND c_period_id = :c_period_id " & "							" & "AND c_committee_id <> :executive_committee_id " & "							" & "AND d_from <= :d_date " & "						" & "GROUP BY " & "							" & "c_committee_list " & "						" & ") committee_list_MAX " & "					" & "WHERE " & "						" & "committee_list.c_committee_list = committee_list_MAX.c_committee_list " & "						" & "AND committee_list.d_from = committee_list_MAX.d_from " & "					" & ") committee_list_B " & "				" & "WHERE " & "					" & "committee_list_dtl.c_committee_list = committee_list_B.c_committee_list " & "					" & "AND committee_list_dtl.d_from = committee_list_B.d_from " & "				" & ") committee_list_dtl_B, " & "				" & "(SELECT " & "					" & "committee_B.c_committee_id, " & "					" & "committee_B.l_name AS l_committee_name, " & "					" & "committee_B.d_from, " & "					" & "committee_dtl.s_committee_seq, " & "					" & "committee_dtl.l_name AS l_post_name " & "				" & "FROM " & "					" & "committee_dtl, " & "					" & "(SELECT " & "						" & "c_committee_id, " & "						" & "d_from, " & "						" & "l_name " & "					" & "FROM " & "						" & "committee " & "					" & "WHERE " & "						" & "d_from <= :d_date " & "						" & "AND d_to >= :d_date " & "					" & ") committee_B " & "				" & "WHERE " & "					" & "committee_dtl.k_head_flg = '1' " & "					" & "AND committee_dtl.c_committee_id = committee_B.c_committee_id " & "					" & "AND committee_dtl.d_from = committee_B.d_from " & "				" & ") committee_dtl_B " & "			" & "WHERE " & "				" & "committee_list_dtl_B.c_committee_id = committee_dtl_B.c_committee_id " & "				" & "AND committee_list_dtl_B.s_committee_seq = committee_dtl_B.s_committee_seq " & "				" & "AND committee_list_dtl_B.c_user_id = staf_attribute_A.c_user_id " & "			" & ") committee_name " & "		" & "ORDER BY " & "			" & "committee_name.c_committee_id, " & "			" & "committee_name.s_committee_seq " & "		" & "LIMIT " & "			" & "1 " & "		" & ") " & "	" & "ELSE " & "		" & "committee_dtl_A.l_name " & "	" & "END) AS " & "ñêE" & ", " & "	" & "committee_dtl_A.k_head_flg FROM " & "	" & "(SELECT " & "		" & "committee_list_dtl.c_user_id, " & "		" & "committee_list_A.c_committee_id, " & "		" & "committee_list_dtl.s_committee_seq, " & "		" & "committee_list_A.c_committee_list " & "	" & "FROM " & "		" & "committee_list_dtl, " & "		" & "(SELECT " & "			" & "committee_list.* " & "		" & "FROM " & "			" & "committee_list, " & "			" & "(SELECT " & "				" & "c_committee_list, " & "				" & "MAX(d_from) AS d_from " & "			" & "FROM " & "				" & "committee_list " & "			" & "WHERE " & "				" & "c_ksh = :c_ksh " & "				" & "AND c_period_id = :c_period_id " & str & "				" & "AND d_from <= :d_date " & "			" & "GROUP BY " & "				" & "c_committee_list " & "			" & ") committee_list_MAX " & "		" & "WHERE " & "			" & "committee_list.c_committee_list = committee_list_MAX.c_committee_list " & "			" & "AND committee_list.d_from = committee_list_MAX.d_from " & "		" & ") committee_list_A " & "	" & "WHERE " & "		" & "committee_list_dtl.c_committee_list = committee_list_A.c_committee_list " & "		" & "AND committee_list_dtl.d_from = committee_list_A.d_from " & "	" & ") committee_list_dtl_A, " & "	" & "(SELECT " & "		" & "staf_attribute.c_user_id, " & "		" & "staf_attribute.c_staf_id, " & "		" & "staf_attribute.l_name, " & "		" & "staf_attribute.l_name_kna, " & "		" & "staf_attribute.k_belonging " & "	" & "FROM " & "		" & "staf_attribute, " & "		" & "(SELECT " & "			" & "c_user_id, " & "			" & "MAX(d_from) AS d_from " & "		" & "FROM " & "			" & "staf_attribute " & "		" & "WHERE " & "			" & "d_from <= :d_date " & "		" & "GROUP BY " & "			" & "c_user_id " & "		" & ") staf_attribute_MAX " & "	" & "WHERE " & "		" & "staf_attribute.c_user_id = staf_attribute_MAX.c_user_id " & "		" & "AND staf_attribute.d_from = staf_attribute_MAX.d_from " & "	" & ") staf_attribute_A, " & "	" & "(SELECT " & "		" & "c_committee_id, " & "		" & "s_committee_seq, " & "		" & "l_name, " & "		" & "k_head_flg " & "	" & "FROM " & "		" & "committee_dtl " & "	" & "WHERE " & "		" & "d_from <= :d_date " & "		" & "AND d_to >= :d_date " & "	" & ") committee_dtl_A, " & "	" & "(SELECT " & "		" & "c_constant_seq, " & "		" & "l_name " & "	" & "FROM " & "		" & "belonging_view " & "	" & "WHERE " & "		" & "d_from <= :d_date " & "		" & "AND d_to >= :d_date " & "	" & ") belonging_view_A WHERE " & "	" & "committee_list_dtl_A.c_user_id = staf_attribute_A.c_user_id " & "	" & "AND committee_list_dtl_A.c_committee_id = committee_dtl_A.c_committee_id " & "	" & "AND committee_list_dtl_A.s_committee_seq = committee_dtl_A.s_committee_seq " & "	" & "AND belonging_view_A.c_constant_seq = staf_attribute_A.k_belonging ORDER BY " & "	" & "committee_list_dtl_A.c_committee_id, " & "	" & "committee_list_dtl_A.s_committee_seq, " & "	" & "TO_NUMBER(staf_attribute_A.c_staf_id, '9999999999') "), MyBase.GetNpgsqlConnection)
        '        command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("executive_committee_id", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("executive_committee_seq", DbType.Int32))
        '        command.Parameters.Add(New NpgsqlParameter("head_committee_seq", DbType.Int32))
        '        command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
        '        command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
        '        command.Parameters.Item("c_period_id").Value = strPeriodId
        '        command.Parameters.Item("c_committee_id").Value = strCommitteeId
        '        command.Parameters.Item("executive_committee_id").Value = "001"
        '        command.Parameters.Item("executive_committee_seq").Value = 5
        '        command.Parameters.Item("head_committee_seq").Value = 1
        '        command.Parameters.Item("d_date").Value = strDate
        '        Dim dReader As NpgsqlDataReader = command.ExecuteReader
        '        If Not dReader.HasRows Then
        '            Return Nothing
        '        End If
        '        Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list_dtl", dReader)
        '        Dim ds As New DataSet
        '        ds.Tables.Add(table)
        '        set2 = ds
        '    Catch exception As NpgsqlException
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0  - 1) {})
        '    Catch exception2 As AppUnionException
        '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception2
        '    Catch exception3 As SysUnionException
        '        exception3.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception3
        '    Catch exception4 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
        '    End Try
        '    Return set2
        'End Function

        'Public Function GetCommitteeSectionList(ByVal strMac As String) As DataTable
        '    Dim table As DataTable
        '    Dim cmdText As String = ""
        '    Try 
        '        dim map As New SpecialCommitteeSectionListReportMap
        '        cmdText = "select l_committee," & "		" & "case when k_branch_officer_pay = '1' then '*' end as k_branch_officer_pay," & "		" & "c_staf_id_officer_pay," & "		" & "l_name_officer_pay," & "		" & "l_model_1," & "		" & "case when k_branch_1 = '1' then '*' end as k_branch_1," & "		" & "c_staf_id_1," & "		" & "l_staf_name_1," & "		" & "l_model_2," & "		" & "case when k_branch_2 = '1' then '*' end as k_branch_2," & "		" & "c_staf_id_2," & "		" & "l_staf_name_2," & "		" & "l_model_3," & "		" & "case when k_branch_3 = '1' then '*' end as k_branch_3," & "		" & "c_staf_id_3," & "		" & "l_staf_name_3," & "		" & "l_model_4," & "		" & "case when k_branch_4 = '1' then '*' end as k_branch_4," & "		" & "c_staf_id_4," & "		" & "l_staf_name_4," & "		" & "l_model_5," & "		" & "case when k_branch_5 = '1' then '*' end as k_branch_5," & "		" & "c_staf_id_5," & "		" & "l_staf_name_5 from committee_section_work where c_mac = :c_mac order by s_seq"
        '        Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
        '        command.Parameters.Add(New NpgsqlParameter("c_mac", DbType.String))
        '        command.Parameters.Item("c_mac").Value = strMac
        '        Dim dReader As NpgsqlDataReader = command.ExecuteReader
        '        If Not dReader.HasRows Then
        '            Return Nothing
        '        End If
        '        table = MyBase.CreateSomeDataSet("dtDetail", dReader)
        '    Catch exception As AppUnionException
        '        exception.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception
        '    Catch exception2 As SysUnionException
        '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception2
        '    Catch exception3 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
        '    End Try
        '    Return table
        'End Function

        Public Function GetCommitteeSeq(ByVal strCommitteeId As String, ByVal strUserId As String, ByVal strDate As String) As Integer
            Dim num As Integer
            Try 
                Dim cmdText As String = _
					 "SELECT " & _
						 "committee_list_dtl.c_committee_list, " & _
						 "committee_list_dtl.c_user_id, " & _
						 "committee_list_dtl.c_committee_id, " & _
						 "committee_list_dtl.s_committee_seq " & _
					 "FROM " & _
						 "committee_list_dtl, " & _
						 "(SELECT " & _
							 "committee_list.c_committee_list, " & _
							 "committee_list.c_committee_id " & _
						 "FROM " & _
							 "committee_list, " & _
							 "(SELECT " & _
								 "c_committee_list, " & _
								 "MAX(committee_list.d_from) AS d_from " & _
							 "FROM " & _
								"committee_list " & _
							 "WHERE " & _
								 "c_committee_id = :c_committee_id " & _
								 "AND c_ksh = :c_ksh " & _
								 "AND d_from <= :d_date " & _
							 "GROUP BY " & _
								"c_committee_list " & _
							 ") committee_list_MAX " & _
						 "WHERE " & _
							 "committee_list.c_committee_list = committee_list_MAX.c_committee_list " & _
							 "AND committee_list.d_from = committee_list_MAX.d_from " & _
						 ") committee_list_A " & _
					 "WHERE " & _
						 "committee_list_dtl.c_committee_list = committee_list_A.c_committee_list " & _
						 "AND committee_list_dtl.c_user_id = :c_user_id " & _
						 "AND committee_list_dtl.c_committee_id = committee_list_A.c_committee_id " & _
						 "AND d_from <= :d_date "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return -1
                End If
                num = Integer.Parse(MyBase.CreateSomeDataSet("committee_list_dtl", dReader).Rows.Item(0).Item("s_committee_seq").ToString)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return num
        End Function

        'Public Function GetData(ByVal strKsh As String, ByVal strPeriodId As String, ByVal strUserId As String, ByVal strDate As String) As DataSet
        '    Dim set2 As DataSet
        '    Try 
        '        Dim table As DataTable
        '        Dim str2 As String = New CommitteeListDtlMap().ToPhysicalString(" c.")
        '        Dim str As String = ("select " & str2 & " from  (select c_committee_id,                       max(d_from) as d_from          from committee_list                   where c_ksh          = :c_ksh       and c_period_id        = :c_period_id ")
        '        If Not strDate.Equals("") Then
        '            str = (str & "and d_from            <= :d_to ")
        '        End If
        '        Dim command As New NpgsqlCommand((str & "group by c_committee_id ) a, committee_list b,    committee_list_dtl c where a.d_from         = b.d_from           and a.c_committee_id   = b.c_committee_id   and b.c_ksh            = :c_ksh and b.c_period_id      = :c_period_id and b.c_committee_list = c.c_committee_list and b.d_from           = c.d_from           and c.c_user_id        = :c_user_id         order by c.c_committee_id " & UtDb.DbOrderOffset ), MyBase.GetNpgsqlConnection)
        '        command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("d_to", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
        '        command.Parameters.Item("c_ksh").Value = strKsh
        '        command.Parameters.Item("c_period_id").Value = strPeriodId
        '        command.Parameters.Item("d_to").Value = strDate
        '        command.Parameters.Item("c_user_id").Value = strUserId
        '        Dim dReader As NpgsqlDataReader = command.ExecuteReader
        '        If Not dReader.HasRows Then
        '            Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0006", New String(0  - 1) {})
        '        End If
        '        If Not strDate.Equals("") Then
        '            table = MyBase.CreateSomeDataSet("committee_list_dtl", dReader)
        '        Else
        '            table = MyBase.CreateSomeDataSet("committee_list_dtl_period", dReader)
        '        End If
        '        Dim ds As New DataSet
        '        ds.Tables.Add(table)
        '        set2 = ds
        '    Catch exception As AppUnionException
        '        exception.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception
        '    Catch exception2 As SysUnionException
        '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception2
        '    Catch exception3 As NpgsqlException
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0  - 1) {})
        '    Catch exception4 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
        '    End Try
        '    Return set2
        'End Function

        Public Function GetMaxDifference(ByVal strDate As String) As Integer
            Dim num2 As Integer
            Try 
                Dim cmdText As String = "select max(s_to_diff) as s_to_diff from committee_dtl where d_from <= :strDate and d_to     >= :strDate"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
                command.Parameters.Item("strDate").Value = strDate
                Dim reader As NpgsqlDataReader = command.ExecuteReader
                If Not reader.HasRows Then
                    Return 0
                End If
                Dim num As Integer = 0
                Do While reader.Read
                    num = Convert.ToInt32(reader.Item(0))
                Loop
                num2 = num
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return num2
        End Function

        Public Function GetOtherHead(ByVal strPeriodId As String, ByVal strCommitteeId As String, ByVal strUserId As String, ByVal strDFrom As String) As String
            Dim str2 As String
            Try 
                Dim cmdText As String = "SELECT " & "	" & "committee_dtl_A.c_committee_id, " & "	" & "committee_dtl_A.s_committee_seq, " & "	" & "committee_list_dtl_A.c_committee_list, " & "	" & "committee_list_dtl_A.c_user_id, " & "	" & "committee_dtl_A.l_committee_name, " & "	" & "committee_dtl_A.l_post_name FROM " & "	" & "(SELECT " & "		" & "committee_list_A.c_committee_list, " & "		" & "committee_list_A.c_committee_id, " & "		" & "committee_list_dtl.s_committee_seq, " & "		" & "committee_list_dtl.c_user_id " & "	" & "FROM " & "		" & "committee_list_dtl, " & "		" & "(SELECT " & "			" & "committee_list.c_committee_list, " & "			" & "committee_list.d_from, " & "			" & "committee_list.c_committee_id " & "		" & "FROM " & "			" & "committee_list, " & "			" & "(SELECT " & "				" & "c_committee_list, " & "				" & "MAX(d_from) AS d_from " & "			" & "FROM " & "				" & "committee_list " & "			" & "WHERE " & "				" & "c_ksh = :c_ksh " & "				" & "AND c_period_id = :c_period_id " & "				" & "AND d_from <= :d_from " & "			" & "GROUP BY " & "				" & "c_committee_list " & "			" & ") committee_list_MAX " & "		" & "WHERE " & "			" & "committee_list.c_committee_list = committee_list_MAX.c_committee_list " & "			" & "AND committee_list.d_from = committee_list_MAX.d_from " & "		" & ") committee_list_A " & "	" & "WHERE " & "		" & "committee_list_dtl.c_committee_list = committee_list_A.c_committee_list " & "		" & "AND committee_list_dtl.d_from = committee_list_A.d_from " & "		" & "AND committee_list_dtl.c_user_id = :c_user_id " & "	" & ") committee_list_dtl_A, " & "	" & "(SELECT " & "		" & "committee_A.c_committee_id, " & "		" & "committee_A.l_name AS l_committee_name, " & "		" & "committee_A.d_from, " & "		" & "committee_dtl.s_committee_seq, " & "		" & "committee_dtl.l_name AS l_post_name " & "	" & "FROM " & "		" & "committee_dtl, " & "		" & "(SELECT " & "			" & "c_committee_id, " & "			" & "d_from, " & "			" & "l_name " & "		" & "FROM " & "			" & "committee " & "		" & "WHERE " & "			" & "c_committee_id <> :c_committee_id " & "			" & "AND d_from <= :d_from " & "			" & "AND d_to >= :d_from " & "		" & ") committee_A " & "	" & "WHERE " & "		" & "committee_dtl.k_head_flg = '1' " & "		" & "AND committee_dtl.c_committee_id = committee_A.c_committee_id " & "		" & "AND committee_dtl.d_from = committee_A.d_from " & "	" & ") committee_dtl_A WHERE " & "	" & "committee_list_dtl_A.c_committee_id = committee_dtl_A.c_committee_id " & "	" & "AND committee_list_dtl_A.s_committee_seq = committee_dtl_A.s_committee_seq "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("d_from").Value = strDFrom
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list_dtl", dReader)
                If (table.Rows.Count = 0) Then
                    Return Nothing
                End If
                Dim builder As New StringBuilder(("Åi" & table.Rows.Item(0).Item("l_committee_name").ToString & table.Rows.Item(0).Item("l_post_name").ToString))
                Dim i As Integer
                For i = 1 To table.Rows.Count - 1
                    builder.Append((", " & table.Rows.Item(i).Item("l_committee_name").ToString & table.Rows.Item(i).Item("l_post_name").ToString))
                Next i
                builder.Append("Åj")
                str2 = builder.ToString
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
            Return str2
        End Function

        Public Overloads Sub InsertData(ByVal dTable As DataTable)
            Try
                Dim map As New CommitteeListDtlMap
                Dim str2 As String = map.ToPhysicalString("")
                Dim str3 As String = map.ToPhysicalString(":")
                Dim command As New NpgsqlCommand(String.Concat(New String() {"insert into committee_list_dtl( ", str2, " ) values( ", str3, " ) "}), MyBase.GetNpgsqlConnection, MyBase.GetNpgsqlTran)
                Dim i As Integer
                For i = 0 To map.ColumnCount - 1
                    command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(i), map.GetDbDataType(i)))
                    command.Parameters.Item(map.GetPhysicalName(i)).Value = dTable.Rows.Item(0).Item(map.GetPhysicalName(i))
                Next i
                command.ExecuteNonQuery()
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Function SelectForUpData(ByVal strCommitteeList As String, ByVal strUserId As String, ByVal strDFrom As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "select d_up from committee_list_dtl  where c_committee_list = :c_committee_list   and c_user_id        = :c_user_id   and d_from           = :d_from for update "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_list", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("c_committee_list").Value = strCommitteeList
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("d_from").Value = strDFrom
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list_dtl", dReader)
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Sub UpdateData(ByVal strCommitteeId As String, ByVal strCommitteeSeq As Integer, ByVal strUserIdUp As String, ByVal strCommitteeList As String, ByVal strUserId As String, ByVal strDFrom As String)
            Try 
                Dim cmdText As String = "update committee_list_dtl   set c_committee_id   = :c_committee_id  ,      s_committee_seq  = :s_committee_seq ,      d_up             = :d_up ,      c_user_id_up     = :c_user_id_up    ,      s_up             = s_up + 1 where c_committee_list = :c_committee_list   and c_user_id        = :c_user_id   and d_from           = :d_from "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_up", DbType.DateTime))
                command.Parameters.Add(New NpgsqlParameter("c_user_id_up", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_list", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("s_committee_seq").Value = strCommitteeSeq
                command.Parameters.Item("d_up").Value = PublicCommand.GetNow
                command.Parameters.Item("c_user_id_up").Value = strUserIdUp
                command.Parameters.Item("c_committee_list").Value = strCommitteeList
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("d_from").Value = strDFrom
                If (command.ExecuteNonQuery < 1) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0005", New String(0  - 1) {})
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0  - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
        End Sub

    End Class
End Namespace
