Imports System

Namespace Framework.Mapping
    Public Class RevenueBudgetaryProcessMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(RevenueBudgetaryProcessMap.mapRBP)
        End Sub

        ' Fields
        Private Shared ReadOnly mapRBP As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_revenue_str", "開始年月日", GetType(String)), _
            New ColumnMap("k_budgetary_kind", "予算種別コード", GetType(String)), _
            New ColumnMap("k_budgetary_process", "予算分類", GetType(String)), _
            New ColumnMap("s_sub_sum", "小計", GetType(Long)), _
            New ColumnMap("l_biko", "備考", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人" & "ID", GetType(String)), _
            New ColumnMap("d_up", "更新日", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "更新者個人" & "ID", GetType(String)), _
            New ColumnMap("s_up", "更新回数", GetType(Integer)) _
        }

    End Class
End Namespace
