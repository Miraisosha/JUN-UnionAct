Imports System

Namespace Framework.Mapping
    Public Class RevenueBudgetaryProcessDtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(RevenueBudgetaryProcessDtlMap.mapRBPDtl)
        End Sub

        ' Fields
        Private Shared ReadOnly mapRBPDtl As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_revenue_str", "ŠJn”NŒ“ú", GetType(String)), _
            New ColumnMap("k_budgetary_kind", "—\Zí•ÊƒR[ƒh", GetType(String)), _
            New ColumnMap("k_budgetary_process", "—\Z•ª—Ş", GetType(String)), _
            New ColumnMap("s_budgetary_process_seq", "—\ZŠÇ—}”Ô", GetType(Integer)), _
            New ColumnMap("l_number", "—\Z€”Ô", GetType(String)), _
            New ColumnMap("l_name", "—\Z‘è–Ú", GetType(String)), _
            New ColumnMap("s_budgetary_money", "—\Z‹àŠz", GetType(Long)), _
            New ColumnMap("l_biko_1", "”õl‚P", GetType(String)), _
            New ColumnMap("d_ins", "ì¬“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "ì¬ÒŒÂl" & "ID", GetType(String)), _
            New ColumnMap("d_up", "XV“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "XVÒŒÂl" & "ID", GetType(String)), _
            New ColumnMap("s_up", "XV‰ñ”", GetType(Integer)) _
        }

    End Class
End Namespace
