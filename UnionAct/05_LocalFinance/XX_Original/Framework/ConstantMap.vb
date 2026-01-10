Imports System

Namespace Framework.Mapping
    Public Class ConstantMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(ConstantMap.mapConstant)
        End Sub

        ' Fields
        Private Shared ReadOnly mapConstant As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_constant", "定数ＩＤ", GetType(String)), _
            New ColumnMap("d_from", "適用開始年月日", GetType(String)), _
            New ColumnMap("d_to", "適用終了年月日", GetType(String)), _
            New ColumnMap("l_name", "名称", GetType(String)), _
            New ColumnMap("l_biko", "備考", GetType(String)), _
            New ColumnMap("d_ins", "作成日", GetType(DateTime)), _
            New ColumnMap("c_user_id_ins", "作成者個人ＩＤ", GetType(String)) _
        }

    End Class
End Namespace
