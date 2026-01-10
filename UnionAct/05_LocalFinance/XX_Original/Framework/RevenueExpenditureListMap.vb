Imports System

Namespace Framework.Mapping
    Public Class RevenueExpenditureListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(RevenueExpenditureListMap.mapRevenueExpenditureList)
        End Sub

        ' Fields
        Private Shared ReadOnly mapRevenueExpenditureList As ColumnMap() = New ColumnMap() { _
            New ColumnMap("‘è–Ú", "‘è–Ú", GetType(String)), _
            New ColumnMap("—\‘zŠJn“ú", "—\‘zŠJn”NŒ“ú", GetType(String)), _
            New ColumnMap("—\‘zI—¹“ú", "—\‘zI—¹”NŒ“ú", GetType(String)), _
            New ColumnMap("û“ü—\‘zó‹µ", "û“ü—\‘zó‹µ", GetType(String)), _
            New ColumnMap("æˆõŒv‰æó‹µ", "æˆõŒv‰æˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("•ª’S‹àó‹µ", "•ª’S‹àˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("—\Z“o˜^ó‹µ", "—\Z“o˜^ˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("C³—\Zó‹µ", "—\ZC³ˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("’S“–Ò", "’S“–Ò", GetType(String)), _
            New ColumnMap("“o˜^“ú", "“o˜^”NŒ“ú", GetType(String)), _
            New ColumnMap("d_revenue_str", "ŠJn”NŒ“ú", GetType(String)), _
            New ColumnMap("d_revenue_end", "I—¹”NŒ“ú", GetType(String)), _
            New ColumnMap("l_title", "•\‘è", GetType(String)), _
            New ColumnMap("k_revenue_expenditure", "û“ü—\‘zˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("k_revenue_member", "æˆõŒv‰æˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("k_revenue_allotted_charge", "•ª’S‹àˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("k_revenue_budgetary_process", "—\Z“o˜^ˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("k_revenue_budgetary_revise", "—\ZC³ˆ—‰Â”Û", GetType(String)), _
            New ColumnMap("k_revenue_seton", "ûx—\‘zŠm’è‹æ•ª", GetType(String)), _
            New ColumnMap("s_revenue_expenditure_ttl", "û“ü—\‘z‡Œv‹àŠz", GetType(Long)), _
            New ColumnMap("s_revise_revenue_ttl", "•â³Œãû“ü‡Œv‹àŠz", GetType(Long)), _
            New ColumnMap("s_general_account_molecule", "ˆê”Ê‰ïŒv”ä—¦•ªq", GetType(Double)), _
            New ColumnMap("s_general_account_denominator", "ˆê”Ê‰ïŒv”ä—¦•ª•ê", GetType(Double)), _
            New ColumnMap("s_general_account", "ˆê”Ê‰ïŒv‹àŠz", GetType(Long)), _
            New ColumnMap("s_senior_monthwork", "ƒVƒjƒA‘g‡ˆõ‡ŒvlŒ", GetType(Integer)), _
            New ColumnMap("s_new_staff_average", "V“ü‘g‡ˆõŠî€”N—î", GetType(Integer)), _
            New ColumnMap("s_cap_promotion_average", "‹@’·¸ŠiŠî€”N—î", GetType(Integer)), _
            New ColumnMap("s_unpromotion_persons", "”ñ‘g‡ˆõ”­¶l”", GetType(Integer)), _
            New ColumnMap("s_unpromotion_rate", "”ñ‘g‡ˆõ”{—¦", GetType(Double)), _
            New ColumnMap("s_unpromotion_average", "”ñ‘g‡ˆõŠî€”N—î", GetType(Integer)), _
            New ColumnMap("s_senior_stay_rate", "ƒVƒjƒA‘g‡ˆõc‘¶—¦", GetType(Double)), _
            New ColumnMap("s_senior_average", "ƒVƒjƒA‘g‡ˆõŠî€”N—î", GetType(Integer)), _
            New ColumnMap("s_new_union_monthwork", "V“ü‘g‡ˆõ‡ŒvlŒ", GetType(Integer)), _
            New ColumnMap("s_cap_up_monthwork", "‹@’·¸Ši‡ŒvlŒ", GetType(Integer)), _
            New ColumnMap("s_cap_retire_monthwork", "‚b‚`‚o‘ŞEÒ‡ŒvlŒ", GetType(Integer)), _
            New ColumnMap("s_cop_retire_monthwork", "‚b‚n‚o‘ŞEÒ‡ŒvlŒ", GetType(Integer)), _
            New ColumnMap("s_fe_retire_monthwork", "‚e‚d‘ŞEÒ‡ŒvlŒ", GetType(Integer)), _
            New ColumnMap("s_unpromotion_persons_monthwork", "”ñ‘g‡ˆõ‡ŒvlŒ", GetType(Integer)), _
            New ColumnMap("s_senior_retire_monthwork", "ƒVƒjƒA‘ŞEÒ‡ŒvlŒ", GetType(Integer)), _
            New ColumnMap("s_budget_sub", "—\Z—\”õ”ï‹àŠz", GetType(Long)), _
            New ColumnMap("s_budget_total", "—\Z‡Œv", GetType(Long)), _
            New ColumnMap("s_revise_budget_sub", "C³—\Z—\”õ”ï‹àŠz", GetType(Long)), _
            New ColumnMap("s_revise_budget_total", "C³—\Z‡Œv", GetType(Long)), _
            New ColumnMap("l_biko_1", "”õl‚P", GetType(String)), _
            New ColumnMap("l_biko_2", "”õl‚Q", GetType(String)), _
            New ColumnMap("l_biko_3", "”õl‚R", GetType(String)), _
            New ColumnMap("d_ins", "ì¬“ú", GetType(DateTime)), _
            New ColumnMap("d_up", "XV“ú", GetType(DateTime)) _
        }

    End Class
End Namespace
