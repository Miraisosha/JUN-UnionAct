Imports System

Namespace Framework.Mapping
    Public Class FullTimeSalaryPersonalBillMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(FullTimeSalaryPersonalBillMap.mapFullTimeSalaryPersonalBill)
        End Sub

        ' Fields
        Private Shared ReadOnly mapFullTimeSalaryPersonalBill As ColumnMap() = New ColumnMap() { _
            New ColumnMap("s_salary_a", "–{‹‹‚`‹àŠz", GetType(Integer)), _
            New ColumnMap("s_salary_b", "–{‹‹‚a‹àŠz", GetType(Integer)), _
            New ColumnMap("s_salary", "“™‹‰", GetType(Integer)), _
            New ColumnMap("s_rank", "†•î", GetType(Integer)), _
            New ColumnMap("s_work", "˜J“­“ú”", GetType(Integer)), _
            New ColumnMap("s_holiday_balance", "—L‹‹c“ú”", GetType(Integer)), _
            New ColumnMap("s_carry_over_holiday_balance", "—L‹‹—‚”NŒJ‰z“ú”", GetType(Integer)), _
            New ColumnMap("d_overtime_work_time", "ŠÔŠOŠÔ", GetType(String)), _
            New ColumnMap("d_holiday_work_time", "‹x“ú˜J“­ŠÔ", GetType(String)), _
            New ColumnMap("d_midnight_work_time", "[–é˜J“­ŠÔ", GetType(String)), _
            New ColumnMap("d_old_overtime_work_time", "‹Œê]EˆõŠÔŠOŠÔ", GetType(String)), _
            New ColumnMap("s_overtime_work_time", "ê]EˆõŠÔŠOŠÔ‹àŠz", GetType(Integer)), _
            New ColumnMap("s_tax_year_total", "”NŠÔ•¥ÅŠz", GetType(Integer)), _
            New ColumnMap("s_calculate_tax", "”NŠÔ”NÅŠziŒvZãj", GetType(Integer)), _
            New ColumnMap("s_year_adjust", "”N––’²®Šz", GetType(Integer)), _
            New ColumnMap("d_up", "XV“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "XVÒŒÂl" & "ID", GetType(String)), _
            New ColumnMap("s_up", "XV‰ñ”", GetType(Integer)) _
        }

    End Class
End Namespace
