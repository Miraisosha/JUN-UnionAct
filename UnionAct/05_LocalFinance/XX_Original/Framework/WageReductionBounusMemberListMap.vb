Imports System

Namespace Framework.Mapping
    Public Class WageReductionBounusMemberListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WageReductionBounusMemberListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "Ğˆõ”Ô†", GetType(String)), _
            New ColumnMap("digit", "CD", GetType(String)), _
            New ColumnMap("name", "–¼‘O", GetType(String)), _
            New ColumnMap("staf_kind", "‘g‡ˆõí•Ê", GetType(String)), _
            New ColumnMap("c_branch", "‰ïĞŠ‘®", GetType(String)), _
            New ColumnMap("u_branch", "‘g‡x•”", GetType(String)), _
            New ColumnMap("license", "‘Ši", GetType(String)), _
            New ColumnMap("bonusname", "ˆê‹à–¼Ì", GetType(String)), _
            New ColumnMap("i_cut_price", "ŠÔ“àTœŠz", GetType(Long)), _
            New ColumnMap("s_cut_price", "‘ˆ‹csˆ×TœŠz", GetType(Long)), _
            New ColumnMap("cut_price_sum", "TœŠzŒv", GetType(Long)), _
            New ColumnMap("user_id", "ƒ†[ƒU" & "ID", GetType(String)) _
        }

    End Class
End Namespace
