Imports log4net
Imports System
Imports System.Reflection

Namespace Framework.UnionException
    Public Class DataNotFoundException
        Inherits ApplicationException
        ' Methods
        Public Sub New()
        End Sub

        Public Sub New(ByVal AdditionalInfo As String)
            DataNotFoundException._logger.Warn(("該当データが存在しません。（" & AdditionalInfo & "）"))
        End Sub


        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
    End Class
End Namespace
