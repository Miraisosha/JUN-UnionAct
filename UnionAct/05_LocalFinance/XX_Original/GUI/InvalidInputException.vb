Imports System

Namespace GUI.Common
    Public MustInherit Class InvalidInputException
        Inherits ApplicationException
        ' Methods
        Public Sub New(ByVal InvalidFieldName As String)
            Me._field = If((InvalidFieldName Is Nothing), "", InvalidFieldName)
        End Sub


        ' Properties
        Public ReadOnly Property InvalidFieldName As String
            Get
                If (Me._field.Length <> 0) Then
                    Return Me._field
                End If
                Return " <Unknown> "
            End Get
        End Property


        ' Fields
        Private _field As String = ""
    End Class
End Namespace
