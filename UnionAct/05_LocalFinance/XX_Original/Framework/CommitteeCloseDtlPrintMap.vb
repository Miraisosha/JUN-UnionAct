Imports System

Namespace Framework.Mapping
    Public Class CommitteeCloseDtlPrintMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CommitteeCloseDtlPrintMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "ŒÂl”FØ‚h‚c", GetType(String)), _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(String)), _
            New ColumnMap("name", "–¼‘O", GetType(String)), _
            New ColumnMap("l_name", "–¼Ì", GetType(String)), _
            New ColumnMap("s_daily_pay", "“ú“–‹àŠz", GetType(Integer)), _
            New ColumnMap("s_day", "“ú•tî•ñ", GetType(String)), _
            New ColumnMap("l_explain", "à–¾", GetType(String)), New ColumnMap("k_model", "‹@í", GetType(String)) _
        }

    End Class
End Namespace
