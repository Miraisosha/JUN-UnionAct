Imports UnionAct.Framework.Command
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
    Friend Class RevenueExpenditureDao
        Inherits AbstractDao
        'Implements IRevenueExpenditureDao
        ' Methods
        Public Sub DeleteRevenueExpenditureDao(ByVal command As NpgsqlCommand, ByVal strRevenueStr As String)
            Try
                Dim cmdText As String = "delete from revenue_budgetary_process_dtl where d_revenue_str = :RevenuStr ; delete from revenue_budgetary_process where d_revenue_str = :RevenuStr ; delete from revenue_allotted_charge where d_revenue_str = :RevenuStr ; delete from revenue_expenditure_promotion_dtl where d_revenue_str = :RevenuStr ; delete from revenue_expenditure_member_dtl where d_revenue_str = :RevenuStr ; delete from revenue_expenditure where d_revenue_str =  :RevenuStr ; "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add(New NpgsqlParameter("RevenuStr", DbType.String))
                command.Parameters.Item("RevenuStr").Value = strRevenueStr
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

        Public Sub DeleteRevenueExpenditureMemberDao(ByVal command As NpgsqlCommand, ByVal strRevenueStr As String)
            Try
                Dim cmdText As String = "delete from revenue_expenditure_member_dtl where d_revenue_str = :RevenuStr ; "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add(New NpgsqlParameter("RevenuStr", DbType.String))
                command.Parameters.Item("RevenuStr").Value = strRevenueStr
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

        Public Function GetRevenueExpenditure(ByVal strDFrom As String) As DataTable
            Dim table2 As DataTable
            Try
                'TODO Dim cmdText As String = " SELECT  REV.l_title AS " & "ëËñ⁄" & ",  SUBSTRING(REV.d_revenue_str, 1, 4) || '/' || SUBSTRING(REV.d_revenue_str, 5, 2) || '/' || SUBSTRING(REV.d_revenue_str, 7, 2) AS " & "ó\ëzäJénì˙" & ",  SUBSTRING(REV.d_revenue_end, 1, 4) || '/' || SUBSTRING(REV.d_revenue_end, 5, 2) || '/' || SUBSTRING(REV.d_revenue_end, 7, 2) AS " & "ó\ëzèIóπì˙" & ",  CASE WHEN REV.k_revenue_expenditure = '0'       THEN '" & "êVãKìoò^" & "' ELSE '" & "è⁄ç◊ï\é¶" & "' END AS " & "é˚ì¸ó\ëzèÛãµ" & ",  CASE WHEN REV.k_revenue_member = '0'            THEN '" & "êVãKìoò^" & "' ELSE '" & "è⁄ç◊ï\é¶" & "' END AS " & "èÊàıåvâÊèÛãµ" & ",  CASE WHEN REV.k_revenue_allotted_charge = '0'   THEN '" & "êVãKìoò^" & "' ELSE '" & "è⁄ç◊ï\é¶" & "' END AS " & "ï™íSã‡èÛãµ" & ",  CASE WHEN REV.k_revenue_budgetary_process = '0' THEN '" & "êVãKìoò^" & "' ELSE '" & "è⁄ç◊ï\é¶" & "' END AS " & "ó\éZìoò^èÛãµ" & ",  CASE WHEN REV.k_revenue_budgetary_revise = '0' THEN '" & "êVãKìoò^" & "' ELSE '" & "è⁄ç◊ï\é¶" & "' END AS " & "èCê≥ó\éZèÛãµ" & ",  STAF.l_name AS " & "íSìñé“" & ",  TO_CHAR((SELECT COALESCE(d_up, d_ins) FROM revenue_expenditure REV2   WHERE REV.d_revenue_str = REV2.d_revenue_str), 'yyyy/MM/dd') AS " & "ìoò^ì˙" & ", REV.d_revenue_str, REV.d_revenue_end, REV.l_title, REV.k_revenue_expenditure, REV.k_revenue_member, REV.k_revenue_allotted_charge, REV.k_revenue_budgetary_process, REV.k_revenue_budgetary_revise, REV.k_revenue_seton, REV.s_revenue_expenditure_ttl, REV.s_revise_revenue_ttl, REV.s_general_account_molecule, REV.s_general_account_denominator, REV.s_general_account, REV.s_senior_monthwork, REV.s_new_staff_average, REV.s_cap_promotion_average, REV.s_unpromotion_persons, REV.s_unpromotion_rate, REV.s_unpromotion_average, REV.s_senior_stay_rate, REV.s_senior_average, REV.s_new_union_monthwork, REV.s_cap_up_monthwork, REV.s_cap_retire_monthwork, REV.s_cop_retire_monthwork, REV.s_fe_retire_monthwork, REV.s_unpromotion_persons_monthwork, REV.s_senior_retire_monthwork, REV.s_budget_sub, REV.s_budget_total, REV.s_revise_budget_sub, REV.s_revise_budget_total, REV.l_biko_1, REV.l_biko_2, REV.l_biko_3, REV.d_ins, REV.d_up FROM revenue_expenditure REV  LEFT OUTER JOIN  ( SELECT STF1.l_name, STF1.c_user_id from staf_attribute_full_time_view STF1,       ( SELECT c_user_id, max(d_from) as d_from from staf_attribute_full_time_view           WHERE d_from <= :d_from         GROUP BY c_user_id ) STMAX    WHERE STF1.d_from    = STMAX.d_from      AND STF1.c_user_id = STMAX.c_user_id  ) STAF ON  (   (SELECT COALESCE(c_user_id_up, c_user_id_ins) AS c_user_id_up FROM revenue_expenditure REV3      WHERE REV.d_revenue_str = REV3.d_revenue_str) = STAF.c_user_id  ) ORDER BY d_revenue_str DESC "
                Dim cmdText As String = "SELECT  REV.l_title AS " & "ëËñ⁄" & ",  SUBSTRING(REV.d_revenue_str, 1, 4) & '/' & SUBSTRING(REV.d_revenue_str, 5, 2) & '/' & SUBSTRING(REV.d_revenue_str, 7, 2) AS " & "ó\ëzäJénì˙" & ",  SUBSTRING(REV.d_revenue_end, 1, 4) & '/' & SUBSTRING(REV.d_revenue_end, 5, 2) & '/' & SUBSTRING(REV.d_revenue_end, 7, 2) AS " & "ó\ëzèIóπì˙" & ",  IIF(REV.k_revenue_expenditure = '1', '" & "è⁄ç◊ï\é¶" & "' , '" & "êVãKìoò^" & "' ) AS " & "é˚ì¸ó\ëzèÛãµ" & ",  IIF( REV.k_revenue_member = '1'  , '" & "è⁄ç◊ï\é¶" & "' , '" & "êVãKìoò^" & "' ) AS " & "èÊàıåvâÊèÛãµ" & ",  IIF( REV.k_revenue_allotted_charge = '1' , '" & "è⁄ç◊ï\é¶" & "' , '" & "êVãKìoò^" & "' ) AS " & "ï™íSã‡èÛãµ" & ",  IIF( REV.k_revenue_budgetary_process = '1' , '" & "è⁄ç◊ï\é¶" & "' , '" & "êVãKìoò^" & "' ) AS " & "ó\éZìoò^èÛãµ" & ",  IIF( REV.k_revenue_budgetary_revise = '1' , '" & "è⁄ç◊ï\é¶" & "' , '" & "êVãKìoò^" & "' ) AS " & "èCê≥ó\éZèÛãµ" & ",  STAF.l_name AS " & "íSìñé“" & ",  TO_CHAR((SELECT IIF(d_up IS NULL, d_ins, d_up) FROM revenue_expenditure REV2   WHERE REV.d_revenue_str = REV2.d_revenue_str), 'yyyy/MM/dd') AS " & "ìoò^ì˙" & ", REV.d_revenue_str, REV.d_revenue_end, REV.l_title, REV.k_revenue_expenditure, REV.k_revenue_member, REV.k_revenue_allotted_charge, REV.k_revenue_budgetary_process, REV.k_revenue_budgetary_revise, REV.k_revenue_seton, REV.s_revenue_expenditure_ttl, REV.s_revise_revenue_ttl, REV.s_general_account_molecule, REV.s_general_account_denominator, REV.s_general_account, REV.s_senior_monthwork, REV.s_new_staff_average, REV.s_cap_promotion_average, REV.s_unpromotion_persons, REV.s_unpromotion_rate, REV.s_unpromotion_average, REV.s_senior_stay_rate, REV.s_senior_average, REV.s_new_union_monthwork, REV.s_cap_up_monthwork, REV.s_cap_retire_monthwork, REV.s_cop_retire_monthwork, REV.s_fe_retire_monthwork, REV.s_unpromotion_persons_monthwork, REV.s_senior_retire_monthwork, REV.s_budget_sub, REV.s_budget_total, REV.s_revise_budget_sub, REV.s_revise_budget_total, REV.l_biko_1, REV.l_biko_2, REV.l_biko_3, REV.d_ins, REV.d_up FROM revenue_expenditure REV  LEFT OUTER JOIN  ( SELECT STF1.l_name, STF1.c_user_id from staf_attribute_full_time_view STF1,       ( SELECT c_user_id, max(staf_attribute_full_time_view.d_from) as d_from from staf_attribute_full_time_view           WHERE d_from <= :d_from         GROUP BY c_user_id ) STMAX    WHERE STF1.d_from    = STMAX.d_from      AND STF1.c_user_id = STMAX.c_user_id  ) STAF ON  (   REV.c_user_id_ins = STAF.c_user_id  ) ORDER BY REV.d_revenue_str DESC "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("d_from").Value = strDFrom
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.CreateSomeDataSet("revenue_expenditure", dReader)
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
            Return table2
        End Function

        Public Function GetRevenueKbnDao(ByVal strDFrom As String, ByVal strControlName As String) As String
            Dim str3 As String
            Try
                Dim str As String = ""
                'If strControlName.Equals("CtlIncomeExpect") Then
                '    str = " COALESCE(REV.k_revenue_member, '') ||  COALESCE(REV.k_revenue_allotted_charge, '') ||  COALESCE(REV.k_revenue_budgetary_process, '') AS COMMNET "
                'ElseIf strControlName.Equals("CtlCrewPlan") Then
                '    str = " COALESCE(REV.k_revenue_allotted_charge, '') ||  COALESCE(REV.k_revenue_budgetary_process, '') AS COMMNET "
                'ElseIf strControlName.Equals("CtlAllottedCharge") Then
                '    str = " COALESCE(REV.k_revenue_budgetary_process, '') AS COMMNET "
                'Else
                '    Return ""
                'End If
                If strControlName.Equals("CtlIncomeExpect") Then
                    str = " IIF(ISNULL(REV.k_revenue_member), '', REV.k_revenue_member) &  IIF(ISNULL(REV.k_revenue_allotted_charge), '', REV.k_revenue_allotted_charge) &  IIF(ISNULL(REV.k_revenue_budgetary_process), '', REV.k_revenue_budgetary_process) AS COMMNET "
                ElseIf strControlName.Equals("CtlCrewPlan") Then
                    str = " IIF(ISNULL(REV.k_revenue_allotted_charge), '', REV.k_revenue_allotted_charge) &  IIF(ISNULL(REV.k_revenue_budgetary_process), '', REV.k_revenue_budgetary_process) AS COMMNET "
                ElseIf strControlName.Equals("CtlAllottedCharge") Then
                    str = " IIF(ISNULL(REV.k_revenue_budgetary_process), '', REV.k_revenue_budgetary_process) AS COMMNET "
                Else
                    Return ""
                End If
                'Dim command As New NpgsqlCommand((" SELECT " & str & " from  " & "	" & "(select  " & "	" & " CASE WHEN k_revenue_member = '1' THEN '" & "èÊàıåvâÊèÛãµÅC" & "' ELSE '' END AS k_revenue_member,  " & "	" & " CASE WHEN k_revenue_allotted_charge = '1' THEN '" & "ï™íSã‡èÛãµÅC" & "' ELSE '' END AS k_revenue_allotted_charge,  " & "	" & " CASE WHEN k_revenue_budgetary_process = '1' THEN '" & "ó\éZìoò^èÛãµ" & "' ELSE '' END AS k_revenue_budgetary_process  " & "	" & "from  " & "	" & " revenue_expenditure  " & "	" & "where  " & "	" & " d_revenue_str = :d_from  " & "	" & ") REV "), MyBase.GetNpgsqlConnection)
                Dim command As New NpgsqlCommand((" SELECT " & str & " from  " & "	" & "(select  " & "	" & " IIF( revenue_expenditure.k_revenue_member = '1' , '" & "èÊàıåvâÊèÛãµ" & "',  '' ) AS k_revenue_member,  " & "	" & " IIF( revenue_expenditure.k_revenue_allotted_charge = '1' , '" & "ï™íSã‡èÛãµ" & "',  '' ) AS k_revenue_allotted_charge,  " & "	" & " IIF( revenue_expenditure.k_revenue_budgetary_process = '1' , '" & "ó\éZìoò^èÛãµ" & "' , '') AS k_revenue_budgetary_process  " & "	" & "from  " & "	" & " revenue_expenditure  " & "	" & "where  " & "	" & " d_revenue_str = :d_from  " & "	" & ") REV "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("d_from").Value = strDFrom
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("revenue_expenditure", dReader)
                If (table.Rows.Count <= 0) Then
                    Return ""
                End If
                str3 = table.Rows.Item(0).Item(0).ToString
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
            Return str3
        End Function

        Public Function GetTargetMemberCount(ByVal strRevenue As String, ByVal strBasisDate As String) As Integer
            Dim num As Integer
            Try
                'Dim cmdText As String = "select COALESCE(MEM_DTL.s_real_number_ttl, 0) + COALESCE(PRM_DTL.s_new_staff_member, 0) - ( COALESCE(PRM_DTL.s_cap_retire_member, 0) + COALESCE(PRM_DTL.s_cop_retire_member, 0) + COALESCE(PRM_DTL.s_fe_retire_member, 0) ) as target_member_count from (select d_revenue_str, sum(s_real_number_ttl) as s_real_number_ttl from revenue_expenditure_member_dtl where d_revenue_str = :d_revenue_str group by d_revenue_str ) MEM_DTL left outer join (select d_revenue_str, sum(s_new_staff_member) as s_new_staff_member, sum(s_cap_retire_member) as s_cap_retire_member, sum(s_cop_retire_member) as s_cop_retire_member, sum(s_fe_retire_member) as s_fe_retire_member from revenue_expenditure_promotion_dtl where d_revenue_str = :d_revenue_str and d_target >= :d_revenue_str and d_target <= :basis_date group by d_revenue_str ) PRM_DTL on MEM_DTL.d_revenue_str = PRM_DTL.d_revenue_str "
                Dim cmdText As String = "select IIF(ISNULL(MEM_DTL.s_real_number_ttl), 0, MEM_DTL.s_real_number_ttl) + IIF(ISNULL(PRM_DTL.s_new_staff_member), 0, PRM_DTL.s_new_staff_member) - ( IIF(ISNULL(PRM_DTL.s_cap_retire_member), 0, PRM_DTL.s_cap_retire_member) + IIF(ISNULL(PRM_DTL.s_cop_retire_member), 0, PRM_DTL.s_cop_retire_member) + IIF(ISNULL(PRM_DTL.s_fe_retire_member), 0, PRM_DTL.s_fe_retire_member) ) as target_member_count from (select d_revenue_str, sum(revenue_expenditure_member_dtl.s_real_number_ttl) as s_real_number_ttl from revenue_expenditure_member_dtl where d_revenue_str = :d_revenue_str group by d_revenue_str ) MEM_DTL left outer join (select d_revenue_str, sum(revenue_expenditure_promotion_dtl.s_new_staff_member) as s_new_staff_member, sum(revenue_expenditure_promotion_dtl.s_cap_retire_member) as s_cap_retire_member, sum(revenue_expenditure_promotion_dtl.s_cop_retire_member) as s_cop_retire_member, sum(revenue_expenditure_promotion_dtl.s_fe_retire_member) as s_fe_retire_member from revenue_expenditure_promotion_dtl where d_revenue_str = :d_revenue_str and d_target >= :d_revenue_str and d_target <= :basis_date group by d_revenue_str ) PRM_DTL on MEM_DTL.d_revenue_str = PRM_DTL.d_revenue_str "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Add("basis_date", DbType.String)
                command.Parameters.Item("d_revenue_str").Value = strRevenue
                command.Parameters.Item("basis_date").Value = strBasisDate
                Dim obj2 As Object = command.ExecuteScalar
                If (Not obj2 Is Nothing) Then
                    Return Convert.ToInt32(obj2)
                End If
                num = 0
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

        Public Function GetTimeStamp(ByVal strRevenue As String) As DataSet
            Return GetTimeStamp(New NpgsqlCommand("", MyBase.GetNpgsqlConnection), strRevenue)
        End Function

        Public Function GetTimeStamp(ByVal command As NpgsqlCommand, ByVal strRevenue As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim cmdText As String = "select d_ins, d_up from revenue_expenditure where d_revenue_str = :d_revenue_str"
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Clear()
                command.SetSql(cmdText)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Item("d_revenue_str").Value = strRevenue
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("revenue_expenditure", dReader))
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

        Public Sub InsertRevenueExpenditure(ByVal command As NpgsqlCommand, ByVal dTableIn As DataTable)
            Try
                Dim cmdText As String = "insert into revenue_expenditure( d_revenue_str, d_revenue_end, l_title, k_revenue_expenditure, s_revenue_expenditure_ttl, s_senior_monthwork, d_ins, c_user_id_ins, s_up ) values ( :d_revenue_str, :d_revenue_end, :l_title, :k_revenue_expenditure, :s_revenue_expenditure_ttl, :s_senior_monthwork, CONVERT(DATE,':d_ins'), :c_user_id_ins, :s_up)"
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection, MyBase.GetNpgsqlTran)
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add(New NpgsqlParameter("d_revenue_str", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_revenue_end", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("l_title", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_revenue_expenditure", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_revenue_expenditure_ttl", DbType.Int64))
                command.Parameters.Add(New NpgsqlParameter("s_senior_monthwork", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_ins", DbType.DateTime))
                command.Parameters.Add(New NpgsqlParameter("c_user_id_ins", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_up", DbType.Int16))
                command.Parameters.Item("d_revenue_str").Value = dTableIn.Rows.Item(0).Item("d_revenue_str").ToString
                command.Parameters.Item("d_revenue_end").Value = dTableIn.Rows.Item(0).Item("d_revenue_end").ToString
                command.Parameters.Item("l_title").Value = dTableIn.Rows.Item(0).Item("l_title").ToString
                command.Parameters.Item("k_revenue_expenditure").Value = "1"
                command.Parameters.Item("s_revenue_expenditure_ttl").Value = Convert.ToInt64(dTableIn.Rows.Item(0).Item("s_revenue_expenditure_ttl"))
                command.Parameters.Item("s_senior_monthwork").Value = Convert.ToInt32(dTableIn.Rows.Item(0).Item("s_senior_monthwork"))
                command.Parameters.Item("d_ins").Value = PublicCommand.GetNow
                command.Parameters.Item("c_user_id_ins").Value = dTableIn.Rows.Item(0).Item("c_user_id_ins").ToString
                command.Parameters.Item("s_up").Value = 0
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

        Public Sub InsertRevenueExpenditureMemberDtl(ByVal command As NpgsqlCommand, ByVal dTableIn As DataTable)
            Try
                Dim map As New RevenueExpenditureMemmberDtlMap
                Dim str2 As String = map.ToPhysicalString("")
                Dim str3 As String = map.ToPhysicalString(":")
                Dim cmdText As String = String.Concat(New String() {"insert into revenue_expenditure_member_dtl( ", str2, " ) values( ", str3, " ) "})
                cmdText = cmdText.Replace(":d_ins", "CONVERT(DATE,:d_ins)")
                cmdText = cmdText.Replace(":d_up", "CONVERT(DATE,:d_up)")
                Dim i As Integer
                For i = 0 To dTableIn.Rows.Count - 1
                    'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection, MyBase.GetNpgsqlTran)
                    command.SetSql(cmdText)
                    command.Parameters.Clear()
                    Dim j As Integer
                    For j = 0 To map.ColumnCount - 1
                        command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(j), map.GetDbDataType(j)))
                        command.Parameters.Item(map.GetPhysicalName(j)).Value = dTableIn.Rows.Item(i).Item(map.GetPhysicalName(j))
                    Next j
                    command.Parameters.Item("d_ins").intColmunType = DbType.String
                    command.Parameters.Item("d_up").intColmunType = DbType.String
                    command.Parameters.Item("d_up").Value = command.Parameters.Item("d_ins").objValue
                    command.Parameters.Item("c_user_id_up").Value = command.Parameters.Item("c_user_id_ins").objValue
                    command.Parameters.Item("s_up").Value = 0
                    command.ExecuteNonQuery()
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

        Public Sub UpDateRevenueExpenditureDao(ByVal command As NpgsqlCommand, ByVal dTableIn As DataTable)
            Try
                Dim cmdText As String = "UPDATE revenue_expenditure SET s_revenue_expenditure_ttl = :s_revenue_expenditure_ttl, s_senior_monthwork = :s_senior_monthwork, d_up = CONVERT(DATE,:d_up), c_user_id_up = :c_user_id_up, s_up = s_up + 1 WHERE d_revenue_str = :d_revenue_str "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add(New NpgsqlParameter("d_revenue_str", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_revenue_expenditure_ttl", DbType.Int64))
                command.Parameters.Add(New NpgsqlParameter("s_senior_monthwork", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_up", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_user_id_up", DbType.String))
                command.Parameters.Item("d_revenue_str").Value = dTableIn.Rows.Item(0).Item("d_revenue_str").ToString
                command.Parameters.Item("s_revenue_expenditure_ttl").Value = Convert.ToInt64(dTableIn.Rows.Item(0).Item("s_revenue_expenditure_ttl"))
                command.Parameters.Item("s_senior_monthwork").Value = Convert.ToInt32(dTableIn.Rows.Item(0).Item("s_senior_monthwork"))
                command.Parameters.Item("d_up").Value = PublicCommand.GetNow
                command.Parameters.Item("c_user_id_up").Value = dTableIn.Rows.Item(0).Item("c_user_id_ins").ToString
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

        Public Sub UpdateRevenueExpenditureForAllotted(ByVal command As NpgsqlCommand, ByVal strRevenue As String, ByVal dTimeUp As DateTime, ByVal strUserID As String)
            Try
                Dim cmdText As String = "update revenue_expenditure set k_revenue_allotted_charge = :k_revenue_allotted_charge, d_up = CONVERT(DATE,:d_up), c_user_id_up = :c_user_id_up, s_up = s_up + 1 where d_revenue_str = :d_revenue_str "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add("k_revenue_allotted_charge", DbType.String)
                command.Parameters.Add("d_up", DbType.String)
                command.Parameters.Add("c_user_id_up", DbType.String)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Item("k_revenue_allotted_charge").Value = "1"
                command.Parameters.Item("d_up").Value = dTimeUp
                command.Parameters.Item("c_user_id_up").Value = strUserID
                command.Parameters.Item("d_revenue_str").Value = strRevenue
                If (command.ExecuteNonQuery <> 1) Then
                    Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0002", New String(0 - 1) {})
                End If
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

        Public Sub UpdateRevenueExpenditureForBudgetary(ByVal command As NpgsqlCommand, ByVal dSetNew As DataSet, ByVal isRevise As Boolean)
            Try
                'Dim command As New NpgsqlCommand(("update revenue_expenditure set " & If(isRevise, "k_revenue_budgetary_revise = :k_revenue_budgetary_revise, ", "k_revenue_budgetary_process = :k_revenue_budgetary_process, ") & If(isRevise, "s_revise_budget_sub = :s_revise_budget_sub, s_revise_budget_total = :s_revise_budget_total, ", "s_budget_sub = :s_budget_sub, s_budget_total = :s_budget_total, ") & "d_up = :d_up, c_user_id_up = :c_user_id_up, s_up = s_up + 1 where d_revenue_str = :d_revenue_str "), MyBase.GetNpgsqlConnection)
                Dim cmdText As String = "update revenue_expenditure set " & If(isRevise, "k_revenue_budgetary_revise = :k_revenue_budgetary_revise, ", "k_revenue_budgetary_process = :k_revenue_budgetary_process, ") & If(isRevise, "s_revise_budget_sub = :s_revise_budget_sub, s_revise_budget_total = :s_revise_budget_total, ", "s_budget_sub = :s_budget_sub, s_budget_total = :s_budget_total, ") & "d_up = CONVERT(DATE,:d_up), c_user_id_up = :c_user_id_up, s_up = s_up + 1 where d_revenue_str = :d_revenue_str "
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add("k_revenue_budgetary_revise", DbType.String)
                command.Parameters.Add("k_revenue_budgetary_process", DbType.String)
                command.Parameters.Add("s_revise_budget_sub", DbType.Int32)
                command.Parameters.Add("s_revise_budget_total", DbType.Int32)
                command.Parameters.Add("s_budget_sub", DbType.Int32)
                command.Parameters.Add("s_budget_total", DbType.Int32)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Add("d_up", DbType.String)
                command.Parameters.Add("c_user_id_up", DbType.String)
                command.Parameters.Item("k_revenue_budgetary_revise").Value = "1"
                command.Parameters.Item("k_revenue_budgetary_process").Value = "1"
                command.Parameters.Item("s_revise_budget_sub").Value = dSetNew.Tables.Item("revenue_expenditure").Rows.Item(0).Item("s_revise_budget_sub")
                command.Parameters.Item("s_revise_budget_total").Value = dSetNew.Tables.Item("revenue_expenditure").Rows.Item(0).Item("s_revise_budget_total")
                command.Parameters.Item("s_budget_sub").Value = dSetNew.Tables.Item("revenue_expenditure").Rows.Item(0).Item("s_budget_sub")
                command.Parameters.Item("s_budget_total").Value = dSetNew.Tables.Item("revenue_expenditure").Rows.Item(0).Item("s_budget_total")
                command.Parameters.Item("d_revenue_str").Value = dSetNew.Tables.Item("revenue_expenditure").Rows.Item(0).Item("d_revenue_str")
                command.Parameters.Item("d_up").Value = dSetNew.Tables.Item("revenue_expenditure").Rows.Item(0).Item("d_up")
                command.Parameters.Item("c_user_id_up").Value = dSetNew.Tables.Item("revenue_expenditure").Rows.Item(0).Item("c_user_id_up")
                If (command.ExecuteNonQuery <> 1) Then
                    Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0002", New String(0 - 1) {})
                End If
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

        Public Sub UpdateRevenueSetonFlag(ByVal strRevenueDate As String, ByVal dTimeUp As DateTime, ByVal strUserIDup As String)
            Try
                Dim cmdText As String = "update revenue_expenditure set k_revenue_seton = :k_revenue_seton, d_up = CONVERT(DATE,:d_up), c_user_id_up = :c_user_id_up, s_up = s_up + 1 where d_revenue_str = :d_revenue_str "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("k_revenue_seton", DbType.String)
                command.Parameters.Add("d_up", DbType.String)
                command.Parameters.Add("c_user_id_up", DbType.String)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Item("k_revenue_seton").Value = "1"
                command.Parameters.Item("d_up").Value = dTimeUp
                command.Parameters.Item("c_user_id_up").Value = strUserIDup
                command.Parameters.Item("d_revenue_str").Value = strRevenueDate
                If (command.ExecuteNonQuery <> 1) Then
                    Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0002", New String(0 - 1) {})
                End If
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0002", New String(0 - 1) {})
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

    End Class
End Namespace
