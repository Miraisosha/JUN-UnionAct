Imports System

Namespace Framework.Mapping
    Public Class ExecutiveCloseListPrintMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(ExecutiveCloseListPrintMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "ŒÂl”FØ‚h‚c", GetType(String)), _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(String)), _
            New ColumnMap("l_name", "–¼‘O", GetType(String)), _
            New ColumnMap("l_post_name", "–ğE–¼", GetType(String)), _
            New ColumnMap("s_unit_price", "’P‰¿", GetType(Integer)), _
            New ColumnMap("l_day", "‘ÎÛ“ú", GetType(String)), _
            New ColumnMap("s_day", "“ú”", GetType(Integer)), _
            New ColumnMap("l_explain", "à–¾", GetType(String)), _
            New ColumnMap("k_model", "‹@í", GetType(String)), _
            New ColumnMap("k_belonging", "‘g‡x•”", GetType(String)), _
            New ColumnMap("k_qualification", "‘Ši", GetType(String)), _
            New ColumnMap("s_pay_day", "“ú“–", GetType(Integer)) _
        }

    End Class
End Namespace
