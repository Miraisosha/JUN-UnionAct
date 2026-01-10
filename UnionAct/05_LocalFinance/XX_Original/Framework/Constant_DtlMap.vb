Imports System

Namespace Framework.Mapping
    Public Class Constant_DtlMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(Constant_DtlMap.mapConstantDtl)
        End Sub

        ' Fields
        Private Shared ReadOnly mapConstantDtl As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_constant", "’è”‚h‚c", GetType(String)), _
            New ColumnMap("c_constant_seq", "’è”‚h‚c}”Ô", GetType(String)), _
            New ColumnMap("d_from", "“K—pŠJn”NŒ“ú", GetType(String)), _
            New ColumnMap("d_to", "“K—pI—¹”NŒ“ú", GetType(String)), _
            New ColumnMap("l_name", "–¼Ì", GetType(String)), _
            New ColumnMap("l_omission_name", "—ª–¼Ì", GetType(String)), _
            New ColumnMap("l_omission_name_2", "—ª–¼Ì‚Q", GetType(String)), _
            New ColumnMap("l_omission_name_3", "—ª–¼Ì‚R", GetType(String)), _
            New ColumnMap("l_omission_name_4", "—ª–¼Ì‚S", GetType(String)), _
            New ColumnMap("l_omission_name_5", "—ª–¼Ì‚T", GetType(String)), _
            New ColumnMap("s_order", "•\¦‡˜", GetType(Integer)), _
            New ColumnMap("l_biko", "”õl", GetType(String)), _
            New ColumnMap("d_ins", "ì¬“ú", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "ì¬ÒŒÂl‚h‚c", GetType(String)) _
        }

    End Class
End Namespace
