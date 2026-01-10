Imports System

Namespace Framework.Mapping
    Public Class BelongingCloseListPrintMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(BelongingCloseListPrintMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("l_name", "–¼Ì", GetType(String)), _
            New ColumnMap("s_daily_pay", "“ú“–‹àŠz", GetType(Integer)), _
            New ColumnMap("s_food_expenses", "’‹H”ï", GetType(Integer)) _
        }

    End Class
End Namespace
