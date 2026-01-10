Imports System

Namespace Framework.Mapping
    Public Class RevenueExpenditureMemmberDtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(RevenueExpenditureMemmberDtlMap.mapRevenueExpenditureMemmberDtl)
        End Sub

        ' Fields
        Private Shared ReadOnly mapRevenueExpenditureMemmberDtl As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_revenue_str", "開始年月日", GetType(String)), _
            New ColumnMap("k_qualification", "資格（乗務員）区分", GetType(String)), _
            New ColumnMap("s_real_number_ttl", "正組合員数合計", GetType(Integer)), _
            New ColumnMap("s_real_union_dues_ttl", "正組合費合計", GetType(Long)), _
            New ColumnMap("s_number_22", "22" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_22", "22" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_23", "23" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_23", "23" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_24", "24" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_24", "24" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_25", "25" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_25", "25" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_26", "26" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_26", "26" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_27", "27" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_27", "27" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_28", "28" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_28", "28" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_29", "29" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_29", "29" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_30", "30" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_30", "30" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_31", "31" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_31", "31" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_32", "32" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_32", "32" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_33", "33" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_33", "33" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_34", "34" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_34", "34" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_35", "35" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_35", "35" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_36", "36" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_36", "36" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_37", "37" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_37", "37" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_38", "38" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_38", "38" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_39", "39" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_39", "39" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_40", "40" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_40", "40" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_41", "41" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_41", "41" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_42", "42" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_42", "42" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_43", "43" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_43", "43" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_44", "44" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_44", "44" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_45", "45" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_45", "45" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_46", "46" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_46", "46" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_47", "47" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_47", "47" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_48", "48" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_48", "48" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_49", "49" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_49", "49" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_50", "50" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_50", "50" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_51", "51" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_51", "51" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_52", "52" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_52", "52" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_53", "53" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_53", "53" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_54", "54" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_54", "54" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_55", "55" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_55", "55" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_56", "56" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_56", "56" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_57", "57" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_57", "57" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_58", "58" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_58", "58" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_59", "59" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_59", "59" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_60", "60" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_60", "60" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_61", "61" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_61", "61" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_62", "62" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_62", "62" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_63", "63" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_63", "63" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_64", "64" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_64", "64" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_65", "65" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_65", "65" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_66", "66" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_66", "66" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_67", "67" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_67", "67" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_68", "68" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_68", "68" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_69", "69" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_69", "69" & "才資格組合費", GetType(Long)), _
            New ColumnMap("s_number_70", "70" & "才対象人数", GetType(Integer)), _
            New ColumnMap("s_union_dues_70", "70" & "才資格組合費", GetType(Long)), _
            New ColumnMap("l_biko", "備考", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人ＩＤ", GetType(String)), _
            New ColumnMap("d_up", "更新日", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "更新者個人ＩＤ", GetType(String)), _
            New ColumnMap("s_up", "更新回数", GetType(Integer)) _
        }

    End Class
End Namespace
