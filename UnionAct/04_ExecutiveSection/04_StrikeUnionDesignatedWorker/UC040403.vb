#Region "UC040403"
'===========================================================================================================
'   クラスＩＤ　　：UC040403
'   クラス名称　　：指名ストライキ－一部解除
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb              ' ローカルレプリカ
Imports UnionAct.NSCLAccessMdbMst           ' サーバデザインマスタ
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.MDNameStrikeCommon
Imports C1.Win.C1FlexGrid
Imports UnionAct.GUI.Common

Public Class UC040403
    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    'この文書に登録されている指名ストライキ者
    Private _dtMember As DataTable = Nothing

    '参照権限
    Private _strGrantReference As String = String.Empty
    '登録権限
    Private _strGrantInsert As String = String.Empty
    '印刷権限
    Private _strGrantPrint As String = String.Empty
    'ファイル出力権限
    Private _strGrantFileOutput As String = String.Empty

    '編集不可フラグ
    Private _blnEdit As Boolean = True
    '現在時刻
    Private _dateTimeNow As DateTime = Now
    ''' <summary>
    ''' 選択した指名ストライキ文書の情報
    ''' </summary>
    ''' <remarks></remarks>
    Private _NameStrikeData As DataRow = Nothing
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
    '0 = 一部解除
    '1 = 一時保存
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
    '   ＩＤ　：UC040403_Load
    '   名称　：フォームロード
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub UC040403_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

            'グリッドの初期化
            If Me.InitializeNameStrikeMemberGrid() = True Then
                If _intSelectBtn = 1 Then '新規の一部解除
                    'この文書に登録されている組合員を取得
                    _dtMember = GetBelongUnionMember(_NameStrikeData.Item("通告番号"))
                    Call Me.InitializeNew()
                Else
                    'この文書に登録されている組合員を取得
                    If _intSelectBtn = 3 Then
                        _dtMember = GetBelongUnionMember(_NameStrikeData.Item("通告番号"))
                    ElseIf _intSelectBtn = 5 Then
                        _dtMember = GetBelongUnionMemberWork(_NameStrikeData.Item("インデックス"))
                    End If
                    'データ照会
                    Call Me.InitializeReference()
                    If _intSelectBtn = 5 Then '一時保存データの照会
                        '「一時保存文書」ラベルの表示
                        Me.lblNoticeWork.Visible = True
                        'バックカラー変更
                        Me.GroupBox1.BackColor = Color.PapayaWhip
                        Me.lblCount.BackColor = Color.PapayaWhip
                        Me.lblNoticeWork.BackColor = Color.PapayaWhip
                        Me.lblNoticeKind.BackColor = Color.PapayaWhip
                        '闘争指令印刷ボタン使用不可
                        'Me.btnFightCancelPrint.Enabled = False
                        Me.grpFightCancel.Visible = False
                    End If
                End If
            End If

            'ボタンの制御
            Call Me.EnableButtons()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040403, SCREEN_NAME_UC040403, "UC040403_Load")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "一時保存ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnWork_Click
    '   名称　：一時保存ボタンクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnWork_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWork.Click
        Me.Cursor = Cursors.WaitCursor
        '終了ボタンのフラグを一時保存にする
        _intFinishBtn = 1
        '一時保存データの新規登録処理呼び出し
        Call Me.NewReleaseNameStrikeWork()
        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "一部解除確認ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnRelease_Click
    '   名称　：一部解除確認ボタンクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：2012/03/17(土) Fujisaku
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    ' 　　　　：2012/03/17(土) Fujisaku　闘争指令の同時印刷に対応
    '***************************************************************************************************
    Private Sub btnRelease_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRelease.Click
        '必須入力チェック
        Me.Cursor = Cursors.WaitCursor
        If ChkInput() = False Then
            Me.Cursor = Cursors.Default
            Exit Sub
        End If

        '終了ボタンのフラグを一部解除確認にする
        _intFinishBtn = 0
        If _intSelectBtn = 1 Then
            Call Me.NewReleaseNameStrike()
        ElseIf _intSelectBtn = 5 Then
            If Me.flxNameStrikeMemberCancel.Rows.Count > 1 Then
                '既に解除済みの組合員が含まれるかチェック
                If ChkExistReleaseMember(_NameStrikeData.Item("関連関連ストＩＤ")) = False Then
                    CLMsg.Show("DE0011")
                    Me.Cursor = Cursors.Default
                    Exit Sub
                End If

                '元文書が一時保存作成後更新されたかチェック
                If ChkUpdateRelatedStrike() = False Then
                    CLMsg.Show("GE0233")
                    Me.Cursor = Cursors.Default
                    Exit Sub
                End If
                '一時保存の解除文書本登録処理
                Call Me.ConfirmTempReleaseNameStrike()
            End If
        End If
        Me.Cursor = Cursors.Default
    End Sub
#End Region

#Region "キャンセルボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If CLMsg.Show("GQ0007") = DialogResult.No Then
            'いいえが選択された場合は何も行わない
            Exit Sub
        End If

        _intFinishBtn = 2
        If _intSelectBtn = 1 Then '新規の一部解除
            Call Me.FormClose()
        ElseIf _intSelectBtn = 5 Then 'データ照会時
            Me.txtApply.Text = _NameStrikeData.Item("代表者")
            Me.txtNote.Text = _NameStrikeData.Item("備考")
            Me.txtApply.ReadOnly = True
            Me.txtNote.ReadOnly = True
            'Me.flxNameStrikeMemberCancel.Enabled = False
            Me.flxNameStrikeMemberCancel.AllowEditing = False
            Dim blnEdit As Boolean = Me.flxNameStrikeMemberCancel.AllowEditing
            If _strGrantPrint = GRANT_VALID Then
                Me.btnPrint.Enabled = True
            End If
            Me.btnChange.Visible = True
            Me.btnReturn.Visible = True
            Me.btnWork.Visible = False
            Me.btnRelease.Visible = False
            Me.btnCancel.Visible = False
            '解除の画像を非表示にする
            For intCnt As Integer = 1 To flxNameStrikeMemberCancel.Rows.Count - 1
                If Me.flxNameStrikeMemberCancel.GetCellImage(intCnt, 2) IsNot Nothing Then
                    Me.flxNameStrikeMemberCancel.SetCellImage(intCnt, 2, Nothing)
                End If
            Next
            _blnEdit = False
        End If

    End Sub
#End Region

#Region "印刷ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：印刷ボタンクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P2 = New DS0404P2()
        Dim fmPreview As FM000203 = New FM000203
        Dim strOfficerName As String = String.Empty

        Me.Cursor = Cursors.WaitCursor
        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 3
        fmPreview.PrintCntVisible = True
        reportObj = New CR0404P5
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
        Dim strNameStrikeList As List(Of String) = New List(Of String)
        Dim strRelatedStrikeList As List(Of String) = New List(Of String)
        strRelatedStrikeList.AddRange(_NameStrikeData.Item("関連関連ストＩＤ").ToString.Split("-"))
        If _intSelectBtn = 3 Then
            strNameStrikeList.AddRange(_NameStrikeData.Item("通告番号").ToString.Split("-"))
            '期
            drHeader.period_id = strNameStrikeList(0)
            '通告番号（連番）
            drHeader.name_strike_info = strNameStrikeList(1)
        Else
            '期
            'drHeader.period_id = MDLoginInfo.Period.ToString
            drHeader.period_id = strRelatedStrikeList(0)
            '通告番号（連番）
            drHeader.name_strike_info = Me.txtNoticeNumber.Text.Trim
        End If
        '通告番号種別（Bのみ）
        drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '日付
        If Me.txtNoticeDate.Text <> "" Then
            drHeader.info = DateValue(Me.txtNoticeDate.Text)
        Else
            drHeader.info = _dateTimeNow.ToString.Substring(0, 10)
        End If
        '代表者
        drHeader.l_stand_name = Me.txtApply.Text.Trim
        '通告番号種別（Bのみ）
        drHeader.strike_strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '解除対象ストライキ文書ID（期ID側）
        drHeader.c_related_info_period_id = strRelatedStrikeList(0)
        '解除対象ストライキ文書ID（連番側）
        drHeader.c_related_info = strRelatedStrikeList(1)
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
        For intCnt As Integer = 1 To Me.flxNameStrikeMemberCancel.Rows.Count - 1
            drDetail = ds.dtDetail.NewRow
            drDetail.BeginEdit()
            '社員番号
            drDetail.c_staf_id = Me.flxNameStrikeMemberCancel.GetData(intCnt, 0)
            '氏名
            drDetail.l_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 1)
            '会社所属省略名
            drDetail.local_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 3)
            '機種省略名
            drDetail.k_model = Me.flxNameStrikeMemberCancel.GetData(intCnt, 4)
            '資格
            drDetail.k_qualification = Me.flxNameStrikeMemberCancel.GetData(intCnt, 5)
            drDetail.EndEdit()
            ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
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
                'キャンセルの場合何も行わない
            Case 3
                '印刷ボタン押下時
                fmPreview.PrintOut()
        End Select
        Me.Cursor = Cursors.Default

    End Sub
#End Region

#Region "闘争指令（解除指令）印刷ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnFightCancelPrint_Click
    '   名称　：闘争指令（解除指令）印刷ボタンクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnFightCancelPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFightCancelPrint.Click
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P3 = New DS0404P3()
        Dim fmPreview As FM000203 = New FM000203

        Me.Cursor = Cursors.WaitCursor
        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 3
        fmPreview.PrintCntVisible = True
        reportObj = New CR0404P4
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
        drHeader.c_fight = Me.txtFightCanceNumber.Text.Trim
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

#Region "内容変更ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnChange_Click
    '   名称　：内容変更ボタンクリック"
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChange.Click
        '一時保存確認、登録確認、キャンセルボタン表示
        '一時保存データの照会時は一時保存ボタンも表示
        If _intSelectBtn = 5 Then
            Me.btnWork.Visible = True
            'Me.flxNameStrikeMemberCancel.Enabled = True
            Me.flxNameStrikeMemberCancel.AllowEditing = True
        End If
        Me.btnRelease.Visible = True
        Me.btnCancel.Visible = True

        '印刷、内容変更、戻るボタン非表示
        Me.btnPrint.Enabled = False
        Me.btnChange.Visible = False
        Me.btnReturn.Visible = False
        '各種オブジェクトを変更可能にする
        Call Me.EditChange(True)
        '編集可能
        _blnEdit = True
    End Sub
#End Region

#Region "戻るボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnReturn_Click
    '   名称　：戻るボタンクリック"
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        _intFinishBtn = 3
        Call Me.FormClose()
    End Sub
#End Region

#Region "フレックスグリッドクリック"
    '***************************************************************************************************
    '   ＩＤ　：flxNameStrikeMemberCancel_Click
    '   名称　：フレックスグリッドクリック
    '   概要　：
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub flxNameStrikeMemberCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles flxNameStrikeMemberCancel.Click
        Try
            If _blnEdit = True Then '編集可能か
                If Me.flxNameStrikeMemberCancel.HitTest.Type.Equals(HitTestTypeEnum.Cell) Then 'セルがクリックされたか
                    Dim row As Integer = Me.flxNameStrikeMemberCancel.Row
                    If (row >= 0) Then 'データが１行以上存在するか
                        Dim col As Integer = Me.flxNameStrikeMemberCancel.Col
                        If ((col >= 2) AndAlso Not Me.flxNameStrikeMemberCancel.Item(row, col).ToString.Equals("")) Then
                            If (Me.flxNameStrikeMemberCancel.GetCellImage(row, col) Is Nothing) Then
                                Me.flxNameStrikeMemberCancel.SetCellImage(row, col, Image.FromFile(MDSystemInfo.AppPath & FILE_RELEASE))
                            Else
                                '日付が表示されていない（=既に解除済み）の場合は画像を表示しない
                                Me.flxNameStrikeMemberCancel.SetCellImage(row, col, Nothing)
                            End If
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040403, SCREEN_NAME_UC040403, "flxNameStrikeMemberCancel_Click")
            log.Fatal(ex.Message)
        End Try
    End Sub
#End Region
#End Region

#Region "関数"

#Region "新規一部解除時の初期表示処理"
    '***************************************************************************************************
    '   ＩＤ　：InitializeNew
    '   名称　：新規一部解除時の初期表示処理
    '   概要　：画面の初期表示時の制御を行います
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '         ：2012/04/12 Fujisaku 日付1の表示内容を活動開始日付で変更
    '***************************************************************************************************
    ''' <summary>
    ''' 新規一部解除時の初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeNew()
        '申請者
        Me.txtApply.Text = _NameStrikeData.Item("代表者")
        '通告日
        If _NameStrikeData.Item("通告日時").ToString.Length < 10 Then
            Me.txtNoticeDate.Text = _NameStrikeData.Item("通告日時")
        Else
            Me.txtNoticeDate.Text = _NameStrikeData.Item("通告日時").ToString.Substring(0, 10)
        End If
        '通告番号種別
        Me.txtNoticeKind.Text = _NameStrikeData.Item("通告番号種別")
        '通告書番号
        Me.txtNoticeNumber.Text = _NameStrikeData.Item("通告番号")
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
        '時間枠
        If _NameStrikeData.Item("時間枠コード") = TIME_FRAME_72 Then
            Me.opt72Frame.Checked = True
        ElseIf _NameStrikeData.Item("時間枠コード") = TIME_FRAME_24 Then
            Me.opt24Frame.Checked = True
        End If
        '闘争指令番号
        Me.txtFightCanceNumber.Text = _NameStrikeData.Item("闘争指令")
        '備考
        Me.txtNote.Text = _NameStrikeData.Item("備考")
        '申請者、備考以外は変更不可
        Me.dtpStartDate.Enabled = False
        Me.txtStartTime.ReadOnly = True
        Me.dtpEndDate.Enabled = False
        Me.txtEndTime.ReadOnly = True
        'Me.opt72Frame.Enabled = False
        'Me.opt24Frame.Enabled = False
        Call Utilities.SetCanEditToControl(False, Me.opt72Frame)
        Call Utilities.SetCanEditToControl(False, Me.opt24Frame)

        '更新回数のラベル非表示
        Me.lblCount.Visible = False
        Me.btnFightCancelPrint.Enabled = False
        '指名ストライキ者の表示
        If _dtMember IsNot Nothing Then
            Dim intCnt As Integer = 1
            For Each dtRow As DataRow In _dtMember.Rows
                Me.flxNameStrikeMemberCancel.Rows.Add()
                '社員番号
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 0, dtRow.Item("c_user_id"))
                '氏名
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 1, dtRow.Item("c_name"))
                '日付1
                If dtRow.Item("cancel_id").Equals(DBNull.Value) = True OrElse ChkNull(dtRow.Item("cancel_id")) = True Then
                    If _NameStrikeData.Item("活動日付").ToString.Length < 10 Then
                        Me.flxNameStrikeMemberCancel.SetData(intCnt, 2, _NameStrikeData.Item("活動日付"))
                    Else
                        Me.flxNameStrikeMemberCancel.SetData(intCnt, 2, _NameStrikeData.Item("活動日付").ToString.Substring(0, 10))
                    End If
                Else
                    Me.flxNameStrikeMemberCancel.SetData(intCnt, 2, "")
                End If
                '会社所属省略名
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 3, dtRow.Item("local_omission"))
                '機種省略名
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 4, dtRow.Item("model_omission"))
                '資格省略名
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 5, dtRow.Item("qualification"))
                intCnt = intCnt + 1
            Next
        End If
    End Sub
#End Region

#Region "データ照会時の初期表示処理"
    '***************************************************************************************************
    '   ＩＤ　：InitializeReference
    '   名称　：データ照会時の初期表示処理
    '   概要　：データ照会時の制御を行います
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '         ：2012/04/12 Fujisaku 日付1の表示内容を活動開始日付で変更
    '***************************************************************************************************
    ''' <summary>
    ''' データ照会時の初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitializeReference()
        Dim strUpdateCount As String = String.Empty

        '申請者
        Me.txtApply.Text = _NameStrikeData.Item("代表者")
        '通告日
        If _intSelectBtn = 3 Then
            If _NameStrikeData.Item("通告日時").ToString.Length > 10 Then
                Me.txtNoticeDate.Text = _NameStrikeData.Item("通告日時").ToString.Substring(0, 10)
            Else
                Me.txtNoticeDate.Text = _NameStrikeData.Item("通告日時")
            End If
            '通告書番号
            Me.txtNoticeNumber.Text = _NameStrikeData.Item("通告番号")
            '解除指令番号
            Me.txtFightCanceNumber.Text = _NameStrikeData.Item("解除指令")
        ElseIf _intSelectBtn = 5 Then
            Me.txtNoticeNumber.Text = NOTICE_NUMBER_UNDEFINE
            Me.txtFightCanceNumber.Text = NOTICE_NUMBER_UNDEFINE
        End If
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
        '時間枠
        If _NameStrikeData.Item("時間枠コード") = TIME_FRAME_72 Then
            Me.opt72Frame.Checked = True
        ElseIf _NameStrikeData.Item("時間枠コード") = TIME_FRAME_24 Then
            Me.opt24Frame.Checked = True
        End If
        '備考
        Me.txtNote.Text = _NameStrikeData.Item("備考")
        '更新回数
        'strUpdateCount = GetRevision(CInt(_NameStrikeData.Item("更新回数")))
        Me.lblCount.Text = GetRevision(CInt(_NameStrikeData.Item("更新回数")))
        'Me.flxNameStrikeMemberCancel.Enabled = False
        Me.flxNameStrikeMemberCancel.AllowEditing = False

        Me.lblRelatedNameStrikeNumber.Visible = True
        Me.txtRelatedNameStrikeNumber.Visible = True
        Me.txtRelatedNameStrikeNumber.Text = _NameStrikeData.Item("関連関連ストＩＤ")
        '解除方法の説明ラベル非表示
        Me.lblHowToRelease.Visible = False
        '各種文言変更
        Me.lblTitle.Text = "指名ストライキ - 通告画面"
        Me.lblNoticeKind.Text = "【一部解除通告】"
        Me.lblFightCancelNumber.Text = "解除通告番号"
        Me.btnFightCancelPrint.Text = "解除指令印刷"
        '一時保存、登録確認、キャンセルボタン非表示
        Me.btnWork.Visible = False
        Me.btnRelease.Visible = False
        Me.btnCancel.Visible = False

        '印刷、内容変更、戻るボタン表示
        Me.btnPrint.Visible = True

        If _intSelectBtn <> 3 Then
            '活動日付が現在以前の場合、本登録の解除データ参照の場合は内容変更使用不可
            Me.btnChange.Visible = True
        End If
        Me.btnReturn.Visible = True

        If _dtMember IsNot Nothing Then
            '指名ストライキ者表示処理
        End If

        '各オブジェクトの編集制限
        Call Me.EditChange(False)
        '編集不可フラグをたてる
        _blnEdit = False
        '指名ストライキ者の表示
        If _dtMember IsNot Nothing Then
            Dim intCnt As Integer = 1
            For Each dtRow As DataRow In _dtMember.Rows
                Me.flxNameStrikeMemberCancel.Rows.Add()
                '社員番号
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 0, dtRow.Item("c_user_id"))
                '氏名
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 1, dtRow.Item("c_name"))
                '日付1
                If _NameStrikeData.Item("活動日付").ToString.Length < 10 Then
                    Me.flxNameStrikeMemberCancel.SetData(intCnt, 2, _NameStrikeData.Item("活動日付"))
                Else
                    Me.flxNameStrikeMemberCancel.SetData(intCnt, 2, _NameStrikeData.Item("活動日付").ToString.Substring(0, 10))
                End If
                '会社所属省略名
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 3, dtRow.Item("local_omission"))
                '機種省略名
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 4, dtRow.Item("model_omission"))
                '資格省略名
                Me.flxNameStrikeMemberCancel.SetData(intCnt, 5, dtRow.Item("qualification"))
                intCnt = intCnt + 1
            Next
        End If
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
            Me.btnFightCancelPrint.Enabled = False
        End If
        '登録権限チェック
        If _strGrantInsert <> GRANT_VALID Then
            If _intSelectBtn = 2 Then
                Me.btnChange.Enabled = False
            End If
        End If
    End Sub

#End Region

#Region "グリッド初期化処理"
    '***************************************************************************************************
    '   ＩＤ　：InitializeNameStrikeMemberGrid
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2012/01/19(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/19(木) a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>グリッド初期化処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function InitializeNameStrikeMemberGrid() As Boolean
        Dim blnRet As Boolean = False   ' 処理結果
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            ' 描画なし（処理終了した最後に描画）
            Me.flxNameStrikeMemberCancel.Redraw = False

            ''-----------------------------------------------------------------------------------
            ''   グリッド全体設定
            ''-----------------------------------------------------------------------------------
            '' 総数
            Me.flxNameStrikeMemberCancel.Rows.Count = 1                                      ' 縦
            Me.flxNameStrikeMemberCancel.Cols.Count = 6                                      ' 横

            ' 固定行
            Me.flxNameStrikeMemberCancel.Rows.Fixed = 1                                      ' 縦
            Me.flxNameStrikeMemberCancel.Cols.Fixed = 2                                      ' 横

            '' 1行選択モード
            Me.flxNameStrikeMemberCancel.FocusRect = C1FLEXGRID_FOCUS_RECT_ENUM_NONE
            ''-----------------------------------------------------------------------------------
            ''   ヘッダー部設定
            ''-----------------------------------------------------------------------------------
            '' ヘッダー文字列
            Me.flxNameStrikeMemberCancel.Cols(0).Caption = "社員番号"                                     ' 社員番号
            Me.flxNameStrikeMemberCancel.Cols(1).Caption = "氏名"                                         ' 氏名
            Me.flxNameStrikeMemberCancel.Cols(2).Caption = "日付1"                                        ' 日付1
            Me.flxNameStrikeMemberCancel.Cols(3).Caption = "会社所属省略名"                               ' 会社氏所属省略名
            Me.flxNameStrikeMemberCancel.Cols(4).Caption = "機種省略名"                                   ' 機種省略名
            Me.flxNameStrikeMemberCancel.Cols(5).Caption = "資格省略名"                                   ' 資格省略名
            '表示列はすべて編集不可
            Me.flxNameStrikeMemberCancel.Cols(0).AllowEditing = False
            Me.flxNameStrikeMemberCancel.Cols(1).AllowEditing = False
            Me.flxNameStrikeMemberCancel.Cols(2).AllowEditing = False
            '会社所属、機種、資格の各列は隠す
            Me.flxNameStrikeMemberCancel.Cols(3).Visible = False
            Me.flxNameStrikeMemberCancel.Cols(4).Visible = False
            Me.flxNameStrikeMemberCancel.Cols(5).Visible = False


            ' ヘッダー文字位置
            Me.flxNameStrikeMemberCancel.Cols(0).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER       ' 社員番号  
            Me.flxNameStrikeMemberCancel.Cols(1).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER       ' 氏名
            Me.flxNameStrikeMemberCancel.Cols(2).TextAlignFixed = C1FLEXGRID_TEXT_ALIGN_ENUM_CENTERCENTER       ' 日付1

            '---------------------------------------------------------------------------
            '   カラム部設定
            '---------------------------------------------------------------------------
            ' カラム文字位置
            Me.flxNameStrikeMemberCancel.Cols(0).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_RIGHTCENTER            ' 社員番号
            Me.flxNameStrikeMemberCancel.Cols(1).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER             ' 氏名
            Me.flxNameStrikeMemberCancel.Cols(2).TextAlign = C1FLEXGRID_TEXT_ALIGN_ENUM_LEFTCENTER             ' 日付1

            ' カラム幅
            Me.flxNameStrikeMemberCancel.Cols(0).Width = 90                                                     ' 社員番号
            Me.flxNameStrikeMemberCancel.Cols(1).Width = 120                                                    ' 氏名
            Me.flxNameStrikeMemberCancel.Cols(2).Width = 92                                                     ' 日付1

            '' 描画
            Me.flxNameStrikeMemberCancel.Redraw = True

            ' 戻り値格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040403, SCREEN_NAME_UC040403, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
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
            Me.txtNote.ReadOnly = False
            Me.txtApply.ReadOnly = False
            'Me.lblFightCancelNumber.Text = "闘争番号"
            'Me.btnFightCancelPrint.Text = "闘争指令印刷"
            'Me.dgdStrikeMember.ReadOnly = False
        Else
            '編集不可時
            Me.dtpStartDate.Enabled = False
            Me.txtStartTime.ReadOnly = True
            Me.dtpEndDate.Enabled = False
            Me.txtEndTime.ReadOnly = True
            'Me.opt72Frame.Enabled = False
            'Me.opt24Frame.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.opt72Frame)
            Call Utilities.SetCanEditToControl(False, Me.opt24Frame)
            Me.txtNote.ReadOnly = True
            Me.txtApply.ReadOnly = True
            'Me.dgdStrikeMember.ReadOnly = True

        End If
    End Sub
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
    '         ：2012/04/12 Fujisaku　過去日付は取消できない判定処理を不要の為撤去
    '***************************************************************************************************
    Private Function ChkInput() As Boolean
        Dim blnRet As Boolean = False
        Dim blnExistRelease = False

        '開催日付が過去日であるか
        'If Me.dtpStartDate.Value <= Now Then
        '    CLMsg.Show("GE0111")
        '    Return blnRet
        'End If
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
            Return blnRet
        End If
        '***　必須入力チェック

        '申請者文字数チェック
        If MDNameStrikeCommon.LengthByte(Me.txtApply.Text) > 30 Then
            CLMsg.Show("GE0103")
            Return blnRet
        End If

        '備考欄
        If MDNameStrikeCommon.LengthByte(txtNote.Text) > 200 Then
            CLMsg.Show("GE0112", "備考は、", "200", "100")
            Return blnRet
        End If

        '解除対象者が未選択の場合
        For intCnt As Integer = 0 To Me.flxNameStrikeMemberCancel.Rows.Count - 1
            If Me.flxNameStrikeMemberCancel.GetCellImage(intCnt, 2) IsNot Nothing Then
                blnExistRelease = True
                Exit For
            End If
        Next
        If blnExistRelease = False Then
            CLMsg.Show("GE0075")
            Return blnRet
        End If

        blnRet = True
        Return blnRet
    End Function

#End Region

#Region "解除済みメンバー存在チェック"
    '***************************************************************************************************
    '   ＩＤ　：InitializeNew
    '   名称　：新規一部解除時の初期表示処理
    '   概要　：画面の初期表示時の制御を行います
    '   作成日：2012/01/13(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/13(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkExistReleaseMember(ByVal strRelatedStrikeId As String) As Boolean
        Dim blnRet As Boolean = True
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim strWhereId As String = String.Empty
        Dim dtRet As DataTable = Nothing
        Try
            For intCnt As Integer = 1 To Me.flxNameStrikeMemberCancel.Rows.Count - 1
                If Me.flxNameStrikeMemberCancel.GetCellImage(intCnt, 2) IsNot Nothing Then
                    'グリッドに表示されている社員番号を検索条件に追加
                    If strWhereId = String.Empty Then
                        strWhereId = "'" & Me.flxNameStrikeMemberCancel.GetData(intCnt, 0) & "' "
                    Else
                        strWhereId = strWhereId & ",'" & Me.flxNameStrikeMemberCancel.GetData(intCnt, 0) & "' "
                    End If
                End If
            Next
            strSql = ""
            strSql = strSql & "SELECT st_member.c_user_id FROM name_strike_member_date AS st_member " & vbCrLf
            strSql = strSql & ", name_strike AS st " & vbCrLf
            strSql = strSql & "WHERE st_member.c_name_strike_id = st.c_name_strike_id " & vbCrLf
            strSql = strSql & "AND st.c_really_name_strike_id = '" & strRelatedStrikeId & "' " & vbCrLf
            strSql = strSql & "AND st_member.c_user_id IN (" & strWhereId & ") " & vbCrLf

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

#Region "元文書の更新チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkUpdateRelatedStrike
    '   名称　：元文書の更新チェック
    '   概要　：一時保存作成時点から、一部解除の対象となる文書が更新されていないかチェックします
    '   作成日：2012/01/26(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/26(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkUpdateRelatedStrike() As Boolean
        Dim blnRet As Boolean = True
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim dtRet As DataTable = Nothing

        Try
            strSql = ""
            strSql = strSql & "SELECT d_up FROM name_strike " & vbCrLf
            strSql = strSql & "WHERE c_name_strike_id = '" & _NameStrikeData.Item("関連関連ストＩＤ") & "' " & vbCrLf

            clsDb.Connect()
            dtRet = clsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                If dtRet.Rows(0).Item("d_up") >= _NameStrikeData.Item("登録日付") Then
                    blnRet = False
                End If
            End If
        Catch ex As Exception
        Finally
            clsDb.Disconnect()
        End Try
        Return blnRet
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
        Dim strNameStrikeIdList As List(Of String) = New List(Of String)

        '一時保存文書照会→一時保存ボタン押下の場合、文書更新データのみ作成
        If _intSelectBtn = 5 AndAlso blnWork = True Then
            'インデックス
            data.intIndex = _NameStrikeData.Item("インデックス")
            '申請者
            data.strStandName = Me.txtApply.Text.Trim
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
            Return data
        End If

        If blnWork = False Then
            '新規通告番号を取得
            'strNewNoticeNumber = GetNoticeNumber()
            strNewNoticeNumber = GetReleaseNumber(_NameStrikeData.Item("期ID").ToString)
            'ストID
            'data.strNameStrikeId = MDLoginInfo.Period.ToString & "-" & strNewNoticeNumber
            Dim strNo As String
            strNo = _NameStrikeData.Item("通告番号").ToString
            If strNo = "" Then
                strNo = _NameStrikeData.Item("関連関連ストＩＤ").ToString
            End If
            data.strNameStrikeId = Split(strNo, "-")(0) & "-" & strNewNoticeNumber
            '通告番号
            data.strNameStrikeNumber = strNewNoticeNumber
            '解除指令番号
            data.strCancelNumber = GetFightCancelNumber(STRIKE_KIND_CALLOFF)
        Else
            'インデックス
            data.intIndex = GetMaxIndex()
            'ストID
            data.strNameStrikeId = String.Empty
            '通告番号
            data.strNameStrikeNumber = String.Empty
            '解除指令番号
            data.strCancelNumber = String.Empty
        End If
        '会社コード
        data.strKsh = MDLoginInfo.Ksh
        '期ID
        data.strPeriodId = _NameStrikeData.Item("期ID").ToString
        '通告番号種別
        data.strStrikeKind = Me.txtNoticeKind.Text.Trim
        '指名スト種別
        data.strNameStrikeKind = STRIKE_KIND_CALLOFF
        '申請地区区分
        data.strApplyArea = String.Empty
        '申請者
        data.strStandName = Me.txtApply.Text.Trim
        '争議ID（争議行為通告番号）
        data.strStrikeId = Me.txtStrikeNumber.Text.Trim
        '通告年月日
        data.dateInfo = _dateTimeNow.ToString
        '闘争指令番号
        data.strFightNumber = String.Empty
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
        '関連ストID
        If _intSelectBtn = 1 Then
            '連番側のみ取得するため配列に格納
            strNameStrikeIdList.AddRange(_NameStrikeData.Item("通告番号").ToString.Split("-"))
            '新規の場合は現在の文書のストIDを入れる
            data.strRelatedNameStrikeId = _NameStrikeData.Item("通告番号")
        Else
            '連番側のみ取得するため配列に格納
            strNameStrikeIdList.AddRange(_NameStrikeData.Item("関連関連ストID").ToString.Split("-"))
            data.strRelatedNameStrikeId = _NameStrikeData.Item("関連関連ストID")
        End If
        '関連通告番号
        data.strRelatedNumber = strNameStrikeIdList(1)
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
                                                ByVal userIdList As List(Of String), _
                                                ByVal blnWork As Boolean, _
                                                Optional ByVal intNewIndex As Integer = -1) As List(Of nameStrikeMemberStructureData)
        Dim dataList As List(Of nameStrikeMemberStructureData) = New List(Of nameStrikeMemberStructureData)
        Dim data As nameStrikeMemberStructureData = Nothing

        For intCnt As Integer = 1 To Me.flxNameStrikeMemberCancel.Rows.Count - 1
            'If _intSelectBtn = 1 Then
            If Me.flxNameStrikeMemberCancel.GetCellImage(intCnt, 2) IsNot Nothing Then '解除のデータか
                If blnWork = True Then
                    'インデックス
                    data.intIndex = intNewIndex
                End If
                'ストID
                data.strNameStrikeId = strNewStrikeId
                '社員番号
                data.strUserId = Me.flxNameStrikeMemberCancel.GetData(intCnt, 0)
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
                dataList.Add(data)
            End If
            'ElseIf _intSelectBtn = 5 Then
            'If blnWork = True Then
            '    'インデックス
            '    data.intIndex = intNewIndex
            'End If
            ''ストID
            'data.strNameStrikeId = strNewStrikeId
            ''社員番号
            'data.strUserId = Me.flxNameStrikeMemberCancel.GetData(intCnt, 0)
            ''通告年月日
            'data.dateInfo = "'" & _dateTimeNow.ToString & "' "
            ''解除ストID
            'data.strCancelNameStrikeId = _NameStrikeData.Item("関連関連ストＩＤ")
            ''解除日
            'data.strCancelDate = "'" & _dateTimeNow.ToString & "' "
            ''備考
            'data.strNote = Me.txtNote.Text.Trim
            ''作成日
            'data.dateInsert = "'" & Now.Date & "' "
            ''作成者ID
            'data.strInsertUserId = MDLoginInfo.UserId
            ''更新日
            'data.dateUpdate = "'" & Now.Date & "' "
            ''更新者ID
            'data.strUpdateUserId = MDLoginInfo.UserId
            ''更新回数
            'data.intUpdateCount = 0
            'dataList.Add(data)
            'End If
        Next
        Return dataList
    End Function

#End Region

#Region "新規指名ストライキ一部解除・印刷"
    '***************************************************************************************************
    '   ＩＤ　：NewReleaseNameStrike
    '   名称　：登新規指名ストライキ一部解除・印刷
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴  ：2012/01/06(金) a.onuma  新規作成
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    Private Sub NewReleaseNameStrike()

        Dim blnNoErr As Boolean = False
        Dim clsDb As CLAccessMdb = Nothing                      ' ローカルレプリカ
        'Dim clsDbMst As CLAccessMdbMst = Nothing                ' サーバデザインマスタ
        Dim dataNameStrike As nameStrikeStructureData = Nothing
        Dim dataNameStrikeMember As List(Of nameStrikeMemberStructureData) = Nothing
        Dim strNewNameStrikeId As String = String.Empty
        Dim intBtn As Integer = -1
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim reportObj2 As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P2 = New DS0404P2()
        Dim ds2 As DS0404P3 = New DS0404P3()
        Dim fmPreview As FM000203 = New FM000203
        Dim releaseUserIdList As List(Of String) = New List(Of String)
        Dim strOfficerName As String = String.Empty

        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 2
        reportObj = New CR0404P5
        reportObj2 = New CR0404P4
        fmPreview.ObjResource = reportObj

        '組合員情報部分
        Dim drDetail As DS0404P2.dtDetailRow
        'レポート上部
        Dim drHeader As DS0404P2.dtHeaderRow = ds.dtHeader.NewRow
        '更新回数
        Dim drFooter As DS0404P2.dtFooterRow = ds.dtFooter.NewRow
        '解除指令
        Dim drCancel As DS0404P3.dtHeaderRow = ds2.dtHeader.NewRow

        drHeader.BeginEdit()
        '社長名
        strOfficerName = GetPresidentName()
        drHeader.president_name = strOfficerName
        Dim strStrikeList As List(Of String) = New List(Of String)
        strStrikeList.AddRange(_NameStrikeData.Item("通告番号").ToString.Split("-"))
        '通告番号種別（Bのみ）
        drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '期
        'drHeader.period_id = MDLoginInfo.Period
        drHeader.period_id = strStrikeList(0)
        '通告番号（連番）
        drHeader.name_strike_info = NOTICE_NUMBER_UNDEFINE
        '日付
        drHeader.info = _dateTimeNow.ToString.Substring(0, 10)
        '代表者
        drHeader.l_stand_name = Me.txtApply.Text.Trim
        '通告番号種別（Bのみ）
        drHeader.strike_strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '解除対象ストライキ文書ID（期ID側）
        drHeader.c_related_info_period_id = strStrikeList(0)
        '解除対象ストライキ文書ID（連番側）
        drHeader.c_related_info = strStrikeList(1)
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

        drCancel.BeginEdit()
        '通告番号種別（Bのみ）
        drCancel.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '期
        drCancel.period_id = MDLoginInfo.Period
        '通告番号（連番） 登録・発番後に設定
        '日付
        drCancel.info = _dateTimeNow.ToString.Substring(0, 10)
        '代表者
        drCancel.l_stand_name = Me.txtApply.Text.Trim
        '開始日時
        drCancel.operation_from = Me.dtpStartDate.Value.Year & " 年  " & Me.dtpStartDate.Value.Month & " 月  " & _
                                  Me.dtpStartDate.Value.Day & " 日  " & Me.txtStartTime.Text.Trim & " 時 "
        '終了日時
        drCancel.operation_to = Me.dtpEndDate.Value.Year & " 年  " & Me.dtpEndDate.Value.Month & " 月  " & _
                                Me.dtpEndDate.Value.Day & " 日  " & Me.txtEndTime.Text.Trim & " 時 "
        '規模
        drCancel.time_frame = Me.txtScale.Text.Trim
        '闘争指令番号 登録・発番後に設定
        drCancel.EndEdit()

        ds.dtHeader.Rows.Add(drHeader) 'ヘッダー情報格納
        ds.dtFooter.Rows.Add(drFooter) 'フッター情報格納
        ds2.dtHeader.Rows.Add(drCancel) '解除指令情報格納
        For intCnt As Integer = 1 To Me.flxNameStrikeMemberCancel.Rows.Count - 1
            If Me.flxNameStrikeMemberCancel.GetCellImage(intCnt, 2) IsNot Nothing Then
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                '社員番号
                drDetail.c_staf_id = Me.flxNameStrikeMemberCancel.GetData(intCnt, 0)
                '氏名
                drDetail.l_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 1)
                '会社所属省略名
                drDetail.local_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 3)
                '機種省略名
                drDetail.k_model = Me.flxNameStrikeMemberCancel.GetData(intCnt, 4)
                '資格
                drDetail.k_qualification = Me.flxNameStrikeMemberCancel.GetData(intCnt, 5)
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
                releaseUserIdList.Add(Me.flxNameStrikeMemberCancel.GetData(intCnt, 0))
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
                dataNameStrikeMember = CreateNameStrikeMemberData(strNewNameStrikeId, releaseUserIdList, False)

                ' オブジェクト生成
                clsDb = New CLAccessMdb             ' ローカルレプリカ
                'clsDbMst = New CLAccessMdbMst       ' サーバデザインマスタ

                '===========================================================================================================
                '   データベース接続
                '===========================================================================================================
                clsDb.Connect()                     ' ローカルレプリカ
                'clsDbMst.Connect()                  ' サーバデザインマスタ

                'トランザクション開始
                clsDb.BeginTran()                   ' ローカルレプリカ
                'clsDbMst.BeginTran()                ' サーバデザインマスタ

                '===============================================================================
                '   名ストライキ文書の登録
                '===============================================================================
                If InsertNameStrikeData(
                    clsDb,
                    dataNameStrike
                ) = True Then
                    '===========================================================================
                    '   指名ストライキ者の登録
                    '===========================================================================
                    If InsertNameStrikeMemberData(
                        clsDb,
                        dataNameStrikeMember
                    ) = True Then
                        '=======================================================================
                        '   解除対象のストライキ者の更新
                        '=======================================================================
                        If UpdateReleaseMemberData(
                            clsDb,
                            _NameStrikeData.Item("通告番号"),
                            strNewNameStrikeId,
                            _dateTimeNow,
                            releaseUserIdList
                        ) = True Then
                            '正常に終了した場合コミット
                            blnNoErr = True
                        End If
                    End If
                End If

                If blnNoErr = True Then
                    '正常終了の場合、トランザクション確定
                    clsDb.CommitTran()              ' ローカルレプリカ
                    'clsDbMst.CommitTran()           ' サーバデザインマスタ

                    ' SEQUENCファイルに値を反映
                    Dim strSeqName1 As String
                    strSeqName1 = "seq_str_nms_" + dataNameStrike.strPeriodId + ".txt"
                    Dim sw1 As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName1, False)
                    sw1.Write(dataNameStrike.strNameStrikeNumber)
                    sw1.Close()

                    Dim strSeqName2 As String
                    strSeqName2 = "seq_str_cnl.txt"
                    Dim sw2 As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName2, False)
                    sw2.Write(dataNameStrike.strCancelNumber)
                    sw2.Close()

                    '文書通告番号を*****から更新
                    ds.dtHeader.Rows.Item(0).Item("name_strike_info") = dataNameStrike.strNameStrikeNumber
                    reportObj.SetDataSource(ds)
                    fmPreview.PrintOut()

                    '解除指令の印刷
                    fmPreview.ObjResource = reportObj2
                    ds2.dtHeader.Rows.Item(0).Item("name_strike_info") = dataNameStrike.strNameStrikeNumber
                    ds2.dtHeader.Rows.Item(0).Item("c_fight") = dataNameStrike.strCancelNumber
                    reportObj2.SetDataSource(ds2)
                    fmPreview.PrintOut()

                    '同期処理による最新データの反映 サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
                    'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    '画面を閉じる
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
                '===========================================================================================================
                '   データベース切断
                '===========================================================================================================
                clsDb.Disconnect()                  ' ローカルレプリカ
                'clsDbMst.Disconnect()               ' サーバデザインマスタ

                '-------------------------------------------------------------------------------
                '   データベースオブジェクト開放
                '-------------------------------------------------------------------------------
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

#Region "一時保存の一部解除データ本登録・印刷"
    '***************************************************************************************************
    '   ＩＤ　：ConfirmTempReleaseNameStrike
    '   名称　：一時保存の一部解除データ本登録・印刷
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴  ：2012/01/06(金) a.onuma  新規作成
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    Private Sub ConfirmTempReleaseNameStrike()

        Dim blnNoErr As Boolean = False
        Dim clsDb As CLAccessMdb = Nothing                  ' ローカルレプリカ
        'Dim clsDbMst As CLAccessMdbMst = Nothing            ' サーバデザインマスタ
        Dim dataNameStrike As nameStrikeStructureData = Nothing
        Dim dataNameStrikeMember As List(Of nameStrikeMemberStructureData) = Nothing
        Dim strNewNameStrikeId As String = String.Empty
        Dim intBtn As Integer = -1
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P2 = New DS0404P2()
        Dim fmPreview As FM000203 = New FM000203
        Dim releaseUserIdList As List(Of String) = New List(Of String)
        Dim strOfficerName As String = String.Empty

        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 2
        reportObj = New CR0404P5
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
        strStrikeList.AddRange(_NameStrikeData.Item("関連関連ストＩＤ").ToString.Split("-"))
        '通告番号種別（Bのみ）
        drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '期（元の通告の期）
        'drHeader.period_id = MDLoginInfo.Period
        drHeader.period_id = strStrikeList(0)
        '通告番号（連番）
        drHeader.name_strike_info = NOTICE_NUMBER_UNDEFINE
        '日付
        drHeader.info = _dateTimeNow.ToString.Substring(0, 10)
        '代表者
        drHeader.l_stand_name = Me.txtApply.Text.Trim
        '通告番号種別（Bのみ）
        drHeader.strike_strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '解除対象ストライキ文書ID（期ID側）
        drHeader.c_related_info_period_id = strStrikeList(0)
        '解除対象ストライキ文書ID（連番側）
        drHeader.c_related_info = strStrikeList(1)
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
        For intCnt As Integer = 1 To Me.flxNameStrikeMemberCancel.Rows.Count - 1
            If Me.flxNameStrikeMemberCancel.GetCellImage(intCnt, 2) IsNot Nothing Then
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                '社員番号
                drDetail.c_staf_id = Me.flxNameStrikeMemberCancel.GetData(intCnt, 0)
                '氏名
                drDetail.l_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 1)
                '会社所属省略名
                drDetail.local_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 3)
                '機種省略名
                drDetail.k_model = Me.flxNameStrikeMemberCancel.GetData(intCnt, 4)
                '資格
                drDetail.k_qualification = Me.flxNameStrikeMemberCancel.GetData(intCnt, 5)
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
                releaseUserIdList.Add(Me.flxNameStrikeMemberCancel.GetData(intCnt, 0))
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
                dataNameStrikeMember = CreateNameStrikeMemberData(strNewNameStrikeId, releaseUserIdList, False)

                ' オブジェクト生成
                clsDb = New CLAccessMdb             ' ローカルレプリカ
                'clsDbMst = New CLAccessMdbMst       ' サーバデザインマスタ

                '===========================================================================================================
                '   データベース接続
                '===========================================================================================================
                clsDb.Connect()                     ' ローカルレプリカ
                'clsDbMst.Connect()                  ' サーバデザインマスタ

                'トランザクション開始
                clsDb.BeginTran()                   ' ローカルレプリカ
                'clsDbMst.BeginTran()                ' サーバデザインマスタ

                '===============================================================================
                '   指名ストライキ文書の登録
                '===============================================================================
                If InsertNameStrikeData(
                    clsDb,
                    dataNameStrike
                ) = True Then
                    '===========================================================================
                    '   指名ストライキ者の登録
                    '===========================================================================
                    If InsertNameStrikeMemberData(
                        clsDb,
                        dataNameStrikeMember
                    ) = True Then
                        '=======================================================================
                        '   解除対象のストライキ者の更新
                        '=======================================================================
                        If UpdateReleaseMemberData(
                            clsDb,
                            _NameStrikeData.Item("関連関連ストＩＤ"),
                            strNewNameStrikeId,
                            _dateTimeNow,
                            releaseUserIdList
                        ) = True Then
                            '===================================================================
                            '   元の一時保存文書、一時保存ストライキ者の削除
                            '===================================================================
                            If DeleteNameStrikeWork(
                                clsDb,
                                CInt(_NameStrikeData.Item("インデックス"))
                            ) = True Then
                                '===============================================================
                                '   一時保存指名ストライキ者の削除
                                '===============================================================
                                If DeleteNameStrikeMemberDataWork(
                                    clsDb,
                                    CInt(_NameStrikeData.Item("インデックス"))
                                ) = True Then
                                    '正常に終了した場合コミット
                                    blnNoErr = True
                                End If
                            End If
                        Else
                            CLMsg.Show("DE0011")
                        End If
                    End If
                End If

                If blnNoErr = True Then
                    '正常終了の場合、トランザクション確定
                    clsDb.CommitTran()                  ' ローカルレプリカ
                    'clsDbMst.CommitTran()               ' サーバデザインマスタ

                    ' SEQUENCファイルに値を反映
                    Dim strSeqName1 As String
                    strSeqName1 = "seq_str_nms_" + dataNameStrike.strPeriodId + ".txt"
                    Dim sw1 As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName1, False)
                    sw1.Write(dataNameStrike.strNameStrikeNumber)
                    sw1.Close()

                    Dim strSeqName2 As String
                    strSeqName2 = "seq_str_cnl.txt"
                    Dim sw2 As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName2, False)
                    sw2.Write(dataNameStrike.strCancelNumber)
                    sw2.Close()

                    '文書通告番号を*****から更新
                    ds.dtHeader.Rows.Item(0).Item("name_strike_info") = dataNameStrike.strNameStrikeNumber
                    reportObj.SetDataSource(ds)
                    fmPreview.PrintOut()
                    '同期処理による最新データの反映 サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
                    'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    '画面を閉じる
                    Call Me.FormClose()
                Else
                    ' 異常終了の場合、トランザクション取消
                    clsDb.RollbackTran()                ' ローカルレプリカ
                    'clsDbMst.RollbackTran()             ' サーバデザインマスタ
                End If

            Catch ex As Exception
                ' トランザクション取消
                clsDb.RollbackTran()                    ' ローカルレプリカ
                clsDb.RollbackTran()                    ' サーバデザインマスタ
                Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040402, SCREEN_NAME_UC040402, "btnConfirm_Click")
                log.Fatal(ex.Message)

            Finally
                '===============================================================================================================
                '   データベース切断
                '===============================================================================================================
                clsDb.Disconnect()                      ' ローカルレプリカ
                'clsDbMst.Disconnect()                   ' サーバデザインマスタ

                '-------------------------------------------------------------------------------
                '   データベースオブジェクト開放
                '-------------------------------------------------------------------------------
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

#Region "一時保存指名ストライキ一部解除・印刷"
    '***************************************************************************************************
    '   ＩＤ　：NewReleaseNameStrike
    '   名称　：一時保存新規指名ストライキ一部解除・印刷
    '   概要　：
    '   作成日：2012/01/06(金) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/06(金) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub NewReleaseNameStrikeWork()

        Dim blnNoErr As Boolean = False
        Dim clsDb As CLAccessMdb = New CLAccessMdb                  ' ローカルレプリカ
        'Dim clsDbMst As CLAccessMdbMst = New CLAccessMdbMst         ' サーバデザインマスタ
        Dim dataNameStrike As nameStrikeStructureData = Nothing
        Dim dataNameStrikeMember As List(Of nameStrikeMemberStructureData) = Nothing
        Dim strNewNameStrikeId As String = String.Empty
        Dim intNewIndex As Integer = -1
        Dim intBtn As Integer = -1
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0404P2 = New DS0404P2()
        Dim fmPreview As FM000203 = New FM000203
        Dim releaseUserIdList As List(Of String) = New List(Of String)
        Dim strOfficerName As String = String.Empty

        'ボタン表示タイプを設定
        fmPreview.ButtonShowType = 1
        fmPreview.PrintCntVisible = True
        reportObj = New CR0404P5
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
        If _intSelectBtn = 1 Then
            strStrikeList.AddRange(_NameStrikeData.Item("通告番号").ToString.Split("-"))
        Else
            strStrikeList.AddRange(_NameStrikeData.Item("関連関連ストＩＤ").ToString.Split("-"))
        End If
        '通告番号種別（Bのみ）
        drHeader.strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '期
        'drHeader.period_id = MDLoginInfo.Period
        drHeader.period_id = strStrikeList(0)
        '通告番号（連番）
        drHeader.name_strike_info = NOTICE_NUMBER_UNDEFINE
        '日付
        drHeader.info = _dateTimeNow.ToString.Substring(0, 10)
        '代表者
        drHeader.l_stand_name = Me.txtApply.Text.Trim
        '通告番号種別（Bのみ）
        drHeader.strike_strike_info = Me.txtNoticeKind.Text.Substring(0, 1)
        '解除対象ストライキ文書ID（期ID側）
        drHeader.c_related_info_period_id = strStrikeList(0)
        '解除対象ストライキ文書ID（連番側）
        drHeader.c_related_info = strStrikeList(1)
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
        If _intSelectBtn = 1 Then
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
        For intCnt As Integer = 1 To Me.flxNameStrikeMemberCancel.Rows.Count - 1
            'If _intSelectBtn = 1 Then
            '新規一時保存登録の場合、解除フラグがたっている組合員のみ解除者として登録
            If Me.flxNameStrikeMemberCancel.GetCellImage(intCnt, 2) IsNot Nothing Then
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                '社員番号
                drDetail.c_staf_id = Me.flxNameStrikeMemberCancel.GetData(intCnt, 0)
                '氏名
                drDetail.l_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 1)
                '会社所属省略名
                drDetail.local_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 3)
                '機種省略名
                drDetail.k_model = Me.flxNameStrikeMemberCancel.GetData(intCnt, 4)
                '資格
                drDetail.k_qualification = Me.flxNameStrikeMemberCancel.GetData(intCnt, 5)
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
                releaseUserIdList.Add(Me.flxNameStrikeMemberCancel.GetData(intCnt, 0))
            End If
            'Else
            'drDetail = ds.dtDetail.NewRow
            'drDetail.BeginEdit()
            ''社員番号
            'drDetail.c_staf_id = Me.flxNameStrikeMemberCancel.GetData(intCnt, 0)
            ''氏名
            'drDetail.l_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 1)
            ''会社所属省略名
            'drDetail.local_name = Me.flxNameStrikeMemberCancel.GetData(intCnt, 3)
            ''機種省略名
            'drDetail.k_model = Me.flxNameStrikeMemberCancel.GetData(intCnt, 4)
            ''資格
            'drDetail.k_qualification = Me.flxNameStrikeMemberCancel.GetData(intCnt, 5)
            'drDetail.EndEdit()
            'ds.dtDetail.Rows.Add(drDetail) '詳細情報格納
            'releaseUserIdList.Add(Me.flxNameStrikeMemberCancel.GetData(intCnt, 0))
            'End If
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
        If intBtn = 0 OrElse intBtn = 1 Then '新規指名ストライキ登録
            Try
                '同期処理による最新データの取得 サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
                'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                '指名ストライキ文書データの作成
                dataNameStrike = CreateNameStrikeData(True)
                'ストライキ文書IDの取得
                strNewNameStrikeId = dataNameStrike.strNameStrikeId
                'インデックスの取得
                intNewIndex = dataNameStrike.intIndex
                '指名ストライキ者リストの作成
                dataNameStrikeMember = CreateNameStrikeMemberData(strNewNameStrikeId, releaseUserIdList, True, intNewIndex)

                '===========================================================================================================
                '   データベース接続
                '===========================================================================================================
                clsDb.Connect()                 ' ローカルレプリカ
                'clsDbMst.Connect()              ' サーバデザインマスタ

                ' トランザクション開始
                clsDb.BeginTran()               ' ローカルレプリカ
                'clsDbMst.BeginTran()            ' サーバデザインマスタ

                If _intSelectBtn = 1 Then
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
                Else
                    '===========================================================================
                    '   指名ストライキ文書の更新
                    '===========================================================================
                    If UpdateNameStrikeDataWork(
                        clsDb,
                        dataNameStrike
                    ) = True Then
                        '以前に登録されていた指名ストライキ者をいったん削除
                        If DeleteNameStrikeMemberDataWork(
                            clsDb,
                            intNewIndex
                        ) = True Then
                            '===================================================================
                            '   指名ストライキ者の登録
                            '===================================================================
                            If InsertNameStrikeMemberDataWork(
                                clsDb,
                                dataNameStrikeMember
                            ) = True Then
                                '正常に終了した場合コミット
                                blnNoErr = True
                            End If
                        End If
                    End If
                End If

                If blnNoErr = True Then
                    '正常終了の場合、トランザクション確定
                    clsDb.CommitTran()              ' ローカルレプリカ
                    'clsDbMst.CommitTran()           ' サーバデザインマスタ
                    If intBtn = 0 Then
                        fmPreview.PrintOut()
                    End If
                    '同期処理による最新データの反映 サーバデザインマスタ更新処理追加に伴い廃止 2014/12/18
                    'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)
                    '画面を閉じる
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