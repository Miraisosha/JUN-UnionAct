Imports System

Namespace Framework.Mapping
    Public Class ExecutiveCloseListPrintHeaderMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(ExecutiveCloseListPrintHeaderMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("d_year", "”N", GetType(String)), _
            New ColumnMap("d_month", "ŒŽ", GetType(String)) _
        }

    End Class
End Namespace
