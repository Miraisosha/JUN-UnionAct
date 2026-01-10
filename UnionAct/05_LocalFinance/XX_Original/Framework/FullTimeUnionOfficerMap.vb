Imports System

Namespace Framework.Mapping
    Public Class FullTimeUnionOfficerMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(FullTimeUnionOfficerMap.mapFullTimeUnionOfficer)
        End Sub

        ' Fields
        Private Shared ReadOnly mapFullTimeUnionOfficer As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "個人認証ＩＤ", GetType(String)), _
            New ColumnMap("c_ksh", "会社コード", GetType(String)), _
            New ColumnMap("c_staf_id", "社員番号", GetType(String)), _
            New ColumnMap("d_from", "適用開始年月日", GetType(String)), _
            New ColumnMap("k_officer_status", "専従職員ステータス区分", GetType(String)), _
            New ColumnMap("l_name", "名前", GetType(String)), _
            New ColumnMap("l_name_kna", "名前カナ", GetType(String)), _
            New ColumnMap("c_full_time_control_id", "画面制御ＩＤ", GetType(String)), _
            New ColumnMap("k_belonging", "所属支部区分", GetType(String)), _
            New ColumnMap("k_sex", "性別", GetType(String)), _
            New ColumnMap("d_birth", "生年月日", GetType(DateTime)), _
            New ColumnMap("d_enter", "入社年月日", GetType(DateTime)), _
            New ColumnMap("d_retire", "退社年月日", GetType(DateTime)), _
            New ColumnMap("l_reason", "事由または理由", GetType(String)), _
            New ColumnMap("l_add_number", "郵便番号", GetType(String)), _
            New ColumnMap("l_prefectures", "都道府県", GetType(String)), _
            New ColumnMap("l_cities", "市区町村", GetType(String)), _
            New ColumnMap("l_add_ather", "番地等", GetType(String)), _
            New ColumnMap("l_building", "建物名等", GetType(String)), _
            New ColumnMap("l_tell_1", "電話番号１", GetType(String)), _
            New ColumnMap("l_tell_2", "電話番号２", GetType(String)), _
            New ColumnMap("l_tell_3", "電話番号３", GetType(String)), _
            New ColumnMap("l_fax", "ファックス", GetType(String)), _
            New ColumnMap("l_e_meil_1", "e-mail 1", GetType(String)), _
            New ColumnMap("l_e_meil_2", "e-mail 2", GetType(String)), _
            New ColumnMap("l_e_meil_3", "e-mail 3", GetType(String)), _
            New ColumnMap("k_del", "削除区分", GetType(String)), _
            New ColumnMap("l_biko_1", "備考１", GetType(String)), _
            New ColumnMap("l_biko_2", "備考２", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人ＩＤ", GetType(String)), _
            New ColumnMap("d_up", "更新日", GetType(DateTime)), _
            New ColumnMap("c_user_id_up", "更新者個人ＩＤ", GetType(String)), _
            New ColumnMap("s_up", "更新回数", GetType(Integer)) _
        }

    End Class
End Namespace
