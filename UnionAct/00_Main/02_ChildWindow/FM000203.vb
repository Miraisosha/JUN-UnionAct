#Region "FM000203"
'===========================================================================================================
'   クラスＩＤ　　：FM000203
'   クラス名称　　：
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDCommon

Public Class FM000203

#Region "使い方 - 呼出元ですることの流れ"
    '①呼出元で当クラスを生成
    'dim form as FM000203 = New FM000203

    '②印刷（レポート）形式を準備
    'dim resourceObj as CrystalDecisions.CrystalReports.Engine.ReportDocument = New レポートクラス

    '③ボタンの表示形式を数値で設定（①で生成した当フォームのプロパティ）
    'form.ButtonShowType = 1
    ' 1 = [登録＆印刷]　、 [登録のみ]   、[キャンセル]の3種のボタンが表示
    ' 2 = [登録（印刷）]、 [キャンセル]　　　　　　　 の2種のボタンが表示
    ' 3 = [印刷]　　　　、 [キャンセル]　　　　　　　 の2種のボタンが表示

    '④印刷部数項目を表示する場合はPrintCntVisibleプロパティをTrueに設定
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
    ' 4 = [登録（印刷）]がクリックされた
    '
    '⑨印刷を行う場合はPrintOutメソッドを呼ぶ
    'form.PrintOut()
    '
    '⑩
    'form.Dispose()
#End Region

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID = SCREEN_ID_FM000203                                ' FM000203
    Private Const SCREEN_NAME = SCREEN_NAME_FM000203                            ' 印刷プレビュー画面
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    'ボタン初期位置を取得
    Private ReadOnly LOCATION_LEFT1 As System.Drawing.Point = New System.Drawing.Point(315, 643)
    Private ReadOnly LOCATION_LEFT2 As System.Drawing.Point = New System.Drawing.Point(448, 643)
    Private ReadOnly LOCATION_LEFT3 As System.Drawing.Point = New System.Drawing.Point(582, 643)
    'Private ReadOnly LOCATION_LEFT4 As System.Drawing.Point = New System.Drawing.Point(724, 643)
    'Private ReadOnly LOCATION_LEFT5 As System.Drawing.Point = New System.Drawing.Point(862, 643)
    '各オブジェクト位置指定用
    Private ARR_ITEMS_LOCATION As System.Drawing.Point() = {LOCATION_LEFT1, LOCATION_LEFT2, LOCATION_LEFT3}
    Public Const BUTTON_INSERTPRINT_INSERT_CANCEL As Integer = 1
    Public Const BUTTON_INSERTPRINT_CANCEL As Integer = 2
    Public Const BUTTON_PRINT_CANCEL As Integer = 3
    Public Const BUTTON_INSERTPRINT_DISABLE_INSERT_CANCEL As Integer = 4
#End Region

#Region "プロパティ"
    'Public _ObjResource As Object = Nothing     'レポートのリソース
    Public _ObjResource As CrystalDecisions.CrystalReports.Engine.ReportDocument = Nothing 'レポートのリソース
    Public _ButtonShowType = 1                      '表示ボタンの種類
    '(画面呼出前に呼出元に)
    '1 = 登録＆印刷　、登録のみ、キャンセル
    '2 = 登録（印刷）、キャンセル
    '3 = 印刷　　　　、キャンセル
    '4 = 登録＆印刷（⇒使用不可状態)　、登録のみ、キャンセル

    Public _printCntVisible As Boolean = False      '印刷部数項目の表示設定
    Private _IntQlickBtnFlag As Integer = -1        'クリックボタン判別用
    '0 = 登録＆印刷　
    '1 = 登録のみ　
    '2 = キャンセル　
    '3 = 印刷 
    '4 = 登録（印刷）

    Public Property IntQlickBtnFlag() As Integer    'クリックボタン判別用（呼び出し元）
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

    Public Property PrintCntVisible() As Boolean    '印刷部数表示判別用
        Get
            Return _printCntVisible
        End Get
        Set(ByVal value As Boolean)
            _printCntVisible = value
        End Set
    End Property

    Public Property ObjResource() As CrystalDecisions.CrystalReports.Engine.ReportDocument  '印刷データ判別用
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
    '   ＩＤ　：FM000203_Load
    '   名称　：フォームロード
    '   概要  ：
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub FM000203_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
            ElseIf ButtonShowType = BUTTON_INSERTPRINT_CANCEL Then
                SetPropertyControl(Me.btnInsertParenthesisPrint, True, 1)
                SetPropertyControl(Me.btnCancel, True, 3)
            ElseIf ButtonShowType = BUTTON_PRINT_CANCEL Then
                SetPropertyControl(Me.btnPrint, True, 1)
                SetPropertyControl(Me.btnCancel, True, 3)
            ElseIf ButtonShowType = BUTTON_INSERTPRINT_DISABLE_INSERT_CANCEL Then
                SetPropertyControl(Me.btnInsertPrint, True, 1)
                SetPropertyControl(Me.btnInsert, True, 2)
                SetPropertyControl(Me.btnCancel, True, 3)
                Me.btnInsertPrint.Enabled = False
            End If
            '印刷
            If PrintCntVisible = True Then
                Me.lblPrintCount.Visible = True
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
    '   ＩＤ　：FM000203_Load
    '   名称　：クリスタルレポートビューロード
    '   概要  ：
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub crvReportMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles crvReportMain.Load
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If Not ObjResource Is Nothing Then
                Me.crvReportMain.ReportSource = ObjResource
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
    '   ＩＤ　：btnInsertPrint_Click
    '   名称　：登録＆印刷ボタン
    '   概要  ：
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub btnInsertPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertPrint.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            IntQlickBtnFlag = 0
            '呼び出し元によっては即印刷しない画面もありそうなので、
            '印刷実行はPrintOutメソッドを呼び出してもらう
            'Dim fm As FM000204 = New FM000204
            'fm.ShowDialog()
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
    '   ＩＤ　：btnInsert_Click
    '   名称　：登録のみボタン
    '   概要  ：
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somesaki  新規作成
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
    '   ＩＤ　：btnPrint_Click
    '   名称　：キャンセルボタン処理
    '   概要  ：
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
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
        End Try
        ' ログ出力（処理正常終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：印刷ボタン処理
    '   概要  ：
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somesaki  新規作成
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
    '   ＩＤ　：btnInsertParenthesisPrint_Click
    '   名称　：登録（印刷）ボタン処理
    '   概要  ：
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somesaki  新規作成
    '***************************************************************************************************
    Private Sub btnInsertParenthesisPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInsertParenthesisPrint.Click
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            IntQlickBtnFlag = 4
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
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：PrintOut
    '   名称　：印刷をおこなう
    '   概要　：
    '   引数　：Integer（省略可）,Boolean（省略可）,Integer（省略可）,Integer（省略可）
    '   戻り値：なし
    '   作成日：2011/11/18  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18  m.somezaki  新規作成
    '***************************************************************************************************
    ''' <summary>印刷をおこなう</summary>
    ''' <param name="printCnt">印刷部数（省略時=1）</param>
    ''' <param name="collated">部数単位で印刷を行うか（省略時=true）</param>
    ''' <param name="startpage">印刷の最初のページ（省略時=1）</param>
    ''' <param name="endPage">印刷の最後のページ（省略時=最終ページ）</param>
    Public Sub PrintOut(Optional ByVal printCnt As Integer = 1, Optional ByVal collated As Boolean = True, Optional ByVal startpage As Integer = 1, Optional ByVal endPage As Integer = -1)
        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            Dim intNowPage As Integer  '表示ページ位置
            Dim intLastPage As Integer '最終ページ位置

            intNowPage = crvReportMain.GetCurrentPageNumber

            '最終ページ数を取得
            crvReportMain.ShowLastPage()
            intLastPage = crvReportMain.GetCurrentPageNumber

            '前のページに戻す
            crvReportMain.ShowNthPage(intNowPage)

            If startpage > endPage Then
                endPage = intLastPage
            End If

            If PrintCntVisible Then
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

    '#Region "登録＆印刷ボタン（btnInsertPrint）の表示・非表示,位置設定を行う"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetPropertyBtnInsertPrint
    '    '   名称　：指定オブジェクトの表示非表示、位置設定を行う
    '    '   概要　：登録＆印刷（btnInsertPrint）の表示非表示、位置設定をおこなう。
    '    '   引数　：Boolean , Integer(位置フラグ1～3)
    '    '   戻り値：なし
    '    '   作成日：2011/11/17  m.somezaki
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/11/17  m.somezaki  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>登録＆印刷ボタン（btnInsertPrint）の表示・非表示,位置設定を行う</summary>
    '    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    '    ''' <param name="locationNumber">表示位置を1～3で指定（左から1）</param>
    '    ''' <remarks></remarks>
    '    Private Sub SetPropertyBtnInsertPrint(ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
    '        Try
    '            SetPropertyControl(Me.btnInsertPrint, visibleValue, locationNumber)
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM000203, SCREEN_NAME_FM000203, "SetPropertyBtnInsertPrint")
    '        End Try
    '    End Sub
    '#End Region

    '#Region "登録ボタン（btnInsert）の表示・非表示,位置設定を行う"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetPropertyBtnInsertPrint
    '    '   名称　：指定オブジェクトの表示非表示、位置設定を行う
    '    '   概要　：登録ボタン（btnInsert）の表示非表示、位置設定をおこなう。
    '    '   引数　：Boolean , Integer(位置フラグ1～3)
    '    '   戻り値：なし
    '    '   作成日：2011/11/17  m.somezaki
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/11/17  m.somezaki  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>登録ボタン（btnInsert）の表示・非表示,位置設定を行う</summary>
    '    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    '    ''' <param name="locationNumber">表示位置を1～3で指定（左から1）</param>
    '    ''' <remarks></remarks>
    '    Private Sub SetPropertyBtnInsert(ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
    '        Try
    '            SetPropertyControl(Me.btnInsert, visibleValue, locationNumber)
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM000203, SCREEN_NAME_FM000203, "SetPropertyBtnInsert")
    '        End Try
    '    End Sub
    '#End Region

    '#Region "キャンセルボタン（btnCancel）の表示・非表示,位置設定を行う"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetPropertyBtnInsertPrint
    '    '   名称　：指定オブジェクトの表示非表示、位置設定を行う
    '    '   概要　：キャンセルボタン（btnInsert）の表示非表示、位置設定をおこなう。
    '    '   引数　：Boolean , Integer(位置フラグ1～3)
    '    '   戻り値：なし
    '    '   作成日：2011/11/17  m.somezaki
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/11/17  m.somezaki  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>キャンセルボタンの表示・非表示、位置設定を行う</summary>
    '    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    '    ''' <param name="locationNumber">表示位置を1～7で指定（左から1）</param>
    '    ''' <remarks></remarks>
    '    Private Sub SetPropertyBtnCancel(ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
    '        Try
    '            SetPropertyControl(Me.btnCancel, visibleValue, locationNumber)
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '    End Sub
    '#End Region

    '#Region "印刷（登録）（btnInsertParenthesisPrint）の表示・非表示,位置設定を行う"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetPropertyBtnInsertPrint
    '    '   名称　：指定オブジェクトの表示非表示、位置設定を行う
    '    '   概要　：キャンセルボタン（btnInsert）の表示非表示、位置設定をおこなう。
    '    '   引数　：Boolean , Integer(位置フラグ1～3)
    '    '   戻り値：なし
    '    '   作成日：2011/11/17  m.somezaki
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/11/17  m.somezaki  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>印刷（登録）の表示・非表示を行う</summary>
    '    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    '    ''' <param name="locationNumber">表示位置を1～3で指定（左から1）</param>
    '    ''' <remarks></remarks>
    '    Private Sub SetPropertyBtnInsertParenthesisPrint(ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
    '        Try
    '            SetPropertyControl(Me.btnInsertParenthesisPrint, visibleValue, locationNumber)
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '    End Sub
    '#End Region

    '#Region "印刷ボタン（btnPrint）の表示・非表示,位置設定を行う"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetPropertyBtnInsertPrint
    '    '   名称　：指定オブジェクトの表示非表示、位置設定を行う
    '    '   概要　：印刷ボタン（btnInsert）の表示非表示、位置設定をおこなう。
    '    '   引数　：Boolean , Integer(位置フラグ1～3)
    '    '   戻り値：なし
    '    '   作成日：2011/11/17  m.somezaki
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/11/17  m.somezaki  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>画面下部の印刷ボタンの表示・非表示、位置設定を行う</summary>
    '    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    '    ''' <param name="locationNumber">表示位置を1～3で指定（左から1）</param>
    '    ''' <remarks></remarks>
    '    Private Sub SetPropertyBtnPrint(ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
    '        Try
    '            SetPropertyControl(Me.btnPrint, visibleValue, locationNumber)
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '    End Sub
    '#End Region

    '***************************************************************************************************
    '   ＩＤ　：SetVisibleItems
    '   名称　：オブジェクトの表示非表示、位置設定を行う
    '   概要　：引数で渡されたコントロールの表示・非表示、表示位置を設定する。
    '   引数　：setControl, Boolean, Integer(フラグ1～3)
    '   戻り値：なし
    '   作成日：2011/11/17  m.somezaki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/17  m.somezaki  新規作成
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

    'オブジェクト名書くのはめんどくさい？間違いなども多そうなのでコメントアウト
    '#Region "オブジェクトの表示非表示・位置を設定する(個別設定時)"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetVisibleItems
    '    '   名称　：各オブジェクトの表示非表示を行う
    '    '   概要　：呼び出し元で表示するボタンなどが違うため、
    '    '           取得したBooleanの配列を元にオブジェクトの表示非表示の設定をおこなう。
    '    '   引数　：Boolean（）
    '    '   戻り値：なし
    '    '   作成日：2011/11/17  m.somezaki
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/11/17  m.somezaki  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>オブジェクトの表示・非表示を行う</summary>
    '    ''' <param name="objectName">オブジェクト名</param>
    '    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    '    ''' <param name="locationNumber">表示位置を1～7で指定（左から1）</param>
    '    ''' <remarks></remarks>
    '    Public Sub SetItemProperty(ByVal objectName As String, ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
    '        Try
    '            For Each changeControl As Control In ARR_HAVE_SET_VALUE_CHANGE_ITEMS
    '                If changeControl.Name.ToUpper.Trim(objectName.ToUpper.Trim()) Then
    '                    changeControl.Visible = visibleValue
    '                    changeControl.Location = ARR_ITEMS_LOCATION(locationNumber - 1)
    '                End If
    '            Next
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '    End Sub
    '#End Region

    '#Region "印刷部数のラベル（lblPrintCount）の表示・非表示,位置設定を行う"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetPropertyLblPrintCount
    '    '   名称　：オブジェクトの表示非表示、位置設定を行う
    '    '   概要　：ラベル（lblPrintCount）の表示非表示の設定をおこなう。
    '    '   引数　：Boolean , Integer(位置フラグ1～3)
    '    '   戻り値：なし
    '    '   作成日：2011/11/17  m.somezaki
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/11/17  m.somezaki  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>画面下部のラベルの表示・非表示を行う</summary>
    '    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    '    ''' <param name="locationNumber">表示位置を1～3で指定（左から1）</param>
    '    ''' <remarks></remarks>
    '    Public Sub SetPropertyLblPrintCount(ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
    '        Try
    '            Me.
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '    End Sub
    '#End Region

    '#Region "印刷部数（nudPrintCount）の表示・非表示,位置設定を行う"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetPropertyNudPrintCount
    '    '   名称　：オブジェクトの表示非表示、位置設定を行う
    '    '   概要　：ナーメリックアップダウン（nudPrintCount）の表示非表示、位置設定をおこなう。
    '    '   引数　：Boolean , Integer(位置フラグ1～3)
    '    '   戻り値：なし
    '    '   作成日：2011/11/17  m.somezaki
    '    '   更新日：
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：2011/11/17  m.somezaki  新規作成
    '    '***************************************************************************************************
    '    ''' <summary>ナメリックアップダウン（nudPrintCount）の表示・非表示,位置設定を行う</summary>
    '    ''' <param name="visibleValue">表示設定値をtrue,falseで指定</param>
    '    ''' <param name="locationNumber">表示位置を1～3で指定（左から1）</param>
    '    ''' <remarks></remarks>
    '    Public Sub SetPropertyNudPrintCount(ByVal visibleValue As Boolean, ByVal locationNumber As Integer)
    '        Try
    '            SetPropertyControl(Me.nudPrintCount, visibleValue, locationNumber)
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '        End Try
    '    End Sub
    '#End Region
#End Region

End Class

#End Region