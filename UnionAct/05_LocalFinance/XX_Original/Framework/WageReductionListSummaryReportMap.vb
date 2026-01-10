Imports System

Namespace Framework.Mapping
    Public Class WageReductionListSummaryReportMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WageReductionListSummaryReportMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("s_time_cut_total", "時間内カット", GetType(Long)), _
            New ColumnMap("s_time_cut_cover_total", "時間内カット補填", GetType(Long)), _
            New ColumnMap("s_time_union_dues", "時間内組合費", GetType(Integer)), _
            New ColumnMap("s_strike_cut_total", "争議行為カット", GetType(Long)), _
            New ColumnMap("s_strike_cut_cover_total", "争議行為カット補填", GetType(Long)), _
            New ColumnMap("s_strike_union_dues", "争議行為切捨額計", GetType(Integer)) _
        }

    End Class
End Namespace
