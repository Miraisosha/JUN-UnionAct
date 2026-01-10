Imports System

Namespace Business.Master
    Public Interface IUnionDuesCommand
        ' Methods
        Function GetUnionDuesCommand(ByVal strUnionDues As String, ByVal strQualification As String, ByVal strStafKind As String, ByVal iAge As Integer, ByVal strKeyDate As String) As Long
    End Interface
End Namespace
