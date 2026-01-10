Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Xml.Serialization
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg

Namespace Framework
    Public MustInherit Class MessageBase
        ' Methods
        Public Sub New()
            If (MessageBase.lstExMsgDatXML Is Nothing) Then
                Me.ReadExMsgData
            End If
        End Sub

        Protected Function GetIdToInt(ByVal strMsgId As String) As Integer
            Return Integer.Parse(strMsgId.Substring(2))
        End Function

        Protected Function GetSmallCode(ByVal strMsgId As String) As SMALL
            Dim str As String = strMsgId.Substring(1, 1)
            If Not str.Equals("E") Then
                If str.Equals("W") Then
                    Return SMALL.Warning
                End If
                If str.Equals("I") Then
                    Return SMALL.Information
                End If
                If str.Equals("Q") Then
                    Return SMALL.Question
                End If
            End If
            Return SMALL.Error
        End Function

        Protected Sub ReadExMsgData()
            'Dim path As String = MDSystemInfo.MessagePath & MDSystemInfo.MessageName
            Dim path As String = "ExceptionMsgDataXXX.xml"
            Dim extraTypes As Type() = New Type() {GetType(List(Of ExceptionMsgData))}
            Dim serializer As New XmlSerializer(GetType(List(Of ExceptionMsgData)), extraTypes)
            Dim stream As FileStream = Nothing
            Try 
                stream = New FileStream(path, FileMode.Open)
                MessageBase.lstExMsgDatXML = DirectCast(serializer.Deserialize(stream), List(Of ExceptionMsgData))
            Catch exception1 As FileNotFoundException
                Return
            Finally
                If (Not stream Is Nothing) Then
                    stream.Close
                End If
            End Try
            Me.ReplaceStrExceptionMsg("$$", "")
        End Sub

        Protected Function ReplaceMessage(ByVal strMsg As String, ByVal strReplace As String()) As String
            Dim i As Byte = 0
            Do While (i < strReplace.Length)
                strMsg = strMsg.Replace(("{" & i.ToString & "}"), strReplace(i))
                i = CByte((i + 1))
            Loop
            Return strMsg
        End Function

        Protected Sub ReplaceStrExceptionMsg(ByVal str1 As String, ByVal str2 As String)
            Dim i As Integer
            For i = 0 To MessageBase.lstExMsgDatXML.Count - 1
                Dim str As String = MessageBase.lstExMsgDatXML.Item(i).OutputMsg.Replace(str1, str2)
                MessageBase.lstExMsgDatXML.Item(i).OutputMsg = str
            Next i
        End Sub

        'Protected Function SearchExMsgData(ByVal MsgId As String) As ExceptionMsgData
        '    Dim data As ExceptionMsgData = Nothing
        '    Dim data2 As ExceptionMsgData = Nothing
        '    Dim i As Integer
        '    For i = 0 To MessageBase.lstExMsgDatXML.Count - 1
        '        If MessageBase.lstExMsgDatXML.Item(i).MessageId.Equals(MsgId) Then
        '            data = MessageBase.lstExMsgDatXML.Item(i)
        '            Exit For
        '        End If
        '        If MessageBase.lstExMsgDatXML.Item(i).MessageId.Equals("FE0001") Then
        '            data2 = MessageBase.lstExMsgDatXML.Item(i)
        '        End If
        '    Next i
        '    If (data Is Nothing) Then
        '        data = data2
        '    End If
        '    Return New ExceptionMsgData With { _
        '        .MessageId = String.Copy(data.MessageId), _
        '        .NotContinue = data.NotContinue, _
        '        .OutputDb = data.OutputDb, _
        '        .OutputMsg = String.Copy(data.OutputMsg) _
        '    }
        'End Function

        Protected Function SearchExMsgData(ByVal MsgId As String) As ExceptionMsgData
            Dim pBlnContinueFlg As Boolean
            Dim pBlnDbFlg As Boolean
            Dim pStrMsg As String = ""

            CLMsg.GetMsgInfo(MsgId, pBlnContinueFlg, pBlnDbFlg, pStrMsg)

            Return New ExceptionMsgData With { _
                .MessageId = String.Copy(MsgId), _
                .NotContinue = pBlnContinueFlg, _
                .OutputDb = Nothing, _
                .OutputMsg = String.Copy(pStrMsg) _
            }
        End Function

        ' Fields
        Protected Shared lstExMsgDatXML As List(Of ExceptionMsgData)
        Protected SmallTable As String() = New String() { "エラー", "注意", "インフォメーション", "問合せ" }

        ' Nested Types
        Public Class ExceptionMsgData
            ' Fields
            Public MessageId As String
            Public NotContinue As Boolean
            Public OutputDb As Boolean
            Public OutputMsg As String
        End Class

        Protected Enum SMALL
            ' Fields
            [Error] = 1
            Information = 3
            Question = 4
            Warning = 2
        End Enum
    End Class
End Namespace
