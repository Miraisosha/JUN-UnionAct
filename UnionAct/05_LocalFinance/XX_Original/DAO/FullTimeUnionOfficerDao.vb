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

Namespace DAO.FullTimeUnionOfficer
    'Public Class FullTimeUnionOfficerDao
    '    Inherits AbstractDao
    '    'Implements IFullTimeUnionOfficerDao
    '    ' Methods
    '    Public Function CheckConstSalary(ByVal strDate As String, ByVal strUserId As String) As Boolean
    '        Dim flag As Boolean
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select c_constant,c_constant_seq ")
    '            builder.Append(" from constant_dtl ")
    '            builder.Append(" where c_constant = 'OFFICER_SALARY' ")
    '            builder.Append(" and d_from <= :strDate ")
    '            builder.Append(" and d_to >= :strDate ")
    '            builder.Append(" and c_constant_seq = ( ")
    '            builder.Append(" select full_time_union_officer_info.k_officer_salary ")
    '            builder.Append(" from ( ")
    '            builder.Append("   select max(d_from) as d_from, c_user_id ")
    '            builder.Append("     from full_time_union_officer_info ")
    '            builder.Append("     where c_user_id = :strUserId ")
    '            builder.Append("     and substring(d_from,1,6) <= substring(:strDate,1,6) ")
    '            builder.Append("   GROUP BY c_user_id ")
    '            builder.Append("   ) max_union, ")
    '            builder.Append("   full_time_union_officer_info ")
    '            builder.Append(" where ")
    '            builder.Append("      full_time_union_officer_info.c_user_id = max_union.c_user_id ")
    '            builder.Append("  AND full_time_union_officer_info.d_from = max_union.d_from ")
    '            builder.Append(" ) ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_salary", dReader))
    '            If (ds.Tables.Item(0).Rows.Count > 0) Then
    '                Return True
    '            End If
    '            flag = False
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return flag
    '    End Function

    '    Public Sub DeleteSalaryPersonalDtl(ByVal strUserId As String, ByVal strDate As String)
    '        Try
    '            Dim cmdText As String = "delete from full_time_salary_personal_dtl where c_salary_item IN ( SELECT c_salary_item FROM salary_item_details WHERE k_deduction_put <> '1'  and d_from <= :strDate and d_to >= :strDate ) and d_salary_pay = :strDate and c_user_id = :strUserId "
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add("strUserId", strUserId)
    '            command.Parameters.Add("strDate", strDate)
    '            command.ExecuteReader()
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '    End Sub

    '    Public Sub DeleteSalaryPersonalDtlTemp(ByVal strUserId As String, ByVal strDate As String)
    '        Try
    '            Dim cmdText As String = "delete from full_time_temporary_salary_dtl where d_salary_pay = :strDate and c_user_id = :strUserId "
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add("strUserId", strUserId)
    '            command.Parameters.Add("strDate", strDate)
    '            command.ExecuteReader()
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '    End Sub

    '    Public Function GetFullTimeTempBillData(ByVal strDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            If (strUserId = "") Then
    '                builder.Append(" c_user_id, ")
    '            End If
    '            builder.Append(" l_salary_item as " & "â»ñ⁄" & ", ")
    '            builder.Append(" s_item as " & "äz" & " ")
    '            builder.Append(" from  ")
    '            builder.Append(" full_time_temporary_salary_dtl ")
    '            builder.Append(" where  ")
    '            builder.Append(" k_salary_item_classify = '01' and ")
    '            If (strUserId <> "") Then
    '                builder.Append(" c_user_id = :strUserId and ")
    '            End If
    '            builder.Append(" d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("SalaryBillTemp", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetFullTimeTempKoujoData(ByVal strDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            If (strUserId = "") Then
    '                builder.Append(" c_user_id, ")
    '            End If
    '            builder.Append(" l_salary_item as " & "â»ñ⁄" & ", ")
    '            builder.Append(" s_item as " & "äz" & " ")
    '            builder.Append(" from  ")
    '            builder.Append(" full_time_temporary_salary_dtl ")
    '            builder.Append(" where  ")
    '            builder.Append(" k_salary_item_classify = '02' and ")
    '            If (strUserId <> "") Then
    '                builder.Append(" c_user_id = :strUserId and ")
    '            End If
    '            builder.Append(" d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("SalaryKoujoTemp", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetFullTimeTempOtherData(ByVal strDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            If (strUserId = "") Then
    '                builder.Append(" c_user_id, ")
    '            End If
    '            builder.Append(" l_salary_item as " & "â»ñ⁄" & ", ")
    '            builder.Append(" s_item as " & "äz" & " ")
    '            builder.Append(" from  ")
    '            builder.Append(" full_time_temporary_salary_dtl ")
    '            builder.Append(" where  ")
    '            builder.Append(" k_salary_item_classify = '03' and ")
    '            If (strUserId <> "") Then
    '                builder.Append(" c_user_id = :strUserId and ")
    '            End If
    '            builder.Append(" d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("SalaryOtherTemp", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetFUllTimeUnionOfficerAttribute(ByVal strKsh As String, ByVal strUserID As String, ByVal strDateFrom As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim map As New FullTimeUnionOfficerMap
    '            Dim command As New NpgsqlCommand(("select " & map.ToPhysicalString("FULL_INFO.") & " from (select c_user_id, max(d_from) as d_from from full_time_union_officer where c_ksh = :c_ksh and k_del = :k_del_off and c_user_id = :c_user_id and d_from <= :d_from group by c_user_id ) FULL_MAX, full_time_union_officer FULL_INFO where FULL_MAX.c_user_id = FULL_INFO.c_user_id and FULL_MAX.d_from = FULL_INFO.d_from "), MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add("c_ksh", strKsh)
    '            command.Parameters.Add("k_del_off", "0")
    '            command.Parameters.Add("c_user_id", strUserID)
    '            command.Parameters.Add("d_from", strDateFrom)
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetFullTimeUnionOfficerForUpdate(ByVal strUserID As String, ByVal strDateFrom As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim cmdText As String = "select d_ins, d_up from full_time_union_officer where c_user_id = :c_user_id and d_from = :d_from for update "
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add("c_user_id", strUserID)
    '            command.Parameters.Add("d_from", strDateFrom)
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetFullTimeUnionOfficerHistoryList(ByVal strKsh As String, ByVal strUserID As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim cmdText As String = "select FULL_TIME.c_user_id, to_date(FULL_TIME.d_from, 'yyyyMMdd') as " & "ìKópì˙ït" & ", FULL_VIEW.l_name as " & "íSìñé“" & " from full_time_union_officer FULL_TIME left outer join (select FULL_STAF_INFO.c_user_id, FULL_STAF_INFO.d_from, FULL_STAF_INFO.l_name from (select c_user_id, max(d_from) as d_from from staf_attribute_full_time_view where c_ksh = :c_ksh and d_from <= :date_today group by c_user_id ) STAF_MAX, staf_attribute_full_time_view FULL_STAF_INFO where FULL_STAF_INFO.c_user_id = STAF_MAX.c_user_id and FULL_STAF_INFO.d_from = STAF_MAX.d_from) FULL_VIEW on COALESCE(FULL_TIME.c_user_id_up, FULL_TIME.c_user_id_ins) = FULL_VIEW.c_user_id where FULL_TIME.c_user_id = :c_user_id and FULL_TIME.c_ksh = :c_ksh and FULL_TIME.k_del = :k_del_off order by to_number(FULL_TIME.d_from, '99999999') desc "
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add("c_user_id", strUserID)
    '            command.Parameters.Add("c_ksh", strKsh)
    '            command.Parameters.Add("date_today", PublicCommand.GetSystemDate)
    '            command.Parameters.Add("k_del_off", "0")
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetFullTimeUnionOfficerNewUserID() As String
    '        Dim str2 As String
    '        Try
    '            Dim cmdText As String = "select lpad(lpad(to_number(substr(max(c_user_id), 2, 6), '999999') + 1, 6, '0'), 7, 'A') from full_time_union_officer where c_user_id like 'A%' "
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            str2 = command.ExecuteScalar.ToString
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return str2
    '    End Function

    '    Public Function GetFullTimeUnionOfficerSearchResult(ByVal strKsh As String, ByVal strNameKna As String, ByVal strBelong As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim command As New NpgsqlCommand(("select FULL_INFO.c_user_id, FULL_INFO.c_staf_id as " & "îFèÿî‘çÜ" & ", FULL_INFO.d_from, FULL_INFO.k_officer_status, FULL_INFO.l_name as " & "ñºëO" & ", BELONG.l_name as " & "ëgçáéxïî" & ", FULL_INFO.d_enter as " & "ì¸é–îNåéì˙" & ", (case when FULL_INFO.k_sex = :sex_male then '" & "íj" & "' else '" & "èó" & "' end) as " & "ê´ï " & ", FULL_INFO.l_prefectures || FULL_INFO.l_cities || (case when FULL_INFO.l_add_ather is null then '' else FULL_INFO.l_add_ather end) || (case when FULL_INFO.l_building is null then '' else FULL_INFO.l_building end) as " & "èZèä" & ", FULL_INFO.d_retire as " & "ëﬁêEîNåéì˙" & ", (case when :date_today < FULL_INFO.d_from then true else false end) as " & "ñ¢óàì˙ÉfÅ[É^ÉtÉâÉO" & " from (select FULL_ORIGINAL.* from (select c_user_id, max(d_from) as d_from from full_time_union_officer where k_del = :k_del_off and c_ksh = :c_ksh group by c_user_id ) FULL_MAX, full_time_union_officer FULL_ORIGINAL where FULL_ORIGINAL.c_user_id = FULL_MAX.c_user_id and FULL_ORIGINAL.c_user_id <> :user_id_sys and FULL_ORIGINAL.d_from = FULL_MAX.d_from " & If(String.IsNullOrEmpty(strNameKna), "", (" and FULL_ORIGINAL.l_name_kna like '%" & strNameKna & "%' ")) & If(String.IsNullOrEmpty(strBelong), "", (" and FULL_ORIGINAL.k_belonging = '" & strBelong & "' ")) & ") FULL_INFO left outer join belonging_view BELONG on BELONG.c_constant_seq = FULL_INFO.k_belonging and BELONG.d_from <= FULL_INFO.d_from and FULL_INFO.d_from <= BELONG.d_to order by FULL_INFO.c_staf_id "), MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add("sex_male", "0")
    '            command.Parameters.Add("k_del_off", "0")
    '            command.Parameters.Add("c_ksh", strKsh)
    '            command.Parameters.Add("user_id_sys", "A000001")
    '            command.Parameters.Add("date_today", PublicCommand.GetSystemDate)
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetFullTimeYearTaxData(ByVal strDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select sum(s_person) as " & "îNä‘ê≈äz" & " from( ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & "s_person ")
    '            builder.Append(" " & "		" & "from full_time_salary_personal_dtl ")
    '            builder.Append(" " & "		" & "where c_salary_item = '011' ")
    '            builder.Append(" " & "		" & "and substr(d_salary_pay,1,4) = substr(:strDate,1,4) ")
    '            builder.Append(" " & "		" & "and substr(d_salary_pay,5,8) <> '1201' ")
    '            builder.Append(" " & "		" & "and c_user_id = :strUserId ")
    '            builder.Append(" " & "				" & "union All ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & "s_person ")
    '            builder.Append(" " & "		" & "from full_time_bonus_personal_dtl ")
    '            builder.Append(" " & "		" & "where c_salary_item = '011' ")
    '            builder.Append(" " & "		" & "and substr(d_salary_pay,1,4) = substr(:strDate,1,4) ")
    '            builder.Append(" " & "		" & "and c_user_id = :strUserId) union_all ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("TaxEndYear", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetHasSameStafID(ByVal strUserID As String, ByVal strStafID As String) As Boolean
    '        Dim hasRows As Boolean
    '        Try
    '            Dim command As New NpgsqlCommand(("select c_user_id from full_time_union_officer where c_staf_id = :c_staf_id " & If(String.IsNullOrEmpty(strUserID), "", "and c_user_id <> :c_user_id ")), MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add("c_staf_id", strStafID)
    '            command.Parameters.Add("c_user_id", strUserID)
    '            hasRows = command.ExecuteReader.HasRows
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return hasRows
    '    End Function

    '    Public Function GetOnlyBonusUnionOfficerNameAndDtl(ByVal strUserId As String, ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select ")
    '            builder.Append(" " & "	" & "union_all.c_user_id as " & "é–àıî‘çÜ" & " , ")
    '            builder.Append(" " & "	" & "union_all.l_name as " & "ñºëO" & ", ")
    '            builder.Append(" " & "	" & "union_all.d_from as " & "ìKâûì˙" & " from ( ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & " union_A.* ")
    '            builder.Append(" " & "	" & "from full_time_union_officer union_A,")
    '            builder.Append("				" & "(select ")
    '            builder.Append("              Officer.c_user_id, ")
    '            builder.Append("              max(Officer.d_from) as d_from ")
    '            builder.Append("              from full_time_union_officer Officer,full_time_bonus_personal sala_P ")
    '            builder.Append(" " & "	" & "  WHERE ")
    '            builder.Append(" " & "		" & "sala_P.c_user_id = Officer.c_user_id ")
    '            builder.Append(" " & "		" & "and  Officer.c_user_id  = :c_user_id")
    '            builder.Append(" " & "		" & "and substring(sala_P.d_salary_pay,1,6) = substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "and substring(Officer.d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append("              group by Officer.c_user_id ")
    '            builder.Append("                     ) union_B ")
    '            builder.Append(" " & "	" & "where union_A.c_user_id = union_B.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A.c_ksh = :strKch  ")
    '            builder.Append(" " & "	" & "and union_A.k_del = '0' ")
    '            builder.Append(" " & "	" & "and union_A.d_from = union_B.d_from ) union_all, ")
    '            builder.Append(" " & "	" & "( ")
    '            builder.Append(" " & "		" & "select ")
    '            builder.Append(" " & "		" & "union_A_info.* ")
    '            builder.Append(" " & "		" & "from full_time_union_officer_info union_A_info, ( ")
    '            builder.Append(" " & "			" & "select ")
    '            builder.Append(" " & "			" & "c_user_id, max(d_from) as d_from ")
    '            builder.Append(" " & "			" & "from full_time_union_officer_info ")
    '            builder.Append(" " & "			" & "WHERE substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "group by c_user_id ) union_B_info ")
    '            builder.Append(" " & "	" & "where union_A_info.c_user_id = union_B_info.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A_info.d_from = union_B_info.d_from  ")
    '            builder.Append(" " & "	" & "and k_officer_salary is not null) union_all_info ")
    '            builder.Append(" where (union_all.d_retire is NULL ")
    '            builder.Append(" " & "	" & "or to_date(:strDate,'yyyyMMdd') <= d_retire ) ")
    '            builder.Append(" and union_all.c_user_id <> 'A000001' ")
    '            builder.Append(" and union_all_info.c_user_id =  union_all.c_user_id ")
    '            builder.Append(" order by union_all.c_user_id ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strKch", DbType.String))
    '            command.Parameters.Item("c_user_id").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strKch").Value = PublicCommand.GetKsh
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetOnlyUnionOfficerName(ByVal strUserId As String, ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select ")
    '            builder.Append(" " & "	" & "union_all.c_user_id as " & "é–àıî‘çÜ" & " , ")
    '            builder.Append(" " & "	" & "union_all.l_name as " & "ñºëO" & ", ")
    '            builder.Append(" " & "	" & "union_all.d_from as " & "ìKâûì˙" & " from ( ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & " union_A.* ")
    '            builder.Append(" " & "	" & "from full_time_union_officer union_A, ( ")
    '            builder.Append(" " & "		" & "select ")
    '            builder.Append(" " & "		" & " c_user_id, ")
    '            builder.Append(" " & "		" & "max(d_from) as d_from ")
    '            builder.Append(" " & "		" & "from full_time_union_officer ")
    '            builder.Append(" " & "		" & "where substring(d_from, 1, 6) <= substring(:strDate, 1, 6)")
    '            builder.Append("         and c_user_id = :c_user_id ")
    '            builder.Append(" " & "		" & "group by c_user_id ) union_B ")
    '            builder.Append(" " & "	" & "where union_A.c_user_id = union_B.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A.c_ksh = :strKch  ")
    '            builder.Append(" " & "	" & "and union_A.k_del = '0'  ")
    '            builder.Append(" " & "	" & "and union_A.d_from = union_B.d_from ) union_all, ")
    '            builder.Append(" " & "	" & "( ")
    '            builder.Append(" " & "		" & "select ")
    '            builder.Append(" " & "		" & "union_A_info.* ")
    '            builder.Append(" " & "		" & "from full_time_union_officer_info union_A_info, ( ")
    '            builder.Append(" " & "			" & "select ")
    '            builder.Append(" " & "			" & "c_user_id, max(d_from) as d_from ")
    '            builder.Append(" " & "			" & "from full_time_union_officer_info ")
    '            builder.Append(" " & "			" & "WHERE substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "group by c_user_id ) union_B_info ")
    '            builder.Append(" " & "	" & "where union_A_info.c_user_id = union_B_info.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A_info.d_from = union_B_info.d_from  ")
    '            builder.Append(" " & "	" & "and k_officer_salary is not null) union_all_info ")
    '            builder.Append(" where (union_all.d_retire is NULL ")
    '            builder.Append(" " & "	" & "or to_date(:strDate,'yyyyMMdd') <= d_retire ) ")
    '            builder.Append(" and union_all.c_user_id <> 'A000001' ")
    '            builder.Append(" and union_all_info.c_user_id =  union_all.c_user_id ")
    '            builder.Append(" order by union_all.c_user_id ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strKch", DbType.String))
    '            command.Parameters.Item("c_user_id").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strKch").Value = PublicCommand.GetKsh
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetOnlyUnionOfficerNameAndDtl(ByVal strUserId As String, ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select ")
    '            builder.Append(" " & "	" & "union_all.c_user_id as " & "é–àıî‘çÜ" & " , ")
    '            builder.Append(" " & "	" & "union_all.l_name as " & "ñºëO" & ", ")
    '            builder.Append(" " & "	" & "union_all.d_from as " & "ìKâûì˙" & " from ( ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & " union_A.* ")
    '            builder.Append(" " & "	" & "from full_time_union_officer union_A,")
    '            builder.Append("				" & "(select ")
    '            builder.Append("              Officer.c_user_id, ")
    '            builder.Append("              max(Officer.d_from) as d_from ")
    '            builder.Append("              from full_time_union_officer Officer,full_time_salary_personal sala_P ")
    '            builder.Append(" " & "	" & "  WHERE ")
    '            builder.Append(" " & "		" & " sala_P.c_user_id = Officer.c_user_id ")
    '            builder.Append(" " & "		" & "and  Officer.c_user_id  = :c_user_id")
    '            builder.Append(" " & "		" & "and substring(sala_P.d_salary_pay,1,6) = substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "and substring(Officer.d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append("              group by Officer.c_user_id ")
    '            builder.Append("                     ) union_B ")
    '            builder.Append(" " & "	" & "where union_A.c_user_id = union_B.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A.c_ksh = :strKch  ")
    '            builder.Append(" " & "	" & "and union_A.k_del = '0' ")
    '            builder.Append(" " & "	" & "and union_A.d_from = union_B.d_from ) union_all, ")
    '            builder.Append(" " & "	" & "( ")
    '            builder.Append(" " & "		" & "select ")
    '            builder.Append(" " & "		" & "union_A_info.* ")
    '            builder.Append(" " & "		" & "from full_time_union_officer_info union_A_info, ( ")
    '            builder.Append(" " & "			" & "select ")
    '            builder.Append(" " & "			" & "c_user_id, max(d_from) as d_from ")
    '            builder.Append(" " & "			" & "from full_time_union_officer_info ")
    '            builder.Append(" " & "			" & "WHERE substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "group by c_user_id ) union_B_info ")
    '            builder.Append(" " & "	" & "where union_A_info.c_user_id = union_B_info.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A_info.d_from = union_B_info.d_from  ")
    '            builder.Append(" " & "	" & "and k_officer_salary is not null) union_all_info ")
    '            builder.Append(" where (union_all.d_retire is NULL ")
    '            builder.Append(" " & "	" & "or to_date(:strDate,'yyyyMMdd') <= d_retire ) ")
    '            builder.Append(" and union_all.c_user_id <> 'A000001' ")
    '            builder.Append(" and union_all_info.c_user_id =  union_all.c_user_id ")
    '            builder.Append(" order by union_all.c_user_id ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strKch", DbType.String))
    '            command.Parameters.Item("c_user_id").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strKch").Value = PublicCommand.GetKsh
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetRightList(ByVal strKsh As String, ByVal strDateFrom As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim cmdText As String = "select c_full_time_control_id, d_from, l_name from full_time_control where :d_from between d_from and d_to and c_full_time_control_id <> :control_id_system order by c_full_time_control_id "
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add("control_id_system", "001")
    '            command.Parameters.Add("c_ksh", strKsh)
    '            command.Parameters.Add("d_from", strDateFrom)
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_control", dReader))
    '            set2 = ds
    '        Catch exception As BaseUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0002", New String(0 - 1) {})
    '        Catch exception3 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetSalayDataForUpdate(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim cmdText As String = "select d_up,s_up,k_salary_details from full_time_salary where d_salary_pay = :strDate  for update"
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Return Nothing
    '            End If
    '            Dim table As DataTable = MyBase.CreateSomeDataSet("full_time_salary", dReader)
    '            Dim ds As New DataSet
    '            ds.Tables.Add(table)
    '            set2 = ds
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetSalayDataUpdateCnt(ByVal strUserId As String, ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim cmdText As String = "select s_up from full_time_salary_personal where d_salary_pay = :strDate  and c_user_id  = :strUserId "
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"êÍè]êEàıããó^å¬êlä«óùÉeÅ[ÉuÉãÇÃÉfÅ[É^"})
    '            End If
    '            Dim table As DataTable = MyBase.CreateSomeDataSet("full_time_salary_personal", dReader)
    '            Dim ds As New DataSet
    '            ds.Tables.Add(table)
    '            set2 = ds
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionBfKoujoData(ByVal strDate As String, ByVal strbeforeDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select c_salary_item, ")
    '            builder.Append("        " & "â»ñ⁄" & ", ")
    '            builder.Append("        " & "çTèúï\èoóÕãÊï™" & ", ")
    '            builder.Append("        " & "äz" & "  ")
    '            builder.Append(" " & "	" & "from( ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & "salary_item_details.l_item_name as c_salary_item ")
    '            builder.Append(" " & "	" & ", full_time_salary_personal_dtl.c_salary_item as " & "â»ñ⁄" & " ")
    '            builder.Append(" " & "	" & ", salary_item_details.k_deduction_put as " & "çTèúï\èoóÕãÊï™" & " ")
    '            builder.Append(" " & "	" & ", full_time_salary_personal_dtl.s_person as " & "äz" & " ")
    '            builder.Append(" " & "	" & ", salary_item_details.s_order ")
    '            builder.Append(" " & "	" & ", salary_item_details.s_deduction_order ")
    '            builder.Append(" " & "		" & "from full_time_salary_personal_dtl ")
    '            builder.Append(" " & "		" & "LEFT OUTER JOIN salary_item_details ")
    '            builder.Append(" " & "		" & "on (full_time_salary_personal_dtl.c_salary_item = salary_item_details.c_salary_item) ")
    '            builder.Append(" " & "		" & "where salary_item_details.k_salary_put = '1' ")
    '            builder.Append(" " & "		" & "and full_time_salary_personal_dtl.d_salary_pay = :strDate ")
    '            builder.Append(" " & "		" & "and salary_item_details.d_from <= :strDate ")
    '            builder.Append(" " & "		" & "and salary_item_details.d_To >= :strDate ")
    '            builder.Append(" " & "		" & "and full_time_salary_personal_dtl.c_user_id = :strUserId ")
    '            builder.Append(" " & "		" & "and salary_item_details.k_salary_item_classify = '02' ")
    '            builder.Append("         and full_time_salary_personal_dtl.s_person != 0 ")
    '            builder.Append(" " & "		" & "and salary_item_details.k_deduction_put = '1' ")
    '            builder.Append(" " & "						" & "union all ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & "salary_item_details.l_item_name as c_salary_item ")
    '            builder.Append(" " & "	" & ", full_time_salary_personal_dtl.c_salary_item as " & "â»ñ⁄" & " ")
    '            builder.Append(" " & "	" & ", salary_item_details.k_deduction_put as " & "çTèúï\èoóÕãÊï™" & " ")
    '            builder.Append(" " & "	" & ", full_time_salary_personal_dtl.s_person as " & "äz" & " ")
    '            builder.Append(" " & "	" & ", salary_item_details.s_order ")
    '            builder.Append(" " & "	" & ", salary_item_details.s_deduction_order ")
    '            builder.Append(" " & "		" & "from full_time_salary_personal_dtl ")
    '            builder.Append(" " & "		" & "LEFT OUTER JOIN salary_item_details ")
    '            builder.Append(" " & "		" & "on (full_time_salary_personal_dtl.c_salary_item = salary_item_details.c_salary_item) ")
    '            builder.Append(" " & "		" & "where salary_item_details.k_salary_put = '1' ")
    '            builder.Append(" " & "		" & "and full_time_salary_personal_dtl.d_salary_pay = :strbeforeDate ")
    '            builder.Append(" " & "		" & "and salary_item_details.d_from <= :strDate ")
    '            builder.Append(" " & "		" & "and salary_item_details.d_To >= :strDate ")
    '            builder.Append(" " & "		" & "and full_time_salary_personal_dtl.c_user_id = :strUserId ")
    '            builder.Append(" " & "		" & "and salary_item_details.k_salary_item_classify = '02' ")
    '            builder.Append("         and full_time_salary_personal_dtl.s_person != 0 ")
    '            builder.Append(" " & "		" & "and salary_item_details.k_deduction_put = '0' ")
    '            builder.Append(" " & "		" & ") union_all ")
    '            builder.Append(" order by " & "çTèúï\èoóÕãÊï™" & " desc,s_order ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strbeforeDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strbeforeDate").Value = strbeforeDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("UnionKoujoData", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionBillData(ByVal strDate As String, ByVal strbeforeDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            builder.Append(" full_time_salary_personal_dtl.c_salary_item as " & "â»ñ⁄" & ", ")
    '            builder.Append(" salary_item_details.k_deduction_put as " & "çTèúï\èoóÕãÊï™" & ",")
    '            If (strUserId = "") Then
    '                builder.Append(" full_time_salary_personal_dtl.c_user_id, ")
    '            End If
    '            builder.Append(" salary_item_details.l_item_name as " & "ñºëO" & ",")
    '            builder.Append(" full_time_salary_personal_dtl.s_person as " & "äz")
    '            builder.Append(" from full_time_salary_personal_dtl ")
    '            builder.Append(" LEFT OUTER JOIN salary_item_details  ")
    '            builder.Append(" " & "	" & "on (full_time_salary_personal_dtl.c_salary_item =  salary_item_details.c_salary_item) ")
    '            builder.Append(" where salary_item_details.k_salary_put = '1'")
    '            If (strbeforeDate = "") Then
    '                builder.Append(" and full_time_salary_personal_dtl.d_salary_pay = :strDate  ")
    '            Else
    '                builder.Append(" and full_time_salary_personal_dtl.d_salary_pay = :strbeforeDate  ")
    '            End If
    '            builder.Append(" and salary_item_details.d_from <= :strDate  ")
    '            builder.Append(" and salary_item_details.d_To >= :strDate  ")
    '            If (strUserId <> "") Then
    '                builder.Append(" and full_time_salary_personal_dtl.c_user_id = :strUserId   ")
    '            End If
    '            builder.Append(" and salary_item_details.k_salary_item_classify = '01' ")
    '            builder.Append(" order by salary_item_details.s_order  ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strbeforeDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strbeforeDate").Value = strbeforeDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("UnionBillData", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionBillMasterData(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select l_item_name as " & "ñºëO" & ",c_salary_item  from salary_item_details ")
    '            builder.Append(" where d_from <= :strDate  ")
    '            builder.Append(" and d_To >= :strDate  ")
    '            builder.Append(" and k_salary_put = '1'  ")
    '            builder.Append(" and k_salary_item_classify = '01' ")
    '            builder.Append(" order by s_order  ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"êÍè]êEàıããó^â»ñ⁄ç◊ñ⁄É}ÉXÉ^ÇÃÉfÅ[É^"})
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("UnionBillMasterData", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionBonusOfficerNameAndDtl(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select ")
    '            builder.Append(" " & "	" & "union_all.c_user_id as " & "é–àıî‘çÜ" & " , ")
    '            builder.Append(" " & "	" & "union_all.l_name as " & "ñºëO" & ", ")
    '            builder.Append(" " & "	" & "union_all.d_from as " & "ìKâûì˙" & " from ( ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & " union_A.* ")
    '            builder.Append(" " & "	" & "from full_time_union_officer union_A,")
    '            builder.Append("				" & "(select ")
    '            builder.Append("              Officer.c_user_id, ")
    '            builder.Append("              max(Officer.d_from) as d_from ")
    '            builder.Append("              from full_time_union_officer Officer,full_time_bonus_personal sala_P ")
    '            builder.Append(" " & "	" & "  WHERE ")
    '            builder.Append(" " & "		" & "sala_P.c_user_id = Officer.c_user_id ")
    '            builder.Append(" " & "		" & "and substring(sala_P.d_salary_pay,1,6) = substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "and substring(Officer.d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append("              group by Officer.c_user_id ")
    '            builder.Append("                     ) union_B ")
    '            builder.Append(" " & "	" & "where union_A.c_user_id = union_B.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A.c_ksh = :strKch   ")
    '            builder.Append(" " & "	" & "and union_A.k_del = '0'   ")
    '            builder.Append(" " & "	" & "and union_A.d_from = union_B.d_from ) union_all, ")
    '            builder.Append(" " & "	" & "( ")
    '            builder.Append(" " & "		" & "select ")
    '            builder.Append(" " & "		" & "union_A_info.* ")
    '            builder.Append(" " & "		" & "from full_time_union_officer_info union_A_info, ( ")
    '            builder.Append(" " & "			" & "select ")
    '            builder.Append(" " & "			" & "c_user_id, max(d_from) as d_from ")
    '            builder.Append(" " & "			" & "from full_time_union_officer_info ")
    '            builder.Append(" " & "			" & "WHERE substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "group by c_user_id ) union_B_info ")
    '            builder.Append(" " & "	" & "where union_A_info.c_user_id = union_B_info.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A_info.d_from = union_B_info.d_from  ")
    '            builder.Append(" " & "	" & "and k_officer_salary is not null) union_all_info ")
    '            builder.Append(" where (union_all.d_retire is NULL ")
    '            builder.Append(" " & "	" & "or to_date(:strDate,'yyyyMMdd') <= d_retire ) ")
    '            builder.Append(" and union_all.c_user_id <> 'A000001' ")
    '            builder.Append(" and union_all_info.c_user_id =  union_all.c_user_id ")
    '            builder.Append(" order by union_all.c_user_id ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strKch", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strKch").Value = PublicCommand.GetKsh
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionKoujoData(ByVal strDate As String, ByVal strbeforeDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            builder.Append(" salary_item_details.l_item_name as c_salary_item,")
    '            builder.Append(" full_time_salary_personal_dtl.c_salary_item as " & "â»ñ⁄" & ", ")
    '            builder.Append(" salary_item_details.k_deduction_put as " & "çTèúï\èoóÕãÊï™" & ",")
    '            If (strUserId = "") Then
    '                builder.Append(" full_time_salary_personal_dtl.c_user_id, ")
    '            End If
    '            builder.Append(" full_time_salary_personal_dtl.s_person as " & "äz")
    '            builder.Append(" from full_time_salary_personal_dtl ")
    '            builder.Append(" LEFT OUTER JOIN salary_item_details  ")
    '            builder.Append(" " & "	" & "on (full_time_salary_personal_dtl.c_salary_item =  salary_item_details.c_salary_item) ")
    '            builder.Append(" where salary_item_details.k_salary_put = '1' ")
    '            If (strbeforeDate = "") Then
    '                builder.Append(" and full_time_salary_personal_dtl.d_salary_pay = :strDate  ")
    '            Else
    '                builder.Append(" and full_time_salary_personal_dtl.d_salary_pay = :strbeforeDate  ")
    '            End If
    '            builder.Append(" and salary_item_details.d_from <= :strDate  ")
    '            builder.Append(" and salary_item_details.d_To >= :strDate  ")
    '            If (strUserId <> "") Then
    '                builder.Append(" and full_time_salary_personal_dtl.c_user_id = :strUserId ")
    '            End If
    '            builder.Append(" and salary_item_details.k_salary_item_classify = '02' ")
    '            builder.Append(" and full_time_salary_personal_dtl.s_person != 0 ")
    '            builder.Append(" order by salary_item_details.k_deduction_put desc,salary_item_details.s_order  ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strbeforeDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strbeforeDate").Value = strbeforeDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("UnionKoujoData", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionKoujoMasterData(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select l_item_name as " & "â»ñ⁄" & ",")
    '            builder.Append(" c_salary_item from salary_item_details ")
    '            builder.Append(" salary_item_details ")
    '            builder.Append(" where d_from <= :strDate  ")
    '            builder.Append(" and d_To >= :strDate  ")
    '            builder.Append(" and k_salary_put = '1'  ")
    '            builder.Append(" and k_salary_item_classify = '02' ")
    '            builder.Append(" and k_deduction_put != '1' ")
    '            builder.Append(" and c_salary_item != '018' ")
    '            builder.Append(" order by s_order  ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"êÍè]êEàıããó^â»ñ⁄ç◊ñ⁄É}ÉXÉ^ÇÃÉfÅ[É^"})
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("UnionKoujoMasterData", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOfficerBillPayData(ByVal strDate As String, ByVal strPay As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            builder.Append(" s_salary_a, ")
    '            builder.Append(" c_user_id ")
    '            builder.Append(" from  ")
    '            If (strPay = "0") Then
    '                builder.Append(" full_time_salary_personal ")
    '            Else
    '                builder.Append(" full_time_bonus_personal ")
    '            End If
    '            builder.Append(" where  ")
    '            builder.Append(" d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"êÍè]êEàıããó^å¬êlä«óùñæç◊ÉeÅ[ÉuÉãÇÃÉfÅ[É^"})
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOfficerData(ByVal strDate As String, ByVal strUserId As String, ByVal strKch As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" SELECT ")
    '            builder.Append(" union_all.k_officer_salary ")
    '            builder.Append(" , full_time_union_officer.d_birth ")
    '            builder.Append(" FROM full_time_salary ")
    '            builder.Append(" , full_time_salary_personal ")
    '            builder.Append(" LEFT OUTER JOIN ( ")
    '            builder.Append(" " & "	" & "SELECT ")
    '            builder.Append(" " & "	" & " full_time_union_officer_info_A.* ")
    '            builder.Append(" " & "	" & "FROM full_time_union_officer_info full_time_union_officer_info_A ")
    '            builder.Append(" " & "	" & ", ( ")
    '            builder.Append(" " & "		" & "SELECT ")
    '            builder.Append(" " & "		" & " c_user_id ")
    '            builder.Append(" " & "		" & ", max(d_from) as d_from ")
    '            builder.Append(" " & "		" & "FROM full_time_union_officer_info ")
    '            builder.Append(" " & "		" & "WHERE substring(d_from ")
    '            builder.Append(" " & "			" & ",1 ")
    '            builder.Append(" " & "			" & ",6) <= substring(:strDate ")
    '            builder.Append(" " & "			" & ",1 ")
    '            builder.Append(" " & "			" & ",6) ")
    '            builder.Append(" " & "		" & "GROUP BY c_user_id ) full_time_union_officer_info_B ")
    '            builder.Append(" " & "	" & "WHERE full_time_union_officer_info_A.c_user_id = full_time_union_officer_info_B.c_user_id ")
    '            builder.Append(" " & "	" & "AND full_time_union_officer_info_A.d_from = full_time_union_officer_info_B.d_from ")
    '            builder.Append(" " & "	" & "AND full_time_union_officer_info_A.c_user_id = :strUserId ) union_all ")
    '            builder.Append(" ON (full_time_salary_personal.c_user_id = union_all.c_user_id) ")
    '            builder.Append(" , ( ")
    '            builder.Append(" " & "	" & "SELECT ")
    '            builder.Append(" " & "	" & " full_time_union_officer_A.* ")
    '            builder.Append(" " & "	" & "FROM full_time_union_officer full_time_union_officer_A ")
    '            builder.Append(" " & "	" & ", ( ")
    '            builder.Append(" " & "		" & "SELECT ")
    '            builder.Append(" " & "		" & " c_user_id ")
    '            builder.Append(" " & "		" & ", max(d_from) as d_from ")
    '            builder.Append(" " & "		" & "FROM full_time_union_officer ")
    '            builder.Append(" " & "		" & "WHERE ")
    '            builder.Append(" " & "		" & "substring(d_from ")
    '            builder.Append(" " & "			" & ", 1 ")
    '            builder.Append(" " & "			" & ", 6) <= substring(:strDate ")
    '            builder.Append(" " & "			" & ", 1 ")
    '            builder.Append(" " & "			" & ", 6) ")
    '            builder.Append(" " & "		" & "GROUP BY c_user_id ) full_time_union_officer_B ")
    '            builder.Append(" " & "	" & "WHERE full_time_union_officer_A.c_user_id = full_time_union_officer_B.c_user_id ")
    '            builder.Append(" " & "	" & "AND full_time_union_officer_A.d_from = full_time_union_officer_B.d_from ")
    '            builder.Append(" " & "	" & "AND full_time_union_officer_A.c_ksh = :strKch  ")
    '            builder.Append(" " & "	" & "AND full_time_union_officer_A.k_del = '0' ")
    '            builder.Append(" " & "	" & "AND full_time_union_officer_A.c_user_id = :strUserId ) full_time_union_officer ")
    '            builder.Append(" WHERE full_time_salary.d_salary_pay = full_time_salary_personal.d_salary_pay ")
    '            builder.Append(" AND full_time_salary_personal.c_user_id = :strUserId ")
    '            builder.Append(" AND full_time_salary_personal.d_salary_pay = :strDate ")
    '            builder.Append(" AND full_time_salary_personal.c_user_id = full_time_union_officer.c_user_id ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strKch", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strKch").Value = PublicCommand.GetKsh
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"êÍè]êEàıããó^å¬êlä«óùÉeÅ[ÉuÉãÇ∆êÍè]êEàıããó^å¬êlä«óùñæç◊ÉeÅ[ÉuÉãÇÃÉfÅ[É^"})
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOfficerEndYearData(ByVal strDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            builder.Append(" s_tax_year_total, ")
    '            If (strUserId = "") Then
    '                builder.Append(" c_user_id, ")
    '            End If
    '            builder.Append(" s_calculate_tax, ")
    '            builder.Append(" s_year_adjust ")
    '            builder.Append(" from  ")
    '            builder.Append(" full_time_salary_personal ")
    '            builder.Append(" where  ")
    '            If (strUserId <> "") Then
    '                builder.Append(" c_user_id = :strUserId and ")
    '            End If
    '            builder.Append(" d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("EndYear", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOfficerName(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select ")
    '            builder.Append(" " & "	" & "union_all.c_user_id as " & "é–àıî‘çÜ" & " , ")
    '            builder.Append(" " & "	" & "union_all.l_name as " & "ñºëO" & ", ")
    '            builder.Append(" " & "	" & "union_all.d_from as " & "ìKâûì˙" & " from ( ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & " union_A.* ")
    '            builder.Append(" " & "	" & "from full_time_union_officer union_A, ( ")
    '            builder.Append(" " & "		" & "select ")
    '            builder.Append(" " & "		" & " c_user_id, ")
    '            builder.Append(" " & "		" & "max(d_from) as d_from ")
    '            builder.Append(" " & "		" & "from full_time_union_officer ")
    '            builder.Append(" " & "		" & "where substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "group by c_user_id ) union_B ")
    '            builder.Append(" " & "	" & "where union_A.c_user_id = union_B.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A.c_ksh = :strKch  ")
    '            builder.Append(" " & "	" & "and union_A.k_del = '0'  ")
    '            builder.Append(" " & "	" & "and union_A.d_from = union_B.d_from ) union_all, ")
    '            builder.Append(" " & "	" & "( ")
    '            builder.Append(" " & "		" & "select ")
    '            builder.Append(" " & "		" & "union_A_info.* ")
    '            builder.Append(" " & "		" & "from full_time_union_officer_info union_A_info, ( ")
    '            builder.Append(" " & "			" & "select ")
    '            builder.Append(" " & "			" & "c_user_id, max(d_from) as d_from ")
    '            builder.Append(" " & "			" & "from full_time_union_officer_info ")
    '            builder.Append(" " & "			" & "WHERE substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "group by c_user_id ) union_B_info ")
    '            builder.Append(" " & "	" & "where union_A_info.c_user_id = union_B_info.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A_info.d_from = union_B_info.d_from  ")
    '            builder.Append(" " & "	" & "and k_officer_salary is not null) union_all_info ")
    '            builder.Append(" where (union_all.d_retire is NULL ")
    '            builder.Append(" " & "	" & "or to_date(:strDate,'yyyyMMdd') <= d_retire ) ")
    '            builder.Append(" and union_all.c_user_id <> 'A000001' ")
    '            builder.Append(" and union_all_info.c_user_id =  union_all.c_user_id ")
    '            builder.Append(" order by union_all.c_user_id ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strKch", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strKch").Value = PublicCommand.GetKsh
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOfficerNameAndDtl(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select ")
    '            builder.Append(" " & "	" & "union_all.c_user_id as " & "é–àıî‘çÜ" & " , ")
    '            builder.Append(" " & "	" & "union_all.l_name as " & "ñºëO" & ", ")
    '            builder.Append(" " & "	" & "union_all.d_from as " & "ìKâûì˙" & " from ( ")
    '            builder.Append(" " & "	" & "select ")
    '            builder.Append(" " & "	" & " union_A.* ")
    '            builder.Append(" " & "	" & "from full_time_union_officer union_A,")
    '            builder.Append("				" & "(select ")
    '            builder.Append("              Officer.c_user_id, ")
    '            builder.Append("              max(Officer.d_from) as d_from ")
    '            builder.Append("              from full_time_union_officer Officer,full_time_salary_personal sala_P ")
    '            builder.Append(" " & "	" & "  WHERE ")
    '            builder.Append(" " & "		" & "sala_P.c_user_id = Officer.c_user_id ")
    '            builder.Append(" " & "		" & "and substring(sala_P.d_salary_pay,1,6) = substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "and substring(Officer.d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append("              group by Officer.c_user_id ")
    '            builder.Append("                     ) union_B ")
    '            builder.Append(" " & "	" & "where union_A.c_user_id = union_B.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A.c_ksh = :strKch   ")
    '            builder.Append(" " & "	" & "and union_A.k_del = '0'  ")
    '            builder.Append(" " & "	" & "and union_A.d_from = union_B.d_from ) union_all, ")
    '            builder.Append(" " & "	" & "( ")
    '            builder.Append(" " & "		" & "select ")
    '            builder.Append(" " & "		" & "union_A_info.* ")
    '            builder.Append(" " & "		" & "from full_time_union_officer_info union_A_info, ( ")
    '            builder.Append(" " & "			" & "select ")
    '            builder.Append(" " & "			" & "c_user_id, max(d_from) as d_from ")
    '            builder.Append(" " & "			" & "from full_time_union_officer_info ")
    '            builder.Append(" " & "			" & "WHERE substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append(" " & "		" & "group by c_user_id ) union_B_info ")
    '            builder.Append(" " & "	" & "where union_A_info.c_user_id = union_B_info.c_user_id ")
    '            builder.Append(" " & "	" & "and union_A_info.d_from = union_B_info.d_from ")
    '            builder.Append(" " & "	" & "and k_officer_salary is not null) union_all_info ")
    '            builder.Append(" where (union_all.d_retire is NULL ")
    '            builder.Append(" " & "	" & "or to_date(:strDate,'yyyyMMdd') <= d_retire ) ")
    '            builder.Append(" and union_all.c_user_id <> 'A000001' ")
    '            builder.Append(" and union_all_info.c_user_id =  union_all.c_user_id ")
    '            builder.Append(" order by union_all.c_user_id ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strKch", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strKch").Value = PublicCommand.GetKsh
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_union_officer", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOfficerOverWorkData(ByVal strDate As String, ByVal strUserId As String, ByVal strSalaKbn As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            If (strSalaKbn = "01") Then
    '                builder.Append(" d_overtime_work_time, ")
    '            Else
    '                builder.Append(" d_old_overtime_work_time, ")
    '            End If
    '            builder.Append(" d_holiday_work_time, ")
    '            builder.Append(" d_midnight_work_time, ")
    '            If (strUserId = "") Then
    '                builder.Append(" c_user_id, ")
    '            End If
    '            builder.Append(" s_overtime_work_time ")
    '            builder.Append(" from  ")
    '            builder.Append(" full_time_salary_personal ")
    '            builder.Append(" where  ")
    '            If (strUserId <> "") Then
    '                builder.Append(" c_user_id = :strUserId and ")
    '            End If
    '            builder.Append(" d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("OverWork", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOfficerSalaryData(ByVal strDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            builder.Append(" full_time_salary.d_up as d_up, ")
    '            builder.Append(" full_time_salary.k_deposit as k_deposit, ")
    '            builder.Append(" full_time_salary_personal.s_salary_a as SalaryA, ")
    '            builder.Append(" full_time_salary_personal.s_salary_b as SalaryB, ")
    '            If (strUserId = "") Then
    '                builder.Append(" full_time_salary_personal.c_user_id, ")
    '            End If
    '            builder.Append(" full_time_salary_personal.s_salary as toukyu, ")
    '            builder.Append(" full_time_salary_personal.s_rank as s_rank, ")
    '            builder.Append(" union_all.k_officer_salary as s_salary ")
    '            builder.Append(" from  ")
    '            builder.Append(" full_time_salary_personal ")
    '            builder.Append(" left outer join  ")
    '            builder.Append("   (select ")
    '            builder.Append("        full_time_union_officer_info_A.* ")
    '            builder.Append("        from full_time_union_officer_info full_time_union_officer_info_A, ")
    '            builder.Append("             (select ")
    '            builder.Append("              c_user_id, ")
    '            builder.Append("              max(d_from) as d_from ")
    '            builder.Append("              from full_time_union_officer_info ")
    '            builder.Append(" " & "			" & "WHERE substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append("              group by c_user_id ")
    '            builder.Append("                     ) full_time_union_officer_info_B ")
    '            builder.Append("             where   ")
    '            builder.Append("             full_time_union_officer_info_A.c_user_id = full_time_union_officer_info_B.c_user_id ")
    '            builder.Append(" " & "	" & "    and" & "	" & "full_time_union_officer_info_A.d_from = full_time_union_officer_info_B.d_from ")
    '            builder.Append("         ) union_all on  ")
    '            builder.Append(" (full_time_salary_personal.c_user_id = union_all.c_user_id) ")
    '            builder.Append(" left outer join ")
    '            builder.Append("  full_time_salary on (full_time_salary_personal.d_salary_pay = full_time_salary.d_salary_pay)  ")
    '            builder.Append(" where  ")
    '            If (strUserId <> "") Then
    '                builder.Append(" full_time_salary_personal.c_user_id = :strUserId and ")
    '            End If
    '            builder.Append(" full_time_salary_personal.d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"êÍè]êEàıããó^å¬êlä«óùÉeÅ[ÉuÉãÇ∆êÍè]êEàıããó^å¬êlä«óùñæç◊ÉeÅ[ÉuÉãÇÃÉfÅ[É^"})
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("Salary", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOfficerUpDateTimeData(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            builder.Append(" d_up,k_deposit ")
    '            builder.Append(" from full_time_salary ")
    '            builder.Append(" where d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_salary", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOtherData(ByVal strDate As String, ByVal strbeforeDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            builder.Append(" full_time_salary_personal_dtl.c_salary_item as " & "â»ñ⁄" & ", ")
    '            builder.Append(" salary_item_details.k_deduction_put as " & "çTèúï\èoóÕãÊï™" & ",")
    '            If (strUserId = "") Then
    '                builder.Append(" full_time_salary_personal_dtl.c_user_id, ")
    '            End If
    '            builder.Append(" salary_item_details.l_item_name as " & "ñºëO" & ",")
    '            builder.Append(" full_time_salary_personal_dtl.s_person as " & "äz")
    '            builder.Append(" from full_time_salary_personal_dtl ")
    '            builder.Append(" LEFT OUTER JOIN salary_item_details  ")
    '            builder.Append(" " & "	" & "on (full_time_salary_personal_dtl.c_salary_item =  salary_item_details.c_salary_item) ")
    '            builder.Append(" where salary_item_details.k_salary_put = '1' ")
    '            If (strbeforeDate = "") Then
    '                builder.Append(" and full_time_salary_personal_dtl.d_salary_pay = :strDate  ")
    '            Else
    '                builder.Append(" and full_time_salary_personal_dtl.d_salary_pay = :strbeforeDate  ")
    '            End If
    '            builder.Append(" and salary_item_details.d_from <= :strDate  ")
    '            builder.Append(" and salary_item_details.d_To >= :strDate  ")
    '            If (strUserId <> "") Then
    '                builder.Append(" and full_time_salary_personal_dtl.c_user_id = :strUserId")
    '            End If
    '            builder.Append(" and salary_item_details.k_salary_item_classify = '03' ")
    '            builder.Append(" order by salary_item_details.s_order  ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strbeforeDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strbeforeDate").Value = strbeforeDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("UnionOtherData", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionOtherMasterData(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select l_item_name as " & "ñºëO" & ",c_salary_item from salary_item_details ")
    '            builder.Append(" where d_from <= :strDate  ")
    '            builder.Append(" and d_To >= :strDate  ")
    '            builder.Append(" and k_salary_put = '1'  ")
    '            builder.Append(" and k_salary_item_classify = '03' ")
    '            builder.Append(" order by s_order  ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"êÍè]êEàıããó^â»ñ⁄ç◊ñ⁄É}ÉXÉ^ÇÃÉfÅ[É^"})
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("UnionOtherMasterData", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionPerSonalDataSum(ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" select  ")
    '            builder.Append(" full_time_salary_personal.s_salary_a, ")
    '            builder.Append(" full_time_salary_personal.s_salary_b, ")
    '            builder.Append(" full_time_salary_personal.d_overtime_work_time, ")
    '            builder.Append(" full_time_salary_personal.d_holiday_work_time, ")
    '            builder.Append(" full_time_salary_personal.d_midnight_work_time, ")
    '            builder.Append(" full_time_salary_personal.d_old_overtime_work_time, ")
    '            builder.Append(" full_time_salary_personal.c_user_id_up, ")
    '            builder.Append(" full_time_salary_personal.s_overtime_work_time, ")
    '            builder.Append(" union_all.k_officer_salary as s_salary ")
    '            builder.Append(" from  ")
    '            builder.Append(" full_time_salary_personal ")
    '            builder.Append(" left outer join  ")
    '            builder.Append("   (select ")
    '            builder.Append("        full_time_union_officer_info_A.* ")
    '            builder.Append("        from full_time_union_officer_info full_time_union_officer_info_A, ")
    '            builder.Append("             (select ")
    '            builder.Append("              c_user_id, ")
    '            builder.Append("              max(d_from) as d_from ")
    '            builder.Append("              from full_time_union_officer_info ")
    '            builder.Append(" " & "			" & "WHERE substring(d_from, 1, 6) <= substring(:strDate, 1, 6) ")
    '            builder.Append("              group by c_user_id ")
    '            builder.Append("                     ) full_time_union_officer_info_B ")
    '            builder.Append("             where   ")
    '            builder.Append("             full_time_union_officer_info_A.c_user_id = full_time_union_officer_info_B.c_user_id ")
    '            builder.Append(" " & "	" & "    and" & "	" & "full_time_union_officer_info_A.d_from = full_time_union_officer_info_B.d_from ")
    '            builder.Append("         ) union_all on  ")
    '            builder.Append(" (full_time_salary_personal.c_user_id = union_all.c_user_id) ")
    '            builder.Append(" left outer join ")
    '            builder.Append("  full_time_salary on (full_time_salary_personal.d_salary_pay = full_time_salary.d_salary_pay)  ")
    '            builder.Append(" where  ")
    '            builder.Append(" full_time_salary_personal.d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_salary_personal", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Function GetUnionRankData(ByVal strDate As String, ByVal strUserId As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim builder As New StringBuilder
    '            builder.Append(" SELECT ")
    '            builder.Append("  full_time_salary.d_up as d_up ")
    '            builder.Append(" , full_time_salary_personal.s_salary ")
    '            builder.Append(" , full_time_salary_personal.s_rank ")
    '            builder.Append(" FROM full_time_salary ")
    '            builder.Append(" , full_time_salary_personal ")
    '            builder.Append(" WHERE full_time_salary.d_salary_pay = full_time_salary_personal.d_salary_pay ")
    '            builder.Append(" AND full_time_salary_personal.c_user_id = :strUserId ")
    '            builder.Append(" AND full_time_salary_personal.d_salary_pay = :strDate ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            command.Parameters.Item("strDate").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("full_time_salary_personal", dReader))
    '            set2 = ds
    '        Catch exception As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
    '        Catch exception2 As AppUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As SysUnionException
    '            exception3.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception3
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    '    Public Sub InsertFullTimeSalaryPersonalDtlData(ByVal dRowNewdata As DataRow)
    '        Try
    '            Dim map As New FullTimeSalaryPersonalDtlBillMap
    '            Dim str2 As String = map.ToPhysicalString("")
    '            Dim str3 As String = map.ToPhysicalString(":")
    '            Dim command As New NpgsqlCommand(String.Concat(New String() {"insert into full_time_salary_personal_dtl ( ", str2, " ) values( ", str3, " ) "}), MyBase.GetNpgsqlConnection, MyBase.GetNpgsqlTran)
    '            Dim i As Integer
    '            For i = 0 To map.ColumnCount - 1
    '                command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(i), map.GetDbDataType(i)))
    '                command.Parameters.Item(map.GetPhysicalName(i)).Value = dRowNewdata.Item(map.GetPhysicalName(i))
    '            Next i
    '            If (command.ExecuteNonQuery <> 1) Then
    '                Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0001", New String(0 - 1) {})
    '            End If
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '    End Sub

    '    Public Sub InsertFullTimeSalaryPersonalDtlTempData(ByVal dRowNewdata As DataRow)
    '        Try
    '            Dim map As New FullTimeTemporarySalaryDtlMap
    '            Dim str2 As String = map.ToPhysicalString("")
    '            Dim str3 As String = map.ToPhysicalString(":")
    '            Dim command As New NpgsqlCommand(String.Concat(New String() {"insert into full_time_temporary_salary_dtl ( ", str2, " ) values( ", str3, " ) "}), MyBase.GetNpgsqlConnection, MyBase.GetNpgsqlTran)
    '            Dim i As Integer
    '            For i = 0 To map.ColumnCount - 1
    '                command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(i), map.GetDbDataType(i)))
    '                command.Parameters.Item(map.GetPhysicalName(i)).Value = dRowNewdata.Item(map.GetPhysicalName(i))
    '            Next i
    '            If (command.ExecuteNonQuery <> 1) Then
    '                Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0001", New String(0 - 1) {})
    '            End If
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '    End Sub

    '    Public Sub InsertFullTimeUnionOfficer(ByVal dSetNew As DataSet)
    '        Try
    '            MyBase.InsertData(New NpgsqlCommand("", CommonDaoClass.connNpgsql), New FullTimeUnionOfficerMap, dSetNew, "full_time_union_officer")
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '    End Sub

    '    Public Sub UpdateFullTimeSalaryData(ByVal dSetNewData As DataSet, ByVal strDate As String)
    '        Try
    '            Dim builder As New StringBuilder
    '            Dim map As New FullTimeSalalyExBillMap
    '            Dim i As Integer
    '            For i = 0 To map.ColumnCount - 1
    '                builder.Append((map.GetPhysicalName(i) & " = :" & map.GetPhysicalName(i) & ", "))
    '            Next i
    '            builder.Remove((builder.Length - 2), 2)
    '            Dim command As New NpgsqlCommand(("update full_time_salary set " & builder.ToString & " where d_salary_pay     = :strDate "), MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            Dim j As Integer
    '            For j = 0 To map.ColumnCount - 1
    '                command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(j), map.GetDbDataType(j)))
    '                command.Parameters.Item(map.GetPhysicalName(j)).Value = dSetNewData.Tables.Item("full_time_salary").Rows.Item(0).Item(map.GetPhysicalName(j))
    '            Next j
    '            command.Parameters.Item("strDate").Value = strDate
    '            If (command.ExecuteNonQuery < 1) Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0005", New String(0 - 1) {})
    '            End If
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '    End Sub

    '    Public Sub UpdateFullTimeSalaryPersonalData(ByVal dSetNewData As DataSet, ByVal strUserId As String, ByVal strDate As String)
    '        Try
    '            Dim builder As New StringBuilder
    '            Dim map As New FullTimeSalaryPersonalBillMap
    '            Dim i As Integer
    '            For i = 0 To map.ColumnCount - 1
    '                builder.Append((map.GetPhysicalName(i) & " = :" & map.GetPhysicalName(i) & ", "))
    '            Next i
    '            builder.Remove((builder.Length - 2), 2)
    '            Dim command As New NpgsqlCommand(("update full_time_salary_personal set " & builder.ToString & " where c_user_id  = :strUserId   and d_salary_pay     = :strDate "), MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("strUserId", DbType.String))
    '            Dim j As Integer
    '            For j = 0 To map.ColumnCount - 1
    '                command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(j), map.GetDbDataType(j)))
    '                command.Parameters.Item(map.GetPhysicalName(j)).Value = dSetNewData.Tables.Item("full_time_salary_personal").Rows.Item(0).Item(map.GetPhysicalName(j))
    '            Next j
    '            command.Parameters.Item("strDate").Value = strDate
    '            command.Parameters.Item("strUserId").Value = strUserId
    '            If (command.ExecuteNonQuery < 1) Then
    '                Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0005", New String(0 - 1) {})
    '            End If
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '    End Sub

    '    Public Sub UpdateFullTimeUnionOfficer(ByVal dSetNew As DataSet)
    '        Try
    '            Dim map As New FullTimeUnionOfficerMap
    '            Dim builder As New StringBuilder("update full_time_union_officer set ")
    '            Dim i As Integer
    '            For i = 0 To map.ColumnCount - 1
    '                builder.Append((map.GetPhysicalName(i) & " = :" & map.GetPhysicalName(i) & ","))
    '            Next i
    '            builder.Remove(builder.ToString.LastIndexOf(","), 1)
    '            builder.Append(" where c_user_id = :c_user_id and d_from = :d_from ")
    '            Dim command As New NpgsqlCommand(builder.ToString, MyBase.GetNpgsqlConnection)
    '            Dim j As Integer
    '            For j = 0 To map.ColumnCount - 1
    '                command.Parameters.Add(map.GetPhysicalName(j), dSetNew.Tables.Item("full_time_union_officer").Rows.Item(0).Item(map.GetPhysicalName(j)))
    '            Next j
    '            If (command.ExecuteNonQuery <> 1) Then
    '                Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0002", New String(0 - 1) {})
    '            End If
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '    End Sub

    '    'TODO DAO.MemberÇà⁄êA---------------------------------------------------------------------
    '    Public Function GetStafName(ByVal strUserId As String, ByVal strDate As String) As String
    '        Dim str2 As String
    '        Try
    '            Dim cmdText As String = "SELECT " & "	" & "l_name FROM " & "	" & "full_time_union_officer WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND d_from <= :d_date "
    '            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
    '            command.Parameters.Item("c_user_id").Value = strUserId
    '            command.Parameters.Item("d_date").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Return ""
    '            End If
    '            str2 = MyBase.CreateSomeDataSet("full_time_union_officer", dReader).Rows.Item(0).Item("l_name").ToString
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return str2
    '    End Function

    '    Public Function GetStafPersonalData(ByVal strUserId As String, ByVal strDate As String) As DataSet
    '        Dim set2 As DataSet
    '        Try
    '            Dim map As New FullTimeUnionOfficerMap
    '            Dim command As New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM full_time_union_officer WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND d_from <= :d_date "), MyBase.GetNpgsqlConnection)
    '            command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
    '            command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
    '            command.Parameters.Item("c_user_id").Value = strUserId
    '            command.Parameters.Item("d_date").Value = strDate
    '            Dim dReader As NpgsqlDataReader = command.ExecuteReader
    '            If Not dReader.HasRows Then
    '                Return Nothing
    '            End If
    '            Dim ds As New DataSet
    '            ds.Tables.Add(MyBase.CreateSomeDataSet("staf_attribute", dReader))
    '            set2 = ds
    '        Catch exception As AppUnionException
    '            exception.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception
    '        Catch exception2 As SysUnionException
    '            exception2.AddMethodName(MethodBase.GetCurrentMethod)
    '            Throw exception2
    '        Catch exception3 As NpgsqlException
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
    '        Catch exception4 As Exception
    '            Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
    '        End Try
    '        Return set2
    '    End Function

    'End Class
End Namespace
