Imports System

Namespace Framework.Mapping
    Public Class TaxRateMap
        Inherits EntityMap
        ' Methods
        Public Sub New()
            MyBase.New(TaxRateMap.map)
        End Sub

        ' Fields
        Private Shared ReadOnly map As ColumnMap() = New ColumnMap() { _
            New ColumnMap("lower", "â∫å¿äz", GetType(Long)), _
            New ColumnMap("upper", "è„å¿äz", GetType(Long)), _
            New ColumnMap("amount", "â€ê≈äz", GetType(Long)), _
            New ColumnMap("rate", "â€ê≈ó¶", GetType(Double)) _
        }

    End Class
End Namespace
