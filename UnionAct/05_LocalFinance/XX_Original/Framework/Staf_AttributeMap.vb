Imports System

Namespace Framework.Mapping
    Public Class Staf_AttributeMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(Staf_AttributeMap.mapStafAttribute)
        End Sub

        ' Fields
        Private Shared mapStafAttribute As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "個人認証ＩＤ", GetType(String)), _
            New ColumnMap("c_ksh", "会社コード", GetType(String)), _
            New ColumnMap("c_staf_id", "社員番号", GetType(String)), _
            New ColumnMap("d_from", "適用日付", GetType(String)), _
            New ColumnMap("c_dezit", "ディジット", GetType(String)), _
            New ColumnMap("l_name", "名前", GetType(String)), _
            New ColumnMap("l_name_kna", "名前カナ", GetType(String)), _
            New ColumnMap("k_user_status", "組合員ステータス区分", GetType(String)), _
            New ColumnMap("c_trans_ksh", "所属会社コード", GetType(String)), _
            New ColumnMap("k_staf_kind", "組合員種別コード", GetType(String)), _
            New ColumnMap("k_belonging", "所属支部", GetType(String)), _
            New ColumnMap("k_qualification", "乗務資格", GetType(String)), _
            New ColumnMap("k_model", "機種", GetType(String)), _
            New ColumnMap("k_international", "国際線", GetType(String)), _
            New ColumnMap("k_work_place", "職場", GetType(String)), _
            New ColumnMap("k_local", "会社支部", GetType(String)), _
            New ColumnMap("k_work_state", "勤務形態", GetType(String)), _
            New ColumnMap("c_area", "地区", GetType(String)), _
            New ColumnMap("k_sex", "性別", GetType(String)), _
            New ColumnMap("d_birth", "生年月日", GetType(DateTime)), _
            New ColumnMap("d_enter", "入社年月日", GetType(DateTime)), _
            New ColumnMap("d_retire", "退職年月日", GetType(DateTime)), _
            New ColumnMap("d_join", "加入年月日", GetType(DateTime)), _
            New ColumnMap("d_drop_out", "脱退年月日", GetType(DateTime)), _
            New ColumnMap("d_captain", "機長年月日", GetType(DateTime)), _
            New ColumnMap("d_teacher_captain", "教官機長年月日", GetType(DateTime)), _
            New ColumnMap("d_los_position", "地位喪失年月日", GetType(DateTime)), _
            New ColumnMap("l_los_position", "地位喪失理由区分", GetType(String)), _
            New ColumnMap("d_die", "死亡年月日", GetType(String)), _
            New ColumnMap("c_staf_id_old", "旧社員番号", GetType(String)), _
            New ColumnMap("c_dezit_old", "旧ディジット", GetType(String)), _
            New ColumnMap("l_reason", "脱退理由", GetType(String)), _
            New ColumnMap("l_biko_1", "備考１", GetType(String)), _
            New ColumnMap("k_del", "削除区分", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人ＩＤ", GetType(String)), _
            New ColumnMap("d_up", "更新日", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "更新者個人ＩＤ", GetType(String)), _
            New ColumnMap("s_up", "更新回数", GetType(Integer)) _
        }

    End Class
End Namespace
