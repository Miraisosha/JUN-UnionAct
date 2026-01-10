Imports System

Namespace Framework.Mapping
    Public Class WithholdingSumUpTaxableDetailListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingSumUpTaxableDetailListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("check", "@", GetType(Boolean)), _
            New ColumnMap("employee_number", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("name", "–¼‘O", GetType(String)), _
            New ColumnMap("license", "‘Ši", GetType(String)), _
            New ColumnMap("remuneration", "–ğˆõè“–", GetType(Long)), _
            New ColumnMap("monthly", "Œ—áTœ•â“U", GetType(Long)), _
            New ColumnMap("bonus", "ˆê‹àTœ•â“U", GetType(Long)), _
            New ColumnMap("paytotal", "‘x‹‹Šz", GetType(Long)), _
            New ColumnMap("withholding", "Œ¹ò’¥ûŠz", GetType(Long)), _
            New ColumnMap("payout", "·ˆøx‹‹Šz", GetType(Long)), _
            New ColumnMap("userid", "ƒ†[ƒU" & "ID", GetType(String)), _
            New ColumnMap("monthly_withholding", "Œ¹ò’¥ûŠz" & "(" & "Œ—á" & ")", GetType(Long)), _
            New ColumnMap("once_withholding", "Œ¹ò’¥ûŠz" & "(" & "ˆê‹à" & ")", GetType(Long)) _
        }

    End Class
End Namespace
