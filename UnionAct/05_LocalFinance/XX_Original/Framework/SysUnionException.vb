Imports log4net
Imports System
Imports System.Reflection

Namespace Framework.UnionException
    Public Class SysUnionException
        Inherits BaseUnionException
        ' Methods
        Public Sub New(ByVal method As MethodBase, ByVal ex As Exception, ByVal msgId As String, ByVal ParamArray strRep As String())
            MyBase.New(method, ex, msgId, strRep)
            SysUnionException._logger.Error(String.Concat(New String() { "MessageID:[", msgId, "] Method:[", method.ToString, "]" }))
            If (Not ex Is Nothing) Then
                SysUnionException._logger.Error(("ExceptionMessage:[" & ex.Message & "]"))
                SysUnionException._logger.Error(("Stacktrace:" & "" & ex.StackTrace))
                MyBase.UnionStackTrace = ex.StackTrace
            End If
        End Sub


        ' Fields
        Private Shared _logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
    End Class
End Namespace
