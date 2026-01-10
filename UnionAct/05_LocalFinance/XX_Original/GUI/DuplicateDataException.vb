Imports System

Namespace GUI.Common
    Public Class DuplicateDataException
        Inherits InvalidInputException
        ' Methods
        Public Sub New(ByVal FieldName As String)
            MyBase.New(FieldName)
        End Sub

    End Class
End Namespace
