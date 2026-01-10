Imports System

Namespace Framework.Mapping
    Public Class WithholdingSumUpTaxableSumListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingSumUpTaxableSumListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("u_branch", "x•”", GetType(String)), _
            New ColumnMap("remuneration", "–ğˆõè“–Šz", GetType(Long)), _
            New ColumnMap("monthly", "Œ—á’À‹àTœ•â“UŠz", GetType(Long)), _
            New ColumnMap("bonus", "ˆê‹àTœ•â“UŠz", GetType(Long)), _
            New ColumnMap("totalpay", "x‹‹‘Šz", GetType(Long)), _
            New ColumnMap("withholding", "Œ¹ò’¥ûŠz", GetType(Long)), _
            New ColumnMap("payout", "·ˆøx‹‹Šz", GetType(Long)) _
        }

    End Class
End Namespace
