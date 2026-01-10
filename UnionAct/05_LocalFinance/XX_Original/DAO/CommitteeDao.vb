'Imports DAO
'Imports Framework.Mapping
'Imports Framework.UnionException
#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Mapping

Namespace DAO.Master
    Public Class CommitteeDao
        Inherits AbstractDao
        'Implements ICommitteeDao
        ' Methods
        Public Function CheckCommittee(ByVal strKsh As String, ByVal strCommitteeId As String, ByVal strKeyDate As String) As Integer
            Dim num As Integer
            Try 
                Dim cmdText As String = "select count(*) from committee  where      c_ksh          = :c_ksh  and c_committee_id = :c_committee_id  and d_from        <= :d_date  and d_to          >= :d_date "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strKeyDate
                num = Convert.ToInt32(command.ExecuteScalar.ToString)
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
            Return num
        End Function

        Public Function GetAllData() As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New CommitteeMap
                Dim dReader As NpgsqlDataReader = New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM " & "	" & "committee ORDER BY " & "	" & "c_committee_id "), MyBase.GetNpgsqlConnection).ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織マスタの情報" })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("committee", dReader))
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

        Public Function GetAllData(ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New CommitteeMap
                Dim command As New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM " & "	" & "committee WHERE " & "	" & "d_from <= :d_date " & "	" & "AND d_to >= :d_date ORDER BY " & "	" & "c_committee_id "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("committee", dReader))
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

        Public Function GetDataCommitteeInPeriod(ByVal strKsh As String, ByVal c_period_id As String, ByVal k_period_kind As String, ByVal strCommitteeId As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim table As DataTable
                Dim str2 As String = New CommitteeMap().ToPhysicalString("COM.")
                Dim str As String = ("select " & str2 & " from committee COM, (select   COMMAX.c_committee_id,  max(COMMAX.d_from) as d_from   from committee COMMAX, period PER   where       COMMAX.c_ksh         = :c_ksh   and PER.c_period_id      = :c_period_id   and PER.k_period_kind    = :k_period_kind ")
                If Not strCommitteeId.Equals("") Then
                    str = (str & "and COMMAX.c_committee_id = :c_committee_id ")
                End If
                str = (str & "  and COMMAX.d_from       <= PER.d_to   and COMMAX.d_to         >= PER.d_FROM   group by COMMAX.c_committee_id   ) COMMAX2 where     COM.c_ksh           = :c_ksh and COM.d_from          = COMMAX2.d_from and COM.c_committee_id  = COMMAX2.c_committee_id ")
                If Not strCommitteeId.Equals("") Then
                    str = (str & "and COM.c_committee_id = :c_committee_id ")
                End If
                Dim command As New NpgsqlCommand((str & "order by COM.c_committee_id  "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_period_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("k_period_kind", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("c_period_id").Value = c_period_id
                command.Parameters.Item("k_period_kind").Value = k_period_kind
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織マスタの情報" })
                End If
                If Not strCommitteeId.Equals("") Then
                    table = MyBase.CreateSomeDataSet("committee_period", dReader)
                Else
                    table = MyBase.CreateSomeDataSet("committee", dReader)
                End If
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

        Public Function GetDataOfCoMid(ByVal strKsh As String, ByVal strCommitteeId As String, ByVal strDTo As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim str2 As String = New CommitteeMap().ToPhysicalString("")
                Dim command As New NpgsqlCommand(("select " & str2 & " from committee where c_ksh          = :c_ksh   and c_committee_id = :c_committee_id   and d_from        <= :d_date   and d_to          >= :d_date "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_committee_id", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("c_committee_id").Value = strCommitteeId
                command.Parameters.Item("d_date").Value = strDTo
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Return Nothing
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee", dReader)
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

        Public Function GetDataOfComList(ByVal strKsh As String, ByVal dTableCommitteeListDtl As DataTable, ByVal strDate As String) As DataSet
            Dim set3 As DataSet
            Try 
                Dim table As New DataTable
                Dim ds As New DataSet
                Dim table2 As New DataTable
                table = dTableCommitteeListDtl.Copy
                table2 = New DataTable
                table2 = New CommitteeMap().CreateDataTablePhysName("committee")
                Dim flag As Boolean = False
                Dim i As Integer
                For i = 0 To table.Rows.Count - 1
                    Dim strCommitteeId As String = table.Rows.Item(i).Item("c_committee_id").ToString
                    flag = False
                    Dim j As Integer
                    For j = 0 To table2.Rows.Count - 1
                        If strCommitteeId.Equals(table2.Rows.Item(j).Item("c_committee_id").ToString) Then
                            flag = True
                            Exit For
                        End If
                    Next j
                    If Not flag Then
                        Dim set2 As DataSet = Me.GetDataOfCoMid(strKsh, strCommitteeId, strDate)
                        If ((Not set2 Is Nothing) AndAlso (set2.Tables.Count > 0)) Then
                            Dim table3 As DataTable = set2.Tables.Item(0).Copy
                            table2.Merge(table3)
                        End If
                    End If
                Next i
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

        Public Function GetDataOfComListOfPeriod(ByVal strKsh As String, ByVal dTableCommitteeListDtl As DataTable, ByVal strPeriodId As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim table As New DataTable
                Dim ds As New DataSet
                Dim table2 As New DataTable
                Dim str2 As String = "01"
                table = dTableCommitteeListDtl.Copy
                table2 = New DataTable
                table2 = New CommitteeMap().CreateDataTablePhysName("committee_period")
                Dim i As Integer
                For i = 0 To table.Rows.Count - 1
                    Dim strCommitteeId As String = table.Rows.Item(i).Item("c_committee_id").ToString
                    Dim table3 As DataTable = Me.GetDataCommitteeInPeriod(strKsh, strPeriodId, str2, strCommitteeId).Tables.Item(0).Copy
                    table2.Merge(table3)
                Next i
                ds.Tables.Add(table2)
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

        Public Function GetDataOfDate(ByVal strKsh As String, ByVal strDTo As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim str2 As String = New CommitteeMap().ToPhysicalString("")
                Dim command As New NpgsqlCommand(("select " & str2 & " from committee where c_ksh          = :c_ksh   and d_from        <= :d_date   and d_to          >= :d_date order by c_committee_id "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("d_date").Value = strDTo
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織マスタの情報" })
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee", dReader)
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

        Public Function GetDataOfDate(ByVal strKsh As String, ByVal strDateFrom As String, ByVal strDateTo As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim str2 As String = New CommitteeMap().ToPhysicalString("")
                Dim command As New NpgsqlCommand(("select " & str2 & " from committee where c_ksh          = :c_ksh   and d_from        <= :d_to   and d_to          >= :d_from order by c_committee_id "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_to", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("d_from").Value = strDateFrom
                command.Parameters.Item("d_to").Value = strDateTo
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "委員会組織マスタの情報" })
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("committee", dReader)
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

        Public Function GetKeyDate(ByVal strCommitteeID As String, ByVal strPeriodID As String, ByVal strBasisDate As String) As DataTable
            Dim table As DataTable
            Try 
                Dim cmdText As String = "select COALESCE( (select d_from from committee where c_committee_id = :c_committee_id and :date_basis between d_from and d_to ), max(COMMITTEE.d_from) ) as d_from from committee COMMITTEE, (select d_from, d_to from period where c_period_id = :c_period_id ) PERIOD where COMMITTEE.c_committee_id = :c_committee_id and COMMITTEE.d_to >= PERIOD.d_from and COMMITTEE.d_from <= PERIOD.d_to group by COMMITTEE.c_committee_id "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("c_committee_id", strCommitteeID)
                command.Parameters.Add("c_period_id", strPeriodID)
                command.Parameters.Add("date_basis", strBasisDate)
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                table = MyBase.CreateSomeDataSet("committee", dReader)
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
            Return table
        End Function

    End Class
End Namespace
