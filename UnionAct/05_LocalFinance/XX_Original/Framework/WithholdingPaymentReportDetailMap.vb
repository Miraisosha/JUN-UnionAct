Imports System

Namespace Framework.Mapping
    Public Class WithholdingPaymentReportDetailMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingPaymentReportDetailMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("year", "‘ÎÛ”N", GetType(String)), _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("l_name", "–¼‘O", GetType(String)), _
            New ColumnMap("k_model", "‹@í", GetType(String)), _
            New ColumnMap("k_qualification", "‘Ši", GetType(String)), _
            New ColumnMap("k_belonging", "x•”", GetType(String)), _
            New ColumnMap("allowance", "x‹‹‘Šz", GetType(Long)) _
        }

    End Class
End Namespace
