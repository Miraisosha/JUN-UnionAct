'===========================================================================================================
'   クラスＩＤ　　：UC040301
'   クラス名称　　：時間内組合活動画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Document
Imports UnionAct.GUI.Common

Public Class UC040301

#Region "定数"
    Private Const SCREEN_ID As String = SCREEN_ID_UC040301
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040301

    '申請番号の分割文字
    Private Const SEPALATE_CHAR As String = "-"
    Private strDelFlg As Boolean
#End Region

#Region "プロパティ"
    Public _blnRefFlg As Byte = 0           ' 検索フラグ（0：検索なし, 1：活動日付検索, 2：申請番号検索）
    ' 検索フラグ
    Public Property blnRefFlg() As Byte
        Get
            Return _blnRefFlg
        End Get
        Set(ByVal value As Byte)
            _blnRefFlg = value
        End Set
    End Property
#End Region

#Region "内部プロパティ"
    '権限フラグ
    Private IsReferenceRight As Boolean
    Private IsGetEntryRight As Boolean
    Private IsPrintRight As Boolean
    Private IsFileOutputRight As Boolean
#End Region

#Region "ログ出力オブジェクト"
    'ログ出力定義
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "イベント"
#Region "イベント：フォームロード"
    '************************************************************************************
    '   ＩＤ　：UC040301_Load
    '   名称　：フォームロード
    '   概要　：フォームロード
    '   作成日：2012/01/06(金) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) Ryu  新規作成
    '************************************************************************************
    Private Sub UC040301_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim dtKengen As DataTable = Nothing

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            '-------------------------------------------------------------------------------
            '   権限設定処理
            '-------------------------------------------------------------------------------
            '権限の取得
            dtKengen = MDCommon.getGrant(MENU_ID_UC040301)

            If dtKengen.Rows.Count > 0 Then
                IsReferenceRight = IIf(dtKengen.Rows(0).Item(3).ToString = "1", True, False)
                IsGetEntryRight = IIf(dtKengen.Rows(0).Item(4).ToString = "1", True, False)
                IsPrintRight = IIf(dtKengen.Rows(0).Item(5).ToString = "1", True, False)
                IsFileOutputRight = IIf(dtKengen.Rows(0).Item(6).ToString = "1", True, False)
            End If

            '参照権限
            If Me.IsReferenceRight Then
                Me.btnCancel.Enabled = True
                Me.btnDetail.Enabled = True
                Me.btnRemainTimes.Enabled = True
            Else
                Me.btnCancel.Enabled = False
                Me.btnDetail.Enabled = False
                Me.btnRemainTimes.Enabled = False
            End If

            '登録権限
            If Me.IsGetEntryRight Then
                Me.btnApply.Enabled = True
                Me.btnCancel.Enabled = True
            Else
                Me.btnApply.Enabled = False
                Me.btnCancel.Enabled = False
            End If

            '印刷権限
            If Me.IsPrintRight Then
                btnPrinting.Enabled = True
            Else
                btnPrinting.Enabled = False
            End If

            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            ' 会社所属・支部コンボボックスデータ取得
            If Me.GetComData() = False Then
                Exit Sub
            End If

            '基準日取得（最新期：現在日、最新期以外：期末日）
            Dim systemDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            Dim periodDFrom As String = MDLoginInfo.PeriodFrom
            Dim periodDTo As String = MDLoginInfo.PeriodTo
            Dim standDate As String
            If (MDLoginInfo.PeriodNewFlg = 1) Then
                standDate = systemDate
            Else
                standDate = periodDTo
            End If

            Dim standYear As String = standDate.Substring(0, 4)
            Dim standMonth As String = standDate.Substring(4, 2)
            Dim standDay As String = standDate.Substring(6, 2)


            '年コンボボックスデータ設定
            If NSMDCommon.CreateComboBoxYear(Me.cmbActivityYear, False) = False Then
                Exit Sub
            End If

            If NSMDCommon.CreateComboBoxYear(Me.cmbApplyYear, False) = False Then
                Exit Sub
            End If

            '月コンボボックスデータ設定
            Me.cmbActivityMonth.Items.AddRange(New Object() {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})
            Me.cmbApplyMonth.Items.AddRange(New Object() {"", "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12"})

            '日コンボボックスデータ設定
            Call Me.SetDaysInMonth(Me.cmbActivityDay, standYear, standMonth, False)
            'Call Me.SetDaysInMonth(Me.cmbApplyDay, standYear, standMonth, True)

            'コンボボックス初期値設定
            Me.cmbActivityYear.Text = standYear
            Me.cmbActivityMonth.Text = standMonth
            Me.cmbActivityDay.Text = standDay

            Me.cmbApplyYear.Text = standYear
            'Me.cmbApplyMonth.Text = standMonth

            '申請番号入力欄初期化
            txtApplyNumber1.Enabled = True
            txtApplyNumber1.Text = ""
            txtApplyNumber2.Enabled = True
            txtApplyNumber2.Text = ""

            'フォーカスの初期設定
            Me.btnSearch1.Focus()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：フォーム表示チェンジ"
    Private Sub UC040301_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)              ' ログ出力（処理開始）
        Try
            If Me.blnRefFlg = 1 Then
                ' 活動日付検索
                If Me.GetSearchActivity() = False Then
                    Exit Sub
                End If
            ElseIf Me.blnRefFlg = 2 Then
                ' 申請番号検索
                If Me.GetSearchApply() = False Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                                   ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' 一度表示したら初期値に戻す
            Me.blnRefFlg = 0
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)              ' ログ出力（処理終了）
    End Sub
#End Region

#Region "イベント：検索ボタン(活動日付検索)クリック"
    '************************************************************************************
    '   ＩＤ　：activitySearch_Click
    '   名称　：検索ボタン(活動日付検索)クリック
    '   概要　：
    '   作成日：2012/01/13(金)
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) Fujisaku  新規作成
    '************************************************************************************
    Private Sub activitySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch1.Click

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 検索データ取得処理呼び出し
            If Me.GetSearchActivity() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：検索ボタン(申請番号検索)クリック"
    '************************************************************************************
    '   ＩＤ　：applySearch_Click
    '   名称　：検索ボタン(申請番号検索)クリック
    '   概要　：
    '   作成日：2012/01/16(月)
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) Fujisaku  新規作成
    '************************************************************************************
    Private Sub applySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch2.Click

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 検索データ取得処理呼び出し
            If Me.GetSearchApply() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：新規申請ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnApply_Click
    '   名称　：新規申請ボタンクリック処理
    '   概要　：
    '   作成日：2012/01/06(金) Ryu
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) Ryu  新規作成
    '***************************************************************************************************
    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click

        Dim cForm1 As New FM040303(Me)

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            Dim curDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            Dim msgRtn As DialogResult
            Dim blnSkip As Boolean = False

            ' 現在期以外でログインしている場合にメッセージ表示
            If curDate < MDLoginInfo.PeriodFrom Or MDLoginInfo.PeriodTo < curDate Then
                If MDLoginInfo.CommitteeStatusFlg = 1 Then
                    ' 組合員の場合はエラーとして申請を行わない
                    CLMsg.Show("GE0234", "新規申請")
                    blnSkip = True
                Else
                    ' 専従業員・管理部の場合は警告を表示
                    msgRtn = CLMsg.Show("GW0039", "新規申請")
                    If Not msgRtn = DialogResult.Yes Then
                        blnSkip = True
                    End If
                End If
            End If

            If Not blnSkip Then
                cForm1.Text = "種類の選択"

                ' 種類の選択画面 をモーダルで表示する
                cForm1.ShowDialog()

                ' 不要になった時点で破棄する
                cForm1.Dispose()
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：取消ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：取消ボタンクリック処理
    '   概要　：
    '   作成日：2012/01/21(土) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/21(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' 処理開始ログ
        Try
            '---------------------------------------------------------------------------
            '   時間内組合活動 - 取消画面遷移処理
            '---------------------------------------------------------------------------
            If ShowUC040306() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)    ' 処理終了ログ
    End Sub
#End Region

#Region "イベント：詳細ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnDetail_Click
    '   名称　：詳細ボタンクリック処理
    '   概要　：
    '   作成日：2012/01/21(土) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/21(土) m.suzuki  新規作成
    '***************************************************************************************************
    Private Sub btnDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetail.Click

        ' 処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try

            '---------------------------------------------------------------------------
            '   時間内組合活動 - 詳細画面遷移処理
            '---------------------------------------------------------------------------
            FrmWaitInfo.ShowWaitForm(Nothing)
            If ShowUC040302() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            FrmWaitInfo.CloseWaitForm()
        End Try
        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：残数表示ボタンクリック"
    Private Sub Button10_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemainTimes.Click

        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            Dim cForm1 As New FM040305()

            ' モーダルで表示する
            cForm1.ShowDialog()

            ' 不要になった時点で破棄する (正しくは オブジェクトの破棄を保証する を参照)
            cForm1.Dispose()

        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            'カーソルを戻す
            Me.Cursor = Cursors.Default
        End Try

    End Sub
#End Region

#Region "イベント：プレ印刷ボタンクリック"
    '************************************************************************************
    '   ＩＤ　：btnPrinting_Click
    '   名称　：プレ印刷ボタンクリック
    '   概要　：
    '   作成日：2012/01/20(金)
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) Fujisaku  新規作成
    '************************************************************************************
    Private Sub btnPrinting_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrinting.Click

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Dim fmPrint As New FM000203     'レポート画面
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument  'レポート

        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            'データセット作成
            Dim dsReportData As DS0403P3 = GetApplyMemberInf()

            'レポートヘッダー部追加
            Dim dhrHeader As DS0403P3.dtHeaderRow
            dhrHeader = dsReportData.dtHeader.NewRow
            dhrHeader.BeginEdit()
            dhrHeader._date = Me.cmbActivityYear.Text & "年 " & Me.cmbActivityMonth.Text & "月 " & Me.cmbActivityDay.Text & "日"
            dhrHeader.EndEdit()
            dsReportData.dtHeader.Rows.Add(dhrHeader)

            'レポート形式の設定
            reportObj = New CR0403P3
            fmPrint.ButtonShowType = 3              '「印刷」「キャンセル」ボタン表示
            fmPrint.PrintCntVisible = True          'ページ設定あり
            fmPrint.ObjResource = reportObj         'レポートファイル
            reportObj.SetDataSource(dsReportData)   'データセット

            Call fmPrint.ShowDialog()               '印刷画面表示

            If fmPrint.IntQlickBtnFlag = 3 Then     '「印刷」押下時のみ印刷実行
                fmPrint.PrintOut()
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            'カーソルを戻す
            Me.Cursor = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

#Region "イベント：年コンボボックス(活動日付検索)値変更"
    Private Sub cmbActivityYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActivityYear.SelectedIndexChanged
        Try
            '活動日付検索の検索結果をクリア
            Call ClearActivitySearch()

            '月が選択されている場合のみ
            If Not ChkNull(Me.cmbActivityMonth.Text) Then
                ' 日ダイアログの更新
                Call SetDaysInMonth(Me.cmbActivityDay, Me.cmbActivityYear.Text, Me.cmbActivityMonth.Text, False)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：月コンボボックス(活動日付検索)値変更"
    Private Sub cmbActivityMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActivityMonth.SelectedIndexChanged
        Try
            '活動日付検索の検索結果をクリア
            Call ClearActivitySearch()

            ' 日ダイアログの更新
            Call SetDaysInMonth(Me.cmbActivityDay, Me.cmbActivityYear.Text, Me.cmbActivityMonth.Text, False)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：日コンボボックス(活動日付検索)値変更"
    Private Sub cmbActivityDay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbActivityDay.SelectedIndexChanged
        Try
            '活動日付検索の検索結果をクリア
            Call ClearActivitySearch()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：会社所属コンボボックス(活動日付検索)値変更"
    Private Sub cmbAreaLocal_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbAreaLocal.SelectedIndexChanged
        Try
            '活動日付検索の検索結果をクリア
            Call ClearActivitySearch()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：支部コンボボックス(申請番号検索)値変更"
    Private Sub cmbApplyArea_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbApplyArea.SelectedIndexChanged
        Try
            '申請番号検索の検索結果をクリア
            Call ClearApplySearch()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：年コンボボックス(申請番号検索)値変更"
    Private Sub cmbApplyYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbApplyYear.SelectedIndexChanged
        Try
            '申請番号検索の検索結果をクリア
            Call ClearApplySearch()

            '月が選択されている場合のみ
            If Not ChkNull(Me.cmbApplyMonth.Text) Then
                ' 日ダイアログの更新
                Call SetDaysInMonth(Me.cmbApplyDay, Me.cmbApplyYear.Text, Me.cmbApplyMonth.Text, True)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：月コンボボックス(申請番号検索)値変更"
    Private Sub cmbApplyMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbApplyMonth.SelectedIndexChanged
        Try
            '申請番号検索の検索結果をクリア
            Call ClearApplySearch()

            ' 日ダイアログの更新
            Call SetDaysInMonth(Me.cmbApplyDay, Me.cmbApplyYear.Text, Me.cmbApplyMonth.Text, True)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：日コンボボックス(申請番号検索)値変更"
    Private Sub cmbApplyDay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbApplyDay.SelectedIndexChanged
        Try
            '申請番号検索の検索結果をクリア
            Call ClearApplySearch()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：申請番号前桁(申請番号検索)値変更"
    Private Sub txtApplyNumber1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtApplyNumber1.TextChanged
        Try
            '申請番号検索の検索結果をクリア
            Call ClearApplySearch()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：申請番号後桁(申請番号検索)値変更"
    Private Sub txtApplyNumber2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtApplyNumber2.TextChanged
        Try
            '申請番号検索の検索結果をクリア
            Call ClearApplySearch()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "イベント：活動日付検索、データグリッドダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgvActionSearchResult_CellDoubleClick
    '   名称　：ダブルクリック
    '   概要  ：
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub flxActivityDate_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles flxActivityDate.CellDoubleClick

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            If e.RowIndex <> -1 Then
                ' 選択したレコードを元に検索条件設定
                If SetApplySearchCondition() = False Then
                    Exit Sub
                End If

                '申請番号検索を実施
                Call GetSearchApply()

                '申請番号検索タブへ移動
                Me.tbcMemberActivityInTerm.SelectedIndex = 1
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#Region "イベント：申請番号検索、データグリッドダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgvActionSearchResult_CellDoubleClick
    '   名称　：ダブルクリック
    '   概要  ：
    '   作成日：2012/01/16 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    Private Sub flxApply_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles flxApply.CellDoubleClick

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            If e.RowIndex <> -1 Then
                '詳細画面遷移を実行
                FrmWaitInfo.ShowWaitForm(Nothing)
                If ShowUC040302() = False Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            FrmWaitInfo.CloseWaitForm()
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

#End Region

#Region "関数"
#Region "関数：コンボボックスデータ取得"
    '************************************************************************************
    '   ＩＤ　：GetComData
    '   名称　：コンボボックスデータ取得
    '   概要　：会社所属・支部コンボボックスデータ取得
    '   作成日：2012/01/06(金) Ryu
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) Ryu  新規作成
    '************************************************************************************
    Private Function GetComData() As Boolean

        Dim blnRet As Boolean = False           ' 処理結果
        Dim clsDb As New CLAccessMdb            ' データベースクラス生成

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' データベース接続
            clsDb.Connect()

            ' 活動日付検索タブ
            ' 会社所属コンボボックス作成処理呼び出し
            If Not CreateCboConstantDtl(clsDb, Me.cmbAreaLocal, "BELONGING") Then
                Return blnRet
            End If

            ' 申請番号検索タブ
            ' 支部コンボボックス作成処理呼び出し
            If Not CreateCboConstantDtl(clsDb, Me.cmbApplyArea, "APPLY_AREA") Then
                Return blnRet
            End If

            ' 処理結果に正常を格納
            blnRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            ' データベース切断
            clsDb.Disconnect()
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "関数：日コンボボックス設定処理"
    '************************************************************************************
    '   ＩＤ　：SetDaysInMonth
    '   名称　：日コンボボックス設定処理
    '   概要　：日コンボボックス設定処理
    '   作成日：2012/01/14(土)
    '   備考  ：True：空欄あり、False：空欄なし
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/14(土) Fujisaku  新規作成
    '************************************************************************************
    Private Function SetDaysInMonth(ByVal pComboDay As ComboBox, ByVal strYear As String, ByVal strMonth As String, Optional ByVal emptySet As Boolean = False) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            Dim intDays As Integer
            Dim intCounter As Integer

            ' チラつき防止の為、最後まで描写しない
            pComboDay.BeginUpdate()
            pComboDay.Items.Clear()

            ' 空欄有りの場合追加
            If emptySet Then
                pComboDay.Items.Add("")
            End If

            ' 指定の年月に合わせて設定
            If (Not strYear.Equals("") AndAlso Not strMonth.Equals("")) Then
                intDays = Date.DaysInMonth(CInt(strYear), CInt(strMonth))
                For intCounter = 1 To intDays
                    pComboDay.Items.Add(CStr(intCounter).PadLeft(2, "0"c))
                Next

            End If

            ' チラつき防止の為、最後に描写
            pComboDay.EndUpdate()
            pComboDay.SelectedIndex = 0

            ' 処理結果に正常を格納
            blnRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        Return blnRet
    End Function
#End Region

#Region "関数：活動日付検索処理"
    '************************************************************************************
    '   ＩＤ　：GetSearchActivity
    '   名称　：活動日付検索
    '   概要　：活動日付検索
    '   作成日：2012/01/14(土)
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/14(土) Fujisaku  新規作成
    '************************************************************************************
    Private Function GetSearchActivity() As Boolean
        Dim blnRet As Boolean = False               ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim strYearSearch As String                 ' 年
        Dim strMonthSearch As String                ' 月
        Dim strDaySearch As String                  ' 日
        Dim strLocalSearch As String                ' 会社所属
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            'グリッドを初期化
            Call Me.flxActivityDate.Rows.Clear()

            ' 検索条件：対象年月日、会社所属
            strYearSearch = Me.cmbActivityYear.Text
            strMonthSearch = Me.cmbActivityMonth.Text
            strDaySearch = Me.cmbActivityDay.Text

            If Me.cmbAreaLocal.SelectedIndex > 0 Then
                strLocalSearch = Me.cmbAreaLocal.SelectedValue
            Else
                strLocalSearch = ""
            End If

            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = ""
            strSql = strSql & "SELECT" & vbCrLf
            strSql = strSql & "    a_strike.l_local_name," & vbCrLf
            strSql = strSql & "    a_strike.c_staf_id," & vbCrLf
            strSql = strSql & "    a_strike.l_staf_name," & vbCrLf
            strSql = strSql & "    a_strike.c_strike_id," & vbCrLf
            strSql = strSql & "    a_strike.d_application," & vbCrLf
            strSql = strSql & "    a_strike.c_cancel_strike_id," & vbCrLf
            strSql = strSql & "    a_strike.d_cancel_application," & vbCrLf
            strSql = strSql & "    a_strike.l_apply_classify_name," & vbCrLf
            strSql = strSql & "    a_strike.l_meeting," & vbCrLf
            strSql = strSql & "    a_strike.l_up_user_name," & vbCrLf
            strSql = strSql & "    a_strike.c_user_id_up," & vbCrLf
            strSql = strSql & "    a_strike.c_cancel_user_id_up," & vbCrLf
            strSql = strSql & "    a_strike.l_cancel_up_user_name" & vbCrLf
            strSql = strSql & "FROM " & vbCrLf
            strSql = strSql & "    apply_strike_date_view a_strike " & vbCrLf
            strSql = strSql & "WHERE " & vbCrLf
            strSql = strSql & "     FORMAT(a_strike.d_strike, 'yyyyMMdd') = '" & strYearSearch & strMonthSearch & strDaySearch & "'" & vbCrLf
            strSql = strSql & " AND a_strike.k_cancel = '0'" & vbCrLf
            strSql = strSql & " AND a_strike.k_replace = '0'" & vbCrLf
            If Not ChkNull(strLocalSearch) Then
                ' 会社所属は選択時のみ検索条件追加
                strSql = strSql & " AND a_strike.k_local = '" & strLocalSearch & "'" & vbCrLf
            End If
            strSql = strSql & "ORDER BY " & vbCrLf
            strSql = strSql & "    a_strike.k_local," & vbCrLf
            strSql = strSql & "    CLng(a_strike.c_staf_id)" & UtDb.DbOrderOffset & vbCrLf  'ok

            ' DB接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' グリッド件数と件数表示の設定
            Me.flxActivityDate.RowCount = intRetCnt
            Me.grpActResult.Text = "検索結果 (" + intRetCnt.ToString + "件)"

            ' レコード数分ループ
            For i = 0 To intRetCnt - 1
                ' データ設定
                With Me.flxActivityDate.Rows(i).Cells
                    .Item(0).Value = NVL(tbRet.Rows(i).Item("l_local_name"))       ' 所属
                    .Item(1).Value = NVL(tbRet.Rows(i).Item("c_staf_id"))          ' 社員番号
                    .Item(2).Value = NVL(tbRet.Rows(i).Item("l_staf_name"))        ' 氏名
                    .Item(3).Value = NVL(tbRet.Rows(i).Item("c_strike_id"))        ' 申請番号
                    .Item(4).Value = NVL(tbRet.Rows(i).Item("d_application"))      ' 申請日付
                    .Item(5).Value = NVL(tbRet.Rows(i).Item("c_cancel_strike_id")) ' 取消番号
                    .Item(6).Value = NVL(tbRet.Rows(i).Item("d_cancel_application"))    ' 取消日付
                    .Item(7).Value = NVL(tbRet.Rows(i).Item("l_apply_classify_name"))   ' 種類区分
                    .Item(8).Value = NVL(tbRet.Rows(i).Item("l_meeting"))          ' 会議名
                    .Item(9).Value = NVL(tbRet.Rows(i).Item("l_up_user_name"))     ' 更新者氏名

                    .Item(10).Value = NVL(tbRet.Rows(i).Item("c_user_id_up"))           ' 更新者個人認証ＩＤ(印刷用非表示項目)
                    .Item(11).Value = NVL(tbRet.Rows(i).Item("c_cancel_user_id_up"))    ' 取消更新者個人認証ＩＤ(印刷用非表示項目)
                    .Item(12).Value = NVL(tbRet.Rows(i).Item("l_cancel_up_user_name"))  ' 取消更新者氏名(印刷用非表示項目)
                End With
            Next

            ' 検索が正常の場合はVisibleの設定
            Me.grpActResult.Visible = True

            ' 検索結果0件の場合は印刷ボタンを無効化
            If intRetCnt = 0 Then
                Me.btnPrinting.Enabled = False
            Else
                If Me.IsPrintRight Then
                    Me.btnPrinting.Enabled = True
                Else
                    Me.btnPrinting.Enabled = False
                End If
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
            'カーソルを戻す
            Me.Cursor = Cursors.Default
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        Return blnRet
    End Function
#End Region

#Region "関数：申請番号検索処理"
    '************************************************************************************
    '   ＩＤ　：GetSearchApply
    '   名称　：申請番号検索
    '   概要　：申請番号検索
    '   作成日：2012/01/16(月)
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) Fujisaku  新規作成
    '************************************************************************************
    Private Function GetSearchApply() As Boolean
        Dim blnRet As Boolean = False               ' 戻り値
        Dim strSql As String = ""                   ' SQL文
        Dim strYearSearch As String                 ' 年
        Dim strMonthSearch As String                ' 月
        Dim strDaySearch As String                  ' 日
        Dim strDateSearch As String                 ' 日付
        Dim strAreaSearch As String                 ' 支部
        Dim strPreNumberSearch As String            ' 申請番号前桁(期)
        Dim strPostNumberSearch As String           ' 申請番号後桁(申請書番号)
        Dim strKsh As String = NSMDInfo.Ksh         ' 会社区分

        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            'グリッドを初期化
            Call Me.flxApply.Rows.Clear()

            ' 検索条件：対象年月日、支部、申請番号
            If Me.cmbApplyArea.SelectedIndex > 0 Then
                strAreaSearch = Me.cmbApplyArea.SelectedValue
            Else
                strAreaSearch = ""
            End If

            strYearSearch = Me.cmbApplyYear.Text
            strMonthSearch = Me.cmbApplyMonth.Text
            strDaySearch = Me.cmbApplyDay.Text
            strDateSearch = strYearSearch & strMonthSearch & strDaySearch

            strPreNumberSearch = Me.txtApplyNumber1.Text
            strPostNumberSearch = Me.txtApplyNumber2.Text

            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = ""
            strSql = strSql & "SELECT " & vbCrLf
            strSql = strSql & "    const_dtl_local.l_name AS l_local_name," & vbCrLf              ' 0.支部
            strSql = strSql & "    IIF(a_strike.k_cancel = '1','取消','申請')" & vbCrLf
            strSql = strSql & "                           AS apply_cancel," & vbCrLf              ' 1.種別
            strSql = strSql & "    a_strike.c_strike_id AS c_strike_id," & vbCrLf                 ' 2.番号
            strSql = strSql & "    a_strike.d_application AS d_application," & vbCrLf             ' 3.日付
            strSql = strSql & "    const_dtl_classify.l_name AS l_apply_classify_name," & vbCrLf  ' 4.種類
            strSql = strSql & "    a_strike.l_meeting AS l_meeting," & vbCrLf                     ' 5.会議名
            strSql = strSql & "    a_strike.c_replace_strike_id AS c_replace_strike_id," & vbCrLf ' 6.差替申請番号
            strSql = strSql & "    now_name.l_name AS l_up_user_name," & vbCrLf                   ' 7.担当者
            strSql = strSql & "    a_strike.c_ksh AS c_ksh," & vbCrLf                             ' 8.会社区分(非表示)
            strSql = strSql & "    a_strike.c_period_id AS c_period_id," & vbCrLf                  ' 9.期ＩＤ(非表示)
            strSql = strSql & "    a_strike.k_apply_area AS k_apply_area," & vbCrLf                ' 10.申請地区(非表示)
            strSql = strSql & "    a_strike.k_apply_classify AS k_apply_classify," & vbCrLf        ' 11.種類区分(非表示)
            strSql = strSql & "    a_strike.union_info_c_union_meeting AS union_info_c_union_meeting" & vbCrLf ' 12.会議通知ID/中執期間ID(非表示)
            strSql = strSql & "FROM ((((" & vbCrLf
            strSql = strSql & "    apply_strike AS a_strike" & vbCrLf
            strSql = strSql & "    LEFT JOIN period AS period_mst" & vbCrLf
            strSql = strSql & "    ON (a_strike.c_period_id = period_mst.c_period_id" & vbCrLf
            strSql = strSql & "     AND period_mst.c_ksh = '" & strKsh & "'))" & vbCrLf
            strSql = strSql & "    LEFT JOIN constant_dtl AS const_dtl_local " & vbCrLf
            strSql = strSql & "    ON a_strike.k_apply_area = const_dtl_local.c_constant_seq) " & vbCrLf
            strSql = strSql & "    LEFT JOIN constant_dtl AS const_dtl_classify " & vbCrLf
            strSql = strSql & "    ON a_strike.k_apply_classify = const_dtl_classify.c_constant_seq) " & vbCrLf
            strSql = strSql & "    LEFT JOIN staf_attribute_full_time_now_name_view AS now_name " & vbCrLf
            strSql = strSql & "    ON a_strike.c_user_id_up = now_name.user_id)" & vbCrLf
            strSql = strSql & "WHERE" & vbCrLf
            strSql = strSql & "     a_strike.c_ksh      = '" & strKsh & "'" & vbCrLf
            strSql = strSql & " AND a_strike.k_replace = '0'" & vbCrLf
            strSql = strSql & " AND const_dtl_local.c_constant = 'BELONGING'" & vbCrLf
            strSql = strSql & " AND const_dtl_classify.c_constant = 'APPLY_CLASSIFY'" & vbCrLf
            If Not ChkNull(strAreaSearch) Then
                ' 支部は選択時のみ検索条件追加
                strSql = strSql & " AND a_strike.k_apply_area = '" & strAreaSearch & "'" & vbCrLf
            End If
            If (strDateSearch.Length = 4) Then
                ' 年まで入力時は申請日付の年を検索条件追加
                strSql = strSql & " AND FORMAT(a_strike.d_application, 'yyyy') = '" & strDateSearch & "'" & vbCrLf
            ElseIf (strDateSearch.Length = 6) Then
                ' 年月まで入力時は申請日付の年月を検索条件追加
                strSql = strSql & " AND FORMAT(a_strike.d_application, 'yyyyMM') = '" & strDateSearch & "'" & vbCrLf
            Else
                ' 年月日まで入力時は申請日付の年月日を検索条件追加
                strSql = strSql & " AND FORMAT(a_strike.d_application, 'yyyyMMdd') = '" & strDateSearch & "'" & vbCrLf
            End If
            If Not ChkNull(strPreNumberSearch) Then
                ' 申請番号前桁は入力時のみ前方一致で検索条件追加
                strSql = strSql & " AND period_mst.l_omission_name LIKE '" & strPreNumberSearch & "%'" & vbCrLf
            End If
            If Not ChkNull(strPostNumberSearch) Then
                ' 申請番号後桁は入力時のみ前方一致で検索条件追加
                strSql = strSql & " AND a_strike.c_application LIKE '" & strPostNumberSearch & "%'" & vbCrLf
            End If
            strSql = strSql & "ORDER BY " & vbCrLf  'ok
            strSql = strSql & "    a_strike.k_apply_area," & vbCrLf
            strSql = strSql & "    period_mst.l_omission_name DESC," & vbCrLf
            strSql = strSql & "    CLng(a_strike.c_application) DESC" & UtDb.DbOrderOffset & vbCrLf

            ' DB接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' グリッド件数と件数表示の設定
            Me.flxApply.RowCount = intRetCnt
            Me.grpApplyResult.Text = "検索結果 (" + intRetCnt.ToString + "件)"

            ' レコード数分ループ
            For i = 0 To intRetCnt - 1
                ' データ設定
                With Me.flxApply.Rows(i).Cells
                    .Item(0).Value = NVL(tbRet.Rows(i).Item("l_local_name"))            ' 支部
                    .Item(1).Value = NVL(tbRet.Rows(i).Item("apply_cancel"))            ' 種別
                    .Item(2).Value = NVL(tbRet.Rows(i).Item("c_strike_id"))             ' 番号
                    .Item(3).Value = NVL(tbRet.Rows(i).Item("d_application"))           ' 日付
                    .Item(4).Value = NVL(tbRet.Rows(i).Item("l_apply_classify_name"))   ' 種類
                    .Item(5).Value = NVL(tbRet.Rows(i).Item("l_meeting"))               ' 会議名
                    .Item(6).Value = NVL(tbRet.Rows(i).Item("c_replace_strike_id"))     ' 差替申請番号
                    .Item(7).Value = NVL(tbRet.Rows(i).Item("l_up_user_name"))          ' 担当者
                    .Item(8).Value = NVL(tbRet.Rows(i).Item("c_ksh"))                   ' 会社区分(詳細表示用非表示項目)
                    .Item(9).Value = NVL(tbRet.Rows(i).Item("c_period_id"))             ' 期ＩＤ(詳細表示用非表示項目)
                    .Item(10).Value = NVL(tbRet.Rows(i).Item("k_apply_area"))           ' 申請地区(詳細表示用非表示項目)
                    .Item(11).Value = NVL(tbRet.Rows(i).Item("k_apply_classify"))       ' 種類区分(詳細表示用非表示項目)
                    .Item(12).Value = NVL(tbRet.Rows(i).Item("union_info_c_union_meeting")) ' 会議通知ID/中執期間ID(詳細表示用非表示項目)
                End With
            Next

            ' 検索が正常の場合はVisibleの設定
            Me.grpApplyResult.Visible = True

            ' 検索結果0件の場合は取消・詳細ボタンを無効化
            If intRetCnt = 0 Then
                Me.btnCancel.Enabled = False
                Me.btnDetail.Enabled = False
            Else
                Me.btnDetail.Enabled = True
                If IsGetEntryRight Then
                    Me.btnCancel.Enabled = True
                Else
                    Me.btnCancel.Enabled = False
                End If
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
            'カーソルを戻す
            Me.Cursor = Cursors.Default
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        Return blnRet
    End Function
#End Region

#Region "関数：時間内組合活動者情報取得（レポート用）"
    '***************************************************************************************************
    '   ＩＤ　：GetApplyMemberInf
    '   名称　：時間内組合活動者情報取得（レポート用）
    '   概要  ：
    '   作成日：2012/01/20 新規作成
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) Fujisaku  新規作成
    '***************************************************************************************************
    Private Function GetApplyMemberInf() As DS0403P3

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")

        Dim dtsReturn As DS0403P3 = New DS0403P3()

        Try
            If Me.flxActivityDate.Rows.Count > 0 Then
                Dim drDetail As DS0403P3.dtDetailRow
                For Each row As DataGridViewRow In flxActivityDate.Rows
                    drDetail = dtsReturn.dtDetail.NewRow
                    drDetail.BeginEdit()

                    drDetail.local = NVL(row.Cells.Item("所属").Value).ToString                         ' 所属
                    drDetail.c_staf_id = NVL(row.Cells.Item("社員番号").Value).ToString                 ' 社番
                    drDetail.l_name = NVL(row.Cells.Item("氏名").Value).ToString                        ' 氏名
                    drDetail.l_omission_name = ""           ' 期略称(不使用)
                    drDetail.c_application = NVL(row.Cells.Item("申請番号").Value).ToString             ' 申請番号
                    drDetail.d_application = NVL(row.Cells.Item("申請日付").Value).ToString             ' 申請日付
                    drDetail.staf_id_ins = ""               ' 登録者ID(不使用)
                    drDetail.name_ins = ""                  ' 登録者名称(不使用)
                    drDetail.staf_id_up = NVL(row.Cells.Item("更新者個人認証ＩＤ").Value).ToString      ' 担当者(ID)
                    drDetail.name_up = NVL(row.Cells.Item("担当者").Value).ToString                     ' 担当者(氏名)
                    drDetail.apply_classify = NVL(row.Cells.Item("種類").Value).ToString                ' 種類
                    drDetail.l_meeting = NVL(row.Cells.Item("会議名").Value).ToString                   ' 会議名
                    drDetail.cancel_l_omission_name = ""    ' 取消期略称(不使用)
                    drDetail.cancel_application = NVL(row.Cells.Item("取消番号").Value).ToString        ' 取消番号
                    drDetail.cancel_application_date = NVL(row.Cells.Item("取消日付").Value).ToString   ' 取消日付
                    drDetail.cancel_staf_id_ins = ""        ' 取消登録者ID(不使用)
                    drDetail.cancel_name_ins = ""           ' 取消登録者名称(不使用)
                    drDetail.cancel_staf_id_up = NVL(row.Cells.Item("取消更新者個人認証ＩＤ").Value).ToString ' 取消担当者(ID)
                    drDetail.cancel_name_up = NVL(row.Cells.Item("取消更新者氏名").Value).ToString      ' 取消担当者(氏名)
                    drDetail.l_biko = ""                                                                ' 備考(対応項目なし)

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

#Region "関数：活動日付検索の検索結果クリア"
    '***************************************************************************************************
    '   ＩＤ　：ClearActivitySearch
    '   名称　：活動日付検索の検索結果クリア
    '   概要  ：
    '   引数　：なし
    '   作成日：2012/01/20(金) 
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) Fujisaku 新規作成
    '***************************************************************************************************
    Public Function ClearActivitySearch() As Boolean
        Try
            Me.flxActivityDate.Rows.Clear()
            Me.grpActResult.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Function
#End Region

#Region "関数：申請番号検索の検索結果クリア"
    '***************************************************************************************************
    '   ＩＤ　：ClearApplySearch
    '   名称　：申請番号検索の検索結果クリア
    '   概要  ：
    '   引数　：なし
    '   作成日：2012/01/20(金) 
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) Fujisaku 新規作成
    '***************************************************************************************************
    Public Function ClearApplySearch() As Boolean
        Try
            Me.flxApply.Rows.Clear()
            Me.grpApplyResult.Visible = False
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Function
#End Region

#Region "関数：申請番号検索条件設定（活動日付の検索結果ダブルクリック）"
    '***************************************************************************************************
    '   ＩＤ　：SetNoticeDateSearch()
    '   名称　：申請番号検索条件設定
    '   概要  ：活動日付検索結果ダブルクリック時に申請番号検索画面の条件を設定する
    '   作成日：2012/01/20(金)
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/20(金) Fujisaku 新規作成
    '***************************************************************************************************
    Private Function SetApplySearchCondition() As Boolean

        Dim blnRet As Boolean = False
        Dim selectRow As DataGridViewRow = Nothing
        Dim strApplyNumber As String
        Dim strApplyDate As String
        Dim dtApplyDate As Date

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            selectRow = Me.flxActivityDate.SelectedRows.Item(0)

            '支部の値はクリアしておく
            Me.cmbApplyArea.Text = ""

            '申請日も同様に検索条件に値を設定する
            strApplyDate = selectRow.Cells.Item("申請日付").Value
            dtApplyDate = Date.Parse(strApplyDate)
            Me.cmbApplyYear.Text = dtApplyDate.Year.ToString
            Me.cmbApplyMonth.Text = dtApplyDate.Month.ToString.PadLeft(2, "0")
            Me.cmbApplyDay.Text = dtApplyDate.Day.ToString.PadLeft(2, "0")

            strApplyNumber = selectRow.Cells.Item("申請番号").Value
            ' - で分割して　申請番号検索条件に値を設定する
            Dim intSepaIndex As Integer = strApplyNumber.IndexOf(SEPALATE_CHAR, 0)
            Me.txtApplyNumber1.Text = strApplyNumber.Substring(0, intSepaIndex)
            Me.txtApplyNumber2.Text = strApplyNumber.Substring(intSepaIndex + 1, strApplyNumber.Length - 1 - intSepaIndex)

            ' 処理結果に正常を格納
            blnRet = True
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        Return blnRet
    End Function
#End Region

#Region "関数：時間内組合活動 - 取消画面遷移処理"
    '***************************************************************************************************
    '   ＩＤ　：ShowUC040306
    '   名称　：時間内組合活動 - 取消画面遷移処理
    '   概要  ：時間内組合活動 - 取消画面遷移処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/21(土) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/21(土) m.suzuki  新規作成
    '         ：2012/02/03(金) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    ''' <summary>時間内組合活動 - 取消画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ShowUC040306() As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim pnl As Panel                                                                ' パネルオブジェクト
        Dim clsUC040306 As UC040306 = Nothing                                           ' 時間内組合活動 - 取消画面クラス
        Dim strSybt As String = ""                                                      ' 種別
        Dim strApplyNo As String = ""                                                   ' 申請書番号
        Dim strKsh As String = ""                                                       ' 会社コード
        Dim strPeriodId As String = ""                                                  ' 期ID
        Dim strApplyArea As String = ""                                                 ' 申請地区

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)  ' ログ出力（処理開始）
        Try
            '---------------------------------------------------------------------------
            '   グリッド選択チェック
            '---------------------------------------------------------------------------
            If Me.flxApply.SelectedRows.Count < 0 Then                                  ' 選択されているかチェック
                Call CLMsg.Show("GW0001", "データ")                                     ' 未選択の場合、エラーメッセージ表示
                Return blnRet
            Else

                ' 各データ取得
                strSybt = Me.flxApply.CurrentRow.Cells.Item(1).Value.ToString()
                strApplyNo = Me.flxApply.CurrentRow.Cells.Item(2).Value.ToString()      ' 申請番号
                strKsh = Me.flxApply.CurrentRow.Cells.Item(8).Value.ToString()          ' 会社コード
                strPeriodId = Me.flxApply.CurrentRow.Cells.Item(9).Value.ToString()     ' 期ID
                strApplyArea = Me.flxApply.CurrentRow.Cells.Item(10).Value.ToString()   ' 申請地区


                ' 取消可否チェック
                If strSybt = "取消" Then
                    ' 取消申請の取消は不可エラー
                    Call CLMsg.Show("GE0041")
                    Return blnRet
                End If

                ' 全ての組合員取消済みチェック
                If Not Me.ChkAllStafCancel(strApplyNo, strApplyArea) Then
                    ' 全ての日付が取消済の場合は不可エラー
                    Call CLMsg.Show("GE0087")
                    Return blnRet
                End If

                ' 画面遷移準備
                Me.Visible = False
                pnl = ParentForm.Controls(MAIN_PANEL_ID)
                clsUC040306 = pnl.Controls(SCREEN_ID_UC040306)
                clsUC040306 = New UC040306                                              ' 時間内組合活動 - 取消画面
                ' 画面間パラメータ情報設定
                clsUC040306.strStrikeId = strApplyNo                                    ' 申請番号
                clsUC040306.strKsh = strKsh                                             ' 会社コード
                clsUC040306.strPeriodId = strPeriodId                                   ' 期ID
                clsUC040306.strApplyArea = strApplyArea                                 ' 申請地区
                Call pnl.Controls.Add(clsUC040306)                                      ' パネルに時間内組合活動 - 取消画面を設定
            End If
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)    ' ログ出力（処理終了）
        Return blnRet                                                                   ' 戻り値設定

    End Function
#End Region

#Region "関数：時間内組合活動 - 詳細画面遷移処理"
    '***************************************************************************************************
    '   ＩＤ　：ShowUC040302
    '   名称　：時間内組合活動 - 詳細画面遷移処理
    '   概要  ：時間内組合活動 - 詳細画面遷移処理を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/22(日)
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/22(日) Fujisaku  新規作成
    '         ：2012/02/03(金) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    Private Function ShowUC040302() As Boolean
        Dim blnRet As Boolean = False                                                   ' 処理結果
        Dim pnl As Panel                                                                ' パネルオブジェクト
        Dim clsUC040302 As UC040302 = Nothing                                           ' 時間内組合活動 - 取消画面クラス
        'Dim strPeriodId As String = ""                                                 ' 期ID
        Dim strApplyNo As String = ""                                                   ' 申請書番号
        Dim strApplyArea As String = ""                                                 ' 申請地区
        Dim dbAccess As New CLAccessMdb                                                 ' DBアクセス
        Dim intCounter As Integer
        ' 処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' 各データ取得
            'strPeriodId = Me.flxApply.CurrentRow.Cells.Item(9).Value.ToString()     ' 期ID
            strApplyNo = Me.flxApply.CurrentRow.Cells.Item(2).Value.ToString()      ' 申請番号
            strApplyArea = Me.flxApply.CurrentRow.Cells.Item(10).Value.ToString()   ' 申請地区

            ' 画面遷移準備
            Me.Visible = False

            pnl = ParentForm.Controls(MAIN_PANEL_ID)
            clsUC040302 = pnl.Controls(SCREEN_ID_UC040302)
            clsUC040302 = New UC040302                                              ' 時間内組合活動 - 詳細画面

            '支部
            Dim txtApplyArea As TextBox
            txtApplyArea = clsUC040302.txtApplyArea
            txtApplyArea.Text = Me.flxApply.CurrentRow.Cells.Item(0).Value.ToString()
            txtApplyArea.ReadOnly = True
            txtApplyArea.BackColor = Color.LightYellow
            clsUC040302.lblApplyArea.Text = strApplyArea
            '種類
            Dim strClassify As String = Me.flxApply.CurrentRow.Cells.Item(11).Value.ToString()
            clsUC040302.lblApplyClassify.Text = strClassify
            Dim txtApplyClassify As TextBox
            txtApplyClassify = clsUC040302.txtApplyClassify
            txtApplyClassify.ReadOnly = True
            txtApplyClassify.BackColor = Color.LightYellow
            '申請番号
            clsUC040302.txtApplyNumber.Text = strApplyNo
            clsUC040302.txtApplyNumber.ReadOnly = True
            clsUC040302.txtApplyNumber.BackColor = Color.LightYellow
            clsUC040302.lblStrikeID.Text = strApplyNo
            '申請日
            clsUC040302.txtApplyDate.Text = Me.flxApply.CurrentRow.Cells.Item(3).Value.ToString()
            clsUC040302.txtApplyDate.ReadOnly = True
            clsUC040302.txtApplyDate.BackColor = Color.LightYellow

            '会議名選択肢設定
            Dim cmbApplyMeetinglist As ComboBox = clsUC040302.cmbApplyMeetinglist
            dbAccess.Connect()
            ' 会議名コンボボックス
            Call CreateCboConstantDtlDate(dbAccess, cmbApplyMeetinglist, CONSTANT_ID_APPLY_MEETINGLIST, Now(), True)
            cmbApplyMeetinglist.SelectedIndex = -1
            cmbApplyMeetinglist.Enabled = False
            cmbApplyMeetinglist.BackColor = Color.White
            '申請者名
            clsUC040302.txtStandName.ReadOnly = True
            clsUC040302.txtStandName.BackColor = Color.White

            Dim dataDt As DataTable = getlMeetingByStrikeID(dbAccess, strApplyNo, strApplyArea)

            If dataDt.Rows.Count = 1 Then
                Dim dataDr As DataRow = dataDt.Rows(0)
                '種類
                txtApplyClassify.Text = dataDr("l_name")
                '会議名
                cmbApplyMeetinglist.DropDownStyle = 1
                cmbApplyMeetinglist.Text = dataDr("l_meeting")
                'Call Utilities.SetCanEditToControl(True, cmbApplyMeetinglist)

                '略称
                clsUC040302.lblOmission.Text = dataDr("l_omission_name")
                '申請者
                clsUC040302.txtStandName.Text = dataDr("l_stand_name")
                ' S_UP
                clsUC040302.lblSUp.Text = GetRevision(CInt(dataDr("s_up")))
                '差替え
                If Not IsDBNull(dataDr("c_replace_strike_id")) Then
                    If dataDr("c_replace_strike_id") <> "" Then
                        clsUC040302.cmbReplaceNumber.Items.Add(dataDr("c_replace_strike_id"))
                        clsUC040302.cmbReplaceNumber.Text = dataDr("c_replace_strike_id")
                        clsUC040302.chkReplace.Checked = True
                    End If
                End If

                ' 件数表示、取り消し申請のときは表示しない
                If Me.flxApply.CurrentRow.Cells.Item(1).Value.ToString() = "取消" Then
                    clsUC040302.grpFrameCount.Visible = False
                    If strClassify = "01" Or strClassify = "02" Then
                        clsUC040302.txtMeetingNo.Visible = False
                        clsUC040302.lblMeetingNo.Visible = False
                        clsUC040302.txtComExe.Visible = True
                        clsUC040302.lblKikan.Visible = True
                        clsUC040302.txtComExe.Text = dataDr("d_from") + " - " + dataDr("d_to")
                        clsUC040302.txtComExe.ReadOnly = True
                        clsUC040302.txtComExe.BackColor = Color.LightYellow
                        clsUC040302.lblTermID.Text = dataDr("union_info_c_union_meeting")
                    ElseIf strClassify = "03" Then
                        clsUC040302.txtMeetingNo.Visible = True
                        clsUC040302.lblMeetingNo.Visible = True
                        clsUC040302.txtComExe.Visible = False
                        clsUC040302.lblKikan.Visible = False
                        clsUC040302.txtMeetingNo.Text = IIf(IsDBNull(dataDr("union_info_c_union_meeting")), "", dataDr("union_info_c_union_meeting"))
                        clsUC040302.txtMeetingNo.ReadOnly = True
                        clsUC040302.txtMeetingNo.BackColor = Color.LightYellow
                    Else
                        clsUC040302.txtMeetingNo.Visible = False
                        clsUC040302.lblMeetingNo.Visible = False
                        clsUC040302.txtComExe.Visible = False
                        clsUC040302.lblKikan.Visible = False
                    End If
                Else
                    If strClassify = "01" Or strClassify = "02" Then
                        clsUC040302.txtMeetingNo.Visible = False
                        clsUC040302.lblMeetingNo.Visible = False
                        clsUC040302.txtComExe.Visible = True
                        clsUC040302.lblKikan.Visible = True
                        clsUC040302.txtComExe.Text = dataDr("d_from") + " - " + dataDr("d_to")
                        clsUC040302.txtComExe.ReadOnly = True
                        clsUC040302.txtComExe.BackColor = Color.LightYellow
                        clsUC040302.lblTermID.Text = dataDr("union_info_c_union_meeting")
                        clsUC040302.grpFrameCount.Visible = True
                        setCounterByClassifyID(clsUC040302.Controls("grpFrameCount"), strClassify, IIf(IsDBNull(dataDr("d_from")), "20000101", dataDr("d_from")))
                    ElseIf strClassify = "03" Then
                        clsUC040302.txtMeetingNo.Visible = True
                        clsUC040302.lblMeetingNo.Visible = True
                        clsUC040302.txtComExe.Visible = False
                        clsUC040302.lblKikan.Visible = False
                        clsUC040302.txtMeetingNo.Text = IIf(IsDBNull(dataDr("union_info_c_union_meeting")), "", dataDr("union_info_c_union_meeting"))
                        clsUC040302.txtMeetingNo.ReadOnly = True
                        clsUC040302.txtMeetingNo.BackColor = Color.LightYellow
                        clsUC040302.grpFrameCount.Visible = True
                        setCounterByClassifyID(clsUC040302.Controls("grpFrameCount"), strClassify, getDateFromUnionInformation(dataDr("union_info_c_union_meeting")))
                    Else
                        clsUC040302.txtMeetingNo.Visible = False
                        clsUC040302.lblMeetingNo.Visible = False
                        clsUC040302.txtComExe.Visible = False
                        clsUC040302.lblKikan.Visible = False
                        clsUC040302.grpFrameCount.Visible = False
                    End If
                End If
            Else
                Return False
            End If

            '組合員取得
            Dim flxNameAndDate = clsUC040302.flxNameAndDate
            Dim curDate As String = Format(Now, DATE_YYYYMMDD_FORMAT)
            Dim flgModify As Boolean
            Dim cntDateRows As Integer
            Dim cntDateMax As Integer = 5   ' 日付の最大表示件数(初期値5)
            flgModify = True
            strDelFlg = False

            dataDt = getUserByStrikeID(dbAccess, strApplyNo, strApplyArea)
            If dataDt.Rows.Count > 0 Then
                flxNameAndDate.Rows.Count = dataDt.Rows.Count + 1
                For iCounter As Integer = 0 To dataDt.Rows.Count - 1
                    For isubCounter As Integer = 0 To 19
                        If Not IsDBNull(dataDt.Rows(iCounter)(isubCounter)) Then
                            flxNameAndDate.SetData(iCounter + 1, isubCounter, dataDt.Rows(iCounter)(isubCounter))
                            If isubCounter > 4 Then
                                intCounter = intCounter + 1
                                If CStr(dataDt.Rows(iCounter)(isubCounter)) <= curDate Then
                                    ' 1件でも過去日付があれば変更不可
                                    flgModify = False
                                End If
                            End If
                            '組合員単位の日付件数保存
                            cntDateRows = isubCounter - 4
                        End If
                    Next
                    '日付件数の最大値保存
                    If cntDateRows > cntDateMax Then
                        cntDateMax = cntDateRows
                    End If
                Next
            End If
            clsUC040302.gridColsVisibleSet(cntDateMax)

            If Me.flxApply.CurrentRow.Cells.Item(1).Value.ToString() = "取消" Then
                flgModify = False
                clsUC040302.lblISDelete.Text = "1"
                clsUC040302.lblKindTitle.Text = "【取消】"
            Else
                clsUC040302.lblISDelete.Text = "0"
                clsUC040302.lblKindTitle.Text = "【申請】"
            End If
            clsUC040302.btnModify.Visible = flgModify
            '取り消しが1件でもあれば、内容変更ボタンを表示しない
            If strDelFlg Then
                clsUC040302.btnModify.Visible = False
            End If
            '編集権限がなければ、内容変更ボタンを表示しない
            If Not IsGetEntryRight Then
                clsUC040302.btnModify.Visible = False
            End If
            'プリント権限がなければ、印刷ボタンを非表示
            If IsPrintRight Then
                clsUC040302.btnPrinting.Visible = True
            Else
                clsUC040302.btnPrinting.Visible = False
            End If
            '差替え
            clsUC040302.chkReplace.Enabled = False
            clsUC040302.cmbReplaceNumber.Enabled = False
            clsUC040302.btnReplaceNumber.Enabled = False

            flxNameAndDate.AllowEditing = False
            clsUC040302.btnConfirm.Visible = False
            clsUC040302.btnAdd.Visible = False
            clsUC040302.btnRemove.Visible = False
            clsUC040302.btnCancel.Text = "戻る"
            clsUC040302.intAlreadyUser = intCounter
            Call pnl.Controls.Add(clsUC040302)                                      ' パネルに時間内組合活動 - 詳細画面を設定

            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            log.Fatal(ex.Message) ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
            dbAccess = Nothing
        End Try

        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        Return blnRet
    End Function
#End Region

#Region "関数：全組合員取消済みチェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkAllStafCancel
    '   名称　：全組合員取消済みチェック
    '   概要  ：
    '   引数　：strStrikeId　申請番号(xx-yyy)
    '         ：strApplyArea　申請地区
    '   戻り値：True = 正常(1人以上、取消可), False = 異常(0人、取消不可)
    '   作成日：2012/01/22(日) 
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/22(日) Fujisaku  新規作成
    '         ：2012/02/03(金) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    Private Function ChkAllStafCancel(ByVal strStrikeId As String, ByVal strApplyArea As String) As Boolean
        Dim blnRet As Boolean = False
        Dim strSql As String = ""                   ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数

        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            ' SQL作成
            strSql = ""
            strSql = strSql & "SELECT" & vbCrLf
            strSql = strSql & "    c_staf_id" & vbCrLf
            strSql = strSql & "FROM" & vbCrLf
            strSql = strSql & "    apply_strike_member_date" & vbCrLf
            strSql = strSql & "WHERE" & vbCrLf
            strSql = strSql & "      c_strike_id = '" & strStrikeId & "'" & vbCrLf
            strSql = strSql & " AND  k_apply_area = '" & strApplyArea & "'" & vbCrLf
            strSql = strSql & " AND (c_cancel_strike_id is null " & vbCrLf
            strSql = strSql & "   OR c_cancel_strike_id = '')" & vbCrLf

            ' DB接続
            clsDb.Connect()

            ' SQL実行
            tbRet = clsDb.ExecuteSql(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' 件数0件で無い場合は正常(申請に対する取消可能)
            If intRetCnt <> 0 Then
                ' 処理結果に正常を格納
                blnRet = True
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try

        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

        Return blnRet
    End Function
#End Region

#Region "関数：getlMeetingByStrikeID"
    Private Function getlMeetingByStrikeID(ByVal dbAccess As CLAccessMdb, ByVal strStrikeID As String, ByVal strApplyArea As String)

        Dim dtRet As New DataTable
        Try
            Dim sql As String
            sql = "select * from ("
            sql = sql & "select l_stand_name,l_meeting,union_info_c_union_meeting,c_replace_strike_id,d_application,l_name,l_omission_name,s_up "
            sql = sql & "from (select * from apply_strike where c_strike_id='" + strStrikeID + "' and k_apply_area = '" + strApplyArea + "') as apply_strike "
            sql = sql & "left join (select l_name,c_constant,c_constant_seq,l_omission_name from constant_dtl where c_constant='APPLY_CLASSIFY') as constant_dtl on apply_strike.k_apply_classify=constant_dtl.c_constant_seq)as apply_strike "
            sql = sql & "left join (select d_from,d_to,c_apply_strike_term_id from apply_strike_executive_term) as apply_strike_executive_term on CStr(apply_strike_executive_term.c_apply_strike_term_id)=apply_strike.union_info_c_union_meeting"
            dtRet = dbAccess.ExecuteSql(sql)

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040302, SCREEN_NAME_UC040302, "getlMeetingByStrikeID")
        End Try
        Return dtRet
    End Function
#End Region

#Region "関数：getUserByStrikeID"
    Private Function getUserByStrikeID(ByVal dbAccess As CLAccessMdb, ByVal strStrikeID As String, ByVal strApplyArea As String)
        Dim dt As New DataTable
        Dim retDt As New DataTable
        Try
            If strStrikeID <> "" Then

                Dim sql As String
                sql = "select c_staf_id,d_strike,k_cancel from apply_strike_member_date "
                sql = sql & "where c_strike_id='" + strStrikeID + "' and k_apply_area ='" + strApplyArea + "'"
                sql = sql & "order by CLng(c_staf_id), d_strike"    'chk
                dt = dbAccess.ExecuteSql(sql)
                Dim intCurNum As Integer

                If dt.Rows.Count > 0 Then
                    retDt.Columns.Add("c_staf_id")
                    retDt.Columns.Add("l_name")
                    retDt.Columns.Add("k_belonging")
                    retDt.Columns.Add("k_model")
                    retDt.Columns.Add("k_qualification")
                    retDt.Columns.Add("d_strike1")
                    retDt.Columns.Add("d_strike2")
                    retDt.Columns.Add("d_strike3")
                    retDt.Columns.Add("d_strike4")
                    retDt.Columns.Add("d_strike5")
                    retDt.Columns.Add("d_strike6")
                    retDt.Columns.Add("d_strike7")
                    retDt.Columns.Add("d_strike8")
                    retDt.Columns.Add("d_strike9")
                    retDt.Columns.Add("d_strike10")
                    retDt.Columns.Add("d_strike11")
                    retDt.Columns.Add("d_strike12")
                    retDt.Columns.Add("d_strike13")
                    retDt.Columns.Add("d_strike14")
                    retDt.Columns.Add("d_strike15")
                    Dim subStafID As String
                    Dim subStafIDCur As String
                    subStafIDCur = dt.Rows(0)(0)
                    Dim newRow As DataRow = retDt.NewRow
                    newRow("c_staf_id") = subStafIDCur
                    newRow("d_strike1") = Format(CDate(dt.Rows(0)(1)), DATE_YYYYMMDD_FORMAT)
                    intCurNum = 1
                    If Not IsDBNull(dt.Rows(0)(2)) Then
                        If CStr(dt.Rows(0)(2)) = "1" Then
                            strDelFlg = True
                        End If
                    End If

                    Dim subDt As DataTable = GetUnionMemberData(dbAccess, subStafIDCur)
                    If subDt.Rows.Count > 0 Then
                        '氏名
                        newRow("l_name") = subDt.Rows(0).Item(1)
                        '資格
                        newRow("k_qualification") = subDt.Rows(0).Item(4)
                        '機種
                        newRow("k_model") = subDt.Rows(0).Item(7)
                        '所属
                        newRow("k_belonging") = subDt.Rows(0).Item(6)
                    End If
                    'retDt.Rows.Add(newRow)

                    For iCounter As Integer = 1 To dt.Rows.Count - 1
                        subStafID = dt.Rows(iCounter)(0)
                        If subStafID = subStafIDCur Then
                            If Not IsDBNull(dt.Rows(iCounter)(2)) Then
                                If CStr(dt.Rows(iCounter)(2)) = "1" Then
                                    strDelFlg = True
                                End If
                            End If

                            intCurNum = intCurNum + 1
                            newRow("d_strike" + CStr(intCurNum)) = Format(CDate(dt.Rows(iCounter)(1)), DATE_YYYYMMDD_FORMAT)
                        Else
                            retDt.Rows.Add(newRow)
                            newRow = retDt.NewRow
                            newRow("c_staf_id") = subStafID
                            If Not IsDBNull(dt.Rows(iCounter)(2)) Then
                                If CStr(dt.Rows(iCounter)(2)) = "1" Then
                                    strDelFlg = True
                                End If
                            End If

                            newRow("d_strike1") = Format(CDate(dt.Rows(iCounter)(1)), DATE_YYYYMMDD_FORMAT)
                            intCurNum = 1

                            subDt = GetUnionMemberData(dbAccess, subStafID)
                            If subDt.Rows.Count > 0 Then
                                '氏名
                                newRow("l_name") = subDt.Rows(0).Item(1)
                                '資格
                                newRow("k_qualification") = subDt.Rows(0).Item(4)
                                '機種
                                newRow("k_model") = subDt.Rows(0).Item(7)
                                '所属
                                newRow("k_belonging") = subDt.Rows(0).Item(6)
                            End If
                            subStafIDCur = subStafID

                        End If
                    Next
                    retDt.Rows.Add(newRow)
                End If
            End If

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return retDt
    End Function
#End Region

#Region "関数：GetReplaceStrikeId"
    'Public Function GetReplaceStrikeId(ByVal strArea As String, ByVal strClassify As String) As DataTable
    '    Dim dbAccess As New CLAccessMdb
    '    Dim sql As String
    '    Dim table2 As New DataTable
    '    Try
    '        sql = "select A_APPLY.c_application, A_APPLY.c_strike_id  from apply_strike A_APPLY, "
    '        sql = sql + "(select distinct(APPLY.c_strike_id) as c_strike_id, APPLY.k_apply_area as k_apply_area "
    '        sql = sql + " from apply_strike APPLY, apply_strike_member_date APP_DATE "
    '        sql = sql + " where APPLY.c_ksh         = '" + MDLoginInfo.Ksh + "' "
    '        sql = sql + "  and APPLY.k_apply_area  = '" + strArea + "' "
    '        sql = sql + "  and APPLY.k_cancel      = '0'    and APPLY.k_replace     = '0' "
    '        sql = sql + "  and Format(APP_DATE.d_strike, 'yyyyMMdd')  >= '" + MDLoginInfo.PeriodFrom + "' "
    '        sql = sql + "  and Format(APP_DATE.d_strike, 'yyyyMMdd')  <= '" + MDLoginInfo.PeriodTo + "' "
    '        sql = sql + "  and (   APP_DATE.c_cancel_strike_id is null OR APP_DATE.c_cancel_strike_id = '')  "
    '        sql = sql + "  and APPLY.k_apply_area = APP_DATE.k_apply_area "
    '        sql = sql + "  and APPLY.c_strike_id = APP_DATE.c_strike_id ) B_APPLY "
    '        sql = sql + "where A_APPLY.c_strike_id = B_APPLY.c_strike_id "
    '        sql = sql + " and A_APPLY.k_apply_area = B_APPLY.k_apply_area "
    '        sql = sql + " and A_APPLY.k_apply_area = '" + strArea + "' "
    '        sql = sql + " and A_APPLY.k_apply_classify = '" + strClassify + "' "
    '        sql = sql + " order by A_APPLY.c_application"

    '        dbAccess.Connect()
    '        table2 = dbAccess.ExecuteSql(sql)

    '    Catch ex As Exception
    '        log.Fatal(ex.Message)
    '        Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
    '    Finally
    '        dbAccess.Disconnect()
    '        dbAccess = Nothing
    '    End Try
    '    Return table2
    'End Function
#End Region

#Region "関数：getDateFromUnionInformation"
    Private Function getDateFromUnionInformation(ByVal strUnionInfoID As String) As Object
        Dim dbAccess As New CLAccessMdb
        Try
            Dim sql As String
            Dim dt As DataTable
            sql = "select d_meeting_1 from union_information where c_ksh='" + MDLoginInfo.Ksh + "' and c_union_meeting='" + strUnionInfoID + "'"
            dbAccess.Connect()
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)(0)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
            dbAccess = Nothing
        End Try
        Return ""
    End Function
#End Region

#End Region

#Region "エンター入力対応"

#Region "エンター入力：年コンボボックス(活動日付検索)"
    Private Sub cmbActivityYear_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActivityYear.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchActivity()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：月コンボボックス(活動日付検索)"
    Private Sub cmbActivityMonth_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActivityMonth.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchActivity()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：日コンボボックス(活動日付検索)"
    Private Sub cmbActivityDay_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbActivityDay.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchActivity()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：会社所属コンボボックス(活動日付検索)"
    Private Sub cmbAreaLocal_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbAreaLocal.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchActivity()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：支部コンボボックス(申請番号検索)"
    Private Sub cmbApplyArea_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbApplyArea.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchApply()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：年コンボボックス(申請番号検索)"
    Private Sub cmbApplyYear_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbApplyYear.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchApply()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：月コンボボックス(申請番号検索)"
    Private Sub cmbApplyMonth_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbApplyMonth.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchApply()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：日コンボボックス(申請番号検索)"
    Private Sub cmbApplyDay_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbApplyDay.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchApply()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：申請番号前桁コンボボックス(申請番号検索)"
    Private Sub txtApplyNumber1_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtApplyNumber1.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchApply()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "エンター入力：申請番号後桁コンボボックス(申請番号検索)"
    Private Sub txtApplyNumber2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtApplyNumber2.KeyPress
        Try
            If e.KeyChar = Chr(Keys.Enter) Then
                Call GetSearchApply()
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#End Region

End Class
