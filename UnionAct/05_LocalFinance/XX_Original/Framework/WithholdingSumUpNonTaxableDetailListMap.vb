Imports System

Namespace Framework.Mapping
    Public Class WithholdingSumUpNonTaxableDetailListMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(WithholdingSumUpNonTaxableDetailListMap.map)
        End Sub

        ' Fields
        Private Shared map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("check", "@", GetType(Boolean)), _
            New ColumnMap("employee_number", "Ğˆõ”Ô†", GetType(Long)), _
            New ColumnMap("name", "–¼‘O", GetType(String)), _
            New ColumnMap("license", "‘Ši", GetType(String)), _
            New ColumnMap("monthly", "Œ—áTœ•â“UŠz", GetType(Long)), _
            New ColumnMap("bonus", "ˆê‹àTœ•â“UŠz", GetType(Long)), _
            New ColumnMap("paytotal", "x‹‹‘Šz", GetType(Long)), _
            New ColumnMap("userid", "ƒ†[ƒU" & "ID", GetType(String)) _
        }

    End Class
End Namespace
