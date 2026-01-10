Imports log4net
Imports System
Imports System.Collections.Generic
Imports System.Reflection

Namespace Framework.UnionException
    Public Class BaseUnionException
        Inherits Exception
        ' Methods
        Protected Sub New(ByVal method As MethodBase, ByVal ex As Exception, ByVal msgId As String, ByVal strRep As String())
            MyBase.New(ex.Message)
            Me.MethodName = New List(Of String)
            Me.AddMethodName(method)
            Me.exception = ex
            Me.lstExMsgText = New List(Of ExceptionMsgText)
            Dim item As New ExceptionMsgText With { _
                .MessageId = msgId _
            }
            If (strRep Is Nothing) Then
                strRep = New String(0  - 1) {}
            End If
            item.strReplace = New String(strRep.Length  - 1) {}
            Dim i As Byte = 0
            Do While (i < strRep.Length)
                item.strReplace(i) = strRep(i)
                i = CByte((i + 1))
            Loop
            Me.lstExMsgText.Add(item)
        End Sub

        Public Sub AddExceptionData(ByVal msgId As String, ByVal ParamArray strRep As String())
            Dim item As New ExceptionMsgText With { _
                .MessageId = msgId _
            }
            If (strRep Is Nothing) Then
                strRep = New String(0  - 1) {}
            End If
            item.strReplace = New String(strRep.Length  - 1) {}
            Dim i As Byte = 0
            Do While (i < strRep.Length)
                item.strReplace(i) = strRep(i)
                i = CByte((i + 1))
            Loop
            Me.lstExMsgText.Add(item)
        End Sub

        Public Sub AddMethodName(ByVal method As MethodBase)
            BaseUnionException.logger.Error(("Method Call Stack = " & method.ToString))
            Me.MethodName.Add((method.DeclaringType.ToString & " : " & method.ToString))
        End Sub


        ' Fields
        Protected exception As Exception
        Private Shared logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Public lstExMsgText As List(Of ExceptionMsgText)
        Public MethodName As List(Of String)
        Public UnionStackTrace As String

        ' Nested Types
        Public Class ExceptionMsgText
            ' Fields
            Public MessageId As String
            Public strReplace As String()
        End Class
    End Class
End Namespace
