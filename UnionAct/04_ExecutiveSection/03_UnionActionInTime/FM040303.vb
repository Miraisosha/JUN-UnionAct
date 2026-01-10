Imports C1.Win.C1FlexGrid
Imports UnionAct.NSMDConst
Imports UnionAct.NSMDCommon
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDInfo
Imports UnionAct.GUI.Common

Public Class FM040303
    Dim agoUserControl As System.Windows.Forms.UserControl
    Dim omission As Hashtable
    'ログ出力定義
    Private Const SCREEN_ID As String = SCREEN_ID_FM040303
    Private Const SCREEN_NAME As String = SCREEN_NAME_FM040303
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)
    Public Sub New()
        'ここに初期処理を書く
        InitializeComponent()
    End Sub

    Public Sub New(ByVal setForm As System.Windows.Forms.UserControl)
        'ここに初期処理を書く
        InitializeComponent()
        agoUserControl = setForm
    End Sub

#Region "キャンセル"
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Dispose()
    End Sub
#End Region

#Region "FM040303_Load"
    Private Sub FM040303_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dbAccess As New CLAccessMdb             '共通ＤＢアクセスクラス
        Dim sql As String                           'SQL文
        Dim dt As DataTable                         'データテーブル
        Dim iCounter As Integer                     'カウンター
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            grpExecutive.Visible = False
            btnOkHeto.Visible = False
            btnOk.Visible = True
            btnOk.Location = New Point(182, 110)
            btnCancel.Location = New Point(313, 110)
            Me.Height = 189
            Me.Location = New Point(335, 224)
            dbAccess.Connect()
            '種類略名称表示
            omission = New Hashtable
            sql = ""
            sql = sql & "   SELECT c_constant_seq" & vbCrLf
            sql = sql & "         ,l_omission_name" & vbCrLf
            sql = sql & "     FROM constant_dtl" & vbCrLf
            sql = sql & "    WHERE c_constant = '" & CONSTANT_APPLY_CLASSIFY & "'" & vbCrLf
            sql = sql & " ORDER BY s_order" & vbCrLf
            dt = dbAccess.ExecuteSql(sql)
            If dt.Rows.Count > 0 Then
                For iCounter = 0 To dt.Rows.Count - 1
                    omission.Add(dt.Rows(iCounter)(0), dt.Rows(iCounter)(1))
                Next
            End If
            ' 支部コンボボックス
            Call CreateCboConstantDtl(dbAccess, cmbApplyArea, CONSTANT_ID_APPLY_AREA, True, 2, 0)
            ' 種類コンボボックス
            Call CreateCboConstantDtl(dbAccess, cmbApplyClassify, CONSTANT_APPLY_CLASSIFY, True, 2, 0)

            lblKind.Text = cmbApplyClassify.Text
            lblSHIBU.Text = cmbApplyArea.Text
            lblKind2.Text = omission(cmbApplyClassify.SelectedValue)
            flxExecutive.Cols.Count = 5
            flxExecutive.Cols(4).Visible = False
            flxExecutive.Cols(0).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            flxExecutive.Cols(1).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            flxExecutive.Cols(2).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            flxExecutive.Cols(3).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
            flxExecutive.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
            flxExecutive.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
            flxExecutive.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
            flxExecutive.AllowEditing = False
            flxExecutive.AutoResize = False

        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
        End Try
        '処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "種別ダイアログ　値変更"
    Private Sub cmbApplyClassify_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbApplyClassify.SelectedIndexChanged
        ' 処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

        Try
            lblKind2.Text = omission(cmbApplyClassify.SelectedValue)
            If lblKind.Text <> cmbApplyClassify.Text Then
                grpExecutive.Visible = False
                btnOkHeto.Visible = False
                btnOk.Visible = True
                btnOk.Location = New Point(182, 110)
                btnCancel.Location = New Point(313, 110)
                Me.Height = 189
                lblKind.Text = cmbApplyClassify.Text
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040303, SCREEN_NAME_FM040303, "FM040303_Load")
        End Try

        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

#Region "支部ダイアログ　値変更"
    Private Sub cmbApplyArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbApplyArea.SelectedIndexChanged
        ' 処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)
        Try
            If lblSHIBU.Text <> cmbApplyArea.Text Then
                grpExecutive.Visible = False
                btnOkHeto.Visible = False
                btnOk.Visible = True
                btnOk.Location = New Point(182, 110)
                btnCancel.Location = New Point(313, 110)
                Me.Height = 189
                lblSHIBU.Text = cmbApplyArea.Text
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040303, SCREEN_NAME_FM040303, "cmbApplyArea_SelectedIndexChanged")
        End Try
        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

#Region "支部・種類選択後ＯＫボタン"
    'OKボタンクリック
    Public Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim dbAccess As New CLAccessMdb                         'DBアクセス
        Dim sql As String                                       'SQL文
        Dim dt As DataTable                                     'データテーブル
        Dim iCounter As Integer                                 'カウンター
        Try
            ' 処理開始ログ
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

            Me.cmbApplyArea.BackColor = Color.White
            Me.cmbApplyClassify.BackColor = Color.White
            If Me.cmbApplyArea.Text.ToString.Equals("") Then
                Me.cmbApplyArea.BackColor = Color.LightPink
                CLMsg.Show("GE0006", cmbApplyArea.Tag.ToString)
                Exit Sub
            End If
            If Me.cmbApplyClassify.Text.ToString.Equals("") Then
                Me.cmbApplyClassify.BackColor = Color.LightPink
                CLMsg.Show("GE0006", cmbApplyClassify.Tag.ToString)
                Exit Sub
            End If

            '現在日
            Dim systemDate As String = Format(Now, DATE_YYYYMMDD_8_FORMAT)

            flxExecutive.Rows.Count = 1
            'DBより登録済みのデータを取得
            If cmbApplyClassify.SelectedValue = "01" Or cmbApplyClassify.SelectedValue = "02" Then
                '中央執行委員会あるいは中央委員会の場合
                flxExecutive.Visible = True
                flxUnionInformation.Visible = False
                grpExecutive.Text = "該当する中央執行委員会・中央委員会の日程を選択"
                sql = "Select apply_strike_executive_term.d_from,apply_strike_executive_term.d_to,apply_strike_executive_term.l_biko_1,staf_attribute_full_time_now_name_view.l_name,apply_strike_executive_term.c_apply_strike_term_id "
                sql = sql & "from apply_strike_executive_term left join staf_attribute_full_time_now_name_view on apply_strike_executive_term.c_user_id_ins=staf_attribute_full_time_now_name_view.user_id "
                sql = sql & "where FORMAT(CONVERT(date, apply_strike_executive_term.d_to, 111), 'yyyyMMdd') >= '" & systemDate & "'"
                dbAccess.Connect()
                dt = dbAccess.ExecuteSql(sql)

                If dt.Rows.Count > 0 Then
                    'flxExecutive.DataSource = dt
                    flxExecutive.Rows.Count = dt.Rows.Count + 1
                    For iCounter = 0 To dt.Rows.Count - 1
                        If Not IsDBNull(dt.Rows(iCounter)("d_from")) Then
                            flxExecutive.SetData(iCounter + 1, 0, dt.Rows(iCounter)("d_from"))
                        Else
                            flxExecutive.SetData(iCounter + 1, 0, "")
                        End If
                        If Not IsDBNull(dt.Rows(iCounter)("d_to")) Then
                            flxExecutive.SetData(iCounter + 1, 1, dt.Rows(iCounter)("d_to"))
                        Else
                            flxExecutive.SetData(iCounter + 1, 1, "")
                        End If
                        If Not IsDBNull(dt.Rows(iCounter)("l_biko_1")) Then
                            flxExecutive.SetData(iCounter + 1, 2, dt.Rows(iCounter)("l_biko_1"))
                        Else
                            flxExecutive.SetData(iCounter + 1, 2, "")
                        End If
                        If Not IsDBNull(dt.Rows(iCounter)("l_name")) Then
                            flxExecutive.SetData(iCounter + 1, 3, dt.Rows(iCounter)("l_name"))
                        Else
                            flxExecutive.SetData(iCounter + 1, 3, "")
                        End If
                        If Not IsDBNull(dt.Rows(iCounter)("c_apply_strike_term_id")) Then
                            flxExecutive.SetData(iCounter + 1, 4, dt.Rows(iCounter)("c_apply_strike_term_id"))
                        Else
                            flxExecutive.SetData(iCounter + 1, 4, "")
                        End If
                    Next
                Else
                    flxExecutive.Rows.Count = 1

                    'ボタン非活性化
                    Me.btnExeComUpdate.Enabled = False
                    Me.btnDelete.Enabled = False
                    Me.btnOkHeto.Enabled = False
                End If

            ElseIf cmbApplyClassify.SelectedValue = "03" Then
                '組合大会
                flxExecutive.Visible = False
                flxUnionInformation.Visible = True
                grpExecutive.Text = "組合大会より該当する会議を選択"
                sql = "select 会議番号,支部,種別,会議日付１,会議場所１,会議日付２, 会議場所２ "
                sql = sql & "from ((select c_union_meeting as 会議番号,k_apply_area,k_information_type,d_meeting_1 as 会議日付１,l_place_1 as 会議場所１,d_meeting_2 as 会議日付２,l_place_2 as 会議場所２ "
                sql = sql & "from union_information where c_ksh='" + MDLoginInfo.Ksh + "' and FORMAT(d_meeting_1, 'yyyyMMdd') >= '" & systemDate & "') as meeting "
                sql = sql & "left join (select l_name as 支部,c_constant_seq from constant_dtl where c_constant='APPLY_AREA') as SHIBU on meeting.k_apply_area=SHIBU.c_constant_seq) "
                sql = sql & "left join (select l_name as 種別,c_constant_seq from constant_dtl where c_constant='UI_CIR_KIND') as kind on meeting.k_information_type=kind.c_constant_seq"

                dbAccess.Connect()
                dt = dbAccess.ExecuteSql(sql)

                If dt.Rows.Count > 0 Then
                    flxUnionInformation.DataSource = dt
                    Dim num As Integer
                    For num = 0 To 6
                        Me.flxUnionInformation.Cols.Item(num).TextAlignFixed = C1.Win.C1FlexGrid.TextAlignEnum.CenterCenter
                        Select Case num

                            Case 0, 2
                                Me.flxUnionInformation.Cols.Item(num).TextAlign = C1.Win.C1FlexGrid.TextAlignEnum.LeftCenter
                                Exit Select
                            Case Else
                                Exit Select
                        End Select
                    Next
                Else
                    flxUnionInformation.Rows.Count = 1

                    'ボタン非活性化
                    btnOkHeto.Enabled = False
                End If
                flxUnionInformation.Cols.Item(0).Width = 80
                flxUnionInformation.Cols.Item(1).Width = 70
                flxUnionInformation.Cols.Item(2).Width = 70
                flxUnionInformation.Cols.Item(3).Width = 90
                flxUnionInformation.Cols.Item(4).Width = 130
                flxUnionInformation.Cols.Item(5).Width = 90
                flxUnionInformation.Cols.Item(6).Width = 130
                flxUnionInformation.AllowSorting = C1.Win.C1FlexGrid.AllowSortingEnum.None
                flxUnionInformation.AllowDragging = C1.Win.C1FlexGrid.AllowDraggingEnum.None
                flxUnionInformation.AllowEditing = False
                flxUnionInformation.AutoResize = False
                flxUnionInformation.SelectionMode = C1.Win.C1FlexGrid.SelectionModeEnum.Row
                flxUnionInformation.FocusRect = C1.Win.C1FlexGrid.FocusRectEnum.None
            Else
                '
                Call btnOkHeto_Click(Nothing, Nothing)
                Exit Sub
            End If

            Me.Height = 576
            grpExecutive.Visible = True
            btnOkHeto.Visible = True
            btnOk.Visible = False
            btnCancel.Location = New Point(313, 500)
            'If flxExecutive.Rows.Count < 2 And flxUnionInformation.Rows.Count < 2 Then
            '    btnOkHeto.Enabled = False
            'End If
            ' 支部・種類のコンボボックスロック
            Call Utilities.SetCanEditToControl(False, Me.cmbApplyArea)
            Call Utilities.SetCanEditToControl(False, Me.cmbApplyClassify)

            ' 処理終了ログ
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            dbAccess.Disconnect()
        End Try


    End Sub
#End Region

#Region "中執・組合大会選択後ＯＫボタン"
    Private Sub btnOkHeto_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOkHeto.Click
        Dim pn As Panel
        Dim uc As Control
        Dim dbAccess As New CLAccessMdb
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")

        Try
            If cmbApplyClassify.SelectedValue = "01" Or cmbApplyClassify.SelectedValue = "02" Then
                If flxExecutive.Rows.Count < 2 Then
                    Exit Sub
                End If
            ElseIf cmbApplyClassify.SelectedValue = "03" Then
                If flxUnionInformation.Rows.Count < 2 Then
                    Exit Sub
                End If
                If IsDBNull(flxUnionInformation.GetData(flxUnionInformation.Row, 0)) Then
                    Exit Sub
                ElseIf flxUnionInformation.GetData(flxUnionInformation.Row, 0) = "" Then
                    Exit Sub
                End If
            Else

            End If
            'Me.Visible = False
            pn = agoUserControl.ParentForm.Controls(MAIN_PANEL_ID)
            pn.Controls(SCREEN_ID_UC040301).Visible = False
            'uc = New UC040302
            uc = pn.Controls(SCREEN_ID_UC040302)
            If uc Is Nothing Then
                uc = New UC040302
                '支部
                Dim txtApplyArea As TextBox
                txtApplyArea = uc.Controls("txtApplyArea")
                txtApplyArea.Text = cmbApplyArea.Text
                txtApplyArea.ReadOnly = True
                txtApplyArea.BackColor = Color.LightYellow
                uc.Controls("lblApplyArea").Text = cmbApplyArea.SelectedValue
                '種類
                Dim txtApplyClassify As TextBox
                txtApplyClassify = uc.Controls("txtApplyClassify")
                txtApplyClassify.Text = cmbApplyClassify.Text
                txtApplyClassify.ReadOnly = True
                txtApplyClassify.BackColor = Color.LightYellow
                uc.Controls("lblApplyClassify").Text = cmbApplyClassify.SelectedValue
                uc.Controls("lblOmission").Text = lblKind2.Text
                '期間
                If cmbApplyClassify.SelectedValue = "01" Or cmbApplyClassify.SelectedValue = "02" Then
                    uc.Controls("lblKikan").Visible = True
                    uc.Controls("txtComExe").Visible = True
                    uc.Controls("grpFrameCount").Visible = True
                    uc.Controls("lblMeetingNo").Visible = False
                    uc.Controls("txtMeetingNo").Visible = False
                    Dim txtComExe As TextBox
                    txtComExe = uc.Controls("txtComExe")
                    txtComExe.Text = flxExecutive.GetData(flxExecutive.RowSel, 0) + " - " + flxExecutive.GetData(flxExecutive.RowSel, 1)
                    txtComExe.ReadOnly = True
                    txtComExe.BackColor = Color.LightYellow
                    uc.Controls("lblTermID").Text = flxExecutive.GetData(flxExecutive.RowSel, 4)
                    '回数表示
                    setCounterByClassifyID(uc.Controls("grpFrameCount"), cmbApplyClassify.SelectedValue, flxExecutive.GetData(flxExecutive.RowSel, 0))

                ElseIf cmbApplyClassify.SelectedValue = "03" Then
                    uc.Controls("lblKikan").Visible = False
                    uc.Controls("txtComExe").Visible = False
                    uc.Controls("grpFrameCount").Visible = True
                    uc.Controls("lblMeetingNo").Visible = True
                    uc.Controls("txtMeetingNo").Visible = True
                    Dim txtMeetingNo As TextBox
                    txtMeetingNo = uc.Controls("txtMeetingNo")
                    txtMeetingNo.Text = flxUnionInformation.GetData(flxUnionInformation.RowSel, 0)
                    txtMeetingNo.ReadOnly = True
                    txtMeetingNo.BackColor = Color.LightYellow
                    uc.Controls("lblTermID").Text = ""
                    '回数表示
                    setCounterByClassifyID(uc.Controls("grpFrameCount"), cmbApplyClassify.SelectedValue, flxUnionInformation.GetData(flxUnionInformation.RowSel, 3))
                Else
                    uc.Controls("lblKikan").Visible = False
                    uc.Controls("txtComExe").Visible = False
                    uc.Controls("lblMeetingNo").Visible = False
                    uc.Controls("txtMeetingNo").Visible = False
                    Dim txtComExe As TextBox
                    txtComExe = uc.Controls("txtComExe")
                    txtComExe.Text = ""
                    Dim txtMeetingNo As TextBox
                    txtMeetingNo = uc.Controls("txtMeetingNo")
                    txtMeetingNo.Text = ""
                    uc.Controls("lblTermID").Text = ""
                    '回数表示エリアを非表示
                    uc.Controls("grpFrameCount").Visible = False
                End If

                '申請番号
                Dim txtApplyNumber As TextBox = uc.Controls("txtApplyNumber")
                txtApplyNumber.Text = "*****"
                txtApplyNumber.ReadOnly = True
                txtApplyNumber.BackColor = Color.LightYellow
                '申請日
                Dim txtApplyDate As TextBox = uc.Controls("txtApplyDate")
                txtApplyDate.Text = Format(Now, DATE_YYYYMMDD_FORMAT)
                txtApplyDate.ReadOnly = True
                txtApplyDate.BackColor = Color.LightYellow
                '申請者
                Dim txtStandName As TextBox = uc.Controls("txtStandName")
                Dim leadName As String = GetUnionLeaderName()
                If String.IsNullOrEmpty(leadName) Then
                    leadName = "中央執行委員会代表"
                Else
                    leadName = "組合長　　" + leadName
                End If
                txtStandName.Text = leadName
                'txtStandName.ReadOnly = True
                'txtStandName.BackColor = Color.White

                '印刷ボタン非表示
                uc.Controls("btnPrinting").Visible = False
                uc.Controls("btnModify").Visible = False
                uc.Controls("grpReplace").Controls("cmbReplaceNumber").Enabled = False
                uc.Controls("grpReplace").Controls("btnReplaceNumber").Enabled = False

                '新規のとき、lblStrikeIDに""をセット
                uc.Controls("lblStrikeID").Text = ""

                '会議名選択肢設定
                Dim cmbApplyMeetinglist As ComboBox = uc.Controls("cmbApplyMeetinglist")

                dbAccess.Connect()
                ' 会議名コンボボックス
                Call CreateCboConstantDtlDate(dbAccess, cmbApplyMeetinglist, CONSTANT_ID_APPLY_MEETINGLIST, Now(), True)
                cmbApplyMeetinglist.SelectedIndex = -1
                cmbApplyMeetinglist.DropDownStyle = 1

                '差替え申請番号を取得
                Dim cmbReplaceNumber As ComboBox = uc.Controls("grpReplace").Controls("cmbReplaceNumber")
                Dim dtStrikeID As DataTable = GetReplaceStrikeId(cmbApplyArea.SelectedValue, cmbApplyClassify.SelectedValue)
                If dtStrikeID.Rows.Count > 0 Then
                    Dim dtBlank As DataRow = dtStrikeID.NewRow()
                    dtBlank(0) = ""
                    dtBlank(1) = ""
                    dtStrikeID.Rows.InsertAt(dtBlank, 0)
                    For iID As Integer = 0 To dtStrikeID.Rows.Count - 1
                        cmbReplaceNumber.Items.Add(dtStrikeID.Rows(iID)("c_strike_id"))
                    Next
                End If

                'FLEX
                Dim flx As C1.Win.C1FlexGrid.C1FlexGrid = uc.Controls("grpNameAndDate").Controls("flxNameAndDate")
                flx.Rows.Count = 2
                flx.AllowEditing = True
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If

            Me.Dispose()

        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040303, SCREEN_NAME_FM040303, "btnOkHeto_Click")
        Finally
            dbAccess.Disconnect()
            dbAccess = Nothing
        End Try

        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

#Region "新規登録"
    '新規登録
    Private Sub btnExeComInsert_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExeComInsert.Click
        Try
            ' 処理開始ログ
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_BEGIN)

            Me.Visible = False
            Dim cForm1 As New FM040304(Me)
            cForm1.Text = "中央委員会の日程　新規追加"
            cForm1.lblTitle.Text = "中央執行委員会・中央委員会の日程を追加してください。"
            cForm1.btnOk.Visible = True
            cForm1.btnUpdate.Visible = False
            cForm1.lblSHIBU.Text = cmbApplyArea.SelectedValue
            cForm1.lblKind.Text = cmbApplyClassify.SelectedValue
            ' Form1 をモーダルで表示する
            cForm1.ShowDialog()
            ' 不要になった時点で破棄する
            cForm1.Dispose()

            ' 処理終了ログ
            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub
#End Region

#Region "内容変更ボタン"
    '内容変更ボタン
    Private Sub btnExeComUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExeComUpdate.Click
        Dim strTermID As String                     '選択した行
        Dim dbAccess As New CLAccessMdb             '共通ＤＢアクセスクラス
        Dim sql As String                           'SQL文
        Dim dt As DataTable                         'データテーブル
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strTermID = CStr(flxExecutive.GetData(flxExecutive.RowSel, 4))
            If Not String.IsNullOrEmpty(strTermID) Then
                sql = "select d_from,d_to,l_biko_1 from apply_strike_executive_term where c_apply_strike_term_id=" + strTermID
                dbAccess.Connect()
                dt = dbAccess.ExecuteSql(sql)

                If dt.Rows.Count = 1 Then
                    Me.Visible = False
                    Dim cForm1 As New FM040304(Me)
                    cForm1.Text = "中央委員会の日程　更新"
                    cForm1.lblTitle.Text = "中央執行委員会・中央委員会の日程を更新してください。"
                    cForm1.btnOk.Visible = False
                    cForm1.btnUpdate.Visible = True
                    '開始日に値をセット
                    cForm1.dtpDateFrom.Value = Date.Parse(CStr(dt.Rows(0)(0)))
                    '終了日に値をセット
                    cForm1.dtpDateTo.Value = Date.Parse(CStr(dt.Rows(0)(1)))
                    '備考に値をセット
                    cForm1.txtBiko.Text = dt.Rows(0)(2)
                    cForm1.lblSHIBU.Text = cmbApplyArea.SelectedValue
                    cForm1.lblKind.Text = cmbApplyClassify.SelectedValue
                    cForm1.lblTermID.Text = strTermID

                    ' Form1 をモーダルで表示する
                    cForm1.ShowDialog()
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040303, SCREEN_NAME_FM040303, "btnExeComUpdate_Click")
        Finally
            dbAccess.Disconnect()
        End Try

        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)
    End Sub
#End Region

#Region "削除ボタン"
    '削除ボタン
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim strTermID As String                     '選択した行
        Dim dbAccess As New CLAccessMdb             '共通ＤＢアクセスクラス
        Dim sql As String                           'SQL文
        '処理開始ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strTermID = CStr(flxExecutive.GetData(flxExecutive.RowSel, 4))
            If Not String.IsNullOrEmpty(strTermID) Then

                ' 既に登録済み申請あればエラー
                If Not Me.CheckApplyStrikeExist(strTermID) Then
                    CLMsg.Show("GE0124", cmbApplyArea.Tag.ToString)
                    Exit Sub
                End If

                If CLMsg.Show("GQ0011") = DialogResult.Yes Then
                    sql = "delete from apply_strike_executive_term where c_apply_strike_term_id=" + strTermID
                    dbAccess.Connect()
                    dbAccess.BeginTran()
                    dbAccess.ExecuteNonQuery(sql)
                    dbAccess.CommitTran()

                    btnOk_Click(Nothing, Nothing)
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040303, SCREEN_NAME_FM040303, "btnDelete_Click")
        Finally
            dbAccess.Disconnect()
        End Try
        ' 処理終了ログ
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & STR_LOG_END)

    End Sub
#End Region

    Public Function GetLeaderName(ByVal strCommID As String) As String
        Dim strLeaderName As String = String.Empty
        Dim clsDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty
        Dim dtRet As DataTable = Nothing
        '現在日付をスラッシュを除いた形で取得
        Dim strDateNow As String = Now.ToString("yyyyMMdd")
        Try
            '最新の中執委員会名簿から委員長名を取得
            strSql = "" & vbCrLf
            strSql = strSql & " SELECT attr1.l_name " & vbCrLf
            strSql = strSql & "   FROM staf_attribute AS attr1, " & vbCrLf
            strSql = strSql & "        ( SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
            strSql = strSql & "            FROM staf_attribute " & vbCrLf
            strSql = strSql & "           WHERE d_from <= '" & strDateNow & "' " & vbCrLf '現在日以前の最新のユーザー情報
            strSql = strSql & "           GROUP BY c_user_id, c_ksh, c_staf_id ) AS attr2 " & vbCrLf
            strSql = strSql & "  WHERE attr1.c_user_id = ( SELECT com_list_dtl.c_user_id " & vbCrLf
            strSql = strSql & "                              FROM committee_list AS t7, " & vbCrLf
            strSql = strSql & "                                   committee_list_dtl AS com_list_dtl, " & vbCrLf
            strSql = strSql & "                                   ( SELECT c_committee_id,c_period_id, MAX(d_from) AS now_from " & vbCrLf
            strSql = strSql & "         	                          FROM committee_list " & vbCrLf
            strSql = strSql & "                                      WHERE d_from <= '" & strDateNow & "'  " & vbCrLf '最新の委員会名簿
            strSql = strSql & "         	                           AND c_committee_id = '" & strCommID & "' " & vbCrLf '委員会
            strSql = strSql & "                                      GROUP BY c_committee_id,c_period_id ) AS t8 " & vbCrLf
            strSql = strSql & "                             WHERE t7.c_committee_id = t8.c_committee_id " & vbCrLf
            strSql = strSql & "                               AND t7.d_from = t8.now_from " & vbCrLf
            strSql = strSql & "                               AND t7.c_committee_list = com_list_dtl.c_committee_list " & vbCrLf
            strSql = strSql & "                               AND com_list_dtl.c_committee_id = '" & strCommID & "' " & vbCrLf  '委員会
            strSql = strSql & "                               AND com_list_dtl.s_committee_seq = '1')  " & vbCrLf '委員長
            strSql = strSql & "    AND attr1.c_user_id = attr2.c_user_id  " & vbCrLf
            strSql = strSql & "    AND attr1.c_ksh = attr2.c_ksh " & vbCrLf
            strSql = strSql & "    AND attr1.d_from = attr2.now_from "
            strSql = strSql & ";"
            clsDb.Connect()                 ' DB接続開始
            dtRet = clsDb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                strLeaderName = dtRet.Rows(0).Item("l_name")
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040303, SCREEN_NAME_FM040303, System.Reflection.MethodBase.GetCurrentMethod.Name())
        Finally
            clsDb.Disconnect()              ' DB接続終了
        End Try
        Return strLeaderName
    End Function

    Private Sub lklMemo_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lklMemo.LinkClicked
        Try
            'カーソルを砂時計に変更
            Me.Cursor = Cursors.WaitCursor

            ' 「覚書を表示」の共通処理起動
            ShowOboegaki()
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

#Region "差替え申請番号一覧取得"
    Public Function GetReplaceStrikeId(ByVal strArea As String, ByVal strClassify As String) As DataTable
        Dim dbAccess As New CLAccessMdb
        Dim sql As String
        Dim table2 As New DataTable
        Try
            dbAccess.Connect()
            sql = "select c_period_id,d_from,l_omission_name from period where d_from<'" + MDLoginInfo.PeriodFrom + "' order by d_from DESC"
            Dim dt As DataTable = dbAccess.ExecuteSql(sql)
            Dim strPrePeriond As String
            If dt.Rows.Count > 0 Then
                strPrePeriond = dt.Rows(0)("l_omission_name")
                sql = "select A_APPLY.c_application, A_APPLY.c_strike_id  from apply_strike A_APPLY, "
                sql = sql + "(select distinct(APPLY.c_strike_id)  as c_strike_id, APPLY.k_apply_area as k_apply_area "
                sql = sql + " from apply_strike APPLY, apply_strike_member_date APP_DATE "
                sql = sql + " where APPLY.c_ksh         = '" + MDLoginInfo.Ksh + "' "
                sql = sql + "  and APPLY.k_apply_area  = '" + strArea + "' "
                sql = sql + "  and APPLY.k_cancel      = '0'    and APPLY.k_replace     = '0' "
                sql = sql + "  and Format(APP_DATE.d_strike, 'yyyyMMdd')  >= '" + MDLoginInfo.PeriodFrom + "' "
                sql = sql + "  and Format(APP_DATE.d_strike, 'yyyyMMdd')  <= '" + MDLoginInfo.PeriodTo + "' "
                sql = sql + "  and Mid(APPLY.c_strike_id,1,2)  = '" + strPrePeriond + "' "
                sql = sql + "  and (   APP_DATE.c_cancel_strike_id is null         OR APP_DATE.c_cancel_strike_id = '')  "
                sql = sql + "  and APPLY.c_strike_id = APP_DATE.c_strike_id and APPLY.k_apply_area = APP_DATE.k_apply_area ) B_APPLY "
                sql = sql + "where A_APPLY.c_strike_id = B_APPLY.c_strike_id "
                sql = sql + " and A_APPLY.k_apply_area = B_APPLY.k_apply_area "
                sql = sql + " and A_APPLY.k_apply_area = '" + strArea + "' "
                sql = sql + " and A_APPLY.k_apply_classify = '" + strClassify + "' "
                sql = sql + " order by A_APPLY.c_application"
                table2 = dbAccess.ExecuteSql(sql)
            Else
                Return table2
            End If
            
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040302, SCREEN_NAME_UC040302, "GetReplaceStrikeId")
        Finally
            dbAccess.Disconnect()
            dbAccess = Nothing
        End Try
        Return table2
    End Function
#End Region

#Region "CheckApplyStrikeExist"
    '***************************************************************************************************
    '   ＩＤ　：CheckApplyStrikeExist
    '   名称　：中央執行時間内組合活動 存在チェック
    '   概要  ：指定の中央執行委員会活動IDに対する時間内組合活動の存在をチェック
    '   引数　：strTermID … 中央執行委員会活動ID
    '   戻り値：True = 申請なし, False = 申請あり
    '   作成日：2012/01/25(水)
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/25(水) Fujisaku  新規作成
    '         ：2012/02/06(月) Fujisaku  変更 申請地区対応
    '***************************************************************************************************
    Private Function CheckApplyStrikeExist(ByVal strTermID As String) As Boolean
        Dim blnFlg As Boolean = False
        Dim dbAccess As New CLAccessMdb
        Dim sql As String
        Dim tbRet As DataTable = Nothing
        Try
            sql = "SELECT count(APP.c_strike_id) FROM apply_strike_member_date DAT, apply_strike APP "
            sql = sql & "WHERE DAT.c_strike_id = APP.c_strike_id AND  APP.c_ksh = '" & MDLoginInfo.Ksh & "'"
            sql = sql & " AND  DAT.k_apply_area = APP.k_apply_area"
            sql = sql & " AND  APP.k_cancel = '0' AND  APP.k_replace= '0' AND  APP.c_strike_id <> ''"
            sql = sql & " AND (DAT.c_cancel_strike_id is null OR DAT.c_cancel_strike_id = '' )"
            sql = sql & " AND APP.union_info_c_union_meeting = '" & strTermID & "'"

            dbAccess.Connect()
            tbRet = dbAccess.ExecuteSql(sql)

            If tbRet.Rows.Count > 0 Then
                If CInt(tbRet.Rows(0).Item(0)) = 0 Then
                    '件数取得成功し、件数0件の場合True
                    blnFlg = True
                End If
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_FM040303, SCREEN_NAME_FM040303, "CheckApplyStrikeExist")
        Finally
            dbAccess.Disconnect()
        End Try

        Return blnFlg
    End Function
#End Region

#Region "flxUnionInformation_DoubleClick"
    ' 組合大会グリッドダブルクリック選択
    Private Sub flxUnionInformation_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flxUnionInformation.DoubleClick
        Try
            If Me.flxUnionInformation.HitTest.Type.Equals(HitTestTypeEnum.Cell) Then
                Me.btnOkHeto_Click(sender, e)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "flxExecutive_DoubleClick"
    ' 中央執行グリッドダブルクリック選択
    Private Sub flxExecutive_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles flxExecutive.DoubleClick
        Try
            If Me.flxUnionInformation.HitTest.Type.Equals(HitTestTypeEnum.Cell) Then
                Me.btnOkHeto_Click(sender, e)
            End If
        Catch ex As Exception
            log.Fatal(ex.Message)                                                       ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

End Class