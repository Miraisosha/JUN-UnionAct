Imports System

Namespace Framework.Mapping
    Public Class StafAttributeAllowanceMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(StafAttributeAllowanceMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("c_user_id", "ŒÂl”FØ‚h‚c", GetType(String)), _
            New ColumnMap("c_staf_id", "Ğˆõ”Ô†", GetType(String)), _
            New ColumnMap("l_name", "–¼‘O", GetType(String)), _
            New ColumnMap("k_belonging", "‘g‡x•”‹æ•ª", GetType(String)), _
            New ColumnMap("k_qualification", "‘Šiiæ–±ˆõj‹æ•ª", GetType(String)) _
        }

    End Class
End Namespace
