Imports System

Namespace Framework.Mapping
    Public Class WithholdingMonthlyNonTaxableSumListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingMonthlyNonTaxableSumListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_title", "x•”", GetType(String)), _
            New ColumnMap("cut_monthly", "Œ—á’À‹àTœŠz", GetType(Long)), _
            New ColumnMap("cut_once", "ˆê‹àTœŠz", GetType(Long)), _
            New ColumnMap("cut_total", "TœŠzŒv", GetType(Long)), _
            New ColumnMap("s_break_total", "ØÌ‚ÄŠz", GetType(Long)), _
            New ColumnMap("payout", "·ˆøx‹‹Šz", GetType(Long)) _
        }

    End Class
End Namespace
