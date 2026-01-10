Imports System

Namespace Framework.Mapping
    Public Class WithholdingMonthlyTaxableDetailListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingMonthlyTaxableDetailListMap.map)
        End Sub

        ' 2016/12/07(…) C³ ‰ÛÅƒtƒ‰ƒO’Ç‰Á Start
        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() {
            New ColumnMap("checks", "@", GetType(Boolean)),
            New ColumnMap("employee_number", "Ğˆõ”Ô†", GetType(Long)),
            New ColumnMap("name", "–¼", GetType(String)),
            New ColumnMap("license", "‘Ši", GetType(String)),
            New ColumnMap("remuneration", "–ğˆõè“–", GetType(Long)),
            New ColumnMap("monthly", "Œ—áTœ", GetType(Long)),
            New ColumnMap("bonus", "ˆê‹àTœ", GetType(Long)),
            New ColumnMap("taxable", "‰ÛÅ‘ÎÛŠz", GetType(Long)),
            New ColumnMap("[truncate]", "ØÌ‚ÄŠz", GetType(Long)),
            New ColumnMap("withholding", "Œ¹ò’¥ûŠz", GetType(Long)),
            New ColumnMap("withholding_monthly", "Œ¹ò’¥ûŠz" & "(" & "Œ—á" & ")", GetType(Long)),
            New ColumnMap("withholding_bonus", "Œ¹ò’¥ûŠz" & "(" & "ˆê‹à" & ")", GetType(Long)),
            New ColumnMap("payout", "·ˆøx‹‹Šz", GetType(Long)),
            New ColumnMap("user_id", "ƒ†[ƒU" & "ID", GetType(String)),
            New ColumnMap("c_taxation_flag", "‰ÛÅƒtƒ‰ƒO", GetType(String))
        }
        ' 2016/12/07(…) C³ ‰ÛÅƒtƒ‰ƒO’Ç‰Á End
    End Class
End Namespace
