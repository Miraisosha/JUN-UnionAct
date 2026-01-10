Imports System

Namespace Framework.Mapping
    Public Class WithholdingSumUpReportDetailMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingSumUpReportDetailMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("year", "‘ÎÛ”N", GetType(String)), _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("l_name", "–¼‘O", GetType(String)), _
            New ColumnMap("k_model", "‹@í", GetType(String)), _
            New ColumnMap("k_qualification", "‘Ši", GetType(String)), _
            New ColumnMap("k_belonging", "x•”", GetType(String)), _
            New ColumnMap("l_add_number", "—X•Ö”Ô†", GetType(String)), _
            New ColumnMap("l_prefectures", "“s“¹•{Œ§", GetType(String)), _
            New ColumnMap("l_cities", "s‹æ’¬‘º", GetType(String)), _
            New ColumnMap("l_add_ather", "”Ô’n“™", GetType(String)), _
            New ColumnMap("l_building", "Œš•¨–¼“™", GetType(String)), _
            New ColumnMap("payroll", "‹‹—^‚Ì‡Œv", GetType(Long)), _
            New ColumnMap("withholding", "Œ¹ò’¥ûŠz", GetType(Long)), _
            New ColumnMap("payer_address", "‘g‡ZŠ" & "1", GetType(String)), _
            New ColumnMap("payer_building", "‘g‡ZŠ" & "2", GetType(String)) _
        }

    End Class
End Namespace
