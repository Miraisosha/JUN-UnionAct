Imports System
Imports System.Data
Imports System.Reflection
Imports UnionAct.Framework.UnionException

Namespace Framework.Command
    Public MustInherit Class MasterBase
        ' Methods
        Protected Sub New()
        End Sub

        Private Sub CheckMethodChoice(ByVal pkFlg As Boolean)
            Try
                If pkFlg Then
                    If (Not Me.strPKColumnName2 Is Nothing) Then
                        Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0001", New String(0 - 1) {})
                    End If
                ElseIf (Me.strPKColumnName2 Is Nothing) Then
                    Throw New SysUnionException(MethodBase.GetCurrentMethod, New Exception, "GE0001", New String(0 - 1) {})
                End If
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
        End Sub

        Protected Function GetConvertedData(ByVal strFilter As String, ByVal returnColumn As String) As String
            Dim str As String
            Try
                Dim rowArray As DataRow() = Me.dSetMaster.Tables.Item(Me.strTargetTableName).Select(strFilter)
                If (rowArray.Length <> 1) Then
                    Return ""
                End If
                str = rowArray(0).Item(returnColumn).ToString
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return str
        End Function

        Public Overridable Function GetListDataTable(ByVal strData As String, ByVal strSearchColumn As String) As DataTable
            Dim table2 As DataTable
            Try
                Dim table As DataTable = Me.dSetMaster.Tables.Item(Me.strTargetTableName).Clone
                table.Rows.Clear()
                Dim filterExpression As String = ""
                If (Not strData Is Nothing) Then
                    filterExpression = (strSearchColumn & " = '" & strData & "'")
                End If
                Dim rowArray As DataRow() = Me.dSetMaster.Tables.Item(Me.strTargetTableName).Select(filterExpression)
                Dim i As Integer
                For i = 0 To rowArray.Length - 1
                    table.ImportRow(rowArray(i))
                Next i
                table2 = table
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return table2
        End Function

        Public Function GetNameByCode(ByVal primaryKey As String) As String
            Dim convertedData As String
            Try
                Me.CheckMethodChoice(True)
                Dim strFilter As String = (Me.strPKColumnName1 & " = '" & primaryKey & "'")
                convertedData = Me.GetConvertedData(strFilter, Me.strWaColumnName)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return convertedData
        End Function

        Public Function GetNameByCode(ByVal headerKey As String, ByVal detailKey As String) As String
            Dim convertedData As String
            Try
                Me.CheckMethodChoice(False)
                Dim strFilter As String = String.Concat(New String() {"(", Me.strPKColumnName1, " = '", headerKey, "') and (", Me.strPKColumnName2, " = '", detailKey, "')"})
                convertedData = Me.GetConvertedData(strFilter, Me.strWaColumnName)
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return convertedData
        End Function

        Public Function GetWaColumnName() As String
            Dim strWaColumnName As String
            Try
                strWaColumnName = Me.strWaColumnName
            Catch exception As AppUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            Catch exception2 As SysUnionException
                exception2.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception2
            Catch exception3 As Exception
                Throw New SysUnionException(MethodBase.GetCurrentMethod, exception3, "GE0001", New String(0 - 1) {})
            End Try
            Return strWaColumnName
        End Function


        ' Fields
        Protected dSetMaster As DataSet
        Protected strCodeColumnName As String
        Protected strPKColumnName1 As String
        Protected strPKColumnName2 As String
        Protected strTargetTableName As String
        Protected strWaColumnName As String
    End Class
End Namespace
