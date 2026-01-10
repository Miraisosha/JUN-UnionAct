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
Imports UnionAct.Framework.Mapping

Namespace DAO.Master
    Public Class CommitteeDtlDao
        Inherits AbstractDao
        'Implements ICommitteeDtlDao
        ' Methods
        Public Function GetCommitteeListForKind(ByVal strCommitteeCondition As String, ByVal strCommitteeDtlCondition As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim command As New NpgsqlCommand(String.Concat(New String() { "SELECT " & "	" & "committee_dtl.c_committee_id, " & "	" & "committee_dtl.s_committee_seq FROM " & "	" & "committee_dtl, " & "	" & "(SELECT " & "		" & "c_committee_id, " & "		" & "MAX(d_from) AS d_from " & "	" & "FROM " & "		" & "committee " & "	" & "WHERE " & "		" & "c_ksh = :c_ksh ", strCommitteeCondition, "		" & "AND d_from <= :d_date " & "		" & "AND d_to >= :d_date " & "	" & "GROUP BY " & "		" & "c_committee_id " & "	" & ") committee_A WHERE " & "	" & "committee_dtl.d_from = committee_A.d_from ", strCommitteeDtlCondition, "GROUP BY " & "	" & "committee_dtl.c_committee_id, " & "	" & "committee_dtl.s_committee_seq ORDER BY " & "	" & "committee_dtl.c_committee_id, " & "	" & "committee_dtl.s_committee_seq " }), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織明細マスタの情報" })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("committee_dtl", dReader))
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

        Public Function GetCommitteePostName(ByVal strCommitteeId As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "SELECT " & "	" & "s_committee_seq, " & "	" & "l_name, " & "	" & "d_from, " & "	" & "d_to FROM " & "	" & "committee_dtl WHERE " & "	" & "c_committee_id = :c_committee_id ORDER BY " & "	" & "s_committee_seq "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織明細マスタの情報" })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("committee_dtl", dReader))
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

        Private Function GetDataOfCoMid(ByVal strCommitteeId As String, ByVal iCommitteeSeq As Integer, ByVal strDTo As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim str2 As String = New CommitteeDtlMap().ToPhysicalString("")
                Dim command As New NpgsqlCommand(("select " & str2 & " from committee_dtl where c_committee_id  = :c_committee_id   and s_committee_seq = :s_committee_seq   and d_from         <= :d_date   and d_to           >= :d_date "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("s_committee_seq").Value = iCommitteeSeq
                command.Parameters.Item("d_date").Value = strDTo
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_dtl", dReader)
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

        Public Function GetDataOfComList(ByVal dTableCommitteeListDtl As DataTable, ByVal strDate As String) As DataSet
            Dim set3 As DataSet
            Try 
                Dim table As New DataTable
                Dim ds As New DataSet
                Dim table2 As New DataTable
                table = dTableCommitteeListDtl.Copy
                table2 = New DataTable
                table2 = New CommitteeDtlMap().CreateDataTablePhysName("committee_dtl")
                Dim flag As Boolean = False
                Dim i As Integer
                For i = 0 To table.Rows.Count - 1
                    Dim strCommitteeId As String = table.Rows.Item(i).Item("c_committee_id").ToString
                    Dim iCommitteeSeq As Integer = CInt(table.Rows.Item(i).Item("s_committee_seq"))
                    flag = False
                    Dim j As Integer
                    For j = 0 To table2.Rows.Count - 1
                        If (strCommitteeId.Equals(table2.Rows.Item(j).Item("c_committee_id").ToString) AndAlso iCommitteeSeq.ToString.Equals(table2.Rows.Item(j).Item("s_committee_seq").ToString)) Then
                            flag = True
                            Exit For
                        End If
                    Next j
                    If Not flag Then
                        Dim set2 As DataSet = Me.GetDataOfCoMid(strCommitteeId, iCommitteeSeq, strDate)
                        If ((Not set2 Is Nothing) AndAlso (set2.Tables.Count > 0)) Then
                            Dim table3 As DataTable = set2.Tables.Item(0).Copy
                            table2.Merge(table3)
                        End If
                    End If
                Next i
                If (table2.Rows.Count <= 0) Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織明細マスタの情報" })
                End If
                ds.Tables.Add(table2)
                set3 = ds
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
            Return set3
        End Function

        Public Function GetDataOfDate(ByVal strDTo As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim str2 As String = New CommitteeDtlMap().ToPhysicalString("")
                Dim command As New NpgsqlCommand(("select " & str2 & " from committee_dtl where d_from         <= :d_date   and d_to           >= :d_date order by c_committee_id, s_committee_seq "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("d_date").Value = strDTo
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織明細マスタの情報" })
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_dtl", dReader)
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

        Public Function GetExecutiveLunchPayId(ByVal strCommitteeId As String, ByVal strCommitteeSeq As String, ByVal strDate As String) As String
            Dim str2 As String
            Try 
                Dim cmdText As String = "SELECT" & "	" & "c_executive_lunch_pay_id FROM" & "	" & "committee_dtl WHERE c_committee_id = :c_committee_id AND s_committee_seq = :s_committee_seq AND d_from <= :d_date AND d_to >= :d_date "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("s_committee_seq", DbType.Int32))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("c_committee_id").Value = Integer.Parse(strCommitteeSeq)
                command.Parameters.Item("d_date").Value = PublicCommand.GetSystemDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_dtl", dReader)
                If ((table.Rows.Item(0).Item("c_executive_lunch_pay_id") Is Nothing) OrElse table.Rows.Item(0).Item("c_executive_lunch_pay_id").ToString.Equals("")) Then
                    Return Nothing
                End If
                str2 = table.Rows.Item(0).Item("c_executive_lunch_pay_id").ToString
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
            Return str2
        End Function

        Public Function GetMaxComMonth(ByVal strCommitteeId As String, ByVal strDTo As String) As Integer
            Dim num2 As Integer
            Try 
                Dim cmdText As String = "select max(s_to_diff) as MaxMonth  from committee_dtl where c_committee_id  = :c_committee_id   and d_from         <= :d_date   and d_to           >= :d_date "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strDTo
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_dtl", dReader)
                Dim result As Integer = 0
                If (table.Rows.Count > 0) Then
                    Integer.TryParse(table.Rows.Item(0).Item("MaxMonth").ToString, result)
                    Return result
                End If
                num2 = 0
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
            Return num2
        End Function

        Public Function GetMonthDifference(ByVal strDate As String, ByVal strCommitteeId As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "SELECT " & "	" & "MIN(committee_dtl.s_from_diff), " & "	" & "MAX(committee_dtl.s_to_diff) FROM " & "	" & "(SELECT " & "		" & "c_committee_id, " & "		" & "d_from " & "	" & "FROM " & "		" & "committee " & "	" & "WHERE " & "		" & "c_committee_id = :c_committee_id " & "		" & "AND d_from <= :d_date " & "		" & "AND d_to >= :d_date " & "		" & "AND c_ksh = :c_ksh " & "	" & ") committee_A, " & "	" & "committee_dtl WHERE " & "	" & "committee_dtl.c_committee_id = committee_A.c_committee_id " & "	" & "AND committee_dtl.d_from = committee_A.d_from "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strDate
                command.Parameters.Item("c_ksh").Value = PublicCommand.GetKsh
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織明細マスタの情報" })
                End If
                Dim ds As New DataSet
                Dim table As DataTable = MyBase.CreateSomeDataSet("month_difference", dReader)
                If (String.IsNullOrEmpty(table.Rows.Item(0).Item("min").ToString) OrElse String.IsNullOrEmpty(table.Rows.Item(0).Item("max").ToString)) Then
                    Return Nothing
                End If
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

        Public Function GetServiceMinMaxDiff(ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "SELECT " & "	" & "MIN(s_from_diff) AS min, " & "	" & "MAX(s_to_diff) AS max FROM " & "	" & "committee_dtl WHERE " & "	" & "d_from <= :d_date " & "	" & "AND d_to >= :d_date "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織明細マスタの情報" })
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee_dtl", dReader)
                Dim ds As New DataSet
                ds.Tables.Add(table)
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
