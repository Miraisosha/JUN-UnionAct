#Region "UC020303"
'===========================================================================================================
'   クラスＩＤ　　：UC020303
'   クラス名称　　：会議通知－合同登録
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst

Public Class UC020303

#Region "変数・定数"
    Private _clsMICommon As CL0203Common = Nothing
    Private _intClickBtnFlg As Integer = 0
    Private _drMeetingInformation As DataRow = Nothing

#Region "プロパティ"
    Public Property MeetingNoticeCommon() As CL0203Common     '会議通知共通処理クラスの取得用  
        Get
            Return _clsMICommon
        End Get
        Set(ByVal value As CL0203Common)
            _clsMICommon = value
        End Set
    End Property
#End Region

#End Region

#Region "イベント"
#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UC020302_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/27(日) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/27(日) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub UC020303_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            ' コンボボックスデータ取得
            If GetData() = False Then
                Exit Sub
            End If
            'クリックボタンフラグを取得
            _intClickBtnFlg = _clsMICommon.IntClickBtnFlg
            '選択されたデータのデータリストを取得
            _drMeetingInformation = _clsMICommon.MeetingInformation


            '初期処理を行う
            Initialize()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "UC020302_Load")
        End Try

    End Sub

#End Region

#Region "登録確認ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnConfirm_Click
    '   名称　：登録確認ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnConfirm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim strMeetingNumber As String = String.Empty

        '各項目入力チェック
        If chkInput() = False Then
            Exit Sub
        End If

        '同一支部での同一会議存在チェック
        If chkSameBranchMeeting() = False Then
            CLMsg.Show("GE0053")
            Exit Sub
        End If

        '同一会議存在チェック
        strMeetingNumber = chkSameMeeting()
        If strMeetingNumber <> String.Empty Then
            If CLMsg.Show("GW0013", strMeetingNumber) = DialogResult.No Then
                Exit Sub
            End If
        End If

        '会議通知のプレビュー画面表示
        showPrintPreview(1)
    End Sub

#End Region

#Region "印刷ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：印刷ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Dim fm000204 As FM000204 = New FM000204()
        fm000204.ShowDialog()

        If fm000204.IntQlickBtnFlag = 0 Then
            '会議通知のプレビュー画面表示
            showPrintPreview(3)
        End If
    End Sub
#End Region

#Region "キャンセルボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタンクリック処理
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim pn As Panel
        Dim uc As Control

        If CLMsg.Show("GQ0007") = DialogResult.No Then
            'いいえが選択された場合は何も行わない
            Exit Sub
        End If

        Me.Visible = False
        pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
        uc = pn.Controls(SCREEN_ID_UC020301)

        If uc Is Nothing Then
            uc = New UC020301
            '会議通知検索画面の表示
            Call pn.Controls.Add(uc)
        Else
            uc.Visible = True
        End If
        Me.Dispose()
    End Sub
#End Region

#Region "内容変更ボタンクリック"
    Private Sub btnChange_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnChange.Click
        '印刷ボタン・内容変更ボタン、戻るを使用不可、
        '登録確認ボタン、キャンセルを使用可にする
        Me.btnPrint.Visible = False
        Me.btnChange.Visible = False
        Me.btnReturn.Visible = False
        Me.btnConfirm.Visible = True
        Me.btnCancel.Visible = True
    End Sub
#End Region

#Region "戻るボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnReturn_Click
    '   名称　：戻るボタンクリック処理
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        Dim pn As Panel
        Dim uc As Control

        Me.Visible = False
        pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
        uc = pn.Controls(SCREEN_ID_UC020301)

        If uc Is Nothing Then
            uc = New UC020301

            Call pn.Controls.Add(uc)
        Else
            uc.Visible = True
        End If
        Me.Dispose()
    End Sub
#End Region

#Region "終了日付チェックボックス値変更"
    '***************************************************************************************************
    '   ＩＤ　：chkEndDateTime_CheckedChanged
    '   名称　：終了日付チェックボックス値変更
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub chkEndDateTime_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkEndDateTime.CheckedChanged
        If chkEndDateTime.CheckState = CheckState.Checked = True Then
            Me.dtpMeetingEndDate.Enabled = True
            Me.dtpEndTime.Enabled = True
            Me.txtReturnMachineName.Enabled = True
            Me.dtpReturnTime.Enabled = True
        Else
            Me.dtpMeetingEndDate.Enabled = False
            Me.dtpEndTime.Enabled = False
            Me.txtReturnMachineName.Enabled = False
            Me.dtpReturnTime.Enabled = False
        End If

    End Sub
#End Region

#End Region

#Region "画面初期表示処理"
    '***************************************************************************************************
    '   ＩＤ　：Initialize
    '   名称　：初期処理
    '   概要  ：画面表示時の初期設定を行う
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/23(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/23(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub Initialize()

        '支部
        If _drMeetingInformation.Item("k_apply_area").Equals(DBNull.Value) = False Then
            Me.cboBranch.SelectedValue() = _drMeetingInformation.Item("k_apply_area")
        End If
        If _drMeetingInformation.Item("k_union") = CL0203Common.UNION_ON Then
            '合同区分
            Me.chkUnion.CheckState = CheckState.Checked
        End If
        '登録日
        If _drMeetingInformation.Item("d_ins").Equals(DBNull.Value) = False Then
            Me.txtInsertDate.Text = _drMeetingInformation.Item("d_ins")
        Else
            Me.txtInsertDate.Text = System.DateTime.Now().Date
        End If

        '会議通知番号
        If _intClickBtnFlg = 3 Then
            '会議通知番号未発行
            Me.txtMeetingNumber.Text = "*****"
        Else
            If _drMeetingInformation.Item("c_meeting").Equals(DBNull.Value) = False Then
                Me.txtMeetingNumber.Text = _drMeetingInformation.Item("c_meeting")
            Else
                Me.txtMeetingNumber.Text = "*****"
            End If
        End If
        '部／委員会
        If _drMeetingInformation.Item("c_committee_id").Equals(DBNull.Value) = False Then
            Me.cboCommittee.SelectedValue() = _drMeetingInformation.Item("c_committee_id")
        End If
        '種類
        If _drMeetingInformation.Item("k_information_type").Equals(DBNull.Value) = False Then
            If _drMeetingInformation.Item("k_information_type").Equals(CL0203Common.INFORMATION_TYPE_OPEN) = True Then
                '開催にチェック
                optOpen.Checked = True
            ElseIf _drMeetingInformation.Item("k_information_type").Equals(CL0203Common.INFORMATION_TYPE_CHANGE) = True Then
                '変更にチェック
                optChange.Checked = True
            ElseIf _drMeetingInformation.Item("k_information_type").Equals(CL0203Common.INFORMATION_TYPE_STOP) = True Then
                '中止にチェック
                optStop.Checked = True
            End If
        End If
        '会議名
        If _drMeetingInformation.Item("l_information_name").Equals(DBNull.Value) = False Then
            Me.cboMeetingName.Text = _drMeetingInformation.Item("l_information_name")
        End If
        '開催場所
        If _drMeetingInformation.Item("l_open_bebiginting").Equals(DBNull.Value) = False Then
            Me.cboMeetingPlace.Text = _drMeetingInformation.Item("l_open_bebiginting")
        End If
        '会議場
        If _drMeetingInformation.Item("l_place").Equals(DBNull.Value) = False Then
            Me.txtMeetingPlace.Text = _drMeetingInformation.Item("l_place")
        End If
        '開始日
        If _drMeetingInformation.Item("d_meeting_1").Equals(DBNull.Value) = False Then
            Me.dtpMeetingStartDate.Value = _drMeetingInformation.Item("d_meeting_1")
        End If
        '開始時間
        If _drMeetingInformation.Item("d_meeting_time_from_1").Equals(DBNull.Value) = False Then
            Me.dtpStartTime.Text = _drMeetingInformation.Item("d_meeting_time_from_1")
        End If

        '終了日
        If _drMeetingInformation.Item("d_meeting_3").Equals(DBNull.Value) = False Then
            '3日目の日付が入っている場合、その日を終了日として設定
            Me.dtpMeetingEndDate.Value = _drMeetingInformation.Item("d_meeting_3")
            Me.dtpEndTime.Text = _drMeetingInformation.Item("d_meeting_time_to_3")
        ElseIf _drMeetingInformation.Item("d_meeting_2").Equals(DBNull.Value) = False Then
            '2日目の日付が入っている場合、その日を終了日として設定
            Me.dtpMeetingEndDate.Value = _drMeetingInformation.Item("d_meeting_2")
            Me.dtpEndTime.Text = _drMeetingInformation.Item("d_meeting_time_to_2")
        Else
            'どちらも未設定の場合、開始日と同じ日を設定
            Me.dtpMeetingEndDate.Value = _drMeetingInformation.Item("d_meeting_1")
            Me.dtpEndTime.Text = _drMeetingInformation.Item("d_meeting_time_to_1")
        End If

        '往路
        If _drMeetingInformation.Item("l_flight_to_1").Equals(DBNull.Value) = False Then
            Me.txtGoMachineName.Text = _drMeetingInformation.Item("l_flight_to_1")
            '往路時間   
            If _drMeetingInformation.Item("d_flight_to_1").Equals(DBNull.Value) = False Then
                If ChkNull(Me.txtGoMachineName.Text) = False Then
                    Me.dtpGoTime.Text = _drMeetingInformation.Item("d_flight_to_1")
                End If
            End If
        End If
        '復路
        If _drMeetingInformation.Item("l_flight_back_1").Equals(DBNull.Value) = False Then
            Me.txtReturnMachineName.Text = _drMeetingInformation.Item("l_flight_back_1")
            '復路時間
            If _drMeetingInformation.Item("d_flight_back_1").Equals(DBNull.Value) = False Then
                If ChkNull(Me.txtReturnMachineName.Text) = False Then
                    Me.dtpReturnTime.Text = _drMeetingInformation.Item("d_flight_back_1")
                End If
            End If
        End If
        '備考
        If _drMeetingInformation.Item("l_biko_1").Equals(DBNull.Value) = False Then
            Me.txtNote.Text = _drMeetingInformation.Item("l_biko_1")
        End If
        If _drMeetingInformation.Item("l_biko_2").Equals(DBNull.Value) = False Then
            Me.txtNote.Text = Me.txtNote.Text & vbCrLf & _drMeetingInformation.Item("l_biko_2")
        End If
        If _drMeetingInformation.Item("l_biko_3").Equals(DBNull.Value) = False Then
            Me.txtNote.Text = Me.txtNote.Text & vbCrLf & _drMeetingInformation.Item("l_biko_3")
        End If
        '議題1～5
        If _drMeetingInformation.Item("l_subject_1").Equals(DBNull.Value) = False Then
            Me.txtTheme1.Text = _drMeetingInformation.Item("l_subject_1")
        End If
        If _drMeetingInformation.Item("l_subject_2").Equals(DBNull.Value) = False Then
            Me.txtTheme2.Text = _drMeetingInformation.Item("l_subject_2")
        End If
        If _drMeetingInformation.Item("l_subject_3").Equals(DBNull.Value) = False Then
            Me.txtTheme3.Text = _drMeetingInformation.Item("l_subject_3")
        End If
        If _drMeetingInformation.Item("l_subject_4").Equals(DBNull.Value) = False Then
            Me.txtTheme4.Text = _drMeetingInformation.Item("l_subject_4")
        End If
        If _drMeetingInformation.Item("l_subject_5").Equals(DBNull.Value) = False Then
            Me.txtTheme5.Text = _drMeetingInformation.Item("l_subject_5")
        End If


        If _intClickBtnFlg = 3 Then
            Me.lblTittle.Text = "会議通知 - 合同登録"
            Me.chkUnion.Enabled = False
            '種類の値を変更できないようにする
            optOpen.Enabled = False
            optChange.Enabled = False
            optStop.Enabled = False
            '種類の背景色を変更する
            optOpen.BackColor = Color.Cornsilk
            optChange.BackColor = Color.Cornsilk
            optStop.BackColor = Color.Cornsilk

            '部／委員会コンボボックスのバックカラー変更
            Me.cboCommittee.BackColor = Color.Cornsilk
            Me.cboCommittee.Enabled = False
            '種類チェックボックスを変更不可にする
            Me.optOpen.Enabled = False
            Me.optChange.Enabled = False
            Me.optStop.Enabled = False
            '会議名コンボボックスのバックカラー変更
            Me.cboMeetingName.BackColor = Color.Cornsilk
            Me.cboMeetingName.Enabled = False
            '開催場所コンボボックスのバックカラー変更
            Me.cboMeetingPlace.BackColor = Color.Cornsilk
            Me.cboMeetingPlace.Enabled = False

        ElseIf _intClickBtnFlg = 4 Then
            Me.lblTittle.Text = "会議通知 - 会議詳細"
            '支部コンボボックスのバックカラー変更
            Me.cboBranch.BackColor = Color.Cornsilk
            Me.cboBranch.Enabled = False
            '印刷ボタンを使用可にする
            Me.btnPrint.Visible = True
            '戻るボタンを使用可にする
            Me.btnReturn.Visible = True

            If dtpMeetingStartDate.Value.Date() >= System.DateTime.Now().Date() Then
                '内容変更ボタンを使用可にする
                Me.btnChange.Visible = True
            End If
            Me.btnConfirm.Visible = False
            Me.btnCancel.Visible = False
        End If

            '登録日を読み取り専用にする
            txtInsertDate.ReadOnly = True
    End Sub
#End Region

#Region "各データ取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要  ：部／委員会コンボボックス作成処理を呼び出す。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False    ' 処理結果
        Dim db As New CLAccessMdb     ' データベースクラス生成

        Try
            ' データベース接続
            db.Connect()

            '---------------------------------------------------------------------------
            '   コンボボックス作成
            '---------------------------------------------------------------------------
            ' 支部コンボボックス作成処理呼び出し
            If CreateCboConstantDtl(db, Me.cboBranch, MDConst.CONSTANT_ID_UI_SHIBU) = False Then
                Return blnRet
            End If

            ' 部/委員会コンボボックス作成処理呼び出し
            If CreateCboCommittee(db) = False Then
                Return blnRet
            End If

            ' 会議名コンボボックス作成処理呼び出し
            If CreateCboMeetingName(db) = False Then
                Return blnRet
            End If

            ' 会議場所コンボボックス作成処理呼び出し
            If CreateCboMeetingPlace(db) = False Then
                Return blnRet
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "GetData")
        Finally
            ' データベース切断
            db.Disconnect()
        End Try

        ' 戻り値格納
        Return blnRet

    End Function

#End Region

#Region "部／委員会コンボボックス作成処理"
    '***************************************************************************************************
    '   ＩＤ　：CreateCbo
    '   名称　：部／委員会コンボボックス作成処理
    '   概要  ：部／委員会コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/18(金) m.miyata
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金) m.miyata  新規作成
    '***************************************************************************************************
    ''' <summary>部／委員会コンボボックス作成処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboCommittee(ByVal db As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文

        Try
            ' 部／委員会コンボボックスクリア
            Me.cboCommittee.Items.Clear()

            ' SQL文
            strSql = "select c_committee_id, l_name from committee order by c_committee_id"
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(db, Me.cboCommittee, strSql, "l_name", "c_committee_id") = False Then
                Return False
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateCbo")
        End Try

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "会議名コンボボックス作成処理"
    '***************************************************************************************************
    '   ＩＤ　：CreateCboMeetingName
    '   名称　：会議名コンボボックス作成処理
    '   概要  ：会議名コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/27(日) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/27(日) a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>会議名コンボボックス作成処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboMeetingName(ByVal db As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果
        Dim strSql As String = ""           ' SQL文

        Try
            ' 会議名コンボボックスクリア
            Me.cboMeetingName.Items.Clear()

            ' SQL文
            strSql = "select c_committee_id, l_name from committee order by c_committee_id"
            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(db, Me.cboMeetingName, strSql, "l_name", "c_committee_id", , MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWN) = False Then
                Return False
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateCbo")
        End Try

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "会議場所コンボボックス作成処理"
    '***************************************************************************************************
    '   ＩＤ　：CreateCboMeetingPlace
    '   名称　：会議場所コンボボックス作成処理
    '   概要  ：会議場所コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/27(日) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/27(日) a.onuma  新規作成
    '***************************************************************************************************
    ''' <summary>会議場所コンボボックス作成処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboMeetingPlace(ByVal db As CLAccessMdb) As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        Try
            ' 会議場所コンボボックスクリア
            Me.cboMeetingPlace.Items.Clear()

            ' コンボボックス作成処理
            If CreateCboConstantDtl(db, Me.cboMeetingPlace, MDConst.CONSTANT_ID_UI_SHIBU, , MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWN) = False Then
                Return False
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateCbo")
        End Try

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "印刷プレビュー画面表示"
    '***************************************************************************************************
    '   ＩＤ　：showPrintPreview
    '   名称　：印刷プレビュー画面表示
    '   概要  ：各入力情報より、印刷プレビュー画面を表示する
    '   引数　：blnWork:一時保存の場合true、本登録の場合false
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub showPrintPreview(ByVal bytBtnType As Byte)
        'データ登録用データ
        Dim data As CL0203Common.miData = Nothing
        Dim blnRet As Boolean = True
        Dim fm As New FM000203
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0203P2 = New DS0203P2()

        'ボタン表示タイプを設定
        fm.ButtonShowType = bytBtnType
        reportObj = New CR0203P2
        fm.ObjResource = reportObj

        Dim drMeetingNotice As DS0203P2.dtHeaderRow
        drMeetingNotice = ds.dtHeader.NewRow
        drMeetingNotice.BeginEdit()

        '登録日？
        drMeetingNotice.d_up = Me.txtInsertDate.Text

        '社員番号
        drMeetingNotice.c_staf_id = "*****"
        '機種
        drMeetingNotice.k_model = "***"
        '指名
        drMeetingNotice.l_name = "**********"

        '画面に入力された情報をセット
        SetReportInfo(drMeetingNotice)

        drMeetingNotice.EndEdit()
        ds.dtHeader.Rows.Add(drMeetingNotice)
        reportObj.SetDataSource(ds)

        Call fm.ShowDialog()
        Select Case fm.IntQlickBtnFlag
            Case 0
                '登録＆印刷

                If _intClickBtnFlg = 3 Then
                    data = CreateInsertData(False)
                    '合同登録処理
                    blnRet = _clsMICommon.InsertData(data)
                Else
                    '会議通知更新処理
                    'UpdateData()
                End If
                '一時保存処理
                'InsertTemporaryData()

                '登録処理呼び出し後、組合員抽出画面呼び出し
                Dim fm000204 As FM000204 = New FM000204()
                fm000204.ShowDialog()

                If fm000204.IntQlickBtnFlag = 1 Then
                    'キャンセルボタンが押下された場合、
                    'その旨を通知
                    CLMsg.Show("GI0019")
                    Exit Sub
                End If

            Case 1
                '登録のみ
                If _intClickBtnFlg = 3 Then
                    data = CreateInsertData(False)
                    '合同登録処理
                    blnRet = _clsMICommon.InsertData(data)
                ElseIf _intClickBtnFlg = 4 Then
                    '会議通知更新処理
                    'UpdateData()
                End If
                '一時保存処理
                'InsertTemporaryData()

            Case 2
                'キャンセルの場合特に何も行わない
            Case 3
                '印刷

        End Select

    End Sub

    Private Sub SetReportInfo(ByVal drMeetingNotice As DS0203P2.dtHeaderRow)
        drMeetingNotice.d_up = Me.txtInsertDate.Text
        '帳票タイトル
        drMeetingNotice.l_title = "会議開催通知"

        '種類
        If optOpen.Checked = True Then
            drMeetingNotice.k_information_type = CL0203Common.INFORMATION_TYPE_OPEN_STRING
        ElseIf optChange.Checked = True Then
            drMeetingNotice.k_information_type = CL0203Common.INFORMATION_TYPE_CHANGE_STRING
        ElseIf optStop.Checked = True Then
            drMeetingNotice.k_information_type = CL0203Common.INFORMATION_TYPE_STOP_STRING
        End If

        '社員番号
        drMeetingNotice.c_staf_id = "*****"
        '機種
        drMeetingNotice.k_model = "***"
        drMeetingNotice.belonging_name = Me.cboBranch.Text
        drMeetingNotice.l_name = "**********"
        drMeetingNotice.l_information_name = Me.cboMeetingName.Text

        '会議通知番号-先頭2文字
        drMeetingNotice.l_ommision_name = MDLoginInfo.Period.ToString()
        If Me.txtMeetingNumber.Text.IndexOf("-") <> -1 Then
            '会議通知番号-後ろ2文字
            If Me.txtMeetingNumber.Text.Length = 4 Then
                drMeetingNotice.c_meeting = Me.txtMeetingNumber.Text.Substring(3, 1)
            ElseIf Me.txtMeetingNumber.Text.Length = 5 Then
                drMeetingNotice.c_meeting = Me.txtMeetingNumber.Text.Substring(3, 2)
            End If
        Else
            '会議通知番号-後ろ2文字
            drMeetingNotice.c_meeting = "***"
        End If

        '開催場所
        drMeetingNotice.l_open_belonging = Me.cboMeetingPlace.Text
        '会議場
        drMeetingNotice.l_place = Me.txtMeetingPlace.Text

        drMeetingNotice.d_meeting_1 = dtpMeetingStartDate.Value.Date()
        drMeetingNotice.d_meeting_time_1 = "開始時間：" & Me.dtpStartTime.Value.Hour().ToString().PadLeft(2, "0") & _
                                           ":" & Me.dtpStartTime.Value.Minute().ToString().PadLeft(2, "0")
        drMeetingNotice.meeting_days_1 = Me.dtpStartTime.Value.Month().ToString() & "/" & Me.dtpStartTime.Value.Day().ToString()
        If chkEndDateTime.CheckState = CheckState.Checked Then
            '会議終了日時が設定されている場合入力内容を反映する
            drMeetingNotice.d_meeting_time_1 = drMeetingNotice.d_meeting_time_1 & "　　　から"
            drMeetingNotice.d_meeting_3 = dtpMeetingEndDate.Value.Date()
            drMeetingNotice.d_meeting_time_3 = Me.dtpEndTime.Value.Hour().ToString().PadLeft(2, "0") & _
                                               ":" & Me.dtpEndTime.Value().Minute().ToString().PadLeft(2, "0") & "　　　まで"
        End If

        '備考
        drMeetingNotice.l_biko_1 = Me.txtNote.Text
        '議題1～5
        drMeetingNotice.l_subject_1 = Me.txtTheme1.Text
        drMeetingNotice.l_subject_2 = Me.txtTheme2.Text
        drMeetingNotice.l_subject_3 = Me.txtTheme3.Text
        drMeetingNotice.l_subject_4 = Me.txtTheme4.Text
        drMeetingNotice.l_subject_5 = Me.txtTheme5.Text
        drMeetingNotice.committee_name = Me.cboCommittee.Text

        '移動‐往路が設定されている場合
        If ChkNull(Me.txtGoMachineName.Text) = False Then
            drMeetingNotice.l_flight_to = Me.txtGoMachineName.Text  '移動便名の設定
            drMeetingNotice.d_flightday_to = dtpMeetingStartDate.Value.Date() '出発日の設定
            drMeetingNotice.d_filght_to = dtpGoTime.Value.Hour().ToString().PadLeft(2, "0") & "時" & _
                                        dtpGoTime.Value.Minute().ToString().PadLeft(2, "0") & "分" '出発時間の設定
        End If
        '移動‐復路が設定されている場合
        If ChkNull(Me.txtReturnMachineName.Text) = False Then
            drMeetingNotice.l_flight_back = Me.txtReturnMachineName.Text  '移動便名の設定
            drMeetingNotice.d_flightday_back = dtpMeetingEndDate.Value.Date()
            drMeetingNotice.d_flight_back = dtpReturnTime.Value.Hour().ToString().PadLeft(2, "0") & "時" & _
                                        dtpReturnTime.Value.Minute().ToString().PadLeft(2, "0") & "分" '出発時間の設定"
        End If
    End Sub

    Private Sub PrintReport()

    End Sub

#End Region

#Region "データ登録処理"
    '***************************************************************************************************
    '   ＩＤ　：InsertData
    '   名称　：データ登録処理
    '   概要  ：Insert文を発行し、テーブルへデータを登録する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function InsertData() As Boolean
        'Insert用データ
        Dim data As CL0203Common.miData = Nothing
        'SQL文
        Dim strSql As String = String.Empty
        'SQL実行結果
        Dim intRet As Integer = -1
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '処理結果
        Dim blnRet As Boolean = False

        Try
            '登録データの取得
            data = CreateInsertData(False)

            'Insert文の作成
            strSql = "Insert Into meeting_information( " & _
                     "c_ksh " & _
                     ",k_apply_area " & _
                     ",c_period_id " & _
                     ",c_meeting " & _
                     ",s_meeting " & _
                     ",c_committee_id " & _
                     ",k_union " & _
                     ",k_information_type " & _
                     ",l_information_name " & _
                     ",d_meeting_1 " & _
                     ",d_meeting_time_from_1 " & _
                     ",d_meeting_time_to_1 " & _
                     ",l_flight_to_1 " & _
                     ",d_flight_to_1 " & _
                     ",l_flight_back_1 " & _
                     ",d_flight_back_1 " & _
                     ",d_meeting_2 " & _
                     ",d_meeting_time_from_2 " & _
                     ",d_meeting_time_to_2 " & _
                     ",l_flight_to_2 " & _
                     ",d_flight_to_2 " & _
                     ",l_flight_back_2 " & _
                     ",d_flight_back_2 " & _
                     ",d_meeting_3 " & _
                     ",d_meeting_time_from_3 " & _
                     ",d_meeting_time_to_3 " & _
                     ",l_flight_to_3 " & _
                     ",d_flight_to_3 " & _
                     ",l_flight_back_3 " & _
                     ",d_flight_back_3 " & _
                     ",l_open_bebiginting " & _
                     ",l_place " & _
                     ",l_subject_1 " & _
                     ",l_subject_2 " & _
                     ",l_subject_3 " & _
                     ",l_subject_4 " & _
                     ",l_subject_5 " & _
                     ",l_biko_1 " & _
                     ",l_biko_2 " & _
                     ",l_biko_3 " & _
                     ",d_ins " & _
                     ",c_user_id_ins " & _
                     ",d_up " & _
                     ",c_user_id_up " & _
                     ",s_up " & _
                     ") VALUES( " & _
                     "'" & data.strKsh & "' " & _
                     ",'" & data.strApplyAreaCode & "' " & _
                     ",'" & data.strPeriodId & "' " & _
                     ",'" & data.strMeetingNumber & "' " & _
                     "," & data.intSeq & _
                     ",'" & data.strCommitteeId & "' " & _
                     ",'" & data.strUnioncode & "' " & _
                     ",'" & data.strInformationTypeCode & "' " & _
                     ",'" & data.strInformationName & "' " & _
                     "," & data.dtmMeeting1 & _
                     ",'" & data.strMeetingTimeFrom1 & "' " & _
                     ",'" & data.strMeetingTimeTo1 & "' " & _
                     ",'" & data.strFlightTo1 & "' " & _
                     ",'" & data.strFlightTimeTo1 & "' " & _
                     ",'" & data.strFlightBack1 & "' " & _
                     ",'" & data.strFlightTimeBack1 & "' " & _
                     "," & data.dtmMeeting2 & _
                     ",'" & data.strMeetingTimeFrom2 & "' " & _
                     ",'" & data.strMeetingTimeTo2 & "' " & _
                     ",'" & data.strFlightTo2 & "' " & _
                     ",'" & data.strFlightTimeTo2 & "' " & _
                     ",'" & data.strFlightBack2 & "' " & _
                     ",'" & data.strFlightTimeBack2 & "' " & _
                     "," & data.dtmMeeting3 & _
                     ",'" & data.strMeetingTimeFrom3 & "' " & _
                     ",'" & data.strMeetingTimeTo3 & "' " & _
                     ",'" & data.strFlightTo3 & "' " & _
                     ",'" & data.strFlightTimeTo3 & "' " & _
                     ",'" & data.strFlightBack3 & "' " & _
                     ",'" & data.strFlightTimeBack3 & "' " & _
                     ",'" & data.strOpenBeBiginting & "' " & _
                     ",'" & data.strPlace & " ' " & _
                     ",'" & data.strSubject1 & "' " & _
                     ",'" & data.strSubject2 & "' " & _
                     ",'" & data.strSubject3 & "' " & _
                     ",'" & data.strSubject4 & "' " & _
                     ",'" & data.strSubject5 & "' " & _
                     ",'" & data.strBiko1 & "' " & _
                     ",'" & data.strBiko2 & "' " & _
                     ",'" & data.strBiko3 & "' " & _
                     "," & data.dtmInsertDate & _
                     ",'" & data.strInsertUserId & "' " & _
                     "," & data.dtmUpdateDate & _
                     ",'" & data.strUpdateUserId & "' " & _
                     "," & data.intUpdateTime & ")"

            'DB接続&トランザクション開始
            clsMdb.Connect()
            clsMdb.BeginTran()

            'SQL実行
            intRet = clsMdb.ExecuteNonQuery(strSql)
            If intRet = 1 Then
                'コミット
                clsMdb.CommitTran()
            Else
                '処理結果が1行でない場合はロールバック
                clsMdb.RollbackTran()
            End If

            blnRet = True
        Catch ex As Exception
            'ロールバック
            clsMdb.RollbackTran()

        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try

        Return blnRet
    End Function

#End Region

#Region "登録データ作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateData
    '   名称　：登録データ作成
    '   概要  ：画面に入力されたデータより、テーブルへ登録するデータを作成する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function CreateInsertData(ByVal blnWork As Boolean) As CL0203Common.miData
        Dim data As CL0203Common.miData = New CL0203Common.miData
        Dim strMeetingNumber As String = String.Empty
        Dim strNewSeq As String = String.Empty

        '会議通知番号の作成
        strNewSeq = GetMeetingNumber(blnWork)
        If strNewSeq = String.Empty Then
            strMeetingNumber = String.Empty
        Else
            strMeetingNumber = MDLoginInfo.Period.ToString() & "-" & strNewSeq
        End If

        '会社コード
        data.strKsh = MDLoginInfo.OffceCode

        '申請地区区分
        If Me.cboBranch.SelectedValue() IsNot DBNull.Value Then
            data.strApplyAreaCode = Me.cboBranch.SelectedValue()
        Else
            If Me.cboBranch.Text = "東京" Then
                data.strApplyAreaCode = MDConst.UNION_BRANCH_TOKYO.ToString().PadLeft(2, "0")
            ElseIf Me.cboBranch.Text = "大阪" Then
                data.strApplyAreaCode = MDConst.UNION_BRANCH_OOSAKA.ToString().PadLeft(2, "0")
            ElseIf Me.cboBranch.Text = "その他" Then
                data.strApplyAreaCode = ""
            Else
                data.strApplyAreaCode = String.Empty
            End If
        End If
        '期ID
        data.strPeriodId = MDLoginInfo.PeriodId
        '会議番号
        data.strMeetingNumber = strMeetingNumber
        '会議番号SEQ
        If strNewSeq = String.Empty Then
            data.intSeq = 0
        Else
            data.intSeq = CInt(strNewSeq)
        End If

        '委員会ID
        If Me.cboCommittee.SelectedValue() IsNot DBNull.Value Then
            data.strCommitteeId = Me.cboCommittee.SelectedValue()
        Else
            data.strCommitteeId = String.Empty
        End If
        '合同区分
        If Me.chkUnion.CheckState = CheckState.Checked Then
            data.strUnioncode = CL0203Common.UNION_ON
        Else
            data.strUnioncode = CL0203Common.UNION_OFF
        End If
        '種類
        If Me.optOpen.Checked = True Then
            '開催
            data.strInformationTypeCode = CL0203Common.INFORMATION_TYPE_OPEN
        ElseIf Me.optChange.Checked = True Then
            '変更
            data.strInformationTypeCode = CL0203Common.INFORMATION_TYPE_CHANGE
        Else
            '中止
            data.strInformationTypeCode = CL0203Common.INFORMATION_TYPE_STOP
        End If
        '会議名（目的）
        data.strInformationName = Me.cboMeetingName.Text
        '会議日付1
        data.dtmMeeting1 = "#" & Me.dtpMeetingStartDate.Value.Date().ToString() & "#"
        '会議時間From1
        data.strMeetingTimeFrom1 = Me.dtpStartTime.Value.Hour().ToString() & "時" & Me.dtpStartTime.Value.Minute().ToString().PadLeft(2, "0") & "分"
        '会議時間To1
        data.strMeetingTimeTo1 = Me.dtpEndTime.Value.Hour().ToString() & "時" & Me.dtpEndTime.Value.Minute().ToString().PadLeft(2, "0") & "分"
        '移動フライト往路1
        data.strFlightTo1 = Me.txtGoMachineName.Text
        '移動フライト時間往路1
        data.strFlightTimeTo1 = Me.dtpGoTime.Value.Hour().ToString() & "時" & Me.dtpGoTime.Value.Minute().ToString.PadLeft(2, "0") & "分"
        '移動フライト復路1
        data.strFlightTo1 = Me.txtGoMachineName.Text
        '移動フライト時間復路1
        data.strFlightTimeTo1 = Me.dtpGoTime.Value.Hour().ToString() & "時" & Me.dtpGoTime.Value.Minute().ToString.PadLeft(2, "0") & "分"

        If _drMeetingInformation.Item("d_meeting_2").Equals(DBNull.Value) = False Then
            data.dtmMeeting2 = _drMeetingInformation.Item("d_meeting_2")
        End If
        data.strMeetingTimeFrom2 = _drMeetingInformation.Item("d_meeting_time_from_2")
        data.strMeetingTimeTo2 = _drMeetingInformation.Item("d_meeting_time_to_2")

        If _drMeetingInformation.Item("d_meeting_3").Equals(DBNull.Value) = False Then
            data.dtmMeeting3 = _drMeetingInformation.Item("d_meeting_3")
        Else
            data.dtmMeeting3 = "Null"
        End If
        data.strMeetingTimeFrom3 = _drMeetingInformation.Item("d_meeting_time_from_3")
        data.strMeetingTimeTo3 = _drMeetingInformation.Item("d_meeting_time_to_3")

        '開催場所
        data.strOpenBeBiginting = Me.cboMeetingPlace.Text
        '会議場所
        data.strPlace = Me.txtMeetingPlace.Text
        '議題1
        data.strSubject1 = Me.txtTheme1.Text
        '議題2
        data.strSubject2 = Me.txtTheme2.Text
        '議題3
        data.strSubject3 = Me.txtTheme3.Text
        '議題4
        data.strSubject4 = Me.txtTheme4.Text
        '議題5
        data.strSubject5 = Me.txtTheme5.Text

        If Me.txtNote.Lines.Length >= 1 Then
            '備考1
            data.strBiko1 = Me.txtNote.Lines(0)
        End If
        If Me.txtNote.Lines.Length >= 2 Then
            '備考2
            data.strBiko2 = Me.txtNote.Lines(1)
        End If
        If Me.txtNote.Lines.Length >= 3 Then
            '備考3
            data.strBiko3 = Me.txtNote.Lines(2)
        End If

        '作成日
        data.dtmInsertDate = "#" & Me.txtInsertDate.Text & "#"
        '作成者個人ID
        data.strInsertUserId = MDLoginInfo.UserId
        '更新日
        data.dtmUpdateDate = "#" & System.DateTime.Now().Date.ToString() & "#"
        '更新者個人ID
        data.strUpdateUserId = MDLoginInfo.UserId
        '更新回数
        data.intUpdateTime = 0


        Return data
    End Function

#End Region

#Region "登録データ作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateUpdateData
    '   名称　：登録データ作成
    '   概要  ：画面に入力されたデータより、テーブルへ登録するデータを作成する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function CreateUpdateData(ByVal blnWork As Boolean) As CL0203Common.miData
        Dim data As CL0203Common.miData = New CL0203Common.miData
        Dim strMeetingNumber As String = String.Empty
        Dim strNewSeq As String = String.Empty

        If Me.txtMeetingNumber.Text.IndexOf("*") <> -1 Then
            '会議通知番号の作成
            strNewSeq = GetMeetingNumber(blnWork)
            If strNewSeq = String.Empty Then
                strMeetingNumber = String.Empty
            Else
                strMeetingNumber = MDLoginInfo.Period.ToString() & "-" & strNewSeq
            End If
        End If


        '会社コード
        data.strKsh = MDLoginInfo.OffceCode

        '申請地区区分
        If Me.cboBranch.SelectedValue() IsNot DBNull.Value Then
            data.strApplyAreaCode = Me.cboBranch.SelectedValue()
        Else
            If Me.cboBranch.Text = "東京" Then
                data.strApplyAreaCode = MDConst.UNION_BRANCH_TOKYO.ToString().PadLeft(2, "0")
            ElseIf Me.cboBranch.Text = "大阪" Then
                data.strApplyAreaCode = MDConst.UNION_BRANCH_OOSAKA.ToString().PadLeft(2, "0")
            ElseIf Me.cboBranch.Text = "その他" Then
                data.strApplyAreaCode = ""
            Else
                data.strApplyAreaCode = String.Empty
            End If
        End If
        '期ID
        data.strPeriodId = MDLoginInfo.PeriodId
        '会議番号
        data.strMeetingNumber = strMeetingNumber
        '会議番号SEQ
        If strNewSeq = String.Empty Then
            data.intSeq = 0
        Else
            data.intSeq = CInt(strNewSeq)
        End If

        '委員会ID
        If Me.cboCommittee.SelectedValue() IsNot DBNull.Value Then
            data.strCommitteeId = Me.cboCommittee.SelectedValue()
        Else
            data.strCommitteeId = String.Empty
        End If
        '合同区分
        If Me.chkUnion.CheckState = CheckState.Checked Then
            data.strUnioncode = CL0203Common.UNION_ON
        Else
            data.strUnioncode = CL0203Common.UNION_OFF
        End If
        '種類
        If Me.optOpen.Checked = True Then
            '開催
            data.strInformationTypeCode = CL0203Common.INFORMATION_TYPE_OPEN
        ElseIf Me.optChange.Checked = True Then
            '変更
            data.strInformationTypeCode = CL0203Common.INFORMATION_TYPE_CHANGE
        Else
            '中止
            data.strInformationTypeCode = CL0203Common.INFORMATION_TYPE_STOP
        End If
        '会議名（目的）
        data.strInformationName = Me.cboMeetingName.Text
        '会議日付1
        data.dtmMeeting1 = "#" & Me.dtpMeetingStartDate.Value.Date().ToString() & "#"
        '会議時間From1
        data.strMeetingTimeFrom1 = Me.dtpStartTime.Value.Hour().ToString() & "時" & Me.dtpStartTime.Value.Minute().ToString().PadLeft(2, "0") & "分"
        '会議時間To1
        data.strMeetingTimeTo1 = Me.dtpEndTime.Value.Hour().ToString() & "時" & Me.dtpEndTime.Value.Minute().ToString().PadLeft(2, "0") & "分"
        '移動フライト往路1
        data.strFlightTo1 = Me.txtGoMachineName.Text
        '移動フライト時間往路1
        data.strFlightTimeTo1 = Me.dtpGoTime.Value.Hour().ToString() & "時" & Me.dtpGoTime.Value.Minute().ToString.PadLeft(2, "0") & "分"
        '移動フライト復路1
        data.strFlightBack1 = Me.txtReturnMachineName.Text
        '移動フライト時間復路1
        data.strFlightTimeBack1 = Me.dtpReturnTime.Value.Hour().ToString() & "時" & Me.dtpReturnTime.Value.Minute().ToString.PadLeft(2, "0") & "分"

        data.dtmMeeting2 = "Null"
        data.strMeetingTimeFrom2 = String.Empty
        data.strMeetingTimeTo2 = String.Empty

        data.dtmMeeting3 = "Null"
        data.strMeetingTimeFrom3 = String.Empty
        data.strMeetingTimeTo3 = String.Empty

        '開催場所
        data.strOpenBeBiginting = Me.cboMeetingPlace.Text
        '会議場所
        data.strPlace = Me.txtMeetingPlace.Text
        '議題1
        data.strSubject1 = Me.txtTheme1.Text
        '議題2
        data.strSubject2 = Me.txtTheme2.Text
        '議題3
        data.strSubject3 = Me.txtTheme3.Text
        '議題4
        data.strSubject4 = Me.txtTheme4.Text
        '議題5
        data.strSubject5 = Me.txtTheme5.Text

        If Me.txtNote.Lines.Length >= 1 Then
            '備考1
            data.strBiko1 = Me.txtNote.Lines(0)
        End If
        If Me.txtNote.Lines.Length >= 2 Then
            '備考2
            data.strBiko2 = Me.txtNote.Lines(1)
        End If
        If Me.txtNote.Lines.Length >= 3 Then
            '備考3
            data.strBiko3 = Me.txtNote.Lines(2)
        End If

        '更新日
        data.dtmUpdateDate = "#" & System.DateTime.Now().Date.ToString() & "#"
        '更新者個人ID
        data.strUpdateUserId = MDLoginInfo.UserId
        '更新回数
        data.intUpdateTime = CInt(_drMeetingInformation.Item("s_up")) + 1

        Return data
    End Function

#End Region

#Region "会議通知番号取得処理"
    Private Function GetMeetingNumber(ByVal blnWork As Boolean) As String
        'SQL文
        Dim strSql As String = String.Empty
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '会議通知番号
        Dim strNewMeetingNumber As String = String.Empty
        '処理結果格納
        Dim tbRet As DataTable = Nothing
        '選択中の支部
        Dim strSelectedAreaCode As String = String.Empty

        If blnWork = True Then
            '一時保存時は新規会議通知番号は発行しない
            If Me.optChange.Checked = True OrElse Me.optStop.Checked = True Then
                strNewMeetingNumber = _drMeetingInformation.Item("c_meeting")
            End If

            Return strNewMeetingNumber
        End If

        Try
            If Me.cboBranch.Text = CL0203Common.SHIBU_TOKYO_STRING Then
                strSelectedAreaCode = MDConst.UI_SHIBU_TOKYO
            ElseIf Me.cboBranch.Text = CL0203Common.SHIBU_OOSAKA_STRING Then
                strSelectedAreaCode = MDConst.UI_SHIBU_OSAKA
            ElseIf Me.cboBranch.Text = CL0203Common.SHIBU_OTHER_STRING Then
                strSelectedAreaCode = MDConst.UI_SHIBU_ETC
            End If
            strSql = "SELECT MAX(s_meeting) FROM meeting_information " & _
                     "WHERE c_meeting LIKE'" & MDLoginInfo.Period & "%' " & _
                     "AND k_apply_area = '" & strSelectedAreaCode & "' "

            'DB接続開始
            clsMdb.Connect()

            tbRet = clsMdb.ExecuteSql(strSql)
            If tbRet.Rows.Count > 0 Then
                If tbRet.Rows(0).Item(0) Is DBNull.Value Then
                    strNewMeetingNumber = "1"
                Else
                    strNewMeetingNumber = (CInt(tbRet.Rows(0).Item(0)) + 1).ToString()
                End If
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "GetMeetingNumber")
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try

        '取得した最大番号を返却
        Return strNewMeetingNumber
    End Function

#End Region

#Region "同一支部での同一会議存在チェック"
    '***************************************************************************************************
    '   ＩＤ　：chkSameBranchMeeting
    '   名称　：同一支部での同一会議存在チェック
    '   概要  ：既に同一支部で同一会議が登録されているかチェックします
    '   引数　：なし
    '   戻り値：True = 同一会議なし, False = 同一会議あり
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function chkSameBranchMeeting() As Boolean
        '処理結果
        Dim blnRet As Boolean = True
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '種類
        Dim strInformationType As String = String.Empty

        'Select文
        Dim strSql As String = String.Empty
        '処理結果格納
        Dim tbRet As DataTable = Nothing

        If optOpen.Checked = True Then
            strInformationType = CL0203Common.INFORMATION_TYPE_OPEN
        ElseIf optChange.Checked = True Then
            strInformationType = CL0203Common.INFORMATION_TYPE_CHANGE
        Else
            strInformationType = CL0203Common.INFORMATION_TYPE_STOP
        End If
        Try
            strSql = " SELECT meeting_information.c_meeting FROM meeting_information " & _
                 " WHERE k_apply_area = '" & Me.cboBranch.SelectedValue() & "' " & _
                 " AND k_union = '" & CL0203Common.UNION_ON & "' " & _
                 " AND c_committee_id = '" & Me.cboCommittee.SelectedValue() & "' " & _
                 " AND k_information_type = '" & strInformationType & "' " & _
                 " AND l_information_name = '" & Me.cboMeetingName.Text & "' " & _
                 " AND l_open_bebiginting = '" & Me.cboMeetingPlace.Text & "' " & _
                 " AND d_meeting_1 = #" & Me.dtpMeetingStartDate.Value.Date() & "# "

            'DB接続開始
            clsMdb.Connect()

            tbRet = clsMdb.ExecuteSql(strSql)
            If tbRet.Rows.Count > 0 Then
                'データが存在する場合、同一会議存在フラグをたてる
                blnRet = False
            End If

            Return blnRet

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "chkSameMeeting")
        Finally
            clsMdb.Disconnect()
        End Try
        Return blnRet

    End Function

#End Region

#Region "データ入力チェック"
    '***************************************************************************************************
    '   ＩＤ　：chkInput
    '   名称　：データ入力チェック
    '   概要  ：必須データ、入力データの整合性をチェックする
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/21(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function chkInput() As Boolean
        Dim blnRet As Boolean = True        ' 処理結果
        Dim errMsg As ArrayList = New ArrayList ' エラーメッセージリスト

        If errMsg.Count > 0 Then
            errMsg.Clear()
        End If

        Try
            '支部
            If ChkNull(Me.cboBranch.Text) Then
                SetErr(Me.cboBranch)
                errMsg.Add(CLMsg.GetMsg("GE0006", "支部"))
            Else
                Me.cboBranch.BackColor = Color.White
            End If

            '部／委員会
            If ChkNull(Me.cboCommittee.Text) Then
                SetErr(Me.cboCommittee)
                errMsg.Add(CLMsg.GetMsg("GE0006", "部／委員会"))
            Else
                Me.cboCommittee.BackColor = Color.White
            End If

            '会議名
            If ChkNull(Me.cboMeetingName.Text) Then
                SetErr(Me.cboMeetingName)
                errMsg.Add(CLMsg.GetMsg("GE0006", "会議名"))
            Else
                Me.cboMeetingName.BackColor = Color.White
            End If

            '開催場所
            If ChkNull(Me.cboMeetingPlace.Text) Then
                SetErr(Me.cboMeetingPlace)
                errMsg.Add(CLMsg.GetMsg("GE0006", "開催場所"))
            Else
                Me.cboMeetingPlace.BackColor = Color.White
            End If

            '日時
            If ChkNull(Me.dtpMeetingStartDate.Text) Then
                SetErr(Me.dtpMeetingStartDate)
                errMsg.Add(CLMsg.GetMsg("GE0006", "日時"))
            Else
                Me.dtpMeetingStartDate.BackColor = Color.White
            End If

            'エラーメッセージが一つでも格納された場合、falseを返却
            If errMsg.Count > 0 Then
                blnRet = False
                ' エラーメッセージボックス表示
                Dim clsUC999999 As New UC999999
                clsUC999999.errMsgList = errMsg

                ' メインメニュー画面表示
                Call clsUC999999.ShowDialog()
            End If

            Return blnRet
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "ChkInput")
        End Try

    End Function
#End Region

#Region "同一会議通知存在チェック"
    '***************************************************************************************************
    '   ＩＤ　：chkSameMeeting
    '   名称　：同一会議通知存在チェック
    '   概要  ：同一支部、同一委員会、同一開催日の会議通知が存在するかチェックします
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '**************************************************************************************************
    Private Function chkSameMeeting() As String
        '処理結果
        Dim strMeetingNumber As String = String.Empty
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '選択した支部のコード

        'Select文
        Dim strSql As String = String.Empty
        '処理結果格納
        Dim tbRet As DataTable = Nothing

        Try
            strSql = " SELECT meeting_information.c_meeting FROM meeting_information " & _
                 " WHERE k_apply_area = '" & Me.cboBranch.SelectedValue() & "' " & _
                 " AND c_committee_id = '" & Me.cboCommittee.SelectedValue() & "' " & _
                 " AND d_meeting_1 = #" & Me.dtpMeetingStartDate.Value.Date().ToString() & "# "

            'DB接続開始
            clsMdb.Connect()

            tbRet = clsMdb.ExecuteSql(strSql)
            If tbRet.Rows.Count > 0 Then
                For Each row As DataRow In tbRet.Rows
                    If strMeetingNumber = String.Empty Then
                        strMeetingNumber = row.Item(0)
                    Else
                        strMeetingNumber = strMeetingNumber & "," & row.Item(0)
                    End If

                Next
            End If

            Return strMeetingNumber

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "chkSameMeeting")
        Finally
            clsMdb.Disconnect()
        End Try
        Return strMeetingNumber
    End Function
#End Region

End Class

#End Region
