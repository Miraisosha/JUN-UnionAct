Imports System

Namespace Framework.Mapping
    Public Class WageReductionMonthlyDetailMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WageReductionMonthlyDetailMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("employee_number", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("digit", "CD", GetType(String)), _
            New ColumnMap("name", "–¼‘O", GetType(String)), _
            New ColumnMap("staf_kind", "‘g‡ˆõí•Ê", GetType(String)), _
            New ColumnMap("c_branch", "‰ïĞŠ‘®", GetType(String)), _
            New ColumnMap("u_branch", "‘g‡x•”", GetType(String)), _
            New ColumnMap("license", "‘Ši", GetType(String)), _
            New ColumnMap("model", "‹@í", GetType(String)), _
            New ColumnMap("cut_price", "TœŠz", GetType(Long)), _
            New ColumnMap("user_id", "ƒ†[ƒU" & "ID", GetType(String)) _
        }

    End Class
End Namespace
