Imports System

Namespace GUI.Common
    Public Class InvalidAttributeException
        Inherits InvalidInputException
        ' Methods
        Public Sub New(ByVal FieldName As String)
            MyBase.New(FieldName)
        End Sub

    End Class
End Namespace
