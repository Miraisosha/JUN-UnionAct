Imports System

Namespace Framework.Mapping
    Public Class RevenueAllottedChargeMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(RevenueAllottedChargeMap.mapAllotted)
        End Sub

        ' Fields
        Private Shared ReadOnly mapAllotted As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_revenue_str", "開始年月日", GetType(String)), _
            New ColumnMap("c_expend_item", "支出科目コード", GetType(String)), _
            New ColumnMap("c_expend_item_seq", "支出科目細目枝番", GetType(Integer)), _
            New ColumnMap("s_unit_price", "単価", GetType(Long)), _
            New ColumnMap("s_year_month", "年間課金月数", GetType(Integer)), _
            New ColumnMap("d_standard_date", "組合員数基準日", GetType(String)), _
            New ColumnMap("s_member", "組合員人数", GetType(Integer)), _
            New ColumnMap("s_total", "合計金額", GetType(Long)), _
            New ColumnMap("l_biko", "備考", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人" & "ID", GetType(String)), _
            New ColumnMap("d_up", "更新日", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "更新者個人" & "ID", GetType(String)), _
            New ColumnMap("s_up", "更新回数", GetType(Integer)) _
        }

    End Class
End Namespace
