Imports System

Namespace Framework.Mapping
    Public Class WageReductionMemberMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WageReductionMemberMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("digit", "CD", GetType(String)), _
            New ColumnMap("name", "名前", GetType(String)), _
            New ColumnMap("c_branch", "会社所属", GetType(String)), _
            New ColumnMap("u_branch", "組合支部", GetType(String)), _
            New ColumnMap("model", "機種", GetType(String)), _
            New ColumnMap("license", "資格", GetType(String)), _
            New ColumnMap("user_id", "個人" & "ID", GetType(String)), _
            New ColumnMap("staf_kind", "組合員種別", GetType(String)) _
        }

    End Class
End Namespace
