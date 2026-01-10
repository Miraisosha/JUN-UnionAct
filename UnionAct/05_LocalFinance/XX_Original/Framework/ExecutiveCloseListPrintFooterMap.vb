Imports System

Namespace Framework.Mapping
    Public Class ExecutiveCloseListPrintFooterMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(ExecutiveCloseListPrintFooterMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_explain", "ê‡ñæ", GetType(String)), _
            New ColumnMap("s_total", "çáåv", GetType(Integer)) _
        }

    End Class
End Namespace
