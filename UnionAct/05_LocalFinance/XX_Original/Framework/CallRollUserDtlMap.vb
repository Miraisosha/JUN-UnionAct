Imports System

Namespace Framework.Mapping
    Public Class CallRollUserDtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CallRollUserDtlMap.mapCallRollUserDtl)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCallRollUserDtl As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "ŒÂl”FØ‚h‚c", GetType(String)), _
            New ColumnMap("d_years", "‘ÎÛ”NŒ", GetType(DateTime)), _
            New ColumnMap("s_day", "“ú•tî•ñ", GetType(DateTime)), _
            New ColumnMap("c_committee_id", "ˆÏˆõ‰ï‚h‚c", GetType(String)), _
            New ColumnMap("s_committee_seq", "ˆÏˆõ‰ï‚h‚c}”Ô", GetType(Integer)), _
            New ColumnMap("c_daily_pay_id", "“ú“–‚h‚c", GetType(String)), _
            New ColumnMap("c_menu_seq", "“ú“–‚h‚c}”Ô", GetType(Integer)), _
            New ColumnMap("k_food_expenses", "’†·’‹H”ï‰Â”Û", GetType(String)), _
            New ColumnMap("d_up_close", "’÷‚ßXV“ú", GetType(DateTime)), _
            New ColumnMap("s_daily_pay", "’÷‚ß“ú“–‹àŠz", GetType(Integer)), _
            New ColumnMap("s_food_expenses", "’÷‚ß’†·’‹H”ï", GetType(Integer)), _
            New ColumnMap("s_next_balance_daily_pay", "Ÿ’÷‚ß·•ª“ú“–‹àŠz", GetType(Integer)), _
            New ColumnMap("s_next_balance_food_expenses", "Ÿ’÷‚ß·•ª’†·’‹H”ï", GetType(Integer)), _
            New ColumnMap("d_ins", "ì¬“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "ì¬ÒŒÂl‚h‚c", GetType(String)), _
            New ColumnMap("d_up", "XV“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "XVÒŒÂl‚h‚c", GetType(String)), _
            New ColumnMap("s_up", "XV‰ñ”", GetType(Integer)) _
        }

    End Class
End Namespace
