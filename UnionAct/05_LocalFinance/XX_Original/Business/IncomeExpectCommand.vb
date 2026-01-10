Imports UnionAct.DAO
Imports UnionAct.Business.Common

#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports UnionAct.Framework.UnionException
Imports UnionAct.DAO.RevenueExpenditure
Imports UnionAct.Framework.Command
Imports UnionAct.Framework.Mapping
Imports UnionAct.DAO.Master
Imports UnionAct.NSMDInfo

Namespace Business.RevenueExpenditure
    Public Class IncomeExpectCommand
        Inherits AbstractCommand
        'Implements IIncomeExpectCommand
        ' Methods
        Public Sub ClickChangeButton(ByVal dSetIn As DataSet, ByVal strDtDup As String)
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                Dim strRevenue As String = dSetIn.Tables.Item("dtDetail").Rows.Item(0).Item("d_revenue_str").ToString
                Dim timeStamp As DataSet = dao.GetTimeStamp(command, strRevenue)
                MyBase.CheckTimeStamp(timeStamp.Tables.Item("revenue_expenditure").Rows.Item(0).Item("d_up").ToString, strDtDup)
                dao.UpDateRevenueExpenditureDao(command, dSetIn.Tables.Item("dtHeader"))
                dao.DeleteRevenueExpenditureMemberDao(command, strRevenue)
                Dim logSessUserId As String = MDLoginInfo.UserId
                Dim i As Integer
                For i = 0 To dSetIn.Tables.Item("dtDetail").Rows.Count - 1
                    dSetIn.Tables.Item("dtDetail").Rows.Item(i).Item("d_ins") = PublicCommand.GetNow
                    dSetIn.Tables.Item("dtDetail").Rows.Item(i).Item("c_user_id_ins") = logSessUserId
                    dSetIn.Tables.Item("dtDetail").Rows.Item(i).Item("s_up") = 0
                Next i
                dao.InsertRevenueExpenditureMemberDtl(command, dSetIn.Tables.Item("dtDetail"))
                objTran.Commit()
            Catch exception As AppUnionException
                objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                objTran.Rollback()
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub ClickNewEntrButton(ByVal dSetIn As DataSet)
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                dao.InsertRevenueExpenditure(command, dSetIn.Tables.Item("dtHeader"))
                Dim logSessUserId As String = MDLoginInfo.UserId
                Dim i As Integer
                For i = 0 To dSetIn.Tables.Item("dtDetail").Rows.Count - 1
                    dSetIn.Tables.Item("dtDetail").Rows.Item(i).Item("d_ins") = PublicCommand.GetNow
                    dSetIn.Tables.Item("dtDetail").Rows.Item(i).Item("c_user_id_ins") = logSessUserId
                    dSetIn.Tables.Item("dtDetail").Rows.Item(i).Item("s_up") = 0
                Next i
                dao.InsertRevenueExpenditureMemberDtl(command, dSetIn.Tables.Item("dtDetail"))
                objTran.Commit()
            Catch exception As AppUnionException
                objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                objTran.Rollback()
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Function ConvetDBtoFlexGrid(ByVal dTableIn As DataTable) As DataTable
            Dim table3 As DataTable
            Try
                If (dTableIn Is Nothing) Then
                    Return Nothing
                End If
                Dim table As DataTable = Me.CreateDataTableForFlexGrid
                If (table.Rows.Count <= 0) Then
                    Return table
                End If
                Dim num As Integer = 0
                Dim num2 As Long = 0
                Dim i As Integer
                For i = 0 To table.Rows.Count - 1
                    Dim str6 As String = table.Rows.Item(i).Item("age").ToString
                    Dim str2 As String = ("s_number_" & Convert.ToInt32(str6))
                    Dim str3 As String = ("s_union_dues_" & Convert.ToInt32(str6))
                    num = 0
                    num2 = 0
                    Dim j As Integer
                    For j = 0 To dTableIn.Rows.Count - 1
                        Dim str As String = dTableIn.Rows.Item(j).Item("k_qualification").ToString
                        Dim str4 As String = (str & "_number")
                        Dim str5 As String = (str & "_union_dues")
                        Dim k As Integer
                        For k = 0 To table.Columns.Count - 1
                            If str4.Equals(table.Columns.Item(k).ToString) Then
                                table.Rows.Item(i).Item(str4) = dTableIn.Rows.Item(j).Item(str2)
                                num = (num + Convert.ToInt32(dTableIn.Rows.Item(j).Item(str2)))
                                table.Rows.Item(i).Item("total_number") = num
                                table.Rows.Item(i).Item(str5) = dTableIn.Rows.Item(j).Item(str3)
                                num2 = (num2 + Convert.ToInt64(dTableIn.Rows.Item(j).Item(str3)))
                                table.Rows.Item(i).Item("total_union_dues") = num2
                                Exit For
                            End If
                        Next k
                    Next j
                Next i
                table3 = Me.MakingDataForFlexGrid(table)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Private Function ConvetFlexGridToDB(ByVal strKeyDate As String, ByVal dTableIn As DataTable) As DataTable
            Dim table3 As DataTable
            Try
                Dim num As Integer
                Dim num2 As Integer
                If (dTableIn Is Nothing) Then
                    Return Nothing
                End If
                If (dTableIn.Rows.Count <= 0) Then
                    Return dTableIn
                End If
                Dim map As New IncomeExpectMap
                Dim table As DataTable = PublicCommand.ConvertLogicalToPhysical(dTableIn, map)
                Dim map2 As New RevenueExpenditureMemmberDtlMap
                Dim table2 As DataTable = map2.CreateDataTablePhysName("dtDetail")
                Dim logicalIndex As Integer = map2.GetLogicalIndex("22" & "Ë‘ÎÛl”")
                Dim num5 As Integer = map2.GetLogicalIndex("70" & "Ë‘ÎÛl”")
                'num()
                For num = 0 To table.Columns.Count - 1
                    Dim caption As String = table.Columns.Item(num).Caption
                    If ((caption.Length = 9) AndAlso caption.Substring(2, 7).Equals("_number")) Then
                        Dim row As DataRow = table2.NewRow
                        row.Item("d_revenue_str") = strKeyDate
                        row.Item("k_qualification") = caption.Substring(0, 2)
                        num2 = logicalIndex
                        Do While (num2 < num5)
                            row.Item(num2) = 0
                            row.Item((num2 + 1)) = 0
                            num2 += 1
                        Loop
                        table2.Rows.Add(row)
                    End If
                Next num
                'num()
                For num = 0 To table2.Rows.Count - 1
                    Dim str2 As String = table2.Rows.Item(num).Item("k_qualification").ToString
                    Dim str3 As String = (str2 & "_number")
                    Dim str4 As String = (str2 & "_union_dues")
                    'num2()
                    For num2 = 0 To table.Rows.Count - 1
                        Dim str7 As String = table.Rows.Item(num2).Item("age").ToString
                        Dim str5 As String = ("s_number_" & str7)
                        Dim str6 As String = ("s_union_dues_" & str7)
                        Dim i As Integer
                        For i = 0 To table2.Columns.Count - 1
                            If str5.Equals(table2.Columns.Item(i).ToString) Then
                                table2.Rows.Item(num).Item(str5) = table.Rows.Item(num2).Item(str3)
                                table2.Rows.Item(num).Item(str6) = table.Rows.Item(num2).Item(str4)
                            End If
                        Next i
                    Next num2
                Next num
                table3 = table2
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Public Function CreateDataBaseStyle(ByVal strKeyDate As String, ByVal dTableIn As DataTable, ByVal lRevenueExpendtureTtl As Long, ByVal iSeniorMonthwork As Integer, ByVal strRevenueStr As String, ByVal strRevenueEnd As String, ByVal strTitle As String) As DataSet
            Dim set2 As DataSet
            Try
                Dim ds As New DataSet("dsIncomEstimate")
                Dim table As DataTable = Me.ConvetFlexGridToDB(strKeyDate, dTableIn)
                Dim map As New RevenueExpenditureMemmberDtlMap
                Dim table2 As New DataTable("dtHeader")
                table2.Clear()
                table2.Columns.Add("d_year", GetType(String))
                table2.Columns.Add("d_month", GetType(String))
                table2.Columns.Add("d_day", GetType(String))
                table2.Columns.Add("s_real_union_dues_ttl", GetType(Long))
                table2.Columns.Add("d_revenue_str", GetType(String))
                table2.Columns.Add("d_revenue_end", GetType(String))
                table2.Columns.Add("l_title", GetType(String))
                table2.Columns.Add("s_revenue_expenditure_ttl", GetType(Long))
                table2.Columns.Add("s_senior_monthwork", GetType(Integer))
                table2.Columns.Add("c_user_id_ins", GetType(String))
                table2.Columns.Add("k_flag_22", GetType(String))
                table2.Columns.Add("k_flag_23", GetType(String))
                table2.Columns.Add("k_flag_24", GetType(String))
                Dim row As DataRow = table2.NewRow
                row.Item("d_year") = strKeyDate.Substring(0, 4)
                row.Item("d_month") = strKeyDate.Substring(4, 2)
                row.Item("d_day") = strKeyDate.Substring(6, 2)
                row.Item("s_real_union_dues_ttl") = lRevenueExpendtureTtl
                row.Item("d_revenue_str") = strRevenueStr
                row.Item("d_revenue_end") = strRevenueEnd
                row.Item("l_title") = strTitle
                row.Item("s_revenue_expenditure_ttl") = lRevenueExpendtureTtl
                row.Item("s_senior_monthwork") = iSeniorMonthwork
                row.Item("c_user_id_ins") = MDLoginInfo.UserId
                Dim physicalIndex As Integer = map.GetPhysicalIndex("k_qualification")
                Dim num2 As Integer = map.GetPhysicalIndex("s_number_22")
                Dim num3 As Integer = map.GetPhysicalIndex("s_number_23")
                Dim num4 As Integer = map.GetPhysicalIndex("s_number_24")
                row.Item("k_flag_22") = "0"
                row.Item("k_flag_23") = "0"
                row.Item("k_flag_24") = "0"
                Dim i As Integer
                For i = 0 To table.Rows.Count - 1
                    Dim str As String = table.Rows.Item(i).Item(physicalIndex).ToString
                    If ((str.Equals("01") OrElse str.Equals("02")) OrElse str.Equals("03")) Then
                        If (Convert.ToInt32(table.Rows.Item(i).Item(num2)) > 0) Then
                            row.Item("k_flag_22") = "1"
                            Exit For
                        End If
                        If (Convert.ToInt32(table.Rows.Item(i).Item(num3)) > 0) Then
                            row.Item("k_flag_23") = "1"
                            Exit For
                        End If
                        If (Convert.ToInt32(table.Rows.Item(i).Item(num4)) > 0) Then
                            row.Item("k_flag_24") = "1"
                            Exit For
                        End If
                    End If
                Next i
                table2.Rows.Add(row)
                ds.Tables.Add(table2)
                Dim j As Integer
                For j = 0 To table.Rows.Count - 1
                    Dim num6 As Integer
                    Dim num7 As Long
                    Dim strKQualification As String = table.Rows.Item(j).Item("k_qualification").ToString
                    Me.GetRealNumberDuesTtl(strKeyDate, table, strKQualification, num6, num7)
                    table.Rows.Item(j).Item("s_real_number_ttl") = num6
                    table.Rows.Item(j).Item("s_real_union_dues_ttl") = num7
                Next j
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return set2
        End Function

        Private Function CreateDataTableForFlexGrid() As DataTable
            Dim table2 As DataTable
            Try
                Dim num As Integer = &H16
                Dim num2 As Integer = 70
                Dim table As DataTable = New IncomeExpectMap().CreateDataTablePhysName("income_expect")
                Dim i As Integer = num2
                Do While (i >= num)
                    Dim row As DataRow = table.NewRow
                    row.Item("age") = i
                    table.Rows.Add(row)
                    i -= 1
                Loop
                table2 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Function GetDetailData(ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strKsh As String, ByRef iSeniorMonthwork As Integer) As DataTable
            Dim table3 As DataTable
            'Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim revenueExpenditureMemberDtl As DataTable = daoInc.GetRevenueExpenditureMemberDtl(strStr)
                Dim table2 As DataTable = Me.ConvetDBtoFlexGrid(revenueExpenditureMemberDtl)
                iSeniorMonthwork = Me.GetSeniorMonthWork(strMac, strControlName, strUserId, strStr, strKsh)
                table3 = table2
            Catch exception As AppUnionException
                'objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                'objTran.Rollback()
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                'objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Public Function GetNewEntryData(ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strKsh As String, ByRef iSeniorMonthwork As Integer) As DataTable
            Dim table3 As DataTable
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim strKeyAgeDate As String = DateTime.Parse(String.Concat(New String() {strStr.Substring(0, 4), "/", strStr.Substring(4, 2), "/", strStr.Substring(6, 2)})).AddMonths(-4).ToString("yyyyMMdd")
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                'TODO(StoredProcedure) daoInc.GetUnionMemberWork(command, strMac, strControlName, strUserId, strStr, strKeyAgeDate, strKsh, "01")
                daoInc.GetDuesWork(command, strMac, strControlName, strUserId, strStr, strKeyAgeDate, strKsh, "01", True)
                objTran.Commit()
                Dim revenueExpenditureMemberDtlView As DataTable = daoInc.GetRevenueExpenditureMemberDtlView(strMac, strControlName)
                Dim table2 As DataTable = Me.ConvetDBtoFlexGrid(revenueExpenditureMemberDtlView)
                iSeniorMonthwork = Me.GetSeniorMonthWork(strMac, strControlName, strUserId, strStr, strKsh)
                table3 = table2
            Catch exception As AppUnionException
                objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                objTran.Rollback()
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table3
        End Function

        Private Sub GetRealNumberDuesTtl(ByVal strKeyDate As String, ByVal dTableIn As DataTable, ByVal strKQualification As String, <Out()> ByRef iRealNumberTtl As Integer, <Out()> ByRef lRealUnionDues As Long)
            Try
                iRealNumberTtl = 0
                lRealUnionDues = 0
                Dim retireAge As Integer = Me.GetRetireAge(strKeyDate, "01")
                Dim rowArray As DataRow() = dTableIn.Select(("k_qualification = '" & strKQualification & "'"))
                If (rowArray.Length > 0) Then
                    Dim i As Integer
                    For i = 0 To dTableIn.Columns.Count - 1
                        Dim caption As String = dTableIn.Columns.Item(i).Caption
                        If (((caption.Length = 11) AndAlso caption.Substring(0, 9).Equals("s_number_")) AndAlso (Convert.ToInt32(caption.Substring(9, 2)) < retireAge)) Then
                            iRealNumberTtl = (iRealNumberTtl + Convert.ToInt32(rowArray(0).Item(i)))
                            lRealUnionDues = (lRealUnionDues + Convert.ToInt32(rowArray(0).Item((i + 1))))
                        End If
                    Next i
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub GetRegularShowTerm(ByVal strKeyDate As String, ByRef iAgeStart As Integer, ByRef iAgeEnd As Integer)
            Try
                Dim dao As New ConstantTblDao
                Dim table As DataTable = dao.GetConstantKind("REGULAR_SHOW_TERM", strKeyDate).Tables.Item(0).Copy
                Dim count As Integer = table.Rows.Count
                Dim rowArray As DataRow() = table.Select("c_constant_seq = '01' ")
                Dim rowArray2 As DataRow() = table.Select("c_constant_seq = '02' ")
                If ((rowArray.Length = 0) OrElse rowArray(0).Item("l_omission_name").ToString.Equals("")) Then
                    iAgeStart = &H19
                End If
                If ((rowArray2.Length = 0) OrElse rowArray2(0).Item("l_omission_name").ToString.Equals("")) Then
                    iAgeStart = 60
                End If
                Try
                    iAgeStart = Convert.ToInt32(rowArray(0).Item("l_omission_name").ToString)
                    iAgeEnd = Convert.ToInt32(rowArray2(0).Item("l_omission_name").ToString)
                Catch obj1 As Exception
                    iAgeStart = &H19
                    iAgeEnd = 60
                End Try
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
        End Sub

        Private Function GetRetireAge(ByVal strKeyDate As String, ByVal strConstantSeq As String) As Integer
            Dim num As Integer = 0
            Try
                Dim dao As New ConstantTblDao
                Dim table As DataTable = dao.GetConstantKind("RETIRE_AGE", strKeyDate).Tables.Item(0).Copy
                Dim count As Integer = table.Rows.Count
                Dim rowArray As DataRow() = table.Select(("c_constant_seq = '" & strConstantSeq & "'"))
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
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return num
        End Function

        Private Function GetSeniorMonthWork(ByVal strMac As String, ByVal strControlName As String, ByVal strUserId As String, ByVal strStr As String, ByVal strKsh As String) As Integer
            Dim num7 As Integer
            Dim objTran As NpgsqlTransaction = CommonDaoClass.connNpgsql.BeginTransaction
            Try
                Dim num As Integer = 0
                Dim strKeyAgeDate As String = DateTime.Parse(String.Concat(New String() {strStr.Substring(0, 4), "/", strStr.Substring(4, 2), "/", strStr.Substring(6, 2)})).AddMonths(-4).ToString("yyyyMMdd")
                Dim command As New NpgsqlCommand("", CommonDaoClass.connNpgsql, objTran)
                'TODO(StoredProcedure) daoInc.GetUnionMemberWork(strMac, strControlName, strUserId, strStr, strKeyAgeDate, strKsh, "02")
                daoInc.GetDuesWork(command, strMac, strControlName, strUserId, strStr, strKeyAgeDate, strKsh, "02", True)
                objTran.Commit()
                Dim revenueExpenditureMemberDtlView As DataTable = daoInc.GetRevenueExpenditureMemberDtlView(strMac, strControlName)
                Dim retireAge As Integer = Me.GetRetireAge(strStr, "01")
                Dim num4 As Integer = Me.GetRetireAge(strStr, "02")
                Dim i As Integer
                For i = 0 To revenueExpenditureMemberDtlView.Rows.Count - 1
                    Dim j As Integer
                    For j = 0 To revenueExpenditureMemberDtlView.Columns.Count - 1
                        Dim caption As String = revenueExpenditureMemberDtlView.Columns.Item(j).Caption
                        If ((caption.Length = 11) AndAlso caption.Substring(0, 9).Equals("s_number_")) Then
                            Dim num2 As Integer = Convert.ToInt32(caption.Substring(9, 2))
                            If (((num2 >= (retireAge - 1)) AndAlso (num2 < num4)) AndAlso Not revenueExpenditureMemberDtlView.Rows.Item(i).Item(j).ToString.Equals("")) Then
                                num = (num + Convert.ToInt32(revenueExpenditureMemberDtlView.Rows.Item(i).Item(j)))
                            End If
                        End If
                    Next j
                Next i
                num7 = (num * 12)
            Catch exception As AppUnionException
                objTran.Rollback()
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                objTran.Rollback()
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                objTran.Rollback()
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return num7
        End Function

        Private Function MakingDataForFlexGrid(ByVal dTableIn As DataTable) As DataTable
            Dim table2 As DataTable
            Try
                Dim map As New IncomeExpectMap
                map.CreateDataTableLogiName("income_expect")
                table2 = PublicCommand.ConvertPhysicalToLogical(dTableIn, map)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "BE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        ' Fields
        Private dao As New RevenueExpenditureDao
        Private daoInc As New IncomeExpectDao
    End Class
End Namespace
