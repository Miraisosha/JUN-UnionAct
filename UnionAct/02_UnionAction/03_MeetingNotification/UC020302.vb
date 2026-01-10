#Region "UC020302"
'===========================================================================================================
'   クラスＩＤ　　：UC020302
'   クラス名称　　：会議通知－新規登録
'   備考  　　　　：
'===========================================================================================================

Imports UnionAct.NSCLAccessMdb
Imports UnionAct.NSMDCommon
Imports UnionAct.NSMDChk
Imports UnionAct.NSMDInfo
Imports UnionAct.NSCLMsg
Imports UnionAct.NSMDConst
Imports UnionAct.GUI.Common

Public Class UC020302

#Region "定数・メンバ変数"
    '合同区分定数
    Public Const UNION_OFF As String = "0"
    Public Const UNION_ON As String = "1"
    '開催種類定数
    Public Const INFORMATION_TYPE_OPEN = "0"   '開催
    Public Const INFORMATION_TYPE_CHANGE = "1" '変更
    Public Const INFORMATION_TYPE_STOP = "2"   '中止
    '合同
    Private Const UNION_ON_STRING As String = "合同"
    '開催種類
    Public Const INFORMATION_TYPE_OPEN_STRING As String = "開催"
    Public Const INFORMATION_TYPE_CHANGE_STRING As String = "変更"
    Public Const INFORMATION_TYPE_STOP_STRING As String = "中止"
    '支部
    Public Const SHIBU_TOKYO_STRING As String = "東京"
    Public Const SHIBU_OOSAKA_STRING As String = "大阪"
    Public Const SHIBU_OTHER_STRING As String = "その他"
    '未確定社員番号
    Private Const UNDEFINE_SHAIN_CODE As String = "*****"
    '未確定機種
    Private Const UNDEFINE_MACHINE_TYPE As String = "***"
    '未確定社員名
    Private Const UNDEFINE_SHAIN_NAME As String = "**********"
    '時間の初期値
    Private Const STR_FIRST_TIME As String = "00時00分"

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

    'TEL・FAX番号格納用
    Private Structure TelFax
        Public strTokyoTel
        Public strTokyoFax
        Public strOsakaTel
        Public strOsakaFax
    End Structure

    'テーブル登録用会議通知構造体
    Public Structure miData
        Public strKsh As String
        Public strApplyAreaCode As String
        Public strPeriodId As String
        Public strMeetingNumber As String
        Public intSeq As Integer
        Public strCommitteeId As String
        Public strUnioncode As String
        Public strInformationTypeCode As String
        Public strInformationName As String
        Public dtmMeeting1 As Object
        Public strMeetingTimeFrom1 As String
        Public strMeetingTimeTo1 As String
        Public strFlightTo1 As String
        Public strFlightTimeTo1 As String
        Public strFlightBack1 As String
        Public strFlightTimeBack1 As String
        Public dtmMeeting2 As Object
        Public strMeetingTimeFrom2 As String
        Public strMeetingTimeTo2 As String
        Public strFlightTo2 As String
        Public strFlightTimeTo2 As String
        Public strFlightBack2 As String
        Public strFlightTimeBack2 As String
        Public dtmMeeting3 As Object
        Public strMeetingTimeFrom3 As String
        Public strMeetingTimeTo3 As String
        Public strFlightTo3 As String
        Public strFlightTimeTo3 As String
        Public strFlightBack3 As String
        Public strFlightTimeBack3 As String
        Public strOpenBeBiginting As String
        Public strPlace As String
        Public strSubject1 As String
        Public strSubject2 As String
        Public strSubject3 As String
        Public strSubject4 As String
        Public strSubject5 As String
        Public strBiko1 As String
        Public strBiko2 As String
        Public strBiko3 As String
        Public dtmInsertDate As Object
        Public strInsertUserId As String
        Public dtmUpdateDate As Object
        Public strUpdateUserId As String
        Public intUpdateTime As Integer
    End Structure

    '検索結果で選択された会議通知のデータリスト
    Private _drMeetingInformation As DataRow = Nothing
    '選択ボタンの値取得
    Private _intSelectBtn As Integer = -1
    '0 = 開催登録　
    '1 = 会議通知‐変更　
    '2 = 会議通知‐中止　
    '3 = 会議通知‐合同 
    '4 = 会議通知‐会議詳細
    '5 = 一時保存‐会議詳細

    '選択ボタンの値取得（検索画面へ戻るとき）
    Private _intReturnBtn As Integer = -1
    '0 = 登録確認　
    '1 = キャンセル
    '2 = 一時保存　
    '3 = 戻る 

    ' log4net
    Private Shared ReadOnly log As log4net.ILog = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#Region "プロパティ"
    Public Property IntClickBtnFlg() As Integer    'クリックボタン判別用
        Get
            Return _intSelectBtn
        End Get
        Set(ByVal value As Integer)
            _intSelectBtn = value
        End Set
    End Property

    Public Property MeetingInformation() As DataRow    '選択会議通知のデータリスト取得用
        Get
            Return _drMeetingInformation
        End Get
        Set(ByVal value As DataRow)
            _drMeetingInformation = value
        End Set
    End Property

    Public ReadOnly Property IntReturnBtnFlg() As Integer
        Get
            Return _intReturnBtn
        End Get
    End Property

#End Region

#End Region

#Region "イベント"
#Region "フォームロード"
    '***************************************************************************************************
    '   ＩＤ　：UC020302_Load
    '   名称　：フォームロード処理
    '   概要　：
    '   作成日：2011/11/18(金) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金) m.miyata  新規作成
    '***************************************************************************************************
    Private Sub UC020302_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim dtGrant As DataTable = Nothing
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '-------------------------------------------------------------------------------
            '   各データ取得処理
            '-------------------------------------------------------------------------------
            '支部、開催場所コンボボックス用にデータ取得
            SetBranch()

            ' コンボボックスデータ取得
            If GetData() = False Then
                Exit Sub
            End If

            dtGrant = getGrant(MENU_ID_UC020301)

            '権限取得
            If dtGrant.Rows.Count > 0 Then
                _strGrantReference = dtGrant.Rows(0).Item(3).ToString  '参照権限
                _strGrantInsert = dtGrant.Rows(0).Item(4).ToString     '登録権限
                _strGrantPrint = dtGrant.Rows(0).Item(5).ToString      '印刷権限
                _strGrantFileOutput = dtGrant.Rows(0).Item(6).ToString 'ファイル出力権限
            End If

            '初期処理を行う
            Initialize()

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "UC020302_Load")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "登録確認ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnConfirm_Click
    '   名称　：登録確認ボタンクリック
    '   概要　：
    '   作成日：2011/11/18(金) m.miyata
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金) m.miyata  新規作成
    '***************************************************************************************************
    Private Sub btnConfirm_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfirm.Click
        Dim strMeetingNumber As String = String.Empty

        Try
            Me.Cursor = Cursors.WaitCursor
            '同期処理による最新データの取得 SEQ対応によって前同期を省略 2013/04/19
            'syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)

            '一時保存の場合、同じ会議通知番号を持つ会議通知が更新できるかチェック
            If IntClickBtnFlg = 5 AndAlso _drMeetingInformation.Item("c_meeting") IsNot DBNull.Value Then
                If ChkNull(_drMeetingInformation.Item("c_meeting")) = False Then
                    If ChkMeetingFinish() = False Then
                        CLMsg.Show("GE0217", INFORMATION_TYPE_STOP_STRING)
                        Exit Sub
                    End If
                End If
            End If

            '入力チェック
            If chkInput() = False Then
                Exit Sub
            End If

            '日時1が過去日でないか
            If Me.dtpDate1.Value.Date < System.DateTime.Now.Date Then
                CLMsg.Show("GE0141", "日時1")
                Exit Sub
            End If

            '日時2が過去日でないか
            If Me.chkDate2.CheckState = CheckState.Checked AndAlso Me.dtpDate2.Value.Date < System.DateTime.Now.Date Then
                CLMsg.Show("GE0141", "日時2")
                Exit Sub
            End If

            '日時3が過去日でないか
            If Me.chkDate3.CheckState = CheckState.Checked AndAlso Me.dtpDate3.Value.Date < System.DateTime.Now.Date Then
                CLMsg.Show("GE0141", "日時3")
                Exit Sub
            End If

            '各開催日付が適切か
            If ChkDateExact() = False Then
                Exit Sub
            End If

            '入力時刻が適切か
            If ChkTime() = False Then
                Exit Sub
            End If

            '入力文字列チェック
            If chkStringLength() = False Then
                Exit Sub
            End If

            If IntClickBtnFlg = 3 Then
                '同一支部での同一会議通知存在チェック
                If chkSameBranchMeeting() = False Then
                    CLMsg.Show("GE0053")
                    Exit Sub
                End If
            End If

            '同一会議通知チェック
            If IntClickBtnFlg <> 1 AndAlso IntClickBtnFlg <> 2 Then
                strMeetingNumber = chkSameMeeting()

                If strMeetingNumber <> String.Empty Then
                    If CLMsg.Show("GW0013", strMeetingNumber) = DialogResult.No Then
                        Exit Sub
                    End If
                End If
            End If

            _intReturnBtn = 0

            '印刷プレビュー画面呼び出し
            showPrintPreview(False)

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "btnConfirm_Click")
            log.Fatal(ex.Message)
        Finally
            If Me.Cursor = Cursors.WaitCursor Then
                Me.Cursor = Cursors.Default
            End If
        End Try
    End Sub
#End Region

#Region "一時保存ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnIhozon_click
    '   名称　：一時保存ボタンクリック
    '   概要　：
    '   作成日：2011/11/24(木) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木) a.onuma  新規作成
    '***************************************************************************************************

    Private Sub btnIhozon_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIhozon.Click
        Try
            _intReturnBtn = 2
            '印刷プレビュー画面呼び出し
            showPrintPreview(True)
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "btnIhozon_click")
            log.Fatal(ex.Message)
        End Try
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
        Try
            If CLMsg.Show("GQ0007") = DialogResult.No Then
                'いいえが選択された場合は何も行わない
                Exit Sub
            End If

            _intReturnBtn = 1
            '画面終了処理
            If IntClickBtnFlg = 4 OrElse IntClickBtnFlg = 5 Then
                '会議詳細の場合、内容変更ボタン押下前に戻る
                ReturnFirst()
            Else
                'それ以外の場合は検索画面に戻る
                FormClose()
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "btnCancel_Click")
            log.Fatal(ex.Message)
        End Try

    End Sub
#End Region

#Region "プレ印刷ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnPprint_Click
    '   名称　：プレ印刷ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/29(火) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnPprint_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPprint.Click
        Me.Cursor = Cursors.WaitCursor

        '電話番号、Fax番号
        Dim telFaxList As TelFax = Nothing
        telFaxList = GetTelFaxNumber()
        If IntClickBtnFlg = 5 Then
            'データ処理結果
            Dim blnRet As Boolean = True
            Dim fm000203 As New FM000203
            Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
            Dim ds As DS0203P1 = New DS0203P1()

            'ボタン表示タイプを設定
            fm000203.ButtonShowType = 3
            fm000203.PrintCntVisible = False
            reportObj = New CR0203P1
            fm000203.ObjResource = reportObj

            Dim drMeetingNotice As DS0203P1.dtHeaderRow
            drMeetingNotice = ds.dtHeader.NewRow
            drMeetingNotice.BeginEdit()

            '社員番号
            drMeetingNotice.c_staf_id = UNDEFINE_SHAIN_CODE
            '機種
            drMeetingNotice.k_model = UNDEFINE_MACHINE_TYPE
            drMeetingNotice.l_name = UNDEFINE_SHAIN_NAME
            '共通情報をクリスタルレポートにセット
            SetReportInfo(drMeetingNotice, telFaxList)

            drMeetingNotice.EndEdit()
            ds.dtHeader.Rows.Add(drMeetingNotice)
            reportObj.SetDataSource(ds)

            Me.Cursor = Cursors.Default
            Call fm000203.ShowDialog()
            Select Case fm000203.IntQlickBtnFlag
                Case 2
                    'キャンセルの場合何も行わない
                Case 3
                    '印刷処理の実行
                    fm000203.PrintOut()
            End Select

        Else
            Dim fm As FM000204 = New FM000204()

            Me.Cursor = Cursors.Default
            Dim strMemberList As String() = CreateUnionMember()
            fm.StafIDList = strMemberList

            '組合員選択画面表示
            fm.ShowDialog()

            Select Case fm.IntQlickBtnFlag
                Case 0
                    Me.Cursor = Cursors.WaitCursor
                    '印刷処理実行
                    CreateCrystalReport(fm, True, telFaxList)
                    Me.Cursor = Cursors.Default
                Case 1
                    'キャンセルの場合何も行わない
            End Select
        End If

    End Sub
#End Region

#Region "内容変更ボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnChange_Click
    '   名称　：内容変更ボタンクリック処理
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnChange_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChange.Click
        '各ボタンの表示／非表示を変更
        '内容変更ボタン、戻るボタン、プレ印刷ボタンの非表示
        btnChange.Visible = False
        btnReturn.Visible = False
        btnPprint.Visible = False

        '登録確認ボタン、一時保存ボタン、キャンセルボタンの表示
        '一時保存ボタンは一時保存から表示されていた場合のみ使用可となる
        btnConfirm.Visible = True
        If IntClickBtnFlg = 5 Then
            Me.btnIhozon.Visible = True
        End If
        '部／委員会コンボボックスを使用可能にする
        'Me.CboCommittee.Enabled = True
        Call Utilities.SetCanEditToControl(True, Me.CboCommittee)
        Me.CboCommittee.BackColor = Color.White
        btnCancel.Visible = True

        Me.cboshibu.BackColor = Color.Cornsilk
        NotChangeData(True)
    End Sub
#End Region

#Region "戻るボタンクリック"
    '***************************************************************************************************
    '   ＩＤ　：btnReturn_Click
    '   名称　：戻るボタンボタンクリック処理
    '   概要　：
    '   作成日：2011/11/28(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/28(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub btnReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReturn.Click
        'クリックしたボタンのフラグセット
        _intReturnBtn = 3
        '画面終了処理
        FormClose()
    End Sub
#End Region

#Region "日時２チェックボックス値変更処理"
    '***************************************************************************************************
    '   ＩＤ　：chkDate2_CheckedChanged
    '   名称　：日時２チェックボックス値変更処理
    '   概要　：
    '   作成日：2011/11/21(月) a.onuma
    '   更新日：2012/07/03(火) k.shouji
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) a.onuma 新規作成
    '         ：2012/07/03(火) k.shouji チェックしていない時に日付＋１にするように変更
    '***************************************************************************************************
    Private Sub chkDate2_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate2.CheckedChanged

        If Me.chkDate2.CheckState = CheckState.Checked Then
            '日時２）にチェックされた場合
            Me.dtpDate2.Enabled = True
            'Me.dtpDate2.Value = Me.dtpDate1.Value.Date.AddDays(1)    '2012/07/03
            Me.mtxStartTime2.Enabled = True
            Me.mtxEndTime2.Enabled = True
        Else
            '日時２）のチェックが外れた場合
            Me.dtpDate2.Enabled = False
            Me.dtpDate2.Value = Me.dtpDate1.Value.Date.AddDays(1)     '2012/07/03
            Me.mtxStartTime2.Enabled = False
            Me.mtxEndTime2.Enabled = False
        End If
    End Sub
#End Region

#Region "日時３チェックボックス値変更処理"
    '***************************************************************************************************
    '   ＩＤ　：chkDate3_CheckedChanged
    '   名称　：日時３チェックボックス値変更処理
    '   概要　：
    '   作成日：2011/11/21(月) a.onuma
    '   更新日：
    '   備考  ：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/21(月) a.onuma 新規作成
    '         ：2012/07/03(火) k.shouji チェックしていない時に日付＋２にするように変更
    '***************************************************************************************************
    Private Sub chkDate3_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDate3.CheckedChanged

        If Me.chkDate3.CheckState = CheckState.Checked Then
            '日時３）にチェックされた場合
            Me.dtpDate3.Enabled = True
            'Me.dtpDate3.Value = Me.dtpDate1.Value.Date.AddDays(2)   '2012/07/03
            Me.mtxStartTime3.Enabled = True
            Me.mtxEndTime3.Enabled = True
        Else
            '日時３）のチェックが外れた場合
            Me.dtpDate3.Enabled = False
            Me.dtpDate3.Value = Me.dtpDate1.Value.Date.AddDays(2)    '2012/07/03
            Me.mtxStartTime3.Enabled = False
            Me.mtxEndTime3.Enabled = False
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
    '   作成日：2011/11/18(金) m.miyata
    '   更新日：2013/09/23(月) Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/18(金) m.miyata  新規作成
    ' 　　　　：2013/09/23(祝) Fujisaku　基準日取得対応
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
            ' 基準日取得（最新期：現在日、最新期以外：期末日）
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
            If MDCommon.CreateComboBoxNew(db, Me.CboCommittee, strSql, "l_name", "c_committee_id") = False Then
                Return False
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateCboCommittee")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "会議名コンボボックス作成処理"
    '***************************************************************************************************
    '   ＩＤ　：CreateCboKName
    '   名称　：会議名コンボボックス作成処理
    '   概要  ：会議名コンボボックスリストデータを作成する。
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/27(日) a.onuma
    '   更新日：2013/09/23(月) Fujisaku
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/27(日) a.onuma  新規作成
    ' 　　　　：2013/09/23(祝) Fujisaku　基準日取得対応
    '***************************************************************************************************
    ''' <summary>会議名コンボボックス作成処理</summary>
    ''' <returns>True = 正常, False = 異常</returns>
    ''' <remarks></remarks>
    Private Function CreateCboKName(ByVal db As CLAccessMdb) As Boolean
        Dim blnRet As Boolean = False       '処理結果
        Dim strSql As String = ""           'SQL文
        Dim comStr As String                '管理部ユーザー参照できる委員会
        'Dim d_from As String                '期の開始日
        Dim dt As DataTable = Nothing       'データテーブル
        Dim dtRow As DataRow = Nothing      '一行のデータ

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            ' 会議名コンボボックスクリア
            Me.cboMeetingName.Items.Clear()

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
            ' 基準日取得（最新期：現在日、最新期以外：期末日）
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
            strSql = strSql & " order by c_committee_id"            ' コンボボックス作成処理
            If MDCommon.CreateComboBoxNew(db, Me.cboMeetingName, strSql, "l_name", "c_committee_id", , MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWN) = False Then
                Return False
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateCbo")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
#End Region

#Region "会議場所コンボボックス作成処理"
    '***************************************************************************************************
    '   ＩＤ　：CreateCboKBasho
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
    Private Function CreateCboKBasho() As Boolean

        Dim blnRet As Boolean = False       ' 処理結果

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            ' 会議場所コンボボックスクリア
            Me.cboMeetingPlace.Items.Clear()

            '' コンボボックス作成処理
            'If CreateCboConstantDtl(db, Me.cboMeetingPlace, MDConst.CONSTANT_ID_UI_SHIBU, , MDConst.COMBO_STYLE_DROPDOWNLIST_DROPDOWN) = False Then
            '    Return False
            'End If
            SetComboMeetingPlace()

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateCbo")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        ' 戻り値格納
        Return blnRet

    End Function
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

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            ' データベース接続
            db.Connect()

            '---------------------------------------------------------------------------
            '   コンボボックス作成
            '---------------------------------------------------------------------------
            ' 支部コンボボックス作成処理呼び出し
            'If CreateCboConstantDtl(db, Me.cboshibu, MDConst.CONSTANT_ID_UI_SHIBU) = False Then
            '    Return blnRet
            'End If
            SetComboBranch()

            ' 部/委員会コンボボックス作成処理呼び出し
            If CreateCboCommittee(db) = False Then
                Return blnRet
            End If

            ' 会議名コンボボックス作成処理呼び出し
            If CreateCboKName(db) = False Then
                Return blnRet
            End If

            ' 開催場所コンボボックス作成処理呼び出し
            If CreateCboKBasho() = False Then
                Return blnRet
            End If

            ' 処理結果に正常を格納
            blnRet = True

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "GetData")
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

#Region "初期処理"
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

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        If IntClickBtnFlg = 0 Then
            '登録日にシステム日付を表示する
            Me.txtInsertDate.Text = System.DateTime.Now().Date
            Me.dtpDate2.Value = Me.dtpDate1.Value.Date().AddDays(1)
            Me.dtpDate3.Value = Me.dtpDate2.Value.Date().AddDays(1)
            '種類の値を変更できないようにする
            'optOpen.Enabled = False
            'optChange.Enabled = False
            'optStop.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.optOpen)
            Call Utilities.SetCanEditToControl(False, Me.optChange)
            Call Utilities.SetCanEditToControl(False, Me.optStop)
            '種類グループボックスの背景色を変更する
            GroupBox2.BackColor = Color.Cornsilk

        ElseIf IntClickBtnFlg = 1 OrElse IntClickBtnFlg = 2 Then '変更または中止の場合
            showLatestMeetingNotice()
            '各種オブジェクトの制限をかける
            InitializeObjectControl()
        Else
            '登録日
            If IntClickBtnFlg <> 3 AndAlso _drMeetingInformation.Item("d_ins").Equals(DBNull.Value) = False Then
                Me.txtInsertDate.Text = _drMeetingInformation.Item("d_ins")
            Else
                '合同登録の場合、登録日が存在しない場合本日の日付を表示
                Me.txtInsertDate.Text = System.DateTime.Now().Date
            End If

            '会議通知番号
            If IntClickBtnFlg = 3 Then
                Me.txtMeetingNumber.Text = "*****"
            Else
                If _drMeetingInformation.Item("c_meeting").Equals(DBNull.Value) = False AndAlso _
                   ChkNull(_drMeetingInformation.Item("c_meeting")) = False Then
                    Me.txtMeetingNumber.Text = _drMeetingInformation.Item("c_meeting")
                Else
                    Me.txtMeetingNumber.Text = "*****"
                End If
            End If

            '支部
            If _drMeetingInformation.Item("k_apply_area").Equals(DBNull.Value) = False Then
                If _dicBranch.ContainsValue(_drMeetingInformation.Item("k_apply_area")) Then
                    Dim iCnt As Integer = 0
                    For Each strName As String In _dicBranch.Values
                        If strName = _drMeetingInformation.Item("k_apply_area") Then
                            Me.cboshibu.Text = _dicBranch.Keys(iCnt)
                        End If
                        iCnt = iCnt + 1
                    Next
                End If
            End If

            If _drMeetingInformation.Item("k_union") = UNION_ON Then
                '合同区分
                Me.chkUnion.CheckState = CheckState.Checked
            End If

            '種類
            If _drMeetingInformation.Item("k_information_type") = INFORMATION_TYPE_OPEN Then
                Me.optOpen.Checked = True
            ElseIf _drMeetingInformation.Item("k_information_type") = INFORMATION_TYPE_CHANGE Then
                Me.optChange.Checked = True
            ElseIf _drMeetingInformation.Item("k_information_type") = INFORMATION_TYPE_STOP Then
                Me.optStop.Checked = True
            End If
            '部／委員会
            If _drMeetingInformation.Item("c_committee_id").Equals(DBNull.Value) = False Then
                Me.CboCommittee.SelectedValue() = _drMeetingInformation.Item("c_committee_id")
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
            '日時1
            If _drMeetingInformation.Item("d_meeting_1").Equals(DBNull.Value) = False Then
                Me.dtpDate1.Value = _drMeetingInformation.Item("d_meeting_1")
            End If
            '開始時間1
            If _drMeetingInformation.Item("d_meeting_time_from_1").Equals(DBNull.Value) = False Then
                Me.mtxStartTime1.Text = _drMeetingInformation.Item("d_meeting_time_from_1")
            End If
            '終了時間1
            If _drMeetingInformation.Item("d_meeting_time_to_1").Equals(DBNull.Value) = False Then
                Me.mtxEndTime1.Text = _drMeetingInformation.Item("d_meeting_time_to_1")
            End If
            '日時2
            If _drMeetingInformation.Item("d_meeting_2").Equals(DBNull.Value) = False Then
                Me.dtpDate2.Value = _drMeetingInformation.Item("d_meeting_2")
                chkDate2.CheckState = CheckState.Checked

                '開始時間2
                If _drMeetingInformation.Item("d_meeting_time_from_2").Equals(DBNull.Value) = False Then
                    Me.mtxStartTime2.Text = _drMeetingInformation.Item("d_meeting_time_from_2")
                End If
                '終了時間2
                If _drMeetingInformation.Item("d_meeting_time_to_2").Equals(DBNull.Value) = False Then
                    Me.mtxEndTime2.Text = _drMeetingInformation.Item("d_meeting_time_to_2")
                End If
            Else
                Me.dtpDate2.Value = Me.dtpDate1.Value.Date().AddDays(1)
            End If
            '日時3
            If _drMeetingInformation.Item("d_meeting_3").Equals(DBNull.Value) = False Then
                Me.dtpDate3.Value = _drMeetingInformation.Item("d_meeting_3")
                chkDate3.CheckState = CheckState.Checked
                '開始時間3
                If _drMeetingInformation.Item("d_meeting_time_from_3").Equals(DBNull.Value) = False Then
                    Me.mtxStartTime3.Text = _drMeetingInformation.Item("d_meeting_time_from_3")
                End If
                '終了時間3
                If _drMeetingInformation.Item("d_meeting_time_to_3").Equals(DBNull.Value) = False Then
                    Me.mtxEndTime3.Text = _drMeetingInformation.Item("d_meeting_time_to_3")
                End If
            Else
                Me.dtpDate3.Value = Me.dtpDate2.Value.Date().AddDays(1)
            End If
            '往路
            If _drMeetingInformation.Item("l_flight_to_1").Equals(DBNull.Value) = False Then
                Me.txtGoMachineName.Text = _drMeetingInformation.Item("l_flight_to_1")
                '往路時間   
                If _drMeetingInformation.Item("d_flight_to_1").Equals(DBNull.Value) = False Then
                    If ChkNull(Me.txtGoMachineName.Text) = False Then
                        Me.mtxGoTime.Text = _drMeetingInformation.Item("d_flight_to_1")
                    End If
                End If
            End If
            '復路
            If _drMeetingInformation.Item("l_flight_back_1").Equals(DBNull.Value) = False Then
                Me.txtReturnMachineName.Text = _drMeetingInformation.Item("l_flight_back_1")
                '復路時間
                If _drMeetingInformation.Item("d_flight_back_1").Equals(DBNull.Value) = False Then
                    If ChkNull(Me.txtReturnMachineName.Text) = False Then
                        Me.mtxReturnTime.Text = _drMeetingInformation.Item("d_flight_back_1")
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

            '各種オブジェクトの制限をかける
            InitializeObjectControl()
        End If

        '登録日を読み取り専用にする
        txtInsertDate.ReadOnly = True
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
    End Sub
#End Region

#Region "押下ボタンごとのコントロール制御"
    '***************************************************************************************************
    '   ＩＤ　：InitializeObjectControl
    '   名称　：押下ボタンごとのコントロール制御
    '   概要  ：押下されたボタンに応じてコントロールを制御する
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/29(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub InitializeObjectControl()
        If IntClickBtnFlg <> 3 Then
            '支部コンボボックスのバックカラー変更
            'Me.cboshibu.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.cboshibu)
            Me.cboshibu.BackColor = Color.Cornsilk
        End If

        If IntClickBtnFlg = 1 Then
            Me.lblTittle.Text = "会議通知 - 変更"
            Me.optChange.Checked = True
            InitializeLimitControl()

        ElseIf IntClickBtnFlg = 2 Then
            Me.lblTittle.Text = "会議通知 - 中止"
            Me.optStop.Checked = True
            InitializeLimitControl()

        ElseIf IntClickBtnFlg = 3 Then
            Me.lblTittle.Text = "会議通知 - 合同登録"
            'Me.chkUnion.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.chkUnion)
            InitializeLimitControl()

            '一時保存ボタン使用不可
            'Me.btnIhozon.Visible = False
            '会議名コンボボックスのバックカラー変更
            'Me.cboMeetingName.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.cboMeetingName)
            Me.cboMeetingName.BackColor = Color.Cornsilk
            '開催場所コンボボックスのバックカラー変更
            'Me.cboMeetingPlace.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.cboMeetingPlace)
            Me.cboMeetingPlace.BackColor = Color.Cornsilk

        ElseIf IntClickBtnFlg = 4 Then
            Me.lblTittle.Text = "会議通知 - 会議詳細"
            InitializeLimitControl()
            ReferenceButtonIni()

            If Me.dtpDate1.Value.Date() < System.DateTime.Now().Date() Then
                '内容変更ボタンを使用不可にする
                Me.btnChange.Visible = False
            End If
            NotChangeData(False)

        ElseIf IntClickBtnFlg = 5 Then
            Me.lblTittle.Text = "会議通知 - 会議詳細"
            InitializeLimitControl()
            ReferenceButtonIni()
            '"※一時保存文書"を表示する
            Me.lblTemporary.Visible = True
            '画面のバックカラーを変更
            Me.GroupBox1.BackColor = Color.PapayaWhip
            Me.txtMeetingNumber.BackColor = Color.PapayaWhip
            Me.txtInsertDate.BackColor = Color.PapayaWhip
            NotChangeData(False)
        End If

    End Sub
#End Region

#Region "一部の機能で共通して変更不可のコントロール制御"
    '***************************************************************************************************
    '   ＩＤ　：InitializeLimitControl
    '   名称　：一部の機能で共通して変更不可のコントロール制御
    '   概要  ：一部の機能で共通して変更不可のコントロール制御
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/29(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub InitializeLimitControl()
        '種類の値を変更できないようにする
        'optOpen.Enabled = False
        'optChange.Enabled = False
        'optStop.Enabled = False
        Call Utilities.SetCanEditToControl(False, Me.optOpen)
        Call Utilities.SetCanEditToControl(False, Me.optChange)
        Call Utilities.SetCanEditToControl(False, Me.optStop)
        '種類グループボックスの背景色を変更する
        GroupBox2.BackColor = Color.Cornsilk
        '部／委員会コンボボックスのバックカラー変更
        'Me.CboCommittee.Enabled = False
        Call Utilities.SetCanEditToControl(False, Me.CboCommittee)
        Me.CboCommittee.BackColor = Color.Cornsilk

    End Sub
#End Region

#Region "会議詳細選択時のボタン制御"
    '***************************************************************************************************
    '   ＩＤ　：ReferenceButtonIni
    '   名称　：会議詳細選択時のボタン制御
    '   概要  ：会議詳細選択時のボタン制御
    '   引数　：なし
    '   戻り値：なし
    '   作成日：2011/11/29(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub ReferenceButtonIni()
        '印刷ボタン使用可
        Me.btnPprint.Visible = True
        If _strGrantPrint <> "1" Then
            '印刷権限がない場合
            Me.btnPprint.Enabled = False
        End If
        '内容変更ボタン使用可
        Me.btnChange.Visible = True

        If _strGrantInsert <> "1" Then
            '登録権限がない場合
            Me.btnChange.Enabled = False
        End If

        '戻るボタン使用可
        Me.btnReturn.Visible = True
        '内容確認ボタン使用不可
        Me.btnConfirm.Visible = False
        'キャンセルボタン使用不可
        Me.btnCancel.Visible = False
        '一時保存ボタン使用不可
        Me.btnIhozon.Visible = False
    End Sub
#End Region

#Region "同一会議通知番号内の最新を表示"
    Private Sub showLatestMeetingNotice()
        Dim dtRet As DataTable = Nothing

        dtRet = GetLatestMeetingNotice()
        Try
            If dtRet.Rows.Count > 0 Then
                '登録日
                If dtRet.Rows(0).Item("d_ins").Equals(DBNull.Value) = False Then
                    Me.txtInsertDate.Text = dtRet.Rows(0).Item("d_ins")
                Else
                    '合同登録の場合、登録日が存在しない場合本日の日付を表示
                    Me.txtInsertDate.Text = System.DateTime.Now().Date
                End If

                '会議通知番号
                If dtRet.Rows(0).Item("c_meeting").Equals(DBNull.Value) = False AndAlso _
                       ChkNull(dtRet.Rows(0).Item("c_meeting")) = False Then
                    Me.txtMeetingNumber.Text = dtRet.Rows(0).Item("c_meeting")
                Else
                    Me.txtMeetingNumber.Text = "*****"
                End If

                '支部
                If dtRet.Rows(0).Item("k_apply_area").Equals(DBNull.Value) = False Then
                    If _dicBranch.ContainsValue(dtRet.Rows(0).Item("k_apply_area")) Then
                        Dim iCnt As Integer = 0
                        For Each strName As String In _dicBranch.Values
                            If strName = dtRet.Rows(0).Item("k_apply_area") Then
                                Me.cboshibu.Text = _dicBranch.Keys(iCnt)
                                Exit For
                            End If
                            iCnt = iCnt + 1
                        Next
                    End If
                End If
               
                If dtRet.Rows(0).Item("k_union") = UNION_ON Then
                    '合同区分
                    Me.chkUnion.CheckState = CheckState.Checked
                End If

                '種類
                If IntClickBtnFlg = 1 Then
                    Me.optChange.Checked = True
                ElseIf IntClickBtnFlg = 2 Then
                    Me.optStop.Checked = True
                Else
                    If dtRet.Rows(0).Item("k_information_type") = INFORMATION_TYPE_OPEN Then
                        Me.optOpen.Checked = True
                    ElseIf dtRet.Rows(0).Item("k_information_type") = INFORMATION_TYPE_CHANGE Then
                        Me.optChange.Checked = True
                    ElseIf dtRet.Rows(0).Item("k_information_type") = INFORMATION_TYPE_STOP Then
                        Me.optStop.Checked = True
                    End If
                End If
                '部／委員会
                If dtRet.Rows(0).Item("c_committee_id").Equals(DBNull.Value) = False Then
                    Me.CboCommittee.SelectedValue() = dtRet.Rows(0).Item("c_committee_id")
                End If
                '会議名
                If dtRet.Rows(0).Item("l_information_name").Equals(DBNull.Value) = False Then
                    Me.cboMeetingName.Text = dtRet.Rows(0).Item("l_information_name")
                End If
                '開催場所
                If dtRet.Rows(0).Item("l_open_bebiginting").Equals(DBNull.Value) = False Then
                    Me.cboMeetingPlace.Text = dtRet.Rows(0).Item("l_open_bebiginting")
                End If
                '会議場
                If dtRet.Rows(0).Item("l_place").Equals(DBNull.Value) = False Then
                    Me.txtMeetingPlace.Text = dtRet.Rows(0).Item("l_place")
                End If
                '日時1
                If dtRet.Rows(0).Item("d_meeting_1").Equals(DBNull.Value) = False Then
                    Me.dtpDate1.Value = dtRet.Rows(0).Item("d_meeting_1")
                End If
                '開始時間1
                If dtRet.Rows(0).Item("d_meeting_time_from_1").Equals(DBNull.Value) = False Then
                    Me.mtxStartTime1.Text = dtRet.Rows(0).Item("d_meeting_time_from_1")
                End If
                '終了時間1
                If dtRet.Rows(0).Item("d_meeting_time_to_1").Equals(DBNull.Value) = False Then
                    Me.mtxEndTime1.Text = dtRet.Rows(0).Item("d_meeting_time_to_1")
                End If
                '日時2
                If dtRet.Rows(0).Item("d_meeting_2").Equals(DBNull.Value) = False Then
                    Me.dtpDate2.Value = dtRet.Rows(0).Item("d_meeting_2")
                    chkDate2.CheckState = CheckState.Checked

                    '開始時間2
                    If dtRet.Rows(0).Item("d_meeting_time_from_2").Equals(DBNull.Value) = False Then
                        Me.mtxStartTime2.Text = dtRet.Rows(0).Item("d_meeting_time_from_2")
                    End If
                    '終了時間2
                    If dtRet.Rows(0).Item("d_meeting_time_to_2").Equals(DBNull.Value) = False Then
                        Me.mtxEndTime2.Text = dtRet.Rows(0).Item("d_meeting_time_to_2")
                    End If
                Else
                    Me.dtpDate2.Value = Me.dtpDate1.Value.Date().AddDays(1)
                End If
                '日時3
                If dtRet.Rows(0).Item("d_meeting_3").Equals(DBNull.Value) = False Then
                    Me.dtpDate3.Value = dtRet.Rows(0).Item("d_meeting_3")
                    chkDate3.CheckState = CheckState.Checked
                    '開始時間3
                    If dtRet.Rows(0).Item("d_meeting_time_from_3").Equals(DBNull.Value) = False Then
                        Me.mtxStartTime3.Text = dtRet.Rows(0).Item("d_meeting_time_from_3")
                    End If
                    '終了時間3
                    If dtRet.Rows(0).Item("d_meeting_time_to_3").Equals(DBNull.Value) = False Then
                        Me.mtxEndTime3.Text = dtRet.Rows(0).Item("d_meeting_time_to_3")
                    End If
                Else
                    Me.dtpDate3.Value = Me.dtpDate2.Value.Date().AddDays(1)
                End If
                '往路
                If dtRet.Rows(0).Item("l_flight_to_1").Equals(DBNull.Value) = False Then
                    Me.txtGoMachineName.Text = dtRet.Rows(0).Item("l_flight_to_1")
                    '往路時間   
                    If dtRet.Rows(0).Item("d_flight_to_1").Equals(DBNull.Value) = False Then
                        If ChkNull(Me.txtGoMachineName.Text) = False Then
                            Me.mtxGoTime.Text = dtRet.Rows(0).Item("d_flight_to_1")
                        End If
                    End If
                End If
                '復路
                If dtRet.Rows(0).Item("l_flight_back_1").Equals(DBNull.Value) = False Then
                    Me.txtReturnMachineName.Text = dtRet.Rows(0).Item("l_flight_back_1")
                    '復路時間
                    If dtRet.Rows(0).Item("d_flight_back_1").Equals(DBNull.Value) = False Then
                        If ChkNull(Me.txtReturnMachineName.Text) = False Then
                            Me.mtxReturnTime.Text = dtRet.Rows(0).Item("d_flight_back_1")
                        End If
                    End If
                End If
                '備考
                If dtRet.Rows(0).Item("l_biko_1").Equals(DBNull.Value) = False Then
                    Me.txtNote.Text = dtRet.Rows(0).Item("l_biko_1")
                End If
                If dtRet.Rows(0).Item("l_biko_2").Equals(DBNull.Value) = False Then
                    Me.txtNote.Text = Me.txtNote.Text & vbCrLf & dtRet.Rows(0).Item("l_biko_2")
                End If
                If dtRet.Rows(0).Item("l_biko_3").Equals(DBNull.Value) = False Then
                    Me.txtNote.Text = Me.txtNote.Text & vbCrLf & dtRet.Rows(0).Item("l_biko_3")
                End If
                '議題1～5
                If dtRet.Rows(0).Item("l_subject_1").Equals(DBNull.Value) = False Then
                    Me.txtTheme1.Text = dtRet.Rows(0).Item("l_subject_1")
                End If
                If dtRet.Rows(0).Item("l_subject_2").Equals(DBNull.Value) = False Then
                    Me.txtTheme2.Text = dtRet.Rows(0).Item("l_subject_2")
                End If
                If dtRet.Rows(0).Item("l_subject_3").Equals(DBNull.Value) = False Then
                    Me.txtTheme3.Text = dtRet.Rows(0).Item("l_subject_3")
                End If
                If dtRet.Rows(0).Item("l_subject_4").Equals(DBNull.Value) = False Then
                    Me.txtTheme4.Text = dtRet.Rows(0).Item("l_subject_4")
                End If
                If dtRet.Rows(0).Item("l_subject_5").Equals(DBNull.Value) = False Then
                    Me.txtTheme5.Text = dtRet.Rows(0).Item("l_subject_5")
                End If

            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "showlatestMeetingNotice")
            log.Fatal(ex.Message)
        End Try
    End Sub

#End Region

#Region "同一会議番号内の最新の会議通知を取得"
    '***************************************************************************************************
    '   ＩＤ　：GetLatestMeetingNotice
    '   名称　：同一会議番号内の最新の会議通知を取得
    '   概要  ：同一会議通知番号のうち最新の会議通知を取得し返却する
    '   引数　：なし
    '   戻り値：SQL実行結果
    '   作成日：2011/12/22(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/22(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetLatestMeetingNotice() As DataTable
        Dim dtRet As DataTable = Nothing
        Dim clsMDb As CLAccessMdb = New CLAccessMdb
        Dim strSql As String = String.Empty

        Try
            strSql = ""
            strSql = "SELECT * FROM meeting_information " & vbCrLf
            strSql = strSql & "WHERE c_meeting = '" & _drMeetingInformation.Item("c_meeting") & "' " & vbCrLf
            strSql = strSql & " AND k_apply_area = '" & _drMeetingInformation.Item("k_apply_area") & "' " & vbCrLf
            strSql = strSql & " AND s_meeting = " & vbCrLf
            strSql = strSql & " (SELECT MAX(s_meeting) As MaxSeq FROM meeting_information " & vbCrLf
            strSql = strSql & "WHERE c_meeting = '" & _drMeetingInformation.Item("c_meeting") & "' " & vbCrLf
            strSql = strSql & "AND k_apply_area = '" & _drMeetingInformation.Item("k_apply_area") & "')" & vbCrLf

            clsMDb.Connect()
            dtRet = clsMDb.ExecuteSql(strSql)

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "GetLatestMeetingNotice")
            log.Fatal(ex.Message)
        Finally
            clsMDb.Disconnect()
        End Try

        Return dtRet
    End Function
#End Region

#Region "印刷プレビュー画面表示"
    '***************************************************************************************************
    '   ＩＤ　：showPrintPreview
    '   名称　：印刷プレビュー画面表示
    '   概要  ：各入力情報より、印刷プレビュー画面を表示する
    '   引数　：blnWork:一時保存の場合true、本登録の場合false
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub showPrintPreview(ByVal blnWork As Boolean)
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        'データ登録用データ
        Dim data As miData = Nothing
        'データ処理結果
        Dim blnRet As Boolean = True
        Dim fm As New FM000203
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0203P1 = New DS0203P1()
        '電話番号、Fax番号
        Dim telFaxList As TelFax = Nothing
        '印刷レポート表示用のシーケンス番号
        Dim strSeq As String = String.Empty

        Try
            Me.Cursor = Cursors.WaitCursor
            telFaxList = GetTelFaxNumber()

            'ボタン表示タイプを設定
            fm.ButtonShowType = 1
            fm.PrintCntVisible = False
            reportObj = New CR0203P1
            fm.ObjResource = reportObj

            Dim drMeetingNotice As DS0203P1.dtHeaderRow
            drMeetingNotice = ds.dtHeader.NewRow
            drMeetingNotice.BeginEdit()

            '社員番号
            drMeetingNotice.c_staf_id = UNDEFINE_SHAIN_CODE
            '機種
            drMeetingNotice.k_model = UNDEFINE_MACHINE_TYPE
            drMeetingNotice.l_name = UNDEFINE_SHAIN_NAME
            '共通情報をクリスタルレポートにセット
            SetReportInfo(drMeetingNotice, telFaxList)

            drMeetingNotice.EndEdit()
            ds.dtHeader.Rows.Add(drMeetingNotice)
            reportObj.SetDataSource(ds)

            'カーソルを元に戻す
            'Me.Cursor = Cursors.Default
            Call fm.ShowDialog()

            Me.Cursor = Cursors.WaitCursor
            Select Case fm.IntQlickBtnFlag
                Case 0, 1
                    '登録＆印刷
                    If blnWork = False Then
                        '本登録処理
                        If IntClickBtnFlg <> 4 AndAlso IntClickBtnFlg <> 5 Then '新規、変更、中止、合同登録いずれか
                            data = CreateInsertData(blnWork)
                            '新規会議通知登録処理
                            blnRet = InsertData(data)
                            If IntClickBtnFlg = 0 OrElse IntClickBtnFlg = 3 Then
                                '新規または合同登録の場合新しい会議通知番号で印刷する
                                strSeq = data.strMeetingNumber.Replace(MDLoginInfo.Period.ToString & "-", "")
                            End If

                        ElseIf IntClickBtnFlg = 4 Then
                            '会議通知-会議詳細
                            data = CreateUpdateData(blnWork)
                            '会議通知更新処理
                            blnRet = UpdateData(data)
                        ElseIf IntClickBtnFlg = 5 Then '一時保存会議通知-会議詳細
                            '通知番号が発行済みかつ開催の一時保存データの場合、同一支部の同一通知番号の
                            'データを上書きする
                            If Me.txtMeetingNumber.Text.IndexOf("*") = -1 AndAlso Me.optOpen.Checked = True Then
                                data = CreateUpdateData(blnWork)
                                '会議通知更新処理
                                blnRet = UpdateData(data)
                            Else
                                data = CreateInsertData(blnWork)
                                '本登録処理
                                blnRet = InsertData(data)
                                '本登録後、元となった一時保存の削除
                                blnRet = DeleteTemporaryData()
                            End If
                        End If
                    Else
                        '一時保存処理
                        If IntClickBtnFlg <> 5 Then
                            '新規一時保存登録処理
                            data = CreateInsertData(True)
                            blnRet = InsertTemporaryData(data)
                        Else
                            '一時保存の詳細画面から一時保存ボタンが押下された場合、
                            '更新処理を行う
                            data = CreateUpdateData(True)
                            blnRet = UpdateTemporaryData(data)
                        End If

                    End If

                    If blnRet = True Then   '異常なくデータが登録できた場合
                        If fm.IntQlickBtnFlag = 0 Then
                            If blnWork = False Then
                                '登録処理呼び出し後、組合員抽出画面呼び出し
                                Dim fm000204 As FM000204 = New FM000204()
                                '組合員情報取得
                                Dim strMemberList As String() = CreateUnionMember()
                                fm000204.StafIDList = strMemberList
                                fm000204.ShowDialog()

                                If fm000204.IntQlickBtnFlag = 1 Then
                                    'キャンセルボタンが押下された場合その旨を通知
                                    CLMsg.Show("GI0019")
                                ElseIf fm000204.IntQlickBtnFlag = 0 Then
                                    If strSeq = String.Empty Then
                                        '選択された組合員分のレポート作成&印刷
                                        CreateCrystalReport(fm000204, False, telFaxList)
                                    Else
                                        CreateCrystalReport(fm000204, False, telFaxList, strSeq)
                                    End If
                                    '登録処理完了メッセージ 
                                    CLMsg.Show("GI0015")
                                End If
                            Else
                                '一時保存の場合はそのまま印刷処理へ
                                fm.PrintOut()
                            End If
                        Else
                            '登録処理完了メッセージ 
                            CLMsg.Show("GI0015")
                        End If

                        '同期処理による最新データの反映
                        ' syncMdb(JRO.SyncTypeEnum.jrSyncTypeImpExp, False)

                    Else    'データ登録が異常終了した場合
                        '"正しくデータが更新できませんでした。"
                        CLMsg.Show("DE0005")
                    End If

                    '会議通知検索画面を閉じる
                    FormClose()
                Case 2
                    'キャンセルの場合特に何も行わない
                    CLMsg.Show("BI0001")
            End Select

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "showPrintPreview")
            log.Fatal(ex.Message)
        Finally
            'カーソルを元に戻す
            Me.Cursor = Cursors.Default
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "会議通知登録画面終了処理"
    '***************************************************************************************************
    '   ＩＤ　：FormClose
    '   名称　：会議通知登録画面終了処理
    '   概要  ：会議通知登録画面を閉じて会議通知検索画面に戻ります
    '   引数　：
    '   戻り値：なし
    '   作成日：2011/12/08(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/08(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub FormClose()
        Dim pn As Panel
        Dim uc As Control
        Dim clsUC020301 As UC020301

        Me.Visible = False

        pn = ParentForm.Controls(MDConst.MAIN_PANEL_ID)
        uc = pn.Controls(SCREEN_ID_UC020301)

        If uc Is Nothing Then
            uc = New UC020301

            Call pn.Controls.Add(uc)
        Else
            clsUC020301 = pn.Controls(SCREEN_ID_UC020301)
            'クリックされたボタンのフラグを取得
            clsUC020301._intReturnBtnFlg = _intReturnBtn
            uc.Visible = True
        End If
        Me.Dispose()

    End Sub
#End Region

#Region "画面の初期化（内容変更前）"
    Private Sub ReturnFirst()

        '部／委員会
        If _drMeetingInformation.Item("c_committee_id").Equals(DBNull.Value) = False Then
            Me.CboCommittee.SelectedValue() = _drMeetingInformation.Item("c_committee_id")
        Else
            Me.CboCommittee.SelectedIndex = 0
        End If
        '会議名
        If _drMeetingInformation.Item("l_information_name").Equals(DBNull.Value) = False Then
            Me.cboMeetingName.Text = _drMeetingInformation.Item("l_information_name")
        Else
            Me.cboMeetingName.Text = String.Empty
        End If
        '開催場所
        If _drMeetingInformation.Item("l_open_bebiginting").Equals(DBNull.Value) = False Then
            Me.cboMeetingPlace.Text = _drMeetingInformation.Item("l_open_bebiginting")
        Else
            Me.cboMeetingPlace.Text = String.Empty
        End If
        '会議場
        If _drMeetingInformation.Item("l_place").Equals(DBNull.Value) = False Then
            Me.txtMeetingPlace.Text = _drMeetingInformation.Item("l_place")
        Else
            Me.txtMeetingPlace.Text = String.Empty
        End If
        '日時1
        If _drMeetingInformation.Item("d_meeting_1").Equals(DBNull.Value) = False Then
            Me.dtpDate1.Value = _drMeetingInformation.Item("d_meeting_1")
        Else
            Me.dtpDate1.Value = System.DateTime.Now().Date
        End If
        '開始時間1
        If _drMeetingInformation.Item("d_meeting_time_from_1").Equals(DBNull.Value) = False Then
            Me.mtxStartTime1.Text = _drMeetingInformation.Item("d_meeting_time_from_1")
        Else
            Me.mtxStartTime1.Text = STR_FIRST_TIME
        End If
        '終了時間1
        If _drMeetingInformation.Item("d_meeting_time_to_1").Equals(DBNull.Value) = False Then
            Me.mtxEndTime1.Text = _drMeetingInformation.Item("d_meeting_time_to_1")
        Else
            Me.mtxEndTime1.Text = STR_FIRST_TIME
        End If
        '日時2
        If _drMeetingInformation.Item("d_meeting_2").Equals(DBNull.Value) = False Then
            Me.dtpDate2.Value = _drMeetingInformation.Item("d_meeting_2")
            chkDate2.CheckState = CheckState.Checked

            '開始時間2
            If _drMeetingInformation.Item("d_meeting_time_from_2").Equals(DBNull.Value) = False Then
                Me.mtxStartTime2.Text = _drMeetingInformation.Item("d_meeting_time_from_2")
            End If
            '終了時間2
            If _drMeetingInformation.Item("d_meeting_time_to_2").Equals(DBNull.Value) = False Then
                Me.mtxEndTime2.Text = _drMeetingInformation.Item("d_meeting_time_to_2")
            End If

        Else
            Me.dtpDate2.Value = Me.dtpDate1.Value.Date().AddDays(1)
            chkDate2.CheckState = CheckState.Unchecked
            Me.mtxStartTime2.Text = STR_FIRST_TIME
            Me.mtxEndTime2.Text = STR_FIRST_TIME
        End If
        '日時3
        If _drMeetingInformation.Item("d_meeting_3").Equals(DBNull.Value) = False Then
            Me.dtpDate3.Value = _drMeetingInformation.Item("d_meeting_3")
            chkDate3.CheckState = CheckState.Checked

            '開始時間3
            If _drMeetingInformation.Item("d_meeting_time_from_3").Equals(DBNull.Value) = False Then
                Me.mtxStartTime3.Text = _drMeetingInformation.Item("d_meeting_time_from_3")
            End If
            '終了時間3
            If _drMeetingInformation.Item("d_meeting_time_to_3").Equals(DBNull.Value) = False Then
                Me.mtxEndTime3.Text = _drMeetingInformation.Item("d_meeting_time_to_3")
            End If
        Else
            Me.dtpDate3.Value = Me.dtpDate2.Value.Date().AddDays(1)
            chkDate3.CheckState = CheckState.Unchecked
            Me.mtxStartTime3.Text = STR_FIRST_TIME
            Me.mtxEndTime3.Text = STR_FIRST_TIME
        End If
        '往路
        If _drMeetingInformation.Item("l_flight_to_1").Equals(DBNull.Value) = False Then
            Me.txtGoMachineName.Text = _drMeetingInformation.Item("l_flight_to_1")
            '往路時間   
            If _drMeetingInformation.Item("d_flight_to_1").Equals(DBNull.Value) = False Then
                If ChkNull(Me.txtGoMachineName.Text) = False Then
                    Me.mtxGoTime.Text = _drMeetingInformation.Item("d_flight_to_1")
                End If
            End If
        Else
            Me.txtGoMachineName.Text = String.Empty
            Me.mtxGoTime.Text = STR_FIRST_TIME
        End If
        '復路
        If _drMeetingInformation.Item("l_flight_back_1").Equals(DBNull.Value) = False Then
            Me.txtReturnMachineName.Text = _drMeetingInformation.Item("l_flight_back_1")
            '復路時間
            If _drMeetingInformation.Item("d_flight_back_1").Equals(DBNull.Value) = False Then
                If ChkNull(Me.txtReturnMachineName.Text) = False Then
                    Me.mtxReturnTime.Text = _drMeetingInformation.Item("d_flight_back_1")
                End If
            End If
        Else
            Me.txtReturnMachineName.Text = String.Empty
            Me.mtxReturnTime.Text = STR_FIRST_TIME
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

        Me.btnIhozon.Visible = False
        Me.btnConfirm.Visible = False
        Me.btnCancel.Visible = False

        Me.btnPprint.Visible = True
        Me.btnChange.Visible = True
        Me.btnReturn.Visible = True

        NotChangeData(False)
    End Sub
#End Region

#Region "編集不可制御"
    '***************************************************************************************************
    '   ＩＤ　：NotChangeData
    '   名称　：編集不可制御
    '   概要  ：会議通知が参照で開かれた際、各コントロールを編集不可とする
    '   引数　：blnChange：編集可不可
    '   戻り値：なし
    '   作成日：2011/12/23(木) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/23(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub NotChangeData(ByVal blnChange As Boolean)
        If blnChange = True Then
            'Me.chkUnion.Enabled = True
            Call Utilities.SetCanEditToControl(True, Me.chkUnion)
            'Me.GroupBox2.Enabled = True
            Call Utilities.SetCanEditToControl(True, Me.GroupBox2)
            'Me.CboCommittee.Enabled = True
            Call Utilities.SetCanEditToControl(True, Me.CboCommittee)
            'Me.cboMeetingName.Enabled = True
            Call Utilities.SetCanEditToControl(True, Me.cboMeetingName)
            'Me.cboMeetingPlace.Enabled = True
            Call Utilities.SetCanEditToControl(True, Me.cboMeetingPlace)
            Me.txtMeetingPlace.ReadOnly = False
            Me.dtpDate1.Enabled = True
            'Call Utilities.SetCanEditToControl(True, Me.dtpDate1)
            Me.mtxStartTime1.Enabled = True
            Me.mtxEndTime1.Enabled = True
            'Me.chkDate2.Enabled = True
            Call Utilities.SetCanEditToControl(True, Me.chkDate2)
            If Me.chkDate2.CheckState = CheckState.Checked Then
                Me.dtpDate2.Enabled = True
                Me.mtxStartTime2.Enabled = True
                Me.mtxEndTime2.Enabled = True
            End If
            'Me.chkDate3.Enabled = True
            Call Utilities.SetCanEditToControl(True, Me.chkDate3)
            If Me.chkDate3.CheckState = CheckState.Checked Then
                Me.dtpDate3.Enabled = True
                Me.mtxStartTime3.Enabled = True
                Me.mtxEndTime3.Enabled = True
            End If

            Me.txtGoMachineName.ReadOnly = False
            Me.mtxGoTime.ReadOnly = False
            Me.txtReturnMachineName.ReadOnly = False
            Me.mtxReturnTime.ReadOnly = False
            Me.txtNote.ReadOnly = False
            Me.txtTheme1.ReadOnly = False
            Me.txtTheme2.ReadOnly = False
            Me.txtTheme3.ReadOnly = False
            Me.txtTheme4.ReadOnly = False
            Me.txtTheme5.ReadOnly = False
        Else
            Call Utilities.SetCanEditToControl(False, Me.chkUnion)
            Call Utilities.SetCanEditToControl(False, Me.GroupBox2)
            Call Utilities.SetCanEditToControl(False, Me.CboCommittee)
            Call Utilities.SetCanEditToControl(False, Me.cboMeetingName)
            Call Utilities.SetCanEditToControl(False, Me.cboMeetingPlace)
            Me.txtMeetingPlace.ReadOnly = True
            Me.dtpDate1.Enabled = False
            'Call Utilities.SetCanEditToControl(False, Me.dtpDate1)
            Me.mtxStartTime1.Enabled = False
            Me.mtxEndTime1.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.chkDate2)
            Me.dtpDate2.Enabled = False
            'Call Utilities.SetCanEditToControl(False, Me.dtpDate2)
            Me.mtxStartTime2.Enabled = False
            Me.mtxEndTime2.Enabled = False
            Call Utilities.SetCanEditToControl(False, Me.chkDate3)
            Me.dtpDate3.Enabled = False
            'Call Utilities.SetCanEditToControl(False, Me.dtpDate3)
            Me.mtxStartTime3.Enabled = False
            Me.mtxEndTime3.Enabled = False

            Me.txtGoMachineName.ReadOnly = True
            Me.mtxGoTime.ReadOnly = True
            Me.txtReturnMachineName.ReadOnly = True
            Me.mtxReturnTime.ReadOnly = True
            Me.txtNote.ReadOnly = True
            Me.txtTheme1.ReadOnly = True
            Me.txtTheme2.ReadOnly = True
            Me.txtTheme3.ReadOnly = True
            Me.txtTheme4.ReadOnly = True
            Me.txtTheme5.ReadOnly = True
        End If

    End Sub
#End Region

#Region "支部データ取得処理"
    '***************************************************************************************************
    '   ＩＤ　：SetBranch
    '   名称　：支部データ取得処理
    '   概要  ：支部情報を取得しリストに格納する。
    '   引数　：なし 
    '   戻り値：なし
    '   作成日：2011/12/21(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/21(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetBranch()
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

            If dtRet.Rows.Count > 0 Then

                For Each dtRow As DataRow In dtRet.Rows
                    _dicBranch.Add(dtRow.Item("l_name"), dtRow.Item("c_constant_seq"))
                Next
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

#Region "支部コンボボックス設定処理"
    Private Sub SetComboBranch()
        Me.cboshibu.Items.Add("")
        For Each strKey As String In _dicBranch.Keys
            If strKey.Equals(SHIBU_OTHER_STRING) = False Then
                Me.cboshibu.Items.Add(strKey)
            End If
        Next
    End Sub
#End Region

#Region "開催場所コンボボックス設定処理"
    Private Sub SetComboMeetingPlace()
        Me.cboMeetingPlace.Items.Add("")
        For Each strKey As String In _dicBranch.Keys
            Me.cboMeetingPlace.Items.Add(strKey)
        Next
    End Sub
#End Region

#Region "組合員選択後のクリスタルレポート印刷処理"
    '***************************************************************************************************
    '   ＩＤ　：CreateCrystalReport
    '   名称　：組合員選択後のクリスタルレポート印刷処理
    '   概要  ：
    '   引数　：fm000204  :組合員抽出画面
    '         ：blnShow   :レポートプレビュー画面を再度表示する必要がある場合にはTrue
    '         ：telFaxList:電話番号、FAX番号リスト
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/30(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/30(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub CreateCrystalReport(ByVal fm000204 As FM000204, _
                                    ByVal blnShow As Boolean, _
                                    ByVal telFaxList As TelFax, _
                                    Optional ByVal number As String = "")
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        '選択された組合員のリスト
        Dim dt As DataTable = fm000204.SelectMemberList
        '処理結果
        Dim blnRet As Boolean = True
        Dim fm As New FM000203
        Dim reportObj As CrystalDecisions.CrystalReports.Engine.ReportDocument
        Dim ds As DS0203P1 = New DS0203P1()

        Try
            'ボタン表示タイプを設定
            fm.ButtonShowType = 3
            fm.PrintCntVisible = False
            reportObj = New CR0203P1
            fm.ObjResource = reportObj

            Dim drMeetingNotice As DS0203P1.dtHeaderRow
            For Each row As DataRow In dt.Rows
                drMeetingNotice = ds.dtHeader.NewRow
                drMeetingNotice.BeginEdit()

                '社員番号
                drMeetingNotice.c_staf_id = row.Item(1)
                '機種
                drMeetingNotice.k_model = row.Item(2)
                '社員名
                drMeetingNotice.l_name = row.Item(0)
                '共通情報をクリスタルレポートにセット
                SetReportInfo(drMeetingNotice, telFaxList, number)

                drMeetingNotice.EndEdit()
                ds.dtHeader.Rows.Add(drMeetingNotice)

            Next
            reportObj.SetDataSource(ds)
            If blnShow = True Then
                '再度プレビュー画面を表示する必要がある場合
                Call fm.ShowDialog()

                Select Case fm.IntQlickBtnFlag
                    Case 2
                        'キャンセルの場合特に何も行わない
                        Exit Sub
                    Case 3
                        '印刷実行
                        fm.PrintOut()
                End Select
            Else
                'そのまま印刷処理を実行する場合
                fm.PrintOut()
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateCrystalReport")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub
#End Region

#Region "クリスタルレポート共通データのセット"
    '***************************************************************************************************
    '   ＩＤ　：SetReportInfo
    '   名称　：クリスタルレポート共通データのセット
    '   概要  ：社員、機種、名前以外の情報を画面入力情報よりセットする
    '   引数　：drMeetingNotice:クリスタルレポートのデータセット
    '         ：telFaxList     :電話番号、FAX番号の構造体
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/29(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/29(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Sub SetReportInfo(ByVal drMeetingNotice As DS0203P1.dtHeaderRow, ByVal telFaxList As TelFax, Optional ByVal number As String = "")
        Dim intMid As Integer = 0
        Dim meetingNumberList As String() = Nothing
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '今日の日付を設定
            drMeetingNotice.d_up = System.DateTime.Now().Date
            If Me.optOpen.Checked = True Then
                drMeetingNotice.l_title = "会議　『開催通知』"
                drMeetingNotice.k_information_type = "開催"
            ElseIf Me.optChange.Checked = True Then
                drMeetingNotice.l_title = "会議　『変更通知』"
                drMeetingNotice.k_information_type = "変更"
            ElseIf Me.optStop.Checked = True Then
                drMeetingNotice.l_title = "会議　『中止通知』"
                drMeetingNotice.k_information_type = "中止"
            End If

            '支部
            drMeetingNotice.belonging_name = Me.cboshibu.Text.Trim()
            '会議名
            drMeetingNotice.l_information_name = Me.cboMeetingName.Text.Trim()
            '合同区分
            If Me.chkUnion.CheckState = CheckState.Checked Then
                drMeetingNotice.k_union = UNION_ON_STRING
            End If

            '会議通知番号-先頭2文字
            drMeetingNotice.l_ommision_name = MDLoginInfo.Period.ToString()
            intMid = Me.txtMeetingNumber.Text.IndexOf("-")
            If number = "" Then
                If intMid <> -1 Then
                    meetingNumberList = Me.txtMeetingNumber.Text.Split("-")
                    '会議通知番号-後ろ2文字
                    drMeetingNotice.c_meeting = meetingNumberList(1)
                Else
                    '会議通知番号-後ろ2文字
                    drMeetingNotice.c_meeting = "***"
                End If
            Else
                drMeetingNotice.c_meeting = number
            End If

            '開催場所
            drMeetingNotice.l_open_belonging = Me.cboMeetingPlace.Text
            '会議場
            drMeetingNotice.l_place = Me.txtMeetingPlace.Text.Trim
            drMeetingNotice.d_meeting_1 = Me.dtpDate1.Value.Date()
            drMeetingNotice.d_meeting_time_1 = Me.dtpDate1.Value.TimeOfDay().ToString()
            drMeetingNotice.meeting_days_1 = Me.dtpDate1.Value.Month().ToString() & "/" & dtpDate1.Value.Day().ToString()
            If Me.mtxStartTime1.ValidateText IsNot Nothing Then
                drMeetingNotice.d_meeting_time_from_1 = Me.mtxStartTime1.Text
            End If

            If Me.mtxEndTime1.ValidateText IsNot Nothing Then
                drMeetingNotice.d_meeting_time_to_1 = Me.mtxEndTime1.Text
            End If

            If Me.chkDate2.CheckState = CheckState.Checked Then
                '2日目の日付が指定されている場合通知書に反映
                drMeetingNotice.d_meeting_2 = Me.dtpDate2.Value.Date()
                drMeetingNotice.d_meeting_time_2 = Me.dtpDate2.Value.TimeOfDay().ToString()
                drMeetingNotice.meeting_days_2 = Me.dtpDate2.Value.Month().ToString() & "/" & dtpDate2.Value.Day()
                If Me.mtxStartTime2.ValidateText IsNot Nothing Then
                    drMeetingNotice.d_meeting_time_from_2 = Me.mtxStartTime2.Text
                End If
                If Me.mtxEndTime2.ValidateText IsNot Nothing Then
                    drMeetingNotice.d_meeting_time_to_2 = Me.mtxEndTime2.Text
                End If
            End If
            If Me.chkDate3.CheckState = CheckState.Checked Then
                '3日目の日付が指定されている場合通知書に反映
                drMeetingNotice.d_meeting_3 = Me.dtpDate3.Value.Date()
                drMeetingNotice.d_meeting_time_3 = Me.dtpDate3.Value.TimeOfDay().ToString()
                drMeetingNotice.meeting_days_3 = Me.dtpDate3.Value.Month().ToString() & "/" & dtpDate3.Value.Day()
                If Me.mtxStartTime3.ValidateText IsNot Nothing Then
                    drMeetingNotice.d_meeting_time_from_3 = Me.mtxStartTime3.Text
                End If
                If Me.mtxEndTime3.ValidateText IsNot Nothing Then
                    drMeetingNotice.d_meeting_time_to_3 = Me.mtxEndTime3.Text
                End If
            End If

            '備考
            drMeetingNotice.l_biko_1 = Me.txtNote.Text.Trim
            '議題1～5
            drMeetingNotice.l_subject_1 = Me.txtTheme1.Text.Trim
            drMeetingNotice.l_subject_2 = Me.txtTheme2.Text.Trim
            drMeetingNotice.l_subject_3 = Me.txtTheme3.Text.Trim
            drMeetingNotice.l_subject_4 = Me.txtTheme4.Text.Trim
            drMeetingNotice.l_subject_5 = Me.txtTheme5.Text.Trim
            drMeetingNotice.committee_name = Me.CboCommittee.Text

            '移動‐往路が設定されている場合
            If ChkNull(Me.txtGoMachineName.Text) = False Then
                drMeetingNotice.l_flight_to = Me.txtGoMachineName.Text  '移動便名の設定
                drMeetingNotice.d_flightday_to = dtpDate1.Value.Date() '出発日の設定
                If Me.mtxGoTime.ValidateText IsNot Nothing Then
                    drMeetingNotice.d_filght_to = Me.mtxGoTime.Text  '出発時間の設定
                End If
            End If
            '移動‐復路が設定されている場合
            If ChkNull(Me.txtReturnMachineName.Text) = False Then
                drMeetingNotice.l_flight_back = Me.txtReturnMachineName.Text.Trim   '移動便名の設定
                If chkDate3.CheckState = CheckState.Checked Then
                    '開催日時3日目より出発日の設定
                    drMeetingNotice.d_flightday_back = Me.dtpDate3.Value.Date()
                ElseIf chkDate2.CheckState = CheckState.Checked Then
                    '開催日時2日目より出発日の設定
                    drMeetingNotice.d_flightday_back = Me.dtpDate2.Value.Date()
                Else
                    '開催日時1日目より出発日の設定
                    drMeetingNotice.d_flightday_back = Me.dtpDate1.Value.Date()
                End If
                If Me.mtxReturnTime.ValidateText IsNot Nothing Then
                    drMeetingNotice.d_flight_back = Me.mtxReturnTime.Text '出発時間の設定"
                End If
            End If

            '東京の電話番号
            drMeetingNotice.t_tel = telFaxList.strTokyoTel
            '東京のFAX番号
            drMeetingNotice.t_fax = telFaxList.strTokyoFax
            '大阪の電話番号
            drMeetingNotice.o_tel = telFaxList.strOsakaTel
            '大阪のFAX番号
            drMeetingNotice.o_fax = telFaxList.strOsakaFax
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "SetReportInfo")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

    End Sub

#End Region

    Private Function CreateUnionMember() As String()
        Dim strMember As String() = Nothing
        Dim strSql As String = String.Empty
        Dim clsMdb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing

        Try
            strSql = strSql & "SELECT staf.c_user_id AS 社員番号 " & vbCrLf

            strSql = strSql & "FROM ((SELECT *" & vbCrLf
            strSql = strSql & "      FROM committee_list AS t1" & vbCrLf
            strSql = strSql & "      WHERE EXISTS" & vbCrLf
            strSql = strSql & "              (SELECT * FROM" & vbCrLf
            strSql = strSql & "                 (SELECT c_committee_id, MAX(d_from) AS now_from" & vbCrLf
            strSql = strSql & "                  FROM committee_list" & vbCrLf
            strSql = strSql & "                  WHERE d_from <= '" & Me.dtpDate1.Value.Date.ToString("yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "                  GROUP BY c_committee_id) AS t2" & vbCrLf
            strSql = strSql & "               WHERE t1.c_committee_id = t2.c_committee_id AND"
            strSql = strSql & "                     t1.d_from         = t2.now_from)) AS com_list" & vbCrLf '委員会名簿を基準日で検索したテーブル

            strSql = strSql & "INNER JOIN committee_list_dtl AS com_list_dtl" & vbCrLf '委員会名簿明細
            strSql = strSql & "        ON com_list.c_committee_list = com_list_dtl.c_committee_list) " & vbCrLf

            strSql = strSql & "INNER JOIN (SELECT * " & vbCrLf
            strSql = strSql & "            FROM staf_attribute AS t3 " & vbCrLf
            strSql = strSql & "            WHERE EXISTS " & vbCrLf
            strSql = strSql & "              (SELECT * FROM " & vbCrLf
            strSql = strSql & "                 (SELECT c_user_id, c_ksh, c_staf_id, MAX(d_from) AS now_from " & vbCrLf
            strSql = strSql & "                  FROM staf_attribute " & vbCrLf
            strSql = strSql & "                  WHERE d_from <= '" & Me.dtpDate1.Value.Date.ToString("yyyyMMdd") & "'" & vbCrLf
            strSql = strSql & "                  GROUP BY c_user_id, c_ksh, c_staf_id) AS t4 " & vbCrLf
            strSql = strSql & "               WHERE t3.c_user_id = t4.c_user_id " & vbCrLf
            strSql = strSql & "                 AND t3.c_ksh     = t4.c_ksh " & vbCrLf
            strSql = strSql & "                 AND t3.d_from    = t4.now_from)) AS staf " & vbCrLf '組合員情報を基準日で検索したテーブル
            strSql = strSql & "        ON com_list_dtl.c_user_id = staf.c_user_id " & vbCrLf

            strSql = strSql & "WHERE com_list.c_period_id            = '" & MDLoginInfo.PeriodId & "'" & vbCrLf '期ID
            strSql = strSql & "  AND com_list_dtl.c_committee_id = '" & Me.CboCommittee.SelectedValue & "'" & vbCrLf  '委員会ID
            strSql = strSql & "  AND staf.k_belonging            = '" & _dicBranch(Me.cboshibu.Text) & "'" & vbCrLf '所属支部ID

            strSql = strSql & "ORDER BY com_list_dtl.s_committee_seq, CLng(staf.c_user_id) " & vbCrLf '役職、社員番号でソート


            'DB接続開始
            clsMdb.Connect()
            ' SQL実行
            dtRet = clsMdb.ExecuteSql(strSql)

            If dtRet.Rows.Count > 0 Then
                Dim strStaffnumber As List(Of String) = New List(Of String)
                For Each dtRow As DataRow In dtRet.Rows
                    strStaffnumber.Add(dtRow.Item("社員番号"))
                Next

                strMember = strStaffnumber.ToArray
            End If

        Catch ex As Exception
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
        Return strMember
    End Function

#Region "登録データ作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateInsertData
    '   名称　：登録データ作成
    '   概要  ：画面に入力されたデータより、テーブルへ登録するデータを作成する
    '   引数　：blnWork:一時保存の場合true、本登録の場合false
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/24(木) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function CreateInsertData(ByVal blnWork As Boolean) As miData
        Dim data As miData = New miData
        Dim strMeetingNumber As String = String.Empty
        Dim strNewSeq As String = String.Empty
        Dim intNewSeq As Integer = 0
        '空白の除いた備考を取得
        Dim strNoteList As List(Of String) = GetNoteList()

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '新規会議通知番号の設定
            If blnWork = False Then
                If IntClickBtnFlg = 1 OrElse IntClickBtnFlg = 2 OrElse _
                   (IntClickBtnFlg = 5 AndAlso _drMeetingInformation.Item("c_meeting").Equals(DBNull.Value) = False _
                                              AndAlso ChkNull(_drMeetingInformation.Item("c_meeting")) = False) Then
                    '変更登録、中止登録、または会議通知が発行済みの一時保存の場合、
                    '会議通知番号は新規作成しない
                    strMeetingNumber = _drMeetingInformation.Item("c_meeting")
                Else
                    '本登録の場合会議通知番号を作成
                    strMeetingNumber = MDLoginInfo.Period.ToString & "-" & GetMaxMeetingNumber(blnWork)
                End If
            Else
                If IntClickBtnFlg <> 0 AndAlso IntClickBtnFlg <> 3 Then
                    '新規または合同以外の一時保存の場合、既に発行済みの会議通知番号をそのまま設定する
                    strMeetingNumber = _drMeetingInformation.Item("c_meeting")
                End If
            End If
            'シーケンス番号の設定
            intNewSeq = GetSequenceNumber(blnWork)


            '会社コード
            data.strKsh = MDLoginInfo.Ksh

            '申請地区区分
            data.strApplyAreaCode = _dicBranch(Me.cboshibu.Text)

            '期ID
            data.strPeriodId = MDLoginInfo.PeriodId
            '会議番号
            data.strMeetingNumber = strMeetingNumber
            '会議番号SEQ
            data.intSeq = intNewSeq

            '合同区分
            If Me.chkUnion.CheckState = CheckState.Checked Then
                data.strUnioncode = UNION_ON
            Else
                data.strUnioncode = UNION_OFF
            End If
            '種類
            If Me.optOpen.Checked = True Then
                '開催
                data.strInformationTypeCode = INFORMATION_TYPE_OPEN
            ElseIf Me.optChange.Checked = True Then
                '変更
                data.strInformationTypeCode = INFORMATION_TYPE_CHANGE
            Else
                '中止
                data.strInformationTypeCode = INFORMATION_TYPE_STOP
            End If
            '会議日付1
            data.dtmMeeting1 = "'" & Me.dtpDate1.Value.Date().ToString() & "'"
            '会議時間From1
            If Me.mtxStartTime1.ValidateText IsNot Nothing Then
                data.strMeetingTimeFrom1 = Me.mtxStartTime1.Text
            Else
                data.strMeetingTimeFrom1 = String.Empty
            End If
            '会議時間To1
            If Me.mtxEndTime1.ValidateText IsNot Nothing Then
                data.strMeetingTimeTo1 = Me.mtxEndTime1.Text
            Else
                data.strMeetingTimeTo1 = String.Empty
            End If
            If Me.txtGoMachineName.Text.Trim() <> String.Empty Then
                '移動フライト往路1
                data.strFlightTo1 = Me.txtGoMachineName.Text.Trim
                '移動フライト時間往路1
                If Me.mtxGoTime.ValidateText IsNot Nothing Then
                    data.strFlightTimeTo1 = Me.mtxGoTime.Text
                Else
                    data.strFlightTimeTo1 = String.Empty
                End If
            Else
                data.strFlightTo1 = String.Empty
                data.strFlightTimeTo1 = String.Empty
            End If
            If Me.txtReturnMachineName.Text.Trim() <> String.Empty Then
                '移動フライト復路1
                data.strFlightBack1 = Me.txtReturnMachineName.Text.Trim()
                '移動フライト時間復路1
                If Me.mtxReturnTime.ValidateText IsNot Nothing Then
                    data.strFlightTimeBack1 = Me.mtxReturnTime.Text
                Else
                    data.strFlightTimeBack1 = String.Empty
                End If
            Else
                data.strFlightBack1 = String.Empty
                data.strFlightTimeBack1 = String.Empty
            End If
            '会議日付2、会議時間From2、会議時間To2
            If chkDate2.CheckState = CheckState.Checked Then
                data.dtmMeeting2 = "'" & Me.dtpDate2.Value.Date().ToString() & "'"
                If Me.mtxStartTime2.ValidateText IsNot Nothing Then
                    data.strMeetingTimeFrom2 = Me.mtxStartTime2.Text
                Else
                    data.strMeetingTimeFrom2 = String.Empty
                End If

                If Me.mtxEndTime2.ValidateText IsNot Nothing Then
                    data.strMeetingTimeTo2 = Me.mtxEndTime2.Text
                Else
                    data.strMeetingTimeTo2 = String.Empty
                End If
            Else
                data.dtmMeeting2 = "Null"
                data.strMeetingTimeFrom2 = String.Empty
                data.strMeetingTimeTo2 = String.Empty
            End If
            '会議日付3、会議時間From3、会議時間To3
            If chkDate3.CheckState = CheckState.Checked Then
                data.dtmMeeting3 = "'" & Me.dtpDate3.Value.Date().ToString() & "'"
                If Me.mtxStartTime3.ValidateText IsNot Nothing Then
                    data.strMeetingTimeFrom3 = Me.mtxStartTime3.Text
                Else
                    data.strMeetingTimeFrom3 = String.Empty
                End If

                If Me.mtxEndTime3.ValidateText IsNot Nothing Then
                    data.strMeetingTimeTo3 = Me.mtxEndTime3.Text
                Else
                    data.strMeetingTimeTo3 = String.Empty
                End If
            Else
                data.dtmMeeting3 = "Null"
                data.strMeetingTimeFrom3 = String.Empty
                data.strMeetingTimeTo3 = String.Empty
            End If

            If IntClickBtnFlg = 3 Then '合同登録の場合
                '現在表示中の会議通知と同一の会議番号のうち、最新のデータを取得
                Dim dtNew As DataTable = GetLatestMeetingNotice()

                If dtNew.Rows.Count > 0 Then
                    '委員会ID
                    data.strCommitteeId = dtNew.Rows(0).Item("c_committee_id")
                    '会議名（目的）
                    data.strInformationName = dtNew.Rows(0).Item("l_information_name")
                    '開催場所
                    data.strOpenBeBiginting = dtNew.Rows(0).Item("l_open_bebiginting")
                Else
                    data.strCommitteeId = String.Empty
                    data.strInformationName = String.Empty
                    data.strOpenBeBiginting = String.Empty
                End If
            Else
                '委員会ID
                If Me.CboCommittee.SelectedValue() IsNot DBNull.Value Then
                    data.strCommitteeId = Me.CboCommittee.SelectedValue()
                Else
                    data.strCommitteeId = String.Empty
                End If
                '会議名（目的）
                data.strInformationName = Me.cboMeetingName.Text.Trim()
                '開催場所
                data.strOpenBeBiginting = Me.cboMeetingPlace.Text.Trim()
            End If
            '会議場所
            data.strPlace = Me.txtMeetingPlace.Text.Trim()
            '議題1
            data.strSubject1 = Me.txtTheme1.Text.Trim()
            '議題2
            data.strSubject2 = Me.txtTheme2.Text.Trim()
            '議題3
            data.strSubject3 = Me.txtTheme3.Text.Trim()
            '議題4
            data.strSubject4 = Me.txtTheme4.Text.Trim()
            '議題5
            data.strSubject5 = Me.txtTheme5.Text.Trim()

            If strNoteList.Count >= 1 Then
                '備考1
                data.strBiko1 = strNoteList(0)
            Else
                data.strBiko1 = String.Empty
            End If
            If strNoteList.Count >= 2 Then
                '備考2
                data.strBiko2 = strNoteList(1)
            Else
                data.strBiko2 = String.Empty
            End If
            If strNoteList.Count >= 3 Then
                '備考3
                data.strBiko3 = strNoteList(2)
            Else
                data.strBiko3 = String.Empty
            End If

            '更新日
            data.dtmUpdateDate = "'" & System.DateTime.Now().Date.ToString() & "'"
            '更新者個人ID
            data.strUpdateUserId = MDLoginInfo.UserId
            '作成日
            'data.dtmInsertDate = "'" & Me.txtInsertDate.Text & "'"
            data.dtmInsertDate = "'" & System.DateTime.Now().Date.ToString() & "'"
            '作成者個人ID
            data.strInsertUserId = MDLoginInfo.UserId
            '更新回数
            data.intUpdateTime = 0

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateInsertData")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return data
    End Function

#End Region

#Region "更新データ作成"
    '***************************************************************************************************
    '   ＩＤ　：CreateUpdateData
    '   名称　：更新データ作成
    '   概要  ：画面に入力されたデータより、テーブルへ登録するデータを作成する
    '   引数　：blnWork:一時保存の場合true、本登録の場合false
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/24(木) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function CreateUpdateData(ByVal blnWork As Boolean) As miData
        Dim data As miData = New miData()
        '空白の除いた備考を取得
        Dim strNoteList As List(Of String) = GetNoteList()

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If blnWork = True Then
                '申請地区区分の設定
                If Me.cboshibu.Text <> String.Empty Then
                    data.strApplyAreaCode = _dicBranch(Me.cboshibu.Text)
                Else
                    data.strApplyAreaCode = String.Empty
                End If

                '委員会の設定
                If Me.CboCommittee.SelectedValue().Equals(DBNull.Value) = False Then
                    data.strCommitteeId = Me.CboCommittee.SelectedValue()
                Else
                    data.strCommitteeId = String.Empty
                End If
            Else
                data.strCommitteeId = _drMeetingInformation.Item("c_committee_id")
            End If

            '合同区分
            If Me.chkUnion.CheckState = CheckState.Checked Then
                data.strUnioncode = UNION_ON
            Else
                data.strUnioncode = UNION_OFF
            End If

            '種類
            If Me.optOpen.Checked = True Then
                '開催
                data.strInformationTypeCode = INFORMATION_TYPE_OPEN
            ElseIf Me.optChange.Checked = True Then
                '変更
                data.strInformationTypeCode = INFORMATION_TYPE_CHANGE
            Else
                '中止
                data.strInformationTypeCode = INFORMATION_TYPE_STOP
            End If

            '会議名（目的）
            data.strInformationName = Me.cboMeetingName.Text
            '会議日付1
            data.dtmMeeting1 = "'" & Me.dtpDate1.Value.Date().ToString() & "'"
            '会議時間From1
            If Me.mtxStartTime1.ValidateText IsNot Nothing Then
                data.strMeetingTimeFrom1 = Me.mtxStartTime1.Text
            End If
            '会議時間To1
            If Me.mtxEndTime1.ValidateText IsNot Nothing Then
                data.strMeetingTimeTo1 = Me.mtxEndTime1.Text
            End If
            If Me.txtGoMachineName.Text.Trim() <> String.Empty Then
                '移動フライト往路1
                data.strFlightTo1 = Me.txtGoMachineName.Text.Trim()
                '移動フライト時間往路1
                If Me.mtxGoTime.ValidateText IsNot Nothing Then
                    data.strFlightTimeTo1 = Me.mtxGoTime.Text
                End If
            Else
                data.strFlightTo1 = String.Empty
                data.strFlightTimeTo1 = String.Empty
            End If
            If Me.txtReturnMachineName.Text.Trim() <> String.Empty Then
                '移動フライト復路1
                data.strFlightBack1 = Me.txtReturnMachineName.Text.Trim()
                '移動フライト時間復路1
                If Me.mtxReturnTime.ValidateText IsNot Nothing Then
                    data.strFlightTimeBack1 = Me.mtxReturnTime.Text
                End If
            Else
                data.strFlightBack1 = String.Empty
                data.strFlightTimeBack1 = String.Empty
            End If
            '会議日付2、会議時間From2、会議時間To2
            If chkDate2.CheckState = CheckState.Checked Then
                data.dtmMeeting2 = "'" & Me.dtpDate2.Value.Date().ToString() & "'"
                If Me.mtxStartTime2.ValidateText IsNot Nothing Then
                    data.strMeetingTimeFrom2 = Me.mtxStartTime2.Text
                End If
                If Me.mtxEndTime2.ValidateText IsNot Nothing Then
                    data.strMeetingTimeTo2 = Me.mtxEndTime2.Text
                End If
            Else
                data.dtmMeeting2 = "Null"
                data.strMeetingTimeFrom2 = String.Empty
                data.strMeetingTimeTo2 = String.Empty
            End If
            '会議日付3、会議時間From3、会議時間To3
            If chkDate3.CheckState = CheckState.Checked Then
                data.dtmMeeting3 = "'" & Me.dtpDate3.Value.Date().ToString() & "'"
                If Me.mtxStartTime3.ValidateText IsNot Nothing Then
                    data.strMeetingTimeFrom3 = Me.mtxStartTime3.Text
                End If
                If Me.mtxEndTime3.ValidateText IsNot Nothing Then
                    data.strMeetingTimeTo3 = Me.mtxEndTime3.Text
                End If
            Else
                data.dtmMeeting3 = "Null"
                data.strMeetingTimeFrom3 = String.Empty
                data.strMeetingTimeTo3 = String.Empty
            End If

            '開催場所
            data.strOpenBeBiginting = Me.cboMeetingPlace.Text.Trim()
            '会議場所
            data.strPlace = Me.txtMeetingPlace.Text.Trim()
            '議題1
            data.strSubject1 = Me.txtTheme1.Text.Trim()
            '議題2
            data.strSubject2 = Me.txtTheme2.Text.Trim()
            '議題3
            data.strSubject3 = Me.txtTheme3.Text.Trim()
            '議題4
            data.strSubject4 = Me.txtTheme4.Text.Trim()
            '議題5
            data.strSubject5 = Me.txtTheme5.Text.Trim()

            If strNoteList.Count >= 1 Then
                '備考1
                data.strBiko1 = strNoteList(0)
            End If
            If strNoteList.Count >= 2 Then
                '備考2
                data.strBiko2 = strNoteList(1)
            End If
            If strNoteList.Count >= 3 Then
                '備考3
                data.strBiko3 = strNoteList(2)
            End If

            '更新日
            data.dtmUpdateDate = "'" & System.DateTime.Now().Date.ToString() & "'"
            '更新者個人ID
            data.strUpdateUserId = MDLoginInfo.UserId
            '更新回数
            data.intUpdateTime = CInt(_drMeetingInformation.Item("s_up")) + 1
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "CreateUpdateData")
            log.Fatal(ex.Message)
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return data
    End Function

#End Region

#Region "データ登録処理"
    '***************************************************************************************************
    '   ＩＤ　：InsertData
    '   名称　：データ登録処理
    '   概要  ：Insert文を発行し、テーブルへデータを登録する
    '   引数　：iData:テーブル登録用のデータ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/24(木) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木) a.onuma  新規作成
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    Public Function InsertData(ByVal iData As miData) As Boolean
        'Insert用データ
        Dim data As miData = Nothing
        'SQL文
        Dim strSql As String = String.Empty
        'SQL実行結果
        Dim intRet As Integer = -1
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '処理結果
        Dim blnRet As Boolean = False

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '登録データの取得
            data = iData

            'Insert文の作成
            strSql = "Insert Into meeting_information( " &
                     "c_ksh " &
                     ",k_apply_area " &
                     ",c_period_id " &
                     ",c_meeting " &
                     ",s_meeting " &
                     ",c_committee_id " &
                     ",k_union " &
                     ",k_information_type " &
                     ",l_information_name " &
                     ",d_meeting_1 " &
                     ",d_meeting_time_from_1 " &
                     ",d_meeting_time_to_1 " &
                     ",l_flight_to_1 " &
                     ",d_flight_to_1 " &
                     ",l_flight_back_1 " &
                     ",d_flight_back_1 " &
                     ",d_meeting_2 " &
                     ",d_meeting_time_from_2 " &
                     ",d_meeting_time_to_2 " &
                     ",l_flight_to_2 " &
                     ",d_flight_to_2 " &
                     ",l_flight_back_2 " &
                     ",d_flight_back_2 " &
                     ",d_meeting_3 " &
                     ",d_meeting_time_from_3 " &
                     ",d_meeting_time_to_3 " &
                     ",l_flight_to_3 " &
                     ",d_flight_to_3 " &
                     ",l_flight_back_3 " &
                     ",d_flight_back_3 " &
                     ",l_open_bebiginting " &
                     ",l_place " &
                     ",l_subject_1 " &
                     ",l_subject_2 " &
                     ",l_subject_3 " &
                     ",l_subject_4 " &
                     ",l_subject_5 " &
                     ",l_biko_1 " &
                     ",l_biko_2 " &
                     ",l_biko_3 " &
                     ",d_ins " &
                     ",c_user_id_ins " &
                     ",d_up " &
                     ",c_user_id_up " &
                     ",s_up " &
                     ") VALUES( " &
                     "'" & data.strKsh & "' " &
                     ",'" & data.strApplyAreaCode & "' " &
                     ",'" & data.strPeriodId & "' " &
                     ",'" & data.strMeetingNumber & "' " &
                     "," & data.intSeq &
                     ",'" & data.strCommitteeId & "' " &
                     ",'" & data.strUnioncode & "' " &
                     ",'" & data.strInformationTypeCode & "' " &
                     ",'" & data.strInformationName & "' " &
                     "," & data.dtmMeeting1 &
                     ",'" & data.strMeetingTimeFrom1 & "' " &
                     ",'" & data.strMeetingTimeTo1 & "' " &
                     ",'" & data.strFlightTo1 & "' " &
                     ",'" & data.strFlightTimeTo1 & "' " &
                     ",'" & data.strFlightBack1 & "' " &
                     ",'" & data.strFlightTimeBack1 & "' " &
                     "," & data.dtmMeeting2 &
                     ",'" & data.strMeetingTimeFrom2 & "' " &
                     ",'" & data.strMeetingTimeTo2 & "' " &
                     ",'" & data.strFlightTo2 & "' " &
                     ",'" & data.strFlightTimeTo2 & "' " &
                     ",'" & data.strFlightBack2 & "' " &
                     ",'" & data.strFlightTimeBack2 & "' " &
                     "," & data.dtmMeeting3 &
                     ",'" & data.strMeetingTimeFrom3 & "' " &
                     ",'" & data.strMeetingTimeTo3 & "' " &
                     ",'" & data.strFlightTo3 & "' " &
                     ",'" & data.strFlightTimeTo3 & "' " &
                     ",'" & data.strFlightBack3 & "' " &
                     ",'" & data.strFlightTimeBack3 & "' " &
                     ",'" & data.strOpenBeBiginting & "' " &
                     ",'" & data.strPlace & "' " &
                     ",'" & data.strSubject1 & "' " &
                     ",'" & data.strSubject2 & "' " &
                     ",'" & data.strSubject3 & "' " &
                     ",'" & data.strSubject4 & "' " &
                     ",'" & data.strSubject5 & "' " &
                     ",'" & data.strBiko1 & "' " &
                     ",'" & data.strBiko2 & "' " &
                     ",'" & data.strBiko3 & "' " &
                     "," & data.dtmInsertDate &
                     ",'" & data.strInsertUserId & "' " &
                     "," & data.dtmUpdateDate &
                     ",'" & data.strUpdateUserId & "' " &
                     "," & data.intUpdateTime & ")"

            'DB接続&トランザクション開始
            clsMdb.Connect()
            clsMdb.BeginTran()

            'SQL実行
            intRet = clsMdb.ExecuteNonQuery(strSql)
            log.Debug(strSql)
            If intRet = 1 Then
                'コミット
                clsMdb.CommitTran()
                blnRet = True

                ' SEQUENCファイルに値を反映
                Dim strSeqName As String = "seq_met_err.txt"
                If _dicBranch(Me.cboshibu.Text) = "01" Then
                    strSeqName = "seq_met_tyo_" + MDLoginInfo.PeriodId + ".txt"
                ElseIf _dicBranch(Me.cboshibu.Text) = "02" Then
                    strSeqName = "seq_met_osa_" + MDLoginInfo.PeriodId + ".txt"
                End If
                Dim sw As New System.IO.StreamWriter(MDSystemInfo.SequencePath + strSeqName, False)
                sw.Write(data.strMeetingNumber.Substring(data.strMeetingNumber.IndexOf("-") + 1))
                sw.Close()

                log.Info(String.Format("{0}件のデータを追加しました。", intRet.ToString()))
            Else
                '処理結果が1行でない場合はロールバック
                clsMdb.RollbackTran()
                log.Error("DB更新処理に異常があったためデータ追加を中止しました。")
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "InsertData")
            'ロールバック
            clsMdb.RollbackTran()
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return blnRet
    End Function

#End Region

#Region "データ更新処理"

    '***************************************************************************************************
    '   ＩＤ　：UpdateData
    '   名称　：データ更新処理
    '   概要  ：Update文を発行し、テーブルデータを更新する
    '   引数　：uData:テーブル登録用データ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma  新規作成
    '***************************************************************************************************
    Public Function UpdateData(ByVal uData As miData) As Boolean
        'Update用データ
        Dim data As miData = Nothing
        'SQL文
        Dim strSql As String = String.Empty
        'SQL実行結果
        Dim intRet As Integer = -1
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '処理結果
        Dim blnRet As Boolean = False

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            data = uData
            strSql = "UPDATE meeting_Information SET " &
                     "k_union = '" & data.strUnioncode & "' " &
                     ",k_information_type = '" & data.strInformationTypeCode & "' " &
                     ",l_information_name = '" & data.strInformationName & "' " &
                     ",d_meeting_1 = " & data.dtmMeeting1 &
                     ",d_meeting_time_from_1 = '" & data.strMeetingTimeFrom1 & "' " &
                     ",d_meeting_time_to_1 = '" & data.strMeetingTimeTo1 & "' " &
                     ",l_flight_to_1 = '" & data.strFlightTo1 & "' " &
                     ",d_flight_to_1 = '" & data.strFlightTimeTo1 & "' " &
                     ",l_flight_back_1 = '" & data.strFlightBack1 & "' " &
                     ",d_flight_back_1 = '" & data.strFlightTimeBack1 & "' " &
                     ",d_meeting_2 = " & data.dtmMeeting2 &
                     ",d_meeting_time_from_2 = '" & data.strMeetingTimeFrom2 & "' " &
                     ",d_meeting_time_to_2 = '" & data.strMeetingTimeTo2 & "' " &
                     ",d_meeting_3  = " & data.dtmMeeting3 &
                     ",d_meeting_time_from_3 = '" & data.strMeetingTimeFrom3 & "' " &
                     ",d_meeting_time_to_3 = '" & data.strMeetingTimeTo3 & "' " &
                     ",l_open_bebiginting = '" & data.strOpenBeBiginting & "' " &
                     ",l_place = '" & data.strPlace & "' " &
                     ",l_subject_1 = '" & data.strSubject1 & "' " &
                     ",l_subject_2 = '" & data.strSubject2 & "' " &
                     ",l_subject_3 = '" & data.strSubject3 & "' " &
                     ",l_subject_4 = '" & data.strSubject4 & "' " &
                     ",l_subject_5 = '" & data.strSubject5 & "' " &
                     ",l_biko_1 = '" & data.strBiko1 & "' " &
                     ",l_biko_2 = '" & data.strBiko2 & "' " &
                     ",l_biko_3 = '" & data.strBiko3 & "' " &
                     ",d_up = " & data.dtmUpdateDate &
                     ",c_user_id_up = '" & data.strUpdateUserId & "' " &
                     ",s_up = " & data.intUpdateTime &
                     " WHERE c_ksh = '" & _drMeetingInformation.Item("c_ksh") & "' " &
                     "AND k_apply_area = '" & _drMeetingInformation.Item("k_apply_area") & "' " &
                     "AND c_period_id = '" & _drMeetingInformation.Item("c_period_id") & "' " &
                     "AND c_meeting = '" & _drMeetingInformation.Item("c_meeting") & "' " &
                     "AND s_meeting = " & CInt(_drMeetingInformation.Item("s_meeting"))

            'DB接続&トランザクション開始
            clsMdb.Connect()
            clsMdb.BeginTran()

            'SQL実行
            intRet = clsMdb.ExecuteNonQuery(strSql)
            log.Debug(strSql)
            If intRet = 1 Then
                'コミット
                clsMdb.CommitTran()
                '正常終了を設定
                blnRet = True
                log.Info(String.Format("{0}件のデータを更新しました。", intRet.ToString()))
            Else
                '処理結果が1行でない場合はロールバック
                clsMdb.RollbackTran()
                log.Error("DB更新処理に異常があったためデータ更新を中止しました。")
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "UpdateData")
            'ロールバック
            clsMdb.RollbackTran()
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        '処理結果返却
        Return blnRet

    End Function
#End Region

#Region "一時保存データ登録処理"
    '***************************************************************************************************
    '   ＩＤ　：InsertTemporaryData
    '   名称　：データ登録処理
    '   概要  ：Insert文を発行し、テーブルへデータを登録する
    '   引数　：iData:テーブル登録用データ
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/24(木) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/24(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function InsertTemporaryData(ByVal iData As miData) As Boolean
        'Insert用データ
        Dim data As miData = Nothing
        'SQL文
        Dim strSql As String = String.Empty
        'SQL実行結果
        Dim intRet As Integer = -1
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '処理結果
        Dim blnRet As Boolean = False

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            data = iData
            'Insert文の作成
            strSql = "Insert Into meeting_information_temporary( " &
                     "c_ksh " &
                     ",k_apply_area " &
                     ",c_period_id " &
                     ",c_meeting " &
                     ",s_meeting " &
                     ",c_committee_id " &
                     ",k_union " &
                     ",k_information_type " &
                     ",l_information_name " &
                     ",d_meeting_1 " &
                     ",d_meeting_time_from_1 " &
                     ",d_meeting_time_to_1 " &
                     ",l_flight_to_1 " &
                     ",d_flight_to_1 " &
                     ",l_flight_back_1 " &
                     ",d_flight_back_1 " &
                     ",d_meeting_2 " &
                     ",d_meeting_time_from_2 " &
                     ",d_meeting_time_to_2 " &
                     ",l_flight_to_2 " &
                     ",d_flight_to_2 " &
                     ",l_flight_back_2 " &
                     ",d_flight_back_2 " &
                     ",d_meeting_3 " &
                     ",d_meeting_time_from_3 " &
                     ",d_meeting_time_to_3 " &
                     ",l_flight_to_3 " &
                     ",d_flight_to_3 " &
                     ",l_flight_back_3 " &
                     ",d_flight_back_3 " &
                     ",l_open_bebiginting " &
                     ",l_place " &
                     ",l_subject_1 " &
                     ",l_subject_2 " &
                     ",l_subject_3 " &
                     ",l_subject_4 " &
                     ",l_subject_5 " &
                     ",l_biko_1 " &
                     ",l_biko_2 " &
                     ",l_biko_3 " &
                     ",d_ins " &
                     ",c_user_id_ins " &
                     ",d_up " &
                     ",c_user_id_up " &
                     ",s_up " &
                     ") VALUES( " &
                     "'" & data.strKsh & "' " &
                     ",'" & data.strApplyAreaCode & "' " &
                     ",'" & data.strPeriodId & "' " &
                     ",'" & data.strMeetingNumber & "' " &
                     "," & data.intSeq &
                     ",'" & data.strCommitteeId & "' " &
                     ",'" & data.strUnioncode & "' " &
                     ",'" & data.strInformationTypeCode & "' " &
                     ",'" & data.strInformationName & "' " &
                     "," & data.dtmMeeting1 &
                     ",'" & data.strMeetingTimeFrom1 & "' " &
                     ",'" & data.strMeetingTimeTo1 & "' " &
                     ",'" & data.strFlightTo1 & "' " &
                     ",'" & data.strFlightTimeTo1 & "' " &
                     ",'" & data.strFlightBack1 & "' " &
                     ",'" & data.strFlightTimeBack1 & "' " &
                     "," & data.dtmMeeting2 &
                     ",'" & data.strMeetingTimeFrom2 & "' " &
                     ",'" & data.strMeetingTimeTo2 & "' " &
                     ",'" & data.strFlightTo2 & "' " &
                     ",'" & data.strFlightTimeTo2 & "' " &
                     ",'" & data.strFlightBack2 & "' " &
                     ",'" & data.strFlightTimeBack2 & "' " &
                     "," & data.dtmMeeting3 &
                     ",'" & data.strMeetingTimeFrom3 & "' " &
                     ",'" & data.strMeetingTimeTo3 & "' " &
                     ",'" & data.strFlightTo3 & "' " &
                     ",'" & data.strFlightTimeTo3 & "' " &
                     ",'" & data.strFlightBack3 & "' " &
                     ",'" & data.strFlightTimeBack3 & "' " &
                     ",'" & data.strOpenBeBiginting & "' " &
                     ",'" & data.strPlace & "' " &
                     ",'" & data.strSubject1 & "' " &
                     ",'" & data.strSubject2 & "' " &
                     ",'" & data.strSubject3 & "' " &
                     ",'" & data.strSubject4 & "' " &
                     ",'" & data.strSubject5 & "' " &
                     ",'" & data.strBiko1 & "' " &
                     ",'" & data.strBiko2 & "' " &
                     ",'" & data.strBiko3 & "' " &
                     "," & data.dtmInsertDate &
                     ",'" & data.strInsertUserId & "' " &
                     "," & data.dtmUpdateDate &
                     ",'" & data.strUpdateUserId & "' " &
                     "," & data.intUpdateTime & ")"

            'DB接続&トランザクション開始
            clsMdb.Connect()
            clsMdb.BeginTran()

            'SQL実行
            intRet = clsMdb.ExecuteNonQuery(strSql)
            log.Debug(strSql)
            If intRet = 1 Then
                'コミット
                clsMdb.CommitTran()
                blnRet = True
                log.Info(String.Format("{0}件のデータを追加しました。", intRet.ToString()))
            Else
                '処理結果が1行でない場合はロールバック
                clsMdb.RollbackTran()
                log.Error("DB更新処理に異常があったためデータ追加を中止しました。")
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "InsertTemporaryData")
            'ロールバック
            clsMdb.RollbackTran()
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return blnRet
    End Function

#End Region

#Region "データ更新処理"
    '***************************************************************************************************
    '   ＩＤ　：UpdateTemporaryData
    '   名称　：一時保存データ更新処理
    '   概要  ：Update文を発行し、テーブルデータを更新する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma  新規作成
    '***************************************************************************************************
    Public Function UpdateTemporaryData(ByVal uData As miData) As Boolean
        'Update用データ
        Dim data As miData = Nothing
        'SQL文
        Dim strSql As String = String.Empty
        'SQL実行結果
        Dim intRet As Integer = -1
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '処理結果
        Dim blnRet As Boolean = False

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            data = uData
            strSql = "UPDATE meeting_Information_temporary SET " &
                     "k_apply_area = '" & data.strApplyAreaCode & "' " &
                     ",c_committee_id = '" & data.strCommitteeId & "' " &
                     ",k_union = '" & data.strUnioncode & "' " &
                     ",k_information_type = '" & data.strInformationTypeCode & "' " &
                     ",l_information_name = '" & data.strInformationName & "' " &
                     ",d_meeting_1 = " & data.dtmMeeting1 &
                     ",d_meeting_time_from_1 = '" & data.strMeetingTimeFrom1 & "' " &
                     ",d_meeting_time_to_1 = '" & data.strMeetingTimeTo1 & "' " &
                     ",l_flight_to_1 = '" & data.strFlightTo1 & "' " &
                     ",d_flight_to_1 = '" & data.strFlightTimeTo1 & "' " &
                     ",l_flight_back_1 = '" & data.strFlightBack1 & "' " &
                     ",d_flight_back_1 = '" & data.strFlightTimeBack1 & "' " &
                     ",d_meeting_2 = " & data.dtmMeeting2 &
                     ",d_meeting_time_from_2 = '" & data.strMeetingTimeFrom2 & "' " &
                     ",d_meeting_time_to_2 = '" & data.strMeetingTimeTo2 & "' " &
                     ",d_meeting_3  = " & data.dtmMeeting3 &
                     ",d_meeting_time_from_3 = '" & data.strMeetingTimeFrom3 & "' " &
                     ",d_meeting_time_to_3 = '" & data.strMeetingTimeTo3 & "' " &
                     ",l_open_bebiginting = '" & data.strOpenBeBiginting & "' " &
                     ",l_place = '" & data.strPlace & "' " &
                     ",l_subject_1 = '" & data.strSubject1 & "' " &
                     ",l_subject_2 = '" & data.strSubject2 & "' " &
                     ",l_subject_3 = '" & data.strSubject3 & "' " &
                     ",l_subject_4 = '" & data.strSubject4 & "' " &
                     ",l_subject_5 = '" & data.strSubject5 & "' " &
                     ",l_biko_1 = '" & data.strBiko1 & "' " &
                     ",l_biko_2 = '" & data.strBiko2 & "' " &
                     ",l_biko_3 = '" & data.strBiko3 & "' " &
                     ",d_up = " & data.dtmUpdateDate &
                     ",c_user_id_up = '" & data.strUpdateUserId & "' " &
                     ",s_up = " & data.intUpdateTime &
                     " WHERE c_ksh = '" & _drMeetingInformation.Item("c_ksh") & "' " &
                     " AND index = " & _drMeetingInformation.Item("index") &
                     " AND k_apply_area = '" & _drMeetingInformation.Item("k_apply_area") & "' " &
                     " AND c_period_id = '" & _drMeetingInformation.Item("c_period_id") & "' "

            'DB接続&トランザクション開始
            clsMdb.Connect()
            clsMdb.BeginTran()

            'SQL実行
            intRet = clsMdb.ExecuteNonQuery(strSql)
            log.Debug(strSql)
            If intRet = 1 Then
                'コミット
                clsMdb.CommitTran()
                '正常終了を設定
                blnRet = True
                log.Info(String.Format("{0}件のデータを更新しました。", intRet.ToString()))
            Else
                '処理結果が1行でない場合はロールバック
                clsMdb.RollbackTran()
                log.Error("DB更新処理に異常があったためデータ更新を中止しました。")
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "UpdateTemporaryData")
            'ロールバック
            clsMdb.RollbackTran()
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        '処理結果返却
        Return blnRet

    End Function
#End Region

#Region "一時保存削除処理"
    Private Function DeleteTemporaryData() As Boolean
        'SQL文
        Dim strSql As String = String.Empty
        'SQL実行結果
        Dim intRet As Integer = -1
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        '処理結果
        Dim blnRet As Boolean = False

        Try
            ' 削除処理
            strSql = strSql & "DELETE " & vbCrLf
            strSql = strSql & "FROM meeting_information_temporary " & vbCrLf
            strSql = strSql & "WHERE index = " & _drMeetingInformation.Item("index") & vbCrLf

            clsMdb.Connect() 'DB接続
            clsMdb.BeginTran() 'トランザクション開始

            intRet = clsMdb.ExecuteNonQuery(strSql)
            log.Debug(strSql)

            If intRet <> 1 Then
                '処理結果が１行以外のときはロールバック
                clsMdb.RollbackTran()
            Else
                blnRet = True
                clsMdb.CommitTran() 'コミット
            End If

            Return blnRet
        Catch ex As Exception
            clsMdb.RollbackTran()
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "DeleteTemporaryData")
            log.Fatal(ex.Message)
        Finally
            clsMdb.Disconnect() '接続終了
        End Try
    End Function

#End Region

#Region "会議通知番号取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetMeetingNumber
    '   名称　：会議通知番号取得処理
    '   概要  ：テーブル登録用のシーケンス番号を設定する
    '   引数　：blnWork:True：一時保存、False：本登録
    '   戻り値：登録用のシーケンス番号
    '   作成日：2011/11/25(金) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/25(金) a.onuma  新規作成
    '***************************************************************************************************
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

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        If blnWork = True Then
            '一時保存時は新規会議通知番号は発行しない
            If IntClickBtnFlg <> 0 Then
                '新規作成以外の場合、既に登録済みの会議通知番号を利用する
                strNewMeetingNumber = _drMeetingInformation.Item("s_meeting")
            End If

            Return strNewMeetingNumber
        End If

        Try
            If Me.cboshibu.Text <> String.Empty Then
                strSelectedAreaCode = _dicBranch(Me.cboshibu.Text)
            End If
            strSql = "SELECT MAX(s_meeting) FROM meeting_information " &
                     "WHERE c_meeting LIKE'" & MDLoginInfo.Period & "%' " &
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
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        '取得した最大番号を返却
        Return strNewMeetingNumber
    End Function

#End Region

#Region "備考のリスト取得"
    Private Function GetNoteList() As List(Of String)
        Dim noteList As List(Of String) = New List(Of String)
        For Each strNote As String In Me.txtNote.Lines()
            If ChkNull(strNote.Trim()) = False Then
                noteList.Add(strNote)
            End If
        Next
        Return noteList
    End Function

#End Region

#Region "会議通知番号取得"
    '***************************************************************************************************
    '   ＩＤ　：GetMaxMeetingNumber
    '   名称　：会議通知番号取得
    '   概要  ：登録処理に応じた会議通知番号を返却する
    '   引数　：blnWork:True = 一時保存、False = 通常保存
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/12(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/12(月) a.onuma  新規作成
    '         ：2013/04/19(金) Fujisaku  変更 Sequenceテキスト使用対応
    '***************************************************************************************************
    Private Function GetMaxMeetingNumber(ByVal blnWork As Boolean) As String
        Dim strMeetingNumber As String = String.Empty
        Dim strSql As String = String.Empty
        Dim clsMdb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing
        Dim intMaxNumberDb As Integer = 0
        Dim intMaxNumberText As Integer = 0

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If blnWork = True Then
                '一時保存時は新規会議通知番号は発行しない
                Return intMaxNumberDb.ToString
            End If

            'If IntClickBtnFlg = 1 OrElse IntClickBtnFlg = 2 Then
            '    '変更または中止登録の場合、既に登録済みの会議通知番号を利用する
            '    intMaxNumber = CInt(_drMeetingInformation.Item("c_meeting").ToString.Replace(MDLoginInfo.Period.ToString & "-", ""))
            '    Return intMaxNumber.ToString
            'End If

            strSql = "SELECT c_meeting FROM meeting_information " &
                     " WHERE k_apply_area = '" & _dicBranch(Me.cboshibu.Text) & "' " &
                     " AND c_period_id = '" & MDLoginInfo.PeriodId & "' "

            'DB接続開始
            clsMdb.Connect()
            dtRet = clsMdb.ExecuteSql(strSql)

            If dtRet.Rows.Count > 0 Then
                For Each row As DataRow In dtRet.Rows
                    If intMaxNumberDb < CInt(row.Item("c_meeting").ToString.Replace("-", "")) Then
                        intMaxNumberDb = CInt(row.Item("c_meeting").ToString.Replace("-", ""))
                    End If
                Next

                '2012/06/29 Replace結果がNullの場合の処理を追加
                If (String.IsNullOrEmpty(intMaxNumberDb.ToString.Replace(MDLoginInfo.Period.ToString, "")) = True) Then
                    intMaxNumberDb = CInt(MDLoginInfo.Period.ToString) + 1
                Else
                    intMaxNumberDb = CInt(intMaxNumberDb.ToString.Replace(MDLoginInfo.Period.ToString, "")) + 1
                End If
            End If

            ' TEXTから最新番号取得
            Dim strSeqName As String = "seq_met.txt"
            If _dicBranch(Me.cboshibu.Text) = "01" Then
                strSeqName = "seq_met_tyo_" + MDLoginInfo.PeriodId + ".txt"
            ElseIf _dicBranch(Me.cboshibu.Text) = "02" Then
                strSeqName = "seq_met_osa_" + MDLoginInfo.PeriodId + ".txt"
            End If
            Try
                Dim sr = New System.IO.StreamReader(MDSystemInfo.SequencePath + strSeqName)
                Dim s As String = sr.ReadToEnd
                sr.Close()
                intMaxNumberText = CInt(s) + 1
            Catch ex As System.IO.FileNotFoundException
                intMaxNumberText = 1
            Catch ex As System.InvalidCastException
                intMaxNumberText = 1
            End Try

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

            ' 値の大きいほうを採用
            If intMaxNumberText >= intMaxNumberDb Then
                Return intMaxNumberText.ToString
            Else
                Return intMaxNumberDb.ToString
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "GetMaxMeetingnumber")
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try

        Return strMeetingNumber
    End Function
#End Region

#Region "シーケンス番号取得"
    '***************************************************************************************************
    '   ＩＤ　：GetSequenceNumber
    '   名称　：シーケンス番号取得
    '   概要  ：同一会議通知番号内のシーケンス番号を取得する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/12(月) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/12(月) a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetSequenceNumber(ByVal blnWork As Boolean) As Integer
        Dim strSql As String = String.Empty
        Dim clsMdb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = Nothing
        Dim intSeq As Integer = 0

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            If IntClickBtnFlg = 0 OrElse IntClickBtnFlg = 3 Then
                '新規保存時
                If blnWork = True Then
                    '一時保存時は0を返却
                    Return intSeq
                Else
                    '本登録時は1を返却
                    Return intSeq + 1
                End If
            End If

            strSql = "SELECT Max(s_meeting) As MaxSeq FROM meeting_information " &
                     "WHERE k_apply_area = '" & _drMeetingInformation.Item("k_apply_area") & "' " &
                     "AND c_period_id = '" & MDLoginInfo.PeriodId & "' " &
                     "AND c_meeting = '" & _drMeetingInformation.Item("c_meeting") & "' "

            'DB接続開始
            clsMdb.Connect()
            dtRet = clsMdb.ExecuteSql(strSql)

            If dtRet.Rows.Count > 0 Then
                If dtRet.Rows(0).Item("MaxSeq") IsNot DBNull.Value Then
                    intSeq = dtRet.Rows(0).Item("MaxSeq")
                End If
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
            Return intSeq + 1

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "GetSequenceNumber")
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
    End Function
#End Region

#Region "電話番号・FAX番号取得処理"
    '***************************************************************************************************
    '   ＩＤ　：GetTelFaxNumber
    '   名称　：電話番号・FAX番号取得処理
    '   概要  ：テーブルより電話番号、FAX番号を取得する
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/30(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/30(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Function GetTelFaxNumber() As TelFax
        '処理結果
        Dim telFaxList As TelFax = New TelFax()
        Dim tbRet As DataTable = Nothing
        'SQL文
        Dim strSql As String = String.Empty
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strSql = "SELECT l_name,l_omission_name,l_omission_name_2 FROM constant_dtl " &
                     "WHERE c_constant = 'TEL_INFO' "

            'DB接続開始
            clsMdb.Connect()

            tbRet = clsMdb.ExecuteSql(strSql)
            log.Debug(strSql)
            If tbRet.Rows.Count > 0 Then
                For Each row As DataRow In tbRet.Rows
                    If row.Item(0) = "東京" Then
                        telFaxList.strTokyoTel = row.Item(1)
                        telFaxList.strTokyoFax = row.Item(2)
                    ElseIf row.Item(0) = "大阪" Then
                        telFaxList.strOsakaTel = row.Item(1)
                        telFaxList.strOsakaFax = row.Item(2)
                    End If

                Next
            End If

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "GetTelFaxNumber")
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Return telFaxList

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
        Dim colorStartTime1 As Color = Color.White
        Dim colorEndTime1 As Color = Color.White
        Dim colorStartTime2 As Color = Color.White
        Dim colorEndTime2 As Color = Color.White
        Dim colorStartTime3 As Color = Color.White
        Dim colorEndTime3 As Color = Color.White
        Dim colorGoTime As Color = Color.White
        Dim colorReturnTime As Color = Color.White

        Dim strErrorNumber As String = String.Empty

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        If errMsg.Count > 0 Then
            errMsg.Clear()
        End If

        Try
            '開催日付１の開始時刻チェック
            strErrorNumber = ChkMeetingTime(Me.mtxStartTime1)
            If strErrorNumber <> String.Empty Then
                errMsg.Add(CLMsg.GetMsg(strErrorNumber, "開始日開始時刻1"))
            End If
            'If Me.mtxStartTime1.ValidateText Is Nothing Then
            '    If ChkNumber(Me.mtxStartTime1.Text.Substring(0, 2)) = False AndAlso _
            '           ChkNumber(Me.mtxStartTime1.Text.Substring(3, 2)) = False Then
            '    Else
            '        If Me.mtxStartTime1.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
            '            errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻1"))
            '            colorStartTime1 = Color.Orange
            '        End If

            '        If colorStartTime1 = Color.White Then
            '            If ChkNumber(Me.mtxStartTime1.Text.Substring(0, 2)) = True Then
            '                If CInt(Me.mtxStartTime1.Text.Substring(0, 2)) > 23 Then
            '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻1"))
            '                    colorStartTime1 = Color.Orange
            '                End If
            '            End If
            '        End If

            '        If colorStartTime1 = Color.White Then
            '            If ChkNumber(Me.mtxStartTime1.Text.Substring(3, 2)) = True Then
            '                If CInt(Me.mtxStartTime1.Text.Substring(3, 2)) > 59 Then
            '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻1"))
            '                    colorStartTime1 = Color.Orange
            '                End If
            '            End If
            '        End If

            '        If colorStartTime1 = Color.White Then
            '            If ChkNumber(Me.mtxStartTime1.Text.Substring(0, 2)) = False OrElse _
            '               ChkNumber(Me.mtxStartTime1.Text.Substring(3, 2)) = False Then
            '                errMsg.Add(CLMsg.GetMsg("GE0006", "開始日開始時刻1"))
            '                colorStartTime1 = Color.Pink
            '            End If
            '        End If
            '    End If
            'Else
            '    If Me.mtxStartTime1.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Length < 4 Then
            '        errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻1"))
            '        colorStartTime1 = Color.Orange
            '    End If
            'End If
            'Me.mtxStartTime1.BackColor = colorStartTime1

            strErrorNumber = ChkMeetingTime(Me.mtxEndTime1)
            If strErrorNumber <> String.Empty Then
                errMsg.Add(CLMsg.GetMsg(strErrorNumber, "開始日終了時刻1"))
            End If
            '開催日付１の終了時刻チェック
            'If Me.mtxEndTime1.ValidateText Is Nothing Then
            '    If ChkNumber(Me.mtxEndTime1.Text.Substring(0, 2)) = False AndAlso _
            '           ChkNumber(Me.mtxEndTime1.Text.Substring(3, 2)) = False Then
            '    Else
            '        If Me.mtxEndTime1.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
            '            errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻1"))
            '            colorEndTime1 = Color.Orange
            '        End If

            '        If colorEndTime1 = Color.White Then
            '            If ChkNumber(Me.mtxEndTime1.Text.Substring(0, 2)) = True Then
            '                If CInt(Me.mtxEndTime1.Text.Substring(0, 2)) > 23 Then
            '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻1"))
            '                    colorEndTime1 = Color.Orange
            '                End If
            '            End If
            '        End If

            '        If colorEndTime1 = Color.White Then
            '            If ChkNumber(Me.mtxEndTime1.Text.Substring(3, 2)) = True Then
            '                If CInt(Me.mtxEndTime1.Text.Substring(3, 2)) > 59 Then
            '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻1"))
            '                    colorEndTime1 = Color.Orange
            '                End If
            '            End If
            '        End If

            '        If colorEndTime1 = Color.White Then
            '            If ChkNumber(Me.mtxEndTime1.Text.Substring(0, 2)) = False OrElse _
            '               ChkNumber(Me.mtxEndTime1.Text.Substring(3, 2)) = False Then
            '                errMsg.Add(CLMsg.GetMsg("GE0006", "開始日終了時刻1"))
            '                colorEndTime1 = Color.Pink
            '            End If
            '        End If
            '    End If
            'Else
            '    If Me.mtxEndTime1.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
            '        errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻1"))
            '        colorEndTime1 = Color.Orange
            '    End If
            'End If
            'Me.mtxEndTime1.BackColor = colorEndTime1

            If Me.chkDate2.CheckState = CheckState.Checked Then
                '開催日付２の開始時刻チェック
                strErrorNumber = ChkMeetingTime(Me.mtxStartTime2)
                If strErrorNumber <> String.Empty Then
                    errMsg.Add(CLMsg.GetMsg(strErrorNumber, "開始日開始時刻2"))
                End If
                'If Me.mtxStartTime2.ValidateText Is Nothing Then
                '    If ChkNumber(Me.mtxStartTime2.Text.Substring(0, 2)) = False AndAlso _
                '       ChkNumber(Me.mtxStartTime2.Text.Substring(3, 2)) = False Then
                '    Else
                '        If Me.mtxStartTime2.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '            errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻2"))
                '            colorStartTime2 = Color.Orange
                '        End If

                '        If colorStartTime2 = Color.White Then
                '            If ChkNumber(Me.mtxStartTime2.Text.Substring(0, 2)) = True Then
                '                If CInt(Me.mtxStartTime2.Text.Substring(0, 2)) > 23 Then
                '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻2"))
                '                    colorStartTime2 = Color.Orange
                '                End If
                '            End If
                '        End If

                '        If colorStartTime2 = Color.White Then
                '            If ChkNumber(Me.mtxStartTime2.Text.Substring(3, 2)) = True Then
                '                If CInt(Me.mtxStartTime2.Text.Substring(3, 2)) > 59 Then
                '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻2"))
                '                    colorStartTime2 = Color.Orange
                '                End If
                '            End If
                '        End If

                '        If colorStartTime2 = Color.White Then
                '            If ChkNumber(Me.mtxStartTime2.Text.Substring(0, 2)) = False OrElse _
                '               ChkNumber(Me.mtxStartTime2.Text.Substring(3, 2)) = False Then
                '                errMsg.Add(CLMsg.GetMsg("GE0006", "開始日開始時刻2"))
                '                colorStartTime2 = Color.Pink
                '            End If
                '        End If
                '    End If
                'Else
                '    If Me.mtxStartTime2.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '        errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻2"))
                '        colorStartTime2 = Color.Orange
                '    End If
                'End If
                'Me.mtxStartTime2.BackColor = colorStartTime2

                ''開催日付２の終了時刻チェック
                strErrorNumber = ChkMeetingTime(Me.mtxEndTime2)
                If strErrorNumber <> String.Empty Then
                    errMsg.Add(CLMsg.GetMsg(strErrorNumber, "開始日終了時刻2"))
                End If
                'If Me.mtxEndTime2.ValidateText Is Nothing Then
                '    If ChkNumber(Me.mtxEndTime2.Text.Substring(0, 2)) = False AndAlso _
                '       ChkNumber(Me.mtxEndTime2.Text.Substring(3, 2)) = False Then
                '    Else
                '        If Me.mtxEndTime2.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '            errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻2"))
                '            colorEndTime2 = Color.Orange
                '        End If

                '        If colorEndTime2 = Color.White Then
                '            If ChkNumber(Me.mtxEndTime2.Text.Substring(0, 2)) = True Then
                '                If CInt(Me.mtxEndTime2.Text.Substring(0, 2)) > 23 Then
                '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻2"))
                '                    colorEndTime2 = Color.Orange
                '                End If
                '            End If
                '        End If

                '        If colorEndTime2 = Color.White Then
                '            If ChkNumber(Me.mtxEndTime2.Text.Substring(3, 2)) = True Then
                '                If CInt(Me.mtxEndTime2.Text.Substring(3, 2)) > 59 Then
                '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻2"))
                '                    colorEndTime2 = Color.Orange
                '                End If
                '            End If
                '        End If

                '        If colorEndTime2 = Color.White Then
                '            If ChkNumber(Me.mtxEndTime2.Text.Substring(0, 2)) = False OrElse _
                '               ChkNumber(Me.mtxEndTime2.Text.Substring(3, 2)) = False Then
                '                errMsg.Add(CLMsg.GetMsg("GE0006", "開始日終了時刻2"))
                '                colorStartTime2 = Color.Pink
                '            End If
                '        End If
                '    End If
                'Else
                '    If Me.mtxEndTime2.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '        errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻2"))
                '        colorEndTime2 = Color.Orange
                '    End If
                'End If
                'Me.mtxEndTime2.BackColor = colorEndTime2
            End If

            If Me.chkDate3.CheckState = CheckState.Checked Then
                '開催日付３の開始時刻チェック
                strErrorNumber = ChkMeetingTime(Me.mtxStartTime3)
                If strErrorNumber <> String.Empty Then
                    errMsg.Add(CLMsg.GetMsg(strErrorNumber, "開始日開始時刻3"))
                End If
                'If Me.mtxStartTime3.ValidateText Is Nothing Then
                '    If ChkNumber(Me.mtxStartTime3.Text.Substring(0, 2)) = False AndAlso _
                '       ChkNumber(Me.mtxStartTime3.Text.Substring(3, 2)) = False Then
                '    Else

                '        If Me.mtxStartTime3.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '            errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻3"))
                '            colorStartTime3 = Color.Orange
                '        End If

                '        If colorStartTime3 = Color.White Then
                '            If ChkNumber(Me.mtxStartTime3.Text.Substring(0, 2)) = True Then
                '                If CInt(Me.mtxStartTime3.Text.Substring(0, 2)) > 23 Then
                '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻3"))
                '                    colorStartTime3 = Color.Orange
                '                End If
                '            End If
                '        End If

                '        If colorStartTime3 = Color.White Then
                '            If ChkNumber(Me.mtxStartTime3.Text.Substring(3, 2)) = True Then
                '                If CInt(Me.mtxStartTime3.Text.Substring(3, 2)) > 59 Then
                '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻3"))
                '                    colorStartTime3 = Color.Orange
                '                End If
                '            End If
                '        End If

                '        If colorStartTime3 = Color.White Then
                '            If ChkNumber(Me.mtxStartTime3.Text.Substring(0, 2)) = False OrElse _
                '               ChkNumber(Me.mtxStartTime3.Text.Substring(3, 2)) = False Then
                '                errMsg.Add(CLMsg.GetMsg("GE0006", "開始日開始時刻3"))
                '                colorStartTime3 = Color.Pink
                '            End If
                '        End If
                '    End If
                'Else
                '    If Me.mtxStartTime3.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '        errMsg.Add(CLMsg.GetMsg("GE0021", "開始日開始時刻3"))
                '        colorStartTime3 = Color.Orange
                '    End If
                'End If
                'Me.mtxStartTime3.BackColor = colorStartTime3

                '開催日付３の終了時刻チェック
                strErrorNumber = ChkMeetingTime(Me.mtxEndTime3)
                If strErrorNumber <> String.Empty Then
                    errMsg.Add(CLMsg.GetMsg(strErrorNumber, "開始日終了時刻3"))
                End If

                'If Me.mtxEndTime3.ValidateText Is Nothing Then

                '    If ChkNumber(Me.mtxEndTime3.Text.Substring(0, 2)) = False AndAlso _
                '       ChkNumber(Me.mtxEndTime3.Text.Substring(3, 2)) = False Then
                '    Else
                '        If Me.mtxEndTime3.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '            errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻3"))
                '            colorEndTime3 = Color.Orange
                '        End If

                '        If colorEndTime3 = Color.White Then
                '            If ChkNumber(Me.mtxEndTime3.Text.Substring(0, 2)) = True Then
                '                If CInt(Me.mtxEndTime3.Text.Substring(0, 2)) > 23 Then
                '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻3"))
                '                    colorEndTime3 = Color.Orange
                '                End If
                '            End If
                '        End If

                '        If colorEndTime3 = Color.White Then
                '            If ChkNumber(Me.mtxEndTime3.Text.Substring(3, 2)) = True Then
                '                If CInt(Me.mtxEndTime3.Text.Substring(3, 2)) > 59 Then
                '                    errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻3"))
                '                    colorEndTime3 = Color.Orange
                '                End If
                '            End If
                '        End If

                '        If colorEndTime3 = Color.White Then
                '            If ChkNumber(Me.mtxEndTime3.Text.Substring(0, 2)) = False OrElse _
                '               ChkNumber(Me.mtxEndTime3.Text.Substring(3, 2)) = False Then
                '                errMsg.Add(CLMsg.GetMsg("GE0006", "開始日終了時刻3"))
                '                colorEndTime3 = Color.Pink
                '            End If
                '        End If
                '    End If
                'Else
                '    If Me.mtxEndTime3.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '        errMsg.Add(CLMsg.GetMsg("GE0021", "開始日終了時刻3"))
                '        colorEndTime3 = Color.Orange
                '    End If
                'End If
                'Me.mtxEndTime3.BackColor = colorEndTime3
            End If

            If ChkNull(Me.txtGoMachineName.Text.Trim()) = False Then
                strErrorNumber = ChkMoveTime(Me.mtxGoTime)
                If strErrorNumber <> String.Empty Then
                    errMsg.Add(CLMsg.GetMsg(strErrorNumber, "往路の移動時刻"))
                End If

                '往路発時間時刻チェック
                'If Me.mtxGoTime.ValidateText Is Nothing Then
                '    If Me.mtxGoTime.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '        errMsg.Add(CLMsg.GetMsg("GE0021", "往路の移動時刻"))
                '        colorGoTime = Color.Orange
                '    End If

                '    If colorGoTime = Color.White Then
                '        If ChkNumber(Me.mtxGoTime.Text.Substring(0, 2)) = True Then
                '            If CInt(Me.mtxGoTime.Text.Substring(0, 2)) > 23 Then
                '                errMsg.Add(CLMsg.GetMsg("GE0021", "往路の移動時刻"))
                '                colorGoTime = Color.Orange
                '            End If
                '        End If
                '    End If

                '    If colorGoTime = Color.White Then
                '        If ChkNumber(Me.mtxGoTime.Text.Substring(3, 2)) = True Then
                '            If CInt(Me.mtxGoTime.Text.Substring(3, 2)) > 59 Then
                '                errMsg.Add(CLMsg.GetMsg("GE0021", "往路の移動時刻"))
                '                colorGoTime = Color.Orange
                '            End If
                '        End If
                '    End If

                '    If colorGoTime = Color.White Then
                '        If ChkNumber(Me.mtxGoTime.Text.Substring(0, 2)) = False OrElse _
                '           ChkNumber(Me.mtxGoTime.Text.Substring(3, 2)) = False Then
                '            errMsg.Add(CLMsg.GetMsg("GE0006", "往路の移動時刻"))
                '            colorGoTime = Color.Pink
                '        End If
                '    End If
                'Else
                '    If Me.mtxGoTime.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '        errMsg.Add(CLMsg.GetMsg("GE0021", "往路の移動時刻"))
                '        colorGoTime = Color.Orange
                '    End If
                'End If
            Else
                Me.mtxGoTime.BackColor = Color.White
                Me.mtxGoTime.Text = String.Empty
            End If

            If ChkNull(Me.txtReturnMachineName.Text.Trim()) = False Then
                strErrorNumber = ChkMoveTime(Me.mtxReturnTime)
                If strErrorNumber <> String.Empty Then
                    errMsg.Add(CLMsg.GetMsg(strErrorNumber, "復路の移動時刻"))
                End If

                '復路発時間時刻チェック
                'If Me.mtxReturnTime.ValidateText Is Nothing Then
                '    If Me.mtxReturnTime.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '        errMsg.Add(CLMsg.GetMsg("GE0021", "復路の移動時刻"))
                '        colorReturnTime = Color.Orange
                '    End If

                '    If colorReturnTime = Color.White Then
                '        If ChkNumber(Me.mtxReturnTime.Text.Substring(0, 2)) = True Then
                '            If CInt(Me.mtxReturnTime.Text.Substring(0, 2)) > 23 Then
                '                errMsg.Add(CLMsg.GetMsg("GE0021", "復路の移動時刻"))
                '                colorReturnTime = Color.Orange
                '            End If
                '        End If
                '    End If

                '    If colorReturnTime = Color.White Then
                '        If ChkNumber(Me.mtxReturnTime.Text.Substring(3, 2)) = True Then
                '            If CInt(Me.mtxReturnTime.Text.Substring(3, 2)) > 59 Then
                '                errMsg.Add(CLMsg.GetMsg("GE0021", "復路の移動時刻"))
                '                colorReturnTime = Color.Orange
                '            End If
                '        End If
                '    End If

                '    If colorReturnTime = Color.White Then
                '        If ChkNumber(Me.mtxReturnTime.Text.Substring(0, 2)) = False OrElse _
                '           ChkNumber(Me.mtxReturnTime.Text.Substring(3, 2)) = False Then
                '            errMsg.Add(CLMsg.GetMsg("GE0006", "復路の移動時刻"))
                '            colorReturnTime = Color.Pink
                '        End If
                '    End If
                'Else
                '    If Me.mtxReturnTime.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                '        errMsg.Add(CLMsg.GetMsg("GE0021", "復路の移動時刻"))
                '        colorReturnTime = Color.Orange
                '    End If
                'End If
            Else
                Me.mtxReturnTime.BackColor = colorReturnTime
                Me.mtxReturnTime.Text = String.Empty
            End If

            '支部
            If ChkNull(Me.cboshibu.Text) Then
                SetErr(Me.cboshibu)
                errMsg.Add(CLMsg.GetMsg("GE0006", "支部"))
            Else
                If Me.cboshibu.Enabled = True Then
                    Me.cboshibu.BackColor = Color.White
                Else
                    Me.cboshibu.BackColor = Color.Cornsilk
                End If
            End If

            '部／委員会
            If ChkNull(Me.CboCommittee.Text) Then
                SetErr(Me.CboCommittee)
                errMsg.Add(CLMsg.GetMsg("GE0006", "部／委員会"))
            Else
                If Me.CboCommittee.Enabled = True Then
                    Me.CboCommittee.BackColor = Color.White
                Else
                    Me.CboCommittee.BackColor = Color.Cornsilk
                End If
            End If

            '会議名
            If ChkNull(Me.cboMeetingName.Text) Then
                SetErr(Me.cboMeetingName)
                errMsg.Add(CLMsg.GetMsg("GE0006", "会議名"))
            Else
                If Me.cboMeetingName.Enabled = True Then
                    Me.cboMeetingName.BackColor = Color.White
                Else
                    Me.cboMeetingName.BackColor = Color.Cornsilk
                End If
            End If

            '開催場所
            If ChkNull(Me.cboMeetingPlace.Text) Then
                SetErr(Me.cboMeetingPlace)
                errMsg.Add(CLMsg.GetMsg("GE0006", "開催場所"))
            Else
                If Me.cboMeetingPlace.Enabled = True Then
                    Me.cboMeetingPlace.BackColor = Color.White
                Else
                    Me.cboMeetingPlace.BackColor = Color.Cornsilk
                End If
            End If

            If ChkNull(Me.dtpDate1.Text) Then
                SetErr(Me.dtpDate1)
                errMsg.Add(CLMsg.GetMsg("GE0006", "日時"))
            Else
                If Me.dtpDate1.Enabled = True Then
                    Me.dtpDate1.BackColor = Color.White
                Else
                    Me.dtpDate1.BackColor = Color.Cornsilk
                End If
            End If

            '中止の場合のみ備考もチェック
            'If IntClickBtnFlg = 2 Then
            '    If ChkNull(Me.txtNote.Text) Then
            '        SetErr(Me.txtNote)
            '        errMsg.Add(CLMsg.GetMsg("GE0006", "備考"))
            '    Else
            '        If Me.txtNote.Enabled = True Then
            '            Me.txtNote.BackColor = Color.White
            '        Else
            '            Me.txtNote.BackColor = Color.Cornsilk
            '        End If
            '    End If

            'End If

            'エラーメッセージが一つでも格納された場合、falseを返却
            If errMsg.Count > 0 Then
                blnRet = False
                ' エラーメッセージボックス表示
                Dim clsUC999999 As New UC999999
                clsUC999999.errMsgList = errMsg

                ' メインメニュー画面表示
                Call clsUC999999.ShowDialog()
            End If

            log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
            Return blnRet
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "ChkInput")
            log.Fatal(ex.Message)
        End Try

    End Function
#End Region

#Region "会議時間の不正チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkMeetingTime
    '   名称　：会議時間の不正チェック
    '   概要  ：会議の開始・終了に入力された時間が不正でないかチェックします
    '   引数　：各会議時間のテキストボックス
    '   戻り値：時間が不正：エラーナンバー、時間が正常：空文字
    '   作成日：2012/01/04(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/04(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkMeetingTime(ByVal mtxTarget As MaskedTextBox) As String
        Dim strErrNumber As String = String.Empty
        Dim colorTarget As Color = Color.White

        If mtxTarget.ValidateText Is Nothing Then
            If ChkNumber(mtxTarget.Text.Substring(0, 2)) = False AndAlso
               ChkNumber(mtxTarget.Text.Substring(3, 2)) = False Then
            Else
                If mtxTarget.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                    strErrNumber = "GE0021"
                    colorTarget = Color.Orange
                End If

                If colorTarget = Color.White Then
                    If ChkNumber(mtxTarget.Text.Substring(0, 2)) = True Then
                        If CInt(mtxTarget.Text.Substring(0, 2)) > 23 Then
                            strErrNumber = "GE0021"
                            colorTarget = Color.Orange
                        End If
                    End If
                End If

                If colorTarget = Color.White Then
                    If ChkNumber(mtxTarget.Text.Substring(3, 2)) = True Then
                        If CInt(mtxTarget.Text.Substring(3, 2)) > 59 Then
                            strErrNumber = "GE0021"
                            colorTarget = Color.Orange
                        End If
                    End If
                End If

                If colorTarget = Color.White Then
                    If ChkNumber(mtxTarget.Text.Substring(0, 2)) = False OrElse
                       ChkNumber(mtxTarget.Text.Substring(3, 2)) = False Then
                        strErrNumber = "GE0006"
                        colorTarget = Color.Pink
                    End If
                End If
            End If
        Else
            If mtxTarget.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                strErrNumber = "GE0021"
                colorTarget = Color.Orange
            End If
        End If
        mtxTarget.BackColor = colorTarget

        Return strErrNumber
    End Function
#End Region

#Region "移動時間の不正チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkMoveTime
    '   名称　：移動時間の不正チェック
    '   概要  ：往路・復路に入力された時間が不正でないかチェックします
    '   引数　：往路または復路のテキストボックス
    '   戻り値：時間が不正：エラーナンバー、時間が正常：空文字
    '   作成日：2012/01/04(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2012/01/04(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkMoveTime(ByVal mtxTarget As MaskedTextBox) As String
        Dim strErrNumber As String = String.Empty
        Dim colorTarget As Color = Color.White

        '発時間チェック
        If mtxTarget.ValidateText Is Nothing Then
            If mtxTarget.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                If mtxTarget.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length = 0 Then
                    strErrNumber = "GE0006"
                    colorTarget = Color.Pink
                Else
                    strErrNumber = "GE0021"
                    colorTarget = Color.Orange
                End If
            End If

            If colorTarget = Color.White Then
                If ChkNumber(mtxTarget.Text.Substring(0, 2)) = True Then
                    If CInt(mtxTarget.Text.Substring(0, 2)) > 23 Then
                        strErrNumber = "GE0021"
                        colorTarget = Color.Orange
                    End If
                End If
            End If

            If colorTarget = Color.White Then
                If ChkNumber(mtxTarget.Text.Substring(3, 2)) = True Then
                    If CInt(mtxTarget.Text.Substring(3, 2)) > 59 Then
                        strErrNumber = "GE0021"
                        colorTarget = Color.Orange
                    End If
                End If
            End If

            If colorTarget = Color.White Then
                If ChkNumber(mtxTarget.Text.Substring(0, 2)) = False OrElse _
                   ChkNumber(mtxTarget.Text.Substring(3, 2)) = False Then
                    strErrNumber = "GE0006"
                    colorTarget = Color.Pink
                End If
            End If
        Else
            If mtxTarget.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length < 4 Then
                If mtxTarget.Text.Replace("時", "").Replace("分", "").Replace(" ", "").Trim.Length = 0 Then
                    strErrNumber = "GE0006"
                    colorTarget = Color.Pink
                Else
                    strErrNumber = "GE0021"
                    colorTarget = Color.Orange
                End If
            End If
        End If
        mtxTarget.BackColor = colorTarget

        Return strErrNumber
    End Function
#End Region

#Region "入力文字列チェック"
    '***************************************************************************************************
    '   ＩＤ　：chkStringLength
    '   名称　：入力文字列チェック
    '   概要  ：入力文字列が適正な範囲内かチェックします
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Function chkStringLength() As Boolean
        '処理結果
        Dim blnRet As Boolean = True
        '文字列のバイト数調査用
        Dim encSjis As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            '会議名
            If Me.cboMeetingName.Text.Length > 20 Then
                blnRet = False
                CLMsg.Show("GE0073", "会議名", "20", "1")
                Return blnRet
            End If

            '開催場所
            If encSjis.GetByteCount(Me.cboMeetingPlace.Text) > 14 Then
                blnRet = False
                CLMsg.Show("GE0073", "開催場所", "全角7文字、半角14", "1")
                Return blnRet
            End If

            '会議場
            If Me.txtMeetingPlace.Text.Length > 30 Then
                blnRet = False
                CLMsg.Show("GE0073", "会議場", "30", "1")
                Return blnRet
            End If

            '備考
            If Me.txtNote.Lines().Length > 0 Then
                '空白を除いた備考のリスト取得
                Dim strNoteList As List(Of String) = GetNoteList()

                If strNoteList.Count > 3 Then
                    '入力行数が3行より多い場合はエラーとする
                    blnRet = False
                    CLMsg.Show("GE0073", "備考", "35", "3")
                    Return blnRet
                Else
                    For Each strLine As String In strNoteList
                        If strLine.Length > 35 Then
                            blnRet = False
                            CLMsg.Show("GE0073", "備考", "35", "3")
                            Return blnRet
                        End If
                    Next
                End If

            End If

            '議題１
            If Me.txtTheme1.Text.Length > 35 Then
                blnRet = False
                CLMsg.Show("GE0073", "議題1", "35", "1")
                Return blnRet
            End If

            '議題２
            If Me.txtTheme2.Text.Length > 35 Then
                blnRet = False
                CLMsg.Show("GE0073", "議題2", "35", "1")
                Return blnRet
            End If

            '議題３
            If Me.txtTheme3.Text.Length > 35 Then
                blnRet = False
                CLMsg.Show("GE0073", "議題3", "35", "1")
                Return blnRet
            End If

            '議題４
            If Me.txtTheme4.Text.Length > 35 Then
                blnRet = False
                CLMsg.Show("GE0073", "議題4", "35", "1")
                Return blnRet
            End If

            '議題５
            If Me.txtTheme5.Text.Length > 35 Then
                blnRet = False
                CLMsg.Show("GE0073", "議題5", "35", "1")
                Return blnRet
            End If
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "chkStringLength")
            log.Fatal(ex.Message)
            blnRet = False
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return blnRet
    End Function

#End Region

#Region "開始時間の整合性チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkTime
    '   名称　：開始時間の整合性チェック
    '   概要  ：開始時間と終了時間の入力がある場合、整合性が取れているかチェックする
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/22(木) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/22(木) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkTime() As Boolean
        Dim blnRet As Boolean = True

        If Me.mtxStartTime1.ValidateText IsNot Nothing Then
            If Me.mtxEndTime1.ValidateText IsNot Nothing Then
                If Me.mtxStartTime1.ValidateText > Me.mtxEndTime1.ValidateText Then
                    '日付1の開始・終了時間チェック
                    CLMsg.Show("GI0009")
                    blnRet = False
                    Return blnRet
                End If
            End If
        End If

        If Me.mtxStartTime2.ValidateText IsNot Nothing Then
            If Me.mtxEndTime2.ValidateText IsNot Nothing Then
                If Me.mtxStartTime2.ValidateText > Me.mtxEndTime2.ValidateText Then
                    '日付2の開始・終了時間チェック
                    CLMsg.Show("GI0009")
                    blnRet = False
                    Return blnRet
                End If
            End If
        End If

        If Me.mtxStartTime3.ValidateText IsNot Nothing Then
            If Me.mtxEndTime3.ValidateText IsNot Nothing Then
                If Me.mtxStartTime3.ValidateText > Me.mtxEndTime3.ValidateText Then
                    '日付3の開始・終了時間チェック
                    CLMsg.Show("GI0009")
                    blnRet = False
                    Return blnRet
                End If
            End If
        End If

        Return blnRet
    End Function

#End Region

#Region "開始開催日付の整合性チェック"
    Private Function ChkDateExact() As Boolean
        Dim blnRet As Boolean = True
        If chkDate2.CheckState = CheckState.Checked Then
            If Me.dtpDate1.Value.Date >= Me.dtpDate2.Value.Date Then
                CLMsg.Show("GE0032", "日時1", "日時2")
                Return False
            End If
        End If

        If Me.chkDate3.CheckState = CheckState.Checked Then
            If Me.chkDate2.CheckState = CheckState.Checked Then
                If Me.dtpDate2.Value.Date >= Me.dtpDate3.Value.Date Then
                    CLMsg.Show("GE0032", "日時2", "日時3")
                    Return False
                End If
            End If

            If Me.dtpDate1.Value.Date >= Me.dtpDate3.Value.Date Then
                CLMsg.Show("GE0032", "日時1", "日時3")
                Return False
            End If
        End If

        Return blnRet
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

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        If optOpen.Checked = True Then
            strInformationType = INFORMATION_TYPE_OPEN
        ElseIf optChange.Checked = True Then
            strInformationType = INFORMATION_TYPE_CHANGE
        Else
            strInformationType = INFORMATION_TYPE_STOP
        End If
        Try
            strSql = " SELECT meeting_information.c_meeting FROM meeting_information " & _
                 " WHERE k_apply_area = '" & _dicBranch(Me.cboshibu.Text) & "' " & _
                 " AND k_union = '" & UNION_ON & "' " & _
                 " AND c_committee_id = '" & Me.CboCommittee.SelectedValue() & "' " & _
                 " AND k_information_type = '" & strInformationType & "' " & _
                 " AND l_information_name = '" & Me.cboMeetingName.Text.Trim & "' " & _
                 " AND l_open_bebiginting = '" & Me.cboMeetingPlace.Text.Trim & "' "

            'DB接続開始
            clsMdb.Connect()

            tbRet = clsMdb.ExecuteSql(strSql)
            log.Debug(strSql)
            If tbRet.Rows.Count > 0 Then
                'データが存在する場合、同一会議存在フラグをたてる
                blnRet = False
            End If

            Return blnRet

        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "chkSameBranchMeeting")
        Finally
            clsMdb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")
        Return blnRet

    End Function

#End Region

#Region "同一会議通知存在チェック"
    '***************************************************************************************************
    '   ＩＤ　：chkSameMeeting
    '   名称　：同一会議通知存在チェック
    '   概要  ：同一支部、同一委員会、同一開催日の会議通知が存在するかチェックします
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/11/22(火) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/11/22(火) a.onuma  新規作成
    '***************************************************************************************************
    Private Function chkSameMeeting() As String
        '処理結果
        Dim strMeetingNumber As String = String.Empty
        Dim clsMdb As CLAccessMdb = New CLAccessMdb()
        'Select文
        Dim strSql As String = String.Empty
        '処理結果格納
        Dim tbRet As DataTable = Nothing

        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理開始")
        Try
            strSql = " SELECT meeting_information.c_meeting FROM meeting_information " &
                 " WHERE k_apply_area = '" & _dicBranch(Me.cboshibu.Text) & "' " &
                 " AND c_committee_id = '" & Me.CboCommittee.SelectedValue() & "' " &
                 " AND d_meeting_1 = '" & Me.dtpDate1.Value.Date().ToString() & "' "

            If IntClickBtnFlg <> 0 Then
                If _drMeetingInformation.Item("c_meeting").Equals(DBNull.Value) = False Then
                    strSql = strSql & " AND c_meeting <> '" & _drMeetingInformation.Item("c_meeting") & "' "

                End If
            End If

            'DB接続開始
            clsMdb.Connect()

            tbRet = clsMdb.ExecuteSql(strSql)
            log.Debug(strSql)
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
            log.Fatal(ex.Message)
        Finally
            clsMdb.Disconnect()
        End Try
        log.Info(System.Reflection.MethodInfo.GetCurrentMethod.Name() & " 処理終了")

        Return strMeetingNumber
    End Function

#Region "会議通知の終了チェック"
    '***************************************************************************************************
    '   ＩＤ　：ChkMeetingFinish
    '   名称　：会議通知の終了チェック
    '   概要  ：会議通知が終了していないか（一時保存から上書き可能か）チェックします
    '   引数　：なし
    '   戻り値：True = 正常, False = 異常
    '   作成日：2011/12/14(水) a.onuma
    '   更新日：
    '---------------------------------------------------------------------------------------------------
    '   履歴　：2011/12/14(水) a.onuma  新規作成
    '***************************************************************************************************
    Private Function ChkMeetingFinish() As Boolean
        Dim blnRet As Boolean = False
        Dim clsMdb As CLAccessMdb = New CLAccessMdb
        Dim dtRet As DataTable = New DataTable
        Dim strSql As String = String.Empty

        Try
            '同一会議番号の最大の開催開始日付を取得
            strSql = "SELECT MAX(d_meeting_1) FROM meeting_information " & _
                     "WHERE c_meeting ='" & _drMeetingInformation.Item("c_meeting") & "' " & _
                     "AND k_apply_area ='" & _drMeetingInformation.Item("k_apply_area") & "' "

            'DB接続
            clsMdb.Connect()

            dtRet = clsMdb.ExecuteSql(strSql)

            If dtRet.Rows(0).Item(0) IsNot DBNull.Value Then
                '会議通知が既に終了した日付かチェック
                If dtRet.Rows(0).Item(0) < System.DateTime.Now().Date Then
                    'CLMsg.Show("GE0217", INFORMATION_TYPE_STOP_STRING)
                    Return blnRet
                End If
            End If

            '同一会議番号の開催種類を取得
            strSql = "SELECT k_information_type FROM meeting_information " & _
                     "WHERE c_meeting = '" & _drMeetingInformation.Item("c_meeting") & "' " & _
                     "AND k_apply_area = '" & _drMeetingInformation.Item("k_apply_area") & "' "

            dtRet = clsMdb.ExecuteSql(strSql)
            If dtRet.Rows.Count > 0 Then
                For Each dtRow As DataRow In dtRet.Rows
                    If dtRow.Item("k_information_type") = INFORMATION_TYPE_STOP Then
                        '中止が存在する場合、一時保存からの本登録は不可とする
                        Return blnRet
                    End If
                Next
            End If

            blnRet = True
            Return blnRet
        Catch ex As Exception
            Call CLMsg.ShowEtarnal(Err.Number, Err.Description, SCREEN_ID_UC020302, SCREEN_NAME_UC020302, "ChkMeetingFinish")
            log.Fatal(ex.Message)
        Finally
            'DB接続終了
            clsMdb.Disconnect()
        End Try
    End Function

#End Region

#End Region

End Class
#End Region
