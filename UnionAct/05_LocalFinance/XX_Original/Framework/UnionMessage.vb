Imports System
Imports System.Windows.Forms

Namespace Framework
    Public Class UnionMessage
        Inherits MessageBase
        ' Methods
        Public Function ShowMessage(ByVal MessageId As String) As DialogResult
            Dim smallCode As Integer = CInt(MyBase.GetSmallCode(MessageId))
            Dim data As ExceptionMsgData = MyBase.SearchExMsgData(MessageId)
            Return Me.ShowMessage(MessageId, data.OutputMsg, smallCode)
        End Function

        Public Function ShowMessage(ByVal MessageId As String, ByVal ParamArray args As String()) As DialogResult
            Dim smallCode As Integer = CInt(MyBase.GetSmallCode(MessageId))
            Dim messageBody As String = String.Format(MyBase.SearchExMsgData(MessageId).OutputMsg, DirectCast(args, Object()))
            Return Me.ShowMessage(MessageId, messageBody, smallCode)
        End Function

        Public Function ShowMessage(ByVal focusYes As Boolean, ByVal MessageId As String, ByVal ParamArray args As String()) As DialogResult
            Dim smallCode As Integer = CInt(MyBase.GetSmallCode(MessageId))
            Dim data As ExceptionMsgData = MyBase.SearchExMsgData(MessageId)
            Dim messageBody As String = If((args Is Nothing), data.OutputMsg, String.Format(data.OutputMsg, DirectCast(args, Object())))
            Dim dfltBtn As MessageBoxDefaultButton = If(focusYes, MessageBoxDefaultButton.Button1, MessageBoxDefaultButton.Button2)
            Return Me.ShowMessage(MessageId, messageBody, smallCode, dfltBtn)
        End Function

        Private Function ShowMessage(ByVal MessageId As String, ByVal MessageBody As String, ByVal small As Integer) As DialogResult
            Return Me.ShowMessage(MessageId, MessageBody, small, MessageBoxDefaultButton.Button1)
        End Function

        Private Function ShowMessage(ByVal MessageId As String, ByVal MessageBody As String, ByVal small As Integer, ByVal dfltBtn As MessageBoxDefaultButton) As DialogResult
            If (MyBase.GetIdToInt(MessageId) >= &H2328) Then
                Return DialogResult.None
            End If
            'Return MessageBox.Show((MessageId & " : " & MessageBody), MyBase.SmallTable((small - 1)), DirectCast(((Not small And 1) << 2), MessageBoxButtons), DirectCast((((small + ((small And 2) >> 1)) - ((small And 4) >> 1)) << 4), MessageBoxIcon), dfltBtn)
            Return MessageBox.Show(MessageBody, MyBase.SmallTable((small - 1)), DirectCast(((Not small And 1) << 2), MessageBoxButtons), DirectCast((((small + ((small And 2) >> 1)) - ((small And 4) >> 1)) << 4), MessageBoxIcon), dfltBtn)
        End Function

    End Class
End Namespace
