'===========================================================================================================
'   ネームスペース：NpgsqlDummy
'   モジュールＩＤ：MDNpgsqlDummy
'   モジュール名称：DBアクセス共通モジュール
'   備考  　　　　：MS-ACCESS用Npgsql代替クラス群
'===========================================================================================================
Imports System.Data.OleDb
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo

Namespace NpgsqlDummy
    ' コネクション
    Public Class NpgsqlConnection
        Private con As OleDbConnection

        Public Sub New()
            Dim cs As String = MDSystemInfo.DbConnectionString
            'Dim cs As String = "Provider=" & MDSystemInfo.AccessProvider
            'cs = cs & ";Data Source=" & MDSystemInfo.AccessPath & MDSystemInfo.AccessName
            'cs = cs & ";Persist Security Info=" & AccessPersistSecurity
            ''cs = cs & ";User ID=" & AccessUserId
            'cs = cs & ";Jet OLEDB:Database Password=" & AccessPassword
            con = New OleDbConnection(cs)
            con.Open()
        End Sub

        Public Sub New(ByVal strConnectionString As String)
            con = New OleDbConnection(strConnectionString)
            con.Open()
        End Sub

        Public Sub open()
            If con.State = ConnectionState.Closed Then
                con.Open()
            End If
        End Sub

        Public Sub close()
            con.Dispose()
        End Sub

        Public Function CreateCommand() As OleDbCommand
            Return con.CreateCommand
        End Function

        Public Function BeginTransaction() As NpgsqlTransaction
            Return New NpgsqlTransaction(con.BeginTransaction)
        End Function

    End Class

    ' トランザクション
    Public Class NpgsqlTransaction
        Private tran As OleDbTransaction

        Public Sub New(ByVal _tran As OleDbTransaction)
            tran = _tran
        End Sub

        Public Sub Commit()
            tran.Commit()
        End Sub

        Public Sub Rollback()
            tran.Rollback()
        End Sub

        Public Function GetObject() As OleDbTransaction
            Return tran
        End Function
    End Class

    ' コマンド
    Public Class NpgsqlCommand
        Private cmd As OleDbCommand
        Private da As OleDbDataAdapter
        Private strExecSql As String
        Private tran As NpgsqlTransaction
        Public Parameters As New NpgsqlParameterCollection
        Public CommandType As CommandType

        ' コンストラクタ
        Public Sub New(ByVal strSql As String, ByRef con As NpgsqlConnection)
            cmd = con.CreateCommand
            da = New OleDbDataAdapter(cmd)
            tran = Nothing
            strExecSql = strSql
            Parameters.Clear()
        End Sub

        ' コンストラクタ
        Public Sub New(ByVal strSql As String, ByRef con As NpgsqlConnection, ByRef _tran As NpgsqlTransaction)
            cmd = con.CreateCommand
            da = New OleDbDataAdapter(cmd)
            tran = _tran
            cmd.Transaction = _tran.GetObject
            strExecSql = strSql
            Parameters.Clear()
        End Sub

        ' SQL設定
        Public Sub SetSql(ByVal strSql As String)
            strExecSql = strSql
            cmd.CommandText = strSql
        End Sub

        ' SQL実行（実行結果の先頭行先頭列を返却）
        Public Function ExecuteScalar() As Object
            Dim result As Object
            Try
                makeSql()
                cmd.CommandText = strExecSql
                result = cmd.ExecuteScalar
            Finally
            End Try
            Return result
        End Function

        ' SQL実行（データテーブルを返却）
        Public Function ExecuteReader() As NpgsqlDataReader
            Dim table As New DataTable
            Try
                makeSql()
                cmd.CommandText = strExecSql
                da.Fill(table)
            Finally
            End Try
            Return New NpgsqlDataReader(table)
        End Function

        ' SQL実行（データ返却無し、セミコロン";"区切りのSQLの連続実行対応）
        Public Function ExecuteNonQuery() As Integer
            Dim result As Integer
            Try
                makeSql()
                Dim strSqls As String() = divideSql(strExecSql)

                If strSqls.Length > 1 Then
                    Try
                        For Each strSql As String In strSqls
                            If strSql.Trim.Length > 0 Then
                                cmd.CommandText = strSql.Trim
                                result = cmd.ExecuteNonQuery
                            End If
                        Next
                    Catch ex As Exception
                    End Try
                Else
                    cmd.CommandText = strExecSql
                    result = cmd.ExecuteNonQuery
                End If
            Finally
            End Try
            Return result
        End Function

        ' SQL生成（:Keyを実データで置換し、実行SQLを作る）
        Private Sub makeSql()
            ' :keyパラメータを文字列長順にソート
            sortParam()

            ' :keyパラメータ置換処理
            Try
                For Each param As NpgsqlParameter In Parameters
                    If param.intColmunType = DbType.String Or param.intColmunType = DbType.Date Then
                        strExecSql = strExecSql.Replace(":" & param.strColmunName, "'" & param.objValue & "'")
                    Else
                        strExecSql = strExecSql.Replace(":" & param.strColmunName, param.objValue)
                    End If
                Next
            Catch ex As Exception
                MsgBox(ex)
            End Try

            ' PostgreSQL用コマンドの置換(取敢えずTO_CHARのみ、TO_DATE、TO_NUMBER等も必要)
            strExecSql = strExecSql.Replace("CInt(", "CONVERT(int,")
            strExecSql = strExecSql.Replace("CLng(", "CONVERT(int,")
            strExecSql = strExecSql.Replace("CStr(", "CONVERT(nvarchar,")
            strExecSql = strExecSql.Replace("Mid(", "SUBSTRING(")
            strExecSql = strExecSql.Replace("TO_CHAR", "FORMAT")
            strExecSql = strExecSql.Replace("&", "+")
            If MDSystemInfo.SQLType = 0 Then
                strExecSql = strExecSql.Replace("SUBSTRING", "MID")
            End If
        End Sub

        ' SQL文字列をセミコロンで分割
        Public Function divideSql(ByVal strSql As String) As String()
            Return Split(strSql, ";", -1, CompareMethod.Text)
        End Function

        ' :keyパラメータを文字列長順にソート
        Private Sub sortParam()
            If Parameters.Count = 1 Then
                Return
            End If

            Dim work As NpgsqlParameter

            For i As Integer = 0 To Parameters.Count - 1
                For j As Integer = i + 1 To Parameters.Count - 1
                    If Parameters.Item(i).strColmunName.Length < Parameters.Item(j).strColmunName.Length Then
                        work = Parameters.Item(i)
                        Parameters.Item(i) = Parameters.Item(j)
                        Parameters.Item(j) = work
                    End If
                Next
            Next
        End Sub

    End Class

    ' コマンドパラメータ
    Public Class NpgsqlParameter
        Public strColmunName As String
        Public intColmunType As DbType
        Public objValue As Object

        Public Property Value()
            Get
                Return intColmunType
            End Get
            Set(ByVal value)
                objValue = value
            End Set
        End Property

        Public Sub New(ByVal strName As String, ByVal iType As DbType)
            strColmunName = strName
            intColmunType = iType
        End Sub

    End Class

    ' コマンドパラメータコレクション
    Public Class NpgsqlParameterCollection
        Inherits CollectionBase

        Public Property Item(ByVal index As Integer) As NpgsqlParameter
            Get
                Return CType(List.Item(index), NpgsqlParameter)
            End Get
            Set(ByVal value As NpgsqlParameter)
                List.Item(index) = value
            End Set
        End Property

        Public Property Item(ByVal key As String) As NpgsqlParameter
            Get
                For Each elem As NpgsqlParameter In List
                    If elem.strColmunName = key Then
                        Return CType(elem, NpgsqlParameter)
                    End If
                Next
                Return Nothing
            End Get
            Set(ByVal value As NpgsqlParameter)
                For Each elem As NpgsqlParameter In List
                    If elem.strColmunName = key Then
                        elem = value
                    End If
                Next
            End Set
        End Property

        Public Sub Add(ByVal clsParam As NpgsqlParameter)
            List.Add(clsParam)
        End Sub

        Public Sub Add(ByVal strKey As String, ByVal strValue As String)
            List.Add(New NpgsqlParameter(strKey, strValue))
        End Sub

        Public Function Contains(ByVal strKey As String) As Boolean
            Return List.Contains(strKey)
        End Function
    End Class

    ' 例外
    Public Class NpgsqlException
        Inherits Exception
    End Class

    ' データテーブルを保持するクラス
    Public Class NpgsqlDataReader
        Implements IEnumerable
        Private table As DataTable
        Public Item As New NpgsqlItem
        Public ReadOnly FieldCount As Integer

        Public Sub New(ByVal tbl As DataTable)
            table = tbl
        End Sub

        Public Function HasRows() As Boolean
            Return table.Rows.Count > 0
        End Function

        Public Function GetDataTypeName(ByVal index As Integer)
            Return table.Columns(index).DataType
        End Function

        Public Function GetName(ByVal index As Integer) As String
            Return table.Columns(index).ColumnName
        End Function

        Public Function Read() As Boolean
            Return False
        End Function

        Public Function getTable() As DataTable
            Return table
        End Function

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            For Each row In table.Rows
                Item.add(row)
            Next
            Return Item
        End Function

        Public Sub Close()

        End Sub

    End Class

    Public Class NpgsqlItem
        Inherits ArrayList
        Implements IEnumerator
        Private index As Integer = -1

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            index = index + 1
            Return (index < MyBase.Count)
        End Function

        Public ReadOnly Property Current() As Object Implements IEnumerator.Current
            Get
                Return Item(index)
            End Get
        End Property

        Public Sub Reset() Implements IEnumerator.Reset
            index = index - 1
        End Sub

        'Public Overrides Function add(ByVal elem As Object) As Integer
        '    index = Count
        '    Return MyBase.Add(elem)
        'End Function
    End Class

End Namespace
