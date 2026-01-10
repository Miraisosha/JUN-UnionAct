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
Imports UnionAct.Framework.Interface
Imports UnionAct.NSMDInfo
Imports UnionAct.DAO.FullTimeUnionOfficer

Namespace DAO.Member
    Public Class StafAttributeTblDao
        Inherits AbstractDao
        'Implements IStafAttributeTblDao
        ' Methods
        Public Sub DeleteExistData(ByVal strUserId As String, ByVal strDate As String)
            Try
                Dim command As New NpgsqlCommand(("delete from staf_attribute " & Me.GetWherePhraseForPrevData(strUserId, strDate)), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_del").Value = "0"
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

        Public Function GetAllDataForStafAttribute(ByVal strUserId As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim map As New Staf_AttributeMap
                Dim command As New NpgsqlCommand(("select " & map.ToPhysicalString("") & " from staf_attribute  where c_user_id = :c_user_id    and d_from    = :d_from    and k_del     = :k_del    and c_ksh     = :c_ksh "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("d_from").Value = strDate
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"組合員情報"})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
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

        Public Function GetAllDataOfDfrom(ByVal strStaf_id As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "SELECT  ST_INFO.l_name AS " & "名前" & ",  ST_KIND.l_name AS " & "種別" & ",  UR_STATUS.l_name AS " & "ステータス" & ",  ST_INFO.* FROM   staf_attribute_current_view ST_NOW,   staf_attribute ST_INFO   LEFT OUTER JOIN staf_kind_view ST_KIND ON   (     ST_KIND.d_from <= :d_date     AND ST_KIND.d_to   >= :d_date     AND ST_KIND.c_constant_seq = ST_INFO.k_staf_kind )   LEFT OUTER JOIN user_status_view UR_STATUS ON   (     UR_STATUS.d_from <= :d_date     AND UR_STATUS.d_to   >= :d_date     AND UR_STATUS.c_constant_seq = ST_INFO.k_user_status ) WHERE     ST_INFO.k_del     = :k_del AND ST_INFO.c_ksh     = ST_NOW.c_ksh AND ST_INFO.c_user_id = ST_NOW.c_user_id AND ST_INFO.d_from    = ST_NOW.d_from AND ST_NOW.c_ksh      = :c_ksh AND ST_NOW.c_staf_id  = :c_staf_id AND ST_NOW.k_del      = :k_del ORDER BY d_join "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_staf_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("c_staf_id").Value = strStaf_id
                command.Parameters.Item("d_date").Value = PublicCommand.GetSystemDate
                command.Parameters.Item("k_del").Value = "0"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
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

        Public Function GetAttributeForBelonging(ByVal strBelonging As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim map As New StafAttributeAllowanceMap
                Dim command As New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM staf_attribute WHERE " & "	" & "k_belonging = :k_belonging " & "	" & "AND d_from <= :d_date "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("k_belonging").Value = strBelonging
                command.Parameters.Item("d_date").Value = PublicCommand.GetSystemDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("staf_attribute", dReader))
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

        Public Function GetDataForTabControl(ByVal strKsh As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "select c.l_name           AS " & "名前" & ",           c.c_staf_id        AS " & "社員番号" & ",       e.l_name           AS " & "機種" & ",           f.l_omission_name  AS " & "資格" & ",           h.l_name           AS " & "組合支部" & ",        c.k_staf_kind AS " & "組合員種別コード" & ", staf_kind.l_name AS " & "組合員種別" & ", c.k_user_status AS " & "組合員ステータス区分" & ", user_status.l_name AS " & "ステータス" & ", m.l_name           AS " & "会社所属" & ",       c.c_ksh            AS " & "会社コード" & ",     c.c_user_id        AS " & "個人認証ＩＤ" & ",   e.l_omission_name  AS " & "機種（略名）" & ",   c.k_model          AS " & "機種区分" & ",       c.k_qualification" & "	" & " AS " & "資格区分" & ",       c.k_belonging      AS " & "所属支部区分" & ",   c.k_local          AS " & "会社支部区分" & ",   c.l_name_kna       AS " & "名前カナ" & ",       f.l_name           AS " & "資格（和名）" & ",   m.l_omission_name  AS " & "所属" & ",           h.l_omission_name  AS " & "支部" & ",           TO_NUMBER(c.c_staf_id, '9999999999') AS " & "社員番号（" & "INT" & "）" & ", c.c_dezit AS " & "ディジット" & " from (select l.* from staf_attribute l,   (select c_user_id, max(d_from) as d_from      from staf_attribute           where d_from <= :d_date         and c_ksh   = :c_ksh          and k_del   = :k_del        group by c_user_id          ) j  where l.d_from    = j.d_from       and l.c_user_id = j.c_user_id ) c LEFT OUTER JOIN model_view e ON (    e.d_from    <= :d_date  and e.d_to      >= :d_date    and e.c_constant_seq = c.k_model ) LEFT OUTER JOIN qualification_view f ON (   f.d_from    <= :d_date and f.d_to      >= :d_date   and f.c_constant_seq = c.k_qualification ) LEFT OUTER JOIN belonging_view h ON (   h.d_from    <= :d_date and h.d_to      >= :d_date   and h.c_constant_seq = c.k_belonging ) LEFT OUTER JOIN area_local_view m ON (   m.d_from    <= :d_date and m.d_to      >= :d_date   and m.c_constant_seq = c.k_local )  LEFT OUTER JOIN " & "	" & "(SELECT " & "		" & "c_constant_seq, " & "		" & "l_name " & "	" & "FROM " & "		" & "constant_dtl " & "	" & "WHERE " & "		" & "c_constant = 'STAF_KIND' " & "		" & "AND d_from <= :d_date " & "		" & "AND d_to >= :d_date " & "	" & ") staf_kind ON " & "	" & "staf_kind.c_constant_seq = c.k_staf_kind LEFT OUTER JOIN " & "	" & "(SELECT " & "		" & "c_constant_seq, " & "		" & "l_name " & "	" & "FROM " & "		" & "constant_dtl " & "	" & "WHERE " & "		" & "c_constant = 'USER_STATUS' " & "		" & "AND d_from <= :d_date " & "		" & "AND d_to >= :d_date " & "	" & ") user_status ON " & "	" & "user_status.c_constant_seq = c.k_user_status where       c.k_staf_kind IN ('01', '04', '02') and   c.k_user_status = :k_user_status and   c.c_ksh         = :c_ksh  and   c.k_del         = :k_del  and   c.d_from       <= :d_date order by c.l_name_kna, TO_NUMBER(c.c_staf_id, '9999999999') "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_main_add", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_user_status", DbType.String))
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("k_main_add").Value = "1"
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("k_user_status").Value = "01"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"組合員"})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
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

        Public Function GetFixedShowData(ByVal strKsh As String, ByVal strKeyDate As String, ByVal strBelonging As String, ByVal strOrder As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim command As New NpgsqlCommand(("select A.c_user_id" & "				" & "AS " & "個人番号" & "," & "		" & "A.c_staf_id" & "				" & "AS " & "社員番号" & "," & "		" & "A.l_name" & "					" & "AS " & "名前" & "," & "			" & "A.l_name_kna" & "				" & "AS " & "フリガナ" & "," & "		" & "A.k_work_place" & "			" & "AS " & "職場区分" & "," & "		" & "E.l_tell_1" & "				" & "AS " & "電話番号" & "1," & "		" & "E.l_tell_2" & "				" & "AS " & "電話番号" & "2," & "		" & "E.l_cities" & "				" & "AS " & "市町村区" & "," & "		" & "E.l_add_ather" & "			" & "AS " & "番地等" & "," & "			" & "E.l_building" & "				" & "AS " & "建物名" & "," & "			" & "A.c_trans_ksh" & "			" & "AS " & "会社コード		" & "from (select l.c_user_id,l.c_staf_id,l.l_name,l.l_name_kna,l.k_work_place,l.c_trans_ksh,l.d_from,l.k_del,l.k_belonging from staf_attribute l, (select c_user_id,max(d_from) as d_from from staf_attribute where c_ksh = :c_ksh and d_from <= :d_from and k_del = :k_del GROUP BY c_user_id) B where l.k_staf_kind IN('01', '04', '02') and l.k_user_status = :k_user_status and l.c_user_id = B.c_user_id and l.d_from = B.d_from and l.k_del = :k_del " & If(String.IsNullOrEmpty(strBelonging), "", " and l.k_belonging = :k_belonging") & " )A LEFT OUTER JOIN (select h.l_tell_1,h.l_tell_2,h.l_cities,h.l_add_ather,h.l_building,h.c_user_id,h.d_from,h.k_main_add,h.k_del from staf_address h, (select c_user_id,max(d_from) as d_from from staf_address where d_from <= :d_from and k_main_add = :k_main_add and k_del = :k_del group by c_user_id) k where h.c_user_id = k.c_user_id and h.d_from = k.d_from and h.k_main_add = :k_main_add and h.k_del = :k_del ) E ON (A.c_user_id = E.c_user_id) " & If(String.Equals(strOrder, "btn50Printing"), " order by A.l_name_kna;", " order by to_number(A.c_staf_id,'9999999999')")), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_main_add", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_user_status", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("k_main_add").Value = "1"
                command.Parameters.Item("d_from").Value = strKeyDate
                command.Parameters.Item("k_user_status").Value = "01"
                command.Parameters.Item("k_belonging").Value = strBelonging
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("fixed_show_need", dReader)
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

        Public Function GetHasPrevData(ByVal strUserId As String, ByVal strDate As String) As Boolean
            Dim hasRows As Boolean
            Try
                Dim command As New NpgsqlCommand(("select c_user_id from staf_attribute " & Me.GetWherePhraseForPrevData(strUserId, strDate)), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_del").Value = "0"
                hasRows = command.ExecuteReader.HasRows
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
            Return hasRows
        End Function

        Public Function GetHistoryListData(ByVal strUserId As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet
                Dim cmdText As String = "select to_date(S_A.d_from, 'yyyyMMdd') as " & "適用日付" & ", " & "	" & "FULL_STAFF.l_name as " & "担当者" & " from " & "	" & "staf_attribute S_A left outer join (select FULL_STAF_INFO.c_user_id, FULL_STAF_INFO.d_from, FULL_STAF_INFO.c_ksh, FULL_STAF_INFO.c_staf_id, FULL_STAF_INFO.c_dezit, FULL_STAF_INFO.l_name, FULL_STAF_INFO.l_name_kna from (select c_user_id, max(d_from) as d_from from staf_attribute_full_time_view where c_ksh = :c_ksh group by c_user_id ) STAF_MAX, staf_attribute_full_time_view FULL_STAF_INFO where FULL_STAF_INFO.c_user_id = STAF_MAX.c_user_id and FULL_STAF_INFO.d_from = STAF_MAX.d_from ) FULL_STAFF on    " & "	" & "COALESCE(S_A.c_user_id_up, S_A.c_user_id_ins) = FULL_STAFF.c_user_id  where " & "	" & "S_A.c_user_id = :c_user_id and" & "	" & "S_A.c_ksh = :c_ksh and" & "	" & "S_A.k_del = :k_del order by " & "	" & "to_number(S_A.d_from, '99999999') desc "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
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

        Public Function GetLatestAttributeData(ByVal strUserId As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim map As New Staf_AttributeMap
                Dim command As New NpgsqlCommand(("select " & map.ToPhysicalString("baseS_A.") & " from " & "	" & "staf_attribute baseS_A, " & "	" & "(select " & "		" & "c_user_id, " & "		" & "max(d_from) as d_from " & "	" & "from " & "		" & "staf_attribute " & "	" & "where " & "		" & "c_user_id = :c_user_id " & "	" & "and" & "	" & "k_del = :k_del " & "	" & "and" & "	" & "c_ksh = :c_ksh " & "	" & "group by " & "		" & "c_user_id " & "	" & ") maxS_A where " & "	" & "baseS_A.c_user_id = maxS_A.c_user_id and" & "	" & "baseS_A.d_from = maxS_A.d_from and" & "	" & "k_del = :k_del and" & "	" & "c_ksh = :c_ksh "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() {"組合員情報"})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
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

        Public Function GetNumberOfStaf(ByVal strBasisDate As String, ByVal strLastYearDate As String, ByVal strBelonging As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = " select " & "	" & "A.s_captain_member, " & "	" & "B.s_copilot_member,  C.s_engineer_member,  D.s_else_member,  (A.s_captain_member - E.s_move_captain) as s_move_captain,  (B.s_copilot_member - F.s_move_copilot) as s_move_copilot,  (C.s_engineer_member - G.s_move_engineer) as s_move_engineer,  (D.s_else_member - H.s_move_else) as s_move_else  from " & "	" & "(select" & "	" & "count(AA.c_user_id) as s_captain_member " & "	" & "from staf_attribute AA, " & "		" & "(select" & "	" & "c_user_id, max(d_from) as d_from " & "		" & "from staf_attribute " & "		" & "where k_del = :k_del " & "		" & "and" & "	" & "c_ksh = :c_ksh " & "		" & "and" & "	" & "d_from <= :d_from " & "		" & "group by c_user_id " & "	" & ") BB " & "	" & "where " & "		" & "AA.k_belonging = :k_belonging " & "	" & "and" & "	" & "AA.c_user_id = BB.c_user_id " & "	" & "and" & "	" & "AA.d_from = BB.d_from  and AA.k_del = :k_del " & "	" & "and" & "	" & "AA.k_staf_kind in (:official, :semi, :senior)  and" & "	" & "AA.k_user_status = :user_status " & "	" & "and" & "	" & "AA.k_qualification = :captain ) A, " & "	" & "(select" & "	" & "count(AA.c_user_id) as s_copilot_member " & "	" & "from staf_attribute AA, " & "		" & "(select" & "	" & "c_user_id, max(d_from) as d_from " & "		" & "from staf_attribute " & "		" & "where k_del = :k_del " & "		" & "and" & "	" & "c_ksh = :c_ksh " & "		" & "and" & "	" & "d_from <= :d_from " & "		" & "group by c_user_id " & "	" & ") BB " & "	" & "where " & "		" & "AA.k_belonging = :k_belonging " & "	" & "and" & "	" & "AA.c_user_id = BB.c_user_id " & "	" & "and" & "	" & "AA.d_from = BB.d_from  and AA.k_del = :k_del " & "	" & "and" & "	" & "AA.k_staf_kind in (:official, :semi, :senior)  and" & "	" & "AA.k_user_status = :user_status " & "	" & "and" & "	" & "AA.k_qualification = :cop ) B, " & "	" & "(select" & "	" & "count(AA.c_user_id) as s_engineer_member " & "	" & "from staf_attribute AA, " & "		" & "(select" & "	" & "c_user_id, max(d_from) as d_from " & "		" & "from staf_attribute " & "		" & "where k_del = :k_del " & "		" & "and" & "	" & "c_ksh = :c_ksh " & "		" & "and" & "	" & "d_from <= :d_from " & "		" & "group by c_user_id " & "	" & ") BB " & "	" & "where " & "		" & "AA.k_belonging = :k_belonging " & "	" & "and" & "	" & "AA.c_user_id = BB.c_user_id " & "	" & "and" & "	" & "AA.d_from = BB.d_from  and AA.k_del = :k_del " & "	" & "and" & "	" & "AA.k_staf_kind in (:official, :semi, :senior)  and" & "	" & "AA.k_user_status = :user_status " & "	" & "and" & "	" & "AA.k_qualification = :fe ) C, " & "	" & "(select" & "	" & "count(AA.c_user_id) as s_else_member " & "	" & "from staf_attribute AA, " & "		" & "(select" & "	" & "c_user_id, max(d_from) as d_from " & "		" & "from staf_attribute " & "		" & "where k_del = :k_del " & "		" & "and" & "	" & "c_ksh = :c_ksh " & "		" & "and" & "	" & "d_from <= :d_from " & "		" & "group by c_user_id " & "	" & ") BB " & "	" & "where " & "		" & "AA.k_belonging = :k_belonging " & "	" & "and" & "	" & "AA.c_user_id = BB.c_user_id " & "	" & "and" & "	" & "AA.d_from = BB.d_from  and AA.k_del = :k_del " & "	" & "and" & "	" & "AA.k_staf_kind in (:official, :semi, :senior)  and" & "	" & "AA.k_user_status = :user_status " & "	" & "and" & "	" & "AA.k_qualification = :else ) D, " & "	" & "(select" & "	" & "count(AA.c_user_id) as s_move_captain " & "	" & "from staf_attribute AA, " & "		" & "(select" & "	" & "c_user_id, max(d_from) as d_from " & "		" & "from staf_attribute " & "		" & "where k_del = :k_del " & "		" & "and" & "	" & "c_ksh = :c_ksh " & "		" & "and" & "	" & "d_from <= :d_from_before " & "		" & "group by c_user_id " & "	" & ") BB " & "	" & "where " & "		" & "AA.k_belonging = :k_belonging " & "	" & "and" & "	" & "AA.c_user_id = BB.c_user_id " & "	" & "and" & "	" & "AA.d_from = BB.d_from  and AA.k_del = :k_del " & "	" & "and" & "	" & "AA.k_staf_kind in (:official, :semi, :senior)  and" & "	" & "AA.k_user_status = :user_status " & "	" & "and" & "	" & "AA.k_qualification = :captain ) E, " & "	" & "(select" & "	" & "count(AA.c_user_id) as s_move_copilot " & "	" & "from staf_attribute AA, " & "		" & "(select" & "	" & "c_user_id, max(d_from) as d_from " & "		" & "from staf_attribute " & "		" & "where k_del = :k_del " & "		" & "and" & "	" & "c_ksh = :c_ksh " & "		" & "and" & "	" & "d_from <= :d_from_before " & "		" & "group by c_user_id " & "	" & ") BB " & "	" & "where " & "		" & "AA.k_belonging = :k_belonging " & "	" & "and" & "	" & "AA.c_user_id = BB.c_user_id " & "	" & "and" & "	" & "AA.d_from = BB.d_from  and AA.k_del = :k_del " & "	" & "and" & "	" & "AA.k_staf_kind in (:official, :semi, :senior)  and" & "	" & "AA.k_user_status = :user_status " & "	" & "and" & "	" & "AA.k_qualification = :cop ) F, " & "	" & "(select" & "	" & "count(AA.c_user_id) as s_move_engineer " & "	" & "from staf_attribute AA, " & "		" & "(select" & "	" & "c_user_id, max(d_from) as d_from " & "		" & "from staf_attribute " & "		" & "where k_del = :k_del " & "		" & "and" & "	" & "c_ksh = :c_ksh " & "		" & "and" & "	" & "d_from <= :d_from_before " & "		" & "group by c_user_id " & "	" & ") BB " & "	" & "where " & "		" & "AA.k_belonging = :k_belonging " & "	" & "and" & "	" & "AA.c_user_id = BB.c_user_id " & "	" & "and" & "	" & "AA.d_from = BB.d_from  and AA.k_del = :k_del " & "	" & "and" & "	" & "AA.k_staf_kind in (:official, :semi, :senior)  and" & "	" & "AA.k_user_status = :user_status " & "	" & "and" & "	" & "AA.k_qualification = :fe ) G, " & "	" & "(select" & "	" & "count(AA.c_user_id) as s_move_else " & "	" & "from staf_attribute AA, " & "		" & "(select" & "	" & "c_user_id, max(d_from) as d_from " & "		" & "from staf_attribute " & "		" & "where k_del = :k_del " & "		" & "and" & "	" & "c_ksh = :c_ksh " & "		" & "and" & "	" & "d_from <= :d_from_before " & "		" & "group by c_user_id " & "	" & ") BB " & "	" & "where " & "		" & "AA.k_belonging = :k_belonging " & "	" & "and" & "	" & "AA.c_user_id = BB.c_user_id " & "	" & "and" & "	" & "AA.d_from = BB.d_from  and AA.k_del = :k_del " & "	" & "and" & "	" & "AA.k_staf_kind in (:official, :semi, :senior)  and" & "	" & "AA.k_user_status = :user_status " & "	" & "and" & "	" & "AA.k_qualification = :else ) H "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from_before", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_belonging", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("official", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("semi", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("senior", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("captain", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("cop", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("fe", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("else", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("user_status", DbType.String))
                command.Parameters.Item("d_from").Value = strBasisDate
                command.Parameters.Item("d_from_before").Value = strLastYearDate
                command.Parameters.Item("k_belonging").Value = strBelonging
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("official").Value = "01"
                command.Parameters.Item("semi").Value = "04"
                command.Parameters.Item("senior").Value = "02"
                command.Parameters.Item("captain").Value = "01"
                command.Parameters.Item("cop").Value = "02"
                command.Parameters.Item("fe").Value = "03"
                command.Parameters.Item("else").Value = "99"
                command.Parameters.Item("user_status").Value = "01"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
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

        Public Function GetQueryTabPageData(ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "SELECT " & "	" & "staf_attribute_AA.c_user_id AS " & "個人認証ＩＤ" & ", " & "	" & "staf_attribute_AA.c_staf_id AS " & "社員番号" & ", " & "	" & "TO_NUMBER(staf_attribute_AA.c_staf_id, '9999999999') AS " & "社員番号" & "LONG, " & "	" & "staf_attribute_AA.l_name AS " & "名前" & ", " & "	" & "staf_attribute_AA.l_name_kna AS " & "名前カナ" & ", " & "	" & "staf_attribute_AA.k_staf_kind AS " & "組合員種別コード" & ", " & "	" & "staf_attribute_AA.k_belonging AS " & "所属支部区分" & ", " & "	" & "belonging_view.l_name AS " & "支部" & " FROM " & "	" & "(SELECT " & "		" & "staf_attribute.* " & "	" & "FROM " & "		" & "staf_attribute, " & "		" & "(SELECT " & "			" & "c_user_id, " & "			" & "MAX(d_from) AS d_from " & "		" & "FROM " & "			" & "staf_attribute " & "		" & "WHERE " & "			" & "d_from <= :d_date " & "			" & "AND c_ksh = :c_ksh " & "			" & "AND k_del = :k_del " & "		" & "GROUP BY " & "			" & "c_user_id " & "		" & ") staf_attribute_A " & "	" & "WHERE " & "		" & "staf_attribute.d_from = staf_attribute_A.d_from " & "		" & "AND staf_attribute.c_user_id = staf_attribute_A.c_user_id " & "	" & ") staf_attribute_AA LEFT OUTER JOIN " & "	" & "belonging_view ON " & "	" & "belonging_view.d_from <= :d_date " & "	" & "AND belonging_view.d_to >= :d_date " & "	" & "AND belonging_view.c_constant_seq = staf_attribute_AA.k_belonging WHERE " & "	" & "staf_attribute_AA.k_user_status = :k_user_status " & "	" & "AND staf_attribute_AA.c_ksh = :c_ksh " & "	" & "AND staf_attribute_AA.k_del = :k_del " & "	" & "AND staf_attribute_AA.d_from <= :d_date ORDER BY " & "	" & "TO_NUMBER(staf_attribute_AA.c_staf_id, '9999999999'); "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_user_status", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("c_ksh").Value = CommonDataClass.Instance.strKsh
                command.Parameters.Item("k_user_status").Value = "01"
                command.Parameters.Item("k_del").Value = "0"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
                Dim ds As New DataSet
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0002", New String(0 - 1) {})
            Catch exception2 As AppUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetSearchListData(ByVal strPeriodId As String, ByVal strAttribute As String, ByVal strAddress As String, ByVal strCommittee As String, ByVal strSortOrder As String, ByVal strDate As String) As DataTable
            Dim table As DataTable
            Try
                'New SearchListAttributeMap
                'New SearchListAddressMap
                Dim str As String = " FALSE                           AS print_check,  staf_attribute_AA.c_user_id     AS c_user_id,  TO_NUMBER(staf_attribute_AA.c_staf_id, '9999999999') AS c_staf_id,   staf_attribute_AA.l_name        AS l_name,  staf_attribute_AA.l_name_kna    AS l_name_kna,  KINDVIEW.l_name                 AS staf_kind,  staf_attribute_AA.k_staf_kind   AS k_staf_kind,  USERVIEW.l_name                 AS k_user_status,  BELOVIEW.l_name                 AS k_belonging,  QUALIVIEW.l_name                AS qualification,  staf_attribute_AA.k_qualification AS k_qualification,  MODELVIEW.l_name                AS model,  staf_attribute_AA.k_model       AS k_model,  LOCALVIEW.l_name                AS k_local,  WORKVIEW.l_name                 AS k_work_place,  ksh.n_abbreviation              AS c_trans_ksh,  SUBSTRING(TO_CHAR(staf_attribute_AA.d_captain, 'yyyyMMdd'), 1, 4) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_captain, 'yyyyMMdd'), 5, 2) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_captain, 'yyyyMMdd'), 7, 2)                                  AS d_captain,  SUBSTRING(TO_CHAR(staf_attribute_AA.d_join, 'yyyyMMdd'), 1, 4) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_join, 'yyyyMMdd'), 5, 2) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_join, 'yyyyMMdd'), 7, 2)                                  AS d_join,   SUBSTRING(TO_CHAR(staf_attribute_AA.d_birth, 'yyyyMMdd'), 1, 4) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_birth, 'yyyyMMdd'), 5, 2) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_birth, 'yyyyMMdd'), 7, 2)                                  AS d_birth,  SUBSTRING(TO_CHAR(staf_attribute_AA.d_enter, 'yyyyMMdd'), 1, 4) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_enter, 'yyyyMMdd'), 5, 2) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_enter, 'yyyyMMdd'), 7, 2)                                  AS d_enter,  SUBSTRING(TO_CHAR(staf_attribute_AA.d_retire, 'yyyyMMdd'), 1, 4) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_retire, 'yyyyMMdd'), 5, 2) || '/' ||  SUBSTRING(TO_CHAR(staf_attribute_AA.d_retire, 'yyyyMMdd'), 7, 2)                                  AS d_retire,  area.n_ksh                      AS c_area,  STATEVIEW.l_name                AS k_work_state,  staf_attribute_AA.d_from "
                Dim str2 As String = " staf_address_AA.l_add_number     AS l_add_number,  CASE WHEN  staf_address_AA.k_foreign = :k_foreign THEN  TRIM(COALESCE( staf_address_AA.l_prefectures," & "	" & "'' ) || ' ' ||       COALESCE( staf_address_AA.l_cities," & "		" & "'' ) || ' ' ||       COALESCE( staf_address_AA.l_add_ather," & "		" & "'' ) || ' ' || " & "		" & " COALESCE( staf_address_AA.l_building," & "		" & "'' ))  ELSE  TRIM(COALESCE( staf_address_AA.l_foreign_adress_1, '' ) || ' ' ||       COALESCE( staf_address_AA.l_foreign_adress_2, '' ) || ' ' ||       COALESCE( staf_address_AA.l_foreign_adress_3, '' ) || ' ' ||       COALESCE( staf_address_AA.l_foreign_adress_4, '' ) || ' ' ||       COALESCE( staf_address_AA.l_foreign_adress_5, '' ))  END AS l_address,  staf_address_AA.l_tell_1        AS l_tell_1,  staf_address_AA.l_tell_2        AS l_tell_2,  staf_address_AA.l_prefectures      AS l_prefectures,  staf_address_AA.l_cities           AS l_cities,  staf_address_AA.l_add_ather        AS l_add_ather,  staf_address_AA.l_building         AS l_building,  staf_address_AA.k_foreign          AS k_foreign,  staf_address_AA.l_foreign_adress_1 AS l_foreign_adress_1,  staf_address_AA.l_foreign_adress_2 AS l_foreign_adress_2,  staf_address_AA.l_foreign_adress_3 AS l_foreign_adress_3,  staf_address_AA.l_foreign_adress_4 AS l_foreign_adress_4,  staf_address_AA.l_foreign_adress_5 AS l_foreign_adress_5  "
                Dim strArray As String() = New String() {" LEFT OUTER JOIN ", " , "}
                Dim strArray2 As String() = New String() {" ON ", " WHERE "}
                Dim index As Integer = If((strAddress.Contains("l_tell_1") OrElse strAddress.Contains("l_add_number")), 1, 0)
                Dim strArray3 As String() = New String() {"", "SELECT join_table.*,  committee_list_dtl_A.c_committee_id AS c_committee_id, " & "	" & "committee_list_dtl_A.s_committee_seq AS s_committee_seq FROM ("}
                Dim strArray4 As String() = New String() {"", String.Concat(New String() {") join_table, " & "	" & "(SELECT  " & "		" & "committee_list_dtl.c_user_id, " & "		" & "committee_list_dtl.c_committee_id, " & "		" & "committee_list_dtl.s_committee_seq " & "	" & "FROM " & "		" & "(SELECT" & "	" & "c_committee_list,Max(d_from) AS d_from " & "		" & "FROM" & "	" & "committee_list  " & "		" & "WHERE" & "	" & "c_ksh = :c_ksh ", strCommittee, "			" & "AND d_from <= :d_date " & "			" & "AND c_period_id = :c_period_id " & "		" & "GROUP BY " & "			" & "c_committee_list " & "		" & ") committee_list_A, " & "		" & "committee_list_dtl " & "	" & "WHERE " & "		" & "committee_list_dtl.c_committee_list = committee_list_A.c_committee_list " & "		" & "AND committee_list_dtl.d_from = committee_list_A.d_from " & "	" & ") committee_list_dtl_A WHERE " & "	" & "join_table.c_user_id = committee_list_dtl_A.c_user_id ORDER BY ", strSortOrder, " "})}
                Dim num2 As Integer = If((strCommittee.Trim.Length = 0), 0, 1)
                Dim command As New NpgsqlCommand(String.Concat(New String() {strArray3(num2), "	" & "SELECT ", str, ", ", str2, " " & "	" & "FROM " & "		" & "(SELECT" & "	" & "staf_attribute.* " & "		" & "FROM" & "	" & "staf_attribute, " & "			" & "(SELECT " & "				" & "c_user_id, " & "				" & "MAX(d_from) AS d_from " & "			" & "FROM " & "				" & "staf_attribute " & "			" & "WHERE " & "				" & "c_ksh = :c_ksh " & "				" & "AND d_from <= :d_date " & "				" & "AND k_del = :k_del " & "			" & "GROUP BY c_user_id) staf_attribute_A " & "		" & "WHERE " & "			" & "staf_attribute.c_user_id = staf_attribute_A.c_user_id " & "			" & "AND staf_attribute.d_from = staf_attribute_A.d_from " & "			" & "AND staf_attribute.k_del = :k_del ", strAttribute, " ) staf_attribute_AA    LEFT OUTER JOIN staf_kind_view KINDVIEW ON    (    KINDVIEW.d_from    <= :d_date     and KINDVIEW.d_to      >= :d_date     and KINDVIEW.c_constant_seq = staf_attribute_AA.k_staf_kind )    LEFT OUTER JOIN user_status_view USERVIEW ON    (    USERVIEW.d_from    <= :d_date     and USERVIEW.d_to      >= :d_date     and USERVIEW.c_constant_seq = staf_attribute_AA.k_user_status )    LEFT OUTER JOIN belonging_view BELOVIEW ON    (    BELOVIEW.d_from    <= :d_date     and BELOVIEW.d_to      >= :d_date     and BELOVIEW.c_constant_seq = staf_attribute_AA.k_belonging )    LEFT OUTER JOIN qualification_view QUALIVIEW ON    (    QUALIVIEW.d_from    <= :d_date     and QUALIVIEW.d_to      >= :d_date     and QUALIVIEW.c_constant_seq = staf_attribute_AA.k_qualification )    LEFT OUTER JOIN model_view MODELVIEW ON    (    MODELVIEW.d_from    <= :d_date     and MODELVIEW.d_to      >= :d_date     and MODELVIEW.c_constant_seq = staf_attribute_AA.k_model )    LEFT OUTER JOIN area_local_view LOCALVIEW ON    (    LOCALVIEW.d_from    <= :d_date     and LOCALVIEW.d_to      >= :d_date     and LOCALVIEW.c_constant_seq = staf_attribute_AA.k_local )    LEFT OUTER JOIN work_place_view WORKVIEW ON    (    WORKVIEW.d_from    <= :d_date     and WORKVIEW.d_to      >= :d_date     and WORKVIEW.c_constant_seq = staf_attribute_AA.k_work_place )    LEFT OUTER JOIN ksh ON    (    ksh.d_from    <= :d_date     and ksh.d_to      >= :d_date     and ksh.c_ksh      = staf_attribute_AA.c_trans_ksh )    LEFT OUTER JOIN area ON    (    area.d_from    <= :d_date     and area.d_to      >= :d_date     and area.c_ksh      = staf_attribute_AA.c_ksh     and area.c_ksh      = staf_attribute_AA.c_area )    LEFT OUTER JOIN work_state_view STATEVIEW ON    (    STATEVIEW.d_from    <= :d_date     and STATEVIEW.d_to      >= :d_date     and STATEVIEW.c_constant_seq = staf_attribute_AA.k_work_state ) ", strArray(index), "			" & "(SELECT " & "				" & "staf_address.* " & "			" & "FROM " & "				" & "staf_address, " & "				" & "(SELECT " & "					" & "c_user_id, " & "					" & "MAX(d_from) AS d_from " & "				" & "FROM " & "					" & "staf_address " & "				" & "WHERE " & "					" & "d_from <= :d_date " & "					" & "AND k_main_add = :k_main_add " & "					" & "AND k_del = :k_del " & "				" & "GROUP BY c_user_id) staf_address_A " & "			" & "WHERE " & "				" & "    staf_address.c_user_id  = staf_address_A.c_user_id " & "				" & "AND staf_address.k_main_add = :k_main_add " & "				" & "AND staf_address.d_from     = staf_address_A.d_from ", strAddress, " ) staf_address_AA ", strArray2(index), "			" & "staf_attribute_AA.c_user_id = staf_address_AA.c_user_id " & "	" & "ORDER BY ", strSortOrder, " ", strArray4(num2)}), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_main_add", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_period_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_foreign", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("c_period_id").Value = strPeriodId
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("k_main_add").Value = "1"
                command.Parameters.Item("k_period_kind").Value = "01"
                command.Parameters.Item("k_foreign").Value = "0"
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DI0001", New String(0 - 1) {})
                End If
                table = MyBase.CreateSomeDataSet("search_list_all", dReader)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            End Try
            Return table
        End Function

        Public Function GetStafAttributeOfStafKind(ByVal strUserId As String, ByVal strDate As String) As Integer
            Dim num As Integer
            Try
                Dim cmdText As String = "select count(a.c_user_id) from staf_attribute a,   (select c_user_id, max(d_from) as d_from from staf_attribute     where c_ksh     = :c_ksh           and k_del     = :k_del           and d_from   <= :d_from          and c_user_id = :c_user_id    group by c_user_id) b         where a.d_from    = b.d_from       and a.c_user_id = b.c_user_id    and a.k_staf_kind IN ('01', '04', '02')   and a.k_user_status = :k_user_status "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_user_status", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("d_from").Value = strDate
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("k_user_status").Value = "01"
                num = Convert.ToInt32(command.ExecuteScalar.ToString)
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
            Return num
        End Function

        Public Function GetStafDataForUpdate(ByVal strUserId As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "select d_up, s_up from staf_attribute where c_user_id = :c_user_id    and d_from    = :d_from    and k_del     = :k_del    and c_ksh     = :c_ksh  for update"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("c_user_id").Value = strUserId
                command.Parameters.Item("d_from").Value = strDate
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
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

        'Public Function GetStafName(ByVal strUserId As String, ByVal strDate As String) As String
        '    Dim str2 As String
        '    Try
        '        Dim cmdText As String = "SELECT " & "	" & "l_name FROM " & "	" & "staf_attribute WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND d_from <= :d_date "
        '        Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
        '        command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
        '        command.Parameters.Item("c_user_id").Value = strUserId
        '        command.Parameters.Item("d_date").Value = strDate
        '        Dim dReader As NpgsqlDataReader = command.ExecuteReader
        '        If Not dReader.HasRows Then
        '            'Dim class2 As New FactoryDaoClass
        '            'Dim dao As IFullTimeUnionOfficerDao = DirectCast(class2.GetObject("DAO.Member.FullTimeUnionOfficerDao"), IFullTimeUnionOfficerDao)
        '            Dim dao As New FullTimeUnionOfficerDao
        '            Return dao.GetStafName(strUserId, strDate)
        '        End If
        '        str2 = MyBase.CreateSomeDataSet("staf_attribute", dReader).Rows.Item(0).Item("l_name").ToString
        '    Catch exception As AppUnionException
        '        exception.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception
        '    Catch exception2 As SysUnionException
        '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception2
        '    Catch exception3 As NpgsqlException
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
        '    Catch exception4 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
        '    End Try
        '    Return str2
        'End Function

        'Public Function GetStafPersonalData(ByVal strUserId As String, ByVal strDate As String) As DataSet
        '    Dim set3 As DataSet
        '    Try
        '        Dim map As New Staf_AttributeMap
        '        Dim command As New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM staf_attribute WHERE " & "	" & "c_user_id = :c_user_id " & "	" & "AND d_from <= :d_date "), MyBase.GetNpgsqlConnection)
        '        command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
        '        command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
        '        command.Parameters.Item("c_user_id").Value = strUserId
        '        command.Parameters.Item("d_date").Value = strDate
        '        Dim dReader As NpgsqlDataReader = command.ExecuteReader
        '        If Not dReader.HasRows Then
        '            'Dim class2 As New FactoryDaoClass
        '            Dim dao As New FullTimeUnionOfficerDao
        '            'Dim stafPersonalData As DataSet = DirectCast(class2.GetObject("DAO.Member.FullTimeUnionOfficerDao"), IFullTimeUnionOfficerDao).GetStafPersonalData(strUserId, strDate)
        '            Dim stafPersonalData As DataSet = dao.GetStafPersonalData(strUserId, strDate)
        '            If (stafPersonalData Is Nothing) Then
        '                stafPersonalData.Tables.Add(map.CreateDataTablePhysName("staf_attribute"))
        '            End If
        '            Return stafPersonalData
        '        End If
        '        Dim set2 As New DataSet
        '        set2.Tables.Add(MyBase.CreateSomeDataSet("staf_attribute", dReader))
        '        set3 = set2
        '    Catch exception As AppUnionException
        '        exception.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception
        '    Catch exception2 As SysUnionException
        '        exception2.AddMethodName(MethodBase.GetCurrentMethod)
        '        Throw exception2
        '    Catch exception3 As NpgsqlException
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0002", New String(0 - 1) {})
        '    Catch exception4 As Exception
        '        Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
        '    End Try
        '    Return set3
        'End Function

        Public Function GetStafSearchList(ByVal strStafId As String, ByVal strKna As String, ByVal strStafKind As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim str As String = "select " & "	" & "A.c_staf_id as " & "社員番号" & ",  " & "	" & "A.l_name    as " & "名前" & ",   " & "	" & "(select " & "		" & "l_name " & "	" & "from " & "		" & "belonging_view " & "	" & "where  " & "		" & "A.k_belonging = c_constant_seq " & "	" & "and" & "	" & "d_from <= A.d_from " & "	" & "and" & "	" & "A.d_from <= d_to " & "	" & ") as " & "組合支部" & ",   " & "	" & "(select " & "		" & "l_name " & "	" & "from " & "		" & "staf_kind_view " & "	" & "where  " & "		" & "A.k_staf_kind = c_constant_seq " & "	" & "and" & "	" & "d_from <= A.d_from " & "	" & "and" & "	" & "A.d_from <= d_to " & "	" & ") as " & "組合員種別" & ",   " & "	" & "(select " & "		" & "l_name " & "	" & "from " & "		" & "model_view " & "	" & "where  " & "		" & "A.k_model = c_constant_seq " & "	" & "and" & "	" & "d_from <= A.d_from " & "	" & "and" & "	" & "A.d_from <= d_to " & "	" & ") as " & "機種" & ",   " & "	" & "(select " & "		" & "l_name " & "	" & "from " & "		" & "qualification_view " & "	" & "where  " & "		" & "A.k_qualification = c_constant_seq " & "	" & "and" & "	" & "d_from <= A.d_from " & "	" & "and" & "	" & "A.d_from <= d_to " & "	" & ") as " & "資格" & ",   " & "	" & "(select " & "		" & "l_name " & "	" & "from " & "		" & "user_status_view " & "	" & "where  " & "		" & "A.k_user_status = c_constant_seq " & "	" & "and" & "	" & "d_from <= A.d_from " & "	" & "and" & "	" & "A.d_from <= d_to " & "	" & ") as " & "ステータス" & ",   " & "	" & "(select " & "		" & "l_name " & "	" & "from " & "		" & "area_local_view " & "	" & "where  " & "		" & "A.k_local = c_constant_seq " & "	" & "and" & "	" & "d_from <= A.d_from " & "	" & "and" & "	" & "A.d_from <= d_to " & "	" & ") as " & "会社所属" & ",   " & "	" & "A.d_join    as " & "加入年月日" & ",   " & "	" & "A.c_user_id as " & "個人認証ＩＤ" & ",   " & "	" & "to_char(to_date(A.d_from, 'yyyyMMdd'), 'yyyy/MM/dd')   as " & "適用日付" & ",   " & "	" & "(case when " & "		" & "(select  " & "			" & "count(S_A.c_user_id)  " & "		" & "from  " & "			" & "(select " & "				" & "S_Abasis.c_user_id, " & "				" & "S_Abasis.d_from, " & "				" & "S_Abasis.c_staf_id_old, " & "				" & "S_Abasis.k_del, " & "				" & "S_Abasis.c_ksh " & "			" & "from " & "				" & "(select " & "					" & "c_user_id, max(d_from) as d_from " & "				" & "from " & "					" & "staf_attribute " & "				" & "where " & "					" & "c_ksh = :c_ksh " & "				" & "and" & "	" & "k_del = :k_del " & "				" & "group by " & "					" & "c_user_id " & "				" & ") maxPK, " & "				" & "staf_attribute S_Abasis " & "			" & "where " & "				" & "maxPK.c_user_id = S_Abasis.c_user_id " & "			" & "and" & "	" & "maxPK.d_from = S_Abasis.d_from " & "			" & ") S_A " & "		" & "where  " & "			" & "S_A.c_staf_id_old = A.c_staf_id  " & "		" & "and" & "	" & "S_A.k_del = :k_del  " & "		" & "and" & "	" & "S_A.c_ksh = :c_ksh  " & "		" & "and" & "	" & "S_A.c_staf_id_old is not NULL  " & "		" & "and S_A.c_user_id <> A.c_user_id " & "		" & "and S_A.d_from <> A.d_from " & "		" & ") <> 0  " & "	" & "then  " & "		" & "true  " & "	" & "else  " & "		" & "false  " & "	" & "end) as " & "シニア前情報フラグ" & ",   (case when      A.d_from > :d_from   then      true   else      false   end) as " & "未来日データフラグ" & " from  " & "	" & "(select  " & "		" & "AA.*  " & "	" & "from  " & "		" & "staf_attribute AA,  " & "		" & "(select  " & "			" & "c_user_id, max(d_from) as d_from  " & "		" & "from " & "			" & "staf_attribute  " & "		" & "where " & "			" & "c_ksh = :c_ksh " & "			" & "and k_del = :k_del  " & "		" & "group by  " & "			" & "c_user_id) BB  " & "	" & "where  " & "		" & "AA.c_user_id = BB.c_user_id  " & "		" & "and AA.k_del = :k_del  " & "		" & "and AA.d_from = BB.d_from  " & "	" & ") A where " & "	" & "    c_ksh = :c_ksh  and k_del = :k_del "
                If Not String.IsNullOrEmpty(strStafId) Then
                    str = (str & " and A.c_staf_id like '" & strStafId & "%' ")
                End If
                If Not String.IsNullOrEmpty(strKna) Then
                    str = (str & " and A.l_name_kna like '%" & strKna & "%' ")
                End If
                If Not String.IsNullOrEmpty(strStafKind) Then
                    str = (str & " and A.k_staf_kind = '" & strStafKind & "' ")
                End If
                Dim command As New NpgsqlCommand((str & " order by to_number(A.c_staf_id, '9999999999') "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("d_from").Value = PublicCommand.GetSystemDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("staf_attribute", dReader)
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

        Public Function GetTell1(ByVal strUserId As String, ByVal strDate As String) As String
            Dim str3 As String
            Try
                Dim str2 As String
                Dim cmdText As String = " select  STAF.l_tell_1 AS " & "電話番号" & "  from  staf_address STAF,  ( select c_user_id, max(d_from) as d_from     from  staf_address     where d_from    <= :d_from       and k_main_add = :k_main_add       and k_del      = :k_del       and c_user_id  = :c_user_id     group by c_user_id ) STAF2    where      STAF.c_user_id  = STAF2.c_user_id  and STAF.d_from     = STAF2.d_from  and STAF.c_user_id  = :c_user_id  and STAF.k_main_add = :k_main_add "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_main_add", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id", DbType.String))
                command.Parameters.Item("d_from").Value = strDate
                command.Parameters.Item("k_main_add").Value = "1"
                command.Parameters.Item("k_del").Value = "0"
                command.Parameters.Item("c_user_id").Value = strUserId
                If (command.ExecuteScalar Is Nothing) Then
                    str2 = ""
                Else
                    str2 = command.ExecuteScalar.ToString
                End If
                str3 = str2
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
            Return str3
        End Function

        Private Function GetWherePhraseForPrevData(ByVal strUserId As String, ByVal strInputDate As String) As String
            Dim str2 As String
            Try
                str2 = String.Concat(New String() {" where c_user_id = '", strUserId, "'    and d_from     > '", strInputDate, "'   and k_del      = :k_del   and c_ksh      = :c_ksh "})
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Dim exception4 As New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
                Throw exception4
            End Try
            Return str2
        End Function

        Public Function HasRowsOfStafId(ByVal strStafId As String, ByVal strStafKind As String) As Boolean
            Dim hasRows As Boolean
            Try
                Dim cmdText As String = "select c_user_id from staf_attribute where c_ksh     = :c_ksh   and c_staf_id = :c_staf_id   and k_staf_kind = :k_staf_kind   and k_del     = :k_del "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_staf_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_staf_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_del", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("c_staf_id").Value = strStafId
                command.Parameters.Item("k_staf_kind").Value = strStafKind
                command.Parameters.Item("k_del").Value = "0"
                hasRows = command.ExecuteReader.HasRows
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
            Return hasRows
        End Function

        Public Sub InsertStafAttributeData(ByVal dSetStafAttribute As DataSet)
            Try
                Dim map As New Staf_AttributeMap
                Dim str2 As String = map.ToPhysicalString("")
                Dim str3 As String = map.ToPhysicalString(":")
                Dim command As New NpgsqlCommand(String.Concat(New String() {"insert into staf_attribute ( ", str2, " ) values( ", str3, " ) "}), MyBase.GetNpgsqlConnection, MyBase.GetNpgsqlTran)
                Dim i As Integer
                For i = 0 To map.ColumnCount - 1
                    command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(i), map.GetDbDataType(i)))
                    command.Parameters.Item(map.GetPhysicalName(i)).Value = dSetStafAttribute.Tables.Item("staf_attribute").Rows.Item(0).Item(map.GetPhysicalName(i))
                Next i
                command.Parameters.Item("d_up").Value = DBNull.Value
                command.Parameters.Item("c_user_id_up").Value = DBNull.Value
                If (command.ExecuteNonQuery <> 1) Then
                    Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0001", New String(0 - 1) {})
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

        Public Sub UpdateExistData(ByVal dSetNewData As DataSet)
            Try
                Dim builder As New StringBuilder
                Dim map As New Staf_AttributeMap
                Dim i As Integer
                For i = 0 To map.ColumnCount - 1
                    builder.Append((map.GetPhysicalName(i) & " = :" & map.GetPhysicalName(i) & ", "))
                Next i
                builder.Remove((builder.Length - 2), 2)
                Dim command As New NpgsqlCommand(("update staf_attribute set " & builder.ToString & " where c_user_id  = :c_user_id   and d_from     = :d_from   and c_ksh      = :c_ksh   and k_del      = :k_del "), MyBase.GetNpgsqlConnection)
                Dim j As Integer
                For j = 0 To map.ColumnCount - 1
                    command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(j), map.GetDbDataType(j)))
                    command.Parameters.Item(map.GetPhysicalName(j)).Value = dSetNewData.Tables.Item("staf_attribute").Rows.Item(0).Item(map.GetPhysicalName(j))
                Next j
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
