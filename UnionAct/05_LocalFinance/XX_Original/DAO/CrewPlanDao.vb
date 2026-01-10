Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.Command
Imports UnionAct.DAO.Master

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.NSMDConst

Namespace DAO.RevenueExpenditure
    Friend Class CrewPlanDao
        Inherits AbstractDao
        'Implements ICrewPlanDao
        ' Methods
        'Public Function CreateRevenueExpenditureRetireDtlWork(ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strEnd As String, ByVal strKsh As String) As Integer
        '    Dim num As Integer
        '    Try
        '        Dim cmdText As String = "RevenueExpenditureRetireDtlWorkCreate( :strMac, :strControlName, :strUserId, :strStr, :strEnd, :strKsh)"
        '        Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection) With { _
        '            .CommandType = CommandType.StoredProcedure _
        '        }
        '        command.Parameters.Add("strMac", strMac)
        '        command.Parameters.Add("strControlName", strControlName)
        '        command.Parameters.Add("strUserId", strUserId)
        '        command.Parameters.Add("strStr", strStr)
        '        command.Parameters.Add("strEnd", strEnd)
        '        command.Parameters.Add("strKsh", strKsh)
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

        ' 資格別退職者数集計
        Public Function CreateRevenueExpenditureRetireDtlWork(ByVal command As NpgsqlCommand, ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strEnd As String, ByVal strKsh As String) As Integer
            Dim num As Integer = 0

            ' 退職年齢取得
            Dim age As Integer = GetRetireAge(command, strStr, MDConst.STAF_KIND_REGULAR)
            If age > 0 Then
                RETIRE_AGE = age + 1
            End If
            age = GetRetireAge(command, strStr, MDConst.STAF_KIND_SENIOR)
            If age > 0 Then
                SENIOR_RETIRE_AGE = age + 1
            End If

            ' 既存レコード削除
            DeleteCurrentRec(command, strMac, strControlName)

            ' 月別退職者集計処理(正組合員)
            Dim startMonth As String = ""
            Dim endMonth As String = ""
            GetTargetRange(MDConst.STAF_KIND_REGULAR, strStr, strEnd, startMonth, endMonth)
            Dim table1 As DataTable = SummaryRetireMember(command, strStr, startMonth, endMonth, strKsh)

            ' 月別退職者集計処理(シニア組合員)
            GetTargetRange(MDConst.STAF_KIND_SENIOR, strStr, strEnd, startMonth, endMonth)
            Dim table2 As DataTable = SummaryRetireSeniorMember(command, strStr, startMonth, endMonth, strKsh)

            ' 搭乗資格別データ作成
            'If table1.Rows.Count > 0 Or table2.Rows.Count > 0 Then
            num = CreateNewData(command, strMac, strControlName, strUserId, table1, table2, strStr)
            'End If

            Return num
        End Function

        ' 既存レコード削除
        Private Sub DeleteCurrentRec(ByVal command As NpgsqlCommand, ByVal strMac As String, ByVal strControlName As String)
            command.SetSql("DELETE FROM revenue_expenditure_retire_dtl_work WHERE c_mac=:c_mac AND c_control=:c_control")
            command.Parameters.Add(New NpgsqlParameter("c_mac", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("c_control", DbType.String))
            command.Parameters.Item("c_mac").Value = strMac
            command.Parameters.Item("c_control").Value = strControlName
            command.ExecuteNonQuery()
        End Sub

        ' 集計対象範囲取得
        Private Sub GetTargetRange(ByVal strStafKind As String, ByVal strStr As String, ByVal strEnd As String, ByRef startMonth As String, ByRef endMonth As String)
            Dim retireAge As Integer

            If strStafKind = MDConst.STAF_KIND_REGULAR Then
                retireAge = RETIRE_AGE
            Else
                retireAge = SENIOR_RETIRE_AGE
            End If

            startMonth = Format(CInt(Left(strStr, 4)) - retireAge, "0000") & Mid(strStr, 5, 2)
            endMonth = Format(CInt(Left(strEnd, 4)) - retireAge, "0000") & Mid(strEnd, 5, 2)
        End Sub

        ' 対象年月リスト作成
        Private Function GenTargetMonthList(ByVal strStart As String, ByVal iMonths As Integer) As String()
            Dim list(iMonths - 1) As String
            Dim year, month As Integer
            year = CInt(Left(strStart, 4))
            month = CInt(Mid(strStart, 5, 2))

            For i As Integer = 0 To iMonths - 1
                list(i) = Format(year, "0000") & Format(month, "00")
                month += 1
                If month = 13 Then
                    year += 1
                    month = 1
                End If
            Next
            Return list
        End Function

        ' 月別退職者集計処理
        Public Function SummaryRetireMember(ByVal command As NpgsqlCommand, ByVal strStr As String, ByVal startMonth As String, ByVal endMonth As String, ByVal strKsh As String) As DataTable
            Dim cmdText As String
            cmdText = "SELECT Format(ATTR.d_birth, 'yyyyMM') AS BIRTH_MONTH, COUNT(*) AS NUM, ATTR.k_staf_kind, ATTR.k_qualification" &
                      " FROM (SELECT MST.*, MT.max_d_from FROM staf_attribute AS MST, (SELECT M.c_user_id AS max_id, MAX(M.d_from) AS max_d_from" &
                      "     FROM staf_attribute AS M WHERE M.d_from<:strStr AND M.c_ksh=:c_ksh GROUP BY M.c_user_id) AS MT WHERE MT.max_id=MST.c_user_id AND MT.max_d_from=MST.d_from) AS ATTR" &
                      "     WHERE ATTR.k_del <> '1' AND ATTR.k_user_status=:k_user_status AND ATTR.k_staf_kind=:k_staf_kind AND Format(ATTR.d_birth, 'yyyyMM')>=:startMonth AND Format(ATTR.d_birth, 'yyyyMM')<=:endMonth" &
                      "     GROUP BY Format(ATTR.d_birth, 'yyyyMM'), ATTR.k_staf_kind, ATTR.k_qualification;"
            command.SetSql(cmdText)
            command.Parameters.Clear()
            command.Parameters.Add(New NpgsqlParameter("strStr", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("startMonth", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("endMonth", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("k_user_status", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("k_staf_kind", DbType.String))
            command.Parameters.Item("strStr").Value = strStr
            command.Parameters.Item("startMonth").Value = startMonth
            command.Parameters.Item("endMonth").Value = endMonth
            command.Parameters.Item("c_ksh").Value = strKsh
            command.Parameters.Item("k_user_status").Value = MDConst.USER_STATUS_ENTRY
            command.Parameters.Item("k_staf_kind").Value = MDConst.STAF_KIND_REGULAR
            Dim dReader As NpgsqlDataReader = command.ExecuteReader
            Return dReader.getTable
        End Function

        ' 月別退職者集計処理
        Public Function SummaryRetireSeniorMember(ByVal command As NpgsqlCommand, ByVal strStr As String, ByVal startMonth As String, ByVal endMonth As String, ByVal strKsh As String) As DataTable
            Dim cmdText As String
            cmdText = "SELECT Format(ATTR.d_birth, 'yyyyMM') AS BIRTH_MONTH, COUNT(*) AS NUM, ATTR.k_staf_kind" &
                      " FROM (SELECT MST.*, MT.max_d_from FROM staf_attribute AS MST, (SELECT M.c_user_id AS max_id, MAX(M.d_from) AS max_d_from" &
                      "     FROM staf_attribute AS M WHERE M.d_from<:strStr AND M.c_ksh=:c_ksh GROUP BY M.c_user_id) AS MT WHERE MT.max_id=MST.c_user_id AND MT.max_d_from=MST.d_from) AS ATTR" &
                      "     WHERE ATTR.k_del <> '1' AND ATTR.k_user_status=:k_user_status AND ATTR.k_staf_kind=:k_staf_kind AND Format(ATTR.d_birth, 'yyyyMM')>=:startMonth AND Format(ATTR.d_birth, 'yyyyMM')<=:endMonth" &
                      "     GROUP BY Format(ATTR.d_birth, 'yyyyMM'), ATTR.k_staf_kind;"
            command.SetSql(cmdText)
            command.Parameters.Clear()
            command.Parameters.Add(New NpgsqlParameter("strStr", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("startMonth", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("endMonth", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("k_user_status", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("k_staf_kind", DbType.String))
            command.Parameters.Item("strStr").Value = strStr
            command.Parameters.Item("startMonth").Value = startMonth
            command.Parameters.Item("endMonth").Value = endMonth
            command.Parameters.Item("c_ksh").Value = strKsh
            command.Parameters.Item("k_user_status").Value = MDConst.USER_STATUS_ENTRY
            command.Parameters.Item("k_staf_kind").Value = MDConst.STAF_KIND_SENIOR
            Dim dReader As NpgsqlDataReader = command.ExecuteReader
            Return dReader.getTable
        End Function

        ' 新規レコード作成
        Function CreateNewData(ByVal command As NpgsqlCommand, ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal objTable1 As DataTable, ByVal objTable2 As DataTable, ByVal strStr As String) As Integer
            Dim dataMap As New Hashtable
            Dim recData As ArrayList
            Dim key As String
            Dim list() As String = GenTargetMonthList(strStr, 12)

            ' 全対象月データ作成
            For i As Integer = 0 To 11
                recData = New ArrayList
                recData.Add(0)
                recData.Add(0)
                recData.Add(0)
                recData.Add(0)
                dataMap.Add(list(i), recData)
            Next

            ' 正組合員集計
            For Each Row As DataRow In objTable1.Rows
                key = Format(CInt(Left(Row.Item(0), 4)) + RETIRE_AGE, "0000") & Mid(Row.Item(0), 5, 2)
                recData = dataMap.Item(key)
                'If recData Is Nothing Then
                '    recData = New ArrayList
                '    recData.Add(0)
                '    recData.Add(0)
                '    recData.Add(0)
                '    recData.Add(0)
                '    dataMap.Add(key, recData)
                'End If
                Select Case (Row.Item(3))
                    Case MDConst.QUALIFICATION_PILOT
                        recData.Item(0) = Row.Item(1)
                    Case MDConst.QUALIFICATION_COPILOT
                        recData.Item(1) = Row.Item(1)
                    Case MDConst.QUALIFICATION_FLIGHT_ENGINEER
                        recData.Item(2) = Row.Item(1)
                End Select
            Next

            ' シニア組合員集計
            For Each Row As DataRow In objTable2.Rows
                key = Format(CInt(Left(Row.Item(0), 4)) + SENIOR_RETIRE_AGE, "0000") & Mid(Row.Item(0), 5, 2)
                recData = dataMap.Item(key)
                'If recData Is Nothing Then
                '    recData = New ArrayList
                '    recData.Add(0)
                '    recData.Add(0)
                '    recData.Add(0)
                '    recData.Add(0)
                '    dataMap.Add(key, recData)
                'End If
                recData.Item(3) = recData.Item(3) + Row.Item(1)
            Next

            ' 新規追加
            Dim cmdText As String = "INSERT INTO revenue_expenditure_retire_dtl_work (c_mac,c_control,d_target,s_cap_retire_member,s_cop_retire_member,s_fe_retire_member,s_senior_retire_member,d_ins,c_user_id_ins) " &
                                        "VALUES(:c_mac,:c_control,:d_target,:s_cap_retire_member,:s_cop_retire_member,:s_fe_retire_member,:s_senior_retire_member,GETDATE(),:c_user_id_ins)"
            Dim count As Integer = 0

            For Each rec As Object In dataMap
                recData = rec.Value
                command.SetSql(cmdText)
                command.Parameters.Clear()
                command.Parameters.Add(New NpgsqlParameter("s_cap_retire_member", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("s_cop_retire_member", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("s_fe_retire_member", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("s_senior_retire_member", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("c_user_id_ins", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_mac", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_control", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_target", DbType.String))
                command.Parameters.Item("s_cap_retire_member").Value = rec.Value.Item(0)
                command.Parameters.Item("s_cop_retire_member").Value = rec.Value.Item(1)
                command.Parameters.Item("s_fe_retire_member").Value = rec.Value.Item(2)
                command.Parameters.Item("s_senior_retire_member").Value = rec.Value.Item(3)
                command.Parameters.Item("c_user_id_ins").Value = strUserId
                command.Parameters.Item("c_mac").Value = strMac
                command.Parameters.Item("c_control").Value = strControlName
                command.Parameters.Item("d_target").Value = rec.Key
                count += command.ExecuteNonQuery
            Next
            Return count
        End Function

        Public Function GetRevenueExpenditurePromotionDtl(ByVal strRevenueStr As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim cmdText As String = "SELECT SUBSTRING(d_target, 1, 4) & '/' & SUBSTRING(d_target, 5, 2) AS d_target, s_new_staff_member, s_new_cap_member, s_cap_retire_member, s_cop_retire_member, s_fe_retire_member, s_senior_retire_member FROM revenue_expenditure_promotion_dtl WHERE d_revenue_str = :d_revenue_str "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Item("d_revenue_str").Value = strRevenueStr
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.CreateSomeDataSet("revenue_expenditure_retire_dtl_work", dReader)
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

        Public Function GetRevenueExpenditureRetireDtlWork(ByVal strMac As String, ByVal strControlName As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim cmdText As String = "SELECT c_mac, c_control, SUBSTRING(d_target, 1, 4) & '/' & SUBSTRING(d_target, 5, 2) AS d_target, s_cap_retire_member, s_cop_retire_member, s_fe_retire_member,s_senior_retire_member, s_reserve_1, s_reserve_2, s_reserve_3, d_ins, c_user_id_ins FROM revenue_expenditure_retire_dtl_work WHERE c_mac = :strMac and c_control = :strControlName ORDER BY d_target"
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("strMac", DbType.String)
                command.Parameters.Add("strControlName", DbType.String)
                command.Parameters.Item("strMac").Value = strMac
                command.Parameters.Item("strControlName").Value = strControlName
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table2 = MyBase.CreateSomeDataSet("revenue_expenditure_retire_dtl_work", dReader)
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

        Public Sub InsertRevenueExpenditurePromotionDtl(ByVal command As NpgsqlCommand, ByVal dTable As DataTable, ByVal strUserID As String)
            Try
                Dim map As New RevenueExpenditurePromotionDtlMap
                Dim str2 As String = map.ToPhysicalString("")
                Dim str3 As String = map.ToPhysicalString(":")
                Dim cmdText As String = String.Concat(New String() {"insert into revenue_expenditure_promotion_dtl( ", str2, " ) values( ", str3, " ) "})
                Dim i As Integer
                For i = 0 To dTable.Rows.Count - 1
                    'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection, MyBase.GetNpgsqlTran)
                    command.Parameters.Clear()
                    command.SetSql(cmdText)
                    Dim j As Integer
                    For j = 0 To map.ColumnCount - 1
                        command.Parameters.Add(New NpgsqlParameter(map.GetPhysicalName(j), map.GetDbDataType(j)))
                        command.Parameters.Item(map.GetPhysicalName(j)).Value = dTable.Rows.Item(i).Item(map.GetPhysicalName(j))
                    Next j
                    command.Parameters.Item(map.GetPhysicalName("作成日")).intColmunType = DbType.Object
                    command.Parameters.Item(map.GetPhysicalName("作成日")).Value = "CONVERT(DATE,'" & PublicCommand.GetNow & "')"
                    command.Parameters.Item(map.GetPhysicalName("作成者個人" & "ID")).Value = strUserID
                    command.Parameters.Item(map.GetPhysicalName("更新日")).intColmunType = DbType.Object
                    command.Parameters.Item(map.GetPhysicalName("更新日")).Value = "CONVERT(DATE,'" & PublicCommand.GetNow & "')"
                    command.Parameters.Item(map.GetPhysicalName("更新回数")).Value = 0
                    command.Parameters.Item("s_reserve_1").Value = 0
                    command.Parameters.Item("s_reserve_2").Value = 0
                    command.Parameters.Item("s_reserve_3").Value = 0
                    command.Parameters.Item("l_biko").Value = ""
                    command.ExecuteNonQuery()
                Next i
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

        Public Sub UpDateRevenueExpenditure(ByVal command As NpgsqlCommand, ByVal dtTable As DataTable, ByVal dtTableCalcData As DataTable, ByVal strKey As String)
            Try
                Dim cmdText As String = "UPDATE revenue_expenditure SET k_revenue_member = :k_revenue_member, s_revise_revenue_ttl = :s_revise_revenue_ttl, s_new_staff_average = :s_new_staff_average, s_cap_promotion_average = :s_cap_promotion_average, s_unpromotion_persons = :s_unpromotion_persons, s_unpromotion_rate = :s_unpromotion_rate, s_unpromotion_average = :s_unpromotion_average, s_senior_stay_rate = :s_senior_stay_rate, s_senior_average = :s_senior_average, s_new_union_monthwork = :s_new_union_monthwork, s_cap_up_monthwork   = :s_cap_up_monthwork, s_cap_retire_monthwork  = :s_cap_retire_monthwork, s_cop_retire_monthwork = :s_cop_retire_monthwork, s_fe_retire_monthwork = :s_fe_retire_monthwork, s_unpromotion_persons_monthwork = :s_unpromotion_person_monthwork, s_senior_retire_monthwork = :s_senior_retire_monthwork, d_up = CONVERT(DATE,:d_up), c_user_id_up = :c_user_id_up, s_up = s_up + 1 WHERE d_revenue_str = :d_revenue_str "
                'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Clear()
                command.SetSql(cmdText)
                command.Parameters.Add("k_revenue_member", DbType.String)
                command.Parameters.Add("s_revise_revenue_ttl", DbType.Int32)
                command.Parameters.Add("s_new_staff_average", DbType.Int32)
                command.Parameters.Add("s_cap_promotion_average", DbType.Int32)
                command.Parameters.Add("s_unpromotion_persons", DbType.Int32)
                command.Parameters.Add("s_unpromotion_rate", DbType.Int32)
                command.Parameters.Add("s_unpromotion_average", DbType.Int32)
                command.Parameters.Add("s_senior_stay_rate", DbType.Int32)
                command.Parameters.Add("s_senior_average", DbType.Int32)
                command.Parameters.Add("s_new_union_monthwork", DbType.Int32)
                command.Parameters.Add("s_cap_up_monthwork", DbType.Int32)
                command.Parameters.Add("s_cap_retire_monthwork", DbType.Int32)
                command.Parameters.Add("s_fe_retire_monthwork", DbType.Int32)
                command.Parameters.Add("s_cop_retire_monthwork", DbType.Int32)
                command.Parameters.Add("s_unpromotion_person_monthwork", DbType.Int32)
                command.Parameters.Add("s_senior_retire_monthwork", DbType.Int32)
                command.Parameters.Add("d_up", DbType.String)
                command.Parameters.Add("c_user_id_up", DbType.String)
                command.Parameters.Add("d_revenue_str", DbType.String)
                command.Parameters.Item("k_revenue_member").Value = "1"
                command.Parameters.Item("s_revise_revenue_ttl").Value = dtTableCalcData.Rows.Item(0).Item("s_revise_revenue_ttl")
                command.Parameters.Item("s_new_staff_average").Value = dtTable.Rows.Item(0).Item("s_new_staff_average")
                command.Parameters.Item("s_cap_promotion_average").Value = dtTable.Rows.Item(0).Item("s_cap_promotion_average")
                command.Parameters.Item("s_unpromotion_persons").Value = dtTableCalcData.Rows.Item(0).Item("s_unpromotion_persons")
                command.Parameters.Item("s_unpromotion_rate").Value = dtTable.Rows.Item(0).Item("s_unpromotion_rate")
                command.Parameters.Item("s_unpromotion_average").Value = dtTable.Rows.Item(0).Item("s_unpromotion_average")
                command.Parameters.Item("s_senior_stay_rate").Value = dtTable.Rows.Item(0).Item("s_senior_stay_rate")
                command.Parameters.Item("s_senior_average").Value = dtTable.Rows.Item(0).Item("s_senior_average")
                command.Parameters.Item("s_new_union_monthwork").Value = dtTableCalcData.Rows.Item(0).Item("s_new_union_monthwork")
                command.Parameters.Item("s_cap_up_monthwork").Value = dtTableCalcData.Rows.Item(0).Item("s_cap_up_monthwork")
                command.Parameters.Item("s_cap_retire_monthwork").Value = dtTableCalcData.Rows.Item(0).Item("s_cap_retire_monthwork")
                command.Parameters.Item("s_fe_retire_monthwork").Value = dtTableCalcData.Rows.Item(0).Item("s_fe_retire_monthwork")
                command.Parameters.Item("s_cop_retire_monthwork").Value = dtTableCalcData.Rows.Item(0).Item("s_cop_retire_monthwork")
                command.Parameters.Item("s_unpromotion_person_monthwork").Value = dtTableCalcData.Rows.Item(0).Item("s_unpromotion_persons_monthwork")
                command.Parameters.Item("s_senior_retire_monthwork").Value = dtTableCalcData.Rows.Item(0).Item("s_senior_retire_monthwork")
                command.Parameters.Item("d_up").Value = PublicCommand.GetNow
                command.Parameters.Item("c_user_id_up").Value = strKey
                command.Parameters.Item("d_revenue_str").Value = dtTable.Rows.Item(0).Item("d_revenue_str")
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
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0004", New String(0 - 1) {})
            Catch exception4 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub UpdateRevenueExpenditurePromotionDtl(ByVal command As NpgsqlCommand, ByVal dtKeyData As DataTable, ByVal dtUpData As DataTable, ByVal strUserID As String)
            Try
                Dim cmdText As String = "UPDATE revenue_expenditure_promotion_dtl SET s_new_staff_member = :s_new_staff_member, s_new_cap_member = :s_new_cap_member, s_cap_retire_member = :s_cap_retire_member, s_cop_retire_member = :s_cop_retire_member, s_fe_retire_member = :s_fe_retire_member, s_senior_retire_member = :s_senior_retire_member, d_up = :d_up, c_user_id_up = :c_user_id_up, s_up = s_up+1 WHERE d_revenue_str = :d_revenue_str and d_target = :d_target "
                Dim i As Integer
                For i = 0 To dtUpData.Rows.Count - 1
                    'Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                    command.Parameters.Clear()
                    command.SetSql(cmdText)
                    command.Parameters.Add("s_new_staff_member", DbType.Int32)
                    command.Parameters.Add("s_new_cap_member", DbType.Int32)
                    command.Parameters.Add("s_cap_retire_member", DbType.Int32)
                    command.Parameters.Add("s_cop_retire_member", DbType.Int32)
                    command.Parameters.Add("s_fe_retire_member", DbType.Int32)
                    command.Parameters.Add("s_senior_retire_member", DbType.Int32)
                    command.Parameters.Add("d_up", DbType.String)
                    command.Parameters.Add("c_user_id_up", DbType.String)
                    command.Parameters.Add("d_revenue_str", DbType.String)
                    command.Parameters.Add("d_target", DbType.String)
                    command.Parameters.Item("s_new_staff_member").Value = dtUpData.Rows.Item(i).Item("新入組合員数")
                    command.Parameters.Item("s_new_cap_member").Value = dtUpData.Rows.Item(i).Item("CAP" & "昇格数")
                    command.Parameters.Item("s_cap_retire_member").Value = dtUpData.Rows.Item(i).Item("CAP" & "退職者数")
                    command.Parameters.Item("s_cop_retire_member").Value = dtUpData.Rows.Item(i).Item("COP" & "退職者数")
                    command.Parameters.Item("s_fe_retire_member").Value = dtUpData.Rows.Item(i).Item("FE" & "退職者数")
                    command.Parameters.Item("s_senior_retire_member").Value = dtUpData.Rows.Item(i).Item("シニア退職者数")
                    command.Parameters.Item("d_up").Value = PublicCommand.GetNow
                    command.Parameters.Item("c_user_id_up").Value = strUserID
                    command.Parameters.Item("d_revenue_str").Value = dtKeyData.Rows.Item(0).Item("d_revenue_str")
                    command.Parameters.Item("d_target").Value = dtUpData.Rows.Item(i).Item("対象月").ToString.Replace("/", "").Insert((dtUpData.Rows.Item(i).Item("対象月").ToString.Length - 1), "01")
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

        Private Function GetRetireAge(ByVal command As NpgsqlCommand, ByVal strKeyDate As String, ByVal strConstantSeq As String) As Integer
            Try
                Dim map As New Constant_DtlMap
                command.SetSql("SELECT " & map.ToPhysicalString("") & " FROM constant_dtl WHERE " & "	" & "c_constant = :c_constant " & "	" & "AND d_from <= :d_date " & "	" & "AND d_to >= :d_date ORDER BY s_order ")
                command.Parameters.Add(New NpgsqlParameter("c_constant", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_constant").Value = "RETIRE_AGE"
                command.Parameters.Item("d_date").Value = strKeyDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return 0
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("constant_dtl", dReader))
                'set2 = ds
                'Dim table As DataTable = set2.Tables.Item(0).Copy
                'Dim count As Integer = table.Rows.Count
                'Dim rowArray As DataRow() = table.Select(("c_constant_seq = '" & strConstantSeq & "'"))

                Dim rowArray As DataRow() = ds.Tables.Item(0).Select(("c_constant_seq = '" & strConstantSeq & "'"))
                If ((rowArray.Length = 0) OrElse rowArray(0).Item("l_omission_name").ToString.Equals("")) Then
                    Return 0
                End If
                Try
                    Return Convert.ToInt32(rowArray(0).Item("l_omission_name").ToString)
                Catch obj1 As Exception
                    Return 0
                End Try
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
        End Function

        Private RETIRE_AGE As Integer = 60
        Private SENIOR_RETIRE_AGE As Integer = 65
    End Class
End Namespace
