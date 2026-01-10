Imports System

Namespace Framework.Mapping
    Public Class FullTimeSalaryPersonalDtlBillMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(FullTimeSalaryPersonalDtlBillMap.mapFullTimeSalaryPersonalDtlBill)
        End Sub

        ' Fields
        Private Shared ReadOnly mapFullTimeSalaryPersonalDtlBill As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_salary_pay", "支払年月", GetType(String)), _
            New ColumnMap("c_user_id", "個人認証ＩＤ", GetType(String)), _
            New ColumnMap("c_salary_item", "給与科目細目コード", GetType(String)), _
            New ColumnMap("s_standard", "標準報酬金額", GetType(Integer)), _
            New ColumnMap("s_person", "本人金額", GetType(Integer)), _
            New ColumnMap("s_union", "組合金額", GetType(Integer)), _
            New ColumnMap("s_item", "合計金額", GetType(Integer)), _
            New ColumnMap("l_biko", "備考", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人ＩＤ", GetType(String)), _
            New ColumnMap("d_up", "更新日", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "更新者個人" & "ID", GetType(String)), _
            New ColumnMap("s_up", "更新回数", GetType(Integer)) _
        }

    End Class
End Namespace
