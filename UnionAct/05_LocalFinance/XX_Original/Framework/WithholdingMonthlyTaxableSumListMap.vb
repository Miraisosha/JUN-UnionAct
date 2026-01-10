Imports System

Namespace Framework.Mapping
    Public Class WithholdingMonthlyTaxableSumListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingMonthlyTaxableSumListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_title", "x•”", GetType(String)), _
            New ColumnMap("s_officer_pay", "–ğˆõè“–Šz", GetType(Long)), _
            New ColumnMap("cut_monthly", "Œ—á’À‹àTœŠz", GetType(Long)), _
            New ColumnMap("cut_once", "ˆê‹àTœŠz", GetType(Long)), _
            New ColumnMap("taxable", "‰ÛÅ‘ÎÛŠz", GetType(Long)), _
            New ColumnMap("s_break_total", "ØÌ‚ÄŠz", GetType(Long)), _
            New ColumnMap("allowance", "Œ¹ò’¥ûŠz", GetType(Long)), _
            New ColumnMap("defference", "·ˆøx‹‹Šz", GetType(Long)) _
        }

    End Class
End Namespace
