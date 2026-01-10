Imports System

Namespace Framework.Mapping
    Public Class WithholdingSumUpNonTaxableSumReportMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingSumUpNonTaxableSumReportMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_title", "Žx•”", GetType(String)), _
            New ColumnMap("s_monthly_cut", "ŒŽ—á•â“U", GetType(Integer)), _
            New ColumnMap("s_once_cut", "ˆêŽž‹à•â“U", GetType(Integer)) _
        }

    End Class
End Namespace
