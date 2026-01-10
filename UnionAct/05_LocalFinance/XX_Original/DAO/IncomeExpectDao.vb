#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.NSMDConst

Namespace DAO.RevenueExpenditure
    Public Class IncomeExpectDao
        Inherits AbstractDao
        'Implements IIncomeExpectDao
        ' Methods
        'Public Function GetDuesWork(ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strKeyAgeDate As String, ByVal strKsh As String, ByVal strStafKind As String) As Integer
        '    Dim num As Integer
        '    Try
        '        Dim cmdText As String = "UnionDuesWorkMoneyCountHeadsCreate( :strMac, :strControlName, :strUserId, :strStr, :strKeyAgeDate, :strKsh, :strStafKind )"
        '        Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection) With { _
        '            .CommandType = CommandType.StoredProcedure _
        '        }
        '        command.Parameters.Add("strMac", strMac)
        '        command.Parameters.Add("strControlName", strControlName)
        '        command.Parameters.Add("strUserId", strUserId)
        '        command.Parameters.Add("strStr", strStr)
        '        command.Parameters.Add("strKeyAgeDate", strKeyAgeDate)
        '        command.Parameters.Add("strKsh", strKsh)
        '        command.Parameters.Add("strStafKind", strStafKind)
        '        Dim obj2 As Object = command.ExecuteScalar
        '        num = If(((obj2 Is Nothing) OrElse TypeOf obj2 Is DBNull), 0, CInt(obj2))
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
        '    Return num
        'End Function

        ' 収支予想集計(年齢別・資格別の組合費集計）
        Public Function GetDuesWork(ByVal command As NpgsqlCommand, ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strKeyAgeDate As String, ByVal strKsh As String, ByVal strStafKind As String, Optional ByVal fDelete As Boolean = False) As Integer
            Dim num As Integer = 0

            ' 既存レコード削除
            If fDelete Then
                DeleteCurrentRec(command, strStr)
            End If

            ' 年齢別組合費集計処理
            Dim table As DataTable = SummaryDues(command, strStr, strKeyAgeDate, strKsh, strStafKind)

            ' 搭乗資格別データ作成
            If table.Rows.Count > 0 Then
                num += CreateNewData(command, strMac, strControlName, strUserId, strStr, MDConst.QUALIFICATION_PILOT, table)
                num += CreateNewData(command, strMac, strControlName, strUserId, strStr, MDConst.QUALIFICATION_COPILOT, table)
                num += CreateNewData(command, strMac, strControlName, strUserId, strStr, MDConst.QUALIFICATION_FLIGHT_ENGINEER, table)
            End If

            Return num
        End Function

        ' 既存レコード削除
        Private Sub DeleteCurrentRec(ByVal command As NpgsqlCommand, ByVal strRevenue As String)
            command.SetSql("DELETE FROM revenue_expenditure_member_dtl_view")
            'command.SetSql("DELETE FROM revenue_expenditure_member_dtl_view WHERE d_revenue_str=:d_revenue_str")
            'command.Parameters.Add(New NpgsqlParameter("d_revenue_str", DbType.String))
            'command.Parameters.Item("d_revenue_str").Value = strRevenue
            command.ExecuteNonQuery()
        End Sub

        ' 年齢別組合費集計処理
        Public Function SummaryDues(ByVal command As NpgsqlCommand, ByVal strStr As String, ByVal strKeyAgeDate As String, ByVal strKsh As String, ByVal strStafKind As String) As DataTable
            Dim cmdText As String
            cmdText = "SELECT STAFF.*, DUES.s_union_dues, STAFF.NUM * DUES.s_union_dues * 12 AS sum_union_dues" & _
                      " FROM (SELECT IIF(RIGHT(:AgeBase, 4) < FORMAT(ATTR.d_birth, 'MMdd'), CInt(LEFT(:AgeBase, 4))-CInt(FORMAT(ATTR.d_birth, 'yyyy'))-1, CInt(LEFT(:AgeBase, 4))-CInt(FORMAT(ATTR.d_birth, 'yyyy'))) AS AGE, COUNT(*) AS NUM, ATTR.k_staf_kind, ATTR.k_qualification" & _
                      "     FROM (SELECT MST.*, MT.max_d_from FROM staf_attribute AS MST, (SELECT M.c_user_id AS max_id, MAX(M.d_from) AS max_d_from" & _
                      "     FROM staf_attribute AS M WHERE M.d_from<:strDate AND M.c_ksh=:c_ksh GROUP BY M.c_user_id) AS MT WHERE MT.max_id=MST.c_user_id AND MT.max_d_from=MST.d_from) AS ATTR" & _
                      "     WHERE ATTR.k_del <> '1' AND ATTR.k_user_status='01' AND ATTR.k_staf_kind=:k_staf_kind " & _
                      "     GROUP BY IIF(RIGHT(:AgeBase, 4) < FORMAT(ATTR.d_birth, 'MMdd'), CInt(LEFT(:AgeBase, 4))-CInt(FORMAT(ATTR.d_birth, 'yyyy'))-1, CInt(LEFT(:AgeBase, 4))-CInt(FORMAT(ATTR.d_birth, 'yyyy'))), ATTR.k_staf_kind, ATTR.k_qualification) AS STAFF" & _
                      " INNER JOIN (SELECT MST.* FROM union_dues_dtl AS MST, (SELECT M.k_union_dues, M.k_qualification, M.k_staf_kind, M.s_age_str, MAX(M.d_from) AS max_d_from" & _
                      " FROM union_dues_dtl AS M WHERE M.d_from<:strDate And :strDate<=M.d_to GROUP BY M.k_union_dues, M.k_qualification, M.k_staf_kind, M.s_age_str) AS MT" & _
                      " WHERE MT.k_union_dues=MST.k_union_dues AND MT.k_qualification=MST.k_qualification AND MT.k_staf_kind=MST.k_staf_kind AND MT.s_age_str=MST.s_age_str AND MT.max_d_from=MST.d_from) AS DUES" & _
                      " ON (STAFF.k_staf_kind=DUES.k_staf_kind) AND (STAFF.k_qualification=DUES.k_qualification) AND (STAFF.AGE >=DUES.s_age_str) AND (STAFF.AGE<=DUES.s_age_end);"
            command.SetSql(cmdText)
            command.Parameters.Clear()
            command.Parameters.Add(New NpgsqlParameter("AgeBase", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("k_staf_kind", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
            command.Parameters.Item("AgeBase").Value = strKeyAgeDate
            command.Parameters.Item("k_staf_kind").Value = strStafKind
            command.Parameters.Item("strDate").Value = strStr
            command.Parameters.Item("c_ksh").Value = strKsh
            Dim dReader As NpgsqlDataReader = command.ExecuteReader
            Return dReader.getTable
        End Function

        ' 新規レコード作成
        Function CreateNewData(ByVal command As NpgsqlCommand, ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strQualification As String, ByVal objTable As DataTable) As Integer
            ' レコードが存在する場合、更新
            Dim strSqlUpd As String = "UPDATE revenue_expenditure_member_dtl_view SET s_real_number_ttl=s_real_number_ttl+:s_real_number_ttl, s_real_union_dues_ttl=s_real_union_dues_ttl+:s_real_union_dues_ttl {0} ,d_up=GETDATE(),c_user_id_up=:c_user_id_up,s_up=s_up+1 " &
                                    "WHERE d_revenue_str=:d_revenue_str AND k_qualification=:k_qualification AND c_mac=:c_mac AND c_control=:c_control"
            Dim params As String = ""
            Dim values As String = ""
            Dim count As Long = 0
            Dim total As Long = 0

            Dim rows() As DataRow = objTable.Select("k_qualification='" & strQualification & "'")
            For Each Row As DataRow In rows
                params = params & ",s_number_" & Row.Item(0) & "=" & Row.Item(1) & ",s_union_dues_" & Row.Item(0) & "=" & IIf(IsDBNull(Row.Item(5)), 0, Row.Item(5))
                count += Row.Item(1)
                total += IIf(IsDBNull(Row.Item(5)), 0, Row.Item(5))
            Next

            Dim result As Integer = 0
            Dim cmdText As String = String.Format(strSqlUpd, params)
            command.SetSql(cmdText)
            command.Parameters.Clear()
            command.Parameters.Add(New NpgsqlParameter("d_revenue_str", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("k_qualification", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("c_mac", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("c_control", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("s_real_number_ttl", DbType.Int32))
            command.Parameters.Add(New NpgsqlParameter("s_real_union_dues_ttl", DbType.Int32))
            command.Parameters.Add(New NpgsqlParameter("c_user_id_up", DbType.String))
            command.Parameters.Item("d_revenue_str").Value = strStr
            command.Parameters.Item("k_qualification").Value = strQualification
            command.Parameters.Item("c_mac").Value = strMac
            command.Parameters.Item("c_control").Value = strControlName
            command.Parameters.Item("s_real_number_ttl").Value = count
            command.Parameters.Item("s_real_union_dues_ttl").Value = total
            command.Parameters.Item("c_user_id_up").Value = strUserId

            ' 新規追加
            If (command.ExecuteNonQuery = 0) Then
                Dim strSqlIns As String = "INSERT INTO revenue_expenditure_member_dtl_view (d_revenue_str,k_qualification,c_mac,c_control,s_real_number_ttl, s_real_union_dues_ttl {0},d_ins,c_user_id_ins) " &
                                        "VALUES(:d_revenue_str,:k_qualification,:c_mac,:c_control,:s_real_number_ttl, :s_real_union_dues_ttl {1},GETDATE(),:c_user_id_ins)"
                params = ""
                values = ""
                count = 0
                total = 0

                rows = objTable.Select("k_qualification='" & strQualification & "'")
                For Each Row As DataRow In rows
                    params = params & ",s_number_" & Row.Item(0) & ",s_union_dues_" & Row.Item(0)
                    values = values & "," & Row.Item(1) & "," & IIf(IsDBNull(Row.Item(5)), 0, Row.Item(5))
                    count += Row.Item(1)
                    total += IIf(IsDBNull(Row.Item(5)), 0, Row.Item(5))
                Next

                cmdText = String.Format(strSqlIns, params, values)
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add(New NpgsqlParameter("d_revenue_str", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_qualification", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_mac", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_control", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_real_number_ttl", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("s_real_union_dues_ttl", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("c_user_id_ins", DbType.String))
                command.Parameters.Item("d_revenue_str").Value = strStr
                command.Parameters.Item("k_qualification").Value = strQualification
                command.Parameters.Item("c_mac").Value = strMac
                command.Parameters.Item("c_control").Value = strControlName
                command.Parameters.Item("s_real_number_ttl").Value = count
                command.Parameters.Item("s_real_union_dues_ttl").Value = total
                command.Parameters.Item("c_user_id_ins").Value = strUserId
                count = command.ExecuteNonQuery
            End If
            Return count
        End Function

        Public Function GetRevenueExpenditureMemberDtl(ByVal strRevenueStr As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim cmdText As String = " SELECT MEM.d_revenue_str, MEM.k_qualification, MEM.s_real_number_ttl, MEM.s_real_union_dues_ttl, MEM.s_number_22, MEM.s_union_dues_22, MEM.s_number_23, MEM.s_union_dues_23, MEM.s_number_24, MEM.s_union_dues_24, MEM.s_number_25, MEM.s_union_dues_25, MEM.s_number_26, MEM.s_union_dues_26, MEM.s_number_27, MEM.s_union_dues_27, MEM.s_number_28, MEM.s_union_dues_28, MEM.s_number_29, MEM.s_union_dues_29, MEM.s_number_30, MEM.s_union_dues_30, MEM.s_number_31, MEM.s_union_dues_31, MEM.s_number_32, MEM.s_union_dues_32, MEM.s_number_33, MEM.s_union_dues_33, MEM.s_number_34, MEM.s_union_dues_34, MEM.s_number_35, MEM.s_union_dues_35, MEM.s_number_36, MEM.s_union_dues_36, MEM.s_number_37, MEM.s_union_dues_37, MEM.s_number_38, MEM.s_union_dues_38, MEM.s_number_39, MEM.s_union_dues_39, MEM.s_number_40, MEM.s_union_dues_40, MEM.s_number_41, MEM.s_union_dues_41, MEM.s_number_42, MEM.s_union_dues_42, MEM.s_number_43, MEM.s_union_dues_43, MEM.s_number_44, MEM.s_union_dues_44, MEM.s_number_45, MEM.s_union_dues_45, MEM.s_number_46, MEM.s_union_dues_46, MEM.s_number_47, MEM.s_union_dues_47, MEM.s_number_48, MEM.s_union_dues_48, MEM.s_number_49, MEM.s_union_dues_49, MEM.s_number_50, MEM.s_union_dues_50, MEM.s_number_51, MEM.s_union_dues_51, MEM.s_number_52, MEM.s_union_dues_52, MEM.s_number_53, MEM.s_union_dues_53, MEM.s_number_54, MEM.s_union_dues_54, MEM.s_number_55, MEM.s_union_dues_55, MEM.s_number_56, MEM.s_union_dues_56, MEM.s_number_57, MEM.s_union_dues_57, MEM.s_number_58, MEM.s_union_dues_58, MEM.s_number_59, MEM.s_union_dues_59, MEM.s_number_60, MEM.s_union_dues_60, MEM.s_number_61, MEM.s_union_dues_61, MEM.s_number_62, MEM.s_union_dues_62, MEM.s_number_63, MEM.s_union_dues_63, MEM.s_number_64, MEM.s_union_dues_64, MEM.s_number_65, MEM.s_union_dues_65, MEM.s_number_66, MEM.s_union_dues_66, MEM.s_number_67, MEM.s_union_dues_67, MEM.s_number_68, MEM.s_union_dues_68, MEM.s_number_69, MEM.s_union_dues_69, MEM.s_number_70, MEM.s_union_dues_70, MEM.d_ins, MEM.c_user_id_ins FROM revenue_expenditure_member_dtl MEM WHERE MEM.d_revenue_str = :d_revenue_str ORDER BY MEM.k_qualification "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_revenue_str", DbType.String))
                command.Parameters.Item("d_revenue_str").Value = strRevenueStr
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.CreateSomeDataSet("revenue_expenditure_member_dtl", dReader)
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

        Public Function GetRevenueExpenditureMemberDtlView(ByVal strMac As String, ByVal strControlName As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim cmdText As String = " SELECT MEM.d_revenue_str, MEM.k_qualification, MEM.s_real_number_ttl, MEM.s_real_union_dues_ttl, MEM.s_number_22, MEM.s_union_dues_22, MEM.s_number_23, MEM.s_union_dues_23, MEM.s_number_24, MEM.s_union_dues_24, MEM.s_number_25, MEM.s_union_dues_25, MEM.s_number_26, MEM.s_union_dues_26, MEM.s_number_27, MEM.s_union_dues_27, MEM.s_number_28, MEM.s_union_dues_28, MEM.s_number_29, MEM.s_union_dues_29, MEM.s_number_30, MEM.s_union_dues_30, MEM.s_number_31, MEM.s_union_dues_31, MEM.s_number_32, MEM.s_union_dues_32, MEM.s_number_33, MEM.s_union_dues_33, MEM.s_number_34, MEM.s_union_dues_34, MEM.s_number_35, MEM.s_union_dues_35, MEM.s_number_36, MEM.s_union_dues_36, MEM.s_number_37, MEM.s_union_dues_37, MEM.s_number_38, MEM.s_union_dues_38, MEM.s_number_39, MEM.s_union_dues_39, MEM.s_number_40, MEM.s_union_dues_40, MEM.s_number_41, MEM.s_union_dues_41, MEM.s_number_42, MEM.s_union_dues_42, MEM.s_number_43, MEM.s_union_dues_43, MEM.s_number_44, MEM.s_union_dues_44, MEM.s_number_45, MEM.s_union_dues_45, MEM.s_number_46, MEM.s_union_dues_46, MEM.s_number_47, MEM.s_union_dues_47, MEM.s_number_48, MEM.s_union_dues_48, MEM.s_number_49, MEM.s_union_dues_49, MEM.s_number_50, MEM.s_union_dues_50, MEM.s_number_51, MEM.s_union_dues_51, MEM.s_number_52, MEM.s_union_dues_52, MEM.s_number_53, MEM.s_union_dues_53, MEM.s_number_54, MEM.s_union_dues_54, MEM.s_number_55, MEM.s_union_dues_55, MEM.s_number_56, MEM.s_union_dues_56, MEM.s_number_57, MEM.s_union_dues_57, MEM.s_number_58, MEM.s_union_dues_58, MEM.s_number_59, MEM.s_union_dues_59, MEM.s_number_60, MEM.s_union_dues_60, MEM.s_number_61, MEM.s_union_dues_61, MEM.s_number_62, MEM.s_union_dues_62, MEM.s_number_63, MEM.s_union_dues_63, MEM.s_number_64, MEM.s_union_dues_64, MEM.s_number_65, MEM.s_union_dues_65, MEM.s_number_66, MEM.s_union_dues_66, MEM.s_number_67, MEM.s_union_dues_67, MEM.s_number_68, MEM.s_union_dues_68, MEM.s_number_69, MEM.s_union_dues_69, MEM.s_number_70, MEM.s_union_dues_70, MEM.d_ins, MEM.c_user_id_ins, MEM.d_up, MEM.c_user_id_up, MEM.s_up FROM revenue_expenditure_member_dtl_view MEM WHERE c_mac     = :c_mac   AND c_control = :c_control ORDER BY MEM.k_qualification "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_mac", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_control", DbType.String))
                command.Parameters.Item("c_mac").Value = strMac
                command.Parameters.Item("c_control").Value = strControlName
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.CreateSomeDataSet("revenue_expenditure_member_dtl", dReader)
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

        Public Function GetStafKindNumber(ByVal strKsh As String, ByVal strDFrom As String, ByVal strUserStatus As String, ByVal strStafKind As String) As Integer
            Dim num As Integer
            Try
                Dim cmdText As String = "SELECT    COUNT(STF.c_user_id)  FROM  staf_attribute STF,    (SELECT      c_user_id,      MAX(d_from) AS max_d_from     FROM      staf_attribute     WHERE      d_from <= :d_from AND      c_ksh   = :c_ksh     group by c_user_id) MAX_INFO  WHERE    STF.c_user_id" & "	" & "    = MAX_INFO.c_user_id AND    STF.d_from" & "		" & "= MAX_INFO.max_d_from AND    STF.k_user_status = :k_user_status AND    STF.k_staf_kind   = :k_staf_kind "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_user_status", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_staf_kind", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("d_from").Value = strDFrom
                command.Parameters.Item("k_user_status").Value = strUserStatus
                command.Parameters.Item("k_staf_kind").Value = strStafKind
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

        Public Function GetUnionMemberWork(ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strKeyAgeDate As String, ByVal strKsh As String, ByVal strStafKind As String) As Integer
            Dim num As Integer
            Try
                Dim cmdText As String = "UnionMemberWorkCountHeadsCreate( :strMac, :strControlName, :strUserId, :strStr, :strKeyAgeDate, :strKsh, :strStafKind )"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection) With { _
                    .CommandType = CommandType.StoredProcedure _
                }
                command.Parameters.Add("strMac", strMac)
                command.Parameters.Add("strControlName", strControlName)
                command.Parameters.Add("strUserId", strUserId)
                command.Parameters.Add("strStr", strStr)
                command.Parameters.Add("strKeyAgeDate", strKeyAgeDate)
                command.Parameters.Add("strKsh", strKsh)
                command.Parameters.Add("strStafKind", strStafKind)
                Dim obj2 As Object = command.ExecuteScalar
                num = If(((obj2 Is Nothing) OrElse TypeOf obj2 Is DBNull), 0, CInt(obj2))
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
