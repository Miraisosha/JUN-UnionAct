Imports System

Namespace Framework.Mapping
    Public Class ExecutiveCloseDtlPrintMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(ExecutiveCloseDtlPrintMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "個人認証ＩＤ", GetType(String)), _
            New ColumnMap("l_name", "名前", GetType(String)), _
            New ColumnMap("c_staf_id", "社員番号", GetType(String)), _
            New ColumnMap("k_model", "機種", GetType(String)), _
            New ColumnMap("k_belonging", "組合支部", GetType(String)), _
            New ColumnMap("l_explain", "説明", GetType(String)), _
            New ColumnMap("s_pay", "単価", GetType(Integer)), _
            New ColumnMap("s_day", "対象日", GetType(String)), _
            New ColumnMap("s_day_total", "日数", GetType(Integer)), _
            New ColumnMap("s_pay_day", "日当", GetType(Integer)) _
        }

    End Class
End Namespace
