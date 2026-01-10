#Region "FM000205"
'===========================================================================================================
'   クラスＩＤ　　：FM000205
'   クラス名称　　：印刷プレビュー画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon

Public Class FM000205

#Region "使い方-呼出元ですることの流れ"
    '①呼出元で当クラスを生成
    'dim form as FM000205

    '②印刷（レポート）形式を準備
    'dim resourceObj as CrystalDecisions.CrystalReports.Engine.ReportDocument

    '③ボタンの表示形式を数値で設定
    'form.ButtonShowType = 1
    ' 1 = [登録＆印刷]　、 [登録のみ]   、[キャンセル]の3種のボタンが表示
    ' 2 = [印刷]　　　　、 [キャンセル]　　　　　　　 の2種のボタンが表示

    '③印刷範囲エリアを表示、印刷部数項目を表示する場合は以下の設定をTrueにする
    'form.PrintAreaVisible = True
    'form.PrintCntVisible = True

    '⑤印刷を行うレポートの形式を設定
    'form.ObjResource = resourceObj

    '⑥データセットを設定する
    'resourceObj.SetDataSource(ds)
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
    '
    '⑨印刷を行う場合はPrintOutメソッドを呼ぶ
    'form.PrintOut()
    '
    '⑩
    'form.Dispose()
#End Region

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID = SCREEN_ID_FM000205                                ' FM000205
    Private Const SCREEN_NAME = SCREEN_NAME_FM000205                            ' 印刷プレビュー画面
    ' ログ出力オブジェクト
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' ボタン初期位置を取得
    Private ReadOnly LOCATION_LEFT1 As System.Drawing.Point = New System.Drawing.Point(745, 687)
    Private ReadOnly LOCATION_LEFT2 As System.Drawing.Point = New System.Drawing.Point(885, 687)
    Private ReadOnly LOCATION_LEFT3 As System.Drawing.Point = New System.Drawing.Point(1023, 687)
    ' 各オブジェクト位置指定用
    Private ARR_ITEMS_LOCATION As System.Drawing.Point() = {LOCATION_LEFT1, _
                                                            LOCATION_LEFT2, _
                                                            LOCATION_LEFT3}
    ' 画面表示ボタン切り分け用フラグ
    Public Const BUTTON_INSERTPRINT_INSERT_CANCEL As Integer = 1    '「登録＆印刷」「登録のみ」「キャンセル」
    'Public Const BUTTON_INSERTPRINT_CANCEL As Integer = 2           '「登録（印刷）」「登録のみ」「キャンセル」
    Public Const BUTTON_PRINT_CANCEL As Integer = 2                 '「印刷」「キャンセル」
    Public Const BUTTON_PRINT_DISABLE_INSERT_CANCEL As Integer = 3    '「登録＆印刷」（使用不能）「登録のみ」「キャンセル」
#End Region

#Region "プロパティ"
    Private _ObjResource As CrystalDecisions.CrystalReports.Engine.ReportDocument = Nothing 'レポートのリソース

    Private _ButtonShowType = 1                  '表示ボタンの種類
    '1 = 登録＆印刷　、登録のみ  、キャンセル
    '2 = 印刷　　　　、キャンセル
    '3 = 登録＆印刷(⇒使用不可能)　、登録のみ  、キャンセル
    Private _printAreaVisible As Boolean = False '印刷範囲項目の表示設定
    Private _printCntVisible As Boolean = False  '印刷部数項目の表示設定
    Private _IntQlickBtnFlag As Integer = -1    'クリックボタン判別用
    '0 = 登録＆印刷　
    '1 = 登録のみ　
    '2 = キャンセル　
    '3 = 印刷
    Public Property IntQlickBtnFlag() As Integer    'クリックしたボタン判別用
        Get
            Return _IntQlickBtnFlag
        End Get
        Set(ByVal value As Integer)
            _IntQlickBtnFlag = value
        End Set
    End Property

    Public Property ButtonShowType() As Integer     '表示ボタン判別用
        Get
            Return _ButtonShowType
        End Get
        Set(ByVal value As Integer)
            _ButtonShowType = value
        End Set
    End Property

    Public Property PrintAreaVisible() As Boolean   '印刷範囲の表示判別用
        Get
            Return _printAreaVisible
        End Get
        Set(ByVal value As Boolean)
            _printAreaVisible = value
        End Set
    End Property

    Public Property PrintCntVisible() As Boolean    '印刷部数の表示判別用
        Get
            Return _printCntVisible
        End Get
        Set(ByVal value As Boolean)
            _printCntVisible = value
        End Set
    End Property

    Public Property ObjResource() As CrystalDecisions.CrystalReports.Engine.ReportDocument  '印刷データのオブジェクト
        Get
            Return _ObjResource
        End Get
        Set(ByVal value As CrystalDecisions.CrystalReports.Engine.ReportDocument)
            _ObjResource = value
        End Set
    End Property

#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM000205_Load
    '   名称　：フォームロード
    '   概要  ：
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub FM000205_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If
            'ボタンの表示タイプ
            If ButtonShowType = BUTTON_INSERTPRINT_INSERT_CANCEL Then
                SetPropertyControl(Me.btnInsertPrint, True, 1)
                SetPropertyControl(Me.btnInsert, True, 2)
                SetPropertyControl(Me.btnCancel, True, 3)
            ElseIf ButtonShowType = BUTTON_PRINT_CANCEL Then
                SetPropertyControl(Me.btnPrint, True, 1)
                SetPropertyControl(Me.btnCancel, True, 2)
            ElseIf ButtonShowType = BUTTON_PRINT_DISABLE_INSERT_CANCEL Then
                SetPropertyControl(Me.btnInsertPrint, True, 1)
                SetPropertyControl(Me.btnInsert, True, 2)
                SetPropertyControl(Me.btnCancel, True, 3)
                Me.btnInsertPrint.Enabled = False
            End If
            '印刷範囲項目
            If PrintAreaVisible Then
                Me.grpPrintArea.Visible = True
            End If
            '印刷部数
            If PrintCntVisible Then
                Me.lblPrintCnt.Visible = True
                Me.nudPrintCount.Visible = True
            End If
            'ページ全体
            Me.crvReportMain.Zoom(2)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            'カーソルを矢印に戻す
            Cursor.Current = Cursors.Default
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：crvReportMain_Load
    '   名称　：クリスタルレポートロード
    '   概要  ：
    '   作成日：2011/11/21  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub crvReportMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles crvReportMain.Load
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If Not ObjResource Is Nothing Then
                Me.crvReportMain.ReportSource = ObjResource
            End If

            Dim intNowPage As Integer   '現在ページ
            Dim intLastPage As Integer  '最終ページ
            intNowPage = crvReportMain.GetCurrentPageNumber
            '最終ページ数を取得（最終ページ移動してカレントページ取得）
            crvReportMain.ShowLastPage()
            intLastPage = crvReportMain.GetCurrentPageNumber
            'ページ処理前に戻す
            crvReportMain.ShowNthPage(intNowPage)
            '▲最終ページ取得処理ここまで
            Me.nudPageFrom.Value = 1
            Me.nudPageFrom.Maximum = intLastPage
            Me.nudPageTo.Value = intLastPage
            Me.nudPageTo.Maximum = intLastPage
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnInsertPrint_Click
    '   名称　：登録＆印刷ボタン
    '   概要  ：
    '   作成日：2011/11/18  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub btnInsertPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertPrint.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            IntQlickBtnFlag = 0
            Me.Close()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnInsertPrint_Click
    '   名称　：登録のみボタン
    '   概要  ：
    '   作成日：2011/11/18  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub btnInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsert.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            IntQlickBtnFlag = 1
            Me.Close()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnInsertPrint_Click
    '   名称　：キャンセルボタン
    '   概要  ：
    '   作成日：2011/11/18  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            IntQlickBtnFlag = 2
            Me.Close()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Me.Close()
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnInsertPrint_Click
    '   名称　：印刷ボタン
    '   概要  ：
    '   作成日：2011/11/18  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            IntQlickBtnFlag = 3
            Me.Close()
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：nudPageFrom_ValueChanged
    '   名称　：印刷開始ページ入力
    '   概要  ：印刷終了の最小設定値を印刷開始ページの入力値とする
    '   作成日：2011/11/18  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub nudPageFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudPageFrom.ValueChanged
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If Me.nudPageFrom.Minimum < Me.nudPageTo.Value Then
                Me.nudPageTo.Minimum = Me.nudPageFrom.Value
            End If
            If Me.nudPageFrom.Value > Me.nudPageTo.Value Then
                Me.nudPageTo.Value = Me.nudPageFrom.Value
                Me.nudPageTo.Minimum = Me.nudPageFrom.Value
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：optPageSet_CheckedChanged
    '   名称　：ページ指定オプションボタンチェンジ処理
    '   概要  ：印刷終了の最小設定値を印刷開始ページの入力値とする
    '   作成日：2011/11/18  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub optPageSet_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles optPageSet.CheckedChanged
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If Me.optPageSet.Checked Then
                Me.nudPageTo.Enabled = True
                Me.nudPageFrom.Enabled = True
            Else
                Me.nudPageTo.Enabled = False
                Me.nudPageFrom.Enabled = False
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：SetPropertyControl
    '   名称　：オブジェクトの表示非表示、位置設定を行う
    '   概要　：引数で渡されたコントロールの表示・非表示、表示位置を設定する。
    '   引数　：setControl, Boolean, Integer(フラグ1～3)
    '   戻り値：なし
    '   作成日：2011/11/17  somezaki
    '***************************************************************************************************
    ''' <summary>引数で渡されたコントロールの表示・非表示、位置を設定する</summary>
    ''' <param name="setControl">表示設定値をtrue,falseで指定</param>
    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    ''' <param name="locationNumber">表示設定値をtrue,falseで指定</param>
    ''' <remarks></remarks> 
    Private Sub SetPropertyControl(ByVal setControl As Control, ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If locationNumber < 1 Or locationNumber > ARR_ITEMS_LOCATION.Length Then
                locationNumber = 1
            End If
            setControl.Visible = visibleValue
            setControl.Location = ARR_ITEMS_LOCATION((locationNumber) - 1)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：PrintOut
    '   名称　：印刷処理
    '   概要　：印刷を行う。
    '           部数省略時は部数入力欄が表示状態であれば「入力値」とし、非表示の場合は「1」
    '           部単位印刷の省略時は「true」
    '           開始ページ省略時は印刷範囲の指定がある場合は「指定値」とし、それ以外は「1」
    '           終了ページ省略時は印刷範囲の指定がある場合は「指定値」とし、それ以外は「最後のページ」
    '   引数　：Integer（省略可）,Boolean（省略可）,Integer（省略可）,Integer（省略可）
    '   戻り値：なし
    '   作成日：2011/11/18  somezaki
    '***************************************************************************************************
    ''' <summary>印刷処理</summary>
    ''' <param name="printCnt">印刷部数（省略時=1）</param>
    ''' <param name="collated">部数単位で印刷を行うか（省略時=true）</param>
    ''' <param name="startpage">印刷の最初のページ（省略時=[1]or[指定値]）</param>
    ''' <param name="endPage">印刷の最後のページ（省略時=[最終ページ]or[指定値]）</param>
    ''' <remarks></remarks>
    Public Sub PrintOut(Optional ByVal printCnt As Integer = 1, Optional ByVal collated As Boolean = True, Optional ByVal startpage As Integer = 1, Optional ByVal endPage As Integer = -1)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            '印刷範囲取得
            If startpage > endPage Then '印刷終了ページ未指定かチェック
                If PrintAreaVisible Then        'ページ範囲の表示有無
                    If optPageSet.Checked Then  'ページ数指定の有無
                        startpage = Me.nudPageFrom.Value
                        endPage = Me.nudPageTo.Value
                    End If
                End If

                If startpage > endPage Then     'ページ指定がない場合、この条件がTrue
                    '▼最終ページ取得処理開始
                    Dim intNowPage As Integer  '表示ページ位置
                    Dim intLastPage As Integer '最終ページ位置
                    intNowPage = crvReportMain.GetCurrentPageNumber

                    '最終ページ数を取得（最終ページ移動してカレントページ取得）
                    crvReportMain.ShowLastPage()
                    intLastPage = crvReportMain.GetCurrentPageNumber

                    'ページ処理前に戻す
                    crvReportMain.ShowNthPage(intNowPage)
                    '▲最終ページ取得処理ここまで
                    endPage = intLastPage
                End If
            End If
            If PrintCntVisible Then     '印刷部数表示あり
                printCnt = Me.nudPrintCount.Value
            End If
            ObjResource.PrintToPrinter(printCnt, collated, startpage, endPage)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

End Class
#End Region