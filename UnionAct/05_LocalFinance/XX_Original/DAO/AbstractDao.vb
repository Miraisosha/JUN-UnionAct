Imports UnionAct.Framework.Command
Imports UnionAct.Framework.Interface
Imports UnionAct.Framework.Mapping
Imports UnionAct.Framework.UnionException
Imports UnionAct.NSMDCommon
#If USE_POSTGRES Then
Imports Npgsql
#Else
Imports UnionAct.NpgsqlDummy
#End If
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Reflection
Imports System.Text
Imports System.Text.RegularExpressions

Namespace DAO
    Public MustInherit Class AbstractDao
        ' Methods
        Protected Sub New()
        End Sub

        Public Function CreateSomeDataSet(ByVal strTable As String, ByVal dReader As NpgsqlDataReader) As DataTable
#If USE_POSTGRES = 0 Then
            dReader.getTable().TableName = strTable
            Return dReader.getTable()
#Else
            Dim table2 As DataTable
            Try 
                Dim table As New DataTable(strTable)
                Dim strArray As String() = New String() { "varchar", "bpchar", "int4", "int8", "timestamp", "oid", "bit", "numeric", "date", "text", "bool", "unknown" }
                Dim typeArray As Type() = New Type() { GetType(String), GetType(String), GetType(Integer), GetType(Long), GetType(DateTime), GetType(Integer), GetType(Byte), GetType(Double), GetType(DateTime), GetType(String), GetType(Boolean), GetType(Object) }
                table.Clear
                Dim i As Integer
                For i = 0 To dReader.FieldCount - 1
                    Dim dataTypeName As String = dReader.GetDataTypeName(i)
                    Dim index As Integer = 0
                    Do While (index < strArray.Length)
                        If dataTypeName.Equals(strArray(index)) Then
                            Exit Do
                        End If
                        index += 1
                    Loop
                    If (index = strArray.Length) Then
                        Throw New AppUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0003", New String() { "ƒe[ƒuƒ‹€–Ú‚ÌŒ^–¼" })
                    End If
                    table.Columns.Add(dReader.GetName(i), typeArray(index))
                Next i
                Dim values As Object() = New Object(dReader.FieldCount  - 1) {}
                Do While dReader.Read
                    Dim j As Integer
                    For j = 0 To dReader.FieldCount - 1
                        values(j) = dReader.Item(dReader.GetName(j))
                    Next j
                    table.Rows.Add(values)
                Loop
                dReader.Close
                table2 = table
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
#End If
        End Function

        Private Function GetColumnString(ByVal viewName As String, ByVal ParamArray columns As String()) As String
            Dim str2 As String
            Try 
                Dim str As String = ""
                If Not String.IsNullOrEmpty(viewName) Then
                    str = (viewName & ".")
                End If
                Dim builder As New StringBuilder
                Dim i As Integer
                For i = 0 To columns.Length - 1
                    builder.Append(If((builder.Length = 0), (str & columns(i)), (", " & str & columns(i))))
                Next i
                builder.Append(" ")
                str2 = builder.ToString
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0  - 1) {})
            End Try
            Return str2
        End Function

        Public Function GetConnectionString() As String
            If Not CommonDataClass.Instance.IsTrainingMode Then
                Return CommonDataClass.Instance.strConStr
            End If
            Return CommonDataClass.Instance.strConStrTraining
        End Function

        Public Function GetNpgsqlConnection() As NpgsqlConnection
            Return CommonDaoClass.connNpgsql
        End Function

        Public Function GetNpgsqlTran() As NpgsqlTransaction
            Return CommonDaoClass.tranNpgsql
        End Function

        Public Function GetParamsFromSQL(ByVal strSQL As String) As List(Of String)
            Dim list2 As List(Of String)
            Try 
                Dim pattern As String = ":[a-zA-Z\d_]+"
                Dim matchs As MatchCollection = Regex.Matches(strSQL, pattern)
                Dim list As New List(Of String)
                Dim match As Match
                For Each match In matchs
                    Dim item As String = match.Value.Replace(":", "")
                    If Not list.Contains(item) Then
                        list.Add(item)
                    End If
                Next
                list2 = list
            Catch exception As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception, "DE0001", New String(0  - 1) {})
            End Try
            Return list2
        End Function

        Public Function GetStafAttributeView(ByVal basisDate As String, ByVal viewName As String, ByVal isHost As Boolean, ByVal ParamArray getInfo As String()) As String
            Dim str3 As String
            Try 
                Dim str As String = ""
                If isHost Then
                    str = basisDate
                Else
                    str = (" '" & basisDate & "' ")
                End If
                str3 = String.Concat(New String() { "(select ", Me.GetColumnString("STAF_INFO", getInfo), "from (select c_user_id, max(d_from) as d_from from staf_attribute where c_ksh = '", PublicCommand.GetKsh, "' and d_from <= ", str, " group by c_user_id ) STAF_MAX, staf_attribute STAF_INFO where STAF_MAX.c_user_id = STAF_INFO.c_user_id and STAF_MAX.d_from = STAF_INFO.d_from and STAF_INFO.k_del = '0' ) ", viewName, " " })
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0  - 1) {})
            End Try
            Return str3
        End Function

        Public Sub InsertData(ByVal command As NpgsqlCommand, ByVal map As EntityMap, ByVal dTblNewData As DataTable)
            Try
                Dim dSetNewData As New DataSet
                dSetNewData.Tables.Add(dTblNewData.Copy)
                Me.InsertData(command, map, dSetNewData, dTblNewData.TableName)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub InsertData(ByVal map As EntityMap, ByVal dTblNewData As DataTable)
            Try
                Dim dSetNewData As New DataSet
                dSetNewData.Tables.Add(dTblNewData.Copy)
                Me.InsertData(New NpgsqlCommand("", CommonDaoClass.connNpgsql), map, dSetNewData, dTblNewData.TableName)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub InsertData(ByVal command As NpgsqlCommand, ByVal map As EntityMap, ByVal dSetNewData As DataSet, ByVal strTableName As String)
            Try
                'Dim command As New NpgsqlCommand(String.Concat(New String() {"insert into ", strTableName, " ( ", map.ToPhysicalString(""), " ) values ( ", map.ToPhysicalString(":"), " ) "}), Me.GetNpgsqlConnection)
                Dim cmdText As String = String.Concat(New String() {"insert into ", strTableName, " ( ", map.ToPhysicalString(""), " ) values ( ", map.ToPhysicalString(":"), " ) "})
                cmdText = cmdText.Replace(":d_ins", "CONVERT(DATE,':d_ins')")
                cmdText = cmdText.Replace(":d_up", "CONVERT(DATE,':d_up')")
                Dim i As Integer
                Dim value As Object
                Dim type As DbType
                For i = 0 To dSetNewData.Tables.Item(strTableName).Rows.Count - 1
                    command.SetSql(cmdText)
                    command.Parameters.Clear()
                    Dim j As Integer
                    For j = 0 To map.ColumnCount - 1
                        type = map.GetDbDataType(j)
                        value = dSetNewData.Tables.Item(strTableName).Rows.Item(i).Item(map.GetPhysicalName(j))
                        command.Parameters.Add(map.GetPhysicalName(j), type)
                        command.Parameters.Item(map.GetPhysicalName(j)).Value = IIf(type = DbType.String, MDCommon.NVL(value), value)
                    Next j
                    If command.Parameters.Contains("c_user_id_up") Then
                        command.Parameters.Item("c_user_id_up").Value = DBNull.Value
                    End If
                    If command.Parameters.Contains("d_up") Then
                        command.Parameters.Item("d_up").Value = DBNull.Value
                    End If
                    If (command.ExecuteNonQuery <> 1) Then
                        Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "DE0002", New String(0 - 1) {})
                    End If
                Next i
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As NpgsqlException
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception2, "DE0002", New String(0 - 1) {})
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "DE0001", New String(0 - 1) {})
            End Try
        End Sub

        Public Sub SetNpgsqlConnection(ByVal connNpgsql As NpgsqlConnection)
            CommonDaoClass.connNpgsql = connNpgsql
        End Sub

    End Class
End Namespace
