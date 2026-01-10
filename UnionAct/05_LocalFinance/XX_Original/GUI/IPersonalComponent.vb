Imports System

Namespace GUI.UnionComponent
    Friend Interface IPersonalComponent
        ' Methods
        Sub AddValidator(ByVal NewValidator As Object) 'TODO Delegate)
        Sub ClearValidator()
        Sub SetValidator(ByVal NewValidator As Object) 'TODO Delegate)
        Sub Validate()
        Sub ValidateDefault()

        ' Properties
        'ReadOnly Property FieldName As String
        '    Get
        'End Property
        'Property Require As Boolean
        '    Get
        '    Set(ByVal value As Boolean)
        'End Property
    End Interface
End Namespace
