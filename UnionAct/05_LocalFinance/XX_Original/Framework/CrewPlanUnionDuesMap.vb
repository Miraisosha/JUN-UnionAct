Imports System

Namespace Framework.Mapping
    Public Class CrewPlanUnionDuesMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CrewPlanUnionDuesMap.mapCrewPlanUnionDues)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCrewPlanUnionDues As ColumnMap() = New ColumnMap() { _
            New ColumnMap("s_new_staff_union_dues", "新入組合員組合費", GetType(Integer)), _
            New ColumnMap("s_entry_money", "加入金", GetType(Integer)), _
            New ColumnMap("s_new_cap_union_dues", "CAP" & "昇格組合費", GetType(Integer)), _
            New ColumnMap("s_cap_retire_union_dues", "CAP" & "退職組合費", GetType(Integer)), _
            New ColumnMap("s_cop_retire_union_dues", "COP" & "退職組合費", GetType(Integer)), _
            New ColumnMap("s_fe_retire_union_dues", "FE" & "退職組合費", GetType(Integer)), _
            New ColumnMap("s_unpromotion_union_dues", "非組合員組合費", GetType(Integer)), _
            New ColumnMap("s_senior_retire_union_dues", "シニア退職組合費", GetType(Integer)) _
        }

    End Class
End Namespace
