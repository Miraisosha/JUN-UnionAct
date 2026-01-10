#Region "UC999999"
'===========================================================================================================
'   クラスＩＤ　　：UC999999
'   クラス名称　　：複数エラー表示画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon

Public Class UC999999

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC999999          ' UC999999
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC999999      ' 複数エラーメッセージ表示画面
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "プロパティ"
    Private _errMsgList As ArrayList = New ArrayList                ' エラーリスト

    ' エラーリスト
    Public Property errMsgList() As ArrayList
        Get
            Return _errMsgList
        End Get
        Set(ByVal value As ArrayList)
            _errMsgList = value
        End Set
    End Property
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：UC999999_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/07(金)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub UC999999_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.txtErr.Clear()                                       ' メッセージ内容クリア
            If SetFormCenter(Me) = False Then                       ' 画面中央表示処理
                Exit Sub
            End If
            For Each errMsg As String In errMsgList                 ' メッセージ表示
                If Me.txtErr.Text = "" Then
                    Me.txtErr.Text = errMsg
                Else
                    Me.txtErr.Text = Me.txtErr.Text & vbCrLf & errMsg
                End If
            Next
            Me.txtErr.ForeColor = Color.Black
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnOk_Click
    '   名称　：OKボタンクリック処理
    '   概要　：
    '   作成日：2011/11/07(月) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(月) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            Me.Dispose()                                            ' 画面破棄
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region
End Class

#End Region
