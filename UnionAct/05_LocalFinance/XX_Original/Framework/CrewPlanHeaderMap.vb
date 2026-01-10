Imports System

Namespace Framework.Mapping
    Public Class CrewPlanHeaderMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CrewPlanHeaderMap.mapCrewPlanHeader)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCrewPlanHeader As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_period", "期", GetType(String)), _
            New ColumnMap("s_unpromotion_persons", "非組合員発生人数", GetType(Integer)), _
            New ColumnMap("s_unpromotion_rate", "非組合員発生倍率", GetType(Double)), _
            New ColumnMap("s_new_staff_average", "新入組合員基準年齢", GetType(Integer)), _
            New ColumnMap("s_cap_promotion_average", "CAP" & "昇格基準年齢", GetType(Integer)), _
            New ColumnMap("s_unpromotion_average", "非組合員発生年齢", GetType(Integer)), _
            New ColumnMap("s_senior_stay_rate", "シニア組合員残存率", GetType(Double)), _
            New ColumnMap("l_unpromotion_rate", "非組合員発生倍率", GetType(String)), _
            New ColumnMap("l_senior_stay_rate", "シニア組合員残存率", GetType(String)), _
            New ColumnMap("l_yearmonth_1", "年月" & "(1" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_2", "年月" & "(2" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_3", "年月" & "(3" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_4", "年月" & "(4" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_5", "年月" & "(5" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_6", "年月" & "(6" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_7", "年月" & "(7" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_8", "年月" & "(8" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_9", "年月" & "(9" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_10", "年月" & "(10" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_11", "年月" & "(11" & "列目）", GetType(String)), _
            New ColumnMap("l_yearmonth_12", "年月" & "(12" & "列目）", GetType(String)) _
        }

    End Class
End Namespace
