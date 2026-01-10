Imports System

Namespace Framework.Mapping
    Public Class CommitteeListDtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CommitteeListDtlMap.mapCommitteeListDtl)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCommitteeListDtl As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_committee_list", "ˆÏˆõ‰ï–¼•ë‚h‚c", GetType(String)), _
            New ColumnMap("c_user_id", "ŒÂl”FØ‚h‚c", GetType(String)), _
            New ColumnMap("d_from", "“K—pŠJn”NŒ“ú", GetType(String)), _
            New ColumnMap("c_committee_id", "ˆÏˆõ‰ï‚h‚c", GetType(String)), _
            New ColumnMap("s_committee_seq", "ˆÏˆõ‰ï‚h‚c}”Ô", GetType(Integer)), _
            New ColumnMap("l_biko", "”õl", GetType(String)), _
            New ColumnMap("d_ins", "ì¬“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "ì¬ÒŒÂl‚h‚c", GetType(String)), _
            New ColumnMap("d_up", "XV“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "XVÒŒÂl‚h‚c", GetType(String)), _
            New ColumnMap("s_up", "XV‰ñ”", GetType(Integer)) _
        }

    End Class
End Namespace
