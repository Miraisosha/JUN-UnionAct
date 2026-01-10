Imports System

Namespace Framework.Mapping
    Public Class DailyPayMasterDtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(DailyPayMasterDtlMap.mapDailyPayDtl)
        End Sub

        ' Fields
        Private Shared ReadOnly mapDailyPayDtl As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_daily_pay_id", "“ú“–‚h‚c", GetType(String)), _
            New ColumnMap("c_menu_seq", "“ú“–‚h‚c}”Ô", GetType(Integer)), _
            New ColumnMap("d_from", "“K—pŠJn”NŒ“ú", GetType(String)), _
            New ColumnMap("d_to", "“K—pI—¹”NŒ“ú", GetType(String)), _
            New ColumnMap("l_name", "•\¦–¼Ì", GetType(String)), _
            New ColumnMap("l_explain", "à–¾", GetType(String)), _
            New ColumnMap("s_daily_pay", "“ú“–‹àŠz", GetType(Integer)), _
            New ColumnMap("l_biko", "”õl", GetType(String)), _
            New ColumnMap("d_ins", "ì¬“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "ì¬ÒŒÂl‚h‚c", GetType(String)) _
        }

    End Class
End Namespace
