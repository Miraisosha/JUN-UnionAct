#Region "UC040401"

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.MDNameStrikeCommon


Public Class UC040401

    'プロパティに0を設定してオープン　→　新規通告040402
    'プロパティに1を設定してオープン　→　一部解除新規通告（画面は違うが統一するためと、今後必要になるかもなので念のため）040403
    'プロパティに2を設定してオープン　→　照会（詳細）040402
    'プロパティに3を設定してオープン　→　一部解除照会（詳細）040403
    'プロパティに4を設定してオープン　→　一時保存照会（詳細）040402
    'プロパティに5を設定してオープン　→　一時保存一部解除040403

    '更新者の取得はstaf_attribute_full_time_now_name_viewのwhere句でID指定
#Region "定数"
    Private Const SCREEN_ID As String = SCREEN_ID_UC040401
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040401

    '活動日検索結果列名
    Private Const TIME_KIND As String = "H24"
    Private Const STRIKE_TIME As String = "時"
    Private Const STAFF_ID As String = "社番"

    Private Const STAFF_NAME As String = "氏名"
    Private Const AREA_LOCAL_COL As String = "会社所属"
    Private Const NOTICE_NUMBER As String = "通告番号"

    Private Const NOTICE_DAY_INF As String = "通告日時"
    Private Const NOTICE_UPDATE_USER As String = "担当者"
    Private Const FIGHT_ID As String = "闘争指令"

    Private Const CANCEL_NOTICE_NUMBER As String = "解除番号"
    Private Const CANCEL_DAY_INF As String = "解除日時"
    Private Const CANCE_UPDATE_USER As String = "解除担当者"

    Private Const CANCEL_ID As String = "解除指令"
    Private Const STRIKE_ID_COL As String = "争議行為ID"

    'レポートで必要な列
    Private Const NOTICE_UPDATE_USER_ID As String = "担当者社番"
    Private Const CANCE_UPDATE_USER_ID As String = "解除担当者社番"

    '列名
    Private Const START_DAY As String = "開始日"
    Private Const START_TIME As String = "開始時間"
    Private Const END_DAY As String = "終了日"
    Private Const END_TIME As String = "終了時間"

    '通行日検索列名のみに必要なもの
    Private Const STRIKE_KIND As String = "種別"
    Private Const ACTION_DATE As String = "活動日付"
    Private Const STRIKE_ACTION_NOTICE_NUMBER As String = "争議行為通告番号"
    Private Const NOTICE_NUMBER_KIND As String = "通告番号種別"
    Private Const STAND_USER As String = "代表者"
    Private Const BIKOU As String = "備考"
    Private Const UP_CNT As String = "更新回数"
    Private Const REALLY_STRIKE_ID As String = "関連関連ストＩＤ"
    Private Const REALLY_NUMBER As String = "関連通告番号"
    Private Const TIME_KIND_CODE As String = "時間枠コード"
    Private Const PERIOD_ID As String = "期ID"

    '一時保存
    Private Const TEMP_INDEX As String = "インデックス"
    Private Const TEMP_INSERT_DAY As String = "登録日付"

    '通告番号の分割文字
    Private Const SEPALATE_CHAR As String = "-"

    '活動日検索(データグリッドビュー表示項目)
    Private ReadOnly ACTIONDATE_DGV_COLUMNS_NAME As String() = {TIME_KIND, STRIKE_TIME, STAFF_ID, _
                                                                STAFF_NAME, AREA_LOCAL_COL, NOTICE_NUMBER, _
                                                                NOTICE_DAY_INF, NOTICE_UPDATE_USER, FIGHT_ID, _
                                                                CANCEL_NOTICE_NUMBER, CANCEL_DAY_INF, CANCE_UPDATE_USER, _
                                                                CANCEL_ID, STRIKE_ID_COL, NOTICE_UPDATE_USER_ID, _
                                                                CANCE_UPDATE_USER_ID, BIKOU}

    Private ReadOnly ACTIONDATE_DGV_COLUMNS_WIDTH As Integer() = {60, 60, 100, _
                                                                  160, 100, 100, _
                                                                  140, 160, 100, _
                                                                  100, 100, 160, _
                                                                  100, 140, 200, _
                                                                  220, 100}

    Private ReadOnly ACTIONDATE_DGV_COLUMNS_VISIBLE As Boolean() = {True, True, True, _
                                                                    True, True, True, _
                                                                    True, True, True, _
                                                                    True, True, True, _
                                                                    True, True, False, _
                                                                    False, False}

    '通告日検索結果列名
    Private ReadOnly NOTICEDATE_DGV_COLUMNS_NAME As String() = {STRIKE_KIND, NOTICE_NUMBER, NOTICE_DAY_INF, _
                                                                ACTION_DATE, NOTICE_UPDATE_USER, FIGHT_ID, _
                                                                CANCEL_ID, STRIKE_ACTION_NOTICE_NUMBER, START_TIME, _
                                                                END_DAY, END_TIME, NOTICE_NUMBER_KIND, _
                                                                STAND_USER, BIKOU, UP_CNT, _
                                                                REALLY_STRIKE_ID, REALLY_NUMBER, TIME_KIND_CODE, _
                                                                PERIOD_ID}

    Private ReadOnly NOTICEDATE_DGV_COLUMNS_WIDTH As Integer() = {80, 100, 160, _
                                                                  120, 140, 100, _
                                                                  100, 180, 100, _
                                                                  100, 100, 100, _
                                                                  100, 100, 140, _
                                                                  200, 160, 140, _
                                                                  100}

    Private ReadOnly NOTICEDATE_DGV_COLUMNS_VISIBLE As Boolean() = {True, True, True, _
                                                                    True, True, True, _
                                                                    True, True, False, _
                                                                    False, False, False, _
                                                                    False, False, False, _
                                                                    False, False, False, _
                                                                    False}

    '一時保存
    Private ReadOnly TEMP_DGV_COLUMNS_NAME As String() = {TEMP_INSERT_DAY, ACTION_DATE, NOTICE_UPDATE_USER, _
                                                          STRIKE_KIND, STRIKE_ACTION_NOTICE_NUMBER, TEMP_INDEX, _
                                                          NOTICE_NUMBER, FIGHT_ID, _
                                                          CANCEL_ID, START_TIME, END_DAY, _
                                                          END_TIME, NOTICE_NUMBER_KIND, STAND_USER, _
                                                          BIKOU, UP_CNT, TIME_KIND_CODE, _
                                                          REALLY_STRIKE_ID, PERIOD_ID}

    Private ReadOnly TEMP_DGV_COLUMNS_WIDTH As Integer() = {160, 140, 160, _
                                                            80, 200, 100, _
                                                            160, 160, _
                                                            160, 160, 160, _
                                                            160, 160, 160, _
                                                            160, 160, 140, _
                                                            200, 100}

    Private ReadOnly TEMP_DGV_COLUMNS_VISIBLE As Boolean() = {True, True, True, _
                                                              True, True, False, _
                                                              False, False, _
                                                              False, False, False, _
                                                              False, False, False, _
                                                              False, False, False, _
                                                              False, False}



#End Region


#Region "内部プロパティ"
    '参照権限
    Private _strGrantReference As String = String.Empty
    '登録権限
    Private _strGrantInsert As String = String.Empty
    '印刷権限
    Private _strGrantPrint As String = String.Empty
    'ファイル出力権限
    Private _strGrantFileOutput As String = String.Empty
#End Region


#Region "ログ出力オブジェクト"
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region


#Region "イベント"

#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UC040401_Load
    '   名称　：フォームロード
    '   概要  ：
    '   作成日：2012/01/11 新規作成 somzaki
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub UC040401_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim dtGrant As DataTable = Nothing
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成

        Try
            '***** 権限チェック *****
            '権限の取得
            dtGrant = getGrant(MENU_ID_UC040401)

            If dtGrant.Rows.Count > 0 Then
                _strGrantReference = dtGrant.Rows(0).Item(3).ToString
                _strGrantInsert = dtGrant.Rows(0).Item(4).ToString
                _strGrantPrint = dtGrant.Rows(0).Item(5).ToString
                _strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString
            End If

            If _strGrantReference <> GRANT_VALID Then
                '参照権限がない場合
                btnSearchActionDate.Enabled = False
                btnSearchNoticeDay.Enabled = False
                btnSearchTmp.Enabled = False
            End If

            If _strGrantPrint <> GRANT_VALID Then
                '印刷権限がない場合
                btnActionPrint.Enabled = False
            End If

            If _strGrantInsert <> GRANT_VALID Then
                '登録権限がない場合
                btnNewNotice.Enabled = False
                btnPartCancel.Enabled = False
            End If
            '***** 権限チェック終了 *****

            '***** コンボボックスなど初期化 *****
            ' コンボボックス（年）作成処理
            If NSMDCommon.CreateComboBoxYear(cmbActionTabYear, False) = False Then
                Exit Sub
            End If
            Call CopyCmbBox(cmbNoticeTabYear, cmbActionTabYear)
            Call SetMonth(cmbActionTabMonth)
            Call SetMonth(cmbNoticeTabMonth, True)

            '活動日タブ初期値
            If cmbActionTabYear.Items.Contains(Now.Year.ToString) Then
                Me.cmbActionTabYear.Text = Now.ToString("yyyy")
                Me.cmbActionTabMonth.Text = Now.ToString("MM")
                Me.cmbActionTabDay.Text = Now.ToString("dd")
                Me.cmbNoticeTabYear.Text = Now.ToString("yyyy")
            Else
                Me.cmbActionTabYear.SelectedIndex = 0
                Me.cmbActionTabMonth.SelectedIndex = 0
                Me.cmbActionTabDay.SelectedIndex = 0
                Me.cmbNoticeTabYear.SelectedIndex = 0
            End If


            '一時保存側
            '空白有りなのでこれはダメ
            'If NSMDCommon.CreateComboBoxYear(cmbActionYearTmp, False) = False Then
            '    Exit Sub
            'End If
            'If NSMDCommon.CreateComboBoxYear(cmbInsertYearTmp, False) = False Then
            '    Exit Sub
            'End If
            Call CopyCmbBox(cmbInsertYearTmp, cmbActionTabYear, True)
            Call CopyCmbBox(cmbActionYearTmp, cmbActionTabYear, True)
            Call SetMonth(cmbInsertMonthTmp, True)
            Call SetMonth(cmbActionMonthTmp, True)

            '***** コンボボックスなど初期化 *****

            ' データベース接続
            clsDb.Connect()

            ' 支部(検索条件) コンボボックス作成
            Call MDCommon.CreateCboConstantDtl(clsDb, Me.cmbAreaLocal, MDConst.CONSTANT_ID_AREA_LOCAL)


        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try
    End Sub
#End Region


#Region "新規通告ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnNewNotice_Click
    '   名称　：新規通告ボタンクリック
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnNewNotice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewNotice.Click
        Dim cForm1 As New FM040404()
        Dim uc040402 As UC040402 = Nothing
        Dim dtGetData As DataRow = Nothing
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)   ' パネルオブジェクト
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Dim curDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            Dim msgRtn As DialogResult
            Dim blnSkip As Boolean = False

            ' 現在期以外でログインしている場合にメッセージ表示
            If curDate < MDLoginInfo.PeriodFrom Or MDLoginInfo.PeriodTo < curDate Then
                If MDLoginInfo.CommitteeStatusFlg = 1 Then
                    ' 組合員の場合はエラーとして通告を行わない
                    CLMsg.Show("GE0234", "新規通告")
                    blnSkip = True
                Else
                    ' 専従業員・管理部の場合は警告を表示
                    msgRtn = CLMsg.Show("GW0039", "新規通告")
                    If Not msgRtn = DialogResult.Yes Then
                        blnSkip = True
                    End If
                End If
            End If

            If Not blnSkip Then
                If Not cForm1.ShowLaborDisputeData() Then
                    '争議行為なし
                    'CLMsg.Show("GI0042")
                Else
                    If cForm1.IntQlickBtnFlag = 0 Then
                        dtGetData = cForm1.SelectDataRow
                        uc040402 = New UC040402
                        '画面間パラメータ渡
                        uc040402.IntClickBtnFlg = 0
                        uc040402.SelectStrikeData = dtGetData
                        '画面呼び出し
                        Call pnl.Controls.Add(uc040402)
                        Me.Visible = False
                        'MessageBox.Show(cForm1.SelectDataRow.Columns(0).ColumnName)
                        'MessageBox.Show(cForm1.SelectDataRow.Rows(0).Item(0))
                    End If
                End If
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        Finally
            cForm1.Close()
            cForm1.Dispose()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

#End Region


#Region "印刷ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnActionPrint_Click
    '   名称　：印刷ボタンクリック
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnActionPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnActionPrint.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call PrintStrikeMember()

        Catch ex As Exception

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewNotice.Click
    '    'Dim cForm1 As New FM040404()
    '    'cForm1.Text = "争議行為通告番号の選択"
    '    '' Form1 をモーダルで表示する
    '    'cForm1.ShowDialog()
    '    '' 不要になった時点で破棄する
    '    'cForm1.Dispose()
    'End Sub

#End Region


#Region "検索ボタン（活動日付検索）クリック"
    '***************************************************************************************************
    '   ＩＤ　：btnSearchActionDay_Click
    '   名称　：検索ボタン（活動日付検索）クリック
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnSearchActionDate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchActionDate.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call SearchActionDate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub
#End Region


#Region "検索ボタン（通告日付検索）クリック"
    '***************************************************************************************************
    '   ＩＤ　：btnSearchNoticeDay_Click
    '   名称　：検索ボタン（通告日付検索）クリック
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnSearchNoticeDay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchNoticeDay.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            SearchNoticeDate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub
#End Region


#Region "一部解除ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPartCancel_Click
    '   名称　：一部解除ボタンクリック
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnPartCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPartCancel.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call PartCancel()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "詳細ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnDetail_Click
    '   名称　：詳細ボタンクリック
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetail.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call DetailShow()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "検索ボタン（一時保存）クリック"
    '***************************************************************************************************
    '   ＩＤ　：btnSearchNoticeDay_Click
    '   名称　：検索ボタン（一時保存）クリック
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub btnSearchTmp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTmp.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call SearchTempSaving()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "詳細ボタン（一時保存）クリック"
    Private Sub btnDetailTmp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetailTmp.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call TempDetailShow()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "削除ボタン（一時保存）クリック"
    Private Sub btnDeleteTmp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteTmp.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If CLMsg.Show("GQ0011") = DialogResult.Yes Then
                Call DeleteTempSaving()
                Call SearchTempSaving()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（活動日付検索タブ―年）選択値変更時"
    Private Sub cmbActionTabYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActionTabYear.SelectedIndexChanged
        Try
            Call ClearActionSearch()
            If Not ChkNull(Me.cmbActionTabMonth.Text) Then
                Call SetDayCmbbox(Me.cmbActionTabYear, Me.cmbActionTabMonth, Me.cmbActionTabDay)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub
#End Region


#Region "コンボボックス（活動日付検索タブ―月）選択値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbActionTabMonth_SelectedIndexChanged
    '   名称　：新規通告ボタンクリック
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbActionTabMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActionTabMonth.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearActionSearch()
            Call SetDayCmbbox(Me.cmbActionTabYear, Me.cmbActionTabMonth, Me.cmbActionTabDay)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（活動日付検索タブ―日）選択値変更時"
    Private Sub cmbActionTabDay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActionTabDay.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearActionSearch()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（活動日付検索タブ―会社所属）選択値変更時"
    Private Sub cmbAreaLocal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAreaLocal.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearActionSearch()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（通告日付検索タブ―年）選択値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbNoticeYear_SelectedIndexChanged
    '   名称　：通告日付タブ 年コンボボックス変更
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbNoticeYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNoticeTabYear.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearNoticeSearch()
            If Not ChkNull(cmbNoticeTabMonth.Text) Then
                '月選択済なら
                Call SetDayCmbbox(Me.cmbNoticeTabYear, Me.cmbNoticeTabMonth, Me.cmbNoticeTabDay, True)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（通告日付検索タブ―月）選択値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbNoticeTabMonth_SelectedIndexChanged
    '   名称　：通告日付タブ 月コンボボックス変更
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbNoticeTabMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNoticeTabMonth.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearNoticeSearch()
            If Not ChkNull(cmbNoticeTabMonth.Text) Then
                '月選択済なら
                Call SetDayCmbbox(Me.cmbNoticeTabYear, Me.cmbNoticeTabMonth, Me.cmbNoticeTabDay, True)
            Else
                Me.cmbNoticeTabDay.SelectedIndex = -1
                Me.cmbNoticeTabDay.Items.Clear()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（通告日付検索タブ―日）選択値変更時"
    Private Sub cmbNoticeTabDay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbNoticeTabDay.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearNoticeSearch()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（一時保存タブ―登録日付:年）選択値変更時"
    Private Sub cmbInsertYearTmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInsertYearTmp.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearTempSearch()
            If ChkNull(cmbInsertYearTmp.Text) Then
                Me.cmbInsertMonthTmp.SelectedIndex = -1
                Me.cmbInsertDayTmp.SelectedIndex = -1
                Me.cmbInsertDayTmp.Items.Clear()
            End If

            If Not ChkNull(cmbInsertMonthTmp.Text) Then
                '月選択済なら
                Call SetDayCmbbox(Me.cmbInsertYearTmp, Me.cmbInsertMonthTmp, Me.cmbInsertDayTmp, True)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（一時保存タブ―登録日付:月）選択値変更時"
    Private Sub cmbInsertMonthTmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInsertMonthTmp.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearTempSearch()
            If Not ChkNull(cmbInsertYearTmp.Text) Then
                If Not ChkNull(cmbInsertMonthTmp.Text) Then
                    Call SetDayCmbbox(Me.cmbInsertYearTmp, Me.cmbInsertMonthTmp, Me.cmbInsertDayTmp, True)
                Else
                    Me.cmbInsertDayTmp.SelectedIndex = -1
                    Me.cmbInsertDayTmp.Items.Clear()
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（一時保存タブ―登録日付:日）選択値変更時"
    Private Sub cmbInsertDayTmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbInsertDayTmp.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearTempSearch()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（一時保存タブ―活動日付:年）選択値変更時"
    Private Sub cmbActionYearTmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActionYearTmp.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearTempSearch()
            If ChkNull(cmbActionYearTmp.Text) Then
                Me.cmbActionMonthTmp.SelectedIndex = -1
                Me.cmbActionDayTmp.SelectedIndex = -1
                Me.cmbActionDayTmp.Items.Clear()
            End If

            If Not ChkNull(Me.cmbActionMonthTmp.Text) Then
                '月選択済なら
                Call SetDayCmbbox(Me.cmbActionYearTmp, Me.cmbActionMonthTmp, Me.cmbActionDayTmp, True)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（一時保存タブ―活動日付:月）選択値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbActionMonthTmp_SelectedIndexChanged
    '   名称　：日コンボボックス(活動日検索タブ)変更
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbActionMonthTmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActionMonthTmp.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearTempSearch()
            If Not ChkNull(cmbActionYearTmp.Text) Then
                If Not ChkNull(cmbActionMonthTmp.Text) Then
                    Call SetDayCmbbox(Me.cmbActionYearTmp, Me.cmbActionMonthTmp, Me.cmbActionDayTmp, True)
                Else
                    Me.cmbActionDayTmp.SelectedIndex = -1
                    Me.cmbActionDayTmp.Items.Clear()
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "コンボボックス（一時保存タブ―活動日付:日）選択値変更時"
    Private Sub cmbActionDayTmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActionDayTmp.SelectedIndexChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearTempSearch()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "活動日検索タブ、データグリッドダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgvActionSearchResult_CellDoubleClick
    '   名称　：ダブルクリック
    '   概要  ：
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub dgvActionSearchResult_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvActionSearchResult.CellDoubleClick
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.RowIndex <> -1 Then
                If SetNoticeDateSearch() = False Then
                    Exit Sub
                End If
                Call SearchNoticeDate()
                Me.TclStrikeSearch.SelectedIndex = 1
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            ' ログ出力（致命的エラー）
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "通告日検索タブ、データグリッドダブルクリック"
    Private Sub dgvNoticeResult_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvNoticeResult.CellDoubleClick
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.RowIndex <> -1 Then
                Call DetailShow()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            ' ログ出力（致命的エラー）
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "一時保存タブ、データグリッドダブルクリック"
    Private Sub dgvSearchResultTmp_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvSearchResultTmp.CellDoubleClick
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.RowIndex <> -1 Then
                Call TempDetailShow()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            ' ログ出力（致命的エラー）
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "通告日通告番号変更"
    Private Sub txtNoticeNumberFirst_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoticeNumberFirst.TextChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearNoticeSearch()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            ' ログ出力（致命的エラー）
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "通告日通告番号変更"
    Private Sub txtNoticeNumberUnder_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtNoticeNumberUnder.TextChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            Call ClearNoticeSearch()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            ' ログ出力（致命的エラー）
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub
#End Region


#Region "通告日検索データグリッド行変更"
    ' 2012/04/12 Fujisaku　過去日付は取消できない判定処理を不要の為撤去
    'Private Sub dgvNoticeResult_RowEnter(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvNoticeResult.RowEnter
    '    Try
    '        If Me.dgvNoticeResult.SelectedRows.Count > 0 Then
    '            If Now.ToString("yyyy/MM/dd") >= dgvNoticeResult.SelectedRows.Item(0).Cells(ACTION_DATE).Value.ToString Then
    '                Me.btnPartCancel.Enabled = False
    '            Else
    '                Me.btnPartCancel.Enabled = True
    '            End If
    '            Call GrantValSetButtonEnable()
    '        End If
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub
#End Region


#Region "エンターキークリック関連"

#Region "活動日検索タブ"
    Private Sub cmbActionTabYear_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActionTabYear.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchActionDate()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub



    Private Sub cmbActionTabMonth_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActionTabMonth.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchActionDate()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub


    Private Sub cmbActionTabDay_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActionTabDay.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchActionDate()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub


    Private Sub cmbAreaLocal_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAreaLocal.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchActionDate()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

#End Region


#Region "通告日検索"
    Private Sub cmbNoticeTabYear_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbNoticeTabYear.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            SearchNoticeDate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    Private Sub cmbNoticeTabMonth_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbNoticeTabMonth.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            SearchNoticeDate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    Private Sub cmbNoticeTabDay_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbNoticeTabDay.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            SearchNoticeDate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    Private Sub txtNoticeNumberFirst_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNoticeNumberFirst.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            SearchNoticeDate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub


    Private Sub txtNoticeNumberUnder_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNoticeNumberUnder.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            SearchNoticeDate()
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

#End Region


#Region "一時保存タブ"
    '***************************************************************************************************
    '   ＩＤ　：cmbInsertYearTmp_KeyPress
    '   名称　：一時保存画面コンボボックス
    '   概要  ：
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbInsertYearTmp_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbInsertYearTmp.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchTempSaving()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbInsertMonthTmp_KeyPress
    '   名称　：一時保存画面コンボボックス
    '   概要  ：
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbInsertMonthTmp_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbInsertMonthTmp.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchTempSaving()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbInsertDayTmp_KeyPress
    '   名称　：一時保存画面コンボボックス
    '   概要  ：
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：)
    '***************************************************************************************************
    Private Sub cmbInsertDayTmp_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbInsertDayTmp.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchTempSaving()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbActionYearTmp_KeyPress
    '   名称　：一時保存画面コンボボックス
    '   概要  ：
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbActionYearTmp_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActionYearTmp.KeyPress

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchTempSaving()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbActionMonthTmp_KeyPress
    '   名称　：一時保存画面コンボボックス
    '   概要  ：
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbActionMonthTmp_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActionMonthTmp.KeyPress
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchTempSaving()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cmbActionDayTmp_KeyPress
    '   名称　：一時保存画面コンボボックス
    '   概要  ：
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub cmbActionDayTmp_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActionDayTmp.KeyPress
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call SearchTempSaving()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Sub

#End Region

#End Region

#End Region


#Region "関数"

#Region "コンボボックスコピー処理"
    '***************************************************************************************************
    '   ＩＤ　：CopyCmbBox
    '   名称　：コンボボックスの選択リストをコピーする
    '   概要  ：コンボボックスの選択リストをコピーする。（引数で空白入力を可能、不可能設定できる）
    '   引数　：なし
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub CopyCmbBox(ByVal pMakeCbo As System.Windows.Forms.ComboBox, ByVal pSourceCbo As System.Windows.Forms.ComboBox, Optional ByVal emptySet As Boolean = False)
        ' 初期処理
        pMakeCbo.BeginUpdate()                                           ' チラつき防止の為、最後まで描写しない
        pMakeCbo.Items.Clear() ' 月コンボボックスクリア
        If emptySet Then
            pMakeCbo.Items.Add("")
        End If
        For intCnt = 0 To pSourceCbo.Items.Count - 1
            pMakeCbo.Items.Add(pSourceCbo.Items.Item(intCnt))
        Next
        pMakeCbo.EndUpdate()                                             ' チラつき防止の為、最後に描写する
    End Sub
#End Region


#Region "月コンボボックス設定処理"
    '***************************************************************************************************
    '   ＩＤ　：SetMonth
    '   名称　：月コンボボックス設定処理
    '   概要  ：月コンボボックスの値を設定する
    '   引数　：なし
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetMonth(ByVal pCbo As System.Windows.Forms.ComboBox, Optional ByVal emptySet As Boolean = False)
        ' 初期処理
        pCbo.BeginUpdate()                                           ' チラつき防止の為、最後まで描写しない
        pCbo.Items.Clear() ' 月コンボボックスクリア
        If emptySet Then
            pCbo.Items.Add("")
        End If
        pCbo.Items.Add("01")
        pCbo.Items.Add("02")
        pCbo.Items.Add("03")
        pCbo.Items.Add("04")
        pCbo.Items.Add("05")
        pCbo.Items.Add("06")
        pCbo.Items.Add("07")
        pCbo.Items.Add("08")
        pCbo.Items.Add("09")
        pCbo.Items.Add("10")
        pCbo.Items.Add("11")
        pCbo.Items.Add("12")
        pCbo.EndUpdate()                                             ' チラつき防止の為、最後に描写する
    End Sub
#End Region


#Region "日コンボボックス(活動日検索タブ)設定呼び出し"
    '***************************************************************************************************
    '   ＩＤ　：SetDayCmbActiondayTab
    '   名称　：日コンボボックス(活動日検索タブ)変更
    '   概要  ：
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetDayCmbbox(ByVal cmbYear As ComboBox, ByVal cmbMonth As ComboBox, ByVal cmbDay As ComboBox, Optional ByVal emptySet As Boolean = False)
        Try
            Dim dNextMonth As Date = Date.Parse(cmbYear.Text + "/" + cmbMonth.Text + "/01").AddMonths(1)
            Dim maxDay = dNextMonth.AddDays(-1).ToString("dd")
            Call SetDay(cmbDay, Integer.Parse(maxDay), emptySet)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub
#End Region

    '#Region "日コンボボックス(通告日検索タブ)設定呼び出し"
    '    '***************************************************************************************************
    '    '   ＩＤ　：SetDayCmbActiondayTab
    '    '   名称　：日コンボボックス(通告日検索タブ)変更
    '    '   概要  ：
    '    '   作成日：2012/01/11 新規作成
    '    '---------------------------------------------------------------------------------------------------
    '    '   履歴　：
    '    '***************************************************************************************************
    '    Private Sub SetDayCmbNoticedayTab()
    '        Try
    '            Dim dNextMonth As Date = Date.Parse(cmbNoticeTabYear.Text + "/" + cmbNoticeTabMonth.Text + "/01").AddMonths(1)
    '            Dim maxDay = dNextMonth.AddDays(-1).ToString("dd")
    '            Call SetDay(Me.cmbNoticeTabDay, Integer.Parse(maxDay), True)
    '        Catch ex As Exception
    '            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '            log.Fatal(ex.Message)
    '        End Try
    '    End Sub
    '#End Region


#Region "日コンボボックス設定処理"
    '***************************************************************************************************
    '   ＩＤ　：SetDay
    '   名称　：月コンボボックス設定処理
    '   概要  ：月コンボボックスの値を設定する
    '   引数　：なし
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetDay(ByVal pCbo As System.Windows.Forms.ComboBox, ByVal maxDay As Integer, Optional ByVal emptySet As Boolean = False)
        Try
            ' 初期処理
            pCbo.BeginUpdate()              ' チラつき防止の為、最後まで描写しない
            pCbo.Items.Clear()
            If emptySet Then
                pCbo.Items.Add("")
            End If
            For intDayCnt = 1 To Integer.Parse(maxDay)
                pCbo.Items.Add(intDayCnt.ToString().PadLeft(2, "0"))
            Next
            ' 月コンボボックスクリア
            pCbo.EndUpdate()                ' チラつき防止の為、最後に描写する
            pCbo.SelectedIndex = 0
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub
#End Region


#Region "検索（活動日付検索）"
    '***************************************************************************************************
    '   ＩＤ　：SearchActionDate
    '   名称　：活動日付検索
    '   概要  ：活動日付タブの検索ボタンクリック後にコールされる検索実行部分
    '   引数　：なし
    '   作成日：2012/01/12 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Friend Function SearchActionDate() As Boolean
        Dim returnVal As Boolean = False
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim strMainSql As String = "" 'メインのSELECT文
        Dim strLatestStaffAttribute As String = ""  'メインで使用されるサブクエリ（SELECT文）
        Dim strTargetDayStrikeSql As String = ""   'メインで使用されるサブクエリ（SELECT文）

        Try
            'カーソル
            Me.Cursor = Cursors.WaitCursor

            Me.dgvActionSearchResult.Rows.Clear()
            Me.dgvActionSearchResult.Columns.Clear()
            'SQL***
            ' 指定した活動日付で登録されたストライキ情報（name_strike_member_date）のデータ
            '  →　人毎のストライキ情報を基に、組合員基本情報（最新のユーザー名、所属会社）とストライキ情報を結びつける
            ' SELECT 列名　FROM  name_strike_member_date 
            '         INNNER JOIN ([最新のstaf_attribute] ON name_strike_member_date.user_id = staf_attribte.user_id)
            '         LEFT JOIN ()

            'SQL
            Dim dateNow As Date = Now
            Dim standardDay As String = dateNow.ToString("yyyyMMdd")

            'INNER JOIN　で使用するサブクエリ（SELECT句（最新の組合員情報取得））
            strLatestStaffAttribute = "   SELECT " & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     staf_attr.c_user_id AS 社員番号, " & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     staf_attr.l_name AS 名前 ," & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     staf_attr.k_local AS 会社所属コード ," & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     cnst1.l_name AS 会社支部" & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "   FROM " & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     staf_attribute AS staf_attr, " & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     constant_dtl AS cnst1," & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     (SELECT c_user_id , max(d_from) AS new_from" & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "      FROM staf_attribute " & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "      WHERE d_from <= '" & standardDay & "'" & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "      GROUP BY c_user_id" & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     ) AS latest_attr  " & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "   WHERE " & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "     staf_attr.c_user_id = latest_attr.c_user_id" & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "   AND staf_attr.d_from = latest_attr.new_from" & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "   AND staf_attr.k_local = cnst1.c_constant_seq " & vbCrLf
            strLatestStaffAttribute = strLatestStaffAttribute & "   AND cnst1.c_constant = 'AREA_LOCAL' " & vbCrLf
            ' 会社所属選択時
            If Not ChkNull(cmbAreaLocal.Text) Then
                strLatestStaffAttribute = strLatestStaffAttribute & "   AND cnst1.l_name = '" & Me.cmbAreaLocal.Text & "' " & vbCrLf
            End If

            'INNER JOIN　で使用するサブクエリ（SELECT句（指定された日が活動日に含まれるストライキ情報））

            strTargetDayStrikeSql = "SELECT * FROM name_strike " & vbCrLf
            strTargetDayStrikeSql = strTargetDayStrikeSql & " WHERE d_operation_from <= '" & Me.cmbActionTabYear.Text & "/" & Me.cmbActionTabMonth.Text & "/" & Me.cmbActionTabDay.Text & "' " & vbCrLf
            strTargetDayStrikeSql = strTargetDayStrikeSql & " AND d_operation_to >= '" & Me.cmbActionTabYear.Text & "/" & Me.cmbActionTabMonth.Text & "/" & Me.cmbActionTabDay.Text & "' "
            strTargetDayStrikeSql = strTargetDayStrikeSql & " AND c_really_name_strike_id = '' "
            ' (SELECT * FROM name_strike WHERE d_operation_from <= '2012/02/03' AND d_operation_to >= '2012/02/03' ) 

            'メイン部分生成
            strMainSql = "" & vbCrLf
            strMainSql = strMainSql & "SELECT" & vbCrLf
            strMainSql = strMainSql & "  IIF(fight_inf.k_time_frame = '" & MDNameStrikeCommon.TIME_FRAME_24 & "' , '*' , '' ) AS " & TIME_KIND & "," & vbCrLf
            strMainSql = strMainSql & "  fight_inf.d_operation_from AS " & START_DAY & "," & vbCrLf
            strMainSql = strMainSql & "  fight_inf.d_operation_time_from AS " & START_TIME & "," & vbCrLf
            strMainSql = strMainSql & "  fight_inf.d_operation_to AS " & END_DAY & "," & vbCrLf
            strMainSql = strMainSql & "  fight_inf.d_operation_time_to AS " & END_TIME & "," & vbCrLf
            strMainSql = strMainSql & "  attr.社員番号 AS " & STAFF_ID & "," & vbCrLf
            strMainSql = strMainSql & "  attr.名前 AS " & STAFF_NAME & "," & vbCrLf
            strMainSql = strMainSql & "  attr.会社支部 AS " & AREA_LOCAL_COL & "," & vbCrLf
            strMainSql = strMainSql & "  fight_inf.c_name_strike_id AS " & NOTICE_NUMBER & "," & vbCrLf
            strMainSql = strMainSql & "  fight_inf.d_info AS " & NOTICE_DAY_INF & "," & vbCrLf
            strMainSql = strMainSql & "  fight_updater.l_name AS " & NOTICE_UPDATE_USER & "," & vbCrLf
            strMainSql = strMainSql & "  fight_inf.c_fight AS " & FIGHT_ID & "," & vbCrLf
            strMainSql = strMainSql & "  name_strike_member_date.c_cancel_name_strike_id AS " & CANCEL_NOTICE_NUMBER & "," & vbCrLf
            strMainSql = strMainSql & "  name_strike_member_date.d_cancel_info AS " & CANCEL_DAY_INF & "," & vbCrLf
            strMainSql = strMainSql & "  cancel_updater.l_name AS " & CANCE_UPDATE_USER & " ," & vbCrLf
            strMainSql = strMainSql & "  cancel_inf.c_cancel AS " & CANCEL_ID & "," & vbCrLf
            strMainSql = strMainSql & "  fight_inf.c_strike_id AS " & STRIKE_ID_COL & ", " & vbCrLf
            strMainSql = strMainSql & "  fight_inf.c_user_id_up AS " & NOTICE_UPDATE_USER_ID & "," & vbCrLf
            strMainSql = strMainSql & "  cancel_inf.c_user_id_up AS " & CANCE_UPDATE_USER_ID & ", " & vbCrLf
            strMainSql = strMainSql & "  name_strike_member_date.l_biko AS " & BIKOU & " " & vbCrLf
            strMainSql = strMainSql & "FROM ( ( ( (name_strike_member_date " & vbCrLf
            '組合員情報取得の為サブクエリ（最新の組合員情報表示テーブル）と結合
            strMainSql = strMainSql & "INNER JOIN ( " & vbCrLf
            strMainSql = strMainSql & strLatestStaffAttribute & vbCrLf
            strMainSql = strMainSql & ") AS attr " & vbCrLf
            strMainSql = strMainSql & "ON name_strike_member_date.c_user_id = attr.社員番号)" & vbCrLf
            '闘争情報取得の為name_strikeと結合
            strMainSql = strMainSql & "INNER JOIN (" & vbCrLf
            strMainSql = strMainSql & strTargetDayStrikeSql & vbCrLf
            strMainSql = strMainSql & ") AS fight_inf " & vbCrLf
            strMainSql = strMainSql & "ON name_strike_member_date.c_name_strike_id = fight_inf.c_name_strike_id " & vbCrLf
            strMainSql = strMainSql & ")" & vbCrLf
            '解除情報取得の為name_strikeと結合
            strMainSql = strMainSql & "LEFT JOIN name_strike AS cancel_inf " & vbCrLf
            strMainSql = strMainSql & "ON name_strike_member_date.c_cancel_name_strike_id = cancel_inf.c_name_strike_id " & vbCrLf
            strMainSql = strMainSql & ")" & vbCrLf
            '担当者（更新者取得）
            strMainSql = strMainSql & "LEFT JOIN staf_attribute_full_time_now_name_view AS fight_updater  " & vbCrLf
            strMainSql = strMainSql & "ON fight_inf.c_user_id_up = fight_updater.user_id " & vbCrLf
            strMainSql = strMainSql & ") " & vbCrLf
            '解除担当者（更新者取得）
            strMainSql = strMainSql & "LEFT JOIN staf_attribute_full_time_now_name_view AS cancel_updater  " & vbCrLf
            strMainSql = strMainSql & "ON cancel_inf.c_user_id_up = cancel_updater.user_id " & vbCrLf
            '並び替え
            strMainSql = strMainSql & "ORDER BY  " & vbCrLf 'ok
            strMainSql = strMainSql & "attr.会社所属コード ASC ,Len(fight_inf.c_name_strike_id) ASC , fight_inf.c_name_strike_id, Len(attr.社員番号), attr.社員番号 ASC " & vbCrLf

            'WHERE
            'strMainSql = strMainSql & "WHERE name_strike_member_date.c_cancel_name_strike_id  = '' " & vbCrLf


            clsDb.Connect()
            Dim dtSqlResult As DataTable = clsDb.ExecuteSql(strMainSql)
            Call SetActionDateResultForDgv(dtSqlResult)
            'Dim bsSource As New BindingSource
            'bsSource.DataSource = dtSqlResult
            'dgvActionSearchResult.DataSource = bsSource
            If dtSqlResult.Rows.Count = 0 Then
                Me.btnActionPrint.Enabled = False
            Else
                Me.btnActionPrint.Enabled = True
            End If

            Call GrantValSetButtonEnable()
            Me.grpResultAction.Visible = True

        Catch ex As Exception
            log.Fatal(ex.Message)
            Me.Cursor = Cursors.Default
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        Finally
            clsDb.Disconnect()
            Me.Cursor = Cursors.Default
        End Try
        Return returnVal
    End Function
#End Region


#Region "検索（通告日付検索）"
    '***************************************************************************************************
    '   ＩＤ　：SearchNoticeDate
    '   名称　：通告日付検索
    '   概要  ：通告日付タブの検索ボタンクリック後にコールされる検索実行部分
    '   引数　：なし
    '   作成日：2012/01/12 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/04/12 Fujisaku　過去日付は取消できない判定処理を不要の為撤去
    '         ：2012/08/08 Fujisaku  後続処理で使う為期IDを取得
    '***************************************************************************************************
    Friend Function SearchNoticeDate() As Boolean
        Dim returnVal As Boolean = False
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim strMainSql As String = "" 'SELECT文
        Dim strWhereSql As String = "" 'SELECT文のWHERE句
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            'カーソル
            Me.Cursor = Cursors.WaitCursor

            Me.dgvNoticeResult.Rows.Clear()
            Me.dgvNoticeResult.Columns.Clear()

            strMainSql = "SELECT " & vbCrLf
            strMainSql = strMainSql & " name_strike.k_name_strike_kind AS " & STRIKE_KIND & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.c_name_strike_id AS " & NOTICE_NUMBER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.d_info AS " & NOTICE_DAY_INF & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.d_operation_from AS " & ACTION_DATE & ", " & vbCrLf
            strMainSql = strMainSql & " updater.l_name AS " & NOTICE_UPDATE_USER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.c_fight AS " & FIGHT_ID & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.c_cancel AS " & CANCEL_ID & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.c_strike_id AS " & STRIKE_ACTION_NOTICE_NUMBER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.d_operation_time_from AS " & START_TIME & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.d_operation_to AS " & END_DAY & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.d_operation_time_to AS " & END_TIME & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.k_strike_info AS " & NOTICE_NUMBER_KIND & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.l_stand_name AS " & STAND_USER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.l_biko AS " & BIKOU & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.s_up AS " & UP_CNT & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.c_really_name_strike_id AS " & REALLY_STRIKE_ID & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.c_related_info AS " & REALLY_NUMBER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.k_time_frame AS " & TIME_KIND_CODE & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike.c_period_id AS " & PERIOD_ID & " " & vbCrLf
            strMainSql = strMainSql & "FROM (name_strike " & vbCrLf
            strMainSql = strMainSql & "LEFT JOIN staf_attribute_full_time_now_name_view AS updater ON name_strike.c_user_id_up = updater.user_id ) " & vbCrLf
            strMainSql = strMainSql & " " & vbCrLf

            'WHERE句
            strWhereSql = "WHERE " & vbCrLf
            strWhereSql = strWhereSql & " name_strike.d_info LIKE '"

            If Not ChkNull(cmbNoticeTabYear.Text) Then
                strWhereSql = strWhereSql & cmbNoticeTabYear.Text
            End If
            If Not ChkNull(cmbNoticeTabMonth.Text) Then
                strWhereSql = strWhereSql & "/" & cmbNoticeTabMonth.Text
            End If
            If Not ChkNull(cmbNoticeTabDay.Text) Then
                strWhereSql = strWhereSql & "/" & cmbNoticeTabDay.Text
            End If

            strWhereSql = strWhereSql & "%' "

            If Not (ChkNull(txtNoticeNumberFirst.Text) And ChkNull(txtNoticeNumberUnder.Text)) Then
                '通告番号がどちらもあればc_name_strike_idを=で検索
                strWhereSql = strWhereSql & "AND c_name_strike_id LIKE '" & txtNoticeNumberFirst.Text & "%" & SEPALATE_CHAR & txtNoticeNumberUnder.Text & "%' "
            ElseIf Not (ChkNull(txtNoticeNumberFirst.Text)) Then
                '通告番号の頭のみc_name_strike_idをLIKEで検索
                strWhereSql = strWhereSql & "AND c_name_strike_id LIKE '" & txtNoticeNumberFirst.Text & "%' "
            ElseIf Not (ChkNull(txtNoticeNumberUnder.Text)) Then
                '通告番号の頭のみc_name_strike_infoをLIKEで検索
                strWhereSql = strWhereSql & "AND c_name_strike_info LIKE " & txtNoticeNumberUnder.Text & "%' "
            End If

            strMainSql = strMainSql & strWhereSql
            'strMainSql = strMainSql & "ORDER BY name_strike.d_info DESC"
            strMainSql = strMainSql & "ORDER BY Len(c_name_strike_id) DESC , c_name_strike_id DESC "

            clsDb.Connect()
            Dim dtSqlResult As DataTable = clsDb.ExecuteSql(strMainSql)
            Call SetNoticeDateResultForDgv(dtSqlResult)

            'Dim bsSource As New BindingSource
            'bsSource.DataSource = dtSqlResult
            'dgvNoticeResult.DataSource = bsSource
            'データ数チェック（データがあれば詳細などボタン使用可能）
            If dtSqlResult.Rows.Count = 0 Then
                Me.btnPartCancel.Enabled = False
                Me.btnDetail.Enabled = False
            Else
                Me.btnPartCancel.Enabled = True
                Me.btnDetail.Enabled = True
            End If

            '過去日付は一部解除できない
            'If Me.dgvNoticeResult.Rows.Count > 0 Then
            '    If Now.ToString("yyyy/MM/dd") >= dgvNoticeResult.Rows.Item(0).Cells(ACTION_DATE).Value.ToString Then
            '        Me.btnPartCancel.Enabled = False
            '    Else
            '        Me.btnPartCancel.Enabled = True
            '    End If
            '    Call GrantValSetButtonEnable()
            'End If
            'ただし権限がない場合はボタン使用不可
            Call GrantValSetButtonEnable()

            Me.grpResultNotice.Visible = True

        Catch ex As Exception
            log.Fatal(ex.Message)
            Me.Cursor = Cursors.Default
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
            Me.Cursor = Cursors.Default
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
    End Function
#End Region


#Region "検索（一時保存検索）"
    '***************************************************************************************************
    '   ＩＤ　：SearchTempSaving
    '   名称　：一時保存検索
    '   概要  ：一時保存データ検索の検索実行部分
    '   引数　：なし
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/08/08 Fujisaku  後続処理で使う為期IDを取得
    '***************************************************************************************************
    Friend Function SearchTempSaving() As Boolean
        Dim returnVal As Boolean = False
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim strMainSql As String = "" 'SELECT文
        Dim strWhereSql As String = "" 'SELECT文のWHERE句
        Try
            'カーソル
            Me.Cursor = Cursors.WaitCursor

            If Not ChkNull(Me.cmbInsertMonthTmp.Text) Then
                If ChkNull(Me.cmbInsertYearTmp.Text) Then
                    CLMsg.Show("GE0006", "通告日付の年")
                    SetErr(Me.cmbInsertYearTmp)
                    Return returnVal
                End If
            End If
            Me.cmbInsertYearTmp.BackColor = Color.White

            If Not ChkNull(Me.cmbActionMonthTmp.Text) Then
                If ChkNull(Me.cmbActionYearTmp.Text) Then
                    CLMsg.Show("GE0006", "活動日付の年")
                    SetErr(Me.cmbActionYearTmp)
                    Return returnVal
                End If
            End If
            Me.cmbActionYearTmp.BackColor = Color.White

            Me.dgvSearchResultTmp.Rows.Clear()
            Me.dgvSearchResultTmp.Columns.Clear()

            strMainSql = "SELECT " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.s_index AS " & TEMP_INDEX & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.k_name_strike_kind AS " & STRIKE_KIND & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.c_name_strike_id AS " & NOTICE_NUMBER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.d_info AS " & TEMP_INSERT_DAY & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.d_operation_from AS " & ACTION_DATE & ", " & vbCrLf
            strMainSql = strMainSql & " updater.l_name AS " & NOTICE_UPDATE_USER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.c_fight AS " & FIGHT_ID & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.c_cancel AS " & CANCEL_ID & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.c_strike_id AS " & STRIKE_ACTION_NOTICE_NUMBER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.d_operation_time_from AS " & START_TIME & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.d_operation_to AS " & END_DAY & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.d_operation_time_to AS " & END_TIME & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.k_strike_info AS " & NOTICE_NUMBER_KIND & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.l_stand_name AS " & STAND_USER & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.l_biko AS " & BIKOU & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.s_up AS " & UP_CNT & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.k_time_frame AS " & TIME_KIND_CODE & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.c_really_name_strike_id AS " & REALLY_STRIKE_ID & ", " & vbCrLf
            strMainSql = strMainSql & " name_strike_work.c_period_id AS " & PERIOD_ID & " " & vbCrLf
            strMainSql = strMainSql & "FROM (name_strike_work " & vbCrLf
            strMainSql = strMainSql & "LEFT JOIN staf_attribute_full_time_now_name_view AS updater ON name_strike_work.c_user_id_up = updater.user_id ) " & vbCrLf
            strMainSql = strMainSql & " " & vbCrLf

            'WHERE句
            If Not ChkNull(cmbInsertYearTmp.Text) Then
                strWhereSql = " name_strike_work.d_info LIKE '"
                strWhereSql = strWhereSql & cmbInsertYearTmp.Text
            End If
            If Not ChkNull(cmbInsertMonthTmp.Text) Then
                strWhereSql = strWhereSql & "/" & cmbInsertMonthTmp.Text
            End If
            If Not ChkNull(cmbInsertDayTmp.Text) Then
                strWhereSql = strWhereSql & "/" & cmbInsertDayTmp.Text
            End If

            If strWhereSql.Length > 0 Then
                strWhereSql = strWhereSql & "%' "
                If Not ChkNull(cmbActionYearTmp.Text) Then
                    strWhereSql = strWhereSql & "And "
                End If
            End If


            If Not ChkNull(cmbActionYearTmp.Text) Then
                Dim strFromSql As String = " name_strike_work.d_operation_from <= '" & Me.cmbActionYearTmp.Text
                Dim strToSql As String = " AND d_operation_to >= '" & Me.cmbActionYearTmp.Text
                If Not ChkNull(cmbActionMonthTmp.Text) Then
                    strFromSql = strFromSql & "/" & Me.cmbActionMonthTmp.Text
                    strToSql = strToSql & "/" & Me.cmbActionMonthTmp.Text
                    If Not ChkNull(cmbActionDayTmp.Text) Then
                        strFromSql = strFromSql & "/" & Me.cmbActionDayTmp.Text & "'"
                        strToSql = strToSql & "/" & Me.cmbActionDayTmp.Text & "'"
                    Else
                        strFromSql = strFromSql & "/99'"
                        strToSql = strToSql & "/00'"
                    End If
                Else
                    strFromSql = strFromSql & "/99/99'"
                    strToSql = strToSql & "/00/00'"
                End If
                strWhereSql = strWhereSql & strFromSql & strToSql
            End If


            If strWhereSql.Length > 0 Then
                strWhereSql = "WHERE " & strWhereSql
            End If
            strMainSql = strMainSql & strWhereSql & " ORDER BY name_strike_work.d_info , name_strike_work.d_operation_from "

            clsDb.Connect()
            Dim dtSqlResult As DataTable = clsDb.ExecuteSql(strMainSql)
            Call SetTempDateResultForDgv(dtSqlResult)
            'データ数チェック
            If dtSqlResult.Rows.Count = 0 Then
                Me.btnDetailTmp.Enabled = False
                Me.btnDeleteTmp.Enabled = False
            Else
                Me.btnDetailTmp.Enabled = True
                Me.btnDeleteTmp.Enabled = True
            End If
            'ただし権限がない場合はボタン使用不可
            Call GrantValSetButtonEnable()

            Me.grpResultTemp.Visible = True

        Catch ex As Exception
            log.Fatal(ex.Message)
            Me.Cursor = Cursors.Default
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        Finally
            clsDb.Disconnect()
            Me.Cursor = Cursors.Default
        End Try
    End Function
#End Region


#Region "データグリッドビューへのデータ表示(活動日付)"
    '***************************************************************************************************
    '   ＩＤ　：SetActionDateResultForDgv
    '   名称　：データグリッドビューへのデータ表示
    '   概要  ：データグリッドビューへのデータ表示
    '   引数　：なし
    '   作成日：2012/01/12 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetActionDateResultForDgv(ByVal pDateTable As DataTable)
        Try
            '規模は開始～終了までを計算して出す必要があるのでバインドせずにデータグリッドに1行ずつ追加出力する
            'Dim bsSource As New BindingSource
            'bsSource.DataSource = pDateTable
            'dgvActionSearchResult.DataSource = bsSource

            '列作成
            Call MakeActionDateDataGridViewColumns()

            '行追加
            Dim strScale As String = ""
            Dim intAddRow As Integer = 0
            For Each rowInf As DataRow In pDateTable.Rows

                Me.dgvActionSearchResult.Rows.Add()

                Dim startDay As String = NVL(rowInf.Item(START_DAY))
                Dim startTime As String = NVL(rowInf.Item(START_TIME))
                Dim endDay As String = NVL(rowInf.Item(END_DAY))
                Dim endTime As String = NVL(rowInf.Item(END_TIME))
                strScale = MDNameStrikeCommon.GetTimeSpan(startDay, startTime, endDay, endTime)

                For intColCnt = 0 To dgvActionSearchResult.Columns.Count - 1
                    Dim strColData As String = ""
                    Dim strColName As String = Me.dgvActionSearchResult.Columns(intColCnt).Name
                    '「時」列は上で求めた開始から終了までの時間を入れる（strScale）
                    If strColName = STRIKE_TIME Then
                        strColData = strScale
                    Else
                        strColData = NVL(rowInf.Item(strColName))
                    End If
                    Me.dgvActionSearchResult.Rows(intAddRow).Cells.Item(intColCnt).Value = strColData
                Next
                intAddRow = intAddRow + 1
            Next
            
            '基本設定
            Call SetDataGridBaseInfForStrike(Me.dgvActionSearchResult)
            'データ件数表示
            Call SetActionSearchCount()


        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub

#End Region


#Region "データグリッドビューへのデータ表示(通告日付)"
    '***************************************************************************************************
    '   ＩＤ　：SetNoticeDateResultForDgv
    '   名称　：データグリッドビューへのデータ表示
    '   概要  ：データグリッドビューへのデータ表示(通告日付検索)
    '   引数　：なし
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetNoticeDateResultForDgv(ByVal pDateTable As DataTable)
        Try
            '列作成
            Call MakeNoticeDateDataGridViewColumns()

            '行追加
            Dim intAddRow As Integer = 0
            For Each rowInf As DataRow In pDateTable.Rows

                Me.dgvNoticeResult.Rows.Add()

                For intColCnt = 0 To dgvNoticeResult.Columns.Count - 1
                    Dim strColData As String = ""
                    Dim strColName As String = Me.dgvNoticeResult.Columns(intColCnt).Name
                    '種別の場合は数字を日本語名称にする（01→通告、02→解除）
                    If strColName = STRIKE_KIND Then
                        strColData = MDNameStrikeCommon.GetStrikeKindMeaning(NVL(rowInf.Item(strColName)))
                    Else
                        strColData = NVL(rowInf.Item(strColName))
                    End If
                    Me.dgvNoticeResult.Rows(intAddRow).Cells.Item(intColCnt).Value = strColData
                Next
                intAddRow = intAddRow + 1
            Next

            '基本設定
            Call SetDataGridBaseInfForStrike(Me.dgvNoticeResult)
            
            '件数表示
            Call SetNoticeSearchCount()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

#End Region


#Region "データグリッドビューへのデータ表示(一時保存)"
    '***************************************************************************************************
    '   ＩＤ　：SetNoticeDateResultForDgv
    '   名称　：データグリッドビューへのデータ表示
    '   概要  ：データグリッドビューへのデータ表示(通告日付検索)
    '   引数　：なし
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetTempDateResultForDgv(ByVal pDateTable As DataTable)
        Try
            '列作成
            Call MakeTempDateDataGridViewColumns()

            '行追加
            Dim intAddRow As Integer = 0
            For Each rowInf As DataRow In pDateTable.Rows

                Me.dgvSearchResultTmp.Rows.Add()

                For intColCnt = 0 To dgvSearchResultTmp.Columns.Count - 1
                    Dim strColData As String = ""
                    Dim strColName As String = Me.dgvSearchResultTmp.Columns(intColCnt).Name
                    '種別の場合は数字を日本語名称にする（01→通告、02→解除）
                    If strColName = STRIKE_KIND Then
                        strColData = MDNameStrikeCommon.GetStrikeKindMeaning(NVL(rowInf.Item(strColName)))
                    Else
                        strColData = NVL(rowInf.Item(strColName))
                    End If
                    Me.dgvSearchResultTmp.Rows(intAddRow).Cells.Item(intColCnt).Value = strColData
                Next
                intAddRow = intAddRow + 1
            Next

            '基本設定
            Call SetDataGridBaseInfForStrike(Me.dgvSearchResultTmp)
            ''データ数チェック
            'If pDateTable.Rows.Count = 0 Then
            '    Me.btnDetailTmp.Enabled = False
            '    Me.btnDeleteTmp.Enabled = False
            'Else
            '    Me.btnDetailTmp.Enabled = True
            '    Me.btnDeleteTmp.Enabled = True
            'End If
            '件数表示
            Call SetTempSearchCount()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub

#End Region


#Region "データグリッドビュー基本設定(指名ストライキ画面のデータグリッドで共通的に設定できる項目のみ)"
    '***************************************************************************************************
    '   ＩＤ　：SetDataGridBaseInfForStrike
    '   名称　：データグリッドビューの基本設定
    '   概要  ：データグリッドビューの基本設定を行う（当画面すべてのデータグリッドで共通の設定のみ）
    '   引数　：なし
    '   作成日：2012/01/12 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetDataGridBaseInfForStrike(ByVal pDgv As System.Windows.Forms.DataGridView)
        Try
            pDgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            pDgv.RowsDefaultCellStyle.ForeColor = Color.Black
            pDgv.AllowUserToAddRows = False
            pDgv.AllowUserToDeleteRows = False
            pDgv.ReadOnly = True
            pDgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            pDgv.MultiSelect = False
            pDgv.RowHeadersVisible = False
            pDgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing
            pDgv.ColumnHeadersHeight = 18
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub

#End Region


#Region "データグリッドビューの列作成（活動日検索タブ）"
    '***************************************************************************************************
    '   ＩＤ　：MakeDataGridViewColumns()
    '   名称　：データグリッドビューの列作成を行う
    '   概要  ：活動日検索タブに表示されるデータグリッドビューの列作成を行う
    '   作成日：2012/01/11 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Function MakeActionDateDataGridViewColumns() As Boolean

        Try
            For intCnt As Integer = 0 To ACTIONDATE_DGV_COLUMNS_NAME.Length - 1
                '列名、ヘッダー
                dgvActionSearchResult.Columns.Add(ACTIONDATE_DGV_COLUMNS_NAME(intCnt), ACTIONDATE_DGV_COLUMNS_NAME(intCnt))
                '幅
                dgvActionSearchResult.Columns(intCnt).Width = ACTIONDATE_DGV_COLUMNS_WIDTH(intCnt)
                '可視
                dgvActionSearchResult.Columns(intCnt).Visible = ACTIONDATE_DGV_COLUMNS_VISIBLE(intCnt)

                '***　データの横位置()
                If ACTIONDATE_DGV_COLUMNS_NAME(intCnt) = STAFF_ID Then
                    dgvActionSearchResult.Columns(intCnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                ElseIf ACTIONDATE_DGV_COLUMNS_NAME(intCnt) = TIME_KIND OrElse _
                    ACTIONDATE_DGV_COLUMNS_NAME(intCnt) = STRIKE_TIME OrElse _
                    ACTIONDATE_DGV_COLUMNS_NAME(intCnt) = AREA_LOCAL_COL Then
                    dgvActionSearchResult.Columns(intCnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If
                '***
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

            Return False
        End Try
        Return True

    End Function
#End Region


#Region "データグリッドビューの列作成（通告日検索タブ）"
    '***************************************************************************************************
    '   ＩＤ　：MakeNoticeDateDataGridViewColumns()
    '   名称　：データグリッドビューの列作成を行う
    '   概要  ：通告日検索タブに表示されるデータグリッドビューの列作成を行う
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Function MakeNoticeDateDataGridViewColumns() As Boolean

        Try
            For intCnt As Integer = 0 To NOTICEDATE_DGV_COLUMNS_NAME.Length - 1
                '列名、ヘッダー
                Me.dgvNoticeResult.Columns.Add(NOTICEDATE_DGV_COLUMNS_NAME(intCnt), NOTICEDATE_DGV_COLUMNS_NAME(intCnt))
                '幅
                Me.dgvNoticeResult.Columns(intCnt).Width = NOTICEDATE_DGV_COLUMNS_WIDTH(intCnt)
                '可視
                Me.dgvNoticeResult.Columns(intCnt).Visible = NOTICEDATE_DGV_COLUMNS_VISIBLE(intCnt)

                '***　データの横位置()
                If NOTICEDATE_DGV_COLUMNS_NAME(intCnt) = STRIKE_KIND Then
                    Me.dgvNoticeResult.Columns(intCnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If
                '***
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

            Return False
        End Try
        Return True

    End Function
#End Region


#Region "データグリッドビューの列作成（一時保存タブ）"
    '***************************************************************************************************
    '   ＩＤ　：MakeTempDateDataGridViewColumns()
    '   名称　：データグリッドビューの列作成を行う
    '   概要  ：一時保存タブに表示されるデータグリッドビューの列作成を行う
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Function MakeTempDateDataGridViewColumns() As Boolean

        Try
            For intCnt As Integer = 0 To TEMP_DGV_COLUMNS_NAME.Length - 1
                '列名、ヘッダー
                Me.dgvSearchResultTmp.Columns.Add(TEMP_DGV_COLUMNS_NAME(intCnt), TEMP_DGV_COLUMNS_NAME(intCnt))
                '幅
                Me.dgvSearchResultTmp.Columns(intCnt).Width = TEMP_DGV_COLUMNS_WIDTH(intCnt)
                '可視
                Me.dgvSearchResultTmp.Columns(intCnt).Visible = TEMP_DGV_COLUMNS_VISIBLE(intCnt)

                '***　データの横位置()
                If TEMP_DGV_COLUMNS_NAME(intCnt) = STRIKE_KIND Then
                    Me.dgvSearchResultTmp.Columns(intCnt).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                End If
                '***
            Next
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

            Return False
        End Try
        Return True

    End Function
#End Region


#Region "新規通告処理"
    '***************************************************************************************************
    '   ＩＤ　：NewNotice()
    '   名称　：新規通告
    '   概要  ：新規通告処理を行う
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub NewNotice()
        Dim cForm1 As New FM040404()
        Dim uc040402 As UC040402 = Nothing
        Dim dtGetData As DataRow = Nothing
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)   ' パネルオブジェクト

        Try
            If Not cForm1.ShowLaborDisputeData() Then
                'エラーメッセージ？

            Else
                If cForm1.IntQlickBtnFlag = 0 Then
                    dtGetData = cForm1.SelectDataRow
                    uc040402 = New UC040402
                    '画面間パラメータ渡
                    uc040402.IntClickBtnFlg = 0
                    uc040402.SelectStrikeData = dtGetData
                    '画面呼び出し
                    Call pnl.Controls.Add(uc040402)
                    Me.Visible = False
                    'MessageBox.Show(cForm1.SelectDataRow.Columns(0).ColumnName)
                    'MessageBox.Show(cForm1.SelectDataRow.Rows(0).Item(0))
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        Finally
            cForm1.Close()
            cForm1.Dispose()
        End Try
    End Sub
#End Region


#Region "通告日検索条件設定（活動日付の検索結果ダブルクリック）"
    '***************************************************************************************************
    '   ＩＤ　：SetNoticeDateSearch()
    '   名称　：通告日検索条件設定
    '   概要  ：活動日付検索結果ダブルクリック時に通告日付検索画面の条件を設定する
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Function SetNoticeDateSearch() As Boolean
        Dim returnVal As Boolean = False
        Dim selectRow As DataGridViewRow = Nothing
        Dim strNoticeNumber As String
        Dim strNoticeDate As String
        Dim dtNoticeDate As Date
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Try
            If Me.dgvActionSearchResult.SelectedRows.Count > 0 Then
                selectRow = Me.dgvActionSearchResult.SelectedRows.Item(0)
                strNoticeNumber = selectRow.Cells.Item(NOTICE_NUMBER).Value
                ' - で分割して　通告日検索条件に値を設定する
                Dim intSepaIndex As Integer = strNoticeNumber.IndexOf(SEPALATE_CHAR, 0)
                Me.txtNoticeNumberFirst.Text = strNoticeNumber.Substring(0, intSepaIndex)
                Me.txtNoticeNumberUnder.Text = strNoticeNumber.Substring(intSepaIndex + 1, strNoticeNumber.Length - 1 - intSepaIndex)

                '通告日も同様に告日検索条件に値を設定する
                strNoticeDate = selectRow.Cells.Item(NOTICE_DAY_INF).Value
                dtNoticeDate = Date.Parse(strNoticeDate)
                Me.cmbNoticeTabYear.Text = dtNoticeDate.Year.ToString
                Me.cmbNoticeTabMonth.Text = dtNoticeDate.Month.ToString.PadLeft(2, "0")
                Me.cmbNoticeTabDay.Text = dtNoticeDate.Day.ToString.PadLeft(2, "0")
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            Return False
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
        Return True
    End Function
#End Region


#Region "通告日件数"
    '***************************************************************************************************
    '   ＩＤ　：SetNoticeDateSearch()
    '   名称　：通告日検索条件設定
    '   概要  ：活動日付検索結果ダブルクリック時に通告日付検索画面の条件を設定する
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetNoticeSearchCount()
        Try
            Me.grpResultNotice.Text = "検索結果(" & Me.dgvNoticeResult.Rows.Count.ToString & "件)"
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub
#End Region


#Region "一時保存件数"
    '***************************************************************************************************
    '   ＩＤ　：SetNoticeDateSearch()
    '   名称　：通告日検索条件設定
    '   概要  ：活動日付検索結果ダブルクリック時に通告日付検索画面の条件を設定する
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetTempSearchCount()
        Try
            Me.grpResultTemp.Text = "検索結果(" & Me.dgvSearchResultTmp.Rows.Count.ToString & "件)"
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub
#End Region


#Region "活動日付件数"
    '***************************************************************************************************
    '   ＩＤ　：SetNoticeDateSearch()
    '   名称　：通告日検索条件設定
    '   概要  ：活動日付検索結果ダブルクリック時に通告日付検索画面の条件を設定する
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub SetActionSearchCount()
        Try
            Me.grpResultAction.Text = "検索結果(" & Me.dgvActionSearchResult.Rows.Count.ToString & "件)"
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub
#End Region


#Region "一部解除クリック処理"
    '***************************************************************************************************
    '   ＩＤ　：SetNoticeDateSearch()
    '   名称　：一部解除クリック時の処理
    '   概要  ：一部解除クリック時の処理
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub PartCancel()
        Dim uc040403 As UC040403 = Nothing
        Dim dtSelectData As DataTable = New DataTable   '戻り値格納用
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)   ' パネルオブジェクト
        Dim arrAddData() As String = Nothing        '選択行データ取得配列
        Dim iCounter As Integer
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim strMainSql As String = "" 'SELECT文
        Try
            If Me.dgvNoticeResult.SelectedRows.Count > 0 Then
                'すでに解除通知だったらエラー
                If Me.dgvNoticeResult.SelectedRows.Item(0).Cells(STRIKE_KIND).Value = MDNameStrikeCommon.GetStrikeKindMeaning("02") Then
                    Call CLMsg.Show("GE0078")
                    Exit Sub
                End If

                
                '列取得
                'いったんデータテーブルで取得
                For intColCnt As Integer = 0 To dgvNoticeResult.ColumnCount - 1
                    dtSelectData.Columns.Add(NOTICEDATE_DGV_COLUMNS_NAME(intColCnt))
                Next
                'データ取得
                iCounter = 0
                For Each GetCell As DataGridViewCell In dgvNoticeResult.SelectedRows.Item(0).Cells()
                    ReDim Preserve arrAddData(iCounter)
                    If Not (IsDBNull(GetCell.Value)) Then
                        arrAddData(iCounter) = GetCell.Value.ToString
                    Else
                        arrAddData(iCounter) = ""
                    End If
                    iCounter = iCounter + 1
                Next
                dtSelectData.Rows.Add(arrAddData)

                'すでに全員解除されていたらエラー
                strMainSql = "SELECT " & vbCrLf
                strMainSql = strMainSql & " name_strike_member_date.c_name_strike_id  " & vbCrLf
                strMainSql = strMainSql & "FROM name_strike_member_date " & vbCrLf
                strMainSql = strMainSql & "WHERE c_name_strike_id = '" & dtSelectData.Rows(0).Item(NOTICE_NUMBER) & "'" & vbCrLf
                strMainSql = strMainSql & "AND (c_cancel_name_strike_id = '' OR c_cancel_name_strike_id IS NULL) "
                clsDb.Connect()
                Dim dtSqlResult As DataTable = clsDb.ExecuteSql(strMainSql)

                If dtSqlResult.Rows.Count = 0 Then
                    Call CLMsg.Show("GE0086")
                    Exit Sub
                End If


                uc040403 = New UC040403
                '画面間パラメータ渡
                uc040403.IntClickBtnFlg = 1
                uc040403.SelectNameStrikeData = dtSelectData.Rows.Item(0)
                '画面呼び出し
                Call pnl.Controls.Add(uc040403)
                Me.Visible = False

            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        Finally
            clsDb.Disconnect()
        End Try
    End Sub
#End Region


#Region "詳細クリック処理"
    '***************************************************************************************************
    '   ＩＤ　：DetailShow()
    '   名称　：詳細クリック時の処理
    '   概要  ：詳細クリック時の処理
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub DetailShow()
        Dim uc040402 As UC040402 = Nothing
        Dim uc040403 As UC040403 = Nothing
        Dim ucOpen As UserControl = Nothing
        Dim dtSelectData As DataTable = New DataTable   '戻り値格納用
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)   ' パネルオブジェクト
        Dim arrAddData() As String = Nothing        '選択行データ取得配列
        Dim iCounter As Integer
        Try
            Me.Cursor = Cursors.WaitCursor

            If Me.dgvNoticeResult.SelectedRows.Count > 0 Then

                '***データテーブルで選択行情報取得
                For intColCnt As Integer = 0 To dgvNoticeResult.ColumnCount - 1
                    dtSelectData.Columns.Add(NOTICEDATE_DGV_COLUMNS_NAME(intColCnt))
                Next
                iCounter = 0
                For Each GetCell As DataGridViewCell In dgvNoticeResult.SelectedRows.Item(0).Cells()
                    ReDim Preserve arrAddData(iCounter)
                    If Not (IsDBNull(GetCell.Value)) Then
                        arrAddData(iCounter) = GetCell.Value.ToString
                    Else
                        arrAddData(iCounter) = ""
                    End If
                    iCounter = iCounter + 1
                Next
                dtSelectData.Rows.Add(arrAddData)
                '***データテーブルで選択行情報取得終了

                '解除通知だった場合
                If Me.dgvNoticeResult.SelectedRows.Item(0).Cells(STRIKE_KIND).Value = MDNameStrikeCommon.GetStrikeKindMeaning("02") Then
                    uc040403 = New UC040403
                    '画面間パラメータ渡
                    uc040403.IntClickBtnFlg = 3
                    uc040403.SelectNameStrikeData = dtSelectData.Rows.Item(0)
                    ucOpen = uc040403
                Else
                    uc040402 = New UC040402
                    '画面間パラメータ渡
                    uc040402.IntClickBtnFlg = 2
                    uc040402.SelectNameStrikeData = dtSelectData.Rows.Item(0)
                    ucOpen = uc040402
                End If

                Call pnl.Controls.Add(ucOpen)
                Me.Visible = False

            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Me.Cursor = Cursors.Default
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
#End Region


#Region "一時保存詳細クリック処理"
    '***************************************************************************************************
    '   ＩＤ　：TmpDetailShow()
    '   名称　：詳細クリック時の処理
    '   概要  ：詳細クリック時の処理
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub TempDetailShow()
        Dim uc040402 As UC040402 = Nothing
        Dim uc040403 As UC040403 = Nothing
        Dim ucOpen As UserControl = Nothing
        Dim dtSelectData As DataTable = New DataTable   '戻り値格納用
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)   ' パネルオブジェクト
        Dim arrAddData() As String = Nothing        '選択行データ取得配列
        Dim iCounter As Integer
        Try
            If Me.dgvSearchResultTmp.SelectedRows.Count > 0 Then

                '***データテーブルで選択行情報取得
                For intColCnt As Integer = 0 To dgvSearchResultTmp.ColumnCount - 1
                    dtSelectData.Columns.Add(TEMP_DGV_COLUMNS_NAME(intColCnt))
                Next
                iCounter = 0
                For Each GetCell As DataGridViewCell In dgvSearchResultTmp.SelectedRows.Item(0).Cells()
                    ReDim Preserve arrAddData(iCounter)
                    If Not (IsDBNull(GetCell.Value)) Then
                        arrAddData(iCounter) = GetCell.Value.ToString
                    Else
                        arrAddData(iCounter) = ""
                    End If
                    iCounter = iCounter + 1
                Next
                dtSelectData.Rows.Add(arrAddData)
                '***データテーブルで選択行情報取得終了

                '解除通知だった場合
                If Me.dgvSearchResultTmp.SelectedRows.Item(0).Cells(STRIKE_KIND).Value = MDNameStrikeCommon.GetStrikeKindMeaning("02") Then
                    uc040403 = New UC040403
                    '画面間パラメータ渡
                    uc040403.IntClickBtnFlg = 5
                    uc040403.SelectNameStrikeData = dtSelectData.Rows.Item(0)
                    ucOpen = uc040403
                Else
                    uc040402 = New UC040402
                    '画面間パラメータ渡
                    uc040402.IntClickBtnFlg = 4
                    uc040402.SelectNameStrikeData = dtSelectData.Rows.Item(0)
                    ucOpen = uc040402
                End If

                Call pnl.Controls.Add(ucOpen)
                Me.Visible = False

            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub

    
#End Region


#Region "活動日付タブ印刷ボタン"
    '***************************************************************************************************
    '   ＩＤ　：PrintStrikeMember()
    '   名称　：通告日検索条件設定
    '   概要  ：活動日付検索結果ダブルクリック時に通告日付検索画面の条件を設定する
    '   作成日：2012/01/17 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub PrintStrikeMember()

        Dim fmPrint As New FM000203     'レポート画面
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument  'レポート

        Try
            'データセット作成
            Dim dsReportData As DS0404P1 = GetStrikeMemberInf()

            'レポートヘッダー部追加
            Dim dhrHeader As DS0404P1.dtHeaderRow
            dhrHeader = dsReportData.dtHeader.NewRow
            dhrHeader.BeginEdit()
            dhrHeader._date = Me.cmbActionTabYear.Text + "/" + Me.cmbActionTabMonth.Text + "/" + Me.cmbActionTabDay.Text
            dhrHeader.EndEdit()
            dsReportData.dtHeader.Rows.Add(dhrHeader)

            '
            reportObj = New CR0404P1
            fmPrint.ButtonShowType = 3              '「印刷」「キャンセル」ボタン表示
            fmPrint.ObjResource = reportObj         'レポートファイル
            reportObj.SetDataSource(dsReportData)   'データセット
            fmPrint.ShowDialog()                    '印刷画面表示

            If fmPrint.IntQlickBtnFlag = 3 Then
                fmPrint.PrintOut()
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Sub
#End Region


#Region "活動日付検索結果情報取得(レポート用)"""
    '***************************************************************************************************
    '   ＩＤ　：GetStrikeMemberInf()
    '   名称　：活動日付検索結果情報取得
    '   概要  ：レポート用に活動日付検索結果の情報取得を行う。
    '   作成日：2012/01/17 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    'Private Function GetStrikeMemberInf(ByVal dtResource As DataTable) As DS0404P1
    Private Function GetStrikeMemberInf() As DS0404P1
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")
        Dim dtsReturn As DS0404P1 = New DS0404P1()
        Try
            If Me.dgvActionSearchResult.Rows.Count > 0 Then
                Dim drDetail As DS0404P1.dtDetailRow
                For Each row As DataGridViewRow In dgvActionSearchResult.Rows
                    drDetail = dtsReturn.dtDetail.NewRow
                    drDetail.BeginEdit()
                    drDetail._24h = NVL(row.Cells.Item(TIME_KIND).Value).ToString
                    drDetail.k_local = NVL(row.Cells.Item(AREA_LOCAL_COL).Value).ToString
                    drDetail.time_frame = NVL(row.Cells.Item(STRIKE_TIME).Value)
                    drDetail.c_staf_id = NVL(row.Cells.Item(STAFF_ID).Value)
                    drDetail.l_name = NVL(row.Cells.Item(STAFF_NAME).Value)
                    drDetail.name_strike_info = NVL(row.Cells.Item(NOTICE_NUMBER).Value)
                    drDetail.info = NVL(row.Cells.Item(NOTICE_DAY_INF).Value)
                    drDetail.staf_id_ins = ""
                    drDetail.name_ins = ""
                    drDetail.staf_id_up = NVL(row.Cells.Item(NOTICE_UPDATE_USER_ID).Value)
                    drDetail.name_up = NVL(row.Cells.Item(NOTICE_UPDATE_USER).Value)
                    drDetail.attendance = ""
                    drDetail.fight = NVL(row.Cells.Item(FIGHT_ID).Value)
                    drDetail.cancel_name_strike_info = NVL(row.Cells.Item(CANCEL_NOTICE_NUMBER).Value)
                    drDetail.cancel_info = NVL(row.Cells.Item(CANCEL_DAY_INF).Value)
                    drDetail.cancel_staf_id_ins = ""
                    drDetail.cancel_name_ins = ""
                    drDetail.cancel_staf_id_up = NVL(row.Cells.Item(CANCE_UPDATE_USER_ID).Value)
                    drDetail.cancel_name_up = NVL(row.Cells.Item(CANCE_UPDATE_USER).Value)
                    drDetail.cancel = NVL(row.Cells.Item(CANCEL_ID).Value)
                    drDetail.l_biko = NVL(row.Cells.Item(BIKOU).Value)
                    drDetail.EndEdit()
                    dtsReturn.dtDetail.Rows.Add(drDetail)
                Next
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")
        Return dtsReturn
    End Function
#End Region


#Region "一時保存削除"
    '***************************************************************************************************
    '   ＩＤ　：DeleteTempSaving
    '   名称　：一時保存削除
    '   概要  ：一時保存削除部分
    '   引数　：なし
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Function DeleteTempSaving() As Boolean
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成
        Dim strDelSql As String = "" 'SELECT文
        Dim strDelMemberSql As String = "" 'SELECT文のWHERE句
        'SQL実行結果
        Dim intRet As Integer = -1
        '処理結果
        Dim blnRet As Boolean = True
        Try
            Dim delIndex As String = dgvSearchResultTmp.SelectedRows.Item(0).Cells(TEMP_INDEX).Value.ToString
            strDelSql = "DELETE FROM name_strike_work WHERE s_index = " & delIndex
            strDelMemberSql = "DELETE FROM name_strike_member_date_work WHERE s_index = " & delIndex

            clsDb.Connect()
            clsDb.BeginTran()
            
            'SQL実行
            intRet = clsDb.ExecuteNonQuery(strDelSql)
            If intRet = -1 Then
                'コミット
                clsDb.RollbackTran()
                log.Error("DB更新処理に異常があったため削除処理を中止しました。")
                blnRet = False
            End If

            If blnRet Then
                intRet = clsDb.ExecuteNonQuery(strDelMemberSql)
                If intRet = -1 Then
                    clsDb.RollbackTran()
                    log.Error("DB更新処理に異常があったため削除処理を中止しました。")
                    blnRet = False
                End If
            End If

            If blnRet Then
                clsDb.CommitTran()
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        Finally
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function
#End Region


#Region "検索結果画面初期化関連"
    '***************************************************************************************************
    '   ＩＤ　：DeleteTempSaving
    '   名称　：一時保存削除
    '   概要  ：一時保存削除部分
    '   引数　：なし
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Public Function ClearActionSearch() As Boolean
        Try
            Me.dgvActionSearchResult.Rows.Clear()
            Me.dgvActionSearchResult.Columns.Clear()
            Me.grpResultAction.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Function

    '***************************************************************************************************
    '   ＩＤ　：DeleteTempSaving
    '   名称　：一時保存削除
    '   概要  ：一時保存削除部分
    '   引数　：なし
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Public Function ClearNoticeSearch() As Boolean
        Try
            Me.dgvNoticeResult.Rows.Clear()
            Me.dgvNoticeResult.Columns.Clear()
            Me.grpResultNotice.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Function

    '***************************************************************************************************
    '   ＩＤ　：DeleteTempSaving
    '   名称　：一時保存削除
    '   概要  ：一時保存削除部分
    '   引数　：なし
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Public Function ClearTempSearch() As Boolean
        Try
            Me.dgvSearchResultTmp.Rows.Clear()
            Me.dgvSearchResultTmp.Columns.Clear()
            Me.grpResultTemp.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

        End Try
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GrantValSetButtonEnable
    '   名称　：権限によるボタンの切り替え
    '   概要  ：権限によるボタンの切り替え
    '   引数　：なし
    '   作成日：2012/01/18 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub GrantValSetButtonEnable()
        If _strGrantReference <> GRANT_VALID Then
            '参照権限がない場合
            btnSearchActionDate.Enabled = False
            btnSearchNoticeDay.Enabled = False
            btnSearchTmp.Enabled = False
        End If

        If _strGrantPrint <> GRANT_VALID Then
            '印刷権限がない場合
            btnActionPrint.Enabled = False
        End If

        If _strGrantInsert <> GRANT_VALID Then
            '登録権限がない場合
            btnNewNotice.Enabled = False
            btnPartCancel.Enabled = False
            btnDeleteTmp.Enabled = False
        End If
    End Sub
#End Region


#End Region

    
End Class

#End Region
