Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Data
Imports System.Text

Namespace Framework.Mapping
    Public MustInherit Class EntityMap
        ' Methods
        Private Sub New()
            Me.lstColumnList = New SortedList
        End Sub

        Public Sub New(ByVal mapInfo As ColumnMap())
            Me.lstColumnList = New SortedList
            Dim i As Integer
            For i = 0 To mapInfo.Length - 1
                Me.lstColumnList.Add(i, mapInfo(i))
            Next i
        End Sub

        Public Function CreateDataTableLogiName(ByVal strDataTableName As String) As DataTable
            Dim table As New DataTable(strDataTableName)
            table.Clear
            table.Columns.AddRange(Me.GetLogicalColumns)
            Return table
        End Function

        Public Function CreateDataTablePhysName(ByVal strDataTableName As String) As DataTable
            Dim table As New DataTable(strDataTableName)
            table.Clear
            table.Columns.AddRange(Me.GetPhysicalColumns)
            Return table
        End Function

        Public Function GetColumnInfo(ByVal index As Integer) As ColumnMap
            Return DirectCast(Me.lstColumnList.GetByIndex(index), ColumnMap)
        End Function

        Public Function GetDbDataType() As DbType()
            Dim list As New List(Of DbType)
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                list.Add(Me.GetDbDataType(i))
            Next i
            Return list.ToArray
        End Function

        Public Function GetDbDataType(ByVal index As Integer) As DbType
            Return Me.GetColumnInfo(index).DbDataType
        End Function

        Public Function GetDbDataTypeByLogical(ByVal key As String) As DbType
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                Dim columnInfo As ColumnMap = Me.GetColumnInfo(i)
                If columnInfo.LogicalName.Equals(key) Then
                    Return columnInfo.DbDataType
                End If
            Next i
            Return DbType.String
        End Function

        Public Function GetDbDataTypeByPhysical(ByVal key As String) As DbType
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                Dim columnInfo As ColumnMap = Me.GetColumnInfo(i)
                If columnInfo.PhysicalName.Equals(key) Then
                    Return columnInfo.DbDataType
                End If
            Next i
            Return DbType.String
        End Function

        Public Function GetLogicalColumns() As DataColumn()
            Dim columnArray As DataColumn() = New DataColumn(Me.ColumnCount  - 1) {}
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                columnArray(i) = New DataColumn(Me.GetLogicalName(i), Me.GetSystemDataType(i))
            Next i
            Return columnArray
        End Function

        Public Function GetLogicalIndex(ByVal key As String) As Integer
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                If Me.GetColumnInfo(i).LogicalName.Equals(key) Then
                    Return i
                End If
            Next i
            Return -1
        End Function

        Public Function GetLogicalName() As String()
            Dim list As New List(Of String)
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                list.Add(Me.GetLogicalName(i))
            Next i
            Return list.ToArray
        End Function

        Public Function GetLogicalName(ByVal index As Integer) As String
            Return DirectCast(Me.lstColumnList.GetByIndex(index), ColumnMap).LogicalName
        End Function

        Public Function GetLogicalName(ByVal key As String) As String
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                Dim columnInfo As ColumnMap = Me.GetColumnInfo(i)
                If columnInfo.PhysicalName.Equals(key) Then
                    Return columnInfo.LogicalName
                End If
            Next i
            Return ""
        End Function

        Public Function GetPhysicalColumns() As DataColumn()
            Dim columnArray As DataColumn() = New DataColumn(Me.ColumnCount  - 1) {}
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                columnArray(i) = New DataColumn(Me.GetPhysicalName(i), Me.GetSystemDataType(i))
            Next i
            Return columnArray
        End Function

        Public Function GetPhysicalIndex(ByVal key As String) As Integer
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                If Me.GetColumnInfo(i).PhysicalName.Equals(key) Then
                    Return i
                End If
            Next i
            Return -1
        End Function

        Public Function GetPhysicalName() As String()
            Dim list As New List(Of String)
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                list.Add(Me.GetPhysicalName(i))
            Next i
            Return list.ToArray
        End Function

        Public Function GetPhysicalName(ByVal index As Integer) As String
            Return DirectCast(Me.lstColumnList.GetByIndex(index), ColumnMap).PhysicalName
        End Function

        Public Function GetPhysicalName(ByVal key As String) As String
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                Dim columnInfo As ColumnMap = Me.GetColumnInfo(i)
                If columnInfo.LogicalName.Equals(key) Then
                    Return columnInfo.PhysicalName
                End If
            Next i
            Return ""
        End Function

        Public Function GetSystemDataType() As Type()
            Dim list As New List(Of Type)
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                list.Add(Me.GetSystemDataType(i))
            Next i
            Return list.ToArray
        End Function

        Public Function GetSystemDataType(ByVal index As Integer) As Type
            Return Me.GetColumnInfo(index).SystemDataType
        End Function

        Public Function GetSystemDataTypeByLogical(ByVal key As String) As Type
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                Dim columnInfo As ColumnMap = Me.GetColumnInfo(i)
                If columnInfo.LogicalName.Equals(key) Then
                    Return columnInfo.SystemDataType
                End If
            Next i
            Return Nothing
        End Function

        Public Function GetSystemDataTypeByPhysical(ByVal key As String) As Type
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                Dim columnInfo As ColumnMap = Me.GetColumnInfo(i)
                If columnInfo.PhysicalName.Equals(key) Then
                    Return columnInfo.SystemDataType
                End If
            Next i
            Return Nothing
        End Function

        Public Sub PhisycalToLogical(ByVal src As DataRow, ByRef dest As DataRow)
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                dest.Item(Me.GetLogicalName(i)) = src.Item(Me.GetPhysicalName(i))
            Next i
        End Sub

        Public Function ToLogicalString() As String
            Dim builder As New StringBuilder
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                Dim logicalName As String = Me.GetLogicalName(i)
                builder.Append(If((builder.Length <> 0), ("," & logicalName), logicalName))
            Next i
            Return builder.ToString
        End Function

        Public Function ToPhysicalString(ByVal strAdd As String) As String
            Dim builder As New StringBuilder
            Dim i As Integer
            For i = 0 To Me.ColumnCount - 1
                Dim str As String = (strAdd & Me.GetPhysicalName(i))
                builder.Append(If((builder.Length <> 0), ("," & str), str))
            Next i
            Return builder.ToString
        End Function


        ' Properties
        Public ReadOnly Property ColumnCount As Integer
            Get
                Return Me.lstColumnList.Count
            End Get
        End Property


        ' Fields
        Private lstColumnList As SortedList
    End Class
End Namespace
