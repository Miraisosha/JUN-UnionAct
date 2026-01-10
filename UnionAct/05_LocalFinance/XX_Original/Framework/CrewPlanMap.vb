Imports System

Namespace Framework.Mapping
    Public Class CrewPlanMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CrewPlanMap.mapCrewPlan)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCrewPlan As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_target", "‘ÎÛŒ", GetType(String)), _
            New ColumnMap("s_new_staff_member", "V“ü‘g‡ˆõ”", GetType(Integer)), _
            New ColumnMap("s_new_cap_member", "CAP" & "¸Ši”", GetType(Integer)), _
            New ColumnMap("s_cap_retire_member", "CAP" & "‘ŞEÒ”", GetType(Integer)), _
            New ColumnMap("s_cop_retire_member", "COP" & "‘ŞEÒ”", GetType(Integer)), _
            New ColumnMap("s_fe_retire_member", "FE" & "‘ŞEÒ”", GetType(Integer)), _
            New ColumnMap("s_senior_retire_member", "ƒVƒjƒA‘ŞEÒ”", GetType(Integer)) _
        }

    End Class
End Namespace
