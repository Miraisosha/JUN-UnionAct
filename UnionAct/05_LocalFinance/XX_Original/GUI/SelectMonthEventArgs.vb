Imports System

Namespace GUI.FinancialAffairs.WithHolding
    Public Class SelectMonthEventArgs
        Inherits EventArgs
        ' Methods
        Public Sub New(ByVal Year As String, ByVal Month As String)
            Me._year = Year
            Me._month = Month
        End Sub


        ' Properties
        Public ReadOnly Property Month As String
            Get
                Return Me._month
            End Get
        End Property

        Public ReadOnly Property Year As String
            Get
                Return Me._year
            End Get
        End Property


        ' Fields
        Private _month As String
        Private _year As String
    End Class
End Namespace
