#Region "UCInfoMsg"
'===========================================================================================================
'   クラスＩＤ　　：UCInfoMsg
'   クラス名称　　：複数の注意メッセージ表示画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon

Public Class UCInfoMsg

#Region "プロパティ"
    Private _msgList As ArrayList = New ArrayList    ' メッセージリスト

    ' メッセージリスト
    Public Property msgList() As ArrayList
        Get
            Return _msgList
        End Get
        Set(ByVal value As ArrayList)
            _msgList = value
        End Set
    End Property
#End Region

#Region "イベント"
#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UCInfoMsg_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/12/06(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/06(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub UCInfoMsg_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            ' メッセージ内容クリア
            Me.txtMsg.Clear()

            ' 画面中央表示処理
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If

            ' メッセージ表示
            For Each msg As String In msgList
                If Me.txtMsg.Text = "" Then
                    Me.txtMsg.Text = msg
                Else
                    Me.txtMsg.Text = Me.txtMsg.Text & vbCrLf & msg
                End If
            Next
            txtMsg.ForeColor = Color.Black

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UCInfoMsg, SCREEN_NAME_UCInfoMsg, "UCInfoMsg_Load")
        End Try

    End Sub
#End Region

#Region "OKボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnOk_Click
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/12/06(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/06(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            ' 画面破棄
            Me.Dispose()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UCInfoMsg, SCREEN_NAME_UCInfoMsg, "btnOk_Click")
        End Try

    End Sub
#End Region
#End Region


End Class
#End Region
