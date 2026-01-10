Imports System

Namespace Framework.Mapping
    Public Class WithholdingMonthlyNonTaxableReportListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingMonthlyNonTaxableReportListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("l_name", "–¼‘O", GetType(String)), _
            New ColumnMap("k_belonging", "x•”", GetType(String)), _
            New ColumnMap("k_qualification", "‘Ši", GetType(String)), _
            New ColumnMap("s_monthly_cut", "Œ—áTœŠz", GetType(Long)), _
            New ColumnMap("s_once_cut", "ˆê‹àTœŠz", GetType(Long)), _
            New ColumnMap("s_break_total", "ØÌ‚ÄŠz", GetType(Long)) _
        }

    End Class
End Namespace
