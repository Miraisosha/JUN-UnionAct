Imports System

Namespace Framework.Mapping
    Public Class CommitteeSearch
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(CommitteeSearch.mapCommitteeSearch)
        End Sub

        ' Fields
        Private Shared ReadOnly mapCommitteeSearch As ColumnMap() = New ColumnMap() { _
            New ColumnMap("stfat_l_name", "名前", GetType(String)), _
            New ColumnMap("stfat_c_staf_id", "社員番号", GetType(String)), _
            New ColumnMap("conmo_l_name", "機種", GetType(String)), _
            New ColumnMap("conqu_l_name", "資格", GetType(String)), _
            New ColumnMap("conbe_l_name", "組合支部", GetType(String)), _
            New ColumnMap("area_l_name", "会社所属", GetType(String)), _
            New ColumnMap("perid_c_ksh", "会社コード", GetType(String)), _
            New ColumnMap("comld_c_user_id", "個人認証ＩＤ", GetType(String)), _
            New ColumnMap("conmo_l_omission_name", "機種（略名）", GetType(String)), _
            New ColumnMap("stfat_k_model", "機種区分", GetType(String)), _
            New ColumnMap("stfat_k_qualification", "資格区分", GetType(String)), _
            New ColumnMap("stfat_k_belonging", "所属支部区分", GetType(String)), _
            New ColumnMap("stfat_k_area_local", "会社支部区分", GetType(String)), _
            New ColumnMap("stfat_l_name_kna", "名前カナ", GetType(String)), _
            New ColumnMap("stfad_l_tell_1", "電話番号", GetType(String)), _
            New ColumnMap("stfat_k_staf_kind", "組合員種別コード", GetType(String)), _
            New ColumnMap("consk_l_name", "組合員種別", GetType(String)), _
            New ColumnMap("stfat_k_user_status", "組合員ステータス区分", GetType(String)), _
            New ColumnMap("conus_l_name", "ステータス", GetType(String)), _
            New ColumnMap("conqu_omission_name", "資格（和名）", GetType(String)), _
            New ColumnMap("area_omission_name", "所属", GetType(String)), _
            New ColumnMap("conbe_omission_name", "支部", GetType(String)), _
            New ColumnMap("i_stfat_c_staf_id", "社員番号（" & "int" & "）", GetType(Long)), _
            New ColumnMap("isremove", "削除可否フラグ", GetType(Boolean)), _
            New ColumnMap("dezit", "ディジット", GetType(String)) _
        }

    End Class
End Namespace
