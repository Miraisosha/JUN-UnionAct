#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Command
Imports UnionAct.NSMDInfo
Imports UnionAct.Framework.Mapping

Namespace DAO.Activity
    Public Class CommitteeListDao
        Inherits AbstractDao
        'Implements ICommitteeListDao
        ' Methods
        Public Function ChkCommitteeList(ByVal strKsh As String, ByVal strPeriodId As String, ByVal strCommitteeId As String, ByVal strDfrom As String, ByVal strUserId As String) As Integer
            Dim num As Integer
            Try
                Dim cmdText As String = "select count(d.c_committee_list) from  (select a.c_committee_list, a.d_from from committee_list a,    ( select c_committee_list, max(d_from) as d_from        from committee_list       where             c_ksh          = :c_ksh         and c_period_id    = :c_period_id         and c_committee_id = :c_committee_id         and d_from        <= :d_from        group by c_committee_list     ) b   where a.c_committee_list = b.c_committee_list     and a.d_from = b.d_from   ) c, committee_list_dtl d  where c.c_committee_list = d.c_committee_list    and c.d_from           = d.d_from    and d.c_user_id        = :c_user_id "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_from").Value = strDfrom
                command.Parameters.Item("c_user_id").Value = strUserId
                num = Convert.ToInt32(command.ExecuteScalar.ToString)
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

        Public Sub DeleteData(ByVal strCommitteeList As String, ByVal strDFrom As String)
            Try
                Dim cmdText As String = "delete from committee_list where c_committee_list = :c_committee_list   and d_from           = :d_from "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_list", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("c_committee_list").Value = strCommitteeList
                command.Parameters.Item("d_from").Value = strDFrom
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

        Public Function GetBackGroundData(ByVal strKsh As String, ByVal strPeriodId As String, ByVal strCommitteeId As String, ByVal strMasterDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "select substring(a.d_from, 1, 4) AS " & "対象年" & ", substring(a.d_from, 5, 2) AS " & "月" & ", b.l_name AS " & "作成者" & ",   a.d_up   AS " & "作成日" & "    from committee_list a LEFT OUTER JOIN ( select h.l_name, h.c_user_id from staf_attribute_full_time_view h,   ( select c_user_id, max(d_from) as d_from from staf_attribute_full_time_view      where d_from <= :d_from      group by c_user_id ) i where h.d_from    = i.d_from   and h.c_user_id = i.c_user_id ) b  ON (a.c_user_id_up    = b.c_user_id ) where a.c_ksh          = :c_ksh          and   a.c_period_id    = :c_period_id    and   a.c_committee_id = :c_committee_id order by a.d_from DESC "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Item("d_from").Value = strMasterDate
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0002", New String(0 - 1) {})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list", dReader)
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetBelongCommittee(ByVal strCommitteeList As String, ByVal strUserId As String, ByVal strDFrom As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim cmdText As String = " select    PER.l_omission_name   AS period_name,     COM.l_name AS committee_name,     COMDTL.l_name AS seq_name,     COMLIST.c_period_id,     COM.c_committee_id,    COMDTL.s_committee_seq,    PER.d_from,    PER.d_to,    COMDTL.s_from_diff,    COMDTL.s_to_diff,    COMLSTDTL.c_committee_list,    '0' AS department_flg   from   committee COM,   committee_dtl COMDTL,   committee_list_dtl COMLSTDTL,   committee_list COMLIST   LEFT OUTER JOIN period PER ON   (    PER.c_ksh       = :c_ksh    and PER.c_period_id = COMLIST.c_period_id )   where        COM.c_committee_id = COMLIST.c_committee_id    AND COM.d_from <= :d_date    AND COM.d_to   >= :d_date    AND COM.c_ksh   = :c_ksh    AND COMDTL.c_committee_id  = COM.c_committee_id     AND COMDTL.s_committee_seq = COMLSTDTL.s_committee_seq    AND COMDTL.d_from <= :d_date    AND COMDTL.d_to   >= :d_date    AND COMLIST.c_committee_list   = COMLSTDTL.c_committee_list    AND COMLIST.c_ksh              = :c_ksh    AND COMLIST.d_from             = COMLSTDTL.d_from    AND COMLSTDTL.c_committee_list = :c_committee_list    AND COMLSTDTL.c_user_id        = :c_user_id    AND COMLSTDTL.d_from           = :d_from  ORDER BY COMLIST.c_period_id, COM.c_committee_id "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_list", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("d_date").Value = PublicCommand.GetSystemDate
                command.Parameters.Item("c_committee_list").Value = strCommitteeList
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("d_from").Value = strDFrom
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                table2 = MyBase.CreateSomeDataSet("committee_list", dReader)
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
            Return table2
        End Function

        Public Function GetCommitteeListData(ByVal strKsh As String, ByVal strPeriodId As String, ByVal strCommitteeId As String, ByVal strDate As String, ByVal strMasterDate As String, ByVal Key As Integer) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = _
					 "SELECT " & _
					 "     COM_D.l_name           AS " & "役職" & "              --  1" & "委員会組織明細" & _
						 ",STAF.l_name            AS " & "名前" & "              --  2" & "組織構成員" & _
						 ",STAF.c_staf_id         AS " & "社員番号" & "          --  3" & "組織構成員" & _
						 ",MODEL.l_name           AS " & "機種" & "              --  4" & "定数マスタ（機種）" & _
						 ",QUA.l_omission_name    AS " & "資格" & "              --  5" & "定数マスタ（資格）" & _
						 ",MODEL.l_omission_name  AS " & "機種（略名）" & "      --  6" & "定数マスタ（機種）" & _
						 ",COM_V.c_ksh            AS " & "会社コード" & "        --  7" & "委員会名簿" & _
						 ",COM_V.c_period_id      AS " & "期ＩＤ" & "            --  8" & "委員会名簿" & _
						 ",COM_V.c_committee_list AS " & "委員会名簿ＩＤ" & "    --  9" & "委員会名簿明細" & _
						 ",COM_V.c_user_id        AS " & "個人認証ＩＤ" & "      --  0" & "委員会名簿明細" & _
						 ",COM_V.d_from           AS " & "適用開始年月日" & "    --  1" & "委員会名簿明細" & _
						 ",COM_V.c_committee_id   AS " & "委員会ＩＤ" & "        --  2" & "委員会名簿明細" & _
						 ",COM_V.s_committee_seq  AS " & "委員会ＩＤ枝番" & "    --  3" & "委員会名簿明細" & _
						 ",STAF.k_model           AS " & "機種区分" & "          --  4" & "組織構成員" & _
						 ",STAF.k_qualification   AS " & "資格区分" & "          --  5" & "組織構成員" & _
						 ",STAF.k_belonging       AS " & "所属支部区分" & "      --  6" & "組織構成員" & _
						 ",BELONG.l_name          AS " & "組合支部" & "          --  7" & "定数マスタ（所属支部区分）" & _
						 ",STAF.k_staf_kind       AS " & "組合員種別コード" & "  --  8" & "組織構成員" & _
						 ",STAF_KIND.l_name       AS " & "組合員種別" & " " & _
						 ",STAF.k_user_status     AS " & "組合員ステータス区分" & "  --  0" & "組織構成員" & _
						 ",USER_STATUS.l_name     AS " & "ステータス" & _
						 ",STAF.l_name_kna        AS " & "名前カナ" & "          --  2" & "組織構成員" & _
						 ",COM_V.com_lst_dtl_d_up AS " & "名簿明細更新日" & "    --  3" & "委員会名簿明細" & _
						 ",COM_V.com_lst_d_up     AS " & "名簿更新日" & "        --  3" & "委員会名簿" & _
                         ",ADDRESS.l_tell_1       AS " & "電話番号" & "          --  " & "組織構成員住所" & "  " & "" & _
                         ",NULL       AS " & "電話番号" & "          --  " & "組織構成員住所" & "  " & "" & _
						 ",QUA.l_name             AS " & "資格（和名）" & "      --  6" & "定数マスタ（資格）" & _
						 ",TO_NUMBER(STAF.c_staf_id, '9999999999') AS " & "社員番号（" & "int" & "）" & "  --7" & "" & _
					 "FROM " & "" & _
						 "committee_list_dtl_view COM_V  -- " & "委員会名簿明細" & "VIEW" & "" & _
						 "--  (" & "委員会組織明細マスタ" & ")" & "" & _
						 "LEFT OUTER JOIN committee_dtl COM_D ON " & "" & _
						 "(   COM_V.c_committee_id  = COM_D.c_committee_id  " & "" & _
						 "AND COM_V.s_committee_seq = COM_D.s_committee_seq " & "" & _
						 "AND :d_Mdate BETWEEN COM_D.d_from AND COM_D.d_to" & "" & _
						 ")" & "" & _
						 ",staf_attribute_view  STAF  -- " & "組織構成員" & "VIEW" & "" & _
						 "--  (" & "定数マスタ" & "(" & "機種" & "))" & "" & _
						 "LEFT OUTER JOIN model_view MODEL ON  " & "" & _
						 "(   :d_Mdate BETWEEN MODEL.d_from AND MODEL.d_to" & "" & _
						 "AND MODEL.c_constant_seq = STAF.k_model )  " & "" & _
						 "--  (" & "定数マスタ" & "(" & "資格" & "))" & "" & _
						 "LEFT OUTER JOIN qualification_view QUA ON  " & "" & _
						 "(   :d_Mdate BETWEEN QUA.d_from AND QUA.d_to" & "" & _
						 "AND QUA.c_constant_seq = STAF.k_qualification )  " & "" & _
						 "--  (" & "定数マスタ" & "(" & "所属支部区分" & "))" & "" & _
						 "LEFT OUTER JOIN belonging_view BELONG ON  " & "" & _
						 "(   :d_Mdate BETWEEN BELONG.d_from AND BELONG.d_to" & "" & _
						 "AND BELONG.c_constant_seq = STAF.k_belonging )  " & "" & _
                         "--  (" & "組織構成員住所" & ")" & _
                         "--LEFT OUTER JOIN staf_address_view ADDRESS ON  " & _
                         "--(   :d_Mdate BETWEEN ADDRESS.d_from AND ADDRESS.d_to" & _
                         "--AND ADDRESS.c_user_id = STAF.c_user_id )  " & _
						 "-- (" & "定数マスタ" & ")" & _
						 "LEFT OUTER JOIN " & _
						 "(SELECT c_constant_seq, l_name FROM constant_dtl" & _
					     "  WHERE c_constant = 'STAF_KIND'" & _
							"AND :d_Mdate BETWEEN d_from AND d_to" & _
						") STAF_KIND ON  " & _
						 "( STAF_KIND.c_constant_seq = STAF.k_staf_kind )" & _
						 "LEFT OUTER JOIN " & _
						 "(SELECT c_constant_seq, l_name FROM constant_dtl" & _
						 "  WHERE c_constant = 'USER_STATUS'" & _
						 "    AND :d_Mdate BETWEEN d_from AND d_to" & _
						 ") USER_STATUS ON  " & _
						 "( USER_STATUS.c_constant_seq = STAF.k_user_status )" & _
						 "WHERE" & _
							 "COM_V.c_ksh          = :c_ksh" & _
						 "AND COM_V.c_committee_id = :c_committee_id" & _
						 "AND COM_V.c_period_id    = :c_period_id" & _
						 "AND :d_date  BETWEEN COM_V.d_from        AND COM_V.d_to" & _
						 "AND :d_Mdate BETWEEN COM_V.master_d_from AND COM_V.master_d_to" & _
						 "AND COM_V.c_user_id      = STAF.c_user_id" & _
						 "AND :d_Mdate BETWEEN STAF.d_from AND STAF.d_to" & _
                         " ORDER BY COM_V.s_committee_seq, to_number(STAF.c_staf_id, 9999999999)"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_Mdate", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_main_add", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strDate
                Dim year As Integer = Integer.Parse(strMasterDate.Substring(0, 4))
                Dim month As Integer = Integer.Parse(strMasterDate.Substring(4, 2))
                command.Parameters.Item("d_Mdate").Value = New DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("yyyyMMdd")
                command.Parameters.Item("k_main_add").Value = "1"
                command.Parameters.Item("k_del").Value = "0"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list_need", dReader)
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCommitteeListMemberByIdAndBelong(ByVal strCommitteeId As String, ByVal strBelonging As String, ByVal strPeriodId As String, ByVal strKeyDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim command As New NpgsqlCommand(("select " & "	" & "ALL_INFO.l_name as " & "名前" & ", " & "	" & "ALL_INFO.c_staf_id as " & "社員番号" & ", " & "	" & "MODEL.l_name as " & "機種" & ", " & "	" & "QUALIFICATION.l_omission_name as " & "資格" & ", " & "	" & "BELONGING.l_name as " & "組合支部" & ", " & "	" & "AREA_LOCAL.l_name as " & "会社所属" & ", " & "	" & "ALL_INFO.c_ksh as " & "会社コード" & ", " & "	" & "ALL_INFO.c_user_id as " & "個人認証ＩＤ" & ", " & "	" & "MODEL.l_omission_name as " & "機種（略名）" & ", " & "	" & "ALL_INFO.k_model as " & "機種区分" & ", " & "	" & "ALL_INFO.k_qualification as " & "資格区分" & ", " & "	" & "ALL_INFO.k_belonging as " & "所属支部区分" & ", " & "	" & "ALL_INFO.k_local as " & "会社支部区分" & ", " & "	" & "ALL_INFO.l_name_kna as " & "名前カナ" & ", " & "	" & "QUALIFICATION.l_name as " & "資格（和名）" & ", " & "	" & "AREA_LOCAL.l_omission_name as " & "所属" & ", " & "	" & "BELONGING.l_omission_name as " & "支部" & ", " & "	" & "to_number(ALL_INFO.c_staf_id, '9999999999') as " & "社員番号（" & "int" & "）" & ", " & "	" & "ALL_INFO.s_committee_seq as " & "委員会ＩＤ枝番" & " from " & "	" & "(select " & "		" & "STAFF_ALL.*, " & "		" & "LISTMEMBER.s_committee_seq " & "	" & "from " & "		" & "(select " & "			" & "S_ABASE.* " & "		" & "from " & "			" & "staf_attribute S_ABASE, " & "			" & "(select " & "				" & "c_user_id, " & "				" & "max(d_from) as d_from " & "			" & "from " & "				" & "staf_attribute " & "			" & "where " & "				" & "c_ksh = :c_ksh " & "			" & "and" & "	" & "k_del = :k_del " & "			" & "and" & "	" & "d_from <= :d_from " & "			" & "group by " & "				" & "c_user_id " & "			" & ") S_AMAX " & "		" & "where " & "			" & "S_ABASE.c_user_id = S_AMAX.c_user_id " & "		" & "and" & "	" & "S_ABASE.d_from = S_AMAX.d_from " & "		" & "and" & "	" & "S_ABASE.c_ksh = :c_ksh " & "		" & "and" & "	" & "S_ABASE.k_del = :k_del      and S_ABASE.k_staf_kind in (:official, :semi, :senior)      and S_ABASE.k_user_status = :k_user_status " & If(String.IsNullOrEmpty(strBelonging), "", (" and S_ABASE.k_belonging = '" & strBelonging & "' ")) & "		" & ") STAFF_ALL, " & "		" & "(select " & "			" & "COM_DTL.c_committee_list, " & "			" & "COM_DTL.c_user_id, " & "			" & "COM_DTL.d_from, " & "			" & "COM_DTL.s_committee_seq " & "		" & "from " & "			" & "committee_list_dtl COM_DTL, " & "			" & "(select " & "				" & "c_committee_list, " & "				" & "max(d_from) as d_from " & "			" & "from " & "				" & "committee_list " & "			" & "where " & "				" & "c_ksh = :c_ksh " & "			" & "and" & "	" & "c_period_id = :c_period_id " & "			" & "and" & "	" & "d_from <= :d_from " & "			" & "and" & "	" & "c_committee_id = :c_committee_id " & "			" & "group by " & "				" & "c_committee_list " & "			" & ") BASE " & "		" & "where " & "			" & "COM_DTL.c_committee_list = BASE.c_committee_list " & "		" & "and" & "	" & "COM_DTL.d_from = BASE.d_from " & "		" & ") LISTMEMBER " & "	" & "where " & "		" & "STAFF_ALL.c_user_id = LISTMEMBER.c_user_id " & "	" & ") ALL_INFO left outer join " & "	" & "(select " & "		" & "c_constant_seq, " & "		" & "l_name, " & "		" & "l_omission_name " & "	" & "from " & "		" & "constant_dtl " & "	" & "where " & "		" & "d_from <= :d_from " & "	" & "and" & "	" & ":d_from <= d_to " & "	" & "and" & "	" & "c_constant = 'MODEL' " & "	" & ") MODEL on " & "	" & "MODEL.c_constant_seq = ALL_INFO.k_model left outer join " & "	" & "(select " & "		" & "c_constant_seq, " & "		" & "l_name, " & "		" & "l_omission_name " & "	" & "from " & "		" & "constant_dtl " & "	" & "where " & "		" & "d_from <= :d_from " & "	" & "and" & "	" & ":d_from <= d_to " & "	" & "and" & "	" & "c_constant = 'QUALIFICATION' " & "	" & ") QUALIFICATION on " & "	" & "QUALIFICATION.c_constant_seq = ALL_INFO.k_qualification left outer join " & "	" & "(select " & "		" & "c_constant_seq, " & "		" & "l_name, " & "		" & "l_omission_name " & "	" & "from " & "		" & "constant_dtl " & "	" & "where " & "		" & "d_from <= :d_from " & "	" & "and" & "	" & ":d_from <= d_to " & "	" & "and" & "	" & "c_constant = 'AREA_LOCAL' " & "	" & ") AREA_LOCAL on " & "	" & "AREA_LOCAL.c_constant_seq = ALL_INFO.k_local left outer join " & "	" & "(select " & "		" & "c_constant_seq, " & "		" & "l_name, " & "		" & "l_omission_name " & "	" & "from " & "		" & "constant_dtl " & "	" & "where " & "		" & "d_from <= :d_from " & "	" & "and" & "	" & ":d_from <= d_to " & "	" & "and" & "	" & "c_constant = 'BELONGING' " & "	" & ") BELONGING on " & "	" & "BELONGING.c_constant_seq = ALL_INFO.k_belonging" & "	" & " "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("official", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("semi", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("senior", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_user_status", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("d_from").Value = strKeyDate
                command.Parameters.Item("official").Value = "01"
                command.Parameters.Item("semi").Value = "04"
                command.Parameters.Item("senior").Value = "02"
                command.Parameters.Item("k_user_status").Value = "01"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list", dReader)
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetCommitteeMemberList(ByVal strCommitteeId As String, ByVal strDate As String) As DataSet
        'Public Function GetCommitteeMemberList(ByVal objLoginSession As LoginSession, ByVal strCommitteeId As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "SELECT " & "	" & "staf_attribute_AAA.* FROM " & "	" & "(SELECT " & "		" & "staf_attribute_AA.c_user_id, " & "		" & "staf_attribute_AA.c_staf_id, " & "		" & "staf_attribute_AA.l_name, " & "		" & "staf_attribute_AA.k_model, " & "		" & "staf_attribute_AA.k_belonging, " & "		" & "staf_attribute_AA.k_qualification, " & "		" & "staf_attribute_AA.d_from " & "	" & "FROM " & "		" & "(SELECT" & "	" & "staf_attribute.* " & "		" & "FROM" & "	" & "staf_attribute, " & "			" & "(SELECT " & "				" & "c_user_id, " & "				" & "MAX(d_from) AS d_from " & "			" & "FROM " & "				" & "staf_attribute " & "			" & "WHERE " & "				" & "c_ksh = :c_ksh " & "				" & "AND d_from <= :d_date " & "				" & "AND k_del = :k_del " & "			" & "GROUP BY c_user_id " & "			" & ") staf_attribute_A " & "		" & "WHERE " & "			" & "staf_attribute.c_user_id = staf_attribute_A.c_user_id " & "			" & "AND staf_attribute.d_from = staf_attribute_A.d_from " & "			" & "AND staf_attribute.k_del = :k_del " & "		" & ") staf_attribute_AA " & "	" & ") staf_attribute_AAA, " & "	" & "(SELECT  " & "		" & "committee_list_dtl.c_user_id, " & "		" & "committee_list_dtl.c_committee_id " & "	" & "FROM " & "		" & "(SELECT  " & "			" & "c_committee_list,Max(d_from) AS d_from " & "		" & "FROM  " & "			" & "committee_list  " & "		" & "WHERE  " & "			" & "c_ksh = :c_ksh " & "			" & "AND c_committee_id = :c_committee_id " & "			" & "AND d_from <= :d_date " & "			" & "AND c_period_id = :c_period_id " & "		" & "GROUP BY " & "			" & "c_committee_list " & "		" & ") committee_list_A, " & "		" & "committee_list_dtl " & "	" & "WHERE " & "		" & "committee_list_dtl.c_committee_list = committee_list_A.c_committee_list " & "		" & "AND committee_list_dtl.d_from = committee_list_A.d_from " & "	" & ") committee_list_dtl_A WHERE " & "	" & "staf_attribute_AAA.c_user_id = committee_list_dtl_A.c_user_id ORDER BY " & "	" & "c_user_id "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("c_period_id").Value = Integer.Parse(MDLoginInfo.PeriodName) 'TODO
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("committee_list", dReader))
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

        Public Function GetData(ByVal strKsh As String, ByVal strPeriodId As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim str2 As String = New CommitteeListMap().ToPhysicalString(" a.")
                Dim command As New NpgsqlCommand(("select " & str2 & " from committee_list a,  ( select max(c_period_id) as c_period_id,           max(d_to) as d_to    from period    where c_ksh = :c_ksh      and c_period_id = :c_period_id      and k_period_kind = :k_period_kind ) b where a.c_ksh = :c_ksh    and a.c_period_id  = b.c_period_id    and a.d_from      <= b.d_to "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_period_kind", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("k_period_kind").Value = "01"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"委員会名簿の情報"})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list", dReader)
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Overloads Sub InsertData(ByVal dTable As DataTable)
            Try
                Dim map As New CommitteeListMap
                Dim str2 As String = map.ToPhysicalString("")
                Dim str3 As String = map.ToPhysicalString(":")
                Dim command As New NpgsqlCommand(String.Concat(New String() {"insert into committee_list( ", str2, " ) values( ", str3, " ) "}), MyBase.GetNpgsqlConnection, MyBase.GetNpgsqlTran)
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

        Public Function SelectForSeq(ByVal strKsh As String, ByVal strPeriodId As String, ByVal strCommitteeId As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "select c_committee_list from committee_list where c_ksh          = :c_ksh          and   c_period_id    = :c_period_id    and   c_committee_id = :c_committee_id "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list", dReader)
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function SelectForUpData(ByVal strCommitteeList As String, ByVal strDFrom As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "select d_up from committee_list where c_committee_list = :c_committee_list   and d_from           = :d_from for update "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_list", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("c_committee_list").Value = strCommitteeList
                command.Parameters.Item("d_from").Value = strDFrom
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_list", dReader)
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Sub UpdateData(ByVal strUserIdUp As String, ByVal strCommitteeList As String, ByVal strDFrom As String)
            Try
                Dim cmdText As String = "update committee_list  set d_up         = :d_up ,      c_user_id_up = :c_user_id_up ,      s_up         = s_up + 1 where c_committee_list = :c_committee_list   and d_from           = :d_from "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_up", DbType.DateTime))
                command.Parameters.Add(New NpgsqlParameter("c_user_id_up", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_list", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("d_up").Value = PublicCommand.GetNow
                command.Parameters.Item("c_user_id_up").Value = strUserIdUp
                command.Parameters.Item("c_committee_list").Value = strCommitteeList
                command.Parameters.Item("d_from").Value = strDFrom
                If (command.ExecuteNonQuery < 1) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0005", New String(0 - 1) {})
                End If
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

    End Class
End Namespace
