Imports CrystalDecisions.CrystalReports.Engine

Namespace GUI.Common
    Public Class ReportViewer
        Inherits FM000203
        ' Methods
        Public Sub New(ByVal dSet As DataSet, ByVal clsReport As ReportClass)
            '①呼出元で当クラスを生成
            'Dim form As New FM000203

            '②印刷（レポート）形式を準備
            'Dim rptObj As ReportDocument = New ReportDocument()

            '③ボタンの表示形式を数値で設定
            Me.ButtonShowType = 3
            ' 1 = [登録＆印刷]　、 [登録のみ]   、[キャンセル]の3種のボタンが表示
            ' 2 = [登録（印刷）]、 [キャンセル]　　　　　　　 の2種のボタンが表示
            ' 3 = [印刷]　　　　、 [キャンセル]　　　　　　　 の2種のボタンが表示

            '④印刷部数項目を表示する場合はPrintCntVisibleプロパティをTrueに設定
            'form.PrintCntVisible = True

            '⑤印刷を行うレポートの形式を設定
            Me.ObjResource = clsReport 'rptObj

            '⑥データセットを設定する
            clsReport.SetDataSource(dSet)
            '
            '⑦フォームを開く
            'Call form.ShowDialog()
            '
            '⑧フォームが閉じられる前にクリックされたボタンを取得
            'dim result as integer = form.IntQlickBtnFlag
            ' 0 = [登録＆印刷]  がクリックされた
            ' 1 = [登録のみ]　  がクリックされた
            ' 2 = [キャンセル]  がクリックされた
            ' 3 = [印刷]        がクリックされた
            ' 4 = [登録（印刷）]がクリックされた
            '
            '⑨印刷を行う場合はPrintOutメソッドを呼ぶ
            'form.PrintOut()
            '
            '⑩
            'form.Dispose()
        End Sub

        'TODO
        Public Sub New(ByVal dSet As DataSet, ByVal clsReport As ReportClass, ByVal type As Integer)
            '①呼出元で当クラスを生成
            'Dim form As New FM000203

            '②印刷（レポート）形式を準備
            'Dim rptObj As ReportDocument = New ReportDocument()

            '③ボタンの表示形式を数値で設定
            Me.ButtonShowType = type
            ' 1 = [登録＆印刷]　、 [登録のみ]   、[キャンセル]の3種のボタンが表示
            ' 2 = [登録（印刷）]、 [キャンセル]　　　　　　　 の2種のボタンが表示
            ' 3 = [印刷]　　　　、 [キャンセル]　　　　　　　 の2種のボタンが表示

            '④印刷部数項目を表示する場合はPrintCntVisibleプロパティをTrueに設定
            Me.PrintCntVisible = True

            '⑤印刷を行うレポートの形式を設定
            Me.ObjResource = clsReport 'rptObj

            '⑥データセットを設定する
            clsReport.SetDataSource(dSet)
            '
            '⑦フォームを開く
            'Call form.ShowDialog()
            '
            '⑧フォームが閉じられる前にクリックされたボタンを取得
            'dim result as integer = form.IntQlickBtnFlag
            ' 0 = [登録＆印刷]  がクリックされた
            ' 1 = [登録のみ]　  がクリックされた
            ' 2 = [キャンセル]  がクリックされた
            ' 3 = [印刷]        がクリックされた
            ' 4 = [登録（印刷）]がクリックされた
            '
            '⑨印刷を行う場合はPrintOutメソッドを呼ぶ
            'form.PrintOut()
            '
            '⑩
            'form.Dispose()
        End Sub

        Public Function ReportViewerShow() As Integer
            Call Me.ShowDialog()
        End Function

        Public Sub Print()
            If Me.IntQlickBtnFlag = 3 Then
                Me.PrintOut()
            End If
        End Sub

        'TODO
        Public Sub RptDataDispose()

        End Sub

        'TODO
        Public Function ConfirmViewerShow(ByVal num As Integer) As Integer
            Call Me.ShowDialog()
            Return Me.IntQlickBtnFlag
        End Function

        'TODO
        Public Function ConfirmViewerShow() As Integer
            Call Me.ShowDialog()
            Return Me.IntQlickBtnFlag
        End Function

        Private Sub ReportViewer_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
            Print()
        End Sub

    End Class
End Namespace
