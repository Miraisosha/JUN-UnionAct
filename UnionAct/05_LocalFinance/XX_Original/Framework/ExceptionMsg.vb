Imports log4net
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms
Imports System.Reflection

Namespace Framework.UnionException
    Public Class ExceptionMsg
        Inherits MessageBase
        ' Methods
        Public Sub New(ByVal ex As Exception)
            If (ex.GetType.ToString.Equals("Framework.UnionException.AppUnionException") OrElse TypeOf ex Is AppUnionException) Then
                Dim exception As AppUnionException = DirectCast(ex, AppUnionException)
                Dim i As Integer
                For i = 0 To exception.lstExMsgText.Count - 1
                    Dim item As New ExceptionMsgData With { _
                        .MessageId = String.Copy(exception.lstExMsgText.Item(i).MessageId) _
                    }
                    item = MyBase.SearchExMsgData(item.MessageId)
                    Me.lstExMsg.Add(item)
                    Dim str As String = MyBase.ReplaceMessage(String.Copy(Me.lstExMsg.Item(i).OutputMsg), exception.lstExMsgText.Item(i).strReplace)
                    Me.lstExMsg.Item(i).OutputMsg = str
                Next i
            ElseIf ex.GetType.ToString.Equals("Framework.UnionException.SysUnionException") Then
                Dim exception2 As SysUnionException = DirectCast(ex, SysUnionException)
                Dim j As Integer
                For j = 0 To exception2.lstExMsgText.Count - 1
                    Dim data2 As New ExceptionMsgData With { _
                        .MessageId = String.Copy(exception2.lstExMsgText.Item(j).MessageId) _
                    }
                    data2 = MyBase.SearchExMsgData(data2.MessageId)
                    Me.lstExMsg.Add(data2)
                    Dim str2 As String = MyBase.ReplaceMessage(String.Copy(Me.lstExMsg.Item(j).OutputMsg), exception2.lstExMsgText.Item(j).strReplace)
                    Me.lstExMsg.Item(j).OutputMsg = str2
                Next j
            End If
        End Sub

        Public Function GetMessageId() As String
            Return String.Copy(Me.lstExMsg.Item(0).MessageId)
        End Function

        Public Function GetOutputMsg() As String
            Return String.Copy(Me.lstExMsg.Item(0).OutputMsg)
        End Function

        Public Function IsNotContinue() As Boolean
            Dim i As Integer
            For i = 0 To Me.lstExMsg.Count - 1
                If Me.lstExMsg.Item(i).NotContinue Then
                    Return True
                End If
            Next i
            Return False
        End Function

        Public Sub SetMessageId(ByVal MsgId As String, ByVal ParamArray strRep As String())
            If (strRep Is Nothing) Then
                strRep = New String(0 - 1) {}
            End If
            Dim item As New ExceptionMsgData With { _
                .MessageId = String.Copy(MsgId) _
            }
            item = MyBase.SearchExMsgData(item.MessageId)
            Me.lstExMsg.Clear()
            Me.lstExMsg.Add(item)
            Dim str As String = MyBase.ReplaceMessage(String.Copy(Me.lstExMsg.Item(0).OutputMsg), strRep)
            Me.lstExMsg.Item(0).OutputMsg = str
        End Sub

        Public Sub SetOutputMsg(ByVal strMsg As String)
            Me.lstExMsg.Item(0).OutputMsg = String.Copy(strMsg)
        End Sub

        Public Function ShowMessage() As DialogResult
            Dim smallCode As Integer = 1
            Dim strArray As String() = New String() {"エラー", "注意", "インフォメーション", "問合せ"}
            Dim message As String = ""
            Dim i As Integer
            For i = 0 To Me.lstExMsg.Count - 1
                smallCode = CInt(MyBase.GetSmallCode(Me.lstExMsg.Item(i).MessageId))
                Dim str2 As String = message
                'message = String.Concat(New String() {str2, Me.lstExMsg.Item(i).MessageId, " : ", Me.lstExMsg.Item(i).OutputMsg, ""})
                message = String.Concat(New String() {str2, Me.lstExMsg.Item(i).OutputMsg, vbCrLf})
            Next i
            ExceptionMsg.logger.Info(message)
            If (Me.lstExMsg.Count = 0) Then
                Return DialogResult.None
            End If
            If (Me.lstExMsg.Count <> 1) Then
                'Return FrmMessageBox.Show(Nothing, message, strArray((smallCode - 1)), DirectCast(((Not smallCode And 1) << 2), MessageBoxButtons))
                Return MessageBox.Show(Nothing, message, strArray((smallCode - 1)), DirectCast(((Not smallCode And 1) << 2), MessageBoxButtons))
            End If
            If (MyBase.GetIdToInt(Me.lstExMsg.Item(0).MessageId) >= &H2328) Then
                Return DialogResult.None
            End If
            Return MessageBox.Show(message, strArray((smallCode - 1)), DirectCast(((Not smallCode And 1) << 2), MessageBoxButtons), DirectCast((((smallCode + ((smallCode And 2) >> 1)) - ((smallCode And 4) >> 1)) << 4), MessageBoxIcon))
        End Function


        ' Fields
        Private Shared logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod.DeclaringType)
        Private lstExMsg As List(Of ExceptionMsgData) = New List(Of ExceptionMsgData)
    End Class
End Namespace
