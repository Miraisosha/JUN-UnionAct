Imports System

Namespace Framework.Mapping
    Public Class WageReductionBonusListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WageReductionBonusListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("branch", "Š‘®", GetType(String)), _
            New ColumnMap("count", "‘ÎÛl”", GetType(Long)), _
            New ColumnMap("intime", "ŠÔ“à", GetType(Long)), _
            New ColumnMap("strike", "‘ˆ‹csˆ×", GetType(Long)), _
            New ColumnMap("cut_sum", "TœŠzŒv", GetType(Long)), _
            New ColumnMap("cover", "•â“UŠzŒv", GetType(Long)), _
            New ColumnMap("dues", "ØÌŠzŒv", GetType(Long)) _
        }

    End Class
End Namespace
