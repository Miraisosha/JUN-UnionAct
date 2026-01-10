#Region "UC040206　クラス"

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk
Public Class UC040206
    Private Const STRIKE_KANJIMEI_SHINSEISYA As String = "申請者"                       '漢字名称-申請者
    Private Const STRIKE_KANJIMEI_KENMEI As String = "件名"                             '漢字名称-件名
    Private Const STRIKE_KANJIMEI_KUMIAIIN As String = "組合員"                         '漢字名称-組合員
    Private Const STRIKE_SOUGIID_INI As String = "001"                                  'その日の一番目の争議ID
    Private Const STRIKE_KANJIMEI_SOUGIHIDUKE As String = "争議日付"                    '漢字名称-争議日付
#Region "変数宣言"
    Private Const SCREEN_ID As String = SCREEN_ID_UC040205
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040205                          ' 争議行為通告細部画面
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
    Private STRIKE_KIND As String = "06"                                '争議行為終結
    Private STRIKE_INFO As String                                       'Sequence
    Private TEXT_NO_KIND As String = "Ｂ（ＡＮＡ宛）"
    Private Const SQL_COLNAME_LIST As String = "c_strike_id, c_ksh, c_period_id, k_strike_info, c_strike_info, d_strike, l_stand_name, k_strike_kind, d_strike_start, c_basis_strike_id, l_event, l_term, l_subject, l_text, l_another_subject, l_biko, d_ins, c_user_id_ins, d_up, c_user_id_up, s_up"
    Private Const SQL_COLNAME_WORK As String = "c_strike_work_id, c_ksh, c_period_id, k_strike_info, d_strike, l_stand_name, k_strike_kind, d_strike_start, c_basis_strike_id, l_event, l_term, l_subject, l_text, l_another_subject, l_biko, d_ins, c_user_id_ins, d_up, c_user_id_up, s_up"
#End Region
#Region "プロパティ"
    Public _bytStatus As Byte = 0                                       ' ステータス（0：新規登録, 1:詳細 2：内容変更）
    Public _bytTabKind As Byte = 0                                      ' Tab Kind （1：本登録, 2：一時保管）
    Public _strStrikeId As String = ""                                  ' strikeID
    Public _strKind As String = ""                                      ' strike Kind 
    Public _strBasisStrikeID As String = ""                             ' Basis strikeID
    Public _strSyuketuKind As String = ""                               ' shuketu Kind
    Public _strPreScreenId As String = ""                               ' 呼び元画面ID
    ' ステータス
    Public Property bytStatus() As Byte
        Get
            Return _bytStatus
        End Get
        Set(ByVal value As Byte)
            _bytStatus = value
        End Set
    End Property
    ' Tab Kind
    Public Property bytTabKind() As Byte
        Get
            Return _bytTabKind
        End Get
        Set(ByVal value As Byte)
            _bytTabKind = value
        End Set
    End Property
    ' strikeID
    Public Property strStrikeId() As String
        Get
            Return _strStrikeId
        End Get
        Set(ByVal value As String)
            _strStrikeId = value
        End Set
    End Property
    ' strike Kind
    Public Property strKind() As String
        Get
            Return _strKind
        End Get
        Set(ByVal value As String)
            _strKind = value
        End Set
    End Property
    ' Basis strikeID
    Public Property strBasisStrikeID() As String
        Get
            Return _strBasisStrikeID
        End Get
        Set(ByVal value As String)
            _strBasisStrikeID = value
        End Set
    End Property
    ' Syuketu Kind
    Public Property strSyuketuKind() As String
        Get
            Return _strSyuketuKind
        End Get
        Set(ByVal value As String)
            _strSyuketuKind = value
        End Set
    End Property
    ' 呼び元画面ID
    Public Property strPreScreenId() As String
        Get
            Return _strPreScreenId
        End Get
        Set(ByVal value As String)
            _strPreScreenId = value
        End Set
    End Property
#End Region


#Region "イベント"
    Private Sub UC040206_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '-------------------------------------------------------------------------------
        '   各Screen処理
        '-------------------------------------------------------------------------------
        If controlScreen() = False Then
            Exit Sub
        End If
        '-------------------------------------------------------------------------------
        '   各データ取得処理
        '-------------------------------------------------------------------------------
        If GetData() = False Then
            Exit Sub
        End If
    End Sub
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Dim pn As Panel                                                     'メインパネル
        Dim uc As Control                                                   '遷移先画面コントロール
        Try
            Me.Visible = False
            pn = ParentForm.Controls(MAIN_PANEL_ID)
            uc = pn.Controls(SCREEN_ID_UC040201)

            If uc Is Nothing Then
                uc = New UC040201
                Call pn.Controls.Add(uc)
            Else
                uc.Visible = True
            End If
            Me.Dispose()
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim pn As Panel                                                     'メインパネル
        Dim uc As Control                                                   '遷移先画面コントロール
        Dim clsUC040201 As UC040201

        If CLMsg.Show("GQ0007") = DialogResult.No Then                                      ' 入力・変更内容破棄メッセージボックス表示
            Exit Sub                                                                        ' 「いいえ」ボタン押下時、処理を抜ける
        Else
            If Me.bytStatus = STATUS_INSERT Then
                '-------------------------------------------------------------------------------
                '   各Screen処理
                '-------------------------------------------------------------------------------
                Me.Visible = False
                pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
                uc = pn.Controls(SCREEN_ID_UC040201)
                If uc Is Nothing Then
                    uc = New UC040201
                    Call pn.Controls.Add(uc)
                Else
                    clsUC040201 = pn.Controls(SCREEN_ID_UC040201)
                    uc.Visible = True
                End If
                Me.Dispose()
            ElseIf Me.bytStatus = STATUS_EDIT Then
                Me.bytStatus = STATUS_DETAIL
                Call UC040206_Load(sender, e)
                Me.Visible = True
            ElseIf (Me.bytStatus = STATUS_TRANSIT) Then
                Me.bytTabKind = STATUS_TAB_TEMP
                Me.bytStatus = STATUS_DETAIL
                Call UC040206_Load(sender, e)
                Me.Visible = True
            End If
        End If
    End Sub
    Private Sub btnSampleText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSampleText.Click
        Dim strSamHonbun As String
        Dim strYYYY, strYYYY1 As String
        Dim strMM, strMM1 As String
        Dim strDD, strDD1 As String
        Dim strKumi As String
        Dim dtpDateTime As DateTimePicker
        Dim dtpDateOneday As Date
        Try
            dtpDateTime = Me.dtpSougiDate
            strYYYY = Mid(Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT), 1, 4)
            strMM = Mid(Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT), 5, 2)
            strDD = Mid(Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT), 7, 2)

            dtpDateOneday = DateAdd(DateInterval.Day, +1, dtpSougiDate.Value.Date)
            strYYYY1 = Mid(Format(dtpDateOneday, DATE_YYYYMMDD_8_FORMAT), 1, 4)
            strMM1 = Mid(Format(dtpDateOneday, DATE_YYYYMMDD_8_FORMAT), 5, 2)
            strDD1 = Mid(Format(dtpDateOneday, DATE_YYYYMMDD_8_FORMAT), 7, 2)
            strKumi = Mid(txtNoKind.Text, 1, 1)
            strSamHonbun = ""
            strSamHonbun = strSamHonbun + "主題の件、先般通知した全日空乗組 " + strKumi + " 第 " + strStrikeId + " 号に関し、" + vbCrLf
            strSamHonbun = strSamHonbun + strYYYY + "年" + strMM + "月" + strDD + "日24時をもって、当該争議行為を終結する。" + vbCrLf

            If (Not ChkNull(txtHonbun.Text)) Then
                If CLMsg.Show("GQ0023", "本文") = DialogResult.Yes Then
                    txtHonbun.Text = strSamHonbun
                End If
            Else
                txtHonbun.Text = strSamHonbun
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
    Private Sub btnSaveTmp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveTmp.Click
        Try
            Me.bytTabKind = STATUS_TAB_TEMP
            ' 一時保存登録処理
            If Not PrintPreview() Then
                Exit Sub
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
            '-------------------------------------------------------------------------------
            '   各Screen処理
            '-------------------------------------------------------------------------------
            Me.bytStatus = STATUS_EDIT
            If controlScreen() = False Then
                Exit Sub
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
    End Sub
    ' 印刷処理'
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Me.bytStatus = STATUS_DETAIL
            ' 印刷プレビュー処理
            If PrintPreview() Then
                Exit Sub
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        End Try
    End Sub
    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        Try
            If (Me.bytTabKind = STATUS_TAB_TEMP) Then
                Me.bytStatus = STATUS_TRANSIT
            Else
                Me.bytTabKind = STATUS_TAB_HONB
            End If
            If Not dataCheck() Then
                Exit Sub
            End If
            If Not PrintPreview() Then
                Exit Sub
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
    End Sub
#End Region

#Region "関数"
    Private Function controlScreen() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Try
            Me.txtNoKind.Visible = True
            Me.txtNoKind.ReadOnly = True
            Me.txtNoKind.BackColor = Color.LightYellow
            Me.txtNo.Visible = True
            Me.txtNo.ReadOnly = True
            Me.txtNo.BackColor = Color.LightYellow
            Me.txtNoKanren.Visible = True
            Me.txtNoKanren.ReadOnly = True
            Me.txtNoKanren.BackColor = Color.LightYellow
            Me.lblShinseiDate.Visible = True
            Me.txtShinseiDate.Visible = True
            Me.txtShinseiDate.ReadOnly = True
            Me.txtShinseiDate.BackColor = Color.LightYellow
            Me.lblTitle.Text = "争議行為終結通告－詳細"
            Me.txtUser.BackColor = Color.White
            Select Case Me.bytStatus
                Case STATUS_DETAIL
                    Me.btnSampleText.Visible = True
                    Me.btnSampleText.Enabled = False
                    Me.dtpSougiDate.Visible = False
                    Me.btnCancel.Visible = False
                    Me.btnBack.Visible = True
                    Me.btnChange.Visible = False
                    Me.btnUpdate.Visible = True
                    Me.btnSaveTmp.Visible = False
                    Me.btnPrint.Visible = True
                    Me.txtUser.ReadOnly = True
                    Me.txtSougiDate.ReadOnly = True
                    Me.txtHonbun.ReadOnly = True
                    Me.txtSougiDate.Visible = True
                    Me.txtSougiDate.BackColor = Color.White
                    Me.txtHonbun.BackColor = Color.White
                    Select Case Me.bytTabKind
                        Case STATUS_TAB_HONB
                            Me.lblIchiji.Visible = False
                            Me.fraDetail.BackColor = SystemColors.Control
                        Case STATUS_TAB_TEMP
                            Me.lblIchiji.Visible = True
                            Me.fraDetail.BackColor = Color.PapayaWhip
                        Case Else
                            Call CLMsg.Show("GE0004", "Source controlScreen")
                            Exit Function
                    End Select
                Case STATUS_EDIT, STATUS_TRANSIT
                    Me.btnSampleText.Visible = True
                    Me.btnSampleText.Enabled = True
                    Me.dtpSougiDate.Visible = True
                    Me.txtSougiDate.Visible = False
                    Me.btnCancel.Visible = True
                    Me.btnBack.Visible = False
                    Me.btnChange.Visible = True
                    Me.btnUpdate.Visible = False
                    Me.btnPrint.Visible = False
                    Me.lblShinseiDate.Visible = True
                    Me.txtUser.ReadOnly = False
                    Me.txtSougiDate.ReadOnly = True
                    Me.txtHonbun.ReadOnly = False
                    Me.txtSougiDate.BackColor = Color.White
                    Me.txtHonbun.BackColor = Color.White
                    Select Case Me.bytTabKind
                        Case STATUS_TAB_HONB
                            Me.lblIchiji.Visible = False
                            Me.btnSaveTmp.Visible = False
                            Me.fraDetail.BackColor = SystemColors.Control
                        Case STATUS_TAB_TEMP
                            Me.lblIchiji.Visible = True
                            Me.btnSaveTmp.Visible = True
                            Me.fraDetail.BackColor = Color.PapayaWhip
                        Case Else
                            Call CLMsg.Show("GE0004", "Source controlScreen")
                            Exit Function
                    End Select
                Case STATUS_INSERT
                    Me.btnSampleText.Visible = True
                    Me.btnSampleText.Enabled = True
                    Me.dtpSougiDate.Visible = True
                    Me.txtSougiDate.Visible = False
                    Me.btnCancel.Visible = True
                    Me.btnBack.Visible = False
                    Me.btnChange.Visible = True
                    Me.btnUpdate.Visible = False
                    Me.btnSaveTmp.Visible = True
                    Me.btnPrint.Visible = False
                    Me.lblShinseiDate.Visible = False
                    Me.txtUser.ReadOnly = False
                    Me.txtSougiDate.ReadOnly = True
                    Me.txtHonbun.ReadOnly = False
                    Me.txtSougiDate.BackColor = Color.White
                    Me.txtHonbun.BackColor = Color.White
                    Me.lblShinseiDate.Visible = False
                    Me.txtShinseiDate.Visible = False
                    Select Case Me.bytTabKind
                        Case STATUS_TAB_HONB
                            Me.lblIchiji.Visible = False
                            Me.fraDetail.BackColor = SystemColors.Control
                        Case STATUS_TAB_TEMP
                            Me.lblIchiji.Visible = True
                            Me.fraDetail.BackColor = Color.PapayaWhip
                        Case Else
                            Call CLMsg.Show("GE0004", "Source controlScreen")
                            Exit Function
                    End Select
                Case Else
                    Call CLMsg.Show("GE0004", "Source controlScreen")
                    Exit Function
            End Select
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            Return False
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function
    Private Function GetData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strsql As String
        Try
            Select Case Me.bytStatus
                Case STATUS_INSERT
                    Me.txtNoKind.Text = TEXT_NO_KIND
                    Me.txtNo.Text = "*****"
                    Me.dtpSougiDate.Value = CDate(Format(Now, DATE_YYYYMMDD_FORMAT))
                    Me.txtUser.Text = "組合長　" & GetUnionLeaderName()
                    Me.txtNoKanren.Text = strStrikeId
                Case STATUS_DETAIL
                    ' データベース接続
                    Call clsDb.Connect()
                    '申請者 Check Null
                    If ChkNull(Me.strStrikeId) Then
                        CLMsg.Show("GE0001")
                        Return False
                    End If
                    If (Me.bytTabKind = STATUS_TAB_HONB) Then
                        '-------------------------------------------------------------------------------
                        '   SQL作成
                        '-------------------------------------------------------------------------------
                        strsql = ""
                        strsql = strsql & " select stli.k_strike_info,stli.c_strike_info,"
                        strsql = strsql & "        stli.d_strike,stli.l_stand_name,"
                        strsql = strsql & "        stli.d_strike_start,stli.l_event,stli.l_text,"
                        strsql = strsql & "        stli.l_term,stli.c_strike_id,stli.c_basis_strike_id "
                        strsql = strsql & " from   strike_list stli "
                        strsql = strsql & " where  stli.c_strike_id = '" & strStrikeId & "'"
                        strsql = strsql & ";"
                        ' SQL実行
                        dt = clsDb.ExecuteSql(strsql)
                        Me.txtNoKind.Text = NVL(dt.Rows(0).Item("k_strike_info"))
                        Me.txtNo.Text = NVL(dt.Rows(0).Item("c_strike_id"))
                        Me.txtNoKanren.Text = dt.Rows(0).Item("c_basis_strike_id")
                        STRIKE_INFO = NVL(dt.Rows(0).Item("c_strike_id"))
                        Me.txtUser.Text = dt.Rows(0).Item("l_stand_name")
                        Me.txtSougiDate.Text = dt.Rows(0).Item("d_strike_start").ToString.Substring(0, 4) & "年" & dt.Rows(0).Item("d_strike_start").ToString.Substring(4, 2) & "月" & dt.Rows(0).Item("d_strike_start").ToString.ToString.Substring(6, 2) & "日"
                        Me.dtpSougiDate.Value = Date.Parse(Format(CInt(dt.Rows(0).Item("d_strike_start").ToString), "0000/00/00"))
                        Me.txtShinseiDate.Text = dt.Rows(0).Item("d_strike").ToString.Substring(0, 4) & "/" & dt.Rows(0).Item("d_strike").ToString.Substring(4, 2) & "/" & dt.Rows(0).Item("d_strike").ToString.ToString.Substring(6, 2)
                        Me.txtHonbun.Text = NVL(dt.Rows(0).Item("l_text"))
                        If (dt.Rows(0).Item("d_strike_start").ToString < Format(Now, DATE_YYYYMMDD_8_FORMAT)) Then
                            Me.btnUpdate.Visible = False
                        End If
                    ElseIf (Me.bytTabKind = STATUS_TAB_TEMP) Then
                        '-------------------------------------------------------------------------------
                        '   SQL作成
                        '-------------------------------------------------------------------------------
                        strsql = ""
                        strsql = strsql & " select stli.k_strike_info,"
                        strsql = strsql & "        stli.d_strike,stli.l_stand_name,"
                        strsql = strsql & "        stli.d_strike_start,stli.l_text,"
                        strsql = strsql & "        stli.l_term,stli.c_strike_work_id,stli.c_basis_strike_id "
                        strsql = strsql & " from   strike_work_list stli"
                        strsql = strsql & " where  stli.c_strike_work_id = '" & strStrikeId & "';"
                        ' SQL実行
                        dt = clsDb.ExecuteSql(strsql)
                        Me.txtNoKind.Text = NVL(dt.Rows(0).Item("k_strike_info"))
                        STRIKE_INFO = "*****"
                        Me.txtNo.Text = STRIKE_INFO
                        Me.txtNoKanren.Text = dt.Rows(0).Item("c_basis_strike_id")
                        Me.txtUser.Text = dt.Rows(0).Item("l_stand_name")
                        Me.txtSougiDate.Text = dt.Rows(0).Item("d_strike_start").ToString.Substring(0, 4) & "年" & dt.Rows(0).Item("d_strike_start").ToString.Substring(4, 2) & "月" & dt.Rows(0).Item("d_strike_start").ToString.ToString.Substring(6, 2) & "日"
                        Me.dtpSougiDate.Value = Date.Parse(Format(CInt(dt.Rows(0).Item("d_strike_start").ToString), "0000/00/00"))
                        Me.txtShinseiDate.Text = dt.Rows(0).Item("d_strike").ToString.Substring(0, 4) & "/" & dt.Rows(0).Item("d_strike").ToString.Substring(4, 2) & "/" & dt.Rows(0).Item("d_strike").ToString.ToString.Substring(6, 2)
                        Me.txtHonbun.Text = NVL(dt.Rows(0).Item("l_text"))
                        If (dt.Rows(0).Item("d_strike_start").ToString < Format(Now, DATE_YYYYMMDD_8_FORMAT)) Then
                            Me.btnUpdate.Visible = False
                        End If
                    End If
                    Call clsDb.Disconnect()
                Case Else
                    Call CLMsg.Show("GE0004", "Source GetData")
                    Exit Function
            End Select
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function
    '***************************************************************************************************
    '   ＩＤ　：PrintPreview
    '   名称　：印刷プレビュー処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/12(火) kim  新規作成
    '***************************************************************************************************
    Private Function PrintPreview() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim ds As DS0402P5 = Nothing                                                        ' 帳票用データセット
        Dim fmPrint As FM000203 = Nothing                                                   ' プレビュークラス
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument = Nothing    ' レポートドキュメントオブジェクト
        Dim intRet As Integer = Nothing                                                     ' プレビュー画面処理結果
        Dim drHeader As DS0402P5.dtHeaderRow = Nothing
        Dim strToday As String                                                              '当日の日付
        Dim intCnt As Integer = Nothing                                                     ' SQL実行結果件数
        Dim printCount As Integer
        Dim strNo As String = ""
        Dim strKsh As String = ""
        Dim strPeriodId As String = ""
        Dim pn As Panel                                                                     'メインパネル
        Dim uc As New UC040201                                                              '遷移先画面コントロール
        Dim strStrikeInfo As String()
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            ' カーソル砂時計
            Cursor.Current = Cursors.WaitCursor
            strToday = Format(Now, DATE_YYYYMMDD_8_FORMAT)

            ' データセットクラス生成
            ds = New DS0402P5
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.l_president_name = GetPresidentName()
            drHeader.l_stand_name = Me.txtUser.Text
            drHeader.l_term = Me.txtHonbun.Text
            drHeader.application_date = Now
            drHeader.k_strike_info = Mid(Me.txtNoKind.Text, 1, 1)
            drHeader.c_period_id = MDLoginInfo.Period
            If Me.txtNo.Text = "*****" Then
                STRIKE_INFO = "***"
            Else
                strStrikeInfo = Me.txtNo.Text.Split("-")
                STRIKE_INFO = strStrikeInfo(1)
            End If
            drHeader.c_strike_info = STRIKE_INFO
            drHeader.EndEdit()
            ds.dtHeader.Rows.Add(drHeader)
            ' クラス生成
            fmPrint = New FM000203                                                          ' 印刷プレビュー画面
            '2015/07/03 前田修正 CR0402P3→CR0402P5 
            reportObj = New CR0402P5                                                        ' レポートドキュメント生成

            If (Me.bytStatus = STATUS_DETAIL) Then
                ' プロパティ設定
                fmPrint.ButtonShowType = 3                                                  ' ボタン形式設定（印刷、キャンセル）
            Else
                ' プロパティ設定
                fmPrint.ButtonShowType = 1                                                  ' ボタン形式設定（登録＆印刷　、登録のみ、キャンセル）
            End If

            fmPrint.PrintCntVisible = True                                                  ' 印刷部数項目表示可否
            fmPrint.ObjResource = reportObj                                                 ' レポート形式設定
            reportObj.SetDataSource(ds)                                                     ' データセット設定

            ' 印刷プレビュー画面表示
            Call fmPrint.ShowDialog()

            ' 印刷プレビュー画面処理結果取得
            intRet = fmPrint.IntQlickBtnFlag

            ' 印刷プレビュー画面処理結果処理判定
            If intRet = 0 Then                                                              '0=>登録＆印刷 1=>登録のみ 2=>キャンセル 3=>印刷
                'Insert作業
                If Not updateData() Then
                    Exit Function
                End If
                ds.dtHeader.Rows.Item(0).Item("c_strike_info") = STRIKE_INFO              ' 取得した争議番号 + 1 を帳票に表示
                reportObj.SetDataSource(ds)
                'Print作業
                printCount = fmPrint.nudPrintCount.Value
                fmPrint.PrintOut(printCount)
            ElseIf intRet = 1 Then
                'Insert作業
                If Not updateData() Then
                    Exit Function
                End If
            ElseIf intRet = 2 Then
                If (Me.bytStatus = STATUS_INSERT) Then
                    Me.bytTabKind = STATUS_TAB_HONB
                ElseIf (Me.bytStatus = STATUS_TRANSIT) Then
                    Me.bytTabKind = STATUS_TAB_TEMP
                    Me.bytStatus = STATUS_EDIT
                End If
            ElseIf intRet = 3 Then
                printCount = fmPrint.nudPrintCount.Value
                fmPrint.PrintOut(printCount)
            End If

            Select Case intRet
                Case 0, 1
                    '争議行為文書一覧画面へ遷移
                    Me.Visible = False
                    pn = ParentForm.Controls(MAIN_PANEL_ID)
                    uc = pn.Controls(SCREEN_ID_UC040201)
                    If uc Is Nothing Then
                        Call pn.Controls.Add(uc)
                    Else
                        uc.blnSearchFlg = True
                        uc.Visible = True
                    End If
            End Select
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
            Cursor.Current = Cursors.Default                                            ' カーソル初期
            fmPrint.Dispose()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")     ' ログ出力（処理終了）
        Return blnRet                                                                   ' 戻り値設定
    End Function
    '***************************************************************************************************
    '   ＩＤ　：updateData
    '   名称　：登録処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/17(火) kim  新規作成
    '***************************************************************************************************
    Private Function updateData() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Try
            Select Case Me.bytStatus
                Case STATUS_INSERT
                    If Not TableInsert() Then
                        Exit Function
                    End If
                Case STATUS_EDIT
                    If strStrikeId = "" Then
                        CLMsg.Show("FE0001")
                        Return blnRet
                    Else
                        TableUpdate()
                    End If
                Case STATUS_TRANSIT
                    If strStrikeId = "" Then
                        CLMsg.Show("FE0001")
                        Return blnRet
                    Else
                        If Not TableTrans() Then
                            Exit Function
                        End If
                    End If
                Case Else
                    Call CLMsg.Show("GE0004", "Source updateData")
                    Exit Function
            End Select
            blnRet = True                                                               ' 処理結果に正常を設定
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            log.Fatal(ex.Message)                                                           ' ログ出力（致命的エラー）
        Finally
        End Try
        Return blnRet                                                                   ' 戻り値設定
    End Function
#Region "争議ID取得"
    '************************************************************************************
    '   ＩＤ　：getSougiID
    '   名称　：争議ID取得
    '   概要　：一時保存と本保存テーブルより、その日の最大争議IDを取得し、次のIDはプラス１とする
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '************************************************************************************
    Private Function getSougiID() As String
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim dtrow As DataRow                                                                '一行のデータ
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim strSeq As String
        Try
            Call clsDb.Connect()
            '本登録テーブルからID取得
            strSql = "select max(CLng(c_strike_info))+1 as c_strike_max "
            strSql = strSql + " from   strike_list "
            strSql = strSql + " where  c_period_id = '" + NSMDInfo.PeriodId + "'"
            strSql = strSql + " group  by c_period_id "
            strSql = strSql + " UNION "
            strSql = strSql + " select max(CLng(c_name_strike_info))+1 as c_strike_max "
            strSql = strSql + " from   name_strike "
            strSql = strSql + " where  c_period_id = '" + NSMDInfo.PeriodId + "'"
            strSql = strSql + " group  by c_period_id "
            strSql = strSql + " order by 1 desc; "

            dt = clsDb.ExecuteSql(strSql)
            If dt.Rows.Count > 0 Then
                dtrow = dt.Rows(0)
                strSeq = CLng(CStr(dtrow("c_strike_max")))
            Else
                strSeq = 1
            End If
            Return strSeq
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040202, SCREEN_ID_UC040202, "getSougiID")
            Return False
        Finally
            Call clsDb.Disconnect()
        End Try
    End Function
#End Region
#Region "争議WorkID取得"
    '************************************************************************************
    '   ＩＤ　：getWorkSougiID
    '   名称　：一時登録ID取得
    '   概要　：一時保存と本保存テーブルより、その日の最大争議IDを取得し、次のIDはプラス１とする
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '************************************************************************************
    Private Function getWorkSougiID() As String
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim strSeq As String
        Try
            Call clsDb.Connect()
            '一時登録テーブルから
            strSql = "select CInt(c_strike_work_id) as c_strike_work_max from strike_work_list order by 1 desc"
            dt = clsDb.ExecuteSql(strSql)
            If dt.Rows.Count > 0 Then
                'If dt.Rows(0).Item("c_strike_work_max") = "" Then
                strSeq = CLng(CStr(dt.Rows(0).Item("c_strike_work_max"))) + 1
            Else
                strSeq = 1
            End If
            Return strSeq
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040202, SCREEN_ID_UC040202, "getSougiID")
            Return False
        Finally
            Call clsDb.Disconnect()
        End Try
    End Function
#End Region
    '************************************************************************************
    '   ＩＤ　：dataCheck
    '   名称　：入力されたデータのチェック
    '   概要　：入力されたデータのチェック
    '   作成日：2012/01/17 kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/17 Kim  変更
    '************************************************************************************
    Private Function dataCheck() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim strArray() As String
        Dim charCnt As Integer
        Dim lineCnt As Integer
        Dim tLineCnt As Integer
        Dim tCharCnt As Integer
        Dim intMaxLineCnt As Integer
        Try
            '-------------------------------------------------------------------------------
            '   初期化
            '-------------------------------------------------------------------------------
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If
            If controlScreen() = False Then
                Exit Function
            End If
            '申請者 Check Null
            If ChkNull(txtUser.Text) Then
                CLMsg.Show("GE0006", "申請者")
                txtUser.BackColor = Color.LightPink
                Me.txtUser.Focus()
                Return False
            ElseIf Not ChkLength(txtUser.Text, 14) Then
                CLMsg.Show("GE0103")
                txtUser.BackColor = Color.LightPink
                Me.txtUser.Focus()
                Return False
            End If
            '本文 Check Null
            If ChkNull(txtHonbun.Text) Then
                CLMsg.Show("GE0006", "本文")
                txtHonbun.BackColor = Color.LightPink
                Me.txtHonbun.Focus()
                Return False
            End If
            '争議日付 Check(現在日より過去日を指定することはできません)
            If Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT) < Format(Now, DATE_YYYYMMDD_8_FORMAT) Then
                CLMsg.Show("GE0077", "争議日付")
                Me.dtpSougiDate.Focus()
                Return False
            Else
                dtpSougiDate.BackColor = Color.White
            End If
            '本文文字Max42*18文字チェック
            tLineCnt = 0
            charCnt = 0
            strArray = Split(txtHonbun.Text, vbCrLf)
            lineCnt = UBound(strArray)
            tLineCnt = tLineCnt + lineCnt + 1
            intMaxLineCnt = 18
            If (tLineCnt > intMaxLineCnt) Then
                CLMsg.Show("GE0103", "本文")
                txtHonbun.BackColor = Color.LightPink
                Me.txtHonbun.Focus()
                Return False
            End If
            For i = 0 To lineCnt
                charCnt = strArray(i).Length
                If (charCnt > 42) Then
                    CLMsg.Show("GE0093", "本文")
                    txtHonbun.BackColor = Color.LightPink
                    Me.txtHonbun.Focus()
                    Return False
                End If
                tCharCnt = tCharCnt + strArray(i).Length
            Next
            blnRet = True
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try
        Return blnRet                                                                       ' 戻り値格納
    End Function
    Private Function TableInsert() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim strSeq As String
        Dim intRet As Integer                                                               ' 登録結果件数
        Dim datToday As Date                                                                ' 当日の日付
        Dim strToday As String                                                              '当日の日付
        Dim c_strike_work_id As String                                                      ' 争議ID
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            datToday = CDate(Format(Now, DATE_YYYYMMDD_FORMAT))
            strToday = Format(Now, DATE_YYYYMMDD_8_FORMAT)

            Call clsDb.Connect()
            Call clsDb.BeginTran()                                                          ' トランザクション開始処理
            Select Case Me.bytTabKind
                Case STATUS_TAB_HONB
                    '本登録テーブルからID取得
                    strSeq = getSougiID()
                    STRIKE_INFO = strSeq
                    c_strike_work_id = NSMDInfo.Period & "-" & strSeq
                    'テーブルへデータ挿入
                    strSql = "Insert into strike_list (" + SQL_COLNAME_LIST + ") values('"
                    ' 一時保存争議ＩＤ
                    strSql = strSql + c_strike_work_id + "','"
                    ' 会社コード、暫定
                    strSql = strSql + MDLoginInfo.Ksh + "','"
                    ' 期ＩＤ
                    strSql = strSql + MDLoginInfo.PeriodId + "','"
                    ' 通告番号種別
                    strSql = strSql + Me.txtNoKind.Text + "','"
                    ' 争議行為通告番号
                    strSql = strSql + strSeq + "','"
                    ' 日付
                    strSql = strSql + strToday + "','"
                    ' 代表者名内容
                    strSql = strSql + txtUser.Text + "','"
                    ' 争議行為種別
                    strSql = strSql + STRIKE_KIND + "','"
                    ' 争議有効日付（開始日付）
                    strSql = strSql + Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT) + "','"
                    ' 元争議ＩＤ
                    strSql = strSql + Me.txtNoKanren.Text + "','"
                    ' 事件
                    strSql = strSql + "','"
                    ' 日時及び期間
                    strSql = strSql + "','"
                    ' 件名
                    strSql = strSql + "','"
                    ' 本文
                    strSql = strSql + txtHonbun.Text + "','"
                    ' 別紙件名
                    strSql = strSql + "','"
                    ' 備考
                    strSql = strSql + "','"
                    ' 作成日
                    strSql = strSql + datToday + "','"
                    ' 作成者個人ＩＤ
                    strSql = strSql + MDLoginInfo.UserId + "',"
                    ' 更新日
                    strSql = strSql + "Null,'"
                    ' 更新者個人ＩＤ
                    strSql = strSql + "',"
                    ' 更新回数
                    strSql = strSql + "0);"
                    intRet = clsDb.ExecuteNonQueryKeyErr(strSql)
                Case STATUS_TAB_TEMP
                    '一時登録テーブルからID取得
                    c_strike_work_id = getWorkSougiID()
                    STRIKE_INFO = "***"
                    '一時保存テーブルへデータ挿入
                    strSql = "Insert into strike_work_list (" + SQL_COLNAME_WORK + ") values('"
                    ' 一時保存争議ＩＤ
                    strSql = strSql + c_strike_work_id + "','"
                    ' 会社コード、暫定
                    strSql = strSql + MDLoginInfo.Ksh + "','"
                    ' 期ＩＤ
                    strSql = strSql + MDLoginInfo.PeriodId + "','"
                    ' 通告番号種別
                    strSql = strSql + txtNoKind.Text + "','"
                    ' 日付
                    strSql = strSql + strToday + "','"
                    ' 代表者名内容
                    strSql = strSql + txtUser.Text + "','"
                    ' 争議行為種別
                    strSql = strSql + STRIKE_KIND + "','"
                    ' 争議有効日付（開始日付）
                    strSql = strSql + Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT) + "','"
                    ' 元争議ＩＤ
                    strSql = strSql + Me.txtNoKanren.Text + "','"
                    ' 事件
                    strSql = strSql + "','"
                    ' 日時及び期間
                    strSql = strSql + "','"
                    ' 件名
                    strSql = strSql + "','"
                    ' 本文
                    strSql = strSql + Me.txtHonbun.Text + "','"
                    ' 別紙件名
                    strSql = strSql + "','"
                    ' 備考
                    strSql = strSql + "','"
                    ' 作成日
                    strSql = strSql + datToday + "','"
                    ' 作成者個人ＩＤ
                    strSql = strSql + MDLoginInfo.UserId + "',"
                    ' 更新日
                    strSql = strSql + "Null,'"
                    ' 更新者個人ＩＤ
                    strSql = strSql + "',"
                    ' 更新回数
                    strSql = strSql + "0);"
                    intRet = clsDb.ExecuteNonQueryKeyErr(strSql)
                Case Else
                    Call CLMsg.Show("GE0004", "Source TableInsert")
                    Exit Function
            End Select
            If intRet = -2 Then
                CLMsg.Show("DE0015")
                Call clsDb.RollbackTran()                                                   ' トランザクションRollBack処理
                Return False
            ElseIf intRet <> 1 Then
                CLMsg.Show("DE0005")
                Call clsDb.RollbackTran()                                                   ' トランザクションRollBack処理
                Return False
            Else
                Call clsDb.CommitTran()                                                     ' トランザクション確定処理
                blnRet = True                                                               ' 処理結果に正常を設定
            End If
        Catch ex As Exception
            Call clsDb.RollbackTran()                                                       ' トランザクションRollBack処理
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")         ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値格納
    End Function
    Private Function TableUpdate() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim datToday As Date                                                                ' 当日の日付
        Dim strToday As String                                                              '当日の日付
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            datToday = CDate(Format(Now, DATE_YYYYMMDD_FORMAT))
            strToday = Format(Now, DATE_YYYYMMDD_8_FORMAT)

            Call clsDb.Connect()
            Call clsDb.BeginTran()                                                          ' トランザクション開始処理

            Select Case Me.bytTabKind
                Case STATUS_TAB_HONB
                    '本番テーブルへデータUpdate
                    strSql = "update strike_list"
                    strSql = strSql + "  set  l_stand_name   = '" + txtUser.Text + "',"
                    strSql = strSql + "       d_strike_start = '" + Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT) + "',"
                    strSql = strSql + "       l_text        = '" + Me.txtHonbun.Text + "',"
                    strSql = strSql + "       d_up           = '" + datToday + "',"
                    strSql = strSql + "       c_user_id_up   = '" + MDLoginInfo.UserId + "',"
                    strSql = strSql + "       s_up           =  s_up + 1 "
                    strSql = strSql + "where  c_strike_id    = '" + strStrikeId + "';"
                    clsDb.ExecuteNonQuery(strSql)
                Case STATUS_TAB_TEMP
                    '一時保存テーブルへデータ挿入
                    strSql = "update strike_work_list"
                    strSql = strSql + "  set  l_stand_name   = '" + txtUser.Text + "',"
                    strSql = strSql + "       d_strike_start = '" + Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT) + "',"
                    strSql = strSql + "       l_text        = '" + Me.txtHonbun.Text + "',"
                    strSql = strSql + "       d_up           = '" + datToday + "',"
                    strSql = strSql + "       c_user_id_up   = '" + MDLoginInfo.UserId + "',"
                    strSql = strSql + "       s_up           =  s_up + 1 "
                    strSql = strSql + "where  c_strike_work_id = '" + strStrikeId + "';"
                    clsDb.ExecuteNonQuery(strSql)
                Case Else
                    Call CLMsg.Show("GE0004", "Source TableUpdate")
                    Exit Function
            End Select
            Call clsDb.CommitTran()                                                         ' トランザクション確定処理
            Call clsDb.Disconnect()
            blnRet = True                                                                   ' 処理結果に正常を設定
        Catch ex As Exception
            Call clsDb.RollbackTran()                                                         ' トランザクションRollBack処理
            Call clsDb.Disconnect()
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")         ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値格納
    End Function
    Private Function TableTrans() As Boolean
        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim strSeq As String = Nothing                                                      ' SQL今期
        Dim intRet As Integer                                                               ' 登録結果件数
        Dim datToday As Date                                                                ' 当日の日付
        Dim strToday As String                                                              '当日の日付
        Dim c_strike_work_id As String
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")         ' ログ出力（処理開始）
        Try
            datToday = CDate(Format(Now, DATE_YYYYMMDD_FORMAT))
            strToday = Format(Now, DATE_YYYYMMDD_8_FORMAT)

            '本登録テーブルからID取得
            strSeq = getSougiID()
            STRIKE_INFO = strSeq
            c_strike_work_id = NSMDInfo.Period & "-" & strSeq

            Call clsDb.Connect()
            Call clsDb.BeginTran()                                                          ' トランザクション開始処理

            'テーブルへデータ挿入
            strSql = "Insert into strike_list (" + SQL_COLNAME_LIST + ") values('"
            ' 一時保存争議ＩＤ
            strSql = strSql + c_strike_work_id + "','"
            ' 会社コード、暫定
            strSql = strSql + MDLoginInfo.Ksh + "','"
            ' 期ＩＤ
            strSql = strSql + MDLoginInfo.PeriodId + "','"
            ' 通告番号種別
            strSql = strSql + txtNoKind.Text + "','"
            ' 争議行為通告番号
            strSql = strSql + strSeq + "','"
            ' 日付
            strSql = strSql + strToday + "','"
            ' 代表者名内容
            strSql = strSql + txtUser.Text + "','"
            ' 争議行為種別
            strSql = strSql + STRIKE_KIND + "','"
            ' 争議有効日付（開始日付）
            strSql = strSql + Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT) + "','"
            ' 元争議ＩＤ
            strSql = strSql + txtNoKanren.Text + "','"
            ' 事件
            strSql = strSql + "','"
            ' 日時及び期間
            strSql = strSql + "','"
            ' 件名
            strSql = strSql + "','"
            ' 本文
            strSql = strSql + Me.txtHonbun.Text + "','"
            ' 別紙件名
            strSql = strSql + "','"
            ' 備考
            strSql = strSql + "','"
            ' 作成日
            strSql = strSql + datToday + "','"
            ' 作成者個人ＩＤ
            strSql = strSql + MDLoginInfo.UserId + "',"
            ' 更新日
            strSql = strSql + "Null,'"
            ' 更新者個人ＩＤ
            strSql = strSql + "',"
            ' 更新回数
            strSql = strSql + "0);"
            intRet = clsDb.ExecuteNonQueryKeyErr(strSql)

            '一時保存テーブルから削除
            strSql = "delete from strike_work_list "
            strSql = strSql + "where  c_strike_work_id = '" + strStrikeId + "';"
            clsDb.ExecuteNonQuery(strSql)

            If intRet = -2 Then
                CLMsg.Show("DE0015")
                Call clsDb.RollbackTran()                                                   ' トランザクションRollBack処理
                Return False
            ElseIf intRet <> 1 Then
                CLMsg.Show("DE0005")
                Call clsDb.RollbackTran()                                                   ' トランザクションRollBack処理
                Return False
            Else
                Call clsDb.CommitTran()                                                     ' トランザクション確定処理
                blnRet = True                                                               ' 処理結果に正常を設定
            End If
        Catch ex As Exception
            Call clsDb.RollbackTran()                                                         ' トランザクションRollBack処理
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")         ' ログ出力（処理終了）
        Return blnRet                                                                       ' 戻り値格納
    End Function
#End Region
End Class
#End Region