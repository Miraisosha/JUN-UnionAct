Imports System

Namespace Framework.Mapping
    Public Class WithholdingMonthlyReportDetailMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingMonthlyReportDetailMap.map)
        End Sub

        ' 2016/09/08(–Ø) C³ ‰ÛÅƒtƒ‰ƒO’Ç‰Á Start
        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("l_name", "–¼‘O", GetType(String)), _
            New ColumnMap("k_model", "‹@í", GetType(String)), _
            New ColumnMap("k_qualification", "‘Ši", GetType(String)), _
            New ColumnMap("s_break", "ØÌ‚Ä’PˆÊŠz", GetType(String)), _
            New ColumnMap("l_month", "‘ÎÛŒ", GetType(String)), _
            New ColumnMap("s_pay_time_cut_monthly_break", "Œ—áŠÔ“à’À‹àTœØÌŠz", GetType(Long)), _
            New ColumnMap("s_pay_strike_cut_monthly_break", "Œ—á‘ˆ‹csˆ×’À‹àTœØÌŠz", GetType(Long)), _
            New ColumnMap("s_cut_monthly_taxation", "Œ—áŒ¹ò’¥ûŠz", GetType(Long)), _
            New ColumnMap("k_local", "x•”", GetType(String)), _
            New ColumnMap("s_pay_time_cut_monthly", "Œ—áŠÔ“à’À‹àTœ", GetType(Long)), _
            New ColumnMap("s_pay_strike_cut_monthly", "Œ—á‘ˆ‹csˆ×’À‹àTœ", GetType(Long)), _
            New ColumnMap("s_cut_once_taxation", "ˆê‹àŒ¹ò’¥ûŠz", GetType(Long)), _
            New ColumnMap("s_officer_pay", "–ğˆõè“–", GetType(Long)), _
            New ColumnMap("s_pay_time_cut_once_break", "ˆê‹àŠÔ“àTœØÌ‚ÄŠz", GetType(Long)), _
            New ColumnMap("s_pay_strike_cut_once_break", "ˆê‹à‘ˆ‹csˆ×TœØÌ‚ÄŠz", GetType(Long)), _
            New ColumnMap("s_pay_time_cut_once", "ˆê‹àŠÔ“àTœ", GetType(Long)), _
            New ColumnMap("s_pay_strike_cut_once", "ˆê‹à‘ˆ‹csˆ×Tœ", GetType(Long)), _
            New ColumnMap("c_taxation_flag", "‰ÛÅƒtƒ‰ƒO", GetType(String)) _
        }
        ' 2016/09/08(–Ø) C³ ‰ÛÅƒtƒ‰ƒO’Ç‰Á End

    End Class
End Namespace
