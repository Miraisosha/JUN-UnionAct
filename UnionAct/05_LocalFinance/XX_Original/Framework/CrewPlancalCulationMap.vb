Imports System

Namespace Framework.Mapping
    Public Class CrewPlancalCulationMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CrewPlancalCulationMap.mapPlancalCulation)
        End Sub

        ' Fields
        Private Shared ReadOnly mapPlancalCulation As ColumnMap() = New ColumnMap() { _
            New ColumnMap("s_new_staff_member_total", "新入組合員合計人数", GetType(Integer)), _
            New ColumnMap("s_new_cap_member_total", "CAP" & "昇格合計人数", GetType(Integer)), _
            New ColumnMap("s_cap_retire_member_total", "CAP" & "退職者合計人数", GetType(Integer)), _
            New ColumnMap("s_cop_retire_member_total", "COP" & "退職者合計人数", GetType(Integer)), _
            New ColumnMap("s_fe_retire_member_total", "FE" & "退職者合計人数", GetType(Integer)), _
            New ColumnMap("s_senior_retire_member_total", "シニア退職者合計人数", GetType(Integer)), _
            New ColumnMap("s_unpromotion_persons", "非組合員合計人数", GetType(Integer)), _
            New ColumnMap("s_new_union_monthwork", "新入組合員合計人月", GetType(Integer)), _
            New ColumnMap("s_cap_up_monthwork", "機長昇格合計人月", GetType(Integer)), _
            New ColumnMap("s_cap_retire_monthwork", "CAP" & "退職者合計人月", GetType(Integer)), _
            New ColumnMap("s_cop_retire_monthwork", "COP" & "退職者合計人月", GetType(Integer)), _
            New ColumnMap("s_fe_retire_monthwork", "FE" & "退職者合計人月", GetType(Integer)), _
            New ColumnMap("s_senior_retire_monthwork", "シニア退職者合計人月", GetType(Long)), _
            New ColumnMap("s_senior_monthwork_total", "シニア組合員合計人月", GetType(Integer)), _
            New ColumnMap("s_retire_monthwork_total", "退職者総人月", GetType(Integer)), _
            New ColumnMap("s_revise_retire_monthwork_total", "補正シニア組合員人月", GetType(Integer)), _
            New ColumnMap("s_unpromotion_persons_monthwork", "非組合発生合計人月", GetType(Integer)), _
            New ColumnMap("s_revise_new_staff", "新入組合員修正", GetType(Long)), _
            New ColumnMap("s_new_staff_money", "新入組合員入会金", GetType(Long)), _
            New ColumnMap("s_revise_up", "昇格修正", GetType(Long)), _
            New ColumnMap("s_revise_cap_retire", "退職修正" & "(CAP)", GetType(Long)), _
            New ColumnMap("s_revise_cop_retire", "退職修正" & "(COP)", GetType(Long)), _
            New ColumnMap("s_revise_fe_retire", "退職修正" & "(FE)", GetType(Long)), _
            New ColumnMap("s_revenue_unpromotion", "非組合発生予想", GetType(Long)), _
            New ColumnMap("s_revise_senior_retire", "退職修正" & "(" & "シニア" & ")", GetType(Long)), _
            New ColumnMap("s_senior_union_dues", "シニア組合費", GetType(Long)), _
            New ColumnMap("s_revise_revenue_ttl", "補正後収入額", GetType(Long)) _
        }

    End Class
End Namespace
