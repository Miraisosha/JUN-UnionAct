Imports System

Namespace Framework.Mapping
    Public Class WithholdingMonthlyTaxableReportListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingMonthlyTaxableReportListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("l_name", "–¼‘O", GetType(String)), _
            New ColumnMap("k_belonging", "x•”", GetType(String)), _
            New ColumnMap("k_qualification", "‘Ši", GetType(String)), _
            New ColumnMap("s_officer_pay", "–ğˆõè“–", GetType(Long)), _
            New ColumnMap("s_time_cut_monthly", "ŠÔ“àŒ—áTœŠz", GetType(Long)), _
            New ColumnMap("s_time_cut_once", "ŠÔ“àˆê‹àTœŠz", GetType(Long)), _
            New ColumnMap("s_strike_cut_monthly", "‘ˆ‹csˆ×Œ—áTœŠz", GetType(Long)), _
            New ColumnMap("s_strike_cut_once", "‘ˆ‹csˆ×ˆê‹àTœŠz", GetType(Long)), _
            New ColumnMap("time_break", "ŠÔ“àØÌ‚ÄŠz", GetType(Long)), _
            New ColumnMap("strike_break", "‘ˆ‹csˆ×ØÌ‚ÄŠz", GetType(Long)), _
            New ColumnMap("allowance", "Œ¹ò’¥ûŠz", GetType(Long)), _
            New ColumnMap("s_break_total", "ØÌ‚ÄŠz", GetType(Long)), _
            New ColumnMap("c_taxation_flag", "‰ÛÅƒtƒ‰ƒO", GetType(String)) _
        }

    End Class
End Namespace
