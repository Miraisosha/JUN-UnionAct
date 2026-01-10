Imports System

Namespace Framework.Mapping
    Public Class SpecialCommitteeSectionListReportMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(SpecialCommitteeSectionListReportMap.mapSpecialCommitteeSectionListReport)
        End Sub

        ' Fields
        Private Shared ReadOnly mapSpecialCommitteeSectionListReport As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_committee", "ˆÏˆõ‰ï–¼", GetType(String)), _
            New ColumnMap("l_name_officer_pay", "–¼‘O", GetType(String)), _
            New ColumnMap("c_staf_id_officer_pay", "Ğˆõ”Ô†", GetType(String)), _
            New ColumnMap("k_branch_officer_pay", "x•”ˆÏˆõ‰ïƒtƒ‰ƒO", GetType(String)), _
            New ColumnMap("l_model_1", "‹@í‚P", GetType(String)), _
            New ColumnMap("l_staf_name_1", "–¼‘O‚P", GetType(String)), _
            New ColumnMap("c_staf_id_1", "Ğˆõ”Ô†‚P", GetType(String)), _
            New ColumnMap("k_branch_1", "x•”ˆÏˆõ‰ïƒtƒ‰ƒO‚P", GetType(String)), _
            New ColumnMap("l_model_2", "‹@í‚Q", GetType(String)), _
            New ColumnMap("l_staf_name_2", "–¼‘O‚Q", GetType(String)), _
            New ColumnMap("c_staf_id_2", "Ğˆõ”Ô†‚Q", GetType(String)), _
            New ColumnMap("k_branch_2", "x•”ˆÏˆõ‰ïƒtƒ‰ƒO‚Q", GetType(String)), _
            New ColumnMap("l_model_3", "‹@í‚R", GetType(String)), _
            New ColumnMap("l_staf_name_3", "–¼‘O‚R", GetType(String)), _
            New ColumnMap("c_staf_id_3", "Ğˆõ”Ô†‚R", GetType(String)), _
            New ColumnMap("k_branch_3", "x•”ˆõ‰ïƒtƒ‰ƒO‚R", GetType(String)), _
            New ColumnMap("l_model_4", "‹@í‚S", GetType(String)), _
            New ColumnMap("l_staf_name_4", "–¼‘O‚S", GetType(String)), _
            New ColumnMap("c_staf_id_4", "Ğˆõ”Ô†‚S", GetType(String)), _
            New ColumnMap("k_branch_4", "x•”ˆõ‰ïƒtƒ‰ƒO‚S", GetType(String)), _
            New ColumnMap("l_model_5", "‹@í‚T", GetType(String)), _
            New ColumnMap("l_staf_name_5", "–¼‘O‚T", GetType(String)), _
            New ColumnMap("c_staf_id_5", "Ğˆõ”Ô†‚T", GetType(String)), _
            New ColumnMap("k_branch_5", "x•”ˆÏˆõ‰ïƒtƒ‰ƒO‚T", GetType(String)) _
        }

    End Class
End Namespace
