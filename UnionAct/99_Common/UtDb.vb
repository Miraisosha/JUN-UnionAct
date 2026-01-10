Imports UnionAct.NSMDInfo

Module UtDb
    ''' <summary>
    '''' 文字列への変換関数
    '''' </summary>
    '''' <param name="colName"></param>
    '''' <returns></returns>
    'Public Function DbCStr(ByVal colName) As String
    '    If MDSystemInfo.SQLType = 0 Then
    '        Return "CStr(" & colName & ")"
    '    Else
    '        Return "CONVERT(nvarchar," & colName & ")"
    '    End If
    'End Function
    '''' <summary>
    '''' 文字列への変換関数
    '''' </summary>
    '''' <param name="colName"></param>
    '''' <returns></returns>
    'Public Function DbCLng(ByVal colName) As String
    '    If MDSystemInfo.SQLType = 0 Then
    '        Return "CLng(" & colName & ")"
    '    Else
    '        Return "CONVERT(int," & colName & ")"
    '    End If
    'End Function

    '''' <summary>
    '''' MDBはカラム名に「.」を使えるがSQL Serverは使えない。
    '''' SQL Server化する時に「.」を「_」に変換したのでカラム名の調整
    '''' </summary>
    '''' <param name="colName"></param>
    '''' <returns></returns>
    'Public Function DbColName(ByVal colName) As String
    '    If MDSystemInfo.SQLType = 0 Then
    '        Return colName
    '    Else
    '        Return Replace(colName, ".", "_")
    '    End If
    'End Function

    ''' <summary>
    ''' SQL Serverでは、サブクエリ等でORDER BYの後に「OFFSET 0 ROWS」が必要
    ''' </summary>
    ''' <returns></returns>
    Public Function DbOrderOffset() As String
        If MDSystemInfo.SQLType = 0 Then
            Return ""
        Else
            Return " OFFSET 0 ROWS "
        End If
    End Function

    Public Function DbStrYYYYMMDDtoDateText(colName) As String
        Return "LEFT(" + colName + ",4)+'/'+SUBSTRING(" + colName + ",5,2)+'/'+RIGHT(" + colName + ",2)"
    End Function

    Public Function DbStrZipCode(colName) As String
        Return "LEFT(" + colName + ",3)+'-'+SUBSTRING(" + colName + ",4,4)"
    End Function
End Module
