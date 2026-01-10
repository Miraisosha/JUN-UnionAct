Imports System

Namespace Framework.Mapping
    Public Class RevenueExpenditurePromotionDtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(RevenueExpenditurePromotionDtlMap.mapRevenueExpenditurePromotionDtlMAP)
        End Sub

        ' Fields
        Private Shared ReadOnly mapRevenueExpenditurePromotionDtlMAP As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_revenue_str", "開始年月日", GetType(String)), _
            New ColumnMap("d_target", "対象年月", GetType(String)), _
            New ColumnMap("s_new_staff_member", "新入組合員昇格予定人数", GetType(Integer)), _
            New ColumnMap("s_new_cap_member", "CAP" & "昇格予定人数", GetType(Integer)), _
            New ColumnMap("s_cap_retire_member", "CAP" & "退職予定人数", GetType(Integer)), _
            New ColumnMap("s_cop_retire_member", "COP" & "退職予定人数", GetType(Integer)), _
            New ColumnMap("s_fe_retire_member", "FE" & "退職予定人数", GetType(Integer)), _
            New ColumnMap("s_senior_retire_member", "シニア退職予定人数", GetType(Integer)), _
            New ColumnMap("s_reserve_1", "予備人数" & "1", GetType(Integer)), _
            New ColumnMap("s_reserve_2", "予備人数" & "2", GetType(Integer)), _
            New ColumnMap("s_reserve_3", "予備人数" & "3", GetType(Integer)), _
            New ColumnMap("l_biko", "備考", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人" & "ID", GetType(String)), _
            New ColumnMap("d_up", "更新日", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "更新者個人" & "ID", GetType(String)), _
            New ColumnMap("s_up", "更新回数", GetType(Integer)) _
        }

    End Class
End Namespace
