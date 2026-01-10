Imports System

Namespace Framework.Mapping
    Public Class CrewPlanDetailMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CrewPlanDetailMap.mapCrewPlanDetail)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCrewPlanDetail As ColumnMap() = New ColumnMap() { _
            New ColumnMap("s_member_month_1", "対象人数" & "(1" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_2", "対象人数" & "(2" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_3", "対象人数" & "(3" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_4", "対象人数" & "(4" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_5", "対象人数" & "(5" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_6", "対象人数" & "(6" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_7", "対象人数" & "(7" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_8", "対象人数" & "(8" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_9", "対象人数" & "(9" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_10", "対象人数" & "(10" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_11", "対象人数" & "(11" & "列目" & ")", GetType(Integer)), _
            New ColumnMap("s_member_month_12", "対象人数" & "(12" & "列目" & ")", GetType(Integer)) _
        }

    End Class
End Namespace
