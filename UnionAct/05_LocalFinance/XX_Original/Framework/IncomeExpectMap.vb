Imports System

Namespace Framework.Mapping
    Public Class IncomeExpectMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(IncomeExpectMap.mapIncomeExpect)
        End Sub

        ' Fields
        Private Shared mapIncomeExpect As ColumnMap() = New ColumnMap() { _
            New ColumnMap("age", "年齢", GetType(Integer)), _
            New ColumnMap("01_number", "機長　人数", GetType(Integer)), _
            New ColumnMap("01_union_dues", "機長　組合費", GetType(Long)), _
            New ColumnMap("02_number", "副操縦士　人数", GetType(Integer)), _
            New ColumnMap("02_union_dues", "副操縦士　組合費", GetType(Long)), _
            New ColumnMap("03_number", "航空機関士　人数", GetType(Integer)), _
            New ColumnMap("03_union_dues", "航空機関士　組合費", GetType(Long)), _
            New ColumnMap("total_number", "合計　人数", GetType(Integer)), _
            New ColumnMap("total_union_dues", "合計　組合費", GetType(Long)) _
        }

    End Class
End Namespace
