Imports System

Namespace Framework.Mapping
    Public Class WageReductionListBonusReportMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WageReductionListBonusReportMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_staf_id", "社員番号", GetType(String)), _
            New ColumnMap("l_name", "氏名", GetType(String)), _
            New ColumnMap("k_local", "所属", GetType(String)), _
            New ColumnMap("s_time_cut", "時間内カット", GetType(Long)), _
            New ColumnMap("s_strike_cut", "争議行為カット", GetType(Long)) _
        }

    End Class
End Namespace
