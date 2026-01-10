Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDCommon
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.MDNameStrikeCommon

Public Class UC040201

#Region "定数・変数"
    ' 画面関連
    Private Const SCREEN_ID As String = SCREEN_ID_UC040201          ' UC040201
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040201      ' 争議行為画面
    ' 専従職員フラグ
    Private blnSenjyuFlg As Boolean = False
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    ' ステータス
    Private Const STATUS_INSERT As Byte = 0                             ' 新規登録
    Private Const STATUS_DETAIL As Byte = 1                             ' 内容表示Screen
    Private Const STATUS_UPDATE As Byte = 2                             ' 内容変更Table
    Private Const STATUS_DELETE As Byte = 3                             ' 内容削除Table
    Private Const STATUS_TRANSIT As Byte = 4                            ' 内容変換（一時->本番）Table
    Private Const STATUS_EDIT As Byte = 9                               ' 内容変更Screen
    Private Const STATUS_TAB_HONB As Byte = 1                           ' 本番登録
    Private Const STATUS_TAB_TEMP As Byte = 2                           ' 一時保存登録
    Private Const SEPALATE_CHAR As String = "-"
#End Region

#Region "プロパティ"
    Public _blnSearchFlg As Boolean = False             ' 再検索フラグ（True：再検索有り, False:再検索無し）
    ' ステータス
    Public Property blnSearchFlg() As Boolean
        Get
            Return _blnSearchFlg
        End Get
        Set(ByVal value As Boolean)
            _blnSearchFlg = value
        End Set
    End Property
#End Region

#Region "イベント"
    Private Sub UC040201_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim intTabIndex As Integer = 1
        '-------------------------------------------------------------------------------
        '   各データ取得処理
        '-------------------------------------------------------------------------------
        Me.GroupBox6.Visible = False
        If GetData() = False Then
            Exit Sub
        End If
    End Sub
    Private Sub UC040201_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.VisibleChanged
        Try
            ' フォーム表示で再検索フラグがTrueの場合、再検索処理を行う。
            If Me.Visible And Me.blnSearchFlg Then
                ' 再検索処理
                Call btnSearch_Click(sender, e)
                ' 再検索処理（一時保存）
                Call btnSearchTmp_Click(sender, e)
                blnSearchFlg = False
            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub btnNewTukoku_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewTukoku.Click
        Dim pn As Panel
        Dim clsUC040202 As UC040202
        Try
            Me.Visible = False
            pn = ParentForm.Controls(MAIN_PANEL_ID)
            clsUC040202 = pn.Controls(SCREEN_ID_UC040202)
            clsUC040202 = New UC040202
            ' 画面間パラメータ情報設定
            ' ステータス
            clsUC040202.bytStatus = STATUS_INSERT                                         ' ステータス（0：登録）
            clsUC040202.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
            Call pn.Controls.Add(clsUC040202)                                             ' パネルに争議行為詳細画面を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
        End Try
    End Sub

    Private Sub cmbYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbYear.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchData() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True

        End If

    End Sub
    Private Sub cmbYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbYear.SelectedIndexChanged
        Try
            '-------------------------------------------------------------------------------
            '   申請日付コンボボックスMM作成処理
            '-------------------------------------------------------------------------------
            If Me.cmbYear.SelectedIndex > 0 Then
                If CreateComboBoxMM(Me.cmbMonth) = False Then
                    Exit Sub
                End If
            Else
                Me.cmbMonth.DataSource = Nothing
                Me.cmbDay.DataSource = Nothing
                Exit Sub
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
    End Sub

    Private Sub cmbMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMonth.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchData() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True

        End If

    End Sub
    Private Sub cmbMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMonth.SelectedIndexChanged
        Try
            '-------------------------------------------------------------------------------
            '   申請日付コンボボックスDD作成処理
            '-------------------------------------------------------------------------------
            If Me.cmbYear.SelectedIndex > 0 Then
                If Me.cmbMonth.SelectedIndex > 0 Then
                    If CreateComboBoxDD(Me.cmbDay, CInt(Me.cmbYear.SelectedValue.ToString), CInt(Me.cmbMonth.SelectedValue.ToString), True) = False Then
                        Exit Sub
                    End If
                Else
                    Me.cmbDay.DataSource = Nothing
                End If
            Else
                Me.cmbMonth.DataSource = Nothing
                Me.cmbDay.DataSource = Nothing
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
    End Sub

    Private Sub cmbYearTmp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbYearTmp.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewTmpIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchDataTmp() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True
        End If

    End Sub
    Private Sub cmbYearTmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbYearTmp.SelectedIndexChanged
        Try
            '-------------------------------------------------------------------------------
            '   申請日付コンボボックスMM作成処理
            '-------------------------------------------------------------------------------
            If Me.cmbYearTmp.SelectedIndex > 0 Then
                If CreateComboBoxMM(Me.cmbMonthTmp) = False Then
                    Exit Sub
                End If
            Else
                Me.cmbMonthTmp.DataSource = Nothing
                Me.cmbDayTmp.DataSource = Nothing
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try

    End Sub

    Private Sub cmbMonthTmp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMonthTmp.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewTmpIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchDataTmp() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True
        End If

    End Sub
    Private Sub cmbMonthTmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbMonthTmp.SelectedIndexChanged
        Try
            '-------------------------------------------------------------------------------
            '   申請日付コンボボックスDD作成処理
            '-------------------------------------------------------------------------------
            If Me.cmbYearTmp.SelectedIndex > 0 Then
                If Me.cmbMonthTmp.SelectedIndex > 0 Then
                    If CreateComboBoxDD(Me.cmbDayTmp, CInt(Me.cmbYearTmp.SelectedValue.ToString), CInt(Me.cmbMonthTmp.SelectedValue.ToString), True) = False Then
                        Exit Sub
                    End If
                Else
                    Me.cmbDayTmp.DataSource = Nothing
                End If
            Else
                Me.cmbMonthTmp.DataSource = Nothing
                Me.cmbDayTmp.DataSource = Nothing
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try

    End Sub

    '************************************************************************************
    '   ＩＤ　：btnSearch_Click
    '   名称　：争議行為文書検索タブ内の検索ボタン
    '   概要　：争議行為文書検索処理
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '   履歴　：2012/01/17(火) Kim  修正
    '************************************************************************************
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click, chkSyuketu.Click
        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソルを砂時計に設定
            ' グリッド初期化
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchData() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True
        Catch ex As Exception
        End Try
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnSearchTmp_Click
    '   名称　：一時保存文書検索タブ内の検索ボタン
    '   概要　：一時保存文書検索処理
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '************************************************************************************
    Private Sub btnSearchTmp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearchTmp.Click
        Try
            Cursor.Current = Cursors.WaitCursor                                             ' カーソルを砂時計に設定
            ' グリッド初期化
            If DataGridViewTmpIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchDataTmp() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True
        Catch ex As Exception
        End Try
    End Sub

    '************************************************************************************
    '   ＩＤ　：btnDetail_Click
    '   名称　：詳細
    '   概要　：詳細ボタン
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '   履歴　：2012/01/19(火) Kim  修正
    '************************************************************************************
    Private Sub btnDetail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetail.Click, dgvResult.CellDoubleClick
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)                           ' パネルオブジェクト
        Dim clsUC040202 As UC040202
        Dim clsUC040203 As UC040203
        Dim clsUC040204 As UC040204
        Dim clsUC040205 As UC040205
        Dim clsUC040206 As UC040206
        Try
            Me.Visible = False
            If Me.dgvResult.SelectedRows.Count < 0 Then                                     ' 選択されているかチェック
                Call CLMsg.Show("GW0001", "データ")                                         ' 未選択の場合、エラーメッセージ表示
                Exit Sub
            Else
                ' 画面間パラメータ情報設定
                Select Case Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString
                    Case "01"                                                                         '争議行為通告
                        clsUC040202 = New UC040202
                        clsUC040202.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040202.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040202.strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040202.strKind = Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString()
                        clsUC040202.strBasisStrikeID = Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString()
                        Call pnl.Controls.Add(clsUC040202)
                    Case "07"                                                                         '労働協約第47条申し入れ
                        clsUC040203 = New UC040203
                        clsUC040203.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040203.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040203.strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040203.strKind = Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString()
                        clsUC040203.strBasisStrikeID = Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString()
                        Call pnl.Controls.Add(clsUC040203)                                             ' パネルに争議行為詳細画面を設定
                    Case "02"
                        clsUC040204 = New UC040204                                                    '細部通告
                        clsUC040204.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040204.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040204.strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040204.strKind = Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString()
                        clsUC040204.strBasisStrikeID = Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString()
                        Call pnl.Controls.Add(clsUC040204)                                             ' パネルに争議行為詳細画面を設定
                    Case "03", "04", "05"
                        clsUC040205 = New UC040205
                        clsUC040205.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040205.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040205.strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040205.strKind = Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString()
                        clsUC040205.strBasisStrikeID = Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString()
                        Call pnl.Controls.Add(clsUC040205)                                             ' パネルに争議行為詳細画面を設定
                    Case "06"
                        clsUC040206 = New UC040206
                        clsUC040206.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040206.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040206.strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040206.strKind = Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString()
                        clsUC040206.strBasisStrikeID = Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString()
                        Call pnl.Controls.Add(clsUC040206)                                             ' パネルに争議行為詳細画面を設定
                    Case Else
                        Call CLMsg.Show("GE0004", "Source btnDetail_Click")
                        Exit Sub
                End Select
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
        End Try
    End Sub
    '************************************************************************************
    '   ＩＤ　：btnDetailTmp_Click
    '   名称　：詳細
    '   概要　：詳細ボタン
    '   作成日：2012/01/23 Kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/23 Kim  新規作成
    '************************************************************************************
    Private Sub btnDetailTmp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDetailTmp.Click, dgvResultTmp.CellDoubleClick
        Dim pnl As Panel = ParentForm.Controls(MAIN_PANEL_ID)                           ' パネルオブジェクト
        Dim clsUC040202 As UC040202
        Dim clsUC040203 As UC040203
        Dim clsUC040204 As UC040204
        Dim clsUC040205 As UC040205
        Dim clsUC040206 As UC040206
        Try
            Me.Visible = False
            If Me.dgvResultTmp.SelectedRows.Count < 0 Then                                     ' 選択されているかチェック
                Call CLMsg.Show("GW0001", "データ")                                         ' 未選択の場合、エラーメッセージ表示
                Exit Sub
            Else
                ' 画面間パラメータ情報設定
                Select Case Me.dgvResultTmp.CurrentRow.Cells.Item(6).Value.ToString

                    Case "01"                                                                         '争議行為通告
                        clsUC040202 = New UC040202
                        clsUC040202.bytTabKind = STATUS_TAB_TEMP                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040202.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040202.strStrikeId = Me.dgvResultTmp.CurrentRow.Cells.Item(5).Value.ToString()
                        clsUC040202.strKind = Me.dgvResultTmp.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040202.strBasisStrikeID = Me.dgvResultTmp.CurrentRow.Cells.Item(7).Value.ToString()
                        Call pnl.Controls.Add(clsUC040202)
                    Case "07"                                                                         '労働協約第47条申し入れ
                        clsUC040203 = New UC040203
                        clsUC040203.bytTabKind = STATUS_TAB_TEMP                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040203.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040203.strStrikeId = Me.dgvResultTmp.CurrentRow.Cells.Item(5).Value.ToString()
                        clsUC040203.strKind = Me.dgvResultTmp.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040203.strBasisStrikeID = Me.dgvResultTmp.CurrentRow.Cells.Item(7).Value.ToString()
                        Call pnl.Controls.Add(clsUC040203)                                             ' パネルに争議行為詳細画面を設定
                    Case "02"
                        clsUC040204 = New UC040204                                                    '細部通告
                        clsUC040204.bytTabKind = STATUS_TAB_TEMP                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040204.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040204.strStrikeId = Me.dgvResultTmp.CurrentRow.Cells.Item(5).Value.ToString()
                        clsUC040204.strKind = Me.dgvResultTmp.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040204.strBasisStrikeID = Me.dgvResultTmp.CurrentRow.Cells.Item(7).Value.ToString()
                        Call pnl.Controls.Add(clsUC040204)                                             ' パネルに争議行為詳細画面を設定
                    Case "03", "04", "05"
                        clsUC040205 = New UC040205
                        clsUC040205.bytTabKind = STATUS_TAB_TEMP                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040205.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040205.strStrikeId = Me.dgvResultTmp.CurrentRow.Cells.Item(5).Value.ToString()
                        clsUC040205.strKind = Me.dgvResultTmp.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040205.strBasisStrikeID = Me.dgvResultTmp.CurrentRow.Cells.Item(7).Value.ToString()
                        Call pnl.Controls.Add(clsUC040205)                                             ' パネルに争議行為詳細画面を設定
                    Case "06"
                        clsUC040206 = New UC040206
                        clsUC040206.bytTabKind = STATUS_TAB_TEMP                                      ' Tab Kind （1：本登録, 2：一時保管）
                        clsUC040206.bytStatus = STATUS_DETAIL                                         ' ステータス（1：詳細）
                        clsUC040206.strStrikeId = Me.dgvResultTmp.CurrentRow.Cells.Item(5).Value.ToString()
                        clsUC040206.strKind = Me.dgvResultTmp.CurrentRow.Cells.Item(6).Value.ToString()
                        clsUC040206.strBasisStrikeID = Me.dgvResultTmp.CurrentRow.Cells.Item(7).Value.ToString()
                        Call pnl.Controls.Add(clsUC040206)                                             ' パネルに争議行為詳細画面を設定
                    Case Else
                        Call CLMsg.Show("GE0004", "Source btnDetailTmp_Click")
                        Exit Sub
                End Select
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
        End Try
    End Sub
    Private Sub txtNo1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNo1.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
    End Sub
    Private Sub txtNo2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNo2.KeyDown
        Try
            If e.KeyCode = Keys.Enter Then
                Me.SelectNextControl(Me.ActiveControl, True, True, True, True)
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
    End Sub
    '************************************************************************************
    '   ＩＤ　：btnNewMosiire_Click
    '   名称　：新規申し入れ
    '   概要　：新規申し入れボタン
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '************************************************************************************
    Private Sub btnNewMosiire_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewMosiire.Click
        Dim pn As Panel
        Dim clsUC040203 As UC040203
        Try
            Me.Visible = False
            pn = ParentForm.Controls(MAIN_PANEL_ID)
            clsUC040203 = pn.Controls(SCREEN_ID_UC040203)
            clsUC040203 = New UC040203
            ' 画面間パラメータ情報設定
            ' ステータス
            clsUC040203.bytStatus = STATUS_INSERT                                         ' ステータス（0：登録）
            Call pn.Controls.Add(clsUC040203)                                             ' パネルに争議行為詳細画面を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
        End Try
    End Sub
    '************************************************************************************
    '   ＩＤ　：btnSaibu_Click
    '   名称　：争議行為細部通告登録ボタン
    '   概要　：争議行為細部通告登録
    '   作成日：2012/02/03 Kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/02/03 Kim  新規作成
    '************************************************************************************
    Private Sub btnSaibu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaibu.Click
        Dim pn As Panel
        Dim clsUC040204 As UC040204
        Dim strStrikeId As String
        Try
            If (Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString() <> "01") Then
                CLMsg.Show("GI0013", "細部通告登録")
                Return
            End If
            strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
            If SyuketuExist(strStrikeId) Then
                CLMsg.Show("GI0014")
                Return
            End If
            Me.Visible = False
            pn = ParentForm.Controls(MAIN_PANEL_ID)
            clsUC040204 = pn.Controls(SCREEN_ID_UC040204)
            clsUC040204 = New UC040204
            ' 画面間パラメータ情報設定
            clsUC040204.strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
            clsUC040204.strKind = Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString()
            clsUC040204.strBasisStrikeID = Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString()
            ' ステータス
            clsUC040204.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
            clsUC040204.bytStatus = STATUS_INSERT                                         ' ステータス（0：登録）
            Call pn.Controls.Add(clsUC040204)                                             ' パネルに争議行為詳細画面を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
        End Try
    End Sub
    '************************************************************************************
    '   ＩＤ　：btnRefresh_Click
    '   名称　：争議行為解除登録ボタン
    '   概要　：争議行為解除登録
    '   作成日：2012/02/03 Kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/02/03 Kim  新規作成
    '************************************************************************************
    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Dim pn As Panel
        Dim clsUC040205 As UC040205
        Dim strStrikeId As String
        Try
            If (Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString() <> "01") Then
                CLMsg.Show("GI0013", "解除登録")
                Return
            End If
            If (Me.cmbSyuketuKind.SelectedIndex < 1) Then
                CLMsg.Show("GI0007")
                Return
            End If
            strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
            If SyuketuExist(strStrikeId) Then
                CLMsg.Show("GI0014")
                Return
            End If
            Me.Visible = False
            pn = ParentForm.Controls(MAIN_PANEL_ID)
            clsUC040205 = pn.Controls(SCREEN_ID_UC040205)
            clsUC040205 = New UC040205
            ' 画面間パラメータ情報設定
            clsUC040205.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
            clsUC040205.bytStatus = STATUS_INSERT                                         ' ステータス（1：詳細）
            clsUC040205.strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
            clsUC040205.strKind = Me.cmbSyuketuKind.SelectedValue
            clsUC040205.strBasisStrikeID = Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString()
            Call pn.Controls.Add(clsUC040205)                                             ' パネルに争議行為詳細画面を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
        End Try
    End Sub
    '************************************************************************************
    '   ＩＤ　：btnDeleteTmp_Click
    '   名称　：一時保存文書の削除
    '   概要　：一時保存文書の削除ボタン
    '   作成日：2012/01/31(火) Kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/31 Kim  新規作成
    '************************************************************************************
    Private Sub btnDeleteTmp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteTmp.Click
        Dim strSql As String                                                                    ' SQL文
        Dim strID As String
        Dim clsDb As New CLAccessMdb                                                      ' データベースクラス生成
        Dim tbRet As DataTable = Nothing                                                        ' 処理結果格納データテーブル
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")             ' ログ出力（処理開始）
        Try
            If CLMsg.Show("GQ0011") = DialogResult.No Then                                      ' データを削除してもよろしいですかメッセージボックス表示
                Exit Sub                                                                        ' 「いいえ」ボタン押下時、処理を抜ける
            Else
                Call clsDb.Connect()                                                                             ' データベース接続
                Call clsDb.BeginTran()                                                          ' トランザクション開始処理
                Cursor.Current = Cursors.WaitCursor                                             ' カーソルを砂時計に設定
                strID = Me.dgvResultTmp.CurrentRow.Cells.Item(5).Value.ToString()
                '-------------------------------------------------------------------
                '   SQL作成
                '-------------------------------------------------------------------
                strSql = ""
                strSql = strSql & "delete from strike_work_list "
                strSql = strSql & "where  c_strike_work_id = '" + strID + "';"
                clsDb.ExecuteSql(strSql)                                                                    ' SQL実行
                strSql = ""
                strSql = strSql & "delete from strike_work_member_list "
                strSql = strSql & "where  c_strike_work_id = '" + strID + "';"
                clsDb.ExecuteSql(strSql)                                                                    ' SQL実行
                Call clsDb.CommitTran()                                                         ' トランザクション確定処理
                Call clsDb.Disconnect()

                Me.btnSearchTmp_Click(sender, e)
            End If
        Catch ex As Exception
            Call clsDb.RollbackTran()                                                         ' トランザクションRollBack処理
            Call clsDb.Disconnect()
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                                       ' ログ出力（致命的エラー）
        Finally
            clsDb.Disconnect()                                                                          ' データベース切断
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")                     ' ログ出力（処理終了）
    End Sub
    Private Sub btnSyuketu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSyuketu.Click
        Dim pn As Panel
        Dim clsUC040206 As UC040206
        Dim strStrikeId As String
        Try
            If (Me.dgvResult.CurrentRow.Cells.Item(7).Value.ToString() <> "01") Then
                CLMsg.Show("GI0013", "終結登録")
                Return
            End If
            strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
            If SyuketuExist(strStrikeId) Then
                CLMsg.Show("GI0014")
                Return
            End If
            Me.Visible = False
            pn = ParentForm.Controls(MAIN_PANEL_ID)
            clsUC040206 = pn.Controls(SCREEN_ID_UC040206)
            clsUC040206 = New UC040206
            ' 画面間パラメータ情報設定
            clsUC040206.strStrikeId = Me.dgvResult.CurrentRow.Cells.Item(6).Value.ToString()
            clsUC040206.strBasisStrikeID = Me.dgvResult.CurrentRow.Cells.Item(8).Value.ToString()
            ' ステータス
            clsUC040206.bytTabKind = STATUS_TAB_HONB                                      ' Tab Kind （1：本登録, 2：一時保管）
            clsUC040206.bytStatus = STATUS_INSERT                                         ' ステータス（0：登録）
            Call pn.Controls.Add(clsUC040206)                                             ' パネルに争議行為詳細画面を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
        End Try
    End Sub
#End Region

#Region "関数"
    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：申請日付、分類のコンボボックス作成処理を呼び出す。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/13(火) kim
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(火) kim  新規作成
    '***************************************************************************************************
    Private Function GetData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス生成
        Try
            Call clsDb.Connect()
            '-------------------------------------------------------------------------------
            '   申請日付コンボボックスYYYY作成処理
            '-------------------------------------------------------------------------------
            If CreateCboTermYYYY(clsDb) = False Then
                'Return False
            End If
            If CreateCboTermTmpYYYY(clsDb) = False Then
                'Return False
            End If
            '-------------------------------------------------------------------------------
            '   分類コンボボックス作成処理
            '-------------------------------------------------------------------------------
            If CreateCboSougiKind(clsDb) = False Then
                Return False
            End If
            Me.chkSyuketu.Checked = True
            Me.btnDetail.Enabled = False
            Me.btnSaibu.Enabled = False
            Me.btnSyuketu.Enabled = False
            Me.btnRefresh.Enabled = False
            Me.cmbSyuketuKind.Enabled = False
            blnRet = True                                                                   ' 処理結果に正常を格納
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
            Call clsDb.Disconnect()
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function
    '***************************************************************************************************
    '   ＩＤ　：CreateCboTermYYYY
    '   名称　：申請日付コンボボックス作成処理
    '   概要  ：申請日付コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/13(火) kim
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(火) kim  新規作成
    '***************************************************************************************************
    Private Function CreateCboTermYYYY(ByVal clsDb As CLAccessMdb) As Boolean
        Dim dtRet As DataTable = Nothing                                                    ' 処理結果データテーブル
        Dim strSql As String = ""                                                           ' SQL文
        Dim strNowDate As String = ""                                                       ' 現在日付
        Dim intRetCnt As Integer = Nothing                                                  ' 処理結果件数
        Dim strYyyy As String = Nothing                                                      ' 現在日付（yyyyMMdd）
        Dim intIndex As Integer = Nothing                                                   ' 選択されたコンボボックスリストインデックス
        Try
            Me.cmbYear.DataSource = Nothing                                                 ' コンボボックスクリア
            ' SQL文
            strSql = ""
            strSql = strSql & " select distinct Mid(d_strike,1,4) as yyyy"
            strSql = strSql & " from strike_list"
            strSql = strSql & " order by 1;"
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, Me.cmbYear, strSql, "yyyy", "yyyy") = False Then
                Return False
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
        Return True                                                                       ' 戻り値格納
    End Function
    '***************************************************************************************************
    '   ＩＤ　：CreateCboTermYYYY
    '   名称　：申請日付コンボボックス作成処理
    '   概要  ：申請日付コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/13(火) kim
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(火) kim  新規作成
    '***************************************************************************************************
    Private Function CreateCboTermTmpYYYY(ByVal clsDb As CLAccessMdb) As Boolean
        Dim dtRet As DataTable = Nothing                                                    ' 処理結果データテーブル
        Dim strSql As String = ""                                                           ' SQL文
        Dim strNowDate As String = ""                                                       ' 現在日付
        Dim intRetCnt As Integer = Nothing                                                  ' 処理結果件数
        Dim strYyyy As String = Nothing                                                      ' 現在日付（yyyyMMdd）
        Dim intIndex As Integer = Nothing                                                   ' 選択されたコンボボックスリストインデックス
        Try
            Me.cmbYearTmp.DataSource = Nothing                                              ' コンボボックスクリア
            ' SQL文
            strSql = ""
            strSql = strSql & " select distinct Mid(d_strike,1,4) as yyyy"
            strSql = strSql & " from strike_work_list"
            strSql = strSql & " order by 1;"
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, Me.cmbYearTmp, strSql, "yyyy", "yyyy") = False Then
                Return False
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
        Return True                                                                       ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：CreateCboSougiKind
    '   名称　：申請日付コンボボックス作成処理
    '   概要  ：申請日付コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/13(火) kim
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(火) kim  新規作成
    '***************************************************************************************************
    Private Function CreateCboSougiKind(ByVal clsDb As CLAccessMdb) As Boolean
        Dim dtRet As DataTable = Nothing                                                    ' 処理結果データテーブル
        Dim strSql As String = ""                                                           ' SQL文
        Dim strNowDate As String = ""                                                       ' 現在日付
        Dim intRetCnt As Integer = Nothing                                                  ' 処理結果件数
        Dim strYyyy As String = Nothing                                                      ' 現在日付（yyyyMMdd）
        Dim intIndex As Integer = Nothing                                                   ' 選択されたコンボボックスリストインデックス
        Try
            Me.cmbKind.DataSource = Nothing                                                 ' コンボボックスクリア
            ' SQL文
            strSql = ""
            strSql = strSql & " select c_constant_seq,l_name"
            strSql = strSql & " from   constant_dtl"
            strSql = strSql & " where  c_constant  =  'SOUGI_KIND'"
            strSql = strSql & " order by 1;"
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, Me.cmbKind, strSql, "l_name", "c_constant_seq") = False Then
                Return False
            End If
            If MDCommon.CreateComboBoxNew(clsDb, Me.cmbKindTmp, strSql, "l_name", "c_constant_seq") = False Then
                Return False
            End If

            Me.cmbSyuketuKind.DataSource = Nothing                                                 ' コンボボックスクリア
            ' SQL文
            strSql = ""
            strSql = strSql & " select c_constant_seq,l_name"
            strSql = strSql & " from   constant_dtl"
            strSql = strSql & " where  c_constant  =  'SOUGI_KIND'"
            strSql = strSql & " and    c_constant_seq  >=  '03'"
            strSql = strSql & " and    c_constant_seq  <=  '05'"
            strSql = strSql & " order by 1;"
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(clsDb, Me.cmbSyuketuKind, strSql, "l_name", "c_constant_seq") = False Then
                Return False
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
        Return True                                                                       ' 戻り値格納
    End Function
    '***************************************************************************************************
    '   ＩＤ　：DataGridViewIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function DataGridViewIni() As Boolean
        Dim blnRet As Boolean = False   ' 処理結果
        Try
            '-----------------------------------------------------------------------------------
            '   グリッド全体設定
            '-----------------------------------------------------------------------------------
            ' 総数
            Me.dgvResult.RowCount = 0                                                           ' 縦
            Me.dgvResult.ColumnCount = 9                                                        ' 横
            ' 固定行
            'Me.cfgResult.Rows.Fixed = FLEXGRID_ROWS_FIXED                                       ' 縦
            'Me.cfgResult.Cols.Fixed = FLEXGRID_COLS_FIXED                                       ' 横
            ' スクロールバー
            'Me.dgvResult.ScrollBars = ScrollBars.Vertical                                       ' 縦のみ
            Me.dgvResult.ScrollBars = ScrollBars.Both                                           ' 縦横両方
            ' 1行選択モード
            Me.dgvResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect                ' 1行選択
            Me.dgvResult.MultiSelect = False                                                    ' 複数選択なし
            '' サイズ変更
            'Me.dgvResult.AllowUserToResizeColumns = True                                        ' 列サイズ変更可
            'Me.dgvResult.AllowUserToResizeRows = True                                           ' 行サイズ変更不可
            ' バックカラー
            'Me.dgvResult.RowsDefaultCellStyle.BackColor = Color.White                           ' 全ての列の背景色を白色
            Me.dgvResult.RowsDefaultCellStyle.ForeColor = Color.Black
            ' 改行表示対応
            'Me.dgvResult.Columns(3).DefaultCellStyle.WrapMode = DataGridViewTriState.True
            'Me.dgvResult.Columns(3).DefaultCellStyle.WrapMode = DataGridViewTriState.NotSet

            '-----------------------------------------------------------------------------------
            '   ヘッダー部設定
            '-----------------------------------------------------------------------------------
            ' ヘッダー文字列
            Me.dgvResult.Columns(0).HeaderText = "争議番号"
            Me.dgvResult.Columns(1).HeaderText = "分類"
            Me.dgvResult.Columns(2).HeaderText = "申請日付"
            Me.dgvResult.Columns(3).HeaderText = "事件"
            Me.dgvResult.Columns(4).HeaderText = "元争議番号"
            Me.dgvResult.Columns(5).HeaderText = "担当者"
            Me.dgvResult.Columns(6).HeaderText = ""
            ' ヘッダー文字位置
            Me.dgvResult.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResult.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResult.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResult.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResult.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResult.Columns(5).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResult.Columns(6).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            '-----------------------------------------------------------------------------------
            '   カラム部設定
            '-----------------------------------------------------------------------------------
            ' カラム文字位置
            Me.dgvResult.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResult.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResult.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResult.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResult.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResult.Columns(5).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResult.Columns(6).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            ' カラム幅
            Me.dgvResult.Columns(0).Width = 100
            Me.dgvResult.Columns(1).Width = 190
            Me.dgvResult.Columns(2).Width = 100
            Me.dgvResult.Columns(3).Width = 330
            Me.dgvResult.Columns(4).Width = 120
            Me.dgvResult.Columns(5).Width = 100
            ' カラム表示有無
            Me.dgvResult.Columns(0).Visible = True
            Me.dgvResult.Columns(1).Visible = True
            Me.dgvResult.Columns(2).Visible = True
            Me.dgvResult.Columns(3).Visible = True
            Me.dgvResult.Columns(4).Visible = True
            Me.dgvResult.Columns(5).Visible = True
            blnRet = True                                                               ' 戻り値格納
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
        End Try
        Return blnRet                                                                   ' 戻り値格納
    End Function
    '***************************************************************************************************
    '   ＩＤ　：DataGridViewTmpIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04(金) m.suzuki  新規作成
    '***************************************************************************************************
    Private Function DataGridViewTmpIni() As Boolean
        Dim blnRet As Boolean = False   ' 処理結果
        Try
            '-----------------------------------------------------------------------------------
            '   グリッド全体設定
            '-----------------------------------------------------------------------------------
            ' 総数
            Me.dgvResultTmp.RowCount = 0                                                           ' 縦
            Me.dgvResultTmp.ColumnCount = 8                                                        ' 横
            ' 固定行
            'Me.cfgResult.Rows.Fixed = FLEXGRID_ROWS_FIXED                                      ' 縦
            'Me.cfgResult.Cols.Fixed = FLEXGRID_COLS_FIXED                                      ' 横
            ' スクロールバー 
            'Me.dgvResultTmp.ScrollBars = ScrollBars.Vertical                                         ' 縦のみ
            Me.dgvResultTmp.ScrollBars = ScrollBars.Both                                           ' 縦横両方
            ' 1行選択モード
            Me.dgvResultTmp.SelectionMode = DataGridViewSelectionMode.FullRowSelect                ' 1行選択
            Me.dgvResultTmp.MultiSelect = False                                                    ' 複数選択なし
            ' サイズ変更
            Me.dgvResultTmp.AllowUserToResizeColumns = True                                        ' 列サイズ変更可
            Me.dgvResultTmp.AllowUserToResizeRows = False                                          ' 行サイズ変更不可
            ' バックカラー
            'Me.dgvResultTmp.RowsDefaultCellStyle.BackColor = Color.White                           ' 全ての列の背景色を白色
            Me.dgvResultTmp.RowsDefaultCellStyle.ForeColor = Color.Black
            ' 改行表示対応
            Me.dgvResultTmp.Columns(2).DefaultCellStyle.WrapMode = DataGridViewTriState.True
            '-----------------------------------------------------------------------------------
            '   ヘッダー部設定
            '-----------------------------------------------------------------------------------
            ' ヘッダー文字列
            Me.dgvResultTmp.Columns(0).HeaderText = "分類"
            Me.dgvResultTmp.Columns(1).HeaderText = "登録日付"
            Me.dgvResultTmp.Columns(2).HeaderText = "件名"
            Me.dgvResultTmp.Columns(3).HeaderText = "元争議番号"
            Me.dgvResultTmp.Columns(4).HeaderText = "担当者"
            ' ヘッダー文字位置
            Me.dgvResultTmp.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResultTmp.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResultTmp.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResultTmp.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Me.dgvResultTmp.Columns(4).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            '-----------------------------------------------------------------------------------
            '   カラム部設定
            '-----------------------------------------------------------------------------------
            ' カラム文字位置
            Me.dgvResultTmp.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResultTmp.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResultTmp.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResultTmp.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            Me.dgvResultTmp.Columns(4).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            ' カラム幅
            Me.dgvResultTmp.Columns(0).Width = 150
            Me.dgvResultTmp.Columns(1).Width = 100
            Me.dgvResultTmp.Columns(2).Width = 320
            Me.dgvResultTmp.Columns(3).Width = 150
            Me.dgvResultTmp.Columns(4).Width = 100
            ' カラム表示有無
            Me.dgvResultTmp.Columns(0).Visible = True
            Me.dgvResultTmp.Columns(1).Visible = True
            Me.dgvResultTmp.Columns(2).Visible = True
            Me.dgvResultTmp.Columns(3).Visible = True
            Me.dgvResultTmp.Columns(4).Visible = True
            blnRet = True                                                               ' 戻り値格納
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
        End Try
        Return blnRet                                                                   ' 戻り値格納
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetSearchData
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04 m.suzuki  新規作成
    '   履歴　：2012/01/17 kim  変更
    '***************************************************************************************************
    Private Function GetSearchData() As Boolean
        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String                        ' SQL文
        Dim clsDb As New CLAccessMdb                ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数
        Dim strYYYYMMDD As String
        Dim strTxtNo1 As String
        Dim strTxtNo2 As String
        Dim strStrike As String
        Try
            Call clsDb.Connect()                                                            ' データベース接続
            ' 各情報取得
            If (Me.cmbYear.SelectedIndex > 0) Then
                strYYYYMMDD = Me.cmbYear.SelectedValue
                If (Me.cmbMonth.SelectedIndex > 0) Then
                    strYYYYMMDD = strYYYYMMDD & Me.cmbMonth.Text
                Else
                    strYYYYMMDD = strYYYYMMDD & "01"
                End If
                If (Me.cmbDay.SelectedIndex > 0) Then
                    strYYYYMMDD = strYYYYMMDD & Me.cmbDay.Text
                Else
                    strYYYYMMDD = strYYYYMMDD & "01"
                End If
            Else
                strYYYYMMDD = ""
            End If

            strTxtNo1 = Me.txtNo1.Text
            strTxtNo2 = Me.txtNo2.Text
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = "select stli.c_strike_info,stli.k_strike_kind,cndt.l_name as k_strike_kind_name,"
            strSql = strSql & "       stli.d_strike,stli.l_event,stli.l_subject,stli.l_text,stli.c_basis_strike_id,"
            strSql = strSql & "       stli.c_user_id_ins,staf.l_name as c_user_id_ins_name,stli.c_strike_id "
            strSql = strSql & "from   strike_list  stli, "
            strSql = strSql & "       staf_attribute_full_time_now_name_view staf, "
            strSql = strSql & "       constant_dtl cndt "
            strSql = strSql & "where  stli.k_strike_kind = cndt.c_constant_seq "
            strSql = strSql & "and    cndt.c_constant    = 'SOUGI_KIND' "
            strSql = strSql & "and    stli.c_user_id_ins = staf.user_id "
            If (Me.chkSyuketu.Checked = True) Then
                strSql = strSql & "and    not exists(SELECT stli1.c_basis_strike_id "
                strSql = strSql & "                  FROM   strike_list AS stli1 "
                strSql = strSql & "                  WHERE  stli1.k_strike_kind = '06' "
                strSql = strSql & "                  AND    stli.c_strike_id = stli1.c_basis_strike_id) "
                strSql = strSql & "and    not exists(SELECT stli2.c_basis_strike_id "
                strSql = strSql & "                  FROM   strike_list AS stli2 "
                strSql = strSql & "                  WHERE  stli2.k_strike_kind = '06' "
                strSql = strSql & "                  AND    stli.c_basis_strike_id = stli2.c_basis_strike_id ) "
            End If
            If (strYYYYMMDD.Length > 0) Then
                strSql = strSql & "  and stli.d_strike >= '" + strYYYYMMDD + "'"
            End If
            If (Me.cmbKind.SelectedIndex > 0) Then
                strSql = strSql & "  and stli.k_strike_kind = '" + Me.cmbKind.SelectedValue + "'"
            End If
            If Not (ChkNull(Me.txtNo1.Text) And ChkNull(Me.txtNo2.Text)) Then
                '通告番号がどちらもあればc_name_strike_idを=で検索
                strSql = strSql & "  and stli.c_strike_id LIKE '" & strTxtNo1 & "%" & SEPALATE_CHAR & strTxtNo2 & "%' "
            ElseIf Not (ChkNull(Me.txtNo1.Text)) Then
                '通告番号の頭のみc_name_strike_idをLIKEで検索
                strSql = strSql & " and stli.c_strike_id LIKE '" & strTxtNo1 & "%' "
            ElseIf Not (ChkNull(Me.txtNo2.Text)) Then
                '通告番号の頭のみc_name_strike_infoをLIKEで検索
                strSql = strSql & " and c_name_strike_info LIKE " & strTxtNo2 & "%' "
            End If

            ' ORDER BY句（社員番号で並替）
            strSql = strSql & "  order by stli.c_period_id,CInt(stli.c_strike_info)" & UtDb.DbOrderOffset & ";" 'ok

            tbRet = clsDb.ExecuteSql(strSql)                                                            ' SQL実行
            intRetCnt = tbRet.Rows.Count                                                                ' 件数取得
            ' 件数チェック
            If intRetCnt > 0 Then
                ' 1件以上の処理
                dgvResult.RowCount = intRetCnt                                                        ' 縦総数設定
                For i = 0 To intRetCnt - 1                                                              ' レコード数分ループ
                    ' データ設定
                    With Me.dgvResult.Rows(i).Cells
                        If (IsDBNull(tbRet.Rows(i).Item("c_strike_id")) Or (tbRet.Rows(i).Item("c_strike_id") = "")) Then
                            .Item(0).Value = "*****"
                        Else
                            .Item(0).Value = tbRet.Rows(i).Item("c_strike_id")
                        End If
                        .Item(1).Value = tbRet.Rows(i).Item("k_strike_kind_name")
                        strStrike = tbRet.Rows(i).Item("d_strike")
                        .Item(2).Value = strStrike.Substring(0, 4) + "/" + strStrike.Substring(4, 2) + "/" + strStrike.Substring(6, 2)
                        If IsDBNull(tbRet.Rows(i).Item("l_event")) Then
                            .Item(3).Value = ""
                        Else
                            Select Case tbRet.Rows(i).Item("k_strike_kind")
                                Case "01"
                                    .Item(3).Value = tbRet.Rows(i).Item("l_event")
                                Case "02", "03", "04", "05", "07"
                                    .Item(3).Value = tbRet.Rows(i).Item("l_subject")
                                Case "06"
                                    .Item(3).Value = tbRet.Rows(i).Item("l_text")
                                Case Else
                                    .Item(3).Value = ""
                            End Select
                        End If
                        If IsDBNull(tbRet.Rows(i).Item("c_basis_strike_id")) Then
                            .Item(4).Value = ""
                        Else
                            .Item(4).Value = tbRet.Rows(i).Item("c_basis_strike_id")
                        End If
                        If IsDBNull(tbRet.Rows(i).Item("c_user_id_ins_name")) Then
                            .Item(5).Value = ""
                        Else
                            .Item(5).Value = tbRet.Rows(i).Item("c_user_id_ins_name")
                        End If
                        .Item(6).Value = tbRet.Rows(i).Item("c_strike_id")
                        .Item(7).Value = tbRet.Rows(i).Item("k_strike_kind")
                        If IsDBNull(tbRet.Rows(i).Item("c_basis_strike_id")) Then
                            .Item(8).Value = ""
                        Else
                            .Item(8).Value = tbRet.Rows(i).Item("c_basis_strike_id")
                        End If
                        dgvResult.Columns(3).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                        Me.dgvResult.Rows(i).Visible = True
                    End With
                Next
                dgvResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
                Me.btnDetail.Enabled = True
                Me.btnSaibu.Enabled = True
                Me.btnSyuketu.Enabled = True
                Me.btnRefresh.Enabled = True
                Me.cmbSyuketuKind.Enabled = True
            Else
                ' 0件の処理
                Me.btnDetail.Enabled = False
                Me.btnSaibu.Enabled = False
                Me.btnSyuketu.Enabled = False
                Me.btnRefresh.Enabled = False
                Me.cmbSyuketuKind.Enabled = False
            End If

            ' グループボックス件数設定
            Me.fraResult.Text = "検索結果（ " + intRetCnt.ToString + " 件）"
            Me.fraResult.Visible = True
            Me.GroupBox6.Visible = True
            blnRet = True                                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                                       ' ログ出力（致命的エラー）
        Finally
            Call clsDb.Disconnect()
        End Try
        Return blnRet                                                                                   ' 戻り値設定
    End Function

    '***************************************************************************************************
    '   ＩＤ　：GetSearchDataTmp
    '   名称　：検索データ取得処理
    '   概要  ：検索データを取得する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/04 m.suzuki  新規作成
    '   履歴　：2012/01/17 kim  変更
    '***************************************************************************************************
    Private Function GetSearchDataTmp() As Boolean
        Dim blnRet As Boolean = False               ' 処理結果
        Dim strSql As String                        ' SQL文
        Dim clsDb As CLAccessMdb = Nothing          ' データベースクラス生成
        Dim tbRet As DataTable = Nothing            ' 処理結果格納データテーブル
        Dim intRetCnt As Integer = 0                ' 検索結果件数
        Dim strYYYYMMDD As String
        Dim strStrike As String
        Try
            clsDb = New CLAccessMdb                                                                     ' データベースクラス生成
            Call clsDb.Connect()                                                                        ' データベース接続
            ' 各情報取得
            If (Me.cmbYearTmp.SelectedIndex > 0) Then
                strYYYYMMDD = Me.cmbYearTmp.SelectedValue
                If (Me.cmbMonthTmp.SelectedIndex > 0) Then
                    strYYYYMMDD = strYYYYMMDD & Me.cmbMonthTmp.Text
                Else
                    strYYYYMMDD = strYYYYMMDD & "01"
                End If
                If (Me.cmbDayTmp.SelectedIndex > 0) Then
                    strYYYYMMDD = strYYYYMMDD & Me.cmbDayTmp.Text
                Else
                    strYYYYMMDD = strYYYYMMDD & "01"
                End If
            Else
                strYYYYMMDD = ""
            End If
            '-------------------------------------------------------------------
            '   SQL作成
            '-------------------------------------------------------------------
            strSql = "select stli.c_strike_work_id,stli.k_strike_kind,cndt.l_name as k_strike_kind_name,"
            strSql = strSql & "       stli.d_strike,stli.l_event,stli.l_subject,stli.l_text,stli.c_basis_strike_id,"
            strSql = strSql & "       stli.c_user_id_ins,staf.l_name as c_user_id_ins_name "
            strSql = strSql & "from   strike_work_list   stli,"
            strSql = strSql & "       period             prod,"
            strSql = strSql & "       staf_attribute_full_time_now_name_view staf, "
            strSql = strSql & "       constant_dtl       cndt "
            strSql = strSql & "where  stli.c_period_id   = prod.c_period_id "
            strSql = strSql & "and    stli.k_strike_kind = cndt.c_constant_seq "
            strSql = strSql & "and    cndt.c_constant    = 'SOUGI_KIND' "
            strSql = strSql & "and    stli.c_user_id_ins = staf.user_id "
            strSql = strSql & "and    not exists(SELECT stli1.c_basis_strike_id "
            strSql = strSql & "                  FROM   strike_list AS stli1 "
            strSql = strSql & "                  WHERE  stli1.k_strike_kind = '06' "
            strSql = strSql & "                  AND    stli.c_basis_strike_id = stli1.c_basis_strike_id) "
            If (strYYYYMMDD.Length > 0) Then
                strSql = strSql & "  and stli.d_strike >= '" + strYYYYMMDD + "' "
            End If
            If (Me.cmbKindTmp.SelectedIndex > 0) Then
                strSql = strSql & "  and stli.k_strike_kind = '" + Me.cmbKindTmp.SelectedValue + "' "
            End If
            strSql = strSql & "  order by CInt(stli.c_strike_work_id) desc" & UtDb.DbOrderOffset & ";"  'ok

            tbRet = clsDb.ExecuteSql(strSql)                                                            ' SQL実行
            intRetCnt = tbRet.Rows.Count                                                                ' 件数取得
            ' 件数チェック
            If intRetCnt > 0 Then
                ' 1件以上の処理
                dgvResultTmp.RowCount = intRetCnt                                                        ' 縦総数設定
                For i = 0 To intRetCnt - 1                                                              ' レコード数分ループ
                    ' データ設定
                    With Me.dgvResultTmp.Rows(i).Cells
                        .Item(0).Value = tbRet.Rows(i).Item("k_strike_kind_name")
                        strStrike = tbRet.Rows(i).Item("d_strike")
                        .Item(1).Value = strStrike.Substring(0, 4) + "/" + strStrike.Substring(4, 2) + "/" + strStrike.Substring(6, 2)
                        If IsDBNull(tbRet.Rows(i).Item("l_event")) Then
                            .Item(2).Value = ""
                        Else
                            Select Case tbRet.Rows(i).Item("k_strike_kind")
                                Case "01"
                                    .Item(2).Value = tbRet.Rows(i).Item("l_event")
                                Case "02", "03", "04", "05", "07"
                                    .Item(2).Value = tbRet.Rows(i).Item("l_subject")
                                Case "06"
                                    .Item(2).Value = tbRet.Rows(i).Item("l_text")
                                Case Else
                                    .Item(2).Value = ""
                            End Select
                        End If
                        If IsDBNull(tbRet.Rows(i).Item("c_basis_strike_id")) Then
                            .Item(3).Value = ""
                        Else
                            .Item(3).Value = tbRet.Rows(i).Item("c_basis_strike_id")
                        End If
                        If IsDBNull(tbRet.Rows(i).Item("c_user_id_ins_name")) Then
                            .Item(4).Value = ""
                        Else
                            .Item(4).Value = tbRet.Rows(i).Item("c_user_id_ins_name")
                        End If
                        .Item(5).Value = tbRet.Rows(i).Item("c_strike_work_id")
                        .Item(6).Value = tbRet.Rows(i).Item("k_strike_kind")
                        .Item(7).Value = tbRet.Rows(i).Item("c_basis_strike_id")
                    End With
                    dgvResult.Columns(3).DefaultCellStyle.WrapMode = DataGridViewTriState.True
                Next
                dgvResult.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders
                Me.btnDetailTmp.Enabled = True
                Me.btnDeleteTmp.Enabled = True
            Else
                ' 0件の処理
                Me.btnDetailTmp.Enabled = False
                Me.btnDeleteTmp.Enabled = False
                'CLMsg.Show("DI0001")                                                                    ' 対象データなしメッセージボックス表示
            End If
            ' グループボックス件数設定
            Me.fraResultTmp.Text = "検索結果（ " + intRetCnt.ToString + " 件）"
            Me.fraResultTmp.Visible = True
            Me.GroupBox6.Visible = True
            blnRet = True                                                                               ' 処理結果に正常を設定

            ' グループボックス件数設定
            Me.fraResultTmp.Text = "検索結果（ " + intRetCnt.ToString + " 件）"
            blnRet = True                                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                                       ' ログ出力（致命的エラー）
        Finally
            clsDb.Disconnect()                                                                          ' データベース切断
        End Try
        Return blnRet                                                                                   ' 戻り値設定
    End Function
    Private Function SyuketuExist(ByVal strStrikeId As String) As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strSql As String = Nothing                                                      ' SQL今期
        Try
            Call clsDb.Connect()
            '本登録テーブルからID取得
            strSql = " select stli.c_strike_id "
            strSql = strSql + " from   strike_list stli, "
            strSql = strSql + "       (select c_strike_id,c_basis_strike_id "
            strSql = strSql + "        from   strike_list "
            strSql = strSql + "        where  k_strike_kind = '06' "
            strSql = strSql + "       ) shuketu "
            strSql = strSql + " where ((stli.c_strike_id = shuketu.c_basis_strike_id)or(stli.c_basis_strike_id = shuketu.c_basis_strike_id)) "
            strSql = strSql + " and   stli.c_strike_id   = '" + strStrikeId + "'; "
            dt = clsDb.ExecuteSql(strSql)
            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040202, SCREEN_ID_UC040202, "SyuketuExist")
            Return False
        Finally
            Call clsDb.Disconnect()
        End Try
    End Function
#End Region

    Private Sub cmbDay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDay.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchData() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True

        End If

    End Sub
    Private Sub cmbKind_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbKind.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchData() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True

        End If

    End Sub

    Private Sub txtNo1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNo1.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchData() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True

        End If

    End Sub

    Private Sub txtNo2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNo2.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then

            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchData() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True

        End If
    End Sub

    Private Sub cmbKindTmp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbKindTmp.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewTmpIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchDataTmp() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True
        End If

    End Sub

    Private Sub cmbDayTmp_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbDayTmp.KeyPress

        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            ' カーソルを砂時計に設定
            Cursor.Current = Cursors.WaitCursor
            ' グリッド初期化
            If DataGridViewTmpIni() = False Then
                Exit Sub
            End If
            ' 検索データ取得処理
            If GetSearchDataTmp() = False Then
                Exit Sub
            End If
            Me.GroupBox6.Visible = True
        End If

    End Sub

End Class
