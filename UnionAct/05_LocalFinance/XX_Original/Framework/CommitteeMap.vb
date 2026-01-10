Imports System

Namespace Framework.Mapping
    Public Class CommitteeMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CommitteeMap.mapCommitee)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCommitee As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_committee_id", "委員会ＩＤ", GetType(String)), _
            New ColumnMap("d_from", "適用開始年月日", GetType(String)), _
            New ColumnMap("d_to", "適用終了年月日", GetType(String)), _
            New ColumnMap("c_ksh", "会社コード", GetType(String)), _
            New ColumnMap("k_committee_kind", "委員会種別区分", GetType(String)), _
            New ColumnMap("k_belonging", "所属支部区分", GetType(String)), _
            New ColumnMap("l_name", "名称", GetType(String)), _
            New ColumnMap("l_biko", "備考", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人ＩＤ", GetType(String)) _
        }

    End Class
End Namespace
