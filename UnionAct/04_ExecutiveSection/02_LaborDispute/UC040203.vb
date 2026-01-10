#Region " UC040203 "
'===========================================================================================================
'   クラスＩＤ　　：UC040203
'   クラス名称　　：労働協約第47条申し入れ - 詳細画面
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSMDConst
Imports UnionAct.NSCLMsg
Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDInfo
Imports UnionAct.NSMDChk

Public Class UC040203

#Region " 変数 "
    Private Const STRIKE_KANJIMEI_SHINSEISYA As String = "申請者"                       ' 漢字名称-申請者
    Private Const STRIKE_KANJIMEI_KENMEI As String = "件名"                             ' 漢字名称-件名
    Private Const STRIKE_KANJIMEI_KUMIAIIN As String = "組合員"                         ' 漢字名称-組合員
    Private Const STRIKE_SOUGIID_INI As String = "001"                                  ' その日の一番目の争議ID
    Private Const STRIKE_KANJIMEI_SOUGIHIDUKE As String = "争議日付"                    ' 漢字名称-争議日付

    Private Const SCREEN_ID As String = SCREEN_ID_UC040202
    Private Const SCREEN_NAME As String = SCREEN_NAME_UC040202                          ' 争議行為通告画面

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
    Private STRIKE_KIND As String = "07"                                '申し入れ通知
    Private STRIKE_INFO As String                                       'Sequence
    Private TEXT_NO_KIND As String = "Ｂ（ＡＮＡ宛）"
    Private Const SQL_COLNAME_LIST As String = "c_strike_id, c_ksh, c_period_id, k_strike_info, c_strike_info, d_strike, l_stand_name, k_strike_kind, d_strike_start, c_basis_strike_id, l_event, l_term, l_subject, l_text, l_another_subject, l_biko, d_ins, c_user_id_ins, d_up, c_user_id_up, s_up"
    Private Const SQL_COLNAME_WORK As String = "c_strike_work_id, c_ksh, c_period_id, k_strike_info, d_strike, l_stand_name, k_strike_kind, d_strike_start, c_basis_strike_id, l_event, l_term, l_subject, l_text, l_another_subject, l_biko, d_ins, c_user_id_ins, d_up, c_user_id_up, s_up"
#End Region

#Region " プロパティ "
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

#Region " イベント "
#Region " フォームロード処理 "
    '***************************************************************************************************
    '   ＩＤ　：UC040203_Load
    '   名称　：フォームロード処理
    '   概要  ：
    '   作成日：
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：
    '***************************************************************************************************
    ''' <summary>フォームロード処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub UC040203_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        '-------------------------------------------------------------------------------
        '   各Screen処理
        '-------------------------------------------------------------------------------
        If controlScreen() = False Then
            Exit Sub
        End If

        '-------------------------------------------------------------------------------
        '   グリッド初期化処理
        '-------------------------------------------------------------------------------
        If DataGridViewIni() = False Then
            Exit Sub
        End If

        '-------------------------------------------------------------------------------
        '   各データ取得処理
        '-------------------------------------------------------------------------------
        If GetData() = False Then
            Exit Sub
        End If

    End Sub
#End Region

#Region " 例文出力ボタン押下処理 "
    '************************************************************************************
    '   ＩＤ　：btnSampleText_Click
    '   名称　：例文出力ボタン押下処理
    '   概要　：例文出力
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) kIM  新規作成
    '************************************************************************************
    ''' <summary>例文出力ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSampleText_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSampleText.Click

        Dim strSamKenmei As String
        Dim strYYYY, strYYYY1 As String
        Dim strMM, strMM1 As String
        Dim strDD, strDD1 As String
        Dim dtpDateTime As DateTimePicker
        Dim dtpDateOneday As Date
        Dim strKumi As String

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

            strSamKenmei = ""
            strSamKenmei = strSamKenmei & "「労働協約第47 条(2)における" & strYYYY & "年" & strMM & "月" & strDD & "日の申し入れ該当者は、次の者とする。」"
            'strSamKenmei = strSamKenmei + "全日空乗組発 " + strKumi + " 第 " + strStrikeId + " 号（" + strYYYY + "年" + strMM + "月" + strDD + "日付）、" + vbCrLf
            'strSamKenmei = strSamKenmei + "********についての争議。"

            ' 件名入力済みかチェック
            If (Not ChkNull(txtKenmei.Text)) Then
                If CLMsg.Show("GQ0068", "件名") = DialogResult.Yes Then
                    txtKenmei.Text = strSamKenmei
                End If
            Else
                txtKenmei.Text = strSamKenmei
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub
#End Region

#Region " 組合員選択ボタン押下処理 "
    '************************************************************************************
    '   ＩＤ　：btnPickup_Click
    '   名称　：組合員選択ボタン押下処理
    '   概要　：組合員選択画面へ遷移
    '   作成日：2012/02/06 Kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/02/06 Kim  新規作成
    '************************************************************************************
    ''' <summary>組合員選択ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPickup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPickup.Click

        Dim cFormFM000204 As New FM000204()
        Dim strUserIdList As List(Of String) = New List(Of String)

        If Me.dgvResult.Rows.Count > 0 Then
            Me.Cursor = Cursors.WaitCursor
            For Each dgRow As DataGridViewRow In Me.dgvResult.Rows
                ' 既に表示済みのユーザーIDを格納
                strUserIdList.Add(dgRow.Cells(0).Value)
            Next
            ' 初期表示するメンバー削除不可
            cFormFM000204.AllowDeleteMember = True
            ' ユーザーIDリストを組合員抽出画面に渡す
            cFormFM000204.StafIDList = strUserIdList.ToArray()
        End If

        ' 組合員抽出画面の表示
        cFormFM000204.ShowDialog()

        Select Case cFormFM000204.IntQlickBtnFlag
            Case 0 ' OKボタン押下時
                ' 選択された組合員のリスト
                Dim dt As DataTable = cFormFM000204.SelectMemberList
                Me.dgvResult.RowCount = dt.Rows.Count

                For i = 0 To dt.Rows.Count - 1
                    With Me.dgvResult
                        .Rows(i).Cells.Item(0).Value = dt.Rows(i).Item("社員番号").ToString()
                        .Rows(i).Cells.Item(1).Value = dt.Rows(i).Item("名前").ToString()
                        .Rows(i).Cells.Item(2).Value = dt.Rows(i).Item("機種").ToString()
                        .Rows(i).Cells.Item(3).Value = dt.Rows(i).Item("組合支部").ToString()
                    End With
                Next
            Case 1
                ' キャンセルの場合何も行わない
        End Select

        Me.Cursor = Cursors.Default

    End Sub
#End Region

#Region " 一時保存ボタン押下処理 "
    '************************************************************************************
    '   ＩＤ　：btnSaveTmp_Click
    '   名称　：一時保存ボタン押下処理
    '   概要　：一時保存ボタン
    '   作成日：2011/11/16(水) Kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/02/03 Kim  新規作成
    '************************************************************************************
    ''' <summary>一時保存ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
#End Region

#Region " 登録確認ボタン押下処理 "
    '************************************************************************************
    '   ＩＤ　：btnChange_Click
    '   名称　：登録確認ボタン押下処理
    '   概要　：登録確認ボタン
    '   作成日：2011/11/16(水) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/16(水) Ryu  新規作成
    '************************************************************************************
    ''' <summary>登録確認ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click

        Try
            ' ステータス取得
            If (Me.bytTabKind = STATUS_TAB_TEMP) Then
                ' 一時保存登録
                Me.bytStatus = STATUS_TRANSIT       ' 内容変換（一時 ⇒ 本番）
            Else
                ' 一時保存登録以外
                Me.bytTabKind = STATUS_TAB_HONB     ' 本番登録
            End If

            ' データチェック処理
            If Not dataCheck() Then
                Exit Sub
            End If

            ' 印刷プレビュー処理
            If Not PrintPreview() Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

    End Sub
#End Region

#Region " 内容変更ボタン押下処理 "
    '************************************************************************************
    '   ＩＤ　：btnUpdate_Click
    '   名称　：内容変更ボタン押下処理
    '   概要　：登録確認ボタン、内容の修正を行う
    '   作成日：2012/01/20 Kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/20 Kim  新規作成
    '************************************************************************************
    ''' <summary>内容変更ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
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
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region " プレ印刷ボタン押下処理 "
    '************************************************************************************
    '   ＩＤ　：btnPrint_Click
    '   名称　：プレ印刷ボタン押下処理
    '   概要　：
    '   作成日：
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：
    '************************************************************************************
    ''' <summary>プレ印刷ボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click

        Try
            Me.bytStatus = STATUS_DETAIL
            ' 印刷プレビュー処理
            If PrintPreview() Then
                Exit Sub
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region " キャンセルボタン押下処理 "
    '************************************************************************************
    '   ＩＤ　：btnCancel_Click
    '   名称　：キャンセルボタン押下処理
    '   概要　：
    '   作成日：
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：
    '************************************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Dim pn As Panel                                                     'メインパネル
        Dim uc As Control                                                   '遷移先画面コントロール
        Dim clsUC040201 As UC040201

        ' 入力・変更内容破棄メッセージボックス表示
        If CLMsg.Show("GQ0007") = DialogResult.No Then
            ' 「いいえ」ボタン押下時、処理を抜ける
            Exit Sub
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
                Call UC040203_Load(sender, e)
                Me.Visible = True
            ElseIf (Me.bytStatus = STATUS_TRANSIT) Then
                Me.bytTabKind = STATUS_TAB_TEMP
                Me.bytStatus = STATUS_DETAIL
                Call UC040203_Load(sender, e)
                Me.Visible = True
            End If
        End If
    End Sub
#End Region

#Region " 戻るボタン押下処理 "
    '************************************************************************************
    '   ＩＤ　：btnBack_Click
    '   名称　：戻るボタン押下処理
    '   概要　：
    '   作成日：
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：
    '************************************************************************************
    ''' <summary>戻るボタン押下処理</summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Dim pn As Panel                                 ' メインパネル
        Dim uc As Control                               ' 遷移先画面コントロール
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
#End Region
#End Region

#Region " 関数 "
#Region " 登録・更新処理 "
    '***************************************************************************************************
    '   ＩＤ　：updateData
    '   名称　：登録・更新処理
    '   概要  ：
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火) m.suzuki
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/17(火) kim  新規作成
    '***************************************************************************************************
    ''' <summary>登録・更新処理</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function updateData() As Boolean

        ' 処理結果
        Dim blnRet As Boolean = False

        Try
            ' ステータス判定
            Select Case Me.bytStatus
                Case STATUS_INSERT
                    '-------------------------------------------------------------------------------
                    '   新規登録
                    '-------------------------------------------------------------------------------
                    If Not TableInsert() Then
                        Return blnRet
                    End If
                Case STATUS_EDIT
                    '-------------------------------------------------------------------------------
                    '   内容変更
                    '-------------------------------------------------------------------------------
                    If strStrikeId = "" Then
                        CLMsg.Show("FE0001")
                        Return blnRet
                    Else
                        TableUpdate()
                    End If
                Case STATUS_TRANSIT
                    '-------------------------------------------------------------------------------
                    '   内容変換（一時 ⇒ 本番）
                    '-------------------------------------------------------------------------------
                    If strStrikeId = "" Then
                        CLMsg.Show("FE0001")
                        Return blnRet
                    Else
                        If Not TableTrans() Then
                            Return blnRet
                        End If
                    End If
                Case Else
                    '-------------------------------------------------------------------------------
                    '   その他
                    '-------------------------------------------------------------------------------
                    Call CLMsg.Show("GE0004", "Source updateData")
                    Exit Function
            End Select

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())

            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
        Finally

        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " 印刷プレビュー処理 "
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
    ''' <summary>印刷プレビュー処理</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PrintPreview() As Boolean

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim ds As DS0402P2 = Nothing                                                        ' 帳票用データセット
        Dim fmPrint As FM000203 = Nothing                                                   ' プレビュークラス
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument = Nothing    ' レポートドキュメントオブジェクト
        Dim intRet As Integer = Nothing                                                     ' プレビュー画面処理結果
        Dim drHeader As DS0402P2.dtHeaderRow = Nothing
        Dim drDetail As DS0402P2.dtDetailRow = Nothing
        Dim strToday As String                                                              '当日の日付
        Dim intCnt As Integer = Nothing                                                     ' SQL実行結果件数
        Dim printCount As Integer
        Dim strSeq As String = "***"
        Dim pn As Panel                                                                     'メインパネル
        Dim uc As New UC040201                                                              '遷移先画面コントロール
        Dim strStrikeInfo As String()
        Try
            ' カーソル砂時計
            Cursor.Current = Cursors.WaitCursor
            strToday = Format(Now, DATE_YYYYMMDD_8_FORMAT)

            ' データセットクラス生成
            ds = New DS0402P2
            drHeader = ds.dtHeader.NewRow
            drHeader.BeginEdit()
            drHeader.l_president_name = GetPresidentName()
            drHeader.l_stand_name = Me.txtUser.Text
            drHeader.l_subject = Me.txtKenmei.Text
            drHeader.application_date = strToday.ToString.Substring(0, 4) & " 年     " & strToday.ToString.Substring(4, 2) & " 月    " & strToday.ToString.Substring(6, 2) & " 日"
            drHeader.k_strike_info = Mid(txtNoKind.Text, 1, 1)
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

            For i = 0 To Me.dgvResult.Rows.Count - 1
                drDetail = ds.dtDetail.NewRow
                drDetail.BeginEdit()
                drDetail.c_staf_id = dgvResult.Rows(i).Cells.Item(0).Value.ToString()
                drDetail.l_name = dgvResult.Rows(i).Cells.Item(1).Value.ToString()
                drDetail.EndEdit()
                ds.dtDetail.Rows.Add(drDetail)
            Next

            ' クラス生成
            fmPrint = New FM000203                                                          ' 印刷プレビュー画面
            reportObj = New CR0402P2                                                        ' レポートドキュメント生成

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
                ds.dtHeader.Rows.Item(0).Item("c_strike_info") = STRIKE_INFO                ' 取得した争議番号 + 1 を帳票に表示
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

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
            ' ログ出力（致命的エラー）
            log.Fatal(ex.Message)
        Finally
            ' カーソル初期
            Cursor.Current = Cursors.Default
            fmPrint.Dispose()
        End Try

        ' 戻り値設定
        Return blnRet

    End Function
#End Region

#Region " データチェック処理 "
    '************************************************************************************
    '   ＩＤ　：dataCheck
    '   名称　：データチェック処理
    '   概要　：入力されたデータのチェック
    '   作成日：2012/01/17 kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/17 Kim  変更
    '************************************************************************************
    ''' <summary>データチェック処理</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function dataCheck() As Boolean

        ' 処理結果
        Dim blnRet As Boolean = False

        ' エラーメッセージリスト
        Dim arlErrMsg As New ArrayList
        Dim strArray() As String
        Dim charCnt As Integer
        Dim lineCnt As Integer
        Dim tLineCnt As Integer
        Dim tCharCnt As Integer

        Try
            ' エラー箇所クリア処理
            If ClearErr(Me) = False Then
                Exit Function
            End If

            ' コントロール制御処理
            If controlScreen() = False Then
                Exit Function
            End If

            '---------------------------------------------------
            '   申請者
            '---------------------------------------------------
            ' 未入力チェック
            If ChkNull(txtUser.Text) Then
                CLMsg.Show("GE0006", "申請者")                  ' エラーメッセージ
                txtUser.BackColor = Color.LightPink             ' バックカラーピンク色
                Me.txtUser.Focus()                              ' フォーカス設定
                Return False                                    ' 異常終了
            End If

            ' 桁数チェック
            If Not ChkLength(txtUser.Text, 14) Then
                CLMsg.Show("GE0103")                            ' エラーメッセージ
                txtUser.BackColor = Color.LightPink             ' バックカラーピンク色
                Me.txtUser.Focus()                              ' フォーカス設定
                Return False                                    ' 異常終了
            End If

            '---------------------------------------------------
            '   件名
            '---------------------------------------------------
            ' 未入力チェック
            If ChkNull(txtKenmei.Text) Then
                CLMsg.Show("GE0006", "件名")                    ' エラーメッセージ
                txtKenmei.BackColor = Color.LightPink           ' バックカラーピンク色
                Me.txtKenmei.Focus()                            ' フォーカス設定
                Return False                                    ' 異常終了
            End If

            '---------------------------------------------------
            '   争議日付
            '---------------------------------------------------
            ' 現在日より過去日を指定することはできません
            If Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT) < Format(Now, DATE_YYYYMMDD_8_FORMAT) Then
                CLMsg.Show("GE0077", "争議日付")                ' エラーメッセージ
                Me.dtpSougiDate.Focus()                         ' フォーカス設定
                Return False                                    ' 異常終了
            Else
                dtpSougiDate.BackColor = Color.White            ' バックカラー白色
            End If

            '---------------------------------------------------
            '  件名
            '---------------------------------------------------
            ' 2022/09/07(水) m.suzuki 変更 Start
            '〇件名の合計文字Max 80char*3line 文字チェック
            '×件名の合計文字Max 42char*3line 文字チェック
            tLineCnt = 0
            charCnt = 0
            strArray = Split(txtKenmei.Text, vbCrLf)
            lineCnt = UBound(strArray)
            tLineCnt = tLineCnt + lineCnt + 1
            For i = 0 To lineCnt
                charCnt = strArray(i).Length
                ' 桁数チェック
                'If (charCnt > 42) Then
                If (charCnt > 80) Then
                    CLMsg.Show("GE0093", "件名")                ' エラーメッセージ
                    txtKenmei.BackColor = Color.LightPink       ' バックカラーピンク色
                    Me.txtKenmei.Focus()                        ' フォーカス設定
                    Return False                                ' 異常終了
                End If
                tCharCnt = tCharCnt + strArray(i).Length
            Next

            ' 1行50文字チェック
            'If (tCharCnt > 42 * 3) Then
            If (tCharCnt > 80 * 3) Then
                CLMsg.Show("GE0103", "件名")                    ' エラーメッセージ
                txtKenmei.BackColor = Color.LightPink           ' バックカラーピンク色
                Me.txtKenmei.Focus()                            ' フォーカス設定
                Return False                                    ' 異常終了
            End If

            ' 行数チェック
            If (tLineCnt > 3) Then
                CLMsg.Show("GE0103", "件名")                    ' エラーメッセージ
                txtKenmei.BackColor = Color.LightPink           ' バックカラーピンク色
                Me.txtKenmei.Focus()                            ' フォーカス設定
                Return False                                    ' 異常終了
            End If
            ' 2022/09/07(水) m.suzuki 変更 End

            '---------------------------------------------------
            '   組合員選択一覧
            '---------------------------------------------------
            ' 1人以上選択済みかチェック
            If (dgvResult.Rows.Count = 0) Then
                CLMsg.Show("GE0072")                            ' エラーメッセージ
                'dgvResult.BackgroundColor = Color.LightPink     ' バックカラーピンク色
                Me.dgvResult.Focus()                            ' フォーカス設定
                Return False                                    ' 異常終了
            End If

            ' 戻り値に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region " 各データ取得処理 "
    '************************************************************************************
    '   ＩＤ　：GetData
    '   名称　：各データ取得処理
    '   概要　：
    '   作成日：2012/01/17 kim
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2012/01/17 Kim  変更
    '************************************************************************************
    ''' <summary>各データ取得処理</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetData() As Boolean

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim intCnt, intRetCnt As Integer
        Dim strSql As String

        Try
            ' ステータス判定
            Select Case Me.bytStatus
                Case STATUS_INSERT
                    Me.txtNoKind.Text = TEXT_NO_KIND
                    Me.txtNo.Text = "*****"
                    Me.txtSougiDate.Text = Format(Now, DATE_YYYYMMDD_FORMAT)
                    Me.dtpSougiDate.Value = CDate(Format(Now, DATE_YYYYMMDD_FORMAT))
                    Me.txtUser.Text = "組合長　" & GetUnionLeaderName()
                    STRIKE_INFO = "*****"
                Case STATUS_DETAIL
                    ' データベース接続
                    Call clsDb.Connect()
                    '申請者 Check Null
                    If ChkNull(Me.strStrikeId) Then
                        CLMsg.Show("GE0001")
                        Return False
                    End If
                    intCnt = 0
                    If (Me.bytTabKind = STATUS_TAB_HONB) Then
                        '-------------------------------------------------------------------------------
                        '   SQL作成
                        '-------------------------------------------------------------------------------
                        strSql = ""
                        strSql = strSql & " select stli.k_strike_info,stli.c_strike_info,"
                        strSql = strSql & "        stli.d_strike,stli.l_stand_name,"
                        strSql = strSql & "        stli.d_strike_start,stli.l_subject,"
                        strSql = strSql & "        stli.c_strike_id,stli.c_basis_strike_id "
                        strSql = strSql & " from   strike_list stli "
                        strSql = strSql & " where  stli.c_strike_id = '" & strStrikeId & "'"
                        strSql = strSql & ";"
                        ' SQL実行
                        dt = clsDb.ExecuteSql(strSql)
                        Me.txtNoKind.Text = NVL(dt.Rows(0).Item("k_strike_info"))
                        STRIKE_INFO = dt.Rows(0).Item("c_strike_info")
                        If (dt.Rows(0).Item("c_strike_id") <> "") Then
                            Me.txtNo.Text = dt.Rows(0).Item("c_strike_id")
                        Else
                            Me.txtNo.Text = "*****"
                        End If

                        Me.txtShinseiDate.Text = dt.Rows(0).Item("d_strike").ToString.Substring(0, 4) & "/" & dt.Rows(0).Item("d_strike").ToString.Substring(4, 2) & "/" & dt.Rows(0).Item("d_strike").ToString.ToString.Substring(6, 2)
                        Me.txtUser.Text = dt.Rows(0).Item("l_stand_name")
                        Me.txtSougiDate.Text = dt.Rows(0).Item("d_strike_start").ToString.Substring(0, 4) & "年" & dt.Rows(0).Item("d_strike_start").ToString.Substring(4, 2) & "月" & dt.Rows(0).Item("d_strike_start").ToString.ToString.Substring(6, 2) & "日"
                        Me.dtpSougiDate.Value = Date.Parse(Format(CInt(dt.Rows(0).Item("d_strike_start").ToString), "0000/00/00"))
                        Me.txtKenmei.Text = NVL(dt.Rows(0).Item("l_subject"))
                        If (dt.Rows(0).Item("d_strike_start").ToString < Format(Now, DATE_YYYYMMDD_8_FORMAT)) Then
                            Me.btnUpdate.Visible = False
                        End If
                        'grid情報表示
                        '-------------------------------------------------------------------------------
                        '   SQL作成
                        '-------------------------------------------------------------------------------
                        strSql = " select stli.c_strike_id,stli.c_user_id,"
                        strSql = strSql & "        staf.l_name as user_name,"
                        strSql = strSql & "        cd1.l_name as belong_name,"
                        strSql = strSql & "        cd2.l_name as model_name"
                        strSql = strSql & " from   strike_member_list stli,"
                        strSql = strSql & "        constant_dtl cd1, "
                        strSql = strSql & "        constant_dtl cd2, "
                        strSql = strSql & "        staf_attribute staf, "
                        strSql = strSql & "        (SELECT staf1.c_user_id,MAX(staf1.d_from ) AS c_d_from"
                        strSql = strSql & "         FROM   staf_attribute AS staf1"
                        strSql = strSql & "         WHERE  staf1.k_del = '0'"
                        strSql = strSql & "         GROUP BY staf1.c_user_id ) AS staf2"
                        strSql = strSql & " where  stli.c_strike_id = '" & strStrikeId & "'"
                        strSql = strSql & " and    staf2.c_user_id = staf.c_user_id"
                        strSql = strSql & " and    staf2.c_d_from =   staf.d_from"
                        strSql = strSql & " and    stli.c_user_id = staf.c_user_id"
                        strSql = strSql & " and    cd1.c_constant_seq = staf.k_belonging"
                        strSql = strSql & " and    cd1.c_constant = 'BELONGING'"
                        strSql = strSql & " and    cd2.c_constant_seq = staf.k_model"
                        strSql = strSql & " and    cd2.c_constant = 'MODEL'"
                        strSql = strSql & " order by CLng(stli.c_user_id)" & UtDb.DbOrderOffset & ";"   'ok

                        ' SQL実行
                        dt = clsDb.ExecuteSql(strSql)

                        intRetCnt = dt.Rows.Count                                                                ' 件数取得
                        ' 件数チェック
                        If intRetCnt > 0 Then
                            ' 1件以上の処理
                            dgvResult.RowCount = intRetCnt                                                        ' 縦総数設定
                            For i = 0 To intRetCnt - 1                                                              ' レコード数分ループ
                                ' データ設定
                                With Me.dgvResult.Rows(i).Cells
                                    If (IsDBNull(dt.Rows(i).Item("c_user_id"))) Then
                                        .Item(0).Value = "*****"
                                    Else
                                        .Item(0).Value = dt.Rows(i).Item("c_user_id")
                                    End If
                                    .Item(1).Value = dt.Rows(i).Item("user_name")
                                    .Item(2).Value = dt.Rows(i).Item("model_name")
                                    .Item(3).Value = dt.Rows(i).Item("belong_name")
                                    Me.dgvResult.Rows(i).Visible = True
                                End With
                            Next
                        Else
                            ' 0件の処理
                            CLMsg.Show("DI0001")                                                                    ' 対象データなしメッセージボックス表示
                        End If
                    ElseIf (Me.bytTabKind = STATUS_TAB_TEMP) Then
                        '未テスト---------------------------------------===================
                        '-------------------------------------------------------------------------------
                        '   SQL作成
                        '-------------------------------------------------------------------------------
                        strSql = ""
                        strSql = strSql & " select stli.k_strike_info,"
                        strSql = strSql & "        stli.d_strike,stli.l_stand_name,"
                        strSql = strSql & "        stli.d_strike_start,stli.l_event,stli.l_term,"
                        strSql = strSql & "        stli.l_subject,stli.l_text,stli.c_strike_work_id"
                        strSql = strSql & " from   strike_work_list stli"
                        strSql = strSql & " where  stli.c_strike_work_id = '" & strStrikeId & "';"
                        ' SQL実行
                        dt = clsDb.ExecuteSql(strSql)
                        Me.txtNoKind.Text = NVL(dt.Rows(0).Item("k_strike_info"))
                        Me.txtNo.Text = "*****"
                        Me.txtShinseiDate.Text = dt.Rows(0).Item("d_strike").ToString.Substring(0, 4) & "/" & dt.Rows(0).Item("d_strike").ToString.Substring(4, 2) & "/" & dt.Rows(0).Item("d_strike").ToString.ToString.Substring(6, 2)
                        Me.txtUser.Text = dt.Rows(0).Item("l_stand_name")
                        Me.txtSougiDate.Text = dt.Rows(0).Item("d_strike_start").ToString.Substring(0, 4) & "年" & dt.Rows(0).Item("d_strike_start").ToString.Substring(4, 2) & "月" & dt.Rows(0).Item("d_strike_start").ToString.ToString.Substring(6, 2) & "日"
                        Me.dtpSougiDate.Value = Date.Parse(Format(CInt(dt.Rows(0).Item("d_strike_start").ToString), "0000/00/00"))
                        Me.txtKenmei.Text = NVL(dt.Rows(0).Item("l_subject"))
                        STRIKE_INFO = "*****"
                        If (dt.Rows(0).Item("d_strike_start").ToString < Format(Now, DATE_YYYYMMDD_8_FORMAT)) Then
                            Me.btnUpdate.Visible = False
                        End If
                        '========================================================================================================
                        'grid情報表示
                        '-------------------------------------------------------------------------------
                        '   SQL作成
                        '-------------------------------------------------------------------------------
                        strSql = " select stli.c_strike_work_id,stli.c_user_id,"
                        strSql = strSql & "        staf.l_name as user_name,"
                        strSql = strSql & "        cd1.l_name as belong_name,"
                        strSql = strSql & "        cd2.l_name as model_name"
                        strSql = strSql & " from   strike_work_member_list stli,"
                        strSql = strSql & "        constant_dtl cd1, "
                        strSql = strSql & "        constant_dtl cd2, "
                        strSql = strSql & "        staf_attribute staf, "
                        strSql = strSql & "        (SELECT staf1.c_user_id,MAX(staf1.d_from ) AS c_d_from"
                        strSql = strSql & "         FROM   staf_attribute AS staf1"
                        strSql = strSql & "         WHERE  staf1.k_del = '0'"
                        strSql = strSql & "         GROUP BY staf1.c_user_id ) AS staf2"
                        strSql = strSql & " where  stli.c_strike_work_id = '" & strStrikeId & "'"
                        strSql = strSql & " and    staf2.c_user_id = staf.c_user_id"
                        strSql = strSql & " and    staf2.c_d_from =   staf.d_from"
                        strSql = strSql & " and    stli.c_user_id = staf.c_user_id"
                        strSql = strSql & " and    cd1.c_constant_seq = staf.k_belonging"
                        strSql = strSql & " and    cd1.c_constant = 'BELONGING'"
                        strSql = strSql & " and    cd2.c_constant_seq = staf.k_model"
                        strSql = strSql & " and    cd2.c_constant = 'MODEL'"
                        strSql = strSql & " order by CLng(stli.c_user_id)" & UtDb.DbOrderOffset & ";"
                        'todo:
                        ' SQL実行
                        dt = clsDb.ExecuteSql(strSql)

                        intRetCnt = dt.Rows.Count                                                                ' 件数取得
                        ' 件数チェック
                        If intRetCnt > 0 Then
                            ' 1件以上の処理
                            dgvResult.RowCount = intRetCnt                                                        ' 縦総数設定
                            For i = 0 To intRetCnt - 1                                                              ' レコード数分ループ
                                ' データ設定
                                With Me.dgvResult.Rows(i).Cells
                                    If (IsDBNull(dt.Rows(i).Item("c_user_id"))) Then
                                        .Item(0).Value = "*****"
                                    Else
                                        .Item(0).Value = dt.Rows(i).Item("c_user_id")
                                    End If
                                    .Item(1).Value = dt.Rows(i).Item("user_name")
                                    .Item(2).Value = dt.Rows(i).Item("model_name")
                                    .Item(3).Value = dt.Rows(i).Item("belong_name")
                                    Me.dgvResult.Rows(i).Visible = True
                                End With
                            Next
                        End If
                    End If
                    Call clsDb.Disconnect()
                Case Else
                    Call CLMsg.Show("GE0004", "Source GetData")
                    Exit Function
            End Select

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally

        End Try

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region " コントロール制御処理 "
    '************************************************************************************
    '   ＩＤ　：controlScreen
    '   名称　：コントロール制御処理
    '   概要　：ステータスを判定してコントロールを制御
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '************************************************************************************
    ''' <summary>コントロール制御処理</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function controlScreen() As Boolean

        ' 処理結果
        Dim blnRet As Boolean = False

        Try
            Me.txtNoKind.Visible = True
            Me.txtNoKind.ReadOnly = True
            Me.txtNoKind.BackColor = Color.LightYellow
            Me.txtNo.Visible = True
            Me.txtNo.ReadOnly = True
            Me.txtNo.BackColor = Color.LightYellow
            Me.lblShinseiDate.Visible = True
            Me.txtShinseiDate.Visible = True
            Me.txtShinseiDate.ReadOnly = True
            Me.txtShinseiDate.BackColor = Color.LightYellow
            Me.lblTitle.Text = "労働協約第４７条申し入れ－詳細"
            Me.txtUser.BackColor = Color.White

            ' ステータス判定
            Select Case Me.bytStatus
                Case STATUS_DETAIL
                    '-------------------------------------------------------------------------------
                    '   内容表示
                    '-------------------------------------------------------------------------------
                    Me.btnSampleText.Visible = True
                    Me.btnSampleText.Enabled = False
                    Me.dtpSougiDate.Visible = False
                    Me.txtSougiDate.Visible = True
                    Me.btnCancel.Visible = False
                    Me.btnBack.Visible = True
                    Me.btnChange.Visible = False
                    Me.btnUpdate.Visible = True
                    Me.btnSaveTmp.Visible = False
                    Me.btnPrint.Visible = True
                    Me.btnPickup.Visible = False
                    Me.txtUser.ReadOnly = True
                    Me.txtSougiDate.ReadOnly = True
                    Me.txtKenmei.ReadOnly = True
                    Me.txtSougiDate.BackColor = Color.White
                    Me.txtKenmei.BackColor = Color.White
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
                    '-------------------------------------------------------------------------------
                    '   内容変更
                    '   内容変換（一時 ⇒ 本番）
                    '-------------------------------------------------------------------------------
                    Me.btnSampleText.Visible = True
                    Me.btnSampleText.Enabled = True
                    Me.dtpSougiDate.Visible = True
                    Me.txtSougiDate.Visible = False
                    Me.btnCancel.Visible = True
                    Me.btnBack.Visible = False
                    Me.btnChange.Visible = True
                    Me.btnUpdate.Visible = False
                    Me.lblIchiji.Visible = False
                    Me.btnPrint.Visible = False
                    Me.btnPickup.Visible = True
                    Me.txtUser.ReadOnly = False
                    Me.txtKenmei.ReadOnly = False
                    Select Case Me.bytTabKind
                        Case STATUS_TAB_HONB
                            Me.lblIchiji.Visible = False
                            Me.btnSaveTmp.Visible = False
                            Me.fraDetail.BackColor = SystemColors.Control
                        Case STATUS_TAB_TEMP
                            Me.btnSaveTmp.Visible = True
                            Me.lblIchiji.Visible = True
                            Me.fraDetail.BackColor = Color.PapayaWhip
                        Case Else
                            Call CLMsg.Show("GE0004", "Source controlScreen")
                            Exit Function
                    End Select

                Case STATUS_INSERT
                    '-------------------------------------------------------------------------------
                    '   新規登録
                    '-------------------------------------------------------------------------------
                    Me.btnSampleText.Visible = True
                    Me.btnSampleText.Enabled = True
                    Me.dtpSougiDate.Visible = True
                    Me.txtSougiDate.Visible = False
                    Me.btnCancel.Visible = True
                    Me.btnBack.Visible = False
                    Me.btnChange.Visible = True
                    Me.btnUpdate.Visible = False
                    Me.btnSaveTmp.Visible = True
                    Me.lblIchiji.Visible = False
                    Me.btnPrint.Visible = False
                    Me.lblShinseiDate.Visible = False
                    Me.txtShinseiDate.Visible = False
                    Me.btnPickup.Visible = True
                    Me.txtUser.ReadOnly = False
                    Me.txtKenmei.ReadOnly = False
                Case Else
                    Call CLMsg.Show("GE0004", "Source controlScreen")
                    Exit Function
            End Select

            ' 処理結果に正常を設定
            blnRet = True

        Catch ex As Exception
            Return False
        End Try

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region " 争議ID取得 "
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
    ''' <summary>争議ID取得</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
            strSql = strSql + " order by 1 desc; "  'chk

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

#Region " TableInsert "
    '************************************************************************************
    '   ＩＤ　：TableInsert
    '   名称　：
    '   概要　：
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '************************************************************************************
    ''' <summary>TableInsert</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TableInsert() As Boolean

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim strSeq As String
        Dim intRet As Integer
        Dim datToday As Date                                                                ' 当日の日付
        Dim strToday As String                                                              '当日の日付
        Dim c_strike_work_id As String                                                      ' 争議ID
        Dim c_user_id As String
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
                    strSql = strSql + "','"
                    ' 事件
                    strSql = strSql + "','"
                    ' 日時及び期間
                    strSql = strSql + "','"
                    ' 件名
                    strSql = strSql + Me.txtKenmei.Text + "','"
                    ' 本文
                    strSql = strSql + "','"
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

                    'member登録
                    For i = 0 To Me.dgvResult.Rows.Count - 1
                        c_user_id = dgvResult.Rows(i).Cells.Item(0).Value.ToString()
                        'テーブルへデータ挿入
                        strSql = "Insert into strike_member_list (c_strike_id, c_user_id, d_ins, c_user_id_ins) values('"
                        ' 争議ＩＤ
                        strSql = strSql + c_strike_work_id + "','"
                        '個人認証ＩＤ
                        strSql = strSql + c_user_id + "','"
                        ' 作成日
                        strSql = strSql + datToday + "','"
                        ' 作成者個人ＩＤ
                        strSql = strSql + MDLoginInfo.UserId + "');"
                        clsDb.ExecuteNonQuery(strSql)
                    Next
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
                    strSql = strSql + "','"
                    ' 事件
                    strSql = strSql + "','"
                    ' 日時及び期間
                    strSql = strSql + "','"
                    ' 件名
                    strSql = strSql + Me.txtKenmei.Text + "','"
                    ' 本文
                    strSql = strSql + "','"
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

                    'member登録
                    For i = 0 To Me.dgvResult.Rows.Count - 1
                        c_user_id = dgvResult.Rows(i).Cells.Item(0).Value.ToString()
                        'テーブルへデータ挿入
                        strSql = "Insert into strike_work_member_list (c_strike_work_id, c_user_id, d_ins, c_user_id_ins) values('"
                        ' 争議ＩＤ
                        strSql = strSql + c_strike_work_id + "','"
                        '個人認証ＩＤ
                        strSql = strSql + c_user_id + "','"
                        ' 作成日
                        strSql = strSql + datToday + "','"
                        ' 作成者個人ＩＤ
                        strSql = strSql + MDLoginInfo.UserId + "');"
                        clsDb.ExecuteNonQuery(strSql)
                    Next
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
                Call clsDb.CommitTran()                                                         ' トランザクション確定処理
                blnRet = True                                                                   ' 処理結果に正常を設定
            End If
        Catch ex As Exception
            Call clsDb.RollbackTran()                                                       ' トランザクションRollBack処理
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")         ' ログ出力（処理終了）

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region " TableUpdate "
    '************************************************************************************
    '   ＩＤ　：TableUpdate
    '   名称　：
    '   概要　：
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '************************************************************************************
    ''' <summary>TableUpdate</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function TableUpdate() As Boolean

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim datToday As Date                                                                ' 当日の日付
        Dim strToday As String                                                              '当日の日付
        Dim c_user_id As String
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
                    strSql = strSql + "       l_subject        = '" + Me.txtKenmei.Text + "',"
                    strSql = strSql + "       d_up           = '" + datToday + "',"
                    strSql = strSql + "       c_user_id_up   = '" + MDLoginInfo.UserId + "',"
                    strSql = strSql + "       s_up           =  s_up + 1 "
                    strSql = strSql + "where  c_strike_id    = '" + strStrikeId + "';"
                    clsDb.ExecuteNonQuery(strSql)

                    'member削除
                    strSql = "delete from strike_member_list where c_strike_id = '" + strStrikeId + "';"
                    clsDb.ExecuteNonQuery(strSql)

                    'member Insert
                    For i = 0 To Me.dgvResult.Rows.Count - 1
                        c_user_id = dgvResult.Rows(i).Cells.Item(0).Value.ToString()
                        'テーブルへデータ挿入
                        strSql = "Insert into strike_member_list (c_strike_id, c_user_id, d_ins, c_user_id_ins) values('"
                        ' 争議ＩＤ
                        strSql = strSql + strStrikeId + "','"
                        '個人認証ＩＤ
                        strSql = strSql + c_user_id + "','"
                        ' 作成日
                        strSql = strSql + datToday + "','"
                        ' 作成者個人ＩＤ
                        strSql = strSql + MDLoginInfo.UserId + "');"
                        clsDb.ExecuteNonQuery(strSql)
                    Next
                Case STATUS_TAB_TEMP
                    '一時保存テーブルへデータ挿入
                    strSql = "update strike_work_list"
                    strSql = strSql + "  set  l_stand_name   = '" + txtUser.Text + "',"
                    strSql = strSql + "       d_strike_start = '" + Format(dtpSougiDate.Value.Date, DATE_YYYYMMDD_8_FORMAT) + "',"
                    strSql = strSql + "       l_subject        = '" + Me.txtKenmei.Text + "',"
                    strSql = strSql + "       d_up           = '" + datToday + "',"
                    strSql = strSql + "       c_user_id_up   = '" + MDLoginInfo.UserId + "',"
                    strSql = strSql + "       s_up           =  s_up + 1 "
                    strSql = strSql + "where  c_strike_work_id = '" + strStrikeId + "';"
                    clsDb.ExecuteNonQuery(strSql)

                    'member削除
                    strSql = "delete from strike_work_member_list where c_strike_work_id = '" + strStrikeId + "';"
                    clsDb.ExecuteNonQuery(strSql)

                    'member Insert
                    For i = 0 To Me.dgvResult.Rows.Count - 1
                        c_user_id = dgvResult.Rows(i).Cells.Item(0).Value.ToString()
                        'テーブルへデータ挿入
                        strSql = "Insert into strike_work_member_list (c_strike_work_id, c_user_id, d_ins, c_user_id_ins) values('"
                        ' 争議ＩＤ
                        strSql = strSql + strStrikeId + "','"
                        '個人認証ＩＤ
                        strSql = strSql + c_user_id + "','"
                        ' 作成日
                        strSql = strSql + datToday + "','"
                        ' 作成者個人ＩＤ
                        strSql = strSql + MDLoginInfo.UserId + "');"
                        clsDb.ExecuteNonQuery(strSql)
                    Next
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

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region " TableTrans "
    '************************************************************************************
    '   ＩＤ　：TableTrans
    '   名称　：
    '   概要　：
    '   作成日：2011/11/15(火) Ryu
    '   更新日：
    '   備考  ：
    '------------------------------------------------------------------------------------
    '   履歴　：2011/11/15(火) Ryu  新規作成
    '************************************************************************************
    ''' <summary>TableTrans</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
        Dim c_user_id As String

        ' ログ出力（処理開始）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理開始")

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
            strSql = strSql + "','"
            ' 事件
            strSql = strSql + "','"
            ' 日時及び期間
            strSql = strSql + "','"
            ' 件名
            strSql = strSql + Me.txtKenmei.Text + "','"
            ' 本文
            strSql = strSql + "','"
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

            'member登録
            For i = 0 To Me.dgvResult.Rows.Count - 1
                c_user_id = dgvResult.Rows(i).Cells.Item(0).Value.ToString()
                'テーブルへデータ挿入
                strSql = "Insert into strike_member_list (c_strike_id, c_user_id, d_ins, c_user_id_ins) values('"
                ' 争議ＩＤ
                strSql = strSql + c_strike_work_id + "','"
                '個人認証ＩＤ
                strSql = strSql + c_user_id + "','"
                ' 作成日
                strSql = strSql + datToday + "','"
                ' 作成者個人ＩＤ
                strSql = strSql + MDLoginInfo.UserId + "');"
                clsDb.ExecuteNonQuery(strSql)
            Next

            'member削除
            strSql = "delete from strike_work_member_list where c_strike_work_id = '" + strStrikeId + "';"
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
            Call clsDb.RollbackTran()                                                       ' トランザクションRollBack処理
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_ID, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        Finally
            Call clsDb.Disconnect()
        End Try

        ' ログ出力（処理終了）
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & "処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region " DataGridViewIni "
    '***************************************************************************************************
    '   ＩＤ　：DataGridViewIni
    '   名称　：グリッド初期化処理
    '   概要　：グリッドの初期化を行う。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/04(金) m.suzuki
    '   更新日：
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
            Me.dgvResult.ColumnCount = 4                                                        ' 横
            ' 固定行
            'Me.cfgResult.Rows.Fixed = FLEXGRID_ROWS_FIXED                                      ' 縦
            'Me.cfgResult.Cols.Fixed = FLEXGRID_COLS_FIXED                                      ' 横
            ' スクロールバー
            'Me.dgvResult.ScrollBars = ScrollBars.Vertical                                      ' 縦のみ
            Me.dgvResult.ScrollBars = ScrollBars.Both                                           ' 縦横両方
            ' 1行選択モード
            Me.dgvResult.SelectionMode = DataGridViewSelectionMode.FullRowSelect                ' 1行選択
            Me.dgvResult.MultiSelect = False                                                    ' 複数選択なし
            ' サイズ変更
            Me.dgvResult.AllowUserToResizeColumns = True                                        ' 列サイズ変更可
            Me.dgvResult.AllowUserToResizeRows = False                                          ' 行サイズ変更不可
            ' バックカラー
            'Me.dgvResult.RowsDefaultCellStyle.BackColor = Color.White                          ' 全ての列の背景色を白色
            '-----------------------------------------------------------------------------------
            '   ヘッダー部設定
            '-----------------------------------------------------------------------------------
            ' ヘッダー文字列
            Me.dgvResult.Columns(0).HeaderText = "社員番号"                                     ' 社員番号
            Me.dgvResult.Columns(1).HeaderText = "名前"                                         ' 名前
            Me.dgvResult.Columns(2).HeaderText = "機種"                                         ' 機種
            Me.dgvResult.Columns(3).HeaderText = "組合支部"                                     ' 組合支部
            ' ヘッダー文字位置
            Me.dgvResult.Columns(0).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 社員番号
            Me.dgvResult.Columns(1).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 名前
            Me.dgvResult.Columns(2).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 機種
            Me.dgvResult.Columns(3).HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 組合支部
            '-----------------------------------------------------------------------------------
            '   カラム部設定
            '-----------------------------------------------------------------------------------
            ' カラム文字位置
            Me.dgvResult.Columns(0).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 社員番号
            Me.dgvResult.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft        ' 名前
            Me.dgvResult.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 機種
            Me.dgvResult.Columns(3).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter      ' 組合支部
            ' カラム幅
            Me.dgvResult.Columns(0).Width = 100                                                                             ' 社員番号
            Me.dgvResult.Columns(1).Width = 200                                                                             ' 名前
            Me.dgvResult.Columns(2).Width = 100                                                                             ' 機種
            Me.dgvResult.Columns(3).Width = 100                                                                             ' 組合支部
            ' カラム表示有無
            Me.dgvResult.Columns(0).Visible = True                                                                          ' 社員番号
            Me.dgvResult.Columns(1).Visible = True                                                                          ' 名前
            Me.dgvResult.Columns(2).Visible = True                                                                          ' 組合支部
            Me.dgvResult.Columns(3).Visible = True                                                                          ' 組合員種別
            blnRet = True                                                                                                   ' 戻り値格納
        Catch ex As Exception
            log.Fatal(ex.Message)                                                                                           ' ログ出力（致命的エラー）
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID, SCREEN_NAME, System.Reflection.MethodInfo.GetCurrentMethod.Name())
        End Try

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region " 一時登録ID取得 "
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
    ''' <summary>一時登録ID取得</summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getWorkSougiID() As String

        Dim blnRet As Boolean = False                                                       ' 処理結果
        Dim clsDb As New CLAccessMdb                                                        ' データベースクラス
        Dim dt As DataTable = Nothing                                                       ' SQL実行結果データテーブル
        Dim strSql As String = Nothing                                                      ' SQL今期
        Dim strSeq As String

        Try
            Call clsDb.Connect()
            '一時登録テーブルから
            strSql = "select CInt(c_strike_work_id) as c_strike_work_max from strike_work_list order by 1 desc" 'chk
            dt = clsDb.ExecuteSql(strSql)
            If dt.Rows.Count > 0 Then
                'If dt.Rows(0).Item("c_strike_work_max") = "" Then
                strSeq = CLng(CStr(dt.Rows(0).Item("c_strike_work_max"))) + 1
            Else
                strSeq = 1
            End If
            Return strSeq
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC040202, SCREEN_ID_UC040202, "getWorkSougiID")
            Return False
        Finally
            Call clsDb.Disconnect()
        End Try

    End Function
#End Region
#End Region

End Class
#End Region