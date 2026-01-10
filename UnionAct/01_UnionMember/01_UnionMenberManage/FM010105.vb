#Region "FM010105"
'===========================================================================================================
'   クラスＩＤ　　：FM010105
'   クラス名称　　：基準日入力
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo

Public Class FM010105

#Region "定数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_FM010105                              ' FM010105
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM010105                          ' 基準日入力画面
    ' log4net
    Private ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
    '***************************************************************************************************
    '   ＩＤ　：FM010105_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub FM010105_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            '---------------------------------------------------
            '   コントロールクリア処理
            '---------------------------------------------------
            If ControlClear() = False Then
                Exit Sub
            End If
            '---------------------------------------------------
            '   画面中央表示処理
            '---------------------------------------------------
            If SetFormCenter(Me) = False Then
                Exit Sub
            End If
            '---------------------------------------------------
            '   各データ取得処理
            '---------------------------------------------------
            If GetData() = False Then
                Exit Sub
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
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnPritntPreview_Click
    '   名称　：印刷プレビューボタンクリック処理
    '   概要　：
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnPritntPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPritntPreview.Click
        Try
            ' 印刷プレビュー処理
            If PrintPreview() Then
                Exit Sub
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
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
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
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboYear_SelectionChangeCommitted
    '   名称　：年コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：2017/10/23(月)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '         ：2017/10/23(月)  y.fujisaku  印刷プレビューボタン活性制御追加
    '***************************************************************************************************
    Private Sub cboYear_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYear.SelectionChangeCommitted
        Try
            '---------------------------------------------------
            '   コンボボックスクリア
            '---------------------------------------------------
            Me.cboMonth.DataSource = Nothing
            Me.cboDay.DataSource = Nothing
            '---------------------------------------------------
            '   コンボボックス（月）作成処理
            '---------------------------------------------------
            If CreateCboMonth(Me.cboMonth, Me.cboYear.Text) = False Then
                Exit Sub
            End If
            '---------------------------------------------------
            '   印刷プレビューボタン非活性化
            '---------------------------------------------------
            Me.btnPritntPreview.Enabled = False
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
    '   ＩＤ　：cboMonth_SelectionChangeCommitted
    '   名称　：月コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：2017/10/23(月)  y.fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '         ：2017/10/23(月)  y.fujisaku  印刷プレビューボタン活性制御追加
    '***************************************************************************************************
    Private Sub cboMonth_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMonth.SelectionChangeCommitted
        Try
            '---------------------------------------------------
            '   コンボボックス（日）作成処理
            '---------------------------------------------------
            If CreateCboDay(Me.cboDay, Me.cboYear.Text, Me.cboMonth.Text) = False Then
                Exit Sub
            End If
            '---------------------------------------------------
            '   印刷プレビューボタン非活性化
            '---------------------------------------------------
            Me.btnPritntPreview.Enabled = False
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
    '   ＩＤ　：cboDay_SelectionChangeCommitted
    '   名称　：日コンボボックスチェンジ処理
    '   概要　：
    '   作成日：2017/10/23(月)  y.fujisaku
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2017/10/23(月)  y.fujisaku  新規作成
    '***************************************************************************************************
    Private Sub cboDay_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboDay.SelectionChangeCommitted
        Try
            '---------------------------------------------------
            '   印刷プレビューボタン活性化処理
            '---------------------------------------------------
            If Me.cboDay.Text = "" Then
                Me.btnPritntPreview.Enabled = False
            Else
                Me.btnPritntPreview.Enabled = True
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
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：ControlClear
    '   名称　：コントロールクリア処理
    '   概要  ：各コントロールをクリアする。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/30(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/07(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールクリア処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlClear() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Try
            ' ComboBox
            Me.cboYear.Text = ""                                                            ' 年
            Me.cboYear.Items.Clear()
            Me.cboMonth.Text = ""                                                           ' 月
            Me.cboMonth.Items.Clear()
            Me.cboDay.Text = ""                                                             ' 日
            Me.cboDay.Items.Clear()
            ' Button
            Me.btnPritntPreview.Visible = True                                              ' 印刷プレビューボタン
            Me.btnCancel.Visible = True                                                     ' キャンセルボタン
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：組合員種別コンボボックス作成処理を呼び出す。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス生成
        Dim intYear As Integer = Nothing                                                    ' 年
        Dim intMonth As Integer = Nothing                                                   ' 月
        Dim intDay As Integer = Nothing                                                     ' 日
        Try
            Call clsDb.Connect()                                                            ' データベース接続
            '---------------------------------------------------
            '   コンボボックス（年）作成処理
            '---------------------------------------------------
            If CreateCboYear(Me.cboYear) = False Then
                Exit Function
            End If
            intYear = CInt(Now.ToString("yyyy"))
            Me.cboYear.Text = intYear.ToString()
            '---------------------------------------------------
            '   コンボボックス（月）作成処理
            '---------------------------------------------------
            If CreateCboMonth(Me.cboMonth, Me.cboYear.Text) = False Then
                Exit Function
            End If
            intMonth = CInt(Now.ToString("MM"))
            Me.cboMonth.Text = intMonth.ToString()
            '---------------------------------------------------
            '   コンボボックス（日）作成処理
            '---------------------------------------------------
            If CreateCboDay(Me.cboDay, Me.cboYear.Text, Me.cboMonth.Text) = False Then
                Exit Function
            End If
            intDay = CInt(Now.ToString("dd"))
            Me.cboDay.Text = intDay.ToString()
            blnRet = True                                                                   ' 処理結果に正常を格納
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()                                                         ' データベース切断
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateCboYear
    '   名称　：コンボボックス（年）作成処理
    '   概要　：引数の情報でコンボボックスのリストを作成する。
    '   引数　：ByVal pCbo   As System.Windows.Forms.ComboBox = コンボボックス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：2017/10/23(月)  y.fujisaku
    '   備考  ：MDConstにDropDownStyleの定数あります。
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function CreateCboYear(ByVal pCbo As System.Windows.Forms.ComboBox) As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim dtRet As DataTable = Nothing                                                ' データテーブル
        Dim drRet As DataRow = Nothing                                                  ' データロー
        Dim dtBlank As DataRow = Nothing                                                ' 空白データロー
        Dim intMin As Integer = Nothing                                                 ' 最小年
        Dim intMax As Integer = Nothing                                                 ' 最大年
        Try
            ' 初期処理
            intMin = 1970                                                               ' 最小年
            intMax = CInt(Now.ToString("yyyy")) + 1                                     ' 最大年
            pCbo.BeginUpdate()                                                          ' チラつき防止の為、最後まで描写しない
            pCbo.DataSource = Nothing                                                   ' データソース初期化
            pCbo.Items.Clear()                                                          ' コンボボックスリストクリア
            ' データテーブル・データロー生成
            dtRet = New DataTable
            dtRet.Columns.Add("YearValue", GetType(Integer))
            dtRet.Columns.Add("YearDisplay", GetType(String))
            ' 先頭行に空白追加
            dtBlank = dtRet.NewRow()
            dtRet.Rows.InsertAt(dtBlank, 0)
            For i = intMin To intMax
                drRet = dtRet.NewRow()                                                  ' 新しいデータロー作成
                drRet(0) = i                                                            ' 値
                drRet(1) = i.ToString()                                                 ' 表示
                dtRet.Rows.Add(drRet)                                                   ' データ追加
            Next
            ' コンボボックス各設定
            pCbo.DropDownStyle = MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST          ' テキスト編集不可
            pCbo.DataSource = dtRet                                                     ' データソース設定
            pCbo.DisplayMember = "YearValue"                                            ' コンボボックス名称設定
            pCbo.ValueMember = "YearDisplay"                                            ' コンボボックス値設定
            pCbo.SelectedIndex = 0                                                      ' 先頭データ選択
            blnRet = True                                                               ' 処理結果に正常を格納
        Catch ex As Exception
            pCbo.DataSource = Nothing                                                   ' コンボボックスデータソース削除
            pCbo.Items.Clear()                                                          ' コンボボックスリストクリア
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.cboMonth.EndUpdate()                                                     ' チラつき防止の為、最後に描写する
        End Try
        Return blnRet                                                                   ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateCboMonth
    '   名称　：コンボボックス（月）作成処理
    '   概要　：引数の情報でコンボボックスのリストを作成する。
    '   引数　：ByVal pCbo    As System.Windows.Forms.ComboBox = 月コンボボックス
    '           ByVal strYear As String                        = 年コンボボックステキスト
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '   備考  ：MDConstにDropDownStyleの定数あります。
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function CreateCboMonth(ByVal pCbo As System.Windows.Forms.ComboBox, _
                                    ByVal strYear As String) As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim dtRet As DataTable = Nothing                                                    ' データテーブル
        Dim drRet As DataRow = Nothing                                                      ' データロー
        Dim dtBlank As DataRow = Nothing                                                    ' 空白データロー
        Dim intStart As Integer = 0                                                         ' 開始月
        Dim intEnd As Integer = 0                                                           ' 終了月
        Try
            ' 初期処理
            pCbo.BeginUpdate()                                                              ' チラつき防止の為、最後まで描写しない
            pCbo.DataSource = Nothing                                                       ' データソース初期化
            pCbo.Items.Clear()                                                              ' コンボボックスリストクリア
            intStart = 1                                                                    ' 開始月
            intEnd = 12                                                                     ' 終了月
            ' 年コンボボックスが選択されているかチェック
            If strYear.Length <> 0 Then
                ' データテーブル・データロー生成
                dtRet = New DataTable
                dtRet.Columns.Add("MonthValue", GetType(Integer))
                dtRet.Columns.Add("MonthDisplay", GetType(String))
                ' 先頭行に空白追加
                dtBlank = dtRet.NewRow()
                dtRet.Rows.InsertAt(dtBlank, 0)
                For i = intStart To intEnd
                    drRet = dtRet.NewRow()                                                  ' 新しいデータロー作成
                    drRet(0) = i                                                            ' 値
                    drRet(1) = i.ToString().PadLeft(2, "0")                                 ' 表示
                    dtRet.Rows.Add(drRet)                                                   ' データ追加
                Next
                ' コンボボックス各設定
                pCbo.DropDownStyle = MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST          ' テキスト編集不可
                pCbo.DataSource = dtRet                                                     ' データソース設定
                pCbo.DisplayMember = "MonthValue"                                           ' コンボボックス名称設定
                pCbo.ValueMember = "MonthDisplay"                                           ' コンボボックス値設定
                pCbo.SelectedIndex = 0                                                      ' 先頭データ選択
            End If
            blnRet = True                                                                   ' 処理結果に正常を格納
        Catch ex As Exception
            pCbo.DataSource = Nothing                                                       ' コンボボックスデータソース削除
            pCbo.Items.Clear()                                                              ' コンボボックスリストクリア
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.cboMonth.EndUpdate()                                                         ' チラつき防止の為、最後に描写する
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateCboDay
    '   名称　：コンボボックス（日）作成処理
    '   概要　：引数の情報でコンボボックスのリストを作成する。
    '   引数　：ByVal pCbo      As System.Windows.Forms.ComboBox = コンボボックス
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '   備考  ：MDConstにDropDownStyleの定数あります。
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '***************************************************************************************************
    Private Function CreateCboDay(ByVal pCbo As System.Windows.Forms.ComboBox, _
                                  ByVal pStrYear As String, _
                                  ByVal pStrMonth As String) As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim dtRet As DataTable = Nothing                                                ' データテーブル
        Dim drRet As DataRow = Nothing                                                  ' データロー
        Dim dtBlank As DataRow = Nothing                                                ' 空白データロー
        Dim intLastDay As Integer = Nothing                                             ' 月末日
        Try
            ' 初期処理
            pCbo.BeginUpdate()                                                          ' チラつき防止の為、最後まで描写しない
            pCbo.DataSource = Nothing                                                   ' データソース初期化
            pCbo.Items.Clear()                                                          ' コンボボックスリストクリア
            If (pStrYear.Length <> 0) And (pStrMonth.Length <> 0) Then
                ' 月末日取得
                intLastDay = Date.DaysInMonth(CInt(pStrYear), CInt(pStrMonth))
                ' データテーブル・データロー生成
                dtRet = New DataTable
                dtRet.Columns.Add("DayValue", GetType(Integer))
                dtRet.Columns.Add("DayDisplay", GetType(String))
                ' 先頭行に空白追加
                dtBlank = dtRet.NewRow()
                dtRet.Rows.InsertAt(dtBlank, 0)
                For i = 1 To intLastDay
                    drRet = dtRet.NewRow()                                              ' 新しいデータロー作成
                    drRet(0) = i                                                        ' 値
                    drRet(1) = i.ToString()                                             ' 表示
                    dtRet.Rows.Add(drRet)                                               ' データ追加
                Next
                ' コンボボックス各設定
                pCbo.DropDownStyle = MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWNLIST      ' テキスト編集不可
                pCbo.DataSource = dtRet                                                 ' データソース設定
                pCbo.DisplayMember = "DayValue"                                         ' コンボボックス名称設定
                pCbo.ValueMember = "DayDisplay"                                         ' コンボボックス値設定
                pCbo.SelectedIndex = 0                                                  ' 先頭データ選択
            End If
            blnRet = True                                                               ' 処理結果に正常を格納
        Catch ex As Exception
            pCbo.DataSource = Nothing                                                   ' コンボボックスデータソース削除
            pCbo.Items.Clear()                                                          ' コンボボックスリストクリア
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.cboMonth.EndUpdate()                                                     ' チラつき防止の為、最後に描写する
        End Try
        Return blnRet                                                                   ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：PrintPreview
    '   名称　：印刷プレビュー処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火)  m.suzuki  新規作成
    '         ：2012/08/15(水)  Fujisaku 基準日が正しくyyyyMMdd型になるように修正
    '         ：2012/11/01(木)　Fujisaku 機長・教官機長に仕様変更、機関士の差分表示異常も修正
    '***************************************************************************************************
    ''' <summary>印刷プレビュー処理</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PrintPreview() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim fmPrint As FM000203 = Nothing                                                   ' プレビュークラス
        Dim ds As DS0101P1 = Nothing                                                        ' 帳票用データセット
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument = Nothing    ' レポートドキュメントオブジェクト
        Dim intRet As Integer = Nothing                                                     ' プレビュー画面処理結果
        Dim drHeaderT As DS0101P1.dtHeaderTRow = Nothing                                    ' 東京データロー
        Dim drHeaderO As DS0101P1.dtHeaderORow = Nothing                                    ' 大阪データロー
        Dim drHeaderE As DS0101P1.dtHeaderERow = Nothing                                    ' その他データロー
        Dim drFooter As DS0101P1.dtFooterRow = Nothing                                      ' フッター
        Dim dateTargetDate As Date                                                          ' 抽出対象日
        ' 今期
        Dim strTargetDateNew As String = ""                                                 ' 抽出対象日（前年同月日）
        Dim dtNew As DataTable = Nothing                                                    ' SQL実行結果データテーブル
        Dim strSqlNew As String = Nothing                                                   ' SQL今期
        Dim intCntNew As Integer = Nothing                                                  ' SQL実行結果件数
        ' 前期
        Dim strTargetDatePre As String = ""                                                 ' 抽出対象日
        Dim dtPre As DataTable = Nothing                                                    ' SQL実行結果データテーブル
        Dim strSqlPre As String = Nothing                                                   ' SQL今期
        Dim intCntPre As Integer = Nothing                                                  ' SQL実行結果件数
        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソル砂時計
            ' 抽出対象日取得
            dateTargetDate = Date.Parse(Me.cboYear.Text & "/" & Me.cboMonth.Text & "/" & Me.cboDay.Text)
            strTargetDateNew = Format(dateTargetDate, "yyyyMMdd")                           ' 抽出対象日（今期）
            strTargetDatePre = Format(dateTargetDate.AddYears(-1), "yyyyMMdd")              ' 抽出対象日（前期）            '-------------------------------------------------------------------------------
            '   SQL作成（今期）
            '-------------------------------------------------------------------------------
            strSqlNew = ""
            strSqlNew = strSqlNew & " SELECT staf_attribute.k_belonging                                            AS k_belonging" & vbCrLf          ' 所属支部（組合支部）
            '                           支部名称取得
            strSqlNew = strSqlNew & "       ,(SELECT constant_dtl.l_name" & vbCrLf
            strSqlNew = strSqlNew & "           FROM constant_dtl" & vbCrLf
            strSqlNew = strSqlNew & "          WHERE constant_dtl.c_constant = 'BELONGING'" & vbCrLf
            strSqlNew = strSqlNew & "            AND constant_dtl.c_constant_seq = staf_attribute.k_belonging)     AS k_belonging_name" & vbCrLf
            strSqlNew = strSqlNew & "       ,staf_attribute.k_qualification                                        AS k_qualification" & vbCrLf      ' 乗務資格
            '                           乗務資格名称取得
            strSqlNew = strSqlNew & "       ,(SELECT constant_dtl.l_name" & vbCrLf                                                                   ' 乗務資格名称
            strSqlNew = strSqlNew & "           FROM constant_dtl" & vbCrLf
            strSqlNew = strSqlNew & "          WHERE constant_dtl.c_constant = 'QUALIFICATION'" & vbCrLf
            strSqlNew = strSqlNew & "            AND constant_dtl.c_constant_seq = staf_attribute.k_qualification) AS k_qualification_name" & vbCrLf
            strSqlNew = strSqlNew & "       ,COUNT(*)                                                              AS cnt" & vbCrLf                  ' 件数
            strSqlNew = strSqlNew & "   FROM staf_attribute" & vbCrLf
            '                          基準日以下で最新のもの
            strSqlNew = strSqlNew & "        INNER JOIN ( SELECT staf_attribute.c_user_id" & vbCrLf
            strSqlNew = strSqlNew & "                           ,staf_attribute.c_ksh" & vbCrLf
            strSqlNew = strSqlNew & "                           ,staf_attribute.c_staf_id" & vbCrLf
            strSqlNew = strSqlNew & "                           ,MAX(staf_attribute.d_from) AS max_d_from" & vbCrLf
            strSqlNew = strSqlNew & "                       FROM staf_attribute" & vbCrLf
            strSqlNew = strSqlNew & "                      WHERE staf_attribute.d_from <= '" & strTargetDateNew & "'" & vbCrLf
            strSqlNew = strSqlNew & "                      GROUP BY staf_attribute.c_user_id" & vbCrLf
            strSqlNew = strSqlNew & "                              ,staf_attribute.c_ksh" & vbCrLf
            strSqlNew = strSqlNew & "                              ,staf_attribute.c_staf_id ) AS new_staf_attribute" & vbCrLf
            strSqlNew = strSqlNew & "        ON staf_attribute.c_user_id = new_staf_attribute.c_user_id" & vbCrLf
            strSqlNew = strSqlNew & "        AND staf_attribute.c_ksh = new_staf_attribute.c_ksh" & vbCrLf
            strSqlNew = strSqlNew & "        AND staf_attribute.c_staf_id = new_staf_attribute.c_staf_id" & vbCrLf
            strSqlNew = strSqlNew & "        AND staf_attribute.d_from = new_staf_attribute.max_d_from" & vbCrLf
            '                          組合員ステータス区分が '01'：加入
            strSqlNew = strSqlNew & "  WHERE staf_attribute.k_user_status = '" & USER_STATUS_ENTRY & "'" & vbCrLf
            '                          組合員種別コードが '01':正組合員か '02':シニア組合員, '04':準組合員
            strSqlNew = strSqlNew & "    AND ( staf_attribute.k_staf_kind = '" & STAF_KIND_REGULAR & "'" & vbCrLf
            strSqlNew = strSqlNew & "        OR staf_attribute.k_staf_kind = '" & STAF_KIND_SENIOR & "'" & vbCrLf
            strSqlNew = strSqlNew & "        OR staf_attribute.k_staf_kind = '" & STAF_KIND_SEMI & "' )" & vbCrLf
            '                          削除区分が '0'
            strSqlNew = strSqlNew & "    AND staf_attribute.k_del = '0'" & vbCrLf
            '                          所属支部（組合支部）と乗務資格で集約
            strSqlNew = strSqlNew & "  GROUP BY staf_attribute.k_belonging" & vbCrLf
            strSqlNew = strSqlNew & "          ,staf_attribute.k_qualification" & vbCrLf
            strSqlNew = strSqlNew & "  ORDER BY staf_attribute.k_belonging" & vbCrLf
            strSqlNew = strSqlNew & "          ,staf_attribute.k_qualification" & UtDb.DbOrderOffset & vbCrLf
            strSqlNew = strSqlNew & ";" & vbCrLf
            '-------------------------------------------------------------------------------
            '   SQL作成（前期）
            '-------------------------------------------------------------------------------
            strSqlPre = ""
            strSqlPre = strSqlPre & " SELECT staf_attribute.k_belonging                                            AS k_belonging" & vbCrLf          ' 所属支部（組合支部）
            '                           支部名称取得
            strSqlPre = strSqlPre & "       ,(SELECT constant_dtl.l_name" & vbCrLf
            strSqlPre = strSqlPre & "           FROM constant_dtl" & vbCrLf
            strSqlPre = strSqlPre & "          WHERE constant_dtl.c_constant = 'BELONGING'" & vbCrLf
            strSqlPre = strSqlPre & "            AND constant_dtl.c_constant_seq = staf_attribute.k_belonging)     AS k_belonging_name" & vbCrLf
            strSqlPre = strSqlPre & "       ,staf_attribute.k_qualification                                        AS k_qualification" & vbCrLf      ' 乗務資格
            '                           乗務資格名称取得
            strSqlPre = strSqlPre & "       ,(SELECT constant_dtl.l_name" & vbCrLf                                                                   ' 乗務資格名称
            strSqlPre = strSqlPre & "           FROM constant_dtl" & vbCrLf
            strSqlPre = strSqlPre & "          WHERE constant_dtl.c_constant = 'QUALIFICATION'" & vbCrLf
            strSqlPre = strSqlPre & "            AND constant_dtl.c_constant_seq = staf_attribute.k_qualification) AS k_qualification_name" & vbCrLf
            strSqlPre = strSqlPre & "       ,COUNT(*)                                                              AS cnt" & vbCrLf                  ' 件数
            strSqlPre = strSqlPre & "   FROM staf_attribute" & vbCrLf
            '                          基準日以下で最新のもの
            strSqlPre = strSqlPre & "        INNER JOIN ( SELECT staf_attribute.c_user_id" & vbCrLf
            strSqlPre = strSqlPre & "                           ,staf_attribute.c_ksh" & vbCrLf
            strSqlPre = strSqlPre & "                           ,staf_attribute.c_staf_id" & vbCrLf
            strSqlPre = strSqlPre & "                           ,MAX(staf_attribute.d_from) AS max_d_from" & vbCrLf
            strSqlPre = strSqlPre & "                       FROM staf_attribute" & vbCrLf
            strSqlPre = strSqlPre & "                      WHERE staf_attribute.d_from <= '" & strTargetDatePre & "'" & vbCrLf
            strSqlPre = strSqlPre & "                      GROUP BY staf_attribute.c_user_id" & vbCrLf
            strSqlPre = strSqlPre & "                              ,staf_attribute.c_ksh" & vbCrLf
            strSqlPre = strSqlPre & "                              ,staf_attribute.c_staf_id ) AS new_staf_attribute" & vbCrLf
            strSqlPre = strSqlPre & "        ON staf_attribute.c_user_id = new_staf_attribute.c_user_id" & vbCrLf
            strSqlPre = strSqlPre & "        AND staf_attribute.c_ksh = new_staf_attribute.c_ksh" & vbCrLf
            strSqlPre = strSqlPre & "        AND staf_attribute.c_staf_id = new_staf_attribute.c_staf_id" & vbCrLf
            strSqlPre = strSqlPre & "        AND staf_attribute.d_from = new_staf_attribute.max_d_from" & vbCrLf
            '                          組合員ステータス区分が '01'：加入
            strSqlPre = strSqlPre & "  WHERE staf_attribute.k_user_status = '" & USER_STATUS_ENTRY & "'" & vbCrLf
            '                          組合員種別コードが '01':正組合員か '02':シニア組合員, '04':準組合員
            strSqlPre = strSqlPre & "    AND ( staf_attribute.k_staf_kind = '" & STAF_KIND_REGULAR & "'" & vbCrLf
            strSqlPre = strSqlPre & "        OR staf_attribute.k_staf_kind = '" & STAF_KIND_SENIOR & "'" & vbCrLf
            strSqlPre = strSqlPre & "        OR staf_attribute.k_staf_kind = '" & STAF_KIND_SEMI & "' )" & vbCrLf
            '                          削除区分が '0'
            strSqlPre = strSqlPre & "    AND staf_attribute.k_del = '0'" & vbCrLf
            '                          所属支部（組合支部）と乗務資格で集約
            strSqlPre = strSqlPre & "  GROUP BY staf_attribute.k_belonging" & vbCrLf
            strSqlPre = strSqlPre & "          ,staf_attribute.k_qualification" & vbCrLf
            strSqlPre = strSqlPre & "  ORDER BY staf_attribute.k_belonging" & vbCrLf
            strSqlPre = strSqlPre & "          ,staf_attribute.k_qualification" & UtDb.DbOrderOffset & vbCrLf
            strSqlPre = strSqlPre & ";" & vbCrLf
            ' データベース接続
            Call clsDb.Connect()
            'todo:
            ' SQL実行
            dtNew = clsDb.ExecuteSql(strSqlNew)
            dtPre = clsDb.ExecuteSql(strSqlPre)
            ' SQL実行件数取得
            intCntNew = dtNew.Rows.Count
            intCntPre = dtPre.Rows.Count
            ' データセットクラス生成
            ds = New DS0101P1
            drHeaderT = ds.dtHeaderT.NewRow
            drHeaderO = ds.dtHeaderO.NewRow
            drHeaderE = ds.dtHeaderE.NewRow
            drFooter = ds.dtFooter.NewRow
            drHeaderT.BeginEdit()
            drHeaderO.BeginEdit()
            drHeaderE.BeginEdit()
            drFooter.BeginEdit()
            ' 初期値設定（今期）
            drHeaderT.s_tcaptain_member = 0             ' 東京　機長
            drHeaderT.s_tcopilot_member = 0             ' 東京　副操縦士
            drHeaderT.s_tengineer_member = 0            ' 東京　航空機関士
            drHeaderT.s_telse_member = 0                ' 東京　その他
            drHeaderO.s_ocaptain_member = 0             ' 大阪　機長
            drHeaderO.s_ocopilot_member = 0             ' 大阪　副操縦士
            drHeaderO.s_oengineer_member = 0            ' 大阪　航空機関士
            drHeaderO.s_oelse_member = 0                ' 大阪　その他
            drHeaderE.s_ecaptain_member = 0             ' その他　機長
            drHeaderE.s_ecopilot_member = 0             ' その他　副操縦士
            drHeaderE.s_eengineer_member = 0            ' その他　航空機関士
            drHeaderE.s_eelse_member = 0                ' その他　その他
            ' 初期値設定（前期）
            drHeaderT.s_tmove_captain = 0               ' 機長
            drHeaderT.s_tmove_copilot = 0               ' 副操縦士
            drHeaderT.s_tmove_engineer = 0              ' 航空機関士
            drHeaderT.s_tmove_else = 0                  ' その他
            drHeaderO.s_omove_captain = 0               ' 機長
            drHeaderO.s_omove_copilot = 0               ' 副操縦士
            drHeaderO.s_omove_engineer = 0              ' 航空機関士
            drHeaderO.s_omove_else = 0                  ' その他
            drHeaderE.s_emove_captain = 0               ' 機長
            drHeaderE.s_emove_copilot = 0               ' 副操縦士
            drHeaderE.s_emove_engineer = 0              ' 機関士
            drHeaderE.s_emove_else = 0                  ' その他
            '-------------------------------------------------------------------------------
            '   フッター
            '-------------------------------------------------------------------------------
            drFooter.d_year = Me.cboYear.Text           ' 基準日年
            drFooter.d_month = Me.cboMonth.Text         ' 基準日月
            drFooter.d_day = Me.cboDay.Text             ' 基準日日
            '-------------------------------------------------------------------------------
            '   今期
            '-------------------------------------------------------------------------------
            ' 0件チェック
            If intCntNew <> 0 Then
                For i = 0 To intCntNew - 1
                    If dtNew.Rows(i).Item(0).ToString = BELONGING_TOKYO Then
                        '-------------------------------------------------------------------
                        '   東京
                        '-------------------------------------------------------------------
                        If dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_PILOT Then
                            ' 機長(機長＋教官機長)
                            drHeaderT.s_tcaptain_member = drHeaderT.s_tcaptain_member + CInt(dtNew.Rows(i).Item(4)) ' 今期
                            drHeaderT.s_tmove_captain = drHeaderT.s_tmove_captain + CInt(dtNew.Rows(i).Item(4))     ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_COPILOT Then
                            ' 副操縦士
                            drHeaderT.s_tcopilot_member = CInt(dtNew.Rows(i).Item(4))                               ' 今期
                            drHeaderT.s_tmove_copilot = CInt(dtNew.Rows(i).Item(4))                                 ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_FLIGHT_ENGINEER Then
                            ' 航空機関士
                            drHeaderT.s_tengineer_member = CInt(dtNew.Rows(i).Item(4))                              ' 今期
                            drHeaderT.s_tmove_engineer = CInt(dtNew.Rows(i).Item(4))                                ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_TEACHER_PILOT Then
                            ' 教官機長(機長＋教官機長)
                            drHeaderT.s_tcaptain_member = drHeaderT.s_tcaptain_member + CInt(dtNew.Rows(i).Item(4)) ' 今期
                            drHeaderT.s_tmove_captain = drHeaderT.s_tmove_captain + CInt(dtNew.Rows(i).Item(4))     ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_ETC Then
                            ' その他
                            drHeaderT.s_telse_member = CInt(dtNew.Rows(i).Item(4))                                  ' 今期
                            drHeaderT.s_tmove_else = CInt(dtNew.Rows(i).Item(4))                                    ' 前期
                        End If
                    ElseIf dtNew.Rows(i).Item(0).ToString() = BELONGING_OSAKA Then
                        '-------------------------------------------------------------------
                        '   大阪
                        '-------------------------------------------------------------------
                        If dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_PILOT Then
                            ' 機長(機長＋教官機長)
                            drHeaderO.s_ocaptain_member = drHeaderO.s_ocaptain_member + CInt(dtNew.Rows(i).Item(4))                               ' 今期
                            drHeaderO.s_omove_captain = drHeaderO.s_omove_captain + CInt(dtNew.Rows(i).Item(4))                                 ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_COPILOT Then
                            ' 副操縦士
                            drHeaderO.s_ocopilot_member = CInt(dtNew.Rows(i).Item(4))                               ' 今期
                            drHeaderO.s_omove_copilot = CInt(dtNew.Rows(i).Item(4))                                 ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_FLIGHT_ENGINEER Then
                            ' 航空機関士
                            drHeaderO.s_oengineer_member = CInt(dtNew.Rows(i).Item(4))                              ' 今期
                            drHeaderO.s_omove_engineer = CInt(dtNew.Rows(i).Item(4))                                ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_TEACHER_PILOT Then
                            ' 教官機長(機長＋教官機長)
                            drHeaderO.s_ocaptain_member = drHeaderO.s_ocaptain_member + CInt(dtNew.Rows(i).Item(4)) ' 今期
                            drHeaderO.s_omove_captain = drHeaderO.s_omove_captain + CInt(dtNew.Rows(i).Item(4))     ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_ETC Then
                            ' その他
                            drHeaderO.s_oelse_member = CInt(dtNew.Rows(i).Item(4))                                  ' 今期
                            drHeaderO.s_omove_else = CInt(dtNew.Rows(i).Item(4))                                    ' 前期
                        End If
                    ElseIf dtNew.Rows(i).Item(0).ToString() = BELONGING_ETC Then
                        '-------------------------------------------------------------------
                        '   その他
                        '-------------------------------------------------------------------
                        If dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_PILOT Then
                            ' 機長(機長＋教官機長)
                            drHeaderE.s_ecaptain_member = drHeaderE.s_ecaptain_member + CInt(dtNew.Rows(i).Item(4))                               ' 今期
                            drHeaderE.s_emove_captain = drHeaderE.s_emove_captain + CInt(dtNew.Rows(i).Item(4))                                 ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_COPILOT Then
                            ' 副操縦士
                            drHeaderE.s_ecopilot_member = CInt(dtNew.Rows(i).Item(4))                               ' 今期
                            drHeaderE.s_emove_copilot = CInt(dtNew.Rows(i).Item(4))                                 ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_FLIGHT_ENGINEER Then
                            ' 航空機関士
                            drHeaderE.s_eengineer_member = CInt(dtNew.Rows(i).Item(4))                              ' 今期
                            drHeaderE.s_emove_engineer = CInt(dtNew.Rows(i).Item(4))                                ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_TEACHER_PILOT Then
                            ' 教官機長(機長＋教官機長)
                            drHeaderE.s_ecaptain_member = drHeaderE.s_ecaptain_member + CInt(dtNew.Rows(i).Item(4)) ' 今期
                            drHeaderE.s_emove_captain = drHeaderE.s_emove_captain + CInt(dtNew.Rows(i).Item(4))     ' 前期
                        ElseIf dtNew.Rows(i).Item(2).ToString() = QUALIFICATION_ETC Then
                            ' その他
                            drHeaderE.s_eelse_member = CInt(dtNew.Rows(i).Item(4))                                  ' 今期
                            drHeaderE.s_emove_else = CInt(dtNew.Rows(i).Item(4))                                    ' 前期
                        End If
                    End If
                Next
            End If
            '-------------------------------------------------------------------------------
            '   前期
            '-------------------------------------------------------------------------------
            ' 0件チェック
            If intCntPre <> 0 Then
                For i = 0 To intCntPre - 1
                    If dtPre.Rows(i).Item(0).ToString() = BELONGING_TOKYO Then
                        '-------------------------------------------------------------------
                        '   東京
                        '-------------------------------------------------------------------
                        If dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_PILOT Then
                            ' 機長
                            drHeaderT.s_tmove_captain = drHeaderT.s_tmove_captain - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_COPILOT Then
                            ' 副操縦士
                            drHeaderT.s_tmove_copilot = drHeaderT.s_tmove_copilot - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_FLIGHT_ENGINEER Then
                            ' 航空機関士
                            drHeaderT.s_tmove_engineer = drHeaderT.s_tmove_engineer - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_TEACHER_PILOT Then
                            ' 教官機長
                            drHeaderT.s_tmove_captain = drHeaderT.s_tmove_captain - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_ETC Then
                            ' その他
                            drHeaderT.s_tmove_else = drHeaderT.s_tmove_else - CInt(dtPre.Rows(i).Item(4))
                        End If
                    ElseIf dtPre.Rows(i).Item(0).ToString() = BELONGING_OSAKA Then
                        '-------------------------------------------------------------------
                        '   大阪
                        '-------------------------------------------------------------------
                        If dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_PILOT Then
                            ' 機長
                            drHeaderO.s_omove_captain = drHeaderO.s_omove_captain - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_COPILOT Then
                            ' 副操縦士
                            drHeaderO.s_omove_copilot = drHeaderO.s_omove_copilot - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_FLIGHT_ENGINEER Then
                            ' 航空機関士
                            drHeaderO.s_omove_engineer = drHeaderO.s_omove_engineer - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_TEACHER_PILOT Then
                            ' 教官機長
                            drHeaderO.s_omove_captain = drHeaderO.s_omove_captain - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_ETC Then
                            ' その他
                            drHeaderO.s_omove_else = drHeaderO.s_omove_else - CInt(dtPre.Rows(i).Item(4))
                        End If

                    ElseIf dtPre.Rows(i).Item(0).ToString() = BELONGING_ETC Then
                        '-------------------------------------------------------------------
                        '   その他
                        '-------------------------------------------------------------------
                        If dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_PILOT Then
                            ' 機長
                            drHeaderE.s_emove_captain = drHeaderE.s_emove_captain - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_COPILOT Then
                            ' 副操縦士
                            drHeaderE.s_emove_copilot = drHeaderE.s_emove_copilot - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_FLIGHT_ENGINEER Then
                            ' 機関士
                            drHeaderE.s_emove_engineer = drHeaderE.s_emove_engineer - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_TEACHER_PILOT Then
                            ' 教官機長
                            drHeaderE.s_emove_captain = drHeaderE.s_emove_captain - CInt(dtPre.Rows(i).Item(4))
                        ElseIf dtPre.Rows(i).Item(2).ToString() = QUALIFICATION_ETC Then
                            ' その他
                            drHeaderE.s_emove_else = drHeaderE.s_emove_else - CInt(dtPre.Rows(i).Item(4))
                        End If
                    End If
                Next
            End If
            drHeaderT.EndEdit()
            drHeaderO.EndEdit()
            drHeaderE.EndEdit()
            drFooter.EndEdit()
            ds.dtHeaderT.Rows.Add(drHeaderT)
            ds.dtHeaderO.Rows.Add(drHeaderO)
            ds.dtHeaderE.Rows.Add(drHeaderE)
            ds.dtFooter.Rows.Add(drFooter)
            ' クラス生成
            fmPrint = New FM000203                                                      ' 印刷プレビュー画面
            reportObj = New CR0101P1                                                    ' レポートドキュメント生成
            ' プロパティ設定
            fmPrint.ButtonShowType = 3                                                  ' ボタン形式設定（印刷・キャンセル）
            fmPrint.PrintCntVisible = True                                              ' 印刷部数項目表示可否
            fmPrint.ObjResource = reportObj                                             ' レポート形式設定
            reportObj.SetDataSource(ds)                                                 ' データセット設定
            ' 印刷プレビュー画面表示
            Call fmPrint.ShowDialog()
            ' 印刷プレビュー画面処理結果取得
            intRet = fmPrint.IntQlickBtnFlag
            ' 印刷プレビュー画面処理結果処理判定
            If intRet = 3 Then
                fmPrint.PrintOut()
                Me.Visible = False
                '' 組合員管理画面遷移
                'If TransitionScreen() = False Then
                '    Exit Function
                'End If
            End If
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Cursor.Current = Cursors.Default                                            ' カーソル初期
            Call clsDb.Disconnect()                                                     ' データベース切断
            fmPrint.Dispose()                                                           ' プレビュー画面クラス破棄
        End Try
        Return blnRet                                                                   ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/28(水)  m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(水)  m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen() As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)                           ' パネルオブジェクト
        Dim clsUC010101 As UC010101 = Nothing                                           ' 組合員管理 - 基本情報クラス
        Try
            '---------------------------------------------------------------------------
            '   組合員管理
            '---------------------------------------------------------------------------
            Me.Visible = False                                                          ' 基準日入力画面非表示
            ' 画面間パラメータ情報設定
            clsUC010101 = New UC010101                                                  ' 組合員管理
            Call pnl.Controls.Add(clsUC010101)                                          ' 組合員管理
            blnRet = True                                                               ' 戻り値設定
        Catch ex As Exception
            pnl.Visible = False                                                         ' パネル非表示
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                   ' 戻り値設定
    End Function
#End Region

End Class

#End Region
