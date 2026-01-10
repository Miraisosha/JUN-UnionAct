Imports System

Namespace Framework.Mapping
    Public Class ExecutiveCloseDtlPrintHeaderMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(ExecutiveCloseDtlPrintHeaderMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("s_year", "”N", GetType(Integer)), _
            New ColumnMap("s_month", "ŒŽ", GetType(Integer)) _
        }

    End Class
End Namespace
