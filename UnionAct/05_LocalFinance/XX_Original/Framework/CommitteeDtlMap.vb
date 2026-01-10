Imports System

Namespace Framework.Mapping
    Public Class CommitteeDtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CommitteeDtlMap.mapCommiteeDtl)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCommiteeDtl As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_committee_id", "ˆÏˆõ‰ï‚h‚c", GetType(String)), _
            New ColumnMap("s_committee_seq", "ˆÏˆõ‰ï‚h‚c}”Ô", GetType(Integer)), _
            New ColumnMap("d_from", "“K—pŠJn”NŒ“ú", GetType(String)), _
            New ColumnMap("d_to", "“K—pI—¹”NŒ“ú", GetType(String)), _
            New ColumnMap("l_name", "–¼Ì", GetType(String)), _
            New ColumnMap("s_appoint_max", "”C–½Å‘å”", GetType(Integer)), _
            New ColumnMap("c_daily_pay_id", "“ú“–‚h‚c", GetType(String)), _
            New ColumnMap("c_officer_pay_id", "–ğˆõè“–‚h‚c", GetType(String)), _
            New ColumnMap("c_executive_lunch_pay_id", "’†‰›·s’‹H”ï‚h‚c", GetType(String)), _
            New ColumnMap("d_service_from", "”CŠúŠJnŒ", GetType(String)), _
            New ColumnMap("d_service_to", "”CŠúI—¹Œ", GetType(String)), _
            New ColumnMap("s_from_diff", "”CŠúŠJn‚s‚nŠúŠJn·", GetType(Integer)), _
            New ColumnMap("s_to_diff", "”CŠúI—¹‚s‚nŠúI—¹·", GetType(Integer)), _
            New ColumnMap("l_biko", "”õl", GetType(String)), _
            New ColumnMap("d_ins", "ì¬“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "ì¬ÒŒÂl‚h‚c", GetType(String)), _
            New ColumnMap("k_head_flg", "’·ƒtƒ‰ƒO", GetType(String)) _
        }

    End Class
End Namespace
