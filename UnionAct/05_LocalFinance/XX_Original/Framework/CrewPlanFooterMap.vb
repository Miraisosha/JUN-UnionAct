Imports System

Namespace Framework.Mapping
    Public Class CrewPlanFooterMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CrewPlanFooterMap.mapCrewPlanFooter)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCrewPlanFooter As ColumnMap() = New ColumnMap() { _
            New ColumnMap("s_revenue_expenditure_ttl", "‘Šz", GetType(Decimal)), _
            New ColumnMap("s_revise_new_staff", "V“ü‘g‡ˆõC³", GetType(Decimal)), _
            New ColumnMap("s_new_staff_money", "V“ü‘g‡ˆõ“ü‰ï‹à", GetType(Decimal)), _
            New ColumnMap("s_revise_up", "¸ŠiC³", GetType(Decimal)), _
            New ColumnMap("s_revise_cap_retire", "‘ŞEC³" & "(CAP)", GetType(Decimal)), _
            New ColumnMap("s_revise_cop_retire", "‘ŞEC³" & "(COP)", GetType(Decimal)), _
            New ColumnMap("s_revise_fe_retire", "‘ŞEC³" & "(F/E)", GetType(Decimal)), _
            New ColumnMap("s_revise_senior_retire", "‘ŞEC³" & "(" & "ƒVƒjƒA" & ")", GetType(Decimal)), _
            New ColumnMap("s_revenue_unpromotion", "”ñ‘g‡”­¶—\‘z", GetType(Decimal)) _
        }

    End Class
End Namespace
