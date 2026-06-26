#Region "UC020301"
'===========================================================================================================
'   クラスＩＤ　　：UC020301
'   クラス名称　　：会議通知
'   備考  　　　　：
'===========================================================================================================

Imports C1.Win.C1FlexGrid
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst

Public Class UC020301

#Region "変数・定数"

    '会議通知種別
    Private Const KIND_KAISAI As Byte = 1                                               ' 会議通知 - 新規登録
    Private Const KIND_HENKO As Byte = 2                                                ' 会議通知 - 変更
    Private Const KIND_TYUSHI As Byte = 3                                               ' 会議通知 - 中止
    Private Const KIND_GOUDOU As Byte = 4                                               ' 会議通知 - 合同登録
    Private Const KIND_KAIGI As Byte = 5                                                ' 会議通知 - 会議詳細
    Private Const KIND_SHOSAI As Byte = 6                                               ' 会議通知 - 会議詳細

    '合同区分定数
    Private Const UNION_OFF As String = "0"
    Private Const UNION_ON As String = "1"
    '開催種類定数
    Private Const INFORMATION_TYPE_OPEN = "0"   '開催
    Private Const INFORMATION_TYPE_CHANGE = "1" '変更
    Private Const INFORMATION_TYPE_STOP = "2"   '中止
    '合同
    Private Const UNION_ON_STRING As String = "合同"
    '開催種類
    Private Const INFORMATION_TYPE_OPEN_STRING As String = "開催"
    Private Const INFORMATION_TYPE_CHANGE_STRING As String = "変更"
    Private Const INFORMATION_TYPE_STOP_STRING As String = "中止"
    '支部
    Private Const SHIBU_TOKYO As String = "東京"
    Private Const SHIBU_OOSAKA As String = "大阪"
    Private Const SHIBU_OTHER As String = "その他"

    ' FlexGrid
    Private Const FLEXGRID_ROWS As Byte = 1                                             ' 縦総数
    Private Const FLEXGRID_COLS As Byte = 9                                             ' 横総数
    Private Const FLEXGRID_ROWS_FIXED As Byte = 1                                       ' 固定縦数
    Private Const FLEXGRID_COLS_FIXED As Byte = 0                                       ' 固定横数

    Private _miList As List(Of DataRow) = Nothing
    Private _miTemporaryList As List(Of DataRow) = Nothing
    Public _intReturnBtnFlg As Integer = -1

    '支部コード取得用
    Private _dicBranch As Dictionary(Of String, String) = Nothing
    '参照権限
    Private _strGrantReference As String = String.Empty
    '登録権限
    Private _strGrantInsert As String = String.Empty
    '印刷権限
    Private _strGrantPrint As String = String.Empty
    'ファイル出力権限
    Private _strGrantFileOutput As String = String.Empty
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
#End Region

#Region "開催登録ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnKaisai_Click
    '   名称　：開催登録ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************

    '開催登録ボタン
    Private Sub btnKaisai_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKaisai.Click

        Try
            Dim pn As Panel
            Dim uc As Control

            'Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020302)

            ' 画面遷移処理（会議通知 - 新規登録）
            If TransitionScreen(KIND_KAISAI) = False Then
                Exit Sub
            End If

            'If uc Is Nothing Then
            'uc = New UC020302 '会議通知
            'Call pn.Controls.Add(uc)
            'Else
            'uc.Visible = True
            'End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnKaisai_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "変更ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnHenko_Click
    '   名称　：変更ボタンボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************

    '変更ボタン
    Private Sub btnHenko_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHenko.Click

        Try
            Dim pn As Panel

            'Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)

            ' 画面遷移処理（会議通知 - 変更）
            If TransitionScreen(KIND_HENKO) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnHenko_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "中止ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnTyushi_Click
    '   名称　：中止ボタンボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    '中止ボタン
    Private Sub btnTyushi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTyushi.Click
        Try
            Dim pn As Panel

            'Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)

            ' 画面遷移処理（会議通知 - 中止）
            If TransitionScreen(KIND_TYUSHI) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnTyushi_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "合同登録ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnGoudou_Click
    '   名称　：合同登録ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************

    '合同登録
    Private Sub btnGoudou_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGoudou.Click

        Try
            Dim pn As Panel
            Dim uc As Control

            'Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC020302)

            ' 画面遷移処理（会議通知 - 合同登録）
            If TransitionScreen(KIND_GOUDOU) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnGoudou_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "会議詳細ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnKaigi_Click
    '   名称　：会議詳細ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    Private Sub btnKaigi_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnKaigi.Click

        Try
            Dim pn As Panel
            Dim uc As Control

            'Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC020302)

            ' 画面遷移処理（会議通知 - 会議詳細）
            If TransitionScreen(KIND_KAIGI) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnKaigi_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "詳細ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnShosai_Click
    '   名称　：詳細ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    Private Sub btnShosai_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShosai.Click

        Try
            Dim pn As Panel
            Dim uc As Control

            'Me.Visible = False

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC020302)

            ' 画面遷移処理（会議通知 - 会議詳細）
            If TransitionScreen(KIND_SHOSAI) = False Then
                Exit Sub
            End If

            'If uc Is Nothing Then
            '    uc = New UC020302 '会議通知
            '    Call pn.Controls.Add(uc)
            'Else
            '    uc.Visible = True
            'End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnShosai_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "削除ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnSakujyo_Click
    '   名称　：削除ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '   履歴　：2011/11/28(月) a.onuma   削除処理の内容追加
    '***************************************************************************************************
    Private Sub btnSakujyo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSakujyo.Click
        '削除処理実行
        DeleteTemporaryData()
    End Sub

#End Region

#Region "イベント"
#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UC020301_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    Private Sub UC020301_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim dtGrant As DataTable = Nothing

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            ' コンボボックスデータ取得
            If GetData() = False Then
                Exit Sub
            End If
            '各権限の取得
            dtGrant = getGrant(MENU_ID_UC020301)

            If dtGrant.Rows.Count > 0 Then
                _strGrantReference = dtGrant.Rows(0).Item(3).ToString
                _strGrantInsert = dtGrant.Rows(0).Item(4).ToString
                _strGrantPrint = dtGrant.Rows(0).Item(5).ToString
                _strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString
            End If

            If _strGrantInsert <> GRANT_VALID Then
                Me.btnKaisai.Enabled = False
                Me.btnGoudou.Enabled = False
                Me.btnHenko.Enabled = False
                Me.btnTyushi.Enabled = False
                Me.btnSakujyo.Enabled = False
            End If

            Me.Focus()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "UC020301_Load")
            log.Fatal(ex.Message)
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "検索ボタン"
    '***************************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：検索ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            ' 検索データ取得処理呼び出し
            If GetSearchData() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSearch_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "検索ボタン（一時保存）"
    '***************************************************************************************************
    '   ＩＤ　：btnSearch0_Click
    '   名称　：検索ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************

    Private Sub btnSearch0_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch0.Click

        Try
            ' 検索データ取得処理呼び出し
            If GetSearchData0() = False Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSearch0_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "会議通知グリッドデータダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：cfgResult_DoubleClick
    '   名称　：会議通知グリッドデータダブルクリック
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma
    '***************************************************************************************************
    Private Sub cfgResult_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cfgResult.DoubleClick
        Try
            Dim pn As Panel
            Dim uc As Control

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC020302)

            ' 画面遷移処理（会議通知 - 会議詳細）
            If TransitionScreen(KIND_KAIGI) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnShosai_Click")
            log.Fatal(ex.Message)
        End Try
    End Sub
#End Region

#Region "会議通知グリッド選択データ変更時"

    '***************************************************************************************************
    '   ＩＤ　：cfgResult_SelChange
    '   名称　：グリッド選択データ変更時
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************
    Private Sub cfgResult_SelChange(ByVal sender As Object, ByVal e As System.EventArgs) Handles cfgResult.SelChange
        Dim strGoudou As String = String.Empty

        If _strGrantInsert = GRANT_VALID Then
            If Me.cfgResult.Selection.r1 < 1 Then
                '選択行がない場合のエラー回避
                Exit Sub
            End If

            If Me.cfgResult.GetUserData(Me.cfgResult.Selection.r1, 2) IsNot DBNull.Value Then
                strGoudou = Me.cfgResult.GetUserData(Me.cfgResult.Selection.r1, 2)
            End If

            '開催開始日付が過去の場合、合同、変更、中止ボタン使用不可
            If chkOpeningDate() = False Then
                Me.btnGoudou.Enabled = False
                Me.btnHenko.Enabled = False
                Me.btnTyushi.Enabled = False
            Else
                '合同ボタン使用可不可の変更
                If strGoudou = UNION_ON Then
                    '合同会議の場合、合同登録ボタンを有効にする
                    Me.btnGoudou.Enabled = True
                Else
                    Me.btnGoudou.Enabled = False
                End If

                '開催中止通知が登録された会議通知かチェック
                If Me.cfgResult.GetData(Me.cfgResult.Selection.r1, 8) = 1 Then
                    Me.btnHenko.Enabled = False
                    Me.btnTyushi.Enabled = False
                    Me.btnGoudou.Enabled = False
                Else
                    Me.btnHenko.Enabled = True
                    Me.btnTyushi.Enabled = True
                End If
            End If
        End If
    End Sub

#End Region

#Region "一時保存会議通知データダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：cfgResult0_DoubleClick
    '   名称　：一時保存会議通知グリッドデータダブルクリック
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma
    '***************************************************************************************************
    Private Sub cfgResult0_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles cfgResult0.DoubleClick
        Try
            Dim pn As Panel
            Dim uc As Control

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            uc = pn.Controls(MDConst.SCREEN_ID_UC020302)

            ' 画面遷移処理（会議通知 - 会議詳細）
            If TransitionScreen(KIND_SHOSAI) = False Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "cfgResult0_DoubleClick")
            log.Fatal(ex.Message)
        End Try

    End Sub

#End Region

#Region "Enter押下時(部／委員会)"
    '***************************************************************************************************
    '   ＩＤ　：CboCommittee_KeyPress
    '   名称　：Enter押下時(部／委員会)
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************
    Private Sub CboCommittee_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CboCommittee.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' 検索データ取得処理呼び出し
                If GetSearchData() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSearch_Click")
                log.Fatal(ex.Message)
            End Try

        End If
    End Sub
#End Region

#Region "Enter押下時（支部）"
    '***************************************************************************************************
    '   ＩＤ　：cboshibu_KeyUp
    '   名称　：Enter押下時（支部）
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************    
    Private Sub cboshibu_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboshibu.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' 検索データ取得処理呼び出し
                If GetSearchData() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSearch_Click")
                log.Fatal(ex.Message)
            End Try

        End If
    End Sub
#End Region

#Region "Enter押下時（月コンボボックス）"
    '***************************************************************************************************
    '   ＩＤ　：cboMonth_KeyUp
    '   名称　：Enter押下時（月コンボボックス）
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************    
    Private Sub cboMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboMonth.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' 検索データ取得処理呼び出し
                If GetSearchData() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSearch_Click")
                log.Fatal(ex.Message)
            End Try

        End If
    End Sub
#End Region

#Region "Enter押下時（年コンボボックス）"
    '***************************************************************************************************
    '   ＩＤ　：cboYear_KeyUp
    '   名称　：Enter押下時（年コンボボックス）
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************
    Private Sub cboYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboYear.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' 検索データ取得処理呼び出し
                If GetSearchData() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSearch_Click")
                log.Fatal(ex.Message)
            End Try

        End If
    End Sub
#End Region

#Region "年コンボボックス選択アイテム変更時"
    '***************************************************************************************************
    '   ＩＤ　：cboYear_SelectionChangeCommitted
    '   名称　：年コンボボックス選択アイテム変更時
    '   概要　：
    '   作成日：2011/11/22(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火) a.onuma
    '***************************************************************************************************
    'Private Sub cboYear_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYear.SelectionChangeCommitted
    '    If ChkNull(Me.cboYear.Text) = False Then
    '        CreateComboBoxMonth(Me.cboYear.Text.ToString(), cboMonth, False)
    '    Else
    '        Me.cboMonth.Items.Clear()
    '    End If
    'End Sub

#End Region

#Region "Enter押下時(一時保存 - 部／委員会)"
    '***************************************************************************************************
    '   ＩＤ　：CboCommittee0_KeyUp
    '   名称　：Enter押下時(一時保存 - 部／委員会)
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************
    Private Sub CboCommittee0_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles CboCommittee0.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' 検索データ取得処理呼び出し
                If GetSearchData0() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSearch0_Click")
                log.Fatal(ex.Message)
            End Try
        End If
    End Sub
#End Region

#Region "Enter押下時(一時保存 - 支部)"
    '***************************************************************************************************
    '   ＩＤ　：cboshibu0_KeyUp
    '   名称　：Enter押下時(一時保存 - 支部)
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************
    Private Sub cboshibu0_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboshibu0.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            Try
                ' 検索データ取得処理呼び出し
                If GetSearchData0() = False Then
                    Exit Sub
                End If

            Catch ex As Exception
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSearch0_Click")
                log.Fatal(ex.Message)
            End Try
        End If
    End Sub
#End Region

#Region "委員会コンボボックスの選択変更（一時保存）"
    '***************************************************************************************************
    '   ＩＤ　：CboCommittee0_SelectedIndexChanged
    '   名称　：委員会コンボボックスの選択変更（一時保存）
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************
    Private Sub CboCommittee0_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboCommittee0.SelectedIndexChanged
        ClearResultWork()
    End Sub
#End Region

#Region "支部コンボボックスの選択変更（一時保存）"
    '***************************************************************************************************
    '   ＩＤ　：cboshibu0_SelectedIndexChanged
    '   名称　：支部コンボボックスの選択変更（一時保存）
    '   概要　：
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma
    '***************************************************************************************************
    Private Sub cboshibu0_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboshibu0.SelectedIndexChanged
        ClearResultWork()
    End Sub
#End Region

#Region "会議通知検索画面のVisibleChange"
    '***************************************************************************************************
    '   ＩＤ　：UC020301_VisibleChanged
    '   名称　：会議通知検索画面のVisibleChange
    '   概要　：会議通知の更新を行った場合、検索結果を最新にする
    '   作成日：2011/12/08(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub UC020301_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged

        If Me.Visible = True Then
            If Me.cfgResult.Visible = True Then
                If _intReturnBtnFlg = 0 OrElse _intReturnBtnFlg = 2 Then
                    '会議通知登録が行われた場合検索実行
                    GetSearchData()
                End If
            End If
            If Me.cfgResult.Visible = False Then
                If _intReturnBtnFlg = 0 OrElse _intReturnBtnFlg = 2 Then
                    '一時保存会議通知登録が行われた場合検索実行
                    GetSearchData0()
                End If
            End If
        End If
    End Sub

#End Region
#End Region

#Region "部／委員会コンボボックス作成処理"
    '***************************************************************************************************
    '   ＩＤ　：CreateCbo
    '   名称　：部／委員会コンボボックス作成処理
    '   概要  ：部／委員会コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>部／委員会コンボボックス作成処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboCommittee(ByVal db As CLAccessMdb) As Boolean
        Dim blnRet As Boolean = False       '処理結果
        Dim strSql As String = ""           'SQL文
        Dim comStr As String                '管理部ユーザー参照できる委員会
        'Dim d_from As String                '期の開始日
        Dim dt As DataTable = Nothing       'データテーブル
        Dim dtRow As DataRow = Nothing      '一行のデータ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            ' 部／委員会コンボボックスクリア
            Me.CboCommittee.Items.Clear()
            '' 期の開始日を取得
            'strSql = "select d_from from period where c_period_id = '" & MDLoginInfo.PeriodId & "' "
            '' データを取得
            'dt = db.ExecuteSql(strSql)
            'If dt.Rows.Count = 1 Then
            '    dtRow = dt.Rows(0)
            '    d_from = dtRow("d_from")
            'Else
            '    CLMsg.Show("GE0004", "期の期間")
            '    Exit Function
            'End If
            '基準日取得（最新期：現在日、最新期以外：期末日）
            Dim systemDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            Dim periodDTo As String = MDLoginInfo.PeriodTo
            Dim standDate As String
            If (MDLoginInfo.PeriodNewFlg = 1) Then
                standDate = systemDate
            Else
                standDate = periodDTo
            End If

            strSql = "select c_committee_id, l_name from committee where d_from <= '" & standDate & "' and d_to >= '" & standDate & "' "
            Select Case MDLoginInfo.CommitteeStatusFlg
                Case 0
                    '専従ユーザー
                Case 1
                    '一般の委員会ユーザー
                    strSql = strSql & "and c_committee_id = '" & MDLoginInfo.CommitteeId & "' "
                Case 2
                    '管理部ユーザー
                    If MDLoginInfo.CommitteeIdList.Count > 0 Then
                        comStr = ""
                        strSql = strSql & "and c_committee_id in("
                        For intComm As Integer = 0 To MDLoginInfo.CommitteeIdList.Count - 1
                            comStr = comStr & "'" + MDLoginInfo.CommitteeIdList(intComm) & "'"
                            If intComm = MDLoginInfo.CommitteeIdList.Count - 1 Then
                                comStr = comStr & ") "
                            Else
                                comStr = comStr & ","
                            End If
                        Next
                        strSql = strSql & comStr
                    Else
                        CLMsg.Show("GE0004", "期の期間")
                        Exit Function
                    End If
            End Select

            ' ORDER BYを付加
            strSql = strSql & " order by c_committee_id"
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(db, Me.CboCommittee, strSql, "l_name", "c_committee_id", False) = False Then
                Return False
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "CreateCbo")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "部／委員会コンボボックス作成処理（一時保存側）"
    '***************************************************************************************************
    '   ＩＤ　：CreateCbo
    '   名称　：部／委員会コンボボックス作成処理
    '   概要  ：部／委員会コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>部／委員会コンボボックス作成処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboCommittee0(ByVal db As CLAccessMdb) As Boolean
        Dim blnRet As Boolean = False       '処理結果
        Dim strSql As String = ""           'SQL文
        Dim comStr As String                '管理部ユーザー参照できる委員会
        'Dim d_from As String                '期の開始日
        Dim dt As DataTable = Nothing       'データテーブル
        Dim dtRow As DataRow = Nothing      '一行のデータ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            ' 部／委員会コンボボックスクリア
            Me.CboCommittee0.Items.Clear()
            '' 期の開始日を取得
            'strSql = "select d_from from period where c_period_id = '" & MDLoginInfo.PeriodId & "' "
            '' データを取得
            'dt = db.ExecuteSql(strSql)
            'If dt.Rows.Count = 1 Then
            '    dtRow = dt.Rows(0)
            '    d_from = dtRow("d_from")
            'Else
            '    CLMsg.Show("GE0004", "期の期間")
            '    Exit Function
            'End If
            '基準日取得（最新期：現在日、最新期以外：期末日）
            Dim systemDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)
            Dim periodDTo As String = MDLoginInfo.PeriodTo
            Dim standDate As String
            If (MDLoginInfo.PeriodNewFlg = 1) Then
                standDate = systemDate
            Else
                standDate = periodDTo
            End If

            strSql = "select c_committee_id, l_name from committee where d_from <= '" & standDate & "' and d_to >= '" & standDate & "' "
            Select Case MDLoginInfo.CommitteeStatusFlg
                Case 0
                    '専従ユーザー
                Case 1
                    '一般の委員会ユーザー
                    strSql = strSql & "and c_committee_id = '" & MDLoginInfo.CommitteeId & "' "
                Case 2
                    '管理部ユーザー
                    If MDLoginInfo.CommitteeIdList.Count > 0 Then
                        comStr = ""
                        strSql = strSql & "and c_committee_id in("
                        For intComm As Integer = 0 To MDLoginInfo.CommitteeIdList.Count - 1
                            comStr = comStr & "'" + MDLoginInfo.CommitteeIdList(intComm) & "'"
                            If intComm = MDLoginInfo.CommitteeIdList.Count - 1 Then
                                comStr = comStr & ") "
                            Else
                                comStr = comStr & ","
                            End If
                        Next
                        strSql = strSql & comStr
                    Else
                        CLMsg.Show("GE0004", "期の期間")
                        Exit Function
                    End If
            End Select

            ' ORDER BYを付加
            strSql = strSql & " order by c_committee_id"
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(db, Me.CboCommittee0, strSql, "l_name", "c_committee_id", False) = False Then
                Return False
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "CreateCbo")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "一時保存データ削除処理"
    '***************************************************************************************************
    '   ＩＤ　：DeleteTemporaryData
    '   名称　：一時保存データ削除処理
    '   概要　：
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/12/05(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/05(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub DeleteTemporaryData()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Dim strSql As String = ""                   ' SQL文
        Dim clsMdb As New CLAccessMdb               ' データベースクラス生成
        Dim intRet As Integer = -1                  '処理結果件数

        If Me.cfgResult0.Selection.r1 < 1 Then
            '選択行が存在しない場合は処理終了
            Exit Sub
        End If

        Try
            Me.Cursor = Cursors.WaitCursor
            '----------------------------------------------------------------------
            '   パターン１
            '----------------------------------------------------------------------
            ' メッセージ判定
            If CLMsg.Show("GQ0011") = DialogResult.No Then
                Exit Sub
            End If

            ' 削除処理
            strSql = strSql & "DELETE " & vbCrLf
            strSql = strSql & "FROM meeting_information_temporary " & vbCrLf
            strSql = strSql & "WHERE index = " & Me.cfgResult0.GetUserData(Me.cfgResult0.Selection.r1, 0) & vbCrLf
            'strSql = strSql & "AND c_committee_id = '" & Me.CboCommittee0.SelectedValue() & "' " & vbCrLf

            clsMdb.Connect() 'DB接続
            clsMdb.BeginTran() 'トランザクション開始

            intRet = clsMdb.ExecuteNonQuery(strSql)
            log.Debug(strSql)

            If intRet <> 1 Then
                '処理結果が１行以外のときはロールバック
                clsMdb.RollbackTran()
                log.Error("DB更新処理に異常があったためデータ削除を中止しました。")
            Else
                clsMdb.CommitTran() 'コミット
                log.Info(String.Format("{0}件のデータを削除しました。", intRet.ToString()))
            End If

        Catch ex As Exception
            clsMdb.RollbackTran()
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "btnSakujyo_Click")
            log.Fatal(ex.Message)
        Finally
            clsMdb.Disconnect() '接続終了
            Me.Cursor = Cursors.Default
        End Try

        '検索処理を呼び出し画面を最新の状態にする
        If GetSearchData0() = False Then
            Exit Sub
        End If
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "各データ取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：部／委員会コンボボックス作成処理を呼び出す。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False    ' 処理結果
        Dim db As New CLAccessMdb     ' データベースクラス生成

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            ' データベース接続
            db.Connect()

            '---------------------------------------------------------------------------
            '   コンボボックス作成
            '---------------------------------------------------------------------------
            ' 部/委員会コンボボックス作成処理呼び出し
            If CreateCboCommittee(db) = False Then
                Return blnRet
            End If

            ' 部/委員会コンボボックス作成処理呼び出し
            If CreateCboCommittee0(db) = False Then
                Return blnRet
            End If

            ' 支部コンボボックス作成処理呼び出し
            'If CreateCboConstantDtl(db, Me.cboshibu, MDConst.CONSTANT_ID_UI_SHIBU) = False Then
            '    Return blnRet
            'End If
            SetBranch(Me.cboshibu)
            ' 支部コンボボックス作成処理呼び出し
            'If CreateCboConstantDtl(db, Me.cboshibu0, MDConst.CONSTANT_ID_UI_SHIBU) = False Then
            '    Return blnRet
            'End If
            SetBranch(Me.cboshibu0)

            '年をコンボボックスに格納
            If CreateComboBoxYear(cboYear) = False Then
                Return blnRet
            End If
            Dim str As String = cboYear.Name

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "GetData")
            log.Fatal(ex.Message)
        Finally
            ' データベース切断
            db.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "開催開始日付チェック"
    '***************************************************************************************************
    '   ＩＤ　：chkOpeningDate
    '   名称　：開催開始日付チェック
    '   概要　：開催開始日付が現在日時を過ぎていないかチェックを行う。
    '   引数　：なし
    '   戻り値：True = 現在日時を過ぎていない, False = 現在日時を過ぎている
    '   作成日：2011/11/23(水) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Function chkOpeningDate() As Boolean
        Dim blnRet As Boolean = True '処理結果
        Dim openingDate As DateTime = Me.cfgResult.GetData(Me.cfgResult.Selection.r1, 3)
        Dim nowDate As DateTime = System.DateTime.Now().Date

        If openingDate < nowDate Then
            blnRet = False
        End If
        Return blnRet
    End Function
#End Region

#Region "コントロールロックアンロック処理"
    '***************************************************************************************************
    '   ＩＤ　：ControlRockUnLock
    '   名称　：コントロールロックアンロック処理
    '   概要  ：各コントロールのロック・アンロックを行う。
    '   引数　：ByVal blnLock As Boolean = True：アンロック, False：ロック
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>コントロールロックアンロック処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function ControlRockUnLock(ByVal blnLock As Boolean) As Boolean

        Dim blnRet As Boolean = False                       ' 処理結果
        Dim blnReadOnly As Boolean = False

        Try
            If blnLock Then
                blnReadOnly = False
            Else
                blnReadOnly = True
            End If

            ' コンボボックス
            Me.CboCommittee.Enabled = blnLock                  ' 部/委員会
            Me.cboshibu.Enabled = blnLock                      ' 支部
            Me.cboYear.Enabled = blnLock                        ' 開催月/年
            Me.cboMonth.Enabled = blnLock                       ' 開催月/月
            Me.CboCommittee0.Enabled = blnLock                 ' 部/委員会
            Me.cboshibu0.Enabled = blnLock                     ' 支部

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "ControlRockUnLock")
            log.Fatal(ex.Message)
        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region "中止通知チェック"
    '***************************************************************************************************
    '   ＩＤ　：chkStopNotice
    '   名称　：中止通知チェック
    '   概要　：中止チェックが通知されている会議通知のフラグをたてる
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/23(水) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub chkStopNotice()
        Dim blnRet As Boolean = True '処理結果
        Dim strSql As String = String.Empty
        Dim clsMdb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing
        Dim intRetCnt As Integer = 0

        Try
            intRetCnt = Me.cfgResult.Rows.Count
            If Me.cfgResult.Rows.Count > 0 Then
                clsMdb.Connect()
                For i = 0 To intRetCnt - 2
                    strSql = "SELECT k_information_type FROM meeting_information " & _
                     "WHERE c_meeting = '" & Me.cfgResult.GetData(i + 1, 1) & "' " & _
                     "AND k_information_type = '" & INFORMATION_TYPE_STOP & "' " & _
                     "AND k_apply_area = '" & Me.cfgResult.GetUserData(i + 1, 0) & "' "

                    dtRet = clsMdb.ExecuteSql(strSql)

                    If dtRet.Rows.Count > 0 Then
                        Me.cfgResult.SetData(i + 1, 8, 1)
                    Else
                        Me.cfgResult.SetData(i + 1, 8, 0)
                    End If

                Next

                'For Each row As C1.Win.C1FlexGrid.Row In 
                '    strSql = "SELECT k_information_type FROM meeting_information " & _
                '     "WHERE c_meeting = '" & row.Item(1) & "' " & _
                '     "AND k_information_type = '" & INFORMATION_TYPE_STOP & "' " & _
                '     "AND k_apply_area = '" & Me.cfgResult.Cols("支部").UserData & "' "

                '    dtRet = clsMdb.ExecuteSql(strSql)

                '    If dtRet.Rows.Count > 0 Then
                '        row.Item(8) = 1
                '    Else
                '        row.Item(8) = 0
                '    End If
                'Next
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
    End Sub

#End Region

#Region "グリッド初期化処理（本登録側）"
    '***************************************************************************************************
    '   ＩＤ　：InitializeMeetingNoticeGrid
    '   名称　：グリッド初期化処理
    '   概要　：本登録データ用のグリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InitializeMeetingNoticeGrid() As Boolean

        Dim blnRet As Boolean = False   ' 処理結果
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try

            ' 描画なし（処理が終了した最後に描画）

            Me.cfgResult.Redraw = False
            'Me.cfgResult0.Redraw = False

            ''-----------------------------------------------------------------------------------
            ''   グリッド全体設定
            ''-----------------------------------------------------------------------------------
            '' 総数
            Me.cfgResult.Rows.Count = FLEXGRID_ROWS                                             ' 縦
            Me.cfgResult.Cols.Count = FLEXGRID_COLS                                             ' 横

            ' 固定行
            Me.cfgResult.Rows.Fixed = FLEXGRID_ROWS_FIXED                                       ' 縦
            Me.cfgResult.Cols.Fixed = FLEXGRID_COLS_FIXED                                       ' 横

            'Me.cfgResult0.Rows.Fixed = FLEXGRID_ROWS_FIXED                                       ' 縦
            'Me.cfgResult0.Cols.Fixed = FLEXGRID_COLS_FIXED                                       ' 横

            '' スクロールバー
            ''Me.cfgResult.ScrollBars = ScrollBars.Vertical = C1FLEXGRID_SCROLLBARS_VERTICAL      ' 縦のみ
            ''Me.cfgResult0.ScrollBars = ScrollBars.Vertical = C1FLEXGRID_SCROLLBARS_VERTICAL      ' 縦のみ

            '' 1行選択モード
            Me.cfgResult.FocusRect = C1FLEXGRID_FOCUS_RECT_ENUM_NONE                            ' フォーカス矩形なし
            'Me.cfgResult.SelectionMode = C1FLEXGRID_SELECTION_MODE_ENUM_ROW                     ' 一度に１つの行を選択

            'Me.cfgResult0.SelectionMode = C1FLEXGRID_SELECTION_MODE_ENUM_ROW                     ' 一度に１つの行を選択

            ''-----------------------------------------------------------------------------------
            ''   ヘッダー部設定
            ''-----------------------------------------------------------------------------------
            '' ヘッダー文字列
            Me.cfgResult.Cols(0).Caption = "支部"                                             ' 支部
            Me.cfgResult.Cols(1).Caption = "通知番号"                                         ' 通知番号
            Me.cfgResult.Cols(2).Caption = "合同"                                             ' 合同
            Me.cfgResult.Cols(3).Caption = "開催開始日付"                                     ' 開催開始日付
            Me.cfgResult.Cols(4).Caption = "種類"                                             ' 種類
            Me.cfgResult.Cols(5).Caption = "会議名"                                           ' 会議名
            Me.cfgResult.Cols(6).Caption = "開催場所"                                         ' 開催場所
            Me.cfgResult.Cols(7).Caption = "会議場"                                           ' 会議場
            Me.cfgResult.Cols(8).Caption = "中止フラグ"
            '中止フラグは隠し列にする
            Me.cfgResult.Cols(8).Visible = False
            Me.cfgResult.AllowMerging = AllowMergingEnum.Free
            Me.cfgResult.Cols(0).AllowMerging = True
            Me.cfgResult.Cols(1).AllowMerging = True

            'Me.cfgResult0.Cols(0).Caption = "支部"                                             ' 支部
            'Me.cfgResult0.Cols(1).Caption = "通知番号"                                         ' 通知番号
            'Me.cfgResult0.Cols(2).Caption = "合同"                                             ' 合同
            'Me.cfgResult0.Cols(3).Caption = "開催開始日付"                                     ' 開催開始日付
            'Me.cfgResult0.Cols(4).Caption = "種類"                                             ' 種類
            'Me.cfgResult0.Cols(5).Caption = "会議名"                                           ' 会議名
            'Me.cfgResult0.Cols(6).Caption = "開催場所"                                         ' 開催場所
            'Me.cfgResult0.Cols(7).Caption = "会議場"                                           ' 会議場

            ' ヘッダー文字位置

            Me.cfgResult.Cols(0).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER       ' 支部  
            Me.cfgResult.Cols(1).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 通知番号
            Me.cfgResult.Cols(2).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 合同
            Me.cfgResult.Cols(3).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 開催開始日付
            Me.cfgResult.Cols(4).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 種類
            Me.cfgResult.Cols(5).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 会議名
            Me.cfgResult.Cols(6).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 開催場所
            Me.cfgResult.Cols(7).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 会議場

            'Me.cfgResult0.Cols(0).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER       ' 支部  
            'Me.cfgResult0.Cols(1).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER         ' 通知番号
            'Me.cfgResult0.Cols(2).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER         ' 合同
            'Me.cfgResult0.Cols(3).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER         ' 開催開始日付
            'Me.cfgResult0.Cols(4).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER         ' 種類
            'Me.cfgResult0.Cols(5).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER         ' 会議名
            'Me.cfgResult0.Cols(6).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER         ' 開催場所
            'Me.cfgResult0.Cols(7).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER         ' 会議場

            '---------------------------------------------------------------------------
            '   カラム部設定
            '---------------------------------------------------------------------------
            ' カラム文字位置
            Me.cfgResult.Cols(0).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER            ' 支部
            Me.cfgResult.Cols(1).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 通知番号
            Me.cfgResult.Cols(2).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 合同
            Me.cfgResult.Cols(3).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 開催開始日付
            Me.cfgResult.Cols(4).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 種類
            Me.cfgResult.Cols(5).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 会議名
            Me.cfgResult.Cols(6).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 開催場所
            Me.cfgResult.Cols(7).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 会議場

            'Me.cfgResult0.Cols(0).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER            ' 支部
            'Me.cfgResult0.Cols(1).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 通知番号
            'Me.cfgResult0.Cols(2).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 合同
            'Me.cfgResult0.Cols(3).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 開催開始日付
            'Me.cfgResult0.Cols(4).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 種類
            'Me.cfgResult0.Cols(5).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 会議名
            'Me.cfgResult0.Cols(6).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 開催場所
            'Me.cfgResult0.Cols(7).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 会議場

            ' カラム幅
            Me.cfgResult.Cols(0).Width = 70                                                     ' 支部
            Me.cfgResult.Cols(1).Width = 100                                                    ' 通知番号
            Me.cfgResult.Cols(2).Width = 70                                                     ' 合同
            Me.cfgResult.Cols(3).Width = 130                                                    ' 開催開始日付
            Me.cfgResult.Cols(4).Width = 70                                                     ' 種類
            Me.cfgResult.Cols(5).Width = 193                                                    ' 会議名
            Me.cfgResult.Cols(6).Width = 106                                                    ' 開催場所
            Me.cfgResult.Cols(7).Width = 160                                                    ' 会議場

            '' カラム表示有無
            'Me.cfgResult.Cols(0).Visible = True                                                 ' 支部
            'Me.cfgResult.Cols(1).Visible = True                                                 ' 通知番号
            'Me.cfgResult.Cols(2).Visible = True                                                 ' 合同
            'Me.cfgResult.Cols(3).Visible = True                                                 ' 開催開始日付
            'Me.cfgResult.Cols(4).Visible = True                                                 ' 種類
            'Me.cfgResult.Cols(5).Visible = True                                                 ' 会議名
            'Me.cfgResult.Cols(6).Visible = True                                                 ' 開催場所
            'Me.cfgResult.Cols(7).Visible = True                                                 ' 会議場

            'Me.cfgResult0.Cols(0).Visible = True                                                 ' 支部
            'Me.cfgResult0.Cols(1).Visible = True                                                 ' 通知番号
            'Me.cfgResult0.Cols(2).Visible = True                                                 ' 合同
            'Me.cfgResult0.Cols(3).Visible = True                                                 ' 開催開始日付
            'Me.cfgResult0.Cols(4).Visible = True                                                 ' 種類
            'Me.cfgResult0.Cols(5).Visible = True                                                 ' 会議名
            'Me.cfgResult0.Cols(6).Visible = True                                                 ' 開催場所
            'Me.cfgResult0.Cols(7).Visible = True                                                 ' 会議場

            '' 描画
            Me.cfgResult.Redraw = True

            ' 戻り値格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "グリッド初期化処理（一時保存側）"
    '***************************************************************************************************
    '   ＩＤ　：InitializeMeetingNoticeTemporaryGrid
    '   名称　：グリッド初期化処理
    '   概要　：一時保存用のグリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InitializeMeetingNoticeTemporaryGrid() As Boolean

        Dim blnRet As Boolean = False   ' 処理結果
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try

            ' 描画なし（処理が終了した最後に描画）
            Me.cfgResult0.Redraw = False

            '-----------------------------------------------------------------------------------
            '   グリッド全体設定
            '-----------------------------------------------------------------------------------
            ' 総数
            Me.cfgResult0.Rows.Count = FLEXGRID_ROWS                                             ' 縦
            Me.cfgResult0.Cols.Count = FLEXGRID_COLS - 1                                         ' 横
            ' 固定行
            Me.cfgResult0.Rows.Fixed = FLEXGRID_ROWS_FIXED                                       ' 縦
            Me.cfgResult0.Cols.Fixed = FLEXGRID_COLS_FIXED                                       ' 横

            Me.cfgResult0.FocusRect = C1FLEXGRID_FOCUS_RECT_ENUM_NONE                            ' フォーカス矩形なし

            '-----------------------------------------------------------------------------------
            '   ヘッダー部設定
            '-----------------------------------------------------------------------------------
            ' ヘッダー文字列
            Me.cfgResult0.Cols(0).Caption = "支部"                                             ' 支部
            Me.cfgResult0.Cols(1).Caption = "通知番号"                                         ' 通知番号
            Me.cfgResult0.Cols(2).Caption = "合同"                                             ' 合同
            Me.cfgResult0.Cols(3).Caption = "開催開始日付"                                     ' 開催開始日付
            Me.cfgResult0.Cols(4).Caption = "種類"                                             ' 種類
            Me.cfgResult0.Cols(5).Caption = "会議名"                                           ' 会議名
            Me.cfgResult0.Cols(6).Caption = "開催場所"                                         ' 開催場所
            Me.cfgResult0.Cols(7).Caption = "会議場"                                           ' 会議場

            Me.cfgResult0.AllowMerging = AllowMergingEnum.Free
            Me.cfgResult0.Cols(0).AllowMerging = True
            Me.cfgResult0.Cols(1).AllowMerging = True

            ' ヘッダー文字位置
            Me.cfgResult0.Cols(0).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER       ' 支部  
            Me.cfgResult0.Cols(1).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 通知番号
            Me.cfgResult0.Cols(2).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 合同
            Me.cfgResult0.Cols(3).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 開催開始日付
            Me.cfgResult0.Cols(4).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 種類
            Me.cfgResult0.Cols(5).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 会議名
            Me.cfgResult0.Cols(6).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 開催場所
            Me.cfgResult0.Cols(7).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER         ' 会議場

            '---------------------------------------------------------------------------
            '   カラム部設定
            '---------------------------------------------------------------------------
            ' カラム文字位置
            Me.cfgResult0.Cols(0).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER            ' 支部
            Me.cfgResult0.Cols(1).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 通知番号
            Me.cfgResult0.Cols(2).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 合同
            Me.cfgResult0.Cols(3).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 開催開始日付
            Me.cfgResult0.Cols(4).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 種類
            Me.cfgResult0.Cols(5).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 会議名
            Me.cfgResult0.Cols(6).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 開催場所
            Me.cfgResult0.Cols(7).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER              ' 会議場

            ' カラム幅
            Me.cfgResult0.Cols(0).Width = 70                                                     ' 支部
            Me.cfgResult0.Cols(1).Width = 100                                                    ' 通知番号
            Me.cfgResult0.Cols(2).Width = 70                                                     ' 合同
            Me.cfgResult0.Cols(3).Width = 130                                                    ' 開催開始日付
            Me.cfgResult0.Cols(4).Width = 70                                                     ' 種類
            Me.cfgResult0.Cols(5).Width = 193                                                    ' 会議名
            Me.cfgResult0.Cols(6).Width = 106                                                    ' 開催場所
            Me.cfgResult0.Cols(7).Width = 160                                                    ' 会議場

            ' 描画
            Me.cfgResult0.Redraw = True

            ' 戻り値格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "検索データ取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetSearchData
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>検索データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSearchData() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim strWhere As String = ""                 ' Where句
        Dim clsMdb As New CLAccessMdb               ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数
        Dim strMemberNo As String = ""              ' 検索項目　部/委員会
        Dim strShibu As String = ""                 ' 検索項目　支部

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        'それぞれ、選択項目に対応する区分を取得
        If Me.CboCommittee.SelectedValue() IsNot DBNull.Value Then '部／委員会
            strMemberNo = Me.CboCommittee.SelectedValue()
        End If
        If ChkNull(Me.cboshibu.Text) = False Then '支部
            strShibu = _dicBranch(Me.cboshibu.Text)
        End If
        Try
            'マウスポインタを砂時計へ変更
            Me.Cursor = Cursors.WaitCursor

            '開催年月チェック
            If ChkNull(Me.cboYear.Text.Trim()) = False AndAlso ChkNull(Me.cboMonth.Text.Trim()) = False Then
                If ChkTargetDate(MDLoginInfo.PeriodId, Me.CboCommittee.SelectedValue, Me.cboYear.Text.Trim() & Me.cboMonth.Text.Trim()) = False Then
                    Exit Function
                End If
            End If

            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            ' SELECT句
            strSql = strSql & "SELECT meeting_information.* " & vbCrLf
            'strSql = strSql & "SELECT meeting_information.k_apply_area" & vbCrLf                     ' 01. 支部
            'strSql = strSql & "      ,meeting_information.c_meeting" & vbCrLf                        ' 02. 通知番号
            'strSql = strSql & "      ,meeting_information.k_union" & vbCrLf                          ' 03. 合同
            'strSql = strSql & "      ,meeting_information.d_meeting_1" & vbCrLf                      ' 04. 開催開始日付
            'strSql = strSql & "      ,meeting_information.k_information_type" & vbCrLf               ' 05. 種類
            'strSql = strSql & "      ,meeting_information.l_information_name" & vbCrLf               ' 06. 会議名
            'strSql = strSql & "      ,meeting_information.l_open_bebiginting" & vbCrLf               ' 07. 開催場所
            'strSql = strSql & "      ,meeting_information.l_place" & vbCrLf                          ' 08. 会議場

            ' FROM句
            strSql = strSql & "  FROM meeting_information " & vbCrLf                           ' 会議通知テーブル

            ' WHERE句
            strSql = strSql & " WHERE meeting_information.c_period_id = '" & MDLoginInfo.PeriodId & "' "
            ' 部/
            If ChkNull(strMemberNo) = False Then
                strSql = strSql & "   AND meeting_information.c_committee_id = '" & strMemberNo & "' " & vbCrLf
            End If
            ' 支部
            If ChkNull(strShibu) = False Then
                strSql = strSql & "   AND meeting_information.k_apply_area = '" & strShibu & "' " & vbCrLf
            End If
            '開催月
            If ChkNull(Me.cboYear.Text.Trim()) = False AndAlso ChkNull(Me.cboMonth.Text.Trim()) = False Then
                strSql = strSql & "   AND FORMAT(meeting_information.d_meeting_1,'yyyyMM')='" & Me.cboYear.Text.Trim() &
                        Me.cboMonth.Text.Trim() & "' " & vbCrLf
                'strSql = strSql & "   AND meeting_information.d_meeting_1 LIKE '" & Me.cboYear.Text.Trim() &
                '        "/" & Me.cboMonth.Text.Trim() & "%' " & vbCrLf
            ElseIf ChkNull(Me.cboYear.Text.Trim()) = False Then
                strSql = strSql & "   AND FORMAT(meeting_information.d_meeting_1,'yyyy')='" & Me.cboYear.Text.Trim() & "' " & vbCrLf
                'strSql = strSql & "   AND meeting_information.d_meeting_1 LIKE '" & Me.cboYear.Text.Trim() & "%' " & vbCrLf
            End If
            '支部、通知番号順にソート
            strSql = strSql & " ORDER BY meeting_information.k_apply_area, Len(meeting_information.c_meeting) " & _
            ",meeting_information.c_meeting,meeting_information.s_meeting "

            ' データベース接続
            clsMdb.Connect()

            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)
            log.Debug(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' グリッド初期化()
            If InitializeMeetingNoticeGrid() = False Then
                Exit Function
            End If

            ' 件数チェック
            If intRetCnt > 0 Then
                _miList = New List(Of DataRow)

                ' 1件以上の処理
                Me.grpSearchResult.Visible = True                               ' グループボックス表示
                Me.cfgResult.Rows.Count = intRetCnt + 1                         ' 縦総数設定
                For i = 0 To intRetCnt - 1                                      ' レコード数分ループ
                    ' データ設定
                    '01.支部
                    If CInt(tbRet.Rows(i).Item("k_apply_area")) = MDConst.UI_SHIBU_TOKYO Then
                        Me.cfgResult.SetData(i + 1, 0, SHIBU_TOKYO)
                    ElseIf CInt(tbRet.Rows(i).Item("k_apply_area")) = MDConst.UI_SHIBU_OSAKA Then
                        Me.cfgResult.SetData(i + 1, 0, SHIBU_OOSAKA)
                    ElseIf CInt(tbRet.Rows(i).Item("k_apply_area")) = MDConst.UI_SHIBU_ETC Then
                        Me.cfgResult.SetData(i + 1, 0, SHIBU_OTHER)
                    Else
                        Me.cfgResult.SetData(i + 1, 0, String.Empty)
                    End If
                    Me.cfgResult.SetUserData(i + 1, 0, tbRet.Rows(i).Item("k_apply_area"))

                    '02.通知番号
                    Me.cfgResult.SetData(i + 1, 1, tbRet.Rows(i).Item("c_meeting"))

                    '03.合同
                    If CInt(tbRet.Rows(i).Item("k_union")) = UNION_ON Then
                        Me.cfgResult.SetData(i + 1, 2, UNION_ON_STRING)
                    End If
                    Me.cfgResult.SetUserData(i + 1, 2, tbRet.Rows(i).Item("k_union"))

                    '04.開催開始日付
                    If tbRet.Rows(i).Item("d_meeting_1").Equals(DBNull.Value) OrElse _
                       tbRet.Rows(i).Item("d_meeting_1").ToString().Length <= 10 Then
                        Me.cfgResult.SetData(i + 1, 3, tbRet.Rows(i).Item("d_meeting_1"))
                    Else
                        Me.cfgResult.SetData(i + 1, 3, tbRet.Rows(i).Item("d_meeting_1").ToString().Substring(0, 10))
                    End If

                    '05.種類
                    If CInt(tbRet.Rows(i).Item("k_information_type")) = INFORMATION_TYPE_OPEN Then
                        Me.cfgResult.SetData(i + 1, 4, INFORMATION_TYPE_OPEN_STRING)
                    ElseIf CInt(tbRet.Rows(i).Item("k_information_type")) = INFORMATION_TYPE_CHANGE Then
                        Me.cfgResult.SetData(i + 1, 4, INFORMATION_TYPE_CHANGE_STRING)
                    ElseIf CInt(tbRet.Rows(i).Item("k_information_type")) = INFORMATION_TYPE_STOP Then
                        Me.cfgResult.SetData(i + 1, 4, INFORMATION_TYPE_STOP_STRING)
                    End If
                    Me.cfgResult.SetUserData(i + 1, 4, tbRet.Rows(i).Item("k_information_type"))

                    '06.会議名
                    Me.cfgResult.SetData(i + 1, 5, tbRet.Rows(i).Item("l_information_name"))
                    '07.開催場所
                    Me.cfgResult.SetData(i + 1, 6, tbRet.Rows(i).Item("l_open_bebiginting"))
                    '08.会議場
                    Me.cfgResult.SetData(i + 1, 7, tbRet.Rows(i).Item("l_place"))

                    _miList.Add(tbRet.Rows(i))
                Next

                chkStopNotice()
                'Me.dgdResult.CurrentCell = Nothing
                ' ボタン設定
                Me.btnKaigi.Enabled = True
            Else
                ' 0件の処理
                'CLMsg.Show("DI0001")                                            ' 対象データなしメッセージボックス表示
                Me.grpSearchResult.Visible = True                               ' グループボックス表示
                Me.cfgResult.Rows.Count = intRetCnt + 1                         ' 縦総数設定
                ' ボタン設定
                Me.btnKaigi.Enabled = False
                Me.btnGoudou.Enabled = False
                Me.btnHenko.Enabled = False
                Me.btnTyushi.Enabled = False
            End If

            ' グループボックス件数設定
            Me.grpSearchResult.Text = "検索結果（ " & intRetCnt & " 件 ）"

            ' レコード単位ボタン制御の初期化処理追加
            Dim sender As Object = Nothing
            Dim e As System.EventArgs = Nothing
            Call Me.cfgResult_SelChange(sender, e)

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "GetSearchData")
            log.Fatal(ex.Message)
        Finally
            ' データベース切断
            clsMdb.Disconnect()
            'マウスポインタを元に戻す
            Me.Cursor = Cursors.Default
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region "検索データ取得処理（一時保存）"
    '***************************************************************************************************
    '   ＩＤ　：GetSearchData0
    '   名称　：検索データ取得処理（一時保存）
    '   概要  ：一時保存会議通知の検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>検索データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetSearchData0() As Boolean

        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String = ""                   ' SQL文
        Dim strWhere As String = ""                 ' Where句
        Dim clsMdb As New CLAccessMdb               ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数
        Dim strMemberNo As String = ""              ' 検索項目　部/委員会
        Dim strShibu As String = ""                 ' 検索項目　支部

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        'それぞれ、選択項目に対応する区分を取得
        If Me.CboCommittee0.SelectedValue() IsNot DBNull.Value Then '部／委員会
            strMemberNo = Me.CboCommittee0.SelectedValue()
        End If
        If ChkNull(Me.cboshibu0.Text) = False Then '支部
            strShibu = _dicBranch(Me.cboshibu0.Text)
        End If
        Try
            'マウスポインタを砂時計へ変更
            Me.Cursor = Cursors.WaitCursor
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            ' SELECT句
            strSql = strSql & "SELECT meeting_information_temporary.*" & vbCrLf
            ' FROM句
            strSql = strSql & "  FROM meeting_information_temporary" & vbCrLf                           ' 会議通知テーブル

            ' WHERE句
            strSql = strSql & " WHERE meeting_information_temporary.c_period_id = '" & MDLoginInfo.PeriodId & "' "
            ' 部/委員会
            If ChkNull(strMemberNo) = False Then
                strSql = strSql & "   AND meeting_information_temporary.c_committee_id = '" & strMemberNo & "' " & vbCrLf
            End If
            ' 支部
            If ChkNull(strShibu) = False Then
                strSql = strSql & "   AND meeting_information_temporary.k_apply_area = '" & strShibu & "'" & vbCrLf
            End If
            '支部、通知番号順にソート
            strSql = strSql & " ORDER BY meeting_information_temporary.k_apply_area, Len(meeting_information_temporary.c_meeting) " & _
            ",meeting_information_temporary.c_meeting,meeting_information_temporary.s_meeting "

            ' データベース接続
            clsMdb.Connect()

            ' SQL実行
            tbRet = clsMdb.ExecuteSql(strSql)
            log.Debug(strSql)

            ' 件数取得
            intRetCnt = tbRet.Rows.Count

            ' グリッド初期化()
            If InitializeMeetingNoticeTemporaryGrid() = False Then
                Exit Function
            End If

            ' 件数チェック
            If intRetCnt > 0 Then
                _miTemporaryList = New List(Of DataRow)

                ' 1件以上の処理
                Me.grpSearchResult0.Visible = True                               ' グループボックス表示
                Me.cfgResult0.Rows.Count = intRetCnt + 1                         ' 縦総数設定
                For i = 0 To intRetCnt - 1                                      ' レコード数分ループ
                    ' データ設定
                    '01.支部
                    If CInt(tbRet.Rows(i).Item("k_apply_area")) = MDConst.UI_SHIBU_TOKYO Then
                        Me.cfgResult0.SetData(i + 1, 0, SHIBU_TOKYO)
                    ElseIf CInt(tbRet.Rows(i).Item("k_apply_area")) = MDConst.UI_SHIBU_OSAKA Then
                        Me.cfgResult0.SetData(i + 1, 0, SHIBU_OOSAKA)
                    ElseIf CInt(tbRet.Rows(i).Item("k_apply_area")) = MDConst.UI_SHIBU_ETC Then
                        Me.cfgResult0.SetData(i + 1, 0, SHIBU_OTHER)
                    Else
                        Me.cfgResult0.SetData(i + 1, 0, String.Empty)
                    End If
                    Me.cfgResult0.SetUserData(i + 1, 0, tbRet.Rows(i).Item("index"))

                    '02.通知番号
                    Me.cfgResult0.SetData(i + 1, 1, tbRet.Rows(i).Item("c_meeting"))

                    '03.合同
                    If CInt(tbRet.Rows(i).Item("k_union")) = UNION_ON Then
                        Me.cfgResult0.SetData(i + 1, 2, UNION_ON_STRING)
                    End If

                    '04.開催開始日付
                    If tbRet.Rows(i).Item("d_meeting_1").Equals(DBNull.Value) OrElse _
                       tbRet.Rows(i).Item("d_meeting_1").ToString().Length < 11 Then
                        Me.cfgResult0.SetData(i + 1, 3, tbRet.Rows(i).Item("d_meeting_1"))
                    Else
                        Me.cfgResult0.SetData(i + 1, 3, tbRet.Rows(i).Item("d_meeting_1").ToString().Substring(0, 10))
                    End If

                    '05.種類
                    If CInt(tbRet.Rows(i).Item("k_information_type")) = INFORMATION_TYPE_OPEN Then
                        Me.cfgResult0.SetData(i + 1, 4, INFORMATION_TYPE_OPEN_STRING)
                    ElseIf CInt(tbRet.Rows(i).Item("k_information_type")) = INFORMATION_TYPE_CHANGE Then
                        Me.cfgResult0.SetData(i + 1, 4, INFORMATION_TYPE_CHANGE_STRING)
                    ElseIf CInt(tbRet.Rows(i).Item("k_information_type")) = INFORMATION_TYPE_STOP Then
                        Me.cfgResult0.SetData(i + 1, 4, INFORMATION_TYPE_STOP_STRING)
                    End If

                    '06.会議名
                    Me.cfgResult0.SetData(i + 1, 5, tbRet.Rows(i).Item("l_information_name"))
                    '07.開催場所
                    Me.cfgResult0.SetData(i + 1, 6, tbRet.Rows(i).Item("l_open_bebiginting"))
                    '08.会議場
                    Me.cfgResult0.SetData(i + 1, 7, tbRet.Rows(i).Item("l_place"))

                    _miTemporaryList.Add(tbRet.Rows(i))
                Next
                ' ボタン設定
                Me.btnShosai.Enabled = True
                Me.btnSakujyo.Enabled = _strGrantInsert.Equals(GRANT_VALID)
            Else
                ' 0件の処理
                'CLMsg.Show("DI0001")                                            ' 対象データなしメッセージボックス表示
                Me.grpSearchResult0.Visible = True                               ' グループボックス表示
                Me.cfgResult0.Rows.Count = intRetCnt + 1                         ' 縦総数設定
                ' ボタン設定
                Me.btnShosai.Enabled = False
                Me.btnSakujyo.Enabled = False
            End If

            ' グループボックス件数設定
            Me.grpSearchResult0.Text = "検索結果（ " & intRetCnt & " 件 ）"

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "GetSearchData0")
            log.Fatal(ex.Message)
        Finally
            ' データベース切断
            clsMdb.Disconnect()
            'マウスポインタを元に戻す
            Me.Cursor = Cursors.Default
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region "画面遷移処理"
    '***************************************************************************************************
    '   ＩＤ　：TransitionScreen
    '   名称　：画面遷移処理
    '   概要  ：画面遷移処理を行う。
    '   引数　：ByVal bytKind As Byte = 1：会議通知 - 登録,変更,中止,合同登録,会議詳細 
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/14(月) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/14(月) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>画面遷移処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function TransitionScreen(ByVal bytKind As Byte) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim pn As Panel                     ' パネルオブジェクト
        Dim clsUC020302 As UC020302         ' 会議通知 - 登録

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try

            ' 選択されているかチェック
            If bytKind = KIND_SHOSAI Then
                If Me.cfgResult0.Selection.r1 < 1 Then
                    CLMsg.Show("GW0001", "データ")                                               ' 未選択の場合、エラーメッセージ表示
                    Return blnRet
                End If
            Else
                If bytKind <> KIND_KAISAI AndAlso Me.cfgResult.Selection.r1 < 1 Then
                    CLMsg.Show("GW0001", "データ")                                               ' 未選択の場合、エラーメッセージ表示
                    Return blnRet
                End If
            End If

            Me.Cursor = Cursors.WaitCursor
            Me.Visible = False
            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            clsUC020302 = pn.Controls(SCREEN_ID_UC020302)

            ' 会議通知 - 登録
            If bytKind = KIND_KAISAI Then
                '---------------------------------------------------------------------------
                '   会議通知 - 新規登録
                '---------------------------------------------------------------------------
                If clsUC020302 Is Nothing Then
                    ' 画面間パラメータ情報設定
                    clsUC020302 = New UC020302                                              ' 会議通知 - 登録
                    clsUC020302.IntClickBtnFlg = 0
                    Call pn.Controls.Add(clsUC020302)                                       ' 
                Else
                    clsUC020302.IntClickBtnFlg = 0
                    pn.Visible = True                                                       ' パネル表示
                End If

            Else
                If bytKind = KIND_HENKO Then
                    '---------------------------------------------------------------------------
                    '   会議通知 - 変更
                    '---------------------------------------------------------------------------
                    If clsUC020302 Is Nothing Then
                        ' 画面間パラメータ情報設定
                        clsUC020302 = New UC020302                                              ' 会議通知 - 変更
                        clsUC020302.IntClickBtnFlg = 1
                        clsUC020302.MeetingInformation = _miList(Me.cfgResult.Selection.r1 - 1)
                        Call pn.Controls.Add(clsUC020302)
                    Else
                        clsUC020302.IntClickBtnFlg = 1
                        clsUC020302.MeetingInformation = _miList(Me.cfgResult.Selection.r1 - 1)
                        pn.Visible = True                                                       ' パネル表示
                    End If

                ElseIf bytKind = KIND_TYUSHI Then
                    '---------------------------------------------------------------------------
                    '   会議通知 - 中止
                    '---------------------------------------------------------------------------
                    If clsUC020302 Is Nothing Then
                        ' 画面間パラメータ情報設定
                        clsUC020302 = New UC020302                                              ' 会議通知 - 中止
                        clsUC020302.IntClickBtnFlg = 2
                        clsUC020302.MeetingInformation = _miList(Me.cfgResult.Selection.r1 - 1)
                        Call pn.Controls.Add(clsUC020302)                                       '
                    Else
                        clsUC020302.IntClickBtnFlg = 2
                        clsUC020302.MeetingInformation = _miList(Me.cfgResult.Selection.r1 - 1)
                        pn.Visible = True                                                       ' パネル表示
                    End If
                ElseIf bytKind = KIND_GOUDOU Then
                    '---------------------------------------------------------------------------
                    '   会議通知 - 合同登録
                    '---------------------------------------------------------------------------
                    If clsUC020302 Is Nothing Then
                        ' 画面間パラメータ情報設定
                        clsUC020302 = New UC020302                                              ' 会議通知 - 合同登録
                        clsUC020302.IntClickBtnFlg = 3
                        clsUC020302.MeetingInformation = _miList(Me.cfgResult.Selection.r1 - 1)
                        Call pn.Controls.Add(clsUC020302)                                       ' 
                    Else
                        clsUC020302.IntClickBtnFlg = 3
                        clsUC020302.MeetingInformation = _miList(Me.cfgResult.Selection.r1 - 1)
                        pn.Visible = True                                                       ' パネル表示
                    End If

                ElseIf bytKind = KIND_KAIGI Then
                    '---------------------------------------------------------------------------
                    '   会議通知 - 会議詳細
                    '---------------------------------------------------------------------------
                    If clsUC020302 Is Nothing Then
                        ' 画面間パラメータ情報設定
                        clsUC020302 = New UC020302                                              ' 会議通知 - 会議詳細
                        clsUC020302.IntClickBtnFlg = 4
                        clsUC020302.MeetingInformation = _miList(Me.cfgResult.Selection.r1 - 1)
                        Call pn.Controls.Add(clsUC020302)                                       ' 
                    Else
                        clsUC020302.IntClickBtnFlg = 4
                        clsUC020302.MeetingInformation = _miList(Me.cfgResult.Selection.r1 - 1)
                        pn.Visible = True                                                       ' パネル表示
                    End If

                ElseIf bytKind = KIND_SHOSAI Then
                    '---------------------------------------------------------------------------
                    '   一時保存会議通知 - 会議詳細
                    '---------------------------------------------------------------------------
                    clsUC020302 = pn.Controls("UC020302")
                    If clsUC020302 Is Nothing Then
                        ' 画面間パラメータ情報設定
                        clsUC020302 = New UC020302                                              ' 一時保存会議通知 - 会議詳細
                        clsUC020302.IntClickBtnFlg = 5
                        clsUC020302.MeetingInformation = _miTemporaryList(Me.cfgResult0.Selection.r1 - 1)
                        Call pn.Controls.Add(clsUC020302)                                       ' 
                    Else
                        clsUC020302.IntClickBtnFlg = 5
                        clsUC020302.MeetingInformation = _miTemporaryList(Me.cfgResult0.Selection.r1 - 1)
                        pn.Visible = True                                                       ' パネル表示
                    End If
                End If
            End If

            ' 戻り値設定
            blnRet = True
            pn.Controls(MDConst.SCREEN_ID_UC020302).Focus()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "TransitionScreen")
            log.Fatal(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        ' 戻り値設定
        Return blnRet

    End Function

#End Region

#Region "月コンボボックス設定処理"
    '***************************************************************************************************
    '   ＩＤ　：SetMonth
    '   名称　：月コンボボックス設定処理
    '   概要  ：月コンボボックスに値を設定する。
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetMonth()
        Me.cboMonth.Items.Add("")
        For iCnt As Integer = 1 To 12
            Me.cboMonth.Items.Add(iCnt.ToString.PadLeft(2, "0"))
        Next
    End Sub
#End Region

#Region "支部コンボボックス設定処理"
    '***************************************************************************************************
    '   ＩＤ　：SetBranch
    '   名称　：支部コンボボックス設定処理
    '   概要  ：支部コンボボックスに値を設定する。
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetBranch(ByVal targetCbo As ComboBox)
        Dim clsMdb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim dtRet As DataTable = Nothing

        If _dicBranch Is Nothing Then
            _dicBranch = New Dictionary(Of String, String)
        End If
        Try
            '組合支部の取得
            strSql = "SELECT c_constant_seq,l_name FROM constant_dtl " & _
                     "WHERE c_constant = 'BELONGING' "

            '接続開始
            clsMdb.Connect()

            dtRet = clsMdb.ExecuteSql(strSql)

            targetCbo.Items.Add("")
            If dtRet.Rows.Count > 0 Then
                For Each dtRow As DataRow In dtRet.Rows
                    If dtRow.Item("l_name") <> SHIBU_OTHER Then
                        targetCbo.Items.Add(dtRow.Item("l_name"))
                    End If
                Next

                If _dicBranch.Count < 1 Then
                    For Each dtRow As DataRow In dtRet.Rows
                        If dtRow.Item("l_name") <> SHIBU_OTHER Then
                            _dicBranch.Add(dtRow.Item("l_name"), dtRow.Item("c_constant_seq"))
                        End If
                    Next
                End If
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020301, SCREEN_NAME_UC020301, "SetBranch")
            log.Fatal(ex.Message)
        Finally
            '接続終了
            clsMdb.Disconnect()
        End Try
    End Sub
#End Region

#Region "検索結果初期化"
    '***************************************************************************************************
    '   ＩＤ　：ClearResult
    '   名称　：検索結果初期化
    '   概要  ：グリッドの検索結果をクリアする。
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ClearResult()
        Me.cfgResult.Clear()
        Me.cfgResult.Rows.Count = 0
        Me.grpSearchResult.Text = "検索結果（ 0 件 ）"
        Me.grpSearchResult.Visible = False
    End Sub
#End Region

#Region "一時保存の検索結果初期化"
    '***************************************************************************************************
    '   ＩＤ　：ClearResultWork
    '   名称　：一時保存の検索結果初期化
    '   概要  ：一時保存会議通知のグリッドの検索結果をクリアする。
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ClearResultWork()
        Me.cfgResult0.Clear()
        Me.cfgResult0.Rows.Count = 0
        Me.grpSearchResult0.Text = "検索結果（ 0 件 ）"
        Me.grpSearchResult0.Visible = False
    End Sub

#End Region

    '***************************************************************************************************
    '   ＩＤ　：cboYear_SelectedIndexChanged
    '   名称　：年コンボボックスの選択値変更
    '   概要  ：年コンボボックスの変更に伴い月コンボボックスを変更する
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cboYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboYear.SelectedIndexChanged
        '検索結果のクリア
        ClearResult()
        If Me.cboYear.Text.Trim = String.Empty Then
            Me.cboMonth.Items.Clear()
        Else
            If Me.cboMonth.Items.Count = 0 Then
                SetMonth()
            End If
        End If
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：CboCommittee_SelectedIndexChanged
    '   名称　：部／委員会コンボボックスの選択値変更
    '   概要  ：
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub CboCommittee_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CboCommittee.SelectedIndexChanged
        '検索結果のクリア
        ClearResult()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboshibu_SelectedIndexChanged
    '   名称　：支部コンボボックスの選択値変更
    '   概要  ：
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cboshibu_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboshibu.SelectedIndexChanged
        '検索結果のクリア
        ClearResult()
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：cboMonth_SelectedIndexChanged
    '   名称　：月コンボボックスの選択値変更
    '   概要  ：
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cboMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboMonth.SelectedIndexChanged
        '検索結果のクリア
        ClearResult()
    End Sub

End Class
#End Region
