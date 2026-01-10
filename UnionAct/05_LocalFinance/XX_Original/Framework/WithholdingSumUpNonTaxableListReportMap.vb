Imports System

Namespace Framework.Mapping
    Public Class WithholdingSumUpNonTaxableListReportMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingSumUpNonTaxableListReportMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("l_name", "–¼", GetType(String)), _
            New ColumnMap("k_belonging", "x•”", GetType(String)), _
            New ColumnMap("k_qualification", "‘Ši", GetType(String)), _
            New ColumnMap("s_monthly_cut", "Œ—á•â“U", GetType(Long)), _
            New ColumnMap("s_once_cut", "ˆê‹à•â“U", GetType(Long)) _
        }

    End Class
End Namespace
