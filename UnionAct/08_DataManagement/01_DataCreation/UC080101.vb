#Region "UC080101"
'===========================================================================================================
'   クラスＩＤ　　：UC080101
'   クラス名称　　：労金データ作成－検索画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Common

Public Class UC080101

#Region "定数・メンバ変数"
    Private Const SCREEN_ID As String = SCREEN_ID_UC080101
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC080101
    '
    Private Const DAILY_PAY_KIND_COMMITTEE As String = "部／委員会日当"
    Private Const DAILY_PAY_KIND_BRANCH As String = "支部委員会（三役）日当"
    Private Const DAILY_PAY_KIND_EXECUTIVE As String = "中央執行日当"
    Private Const DAILY_PAY_KIND_DGM As String = "ＤＧＭ日当"
    Private Const DAILY_PAY_KIND_CUT As String = "賃金カット（月例）"
    Private Const DAILY_PAY_KIND_ONCE_CUT As String = "賃金カット（一時金）"

    '
    Private Const DAILY_PAY_KIND_COMMITTEE_CODE As String = "01"
    Private Const DAILY_PAY_KIND_BRANCH_CODE As String = "02"
    Private Const DAILY_PAY_KIND_EXECUTIVE_CODE As String = "03"
    Private Const DAILY_PAY_KIND_DGM_CODE As String = "04"
    Private Const DAILY_PAY_KIND_CUT_CODE As String = "05"
    Private Const DAILY_PAY_KIND_ONCE_CUT_CODE As String = "06"

    Private Const SEARCH_RESULT_COUNT As String = "振込一覧（{0}件）"
    Private Const SEARCH_RESULT_COUNT_PERSONAL As String = "個人別振込情報一覧（{0}件）"

    '参照権限
    Private _strGrantReference As String = String.Empty
    '登録権限
    Private _strGrantInsert As String = String.Empty
    '印刷権限
    Private _strGrantPrint As String = String.Empty
    'ファイル出力権限
    Private _strGrantFileOutput As String = String.Empty

    ''' <summary>
    ''' 振込状況
    ''' </summary>
    ''' <remarks></remarks>
    Private _dicBankStatus As Dictionary(Of String, String) = New Dictionary(Of String, String)
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "イベント"

#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：btnMakeDataSearch_Click
    '   名称　：振込状況確認、データ作成検索ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub UC080101_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtGrant As DataTable = Nothing
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        dtGrant = getGrant(MENU_ID_UC080101)
        '権限取得
        If dtGrant.Rows.Count > 0 Then
            _strGrantReference = dtGrant.Rows(0).Item(3).ToString  '参照権限
            _strGrantInsert = dtGrant.Rows(0).Item(4).ToString     '登録権限
            _strGrantPrint = dtGrant.Rows(0).Item(5).ToString      '印刷権限
            _strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString 'ファイル出力権限
        End If

        '登録権限がない場合は新規作成ボタン使用不可
        If _strGrantInsert <> GRANT_VALID Then
            Me.btnMakeData.Enabled = False
        End If
        '振込状況の文字列取得
        Call Me.GetBankStatusString()

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        '各種コンボボックスに年月を設定
        Call Me.SetComboList()
    End Sub
#End Region

#Region "振込状況確認、データ作成検索ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnMakeDataSearch_Click
    '   名称　：振込状況確認、データ作成検索ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnMakeDataSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMakeDataSearch.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        If CheckMakeDataCondition() = False Then
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
            Exit Sub
        End If
        Call Me.SearchMakeData()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "新規作成ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnMakeData_Click
    '   名称　：新規作成ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnMakeData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMakeData.Click
        Dim dtSelect As DataTable = Me.GetSelectCloseDay()
        Dim blnIsFreeEntry As Boolean = False
        Dim pn As Panel
        Dim clsUC080104 As UC080104
        Dim fmNewEntryData As FM080103 = Nothing

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
            If dtSelect.Rows.Count < 1 Then
                '選択行がない場合、処理を続行するかメッセージ表示
                If CLMsg.Show("GQ0065") = DialogResult.No Then
                    Exit Sub
                End If
                blnIsFreeEntry = True
            ElseIf dtSelect.Rows.Count > 10 Then
                '締め日が11個以上チェックされている場合は処理続行不可
                CLMsg.Show("GE0146")
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor
            fmNewEntryData = New FM080103()
            fmNewEntryData.ShowDialog()

            If fmNewEntryData.intClickBtn = 0 Then
                pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                clsUC080104 = pn.Controls(SCREEN_ID_UC080104)
                If clsUC080104 Is Nothing Then
                    clsUC080104 = New UC080104
                    clsUC080104.intClickBtnFlg = 0
                    '締め日未選択データのフラグ
                    clsUC080104.IsFreeEntry = blnIsFreeEntry
                    '各設定項目を渡す
                    clsUC080104.strPayStatus = fmNewEntryData.strPayStatus
                    clsUC080104.strPayStatusCd = fmNewEntryData.strPayStatusCd
                    clsUC080104.strTitle = fmNewEntryData.strTitle
                    clsUC080104.datePayDay = fmNewEntryData.datePayDate
                    clsUC080104.SelectCloseDay = dtSelect
                    Call pn.Controls.Add(clsUC080104)
                Else
                    clsUC080104.intClickBtnFlg = 0
                    '締め日未選択データのフラグ
                    clsUC080104.IsFreeEntry = blnIsFreeEntry
                    '各設定項目を渡す
                    clsUC080104.strPayStatus = fmNewEntryData.strPayStatus
                    clsUC080104.strTitle = fmNewEntryData.strTitle
                    clsUC080104.datePayDay = fmNewEntryData.datePayDate
                    clsUC080104.SelectCloseDay = dtSelect
                End If

                Me.Visible = False '労金データ検索画面非表示
                pn.Visible = True '労金データ新規作成画面表示
            End If

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        End Try

    End Sub
#End Region

#Region "振込検索ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPayDaySearch_Click
    '   名称　：振込検索ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPayDaySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPayDaySearch.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        '振込日検索処理
        Call Me.SearchPayDay()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "振込検索詳細ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPayDayDtl_Click
    '   名称　：振込検索詳細ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPayDayDtl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPayDayDtl.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Call Me.ShowPayDayDtl()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "個人別振込検索ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPersonalPaySearch_Click
    '   名称　：個人別振込検索ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPersonalPaySearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPersonalPaySearch.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        If ChkNull(Me.txtPersonalPayStafId.Text.Trim) = True Then
            CLMsg.Show("GE0006", "社員番号")
            Exit Sub
        End If

        '個人別振込検索処理の呼び出し
        Call Me.SearchPersonalPay()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "個人別振込詳細ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPersonalPayDtl_Click
    '   名称　：個人別振込詳細ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPersonalPayDtl_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPersonalPayDtl.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Call Me.ShowPersonalPayDtl()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "組合員抽出ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnSelectMember_Click
    '   名称　：組合員抽出ボタンクリック
    '   概要　：
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnSelectMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSelectMember.Click
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Me.Cursor = Cursors.WaitCursor
        '組合員抽出画面
        Dim fmUnion As FM000204 = New FM000204()
        Dim strUserIdList As List(Of String) = New List(Of String)

        '組合員抽出画面の表示
        fmUnion.ShowDialog()
        Select Case fmUnion.IntQlickBtnFlag
            Case 0 'OKボタン押下時
                '選択された組合員のリスト
                Dim dt As DataTable = fmUnion.SelectMemberList

                If dt.Rows.Count > 1 Then
                    CLMsg.Show("GE0150")
                    Me.Cursor = Cursors.Default
                    Exit Sub
                End If

                Me.txtPersonalPayStafId.Text = dt.Rows(0).Item("社員番号").ToString
                Me.lblPersonalPayStafName.Text = dt.Rows(0).Item("名前").ToString
            Case 1
                'キャンセルの場合何も行わない
        End Select
        Me.Cursor = Cursors.Default
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "賃金カット・役員手当ダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgdPayCut_CellDoubleClick
    '   名称　：賃金カット・役員手当ダブルクリック
    '   概要　：賃金カット・役員手当ダブルクリック
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdPayCut_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdPayCut.CellDoubleClick
        If Me.dgdPayCut.SelectedRows.Count < 1 Then
            Exit Sub
        End If

        If Me.dgdPayCut.SelectedRows(0).Cells(2).Tag = "02" Then '労金データはあるが未作成の組合員が存在する
            Dim fmRemain As FM080102 = New FM080102(DAILY_PAY_KIND_CUT, _
                                                    Me.dgdPayCut.SelectedRows(0).Cells(1).Value, _
                                                    DAILY_PAY_KIND_CUT_CODE)
            '振込データ未作成の組合員画面の表示
            fmRemain.ShowDialog()
        ElseIf Me.dgdPayCut.SelectedRows(0).Cells(2).Tag = "01" Then '全組合員に対して労金データ作成済み
            CLMsg.Show("GI0025")
            Exit Sub
        ElseIf Me.dgdPayCut.SelectedRows(0).Cells(2).Tag = "03" Then '労金データ未作成
            CLMsg.Show("GI0024")
            Exit Sub
        End If
    End Sub

#End Region

#Region "一時金カットダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgdOnceCut_CellDoubleClick
    '   名称　：一時金カットダブルクリック
    '   概要　：一時金カットダブルクリック
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdOnceCut_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdOnceCut.CellDoubleClick
        If Me.dgdOnceCut.SelectedRows.Count < 1 Then
            Exit Sub
        End If

        If Me.dgdOnceCut.SelectedRows(0).Cells(2).Tag = "02" Then '労金データはあるが未作成の組合員が存在する
            Dim fmRemain As FM080102 = New FM080102(DAILY_PAY_KIND_ONCE_CUT, _
                                                    Me.dgdOnceCut.SelectedRows(0).Cells(1).Value, _
                                                    DAILY_PAY_KIND_ONCE_CUT_CODE)
            '振込で未作成の組合員画面の表示
            fmRemain.ShowDialog()
        ElseIf Me.dgdOnceCut.SelectedRows(0).Cells(2).Tag = "01" Then '全組合員に対して労金データ作成済み
            CLMsg.Show("GI0025")
            Exit Sub
        ElseIf Me.dgdOnceCut.SelectedRows(0).Cells(2).Tag = "03" Then '労金データ未作成
            CLMsg.Show("GI0024")
            Exit Sub
        End If
    End Sub
#End Region

#Region "部／委員会日当ダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgdDailyPayCommittee_CellDoubleClick
    '   名称　：部／委員会日当ダブルクリック
    '   概要　：部／委員会日当ダブルクリック
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdDailyPayCommittee_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdDailyPayCommittee.CellDoubleClick
        If Me.dgdDailyPayCommittee.SelectedRows.Count < 1 Then
            Exit Sub
        End If

        If Me.dgdDailyPayCommittee.SelectedRows(0).Cells(2).Tag = "02" Then '労金データはあるが未作成の組合員が存在する
            Dim fmRemain As FM080102 = New FM080102(DAILY_PAY_KIND_COMMITTEE, _
                                                    Me.dgdDailyPayCommittee.SelectedRows(0).Cells(1).Value, _
                                                    DAILY_PAY_KIND_COMMITTEE_CODE)
            '振込で未作成の組合員画面の表示
            fmRemain.ShowDialog()
        ElseIf Me.dgdDailyPayCommittee.SelectedRows(0).Cells(2).Tag = "01" Then '全組合員に対して労金データ作成済み
            CLMsg.Show("GI0025")
            Exit Sub
        ElseIf Me.dgdDailyPayCommittee.SelectedRows(0).Cells(2).Tag = "03" Then '労金データ未作成
            CLMsg.Show("GI0024")
            Exit Sub
        End If
    End Sub
#End Region

#Region "支部委員（三役）日当ダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgdDailyPayBranch_CellDoubleClick
    '   名称　：支部委員（三役）日当ダブルクリック
    '   概要　：支部委員（三役）日当ダブルクリック
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdDailyPayBranch_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdDailyPayBranch.CellDoubleClick
        If Me.dgdDailyPayBranch.SelectedRows.Count < 1 Then
            Exit Sub
        End If

        If Me.dgdDailyPayBranch.SelectedRows(0).Cells(2).Tag = "02" Then '労金データはあるが未作成の組合員が存在する
            Dim fmRemain As FM080102 = New FM080102(DAILY_PAY_KIND_BRANCH, _
                                                    Me.dgdDailyPayBranch.SelectedRows(0).Cells(1).Value, _
                                                    DAILY_PAY_KIND_BRANCH_CODE)
            '振込で未作成の組合員画面の表示
            fmRemain.ShowDialog()
        ElseIf Me.dgdDailyPayBranch.SelectedRows(0).Cells(2).Tag = "01" Then '全組合員に対して労金データ作成済み
            CLMsg.Show("GI0025")
            Exit Sub
        ElseIf Me.dgdDailyPayBranch.SelectedRows(0).Cells(2).Tag = "03" Then '労金データ未作成
            CLMsg.Show("GI0024")
            Exit Sub
        End If
    End Sub
#End Region

#Region "中央執行委員会日当ダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgdDailyPayExecutive_CellDoubleClick
    '   名称　：中央執行委員会日当ダブルクリック
    '   概要　：中央執行委員会日当ダブルクリック
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdDailyPayExecutive_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdDailyPayExecutive.CellDoubleClick
        If Me.dgdDailyPayExecutive.SelectedRows.Count < 1 Then
            Exit Sub
        End If

        If Me.dgdDailyPayExecutive.SelectedRows(0).Cells(2).Tag = "02" Then '労金データはあるが未作成の組合員が存在する
            Dim fmRemain As FM080102 = New FM080102(DAILY_PAY_KIND_EXECUTIVE, _
                                                    Me.dgdDailyPayExecutive.SelectedRows(0).Cells(1).Value, _
                                                    DAILY_PAY_KIND_EXECUTIVE_CODE)
            '振込で未作成の組合員画面の表示
            fmRemain.ShowDialog()
        ElseIf Me.dgdDailyPayExecutive.SelectedRows(0).Cells(2).Tag = "01" Then '全組合員に対して労金データ作成済み
            CLMsg.Show("GI0025")
            Exit Sub
        ElseIf Me.dgdDailyPayExecutive.SelectedRows(0).Cells(2).Tag = "03" Then '労金データ未作成
            CLMsg.Show("GI0024")
            Exit Sub
        End If
    End Sub
#End Region

#Region "DGM日当ダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgdDailyPayDGM_CellDoubleClick
    '   名称　：DGM日当ダブルクリック
    '   概要　：DGM日当ダブルクリック
    '   作成日：2012/02/24(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/24(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdDailyPayDGM_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdDailyPayDGM.CellDoubleClick
        If Me.dgdDailyPayDGM.SelectedRows.Count < 1 Then
            Exit Sub
        End If

        If Me.dgdDailyPayDGM.SelectedRows(0).Cells(2).Tag = "02" Then '労金データはあるが未作成の組合員が存在する
            Dim fmRemain As FM080102 = New FM080102(DAILY_PAY_KIND_DGM, _
                                                    Me.dgdDailyPayDGM.SelectedRows(0).Cells(1).Value, _
                                                    DAILY_PAY_KIND_DGM_CODE)
            '振込で未作成の組合員画面の表示
            fmRemain.ShowDialog()
        ElseIf Me.dgdDailyPayDGM.SelectedRows(0).Cells(2).Tag = "01" Then '全組合員に対して労金データ作成済み
            CLMsg.Show("GI0025")
            Exit Sub
        ElseIf Me.dgdDailyPayDGM.SelectedRows(0).Cells(2).Tag = "03" Then '労金データ未作成
            CLMsg.Show("GI0024")
            Exit Sub
        End If

    End Sub
#End Region

#Region "振込状況確認、データ作成タブ-開始年コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbMakeDataYearFrom_KeyPress
    '   名称　：振込状況確認、データ作成タブ-開始年コンボボックスKeyPressイベント
    '   概要　：振込状況確認、データ作成タブ-開始年コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbMakeDataYearFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMakeDataYearFrom.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            'Enter押下時、検索処理を実行する
            Call Me.SearchMakeData()
        End If
    End Sub

#End Region

#Region "振込状況確認、データ作成タブ-開始年コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbMakeDataYearFrom_SelectedIndexChanged
    '   名称　：振込状況確認、データ作成タブ-開始年コンボボックス値変更時
    '   概要　：振込状況確認、データ作成タブ-開始年コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbMakeDataYearFrom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMakeDataYearFrom.SelectedIndexChanged
        If grpMakeDataPayCutResult.Visible = True Then
            Call Me.ShowMakeDataControls(False)
        End If
    End Sub
#End Region

#Region "振込状況確認、データ作成タブ-開始月コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbMakeDataMonthFrom_KeyPress
    '   名称　：振込状況確認、データ作成タブ-開始月コンボボックスKeyPressイベント
    '   概要　：振込状況確認、データ作成タブ-開始月コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbMakeDataMonthFrom_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMakeDataMonthFrom.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            'Enter押下時、検索処理を実行する
            Call Me.SearchMakeData()
        End If

    End Sub
#End Region

#Region "振込状況確認、データ作成タブ-開始月コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbMakeDataMonthFrom_SelectedIndexChanged
    '   名称　：振込状況確認、データ作成タブ-開始月コンボボックス値変更時
    '   概要　：振込状況確認、データ作成タブ-開始月コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbMakeDataMonthFrom_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMakeDataMonthFrom.SelectedIndexChanged
        If grpMakeDataPayCutResult.Visible = True Then
            Call Me.ShowMakeDataControls(False)
        End If
    End Sub
#End Region

#Region "振込状況確認、データ作成タブ-終了年コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbMakeDataYearTo_KeyPress
    '   名称　：振込状況確認、データ作成タブ-終了年コンボボックスKeyPressイベント
    '   概要　：振込状況確認、データ作成タブ-終了年コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbMakeDataYearTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMakeDataYearTo.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            'Enter押下時、検索処理を実行する
            Call Me.SearchMakeData()
        End If

    End Sub
#End Region

#Region "振込状況確認、データ作成タブ-終了年コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbMakeDataYearTo_SelectedIndexChanged
    '   名称　：振込状況確認、データ作成タブ-終了年コンボボックス値変更時
    '   概要　：振込状況確認、データ作成タブ-終了年コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbMakeDataYearTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMakeDataYearTo.SelectedIndexChanged
        If grpMakeDataPayCutResult.Visible = True Then
            Call Me.ShowMakeDataControls(False)
        End If
    End Sub
#End Region

#Region "振込状況確認、データ作成タブ-終了月コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbMakeDataMonthTo_KeyPress
    '   名称　：振込状況確認、データ作成タブ-終了月コンボボックスKeyPressイベント
    '   概要　：振込状況確認、データ作成タブ-終了月コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbMakeDataMonthTo_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbMakeDataMonthTo.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            'Enter押下時、検索処理を実行する
            Call Me.SearchMakeData()
        End If

    End Sub
#End Region

#Region "振込状況確認、データ作成タブ-終了月コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbMakeDataMonthTo_SelectedIndexChanged
    '   名称　：振込状況確認、データ作成タブ-終了月コンボボックス値変更時
    '   概要　：振込状況確認、データ作成タブ-終了月コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbMakeDataMonthTo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMakeDataMonthTo.SelectedIndexChanged
        If grpMakeDataPayCutResult.Visible = True Then
            Call Me.ShowMakeDataControls(False)
        End If
    End Sub
#End Region

#Region "振込日検索タブ-年コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbPayDayYear_KeyPress
    '   名称　：振込日検索タブ-年コンボボックスKeyPressイベント
    '   概要　：振込日検索タブ-年コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPayDayYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPayDayYear.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            'Enter押下時、検索処理を実行する
            Call Me.SearchPayDay()
        End If

    End Sub
#End Region

#Region "振込日検索タブ-年コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbPayDayYear_SelectedIndexChanged
    '   名称　：振込日検索タブ-年コンボボックス値変更時
    '   概要　：振込日検索タブ-年コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPayDayYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPayDayYear.SelectedIndexChanged
        If grpPayDayResult.Visible = True Then
            Call Me.ShowPayDayControls()
        End If

        If ChkNull(Me.cmbPayDayYear.Text) = False AndAlso ChkNull(Me.cmbPayDayMonth.Text) = False Then
            If ChkNumber(Me.cmbPayDayYear.Text) = True AndAlso ChkNumber(Me.cmbPayDayMonth.Text) = True Then
                '選択年月に応じた日付のセット
                Call CreateComboBoxDD(Me.cmbPayDayDay, CInt(Me.cmbPayDayYear.Text), CInt(Me.cmbPayDayMonth.Text))
            End If
        Else
            '月が選択されていない場合、日付コンボボックスをクリアする
            Me.cmbPayDayDay.DataSource = Nothing
        End If
    End Sub
#End Region

#Region "振込日検索タブ-月コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbPayDayMonth_KeyPress
    '   名称　：振込日検索タブ-月コンボボックスKeyPressイベント
    '   概要　：振込日検索タブ-月コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPayDayMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPayDayMonth.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            'Enter押下時、検索処理を実行する
            Call Me.SearchPayDay()
        End If
    End Sub
#End Region

#Region "振込日検索タブ-月コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbPayDayMonth_SelectedIndexChanged
    '   名称　：振込日検索タブ-月コンボボックス値変更時
    '   概要　：振込日検索タブ-月コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPayDayMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPayDayMonth.SelectedIndexChanged
        If grpPayDayResult.Visible = True Then
            Call Me.ShowPayDayControls()
        End If

        If ChkNull(Me.cmbPayDayYear.Text) = False AndAlso ChkNull(Me.cmbPayDayMonth.Text) = False Then
            If ChkNumber(Me.cmbPayDayYear.Text) = True AndAlso ChkNumber(Me.cmbPayDayMonth.Text) = True Then
                '選択年月に応じた日付のセット
                Call CreateComboBoxDD(Me.cmbPayDayDay, CInt(Me.cmbPayDayYear.Text), CInt(Me.cmbPayDayMonth.Text))
            End If
        Else
            '月が選択されていない場合、日付コンボボックスをクリアする
            Me.cmbPayDayDay.DataSource = Nothing
        End If
    End Sub
#End Region

#Region "振込日検索タブ-日コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbPayDayDay_KeyPress
    '   名称　：振込日検索タブ-日コンボボックスKeyPressイベント
    '   概要　：振込日検索タブ-日コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPayDayDay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPayDayDay.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            'Enter押下時、検索処理を実行する
            Call Me.SearchPayDay()
        End If
    End Sub
#End Region

#Region "振込日検索タブ-日コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbPayDayDay_SelectedIndexChanged
    '   名称　：振込日検索タブ-日コンボボックス値変更時
    '   概要　：振込日検索タブ-日コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPayDayDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPayDayDay.SelectedIndexChanged
        If grpPayDayResult.Visible = True Then
            Call Me.ShowPayDayControls()
        End If
    End Sub
#End Region

#Region "個人別振込検索タブ-年コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbPersonalPayYear_KeyPress
    '   名称　：個人別振込検索タブ-年コンボボックスKeyPressイベント
    '   概要　：個人別振込検索タブ-年コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPersonalPayYear_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPersonalPayYear.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            If ChkNull(Me.txtPersonalPayStafId.Text.Trim) = True Then
                CLMsg.Show("GE0006", "社員番号")
                Exit Sub
            End If
            'Enter押下時、検索処理を実行する
            Call Me.SearchPersonalPay()
        End If
    End Sub
#End Region

#Region "個人別振込検索タブ-年コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbPersonalPayYear_SelectedIndexChanged
    '   名称　：個人別振込検索タブ-年コンボボックス値変更時
    '   概要　：個人別振込検索タブ-年コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPersonalPayYear_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPersonalPayYear.SelectedIndexChanged
        If grpPersonalPayResult.Visible = True Then
            Call Me.ShowPersonalPayControls()
        End If

        If ChkNull(Me.cmbPersonalPayYear.Text) = False AndAlso ChkNull(Me.cmbPersonalPayMonth.Text) = False Then
            If ChkNumber(Me.cmbPersonalPayYear.Text) = True AndAlso ChkNumber(Me.cmbPersonalPayMonth.Text) = True Then
                '選択年月に応じた日付のセット
                Call CreateComboBoxDD(Me.cmbPersonalPayDay, CInt(Me.cmbPersonalPayYear.Text), CInt(Me.cmbPersonalPayMonth.Text))
            End If
        Else
            '月が選択されていない場合、日付コンボボックスをクリアする
            Me.cmbPersonalPayDay.DataSource = Nothing
        End If
    End Sub
#End Region

#Region "個人別振込検索タブ-月コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbPersonalPayYear_KeyPress
    '   名称　：個人別振込検索タブ-月コンボボックスKeyPressイベント
    '   概要　：個人別振込検索タブ-月コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPersonalPayMonth_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPersonalPayMonth.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            If ChkNull(Me.txtPersonalPayStafId.Text.Trim) = True Then
                CLMsg.Show("GE0006", "社員番号")
                Exit Sub
            End If
            'Enter押下時、検索処理を実行する
            Call Me.SearchPersonalPay()
        End If
    End Sub
#End Region

#Region "個人別振込検索タブ-月コンボボックス値変更時"

    '***************************************************************************************************
    '   ＩＤ　：cmbPersonalPayMonth_SelectedIndexChanged
    '   名称　：個人別振込検索タブ-月コンボボックス値変更時
    '   概要　：個人別振込検索タブ-月コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPersonalPayMonth_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPersonalPayMonth.SelectedIndexChanged
        If grpPersonalPayResult.Visible = True Then
            Call Me.ShowPersonalPayControls()
        End If

        If ChkNull(Me.cmbPersonalPayYear.Text) = False AndAlso ChkNull(Me.cmbPersonalPayMonth.Text) = False Then
            If ChkNumber(Me.cmbPersonalPayYear.Text) = True AndAlso ChkNumber(Me.cmbPersonalPayMonth.Text) = True Then
                '選択年月に応じた日付のセット
                Call CreateComboBoxDD(Me.cmbPersonalPayDay, CInt(Me.cmbPersonalPayYear.Text), CInt(Me.cmbPersonalPayMonth.Text))
            End If
        Else
            '月が選択されていない場合、日付コンボボックスをクリアする
            Me.cmbPersonalPayDay.DataSource = Nothing
        End If
    End Sub
#End Region

#Region "個人別振込検索タブ-日コンボボックスKeyPressイベント"
    '***************************************************************************************************
    '   ＩＤ　：cmbPersonalPayYear_KeyPress
    '   名称　：個人別振込検索タブ-日コンボボックスKeyPressイベント
    '   概要　：個人別振込検索タブ-日コンボボックスKeyPressイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPersonalPayDay_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cmbPersonalPayDay.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            If ChkNull(Me.txtPersonalPayStafId.Text.Trim) = True Then
                CLMsg.Show("GE0006", "社員番号")
                Exit Sub
            End If
            'Enter押下時、検索処理を実行する
            Call Me.SearchPersonalPay()
        End If
    End Sub
#End Region

#Region "個人別振込検索タブ-日コンボボックス値変更時"
    '***************************************************************************************************
    '   ＩＤ　：cmbPersonalPayDay_SelectedIndexChanged
    '   名称　：個人別振込検索タブ-日コンボボックス値変更時
    '   概要　：個人別振込検索タブ-日コンボボックス値変更時
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub cmbPersonalPayDay_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPersonalPayDay.SelectedIndexChanged
        If grpPersonalPayResult.Visible = True Then
            Call Me.ShowPersonalPayControls()
        End If
    End Sub
#End Region

#Region "社員番号テキストボックスkeypressイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtPersonalPayStafId_KeyPress
    '   名称　：社員番号テキストボックスLeaveイベント
    '   概要　：社員番号テキストボックスLeaveイベント
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtPersonalPayStafId_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPersonalPayStafId.KeyPress
        If e.KeyChar.Equals(Chr(Keys.Enter)) Then
            If ChkNull(Me.txtPersonalPayStafId.Text.Trim) = True Then
                CLMsg.Show("GE0006", "社員番号")
                Exit Sub
            End If
            '社員名表示処理
            Call Me.SetMemberName()
        End If
    End Sub
#End Region

#Region "社員番号テキストボックスLeaveイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtPersonalPayStafId_Leave
    '   名称　：社員番号テキストボックスLeaveイベント
    '   概要　：社員番号テキストボックスLeaveイベント
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtPersonalPayStafId_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPersonalPayStafId.Leave
        '社員名表示処理
        Call Me.SetMemberName()
    End Sub
#End Region

#Region "社員番号テキストチェンジイベント"
    '***************************************************************************************************
    '   ＩＤ　：txtPersonalPayStafId_TextChanged
    '   名称　：社員番号テキストチェンジイベントイベント
    '   概要　：
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtPersonalPayStafId_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPersonalPayStafId.TextChanged
        '検索結果を非表示にする
        Call Me.ShowPersonalPayControls()
    End Sub
#End Region

#Region "振込日検索グリッドダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgdPayDay_CellDoubleClick
    '   名称　：振込日検索グリッドダブルクリック
    '   概要　：
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdPayDay_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdPayDay.CellDoubleClick
        Call Me.ShowPayDayDtl()
    End Sub
#End Region

#Region "個人別振込検索グリッドダブルクリック"
    '***************************************************************************************************
    '   ＩＤ　：dgdPersonalPay_CellDoubleClick
    '   名称　：振込日検索グリッドダブルクリック
    '   概要　：
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdPersonalPay_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdPersonalPay.CellDoubleClick
        Call Me.ShowPersonalPayDtl()
    End Sub

#End Region

#End Region

#Region "関数"

#Region "各グリッドの選択データカウント"
    '***************************************************************************************************
    '   ＩＤ　：GetSelectCloseDayCount
    '   名称　：各グリッドの選択データカウント
    '   概要　：各グリッドの選択データをカウントします
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetSelectCloseDay() As DataTable
        Dim dtSelectCloseDay As DataTable = New DataTable()
        Dim intSelectCount As Integer = 0
        Dim intCnt As Integer = 0

        '列追加
        dtSelectCloseDay.Columns.Add("close_day")
        dtSelectCloseDay.Columns.Add("close_day_kind")

        '賃金カット・役員手当
        For intCnt = 0 To Me.dgdPayCut.Rows.Count - 1
            If Me.dgdPayCut.Rows(intCnt).Cells(0).Value = True Then
                dtSelectCloseDay.Rows.Add()
                '締め日をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(0) = Me.dgdPayCut.Rows(intCnt).Cells(1).Value
                '締め日種別をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(1) = DAILY_PAY_KIND_CUT_CODE
            End If
        Next

        '一時金カット
        For intCnt = 0 To Me.dgdOnceCut.Rows.Count - 1
            If Me.dgdOnceCut.Rows(intCnt).Cells(0).Value = True Then
                dtSelectCloseDay.Rows.Add()
                '締め日をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(0) = Me.dgdOnceCut.Rows(intCnt).Cells(1).Value
                '締め日種別をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(1) = DAILY_PAY_KIND_ONCE_CUT_CODE
            End If
        Next

        '部／委員会
        For intCnt = 0 To Me.dgdDailyPayCommittee.Rows.Count - 1
            If Me.dgdDailyPayCommittee.Rows(intCnt).Cells(0).Value = True Then
                dtSelectCloseDay.Rows.Add()
                '締め日をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(0) = Me.dgdDailyPayCommittee.Rows(intCnt).Cells(1).Value
                '締め日種別をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(1) = DAILY_PAY_KIND_COMMITTEE_CODE
            End If
        Next

        '支部委員（三役）
        For intCnt = 0 To Me.dgdDailyPayBranch.Rows.Count - 1
            If Me.dgdDailyPayBranch.Rows(intCnt).Cells(0).Value = True Then
                dtSelectCloseDay.Rows.Add()
                '締め日をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(0) = Me.dgdDailyPayBranch.Rows(intCnt).Cells(1).Value
                '締め日種別をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(1) = DAILY_PAY_KIND_BRANCH_CODE
            End If
        Next

        '中央執行委員会
        For intCnt = 0 To Me.dgdDailyPayExecutive.Rows.Count - 1
            If Me.dgdDailyPayExecutive.Rows(intCnt).Cells(0).Value = True Then
                dtSelectCloseDay.Rows.Add()
                '締め日をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(0) = Me.dgdDailyPayExecutive.Rows(intCnt).Cells(1).Value
                '振込状況コードをセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(1) = DAILY_PAY_KIND_EXECUTIVE_CODE
            End If
        Next

        'DGM
        For intCnt = 0 To Me.dgdDailyPayDGM.Rows.Count - 1
            If Me.dgdDailyPayDGM.Rows(intCnt).Cells(0).Value = True Then
                dtSelectCloseDay.Rows.Add()
                '締め日をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(0) = Me.dgdDailyPayDGM.Rows(intCnt).Cells(1).Value
                '締め日種別をセット
                dtSelectCloseDay.Rows(dtSelectCloseDay.Rows.Count - 1).Item(1) = DAILY_PAY_KIND_DGM_CODE
            End If
        Next

        Return dtSelectCloseDay
    End Function
#End Region

#Region "振込状況文字列取得"
    '***************************************************************************************************
    '   ＩＤ　：GetBankStatusString
    '   名称　：振込状況文字列取得
    '   概要　：振込状況の各コードに対応する文字列を取得し、ディクショナリ―にセットします
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub GetBankStatusString()
        Dim dtRet As DataTable = Nothing
        Dim strSql As String = String.Empty
        Dim clsDb As CLAccessMdb = New CLAccessMdb

        Try
            _dicBankStatus.Clear()
            strSql = "SELECT c_constant_seq,l_omission_name FROM constant_dtl " & _
                     "WHERE c_constant = 'BANK_SEND_STATUS' "
            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                For Each dtRow As DataRow In dtRet.Rows
                    If Me._dicBankStatus.ContainsKey(dtRow.Item("c_constant_seq")) = False Then
                        'コードをキーに振込状況文字列をセット
                        Me._dicBankStatus.Add(dtRow.Item("c_constant_seq"), dtRow.Item("l_omission_name"))
                    End If
                Next
            End If
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try
    End Sub

#End Region

#Region "各締め日検索"
    '***************************************************************************************************
    '   ＩＤ　：SearchMakeData
    '   名称　：各締め日検索
    '   概要　：各締め日検索
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SearchMakeData()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        If ChkSelectMonthOnly(Me.cmbMakeDataYearFrom, Me.cmbMakeDataMonthFrom) = False _
        OrElse ChkSelectMonthOnly(Me.cmbMakeDataYearTo, Me.cmbMakeDataMonthTo) = False Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor

        Dim strFromDate As String = Nothing
        Dim strToDate As String = Nothing

        If Me.cmbMakeDataMonthFrom.SelectedIndex = 0 Then
            '開始月の選択がない場合、1月検索として実行する
            strFromDate = Me.cmbMakeDataYearFrom.Text & "/" & "01/01"
        Else
            strFromDate = Me.cmbMakeDataYearFrom.Text & "/" & Me.cmbMakeDataMonthFrom.Text & "/" & "01"
        End If
        If Me.cmbMakeDataMonthTo.SelectedIndex = 0 Then
            '終了月の選択がない場合、12月検索として実行する
            strToDate = Me.cmbMakeDataYearTo.Text & "/" & "12/31"
        Else
            strToDate = Me.cmbMakeDataYearTo.Text & "/" &
            Me.cmbMakeDataMonthTo.Text & "/" & DateTime.DaysInMonth(CInt(Me.cmbMakeDataYearTo.Text), CInt(Me.cmbMakeDataMonthTo.Text))
        End If
        If (strFromDate.Length = 6) Then
            strFromDate = "1900" & strFromDate
        End If
        strToDate = strToDate.PadLeft(10, "9")

        '検索処理の呼び出し
        Call SetSearchResultCloseDay(strFromDate, strToDate)
        '各種オブジェクトの表示
        Call Me.ShowMakeDataControls(True)
        '各グリッドの選択状態を解除
        Call Me.CurrentCellReset()

        Me.Cursor = Cursors.Default
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

#End Region

#Region "日当側の締め日検索"
    '***************************************************************************************************
    '   ＩＤ　：SearchDailyPayCloseDay
    '   名称　：日当側の締め日検索
    '   概要　：日当側の締め日検索
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function SearchDailyPayCloseDay(ByVal strDayFrom As String, ByVal strDayTo As String, ByVal strCloseDayKind As String) As DataTable
        Dim dtRet As DataTable = Nothing
        Dim strSql As String = String.Empty
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Try
            'daily_pay_close、staf_bank_closeテーブルより該当締め日を取得
            strSql = "SELECT DAYPAY_CLOSE.d_daily_pay_close as 締め日, " &
                      "      IIF(STAF_CLOSE.k_bank_send_status is null, '03', STAF_CLOSE.k_bank_send_status) as k_bank_send_status," &
                      "      STAF_CLOSE.d_pay_close, " &
                      "      STAF_CLOSE.k_daily_pay_kind, " &
                      "      STAF_CLOSE.d_ins," &
                      "      STAF_CLOSE.d_up " &
                      "FROM ( " &
                      "      select distinct d_daily_pay_close " &
                      "      from daily_pay_close " &
                      "      where k_daily_pay_kind = '" & strCloseDayKind & "' " &
                      "      and d_daily_pay_close >= '" & strDayFrom & "' " &
                      "      and d_daily_pay_close <= '" & strDayTo & "' " &
                      "      ) DAYPAY_CLOSE left outer join " &
                      "        ( " &
                      "         select d_pay_close, " &
                      "                k_bank_send_status, " &
                      "                k_daily_pay_kind,  " &
                      "                d_ins,  " &
                      "            d_up " &
                      "            from staf_bank_close " &
                      "         where k_daily_pay_kind = '" & strCloseDayKind & "' " &
                      "         ) STAF_CLOSE on  Mid(DAYPAY_CLOSE.d_daily_pay_close,1,6) = STAF_CLOSE.d_pay_close " &
                      "ORDER BY DAYPAY_CLOSE.d_daily_pay_close desc "   'ok

            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try

        Return dtRet
    End Function
#End Region

#Region "源泉徴収側の締め日検索"
    '***************************************************************************************************
    '   ＩＤ　：GetSelectCloseDayCount
    '   名称　：源泉徴収側の締め日検索
    '   概要　：源泉徴収側の締め日検索
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function SearchPayCutCloseDay(ByVal strDayFrom As String, ByVal strDayTo As String, ByVal strCloseDayKind As String) As DataTable
        Dim dtRet As DataTable = Nothing
        Dim strSql As String = String.Empty
        Dim clsDb As CLAccessMdb = New CLAccessMdb

        Try
            'taxation_total、staf_bank_closeテーブルより該当締め日を取得
            strSql = "SELECT FORMAT(TAX_TOTAL.d_years, 'yyyy/MM/dd') as 締め日, " &
                     "       IIF(STAF_CLOSE.k_bank_send_status is null, '03', STAF_CLOSE.k_bank_send_status) as k_bank_send_status, " &
                     "       STAF_CLOSE.d_pay_close,  " &
                     "       STAF_CLOSE.k_daily_pay_kind,  " &
                     "       STAF_CLOSE.d_ins,  " &
                     "       STAF_CLOSE.d_up " &
                     "FROM (select distinct d_years " &
                     "      from taxation_total " &
                     "      where d_years >= '" & strDayFrom & "' " &
                     "      and d_years <= '" & strDayTo & "'  " &
                     "      and k_daily_pay_kind = '" & strCloseDayKind & "' " &
                     "     ) TAX_TOTAL " &
                     "LEFT OUTER JOIN " &
                     "     (SELECT d_pay_close, " &
                     "             k_bank_send_status, " &
                     "             k_daily_pay_kind,  " &
                     "             d_ins, " &
                     "             d_up " &
                     "      FROM staf_bank_close " &
                     "      WHERE k_daily_pay_kind = '" & strCloseDayKind & "' " &
                     "      ) STAF_CLOSE  " &
                     "      ON FORMAT(TAX_TOTAL.d_years,'yyyyMM') = STAF_CLOSE.d_pay_close " &
                     "ORDER BY TAX_TOTAL.d_years DESC " 'ok

            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try

        Return dtRet
    End Function

#End Region

#Region "締め日の検索結果セット"
    '***************************************************************************************************
    '   ＩＤ　：SetSearchResult
    '   名称　：源泉徴収側の締め日検索
    '   概要　：源泉徴収側の締め日検索
    '   作成日：2012/02/02(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/02(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetSearchResultCloseDay(ByVal strFromDate As String, ByVal strToDate As String)
        '賃金カット
        Dim dtPayCut As DataTable = SearchPayCutCloseDay(strFromDate, strToDate, DAILY_PAY_KIND_CUT_CODE)
        '一時金カット
        Dim dtOnceCut As DataTable = SearchPayCutCloseDay(strFromDate, strToDate, DAILY_PAY_KIND_ONCE_CUT_CODE)
        '部／委員会日当
        Dim dtDailyPayCommittee As DataTable = SearchDailyPayCloseDay(strFromDate.Replace("/", "").Replace("-", ""),
                                                                      strToDate.Replace("/", "").Replace("-", ""),
                                                                      DAILY_PAY_KIND_COMMITTEE_CODE)
        '支部委員会三役日当
        Dim dtDailyPayBranch As DataTable = SearchDailyPayCloseDay(strFromDate.Replace("/", "").Replace("-", ""),
                                                                   strToDate.Replace("/", "").Replace("-", ""), DAILY_PAY_KIND_BRANCH_CODE)
        '中央執行委員会日当
        Dim dtDailyPayExecutive As DataTable = SearchDailyPayCloseDay(strFromDate.Replace("/", "").Replace("-", ""),
                                                                      strToDate.Replace("/", "").Replace("-", ""), DAILY_PAY_KIND_EXECUTIVE_CODE)
        'DGM日当
        Dim dtDailyPayDgm As DataTable = SearchDailyPayCloseDay(strFromDate.Replace("/", "").Replace("-", ""),
                                                                strToDate.Replace("/", "").Replace("-", ""), DAILY_PAY_KIND_DGM_CODE)
        Dim intCnt As Integer = 0

        '各グリッドのクリア
        Me.dgdPayCut.Rows.Clear()
        Me.dgdOnceCut.Rows.Clear()
        Me.dgdDailyPayCommittee.Rows.Clear()
        Me.dgdDailyPayBranch.Rows.Clear()
        Me.dgdDailyPayExecutive.Rows.Clear()
        Me.dgdDailyPayDGM.Rows.Clear()

        '賃金カット・役員手当へセット
        If dtPayCut.Rows.Count > 0 Then
            For Each dtRow As DataRow In dtPayCut.Rows
                Me.dgdPayCut.Rows.Add()
                Me.dgdPayCut(1, intCnt).Value = dtRow.Item("締め日").ToString.Substring(0, 7)
                If _dicBankStatus.ContainsKey(dtRow.Item("k_bank_send_status").ToString) = True Then
                    Me.dgdPayCut(2, intCnt).Value = _dicBankStatus(dtRow.Item("k_bank_send_status").ToString)
                    Me.dgdPayCut(2, intCnt).Tag = dtRow.Item("k_bank_send_status")
                    If dtRow.Item("k_bank_send_status") = "01" Then
                        '労金データ作成済み締め日の場合チェック不可にする
                        Me.dgdPayCut(0, intCnt).ReadOnly = True
                        Me.dgdPayCut(0, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdPayCut(1, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdPayCut(2, intCnt).Style.BackColor = Color.Cornsilk
                    End If
                End If
                intCnt = intCnt + 1
            Next
            Me.dgdPayCut.CurrentCell = Nothing
        End If
        intCnt = 0

        '一時金カットへセット
        If dtOnceCut.Rows.Count > 0 Then
            For Each dtRow As DataRow In dtOnceCut.Rows
                Me.dgdOnceCut.Rows.Add()
                Me.dgdOnceCut(1, intCnt).Value = dtRow.Item("締め日").ToString.Substring(0, 7)
                If _dicBankStatus.ContainsKey(dtRow.Item("k_bank_send_status").ToString) = True Then
                    Me.dgdOnceCut(2, intCnt).Value = _dicBankStatus(dtRow.Item("k_bank_send_status").ToString)
                    Me.dgdOnceCut(2, intCnt).Tag = dtRow.Item("k_bank_send_status")
                    If dtRow.Item("k_bank_send_status") = "01" Then
                        '労金データ作成済み締め日の場合チェック不可にする
                        Me.dgdOnceCut(0, intCnt).ReadOnly = True
                        Me.dgdOnceCut(0, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdOnceCut(1, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdOnceCut(2, intCnt).Style.BackColor = Color.Cornsilk
                    End If

                End If
                intCnt = intCnt + 1
            Next
            Me.dgdOnceCut.CurrentCell = Nothing
        End If
        intCnt = 0

        '部／委員会へセット
        If dtDailyPayCommittee.Rows.Count > 0 Then
            For Each dtRow As DataRow In dtDailyPayCommittee.Rows
                Me.dgdDailyPayCommittee.Rows.Add()
                Me.dgdDailyPayCommittee(1, intCnt).Value = dtRow.Item("締め日").ToString.Substring(0, 6).Insert(4, "/")
                If _dicBankStatus.ContainsKey(dtRow.Item("k_bank_send_status").ToString) = True Then
                    Me.dgdDailyPayCommittee(2, intCnt).Value = _dicBankStatus(dtRow.Item("k_bank_send_status").ToString)
                    Me.dgdDailyPayCommittee(2, intCnt).Tag = dtRow.Item("k_bank_send_status")
                    If dtRow.Item("k_bank_send_status") = "01" Then
                        '労金データ作成済み締め日の場合チェック不可にする
                        Me.dgdDailyPayCommittee(0, intCnt).ReadOnly = True
                        Me.dgdDailyPayCommittee(0, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdDailyPayCommittee(1, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdDailyPayCommittee(2, intCnt).Style.BackColor = Color.Cornsilk
                    End If
                End If
                intCnt = intCnt + 1
            Next
            Me.dgdDailyPayCommittee.CurrentCell = Nothing
        End If
        intCnt = 0

        '支部委員へセット
        If dtDailyPayBranch.Rows.Count > 0 Then
            For Each dtRow As DataRow In dtDailyPayBranch.Rows
                Me.dgdDailyPayBranch.Rows.Add()
                Me.dgdDailyPayBranch(1, intCnt).Value = dtRow.Item("締め日").ToString.Substring(0, 6).Insert(4, "/")
                If _dicBankStatus.ContainsKey(dtRow.Item("k_bank_send_status").ToString) = True Then
                    Me.dgdDailyPayBranch(2, intCnt).Value = _dicBankStatus(dtRow.Item("k_bank_send_status").ToString)
                    Me.dgdDailyPayBranch(2, intCnt).Tag = dtRow.Item("k_bank_send_status")
                    If dtRow.Item("k_bank_send_status") = "01" Then
                        '労金データ作成済み締め日の場合チェック不可にする
                        Me.dgdDailyPayBranch(0, intCnt).ReadOnly = True
                        Me.dgdDailyPayBranch(0, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdDailyPayBranch(1, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdDailyPayBranch(2, intCnt).Style.BackColor = Color.Cornsilk
                    End If

                End If
                intCnt = intCnt + 1
            Next
            Me.dgdDailyPayBranch.CurrentCell = Nothing
        End If
        intCnt = 0

        '中央執行委員会へセット
        If dtDailyPayExecutive.Rows.Count > 0 Then
            For Each dtRow As DataRow In dtDailyPayExecutive.Rows
                Me.dgdDailyPayExecutive.Rows.Add()
                Me.dgdDailyPayExecutive(1, intCnt).Value = dtRow.Item("締め日").ToString.Substring(0, 6).Insert(4, "/")
                If _dicBankStatus.ContainsKey(dtRow.Item("k_bank_send_status").ToString) = True Then
                    Me.dgdDailyPayExecutive(2, intCnt).Value = _dicBankStatus(dtRow.Item("k_bank_send_status").ToString)
                    Me.dgdDailyPayExecutive(2, intCnt).Tag = dtRow.Item("k_bank_send_status")
                    If dtRow.Item("k_bank_send_status") = "01" Then
                        '労金データ作成済み締め日の場合チェック不可にする
                        Me.dgdDailyPayExecutive(0, intCnt).ReadOnly = True
                        Me.dgdDailyPayExecutive(0, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdDailyPayExecutive(1, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdDailyPayExecutive(2, intCnt).Style.BackColor = Color.Cornsilk
                    End If
                End If
                intCnt = intCnt + 1
            Next
            Me.dgdDailyPayExecutive.CurrentCell = Nothing
        End If
        intCnt = 0

        'DGMへセット
        If dtDailyPayDgm.Rows.Count > 0 Then
            For Each dtRow As DataRow In dtDailyPayDgm.Rows
                Me.dgdDailyPayDGM.Rows.Add()
                Me.dgdDailyPayDGM(1, intCnt).Value = dtRow.Item("締め日").ToString.Substring(0, 6).Insert(4, "/")
                If _dicBankStatus.ContainsKey(dtRow.Item("k_bank_send_status").ToString) = True Then
                    Me.dgdDailyPayDGM(2, intCnt).Value = _dicBankStatus(dtRow.Item("k_bank_send_status").ToString)
                    Me.dgdDailyPayDGM(2, intCnt).Tag = dtRow.Item("k_bank_send_status")
                    If dtRow.Item("k_bank_send_status") = "01" Then
                        '労金データ作成済み締め日の場合チェック不可にする
                        Me.dgdDailyPayDGM(0, intCnt).ReadOnly = True
                        Me.dgdDailyPayDGM(0, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdDailyPayDGM(1, intCnt).Style.BackColor = Color.Cornsilk
                        Me.dgdDailyPayDGM(2, intCnt).Style.BackColor = Color.Cornsilk
                    End If
                End If
                intCnt = intCnt + 1
            Next
            Me.dgdDailyPayDGM.CurrentCell = Nothing
        End If
    End Sub

#End Region

#Region "振込日検索処理"
    '***************************************************************************************************
    '   ＩＤ　：SearchPayDay
    '   名称　：振込日検索処理
    '   概要　：振込日検索処理
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SearchPayDay()
        Dim dtRet As DataTable = Nothing
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim iCnt As Integer = 0
        Dim strPayDay As String = Me.cmbPayDayYear.Text & Me.cmbPayDayMonth.Text & Me.cmbPayDayDay.Text
        Dim strPayDayFrom As String = strPayDay.PadRight(8, "0")
        Dim strPayDayTo As String = strPayDay.PadRight(8, "9")

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
            Me.Cursor = Cursors.WaitCursor
            '検索結果を一旦クリア
            Me.dgdPayDay.Rows.Clear()
            If Me.grpPayDayResult.Visible = False Then
                Me.grpPayDayResult.Visible = True
            End If

            strSql = "SELECT BANK_INFO.c_staf_bank_send_id, " &
                     "       BANK_INFO.k_bank_send_margin, " &
                     "       " & UtDb.DbStrYYYYMMDDtoDateText("BANK_INFO.d_bank_send") & " as 振込日, " &
                     "       BANK_INFO.l_name as 支払方法, " &
                     "       BANK_INFO.l_bank_send_item as 題目, " &
                     "       BANK_INFO.bank_pay as 振込総額, " &
                     "       BANK_INFO.pay_cut_total as 源泉総額, " &
                     "       BANK_INFO.daily_pay_total as 日当総額, " &
                     "       BANK_INFO.adjust as 調整総額, " &
                     "       IIF(BANK_INFO.d_up IS NULL, FORMAT(BANK_INFO.d_ins,'yyyy/MM/dd'), FORMAT(BANK_INFO.d_up,'yyyy/MM/dd')) as 更新日, " &
                     "       update_staf.l_name as 更新者 " &
                     "FROM ( " &
                     "     SELECT BANK_SEND.*, " &
                     "            BANK_SEND_MEM.bank_pay, " &
                     "            BANK_SEND_MEM.adjust, " &
                     "            BANK_SEND_MEM.pay_cut_total, " &
                     "            BANK_SEND_MEM.daily_pay_total " &
                     "     FROM " &
                     "     (select staf_bank_send.*, dtl1.l_name " &
                     "     from staf_bank_send, " &
                     "          constant_dtl AS dtl1 " &
                     "     where d_bank_send >= '" & strPayDayFrom & "' " &
                     "     and d_bank_send <= '" & strPayDayTo & "' " &
                     "     and dtl1.c_constant = 'BANK_SEND_MARGIN' " &
                     "     and dtl1.c_constant_seq = staf_bank_send.k_bank_send_margin " &
                     "     ) BANK_SEND, " &
                     "     ( SELECT " &
                     "        c_staf_bank_send_id, " &
                     "        d_bank_send, " &
                     "        SUM(s_bank_pay) as bank_pay, " &
                     "        SUM(s_adjust) as adjust, " &
                     "        SUM(s_pay_cut_total) as pay_cut_total, " &
                     "        SUM(s_daily_pay_total) as daily_pay_total " &
                     " from staf_bank_send_member " &
                     " group by c_staf_bank_send_id, d_bank_send " &
                     ") BANK_SEND_MEM " &
                     "WHERE BANK_SEND.c_staf_bank_send_id = BANK_SEND_MEM.c_staf_bank_send_id  " &
                     "AND BANK_SEND.d_bank_send = BANK_SEND_MEM.d_bank_send) AS BANK_INFO " &
                     "LEFT JOIN " &
                     "staf_attribute_full_time_now_name_view AS update_staf " &
                     "ON IIF(BANK_INFO.c_user_id_up = '', BANK_INFO.c_user_id_ins, BANK_INFO.c_user_id_up) = update_staf.user_id " &
                     "ORDER BY BANK_INFO.d_bank_send "

            'DB接続開始
            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                '検索結果表示
                For Each dtRow As DataRow In dtRet.Rows
                    Me.dgdPayDay.Rows.Add()
                    Me.dgdPayDay(0, iCnt).Value = dtRow.Item("振込日")
                    Me.dgdPayDay(1, iCnt).Value = dtRow.Item("支払方法")
                    Me.dgdPayDay(2, iCnt).Value = dtRow.Item("題目")
                    Me.dgdPayDay(3, iCnt).Value = dtRow.Item("振込総額")
                    Me.dgdPayDay(4, iCnt).Value = dtRow.Item("源泉総額")
                    Me.dgdPayDay(5, iCnt).Value = dtRow.Item("日当総額")
                    Me.dgdPayDay(6, iCnt).Value = dtRow.Item("調整総額")
                    Me.dgdPayDay(7, iCnt).Value = dtRow.Item("更新日")
                    Me.dgdPayDay(8, iCnt).Value = dtRow.Item("更新者")
                    '振込ID
                    Me.dgdPayDay(9, iCnt).Value = dtRow.Item("c_staf_bank_send_id")
                    '支払方法コード
                    Me.dgdPayDay(10, iCnt).Value = dtRow.Item("k_bank_send_margin")
                    '支払方法名称
                    Me.dgdPayDay(11, iCnt).Value = dtRow.Item("支払方法")
                    iCnt = iCnt + 1
                Next
            End If

            '検索結果件数表示
            Call Me.SetPayDaySearchResultCount()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
            clsDb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

#End Region

#Region "個人別振込検索処理"
    '***************************************************************************************************
    '   ＩＤ　：SearchPersonalPay
    '   名称　：個人別振込検索処理
    '   概要　：個人別振込検索処理
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SearchPersonalPay()
        Dim dtRet As DataTable = Nothing
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim iCnt As Integer = 0
        Dim strPayDay As String = Me.cmbPersonalPayYear.Text
        If ChkNumber(Me.cmbPersonalPayMonth.Text) = True Then
            strPayDay = strPayDay & Me.cmbPersonalPayMonth.Text
        End If
        If ChkNumber(Me.cmbPersonalPayDay.Text) = True Then
            strPayDay = strPayDay & Me.cmbPersonalPayDay.Text
        End If

        Dim strPayDayFrom As String = strPayDay.PadRight(8, "0")
        Dim strPayDayTo As String = strPayDay.PadRight(8, "9")
        Dim strUserId As String = Me.txtPersonalPayStafId.Text.Trim

        Try
            Me.Cursor = Cursors.WaitCursor
            '検索結果を一旦クリア
            Me.dgdPersonalPay.Rows.Clear()
            If Me.grpPersonalPayResult.Visible = False Then
                Me.grpPersonalPayResult.Visible = True
            End If

            strSql = "SELECT BANK_SEND_MEM.c_staf_bank_send_id, " &
                     "       " & UtDb.DbStrYYYYMMDDtoDateText("BANK_SEND_MEM.d_bank_send") & " as 振込日, " &
                     "       BANK_SEND.k_bank_send_margin, " &
                     "       BANK_SEND.l_name as 支払方法, " &
                     "       BANK_SEND.l_bank_send_item as 題目, " &
                     "       BANK_SEND_MEM.s_bank_pay as 振込金額, " &
                     "       BANK_SEND_MEM.s_adjust as 調整金額, " &
                     "       FORMAT(BANK_SEND_MEM.d_ins,'yyyy/MM/dd') as 更新日, " &
                     "       BANK_SEND_MEM.update_staf_name as 更新者 " &
                     "FROM " &
                     "(SELECT member.*,update_staf.l_name AS update_staf_name " &
                     " FROM staf_bank_send_member AS member, " &
                     "      staf_attribute_full_time_now_name_view AS update_staf " &
                     " WHERE member.c_user_id_ins = update_staf.user_id " &
                     " AND   member.d_bank_send >= '" & strPayDayFrom & "' " &
                     " AND   member.d_bank_send <= '" & strPayDayTo & "' " &
                     " AND   member.c_user_id = '" & strUserId & "' " &
                     ") AS BANK_SEND_MEM " &
                     " LEFT JOIN " &
                     "(SELECT bank.*,dtl1.l_name " &
                     " FROM staf_bank_send AS bank, " &
                     "      constant_dtl AS dtl1 " &
                     " WHERE dtl1.c_constant = 'BANK_SEND_MARGIN' AND dtl1.c_constant_seq = bank.k_bank_send_margin " &
                     ") AS BANK_SEND " &
                     "ON BANK_SEND.c_staf_bank_send_id = BANK_SEND_MEM.c_staf_bank_send_id " &
                     "AND BANK_SEND.d_bank_send =BANK_SEND_MEM.d_bank_send " &
                     "ORDER BY BANK_SEND_MEM.d_bank_send "
            'DB接続
            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                For Each dtRow As DataRow In dtRet.Rows
                    Me.dgdPersonalPay.Rows.Add()
                    Me.dgdPersonalPay(0, iCnt).Value = dtRow.Item("振込日")
                    Me.dgdPersonalPay(1, iCnt).Value = dtRow.Item("支払方法")
                    Me.dgdPersonalPay(2, iCnt).Value = dtRow.Item("題目")
                    Me.dgdPersonalPay(3, iCnt).Value = dtRow.Item("振込金額")
                    Me.dgdPersonalPay(4, iCnt).Value = dtRow.Item("調整金額")
                    Me.dgdPersonalPay(5, iCnt).Value = dtRow.Item("更新日")
                    Me.dgdPersonalPay(6, iCnt).Value = dtRow.Item("更新者")
                    '振込ID
                    Me.dgdPersonalPay(7, iCnt).Value = dtRow.Item("c_staf_bank_send_id")
                    '支払方法
                    Me.dgdPersonalPay(8, iCnt).Value = dtRow.Item("k_bank_send_margin")
                    '支払方法名称
                    Me.dgdPersonalPay(9, iCnt).Value = dtRow.Item("支払方法")
                    iCnt = iCnt + 1
                Next
            End If

            '検索結果件数表示
            Call Me.SetPersonalSearchResultCount()

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
            'DB切断処理
            clsDb.Disconnect()
        End Try
    End Sub

#End Region

#Region "社員番号より組合員の取得"
    '***************************************************************************************************
    '   ＩＤ　：GetUnionMemberData
    '   名称　：社員番号より組合員の取得
    '   概要　：受け取った社員番号の組合員情報をDataTable型で返却します
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '         ：2012/08/15(水) Fujisaku 参照時にはステータスチェックを行わない
    '***************************************************************************************************
    Private Function GetUnionMemberData(ByVal strUserId As String) As DataTable
        'DB接続クラス
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        '登録用SQL
        Dim strSql As String = String.Empty
        'SQL結果取得
        Dim dtRet As DataTable = Nothing
        '現在日付をスラッシュを除いた形で取得
        Dim strDateNow As String = Now.ToString("yyyyMMdd")

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
            'DB接続開始
            clsDb.Connect()
            strSql = ""
            strSql = strSql & " SELECT attr1.c_user_id AS c_user_id, attr1.c_staf_id AS c_staf_id, attr1.l_name AS l_name " & vbCrLf
            strSql = strSql & "      ,dtl1.l_name AS arealocal, dtl2.l_name AS model, dtl3.l_omission_name AS qualification " & vbCrLf
            strSql = strSql & "      ,dtl4.l_name AS belonging " & vbCrLf
            strSql = strSql & "      ,dtl1.l_omission_name AS local_omission, dtl2.l_omission_name AS model_omission " & vbCrLf
            strSql = strSql & "   FROM staf_attribute AS attr1, " & vbCrLf
            strSql = strSql & "        ( SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
            strSql = strSql & "            FROM staf_attribute " & vbCrLf
            strSql = strSql & "           WHERE d_from <= '" & strDateNow & "' " & vbCrLf '現在日以前の最新のユーザー情報
            strSql = strSql & "           GROUP BY c_user_id, c_ksh, c_staf_id ) AS attr2, " & vbCrLf
            strSql = strSql & "        constant_dtl AS dtl1, " & vbCrLf
            strSql = strSql & "        constant_dtl AS dtl2, " & vbCrLf
            strSql = strSql & "        constant_dtl AS dtl3, " & vbCrLf
            strSql = strSql & "        constant_dtl AS dtl4 " & vbCrLf
            strSql = strSql & "  WHERE attr1.c_user_id = '" & strUserId & "' " & vbCrLf '検索対象の社員番号
            strSql = strSql & "    AND attr1.c_user_id = attr2.c_user_id  " & vbCrLf
            strSql = strSql & "    AND attr1.c_staf_id = attr2.c_staf_id  " & vbCrLf
            strSql = strSql & "    AND attr1.c_ksh = attr2.c_ksh " & vbCrLf
            strSql = strSql & "    AND attr1.d_from = attr2.now_from " & vbCrLf
            strSql = strSql & "    AND dtl1.c_constant = 'AREA_LOCAL' AND dtl1.c_constant_seq = attr1.k_local " & vbCrLf
            strSql = strSql & "    AND dtl2.c_constant = 'MODEL' AND dtl2.c_constant_seq = attr1.k_model " & vbCrLf
            strSql = strSql & "    AND dtl3.c_constant = 'QUALIFICATION' AND dtl3.c_constant_seq = attr1.k_qualification " & vbCrLf
            strSql = strSql & "    AND dtl4.c_constant = 'BELONGING' AND dtl4.c_constant_seq = attr1.k_belonging " & vbCrLf
            strSql = strSql & "ORDER BY CLng(attr1.c_user_id) "

            dtRet = clsDb.ExecuteSql(strSql)
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try

        Return dtRet
    End Function

#End Region

#Region "社員名表示処理"
    '***************************************************************************************************
    '   ＩＤ　：SetMemberName
    '   名称　：社員名表示処理
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetMemberName()
        Dim dtUser As DataTable = Nothing
        Dim strStafId As String = String.Empty
        strStafId = Me.txtPersonalPayStafId.Text.Trim

        If ChkNull(strStafId) = True Then
            '社員番号に入力がない場合、名前もクリア
            Me.lblPersonalPayStafName.Text = String.Empty
        Else
            '組合員情報の取得
            dtUser = GetUnionMemberData(strStafId)
            If dtUser.Rows.Count > 0 Then
                Me.lblPersonalPayStafName.Text = dtUser.Rows(0).Item("l_name").ToString
            Else
                '情報が取得できなかった場合、社員番号と名前をクリア
                Me.txtPersonalPayStafId.Text = String.Empty
                Me.lblPersonalPayStafName.Text = String.Empty
                CLMsg.Show("GI0031", strStafId)
            End If
        End If
    End Sub
#End Region

#Region "コンボボックス作成"
    '***************************************************************************************************
    '   ＩＤ　：SetComboList
    '   名称　：コンボボックス作成
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetComboList()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        '各年のコンボボックスへ値を格納
        Call Me.SetYearCombo()
        '各月のコンボボックスへ値を格納
        Call Me.SetMonthCombo()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

    '***************************************************************************************************
    '   ＩＤ　：SetYearCombo
    '   名称　：各年のコンボボックス作成
    '   概要　：
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetYearCombo()
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim dtRetCloseYear As DataTable = Nothing
        Dim dtRetPayYear As DataTable = Nothing
        Dim strYearList As List(Of String) = New List(Of String)
        Dim strPayYearList As List(Of String) = New List(Of String)
        Try
            clsDb.Connect()
            strSql = "SELECT DISTINCT YEARS.d_years " & _
                     "FROM (select distinct Mid(d_daily_pay_close, 1, 4) as d_years from daily_pay_close " & _
                     "UNION ALL " & _
                     "select distinct format(d_years, 'yyyy') as d_years_cut from taxation_total ) YEARS " & _
                     "ORDER BY YEARS.d_years "

            dtRetCloseYear = clsDb.ExecuteSql(strSql)
            If dtRetCloseYear.Rows.Count > 0 Then
                For Each dtRow As DataRow In dtRetCloseYear.Rows
                    If strYearList.Contains(dtRow.Item("d_years")) = False Then
                        strYearList.Add(dtRow.Item("d_years"))
                    End If
                Next
            End If
            '振込状況確認、データ作成
            Me.cmbMakeDataYearFrom.Items.AddRange(strYearList.ToArray)
            Me.cmbMakeDataYearTo.Items.AddRange(strYearList.ToArray)
            Me.cmbMakeDataYearFrom.Text = Now.Date.Year.ToString
            Me.cmbMakeDataYearTo.Text = Now.Date.Year.ToString

            strSql = "SELECT DISTINCT Mid(d_bank_send, 1, 4) AS years FROM staf_bank_send "
            dtRetPayYear = clsDb.ExecuteSql(strSql)
            If dtRetPayYear.Rows.Count > 0 Then
                For Each dtRow As DataRow In dtRetPayYear.Rows
                    If strPayYearList.Contains(dtRow.Item("years")) = False Then
                        strPayYearList.Add(dtRow.Item("years"))
                    End If
                Next
            End If

            '振込日検索、個人別振込検索
            Me.cmbPayDayYear.Items.AddRange(strPayYearList.ToArray)
            Me.cmbPayDayYear.Text = Now.Date.Year.ToString
            Me.cmbPersonalPayYear.Items.AddRange(strPayYearList.ToArray)
            Me.cmbPersonalPayYear.Text = Now.Date.Year.ToString
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()
        End Try
    End Sub

    Private Sub SetMonthCombo()
        '振込状況確認、データ作成タブ開始側月コンボボックス
        CreateComboBoxMM(Me.cmbMakeDataMonthFrom)
        '振込状況確認、データ作成タブ終了側月コンボボックス
        CreateComboBoxMM(Me.cmbMakeDataMonthTo)
        If Me.cmbPayDayYear.Items.Count > 0 Then
            '振込日検索タブ月コンボボックス
            CreateComboBoxMM(Me.cmbPayDayMonth)
        End If
        If Me.cmbPersonalPayYear.Items.Count > 0 Then
            '個人別振込検索タブ月コンボボックス
            CreateComboBoxMM(Me.cmbPersonalPayMonth)
        End If
    End Sub

#End Region

#Region "振込日検索-詳細表示"
    '***************************************************************************************************
    '   ＩＤ　：ShowPersonalPayDtl
    '   名称　：振込日検索-詳細表示
    '   概要　：
    '   作成日：2012/02/21(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ShowPayDayDtl()
        Dim pn As Panel
        Dim clsUC080104 As UC080104

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
            If Me.dgdPayDay.SelectedRows.Count < 1 Then
                CLMsg.Show("GE0010", "一覧からデータ")
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor

            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            clsUC080104 = pn.Controls(SCREEN_ID_UC080104)
            If clsUC080104 Is Nothing Then
                clsUC080104 = New UC080104
                clsUC080104.intClickBtnFlg = 1
                '振込ID、振込日、支払方法、題目を渡す
                clsUC080104.strStafBankSendId = Me.dgdPayDay.SelectedRows(0).Cells(9).Value
                clsUC080104.datePayDay = CDate(Me.dgdPayDay.SelectedRows(0).Cells(0).Value).Date
                clsUC080104.strPayStatus = Me.dgdPayDay.SelectedRows(0).Cells(11).Value
                clsUC080104.strPayStatusCd = Me.dgdPayDay.SelectedRows(0).Cells(10).Value
                clsUC080104.strTitle = Me.dgdPayDay.SelectedRows(0).Cells(2).Value
                Call pn.Controls.Add(clsUC080104)
            Else
                clsUC080104.intClickBtnFlg = 1
                '振込ID、振込日、支払方法種別、題目を渡す
                clsUC080104.strStafBankSendId = Me.dgdPayDay.SelectedRows(0).Cells(9).Value
                clsUC080104.datePayDay = CDate(Me.dgdPayDay.SelectedRows(0).Cells(0).Value).Date
                clsUC080104.strPayStatus = Me.dgdPayDay.SelectedRows(0).Cells(11).Value
                clsUC080104.strPayStatusCd = Me.dgdPayDay.SelectedRows(0).Cells(10).Value
                clsUC080104.strTitle = Me.dgdPayDay.SelectedRows(0).Cells(2).Value
            End If
            Me.Visible = False '労金データ検索画面非表示
            pn.Visible = True '労金データ新規作成画面表示
        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        End Try

    End Sub

#End Region

#Region "個人別振込検索-詳細表示"
    '***************************************************************************************************
    '   ＩＤ　：ShowPersonalPayDtl
    '   名称　：個人別振込検索-詳細表示
    '   概要　：
    '   作成日：2012/02/21(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/21(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ShowPersonalPayDtl()
        Dim pn As Panel
        Dim clsUC080104 As UC080104

        Try
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
            Me.Cursor = Cursors.WaitCursor

            If Me.dgdPersonalPay.SelectedRows.Count < 1 Then
                CLMsg.Show("GE0010", "一覧からデータ")
                Exit Sub
            End If


            pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
            clsUC080104 = pn.Controls(SCREEN_ID_UC080104)
            If clsUC080104 Is Nothing Then
                clsUC080104 = New UC080104
                clsUC080104.intClickBtnFlg = 1
                '振込ID、振込日、支払方法、題目を渡す
                clsUC080104.strStafBankSendId = Me.dgdPersonalPay.SelectedRows(0).Cells(7).Value
                clsUC080104.datePayDay = CDate(Me.dgdPersonalPay.SelectedRows(0).Cells(0).Value).Date
                clsUC080104.strPayStatus = Me.dgdPersonalPay.SelectedRows(0).Cells(9).Value
                clsUC080104.strPayStatusCd = Me.dgdPersonalPay.SelectedRows(0).Cells(8).Value
                clsUC080104.strTitle = Me.dgdPersonalPay.SelectedRows(0).Cells(2).Value
                Call pn.Controls.Add(clsUC080104)
            Else
                clsUC080104.intClickBtnFlg = 1
                '振込ID、振込日、支払方法種別、題目を渡す
                clsUC080104.strStafBankSendId = Me.dgdPersonalPay.SelectedRows(0).Cells(7).Value
                clsUC080104.datePayDay = CDate(Me.dgdPersonalPay.SelectedRows(0).Cells(0).Value).Date
                clsUC080104.strPayStatus = Me.dgdPersonalPay.SelectedRows(0).Cells(9).Value
                clsUC080104.strPayStatusCd = Me.dgdPersonalPay.SelectedRows(0).Cells(8).Value
                clsUC080104.strTitle = Me.dgdPersonalPay.SelectedRows(0).Cells(2).Value
            End If
            Me.Visible = False '労金データ検索画面非表示
            pn.Visible = True '労金データ新規作成画面表示

        Catch ex As Exception
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
            ' 致命的エラーメッセージボックス表示
            Call CLMsg.ShowEtarnal(Err.Number, _
                                   Err.Description, _
                                   SCREEN_ID, SCREEN_NAME, _
                                   System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        End Try

    End Sub
#End Region

#Region "振込日検索結果件数更新"
    '***************************************************************************************************
    '   ＩＤ　：SetPayDaySearchResultCount
    '   名称　：振込日検索結果件数更新
    '   概要　：振込日検索結果件数更新
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetPayDaySearchResultCount()
        Me.grpPayDayResult.Text = String.Format(SEARCH_RESULT_COUNT, Me.dgdPayDay.Rows.Count.ToString)
    End Sub

#End Region

#Region "個人別振込情報検索結果件数更新"
    '***************************************************************************************************
    '   ＩＤ　：SetPersonalSearchResultCount
    '   名称　：個人別振込情報検索結果件数更新
    '   概要　：個人別振込情報検索結果件数更新
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetPersonalSearchResultCount()
        Me.grpPersonalPayResult.Text = String.Format(SEARCH_RESULT_COUNT_PERSONAL, Me.dgdPersonalPay.Rows.Count.ToString)
    End Sub

#End Region

#Region "締め日大小チェック（データ作成タブ）"
    '***************************************************************************************************
    '   ＩＤ　：CheckMakeDataCondition
    '   名称　：締め日大小チェック（データ作成タブ）
    '   概要　：
    '   作成日：2012/02/23(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function CheckMakeDataCondition() As Boolean
        Dim blnNoError As Boolean = True
        Try
            Dim num As Integer = 0
            Dim num2 As Integer = 0
            '開始側締め日を取得
            Dim strFromYM As String = (Me.cmbMakeDataYearFrom.Text & Me.cmbMakeDataMonthFrom.Text).PadRight(6, "0"c)
            '終了側締め日を取得
            Dim strToYM As String = (Me.cmbMakeDataYearTo.Text & Me.cmbMakeDataMonthTo.Text).PadRight(6, "9"c)
            If (Integer.TryParse(strFromYM, num) AndAlso Integer.TryParse(strToYM, num2)) Then
                If (num > num2) Then
                    CLMsg.Show("GE0143")
                    blnNoError = False
                End If
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

        Return blnNoError
    End Function

#End Region

#Region "年コンボボックス未選択チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkSelectMonthOnly
    '   名称　：年コンボボックス未選択チェック
    '   概要　：検索実行時、年コンボボックスが未選択かつ月コンボボックスが選択状態であるかチェックします
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkSelectMonthOnly(ByVal cmbYear As ComboBox, ByVal cmbMonth As ComboBox) As Boolean
        Dim blnRet As Boolean = False
        If (String.IsNullOrEmpty(cmbYear.Text) AndAlso Not String.IsNullOrEmpty(cmbMonth.Text)) Then
            CLMsg.Show("GE0144")
            Return blnRet
        End If

        blnRet = True
        Return blnRet
    End Function
#End Region

#Region "振込状況、データ作成コントロール表示・非表示"
    '***************************************************************************************************
    '   ＩＤ　：ShowControls
    '   名称　：振込状況、データ作成コントロール表示・非表示
    '   概要　：振込状況、データ作成のコントロールの表示・非表示を切り替えます
    '   作成日：2012/02/06(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/06(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ShowMakeDataControls(ByVal blnShow As Boolean)
        '各種ラベル、検索条件以外のグループボックス非表示
        Me.lblNotice1.Visible = blnShow
        Me.lblNotice2.Visible = blnShow
        Me.lblNotice3.Visible = blnShow
        Me.grpMakeDataPayCutResult.Visible = blnShow
        Me.grpMakeDataOnceCutResult.Visible = blnShow
        Me.grpMakeDataDailyPayResult.Visible = blnShow
        Me.grpMakeData.Visible = blnShow
    End Sub
#End Region

#Region "振込日検索タブコントロール表示・非表示"
    '***************************************************************************************************
    '   ＩＤ　：ShowParDayControls
    '   名称　：振込状況、データ作成コントロール表示・非表示
    '   概要　：
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ShowPayDayControls()
        '検索条件グループボックス以外非表示
        Me.grpPayDayResult.Visible = False
        Me.dgdPayDay.Rows.Clear()
    End Sub
#End Region

#Region "個人別振込検索タブコントロール表示・非表示"
    '***************************************************************************************************
    '   ＩＤ　：ShowPersonalPayControls
    '   名称　：個人別振込検索タブコントロール表示・非表示
    '   概要　：
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ShowPersonalPayControls()
        '検索条件グループボックス以外非表示
        Me.grpPersonalPayResult.Visible = False
        Me.dgdPersonalPay.Rows.Clear()
    End Sub
#End Region

#Region "画面更新"
    '***************************************************************************************************
    '   ＩＤ　：RenewScreenInfo
    '   名称　：画面更新
    '   概要　：振込データ登録後、画面情報を再セットします
    '   作成日：2012/02/13(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/02/13(月) a.onuma  新規作成
    '***************************************************************************************************
    Public Sub RenewScreenInfo()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        'コンボボックスのデータクリア
        '振込状況確認、データ作成タブ
        Me.cmbMakeDataYearFrom.Items.Clear()
        Me.cmbMakeDataMonthFrom.DataSource = Nothing
        Me.cmbMakeDataYearTo.Items.Clear()
        Me.cmbMakeDataMonthTo.DataSource = Nothing
        '振込日検索タブ
        Me.cmbPayDayYear.Items.Clear()
        Me.cmbPayDayMonth.DataSource = Nothing
        Me.cmbPayDayDay.DataSource = Nothing
        '個人別振込検索
        Me.cmbPersonalPayYear.Items.Clear()
        Me.cmbPersonalPayMonth.DataSource = Nothing
        Me.cmbPersonalPayDay.DataSource = Nothing
        'コンボボックスを再セット
        Call Me.SetComboList()

        If Me.tbcNetBank.SelectedIndex = 0 Then
            '振込データ作成状況を再検索
            Call Me.SearchMakeData()
            '振込日検索、個人別振込検索の検索結果をクリア
            Call Me.ShowPayDayControls()
            Call Me.ShowPersonalPayControls()

        ElseIf Me.tbcNetBank.SelectedIndex = 1 Then
            '振込日検索
            Call Me.SearchPayDay()
            '振込データ状況、個人別振込検索の結果をクリア
            Call Me.ShowMakeDataControls(False)
            Call Me.ShowPersonalPayControls()
        Else
            '個人別振込検索
            Call Me.SearchPersonalPay()
            '振込データ状況、振込日検索の結果をクリア
            Call Me.ShowMakeDataControls(False)
            Call Me.ShowPayDayControls()
        End If

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub

#End Region

#Region "各グリッドの選択状態を解除"
    '***************************************************************************************************
    '   ＩＤ　：CurrentCellReset
    '   名称　：各グリッドの選択状態を解除
    '   概要　：各グリッドの選択状態を解除
    '   作成日：2012/05/02(水) Fujisaku
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/05/02(水) Fujisaku  新規作成
    '***************************************************************************************************
    Private Sub CurrentCellReset()
        ' 各グリッド全てクリア
        If Me.dgdPayCut.RowCount() > 0 Then
            Me.dgdPayCut.CurrentCell = Nothing
        End If
        If Me.dgdOnceCut.RowCount() > 0 Then
            Me.dgdOnceCut.CurrentCell = Nothing
        End If
        If Me.dgdDailyPayCommittee.RowCount() > 0 Then
            Me.dgdDailyPayCommittee.CurrentCell = Nothing
        End If
        If Me.dgdDailyPayBranch.RowCount() > 0 Then
            Me.dgdDailyPayBranch.CurrentCell = Nothing
        End If
        If Me.dgdDailyPayExecutive.RowCount() > 0 Then
            Me.dgdDailyPayExecutive.CurrentCell = Nothing
        End If
        If Me.dgdDailyPayDGM.RowCount() > 0 Then
            Me.dgdDailyPayDGM.CurrentCell = Nothing
        End If
    End Sub
#End Region

#End Region

End Class
#End Region
