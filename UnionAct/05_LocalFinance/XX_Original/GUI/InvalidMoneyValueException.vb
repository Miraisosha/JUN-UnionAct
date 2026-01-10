Imports System

Namespace GUI.Common
    Public Class InvalidMoneyValueException
        Inherits InvalidInputException
        ' Methods
        Public Sub New(ByVal FieldName As String)
            MyBase.New(FieldName)
        End Sub

    End Class
End Namespace

