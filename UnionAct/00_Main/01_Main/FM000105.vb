#Region "FM000105"
'===========================================================================================================
'   クラスＩＤ　　：FM000105
'   クラス名称　　：エラーメッセージ画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon
Imports System.Windows.Forms

Public Class FM000105

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM000105                     ' FM000105
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM000105                 ' エラーメッセージ画面
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000105_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM000105_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            If SetFormCenter(Me) = False Then                                           ' 画面中央表示処理
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnYes_Click
    '   名称　：はいボタンクリック処理
    '   概要  ：
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnYes_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnYes.Click
        Try
            Me.DialogResult = System.Windows.Forms.DialogResult.Yes
            Me.Close()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnNo_Click
    '   名称　：いいえボタンクリック処理
    '   概要  ：
    '   作成日：2011/11/17(木)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17(木)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNo.Click
        Try
            Me.DialogResult = System.Windows.Forms.DialogResult.No
            Me.Close()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
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
