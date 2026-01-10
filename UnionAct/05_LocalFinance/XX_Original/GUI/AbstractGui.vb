Imports log4net
Imports System
Imports System.Reflection
Imports UnionAct.Business.Common
Imports UnionAct.Framework.UnionException

Namespace GUI.Common
    Public MustInherit Class AbstractGui
        ' Methods
        Protected Sub New()
        End Sub

        Public Shared Function GetBusinessObject(Of T As Class)(ByVal strClassName As String, ByVal ParamArray args As Object()) As T
            Dim local As T
            Try
                local = TryCast(New FactoryBusClass().GetObject(strClassName, args), T)
            Catch exception As SysUnionException
                exception.AddMethodName(MethodBase.GetCurrentMethod)
                Throw exception
            End Try
            Return local
        End Function

        Public Shared Function ShowErrMessageBox(ByVal ex As AppUnionException) As Boolean
            Try
                Dim msg As New ExceptionMsg(ex)
                If msg.IsNotContinue Then
                    Return False
                End If
                msg.ShowMessage()
                Return True
            Catch exception1 As Exception
                Return False
            End Try
        End Function

        Public Shared Sub WriteLog(ByVal strMsg As String)
            Try
                AbstractGui.logger.Info(strMsg)
                logger.Info(strMsg)
            Catch exception1 As Exception
            End Try
        End Sub


        ' Fields
        Private Shared ReadOnly logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
    End Class
End Namespace
