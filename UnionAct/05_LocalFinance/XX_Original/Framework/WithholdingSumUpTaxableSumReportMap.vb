Imports System

Namespace Framework.Mapping
    Public Class WithholdingSumUpTaxableSumReportMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingSumUpTaxableSumReportMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_title", "x•”", GetType(String)), _
            New ColumnMap("s_officer_pay", "–ğˆõè“–", GetType(Integer)), _
            New ColumnMap("s_monthly_cut", "Œ—á•â“U", GetType(Integer)), _
            New ColumnMap("s_once_cut", "ˆê‹à•â“U", GetType(Integer)), _
            New ColumnMap("allowance", "Œ¹ò’¥ûŠz", GetType(Integer)) _
        }

    End Class
End Namespace
