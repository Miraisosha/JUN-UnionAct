Imports System

Namespace Framework.Mapping
    Public Class WageReductionMonthlyListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WageReductionMonthlyListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("branch", "Š‘®", GetType(String)), _
            New ColumnMap("count", "‘ÎÛl”", GetType(Integer)), _
            New ColumnMap("cut_sum", "TœŠzŒv", GetType(Long)), _
            New ColumnMap("cover_sum", "•â“UŠzŒv", GetType(Long)), _
            New ColumnMap("dues_sum", "ØÌŠzŒv", GetType(Integer)) _
        }

    End Class
End Namespace
