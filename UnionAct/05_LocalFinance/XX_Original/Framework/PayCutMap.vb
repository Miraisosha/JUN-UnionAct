Imports System

Namespace Framework.Mapping
    Public Class PayCutMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(PayCutMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "個人認証ＩＤ", GetType(String)), _
            New ColumnMap("d_years", "対象年月", GetType(String)), _
            New ColumnMap("s_pay_cut", "賃金内カット金額", GetType(Long)), _
            New ColumnMap("c_pay_once_name", "一時金名称", GetType(String)), _
            New ColumnMap("s_pay_cut_sum", "カット金額合計", GetType(Long)) _
        }

    End Class
End Namespace
