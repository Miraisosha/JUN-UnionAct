Imports System

Namespace GUI.Common
    Friend Class InvalidTimeException
        Inherits InvalidInputException
        ' Methods
        Public Sub New(ByVal FieldName As String)
            MyBase.New(FieldName)
        End Sub

    End Class
End Namespace

