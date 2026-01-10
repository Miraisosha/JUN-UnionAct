#Region "UC040402"
'===========================================================================================================
'   クラスＩＤ　　：UC040402
'   クラス名称　　：指名ストライキ－通告
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLAccessMdbMst
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.MDNameStrikeCommon
Imports UnionAct.GUI.Common

Public Class UC040402

    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    '一部解除済みフラグ
    Dim _blnNoRelease As Boolean = True
    'この文書に登録されている指名ストライキ者
    Private _dtMember As DataTable = Nothing
    '前回入力された社員番号
    Private _strPreUserId As String = String.Empty
    '参照権限
    Private _strGrantReference As String = String.Empty
    '登録権限
    Private _strGrantInsert As String = String.Empty
    '印刷権限
    Private _strGrantPrint As String = String.Empty
    'ファイル出力権限
    Private _strGrantFileOutput As String = String.Empty

    '現在時刻
    Private _dateTimeNow As DateTime = Now
    ''' <summary>
    ''' 選択した指名ストライキ文書の情報
    ''' </summary>
    ''' <remarks></remarks>
    Private _NameStrikeData As DataRow = Nothing
    ''' <summary>
    ''' 選択した争議行為の情報
    ''' </summary>
    ''' <remarks></remarks>
    Private _StrikeData As DataRow = Nothing
    ''' <summary>
    ''' 選択ボタンの値取得
    ''' </summary>
    ''' <remarks></remarks>
    Private _intSelectBtn As Integer = -1
    '0 = 新規通告　
    '2 = 通告文書詳細　
    '4 = 通告の一時保存文書詳細　

    '1 = 一部解除新規
    '3 = 一部解除文書詳細　
    '5 = 一部解除の一時保存文書詳細　

    ''' <summary>
    ''' 画面終了時の押下ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private _intFinishBtn As Integer = -1
    '0 = 登録確認
    '1 = 一時保存確認
    '2 = キャンセル
    '3 = 戻る

#Region "プロパティ"
    ''' <summary>
    ''' 指名ストライキ画面で選択したボタンの取得・返却
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IntClickBtnFlg() As Integer    'クリックボタン判別用
        Get
            Return _intSelectBtn
        End Get
        Set(ByVal value As Integer)
            _intSelectBtn = value
        End Set
    End Property

    ''' <summary>
    ''' 新規登録時に選択した争議行為データ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectStrikeData() As DataRow   '選択争議行為の情報取得用
        Get
            Return _StrikeData
        End Get
        Set(ByVal value As DataRow)
            _StrikeData = value
        End Set
    End Property

    ''' <summary>
    ''' 検索画面で選択した指名ストライキ文書データ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectNameStrikeData() As DataRow
        Get
            Return _NameStrikeData
        End Get
        Set(ByVal value As DataRow)
            _NameStrikeData = value
        End Set
    End Property

#End Region

#Region "イベント"

#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UC040402_Load
    '   名称　：フォームロード
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub UC040402_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtGrant As DataTable = Nothing
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            '権限取得
            dtGrant = getGrant(MENU_ID_UC040401)
            If dtGrant.Rows.Count > 0 Then
                _strGrantReference = dtGrant.Rows(0).Item(3).ToString  '参照権限
                _strGrantInsert = dtGrant.Rows(0).Item(4).ToString     '登録権限
                _strGrantPrint = dtGrant.Rows(0).Item(5).ToString      '印刷権限
                _strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString 'ファイル出力権限
            End If

            '初期表示処理
            If _intSelectBtn = 0 Then '新規通告
                Call Me.InitializeNew()
            Else '照会
                If _intSelectBtn = 2 Then
                    'この文書に登録されている組合員を取得
                    _dtMember = GetBelongUnionMember(_NameStrikeData.Item("通告番号"))
                    '一部解除されている文書か検索
                    _blnNoRelease = ChkExistRelease()
                ElseIf _intSelectBtn = 4 Then
                    'この文書に登録されている組合員を取得
                    _dtMember = GetBelongUnionMemberWork(_NameStrikeData.Item("インデックス"))
                End If
                Call Me.InitializeReference()
                If _intSelectBtn = 4 Then '一時保存データの照会（初回のみの設定なのでロード時に実行）
                    '「一時保存文書」ラベルの表示
                    Me.lblNoticeWork.Visible = True
                    'バックカラー変更
                    Me.GroupBox1.BackColor = Color.PapayaWhip
                    Me.lblCount.BackColor = Color.PapayaWhip
                    Me.lblNoticeWork.BackColor = Color.PapayaWhip
                    Me.lblNoticeKind.BackColor = Color.PapayaWhip
                End If
            End If

            'ボタンの使用制御
            Call Me.EnableButtons()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "UC040402_Load")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "戻るボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnReturn_Click
    '   名称　：戻るボタンクリック
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnReturn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        '押下ボタンのフラグをたてる
        _intFinishBtn = 3
        '画面終了処理
        Call Me.FormClose()
    End Sub
#End Region

#Region "印刷ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：印刷ボタンクリック
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P2 = New DS0404P2()
        Dim fmPreview As FM000203 = New FM000203
        Dim strOfficerName As String = String.Empty

        Me.Cursor = Cursors.WaitCursor
        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 3
        fmPreview.PrintCntVisible = True
        reportObj = New CR0404P2
        fmPreview.ObjResource = reportObj

        '組合員情報部分
        Dim drDetail As DS0404P2.dtDetailRow
        'レポート上部
        Dim drHeader As DS0404P2.dtHeaderRow = ds.dtHeader.NewRow
        '更新回数
        Dim drFooter As DS0404P2.dtFooterRow = ds.dtFooter.NewRow

        drHeader.BeginEdit()
        '社長名
        strOfficerName = GetPresidentName()
        drHeader.president_name = strOfficerName
        Dim strStrikeList As List(Of String) = New List(Of String)
        strStrikeList.AddRange(Me.txtStrikeNumber.Text.Split("-"))
        Dim strNameStrikeList As List(Of String) = New List(Of String)
        strNameStrikeList.AddRange(_NameStrikeData.Item("通告番号").ToString.Split("-"))
        '通告番号種別（Bのみ）
        drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        'タイトルの２４時間を表示するように2012/06/13 START
        If Me.opt24Frame.Checked Then
            drHeader.k_time_frame = "02"
        End If
        If Me.opt72Frame.Checked Then
            drHeader.k_time_frame = "01"
        End If
        'タイトルの２４時間を表示するように2012/06/13 END
        If _intSelectBtn = 2 Then
            '期
            drHeader.period_id = strNameStrikeList(0)
            '通告番号（連番）
            drHeader.name_strike_info = strNameStrikeList(1)
        ElseIf _intSelectBtn = 4 Then
            '期
            drHeader.period_id = MDLoginInfo.Period.ToString
            '通告番号（連番）
            drHeader.name_strike_info = Me.txtNoticeNumber.Text.Trim
        End If
        '日付
        'NULL値対応 2012/06/14 START
        If Me.txtNoticeDate.Text <> "" Then
            If IsDate(Me.txtNoticeDate.Text) Then
                drHeader.info = DateValue(Me.txtNoticeDate.Text)
            End If
        End If
        'drHeader.info = DateValue(Me.txtNoticeDate.Text)
        'NULL値対応 2012/06/14 END
        '代表者
        drHeader.l_stand_name = Me.txtApply.Text.Trim
        '通告番号種別（Bのみ）
        drHeader.strike_strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '争議行為ID（期ID側）
        drHeader.strike_period_id = strStrikeList(0)
        '争議行為ID（連番側）
        drHeader.strike_name_strike_info = strStrikeList(1)
        '開始日時
        drHeader.operation_from = Me.dtpStartDate.Value.Year & " 年  " & Me.dtpStartDate.Value.Month & " 月  " & _
                                  Me.dtpStartDate.Value.Day & " 日  " & Me.txtStartTime.Text.Trim & " 時 "
        '終了日時
        drHeader.operation_to = Me.dtpEndDate.Value.Year & " 年  " & Me.dtpEndDate.Value.Month & " 月  " & _
                                Me.dtpEndDate.Value.Day & " 日  " & Me.txtEndTime.Text.Trim & " 時 "
        '規模
        drHeader.time_frame = Me.txtScale.Text.Trim
        drHeader.EndEdit()

        drFooter.BeginEdit()
        '更新回数
        drFooter.up = Me.lblCount.Text
        drFooter.EndEdit()

        ds.dtHeader.Rows.Add(drHeader) 'ヘッダー情報格納
        ds.dtFooter.Rows.Add(drFooter) 'フッター情報格納
        For intCnt As Integer = 0 To Me.dgdStrikeMember.Rows.Count - 1
            If ChkNull(Me.dgdStrikeMember.Rows(intCnt).Cells(0).Value) = False Then
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                '社員番号
                drDetail.c_staf_id = dgdStrikeMember.Rows(intCnt).Cells(0).Value
                '氏名
                drDetail.l_name = dgdStrikeMember.Rows(intCnt).Cells(1).Value
                '会社所属省略名
                drDetail.local_name = dgdStrikeMember.Rows(intCnt).Cells(5).Value
                '機種省略名
                drDetail.k_model = dgdStrikeMember.Rows(intCnt).Cells(6).Value
                '資格
                drDetail.k_qualification = dgdStrikeMember.Rows(intCnt).Cells(4).Value
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
            End If
        Next
        Dim intRowCnt As Integer = ds.dtDetail.Rows.Count
        Dim intRest As Integer = (intRowCnt Mod 40)
        If intRest > 0 Then
            Dim intQuotient As Integer = System.Math.Floor(intRowCnt / 40)
            Do While (intRowCnt < 40 * (intQuotient + 1))
                drDetail = ds.dtDetail.NewRow
                ds.dtDetail.Rows.Add(drDetail)
                intRowCnt = intRowCnt + 1
            Loop
        End If
        'データソースセット
        reportObj.SetDataSource(ds)

        Call fmPreview.ShowDialog()
        Select Case fmPreview.IntQlickBtnFlag
            Case 2
                'キャンセルばの場合何も行わない
            Case 3
                '印刷ボタン押下時
                fmPreview.PrintOut()
        End Select
        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "一時保存ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnWork_Click
    '   名称　：一時保存ボタンクリック
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnWork_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWork.Click
        Me.Cursor = Cursors.WaitCursor
        '押下ボタンのフラグをたてる
        _intFinishBtn = 1
        '一時保存処理
        Call Me.NewUpdateNameStrikeWork()
        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "登録確認ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnConfirm_Click
    '   名称　：登録確認ボタンクリック
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Me.Cursor = Cursors.WaitCursor
        '各種データが正しく入力されているかチェック
        If ChkInput() = False Then
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        '押下ボタンのフラグをたてる
        _intFinishBtn = 0
        If _intSelectBtn = 0 OrElse _intSelectBtn = 4 Then
            '新規指名ストライキ登録
            Call Me.NewNameStrike()
        ElseIf _intSelectBtn = 2 Then
            '指名ストライキ更新
            Call Me.UpdateNameStrike()
        End If
        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "キャンセルボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If CLMsg.Show("GQ0007") = DialogResult.No Then
            'いいえが選択された場合は何も行わない
            Exit Sub
        End If

        If _intSelectBtn = 0 Then
            '押下ボタンのフラグをたてる
            _intFinishBtn = 2
            '新規通告で開かれた場合指名ストライキ検索画面に戻る
            '画面終了処理
            Call Me.FormClose()
        ElseIf _intSelectBtn = 2 OrElse _intSelectBtn = 4 Then
            'データ照会から開かれた場合初期表示状態に戻す
            Call Me.InitializeReference()
            If _strGrantPrint = GRANT_VALID Then
                Me.btnPrint.Enabled = True
            End If
        End If

    End Sub
#End Region

#Region "内容変更ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnChange_Click
    '   名称　：内容変更ボタンクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChange.Click
        Dim blnAdd As Boolean = True
        Dim intNum As Integer = 0

        '組合員の追加・削除、一時保存確認、登録確認、キャンセルボタン表示
        Me.btnAddMember.Visible = True
        Me.btnDeleteMember.Visible = True
        '一時保存データの照会時は一時保存ボタンも表示
        If _intSelectBtn = 4 Then
            Me.btnWork.Visible = True
        End If
        Me.btnConfirm.Visible = True
        Me.btnCancel.Visible = True

        '印刷、内容変更、戻るボタン非表示
        Me.btnPrint.Enabled = False
        Me.btnChange.Visible = False
        Me.btnReturn.Visible = False
        '各種オブジェクトを変更可能にする
        Call Me.EditChange(True)

        '新規入力可能行の存在チェック
        Do While (intNum < (Me.dgdStrikeMember.Rows.Count))
            If Me.dgdStrikeMember.Rows(intNum).Cells(0).Value Is Nothing OrElse _
               Me.dgdStrikeMember.Rows(intNum).Cells(0).Value.Equals("") Then
                '新規入力可能行がある場合は行追加は行わない
                blnAdd = False
                Exit Do
            End If
            intNum = intNum + 1
        Loop

        If blnAdd = True Then
            AddGridRow()
        End If
    End Sub
#End Region

#Region "組合員の追加ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnAddMember_Click
    '   名称　：組合員の追加ボタンクリック
    '   概要　：
    '   作成日：2012/01/15(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/15(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnAddMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddMember.Click
        '組合員抽出画面
        Dim fmUnion As FM000204 = New FM000204()
        Dim strUserIdList As List(Of String) = New List(Of String)

        If Me.dgdStrikeMember.Rows.Count > 0 Then
            Me.Cursor = Cursors.WaitCursor
            For Each dgRow As DataGridViewRow In Me.dgdStrikeMember.Rows
                '既に表示済みのユーザーIDを格納
                strUserIdList.Add(dgRow.Cells(0).Value)
            Next
            '初期表示するメンバー削除不可
            fmUnion.AllowDeleteMember = False
            'ユーザーIDリストを組合員抽出画面に渡す
            fmUnion.StafIDList = strUserIdList.ToArray()
        End If

        '組合員抽出画面の表示
        fmUnion.ShowDialog()
        Select Case fmUnion.IntQlickBtnFlag
            Case 0 'OKボタン押下時
                '選択された組合員のリスト
                Dim dt As DataTable = fmUnion.SelectMemberList

                For Each dtRow As DataRow In dt.Rows
                    If strUserIdList.Contains(dtRow.Item("社員番号")) = False Then
                        Me.dgdStrikeMember(0, Me.dgdStrikeMember.Rows.Count - 1).Value = dtRow.Item("社員番号")
                        Me.dgdStrikeMember(1, Me.dgdStrikeMember.Rows.Count - 1).Value = dtRow.Item("名前")
                        Me.dgdStrikeMember(2, Me.dgdStrikeMember.Rows.Count - 1).Value = dtRow.Item("会社所属")
                        Me.dgdStrikeMember(3, Me.dgdStrikeMember.Rows.Count - 1).Value = dtRow.Item("機種")
                        Me.dgdStrikeMember(4, Me.dgdStrikeMember.Rows.Count - 1).Value = dtRow.Item("資格")
                        Me.dgdStrikeMember(5, Me.dgdStrikeMember.Rows.Count - 1).Value = dtRow.Item("会社所属簡略")
                        Me.dgdStrikeMember(6, Me.dgdStrikeMember.Rows.Count - 1).Value = dtRow.Item("機種簡略")
                        Me.dgdStrikeMember(7, Me.dgdStrikeMember.Rows.Count - 1).Value = dtRow.Item("組合支部")
                        '一行追加
                        AddGridRow()
                    End If
                Next
            Case 1
                'キャンセルの場合何も行わない
        End Select
        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "組合員の削除ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnDeleteMember_Click
    '   名称　：組合員の削除ボタンクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnDeleteMember_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteMember.Click
        If Me.dgdStrikeMember.Rows.Count > 0 AndAlso Me.dgdStrikeMember.SelectedRows.Count > 0 Then
            If Me.dgdStrikeMember.SelectedRows(0).Cells(0).Value IsNot Nothing Then
                Me.Cursor = Cursors.WaitCursor
                Me.dgdStrikeMember.Rows.Remove(Me.dgdStrikeMember.SelectedRows(0))
                Me.Cursor = Cursors.Default
            End If
        End If
    End Sub
#End Region

#Region "闘争指令印刷ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnFightPrint_Click
    '   名称　：闘争指令印刷ボタンクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnFightPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFightPrint.Click
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P3 = New DS0404P3()
        Dim fmPreview As FM000203 = New FM000203

        Me.Cursor = Cursors.WaitCursor
        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 3
        fmPreview.PrintCntVisible = True
        reportObj = New CR0404P3
        fmPreview.ObjResource = reportObj

        'レポート上部
        Dim drHeader As DS0404P3.dtHeaderRow = ds.dtHeader.NewRow
        drHeader.BeginEdit()
        Dim strStrikeList As List(Of String) = New List(Of String)
        strStrikeList.AddRange(Me.txtStrikeNumber.Text.Split("-"))
        Dim strNameStrikeList As List(Of String) = New List(Of String)
        strNameStrikeList.AddRange(_NameStrikeData.Item("通告番号").ToString.Split("-"))
        '通告番号種別（Bのみ）
        drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '期
        drHeader.period_id = strNameStrikeList(0)
        '通告番号（連番）
        drHeader.name_strike_info = strNameStrikeList(1)
        '日付
        drHeader.info = _dateTimeNow.ToString.Substring(0, 10)
        '代表者
        drHeader.l_stand_name = Me.txtApply.Text.Trim
        '開始日時
        drHeader.operation_from = Me.dtpStartDate.Value.Year & " 年  " & Me.dtpStartDate.Value.Month & " 月  " & _
                                  Me.dtpStartDate.Value.Day & " 日  " & Me.txtStartTime.Text.Trim & " 時 "
        '終了日時
        drHeader.operation_to = Me.dtpEndDate.Value.Year & " 年  " & Me.dtpEndDate.Value.Month & " 月  " & _
                                Me.dtpEndDate.Value.Day & " 日  " & Me.txtEndTime.Text.Trim & " 時 "
        '規模
        drHeader.time_frame = Me.txtScale.Text.Trim
        '闘争指令番号
        drHeader.c_fight = Me.txtFightNumber.Text.Trim
        drHeader.EndEdit()
        ds.dtHeader.Rows.Add(drHeader)
        'データソースセット
        reportObj.SetDataSource(ds)

        Call fmPreview.ShowDialog()
        Select Case fmPreview.IntQlickBtnFlag
            Case 2
                'キャンセルの場合何も行わない
            Case 3
                '印刷ボタン押下時
                fmPreview.PrintOut()
        End Select
        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "開始時間ロストフォーカス"
    '***************************************************************************************************
    '   ＩＤ　：txtStartTime_Leave
    '   名称　：開始時間ロストフォーカス
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtStartTime_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStartTime.Leave
        '入力内容の不正チェック
        If ChkStrikeTime(Me.txtStartTime) = False Then
            Me.txtStartTime.BackColor = Color.LightSalmon
            Me.txtStartTime.Focus()
            CLMsg.Show("GE0021", "開始時刻")
        Else
            Me.txtStartTime.BackColor = Color.White
        End If
    End Sub
#End Region

#Region "終了時間ロストフォーカス"
    '***************************************************************************************************
    '   ＩＤ　：txtEndTime_Leave
    '   名称　：終了時間ロストフォーカス
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtEndTime_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEndTime.Leave
        '入力内容の不正チェック
        If ChkStrikeTime(Me.txtEndTime) = False Then
            Me.txtEndTime.BackColor = Color.LightSalmon
            Me.txtEndTime.Focus()
            CLMsg.Show("GE0021", "終了時刻")
        Else
            Me.txtEndTime.BackColor = Color.White
        End If
    End Sub
#End Region

#Region "開始時間テキスト変更時"
    '***************************************************************************************************
    '   ＩＤ　：txtStartTime_TextChanged
    '   名称　：開始時間テキスト変更時
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtStartTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtStartTime.TextChanged
        '入力値が空欄以外かつ数値の場合は規模を更新
        If ChkNull(Me.txtStartTime.Text.Trim) = False AndAlso _
           ChkNumber(Me.txtStartTime.Text.Trim) = True Then
            If (CInt(Me.txtStartTime.Text.Trim) >= 0) AndAlso (CInt(Me.txtStartTime.Text.Trim) <= 24) Then
                '規模更新
                Me.txtScale.Text = GetTimeSpan(Me.dtpStartDate.Value.Date, Me.txtStartTime.Text, _
                                               Me.dtpEndDate.Value.Date, Me.txtEndTime.Text)
            End If
        End If
    End Sub
#End Region

#Region "終了時間テキスト変更時"
    '***************************************************************************************************
    '   ＩＤ　：txtEndTime_TextChanged
    '   名称　：終了時間テキスト変更時
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub txtEndTime_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtEndTime.TextChanged
        '入力値が空欄以外かつ数値の場合は規模を更新
        If ChkNull(Me.txtEndTime.Text.Trim) = False AndAlso _
           ChkNumber(Me.txtEndTime.Text.Trim) = True Then
            If (CInt(Me.txtEndTime.Text.Trim) >= 0) AndAlso (CInt(Me.txtEndTime.Text.Trim) <= 24) Then
                '規模更新
                Me.txtScale.Text = GetTimeSpan(Me.dtpStartDate.Value.Date, Me.txtStartTime.Text, _
                                               Me.dtpEndDate.Value.Date, Me.txtEndTime.Text)
            End If
        End If

    End Sub
#End Region

#Region "グリッドセル編集完開始時イベント"
    '***************************************************************************************************
    '   ＩＤ　：dgdStrikeMember_CellBeginEdit
    '   名称　：グリッドセル編集開始時イベント
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdStrikeMember_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles dgdStrikeMember.CellBeginEdit
        If e.ColumnIndex = 0 Then
            _strPreUserId = Me.dgdStrikeMember(e.ColumnIndex, e.RowIndex).Value
        Else
            _strPreUserId = String.Empty
        End If
    End Sub
#End Region

#Region "グリッドセル編集完了イベント"
    '***************************************************************************************************
    '   ＩＤ　：dgdStrikeMember_CellValidated
    '   名称　：グリッドセル編集完了イベント
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub dgdStrikeMember_CellEndEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgdStrikeMember.CellEndEdit
        Dim strNowUserId As String = String.Empty
        Dim intEditColumn As Integer = e.ColumnIndex
        Dim intEditRow As Integer = e.RowIndex
        Dim intNowRow As Integer = 0

        If intEditColumn = 0 Then
            strNowUserId = Me.dgdStrikeMember(intEditColumn, intEditRow).Value
            If _strPreUserId = strNowUserId Then
                '入力値に変更がない場合処理終了
                Exit Sub
            End If
            If strNowUserId = String.Empty AndAlso _strPreUserId <> String.Empty Then
                CLMsg.Show("GE0021", "社員番号")
                Me.dgdStrikeMember(intEditColumn, intEditRow).Value = _strPreUserId
                Exit Sub
            End If

            If strNowUserId <> String.Empty Then
                For Each dgRow As DataGridViewRow In Me.dgdStrikeMember.Rows
                    If dgRow.Cells(0).Value IsNot Nothing Then
                        If intNowRow <> intEditRow AndAlso dgRow.Cells(0).Value.Equals(strNowUserId) Then
                            CLMsg.Show("GE0009", strNowUserId)
                            Me.dgdStrikeMember(intEditColumn, intEditRow).Value = _strPreUserId
                            Exit Sub
                        End If
                    End If
                    intNowRow = intNowRow + 1
                Next

                If SetUnionMemberData(intEditColumn, intEditRow) = True Then '入力された社員番号の表示処理
                    '行追加フラグが立っている場合、一行追加
                    AddGridRow()
                End If
            End If
        End If
    End Sub

#End Region

#Region "開始日付CloseUp"
    '***************************************************************************************************
    '   ＩＤ　：dtpStartDate_CloseUp
    '   名称　：開始日付CloseUp
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：2012/03/02(金) Fujisaku
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    ' 　　　　：2012/03/02(金) Fujisaku  LeaveをCloseUpに変更
    '***************************************************************************************************
    Private Sub dtpStartDate_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpStartDate.CloseUp

        '終了日付を強制的に変更
        Me.dtpEndDate.Value = Me.dtpStartDate.Value
        '規模更新
        Me.txtScale.Text = GetTimeSpan(Me.dtpStartDate.Value.Date, Me.txtStartTime.Text, _
                                       Me.dtpEndDate.Value.Date, Me.txtEndTime.Text)
    End Sub
#End Region
    
#Region "終了日付CloseUp"
    '***************************************************************************************************
    '   ＩＤ　：dtpEndDate_CloseUp
    '   名称　：終了日付CloseUp
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    ' 　　　　：2012/03/02(金) Fujisaku  LeaveをCloseUpに変更
    '***************************************************************************************************
    Private Sub dtpEndDate_CloseUp(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpEndDate.CloseUp
        '規模更新
        Me.txtScale.Text = GetTimeSpan(Me.dtpStartDate.Value.Date, Me.txtStartTime.Text, _
                                       Me.dtpEndDate.Value.Date, Me.txtEndTime.Text)

    End Sub
#End Region

#Region "開始日付Leave"
    '***************************************************************************************************
    '   ＩＤ　：dtpStartDate_Leave
    '   名称　：開始日付Leave
    '   概要　：
    '   作成日：2012/03/16(金) Fujisaku
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金) Fujisaku  Leaveを復活し内容を修正
    '***************************************************************************************************
    Private Sub dtpStartDate_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpStartDate.Leave

        '終了日付を強制的に変更
        Me.dtpEndDate.Value = Me.dtpStartDate.Value
        '規模更新
        Me.txtScale.Text = GetTimeSpan(Me.dtpStartDate.Value.Date, Me.txtStartTime.Text, _
                                       Me.dtpEndDate.Value.Date, Me.txtEndTime.Text)
    End Sub
#End Region

#Region "終了日付CloseUp"
    '***************************************************************************************************
    '   ＩＤ　：dtpEndDate_Leave
    '   名称　：終了日付Leave
    '   概要　：
    '   作成日：2012/03/16(金) Fujisaku
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/03/16(金) Fujisaku  Leaveを復活し内容を修正
    '***************************************************************************************************
    Private Sub dtpEndDate_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtpEndDate.Leave
        '規模更新
        Me.txtScale.Text = GetTimeSpan(Me.dtpStartDate.Value.Date, Me.txtStartTime.Text, _
                                       Me.dtpEndDate.Value.Date, Me.txtEndTime.Text)

    End Sub
#End Region

#End Region

#Region "関数"

#Region "一部解除存在チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkExistRelease
    '   名称　：一部解除存在チェック
    '   概要　：この文書が既に一部解除済みかチェックします
    '   作成日：2012/01/16(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkExistRelease() As Boolean
        Dim blnRet As Boolean = True
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim dtRet As DataTable = Nothing

        Try
            strSql = ""
            strSql = strSql & "SELECT c_name_strike_id FROM name_strike " & vbCrLf
            strSql = strSql & "WHERE c_really_name_strike_id = '" & _NameStrikeData.Item("通告番号") & "' " & vbCrLf

            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)

            If dtRet.Rows.Count > 0 Then
                blnRet = False
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "btnConfirm_Click")
            log.Fatal(ex.Message)
        Finally
            clsDb.Disconnect()
        End Try

        Return blnRet
    End Function

#End Region

#Region "必須入力チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkInput
    '   名称　：必須入力チェック
    '   概要　：必須項目がすべて入力されているチェックします
    '   作成日：2012/01/16(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkInput() As Boolean
        Dim blnRet As Boolean = False

        '***　必須入力チェック
        '開始時刻
        If ChkNull(Me.txtStartTime.Text.Trim) = True Then
            CLMsg.Show("GE0006", "開始時刻")
            Return blnRet
        End If

        '終了時刻
        If ChkNull(Me.txtEndTime.Text.Trim) = True Then
            CLMsg.Show("GE0006", "終了時刻")
            Return blnRet
        End If

        '時間枠
        If Me.opt72Frame.Checked = False AndAlso Me.opt24Frame.Checked = False Then
            CLMsg.Show("GE0006", "時間枠")
            Return blnRet
        End If

        '申請者
        If ChkNull(Me.txtApply.Text.Trim) = True Then
            CLMsg.Show("GE0006", "申請者")
            SetErr(Me.txtApply)
            Return blnRet
        Else
            Me.txtApply.BackColor = Color.White
        End If
        '***　必須入力チェック

        '申請者文字数チェック
        If MDNameStrikeCommon.LengthByte(Me.txtApply.Text) > 30 Then
            CLMsg.Show("GE0103")
            SetErr(Me.txtApply)
            Return blnRet
        Else
            Me.txtApply.BackColor = Color.White
        End If

        '備考欄
        If MDNameStrikeCommon.LengthByte(txtNote.Text) > 200 Then
            CLMsg.Show("GE0112", "備考は、", "200", "100")
            SetErr(Me.txtNote)
            Return blnRet
        Else
            Me.txtNote.BackColor = Color.White
        End If

        '組合員が未選択の場合
        If Me.dgdStrikeMember.Rows.Count = 1 Then
            If _intSelectBtn = 0 Then
                CLMsg.Show("GE0045", "組合員")
                Return blnRet
            ElseIf _intSelectBtn = 2 Then
                CLMsg.Show("GE0074")
                Return blnRet
            End If
        End If

        '時間関連エラーチェック
        If ChkTimeDataNotErr() = False Then
            Return blnRet
        End If

        blnRet = True
        Return blnRet
    End Function

    Private Function ChkTimeDataNotErr() As Boolean
        Dim blnRet As Boolean = False
        Dim dateStart As Date = Nothing
        Dim dateEnd As Date = Nothing
        '***　時間エラーチェック
        '開始時間と現在時間の時間差分
        Dim lSpan As Long = 0
        Dim startEndSpan As Long = 0

        If Me.dtpStartDate.Value.Date > Me.dtpEndDate.Value.Date Then
            CLMsg.Show("GE0032", "開始日", "終了日")
            Return blnRet
        End If
        '入力値取得0時、24時入力対応
        dateStart = Date.Parse(Me.dtpStartDate.Value.Date.ToString("yyyy/MM/dd") & " 00:00:00")
        If Me.txtStartTime.Text.Trim = "24" Then
            dateStart = dateStart.AddDays(1)
        Else
            dateStart = dateStart.AddHours(Me.txtStartTime.Text.Trim.PadLeft(2, "0"))
        End If

        dateEnd = Date.Parse(Me.dtpEndDate.Value.Date.ToString("yyyy/MM/dd") & " 00:00:00")
        If Me.txtEndTime.Text.Trim = "24" Then
            dateEnd = dateEnd.AddDays(1)
        Else
            dateEnd = dateEnd.AddHours(Me.txtEndTime.Text.Trim.PadLeft(2, "0"))
        End If

        lSpan = DateDiff(DateInterval.Hour, Now, dateStart)

        Dim limitVal As Integer

        If Me.opt24Frame.Checked Then
            '24時間枠の場合は24時間後以降
            limitVal = 24
            If lSpan <= limitVal Then
                If MDLoginInfo.CommitteeStatusFlg = 1 Then
                    CLMsg.Show("GE0080", limitVal.ToString)
                    Return blnRet
                Else
                    If Not CLMsg.Show("GW0037") = DialogResult.Yes Then
                        Return blnRet
                    End If
                End If
            End If
        ElseIf Me.opt72Frame.Checked Then
            limitVal = 72
            '72時間枠の場合は72時間後以降
            If lSpan <= limitVal Then
                CLMsg.Show("GE0080", limitVal.ToString)
                Return blnRet
            End If
        End If

        'Dim test = Date.Compare(dateStart, dateEnd)
        'test = -1がEndが大きい test = 0 は同じ test =1 はstartが大きい
        If dateStart > dateEnd Then
            CLMsg.Show("GE0032", "開始日", "終了日")
            Return blnRet
        End If

        startEndSpan = DateDiff(DateInterval.Day, dateStart, dateEnd)
        If startEndSpan >= 100 Then
            CLMsg.Show("GE0117")
            Return blnRet
        End If

        'エラー優先のためデータチェックを行う
        If ChkInsertDataNotErr(Me.dtpStartDate.Value.Date.ToString("yyyy/MM/dd"), Me.dtpEndDate.Value.Date.ToString("yyyy/MM/dd")) = False Then
            Return blnRet
        End If

        '確認メッセ―ジ
        If Me.opt24Frame.Checked AndAlso lSpan >= 72 Then
            If CLMsg.Show("GQ0022") = DialogResult.No Then
                Return blnRet
            End If
        End If

        If startEndSpan >= 30 Then
            If CLMsg.Show("GW0014") = DialogResult.No Then
                Return blnRet
            End If
        End If

        Return True
    End Function

    Private Function ChkInsertDataNotErr(ByVal pStrStart As String, ByVal pStrEnd As String) As Boolean
        Dim blnRet As Boolean = False
        Dim clsDb As New CLAccessMdb        ' データベースクラス生成（ローカルレプリカ）
        'Dim clsDbMst As New CLAccessMdbMst  ' データベースクラス生成（サーバデザインマスタ）
        Dim strSQL As String = ""
        Dim strSubSQLLatestAttr As String = ""
        Dim dtSqlResult As DataTable
        'Dim dtMSqlResult As DataTable
        Dim dtUseResult As DataTable

        Try
            Dim dateNow As Date = Now
            '組合員情報取得時は今日を基準に取得
            Dim standardDay As String = dateNow.ToString("yyyyMMdd")

            '開始日と終了日は24時入力可能となっており、本パラメータは24:00は次の日の00:00となっている。
            'ただし活動日の検索などは前日のyyyy/MM/ddとする。
            '（終了日を基準とする2010/02/02 24:00までは→2010/02/03 00:00として渡されるので02日とする為）


            '*** DB接続
            clsDb.Connect()
            'clsDbMst.Connect()

            '①組合大会に登録されているかチェック
            For intColCnt = 0 To Me.dgdStrikeMember.Rows.Count - 1
                If ChkNull(NVL(Me.dgdStrikeMember.Rows(intColCnt).Cells("Column1").Value)) Then
                    Exit For
                End If
                If NVL(Me.dgdStrikeMember.Rows(intColCnt).Cells("Column1").Value).Trim.Length > 0 Then
                    strSQL = ""
                    strSQL = "SELECT " & vbCrLf
                    strSQL = strSQL & " apply_member.c_strike_id AS 申告書番号, " & vbCrLf
                    strSQL = strSQL & " apply_member.d_strike AS 日付 " & vbCrLf
                    strSQL = strSQL & "FROM apply_strike_member_date AS apply_member , apply_strike AS apply " & vbCrLf
                    strSQL = strSQL & "WHERE (apply_member.k_cancel <> '' AND apply_member.k_cancel IS NOT NULL) " & vbCrLf
                    strSQL = strSQL & "AND apply_member.c_strike_id = apply.c_strike_id " & vbCrLf
                    strSQL = strSQL & "AND apply_member.k_apply_area = apply.k_apply_area " & vbCrLf
                    strSQL = strSQL & "AND apply_member.k_cancel = '0' " & vbCrLf
                    strSQL = strSQL & "AND apply.k_cancel = '0' " & vbCrLf
                    strSQL = strSQL & "AND apply.k_replace = '0' " & vbCrLf
                    strSQL = strSQL & "AND apply_member.c_staf_id = '" & Me.dgdStrikeMember.Rows(intColCnt).Cells("Column1").Value & "' " & vbCrLf
                    strSQL = strSQL & "AND apply_member.d_strike between '" & pStrStart & "' AND '" & pStrEnd & "' " & vbCrLf
                    strSQL = strSQL & " " & vbCrLf
                    dtSqlResult = clsDb.ExecuteSql(strSQL)
                    'dtMSqlResult = clsDbMst.ExecuteSql(strSQL)
                    'If dtSqlResult.Rows.Count > 0 Or dtMSqlResult.Rows.Count > 0 Then
                    If dtSqlResult.Rows.Count > 0 Then
                        '行数が多い方(同数ならローカル側)を使用 2015/03/30
                        Dim errMess As String
                        'If dtSqlResult.Rows.Count >= dtMSqlResult.Rows.Count Then
                        errMess = "氏名:" & Me.dgdStrikeMember.Rows(intColCnt).Cells("Column2").Value &
                                            "(通告書番号:" & dtSqlResult.Rows(0).Item("申告書番号") & ", 日付:" & dtSqlResult.Rows(0).Item("日付") & ")"
                        'Else
                        '    errMess = "氏名:" & Me.dgdStrikeMember.Rows(intColCnt).Cells("Column2").Value &
                        '                    "(通告書番号:" & dtMSqlResult.Rows(0).Item("申告書番号") & ", 日付:" & dtMSqlResult.Rows(0).Item("日付") & ")"
                        'End If
                        CLMsg.Show("GE0186", "時間内組合活動", errMess)
                        Return False
                    End If
                End If
            Next

            '②すでにストライキされているかチェック
            '③同一日で各組合支部の24時間枠の人数が７人を超えたかチェック
            'チェックする組合支部格納用
            Dim strBelongList As List(Of String) = New List(Of String)
            'Dim intBelongCnt As List(Of Integer) = New List(Of Integer)
            'Dim messageCnt = 0
            '社員名取得の貯めのサブクエリ
            strSubSQLLatestAttr = strSubSQLLatestAttr & "    SELECT " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      staf_attr.c_user_id AS 社員番号, " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      staf_attr.l_name AS 名前 , " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      cnst1.l_name AS 会社支部 , " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      cnst2.l_name AS 組合 " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "    FROM " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      staf_attribute AS staf_attr, " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      constant_dtl AS cnst1, " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      constant_dtl AS cnst2, " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      (SELECT c_user_id , max(d_from) AS new_from " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "       FROM staf_attribute  " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "       WHERE d_from <= '" & standardDay & "'" & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "       GROUP BY c_user_id " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      ) AS latest_attr  " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "    WHERE " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "      staf_attr.c_user_id = latest_attr.c_user_id " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "    AND staf_attr.d_from = latest_attr.new_from " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "    AND staf_attr.k_local = cnst1.c_constant_seq " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "    AND cnst1.c_constant = 'AREA_LOCAL' " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "    AND staf_attr.k_belonging = cnst2.c_constant_seq  " & vbCrLf
            strSubSQLLatestAttr = strSubSQLLatestAttr & "    AND cnst2.c_constant = 'BELONGING'  " & vbCrLf

            strSQL = " SELECT " & vbCrLf
            'strSQL = strSQL & "   IIF(fight_inf.k_time_frame = '1' , '*' , '' ) AS 24H, " & vbCrLf
            strSQL = strSQL & "   fight_inf.d_operation_from AS 開始日, " & vbCrLf
            strSQL = strSQL & "   fight_inf.d_operation_time_from AS 開始時間, " & vbCrLf
            strSQL = strSQL & "   fight_inf.d_operation_to AS 終了日, " & vbCrLf
            strSQL = strSQL & "   fight_inf.d_operation_time_to AS 終了時間, " & vbCrLf
            strSQL = strSQL & "   attr.社員番号 AS 社番, " & vbCrLf
            strSQL = strSQL & "   attr.名前 AS 氏名, " & vbCrLf
            strSQL = strSQL & "   attr.会社支部 AS 会社所属, " & vbCrLf
            strSQL = strSQL & "   attr.組合 AS 組合支部 , " & vbCrLf
            strSQL = strSQL & "   fight_inf.c_name_strike_id AS 通告番号, " & vbCrLf
            strSQL = strSQL & "   fight_inf.d_info AS 通告日時 " & vbCrLf
            'strSQL = strSQL & "   fight_updater.l_name AS 担当者, " & vbCrLf
            'strSQL = strSQL & "   fight_inf.c_fight AS 闘争指令, " & vbCrLf
            'strSQL = strSQL & "   name_strike_member_date.c_cancel_name_strike_id AS 解除番号, " & vbCrLf
            'strSQL = strSQL & "   name_strike_member_date.d_cancel_info AS 解除日時, " & vbCrLf
            'strSQL = strSQL & "   cancel_updater.l_name AS 解除担当者 , " & vbCrLf
            'strSQL = strSQL & "   cancel_inf.c_cancel AS 解除指令, " & vbCrLf
            'strSQL = strSQL & "   fight_inf.c_strike_id AS 争議行為ID, " & vbCrLf
            'strSQL = strSQL & "   fight_inf.c_user_id_up AS 担当者社番, " & vbCrLf
            'strSQL = strSQL & "   cancel_inf.c_user_id_up AS 解除担当者社番, " & vbCrLf
            'strSQL = strSQL & "   name_strike_member_date.l_biko AS 備考 " & vbCrLf
            strSQL = strSQL & " FROM ( ( ( (name_strike_member_date " & vbCrLf
            strSQL = strSQL & " INNER JOIN ( " & vbCrLf
            strSQL = strSQL & strSubSQLLatestAttr
            strSQL = strSQL & " ) AS attr " & vbCrLf
            strSQL = strSQL & " ON name_strike_member_date.c_user_id = attr.社員番号) " & vbCrLf
            strSQL = strSQL & " INNER JOIN ( " & vbCrLf

            strSQL = strSQL & " SELECT * FROM name_strike " & vbCrLf
            strSQL = strSQL & "  WHERE d_operation_from <= '" & pStrEnd & "' " & vbCrLf
            strSQL = strSQL & "  AND d_operation_to >= '" & pStrStart & "'  AND c_really_name_strike_id = '' " & vbCrLf
            If _intSelectBtn = 2 Then
                strSQL = strSQL & "AND c_name_strike_id <> '" & _NameStrikeData.Item("通告番号") & "' " & vbCrLf
            End If

            strSQL = strSQL & " ) AS fight_inf " & vbCrLf
            strSQL = strSQL & " ON name_strike_member_date.c_name_strike_id = fight_inf.c_name_strike_id " & vbCrLf
            strSQL = strSQL & " ) " & vbCrLf
            strSQL = strSQL & " LEFT JOIN name_strike AS cancel_inf " & vbCrLf
            strSQL = strSQL & " ON name_strike_member_date.c_cancel_name_strike_id = cancel_inf.c_name_strike_id " & vbCrLf
            strSQL = strSQL & " ) " & vbCrLf
            strSQL = strSQL & " LEFT JOIN staf_attribute_full_time_now_name_view AS fight_updater  " & vbCrLf
            strSQL = strSQL & " ON fight_inf.c_user_id_up = fight_updater.user_id " & vbCrLf
            strSQL = strSQL & " ) " & vbCrLf
            strSQL = strSQL & " LEFT JOIN staf_attribute_full_time_now_name_view AS cancel_updater  " & vbCrLf
            strSQL = strSQL & " ON cancel_inf.c_user_id_up = cancel_updater.user_id " & vbCrLf
            strSQL = strSQL & " WHERE name_strike_member_date.c_cancel_name_strike_id IS NULL OR name_strike_member_date.c_cancel_name_strike_id = '' "

            dtSqlResult = clsDb.ExecuteSql(strSQL)
            'dtMSqlResult = clsDbMst.ExecuteSql(strSQL)
            '対象組合員分だけループ
            For intColCnt = 0 To Me.dgdStrikeMember.Rows.Count - 1

                    If ChkNull(NVL(Me.dgdStrikeMember.Rows(intColCnt).Cells("Column1").Value)) Then
                        Exit For
                    End If

                    If Not ChkNull(NVL(Me.dgdStrikeMember.Rows(intColCnt).Cells("Column8").Value)) Then
                        Dim strSet As String = NVL(Me.dgdStrikeMember.Rows(intColCnt).Cells("Column8").Value)
                        If Not strBelongList.Contains(strSet) And strSet.Length > 0 Then
                            strBelongList.Add(strSet)
                        End If
                    End If

                '検索結果と比較
                '行数が多い方(同数ならローカル側)を使用 2015/03/30
                'If dtSqlResult.Rows.Count >= dtMSqlResult.Rows.Count Then
                dtUseResult = dtSqlResult
                'Else
                '    dtUseResult = dtMSqlResult
                'End If

                For Each getRow As DataRow In dtUseResult.Rows
                    If NVL(getRow.Item("社番")).ToString = NVL(Me.dgdStrikeMember.Rows(intColCnt).Cells("Column1").Value) Then
                        Dim dtStartDay As Date = Date.Parse(getRow.Item("開始日"))
                        Dim dtEndday As Date = Date.Parse(getRow.Item("終了日"))
                        Dim startEndSpan As Long = DateDiff(DateInterval.Day, dtStartDay, dtEndday)
                        '一致日がない異常パターンは一致した文書の通告日時を出す
                        Dim sameDay As String = getRow.Item("通告日時")

                        For intCnt = 0 To startEndSpan
                            Dim checkDay As Date = dtStartDay.AddDays(intCnt)
                            If checkDay >= Me.dtpStartDate.Value And checkDay <= Me.dtpEndDate.Value Then
                                sameDay = checkDay.ToString("yyyy/MM/dd")
                                Exit For
                            End If
                        Next

                        Dim errMess As String = vbCrLf & "氏名:" & Me.dgdStrikeMember.Rows(intColCnt).Cells("Column2").Value & _
                                             "(通告書番号:" & getRow.Item("通告番号") & ", 日付:" & sameDay & ")"
                        CLMsg.Show("GE0186", "指名ストライキ", errMess)
                        Return False
                    End If
                Next
            Next

            If Me.opt24Frame.Checked = True Then
                '③同一日に各組合支部の24時間枠の登録人数が7人を超えた場合
                Dim dateLoop As Long = DateDiff(DateInterval.Day, Date.Parse(pStrStart), Date.Parse(pStrEnd))
                Dim checkDate As Date = Nothing

                '日付でループ
                For cntDate = 0 To dateLoop
                    checkDate = Date.Parse(pStrStart).AddDays(cntDate)

                    strSQL = " SELECT " & vbCrLf
                    strSQL = strSQL & "   attr.社員番号 AS 社番, " & vbCrLf
                    strSQL = strSQL & "   attr.組合 AS 組合支部  " & vbCrLf
                    strSQL = strSQL & " FROM ( ( ( (name_strike_member_date " & vbCrLf
                    strSQL = strSQL & " INNER JOIN ( " & vbCrLf
                    strSQL = strSQL & strSubSQLLatestAttr
                    strSQL = strSQL & " ) AS attr " & vbCrLf
                    strSQL = strSQL & " ON name_strike_member_date.c_user_id = attr.社員番号) " & vbCrLf
                    strSQL = strSQL & " INNER JOIN ( " & vbCrLf

                    strSQL = strSQL & " SELECT * FROM name_strike " & vbCrLf
                    strSQL = strSQL & "  WHERE d_operation_from <= '" & checkDate.Date.ToString("yyyy/MM/dd") & "' " & vbCrLf
                    strSQL = strSQL & "  AND d_operation_to >= '" & checkDate.Date.ToString("yyyy/MM/dd") & "'  AND c_really_name_strike_id = '' " & vbCrLf
                    strSQL = strSQL & "  AND k_time_frame = '1' " & vbCrLf
                    If _intSelectBtn = 2 Then
                        strSQL = strSQL & "AND c_name_strike_id <> '" & _NameStrikeData.Item("通告番号") & "' " & vbCrLf
                    End If

                    strSQL = strSQL & " ) AS fight_inf " & vbCrLf
                    strSQL = strSQL & " ON name_strike_member_date.c_name_strike_id = fight_inf.c_name_strike_id " & vbCrLf
                    strSQL = strSQL & " ) " & vbCrLf
                    strSQL = strSQL & " LEFT JOIN name_strike AS cancel_inf " & vbCrLf
                    strSQL = strSQL & " ON name_strike_member_date.c_cancel_name_strike_id = cancel_inf.c_name_strike_id " & vbCrLf
                    strSQL = strSQL & " ) " & vbCrLf
                    strSQL = strSQL & " LEFT JOIN staf_attribute_full_time_now_name_view AS fight_updater  " & vbCrLf
                    strSQL = strSQL & " ON fight_inf.c_user_id_up = fight_updater.user_id " & vbCrLf
                    strSQL = strSQL & " ) " & vbCrLf
                    strSQL = strSQL & " LEFT JOIN staf_attribute_full_time_now_name_view AS cancel_updater  " & vbCrLf
                    strSQL = strSQL & " ON cancel_inf.c_user_id_up = cancel_updater.user_id " & vbCrLf
                    strSQL = strSQL & " WHERE name_strike_member_date.c_cancel_name_strike_id IS NULL OR name_strike_member_date.c_cancel_name_strike_id = '' "

                    dtSqlResult = clsDb.ExecuteSql(strSQL)

                    For Each belong As String In strBelongList
                        '今回登録メンバー
                        Dim intNewInsertCnt As Integer = 0
                        For intColCnt = 0 To Me.dgdStrikeMember.Rows.Count - 1
                            '同じ組合の場合はカウンターを+1
                            If Not ChkNull(NVL(Me.dgdStrikeMember.Rows(intColCnt).Cells("Column8").Value)) Then
                                If NVL(Me.dgdStrikeMember.Rows(intColCnt).Cells("Column8").Value).Trim = belong.Trim Then
                                    intNewInsertCnt = intNewInsertCnt + 1
                                End If
                            End If
                        Next
                        '新規メンバーだけで規定数超え
                        If intNewInsertCnt >= 7 Then
                            If CLMsg.Show("GW0004", checkDate.Date.ToString("yyyy/MM/dd"), belong) = DialogResult.Yes Then
                                Continue For
                            Else
                                Return False
                            End If
                        End If

                        For Each getRow As DataRow In dtSqlResult.Rows
                            If NVL(getRow.Item("組合支部")).Trim = belong.Trim Then
                                intNewInsertCnt = intNewInsertCnt + 1
                            End If
                            If intNewInsertCnt >= 7 Then
                                If CLMsg.Show("GW0004", checkDate.Date.ToString("yyyy/MM/dd"), belong) = DialogResult.Yes Then
                                    Exit For
                                Else
                                    Return False
                                End If
                            End If
                        Next
                    Next
                Next
            End If

            '正常終了を返却
            Return True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "btnConfirm_Click")
            log.Fatal(ex.Message)
        Finally
            clsDb.Disconnect()
            'clsDbMst.Disconnect()
        End Try

    End Function

#End Region

#Region "新規通告時の初期表示処理"
    '***************************************************************************************************
    '   ＩＤ　：InitializeNew
    '   名称　：新規通告時の初期表示処理
    '   概要　：画面の初期表示時の制御を行います
    '   作成日：2012/01/11(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/11(木) a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>
    ''' 新規通告時の初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeNew()
        '申請者
        Dim strName As String = GetUnionLeaderName()
        If strName <> String.Empty Then
            '組合長の名前を表示
            Me.txtApply.Text = UNION_LEADERNAME_HEADER & "　　" & strName
        End If

        '通告日
        Me.txtNoticeDate.Text = _dateTimeNow.ToString.Substring(0, 10)
        '通告番号種別
        Me.txtNoticeKind.Text = NOTICE_KIND
        '通告書番号
        Me.txtNoticeNumber.Text = NOTICE_NUMBER_UNDEFINE
        '争議行為通告番号
        Me.txtStrikeNumber.Text = _StrikeData.Item(2)
        '開始日付
        Me.dtpStartDate.Value = Now.Date
        '開始時間
        Me.txtStartTime.Text = "0"
        '終了日付
        Me.dtpEndDate.Value = Now.Date
        '終了時間
        Me.txtEndTime.Text = "24"
        '規模
        Me.txtScale.Text = GetTimeSpan(Me.dtpStartDate.Value.Date, Me.txtStartTime.Text, _
                                       Me.dtpEndDate.Value.Date, Me.txtEndTime.Text)


        '印刷、内容変更、戻るボタン非表示
        Me.btnPrint.Visible = False
        Me.btnChange.Visible = False
        Me.btnReturn.Visible = False

        'グリッドに一行追加
        AddGridRow()

    End Sub
#End Region

#Region "内容照会時の初期表示処理"
    '***************************************************************************************************
    '   ＩＤ　：InitializeReference
    '   名称　：内容照会時の初期表示処理
    '   概要　：ストライキ文書照会時の初期表示処理を行います
    '   作成日：2012/01/11(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/11(木) a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>
    ''' 内容照会時の初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeReference()
        Dim strUpdateCount As String = String.Empty

        If _intSelectBtn = 2 Then
            '通告日
            If _NameStrikeData.Item("通告日時").ToString.Length > 10 Then
                Me.txtNoticeDate.Text = _NameStrikeData.Item("通告日時").ToString.Substring(0, 10)
            Else
                Me.txtNoticeDate.Text = _NameStrikeData.Item("通告日時")
            End If
            '闘争指令番号
            Me.txtFightNumber.Text = _NameStrikeData.Item("闘争指令")
            '通告書番号
            Me.txtNoticeNumber.Text = _NameStrikeData.Item("通告番号")
        ElseIf _intSelectBtn = 4 Then
            Me.txtNoticeNumber.Text = NOTICE_NUMBER_UNDEFINE
        End If
        '申請者
        Me.txtApply.Text = _NameStrikeData.Item("代表者")
        '通告番号種別
        Me.txtNoticeKind.Text = _NameStrikeData.Item("通告番号種別")
        '争議行為通告番号
        Me.txtStrikeNumber.Text = _NameStrikeData.Item("争議行為通告番号")
        '開始日付
        Me.dtpStartDate.Value = _NameStrikeData.Item("活動日付")
        '開始時間
        Me.txtStartTime.Text = _NameStrikeData.Item("開始時間")
        '終了日付
        Me.dtpEndDate.Value = _NameStrikeData.Item("終了日")
        '終了時間
        Me.txtEndTime.Text = _NameStrikeData.Item("終了時間")
        '規模
        Me.txtScale.Text = GetTimeSpan(Me.dtpStartDate.Value.Date, Me.txtStartTime.Text, _
                               Me.dtpEndDate.Value.Date, Me.txtEndTime.Text)
        '備考
        Me.txtNote.Text = _NameStrikeData.Item("備考")
        '更新回数
        'strUpdateCount = GetRevision(CInt(_NameStrikeData.Item("更新回数")))
        Me.lblCount.Text = GetRevision(CInt(_NameStrikeData.Item("更新回数")))
        '時間枠
        If _NameStrikeData.Item("時間枠コード") = TIME_FRAME_72 Then
            Me.opt72Frame.Checked = True
        ElseIf _NameStrikeData.Item("時間枠コード") = TIME_FRAME_24 Then
            Me.opt24Frame.Checked = True
        End If

        If _intSelectBtn = 2 Then '本登録データの照会
            '闘争番号関連のオブジェクトを表示する
            Me.grpFight.Visible = True
        End If
        '組合員の追加・削除、一時保存、登録確認、キャンセルボタン非表示
        Me.btnAddMember.Visible = False
        Me.btnDeleteMember.Visible = False
        Me.btnWork.Visible = False
        Me.btnConfirm.Visible = False
        Me.btnCancel.Visible = False

        '印刷、内容変更、戻るボタン表示
        Me.btnPrint.Visible = True

        If _intSelectBtn = 4 Then
            Me.btnChange.Visible = True
            '登録権限チェック
            If _strGrantInsert <> GRANT_VALID Then
                Me.btnChange.Enabled = False
            End If
        Else
            If Me.dtpStartDate.Value.Date > Now.Date AndAlso _blnNoRelease = True Then '活動日付が現在以前の場合は内容変更使用不可
                Me.btnChange.Visible = True
            End If
        End If
        Me.btnReturn.Visible = True

        Me.dgdStrikeMember.Rows.Clear()
        If _dtMember IsNot Nothing Then
            '指名ストライキ者表示処理
            For Each dtRow As DataRow In _dtMember.Rows
                AddGridRow()
                Me.dgdStrikeMember(0, Me.dgdStrikeMember.RowCount - 1).Value = dtRow.Item("c_user_id")
                Me.dgdStrikeMember(1, Me.dgdStrikeMember.RowCount - 1).Value = dtRow.Item("c_name")
                Me.dgdStrikeMember(2, Me.dgdStrikeMember.RowCount - 1).Value = dtRow.Item("arealocal")
                Me.dgdStrikeMember(3, Me.dgdStrikeMember.RowCount - 1).Value = dtRow.Item("model")
                Me.dgdStrikeMember(4, Me.dgdStrikeMember.RowCount - 1).Value = dtRow.Item("qualification")
                Me.dgdStrikeMember(5, Me.dgdStrikeMember.RowCount - 1).Value = dtRow.Item("local_omission")
                Me.dgdStrikeMember(6, Me.dgdStrikeMember.RowCount - 1).Value = dtRow.Item("model_omission")
                Me.dgdStrikeMember(7, Me.dgdStrikeMember.RowCount - 1).Value = dtRow.Item("belonging")
            Next
        End If

        '各オブジェクトの編集制限
        Call Me.EditChange(False)

    End Sub

#End Region

#Region "権限によるボタンの制御"
    '***************************************************************************************************
    '   ＩＤ　：EnableButtons
    '   名称　：権限によるボタンの制御
    '   概要　：ボタンの使用可不可を制御します
    '   作成日：2012/01/23(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/23(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub EnableButtons()
        '印刷権限チェック
        If _strGrantPrint <> GRANT_VALID Then
            Me.btnPrint.Enabled = False
            Me.btnFightPrint.Enabled = False
        End If
        '登録権限チェック
        If _strGrantInsert <> GRANT_VALID Then
            If _intSelectBtn = 2 Then
                Me.btnChange.Enabled = False
            End If
        End If
    End Sub

#End Region

#Region "グリッドに行を追加"
    '***************************************************************************************************
    '   ＩＤ　：AddGridRow
    '   名称　：グリッドに行を追加
    '   概要　：グリッドに行を追加
    '   作成日：2012/01/16(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>
    ''' グリッドに行を追加
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AddGridRow()
        Dim intAddRowIndex = 0

        'グリッドに一行追加
        Me.dgdStrikeMember.Rows.Add()
        intAddRowIndex = Me.dgdStrikeMember.Rows.Count - 1

        '社員番号のみ編集可能
        Me.dgdStrikeMember.Rows(intAddRowIndex).Cells(0).ReadOnly = False
        Me.dgdStrikeMember.Rows(intAddRowIndex).Cells(1).ReadOnly = True
        Me.dgdStrikeMember.Rows(intAddRowIndex).Cells(2).ReadOnly = True
        Me.dgdStrikeMember.Rows(intAddRowIndex).Cells(3).ReadOnly = True
        Me.dgdStrikeMember.Rows(intAddRowIndex).Cells(4).ReadOnly = True
        '氏名、会社所属、機種、資格のバックカラー変更
        Me.dgdStrikeMember(1, intAddRowIndex).Style.BackColor = Color.Cornsilk
        Me.dgdStrikeMember(2, intAddRowIndex).Style.BackColor = Color.Cornsilk
        Me.dgdStrikeMember(3, intAddRowIndex).Style.BackColor = Color.Cornsilk
        Me.dgdStrikeMember(4, intAddRowIndex).Style.BackColor = Color.Cornsilk
    End Sub
#End Region

#Region "各オブジェクトの編集可不可変更"
    '***************************************************************************************************
    '   ＩＤ　：EditChange
    '   名称　：各オブジェクトの編集可不可変更
    '   概要　：各オブジェクトの編集可不可変更
    '   作成日：2012/01/11(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/11(木) a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>
    ''' 各オブジェクトの編集可不可変更
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub EditChange(ByVal blnEdit As Boolean)

        If blnEdit = True Then
            '編集可能時
            Me.dtpStartDate.Enabled = True
            Me.txtStartTime.ReadOnly = False
            Me.dtpEndDate.Enabled = True
            Me.txtEndTime.ReadOnly = False
            'Me.opt72Frame.Enabled = True
            'Me.opt24Frame.Enabled = True
            Call Utilities.SetCanEditToControl(True, Me.opt72Frame)
            Call Utilities.SetCanEditToControl(True, Me.opt24Frame)
            Me.txtNote.ReadOnly = False
            Me.txtApply.ReadOnly = False
            Me.dgdStrikeMember.ReadOnly = False
            '編集可能時は闘争指令印刷不可
            Me.grpFight.Enabled = False
        Else
            '編集不可時
            Me.dtpStartDate.Enabled = False
            Me.dtpStartDate.CalendarForeColor = Color.Black
            Me.dtpStartDate.CalendarMonthBackground = Color.White
            Me.txtStartTime.ReadOnly = True
            Me.dtpEndDate.Enabled = False
            'Call Utilities.SetCanEditToControl(False, Me.dtpStartDate)
            'Call Utilities.SetCanEditToControl(False, Me.dtpEndDate)
            Me.txtEndTime.ReadOnly = True
            'Me.opt72Frame.Enabled = False
            'Me.opt24Frame.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.opt72Frame)
            Call Utilities.SetCanEditToControl(False, Me.opt24Frame)
            Me.txtNote.ReadOnly = True
            Me.txtApply.ReadOnly = True
            Me.dgdStrikeMember.ReadOnly = True
            Me.grpFight.Enabled = True
        End If
    End Sub
#End Region

#Region "ユーザー情報の表示"
    '***************************************************************************************************
    '   ＩＤ　：SetUnionMemberData
    '   名称　：ユーザー情報の表示
    '   概要　：社員番号から取得したユーザー情報を表示します
    '   作成日：2012/01/16(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function SetUnionMemberData(ByVal intColumn As Integer, ByVal intRow As Integer) As Boolean
        Dim dtUserData As DataTable = Nothing
        Dim blnAdd As Boolean = False
        'Dim intColumn = e.ColumnIndex
        'Dim intRow = e.RowIndex

        If intColumn = 0 Then '社員番号列で発生したか
            Dim strUserId As String = Me.dgdStrikeMember(intColumn, intRow).Value
            If ChkNull(strUserId) = True Then
                '入力文字列がない場合処理終了
                Exit Function
            End If

            '入力された社員番号のユーザー情報取得
            dtUserData = GetUnionMemberData(strUserId)
            If dtUserData Is Nothing Then
            ElseIf dtUserData.Rows.Count = 0 Then
                CLMsg.Show("GE0025", strUserId)
                Me.dgdStrikeMember(intColumn, intRow).Value = _strPreUserId
                Exit Function
            Else
                Me.dgdStrikeMember(1, intRow).Value = dtUserData.Rows(0).Item("l_name")                 '氏名
                Me.dgdStrikeMember(2, intRow).Value = dtUserData.Rows(0).Item("arealocal")              '会社所属
                Me.dgdStrikeMember(3, intRow).Value = dtUserData.Rows(0).Item("model")                  '機種
                Me.dgdStrikeMember(4, intRow).Value = dtUserData.Rows(0).Item("qualification")          '資格
                Me.dgdStrikeMember(5, intRow).Value = dtUserData.Rows(0).Item("local_omission")         '会社所属省略名
                Me.dgdStrikeMember(6, intRow).Value = dtUserData.Rows(0).Item("model_omission")         '機種省略名
                Me.dgdStrikeMember(7, intRow).Value = dtUserData.Rows(0).Item("belonging")         '組合支部

                blnAdd = True '行追加フラグをたてる
                Dim intNum As Integer = 0
                Do While (intNum < (Me.dgdStrikeMember.Rows.Count))
                    If Me.dgdStrikeMember.Rows(intNum).Cells(0).Value Is Nothing OrElse _
                       Me.dgdStrikeMember.Rows(intNum).Cells(0).Value.Equals("") Then
                        '新規入力可能行がある場合は行追加は行わない
                        blnAdd = False
                        Exit Do
                    End If
                    intNum = intNum + 1
                Loop
            End If
        End If

        Return blnAdd
    End Function
#End Region

#Region "社員番号より組合員の取得"
    '***************************************************************************************************
    '   ＩＤ　：GetUnionMemberData
    '   名称　：社員番号より組合員の取得
    '   概要　：受け取った社員番号の組合員情報をDataTable型で返却します
    '   作成日：2012/01/16(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) a.onuma  新規作成
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
            'DB接続開始
            clsDb.Connect()
            strSql = ""
            strSql = strSql & " SELECT attr1.c_user_id AS c_user_id, attr1.l_name AS l_name " & vbCrLf
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
            strSql = strSql & "    AND attr1.c_ksh = attr2.c_ksh " & vbCrLf
            strSql = strSql & "    AND attr1.d_from = attr2.now_from " & vbCrLf
            strSql = strSql & "    AND attr1.k_staf_kind IN ('" & STAF_KIND_REGULAR & "','" & STAF_KIND_SENIOR & "')" & vbCrLf
            strSql = strSql & "    AND attr1.k_user_status = '" & USER_STATUS_ENTRY & "' " & vbCrLf
            strSql = strSql & "    AND dtl1.c_constant = 'AREA_LOCAL' AND dtl1.c_constant_seq = attr1.k_local " & vbCrLf
            strSql = strSql & "    AND dtl2.c_constant = 'MODEL' AND dtl2.c_constant_seq = attr1.k_model " & vbCrLf
            strSql = strSql & "    AND dtl3.c_constant = 'QUALIFICATION' AND dtl3.c_constant_seq = attr1.k_qualification " & vbCrLf
            strSql = strSql & "    AND dtl4.c_constant = 'BELONGING' AND dtl4.c_constant_seq = attr1.k_belonging " & vbCrLf
            strSql = strSql & "ORDER BY CLng(attr1.c_user_id) "

            dtRet = clsDb.ExecuteSql(strSql)

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "GetFightNumber")
            log.Fatal(ex.Message)

        Finally
            clsDb.Disconnect()
        End Try

        Return dtRet
    End Function

#End Region

#Region "ストライキ文書登録データ作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateNameStrikeData
    '   名称　：ストライキ文書登録データ作成
    '   概要　：画面の入力内容より、ストライキ者の登録データを作成します
    '   作成日：2012/01/16(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function CreateNameStrikeData(ByVal blnWork As Boolean) As nameStrikeStructureData
        Dim data As nameStrikeStructureData = New nameStrikeStructureData
        Dim strNewNoticeNumber As String = String.Empty
        If _intSelectBtn = 0 OrElse _intSelectBtn = 4 Then '新規、または一時保存の本登録
            If blnWork = False Then
                '新規通告番号を取得
                strNewNoticeNumber = GetNoticeNumber()
                'ストID
                data.strNameStrikeId = MDLoginInfo.Period.ToString & "-" & strNewNoticeNumber
                '通告番号
                data.strNameStrikeNumber = strNewNoticeNumber
                '闘争指令番号
                data.strFightNumber = GetFightCancelNumber(STRIKE_KIND_NOTICE)
            Else
                'インデックス
                If _intSelectBtn = 0 Then
                    '新規にインデックス作成
                    data.intIndex = GetMaxIndex()
                ElseIf _intSelectBtn = 4 Then
                    'この一時保存文書に発行済みのインデックス取得
                    data.intIndex = _NameStrikeData.Item("インデックス")
                End If
                'ストID
                data.strNameStrikeId = String.Empty
                '通告番号
                data.strNameStrikeNumber = String.Empty
                '闘争指令番号
                data.strFightNumber = String.Empty
            End If
            '会社コード
            data.strKsh = MDLoginInfo.Ksh
            '期ID
            data.strPeriodId = MDLoginInfo.PeriodId
            '通告番号種別
            data.strStrikeKind = Me.txtNoticeKind.Text.Trim
            '指名スト種別
            data.strNameStrikeKind = STRIKE_KIND_NOTICE
            '申請地区区分
            data.strApplyArea = String.Empty
            '申請者
            data.strStandName = Me.txtApply.Text.Trim
            '争議ID（争議行為通告番号）
            data.strStrikeId = Me.txtStrikeNumber.Text.Trim
            '通告年月日
            data.dateInfo = _dateTimeNow.ToString
            '解除指令番号
            data.strCancelNumber = String.Empty
            'スト開始日付
            data.strOperationFrom = Me.dtpStartDate.Value.Date
            'スト開始時間
            data.strOperationTimeFrom = Me.txtStartTime.Text.Trim
            'スト終了日付
            data.strOperationTo = Me.dtpEndDate.Value.Date
            'スト終了時間
            data.strOperationTimeTo = Me.txtEndTime.Text.Trim
            '時間枠
            If Me.opt72Frame.Checked = True Then
                data.strTimeFrame = TIME_FRAME_72
            ElseIf Me.opt24Frame.Checked = True Then
                data.strTimeFrame = TIME_FRAME_24
            End If
            '関連通告番号
            data.strRelatedNumber = String.Empty
            '関連ストID
            data.strRelatedNameStrikeId = String.Empty
            '備考
            data.strNote = Me.txtNote.Text.Trim
            '作成日
            data.dateInsert = "'" & _dateTimeNow.Date & "' "
            '作成者ID
            data.strInsertUserId = MDLoginInfo.UserId
            '更新日
            data.dateUpdate = "'" & _dateTimeNow & "' "
            '更新者ID
            data.strUpdateUserId = MDLoginInfo.UserId
            '更新回数
            If _intSelectBtn = 4 AndAlso blnWork = True Then
                data.intUpdateCount = _NameStrikeData.Item("更新回数") + 1
            Else
                data.intUpdateCount = 0
            End If
        Else
            '一時保存の場合はインデックスを現在の文書から取得
            If blnWork = True Then
                data.intIndex = _NameStrikeData.Item("インデックス")
            End If
            '指名ストライキ文書ID
            data.strNameStrikeId = _NameStrikeData.Item("通告番号")
            '申請者
            data.strStandName = Me.txtApply.Text.Trim
            '会社コード
            'data.strKsh = _NameStrikeData.Item("会社コード")
            ''期ID
            'data.strPeriodId = _NameStrikeData.Item("期ID")
            '通告日
            'data.dateInfo = _NameStrikeData.Item("通告日時")
            'スト開始日付
            data.strOperationFrom = Me.dtpStartDate.Value.Date
            'スト開始時間
            data.strOperationTimeFrom = Me.txtStartTime.Text.Trim
            'スト終了日付
            data.strOperationTo = Me.dtpEndDate.Value.Date
            'スト終了時間
            data.strOperationTimeTo = Me.txtEndTime.Text.Trim
            '時間枠
            If Me.opt72Frame.Checked = True Then
                data.strTimeFrame = TIME_FRAME_72
            ElseIf Me.opt24Frame.Checked = True Then
                data.strTimeFrame = TIME_FRAME_24
            End If
            '備考
            data.strNote = Me.txtNote.Text.Trim
            '更新日
            data.dateUpdate = "'" & _dateTimeNow & "' "
            '更新者ID
            data.strUpdateUserId = MDLoginInfo.UserId
            '更新回数
            data.intUpdateCount = _NameStrikeData.Item("更新回数") + 1
        End If

        Return data
    End Function

#End Region

#Region "ストライキ者登録データ作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateNameStrikeMemberData
    '   名称　：ストライキ者登録データ作成
    '   概要　：画面の入力内容より、ストライキ者の登録データを作成します
    '   作成日：2012/01/16(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/16(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function CreateNameStrikeMemberData(ByVal strNewStrikeId As String, _
                                                ByVal blnWork As Boolean, _
                                                Optional ByVal intNewIndex As Integer = -1) As List(Of nameStrikeMemberStructureData)
        Dim dataList As List(Of nameStrikeMemberStructureData) = New List(Of nameStrikeMemberStructureData)
        Dim data As nameStrikeMemberStructureData = Nothing

        For intCnt As Integer = 0 To Me.dgdStrikeMember.Rows.Count - 1
            If Me.dgdStrikeMember.Rows(intCnt).Cells(0).Value IsNot Nothing AndAlso _
            Me.dgdStrikeMember.Rows(intCnt).Cells(0).Value.Equals("") = False Then
                data = New nameStrikeMemberStructureData

                If _intSelectBtn = 0 OrElse _intSelectBtn = 4 Then
                    If blnWork = True Then
                        'インデックス
                        data.intIndex = intNewIndex
                    End If
                    'ストID
                    data.strNameStrikeId = strNewStrikeId
                    '社員番号
                    data.strUserId = Me.dgdStrikeMember.Rows(intCnt).Cells(0).Value
                    '通告年月日
                    data.dateInfo = "'" & _dateTimeNow.ToString & "' "
                    '解除ストID
                    data.strCancelNameStrikeId = String.Empty
                    '解除日
                    data.strCancelDate = "Null"
                    '備考
                    data.strNote = Me.txtNote.Text.Trim
                    '作成日
                    data.dateInsert = "'" & _dateTimeNow.Date & "' "
                    '作成者ID
                    data.strInsertUserId = MDLoginInfo.UserId
                    '更新日
                    data.dateUpdate = "'" & _dateTimeNow & "' "
                    '更新者ID
                    data.strUpdateUserId = MDLoginInfo.UserId
                    '更新回数
                    data.intUpdateCount = 0
                Else
                    '通告年月日
                    If _intSelectBtn = 2 Then
                        '内容変更の場合
                        data.dateInfo = "'" & _NameStrikeData.Item("通告日時") & "' "
                        'ElseIf _intSelectBtn = 4 Then
                        '    'インデックス
                        '    data.intIndex = intNewIndex
                        '    '一時保存の場合、通告日時を登録日付として出力しているので
                        '    'そちらから取得
                        '    data.dateInfo = "'" & _NameStrikeData.Item("登録日付") & "' "
                    End If
                    'ストID
                    data.strNameStrikeId = strNewStrikeId
                    '社員番号
                    data.strUserId = Me.dgdStrikeMember.Rows(intCnt).Cells(0).Value
                    '解除ストID
                    data.strCancelNameStrikeId = String.Empty
                    '解除日
                    data.strCancelDate = "Null"
                    '備考
                    data.strNote = Me.txtNote.Text.Trim
                    '作成日
                    data.dateInsert = "'" & _dateTimeNow.Date & "' "
                    '作成者ID
                    data.strInsertUserId = MDLoginInfo.UserId
                    '更新日
                    data.dateUpdate = "'" & _dateTimeNow & "' "
                    '更新者ID
                    data.strUpdateUserId = MDLoginInfo.UserId
                    '更新回数
                    data.intUpdateCount = 0
                End If

                '作成データの格納
                dataList.Add(data)
            End If
        Next
        Return dataList
    End Function

#End Region

#Region "新規指名ストライキの登録・印刷"
    '***************************************************************************************************
    '   ＩＤ　：NewNameStrike
    '   名称　：登新規指名ストライキの登録・印刷
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：2012/03/17(土) Fujisaku
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴  ：2012/01/06(金) a.onuma  新規作成
    '         ：2012/03/17(土) Fujisaku　闘争指令の同時印刷に対応
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    Private Sub NewNameStrike()

        Dim blnNoErr As Boolean = False
        Dim clsDb As CLAccessMdb = Nothing                                  ' ローカルレプリカ
        'Dim clsDbMst As CLAccessMdbMst = Nothing                            ' サーバデザインマスタ
        Dim dataNameStrike As nameStrikeStructureData = Nothing
        Dim dataNameStrikeMember As List(Of nameStrikeMemberStructureData) = Nothing
        Dim strNewNameStrikeId As String = String.Empty
        Dim intBtn As Integer = -1
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim reportObj2 As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P2 = New DS0404P2()
        Dim ds2 As DS0404P3 = New DS0404P3()
        Dim fmPreview As FM000203 = New FM000203
        Dim strOfficerName As String = String.Empty

        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 2
        reportObj = New CR0404P2
        reportObj2 = New CR0404P3
        fmPreview.ObjResource = reportObj

        '組合員情報部分
        Dim drDetail As DS0404P2.dtDetailRow
        'レポート上部
        Dim drHeader As DS0404P2.dtHeaderRow = ds.dtHeader.NewRow
        '更新回数
        Dim drFooter As DS0404P2.dtFooterRow = ds.dtFooter.NewRow
        '闘争指令
        Dim drFight As DS0404P3.dtHeaderRow = ds2.dtHeader.NewRow

        drHeader.BeginEdit()
        '社長名
        strOfficerName = GetPresidentName()
        drHeader.president_name = strOfficerName
        Dim strStrikeList As List(Of String) = New List(Of String)
        strStrikeList.AddRange(Me.txtStrikeNumber.Text.Split("-"))
        If _intSelectBtn = 0 OrElse _intSelectBtn = 4 Then
            '通告番号種別（Bのみ）
            drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
            'タイトルの２４時間を表示するように修正2012/06/13 START
            If Me.opt24Frame.Checked Then
                drHeader.k_time_frame = "02"
            End If
            If Me.opt72Frame.Checked Then
                drHeader.k_time_frame = "01"
            End If
            'タイトルの２４時間を表示するように修正2012/06/13 END
            '期
            drHeader.period_id = MDLoginInfo.Period
            '通告番号（連番）
            drHeader.name_strike_info = NOTICE_NUMBER_UNDEFINE
            '日付
            drHeader.info = _dateTimeNow.ToString.Substring(0, 10)
            '代表者
            drHeader.l_stand_name = Me.txtApply.Text.Trim
            '通告番号種別（Bのみ）
            drHeader.strike_strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
            '争議行為ID（期ID側）
            drHeader.strike_period_id = strStrikeList(0)
            '争議行為ID（連番側）
            drHeader.strike_name_strike_info = strStrikeList(1)
            '開始日時
            drHeader.operation_from = Me.dtpStartDate.Value.Year & " 年  " & Me.dtpStartDate.Value.Month & " 月  " & _
                                      Me.dtpStartDate.Value.Day & " 日  " & Me.txtStartTime.Text.Trim & " 時 "
            '終了日時
            drHeader.operation_to = Me.dtpEndDate.Value.Year & " 年  " & Me.dtpEndDate.Value.Month & " 月  " & _
                                    Me.dtpEndDate.Value.Day & " 日  " & Me.txtEndTime.Text.Trim & " 時 "
            '規模
            drHeader.time_frame = Me.txtScale.Text.Trim
            drHeader.EndEdit()

            drFooter.BeginEdit()
            '更新回数
            drFooter.up = Me.lblCount.Text
            drFooter.EndEdit()
        End If

        drFight.BeginEdit()
        '通告番号種別（Bのみ）
        drFight.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '期
        drFight.period_id = MDLoginInfo.Period
        '通告番号（連番）　登録・発番後に設定
        '日付
        drFight.info = _dateTimeNow.ToString.Substring(0, 10)
        '代表者
        drFight.l_stand_name = Me.txtApply.Text.Trim
        '開始日時
        drFight.operation_from = Me.dtpStartDate.Value.Year & " 年  " & Me.dtpStartDate.Value.Month & " 月  " & _
                                  Me.dtpStartDate.Value.Day & " 日  " & Me.txtStartTime.Text.Trim & " 時 "
        '終了日時
        drFight.operation_to = Me.dtpEndDate.Value.Year & " 年  " & Me.dtpEndDate.Value.Month & " 月  " & _
                                Me.dtpEndDate.Value.Day & " 日  " & Me.txtEndTime.Text.Trim & " 時 "
        '規模
        drFight.time_frame = Me.txtScale.Text.Trim
        '闘争指令番号　登録・発番後に設定
        drFight.EndEdit()

        ds.dtHeader.Rows.Add(drHeader) 'ヘッダー情報格納
        ds.dtFooter.Rows.Add(drFooter) 'フッター情報格納
        ds2.dtHeader.Rows.Add(drFight) '闘争指令情報格納
        For intCnt As Integer = 0 To Me.dgdStrikeMember.Rows.Count - 1
            If ChkNull(Me.dgdStrikeMember.Rows(intCnt).Cells(0).Value) = False Then
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                '社員番号
                drDetail.c_staf_id = dgdStrikeMember.Rows(intCnt).Cells(0).Value
                '氏名
                drDetail.l_name = dgdStrikeMember.Rows(intCnt).Cells(1).Value
                '会社所属省略名
                drDetail.local_name = dgdStrikeMember.Rows(intCnt).Cells(5).Value
                '機種省略名
                drDetail.k_model = dgdStrikeMember.Rows(intCnt).Cells(6).Value
                '資格
                drDetail.k_qualification = dgdStrikeMember.Rows(intCnt).Cells(4).Value
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
            End If
        Next

        Dim intRowCnt As Integer = ds.dtDetail.Rows.Count
        Dim intRest As Integer = (intRowCnt Mod 40)
        If intRest > 0 Then
            Dim intQuotient As Integer = System.Math.Floor(intRowCnt / 40)
            Do While (intRowCnt < 40 * (intQuotient + 1))
                drDetail = ds.dtDetail.NewRow
                ds.dtDetail.Rows.Add(drDetail)
                intRowCnt = intRowCnt + 1
            Loop
        End If
        'データソースセット
        reportObj.SetDataSource(ds)

        Call fmPreview.ShowDialog()
        'どのボタンが押下されたか受け取る
        intBtn = fmPreview.IntQlickBtnFlag
        If intBtn = 4 Then '新規指名ストライキ登録
            Try
                '同期処理による最新データの取得 SEQ対応によって前同期を省略 2013/04/19
                'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                '指名ストライキ文書データの作成
                dataNameStrike = CreateNameStrikeData(False)
                'ストライキ文書IDの取得
                strNewNameStrikeId = dataNameStrike.strNameStrikeId
                '指名ストライキ者リストの作成
                dataNameStrikeMember = CreateNameStrikeMemberData(strNewNameStrikeId, False)

                ' オブジェクト生成
                clsDb = New CLAccessMdb         ' ローカルレプリカ
                'clsDbMst = New CLAccessMdbMst   ' サーバデザインマスタ

                '===========================================================================================================
                '   データベース接続
                '===========================================================================================================
                clsDb.Connect()                 ' ローカルレプリカ
                'clsDbMst.Connect()              ' サーバデザインマスタ

                ' トランザクション開始
                clsDb.BeginTran()               ' ローカルレプリカ
                'clsDbMst.BeginTran()            ' サーバデザインマスタ

                If _intSelectBtn = 0 Then
                    '===========================================================================
                    '   指名ストライキ文書の登録
                    '===========================================================================
                    If InsertNameStrikeData(
                        clsDb,
                        dataNameStrike
                    ) = True Then ' 接続二重化(2014/12)
                        '=======================================================================
                        '   指名ストライキ者の登録
                        '=======================================================================
                        If InsertNameStrikeMemberData(
                            clsDb,
                            dataNameStrikeMember
                        ) = True Then ' 接続二重化(2014/12)
                            '正常に終了した場合コミット
                            blnNoErr = True
                        End If
                    End If
                ElseIf _intSelectBtn = 4 Then
                    '===========================================================================
                    '   指名ストライキ文書の登録（一時保存からの本登録）
                    '===========================================================================
                    If InsertNameStrikeData(
                        clsDb,
                        dataNameStrike
                    ) = True Then ' 接続二重化(2014/12)
                        '=======================================================================
                        '   指名ストライキ者の登録
                        '=======================================================================
                        If InsertNameStrikeMemberData(
                            clsDb,
                            dataNameStrikeMember
                        ) = True Then ' 接続二重化(2014/12)
                            '===================================================================
                            '   元の一時保存文書、一時保存ストライキ者の削除
                            '===================================================================
                            If DeleteNameStrikeWork(
                                clsDb,
                                CInt(_NameStrikeData.Item("インデックス"))
                            ) = True Then ' 接続二重化(2014/12)
                                '===============================================================
                                '   一時保存指名ストライキ者の削除
                                '===============================================================
                                If DeleteNameStrikeMemberDataWork(
                                    clsDb,
                                    CInt(_NameStrikeData.Item("インデックス"))
                                ) = True Then ' 接続二重化(2014/12)
                                    '正常に終了した場合コミット
                                    blnNoErr = True
                                End If
                            End If
                        End If
                    End If
                End If

                If blnNoErr = True Then
                    '正常終了の場合、トランザクション確定
                    clsDb.CommitTran()              ' ローカルレプリカ
                    'clsDbMst.CommitTran()           ' サーバデザインマスタ

                    ' SEQUENCファイルに値を反映
                    Dim strSeqName1 As String
                    strSeqName1 = "seq_str_nms_" + NSMDInfo.PeriodId + ".txt"
                    Dim sw1 As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName1, False)
                    sw1.Write(dataNameStrike.strNameStrikeNumber)
                    sw1.Close()

                    Dim strSeqName2 As String
                    strSeqName2 = "seq_str_fit.txt"
                    Dim sw2 As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName2, False)
                    sw2.Write(dataNameStrike.strFightNumber)
                    sw2.Close()

                    '文書通告番号を*****から更新
                    ds.dtHeader.Rows.Item(0).Item("name_strike_info") = dataNameStrike.strNameStrikeNumber
                    reportObj.SetDataSource(ds)
                    fmPreview.PrintOut()

                    '闘争指令の印刷
                    fmPreview.ObjResource = reportObj2
                    ds2.dtHeader.Rows.Item(0).Item("name_strike_info") = dataNameStrike.strNameStrikeNumber
                    ds2.dtHeader.Rows.Item(0).Item("c_fight") = dataNameStrike.strFightNumber
                    reportObj2.SetDataSource(ds2)
                    fmPreview.PrintOut()

                    '同期処理による最新データの反映　接続二重化対応で同期は削除(2014/12)
                    ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    '画面終了処理
                    Call Me.FormClose()
                Else
                    ' トランザクション取消
                    clsDb.RollbackTran()            ' ローカルレプリカ
                    'clsDbMst.RollbackTran()         ' サーバデザインマスタ
                End If

            Catch ex As Exception
                ' トランザクション取消
                clsDb.RollbackTran()                ' ローカルレプリカ
                'clsDbMst.RollbackTran()             ' サーバデザインマスタ
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "btnConfirm_Click")
                log.Fatal(ex.Message)

            Finally
                '===============================================================================================================
                '   データベース切断
                '===============================================================================================================
                clsDb.Disconnect()                  ' ローカルレプリカ
                'clsDbMst.Disconnect()               ' サーバデザインマスタ
                '-----------------------------------------------------------------------------------
                '   データベースオブジェクト開放
                '-----------------------------------------------------------------------------------
                ' ローカルレプリカ
                If Not clsDb Is Nothing Then
                    If Not clsDb Is Nothing Then
                        clsDb = Nothing
                    End If
                    '' サーバデザインマスタ
                    'If Not clsDbMst Is Nothing Then
                    '    clsDbMst = Nothing
                    'End If
                End If
            End Try
        ElseIf intBtn = 2 Then 'キャンセルボタン押下時は処理終了
            Exit Sub
        End If
    End Sub

#End Region

#Region "新規指名ストライキの更新・印刷"
    '***************************************************************************************************
    '   ＩＤ　：UpdateNameStrike
    '   名称　：登新規指名ストライキの更新・印刷
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub UpdateNameStrike()

        Dim blnNoErr As Boolean = False
        Dim clsDb As CLAccessMdb = New CLAccessMdb                  ' ローカルレプリカ
        'Dim clsDbMst As CLAccessMdbMst = New CLAccessMdbMst         ' サーバデザインマスタ
        Dim dataNameStrike As nameStrikeStructureData = Nothing
        Dim dataNameStrikeMember As List(Of nameStrikeMemberStructureData) = Nothing
        Dim strNewNameStrikeId As String = String.Empty
        Dim intBtn As Integer = -1
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P2 = New DS0404P2()
        Dim fmPreview As FM000203 = New FM000203
        Dim strOfficerName As String = String.Empty

        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 2
        reportObj = New CR0404P2
        fmPreview.ObjResource = reportObj

        '組合員情報部分
        Dim drDetail As DS0404P2.dtDetailRow
        'レポート上部
        Dim drHeader As DS0404P2.dtHeaderRow = ds.dtHeader.NewRow
        '更新回数
        Dim drFooter As DS0404P2.dtFooterRow = ds.dtFooter.NewRow

        drHeader.BeginEdit()
        '社長名
        strOfficerName = GetPresidentName()
        drHeader.president_name = strOfficerName
        Dim strStrikeList As List(Of String) = New List(Of String)
        strStrikeList.AddRange(Me.txtStrikeNumber.Text.Split("-"))
        Dim strNameStrikeList As List(Of String) = New List(Of String)
        strNameStrikeList.AddRange(_NameStrikeData.Item("通告番号").ToString.Split("-"))
        '通告番号種別（Bのみ）
        drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        'タイトルの２４時間を表示するように2012/06/13 START
        If Me.opt24Frame.Checked Then
            drHeader.k_time_frame = "02"
        End If
        If Me.opt72Frame.Checked Then
            drHeader.k_time_frame = "01"
        End If
        'タイトルの２４時間を表示するように2012/06/13 START
        '期
        drHeader.period_id = strNameStrikeList(0)
        '通告番号（連番）
        drHeader.name_strike_info = strNameStrikeList(1)
        '日付
        drHeader.info = _dateTimeNow.ToString.Substring(0, 10)
        '代表者
        drHeader.l_stand_name = Me.txtApply.Text.Trim
        '通告番号種別（Bのみ）
        drHeader.strike_strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '争議行為ID（期ID側）
        drHeader.strike_period_id = strStrikeList(0)
        '争議行為ID（連番側）
        drHeader.strike_name_strike_info = strStrikeList(1)
        '開始日時
        drHeader.operation_from = Me.dtpStartDate.Value.Year & " 年  " & Me.dtpStartDate.Value.Month & " 月  " & _
                                  Me.dtpStartDate.Value.Day & " 日  " & Me.txtStartTime.Text.Trim & " 時 "
        '終了日時
        drHeader.operation_to = Me.dtpEndDate.Value.Year & " 年  " & Me.dtpEndDate.Value.Month & " 月  " & _
                                Me.dtpEndDate.Value.Day & " 日  " & Me.txtEndTime.Text.Trim & " 時 "
        '規模
        drHeader.time_frame = Me.txtScale.Text.Trim
        drHeader.EndEdit()

        drFooter.BeginEdit()
        '更新回数
        'Dim strUpdate As String = GetRevision(CInt(_NameStrikeData.Item("更新回数")) + 1)
        drFooter.up = GetRevision(CInt(_NameStrikeData.Item("更新回数")) + 1)
        drFooter.EndEdit()

        ds.dtHeader.Rows.Add(drHeader) 'ヘッダー情報格納
        ds.dtFooter.Rows.Add(drFooter) 'フッター情報格納
        For intCnt As Integer = 0 To Me.dgdStrikeMember.Rows.Count - 1
            If ChkNull(Me.dgdStrikeMember.Rows(intCnt).Cells(0).Value) = False Then
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                '社員番号
                drDetail.c_staf_id = dgdStrikeMember.Rows(intCnt).Cells(0).Value
                '氏名
                drDetail.l_name = dgdStrikeMember.Rows(intCnt).Cells(1).Value
                '会社所属省略名
                drDetail.local_name = dgdStrikeMember.Rows(intCnt).Cells(5).Value
                '機種省略名
                drDetail.k_model = dgdStrikeMember.Rows(intCnt).Cells(6).Value
                '資格
                drDetail.k_qualification = dgdStrikeMember.Rows(intCnt).Cells(4).Value
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
            End If
        Next
        Dim intRowCnt As Integer = ds.dtDetail.Rows.Count
        Dim intRest As Integer = (intRowCnt Mod 40)
        If intRest > 0 Then
            Dim intQuotient As Integer = System.Math.Floor(intRowCnt / 40)
            Do While (intRowCnt < 40 * (intQuotient + 1))
                drDetail = ds.dtDetail.NewRow
                ds.dtDetail.Rows.Add(drDetail)
                intRowCnt = intRowCnt + 1
            Loop
        End If
        'データソースセット
        reportObj.SetDataSource(ds)

        Call fmPreview.ShowDialog()
        'どのボタンが押下されたか受け取る
        intBtn = fmPreview.IntQlickBtnFlag
        If intBtn = 4 Then '指名ストライキ更新
            Try
                '同期処理による最新データの取得 サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
                'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                '指名ストライキ文書更新データの作成
                dataNameStrike = CreateNameStrikeData(False)
                'ストライキ文書IDの取得
                strNewNameStrikeId = dataNameStrike.strNameStrikeId
                '指名ストライキ者リストの作成
                dataNameStrikeMember = CreateNameStrikeMemberData(strNewNameStrikeId, False)

                '===============================================================================
                '   データベース接続
                '===============================================================================
                clsDb.Connect()             ' ローカルレプリカ
                'clsDbMst.Connect()          ' サーバデザインマスタ

                'トランザクション開始
                clsDb.BeginTran()           ' ローカルレプリカ
                'clsDbMst.BeginTran()        ' サーバデザインマスタ

                '===============================================================================
                '   指名ストライキ文書の更新
                '===============================================================================
                If UpdateNameStrikeData(
                    clsDb,
                    dataNameStrike
                ) = True Then
                    '===========================================================================
                    '   以前に登録されていた指名ストライキ者をいったん削除
                    '===========================================================================
                    If DeleteNameStrikeMemberData(
                        clsDb,
                        dataNameStrike.strNameStrikeId
                    ) = True Then
                        '=======================================================================
                        '   今回の変更で選択されていた指名ストライキ者を登録
                        '=======================================================================
                        If InsertNameStrikeMemberData(
                            clsDb,
                            dataNameStrikeMember
                        ) = True Then
                            'すべて問題なく処理できた場合正常終了フラグをたてる
                            blnNoErr = True
                        End If
                    End If
                End If

                If blnNoErr = True Then
                    '正常終了の場合、トランザクション確定
                    clsDb.CommitTran()              ' ローカルレプリカ
                    'clsDbMst.CommitTran()           ' サーバデザインマスタ
                    'プリント
                    fmPreview.PrintOut()
                    '同期処理による最新データの反映 サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
                    'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    '画面終了処理
                    Call Me.FormClose()
                Else
                    ' 異常終了の場合、トランザクション取消
                    clsDb.RollbackTran()            ' ローカルレプリカ
                    'clsDbMst.RollbackTran()         ' サーバデザインマスタ
                End If

            Catch ex As Exception
                ' トランザクション取消
                clsDb.RollbackTran()                ' ローカルレプリカ
                'clsDbMst.RollbackTran()             ' サーバデザインマスタ
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "btnConfirm_Click")
                log.Fatal(ex.Message)

            Finally
                '===============================================================================================================
                '   データベース切断
                '===============================================================================================================
                clsDb.Disconnect()                  ' ローカルレプリカ
                'clsDbMst.Disconnect()               ' サーバデザインマスタ

                '-----------------------------------------------------------------------------------
                '   データベースオブジェクト開放
                '-----------------------------------------------------------------------------------
                ' ローカルレプリカ
                If Not clsDb Is Nothing Then
                    clsDb = Nothing
                End If
                '' サーバデザインマスタ
                'If Not clsDbMst Is Nothing Then
                '    clsDbMst = Nothing
                'End If
            End Try
        ElseIf intBtn = 2 Then 'キャンセルボタン押下時は処理終了
            Exit Sub
        End If
    End Sub

#End Region

#Region "一時保存指名ストライキの登録（更新）・印刷"
    '***************************************************************************************************
    '   ＩＤ　：NewNameStrikeWork
    '   名称　：一時保存指名ストライキの登録（更新）・印刷
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub NewUpdateNameStrikeWork()

        Dim blnNoErr As Boolean = False
        Dim clsDb As CLAccessMdb = New CLAccessMdb                      ' ローカルレプリカ
        'Dim clsDbMst As CLAccessMdbMst = New CLAccessMdbMst             ' サーバデザインマスタ
        Dim dataNameStrike As nameStrikeStructureData = Nothing
        Dim dataNameStrikeMember As List(Of nameStrikeMemberStructureData) = Nothing
        Dim strNewNameStrikeId As String = String.Empty
        Dim intNewIndex As Integer = -1
        Dim intBtn As Integer = -1
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P2 = New DS0404P2()
        Dim fmPreview As FM000203 = New FM000203
        Dim strOfficerName As String = String.Empty

        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 1
        '印刷部数の表示設定
        fmPreview.PrintCntVisible = True
        reportObj = New CR0404P2
        fmPreview.ObjResource = reportObj

        '組合員情報部分
        Dim drDetail As DS0404P2.dtDetailRow
        'レポート上部
        Dim drHeader As DS0404P2.dtHeaderRow = ds.dtHeader.NewRow
        '更新回数
        Dim drFooter As DS0404P2.dtFooterRow = ds.dtFooter.NewRow

        drHeader.BeginEdit()
        '社長名
        strOfficerName = GetPresidentName()
        drHeader.president_name = strOfficerName
        Dim strStrikeList As List(Of String) = New List(Of String)
        strStrikeList.AddRange(Me.txtStrikeNumber.Text.Split("-"))
        '通告番号種別（Bのみ）
        drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        'タイトルの２４時間を表示するように2012/06/13 START
        If Me.opt24Frame.Checked Then
            drHeader.k_time_frame = "02"
        End If
        If Me.opt72Frame.Checked Then
            drHeader.k_time_frame = "01"
        End If
        'タイトルの２４時間を表示するように2012/06/13 START
        '期
        drHeader.period_id = MDLoginInfo.Period
        '通告番号（連番）
        drHeader.name_strike_info = NOTICE_NUMBER_UNDEFINE
        '日付
        drHeader.info = _dateTimeNow.ToString.Substring(0, 10)
        '代表者
        drHeader.l_stand_name = Me.txtApply.Text.Trim
        '通告番号種別（Bのみ）
        drHeader.strike_strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '争議行為ID（期ID側）
        drHeader.strike_period_id = strStrikeList(0)
        '争議行為ID（連番側）
        drHeader.strike_name_strike_info = strStrikeList(1)
        '開始日時
        drHeader.operation_from = Me.dtpStartDate.Value.Year & " 年  " & Me.dtpStartDate.Value.Month & " 月  " & _
                                  Me.dtpStartDate.Value.Day & " 日  " & Me.txtStartTime.Text.Trim & " 時 "
        '終了日時
        drHeader.operation_to = Me.dtpEndDate.Value.Year & " 年  " & Me.dtpEndDate.Value.Month & " 月  " & _
                                Me.dtpEndDate.Value.Day & " 日  " & Me.txtEndTime.Text.Trim & " 時 "
        '規模
        drHeader.time_frame = Me.txtScale.Text.Trim
        drHeader.EndEdit()

        drFooter.BeginEdit()
        '更新回数
        If _intSelectBtn = 0 Then
            '新規のときは画面のデータをそのまま出す
            drFooter.up = Me.lblCount.Text
        Else
            '上書きの際は更新回数+1で出す
            'Dim strUpdate As String = GetRevision(CInt(_NameStrikeData.Item("更新回数")) + 1)
            drFooter.up = GetRevision(CInt(_NameStrikeData.Item("更新回数")) + 1)
        End If
        drFooter.EndEdit()

        ds.dtHeader.Rows.Add(drHeader) 'ヘッダー情報格納
        ds.dtFooter.Rows.Add(drFooter) 'フッター情報格納
        For intCnt As Integer = 0 To Me.dgdStrikeMember.Rows.Count - 1
            If ChkNull(Me.dgdStrikeMember.Rows(intCnt).Cells(0).Value) = False Then
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                '社員番号
                drDetail.c_staf_id = dgdStrikeMember.Rows(intCnt).Cells(0).Value
                '氏名
                drDetail.l_name = dgdStrikeMember.Rows(intCnt).Cells(1).Value
                '会社所属省略名
                drDetail.local_name = dgdStrikeMember.Rows(intCnt).Cells(5).Value
                '機種省略名
                drDetail.k_model = dgdStrikeMember.Rows(intCnt).Cells(6).Value
                '資格
                drDetail.k_qualification = dgdStrikeMember.Rows(intCnt).Cells(4).Value
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
            End If
        Next
        Dim intRowCnt As Integer = ds.dtDetail.Rows.Count
        Dim intRest As Integer = (intRowCnt Mod 40)
        If intRowCnt = 0 Then
            Do While (intRowCnt < 40)
                drDetail = ds.dtDetail.NewRow
                ds.dtDetail.Rows.Add(drDetail)
                intRowCnt = intRowCnt + 1
            Loop
        Else
            If intRest > 0 Then
                Dim intQuotient As Integer = System.Math.Floor(intRowCnt / 40)
                Do While (intRowCnt < 40 * (intQuotient + 1))
                    drDetail = ds.dtDetail.NewRow
                    ds.dtDetail.Rows.Add(drDetail)
                    intRowCnt = intRowCnt + 1
                Loop
            End If
        End If

        'データソースセット
        reportObj.SetDataSource(ds)

        Call fmPreview.ShowDialog()
        'どのボタンが押下されたか受け取る
        intBtn = fmPreview.IntQlickBtnFlag
        If intBtn = 0 OrElse intBtn = 1 Then '登録＆印刷、または登録のみ
            Try
                '同期処理による最新データの取得 SEQ対応によって前同期を省略 2013/04/19
                'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                '指名ストライキ文書データの作成
                dataNameStrike = CreateNameStrikeData(True)
                'ストライキ文書IDの取得
                strNewNameStrikeId = dataNameStrike.strNameStrikeId
                'インデックスの取得
                intNewIndex = dataNameStrike.intIndex
                '指名ストライキ者リストの作成
                dataNameStrikeMember = CreateNameStrikeMemberData(strNewNameStrikeId, True, intNewIndex)

                '===============================================================================
                '   データベース接続
                '===============================================================================
                clsDb.Connect()             ' ローカルレプリカ
                'clsDbMst.Connect()          ' サーバデザインマスタ

                'トランザクション開始
                clsDb.BeginTran()           ' ローカルレプリカ
                'clsDbMst.BeginTran()        ' サーバデザインマスタ

                If _intSelectBtn = 0 Then
                    '===========================================================================
                    '   指名ストライキ文書の登録
                    '===========================================================================
                    If InsertNameStrikeDataWork(
                        clsDb,
                        dataNameStrike
                    ) = True Then
                        '=======================================================================
                        '   指名ストライキ者の登録
                        '=======================================================================
                        If InsertNameStrikeMemberDataWork(
                            clsDb,
                            dataNameStrikeMember
                        ) = True Then
                            '正常に終了した場合コミット
                            blnNoErr = True
                        End If
                    End If
                ElseIf _intSelectBtn = 4 Then
                    '===========================================================================
                    '   指名ストライキ文書の更新
                    '===========================================================================
                    If UpdateNameStrikeDataWork(
                        clsDb,
                        dataNameStrike
                    ) = True Then
                        '=======================================================================
                        '   以前に登録されていた指名ストライキ者をいったん削除
                        '=======================================================================
                        If DeleteNameStrikeMemberDataWork(
                            clsDb,
                            intNewIndex
                        ) = True Then
                            '===================================================================
                            '   今回の変更で選択されていた指名ストライキ者を登録
                            '===================================================================
                            If InsertNameStrikeMemberDataWork(
                                clsDb,
                                dataNameStrikeMember
                            ) = True Then
                                'すべて問題なく処理できた場合正常終了フラグをたてる
                                blnNoErr = True
                            End If
                        End If
                    End If
                End If

                If blnNoErr = True Then
                    '正常終了の場合、トランザクション確定
                    clsDb.CommitTran()                  ' ローカルレプリカ
                    'clsDbMst.CommitTran()               ' サーバデザインマスタ
                    If intBtn = 0 Then
                        fmPreview.PrintOut()
                    End If
                    '同期処理による最新データの反映 サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
                    'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    '画面終了処理
                    Call Me.FormClose()
                Else
                    ' トランザクション取消
                    clsDb.RollbackTran()                ' ローカルレプリカ
                    'clsDbMst.RollbackTran()             ' サーバデザインマスタ
                End If

            Catch ex As Exception
                ' トランザクション取消
                clsDb.RollbackTran()                    ' ローカルレプリカ
                'clsDbMst.RollbackTran()                 ' サーバデザインマスタ
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "btnConfirm_Click")
                log.Fatal(ex.Message)

            Finally
                '===============================================================================================================
                '   データベース切断
                '===============================================================================================================
                clsDb.Disconnect()                      ' ローカルレプリカ
                'clsDbMst.Disconnect()                   ' サーバデザインマスタ

                '-----------------------------------------------------------------------------------
                '   データベースオブジェクト開放
                '-----------------------------------------------------------------------------------
                ' ローカルレプリカ
                If Not clsDb Is Nothing Then
                    clsDb = Nothing
                End If
                '' サーバデザインマスタ
                'If Not clsDbMst Is Nothing Then
                '    clsDbMst = Nothing
                'End If

            End Try
        ElseIf intBtn = 2 Then 'キャンセルボタン押下時は処理終了
            Exit Sub
        End If
    End Sub

#End Region

#Region "フォームクローズ"
    '***************************************************************************************************
    '   ＩＤ　：FormClose
    '   名称　：フォームクローズ
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub FormClose()
        Dim pn As Panel
        Dim uc As Control
        Dim selTab As Integer
        Dim cls040401 As UC040401

        Me.Visible = False
        pn = ParentForm.Controls(MAIN_PANEL_ID)
        uc = pn.Controls(SCREEN_ID_UC040401)

        If uc Is Nothing Then
            uc = New UC040401
            Call pn.Controls.Add(uc)
        Else
            If _intFinishBtn = 0 OrElse _intFinishBtn = 1 Then
                '登録処理実行後の場合、検索画面の各検索結果をクリア
                cls040401 = pn.Controls(SCREEN_ID_UC040401)
                Call cls040401.ClearActionSearch()
                Call cls040401.ClearNoticeSearch()
                Call cls040401.ClearTempSearch()

                '再検索処理
                selTab = cls040401.TclStrikeSearch.SelectedIndex()
                If _intFinishBtn = 0 AndAlso selTab = 0 Then
                    Call cls040401.SearchActionDate()
                    cls040401.TabControl2.SelectedIndex = 0
                ElseIf _intFinishBtn = 0 AndAlso selTab = 1 Then
                    Call cls040401.SearchNoticeDate()
                    cls040401.TabControl2.SelectedIndex = 0
                ElseIf _intFinishBtn = 1 Then
                    Call cls040401.SearchTempSaving()
                    cls040401.TabControl2.SelectedIndex = 1
                End If
            End If
            uc.Visible = True
        End If
        Me.Dispose()
    End Sub

#End Region

#End Region

End Class

#End Region
