Imports System

Namespace Framework.Mapping
    Public Class FullTimeTemporarySalaryDtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(FullTimeTemporarySalaryDtlMap.mapFullTimeTemporarySalaryDtl)
        End Sub

        ' Fields
        Private Shared ReadOnly mapFullTimeTemporarySalaryDtl As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_salary_pay", "x•¥”NŒ", GetType(String)), _
            New ColumnMap("c_user_id", "ŒÂl”FØ‚h‚c", GetType(String)), _
            New ColumnMap("l_salary_item", "‹‹—^‰È–Ú×–Ú–¼Ì", GetType(String)), _
            New ColumnMap("k_salary_item_classify", "‹‹—^‰È–Ú•ª—Ş", GetType(String)), _
            New ColumnMap("s_item", "‹àŠz", GetType(Integer)), _
            New ColumnMap("k_tax", "‰ÛÅ‹æ•ª", GetType(String)), _
            New ColumnMap("l_biko", "”õl", GetType(String)), _
            New ColumnMap("d_ins", "ì¬“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "ì¬ÒŒÂl‚h‚c", GetType(String)), _
            New ColumnMap("d_up", "XV“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "XVÒŒÂl" & "ID", GetType(String)), _
            New ColumnMap("s_up", "XV‰ñ”", GetType(Integer)) _
        }

    End Class
End Namespace
