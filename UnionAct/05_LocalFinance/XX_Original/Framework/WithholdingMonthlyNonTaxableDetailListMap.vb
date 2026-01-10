Imports System

Namespace Framework.Mapping
    Public Class WithholdingMonthlyNonTaxableDetailListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingMonthlyNonTaxableDetailListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() {
            New ColumnMap("checks", "@", GetType(Boolean)),
            New ColumnMap("employee_number", "Ğˆõ”Ô†", GetType(Long)),
            New ColumnMap("name", "–¼", GetType(String)),
            New ColumnMap("license", "‘Ši", GetType(String)),
            New ColumnMap("monthly", "Œ—áTœ", GetType(Long)),
            New ColumnMap("bonus", "ˆê‹àTœ", GetType(Long)),
            New ColumnMap("[truncate]", "ØÌ‚ÄŠz", GetType(Long)),
            New ColumnMap("payout", "·ˆøx‹‹Šz", GetType(Long)),
            New ColumnMap("user_id", "ƒ†[ƒU" & "ID", GetType(String))
        }

    End Class
End Namespace
