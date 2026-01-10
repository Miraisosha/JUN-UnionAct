Imports log4net
#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException
Imports UnionAct.Framework.Mapping

Namespace DAO.Master
    Public Class ConstantTblDao
        Inherits AbstractDao
        'Implements IConstantTblDao
        ' Methods
        Public Function GetApplyStrikeLimit(ByVal strApplyClassify As String, ByVal strKeyDate As String) As String
            Dim str3 As String
            Try 
                Dim str As String
                Dim cmdText As String = "select l_name  from constant_dtl  where c_constant     = 'APPLY_STRIKE_LIMIT'    and c_constant_seq = :c_constant_seq    and d_from        <= :c_date    and d_to          >= :c_date "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_constant_seq", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("c_date", DbType.String))
                command.Parameters.Item("c_constant_seq").Value = strApplyClassify
                command.Parameters.Item("c_date").Value = strKeyDate
                If (command.ExecuteScalar Is Nothing) Then
                    str = ""
                Else
                    str = command.ExecuteScalar.ToString
                End If
                str3 = str
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return str3
        End Function

        Public Function GetConstantDtlTbl(ByVal basisDate As String) As DataSet
            Dim set2 As DataSet
            Dim cmdText As String = ""
            Try 
                Dim map As New Constant_DtlMap
                cmdText = ("select " & map.ToPhysicalString("") & " from constant_dtl where d_from <= :d_from   and :d_from <= d_to ")
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("d_from").Value = basisDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New DataNotFoundException(("TABLE:[" & "定数マスタ" & "] " & "適用開始日" & ":[" & basisDate & "]"))
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("constant_dtl", dReader)
                Dim ds As New DataSet
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As DataNotFoundException
                Throw New AppUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0  - 1) {})
            Catch exception2 As AppUnionException
                ConstantTblDao._logger.Error(("SQL= " & cmdText))
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As SysUnionException
                ConstantTblDao._logger.Error(("SQL= " & cmdText))
                exception3.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception3
            Catch exception4 As Exception
                ConstantTblDao._logger.Error(("SQL= " & cmdText))
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception4, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetConstantDtlTblAll() As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New Constant_DtlMap
                Dim dReader As NpgsqlDataReader = New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM constant_dtl "), MyBase.GetNpgsqlConnection).ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0001", New String(0  - 1) {})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("constant_dtl", dReader)
                Dim ds As New DataSet
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetConstantKind(ByVal strConstant As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New Constant_DtlMap
                Dim command As New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM constant_dtl WHERE " & "	" & "c_constant = :c_constant ORDER BY s_order "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_constant", DbType.String))
                command.Parameters.Item("c_constant").Value = strConstant
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { ("定数" & "ID" & "が「" & " " & strConstant & "」のデータ") })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("constant_dtl", dReader))
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

        Public Function GetConstantKind(ByVal strConstant As String, ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New Constant_DtlMap
                Dim command As New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM constant_dtl WHERE " & "	" & "c_constant = :c_constant " & "	" & "AND d_from <= :d_date " & "	" & "AND d_to >= :d_date ORDER BY s_order "), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_constant", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_constant").Value = strConstant
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { ("定数" & "ID" & "が「" & " " & strConstant & "」のデータ") })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("constant_dtl", dReader))
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

        Public Function GetConstantName(ByVal strConstantId As String, ByVal strDate As String) As DataTable
            Dim table2 As DataTable
            Dim cmdText As String = " select l_name from constant_dtl where c_constant = :c_constant   and d_from    <= :strDate   and d_to      >= :strDate"
            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
            command.Parameters.Add(New NpgsqlParameter("c_constant", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
            command.Parameters.Item("c_constant").Value = strConstantId
            command.Parameters.Item("strDate").Value = strDate
            Try 
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { ("定数" & "ID" & "が「" & " " & strConstantId & "」のデータ") })
                End If
                Dim table As New DataTable
                table2 = MyBase.CreateSomeDataSet("dtHeader", dReader)
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
            Return table2
        End Function

        Public Function GetConstantSeq(ByVal strConstant As String, ByVal strDate As String) As String()
            Dim strArray As String()
            Dim cmdText As String = " select c_constant_seq from constant_dtl where c_constant = :c_constant   and d_from    <= :strDate   and d_to      >= :strDate"
            Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
            command.Parameters.Add(New NpgsqlParameter("c_constant", DbType.String))
            command.Parameters.Add(New NpgsqlParameter("strDate", DbType.String))
            command.Parameters.Item("c_constant").Value = strConstant
            command.Parameters.Item("strDate").Value = strDate
            Try 
                Dim reader As NpgsqlDataReader = command.ExecuteReader
                If Not reader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { ("定数" & "ID" & "が「" & " " & strConstant & "」のデータ") })
                End If
                Dim list As New List(Of String)
                Dim i As Integer = 0
                Do While reader.Read
                    list.Add(reader.Item("c_constant_seq").ToString)
                    i += 1
                Loop
                strArray = list.ToArray
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
            Return strArray
        End Function

        Public Function GetConstantTbl(ByVal basisDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "select * from constant where d_from <= :d_from   and :d_from <= d_to "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("d_from", DbType.String))
                command.Parameters.Item("d_from").Value = basisDate
                Dim reader As NpgsqlDataReader = command.ExecuteReader
                If Not reader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0001", New String(0  - 1) {})
                End If
                Dim map As New ConstantMap
                Dim table As DataTable = map.CreateDataTablePhysName("constant")
                Do While reader.Read
                    Dim row As DataRow = table.NewRow
                    Dim i As Integer
                    For i = 0 To map.ColumnCount - 1
                        row.Item(map.GetPhysicalName(i)) = reader.Item(map.GetPhysicalName(i))
                    Next i
                    table.Rows.Add(row)
                Loop
                Dim ds As New DataSet
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetConstantTblAll() As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New ConstantMap
                Dim dReader As NpgsqlDataReader = New NpgsqlCommand(("SELECT " & map.ToPhysicalString("") & " FROM constant "), MyBase.GetNpgsqlConnection).ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0001", New String(0  - 1) {})
                End If
                Dim table As DataTable = MyBase.CreateSomeDataSet("constant", dReader)
                Dim ds As New DataSet
                ds.Tables.Add(table)
                set2 = ds
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return set2
        End Function

        Public Function GetDailyPayKindAll(ByVal strDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim map As New Constant_DtlMap
                Dim command As New NpgsqlCommand(("SELECT" & "	" & map.ToPhysicalString("") & " FROM constant_dtl WHERE " & "	" & "c_constant = :c_constant " & "	" & "AND d_from <= :d_date " & "	" & "AND d_to >= :d_date ORDER BY " & "	" & "s_order"), MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_constant", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_constant").Value = "DAILY_PAY_KIND"
                command.Parameters.Item("d_date").Value = strDate
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                If Not dReader.HasRows Then
                    Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "定数" & "ID" & "が「" & " DAILY_PAY_KIND" & "」のデータ" })
                End If
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("constant_dtl", dReader))
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

        Public Function GetLawOfficeList(ByVal strBasisDate As String) As DataSet
            Dim set2 As DataSet
            Try 
                Dim cmdText As String = "select c_constant, c_constant_seq, l_name as " & "事務所名" & ", cast(null as integer) as " & "支給総額" & " from constant_dtl where d_from <= :d_from and :d_from <= d_to and c_constant = :constant_id_law_office order by s_order "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add("d_from", strBasisDate)
                command.Parameters.Add("constant_id_law_office", "LAW_OFFICE")
                Dim dReader As NpgsqlDataReader = command.ExecuteReader
                Dim ds As New DataSet
                ds.Tables.Add(MyBase.CreateSomeDataSet("constant_dtl", dReader))
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

        Public Function GetOfficerName(ByVal strKsh As String, ByVal strKeyDate As String) As String
            Dim str3 As String
            Try 
                Dim str As String = ""
                Dim cmdText As String = "select l_name from officer_name_view  where c_constant_seq = :c_ksh    and d_from <= :d_date    and d_to   >= :d_date "
                Dim command As New NpgsqlCommand(cmdText, MyBase.GetNpgsqlConnection)
                command.Parameters.Add(New NpgsqlParameter("c_ksh", DbType.String))
                command.Parameters.Add(New NpgsqlParameter("d_date", DbType.String))
                command.Parameters.Item("c_ksh").Value = strKsh
                command.Parameters.Item("d_date").Value = strKeyDate
                If (command.ExecuteScalar Is Nothing) Then
                    str = ""
                Else
                    str = command.ExecuteScalar.ToString
                End If
                str3 = str
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0  - 1) {})
            End Try
            Return str3
        End Function


        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
    End Class
End Namespace
