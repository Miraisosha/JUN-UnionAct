Imports System.Runtime.InteropServices

Namespace My
    ' 次のイベントは MyApplication に対して利用できます:
    ' Startup:アプリケーションが開始されたとき、スタートアップ フォームが作成される前に発生します。
    ' Shutdown:アプリケーション フォームがすべて閉じられた後に発生します。このイベントは、アプリケーションが異常終了したときには発生しません。
    ' UnhandledException:ハンドルされない例外がアプリケーションで発生したときに発生します。
    ' StartupNextInstance:単一インスタンス アプリケーションが起動され、それが既にアクティブであるときに発生します。 
    ' NetworkAvailabilityChanged:ネットワーク接続が接続されたとき、または切断されたときに発生します。
    Partial Friend Class MyApplication


        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

            ''メッセージボックスでNoを選択すると、アプリケーションを終了
            ''この場合、Shutdownイベントは発生しない
            'If MsgBox("アプリケーションを開始しますか？", MsgBoxStyle.YesNo) =
            '        MsgBoxResult.No Then
            '    e.Cancel = True
            'End If

            ''指定されたコマンドライン引数を列挙
            'For Each cmd As String In e.CommandLine
            '    Console.WriteLine(cmd)
            'Next
            App.Start()



        End Sub

        Private Sub MyApplication_Shutdown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shutdown
            App.Shutdown()
        End Sub
    End Class
End Namespace
