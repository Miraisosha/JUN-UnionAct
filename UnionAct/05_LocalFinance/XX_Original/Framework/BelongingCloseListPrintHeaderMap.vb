Imports System

Namespace Framework.Mapping
    Public Class BelongingCloseListPrintHeaderMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(BelongingCloseListPrintHeaderMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_local_name", "Žx•”–¼", GetType(String)), _
            New ColumnMap("d_from_year", "”N‚P", GetType(String)), _
            New ColumnMap("d_from_month", "ŒŽ‚P", GetType(String)), _
            New ColumnMap("d_to_year", "”N‚Q", GetType(String)), _
            New ColumnMap("d_to_month", "ŒŽ‚Q", GetType(String)) _
        }

    End Class
End Namespace
